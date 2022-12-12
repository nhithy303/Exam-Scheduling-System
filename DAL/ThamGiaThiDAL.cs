using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ThamGiaThiDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private ThamGiaThi GetThamGiaThiFromDataRow(DataRow row)
        {
            ThamGiaThi tgt = new ThamGiaThi();
            tgt.MSSV = row["MSSV"].ToString();
            tgt.MaMon = row["MaMon"].ToString();
            tgt.MaPhong = row["MaPhong"].ToString();
            return tgt;
        }
        public ThamGiaThi[] GetList(string condition)
        {
            ThamGiaThi[] list = null;
            DataTable table = null;
            int n = 0;
            string query = "select * from ThamGiaThi";
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
            list = new ThamGiaThi[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetThamGiaThiFromDataRow(table.Rows[i]);
            }
            return list;
        }
        public void Insert(ThamGiaThi tgt)
        {
            string query = String.Format("insert into ThamGiaThi values ('{0}','{1}','{2}')",
                tgt.MSSV, tgt.MaMon, tgt.MaPhong);
            da.ExecuteNonQuery(query);
        }
        public void Delete(string condition)
        {
            string query = "delete from ThamGiaThi";
            if (condition != String.Empty)
            {
                query += " where " + condition;
            }
            da.ExecuteNonQuery(query);
        }
        public void Update(ThamGiaThi tgt)
        {
            string query = String.Format("update ThamGiaThi set MaPhong = '{0}' where MSSV = '{1}' and MaMon = '{2}'",
                tgt.MaPhong, tgt.MSSV, tgt.MaMon);
            da.ExecuteNonQuery(query);
        }
        public bool IsConflicting(string monthi1, string monthi2)
        {
            string query = String.Format("select MSSV, count(*) from ThamGiaThi where MaMon = '{0}' or MaMon = '{1}'" +
                " group by MSSV having count(*) = 2", monthi1, monthi2);
            DataTable table = da.ExecuteQuery(query);
            if (table.Rows.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
