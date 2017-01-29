using System;
using System.Collections.Generic;
using System.Linq;

namespace PiSS_Sensors
{
    public class Constants
    {
        // List of Pin pairs - first is physical pin number, seconds is WiringPi pin number
        public static List<Tuple<int, int, PinType>> PinsTable = new List<Tuple<int, int, PinType>>
        {
            Tuple.Create(3, 8, PinType.SDA),
            Tuple.Create(5, 9, PinType.SCL),
            Tuple.Create(7, 7, PinType.GPCLK0),
            Tuple.Create(8, 15, PinType.TXD),
            Tuple.Create(10, 16, PinType.RXD),
            Tuple.Create(11, 0, PinType.GPIO),
            Tuple.Create(12, 1, PinType.PWM0),
            Tuple.Create(13, 2, PinType.GPIO),
            Tuple.Create(15, 3, PinType.GPIO),
            Tuple.Create(16, 4, PinType.GPIO), 
            Tuple.Create(18, 5, PinType.GPIO),
            Tuple.Create(19, 12, PinType.MOSI),
            Tuple.Create(21, 13, PinType.MISO),
            Tuple.Create(22, 6, PinType.GPIO),
            Tuple.Create(23, 14, PinType.SCLK),
            Tuple.Create(24, 10, PinType.CE0),
            Tuple.Create(26, 11, PinType.CE1),
            Tuple.Create(27, 30, PinType.ID_SD),
            Tuple.Create(28, 31, PinType.ID_SC),
            Tuple.Create(29, 21, PinType.GPIO),
            Tuple.Create(31, 22, PinType.GPIO),
            Tuple.Create(32, 26, PinType.PWM0),
            Tuple.Create(33, 23, PinType.PWM1),
            Tuple.Create(35, 24, PinType.MISO),
            Tuple.Create(36, 27, PinType.GPIO),
            Tuple.Create(37, 25, PinType.GPIO),
            Tuple.Create(38, 28, PinType.MOSI),
            Tuple.Create(40, 29, PinType.SCLK),
        };

        public enum PinType
        {
            Ground,
            Power,
            SDA,
            SCL,
            TXD,
            RXD,
            GPCLK0,
            PWM0,
            MOSI,
            MISO,
            CE0,
            CE1,
            SCLK,
            ID_SC,
            ID_SD,
            PWM1,
            GPIO
        }

        /// <summary>
        /// Converts between physical and WiringPi Pin numbers
        /// </summary>
        /// <param name="pinNumber">Specify the Pin number</param>
        /// <param name="isPhysicalPinNumber">True if specified pin number is physical, false if pin number is WiringPi.</param>
        /// <returns></returns>
        public int ConvertPinNumber(int pinNumber, bool isPhysicalPinNumber = true)
        {
            if (isPhysicalPinNumber)
            {
                return PinsTable.AsEnumerable().First(tuple => tuple.Item1 == pinNumber).Item2;
            }
            else
            {
                return PinsTable.AsEnumerable().First(tuple => tuple.Item2 == pinNumber).Item1;
            }
        }
    }
}
