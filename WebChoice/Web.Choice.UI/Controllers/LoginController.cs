using System.Web.Mvc;
using Web.Choice.Bussiness.Implementation;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Common;
using Web.Choice.UI.Models;

namespace Web.Choice.UI.Controllers
{
    public class LoginController : Controller
    {
        private ILoginBus _loginBus = null;
        private ILoginBus Login => _loginBus ?? (_loginBus = new LoginBus());
        private User _user;
        // GET: Login
        public ActionResult Index()
        {
            if (Session[UserSession.ISLOGIN] != null && (bool)Session[UserSession.ISLOGIN])
            {
                if ((int)Session[UserSession.PERMISSION] == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                if ((int)Session[UserSession.PERMISSION] == 2)
                {
                    return RedirectToAction("Index", "Teacher");
                }
                if ((int)Session[UserSession.PERMISSION] == 3)
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            var isLogin = Login.IsValid(model.UserName, model.Password);
            if (ModelState.IsValid)
            {
                if (isLogin)
                {
                    _user = new User();
                    if (_user.IsAdmin())
                        return RedirectToAction("Index", "Admin");
                    if (_user.IsTeacher())
                        return RedirectToAction("Index", "Teacher");
                    if (_user.IsStudent())
                        return RedirectToAction("Index", "Student");
                }
                else
                {
                    ViewBag.error = "Tài khoản hoặc mật khẩu không đúng";
                }
            }
            else
            {
                ViewBag.error = "Có lỗi xảy ra trong quá trình xử lý, vui lòng thử lại sau.";
            }

            return View("Index", model);
        }
    }
}