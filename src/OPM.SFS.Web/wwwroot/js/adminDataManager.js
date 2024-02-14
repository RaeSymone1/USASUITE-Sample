agGrid.LicenseManager.setLicenseKey("Using_this_AG_Grid_Enterprise_key_( AG-045805 )_in_excess_of_the_licence_granted_is_not_permitted___Please_report_misuse_to_( legal@ag-grid.com )___For_help_with_changing_this_key_please_contact_( info@ag-grid.com )___( U.S. Office of Personnel )_is_granted_a_( Single Application )_Developer_License_for_the_application_( ScholarshipForService )_only_for_( 3 )_Front-End_JavaScript_developers___All_Front-End_JavaScript_developers_working_on_( ScholarshipForService )_need_to_be_licensed___( ScholarshipForService )_has_been_granted_a_Deployment_License_Add-on_for_( 1 )_Production_Environment___This_key_works_with_AG_Grid_Enterprise_versions_released_before_( 27 July 2024 )____[v2]_MTcyMjAzNDgwMDAwMA==ebbeba7aa2b16b12e24d383cfa8613e8");
setDefaultOption();
document.addEventListener('DOMContentLoaded', function () {
	let defaultOption = sessionStorage.getItem("defaultOption");
	setPageLoad(defaultOption);
	document.getElementById("dataoptions").addEventListener("change", getSelectedDataOption);
	document.getElementById("btnDelete").addEventListener("click", deleteSelectedRow);
	document.getElementById("btnAddRow").addEventListener("click", showAndFormatModal);
	document.getElementById("btn-add-data").addEventListener("click", addNewDataGridOne);
	document.getElementById("btn-cancel-add").addEventListener("click", closeModal);
}, false);

var gridOptions = null;

function deleteSelectedRow() {
	const selectedRows = gridOptions.api.getSelectedRows();	
	if (confirm("Are you sure you want to delete " + selectedRows[0].name) == true) {
		gridOptions.api.applyTransaction({ remove: selectedRows });

		fetch('/admin/DataManagement?handler=DeleteOption', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(selectedRows[0]),
		})
			.then((response) => response.json())
			.catch((error) => {
				alert(error);
				alert('An error occured while saving your changes.');
			});
	} 
}

function getSelectedDataOption(event) {
	
	let selectElement = event.target;
	let value = selectElement.value;
	sessionStorage.setItem("defaultOption", value);


	const select = document.getElementById('dataoptions');
	let text = select.options[select.selectedIndex].text


	let mainLabel = document.getElementById("typeDisplay");
	mainLabel.innerHTML = text;

	//hide all grids!
	hideAllGrids();
	if (gridOptions !== null) {
		gridOptions.api.destroy();	
	}
	
	renderGridForOption(value);
}

function setPageLoad(dataOption) {
	hideAllGrids();
	renderGridForOption(dataOption);
	document.getElementById("dataoptions").value = dataOption;
	const select = document.getElementById('dataoptions');
	let text = select.options[select.selectedIndex].text
	let mainLabel = document.getElementById("typeDisplay");
	mainLabel.innerHTML = text;

}

function hideAllGrids() {
	const allGrids = document.getElementsByClassName("ag-theme-alpine");
	for (const element of allGrids) {
		element.style.display = "none";
	}
}

function renderGridForOption(selectedOption) {
	
	if (selectedOption == 'session' || selectedOption == 'year' || selectedOption == 'contract' || selectedOption == 'followupactiontype') {
		setGridFormatOne(selectedOption);		
		let x = document.getElementById(selectedOption);
		x.style.display = "block";
	}
	if (selectedOption == 'degree' || selectedOption == 'discipline' || selectedOption == 'extensiontype') {
		setGridFormatTwo(selectedOption);
		let x = document.getElementById(selectedOption);
		x.style.display = "block";
	}

	if (selectedOption == 'status') {
		setGridFormatStatus(selectedOption);
		let x = document.getElementById(selectedOption);
		x.style.display = "block";
	}
}

