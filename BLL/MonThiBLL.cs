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
        public void UpdateMaCa(string maca, string mamon)
        {
            mt_dal.UpdateMaCa(maca, mamon);
        }
        public void UpdateSoPhong(string succhua)
        {
            mt_dal.UpdateSoPhong(succhua);
        }
    }
}
