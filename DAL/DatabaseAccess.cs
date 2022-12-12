using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DatabaseAccess
    {
        private static SqlConnection connection = null;
        public DatabaseAccess()
        {
            string server_name = "LAPTOP-NL39PTHM\\MEI";
            string conn_str = "Data Source=" + server_name + ";Initial Catalog=QL_LichThi;Integrated Security=True";
            connection = new SqlConnection(conn_str);
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
    }
}
