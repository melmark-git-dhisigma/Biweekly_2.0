using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Hosting;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.IO;
using System.Threading;


public partial class repository_manag : System.Web.UI.Page
{
    cls_data oData = new cls_data();
    clsGeneral oGeneral = new clsGeneral();
    clsSession oSession = null;
    clsSession sess = null;
    public HttpPostedFile file;
    private static Action delegate_Search;
    protected void Page_Load(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];
        if (oSession != null)
        {
            Session["UserID"] = oSession.LoginId;
            lblLoginName.Text = oSession.UserName;
        }



        if (!IsPostBack)
        {
            setTitle();

            if (Session["type"] == null || Session["type"].ToString() == "")
            {
                repository_type.Value = "images";
                Session["filetype"] = "images";
                Session["type"] = "images";
            }
            getSearchData(Session["type"].ToString());

            repository_repCurrId.Value = getCurrId();

        }
        else
        {
            repository_repCurrId.Value = getCurrId();
            // int milliseconds = 5000;
            //System.Threading.Thread.Sleep(milliseconds);
        }

        lbl_type.Text = repository_type.Value.ToUpper();
        if (lbl_type.Text == "images") lbl_type.Text = "IMAGES";

        if (Request.Files.Count > 0)
        {
            //file = Request.Files[0];
            //Session["PostedFile"] = file;
            if (Directory.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails")) == false)
            {
                Directory.CreateDirectory(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails"));
            }
            string type = Session["type"].ToString();
            if (Directory.Exists(Server.MapPath("~\\VisualTool\\Repository\\" + type)) == false)
            {
                Directory.CreateDirectory(Server.MapPath("~\\VisualTool\\Repository\\" + type));
            }
            if (Directory.Exists(Server.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp")) == false)
            {
                Directory.CreateDirectory(Server.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp"));
            }
            string extension = Path.GetExtension(Request.Files[0].FileName);
            string tempFilename = "";
            if (type == "images")
                tempFilename = "temp_Image_" + Session["UserID"] + extension;
            else if (type == "reinforcement")
                tempFilename = "tempSWF_" + Session["UserID"] + extension;
            else if (type == "videos")
                tempFilename = "tempVideo_" + Session["UserID"] + extension;
            else if (type == "audios")
            {
                if (extension == "")
                    extension = ".mp3";
                tempFilename = "tempAudio_" + Session["UserID"] + extension;
                //rcrd_filename.InnerHtml = Request.Files[0].FileName;
            }


            //string thumbName = "img_thumb_" + Session["UserID"] + ".jpg";
            //string fileName = "img_filename_" + Session["UserID"] + extension;
            CheckFile("~\\VisualTool\\Repository\\" + type + "\\temp\\" + tempFilename);
            Request.Files[0].SaveAs(Server.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp\\" + tempFilename));


            Session["tempPath"] = "~\\VisualTool\\Repository\\" + type + "\\temp\\";
            Session["tempFilename"] = tempFilename;
        }
    }


    private void setTitle()
    {
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            object obj = objData.FetchValue("Select SchoolDesc from School Where SchoolId=" + sess.SchoolId + "");
            if (obj != null)
            {
                TitleName.InnerText = obj.ToString();


            }
        }
    }



    public string getCurrId()
    {
        try
        {
            string query = "select IDENT_CURRENT('LE_Media') as value ";
            string[] argument = new string[6];
            argument[0] = "";
            argument[1] = "";
            argument[2] = "";
            argument[3] = "";
            argument[4] = "0";
            argument[5] = "";

            DataTable dt = oData.getMediaItems(query, argument);
            if (dt.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dt.Rows[0]["value"].ToString());
                return (++id).ToString();
            }
            else
            {
                return "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void nonAcc_menu_Click(object sender, BulletedListEventArgs e)
    {


    }
    protected void opt_search_Click(object sender, ImageClickEventArgs e)
    {
        getSearchData(repository_type.Value);
    }

    void getSearchData(string type)
    {
        try
        {
            string query = "";
            if (txt_search.Text != "")
            {
                query = "select * from LE_Media where name like '%'+@name+'%' and Type=@type";
            }
            else
            {
                query = "select * from LE_Media where Type=@type";
            }
            string[] argument = new string[6];

            argument[0] = clsGeneral.convertQuotes(txt_search.Text);
            argument[1] = "";
            argument[2] = "";
            argument[3] = type;
            argument[4] = "0";
            argument[5] = "";

            DataTable dt = oData.getMediaItems(query, argument);

            if (dt.Rows.Count > 0)
            {
                dt_mediaList.Visible = true;
                lbl_message.Visible = false;
                dt_mediaList.DataSource = dt;
                dt_mediaList.DataBind();
            }
            else
            {
                dt_mediaList.Visible = false;
                lbl_message.Visible = true;
                lbl_message.Text = "No Data Found";
            }

            for (int i = 0; i < nonAcc_menu.Items.Count; i++)
            {
                if (nonAcc_menu.Items[i].Value == type)
                {
                    nonAcc_menu.Items[i].Attributes.Add("class", "selectedItem");
                }
                else
                {
                    nonAcc_menu.Items[i].Attributes.Add("class", "unselectedItem");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void opt_trash_Click(object sender, ImageClickEventArgs e)
    {

        foreach (DataListItem li in dt_mediaList.Items)
        {
            HtmlInputCheckBox cb = li.FindControl("delete") as HtmlInputCheckBox;
            Label mediaId = (Label)li.FindControl("lbl_mediaId");
            HiddenField hfmediaId = (HiddenField)li.FindControl("hf_mediaId");
            if (cb != null)
            {
                if (cb.Checked)
                {
                    deleteItems(hfmediaId.Value);
                }
            }
        }

        //upPnl_dtMediaList.Update();
        getSearchData(repository_type.Value);
    }

    private void deleteItems(string toDeleteIds)
    {
        try
        {
            string query = "delete from LE_Media where MediaId = @mediaId";

            string[] argument = new string[7];

            argument[0] = "";
            argument[1] = "";
            argument[2] = "";
            argument[3] = "";
            argument[4] = toDeleteIds;
            argument[5] = "";
            argument[6] = "";

            DataClass data = new DataClass();
            string selMediapath = data.ExecuteScalarString("SELECT Path FROM LE_Media WHERE MediaId=" + toDeleteIds);
            string thumbPath = data.ExecuteScalarString("SELECT Thumbnail FROM LE_Media WHERE MediaId=" + toDeleteIds);

            int i = oData.saveMediaElements(query, argument);


            string type = Session["type"].ToString();

            // not allowing the files to delete. Since there may be chances that the files are used in any lessons.


            //if (i == 0)
            //{
            //    if (File.Exists(Server.MapPath(selMediapath)))
            //    {
            //        File.Delete(Server.MapPath(selMediapath));
            //    }
            //    if (File.Exists(Server.MapPath(thumbPath)))
            //    {
            //        File.Delete(Server.MapPath(thumbPath));
            //    }
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string setSelThumbNail(string src)
    {
        try
        {
            HttpContext.Current.Session["thumb_Path"] = src.Replace("/", "\\");
            return "success";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string SaveData(string name, string description, string keyword, string sType)
    {
        cls_data oData = new cls_data();
        //HttpContext.Current.Session["killThread"] = "kill";
        //HttpContext.Current.Response.Cookies["killThread"].Value = "kill";
        string type = sType;//HttpContext.Current.Session["type"].ToString();
        if (Directory.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp")) == false)
        {
            Directory.CreateDirectory(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp"));
        }
        if (type == "videos")
        {
            try
            {

                string tempPath = HostingEnvironment.MapPath(HttpContext.Current.Session["tempPath"].ToString());
                string tempFilename = HttpContext.Current.Session["tempFilename"].ToString();

                string thumb_Path = HttpContext.Current.Session["thumb_Path"].ToString();
                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];
                string fileName = "fileName_" + HttpContext.Current.Session["UserID"];
                string thumbName = "userthumb_" + HttpContext.Current.Session["UserID"] + ".jpg";
                //System.IO.File.Move(Server.MapPath(thumb_Path), Server.MapPath("~\\VisualTool\\Repository\\thumbnails\\" + 1));
                imageResizer imgRzr = new imageResizer();
                thumbName = imgRzr.deleteUnwantedThumbnail(HostingEnvironment.MapPath("~\\VisualTool\\" + thumb_Path), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails\\"), thumbName);

                videoConverter vConvrt = new videoConverter();
                int result = vConvrt.videoFormtConverter(HostingEnvironment.MapPath("~/bin/ffmpeg.exe"), tempPath + tempFilename, HostingEnvironment.MapPath("~\\VisualTool\\Repository\\videos\\"), fileName, 3, 0, 0);
                if (result == 0)
                {
                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + HttpContext.Current.Session["type"].ToString() + "/" + fileName + ".flv";
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/Repository/thumbnails/" + thumbName;

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], ".flv");
                }
                else
                {
                    // video convertion failed...insertion not possible.....
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (type == "images")
        {
            try
            {
                imageResizer imgRzr = new imageResizer();

                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];

                string tempPath = HostingEnvironment.MapPath(HttpContext.Current.Session["tempPath"].ToString());
                string tempFilename = HttpContext.Current.Session["tempFilename"].ToString();


                string extension = Path.GetExtension(tempFilename);

                string thumbName = "img_thumb_" + HttpContext.Current.Session["UserID"] + ".jpg";
                string fileName = "img_filename_" + HttpContext.Current.Session["UserID"] + extension;
                //CheckFile("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename);
                //fileupload.SaveAs(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename));
                int result = imgRzr.ScaleImage(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails\\" + thumbName), 100);
                //File.Delete(Server.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + "tempImage.jpg")); // After Scaling delete the temporary image.....
                if (result == 1)
                {
                    //if (File.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\" + fileName)))
                    //{
                    //    File.Delete(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\" + fileName));
                    //}
                    CheckFile("~\\VisualTool\\Repository\\images\\" + fileName);
                    File.Move(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\" + fileName));
                    //fileupload.SaveAs(Server.MapPath("~\\VisualTool\\Repository\\images\\" + "filename.jpg"));
                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + type + "/" + fileName;
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/Repository/thumbnails/" + thumbName;

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], extension);
                    //string updqry = "update LE_Media set Path='~/Phase002.1/Repository/" + type + "/" + data.ToString() + ".jpg' ,Thumbnail='~/VisualTool/Repository/thumbnails/" + data.ToString() + ".jpg' where MediaId=" + data;
                    //DataClass odata = new DataClass();
                    //odata.ExecuteNonQuery(updqry);
                    //File.Move(HostingEnvironment.MapPath(argument[2]), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + data.ToString() + ".jpg"));
                    //File.Move(HostingEnvironment.MapPath(argument[6]), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails\\" + data.ToString() + ".jpg"));
                }
                else
                {
                    // image resizing failed......so insertion not possible.....
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (type == "reinforcement")
        {
            try
            {
                imageResizer imgRzr = new imageResizer();

                string tempPath = HostingEnvironment.MapPath(HttpContext.Current.Session["tempPath"].ToString());
                string tempFilename = HttpContext.Current.Session["tempFilename"].ToString();

                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];
                string extension = Path.GetExtension(tempFilename);
                //string tempName = "tempSWF_" + HttpContext.Current.Session["UserID"] + extension;
                string thumbName = "swfThumb_" + HttpContext.Current.Session["UserID"];
                string fileName = "filename_" + HttpContext.Current.Session["UserID"] + extension;
                //CheckFile("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempName);
                //fileupload.SaveAs(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempName));
                //int result = imgRzr.swfThumbnilGenrator(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempFilename), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails"), thumbName, HostingEnvironment.MapPath("~\\VisualTool\\images\\f.PNG"), 50);
                int result = 0;
                if (result == 0)
                {
                    CheckFile("~\\VisualTool\\Repository\\reinforcement\\" + fileName);
                    File.Move(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempFilename), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\reinforcement\\" + fileName));
                    //fileupload.SaveAs(Server.MapPath("~\\VisualTool\\Repository\\reinforcement"));
                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + type + "/" + fileName;
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/icons/flashicon.png";

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], extension);
                }
                else
                {
                    // thumbnail generation failed......so insertion not possible.....
                    if (File.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempFilename)))
                        File.Delete(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempFilename));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (type == "audios")
        {
            try
            {
                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];

                string tempPath = HostingEnvironment.MapPath(HttpContext.Current.Session["tempPath"].ToString());
                string tempFilename = HttpContext.Current.Session["tempFilename"].ToString();

                string fileName = "filename" + HttpContext.Current.Session["UserID"] + ".mp3";
                //int result = imgRzr.swfThumbnilGenrator(Server.MapPath(fileupload.FileName), Server.MapPath("~\\VisualTool\\Repository\\thumbnails"), "swfThumb", "", 50);
                //CheckFile("~\\VisualTool\\Repository\\" + type + "\\" + fileName);
                //fileupload.SaveAs(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + fileName));
                int result = 0;
                if (result == 0)
                {
                    CheckFile("~\\VisualTool\\Repository\\" + type + "\\" + fileName);
                    File.Move(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp\\" + tempFilename), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + fileName));

                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + type + "/" + fileName;
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/icons/audio (2).png";

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], ".mp3");
                }
                else
                {
                    // thumbnail generation failed......so insertion not possible.....
                    if (File.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp\\" + tempFilename)))
                        File.Delete(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp\\" + tempFilename));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //PointDelegate(type);    // Call the function to point the delegate to the non static function getsearchdata....
        //delegate_Search();      // Call the delegate which points to the function getsearchdata...
        return "Success";
    }

    public static void CheckFile(string path)
    {
        if (File.Exists(HostingEnvironment.MapPath(path)))
        {
            File.Delete(HostingEnvironment.MapPath(path));
        }
    }
    public static void UpdateFileName(string type, int ID, string filePath, string thumbPath, string extension)
    {
        try
        {
            string updqry = "";
            if ((type != "audios") && (type != "reinforcement"))
                updqry = "update LE_Media set Path='~/VisualTool/Repository/" + type + "/" + ID.ToString() + extension + "' ,Thumbnail='~/VisualTool/Repository/thumbnails/" + ID.ToString() + ".jpg' where MediaId=" + ID;
            else
                updqry = "update LE_Media set Path='~/VisualTool/Repository/" + type + "/" + ID.ToString() + extension + "' where MediaId=" + ID;
            DataClass odata = new DataClass();
            odata.ExecuteNonQuery(updqry);
            //if (File.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + ID.ToString() + extension)))
            //{
            //    File.Delete(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + ID.ToString() + extension));
            //}
            CheckFile("~\\VisualTool\\Repository\\" + type + "\\" + ID.ToString() + extension);
            File.Move(HostingEnvironment.MapPath(filePath), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + ID.ToString() + extension));
            if ((type != "audios") && (type != "reinforcement"))
            {
                CheckFile("~\\VisualTool\\Repository\\thumbnails\\" + ID.ToString() + ".jpg");
                File.Move(HostingEnvironment.MapPath(thumbPath), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails\\" + ID.ToString() + ".jpg"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void upPnl_dtMediaList_Load(object sender, EventArgs e)
    {
        base.OnLoad(e);
        string parameter = Request["__EVENTARGUMENT"];
        if (parameter == "uploaded")
        {


            getSearchData(repository_type.Value);
        }

    }

    protected void nonAcc_menu_Click1(object sender, BulletedListEventArgs e)
    {
        try
        {
            Session["type"] = nonAcc_menu.Items[e.Index].Value;
            for (int i = 0; i < nonAcc_menu.Items.Count; i++)
            {
                nonAcc_menu.Items[i].Attributes.Add("class", "unselectedItem");
            }
            nonAcc_menu.Items[e.Index].Attributes.Add("class", "selectedItem");
            switch (nonAcc_menu.Items[e.Index].Value)
            {
                case "images": repository_type.Value = "images";
                    lbl_type.Text = "IMAGES";
                    break;
                case "videos": repository_type.Value = "videos";
                    lbl_type.Text = "VIDEOS";
                    break;
                case "audios": repository_type.Value = "audios";
                    lbl_type.Text = "AUDIOS";
                    break;
                case "reinforcement": repository_type.Value = "reinforcement";
                    lbl_type.Text = "REINFORCEMENT";
                    break;
            }

            getSearchData(Session["type"].ToString());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string UploadVid()
    {
        try
        {
            if (Directory.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\videos\\temp")) == false)
            {
                Directory.CreateDirectory(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\videos\\temp"));
            }
            imageResizer imgRzr = new imageResizer();
            clsSession oSession = new clsSession();
            cls_data oData = new cls_data();
            int id;
            string query = "select IDENT_CURRENT('LE_Media') as value ";
            string[] argument = new string[6];
            argument[0] = "";
            argument[1] = "";
            argument[2] = "";
            argument[3] = "";
            argument[4] = "0";
            argument[5] = "";

            DataTable dt = oData.getMediaItems(query, argument);
            if (dt.Rows.Count > 0)
            {
                id = Convert.ToInt32(dt.Rows[0]["value"].ToString());
                id = (++id);
            }

            else
            {
                return "";
            }

            //HttpPostedFile fu = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];

            string tempPath = HostingEnvironment.MapPath(HttpContext.Current.Session["tempPath"].ToString());
            string tempFilename = HttpContext.Current.Session["tempFilename"].ToString();
            string typeName = HttpContext.Current.Session["type"].ToString();
            //if (fu.HasFile)
            //{
            string[] extesion = tempFilename.Split('.');
            //string tempPath = HostingEnvironment.MapPath("~/VisualTool/Repository/" + HttpContext.Current.Request.Cookies["type"].Value + "/temp/" + HttpContext.Current.Session["UserID"].ToString() + "." + extesion[extesion.Length - 1]);
            HttpContext.Current.Session["temp_VideoPath"] = tempPath;
            //CheckFile("~/VisualTool/Repository/" + HttpContext.Current.Request.Cookies["type"].Value + "/temp/" + HttpContext.Current.Session["UserID"].ToString() + "." + extesion[extesion.Length - 1]);
            //fu.SaveAs(tempPath);
            string thumbnailpaths = imgRzr.videoThumbnailGenrator(HostingEnvironment.MapPath("~/bin/ffmpeg.exe"), tempPath + tempFilename, "temp/" + "Thumb_", HostingEnvironment.MapPath("~/VisualTool/Repository/" + HttpContext.Current.Session["type"].ToString()), 100, Convert.ToInt32(HttpContext.Current.Session["UserID"]));
            if (thumbnailpaths.Length > 0)
            {

                //NonStaticDelegate();

                HttpContext.Current.Session["thumbnailFolder"] = Directory.GetParent(HostingEnvironment.MapPath("~/VisualTool/Repository/" + HttpContext.Current.Session["type"].ToString() + "/" + thumbnailpaths.Split('*')[0]));
                HttpContext.Current.Session["thumPath"] = HttpContext.Current.Session["thumbnailFolder"].ToString() + "*" + HttpContext.Current.Session["temp_VideoPath"].ToString();
                
                //HttpContext.Current.Response.Cookies["thumbnailFolder"].Value = HttpContext.Current.Session["thumbnailFolder"].ToString();
                //HttpContext.Current.Response.Cookies["thumPath"].Value = HttpContext.Current.Session["thumPath"].ToString();

                //CaptureDelegate(HttpContext.Current);

                //Thread TdCheckBrowserClose = new Thread(new ThreadStart(NonStaticDelegate));
                //TdCheckBrowserClose.Start();
            }
            return thumbnailpaths;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //}
        //else
        //    return "";
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void DeleteIfCancel()
    {
        try
        {
            string type = HttpContext.Current.Session["type"].ToString();
            if (type == "videos")
            {
                string tempPath = HostingEnvironment.MapPath(HttpContext.Current.Session["tempPath"].ToString());
                string tempFilename = HttpContext.Current.Session["tempFilename"].ToString();
                if (File.Exists(tempPath + tempFilename))
                {
                    File.Delete(tempPath + tempFilename);
                }

                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];

                //if (HttpContext.Current.Request.Cookies["thumb_Path"] != null)
                //{
                //    string thumb_Path = HttpContext.Current.Request.Cookies["thumb_Path"].Value;


                //}
                if (HttpContext.Current.Session["thumbnailFolder"] != null)
                {
                    DirectoryInfo dirInfo = (DirectoryInfo)HttpContext.Current.Session["thumbnailFolder"];
                    dirInfo.Delete(true);
                    File.Delete(HttpContext.Current.Session["temp_VideoPath"].ToString());  // Delete the video saved in the temp folder.....
                }

            }
            else
            {
                string tempPath = HostingEnvironment.MapPath(HttpContext.Current.Session["tempPath"].ToString());
                string tempFilename = HttpContext.Current.Session["tempFilename"].ToString();
                if (File.Exists(tempPath + tempFilename))
                {
                    File.Delete(tempPath + tempFilename);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetItemsData(int id)
    {
        string val = "";
        clsData oData = new clsData();
        string query = "SELECT Name + '*' + Keyword as Data FROM LE_Media WHERE MediaId=" + id;
        //query = "UPDATE LE_Media SET Name='" + name + "',Keyword='" + keyword + "' WHERE MediaId=" + id;
        object value = oData.FetchValue(query);
        if (value != null)
            val = value.ToString();
        return val;
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static int UpdateItemData(int id, string name, string keyword)
    {
        int result = 0;
        clsData oData = new clsData();
        string query = "UPDATE LE_Media SET Name='" + name + "',Keyword='" + keyword + "' WHERE MediaId=" + id;

        result = oData.Execute(query);
        return result;
    }
    //[System.Web.Services.WebMethod(EnableSession = true)]
    //public static void closeEvent()
    //{
    //    HttpContext.Current.Session["thumPath"] = null;
    //    HttpContext.Current.Response.Cookies["thumPath"].Value = null;
    //}
    /// <summary>
    /// this function called as thread..
    /// </summary>

    //private void browserCloseEvents(HttpContext asp)
    //{
    //    string[] delPath = (Session["thumPath"].ToString()).Split('*');
    //    while (true)
    //    {
    //        if (Session["killThread"] != null)
    //        {
    //            Session["killThread"] = null;
    //            break;
    //        }
    //        if (Session["thumPath"] == null)
    //        {
    //            if (Directory.Exists(delPath[0]))
    //            {
    //                Directory.Delete(delPath[0], true);
    //            }
    //            if (File.Exists(delPath[1]))
    //            {
    //                File.Delete(delPath[1]);
    //            }
    //            break;
    //        }


    //        Thread.Sleep(3000);
    //    }
    //}

    public static void PointDelegate(string type)
    {
        repository_manag temp = new repository_manag();
        repository_manag.delegate_Search = new Action(() => temp.getSearchData(type));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string name = txt_fName.Text; string description = txt_descriptions.Text; string keyword = txt_keywords.Text;

        cls_data oData = new cls_data();
        //HttpContext.Current.Session["killThread"] = "kill";
        //HttpContext.Current.Response.Cookies["killThread"].Value = "kill";
        string type = Session["type"].ToString();
        if (Directory.Exists(Server.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp")) == false)
        {
            Directory.CreateDirectory(Server.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp"));
        }
        if (type == "videos")
        {
            try
            {

                string tempPath = Server.MapPath(Session["tempPath"].ToString());
                string tempFilename = Session["tempFilename"].ToString();

                string thumb_Path = repository_thumb_Path.Value;
                Session["thumb_Path"] = thumb_Path;
                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];
                string fileName = "fileName_" + Session["UserID"];
                string thumbName = "userthumb_" + Session["UserID"] + ".jpg";
                //System.IO.File.Move(Server.MapPath(thumb_Path), Server.MapPath("~\\VisualTool\\Repository\\thumbnails\\" + 1));
                imageResizer imgRzr = new imageResizer();
                thumbName = imgRzr.deleteUnwantedThumbnail(Server.MapPath("~\\VisualTool\\" + thumb_Path), Server.MapPath("~\\VisualTool\\Repository\\thumbnails\\"), thumbName);

                videoConverter vConvrt = new videoConverter();
                int result = vConvrt.videoFormtConverter(Server.MapPath("~/bin/ffmpeg.exe"), tempPath + tempFilename, Server.MapPath("~\\VisualTool\\Repository\\videos\\"), fileName, 3, 0, 0);
                if (result == 0)
                {
                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + repository_type.Value + "/" + fileName + ".flv";
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/Repository/thumbnails/" + thumbName;

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], ".flv");
                }
                else
                {
                    // video convertion failed...insertion not possible.....
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (type == "images")
        {
            try
            {
                imageResizer imgRzr = new imageResizer();

                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];

                string tempPath = Server.MapPath(Session["tempPath"].ToString());
                string tempFilename = Session["tempFilename"].ToString();


                string extension = Path.GetExtension(tempFilename);

                string thumbName = "img_thumb_" + Session["UserID"] + ".jpg";
                string fileName = "img_filename_" + Session["UserID"] + extension;
                //CheckFile("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename);
                //fileupload.SaveAs(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename));
                int result = imgRzr.ScaleImage(Server.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename), Server.MapPath("~\\VisualTool\\Repository\\thumbnails\\" + thumbName), 100);
                //File.Delete(Server.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + "tempImage.jpg")); // After Scaling delete the temporary image.....
                if (result == 1)
                {
                    //if (File.Exists(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\" + fileName)))
                    //{
                    //    File.Delete(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\images\\" + fileName));
                    //}
                    CheckFile("~\\VisualTool\\Repository\\images\\" + fileName);
                    File.Move(Server.MapPath("~\\VisualTool\\Repository\\images\\temp\\" + tempFilename), Server.MapPath("~\\VisualTool\\Repository\\images\\" + fileName));
                    //fileupload.SaveAs(Server.MapPath("~\\VisualTool\\Repository\\images\\" + "filename.jpg"));
                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + type + "/" + fileName;
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/Repository/thumbnails/" + thumbName;

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], extension);
                    //string updqry = "update LE_Media set Path='~/Phase002.1/Repository/" + type + "/" + data.ToString() + ".jpg' ,Thumbnail='~/VisualTool/Repository/thumbnails/" + data.ToString() + ".jpg' where MediaId=" + data;
                    //DataClass odata = new DataClass();
                    //odata.ExecuteNonQuery(updqry);
                    //File.Move(HostingEnvironment.MapPath(argument[2]), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + data.ToString() + ".jpg"));
                    //File.Move(HostingEnvironment.MapPath(argument[6]), HostingEnvironment.MapPath("~\\VisualTool\\Repository\\thumbnails\\" + data.ToString() + ".jpg"));
                }
                else
                {
                    // image resizing failed......so insertion not possible.....
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (type == "reinforcement")
        {
            try
            {
                imageResizer imgRzr = new imageResizer();

                string tempPath = Server.MapPath(Session["tempPath"].ToString());
                string tempFilename = Session["tempFilename"].ToString();

                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];
                string extension = Path.GetExtension(tempFilename);
                //string tempName = "tempSWF_" + HttpContext.Current.Session["UserID"] + extension;
                string thumbName = "swfThumb_" + Session["UserID"];
                string fileName = "filename_" + Session["UserID"] + extension;
                //CheckFile("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempName);
                //fileupload.SaveAs(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempName));
                int result = imgRzr.swfThumbnilGenrator(Server.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempFilename), Server.MapPath("~\\VisualTool\\Repository\\thumbnails"), thumbName, Server.MapPath("~\\VisualTool\\images\\f.png"), 50);
                if (result == 0)
                {
                    CheckFile("~\\VisualTool\\Repository\\reinforcement\\" + fileName);
                    File.Move(Server.MapPath("~\\VisualTool\\Repository\\reinforcement\\temp\\" + tempFilename), Server.MapPath("~\\VisualTool\\Repository\\reinforcement\\" + fileName));
                    //fileupload.SaveAs(Server.MapPath("~\\VisualTool\\Repository\\reinforcement"));
                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + type + "/" + fileName;
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/Repository/thumbnails/" + thumbName + ".jpg";

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], extension);
                }
                else
                {
                    // thumbnail generation failed......so insertion not possible.....
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else if (type == "audios")
        {
            try
            {
                //HttpPostedFile fileupload = (HttpPostedFile)HttpContext.Current.Session["PostedFile"];

                string tempPath = Server.MapPath(Session["tempPath"].ToString());
                string tempFilename = Session["tempFilename"].ToString();

                string fileName = "filename" + Session["UserID"] + ".mp3";
                //int result = imgRzr.swfThumbnilGenrator(Server.MapPath(fileupload.FileName), Server.MapPath("~\\VisualTool\\Repository\\thumbnails"), "swfThumb", "", 50);
                //CheckFile("~\\VisualTool\\Repository\\" + type + "\\" + fileName);
                //fileupload.SaveAs(HostingEnvironment.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + fileName));
                int result = 0;
                if (result == 0)
                {
                    CheckFile("~\\VisualTool\\Repository\\" + type + "\\" + fileName);
                    File.Move(Server.MapPath("~\\VisualTool\\Repository\\" + type + "\\temp\\" + tempFilename), Server.MapPath("~\\VisualTool\\Repository\\" + type + "\\" + fileName));

                    string query = "insert into LE_Media (Name,Description,Path,Type,Keyword,Thumbnail) values(@name,@description,@path,@type,@keyword,@thumbnail)\t\nselect SCOPE_IDENTITY();";
                    string[] argument = new string[7];
                    argument[0] = clsGeneral.convertQuotes(name);
                    argument[1] = clsGeneral.convertQuotes(description);
                    argument[2] = "~/VisualTool/Repository/" + type + "/" + fileName;
                    argument[3] = type;
                    argument[4] = "0";
                    argument[5] = clsGeneral.convertQuotes(keyword);
                    argument[6] = "~/VisualTool/Repository/thumbnails/" + "swfThumb" + "." + "jpg";

                    int data = oData.saveMediaElements(query, argument);

                    UpdateFileName(type, data, argument[2], argument[6], ".mp3");
                }
                else
                {
                    // thumbnail generation failed......so insertion not possible.....
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //PointDelegate(type);    // Call the function to point the delegate to the non static function getsearchdata....
        //delegate_Search();      // Call the delegate which points to the function getsearchdata...

        getSearchData(repository_type.Value);
    }
}
