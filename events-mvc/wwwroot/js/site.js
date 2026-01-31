// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
	// Initialize tooltips and popovers if using Bootstrap
	initializeBootstrapComponents();

	// Add event listeners
	setupEventListeners();
});

// Initialize Bootstrap components
function initializeBootstrapComponents() {
	// Initialize tooltips
	const tooltipTriggerList = [].slice.call(
		document.querySelectorAll('[data-bs-toggle="tooltip"]'),
	);
	tooltipTriggerList.map(function (tooltipTriggerEl) {
		return new bootstrap.Tooltip(tooltipTriggerEl);
	});

	// Initialize popovers
	const popoverTriggerList = [].slice.call(
		document.querySelectorAll('[data-bs-toggle="popover"]'),
	);
	popoverTriggerList.map(function (popoverTriggerEl) {
		return new bootstrap.Popover(popoverTriggerEl);
	});
}

// Setup event listeners
function setupEventListeners() {
	// Delete confirmation
	const deleteButtons = document.querySelectorAll('[data-action="delete"]');
	deleteButtons.forEach((button) => {
		button.addEventListener("click", function (e) {
			if (!confirm("Jeste li sigurni da želite obrisati ovaj događaj?")) {
				e.preventDefault();
			}
		});
	});

	// Form validation
	const forms = document.querySelectorAll("form[novalidate]");
	forms.forEach((form) => {
		form.addEventListener("submit", function (e) {
			if (!form.checkValidity() === false) {
				e.preventDefault();
				e.stopPropagation();
			}
			form.classList.add("was-validated");
		});
	});
}

// Format date helper
function formatDate(date) {
	const options = { year: "numeric", month: "2-digit", day: "2-digit" };
	return new Date(date).toLocaleDateString("hr-HR", options);
}

// Format currency helper
function formatCurrency(amount) {
	return new Intl.NumberFormat("hr-HR", {
		style: "currency",
		currency: "HRK",
	}).format(amount);
}

// Show notification
function showNotification(message, type = "info") {
	const alertDiv = document.createElement("div");
	alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
	alertDiv.setAttribute("role", "alert");
	alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;

	const container = document.querySelector("main") || document.body;
	container.insertBefore(alertDiv, container.firstChild);

	// Auto-dismiss after 5 seconds
	setTimeout(() => {
		const bsAlert = new bootstrap.Alert(alertDiv);
		bsAlert.close();
	}, 5000);
}
