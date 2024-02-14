//Wiring up the download to excel button to the method.
agGrid.LicenseManager.setLicenseKey("Using_this_AG_Grid_Enterprise_key_( AG-045805 )_in_excess_of_the_licence_granted_is_not_permitted___Please_report_misuse_to_( legal@ag-grid.com )___For_help_with_changing_this_key_please_contact_( info@ag-grid.com )___( U.S. Office of Personnel )_is_granted_a_( Single Application )_Developer_License_for_the_application_( ScholarshipForService )_only_for_( 3 )_Front-End_JavaScript_developers___All_Front-End_JavaScript_developers_working_on_( ScholarshipForService )_need_to_be_licensed___( ScholarshipForService )_has_been_granted_a_Deployment_License_Add-on_for_( 1 )_Production_Environment___This_key_works_with_AG_Grid_Enterprise_versions_released_before_( 27 July 2024 )____[v2]_MTcyMjAzNDgwMDAwMA==ebbeba7aa2b16b12e24d383cfa8613e8");
var gridOptions = null;
sessionStorage.setItem("handleMultipleUpdates", "");
document.getElementById("btnExcelDownload").addEventListener("click", onBtExport);

document.addEventListener('DOMContentLoaded', function () {
    fetch("/admin/studentdashboard?handler=AllReferenceData")
        .then((response) => response.json())
        .then((data) => {
            initializeAgGrid(data);
        });

    // Fetch data from server
    fetch("/admin/studentdashboard?handler=AllStudents")
        .then(response => response.json())
        .then(data => {
            // load fetched data into grid
            gridOptions.api.setRowData(data);
        });
    document.getElementById("btn-add-student").addEventListener("click", showAndPopulateModal);
    var form = document.getElementById("newStudentForm");
    document.getElementById("ddlDegree").addEventListener("change", showSecondDegrees )
    form.addEventListener("submit", newStudent, true);
    document.getElementById("btn-add-funding").addEventListener("click", submitAdditionalFunding);
    document.getElementById("lnkGoback").addEventListener("click", dismissDuplicateModal);
}, false);

function deselect() {
    gridOptions.api.deselectAll()
}

function onBtExport() {
    gridOptions.api.exportDataAsExcel();
}

