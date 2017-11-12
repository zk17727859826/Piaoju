using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace piaoju.Dal.Affice
{
    public class DetailsDal
    {
        /// <summary>
        /// 通过编码获得缴费明细
        /// </summary>
        /// <param name="bianma">编码</param>
        /// <returns></returns>
        public Tdetails GetDetailById(string bianma)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                Tdetails detail = ims.Tdetails.Where(p => p.bianm == bianma).FirstOrDefault();
                return detail;
            }
        }

        /// <summary>
        /// 获得操作员记录
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="oper">操作员</param>
        /// <param name="iStartPos">开始记录号</param>
        /// <param name="iEndPos">结束记录号</param>
        /// <returns></returns>
        public List<Tdetails> GetDetailsByOper(DateTime? dtStart, DateTime? dtEnd, string oper, string type, string man, string xueyh, string bianm, int iStartPos, int iEndPos)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                IEnumerable<Tdetails> details = ims.Tdetails;
                if (dtStart != null)
                {
                    details = details.Where(p => p.riqi >= dtStart.Value);
                }
                if (dtEnd != null)
                {
                    details = details.Where(p => p.riqi < dtEnd.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(oper))
                {
                    details = details.Where(p => p.@operator.Contains(oper));
                }
                if (!string.IsNullOrEmpty(type))
                {
                    details = details.Where(p => p.type == type);
                }
                if (!string.IsNullOrEmpty(man))
                {
                    details = details.Where(p => p.man.Contains(man));
                }
                if (!string.IsNullOrEmpty(xueyh))
                {
                    details = details.Where(p => p.xueyh.Contains(xueyh));
                }
                if (!string.IsNullOrEmpty(bianm))
                {
                    details = details.Where(p => p.bianm.Contains(bianm));
                }

                if (iStartPos == 0 && iEndPos == 0)
                {
                    return details.ToList();
                }
                else
                {
                    return details.Skip(iStartPos - 1).Take(iEndPos - iStartPos + 1).ToList();
                }
            }
        }

        /// <summary>
        /// 获得操作员记录数量
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="oper">操作员</param>
        /// <param name="totalmoney">总金额</param>
        /// <returns></returns>
        public int GetDetailsByOperCount(DateTime? dtStart, DateTime? dtEnd, string oper, string type, string man, string xueyh, string bianm, out decimal totalmoney)
        {
            totalmoney = 0;
            using (detailsEntities ims = new detailsEntities())
            {
                IEnumerable<Tdetails> details = ims.Tdetails;
                if (dtStart != null)
                {
                    details = details.Where(p => p.riqi >= dtStart.Value);
                }
                if (dtEnd != null)
                {
                    details = details.Where(p => p.riqi < dtEnd.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(oper))
                {
                    details = details.Where(p => p.@operator.Contains(oper));
                }
                if (!string.IsNullOrEmpty(type))
                {
                    details = details.Where(p => p.type == type);
                }
                if (!string.IsNullOrEmpty(man))
                {
                    details = details.Where(p => p.man.Contains(man));
                }
                if (!string.IsNullOrEmpty(xueyh))
                {
                    details = details.Where(p => p.xueyh.Contains(xueyh));
                }
                if (!string.IsNullOrEmpty(bianm))
                {
                    details = details.Where(p => p.bianm.Contains(bianm));
                }

                totalmoney = details.Sum(p => p.money);
                return details.Count();
            }
        }

        /// <summary>
        /// 获得操作员记录
        /// </summary>
        /// <param name="xueyh">学员号</param>
        /// <returns></returns>
        public List<Tdetails> GetDetailsByXueyuan(string xueyh)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                IEnumerable<Tdetails> details = ims.Tdetails;
                details = details.Where(p => p.xueyh == xueyh);
                return details.ToList();
            }
        }

        /// <summary>
        /// 添加缴费记录
        /// </summary>
        /// <param name="model">缴费实体</param>
        /// <returns></returns>
        public bool Add(Tdetails model)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    ims.Tdetails.Add(model);
                    ims.SaveChanges();

                    Tjournals jm = new Tjournals()
                    {
                        id = DateTime.Now.Year.ToString() + (DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()) + (DateTime.Now.Day > 9 ? DateTime.Now.Day.ToString() : "0" + DateTime.Now.Day.ToString()) + (DateTime.Now.Hour > 9 ? DateTime.Now.Hour.ToString() : "0" + DateTime.Now.Hour.ToString()) + (DateTime.Now.Minute > 9 ? DateTime.Now.Minute.ToString() : "0" + DateTime.Now.Minute.ToString()) + (DateTime.Now.Second > 9 ? DateTime.Now.Second.ToString() : "0" + DateTime.Now.Second.ToString()) + new Random().Next(10, 99).ToString(),
                        @operator = model.@operator,
                        operator_name = model.operator_name,
                        riqi = model.riqi,
                        thing = "进行收费操作：收取【" + model.man + "】" + "【" + model.type + "】" + "【" + model.money + "】元钱"
                    };
                    ims.Tjournals.Add(jm);
                    ims.SaveChanges();

                    string riqi = DateTime.Now.ToString("yyyyMMdd");
                    int iCount = ims.Tbianma.Where(p => p.riqi == riqi && p.@operator == model.@operator).Count();
                    if (iCount == 0)
                    {
                        Tbianma bianma = new Tbianma()
                        {
                            @operator = model.@operator,
                            bianmaBased = "1",
                            riqi = riqi
                        };
                        ims.Tbianma.Add(bianma);
                        ims.SaveChanges();
                    }
                    else
                    {
                        Tbianma bianma = ims.Tbianma.Where(p => p.@operator == model.@operator && p.riqi == riqi).First();
                        bianma.bianmaBased = (Convert.ToInt32(bianma.bianmaBased) + 1).ToString();
                        ims.SaveChanges();
                    }
                    scope.Complete();
                    return true;
                }
            }
        }
    }
}
