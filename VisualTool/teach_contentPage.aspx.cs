using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class teach_contentPage : System.Web.UI.Page
{
    svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
    svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();
    svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();


    protected void Page_Load(object sender, EventArgs e)
    {
        //  Session["LessonId"] = 1;

        request.lessonId = Convert.ToInt32(Session["LessonId"].ToString());
        request.pageNo = Convert.ToInt32(Request.QueryString["pageNo"]);


        response = cp.getContentData(request);
        // Label1.Text = response.responseString;
        lblMp3.Text = response.responseString2;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getSlideDatas(string slideNo, string type)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.pageNo = Convert.ToInt32(slideNo);
        request.pageType = type;
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["lessonId"].ToString());


        svc_contentPage.clsResponse response = cp.getSlideContents(request);

        return response.responseString + '^' + response.responseString2;
    }

}