using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Models.Academia;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Web.Pages.Academia
{
    [Authorize(Roles = "PI")]
    public class CommitmentsModel : PageModel
    {
        [BindProperty]
        public AcademiaCommitmentsViewModel Data { get; set; }

        [FromQuery(Name = "cid")]
        public int StudCommitmentId { get; set; } = 0;

        private readonly IMediator _mediator;
        public CommitmentsModel(IMediator mediator) => _mediator = mediator;


        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { ID = User.GetUserId(), CommitID = StudCommitmentId });
        }

        public async Task<FileResult> OnGetExcelView()
        {
            var file = await _mediator.Send(new QueryExcelView() { ID = User.GetUserId() });
            return file;
        }


        public class Query : IRequest<AcademiaCommitmentsViewModel>
        {
            public int ID { get; set; }
            public int CommitID { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, AcademiaCommitmentsViewModel>
        {
            private readonly ScholarshipForServiceContext _efDB;

            public QueryHandler(ScholarshipForServiceContext db)
            {
                _efDB = db;
            }
            public async Task<AcademiaCommitmentsViewModel> Handle(Query request, CancellationToken cancellationToken)
            {
                AcademiaCommitmentsViewModel model = new();
                model.Commitments = new();
                var institutionID = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == request.ID)
                    .Select(m => m.InstitutionID).FirstOrDefaultAsync();

              

				var allInstitutionIds = await _efDB.Institutions.Where(m => m.InstitutionId.Equals(institutionID) || m.ParentInstitutionID.Equals(institutionID)).Select(m => m.InstitutionId).ToListAsync();
				if (request.CommitID > 0)
                {
                    var previousCommitmentReviewed = await _efDB.StudentCommitments
                        .Where(m => m.StudentCommitmentId == request.CommitID)
                        .Select(m => new
                        {
                            Name = $"{m.Student.FirstName} {m.Student.LastName}",
                            Status = m.PIRecommendation,
                            Agency = m.Agency.Name

                        }).FirstOrDefaultAsync();
                    model.AlertDisplay = $"Recommendation for Commitment from {previousCommitmentReviewed.Name} for agency {previousCommitmentReviewed.Agency} has been submitted. The Program Office will review.";
                }

                var data = await _efDB.StudentCommitments.Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
				   .Where(m => allInstitutionIds.Contains(m.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId.Value))
				   .Where(m => m.CommitmentStatus.Code != CommitmentProcessConst.Incomplete)
                   .Where(m => m.Agency.IsDisabled == false)
                   .Where(m => !m.IsDeleted)
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
                       Status = s.CommitmentStatus.StudentDisplay,
                       StatusDescription = s.CommitmentStatus.Description
                   })
                   .OrderByDescending(m => m.CommitmentID)
                   .ToListAsync();

                foreach (var item in data)
                {
                    AcademiaCommitmentsViewModel.CommitmentItem c = new();
                    c.StudentName = item.Name;
                    c.CommitmentType = item.CommitmentType;
                    c.AgencyName = item.Agency;
                    c.AgencyType = item.AgencyType;
                    c.JobTitle = item.JobTitle;
                    c.StartDate = item.StartDate;
                    c.Status = item.Status;
                    c.StudentID = item.StudentID;
                    c.CommitmentID = item.CommitmentID;
                    c.StatusDescription = item.StatusDescription;
                    model.Commitments.Add(c);
                }
                return model;
            }
        }

        public class QueryExcelView : IRequest<FileStreamResult>
        {
            public int ID { get; set; }
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

                var institutionID = await _efDB.AcademiaUsers.Where(m => m.AcademiaUserId == request.ID)
                    .Select(m => m.InstitutionID).FirstOrDefaultAsync();


                var data = await _efDB.StudentCommitments.Include(m => m.Student).ThenInclude(m => m.StudentInstitutionFundings).ThenInclude(m => m.Institution)
                    .Where(m => m.Student.StudentInstitutionFundings.FirstOrDefault().InstitutionId == institutionID)
                    .Where(m => m.CommitmentStatus.Code != CommitmentProcessConst.Incomplete)
                    .Where(m => m.Agency.IsDisabled == false)
                    .Where(m => m.IsDeleted == false)
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
                        Status = s.CommitmentStatus.StudentDisplay,
                        StatusDescription = s.CommitmentStatus.Description
                    })
                    .OrderByDescending(m => m.CommitmentID)
                    .ToListAsync();

                var model = new AcademiaCommitmentListExcelViewModel();
                model.Commitments = new();
                foreach (var d in data)
                {
                    model.Commitments.Add(new AcademiaCommitmentListExcelViewModel.CommitmentListItem()
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
                    worksheet.Cells["C1"].Value = "Principal Investigator (Academia) Commitment List";
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
