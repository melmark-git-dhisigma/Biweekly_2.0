using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_ViewLessonplanList : System.Web.UI.Page
{
    DataClass objDataClass = new DataClass();
    ClsTemplateSession ObjTempSess = null;
    clsSession sess = null;
    clsData objData = new clsData();
    static bool Disable = false;
    static int iGoalValue = 0;
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
            //static bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            HdFldActiveInactive.Value = "1";
            if (Disable == true)
            {
                BtnInsert.Visible = false;

                if (gvLessonData.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in gvLessonData.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("LinkButton1"));
                        lb_delete.Visible = false;
                    }
                }

            }
            else
            {
                BtnInsert.Visible = true;

                if (gvLessonData.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in gvLessonData.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("LinkButton1"));
                        lb_delete.Visible = true;
                    }
                }

            }
            FillGrid();
            FillddlGoals();
        }

    }
    public void FillGrid()
    {
        string selLesson = "select lp.LessonPlanId,lp.LessonPlanname,lp.LessonPlanDesc,CASE WHEN lp.ModifiedOn is null then lp.CreatedOn ELSE lp.ModifiedOn END AS ModifiedDate,CASE WHEN lp.ModifiedBy is null then (Select UserLName+ ','+UserFName from [User] where UserId=lp.CreatedBy) ELSE (Select UserLName+ ','+UserFName from [User] where UserId=lp.ModifiedBy) END AS ModifiedUser from (LessonPlan lp FULL JOIN [User] Usr ON lp.ModifiedBy = Usr.UserId) WHERE lp.ActiveInd = 'A'";

        DataTable gridLesson = objDataClass.fillData(selLesson);
        gvLessonData.DataSource = gridLesson;
        gvLessonData.DataBind();
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;

    }
    protected void BtnInsert_Click(object sender, EventArgs e)
    {
        Response.Redirect("LessonPlanTemplate.aspx?id=0");
    }
    protected void gvLessonData_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gvLessonData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (lnkInactive.ForeColor == System.Drawing.Color.Red)
            Session["Status"] = "Inactive";
        if (e.CommandName == "View")
        {
            viewLessonplan(Convert.ToInt32(e.CommandArgument));
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
        }
        if (e.CommandName == "editValue")
        {
            ObjTempSess = new ClsTemplateSession();
            ObjTempSess.LessonPlanId = Convert.ToInt32(e.CommandArgument);

            bool Result = objData.IFExists("Select DSTempHdrId from DSTempHdr where LessonPlanId=" + ObjTempSess.LessonPlanId + " and StudentId Is Null");
            if (Result == true)
            {
                string Qry = objData.FetchValue("Select DSTempHdrId from DSTempHdr where LessonPlanId=" + ObjTempSess.LessonPlanId + " and StudentId Is Null").ToString();
                if (Qry != "")
                {
                    ObjTempSess.TemplateId = Convert.ToInt32(Qry);
                }
            }
            else
            {
                ObjTempSess.TemplateId = 0;
            }

            Session["BiweeklySession"] = ObjTempSess;
            Session["LessonData"] = e.CommandArgument;

            Response.Redirect("LessonPlanTemplate.aspx");
        }
        if (e.CommandName == "deleteValue")
        {


            try
            {
                string deleteQuerry = "UPDATE LessonPlan SET ActiveInd = 'D'  where LessonPlanId = " + e.CommandArgument + "";
                int deleteLp = objDataClass.ExecuteNonQuery(deleteQuerry);
                string deleteList = "DELETE from LPAttribute where LessonPlanId = " + e.CommandArgument + "";
                int deleteListIndex = Convert.ToInt32(objDataClass.ExecuteNonQuery(deleteList));
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Deleted Successfully");
                FillGrid();
            }
            catch(Exception Ex)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Error!!! Please Try After Sometime");
                throw Ex;
            }
        }

    }
    protected void gvLessonData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        tdMsg.InnerHtml = "";
        gvLessonData.PageIndex = e.NewPageIndex;
        if ((ddlSelectGoals.SelectedIndex > 0) && (txtSearchLesson.Text == "") && (linkActive.ForeColor == System.Drawing.Color.Red))
        {
            FillGridGoals();
        }
        else if ((ddlSelectGoals.SelectedIndex > 0) && (txtSearchLesson.Text != "") && (linkActive.ForeColor == System.Drawing.Color.Red))
        {
            FillSearchAndGoal();
        }
        else if ((ddlSelectGoals.SelectedIndex == 0) && (txtSearchLesson.Text != "") && (linkActive.ForeColor == System.Drawing.Color.Red))
        {
            FillSearch();

        }
        else
        {
            if (linkActive.ForeColor == System.Drawing.Color.Red)
                FillGrid();
            else
                fillGridInActive();

        }
        //if (lnkInactive.ForeColor == System.Drawing.Color.Red)
        //{


        //}

        //if (linkActive.ForeColor == System.Drawing.Color.Red)
        //{
        //    FillGrid();

        //}


    }
    protected void viewLessonplan(int lessonViewId)        // View LessonPlanList
    {
        clsUser oUser = new clsUser();
        objData = new clsData();
        objDataClass = new DataClass();
        DataTable Dt = new DataTable();

        string selctQuerry = " SELECT Distinct  Lp.LessonPlanId,Lp.LessonPlanName,Lp.PreReq,Lp.Materials,Lp.TeacherSD,Lp.TeacherInst,Lp.Consequence, " +
                                    "  Gl.GoalName,Gl.GoalId FROM (LessonPlan LP LEFT JOIN (GoalLPRel GRel Inner JOIN Goal Gl On GRel.GoalId = Gl.GoalId)  " +
                                             "    ON LP.LessonPlanId = GRel.LessonPlanId) WHERE Lp.LessonPlanId = " + lessonViewId + "";
        DataTable dtList = objData.ReturnDataTable(selctQuerry, false);
        if (dtList.Rows.Count > 0)
        {
            lblLessonName.Text = dtList.Rows[0]["LessonPlanName"].ToString();
            lblPrerequisite.Text = dtList.Rows[0]["PreReq"].ToString();
            lblConsequence.Text = dtList.Rows[0]["Consequence"].ToString();
            lblMaterials.Text = dtList.Rows[0]["Materials"].ToString();
            lblTeacherSd.Text = dtList.Rows[0]["TeacherSD"].ToString();
            lblTeacherInst.Text = dtList.Rows[0]["TeacherInst"].ToString();
            for (int i = 0; i < dtList.Rows.Count; i++)
            {
                lblGoals.Text += dtList.Rows[i]["GoalName"].ToString() + ",";
            }

            lblGoals.Text = lblGoals.Text.Substring(0, lblGoals.Text.Length - 1);
        }


    }

    protected void FillddlGoals()    //  Fill DropDown Select Goals
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT GoalId As Id,GoalName As Name FROM Goal", ddlSelectGoals);

    }

    protected void gvLessonData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Disable == true)
        {
            gvLessonData.Columns[4].Visible = false;
            gvLessonData.Columns[5].Visible = false;
        }
        else
        {
            gvLessonData.Columns[4].Visible = true;
            gvLessonData.Columns[5].Visible = true;
        }
    }
    protected void ddlSelectGoals_SelectedIndexChanged(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        objData = new clsData();
        if (ddlSelectGoals.SelectedIndex == 0)
        {
            FillGrid();
        }
        else
        {
            FillGridGoals();

        }

    }


    protected void FillGridGoals()
    {
        objData = new clsData();
        iGoalValue = Convert.ToInt32(ddlSelectGoals.SelectedValue);
        string selLesson = "SELECT Distinct  Lp.LessonPlanId,Lp.LessonPlanName,CASE WHEN lp.ModifiedOn is null then lp.CreatedOn ELSE lp.ModifiedOn END AS ModifiedDate, " +
                                            " CASE WHEN lp.ModifiedBy is null then (Select UserLName+ ','+UserFName from [User] where UserId=lp.CreatedBy) ELSE (Select UserLName+ ','+UserFName from [User] where UserId=lp.ModifiedBy) END AS ModifiedUser , " +
                                                  "  Gl.GoalName,Gl.GoalId FROM ((LessonPlan LP FULL JOIN  " +
                                                                  " [User] Usr ON lp.ModifiedBy = Usr.UserId ) LEFT JOIN (GoalLPRel GRel Inner JOIN Goal Gl On GRel.GoalId = Gl.GoalId)  " +
                                                                                   " ON LP.LessonPlanId = GRel.LessonPlanId) where Gl.GoalId = " + iGoalValue + " AND Lp.ActiveInd = 'A'";

        DataTable gridLesson = objData.ReturnDataTable(selLesson, false);
        gvLessonData.DataSource = gridLesson;
        gvLessonData.DataBind();
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
    }

    protected void FillSearchAndGoal()
    {
        objData = new clsData();
        string selctValue = " SELECT Distinct  Lp.LessonPlanId,Lp.LessonPlanName,CASE WHEN lp.ModifiedOn is null then lp.CreatedOn ELSE lp.ModifiedOn END AS ModifiedDate, " +
                                           " CASE WHEN lp.ModifiedBy is null then (Select UserLName+ ','+UserFName from [User] where UserId=lp.CreatedBy) ELSE (Select UserLName+ ','+UserFName from [User] where UserId=lp.ModifiedBy) END AS ModifiedUser ,  " +
                                               "    Gl.GoalName,Gl.GoalId FROM ((LessonPlan LP FULL JOIN   " +
                                                            "    [User] Usr ON lp.ModifiedBy = Usr.UserId ) LEFT JOIN (GoalLPRel GRel Inner JOIN Goal Gl On GRel.GoalId = Gl.GoalId)   " +
                                                                              "     ON LP.LessonPlanId = GRel.LessonPlanId) where Gl.GoalId = " + iGoalValue + "  AND LP.LessonPlanName LIKE '" + txtSearchLesson.Text + "%'  AND LP.ActiveInd = 'A'   ";
        DataTable dtSearch = objData.ReturnDataTable(selctValue, false);
        gvLessonData.DataSource = dtSearch;
        gvLessonData.DataBind();
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
    }

    protected void FillSearch()
    {
        objData = new clsData();
        string selctLessonPlans = " select lp.LessonPlanId,lp.LessonPlanname,lp.LessonPlanDesc,CASE WHEN lp.ModifiedOn is null then lp.CreatedOn ELSE lp.ModifiedOn END AS ModifiedDate,CASE WHEN lp.ModifiedBy is null then (Select UserLName+ ','+UserFName from [User] where UserId=lp.CreatedBy) ELSE (Select UserLName+ ','+UserFName from [User] where UserId=lp.ModifiedBy) END AS ModifiedUser from (LessonPlan lp FULL JOIN [User] Usr ON lp.ModifiedBy = Usr.UserId) WHERE lp.LessonPlanName like  '" + txtSearchLesson.Text + "%' AND lp.ActiveInd = 'A'";
        DataTable dtSearch = objData.ReturnDataTable(selctLessonPlans, false);
        gvLessonData.DataSource = dtSearch;
        gvLessonData.DataBind();
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
    }


    protected void ddlSelectGoals_SelectedIndexChanged1(object sender, EventArgs e)
    {
        objData = new clsData();
        tdMsg.InnerHtml = "";
        if (ddlSelectGoals.SelectedIndex == 0)
        {
            FillGrid();
        }
        else
        {
            FillGridGoals();

        }
    }
    protected void fillGridInActive()
    {
        tdMsg.InnerHtml = "";
        string selLesson = "select lp.LessonPlanId,lp.LessonPlanname,lp.LessonPlanDesc,CASE WHEN lp.ModifiedOn is null then lp.CreatedOn ELSE lp.ModifiedOn END AS ModifiedDate,CASE WHEN lp.ModifiedBy is null then (Select UserLName+ ','+UserFName from [User] where UserId=lp.CreatedBy) ELSE (Select UserLName+ ','+UserFName from [User] where UserId=lp.ModifiedBy) END AS ModifiedUser from (LessonPlan lp FULL JOIN [User] Usr ON lp.ModifiedBy = Usr.UserId) WHERE lp.ActiveInd = 'D'";

        DataTable gridLesson = objDataClass.fillData(selLesson);
        gvLessonData.DataSource = gridLesson;
        gvLessonData.DataBind();
    }


    protected void linkActive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "1";
        linkActive.ForeColor = System.Drawing.Color.Red;
        lnkInactive.ForeColor = System.Drawing.Color.Blue;
        FillGrid();
    }
    protected void lnkInactive_Click(object sender, EventArgs e)
    {
        HdFldActiveInactive.Value = "0";
        lnkInactive.ForeColor = System.Drawing.Color.Red;
        linkActive.ForeColor = System.Drawing.Color.Blue;
        fillGridInActive();
    }
    protected void Button_Search_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        tdMsg.InnerHtml = "";
        if ((ddlSelectGoals.SelectedIndex > 0) && (txtSearchLesson.Text != ""))
        {
            FillSearchAndGoal();

        }
        else if ((ddlSelectGoals.SelectedIndex > 0) && (txtSearchLesson.Text == ""))
        {
            FillGridGoals();
        }
        else
        {
            FillSearch();

        }
    }

}