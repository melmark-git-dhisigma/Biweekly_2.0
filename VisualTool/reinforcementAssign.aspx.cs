using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class reinforcementAssign : System.Web.UI.Page
{

    clsData objData = new clsData();
    clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {

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
            // Session["EditValue"] = Session["LessonId"];
            getImageList();
            getStudentList();
            getReinforcemntList();
        }
        //  int stdId = Convert.ToInt32(Session["studentId"]);
        // string sel = "SELECT StudentLname+','+StudentFname as Name,ImageURL FROM Student WHERE StudentId=" + stdId;

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
    protected void getStudentList()
    {

        try
        {

        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();
        int stdId = Convert.ToInt32(Session["studentId"]);
        request.studID = stdId;
        response = cp.getStudDetails(request);
        svc_contentPage.clsContentLesson[] listdata = response.List_contentLesson;
        // lblName.Text = listdata.ElementAt(0).studName;
        // lblClass.Text = listdata.ElementAt(0).className;
        if (listdata.Length > 0)
        {
            string stdDetails = "Name: " + listdata.ElementAt(0).studName + "\nClass: " + listdata.ElementAt(0).className;
            string studentName = listdata.ElementAt(0).studName.ToString();
            lblStudentName.Text = studentName;
        }
       

        }
        catch
        {

        }
    }
    protected void getReinforcemntList()
    {
        try
        {
            svc_reinforcementAssg.IreinforcementAssgClient cp = new svc_reinforcementAssg.IreinforcementAssgClient();
            svc_reinforcementAssg.reinfRespond response = new svc_reinforcementAssg.reinfRespond();
            svc_reinforcementAssg.reinfRequest request = new svc_reinforcementAssg.reinfRequest();
            request.studentId = Convert.ToInt32(Session["studentId"]);
            response = cp.getReinforcement(request);
            svc_reinforcementAssg.reinforcement[] listdata = response.reinforcementList;
            if (listdata.Length > 0)
            {
                hfCorrect.Value = listdata.ElementAt(0).CorrectAns;
                hfWrong.Value = listdata.ElementAt(0).WrongAns;
                int reinId = listdata.ElementAt(0).ReinId;
            }
        }
        catch { }
    }
    private void getImageList()
    {
        try
        {
            svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
            svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();
            svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

            request.elemType = "reinforcement";
            response = cp.getElementTable(request);
            svc_contentPage.media[] list = response.responseList;
            if (list.Length > 0)
            {
                dl_corrAns.DataSource = list;
                dl_corrAns.DataBind();
                dl_wrngAns.DataSource = list;
                dl_wrngAns.DataBind();
            }
        }
        catch { }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string saveFile(string correctAns, string wrongAns)
    {
        svc_reinforcementAssg.IreinforcementAssgClient svc_reinforcement = new svc_reinforcementAssg.IreinforcementAssgClient();
        svc_reinforcementAssg.reinfRequest request = new svc_reinforcementAssg.reinfRequest();

        request.studentId = Convert.ToInt32(HttpContext.Current.Session["studentId"]);
        request.correctAns = correctAns;
        request.wrongAns = wrongAns;


        svc_reinforcementAssg.reinfRespond response = svc_reinforcement.saveReinforcement(request);


        return response.outputString;
    }



    protected void btnPreview_Click(object sender, EventArgs e)
    {
        Response.Redirect("reinforcement_Demo.aspx");
    }
}