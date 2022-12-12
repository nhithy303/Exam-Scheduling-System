using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LichThiDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private LichThi GetLichThiFromDataRow(DataRow row)
        {
            LichThi lt = new LichThi();
            lt.MaCa = row["MaCa"].ToString();
            lt.NgayThi = DateTime.Parse(row["NgayThi"].ToString()).ToString("dd/MM/yyyy");
            lt.BuoiThi = row["BuoiThi"].ToString();
            lt.MonThi = row["TenMon"].ToString();
            lt.PhongThi = row["MaPhong"].ToString();
            return lt;
        }
        public LichThi[] GetList(string condition)
        {
            LichThi[] list = null;
            DataTable table = null;
            int n = 0;
            string query = "select ct.MaCa, ct.NgayThi, ct.BuoiThi, mt.TenMon, pbpt.MaPhong"
                + " from CaThi ct inner join MonThi mt on ct.MaCa = mt.MaCa"
                + " inner join PhanBoPhongThi pbpt on mt.MaMon = pbpt.MaMon"
                + " order by ct.NgayThi, ct.MaCa desc";
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
            list = new LichThi[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetLichThiFromDataRow(table.Rows[i]);
            }
            return list;
        }
    }
}
