using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using piaoju.Bll;
using piaoju.Model;

namespace jiax_drive_affice.Controllers
{
    public class IndexController : BaseController
    {
        public ActionResult Index()
        {
            Toperator user = GetUser();
            return View(user);
        }

        public ActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Users(string userno, string username)
        {
            try
            {
                AccountBll bll = new AccountBll();
                List<Toperator> users = bll.GetUsers(userno, username);
                return Json(new
                {
                    success = true,
                    data = users
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

        [HttpPost]
        public JsonResult SUser(string id)
        {
            try
            {
                AccountBll bll = new AccountBll();
                Toperator user = bll.GetUser(id);
                return Json(new
                {
                    success = true,
                    data = user
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

        [HttpPost]
        public JsonResult UserAction(string bianm, string xingming, string sex, string power, string actiontype)
        {
            try
            {
                Toperator user = new Toperator()
                {
                    bianm = bianm,
                    xingming = xingming,
                    xingbie = sex,
                    power = power
                };

                AccountBll bll = new AccountBll();
                string msg = string.Empty;
                if (actiontype == "add")
                {
                    user.password = "123456";
                    if (bll.AddUser(user))
                    {
                        msg = "添加成功";
                    }
                    else
                    {
                        throw new Exception("添加失败");
                    }
                }
                else if (actiontype == "edit")
                {
                    if (bll.UpdateUser(user))
                    {
                        msg = "更新成功";
                    }
                    else
                    {
                        throw new Exception("更新失败");
                    }
                }

                return Json(new
                {
                    success = true,
                    msg = msg,
                    data = user
                });
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message;
                if (msg.IndexOf("PK__Toperator__1A14E395") > -1)
                {
                    msg = "用户代码【" + bianm + "】已存在";
                }
                return Json(new
                {
                    success = false,
                    msg = msg
                });
            }
        }

        [HttpPost]
        public JsonResult DeleteUser(string id)
        {
            try
            {
                if (id.ToUpper() == "ADMIN")
                {
                    throw new Exception("ADMIN是超级管理员，不能删除！");
                }
                AccountBll bll = new AccountBll();
                if (bll.DeleteUser(id))
                {
                    return Json(new
                    {
                        success = true
                    });
                }
                else
                {
                    throw new Exception("删除失败");
                }
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


        public ActionResult Password()
        {
            Toperator user = GetUser();
            return View(user);
        }

        [HttpPost]
        public JsonResult Password(string oldpwd, string newpwd)
        {
            try
            {
                if (string.IsNullOrEmpty(oldpwd) || string.IsNullOrEmpty(newpwd))
                {
                    throw new Exception("新旧密码都不能为空");
                }

                string userno = SessionUser.UserNo;
                AccountBll bll = new AccountBll();
                Toperator user = bll.GetUser(userno);
                if (user == null)
                {
                    throw new Exception("登陆用户不存，请重新登陆");
                }

                if (user.password != oldpwd)
                {
                    throw new Exception("旧密码输入错误");
                }

                if (user.password == newpwd)
                {
                    throw new Exception("新密码和旧密码不能一致");
                }

                if (!bll.UpdatePassword(userno, newpwd))
                {
                    throw new Exception("修改密码失败");
                }

                return Json(new
                {
                    success = true
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
