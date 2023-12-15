document.addEventListener('DOMContentLoaded', function () {
	const form = document.getElementById('contact_form');
	const submitButton = document.getElementById('btn_submit');

	const nameField = document.getElementById('name');
	const contactField = document.getElementById('contact');
	const emailField = document.getElementById('email');
	const passwordField = document.getElementById('password');
	const confirmPasswordField = document.getElementById('confirm-password');

	nameField.addEventListener('input', function (event) {
		if (/^[a-zA-Z\s]+$/.test(nameField.value)) {
			nameField.classList.remove('is-invalid');
			nameField.classList.add('is-valid');
		} else {
			nameField.classList.add('is-invalid');
			nameField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});

	
	emailField.addEventListener('input', function (event) {

		if (/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(emailField.value)) {
			emailField.classList.remove('is-invalid');
			emailField.classList.add('is-valid');
		} else {
			emailField.classList.add('is-invalid');
			emailField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});

	passwordField.addEventListener('input', function (event) {
		if (passwordField.value.length >= 8) {
			passwordField.classList.remove('is-invalid');
			passwordField.classList.add('is-valid');
		} else {
			passwordField.classList.add('is-invalid');
			passwordField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});

	confirmPasswordField.addEventListener('input', function (event) {
		if (passwordField.value === confirmPasswordField.value) {
			confirmPasswordField.classList.remove('is-invalid');
			confirmPasswordField.classList.add('is-valid');
			updateSubmitButtonState();
		} else {
			confirmPasswordField.classList.add('is-invalid');
			confirmPasswordField.classList.remove('is-valid');
		}
	});

	function updateSubmitButtonState() {
		if (form.checkValidity()) {
			submitButton.removeAttribute('disabled');
		} else {
			submitButton.setAttribute('disabled', 'disabled');
		}
	}


});
