using piaoju.Dal.Affice;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Bll.Affice
{
    public partial class AfficeBll
    {
        /// <summary>
        /// 获得日志记录
        /// </summary>
        /// <param name="strStart">开始时间</param>
        /// <param name="strEnd">结束时间</param>
        /// <param name="oper">操作人员</param>
        /// <param name="page">当前页码</param>
        /// <param name="pagesize">每一页的数量</param>
        /// <returns></returns>
        public List<Tjournals> GetLog(string strStart, string strEnd, string oper, int page, int pagesize)
        {
            DateTime dt = new DateTime();
            DateTime? dtStart = null;
            if (DateTime.TryParse(strStart, out dt))
            {
                dtStart = dt;
            }

            DateTime? dtEnd = null;
            if (DateTime.TryParse(strEnd, out dt))
            {
                dtEnd = dt;
            }

            int iStartPos = 0, iEndPos = 0;

            if (page == 0 || pagesize == 0)
            {

            }
            else
            {
                iStartPos = (page - 1) * pagesize + 1;
                iEndPos = iStartPos + pagesize - 1;
            }

            JournalsDal dal = new JournalsDal();
            return dal.GetLog(dtStart, dtEnd, oper, iStartPos, iEndPos);
        }

        /// <summary>
        /// 获得日志记录数量
        /// </summary>
        /// <param name="strStart">开始时间</param>
        /// <param name="strEnd">结束时间</param>
        /// <param name="oper">操作人员</param>
        /// <returns></returns>
        public int GetLogCount(string strStart, string strEnd, string oper)
        {
            DateTime dt = new DateTime();
            DateTime? dtStart = null;
            if (DateTime.TryParse(strStart, out dt))
            {
                dtStart = dt;
            }

            DateTime? dtEnd = null;
            if (DateTime.TryParse(strEnd, out dt))
            {
                dtEnd = dt;
            }

            JournalsDal dal = new JournalsDal();
            return dal.GetLogCount(dtStart, dtEnd, oper);
        }
    }
}
