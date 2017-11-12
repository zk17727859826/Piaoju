using piaoju.Bll;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using tg.Helper;

namespace jiax_drive_affice.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 登陆用户信息
        /// </summary>
        public UserInfo SessionUser
        {
            set
            {
                if (value == null)
                {
                    FormsAuthentication.SignOut();
                }
                else
                {
                    UserInfo ui = value;
                    string strUi = ModelHelper.ToJson(ui);
                    FormsAuthentication.SetAuthCookie(strUi, false);
                }
            }
            get
            {
                string strUserInfo = System.Web.HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(strUserInfo))
                {
                    return null;
                }

                FormsAuthentication.SetAuthCookie(strUserInfo, false);
                UserInfo ui = ModelHelper.ToModel<UserInfo>(strUserInfo);
                if (string.IsNullOrEmpty(ui.UserNo))
                {
                    FormsAuthentication.SignOut();
                    ui = null;
                }
                return ui;
            }
        }

        /// <summary>
        /// 脚本版本
        /// </summary>
        public string ScriptVer
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["script_ver"].ToString(); }
        }

        /// <summary>
        /// 获得用户所有信息
        /// </summary>
        /// <returns></returns>
        public static Toperator GetUser()
        {
            string userno = new BaseController().SessionUser.UserNo;
            AccountBll accbll = new AccountBll();
            Toperator user = accbll.GetUser(userno);
            if (user != null)
            {
                return user;
            }
            else
            {
                return new Toperator();
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ScriptVer = ScriptVer;
            if (SessionUser == null)
            {
                filterContext.Result = RedirectToAction("Index", "Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserNo { set; get; }
    }
}
