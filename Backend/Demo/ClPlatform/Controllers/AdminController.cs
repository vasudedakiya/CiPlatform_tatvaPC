using ClPlatform.DataModels;
using ClPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClPlatform.Controllers
{
    public class AdminController : Controller
    {
        public ClPlatformContext _db = new ClPlatformContext();
        public IActionResult User()
        {
            var model = new AdminUserModel();
            model.users = _db.Users.ToList();


            return View(model);
        }

        public IActionResult StoryList()
        {
            return View();
        }
    }
}
