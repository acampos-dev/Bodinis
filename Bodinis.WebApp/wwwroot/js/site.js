// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function () {
    const formatter = new Intl.NumberFormat("es-UY", {
        maximumFractionDigits: 0
    });

    function digitsOnly(value) {
        return String(value || "").replace(/\D/g, "");
    }

    function formatMoneyInput(input) {
        const digits = digitsOnly(input.value);
        input.value = digits.length ? formatter.format(Number(digits)) : "";
    }

    function normalizeMoneyInputs(form) {
        form.querySelectorAll("[data-money-input]").forEach((input) => {
            input.value = digitsOnly(input.value);
        });
    }

    document.querySelectorAll("[data-money-input]").forEach((input) => {
        formatMoneyInput(input);
        input.addEventListener("input", () => formatMoneyInput(input));
    });

    document.querySelectorAll("form").forEach((form) => {
        form.addEventListener("submit", (event) => {
            const message = form.dataset.confirmMessage;

            if (message) {
                const amountInputSelector = form.dataset.confirmAmountInput;
                const amountInput = amountInputSelector ? form.querySelector(amountInputSelector) : null;
                const amount = amountInput ? `\nMonto: $ ${amountInput.value || "0"}` : "";

                if (!window.confirm(`${message}${amount}`)) {
                    event.preventDefault();
                    return;
                }
            }

            normalizeMoneyInputs(form);
        });
    });
})();
