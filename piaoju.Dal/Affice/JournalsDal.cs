using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Dal.Affice
{
    public class JournalsDal
    {
        /// <summary>
        /// 获得日志记录
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="oper">操作人员</param>
        /// <param name="iStartPos">开始记录号</param>
        /// <param name="iEndPos">结束记录号</param>
        /// <returns></returns>
        public List<Tjournals> GetLog(DateTime? dtStart, DateTime? dtEnd, string oper,int iStartPos,int iEndPos)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                IEnumerable<Tjournals> log = ims.Tjournals;
                if (dtStart != null)
                {
                    log = log.Where(p => p.riqi >= dtStart.Value);
                }
                if (dtEnd != null)
                {
                    log = log.Where(p => p.riqi < dtEnd.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(oper))
                {
                    log = log.Where(p => p.@operator == oper);
                }

                if (iStartPos == 0 && iEndPos == 0)
                {
                    return log.ToList();
                }
                else
                {
                    return log.Skip(iStartPos - 1).Take(iEndPos - iStartPos + 1).ToList();
                }
            }
        }

        /// <summary>
        /// 获得日志记录数量
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="oper">操作人员</param>
        /// <returns></returns>
        public int GetLogCount(DateTime? dtStart, DateTime? dtEnd, string oper)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                IEnumerable<Tjournals> log = ims.Tjournals;
                if (dtStart != null)
                {
                    log = log.Where(p => p.riqi >= dtStart.Value);
                }
                if (dtEnd != null)
                {
                    log = log.Where(p => p.riqi < dtEnd.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(oper))
                {
                    log = log.Where(p => p.@operator == oper);
                }

                return log.Count();
            }
        }
    }
}
