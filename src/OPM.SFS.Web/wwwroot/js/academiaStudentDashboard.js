//Wiring up the download to excel button to the method.
agGrid.LicenseManager.setLicenseKey("Using_this_AG_Grid_Enterprise_key_( AG-045805 )_in_excess_of_the_licence_granted_is_not_permitted___Please_report_misuse_to_( legal@ag-grid.com )___For_help_with_changing_this_key_please_contact_( info@ag-grid.com )___( U.S. Office of Personnel )_is_granted_a_( Single Application )_Developer_License_for_the_application_( ScholarshipForService )_only_for_( 3 )_Front-End_JavaScript_developers___All_Front-End_JavaScript_developers_working_on_( ScholarshipForService )_need_to_be_licensed___( ScholarshipForService )_has_been_granted_a_Deployment_License_Add-on_for_( 1 )_Production_Environment___This_key_works_with_AG_Grid_Enterprise_versions_released_before_( 27 July 2024 )____[v2]_MTcyMjAzNDgwMDAwMA==ebbeba7aa2b16b12e24d383cfa8613e8");
var gridOptions = null;
sessionStorage.setItem("handleMultipleUpdates", "");
document.getElementById("btnExcelDownload").addEventListener("click", onBtExport);

document.addEventListener('DOMContentLoaded', function () {
    fetch("/academia/studentdashboard?handler=AllReferenceData")
        .then((response) => response.json())
        .then((data) => {
            initializeAgGrid(data);
        });

    // Fetch data from server
    fetch("/academia/studentdashboard?handler=AllStudents")
        .then(response => response.json())
        .then(data => {
            // load fetched data into grid
            gridOptions.api.setRowData(data);
        });
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
            { headerName: 'Student Name', field: "studentname", cellRenderer: function (params) { return '<a href="StudentProfile?sid=' + params.data.studentID + '" target="_self">' + params.value + '</a>' } },
            { headerName: 'Contract', field: "contract" },
            { headerName: 'Institution', field: "institution" }, 
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
            { headerName: 'Total Quarter/ Semester/ Trimester Hours', field: "totalAcademicTerms"},
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
            { headerName: 'PG Extension Type', field: "extensionTypes"},
            {
                headerName: 'PG Employment Due Date',
                field: "pgEmploymentDueDate",              
            },
            { headerName: 'Post Graduate Reported', field: "pgReported" },
            { headerName: 'Days Between Graduation Date and PG EOD', field: "pgDaysBetween" },
            {
                headerName: 'Date Left Post Graduate Commitment Early',
                field: "dateLeftPGEarly",
            },
            { headerName: 'Additional PG Agency Type', field: "addPGAgencyType" },
            { headerName: 'Additional PG HDQ Agency Name', field: "addPGHDQAgencyName" },
            { headerName: 'Additional PG Sub Agency Name', field: "addPGSubAgencyName" },
            { headerName: 'Additional PG Reported on Website', field: "addPGReportedWebsite" },
            {
                colId: 'statusOption',
                headerName: 'Status Option',
                field: "statusOption",
            },
            {
                headerName: 'Status',
                field: "status",
            },
            { colId: 'programPhase', headerName: 'Program Phase', field: "programPhase" },
            { headerName: 'Deferral Agreement Received', field: "deferralAgreementReceived" },
            { headerName: 'PG Verification 1 Due', field: "pgVerificationOneDue"},
            { headerName: 'PG Verification 1 Complete', field: "pgVerificationOneComplete"},
            { headerName: 'PG Verification 2 Due', field: "pgVerificationTwoDue"},
            { headerName: 'PG Verification 2 Complete', field: "pgVerificationTwoComplete"},
            { headerName: 'SOC Verification Due', field: "commitmentPhaseComplete"},
            { headerName: 'SOC Verification Complete', field: "socVerificationComplete"},
            { headerName: 'Followup Date', field: "followupDate"},
            { headerName: 'Followup Action Type', field: "followupActionType"},
            { headerName: 'Followup Action', field: "followupAction"},
            { headerName: 'Date Pending Release Req/Collection Info or Dropped Date', field: "releasePackageDueDate"},
            { headerName: 'Date Released/Collection Package or University Check Sent to NSF', field: "releasePackageSent" },
            {headerName: 'Amount', field: "amount"},
            { headerName: 'Email Address', field: "emailAddress" },
            { headerName: 'Alternate Email', field: "altEmail" },
            { headerName: 'Citizenship', field: "citizenship"},
            { headerName: 'PG Placement Category', field: "pgPlacementCategory" },
            { headerName: 'Profile Status', field: "profileStatus", hide: true, suppressToolPanel: true },

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
    };

    // get div to host the grid
    const eGridDiv = document.getElementById("myGrid");
    // new grid instance, passing in the hosting DIV and Grid Options
    new agGrid.Grid(eGridDiv, gridOptions);

}

