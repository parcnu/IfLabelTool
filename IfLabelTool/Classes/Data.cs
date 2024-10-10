using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IfLabelTool.Classes;

namespace IfLabelTool
{
    class Data
    {
        private object _cmdbyte { get; set; }
        private object _addrbytes { get; set; }
        private string _localAdrOne { get; set; }
        private string _localAdrTwo { get; set; }
        private string _localCmdByte { get; set; }
        private int _datalengthwords { get; set; }
        private int _datalengthbytes { get; set; }



        public SystemLogFile SystemLogFileHandlerI { get; set; }
        public int DataLengthBytes
        {
            get
            {
                return _datalengthwords;
            }
            set
            {
                _datalengthwords = value;
            }
        }

        public object CmdByte
        {
            get
            {
                return _cmdbyte;
            }

            set
            {
                _cmdbyte = value;
                 GetCommandByte();
                /*if (res == Enums.CommandTypes.ReadCommand) ;
                else if (res == Enums.CommandTypes.WriteCommand) ; //do someting;
                else if (res == Enums.CommandTypes.ControlCommand) ;
                else if (res == Enums.CommandTypes.ActiveReportCommand) ;
                else;*/ 
                
            }
        }
        public object AddrBytes 
        {
            get
            {
                return _addrbytes;
            }
            set
            {
                _addrbytes = value;
                GetAddressCodes();
            }
        }
        
        public string DataBytes { get; set; }
        public int DataLengthWords
        { 
            get
            {
                return _datalengthwords;
            }
            set
            {
                _datalengthwords = value;
                _datalengthbytes = _datalengthwords * Enums.MessageConsts.NumberOfBytesInWord;
            }
        }

        public void GetCommandByte()
        {
            _localCmdByte = (string)CmdByte.GetType().GetProperty(Enums.FunctionNames.CommandByte).GetValue(CmdByte);
           
        }

        public void GetAddressCodes()
        {
            _localAdrOne = (string)AddrBytes.GetType().GetProperty(Enums.FunctionNames.AddressCommandOne).GetValue(AddrBytes);
            _localAdrTwo = (string)AddrBytes.GetType().GetProperty(Enums.FunctionNames.AddressCommandTwo).GetValue(AddrBytes);
        }

        public Data()
        {
            SystemLogFileHandlerI = null;
        }

        public Data(SystemLogFile slf)
        {
            SystemLogFileHandlerI = slf;
        }

    }
}
