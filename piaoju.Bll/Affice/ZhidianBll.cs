using piaoju.Dal.Affice;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Bll.Affice
{
    public class ZhidianBll
    {
        /// <summary>
        /// 获得报名点信息
        /// </summary>
        /// <returns></returns>
        public List<Tzhidian1> GetPaytype()
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.GetZhidian1("00002");
        }

        /// <summary>
        /// 获得报名点
        /// </summary>
        /// <returns></returns>
        public List<jiax.Model.Tzidian> GetBaomd()
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.GetBaomd();
        }

        /// <summary>
        /// 获得字典信息
        /// </summary>
        /// <returns></returns>
        public List<Tzhidian> GetZhidian()
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.GetZhidian();
        }

        /// <summary>
        /// 获得字典信息
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        public Tzhidian GetZhidian(string id)
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.GetZhidian(id);
        }

         /// <summary>
        /// 获得子字典信息
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <param name="upid">上级字典ID</param>
        /// <returns></returns>
        public Tzhidian1 GetZhidian1(string id, string upid)
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.GetZhidian1(id, upid);
        }

        /// <summary>
        /// 获得子字典信息
        /// </summary>
        /// <param name="zdid">字典ID</param>
        /// <returns></returns>
        public List<Tzhidian1> GetZhidian1(string zdid)
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.GetZhidian1(zdid);
        }

        /// <summary>
        /// 添加子字典
        /// </summary>
        /// <param name="zd"></param>
        /// <returns></returns>
        public bool AddZhidian1(Tzhidian1 zd)
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.AddZhidian1(zd);
        }

        /// <summary>
        /// 更新子字典
        /// </summary>
        /// <param name="zd"></param>
        /// <returns></returns>
        public bool UpdateZhidian1(Tzhidian1 zd)
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.UpdateZhidian1(zd);
        }

        /// <summary>
        /// 删除子字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="upid"></param>
        /// <returns></returns>
        public bool DeleteZhidian1(string id, string upid)
        {
            ZhidianDal dal = new ZhidianDal();
            return dal.DeleteZhidian1(id, upid);
        }
    }
}
