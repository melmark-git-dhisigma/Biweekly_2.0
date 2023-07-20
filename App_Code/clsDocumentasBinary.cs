using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;
using Microsoft.Office.Interop.Word;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Vml.Office;

using Ap = DocumentFormat.OpenXml.ExtendedProperties;
using Vt = DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml;

using V = DocumentFormat.OpenXml.Vml;
using Ovml = DocumentFormat.OpenXml.Vml.Office;
using Ds = DocumentFormat.OpenXml.CustomXmlDataProperties;
using A = DocumentFormat.OpenXml.Drawing;
using M = DocumentFormat.OpenXml.Math;
using System.Threading;



public class clsDocumentasBinary
{
    private static Dictionary<string, string> contentTypes;






    #region//Convert Doc to Html String

    public string ConvertToHtml(string TemplatePath, string Path, out string HtmlFile)
    {
        Guid g = Guid.NewGuid();
        string ids = g.ToString();

        //Create HTML File
        HtmlFile = Path + "HTML" + ids + ".html";

        //Convert OriginalTemplate to Html File
        PageConvert(TemplatePath, HtmlFile, WdSaveFormat.wdFormatHTML);

        Thread.Sleep(500);
        string htmlString = File.ReadAllText(HtmlFile);

        return htmlString;
    }


    public byte[] ConvertToByte(string htmlString, string Path, string HtmlNew)
    {
        Guid g = Guid.NewGuid();
        string ids = g.ToString();

        htmlString = htmlString.Replace("’", "'");
        htmlString = htmlString.Replace("…", "...");
        htmlString = htmlString.Replace("‘", "'");
        htmlString = htmlString.Replace("·", "- ");
        htmlString = htmlString.Replace("“", "'");
        htmlString = htmlString.Replace("”", "'");
        htmlString = htmlString.Replace("<table>", "<table style='width:100%'>");
        htmlString = htmlString.Replace("<th>", "<th style='text-align:left;'>");
        htmlString = htmlString.Replace("MsoListParagraphCxSpFirst", "-");
        htmlString = htmlString.Replace("mso-list", "-");
        htmlString = htmlString.Replace("�", "");
        htmlString = htmlString.Replace("order:none'>", "");

        string strBody = string.Empty;
        //strBody = @"<head><html xmlns:o='urn:schemas-microsoft-com:office:office' " +
        //"xmlns:w='urn:schemas-microsoft-com:office:word'" +
        //"xmlns=''>";

        strBody = strBody + "<!--[if gte mso 9]>" +
        "<xml>" +
        "<w:WordDocument>" +
        "<w:View>Print</w:View>" +
        "<w:Zoom>100</w:Zoom>" +
        "</w:WordDocument>" +
        "</xml>" +
        "<![endif]-->";

        htmlString = htmlString.Replace("<head>", strBody);


        FileStream Fs = File.OpenWrite(HtmlNew);
        StreamWriter str = new StreamWriter(Fs, Encoding.UTF8);
        str.Write(htmlString);
        str.Close();

        string filePath = Path + "Template" + ids + ".docx";

        Thread.Sleep(1000);
        PageConvert(HtmlNew, filePath, WdSaveFormat.wdFormatDocumentDefault);

        Thread.Sleep(1000);
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);

        DeleteFiles(Path);

