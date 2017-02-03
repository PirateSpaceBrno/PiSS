using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using PiSS.Helpers;

namespace PiSS
{
    // PiSS - Pirate Space Security
    public class main
    {
        // Initialize components
        private static SqlHelper db = new SqlHelper(File.ReadAllText(Environment.CurrentDirectory + "/dbConnectionString.txt"));
        private static List<Sensor> sensors = SensorsHelper.LoadAllSensors();
        private static Logger _logger = new Logger("PiSS");
        //private static Phone phone = new Phone();

        static void Main(string[] args)
        {
            // Infinite loop checking state of sensors
            while (true)
            {
                OfficeState currentOfficeState = SqlHelper.IsOfficeLocked;

                Console.WriteLine($"Office state is '{currentOfficeState.ToString()}'");
                _logger.Log($"Office state is '{currentOfficeState.ToString()}'", Logger.logLevel.DEBUG);

                if (currentOfficeState == OfficeState.locked || currentOfficeState == OfficeState.robbed)
                {
                    // If some sensor was triggered, check sensors for possible motion for next 10 seconds
                    if (SensorsHelper.ScanSensors(sensors) == Sensor.SensorState.triggered)
                    {
                        Console.WriteLine("Starting routine before alarm is triggered");
                        _logger.Log("Starting routine before alarm is triggered", Logger.logLevel.DEBUG);

                        // Wait 100 millisecond before sensors state check
                        System.Threading.Thread.Sleep(100);

                        DoAlarmRoutine(DateTime.Now);
                    }

                    // Wait 1 second before next office and sensors state check
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    // Wait 5 second before next office and sensors state check
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

        private static void DoAlarmRoutine(DateTime checkStarted)
        {
            // Check sensors for 10 seconds
            List<bool> checker = new List<bool>();

            while (checkStarted.AddSeconds(10) <= DateTime.Now)
            {
                checker.Add((SensorsHelper.ScanSensors(sensors) == Sensor.SensorState.triggered) ? true : false);
                
                // Wait 100 millisecond before sensors state check
                System.Threading.Thread.Sleep(100);
            }

            // If motion was detected, trigger alarm
            if (checker.AsEnumerable().Where(x => x == true).Count() >= (checker.Count / 2))
            {
                // Log this event to the database
                SqlHelper.NotifyOfficeBeingRobbed();

                // Log this event to the file
                _logger.Log($"Alarm was triggered!", Logger.logLevel.FATAL);

                // Notify by SMS
                //phone.Test();

                // If alarm triggered, hold until alarm deactivated
                while (SqlHelper.IsOfficeLocked == OfficeState.robbed)
                {
                    // If alarm is not deactivated within 5 minutes, notify by phone again
                    if (DateTime.Now >= checkStarted.AddMinutes(5))
                    {
                        //phone

                        // Restart time counting
                        DoAlarmRoutine(DateTime.Now);

                        // Jump out
                        return;
                    }
                }
            }
        }

        public enum OfficeState
        {
            unlocked,
            locked,
            robbed
        }
    }
}
