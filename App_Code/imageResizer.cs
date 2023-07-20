using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using SWFToImage;

/// <summary>
/// Summary description for imageResizer
/// 
/// Important NOTE:-
/// For using namespace SWFToImage, Add reference Introp.SWFTTOImage.dll before that download and install SwfToImage software from http://bytescout.com/files/SWFToImage.exe
/// to run SwfToImage software, flash player should be installed...
/// </summary>
public class imageResizer
{

    public imageResizer()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// image resizing...
    /// </summary>
    /// <param name="source">Path of the source image</param>
    /// <param name="destination">path of the rescaled destinaion file (It should conatine full path means; it contain filename and file extension eg: D:\path\filename.jpg</param>
    /// <param name="maxHeight">size for rescaling</param>
    public int ScaleImage(string source, string destination, int maxHeight)
    {
        try
        {
            Bitmap sourceImage = new Bitmap(source);
            Image image = sourceImage;
            if ((image.Height > maxHeight) || (image.Width > maxHeight))
            {
                var ratio = (double)maxHeight / image.Height;
                if (image.Height < image.Width)
                {
                    ratio = (double)maxHeight / image.Width;
                }

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newImage = new Bitmap(newWidth, newHeight);
                using (var g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                newImage.Save(destination);
                newImage.Dispose();
            }
            else
            {
                image.Save(destination);
            }
            image.Dispose();
            sourceImage.Dispose();
            return 1;
        }
        catch(Exception Ex)
        {
            return 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
    }
    /// <summary>
    /// Genratoring thumbnil from a video
    /// </summary>
    /// <param name="ffmpegPath">path of ffmpeg.exe in project</param>
    /// <param name="videoPath">path of source video</param>
    /// <param name="sessionSource">session name</param>
    /// <param name="destination">destination path</param>
    /// <param name="maxsize">size of thumbnil image</param>
    /// <returns>return path of the generated images</returns>
    public string videoThumbnailGenrator(string ffmpegPath, string videoPath, string sessionSource, string destination, int maxsize, int userid)
    {
        Random randomNo = new Random();
        //int sessionid = randomNo.Next(100, 999);
        int sessionid = userid;
        //StreamReader readcmd, readcmd2;
        int hh, mm, ss;
        string[] splitDuration;
        string returnPaths = string.Empty;
        string duration, readcmdString;
        Process ffmpeg = new Process();
        ffmpeg.StartInfo.CreateNoWindow = true;
        ffmpeg.StartInfo.UseShellExecute = false;
        ffmpeg.StartInfo.RedirectStandardOutput = true;
        ffmpeg.StartInfo.RedirectStandardError = true;
        ffmpeg.StartInfo.Arguments = "-i \"" + videoPath;
        ffmpeg.StartInfo.FileName = ffmpegPath;
        ffmpeg.Start();
        //readcmd = ffmpeg.StandardError;

        readcmdString = ffmpeg.StandardError.ReadToEnd() + ffmpeg.StandardOutput.ReadToEnd();
        ffmpeg.WaitForExit();
        ffmpeg.Close();
        duration = readcmdString.Substring(readcmdString.IndexOf("Duration: ") + ("Duration: ").Length, ("00:00:00").Length);
        splitDuration = duration.Split(':');
        try
        {
            hh = Convert.ToInt32(splitDuration[0]);
            mm = Convert.ToInt32(splitDuration[1]);
            ss = Convert.ToInt32(splitDuration[2]);
        }
        catch(Exception Ex)
        {
            hh = mm = ss = 0;
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
        }
        for (int count = 0; count < 3; count++)
        {
            if (!Directory.Exists(destination + "\\" + sessionSource + sessionid))
            {
                Directory.CreateDirectory(destination + "\\" + sessionSource + sessionid);
            }
            if (hh <= 0 && mm <= 0 && ss > 0)
            {
                ffmpeg.StartInfo.Arguments = "-i \"" + videoPath + "\"  -ss 00:00:" + randomNo.Next(0, ss) + " -vframes 1 -f image2 -vcodec mjpeg \"" + destination + "\\" + sessionSource + sessionid + "\\" + count + ".jpg" + "\"";
            }
            else if (hh <= 0 && mm > 0)
            {
                ffmpeg.StartInfo.Arguments = "-i \"" + videoPath + "\"  -ss 00:" + randomNo.Next(0, mm) + ":" + randomNo.Next(0, ss) + " -vframes 1 -f image2 -vcodec mjpeg \"" + destination + "\\" + sessionSource + sessionid + "\\" + count + ".jpg" + "\"";
            }
            else if (hh > 0)
            {
                ffmpeg.StartInfo.Arguments = "-i \"" + videoPath + "\"  -ss " + randomNo.Next(0, hh) + ":" + randomNo.Next(0, mm) + ":" + randomNo.Next(0, ss) + " -vframes 1 -f image2 -vcodec mjpeg \"" + destination + "\\" + sessionSource + sessionid + "\\" + count + ".jpg" + "\"";
            }
            else
            {
                ffmpeg.StartInfo.Arguments = "-i \"" + videoPath + "\"  -ss 1 -vframes 1 -f image2 -vcodec mjpeg \"" + destination + "\\" + sessionSource + sessionid + "\\" + count + ".jpg" + "\"";
            }
            ffmpeg.StartInfo.RedirectStandardOutput = false;
            ffmpeg.StartInfo.RedirectStandardError = false;
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            ffmpeg.Close();
            ScaleImage(destination + "\\" + sessionSource + sessionid + "\\" + count + ".jpg", destination + "\\" + sessionSource + sessionid + "\\" + count + count + ".jpg", maxsize);
            if (File.Exists(destination + "\\" + sessionSource + sessionid + "\\" + count + ".jpg"))
            {
                File.Delete(destination + "\\" + sessionSource + sessionid + "\\" + count + ".jpg");
            }
            returnPaths += sessionSource + sessionid + "/" + count + count + ".jpg" + "*";
        }
        return returnPaths;
    }
    /// <summary>
    /// After select thumbnail, delect all thumbnile before seleted thumbnil copy to the thumbnail folder 
    /// </summary>
    /// <param name="copyFileSource">path of the selected thumbnil</param>
    /// <param name="copyFileDestination">destination path to copy selected thumbnail</param>
    /// <param name="filename">file name ( file name should be with extension eg: name.jpg )</param>
    public string deleteUnwantedThumbnail(string copyFileSource, string copyFileDestination, string filename)
    {
        string[] filenamefounder;
        string sourceFilename, parentFolder;
        while (File.Exists(copyFileDestination + "\\" + filename))
        {
            string[] temp = filename.Split('.')[0].Split('_');
            string ext = filename.Split('.')[1];
            if (temp.Length > 2)
            {
                int index = Convert.ToInt32(temp[2]) + 1;
                filename = temp[0] + "_" + temp[1] + "_" + index.ToString() + "." + ext;
            }
            else
            {
                filename = temp[0] + "_" + temp[1] + "_0" + "." + ext;
            }
            
        }
        File.Move(copyFileSource, copyFileDestination + "\\" + filename);
        filenamefounder = copyFileSource.Split('\\');
        sourceFilename = filenamefounder[filenamefounder.Length - 1];
        parentFolder = copyFileSource.Substring(0, copyFileSource.IndexOf(sourceFilename));
        Directory.Delete(parentFolder, true);
        return filename;
    }
    /// <summary>
    /// it scale image and return its image object
    /// </summary>
    /// <param name="source">path of the image</param>
    /// <param name="maxHeight">resize size</param>
    /// <returns>return resized image object</returns>
    public Image ScaleImageGetBitmap(string source, int maxHeight)
    {
        Image image = Image.FromFile(source);
        if ((image.Height > maxHeight) || (image.Width > maxHeight))
        {
            var ratio = (double)maxHeight / image.Height;
            if (image.Height < image.Width)
            {
                ratio = (double)maxHeight / image.Width;
            }

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            image.Dispose();
            return newImage;
        }
        else
        {
            return image;

        }

    }
    /// <summary>
    /// scale a image width only
    /// </summary>
    /// <param name="img">Image object of Rescaleable image</param>
    /// <param name="size">width size to rescale it</param>
    /// <returns>return resized Image Object</returns>
    public Image scaleWidthGetBitmap(Image img, double size)
    {
        if ((img.Height > size) || (img.Width > size))
        {
            var ratio = (double)size / img.Width;
            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(img, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        else
        {
            return img;
        }
    }
    /// <summary>
    /// scale a images height only
    /// </summary>
    /// <param name="img">Image object of Rescaleable image</param>
    /// <param name="size">heigh size to rescale it</param>
    /// <returns>return resized Image Object</returns>
    public Image scaleheightGetBitmap(Image img, double size)
    {
        if ((img.Height > size) || (img.Width > size))
        {
            var ratio = (double)size / img.Height;
            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(img, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        else
        {
            return img;
        }
    }
    /// <summary>
    /// Swf (flash file) thumbnil generator, it generat jpg file from swf fist frame..
    /// </summary>
    /// <param name="sourceSwfPath">source path of the swf (flash) file</param>
    /// <param name="destinationPath">destination path of the folder which the thumbnil want to save</param>
    /// <param name="filename">file name of the thumbnil it should be without file extension( eg: name)</param>
    /// <param name="PathOfFashIcon">Flash Icon file path, its should be transparent png file</param>
    /// <param name="size">maximun size of the thumbnil file</param>
    /// <returns>it return 1 when it couse an error, else it return 0 ( no error )</returns>
    public int swfThumbnilGenrator(string sourceSwfPath, string destinationPath, string filename, string PathOfFashIcon, int size)
    {
        try
        {
            SWFToImageObjectClass objS2Img = new SWFToImageObjectClass();
            objS2Img.InputSWFFileName = sourceSwfPath;
            objS2Img.ImageOutputType = TImageOutputType.iotJPG;
            objS2Img.Execute();
            objS2Img.SaveToFile(destinationPath + "/" + filename + ".jpg");
            Image img = ScaleImageGetBitmap(destinationPath + "/" + filename + ".jpg", size);
            Image f = Image.FromFile(PathOfFashIcon);
            if (img.Height < 100)
            {
                f = scaleheightGetBitmap(f, img.Height / Convert.ToDouble(size / f.Height));
            }
            if (img.Width < 100)
            {
                f = scaleWidthGetBitmap(f, Convert.ToDouble(img.Width) / Convert.ToDouble(size / f.Width));
            }
            Graphics grap = Graphics.FromImage(img);
            grap.DrawImage(f, new Point(img.Width - f.Width, img.Height - f.Height));
            img.Save(destinationPath + "/" + filename + "temp.jpg");
            img.Dispose();
            grap.Dispose();
            f.Dispose();
            File.Delete(destinationPath + "/" + filename + ".jpg");
            File.Move(destinationPath + "/" + filename + "temp.jpg", destinationPath + "/" + filename + ".jpg");
            return 0;
        }
        catch(Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
            return 1;
            
        }
    }
}