using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace PiSS.Helpers
{
    public class MySQL
    {
        private MySqlConnection dbConn;
        private static Logger _logger = new Logger("DB_connection");

        private String dbConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an instance of MySql connector by specifying connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public MySQL(String connectionString)
        {
            dbConnectionString = connectionString;
            dbConn = new MySqlConnection(connectionString);
        }

        private void Connect()
        {
            if (dbConn.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    dbConn.Close();
                    dbConn.Open();
                }
                catch (Exception ex)
                {
                    _logger.Log("DB connection failed", Logger.logLevel.FATAL, ex);
                }
            }
        }

        public DataTable Query(string query)
        {
            Connect();

            MySqlCommand command = new MySqlCommand(query, dbConn);

            MySqlDataReader result = command.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Load(result);

            result.Close();
            result.Dispose();

            return dt;
        }

        public DataRow ScalarQuery(string query)
        {
            Connect();

            MySqlCommand command = new MySqlCommand(query, dbConn);

            MySqlDataReader result = command.ExecuteReader();
            DataTable dt = new DataTable();

            dt.Load(result);

            result.Close();
            result.Dispose();

            return dt.Rows[0];
        }

        public void ExecuteQuery(string query)
        {
            Connect();

            new MySqlCommand(query, dbConn).ExecuteNonQuery();
        }
    }
}