function initializeAgGrid(data) {
    gridOptions = {
        // each entry here represents one column
        columnDefs: [
            { colId: 'uid', headerName: 'UID', field: "id" },
            { headerName: 'Student Name', field: "studentname", cellRenderer: function (params) { return '<a href="StudentProfileEdit?sid=' + params.data.studentID + '" target="_blank">' + params.value + '</a>' } },
            { headerName: 'Contract', field: "contract", editable: params => params.data.programPhase != 'Closed', cellEditor: 'agSelectCellEditor', cellEditorParams: { values: data.contract, }, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Institution', field: "institution", cellRenderer: function (params) { return '<a href="InstitutionEdit?iid=' + params.data.institutionID + '" target="_blank">' + params.value + '</a>' } }, //editable: true, cellEditor: 'agSelectCellEditor', cellEditorParams: {  values: [], }, }, 
            { headerName: 'School Type', field: "schoolType" },
            { headerName: 'Enrolled Session', field: "enrolledSession" },
            { headerName: 'Enrolled Year', field: "enrolledYear" },
            { headerName: 'Registration Code', field: "registrationCode", hide: !data.isEnabledOnSite, suppressToolPanel: !data.isEnabledOnSite },
            { headerName: 'Registered', field: "registered" },
            { headerName: 'Funding End Session', field: "fundingEndSession" },
            { headerName: 'Funding End Year', field: "fundingEndYear" },
            { headerName: 'Graduation Date Month', field: "graduationDateMonth" },
            { headerName: 'Graduation Date Year', field: "graduationDateYear" },
            { headerName: 'Degree', field: "degree" }, 
            { headerName: 'Major', field: "major" }, 
            { headerName: 'Minor', field: "minor" }, 
            { headerName: 'Second Degree Major', field: "secondDegreeMajor" }, 
            { headerName: 'Second Degree Minor', field: "secondDegreeMinor" }, 
            { headerName: 'Academic Schedule', field: "academicSchedule" }, 
            { headerName: 'Total Quarter/ Semester/ Trimester Hours', field: "totalAcademicTerms",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: 'agTextCellEditor', cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Service Owed', field: "serviceOwed" },
            { headerName: 'IN Agency Type', field: "inAgencyType" },
            { headerName: 'IN HDQ Agency Name', field: "inhdqAgencyName" },
            { headerName: 'IN Sub Agency Name', field: "inSubAgencyName" },
            { headerName: 'Internship EOD', field: "ineod" },
            { headerName: 'Internship Reported', field: "inReported" },
            { headerName: 'Additional IN Agency Type', field: "addINAgencyType" },
            { headerName: 'Additional IN HDQ Agency Name', field: "addINHDQAgencyName" },
            { headerName: 'Additional IN Sub Agency Name', field: "addINSubAgencyName" },
            { headerName: 'Additional IN Reported on Website', field: "addINReportedWebsite" },
            { headerName: 'PG Agency Type', field: "pgAgencyType" },
            { headerName: 'PG HDQ Agency Name', field: "pghdqAgencyName" },
            { headerName: 'PG Sub Agency Name', field: "pgSubAgencyName" },
            { headerName: 'PG EOD', field: "pgeod" },
            { headerName: 'PG Extension Type', field: "extensionTypes",editable: params => params.data.programPhase != 'Closed', cellEditor: 'agSelectCellEditor', cellEditorParams: { values: data.extensionTypes }, cellClass: 'sfs-dashboard-edit' },
            //{ headerName: 'PG Employment Due Date', field: "pgEmploymentDueDate",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: 'agTextCellEditor', cellClass: 'sfs-dashboard-edit' },
            {
                headerName: 'PG Employment Due Date',
                field: "pgEmploymentDueDate",
               editable: params => params.data.programPhase != 'Closed',
                cellEditorPopup: true,
                cellEditor: DatePickerMMYYYY,
                cellClass: 'sfs-dashboard-edit'
            },
            { headerName: 'Post Graduate Reported', field: "pgReported" },
            { headerName: 'Days Between Graduation Date and PG EOD', field: "pgDaysBetween" },
            //{ headerName: 'Date Left Post Graduate Commitment Early', field: "dateLeftPGEarly",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: 'agTextCellEditor', cellClass: 'sfs-dashboard-edit' },
            {
                headerName: 'Date Left Post Graduate Commitment Early',
                field: "dateLeftPGEarly",
               editable: params => params.data.programPhase != 'Closed',
                cellEditorPopup: true,
                cellEditor: DatePicker,
                cellClass: 'sfs-dashboard-edit'
            },
            { headerName: 'Additional PG Agency Type', field: "addPGAgencyType" },
            { headerName: 'Additional PG HDQ Agency Name', field: "addPGHDQAgencyName" },
            { headerName: 'Additional PG Sub Agency Name', field: "addPGSubAgencyName" },
            { headerName: 'Additional PG Reported on Website', field: "addPGReportedWebsite" },
            {
                colId: 'statusOption',
                headerName: 'Status Option',
                field: "statusOption",
               editable: params => params.data.programPhase != 'Closed',
                cellEditor: 'agSelectCellEditor',
                cellEditorParams: {
                    values: Object.keys(data.statusOptionLookup),
                },
                cellClass: 'sfs-dashboard-edit'
            },
            {
                headerName: 'Status',
                field: "status",
               editable: params => params.data.programPhase != 'Closed',
                cellEditorSelector: (params) => {
                    if (data.statusOptionLookup[params.data.statusOption] !== undefined) {
                        return { component: 'agSelectCellEditor' };
                    } else {
                        return { component: 'agTextCellEditor' };
                    }
                },
                cellEditorParams: (params) => {
                    return {
                        values: data.statusOptionLookup[params.data.statusOption],
                    };
                },
                cellClass: 'sfs-dashboard-edit'
            },
            { colId: 'programPhase', headerName: 'Program Phase', field: "programPhase" },
            { headerName: 'Deferral Agreement Received', field: "deferralAgreementReceived" },
            { headerName: 'PG Verification 1 Due', field: "pgVerificationOneDue",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'PG Verification 1 Complete', field: "pgVerificationOneComplete",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'PG Verification 2 Due', field: "pgVerificationTwoDue",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'PG Verification 2 Complete', field: "pgVerificationTwoComplete",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'SOC Verification Due', field: "commitmentPhaseComplete",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'SOC Verification Complete', field: "socVerificationComplete",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Notes', field: "studentNote",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: 'agLargeTextCellEditor', cellEditorParams: { maxLength: 5000 }, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Last Update Received', field: "lastUpdateReceived",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Followup Date', field: "followupDate",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Followup Action Type', field: "followupActionType",editable: params => params.data.programPhase != 'Closed', cellEditor: 'agSelectCellEditor', cellEditorParams: { values: data.followUpType }, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Followup Action', field: "followupAction",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: 'agLargeTextCellEditor', cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Date Pending Release Req/Collection Info or Dropped Date', field: "releasePackageDueDate",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'Date Released/Collection Package or University Check Sent to NSF', field: "releasePackageSent",editable: params => params.data.programPhase != 'Closed', cellEditorPopup: true, cellEditor: DatePicker, cellClass: 'sfs-dashboard-edit' },
            {
                headerName: 'Amount', field: "amount",editable: params => params.data.programPhase != 'Closed',
                //valueFormatter: currencyFormatter, *Currency Formatting to be added back once amount has been implemented in data
                cellEditorPopup: true, cellEditor: 'agTextCellEditor', cellClass: 'sfs-dashboard-edit'
            },
            { headerName: 'Email Address', field: "emailAddress" },
            { headerName: 'Alternate Email', field: "altEmail" },
            { headerName: 'Citizenship', field: "citizenship",editable: params => params.data.programPhase != 'Closed', cellEditor: 'agSelectCellEditor', cellEditorParams: { values: data.citizenship }, cellClass: 'sfs-dashboard-edit' },
            { headerName: 'PG Placement Category', field: "pgPlacementCategory" },
            { headerName: 'Profile Status', field: "profileStatus", hide: true, suppressToolPanel: true },
            { headerName: 'FundingID', field: "fundingID", hide: true, suppressToolPanel: true }

        ],

        // default col def properties get applied to all columns
        defaultColDef: { sortable: true, filter: true },

        rowSelection: 'multiple', // allow rows to be selected
        animateRows: true, // have rows animate to new positions when sorted
        postSortRows: (params) => {
            const rowNodes = params.nodes;
            let nextInsertPos = 0;
            for (let i = 0; i < rowNodes.length; i++) {
                const profileStatus = rowNodes[i].data ? rowNodes[i].data.profileStatus : undefined;
                if (profileStatus === 'Not Registered') {
                    rowNodes.splice(nextInsertPos, 0, rowNodes.splice(i, 1)[0]);
                    nextInsertPos++;
                }
            }
        },

        onCellValueChanged: onCellValueChanged

    };

    // get div to host the grid
    const eGridDiv = document.getElementById("myGrid");
    // new grid instance, passing in the hosting DIV and Grid Options
    new agGrid.Grid(eGridDiv, gridOptions);

}

function showAndPopulateModal() {
    fetch("/admin/studentdashboard?handler=AllReferenceData")
        .then((response) => response.json())
        .then((data) => {
            let Institution = document.getElementById('ddlCollege');
            let Contract = document.getElementById('ddlContract');
            let es = document.getElementById('ddlEnrolledSession');
            let ey = document.getElementById('ddlEnrolledYear');
            let fes = document.getElementById('ddlFundingEndSession');
            let fey = document.getElementById('ddlFundingEndYear');
            let c = document.getElementById('ddlCitizenship');
            let d = document.getElementById('ddlDegree');
            let Major = document.getElementById('ddlMajor');
            let Minor = document.getElementById('ddlMinor');
            let SecondMajor = document.getElementById('ddlSecondMajor');
            let SecondMinor = document.getElementById('ddlSecondMinor');
            let SecondMajorLabel = document.getElementById('lblSecondMajor');
            let SecondMinorLabel = document.getElementById('lblSecondMinor');

             Institution.length = 1
             Contract.length = 1
             es.length = 1
             ey.length = 1 
             fes.length = 1 
             fey.length = 1
             c.length = 1
             d.length = 1 
             Major.length = 1 
             Minor.length = 1
             SecondMajor.length = 1 
             SecondMinor.length = 1 
             SecondMajorLabel.length = 1 
             SecondMinorLabel.length = 1 

            for (var i = 0; i < data.institutions.length; i++)
            {
                Institution.innerHTML = Institution.innerHTML + '<option value="' + i + '">' + data.institutions[i] + '</option>';
            }
            for (var i = 0; i < data.contract.length; i++) {
                Contract.innerHTML = Contract.innerHTML + '<option value="' + i + '">' + data.contract[i] + '</option>';
            }
            for (var i = 0; i < data.sessions.length; i++) {
                es.innerHTML = es.innerHTML + '<option value="' + i + '">' + data.sessions[i] + '</option>';
            }
            for (var i = 0; i < data.years.length; i++) {
                ey.innerHTML = ey.innerHTML + '<option value="' + i + '">' + data.years[i] + '</option>';
            }
            for (var i = 0; i < data.sessions.length; i++) {
                fes.innerHTML = fes.innerHTML + '<option value="' + i + '">' + data.sessions[i] + '</option>';
            }
            for (var i = 0; i < data.years.length; i++) {
                fey.innerHTML = fey.innerHTML + '<option value="' + i + '">' + data.years[i] + '</option>';
            }
            for (var i = 0; i < data.citizenship.length; i++) {
                c.innerHTML = c.innerHTML + '<option value="' + i + '">' + data.citizenship[i] + '</option>';
            }
            for (var i = 0; i < data.degrees.length; i++) {
                d.innerHTML = d.innerHTML + '<option value="' + i + '">' + data.degrees[i] + '</option>';
            }
            for (var i = 0; i < data.majors.length; i++) {
                Major.innerHTML = Major.innerHTML + '<option value="' + i + '">' + data.majors[i] + '</option>';
            }
            for (var i = 0; i < data.majors.length; i++) {
                Minor.innerHTML = Minor.innerHTML + '<option value="' + i + '">' + data.majors[i] + '</option>';
            }
            for (var i = 0; i < data.majors.length; i++) {
                SecondMajor.innerHTML = SecondMajor.innerHTML + '<option value="' + i + '">' + data.majors[i] + '</option>';
            }
            for (var i = 0; i < data.majors.length; i++) {
                SecondMinor.innerHTML = SecondMinor.innerHTML + '<option value="' + i + '">' + data.majors[i] + '</option>';
            }
            SecondMajorLabel.style.display = "none";
            SecondMajor.style.display = "none";
            SecondMinorLabel.style.display = "none";
            SecondMinor.style.display = "none";
        });

    $('#addstudentModal').modal();
}

var newStudent = function AddStudent(event) {
    event.preventDefault();
    const institutionVal = document.getElementById('ddlCollege').options[document.getElementById('ddlCollege').selectedIndex].text;
    const contractVal = document.getElementById('ddlContract').options[document.getElementById('ddlContract').selectedIndex].text;
    const enrolledSessionVal = document.getElementById('ddlEnrolledSession').options[document.getElementById('ddlEnrolledSession').selectedIndex].text;
    const enrolledYearVal = document.getElementById('ddlEnrolledYear').options[document.getElementById('ddlEnrolledYear').selectedIndex].text;
    const fundingendSessionVal = document.getElementById('ddlFundingEndSession').options[document.getElementById('ddlFundingEndSession').selectedIndex].text;
    const fundingendYearVal = document.getElementById('ddlFundingEndYear').options[document.getElementById('ddlFundingEndYear').selectedIndex].text;
    const expectedgraddateVal = document.getElementById('txtExpGradDate').value;
    const firstnameVal = document.getElementById('txtFirstName').value;
    const lastnameVal = document.getElementById('txtLastName').value;
    const emailVal = document.getElementById('txtEmail').value;
    const citizenshipVal = document.getElementById('ddlCitizenship').options[document.getElementById('ddlCitizenship').selectedIndex].text;
    const degreeVal = document.getElementById('ddlDegree').options[document.getElementById('ddlDegree').selectedIndex].text;
    const majorVal = document.getElementById('ddlMajor').options[document.getElementById('ddlMajor').selectedIndex].text;
    const minorVal = document.getElementById('ddlMinor').options[document.getElementById('ddlMinor').selectedIndex].text;
    const secondmajorVal = document.getElementById('ddlMajor').options[document.getElementById('ddlSecondMajor').selectedIndex].text;
    const secondminorVal = document.getElementById('ddlMinor').options[document.getElementById('ddlSecondMinor').selectedIndex].text;

    let request = {
        institution: institutionVal, 
        contract: contractVal ,
        enrolledSession: enrolledSessionVal, 
        enrolledYear: enrolledYearVal,
        fundingendSession: fundingendSessionVal, 
        fundingendYear: fundingendYearVal, 
        expectedgradDate: expectedgraddateVal, 
        firstname: firstnameVal, 
        lastname: lastnameVal ,
        email: emailVal ,
        citizenship: citizenshipVal ,
        degree: degreeVal ,
        major: majorVal ,
        minor: minorVal,
        secondmajor: secondmajorVal,
        secondminor: secondminorVal

    }
    

    if (isFormValid(request)) {
        fetch('/admin/StudentDashboard?handler=InsertNewStudent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.isSuccessful) {
                    $.modal.close();
                    location.reload();
                }
                else if (data.isDuplicate) {
                    sessionStorage.addStudentRequest = JSON.stringify(request);
                    $.modal.close();
                    $('#dupStudentModal').modal();
                }
                else {
                    alert(data.errorMessage);
                }
              
            })
            .catch((error) => {
                alert(error);
                alert('An error occured while saving your changes.');
            });
    }
    else {
        const errorMessage = document.getElementById('error-message');
        errorMessage.innerHTML = 'All fields are required to save';
    }
}

