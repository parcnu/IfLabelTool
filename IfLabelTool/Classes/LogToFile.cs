using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace IfLabelTool
{
    class LogToFile
    {
        public delegate void myException(string e);
        public static event myException ExceptionEvent;
        

        public void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
        }

        public void CloseFile()
        {

        }

        public void AppendFile(string str)
        {

        }

       
    }
}
