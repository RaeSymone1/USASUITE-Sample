using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using OPM.SFS.Core.DTO;
using OPM.SFS.Core.Shared;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.Shared.Extensions;
using OPM.SFS.Web.SharedCode;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Mappings
{
    public interface ICommitmentMappingHelper
    {
        MapperConfiguration GetConfigurationForDTO();
        Task<CommitmentModelViewModel> PopulateViewModelAsync(StudentCommitmentDTO data);
    }

    public class CommitmentMappingHelper : ICommitmentMappingHelper
    {
        private readonly ICacheHelper _cache;
        private readonly ICryptoHelper _crypto;
        public readonly ICommitmentProcessService _commitHelper;

        public CommitmentMappingHelper(ICacheHelper cache, ICryptoHelper crypto, ICommitmentProcessService commitHelper)
        {
            _cache = cache;
            _crypto = crypto;
            _commitHelper = commitHelper;
        }
        public MapperConfiguration GetConfigurationForDTO()
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateProjection<StudentCommitment, StudentCommitmentDTO>()
                   .ForMember(dto => dto.CommitmentApprovalWorkflowCode, ef => ef.MapFrom(d => d.Agency.CommitmentApprovalWorkflow.Code))
                   .ForMember(dto => dto.CommitmentId, ef => ef.MapFrom(d => d.StudentCommitmentId))
                    .ForMember(dto => dto.FirstName, ef => ef.MapFrom(d => d.Student.FirstName))
                    .ForMember(dto => dto.LastName, ef => ef.MapFrom(d => d.Student.LastName))
                    .ForMember(dto => dto.Ssn, ef => ef.MapFrom(d => d.Student.Ssn))
                    .ForMember(dto => dto.AgencyTypeId, ef => ef.MapFrom(d => d.Agency.AgencyTypeId))
                    .ForMember(dto => dto.CommitmentStatusCode, ef => ef.MapFrom(d => d.CommitmentStatus.Code))
                    .ForMember(dto => dto.ParentAgencyId, ef => ef.MapFrom(d => d.Agency.ParentAgencyId))
                    .ForMember(dto => dto.AgencyApprovalWorkflowCode, ef => ef.MapFrom(d => d.Agency.CommitmentApprovalWorkflow.Code))
                    .ForMember(dto => dto.CommitmentTypeCode, ef => ef.MapFrom(d => d.CommitmentType.Code))
                    .ForMember(dto => dto.AddressCity, ef => ef.MapFrom(d => d.Address.City))
                    .ForMember(dto => dto.AddressStateID, ef => ef.MapFrom(d => d.Address.StateId))
                    .ForMember(dto => dto.AddressPostalCode, ef => ef.MapFrom(d => d.Address.PostalCode))
                    .ForMember(dto => dto.AddressCountry, ef => ef.MapFrom(d => d.Address.Country))
                    .ForMember(dto => dto.SupervisorEmailAddress, ef => ef.MapFrom(d => d.SupervisorContact.Email))
                    .ForMember(dto => dto.SupervisorContactPhone, ef => ef.MapFrom(d => d.SupervisorContact.Phone))
                    .ForMember(dto => dto.SupervisorFirstName, ef => ef.MapFrom(d => d.SupervisorContact.FirstName))
                    .ForMember(dto => dto.SupervisorLastName, ef => ef.MapFrom(d => d.SupervisorContact.LastName))
                    .ForMember(dto => dto.SupervisorPhoneExtension, ef => ef.MapFrom(d => d.SupervisorContact.PhoneExt))
                    .ForMember(dto => dto.MentorFirstName, ef => ef.MapFrom(d => d.MentorContact.FirstName))
                    .ForMember(dto => dto.MentorLastName, ef => ef.MapFrom(d => d.MentorContact.LastName))
                    .ForMember(dto => dto.MentorEmailAddress, ef => ef.MapFrom(d => d.MentorContact.Email))
                    .ForMember(dto => dto.MentorPhone, ef => ef.MapFrom(d => d.MentorContact.Phone))
                    .ForMember(dto => dto.MentorPhoneExtension, ef => ef.MapFrom(d => d.MentorContact.PhoneExt))
                    .ForMember(dto => dto.StatusDisplay, ef => ef.MapFrom(d => d.CommitmentStatus.AdminDisplay))


                   );
            return configuration;
        }

        public async Task<CommitmentModelViewModel> PopulateViewModelAsync(StudentCommitmentDTO data)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<StudentCommitmentDTO, CommitmentModelViewModel>()
                .ForMember(vm => vm.AgencyType, dto => dto.MapFrom(d => d.AgencyTypeId))
                .ForMember(vm => vm.StudentID, dto => dto.MapFrom(d => d.StudentId))
                .ForMember(vm => vm.AgencyApprovalWorkflow, dto => dto.MapFrom(d => d.AgencyApprovalWorkflowCode))
                .ForMember(vm => vm.Agency, dto => dto.MapFrom(d => d.AgencyId))
                .ForMember(vm => vm.CommitmentType, dto => dto.MapFrom(d => d.CommitmentTypeId))
                .ForMember(vm => vm.PayRate, dto => dto.MapFrom(d => d.SalaryTypeId))
                .ForMember(vm => vm.SalaryMin, dto => dto.MapFrom(d => d.SalaryMinimum))
                .ForMember(vm => vm.SalaryMax, dto => dto.MapFrom(d => d.SalaryMaximum))
                .ForMember(vm => vm.Country, dto => dto.MapFrom(d => d.AddressCountry))
                .ForMember(vm => vm.City, dto => dto.MapFrom(d => d.AddressCity))
                .ForMember(vm => vm.State, dto => dto.MapFrom(d => d.AddressStateID))
                .ForMember(vm => vm.PostalCode, dto => dto.MapFrom(d => d.AddressPostalCode))
                .ForMember(vm => vm.JobSearchType, dto => dto.MapFrom(d => d.JobSearchTypeId))
                .ForMember(vm => vm.Status, dto => dto.MapFrom(d => d.CommitmentStatusCode))
                .ForMember(vm => vm.CommitmentType, dto => dto.MapFrom(d => d.CommitmentTypeId))
                .ForMember(vm => vm.MentorContactPhone, dto => dto.MapFrom(d => d.MentorPhone))
            );

            var mapper = new Mapper(config);
            CommitmentModelViewModel vm = mapper.Map<CommitmentModelViewModel>(data);

            //sets lists on the vm
            vm.AgencyTypeList = new SelectList(await _cache.GetAgencyTypesAsync(), nameof(AgencyType.AgencyTypeId), nameof(AgencyType.Name));
            vm.PayRateList = new SelectList(await _cache.GetPayRateListAsync(), nameof(SalaryType.SalaryTypeId), nameof(SalaryType.Name));
            vm.JobSearchTypeList = new SelectList(await _cache.GetJobSearchTypeAsync(), nameof(JobSearchType.JobSearchTypeID), nameof(JobSearchType.Name));
            vm.StateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));
            vm.CommitmentTypeList = new SelectList(await _cache.GetCommitmentTypeAsync(), nameof(CommitmentType.CommitmentTypeId), nameof(CommitmentType.Name));
            string startDate = data.StartDate.HasValue ? data.StartDate.Value.ToShortDateString() : "";
            vm.ShowForm = _commitHelper.GetFormByStatus(data.CommitmentStatusCode, data.AgencyApprovalWorkflowCode, startDate);
			//pull agencies from cache!
			var agencies = await _cache.GetAgenciesAsync();

            //set agency display entries
            if (data.ParentAgencyId.HasValue && data.ParentAgencyId.Value > 0)
            {
                //populate the agency lists

                vm.AgencyList = new SelectList(agencies.OrderBy(m => m.Name).Where(m => m.AgencyTypeId == data.AgencyTypeId).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));

                vm.SubAgencyList = new SelectList(agencies.Where(m => m.ParentAgencyId == data.ParentAgencyId.Value)
                                                .OrderBy(m => m.Name).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));


                vm.Agency = agencies.Where(m => m.AgencyId == data.ParentAgencyId.Value).Select(m => m.AgencyId).FirstOrDefault();
                vm.SubAgency = data.AgencyId;
            }
            else
            {
                var filteredAgencies = agencies.Where(m => m.AgencyTypeId == data.AgencyTypeId).ToList();
                vm.AgencyList = new SelectList(agencies.OrderBy(m => m.Name).ToList(), nameof(OPM.SFS.Data.Agency.AgencyId), nameof(OPM.SFS.Data.Agency.Name));
                vm.Agency = data.AgencyId.Value;
            }

            //set dates for display 
            vm.StartDateDay = data.StartDate.HasValue ? data.StartDate.Value.Day.ToString() : "";
            vm.StartDateMonth = data.StartDate.HasValue ? data.StartDate.Value.Month.ToString() : "";
            vm.StartDateYear = data.StartDate.HasValue ? data.StartDate.Value.Year.ToString() : "";
            vm.EndDateDay = data.EndDate.HasValue ? data.EndDate.Value.Day.ToString() : "";
            vm.EndDateMonth = data.EndDate.HasValue ? data.EndDate.Value.Month.ToString() : "";
            vm.EndDateYear = data.EndDate.HasValue ? data.EndDate.Value.Year.ToString() : "";
            vm.JobSearchType = data.JobSearchTypeId;

            vm.SalaryMax = vm.SalaryMax.HasValue ? decimal.Round(vm.SalaryMax.Value, 2, MidpointRounding.AwayFromZero) : vm.SalaryMax;
            vm.SalaryMin = vm.SalaryMin.HasValue ? decimal.Round(vm.SalaryMin.Value, 2, MidpointRounding.AwayFromZero) : vm.SalaryMin;

            if (!string.IsNullOrWhiteSpace(data.SupervisorContactPhone))
            {
                vm.SupervisorPhoneAreaCode = data.SupervisorContactPhone.ValidSubstring(0, 3);
                vm.SupervisorPhonePrefix = data.SupervisorContactPhone.ValidSubstring(3, 3);
                vm.SupervisorPhoneSuffix = data.SupervisorContactPhone.ValidSubstring(6, 4);
            }

            if (!string.IsNullOrWhiteSpace(data.MentorPhone))
            {
                vm.MentorPhoneAreaCode = data.MentorPhone.ValidSubstring(0, 3);
                vm.MentorPhonePrefix = data.MentorPhone.ValidSubstring(3, 3);
                vm.MentorPhoneSuffix = data.MentorPhone.ValidSubstring(6, 4);
            }

            //clean whitespace on payplan
            if (!string.IsNullOrWhiteSpace(vm.PayPlan)) vm.PayPlan = vm.PayPlan.Trim();
            if (!string.IsNullOrWhiteSpace(vm.Series)) vm.Series = vm.Series.Trim();
            return vm;
        }
    }
}
