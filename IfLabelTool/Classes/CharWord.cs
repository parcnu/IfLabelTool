using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IfLabelTool.Classes;

namespace IfLabelTool
{
    class CharWord
    {
        /* Class to handle Bytes reading from the message.
         * Input is string which contains full message received from radar.
         * Reads two characters from the message and constucts word out of those.
         * 
         * Has internal class for extracting the word out of string. 
         * 
         */
        internal class Container
        {
            /* Container class for holding characters 
             * 
             */
            public char LowByte { get; set; }
            public char HighByte { get; set;  }
        }

        private Container cw;

        protected string _byte;

        public string Byte
        {
            get
            {
                return _byte;
            }
            protected set
            {
                _byte = value;
            }
        }

        /* Method to extract two character word (string) from message string
         * input message and index from which two characters are read.
         * 
         */
        public void ReadCharWord(String ms, ref int index)
        {
            int msLen = ms.Length;
            if (index < msLen)
            {
                cw.HighByte = ms[index];
            }
            if (index + 1 < msLen)
            {
                index++;
                cw.LowByte = ms[index];
                
            }
            _byte = cw.HighByte.ToString() + cw.LowByte.ToString();
        }
        
        /* Constructor for class.
         * Instantiates internal class.
         * 
         */
        public CharWord()
        {
            cw = new Container();
        }
    }
}
