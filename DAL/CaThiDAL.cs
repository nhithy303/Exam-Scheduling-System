using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CaThiDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private CaThi GetCaThiFromDataRow(DataRow row)
        {
            CaThi ct = new CaThi();
            ct.MaCa = row["MaCa"].ToString();
            ct.NgayThi = DateTime.Parse(row["NgayThi"].ToString());
            ct.BuoiThi = row["BuoiThi"].ToString();
            return ct;
        }
        public CaThi[] GetList()
        {
            CaThi[] list = null;
            DataTable table = null;
            int n = 0;
            table = da.ExecuteQuery("select * from CaThi order by NgayThi, MaCa desc");
            n = table.Rows.Count;
            if (n == 0)
            {
                return null;
            }
            list = new CaThi[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetCaThiFromDataRow(table.Rows[i]);
            }
            return list;
        }
        public void Insert(CaThi ct)
        {
            string query = String.Format("insert into CaThi values ('{0}', '{1}', N'{2}')",
                ct.MaCa, ct.NgayThi.ToString("yyyy/MM/dd"), ct.BuoiThi);
            da.ExecuteNonQuery(query);
        }
        public void Delete(string condition)
        {
            string query = "delete from CaThi";
            if (condition != String.Empty)
            {
                query += " where " + condition;
            }
            da.ExecuteNonQuery(query);
        }
    }
}
