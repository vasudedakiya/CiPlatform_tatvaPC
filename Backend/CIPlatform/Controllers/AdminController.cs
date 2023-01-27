using CIPlatform.DataModels;
using CIPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CIPlatform.Controllers
{
    public class AdminController : Controller
    {
        public CiPlatformContext _db = new CiPlatformContext();
        private readonly IWebHostEnvironment _hostEnvironment;

        #region Constructor
        public AdminController(IWebHostEnvironment _environment)
        {
            _hostEnvironment = _environment;
        }
        #endregion Constructor

        #region User

        #region User DataTable
        [CheckSession]
        public IActionResult User()
        {
            var model = new AdminModel();
            model.users = _db.Users.Where(u => u.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion User DataTable

        #region User Add

        #region User Add GET
        public IActionResult CreateUser()
        {
            User user = new User();
            return PartialView("_AddUserPartial", user);
        }
        #endregion User Add GET

        #region User Add POST
        [HttpPost]
        public IActionResult CreateUser(User model)
        {
            if (model.Email != null && model.Password != null)
            {

                if (model.UserId != 0)
                {
                    EditUser(model);
                    return RedirectToAction("User", "Admin");
                }
                else
                {
                    var user = _db.Users.FirstOrDefault(u => u.Email.Equals(model.Email.ToLower()) && u.DeletedAt == null);

                    if (user == null)
                    {
                        model.Status = 1;
                        var newUser = _db.Users.Add(model);
                        _db.SaveChanges();

                        return RedirectToAction("User", "Admin");
                    }

                    TempData["ErrorMes"] = "User Alrady exist with same Email";
                    return RedirectToAction("User", "Admin");
                }
            }
            return RedirectToAction("User", "Admin");
        }
        #endregion User Add POST

        #endregion User Add

        #region User Edit

        #region User Edit GET
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserId == id);
            return PartialView("_AddUserPartial", user);
        }
        #endregion User Edit GET

        #region User Edit POST

        public IActionResult EditUser(User model)
        {
            model.UpdatedAt = DateTime.Now;
            _db.Users.Update(model);
            _db.SaveChanges();
            return RedirectToAction("User", "Admin");
        }

        #endregion User Edit POST

        #endregion User Edit

        #region User Delete
        public IActionResult DeleteUser(int id)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserId == id);
            user.DeletedAt = DateTime.Now;
            _db.Users.Update(user);
            _db.SaveChanges();
            return RedirectToAction("User", "Admin");
        }
        #endregion User Delete

        #region Change User Status
        [HttpGet]
        public IActionResult ChangeUserStatus(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == id);
            user.Status = (user.Status == 1) ? 0 : 1;
            _db.Users.Update(user);
            _db.SaveChanges();

            return RedirectToAction("User", "Admin");
        }
        #endregion Change User Status

        #region Admin Add

        #region Admin Add GET
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            Admin admin = new Admin();
            return PartialView("_AddAdminPartial", admin);
        }
        #endregion Admin Add GET

        #region Admin Add POST
        [HttpPost]
        public IActionResult CreateAdmin(Admin model)
        {
            if (model.Email != null && model.Password != null)
            {
                var admin = _db.Admins.FirstOrDefault(u => u.Email.Equals(model.Email.ToLower()) && u.DeletedAt == null);
                if (admin == null)
                {
                    var newAdmin = _db.Admins.Add(model);
                    _db.SaveChanges();
                    return RedirectToAction("User", "Admin");
                }

                TempData["ErrorMes"] = "User Alrady exist with same Email";
            }
            return RedirectToAction("User", "Admin");
        }
        #endregion Admin Add POST

        #endregion Admin Add

        #endregion User

        #region CMS

        #region CMS DataTable
        [CheckSession]
        public IActionResult CMSPage()
        {
            var model = new AdminModel();
            model.cmsPages = _db.CmsPages.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion CMS DataTable

        #region CMS Add

        #region Add CMS GET
        [HttpGet]
        [CheckSession]
        public IActionResult AddCMSPage(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                var page = _db.CmsPages.FirstOrDefault(p => p.CmsPageId == id);
                return View(page);
            }
        }
        #endregion Add CMS GET

        #region Add CMS POST
        [HttpPost]
        public IActionResult AddCMSPage(CmsPage model)
        {
            if (model.CmsPageId != 0)
            {
                EditCMSPage(model);
                return RedirectToAction("CMSPage", "Admin");
            }
            else
            {
                model.Status = 1;
                _db.CmsPages.Add(model);
                _db.SaveChanges();
                return RedirectToAction("CMSPage", "Admin");
            }
        }
        #endregion Add CMS POST

        #endregion CMS Add

        #region CMS Edit

        #region Edit CMS POST
        [HttpPost]
        public IActionResult EditCMSPage(CmsPage model)
        {
            model.UpdatedAt = DateTime.Now;
            _db.CmsPages.Update(model);
            _db.SaveChanges();
            return RedirectToAction("CMSPage", "Admin");
        }
        #endregion Edit CMS POST

        #endregion CMS Edit

        #region CMS Delete

        [HttpGet]
        public IActionResult DeleteCMS(int id)
        {
            var page = _db.CmsPages.FirstOrDefault(p => p.CmsPageId == id);
            page.DeletedAt = DateTime.Now;
            _db.CmsPages.Update(page);
            _db.SaveChanges();
            return RedirectToAction("CMSPage", "Admin");
        }

        #endregion CMS Delete

        #region Change CMS Status
        [HttpGet]
        public IActionResult ChangeCMSStatus(int id)
        {
            var page = _db.CmsPages.FirstOrDefault(u => u.CmsPageId == id);
            page.Status = (page.Status == 1) ? 0 : 1;
            _db.CmsPages.Update(page);
            _db.SaveChanges();

            return RedirectToAction("CMSPage", "Admin");
        }
        #endregion Change CMS Status

        #endregion CMS

        #region Mission

        #region Mission DataTable
        [CheckSession]
        public IActionResult Mission()
        {
            var model = new AdminModel();
            model.missions = _db.Missions.Where(m => m.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion Mission DataTable

        #region Mission Add

        #region Add Mission GET
        [HttpGet]
        public IActionResult CreateMission()
        {
            AddMissionModel mission = new AddMissionModel();

            #region Fill Country Drop-down
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.Countries.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            mission.countrys = list;
            #endregion Fill Country Drop-down

            #region Fill Theme Drop-down
            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp1 = _db.Themes.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp1)
            {
                list1.Add(new SelectListItem() { Text = item.Title, Value = item.ThemeId.ToString() });
            }
            mission.themes = list1;
            #endregion Fill Theme Drop-down

            #region Fill Skill Drop-Down
            List<SelectListItem> list2 = new List<SelectListItem>();
            var temp2 = _db.Skills.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp2)
            {
                list2.Add(new SelectListItem() { Text = item.SkillName, Value = item.SkillId.ToString() });
            }
            mission.skills = list2;
            #endregion Fill Skill Drop-Down

            return PartialView("_AddMissionPartial", mission);
        }
        #endregion Add Mission GET

        #region Fill City Drop-down
        [HttpPost]
        public JsonResult GetCity(int id)
        {
            AddMissionModel mission = new AddMissionModel();
            List<SelectListItem> list = new List<SelectListItem>();

            var temp = _db.Cities.Where(x => x.DeletedAt == null && x.CountryId == id).AsEnumerable().ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            mission.citys = list;
            return Json(mission.citys);
        }
        #endregion Fill City Drop-down

        #region Add Mission POST
        [HttpPost]
        public IActionResult CreateMission(AddMissionModel model, List<IFormFile>? imgsFiles, List<IFormFile>? docFiles)
        {
            if (model.mission.Title != null && model.mission.ThemeId != null && model.mission.CityId != null && model.mission.CountryId != null && model.mission.MissionType != null && imgsFiles != null)
            {
                if (model.mission.MissionId != 0)
                {
                    EditMission(model, imgsFiles, docFiles);
                    return RedirectToAction("Mission", "Admin");
                }
                else
                {
                    #region Add Mission DB
                    model.mission.Status = 1;
                    _db.Missions.Add(model.mission);
                    _db.SaveChanges();
                    #endregion Add Mission DB

                    #region Add Mission Img
                    if (imgsFiles != null)
                    {
                        foreach (var im in imgsFiles)
                        {
                            var missionMedia = new MissionMedium();
                            missionMedia.MissionId = model.mission.MissionId;
                            missionMedia.MediaType = Path.GetExtension(im.FileName); ;
                            missionMedia.MediaName = im.FileName;
                            missionMedia.MediaPath = saveImg(im, "MissionMedia");

                            _db.MissionMedia.Add(missionMedia);
                            _db.SaveChanges();
                        }
                    }
                    #endregion Add Mission Img

                    #region Add Mission Doc
                    if (docFiles != null)
                    {
                        foreach (var file in docFiles)
                        {
                            var missionDoc = new MissionDocument();
                            missionDoc.MissionId = model.mission.MissionId;
                            missionDoc.DocumentType = Path.GetExtension(file.FileName);
                            missionDoc.DocumentName = file.FileName;
                            missionDoc.DocumentPath = saveImg(file, "MissionDocument");

                            _db.MissionDocuments.Add(missionDoc);
                            _db.SaveChanges();
                        }
                    }
                    #endregion Add Mission Doc

                    #region Add Goal Mission
                    if (model.mission.MissionType == 2)
                    {
                        var goalMission = new GoalMission();
                        goalMission.MissionId = model.mission.MissionId;
                        goalMission.GoalValue = model.goalMission.GoalValue;
                        goalMission.GoalObjectiveText = model.goalMission.GoalObjectiveText;

                        _db.GoalMissions.Add(goalMission);
                        _db.SaveChanges();
                    }
                    #endregion Add Goal Mission

                    #region Add Mission Skill
                    if (model.addSkill != null)
                    {
                        var temp = model.addSkill[0];
                        var numbers = temp?.Split(',')?.Select(Int32.Parse)?.ToList();
                        foreach (var n in numbers)
                        {
                            var missionSkill = new MissionSkill();
                            missionSkill.MissionId = model.mission.MissionId;
                            missionSkill.SkillId = n;

                            _db.MissionSkills.Add(missionSkill);
                            _db.SaveChanges();
                        }
                    }
                    #endregion Add Mission Skill

                    return RedirectToAction("Mission", "Admin");
                }
            }
            return RedirectToAction("Mission", "Admin");
        }
        #endregion Add Mission POST

        #endregion Mission Add

        #region Mission Edit 

        #region Edit Mission GET
        [HttpGet]
        public IActionResult EditMission(int id)
        {
            AddMissionModel mission = new AddMissionModel();

            mission.mission = _db.Missions.FirstOrDefault(x => x.MissionId == id);

            mission.goalMission = _db.GoalMissions.FirstOrDefault(x => x.MissionId == mission.mission.MissionId);

            mission.addSkill = _db.MissionSkills.Where(s => s.MissionId == mission.mission.MissionId).Select(x => x.SkillId.ToString()).ToList();

            #region Fill Country Drop-down
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.Countries.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp)
            {
                list.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            mission.countrys = list;
            #endregion Fill Country Drop-down

            #region Fill City Drop-down
            List<SelectListItem> list3 = new List<SelectListItem>();
            var temp3 = _db.Cities.Where(x => x.DeletedAt == null && x.CountryId == mission.mission.CountryId).AsEnumerable().ToList();
            foreach (var item in temp3)
            {
                list3.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            mission.citys = list3;
            #endregion Fill City Drop-down

            #region Fill Theme Drop-down
            List<SelectListItem> list1 = new List<SelectListItem>();
            var temp1 = _db.Themes.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp1)
            {
                list1.Add(new SelectListItem() { Text = item.Title, Value = item.ThemeId.ToString() });
            }
            mission.themes = list1;
            #endregion Fill Theme Drop-down

            #region Fill Skill Drop-Down
            List<SelectListItem> list2 = new List<SelectListItem>();
            var temp2 = _db.Skills.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            foreach (var item in temp2)
            {
                list2.Add(new SelectListItem() { Text = item.SkillName, Value = item.SkillId.ToString() });
            }
            mission.skills = list2;
            #endregion Fill Skill Drop-Down

            return PartialView("_AddMissionPartial", mission);
        }
        #endregion Edit Mission GET

        #region Edit Mission POST
        [HttpPost]
        public IActionResult EditMission(AddMissionModel model, List<IFormFile> imgsFiles, List<IFormFile> docFiles)
        {
            #region Add Mission DB
            if (model.mission.MissionType == 2)
            {
                model.mission.StartDate = null;
                model.mission.EndDate = null;
                model.mission.Deadline = null;
            }
            model.mission.UpdatedAt = DateTime.Now;
            _db.Missions.Update(model.mission);
            _db.SaveChanges();
            #endregion Add Mission DB

            #region Add Mission Img
            if (imgsFiles != null)
            {
                foreach (var im in imgsFiles)
                {
                    var missionMedia = new MissionMedium();
                    missionMedia.MissionId = model.mission.MissionId;
                    missionMedia.MediaType = Path.GetExtension(im.FileName); ;
                    missionMedia.MediaName = im.FileName;
                    missionMedia.MediaPath = saveImg(im, "MissionMedia");
                    missionMedia.UpdatedAt = DateTime.Now;

                    _db.MissionMedia.Add(missionMedia);
                    _db.SaveChanges();
                }
            }
            #endregion Add Mission Img

            #region Add Mission Doc
            if (docFiles != null)
            {
                foreach (var file in docFiles)
                {
                    var missionDoc = new MissionDocument();
                    missionDoc.MissionId = model.mission.MissionId;
                    missionDoc.DocumentType = Path.GetExtension(file.FileName);
                    missionDoc.DocumentName = file.FileName;
                    missionDoc.DocumentPath = saveImg(file, "MissionDocument");
                    missionDoc.UpdatedAt = DateTime.Now;

                    _db.MissionDocuments.Add(missionDoc);
                    _db.SaveChanges();
                }
            }
            #endregion Add Mission Doc

            #region Add Goal Mission
            if (model.mission.MissionType == 2)
            {
                model.goalMission.MissionId = model.mission.MissionId;
                model.goalMission.UpdatedAt = DateTime.Now;
                _db.GoalMissions.Update(model.goalMission);
                _db.SaveChanges();
            }
            else
            {
                var temp = _db.GoalMissions.FirstOrDefault(x => x.MissionId == model.mission.MissionId);
                if (temp != null)
                {
                    _db.GoalMissions.Remove(temp);
                }
            }
            #endregion Add Goal Mission

            #region Add Mission Skill
            if (model.addSkill != null)
            {
                var temp = model.addSkill[0];
                var numbers = temp?.Split(',')?.Select(Int32.Parse)?.ToList();

                _db.MissionSkills.RemoveRange(_db.MissionSkills.Where(x => x.MissionId == model.mission.MissionId));
                _db.SaveChanges();

                foreach (var n in numbers)
                {
                    var missionSkill = new MissionSkill();
                    missionSkill.MissionId = model.mission.MissionId;
                    missionSkill.SkillId = n;
                    missionSkill.UpdatedAt = DateTime.Now;

                    _db.MissionSkills.Add(missionSkill);
                    _db.SaveChanges();
                }
            }
            #endregion Add Mission Skill

            return RedirectToAction("Mission", "Admin");
        }
        #endregion Edit Mission POST

        #endregion Mission Edit 

        #region Mission Delete

        public IActionResult DeleteMission(int id)
        {
            _db.MissionDocuments.RemoveRange(_db.MissionDocuments.Where(x => x.MissionId == id));
            _db.MissionMedia.RemoveRange(_db.MissionMedia.Where(x => x.MissionId == id));
            _db.MissionSkills.RemoveRange(_db.MissionSkills.Where(x => x.MissionId == id));

            var mission = _db.Missions.FirstOrDefault(x => x.MissionId == id);
            mission.DeletedAt = DateTime.Now;
            _db.Missions.Update(mission);

            if (mission.MissionType == 2)
            {
                var goalMission = _db.GoalMissions.FirstOrDefault(x => x.MissionId == id);
                goalMission.DeletedAt = DateTime.Now;
                _db.GoalMissions.Update(goalMission);
            }
            _db.SaveChanges();


            return RedirectToAction("Mission", "Admin");
        }

        #endregion Mission Delete

        #region Change Mission Status
        [HttpGet]
        public IActionResult ChangeMissionStatus(int id)
        {
            var mission = _db.Missions.FirstOrDefault(u => u.MissionId == id);
            mission.Status = (mission.Status == 1) ? 0 : 1;
            _db.Missions.Update(mission);
            _db.SaveChanges();

            return RedirectToAction("Mission", "Admin");
        }
        #endregion Change Mission Status

        #endregion Mission

        #region Theme

        #region Theme DataTable
        [CheckSession]
        public IActionResult MissionTheme()
        {
            var model = new AdminModel();
            model.themes = _db.Themes.Where(t => t.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion Theme DataTable

        #region Theme Add

        #region Add Theme GET
        [HttpGet]
        public IActionResult CreateTheme()
        {
            Theme theme = new Theme();
            return PartialView("_AddThemePartial", theme);
        }
        #endregion Add Theme GET

        #region Add Theme Post
        [HttpPost]
        public IActionResult CreateTheme(Theme model)
        {
            if (model.Title != null)
            {

                if (model.ThemeId != 0)
                {
                    EditTheme(model);
                }
                else
                {
                    model.Status = 1;
                    _db.Themes.Add(model);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("MissionTheme", "Admin");
        }
        #endregion Add Theme POST

        #endregion Theme Add

        #region Theme Edit

        #region Edit Theme GET
        [HttpGet]
        public IActionResult EditTheme(int id)
        {
            var theme = _db.Themes.FirstOrDefault(t => t.ThemeId == id);
            return PartialView("_AddThemePartial", theme);
        }
        #endregion Edit Theme GET

        #region Edit Theme POST
        [HttpPost]
        public IActionResult EditTheme(Theme model)
        {
            model.UpdatedAt = DateTime.Now;
            _db.Themes.Update(model);
            _db.SaveChanges();
            return RedirectToAction("MissionTheme", "Admin");
        }
        #endregion Edit Theme POST

        #endregion Theme Edit

        #region Theme Delete

        public IActionResult DeleteTheme(int id)
        {
            var theme = _db.Themes.FirstOrDefault(t => t.ThemeId == id);
            theme.DeletedAt = DateTime.Now;
            _db.Themes.Update(theme);
            _db.SaveChanges();
            return RedirectToAction("MissionTheme", "Admin");
        }

        #endregion Theme Delete

        #region Change Theme Status
        [HttpGet]
        public IActionResult ChangeThemeStatus(int id)
        {
            var theme = _db.Themes.FirstOrDefault(u => u.ThemeId == id);
            theme.Status = (theme.Status == 1) ? 0 : 1;
            _db.Themes.Update(theme);
            _db.SaveChanges();

            return RedirectToAction("MissionTheme", "Admin");
        }
        #endregion Change Theme Status

        #endregion Theme

        #region Skill

        #region Skill DataTable
        [CheckSession]
        public IActionResult MissionSkill()
        {
            var model = new AdminModel();
            model.skills = _db.Skills.Where(s => s.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion Skill DataTable

        #region Skill Add

        #region Skill Add GET
        [HttpGet]
        public IActionResult CreateSkill()
        {
            Skill skill = new Skill();
            return PartialView("_AddSkillPartial", skill);
        }
        #endregion Skill Add GET

        #region Skill Add POST

        [HttpPost]
        public IActionResult CreateSkill(Skill model)
        {
            if (model.SkillName != null)
            {
                if (model.SkillId != 0)
                {
                    EditSkill(model);
                }
                else
                {
                    model.Status = 1;
                    _db.Skills.Add(model);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("MissionSkill", "Admin");
        }

        #endregion Skill Add POST

        #endregion Skill Add

        #region Skill Edit

        #region Skill Edit GET
        [HttpGet]
        public IActionResult EditSkill(int id)
        {
            var skill = _db.Skills.FirstOrDefault(x => x.SkillId == id);
            return PartialView("_AddSkillPartial", skill);
        }
        #endregion Skill Edit GET

        #region Skill Edit POST
        [HttpPost]
        public IActionResult EditSkill(Skill model)
        {
            model.UpdatedAt = DateTime.Now;
            _db.Skills.Update(model);
            _db.SaveChanges();
            return RedirectToAction("MissionSkill", "Admin");
        }
        #endregion Skill Edit POST

        #endregion Skill Edit

        #region Skill Delete 
        public IActionResult DeleteSkill(int id)
        {
            var skill = _db.Skills.FirstOrDefault(x => x.SkillId == id);
            skill.DeletedAt = DateTime.Now;
            _db.Skills.Update(skill);
            _db.SaveChanges();
            return RedirectToAction("MissionSkill", "Admin");
        }
        #endregion Skill Delete

        #region Change Skill Status
        [HttpGet]
        public IActionResult ChangeSkillStatus(int id)
        {
            var skill = _db.Skills.FirstOrDefault(u => u.SkillId == id);
            skill.Status = (skill.Status == 1) ? 0 : 1;
            _db.Skills.Update(skill);
            _db.SaveChanges();

            return RedirectToAction("MissionSkill", "Admin");
        }
        #endregion Change User Status

        #endregion Skill

        #region Mission Application

        #region Mission Application DataTable
        [CheckSession]
        public IActionResult MissionApplication()
        {
            var model = new AdminModel();
            model.missionApplications = _db.MissionApplications.Where(s => s.DeletedAt == null && s.ApprovalStatus == 0).AsEnumerable().ToList();
            model.missions = _db.Missions.Where(m => m.DeletedAt == null).AsEnumerable().ToList();
            model.users = _db.Users.Where(u => u.DeletedAt == null).AsEnumerable().ToList();

            return View(model);
        }
        #endregion Mission Application DataTable

        #region Mission Application Accept
        [HttpGet]
        public IActionResult AcceptMissionApplication(int id)
        {
            var application = _db.MissionApplications.FirstOrDefault(x => x.MissionApplicationId == id);
            application.ApprovalStatus = 1;
            _db.MissionApplications.Update(application);
            _db.SaveChanges();
            return RedirectToAction("MissionApplication", "Admin");
        }

        #endregion Mission Application Accept 

        #region Mission Application Decline
        [HttpGet]
        public IActionResult DeclineMissionApplication(int id)
        {
            _db.MissionApplications.Remove(_db.MissionApplications.FirstOrDefault(x => x.MissionApplicationId == id));
            _db.SaveChanges();
            return RedirectToAction("MissionApplication", "Admin");
        }

        #endregion Mission Application Decline

        #endregion Mission Application

        #region Story List

        #region Story List DataTable
        [CheckSession]
        public IActionResult StoryList()
        {
            var model = new AdminModel();
            model.users = _db.Users.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            model.stories = _db.Stories.Where(x => x.Status == 1 && x.DeletedAt == null).AsEnumerable().ToList();
            model.missions = _db.Missions.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion Story List DataTable

        #region Story Delete
        public IActionResult DeleteStory(int id)
        {
            var story = _db.Stories.FirstOrDefault(x => x.StoryId == id);
            story.Status = 0;
            story.DeletedAt = DateTime.Now;
            _db.Stories.Update(story);
            _db.SaveChanges();
            return RedirectToAction("StoryList", "Admin");
        }
        #endregion Story Delete

        #endregion Story List

        #region Story Application

        #region Story Application DataTable
        [CheckSession]
        public IActionResult StoryApplication()
        {
            var model = new AdminModel();
            model.users = _db.Users.Where(x => x.DeletedAt == null).AsEnumerable().ToList();
            model.stories = _db.Stories.Where(x => x.Status == 0 && x.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion Story Application DataTable

        #region Story Application Accept
        [HttpGet]
        public IActionResult AcceptStoryApplication(int id)
        {
            var application = _db.Stories.FirstOrDefault(x => x.StoryId == id);
            application.Status = 1;
            _db.Stories.Update(application);
            _db.SaveChanges();
            return RedirectToAction("StoryApplication", "Admin");
        }
        #endregion Story Application Accept

        #region Story Application Decline
        public IActionResult DeclineStoryApplication(int id)
        {
            _db.Stories.Remove(_db.Stories.FirstOrDefault(x => x.StoryId == id));
            _db.SaveChanges();
            return RedirectToAction("StoryApplication", "Admin");
        }
        #endregion Story Application Decline

        #endregion Story Application

        #region Banner

        #region Banner DataTable
        [CheckSession]
        public IActionResult BannerManagement()
        {
            var model = new AdminModel();
            model.banners = _db.Banners.Where(b => b.DeletedAt == null).AsEnumerable().ToList();
            return View(model);
        }
        #endregion Banner DataTable

        #region Banner Add
        [HttpGet]
        public IActionResult CreateBanner()
        {
            Banner banner = new Banner();
            return PartialView("_AddBannerPartial", banner);
        }

        [HttpPost]
        public IActionResult CreateBanner(Banner model, IFormFile? carouselImg)
        {
            if (carouselImg != null && model.Title != null)
            {
                if (model.BannerId != 0)
                {
                    EditBanner(model, carouselImg);
                    return RedirectToAction("BannerManagement", "Admin");
                }
                else
                {
                    if (carouselImg != null)
                    {
                        model.Image = saveImg(carouselImg, "Banner");
                    }
                    var newBanner = _db.Banners.Add(model);
                    _db.SaveChanges();

                    return RedirectToAction("BannerManagement", "Admin");
                }
            }
            return RedirectToAction("BannerManagement", "Admin");
        }
        #endregion Banner Add

        #region Banner Edit

        #region Banner Edit GET
        [HttpGet]
        public IActionResult EditBanner(int id)
        {
            var banner = _db.Banners.FirstOrDefault(b => b.BannerId == id);
            return PartialView("_AddBannerPartial", banner);
        }
        #endregion Banner Edit GET

        #region Banner Edit POST
        [HttpPost]
        public IActionResult EditBanner(Banner model, IFormFile? carouselImg)
        {
            if (carouselImg != null)
            {
                model.Image = saveImg(carouselImg, "Banner");
            }

            model.UpdatedAt = DateTime.Now;
            _db.Banners.Update(model);
            _db.SaveChanges();
            return RedirectToAction("BannerManagement", "Admin");
        }
        #endregion Banner Edit POST

        #endregion Banner Edit

        #region Banner Delete
        public IActionResult DeleteBanner(int id)
        {
            var banner = _db.Banners.FirstOrDefault(x => x.BannerId == id);
            banner.DeletedAt = DateTime.Now;
            _db.Banners.Update(banner);
            _db.SaveChanges();
            return RedirectToAction("BannerManagement", "Admin");
        }
        #endregion Banner Delete

        #endregion Banner

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

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");

            return RedirectToAction("Login", "Login");
        }
        #endregion Logout
    }
}