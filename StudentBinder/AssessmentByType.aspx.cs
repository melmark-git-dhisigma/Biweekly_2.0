using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Services;

public partial class Administration_FormAssess : System.Web.UI.Page
{
    ClassFormAssess oForm = new ClassFormAssess();
    ClassAssess oAssess = new ClassAssess();
    XmlDocument xmldoc = new XmlDocument();
    XmlNodeList xmldocnodes;
    XmlNodeList xmlsubChild;
    public XmlNodeList xmlsubsections;
    clsSession oSession = null;
    //public static DataTable _dtTest;
    List<TextBox> txtCollctn = new List<TextBox>();
    //public static string _section = "", _skill = "", _subsectn = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        //System.Threading.Thread.Sleep(10000);

        oSession = (clsSession)Session["UserSession"];

        if (oSession == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        //else
        //{
        //    bool flag = clsGeneral.PageIdentification(oSession.perPage);
        //    if (flag == false)
        //    {
        //        Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
        //    }
        //}

        if (!IsPostBack)
        {
            bool Disable = false;
            //clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
            if (Request.QueryString["Type"] != null)
                Disable = Convert.ToBoolean(Request.QueryString["Type"]);
            //if (Disable == true)
            //{
            //    btn_Save.Visible = false;
            //    btn_Submit.Visible = false;
            //}
            //else
            //{
            //    btn_Save.Visible = true;
            //    btn_Submit.Visible = true;
            //}
            clsData oData = new clsData();
            if (oSession != null)
            {
                lblYear.Text = "";
                object yr = oData.FetchValue("SELECT AsmntYearCode FROM AsmntYear Yr INNER JOIN Assessment Asmnt ON Asmnt.AsmntYearId= Yr.AsmntYearId WHERE Asmnt.AsmntID=" + Session["xmlname"].ToString() + "").ToString();
                if (yr != null)
                {
                    lblYear.Text = yr.ToString();
                }
            }
            LoadXML();

            //  bool Disable = false;
            //clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
            //if (Disable == true)
            //{
            //    btnSave.Visible = false;
            //    btnSubmit.Visible = false;
            //}
            //else
            //{
            //    btnSave.Visible = true;
            //    btnSubmit.Visible = true;
            //}
        }

    }
    protected void LoadXML()
    {
        try
        {
            btnSave.Style.Add("display", "none"); //
            btnSubmit.Style.Add("display", "none"); // To make the button invisible, and also to make it accessible in javascript, we need to add this style.....
            DataClass oData = new DataClass();
            txt_AssmntName.Text = oData.ExecuteScalarString("SELECT AsmntTemplateName FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
            lbl_Asmnt.Text = "Assessment Sheet for " + oData.ExecuteScalarString("SELECT AsmntName FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
            lblStatus.Text = oData.ExecuteScalarString("SELECT LookupName FROM LookUp Look INNER JOIN Assessment Asmnt ON LookUpId=AsmntStatusId WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));

            lblNote.Text = oData.ExecuteScalarString("SELECT Note FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
            if (lblNote.Text.Trim() != "")
                notepnl.Visible = true;
            xmldoc = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));

            //Converting the xmldocument into byte array and bind it to the datalist

            byte[] bytes = Encoding.Default.GetBytes(xmldoc.OuterXml);
            Stream str = new MemoryStream(bytes);

            DataSet dsXml = new DataSet();
            dsXml.ReadXml(str);
            if (Session["skill"].ToString() != "All")   //To filter the xml for the selcted Skill
            {
                DataTable dtAssess_temp = new DataTable();
                ClassDatatable oDt = new ClassDatatable();
                dtAssess_temp = oDt.CreateColumn(dsXml.Tables[0].Columns[0].ColumnName, dtAssess_temp);
                dtAssess_temp = oDt.CreateColumn(dsXml.Tables[0].Columns[1].ColumnName, dtAssess_temp);
                dtAssess_temp = oDt.CreateColumn(dsXml.Tables[0].Columns[2].ColumnName, dtAssess_temp);
                foreach (DataRow drSkill in dsXml.Tables[0].Rows)
                {
                    if (drSkill["Skill"].ToString() == Session["skill"].ToString())
                    {
                        dtAssess_temp = oDt.CreateAssessmntsTable(new string[] { dtAssess_temp.Columns[0].ColumnName, dtAssess_temp.Columns[1].ColumnName, dtAssess_temp.Columns[2].ColumnName }, dtAssess_temp, new string[] { drSkill[0].ToString(), drSkill[1].ToString(), drSkill[2].ToString() });
                    }
                }
                dl_Sections.DataSource = dtAssess_temp;
            }
            else
                dl_Sections.DataSource = dsXml;         //To bind the whole data without filter
            dl_Sections.DataBind();

            if (dl_Sections.Items.Count > 0)
            {
                Msg.Visible = false;
                xmldoc = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
                xmldocnodes = xmldoc.GetElementsByTagName("Section");

                foreach (DataListItem diSection in dl_Sections.Items)       //Binding all the sections into the datalist
                {
                    HiddenField hf_Skill = (HiddenField)diSection.FindControl("hfSkill");
                    LinkButton lbSectn = (LinkButton)diSection.FindControl("lb_Section");
                    string _section = "";
                    string _skill = hf_Skill.Value;
                    string skill = Session["skill"].ToString();
                    if (skill != "All")
                        if (hf_Skill.Value != Session["skill"].ToString())
                        {
                            //lbSectn.Visible = false;
                            //Image img = (Image)diSection.FindControl("imgArrows");
                            //img.Visible = false;

                            //Panel pnl = (Panel)diSection.FindControl("pnlClick");
                            //pnl.Visible = false;
                            continue;
                        }

                    DataList dlSubSection = (DataList)diSection.FindControl("dl_SubSections");
                    GridView grdSubSection = (GridView)diSection.FindControl("grd_SubSections");

                    for (int index = 0; index < xmldocnodes.Count; index++)
                    {
                        if (lbSectn.Text == xmldocnodes.Item(index).Attributes["name"].Value)
                        {
                            _section = lbSectn.Text;
                            XmlNodeList xmldocChild = xmldoc.GetElementsByTagName("Section").Item(index).ChildNodes;
                            for (int j = 0; j < xmldocChild.Count; j++)
                            {
                                XmlNodeList xmlsubChild = xmldocChild.Item(j).ChildNodes;
                                DataTable dtGrdSubSectn = new DataTable();
                                DataTable dtDlSubSectn = new DataTable();


                                dtGrdSubSectn = oForm.ConvertXmlNodeListToDataTable(xmlsubChild, "False");
                                grdSubSection.DataSource = dtGrdSubSectn;
                                grdSubSection.DataBind();
                                // Retrieving scores from the xml to the TextBox.....
                                int count = 0;
                                foreach (GridViewRow grSubSect in grdSubSection.Rows)
                                {
                                    HiddenField hf_Sect = (HiddenField)grSubSect.FindControl("hfSectn");
                                    hf_Sect.Value = _section;
                                    HiddenField hf_SectSkill = (HiddenField)grSubSect.FindControl("hfSkill");
                                    hf_SectSkill.Value = _skill;
                                    XmlNodeList xmlSub_Temp = xmlsubChild;
                                    TextBox txtPoint = (TextBox)grSubSect.FindControl("txt_Section");
                                    CheckBox chkSectApp = (CheckBox)grSubSect.FindControl("chkSectAppl");
                                    txtPoint.Text = xmlSub_Temp.Item(count).InnerText;
                                    if (xmlSub_Temp.Item(count).Attributes["Applicable"].Value == "false")
                                        chkSectApp.Checked = true;
                                    count++;
                                }
                                //
                                if (xmlsubChild.Item(j).Attributes["ContainsQuestions"] != null)
                                {
                                    if (xmlsubChild.Item(j).Attributes["ContainsQuestions"].Value == "True")
                                    {

                                        dtDlSubSectn = oForm.ConvertXmlNodeListToDataTable(xmlsubChild, "True");
                                        dlSubSection.DataSource = dtDlSubSectn;
                                        dlSubSection.DataBind();
                                    }
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
                                string _subsectn = lbSubSectn.Text;
                                XmlNodeList xmlquesChild = xmlques.Item(index1).ChildNodes;
                                for (int index2 = 0; index2 < xmlquesChild.Count; index2++)
                                {
                                    xmlsubChild = xmlquesChild.Item(index2).ChildNodes;
                                    DataTable dtGrdSubSectn = new DataTable();

                                    dtGrdSubSectn = oForm.ConvertXmlNodeListToDataTable(xmlsubChild);
                                    grdQuestions.DataSource = dtGrdSubSectn;

                                    grdQuestions.DataBind();
                                    // Retrieving scores from the xml to the TextBox.....
                                    int count = 0;
                                    foreach (GridViewRow grQtns in grdQuestions.Rows)
                                    {
                                        HiddenField hfSubSec = (HiddenField)grQtns.FindControl("hfSubSectn");
                                        hfSubSec.Value = _subsectn;
                                        HiddenField hf_Sect = (HiddenField)grQtns.FindControl("hfSectn");
                                        hf_Sect.Value = _section;
                                        HiddenField hf_SectSkill = (HiddenField)grQtns.FindControl("hfSkill");
                                        hf_SectSkill.Value = _skill;
                                        XmlNodeList xmlSubchild_Temp = xmlsubChild;
                                        TextBox txtPoint = (TextBox)grQtns.FindControl("txt_Question");

                                        CheckBox chkQuesApp = (CheckBox)grQtns.FindControl("chkQuesAppl");
                                        if (xmlSubchild_Temp.Item(count).Attributes["ContainsQuestions"] != null)
                                        {
                                            if (xmlSubchild_Temp.Item(count).Attributes["ContainsQuestions"].Value == "False")
                                                txtPoint.Text = xmlSubchild_Temp.Item(count).InnerText;
                                        }
                                        
                                        if (xmlSubchild_Temp.Item(count).Attributes.Item(4).Name == "Applicable")
                                        {
                                            if (xmlSubchild_Temp.Item(count).Attributes["Applicable"].Value == "false")
                                                chkQuesApp.Checked = true;
                                        }
                                        else
                                            chkQuesApp.Visible = false;
                                        count++;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                Msg.InnerHtml = "No Data Found !!!!";
                //btn_Save.Visible = false;
                //btn_Submit.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void dl_Sections_ItemCommand(object source, DataListCommandEventArgs e)
    {

    }

    protected void grd_Questions_RowDataBound(object source, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ClassDatatable oDt = new ClassDatatable();
                foreach (XmlNode xsubChild in xmlsubChild)
                {
                    string question = e.Row.Cells[1].Text.Replace("&#39;", "'");
                    if (xsubChild.Attributes["name"].Value == question)
                    {
                        if (xsubChild.Attributes["ContainsQuestions"] != null)
                        {
                            if (xsubChild.Attributes["ContainsQuestions"].Value == "True")      //If the current node contains Sub Nodes,
                            //then makes the text bold,gives underline
                            {

                                e.Row.Cells[1].Font.Bold = true;
                                e.Row.Cells[1].Font.Underline = true;
                                e.Row.Cells[2].Visible = false;


                                GridView GridSubQues = (GridView)e.Row.FindControl("grd_SubQuestion");
                                DataTable dtSubQues = new DataTable();
                                dtSubQues = oForm.ConvertXmlNodeListToDataTable(xsubChild.ChildNodes);

                                GridSubQues.DataSource = dtSubQues;
                                GridSubQues.DataBind();
                                // Retrieving scores from the xml to the TextBox.....
                                int count = 0;
                                foreach (GridViewRow grSubQtns in GridSubQues.Rows)
                                {
                                    XmlNodeList xmlSubQtns_Temp = xsubChild.ChildNodes;
                                    TextBox txtPoint = (TextBox)grSubQtns.FindControl("txt_SubQuestion");

                                    CheckBox chkSubQuesApp = (CheckBox)grSubQtns.FindControl("chkSubQuesAppl");
                                    CheckBox chkChoice = (CheckBox)grSubQtns.FindControl("chkChoice");
                                    if (xsubChild.Attributes["Type"] == null)   //If the SubQuestion does not have a Type Attribute..So by default it have SubQuestions.....
                                    {
                                        chkChoice.Visible = false;
                                        txtPoint.Text = xmlSubQtns_Temp.Item(count).InnerText;
                                        if (xmlSubQtns_Temp.Item(count).Attributes["Applicable"].Value == "false")
                                            chkSubQuesApp.Checked = true;
                                    }
                                    else
                                    {                                   //If the SubQuestion have a Type Attribute......
                                        chkChoice.Visible = true;
                                        txtPoint.Visible = false;
                                        chkSubQuesApp.Visible = false;
                                        e.Row.Cells[2].Visible = true;
                                        TextBox txtQtnPnt = (TextBox)e.Row.FindControl("txt_Question");
                                        if (xsubChild.Attributes["Score"] != null)
                                            if (count.ToString() == xsubChild.Attributes["Score"].Value)
                                            {
                                                chkChoice.Checked = true;
                                                txtQtnPnt.Enabled = false;
                                                txtQtnPnt.Text = count.ToString();
                                            }
                                    }
                                    count++;
                                }
                                //
                            }
                            else
                            {
                                //e.Row.Cells[0].Visible = false;                 //If it does not have any child nodes ,then makes the plus image invisible
                                GridView grdParnt = (GridView)e.Row.NamingContainer;
                                grdParnt.Columns[0].Visible = false;
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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void dl_SubSections_ItemCommand(object source, DataListCommandEventArgs e)
    {

    }
    /// <summary>
    /// Saving the xml file with the score into the database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SaveAssess(int status, string updqry)
    {
        try
        {

            xmldoc = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
            xmldocnodes = xmldoc.GetElementsByTagName("Section");
            XmlDocument xmlAssess = new XmlDocument();
            //xmlnew.Load(Server.MapPath("~/Assessmnts/" + Session["xmlname"] + ".xml"));
            xmlAssess = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
            foreach (DataListItem di in dl_Sections.Items)
            {
                LinkButton lbSection = (LinkButton)di.FindControl("lb_Section");
                GridView grdSection = (GridView)di.FindControl("grd_SubSections");
                DataList dlQues = (DataList)di.FindControl("dl_SubSections");
                for (int index = 0; index < xmldocnodes.Count; index++)
                {
                    if (lbSection.Text == xmldocnodes.Item(index).Attributes["name"].Value)
                    {
                        xmlsubsections = xmldocnodes.Item(index).ChildNodes;
                    }
                }




                for (int index1 = 0; index1 < xmlsubsections.Count; index1++)
                {
                    XmlNodeList xmlsubsection = xmlsubsections.Item(index1).ChildNodes;
                    int temp = 0;
                    int sum = 0;
                    for (int index2 = 0; index2 < xmlsubsection.Count; index2++)
                    {

                        if (xmlsubsection.Item(index2).Attributes["ContainsQuestions"].Value == "False")
                        {
                            if (grdSection.Visible == true)
                            {
                                TextBox txtSectn = (TextBox)grdSection.Rows[temp].FindControl("txt_Section");
                                xmlsubsection.Item(index2).InnerText = txtSectn.Text;
                                CheckBox chkSectApp = (CheckBox)grdSection.Rows[temp].FindControl("chkSectAppl");
                                if (chkSectApp.Checked == false) txtCollctn.Add(txtSectn);

                                if (txtSectn.Text.Length > 0)
                                {
                                    xmlsubsection.Item(index2).Attributes["Applicable"].Value = "true";
                                    chkSectApp.Checked = false;
                                }
                                if (chkSectApp.Checked == false)
                                    xmlsubsection.Item(index2).Attributes["Applicable"].Value = "true";
                                if (chkSectApp.Checked == true)
                                    xmlsubsection.Item(index2).Attributes["Applicable"].Value = "false";
                                temp++;
                                //if (txtSectn.Text == "")
                                //    txtSectn.Text = "0";
                                //sum = sum + Convert.ToInt32(txtSectn.Text);
                            }

                        }
                        else
                        {
                            int quessum = 0;
                            XmlNodeList xmlQues = xmlsubsection.Item(index2).ChildNodes;
                            for (int index3 = 0; index3 < xmlQues.Count; index3++)
                            {
                                XmlNodeList xmlQuestns = xmlQues.Item(index3).ChildNodes;
                                if (dlQues.Visible == true)
                                {
                                    foreach (DataListItem dliQues in dlQues.Items)
                                    {
                                        LinkButton lbQuestn = (LinkButton)dliQues.FindControl("lb_SubSection");
                                        if (lbQuestn.Text == xmlsubsection.Item(index2).Attributes["name"].Value)
                                        {
                                            GridView grdQuestn = (GridView)dliQues.FindControl("grd_Questions");
                                            if (grdQuestn.Visible == true)
                                            {
                                                for (int index4 = 0; index4 < xmlQuestns.Count; index4++)
                                                {
                                                    if (xmlQuestns.Item(index4).Attributes["ContainsQuestions"].Value == "True")
                                                    {
                                                        XmlNodeList xmlSubQues = xmlQuestns.Item(index4).ChildNodes;
                                                        GridView grdSubQtn = (GridView)grdQuestn.Rows[index4].FindControl("grd_SubQuestion");
                                                        for (int index5 = 0; index5 < xmlSubQues.Count; index5++)
                                                        {
                                                            TextBox txtSubQtn = (TextBox)grdSubQtn.Rows[index5].FindControl("txt_SubQuestion");

                                                            CheckBox chkSubQuesApp = (CheckBox)grdSubQtn.Rows[index5].FindControl("chkSubQuesAppl");
                                                            CheckBox chkChoice = (CheckBox)grdSubQtn.Rows[index5].FindControl("chkChoice");
                                                            if (xmlQuestns.Item(index4).Attributes["Type"] == null)
                                                            {
                                                                chkChoice.Visible = false;
                                                                xmlSubQues.Item(index5).InnerText = txtSubQtn.Text;
                                                                if (chkSubQuesApp.Checked == false) txtCollctn.Add(txtSubQtn);
                                                                if (txtSubQtn.Text.Length > 0)
                                                                {
                                                                    xmlSubQues.Item(index5).Attributes["Applicable"].Value = "true";
                                                                    chkSubQuesApp.Checked = false;
                                                                }
                                                                if (chkSubQuesApp.Checked == false)
                                                                    xmlSubQues.Item(index5).Attributes["Applicable"].Value = "true";
                                                                if (chkSubQuesApp.Checked == true)
                                                                    xmlSubQues.Item(index5).Attributes["Applicable"].Value = "false";
                                                            }
                                                            else
                                                            {
                                                                chkChoice.Visible = true;
                                                                txtSubQtn.Visible = false;
                                                                chkSubQuesApp.Visible = false;
                                                                TextBox txtQtn = (TextBox)grdQuestn.Rows[index4].FindControl("txt_Question");
                                                                CheckBox chkQuesApp = (CheckBox)grdQuestn.Rows[index4].FindControl("chkQuesAppl");
                                                                if (chkChoice.Checked == true)
                                                                {
                                                                    XmlAttribute newAttrScore = xmldoc.CreateAttribute("Score");
                                                                    newAttrScore.Value = index5.ToString();
                                                                    xmlQuestns.Item(index4).Attributes.Append(newAttrScore);
                                                                    txtQtn.Text = index5.ToString();
                                                                    if (chkQuesApp.Checked == false) txtCollctn.Add(txtQtn);
                                                                }

                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        TextBox txtQtn = (TextBox)grdQuestn.Rows[index4].FindControl("txt_Question");
                                                        xmlQuestns.Item(index4).InnerText = txtQtn.Text;
                                                        CheckBox chkQuesApp = (CheckBox)grdQuestn.Rows[index4].FindControl("chkQuesAppl");
                                                        if (chkQuesApp.Checked == false) txtCollctn.Add(txtQtn);

                                                        if (txtQtn.Text.Length > 0)
                                                        {
                                                            xmlQuestns.Item(index4).Attributes["Applicable"].Value = "true";
                                                            chkQuesApp.Checked = false;
                                                        }
                                                        if (chkQuesApp.Checked == false)
                                                            xmlQuestns.Item(index4).Attributes["Applicable"].Value = "true";
                                                        if (chkQuesApp.Checked == true)
                                                            xmlQuestns.Item(index4).Attributes["Applicable"].Value = "false";

                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            //Convert xml file to byte array
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Successfully Added');", true);
            MemoryStream ms = new MemoryStream();
            xmldoc.Save(ms);
            byte[] bytes = ms.ToArray();

            ClassStudntAssess oStAssess = new ClassStudntAssess();
            oStAssess.BlobData = bytes;
            oStAssess.StudAssessName = txt_AssmntName.Text;
            oSession = (clsSession)Session["UserSession"];
            if (txtNote.Text.Length > 0)
            {
                oStAssess.Note = lblNote.Text + "\n" + "[" + txtNote.Text + ", " + oSession.UserName + " " + DateTime.Now.ToShortDateString() + "]";
                //string updQry = "UPDATE StdtAsmnt SET StdtAsmntXML=@XML,StdtAsmntName=@AsmntName,AsmntStatusId=@Status,ModifiedOn=@ModDate,ModifiedBy=@ModId WHERE StdtAsmntId=@AssessID";
                lblNote.Text = oStAssess.Note;
            }
            else
            {
                oStAssess.Note = lblNote.Text;
            }
            //findQtns();
            oStAssess.Save(updqry, Convert.ToInt32(Session["xmlname"]), oSession.LoginId, oSession.SchoolId, status);

            DataClass oData = new DataClass();
            lblStatus.Text = oData.ExecuteScalarString("SELECT LookupName FROM LookUp WHERE LookUpId=" + status);
            txtNote.Text = "";
            lblNote.Text = oData.ExecuteScalarString("SELECT Note FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
            if (lblNote.Text.Trim() != "")
                notepnl.Visible = true;
            lbl_Msg.InnerHtml = clsGeneral.sucessMsg("Succesfully Saved");
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (hfCheckError.Value == "0")
            {
                DataClass oData = new DataClass();
                int status = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='Assessment Status' AND LookupName='In Progress'");
                string upd = "UPDATE Assessment SET AsmntXML=@XML,AsmntTemplateName=@AsmntName,AsmntStatusId=@Status,ModifiedOn=(SELECT convert(varchar, getdate(), 100)),ModifiedBy=@ModId,Note=@Note WHERE AsmntId=@AssessID";
                SaveAssess(status, upd);
                //findQtns();
                Session["Asmnt_ModDate"] = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + Session["xmlname"].ToString());   //Select the last modified date before submitting.....
                
            }
            else if (hfCheckError.Value != "")
            {
                string msg = hfCheckError.Value + " Invalid Score!!!";
                //ScriptManager.RegisterStartupScript(this, this.GetType(),"alert","alert("+msg+");", true);
                lbl_Msg.InnerHtml = clsGeneral.failedMsg(msg);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    /// <summary>
    /// It finds all the questions save into a datatable
    /// </summary>
    protected void findQtns()
    {
        try
        {
            InitDatatable();
            ClassDatatable oDt = new ClassDatatable();
            DataClass oData = new DataClass();
            foreach (TextBox txtElement in txtCollctn)
            {
                GridViewRow grRow = (GridViewRow)txtElement.NamingContainer;
                HiddenField hfBox = (HiddenField)grRow.FindControl("hfBoxSect");
                //if (txtElement.Text == hfBox.Value)
                //{
                //    //if the score is maximum, no need to store it to the database...
                //}
                //else
                //{
                HiddenField hf_Sectn = (HiddenField)grRow.FindControl("hfSectn");
                HiddenField hf_SubSectn = (HiddenField)grRow.FindControl("hfSubSectn");
                HiddenField hf_Qtn = (HiddenField)grRow.FindControl("hfQtn");
                HiddenField hf_Skill = (HiddenField)grRow.FindControl("hfSkill");
                string AsmntName = oData.ExecuteScalarString("SELECT AsmntName FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["xmlname"]));
                if (hf_SubSectn.Value == null)
                    hf_SubSectn.Value = "NULL-SUB-CAT";
                string selLPid = "SELECT Rel.LessonPlanId as LPid,ISNULL(SortOrder,0) SortOrder FROM AsmntLPRel Rel " +
                                 "WHERE REPLACE(Rel.AsmntCat,'_',' ')='" + hf_Sectn.Value.Replace("_", " ") + "' AND REPLACE(Rel.AsmntName,'_',' ')='" + AsmntName.Replace("_", " ") + "' AND " +
                                 "ISNULL(REPLACE(Rel.AsmntSubCat,'_',' '),'')='" + hf_SubSectn.Value.Replace("_", " ") + "' AND " +
                                 "REPLACE(Rel.AsmntQId,'_',' ')='" + hf_Qtn.Value.Replace("_", " ") + "' ";

                DataTable LP_id = oData.fillData(selLPid);
                for (int LPcount = 0; LPcount < LP_id.Rows.Count; LPcount++)
                {
                    oSession = (clsSession)Session["UserSession"];
                    string ins = "";
                    int sortorder = 0;
                    if (LP_id.Rows[LPcount]["SortOrder"] != null)
                    {
                        if (LP_id.Rows[LPcount]["SortOrder"].ToString().Trim() != "")
                        {
                            sortorder = Convert.ToInt32(LP_id.Rows[LPcount]["SortOrder"]);
                        }
                    }

                    if (LP_id.Rows[LPcount]["LPid"].ToString() == "")
                        LP_id.Rows[LPcount]["LPid"] = "0";
                    if (hf_SubSectn.Value.Length > 0)
                    {
                        ins = "INSERT INTO StdtLPStg(SchoolId,StudentId,GoalName,AssessmentId,AsmntName,AsmntCat,AsmntSubCat,AsmntQId,AsmntScr,TotScr,LessonPlanId,SortOrder,CreatedBy,CreatedOn) " +
                                   "VALUES(" + oSession.SchoolId + "," + Convert.ToInt32(oSession.StudentId.ToString()) + ",'" + hf_Skill.Value.Replace("_", " ") + "'," + Session["xmlname"].ToString() + "," +
                                   "'" + AsmntName.Replace("_", " ") + "','" + hf_Sectn.Value.Replace("_", " ") + "','" + hf_SubSectn.Value.Replace("_", " ") + "','" + hf_Qtn.Value.Replace("_", " ") + "','" + txtElement.Text + "','" + hfBox.Value + "'," + LP_id.Rows[LPcount]["LPid"].ToString() + "," + sortorder + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                                   "SELECT SCOPE_IDENTITY();";
                    }
                    else
                    {

                        ins = "INSERT INTO StdtLPStg(SchoolId,StudentId,GoalName,AssessmentId,AsmntName,AsmntCat,AsmntQId,AsmntScr,TotScr,LessonPlanId,SortOrder,CreatedBy,CreatedOn) " +
                                   "VALUES(" + oSession.SchoolId + "," + Convert.ToInt32(oSession.StudentId.ToString()) + ",'" + hf_Skill.Value.Replace("_", " ") + "'," + Session["xmlname"].ToString() + "," +
                                   "'" + AsmntName.Replace("_", " ") + "','" + hf_Sectn.Value.Replace("_", " ") + "','" + hf_Qtn.Value.Replace("_", " ") + "','" + txtElement.Text + "','" + hfBox.Value + "'," + LP_id.Rows[LPcount]["LPid"].ToString() + "," + sortorder + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                                   "SELECT SCOPE_IDENTITY();";
                    }
                    int LPstgId = oData.ExecuteScalar(ins);
                }
                //_dtTest = oDt.CreateAssessmntsTable(new string[] { "Assessment", "Skill", "Cat", "SubCat", "Qtn", "Score" }, _dtTest, new string[] { AsmntName, hf_Skill.Value, hf_Sectn.Value, hf_SubSectn.Value, hf_Qtn.Value, txtElement.Text });
                //}
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void txt_Section_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btn_Home_Click(object sender, EventArgs e)
    {
        Response.Redirect("Assessments.aspx");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hfConfirm.Value == "true")
            {
                DataClass oData = new DataClass();
                string mod_date = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + Session["xmlname"].ToString());   //Select the last modified date before submitting.....
                //Check whether the modified date before and after editing is same or not, to check whether any other user made any changes in this assessment
                if (mod_date == Session["Asmnt_ModDate"].ToString())//if this condition is true, then no other uses made any changes..so save it
                {
                    int status = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='Assessment Status' AND LookupName='Complete'");
                    string upd = "UPDATE Assessment SET AsmntXML=@XML,AsmntTemplateName=@AsmntName,AsmntStatusId=@Status,AsmntEndTs=(SELECT convert(varchar, getdate(), 100)),ModifiedOn=(SELECT convert(varchar, getdate(), 100)),ModifiedBy=@ModId,Note=@Note WHERE AsmntId=@AssessID";
                    SaveAssess(status, upd);

                    findQtns();
                    Response.Redirect("ReviewAssessmnt.aspx");
                }
                else
                {//otherwise, another user made changes in this assessmnt..so updation not possible.
                    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('Submission not possible because another user made some changes in this Assessment');", true);
                }
            }
            else
            {
                return;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }
    protected void dl_Sections_ItemDataBound(object sender, DataListItemEventArgs e)
    {

    }
    protected void InitDatatable()
    {
        //_dtTest = new DataTable();
        //ClassDatatable oDt = new ClassDatatable();
        //_dtTest = oDt.CreateColumn("Assessment", _dtTest);
        //_dtTest = oDt.CreateColumn("Skill", _dtTest);
        //_dtTest = oDt.CreateColumn("Cat", _dtTest);
        //_dtTest = oDt.CreateColumn("SubCat", _dtTest);
        //_dtTest = oDt.CreateColumn("Qtn", _dtTest);
        //_dtTest = oDt.CreateColumn("Score", _dtTest);
    }

    [WebMethod]
    public static bool IsValidQtn(int box, int score)
    {
        bool status = false;
        if (score > box)
        {
            status = true;
        }
        return status;
    }

    protected void dl_SubSections_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

}