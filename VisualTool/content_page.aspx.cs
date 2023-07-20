using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class content_page : System.Web.UI.Page
{
    svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
    svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();
    svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

    protected void Page_Load(object sender, EventArgs e)
    {
        //string x = Session["LessonId"].ToString();
        Session["EditValue"] = Session["LessonId"];

        getImageList();
        getVideoList();
        getAudioList();
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
    public static string saveFile( string contents,string musicFile)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.pageNo = 1;
        request.pageType = "content-single";
        request.contentData = contents;
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["LessonId"].ToString());
        request.MusicFile = musicFile;

        svc_contentPage.clsResponse response = cp.saveLesson(request);


        return response.responseString;
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

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getSlideDatas(string slideNo)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.pageNo = Convert.ToInt32(slideNo);
        request.pageType = "content-single";
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["lessonId"].ToString());


        svc_contentPage.clsResponse response = cp.getSlideContents(request);

        return response.responseString+'^'+response.responseString2;
    }
}

