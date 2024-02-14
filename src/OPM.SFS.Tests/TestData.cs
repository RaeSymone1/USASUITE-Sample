using OPM.SFS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Tests
{
    public static class TestData
    {
        public static List<INCommitmentDTO> GetOneInternshipRecords()
        {
            List<INCommitmentDTO> internshipList_One_Record = new List<INCommitmentDTO>();
            internshipList_One_Record.Add(new INCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), INEOD = Convert.ToDateTime("12/13/2021") });
            return internshipList_One_Record;
        }

        public static List<INCommitmentDTO> GetTwoInternshipRecords()
        {
            List<INCommitmentDTO> internshipList_TWO_Records = new List<INCommitmentDTO>();
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), INEOD = Convert.ToDateTime("12/13/2021") });
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "US Navy", AgencyParentId = 5, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), INEOD = Convert.ToDateTime("10/13/2022") });

            return internshipList_TWO_Records;
        }

        public static List<INCommitmentDTO> GetThreeInternshipRecords()
        {
            List<INCommitmentDTO> internshipList_TWO_Records = new List<INCommitmentDTO>();
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), INEOD = Convert.ToDateTime("12/13/2021") });
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "US Navy", AgencyParentId = 5, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), INEOD = Convert.ToDateTime("10/13/2022") });
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "US Army", AgencyParentId = null, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), INEOD = Convert.ToDateTime("10/13/2022") });
            return internshipList_TWO_Records;
        }

        public static List<INCommitmentDTO> GetThreePostGradRecords()
        {
            List<INCommitmentDTO> internshipList_TWO_Records = new List<INCommitmentDTO>();
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), INEOD = Convert.ToDateTime("12/13/2021") });
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "US Navy", AgencyParentId = 5, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), INEOD = Convert.ToDateTime("10/13/2022") });
            internshipList_TWO_Records.Add(new INCommitmentDTO() { Agency = "US Army", AgencyParentId = null, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), INEOD = Convert.ToDateTime("10/13/2022") });
            return internshipList_TWO_Records;
        }

        public static List<INCommitmentDTO> GetFiveInternshipRecords()
        {
            List<INCommitmentDTO> internshipList_Five_Records = new List<INCommitmentDTO>();
            internshipList_Five_Records.Add(new INCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), INEOD = Convert.ToDateTime("12/13/2021") });
            internshipList_Five_Records.Add(new INCommitmentDTO() { Agency = "US Navy", AgencyParentId = 5, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), INEOD = Convert.ToDateTime("10/13/2021") });
            internshipList_Five_Records.Add(new INCommitmentDTO() { Agency = "US Army", AgencyParentId = 5, AgencyType = "Federal", CommitmentId = 109, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2020"), StartDate = Convert.ToDateTime("10/13/2020"), INEOD = Convert.ToDateTime("10/13/2020") });
            internshipList_Five_Records.Add(new INCommitmentDTO() { Agency = "Treasury Department", AgencyParentId = 52, AgencyType = "Federal", CommitmentId = 135, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("9/05/2023"), StartDate = Convert.ToDateTime("10/13/2023"), INEOD = Convert.ToDateTime("10/13/2023") });
            internshipList_Five_Records.Add(new INCommitmentDTO() { Agency = "National Census", AgencyParentId = null, AgencyType = "Federal", CommitmentId = 125, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2019"), StartDate = Convert.ToDateTime("10/13/2019"), INEOD = Convert.ToDateTime("10/13/2019") });

            return internshipList_Five_Records;
        }

        public static List<PGCommitmentDTO> GetOnePGCommitmentRecord()
        {
            List<PGCommitmentDTO> postGraduateList_One_Record = new List<PGCommitmentDTO>();
            postGraduateList_One_Record.Add(new PGCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), PGEOD = Convert.ToDateTime("12/13/2021") });

            return postGraduateList_One_Record;
        }

        public static List<PGCommitmentDTO> GetTwoPGCommitmentRecord()
        {
            List<PGCommitmentDTO> postGraduateList_TWO_Records = new List<PGCommitmentDTO>();
            postGraduateList_TWO_Records.Add(new PGCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), PGEOD = Convert.ToDateTime("12/13/2021") });
            postGraduateList_TWO_Records.Add(new PGCommitmentDTO() { Agency = "US Navy", AgencyParentId = 5, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), PGEOD = Convert.ToDateTime("10/13/2022") });

            return postGraduateList_TWO_Records;
        }
        public static List<PGCommitmentDTO> GetFivePGCommitmentRecord()
        {
            List<PGCommitmentDTO> postGraudateList_Five_Records = new List<PGCommitmentDTO>();
            postGraudateList_Five_Records.Add(new PGCommitmentDTO() { Agency = "LOS Laboratory Institute", AgencyParentId = null, AgencyType = "FFRDC", CommitmentId = 100, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("11/05/2021"), StartDate = Convert.ToDateTime("12/13/2021"), PGEOD = Convert.ToDateTime("12/13/2021") });
            postGraudateList_Five_Records.Add(new PGCommitmentDTO() { Agency = "US Navy", AgencyParentId = 5, AgencyType = "Federal", CommitmentId = 105, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2022"), StartDate = Convert.ToDateTime("10/13/2022"), PGEOD = Convert.ToDateTime("10/13/2021") });
            postGraudateList_Five_Records.Add(new PGCommitmentDTO() { Agency = "Federal Bureau Investigation", AgencyParentId = 98, AgencyType = "Federal", CommitmentId = 109, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2020"), StartDate = Convert.ToDateTime("10/13/2020"), PGEOD = Convert.ToDateTime("10/13/2020") });
            postGraudateList_Five_Records.Add(new PGCommitmentDTO() { Agency = "Treasury Department", AgencyParentId = 52, AgencyType = "Federal", CommitmentId = 135, CommitmentType = "Internship", DateApproved = Convert.ToDateTime("9/05/2023"), StartDate = Convert.ToDateTime("10/13/2023"), PGEOD = Convert.ToDateTime("10/13/2023") });
            postGraudateList_Five_Records.Add(new PGCommitmentDTO() { Agency = "National Census", AgencyParentId = 66, AgencyType = "Federal", CommitmentId = 125, CommitmentType = "Tentative Internship", DateApproved = Convert.ToDateTime("9/05/2019"), StartDate = Convert.ToDateTime("10/13/2019"), PGEOD = Convert.ToDateTime("10/13/2019") });

            return postGraudateList_Five_Records;
        }


        public static List<AgencyReferenceDTO> GetParentAgencies()
        {
            List<AgencyReferenceDTO> parentAgecies = new List<AgencyReferenceDTO>();
            parentAgecies.Add(new AgencyReferenceDTO() { AgencyId = 5, Name = "US Arm Forces", AgencyTypeId = 1, ParentAgencyId = 0 });
            parentAgecies.Add(new AgencyReferenceDTO() { AgencyId = 52, Name = "US Federal Reserve", AgencyTypeId = 1, ParentAgencyId = 0 });
            parentAgecies.Add(new AgencyReferenceDTO() { AgencyId = 98, Name = "US Justice Department", AgencyTypeId = 1, ParentAgencyId = 0 });
            parentAgecies.Add(new AgencyReferenceDTO() { AgencyId = 66, Name = "US Congress", AgencyTypeId = 1, ParentAgencyId = 0 });

            return parentAgecies;
        }

        public static string ExpectedResult_For_5_Commitments()
        {
            return $"{DateTime.UtcNow.ToShortDateString()} Third IN with US Arm Forces / US Army with start date of 10/13/2020; " +
                $"{DateTime.UtcNow.ToShortDateString()} Fourth IN with National Census with start date of 10/13/2019; " +
                $"{DateTime.UtcNow.ToShortDateString()} Fifth IN with US Federal Reserve / Treasury Department with start date of 10/13/2023; " +
                $"{DateTime.UtcNow.ToShortDateString()} Third PG with US Justice Department / Federal Bureau Investigation with start date of 10/13/2020; " +
                $"{DateTime.UtcNow.ToShortDateString()} Fourth PG with US Congress / National Census with start date of 10/13/2019; {DateTime.Now.ToShortDateString()} Fifth PG with US Federal Reserve / Treasury Department with start date of 10/13/2023;";

        }

        public static string ExpectedResult_For_3_Internships()
        {
            return $"{DateTime.UtcNow.ToShortDateString()} Third IN with US Army with start date of 10/13/2022;";
        }

        public static List<CommitmentReportedDTO> GetInternshipData()
        {
            List<CommitmentReportedDTO> data = new List<CommitmentReportedDTO>();
            data.Add(new CommitmentReportedDTO() { StudentID = 1, AgencyName = "Agency One", AgencyType = "DOE Labs", CommitmentID = 1, 
                DateApproved = Convert.ToDateTime("1/1/2023"), ParentAgencyID = null, ServiceOwed = "2", StartDate = Convert.ToDateTime("1/1/2023"), Status = "", SubAgencyName = "N/A" });
            data.Add(new CommitmentReportedDTO() { StudentID = 1, AgencyName = "Agency Two", AgencyType = "DOE Labs", CommitmentID = 2, 
                DateApproved = Convert.ToDateTime("2/1/2023"), ParentAgencyID = null, ServiceOwed = "2", StartDate = Convert.ToDateTime("2/1/2023"), Status = "", SubAgencyName = "N/A" });
            data.Add(new CommitmentReportedDTO() { StudentID = 1, AgencyName = "Agency Three", AgencyType = "DOE Labs", CommitmentID = 3, 
                DateApproved = Convert.ToDateTime("3/1/2023"), ParentAgencyID = null, ServiceOwed = "2", StartDate = Convert.ToDateTime("3/1/2023"), Status = "", SubAgencyName = "N/A" });
            return data;
        }

        public static List<CommitmentVerificationDetailsDTO> GetFiveInternshipRecords_For_EVF()
        {
            List<CommitmentVerificationDetailsDTO> internshipList_Five_Records = new List<CommitmentVerificationDetailsDTO>
            {
                new CommitmentVerificationDetailsDTO() { ID = 1, Agency = "Agency One", JobTitle = "Test 1", StartDate = DateTime.Parse("01/01/2024"), Type = "Internship", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
                new CommitmentVerificationDetailsDTO() { ID = 2, Agency = "Agency Two", JobTitle = "Test 2", StartDate = DateTime.Parse("02/02/2024"), Type = "Internship", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
                new CommitmentVerificationDetailsDTO() { ID = 3, Agency = "Agency Three", JobTitle = "Test 3", StartDate = DateTime.Parse("03/03/2024"), Type = "Internship", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
                new CommitmentVerificationDetailsDTO() { ID = 4, Agency = "Agency Four", JobTitle = "Test 4", StartDate = DateTime.Parse("04/04/2024"), Type = "Internship", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
                new CommitmentVerificationDetailsDTO() { ID = 5, Agency = "Agency Five", JobTitle = "Test 5", StartDate = DateTime.Parse("05/05/2024"), Type = "Internship", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" }
            };

            return internshipList_Five_Records;
        }

        internal static List<CommitmentVerificationDetailsDTO> GetTwoInternshipTwoPGRecords_For_EVF()
        {
            List<CommitmentVerificationDetailsDTO> Commitment_Records = new List<CommitmentVerificationDetailsDTO>
            {
                new CommitmentVerificationDetailsDTO() { ID = 1, Agency = "Agency One", JobTitle = "Test 1", StartDate = DateTime.Parse("01/01/2024"), Type = "Internship", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
                new CommitmentVerificationDetailsDTO() { ID = 2, Agency = "Agency Two", JobTitle = "Test 2", StartDate = DateTime.Parse("02/02/2024"), Type = "Postgraduate", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
                new CommitmentVerificationDetailsDTO() { ID = 3, Agency = "Agency Three", JobTitle = "Test 3", StartDate = DateTime.Parse("03/03/2024"), Type = "Postgraduate", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
                new CommitmentVerificationDetailsDTO() { ID = 4, Agency = "Agency Four", JobTitle = "Test 4", StartDate = DateTime.Parse("04/04/2024"), Type = "Internship", Status = "Approved", EVFStatus = "Incomplete", EVFDateSubmitted = "12/25/2023" },
            };
            return Commitment_Records;
        }
    }
}
