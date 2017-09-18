using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RateBlog.Data;
using RateBlog.Helper;
using RateBlog.Models;
using RateBlog.Models.VoteViewModels;
using RateBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    [Authorize(Roles = "Influencer")]
    public class VotesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public VotesController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var votes = _dbContext.Votes.Where(x => x.InfluencerId == user.Id).Include(x => x.VoteQuestions).ThenInclude(x => x.VoteAnswers);

            var model = new IndexViewModel()
            {
                Votes = votes,
                InfluencerId = user.Id
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if(model.FollowerQuestions.Where(x => x == string.Empty || x == null).Count() > 3)
                {
                    TempData["Error"] = "Du skal vælge mindst 2 svarmuligheder"; 
                    return View(model); 
                }

                // Gør alle andre inaktive...
                var activeVotes = _dbContext.Votes.Where(x => x.InfluencerId == user.Id && x.Active == true); 

                foreach(var v in activeVotes)
                {
                    v.Active = false;
                    _dbContext.Votes.Update(v); 
                }

                var vote = new Vote()
                {
                    InfluencerId = user.Id,
                    Title = model.Question,
                    Active = true,
                    DateTime = DateTime.Now
                };

                vote.VoteQuestions = new List<VoteQuestion>();

                foreach (var v in model.FollowerQuestions)
                {
                    if (!string.IsNullOrEmpty(v))
                        vote.VoteQuestions.Add(new VoteQuestion() { Question = v, VoteId = vote.Id });
                }

                await _dbContext.Votes.AddAsync(vote);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var message = allErrors.First();
            TempData["Error"] = message.ErrorMessage;

            return View(model); 
        }

        [HttpGet]
        public async Task<IActionResult> Vote(string id)
        {
            var vote = await _dbContext.Votes.Include(x => x.VoteQuestions).SingleOrDefaultAsync(x => x.Id == id);

            return View(vote);
        }

        [HttpGet]
        public async Task<JsonResult> GetVoteData(string id)
        {
            var vote = await _dbContext.Votes
                .Include(x => x.VoteQuestions)
                .ThenInclude(x => x.VoteAnswers)
                .ThenInclude(x => x.ApplicationUser)
                .SingleOrDefaultAsync(x => x.Id == id);

            var males = vote.VoteQuestions.Select(x => x.VoteAnswers.Select(i => i.ApplicationUser).Where(p => p.Gender == "male").Count()).Sum();
            var females = vote.VoteQuestions.Select(x => x.VoteAnswers.Select(i => i.ApplicationUser).Where(p => p.Gender == "female").Count()).Sum();

            var users = vote.VoteQuestions.Select(x => x.VoteAnswers.Select(i => i.ApplicationUser)).SelectMany(x => x);

            var ageGroup = new int[5];
            ageGroup[0] = users.Where(x => GetAge(x.BirthDay) < 13).Count();
            ageGroup[1] = users.Where(x => GetAge(x.BirthDay) >= 13 && GetAge(x.BirthDay) <= 17).Count();
            ageGroup[2] = users.Where(x => GetAge(x.BirthDay) >= 18 && GetAge(x.BirthDay) <= 24).Count();
            ageGroup[3] = users.Where(x => GetAge(x.BirthDay) >= 25 && GetAge(x.BirthDay) <= 34).Count();
            ageGroup[4] = users.Where(x => GetAge(x.BirthDay) >= 35).Count();

            var answerData = vote.VoteQuestions.Select(x => new VoteData()
            {
                Title = x.Question,
                Count = x.VoteAnswers.Where(p => p.VoteQuestionId == x.Id).Count()
            });

            var sum = answerData.Select(x => x.Count).Sum();

            return Json(new VoteStatisticData() { Males = males, Females = females, AgeGroup = ageGroup, AnswerData = answerData, AnswerSum = sum });
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateVote(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var vote = await _dbContext.Votes.SingleOrDefaultAsync(x => x.Id == id);
            vote.Active = false;
            _dbContext.Votes.Update(vote);
            await _dbContext.SaveChangesAsync();
            TempData["Success"] = "Du har deaktiveret din survey!"; 
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public async Task<IActionResult> ActivateVote(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var vote = await _dbContext.Votes.SingleOrDefaultAsync(x => x.Id == id);
            vote.Active = true;
            _dbContext.Votes.Update(vote);

            var activeVotes = _dbContext.Votes.Where(x => x.InfluencerId == user.Id && x.Active == true);

            foreach (var v in activeVotes)
            {
                v.Active = false;
                _dbContext.Votes.Update(v);
            }

            await _dbContext.SaveChangesAsync();
            TempData["Success"] = "Du har aktiveret dit survey!";
            return RedirectToAction("Index");
        }

        private int GetAge(DateTime bornDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - bornDate.Year;
            if (bornDate > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
