using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Administration_AssessmentsAndLPMappings : System.Web.UI.Page
{
    DataClass objDataClass = new DataClass();
    clsData objData = null;
    clsSession sess = null;
    bool Disable = false;
    string[] LP = null;
    public string sqlAssessment, selAssessment, AsmntName, AsmntCat, AsmntSubCat, AsmntQId, LPcnt;
    public string active;
    public int goalid;
    public string sqlAssessmentmap, selAssessmentmap, AsmntNamemap, AsmntCatmap, LPcntmap;
    public int goalidmap;
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
        grd_assessmentQues.PageSize = sess.GridPagingSize;
        grd_LPassessmentQues.PageSize = sess.GridPagingSize;
        if (!IsPostBack)
        {

            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnAdd.Visible = false;
                btnAssessAdd.Visible = false;
            }
            else
            {
                btnAdd.Visible = true;
                btnAssessAdd.Visible = true;
            }

            tdMsg.InnerHtml = "";
            filldropassess();
            lbtnAll.ForeColor = System.Drawing.Color.Red;
            FillforPage();
            filldropassessMAP();
            lbtnLPAll.ForeColor = System.Drawing.Color.Red;
            FillforPageMAP();
            viewTab1();


        }
    }



    public void FillAssessment()
    {
        grd_assessmentQues.DataSource = null;
        grd_assessmentQues.DataBind();
        string Assessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' ORDER BY asmt.AsmntName,asmt.SortOrder";
        //string Assessment = "Select AsmntName,AsmntCat,AsmntSubCat,AsmntQId,G.GoalName from AsmntLPRel ALR Inner Join Goal G On ALR.GoalId=G.GoalId where AsmntQId In(Select Distinct AsmntQId from AsmntLPRel) And ALR.ActiveInd='A'  Group By AsmntName,AsmntCat,AsmntSubCat,AsmntQId,G.GoalName ORDER BY AsmntName,AsmntCat,AsmntSubCat,AsmntQId,G.GoalName";
        DataTable dlassess = objDataClass.fillData(Assessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_assessmentQues.DataSource = dlassess;
            grd_assessmentQues.DataBind();
        }
    }
    public void FillMappedAssessment()
    {
        grd_assessmentQues.DataSource = null;
        grd_assessmentQues.DataBind();
        string MappedAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName<>'' and asmt.ActiveInd='A' ORDER BY asmt.AsmntName,asmt.SortOrder";
        DataTable dlassess = objDataClass.fillData(MappedAssessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_assessmentQues.DataSource = dlassess;
            grd_assessmentQues.DataBind();
        }
    }
    public void FillUnmappedAssessment()
    {
        grd_assessmentQues.DataSource = null;
        grd_assessmentQues.DataBind();
        //string UnmappedAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where ISNULL(asmt.LessonPlanName,'')='' and asmt.ActiveInd='A' ORDER BY asmt.AsmntName,asmt.SortOrder";
        string UnmappedAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName is null and asmt.ActiveInd='A' except (select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.LessonPlanName<>'') ORDER BY asmt.AsmntName,asmt.SortOrder";
        DataTable dlassess = objDataClass.fillData(UnmappedAssessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_assessmentQues.DataSource = dlassess;
            grd_assessmentQues.DataBind();
        }
    }
    public void FillLessonplansearch()
    {
        grd_assessmentQues.DataSource = null;
        grd_assessmentQues.DataBind();
        if (lbtnAll.ForeColor == System.Drawing.Color.Red)
            selAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.LessonPlanName like +'%'+'" + clsGeneral.convertQuotes(txtassessment.Text.Trim()) + "'+'%' ORDER BY asmt.AsmntName,asmt.SortOrder ";
        else if (lbtnmapped.ForeColor == System.Drawing.Color.Red)
            selAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.LessonPlanName like +'%'+'" + clsGeneral.convertQuotes(txtassessment.Text.Trim()) + "'+'%' ORDER BY asmt.AsmntName,asmt.SortOrder";
        else if (lbtnunmapped.ForeColor == System.Drawing.Color.Red)
            selAssessment = "select asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntLPRelId,asmt.AsmntQId,gol.GoalName from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where (asmt.LessonPlanName is null OR asmt.LessonPlanName='')  and asmt.LessonPlanName like +'%'+'" + clsGeneral.convertQuotes(txtassessment.Text.Trim()) + "'+'%' and asmt.ActiveInd='A' ORDER BY asmt.AsmntName,asmt.SortOrder";
        DataTable dlassess = objDataClass.fillData(selAssessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_assessmentQues.DataSource = dlassess;
            grd_assessmentQues.DataBind();
        }
    }
    public void FillAssessmentLessonplansearch()     //GoalCode is used to display the Edited goals
    {
        grd_assessmentQues.DataSource = null;
        grd_assessmentQues.DataBind();
        if (lbtnAll.ForeColor == System.Drawing.Color.Red)
            selAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,gol.GoalCode,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.LessonPlanName like +'%'+'" + txtassessment.Text.Trim() + "'+'%' and asmt.AsmntName ='" + ddlAssessment.SelectedItem.Text.Trim() + "' ORDER BY asmt.AsmntName,asmt.SortOrder";
        else if (lbtnmapped.ForeColor == System.Drawing.Color.Red)
            selAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,gol.GoalCode,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.LessonPlanName like +'%'+'" + txtassessment.Text.Trim() + "'+'%' and asmt.AsmntName='" + ddlAssessment.SelectedItem.Text.Trim() + "' ORDER BY asmt.AsmntName,asmt.SortOrder";
        else if (lbtnunmapped.ForeColor == System.Drawing.Color.Red)
            selAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,gol.GoalCode,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName is null and asmt.ActiveInd='A' and asmt.LessonPlanName like +'%'+'" + txtassessment.Text.Trim() + "'+'%' and asmt.AsmntName='" + ddlAssessment.SelectedItem.Text.Trim() + "' except (select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.LessonPlanName<>'') ORDER BY asmt.AsmntName,asmt.SortOrder";
        DataTable dlassess = objDataClass.fillData(selAssessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_assessmentQues.DataSource = dlassess;
            grd_assessmentQues.DataBind();
        }
    }
    private void BindGrid(GridView GridView, string question, string AsmtName, string category, string subcategory)
    {
        string Sql;
        if (txtassessment.Text == "")
            Sql = "Select LessonPlanName,AsmntLPRelId  from AsmntLPRel where AsmntQId ='" + question + "' and AsmntName='" + AsmtName + "' and AsmntCat='" + category + "' and ISNULL(AsmntSubCat,'')='" + subcategory + "' and LessonPlanName<>'' and ActiveInd='A' ORDER BY AsmntName,SortOrder";
        else
            Sql = "Select LessonPlanName,AsmntLPRelId  from AsmntLPRel where AsmntQId ='" + question + "' and AsmntName='" + AsmtName + "' and AsmntCat='" + category + "' and ISNULL(AsmntSubCat,'')='" + subcategory + "' and LessonPlanName like +'%'+'" + txtassessment.Text.Trim() + "'+'%' and ActiveInd='A' ORDER BY AsmntName,SortOrder";
        DataTable gridLesson = objDataClass.fillData(Sql);
        if (gridLesson.Rows.Count > 0)
        {
            GridView.Visible = true;
            GridView.DataSource = gridLesson;
            GridView.DataBind();
        }
        else
            GridView.Visible = false;
    }


    public void FillAssessmentsearch()
    {
        grd_assessmentQues.DataSource = null;
        grd_assessmentQues.DataBind();
        if (lbtnAll.ForeColor == System.Drawing.Color.Red)
            sqlAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.AsmntName ='" + ddlAssessment.SelectedItem.Text.Trim() + "' ORDER BY asmt.AsmntName,asmt.SortOrder";
        else if (lbtnmapped.ForeColor == System.Drawing.Color.Red)
            sqlAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName<>'' and asmt.ActiveInd='A' and asmt.AsmntName='" + ddlAssessment.SelectedItem.Text.Trim() + "' ORDER BY asmt.AsmntName,asmt.SortOrder";
        else if (lbtnunmapped.ForeColor == System.Drawing.Color.Red)
            sqlAssessment = "select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName is null and asmt.ActiveInd='A' and asmt.AsmntName='" + ddlAssessment.SelectedItem.Text + "' except (select distinct asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,asmt.AsmntQId,gol.GoalName,asmt.SortOrder from AsmntLPRel asmt inner join Goal gol on asmt.GoalId=gol.GoalId where asmt.ActiveInd='A' and asmt.LessonPlanName<>'') ORDER BY asmt.AsmntName,asmt.SortOrder ";
        DataTable dlassess = objDataClass.fillData(sqlAssessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_assessmentQues.DataSource = dlassess;
            grd_assessmentQues.DataBind();
        }
    }
    private void filldropassess()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  AsmntName as Name FROM AsmntLPRel where ActiveInd='A' ORDER BY AsmntName", ddlAssessment);
    }
    private void filldroplessonplanview()
    {
        lstLP.Items.Clear();
        objData = new clsData();
        objData.ReturnListBox("SELECT LessonPlanName as Name,LessonPlanId as Id FROM LessonPlan where ActiveInd='A' and LessonPlanName like +'%'+'" + txtLP.Text.Trim() + "'+'%' ORDER BY LessonPlanName", lstLP);
    }
    protected void grd_assessmentQues_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_assessmentQues.PageIndex = e.NewPageIndex;
        if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text == ""))
        {
            FillAssessmentsearch();
        }
        else if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text != ""))
        {
            FillAssessmentLessonplansearch();
        }
        else if ((ddlAssessment.SelectedIndex == 0) && (txtassessment.Text != ""))
        {
            FillLessonplansearch();
        }
        else
        {
            FillforPage();
        }

    }

    private void FillforPage()
    {
        if (lbtnAll.ForeColor == System.Drawing.Color.Red)
            FillAssessment();
        else if (lbtnmapped.ForeColor == System.Drawing.Color.Red)
            FillMappedAssessment();
        else if (lbtnunmapped.ForeColor == System.Drawing.Color.Red)
            FillUnmappedAssessment();
    }

    protected void lbtnmapped_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = null;
        lbtnAll.ForeColor = System.Drawing.Color.Blue;
        lbtnunmapped.ForeColor = System.Drawing.Color.Blue;
        lbtnmapped.ForeColor = System.Drawing.Color.Red;
        if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text == ""))
        {
            FillAssessmentsearch();
        }
        else if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text != ""))
        {
            FillAssessmentLessonplansearch();
        }
        else if ((ddlAssessment.SelectedIndex == 0) && (txtassessment.Text != ""))
        {
            FillLessonplansearch();
        }
        else
        {
            FillforPage();
        }
    }
    protected void lbtnunmapped_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = null;
        lbtnAll.ForeColor = System.Drawing.Color.Blue;
        lbtnunmapped.ForeColor = System.Drawing.Color.Red;
        lbtnmapped.ForeColor = System.Drawing.Color.Blue;
        if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text == ""))
        {
            FillAssessmentsearch();
        }
        else if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text != ""))
        {
            FillAssessmentLessonplansearch();
        }
        else if ((ddlAssessment.SelectedIndex == 0) && (txtassessment.Text != ""))
        {
            FillLessonplansearch();
        }
        else
        {
            FillforPage();
        }
    }
    protected void lbtnAll_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = null;
        lbtnAll.ForeColor = System.Drawing.Color.Red;
        lbtnunmapped.ForeColor = System.Drawing.Color.Blue;
        lbtnmapped.ForeColor = System.Drawing.Color.Blue;
        if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text == ""))
        {
            FillAssessmentsearch();
        }
        else if ((ddlAssessment.SelectedIndex > 0) && (txtassessment.Text != ""))
        {
            FillAssessmentLessonplansearch();
        }
        else if ((ddlAssessment.SelectedIndex == 0) && (txtassessment.Text != ""))
        {
            FillLessonplansearch();
        }
        else
        {
            FillforPage();
        }
    }
    private int lessonplancnt(int asmtid)
    {
        string SelQry = "Select AsmntName,AsmntCat,AsmntSubCat,AsmntQId from AsmntLPRel where AsmntLPRelId='" + asmtid + "'";
        DataTable Dt = objDataClass.fillData(SelQry);
        if (Dt.Rows.Count > 0)
        {
            AsmntName = Dt.Rows[0]["AsmntName"].ToString().Trim();
            AsmntCat = Dt.Rows[0]["AsmntCat"].ToString().Trim();
            AsmntSubCat = Dt.Rows[0]["AsmntSubCat"].ToString().Trim();
            AsmntQId = Dt.Rows[0]["AsmntQId"].ToString().Trim();
        }
        LPcnt = "SELECT COUNT(LessonPlanName) FROM AsmntLPRel where AsmntName='" + AsmntName + "' and AsmntCat='" + AsmntCat + "' and AsmntQId='" + AsmntQId + "' and ISNULL(AsmntSubCat,'')='" + AsmntSubCat + "' and ActiveInd='A' and LessonPlanName<>''";
        int lessonplancnt = 0;
        lessonplancnt = objDataClass.ExecuteScalar(LPcnt);
        return lessonplancnt;
    }
    protected void grd_assessment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int AsmntLPRelId = Convert.ToInt32(e.CommandArgument.ToString());
            int lessoncnt = lessonplancnt(AsmntLPRelId);
            string deltlessonPlan;
            if (lessoncnt > 1)
            {
                deltlessonPlan = "DELETE from AsmntLPRel where AsmntLPRelId='" + AsmntLPRelId + "'";
            }
            else
            {
                deltlessonPlan = "Update AsmntLPRel set LessonPlanName='',LessonPlanId=0 where AsmntLPRelId='" + AsmntLPRelId + "'";
            }
            int retVal = objDataClass.ExecuteNonQuery(deltlessonPlan);
            if (retVal > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("LessonPlan Deleted Successfully");
                if (ddlAssessment.SelectedIndex == 0 && txtassessment.Text.Trim() != "")
                {
                    FillLessonplansearch();
                }
                else if (ddlAssessment.SelectedIndex > 0 && txtassessment.Text.Trim() == "")
                {
                    FillAssessmentsearch();
                }
                else if (ddlAssessment.SelectedIndex > 0 && txtassessment.Text.Trim() != "")
                {
                    FillAssessmentLessonplansearch();
                }
                else
                {
                    FillforPage();
                }
                FillforPageMAP();
            }
        }


    }
    protected void grd_assessmentQues_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Disable = false;
        sess = (clsSession)Session["UserSession"];
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        if (lbtnunmapped.ForeColor != System.Drawing.Color.Red)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gridResponses = (GridView)e.Row.FindControl("grd_assessment");
                Label question = (Label)e.Row.FindControl("lbl_question");
                Label AsmtName = (Label)e.Row.FindControl("lbl_assessname");
                Label category = (Label)e.Row.FindControl("lbl_category");
                Label subcategory = (Label)e.Row.FindControl("lbl_subcategory");
                BindGrid(gridResponses, question.Text, AsmtName.Text, category.Text, subcategory.Text);

                if (Disable == true)
                {
                    gridResponses.Columns[1].Visible = false;
                }
                else
                {
                    gridResponses.Columns[1].Visible = true;
                }
            }

        if (Disable == true)
        {
            grd_assessmentQues.Columns[6].Visible = false;
        }
        else
            grd_assessmentQues.Columns[6].Visible = true;

    }
    protected void grd_assessmentQues_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ADD")
            {
                lstLP.Items.Clear();
                txtLP.Text = string.Empty;
                lbl_msg.Text = string.Empty;
                GridViewRow GVRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label Assess = GVRow.Cells[1].FindControl("lbl_assessname") as Label;
                Label category = GVRow.Cells[2].FindControl("lbl_category") as Label;
                Label sub = GVRow.Cells[2].FindControl("lbl_subcategory") as Label;
                Label question = GVRow.Cells[2].FindControl("lbl_question") as Label;
                HiddenField sortorder = GVRow.Cells[2].FindControl("hdnsortorder") as HiddenField;
                lbl_assess.Text = Assess.Text;
                lbl_category.Text = category.Text;
                lbl_subcategory.Text = sub.Text;
                lbl_question.Text = question.Text;
                hdnsort.Value = sortorder.Value;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        if (lstLP.SelectedIndex != -1)
        {
            string LPString;
            LPString = "SELECT LessonPlanName FROM AsmntLPRel where AsmntName='" + lbl_assess.Text.Trim() + "' and AsmntCat='" + lbl_category.Text.Trim() + "' and AsmntQId='" + lbl_question.Text.Trim() + "' and ISNULL(AsmntSubCat,'')='" + lbl_subcategory.Text.Trim() + "' and LessonPlanName<>'' and ActiveInd='A' and ISNULL(SortOrder,'')='" + hdnsort.Value + "' ";
            DataTable DT = new DataTable();
            DT = objDataClass.fillData(LPString);
            int rowcnt = DT.Rows.Count;
            if (rowcnt > 0)
            {
                LP = new string[rowcnt];
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    LP[i] = DT.Rows[i]["LessonPlanName"].ToString().Trim();
                }
            }
            sess = (clsSession)Session["UserSession"];
            int loginid = sess.LoginId;
            DataTable Dt = new DataTable();
            string StrQry = "Select GoalId,ActiveInd from AsmntLPRel where AsmntName='" + lbl_assess.Text.Trim() + "' and AsmntCat='" + lbl_category.Text.Trim() + "' and AsmntQId='" + lbl_question.Text.Trim() + "' and ISNULL(AsmntSubCat,'')='" + lbl_subcategory.Text.Trim() + "' and ActiveInd='A' and ISNULL(SortOrder,'')='" + hdnsort.Value + "' ";
            Dt = objDataClass.fillData(StrQry);
            if (Dt.Rows.Count > 0)
            {
                goalid = Convert.ToInt32(Dt.Rows[0]["GoalId"].ToString().Trim());
                active = Dt.Rows[0]["ActiveInd"].ToString().Trim();
            }
            for (int i = lstLP.Items.Count - 1; i >= 0; i--)
            {
                if (lstLP.Items[i].Selected == true)
                {
                    object sortordr;
                    if (hdnsort.Value == "")
                    {
                        sortordr = null;
                    }
                    else
                    {
                        sortordr = Convert.ToInt32(hdnsort.Value);
                    }
                    if (LP != null)
                    {
                        if (LP.Contains(lstLP.Items[i].Text) == false)
                        {
                            string SQLQRY = "INSERT into AsmntLPRel(GoalId,AsmntName,AsmntCat,AsmntSubCat,AsmntQId,LessonPlanId,LessonPlanName,ActiveInd,SortOrder,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn)" +
                                              " VALUES ('" + goalid + "','" + lbl_assess.Text.Trim() + "','" + lbl_category.Text.Trim() + "','" + lbl_subcategory.Text.Trim() + "','" + lbl_question.Text.Trim() + "'," +
                                              "'" + Convert.ToInt32(lstLP.Items[i].Value) + "','" + lstLP.Items[i].Text + "','" + active + "','" + sortordr + "','" + loginid + "',GETDATE(),'" + loginid + "',GETDATE())";

                            objData = new clsData();
                            objData.Execute(SQLQRY);
                        }
                    }
                    else
                    {
                        string SQLQRY = "INSERT into AsmntLPRel(GoalId,AsmntName,AsmntCat,AsmntSubCat,AsmntQId,LessonPlanId,LessonPlanName,ActiveInd,SortOrder,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn)" +
                                              " VALUES ('" + goalid + "','" + lbl_assess.Text.Trim() + "','" + lbl_category.Text.Trim() + "','" + lbl_subcategory.Text.Trim() + "','" + lbl_question.Text.Trim() + "'," +
                                              "'" + Convert.ToInt32(lstLP.Items[i].Value) + "','" + lstLP.Items[i].Text + "','" + active + "','" + sortordr + "','" + loginid + "',GETDATE(),'" + loginid + "',GETDATE())";

                        objData = new clsData();
                        objData.Execute(SQLQRY);
                    }

                }
            }
            if (ddlAssessment.SelectedIndex == 0 && txtassessment.Text.Trim() != "")
            {
                FillLessonplansearch();
            }
            else if (ddlAssessment.SelectedIndex > 0 && txtassessment.Text.Trim() == "")
            {
                FillAssessmentsearch();
            }
            else if (ddlAssessment.SelectedIndex > 0 && txtassessment.Text.Trim() != "")
            {
                FillAssessmentLessonplansearch();
            }
            else
            {
                FillforPage();
            }
            FillforPageMAP();
            tdMsg.InnerHtml = clsGeneral.sucessMsg("LessonPlan Added Successfully");
        }



        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Search the lesson Plan before adding");
        }
    }
    protected void grd_assessment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        grd_assessmentQues.EditIndex = -1;
        grd_assessmentQues.DataBind();
    }


    public void FilllessonplanMAP()
    {
        grd_LPassessmentQues.DataSource = null;
        grd_LPassessmentQues.DataBind();
        string Assessment = "select distinct lp.LessonPlanName,lp.LessonPlanId from LessonPlan lp  Where lp.ActiveInd='A' and lp.LessonPlanName<>'' ORDER BY lp.LessonPlanName";
        DataTable dlassess = objDataClass.fillData(Assessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_LPassessmentQues.DataSource = dlassess;
            grd_LPassessmentQues.DataBind();
        }
    }
    public void FillMappedLPMAP()
    {
        grd_LPassessmentQues.DataSource = null;
        grd_LPassessmentQues.DataBind();
        string MappedAssessment = "select distinct lp.LessonPlanName,lp.LessonPlanId from AsmntLPRel asmt Inner join LessonPlan lp on asmt.LessonPlanId=lp.LessonPlanId Where asmt.ActiveInd='A' and lp.ActiveInd='A' and lp.LessonPlanName<>'' ORDER BY lp.LessonPlanName";
        DataTable dlassess = objDataClass.fillData(MappedAssessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_LPassessmentQues.DataSource = dlassess;
            grd_LPassessmentQues.DataBind();
        }
    }
    public void FillUnmappedLPMAP()
    {
        grd_LPassessmentQues.DataSource = null;
        grd_LPassessmentQues.DataBind();
        string UnmappedAssessment = "SELECT lp.LessonPlanName,lp.LessonPlanId FROM LessonPlan lp WHERE NOT EXISTS( SELECT asmt.LessonPlanName FROM AsmntLPRel asmt WHERE asmt.LessonPlanName = lp.LessonPlanName ) and lp.ActiveInd='A' ORDER BY lp.LessonPlanName";
        DataTable dlassess = objDataClass.fillData(UnmappedAssessment);
        if (dlassess.Rows.Count > 0)
        {
            grd_LPassessmentQues.DataSource = dlassess;
            grd_LPassessmentQues.DataBind();
        }
    }
    public void FillLessonplansearchMAP()
    {
        grd_LPassessmentQues.DataSource = null;
        grd_LPassessmentQues.DataBind();
        if (lbtnLPAll.ForeColor == System.Drawing.Color.Red)
            selAssessmentmap = "select distinct lp.LessonPlanName,lp.LessonPlanId from LessonPlan lp  Where lp.ActiveInd='A' and lp.LessonPlanName<>'' and lp.LessonPlanName like +'%'+'" + txtSearchLessonPlan.Text + "'+'%' ORDER BY lp.LessonPlanName";
        else if (lbtnLPmapped.ForeColor == System.Drawing.Color.Red)
            selAssessmentmap = "select distinct lp.LessonPlanName,lp.LessonPlanId from AsmntLPRel asmt Inner join LessonPlan lp on asmt.LessonPlanId=lp.LessonPlanId Where asmt.ActiveInd='A' and lp.ActiveInd='A' and lp.LessonPlanName<>'' and lp.LessonPlanName like +'%'+'" + txtSearchLessonPlan.Text + "'+'%' ORDER BY lp.LessonPlanName";
        else if (lbtnLPunmapped.ForeColor == System.Drawing.Color.Red)
            selAssessmentmap = "SELECT lp.LessonPlanName,lp.LessonPlanId FROM LessonPlan lp WHERE NOT EXISTS( SELECT asmt.LessonPlanName FROM AsmntLPRel asmt WHERE asmt.LessonPlanName = lp.LessonPlanName ) and lp.ActiveInd='A' and lp.LessonPlanName like +'%'+'" + txtSearchLessonPlan.Text + "'+'%' ORDER BY lp.LessonPlanName";
        DataTable dlassess = objDataClass.fillData(selAssessmentmap);
        if (dlassess.Rows.Count > 0)
        {
            grd_LPassessmentQues.DataSource = dlassess;
            grd_LPassessmentQues.DataBind();
        }
        selAssessmentmap = null;
    }
    public void FillAssessmentLessonplansearchMAP()
    {
        grd_LPassessmentQues.DataSource = null;
        grd_LPassessmentQues.DataBind();
        if (lbtnLPAll.ForeColor == System.Drawing.Color.Red)
            selAssessmentmap = "select distinct lp.LessonPlanName,lp.LessonPlanId from LessonPlan lp full join AsmntLPRel asmt on asmt.LessonPlanId=lp.LessonPlanId Where lp.ActiveInd='A' and lp.LessonPlanName<>'' and lp.LessonPlanName like +'%'+'" + txtSearchLessonPlan.Text + "'+'%' and asmt.AsmntName='" + ddlBindAssessment.SelectedItem.Text + "' ORDER BY lp.LessonPlanName";
        else if (lbtnLPmapped.ForeColor == System.Drawing.Color.Red)
            selAssessmentmap = "select distinct lp.LessonPlanName,lp.LessonPlanId from AsmntLPRel asmt Inner join LessonPlan lp on asmt.LessonPlanId=lp.LessonPlanId Where lp.ActiveInd='A' and lp.LessonPlanName<>'' and asmt.LessonPlanName like +'%'+'" + txtSearchLessonPlan.Text + "'+'%' and asmt.AsmntName='" + ddlBindAssessment.SelectedItem.Text + "' ORDER BY lp.LessonPlanName";
        else if (lbtnLPunmapped.ForeColor == System.Drawing.Color.Red)
            selAssessmentmap = "SELECT lp.LessonPlanName,lp.LessonPlanId FROM LessonPlan lp WHERE NOT EXISTS( SELECT asmt.LessonPlanName FROM AsmntLPRel asmt WHERE asmt.LessonPlanName = lp.LessonPlanName ) and lp.ActiveInd='A' and asmt.LessonPlanName like +'%'+'" + txtSearchLessonPlan.Text + "'+'%' ORDER BY lp.LessonPlanName";
        DataTable dlassess = objDataClass.fillData(selAssessmentmap);
        if (dlassess.Rows.Count > 0)
        {
            grd_LPassessmentQues.DataSource = dlassess;
            grd_LPassessmentQues.DataBind();
        }
        selAssessmentmap = null;
    }
    private void BindGridMAP(GridView GridView, string Lessonplan, int lpid)
    {
        GridView.DataSource = null;
        GridView.DataBind();
        if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text == ""))
        {
            selAssessmentmap = "Select asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,gol.GoalName,asmt.AsmntQId,asmt.AsmntLPRelId  from AsmntLPRel asmt Inner Join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName='" + clsGeneral.convertQuotes(Lessonplan) + "' and asmt.LessonPlanId='" + lpid + "' and asmt.ActiveInd='A' and asmt.AsmntName='" + ddlBindAssessment.SelectedItem.Text.Trim() + "' ";
        }
        else if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text != ""))
        {
            selAssessmentmap = "Select asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,gol.GoalName,asmt.AsmntQId,asmt.AsmntLPRelId  from AsmntLPRel asmt Inner Join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName like +'%'+'" + clsGeneral.convertQuotes(Lessonplan) + "'+'%' and asmt.LessonPlanId='" + lpid + "' and asmt.ActiveInd='A' and asmt.AsmntName='" + ddlBindAssessment.SelectedItem.Text.Trim() + "' ";
        }
        else if ((ddlBindAssessment.SelectedIndex == 0) && (txtSearchLessonPlan.Text != ""))
        {
            selAssessmentmap = "Select asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,gol.GoalName,asmt.AsmntQId,asmt.AsmntLPRelId  from AsmntLPRel asmt Inner Join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName like +'%'+'" + clsGeneral.convertQuotes(Lessonplan) + "'+'%' and asmt.LessonPlanId='" + lpid + "' and asmt.ActiveInd='A' ";
        }
        else
        {
            selAssessmentmap = "Select asmt.AsmntName,asmt.AsmntCat,asmt.AsmntSubCat,gol.GoalName,asmt.AsmntQId,asmt.AsmntLPRelId  from AsmntLPRel asmt Inner Join Goal gol on asmt.GoalId=gol.GoalId where asmt.LessonPlanName='" + clsGeneral.convertQuotes(Lessonplan) + "' and asmt.LessonPlanId='" + lpid + "' and asmt.ActiveInd='A' ";
        }
        DataTable gridLesson = objDataClass.fillData(selAssessmentmap);
        if (gridLesson.Rows.Count > 0)
        {
            GridView.Visible = true;
            GridView.DataSource = gridLesson;
            GridView.DataBind();
        }
        else
            GridView.Visible = false;
        selAssessmentmap = null;
    }
    public void FillAssessmentsearchMAP()
    {
        grd_LPassessmentQues.DataSource = null;
        grd_LPassessmentQues.DataBind();
        if (lbtnLPAll.ForeColor == System.Drawing.Color.Red)
            sqlAssessmentmap = "select distinct lp.LessonPlanName,lp.LessonPlanId from LessonPlan lp full join AsmntLPRel asmt on asmt.LessonPlanId=lp.LessonPlanId Where asmt.ActiveInd='A' and lp.ActiveInd='A' and lp.LessonPlanName<>'' and asmt.LessonPlanId is not null and asmt.AsmntName='" + ddlBindAssessment.SelectedItem.Text + "' ORDER BY lp.LessonPlanName";
        else if (lbtnLPmapped.ForeColor == System.Drawing.Color.Red)
            sqlAssessmentmap = "select distinct lp.LessonPlanName,lp.LessonPlanId from AsmntLPRel asmt Inner join LessonPlan lp on asmt.LessonPlanId=lp.LessonPlanId Where asmt.ActiveInd='A' and lp.ActiveInd='A' and lp.LessonPlanName<>'' and asmt.AsmntName='" + ddlBindAssessment.SelectedItem.Text + "' ORDER BY lp.LessonPlanName";
        else if (lbtnLPunmapped.ForeColor == System.Drawing.Color.Red)
            sqlAssessmentmap = "SELECT lp.LessonPlanName,lp.LessonPlanId FROM LessonPlan lp WHERE NOT EXISTS( SELECT asmt.LessonPlanName FROM AsmntLPRel asmt WHERE asmt.ActiveInd='A' and asmt.LessonPlanName = lp.LessonPlanName ) and lp.ActiveInd='A' and AsmntName='" + ddlBindAssessment.SelectedItem.Text + "' ORDER BY lp.LessonPlanName";
        DataTable dlassess = objDataClass.fillData(sqlAssessmentmap);
        if (dlassess.Rows.Count > 0)
        {
            grd_LPassessmentQues.DataSource = dlassess;
            grd_LPassessmentQues.DataBind();
        }
    }
    private void filldropgoalMAP()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  GoalId as Id,GoalCode as Name FROM Goal where ActiveInd='A' ORDER BY GoalCode", ddlgoal);
    }
    private void filldropviewassessMAP()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  AsmntName as Name FROM AsmntLPRel where ActiveInd='A' and GoalId=" + Convert.ToInt32(ddlgoal.SelectedItem.Value) + " ORDER BY AsmntName", ddlassess);
    }
    private void filldropviewcategoryMAP()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  AsmntCat as Name FROM AsmntLPRel where ActiveInd='A' and AsmntName='" + ddlassess.SelectedItem.Text.Trim() + "' ORDER BY AsmntCat", ddlcategory);
    }
    private void filldropviewsubcategoryMAP()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  AsmntSubCat as Name FROM AsmntLPRel where ActiveInd='A' and AsmntSubCat<>'' and AsmntCat='" + ddlcategory.SelectedItem.Text.Trim() + "' ORDER BY AsmntSubCat", ddlsubcategory);
    }
    private void filldropviewquestionMAP()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  AsmntQId as Name FROM AsmntLPRel where ActiveInd='A' and AsmntCat='" + ddlcategory.SelectedItem.Text.Trim() + "' ORDER BY AsmntQId", ddlquestion);
    }
    private void filldropviewsubquestionMAP()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  AsmntQId as Name FROM AsmntLPRel where ActiveInd='A' and AsmntSubCat='" + ddlsubcategory.SelectedItem.Text.Trim() + "' ORDER BY AsmntQId", ddlquestion);
    }
    private void filldropassessMAP()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT DISTINCT  AsmntName as Name FROM AsmntLPRel where ActiveInd='A' ORDER BY AsmntName", ddlBindAssessment);
    }
    private void FillforPageMAP()
    {
        if (lbtnLPAll.ForeColor == System.Drawing.Color.Red)
            FilllessonplanMAP();
        else if (lbtnLPmapped.ForeColor == System.Drawing.Color.Red)
            FillMappedLPMAP();
        else if (lbtnLPunmapped.ForeColor == System.Drawing.Color.Red)
            FillUnmappedLPMAP();
    }
    private int lessonplancntMAP(int AsmLPid)
    {
        string SelQry = "Select LessonPlanName,LessonPlanId from AsmntLPRel where ActiveInd='A' and AsmntLPRelId='" + AsmLPid + "'";
        DataTable Dt = objDataClass.fillData(SelQry);
        if (Dt.Rows.Count > 0)
        {
            AsmntNamemap = Dt.Rows[0]["LessonPlanName"].ToString().Trim();
            AsmntCatmap = Dt.Rows[0]["LessonPlanId"].ToString().Trim();
        }
        LPcntmap = "SELECT COUNT(*) FROM AsmntLPRel where LessonPlanName='" + clsGeneral.convertQuotes(AsmntNamemap) + "' and LessonPlanId='" + Convert.ToInt32(AsmntCatmap) + "' and ActiveInd='A' and LessonPlanName<>'' ";

        int lessonplancnt = 0;
        lessonplancnt = objDataClass.ExecuteScalar(LPcntmap);
        return lessonplancnt;
    }
    protected void btn_search_AssessLP_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = null;
        if (ddlBindAssessment.SelectedIndex == 0 && txtSearchLessonPlan.Text.Trim() != "")
        {
            FillLessonplansearchMAP();
        }
        else if (ddlBindAssessment.SelectedIndex > 0 && txtSearchLessonPlan.Text.Trim() == "")
        {
            FillAssessmentsearchMAP();
        }
        else if (ddlBindAssessment.SelectedIndex > 0 && txtSearchLessonPlan.Text.Trim() != "")
        {
            FillAssessmentLessonplansearchMAP();
        }
        else
        {
            FillforPageMAP();
        }
    }
    protected void lbtnLPAll_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = null;
        lbtnLPAll.ForeColor = System.Drawing.Color.Red;
        lbtnLPunmapped.ForeColor = System.Drawing.Color.Blue;
        lbtnLPmapped.ForeColor = System.Drawing.Color.Blue;
        if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text == ""))
        {
            FillAssessmentsearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillAssessmentLessonplansearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex == 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillLessonplansearchMAP();
        }
        else
        {
            FillforPageMAP();
        }
    }
    protected void lbtnLPmapped_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = null;
        lbtnLPAll.ForeColor = System.Drawing.Color.Blue;
        lbtnLPunmapped.ForeColor = System.Drawing.Color.Blue;
        lbtnLPmapped.ForeColor = System.Drawing.Color.Red;
        if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text == ""))
        {
            FillAssessmentsearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillAssessmentLessonplansearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex == 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillLessonplansearchMAP();
        }
        else
        {
            FillforPageMAP();
        }
    }
    protected void lbtnLPunmapped_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = null;
        lbtnLPAll.ForeColor = System.Drawing.Color.Blue;
        lbtnLPunmapped.ForeColor = System.Drawing.Color.Red;
        lbtnLPmapped.ForeColor = System.Drawing.Color.Blue;
        if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text == ""))
        {
            FillAssessmentsearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillAssessmentLessonplansearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex == 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillLessonplansearchMAP();
        }
        else
        {
            FillforPageMAP();
        }
    }
    protected void grd_LPassessmentQues_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_LPassessmentQues.PageIndex = e.NewPageIndex;
        if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text == ""))
        {
            FillAssessmentsearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex > 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillAssessmentLessonplansearchMAP();
        }
        else if ((ddlBindAssessment.SelectedIndex == 0) && (txtSearchLessonPlan.Text != ""))
        {
            FillLessonplansearchMAP();
        }
        else
        {
            FillforPageMAP();
        }
    }
    protected void grd_LPassessmentQues_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ADD")
            {
                GridViewRow GVRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lesson = GVRow.Cells[0].FindControl("lbl_lessonplan") as Label;
                ImageButton imglpid = GVRow.Cells[2].FindControl("clickMe") as ImageButton;
                Session["LessonIDMapping"] = imglpid.CommandArgument.ToString();
                lbl_lesson.Text = lesson.Text;
                filldropgoalMAP();
                ddlassess.SelectedIndex = 0;
                ddlcategory.SelectedIndex = 0;
                ddlsubcategory.SelectedIndex = 0;
                ddlquestion.SelectedIndex = 0;
                //filldropviewassessMAP();
                //filldropviewcategoryMAP();
                //filldropviewsubcategoryMAP();
                //filldropviewquestionMAP();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp1(), true);
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;
        }
    }
    protected void grd_LPassessmentQues_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Disable = false;
        sess = (clsSession)Session["UserSession"];
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);   //Check the permission add/disable buttons

        if (lbtnLPunmapped.ForeColor != System.Drawing.Color.Red)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gridResponses = (GridView)e.Row.FindControl("grd_LPassessment");
                Label lessonPlan = (Label)e.Row.FindControl("lbl_lessonplan");
                ImageButton imglpid = (ImageButton)e.Row.FindControl("clickMe");
                BindGridMAP(gridResponses, lessonPlan.Text, Convert.ToInt32(imglpid.CommandArgument.ToString()));


                if (Disable == true)
                {
                    gridResponses.Columns[5].Visible = false;
                }
                else
                {
                    gridResponses.Columns[5].Visible = true;
                }
            }

        if (Disable == true)  //disabling Add Button
        {
            // grd_LPassessmentQues.Columns[1].Visible = false;
            grd_LPassessmentQues.Columns[2].Visible = false;

        }
        else
            grd_LPassessmentQues.Columns[2].Visible = true;
    }


    protected void grd_LPassessment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int AsmntLPRelId = Convert.ToInt32(e.CommandArgument.ToString());
            int lessoncnt = lessonplancntMAP(AsmntLPRelId);
            string deltlessonPlan;
            if (lessoncnt > 1)
            {
                deltlessonPlan = "DELETE from AsmntLPRel where AsmntLPRelId='" + AsmntLPRelId + "'";
            }
            else
            {
                deltlessonPlan = "Update AsmntLPRel set LessonPlanName='' where AsmntLPRelId='" + AsmntLPRelId + "'";
            }
            int retVal = objDataClass.ExecuteNonQuery(deltlessonPlan);
            if (retVal > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Assessment details Deleted Successfully");
                if (ddlBindAssessment.SelectedIndex == 0 && txtSearchLessonPlan.Text.Trim() != "")
                {
                    FillLessonplansearchMAP();
                }
                else if (ddlBindAssessment.SelectedIndex > 0 && txtSearchLessonPlan.Text.Trim() == "")
                {
                    FillAssessmentsearchMAP();
                }
                else if (ddlBindAssessment.SelectedIndex > 0 && txtSearchLessonPlan.Text.Trim() != "")
                {
                    FillAssessmentLessonplansearchMAP();
                }
                else
                {
                    FillforPageMAP();
                }
                FillforPage();
            }
        }
    }


    protected void grd_LPassessment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        grd_LPassessmentQues.EditIndex = -1;
        grd_LPassessmentQues.DataBind();
    }


    protected void btnAssessAdd_Click(object sender, EventArgs e)
    {
        int loginid = sess.LoginId;
        objData = new clsData();
        string subcategory = ddlsubcategory.SelectedItem.Text.Trim();
        if (Session["LessonIDMapping"] != "")
        {
            if (ddlgoal.SelectedIndex != 0)
            {
                if (ddlassess.SelectedIndex != 0)
                {
                    if (ddlcategory.SelectedIndex != 0)
                    {
                        if (ddlquestion.SelectedIndex != 0)
                        {
                            if (ddlsubcategory.SelectedIndex == 0)
                                subcategory = "";
                            string Exist = "SELECT COUNT(*) FROM AsmntLPRel WHERE GoalId='" + Convert.ToInt32(ddlgoal.SelectedItem.Value) + "' AND AsmntName='" + ddlassess.SelectedItem.Text.Trim() + "' AND AsmntCat='" + ddlcategory.SelectedItem.Text.Trim() + "' " +
                                "AND AsmntSubCat='" + subcategory + "' AND AsmntQId='" + ddlquestion.SelectedItem.Text.Trim() + "' AND LessonPlanId='" + Session["LessonIDMapping"] + "' AND LessonPlanName='" + clsGeneral.convertQuotes(lbl_lesson.Text) + "' AND ActiveInd='A'";
                            if (Convert.ToInt32(objData.FetchValue(Exist)) == 0)
                            {
                                string SortOrder = "SELECT MAX(SortOrder) FROM AsmntLPRel WHERE AsmntName='" + ddlassess.SelectedItem.Text.Trim() + "' ";
                                int SORTORDR = Convert.ToInt32(objData.FetchValue(SortOrder)) + 1;
                                string SQLQRY = "INSERT into AsmntLPRel(GoalId,AsmntName,AsmntCat,AsmntSubCat,AsmntQId,LessonPlanId,LessonPlanName,ActiveInd,SortOrder,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn)" +
                                                              " VALUES ('" + Convert.ToInt32(ddlgoal.SelectedItem.Value) + "','" + ddlassess.SelectedItem.Text.Trim() + "','" + ddlcategory.SelectedItem.Text.Trim() + "','" + subcategory + "','" + ddlquestion.SelectedItem.Text.Trim() + "'," +
                                                              "'" + Session["LessonIDMapping"] + "','" + lbl_lesson.Text + "','A','" + SORTORDR + "','" + loginid + "',GETDATE(),'" + loginid + "',GETDATE())";


                                objData.Execute(SQLQRY);
                                FillforPageMAP();
                                FillforPage();
                                tdMsg.InnerHtml = clsGeneral.sucessMsg("Goal details Added Successfully");
                            }
                            else
                            {
                                tdMsg.InnerHtml = clsGeneral.warningMsg("Goal details Already Exist");
                            }
                        }
                        else
                        {
                            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select the Question ID");
                        }
                    }

                    else
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select the category");
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select the Assessment Name");
                }
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please select the Goal Name");
            }
        }
    }


    protected void Button_Search_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        if (ddlAssessment.SelectedIndex == 0 && txtassessment.Text.Trim() != "")
        {
            FillLessonplansearch();
        }
        else if (ddlAssessment.SelectedIndex > 0 && txtassessment.Text.Trim() == "")
        {
            FillAssessmentsearch();
        }
        else if (ddlAssessment.SelectedIndex > 0 && txtassessment.Text.Trim() != "")
        {
            FillAssessmentLessonplansearch();
        }
        else
        {
            FillforPage();
        }
    }


    protected void ddlgoal_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgoal.SelectedIndex != 0)
        {
            filldropviewassessMAP();
        }
        else if (ddlgoal.SelectedIndex == 0)
        {
            ddlassess.Items.Clear();
            ddlassess.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
        }

    }


    protected void ddlassess_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlassess.SelectedIndex != 0)
        {
            filldropviewcategoryMAP();
        }
        else if (ddlassess.SelectedIndex == 0)
        {
            ddlcategory.Items.Clear();
            ddlcategory.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
        }

    }


    protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcategory.SelectedIndex != 0)
        {
            filldropviewsubcategoryMAP();
            filldropviewquestionMAP();
        }
        else if (ddlcategory.SelectedIndex == 0)
        {
            ddlsubcategory.Items.Clear();
            ddlsubcategory.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
            ddlquestion.Items.Clear();
            ddlquestion.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
        }
    }


    protected void ddlsubcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsubcategory.SelectedIndex != 0)
        {
            filldropviewsubquestionMAP();
        }
        else if (ddlsubcategory.SelectedIndex == 0)
        {
            filldropviewquestionMAP();
        }
    }


    protected void img_LPSearch_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
        filldroplessonplanview();
    }
    protected void Tab1_Click(object sender, EventArgs e)
    {
        viewTab1();
    }
    protected void Tab2_Click(object sender, EventArgs e)
    {
        Tab1.CssClass = "Initial";
        Tab2.CssClass = "Clicked";
        MainView.ActiveViewIndex = 1;
        tdMsg.InnerHtml = "";
    }
    private void viewTab1()
    {
        Tab1.CssClass = "Clicked";
        Tab2.CssClass = "Initial";
        MainView.ActiveViewIndex = 0;
        tdMsg.InnerHtml = "";
    }
}