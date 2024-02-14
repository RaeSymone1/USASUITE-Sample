using OPM.SFS.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace OPM.SFS.Web.SharedCode
{
    public interface IServiceOwedService
    {
        ServiceOwed CalculateServiceOwedbyDateTime(string InstitutionAcademicSchedule, DateTime EnrolledSession, DateTime FundingEndDate);
        ServiceOwed CalculateServiceOwedbySeason(string InstitutionAcademicSchedule, string StartingSeason, int StartingYear, string EndingSeason, int EndingYear);
        ServiceOwed CalculateServiceOwedbyTerms(string InstitutionAcademicSchedule, int Terms);
    }
    public struct ServiceOwed
    {
        public string ex;
        public double ServiceTime;
        public ServiceOwed(string error, double Time)
        {
            ex = error;
            ServiceTime = Time;
        }
    }
    public class ServiceOwedService : IServiceOwedService
    {
        public ServiceOwedService()
        {

        }
        public ServiceOwed CalculateServiceOwedbyDateTime(string InstitutionAcademicSchedule, DateTime EnrolledSession, DateTime FundingEndDate)
        {
            double TimeDifference = (FundingEndDate - EnrolledSession).TotalDays / 365;
            double YearsOwed = Math.Round(TimeDifference, 1);
            double Terms;
            ServiceOwed service = new ServiceOwed(null, 0.0);
            switch (InstitutionAcademicSchedule)
            {
                case "Semester":
                    Terms = Math.Round(YearsOwed * 2, 0);
                    service = CalculatebySemester(Terms);
                    break;

                case "Trimester":
                    Terms = Math.Round(YearsOwed * 3, 0);
                    service = CalculatebyTrimester(Terms);
                    break;

                case "Quarter":
                    Terms = Math.Round(YearsOwed * 4, 0);
                    service = CalculatebyQuarter(Terms);
                    break;
                default:
                    service.ex = "Invalid Institution Academic Schedule";
                    break;
            }

            return service;
        }
        public ServiceOwed CalculateServiceOwedbySeason(string InstitutionAcademicSchedule, string StartingSeason, int StartingYear, string EndingSeason, int EndingYear)
        {
            int TimeDifferenceYears = EndingYear - StartingYear;
            int TimeDifferenceSeason = TranslateSeasonsToMonths(StartingSeason, EndingSeason);
            double MonthsOwed = TimeDifferenceYears * 12 + TimeDifferenceSeason;
            double Terms;
            ServiceOwed service = new ServiceOwed("", 0.0);
            switch (InstitutionAcademicSchedule)
            {
                case "Semester":
                    Terms = Math.Round(MonthsOwed / 6, 0);
                    if (Terms < 1)
                    {
                        service = CalculatebySemester(Terms);
                    }
                    else
                    {
                        service = CalculatebySemester(Terms + 1);
                    }
                    break;

                case "Trimester":
                    Terms = Math.Round(MonthsOwed / 4, 0);
                    if (Terms < 1)
                    {
                        service = CalculatebyTrimester(Terms);
                    }
                    else
                    {
                        service = CalculatebyTrimester(Terms + 1);
                    }

                    break;

                case "Quarter":
                    Terms = Math.Round(MonthsOwed / 3, 0);
                    if (Terms < 1)
                    {
                        service = CalculatebyQuarter(Terms);
                    }
                    else
                    {
                        service = CalculatebyQuarter(Terms + 1);
                    }
                    break;
                default:
                    service.ex = "Invalid Institution Academic Schedule";
                    break;
            }

            return service;
        }

        public ServiceOwed CalculateServiceOwedbyTerms(string InstitutionAcademicSchedule, int Terms)
        {
            ServiceOwed service = new ServiceOwed(null, 0.0);
            switch (InstitutionAcademicSchedule)
            {
                case "Semester":

                    service = CalculatebySemester(Terms);
                    break;

                case "Trimester":
                    service = CalculatebyTrimester(Terms);
                    break;

                case "Quarter":
                    service = CalculatebyQuarter(Terms);
                    break;
                default:
                    service.ex = "Invalid Institution Academic Schedule";
                    break;
            }

            return service;
        }
        private int TranslateSeasonsToMonths(params object[] parameters)
        {
            List<int> MonthTotal = new List<int>();
            foreach (string p in parameters)
            {
                switch (p)
                {
                    case "Winter":
                        MonthTotal.Add(0);
                        break;

                    case "Spring":
                        MonthTotal.Add(3);
                        break;

                    case "Summer":
                        MonthTotal.Add(6);
                        break;
                    case "Fall":
                        MonthTotal.Add(9);
                        break;
                }

            }
            int[] terms = MonthTotal.ToArray();
            int result = 0;
            if(terms.Length > 0)
            {
                result = terms[1] - terms[0];
            }           
            return result;
        }

        private ServiceOwed CalculatebyTrimester(double Terms)
        {
            ServiceOwed result = new ServiceOwed("", 0.0);
            if (Terms < 1.0) { result.ex = "Invalid Funding Date Entered, Please enter a Funding Date that is at least one year more than your funding start date"; }

            else if (Terms >= 1 & Terms <= 3) { result.ServiceTime = 1; }

            else if (Terms == 4 || Terms == 5) { result.ServiceTime = 1.5; }

            else if (Terms == 6) { result.ServiceTime = 2; }

            else if (Terms == 7 || Terms == 8) { result.ServiceTime = 2.5; }

            else if (Terms == 9) { result.ServiceTime = 3; }

            else { result.ex = "Maximum threshold for service owed was exceeded"; }

            return result;
        }

        private ServiceOwed CalculatebyQuarter(double Terms)
        {
            ServiceOwed result = new ServiceOwed("", 0.0);
            if (Terms < 1.0) { result.ex = "Invalid Funding Date Entered, Please enter a Funding Date that is at least one year more than your funding start date"; }

            else if (Terms >= 1 & Terms <= 4) { result.ServiceTime = 1; }

            else if (Terms >= 5 & Terms <= 7) { result.ServiceTime = 1.5; }

            else if (Terms == 8) { result.ServiceTime = 2; }

            else if (Terms >= 9 & Terms <= 11) { result.ServiceTime = 2.5; }

            else if (Terms == 12) { result.ServiceTime = 3; }

            else { result.ex = "Maximum threshold for service owed was exceeded"; }

            return result;
        }

        private ServiceOwed CalculatebySemester(double Terms)
        {
            ServiceOwed result = new ServiceOwed("", 0.0);
            if (Terms < 1.0) { result.ex = "Invalid Funding Date Entered, Please enter a Funding Date that is at least one year more than your funding start date"; }

            else if (Terms == 1) { result.ServiceTime = 1; }

            else if (Terms >= 2 & Terms <= 6) { result.ServiceTime = Terms / 2; }

            else { result.ex = "Maximum threshold for service owed was exceeded"; }

            return result;
        }


    }
}
