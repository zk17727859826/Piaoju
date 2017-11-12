using jiax.Model;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Dal.Affice
{
    public class TxueyuanDal
    {
        /// <summary>
        /// 获得学员信息
        /// </summary>
        /// <param name="xueyh">学员号</param>
        /// <param name="shenfhm">身份号码</param>
        /// <returns></returns>
        public jiax.Model.Txueyuan GetXueyuan(string xueyh, string shenfhm)
        {
            if (string.IsNullOrEmpty(xueyh) && string.IsNullOrEmpty(shenfhm))
            {
                return default(jiax.Model.Txueyuan);
            }
            using (jiaxEntities ims = new jiaxEntities())
            {
                IEnumerable<jiax.Model.Txueyuan> xueyuan = ims.Txueyuan;
                if (!string.IsNullOrEmpty(xueyh))
                {
                    xueyuan = xueyuan.Where(p => p.Xueyh == xueyh);
                }
                if (!string.IsNullOrEmpty(shenfhm))
                {
                    xueyuan = xueyuan.Where(p => p.Shenfhm == shenfhm);
                }

                if (xueyuan.Count() == 1)
                {
                    return xueyuan.FirstOrDefault();
                }
                else
                {
                    return default(jiax.Model.Txueyuan);
                }
            }
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
            using (jiaxEntities ims = new jiaxEntities())
            {
                IEnumerable<jiax.Model.Txueyuan> xueyuan = ims.Txueyuan;
                if (!string.IsNullOrEmpty(xueyh))
                {
                    xueyuan = xueyuan.Where(p => p.Xueyh.Contains(xueyh));
                }
                if (!string.IsNullOrEmpty(xingming))
                {
                    xueyuan = xueyuan.Where(p => p.Xingming.Contains(xingming));
                }
                if (!string.IsNullOrEmpty(shenfhm))
                {
                    xueyuan = xueyuan.Where(p => p.Shenfhm.Contains(shenfhm));
                }
                return xueyuan.Take(30).ToList();
            }
        }
    }
}
