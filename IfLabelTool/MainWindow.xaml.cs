using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;              //requires nuget package installation in .NET 5.0
using System.Management;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using IfLabelTool.Classes;
using System.IO;
using System.Reflection;
using Path = System.IO.Path;

namespace IfLabelTool
{
    /* IfLabelTool MainWondow class.
     * Handles windowing and is run in own Thread.
     * Receives Events from ReadUSB class ReceivedDataEvent. When received payload is coverted from bytes to String and displayed in TextBox in UI.
     * 
     * ToDo: Send configuration messages to Radar.
     * ToDo: Unit Tests.
     * ToDo: Stucs sometimes on exit. Check UsbPort and file writing.
     * ToDo: Check AddressCode evaluation - some Error is shown on UI.
     * ToDo: Add LogWriting on AddressCode evaluation.
     */
    public partial class MainWindow : Window
    {

        SerialPort serialPort = new SerialPort();
        ReadUSB rUSB;
        Thread bgWorker;
        LogToFile lg = new LogToFile();
        IfLMessage message = null;
        SystemLogFile systemLogFile = new SystemLogFile();
        FileStream fs = null;
        DateTime datetime = new DateTime();

        /* MainWindow settings.
         * Starting the ReadUSB in own thread
         * 
         */
        public MainWindow()
        {
            InitializeComponent();
            bgWorker = new Thread(() => rUSB = new ReadUSB());
            bgWorker.Start();
            datetime = DateTime.Now;
            systemLogFile.LogFileName = datetime.ToString(Enums.IfLabelCommonLabels.DateFormat) + Enums.IfLabelCommonLabels.SystemLogFileName;
            systemLogFile.LogFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogViewScroll.ScrollToBottom();
            try
            {
                //using (fs = File.Create(systemLogFile.LogFilePath + @"\" + systemLogFile.LogFileName))
                //{
                //    systemLogFile.FileStream = fs;
                    systemLogFile.CreateSystemLogFile(DateTime.Now.ToString() + "Log Started");
                    message = new IfLMessage(systemLogFile);

                //}

            }
            catch (Exception ex)
            {

            }
        }

