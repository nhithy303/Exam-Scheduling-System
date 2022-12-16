using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TaiKhoanBLL
    {
        TaiKhoanDAL tk_dal = new TaiKhoanDAL();
        public string CheckLoginBLL(TaiKhoan tk)
        {
            if (tk.TenDangNhap == String.Empty)
            {
                return "empty tendangnhap";
            }
            if (tk.MatKhau == String.Empty)
            {
                return "empty matkhau";
            }
            bool successful = tk_dal.CheckLoginDAL(tk);
            if (!successful)
            {
                return "failed";
            }
            return "successful";
        }
        public string Update(TaiKhoan tk, string matkhaucu, string matkhaumoi, string xacnhanmatkhau)
        {
            if (matkhaucu == String.Empty || matkhaumoi == String.Empty || xacnhanmatkhau == String.Empty)
            {
                return "empty";
            }
            if (matkhaumoi != xacnhanmatkhau)
            {
                return "unmatched";
            }
            if (tk.MatKhau != matkhaucu)
            {
                return "wrong password";
            }
            tk_dal.Update(tk, matkhaumoi);
            return "success";
        }
        public void Insert(string mssv)
        {
            tk_dal.Insert(mssv);
        }
    }
}
