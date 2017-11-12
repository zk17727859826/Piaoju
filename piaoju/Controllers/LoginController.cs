using piaoju.Bll;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jiax_drive_affice.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(string userno, string password)
        {
            try
            {
                AccountBll bll = new AccountBll();
                Toperator user = bll.GetUser(userno);
                if (user == null)
                {
                    throw new Exception("用户不存在");
                }

                if (user.password == password)
                {
                    UserInfo ui = new UserInfo()
                     {
                         UserNo = userno
                     };

                    BaseController bc = new BaseController();
                    bc.SessionUser = ui;

                    return Json(new
                    {
                        success = true,
                        msg = "登陆成功"
                    });
                }
                else
                {
                    throw new Exception("用户名或密码失败");
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = ex.Message.Replace("\r\n", " ").Replace("\n", " ")
                });
            }
        }

        public ActionResult Logout()
        {
            BaseController bc = new BaseController();
            bc.SessionUser = null;
            return RedirectToAction("Index", "Login");
        }
    }
}
