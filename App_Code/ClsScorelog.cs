using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClsScoreErrorlog
/// </summary>
public class ClsScoreErrorlog
{
    public ClsScoreErrorlog()
    {
    }

    public static string strLogPath = AppDomain.CurrentDomain.BaseDirectory;
    public static string strScoreLogFilePath = strLogPath + @"ErrorLog\Scorelog.csv";


    public void WriteToLog(string msg)
    {
        try
        {
            if (!File.Exists(strScoreLogFilePath))
            {
                File.Create(strScoreLogFilePath).Close();
            }
            using (StreamWriter w = File.AppendText(strScoreLogFilePath))
            {
                // w.WriteLine("\r<div classLog: ");
                //w.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                string err = msg;
                w.WriteLine(err);
                w.Flush();
                w.Close();
            }
        }
        catch { }

    }

    public string ReadLogFile()
    {
        try
        {
            string filePath = strScoreLogFilePath;//string.Concat(Path.Combine(_templateDirectory, templateName), ".txt");

            StreamReader sr = new StreamReader(filePath);
            string body = sr.ReadToEnd();
            sr.Close();
            return body;
        }
        catch (Exception exp)
        {
            ClsTestDataLog errlog = new ClsTestDataLog();
            errlog.WriteToLog(exp.ToString());
            return "Error";
            //throw exp;
        }
    }

    public string getLogFileList()
    {
        try
        {
            string strLogPath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dinfo = new DirectoryInfo(strLogPath + @"ErrorLog");
            // What type of file do we want?...
            string logList = "";

            System.IO.FileInfo[] Files = dinfo.GetFiles("*.csv");
            // Iterate through each file, displaying only the name inside the listbox...


            foreach (System.IO.FileInfo file in Files)
            {
                logList += file.Name + "/";
            }



            return logList.Substring(0, logList.Length - 1);
        }
        catch (Exception exp)
        {
            ClsTestDataLog errlog = new ClsTestDataLog();
            errlog.WriteToLog(exp.ToString());
            return "error";
            //throw exp;
        }

    }

    public string renameFile()
    {
        try
        {
            string strLogPath = AppDomain.CurrentDomain.BaseDirectory;
            // System.IO.File.Move(
            //System.IO.FileInfo fi = new System.IO.FileInfo(strLogPath + @"log\log.txt");

            //fi.MoveTo("log[" + DateTime.Now.ToString() + @"].txt");



            System.IO.File.Move(strScoreLogFilePath, strLogPath + @"ErrorLog\Scorelog(" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + ").csv");
            createNewScoreLog();
            return "success";
        }
        catch (Exception exp)
        {
            ClsTestDataLog errlog = new ClsTestDataLog();
            errlog.WriteToLog(exp.ToString());
            return "failed";
            //throw exp;
        }

    }
    public void createNewScoreLog()
    {
        try
        {
            string strLogPath = AppDomain.CurrentDomain.BaseDirectory;

            if (System.IO.File.Exists(strScoreLogFilePath))
            {

            }
            else
            {

                System.IO.File.Create(strScoreLogFilePath);
            }

        }
        catch (Exception exp)
        {
            ClsTestDataLog errlog = new ClsTestDataLog();
            errlog.WriteToLog(exp.ToString());
            //throw exp;
        }

    }

}