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
    public class StoryController : Controller
    {
        public CiPlatformContext _db = new CiPlatformContext();

        private readonly IWebHostEnvironment _hostEnvironment;

        #region Constructor
        public StoryController(IWebHostEnvironment _environment)
        {
            _hostEnvironment = _environment;
        }
        #endregion Constructor

        #region StoryList
        public IActionResult StoryList(int pg = 1)
        {
            var model = new List<StoryListModel>();

            var storys = _db.Stories.Where(x => x.Status == 1 && x.DeletedAt == null).AsEnumerable().ToList();

            foreach (var item in storys)
            {
                var story = new StoryListModel();
                story.story = item;
                var user = _db.Users.FirstOrDefault(x => x.UserId == story.story.UserId);
                story.userName = user.LastName + " " + user.FirstName;
                story.userImg = user.Avatar != null ? user.Avatar : "~/assets/volunteer9.png";
                var img = _db.StoryMedia.FirstOrDefault(x => x.StoryId == story.story.StoryId);
                story.cardImg = img != null ? img.Path : "~/assets/card_3.png";
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == story.story.MissionId);
                story.themeText = _db.Themes.FirstOrDefault(x => x.ThemeId == mission.ThemeId).Title;

                model.Add(story);
            }

            const int pageSize = 9;
            int recsCount = storys.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            var data = model.Skip(recSkip).Take(pager.PageSize).ToList();
            model = data;
            this.ViewBag.Pager = pager;

            return View(model);
        }
        #endregion StoryList

        #region Create Story

        #region  GET
        [HttpGet]
        public IActionResult CreateStory()
        {
            var model = new AddStoryModel();

            #region Fill mission Drop-down 
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.MissionApplications.Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.ApprovalStatus == 1).Select(x => x.MissionId).ToList();
            foreach (var item in temp)
            {
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == item);
                list.Add(new SelectListItem() { Text = mission.Title, Value = mission.MissionId.ToString() });
            }
            model.missions = list;
            #endregion Fill mission Drop-down 

            return View(model);
        }
        #endregion  GET

        #region POST
        [HttpPost]
        public IActionResult CreateStory(AddStoryModel model, List<IFormFile> storyImg)
        {
            #region AddStory
            model.story.Status = 0;
            model.story.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
            _db.Stories.Add(model.story);
            _db.SaveChanges();
            #endregion AddStory

            #region Add Mission Img
            if (storyImg != null)
            {
                foreach (var im in storyImg)
                {
                    var storyMedia = new StoryMedium();
                    storyMedia.StoryId = model.story.StoryId;
                    storyMedia.Type = Path.GetExtension(im.FileName); ;
                    storyMedia.Path = saveImg(im, "StoryMedia");

                    _db.StoryMedia.Add(storyMedia);
                    _db.SaveChanges();
                }
            }
            #endregion Add Mission Img

            return RedirectToAction("StoryList", "Story");
        }
        #endregion POST

        #endregion Create Story

        #region saveImg
        public string saveImg(IFormFile img, string folder)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;

            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"Images\" + folder);
            var extension = Path.GetExtension(img.FileName);

            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                img.CopyTo(fileStreams);
            }
            return @"~/Images/" + folder + "/" + fileName + extension;

        }
        #endregion saveImg

        #region Story Detail
        public IActionResult StoryDetail(int id)
        {
            var storyDetail = new StoryDetailModel();
            var story = _db.Stories.FirstOrDefault(x => x.StoryId == id);
            story.ViewCount += 1;
            _db.Stories.Update(story);
            _db.SaveChanges();
            storyDetail.storyTitle = story.Title;
            storyDetail.storyDetail = story.Description;
            storyDetail.viewCount = story.ViewCount;
            storyDetail.storyImg = _db.StoryMedia.Where(x => x.StoryId == id).Select(x => x.Path).ToList();
            storyDetail.missionId = (int)story.MissionId;
            storyDetail.storyId = (int)story.StoryId;
            var user = _db.Users.FirstOrDefault(x => x.UserId == story.UserId);
            storyDetail.userName = user.FirstName + " " + user.LastName;
            storyDetail.userImg = user.Avatar == null ? "~/assets/volunteer8.png" : user.Avatar;
            storyDetail.whyVolenteer = user.WhyIVolunteer != null ? user.WhyIVolunteer.ToString() : "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut laboreet dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
            return View(storyDetail);
        }
        #endregion Story Detail

        #region Suggest CoWorker
        [HttpGet]
        public IActionResult SuggestCoWorker(long id)
        {
            var story = new Story();
            story.StoryId = id;
            return PartialView("_CoWorkerPartial", story);
        }

        [HttpPost]
        public IActionResult SuggestCoWorker(string WorkerEmail, Story model)
        {
            if (WorkerEmail != null)
            {
                var story = _db.Stories.FirstOrDefault(x => x.StoryId == model.StoryId);
                
                #region Send Mail
                var mailBody = "<h1>" + HttpContext.Session.GetString("UserName") + " Suggest Mission : " + story.Title + " to You</h1><br><h2><a href='https://localhost:7227/Story/StoryDetail?id= " + model.StoryId + "'>Go Website</a></h2>";

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

            return RedirectToAction("StoryList", "Story");
        }
        #endregion Suggest CoWorker
    }
}
