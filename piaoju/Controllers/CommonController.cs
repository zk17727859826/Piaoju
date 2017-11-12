using piaoju.Bll;
using piaoju.Bll.Affice;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace piaoju.Controllers
{
    public class CommonController : Controller
    {
        public PartialViewResult Operator(string id, string name, string classname)
        {
            AccountBll bll = new AccountBll();
            List<Toperator> users = bll.GetUsers("", "");
            ViewBag.ID = id;
            ViewBag.ClassName = classname;
            ViewBag.Name = name;
            return PartialView(users);
        }

        public PartialViewResult Baomd(string id, string name, string classname)
        {
            ZhidianBll bll = new ZhidianBll();
            List<jiax.Model.Tzidian> baomd = bll.GetBaomd();
            ViewBag.ID = id;
            ViewBag.ClassName = classname;
            ViewBag.Name = name;
            return PartialView(baomd);
        }

        public PartialViewResult Paytype(string id, string name, string classname)
        {
            ZhidianBll bll = new ZhidianBll();
            List<Tzhidian1> paytype = bll.GetPaytype();
            ViewBag.ID = id;
            ViewBag.ClassName = classname;
            ViewBag.Name = name;
            return PartialView(paytype);
        }
    }
}
