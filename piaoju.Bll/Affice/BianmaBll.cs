using piaoju.Dal.Affice;
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
        /// 获得最新的编码
        /// </summary>
        /// <param name="oper">操作人员</param>
        /// <returns></returns>
        public string GetNewBianMa(string oper)
        {
            BianmaDal dal = new BianmaDal();
            return dal.GetNewBianMa(oper);
        }
    }
}