        return bytes;
    }
    public static Boolean IsFileLocked(FileInfo file)
    {
        FileStream stream = null;

        try
        {
            //Don't change FileAccess to ReadWrite, 
            //because if a file is in readOnly, it fails.
            stream = file.Open
            (
                FileMode.Open,
                FileAccess.Read,
                FileShare.None
            );
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }

        //file is not locked
        return false;
    }
    private static void DeleteFiles(string path)
    {
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(path);
        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            if (file.Exists) if (!IsFileLocked(file)) file.Delete();
        }
        foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
        {
            if (dir.Exists) dir.Delete(true);
        }
    }

    private void PageConvert(string input, string output, Microsoft.Office.Interop.Word.WdSaveFormat format)
    {
        try
        {
            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object bConfirmDialog = false;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = format;
            object oFileShare = true;

            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref bConfirmDialog, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Make this document the active document.
            oDoc.Activate();

            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #endregion



    private void PageConvert(string input, string output)
    {
        try
        {
            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;
            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object bConfirmDialog = false;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
            object oFileShare = true;
            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref bConfirmDialog, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            // Make this document the active document.
            oDoc.Activate();
            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);

        }
        catch (IOException ex)
        {
            throw ex;
        }
    }







    #region

    private static void InitializeMimeTypes()
    {
        contentTypes = new Dictionary<string, string>
                               {
                                   {"3dm", "x-world/x-3dmf"},
                                   {"3dmf", "x-world/x-3dmf"},
                                   {"a", "application/octet-stream"},
                                   {"aab", "application/x-authorware-bin"},
                                   {"aam", "application/x-authorware-map"},
                                   {"aas", "application/x-authorware-seg"},
                                   {"abc", "text/vnd.abc"},
                                   {"acgi", "text/html"},
                                   {"afl", "video/animaflex"},
                                   {"ai", "application/postscript"},
                                   {"aif", "audio/aiff"},
                                   {"aifc", "audio/aiff"},
                                   {"aiff", "audio/aiff"},
                                   {"aim", "application/x-aim"},
                                   {"aip", "text/x-audiosoft-intra"},
                                   {"ani", "application/x-navi-animation"},
                                   {"aos", "application/x-nokia-9000-communicator-add-on-software"},
                                   {"aps", "application/mime"},
                                   {"arc", "application/octet-stream"},
                                   {"arj", "application/arj"},
                                   {"art", "image/x-jg"},
                                   {"asf", "video/x-ms-asf"},
                                   {"asm", "text/x-asm"},
                                   {"asp", "text/asp"},
                                   {"asx", "application/x-mplayer2"},
                                   {"au", "audio/basic"},
                                   {"avi", "video/avi"},
                                   {"avs", "video/avs-video"},
                                   {"bcpio", "application/x-bcpio"},
                                   {"bin", "application/octet-stream"},
                                   {"bm", "image/bmp"},
                                   {"bmp", "image/bmp"},
                                   {"boo", "application/book"},
                                   {"book", "application/book"},
                                   {"boz", "application/x-bzip2"},
                                   {"bsh", "application/x-bsh"},
                                   {"bz", "application/x-bzip"},
                                   {"bz2", "application/x-bzip2"},
                                   {"c", "text/plain"},
                                   {"c++", "text/plain"},
                                   {"cat", "application/vnd.ms-pki.seccat"},
                                   {"cc", "text/plain"},
                                   {"ccad", "application/clariscad"},
                                   {"cco", "application/x-cocoa"},
                                   {"cdf", "application/cdf"},
                                   {"cer", "application/pkix-cert"},
                                   {"cha", "application/x-chat"},
                                   {"chat", "application/x-chat"},
                                   {"class", "application/java"},
                                   {"com", "application/octet-stream"},
                                   {"conf", "text/plain"},
                                   {"cpio", "application/x-cpio"},
                                   {"cpp", "text/x-c"},
                                   {"cpt", "application/x-cpt"},
                                   {"crl", "application/pkcs-crl"},
                                   {"css", "text/css"},
                                   {"def", "text/plain"},
                                   {"der", "application/x-x509-ca-cert"},
                                   {"dif", "video/x-dv"},
                                   {"dir", "application/x-director"},
                                   {"dl", "video/dl"},
                                   {"doc", "application/msword"},
                                   {"dot", "application/msword"},
                                   {"dp", "application/commonground"},
                                   {"drw", "application/drafting"},
                                   {"dump", "application/octet-stream"},
                                   {"dv", "video/x-dv"},
                                   {"dvi", "application/x-dvi"},
                                   {"dwf", "drawing/x-dwf (old)"},
                                   {"dwg", "application/acad"},
                                   {"dxf", "application/dxf"},
                                   {"eps", "application/postscript"},
                                   {"es", "application/x-esrehber"},
                                   {"etx", "text/x-setext"},
                                   {"evy", "application/envoy"},
                                   {"exe", "application/octet-stream"},
                                   {"f", "text/plain"},
                                   {"f90", "text/x-fortran"},
                                   {"fdf", "application/vnd.fdf"},
                                   {"fif", "image/fif"},
                                   {"fli", "video/fli"},
                                   {"for", "text/x-fortran"},
                                   {"fpx", "image/vnd.fpx"},
                                   {"g", "text/plain"},
                                   {"g3", "image/g3fax"},
                                   {"gif", "image/gif"},
                                   {"gl", "video/gl"},
                                   {"gsd", "audio/x-gsm"},
                                   {"gtar", "application/x-gtar"},
                                   {"gz", "application/x-compressed"},
                                   {"h", "text/plain"},
                                   {"help", "application/x-helpfile"},
                                   {"hgl", "application/vnd.hp-hpgl"},
                                   {"hh", "text/plain"},
                                   {"hlp", "application/x-winhelp"},
                                   {"htc", "text/x-component"},
                                   {"htm", "text/html"},
                                   {"html", "text/html"},
                                   {"htmls", "text/html"},
                                   {"htt", "text/webviewhtml"},
                                   {"htx", "text/html"},
                                   {"ice", "x-conference/x-cooltalk"},
                                   {"ico", "image/x-icon"},
                                   {"idc", "text/plain"},
                                   {"ief", "image/ief"},
                                   {"iefs", "image/ief"},
                                   {"iges", "application/iges"},
                                   {"igs", "application/iges"},
                                   {"ima", "application/x-ima"},
                                   {"imap", "application/x-httpd-imap"},
                                   {"inf", "application/inf"},
                                   {"ins", "application/x-internett-signup"},
                                   {"ip", "application/x-ip2"},
                                   {"isu", "video/x-isvideo"},
                                   {"it", "audio/it"},
                                   {"iv", "application/x-inventor"},
                                   {"ivr", "i-world/i-vrml"},
                                   {"ivy", "application/x-livescreen"},
                                   {"jam", "audio/x-jam"},
                                   {"jav", "text/plain"},
                                   {"java", "text/plain"},
                                   {"jcm", "application/x-java-commerce"},
                                   {"jfif", "image/jpeg"},
                                   {"jfif-tbnl", "image/jpeg"},
                                   {"jpe", "image/jpeg"},
                                   {"jpeg", "image/jpeg"},
                                   {"jpg", "image/jpeg"},
                                   {"jps", "image/x-jps"},
                                   {"js", "application/x-javascript"},
                                   {"jut", "image/jutvision"},
                                   {"kar", "audio/midi"},
                                   {"ksh", "application/x-ksh"},
                                   {"la", "audio/nspaudio"},
                                   {"lam", "audio/x-liveaudio"},
                                   {"latex", "application/x-latex"},
                                   {"lha", "application/lha"},
                                   {"lhx", "application/octet-stream"},
                                   {"list", "text/plain"},
                                   {"lma", "audio/nspaudio"},
                                   {"log", "text/plain"},
                                   {"lsp", "application/x-lisp"},
                                   {"lst", "text/plain"},
                                   {"lsx", "text/x-la-asf"},
                                   {"ltx", "application/x-latex"},
                                   {"lzh", "application/octet-stream"},
                                   {"lzx", "application/lzx"},
                                   {"m", "text/plain"},
                                   {"m1v", "video/mpeg"},
                                   {"m2a", "audio/mpeg"},
                                   {"m2v", "video/mpeg"},
                                   {"m3u", "audio/x-mpequrl"},
                                   {"man", "application/x-troff-man"},
                                   {"map", "application/x-navimap"},
                                   {"mar", "text/plain"},
                                   {"mbd", "application/mbedlet"},
                                   {"mc$", "application/x-magic-cap-package-1.0"},
                                   {"mcd", "application/mcad"},
                                   {"mcf", "image/vasa"},
                                   {"mcp", "application/netmc"},
                                   {"me", "application/x-troff-me"},
                                   {"mht", "message/rfc822"},
                                   {"mhtml", "message/rfc822"},
                                   {"mid", "audio/midi"},
                                   {"midi", "audio/midi"},
                                   {"mif", "application/x-frame"},
                                   {"mime", "message/rfc822"},
                                   {"mjf", "audio/x-vnd.audioexplosion.mjuicemediafile"},
                                   {"mjpg", "video/x-motion-jpeg"},
                                   {"mm", "application/base64"},
                                   {"mme", "application/base64"},
                                   {"mod", "audio/mod"},
                                   {"moov", "video/quicktime"},
                                   {"mov", "video/quicktime"},
                                   {"movie", "video/x-sgi-movie"},
                                   {"mp2", "audio/mpeg"},
                                   {"mp3", "audio/mpeg3"},
                                   {"mpa", "audio/mpeg"},
                                   {"mpc", "application/x-project"},
                                   {"mpe", "video/mpeg"},
                                   {"mpeg", "video/mpeg"},
                                   {"mpg", "video/mpeg"},
                                   {"mpga", "audio/mpeg"},
                                   {"mpp", "application/vnd.ms-project"},
                                   {"mpt", "application/x-project"},
                                   {"mpv", "application/x-project"},
                                   {"mpx", "application/x-project"},
                                   {"mrc", "application/marc"},
                                   {"ms", "application/x-troff-ms"},
                                   {"mv", "video/x-sgi-movie"},
                                   {"my", "audio/make"},
                                   {"mzz", "application/x-vnd.audioexplosion.mzz"},
                                   {"nap", "image/naplps"},
                                   {"naplps", "image/naplps"},
                                   {"nc", "application/x-netcdf"},
                                   {"ncm", "application/vnd.nokia.configuration-message"},
                                   {"nif", "image/x-niff"},
                                   {"niff", "image/x-niff"},
                                   {"nix", "application/x-mix-transfer"},
                                   {"nsc", "application/x-conference"},
                                   {"nvd", "application/x-navidoc"},
                                   {"o", "application/octet-stream"},
                                   {"oda", "application/oda"},
                                   {"omc", "application/x-omc"},
                                   {"omcd", "application/x-omcdatamaker"},
                                   {"omcr", "application/x-omcregerator"},
                                   {"p", "text/x-pascal"},
                                   {"p10", "application/pkcs10"},
                                   {"p12", "application/pkcs-12"},
                                   {"p7a", "application/x-pkcs7-signature"},
                                   {"p7c", "application/pkcs7-mime"},
                                   {"pas", "text/pascal"},
                                   {"pbm", "image/x-portable-bitmap"},
                                   {"pcl", "application/vnd.hp-pcl"},
                                   {"pct", "image/x-pict"},
                                   {"pcx", "image/x-pcx"},
                                   {"pdf", "application/pdf"},
                                   {"pfunk", "audio/make"},
                                   {"pgm", "image/x-portable-graymap"},
                                   {"pic", "image/pict"},
                                   {"pict", "image/pict"},
                                   {"pkg", "application/x-newton-compatible-pkg"},
                                   {"pko", "application/vnd.ms-pki.pko"},
                                   {"pl", "text/plain"},
                                   {"plx", "application/x-pixclscript"},
                                   {"pm", "image/x-xpixmap"},
                                   {"png", "image/png"},
                                   {"pnm", "application/x-portable-anymap"},
                                   {"pot", "application/mspowerpoint"},
                                   {"pov", "model/x-pov"},
                                   {"ppa", "application/vnd.ms-powerpoint"},
                                   {"ppm", "image/x-portable-pixmap"},
                                   {"pps", "application/mspowerpoint"},
                                   {"ppt", "application/mspowerpoint"},
                                   {"ppz", "application/mspowerpoint"},
                                   {"pre", "application/x-freelance"},
                                   {"prt", "application/pro_eng"},
                                   {"ps", "application/postscript"},
                                   {"psd", "application/octet-stream"},
                                   {"pvu", "paleovu/x-pv"},
                                   {"pwz", "application/vnd.ms-powerpoint"},
                                   {"py", "text/x-script.phyton"},
                                   {"pyc", "applicaiton/x-bytecode.python"},
                                   {"qcp", "audio/vnd.qcelp"},
                                   {"qd3", "x-world/x-3dmf"},
                                   {"qd3d", "x-world/x-3dmf"},
                                   {"qif", "image/x-quicktime"},
                                   {"qt", "video/quicktime"},
                                   {"qtc", "video/x-qtc"},
                                   {"qti", "image/x-quicktime"},
                                   {"qtif", "image/x-quicktime"},
                                   {"ra", "audio/x-pn-realaudio"},
                                   {"ram", "audio/x-pn-realaudio"},
                                   {"ras", "application/x-cmu-raster"},
                                   {"rast", "image/cmu-raster"},
                                   {"rexx", "text/x-script.rexx"},
                                   {"rf", "image/vnd.rn-realflash"},
                                   {"rgb", "image/x-rgb"},
                                   {"rm", "application/vnd.rn-realmedia"},
                                   {"rmi", "audio/mid"},
                                   {"rmm", "audio/x-pn-realaudio"},
                                   {"rmp", "audio/x-pn-realaudio"},
                                   {"rng", "application/ringing-tones"},
                                   {"rnx", "application/vnd.rn-realplayer"},
                                   {"roff", "application/x-troff"},
                                   {"rp", "image/vnd.rn-realpix"},
                                   {"rpm", "audio/x-pn-realaudio-plugin"},
                                   {"rt", "text/richtext"},
                                   {"rtf", "text/richtext"},
                                   {"rtx", "application/rtf"},
                                   {"rv", "video/vnd.rn-realvideo"},
                                   {"s", "text/x-asm"},
                                   {"s3m", "audio/s3m"},
                                   {"saveme", "application/octet-stream"},
                                   {"sbk", "application/x-tbook"},
                                   {"scm", "application/x-lotusscreencam"},
                                   {"sdml", "text/plain"},
                                   {"sdp", "application/sdp"},
                                   {"sdr", "application/sounder"},
                                   {"sea", "application/sea"},
                                   {"set", "application/set"},
                                   {"sgm", "text/sgml"},
                                   {"sgml", "text/sgml"},
                                   {"sh", "application/x-bsh"},
                                   {"shtml", "text/html"},
                                   {"sid", "audio/x-psid"},
                                   {"sit", "application/x-sit"},
                                   {"skd", "application/x-koan"},
                                   {"skm", "application/x-koan"},
                                   {"skp", "application/x-koan"},
                                   {"skt", "application/x-koan"},
                                   {"sl", "application/x-seelogo"},
                                   {"smi", "application/smil"},
                                   {"smil", "application/smil"},
                                   {"snd", "audio/basic"},
                                   {"sol", "application/solids"},
                                   {"spc", "application/x-pkcs7-certificates"},
                                   {"spl", "application/futuresplash"},
                                   {"spr", "application/x-sprite"},
                                   {"sprite", "application/x-sprite"},
                                   {"src", "application/x-wais-source"},
                                   {"ssi", "text/x-server-parsed-html"},
                                   {"ssm", "application/streamingmedia"},
                                   {"sst", "application/vnd.ms-pki.certstore"},
                                   {"step", "application/step"},
                                   {"stl", "application/sla"},
                                   {"stp", "application/step"},
                                   {"sv4cpio", "application/x-sv4cpio"},
                                   {"sv4crc", "application/x-sv4crc"},
                                   {"svf", "image/vnd.dwg"},
                                   {"svr", "application/x-world"},
                                   {"swf", "application/x-shockwave-flash"},
                                   {"t", "application/x-troff"},
                                   {"talk", "text/x-speech"},
                                   {"tar", "application/x-tar"},
                                   {"tbk", "application/toolbook"},
                                   {"tcl", "application/x-tcl"},
                                   {"tcsh", "text/x-script.tcsh"},
                                   {"tex", "application/x-tex"},
                                   {"texi", "application/x-texinfo"},
                                   {"texinfo", "application/x-texinfo"},
                                   {"text", "text/plain"},
                                   {"tgz", "application/x-compressed"},
                                   {"tif", "image/tiff"},
                                   {"tr", "application/x-troff"},
                                   {"tsi", "audio/tsp-audio"},
                                   {"tsp", "audio/tsplayer"},
                                   {"tsv", "text/tab-separated-values"},
                                   {"turbot", "image/florian"},
                                   {"txt", "text/plain"},
                                   {"uil", "text/x-uil"},
                                   {"uni", "text/uri-list"},
                                   {"unis", "text/uri-list"},
                                   {"unv", "application/i-deas"},
                                   {"uri", "text/uri-list"},
                                   {"uris", "text/uri-list"},
                                   {"ustar", "application/x-ustar"},
                                   {"uu", "application/octet-stream"},
                                   {"vcd", "application/x-cdlink"},
                                   {"vcs", "text/x-vcalendar"},
                                   {"vda", "application/vda"},
                                   {"vdo", "video/vdo"},
                                   {"vew", "application/groupwise"},
                                   {"viv", "video/vivo"},
                                   {"vivo", "video/vivo"},
                                   {"vmd", "application/vocaltec-media-desc"},
                                   {"vmf", "application/vocaltec-media-file"},
                                   {"voc", "audio/voc"},
                                   {"vos", "video/vosaic"},
                                   {"vox", "audio/voxware"},
                                   {"vqe", "audio/x-twinvq-plugin"},
                                   {"vqf", "audio/x-twinvq"},
                                   {"vql", "audio/x-twinvq-plugin"},
                                   {"vrml", "application/x-vrml"},
                                   {"vrt", "x-world/x-vrt"},
                                   {"vsd", "application/x-visio"},
                                   {"vst", "application/x-visio"},
                                   {"vsw", "application/x-visio"},
                                   {"w60", "application/wordperfect6.0"},
                                   {"w61", "application/wordperfect6.1"},
                                   {"w6w", "application/msword"},
                                   {"wav", "audio/wav"},
                                   {"wb1", "application/x-qpro"},
                                   {"wbmp", "image/vnd.wap.wbmp"},
                                   {"web", "application/vnd.xara"},
                                   {"wiz", "application/msword"},
                                   {"wk1", "application/x-123"},
                                   {"wmf", "windows/metafile"},
                                   {"wml", "text/vnd.wap.wml"},
                                   {"wmlc", "application/vnd.wap.wmlc"},
                                   {"wmls", "text/vnd.wap.wmlscript"},
                                   {"wmlsc", "application/vnd.wap.wmlscriptc"},
                                   {"word", "application/msword"},
                                   {"docx", "application/msword"},                                  
                                   {"wp", "application/wordperfect"},
                                   {"wp5", "application/wordperfect"},
                                   {"wp6", "application/wordperfect"},
                                   {"wpd", "application/wordperfect"},
                                   {"wq1", "application/x-lotus"},
                                   {"wri", "application/mswrite"},
                                   {"wrl", "application/x-world"},
                                   {"wrz", "model/vrml"},
                                   {"wsc", "text/scriplet"},
                                   {"wsrc", "application/x-wais-source"},
                                   {"wtk", "application/x-wintalk"},
                                   {"xbm", "image/x-xbitmap"},
                                   {"xdr", "video/x-amt-demorun"},
                                   {"xgz", "xgl/drawing"},
                                   {"xif", "image/vnd.xiff"},
                                   {"xl", "application/excel"},
                                   {"xla", "application/excel"},
                                   {"xlb", "application/excel"},
                                   {"xlc", "application/excel"},
                                   {"xld", "application/excel"},
                                   {"xlk", "application/excel"},
                                   {"xll", "application/excel"},
                                   {"xlm", "application/excel"},
                                   {"xls", "application/excel"},
                                   {"xlt", "application/excel"},
                                   {"xlv", "application/excel"},
                                   {"xlw", "application/excel"},
                                   {"xm", "audio/xm"},
                                   {"xml", "text/xml"},
                                   {"xmz", "xgl/movie"},
                                   {"xpix", "application/x-vnd.ls-xpix"},
                                   {"xpm", "image/x-xpixmap"},
                                   {"x-png", "image/png"},
                                   {"xsr", "video/x-amt-showrun"},
                                   {"xwd", "image/x-xwd"},
                                   {"xyz", "chemical/x-pdb"},
                                   {"z", "application/x-compress"},
                                   {"zip", "application/x-compressed"},
                                   {"zoo", "application/octet-stream"},
                                   {"zsh", "text/x-script.zsh"}

                               };
    }
    //get content type of a file -Neethu 5/27/2014
    public static string GetContentType(string fileName)
    {
        if (contentTypes == null || !(contentTypes.Count > 0))
        {
            InitializeMimeTypes();
        }

        string extension;
        var fi = new FileInfo(fileName);
        extension = fi.Extension.Replace(".", "");
        string contentType;
        contentTypes.TryGetValue(extension.ToLower(), out contentType);
        if (String.IsNullOrEmpty(contentType))
        {
            contentType = "application/octet-stream";
        }

        return contentType;
    }


    public int saveDoc(int SchoolId, int StudentId, int UserId, int lookupId1)
    {
        clsData objData = new clsData();
        int DocId = 0;
        try
        {

            string query = "INSERT INTO Document(SchoolId,StudentPersonalId,DocumentType,DocumentName,Other,UserType,Status,CreatedBy,CreatedOn) VALUES (@SchoolId,@StudentPersonalId,@DocumentType,@DocumentName,@Other,@UserType,@Status,@CreatedBy,@CreatedOn) \nSELECT SCOPE_IDENTITY()";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = objData.Open();
                cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                cmd.Parameters.AddWithValue("@StudentPersonalId", StudentId);
                cmd.Parameters.AddWithValue("@DocumentType", lookupId1);
                cmd.Parameters.AddWithValue("@DocumentName", "IEP");
                cmd.Parameters.AddWithValue("@Other", "IEP");
                cmd.Parameters.AddWithValue("@UserType", "Staff");
                cmd.Parameters.AddWithValue("@Status", true);
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                DocId = Convert.ToInt16(cmd.ExecuteScalar());
            }

        }
        catch
        {

        }
        return DocId;
    }

    //Function used for Saving Document as Binaryfile -Neethu 5/27/2014
    public int saveDocument(byte[] contents, string FileName, string Ver, string Type, int DocId, string Module, int SchoolId, int StudentId, int UserId)
    {
        clsData objData = new clsData();
        string contentType = "";
        contentType = "application/msword";
        int BinaryId = 0;
        try
        {
            string query = "Insert into binaryFiles(SchoolId,StudentId,DocId,DocumentName,ContentType,Data,type,ModuleName,VersionNo,Varified,CreatedBy,CreatedOn) values (@SchoolId,@StudentId,@DocId,@DocumentName,@ContentType,@Data,@type,@ModuleName,@VersionNo,@Varified,@CreatedBy,@CreatedOn) \nSELECT SCOPE_IDENTITY()";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = objData.Open();
                cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                cmd.Parameters.AddWithValue("@StudentId", StudentId);
                cmd.Parameters.AddWithValue("@DocId", DocId);
                cmd.Parameters.AddWithValue("@DocumentName", FileName);
                cmd.Parameters.AddWithValue("@ContentType", contentType);
                cmd.Parameters.AddWithValue("@Data", contents);
                cmd.Parameters.AddWithValue("@type", Type);
                cmd.Parameters.AddWithValue("@ModuleName", Module);
                cmd.Parameters.AddWithValue("@VersionNo", Ver);
                cmd.Parameters.AddWithValue("@Varified", true);
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                BinaryId = Convert.ToInt16(cmd.ExecuteScalar());

            }


        }
        catch
        {
        }

        return BinaryId;
    }


    public int saveDocument(byte[] contents, string FileName, string Ver, string Type, int DocId, int iepid, string Module, int SchoolId, int StudentId, int UserId)
    {
        clsData objData = new clsData();
        string contentType = "";
        contentType = "application/msword";
        int BinaryId = 0;
        try
        {
            string query = "Insert into binaryFiles(SchoolId,StudentId,DocId,IEPId,DocumentName,ContentType,Data,type,ModuleName,VersionNo,Varified,CreatedBy,CreatedOn) values (@SchoolId,@StudentId,@DocId,@IEPId,@DocumentName,@ContentType,@Data,@type,@ModuleName,@VersionNo,@Varified,@CreatedBy,@CreatedOn) \nSELECT SCOPE_IDENTITY()";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = objData.Open();
                cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                cmd.Parameters.AddWithValue("@StudentId", StudentId);
                cmd.Parameters.AddWithValue("@DocId", DocId);
                cmd.Parameters.AddWithValue("@IEPId", iepid);
                cmd.Parameters.AddWithValue("@DocumentName", FileName);
                cmd.Parameters.AddWithValue("@ContentType", contentType);
                cmd.Parameters.AddWithValue("@Data", contents);
                cmd.Parameters.AddWithValue("@type", Type);
                cmd.Parameters.AddWithValue("@ModuleName", Module);
                cmd.Parameters.AddWithValue("@VersionNo", Ver);
                cmd.Parameters.AddWithValue("@Varified", true);
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                BinaryId = Convert.ToInt16(cmd.ExecuteScalar());

            }


        }
        catch
        {
        }

        return BinaryId;
    }

    public int UpdateDocument(byte[] contents, int DocId, int iepid, int SchoolId, int StudentId)
    {
        clsData objData = new clsData();
        int BinaryId = 0;
        try
        {
            string query = "update binaryFiles set Data=@Data where SchoolId=@SchoolId and StudentId=@StudentId and IEPId=@IEPId";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = objData.Open();
                cmd.Parameters.AddWithValue("@SchoolId", SchoolId);
                cmd.Parameters.AddWithValue("@StudentId", StudentId);
                cmd.Parameters.AddWithValue("@IEPId", iepid);
                cmd.Parameters.AddWithValue("@Data", contents);
                BinaryId = Convert.ToInt16(cmd.ExecuteScalar());

            }


        }
        catch
        {
        }

        return BinaryId;
    }

    public static byte[] ReadFile(string filePath)
    {
        byte[] buffer;
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        try
        {
            int length = (int)fileStream.Length;  // get file length
            buffer = new byte[length];            // create buffer
            int count;                            // actual number of bytes read
            int sum = 0;                          // total number of bytes read

            // read until Read method returns 0 (end of the stream has been reached)
            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                sum += count;  // sum is a buffer offset for next reading
        }
        finally
        {
            fileStream.Close();
        }
        return buffer;
    }

    public static byte[] ConvertToBinary(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetBytes(str);
    }
    public static string ConvertToString(byte[] ar)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetString(ar);
    }

    public void ShowDocument(string fileName, byte[] fileContent, string ContentType)
    {
        try
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = ContentType;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(fileContent);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        catch
        {
        }
        finally
        {
        }
    }
    //Function used for get Binary Document -Neethu 5/27/2014
    public byte[] getDocument(string strQuery, out string ContentType, out string FileName)
    {

        clsData objData = new clsData();
        byte[] bytes = null;
        FileName = "";
        ContentType = "";

        SqlDataReader dr = objData.ReturnDataReader(strQuery, false);
        try
        {
            if (dr.Read())
            {
                bytes = (Byte[])dr["Data"];
                ContentType = dr["ContentType"].ToString();
                FileName = dr["DocumentName"].ToString();
            }

            dr.Close();
        }
        catch (Exception exp)
        {
            dr.Close();
        }
        return bytes;



    }

    #endregion



}
