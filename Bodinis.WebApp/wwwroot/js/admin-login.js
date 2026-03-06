(function () {
  const form = document.getElementById("loginForm");
  const message = document.getElementById("loginMessage");

  if (!form) return;

  const token = localStorage.getItem("bod_token");
  if (token) {
    window.location.href = "/Admin/Index";
  }

  form.addEventListener("submit", async (e) => {
    e.preventDefault();
    message.textContent = "Ingresando...";
    message.className = "admin-message";

    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value;

    try {
      const res = await fetch("/admin-api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
      });

      const data = await res.json().catch(() => ({}));

      if (!res.ok) {
        message.textContent = data.error || "No se pudo iniciar sesion.";
        message.className = "admin-message is-error";
        return;
      }

      if (!data.token) {
        message.textContent = "No se recibio token de autenticacion.";
        message.className = "admin-message is-error";
        return;
      }

      localStorage.setItem("bod_token", data.token);
      window.location.href = "/Admin/Index";
    } catch {
      message.textContent = "Error de conexion con el servidor.";
      message.className = "admin-message is-error";
    }
  });
})();
