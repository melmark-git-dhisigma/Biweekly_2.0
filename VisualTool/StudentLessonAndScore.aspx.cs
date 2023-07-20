using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class StudentLessonAndScore : System.Web.UI.Page
{
    clsData objData = new clsData();
    clsSession sess = null;
    svc_lessonManagement.LessonManagementClient lpClient = new svc_lessonManagement.LessonManagementClient();
    svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
    svc_lessonManagement.clsRequest request = new svc_lessonManagement.clsRequest();
    private int studId;
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
            fillclass();
            FillStudentList();
        }

    }
    public void fillclass()
    {
        sess = (clsSession)Session["UserSession"];
        clsData objData = new clsData();
        if (objData.IFExists("SELECT ClassId from UserClass where UserId='" + sess.LoginId + "'") == true)
        {
            string strQuery = "SELECT UsrCls.ClassId AS Id,Cls.ClassName AS Name FROM UserClass UsrCls INNER JOIN Class Cls ON Cls.ClassId=UsrCls.ClassId WHERE UsrCls.UserId=" + sess.LoginId + " AND Cls.ActiveInd='A' AND UsrCls.SchoolId=" + sess.SchoolId;
            objData.ReturnDropDown(strQuery, ddlClassName);
        }
    }

    protected void FillStudentList()
    {

        request.clsId = 0;
        response = lpClient.FillStudentListComplete(request);
        svc_lessonManagement.output[] listData = response.outputList;
        dlistStudent.DataSource = listData;
        dlistStudent.DataBind();

    }

    protected void ddlClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlClassName.SelectedIndex == 0)
        {
            request.clsId = 0;
            response = lpClient.FillStudentListComplete(request);
            svc_lessonManagement.output[] listData = response.outputList;
            dlistStudent.DataSource = listData;
            dlistStudent.DataBind();
        }
        else
        {
            request.clsId = Convert.ToInt32(ddlClassName.SelectedValue);
            response = lpClient.FillStudentListComplete(request);
            svc_lessonManagement.output[] listData = response.outputList;
            dlistStudent.DataSource = listData;
            dlistStudent.DataBind();
        }
    }

    protected void btnLessonAssign_Click(object sender, EventArgs e)
    {
        int x = Convert.ToInt32(Request.Cookies["studentId"].Value);
        //  Response.Write(x);
    }
    protected void btnReinforce_Click(object sender, EventArgs e)
    {
        if (Request.Cookies["studentId"] != null)
        {
            studId = Convert.ToInt32(Request.Cookies["studentId"].Value);
            Session["StudentId"] = studId;
            if (studId > 0)
            {
                Response.Redirect("reinforcementAssign.aspx");
            }
        }
    }
    protected void Btnscores_Click(object sender, EventArgs e)
    {

        int x = Convert.ToInt32(Request.Cookies["studentId"].Value);
        //Response.Write(x);
    }
    protected void dlistStudent_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if ((e.Item.ItemType != ListItemType.Header) && (e.Item.ItemType != ListItemType.Footer))
        {
            HiddenField url = (HiddenField)e.Item.FindControl("hdnURL");
            Image imgStudents = (Image)e.Item.FindControl("imgStudents");
            string ImageUrl = "";
            if (ConfigurationManager.AppSettings["BuildName"].ToString() == "Local")
            {
                ImageUrl = "../Administration/StudentsPhoto/" + url.Value;
            }
            else
            {
                ImageUrl = "data:image/gif;base64," + url.Value;
            }
            imgStudents.ImageUrl = ImageUrl;
        }
    }
    
}