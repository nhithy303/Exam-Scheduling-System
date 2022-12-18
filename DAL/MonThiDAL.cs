using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MonThiDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private MonThi GetMonThiFromDataRow(DataRow row)
        {
            MonThi mt = new MonThi();
            mt.MaMon = row["MaMon"].ToString();
            mt.TenMon = row["TenMon"].ToString();
            if (row["SoLuongSV"].ToString() != String.Empty)
            {
                mt.SoLuongSV = int.Parse(row["SoLuongSV"].ToString());
            }
            if (row["SoPhong"].ToString() != String.Empty)
            {
                mt.SoPhong = int.Parse(row["SoPhong"].ToString());
            }
            mt.MaCa = row["MaCa"].ToString();
            return mt;
        }
        public MonThi[] GetList(string condition)
        {
            MonThi[] list = null;
            DataTable table = null;
            int n = 0;
            string query = "select * from MonThi";
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
            list = new MonThi[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetMonThiFromDataRow(table.Rows[i]);
            }
            return list;
        }
        public void Insert(MonThi mt)
        {
            string query = String.Format("insert into MonThi values" +
                "('{0}',N'{1}',{2},{3},'{4}')",
                mt.MaMon, mt.TenMon, mt.SoLuongSV, mt.SoPhong, mt.MaCa);
            da.ExecuteNonQuery(query);
        }
        public void UpdateMaCa(string maca, string mamon)
        {
            string query = "update MonThi set MaCa = ";
            if (maca == "null")
            {
                query += maca;
            }
            else
            {
                query += "'" + maca + "'";
            }
            if (mamon != String.Empty)
            {
                query += " where MaMon = '" + mamon + "'"; ;
            }
            da.ExecuteNonQuery(query);
        }
        public void UpdateSoPhong(string succhua)
        {
            string query = "update MonThi set SoPhong = ceiling(SoLuongSV * 1.0 / " + succhua + ")";
            da.ExecuteNonQuery(query);
        }
    }
}
