using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Admin
{
    [Authorize(Roles = "AD")]
    public class CommitmentListModel : PageModel
    {
        [BindProperty]
        public AdminCommitmentListViewModel Data { get; set; }

        [FromQuery(Name = "at")]
        public int AgencyTypeID { get; set; }

        [FromQuery(Name = "ct")]
        public int CommitmentTypeID { get; set; }
        [FromQuery(Name = "s")]
        public int ApprovalStatus { get; set; }

        [FromQuery(Name = "sd")]
        public string Startdate { get; set; }
        [FromQuery(Name = "ed")]
        public string Enddate { get; set; }


        private readonly IMediator _mediator;

        public CommitmentListModel(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query(){ AgencyTypeID = AgencyTypeID, CommitmentTypeID = CommitmentTypeID, StartDate = Startdate, EndDate = Enddate, ApprovalStatus = ApprovalStatus });
        }

        public async Task<FileResult> OnGetExcelView()
        {
            var file = await _mediator.Send(new QueryExcelView() { ExcelFile = 1, studentId = 1 });
            return file;
        }

        public class Query : IRequest<AdminCommitmentListViewModel>
        {
            public int AgencyTypeID { get; set; }
            public int CommitmentTypeID { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public int ApprovalStatus { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AdminCommitmentListViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<AdminCommitmentListViewModel> Handle(Query request, CancellationToken cancellationToken)
            {

                var data = await _db.StudentCommitments.Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.Agency).ThenInclude(m => m.AgencyType)
                    .Include(m => m.CommitmentStatus)
                    .Include(m => m.CommitmentType)
                    .AsNoTracking()
                    .Where(m => m.CommitmentStatus.Code != CommitmentProcessConst.Incomplete)
                    .Where(m => m.Agency.IsDisabled == false)
                    .Where(m => m.IsDeleted == false)
                    .WhereIf(request.AgencyTypeID > 0, x => x.Agency.AgencyTypeId == request.AgencyTypeID)
                    .WhereIf(request.CommitmentTypeID > 0, x => x.CommitmentTypeId == request.CommitmentTypeID)
                    .WhereIf(request.ApprovalStatus  > 0, x => x.CommitmentStatus.CommitmentStatusID == request.ApprovalStatus)
                    .WhereIf(!string.IsNullOrWhiteSpace(request.StartDate) && !string.IsNullOrWhiteSpace(request.EndDate), 
                        (x => x.DateApproved >= Convert.ToDateTime(request.StartDate) && x.DateApproved < Convert.ToDateTime(request.EndDate)))
                    .Select(s => new
                    {
                        Name = $"{s.Student.FirstName} {s.Student.LastName}",
                        StudentID = s.StudentId,
                        CommitmentID = s.StudentCommitmentId,
                        Institution = s.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                        CommitmentType = s.CommitmentType.Name,
                        Agency = s.Agency.Name,
                        AgencyType = s.Agency.AgencyType.Name,
                        JobTitle = s.JobTitle,
                        StartDate = s.StartDate.HasValue ? s.StartDate.Value.ToShortDateString() : "",
                        Status = s.CommitmentStatus.AdminDisplay,
                        Description = s.CommitmentStatus.Description,
                        SubmitDate = s.DateSubmitted.HasValue ? s.DateSubmitted.Value.ToShortDateString() : "",
                        RejectNote = s.RejectNote
                    }).OrderByDescending(m => m.CommitmentID).ToListAsync();

                var model = new AdminCommitmentListViewModel();
                model.Commitments = new();
                foreach(var d in data)
                {
                    model.Commitments.Add(new AdminCommitmentListViewModel.CommitListItem()
                    {
                        StudentFullName = d.Name,
                        StudentID = d.StudentID, 
                        CommitmentID = d.CommitmentID,
                        Institution = d.Institution,
                        CommitmentType = d.CommitmentType,
                        Agency = d.Agency,
                        AgencyType = d.AgencyType,
                        JobTitle = d.JobTitle,
                        StartDate = d.StartDate,
                        Status = d.Status,
                        SubmitDate = d.SubmitDate,
                        StatusDisplay = d.Description,
                        RejectNote = d.RejectNote
                        
                    });
                }

                return model;
            }
        }

        public class QueryExcelView : IRequest<FileStreamResult>
        {
            public int ExcelFile { get; set; }
            public int studentId { get; set; }
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

                var data = await _db.StudentCommitments.Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Include(m => m.Agency).ThenInclude(m => m.AgencyType)
                    .Include(m => m.CommitmentStatus)
                    .Include(m => m.CommitmentType)
                    .AsNoTracking()
                    .Where(m => m.CommitmentStatus.Code != CommitmentProcessConst.Incomplete && m.IsDeleted == false)
                    .Where(m => m.Agency.IsDisabled == false)
                    .Select(s => new
                    {
                        Name = $"{s.Student.FirstName} {s.Student.LastName}",
                        StudentID = s.StudentId,
                        CommitmentID = s.StudentCommitmentId,
                        Institution = s.Student.StudentInstitutionFundings.FirstOrDefault().Institution.Name,
                        CommitmentType = s.CommitmentType.Name,
                        Agency = s.Agency.Name,
                        AgencyType = s.Agency.AgencyType.Name,
                        JobTitle = s.JobTitle,
                        StartDate = s.StartDate.HasValue ? s.StartDate.Value.ToShortDateString() : "",
                        Status = s.CommitmentStatus.AdminDisplay,
                        SubmitDate = s.DateSubmitted.HasValue ? s.StartDate.Value.ToShortDateString() : ""
                    }).ToListAsync();

                var model = new AdminCommitmentListExcelViewModel();
                model.Commitments = new();
                foreach (var d in data)
                {
                    model.Commitments.Add(new AdminCommitmentListExcelViewModel.CommitmentListItem()
                    {
                        StudentFullName = d.Name,
                        StudentID = d.StudentID,
                        CommitmentID = d.CommitmentID,
                        Institution = d.Institution,
                        CommitmentType = d.CommitmentType,
                        Agency = d.Agency,
                        AgencyType = d.AgencyType,
                        JobTitle = d.JobTitle,
                        StartDate = d.StartDate,
                        Status = d.Status,
                    });
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var stream = new MemoryStream();
                using (var xlPackage = new ExcelPackage(stream))
                {
                    //Define worksheet
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Commitment");

                    //Styling
                    var customStyle = xlPackage.Workbook.Styles.CreateNamedStyle("CustomStyle");
                    customStyle.Style.Font.UnderLine = true;

                    //First row
                    var firstRow = 5;
                    var firstColumn = 2;
                    var row = firstRow;
                    var column = firstColumn;

                    //Set report heading
                    worksheet.Cells["C1"].Value = "Admin Commitment List";
                    using (var r = worksheet.Cells["C1:G1"])
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
                    worksheet.Cells[row, column++].Value = "Student Name";
                    worksheet.Cells[row, column++].Value = "Institution";
                    worksheet.Cells[row, column++].Value = "Commitment Type";
                    worksheet.Cells[row, column++].Value = "Agency";
                    worksheet.Cells[row, column++].Value = "Agency Type";
                    worksheet.Cells[row, column++].Value = "Status";
                    worksheet.Cells[row, column++].Value = "Job Title";
                    worksheet.Cells[row, column].Value = "Start Date";

                    //Header boarder
                    column = firstColumn;
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worksheet.Cells[row, column++].Style.Border.BorderAround(ExcelBorderStyle.Thin);
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

                    column++;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                    column++;
                    worksheet.Column(column).Width = 25;
                    worksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;


                    //Format table headings
                    using (var r = worksheet.Cells["C5:J5"])
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
                    foreach (var c in model.Commitments)
                    {
                        worksheet.Cells[row, column++].Value = c.StudentFullName;
                        worksheet.Cells[row, column++].Value = c.Institution;
                        worksheet.Cells[row, column++].Value = c.CommitmentType;
                        worksheet.Cells[row, column++].Value = c.Agency;
                        worksheet.Cells[row, column++].Value = c.AgencyType;
                        worksheet.Cells[row, column++].Value = c.Status;
                        worksheet.Cells[row, column++].Value = c.JobTitle;
                        worksheet.Cells[row, column++].Value = c.StartDate;

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
