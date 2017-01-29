using System.Linq;
using PinType = PiSS_Sensors.Constants.PinType;

namespace PiSS_Sensors
{
    public class Pin
    {
        private int _physicalPinNumber;

        /// <summary>
        /// Creates an RaspberryPi Pin by specifying physical pin number.
        /// </summary>
        /// <param name="physicalPinNumber"></param>
        public Pin(int physicalPinNumber)
        {
            _physicalPinNumber = physicalPinNumber;
        }

        /// <summary>
        /// Returns physical pin number
        /// </summary>
        public int PhysicalPinNumber
        {
            get
            {
                return _physicalPinNumber;
            }
        }

        /// <summary>
        /// Returns WiringPi pin number from Pins table
        /// </summary>
        public int WiringPiPinNumber
        {
            get
            {
                return Constants.PinsTable.AsEnumerable().First(tuple => tuple.Item1 == _physicalPinNumber).Item2;
            }
        }

        /// <summary>
        /// Returns pin type read from Pins table
        /// </summary>
        public PinType PinType
        {
            get
            {
                return Constants.PinsTable.AsEnumerable().First(tuple => tuple.Item1 == _physicalPinNumber).Item3;
            }
        }
    }
}
