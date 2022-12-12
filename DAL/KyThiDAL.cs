using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class KyThiDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        public KyThi GetInfo()
        {
            KyThi kt = null;
            DataTable table = null;
            table = da.ExecuteQuery("select * from KyThi");
            if (table.Rows.Count == 0)
            {
                return null;
            }
            kt = new KyThi();
            DataRow row = table.Rows[0];
            kt.NamHoc = row["NamHoc"].ToString();
            kt.HocKy = int.Parse(row["HocKy"].ToString());
            kt.NgayBatDau = DateTime.Parse(row["NgayBatDau"].ToString());
            kt.NgayKetThuc = DateTime.Parse(row["NgayKetThuc"].ToString());
            return kt;
        }
        public void Insert(KyThi kt)
        {
            string query = String.Format("insert into KyThi values ('{0}', {1}, '{2}', '{3}')",
                kt.NamHoc, kt.HocKy, kt.NgayBatDau, kt.NgayKetThuc);
            da.ExecuteNonQuery(query);
        }
        public void Delete()
        {
            string query = "delete from KyThi";
            da.ExecuteQuery(query);
        }
    }
}
