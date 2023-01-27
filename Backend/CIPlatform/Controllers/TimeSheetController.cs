using CIPlatform.DataModels;
using CIPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIPlatform.Controllers
{
    public class TimeSheetController : Controller
    {
        public CiPlatformContext _db = new CiPlatformContext();

        #region TimeSheet Home
        public IActionResult TimeSheet()
        {
            var timeSheetDisplay = new TimeSheetDisplayModel();
            timeSheetDisplay.timeSheets = _db.Timesheets.Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.DeletedAt == null && x.Status == 1).ToList();
            timeSheetDisplay.missions = _db.Missions.ToList();
            return View(timeSheetDisplay);
        }
        #endregion TimeSheet Home

        #region Time Mission

        #region Add GET
        public IActionResult AddTimeMission()
        {
            var timeSheet = new TimeSheetModel();

            #region Fill mission Drop-down 
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.MissionApplications.Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.DeletedAt == null && x.Mission.MissionType == 1 && x.ApprovalStatus == 1).Select(x => x.MissionId).ToList();
            foreach (var item in temp)
            {
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == item);
                list.Add(new SelectListItem() { Text = mission.Title, Value = mission.MissionId.ToString() });
            }
            timeSheet.missions = list;
            #endregion Fill mission Drop-down 

            return PartialView("_AddTimeMissionPartial", timeSheet);
        }
        #endregion Add GET

        #region Edit GET
        public IActionResult EditTimeMission(int id)
        {
            var model = new TimeSheetModel();

            #region Fill mission Drop-down 
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.MissionApplications.Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.DeletedAt == null && x.Mission.MissionType == 1 && x.ApprovalStatus == 1).Select(x => x.MissionId).ToList();
            foreach (var item in temp)
            {
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == item);
                list.Add(new SelectListItem() { Text = mission.Title, Value = mission.MissionId.ToString() });
            }
            model.missions = list;
            model.timeSheet = _db.Timesheets.FirstOrDefault(x => x.TimesheetId == id);

            #endregion Fill mission Drop-down 

            return PartialView("_AddTimeMissionPartial", model);
        }
        #endregion Edit GET

        #region POST 
        [HttpPost]
        public IActionResult AddTimeMission(TimeSheetModel model)
        {
            if (model.timeSheet.MissionId != 0 && model.timeSheet.DateVolunteered != DateTime.MinValue && (model.timeSheet.Hour != null || model.timeSheet.Minute !=null))
            {
                if (model.timeSheet.Minute != null && model.timeSheet.Minute / 60 > 0)
                {
                    model.timeSheet.Hour = model.timeSheet.Hour + (int)(model.timeSheet.Minute / 60);
                    model.timeSheet.Minute = model.timeSheet.Minute % 60;
                }
                var timesheet = new Timesheet();
                timesheet = model.timeSheet;
                timesheet.UserId = int.Parse(HttpContext.Session.GetString("UserId"));

                if (model.timeSheet.TimesheetId == 0)
                {
                    timesheet.Status = 1;
                    _db.Timesheets.Add(timesheet);
                }
                else
                {
                    timesheet.UpdatedAt = DateTime.Now;
                    _db.Timesheets.Update(timesheet);
                }

                _db.SaveChanges();
            }

            return RedirectToAction("TimeSheet", "TimeSheet");
        }
        #endregion POST 

        #endregion Time Mission

        #region Goal Mission

        #region Add GET
        public IActionResult AddGoalMission()
        {
            var timeSheet = new TimeSheetModel();

            #region Fill mission Drop-down 
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.MissionApplications.Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.DeletedAt == null && x.Mission.MissionType == 2 && x.ApprovalStatus == 1).Select(x => x.MissionId).ToList();
            foreach (var item in temp)
            {
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == item);
                list.Add(new SelectListItem() { Text = mission.Title, Value = mission.MissionId.ToString() });
            }
            timeSheet.missions = list;
            #endregion Fill mission Drop-down 

            return PartialView("_AddGoalMissionPartial", timeSheet);
        }
        #endregion Add GET

        #region Edit GET
        public IActionResult EditGoalMission(int id)
        {
            var timeSheet = new TimeSheetModel();

            #region Fill mission Drop-down 
            List<SelectListItem> list = new List<SelectListItem>();
            var temp = _db.MissionApplications.Where(x => x.UserId == int.Parse(HttpContext.Session.GetString("UserId")) && x.DeletedAt == null && x.Mission.MissionType == 2 && x.ApprovalStatus == 1).Select(x => x.MissionId).ToList();
            foreach (var item in temp)
            {
                var mission = _db.Missions.FirstOrDefault(x => x.MissionId == item);
                list.Add(new SelectListItem() { Text = mission.Title, Value = mission.MissionId.ToString() });
            }
            timeSheet.missions = list;
            timeSheet.timeSheet = _db.Timesheets.FirstOrDefault(x => x.TimesheetId == id);
            #endregion Fill mission Drop-down 

            return PartialView("_AddGoalMissionPartial", timeSheet);
        }
        #endregion Edit GET

        #region POST
        [HttpPost]
        public IActionResult AddGoalMission(TimeSheetModel model)
        {
            if (model.timeSheet.MissionId != 0 && model.timeSheet.DateVolunteered != DateTime.MinValue && model.timeSheet.Action != null)
            {
                var timesheet = new Timesheet();
                timesheet = model.timeSheet;
                timesheet.UserId = int.Parse(HttpContext.Session.GetString("UserId"));
                if (model.timeSheet.TimesheetId == 0)
                {
                    timesheet.Status = 1;
                    _db.Timesheets.Add(timesheet);
                }
                else
                {
                    timesheet.UpdatedAt = DateTime.Now;
                    _db.Timesheets.Update(timesheet);
                }
                _db.SaveChanges();

            }
            return RedirectToAction("TimeSheet", "TimeSheet");
        }
        #endregion POST

        #endregion Goal Mission

        #region Delete TimeSheet
        public IActionResult DeleteTimeSheet(int id)
        {
            var timesheet = _db.Timesheets.FirstOrDefault(x => x.TimesheetId == id);
            timesheet.DeletedAt = DateTime.Now;
            timesheet.Status = 0;
            _db.Timesheets.Update(timesheet);
            _db.SaveChanges();

            return RedirectToAction("TimeSheet", "TimeSheet");
        }
        #endregion Delete TimeSheet
    }
}
