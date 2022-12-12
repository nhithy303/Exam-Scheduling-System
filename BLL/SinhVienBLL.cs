using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SinhVienBLL
    {
        SinhVienDAL sv_dal = new SinhVienDAL();
        public SinhVien[] GetList(string condition)
        {
            return sv_dal.GetList(condition);
        }
        public void Insert(SinhVien sv)
        {
            sv_dal.Insert(sv);
        }
    }
}
