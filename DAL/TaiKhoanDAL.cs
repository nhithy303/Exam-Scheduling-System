using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TaiKhoanDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        public bool CheckLoginDAL(TaiKhoan tk)
        {
            string query = String.Format("select * from TaiKhoan where TenDangNhap = '{0}' and MatKhau = '{1}' and QuanTriVien = {2}",
                tk.TenDangNhap, tk.MatKhau, tk.QuanTriVien);
            DataTable table = null;
            table = da.ExecuteQuery(query);
            if (table.Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
        public void Update(TaiKhoan tk, string matkhaumoi)
        {
            string query = String.Format("update TaiKhoan set MatKhau = '{0}' where TenDangNhap = '{1}' and MatKhau = '{2}'",
                matkhaumoi, tk.TenDangNhap, tk.MatKhau);
            da.ExecuteNonQuery(query);
        }
    }
}