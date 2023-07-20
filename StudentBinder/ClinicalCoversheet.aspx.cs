using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Xml;
using System.IO.Packaging;
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using System.Data;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;


using System.Globalization;



public partial class StudentBinder_ClinicalCoversheet : System.Web.UI.Page
{
    System.Data.DataTable dtRecChang = new System.Data.DataTable();
    System.Data.DataTable dtAssmtReinfo = new System.Data.DataTable();
    System.Data.DataTable dtAssmntTool = new System.Data.DataTable();
    clsData objData = null;
    System.Data.DataTable dt = null;
    System.Data.DataTable Dt = null;


    static string[] columns;
    static string[] placeHolders;

    static string[] tblAppnd;
    static string[] placeHoldersCheck;

    static string[] columnsToAdd;
    static string[] columnsP4;
    static string[] placeHoldersP4;

    static int checkCount = 0;
    static int P4TotalCount = 1;

    clsSession sess = null;

    int StudentId = 2;
    int SchoolId = 1;
    int Classid = 1;


    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
           
            sess = (clsSession)Session["UserSession"];

            

            PanelMain.Visible = false;
            loadDrpDates();

            loadData();        // Fill behaviour data

            tdMsg.InnerHtml = "";
            //select distinct CONVERT(VARCHAR(10),PgmCordDate,101) as id,CONVERT(VARCHAR(10),PgmCordDate,101) from  StdtClinicalCoverSheet
        }
    }
    public void loadDrpDates()
    {//select CONVERT(varchar(10),EndDate,101) as EndDate from StdtClinicalCoverSheet order by EndDate desc
        dt = new System.Data.DataTable();
        objData = new clsData();
        dt = objData.ReturnDataTable("select CONVERT(varchar(10),EndDate,101) as EndDate from StdtClinicalCoverSheet order by EndDate desc", false);
        drpSelectDate.Items.Clear();
        drpSelectDate.Items.Add(DateTime.Now.ToString("MM'/'dd'/'yyyy"));
        foreach (DataRow dr in dt.Rows)
        {
           
            if (dr[0].ToString() != DateTime.Now.ToString("MM'/'dd'/'yyyy"))
            {
                drpSelectDate.Items.Add(dr[0].ToString());
            }
           
        }

    }

    public void loadDataToday()
    {

        dt = new System.Data.DataTable();
        objData=new clsData();
        sess = (clsSession)Session["UserSession"];
        dt = objData.ReturnDataTable("select ClinicalCvId,SchoolId,ClassId,StudentId,CONVERT(VARCHAR(10),StartDate,101) as StartDate,CONVERT(VARCHAR(10),EndDate,101) as EndDate,FollowUp,Proposals,PgmCord,CONVERT(VARCHAR(10),PgmCordDate,101) as PgmCordDate ,EduCord,CONVERT(VARCHAR(10),EduCordDate,101) as EduCordDate ,BCBA,CONVERT(VARCHAR(10),BCBADate,101) as BCBADate from StdtClinicalCoverSheet where CAST( EndDate as date)= cast('"+drpSelectDate.SelectedItem.Text+"' as date) and SchoolId='"+sess.SchoolId+"' and StudentId='"+sess.StudentId+"'", true);
        txtFollowUp.Text = dt.Rows[0]["FollowUp"].ToString();
        txtPreposals.Text = dt.Rows[0]["Proposals"].ToString();
        txtPgmCordntr.Text = dt.Rows[0]["PgmCord"].ToString();
        txteduCodntr.Text = dt.Rows[0]["EduCord"].ToString();
        txtBCBA.Text = dt.Rows[0]["BCBA"].ToString();
        txtDatePgnCord.Text = dt.Rows[0]["PgmCordDate"].ToString();
        txtDateEduCord.Text = dt.Rows[0]["EduCordDate"].ToString();
        txtDateBCBACord.Text = dt.Rows[0]["BCBADate"].ToString();
        drpSelectDate.SelectedItem.Text = dt.Rows[0]["EndDate"].ToString();
        int clvId = int.Parse(dt.Rows[0]["ClinicalCvId"].ToString());
        hdFldCvid.Value = dt.Rows[0]["ClinicalCvId"].ToString();
        dt = objData.ReturnDataTable("select AsmtToolId,TrgetBehav as TargetBehavior,Functions as 'Function' ,AnalysisTool as AnalysisTools from clvAsmtTool where StdtCoverSheetId=" + clvId, true);
        if (dt.Rows.Count > 0)
        {
            gVAssmntTool.DataSource = dt;
            gVAssmntTool.DataBind();
        }
         

        dt = objData.ReturnDataTable("select RecChangId,Recomendation as Recommendation,TimeLine as Timeline, PersonResponsible as 'Person Responsible' from clvRecChange where StdtCoverSheetId=" + clvId, true);
        if (dt.Rows.Count > 0)
        {
            gVRecChange.DataSource = dt;
            gVRecChange.DataBind();

        }
       

        dt = objData.ReturnDataTable("select ReinfoSurId,CONVERT(varchar(10),ReinfoDate,101) as 'Date', ToolUtilizd as ToolUtilized from clvReinfoSur where StdtCoverSheetId=" + clvId, true);
        if (dt.Rows.Count > 0)
        {
            gVAssmtReinfo.DataSource = dt;
            gVAssmtReinfo.DataBind();
        }
       
    }

    public void clearDatas()
    {
        txtFollowUp.Text = "";
        txtPreposals.Text = "";
        txtPgmCordntr.Text = "";
        txteduCodntr.Text = "";
        txtBCBA.Text = "";
        txtDatePgnCord.Text = "";
        txtDateEduCord.Text = "";
        txtDateBCBACord.Text = "";
        gVAssmntTool.DataSource = null;
        gVAssmntTool.DataBind();
        gVAssmtReinfo.DataSource = null;
        gVAssmtReinfo.DataBind();
        gVBehavior.DataSource = null;
        gVBehavior.DataBind();
        gVRecChange.DataSource = null;
        gVRecChange.DataBind();
        
    }
    public void loadData()
    {
        objData = new clsData();
        dt = new System.Data.DataTable();
      //  DateTime todayDate=DateTime.Parse(drpSelectDate.SelectedItem.Text);
        DateTime todayDate = DateTime.ParseExact(drpSelectDate.SelectedItem.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime lastDayDate=todayDate.AddDays(-90);

        dt = objData.ReturnDataTable("select distinct BehaviourDetails.Behaviour from StdtAggScores inner join BehaviourDetails on StdtAggScores.MeasurementId=BehaviourDetails.MeasurementId where CAST( AggredatedDate as date) between   cast( '" + lastDayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and CAST( '" + todayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and StdtAggScores.MeasurementId is not null and BehaviourDetails.ActiveInd='A'", true);
        gVBehavior.DataSource = dt;
        gVBehavior.DataBind();
    }


    //*********** load and gVAssmntTool ***********
    public void loadAssmntTool()
    {

        DataRow dr = dtAssmntTool.NewRow();
        dr["TargetBehavior"] = "";
        dr["Function"] = "";
        dr["AnalysisTools"] = "";
        dtAssmntTool.Rows.Add(dr);
        gVAssmntTool.DataSource = dtAssmntTool;
        gVAssmntTool.DataBind();
        
    }
    public void addRowAssmntTool()
    {
        dtAssmntTool.Columns.Add("TargetBehavior", typeof(string));
        dtAssmntTool.Columns.Add("Function", typeof(string));
        dtAssmntTool.Columns.Add("AnalysisTools", typeof(string));
        foreach (GridViewRow gdVr in gVAssmntTool.Rows)
        {
            DataRow drtemp = dtAssmntTool.NewRow();

            drtemp["TargetBehavior"] = ((TextBox)gdVr.FindControl("txtTargetBehavior")).Text;
            drtemp["Function"] = ((TextBox)gdVr.FindControl("txtFunction")).Text;
            drtemp["AnalysisTools"] = ((TextBox)gdVr.FindControl("txtAnalysisTools")).Text;
            dtAssmntTool.Rows.Add(drtemp);
        }
        DataRow dr = dtAssmntTool.NewRow();
        dr["TargetBehavior"] = "";
        dr["Function"] = "";
        dr["AnalysisTools"] = "";
        dtAssmntTool.Rows.Add(dr);
        gVAssmntTool.DataSource = dtAssmntTool;
        gVAssmntTool.DataBind();
    }
    public void delRowAssmntTool(int intexRow)
    {
        dtAssmntTool.Columns.Add("TargetBehavior", typeof(string));
        dtAssmntTool.Columns.Add("Function", typeof(string));
        dtAssmntTool.Columns.Add("AnalysisTools", typeof(string));
        foreach (GridViewRow gdVr in gVAssmntTool.Rows)
        {
            DataRow drtemp = dtAssmntTool.NewRow();

            drtemp["TargetBehavior"] = ((TextBox)gdVr.FindControl("txtTargetBehavior")).Text;
            drtemp["Function"] = ((TextBox)gdVr.FindControl("txtFunction")).Text;
            drtemp["AnalysisTools"] = ((TextBox)gdVr.FindControl("txtAnalysisTools")).Text;
            dtAssmntTool.Rows.Add(drtemp);
        }
        if (dtAssmntTool.Rows.Count == 1)
        {
            dtAssmntTool.Rows.RemoveAt(intexRow);
            loadAssmntTool();
        }
        else
        {
            dtAssmntTool.Rows.RemoveAt(intexRow);
        }

        gVAssmntTool.DataSource = dtAssmntTool;
        gVAssmntTool.DataBind();
    }

    protected void gVAssmntTool_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (gVAssmntTool.Rows.Count == 5) return;
            addRowAssmntTool();
        }
    }
    protected void gVAssmntTool_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowAssmntTool(e.RowIndex);
    }

    //*********** load and gVAssmntTool End***********

    //*********** load and gVRecChange ***********
    public void loadRecChange()
    {
       
        DataRow dr = dtRecChang.NewRow();
        dr["Recommendation"] = "";
        dr["Timeline"] = "";
        dr["Person Responsible"] = "";
        dtRecChang.Rows.Add(dr);
        gVRecChange.DataSource = dtRecChang;
        gVRecChange.DataBind();
        //dtRecChang.Rows.Add(
    }
    public void addRow()
    {
        dtRecChang.Columns.Add("Recommendation", typeof(string));
        dtRecChang.Columns.Add("Timeline", typeof(string));
        dtRecChang.Columns.Add("Person Responsible", typeof(string));
        foreach (GridViewRow gdVr in gVRecChange.Rows)
        {
            DataRow drtemp = dtRecChang.NewRow();
            
            drtemp["Recommendation"] = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            drtemp["Timeline"] = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            drtemp["Person Responsible"] = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            dtRecChang.Rows.Add(drtemp);
        }
            DataRow dr = dtRecChang.NewRow();
        dr["Recommendation"] = "";
        dr["Timeline"] = "";
        dr["Person Responsible"] = "";
        dtRecChang.Rows.Add(dr);
        gVRecChange.DataSource = dtRecChang;
        gVRecChange.DataBind();
    }
    public void delRow(int intexRow)
    {
        dtRecChang.Columns.Add("Recommendation", typeof(string));
        dtRecChang.Columns.Add("Timeline", typeof(string));
        dtRecChang.Columns.Add("Person Responsible", typeof(string));
        foreach (GridViewRow gdVr in gVRecChange.Rows)
        {
            DataRow drtemp = dtRecChang.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            temp = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            drtemp["Recommendation"] = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            drtemp["Timeline"] = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            drtemp["Person Responsible"] = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            dtRecChang.Rows.Add(drtemp);
        }
        if (dtRecChang.Rows.Count == 1)
        {
            dtRecChang.Rows.RemoveAt(intexRow);
            loadRecChange();
        }
        else
        {
            dtRecChang.Rows.RemoveAt(intexRow);
        }
       
        gVRecChange.DataSource = dtRecChang;
        gVRecChange.DataBind();
    }

    protected void gVRecChange_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (gVRecChange.Rows.Count == 5) return;
            addRow();
        }
        
    }
    protected void btnAddRow_Click(object sender, EventArgs e)
    {

    }
    protected void gVRecChange_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRow(e.RowIndex);
    }

    //*********** load and gVRecChange End***********

    //*********** load and gVAssmtReinfo ***********
    public void loadAssmtReinfo()
    {
       
       
        DataRow dr = dtAssmtReinfo.NewRow();
        dr["Date"] = "";
        dr["ToolUtilized"] = "";
       
        dtAssmtReinfo.Rows.Add(dr);
        gVAssmtReinfo.DataSource = dtAssmtReinfo;
        gVAssmntTool.DataBind();
       
    }
    public void addRowAssmtReinfo()
    {
        dtAssmtReinfo.Columns.Add("Date", typeof(string));
        dtAssmtReinfo.Columns.Add("ToolUtilized", typeof(string));
      
        foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
        {
            DataRow drtemp = dtAssmtReinfo.NewRow();

            drtemp["Date"] = ((TextBox)gdVr.FindControl("txtDate")).Text;
            drtemp["ToolUtilized"] = ((TextBox)gdVr.FindControl("txtToolUtilized")).Text;
           
            dtAssmtReinfo.Rows.Add(drtemp);
        }
        DataRow dr = dtAssmtReinfo.NewRow();
        dr["Date"] = "";
        dr["ToolUtilized"] = "";
       
        dtAssmtReinfo.Rows.Add(dr);
        gVAssmtReinfo.DataSource = dtAssmtReinfo;
        gVAssmtReinfo.DataBind();
    }
    public void delRowAssmtReinfo(int intexRow)
    {
        dtAssmtReinfo.Columns.Add("Date", typeof(string));
        dtAssmtReinfo.Columns.Add("ToolUtilized", typeof(string));
     
        foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
        {
            DataRow drtemp = dtAssmtReinfo.NewRow();

            drtemp["Date"] = ((TextBox)gdVr.FindControl("txtDate")).Text;
            drtemp["ToolUtilized"] = ((TextBox)gdVr.FindControl("txtToolUtilized")).Text;
           
            dtAssmtReinfo.Rows.Add(drtemp);
        }
        if (dtAssmtReinfo.Rows.Count == 1)
        {
            dtAssmtReinfo.Rows.RemoveAt(intexRow);
            loadAssmtReinfo();
        }
        else
        {
            dtAssmtReinfo.Rows.RemoveAt(intexRow);
        }
        gVAssmtReinfo.DataSource = dtAssmtReinfo;
        gVAssmtReinfo.DataBind();
    }
  
    protected void gVAssmtReinfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (gVAssmtReinfo.Rows.Count == 5) return;
            addRowAssmtReinfo();
        }
    }
    protected void gVAssmtReinfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowAssmtReinfo(e.RowIndex);
    }
    //*********** load and gVAssmtReinfo End ***********
    protected void btnSave_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        objData = new clsData();
        DateTime todayDate = new DateTime();
        DateTime lastdate = new DateTime();
        todayDate = DateTime.Now;
        lastdate = todayDate.AddDays(-90);

        if (btnSave.Text == "Save")
        {
            string strquery = "insert into StdtClinicalCoverSheet(StartDate,EndDate,FollowUp,Proposals,PgmCord,PgmCordDate,EduCord,EduCordDate,BCBA,BCBADate) values('" + lastdate.ToString("yyyy'/'MM'/'dd") + "','" + todayDate.ToString("yyyy'/'MM'/'dd") + "','" + txtFollowUp.Text + "','" + txtPreposals.Text + "','" + txtPgmCordntr.Text + "','" + txtDatePgnCord.Text + "','" + txteduCodntr.Text + "','" + txtDateEduCord.Text + "','" + txtBCBA.Text + "','" + txtDateBCBACord.Text + "')";
            int cvrsheetId = objData.ExecuteWithScope(strquery);

            if (cvrsheetId > 0)
            {
                // inserting Assmt tools
                foreach (GridViewRow gdVr in gVAssmntTool.Rows)
                {
                    if ((((TextBox)gdVr.FindControl("txtTargetBehavior")).Text != "") && (((TextBox)gdVr.FindControl("txtFunction")).Text != "") && (((TextBox)gdVr.FindControl("txtAnalysisTools")).Text != ""))
                    {
                        strquery = "insert into clvAsmtTool(StdtCoverSheetId,TrgetBehav,Functions,AnalysisTool) values(" + cvrsheetId + ",'" + ((TextBox)gdVr.FindControl("txtTargetBehavior")).Text + "','" + ((TextBox)gdVr.FindControl("txtFunction")).Text + "','" + ((TextBox)gdVr.FindControl("txtAnalysisTools")).Text + "')";
                        objData.Execute(strquery);
                    }
                }
                //inserting Reinforcment  insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values()

                foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
                {
                    if ((((TextBox)gdVr.FindControl("txtDate")).Text != "") && (((TextBox)gdVr.FindControl("txtToolUtilized")).Text != ""))
                    {
                        strquery = "insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values(" + cvrsheetId + ",'" + ((TextBox)gdVr.FindControl("txtDate")).Text + "','" + ((TextBox)gdVr.FindControl("txtToolUtilized")).Text + "')";
                        objData.Execute(strquery);
                    }

                }
                //inserting Rec Changes
                foreach (GridViewRow gdVr in gVRecChange.Rows)
                {
                    if ((((TextBox)gdVr.FindControl("txtRecomd")).Text != "") && (((TextBox)gdVr.FindControl("txtTimeLine")).Text != "") && (((TextBox)gdVr.FindControl("txtPerRes")).Text != ""))
                    {
                        strquery = "insert into clvRecChange(StdtCoverSheetId,Recomendation,TimeLine,PersonResponsible) values(" + cvrsheetId + ",'" + ((TextBox)gdVr.FindControl("txtRecomd")).Text + "','" + ((TextBox)gdVr.FindControl("txtTimeLine")).Text + "','" + ((TextBox)gdVr.FindControl("txtPerRes")).Text + "')";
                        objData.Execute(strquery);
                    }
                }
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Details Inserted Successfully");
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Details Insertion Failed");
            }
        }
        if (btnSave.Text == "Update")
        {
          //  string s1 = " update StdtClinicalCoverSheet set FollowUp='" + txtFollowUp.Text + "',Proposals='" + txtPreposals.Text + "',PgmCord='" + txtPgmCordntr.Text + "',PgmCordDate='" + txtDatePgnCord.Text + "',EduCord='" + txteduCodntr.Text + "',EduCordDate='" + txtDateEduCord.Text + "',BCBA='" + txtBCBA.Text + "',BCBADate='"+txtDateBCBACord.Text+"'";

            string strquery = "update StdtClinicalCoverSheet set FollowUp='" + txtFollowUp.Text + "',Proposals='" + txtPreposals.Text + "',PgmCord='" + txtPgmCordntr.Text + "',PgmCordDate='" + txtDatePgnCord.Text + "',EduCord='" + txteduCodntr.Text + "',EduCordDate='" + txtDateEduCord.Text + "',BCBA='" + txtBCBA.Text + "',BCBADate='" + txtDateBCBACord.Text + "' where ClinicalCvId="+int.Parse(hdFldCvid.Value);
            int cvrsheetId = objData.Execute(strquery);


            strquery = " delete from clvAsmtTool where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvRecChange where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvReinfoSur where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);

            foreach (GridViewRow gdVr in gVAssmntTool.Rows)
            {
                if ((((TextBox)gdVr.FindControl("txtTargetBehavior")).Text != "") && (((TextBox)gdVr.FindControl("txtFunction")).Text != "") && (((TextBox)gdVr.FindControl("txtAnalysisTools")).Text != ""))
                {
                    strquery = "insert into clvAsmtTool(StdtCoverSheetId,TrgetBehav,Functions,AnalysisTool) values(" + int.Parse(hdFldCvid.Value) + ",'" + ((TextBox)gdVr.FindControl("txtTargetBehavior")).Text + "','" + ((TextBox)gdVr.FindControl("txtFunction")).Text + "','" + ((TextBox)gdVr.FindControl("txtAnalysisTools")).Text + "')";
                    objData.Execute(strquery);
                }
            }
            //inserting Reinforcment  insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values()

            foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
            {
                if ((((TextBox)gdVr.FindControl("txtDate")).Text != "") && (((TextBox)gdVr.FindControl("txtToolUtilized")).Text != ""))
                {
                    strquery = "insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values(" + int.Parse(hdFldCvid.Value) + ",'" + ((TextBox)gdVr.FindControl("txtDate")).Text + "','" + ((TextBox)gdVr.FindControl("txtToolUtilized")).Text + "')";
                    objData.Execute(strquery);
                }

            }
            //inserting Rec Changes
            foreach (GridViewRow gdVr in gVRecChange.Rows)
            {
                if ((((TextBox)gdVr.FindControl("txtRecomd")).Text != "") && (((TextBox)gdVr.FindControl("txtTimeLine")).Text != "") && (((TextBox)gdVr.FindControl("txtPerRes")).Text != ""))
                {
                    strquery = "insert into clvRecChange(StdtCoverSheetId,Recomendation,TimeLine,PersonResponsible) values(" + int.Parse(hdFldCvid.Value) + ",'" + ((TextBox)gdVr.FindControl("txtRecomd")).Text + "','" + ((TextBox)gdVr.FindControl("txtTimeLine")).Text + "','" + ((TextBox)gdVr.FindControl("txtPerRes")).Text + "')";
                    objData.Execute(strquery);
                }
            }

            if (cvrsheetId > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Details Updated Successfully");
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Details Updation Failed");
            }
        }
    }


    protected void btnLoadDate_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        if (btnLoadDate.Text == "Load Report")
        {
            clearDatas();
            PanelMain.Visible = true;
            drpSelectDate.Enabled = false;
            btnLoadDate.Text = "Select New Date";

            dtAssmtReinfo.Columns.Add("Date");
            dtAssmtReinfo.Columns.Add("ToolUtilized");

            dtRecChang.Columns.Add("Recommendation");
            dtRecChang.Columns.Add("Timeline");
            dtRecChang.Columns.Add("Person Responsible");

            dtAssmntTool.Columns.Add("TargetBehavior");
            dtAssmntTool.Columns.Add("Function");
            dtAssmntTool.Columns.Add("AnalysisTools");

            loadAssmtReinfo();
            loadRecChange();
            loadAssmntTool();
            objData = new clsData();
            // date for sected date
            if (drpSelectDate.SelectedItem.Text == DateTime.Now.ToString("MM'/'dd'/'yyyy"))
            {
                if (objData.IFExists("select ClinicalCvId from StdtClinicalCoverSheet where CAST( EndDate as date)= cast(GETDATE() as date) and SchoolId='"+sess.SchoolId+"' and StudentId='"+sess.StudentId+"'") == false)
                {

                    loadData();
                    btnSave.Text = "Save";
                }
                else
                {
                    loadData();
                    loadDataToday();
                    btnSave.Text = "Update";
                }
            }
            else
            {
                if (objData.IFExists("select ClinicalCvId from StdtClinicalCoverSheet where CAST( EndDate as date)= cast('" + drpSelectDate.SelectedItem.Text + "' as date) and SchoolId='" + sess.SchoolId + "' and StudentId='" + sess.StudentId + "'") == true)
                {

                    loadData();//this is to load behavior
                    loadDataToday();
                    btnSave.Text = "Update";
                }
            }
        }
        else
        {         

            PanelMain.Visible = false;
            drpSelectDate.Enabled = true;
            btnLoadDate.Text = "Load Report";

        }

    }
    protected void btnTodayzReport_Click(object sender, EventArgs e)
    {
        clearDatas();
        PanelMain.Visible = true;
        drpSelectDate.SelectedIndex = 0;
        drpSelectDate.Enabled = false;

        dtAssmtReinfo.Columns.Add("Date");
        dtAssmtReinfo.Columns.Add("ToolUtilized");

        dtRecChang.Columns.Add("Recommendation");
        dtRecChang.Columns.Add("Timeline");
        dtRecChang.Columns.Add("Person Responsible");

        dtAssmntTool.Columns.Add("TargetBehavior");
        dtAssmntTool.Columns.Add("Function");
        dtAssmntTool.Columns.Add("AnalysisTools");

        loadAssmtReinfo();
        loadRecChange();
        loadAssmntTool();
        objData = new clsData();
        if (objData.IFExists("select ClinicalCvId from StdtClinicalCoverSheet where CAST( EndDate as date)= cast(GETDATE() as date) and SchoolId='" + sess.SchoolId + "' and StudentId='" + sess.StudentId + "'") == false)
        {

            loadData();
            btnSave.Text = "Save";
        }
        else
        {          
            loadDataToday();

            loadData();
            btnSave.Text = "Update";
        }
    }



    public void SearchAndReplace(string document)
    {
        int m = 0;

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        {
            string docText = null;
            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();
            }
            string col = "";
            string plc = "";




            for (int i = 0; i < columns.Length; i++)
            {
                plc = placeHolders[i].ToString().Trim();
                col = columns[i].ToString().Trim();


                Regex regexText = new Regex(plc);
                docText = regexText.Replace(docText, col);



            }

            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(docText);
            }

        }
    }
    private string CopyTemplate(string oldPath, string PageNo)
    {
        try
        {
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\StudentBinder") + "\\Temp2\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string newpath = path + "\\";
            string newFileName = "ClvCovSheet" + PageNo;
            FileInfo f1 = new FileInfo(oldPath);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }

                f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
            }
            return newpath + newFileName + f1.Extension;
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg(Ex.Message.ToString() + "Directory or File already Exit !");
            return "";
        }
    }
    private void CreateQuery(string StateName, string Path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");

        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                columns = new string[xmlListColumns.Count];
                placeHolders = new string[xmlListColumns.Count];
                columnsToAdd = new string[xmlListColumns.Count];



                int i = 0, j = 0;
                foreach (XmlNode stMs in xmlListColumns)
                {
                    columns[i] = stMs.Attributes["Column"].Value;
                    columnsToAdd[i] = stMs.Attributes["Column"].Value;
                    i++;
                }
                foreach (XmlNode stMs in xmlListColumns)
                {
                    placeHolders[j] = stMs.Attributes["PlaceHolder"].Value;
                    j++;
                }

            }
        }

    }






    private void AllInOne()
    {
        tblAppnd = new string[1];

         sess = (clsSession)Session["UserSession"];
        string Path = "";
        string NewPath = "";
        Dt = new System.Data.DataTable();
        clsClinicalCoverSheet objExport = new clsClinicalCoverSheet();
        try
        {

            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp2\\";
            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }



            int pageCount = 0;
            Path = Server.MapPath("~\\StudentBinder\\CsTemplatesTest\\ClinicalTemplate1.docx");
            NewPath = CopyTemplate(Path, pageCount.ToString());

            CreateQuery("NE", "XMLCS\\CsXMLHeader.xml");
            Dt = objExport.getHeaderValz(StudentId, SchoolId, int.Parse(hdFldCvid.Value));
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < Dt.Columns.Count; i++)
                    {
                        columns[i] = dr[columnsToAdd[i]].ToString();
                    }
                   
                }
            }
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
            }         
           
            Dt = objExport.getClinicalBehavior(StudentId, SchoolId, drpSelectDate.SelectedItem.Text);
            AppndTableBehav(NewPath, Dt);

            CreateQuery("NE", "XMLCS\\CS1Creation.xml");
            /*
            int i = 0;
            int iCounter = 0;
            Dt = objExport.getHeaderValz(StudentId, SchoolId,int.Parse(hdFldCvid.Value) );
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    for (i = 0; i < Dt.Columns.Count; i++)
                   {
                       columns[i] = dr[columnsToAdd[i]].ToString();
                   }
                   iCounter = i;
                }
            }

            Dt = objExport.getClinicalBehavior(StudentId, SchoolId, drpSelectDate.SelectedItem.Text);

            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    
                        columns[i] = dr[columnsToAdd[i]].ToString();
                    
                    iCounter = i;
                }
            }
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
            }

            pageCount++;

            */
          

           

            if (Dt.Rows.Count > 0)
            {
                //foreach (DataRow dr in Dt.Rows)
                //{
                //    for (int i = 0; i < placeHolders.Length; i++)
                //    {

                //        columns[i] = dr[columnsToAdd[i]].ToString();
                //    }


                    
                //    if (NewPath != "")
                //    {
                //        SearchAndReplace(NewPath);
                //    }

                //    pageCount++;
                   
                //}
                pageCount++;

               
            }
            

            //apptable Page 2

            CreateQuery("NE", "XMLCS\\CS2Creation.xml");
          
            Path = Server.MapPath("~\\StudentBinder\\CsTemplatesTest\\ClinicalTemplate2.docx");
            NewPath = CopyTemplate(Path, pageCount.ToString());

            CreateQuery("NE", "XMLCS\\CsXMLHeader.xml");
            Dt = objExport.getHeaderValz(StudentId, SchoolId, int.Parse(hdFldCvid.Value));
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < Dt.Columns.Count; i++)
                    {
                        columns[i] = dr[columnsToAdd[i]].ToString();
                    }

                }
            }
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
            }

            Dt = objExport.getClinicalCvSheetAsmtTool(StudentId, SchoolId,int.Parse( hdFldCvid.Value));
            AppndTableAssmtTool(NewPath, Dt);


            Dt = objExport.getClinicalCvReinfo(StudentId, SchoolId,int.Parse( hdFldCvid.Value));
            AppndTableReinfo(NewPath, Dt);
            pageCount++;

            /*
            foreach (DataRow dr in Dt.Rows)
            {
                for (int i = 0; i < placeHolders.Length; i++)
                {
                    
                        columns[i] = dr[columnsToAdd[i]].ToString();
                    
                }


                Path = Server.MapPath("~\\StudentBinder\\ASTemplates\\ASTemplates1.docx");
                NewPath = CopyTemplate(Path, pageCount.ToString());
                if (NewPath != "")
                {
                    SearchAndReplace(NewPath);
                }

                pageCount++;
            }
            */

            
           

            Path = Server.MapPath("~\\StudentBinder\\CsTemplatesTest\\ClinicalTemplate3.docx");
            NewPath = CopyTemplate(Path, pageCount.ToString());
            CreateQuery("NE", "XMLCS\\CsXMLHeader.xml");
            Dt = objExport.getHeaderValz(StudentId, SchoolId, int.Parse(hdFldCvid.Value));
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < Dt.Columns.Count; i++)
                    {
                        columns[i] = dr[columnsToAdd[i]].ToString();
                    }

                }
            }
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
            }


            Dt = objExport.getClinicalCvRecChange(StudentId, SchoolId, int.Parse(hdFldCvid.Value));
            AppndTableRecChange(NewPath, Dt);

            Dt = objExport.getClinicalCvSheet(StudentId, SchoolId, int.Parse(hdFldCvid.Value));

            CreateQuery("NE", "XMLCS\\CS3Creation.xml");

            if (Dt.Rows.Count > 0)
            {
                int tblCount=0;
              
                foreach (DataRow dr in Dt.Rows)
                {
                    for ( int i = 0; i < placeHolders.Length; i++)
                    {
                        if (columnsToAdd[i] != "TableApp")
                        {
                            columns[i] = dr[columnsToAdd[i]].ToString();
                        }
                         else if(columnsToAdd[i] == "TableApp")
                        {
                            tblAppnd[tblCount] = placeHolders[i];
                        }
                    }
                                      
                   
                }
                 if (NewPath != "")
                    {
                        SearchAndReplace(NewPath);
                    }
               /*  if (tblAppnd.Length > 0)
                 {
                     Dt = objExport.getClinicalCvSheetAsmtTool(StudentId, SchoolId, int.Parse(hdFldCvid.Value));
                     DocumentFormat.OpenXml.Wordprocessing.Table tableXmlchk = new DocumentFormat.OpenXml.Wordprocessing.Table();
                     tableXmlchk = CreateTable(Dt);
                  //   replaceWithHtml(NewPath, tblAppnd[0], tableXml);
                     Path = Server.MapPath("~\\StudentBinder\\CsTemplates\\test.docx");
                     replaceWithHtml(Path, "plcTest", tableXmlchk);


                 }*/
                    pageCount++;

            }



            Path = Server.MapPath("~\\StudentBinder\\CsTemplatesTest\\ClinicalTemplate4.docx");
            NewPath = CopyTemplate(Path, pageCount.ToString());

            CreateQuery("NE", "XMLCS\\CsXMLHeader.xml");
            Dt = objExport.getHeaderValz(StudentId, SchoolId, int.Parse(hdFldCvid.Value));
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < Dt.Columns.Count; i++)
                    {
                        columns[i] = dr[columnsToAdd[i]].ToString();
                    }

                }
            }
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
               // getheader(NewPath);
            }
            CreateQuery("NE", "XMLCS\\CS4Creation.xml");
            Dt = objExport.getClinicalCvSheet(StudentId, SchoolId, int.Parse(hdFldCvid.Value));
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < placeHolders.Length; i++)
                    {
                            columns[i] = dr[columnsToAdd[i]].ToString();
                       
                    }


                    if (NewPath != "")
                    {
                        SearchAndReplace(NewPath);
                    }

                    pageCount++;
                }

            }

           

            MergeFiles();
           // TestMergeFiles();

            tdMsg.InnerHtml = clsGeneral.sucessMsg("Academic Coversheet Documents Sucessfully Created....!");

        }
        catch (Exception eX)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
        }
    }



    public void convertDrRowToOpenXmlRow(System.Data.DataRow dRow)
    {

    }

   
   // public
    public void replaceWithHtml(string fileName, string replace, DocumentFormat.OpenXml.Wordprocessing.Table tableOpenXml)
    {

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);
            Body body = mainPart.Document.Body;

            //     converter.ConsiderDivAsParagraph = false;

            //     SpacingBetweenLines spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
            ParagraphProperties paragraphProperties = new ParagraphProperties();

            //paragraphProperties.Append(spacing);

            paragraphProperties.RemoveAllChildren<ParagraphStyleId>();
            paragraphProperties.RemoveAllChildren<SpacingBetweenLines>();

            try
            {
                var placeholder = mainPart.Document.Body
                  .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                  .Where(t => t.Text.Contains(replace)).First();

               // var paragraphs = converter.Parse(replaceTest);




             //   for (int i = 0; i < paragraphs.Count; i++)
              //  {
                    var parent = placeholder.Parent;
              //      paragraphs[i].Append(paragraphProperties);
                    parent.ReplaceChild(tableOpenXml, placeholder);
              //  }
                mainPart.Document.Save();
            }
            catch (Exception eX)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
            }


        }
    }

    public void MergeFiles()
    {
        try
        {
            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp2\\";
            const string DOC_URL = "/word/document.xml";

            string FolderName = "\\AcademicSheet_{0:ddMMyy}-{0:HHmmss}";
            FolderName = string.Format(FolderName, DateTime.Now);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + FolderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string OUTPUT_FILE = path + "\\AcademicSheet_{0:ddMMyy}-{0:HHmmss}.docx";
            string FIRST_PAGE = Server.MapPath("~\\StudentBinder\\ASTemplates\\Dummy.docx");

            string fileName = string.Format(OUTPUT_FILE, DateTime.Now);
            File.Copy(FIRST_PAGE, fileName);
            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))

            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            //{
            //    //MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            //    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            //    DocumentFormat.OpenXml.Wordprocessing.Document d = new DocumentFormat.OpenXml.Wordprocessing.Document();
            //    HeaderPart hp = mainPart.AddNewPart<HeaderPart>();
            //    string relId = mainPart.GetIdOfPart(hp);
            //    SectionProperties sectPr = new SectionProperties();
            //    HeaderReference headerReference = new HeaderReference();
            //    headerReference.Id = relId;
            //    sectPr.Append(headerReference);

            //    d.Append(sectPr);
            //    mainPart.Document = d;
            //   mainPart.Document.Save();
            //    hp.Header = GetHeader();
            //    hp.Header.Save();

            //    wordDoc.Close();
            //}

            using (Package package = Package.Open(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                PackagePart docPart = package.GetPart(new Uri(DOC_URL, UriKind.Relative));

                XmlDocument document = new XmlDocument();

                
                
                document.Load(docPart.GetStream());

                XmlNamespaceManager nsm = new XmlNamespaceManager(document.NameTable);
                nsm.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                XmlNode body = document.SelectSingleNode("/w:document/w:body", nsm);


                
                string[] filePaths = Directory.GetFiles(Temp);

                foreach (string filePath in filePaths)
                {
                    using (Package lastPackage = Package.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        XmlDocument lastPage = new XmlDocument();
                        lastPage.Load(lastPackage.GetPart(new Uri(DOC_URL, UriKind.Relative)).GetStream());

                        XmlNode last = body.LastChild;
                        foreach (XmlNode childNode in lastPage.SelectSingleNode("/w:document/w:body", nsm).ChildNodes)
                        {
                            XmlNode docChildNode = document.ImportNode(childNode, true);
                            body.InsertAfter(docChildNode, last);
                            last = docChildNode;
                        }
                    }

                    document.Save(docPart.GetStream(FileMode.Create, FileAccess.Write));

                }


                

            }
            


            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }

        }
        catch (Exception Ex)
        {

        }
    }

    public Header GetHeader()
    {
        Header h = new Header();
        DocumentFormat.OpenXml.Wordprocessing.Paragraph p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
        Run r = new Run();
        Text t = new Text();
        t.Text = "This is the header.";
        r.Append(t);
        p.Append(r);
        h.Append(p);
        return h;
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        AllInOne();
    }



    //************** Table Appending functions **************

    public void AppndTableBehav(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                if (tablecounter == 2)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell6 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell7 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell8 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell9 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell10 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(" "))));

                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);
                        trXml.Append(tblCell4);
                        trXml.Append(tblCell5);
                        trXml.Append(tblCell6);
                        trXml.Append(tblCell7);
                        trXml.Append(tblCell8);
                        trXml.Append(tblCell9);
                        trXml.Append(tblCell10);
                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tablecounter++;
            }

            mainPart.Document.Save();

        }

    }

    public void AppndTableAssmtTool(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                if (tablecounter == 3)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[2].ToString()))));


                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);

                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tablecounter++;
            }

            mainPart.Document.Save();

        }

    }

    public void AppndTableReinfo(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tableCount = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                if (tableCount == 4)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));


                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);

                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tableCount++;
            }

            mainPart.Document.Save();

        }

    }

    public void AppndTableRecChange(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tableCounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                
                foreach (DataRow dr in XmlAppDatatable.Rows)
                {
                    if (tableCounter == 4)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[2].ToString()))));

                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);

                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tableCounter++;
            }

            mainPart.Document.Save();

        }

    }
    //*****************************************************

 
    //*****CreateDoc******
    /*
    public void CreateDocument()
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Create("test.docx", WordprocessingDocumentType.Document))
        {
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            Document d = new Document();
            Body b = new Body();
            Table tbl = new Table();
            TableProperties tblPr = new TableProperties();
            TableCellMarginDefault tblCellMar = new TableCellMarginDefault();
            tblCellMar.TopMargin = new TopMargin { Width = 0, Type = TableWidthUnitValues.Dxa };
            tblCellMar.RightMargin = new RightMargin { Width = 120, Type = TableWidthUnitValues.Dxa };
            tblCellMar.BottomMargin = new BottomMargin { Width = 0, Type = TableWidthUnitValues.Dxa };
            tblCellMar.LeftMargin = new LeftMargin { Width = 120, Type = TableWidthUnitValues.Dxa };
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            Paragraph p = new Paragraph();
            Run r = new Run();
            Text t = new Text();
            t.Text = "This is text in a table cell.";
            r.Append(t);
            p.Append(r);
            tc.Append(p);
            tr.Append(tc);
            tblPr.Append(tblCellMar);
            tbl.Append(tblPr);
            tbl.Append(tr);
            b.Append(tbl);
            d.Append(b);

            HeaderPart hp = mainPart.AddNewPart<HeaderPart>();
            string relId = mainPart.GetIdOfPart(hp);
            SectionProperties sectPr = new SectionProperties();
            HeaderReference headerReference = new HeaderReference();
            headerReference.Id = relId;
            sectPr.Append(headerReference);

            d.Append(sectPr);
            mainPart.Document = d;
            mainPart.Document.Save();
            hp.Header = GetHeader();
            hp.Header.Save();

            wordDoc.Close();
        }
    }*/
    //********************

    //************* Test Mearge ************
    public void TestMergeFiles()
    {
        try
        {
            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp2\\";
            const string DOC_URL = "/word/document.xml";

            string FolderName = "\\AcademicSheet_{0:ddMMyy}-{0:HHmmss}";
            FolderName = string.Format(FolderName, DateTime.Now);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + FolderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string OUTPUT_FILE = path + "\\AcademicSheet_{0:ddMMyy}-{0:HHmmss}.docx";
            string FIRST_PAGE = Server.MapPath("~\\StudentBinder\\ASTemplates\\Dummy.docx");

            string fileName = string.Format(OUTPUT_FILE, DateTime.Now);
            // File.Copy(FIRST_PAGE, fileName);
            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            {
                //MainDocumentPart mainPart = wordDoc.MainDocumentPart;


                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                DocumentFormat.OpenXml.Wordprocessing.Document d = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body b = new Body();
                DocumentFormat.OpenXml.Wordprocessing.Table tbl = new DocumentFormat.OpenXml.Wordprocessing.Table();
                DocumentFormat.OpenXml.Wordprocessing.TableProperties tblPr = new DocumentFormat.OpenXml.Wordprocessing.TableProperties();
                DocumentFormat.OpenXml.Wordprocessing.TableCellMarginDefault tblCellMar = new DocumentFormat.OpenXml.Wordprocessing.TableCellMarginDefault();

                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                DocumentFormat.OpenXml.Wordprocessing.TableCell tc = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                DocumentFormat.OpenXml.Wordprocessing.Paragraph p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                Run r = new Run();
                Text t = new Text();
               
                
                t.Text = "This is text in a table cell.";
               
               // t.Text = docText;

               // r.Append(t);
                p.Append(r);
                tc.Append(p);
                tr.Append(tc);
                tblPr.Append(tblCellMar);
                tbl.Append(tblPr);
                tbl.Append(tr);
                b.Append(tbl);
                d.Append(b);

                HeaderPart hp = mainPart.AddNewPart<HeaderPart>();
                string relId = mainPart.GetIdOfPart(hp);
                SectionProperties sectPr = new SectionProperties();
                HeaderReference headerReference = new HeaderReference();
                headerReference.Id = relId;
                sectPr.Append(headerReference);

                d.Append(sectPr);
                mainPart.Document = d;
                mainPart.Document.Save();
                hp.Header = GetHeader();
                hp.Header.Save();

                wordDoc.Close();
            }

            string docText = "";
            int i = 0;
            string[] filePaths = Directory.GetFiles(Temp);
            foreach (string filePath in filePaths)
            {
                // using (WordprocessingDocument wordDoc1 = WordprocessingDocument.Open(fileName, false))
                using (WordprocessingDocument wordDoc2 = WordprocessingDocument.Open(filePath, true))
                {
                  //  XElement tempBody = XElement.Parse(tempDocument.MainDocumentPart.Document.Body.OuterXml);
                 //   XmlElement tempbody=wordDoc2.MainDocumentPart.CustomXmlParts<>;
                    var main = wordDoc2.MainDocumentPart;

                    //replace the fields in the main document
                   // main.Document.InnerXml.

                    using (StreamReader sr = new StreamReader(wordDoc2.MainDocumentPart.GetStream()))
                    {
                      
                        docText += sr.ReadToEnd();
                    }


                    

                }
                i++;
            }
            using (WordprocessingDocument wordDoc3 = WordprocessingDocument.Open(fileName, true))
            {
                
                using (StreamWriter sw = new StreamWriter(wordDoc3.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
            //MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            //DocumentFormat.OpenXml.Wordprocessing.Document d = new DocumentFormat.OpenXml.Wordprocessing.Document();
            //Body b = new Body();
            //HeaderPart hp = mainPart.AddNewPart<HeaderPart>();
            //string relId = mainPart.GetIdOfPart(hp);
            //SectionProperties sectPr = new SectionProperties();
            //HeaderReference headerReference = new HeaderReference();
            //headerReference.Id = relId;
            //sectPr.Append(headerReference);


            //d.Append(b);

            //d.Append(sectPr);
            //mainPart.Document = d;
            //mainPart.Document.Save();
            //hp.Header = GetHeader();
            //hp.Header.Save();

            //wordDoc.Close();

        //   public static void Merge(string document1, string document2, string mergedDocPath)
        //{
        //    var combinedSourceList = new List<Source>();

        //    using (var document1XmlDoc = WordprocessingDocument.Open(document1, false))
        //    using (var document2OpenXmlDoc = WordprocessingDocument.Open(document2, false))
        //    {
        //        var analysisSource = new Source(document1XmlDoc, false);
        //        var detailsSource = new Source(document2OpenXmlDoc, true);

        //        combinedSourceList.Add(analysisSource);
        //        combinedSourceList.Add(detailsSource);
                
        //        // Build and merge the source documents
        //        DocumentBuilder.BuildDocument(combinedSourceList, mergedDocPath);
        //    }
        //}


            //string[] filePaths = Directory.GetFiles(Temp);


            //foreach (string filePath in filePaths)
            //{
            //    using (WordprocessingDocument wordDoc1 = WordprocessingDocument.Open(fileName, false))
            //    using (WordprocessingDocument wordDoc2 = WordprocessingDocument.Open(filePath, true))
            //    {
            //        using (StreamReader sr = new StreamReader(wordDoc2.MainDocumentPart.GetStream()))
            //        {
            //            docText = sr.ReadToEnd();
            //        }
            //       // ThemePart themePart1 = wordDoc1.MainDocumentPart.ThemePart;
            //      //  ThemePart themePart2 = wordDoc2.MainDocumentPart.ThemePart;

            //        using (StreamReader streamReader = new StreamReader(wordDoc1.MainDocumentPart.GetStream()))
            //        using (StreamWriter streamWriter = new StreamWriter(wordDoc2.MainDocumentPart.GetStream(FileMode.Create)))
            //        {
            //            streamWriter.Write(streamReader.ReadToEnd());
            //        }
            //    }
            //}




            /*  using (Package package = Package.Open(fileName, FileMode.Open, FileAccess.ReadWrite))
              {
                  PackagePart docPart = package.GetPart(new Uri(DOC_URL, UriKind.Relative));

                  XmlDocument document = new XmlDocument();



                  document.Load(docPart.GetStream());

                  XmlNamespaceManager nsm = new XmlNamespaceManager(document.NameTable);
                  nsm.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                  XmlNode body = document.SelectSingleNode("/w:document/w:body", nsm);



              




              }
              */


            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }

        }

        catch (Exception Ex)
        {

        }
    }
    //**************************************

}