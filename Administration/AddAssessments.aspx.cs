using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Data.SqlClient;
using System.Data;

public partial class Admin_AddAssessments : System.Web.UI.Page
{
    //public static string errorMsg = "";
    clsSession oSession = null;
    clsData objData = null;
    static ClassAssess oAssess = new ClassAssess();
    //static DataTable _dtLP = null, _dtTemp = null, _dtSkills = null, _dtInActive = null;
    static bool Disabled = false;
    clsAsmntSession oAsmntSess;

    protected void Page_Load(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];

        if (oSession == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(oSession.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        if (!IsPostBack)
        {
            //static bool Disable = false;
            clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disabled);
            if (Disabled == true)
            {
                btnSubmit.Visible = false;
            }
            else
            {
                btnSubmit.Visible = true;
            }

            fillDrop();
            bool Disable = false;
            clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSubmit.Visible = false;
            }
            else
                btnSubmit.Visible = true;
        }
    }
    /// <summary>
    /// Fill DropDownList...
    /// </summary>
    protected void fillDrop()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT LookupCode as Id,LookupName as Name FROM LookUp WHERE LookupType='Assessment Name' and ActiveInd='A'", ddlAssess);
    }
    /// <summary>
    /// Submit button click event...During this event the XML file saved into database after validation...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        clsData oData = new clsData();
        oSession = (clsSession)Session["UserSession"];
        //oData.Execute("DELETE FROM AsmntTempStg\nDBCC CHECKIDENT('AsmntTempStg',reseed,0)");
        oData.Execute("DELETE FROM AsmntTempStg WHERE CreatedBy=" + oSession.LoginId);
        assmnt.InnerHtml = ddlAssess.SelectedItem.Text;
        if (fu_AssessXML.HasFile)
        {
            string extension = Path.GetExtension(fu_AssessXML.PostedFile.FileName);
            if (extension != ".xml")    //Check whether the file is XML or not...
            {
                tdmsg.InnerHtml = clsGeneral.failedMsg("File Format not Valid. Please check whether the selected file is XML or not");
            }
            else
            {
                oAssess.AssessName = ddlAssess.SelectedItem.Text;

                oAssess.AssessDesc = this.txt_AssessDesc.Text;
                string filename = Path.GetFileName(fu_AssessXML.FileName);
                fu_AssessXML.SaveAs(Server.MapPath("~/Assessmnts/New/" + filename));    //Save the selected file to the specified location....
                oAssess.AssessXML = Server.MapPath("~/Assessmnts/New/" + filename);
                bool valid = parseQtns(Server.MapPath("~/Assessmnts/New/" + filename));
                oSession = (clsSession)Session["UserSession"];
                if (valid == false)
                {
                    //tdmsg.InnerHtml = clsGeneral.failedMsg(errorMsg);
                }
                txt_AssessDesc.Text = "";

            }
        }
        else
        {
            lbl_Msg.InnerHtml = clsGeneral.warningMsg("Select a XML File");
        }
    }
    /// <summary>
    /// XML parsing..This function stores all the questions in the XML into AsmntTempTable...
    /// And also check whether the Skills,Names are valid or not...
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    protected bool parseQtns(string xml)
    {

        bool valid = false;
        clsData oData = new clsData();
        oSession = (clsSession)Session["UserSession"];
    RollBack://When Invalid Sections,Goals are detected in the XML. These area Executes...
        {
            //oData.Execute("DELETE FROM AsmntTempStg\nDBCC CHECKIDENT('AsmntTempStg',reseed,0)");
            oData.Execute("DELETE FROM AsmntTempStg WHERE CreatedBy=" + oSession.LoginId);
            valid = false;
        }
        try
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xml);
            XmlNodeList xSections = xdoc.SelectNodes("/Sections/Section[@Skill]");
            DataClass dtCls = new DataClass();
            if (xSections.Count == 0)
            {
                tdmsg.InnerHtml = "Check whether an Attribute 'Skill' is there in the Section tag of the XML";
                goto RollBack;
            }


            int sort = 1;
            string section = "";
            foreach (XmlNode xSectn in xSections)
            {
                string skill = xSectn.Attributes["Skill"].Value;
                section = xSectn.Attributes["name"].Value;
                foreach (XmlNode xSubSectns in xSectn.ChildNodes)
                {
                    for (int index = 0; index < xSubSectns.ChildNodes.Count; index++)
                    {
                        string code = "", subsection = "", insQry = "";
                        if (xSubSectns.ChildNodes[index].Attributes["ContainsQuestions"] != null)
                            if (xSubSectns.ChildNodes[index].Attributes["ContainsQuestions"].Value == "True")
                            {
                                foreach (XmlNode xQtns in xSubSectns.ChildNodes[index].ChildNodes)
                                {
                                    for (int index2 = 0; index2 < xQtns.ChildNodes.Count; index2++)
                                    {
                                        subsection = xSubSectns.ChildNodes[index].Attributes["name"].Value;
                                        code = xQtns.ChildNodes[index2].Attributes["Code"].Value;
                                        insQry = "INSERT INTO [AsmntTempStg]([SchoolId],[GoalName],[AsmntName],[AsmntCat],[AsmntSubCat],[AsmntQId],[SortOrder],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) " +
                                            "VALUES(" + oSession.SchoolId + ",'" + skill + "','" + ddlAssess.SelectedItem.Text + "','" + section + "','" + subsection + "','" + code + "'," + sort++ + "," +
                                            "" + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100))," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n\nSELECT SCOPE_IDENTITY()";
                                        int nRetVal = dtCls.ExecuteScalar(insQry);
                                        if (nRetVal <= 0)
                                        {
                                            tdmsg.InnerHtml = "Upload Failed. Duplicate values in the Section '" + section + "'";
                                            goto RollBack;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                code = xSubSectns.ChildNodes[index].Attributes["Code"].Value;
                                insQry = "INSERT INTO [AsmntTempStg]([SchoolId],[GoalName],[AsmntName],[AsmntCat],[AsmntQId],[SortOrder],[CreatedBy],[CreatedOn]) " +
                                            "VALUES(" + oSession.SchoolId + ",'" + skill + "','" + ddlAssess.SelectedItem.Text + "','" + section + "','" + code + "'," + sort++ + "," +
                                            "" + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n\nSELECT SCOPE_IDENTITY()";
                                int nRetVal = dtCls.ExecuteScalar(insQry);
                                if (nRetVal <= 0)
                                {
                                    tdmsg.InnerHtml = "Upload Failed. Duplicate values in the Section '" + section + "'";
                                    goto RollBack;
                                }
                            }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            tdmsg.InnerHtml = ex.Message;
            throw ex;
        }
        //The following process calls a stored procedure and find all the miss matched questions,skills,etc...
        SqlCommand command;
        SqlDataAdapter adp;
        try
        {
            command = new SqlCommand("[ValidateAsmnt]", new SqlConnection(oData.ConnectionString));
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SchoolId", oSession.SchoolId);
            command.Parameters.AddWithValue("@AsmntName", ddlAssess.SelectedItem.Text);
            command.Connection.Open();
            command.ExecuteNonQuery();
            DataSet dttemp = new DataSet();
            adp = new SqlDataAdapter(command);
            adp.Fill(dttemp);
            adp = null;
            //if (dttemp.Tables[0].Rows.Count > 0)
            //    grdSkills.DataSource = dttemp.Tables[0];
            if (dttemp.Tables[2].Rows.Count > 0)
            {
                tdTemp.InnerText = "Click Submit to Insert this Questions in to the Assessment-LessonPlan Relationship Table";
                grdTempQtns.DataSource = dttemp.Tables[2];
            }
            else
                tdTemp.InnerText = "All the Questions in the Assessment are there in the Assessment-LessonPlan Relationship Table";
            if (dttemp.Tables[1].Rows.Count > 0)
            {
                tdLP.InnerText = "Click Submit to Inactive this Questions";
                grdLPQtns.DataSource = dttemp.Tables[1];
            }
            else
                tdLP.InnerText = "All the Questions in the Assessment are Mapped Properly in the Assessment-LessonPlan Relationship Table";
            oAsmntSess = new clsAsmntSession();
            if (dttemp.Tables[3].Rows.Count > 0)
            {
                //_dtSkills = dttemp.Tables[3];
                oAsmntSess.dt_Skill = dttemp.Tables[3];
            }
            //_dtLP = dttemp.Tables[1];
            oAsmntSess.dt_LP = dttemp.Tables[1];
            //_dtTemp = dttemp.Tables[2];
            oAsmntSess.dt_Temp = dttemp.Tables[2];
            //_dtInActive = dttemp.Tables[4];
            oAsmntSess.dt_Inactive = dttemp.Tables[4];
            Session["AsmntSess"] = oAsmntSess;
            if (dttemp.Tables[4].Rows.Count > 0)
            {
                tdInactive.InnerHtml = "Click Submit to Active this Questions";
                grdInActive.DataSource = dttemp.Tables[4];
                grdInActive.DataBind();
            }
            else
                tdInactive.InnerHtml = "All the Questions in the Assessment are Active!!!";
            grdLPQtns.DataBind();
            //grdSkills.DataBind();
            grdTempQtns.DataBind();
            grdLPQtns.PageIndex = 0;
            grdTempQtns.PageIndex = 0;
            if (dttemp.Tables[0].Rows.Count > 0)//Upload not possible,because XML contains invalid skills....
            {
                string invalid_goals = "";
                for (int index = 0; index < dttemp.Tables[0].Rows.Count; index++)
                {
                    invalid_goals = invalid_goals + dttemp.Tables[0].Rows[index]["GoalName"].ToString() + ", ";
                }
                invalid_goals = invalid_goals.Substring(0, (invalid_goals.Length - 1));
                tdmsg.InnerHtml = "Insertion not Possible because One or more unknown Skills(" + invalid_goals + ") are there in the Assessment.";
                btnSbmit.Visible = false;
                //oData.Execute("DELETE FROM AsmntTempStg\nDBCC CHECKIDENT('AsmntTempStg',reseed,0)");
                oData.Execute("DELETE FROM AsmntTempStg WHERE CreatedBy=" + oSession.LoginId);
            }
            else
            {
                if ((dttemp.Tables[1].Rows.Count == 0) & (dttemp.Tables[2].Rows.Count == 0) & (dttemp.Tables[4].Rows.Count == 0))//if all the questions are properly marked...
                {
                    lblMessage.Text = "All the Questions are properly Mapped in the Assessment-LessonPlan Relationship Table. Click Submit to Insert the Assessment";
                    btnSbmit.Visible = true;
                    valid = true;
                }
                else
                {
                    lblMessage.Text = "Click Submit to Insert the Unmapped Questions into the Assessment-LessonPlan Relationship Table";
                    btnSbmit.Visible = true;
                    valid = true;
                }
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
                ClientScript.RegisterStartupScript(this.GetType(), "", "ShowDialog(true);", true);
            }
        }
        catch (Exception ex)
        {
            tdmsg.InnerHtml = ex.Message;
            throw ex;
        }

        return valid;
    }
    protected void grdTempQtns_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        oAsmntSess = (clsAsmntSession)Session["AsmntSess"];
        if (oAsmntSess != null)
        {
            grdTempQtns.PageIndex = e.NewPageIndex;
            grdTempQtns.DataSource = oAsmntSess.dt_Temp;
            grdTempQtns.DataBind();
        }
    }
    protected void grdLPQtns_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        oAsmntSess = (clsAsmntSession)Session["AsmntSess"];
        if (oAsmntSess != null)
        {
            grdLPQtns.PageIndex = e.NewPageIndex;
            grdLPQtns.DataSource = oAsmntSess.dt_LP;
            grdLPQtns.DataBind();
        }
    }

    protected bool ValidateData()
    {
        bool check = true;
        //DataClass oData = new DataClass();
        oSession = (clsSession)Session["UserSession"];
        oAsmntSess = (clsAsmntSession)Session["AsmntSess"];
        clsData oData = new clsData();
        try
        {
            if (oAsmntSess != null)
            {
                foreach (DataRow drLP in oAsmntSess.dt_LP.Rows)
                {
                    int goalID = Convert.ToInt32(oData.FetchValue("SELECT GoalId FROM Goal WHERE GoalName='" + drLP["LPGoal"].ToString() + "'"));
                    string updLP = "UPDATE AsmntLPRel SET ActiveInd='N',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE AsmntName='" + ddlAssess.SelectedItem.Text + "' AND AsmntCat='" + drLP["LPCat"].ToString() + "' " +
                                "AND ISNULL(AsmntSubCat,'')='" + drLP["LPSubCat"].ToString() + "' AND AsmntQId='" + drLP["LPQId"].ToString() + "' AND GoalId=" + goalID + "";
                    oData.Execute(updLP);

                }
                foreach (DataRow drTemp in oAsmntSess.dt_Temp.Rows)
                {
                    int goalid = Convert.ToInt32(oData.FetchValue("SELECT GoalId FROM Goal WHERE GoalName='" + drTemp["TempGoal"] + "'"));
                    if (goalid > 0)
                    {
                        string insTempQtn = "INSERT INTO AsmntLPRel(GoalId,AsmntName,AsmntCat,AsmntSubCat,AsmntQId,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) " +
                                    "VALUES(" + goalid + ",'" + ddlAssess.SelectedItem.Text + "','" + drTemp["TempCat"] + "','" + drTemp["TempSubCat"].ToString() + "','" + drTemp["TempQId"].ToString() + "'," +
                                    "'A'," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100))," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                        oData.Execute(insTempQtn);
                    }
                }
                foreach (DataRow drInactive in oAsmntSess.dt_Inactive.Rows)
                {
                    int goalID = Convert.ToInt32(oData.FetchValue("SELECT GoalId FROM Goal WHERE GoalName='" + drInactive["LPGoal"].ToString() + "'"));
                    string updLP = "UPDATE AsmntLPRel SET ActiveInd='A',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE AsmntName='" + ddlAssess.SelectedItem.Text + "' AND AsmntCat='" + drInactive["LPCat"].ToString() + "' " +
                                "AND ISNULL(AsmntSubCat,'')='" + drInactive["LPSubCat"].ToString() + "' AND AsmntQId='" + drInactive["LPQId"].ToString() + "' AND GoalId=" + goalID + "";
                    oData.Execute(updLP);
                }
            }
            else
                check = false;
        }
        catch (Exception ex)
        {
            check = false;
            throw ex;
        }
        return check;
    }
    /// <summary>
    /// Insert the XML into the database..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSbmit_Click(object sender, EventArgs e)
    {
        if (Validation() == true)
        {
            //DataClass oData = new DataClass();
            if (ValidateData())
            {
                try
                {
                    objData = new clsData();
                    oSession = (clsSession)Session["UserSession"];
                    oAsmntSess = (clsAsmntSession)Session["AsmntSess"];
                    if (oAsmntSess != null)
                    {
                        //If the Assessment is already in the database, Update the existing Assessment as Inactive and return the Id of the assessment using 'OUTPUT INSERTED.PrimaryKeyID' ,then insert the new XML..
                        string updAsmnt = "UPDATE Assessment SET EffEndDate=(SELECT convert(varchar, getdate(), 100)),ActiveInd='N',ModifiedBy=" + oSession.LoginId + "," +
                                            "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) " +
                                            "OUTPUT INSERTED.AsmntId " +
                                            "WHERE AsmntName='" + ddlAssess.SelectedItem.Text + "' AND " +
                                            "ISNULL(StudentId,'')='' AND ISNULL(EffEndDate,'')='' AND ActiveInd='A'";
                        object objAsmntId = objData.FetchValue(updAsmnt);
                        if (objAsmntId != null)
                        {
                            objData.Execute("DELETE FROM AsmntGoalRel WHERE AsmntId=" + objAsmntId.ToString());
                        }

                        SqlTransaction Transs = null;
                        objData = new clsData();
                        clsData.blnTrans = true;
                        SqlConnection con = objData.Open();
                        Transs = con.BeginTransaction();

                        string ins = "";

                        ins = "INSERT INTO [Assessment]([SchoolId],[AsmntName],[AsmntDesc],[AsmntXML]," +
                                         "[EffStartDate],[AsmntStatusId],[ActiveInd],[CreatedBy],[CreatedOn],[ModifiedBy],[ModifiedOn]) " +
                                         "VALUES(@School, @Name, @Desc, @XML, (SELECT convert(varchar, getdate(), 100)),0,'A', " +
                                         "@User, (SELECT convert(varchar, getdate(), 100)),@User, (SELECT convert(varchar, getdate(), 100)))\n\t" +
                                         "SELECT SCOPE_IDENTITY();";
                        int AssessID = oAssess.Save(ins, oSession.LoginId, oSession.SchoolId, con, Transs);
                        //AssessID = (int)oData.ExecuteScalar("SELECT AsmntId FROM Assessment WHERE ActiveInd='Y' AND AsmntName='" + ddlAssess.SelectedItem.Text + "' AND EffEndDate IS NULL AND StudentId IS NULL");
                        //After XML insertion ,Store the Goals in Goal table with the Assessmnt ID...
                        if (AssessID > 0)
                        {
                            foreach (DataRow drSkill in oAsmntSess.dt_Skill.Rows)
                            {
                                if (Convert.ToInt32(drSkill["GoalId"]) > 0)
                                {
                                    string insGoal = "INSERT INTO [AsmntGoalRel]([AsmntId],[GoalId],[SchoolId],[Description],[CreatedBy],[CreateOn],[ModifiedBy],[ModifiedOn]) " +
                                                "VALUES(@asmnt,@goal,@school,@desc,@user,(SELECT convert(varchar, getdate(), 100)),@user,(SELECT convert(varchar, getdate(), 100)))";

                                    oAssess.SaveGoal(insGoal, oSession.SchoolId, oSession.LoginId, AssessID, Convert.ToInt32(drSkill["GoalId"]), txt_AssessDesc.Text, con, Transs);
                                }
                            }
                        }
                        objData.CommitTransation(Transs, con);
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Successfully Added');", true);
                        oAsmntSess.dt_Temp = objData.ReturnDataTable("AsmntTempStg");

                        //Update the sort order in the AsmntLPRel Table based on AsmntTempStg table....
                        foreach (DataRow drtemp in oAsmntSess.dt_Temp.Rows)
                        {
                            objData.Execute("UPDATE AsmntLPRel SET SortOrder=" + drtemp["SortOrder"].ToString() + " WHERE [AsmntName]='" + drtemp["AsmntName"].ToString() + "' AND [AsmntCat]='" + drtemp["AsmntCat"].ToString() + "' " +
                                        "AND ISNULL([AsmntSubCat],'')='" + drtemp["AsmntSubCat"].ToString() + "' AND [AsmntQId]='" + drtemp["AsmntQId"] + "'");
                        }


                        //oData.Execute("DELETE FROM AsmntTempStg\nDBCC CHECKIDENT('AsmntTempStg',reseed,0)");
                        objData.Execute("DELETE FROM AsmntTempStg WHERE CreatedBy=" + oSession.LoginId);
                        Response.Redirect("AssessmntList.aspx");
                    }
                }
                catch (SqlException ex)
                {
                    objData.RollBackTransation();
                    tdmsg.InnerHtml = clsGeneral.sucessMsg(ex.Message.ToString());
                    throw ex;
                }
            }
            else
            {
                tdmsg.InnerHtml = clsGeneral.sucessMsg("Error Occured while Inserting Unmapped Questions");
            }
        }

    }
    protected void grdInActive_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        oAsmntSess = (clsAsmntSession)Session["AsmntSess"];
        if (oAsmntSess != null)
        {
            grdInActive.PageIndex = e.NewPageIndex;
            grdInActive.DataSource = oAsmntSess.dt_Inactive;
            grdInActive.DataBind();
        }
    }
    /// <summary>
    /// User cancels the uploading, then delete all the Questions from the temp table...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Closepop_Click(object sender, EventArgs e)
    {
        clsData oData = new clsData();
        oSession = (clsSession)Session["UserSession"];
        //oData.Execute("DELETE FROM AsmntTempStg\nDBCC CHECKIDENT('AsmntTempStg',reseed,0)");
        oData.Execute("DELETE FROM AsmntTempStg WHERE CreatedBy=" + oSession.LoginId);
        tdmsg.InnerHtml = clsGeneral.warningMsg("Uploading Cancelled by User");
    }
    private bool Validation()
    {
        if (ddlAssess.SelectedIndex == 0)
        {
            tdmsg.InnerHtml = clsGeneral.warningMsg("Please Select Assessment");
            ddlAssess.Focus();
            return false;
        }


        return true;
    }
}