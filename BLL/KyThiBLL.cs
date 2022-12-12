using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class KyThiBLL
    {
        KyThiDAL kt_dal = new KyThiDAL();
        public KyThi GetInfo()
        {
            return kt_dal.GetInfo();
        }
        public void Insert(KyThi kt)
        {
            kt_dal.Insert(kt);
        }
        public void Delete()
        {
            kt_dal.Delete();
        }
    }
}
