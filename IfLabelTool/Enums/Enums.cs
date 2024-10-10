using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfLabelTool
{
    class Enums
    {
        public struct CommandTypes
        {
            public const string ReadCommand = "01";
            public const string WriteCommand = "02";
            public const string PassiveReportCommand = "03";
            public const string ActiveReportCommand = "04";
        }

        public struct AddressCodeOneReadCodes                   //CommadType = 01
        {
            public const string IdentificationQuery = "01";
            public const string RadarInformationQuery = "03";
            public const string SystemParameter = "04";
            public const string OtherFunctions = "05";
            public struct AddressCodeTwoIdentificationQueryCodes     //Command code = 01 (ReadCommand) AddrCodeOne = 01 (IdentificationQuery)
            {
                public const string DeviceID = "01";
                public const string SWVersion = "02";
                public const string HWVersion = "03";
                public const string ProtocolVersion = "04";
            }

            public struct AddressCodeTwoReportRadaInformationQueryCodes  //Command code 01 AddrConeOne 03
            {
                public const string EnvironmentalStatus = "05";
                public const string SignParameter = "06";

            }

            public struct AddressCodeTwoReportSystemParametersCodes //Command code 01 AddrCodeOne 04
            {
                public const string CurrentGearValue = "0C";
                public const string SceneSetting = "10";
            }

        }

        public struct AddressCodeOneWriteCodes
        {
            public const string SystemParameter = "04";
            public const string OtherFunctions = "05";

            public struct AddressCodeTwoSystemParameterCodes
            {
                public const string TresholdGear = "0C";
                public const string SceneSetting = "10";
            }

            public struct AddressCodeTwoOtherFunctionsCodes
            {
                public const string HeatbeatPackage = "01";
                public const string Restart = "04";
            }

        }


        public struct AddressCodeOnePassiveReportCodes
        {
            public const string ReportModuleID = "01";
            public const string ReportRadarInformation = "03";
            public const string ReportSystemParameter = "04";
            public struct AddressCodeTwoReportModuleIDCodes
            {
                public const string DeviceID = "01";
                public const string SWVersion = "02";
                public const string HWVersion = "03";
                public const string ProtocolVersion = "04";
            }

            public struct AddressCodeTwoReportRadarInformationCodes
            {
                public const string Environment = "05";
                public const string SignParameter = "06";
            }

            public struct AddressCodeTwoReportSystemParameterCodes
            {
                public const string TresholdGear = "0C";
                public const string SceneSetting = "10";
            }

        }

        public struct AddressCodeOneActiveReportCodes
        {
            public const string RadarInformation = "03";
            public const string ReportOtherInformation = "05";

            public struct AddressCodeTwoReportRadarInformation
            {
                public const string EnvironmentStatus = "05";
                public const string SignParameter = "06";
                public const string ApproachOrAway = "07";

                public struct EnvironmentStatusCodes
                {
                    public const string Nobody = "00FFFF";
                    public const string SomeoneStationary = "0100FF";
                    public const string SomeoneMoving = "010101";
                }

                public struct ApproachingOrAwayCodes
                {
                    public const string None = "010101";
                    public const string Close = "010102";
                    public const string Away = "010103";
                }

            }

            public struct AddressCodeTwoReportOtherInfoCodes
            {
                public const string HeartbeatPackage = "01";
                public const string AbnormalReset = "02";
                public struct HeartbeatPacageCodes
                {
                    public const string Nobody = "00FFFF";
                    public const string SomeoneStationary = "0100FF";
                    public const string SomeoneMovement = "010101";
                }
            }
        }

 

        public struct FunctionNames
        {
            public const string CommandByte = "CommandByte";
            public const string AddressCommandOne = "AddrCmdOne";
            public const string AddressCommandTwo = "AddrCmdTwo";
        }

        public struct MessageConsts
        {
            public const string StartByte = "55";           //Mesasge start byte is 55 following length
            public const int DataReservedBytes = 18;        //predefined bytes = 8 (start code, length high, lenght low, func code, Adr1, Adr2, CRC16_L, CRC16_H) eachone has two bytes.
            public const int NumberOfBytesInWord = 2;
        }

        public struct ErrorCodes
        {
            public const string ErrorLabel = "ERROR: ";
        }

        public struct IfLabelCommonLabels
        {
            public const string SystemLogFileName =  "IFLabelLogFile.txt";
            public const string DateFormat = "yyyyMMddHHmmss";
        }

       
    }
}
