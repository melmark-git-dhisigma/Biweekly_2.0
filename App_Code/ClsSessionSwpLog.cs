﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClsSessionSwpLog
/// </summary>
public class ClsSessionSwpLog
{
	public ClsSessionSwpLog()
	{
	}
    public static string strPath = AppDomain.CurrentDomain.BaseDirectory;
    public static string strLogFilePath = strPath + @"ErrorLog\SessionSwplog.csv";


    public void WriteToLog(string msg)
    {
        try
        {
            if (!File.Exists(strLogFilePath))
            {
                File.Create(strLogFilePath).Close();
            }
            using (StreamWriter w = File.AppendText(strLogFilePath))
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
            string filePath = strLogFilePath;//string.Concat(Path.Combine(_templateDirectory, templateName), ".txt");

            StreamReader sr = new StreamReader(filePath);
            string body = sr.ReadToEnd();
            sr.Close();
            return body;
        }
        catch (Exception exp)
        {
            WriteToLog(exp.ToString());
            return "Error";
            //throw exp;
        }
    }

    public string getLogFileList()
    {
        try
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dinfo = new DirectoryInfo(strPath + @"ErrorLog");
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
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog(exp.ToString());
            return "error";
            //throw exp;
        }

    }

    public string renameFile()
    {
        try
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            // System.IO.File.Move(
            //System.IO.FileInfo fi = new System.IO.FileInfo(strPath + @"log\log.txt");

            //fi.MoveTo("log[" + DateTime.Now.ToString() + @"].txt");



            System.IO.File.Move(strPath + @"ErrorLog\SessionSwplog.csv", strPath + @"ErrorLog\SessionSwplog(" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + ").csv");
            createNewLog();
            return "success";
        }
        catch (Exception exp)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog(exp.ToString());
            return "failed";
            //throw exp;
        }

    }
    public void createNewLog()
    {
        try
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;

            if (System.IO.File.Exists(strPath + @"ErrorLog/SessionSwplog.csv"))
            {

            }
            else
            {

                System.IO.File.Create(strPath + @"ErrorLog/SessionSwplog.csv");
            }

        }
        catch (Exception exp)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog(exp.ToString());
            //throw exp;
        }

    }
}