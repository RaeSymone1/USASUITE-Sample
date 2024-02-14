using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using System;

namespace OPM.SFS.Web.Shared
{
    public interface IStudentRegistrationHelper
    {
        DateTime GetFirstDayOfQuarter(DateTime originalDate);
        DateTime GetLastDayOfQuarter(DateTime originalDate);
        DateTime AddQuarters(DateTime originalDate, int quarters);
        int GetQuarter(DateTime fromDate);
        bool ValidateCode(RegistrationCode _lookup);
    }

    public class StudentRegistrationHelper : IStudentRegistrationHelper
    {
        private readonly IUtilitiesService _utilities;
        
        public StudentRegistrationHelper(IUtilitiesService utilities)
        {
            _utilities= utilities;
        }

        public DateTime GetFirstDayOfQuarter(DateTime originalDate)
        {
            return AddQuarters(new DateTime(originalDate.Year, 1, 1), GetQuarter(originalDate) - 1);
        }

        public DateTime GetLastDayOfQuarter(DateTime originalDate)
        {
            return AddQuarters(new DateTime(originalDate.Year, 1, 1), GetQuarter(originalDate)).AddDays(-1);
        }

        public DateTime AddQuarters(DateTime originalDate, int quarters)
        {
            return originalDate.AddMonths(quarters * 3);
        }
        public int GetQuarter(DateTime fromDate)
        {
            int month = fromDate.Month - 1;
            int month2 = Math.Abs(month / 3) + 1;
            return month2;
        }

        public bool ValidateCode(RegistrationCode lookup)
        {
            var currentQuarterStart = GetFirstDayOfQuarter(_utilities.ConvertUtcToEastern(DateTime.UtcNow));
            if (lookup is not null && lookup.QuarterStartDate.Equals(currentQuarterStart))
                return true;
            return false;
        }
    }
}
