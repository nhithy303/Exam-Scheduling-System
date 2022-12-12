using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PhanBoPhongThiBLL
    {
        PhanBoPhongThiDAL pbpt_dal = new PhanBoPhongThiDAL();
        public PhanBoPhongThi[] GetList(string condition)
        {
            return pbpt_dal.GetList(condition);
        }
        public void Insert(PhanBoPhongThi pbpt)
        {
            pbpt_dal.Insert(pbpt);
        }
        public void Delete(string condition)
        {
            pbpt_dal.Delete(condition);
        }
    }
}
