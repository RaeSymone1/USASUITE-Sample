using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST,AO,PI")]
    public class AgencyOfficialModel : PageModel
    {

        [BindProperty]
        public AgencyOfficialListViewModel Data { get; set; }

        private readonly IMediator _mediator;

        public AgencyOfficialModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() {  Referer = Request.Headers["Referer"].ToString()});
        }
        public async Task<FileResult> OnGetExcelView()
        {
            var file = await _mediator.Send(new QueryExcelView() { ExcelFile = 1 });
            return file;
        }

        public class Query : IRequest<AgencyOfficialListViewModel>
        {
            public string Referer { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AgencyOfficialListViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }


            public async Task<AgencyOfficialListViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AgencyOfficialListViewModel model = new();
                model.RefererURL = request.Referer;
                model.AgencyOfficials = new();
                var agencyOfficials = await _db.AgencyUsers.Where(m => m.DisplayContactInfo == true && m.ProfileStatus.Name == "Active")
                    .Select(m => new
                    {
                        Name = $"{m.Lastname},{m.Firstname}",
                        Agency = m.Agency.Name,
                        AgencyType = m.Agency.AgencyType.Name,
                        Email = m.Email,
                        Phone = m.Address.PhoneNumber
                    })
                    .ToListAsync();

                if(agencyOfficials is not null && agencyOfficials.Count > 0)
                {

                    var agencyList = await _cache.GetAgenciesAsync();
                    foreach (var ao in agencyOfficials)
                    {                                       
                        model.AgencyOfficials.Add(new AgencyOfficialListViewModel.AgencyOfficial()
                        {
                            Name = ao.Name,
                            Agency = ao.Agency,
                            AgencyType = ao.AgencyType,
                            Email = ao.Email,
                            Phone = ao.Phone 
                        });
                    }
                }
                return model;
            }
        }

        public class QueryExcelView : IRequest<FileStreamResult>
        {
            public int ExcelFile { get; set; }
        }

        public class QueryExcelViewHandler : IRequestHandler<QueryExcelView, FileStreamResult>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryExcelViewHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<FileStreamResult> Handle(QueryExcelView request, CancellationToken cancellationToken)
            {

                AgencyOfficialListViewModel model = new();
                //model.RefererURL = request.Referer;
                model.AgencyOfficials = new();
                var agencyOfficials = await _db.AgencyUsers.Where(m => m.DisplayContactInfo == true && m.ProfileStatus.Name == "Active")
                     .Select(m => new
                     {
                         Name = $"{m.Lastname},{m.Firstname}",
                         Agency = m.Agency.Name,
                         AgencyType = m.Agency.AgencyType.Name,
                         Email = m.Email,
                         Phone = m.Address.PhoneNumber
                     })
                     .ToListAsync();

                if (agencyOfficials is not null && agencyOfficials.Count > 0)
                {

                    var agencyList = await _cache.GetAgenciesAsync();
                    foreach (var ao in agencyOfficials)
                    {
                        model.AgencyOfficials.Add(new AgencyOfficialListViewModel.AgencyOfficial()
                        {
                            Name = ao.Name,
                            Agency = ao.Agency,
                            AgencyType = ao.AgencyType,
                            Email = ao.Email,
                            Phone = ao.Phone
                        });
                    }
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var stream = new MemoryStream();
                using (var xlPackage = new ExcelPackage(stream))
                {
                    //Define worksheet
                    var worksheet = xlPackage.Workbook.Worksheets.Add("AgencyOfficial");

                    //Styling
                    var customStyle = xlPackage.Workbook.Styles.CreateNamedStyle("CustomStyle");
                    customStyle.Style.Font.UnderLine = true;

                    //First row
                    var firstRow = 5;
                    var firstColumn = 2;
                    var row = firstRow;
                    var column = firstColumn;

                    //Set report heading
                    worksheet.Cells["C1"].Value = "Agency Official List";
                    using (var r = worksheet.Cells["B1:F1"])
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
                    worksheet.Cells[row, column++].Value = "Agency Official";
                    worksheet.Cells[row, column++].Value = "Agency";
                    worksheet.Cells[row, column++].Value = "Agency Type";
                    worksheet.Cells[row, column++].Value = "Email";
                    worksheet.Cells[row, column++].Value = "Phone Number";

                    //Header boarder
                    column = firstColumn;
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



                    //Format table headings
                    using (var r = worksheet.Cells["B5:F5"])
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
                    foreach (var c in model.AgencyOfficials)
                    {
                        worksheet.Cells[row, column++].Value = c.Name;
                        worksheet.Cells[row, column++].Value = c.Agency;
                        worksheet.Cells[row, column++].Value = c.AgencyType;
                        worksheet.Cells[row, column++].Value = c.Email;
                        worksheet.Cells[row, column++].Value = c.Phone;

                        row++;
                        column = firstColumn;
                    }

                    xlPackage.Workbook.Properties.Title = "Agency Officials List";
                    xlPackage.Save();
                }

                stream.Position = 0;
                FileStreamResult fs = new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                return fs;

            }
        }
    }
}
