using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfLabelTool
{
    /* StartByte Class
     * Holding Start Byte string and boolen if start byte has been found.
     * 
     */
    class StartByte
    {
        bool _startByteFound;
        public string StartB { get; set; }
        public bool StartByteFound 
        { 
            get
            {
                return _startByteFound;

            }
            set
            {
                _startByteFound = value;
                StartB = Enums.MessageConsts.StartByte;
            } 
        }
        public bool FindStartByte(String ms)
        {
            
            if (ms.Equals(Enums.MessageConsts.StartByte))
            {
                 StartByteFound = true; ;
                 return true;
            }
            return false; 
        }
    }
}
