using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace QA.Common.Utilities
{
    public class SQLUtilities
    {
        public static string GetConnectionString(string environment = null)
        {
            string env = environment != null ? environment : ConfigurationManager.AppSettings["targetEnvironment"];
            switch (env)
            {
                case "rc":
                    return "Data Source=ro.rc.sql.fraudguard.firstam.cloud;Initial Catalog={0};User ID=;Password=;TrustServerCertificate=true";
                default:
                    return null;
            }
        }

        public static string ExecuteSQLQuery(string sqlQuery, string columnName, string environment = null)
        {
            string connectionString = GetConnectionString(environment);
            string result = null;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader[columnName].ToString();
                }
                reader.Close();
                connection.Close();
            }
            return result;
        }

        public static IList<string> ExecuteSQLQueryList(string sqlQuery, string columnName, string environment = null)
        {
            string connectionString = GetConnectionString(environment);
            IList<string> result = null;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader[columnName].ToString());
                }
                reader.Close();
                connection.Close();
            }
            return result;
        }

        public static IDictionary<string, string> ExecuteSQLQuery(string sqlQuery, IDictionary<string,string> input, string environment = null)
        {
            string connectionString = GetConnectionString(environment);
            IDictionary<string,string> result = null;
             

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    foreach(string key in input.Keys)
                    {
                        input[key] = reader[key].ToString();
                    }
                  
                }
                reader.Close();
                connection.Close();
            }
            return result;
        }
  
        public static DataTable ExecuteSQLQueryDataTable(string sqlQuery, string environment = null)
        {
            string connectionString = GetConnectionString(environment);
            DataTable table = new DataTable();
            DataRow workRow = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                int columnCount = reader.FieldCount;
                for(int col=0; col<reader.FieldCount; col++)
                {
                    table.Columns.Add(reader.GetName(col).ToString());
                }
                while(reader.Read())
                {
                    workRow = table.NewRow();
                    for(int col = 0; col <reader.FieldCount; col++)
                    {
                        workRow[col] = reader.GetValue(col).ToString();
                    }
                }
                return table;
            }
        }
    }
}
