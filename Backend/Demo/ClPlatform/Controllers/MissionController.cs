using Microsoft.AspNetCore.Mvc;

namespace ClPlatform.Controllers
{
    public class MissionController : Controller
    {
        public IActionResult MissionList()
        {
            return View();
        }
    }
}
