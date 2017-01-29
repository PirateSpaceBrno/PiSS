using System.Linq;
using PinType = PiSS_Sensors.Constants.PinType;

namespace PiSS_Sensors
{
    public class Pin
    {
        private int _physicalPinNumber;

        public Pin(int physicalPinNumber)
        {
            _physicalPinNumber = physicalPinNumber;
        }

        public int PhysicalPinNumber
        {
            get
            {
                return _physicalPinNumber;
            }
        }

        public int WiringPiPinNumber
        {
            get
            {
                return Constants.PinsTable.AsEnumerable().First(tuple => tuple.Item1 == _physicalPinNumber).Item2;
            }
        }

        public PinType PinType
        {
            get
            {
                return Constants.PinsTable.AsEnumerable().First(tuple => tuple.Item1 == _physicalPinNumber).Item3;
            }
        }
    }
}
