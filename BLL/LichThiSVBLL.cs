using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LichThiSVBLL
    {
        LichThiSVDAL ltsv_dal = new LichThiSVDAL();
        public LichThiSV[] GetList(string mssv)
        {
            return ltsv_dal.GetList(mssv);
        }
    }
}
