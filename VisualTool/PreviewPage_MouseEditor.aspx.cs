using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreviewPage_MouseEditor : System.Web.UI.Page
{
    svc_MouseEditor.MouseEditorClient mouse = new svc_MouseEditor.MouseEditorClient();
    svc_MouseEditor.ResponseResult mResponse = new svc_MouseEditor.ResponseResult();
    svc_MouseEditor.classRequest mRequest = new svc_MouseEditor.classRequest();
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
        svc_MouseEditor.MouseEditorClient mp = new svc_MouseEditor.MouseEditorClient();
        svc_MouseEditor.classRequest mRequest = new svc_MouseEditor.classRequest();
        svc_MouseEditor.ResponseResult mResponse = new svc_MouseEditor.ResponseResult();

        mRequest.lessonId = Convert.ToInt32(HttpContext.Current.Session["LessonId"].ToString());
        mRequest.setNumber = setNumber;
        mRequest.stepNumber = stepNumber;
        svc_MouseEditor.ResponseResult response = mp.GetLessonVal(mRequest);
        string[] newVal = response.arryList;
        string contents = "";
        for (int i = 0; i < newVal.Length; i++)
        {
            contents += newVal[i]+",";
        }
        contents = contents + '^' + response.outInt;

        return contents;        
        
    }

   // [System.Web.Services.WebMethod(EnableSession = true)]
   // public static string GetElementData(int lessonId)
   // {
   //     svc_MouseEditor.MouseEditorClient mp = new svc_MouseEditor.MouseEditorClient();
   //     svc_MouseEditor.classRequest mRequest = new svc_MouseEditor.classRequest();
   //     svc_MouseEditor.ResponseResult mResponse = new svc_MouseEditor.ResponseResult();

   //     mRequest.lessonId = lessonId;

   //     mResponse = mp.GetLessonVal(mRequest);
   //     svc_MouseEditor.outValue[] listNew = mp.GetLessonVal(mRequest);
   //}
}