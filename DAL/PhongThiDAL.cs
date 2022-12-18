using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PhongThiDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private PhongThi GetPhongThiFromDataRow(DataRow row)
        {
            PhongThi pt = new PhongThi();
            pt.MaPhong = row["MaPhong"].ToString();
            if (row["SucChua"].ToString() != String.Empty)
            {
                pt.SucChua = int.Parse(row["SucChua"].ToString());
            }
            return pt;
        }
        public PhongThi[] GetList()
        {
            PhongThi[] list = null;
            DataTable table = null;
            int n = 0;
            table = da.ExecuteQuery("select * from PhongThi");
            n = table.Rows.Count;
            if (n == 0)
            {
                return null;
            }
            list = new PhongThi[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetPhongThiFromDataRow(table.Rows[i]);
            }
            return list;
        }
        public bool Insert(PhongThi pt)
        {
            if (!CheckPrimaryKey(pt))
            {
                return false;
            }
            pt.SucChua = this.GetList()[0].SucChua;
            string query = String.Format("insert into PhongThi values ('{0}',{1})", pt.MaPhong, pt.SucChua);
            da.ExecuteNonQuery(query);
            return true;
        }
        public void Update(string succhua)
        {
            string query = String.Format("update PhongThi set SucChua = {0}", succhua);
            da.ExecuteNonQuery(query);
        }
        public bool Delete(PhongThi pt)
        {
            if (!CheckForeignKey(pt))
            {
                return false;
            }
            string query = String.Format("delete from PhongThi where MaPhong = '{0}'", pt.MaPhong);
            da.ExecuteNonQuery(query);
            return true;
        }
        private bool CheckForeignKey(PhongThi pt)
        {
            string query = String.Format("select * from PhanBoPhongThi where MaPhong = '{0}'", pt.MaPhong);
            DataTable table = null;
            table = da.ExecuteQuery(query);
            if (table.Rows.Count == 0)
            {
                return true;
            }
            return false;
        }
        private bool CheckPrimaryKey(PhongThi pt)
        {
            string query = String.Format("select * from PhongThi where MaPhong = '{0}'", pt.MaPhong);
            DataTable table = null;
            table = da.ExecuteQuery(query);
            if (table.Rows.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
