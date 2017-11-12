using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelperCenter.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Users(string userno, string username, int page, int pagesize)
        {
            try
            {
                AccountBll accbll = new AccountBll();
                int iCount = accbll.GetUsersCount(userno, username);
                List<sysuser> users = accbll.GetUsers(userno, username, page, pagesize);

                return Json(new
                {
                    success = true,
                    rows = users,
                    total = iCount
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message
                });
            }
        }
    }
}