function setGridFormatOne(selectedOption) {	
	 gridOptions = {
		columnDefs: [
			{ colId: 'id', headerName: 'ID', field: "id" },
			{ colId: 'name', headerName: 'Name', field: "name", editable: true, cellEditorPopup: true, cellEditor: 'agTextCellEditor' },
			 { colId: 'datagroup', headerName: 'Group', field: "dataGroup", hide: true }
		],

		defaultColDef: { sortable: true, filter: true },

		rowSelection: 'multiple', // allow rows to be selected
		animateRows: true,
		onCellValueChanged: onCellValueChanged
	};

	const eGridDiv = document.getElementById(selectedOption);
	// new grid instance, passing in the hosting DIV and Grid Options
	new agGrid.Grid(eGridDiv, gridOptions);
	
	// Fetch data from server
	fetch("/admin/DataManagement?handler=DataOptions&option=" + selectedOption)
		.then(response => response.json())
		.then(data => {
			// load fetched data into grid
			gridOptions.api.setRowData(data);
		});
}

function setGridFormatTwo(selectedOption) {
	 gridOptions = {
		columnDefs: [
			{ colId: 'id', headerName: 'ID', field: "id" },
			{ colId: 'name', headerName: 'Name', field: "name", editable: true, cellEditorPopup: true, cellEditor: 'agTextCellEditor' },
			 { colId: 'code', headerName: 'Code', field: "code", editable: true, cellEditorPopup: true, cellEditor: 'agTextCellEditor' },
			 { colId: 'datagroup', headerName: 'Group', field: "dataGroup", hide: true }
		],

		defaultColDef: { sortable: true, filter: true },

		rowSelection: 'multiple',
		animateRows: true, 
		onCellValueChanged: onCellValueChanged
	};

	const eGridDiv = document.getElementById(selectedOption);
	// new grid instance, passing in the hosting DIV and Grid Options
	new agGrid.Grid(eGridDiv, gridOptions);

	// Fetch data from server
	fetch("/admin/DataManagement?handler=DataOptions&option=" + selectedOption)
		.then(response => response.json())
		.then(data => {
			// load fetched data into grid
			gridOptions.api.setRowData(data);
		});
}

function setGridFormatStatus(selectedOption) {
	 gridOptions = {
		columnDefs: [
			{ colId: 'id', headerName: 'ID', field: "id" },
			{ colId: 'option', headerName: 'Option', field: "option", editable: true, cellEditorPopup: true, cellEditor: 'agTextCellEditor' },
			{ colId: 'status', headerName: 'Status', field: "status", editable: true, cellEditorPopup: true, cellEditor: 'agTextCellEditor' },
			{ colId: 'phase', headerName: 'Phase', field: "phase", editable: true, cellEditorPopup: true, cellEditor: 'agTextCellEditor' },
			 { colId: 'placement', headerName: 'Placement Group', field: "placement", editable: true, cellEditorPopup: true, cellEditor: 'agTextCellEditor' },
			 { colId: 'datagroup', headerName: 'Group', field: "dataGroup", hide: true }
		],

		defaultColDef: { sortable: true, filter: true },

		rowSelection: 'multiple',
		animateRows: true, 
		onCellValueChanged: onCellValueChanged
	};
	
	const eGridDiv = document.getElementById(selectedOption);
	// new grid instance, passing in the hosting DIV and Grid Options
	new agGrid.Grid(eGridDiv, gridOptions);
	gridOptions.api.redrawRows();
	// Fetch data from server
	fetch("/admin/DataManagement?handler=DataOptions&option=" + selectedOption)
		.then(response => response.json())
		.then(data => {
			// load fetched data into grid
			gridOptions.api.setRowData(data);
		});
}

function hideGrid(name) {
	let x = document.getElementById("myGrid");
	x.style.display = "none";

}

function showGrid(name) {
	let x = document.getElementById("myGrid2");
	x.style.display = "block";
}

function onCellValueChanged(params) {
	fetch('/admin/DataManagement?handler=UpdateDataOption', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(params.data),
		})
		.then((response) => response.json())
		.catch((error) => {
			alert('An error occured while saving your changes.');
		});
	
}      

