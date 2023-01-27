using ClPlatform.DataModels;
using ClPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClPlatform.Controllers
{
    public class PolicyController : Controller
    {

        public ClPlatformContext _db = new ClPlatformContext();
        public IActionResult Home()
        {
            var policy = new PolicyModel();
            policy.PolicyList = GetPolicy();

            return View(policy);
        }

        public List<Policy> GetPolicy()
        {
            return _db.Policies.ToList();
        }
    }
}
