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

public partial class StudentBinder_NewGoalAssess : System.Web.UI.Page
{
    string currFilePath = string.Empty; //File Full Path
    string currFileExtension = string.Empty;  //File Extension
    //clsSession sess = null;
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
            
            if (Request.QueryString["studid"] != null)
            {
                int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());
                oSession.StudentId = studid;
            }
            if (oSession.StudentId != 0)
            {
                oSession = (clsSession)Session["UserSession"];
                fillSkillList();
            }
           
            string actYear = oData.ExecuteScalarString("SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A'");
            lblYear.Text = actYear;
        }


    }


    protected void fillSkillList()
    {
        // Fill the Assessment Name into the Datalist

        clsData objData = new clsData();
        DataTable Dt = new DataTable();

        Dt = objData.ReturnDataTable("Select GoalName Goal,GoalPic as Iamge from Goal");




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
            Response.Redirect("AssessmentBySkill.aspx");
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Select Student");
            //fillSkillList();
        }
    }
    
    protected void dlSkillMenu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //ImageButton imgBtn = (ImageButton)e.Item.FindControl("imgbtnIcon2");
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
}