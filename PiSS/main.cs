using System;
using System.Collections.Generic;

namespace PiSS
{
    class main
    {
        public static Logger _logger = new Logger();

        static void Main(string[] args)
        {
            List<Sensor> sensors = new List<Sensor> { Sensor.Create(1, 37) };

            while (true)
            {
                foreach (Sensor sensor in sensors)
                {
                    var grabbedState = sensor.PinResponse;

                    Console.WriteLine($"State of Sensor #{sensor.Id} on Pin '{sensor.Pin.PhysicalPinNumber}' is {grabbedState.ToString()}");

                    System.Threading.Thread.Sleep(1000);
                }
            }

            Phone phone = new Phone();

            phone.Test();
        }
    }
}
