using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.Models;
using OPM.SFS.Web.SharedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Pages.Student
{
    [Authorize(Roles = "ST")]
    [ValidateAntiForgeryToken]
    public class BackgroundInfoModel : PageModel
    {
        [BindProperty]
        public BackgroundInfoViewModel Data { get; set; }

        [FromQuery(Name = "s")]
        public string IsSuccess { get; set; }

        [FromQuery(Name = "i")]
        public string ShowIncompleteWarning { get; set; }

        private readonly IMediator _mediator;

        public BackgroundInfoModel(IMediator mediator) => _mediator = mediator;



        public async Task OnGetAsync()
        {
            Data = await _mediator.Send(new Query() { StudentId = User.GetUserId() });
            if (!string.IsNullOrWhiteSpace(IsSuccess) && IsSuccess == "true") Data.ShowSuccessMessage = true;
            if (!string.IsNullOrWhiteSpace(ShowIncompleteWarning) && ShowIncompleteWarning == "true") Data.ShowIncompleteWarning = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Data = await _mediator.Send(new Query() { StudentId = User.GetUserId() });
                return Page();
            }
            var response = await _mediator.Send(new Command() { Model = Data, StudentId = User.GetUserId() });
            return RedirectToPage("Dashboard");
        }

        public class BackgroundInfoDataValidator : AbstractValidator<BackgroundInfoViewModel>
        {
            public BackgroundInfoDataValidator()
            {
                RuleFor(request => request.Gender).NotEmpty().WithMessage("Gender required");
                RuleFor(request => request.Ethnicity).NotEmpty().NotEqual(0).WithMessage("Ethicity required");
                RuleFor(request => request.HighSchoolState).NotEmpty().NotEqual(0).WithMessage("High School State required");
                RuleFor(request => request.HighSchoolType).NotEmpty().NotEqual(0).WithMessage("High School Type required");
                RuleFor(request => request.IsArmedForces).NotEmpty().NotEqual("").WithMessage("Current Armed Forces required");
                RuleFor(request => request.YearsInSecurityPosition).NotNull().WithMessage("Years of Employment required");
                RuleFor(model => model.SelectedRace).NotEmpty().WithMessage("Race is required");
            }
        }

        public class Query : IRequest<BackgroundInfoViewModel>
        {
            public int StudentId { get; set; }

        }

        public class QueryHandler : IRequestHandler<Query, BackgroundInfoViewModel>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public QueryHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }

            public async Task<BackgroundInfoViewModel> Handle(Query request, CancellationToken cancellationToken)
            {

                BackgroundInfoViewModel model = new();
                model.GenderList = new SelectList(await _cache.GetGenderAsync(), nameof(Gender.GenderId), nameof(Gender.GenderName));
                model.EthnicityList = new SelectList(await _cache.GetEthnicityAsync(), nameof(Ethnicity.EthnicityId), nameof(Ethnicity.EthnicityName));
                model.HighSchoolTypeList = new SelectList(await _cache.GetSchoolTypesAsync(), nameof(SchoolType.SchoolTypeID), nameof(SchoolType.SchoolTypeName));
                model.HighSchoolStateList = new SelectList(await _cache.GetStatesAsync(), nameof(State.StateId), nameof(State.Name));

                List<BackgroundInfoViewModel.ArmedForceOptions> armedForceOptions = new();
                armedForceOptions.Add(new BackgroundInfoViewModel.ArmedForceOptions() { ID = "Yes", Name = "Yes" });
                armedForceOptions.Add(new BackgroundInfoViewModel.ArmedForceOptions() { ID = "No", Name = "No" });
                armedForceOptions.Add(new BackgroundInfoViewModel.ArmedForceOptions() { ID = "Not Reported", Name = "I wish to decline to respond" });
                model.IsArmedForcesList = new SelectList(armedForceOptions, nameof(BackgroundInfoViewModel.ArmedForceOptions.ID), nameof(BackgroundInfoViewModel.ArmedForceOptions.Name));
               
                if(request.StudentId > 0)
                {
                    var studentData = await _db.Students
                        .Where(m => m.StudentId == request.StudentId)
                        .Include(m => m.Gender)
                        .Include(m => m.StudentRaces)
                        .Include(m => m.Ethnicity)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

                    if(studentData != null)
                    {
                        
                        if(studentData.StudentRaces is not null && studentData.StudentRaces.Count > 0)
                        {
                            model.SelectedRace = new();
                            foreach(var race in studentData.StudentRaces)
                            {
                                model.SelectedRace.Add(race.RaceId);
                            }
                        }
                        model.Ethnicity = studentData.EthnicityId.HasValue ? studentData.EthnicityId.Value : 0;
                        model.HighSchoolState = studentData.HighSchoolStateStateId.HasValue ? studentData.HighSchoolStateStateId.Value : 0;
                        model.HighSchoolType = studentData.SchoolTypeID.HasValue ? studentData.SchoolTypeID.Value : 0;
                        model.IsArmedForces = studentData.IsMilitary;
                        model.YearsInSecurityPosition = studentData.YearsInSecurityPosition.HasValue ? studentData.YearsInSecurityPosition.Value : 0;
                        model.Gender = studentData.GenderId.HasValue ? studentData.GenderId.Value : 0;


                        var races = await _cache.GetRacesAsync();
                        model.RaceOptions = new();
                        foreach (var r in races)
                        {
                            if (studentData.StudentRaces.Where(m => m.RaceId  == r.RaceId).Any())
                            {
                                model.RaceOptions.Add(new BackgroundInfoViewModel.RaceOption()
                                {
                                    ID = r.RaceId,
                                    Name = r.RaceName,
                                    Selected = true
                                });
                            }
                            else
                            {
                                model.RaceOptions.Add(new BackgroundInfoViewModel.RaceOption()
                                {
                                    ID = r.RaceId,
                                    Name = r.RaceName,
                                    Selected = false
                                });
                            }
                        }
                    }
                }

                return model;
                
            }
        }


        public class Command : IRequest<bool>
        {
            public BackgroundInfoViewModel Model { get; set; }
            public int StudentId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, bool>
        {
            private readonly ScholarshipForServiceContext _db;
            private readonly ICacheHelper _cache;

            public CommandHandler(ScholarshipForServiceContext db, ICacheHelper cache)
            {
                _db = db;
                _cache = cache;
            }
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var  studentData = await _db.Students
                    .Where(m => m.StudentId == request.StudentId)
                    .Include(m => m.StudentRaces).FirstOrDefaultAsync();

                if (studentData is not null)
                {
                    studentData.GenderId = request.Model.Gender;
                    studentData.EthnicityId = request.Model.Ethnicity;
                    studentData.HighSchoolStateStateId = request.Model.HighSchoolState;
                    studentData.SchoolTypeID = request.Model.HighSchoolType;
                    studentData.IsMilitary = request.Model.IsArmedForces;
                    studentData.YearsInSecurityPosition = request.Model.YearsInSecurityPosition;
                    studentData.LastUpdated = DateTime.UtcNow;

                    //Races remove all and re-add
                    var currentRaces = _db.StudentRaces.Where(m => m.StudentId == request.StudentId).ToList();
                    if (currentRaces.Any())
                    {
                        _db.StudentRaces.RemoveRange(currentRaces);
                    }
                    foreach (var r in request.Model.SelectedRace)
                    {
                        studentData.StudentRaces.Add(new StudentRace() { RaceId = r, DateInserted = DateTime.UtcNow });
                    }
                }

                await _db.SaveChangesAsync();
                return true;

            }
        }


    }


}
