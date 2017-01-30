using RaspberryPi2_Pins;
using System;
using WiringPi;
using Thread = System.Threading.Thread;

namespace PiSS
{
    public class Sensor
    {
        private int _sensorId;
        private Pin _sensorPin;
        private static Logger _logger = new Logger();

        /// <summary>
        /// Creates new sensor instance
        /// </summary>
        /// <param name="sensorId">Sensor ID just for marking.</param>
        /// <param name="sensorPin">GPIO Pin</param>
        private Sensor(int sensorId, Pin sensorPin)
        {
            _sensorId = sensorId;
            _sensorPin = sensorPin;

            try
            {

                // Setup the WiringPi component
                if (Init.WiringPiSetup() == -1)
                {
                    Thread.CurrentThread.Abort();
                }
                if (SPI.wiringPiSPISetup(0, 20000000) == -1)
                {
                    Thread.CurrentThread.Abort();
                }

                // Set specified GPIO pin as INPUT
                GPIO.pinMode(Pin.WiringPiPinNumber, (int)GPIO.GPIOpinmode.Input);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error occured during sensor #{_sensorId} registration by process '{Thread.CurrentThread.Name}'", Logger.logLevel.FATAL, ex);
            }

            // Report sensor to the user
            Console.WriteLine($"Sensor #{_sensorId} registered.");
            _logger.Log($"Sensor #{_sensorId} registered by process '{Thread.CurrentThread.Name}'", Logger.logLevel.INFO);
        }

        /// <summary>
        /// Creates new sensor instance. Returns Null if selected physical pin is not GPIO.
        /// </summary>
        /// <param name="sensorId">Sensor ID just for marking.</param>
        /// <param name="sensorPhysicalPinNumber">Number of GPIO pin where the sensor is connected.</param>
        public static Sensor Create(int sensorId, int sensorPhysicalPinNumber)
        {
            if (PinTable.GetPinType(sensorPhysicalPinNumber) != PinTable.PinType.GPIO)
            {
                _logger.Log($"Process '{Thread.CurrentThread.Name}' tried to register sensor #{sensorId} on not GPIO pin.", Logger.logLevel.FAIL);
                return null;
            }
            return new Sensor(sensorId, new Pin(sensorPhysicalPinNumber));
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
        /// Returns actual pin response (~3.3V returns logical 1, ~0.8V returns logical 0 - MAXIMUM 0.5mA)
        /// </summary>
        public int PinResponse
        {
            get
            {
                int response = GPIO.digitalRead(Pin.WiringPiPinNumber);

                _logger.Log($"Sensor #{_sensorId} respond value '{response}' to the process '{Thread.CurrentThread.Name}'", Logger.logLevel.DEBUG);

                return response;
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
