
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class AccountListModel : PageModel
    {
        [BindProperty]
        public AdminAccountListViewModel Data { get; set; }


        private readonly IMediator _mediator;

        public AccountListModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { AccountType = "AO" });
        }

        public PartialViewResult OnGetChangeAccountType(string type)
        {
            var results = _mediator.Send(new Query () { AccountType = type}).Result;
            return Partial("_AdminAccountListPartial", results);
           
        }
        public async Task<FileResult> OnGetExcelView(string type)
        {
            var file = await _mediator.Send(new QueryExcelView() { AccountType = type });
            return file;
        }

        public class Query : IRequest<AdminAccountListViewModel>
        {
            public string AccountType { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminAccountListViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<AdminAccountListViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AdminAccountListViewModel model = new();
                model.Accounts = new();
                if(request.AccountType == "AO")
                {
                    var allAgencies = await _cache.GetAgenciesWithDisabledAsync();
                    model.AccountType = "AO";
                    var results = await _db.AgencyUsers.AsNoTracking()
                        .Select(x => new
                        {
                            ID = x.AgencyUserId,
                            Username = x.UserName,
                            Email = x.Email,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            Agency = x.Agency.Name,
                            ParentAgency = x.Agency.ParentAgencyId,
                            ProfileStatus = x.ProfileStatus.Name,
                            Display = x.ProfileStatus.Display 
                                                     
                        })
                        .ToListAsync();

                    foreach(var item in results)
                    {
                        AdminAccountListViewModel.AccountItems t = new();
                        t.AccountID = item.ID;
                        t.Username = item.Username;
                        t.Email = item.Email;
                        t.FirstName = item.Firstname;
                        t.Lastname = item.Lastname;
                        if (item.ParentAgency.HasValue && item.ParentAgency.Value > 0)
                        {
                            string parent  = allAgencies.Where(m => m.AgencyId == item.ParentAgency).FirstOrDefault().Name;
                            string child = item.Agency;
                            t.AgencyOrInstitution = $"{parent} - {child}";
                        }
                        else
                        {                           
                            t.AgencyOrInstitution = item.Agency;
                        }
                        t.ProfileStatus = item.ProfileStatus;
                        t.Display = item.Display;
                        model.Accounts.Add(t);
                        
                    }
                }
                else
                {
                    model.AccountType = "PI";
                    var results = await _db.AcademiaUsers.AsNoTracking()
                        .Select(x => new
                        {
                            ID = x.AcademiaUserId,
                            Username = x.UserName,
                            Email = x.Email,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            Instituion = x.Institution.Name,
                            ProfileStatus = x.ProfileStatus.Name,
                            Display = x.ProfileStatus.Display
                        })
                        .ToListAsync();

                    foreach (var item in results)
                    {
                        model.Accounts.Add(new AdminAccountListViewModel.AccountItems
                        {
                            AccountID = item.ID,
                            Username = item.Username,
                            Email = item.Email,
                            FirstName = item.Firstname,
                            Lastname = item.Lastname,
                            AgencyOrInstitution = item.Instituion,
                            ProfileStatus = item.ProfileStatus,
                            Display = item.Display
                    });
                    }
                }

                return model;
            }
        }

        public class QueryExcelView : IRequest<FileStreamResult>
        {
            public string AccountType { get; set; }
        }

        public class QueryExcelViewHandler : IRequestHandler<QueryExcelView, FileStreamResult>
        {
            private readonly ScholarshipForServiceContext _efDB;
            private readonly ICacheHelper _cache;

            public QueryExcelViewHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _efDB = db;
                _cache = cache;
            }

            public async Task<FileStreamResult> Handle(QueryExcelView request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.AccountType)) request.AccountType = "AO";
                AdminPIAgencyListViewModel model = new();
                model.PIAgency = new();
                var a = model.AccountType;
                if (request.AccountType == "AO")
                {
                    var allAgencies = await _cache.GetAgenciesWithDisabledAsync();
                    model.AccountType = "AO";
                    var results = await _efDB.AgencyUsers.AsNoTracking()
                        .Select(x => new
                        {
                            ID = x.AgencyUserId,
                            Username = x.UserName,
                            Email = x.Email,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            Agency = x.Agency.Name,
                            ParentAgency = x.Agency.ParentAgencyId,
                            ProfileStatus = x.ProfileStatus.Name,
                            Display = x.ProfileStatus.Display
                        })
                        .ToListAsync();

                    foreach (var item in results)
                    {
                        AdminPIAgencyListViewModel.PIAgencyItem t = new();
                        t.UserID = item.ID;
                        t.UserName = item.Username;
                        t.Email = item.Email;
                        t.FirstName = item.Firstname;
                        t.LastName = item.Lastname;
                        if (item.ParentAgency.HasValue && item.ParentAgency.Value > 0)
                        {
                            string parent = allAgencies.Where(m => m.AgencyId == item.ParentAgency).FirstOrDefault().Name;
                            string child = item.Agency;
                            t.AgencyOrInstitution = $"{parent} - {child}";
                        }
                        else
                        {
                            t.AgencyOrInstitution = item.Agency;
                        }
                        t.ProfileStatus = item.ProfileStatus;
                        t.Display = item.Display;
                        model.PIAgency.Add(t);

                    }
                }
                else
                {
                    model.AccountType = "PI";
                    var results = await _efDB.AcademiaUsers.AsNoTracking()
                        .Select(x => new
                        {
                            ID = x.AcademiaUserId,
                            Username = x.UserName,
                            Email = x.Email,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            Instituion = x.Institution.Name,
                            ProfileStatus = x.ProfileStatus.Name
       
                        })
                        .ToListAsync();

                    foreach (var item in results)
                    {
                        model.PIAgency.Add(new AdminPIAgencyListViewModel.PIAgencyItem
                        {
                            UserID = item.ID,
                            UserName = item.Username,
                            Email = item.Email,
                            FirstName = item.Firstname,
                            LastName = item.Lastname,
                            AgencyOrInstitution = item.Instituion,
                            ProfileStatus = item.ProfileStatus
                            
                        });
                    }
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var stream = new MemoryStream();
                using (var xlPackage = new ExcelPackage(stream))
                {
                    //Define worksheet
                    var worksheet = xlPackage.Workbook.Worksheets.Add("User List");

                    //Styling
                    var customStyle = xlPackage.Workbook.Styles.CreateNamedStyle("CustomStyle");
                    customStyle.Style.Font.UnderLine = true;

                    //First row
                    var firstRow = 5;
                    var firstColumn = 2;
                    var row = firstRow;
                    var column = firstColumn;

                    //Set report heading
                    if (request.AccountType == "AO")
                    {
                        worksheet.Cells["C1"].Value = "Agency Official User List";
                    }
                    else
                    {
                        worksheet.Cells["C1"].Value = "Principal Investigator (Academia) User List";
                    }
                    using (var r = worksheet.Cells["C1:H1"])
                    {
                        //r.Merge = true;
                        // r.Style.Font.Size = 18;
                        r.Style.Font.Bold = true;
                        r.Style.Font.Color.SetColor(Color.Black);
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.White);
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    }

                    //header title
                    worksheet.Cells[row, column++].Value = "User Name";
                    worksheet.Cells[row, column++].Value = "Email";
                    worksheet.Cells[row, column++].Value = "First Name";
                    worksheet.Cells[row, column++].Value = "Last Name";
                    worksheet.Cells[row, column++].Value = "Profile Status";

                    if (request.AccountType == "AO")
                    {
                        worksheet.Cells[row, column++].Value = "Agency";
                    }
                    else
                    {
                        worksheet.Cells[row, column++].Value = "Institution";
                    }

                    //Header boarder
                    column = firstColumn;
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    //Set column width
                    column = firstColumn;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    column++;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    column++;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    column++;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    column++;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    column++;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;



                    //Format table headings
                    using (var r = worksheet.Cells["C5:H5"])
                    {
                        //  r.Style.Font.Size = 11;  
                        r.Style.Font.Bold = true;
                        r.Style.Font.Color.SetColor(Color.Black);
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.White);
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    row++;

                    //Insert data row by row
                    column = firstColumn;
                    foreach (var c in model.PIAgency)
                    {
                        worksheet.Cells[row, column++].Value = c.UserName;
                        worksheet.Cells[row, column++].Value = c.Email;
                        worksheet.Cells[row, column++].Value = c.FirstName;
                        worksheet.Cells[row, column++].Value = c.LastName;
                        worksheet.Cells[row, column++].Value = c.AgencyOrInstitution;
                        worksheet.Cells[row, column++].Value = c.ProfileStatus;

                        row++;
                        column = firstColumn;
                    }

                    xlPackage.Workbook.Properties.Title = "Admin Commitment Student List";
                    xlPackage.Save();
                }

                stream.Position = 0;
                FileStreamResult fs = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return fs;

            }

        }
    }
}
