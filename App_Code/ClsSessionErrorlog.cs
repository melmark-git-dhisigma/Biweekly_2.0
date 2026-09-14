using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for ClsSessionErrorlog
/// </summary>
public class ClsSessionErrorlog
{
    public ClsSessionErrorlog()
    {
    }

    public static string strPath = AppDomain.CurrentDomain.BaseDirectory;
    public static string strLogFilePath = strPath + @"ErrorLog\SessionLog\Sessionlog_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


    public void WriteToLog(string msg)
    {
        if (ConfigurationManager.AppSettings["EnableBiSessionLog"].ToString() == "true")
        {
            try
            {
                string logDirectory = Path.GetDirectoryName(strLogFilePath);

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                if (!File.Exists(strLogFilePath))
                {
                    File.Create(strLogFilePath).Close();
                }
                using (StreamWriter w = File.AppendText(strLogFilePath))
                {
                    w.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Error writing to log file: " + ex.Message);
            }
        }

    }  

}