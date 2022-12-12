using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CaThiBLL
    {
        CaThiDAL ct_dal = new CaThiDAL();
        public CaThi[] GetList()
        {
            return ct_dal.GetList();
        }
        public void Insert(CaThi ct)
        {
            ct_dal.Insert(ct);
        }
        public void Delete(string condition)
        {
            ct_dal.Delete(condition);
        }
        public bool Generate(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                return false;
            }
            bool sang = true;
            while (start <= end)
            {
                CaThi ct = new CaThi();
                ct.NgayThi = start;
                ct.BuoiThi = (sang == true) ? "Sáng" : "Chiều";
                ct.MaCa = ct.BuoiThi[0].ToString() + ct.NgayThi.Year.ToString() + ct.NgayThi.Month.ToString() + ct.NgayThi.Day.ToString();
                if (!sang)
                {
                    start = start.AddDays(1);
                }
                sang = !sang;
                this.Insert(ct);
            }
            return true;
        }
    }
}
