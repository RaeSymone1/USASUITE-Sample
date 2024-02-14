
using LazyCache;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public interface ICacheHelper
    {
        Task<List<CacheHelper.AgencyCacheDTO>> GetAgenciesAsync();
        Task<List<Agency>> GetAgenciesWithDisabledAsync();
        Task<List<AgencyType>> GetAgencyTypesAsync();
        Task<List<CommitmentType>> GetCommitmentTypeAsync();
        Task<List<JobSearchType>> GetJobSearchTypeAsync();
        Task<List<SalaryType>> GetPayRateListAsync();
        Task<List<State>> GetStatesAsync();
        Task<List<CommitmentStatus>> GetCommitmentStatusAsync();
        Task<List<DocumentType>> GetDocumentTypeAsync();
        Task<List<SecurityCertification>> GetSecurityCertificationAsync();
        Task<List<SchoolType>> GetSchoolTypesAsync();
        Task<List<Degree>> GetDegreesAsync();
        Task<List<Gender>> GetGenderAsync();
        Task<List<Ethnicity>> GetEthnicityAsync();
        Task<List<Race>> GetRacesAsync();
        Task<List<Institution>> GetInstitutionsAsync();
        Task<List<Discipline>> GetDisciplineAsync();
        Task<List<ProfileStatus>> GetProfileStatusAsync();
        Task<List<AcademiaUserRole>> GetAcademiaUserRoleAsync();
        Task<List<AgencyUserRole>> GetAgencyUserRolesAsync();
        Task<List<GlobalConfiguration>> GetGlobalConfigurationsAsync();
        Task<List<JobActivityStatus>> GetJobActivityStatusAsync();
        Task<List<InstitutionType>> GetInsitutionTypeAsync();
        Task<List<AcademicSchedule>> GetAcademicSchedulesAsync();
        Task<List<InstitutionContactType>> GetInstituionContactTypeAsync();
        Task<List<CommitmentApprovalWorkflow>> GetCommitmentApprovalWorkflowsAsync();
        Task<List<StatusOption>> GetStatusOptionsAsync();
        Task<List<Citizenship>> GetCitizenship();

        Task<List<ExtensionType>> GetExtensionTypeAsync();
		Task<List<SessionList>> GetSessionsAsync();
		Task<List<ProgramYear>> GetProgramYears();
        Task<List<FollowUpTypeOption>> GetFollowUpTypeOptions();
        
	}

    public class CacheHelper : ICacheHelper
    {
        private readonly IAppCache _cache = new CachingService();
        private readonly ScholarshipForServiceContext _db;

        public CacheHelper(IAppCache cache, ScholarshipForServiceContext db)
        {
            _cache = cache;
            _db = db;
        }

        public async Task<List<AgencyCacheDTO>> GetAgenciesAsync()
        {
            var data = await _cache.GetOrAddAsync("Agency", LoadAgencies, DateTimeOffset.Now.AddMinutes(1));
            return data;
        }

        public async Task<List<Agency>> GetAgenciesWithDisabledAsync()
        {
            var data = await _cache.GetOrAddAsync("Agency", LoadAgenciesAll, DateTimeOffset.Now.AddMinutes(1));
            return data;
        }

        public async Task<List<AgencyType>> GetAgencyTypesAsync()
        {
            var data = await _cache.GetOrAddAsync("AgencyType", LoadAgencyTypes, DateTimeOffset.Now.AddMinutes(30));
            return data;

        }

        public async Task<List<CommitmentType>> GetCommitmentTypeAsync()
        {
            var data = await _cache.GetOrAddAsync("CommitmentTypes", LoadCommitmentTypes, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<SalaryType>> GetPayRateListAsync()
        {

            var data = await _cache.GetOrAddAsync("PayRates", LoadPayRates, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<JobSearchType>> GetJobSearchTypeAsync()
        {

            var data = await _cache.GetOrAddAsync("JobSearchType", LoadJobSearchTypes, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<State>> GetStatesAsync()
        {
            var data = await _cache.GetOrAddAsync("States", LoadStates, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<CommitmentStatus>> GetCommitmentStatusAsync()
        {

            var data = await _cache.GetOrAddAsync("CommitmentStatus", LoadCommitmentStatus, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<DocumentType>> GetDocumentTypeAsync()
        {
            var data = await _cache.GetOrAddAsync("DocumentTypes", LoadDocumentTypes, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<SecurityCertification>> GetSecurityCertificationAsync()
        {
            var data = await _cache.GetOrAddAsync("SecurityCertificates", LoadSecurityCertification, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<SchoolType>> GetSchoolTypesAsync()
        {
            var data = await _cache.GetOrAddAsync("SchoolTypes", LoadSchoolTypes, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<Degree>> GetDegreesAsync()
        {
            var data = await _cache.GetOrAddAsync("Degrees", LoadDegrees, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        //public async Task<List<DegreeMinor>> GetDegreeMinorsAsync()
        //{
        //    var data = await _cache.GetOrAddAsync("DegreeMinors", LoadDegreesMinors, DateTimeOffset.Now.AddMinutes(30));
        //    return data;
        //}

        public async Task<List<Ethnicity>> GetEthnicityAsync()
        {
            var data = await _cache.GetOrAddAsync("Ethnicities", LoadEthnicities, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<Gender>> GetGenderAsync()
        {
            var data = await _cache.GetOrAddAsync("Genders", LoadGenders, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<Race>> GetRacesAsync()
        {
            var data = await _cache.GetOrAddAsync("Races", LoadRaces, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<Institution>> GetInstitutionsAsync()
        {
            var data = await _cache.GetOrAddAsync("Institutions", LoadInstitutions, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<Discipline>> GetDisciplineAsync()
        {
            var data = await _cache.GetOrAddAsync("Discipline", LoadDisciplines, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<ProfileStatus>> GetProfileStatusAsync()
        {
            var data = await _cache.GetOrAddAsync("ProfileStatus", LoadProfileStatus, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<AcademiaUserRole>> GetAcademiaUserRoleAsync()
        {
            var data = await _cache.GetOrAddAsync("AcademiaUserRole", LoadAcademiaUserRole, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<AgencyUserRole>> GetAgencyUserRolesAsync()
        {
            var data = await _cache.GetOrAddAsync("AgencyUserRoles", LoadAgencyUserRole, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<GlobalConfiguration>> GetGlobalConfigurationsAsync()
        {
            var data = await _cache.GetOrAddAsync("GlobalConfiguration", LoadGlobalConfiguration, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<JobActivityStatus>> GetJobActivityStatusAsync()
        {
            var data = await _cache.GetOrAddAsync("GlobalConfiguration", LoadJobActivityStatus, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<InstitutionType>> GetInsitutionTypeAsync()
        {
            var data = await _cache.GetOrAddAsync("InstitutionTypes", LoadInstitutionTypes, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<AcademicSchedule>> GetAcademicSchedulesAsync()
        {
            var data = await _cache.GetOrAddAsync("AcademicSchedules", LoadAcademicSchedules, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<InstitutionContactType>> GetInstituionContactTypeAsync()
        {
            var data = await _cache.GetOrAddAsync("InstitutionContactTypes", LoadInstitutionContactTypes, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<CommitmentApprovalWorkflow>> GetCommitmentApprovalWorkflowsAsync()
        {
            var data = await _cache.GetOrAddAsync("CommitmentApprovalWorkflows", LoadCommitmentApprovalWorkflows, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }
        public async Task<List<StatusOption>> GetStatusOptionsAsync()
        {
            var data = await _cache.GetOrAddAsync("StatusOption", LoadStatusOption, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }
        public async Task<List<Citizenship>> GetCitizenship()
        {
            var data = await _cache.GetOrAddAsync("Citizenship", LoadCitizenship, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        public async Task<List<FollowUpTypeOption>> GetFollowUpTypeOptions()
        {
            var data = await _cache.GetOrAddAsync("FollowUpTypeOption", LoadFollowUpTypeOptions, DateTimeOffset.Now.AddMinutes(30));
            return data;
        }

        private async Task<List<AgencyCacheDTO>> LoadAgencies()
        {
            return await _db.Agencies.Where(m => m.IsDisabled == false).Select(x => new AgencyCacheDTO()
            {
                AgencyId = x.AgencyId,
                Name = x.Name,
                ParentAgencyId = x.ParentAgencyId,
                StateID = x.Address.StateId,
                AgencyTypeId = x.AgencyTypeId,
                Workflow = x.CommitmentApprovalWorkflow.Code

            }).ToListAsync();
        }

        private async Task<List<Agency>> LoadAgenciesAll()
        {
            return await _db.Agencies.AsNoTracking().ToListAsync();
        }

        private async Task<List<AgencyType>> LoadAgencyTypes()
        {
            return await _db.AgencyTypes.AsNoTracking().ToListAsync();
        }

        private async Task<List<CommitmentType>> LoadCommitmentTypes()
        {
            return await _db.CommitmentTypes.AsNoTracking().ToListAsync();
        }

        private async Task<List<SalaryType>> LoadPayRates()
        {
            return await _db.SalaryTypes.AsNoTracking().ToListAsync();
        }

        private async Task<List<JobSearchType>> LoadJobSearchTypes()
        {
            return await _db.JobSearchTypes.AsNoTracking().ToListAsync();
        }

        private async Task<List<State>> LoadStates()
        {
            return await _db.States.AsNoTracking().ToListAsync();
        }

        private async Task<List<CommitmentStatus>> LoadCommitmentStatus()
        {
            return await _db.CommitmentStatus.AsNoTracking().ToListAsync();
        }

        private async Task<List<DocumentType>> LoadDocumentTypes()
        {
            return await _db.DocumentTypes.AsNoTracking().ToListAsync();
        }

        private async Task<List<SecurityCertification>> LoadSecurityCertification()
        {
            return await _db.SecurityCertifications.AsNoTracking().ToListAsync();
        }

        private async Task<List<SchoolType>> LoadSchoolTypes()
        {
            return await _db.SchoolType.AsNoTracking().ToListAsync();
        }

        private async Task<List<Degree>> LoadDegrees()
        {
            return await _db.Degrees.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        //private async Task<List<DegreeMinor>> LoadDegreesMinors()
        //{
        //    return await _db.DegreeMinor.AsNoTracking().ToListAsync();
        //}

        private async Task<List<Gender>> LoadGenders()
        {
            return await _db.Genders.AsNoTracking().ToListAsync();
        }

        private async Task<List<Ethnicity>> LoadEthnicities()
        {
            return await _db.Ethnicities.AsNoTracking().ToListAsync();
        }

        private async Task<List<Race>> LoadRaces()
        {
            return await _db.Races.AsNoTracking().ToListAsync();
        }

        private async Task<List<Institution>> LoadInstitutions()
        {
            return await _db.Institutions.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        private async Task<List<Discipline>> LoadDisciplines()
        {
            return await _db.Disciplines.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        private async Task<List<ProfileStatus>> LoadProfileStatus()
        {
            return await _db.ProfileStatus.AsNoTracking().ToListAsync();
        }

        private async Task<List<AcademiaUserRole>> LoadAcademiaUserRole()
        {
            return await _db.AcademiaUserRoles.AsNoTracking().ToListAsync();
        }

        private async Task<List<AgencyUserRole>> LoadAgencyUserRole()
        {
            return await _db.AgencyUserRoles.AsNoTracking().ToListAsync();
        }

        private async Task<List<GlobalConfiguration>> LoadGlobalConfiguration()
        {
            return await _db.GlobalConfiguration.AsNoTracking().ToListAsync();
        }

        private async Task<List<JobActivityStatus>> LoadJobActivityStatus()
        {
            return await _db.JobActivityStatus.AsNoTracking().ToListAsync();
        }

        private async Task<List<InstitutionType>> LoadInstitutionTypes()
        {
            return await _db.InstitutionTypes.AsNoTracking().ToListAsync();
        }

        private async Task<List<AcademicSchedule>> LoadAcademicSchedules()
        {
            return await _db.AcademicSchedule.AsNoTracking().ToListAsync();
        }

        private async Task<List<InstitutionContactType>> LoadInstitutionContactTypes()
        {
            return await _db.InstitutionContactType.AsNoTracking().ToListAsync();
        }

        private async Task<List<CommitmentApprovalWorkflow>> LoadCommitmentApprovalWorkflows()
        {
            return await _db.CommitmentApprovalWorkflow.AsNoTracking().ToListAsync();
        }
        private async Task<List<StatusOption>> LoadStatusOption()
        {
            return await _db.StatusOption.AsNoTracking().ToListAsync();
        }

        private async Task<List<Citizenship>> LoadCitizenship()
        {
            return await _db.Citizenship.AsNoTracking().ToListAsync();
        }

        public async Task<List<ExtensionType>> GetExtensionTypeAsync()
        {
            return await _db.ExtensionType.AsNoTracking().ToListAsync();
        }

		public async Task<List<SessionList>> GetSessionsAsync()
		{
			return await _db.SessionList.AsNoTracking().ToListAsync();
		}

		public async Task<List<ProgramYear>> GetProgramYears()
		{
			return await _db.ProgramYear.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
		}

        public async Task<List<FollowUpTypeOption>> LoadFollowUpTypeOptions()
        {
            return await _db.FollowUpTypeOption.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        public class AgencyCacheDTO
        {
            public int AgencyId { get; set; }
            public int? AgencyTypeId { get; set; }
            public string Name { get; set; }
            public int? ParentAgencyId { get; set; }
            public int? StateID { get; set; }
            public string Workflow { get; set; }
        }

    }
}
