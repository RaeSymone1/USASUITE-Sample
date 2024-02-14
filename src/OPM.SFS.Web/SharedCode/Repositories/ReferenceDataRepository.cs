using OPM.SFS.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OPM.SFS.Core.DTO;
using static OPM.SFS.Web.SharedCode.CacheHelper;

namespace OPM.SFS.Web.SharedCode
{
	public interface IReferenceDataRepository
	{
		Task<List<Citizenship>> GetCitizenshipAsync();
		Task<List<Contract>> GetContractAsync();
		Task<List<ExtensionType>> GetExtensionTypeAsync();
		Task<List<FollowUpTypeOption>> GetFollowUpTypeOptionsAsync();
		Task<List<StatusOption>> GetStatusOptionsAsync();
		Task<List<Discipline>> GetDisciplinesAsync();
		Task<List<Degree>> GetDegreesAsync();
		Task<List<State>> GetStatesAsync();
		Task<List<Institution>> GetInstitutionsAsync();
		Task<List<SessionList>> GetSessionsAsync();
		Task<List<ProgramYear>> GetProgramYearsAsync();
		Task<List<GlobalConfiguration>> GetGlobalConfigurationsAsync();
		Task<List<ProfileStatus>> GetProfileStatusAsync();
        Task<List<SecurityCertification>> GetSecurityCertificationAsync();
        Task<List<AgencyReferenceDTO>> GetAgenciesAsync();
        Task<List<AgencyReferenceDTO>> GetAgenciesWithDisabledAsync();
        Task<List<CommitmentType>> GetCommitmentTypeAsync();

    }

	public class ReferenceDataRepository : IReferenceDataRepository
	{
		private readonly ScholarshipForServiceContext _db;
		public ReferenceDataRepository(ScholarshipForServiceContext db) => _db = db;


		public async Task<List<StatusOption>> GetStatusOptionsAsync()
		{
			return await _db.StatusOption.AsNoTracking().ToListAsync();
		}

		public async Task<List<ExtensionType>> GetExtensionTypeAsync()
		{
			return await _db.ExtensionType.AsNoTracking().ToListAsync();
		}

		public async Task<List<Citizenship>> GetCitizenshipAsync()
		{
			return await _db.Citizenship.AsNoTracking().ToListAsync();
		}

		public async Task<List<Contract>> GetContractAsync()
		{
			return await _db.Contract.AsNoTracking().ToListAsync();
		}

		public async Task<List<FollowUpTypeOption>> GetFollowUpTypeOptionsAsync()
		{
			return await _db.FollowUpTypeOption.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
		}

        public async Task<List<Discipline>> GetDisciplinesAsync()
        {
            return await _db.Disciplines.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<List<Degree>> GetDegreesAsync()
        {
            return await _db.Degrees.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<List<State>> GetStatesAsync()
        {
            return await _db.States.AsNoTracking().ToListAsync();
        }

        public async Task<List<Institution>> GetInstitutionsAsync()
        {
            return await _db.Institutions.Include(m => m.AcademicSchedule).AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<List<SessionList>> GetSessionsAsync()
        {
            return await _db.SessionList.AsNoTracking().ToListAsync();
        }

        public async Task<List<ProgramYear>> GetProgramYearsAsync()
        {
            return await _db.ProgramYear.AsNoTracking().OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<List<GlobalConfiguration>> GetGlobalConfigurationsAsync()
        {
            return await _db.GlobalConfiguration.AsNoTracking().ToListAsync();
        }

        public async Task<List<ProfileStatus>> GetProfileStatusAsync()
        {
            return await _db.ProfileStatus.AsNoTracking().ToListAsync();
        }

        public async Task<List<SecurityCertification>> GetSecurityCertificationAsync()
        {
            return await _db.SecurityCertifications.AsNoTracking().ToListAsync();
        }

        public async Task<List<CommitmentType>> GetCommitmentTypeAsync()
        {
            return await _db.CommitmentTypes.AsNoTracking().ToListAsync();
        }

        public async Task<List<AgencyReferenceDTO>> GetAgenciesAsync()
        {
            return await _db.Agencies.Where(m => m.IsDisabled == false).Select(x => new AgencyReferenceDTO()
            {
                AgencyId = x.AgencyId,
                Name = x.Name,
                ParentAgencyId = x.ParentAgencyId,
                StateID = x.Address.StateId,
                AgencyTypeId = x.AgencyTypeId,
                Workflow = x.CommitmentApprovalWorkflow.Code

            }).ToListAsync();
        }

        public async Task<List<AgencyReferenceDTO>> GetAgenciesWithDisabledAsync()
        {
            return await _db.Agencies.Select(x => new AgencyReferenceDTO()
            {
                AgencyId = x.AgencyId,
                Name = x.Name,
                ParentAgencyId = x.ParentAgencyId,
                StateID = x.Address.StateId,
                AgencyTypeId = x.AgencyTypeId,
                Workflow = x.CommitmentApprovalWorkflow.Code

            }).ToListAsync();
        }
    }
}
