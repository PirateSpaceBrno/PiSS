using System;
using System.Collections.Generic;

namespace PiSS
{
    class main
    {
        public const bool IsDebug = true;
        public static Logger _logger = new Logger(IsDebug);

        static void Main(string[] args)
        {
            //List<Sensor> sensors = new List<Sensor> { new Sensor(1, 37), new Sensor(2, 26), new Sensor(3, 25)};

            //while(true)
            //{
            //    foreach(Sensor sensor in sensors)
            //    {
            //        var grabbedState = sensor.PinResponse;

            //        Console.WriteLine($"State of Sensor #{sensor.Id} on Pin '{sensor.Pin.PhysicalPinNumber}' is {grabbedState.ToString()}");

            //        System.Threading.Thread.Sleep(1000);
            //    }
            //}

            Phone phone = new Phone();

            phone.Test();
        }
    }
}
