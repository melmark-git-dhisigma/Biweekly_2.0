using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Text;
using System.IO;

public partial class Admin_AssessmntList : System.Web.UI.Page
{
    clsSession sess = null;
    static bool Disable = false;

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


        if (Page.IsPostBack == false)
        {
            fillGrid();
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
               
                if (grdAssessmnts.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdAssessmnts.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("imgbtnDelete"));
                        lb_delete.Visible = false;
                    }
                }

            }
            else
            {

                if (grdAssessmnts.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdAssessmnts.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("imgbtnDelete"));
                        lb_delete.Visible = true;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Function to fill the gridview
    /// </summary>
    public void fillGrid()
    {
        DataClass oData = new DataClass();
        //string selAssmnts = "SELECT AsmntTemplateId,AsmntTemplateName,AcademicInd,BasicLearnerInd,SelfHelpInd,MotorInd,LanguageInd,SocialInd,DailyLivingInd,PersonalRespInd, " +
        //                  "SocialAdaptnsInd,CommunicationInd,TransitionInd,CommunityLivingInd,EmploymentInd,CognitiveInd,CreatedBy,CreatedOn FROM AsmntTemplate";
        string sel = "SELECT [AsmntId],[AsmntName],Asmnt.[ModifiedOn],(Usr.[UserLName]+' '+Usr.[UserFName]) as CreatedBy FROM ([Assessment] Asmnt INNER JOIN [User] Usr ON Usr.UserId=Asmnt.CreatedBy) WHERE Asmnt.[ActiveInd]='A' AND ISNULL(StudentId,'')=''";
        grdAssessmnts.DataSource = oData.fillData(sel);
        grdAssessmnts.DataBind();
        if (grdAssessmnts.Rows.Count == 0)
        {
            Msg.InnerHtml = "No Data Found !!!!";
        }
        else
            Msg.Visible = false;
    }
    /// <summary>
    /// Function to perform delete and view in gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdAssessmnts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataClass oData = new DataClass();
        if (e.CommandName == "Delete")  //Update the Assessment as InActive
        {
            string delAssess = "UPDATE Assessment SET ActiveInd='N',ModifiedBy=" + sess.LoginId + ",ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE AsmntId=" + Convert.ToInt32(e.CommandArgument);
            int check = oData.ExecuteNonQuery(delAssess);
            string selAmntname = "SELECT AsmntName FROM Assessment WHERE AsmntId=" + Convert.ToInt32(e.CommandArgument);
            if (check > 0)
            {
                string updAsmntLP = "UPDATE AsmntLPRel SET ActiveInd='N',ModifiedBy=" + sess.LoginId + ",ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE LTRIM(RTRIM(AsmntName)) ='" + selAmntname.Trim() + "'";
                oData.ExecuteNonQuery(updAsmntLP);
            }
            fillGrid();
        }
        if (e.CommandName == "View")    //View the skills and details of the assessment  |  GoalCode is used to display the Edited goals
        {
            DataTable dtSkills = new DataTable();
            DataTable dtAsmnt = new DataTable();
            ClassDatatable oDt = new ClassDatatable();

            string sel = "SELECT Asmnt.AsmntName,Asmnt.AsmntDesc,Gl.GoalName,GoalCode,Asmnt.ModifiedOn,(Usr.UserLName+' '+Usr.UserFName) as Name FROM Assessment Asmnt " +
                        "INNER JOIN [User] Usr ON Usr.UserId=Asmnt.CreatedBy " +
                        "INNER JOIN (AsmntGoalRel Rel INNER JOIN Goal Gl ON Gl.GoalId=Rel.GoalId) ON Rel.AsmntId=Asmnt.AsmntId " +
                        "WHERE Asmnt.AsmntId=" + e.CommandArgument.ToString() + "  AND Gl.GoalTypeId=(Select GoalTypeId from GoalType Where GoalTypeName='Academic Goals')";
            dtAsmnt = oData.fillData(sel);
            if (dtAsmnt.Rows.Count > 0)
            {
                lblName.Text = dtAsmnt.Rows[0]["AsmntName"].ToString();
                lblDesc.Text = dtAsmnt.Rows[0]["AsmntDesc"].ToString();
                lblby.Text = dtAsmnt.Rows[0]["Name"].ToString();
                lblOn.Text = dtAsmnt.Rows[0]["ModifiedOn"].ToString();
            }
            dtSkills = oData.fillData("SELECT REPLACE(GoalCode,'_',' ') as GoalCode FROM Goal where ActiveInd='A' ");
            dtSkills = oDt.CreateColumn("Url", dtSkills);
            foreach (DataRow drSkill in dtSkills.Rows)
            {
                int isExists = oData.ExecuteScalar("SELECT [AsmntGoalRelId] FROM [AsmntGoalRel] WHERE GoalId=(SELECT GoalId FROM Goal WHERE REPLACE(GoalCode,'_',' ')='" + drSkill["GoalCode"] + "') " +
                                            "AND AsmntId=" + e.CommandArgument.ToString() + "");
                if (isExists > 0)
                    drSkill["Url"] = "Images/Tick.png";
                else
                    drSkill["Url"] = "Images/Delete.ico";
            }
            dlAsmntView.DataSource = dtSkills;
            dlAsmntView.DataBind();

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "ShowDialog(true);", true);
        }
        if (e.CommandName == "Preview")
        {
            oData = new DataClass();
            ClassAssess oAssess = new ClassAssess();
            XmlDocument xmldoc = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(e.CommandArgument.ToString()));

            //Converting the xmldocument into byte array and bind it to the datalist

            byte[] bytes = Encoding.Default.GetBytes(xmldoc.OuterXml);
            Stream str = new MemoryStream(bytes);

            DataSet dsXml = new DataSet();
            dsXml.ReadXml(str);

            dl_Sections.DataSource = dsXml;         //To bind the whole data without filter
            dl_Sections.DataBind();

            xmldoc = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(e.CommandArgument.ToString()));
            XmlNodeList xmldocnodes = xmldoc.GetElementsByTagName("Section");

            foreach (DataListItem diSection in dl_Sections.Items)       //Binding all the sections into the datalist
            {
                //HiddenField hf_Skill = (HiddenField)diSection.FindControl("hfSkill");
                LinkButton lbSectn = (LinkButton)diSection.FindControl("lb_Section");
                //_skill = hf_Skill.Value;
                
                DataList dlSubSection = (DataList)diSection.FindControl("dl_SubSections");
                GridView grdSubSection = (GridView)diSection.FindControl("grd_SubSections");

                for (int index = 0; index < xmldocnodes.Count; index++)
                {
                    if (lbSectn.Text == xmldocnodes.Item(index).Attributes["name"].Value)
                    {
                        //_section = lbSectn.Text;
                        XmlNodeList xmldocChild = xmldoc.GetElementsByTagName("Section").Item(index).ChildNodes;
                        for (int j = 0; j < xmldocChild.Count; j++)
                        {
                            XmlNodeList xmlsubChild = xmldocChild.Item(j).ChildNodes;
                            DataTable dtGrdSubSectn = new DataTable();
                            DataTable dtDlSubSectn = new DataTable();

                            ClassFormAssess oForm = new ClassFormAssess();
                            dtGrdSubSectn = oForm.ConvertXmlNodeListToDataTable(xmlsubChild, "False");
                            grdSubSection.DataSource = dtGrdSubSectn;
                            grdSubSection.DataBind();
                            
                            //
                            if (xmlsubChild.Item(j).Attributes["ContainsQuestions"].Value == "True")
                            {

                                dtDlSubSectn = oForm.ConvertXmlNodeListToDataTable(xmlsubChild, "True");
                                dlSubSection.DataSource = dtDlSubSectn;
                                dlSubSection.DataBind();
                            }

                        }
                    }
                }
                foreach (DataListItem diSubSection in dlSubSection.Items)       //Binding all the SubSections into the datalist
                {
                    GridView grdQuestions = (GridView)diSubSection.FindControl("grd_Questions");
                    LinkButton lbSubSectn = (LinkButton)diSubSection.FindControl("lb_SubSection");
                    XmlNodeList xmlques = xmldoc.GetElementsByTagName("SubSection");
                    for (int index1 = 0; index1 < xmlques.Count; index1++)
                    {
                        if (lbSubSectn.Text == xmlques.Item(index1).Attributes["name"].Value)
                        {
                            //_subsectn = lbSubSectn.Text;
                            XmlNodeList xmlquesChild = xmlques.Item(index1).ChildNodes;
                            for (int index2 = 0; index2 < xmlquesChild.Count; index2++)
                            {
                                XmlNodeList xmlsubChild = xmlquesChild.Item(index2).ChildNodes;
                                DataTable dtGrdSubSectn = new DataTable();
                                ClassFormAssess oForm = new ClassFormAssess();
                                dtGrdSubSectn = oForm.ConvertXmlNodeListToDataTable(xmlsubChild);
                                grdQuestions.DataSource = dtGrdSubSectn;

                                grdQuestions.DataBind();
                                
                                
                            }
                        }
                    }
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "ShowDialogPreview(true);", true);
        }
        
    }
    protected void grdAssessmnts_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void grdAssessmnts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
            
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                grdAssessmnts.Columns[5].Visible = false;
            }
            else
            {
                grdAssessmnts.Columns[5].Visible = true;
            }
        
    }
}