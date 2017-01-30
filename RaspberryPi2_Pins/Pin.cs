using System.Linq;
using PinType = RaspberryPi2_Pins.PinTable.PinType;

namespace RaspberryPi2_Pins
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
        /// Returns WiringPi pin number
        /// </summary>
        public int WiringPiPinNumber
        {
            get
            {
                var wiringPiPinNumber = PinTable.PinsTable.AsEnumerable().FirstOrDefault(tuple => tuple.Item1 == _physicalPinNumber);

                return (wiringPiPinNumber == null) ? -1 : wiringPiPinNumber.Item2;
            }
        }

        /// <summary>
        /// Returns pin type
        /// </summary>
        public PinType PinType
        {
            get
            {
                var wiringPiPinNumber = PinTable.PinsTable.AsEnumerable().FirstOrDefault(tuple => tuple.Item1 == _physicalPinNumber);

                return (wiringPiPinNumber == null) ? PinType.Undefined : wiringPiPinNumber.Item3;
            }
        }
    }
}
