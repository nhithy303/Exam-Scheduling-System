using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class KhoaBLL
    {
        KhoaDAL kh_dal = new KhoaDAL();
        public Khoa[] GetList()
        {
            return kh_dal.GetList();
        }
        public void Insert(Khoa kh)
        {
            kh_dal.Insert(kh);
        }
    }
}
