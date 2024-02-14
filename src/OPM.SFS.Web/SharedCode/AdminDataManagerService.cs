using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OPM.SFS.Data;
using OPM.SFS.Web.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.SharedCode
{
    public interface IAdminDataManagerService
    {
        Task<List<DataManagementVM>> GetDataOptionsByType(string option);
        Task<bool> UpdateDataOptions(string option, DataManagementVM request, string action);
    }

    public class AdminDataManagerService : IAdminDataManagerService
    {
        private readonly ScholarshipForServiceContext _efDB;
        private Dictionary<string, Func<string, Task<List<DataManagementVM>>>> getDataFunctions;
        private Dictionary<string, Func<DataManagementVM, string, Task<bool>>> updateDateFunctions;
        public AdminDataManagerService(ScholarshipForServiceContext efDB)
        {

            _efDB = efDB;
            SetGetDataFunctions();
            SetUpdateFunctions();
        }

        private void SetGetDataFunctions()
        {
            getDataFunctions = new Dictionary<string, Func<string, Task<List<DataManagementVM>>>>();
            getDataFunctions.Add("year", DataOptionYear);
            getDataFunctions.Add("session", DataOptionSession);
            getDataFunctions.Add("contract", DataOptionContract);
            getDataFunctions.Add("degree", DataOptionDegree);
            getDataFunctions.Add("discipline", DataOptionDiscipline);
            getDataFunctions.Add("status", DataOptionStatus);
            getDataFunctions.Add("followupactiontype", DataOptionFollowUpType);
            getDataFunctions.Add("extensiontype", DataOptionExtensionType);
        }

        private void SetUpdateFunctions()
        {
            updateDateFunctions = new Dictionary<string, Func<DataManagementVM, string, Task<bool>>>();
            updateDateFunctions.Add("year", UpdateDataOptionYear);
            updateDateFunctions.Add("session", UpdateDataOptionSession);
            updateDateFunctions.Add("contract", UpdateDataOptionContract);
            updateDateFunctions.Add("degree", UpdateDataOptionDegree);
            updateDateFunctions.Add("discipline", UpdateDataOptionDiscipline);
            updateDateFunctions.Add("status", UpdateDataOptionStatus);
            updateDateFunctions.Add("followupactiontype", UpdateDataOptionFollowUpActionType);
            updateDateFunctions.Add("extensiontype", UpdateDataOptionExtensionType);
        }

        public async Task<List<DataManagementVM>> GetDataOptionsByType(string option)
        {            
            return await getDataFunctions[option].Invoke(option);
        }

        public async Task<bool> UpdateDataOptions(string option, DataManagementVM request, string action)
        {            
            return await updateDateFunctions[option].Invoke(request, action);
        }        

        private async Task<List<DataManagementVM>> DataOptionYear(string option)
        {
            var data = await _efDB.ProgramYear.Select(m => new DataManagementVM()
            {
                ID = m.ProgramYearId.ToString(),
                Name = m.Name.ToString(),
                DataGroup = option
            }).OrderBy(m => m.Name).ToListAsync();
            return data;
        }

        private async Task<List<DataManagementVM>> DataOptionSession(string option)
        {
            var data = await _efDB.SessionList.Select(m => new DataManagementVM()
            {
                ID = m.SessionListId.ToString(),
                Name = m.Name,
                DataGroup = option
            }).ToListAsync();
            return data;
        }

        private async Task<List<DataManagementVM>> DataOptionContract(string option)
        {
            var data = await _efDB.Contract.Select(m => new DataManagementVM()
            {
                ID = m.ContractId.ToString(),
                Name = m.Name,
                DataGroup = option
            }).OrderBy(m => m.Name).ToListAsync();
            return data;
        }

        private async Task<List<DataManagementVM>> DataOptionDegree(string option)
        {
            var data = await _efDB.Degrees.Select(m => new DataManagementVM()
            {
                ID = m.DegreeId.ToString(),
                Name = m.Name,
                Code = m.Code,
                DataGroup = option
            }).OrderBy(m => m.Name).ToListAsync();
            return data;
        }

        private async Task<List<DataManagementVM>> DataOptionDiscipline(string option)
        {
            var data = await _efDB.Disciplines.Select(m => new DataManagementVM()
            {
                ID = m.DisciplineId.ToString(),
                Name = m.Name,
                Code = m.Code,
                DataGroup = option
            }).OrderBy(m => m.Name).ToListAsync();
            return data;
        }

        private async Task<List<DataManagementVM>> DataOptionStatus(string option)
        {
            var data = await _efDB.StatusOption.Where(m => m.IsDeleted == false).Select(m => new DataManagementVM()
            {
                ID = m.StudentStatusId.ToString(),
                Option = m.Option,
                Status = m.Status,
                Phase = m.Phase,
                Placement = m.PostGradPlacementGroup,
                DataGroup = option
            }).OrderBy(m => m.Option).ThenBy(m => m.Status).ThenBy(m => m.Phase).ToListAsync();
            return data;
        }

        private async Task<List<DataManagementVM>> DataOptionFollowUpType(string option)
        {
            var data = await _efDB.FollowUpTypeOption.Select(m => new DataManagementVM()
            {
                ID = m.FollowUpTypeOptionID.ToString(),
                Name = m.Name,
                DataGroup = option
            }).OrderBy(m => m.Name).ToListAsync();
            return data;
        }

        private async Task<List<DataManagementVM>> DataOptionExtensionType(string option)
        {
            var data = await _efDB.ExtensionType.Select(m => new DataManagementVM()
            {
                ID = m.ExtensionTypeID.ToString(),
                Name = m.Extension,
                Code = m.Months.ToString(),
                DataGroup = option
            }).OrderBy(m => m.Name).ToListAsync();
            return data;
        }

        private async Task<bool> UpdateDataOptionYear(DataManagementVM request, string action)
        {
            if(action == "update")
            {
                var update = await _efDB.ProgramYear.FirstOrDefaultAsync(m => m.ProgramYearId == Convert.ToInt32(request.ID));
                update.Name = Convert.ToInt32(request.Name);
                await _efDB.SaveChangesAsync();
            }
            else if(action == "delete")
            {
                var toDelete = await _efDB.ProgramYear.Where(m => m.ProgramYearId == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                _efDB.ProgramYear.Remove(toDelete);
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.ProgramYear.Add(new ProgramYear() { Name = Convert.ToInt32(request.Name) });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UpdateDataOptionSession(DataManagementVM request, string action)
        {
            if (action == "update")
            {
                var update = await _efDB.SessionList.FirstOrDefaultAsync(m => m.SessionListId == Convert.ToInt32(request.ID));
                update.Name = request.Name;
                await _efDB.SaveChangesAsync();
            }
            else if (action == "delete")
            {
                var toDelete = await _efDB.SessionList.Where(m => m.SessionListId == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                _efDB.SessionList.Remove(toDelete);
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.SessionList.Add(new SessionList() { Name = request.Name });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UpdateDataOptionContract(DataManagementVM request, string action)
        {
            if (action == "update")
            {
                var update = await _efDB.Contract.FirstOrDefaultAsync(m => m.ContractId == Convert.ToInt32(request.ID));
                update.Name = request.Name;
                await _efDB.SaveChangesAsync();
            }
            else if (action == "delete")
            {
                var toDelete = await _efDB.Contract.Where(m => m.ContractId == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                _efDB.Contract.Remove(toDelete);
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.Contract.Add(new Contract() { Name = request.Name });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UpdateDataOptionDegree(DataManagementVM request, string action)
        {
            if (action == "update")
            {
                var update = await _efDB.Degrees.FirstOrDefaultAsync(m => m.DegreeId == Convert.ToInt32(request.ID));
                update.LastModified = DateTime.UtcNow;
                update.Code = request.Code;
                update.Name = request.Name;
                await _efDB.SaveChangesAsync();
            }
            else if (action == "delete")
            {
                var toDelete = await _efDB.Degrees.Where(m => m.DegreeId == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                _efDB.Degrees.Remove(toDelete);
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.Degrees.Add(new Degree() { Name = request.Name, Code = request.Code, DateInserted = DateTime.UtcNow });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UpdateDataOptionDiscipline(DataManagementVM request, string action)
        {
            if (action == "update")
            {
                var update = await _efDB.Disciplines.FirstOrDefaultAsync(m => m.DisciplineId == Convert.ToInt32(request.ID));
                update.LastModified = DateTime.UtcNow;
                update.Code = request.Code;
                update.Name = request.Name;
                await _efDB.SaveChangesAsync();
            }
            else if (action == "delete")
            {
                var toDelete = await _efDB.Disciplines.Where(m => m.DisciplineId == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                _efDB.Disciplines.Remove(toDelete);
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.Disciplines.Add(new Discipline() { Name = request.Name, Code = request.Code, DateInserted = DateTime.UtcNow });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UpdateDataOptionStatus(DataManagementVM request, string action)
        {
            if (action == "update")
            {
                var update = await _efDB.StatusOption.FirstOrDefaultAsync(m => m.StudentStatusId == Convert.ToInt32(request.ID));
                update.Option = request.Option;
                update.Status = request.Status;
                update.Phase = request.Phase;
                update.PostGradPlacementGroup = request.Placement;
                await _efDB.SaveChangesAsync();
            }
            else if (action == "delete")
            {
                var toDelete = await _efDB.StatusOption.Where(m => m.StudentStatusId == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                toDelete.IsDeleted = true;
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.StatusOption.Add(new StatusOption()
                {
                    Option = request.Option,
                    Phase = request.Phase,
                    Status = request.Status,
                    PostGradPlacementGroup = request.Placement,
                    IsDeleted = false
                });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UpdateDataOptionExtensionType(DataManagementVM request, string action)
        {
            if (action == "update")
            {
                var update = await _efDB.ExtensionType.FirstOrDefaultAsync(m => m.ExtensionTypeID == Convert.ToInt32(request.ID));
                update.Extension = request.Name;
                update.Months = Convert.ToInt32(request.Code);
                await _efDB.SaveChangesAsync();
            }
            else if (action == "delete")
            {
                var toDelete = await _efDB.ExtensionType.Where(m => m.ExtensionTypeID == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                _efDB.ExtensionType.Remove(toDelete);
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.ExtensionType.Add(new ExtensionType()
                {
                    Extension = request.Name,
                    Months = string.IsNullOrWhiteSpace(request.Code) ? 0 : Convert.ToInt32(request.Code),
                });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> UpdateDataOptionFollowUpActionType(DataManagementVM request, string action)
        {
            if (action == "update")
            {
                var update = await _efDB.FollowUpTypeOption.FirstOrDefaultAsync(m => m.FollowUpTypeOptionID == Convert.ToInt32(request.ID));
                update.Name = request.Name;
                await _efDB.SaveChangesAsync();
            }
            else if (action == "delete")
            {
                var toDelete = await _efDB.FollowUpTypeOption.Where(m => m.FollowUpTypeOptionID == Convert.ToInt32(request.ID)).FirstOrDefaultAsync();
                _efDB.FollowUpTypeOption.Remove(toDelete);
                await _efDB.SaveChangesAsync();
            }
            else
            {
                _efDB.FollowUpTypeOption.Add(new FollowUpTypeOption()
                {
                    Name = request.Name
                });
                await _efDB.SaveChangesAsync();
            }
            return true;
        }
    }
}