        /* Convert Bytes to Hex values in String
         * 
         */
        public static string ByteArrayToString(byte[] byt)
        {
            StringBuilder hex = new StringBuilder(byt.Length * 2);
            foreach (byte b in byt)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        
        /*Deserialize received radar data
         * 1 byte 0x55 - start byte
         * 2 byte Data length (Big endian) - remember to swap bytes.
         * 1 byte Function code
         * 1 byte Address code 1
         * 1 byte Address code 2
         * n byte (calculte from length) Data
         * 1 byte - crc16_l 
         * 1 byte - crc16_h
         * 
         */
        private void DeSerializeRadarMessage(string msg, string now)
        {

            IfLMessage tempmessage = new IfLMessage(systemLogFile);
            tempmessage.MessageString = msg;                    //get message as part of object.

            
            StringBuilder str = new StringBuilder();
      
            for (int i = 0; i < msg.Length; i++)
            {
                
                CharWord cw = new CharWord();
               
                
                cw.ReadCharWord(msg, ref i);
                if (!tempmessage.StartByteB.FindStartByte(cw.Byte))
                {


                //if (i == 1 && cw.Byte == Enums.MessageConsts.StartByte) tempmessage.StartByteB.StartByteFound = true; // If first byte was start byte

                    if (i == 3)                               // MessageLen low byte
                    {
                        tempmessage.MsgLengthB.LowByte = cw.Byte;
                        tempmessage.MsgLengthB.messageLength();
                        if (tempmessage.MessageString.Length > 26 || tempmessage.MessageString.Length < 18) //Disgard too small and too long messages.
                        {
                            tempmessage = null;   //flush buffer
                            msg = null;
                            break;
                        }
                        else
                        {
                            Dispatcher.Invoke(() => LogWindowTextBlock.Text += (now + msg + Environment.NewLine));
                        }
                    }

                    else if (i == 5)
                    {
                        tempmessage.MsgLengthB.HighByte = cw.Byte;      // Message length High byte
                    }

                    else if (i == 7)                                //CommandByte
                    {
                        tempmessage.CommandB.CommandChar = cw.Byte;
                        Dispatcher.Invoke(() => RadarCommandTextBlock.Text = tempmessage.CommandB.CommandChar.ToString());
                    }

                    else if (i == 9)                                //Address Command one
                    {
                        tempmessage.AddressB.CommandByte = tempmessage.CommandB.CommandChar;
                        tempmessage.AddressB.AddrCmdOne = cw.Byte;
                        Dispatcher.Invoke(() =>
                        {
                            if (tempmessage.AddressB.AddrCmdOneString != null)
                            {
                                RadarAddrOneTextBlock.Text = tempmessage.AddressB.AddrCmdOneString.ToString();
                            }
                            else
                            {
                                tempmessage.SystemLogFileHandler.WriteToFile(Enums.ErrorCodes.ErrorLabel + " AddrCmdOneString is null" + " Cmd Char " + tempmessage.CommandB.CommandChar + cw.Byte);
                            }
                        });
                    }

                    else if (i == 11)                               //Address Command one
                    {
                        tempmessage.AddressB.AddrCmdTwo = cw.Byte;
                        Dispatcher.Invoke(() =>
                        {
                            if (tempmessage.AddressB.AddrCmdTwoString != null) 
                            {
                                RadarAddrTwoTextBlock.Text = tempmessage.AddressB.AddrCmdTwoString.ToString();
                            }
                            else
                            {
                                tempmessage.SystemLogFileHandler.WriteToFile(Enums.ErrorCodes.ErrorLabel + " AddrCmdOneString is null" + " Cmd Char " + tempmessage.CommandB.CommandChar + cw.Byte);
                            }
                        });
                    }

                    else if (i == 13)                               //Message Data Bytes. Length is caliculated from mesage len - known bytes.
                    {
                        tempmessage.DataB.DataLengthBytes = (tempmessage.MsgLengthB.MsgLengthBytesInt) - Enums.MessageConsts.DataReservedBytes;
                        int x = i;
                        str.Append(cw.Byte);
                        i++;
                        for (; i < x + tempmessage.DataB.DataLengthBytes - Enums.MessageConsts.NumberOfBytesInWord; i++)     //Get message data bytes
                        {
                            cw.ReadCharWord(msg, ref i);
                            str.Append(cw.Byte);
                        }
                        tempmessage.DataB.DataBytes = str.ToString();

                        // get crc data out of the message.
                        cw.ReadCharWord(msg, ref i);
                        i++;
                        tempmessage.Crc16B.LowByte = cw.Byte;           //CRC16 Low Byte, Lower Byte

                        cw.ReadCharWord(msg, ref i);                    //CRC16 Low Byte, High Byte
                        i++;

                        tempmessage.Crc16B.LowByte = tempmessage.Crc16B.LowByte + cw.Byte;

                        cw.ReadCharWord(msg, ref i);                    //CRC16 High Byte, Low Byte
                        i++;

                        tempmessage.Crc16B.HighByte = cw.Byte;
                        cw.ReadCharWord(msg, ref i);                    //CEC16 High Byte, High Byte

                        tempmessage.Crc16B.HighByte = tempmessage.Crc16B.HighByte + cw.Byte;
                        tempmessage.ConstructMsg();
                        if (tempmessage.MessageString != tempmessage.ConstructedMessage)    // Just for debuggng purposes that message is constructec correctly.
                        {
                            tempmessage.SystemLogFileHandler.WriteToFile(Enums.ErrorCodes.ErrorLabel + "Original ad constructed messages does no match " + "Orig: " + tempmessage.MessageString + Environment.NewLine + "Con: " + tempmessage.ConstructedMessage);
                            //MessageBox.Show("Orig: " + tempmessage.MessageString + Environment.NewLine + "Con: " + tempmessage.ConstructedMessage);
                        }
                    }
                }
                
            }
            message = tempmessage;
            
        }

        /* Receiving event from ReadUSB when new message is read from Serial buffer.
         * Writes received data to TextBlock.
         */
        public void ReceiveDataEvent(byte[] t, string now)
        {

            string m = ByteArrayToString(t).ToUpper();
            systemLogFile.WriteToFile(now + m) ;
            DeSerializeRadarMessage(m, now);

        }

        public void ExceptionEvent(string ex)
        {
            MessageBox.Show(ex);
            this.Close();

            // Do common close for all events.
            
        }
       
        /* OpenConnection button clicked.
         * Binds events from ReadUSB class.
         * Runs serialport settings
         * Opens the serialport.
         */
        private void OpenConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (rUSB.IsPortOpen())  //If port is open run close acivites else this is opening the uart stream
            {
                CloseApp();
            }
            else
            {
                ReadUSB.USBDataEvent += ReceiveDataEvent;
                ReadUSB.ExceptionEvent += ExceptionEvent;
                Dispatcher.Invoke(() => rUSB.SetupPort());
                Dispatcher.Invoke(() => rUSB.PortOpen());
                PortControlButton.Content = "Close Connection";
            }
        }

        private void LogToFileButton_Click(object sender, RoutedEventArgs e)
        {
            lg.OpenFile();
            //tänne a) file open, b)filen asettaminen logitukseen, c) aloittaa logitus
        }

        /* Cleaning the environment when port is closed.
         */
        private void CloseApp()
        {
            try
            {
                ReadUSB.USBDataEvent -= ReceiveDataEvent;
                ReadUSB.ExceptionEvent -= ExceptionEvent;
                if (rUSB.IsPortOpen()) Dispatcher.Invoke(() => rUSB.PortClose());
                PortControlButton.Content = "Open Connection";
            }
            catch (Exception ex)
            {
                message.SystemLogFileHandler.WriteToFile(Enums.ErrorCodes.ErrorLabel + "Error in CloseApp " + ex.Message);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            //Close Serial port
            if (rUSB.IsPortOpen()) rUSB.PortClose();
            //System log file closing.
            systemLogFile.CloseSystemLogFile();
            // Shutdown the application.
            Application.Current.Shutdown();
        }
    }
    
}