function showAndFormatModal() {
	
	const selectElement = document.getElementById('dataoptions');
	let value = selectElement.value;
	let text = selectElement.options[selectElement.selectedIndex].text
	const modelHeader = document.getElementById('modalHeader1');
	modelHeader.innerHTML = 'Add new row for ' + text;

	const errorMessage = document.getElementById('error-message');
	errorMessage.innerHTML = '';
	let formElements = document.getElementsByClassName("modal-fields");
	for (const element of formElements) {
		element.style.display = 'none';
	}
	let formLabels = document.getElementsByClassName("modal-label");
	for (const element of formLabels) {
		element.style.display = 'none';
	}
	
	if (value === "session" || value === "year" || value === "contract" || value === "followupactiontype" ) {
		const nameField = document.getElementById('txtNameValue');
		const nameLabel = document.getElementById('lblName');
		nameField.style.display = 'block';
		nameLabel.style.display = 'block';
	}

	if (value === "degree" || value === "discipline" || value === "extensiontype") {
		const nameField = document.getElementById('txtNameValue');
		const nameLabel = document.getElementById('lblName');
		const codeField = document.getElementById('txtCode');
		const codeLabel = document.getElementById('lblCode');
		nameField.style.display = 'block';
		codeField.style.display = 'block';
		nameLabel.style.display = 'block';
		codeLabel.style.display = 'block';
	}

	if (value === "status") {
		const optionField = document.getElementById('txtOption');
		const optionLabel = document.getElementById('lblOption');
		const statusField = document.getElementById('txtStatus');
		const statusLabel = document.getElementById('lblStatus');
		const phaseField = document.getElementById('txtPhase');
		const phaseLabel = document.getElementById('lblPhase');
		const placementField = document.getElementById('txtPlacement');
		const placementLabel = document.getElementById('lblPlacement');
		optionField.style.display = 'block';
		optionLabel.style.display = 'block';
		statusField.style.display = 'block';
		statusLabel.style.display = 'block';
		phaseField.style.display = 'block';
		phaseLabel.style.display = 'block';
		placementField.style.display = 'block';
		placementLabel.style.display = 'block'
	}

	$('#addDataForGridFormatOne').modal();
}

function addNewDataGridOne() {
	//txtNameValue
	const nameVal = document.getElementById('txtNameValue').value;
	const codeVal = document.getElementById('txtCode').value;
	const optionVal = document.getElementById('txtOption').value;
	const statusVal = document.getElementById('txtStatus').value;
	const phaseVal = document.getElementById('txtPhase').value;
	const placementVal = document.getElementById('txtPlacement').value;

	let txtStatus = document.getElementById('txtStatus').value;

	const selectElement = document.getElementById('dataoptions');
	let groupVal = selectElement.value;

	let request = {
		name: nameVal,
		option: optionVal,
		code: codeVal,
		status: statusVal,
		phase: phaseVal,
		placement: placementVal,
		dataGroup: groupVal
	}
	if (isFormValid(groupVal)) {
		fetch('/admin/DataManagement?handler=InsertDataOption', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(request),
		})
		.then((response) => response.json())
			.then((data) => {				
				$.modal.close();
				location.reload();
		})
		.catch((error) => {
			alert('An error occured while saving your changes.');
		});
	}
	else {
		const errorMessage = document.getElementById('error-message');
		errorMessage.innerHTML = 'All fields are required to save';
	}
}

function isFormValid(dataGroup) {
	if (dataGroup === 'session' || dataGroup === 'year' || dataGroup === 'contract' || dataGroup == 'followupactiontype') {
		const nameField = document.getElementById('txtNameValue').value;
		if (nameField.trim() == '')
			return false;
	}
	if (dataGroup === 'degree' || dataGroup === 'discipline' || dataGroup == 'extensiontype' ) {
		const nameField = document.getElementById('txtNameValue').value;
		const codeField = document.getElementById('txtCode').value;
		if (nameField.trim() == '' && codeField.trim() == '')
			return false;
	}
	if (dataGroup == 'status') {
		const optionField = document.getElementById('txtOption').value;
		const statusField = document.getElementById('txtStatus').value;
		const phaseField = document.getElementById('txtPhase').value;
		if (optionField.trim() == '' && statusField.trim() == '' && phaseField.trim() == '')
			return false;
	}
	return true;
}

function closeModal() {
	$.modal.close();
}

function setDefaultOption() {
	let defaultOption = sessionStorage.getItem("defaultOption");
	if (defaultOption === null) {
		sessionStorage.setItem("defaultOption", "discipline");
	}
}
