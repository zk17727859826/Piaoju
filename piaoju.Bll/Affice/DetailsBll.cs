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
        /// 获得操作员记录
        /// </summary>
        /// <param name="strStart">开始时间</param>
        /// <param name="strEnd">结束时间</param>
        /// <param name="oper">操作员</param>
        /// <param name="page">当前页码</param>
        /// <param name="pagesize">每页的数量</param>
        /// <returns></returns>
        public List<Tdetails> GetDetailsByOper(string strStart, string strEnd, string oper, string type, string man, string xueyh, string bianm, int page, int pagesize)
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

            DetailsDal dal = new DetailsDal();
            return dal.GetDetailsByOper(dtStart, dtEnd, oper, type, man, xueyh, bianm, iStartPos, iEndPos);
        }

        /// <summary>
        /// 获得操作员记录数量
        /// </summary>
        /// <param name="strStart">开始时间</param>
        /// <param name="strEnd">结束时间</param>
        /// <param name="oper">操作员</param>
        /// <param name="totalmoney">总金额</param>
        /// <returns></returns>
        public int GetDetailsByOperCount(string strStart, string strEnd, string oper, string type, string man, string xueyh, string bianm, out decimal totalmoney)
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

            DetailsDal dal = new DetailsDal();
            return dal.GetDetailsByOperCount(dtStart, dtEnd, oper, type, man, xueyh, bianm, out totalmoney);
        }

        /// <summary>
        /// 添加缴费记录
        /// </summary>
        /// <param name="model">缴费实体</param>
        /// <returns></returns>
        public bool Add(Tdetails model)
        {
            DetailsDal dal = new DetailsDal();
            return dal.Add(model);
        }

        /// <summary>
        /// 通过编码获得缴费明细
        /// </summary>
        /// <param name="bianma">编码</param>
        /// <returns></returns>
        public Tdetails GetDetailById(string bianma)
        {
            DetailsDal dal = new DetailsDal();
            return dal.GetDetailById(bianma);
        }

        /// <summary>
        /// 获得操作员记录
        /// </summary>
        /// <param name="xueyh">学员号</param>
        /// <returns></returns>
        public List<Tdetails> GetDetailsByXueyuan(string xueyh)
        {
            DetailsDal dal = new DetailsDal();
            return dal.GetDetailsByXueyuan(xueyh);
        }
    }
}
