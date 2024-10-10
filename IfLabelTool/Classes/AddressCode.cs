using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IfLabelTool.Classes;

namespace IfLabelTool
{
    /* Class to maintain address codes in the message
     * See the IFLabel DataSheet for table how these are used.
     */
    class AddressCode
    {
        private string _addrcmdonestr { get; set; }
        private string _addrcmdtwostr { get; set; }

        public string CommandByte { get; set; }
        public string AddrCmdOneString { get; set; }
        public string AddrCmdTwoString { get; set; }
        public SystemLogFile SystemLogFileHandlerI { get; set; }

        public string AddrCmdOne
        {
            get
            {
                return _addrcmdonestr;
            }
            set
            {
                _addrcmdonestr = value;
                GetCmdOneString();
            }
        }
        public string AddrCmdTwo {
            get
            {
                return _addrcmdtwostr;
            }
            set
            {
                _addrcmdtwostr = value;
                GetCmdTwoString();
            }

        }

        /* Read Command 0x01
         *  0x01 = Identification query
         *  0x03 = Radar Information query
         *  0x04 = System parameter
         * 
         * Write Command 0x02
         *  0x04 = System Parameter
         *  0x05 = Other functions
         *  
         * Passive Report command 0x03
         *  0x01 = Report Module ID
         *  0x03 = Report Radar information 
         *  0x04 = Report system parameters
         *  
         * System Parameter 0x04
         *  0x03 = Radar Information
         *  0x05 = Report other information
         */

