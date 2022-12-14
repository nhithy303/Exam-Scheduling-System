using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ExcelFileDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        public bool Import(string filepath, string tablename, bool overwrite)
        {
            try
            {
                string s = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=Excel 12.0";
                OleDbConnection olecon = new OleDbConnection(s);
                olecon.Open();
                string query = "select * from [sheet1$]";
                OleDbDataAdapter oleda = new OleDbDataAdapter(query, olecon);
                DataTable table = new DataTable(tablename);
                oleda.Fill(table);
                olecon.Close();
                if (overwrite)
                {
                    da.ExecuteNonQuery("delete from " + tablename);
                }
                return da.ExecuteBulkCopy(table);
            }
            catch
            {
                return false;
            }
        }
    }
}
