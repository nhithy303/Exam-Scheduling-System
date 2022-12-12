using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LichThiBLL
    {
        LichThiDAL lt_dal = new LichThiDAL();
        public LichThi[] GetList(string condition)
        {
            return lt_dal.GetList(condition);
        }
    }
}
