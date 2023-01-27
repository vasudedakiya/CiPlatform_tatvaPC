using CIPlatform.DataModels;
using CIPlatform.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;


namespace CIPlatform.Controllers
{
    public class LoginController : Controller
    {
        public CiPlatformContext _db = new CiPlatformContext();

        #region Login
        public IActionResult Login()
        {
            var loginModel = new LoginModel();
            loginModel.banner = GetBanner();
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email.Equals(model.user.Email.ToLower()) && u.Password.Equals(model.user.Password) && u.DeletedAt == null);

            if (user == null)
            {
                #region Check Admin
                var admin = _db.Admins.FirstOrDefault(u => u.Email.Equals(model.user.Email.ToLower()) && u.Password.Equals(model.user.Password) && u.DeletedAt == null);

                if (admin != null)
                {
                    HttpContext.Session.SetString("UserId", admin.AdminId.ToString());
                    HttpContext.Session.SetString("UserName", admin.FirstName.ToString());
                    HttpContext.Session.SetString("Role", "1");

                    return RedirectToAction("User", "Admin");
                }
                #endregion Check Admin

                #region Incorrect Info
                var loginModel = new LoginModel();
                loginModel.banner = GetBanner();
                TempData["ErrorMes"] = "Email or password is incorrect";
                return View(loginModel);
                #endregion Incorrect Info
            }

            #region Check User Deactivated
            if (user.Status != 1)
            {
                var loginModel = new LoginModel();
                loginModel.banner = GetBanner();
                TempData["ErrorMes"] = "This user is deactivated";
                return View(loginModel);
            }
            #endregion Check User Deactivated

            var notification = _db.Notifications.Where(u => u.Status == 1).AsEnumerable().ToList();

            #region Store Info in session   

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.FirstName.ToString() + " " + user.LastName.ToString());
            HttpContext.Session.SetString("Role", "0");
            HttpContext.Session.SetString("Img", user.Avatar.ToString());
            TempData["Notification"] = notification; 


            #endregion Store Info in session

            return RedirectToAction("MissionListing", "Mission");
        }

        #endregion Login

        #region Register
        public IActionResult Register()
        {
            var loginModel = new LoginModel();
            loginModel.banner = GetBanner();
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Register(LoginModel model)
        {

            var user = _db.Users.FirstOrDefault(u => u.Email.Equals(model.user.Email.ToLower()) && u.DeletedAt == null);

            if (user == null)
            {
                model.user.Status = 1;
                var registerUser = _db.Users.Add(model.user);
                _db.SaveChanges();

                return RedirectToAction("Login", "Login");
            }

            TempData["ErrorMes"] = "User alraedy exist with same email";
            var loginModel = new LoginModel();
            loginModel.banner = GetBanner();
            return View(loginModel);
        }
        #endregion Register

        #region Lost Password
        public IActionResult LostPassword()
        {
            var loginModel = new LoginModel();
            loginModel.banner = GetBanner();
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult LostPassword(LoginModel model)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email.Equals(model.user.Email.ToLower()) && u.DeletedAt == null);

            if (user == null)
            {
                TempData["ErrorMes"] = "Email Not exist";
                var loginModel = new LoginModel();
                loginModel.banner = GetBanner();
                return View(loginModel);
            }

            #region Genrate Token
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            #endregion Genrate Token

            #region Update Password Reset Table
            PasswordReset entry = new PasswordReset();
            entry.Email = model.user.Email;
            entry.Token = finalString;
            _db.PasswordResets.Add(entry);
            _db.SaveChanges();
            #endregion Update Password Reset Table

            #region Send Mail
            var mailBody = "<h1>Click link to reset password</h1><br><h2><a href='" + "https://localhost:7227/Login/ResetPassword?token=" + finalString + "'>Reset Password</a></h2>";

            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("vasudedakiya3@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Reset Your Password";
            email.Body = new TextPart(TextFormat.Html) { Text = mailBody };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("vasudedakiya3@gmail.com", "whdatdbclkgporxj");
            smtp.Send(email);
            smtp.Disconnect(true);
            #endregion Send Mail

            TempData["ErrorMes"] = "Check your email to reset password";
            return RedirectToAction("Login", "Login");
        }
        #endregion Lost Password

        #region Reset Password

        public IActionResult ResetPassword()
        {
            var loginModel = new LoginModel();
            loginModel.banner = GetBanner();
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult ResetPassword(LoginModel model, string token)
        {
            var resetPassUser = _db.PasswordResets.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => x.Token.Equals(token));

            DateTime currentTime = DateTime.Now;
            TimeSpan diffrenceTime = (TimeSpan)(currentTime - resetPassUser.CreatedAt);
            if (diffrenceTime.TotalHours <= 4.0)
            {
                var user = _db.Users.FirstOrDefault(x => x.Email.Equals(resetPassUser.Email) && x.DeletedAt == null);
                user.Password = model.user.Password;
                _db.Users.Update(user);
                _db.SaveChanges();
                return RedirectToAction("Login", "Login");

            }
            else
            {
                TempData["ErrorMes"] = "Reset password Token is expired";
                return RedirectToAction("Login", "Login");
            }
        }
        #endregion Reset Password

        #region Banner
        public List<Banner> GetBanner()
        {
            return _db.Banners.Where(x => x.DeletedAt == null).ToList();
        }
        #endregion Banner

    }
}
