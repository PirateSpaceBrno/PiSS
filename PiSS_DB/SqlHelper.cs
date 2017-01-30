using MySql.Data.MySqlClient;
using System;

namespace PiSS.Helpers
{
    public class SqlHelper
    {
        private MySqlConnection dbConn;
        private static Logger _logger = new Logger();

        private String dbConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an instance of MySql connector by specifying connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlHelper(String connectionString)
        {
            dbConnectionString = connectionString;
            dbConn = new MySqlConnection(connectionString);
        }

        private void Connect()
        {
            try
            {
                dbConn.Open();
            }
            catch (Exception ex)
            {
                _logger.Log("DB connection failed", Logger.logLevel.FATAL, ex);
            }
        }
    }
}