function submitAdditionalFunding() {
    var data = sessionStorage.addStudentRequest;
    if (!isEmpty(data)) {
        fetch('/admin/StudentDashboard?handler=InsertStudentFunding', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: data,
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.isSuccessful) {
                    $.modal.close();
                    location.reload();
                    sessionStorage.addStudentRequest = null;
                }              
                else {
                    alert(data.errorMessage);
                }
            })
            .catch((error) => {
                alert(error);
                alert('An error occured while saving your changes.');
            });
    }
}

function isFormValid(request) {
    var nameRegex = new RegExp("^[^><&]+$");
    var IsSecondMajorEnabled = document.getElementById('ddlSecondMajor').style.display;
    if (request.firstname == '' || request.lastname == '' || request.institution == 'Please select' || request.contract == 'Please select' || request.enrolledSession == 'Please select' || request.enrolledYear == 'Please select' || request.fundingendSession == 'Please select' || request.fundingendYear == 'Please select'
        || request.expectedgradDate == '' || request.email == '' || request.citizenship == 'Please select' || request.degree == 'Please select' || request.major == 'Please select' || (request.secondmajor == 'Please select' && IsSecondMajorEnabled != "none"))
    {
        return false 
    }
    if (!nameRegex.test(request.firstname) || !nameRegex.test(request.lastname)) {
        return false
    }
    return true;
}