        /* Method to convert AddressOne to String to be shown on UI.
         * 
         */
        private void GetCmdOneString()
        {
            if (CommandByte == Enums.CommandTypes.ReadCommand)
            {
                if (AddrCmdOne == Enums.AddressCodeOneReadCodes.IdentificationQuery) AddrCmdOneString = "Identification query";
                else if (AddrCmdOne == Enums.AddressCodeOneReadCodes.RadarInformationQuery) AddrCmdOneString = "Radar Information query";
                else if (AddrCmdOne == Enums.AddressCodeOneReadCodes.SystemParameter) AddrCmdOneString = "System parameter";
                else
                {
                    AddrCmdOneString = Enums.ErrorCodes.ErrorLabel + " ReadCommand AddresCodeone error - not identified addr one command byte " + AddrCmdOne;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdOneString);
                }
            }
            else if (CommandByte == Enums.CommandTypes.WriteCommand)
            {
                if (AddrCmdOne == Enums.AddressCodeOneWriteCodes.SystemParameter) AddrCmdOneString = "System parameter";
                else if (AddrCmdOne == Enums.AddressCodeOneWriteCodes.OtherFunctions) AddrCmdOneString = "Other functions";
                else
                {
                    AddrCmdOneString = Enums.ErrorCodes.ErrorLabel + " WriteCommand  error - not identified addr one command byte " + AddrCmdOne;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdOneString);
                }

            }
            else if (CommandByte == Enums.CommandTypes.PassiveReportCommand)
            {
                if (AddrCmdOne == Enums.AddressCodeOnePassiveReportCodes.ReportModuleID) AddrCmdOneString = "Report Module ID";
                else if (AddrCmdOne == Enums.AddressCodeOnePassiveReportCodes.ReportRadarInformation) AddrCmdOneString = "Report Radar information";
                else if (AddrCmdOne == Enums.AddressCodeOnePassiveReportCodes.ReportSystemParameter) AddrCmdOneString = "Report system parameters";
                else
                {
                    AddrCmdOneString = Enums.ErrorCodes.ErrorLabel + " PassiveReportCommand error - not identified addr one command byte " + AddrCmdOne;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdOneString);
                }
            }
            else if (CommandByte == Enums.CommandTypes.ActiveReportCommand)
            {
                if (AddrCmdOne == Enums.AddressCodeOneActiveReportCodes.RadarInformation) AddrCmdOneString = "Radar Information";
                else if (AddrCmdOne == Enums.AddressCodeOneActiveReportCodes.ReportOtherInformation) AddrCmdOneString = "Report other information";
                else
                {
                    AddrCmdOneString = Enums.ErrorCodes.ErrorLabel +  "ActiveReportCommand error - not identified addr one command byte " + AddrCmdOne;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdOneString);
                }
            }

        }

        public AddressCode()
        {
            SystemLogFileHandlerI = null;
        }

        public AddressCode(SystemLogFile slf)
        {
            SystemLogFileHandlerI = slf;
        }

        /* Method to convert Address Commad Two to String to be shown on UI.
         * 
         */
        private void GetCmdTwoString()
        {
            //***********************************************************************************************************************************
            // Read Codes
            //
            //***********************************************************************************************************************************
            if (AddrCmdOne == Enums.AddressCodeOneReadCodes.IdentificationQuery)
            {
                if (AddrCmdTwo == Enums.AddressCodeOneReadCodes.AddressCodeTwoIdentificationQueryCodes.DeviceID) AddrCmdTwoString = "Device ID";
                else if (AddrCmdTwo == Enums.AddressCodeOneReadCodes.AddressCodeTwoIdentificationQueryCodes.SWVersion) AddrCmdTwoString = "SW Version";
                else if (AddrCmdTwo == Enums.AddressCodeOneReadCodes.AddressCodeTwoIdentificationQueryCodes.HWVersion) AddrCmdTwoString = "HW Version";
                else if (AddrCmdTwo == Enums.AddressCodeOneReadCodes.AddressCodeTwoIdentificationQueryCodes.ProtocolVersion) AddrCmdTwoString = "Protocol version";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Read Command Identification Query error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }
            else if (AddrCmdOne == Enums.AddressCodeOneActiveReportCodes.RadarInformation)
            {
                if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportRadarInformation.EnvironmentStatus) AddrCmdTwoString = "Environmental Status";
                else if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportRadarInformation.SignParameter) AddrCmdTwoString = "Sign Parameter";
                else if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportRadarInformation.ApproachOrAway) AddrCmdTwoString = "Approaching or Away";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Read Command Radar Information Query error -not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }
            else if (AddrCmdOne == Enums.AddressCodeOneReadCodes.SystemParameter)
            {
                if (AddrCmdTwo == Enums.AddressCodeOneReadCodes.AddressCodeTwoReportSystemParametersCodes.CurrentGearValue) AddrCmdTwoString = "Treshold Gear";
                else if (AddrCmdTwo == Enums.AddressCodeOneReadCodes.AddressCodeTwoReportSystemParametersCodes.SceneSetting) AddrCmdTwoString = "Scene Setting";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Read Command System Parameter error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }

            //***********************************************************************************************************************************
            // Write Codes
            //
            //***********************************************************************************************************************************
            else if (AddrCmdOne == Enums.AddressCodeOneWriteCodes.AddressCodeTwoSystemParameterCodes.TresholdGear)
            {
                if (AddrCmdTwo == Enums.AddressCodeOneWriteCodes.AddressCodeTwoSystemParameterCodes.TresholdGear) AddrCmdTwoString = "Treshold Gear";
                else if (AddrCmdTwo == Enums.AddressCodeOneWriteCodes.AddressCodeTwoSystemParameterCodes.SceneSetting) AddrCmdTwoString = "Scene Setting";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Write Command Treshold Gear error - not identified addr two command byte" + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }
            else if (AddrCmdOne == Enums.AddressCodeOneWriteCodes.OtherFunctions)
            {
                if (AddrCmdTwo == Enums.AddressCodeOneWriteCodes.AddressCodeTwoOtherFunctionsCodes.HeatbeatPackage) AddrCmdTwoString = "Heartbeat Package";
                else if (AddrCmdTwo == Enums.AddressCodeOneWriteCodes.AddressCodeTwoOtherFunctionsCodes.Restart) AddrCmdTwoString = "Restart";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Write Command Other Functions error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }

            //***********************************************************************************************************************************
            // Passive Report Codes
            //
            //***********************************************************************************************************************************
            else if (AddrCmdOne == Enums.AddressCodeOnePassiveReportCodes.ReportModuleID)
            {
                if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportModuleIDCodes.DeviceID) AddrCmdTwoString = "Device ID";
                else if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportModuleIDCodes.SWVersion) AddrCmdTwoString = "SW Version";
                else if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportModuleIDCodes.HWVersion) AddrCmdTwoString = "HW Version";
                else if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportModuleIDCodes.ProtocolVersion) AddrCmdTwoString = "Protocol Version";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Passive Report Report Module ID error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }
            else if (AddrCmdOne == Enums.AddressCodeOnePassiveReportCodes.ReportRadarInformation)
            {
                if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportRadarInformationCodes.Environment) AddrCmdTwoString = "Environment Status";
                else if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportRadarInformationCodes.SignParameter) AddrCmdTwoString = "Sign Parameter";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Passive Report Report Radar Information error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }
            else if (AddrCmdOne == Enums.AddressCodeOnePassiveReportCodes.ReportSystemParameter)
            {
                if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportSystemParameterCodes.TresholdGear) AddrCmdTwoString = "Threshold Gear";
                else if (AddrCmdTwo == Enums.AddressCodeOnePassiveReportCodes.AddressCodeTwoReportSystemParameterCodes.SceneSetting) AddrCmdTwoString = "Scene Setting";
                else
                {
                    AddrCmdTwoString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Passive Report Report System Parameter error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }

            //***********************************************************************************************************************************
            // Active Report Codes
            //
            //***********************************************************************************************************************************
            else if (AddrCmdOne == Enums.AddressCodeOneActiveReportCodes.RadarInformation)
            {
                if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportRadarInformation.EnvironmentStatus) AddrCmdTwoString = "Environment Status";
                else if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportRadarInformation.SignParameter) AddrCmdTwoString = "Sign Parameter";
                else if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportRadarInformation.ApproachOrAway) AddrCmdTwoString = "Approaching Or Away";
                else
                {
                    AddrCmdOneString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Active Report Radar Information error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }
            else if (AddrCmdOne == Enums.AddressCodeOneActiveReportCodes.ReportOtherInformation)
            {
                if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportOtherInfoCodes.HeartbeatPackage) AddrCmdTwoString = "Heartbeat Package";
                else if (AddrCmdTwo == Enums.AddressCodeOneActiveReportCodes.AddressCodeTwoReportOtherInfoCodes.AbnormalReset) AddrCmdTwoString = "Abnormal Reset";
                else
                {
                    AddrCmdOneString = Enums.ErrorCodes.ErrorLabel + AddrCmdOneString + " Active Report Report Other Information error - not identified addr two command byte " + AddrCmdTwo;
                    SystemLogFileHandlerI.WriteToFile(AddrCmdTwoString);
                }
            }


            //else if (CommandByte == "03")
            //{
            //    if (AddrCmdOne == "03") AddrCmdOneString = "Radar Information";
            //    else if (AddrCmdOne == "05") AddrCmdOneString = "Report other informationn";
            //    else AddrCmdOneString = "error - not identified addr one command byte";
            //}



        }

    }
}
