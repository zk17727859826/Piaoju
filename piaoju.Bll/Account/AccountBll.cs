using piaoju.Dal.Account;
using piaoju.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piaoju.Bll
{
    public partial class AccountBll
    {
        /// <summary>
        /// 获得用户信息
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <returns></returns>
        public Toperator GetUser(string userno)
        {
            AccountDal dal = new AccountDal();
            return dal.GetUser(userno);
        }

        /// <summary>
        /// 获得用户列表
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <param name="username">用户姓名</param>
        /// <returns></returns>
        public List<Toperator> GetUsers(string userno, string username)
        {
            AccountDal dal = new AccountDal();
            return dal.GetUsers(userno, username);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public bool UpdatePassword(string userno, string password)
        {
            AccountDal dal = new AccountDal();
            return dal.UpdatePassword(userno, password);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public bool UpdateUser(Toperator user)
        {
            AccountDal dal = new AccountDal();
            return dal.UpdateUser(user);
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public bool AddUser(Toperator user)
        {
            AccountDal dal = new AccountDal();
            return dal.AddUser(user);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userno">用户编码</param>
        /// <returns></returns>
        public bool DeleteUser(string userno)
        {
            AccountDal dal = new AccountDal();
            return dal.DeleteUser(userno);
        }
    }
}
