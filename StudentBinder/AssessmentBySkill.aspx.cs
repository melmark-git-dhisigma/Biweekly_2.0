using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Data;
using System.Web.Services;

public partial class Administration_GoalAssess : System.Web.UI.Page
{
    XmlDocument oDocument = new XmlDocument();
    XmlNodeList xSubNodes;
    XmlNodeList xSubSectns;
    XmlNodeList xSubSect = null;
    XmlNodeList xQtns = null;
    public int rowCount = 0;
    //public static int _studAssessID = 0;
    clsSession oSession = null;
    //public static DataTable _dtTest;
    List<TextBox> txtCollctn = new List<TextBox>();

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
            Session["AssmtID"] = 0;
            bool Disable = false;
            //clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
            if (Request.QueryString["Skill"] != null)
                Disable = Convert.ToBoolean(Request.QueryString["Skill"]);
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


            LoadData();


            if (dl_Assessmnts.Items.Count == 0)
            {
                Msg.InnerHtml = "No Data Found !!!!";
                //btn_Submit.Visible = false;
                //btn_Save.Visible = false;
            }
            else
            {
                Msg.Visible = false;
            }

        }
    }
    protected void LoadData()
    {
        try
        {
            btnSave.Style.Add("display", "none");//
            btnSubmit.Style.Add("display", "none"); // To make the button invisible, and also to make it accessible in javascript, we need to add this style.....
            lbl_Goal.Text = "Assessment Sheet for " + Session["goalname"].ToString();
            //XML Document Creation based on the Selected Skill

            XmlDeclaration oDeclaration = oDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement rootElment = oDocument.CreateElement("Assessments");


            oDocument.InsertBefore(oDeclaration, oDocument.DocumentElement);
            oDocument.AppendChild(rootElment);
            DataClass oData = new DataClass();
            ClassGoal oGoal = new ClassGoal();
            //DataSet dsAssess = oGoal.SelectAssessmnts(Session["goalname"].ToString());
            DataTable dsAssess = oData.fillData("SELECT distinct Gl.GoalName,Asmnt.AsmntId,Asmnt.AsmntName,Asmnt.AsmntXML FROM (AsmntGoalRel Rel INNER JOIN Goal Gl ON Gl.GoalId=Rel.GoalId) " +
                                                 "INNER JOIN Assessment Asmnt " +
                                                 "ON Asmnt.AsmntId=Rel.AsmntId " +
                                                 "WHERE Gl.GoalName='" + Session["goalname"].ToString() + "' AND Asmnt.ActiveInd='A' AND ISNULL(Asmnt.StudentId,'')=''");
            dl_Assessmnts.DataSource = dsAssess;
            dl_Assessmnts.DataBind();
            if (dl_Assessmnts.Items.Count > 0)
            {
                int index = 0;
                foreach (DataListItem di_Assmnt in dl_Assessmnts.Items)
                {

                    ClassAssess oAssess = new ClassAssess();
                    //byte[] blobData = oAssess.SelectBlobData(Convert.ToInt32(dsAssess.Tables[0].Rows[i]["AsmntTemplateID"].ToString()));

                    XmlDocument xAssmnts = new XmlDocument();
                    if (Session["Mode"].ToString() == "New")    //If the assessment is newly created one....
                    {
                        Session["AssmtID"] = 0;
                        txt_AssmntName.Text = Session["goalname"].ToString() + " - " + DateTime.Now.ToShortDateString();

                        xAssmnts = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(dsAssess.Rows[index]["AsmntID"].ToString()));
                        xSubNodes = xAssmnts.SelectNodes("//Section");
                        ClassFormAssess oForm = new ClassFormAssess();
                        DataTable dtSection = new DataTable();
                        dtSection = oGoal.ConvertXmlNodeListToDataTable(xSubNodes, Session["goalname"].ToString());

                        DataList dlSection = (DataList)di_Assmnt.FindControl("dl_Sections");
                        dlSection.DataSource = dtSection;
                        dlSection.DataBind();

                        XmlElement xmlParentNode = oDocument.CreateElement("Assessment");
                        xmlParentNode.SetAttribute("name", dsAssess.Rows[index]["AsmntName"].ToString());
                        oDocument.DocumentElement.PrependChild(xmlParentNode);

                        foreach (XmlNode xNodeSection in xSubNodes)
                        {
                            if (xNodeSection.Attributes.Count > 1)
                            {
                                if (xNodeSection.Attributes["Skill"].Value == Session["goalname"].ToString())
                                {
                                    xmlParentNode.AppendChild(oDocument.ImportNode(xNodeSection, true));
                                }
                            }
                        }
                    }
                    else
                    {           //if it is for editing purposes....
                        oData = new DataClass();
                        Session["AssmtID"] = Convert.ToInt32(Session["stdtAsmntID"]);
                        if (Convert.ToInt32(Session["AssmtID"].ToString()) > 0)
                            lblStatus.Text = oData.ExecuteScalarString("SELECT LookupName FROM LookUp Look INNER JOIN Assessment Asmnt ON LookUpId=AsmntStatusId WHERE AsmntId=" + Convert.ToInt32(Session["AssmtID"].ToString()));
                        oSession = (clsSession)Session["UserSession"];
                        txt_AssmntName.Text = oData.ExecuteScalarString("SELECT AsmntTemplateName FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["stdtAsmntID"]));
                        lblNote.Text = oData.ExecuteScalarString("SELECT Note FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["stdtAsmntID"]));
                        if (lblNote.Text.Trim() != "")
                            pnlnote.Visible = true;
                        if (oSession != null)
                            lblYear.Text = oData.ExecuteScalarString("SELECT AsmntYearCode FROM AsmntYear Yr INNER JOIN Assessment Asmnt ON Asmnt.AsmntYearId= Yr.AsmntYearId WHERE Asmnt.AsmntID=" + Convert.ToInt32(Session["AssmtID"].ToString()) + "");
                        //txtNote.Text = oData.ExecuteScalarString("SELECT Note FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["stdtAsmntID"]));
                        ClassFormAssess oForm = new ClassFormAssess();
                        xAssmnts = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["stdtAsmntID"]));
                        string path = "//Assessments/Assessment[@name='" + dsAssess.Rows[index]["AsmntName"].ToString() + "']/Section";
                        xSubNodes = xAssmnts.SelectNodes("//Assessments/Assessment[@name='" + dsAssess.Rows[index]["AsmntName"].ToString() + "']/Section");
                        //ClassFormAssess oForm = new ClassFormAssess();
                        DataTable dtSection = new DataTable();
                        dtSection = oForm.ConvertXmlNodeListToDataTable(xSubNodes);

                        DataList dlSection = (DataList)di_Assmnt.FindControl("dl_Sections");
                        dlSection.DataSource = dtSection;
                        dlSection.DataBind();
                    }




                    index++;

                }   //XML Creation Process ENDS   

                // Converts the XML file into bytes array, if it is a newly created one and pass the data into the class file 'ClassStudntAssess'
                if (Session["Mode"].ToString() == "New" & Convert.ToInt32(Session["AssmtID"].ToString()) == 0)
                {
                    MemoryStream msAssess = new MemoryStream();
                    oDocument.Save(msAssess);
                    byte[] blobAssess = msAssess.ToArray();

                    //DataClass oData = new DataClass();
                    int YearID = oData.ExecuteScalar("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
                    ClassStudntAssess oStAssess = new ClassStudntAssess();
                    oSession = (clsSession)Session["UserSession"];
                    if (oSession != null)
                        oStAssess.StudID = Convert.ToInt32(oSession.StudentId.ToString());
                    oStAssess.YearID = YearID;
                    oStAssess.StudAssessName = txt_AssmntName.Text;
                    oStAssess.AssessTempName = Session["goalname"].ToString();
                    oStAssess.AssessType = "By Skill";
                    oStAssess.BlobData = blobAssess;
                    oSession = (clsSession)Session["UserSession"];

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
                    Session["AssmtID"] = oStAssess.Save(ins, 0, oSession.LoginId, oSession.SchoolId, status);

                    Session["Asmnt_ModDate"] = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["AssmtID"].ToString()));   //Add the last modified date into a session before editing.....
                    oData = new DataClass();
                    if (Convert.ToInt32(Session["AssmtID"].ToString()) > 0)
                        lblStatus.Text = oData.ExecuteScalarString("SELECT LookupName FROM LookUp Look INNER JOIN Assessment Asmnt ON LookUpId=AsmntStatusId WHERE AsmntId=" + Convert.ToInt32(Session["AssmtID"].ToString()));
                    if (oSession != null)
                        lblYear.Text = oData.ExecuteScalarString("SELECT AsmntYearCode FROM AsmntYear Yr INNER JOIN Assessment Asmnt ON Asmnt.AsmntYearId= Yr.AsmntYearId WHERE Asmnt.AsmntID=" + Convert.ToInt32(Session["AssmtID"].ToString()) + "");
                }
                //oDocument.Save(Server.MapPath("~/Assessmnts/Output.xml"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void dl_Assessmnts_ItemDataBound(object sender, DataListItemEventArgs e)
    {

    }
    /// <summary>
    /// During each databound in dl_Sections, bind the subnodes of the current item into gridview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dl_Sections_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            ClassGoal oGoal = new ClassGoal();
            GridView grdSubSect = (GridView)e.Item.FindControl("grd_SubSection");
            LinkButton lbSection = (LinkButton)e.Item.FindControl("lb_Section");
            for (int nodeCount = 0; nodeCount < xSubNodes.Count; nodeCount++)
            {
                if (lbSection.Text == xSubNodes.Item(nodeCount).Attributes["name"].Value)   //Bind the childnodes of current node only if it matches
                {
                    xSubSectns = xSubNodes.Item(nodeCount).ChildNodes;
                    DataTable dtSubSection = oGoal.ConvertXmlNodeListToDataTable(xSubSectns.Item(0).ChildNodes);
                    XmlNodeList xmlSubSect_Temp = xSubSectns.Item(0).ChildNodes;
                    grdSubSect.DataSource = dtSubSection;
                    grdSubSect.DataBind();

                    // Retrieving scores from the xml to the TextBox.....
                    int count = 0, countB = 0, check = 0;
                    foreach (GridViewRow grSubSect in grdSubSect.Rows)
                    {
                        CheckBox chk_SectApp = (CheckBox)grSubSect.FindControl("chkSectAppl");
                        TextBox txtPoint = (TextBox)grSubSect.FindControl("txtSectPoint");
                        if (xmlSubSect_Temp.Item(count).Attributes["ContainsQuestions"] != null)
                        {
                            if (xmlSubSect_Temp.Item(count).Attributes["ContainsQuestions"].Value == "False")
                            {
                                if (xmlSubSect_Temp.Item(count).Attributes["Applicable"].Value == "false")
                                    chk_SectApp.Checked = true;
                                txtPoint.Text = xmlSubSect_Temp.Item(count).InnerText;
                                count++;
                                check = 0;
                            }
                            else
                            {
                                if (check == 0)
                                {
                                    check++;
                                    continue;
                                }
                                if (xmlSubSect_Temp.Item(count).ChildNodes.Count > 0)
                                {
                                    XmlNodeList xmlSub_Temp = xmlSubSect_Temp.Item(count).ChildNodes.Item(0).ChildNodes;
                                    txtPoint.Text = xmlSub_Temp.Item(countB).InnerText;
                                    if (xmlSub_Temp.Item(countB).Attributes["ContainsQuestions"] != null)
                                    {
                                        if (xmlSub_Temp.Item(countB).Attributes["ContainsQuestions"].Value == "False")
                                        {
                                            if (xmlSub_Temp.Item(countB).Attributes["Applicable"].Value == "false")
                                                chk_SectApp.Checked = true;
                                        }
                                    }
                                    countB++;
                                    if (xmlSub_Temp.Count == countB)
                                    {
                                        count++;
                                        countB = 0; check = 0;
                                    }
                                }
                            }
                        }
                    }
                    //
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void nestedGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grd_SubSection_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Header) //if the cuurent row of the gridview is a header row
            {
                rowCount = 0;
                xSubSect = xSubSectns.Item(0).ChildNodes;
            }
            else
            {
                GridView grd_SubQtn = (GridView)e.Row.FindControl("nestedGridView");
                if (grd_SubQtn != null)
                {                       //
                    if (grd_SubQtn.Rows.Count == 0)             //if the grid is not null and it does not have any data,
                    {
                        //e.Row.Cells[0].Visible = false;         //then makes the plus image corresponding to that row to invisible
                        GridView grdParnt = (GridView)e.Row.NamingContainer;
                        grdParnt.Columns[0].Visible = false;
                        grdParnt.Columns[5].Visible = false;
                    }
                }

                if (rowCount < xSubSect.Count)
                {
                    if (xSubSect.Item(rowCount).Attributes["ContainsQuestions"] != null)
                    {
                        if (xSubSect.Item(rowCount).Attributes["ContainsQuestions"].Value == "True")    //If the current node contains Sub Nodes,
                        //then makes the text bold,gives underline
                        {
                            if (xSubSect.Item(rowCount).Attributes["name"].Value == e.Row.Cells[1].Text)
                            {
                                //e.Row.Width = 100;
                                e.Row.Font.Bold = true;
                                e.Row.Font.Underline = true;
                                //e.Row.Cells[2].Visible = false;
                                e.Row.Cells[2].Text = "";
                                e.Row.Height = 25;

                                CheckBox chk_SectApp = (CheckBox)e.Row.FindControl("chkSectAppl");
                                chk_SectApp.Visible = false;

                                xQtns = xSubSect.Item(rowCount).ChildNodes;
                                rowCount++;
                            }
                            else
                            {

                                XmlNodeList xQues = xQtns.Item(0).ChildNodes;
                                foreach (XmlNode xQtn in xQues)
                                {
                                    if (xQtn.Attributes["ContainsQuestions"] != null)
                                    {
                                        if (xQtn.Attributes["ContainsQuestions"].Value == "True")   //If the current node contains Sub Nodes,
                                        //then fill the subnodes into the gridview
                                        {
                                            //e.Row.Cells[0].Visible = false;
                                            //e.Row.Cells[2].Visible = false;
                                            e.Row.Cells[2].Text = "";
                                            ClassFormAssess oForm = new ClassFormAssess();
                                            GridView grdSubQuestn = (GridView)e.Row.FindControl("nestedGridView");
                                            XmlNodeList xSubQues = xQtn.ChildNodes;
                                            DataTable dtSubQtn = oForm.ConvertXmlNodeListToDataTable(xSubQues);

                                            grdSubQuestn.DataSource = dtSubQtn;
                                            grdSubQuestn.DataBind();

                                            if (grdSubQuestn.Rows.Count > 0)
                                            {
                                                CheckBox chk_SectApp = (CheckBox)e.Row.FindControl("chkSectAppl");
                                                chk_SectApp.Visible = false;
                                            }

                                            // Retrieving scores from the xml to the TextBox.....
                                            int count = 0;
                                            foreach (GridViewRow grSubQtns in grdSubQuestn.Rows)
                                            {
                                                XmlNodeList xmlSubQtns_Temp = xSubQues;
                                                TextBox txtPoint = (TextBox)grSubQtns.FindControl("txtSubPoint");
                                                CheckBox chk_QuesApp = (CheckBox)grSubQtns.FindControl("chkQuesAppl");

                                                if (xQtn.Attributes["Type"] != null)
                                                {
                                                    CheckBox chkChoice = (CheckBox)grSubQtns.FindControl("chkChoice");
                                                    txtPoint.Visible = false;
                                                    chk_QuesApp.Visible = false;
                                                    TextBox txtSubPnt = (TextBox)e.Row.FindControl("txtSectPoint");
                                                    txtSubPnt.Enabled = false;
                                                    e.Row.Cells[2].Visible = true;
                                                    if (xQtn.Attributes["Score"] != null)
                                                        if (xQtn.Attributes["Score"].Value == count.ToString())
                                                        {
                                                            chkChoice.Checked = true;
                                                            txtSubPnt.Text = count.ToString();
                                                        }
                                                }
                                                else
                                                {
                                                    txtPoint.Text = xmlSubQtns_Temp.Item(count).InnerText;
                                                    if (xmlSubQtns_Temp.Item(count).Attributes["Applicable"].Value == "false")
                                                        chk_QuesApp.Checked = true;
                                                }
                                                count++;
                                            }
                                            //
                                            if (e.Row.Cells[0].Visible == false)
                                                e.Row.Cells[0].Visible = true;
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
                else if (rowCount == xSubSect.Count)
                {

                    rowCount++;

                    XmlNodeList xQues = xQtns.Item(0).ChildNodes;
                    foreach (XmlNode xQtn in xQues)
                    {
                        if (xQtn.Attributes["ContainsQuestions"] != null)
                        {
                            if (xQtn.Attributes["ContainsQuestions"].Value == "True")
                            {
                                e.Row.Cells[2].Visible = false;
                                //e.Row.Cells[0].Visible = false;
                                ClassFormAssess oForm = new ClassFormAssess();
                                GridView grdSubQuestn = (GridView)e.Row.FindControl("nestedGridView");
                                XmlNodeList xSubQues = xQtn.ChildNodes;
                                DataTable dtSubQtn = oForm.ConvertXmlNodeListToDataTable(xSubQues);

                                grdSubQuestn.DataSource = dtSubQtn;
                                grdSubQuestn.DataBind();
                                if (grdSubQuestn.Rows.Count > 0)
                                {
                                    CheckBox chk_SectApp = (CheckBox)e.Row.FindControl("chkSectAppl");
                                    chk_SectApp.Visible = false;
                                }
                                // Retrieving scores from the xml to the TextBox.....
                                int count = 0;
                                foreach (GridViewRow grSubQtns in grdSubQuestn.Rows)
                                {
                                    XmlNodeList xmlSubQtns_Temp = xSubQues;
                                    TextBox txtPoint = (TextBox)grSubQtns.FindControl("txtSubPoint");
                                    CheckBox chk_QuesApp = (CheckBox)grSubQtns.FindControl("chkQuesAppl");
                                    if (xQtn.Attributes["Type"] != null)
                                    {
                                        CheckBox chkChoice = (CheckBox)grSubQtns.FindControl("chkChoice");
                                        txtPoint.Visible = false;
                                        chk_QuesApp.Visible = false;
                                        TextBox txtSubPnt = (TextBox)e.Row.FindControl("txtSectPoint");
                                        txtSubPnt.Enabled = false;
                                        e.Row.Cells[2].Visible = true;
                                        if (xQtn.Attributes["Score"] != null)
                                            if (xQtn.Attributes["Score"].Value == count.ToString())
                                            {
                                                chkChoice.Checked = true;
                                                txtSubPnt.Text = count.ToString();
                                            }
                                    }
                                    else
                                    {
                                        txtPoint.Text = xmlSubQtns_Temp.Item(count).InnerText;
                                        if (xmlSubQtns_Temp.Item(count).Attributes["Applicable"].Value == "false")
                                            chk_QuesApp.Checked = true;
                                    }
                                    count++;
                                }
                                //
                                if (e.Row.Cells[0].Visible == false)
                                    e.Row.Cells[0].Visible = true;
                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    /// <summary>
    /// In this button click, the xml with the scores is saved into the database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SaveAssess(int status, string updqry)
    {
        try
        {
            ClassGoal oGoal = new ClassGoal();
            DataClass odata = new DataClass();
            //DataSet dsAssess = oGoal.SelectAssessmnts(Session["goalname"].ToString());
            DataTable dsAssess = odata.fillData("SELECT Gl.GoalName,Asmnt.AsmntId,Asmnt.AsmntName,Asmnt.AsmntXML FROM (AsmntGoalRel Rel INNER JOIN Goal Gl ON Gl.GoalId=Rel.GoalId) " +
                                                 "INNER JOIN Assessment Asmnt " +
                                                 "ON Asmnt.AsmntId=Rel.AsmntId " +
                                                 "WHERE Gl.GoalName='" + Session["goalname"].ToString() + "' AND Asmnt.ActiveInd='A' AND ISNULL(Asmnt.StudentId,'')=''");
            XmlDocument xGoal = new XmlDocument();
            ClassFormAssess oForm = new ClassFormAssess();
            ClassAssess oAssess = new ClassAssess();

            xGoal = oAssess.LoadXmlfromBlob("SELECT AsmntXML FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["AssmtID"].ToString()));
            //xGoal.Load(Server.MapPath("~/Assessmnts/Result.xml"));
            XmlNodeList xAssessmnts = xGoal.SelectNodes("//Assessment");
            //int i = 0;
            foreach (DataListItem di_Assmnt in dl_Assessmnts.Items)
            {
                LinkButton lbAssessmnt = (LinkButton)di_Assmnt.FindControl("lb_Assess");
                //ClassAssess oAssess = new ClassAssess();

                for (int assessCount = 0; assessCount < xAssessmnts.Count; assessCount++)
                {
                    if (lbAssessmnt.Text == xAssessmnts.Item(assessCount).Attributes["name"].Value)
                    {
                        xSubNodes = xAssessmnts.Item(assessCount).ChildNodes;
                        //ClassFormAssess oForm = new ClassFormAssess();
                        DataTable dtSection = new DataTable();
                        dtSection = oGoal.ConvertXmlNodeListToDataTable(xSubNodes, Session["goalname"].ToString());

                        DataList dlSection = (DataList)di_Assmnt.FindControl("dl_Sections");


                        foreach (DataListItem diSect in dlSection.Items)
                        {
                            GridView grdSubSect = (GridView)diSect.FindControl("grd_SubSection");
                            LinkButton lbSection = (LinkButton)diSect.FindControl("lb_Section");
                            for (int nodeCount = 0; nodeCount < xSubNodes.Count; nodeCount++)
                            {
                                if (lbSection.Text == xSubNodes.Item(nodeCount).Attributes["name"].Value)
                                {
                                    xSubSectns = xSubNodes.Item(nodeCount).ChildNodes;
                                    for (int grdCells = 0; grdCells < grdSubSect.Rows.Count; )
                                    {

                                        XmlNodeList xSubSect = xSubSectns.Item(0).ChildNodes;
                                        foreach (XmlNode xSubSec in xSubSect)
                                        {
                                            if (xSubSec.Attributes["ContainsQuestions"] != null)
                                            {
                                                if (xSubSec.Attributes["ContainsQuestions"].Value == "False")
                                                {
                                                    HiddenField hfAssmnt = (HiddenField)grdSubSect.Rows[grdCells].FindControl("hfAsmnt");
                                                    hfAssmnt.Value = lbAssessmnt.Text;
                                                    HiddenField hfSect = (HiddenField)grdSubSect.Rows[grdCells].FindControl("hfSectn");
                                                    hfSect.Value = lbSection.Text;

                                                    TextBox txtPoint = (TextBox)grdSubSect.Rows[grdCells].Cells[2].FindControl("txtSectPoint");
                                                    CheckBox chk_SecAppl = (CheckBox)grdSubSect.Rows[grdCells].FindControl("chkSectAppl");
                                                    xSubSec.InnerText = txtPoint.Text;
                                                    if (txtPoint.Visible == true)
                                                    {
                                                        if (chk_SecAppl.Checked == false) txtCollctn.Add(txtPoint);
                                                    }
                                                    if (txtPoint.Text.Length > 0)
                                                    {
                                                        xSubSec.Attributes["Applicable"].Value = "true";
                                                        chk_SecAppl.Checked = false;
                                                    }
                                                    if (chk_SecAppl.Checked == false)
                                                        xSubSec.Attributes["Applicable"].Value = "true";
                                                    if (chk_SecAppl.Checked == true)
                                                        xSubSec.Attributes["Applicable"].Value = "false";
                                                    grdCells++;
                                                }

                                                else
                                                {
                                                    grdCells++;
                                                    if (xSubSec.ChildNodes.Count > 0)
                                                    {
                                                        XmlNodeList xQues = xSubSec.ChildNodes.Item(0).ChildNodes;
                                                        int quesCount = 0;
                                                        foreach (XmlNode xQtn in xQues)
                                                        {
                                                            HiddenField hfAssmnt = (HiddenField)grdSubSect.Rows[grdCells].FindControl("hfAsmnt");
                                                            hfAssmnt.Value = lbAssessmnt.Text;
                                                            HiddenField hfSect = (HiddenField)grdSubSect.Rows[grdCells].FindControl("hfSectn");
                                                            hfSect.Value = lbSection.Text;
                                                            HiddenField hfSubSect = (HiddenField)grdSubSect.Rows[grdCells].FindControl("hfSubSectn");
                                                            hfSubSect.Value = xSubSec.Attributes["name"].Value;
                                                            TextBox txtPoint = (TextBox)grdSubSect.Rows[grdCells].Cells[2].FindControl("txtSectPoint");
                                                            CheckBox chk_SecAppl = (CheckBox)grdSubSect.Rows[grdCells].FindControl("chkSectAppl");
                                                            if (xQtn.Attributes["ContainsQuestions"] != null)
                                                            {
                                                                if (xQtn.Attributes["ContainsQuestions"].Value == "False")
                                                                {
                                                                    xQtn.InnerText = txtPoint.Text;
                                                                    if (txtPoint.Visible == true)
                                                                    {
                                                                        if (chk_SecAppl.Checked == false) txtCollctn.Add(txtPoint);
                                                                    }
                                                                    if (txtPoint.Text.Length > 0)
                                                                    {
                                                                        xQtn.Attributes["Applicable"].Value = "true";
                                                                        chk_SecAppl.Checked = false;
                                                                    }
                                                                    if (chk_SecAppl.Checked == false)
                                                                        xQtn.Attributes["Applicable"].Value = "true";
                                                                    if (chk_SecAppl.Checked == true)
                                                                        xQtn.Attributes["Applicable"].Value = "false";
                                                                }
                                                                else
                                                                {

                                                                    GridView grd_SubQtns = (GridView)grdSubSect.Rows[grdCells].Cells[3].FindControl("nestedGridView");
                                                                    XmlNodeList xSubQtns = xQtn.ChildNodes;
                                                                    int grdRowCount = 0;
                                                                    bool notScore = false;
                                                                    foreach (XmlNode xSubQtn in xSubQtns)
                                                                    {
                                                                        //HiddenField hfAssmntSub = (HiddenField)grd_SubQtns.Rows[grdCells].FindControl("hfAsmnt");
                                                                        //hfAssmntSub.Value = lbAssessmnt.Text;
                                                                        //HiddenField hfSectSub = (HiddenField)grd_SubQtns.Rows[grdCells].FindControl("hfSectn");
                                                                        //hfSectSub.Value = lbSection.Text;
                                                                        TextBox txtSubPoint = (TextBox)grd_SubQtns.Rows[grdRowCount].Cells[1].FindControl("txtSubPoint");
                                                                        CheckBox chk_QuesApp = (CheckBox)grd_SubQtns.Rows[grdRowCount].FindControl("chkQuesAppl");
                                                                        CheckBox chkChoice = (CheckBox)grd_SubQtns.Rows[grdRowCount].FindControl("chkChoice");

                                                                        if (chkChoice.Checked == true)
                                                                        {
                                                                            notScore = true;
                                                                            if (xQtn.Attributes["Type"] != null)
                                                                            {
                                                                                XmlAttribute newAttrScore = xGoal.CreateAttribute("Score");
                                                                                newAttrScore.Value = grdRowCount.ToString();
                                                                                xQtn.Attributes.Append(newAttrScore);
                                                                                txtPoint.Text = grdRowCount.ToString();
                                                                            }
                                                                            if (chk_SecAppl.Checked == false) txtCollctn.Add(txtPoint);
                                                                        }

                                                                        xSubQtn.InnerText = txtSubPoint.Text;
                                                                        if (txtSubPoint.Visible == true)
                                                                        {
                                                                            if (chk_QuesApp.Checked == false) txtCollctn.Add(txtSubPoint);
                                                                        }
                                                                        if (txtSubPoint.Text.Length > 0)
                                                                        {
                                                                            xSubQtn.Attributes["Applicable"].Value = "true";
                                                                            chk_QuesApp.Checked = false;
                                                                        }
                                                                        if (chk_QuesApp.Checked == false)
                                                                            xSubQtn.Attributes["Applicable"].Value = "true";
                                                                        if (chk_QuesApp.Checked == true)
                                                                            xSubQtn.Attributes["Applicable"].Value = "false";
                                                                        grdRowCount++;

                                                                    }
                                                                    if (notScore == false)
                                                                    {
                                                                        if (chk_SecAppl.Checked == false) txtCollctn.Add(txtPoint);
                                                                    }
                                                                }
                                                            }
                                                            quesCount++;
                                                            grdCells++;
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        //i++;

                    }
                }
            }
            //Converting the xml file into byte array
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Successfully Added');", true);
            MemoryStream msAssess = new MemoryStream();
            xGoal.Save(msAssess);
            byte[] blobXML = msAssess.ToArray();
            oSession = (clsSession)Session["UserSession"];
            ClassStudntAssess oStAssess = new ClassStudntAssess();
            oStAssess.BlobData = blobXML;
            oStAssess.StudAssessName = txt_AssmntName.Text;
            if (txtNote.Text.Length > 0)
            {
                oStAssess.Note = lblNote.Text + "\n" + "[" + txtNote.Text + ", " + oSession.UserName + " " + DateTime.Now.ToShortDateString() + "]";
                lblNote.Text = oStAssess.Note;
            }
            else
            {
                oStAssess.Note = lblNote.Text;
            }
            //string updQry = "UPDATE StdtAsmnt SET StdtAsmntXML=@XML,StdtAsmntName=@AsmntName,AsmntStatusId=@Status,ModifiedOn=@ModDate,ModifiedBy=@ModId WHERE StdtAsmntId=@AssessID";

            oStAssess.Save(updqry, Convert.ToInt32(Session["AssmtID"].ToString()), oSession.LoginId, oSession.SchoolId, status);
            //findQtns();
            DataClass oData = new DataClass();
            lblStatus.Text = oData.ExecuteScalarString("SELECT LookupName FROM LookUp WHERE LookUpId=" + status);
            lblNote.Text = oData.ExecuteScalarString("SELECT Note FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["AssmtID"].ToString()));
            txtNote.Text = "";
            if (lblNote.Text.Trim() != "")
                pnlnote.Visible = true;
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
            DataClass oData = new DataClass();
            int status = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='Assessment Status' AND LookupName='In Progress'");
            string upd = "UPDATE Assessment SET AsmntXML=@XML,AsmntTemplateName=@AsmntName,AsmntStatusId=@Status,ModifiedOn=(SELECT convert(varchar, getdate(), 100)),ModifiedBy=@ModId,Note=@Note WHERE AsmntId=@AssessID";
            SaveAssess(status, upd);

            Session["Asmnt_ModDate"] = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["AssmtID"].ToString()));   //Select the last modified date before submitting.....
            
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hfConfirm.Value == "true")
            {
                DataClass oData = new DataClass();
                string mod_date = oData.ExecuteScalarString("SELECT ModifiedOn FROM Assessment WHERE AsmntId=" + Convert.ToInt32(Session["AssmtID"].ToString()));   //Select the last modified date before submitting.....
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
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    /// <summary>
    /// 
    /// </summary>
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
    /// <summary>
    /// 
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
                HiddenField hf_Asmnt = (HiddenField)grRow.FindControl("hfAsmnt");
                HiddenField hf_Sectn = (HiddenField)grRow.FindControl("hfSectn");
                HiddenField hf_SubSectn = (HiddenField)grRow.FindControl("hfSubSectn");
                HiddenField hf_Qtn = (HiddenField)grRow.FindControl("hfQtn");
                //string AsmntName = oData.ExecuteScalarString("SELECT AsmntTemplateName FROM StdtAsmnt WHERE StdtAsmntId=" + Convert.ToInt32(Request.QueryString["xmlname"]));

                string selLPid = "SELECT Rel.LessonPlanId as LPid,ISNULL(SortOrder,0) SortOrder FROM AsmntLPRel Rel " +
                                 "WHERE REPLACE(Rel.AsmntCat,'_',' ')='" + hf_Sectn.Value.Replace("_", " ") + "' AND REPLACE(Rel.AsmntName,'_',' ')='" + hf_Asmnt.Value.Replace("_", " ") + "' AND " +
                                 "ISNULL(REPLACE(Rel.AsmntSubCat,'_',' '),'')='" + hf_SubSectn.Value.Replace("_", " ") + "' AND " +
                                 "REPLACE(Rel.AsmntQId,'_',' ')='" + hf_Qtn.Value.Replace("_", " ") + "'";

                DataTable LP_id = oData.fillData(selLPid);
                oSession = (clsSession)Session["UserSession"];
                for (int LPcount = 0; LPcount < LP_id.Rows.Count; LPcount++)
                {
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
                                     "VALUES(" + oSession.SchoolId + "," + Convert.ToInt32(oSession.StudentId.ToString()) + ",'" + Session["goalname"].ToString().Replace("_", " ") + "'," + Convert.ToInt32(Session["AssmtID"].ToString()) + "," +
                                     "'" + hf_Asmnt.Value.Replace("_", " ") + "','" + hf_Sectn.Value.Replace("_", " ") + "','" + hf_SubSectn.Value.Replace("_", " ") + "','" + hf_Qtn.Value.Replace("_", " ") + "','" + txtElement.Text + "','" + hfBox.Value + "'," + LP_id.Rows[LPcount]["LPid"].ToString() + "," + sortorder + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                                     "SELECT SCOPE_IDENTITY();";
                    }
                    else
                    {
                        ins = "INSERT INTO StdtLPStg(SchoolId,StudentId,GoalName,AssessmentId,AsmntName,AsmntCat,AsmntQId,AsmntScr,TotScr,LessonPlanId,SortOrder,CreatedBy,CreatedOn) " +
                                     "VALUES(" + oSession.SchoolId + "," + Convert.ToInt32(oSession.StudentId.ToString()) + ",'" + Session["goalname"].ToString().Replace("_", " ") + "'," + Convert.ToInt32(Session["AssmtID"].ToString()) + "," +
                                     "'" + hf_Asmnt.Value.Replace("_", " ") + "','" + hf_Sectn.Value.Replace("_", " ") + "','" + hf_Qtn.Value.Replace("_", " ") + "','" + txtElement.Text + "','" + hfBox.Value + "'," + LP_id.Rows[LPcount]["LPid"].ToString() + "," + sortorder + "," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))\t\n" +
                                     "SELECT SCOPE_IDENTITY();";
                    }
                    int LPstgId = oData.ExecuteScalar(ins);
                }

                //_dtTest = oDt.CreateAssessmntsTable(new string[] { "Assessment", "Skill", "Cat", "SubCat", "Qtn", "Score" }, _dtTest, new string[] { hf_Asmnt.Value, Session["goalname"].ToString(), hf_Sectn.Value, hf_SubSectn.Value, hf_Qtn.Value, txtElement.Text });
                // }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    //[WebMethod]
    //public static bool IsValidStatus(int box, int score)
    //{       
    //    if (score > box)
    //    {
    //        status = true;
    //    }
    //    return status;
    //}

}