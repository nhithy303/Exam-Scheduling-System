using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SinhVienDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private SinhVien GetSinhVienFromDataRow(DataRow row)
        {
            SinhVien sv = new SinhVien();
            sv.MSSV = row["MSSV"].ToString();
            sv.HoSV = row["HoSV"].ToString();
            sv.TenSV = row["TenSV"].ToString();
            sv.NgaySinh = DateTime.Parse(row["NgaySinh"].ToString()).ToString("dd/MM/yyyy");
            sv.GioiTinh = row["GioiTinh"].ToString();
            sv.MaKhoa = row["MaKhoa"].ToString();
            return sv;
        }
        public SinhVien[] GetList(string condition)
        {
            SinhVien[] list = null;
            DataTable table = null;
            int n = 0;
            string query = "select * from SinhVien";
            if (condition != "")
            {
                query += " where " + condition;
            }
            table = da.ExecuteQuery(query);
            n = table.Rows.Count;
            if (n == 0)
            {
                return null;
            }
            list = new SinhVien[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetSinhVienFromDataRow(table.Rows[i]);
            }
            return list;
        }
        public void Insert(SinhVien sv)
        {
            string query = String.Format("insert into SinhVien values" +
                "('{0}',N'{1}',N'{2}','{3}',N'{4}',N'{5}')",
                sv.MSSV, sv.HoSV, sv.TenSV, sv.NgaySinh, sv.GioiTinh, sv.MaKhoa);
            da.ExecuteNonQuery(query);
        }
    }
}
