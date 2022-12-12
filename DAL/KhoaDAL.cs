using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class KhoaDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private Khoa GetKhoaFromDataRow(DataRow row)
        {
            Khoa kh = new Khoa();
            kh.MaKhoa = row["MaKhoa"].ToString();
            kh.TenKhoa = row["TenKhoa"].ToString();
            return kh;
        }
        public Khoa[] GetList()
        {
            Khoa[] list = null;
            DataTable table = null;
            int n = 0;
            table = da.ExecuteQuery("select * from Khoa order by TenKhoa");
            n = table.Rows.Count;
            if (n == 0)
            {
                return null;
            }
            list = new Khoa[n + 1];
            list[0] = new Khoa();
            list[0].MaKhoa = "All"; list[0].TenKhoa = "All";
            for (int i = 1; i <= n; i++)
            {
                list[i] = GetKhoaFromDataRow(table.Rows[i - 1]);
            }
            return list;
        }
        public void Insert(Khoa kh)
        {
            string query = String.Format("insert into Khoa values ('{0}',N'{1}')", kh.MaKhoa, kh.TenKhoa);
            da.ExecuteNonQuery(query);
        }
    }
}
