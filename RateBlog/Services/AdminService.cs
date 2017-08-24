using Microsoft.AspNetCore.Identity;
using RateBlog.Models;
using RateBlog.Repository;
using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(IRepository<Feedback> feedbackRepo, UserManager<ApplicationUser> userManager)
        {
            _feedbackRepo = feedbackRepo;
            _userManager = userManager;
        }

        public double GetAverageKvalitetForFiltered(IEnumerable<Feedback> JoinedFeedbackUser)
        {
            double avgFeedback = 0;
            foreach (var i in JoinedFeedbackUser)
            {
                avgFeedback = avgFeedback + i.Kvalitet;
            }
            return avgFeedback / JoinedFeedbackUser.Count();
        }

        public double GetAverageOpførelseForFiltered(IEnumerable<Feedback> JoinedFeedbackUser)
        {
            double avgFeedback = 0;
            foreach (var i in JoinedFeedbackUser)
            {
                avgFeedback = avgFeedback + i.Opførsel;
            }
            return avgFeedback / JoinedFeedbackUser.Count();
        }

        public double GetAverageTroværdighedForFiltered(IEnumerable<Feedback> JoinedFeedbackUser)
        {
            double avgFeedback = 0;
            foreach (var i in JoinedFeedbackUser)
            {
                avgFeedback = avgFeedback + i.Troværdighed;
            }
            return avgFeedback / JoinedFeedbackUser.Count();
        }

        public double GetAverageInteraktionForFiltered(IEnumerable<Feedback> FilterdFeedback)
        {
            double avgFeedback = 0;
            foreach (var i in FilterdFeedback)
            {
                avgFeedback = avgFeedback + i.Interaktion;
            }
            return avgFeedback / FilterdFeedback.Count();
        }

        public double GetNumberFeedback(IEnumerable<Feedback> FilterdFeedback)
        {
            return FilterdFeedback.Count();
        }

        public double GetNpsForFilter(IEnumerable<Feedback> FilterdFeedback)
        {
            double Lower = 0;
            double Upper = 0;
            foreach (var i in FilterdFeedback)
            {
                if (i.Anbefaling >= 0 && i.Anbefaling <= 6)
                {
                    Lower++;
                }
                if (i.Anbefaling >= 9 && i.Anbefaling <= 10)
                {
                    Upper++;
                }

            }
            return Math.Round((((Upper / (Upper + Lower)) - (Lower / (Upper + Lower))) * 100), 0);
        }

        public IEnumerable<ApplicationUser> GetGenderFilterdUsers(string Køn, IEnumerable<ApplicationUser> UsersAlreadyFilterd)
        {
            if (!(Køn == "both"))
            {
                if (UsersAlreadyFilterd == null)
                {
                    return _userManager.Users.Where(x => x.Gender == Køn);
                }
                else
                {
                    return UsersAlreadyFilterd.Where(x => x.Gender == Køn);
                }
            }
            return UsersAlreadyFilterd ?? _userManager.Users;
        }

        public IEnumerable<ApplicationUser> GetAgeGroupFilterdUsers(List<int> AldersGruppe, IEnumerable<ApplicationUser> UsersAlreadyFilterd)
        {
            #region Alders gruppe definition
            //bestemmer Aldersgrupperne
            int[,] AldersGrupDiff = new int[5, 2];
            //Gruppe0 alder 0-18
            AldersGrupDiff[0, 0] = 0;
            AldersGrupDiff[0, 1] = 18;
            //Gruppe1 Alder 19-24 osv.
            AldersGrupDiff[1, 0] = 19;
            AldersGrupDiff[1, 1] = 24;
            AldersGrupDiff[2, 0] = 25;
            AldersGrupDiff[2, 1] = 30;
            AldersGrupDiff[3, 0] = 31;
            AldersGrupDiff[3, 1] = 39;
            AldersGrupDiff[4, 0] = 40;
            AldersGrupDiff[4, 1] = 1000;
            #endregion

            #region If everything is chosen return list(for minimal load time for first page load)
            if (AldersGruppe.Count == AldersGrupDiff.Length / 2) return UsersAlreadyFilterd;
            #endregion

            IEnumerable<ApplicationUser>[] AgeGrups = new IEnumerable<ApplicationUser>[AldersGrupDiff.Length / 2];
            List<ApplicationUser> Result = new List<ApplicationUser>();

            for (int x = 0; x < AldersGruppe.Count; x++)
            {
                for (int y = 0; y < AldersGrupDiff.Length / 2; y++)
                    if (AldersGruppe[x] == y)
                    {
                        AgeGrups[x] = (from users in UsersAlreadyFilterd ?? _userManager.Users
                                       where DateTime.Now.Year - users.BirthDay.Year >= AldersGrupDiff[y, 0]
                                          && DateTime.Now.Year - users.BirthDay.Year <= AldersGrupDiff[y, 1]
                                       select users).ToList();
                    }
                foreach (var z in AgeGrups[x])
                {
                    Result.Add(z);
                }
            }
            return Result;
        }

        public IEnumerable<Feedback> GetPlatformFilterdFeedbacks(string Id, List<int> Platform, IEnumerable<Feedback> FeedbackAllreadyFilterd)
        {
            #region If everything is chosen return list(for minimal load time for first page load) remember to change number if platform is expanded
            if (Platform.Count == 6) return FeedbackAllreadyFilterd;
            #endregion

            IEnumerable<Feedback>[] Platforms = new IEnumerable<Feedback>[Platform.Count];
            List<Feedback> Result = new List<Feedback>();
            var UsersByInfluenter = FeedbackAllreadyFilterd ?? _feedbackRepo.GetAll().Where(x => x.InfluenterId == Id);

            #region Platform sort logic
            for (int x = 0; x < Platform.Count; x++)
            {
                if (Platform[x] == 0)
                {
                    Platforms[x] = from users in UsersByInfluenter
                                   where users.BasedOnTwitter == true
                                   select users;
                }
                if (Platform[x] == 1)
                {
                    Platforms[x] = from users in UsersByInfluenter
                                   where users.BasedOnYoutube == true
                                   select users;
                }
                if (Platform[x] == 2)
                {
                    Platforms[x] = from users in UsersByInfluenter
                                   where users.BasedOnTwitch == true
                                   select users;
                }
                if (Platform[x] == 3)
                {
                    Platforms[x] = from users in UsersByInfluenter
                                   where users.BasedOnInstagram == true
                                   select users;
                }
                if (Platform[x] == 4)
                {
                    Platforms[x] = from users in UsersByInfluenter
                                   where users.BasedOnSnapchat == true
                                   select users;
                }
                if (Platform[x] == 5)
                {
                    Platforms[x] = from users in UsersByInfluenter
                                   where users.BasedOnFacebook == true
                                   select users;
                }
                if (Platform[x] == 6)
                {
                    Platforms[x] = from users in UsersByInfluenter
                                   where users.BasedOnWebsite == true
                                   select users;
                }
                foreach (var z in Platforms[x])
                {
                    Result.Add(z);
                }
            }
            #endregion

            Result = Result.GroupBy(x => x.Id)
            .Select(grp => grp.First()).ToList();

            return Result;
        }

        public double GetFilterdUniqueUsers(IEnumerable<Feedback> FeedbackAllreadyFilterd)
        {
            return FeedbackAllreadyFilterd.GroupBy(x => x.ApplicationUserId).Select(grp => grp.First()).Count();
        }

        public IEnumerable<Feedback> GetJoinFiltersAndGetFeedbacks(IEnumerable<Feedback> FeedbackAllreadyFilterd, IEnumerable<ApplicationUser> UsersAllreadyFilterd)
        {
            var FilterdFeedbacks = from Feedback in FeedbackAllreadyFilterd
                                   join ApplicationUser in UsersAllreadyFilterd on Feedback.ApplicationUserId equals ApplicationUser.Id
                                   select Feedback;

            return FilterdFeedbacks;
        }

        public IEnumerable<Feedback> GetTheFilterdFeedbacks(string Id, List<int> Platform, List<int> AldersGruppe, string Køn)
        {
            return GetJoinFiltersAndGetFeedbacks(GetPlatformFilterdFeedbacks(Id, Platform, null), GetAgeGroupFilterdUsers(AldersGruppe, GetGenderFilterdUsers(Køn, null)));
        }

        public List<int> GetPlatformToDisplay(string Id)
        {
            #region Slamkode
            List<int> AldersGruppe = new List<int>(new int[] { 0, 1, 2, 3, 4 });
            var allFeedbacksByinfuencer = _feedbackRepo.GetAll().Where(x => x.InfluenterId == Id);
            var FeedbackAllreadyFilterd = GetJoinFiltersAndGetFeedbacks(allFeedbacksByinfuencer, GetAgeGroupFilterdUsers(AldersGruppe, GetGenderFilterdUsers("both", null)));

            int[] Platforms = new int[7];

            List<int> Result = new List<int>();

            Platforms[0] = (from users in FeedbackAllreadyFilterd
                            where users.BasedOnTwitter == true
                            select users).Count();

            Platforms[1] = (from users in FeedbackAllreadyFilterd
                            where users.BasedOnYoutube == true
                            select users).Count();

            Platforms[2] = (from users in FeedbackAllreadyFilterd
                            where users.BasedOnTwitch == true
                            select users).Count();

            Platforms[3] = (from users in FeedbackAllreadyFilterd
                            where users.BasedOnInstagram == true
                            select users).Count();

            Platforms[4] = (from users in FeedbackAllreadyFilterd
                            where users.BasedOnSnapchat == true
                            select users).Count();

            Platforms[5] = (from users in FeedbackAllreadyFilterd
                            where users.BasedOnFacebook == true
                            select users).Count();

            Platforms[6] = (from users in FeedbackAllreadyFilterd
                            where users.BasedOnWebsite == true
                            select users).Count();

            for (int i = 0; i < Platforms.Length; i++)
            {
                Result.Add(Platforms[i]);
            }

            return Result;
            #endregion
        }

        public List<int> GetAgeToDisplay(string Id)
        {
            #region Slamkode

            #region Alders gruppe definition
            //bestemmer Aldersgrupperne
            int[,] AldersGrupDiff = new int[5, 2];
            //Gruppe0 alder 0-18
            AldersGrupDiff[0, 0] = 0;
            AldersGrupDiff[0, 1] = 18;
            //Gruppe1 Alder 19-24 osv.
            AldersGrupDiff[1, 0] = 19;
            AldersGrupDiff[1, 1] = 24;
            AldersGrupDiff[2, 0] = 25;
            AldersGrupDiff[2, 1] = 30;
            AldersGrupDiff[3, 0] = 31;
            AldersGrupDiff[3, 1] = 39;
            AldersGrupDiff[4, 0] = 40;
            AldersGrupDiff[4, 1] = 1000;
            #endregion

            List<int> AldersGruppe = new List<int>(new int[] { 0, 1, 2, 3, 4 });

            IEnumerable<ApplicationUser>[] AgeGrups = new IEnumerable<ApplicationUser>[AldersGrupDiff.Length / 2];

            for (int x = 0; x < AldersGruppe.Count; x++)
            {
                for (int y = 0; y < AldersGrupDiff.Length / 2; y++)
                    if (AldersGruppe[x] == y)
                    {
                        AgeGrups[x] = (from users in _userManager.Users
                                       where DateTime.Now.Year - users.BirthDay.Year >= AldersGrupDiff[y, 0]
                                          && DateTime.Now.Year - users.BirthDay.Year <= AldersGrupDiff[y, 1]
                                       select users).ToList();
                    }
            }

            var UsersAlreadyFilterd = _feedbackRepo.GetAll().Where(x => x.InfluenterId == Id);

            List<int> Result = new List<int>();

            for (int i = 0; i < AldersGruppe.Count(); i++)
            {
                Result.Add(GetJoinFiltersAndGetFeedbacks(UsersAlreadyFilterd, AgeGrups[i]).GroupBy(x => x.Id)
            .Select(grp => grp.First()).ToList().Count());
            }

            return Result;
            #endregion
        }

        public List<int> GetGenderToDisplay(string Id)
        {
            List<int> Result = new List<int>();
            Result.Add(_userManager.Users.Where(x => x.Gender == "female").Count());
            Result.Add(_userManager.Users.Where(x => x.Gender == "male").Count());
            return Result;
        }

        public IEnumerable<Feedback>[] GetFeedbackDifferenceInRatingLast7Days(string Id, int days)
        { //days7 days14 navn givning er fra tidligere og skal overses eller navn ændres
            var days7 = new TimeSpan(days, 0, 0, 0);
            var days14 = new TimeSpan(days*2, 0, 0, 0);

            IEnumerable<Feedback>[] FeedbackGroup = new IEnumerable<Feedback>[2];

            var feedfromfluencer = _feedbackRepo.GetAll().Where(x => x.InfluenterId == Id);

            FeedbackGroup[0] = from feedback in feedfromfluencer
                               where DateTime.Now.Subtract(feedback.FeedbackDateTime).Days <= days7.Days
                               select feedback;

            FeedbackGroup[1] = from feedback in feedfromfluencer
                               where DateTime.Now.Subtract(feedback.FeedbackDateTime).Days > days7.Days &&
                               DateTime.Now.Subtract(feedback.FeedbackDateTime).Days <= days14.Days
                               select feedback;

            return FeedbackGroup;
        }

        public double GetNumberTotalAvg7daysOrProcent(string Id, bool Get7Days, int days)
        {
            var DifferencedFeedback = GetFeedbackDifferenceInRatingLast7Days(Id,days);

            double AvgRating7 = 0;
            foreach (var x in DifferencedFeedback[0])
            {
                AvgRating7 = AvgRating7 + Convert.ToDouble(x.Kvalitet);
                AvgRating7 = AvgRating7 + Convert.ToDouble(x.Troværdighed);
                AvgRating7 = AvgRating7 + Convert.ToDouble(x.Interaktion);
                AvgRating7 = AvgRating7 + Convert.ToDouble(x.Kvalitet);
            }
            AvgRating7 = (AvgRating7 /4) / Convert.ToDouble(DifferencedFeedback[0].Count());

            double AvgRating14 = 0;
            foreach (var x in DifferencedFeedback[1])
            {
                AvgRating14 = AvgRating14 + x.Kvalitet;
                AvgRating14 = AvgRating14 + x.Troværdighed;
                AvgRating14 = AvgRating14 + x.Interaktion;
                AvgRating14 = AvgRating14 + x.Kvalitet;
         
            }
            AvgRating14 = (AvgRating14/4) /  Convert.ToDouble(DifferencedFeedback[1].Count());
            if (Get7Days) return Math.Round(AvgRating7,1);
            return Math.Round(((AvgRating7/AvgRating14)-1)*100, 1);
        }

        public double GetNumberRatingDifference7days(string Id, int days)
        {
            var DifferencedFeedback = GetFeedbackDifferenceInRatingLast7Days(Id,days);
            return DifferencedFeedback[0].Count() - DifferencedFeedback[1].Count();
        }

        public double GetNumberRatingDifferenceProcentage7days(string Id, int days)
        {
            var DifferencedFeedback = GetFeedbackDifferenceInRatingLast7Days(Id,days);
            return Math.Round(((Convert.ToDouble(DifferencedFeedback[0].Count()) / Convert.ToDouble(DifferencedFeedback[1].Count())) - 1) * 100, 1);
        }

        public double GetNumberKvalitetDifference7days(string Id, bool GetDifference, int days)
        {
            var DifferencedFeedback = GetFeedbackDifferenceInRatingLast7Days(Id,days);
            double AvgRating7 = 0;
            foreach (var x in DifferencedFeedback[0])
            {
                AvgRating7 = AvgRating7 + x.Kvalitet;
            }
            AvgRating7 = AvgRating7 / DifferencedFeedback[0].Count();
            double AvgRating14 = 0;
            foreach (var x in DifferencedFeedback[1])
            {
                AvgRating14 = AvgRating14 + x.Kvalitet;
            }
            AvgRating14 = AvgRating14 / DifferencedFeedback[1].Count();
            if(GetDifference)return Math.Round(((AvgRating7/AvgRating14)-1)*100, 1);
            return Math.Round(AvgRating7, 1);
        }

        public double GetNumberInteraktionDifference7days(string Id, bool GetDifference, int days)
        {
            var DifferencedFeedback = GetFeedbackDifferenceInRatingLast7Days(Id,days);
            double AvgRating7 = 0;
            foreach (var x in DifferencedFeedback[0])
            {
                AvgRating7 = AvgRating7 + x.Interaktion;
            }
            AvgRating7 = AvgRating7 / DifferencedFeedback[0].Count();
            double AvgRating14 = 0;
            foreach (var x in DifferencedFeedback[1])
            {
                AvgRating14 = AvgRating14 + x.Interaktion;
            }
            AvgRating14 = AvgRating14 / DifferencedFeedback[1].Count();
            if (GetDifference) return Math.Round(((AvgRating7 / AvgRating14) - 1) * 100, 1);
            return Math.Round(AvgRating7, 1);
        }

        public double GetNumberTroværdighedDifference7days(string Id, bool GetDifference, int days)
        {
            var DifferencedFeedback = GetFeedbackDifferenceInRatingLast7Days(Id, days);
            double AvgRating7 = 0;
            foreach (var x in DifferencedFeedback[0])
            {
                AvgRating7 = AvgRating7 + x.Troværdighed;
            }
            AvgRating7 = AvgRating7 / DifferencedFeedback[0].Count();
            double AvgRating14 = 0;
            foreach (var x in DifferencedFeedback[1])
            {
                AvgRating14 = AvgRating14 + x.Troværdighed;
            }
            AvgRating14 = AvgRating14 / DifferencedFeedback[1].Count();
            if (GetDifference) return Math.Round(((AvgRating7 / AvgRating14) - 1) * 100, 1);
            return Math.Round(AvgRating7, 1);
        }

        public double GetNumberOpførselDifference7days(string Id, bool GetDifference, int days)
        {
            var DifferencedFeedback = GetFeedbackDifferenceInRatingLast7Days(Id,days);
            double AvgRating7 = 0;
            foreach (var x in DifferencedFeedback[0])
            {
                AvgRating7 = AvgRating7 + x.Opførsel;
            }
            AvgRating7 = AvgRating7 / DifferencedFeedback[0].Count();
            double AvgRating14 = 0;
            foreach (var x in DifferencedFeedback[1])
            {
                AvgRating14 = AvgRating14 + x.Opførsel;
            }
            AvgRating14 = AvgRating14 / DifferencedFeedback[1].Count();
            if(GetDifference) return Math.Round(((AvgRating7 / AvgRating14) - 1) * 100, 1);
            return Math.Round(AvgRating7, 1);
        }
        
        public IEnumerable<Feedback>[] GetReturningAndUniqueFeedbacks(IEnumerable<Feedback> DaysFilterdFeedback)
        {
            var FeedbacksFromInfluenter = DaysFilterdFeedback.ToList();
             FeedbacksFromInfluenter= FeedbacksFromInfluenter.OrderBy(x => x.ApplicationUserId).ToList();
            List<Feedback>[] UniqeReturningFeedback = new List<Feedback>[2];
            List<Feedback> UniqeFeedback = new List<Feedback>();
            List<Feedback> ReturningFeedback = new List<Feedback>();
            bool Returning = false;
            for (int x = 0; x < FeedbacksFromInfluenter.Count(); x++)
            {
                Returning = false;
                if (x == FeedbacksFromInfluenter.Count() - 1 || x == 0)
                {
                    if (x == FeedbacksFromInfluenter.Count() - 1)
                    {
                        if (FeedbacksFromInfluenter.ElementAt(x).ApplicationUserId == FeedbacksFromInfluenter.ElementAt(x - 1).ApplicationUserId)
                        {
                            UniqeFeedback.Add(FeedbacksFromInfluenter[x]);
                            Returning = true;
                        }
                    }
                    if (x == 0)
                    {
                        if (FeedbacksFromInfluenter.ElementAt(x).ApplicationUserId == FeedbacksFromInfluenter.ElementAt(x + 1).ApplicationUserId)
                        {
                            UniqeFeedback.Add(FeedbacksFromInfluenter[x]);
                            Returning = true;
                        }
                    }
                }
                else
                {
                    if ((FeedbacksFromInfluenter.ElementAt(x).ApplicationUserId == FeedbacksFromInfluenter.ElementAt(x + 1).ApplicationUserId) || (FeedbacksFromInfluenter.ElementAt(x).ApplicationUserId == FeedbacksFromInfluenter.ElementAt(x - 1).ApplicationUserId))
                    {
                        UniqeFeedback.Add(FeedbacksFromInfluenter[x]);
                        Returning = true;
                    }
                }
                if(Returning == false)
                {
                    ReturningFeedback.Add(FeedbacksFromInfluenter[x]);
                }
            }

            UniqeReturningFeedback[1] = UniqeFeedback;
            UniqeReturningFeedback[0] = ReturningFeedback;
            return UniqeReturningFeedback;
        }

        public double UniqeUsersComparedToDays(string Id, int days, int timeGroup)
        {
            
            return GetReturningAndUniqueFeedbacks(GetFeedbackDifferenceInRatingLast7Days(Id, days)[timeGroup])[0].Count();
        }

        public double ReturningUsersComparedToDays(string Id, int days, int timeGroup)
        {
            return GetReturningAndUniqueFeedbacks(GetFeedbackDifferenceInRatingLast7Days(Id, days)[timeGroup])[1].Count();

        }
    }
}