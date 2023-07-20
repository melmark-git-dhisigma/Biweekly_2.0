using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class LessonPlanDetailsAdd : System.Web.UI.Page
{
    clsData objData = null;
    DataClass listClass = new DataClass();
    DataTable dt = null;
    DataTable dt2 = null;
    clsTemplate objTemp = null;
    SqlConnection con = null;
    SqlTransaction trans = null;
    string strQuery = string.Empty;
    clsSession sess = null;
    ClsTemplateSession ObjTempSess = null;
    string lessionid = string.Empty;
    string studentid = string.Empty;
    bool retVal = false;
    int PromptId = 0;
    int PromptOrder = 0;
    bool Disable = false;
    int temphdid = 0;


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
            //bool flag = clsGeneral.PageIdentification(sess.perPage);
            //if (flag == false)
            //{
            //    Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            //}
        }
        if (!IsPostBack)
        {
            //string studentId;
            fillinsertdata();
            try
            {
                studentid = sess.StudentId.ToString();
            }
            catch(Exception Ex)
            {
                studentid = string.Empty;
                throw Ex;
            }

            //------------- checking lesson plan id -------------
            //try
            //{
            //    lessionid = ObjTempSess.LessonPlanId.ToString();
            //}
            //catch(Exception Ex)
            //{
            //    lessionid = string.Empty;
            //    throw Ex;
            //}


            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                butSave.Visible = false;
            }
            else
            {
                butSave.Visible = true;
            }

            if (Session["LessonData"] != null)
            {
                ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
                if (ObjTempSess == null)
                {
                    ObjTempSess = new ClsTemplateSession();
                }
                ObjTempSess.TemplateId = temphdid;

                fillLessonTemphd(Session["LessonData"].ToString(), temphdid.ToString());
                fillmeasurement(temphdid.ToString());
                fillcriteria(temphdid.ToString());
                fillAtepAndSet(temphdid.ToString());
            }

            if (lessionid != "")
            {
                butSave.Visible = false;
                 temphdid = clsGeneral.GetTemplateHeaderId(Convert.ToInt32(lessionid), sess.StudentId.ToString(), Convert.ToInt32(sess.SchoolId.ToString()));

                if (temphdid != 0)
                {
                    if (Disable == false)
                    {
                        butSave.Visible = true;
                        butSave.Text = "Edit";
                    }
                    else
                    {
                        butSave.Visible = false;
                    }
                }

               
            }
            else
            {
                objective.Visible = false;
            }

            if (sess.AdminView != 1)
            {
                txtLessonName.ReadOnly = true;
                txtLessonName.BorderWidth = 0;
                objData = new clsData();
                dt = new DataTable();

                stdLevel.Visible = true;
                disableFiledEdit();
                strQuery = "select StudentLname,StudentFname from  dbo.Student where StudentId =" + sess.StudentId + "";
                dt = objData.ReturnDataTable(strQuery, false);
                if (dt.Rows.Count > 0)
                {
                    labStudentname.Text = dt.Rows[0]["StudentLname"].ToString() + ", " + dt.Rows[0]["StudentFname"].ToString();
                }
                chkgoal.Enabled = false;

                txtspecEntrypoint.ReadOnly = true;
                txtPrerequistskill.ReadOnly = true;
                txtMaterials.ReadOnly = true;

                chkgoal.Visible = false;
                fillsudentdetailpart(lessionid.ToString(), sess.StudentId.ToString());
                fillObjective(sess.StudentId.ToString(), sess.SchoolId.ToString(), lessionid.ToString());
            }
            else if (lessionid != "")
            {
                txtLessonName.ReadOnly = false;
                txtLessonName.BorderWidth = 1;
                if (Disable == true)
                {
                    butSave.Visible = false;
                }
                else
                {
                    butSave.Visible = true;
                    butSave.Text = "Update";
                }
                objective.Visible = false;
            }
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "c", " changediptrial($('#disc'));");

    }

    private void fillinsertdata()
    {
        try
        {
            stdLevel.Visible = false;
            objData = new clsData();
            dt = new DataTable();
            dt = objData.ReturnDataTable("select Goal.GoalId,Goal.GoalName from Goal inner join GoalType on Goal.GoalTypeId =GoalType.GoalTypeId where GoalType.GoalTypeName = 'Academic Goals' order by Goal.GoalName", true);
            chkgoal.DataSource = dt;
            chkgoal.DataTextField = "GoalName";
            chkgoal.DataValueField = "GoalId";
            chkgoal.DataBind();
            objData.ReturnDropDown("Select LookupId as Id , LookupCode as Name from dbo.LookUp where LookupType='Datasheet-Teaching Procedures'", drpTeachingProc);
            objData.ReturnDropDown("Select LookupId as Id , LookupName as Name from dbo.LookUp where LookupType='Datasheet-Prompt Procedures'", drpPromptproc);
            objData.ReturnCheckBoxList("Select CONVERT(varchar, LookupId)+','+CONVERT(varchar, SortOrder) as Id , LookupName as Name from dbo.LookUp where LookupType='DSTempPrompt' order by SortOrder Asc ", chkpromptused);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    private void fillsudentdetailpart(string lesid, string stdid)
    {
        strQuery = "select Goal.GoalName from StdtLessonPlan inner join Goal on Goal.GoalId=StdtLessonPlan.GoalId where LessonPlanId = " + lesid + " and StudentId = " + stdid + "";
        dt = new DataTable();
        objData = new clsData();
        dt = objData.ReturnDataTable(strQuery, false);
        if (dt.Rows.Count > 0)
        {
            labgoal.Text = dt.Rows[0]["GoalName"].ToString();
        }
        strQuery = "select StdtIEP.EffStartDate,StdtIEP.EffEndDate from StdtLessonPlan inner join StdtIEP on StdtLessonPlan.StdtIEPId=StdtIEP.StdtIEPId where StdtLessonPlan.LessonPlanId = " + lesid + " and StdtLessonPlan.StudentId = " + stdid + "";
        dt = new DataTable();
        objData = new clsData();
        dt = objData.ReturnDataTable(strQuery, false);
        if (dt.Rows.Count > 0)
        {
            Lbl_IEPDates.Text = dt.Rows[0]["EffStartDate"].ToString().Replace(" 12:00:00 AM", "") + " - " + dt.Rows[0]["EffEndDate"].ToString().Replace(" 12:00:00 AM", "");
        }

    }

    private void fillAtepAndSet(string tempHdId)
    {
        dt = new DataTable();
        objData = new clsData();

        //-------- takeing set desc on template header ID -------------

        strQuery = "select SetName,SetCd from dbo.DSTempSet where DSTempHdrId = " + tempHdId + "";
        dt = objData.ReturnDataTable(strQuery, false);
        if (dt.Rows.Count > 0)
        {
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                Setdes.InnerHtml += dt.Rows[count]["SetCd"].ToString() + "-" + dt.Rows[count]["SetName"].ToString() + "<br/>";
            }
        }

        //--------- taking step desc on template header ID -------------
        strQuery = "select StepName,StepCd from dbo.DSTempStep where DSTempHdrId = " + tempHdId + "";
        dt = objData.ReturnDataTable(strQuery, false);
        if (dt.Rows.Count > 0)
        {
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                Stepdes.InnerHtml += dt.Rows[count]["StepCd"].ToString() + "-" + dt.Rows[count]["StepName"].ToString() + "<br/>";
            }
        }

    }

    private int insert()
    {
        int ierror = 0;
        int returnLesionId = 0;        
        sess = (clsSession)Session["UserSession"];


        try
        {
            objData = new clsData();
            con = new SqlConnection();
            con = objData.Open();
            trans = con.BeginTransaction();
            strQuery = "insert into LessonPlan(SchoolId,ActiveInd,LessonPlanName,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials,CreatedBy,ModifiedBy,CreatedOn) values(1,'A','" + clsGeneral.convertQuotes(txtLessonName.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtFramandStrand.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSpecStd.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtspecEntrypoint.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtPrerequistskill.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtMaterials.Text.Trim()) + "'," + sess.LoginId + "," + sess.LoginId + " ,(SELECT Convert(Varchar,getdate(),100))) ";
            returnLesionId = objData.ExecuteWithScopeandConnection(strQuery, con, trans);
            if (returnLesionId > 0)
            {
                foreach (ListItem li in chkgoal.Items)
                {

                    if (li.Selected == true)
                    {
                        int goalId = Convert.ToInt32(li.Value);
                        strQuery = "INSERT into GoalLPRel(GoalId,LessonPlanId,ActiveInd,CreatedBy,CreatedOn) Values(" + goalId + "," + returnLesionId + ",'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                        int iIndex = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, trans));
                    }
                }

            }

            //retAddTemp = addEditTemp(returnLesionId, con, trans);
            //try
            //{

            //    foreach (ListItem lip in chkpromptused.Items)
            //    {
            //        if (lip.Selected == true)
            //        {

            //            string[] valOder = lip.Value.Split(',');
            //            strQuery = "Insert into DSTempPrompt(DSTempHdrId,PromptId,promptorder, ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) VALUES" +
            //                " (" + retAddTemp + "," + valOder[0] + "," + valOder[1] + ",'A'," + sess.LoginId + ",GETDATE()," + sess.LoginId + ",GETDATE())";
            //            int idofPromt = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, trans));
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    throw Ex;
            //}


            //ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            //if (ObjTempSess == null)
            //{
            //    ObjTempSess = new ClsTemplateSession();
            //}
            //ObjTempSess.TemplateId = retAddTemp;
            //ObjTempSess.LessonPlanId = returnLesionId;
            //Session["BiweeklySession"] = ObjTempSess;

            objData.CommitTransation(trans,con);
        }
        catch(Exception Ex)
        {
            objData.RollBackTransation(trans,con);
            con.Close();
            throw Ex;
        }
        return ierror;
    }

    private int addEditTemp(int lessId, SqlConnection conN, SqlTransaction transA)
    {
        string skilltype = string.Empty;
        int noofTrial = 0;
        string StrNoTril;
        string taskAnalysis = string.Empty;
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
        catch(Exception Ex)
        {
            noofTrial = 0;
            StrNoTril = string.Empty;
            throw Ex;
        }

        if (drpTasklist.SelectedValue != "0" && rdolistDatasheet.SelectedValue == "1")
        {
            taskAnalysis = drpTasklist.SelectedItem.Text;
        }

        strQuery = "insert into DSTempHdr(DSTemplateName,SchoolId,LessonPlanId,TeachingProcId,SkillType,NbrOfTrials,ChainType,PromptTypeId," +
            "BaselineProc,CorrRespDef,IncorrRespDef,LessonDefInst,StudentReadCrita,TeacherRespReadness,ReinforcementProc,CorrectionProc," +
            "MajorSetting,MinorSetting,CreatedBy,CreatedOn,StatusId,StudIncorrRespDef,StudCorrRespDef) values ('" + clsGeneral.convertQuotes(txtLessonName.Text.Trim()) + "',1," + lessId + "," + drpTeachingProc.SelectedValue + "," +
            "'" + skilltype + "'," + noofTrial + ",'" + taskAnalysis + "'," + drpPromptproc.SelectedValue + ",'" + clsGeneral.convertQuotes(txtBaselineProc.Text.Trim()) + "'," +
            "'" + clsGeneral.convertQuotes(txtCorretResp.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtIncorrResp.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtLessondelivery.Text.Trim()) + "'," +
            "'" + clsGeneral.convertQuotes(txtStdread.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtTeacherResp.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtReinfoProce.Text.Trim()) + "'," +
            "'" + clsGeneral.convertQuotes(txtCorretProce.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtmajorset.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtminorset.Text.Trim()) + "'," + sess.LoginId + "," +
            "(SELECT Convert(Varchar,getdate(),100)),(select LookupId from LookUp where LookupName = 'In Progress' and LookupType = 'TemplateStatus'),'" + clsGeneral.convertQuotes(txtincorrtlessResp.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCorrtlessResp.Text.Trim()) + "')";
        int iIndex = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, conN, transA));
        return iIndex;
    }



    private bool Update(string templatId)
    {
        string skilltype = string.Empty;
        int noofTrial = 0;
        string StrNoTril;
        string taskAnalysis = string.Empty;
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
        catch(Exception Ex)
        {
            noofTrial = 0;
            StrNoTril = "''";
            throw Ex;
        }
        if (drpTasklist.SelectedValue != "0" && rdolistDatasheet.SelectedValue == "1")
        {
            taskAnalysis = drpTasklist.SelectedItem.Text;
        }
        try
        {
            objData = new clsData();
            strQuery = "update DSTempHdr set TeachingProcId=" + drpTeachingProc.SelectedValue + ",SkillType='" + skilltype + "',NbrOfTrials=" + StrNoTril + ",ChainType='" + taskAnalysis + "'," +
                "PromptTypeId=" + drpPromptproc.SelectedValue + ",BaselineProc='" + clsGeneral.convertQuotes(txtBaselineProc.Text.Trim()) + "',CorrRespDef='" + clsGeneral.convertQuotes(txtCorretResp.Text.Trim()) + "'," +
                "IncorrRespDef='" + clsGeneral.convertQuotes(txtIncorrResp.Text.Trim()) + "',LessonDefInst='" + clsGeneral.convertQuotes(txtLessondelivery.Text.Trim()) + "',StudentReadCrita='" + clsGeneral.convertQuotes(txtStdread.Text.Trim()) + "'," +
                "TeacherRespReadness='" + clsGeneral.convertQuotes(txtTeacherResp.Text.Trim()) + "',ReinforcementProc='" + clsGeneral.convertQuotes(txtReinfoProce.Text.Trim()) + "',CorrectionProc='" + clsGeneral.convertQuotes(txtCorretProce.Text.Trim()) + "'," +
                "MajorSetting='" + clsGeneral.convertQuotes(txtmajorset.Text.Trim()) + "',MinorSetting='" + clsGeneral.convertQuotes(txtminorset.Text.Trim()) + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn=GETDATE(),StudCorrRespDef='" + clsGeneral.convertQuotes(txtCorrtlessResp.Text.Trim()) + "',StudIncorrRespDef='" + clsGeneral.convertQuotes(txtincorrtlessResp.Text.Trim()) + "' where DSTempHdrId =" + templatId + "";
            retVal = Convert.ToBoolean(objData.Execute(strQuery));
            PromptUpdate(Convert.ToInt32(templatId));
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return retVal;
    }

    private void PromptUpdate(int TemplateId)
    {
        ViewState["ColData"] = null;
        objTemp = new clsTemplate();

        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            if (TemplateId > 0)
            {
                foreach (ListItem li in chkpromptused.Items)
                {
                    if (li.Selected == true)
                    {
                        // isCol = 1;
                        //   ViewState["ColData"] = isCol;
                        clsTemplate.PromptOrderSplit(li.Value, out PromptId, out  PromptOrder);
                        if (objTemp.PromptExit(TemplateId, Convert.ToInt32(PromptId)) == false)
                        {
                            strQuery = "Insert into DSTempPrompt(DSTempHdrId,PromptId,PromptOrder,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) VALUES (" + TemplateId + "," + PromptId + "," + PromptOrder + ",'A'," + sess.LoginId + ",GETDATE()," + sess.LoginId + ",GETDATE())";
                            retVal = Convert.ToBoolean(objData.Execute(strQuery));
                        }
                        else
                        {
                            if (objTemp.GetADPrompt(TemplateId, Convert.ToInt32(PromptId)) == "D")
                            {
                                strQuery = "Update DSTempPrompt set ActiveInd='A' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where DSTempHdrId=" + TemplateId + " and PromptId=" + Convert.ToInt32(PromptId) + "";
                                objData.Execute(strQuery);
                            }
                        }

                    }

                    else
                    {
                        clsTemplate.PromptOrderSplit(li.Value, out PromptId, out  PromptOrder);
                        if (objTemp.PromptExit(TemplateId, Convert.ToInt32(PromptId)) == true)
                        {

                            if (objTemp.GetADPrompt(TemplateId, Convert.ToInt32(PromptId)) == "A")
                            {
                                strQuery = "Update DSTempPrompt set ActiveInd='D' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where DSTempHdrId=" + TemplateId + " and PromptId=" + Convert.ToInt32(PromptId) + "";
                                objData.Execute(strQuery);
                            }
                        }
                    }
                }

                // ColumnSaveForVT();
            }
        }
    }

    //protected void ColumnSaveForVT()
    //{
    //    objData = new clsData();
    //    string insertCode = "";
    //    int promptLookupId = 0;
    //    string selVT = "SELECT IsVisualTool FROM DSTempHdr WHERE DsTempHdrId = " + ObjTempSess.TemplateId;
    //    int isVt = Convert.ToInt32(objData.FetchValue(selVT));

    //    if (isVt != 0)
    //    {
    //        string selctPrompt = "SELECT LookupId FROM LookUp WHERE LookupType = 'DsTempPrompt' AND LookupName = 'NA'";
    //        promptLookupId = Convert.ToInt32(objData.FetchValue(selctPrompt));
    //        string selctData = "SELECT * FROM DSTempPrompt WHERE DSTempHdrId = " + ObjTempSess.TemplateId + " AND PromptId = " + promptLookupId;

    //        DataTable dtList = objData.ReturnDataTable(selctData, false);
    //        if (dtList.Rows.Count > 0)
    //        {
    //            string selctQuerry = "SELECT * FROM DSTempPrompt WHERE DSTempHdrId = " + ObjTempSess.TemplateId + " AND PromptId != " + promptLookupId;
    //            DataTable dtData = objData.ReturnDataTable(selctQuerry, false);
    //            if (dtData.Rows.Count > 0)
    //            {
    //                string deltQuerry = "DELETE FROM DSTempSetCol WHERE DSTempHdrId = " + ObjTempSess.TemplateId + " AND ColTypeCd = 'Prompt'";
    //                int iDelete = objData.Execute(deltQuerry);
    //                insertCode = "INSERT into DSTempSetCol(SchoolId,DSTempHdrId,ColName,ColTypeCd,IncMisTrialInd,ActiveInd,CreatedBy,CreatedOn) VALUES(" + sess.SchoolId + "," + ObjTempSess.TemplateId + ",'Column3','Prompt',0,'A'," + sess.LoginId + ",GETDATE())";
    //                int index1 = objData.Execute(insertCode);
    //            }
    //        }
    //        else
    //        {
    //            if (ViewState["ColData"] != null)
    //            {
    //                string deltQuerry = "DELETE FROM DSTempSetCol WHERE DSTempHdrId = " + ObjTempSess.TemplateId + " AND ColTypeCd = 'Prompt'";
    //                int iDelete = objData.Execute(deltQuerry);
    //                insertCode = "INSERT into DSTempSetCol(SchoolId,DSTempHdrId,ColName,ColTypeCd,IncMisTrialInd,ActiveInd,CreatedBy,CreatedOn) VALUES(" + sess.SchoolId + "," + ObjTempSess.TemplateId + ",'Column3','Prompt',0,'A'," + sess.LoginId + ",GETDATE())";
    //                int index = objData.Execute(insertCode);
    //            }
    //        }
    //    }
    //}

    private bool updatelesson(string lessionid)
    {
        strQuery = "Update LessonPlan set LessonPlanName='" + clsGeneral.convertQuotes(txtLessonName.Text.Trim()) + "',FrameandStrand='" + clsGeneral.convertQuotes(txtFramandStrand.Text.Trim()) + "',SpecStandard='" + clsGeneral.convertQuotes(txtSpecStd.Text.Trim()) + "',SpecEntryPoint='" + clsGeneral.convertQuotes(txtspecEntrypoint.Text.Trim()) + "',PreReq='" + clsGeneral.convertQuotes(txtPrerequistskill.Text.Trim()) + "',Materials='" + clsGeneral.convertQuotes(txtMaterials.Text.Trim()) + "',ModifiedBy=" + sess.LoginId.ToString() + ",ModifiedOn=GETDATE() where lessonPlanId = " + lessionid + " and ActiveInd = 'A'";
        objData = new clsData();
        objData.Execute(strQuery);
        strQuery = "delete from GoalLPRel where LessonPlanId = " + lessionid + "";
        objData.Execute(strQuery);
        foreach (ListItem li in chkgoal.Items)
        {
            if (li.Selected == true)
            {
                int goalId = Convert.ToInt32(li.Value);
                strQuery = "INSERT into GoalLPRel(GoalId,LessonPlanId,ActiveInd,CreatedBy,CreatedOn) Values(" + goalId + "," + lessionid + ",'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                retVal = Convert.ToBoolean(objData.Execute(strQuery));
            }
        }
        return retVal;
    }

    private void fillLessonTemphd(string lessonid, string tempid)
    {
        strQuery = "select LessonPlanName,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials from LessonPlan where LessonPlanId=" + lessonid + " and ActiveInd='A'";
        dt = new DataTable();
        objData = new clsData();
        dt = objData.ReturnDataTable(strQuery, true);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                txtLessonName.Text = dt.Rows[0]["LessonPlanName"].ToString();
                txtFramandStrand.Text = dt.Rows[0]["FrameandStrand"].ToString();
                txtSpecStd.Text = dt.Rows[0]["SpecStandard"].ToString();
                txtspecEntrypoint.Text = dt.Rows[0]["SpecEntryPoint"].ToString();
                txtPrerequistskill.Text = dt.Rows[0]["PreReq"].ToString();
                txtMaterials.Text = dt.Rows[0]["Materials"].ToString();
            }
        }
        strQuery = "select GoalId from dbo.GoalLPRel where lessonPlanId = " + lessonid + " and ActiveInd = 'A'";
        dt = objData.ReturnDataTable(strQuery, true);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        foreach (ListItem li in chkgoal.Items)
                        {
                            if (li.Value == dt.Rows[i]["GoalId"].ToString())
                            {
                                li.Selected = true;
                            }
                        }
                    }

                }
            }
        }

        strQuery = "select TeachingProcId,SkillType,NbrOfTrials,ChainType,PromptTypeId,BaselineProc,CorrRespDef,IncorrRespDef,LessonDefInst,StudentReadCrita,TeacherRespReadness,ReinforcementProc,CorrectionProc,MajorSetting,MinorSetting,StudIncorrRespDef,StudCorrRespDef from DSTempHdr where DSTempHdrId = " + tempid + "";
        dt = objData.ReturnDataTable(strQuery, false);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                drpTeachingProc.SelectedValue = dt.Rows[0]["TeachingProcId"].ToString();
                drpTasklist.SelectedValue = dt.Rows[0]["ChainType"].ToString();
                if (dt.Rows[0]["SkillType"].ToString() != "")
                {
                    string Typeinc;
                    foreach (ListItem rdli in rdolistDatasheet.Items)
                    {
                        if (rdli.Text == "Task Analysis")
                        {
                            Typeinc = "Chained";
                        }
                        else
                        {
                            Typeinc = "Discrete";
                        }
                        if (Typeinc == dt.Rows[0]["SkillType"].ToString())
                        {
                            rdli.Selected = true;
                            break;
                        }
                    }
                }
                txtNoofTrail.Text = dt.Rows[0]["NbrOfTrials"].ToString();
                drpPromptproc.SelectedValue = dt.Rows[0]["PromptTypeId"].ToString();
                txtBaselineProc.Text = dt.Rows[0]["BaselineProc"].ToString();
                txtCorrtlessResp.Text = txtCorretResp.Text = dt.Rows[0]["CorrRespDef"].ToString();
                txtincorrtlessResp.Text = txtIncorrResp.Text = dt.Rows[0]["IncorrRespDef"].ToString();
                txtLessondelivery.Text = dt.Rows[0]["LessonDefInst"].ToString();
                txtStdread.Text = dt.Rows[0]["StudentReadCrita"].ToString();
                txtTeacherResp.Text = dt.Rows[0]["TeacherRespReadness"].ToString();
                txtReinfoProce.Text = dt.Rows[0]["ReinforcementProc"].ToString();
                txtCorretProce.Text = dt.Rows[0]["CorrectionProc"].ToString();
                txtmajorset.Text = dt.Rows[0]["MajorSetting"].ToString();
                txtminorset.Text = dt.Rows[0]["MinorSetting"].ToString();
                txtCorrtlessResp.Text = dt.Rows[0]["StudCorrRespDef"].ToString();
                txtincorrtlessResp.Text = dt.Rows[0]["StudIncorrRespDef"].ToString();
            }
        }
        strQuery = "select PromptId from DSTempPrompt where DSTempHdrId = " + tempid + " and ActiveInd = 'A'";
        dt = objData.ReturnDataTable(strQuery, true);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (ListItem lipu in chkpromptused.Items)
                    {
                        string[] value = lipu.Value.Split(',');
                        if (value[0] == dt.Rows[i]["PromptId"].ToString())
                        {
                            lipu.Selected = true;
                        }
                    }
                }
            }
        }


    }



    /// <summary>
    /// this funtion is to get tempate id from lession id and student id and school id
    /// 
    /// 
    /// </summary>
    /// <param name="lessionId"></param>
    /// <param name="studentId"></param>
    /// <param name="schoolId"></param>
    /// <returns></returns>
    /// 





    //public int gettemplateheaderid(int lessionId, string studentId, int schoolId)
    //{
    //    if (studentId == null || studentId == "" || studentId == "NULL" || studentId == "0")
    //    {
    //        studentId = " is null";
    //    }
    //    else
    //    {
    //        studentId = "= " + studentId;
    //    }
    //    int temphr = 0;
    //    object ret = null;
    //    strQuery = "select DH.statusid from DSTempHdr DH INNER JOIN [dbo].[LookUp] LK  on DH.StatusId= LK.LookupId WHERE DH.DSTempHdrId=" + ObjTempSess.TemplateId + "" +
    //                " AND LK.LookupType='TemplateStatus' AND LK.LookupName='Approved'";
    //    objData = new clsData();
    //    ret = objData.FetchValue(strQuery);
    //    if (ret == null)
    //    {
    //        butSave.Visible = true;
    //        butSave.Text = "Edit";
    //    }
    //    try
    //    {
    //        temphr = ObjTempSess.TemplateId;
    //    }
    //    catch
    //    {
    //        temphr = 0;
    //    }
    //    return temphr;
    //}

    private int updatevalidation()
    {
        int chkcount = 0;
        int rerror = 0;
        for (int i = 0; i < chkpromptused.Items.Count; i++)
        {
            if (chkpromptused.Items[i].Selected == true)
            {
                chkcount++;
            }
        }
        if (chkpromptused.Items[0].Selected == true && chkcount > 1)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Cannot more items when you select NA");
            rerror = 1;
        }
        else if (chkpromptused.Items[0].Selected != true && chkcount < 2)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Select ant two item from Prompt used");
            rerror = 1;
        }
        if (txtLessonName.Text == "")
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please enter Lesson name");
            rerror = 1;
        }
        if (rdolistDatasheet.SelectedValue == "1")
        {
            if (drpTasklist.SelectedValue == "0")
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Please choose a Task Analysis");
                rerror = 1;
            }
        }
        return rerror;
    }

    private int insetvalidation()
    {
        int chkcount = 0;
        int rerror = 0;
        for (int i = 0; i < chkpromptused.Items.Count; i++)
        {
            if (chkpromptused.Items[i].Selected == true)
            {
                chkcount++;
            }
        }
        if (chkpromptused.Items[0].Selected == true && chkcount > 1)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Cannot more items when you select NA");
            rerror = 1;
        }
        else if (chkpromptused.Items[0].Selected != true && chkcount < 2)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Select ant two item from Prompt used");
            rerror = 1;
        }

        if (txtLessonName.Text == "")
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please enter Lesson name");
            rerror = 1;
        }
        if (rdolistDatasheet.SelectedValue == "1")
        {
            if (drpTasklist.SelectedValue == "0")
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Please choose a Task Analysis");
                rerror = 1;
            }
        }
        return rerror;
    }


    /// <summary>
    /// Fill measurement system.....
    /// </summary>
    /// <param name="tempheaderId"></param>
    private void fillmeasurement(string tempheaderId)
    {

        strQuery = "select ColTypeCd from DSTempSetCol where DSTempHdrId = " + tempheaderId + " and ActiveInd ='A'";
        objData = new clsData();
        dt = new DataTable();
        dt = objData.ReturnDataTable(strQuery, false);
        lstMeasurement.DataSource = dt;
        lstMeasurement.DataTextField = "ColTypeCd";
        lstMeasurement.DataBind();

    }

    private void fillObjective(string studId, string schoolId, string lessId)
    {
        strQuery = "select Objective1,Objective2,Objective3 from StdtLessonPlan where SchoolId = " + schoolId + " and StudentId=" + studId + " and LessonPlanId=" + lessId + " and ActiveInd= 'A'";
        objData = new clsData();
        dt = new DataTable();
        dt = objData.ReturnDataTable(strQuery, false);
        if (dt.Rows.Count > 0)
        {
            txtObjective.InnerHtml += dt.Rows[0]["Objective1"].ToString() + "<br/>" + dt.Rows[0]["Objective2"].ToString() + "<br/>" + dt.Rows[0]["Objective3"].ToString();
        }
    }

    /// <summary>
    /// ------------------ filling CRITERIA -------------------
    /// </summary>
    /// <param name="tempheaderId"></param>
    private void fillcriteria(string tempheaderId)
    {
        string caltype = string.Empty;
        strQuery = "select DSTempSetColId,ColName from DSTempSetCol where DSTempHdrId = " + tempheaderId + " and ActiveInd ='A'";
        objData = new clsData();
        dt = new DataTable();
        dt = objData.ReturnDataTable(strQuery, false);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strQuery = " SELECT  DR.RuleType, DR.CriteriaType, DR.TotalInstance, DR.TotCorrInstance,DST.IncMisTrialInd, DST.ColName," +
                           "DC.CalcType,DC.CalcLabel,DT.DSTempHdrId, DR.ScoreReq,DR.ConsequetiveInd FROM DSTempHdr DT " +
                           "INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId " +
                           "INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId " +
                           "INNER JOIN DSTempRule DR ON DR.DSTempSetColCalcId = DC.DSTempSetColCalcId " +
                           "WHERE DR.DSTempSetColId=" + dt.Rows[0]["DSTempSetColId"].ToString();
                dt2 = new DataTable();
                dt2 = objData.ReturnDataTable(strQuery, false);
                for (int j = 0; j < dt2.Rows.Count; j++)
                {

                    if (dt2.Rows[j]["CalcLabel"].ToString() == "")
                    {
                        caltype = dt2.Rows[j]["CalcType"].ToString();
                    }
                    else
                    {
                        caltype = dt2.Rows[j]["CalcLabel"].ToString();
                    }

                    if (dt2.Rows[j]["CriteriaType"].ToString() == "MOVE UP")
                    {
                        if (dt2.Rows[j]["RuleType"].ToString() == "PROMPT")
                        {
                            if (dt2.Rows[j]["ConsequetiveInd"].ToString() == "False")
                            {
                                txtadvprompt.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " out of " + dt2.Rows[j]["TotalInstance"].ToString() + " session<br/>";
                            }
                            else
                            {
                                txtadvprompt.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " consecutive session<br/>";
                            }
                        }
                        else if (dt2.Rows[j]["RuleType"].ToString() == "SET")
                        {
                            if (dt2.Rows[j]["ConsequetiveInd"].ToString() == "False")
                            {
                                txtadvSet.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " out of " + dt2.Rows[j]["TotalInstance"].ToString() + " session<br/>";
                            }
                            else
                            {
                                txtadvSet.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " consecutive session<br/>";
                            }
                        }
                        else if (dt2.Rows[j]["RuleType"].ToString() == "STEP")
                        {
                            if (dt2.Rows[j]["ConsequetiveInd"].ToString() == "False")
                            {
                                txtadvstep.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " out of " + dt2.Rows[j]["TotalInstance"].ToString() + " session<br/>";
                            }
                            else
                            {
                                txtadvstep.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " consecutive session<br/>";
                            }
                        }
                    }
                    else if (dt2.Rows[j]["CriteriaType"].ToString() == "MOVE DOWN")
                    {
                        if (dt2.Rows[j]["RuleType"].ToString() == "PROMPT")
                        {
                            if (dt2.Rows[j]["ConsequetiveInd"].ToString() == "False")
                            {
                                txtpromptmoveback.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " out of " + dt2.Rows[j]["TotalInstance"].ToString() + " session<br/>";
                            }
                            else
                            {
                                txtpromptmoveback.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " consecutive session<br/>";
                            }
                        }
                        else if (dt2.Rows[j]["RuleType"].ToString() == "SET")
                        {
                            if (dt2.Rows[j]["ConsequetiveInd"].ToString() == "False")
                            {
                                txtsetmoveback.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " out of " + dt2.Rows[j]["TotalInstance"].ToString() + " session<br/>";
                            }
                            else
                            {
                                txtsetmoveback.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " consecutive session<br/>";
                            }
                        }
                        else if (dt2.Rows[j]["RuleType"].ToString() == "STEP")
                        {
                            if (dt2.Rows[j]["ConsequetiveInd"].ToString() == "False")
                            {
                                txtstepmoveback.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " out of " + dt2.Rows[j]["TotalInstance"].ToString() + " session<br/>";
                            }
                            else
                            {
                                txtstepmoveback.InnerHtml += dt2.Rows[j]["ScoreReq"].ToString() + " " + caltype + " of " + dt.Rows[i]["ColName"].ToString() + " in " + dt2.Rows[j]["TotCorrInstance"].ToString() + " consecutive session<br/>";
                            }
                        }
                    }

                }
            }
        }
    }

    private void disableFiledEdit()
    {
        txtmajorset.ReadOnly = true;
        txtminorset.ReadOnly = true;
        txtBaselineProc.ReadOnly = true;
        txtLessondelivery.ReadOnly = true;
        txtStdread.ReadOnly = true;
        drpTeachingProc.Enabled = false;
        drpPromptproc.Enabled = false;
        txtNoofTrail.ReadOnly = true;
        rdolistDatasheet.Enabled = false;
        chkpromptused.Enabled = false;
        txtCorrtlessResp.ReadOnly = true;
        txtincorrtlessResp.ReadOnly = true;
        txtTeacherResp.ReadOnly = true;
        txtReinfoProce.ReadOnly = true;
        txtCorretProce.ReadOnly = true;
        txtCorretResp.ReadOnly = true;
        txtIncorrResp.ReadOnly = true;
        drpTasklist.Enabled = false;
    }
    private void enableFiledEdit()
    {
        txtmajorset.ReadOnly = false;
        txtminorset.ReadOnly = false;
        txtBaselineProc.ReadOnly = false;
        txtLessondelivery.ReadOnly = false;
        txtStdread.ReadOnly = false;
        drpTeachingProc.Enabled = true;
        drpPromptproc.Enabled = true;
        txtNoofTrail.ReadOnly = false;
        rdolistDatasheet.Enabled = true;
        chkpromptused.Enabled = true;
        txtCorrtlessResp.ReadOnly = false;
        txtincorrtlessResp.ReadOnly = false;
        txtTeacherResp.ReadOnly = false;
        txtReinfoProce.ReadOnly = false;
        txtCorretProce.ReadOnly = false;
        txtCorretResp.ReadOnly = false;
        txtIncorrResp.ReadOnly = false;
        drpTasklist.Enabled = true;
    }

    protected void butSave_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        insert();
        if (butSave.Text == "Save" && insetvalidation() == 0)
        {
            insert();
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Lession Plan Saved Successfully");
        }
        else if (butSave.Text == "Edit")
        {
            enableFiledEdit();
            butSave.Text = "Update";
        }
        else if (butSave.Text == "Update" && updatevalidation() == 0)
        {
           
            string templidup = clsGeneral.GetTemplateHeaderId(Convert.ToInt32(ObjTempSess.LessonPlanId), sess.StudentId.ToString(), Convert.ToInt32(sess.SchoolId.ToString())).ToString();
            if (templidup == "0")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Error in Lesson Plan Configuration.");
                return;
            }

            if (Update(templidup) == true)
            {
                if (updatelesson(ObjTempSess.LessonPlanId.ToString()) == true)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Lesson Plan Updated Successfully.");
                }
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Error in Lesson plan configuration.");
            }
        }
    }
}