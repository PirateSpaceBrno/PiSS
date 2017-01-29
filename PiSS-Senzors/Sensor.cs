using System;
using WiringPi;
using Thread = System.Threading.Thread;

namespace PiSS_Sensors
{
    public class Sensor
    {
        private int _sensorId;
        private Pin _sensorPin;

        /// <summary>
        /// Creates new sensor instance
        /// </summary>
        /// <param name="sensorId">Sensor ID just for marking.</param>
        /// <param name="sensorPhysicalPinNumber">Number of GPIO pin where the sensor is connected.</param>
        public Sensor(int sensorId, int sensorPhysicalPinNumber)
        {
            _sensorId = sensorId;
            _sensorPin = new Pin(sensorPhysicalPinNumber);

            // Setup the WiringPi component
            if (Init.WiringPiSetup() == -1)
            {
                Thread.CurrentThread.Abort();
            }

            // Set specified GPIO pin as INPUT
            GPIO.pinMode(Pin.WiringPiPinNumber, (int)GPIO.GPIOpinmode.Input);
        }

        /// <summary>
        /// Returns Sensor ID
        /// </summary>
        public int Id
        {
            get
            {
                return _sensorId;
            }
        }

        /// <summary>
        /// Returns GPIO Pin where the sensor is connected.
        /// </summary>
        public Pin Pin
        {
            get
            {
                return _sensorPin;
            }
        }

        /// <summary>
        /// Returns actual sensor state read from GPIO pin.
        /// </summary>
        public SensorState State
        {
            get
            {
                SensorState actualState;

                Enum.TryParse(PinResponse.ToString(), out actualState);

                return actualState;
            }
        }

        /// <summary>
        /// Returns actual pin response (~3.3V returns logical 1, ~0.8V returns logical 0)
        /// </summary>
        public int PinResponse
        {
            get
            {
                return GPIO.digitalRead(Pin.WiringPiPinNumber);
            }
        }

        /// <summary>
        /// SensorState: 'triggered' means motion detected, 'standby' means operating/active sensor.
        /// </summary>
        public enum SensorState
        {
            triggered,
            standby
        }
    }
}
