using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using piaoju.Model;

namespace piaoju.Dal.Affice
{
    public class BianmaDal
    {
        /// <summary>
        /// 获得最新的编码
        /// </summary>
        /// <param name="oper">操作人员</param>
        /// <returns></returns>
        public string GetNewBianMa(string oper)
        {
            using (detailsEntities ims = new detailsEntities())
            {
                oper = oper.ToUpper();
                string riqi = DateTime.Now.ToString("yyyyMMdd");
                var bianmas = ims.Tbianma.Where(p => p.@operator == oper && p.riqi == riqi);
                int index = 1;
                if (bianmas.Count() > 0)
                {
                    index = Convert.ToInt32(bianmas.First().bianmaBased) + 1;
                }
                return string.Format("{0}{1}{2}", riqi, oper, index.ToString().PadLeft(4, '0'));
            }
        }
    }
}
