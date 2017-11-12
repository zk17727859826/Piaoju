using jiax.Model;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Dal.Affice
{
    public class ZhidianDal
    {
        /// <summary>
        /// 获得字典信息
        /// </summary>
        /// <returns></returns>
        public Tzhidian GetZhidian(string id)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                return ims.Tzhidian.Where(p => p.id == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得字典信息
        /// </summary>
        /// <returns></returns>
        public List<Tzhidian> GetZhidian()
        {
            using (detailsEntities ims = new detailsEntities())
            {
                return ims.Tzhidian.ToList();
            }
        }

        /// <summary>
        /// 获得子字典信息
        /// </summary>
        /// <param name="zdid">字典ID</param>
        /// <returns></returns>
        public List<Tzhidian1> GetZhidian1(string zdid)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                var zd = ims.Tzhidian1.Where(p => p.upid == zdid);
                return zd.ToList();
            }
        }

        /// <summary>
        /// 添加子字典
        /// </summary>
        /// <param name="zd"></param>
        /// <returns></returns>
        public bool AddZhidian1(Tzhidian1 zd)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                string id = ims.Tzhidian1.Where(p => p.upid == zd.upid).Max(p => p.id);
                string newid = "";
                if (id == null)
                {
                    newid = "00001";
                }
                else
                {
                    newid = (Convert.ToInt32(id) + 1).ToString().PadLeft(5, '0');
                }
                zd.id = newid;

                ims.Tzhidian1.Add(zd);
                return ims.SaveChanges() == 1;
            }
        }

        /// <summary>
        /// 更新子字典
        /// </summary>
        /// <param name="zd"></param>
        /// <returns></returns>
        public bool UpdateZhidian1(Tzhidian1 zd)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                Tzhidian1 zd1 = ims.Tzhidian1.Where(p => p.id == zd.id && p.upid == zd.upid).FirstOrDefault();
                if (zd1 == null)
                {
                    throw new Exception("要编辑的字典不存在");
                }
                zd1.content = zd.content;
                zd1.memo = zd.memo;
                return ims.SaveChanges() == 1;
            }
        }

        /// <summary>
        /// 删除子字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="upid"></param>
        /// <returns></returns>
        public bool DeleteZhidian1(string id, string upid)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                Tzhidian1 zd1 = ims.Tzhidian1.Where(p => p.id == id && p.upid == upid).FirstOrDefault();
                if (zd1 == null)
                {
                    throw new Exception("要删除的字典不存在");
                }
                ims.Tzhidian1.Remove(zd1);
                return ims.SaveChanges() == 1;
            }
        }

        /// <summary>
        /// 获得子字典信息
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <param name="upid">上级字典ID</param>
        /// <returns></returns>
        public Tzhidian1 GetZhidian1(string id, string upid)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                return ims.Tzhidian1.Where(p => p.id == id && p.upid == upid).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得报名点
        /// </summary>
        /// <returns></returns>
        public List<Tzidian> GetBaomd()
        {
            using (jiaxEntities ims = new jiaxEntities())
            {
                var baomd = ims.Tzidian.Where(p => p.ID == 100034);
                return baomd.ToList();
            }
        }
    }
}
