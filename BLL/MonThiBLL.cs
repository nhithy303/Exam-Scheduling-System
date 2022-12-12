using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MonThiBLL
    {
        MonThiDAL mt_dal = new MonThiDAL();
        public MonThi[] GetList(string condition)
        {
            return mt_dal.GetList(condition);
        }
        public void Insert(MonThi mt)
        {
            mt_dal.Insert(mt);
        }
        public void Update(string maca, string mamon)
        {
            mt_dal.Update(maca, mamon);
        }
    }
}
