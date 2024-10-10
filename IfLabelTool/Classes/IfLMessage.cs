using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IfLabelTool.Classes;
namespace IfLabelTool
{
    class IfLMessage
    {
        private StartByte StartBI;
        private MsgLength MessageLengthBI;
        private CommandByte CommandBI;
        private AddressCode AddressBI;
        private Data DataBI;
        private CRC16 Crc16BI;
        private SystemLogFile SystemLogFileHandI;
        public string MessageString { get; set; }
        public string ConstructedMessage { get; protected set; }

        public StartByte StartByteB
        {
            get
            {
                return StartBI;
            }
            protected set
            {
                StartBI = value;
            }
        }
        public MsgLength MsgLengthB
        {
            get
            {
                return MessageLengthBI;
            }
            protected set
            {
                MessageLengthBI = value;
            }
        }
        public CommandByte CommandB 
        { 
            get
            {
                return CommandBI;
            }
            protected set
            {
                CommandBI = value;
            }
        }
        public AddressCode AddressB 
        { 
            get
            {
                return AddressBI;
            }
            protected set 
            {
                AddressBI = value;
            } 
        }
        public Data DataB 
        {
            get
            {
                return DataBI;
            }
            protected set
            {
                DataBI = value;
            } 
        }
        public CRC16 Crc16B 
        { 
            get
            {
                return Crc16BI;
            }
            protected set
            {
                Crc16BI = value;
            } 
        }

        public SystemLogFile SystemLogFileHandler
        {
            get
            {
                return SystemLogFileHandI;
            }
            set
            {
                SystemLogFileHandI = value;
            }
        }

        public IfLMessage(SystemLogFile slf)
        {
            SystemLogFileHandI = slf;
            StartBI = new StartByte();
            MessageLengthBI = new MsgLength(SystemLogFileHandI);
            CommandBI = new CommandByte(SystemLogFileHandI);
            AddressBI = new AddressCode(SystemLogFileHandI);
            DataBI = new Data(SystemLogFileHandI);
            Crc16BI = new CRC16(SystemLogFileHandI);
           
        }

        public string ConstructMsg()
        {
            ConstructedMessage = StartByteB.StartB + MsgLengthB.LowByte + MsgLengthB.HighByte + CommandB.CommandChar + AddressB.AddrCmdOne + AddressB.AddrCmdTwo + DataB.DataBytes; //Crc16B.LowByte + Crc16B.HighByte;
            Crc16BI.ComputeCrc(Encoding.ASCII.GetBytes(ConstructedMessage));
            ConstructedMessage = ConstructedMessage + Crc16BI.HighByte + Crc16BI.LowByte;

            return ConstructedMessage;
        }

    }
}
