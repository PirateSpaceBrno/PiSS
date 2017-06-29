using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PiSS.Helpers
{
    public class SensorsHelper
    {
        /// <summary>
        /// Returns list of sensors defined by files stored in sensors directory. Files are named in format sensorId.connectedPhysicalPinNumber
        /// </summary>
        /// <returns></returns>
        public static List<Sensor> LoadAllSensors()
        {
            IEnumerable<string> requiredSensors = Directory.EnumerateFiles(Environment.CurrentDirectory + "/sensors/");

            List<Sensor> allSensors = new List<Sensor>();

            foreach (string sensorFile in requiredSensors)
            {
                string sensor = sensorFile.Replace(Environment.CurrentDirectory + "/sensors/", "");
                string id = sensor.Substring(0, sensor.LastIndexOf("."));
                string pin = sensor.Replace($"{id}.", "");

                allSensors.Add(Sensor.Create(Convert.ToInt32(id), Convert.ToInt32(pin)));
            }

            return allSensors;
        }

        /// <summary>
        /// Scan all sensors. If some sensor is triggered, returns triggered else standby.
        /// </summary>
        /// <param name="sensors"></param>
        /// <returns></returns>
        public static Sensor.SensorState ScanSensors(List<Sensor> sensors)
        {
            List<bool> checker = new List<bool>();

            foreach (Sensor sensor in sensors)
            {
                checker.Add((sensor.State == Sensor.SensorState.triggered) ? true : false);
            }

            // If at least one sensor was triggered, return triggered
            return (checker.AsEnumerable().Where(x => x == true).Count() >= 1) ? Sensor.SensorState.triggered : Sensor.SensorState.standby;
        }
    }
}
