using CIPlatform.DataModels;
using CIPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace CIPlatform.Controllers
{
    public class MissionController : Controller
    {
        public CiPlatformContext _db = new CiPlatformContext();

        #region Mission List Init
        [CheckSession]
        public IActionResult MissionListing(int pg = 1)
        {
            MissionListingModel model = new MissionListingModel();

            #region Fill Country Drop-down
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.Countries.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            model.countrys = list;
            #endregion Fill Country Drop-down

            #region Fill City Drop-down
            List<SelectListItem> list3 = new List<SelectListItem>();
            var temp3 = _db.Cities.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp3)
            {
                list3.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            model.citys = list3;
            #endregion Fill Country Drop-down

            #region Fill Theme Drop-down
            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp1 = _db.Themes.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp1)
            {
                list1.Add(new SelectListItem() { Text = item.Title, Value = item.ThemeId.ToString() });
            }
            model.themes = list1;
            #endregion Fill Theme Drop-down

            #region Fill Skill Drop-Down
            List<SelectListItem> list2 = new List<SelectListItem>();
            var temp2 = _db.Skills.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp2)
            {
                list2.Add(new SelectListItem() { Text = item.SkillName, Value = item.SkillId.ToString() });
            }
            model.skills = list2;
            #endregion Fill Skill Drop-Down

            var cardData = new List<MissionCardModel>();

            var tempMission = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1).AsEnumerable().ToList();
            model.TotalMission = tempMission.Count();
            foreach (var item in tempMission)
            {
                cardData.Add(CreateCard(item));
            }
            model.missionsCard = cardData;

            const int pageSize = 9;
            int recsCount = tempMission.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = cardData.Skip(recSkip).Take(pager.PageSize).ToList();
            model.missionsCard = data;
            this.ViewBag.Pager = pager;

            return View(model);
        }
        #endregion Mission List Init

        #region Mission Filter
        [HttpPost]
        public PartialViewResult Filter(List<int>? CountryId, List<int>? CityId, List<int>? ThemeId, List<int>? SkillId, string? searchText, int? sort, int pg = 1)
        {
            var cardData = new List<MissionCardModel>();
            var tempMission = new List<Mission>();

            #region Search

            if (searchText != null)
            {
                var missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1 && x.Title.Contains(searchText)).AsEnumerable().ToList();
                foreach (var m in missions)
                {
                    bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                    if (t == false)
                    {
                        tempMission.Add(m);
                    }
                }
            }

            #endregion Search

            #region Filter Country
            if (CountryId.Count != 0)
            {
                foreach (var n in CountryId)
                {
                    var missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1 && x.CountryId == n).AsEnumerable().ToList();
                    foreach (var m in missions)
                    {
                        bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                        if (t == false)
                        {
                            tempMission.Add(m);
                        }
                    }
                }
            }
            #endregion Filter Country

            #region Filter City
            if (CityId.Count != 0)
            {
                foreach (var n in CityId)
                {
                    var missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1 && x.CityId == n).AsEnumerable().ToList();
                    foreach (var m in missions)
                    {
                        bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                        if (t == false)
                        {
                            tempMission.Add(m);
                        }
                    }
                }
            }
            #endregion Filter City

            #region FIlter Theme
            if (ThemeId.Count != 0)
            {
                foreach (var n in ThemeId)
                {
                    var missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1 && x.ThemeId == n).AsEnumerable().ToList();

                    foreach (var m in missions)
                    {
                        bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                        if (t == false)
                        {
                            tempMission.Add(m);
                        }
                    }
                }
            }
            #endregion FIlter Theme

            #region Filter Skill
            if (SkillId.Count != 0)
            {
                foreach (var n in SkillId)
                {
                    var missions = new List<Mission>();
                    var missionList = _db.MissionSkills.Where(x => x.SkillId == n).Select(m => m.MissionId).AsEnumerable().ToList();
                    foreach (var mission in missionList)
                    {
                        missions.Add(_db.Missions.FirstOrDefault(x => x.MissionId == mission));
                    }
                    foreach (var m in missions)
                    {
                        bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                        if (t == false)
                        {
                            tempMission.Add(m);
                        }
                    }
                }
            }
            #endregion Filter Skill

            #region Default Mission
            if (CountryId.Count == 0 && CityId.Count == 0 && ThemeId.Count == 0 && SkillId.Count == 0 && searchText == null)
            {
                tempMission = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1).AsEnumerable().ToList();
            }
            #endregion Default Mission

            #region Create Card
            foreach (var item in tempMission)
            {
                cardData.Add(CreateCard(item));
            }
            #endregion Create Card

            #region Sort Data
            if (sort != null)
            {
                switch (sort)
                {
                    case 1:
                        cardData = cardData.OrderByDescending(x => x.mission.CreatedAt).ToList();
                        break;
                    case 2:
                        cardData = cardData.OrderBy(x => x.mission.CreatedAt).ToList();
                        break;
                    case 3:
                        cardData = cardData.OrderBy(x => x.seatsLeft).ToList();
                        break;
                    case 4:
                        cardData = cardData.OrderByDescending(x => x.seatsLeft).ToList();
                        break;
                    case 5:
                        cardData = cardData.OrderByDescending(x => x.favMission).ToList();
                        break;
                    case 6:
                        cardData = cardData.OrderByDescending(x => x.mission.Deadline).ToList();
                        break;
                }
            }
            #endregion Sort Data

            const int pageSize = 9;
            int recsCount = tempMission.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = cardData.Skip(recSkip).Take(pager.PageSize).ToList();
            cardData = data;
            this.ViewBag.Pager = pager;

            return PartialView("_MissionGridPartial", cardData);
        }
        #endregion Mission Filter

        #region Fav Mission
        [HttpPost]
        public JsonResult FavMission(int id, int fav)
        {
            if (fav == 1)
            {
                var favMission = new FavouriteMission();
                favMission.MissionId = id;
                favMission.UserId = long.Parse(HttpContext.Session.GetString("UserId"));
                _db.FavouriteMissions.Add(favMission);
                _db.SaveChanges();
            }
            else
            {
                var favMission = _db.FavouriteMissions.FirstOrDefault(x => x.MissionId == id && x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.DeletedAt == null);
                favMission.DeletedAt = DateTime.Now;
                _db.FavouriteMissions.Update(favMission);
                _db.SaveChanges();
            }

            return Json("True");
        }
        #endregion Fav Mission

        #region Suggest CoWorker
        [HttpGet]
        public IActionResult SuggestCoWorker(long id)
        {
            var mission = new Mission();
            mission.MissionId = id;
            return PartialView("_CoWorkerPartial", mission);
        }

        [HttpPost]
        public IActionResult SuggestCoWorker(string WorkerEmail, Mission model)
        {
            if (WorkerEmail != null)
            {
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == model.MissionId);

                #region Send Mail
                var mailBody = "<h1>" + HttpContext.Session.GetString("UserName") + " Suggest Mission : " + mission.Title + " to You</h1><br><h2><a href='https://localhost:7227/Mission/MisionDetail?id= " + model.MissionId + "'>Go Website</a></h2>";

                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("vasudedakiya3@gmail.com"));
                email.To.Add(MailboxAddress.Parse(WorkerEmail));
                email.Subject = "Co-Worker Suggestion";
                email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("vasudedakiya3@gmail.com", "whdatdbclkgporxj");
                smtp.Send(email);
                smtp.Disconnect(true);
                #endregion Send Mail
            }

            return RedirectToAction("MissionListing", "Mission");
        }
        #endregion Suggest CoWorker

        #region Apply Mission
        [HttpPost]
        public JsonResult ApplyMission(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                return Json("You have to login first");
            }
            if (_db.MissionApplications.FirstOrDefault(x => x.MissionId == id && x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.ApprovalStatus == 1) != null)
            {
                return Json("You are alrady Part of mission");
            }
            else if (_db.MissionApplications.FirstOrDefault(x => x.MissionId == id && x.UserId == int.Parse(HttpContext.Session.GetString("UserId"))) != null)
            {
                return Json("You alrady applyed in this mission");
            }
            else
            {
                var missionApplication = new MissionApplication();
                missionApplication.MissionId = id;
                missionApplication.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
                missionApplication.ApprovalStatus = 0;
                missionApplication.AppliedAt = DateTime.Now;
                _db.MissionApplications.Add(missionApplication);
                _db.SaveChanges();
                return Json("Applied Sucessfully");
            }
        }
        #endregion Apply Mission

        #region LogOut
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");

            return RedirectToAction("Login", "Login");
        }
        #endregion LogOut

        #region Mission Detail Page
        public IActionResult MisionDetail(int id)
        {
            var missionDetail = new MissionDetailModel();
            var item = _db.Missions.FirstOrDefault(x => x.MissionId == id);
            var userId = long.Parse(HttpContext.Session.GetString("UserId"));
            missionDetail.missionCard = CreateCard(item);
            missionDetail.missionId = id;
            missionDetail.skills = String.Join(", ", _db.MissionSkills.Where(x => x.MissionId == id && x.DeletedAt == null).Select(x => x.Skill.SkillName).ToList());
            missionDetail.availability = item.Availability == 1 ? "daily" : item.Availability == 2 ? "weekly" : item.Availability == 3 ? "week-end" : "monthly";

            var temp = _db.MissionApplications.Where(x => x.MissionId == id && x.ApprovalStatus == 1).AsEnumerable().ToList();
            var listVol = new List<VolunteerModel>();
            foreach (var u in temp)
            {
                var vol = new VolunteerModel();
                var user = _db.Users.FirstOrDefault(x => x.UserId == u.UserId);
                vol.volunteerName = user.FirstName + " " + user.LastName;
                vol.volunteerImg = user.Avatar != null ? user.Avatar : "~/assets/volunteer1.png";
                listVol.Add(vol);
            }

            missionDetail.volunteers = listVol;

            missionDetail.imgs = _db.MissionMedia.Where(x => x.MissionId == id).AsEnumerable().ToList();

            missionDetail.docs = _db.MissionDocuments.Where(x => x.MissionId == id && x.DeletedAt == null).AsEnumerable().ToList();
            missionDetail.relatedMission = relatedMissions((int)item.CityId, (int)item.CountryId, (int)item.ThemeId);
            var com = _db.Comments.Where(x => x.MissionId == id).AsEnumerable().ToList();
            var coms = new List<CommentModel>();
            foreach (var comment in com)
            {
                var temp1 = new CommentModel();
                var user = _db.Users.FirstOrDefault(x => x.UserId == comment.UserId);
                temp1.commentText = comment.CommentText;
                temp1.img = user.Avatar != null ? user.Avatar : "~/assets/volunteer4.png";
                temp1.img = user.Avatar != null ? user.Avatar : "~/assets/volunteer4.png";
                temp1.name = user.FirstName + " " + user.LastName;
                temp1.createdAt = comment.CreatedAt;
                coms.Add(temp1);
            }
            missionDetail.comments = coms;
            var rate = _db.MissionRatings.FirstOrDefault(x => x.UserId == userId && x.MissionId == id);
            missionDetail.myRating = rate != null ? rate.Rating : 0;

            float totalRate = 0;
            float RateCount = 0;
            var rateRecord = _db.MissionRatings.Where(x => x.MissionId == id).ToList();
            foreach (var i in rateRecord)
            {
                RateCount = RateCount + 1;
                totalRate = totalRate + i.Rating;
            }
            missionDetail.avgRating = totalRate > 0 ? totalRate / RateCount : 0;
            missionDetail.ratingUserCount = (int)RateCount;

            return View(missionDetail);
        }
        #endregion Mission Detail Page

        #region Related Mission
        public List<MissionCardModel> relatedMissions(int cityId, int countryId, int themeId)
        {
            var cardData = new List<MissionCardModel>();
            var tempMission = new List<Mission>();

            #region Filter City
            var missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1 && x.CityId == cityId).AsEnumerable().ToList();
            foreach (var m in missions)
            {
                bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                if (t == false)
                {
                    tempMission.Add(m);
                }
            }
            #endregion Filter City

            #region Filter Country
            if (tempMission.Count < 3)
            {
                missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1 && x.CountryId == countryId).AsEnumerable().ToList();
                foreach (var m in missions)
                {
                    bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                    if (t == false)
                    {
                        tempMission.Add(m);
                    }
                }
            }
            #endregion Filter Country

            #region FIlter Theme
            if (tempMission.Count < 3)
            {
                missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == 1 && x.ThemeId == themeId).AsEnumerable().ToList();

                foreach (var m in missions)
                {
                    bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                    if (t == false)
                    {
                        tempMission.Add(m);
                    }
                }
            }
            #endregion FIlter Theme

            #region Create Card
            for (int i = 0; i < tempMission.Count(); i++)
            {
                cardData.Add(CreateCard(tempMission[i]));
                if (i > 2)
                    break;
            }
            #endregion Create Card
            return cardData;
        }
        #endregion Related Mission

        #region Post Comment
        [HttpPost]
        public JsonResult PostComment(int id, string comment)
        {
            var com = new Comment();
            com.MissionId = id;
            com.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
            com.CommentText = comment;
            com.ApprovalStatus = 1;
            _db.Comments.Add(com);
            _db.SaveChanges();
            return Json("done");
        }
        #endregion Post Comment

        #region Create Card
        public MissionCardModel CreateCard(Mission item)
        {
            var card = new MissionCardModel();
            card.mission = item;
            var img = _db.MissionMedia.FirstOrDefault(x => x.MissionId == item.MissionId);
            card.CardImg = img != null ? img.MediaPath : "~/assets/card_1.png";
            card.seatsLeft = (int)(item.TotalSeat - (_db.MissionApplications.Where(x => x.MissionId == item.MissionId && x.ApprovalStatus == 1).Count()));
            card.goalMission = _db.GoalMissions.FirstOrDefault(x => x.MissionId == item.MissionId);
            if(card.goalMission != null)
            {
                float action = (float)_db.Timesheets.Where(x => x.MissionId == item.MissionId && x.DeletedAt == null).Select(x => x.Action).Sum();
                float totalGoal = card.goalMission.GoalValue;
                card.progressBar = (action*100)/totalGoal;
            }
            card.favMission = _db.FavouriteMissions.FirstOrDefault(x => x.MissionId == item.MissionId && x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.DeletedAt == null) != null ? 1 : 0;
            card.theme = _db.Themes.FirstOrDefault(x => x.ThemeId == item.ThemeId).Title;
            card.country = _db.Cities.FirstOrDefault(x => x.CityId == item.CityId).Name;

            float totalRate = 0;
            float user = 0;
            var rate = _db.MissionRatings.Where(x => x.MissionId == item.MissionId).ToList();
            foreach (var i in rate)
            {
                user = user + 1;
                totalRate = totalRate + i.Rating;
            }

            card.avgRateing = totalRate > 0 ? totalRate / user : 0;

            return card;
        }
        #endregion Create Card

        #region Rate Mission
        [HttpPost]
        public JsonResult RateMission(int missionId, int rate)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var missionRating = new MissionRating();
            var alradyRate = _db.MissionRatings.Where(x => x.MissionId == missionId && x.UserId == userId).Count();

            if (alradyRate == 0)
            {
                missionRating.MissionId = missionId;
                missionRating.Rating = rate;
                missionRating.UserId = userId;
                _db.MissionRatings.Add(missionRating);
            }
            else
            {
                missionRating = (MissionRating)_db.MissionRatings.FirstOrDefault(x => x.MissionId == missionId && x.UserId == userId);
                missionRating.Rating = rate;
                missionRating.UpdatedAt = DateTime.Now;
                _db.MissionRatings.Update(missionRating);
            }
            _db.SaveChanges();

            return Json("True");
        }
        #endregion Rate Mission

    }
}
