using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfLabelTool.Classes
{
    class SystemLogFile
    {
        public string LogFileName { get; set; }
        public string LogFilePath { get; set; }
        private StreamWriter fs;


        public bool WriteToFile(string str)
        {
            try
            {
                using (fs = File.AppendText(LogFilePath + @"\" + LogFileName))
                {
                    //Byte[] title = new UTF8Encoding(true).GetBytes(str + Environment.NewLine);
                    string title = str + Environment.NewLine;
                    fs.WriteLine(title);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateSystemLogFile(String str)
        {
            try
            {
                using (fs = File.CreateText(LogFilePath + @"\" + LogFileName))
                {
                    //Byte[] title = new UTF8Encoding(true).GetBytes(str + Environment.NewLine);
                    string title = str + Environment.NewLine;
                    fs.WriteLine(title);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;   
            }
        }

        public void CloseSystemLogFile()
        {
            fs.Close();
            fs.Dispose();

        }
    }
}
