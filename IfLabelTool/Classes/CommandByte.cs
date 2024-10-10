using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IfLabelTool.Classes;
namespace IfLabelTool
{
    /* Command Byte class to extract which command is in question in the message.
     * 
     */
    class CommandByte
    {
        private string _commandChar;

        public SystemLogFile SystemLogFileHandlerI { get; set; }
        public string CommandChar 
        { 
            get
            {
                return _commandChar; 
            } 
          set
            {
                _commandChar = value; 
                CommandToString();
            }
        }
        public string CommandString { get; protected set; }

        /* Method to convert command byte to string which can be shown on UI
         * 
         */
        private void CommandToString()
        {
            if (Char.Equals(CommandChar, Enums.CommandTypes.ReadCommand)) CommandString = "Read command";
            else if (Char.Equals(CommandChar, Enums.CommandTypes.WriteCommand)) CommandString = "Write command";
            else if (Char.Equals(CommandChar, Enums.CommandTypes.PassiveReportCommand)) CommandString = "Passive Report command";
            else if (Char.Equals(CommandChar, Enums.CommandTypes.ActiveReportCommand)) CommandString = "Active report command";
            else
            {
                CommandString = Enums.ErrorCodes.ErrorLabel + " Command Byte error - no correct command " + CommandChar;
                SystemLogFileHandlerI.WriteToFile(Enums.ErrorCodes.ErrorLabel + CommandString);
            }
        }

        public CommandByte()
        {
            SystemLogFileHandlerI = null;
        }

        public CommandByte(SystemLogFile slf)
        {
            SystemLogFileHandlerI = slf;
        }

    }

}
