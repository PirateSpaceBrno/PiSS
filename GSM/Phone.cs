using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;


namespace GSM
{
    public class Phone
    {
        private SerialPort phoneSerialPort;

        public Phone()
        {
            phoneSerialPort = new SerialPort("/dev/ttyAMA0", 115200);
        }

        /// <summary>
        /// Returns modem info when connection is successful
        /// </summary>
        /// <returns>Modem info</returns>
        public string Test()
        {
            if (phoneSerialPort != null)
                if (phoneSerialPort.IsOpen)
                {
                    phoneSerialPort.Close();
                }

            phoneSerialPort.Open();

            phoneSerialPort.ReadTimeout = 1000;
            SendData("ATI3\r");

            return ReadData();
        }


        /// <summary>
        /// Reads output of Phone serial line
        /// </summary>
        /// <returns></returns>
        public string ReadData()
        {
            byte tmpByte;
            string rxString = "";

            tmpByte = (byte)phoneSerialPort.ReadByte();

            while (tmpByte != 255)
            {
                rxString += ((char)tmpByte);
                tmpByte = (byte)phoneSerialPort.ReadByte();
            }

            return rxString;
        }

        /// <summary>
        /// Sends data to Phone serial line.
        /// </summary>
        /// <param name="Data">e.g. AT command</param>
        public void SendData(string Data)
        {
            phoneSerialPort.Write(Data);
        }

        public void SendSMS()
        {
            
        }
    }
}
