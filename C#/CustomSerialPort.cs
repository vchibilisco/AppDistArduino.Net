using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public class CustomSerialPort
    {
        public SerialPort SerialPort { get; set; }

        public CustomSerialPort(String portName, int baudRate)
        {
            SerialPort = new SerialPort(portName, baudRate);
            SerialPort.Open();
        }

        public void evaluarKey(char sKey)
        {
            sKey = Convert.ToChar(sKey.ToString().ToUpper());
            switch (sKey)
            {
                case (char)Keys.W:
                    enviarDato("w");
                    sleep();
                    break;
                case (char)Keys.A:
                    enviarDato("a");
                    sleep();
                    break;
                case (char)Keys.S:
                    enviarDato("s");
                    sleep();
                    break;
                case (char)Keys.D:
                    enviarDato("d");
                    sleep();
                    break;
                default:
                    enviarDato("s");
                    sleep();
                    break;
            }
        }

        public void sleep()
        {
            Thread.Sleep(200);
        }

        private void enviarDato(string c)
        {
            if (SerialPort.IsOpen)
            {

                try
                {
                    SerialPort.Write(c);
                }
                catch
                {
                    MessageBox.Show("There was an error. Please make sure that the correct port was selected, and the device, plugged in.");
                }
            }
        }
    }
}
