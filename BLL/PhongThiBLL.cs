using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PhongThiBLL
    {
        PhongThiDAL pt_dal = new PhongThiDAL();
        public PhongThi[] GetList()
        {
            return pt_dal.GetList();
        }
        public bool Insert(PhongThi pt)
        {
            if (pt.MaPhong == String.Empty)
            {
                return false;
            }
            return pt_dal.Insert(pt);
        }
        public bool Update(string succhua)
        {
            if (succhua == String.Empty)
            {
                return false;
            }
            if (!succhua.All(char.IsDigit))
            {
                return false;
            }
            pt_dal.Update(succhua);
            return true;
        }
        public bool Delete(PhongThi pt)
        {
            return pt_dal.Delete(pt);
        }
    }
}
