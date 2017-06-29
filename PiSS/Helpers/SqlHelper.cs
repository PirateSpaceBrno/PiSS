using static PiSS.main;
using System;

namespace PiSS.Helpers
{
    public class SqlHelper
    {
        private static MySQL _db;

        public SqlHelper(string connectionString)
        {
            _db = new MySQL(connectionString);
        }

        /// <summary>
        /// Returns current state of office stored in DB
        /// </summary>
        public static OfficeState IsOfficeLocked
        {
            get
            {
                OfficeState currentState;

                Enum.TryParse(_db.ScalarQuery("SELECT Value FROM Common WHERE Title = 'OfficeLocked'")[0].ToString(), out currentState);

                return currentState;
            }
        }

        public static void NotifyOfficeBeingRobbed()
        {
            // Set Office state to 'robbed'


            // Insert into event log
        }
    }
}
