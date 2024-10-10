using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;


namespace IfLabelTool
{
    
    /* Class to handle Serial Port reading.
     * This instance should be run in own Thread.
     * 
     * Receives events from the SerialPort SerialDataReceivedEventHandler which initiates SerilaPort buffer reading
     * 
     * ToDo: SerialPort selection which is now fixed to COM3.
     * ToDo: SerialPort write for saending configuration messages to IfLabel.
     * ToDo: unbinding the SerialDataReceivedEventHandler
     */
    class ReadUSB
    {
        //Event delegates for sending events out of the instance.
        public delegate void myDelegat(byte[] t, string now);
        public static event myDelegat USBDataEvent;
        public delegate void myException(string e);
        public static event myException ExceptionEvent;

        SerialPort sp = new SerialPort();

        /* SerialPort Setup 
         * Others may be hardcoded, but PortName should be dynacmic and read from the system.
         */
        public void SetupPort()
        {
            try
            {
                sp.PortName = "COM3";
                sp.BaudRate = 9600;
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.Encoding = Encoding.UTF8;
            }
            catch (Exception ex)
            {
                ExceptionEvent(ex.ToString());
            }
        }
        /* Opens the SerialPort for Reading and Writing. 
         * Binds the SerialDataReceivedEventHandler
         */
        public void PortOpen()
        {
            try
            {
                if (sp != null && sp.IsOpen)
                    sp.Close();
                sp.Open();

                sp.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            }
            catch (Exception ex)
            {
                ExceptionEvent(ex.ToString());
            }
        }

        /* Closeing the SerialPort and removing the binging of the Event.
         */
        public void PortClose()
        {
            sp.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);
            try
            {
                sp.Close();
                sp.Dispose();
            }
            catch (Exception ex)
            {
                // error
            }
            
        }

        /* Wrapper method for serialport status.
         * Returns the bool
         */
        public bool IsPortOpen()
        {
            return sp.IsOpen;
        }


        /* 
         * Event handler to read serial buffer.
         * When event is received, serial buffer will be read and new event is sent to MainWindow Thread.
         * ToDo: use BaseStream
         */
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string now = DateTime.Now.ToString("hh:mm:ss.fff") + " ";
            string str = "";
            byte[] bytes = null;
            

            Thread.Sleep(40);                                       //Let the buffer get fullfilled before reading it. 15ms is already too low time and will split the message.
            try
            {
                if (sp.IsOpen)
                {
                    var serialDevice = sender as SerialPort;
                    var buffer = new byte[serialDevice.BytesToRead];
                    bytes = new byte[buffer.Length];
                    serialDevice.Read(bytes, 0, buffer.Length);
                    USBDataEvent(bytes, now);
                    //Send event with read bytes.
                }
            }
            catch (Exception ex)
            {
                ExceptionEvent(ex.ToString());
            }

        }

    }
}