function showSecondDegrees() {
    var type = document.getElementById('ddlDegree').options[document.getElementById('ddlDegree').selectedIndex].text;
    let SecondMajor = document.getElementById('ddlSecondMajor');
    let SecondMinor = document.getElementById('ddlSecondMinor');
    let SecondMajorLabel = document.getElementById('lblSecondMajor');
    let SecondMinorLabel = document.getElementById('lblSecondMinor');
    if (type.includes("/")) {
        SecondMajorLabel.style.display = "";
        SecondMajor.style.display = "";
        SecondMinorLabel.style.display = "";
        SecondMinor.style.display = "";
    }
    else {
        SecondMajorLabel.style.display = "none";
        SecondMajor.style.display = "none";
        SecondMinorLabel.style.display = "none";
        SecondMinor.style.display = "none";
    }
}

function onCellValueChanged(params) {
    var colId = params.column.getId();      
    if (colId != 'statusOption' && colId != 'programPhase' && colId != 'pgPlacementCategory' && colId != 'serviceOwed') {
        fetch('/admin/studentdashboard?handler=UpdateStudent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(params.data),
        })
            .then((response) => response.json())
            .then((data) => {
                sessionStorage.setItem("handleMultipleUpdates", colId);
                params.node.setDataValue('programPhase', data.programPhase);
                params.node.setDataValue('pgPlacementCategory', data.pgPlacementCategory);
                params.node.setDataValue('pgEmploymentDueDate', data.pgEmploymentDueDate);
                params.node.setDataValue('serviceOwed', data.serviceOwed);
                params.node.setDataValue('commitmentPhaseComplete', data.commitmentPhaseComplete);
                params.node.setDataValue('pgVerificationOneDue', data.pgVerificationOneDue);
                params.node.setDataValue('pgVerificationTwoDue', data.pgVerificationTwoDue)
            })
            .catch((error) => {
                console.error('Error:', error);
            });
        
    }   
}      

function isEmpty(value) {
    return (value == null || (typeof value === "string" && value.trim().length === 0));
}

function dismissDuplicateModal() {
    sessionStorage.addStudentRequest = null;
    location.reload();

}