using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


public partial class StudentBinder_LessonPlanNew : System.Web.UI.Page
{
    clsSession sess;
    clsLessons oLessons;
    ClsTemplateSession ObjTempSess;
    clsMathToSamples objMatch = null;
    clsData objData = null;
    DataClass oData = null;
    static bool Disable = false;
    public int glbStudentId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];


        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            //HiddenField1.Value = sess.UserName;
            //bool flag = clsGeneral.PageIdentification(sess.perPage);
            //if (flag == false)
            //{
            //    Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            //}
        }

        //if (ObjTempSess == null) return;

        if (!IsPostBack)
        {

            tdReadMsg.InnerHtml = "";
            bool visible = false;
            Fillinsertdata();
            FillApprovedLessonData();

            AddNewLesson();

            //   ButtonVisibility(visible);


        }
    }



    private void setWritePermissions(bool createNew)
    {
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            //  BtnCopyTemplate.Visible = false;
            //   BtnSubmit.Visible = false;
            ButtonVisibility(false);
            DatalistVisibility(false);
            //  if (createNew == true) BtnCopyTemplate.Visible = false;
        }
        else
        {
            int headerId = 0;
            if (ViewState["HeaderId"] != null)
            {
                headerId = Convert.ToInt32(ViewState["HeaderId"]);
            }

            //  if (createNew == true) BtnCopyTemplate.Visible = true;
            // GetStatus(headerId);
        }
    }


    private void setApprovePermission()
    {
        clsGeneral.ApprovePermissions(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            //BtnApproval.Visible = false;
        }
        else
        {
            //  BtnApproval.Visible = true;
        }
    }
    private void Fillinsertdata()
    {
        try
        {
            //stdLevel.Visible = false;
            objData = new clsData();
            objData.ReturnDropDown("Select LookupId as Id , LookupCode as Name from dbo.LookUp where LookupType='Datasheet-Teaching Procedures'", drpTeachingProc);
            objData.ReturnDropDown("Select LookupId as Id , LookupName as Name from dbo.LookUp where LookupType='Datasheet-Prompt Procedures'", ddlPromptProcedure);
            // objData.ReturnListBox("Select LookupId as Id , LookupName as Name from dbo.LookUp where LookupType='DSTempPrompt'", lstCompletePrompts);

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void ButtonVisibility(bool visibility)
    {
        if (visibility == false)
        {
            BtnUpdate.Visible = false;
            BtnAddMeasure.Visible = false;
            btnAddSet.Visible = false;
            btnAddSetCriteria.Visible = false;
            BtnAddStep.Visible = false;
            btnAddStepCriteria.Visible = false;
            BtnSavePrompt.Visible = false;
            btnAddPrompt.Visible = false;
            BtnAddPromptSelctd.Visible = false;
            BtnAddAllPrompt.Visible = false;
            BtnRemvePrmptSelctd.Visible = false;
            BtnRemoveAllPrmpt.Visible = false;
            btUpdateLessonProc.Visible = false;
            ddlPromptProcedure.Enabled = false;
            lstCompletePrompts.Visible = true;
            lstSelectedPrompts.Visible = true;
            lstCompletePrompts.Enabled = false;
            lstSelectedPrompts.Enabled = false;

        }
        else
        {
            BtnUpdate.Visible = true;
            BtnAddMeasure.Visible = true;
            btnAddSet.Visible = true;
            btnAddSetCriteria.Visible = true;
            BtnAddStep.Visible = true;
            btnAddStepCriteria.Visible = true;
            BtnSavePrompt.Visible = true;
            btnAddPrompt.Visible = true;
            BtnAddPromptSelctd.Visible = true;
            BtnAddAllPrompt.Visible = true;
            BtnRemvePrmptSelctd.Visible = true;
            BtnRemoveAllPrmpt.Visible = true;
            btUpdateLessonProc.Visible = true;
            lstCompletePrompts.Visible = true;
            lstSelectedPrompts.Visible = true;
            lblSelctPrompt.Visible = true;
            ddlPromptProcedure.Enabled = true;
            lstCompletePrompts.Enabled = true;
            lstSelectedPrompts.Enabled = true;

        }
    }

    protected void FillApprovedLessonData()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string selctQuerry = "";

        try
        {
            if (sess != null)
            {

                //selctQuerry = " SELECT LP.LessonPlanName as Name,StdtLP.GoalId, DTmp.DSTempHdrId As ID,DTmp.StatusId,LU.LookupName " +
                //                      " FROM (StdtLessonPlan StdtLP INNER JOIN (DSTempHdr DTmp  INNER JOIN LookUp LU ON LU.LookupId=DTmp.StatusId)  " +
                //                    " ON DTmp.LessonPlanId=StdtLP.LessonPlanId AND DTmp.StudentId=StdtLP.StudentId) " +
                //                        "   INNER JOIN LessonPlan LP ON StdtLP.LessonPlanId=LP.LessonPlanId WHERE StdtLP.StudentId=" + sess.StudentId + " AND " +
                //                             "  StdtLP.ActiveInd='A' AND LU.LookupName<>'Expired' AND LU.LookupName = 'Approved' ";


                //selctQuerry = "SELECT  LessonPlanId As ID,LessonPlanName As Name FROM LessonPlan WHERE ActiveInd = 'A'";
                selctQuerry = "SELECT DSTempHdrId As ID,DSTemplateName As Name FROM DSTempHdr WHERE StudentId IS NULL";
                DataTable dtList = objData.ReturnDataTable(selctQuerry, false);
                dlApprovedLessons.DataSource = dtList;
                dlApprovedLessons.DataBind();
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void BtnUpdateLessonPlan_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string strQuery = "";
        int headerId = 0;
        object objLid = null;
        SqlTransaction Transs = null;
        SqlConnection con = objData.Open();
        int lessonPlanId = 0;
        int newLessonId = 0;
        int newHeaderId = 0;
        sess = (clsSession)Session["UserSession"];

        if (sess != null)
        {
            if (ViewState["HeaderId"] != null)
            {
                try
                {
                    headerId = Convert.ToInt32(ViewState["HeaderId"]);
                    strQuery = "SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId = " + headerId;
                    objLid = objData.FetchValue(strQuery);
                    if (objLid != null)
                    {
                        lessonPlanId = Convert.ToInt32(objLid);
                    }
                    strQuery = "UPDATE LessonPlan SET LessonPlanName = '" + clsGeneral.convertQuotes(txtLessonName.Text.Trim()) + "', FrameandStrand = '" + clsGeneral.convertQuotes(txtFramework.Text.Trim()) + "', SpecStandard = '" + clsGeneral.convertQuotes(txtSpecStandrd.Text.Trim()) + "',SpecEntryPoint = '" + clsGeneral.convertQuotes(txtSpecEntrypoint.Text.Trim()) + "'," +
                                   "  PreReq = '" + clsGeneral.convertQuotes(txtPreSkills.Text.Trim()) + "', Materials = '" + clsGeneral.convertQuotes(txtMaterials.Text.Trim()) + "' WHERE LessonPlanId = " + lessonPlanId;
                    objData.Execute(strQuery);

                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSuccessMsg();", true);
                    ClientScript.RegisterStartupScript(this.GetType(), "", "AlertSuccessMsg();", true);
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }

            }
            else
            {
                if (ValidateLesson() == true)
                {
                    clsData.blnTrans = true;
                    Transs = con.BeginTransaction();
                    try
                    {

                        strQuery = "INSERT INTO  LessonPlan(SchoolId,ActiveInd,LessonPlanName,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials,CreatedBy,ModifiedBy,CreatedOn) values(1,'A','" + clsGeneral.convertQuotes(txtLessonName.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtFramework.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSpecStandrd.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSpecEntrypoint.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtPreSkills.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtMaterials.Text.Trim()) + "'," + sess.LoginId + "," + sess.LoginId + " ,(SELECT Convert(Varchar,getdate(),100))) ";
                        newLessonId = objData.ExecuteWithScopeandConnection(strQuery, con, Transs);
                        strQuery = "INSERT INTO DSTempHdr(DSTemplateName,SchoolId,LessonPlanId,CreatedBy,CreatedOn,StatusId,[FrameandStrand],[SpecStandard],[SpecEntryPoint],[PreReq],[Materials]) " +
                                    "VALUES ('" + clsGeneral.convertQuotes(txtLessonName.Text.Trim()) + "'," + sess.SchoolId + "," + newLessonId + "," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," +
                                                   "(select LookupId from LookUp where LookupName = 'In Progress' and LookupType = 'TemplateStatus'),(SELECT [FrameandStrand] FROM LessonPlan WHERE LessonPlanId=" + newLessonId + ") ,(SELECT [SpecStandard] FROM LessonPlan WHERE LessonPlanId=" + newLessonId + ") ,(SELECT [SpecEntryPoint]  FROM LessonPlan WHERE LessonPlanId=" + newLessonId + ") ,(SELECT [PreReq] FROM LessonPlan WHERE LessonPlanId=" + newLessonId + ") ,(SELECT [Materials] FROM LessonPlan WHERE LessonPlanId=" + newLessonId + ") )";
                        newHeaderId = objData.ExecuteWithScopeandConnection(strQuery, con, Transs);
                        
                        ViewState["HeaderId"] = newHeaderId;

                        objData.CommitTransation(Transs, con);
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSuccessMsg();", true);
                        FillApprovedLessonData();
                        lblDataFill(newHeaderId);


                    }
                    catch (Exception Ex)
                    {
                        objData.RollBackTransation(Transs, con);
                        con.Close();
                        throw Ex;
                    }
                }       


            }
        }
    }


    protected bool ValidateLesson()
    {
        if (txtLessonName.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "NoLessonName();", true);
            return false;
        }
        return true;
    }



    protected void BtnAddNewLessonPlan_Click(object sender, EventArgs e)
    {
        AddNewLesson();
    }


    protected void AddNewLesson()
    {
        objData = new clsData();
        int headerId = 0;
        ViewState["HeaderId"] = null;
        txtLessonName.Text = "";
        txtFramework.Text = "";
        txtSpecStandrd.Text = "";
        txtSpecEntrypoint.Text = "";
        txtPreSkills.Text = "";
        txtMaterials.Text = "";

        rdolistDatasheet.SelectedValue = "1";
        showtrailid.Visible = false;
        txtNoofTrail.Visible = false;
        taskAnalysis.Visible = true;
        drpTasklist.Visible = true;
        drpTasklist.SelectedIndex = 0;
        txtNoofTrail.Text = "";
        txtmajorset.Text = "";
        txtminorset.Text = "";

        txtSDInstruction.Text = "";
        txtResponseOutcome.Text = "";
        txtCorrectResponse.Text = "";
        txtIncorrectResponse.Text = "";
        txtCorrectionProcedure.Text = "";
        txtReinforcementProc.Text = "";

        ddlPromptProcedure.SelectedIndex = 0;
        lstSelectedPrompts.Items.Clear();
        objData.ReturnListBox("Select LookupId as Id , LookupName as Name from dbo.LookUp where LookupType='DSTempPrompt'", lstCompletePrompts);



        MainDiv.Visible = true;                      // Right Main Div Visiblity option set true.
        IsDiscrete(headerId);                       // Check the template is discrete
        lblDataFill(headerId);                      // Fill Current LessonName
        LessonInfo(headerId);                       // Fill the lessonPlanDetails.
        GetMeasureData(headerId);                   // Fill the measure Data 
        FillTypeOfInstruction(headerId);                   // Fill Type of Instruction Data
        GetSetData(headerId);                      // Fill the Sets Data
        GetStepData(headerId);                    // Fill the Steps Data
        GetSetCriteriaData(headerId);             // Fill the Set Criteria Data
        GetStepCriteriaData(headerId);           // Fill the Step Criteria Data
        GetPromptCriteriaData(headerId);        // Fill the Prompt Criteria Data
        GetPromptProcedureList(headerId);      // Fill the prompt Procedure...
        FillDropPrompt(headerId);             // Fill dropdownlist prompt procedure...
        GetLessonProcData(headerId);           // Fill LessonProcedureData
        //   GetStatus(headerId);                    // Button Permissions
        SetLessonProcedure(headerId);                       // Fill Lesson Procedure Data




        //  setWritePermissions(false);


    }




    protected void rdolistDatasheet_SelectedIndexChanged(object sender, EventArgs e)
    {
        objData = new clsData();
        if (rdolistDatasheet.SelectedValue == "0")
        {
            showtrailid.Visible = true;
            txtNoofTrail.Visible = true;
            taskAnalysis.Visible = false;
            drpTasklist.Visible = false;
            txtNoofTrail.Text = "";
        }
        else
        {
            showtrailid.Visible = false;
            txtNoofTrail.Visible = false;
            taskAnalysis.Visible = true;
            drpTasklist.Visible = true;
            drpTasklist.SelectedIndex = 0;
        }
    }


    protected void GetSetData(int headerId)
    {
        objData = new clsData();
        string selQuerry = "";
        try
        {
            selQuerry = "SELECT DSTempSetId, SetCd, SetName FROm DSTempSet WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
            DataTable dtList = objData.ReturnDataTable(selQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {
                    dlSetDetails.DataSource = dtList;
                    dlSetDetails.DataBind();
                }
                else
                {
                    dlSetDetails.DataSource = dtList;
                    dlSetDetails.DataBind();
                }
            }
            else
            {
                dlSetDetails.DataSource = dtList;
                dlSetDetails.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    protected void GetStepData(int headerId)
    {
        objData = new clsData();
        string selQuerry = "";
        try
        {
            selQuerry = " SELECT dsStep.DSTempStepId,dsStep.StepCd,dsStep.StepName,dsSet.SetCd FROM DSTempStep dsStep LEFT JOIN DSTempSet dsSet ON " +
                                   " dsStep.DSTempSetId = dsSet.DSTempSetId WHERE dsStep.DSTempHdrId = " + headerId + " And dsStep.ActiveInd = 'A'";

            DataTable dtList = objData.ReturnDataTable(selQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {
                    dlStepDetails.DataSource = dtList;
                    dlStepDetails.DataBind();
                }
                else
                {
                    dlStepDetails.DataSource = dtList;
                    dlStepDetails.DataBind();
                }
            }
            else
            {
                dlStepDetails.DataSource = dtList;
                dlStepDetails.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }



    protected void GetSetCriteriaData(int headerId)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        try
        {

            if (sess != null)
            {
                string selQuerry = "Select DSR.DSTempRuleId,DSCol.DSTempHdrId,DSR.DSTempSetColId,DSR.DSTempSetColCalcId,DSR.RuleType,Dscol.ColName,  " +
                                                  "  DSR.CriteriaType,DSR.ScoreReq,DSR.TotalInstance,DSR.TotCorrInstance,DSR.ConsequetiveInd,DSR.IOAReqInd," +
                                                        "  DSR.MultiTeacherReqInd from DSTempRule DSR Inner Join " +
                                                          "   DSTempSetCol DSCol ON    DSR.DSTempSetColId=DSCol.DSTempSetColId  Where DSR.ActiveInd='A' And DSR.SchoolId=" + sess.SchoolId + " And " +
                                                          "  DSCol.DSTempHdrId=" + headerId + " And DSR.RuleType='SET'";

                DataTable dtList = objData.ReturnDataTable(selQuerry, false);
                if (dtList != null)
                {
                    if (dtList.Rows.Count > 0)
                    {

                        dlSetCriteria.DataSource = dtList;
                        dlSetCriteria.DataBind();
                    }
                    else
                    {
                        dlSetCriteria.DataSource = dtList;
                        dlSetCriteria.DataBind();
                    }
                }
                else
                {
                    dlSetCriteria.DataSource = dtList;
                    dlSetCriteria.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void GetStepCriteriaData(int headerId)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        try
        {
            if (sess != null)
            {
                string selQuerry = "Select DSR.DSTempRuleId,DSCol.DSTempHdrId,DSR.DSTempSetColId,DSR.DSTempSetColCalcId,DSR.RuleType,Dscol.ColName,  " +
                                                  "  DSR.CriteriaType,DSR.ScoreReq,DSR.TotalInstance,DSR.TotCorrInstance,DSR.ConsequetiveInd,DSR.IOAReqInd," +
                                                        "  DSR.MultiTeacherReqInd from DSTempRule DSR Inner Join " +
                                                          "   DSTempSetCol DSCol ON    DSR.DSTempSetColId=DSCol.DSTempSetColId  Where DSR.ActiveInd='A' And DSR.SchoolId=" + sess.SchoolId + " And " +
                                                          "  DSCol.DSTempHdrId=" + headerId + " And DSR.RuleType='STEP'";

                DataTable dtList = objData.ReturnDataTable(selQuerry, false);
                if (dtList != null)
                {
                    if (dtList.Rows.Count > 0)
                    {

                        dlStepCriteria.DataSource = dtList;
                        dlStepCriteria.DataBind();
                    }
                    else
                    {
                        dlStepCriteria.DataSource = dtList;
                        dlStepCriteria.DataBind();
                    }
                }
                else
                {
                    dlStepCriteria.DataSource = dtList;
                    dlStepCriteria.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    protected void GetPromptCriteriaData(int headerId)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        try
        {
            if (sess != null)
            {
                string selQuerry = "Select DSR.DSTempRuleId,DSCol.DSTempHdrId,DSR.DSTempSetColId,DSR.DSTempSetColCalcId,DSR.RuleType,Dscol.ColName,  " +
                                                  "  DSR.CriteriaType,DSR.ScoreReq,DSR.TotalInstance,DSR.TotCorrInstance,DSR.ConsequetiveInd,DSR.IOAReqInd," +
                                                        "  DSR.MultiTeacherReqInd from DSTempRule DSR Inner Join " +
                                                          "   DSTempSetCol DSCol ON    DSR.DSTempSetColId=DSCol.DSTempSetColId  Where DSR.ActiveInd='A' And DSR.SchoolId=" + sess.SchoolId + " And " +
                                                          "  DSCol.DSTempHdrId=" + headerId + " And DSR.RuleType='PROMPT'";

                DataTable dtList = objData.ReturnDataTable(selQuerry, false);
                if (dtList != null)
                {
                    if (dtList.Rows.Count > 0)
                    {

                        dlPromptCriteria.DataSource = dtList;
                        dlPromptCriteria.DataBind();
                    }
                    else
                    {
                        dlPromptCriteria.DataSource = dtList;
                        dlPromptCriteria.DataBind();
                    }
                }
                else
                {
                    dlPromptCriteria.DataSource = dtList;
                    dlPromptCriteria.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void dlStepDetails_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        Label lblParentSet = (Label)e.Item.FindControl("lblParntSet");
        Label lblDesc = (Label)e.Item.FindControl("lblStepDesc");
        Button btnEdit = (Button)e.Item.FindControl("btnEditStep");
        Button btnDelt = (Button)e.Item.FindControl("btnRemoveStep");
        int headerId = 0;
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (IsDiscrete(headerId) == true)
        {
            btnEdit.Visible = false;
            btnDelt.Visible = false;
        }

        if (lblParentSet.Text == "")
        {
            lblParentSet.Text = "Not Assigned ";
        }

        if (lblDesc.Text.ToString() != "")
        {
            if (lblDesc.Text.ToString().Length > 100)
            {
                lblDesc.Text = lblDesc.Text.ToString().Substring(0, 100) + "........";
            }
        }

    }


    protected void GetMeasureData(int headerId)
    {
        objData = new clsData();
        string selQuerry = "";
        try
        {
            selQuerry = "SELECT DSTempSetColId, ColName,ColTypeCd,CorrResp,CorrRespDesc,InCorrRespDesc,MisTrialDesc FROM DSTempSetCol WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
            DataTable dtList = objData.ReturnDataTable(selQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {
                    dlMeasureData.DataSource = dtList;
                    dlMeasureData.DataBind();
                }
                else
                {
                    dlMeasureData.DataSource = dtList;
                    dlMeasureData.DataBind();
                }
            }
            else
            {
                dlMeasureData.DataSource = dtList;
                dlMeasureData.DataBind();
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }


    }

    protected void dlLP_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        LinkButton lnkLesson = (LinkButton)e.Item.FindControl("lnkLessonPlan");
        try
        {
            if (lnkLesson.Text.ToString() != null)
            {
                if (lnkLesson.Text.ToString().Length > 26)
                {
                    lnkLesson.Text = lnkLesson.Text.ToString().Substring(0, 26) + "......";
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    //protected void dlCompltdLessonPlans_ItemDataBound(object sender, DataListItemEventArgs e)
    //{
    //    objData = new clsData();
    //    LinkButton lnkLesson = (LinkButton)e.Item.FindControl("lnkCompltdLessonPlan");
    //    try
    //    {
    //        if (lnkLesson.Text.ToString() != null)
    //        {
    //            if (lnkLesson.Text.ToString().Length > 26)
    //            {
    //                lnkLesson.Text = lnkLesson.Text.ToString().Substring(0, 26) + "......";
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //}

    protected void dlApprovedLessons_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        LinkButton lnkApprvdLess = (LinkButton)e.Item.FindControl("lnkApprovedLessons");
        try
        {
            if (lnkApprvdLess.Text.ToString() != null)
            {
                if (lnkApprvdLess.Text.ToString().Length > 26)
                {
                    lnkApprvdLess.Text = lnkApprvdLess.Text.ToString().Substring(0, 26) + ".....";
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void dlSetDetails_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        Label lblDesc = (Label)e.Item.FindControl("lblSetDesc");
        Button BtnEdit = (Button)e.Item.FindControl("btnEditSet");
        Button btnDelt = (Button)e.Item.FindControl("btnRemoveSet");
        int headerId = 0;
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {

            if (lblDesc.Text.ToString() != "")
            {
                if (lblDesc.Text.ToString().Length > 100)
                {
                    lblDesc.Text = lblDesc.Text.ToString().Substring(0, 100) + "........";
                }
            }



        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void dlMeasureData_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        string selQuerry = "";
        string misDesc = "";
        bool IncldeMistrial = false;
        HiddenField hiddenFld = (HiddenField)e.Item.FindControl("hdnColId");
        Literal ltData = (Literal)e.Item.FindControl("ltMeasureCaegory");
        Button BtnEdit = (Button)e.Item.FindControl("btnEditMeasure");
        Button btnDelt = (Button)e.Item.FindControl("BtnRemove");
        Label MisTrial = (Label)e.Item.FindControl("lblMistrialDesc");
        string selectQuerry = "";
        int headerId = 0;
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            selQuerry = "SELECT IncMisTrialInd, MisTrialDesc FROM DSTempSetCol WHERE DSTempSetColId = " + hiddenFld.Value;
            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    IncldeMistrial = Convert.ToBoolean(dtNew.Rows[0]["IncMisTrialInd"]);
                    misDesc = dtNew.Rows[0]["MisTrialDesc"].ToString();

                    if (IncldeMistrial == true)
                    {
                        MisTrial.Text = "Included," + misDesc.ToString() + "";
                    }
                    else
                    {
                        MisTrial.Text = "Not Included," + misDesc.ToString() + "";
                    }
                }
                else
                {
                    MisTrial.Text = "Not Included Mistrial";
                }
            }
            else
            {
                MisTrial.Text = "Not Included Mistrial";
            }
            selectQuerry = "SELECT CalcType FROM DSTempSetColCalc WHERE DSTempSetColId = " + hiddenFld.Value;
            DataTable dtList = objData.ReturnDataTable(selectQuerry, false);
            if (dtList != null)
            {

                if (dtList.Rows.Count > 0)
                {
                    ltData.Text = "<table>";
                    for (int i = 0; i < dtList.Rows.Count; i++)
                    {

                        ltData.Text += "<tr><td><p>" + dtList.Rows[i]["CalcType"].ToString() + "</p></td></tr>";


                    }
                    ltData.Text += "</table>";
                }
                else
                {
                    ltData.Text = "No Data";
                }
            }
            else
            {
                ltData.Text = "No Data";
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    protected void dlStepCriteria_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        HiddenField hdnSetRuleId = (HiddenField)e.Item.FindControl("hdnStepCritVal");
        Label lblCriteriaDef = (Label)e.Item.FindControl("lblCriteriaDefStep");
        Label lblCritType = (Label)e.Item.FindControl("lblCriteriaTypeStep");
        Button btnEdit = (Button)e.Item.FindControl("BtnEditStepCriteria");
        Button btnDelt = (Button)e.Item.FindControl("BtnRemoveStepCriteria");
        int scoreReq = 0;
        int totalInstance = 0;
        int totalCorrInstance = 0;
        bool consecutveSess = false;
        bool isMultitchr = false;
        bool isIoReq = false;
        string colName = "";
        string measureType = "";
        string selQuerry = "";
        string cmpleteData = "";
        int headerId = 0;
        string criteriaType = "";

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (IsDiscrete(headerId) == true)
        {
            btnEdit.Visible = false;
            btnDelt.Visible = false;
        }
        try
        {

            selQuerry = "SELECT DsRule.DSTempSetColId,DsRule.DSTempSetColCalcId,DsRule.RuleType,DsRule.CriteriaType,DsRule.ScoreReq,DsRule.TotalInstance," +
                                "  DsRule.TotCorrInstance,DsRule.ConsequetiveInd,DsRule.MultiTeacherReqInd,DsRule.IOAReqInd,DsCol.ColName,DsColCalc.CalcType FROM DSTempRule DsRule LEFT JOIN DSTempSetCol DsCol " +
                                    "  ON DsRule.DSTempSetColId = DsCol.DSTempSetColId LEFT JOIN DSTempSetColCalc DsColCalc ON DsRule.DSTempSetColCalcId = DsColCalc.DSTempSetColCalcId WHERE DSTempRuleId = " + hdnSetRuleId.Value;

            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    lblCritType.Text = dtNew.Rows[0]["CriteriaType"].ToString();
                    scoreReq = Convert.ToInt32(dtNew.Rows[0]["ScoreReq"]);
                    totalInstance = Convert.ToInt32(dtNew.Rows[0]["TotalInstance"]);
                    totalCorrInstance = Convert.ToInt32(dtNew.Rows[0]["TotCorrInstance"]);
                    consecutveSess = Convert.ToBoolean(dtNew.Rows[0]["ConsequetiveInd"]);
                    isMultitchr = Convert.ToBoolean(dtNew.Rows[0]["MultiTeacherReqInd"]);
                    isIoReq = Convert.ToBoolean(dtNew.Rows[0]["IOAReqInd"]);
                    colName = dtNew.Rows[0]["ColName"].ToString();
                    measureType = dtNew.Rows[0]["CalcType"].ToString();
                    criteriaType = lblCritType.Text;
                    if (criteriaType == "MOVE UP")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }
                    else if (criteriaType == "MOVE DOWN")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Less than " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Less than " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }
                    else if (criteriaType == "MODIFICATION")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }

                    try
                    {
                        selQuerry = "UPDATE DSTempRule SET CriteriaDetails = '" + cmpleteData.ToString() + "' WHERE DSTempRuleId = " + hdnSetRuleId.Value;
                        objData.Execute(selQuerry);
                    }
                    catch
                    {
                    }
                    cmpleteData = "";

                }
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void dlSetCriteria_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        HiddenField hdnSetRuleId = (HiddenField)e.Item.FindControl("hdnSetCritVal");
        Label lblCriteriaDef = (Label)e.Item.FindControl("lblCriteriaDef");
        Label lblCritType = (Label)e.Item.FindControl("lblCriteriaType");
        Button BtnEdit = (Button)e.Item.FindControl("btnEditMeasure");
        Button btnDelt = (Button)e.Item.FindControl("BtnRemove");
        int headerId = 0;
        int scoreReq = 0;
        int totalInstance = 0;
        int totalCorrInstance = 0;
        bool consecutveSess = false;
        bool isMultitchr = false;
        bool isIoReq = false;
        string colName = "";
        string measureType = "";
        string selQuerry = "";
        string cmpleteData = "";
        string criteriaType = "";
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            selQuerry = "SELECT DsRule.DSTempSetColId,DsRule.DSTempSetColCalcId,DsRule.RuleType,DsRule.CriteriaType,DsRule.ScoreReq,DsRule.TotalInstance," +
                                "  DsRule.TotCorrInstance,DsRule.ConsequetiveInd,DsRule.MultiTeacherReqInd,DsRule.IOAReqInd,DsCol.ColName,DsColCalc.CalcType FROM DSTempRule DsRule LEFT JOIN DSTempSetCol DsCol " +
                                    "  ON DsRule.DSTempSetColId = DsCol.DSTempSetColId LEFT JOIN DSTempSetColCalc DsColCalc ON DsRule.DSTempSetColCalcId = DsColCalc.DSTempSetColCalcId WHERE DSTempRuleId = " + hdnSetRuleId.Value;

            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    lblCritType.Text = dtNew.Rows[0]["CriteriaType"].ToString();
                    scoreReq = Convert.ToInt32(dtNew.Rows[0]["ScoreReq"]);
                    totalInstance = Convert.ToInt32(dtNew.Rows[0]["TotalInstance"]);
                    totalCorrInstance = Convert.ToInt32(dtNew.Rows[0]["TotCorrInstance"]);
                    consecutveSess = Convert.ToBoolean(dtNew.Rows[0]["ConsequetiveInd"]);
                    isMultitchr = Convert.ToBoolean(dtNew.Rows[0]["MultiTeacherReqInd"]);
                    isIoReq = Convert.ToBoolean(dtNew.Rows[0]["IOAReqInd"]);
                    colName = dtNew.Rows[0]["ColName"].ToString();
                    measureType = dtNew.Rows[0]["CalcType"].ToString();
                    criteriaType = lblCritType.Text;
                    if (criteriaType == "MOVE UP")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }
                    else if (criteriaType == "MOVE DOWN")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Less than " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Less than " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }
                    else if (criteriaType == "MODIFICATION")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }


                    try
                    {
                        selQuerry = "UPDATE DSTempRule SET CriteriaDetails = '" + cmpleteData.ToString() + "' WHERE DSTempRuleId = " + hdnSetRuleId.Value;
                        objData.Execute(selQuerry);
                    }
                    catch
                    {
                    }

                    cmpleteData = "";

                }
            }


        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void dlPromptCriteria_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        HiddenField hdnSetRuleId = (HiddenField)e.Item.FindControl("hdnPromptCritVal");
        Label lblCriteriaDef = (Label)e.Item.FindControl("lblCriteriaDefPrompt");
        Label lblCritType = (Label)e.Item.FindControl("lblCriteriaTypePrompt");
        int scoreReq = 0;
        int totalInstance = 0;
        int totalCorrInstance = 0;
        bool consecutveSess = false;
        bool isMultitchr = false;
        bool isIoReq = false;
        string colName = "";
        string measureType = "";
        string selQuerry = "";
        string cmpleteData = "";
        Button BtnEdit = (Button)e.Item.FindControl("btnEditMeasure");
        Button btnDelt = (Button)e.Item.FindControl("BtnRemove");
        int headerId = 0;
        string criteriaType = "";
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        try
        {
            selQuerry = "SELECT DsRule.DSTempSetColId,DsRule.DSTempSetColCalcId,DsRule.RuleType,DsRule.CriteriaType,DsRule.ScoreReq,DsRule.TotalInstance," +
                                "  DsRule.TotCorrInstance,DsRule.ConsequetiveInd,DsRule.MultiTeacherReqInd,DsRule.IOAReqInd,DsCol.ColName,DsColCalc.CalcType FROM DSTempRule DsRule LEFT JOIN DSTempSetCol DsCol " +
                                    "  ON DsRule.DSTempSetColId = DsCol.DSTempSetColId LEFT JOIN DSTempSetColCalc DsColCalc ON DsRule.DSTempSetColCalcId = DsColCalc.DSTempSetColCalcId WHERE DSTempRuleId = " + hdnSetRuleId.Value;

            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    lblCritType.Text = dtNew.Rows[0]["CriteriaType"].ToString();
                    scoreReq = Convert.ToInt32(dtNew.Rows[0]["ScoreReq"]);
                    totalInstance = Convert.ToInt32(dtNew.Rows[0]["TotalInstance"]);
                    totalCorrInstance = Convert.ToInt32(dtNew.Rows[0]["TotCorrInstance"]);
                    consecutveSess = Convert.ToBoolean(dtNew.Rows[0]["ConsequetiveInd"]);
                    isMultitchr = Convert.ToBoolean(dtNew.Rows[0]["MultiTeacherReqInd"]);
                    isIoReq = Convert.ToBoolean(dtNew.Rows[0]["IOAReqInd"]);
                    colName = dtNew.Rows[0]["ColName"].ToString();
                    measureType = dtNew.Rows[0]["CalcType"].ToString();

                    criteriaType = lblCritType.Text;
                    if (criteriaType == "MOVE UP")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }
                    else if (criteriaType == "MOVE DOWN")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Less than " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Less than " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }
                    else if (criteriaType == "MODIFICATION")
                    {
                        if (consecutveSess == false)
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " in " + totalCorrInstance + " out of " + totalInstance + " Sessions";
                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ".IOA should be completed before advancing a Set.";
                            }
                        }
                        else
                        {
                            cmpleteData = "Atleast " + scoreReq + " " + measureType + " for " + colName + " for " + totalInstance + " Consecutive Sessions";

                            if (isMultitchr == true)
                            {
                                cmpleteData += " With atleast 2 different staff";
                            }
                            if (isIoReq == true)
                            {
                                cmpleteData += ". IOA should be completed before advancing a Set.";
                            }
                        }

                        lblCriteriaDef.Text = cmpleteData.ToString();
                    }
                    try
                    {
                        selQuerry = "UPDATE DSTempRule SET CriteriaDetails = '" + cmpleteData.ToString() + "' WHERE DSTempRuleId = " + hdnSetRuleId.Value;
                        objData.Execute(selQuerry);
                    }
                    catch
                    {
                    }
                    cmpleteData = "";

                }
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    //protected void lnkLessonPlan_Click(object sender, EventArgs e)
    //{
    //    objData = new clsData();
    //    tdReadMsg.InnerHtml = "";
    //    // bool visibility = false;
    //    LinkButton lbLesson = (LinkButton)sender;
    //    try
    //    {
    //        int headerId = Convert.ToInt32(lbLesson.CommandArgument);
    //        ViewState["HeaderId"] = headerId;

    //        //    headerId = 0;

    //        MainDiv.Visible = true;                      // Right Main Div Visiblity option set true.
    //        IsDiscrete(headerId);                       // Check the template is discrete
    //        lblDataFill(headerId);                      // Fill Current LessonName
    //        LessonInfo(headerId);                       // Fill the lessonPlanDetails.
    //        GetMeasureData(headerId);                   // Fill the measure Data 
    //        FillTypeOfInstruction(headerId);                   // Fill Type of Instruction Data
    //        GetSetData(headerId);                      // Fill the Sets Data
    //        GetStepData(headerId);                    // Fill the Steps Data
    //        GetSetCriteriaData(headerId);             // Fill the Set Criteria Data
    //        GetStepCriteriaData(headerId);           // Fill the Step Criteria Data
    //        GetPromptCriteriaData(headerId);        // Fill the Prompt Criteria Data
    //        GetPromptProcedureList(headerId);      // Fill the prompt Procedure...
    //        FillDropPrompt(headerId);             // Fill dropdownlist prompt procedure...
    //        GetLessonProcData(headerId);           // Fill LessonProcedureData
    //        //  GetStatus(headerId);                    // Button Permissions
    //        SetLessonProcedure(headerId);                       // Fill Lesson Procedure Data

    //        setWritePermissions(false);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }

    //}




    //protected void lnkCompltdLessonPlan_Click(object sender, EventArgs e)
    //{
    //    objData = new clsData();
    //    tdReadMsg.InnerHtml = "";
    //    //  bool visibility = false;
    //    LinkButton lbLesson = (LinkButton)sender;
    //    try
    //    {
    //        int headerId = Convert.ToInt32(lbLesson.CommandArgument);
    //        ViewState["HeaderId"] = headerId;

    //        MainDiv.Visible = true;                      // Right Main Div Visiblity option set true.
    //        IsDiscrete(headerId);                       // Check the template is discrete
    //        lblDataFill(headerId);                      // Fill Current LessonName
    //        LessonInfo(headerId);                       // Fill the lessonPlanDetails.
    //        GetMeasureData(headerId);                   // Fill the measure Data 
    //        FillTypeOfInstruction(headerId);                   // Fill Type of Instruction Data
    //        GetSetData(headerId);                      // Fill the Sets Data
    //        GetStepData(headerId);                    // Fill the Steps Data
    //        GetSetCriteriaData(headerId);             // Fill the Set Criteria Data
    //        GetStepCriteriaData(headerId);           // Fill the Step Criteria Data
    //        GetPromptCriteriaData(headerId);        // Fill the Prompt Criteria Data
    //        GetPromptProcedureList(headerId);      // Fill the prompt Procedure...
    //        FillDropPrompt(headerId);             // Fill dropdownlist prompt procedure...
    //        GetLessonProcData(headerId);           // Fill LessonProcedureData
    //        //    GetStatus(headerId);                    // Button Permissions
    //        SetLessonProcedure(headerId);                       // Fill Lesson Procedure Data


    //        setWritePermissions(false);


    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    setApprovePermission();
    //}


    protected void lnkApprovedLessons_Click(object sender, EventArgs e)
    {

        objData = new clsData();
        tdReadMsg.InnerHtml = "";
        // bool visibility = false;
        LinkButton lnkApprvdless = (LinkButton)sender;
        try
        {
            int headerId = Convert.ToInt32(lnkApprvdless.CommandArgument);
            ViewState["HeaderId"] = headerId;

            MainDiv.Visible = true;                      // Right Main Div Visiblity option set true.
            IsDiscrete(headerId);                       // Check the template is discrete
            lblDataFill(headerId);                      // Fill Current LessonName
            LessonInfo(headerId);                       // Fill the lessonPlanDetails.
            GetMeasureData(headerId);                   // Fill the measure Data 
            FillTypeOfInstruction(headerId);                   // Fill Type of Instruction Data
            GetSetData(headerId);                      // Fill the Sets Data
            GetStepData(headerId);                    // Fill the Steps Data
            GetSetCriteriaData(headerId);             // Fill the Set Criteria Data
            GetStepCriteriaData(headerId);           // Fill the Step Criteria Data
            GetPromptCriteriaData(headerId);        // Fill the Prompt Criteria Data
            GetPromptProcedureList(headerId);      // Fill the prompt Procedure...
            FillDropPrompt(headerId);             // Fill dropdownlist prompt procedure...
            GetLessonProcData(headerId);           // Fill LessonProcedureData
            //  GetStatus(headerId);                    // Button Permissions
            SetLessonProcedure(headerId);                       // Fill Lesson Procedure Data

            //   CheckAsigned(headerId);



            //    setWritePermissions(true);




        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    protected void LessonInfo(int templateId)
    {
        objData = new clsData();
        string strQuery = "";
        object objId = null;
        int lessonPlanId = 0;
        strQuery = "SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId = " + templateId;
        objId = objData.FetchValue(strQuery);
        if (objId != null)
        {
            lessonPlanId = Convert.ToInt32(objId);
        }
        try
        {
            strQuery = "SELECT LessonPlanName,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials FROM LessonPlan WHERE LessonPlanId = " + lessonPlanId;
            DataTable dtNew = objData.ReturnDataTable(strQuery, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    txtLessonName.Text = dtNew.Rows[0]["LessonPlanName"].ToString();
                    txtFramework.Text = dtNew.Rows[0]["FrameandStrand"].ToString();
                    txtSpecStandrd.Text = dtNew.Rows[0]["SpecStandard"].ToString();
                    txtSpecEntrypoint.Text = dtNew.Rows[0]["SpecEntryPoint"].ToString();
                    txtPreSkills.Text = dtNew.Rows[0]["PreReq"].ToString();
                    txtMaterials.Text = dtNew.Rows[0]["Materials"].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    //protected void CheckAsigned(int headerId)
    //{
    //    objData = new clsData();
    //    string strQuerry = "";
    //    int LessonPlanId = 0;
    //    int count = 0;
    //    int statusExpId = 0;
    //    object objLessonId = null;
    //    object objcount = null;
    //    sess = (clsSession)Session["UserSession"];

    //    if (sess != null)
    //    {
    //        try
    //        {
    //            statusExpId = Convert.ToInt16(objData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Expired' "));

    //            strQuerry = "SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId = " + headerId;
    //            objLessonId = objData.FetchValue(strQuerry);
    //            if (objLessonId != null)
    //            {
    //                LessonPlanId = Convert.ToInt32(objLessonId);
    //            }

    //            strQuerry = "SELECT COUNT(DSTempHdrId) FROM DSTempHdr WHERE StudentId=" + sess.StudentId + " and LessonPlanId=" + LessonPlanId + " AND StatusId ! = " + statusExpId + "";
    //            objcount = objData.FetchValue(strQuerry);
    //            if (objcount != null)
    //            {
    //                count = Convert.ToInt32(objcount);
    //            }
    //            if (count > 1)
    //            {
    //             //   BtnCopyTemplate.Visible = false;

    //            }
    //            else
    //            {
    //             //   BtnCopyTemplate.Visible = true;

    //            }


    //        }
    //        catch (Exception Ex)
    //        {

    //        }
    //    }

    //}



    protected void SetLessonProcedure(int headerId)
    {
        objData = new clsData();
        string strQuerry = "";
        txtCorrectResponse.Text = "";
        txtIncorrectResponse.Text = "";
        try
        {
            strQuerry = "SELECT CorrRespDesc,InCorrRespDesc FROM DSTempSetCol WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
            DataTable dtList = objData.ReturnDataTable(strQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {

                    for (int i = 0; i < dtList.Rows.Count; i++)
                    {
                        txtCorrectResponse.Text += dtList.Rows[i]["CorrRespDesc"].ToString() + ",";
                        txtIncorrectResponse.Text += dtList.Rows[i]["InCorrRespDesc"].ToString() + ",";

                    }

                }
            }
            else
            {
                txtCorrectResponse.Text = "No Data Input";
                txtIncorrectResponse.Text = "No Data Input";
            }
        }
        catch (Exception Ex)
        {

        }
    }

    protected void lblDataFill(int headerID)
    {
        objData = new clsData();
        try
        {
            string strQuerry = "SELECT DSTemplateName FROM DSTempHdr WHERE DSTempHdrId = " + headerID;
            DataTable dtNew = objData.ReturnDataTable(strQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    string lessonName = dtNew.Rows[0]["DSTemplateName"].ToString();
                    lblCaptnLesson.Text = "Currently Working On :";
                    lblcurrntLessonName.Text = lessonName;
                    //  lblLessonNameSet.Text = lessonName;
                    // lblLessonNameStep.Text = lessonName;
                    //   lblLessonNamePrompt.Text = lessonName;
                    // lblLessonNameProcedure.Text = lessonName;

                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected bool IsDiscrete(int headerId)
    {
        objData = new clsData();
        string strQuerry = "";
        string chainType = "";
        int teachId = 0;
        string teachName = "";
        object objTeach = null;
        try
        {
            strQuerry = "SELECT SkillType,TeachingProcId FROM DSTempHdr WHERE DSTempHdrId = " + headerId;
            DataTable dtNew = objData.ReturnDataTable(strQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    chainType = dtNew.Rows[0]["SkillType"].ToString();
                    try
                    {
                        teachId = Convert.ToInt32(dtNew.Rows[0]["TeachingProcId"]);
                    }
                    catch
                    {
                        teachId = 0;
                    }
                    if (teachId > 0)
                    {
                        try
                        {
                            strQuerry = "SELECT LookupName FROM LookUp WHERE LookupId = " + teachId;
                            objTeach = objData.FetchValue(strQuerry);
                            if (objTeach != null)
                            {
                                teachName = objTeach.ToString();
                            }
                        }
                        catch
                        {
                        }
                    }


                    if (chainType == "Discrete")
                    {
                        BtnAddStep.Visible = false;
                        btnAddStepCriteria.Visible = false;
                        dlStepDetails.Visible = false;
                        dlStepCriteria.Visible = false;
                        if (teachName == "Match-to-Sample")
                        {
                            lblMatchtoSamples.Visible = true;
                            txtMatcSamples.Visible = true;
                            BtnAddSamples.Visible = true;
                            lstMatchSamples.Visible = true;
                            btnDeltSamples.Visible = true;

                        }
                        else
                        {
                            lblMatchtoSamples.Visible = false;
                            txtMatcSamples.Visible = false;
                            BtnAddSamples.Visible = false;
                            lstMatchSamples.Visible = false;
                            btnDeltSamples.Visible = false;

                        }
                        return true;
                    }
                    else
                    {
                        lblMatchtoSamples.Visible = false;
                        txtMatcSamples.Visible = false;
                        BtnAddSamples.Visible = false;
                        lstMatchSamples.Visible = false;
                        btnDeltSamples.Visible = false;
                        BtnAddStep.Visible = true;
                        btnAddStepCriteria.Visible = true;
                        dlStepDetails.Visible = true;
                        dlStepCriteria.Visible = true;
                        return false;
                    }
                }
            }
            return false;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected bool IsMatchToSample(int headerId)
    {
        objData = new clsData();
        string strQuerry = "";
        string chainType = "";
        int teachId = 0;
        string teachName = "";
        bool valid = false;
        object objTeach = null;
        try
        {
            strQuerry = "SELECT SkillType,TeachingProcId FROM DSTempHdr WHERE DSTempHdrId = " + headerId;
            DataTable dtNew = objData.ReturnDataTable(strQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    chainType = dtNew.Rows[0]["SkillType"].ToString();
                    try
                    {
                        teachId = Convert.ToInt32(dtNew.Rows[0]["TeachingProcId"]);
                    }
                    catch
                    {
                        teachId = 0;
                    }
                    if (teachId > 0)
                    {
                        try
                        {
                            strQuerry = "SELECT LookupName FROM LookUp WHERE LookupId = " + teachId;
                            objTeach = objData.FetchValue(strQuerry);
                            if (objTeach != null)
                            {
                                teachName = objTeach.ToString();
                            }
                        }
                        catch
                        {
                        }
                    }


                    if (chainType == "Discrete")
                    {
                        if (teachName == "Match-to-Sample")
                        {

                            valid = true;

                        }
                        else
                        {

                            valid = false;
                        }

                    }
                    else
                    {

                        valid = false;
                    }
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
            return valid;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }




    protected void GetLessonProcData(int headerId)
    {
        objData = new clsData();
        string strQuerry = "";
        try
        {
            strQuerry = "SELECT LessonDefInst,StudentReadCrita,StudCorrRespDef,StudIncorrRespDef,ReinforcementProc,CorrectionProc FROM " +
                                " DSTempHdr WHERE DSTempHdrId = " + headerId;
            DataTable dtNew = objData.ReturnDataTable(strQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    txtSDInstruction.Text = dtNew.Rows[0]["LessonDefInst"].ToString();
                    txtResponseOutcome.Text = dtNew.Rows[0]["StudentReadCrita"].ToString();
                    //   txtCorrectResponse.Text = dtNew.Rows[0]["StudCorrRespDef"].ToString();
                    //  txtIncorrectResponse.Text = dtNew.Rows[0]["StudIncorrRespDef"].ToString();
                    txtReinforcementProc.Text = dtNew.Rows[0]["ReinforcementProc"].ToString();
                    txtCorrectionProcedure.Text = dtNew.Rows[0]["CorrectionProc"].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void GetPromptProcedureList(int headerId)
    {
        objData = new clsData();
        string strQuerry = "";
        try
        {
            strQuerry = "SELECT dsprompt.PromptId As ID,lukUp.LookupName As Name FROM DSTempPrompt dsprompt LEFT JOIN [LookUp] lukUp ON dsprompt.PromptId = lukUp.LookupId WHERE DSTempHdrId = " + headerId;
            DataTable dtNew = objData.ReturnDataTable(strQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    lstSelectedPrompts.DataSource = dtNew;
                    lstSelectedPrompts.DataTextField = "Name";
                    lstSelectedPrompts.DataValueField = "ID";
                    lstSelectedPrompts.DataBind();
                    lstCompletePrompts.Items.Clear();
                    objData.ReturnListBox("SELECT LookupId As ID,LookupName As Name FROM [LookUp] WHERE LookupType = 'DSTempPrompt' AND  LookupId NOT IN (Select PromptId FROM DSTempPrompt WHERE DSTempHdrId = " + headerId + ")", lstCompletePrompts);

                }
                else
                {
                    lstSelectedPrompts.Items.Clear();
                    objData.ReturnListBox("Select LookupId as Id , LookupName as Name from dbo.LookUp where LookupType='DSTempPrompt'", lstCompletePrompts);
                }
            }
            else
            {
                lstSelectedPrompts.Items.Clear();
                objData.ReturnListBox("Select LookupId as Id , LookupName as Name from dbo.LookUp where LookupType='DSTempPrompt'", lstCompletePrompts);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void FillDropPrompt(int headerId)
    {
        objData = new clsData();
        string naValue = "";
        string selQuerry = "";
        try
        {

            selQuerry = "SELECT LookupId FROM LookUp WHERE LookupType = 'Datasheet-Prompt Procedures' AND LookupName = 'NA'";
            object val = objData.FetchValue(selQuerry);

            if (val != null) naValue = val.ToString();

            selQuerry = "SELECT PromptTypeId FROM DSTempHdr WHERE DsTempHdrId = " + headerId;
            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    try
                    {
                        ddlPromptProcedure.SelectedValue = dtNew.Rows[0]["PromptTypeId"].ToString();
                        if (naValue != "")
                        {
                            if (ddlPromptProcedure.SelectedValue == naValue.ToString())
                            {
                                lstCompletePrompts.Visible = false;
                                lstSelectedPrompts.Visible = false;
                                BtnAddPromptSelctd.Visible = false;
                                BtnAddAllPrompt.Visible = false;
                                BtnRemvePrmptSelctd.Visible = false;
                                BtnRemoveAllPrmpt.Visible = false;
                                lblSelctPrompt.Visible = false;
                            }
                            else
                            {
                                lstCompletePrompts.Visible = true;
                                lstSelectedPrompts.Visible = true;
                                BtnAddPromptSelctd.Visible = true;
                                BtnAddAllPrompt.Visible = true;
                                BtnRemvePrmptSelctd.Visible = true;
                                BtnRemoveAllPrmpt.Visible = true;
                                lblSelctPrompt.Visible = true;
                            }
                        }
                    }
                    catch
                    {
                        ddlPromptProcedure.SelectedIndex = 0;
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string skilltype = string.Empty;
        int teachProcId = 0;
        int noofTrial = 0;
        string StrNoTril;
        string taskAnalysis = string.Empty;
        string strQuery = "";
        int headerId = 0;
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            if (rdolistDatasheet.SelectedItem.Text.Trim() == "Discrete Trial")
            {
                skilltype = "Discrete";
            }
            else
            {
                skilltype = "Chained";
                txtNoofTrail.Text = "";
            }
            try
            {
                noofTrial = Convert.ToInt32(txtNoofTrail.Text.Trim());
                StrNoTril = noofTrial.ToString();
            }
            catch
            {
                noofTrial = 0;
                StrNoTril = "''";
            }

            if (drpTasklist.SelectedValue != "0" && rdolistDatasheet.SelectedValue == "1")
            {
                taskAnalysis = drpTasklist.SelectedItem.Text;
            }
            try
            {
                teachProcId = Convert.ToInt32(drpTeachingProc.SelectedValue);
            }
            catch
            {
                teachProcId = 0;
            }

            if (ValidateTeach() == true)
            {
                try
                {

                    if (skilltype == "Discrete")
                    {
                        if (CheckAnyCriteria(headerId) == true)
                        {
                            strQuery = "update DSTempHdr set TeachingProcId=" + teachProcId + ",SkillType='" + skilltype + "',NbrOfTrials=" + StrNoTril + ",ChainType='" + taskAnalysis + "'," +
                         "MajorSetting='" + clsGeneral.convertQuotes(txtmajorset.Text.Trim()) + "',MinorSetting='" + clsGeneral.convertQuotes(txtminorset.Text.Trim()) + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() where DSTempHdrId =" + headerId + "";
                            objData.Execute(strQuery);
                            IsDiscrete(headerId);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "AlertSuccessMsg();", true);
                        }
                        else
                        {
                            FillTypeOfInstruction(headerId);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "AlertConvertDisc();", true);
                        }

                    }
                    else
                    {

                        strQuery = "update DSTempHdr set TeachingProcId=" + teachProcId + ",SkillType='" + skilltype + "',NbrOfTrials=" + StrNoTril + ",ChainType='" + taskAnalysis + "'," +
                            "MajorSetting='" + clsGeneral.convertQuotes(txtmajorset.Text.Trim()) + "',MinorSetting='" + clsGeneral.convertQuotes(txtminorset.Text.Trim()) + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() where DSTempHdrId =" + headerId + "";
                        objData.Execute(strQuery);

                        IsDiscrete(headerId);                // Check updated template is chained or discrete.
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "AlertSuccessMsg();", true);
                    }



                    //tdMsgTypeInstruction.InnerHtml = clsGeneral.sucessMsg("Updated Successfully!!");

                }
                catch
                {
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }



    private bool CheckAnyCriteria(int headerId)
    {
        objData = new clsData();
        string strQuerry = "";
        int countStep = 0;
        int countCriteria = 0;
        bool valid = false;
        object objVal = null;
        object objCount = null;
        int columnId = 0;
        DataTable dtNew = new DataTable();
        strQuerry = "SELECT COUNT(DSTempStepId) FROM DSTempStep WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
        objCount = objData.FetchValue(strQuerry);
        if (objCount != null)
        {
            countStep = Convert.ToInt32(objCount);
        }
        if (countStep == 0)
        {
            strQuerry = "SELECT DSTempSetColId FROM DSTempSetCol WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
            dtNew = objData.ReturnDataTable(strQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNew.Rows.Count; i++)
                    {
                        columnId = Convert.ToInt32(dtNew.Rows[i]["DSTempSetColId"]);
                        strQuerry = "SELECT COUNT(DSTempRuleId) FROM DSTempRule WHERE DSTempSetColId = " + columnId + " AND RuleType = 'STEP' AND ActiveInd = 'A'";
                        objVal = objData.FetchValue(strQuerry);
                        if (objVal != null)
                        {
                            countCriteria = Convert.ToInt32(objVal);
                        }
                        if (countCriteria > 0)
                        {
                            return false;
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                }
                else
                {
                    valid = true;
                }
            }
            else
            {
                valid = true;
            }

        }
        else
        {
            valid = false;
        }
        return valid;
    }


    protected bool ValidateTeach()
    {
        objData = new clsData();
        if (drpTeachingProc.SelectedValue == "0")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "ValidateTeachingProc();", true);
            return false;
        }

        if (rdolistDatasheet.SelectedValue == "0")
        {
            if (txtNoofTrail.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "ValidateNoofTrials();", true);
                return false;
            }
        }
        else if (rdolistDatasheet.SelectedValue == "1")
        {
            if (drpTasklist.SelectedValue == "0")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "ValidateDrpclass();", true);
                return false;
            }
        }
        return true;
    }

    protected void ClearTypeInstruction()
    {
        drpTeachingProc.SelectedIndex = 0;
        rdolistDatasheet.SelectedValue = "1";
        drpTasklist.SelectedIndex = 0;
        txtmajorset.Text = "";
        txtminorset.Text = "";

    }

    protected void FillTypeOfInstruction(int headerId)
    {
        objData = new clsData();
        string skillType = "";
        string chainType = "";
        int noofTrials = 0;
        string teachingProcId = "";
        //if (ViewState["HeaderId"] != null)
        //{
        //    headerId = Convert.ToInt32(ViewState["HeaderId"]);
        //}
        try
        {
            objData.ReturnDropDown("Select LookupId as Id , LookupCode as Name from dbo.LookUp where LookupType='Datasheet-Teaching Procedures'", drpTeachingProc);
            string selQuerry = "SELECT TeachingProcId, SkillType,NbrOfTrials,ChainType,MajorSetting,MinorSetting FROm DSTempHdr WHERE DSTempHdrId = " + headerId;
            DataTable dtList = objData.ReturnDataTable(selQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {
                    teachingProcId = dtList.Rows[0]["TeachingProcId"].ToString();
                    if (teachingProcId != null)
                    {
                        try
                        {
                            drpTeachingProc.SelectedValue = teachingProcId;
                        }
                        catch
                        {
                            drpTeachingProc.SelectedValue = "0";
                        }
                    }
                    skillType = dtList.Rows[0]["SkillType"].ToString();
                    if (skillType == "Chained")
                    {
                        rdolistDatasheet.SelectedValue = "1";
                        showtrailid.Visible = false;
                        txtNoofTrail.Visible = false;
                        taskAnalysis.Visible = true;
                        drpTasklist.Visible = true;
                        chainType = dtList.Rows[0]["ChainType"].ToString();
                        drpTasklist.SelectedValue = chainType.ToString();

                    }
                    else if (skillType == "Discrete")
                    {
                        rdolistDatasheet.SelectedValue = "0";
                        txtNoofTrail.Visible = true;
                        showtrailid.Visible = true;
                        drpTasklist.Visible = false;
                        noofTrials = Convert.ToInt32(dtList.Rows[0]["NbrOfTrials"]);
                        txtNoofTrail.Text = noofTrials.ToString();
                    }
                    else
                    {
                        showtrailid.Visible = false;
                        txtNoofTrail.Visible = false;
                        taskAnalysis.Visible = true;
                        drpTasklist.Visible = true;
                        rdolistDatasheet.SelectedValue = "1";
                        drpTasklist.SelectedIndex = 0;
                    }

                    txtmajorset.Text = dtList.Rows[0]["MajorSetting"].ToString();
                    txtminorset.Text = dtList.Rows[0]["MinorSetting"].ToString();


                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }
    protected void btnAddSetDetails_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        object objCount = null;
        int headerId = 0;
        int findCount = 0;
        int newCount = 0;
        string insertQuerry = "";
        string setMatch = "";
        string matchSelctd = "";
        int SetId = 0;
        int length = 0;
        try
        {
            if (sess != null)
            {

                if (ViewState["HeaderId"] != null)
                {
                    headerId = Convert.ToInt32(ViewState["HeaderId"]);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
                    return;
                }

                if (IsDiscrete(headerId) == true)
                {
                    string[] arryMatchValue = new string[lstMatchSamples.Items.Count];
                    if (lstMatchSamples.Items.Count > 0)
                    {
                        for (int i = 0; i < lstMatchSamples.Items.Count; i++)
                        {
                            arryMatchValue[i] = lstMatchSamples.Items[i].Value.ToString();
                        }
                        for (int arryInt = 0; arryInt < lstMatchSamples.Items.Count; arryInt++)
                        {
                            setMatch += arryMatchValue[arryInt].ToString() + ",";
                        }
                        length = setMatch.Length;
                        matchSelctd = setMatch.ToString().Substring(0, length - 1);

                    }

                }
                string selQuerry = "SELECT COUNT(*) FROM DSTempSet WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
                objCount = objData.FetchValue(selQuerry);
                if (objCount != null)
                {
                    findCount = Convert.ToInt32(objCount);
                }
                newCount = findCount + 1;
                try
                {
                    insertQuerry = "Insert Into DSTempSet(SchoolId,DSTempHdrId,SetCd,SetName,Samples,SortOrder,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
                    insertQuerry += " Values(" + sess.SchoolId + "," + headerId + ",'" + clsGeneral.convertQuotes(txtBoxAddSet.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSetDescription.Text.Trim()) + "','" + setMatch + "'," + newCount + ",'A'," + sess.LoginId + ",getdate()," + sess.LoginId + " ,getdate()) ";

                    SetId = objData.ExecuteWithScope(insertQuerry);


                    if (IsMatchToSample(headerId) == true)
                    {
                        if (matchSelctd != null)
                        {
                            MatchSampDef(headerId, matchSelctd, SetId);                 // Match to Sample code 
                        }
                    }

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseSetPopup();", true);
                    GetSetData(headerId);


                }
                catch (Exception Ex)
                {
                    string error = Ex.Message;
                    tdMsgSet.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }



    protected void MatchSampDef(int TId, string setMatch, int setId)
    {
        objData = new clsData();
        objMatch = new clsMathToSamples();
        // setMatch = setMatch.Remove(0, 1);
        int NoOfTrials = 0;
        string strQuerry = "";
        string deltQuerry = "";
        string samples = "";
        sess = (clsSession)Session["UserSession"];
        string[] setArray = setMatch.Split(',');
        int order = 0;
        try
        {
            if (TId > 0)
            {
                NoOfTrials = Convert.ToInt32(objData.FetchValue("Select NbrofTrials from DSTempHdr where NbrofTrials <> '' AND SkillType = 'Discrete' AND DSTempHdrId=" + TId + ""));

                if (NoOfTrials != 0)
                {
                    deltQuerry = "UPDATE DSTempStep SET ActiveInd = 'D' WHERE DSTempSetId =" + setId;
                    objData.Execute(deltQuerry);

                    clsMathToSamples.Step[] steps = clsMathToSamples.FormSteps(setArray, NoOfTrials);

                    foreach (clsMathToSamples.Step step in steps)
                    {
                        if (step != null)
                        {
                            order++;
                            samples = step.TrialText + " ";
                            strQuerry = "Insert into DSTempStep (SchoolId,DSTempHdrId,DSTempSetId,StepName,SortOrder,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) " +
                            "Values(" + sess.SchoolId + "," + TId + "," + setId + ",'" + samples + "'," + order + ",'A'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate()) ";
                            objData.Execute(strQuerry);
                        }
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnAddStepDetails_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        string strQuery = "";
        int parentSetId = 0;
        int findCount = 0;
        int newCount = 0;
        int stepId = 0;
        object objCount = null;
        sess = (clsSession)Session["UserSession"];
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            if (sess != null)
            {
                if (ddlSetData.SelectedIndex == 0)
                {
                    parentSetId = 0;
                }
                else
                {
                    try
                    {
                        parentSetId = Convert.ToInt32(ddlSetData.SelectedValue);
                    }
                    catch
                    {
                        parentSetId = 0;
                    }
                }

                string selQuerry = "SELECT COUNT(*) FROM DSTempStep WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A' AND DSTempSetId = " + parentSetId;
                objCount = objData.FetchValue(selQuerry);
                if (objCount != null)
                {
                    findCount = Convert.ToInt32(objCount);
                }

                newCount = findCount + 1;

                try
                {
                    strQuery = "Insert Into DSTempStep(SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
                    strQuery += " Values(" + sess.SchoolId + "," + headerId + ",'" + clsGeneral.convertQuotes(txtStepName.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtStepDesc.Text.Trim()) + "'," + parentSetId + "," + newCount + ",'A'," + sess.LoginId + ",getdate()," + sess.LoginId + " ,getdate()) ";
                    stepId = objData.ExecuteWithScope(strQuery);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseStepPopup();", true);
                    GetStepData(headerId);

                }
                catch (Exception Ex)
                {
                    string error = Ex.Message;
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void BtnAddSetDCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        string insertQuerry = "";
        string criteriaType = "";
        int ioaReq = 0;
        int multiTeach = 0;
        int consctveSess = 0;
        int columnValue = 0;
        int measureValue = 0;
        string requiredScore = "";
        string totalInstance = "";
        string totalcorrectInstance = "";
        string numbrSessions = "";
        sess = (clsSession)Session["UserSession"];

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (ValidateCheck() == true)
        {

            try
            {
                if (sess != null)
                {
                    if (rbtnIoaReq.SelectedValue == "1") ioaReq = 1;
                    else ioaReq = 0;
                    if (rbtnMultitchr.SelectedValue == "1") multiTeach = 1;
                    else multiTeach = 0;
                    if (rbtnConsectiveSes.SelectedValue == "1") consctveSess = 1;
                    else consctveSess = 0;
                    if (ddlCriteriaType.SelectedIndex != 0) criteriaType = ddlCriteriaType.SelectedValue.ToString();
                    if (ddlTempColumn.SelectedIndex != 0)
                        try
                        {
                            columnValue = Convert.ToInt32(ddlTempColumn.SelectedValue);
                        }
                        catch
                        {
                            columnValue = 0;
                        }
                    if (ddlTempMeasure.SelectedIndex != 0)
                        try
                        {
                            measureValue = Convert.ToInt32(ddlTempMeasure.SelectedValue);
                        }
                        catch
                        {
                            measureValue = 0;
                        }
                    requiredScore = clsGeneral.convertQuotes(txtRequiredScore.Text.Trim());
                    numbrSessions = clsGeneral.convertQuotes(txtNumbrSessions.Text.Trim());
                    if (consctveSess == 1) totalInstance = clsGeneral.convertQuotes(txtNumbrSessions.Text.Trim());
                    else
                    {
                        totalInstance = clsGeneral.convertQuotes(txtIns2.Text.Trim());
                        totalcorrectInstance = clsGeneral.convertQuotes(txtIns1.Text.Trim());
                    }

                    insertQuerry = "Insert Into DSTempRule(SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,IOAReqInd,MultiTeacherReqInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) "
                              + "Values(" + sess.SchoolId + "," + columnValue + "," + measureValue + ",'SET','" + criteriaType + "'  ," + requiredScore + ",'" + totalInstance + "','" + totalcorrectInstance + "', " + consctveSess + "," + ioaReq + "," + multiTeach + ", " + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A')  ;";
                    int index = objData.Execute(insertQuerry);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseCriteriaPopup();", true);
                    GetSetCriteriaData(headerId);
                }
            }
            catch (Exception Ex)
            {
                string error = Ex.Message;
                tdMsgCriteria.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                throw Ex;
            }
        }



    }


    protected bool ValidateCheck()
    {
        objData = new clsData();
        if (ddlCriteriaType.SelectedValue == "0")
        {
            tdMsgCriteria.InnerHtml = clsGeneral.warningMsg("Select Criteria Type");
            ddlCriteriaType.Focus();
            return false;
        }
        if (ddlTempColumn.SelectedValue == "0" || ddlTempColumn.SelectedValue == "")
        {
            tdMsgCriteria.InnerHtml = clsGeneral.warningMsg("Select Column Type");
            ddlTempColumn.Focus();
            return false;
        }
        if (ddlTempMeasure.SelectedValue == "0" || ddlTempMeasure.SelectedValue == "")
        {
            tdMsgCriteria.InnerHtml = clsGeneral.warningMsg("Select Measure Type");
            ddlTempMeasure.Focus();
            return false;
        }

        if (rbtnConsectiveSes.SelectedValue == "0")
        {
            if (txtIns2.Text == "")
            {
                tdMsgCriteria.InnerHtml = clsGeneral.warningMsg("Enter Total  Instance Value");
                txtIns2.Focus();
                return false;
            }
            if (txtIns1.Text == "")
            {
                tdMsgCriteria.InnerHtml = clsGeneral.warningMsg("Enter Total Correct Instance Value");
                txtIns1.Focus();
                return false;
            }
        }
        else if (rbtnConsectiveSes.SelectedValue == "1")
        {
            if (txtNumbrSessions.Text == "")
            {
                tdMsgCriteria.InnerHtml = clsGeneral.warningMsg("Please Define No of Sessions");
                txtNumbrSessions.Focus();
                return false;

            }
        }

        return true;
    }





    protected void BtnAddStepDCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        string insertQuerry = "";
        string criteriaType = "";
        int ioaReq = 0;
        int multiTeach = 0;
        int consctveSess = 0;
        int columnValue = 0;
        int measureValue = 0;
        string requiredScore = "";
        string totalInstance = "";
        string totalcorrectInstance = "";
        string numbrSessions = "";
        sess = (clsSession)Session["UserSession"];

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (ValidateCheck() == true)
        {

            try
            {
                if (sess != null)
                {
                    if (rbtnIoaReq.SelectedValue == "1") ioaReq = 1;
                    else ioaReq = 0;
                    if (rbtnMultitchr.SelectedValue == "1") multiTeach = 1;
                    else multiTeach = 0;
                    if (rbtnConsectiveSes.SelectedValue == "1") consctveSess = 1;
                    else consctveSess = 0;
                    if (ddlCriteriaType.SelectedIndex != 0) criteriaType = ddlCriteriaType.SelectedValue.ToString();
                    if (ddlTempColumn.SelectedIndex != 0)
                        try
                        {
                            columnValue = Convert.ToInt32(ddlTempColumn.SelectedValue);
                        }
                        catch
                        {
                            columnValue = 0;
                        }
                    if (ddlTempMeasure.SelectedIndex != 0)
                        try
                        {
                            measureValue = Convert.ToInt32(ddlTempMeasure.SelectedValue);
                        }
                        catch
                        {
                            measureValue = 0;
                        }
                    requiredScore = clsGeneral.convertQuotes(txtRequiredScore.Text.Trim());
                    numbrSessions = clsGeneral.convertQuotes(txtNumbrSessions.Text.Trim());
                    if (consctveSess == 1) totalInstance = clsGeneral.convertQuotes(txtNumbrSessions.Text.Trim());
                    else
                    {
                        totalInstance = clsGeneral.convertQuotes(txtIns2.Text.Trim());
                        totalcorrectInstance = clsGeneral.convertQuotes(txtIns1.Text.Trim());
                    }

                    insertQuerry = "Insert Into DSTempRule(SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,IOAReqInd,MultiTeacherReqInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) "
                              + "Values(" + sess.SchoolId + "," + columnValue + "," + measureValue + ",'STEP','" + criteriaType + "'  ," + requiredScore + ",'" + totalInstance + "','" + totalcorrectInstance + "', " + consctveSess + "," + ioaReq + "," + multiTeach + ", " + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A')  ;";
                    int index = objData.Execute(insertQuerry);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseCriteriaPopup();", true);
                    GetStepCriteriaData(headerId);
                }
            }
            catch (Exception Ex)
            {
                string error = Ex.Message;
                tdMsgCriteria.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                throw Ex;
            }
        }
    }

    protected void BtnAddPromptDCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        string insertQuerry = "";
        string criteriaType = "";
        int ioaReq = 0;
        int multiTeach = 0;
        int consctveSess = 0;
        int columnValue = 0;
        int measureValue = 0;
        string requiredScore = "";
        string totalInstance = "";
        string totalcorrectInstance = "";
        string numbrSessions = "";
        sess = (clsSession)Session["UserSession"];

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (ValidateCheck() == true)
        {

            try
            {
                if (sess != null)
                {
                    if (rbtnIoaReq.SelectedValue == "1") ioaReq = 1;
                    else ioaReq = 0;
                    if (rbtnMultitchr.SelectedValue == "1") multiTeach = 1;
                    else multiTeach = 0;
                    if (rbtnConsectiveSes.SelectedValue == "1") consctveSess = 1;
                    else consctveSess = 0;
                    if (ddlCriteriaType.SelectedIndex != 0) criteriaType = ddlCriteriaType.SelectedValue.ToString();
                    if (ddlTempColumn.SelectedIndex != 0)
                        try
                        {
                            columnValue = Convert.ToInt32(ddlTempColumn.SelectedValue);
                        }
                        catch
                        {
                            columnValue = 0;
                        }
                    if (ddlTempMeasure.SelectedIndex != 0)
                        try
                        {
                            measureValue = Convert.ToInt32(ddlTempMeasure.SelectedValue);
                        }
                        catch
                        {
                            measureValue = 0;
                        }
                    requiredScore = clsGeneral.convertQuotes(txtRequiredScore.Text.Trim());
                    numbrSessions = clsGeneral.convertQuotes(txtNumbrSessions.Text.Trim());
                    if (consctveSess == 1) totalInstance = clsGeneral.convertQuotes(txtNumbrSessions.Text.Trim());
                    else
                    {
                        totalInstance = clsGeneral.convertQuotes(txtIns2.Text.Trim());
                        totalcorrectInstance = clsGeneral.convertQuotes(txtIns1.Text.Trim());
                    }

                    insertQuerry = "Insert Into DSTempRule(SchoolId,DSTempSetColId,DSTempSetColCalcId,RuleType,CriteriaType,ScoreReq,TotalInstance,TotCorrInstance,ConsequetiveInd,IOAReqInd,MultiTeacherReqInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) "
                              + "Values(" + sess.SchoolId + "," + columnValue + "," + measureValue + ",'PROMPT','" + criteriaType + "'  ," + requiredScore + ",'" + totalInstance + "','" + totalcorrectInstance + "', " + consctveSess + "," + ioaReq + "," + multiTeach + ", " + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A')  ;";
                    int index = objData.Execute(insertQuerry);

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseCriteriaPopup();", true);
                    GetPromptCriteriaData(headerId);
                }
            }
            catch (Exception Ex)
            {
                string error = Ex.Message;
                tdMsgCriteria.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                throw Ex;
            }
        }

    }

    protected void btnEditSet_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        try
        {
            Button btnEdit = (Button)sender;
            int setId = Convert.ToInt32(btnEdit.CommandArgument);
            ViewState["EditSetId"] = setId;
            BtnUpdateSetDetails.Visible = true;
            btnAddSetDetails.Visible = false;
            ClearSetData();
            EditSetData(setId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddSet();", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void btnEditStep_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        try
        {
            Button btnEditStep = (Button)sender;
            int stepId = Convert.ToInt32(btnEditStep.CommandArgument);
            ViewState["EditStepId"] = stepId;
            BtnUpdateStep.Visible = true;
            btnAddStepDetails.Visible = false;
            ClearStepData();
            FillSetDrpList();
            EditStepData(stepId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddStep();", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void BtnEditSetCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        try
        {
            Button btnEditCriteria = (Button)sender;
            int criteriaId = Convert.ToInt32(btnEditCriteria.CommandArgument);
            ViewState["EditSetCriteria"] = criteriaId;
            BtnAddSetDCriteria.Visible = false;
            BtnAddStepDCriteria.Visible = false;
            BtnAddPromptDCriteria.Visible = false;
            BtnUpdateStepDCriteria.Visible = false;
            BtnUpdatePromptDCriteria.Visible = false;
            BtnUpdateSetDCriteria.Visible = true;
            ClearCriteriaData();
            FillDropMeasure();
            EditSetCriteria(criteriaId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddCriteriaPopup();", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void BtnEditStepCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        try
        {
            Button btnEditCriteria = (Button)sender;
            int criteriaId = Convert.ToInt32(btnEditCriteria.CommandArgument);
            ViewState["EditStepCriteria"] = criteriaId;
            BtnAddSetDCriteria.Visible = false;
            BtnAddStepDCriteria.Visible = false;
            BtnAddPromptDCriteria.Visible = false;
            BtnUpdateStepDCriteria.Visible = true;
            BtnUpdatePromptDCriteria.Visible = false;
            BtnUpdateSetDCriteria.Visible = false;
            ClearCriteriaData();
            FillDropMeasure();
            EditSetCriteria(criteriaId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddCriteriaPopup();", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void BtnEditPromptCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        try
        {
            Button btnEditCriteria = (Button)sender;
            int criteriaId = Convert.ToInt32(btnEditCriteria.CommandArgument);
            ViewState["EditPromptCriteria"] = criteriaId;
            BtnAddSetDCriteria.Visible = false;
            BtnAddStepDCriteria.Visible = false;
            BtnAddPromptDCriteria.Visible = false;
            BtnUpdateStepDCriteria.Visible = false;
            BtnUpdatePromptDCriteria.Visible = true;
            BtnUpdateSetDCriteria.Visible = false;
            ClearCriteriaData();
            FillDropMeasure();
            EditSetCriteria(criteriaId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddCriteriaPopup();", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }






    protected void EditSetData(int setId)
    {
        objData = new clsData();
        string matchData = "";
        string itemData = "";
        //string[] arryMatchValue = new string[100];
        try
        {
            string selQuerry = "SELECT SetCd,SetName,Samples FROm DSTempSet WHERE DSTempSetId = " + setId;
            DataTable dtList = objData.ReturnDataTable(selQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {
                    txtBoxAddSet.Text = dtList.Rows[0]["SetCd"].ToString();
                    txtSetDescription.Text = dtList.Rows[0]["SetName"].ToString();
                    matchData = dtList.Rows[0]["Samples"].ToString();
                    if (matchData != null)
                    {
                        string[] arryMatchValue = new string[matchData.Length];
                        arryMatchValue = matchData.Split(',');

                        for (int i = 0; i < arryMatchValue.Length - 1; i++)
                        {

                            itemData = arryMatchValue[i].ToString();
                            ListItem item = new ListItem();
                            item.Text = itemData;
                            item.Value = itemData;
                            lstMatchSamples.Items.Add(item);
                        }

                    }

                    //  txtMatcSamples.Text = dtList.Rows[0]["Samples"].ToString();


                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void EditStepData(int stepId)
    {
        objData = new clsData();
        int setParentId = 0;
        try
        {
            string selQuerry = "SELECT DSTempStepId,DSTempSetId,StepCd,StepName FROM DSTempStep WHERE DSTempStepId = " + stepId;
            DataTable dtList = objData.ReturnDataTable(selQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {
                    txtStepName.Text = dtList.Rows[0]["StepCd"].ToString();
                    txtStepDesc.Text = dtList.Rows[0]["StepName"].ToString();
                    setParentId = Convert.ToInt32(dtList.Rows[0]["DSTempSetId"]);
                    if (setParentId != 0)
                    {
                        ddlSetData.SelectedValue = setParentId.ToString();
                    }
                    else ddlSetData.SelectedIndex = 0;


                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void EditSetCriteria(int criteriaId)
    {
        objData = new clsData();
        string criteriaType = "";
        int scoreReq = 0;
        int totalInstance = 0;
        int totalCorrInstance = 0;
        bool consecutveSess = false;
        bool isMultitchr = false;
        bool isIoReq = false;
        string colName = "";
        string measureType = "";
        string selQuerry = "";
        int columnId = 0;
        int measureId = 0;
        string ruleType = "";
        //  string cmpleteData = "";         

        try
        {

            selQuerry = "SELECT DsRule.DSTempRuleId,DsRule.DSTempSetColId,DsRule.DSTempSetColCalcId,DsRule.RuleType,DsRule.CriteriaType,DsRule.ScoreReq,DsRule.TotalInstance, " +
                               "  DsRule.TotCorrInstance,DsRule.ConsequetiveInd,DsRule.MultiTeacherReqInd,DsRule.IOAReqInd,DsCol.ColName,DsColCalc.CalcType FROM DSTempRule DsRule LEFT JOIN DSTempSetCol DsCol " +
                               "  ON DsRule.DSTempSetColId = DsCol.DSTempSetColId LEFT JOIN DSTempSetColCalc DsColCalc ON DsRule.DSTempSetColCalcId = DsColCalc.DSTempSetColCalcId WHERE DSTempRuleId = " + criteriaId;

            DataTable dtList = objData.ReturnDataTable(selQuerry, false);
            if (dtList != null)
            {
                if (dtList.Rows.Count > 0)
                {
                    criteriaType = dtList.Rows[0]["CriteriaType"].ToString();
                    scoreReq = Convert.ToInt32(dtList.Rows[0]["ScoreReq"]);
                    totalInstance = Convert.ToInt32(dtList.Rows[0]["TotalInstance"]);
                    totalCorrInstance = Convert.ToInt32(dtList.Rows[0]["TotCorrInstance"]);
                    consecutveSess = Convert.ToBoolean(dtList.Rows[0]["ConsequetiveInd"]);
                    isMultitchr = Convert.ToBoolean(dtList.Rows[0]["MultiTeacherReqInd"]);
                    isIoReq = Convert.ToBoolean(dtList.Rows[0]["IOAReqInd"]);
                    colName = dtList.Rows[0]["ColName"].ToString();
                    measureType = dtList.Rows[0]["CalcType"].ToString();
                    columnId = Convert.ToInt32(dtList.Rows[0]["DSTempSetColId"]);
                    measureId = Convert.ToInt32(dtList.Rows[0]["DSTempSetColCalcId"]);
                    ruleType = dtList.Rows[0]["RuleType"].ToString();

                    if (ruleType == "SET")
                    {
                        lblCriteriaName.Text = "Add Set";
                    }
                    else if (ruleType == "STEP")
                    {
                        lblCriteriaName.Text = "Add Step";
                    }
                    else if (ruleType == "PROMPT")
                    {
                        lblCriteriaName.Text = "Add Prompt";
                    }
                    try
                    {
                        ddlCriteriaType.SelectedValue = criteriaType;
                    }
                    catch
                    {
                        ddlCriteriaType.SelectedValue = "0";
                    }

                    if (isIoReq == true) rbtnIoaReq.SelectedValue = "1";
                    else rbtnIoaReq.SelectedValue = "0";
                    if (isMultitchr == true) rbtnMultitchr.SelectedValue = "1";
                    else rbtnMultitchr.SelectedValue = "0";
                    if (consecutveSess == true) rbtnConsectiveSes.SelectedValue = "1";
                    else rbtnConsectiveSes.SelectedValue = "0";
                    try
                    {
                        ddlTempColumn.SelectedValue = columnId.ToString();
                    }
                    catch
                    {
                        ddlTempColumn.SelectedIndex = 0;
                    }
                    FillMeasureDrop(columnId);
                    try
                    {
                        ddlTempMeasure.SelectedValue = measureId.ToString();
                    }
                    catch
                    {
                        ddlTempMeasure.SelectedIndex = 0;
                    }
                    txtRequiredScore.Text = scoreReq.ToString();

                    if (consecutveSess == true)
                    {
                        txtNumbrSessions.Text = totalInstance.ToString();
                    }
                    else
                    {
                        txtIns2.Text = totalInstance.ToString();
                        txtIns1.Text = totalCorrInstance.ToString();
                    }


                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    protected void FillMeasureDrop(int columnId)
    {
        objData = new clsData();
        try
        {
            string selQuerry = " SELECT DSTempSetColCalcId As Id, CalcType As Name  FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId;
            objData.ReturnDropDownForMeasureCriteria(selQuerry, ddlTempMeasure);
            if (ddlTempMeasure.Items.Count == 0)
            {
                ddlTempMeasure.Items.Insert(0, new ListItem("------------Select Column------------", "0"));
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void btnAddSet_Click(object sender, EventArgs e)
    {
        if (ViewState["HeaderId"] != null)
        {
            ClearSetData();
            BtnUpdateSetDetails.Visible = false;
            btnAddSetDetails.Visible = true;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddSet();", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
        }

    }


    protected void BtnAddStep_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["HeaderId"] != null)
            {
                ClearStepData();
                FillSetDrpList();
                BtnUpdateStep.Visible = false;
                btnAddStepDetails.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddStep();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void btnAddSetCriteria_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["HeaderId"] != null)
            {
                ClearCriteriaData();
                FillDropMeasure();                          //Fill dropdown for measure in criteria
                BtnAddSetDCriteria.Visible = true;
                BtnAddStepDCriteria.Visible = false;
                BtnAddPromptDCriteria.Visible = false;
                BtnUpdateSetDCriteria.Visible = false;
                BtnUpdateStepDCriteria.Visible = false;
                BtnUpdatePromptDCriteria.Visible = false;
                lblCriteriaName.Text = "Set Criteria";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddCriteriaPopup();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void btnAddStepCriteria_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["HeaderId"] != null)
            {
                ClearCriteriaData();
                FillDropMeasure();                          //Fill dropdown for measure in criteria
                BtnAddSetDCriteria.Visible = false;
                BtnAddStepDCriteria.Visible = true;
                BtnAddPromptDCriteria.Visible = false;
                BtnUpdateSetDCriteria.Visible = false;
                BtnUpdateStepDCriteria.Visible = false;
                BtnUpdatePromptDCriteria.Visible = false;
                lblCriteriaName.Text = "Step Criteria";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddCriteriaPopup();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void btnAddPrompt_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["HeaderId"] != null)
            {
                ClearCriteriaData();
                FillDropMeasure();                          //Fill dropdown for measure in criteria
                BtnAddSetDCriteria.Visible = false;
                BtnAddStepDCriteria.Visible = false;
                BtnAddPromptDCriteria.Visible = true;
                BtnUpdateSetDCriteria.Visible = false;
                BtnUpdateStepDCriteria.Visible = false;
                BtnUpdatePromptDCriteria.Visible = false;
                lblCriteriaName.Text = "Prompt Criteria";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AddCriteriaPopup();", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void ddlTempColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        objData = new clsData();
        int selectdColumnId = 0;
        string selQuerry = "";
        try
        {
            if (ddlTempColumn.SelectedIndex != 0)
            {
                selectdColumnId = Convert.ToInt32(ddlTempColumn.SelectedValue);
                selQuerry = "Select DSTempSetColCalcId as Id,CalcType as Name from DSTempSetColCalc where DSTempSetColId= " + selectdColumnId;
                objData.ReturnDropDownForMeasureCriteria(selQuerry, ddlTempMeasure);
                DataTable dttemp = objData.ReturnDataTable(selQuerry, false);
                if ((dttemp != null) && (dttemp.Rows.Count == 0))
                {
                    ddlTempMeasure.DataSource = dttemp;
                    ddlTempMeasure.DataBind();
                }

                if (ddlTempMeasure.Items.Count == 0)
                {
                    ddlTempMeasure.Items.Insert(0, new ListItem("---------------Select Measure--------------", "0"));
                }
            }

            else
            {
                ddlTempMeasure.Items.Clear();
                ddlTempMeasure.Items.Insert(0, new ListItem("---------------Select Measure--------------", "0"));
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void FillDropMeasure()
    {
        objData = new clsData();
        string selQuerry = "";
        int headerId = 0;

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        try
        {

            selQuerry = "SELECT DSTempSetColId As Id,ColName As Name FROM DSTempSetCol WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
            objData.ReturnDropDownForCriteria(selQuerry, ddlTempColumn);
            DataTable dttemp = objData.ReturnDataTable(selQuerry, false);
            if ((dttemp != null) && (dttemp.Rows.Count == 0))
            {
                ddlTempColumn.DataSource = dttemp;
                ddlTempColumn.DataBind();
            }

            if (ddlTempColumn.Items.Count == 0)
            {
                ddlTempColumn.Items.Insert(0, new ListItem(".................Select Column..............", "0"));
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }


    }


    protected void FillSetDrpList()
    {
        objData = new clsData();
        string strQuerry = "";
        int headerId = 0;
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        try
        {

            strQuerry = "SELECT DSTempSetId AS ID, SetCd As Name FROM DSTempSet WHERE DSTempHdrId = " + headerId + " AND ActiveInd = 'A'";
            objData.ReturnDropDownForStep(strQuerry, ddlSetData);
            DataTable dttemp = objData.ReturnDataTable(strQuerry, false);
            if ((dttemp != null) && (dttemp.Rows.Count == 0))
            {
                ddlSetData.DataSource = dttemp;
                ddlSetData.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }



    protected void BtnUpdateSetDetails_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string updateQuerry = "";
        int editSetId = 0;
        int headerId = 0;
        string setMatch = "";
        int length = 0;
        string matchSelctd = "";
        sess = (clsSession)Session["UserSession"];
        if (ViewState["EditSetId"] != null)
        {
            editSetId = Convert.ToInt32(ViewState["EditSetId"]);
        }
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (sess != null)
        {

            if (lstMatchSamples.Items.Count > 0)
            {
                string[] arryMatchValue = new string[lstMatchSamples.Items.Count];
                updateQuerry = "UPDATE DSTempSet SET Samples = '' WHERE DSTempSetId = " + editSetId;
                int index = objData.Execute(updateQuerry);

                for (int i = 0; i < lstMatchSamples.Items.Count; i++)
                {
                    arryMatchValue[i] = lstMatchSamples.Items[i].Value.ToString();
                }
                for (int arryInt = 0; arryInt < lstMatchSamples.Items.Count; arryInt++)
                {
                    setMatch += arryMatchValue[arryInt].ToString() + ",";
                }

                length = setMatch.Length;
                matchSelctd = setMatch.ToString().Substring(0, length - 1);

            }

            try
            {
                updateQuerry = "UPDATE DSTempSet SET SetCd = '" + clsGeneral.convertQuotes(txtBoxAddSet.Text.Trim()) + "', SetName = '" + clsGeneral.convertQuotes(txtSetDescription.Text.Trim()) + "',Samples='" + setMatch + "',ModifiedBy = " + sess.LoginId + ",ModifiedOn = GetDate()  WHERE DSTempSetId = " + editSetId;
                int index = objData.Execute(updateQuerry);

                if (IsMatchToSample(headerId) == true)
                {
                    if (matchSelctd != null)
                    {
                        MatchSampDef(headerId, matchSelctd, editSetId);                 // Match to Sample code 
                    }
                }


                ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseSetPopup();", true);

                GetSetData(headerId);
            }
            catch (Exception Ex)
            {
                string error = Ex.Message;
                tdMsgSet.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                throw Ex;
            }
        }

    }

    protected void BtnUpdateStep_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string updateQuerry = "";
        int editStepId = 0;
        int headerId = 0;
        int parentSetId = 0;
        sess = (clsSession)Session["UserSession"];

        if (ViewState["EditStepId"] != null)
        {
            editStepId = Convert.ToInt32(ViewState["EditStepId"]);
        }
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        if (ddlSetData.SelectedIndex != 0)
        {
            parentSetId = Convert.ToInt32(ddlSetData.SelectedValue);
        }
        try
        {
            if (sess != null)
            {
                updateQuerry = "UPDATE DSTempStep SET StepCd = '" + clsGeneral.convertQuotes(txtStepName.Text.Trim()) + "', StepName = '" + clsGeneral.convertQuotes(txtStepDesc.Text.Trim()) + "', DSTempSetId = " + parentSetId + ",ModifiedBy = " + sess.LoginId + ",ModifiedOn = GETDATE() WHERE DSTempStepId = " + editStepId;
                int index = objData.Execute(updateQuerry);

                ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseStepPopup();", true);
                GetStepData(headerId);
            }
        }
        catch (Exception Ex)
        {
            string error = Ex.Message;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
            throw Ex;
        }

    }


    protected void BtnUpdateSetDCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string updateQuerry = "";
        int setCriteriaId = 0;
        int headerId = 0;
        string totCorrectResp = "";
        string totalResp = "";
        sess = (clsSession)Session["UserSession"];
        //  int consctveSess = 0;
        //  string totalInstance = "";

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (ViewState["EditSetCriteria"] != null)
        {
            setCriteriaId = Convert.ToInt32(ViewState["EditSetCriteria"]);
        }
        try
        {
            if (sess != null)
            {
                if (rbtnConsectiveSes.SelectedValue == "0")
                {
                    totCorrectResp = txtIns2.Text.ToString();
                    totalResp = txtIns1.Text.ToString();
                }
                else
                {
                    totCorrectResp = txtNumbrSessions.Text.ToString();
                }

                if (ValidateCheck() == true)
                {

                    try
                    {
                        updateQuerry = "UPDATE DSTempRule SET CriteriaType = '" + ddlCriteriaType.SelectedValue.ToString() + "', IOAReqInd = '" + rbtnIoaReq.SelectedValue.ToString() + "', MultiTeacherReqInd = '" + rbtnMultitchr.SelectedValue.ToString() + "',DSTempSetColId = '" + ddlTempColumn.SelectedValue.ToString() + "',DSTempSetColCalcId = '" + ddlTempMeasure.SelectedValue.ToString() + "',ScoreReq = '" + clsGeneral.convertQuotes(txtRequiredScore.Text.Trim()) + "',ConsequetiveInd = '" + rbtnConsectiveSes.SelectedValue.ToString() + "',TotCorrInstance = '" + totalResp + "',TotalInstance = '" + totCorrectResp + "',ModifiedBy = " + sess.LoginId + ", ModifiedOn = GETDATE() WHERE DSTempRuleId = " + setCriteriaId;
                        int index = objData.Execute(updateQuerry);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseCriteriaPopup();", true);
                        GetSetCriteriaData(headerId);

                    }
                    catch (Exception Ex)
                    {
                        string error = Ex.Message;
                        tdMsgCriteria.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    protected void BtnUpdateStepDCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string updateQuerry = "";
        int setCriteriaId = 0;
        int headerId = 0;
        string totCorrectResp = "";
        string totalResp = "";
        sess = (clsSession)Session["UserSession"];
        //  int consctveSess = 0;
        //  string totalInstance = "";

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (ViewState["EditStepCriteria"] != null)
        {
            setCriteriaId = Convert.ToInt32(ViewState["EditStepCriteria"]);
        }
        try
        {
            if (sess != null)
            {
                if (rbtnConsectiveSes.SelectedValue == "0")
                {
                    totCorrectResp = txtIns2.Text.ToString();
                    totalResp = txtIns1.Text.ToString();
                }
                else
                {
                    totCorrectResp = txtNumbrSessions.Text.ToString();
                }

                if (ValidateCheck() == true)
                {

                    try
                    {
                        updateQuerry = "UPDATE DSTempRule SET CriteriaType = '" + ddlCriteriaType.SelectedValue.ToString() + "', IOAReqInd = '" + rbtnIoaReq.SelectedValue.ToString() + "', MultiTeacherReqInd = '" + rbtnMultitchr.SelectedValue.ToString() + "',DSTempSetColId = '" + ddlTempColumn.SelectedValue.ToString() + "',DSTempSetColCalcId = '" + ddlTempMeasure.SelectedValue.ToString() + "',ScoreReq = '" + clsGeneral.convertQuotes(txtRequiredScore.Text.Trim()) + "',ConsequetiveInd = '" + rbtnConsectiveSes.SelectedValue.ToString() + "',TotCorrInstance = '" + totalResp + "',TotalInstance = '" + totCorrectResp + "',ModifiedBy = " + sess.LoginId + ", ModifiedOn = GETDATE() WHERE DSTempRuleId = " + setCriteriaId;
                        int index = objData.Execute(updateQuerry);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseCriteriaPopup();", true);
                        GetStepCriteriaData(headerId);

                    }
                    catch (Exception Ex)
                    {
                        string error = Ex.Message;
                        tdMsgCriteria.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void BtnUpdatePromptDCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string updateQuerry = "";
        int setCriteriaId = 0;
        int headerId = 0;
        string totCorrectResp = "";
        string totalResp = "";
        sess = (clsSession)Session["UserSession"];
        //  int consctveSess = 0;
        //  string totalInstance = "";

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        if (ViewState["EditPromptCriteria"] != null)
        {
            setCriteriaId = Convert.ToInt32(ViewState["EditPromptCriteria"]);
        }
        try
        {
            if (sess != null)
            {
                if (rbtnConsectiveSes.SelectedValue == "0")
                {
                    totCorrectResp = txtIns2.Text.ToString();
                    totalResp = txtIns1.Text.ToString();
                }
                else
                {
                    totCorrectResp = txtNumbrSessions.Text.ToString();
                }

                if (ValidateCheck() == true)
                {

                    try
                    {
                        updateQuerry = "UPDATE DSTempRule SET CriteriaType = '" + ddlCriteriaType.SelectedValue.ToString() + "', IOAReqInd = '" + rbtnIoaReq.SelectedValue.ToString() + "', MultiTeacherReqInd = '" + rbtnMultitchr.SelectedValue.ToString() + "',DSTempSetColId = '" + ddlTempColumn.SelectedValue.ToString() + "',DSTempSetColCalcId = '" + ddlTempMeasure.SelectedValue.ToString() + "',ScoreReq = '" + clsGeneral.convertQuotes(txtRequiredScore.Text.Trim()) + "',ConsequetiveInd = '" + rbtnConsectiveSes.SelectedValue.ToString() + "',TotCorrInstance = '" + totalResp + "',TotalInstance = '" + totCorrectResp + "',ModifiedBy = " + sess.LoginId + ", ModifiedOn = GETDATE() WHERE DSTempRuleId = " + setCriteriaId;
                        int index = objData.Execute(updateQuerry);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel11, UpdatePanel11.GetType(), "", "CloseCriteriaPopup();", true);
                        GetPromptCriteriaData(headerId);

                    }
                    catch (Exception Ex)
                    {
                        string error = Ex.Message;
                        tdMsgCriteria.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void btnRemoveSet_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int setId = 0;
        int headerId = 0;
        string deltQuerry = "";
        string selQuerry = "";
        Button BtnDelt = (Button)sender;
        setId = Convert.ToInt32(BtnDelt.CommandArgument);
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            // Check the selected set has any child step.
            selQuerry = "SELECT DSTempStepId FROM DSTempStep WHERE DSTempHdrId = " + headerId + " AND DSTempSetId = " + setId + " AND ActiveInd = 'A'";
            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);

            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "AlertNotDelete();", true);
                }
                else
                {
                    deltQuerry = "UPDATE DSTempSet SET ActiveInd = 'D' WHERE DSTempSetId = " + setId;
                    int index = objData.Execute(deltQuerry);
                }
            }
            else
            {
                deltQuerry = "UPDATE DSTempSet SET ActiveInd = 'D' WHERE DSTempSetId = " + setId;
                int index = objData.Execute(deltQuerry);
            }




            GetSetData(headerId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    protected void btnRemoveStep_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int stepId = 0;
        int headerId = 0;
        string deltQuerry = "";
        Button BtnDelt = (Button)sender;
        stepId = Convert.ToInt32(BtnDelt.CommandArgument);
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            deltQuerry = "UPDATE DSTempStep SET ActiveInd = 'D' WHERE DSTempStepId = " + stepId;
            int index = objData.Execute(deltQuerry);

            GetStepData(headerId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void BtnRemoveSetCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        int setCriteriaId = 0;
        Button BtnDelt = (Button)sender;
        setCriteriaId = Convert.ToInt32(BtnDelt.CommandArgument);
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            string strQuerry = "UPDATE DSTempRule SET ActiveInd = 'D' WHERE DSTempRuleId = " + setCriteriaId;
            int index = objData.Execute(strQuerry);
            GetSetCriteriaData(headerId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    protected void BtnRemoveStepCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        int setCriteriaId = 0;
        Button BtnDelt = (Button)sender;
        setCriteriaId = Convert.ToInt32(BtnDelt.CommandArgument);
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            string strQuerry = "UPDATE DSTempRule SET ActiveInd = 'D' WHERE DSTempRuleId = " + setCriteriaId;
            int index = objData.Execute(strQuerry);
            GetStepCriteriaData(headerId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void BtnRemovePromptCriteria_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        int setCriteriaId = 0;
        Button BtnDelt = (Button)sender;
        setCriteriaId = Convert.ToInt32(BtnDelt.CommandArgument);
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            string strQuerry = "UPDATE DSTempRule SET ActiveInd = 'D' WHERE DSTempRuleId = " + setCriteriaId;
            int index = objData.Execute(strQuerry);
            GetPromptCriteriaData(headerId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }




    protected void ddlColumnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlColumnType.SelectedValue == "+/-")
        {
            PlusMinusDiv.Visible = true;
            promptDiv.Visible = false;
            TextDiv.Visible = false;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = false;
        }
        else if (ddlColumnType.SelectedValue == "Prompt")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = true;
            TextDiv.Visible = false;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = false;
        }

        else if (ddlColumnType.SelectedValue == "Text")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = false;
            TextDiv.Visible = true;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = false;
        }
        else if (ddlColumnType.SelectedValue == "Duration")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = false;
            TextDiv.Visible = false;
            DurationDiv.Visible = true;
            FrequencyDiv.Visible = false;
        }
        else if (ddlColumnType.SelectedValue == "Frequency")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = false;
            TextDiv.Visible = false;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = true;
        }
    }

    protected void BtnAddMeasure_Click(object sender, EventArgs e)
    {
        if (ViewState["HeaderId"] != null)
        {
            BtnSaveMeasure.Visible = true; ;
            BtnUpdateMeasure.Visible = false;
            PlusMinusDiv.Visible = true;
            promptDiv.Visible = false;
            TextDiv.Visible = false;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = false;
            ClearData();
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "EditMeasurePopup();", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
        }

    }
    protected void ClearData()
    {
        txtColumnName.Text = "";
        ddlColumnType.SelectedValue = "+/-";
        rdbplusMinus.SelectedValue = "+";
        txtplusCorrectResponse.Text = "";
        txtPlusIncorrectResp.Text = "";
        chkplusIncludeMistrial.Checked = false;
        txtplusIncludeMistrial.Text = "";
        chkplusAccuracy.Checked = false;
        chkplusAccuracy.Enabled = true;
        txtplusAccuracy.Text = "";
        chkplusindependent.Checked = false;
        chkplusindependent.Enabled = true;
        txtplusIndependent.Text = "";
        chkPlusPromptPerc.Checked = false;
        chkPlusPromptPerc.Enabled = true;
        txtPlusPromptPerc.Text = "";

        //   chkCurrentPrompt.Checked = false;
        txtpromptSelectPrompt.Text = "";
        txtPromptIncrctResp.Text = "";
        txtPromptIncMisTrial.Text = "";
        chkPromptInclMisTrial.Checked = false;
        chkPrompPercAccuracy.Checked = false;
        chkPrompPercAccuracy.Enabled = true;
        chkPromptPercPrompt.Checked = false;
        chkPromptPercPrompt.Enabled = true;
        txtPromptpecPrompt.Text = "";
        chkPercIndependent.Checked = false;
        chkPercIndependent.Enabled = true;
        txtPromptIndependent.Text = "";
        txtPromptAccuracy.Text = "";

        txtTextCrctResponse.Text = "";
        txtTextInCrctResp.Text = "";
        chkTxtIncMisTrial.Checked = false;
        chkTxtIncMisTrial.Enabled = true;
        txtTxtIncMisTrial.Text = "";
        chkTxtNa.Checked = false;
        chkTxtNa.Enabled = true;
        txtTxtNA.Text = "";
        chkTextCustomize.Checked = false;
        chkTextCustomize.Enabled = true;
        txtTxtCustomize.Text = "";

        txtDurCorrectResponse.Text = "";
        txtDurIncrctResp.Text = "";
        chkDurIncludeMistrial.Checked = false;
        txtDurInclMisTrial.Text = "";
        chkDurAverage.Checked = false;
        chkDurAverage.Enabled = true;
        txtFreqCorrectResponse.Text = "";
        txtfreqIncrctResp.Text = "";
        chkFreqIncludeMistrial.Checked = false;
        txtFreqIncludeMistrial.Text = "";
        chkFrequency.Checked = false;
        chkFrequency.Enabled = true;
        txtFrequency.Text = "";
        ddlColumnType.Enabled = true;
        txtDurAverage.Text = "";
        txtDurTotalDuration.Text = "";
        divBtn.Visible = false;

        tdMsgMeasure.InnerHtml = "";
    }

    protected void ClearSetData()
    {
        txtBoxAddSet.Text = "";
        txtSetDescription.Text = "";
        tdMsgSet.InnerHtml = "";
        lstMatchSamples.Items.Clear();
        txtMatcSamples.Text = "";

    }

    protected void ClearStepData()
    {
        txtStepName.Text = "";
        txtStepDesc.Text = "";
        tdMsgStep.InnerHtml = "";
    }

    protected void ClearCriteriaData()
    {
        ddlCriteriaType.SelectedIndex = 0;
        //for (int i = 0; i < rbtnIoaReq.Items.Count; i++)
        //{
        //    rbtnIoaReq.Items[i].Selected = false;
        //}

        //for (int i = 0; i < rbtnMultitchr.Items.Count; i++)
        //{
        //    rbtnMultitchr.Items[i].Selected = false;
        //}
        //ddlTempColumn.SelectedValue = "0";
        //for (int i = 0; i < rbtnConsectiveSes.Items.Count; i++)
        //{
        //    rbtnConsectiveSes.Items[i].Selected = false;
        //}

        rbtnIoaReq.SelectedValue = "0";
        rbtnMultitchr.SelectedValue = "0";
        rbtnConsectiveSes.SelectedValue = "0";
        ddlTempMeasure.SelectedValue = "0";
        txtNumbrSessions.Text = "";
        txtRequiredScore.Text = "";
        txtIns1.Text = "";
        txtIns2.Text = "";
        txtIns2.ReadOnly = false;
        txtIns1.ReadOnly = false;
        tdMsgCriteria.InnerHtml = "";

    }




    protected void BtnSaveMeasure_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        SqlTransaction Transs = null;
        SqlConnection con = objData.Open();
        string insertQuerry = "";
        int returnId = 0;
        int headerId = 0;
        int inclMistrial = 0;
        int index = 0;
        sess = (clsSession)Session["UserSession"];
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        try
        {
            if (sess != null)
            {
                if (ddlColumnType.SelectedValue == "+/-")
                {
                    clsData.blnTrans = true;
                    Transs = con.BeginTransaction();
                    try
                    {
                        if (chkplusIncludeMistrial.Checked == true)
                        {
                            inclMistrial = 1;
                        }
                        insertQuerry = "Insert Into dbo.DSTempSetCol (SchoolId,DSTempHdrId,ColName,ColTypeCd,CorrResp,CorrRespDesc,InCorrRespDesc,IncMisTrialInd,MisTrialDesc " +
                              ",CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                              "Values(" + sess.SchoolId + "," + headerId + ",'" + clsGeneral.convertQuotes(txtColumnName.Text.Trim()) + "','" + Convert.ToString(ddlColumnType.SelectedValue) + "' ,'" + Convert.ToString(rdbplusMinus.SelectedValue) + "','" + clsGeneral.convertQuotes(txtplusCorrectResponse.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtPlusIncorrectResp.Text.Trim()) + "'," + inclMistrial + ",'" + clsGeneral.convertQuotes(txtplusIncludeMistrial.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                        returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));

                        if (chkplusAccuracy.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                            "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkplusAccuracy.Text) + "','" + clsGeneral.convertQuotes(txtplusAccuracy.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }
                        if (chkPlusPromptPerc.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                            "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkPlusPromptPerc.Text) + "','" + clsGeneral.convertQuotes(txtPlusPromptPerc.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }

                        if (chkplusindependent.Checked == true)
                        {

                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                            "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkplusindependent.Text) + "','" + clsGeneral.convertQuotes(txtplusIndependent.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }

                        objData.CommitTransation(Transs, con);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                        GetMeasureData(headerId);
                        SetLessonProcedure(headerId);

                    }
                    catch (Exception Ex)
                    {
                        objData.RollBackTransation(Transs, con);
                        string error = Ex.Message;
                        tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                    }
                }
                else if (ddlColumnType.SelectedValue == "Prompt")
                {

                    clsData.blnTrans = true;
                    Transs = con.BeginTransaction();

                    try
                    {
                        if (chkPromptInclMisTrial.Checked == true)
                        {
                            inclMistrial = 1;
                        }
                        //if (chkCurrentPrompt.Checked == true)
                        //{
                        //    currentPrompt = 1;
                        //}

                        insertQuerry = "Insert Into dbo.DSTempSetCol (SchoolId,DSTempHdrId,ColName,ColTypeCd,CorrResp,CorrRespDesc,InCorrRespDesc,IncMisTrialInd,MisTrialDesc " +
                               ",CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                               "Values(" + sess.SchoolId + "," + headerId + ",'" + clsGeneral.convertQuotes(txtColumnName.Text.Trim()) + "','" + Convert.ToString(ddlColumnType.SelectedValue) + "' ,'" + clsGeneral.convertQuotes(txtpromptSelectPrompt.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtpromptSelectPrompt.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtPromptIncrctResp.Text.Trim()) + "'," + inclMistrial + ",'" + clsGeneral.convertQuotes(txtPromptIncMisTrial.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                        returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));

                        if (chkPrompPercAccuracy.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                           "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkPrompPercAccuracy.Text) + "','" + clsGeneral.convertQuotes(txtPromptAccuracy.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }

                        if (chkPromptPercPrompt.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                            "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkPromptPercPrompt.Text) + "','" + clsGeneral.convertQuotes(txtPromptpecPrompt.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }
                        if (chkPercIndependent.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                          "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkPercIndependent.Text) + "','" + clsGeneral.convertQuotes(txtPromptIndependent.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }

                        objData.CommitTransation(Transs, con);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                        GetMeasureData(headerId);
                        SetLessonProcedure(headerId);
                    }
                    catch (Exception Ex)
                    {
                        objData.RollBackTransation(Transs, con);
                        string error = Ex.Message;
                        tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                    }

                }
                else if (ddlColumnType.SelectedValue == "Text")
                {
                    clsData.blnTrans = true;
                    Transs = con.BeginTransaction();

                    try
                    {
                        if (chkTxtIncMisTrial.Checked == true)
                        {
                            inclMistrial = 1;
                        }

                        insertQuerry = "Insert Into dbo.DSTempSetCol (SchoolId,DSTempHdrId,ColName,ColTypeCd,CorrResp,CorrRespDesc,InCorrRespDesc,IncMisTrialInd,MisTrialDesc " +
                               ",CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                               "Values(" + sess.SchoolId + "," + headerId + ",'" + clsGeneral.convertQuotes(txtColumnName.Text.Trim()) + "','" + Convert.ToString(ddlColumnType.SelectedValue) + "' ,'" + clsGeneral.convertQuotes(txtTextCrctResponse.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtTextCrctResponse.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtTextInCrctResp.Text.Trim()) + "'," + inclMistrial + ",'" + clsGeneral.convertQuotes(txtTxtIncMisTrial.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                        returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));

                        if (chkTxtNa.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                            "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkTxtNa.Text) + "','" + clsGeneral.convertQuotes(txtTxtNA.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }
                        if (chkTextCustomize.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                          "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkTextCustomize.Text) + "','" + clsGeneral.convertQuotes(txtTxtCustomize.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }

                        objData.CommitTransation(Transs, con);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                        GetMeasureData(headerId);
                        SetLessonProcedure(headerId);
                    }
                    catch (Exception Ex)
                    {
                        objData.RollBackTransation(Transs, con);
                        string error = Ex.Message;
                        tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                    }
                }
                else if (ddlColumnType.SelectedValue == "Duration")
                {
                    clsData.blnTrans = true;
                    Transs = con.BeginTransaction();

                    try
                    {
                        if (chkDurIncludeMistrial.Checked == true)
                        {
                            inclMistrial = 1;
                        }

                        insertQuerry = "Insert Into dbo.DSTempSetCol (SchoolId,DSTempHdrId,ColName,ColTypeCd,CorrResp,CorrRespDesc,InCorrRespDesc,IncMisTrialInd,MisTrialDesc " +
                               ",CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                               "Values(" + sess.SchoolId + "," + headerId + ",'" + clsGeneral.convertQuotes(txtColumnName.Text.Trim()) + "','" + Convert.ToString(ddlColumnType.SelectedValue) + "' ,'" + clsGeneral.convertQuotes(txtDurCorrectResponse.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDurCorrectResponse.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDurIncrctResp.Text.Trim()) + "'," + inclMistrial + ",'" + clsGeneral.convertQuotes(txtDurInclMisTrial.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                        returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));

                        if (chkDurAverage.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                            "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkDurAverage.Text) + "','" + clsGeneral.convertQuotes(txtDurAverage.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }
                        if (chkDurTotalDur.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                          "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkDurTotalDur.Text) + "','" + clsGeneral.convertQuotes(txtDurTotalDuration.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }

                        objData.CommitTransation(Transs, con);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                        GetMeasureData(headerId);
                        SetLessonProcedure(headerId);
                    }
                    catch (Exception Ex)
                    {
                        objData.RollBackTransation(Transs, con);
                        string error = Ex.Message;
                        tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                    }
                }
                else if (ddlColumnType.SelectedValue == "Frequency")
                {
                    clsData.blnTrans = true;
                    Transs = con.BeginTransaction();

                    try
                    {
                        if (chkFreqIncludeMistrial.Checked == true)
                        {
                            inclMistrial = 1;
                        }

                        insertQuerry = "Insert Into dbo.DSTempSetCol (SchoolId,DSTempHdrId,ColName,ColTypeCd,CorrResp,CorrRespDesc,InCorrRespDesc,IncMisTrialInd,MisTrialDesc " +
                               ",CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                               "Values(" + sess.SchoolId + "," + headerId + ",'" + clsGeneral.convertQuotes(txtColumnName.Text.Trim()) + "','" + Convert.ToString(ddlColumnType.SelectedValue) + "' ,'" + clsGeneral.convertQuotes(txtFreqCorrectResponse.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtFreqCorrectResponse.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtfreqIncrctResp.Text.Trim()) + "'," + inclMistrial + ",'" + clsGeneral.convertQuotes(txtFreqIncludeMistrial.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                        returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));

                        if (chkFrequency.Checked == true)
                        {
                            insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                            "Values(" + sess.SchoolId + "," + returnId + ",'" + Convert.ToString(chkFrequency.Text) + "','" + clsGeneral.convertQuotes(txtFrequency.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                        }

                        objData.CommitTransation(Transs, con);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                        GetMeasureData(headerId);
                        SetLessonProcedure(headerId);
                    }
                    catch (Exception Ex)
                    {
                        objData.RollBackTransation(Transs, con);
                        string error = Ex.Message;
                        tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Insertion Failed! <br> '" + error + "' ");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void BtnUpdateMeasure_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int columnId = 0;
        string updateQuerry = "";
        string insertQuerry = "";
        string deltQuerry = "";
        SqlTransaction Transs = null;
        int returnId = 0;
        int inclMistrial = 0;
        int index = 0;
        int headerId = 0;
        string measureValue = "";
        sess = (clsSession)Session["UserSession"];
        if (ViewState["EditValue"] != null)
        {
            columnId = Convert.ToInt32(ViewState["EditValue"]);
        }
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        try
        {
            if (sess != null)
            {
                string selQuerry = "SELECT DSTempRuleId FROM DSTempRule WHERE DSTempSetColId = " + columnId + " AND ActiveInd = 'A'";        // Check the currrent column assigned to any criteria. If so, it will ignore the updation.
                DataTable dtCheckData = objData.ReturnDataTable(selQuerry, false);

                SqlConnection con = objData.Open();
                clsData.blnTrans = true;
                Transs = con.BeginTransaction();

                updateQuerry = "UPDATE DSTempSetCol SET ColName = '" + clsGeneral.convertQuotes(txtColumnName.Text.Trim()) + "', ColTypeCd = '" + Convert.ToString(ddlColumnType.SelectedValue) + "',ModifiedBy = " + sess.LoginId + ",ModifiedOn = getdate() WHERE DSTempSetColId = " + columnId;
                returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));

                if (dtCheckData != null && dtCheckData.Rows.Count > 0)
                {

                    if (ddlColumnType.SelectedValue == "+/-")
                    {
                        try
                        {
                            if (chkplusIncludeMistrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }

                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + Convert.ToString(rdbplusMinus.SelectedValue) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtplusCorrectResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtPlusIncorrectResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtplusIncludeMistrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));

                            if (chkplusAccuracy.Enabled == true)
                            {
                                measureValue = "%Accuracy";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));

                                if (chkplusAccuracy.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                    "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkplusAccuracy.Text) + "','" + clsGeneral.convertQuotes(txtplusAccuracy.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }
                            if (chkPlusPromptPerc.Enabled == true)
                            {
                                measureValue = "%Prompted";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));

                                if (chkPlusPromptPerc.Checked == true)
                                {

                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                    "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPlusPromptPerc.Text) + "','" + clsGeneral.convertQuotes(txtPlusPromptPerc.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }
                            if (chkplusindependent.Enabled == true)
                            {
                                measureValue = "%Independent";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkplusindependent.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                    "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkplusindependent.Text) + "','" + clsGeneral.convertQuotes(txtplusIndependent.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);

                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }


                    else if (ddlColumnType.SelectedValue == "Prompt")
                    {

                        try
                        {
                            if (chkPromptInclMisTrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }


                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtpromptSelectPrompt.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtpromptSelectPrompt.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtPromptIncrctResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtPromptIncMisTrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));

                            if (chkPrompPercAccuracy.Enabled == true)
                            {
                                measureValue = "%Accuracy";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));

                                if (chkPrompPercAccuracy.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                   "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPrompPercAccuracy.Text) + "','" + clsGeneral.convertQuotes(txtPromptAccuracy.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }
                            if (chkPromptPercPrompt.Enabled == true)
                            {
                                measureValue = "%Prompted";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkPromptPercPrompt.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                    "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPromptPercPrompt.Text) + "','" + clsGeneral.convertQuotes(txtPromptpecPrompt.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }
                            if (chkPercIndependent.Enabled == true)
                            {
                                measureValue = "%Independent";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkPercIndependent.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                  "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPercIndependent.Text) + "','" + clsGeneral.convertQuotes(txtPromptIndependent.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }

                    }
                    else if (ddlColumnType.SelectedValue == "Text")
                    {

                        try
                        {
                            if (chkTxtIncMisTrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }


                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtTextCrctResponse.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtTextCrctResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtTextInCrctResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtTxtIncMisTrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));

                            if (chkTxtNa.Enabled == true)
                            {
                                measureValue = "NA";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkTxtNa.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                    "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkTxtNa.Text) + "','" + clsGeneral.convertQuotes(txtTxtNA.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }
                            if (chkTextCustomize.Enabled == true)
                            {
                                measureValue = "Customize";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkTextCustomize.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                  "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkTextCustomize.Text) + "','" + clsGeneral.convertQuotes(txtTxtCustomize.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }
                    else if (ddlColumnType.SelectedValue == "Duration")
                    {

                        try
                        {
                            if (chkDurIncludeMistrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }

                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtDurCorrectResponse.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtDurCorrectResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtDurIncrctResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtDurInclMisTrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));

                            if (chkDurAverage.Enabled == true)
                            {
                                measureValue = "Avg Duration";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkDurAverage.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                    "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkDurAverage.Text) + "','" + clsGeneral.convertQuotes(txtDurAverage.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }
                            if (chkDurTotalDur.Enabled == true)
                            {
                                measureValue = "Total Duration";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkDurTotalDur.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                  "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkDurTotalDur.Text) + "','" + clsGeneral.convertQuotes(txtDurTotalDuration.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }
                    else if (ddlColumnType.SelectedValue == "Frequency")
                    {

                        try
                        {
                            if (chkFreqIncludeMistrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }

                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtFreqCorrectResponse.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtFreqCorrectResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtfreqIncrctResp.Text.Trim()) + "',IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtFreqIncludeMistrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));

                            if (chkFrequency.Enabled == true)
                            {
                                measureValue = "Frequency";
                                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureValue + "' And ActiveInd = 'A'";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));
                                if (chkFrequency.Checked == true)
                                {
                                    insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                    "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkFrequency.Text) + "','" + clsGeneral.convertQuotes(txtFrequency.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                                }
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }
                }
                else
                {

                    deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId;
                    returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(deltQuerry, con, Transs));

                    if (ddlColumnType.SelectedValue == "+/-")
                    {
                        try
                        {
                            if (chkplusIncludeMistrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }

                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + Convert.ToString(rdbplusMinus.SelectedValue) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtplusCorrectResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtPlusIncorrectResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtplusIncludeMistrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));

                            if (chkplusAccuracy.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkplusAccuracy.Text) + "','" + clsGeneral.convertQuotes(txtplusAccuracy.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }
                            if (chkPlusPromptPerc.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPlusPromptPerc.Text) + "','" + clsGeneral.convertQuotes(txtPlusPromptPerc.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }
                            if (chkplusindependent.Checked == true)
                            {

                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkplusindependent.Text) + "','" + clsGeneral.convertQuotes(txtplusIndependent.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);

                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }
                    else if (ddlColumnType.SelectedValue == "Prompt")
                    {

                        try
                        {
                            if (chkPromptInclMisTrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }


                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtpromptSelectPrompt.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtpromptSelectPrompt.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtPromptIncrctResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtPromptIncMisTrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));


                            if (chkPrompPercAccuracy.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                               "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPrompPercAccuracy.Text) + "','" + clsGeneral.convertQuotes(txtPromptAccuracy.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }

                            if (chkPromptPercPrompt.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPromptPercPrompt.Text) + "','" + clsGeneral.convertQuotes(txtPromptpecPrompt.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }
                            if (chkPercIndependent.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                              "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkPercIndependent.Text) + "','" + clsGeneral.convertQuotes(txtPromptIndependent.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }

                    }
                    else if (ddlColumnType.SelectedValue == "Text")
                    {

                        try
                        {
                            if (chkTxtIncMisTrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }


                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtTextCrctResponse.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtTextCrctResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtTextInCrctResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtTxtIncMisTrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));


                            if (chkTxtNa.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkTxtNa.Text) + "','" + clsGeneral.convertQuotes(txtTxtNA.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }
                            if (chkTextCustomize.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                              "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkTextCustomize.Text) + "','" + clsGeneral.convertQuotes(txtTxtCustomize.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }
                    else if (ddlColumnType.SelectedValue == "Duration")
                    {

                        try
                        {
                            if (chkDurIncludeMistrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }

                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtDurCorrectResponse.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtDurCorrectResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtDurIncrctResp.Text.Trim()) + "' ,IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtDurInclMisTrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));


                            if (chkDurAverage.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkDurAverage.Text) + "','" + clsGeneral.convertQuotes(txtDurAverage.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }
                            if (chkDurTotalDur.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                              "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkDurTotalDur.Text) + "','" + clsGeneral.convertQuotes(txtDurTotalDuration.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }
                    else if (ddlColumnType.SelectedValue == "Frequency")
                    {

                        try
                        {
                            if (chkFreqIncludeMistrial.Checked == true)
                            {
                                inclMistrial = 1;
                            }

                            updateQuerry = "UPDATE DSTempSetCol SET CorrResp = '" + clsGeneral.convertQuotes(txtFreqCorrectResponse.Text.Trim()) + "' ,CorrRespDesc = '" + clsGeneral.convertQuotes(txtFreqCorrectResponse.Text.Trim()) + "',InCorrRespDesc = '" + clsGeneral.convertQuotes(txtfreqIncrctResp.Text.Trim()) + "',IncMisTrialInd = " + inclMistrial + ", MisTrialDesc = '" + clsGeneral.convertQuotes(txtFreqIncludeMistrial.Text.Trim()) + "' WHERE DSTempSetColId = " + columnId;
                            returnId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(updateQuerry, con, Transs));


                            if (chkFrequency.Checked == true)
                            {
                                insertQuerry = "Insert Into DSTempSetColCalc (SchoolId,DSTempSetColId,CalcType,CalcRptLabel,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ActiveInd) " +
                                                "Values(" + sess.SchoolId + "," + columnId + ",'" + Convert.ToString(chkFrequency.Text) + "','" + clsGeneral.convertQuotes(txtFrequency.Text.Trim()) + "'," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate(),'A') ";
                                index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(insertQuerry, con, Transs));
                            }

                            objData.CommitTransation(Transs, con);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "CloseMeasure();", true);
                            GetMeasureData(headerId);
                            SetLessonProcedure(headerId);
                        }
                        catch (Exception Ex)
                        {
                            objData.RollBackTransation(Transs, con);
                            string error = Ex.Message;
                            tdMsgMeasure.InnerHtml = clsGeneral.failedMsg("Updation Failed! <br> '" + error + "' ");
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    //protected void DeltSelctdMeasure(int columnId, string measureData)
    //{
    //    objData = new clsData();
    //    string deltQuerry = "";
    //    deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND CalcType = '" + measureData + "' And ActiveInd = 'A'";
    //    objData.Execute(deltQuerry);
    //}

    protected void BtnRemove_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int columnId = 0;
        int index = 0;
        int headerId = 0;
        string deltQuerry = "";
        Button BtnDelt = (Button)sender;
        columnId = Convert.ToInt32(BtnDelt.CommandArgument);
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {

            string selQuerry = "SELECT DSTempRuleId FROM DSTempRule WHERE DSTempSetColId = " + columnId + " AND ActiveInd = 'A'";
            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel5, UpdatePanel5.GetType(), "", "AlertNotDelete();", true);
                }
                else
                {

                    deltQuerry = "Update DSTempSetCol SET ActiveInd = 'D'  WHERE DSTempSetColId = " + columnId;
                    index = objData.Execute(deltQuerry);
                    deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId;
                    index = objData.Execute(deltQuerry);
                }
            }
            else
            {
                deltQuerry = "Update DSTempSetCol SET ActiveInd = 'D'  WHERE DSTempSetColId = " + columnId;
                index = objData.Execute(deltQuerry);
                deltQuerry = "DELETE FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId;
                index = objData.Execute(deltQuerry);
            }

            GetMeasureData(headerId);   // Bind the DataList Measure Details
            SetLessonProcedure(headerId);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }


    }




    protected void btnEditMeasure_Click(object sender, EventArgs e)
    {
        int columnId = 0;
        try
        {
            Button btnEdit = (Button)sender;
            columnId = Convert.ToInt32(btnEdit.CommandArgument);
            ViewState["EditValue"] = columnId;
            BtnSaveMeasure.Visible = false;
            BtnUpdateMeasure.Visible = true;
            ClearData();
            EditMeasureData(columnId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "EditMeasurePopup();", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void EditMeasureData(int columnId)
    {
        objData = new clsData();
        string selQuerry = "";
        string columnType = "";
        string calcType = "";
        string calcDesc = "";
        int inclMistrial = 0;
        string mistrialDesc = "";
        string crctResponse = "";
        string crctResponseDesc = "";
        string incorrctResp = "";
        int colCalcId = 0;
        try
        {

            selQuerry = "SELECT ColName,ColTypeCd,CorrResp,CorrRespDesc,InCorrRespDesc,IncMisTrialInd,MisTrialDesc FROM DSTempSetCol WHERE DSTempSetColId = " + columnId + " AND ActiveInd = 'A'";
            DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    txtColumnName.Text = dtNew.Rows[0]["ColName"].ToString();
                    inclMistrial = Convert.ToInt32(dtNew.Rows[0]["IncMisTrialInd"]);
                    mistrialDesc = dtNew.Rows[0]["MisTrialDesc"].ToString();
                    crctResponse = dtNew.Rows[0]["CorrResp"].ToString();
                    crctResponseDesc = dtNew.Rows[0]["CorrRespDesc"].ToString();
                    incorrctResp = dtNew.Rows[0]["InCorrRespDesc"].ToString();
                    columnType = dtNew.Rows[0]["ColTypeCd"].ToString();
                    ddlColumnType.SelectedValue = columnType;
                    DivMeasureVisibility(columnType);
                    selQuerry = "SELECT DSTempSetColCalcId,CalcType,CalcRptLabel FROM DSTempSetColCalc WHERE DSTempSetColId = " + columnId + " AND ActiveInd = 'A'";
                    DataTable dtList = objData.ReturnDataTable(selQuerry, false);

                    if (dtList != null)
                    {
                        if (dtList.Rows.Count > 0)
                        {
                            if (columnType == "+/-")
                            {
                                if (crctResponse == "+")
                                {
                                    rdbplusMinus.SelectedValue = "+";
                                }
                                else rdbplusMinus.SelectedValue = "-";
                                txtplusCorrectResponse.Text = crctResponseDesc.ToString();
                                txtPlusIncorrectResp.Text = incorrctResp.ToString();

                                if (inclMistrial == 1)
                                {
                                    chkplusIncludeMistrial.Checked = true;
                                    txtplusIncludeMistrial.Text = mistrialDesc;
                                }
                                for (int i = 0; i < dtList.Rows.Count; i++)
                                {
                                    calcType = dtList.Rows[i]["CalcType"].ToString();
                                    calcDesc = dtList.Rows[i]["CalcRptLabel"].ToString();
                                    colCalcId = Convert.ToInt32(dtList.Rows[i]["DSTempSetColCalcId"]);


                                    if (calcType == "%Accuracy")
                                    {
                                        chkplusAccuracy.Checked = true;
                                        txtplusAccuracy.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkplusAccuracy.Enabled = false;
                                        }
                                    }
                                    if (calcType == "%Prompted")
                                    {
                                        chkPlusPromptPerc.Checked = true;
                                        txtPlusPromptPerc.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkPlusPromptPerc.Enabled = false;
                                        }
                                    }
                                    if (calcType == "%Independent")
                                    {
                                        chkplusindependent.Checked = true;
                                        txtplusIndependent.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkplusindependent.Enabled = false;
                                        }
                                    }

                                }
                            }

                            else if (columnType == "Prompt")
                            {
                                if (inclMistrial == 1)
                                {
                                    chkPromptInclMisTrial.Checked = true;
                                    txtPromptIncMisTrial.Text = mistrialDesc;
                                }
                                txtpromptSelectPrompt.Text = crctResponseDesc;
                                txtPromptIncrctResp.Text = incorrctResp.ToString();

                                for (int i = 0; i < dtList.Rows.Count; i++)
                                {
                                    calcType = dtList.Rows[i]["CalcType"].ToString();
                                    calcDesc = dtList.Rows[i]["CalcRptLabel"].ToString();
                                    colCalcId = Convert.ToInt32(dtList.Rows[i]["DSTempSetColCalcId"]);

                                    if (calcType == "%Accuracy")
                                    {
                                        chkPrompPercAccuracy.Checked = true;
                                        txtPromptAccuracy.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkPrompPercAccuracy.Enabled = false;
                                        }
                                    }
                                    if (calcType == "%Prompted")
                                    {
                                        chkPromptPercPrompt.Checked = true;
                                        txtPromptpecPrompt.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkPromptPercPrompt.Enabled = false;
                                        }

                                    }
                                    if (calcType == "%Independent")
                                    {
                                        chkPercIndependent.Checked = true;
                                        txtPromptIndependent.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkPercIndependent.Enabled = false;
                                        }
                                    }
                                }

                            }
                            else if (columnType == "Text")
                            {
                                txtTextCrctResponse.Text = crctResponseDesc.ToString();
                                txtTextInCrctResp.Text = incorrctResp.ToString();
                                if (inclMistrial == 1)
                                {
                                    chkTxtIncMisTrial.Checked = true;
                                    txtTxtIncMisTrial.Text = mistrialDesc;
                                }

                                for (int i = 0; i < dtList.Rows.Count; i++)
                                {
                                    calcType = dtList.Rows[i]["CalcType"].ToString();
                                    calcDesc = dtList.Rows[i]["CalcRptLabel"].ToString();
                                    colCalcId = Convert.ToInt32(dtList.Rows[i]["DSTempSetColCalcId"]);

                                    if (calcType == "NA")
                                    {
                                        chkTxtNa.Checked = true;
                                        txtTxtNA.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkTxtNa.Enabled = false;
                                        }
                                    }
                                    if (calcType == "Customize")
                                    {
                                        chkTextCustomize.Checked = true;
                                        txtTxtCustomize.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkTextCustomize.Enabled = false;
                                        }
                                    }
                                }
                            }

                            else if (columnType == "Duration")
                            {
                                txtDurCorrectResponse.Text = crctResponseDesc.ToString();
                                txtDurIncrctResp.Text = incorrctResp.ToString();
                                if (inclMistrial == 1)
                                {
                                    chkDurIncludeMistrial.Checked = true;
                                    txtDurInclMisTrial.Text = mistrialDesc;
                                }
                                for (int i = 0; i < dtList.Rows.Count; i++)
                                {
                                    calcType = dtList.Rows[i]["CalcType"].ToString();
                                    calcDesc = dtList.Rows[i]["CalcRptLabel"].ToString();
                                    colCalcId = Convert.ToInt32(dtList.Rows[i]["DSTempSetColCalcId"]);

                                    if (calcType == "Avg Duration")
                                    {
                                        chkDurAverage.Checked = true;
                                        txtDurAverage.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkDurAverage.Enabled = false;
                                        }
                                    }
                                    if (calcType == "Total Duration")
                                    {
                                        chkDurTotalDur.Checked = true;
                                        txtDurTotalDuration.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkDurTotalDur.Enabled = false;
                                        }
                                    }

                                }
                            }

                            else if (columnType == "Frequency")
                            {
                                txtFreqCorrectResponse.Text = crctResponseDesc.ToString();
                                txtfreqIncrctResp.Text = incorrctResp.ToString();
                                if (inclMistrial == 1)
                                {
                                    chkFreqIncludeMistrial.Checked = true;
                                    txtFreqIncludeMistrial.Text = mistrialDesc;
                                }

                                for (int i = 0; i < dtList.Rows.Count; i++)
                                {
                                    calcType = dtList.Rows[i]["CalcType"].ToString();
                                    calcDesc = dtList.Rows[i]["CalcRptLabel"].ToString();
                                    colCalcId = Convert.ToInt32(dtList.Rows[i]["DSTempSetColCalcId"]);

                                    if (calcType == "Frequency")
                                    {
                                        chkFrequency.Checked = true;
                                        txtFrequency.Text = calcDesc.ToString();
                                        if (IsMeasureAssigned(columnId, colCalcId) == true)
                                        {
                                            chkFrequency.Enabled = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }




    protected bool IsMeasureAssigned(int colId, int measureId)
    {
        string selQuerry = "SELECT DSTempRuleId FROM DSTempRule WHERE DSTempSetColId = " + colId + " AND DSTempSetColCalcId = " + measureId + "   AND ActiveInd = 'A'";
        DataTable dtNew = objData.ReturnDataTable(selQuerry, false);
        if (dtNew != null && dtNew.Rows.Count > 0)
        {
            ddlColumnType.Enabled = false;
            return true;
        }
        else
        {
            return false;
        }
    }


    protected void DivMeasureVisibility(string colType)
    {
        if (colType == "+/-")
        {
            PlusMinusDiv.Visible = true;
            promptDiv.Visible = false;
            TextDiv.Visible = false;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = false;
        }
        else if (colType == "Prompt")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = true;
            TextDiv.Visible = false;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = false;
        }

        else if (colType == "Text")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = false;
            TextDiv.Visible = true;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = false;
        }
        else if (colType == "Duration")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = false;
            TextDiv.Visible = false;
            DurationDiv.Visible = true;
            FrequencyDiv.Visible = false;
        }
        else if (colType == "Frequency")
        {
            PlusMinusDiv.Visible = false;
            promptDiv.Visible = false;
            TextDiv.Visible = false;
            DurationDiv.Visible = false;
            FrequencyDiv.Visible = true;
        }
    }



    protected void BtnAddPromptSelctd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPromptProcedure.SelectedIndex == 0)
            {
                //tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Prompt Procedure");
                //ddlPromptProcedure.Focus();

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSelectPrompt();", true);
                return;
            }
            else
            {
                if (lstCompletePrompts.SelectedIndex > -1)
                {
                    string _value = lstCompletePrompts.SelectedItem.Value; //Gets the value of  items in list.
                    string _text = lstCompletePrompts.SelectedItem.Text;  // Gets the Text of items in the list.  
                    ListItem item = new ListItem(); //create a list item
                    item.Text = _text;               //Assign the values to list item   
                    item.Value = _value;
                    lstSelectedPrompts.Items.Add(item); //Add the list item to the selected list of prompts   
                    lstCompletePrompts.Items.Remove(item); //Remove the details from AllPrompts list   
                    //  tdMsg.InnerHtml = "";
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void BtnAddAllPrompt_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPromptProcedure.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSelectPrompt();", true);
                // tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Prompt Procedure");
                // ddlPromptProcedure.Focus();
                return;
            }
            else
            {
                int _count = lstCompletePrompts.Items.Count;
                if (_count != 0)
                {
                    for (int i = 0; i < _count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = lstCompletePrompts.Items[i].Text;
                        item.Value = lstCompletePrompts.Items[i].Value;
                        //Add the item to selected Role list
                        lstSelectedPrompts.Items.Add(item);
                        // tdMsg.InnerHtml = "";
                    }

                }
            }
            lstCompletePrompts.Items.Clear();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void BtnRemvePrmptSelctd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPromptProcedure.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSelectPrompt();", true);
                // tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Prompt Procedure");
                //  ddlPromptProcedure.Focus();
                return;
            }
            else
            {

                if (lstSelectedPrompts.SelectedIndex > -1)
                {
                    string _value = lstSelectedPrompts.SelectedItem.Value; //Gets the value of items in list.
                    string _text = lstSelectedPrompts.SelectedItem.Text;  // Gets the Text of items in the list.  
                    ListItem item = new ListItem(); //create a list item
                    item.Text = _text;               //Assign the values to list item   
                    item.Value = _value;
                    lstSelectedPrompts.Items.Remove(item); //Remove from the selected list
                    lstCompletePrompts.Items.Add(item); //Add in the PromptAll list 
                    //  tdMsg.InnerHtml = "";

                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void BtnRemoveAllPrmpt_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPromptProcedure.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSelectPrompt();", true);
                // tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Prompt Procedure");
                // ddlPromptProcedure.Focus();
                return;
            }
            else
            {
                int _count = lstSelectedPrompts.Items.Count;
                if (_count != 0)
                {
                    for (int i = 0; i < _count; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = lstSelectedPrompts.Items[i].Text;
                        item.Value = lstSelectedPrompts.Items[i].Value;
                        lstCompletePrompts.Items.Add(item);
                        //   tdMsg.InnerHtml = "";
                    }
                }
            }
            lstSelectedPrompts.Items.Clear();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }


    protected void ddlPromptProcedure_SelectedIndexChanged(object sender, EventArgs e)
    {
        objData = new clsData();
        string selQuerry = "";
        string naValue = "";
        object objVal = null;
        try
        {
            selQuerry = "SELECT LookupId FROM LookUp WHERE LookupType = 'Datasheet-Prompt Procedures' AND LookupName = 'NA'";
            objVal = objData.FetchValue(selQuerry);
            if (objVal != null)
            {
                naValue = objVal.ToString();
            }

            if (ddlPromptProcedure.SelectedValue == naValue.ToString())
            {
                lstCompletePrompts.Visible = false;
                lstSelectedPrompts.Visible = false;
                BtnAddPromptSelctd.Visible = false;
                BtnAddAllPrompt.Visible = false;
                BtnRemvePrmptSelctd.Visible = false;
                BtnRemoveAllPrmpt.Visible = false;
                lblSelctPrompt.Visible = false;
            }
            else
            {
                lstCompletePrompts.Visible = true;
                lstSelectedPrompts.Visible = true;
                BtnAddPromptSelctd.Visible = true;
                BtnAddAllPrompt.Visible = true;
                BtnRemvePrmptSelctd.Visible = true;
                BtnRemoveAllPrmpt.Visible = true;
                lblSelctPrompt.Visible = true;
            }
        }
        catch
        {
        }



    }

    protected void BtnSavePrompt_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        int count = 0;
        string strQuerry = "";
        string naValue = "";
        int index = 0;
        object objVal = null;
        object objnew = null;
        object objIfexist = null;
        int ifexists = 0;
        sess = (clsSession)Session["UserSession"];
        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        try
        {
            try
            {
                strQuerry = "SELECT LookupId FROM LookUp WHERE LookupType = 'Datasheet-Prompt Procedures' AND LookupName = 'NA'";
                objVal = objData.FetchValue(strQuerry);
                if (objVal != null)
                {
                    naValue = objVal.ToString();
                }
            }
            catch
            {
            }

            if (ddlPromptProcedure.SelectedIndex == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSelectPrompt();", true);
                return;
            }

            else
            {
                int promptProcedure = Convert.ToInt32(ddlPromptProcedure.SelectedValue);
                try
                {

                    if (ddlPromptProcedure.SelectedValue == naValue.ToString())                            // If selected prompt procedure is NA, then the complete asigned promptes of that headerId will delete.
                    {
                        strQuerry = "SELECT COUNT(DSTempSetColId) FROM DSTempSetCol WHERE DSTempHdrId = " + headerId + " AND ColTypeCd = 'Prompt' AND ActiveInd = 'A'";
                        objnew = objData.FetchValue(strQuerry);
                        if (objnew != null)
                        {
                            count = Convert.ToInt32(objnew);
                        }

                        if (count == 0)
                        {
                            strQuerry = "UPDATE DSTempHdr SET PromptTypeId = " + promptProcedure + " WHERE DSTempHdrId =" + headerId;
                            index = objData.Execute(strQuerry);
                            string del = "DELETE FROM DSTempPrompt WHERE DSTempHdrId = " + headerId;
                            objData.Execute(del);
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSuccessMsg();", true);
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertPromptValid();", true);
                            return;
                        }
                    }
                }
                catch
                {
                }

                strQuerry = "UPDATE DSTempHdr SET PromptTypeId = " + promptProcedure + " WHERE DSTempHdrId =" + headerId;
                index = objData.Execute(strQuerry);

                for (int j = 0; j < lstCompletePrompts.Items.Count; j++)
                {
                    string promptId = lstCompletePrompts.Items[j].Value.ToString();
                    try
                    {
                        string del = "DELETE FROM DSTempPrompt WHERE DSTempHdrId = " + headerId + " AND PromptId = " + promptId;
                        objData.Execute(del);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                for (int i = 0; i < lstSelectedPrompts.Items.Count; i++)
                {
                    string selctPromptId = lstSelectedPrompts.Items[i].Value.ToString();

                    try
                    {
                        string sel = "SELECT DSTempPromptId FROM DSTempPrompt WHERE DSTempHdrId = " + headerId + " AND PromptId = " + selctPromptId;
                        objIfexist = objData.FetchValue(sel);
                        if (objIfexist != null)
                        {
                            ifexists = Convert.ToInt32(objIfexist);
                        }
                        if (ifexists <= 0)
                        {

                            string strQuery = "INSERT INTO DSTempPrompt(DSTempHdrId,PromptId,PromptOrder,ActiveInd,CreatedBy,CreatedOn) Values(" + headerId + "," + selctPromptId + "," + i + ",'A'," + sess.LoginId + ",GetDate())";
                            int promptId = objData.Execute(strQuery);

                            //    tdMsg.InnerHtml = clsGeneral.sucessMsg("Prompt Procedure Added Successfully");

                        }

                    }

                    catch (SqlException ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertFailedMsg();", true);
                        // tdMsg.InnerHtml = clsGeneral.failedMsg("Class Insertion For User Failed");
                    }


                }
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSuccessMsg();", true);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }



    protected void BtnAddSamples_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string textValue = "";
        textValue = clsGeneral.convertQuotes(txtMatcSamples.Text.Trim());
        if (textValue != "")
        {
            ListItem item = new ListItem();
            item.Text = textValue;
            item.Value = textValue;
            lstMatchSamples.Items.Add(item);
            txtMatcSamples.Text = "";
        }

    }


    protected void btnDeltSamples_Click(object sender, EventArgs e)
    {
        objData = new clsData();

        if (lstMatchSamples.SelectedIndex > -1)
        {
            string value = lstMatchSamples.SelectedItem.Value;
            string text = lstMatchSamples.SelectedItem.Text;
            ListItem item = new ListItem(); //create a list item
            item.Text = text;               //Assign the values to list item   
            item.Value = value;
            lstMatchSamples.Items.Remove(item); //Remove from the selected list
        }


    }



    protected void rbtnConsectiveSes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnConsectiveSes.SelectedValue == "1")
        {
            txtIns2.ReadOnly = true;
            txtIns1.ReadOnly = true;
            txtNumbrSessions.ReadOnly = false;
            txtIns2.Text = "";
            txtIns1.Text = "";
        }
        else if (rbtnConsectiveSes.SelectedValue == "0")
        {
            txtNumbrSessions.ReadOnly = true;
            txtIns2.ReadOnly = false;
            txtIns1.ReadOnly = false;
            txtNumbrSessions.Text = "";

        }

    }
    protected void btUpdateLessonProc_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        int headerId = 0;
        string updateQuerry = "";

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }

        try
        {
            updateQuerry = "UPDATE DSTempHdr SET LessonDefInst = '" + clsGeneral.convertQuotes(txtSDInstruction.Text.Trim()) + "',StudentReadCrita = '" + clsGeneral.convertQuotes(txtResponseOutcome.Text.Trim()) + "', " +
                            "ReinforcementProc = '" + clsGeneral.convertQuotes(txtReinforcementProc.Text.Trim()) + "',CorrectionProc = '" + clsGeneral.convertQuotes(txtCorrectionProcedure.Text.Trim()) + "' WHERE DSTempHdrId = " + headerId;

            int index = objData.Execute(updateQuerry);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertSuccessMsg();", true);
        }
        catch (Exception Ex)
        {
            string error = Ex.Message;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "AlertFailedMsg();", true);
            throw Ex;
        }


    }


    //private void GetStatus(int TemplateId)
    //{
    //    bool visibility = false;
    //    objData = new clsData();

    //    try
    //    {
    //        int tempStatusId = Convert.ToInt16(objData.FetchValue("SELECT  [StatusId] FROM DSTempHdr WHERE DSTempHdrId= " + TemplateId + " "));
    //        int AppStatusId = Convert.ToInt16(objData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Approved' "));
    //        int progresStatusId = Convert.ToInt16(objData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='In Progress' "));
    //        int PendStatusId = Convert.ToInt16(objData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Pending Approval' "));

    //        if (tempStatusId == AppStatusId)
    //        {
    //           // BtnCopyTemplate.Visible = true;
    //          //  BtnSubmit.Visible = false;
    //           // BtnApproval.Visible = false;
    //            visibility = false;
    //            ButtonVisibility(visibility);
    //            DatalistVisibility(visibility);
    //       //     CheckAsigned(TemplateId);              // Check Is any other lesson plan waiting for approval.


    //        }
    //        else if (tempStatusId == progresStatusId)
    //        {
    //         //   BtnCopyTemplate.Visible = false;
    //         //   BtnSubmit.Visible = true; ;
    //           // BtnApproval.Visible = false;
    //            visibility = true;
    //            ButtonVisibility(visibility);
    //            DatalistVisibility(visibility);
    //            try
    //            {
    //                FillDropPrompt(TemplateId);                              // Prompt Procedure NA Check. 
    //                IsDiscrete(TemplateId);                                  // Check the current Lesson plan is discrete or not                   

    //            }
    //            catch (Exception Ex)
    //            {
    //                throw Ex;
    //            }

    //        }
    //        else if (tempStatusId == PendStatusId)
    //        {
    //          //  BtnCopyTemplate.Visible = false;
    //           // BtnSubmit.Visible = false;
    //           // BtnApproval.Visible = true;
    //            visibility = false;
    //            ButtonVisibility(visibility);
    //            DatalistVisibility(visibility);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //}


    protected void DatalistVisibility(bool valid)
    {
        objData = new clsData();
        foreach (DataListItem item in dlMeasureData.Items)
        {
            Button btnEdit = (Button)item.FindControl("btnEditMeasure");
            Button btnDelt = (Button)item.FindControl("BtnRemove");
            if (valid == false)
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = false;
                }
            }
            else
            {
                btnDelt.Visible = true;
                btnEdit.Visible = true;
            }
        }

        foreach (DataListItem item in dlSetDetails.Items)
        {
            Button btnEdit = (Button)item.FindControl("btnEditSet");
            Button btnDelt = (Button)item.FindControl("btnRemoveSet");
            if (valid == false)
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = false;
                }
            }
            else
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = true;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = true;
                }
            }
        }

        foreach (DataListItem item in dlSetCriteria.Items)
        {
            Button btnEdit = (Button)item.FindControl("BtnEditSetCriteria");
            Button btnDelt = (Button)item.FindControl("BtnRemoveSetCriteria");
            if (valid == false)
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = false;
                }
            }
            else
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = true;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = true;
                }
            }
        }
        foreach (DataListItem item in dlStepDetails.Items)
        {
            Button btnEdit = (Button)item.FindControl("btnEditStep");
            Button btnDelt = (Button)item.FindControl("btnRemoveStep");
            if (valid == false)
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = false;
                }
            }
            else
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = true;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = true;
                }
            }
        }

        foreach (DataListItem item in dlStepCriteria.Items)
        {
            Button btnEdit = (Button)item.FindControl("BtnEditStepCriteria");
            Button btnDelt = (Button)item.FindControl("BtnRemoveStepCriteria");
            if (valid == false)
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = false;
                }
            }
            else
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = true;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = true;
                }
            }
        }

        foreach (DataListItem item in dlPromptCriteria.Items)
        {
            Button btnEdit = (Button)item.FindControl("BtnEditPromptCriteria");
            Button btnDelt = (Button)item.FindControl("BtnRemovePromptCriteria");
            if (valid == false)
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = false;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = false;
                }
            }
            else
            {
                if (btnEdit != null)
                {
                    btnEdit.Visible = true;
                }
                if (btnDelt != null)
                {
                    btnDelt.Visible = true;
                }
            }
        }


    }


    //protected void BtnCopyTemplate_Click(object sender, EventArgs e)             // Submit Template
    //{
    //    objData = new clsData();
    //    int apprvdLessonId = 0;
    //    int visualLessonId = 0;
    //    if (ViewState["HeaderId"] != null)
    //    {
    //        apprvdLessonId = Convert.ToInt32(ViewState["HeaderId"]);
    //    }
    //    tdReadMsg.InnerHtml = "";
    //    sess = (clsSession)Session["UserSession"];
    //    clsAssignLessonPlan AssignLP = new clsAssignLessonPlan();
    //    try
    //    {
    //        if (sess != null)
    //        {

    //            int tempid = AssignLP.CopyCustomtemplate(apprvdLessonId, sess.LoginId, visualLessonId);      // Set the approved template to expired and copied the new template
    //            ViewState["HeaderId"] = tempid;                    //Set new headerId to load
    //        //    FillData();                                      // Fill the goal type assigned for the student
    //            FillApprovedLessonData();

    //            IsDiscrete(tempid);                       // Check the template is discrete
    //            lblDataFill(tempid);                      // Fill Current LessonName
    //            GetMeasureData(tempid);                   // Fill the measure Data 
    //            FillTypeOfInstruction(tempid);                   // Fill Type of Instruction Data
    //            GetSetData(tempid);                      // Fill the Sets Data
    //            GetStepData(tempid);                    // Fill the Steps Data
    //            GetSetCriteriaData(tempid);             // Fill the Set Criteria Data
    //            GetStepCriteriaData(tempid);           // Fill the Step Criteria Data
    //            GetPromptCriteriaData(tempid);        // Fill the Prompt Criteria Data
    //            GetPromptProcedureList(tempid);      // Fill the prompt Procedure...
    //            FillDropPrompt(tempid);             // Fill dropdownlist prompt procedure...
    //            GetLessonProcData(tempid);           // Fill LessonProcedureData
    //            GetStatus(tempid);                    // Button Permissions


    //        }


    //    }
    //    catch (Exception Ex)
    //    {

    //    }

    //}

    //protected void BtnSubmit_Click(object sender, EventArgs e)             // Submit Template
    //{
    //    string hdrid = "";
    //    int TemplateId = 0;
    //    bool validSet = false;
    //    bool validStep = false;
    //    int setCriteria = 0;
    //    int stepCriteria = 0;
    //    string message = "";
    //    tdReadMsg.InnerHtml = "";
    //    DataTable dt = new DataTable();
    //    sess = (clsSession)Session["UserSession"];

    //    if (ViewState["HeaderId"] != null)
    //    {
    //        TemplateId = Convert.ToInt32(ViewState["HeaderId"]);
    //    }
    //    objData = new clsData();

    //    // FUnction to asign sets and steps If the lesson plan is a visual lesson 

    //    // valid = FunInsrtSetStepVT();

    //    try
    //    {

    //        string skilltype = Convert.ToString(objData.FetchValue("SELECT SkillType FROM [dbo].[DSTempHdr] WHERE StudentId='" + sess.StudentId + "' and DSTempHdrId='" + TemplateId + "'"));
    //        if (skilltype == "Chained")
    //        {
    //            hdrid = Convert.ToString(objData.FetchValue("SELECT DSTempHdrId FROM [dbo].[DSTempSet] WHERE ActiveInd = 'A' INTERSECT SELECT DSTempHdrId FROM [dbo].[DSTempStep] WHERE ActiveInd = 'A' INTERSECT SELECT DSTempHdrId FROM [dbo].[DSTempSetCol] WHERE DSTempHdrId='" + TemplateId + "'"));

    //            dt = objData.ReturnDataTable("SELECT COUNT(DSTempHdrId),(SELECT COUNT(RuleType) FROM [dbo].[DSTempRule] WHERE RuleType='STEP' AND ActiveInd = 'A' AND DSTempSetColId IN (SELECT DSTempSetColId " +
    //"FROM DSTempSetCol WHERE DSTempHdrId=" + hdrid + " AND ActiveInd='A')) AS STEP,(SELECT COUNT(RuleType) FROM [dbo].[DSTempRule] WHERE RuleType='SET' AND ActiveInd = 'A' AND DSTempSetColId IN (SELECT  DSTempSetColId " +
    //"FROM DSTempSetCol WHERE DSTempHdrId=" + hdrid + " AND ActiveInd='A')) AS SET1 FROM [dbo].[DSTempSetCol] col INNER JOIN [dbo].[DSTempRule] rul on " +

    //"col.DSTempSetColId=rul.DSTempSetColId WHERE col.DSTempHdrId=" + hdrid + "", false);
    //        }
    //        else if (skilltype == "Discrete")
    //        {
    //            hdrid = Convert.ToString(objData.FetchValue("SELECT DSTempHdrId FROM [dbo].[DSTempSet]  WHERE ActiveInd = 'A' INTERSECT SELECT DSTempHdrId FROM [dbo].[DSTempSetCol] WHERE DSTempHdrId='" + TemplateId + "'"));

    //            dt = objData.ReturnDataTable("SELECT COUNT(DSTempHdrId),(SELECT COUNT(RuleType) FROM [dbo].[DSTempRule] WHERE RuleType='SET' AND ActiveInd = 'A' AND  DSTempSetColId IN (SELECT DSTempSetColId " +
    //"FROM DSTempSetCol WHERE DSTempHdrId=" + hdrid + " AND ActiveInd='A')) AS SET1 FROM [dbo].[DSTempSetCol] col INNER JOIN [dbo].[DSTempRule] rul on " +

    //"col.DSTempSetColId=rul.DSTempSetColId WHERE col.DSTempHdrId=" + hdrid + "", false);
    //        }
    //        else
    //        {
    //            tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting. Skill type is missing");
    //            return;
    //        }
    //        if (hdrid != "")
    //        {
    //            if (dt != null)
    //            {

    //                int colcnt = dt.Columns.Count;  // colcnt=2 for Discrete and colcnt=3 for chained
    //                if (colcnt > 0)
    //                {
    //                    if (colcnt == 3)
    //                    {
    //                        if (Convert.ToInt32(dt.Rows[0]["SET1"]) > 0 && Convert.ToInt32(dt.Rows[0]["STEP"]) > 0)
    //                        {
    //                            PENDINGAPPROVE(TemplateId);
    //                            GetStatus(TemplateId);
    //                          //  FillData();                                      // Fill the goal type assigned for the student
    //                            FillApprovedLessonData();
    //                            FillCompltdLessonPlans();
    //                            setApprovePermission();
    //                        }
    //                        else
    //                        {
    //                            setCriteria = Convert.ToInt32(dt.Rows[0]["SET1"]);
    //                            stepCriteria = Convert.ToInt32(dt.Rows[0]["STEP"]);
    //                            if (setCriteria == 0)
    //                            {
    //                                message = "Set criteria details are missing";
    //                                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting.." + message + "");
    //                            }
    //                            else if (stepCriteria == 0)
    //                            {
    //                                message = "Step criteria details are missing";
    //                                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting. " + message + "");
    //                            }
    //                            else
    //                            {
    //                                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting");
    //                            }
    //                        }
    //                    }
    //                    else if (colcnt == 2)
    //                    {
    //                        if (Convert.ToInt32(dt.Rows[0]["SET1"]) > 0)
    //                        {
    //                            PENDINGAPPROVE(TemplateId);
    //                            GetStatus(TemplateId);
    //                       //     FillData();                                      // Fill the goal type assigned for the student
    //                            FillApprovedLessonData();
    //                        //    FillCompltdLessonPlans();
    //                            setApprovePermission();
    //                        }
    //                        else
    //                        {
    //                            setCriteria = Convert.ToInt32(dt.Rows[0]["SET1"]);
    //                            if (setCriteria == 0)
    //                            {
    //                                message = "Set criteria details are missing";
    //                                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting.." + message + "");
    //                            }
    //                            else
    //                            {
    //                                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting..Undefined error.");
    //                            }

    //                        }
    //                    }
    //                    else
    //                    {
    //                        tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting..Undefined error.");

    //                    }
    //                }
    //                else
    //                {
    //                    tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting..Undefined error.");

    //                }

    //            }
    //            else
    //            {
    //                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting..Undefined error.");

    //            }
    //        }
    //        else
    //        {
    //            validSet = SetValidation(TemplateId);
    //            validStep = StepValidation(TemplateId);
    //            if (validSet == false)
    //            {
    //                message = "Sets are missing.";
    //                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting..." + message + "");
    //            }
    //            else if (validStep == false)
    //            {
    //                message = "Steps are missing.";
    //                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting.." + message + "");
    //            }
    //            else
    //            {
    //                tdReadMsg.InnerHtml = clsGeneral.warningMsg("Please complete template details before submitting");
    //            }

    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }

    //}

    private bool SetValidation(int templateId)
    {
        int countSet = 0;
        string selQuerry = "";
        object objVal = null;

        selQuerry = "SELECT COUNT(DSTempSetId) FROM DSTempSet WHERE DSTempHdrId = " + templateId + " AND ActiveInd = 'A'";
        objVal = objData.FetchValue(selQuerry);
        if (objVal != null)
        {
            countSet = Convert.ToInt32(objVal);
        }
        if (countSet > 0)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    private bool StepValidation(int templateId)
    {
        int countStep = 0;
        object objVal = null;
        string selQuerry = "";
        selQuerry = "SELECT COUNT(DSTempStepId) FROM DSTempStep WHERE DSTempHdrId = " + templateId + " AND ActiveInd = 'A'";
        objVal = objData.FetchValue(selQuerry);
        if (objVal != null)
        {
            countStep = Convert.ToInt32(objVal);
        }
        if (countStep > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //private void PENDINGAPPROVE(int templateId)
    //{
    //    try
    //    {
    //        int StatusId = Convert.ToInt16(objData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Pending Approval' "));
    //        objData.Execute("Update DSTempHdr set [StatusId]=" + StatusId + " WHERE DSTempHdrId= " + templateId + "");
    //        //  BtnSubmit.Visible = false;
    //        // BtnApproval.Visible = true;
    //        //  imgApprove.Visible = true;
    //        //  imgpending.Visible = false;
    //        //  idLessonType.Visible = false;
    //        tdReadMsg.InnerHtml = clsGeneral.sucessMsg("Template Editor Successfully Submitted...");
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //}



    //protected void BtnApproval_Click(object sender, EventArgs e)         //Approval
    //{
    //    sess = (clsSession)Session["UserSession"];
    //    int TemplateId = 0;
    //    object objLessId = null;
    //    int LessonPlanId = 0;
    //    tdReadMsg.InnerHtml = "";
    //    if (ViewState["HeaderId"] != null)
    //    {
    //        TemplateId = Convert.ToInt32(ViewState["HeaderId"]);
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
    //        return;
    //    }
    //    try
    //    {
    //        objData = new clsData();
    //        string selQuerry = "SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId = " + TemplateId;
    //        objLessId = objData.FetchValue(selQuerry);
    //        if (objLessId != null)
    //        {
    //            LessonPlanId = Convert.ToInt32(objLessId);
    //        }

    //        if (sess != null)
    //        {
    //            try
    //            {
    //                int StatusId = Convert.ToInt16(objData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Approved' "));
    //                if (objData.IFExists("SELECT DSTempHdrId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + " and StatusId=" + StatusId + " and LessonPlanId=" + LessonPlanId + ""))
    //                {
    //                    objData.Execute("Update DSTempHdr set [StatusId]=(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Expired')  WHERE StudentId= " + sess.StudentId + " and StatusId=" + StatusId + " AND LessonPlanId = " + LessonPlanId + "");
    //                }

    //                objData.Execute("Update DSTempHdr set [StatusId]=" + StatusId + " WHERE DSTempHdrId= " + TemplateId + "");
    //             //   BtnSubmit.Visible = false;
    //               // BtnApproval.Visible = false;
    //                GetStatus(TemplateId);
    //             //   FillData();                                      // Fill the goal type assigned for the student
    //                FillApprovedLessonData();
    //              //  FillCompltdLessonPlans();

    //                tdReadMsg.InnerHtml = clsGeneral.sucessMsg("Template Editor Successfully Approved...");

    //            }
    //            catch (Exception Ex)
    //            {
    //                throw Ex;
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //}

    protected void imgCreateEqutn_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string strQuerry = "";
        int headerId = 0;
        string columnName = "";

        if (ViewState["HeaderId"] != null)
        {
            headerId = Convert.ToInt32(ViewState["HeaderId"]);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoHeaderID();", true);
            return;
        }
        string currentColumnName = txtColumnName.Text.ToString();
        if (currentColumnName == "")
        {
            tdMsgMeasure.InnerHtml = clsGeneral.warningMsg("Please enter Current Column Name...");
            return;
        }
        divBtn.Visible = true;
        strQuerry = "SELECT DSTempSetColId,ColName FROM DSTempSetCol WHERE DSTempHdrId = " + headerId + " AND ColTypeCd = 'Text' AND ActiveInd = 'A'";
        DataTable dtList = objData.ReturnDataTable(strQuerry, false);

        divBtn.InnerHtml = " <br />  " +
                           "  <input type='button' id='NFButton' class='lbtn' onclick='getValueBtn(this.value)' style='width: 95px' value='Sum(' alt='Sum(' />  " +
                           " <input type='button' id='Button1' class='lbtn' onclick='getValueBtn(this.value)' style='width: 95px' value='Average(' alt='Average(' />" +
                           "<input type='button' id='imgClear' class='smllbtn' onclick='cAsgn()' style='height: 23px' value='C' alt='C' />" +
                             "   <input type='button' id='Button17' class='smllbtn' onclick='backspace()' style='width: 32px' value='BS' alt='BS' />" +
                           " <input type='button' id='Button2' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='+' alt='+' />" +
                           " <input type='button' id='Button3' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='-' alt='-' />" +
                            "  <input type='button' id='Button4' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='*' alt='*' />" +
                               " <input type='button' id='Button5' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='/' alt='/' />" +
                              " <input type='button' id='Button6' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='%' alt='%' />" +
                              " <input type='button' id='Button7' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='(' alt='(' />" +
                              " <input type='button' id='Button8' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value=')' alt=')' />" +
                               "  <br />" +
                                " <input type='button' id='Button9' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='0' alt='0' />" +
                               " <input type='button' id='Button10' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='1' alt='1' />" +
                              "<input type='button' id='Button11' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='2' alt='2' />" +
                                 " <input type='button' id='Button12' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='3' alt='3' />" +
                               " <input type='button' id='Button13' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='4' alt='4' />" +
                                " <input type='button' id='Button14' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='5' alt='5' />" +
                                 " <input type='button' id='Button15' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='6' alt='6' />" +
                                " <input type='button' id='Button16' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='7' alt='7' />" +
                                "   <input type='button' id='Button17' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='8' alt='8' />" +
                             " <input type='button' id='Button18' class='smllbtn' onclick='getValueBtn(this.value)' style='width: 32px' value='9' alt='9' />" +
                             " <input type='button' id='BtnCurrntClm' class='lbtn' onclick='getValueBtn(this.value)' style='width: 100px' value='" + currentColumnName + "' alt='" + currentColumnName + "' />";

        if (dtList != null)
        {
            if (dtList.Rows.Count > 0)
            {
                for (int i = 0; i < dtList.Rows.Count; i++)
                {
                    columnName = dtList.Rows[i]["ColName"].ToString();
                    if (columnName.Length > 15)
                    {
                        columnName = columnName.ToString().Substring(0, 15) + "...";
                    }
                    divBtn.InnerHtml += " <input type='button' id='Button" + columnName + "' class='lbtn' onclick='getValueBtn(this.value)' style='width: 100px' value='" + columnName + "' alt='" + columnName + "' />";


                }
            }
        }

        //" <%--<input type='button' id='Button19' style='width: 162px' value='OK' alt='OK' />--%>";







        //divBtn.InnerHtml += "<input type='button' class='NFBUTTON' id='btnEdit' value = 'New' onclick='getValueBtn(this.value)'>";

        // divBtn.InnerHtml = 

        //     divBtn.InnerHtml += "<input type='button' class='NFBUTTON' id='btnEdit' onclick='getValueBtn(Edit Member);'>";

        //Button btnTest = new Button();
        //btnTest.ID = "btnEdit";
        //btnTest.Text = "Edit Member";
        //btnTest.CssClass = "NFBUTTON";

        //btnTest.OnClientClick = "return getValueBtn(" + btnTest.Text + ")";
        ////   btnTest.Click += new EventHandler(btnEdit_Click);
        //divBtn.Controls.Add(btnTest);
    }



    protected void imageCollapseDiv_Click(object sender, EventArgs e)
    {
        divBtn.Visible = false;
    }
}