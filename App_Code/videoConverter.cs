using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Summary description for videoConverter
/// </summary>
public class videoConverter
{
	public videoConverter()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    /// <summary>
    /// almost all video format to flv converter..
    /// It use ffmpeg to use encoding and decoding.
    /// to run these code want to put ffmpeg in project; download it from http://www.ffmpeg.org/download.html
    /// </summary>
    /// <param name="ffmpegPath">server path of ffmpeg.exe</param>
    /// <param name="SourcevideoPath">source video path</param>
    /// <param name="destinationvideoPath">destination path of a folder that converted video to copy</param>
    /// <param name="fileID">File name for the convered file, file name should be without file extionsion</param>
    /// <param name="quality">quality meansl; quality value are 1,2 and 3 for LOW, MEDIUM and HIGH quality,quality value should be any of these values</param>
    /// <param name="width">to change video to size of width; to need to change the width of video, give width = 0</param>
    /// <param name="height">to change video to size of height; to need to change the height of video, give height = 0</param>
    /// <returns>returns 0, when it convert video to flv successfully; else non zero value any error</returns>
    public int videoFormtConverter(string ffmpegPath, string SourcevideoPath, string destinationvideoPath,string fileID,int quality,int width,int height)
    {
        Random randomno = new Random();
        string randirectory = randomno.Next(100, 999).ToString();
        //Directory.CreateDirectory(destinationvideoPath+"\\"+ randirectory.ToString());
        int iErrorCode;
        Process ffmpeg = new Process();
        int qualityValue = 200;
        ffmpeg.StartInfo.CreateNoWindow = true;
        ffmpeg.StartInfo.UseShellExecute = false;
        ffmpeg.StartInfo.RedirectStandardOutput = false;
        ffmpeg.StartInfo.RedirectStandardError = false;
        if (quality == 1)
        {
            qualityValue = 400;
        }
        else if (quality == 2)
        {
            qualityValue = 900;
        }
        else if (quality == 3)
        {
            qualityValue = 1200;
        }
        if (width > 0 && height > 0)
        {
            ffmpeg.StartInfo.Arguments = "-i \"" + SourcevideoPath + "\" -vb " + qualityValue + "k -s " + width + "x" + height + " \"" + destinationvideoPath+"\\"+fileID+".flv\"";
        }
        else
        {
            ffmpeg.StartInfo.Arguments = "-i \"" + SourcevideoPath + "\" -vb " + qualityValue + "k \"" + destinationvideoPath + "\\" + fileID + ".flv\"";
        }
        if (File.Exists(destinationvideoPath + "\\" + fileID + ".flv"))
        {
            File.Delete(destinationvideoPath + "\\" + fileID + ".flv");
        }
        ffmpeg.StartInfo.FileName = ffmpegPath;
        ffmpeg.Start();
        ffmpeg.WaitForExit(500000);
        iErrorCode = ffmpeg.ExitCode;
        ffmpeg.Close();
        if (File.Exists(destinationvideoPath + "\\" + fileID + ".flv"))
        {
            File.Delete(SourcevideoPath);
        }
        //return 0 if convertion success.. otherwise not 0 will return
        return iErrorCode;
    }
}