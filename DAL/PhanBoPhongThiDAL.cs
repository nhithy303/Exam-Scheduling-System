using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PhanBoPhongThiDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private PhanBoPhongThi GetPhanBoPhongThiFromDataRow(DataRow row)
        {
            PhanBoPhongThi pbpt = new PhanBoPhongThi();
            pbpt.MaMon = row["MaMon"].ToString();
            pbpt.MaPhong = row["MaPhong"].ToString();
            return pbpt;
        }
        public PhanBoPhongThi[] GetList(string condition)
        {
            PhanBoPhongThi[] list = null;
            DataTable table = null;
            int n = 0;
            string query = "select * from PhanBoPhongThi";
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
            list = new PhanBoPhongThi[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetPhanBoPhongThiFromDataRow(table.Rows[i]);
            }
            return list;
        }
        public void Insert(PhanBoPhongThi pbpt)
        {
            string query = String.Format("insert into PhanBoPhongThi values ('{0}','{1}')", pbpt.MaMon, pbpt.MaPhong);
            da.ExecuteNonQuery(query);
        }
        public void Delete(string condition)
        {
            string query = "delete from PhanBoPhongThi";
            if (condition != String.Empty)
            {
                query += " where " + condition;
            }
            da.ExecuteNonQuery(query);
        }
    }
}
