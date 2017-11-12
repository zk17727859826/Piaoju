using ASP.NETClass;
using jiax_drive_affice.Controllers;
using piaoju.Bll;
using piaoju.Bll.Affice;
using piaoju.Model;
using piaoju.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using tg.Helper;
using ThoughtWorks.QRCode.Codec;

namespace piaoju.Controllers
{
    public class AfficeController : BaseController
    {
        int pagesize = 30;

        public ActionResult acceptlist()
        {
            return View();
        }

        [HttpPost]
        public JsonResult acceptlist(string startdate, string enddate, string oper, int page = 1)
        {
            try
            {
                AfficeBll bll = new AfficeBll();
                List<Tdetails> details = bll.GetDetailsByOper(startdate, enddate, oper, "", "", "", "", page, pagesize);
                decimal currentpagemoney = 0;
                if (details.Count > 0)
                {
                    currentpagemoney = details.Sum(p => p.money);
                }
                decimal totalmoney = 0;
                int iTotalCount = bll.GetDetailsByOperCount(startdate, enddate, oper, "", "", "", "", out totalmoney);
                int itotalpage = Convert.ToInt32(Math.Ceiling(iTotalCount / (float)pagesize));
                return Json(new
                {
                    success = true,
                    data = details,
                    footertext = string.Format("当前页金额：<span style=\"color:blue;\">{0}</span>元，总金额：<span style=\"color:blue;\">{1}</span>", currentpagemoney.ToString("#.##"),totalmoney.ToString("#.##")),
                    totalpage = itotalpage == 0 ? 1 : itotalpage,
                    page = page
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

        public ActionResult xueyuanlist(string xueyh, string xingming, string shenfhm)
        {
            TxueyuanBll bll = new TxueyuanBll();
            List<jiax.Model.Txueyuan> xueys = bll.GetXueyuans(xueyh, xingming, shenfhm);
            return View(xueys);
        }

        public ActionResult XueyuanDetails(string id)
        {
            AfficeBll bll = new AfficeBll();
            List<Tdetails> details = bll.GetDetailsByXueyuan(id);

            TxueyuanBll xybll = new TxueyuanBll();
            jiax.Model.Txueyuan xy = xybll.GetXueyuan(id, "");

            ViewBag.xingming = xy.Xingming;
            ViewBag.xinebie = xy.Xingbie_sm;
            ViewBag.shenfhm = xy.Shenfhm;
            ViewBag.xueyh = xy.Xueyh;

            return View(details);
        }

        public ActionResult accept(string id)
        {
            string userno = SessionUser.UserNo;
            AfficeBll bll = new AfficeBll();
            ViewBag.Bianma = bll.GetNewBianMa(userno);

            AccountBll accbll = new AccountBll();
            Toperator user = accbll.GetUser(userno);
            ViewBag.User = user.xingming;

            ViewBag.Xueyh = id;

            return View();
        }

        [HttpPost]
        public JsonResult xueyuan(string xueyh, string shenfhm)
        {
            try
            {
                TxueyuanBll bll = new TxueyuanBll();
                jiax.Model.Txueyuan xueyuan = bll.GetXueyuan(xueyh, shenfhm);
                if (xueyuan != null)
                {
                    return Json(new
                    {
                        success = true,
                        data = xueyuan
                    });
                }
                else
                {
                    throw new Exception("获取人员信息失败");
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

        [HttpPost]
        public JsonResult rmb(string money)
        {
            try
            {
                double dblMoney = Convert.ToDouble(money);
                string strMoney = dblMoney.ToRmb();
                return Json(new
                {
                    success = true,
                    money = strMoney
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
        public JsonResult Add(Tdetails details)
        {
            try
            {
                details.Kaipri = DateTime.Now;
                details.riqi = DateTime.Now.Date;
                UserInfo ui = SessionUser;
                details.@operator = ui.UserNo;
                AccountBll accbll = new AccountBll();
                Toperator user = accbll.GetUser(ui.UserNo);
                details.operator_name = user.xingming;
                AfficeBll bll=new AfficeBll();
                bool isSuccess = bll.Add(details);
                if (!isSuccess) { throw new Exception("保存失败"); }
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

        public ActionResult Detail(string id, string back)
        {
            AfficeBll bll = new AfficeBll();
            Tdetails detail = bll.GetDetailById(id);

            ViewBag.Back = back;
            return View(detail);
        }

        public ActionResult paylist()
        {
            return View();
        }

        [HttpPost]
        public JsonResult paylist(string startdate, string enddate, string oper, string type, string man, string bianm, string xueyh, int page)
        {
            try
            {
                AfficeBll bll = new AfficeBll();
                List<Tdetails> details = bll.GetDetailsByOper(startdate, enddate, oper, type, man, xueyh, bianm, page, pagesize);
                decimal currentpagemoney = 0;
                if (details.Count > 0)
                {
                    currentpagemoney = details.Sum(p => p.money);
                }
                decimal totalmoney = 0;
                int iTotalCount = bll.GetDetailsByOperCount(startdate, enddate, oper, type, man, xueyh, bianm, out totalmoney);
                int itotalpage = Convert.ToInt32(Math.Ceiling(iTotalCount / (float)pagesize));
                return Json(new
                {
                    success = true,
                    data = details,
                    footertext = string.Format("当前页金额：<span style=\"color:blue;\">{0}</span>元，总金额：<span style=\"color:blue;\">{1}</span>", currentpagemoney.ToString("#.##"), totalmoney.ToString("#.##")),
                    totalpage = itotalpage == 0 ? 1 : itotalpage,
                    page = page
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

        public ActionResult Log()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Log(string startdate, string enddate, string oper, int page)
        {
            try
            {
                AfficeBll bll = new AfficeBll();
                List<Tjournals> logs = bll.GetLog(startdate, enddate, oper, page, pagesize);
                int iTotalCount = bll.GetLogCount(startdate, enddate, oper);
                int itotalpage = Convert.ToInt32(Math.Ceiling(iTotalCount / (float)pagesize));

                return Json(new
                {
                    success = true,
                    data = logs,
                    totalpage = itotalpage == 0 ? 1 : itotalpage,
                    page = page
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

        public ActionResult Zidian()
        {
            ZhidianBll bll = new ZhidianBll();
            List<Tzhidian> zdlist = bll.GetZhidian();
            return View(zdlist);
        }

        public ActionResult Zidian1(string id)
        {
            ZhidianBll bll = new ZhidianBll();
            Tzhidian zd = bll.GetZhidian(id);
            ViewBag.ZdContent = zd.content;
            ViewBag.UPID = id;
            List<Tzhidian1> zdlist = bll.GetZhidian1(id);
            return View(zdlist);
        }

        public ActionResult AddZidian1(string id, string upid)
        {
            ZhidianBll bll = new ZhidianBll();
            Tzhidian zd = bll.GetZhidian(upid);
            ViewBag.UPID = upid;
            ViewBag.ID = id;
            ViewBag.ZdContent = zd.content;

            Tzhidian1 zd1 = bll.GetZhidian1(id, upid);
            return View(zd1);
        }

        [HttpPost]
        public JsonResult AddZidian1(Tzhidian1 zd)
        {
            try
            {
                ZhidianBll bll = new ZhidianBll();
                if (!bll.AddZhidian1(zd))
                {
                    throw new Exception("添加失败");
                }
                else
                {
                    return Json(new
                    {
                        success = true
                    });
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

        [HttpPost]
        public JsonResult UpdateZidian1(Tzhidian1 zd)
        {
            try
            {
                ZhidianBll bll = new ZhidianBll();
                if (!bll.UpdateZhidian1(zd))
                {
                    throw new Exception("更新失败");
                }
                else
                {
                    return Json(new
                    {
                        success = true
                    });
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

        [HttpPost]
        public JsonResult DeleteZidian1(string id, string upid)
        {
            try
            {
                ZhidianBll bll = new ZhidianBll();
                if (!bll.DeleteZhidian1(id, upid))
                {
                    throw new Exception("删除失败");
                }
                else
                {
                    return Json(new
                    {
                        success = true
                    });
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

        public ImageResult QRCode(string id)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            string encoding = "Byte";
            switch (encoding)
            {
                case "Byte":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
                case "AlphaNumeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
                    break;
                case "Numeric":
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
                    break;
                default:
                    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    break;
            }

            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeVersion = 6;
            string level = "M";
            switch (level)
            {
                case "L":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                    break;
                case "M":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                    break;
                case "Q":
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    break;
                default:
                    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                    break;
            }

            AfficeBll bll = new AfficeBll();
            Tdetails detail = bll.GetDetailById(id);
            string strData = string.Format("{0}+{1}+{2}+{3}+{4}", detail.baomd, detail.Kaipri, detail.man, detail.money, detail.@operator);
            //文字生成图片
            Image image = qrCodeEncoder.Encode(strData);
            return new ImageResult()
            {
                Image = image,
                ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg
            };
        }
    }
}
