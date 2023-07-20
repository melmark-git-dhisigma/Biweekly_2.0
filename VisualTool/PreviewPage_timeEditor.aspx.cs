using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreviewPage_timeEditor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LessonDetailId"] != null)
        {
            int lessonDetailId = Convert.ToInt32(Session["LessonDetailId"]);
        }
        if (Session["LessonId"] != null)
        {
            int lessonId = Convert.ToInt32(Session["LessonId"]);
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetElementData(int setNumber, int stepNumber)
    {
        cls_data objData = new cls_data();
        svc_timeEditor.ItimeEditorClient tp = new svc_timeEditor.ItimeEditorClient();
        svc_timeEditor.classRequestTime request = new svc_timeEditor.classRequestTime();

        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["LessonId"].ToString());
        request.setNumber = setNumber;
        request.stepNumber = stepNumber;
        svc_timeEditor.ResponseResultTime response = tp.GetLessonDetails(request);
        string[] returnArrayData = response.arryList;

           // Function to return array in random order.
        string contentData = objData.GetArrayInRandomOrder(returnArrayData);
        return contentData;


    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetClockImageSrc(int setNumber, int stepNumber)
    {
        svc_timeEditor.ItimeEditorClient tp = new svc_timeEditor.ItimeEditorClient();
        svc_timeEditor.classRequestTime request = new svc_timeEditor.classRequestTime();

        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["LessonId"].ToString());
        request.setNumber = setNumber;
        request.stepNumber = stepNumber;
        svc_timeEditor.ResponseResultTime response = tp.GetClockImageDetails(request);
        string returnData = response.outString;

        return returnData;


    }
        
}