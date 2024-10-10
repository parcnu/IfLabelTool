using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IfLabelTool.Classes;

namespace IfLabelTool
{
    /* Class holding the Message length
     * 
     */
    class MsgLength
    {
        private string _lowbyte;
        private string _highbyte;
        public SystemLogFile SystemLogFileHandlerI { get; set; }
        public string LowByte {
            get
            {
                return _lowbyte;
            }
            set
            {
                _lowbyte = value;
                MsgLengthWordsInt = ToInteger(_lowbyte);
                MsgLengthBytesInt = MsgLengthWordsInt * Enums.MessageConsts.NumberOfBytesInWord;
            }

        }
        public string HighByte 
        { 
            get
            {
                return _highbyte;
            }
            set
            {
                _highbyte = value;
            }
        }

        public int MsgLengthWordsInt { get; protected set; }
        public int MsgLengthBytesInt { get; protected set; }

        /* Conversion from String to Hex
         * 
         */
        private string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }

        /* Conversion from Hex to Integer
         * 
         */
        public int ToInteger(string c)
        {
            
           
            if (c == "00") return 0;
            else if (c == "01") return 1;
            else if (c == "02") return 2;
            else if (c == "03") return 3;
            else if (c == "04") return 4;
            else if (c == "05") return 5;
            else if (c == "06") return 6;
            else if (c == "07") return 7;
            else if (c == "08") return 8;
            else if (c == "09") return 9;
            else if (c == "0A") return 10;
            else if (c == "0B") return 11;
            else if (c == "0C") return 12;
            else if (c == "0D") return 13;
            else if (c == "0E") return 14;
            else if (c == "0F") return 15;
            else return -1;
 
        }
       
        public int messageLength()
        {
            return MsgLengthWordsInt;
        }

        public MsgLength()
        {
            SystemLogFileHandlerI = null;
        }

        public MsgLength(SystemLogFile slf)
        {
            SystemLogFileHandlerI = slf;
        }
    }
}
