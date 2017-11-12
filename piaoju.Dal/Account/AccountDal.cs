using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Dal.Account
{
    public class AccountDal
    {
        /// <summary>
        /// 获得用户信息
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <returns></returns>
        public Toperator GetUser(string userno)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                Toperator user = ims.Toperator.Where(p => p.bianm == userno).FirstOrDefault();
                return user;
            }
        }

        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <param name="username">用户姓名</param>
        /// <returns></returns>
        public List<Toperator> GetUsers(string userno, string username)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                var models = ims.Toperator.Where(p => p.bianm.Contains(userno) && p.xingming.Contains(username));
                return models.ToList();
            }
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool UpdatePassword(string userno, string password)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                Toperator user = ims.Toperator.Where(p => p.bianm == userno).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("用户不存在");
                }

                user.password = password;
                return ims.SaveChanges() == 1;
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public bool UpdateUser(Toperator user)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                Toperator model = ims.Toperator.Where(p => p.bianm == user.bianm).FirstOrDefault();
                if (model == null)
                {
                    throw new Exception("用户不存在");
                }

                model.power = user.power;
                model.state = user.state;
                model.xingbie = user.xingbie;
                model.xingming = user.xingming;

                return ims.SaveChanges() == 1;
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public bool AddUser(Toperator user)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                ims.Toperator.Add(user);
                return ims.SaveChanges() == 1;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <returns></returns>
        public bool DeleteUser(string userno)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                Toperator user = ims.Toperator.Where(p => p.bianm == userno).FirstOrDefault();
                if (user != null)
                {
                    ims.Toperator.Remove(user);
                }
                return ims.SaveChanges() == 1;
            }
        }
    }
}
