using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DAL
{
    public class DatabaseAccess
    {
        private static SqlConnection connection = null;
        private static SqlBulkCopy copy = null;
        public DatabaseAccess()
        {
            string server_name = "LAPTOP-NL39PTHM\\MEI";
            string conn_str = "Data Source=" + server_name + ";Initial Catalog=QL_LichThi;Integrated Security=True";
            connection = new SqlConnection(conn_str);
            copy = new SqlBulkCopy(conn_str);
        }

        // Execute select query
        public DataTable ExecuteQuery(string query)
        {
            DataTable table = new DataTable();
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.Fill(table);
            connection.Close();
            return table;
        }

        // Execute insert, update, delete query
        public void ExecuteNonQuery(string query)
        {
            connection.Open();
            new SqlCommand(query, connection).ExecuteNonQuery();
            connection.Close();
        }

        // Execute bulk copy from Excel File into database
        public bool ExecuteBulkCopy(DataTable table)
        {
            try
            {
                connection.Open();
                copy.DestinationTableName = table.TableName;
                copy.WriteToServer(table);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
