using CIPlatform.DataModels;
using CIPlatform.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MimeKit.Text;

namespace CIPlatform.Controllers
{
    public class UserProfileController : Controller
    {
        public CiPlatformContext _db = new CiPlatformContext();
        private readonly IWebHostEnvironment _hostEnvironment;

        #region Constructor
        public UserProfileController(IWebHostEnvironment _environment)
        {
            _hostEnvironment = _environment;
        }
        #endregion Constructor

        #region User Profile

        public IActionResult UserProfile()
        {
            var id = int.Parse(HttpContext.Session.GetString("UserId"));
            var model = new UserProfileModel();
            model.user = _db.Users.FirstOrDefault(x => x.UserId == id);
            model.UserSkills = _db.UserSkills.Where(x => x.UserId == id && x.DeletedAt == null).Select(x => x.Skill.SkillName).ToList();

            #region Fill Country Drop-down
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.Countries.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            model.Countrys = list;
            #endregion Fill Country Drop-down

            if (model.user.CityId != null)
            {
                #region Fill City Drop-down
                List<SelectListItem> list1 = new List<SelectListItem>();
                var temp1 = _db.Cities.Where(x => x.DeletedAt == null && x.CountryId == model.user.CountryId).AsEnumerable().ToList();
                foreach (var item in temp1)
                {
                    list1.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
                }
                model.Citys = list1;
                #endregion Fill City Drop-down
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult UserProfile(UserProfileModel model, IFormFile? userImg)
        {
            model.user.UpdatedAt = DateTime.Now;
            if (userImg != null)
            {
                model.user.Avatar = saveImg(userImg, "UserImg");
            }
            _db.Users.Update(model.user);
            _db.SaveChanges();

            return RedirectToAction("MissionListing", "Mission");
        }
        #endregion User Profile

        #region Contact Us
        public IActionResult ContactUs()
        {
            var model = new ContactUsModel();
            model.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = _db.Users.FirstOrDefault(x => x.UserId == model.UserId);
            model.UserName = user.FirstName + " " + user.LastName;
            model.UserEmail = user.Email;
            return PartialView("_ContactUsPartial", model);
        }

        [HttpPost]
        public IActionResult ContactUs(ContactUsModel model)
        {
            if (model.Subject != null && model.Message != null)
            {
                #region Send Mail
                var mailBody = "<b>User Id</b> = " + model.UserId + "</br><b>User Name</b> = " + model.UserName + " </br> <b>User Email</b> = " + model.UserEmail + "</br> <b>message</b> = " + model.Message;
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("vasudedakiya3@gmail.com"));
                email.To.Add(MailboxAddress.Parse("vasudedakiya3@gmail.com"));
                email.Subject = "CI Platform" + model.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("vasudedakiya3@gmail.com", "whdatdbclkgporxj");
                smtp.Send(email);
                smtp.Disconnect(true);
                #endregion Send Mail
            }

            return RedirectToAction("UserProfile", "UserProfile");
        }
        #endregion Contact Us

        #region Change Password
        public IActionResult ChangePassword()
        {
            var model = new ChangePassModel();
            model.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
            return PartialView("_ChangePassPartial", model);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassModel model)
        {

            var user = _db.Users.FirstOrDefault(x => x.UserId == model.UserId);
            if (model.Password == user.Password)
            {
                user.Password = model.newPassword;
                _db.Users.Update(user);
                _db.SaveChanges();
            }
            else
            {
                TempData["ErrorMes"] = "Old Password is wrong";
            }
            return RedirectToAction("UserProfile", "UserProfile");
        }
        #endregion Change Password

        #region Add Skill

        public IActionResult AddUserSkill()
        {
            var model = new AddUserSkillModel();
            model.UserId = int.Parse(HttpContext.Session.GetString("UserId"));

            #region Fill Skill Drop-Down
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.Skills.Where(x => x.DeletedAt == null).AsEnumerable().ToList();

            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp2 = _db.UserSkills.Where(x => x.UserId == model.UserId).AsEnumerable().ToList();
            foreach (var item in temp2)
            {
                list1.Add(new SelectListItem() { Text = temp.Find(x => x.SkillId == item.SkillId).SkillName, Value = item.SkillId.ToString() });
            }
            model.userOldSkill = list1;

            foreach (var item in temp)
            {
                if (temp2.Find(x => x.SkillId == item.SkillId) == null)
                {
                    list.Add(new SelectListItem() { Text = item.SkillName, Value = item.SkillId.ToString() });
                }
            }
            model.skills = list;
            #endregion Fill Skill Drop-Down

            return PartialView("_AddSkillPartial", model);
        }

        [HttpPost]
        public JsonResult AddUserSkill(string userSkills)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            _db.UserSkills.RemoveRange(_db.UserSkills.Where(x => x.UserId == userId));
            _db.SaveChanges();

            #region Add  Skill
            if (userSkills != null)
            {
                var temp = userSkills;
                var numbers = temp?.Split(',')?.Select(Int32.Parse)?.ToList();
                foreach (var n in numbers)
                {
                    var skill = new UserSkill();
                    skill.UserId = userId;
                    skill.SkillId = n;

                    _db.UserSkills.Add(skill);
                    _db.SaveChanges();
                }
            }
            #endregion Add Skill

            return Json("True");
        }

        #endregion Add Skill

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
    }
}
