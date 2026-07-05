(function () {
    const products = Array.from(document.querySelectorAll(".order-product-item"));
    const search = document.getElementById("pedidoProductSearch");
    const lines = document.getElementById("pedidoLines");
    const hiddenFields = document.getElementById("pedidoHiddenFields");
    const totalEl = document.getElementById("pedidoTotal");
    const deliveryAddressField = document.getElementById("deliveryAddressField");
    const deliveryAddressInput = document.getElementById("DireccionCliente");
    const paymentField = document.getElementById("orderPaymentField");
    const paymentSelect = document.getElementById("MetodoPagoId");
    const modeInputs = document.querySelectorAll("input[name='TipoPedido']");
    const categoryButtons = Array.from(document.querySelectorAll("[data-category-filter]"));
    const form = document.querySelector(".order-ticket");
    const printTicketButton = document.querySelector("[data-print-ticket]");
    const moneyFormatter = new Intl.NumberFormat("es-UY", {
        maximumFractionDigits: 0
    });
    const cart = new Map();
    let activeCategory = "all";

    printTicketButton?.addEventListener("click", () => window.print());

    if (!lines || !hiddenFields || !totalEl) {
        return;
    }

    function money(value) {
        return `$ ${moneyFormatter.format(value)}`;
    }

    function appendHidden(name, value) {
        const input = document.createElement("input");
        input.type = "hidden";
        input.name = name;
        input.value = value;
        hiddenFields.appendChild(input);
    }

    function createQuantityButton(action, id, label) {
        const button = document.createElement("button");
        button.type = "button";
        button.dataset.action = action;
        button.dataset.id = id;
        button.textContent = label;
        return button;
    }

    function renderCart() {
        lines.innerHTML = "";
        hiddenFields.innerHTML = "";
        let total = 0;

        if (cart.size === 0) {
            const empty = document.createElement("div");
            empty.className = "order-empty";
            empty.textContent = "Agrega productos desde la lista.";
            lines.appendChild(empty);
            totalEl.textContent = money(0);
            products.forEach((button) => button.classList.remove("selected", "limit-reached"));
            return;
        }

        for (const item of cart.values()) {
            total += item.price * item.quantity;

            const row = document.createElement("div");
            row.className = "order-line";

            const detail = document.createElement("div");
            const name = document.createElement("strong");
            name.textContent = item.name;
            const unit = document.createElement("span");
            unit.textContent = `${money(item.price)} c/u`;
            detail.append(name, unit);

            const quantity = document.createElement("div");
            quantity.className = "order-qty";
            const amount = document.createElement("b");
            amount.textContent = item.quantity;
            quantity.append(
                createQuantityButton("decrease", item.id, "-"),
                amount,
                createQuantityButton("increase", item.id, "+")
            );

            const subtotal = document.createElement("strong");
            subtotal.textContent = money(item.price * item.quantity);

            row.append(detail, quantity, subtotal);
            lines.appendChild(row);

            appendHidden("ProductoIds", item.id);
            appendHidden("Cantidades", item.quantity);
        }

        products.forEach((button) => {
            const item = cart.get(button.dataset.id);
            button.classList.toggle("selected", Boolean(item));
            button.classList.toggle("limit-reached", Boolean(item && item.quantity >= item.stock));
        });

        totalEl.textContent = money(total);
    }

    function addProduct(button) {
        const id = button.dataset.id;
        const stock = Number(button.dataset.stock || 0);
        const current = cart.get(id);

        if (stock <= 0 || (current && current.quantity >= stock)) {
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

    function applyFilters() {
        const query = (search?.value || "").trim().toLowerCase();

        products.forEach((button) => {
            const matchesSearch = query.length === 0 || button.dataset.search.includes(query);
            const matchesCategory = activeCategory === "all" || button.dataset.category === activeCategory;
            button.hidden = !matchesSearch || !matchesCategory;
        });
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

    search?.addEventListener("input", applyFilters);

    categoryButtons.forEach((button) => {
        button.addEventListener("click", () => {
            activeCategory = button.dataset.categoryFilter || "all";
            categoryButtons.forEach((item) => item.classList.toggle("active", item === button));
            applyFilters();
        });
    });

    function syncOrderFields() {
        const selected = document.querySelector("input[name='TipoPedido']:checked");
        const isDelivery = selected?.value === "2";

        if (deliveryAddressField) {
            deliveryAddressField.hidden = !isDelivery;
        }

        if (deliveryAddressInput) {
            deliveryAddressInput.required = isDelivery;
            if (!isDelivery) {
                deliveryAddressInput.value = "";
            }
        }

        if (paymentField) {
            paymentField.hidden = isDelivery;
        }

        if (paymentSelect) {
            paymentSelect.required = !isDelivery;
            if (isDelivery) {
                paymentSelect.value = "";
            }
        }
    }

    form?.addEventListener("submit", (event) => {
        if (cart.size === 0) {
            event.preventDefault();
            lines.classList.add("needs-attention");
            return;
        }

        lines.classList.remove("needs-attention");
    });

    modeInputs.forEach((input) => input.addEventListener("change", syncOrderFields));
    syncOrderFields();
    applyFilters();
    renderCart();
})();
