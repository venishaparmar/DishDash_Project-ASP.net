document.addEventListener('DOMContentLoaded', function () {
	const form = document.getElementById('contact_form');
	const submitButton = document.getElementById('btn_submit');

	const nameField = document.getElementById('name');
	const contactField = document.getElementById('contact');
	const pincodeField = document.getElementById('pincode');
	const cityField = document.getElementById('city');
	const landmarkField = document.getElementById('landmark');
	const areaDetailsField = document.getElementById('areaDetails');
	const houseDetailsField = document.getElementById('houseDetails');
	const orderForMyself = document.getElementById('orderForMyself');
	const orderForSomeoneElse = document.getElementById('orderForSomeoneElse');

	orderForMyself.addEventListener('change', function () {
		updateSubmitButtonState();
	});

	orderForSomeoneElse.addEventListener('change', function () {
		updateSubmitButtonState();
	});

	nameField.addEventListener('input', function (event) {
		if (!orderForMyself.checked) {
			if (/^[a-zA-Z\s]+$/.test(nameField.value)) {
				nameField.classList.remove('is-invalid');
				nameField.classList.add('is-valid');
			} else {
				nameField.classList.add('is-invalid');
				nameField.classList.remove('is-valid');
			}
		}
		updateSubmitButtonState();
	});

	contactField.addEventListener('input', function (event) {
		if (/^\d{10,}$/.test(contactField.value)) {
			contactField.classList.remove('is-invalid');
			contactField.classList.add('is-valid');
		} else {
			contactField.classList.add('is-invalid');
			contactField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});
	
	houseDetailsField.addEventListener('input', function (event) {
		if (/^[a-zA-Z0-9\s]+$/.test(houseDetailsField.value)) {
			houseDetailsField.classList.remove('is-invalid');
			houseDetailsField.classList.add('is-valid');
		} else {
			houseDetailsField.classList.add('is-invalid');
			houseDetailsField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});

	areaDetailsField.addEventListener('input', function (event) {
		
		if (/^[a-zA-Z0-9\s]+$/.test(areaDetailsField.value)) {
			areaDetailsField.classList.remove('is-invalid');
			areaDetailsField.classList.add('is-valid');
		} else {
			areaDetailsField.classList.add('is-invalid');
			areaDetailsField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});

	landmarkField.addEventListener('input', function (event) {
		if (landmarkField.value === '' || /^[a-zA-Z0-9\s]+$/.test(landmarkField.value)) {
			landmarkField.classList.remove('is-invalid');
			landmarkField.classList.add('is-valid');
		} else {
			landmarkField.classList.add('is-invalid');
			landmarkField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});

	pincodeField.addEventListener('input', function (event) {
		if (/^\d{6}$/.test(pincodeField.value)) {
			pincodeField.classList.remove('is-invalid');
			pincodeField.classList.add('is-valid');
		} else {
			pincodeField.classList.add('is-invalid');
			pincodeField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});

	cityField.addEventListener('input', function (event) {
		if (/^[a-zA-Z\s]+$/.test(cityField.value)) {
			cityField.classList.remove('is-invalid');
			cityField.classList.add('is-valid');
		} else {
			cityField.classList.add('is-invalid');
			cityField.classList.remove('is-valid');
		}

		updateSubmitButtonState();
	});
	

	function updateSubmitButtonState() {
		if (form.checkValidity() || (orderForMyself.checked && nameField.value === '')) {
			submitButton.removeAttribute('disabled');
		} else if (orderForSomeoneElse.checked && nameField.value === '') {
			submitButton.setAttribute('disabled', 'disabled');
		} else {
			submitButton.setAttribute('disabled', 'disabled');
		}
	}


});
