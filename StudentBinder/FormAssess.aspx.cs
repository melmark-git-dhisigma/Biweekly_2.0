using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;
using System.Web.Services;

public partial class StudentBinder_NewFormAssess : System.Web.UI.Page
{
    
    DataClass oData = new DataClass();
    clsSession oSession = null;
    static bool Disable = false;
    //Page_Load Event, Register Button Click Event

    protected void Page_Load(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];
        if (oSession == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            //bool flag = clsGeneral.PageIdentification(oSession.perPage);
            //if (flag == false)
            //{
            //    Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            //}

        }
        DataClass oData = new DataClass();
        if (!Page.IsPostBack)
        {

            //static bool Disable = false;
            clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
            
            if (Request.QueryString["studid"] != null)
            {             
                int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
                oSession.StudentId = studid;
            }
            if (oSession.StudentId != 0)
            {
                oSession = (clsSession)Session["UserSession"];
                fillList();
                fillSkillList();
            }

            string actYear = oData.ExecuteScalarString("SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A'");
            lblYear.Text = actYear;
        }


    }


    protected void fillList()
    {
        // Fill the Assessment Name into the Datalist
        dlFormMenu.DataSource = oData.fillData("SELECT AsmntName,AsmntId FROM Assessment WHERE ISNULL(StudentId,'')='' AND ISNULL(EffEndDate,'')='' AND ActiveInd='A'");
        dlFormMenu.DataBind();


    }

    protected void fillSkillList()  //GoalCode is used to display the Edited goals
    {
        // Fill the Assessment Name into the Datalist
        clsData objData = new clsData();
        DataTable Dt = new DataTable();

        Dt = objData.ReturnDataTable("Select GoalCode Goal,GoalPic as [Image] from Goal where ActiveInd='A' ", false);

        //DataTable dt_temp = new DataTable();
        //ClassDatatable oDt = new ClassDatatable();
        //dt_temp = oDt.CreateColumn("Goal", dt_temp);
        //dt_temp = oDt.CreateColumn("Image", dt_temp);

        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Academic Skills", "~/StudentBinder/img/academicskills.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Gross Motor Skills", "~/StudentBinder/img/GrossMotorSkills.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Social Skills", "~/StudentBinder/img/socialskills.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Activities of Daily Living", "~/StudentBinder/img/activitysofdailyLiving.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Behavior", "~/StudentBinder/img/behavour.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Early Learner Skills", "~/StudentBinder/img/earlyLearnSkills.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Language Skills", "~/StudentBinder/img/languageskills.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Community Living", "~/StudentBinder/img/communityLiving.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Personal Responsibility", "~/StudentBinder/img/personalresponsibility.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Vocational Skills", "~/StudentBinder/img/vocationalSkills.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Fine Motor Skills", "~/StudentBinder/img/findmotorskills.png" });
        //dt_temp = oDt.CreateAssessmntsTable(new string[] { "Goal", "Image" }, dt_temp, new string[] { "Communication", "~/StudentBinder/img/communication.png" });


        dlSkillMenu.DataSource = Dt;
        dlSkillMenu.DataBind();

    }

    protected void dlSkillMenu_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (oSession.StudentId != 0)
        {
            Session["goalname"] = e.CommandArgument.ToString();

            //Session["studID"] = ddl_Students.SelectedValue;
            Session["Mode"] = "New";
            Response.Redirect("AssessmentBySkill.aspx?Skill=" + Disable);
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Select Student");
            //fillSkillList();
        }
    }

    protected void dlFormMenu_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (oSession.StudentId != 0)
        {
            DataClass oData = new DataClass();
            //int YearID = oData.ExecuteScalar("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='Y'");
            ClassAssess oAssess = new ClassAssess();
            byte[] blobData = oAssess.SelectBlobData("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(e.CommandArgument));

            ClassStudntAssess oStAssess = new ClassStudntAssess();
            oSession = (clsSession)Session["UserSession"];
            if (oSession != null)
                oStAssess.StudID = Convert.ToInt32(oSession.StudentId);
            oStAssess.YearID = oData.ExecuteScalar("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
            oStAssess.StudAssessName = e.CommandName + " - " + DateTime.Now.ToShortDateString();
            oStAssess.AssessTempName = e.CommandName;
            oStAssess.AssessType = "By Assessment";
            oStAssess.BlobData = blobData;
            oStAssess.AsmntID = Convert.ToInt32(e.CommandArgument);

            //string insQry = "INSERT INTO StdtAsmnt(SchoolId, StudentId,AsmntYearId, AssignedUserId, AsmntTemplateId," +
            //                       "AsmntGroupId,AsmntStatusId,IncScoreInd,StdtAsmntName,AsmntTemplateName,AsmntType,StdtAsmntXML," +
            //                       "CreatedBy,CreateOn)" +
            //                       "VALUES (@School,@StId,@YearId,@AssgnUserId,@AssmntId,@AssmntGrpId,@AssmntStatusId,@IncScr," +
            //                       "@StAssessName,@AssessTempName,@Type,@XML,@User,@Date)\r\n" +
            //                       "SELECT SCOPE_IDENTITY()";

            string ins = "INSERT INTO [Assessment]([SchoolId],[StudentId],[AsmntYearId],[AssignedUserId],[OrigAsmntId],[AsmntStatusId],[AsmntName]," +
                "[AsmntTemplateName],[AsmntTyp],[AsmntStartTs],[EffStartDate],[ActiveInd],[AsmntXML],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) " +
                "VALUES(@School,@StId,@YearId,@AssgnUserId,@AssmntId,@AssmntStatusId,@AsmntName,@AssessTempName,@Type,(SELECT convert(varchar, getdate(), 100))," +
                "(SELECT convert(varchar, getdate(), 100)),'A',@XML,@User,(SELECT convert(varchar, getdate(), 100)),@ModUsr,(SELECT convert(varchar, getdate(), 100)))\r\n" +
                "SELECT SCOPE_IDENTITY()";
            int status = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='Assessment Status' AND LookupName='Not Started'");
            int nRetValue = oStAssess.Save(ins, 0, oSession.LoginId, oSession.SchoolId, status);

            Session["xmlname"] = nRetValue;
            Session["Asmnt_ModDate"] = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + nRetValue);   //Add the last modified date into a session before editing.....
            //Session["studID"] = ddl_Students.SelectedValue;
            Session["skill"] = "All";
            Response.Redirect("AssessmentByType.aspx?Type=" + Disable);
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Select Student");
            //fillList();
        }

    }

    protected void dlFormMenu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //ImageButton imgBtn = (ImageButton)e.Item.FindControl("imgbtnIcon");
        //bool Disable = false;
        //clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
        //if (Disable == true)
        //{
        //    imgBtn.Visible = false;
        //}
        //else
        //{
        //    imgBtn.Visible = true;
        //}
    }

    protected void btnPrior_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReviewAssessmnt.aspx?Edit=1");
    }
   
}