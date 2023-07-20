using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StudentBinder_AssessmentSkills : System.Web.UI.Page
{
    clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(sess.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        
        if (!IsPostBack)
        {
            int AsmntId = Convert.ToInt32(Request.QueryString["AsmntId"].ToString());
            int Value = Convert.ToInt32(Request.QueryString["Value"].ToString());
            DataClass oData = new DataClass();

            lbl_AsmntSkills.Text = "Skills for " + oData.ExecuteScalarString("SELECT AsmntName FROM Assessment WHERE AsmntId=" + AsmntId);
            //dlSkillView.DataSource = oData.fillData("SELECT GoalName FROM Goal");
            //dlSkillView.DataBind();
            string selQry = "SELECT DISTINCT Gl.GoalName FROM (AsmntGoalRel Rel INNER JOIN Goal Gl ON Gl.GoalId=Rel.GoalId) INNER JOIN Assessment Asmnt " +
                "ON Asmnt.OrigAsmntId=Rel.AsmntId WHERE Asmnt.AsmntId=" + AsmntId + " AND Asmnt.ActiveInd='A'";
            dlSkillView.DataSource = oData.fillData(selQry);
            dlSkillView.DataBind();
            foreach (DataListItem diSkill in dlSkillView.Items)
            {
                HiddenField hfAsmID = (HiddenField)diSkill.FindControl("hfAsmntID");
                hfAsmID.Value = Value.ToString();

            }
        }
    }
    protected void dlSkillView_ItemCommand(object source, DataListCommandEventArgs e)
    {
        HiddenField hfAsmntID = (HiddenField)e.Item.FindControl("hfAsmntID");
        int asmntID = Convert.ToInt32(hfAsmntID.Value);
        //oSession = (clsSession)Session["UserSession"];
        //if (oSession != null)
        //    oSession.StudentId = Convert.ToInt32(ddl_Student.SelectedValue);
        DataClass oData = new DataClass();
        Session["Asmnt_ModDate"] = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + asmntID);   //Select the last modified date before submitting.....
        Session["skill"] = e.CommandArgument;
        Session["xmlname"] = asmntID;
        Response.Redirect("AssessmentByType.aspx");
    }
    protected void dlSkillView_ItemDataBound(object sender, DataListItemEventArgs e)
    {

    }
    protected void lnk_backtolist_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReviewAssessmnt.aspx?Edit=1");
    }
}