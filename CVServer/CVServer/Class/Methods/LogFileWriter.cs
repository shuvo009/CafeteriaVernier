using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Procesta.CvServer
{
    public class LogFileWriter
    {
        public static void ErrorToLog(string header, Exception occuredException)
        {
            try
            {
                LogFileWriter tempLogFileWriter = new LogFileWriter();
                Task writelog = new Task(() => tempLogFileWriter.writeToLogFile(header, occuredException));
                writelog.Start();
            }
            catch
            {
                throw;
            }
        }

        private void writeToLogFile(string header, Exception occuredException)
        {
            try
            {
                string strLogMessage = string.Empty;
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CVServer");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string strLogFile = Path.Combine(folderPath, "CvServer.log");
                StreamWriter swLog;
                strLogMessage = string.Format("{0} : {1} => {2}", DateTime.Now, header, occuredException.Message);
                if (!File.Exists(strLogFile))
                {
                    swLog = new StreamWriter(strLogFile);
                }
                else
                {
                    swLog = File.AppendText(strLogFile);
                }
                swLog.WriteLine(strLogMessage);
                swLog.WriteLine();
                swLog.Close();
            }
            catch (Exception)
            {
                throw;
            }

        } 

    }
}
