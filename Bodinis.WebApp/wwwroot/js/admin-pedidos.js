(function () {
    const products = document.querySelectorAll(".order-product-item");
    const search = document.getElementById("pedidoProductSearch");
    const lines = document.getElementById("pedidoLines");
    const hiddenFields = document.getElementById("pedidoHiddenFields");
    const totalEl = document.getElementById("pedidoTotal");
    const deliveryAddressField = document.getElementById("deliveryAddressField");
    const modeInputs = document.querySelectorAll("input[name='TipoPedido']");
    const cart = new Map();

    function money(value) {
        return `$ ${value}`;
    }

    function renderCart() {
        lines.innerHTML = "";
        hiddenFields.innerHTML = "";
        let total = 0;

        if (cart.size === 0) {
            lines.innerHTML = '<div class="order-empty">Agrega productos desde la lista.</div>';
            totalEl.textContent = money(0);
            return;
        }

        for (const item of cart.values()) {
            total += item.price * item.quantity;

            const row = document.createElement("div");
            row.className = "order-line";
            row.innerHTML = `
                <div>
                    <strong>${item.name}</strong>
                    <span>${money(item.price)} c/u</span>
                </div>
                <div class="order-qty">
                    <button type="button" data-action="decrease" data-id="${item.id}">-</button>
                    <b>${item.quantity}</b>
                    <button type="button" data-action="increase" data-id="${item.id}">+</button>
                </div>
                <strong>${money(item.price * item.quantity)}</strong>
            `;
            lines.appendChild(row);

            hiddenFields.insertAdjacentHTML("beforeend", `
                <input type="hidden" name="ProductoIds" value="${item.id}" />
                <input type="hidden" name="Cantidades" value="${item.quantity}" />
            `);
        }

        totalEl.textContent = money(total);
    }

    function addProduct(button) {
        const id = button.dataset.id;
        const stock = Number(button.dataset.stock || 0);
        const current = cart.get(id);

        if (current && current.quantity >= stock) {
            return;
        }

        cart.set(id, {
            id,
            name: button.dataset.name,
            price: Number(button.dataset.price || 0),
            stock,
            quantity: current ? current.quantity + 1 : 1
        });

        renderCart();
    }

    products.forEach((button) => {
        button.addEventListener("click", () => addProduct(button));
    });

    lines.addEventListener("click", (event) => {
        const button = event.target.closest("button[data-action]");
        if (!button) return;

        const item = cart.get(button.dataset.id);
        if (!item) return;

        if (button.dataset.action === "increase" && item.quantity < item.stock) {
            item.quantity += 1;
        }

        if (button.dataset.action === "decrease") {
            item.quantity -= 1;
            if (item.quantity <= 0) {
                cart.delete(item.id);
            }
        }

        renderCart();
    });

    search?.addEventListener("input", () => {
        const query = search.value.trim().toLowerCase();
        products.forEach((button) => {
            button.hidden = query.length > 0 && !button.dataset.search.includes(query);
        });
    });

    function syncDeliveryField() {
        const selected = document.querySelector("input[name='TipoPedido']:checked");
        deliveryAddressField.hidden = selected?.value !== "2";
    }

    modeInputs.forEach((input) => input.addEventListener("change", syncDeliveryField));
    syncDeliveryField();
    renderCart();
})();
