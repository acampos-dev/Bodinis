(function () {
  const token = localStorage.getItem("bod_token");
  if (!token) {
    window.location.href = "/Admin/Login";
    return;
  }

  const ventasDiaTotal = document.getElementById("ventasDiaTotal");
  const ventasDiaCantidad = document.getElementById("ventasDiaCantidad");
  const ventasMesTotal = document.getElementById("ventasMesTotal");
  const ventasMesCantidad = document.getElementById("ventasMesCantidad");
  const productosWrap = document.getElementById("productosWrap");
  const pedidoMsg = document.getElementById("pedidoMsg");

  const usuarioIdInput = document.getElementById("usuarioId");
  const tipoPedidoSelect = document.getElementById("tipoPedido");
  const anioInput = document.getElementById("anioInput");
  const mesInput = document.getElementById("mesInput");

  const now = new Date();
  anioInput.value = String(now.getUTCFullYear());
  mesInput.value = String(now.getUTCMonth() + 1);

  const payload = parseJwt(token);
  const userId = payload?.nameid || payload?.sub || payload?.userid;
  if (userId && !Number.isNaN(Number(userId))) {
    usuarioIdInput.value = String(userId);
  } else {
    usuarioIdInput.value = "1";
  }

  document.getElementById("logoutBtn")?.addEventListener("click", () => {
    localStorage.removeItem("bod_token");
    window.location.href = "/Admin/Login";
  });

  document.getElementById("refreshResumenBtn")?.addEventListener("click", loadResumenDia);
  document.getElementById("buscarMesBtn")?.addEventListener("click", loadResumenMes);

  document.getElementById("pedidoForm")?.addEventListener("submit", async (e) => {
    e.preventDefault();
    pedidoMsg.textContent = "Creando pedido...";
    pedidoMsg.className = "admin-message";

    const usuarioId = Number(usuarioIdInput.value);
    const tipoPedido = tipoPedidoSelect.value;
    const items = collectItems();

    if (!usuarioId || usuarioId < 1) {
      setPedidoError("Usuario Id invalido.");
      return;
    }

    if (items.length === 0) {
      setPedidoError("Selecciona al menos un producto con cantidad mayor a 0.");
      return;
    }

    try {
      const res = await fetchWithAuth("/admin-api/pedidos", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ usuarioId, tipoPedido, items })
      });

      const data = await res.json().catch(() => ({}));

      if (!res.ok) {
        setPedidoError(data.error || "No se pudo crear el pedido.");
        return;
      }

      pedidoMsg.textContent = `Pedido creado correctamente (ID ${data.pedidoId || data.PedidoId || "nuevo"}).`;
      pedidoMsg.className = "admin-message is-ok";
      clearQuantities();
      await Promise.all([loadResumenDia(), loadResumenMes()]);
    } catch {
      setPedidoError("Error de conexion al crear pedido.");
    }
  });

  init();

  async function init() {
    await Promise.all([loadResumenDia(), loadResumenMes(), loadProductos()]);
  }

  async function loadResumenDia() {
    try {
      const res = await fetchWithAuth("/admin-api/ventas/resumen-dia");
      if (!res.ok) throw new Error();
      const data = await res.json();
      ventasDiaTotal.textContent = `$ ${formatNumber(data.totalVendido || 0)}`;
      ventasDiaCantidad.textContent = `${data.cantidadVentas || 0} ventas`;
    } catch {
      ventasDiaTotal.textContent = "$ 0";
      ventasDiaCantidad.textContent = "No disponible";
    }
  }

  async function loadResumenMes() {
    const anio = Number(anioInput.value);
    const mes = Number(mesInput.value);

    if (anio < 1 || mes < 1 || mes > 12) {
      ventasMesTotal.textContent = "$ 0";
      ventasMesCantidad.textContent = "Periodo invalido";
      return;
    }

    try {
      const res = await fetchWithAuth(`/admin-api/ventas/resumen-mes?anio=${anio}&mes=${mes}`);
      if (!res.ok) throw new Error();
      const data = await res.json();
      ventasMesTotal.textContent = `$ ${formatNumber(data.totalVendido || 0)}`;
      ventasMesCantidad.textContent = `${data.cantidadVentas || 0} ventas en ${data.mes}/${data.anio}`;
    } catch {
      ventasMesTotal.textContent = "$ 0";
      ventasMesCantidad.textContent = "No disponible";
    }
  }

  async function loadProductos() {
    try {
      const res = await fetchWithAuth("/admin-api/productos");
      if (!res.ok) throw new Error();
      const data = await res.json();

      const items = Array.isArray(data) ? data : [];
      productosWrap.innerHTML = items
        .map((p) => {
          const id = p.id ?? p.Id;
          const nombre = p.nombre ?? p.nombreProducto ?? p.Nombre ?? p.NombreProducto ?? `Producto ${id}`;
          const precio = p.precio ?? p.Precio ?? 0;

          return `
            <label class="product-row" for="prod-${id}">
              <span class="product-main">${escapeHtml(nombre)}</span>
              <span class="product-price">$ ${formatNumber(precio)}</span>
              <input id="prod-${id}" data-id="${id}" type="number" min="0" value="0" class="product-qty" />
            </label>`;
        })
        .join("");
    } catch {
      productosWrap.innerHTML = "<p class='admin-message is-error'>No se pudieron cargar productos.</p>";
    }
  }

  function collectItems() {
    const qtyInputs = Array.from(document.querySelectorAll(".product-qty"));
    return qtyInputs
      .map((input) => ({
        productoId: Number(input.dataset.id),
        cantidad: Number(input.value)
      }))
      .filter((x) => x.productoId > 0 && x.cantidad > 0);
  }

  function clearQuantities() {
    document.querySelectorAll(".product-qty").forEach((i) => {
      i.value = "0";
    });
  }

  function setPedidoError(text) {
    pedidoMsg.textContent = text;
    pedidoMsg.className = "admin-message is-error";
  }

  function parseJwt(jwt) {
    try {
      const parts = jwt.split(".");
      if (parts.length !== 3) return null;
      const payload = parts[1].replace(/-/g, "+").replace(/_/g, "/");
      const json = decodeURIComponent(atob(payload).split("").map((c) => `%${(`00${c.charCodeAt(0).toString(16)}`).slice(-2)}`).join(""));
      return JSON.parse(json);
    } catch {
      return null;
    }
  }

  async function fetchWithAuth(url, options = {}) {
    const headers = { ...(options.headers || {}), Authorization: `Bearer ${token}` };
    const res = await fetch(url, { ...options, headers });

    if (res.status === 401) {
      localStorage.removeItem("bod_token");
      window.location.href = "/Admin/Login";
      throw new Error("Unauthorized");
    }

    return res;
  }

  function formatNumber(n) {
    return new Intl.NumberFormat("es-UY").format(Number(n) || 0);
  }

  function escapeHtml(str) {
    return String(str)
      .replaceAll("&", "&amp;")
      .replaceAll("<", "&lt;")
      .replaceAll(">", "&gt;")
      .replaceAll('"', "&quot;")
      .replaceAll("'", "&#39;");
  }
})();
