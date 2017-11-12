using piaoju.Dal.Affice;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Bll.Affice
{
    public class TxueyuanBll
    {
        /// <summary>
        /// 获得学员信息
        /// </summary>
        /// <param name="xueyh">学员号</param>
        /// <param name="shenfhm">身份号码</param>
        /// <returns></returns>
        public jiax.Model.Txueyuan GetXueyuan(string xueyh, string shenfhm)
        {
            TxueyuanDal dal = new TxueyuanDal();
            return dal.GetXueyuan(xueyh, shenfhm);
        }

        /// <summary>
        /// 获得学员明细
        /// </summary>
        /// <param name="xueyh">学员号</param>
        /// <param name="xingming">姓名</param>
        /// <param name="shenfhm">身份号码</param>
        /// <returns></returns>
        public List<jiax.Model.Txueyuan> GetXueyuans(string xueyh, string xingming, string shenfhm)
        {
            TxueyuanDal dal = new TxueyuanDal();
            return dal.GetXueyuans(xueyh, xingming, shenfhm);
        }
    }
}
