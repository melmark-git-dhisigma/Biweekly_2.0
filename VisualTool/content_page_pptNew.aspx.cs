﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class content_page_pptNew : System.Web.UI.Page
{
    svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
    svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();
    svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();
    clsData oData = null;
    public clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        oData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        if (!IsPostBack)
        {
            if (sess != null)
            {
                lblLoginName.Text = sess.UserName;
            }

            setTitle();
        }
        Session["EditValue"] = Session["LessonId"];
        if (Session["LessonId"] != null)
        {
            object objLP = oData.FetchValue("SELECT '[' + LessonName + '] - [' + Convert(varchar(20),LessonId) + '] - ' + ISNULL(OwnerName,'') FROM LE_Lesson WHERE LessonId=" + Session["LessonId"].ToString());
            if (objLP != null)
                td_LP.InnerHtml = "<b>Lesson Name</b> : " + objLP.ToString();
        }
        if (Request.QueryString["Edit"] != null && Request.QueryString["edit"] == "1")
        {
            fillLessons();
        }

        getImageList();
        getVideoList();
        getAudioList();

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
                TitleName.Text = obj.ToString();


            }
        }
    }



    private void fillLessons()
    {
        // pass lessonId and get the whole lesson details

        string slides = "";

        request.lessonId = Convert.ToInt32(Session["LessonId"].ToString());
        response = cp.getLessonDetails(request);
        svc_contentPage.clsContentLesson[] list = response.List_contentLesson;

        if (list.Length > 0)
        {
            for (int i = 0; i < list.Length; i++)
            {
                slides += list.ElementAt(i).PageNo.ToString() + ",";
            }
        }

        hd_slideNo.Value = slides;
        try
        {
            Response.Cookies["slides"].Value = slides;
            Response.Cookies["slides"].Expires = DateTime.Now.AddDays(1);
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    private void getAudioList()
    {
        request.elemType = "audios";
        response = cp.getElementTable(request);
        svc_contentPage.media[] list = response.responseList;
        if (list.Length > 0)
        {
            dl_musicList.DataSource = list;
            dl_musicList.DataBind();
        }
    }
    private void getImageList()
    {
        request.elemType = "images";
        response = cp.getElementTable(request);
        svc_contentPage.media[] list = response.responseList;

        if (list.Length > 0)
        {
            DataList1.DataSource = list;
            DataList1.DataBind();
        }
    }
    private void getVideoList()
    {
        request.elemType = "videos";
        response = cp.getElementTable(request);
        svc_contentPage.media[] list = response.responseList;
        if (list.Length > 0)
        {
            DataList2.DataSource = list;
            DataList2.DataBind();
        }
    }

    private void getSearchElements(string type, string name)
    {


        if (type == "images")
        {
            request.elemType = "images";
            request.searchWord = name;

            response = cp.getSearchElements(request);
            svc_contentPage.media[] list = response.responseList;


            if (list.Length > 0)
            {
                DataList1.DataSource = list;
                DataList1.DataBind();
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "$('.draggable').draggable({revert: 'invalid',helper: 'clone',cursor: 'move'});", true);
        }
        else
        {
            request.elemType = "videos";
            request.searchWord = name;

            response = cp.getSearchElements(request);
            svc_contentPage.media[] list = response.responseList;

            if (list.Length > 0)
            {
                DataList2.DataSource = list;
                DataList2.DataBind();
            }
        }

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string saveFile(string contents, string musicFile)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.pageNo = 1;
        request.pageType = "content-ppt";
        request.contentData = contents;
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["LessonId"].ToString());
        request.MusicFile = musicFile;

        svc_contentPage.clsResponse response = cp.saveLesson(request);


        return response.responseString;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string deleteSlide_ppt(string slideNo)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.pageNo = Convert.ToInt32(slideNo);
        request.pageType = "content-ppt";
        request.contentData = "";
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["lessonId"].ToString());


        svc_contentPage.clsResponse response = cp.deleteSlide(request);


        return response.responseString;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string saveFile_ppt(string contents, string slideNo, string MusicFile)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.pageNo = Convert.ToInt16(slideNo);
        request.pageType = "content-ppt";
        request.contentData = contents;
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["lessonId"].ToString());
        request.MusicFile = MusicFile;

        svc_contentPage.clsResponse response = cp.saveLesson(request);


        return response.responseString;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getSlideDatas(string slideNo)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.pageNo = Convert.ToInt32(slideNo);
        request.pageType = "content-ppt";
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["lessonId"].ToString());


        svc_contentPage.clsResponse response = cp.getSlideContents(request);

        return response.responseString + '^' + response.responseString2;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static String[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();
        svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();

        request.elemType = "images";


        response = cp.getMediaSearch(request);

        svc_contentPage.mediaSearch[] name = response.responseSearchList;

        return (from m in name where m.searchResult.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m.searchResult).Take(count).ToArray();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static String[] GetCompletionList2(string prefixText, int count, string contextKey)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();
        svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();

        request.elemType = "videos";


        response = cp.getMediaSearch(request);

        svc_contentPage.mediaSearch[] name = response.responseSearchList;

        return (from m in name where m.searchResult.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m.searchResult).Take(count).ToArray();
    }



    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        getSearchElements("images", txtSearchImage.Text);
    }
    protected void imgSearchVideo_Click(object sender, ImageClickEventArgs e)
    {
        //getSearchElements("videos", txtSearchVideo.Text);
    }

    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Logout.aspx");
    }
}