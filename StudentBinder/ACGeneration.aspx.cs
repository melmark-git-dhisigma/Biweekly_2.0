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
using System.Net;




public partial class Administration_ACGeneration : System.Web.UI.Page
{
    static string[] columns;
    static string[] columnsToAdd;
    static string[] placeHolders;



    System.Data.DataTable Dt = null;
    clsData objData = null;
    clsSession sess = null;



    protected void Page_Load(object sender, EventArgs e)
    {
        // loadData();

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
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
                btnUpdate.Visible = false;
                btnLoadDataEdit.Visible = false;
                btnGenNewSheet0.Visible = false;
                //  btnImport.Visible = false;
                btnLoadData.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnUpdate.Visible = true;
                btnLoadDataEdit.Visible = true;
                btnGenNewSheet0.Visible = true;
                //    btnImport.Visible = true;
                btnLoadData.Visible = true;
            }

            btnUpdate.Visible = false;
            btnImport.Visible = false;
            MultiView1.ActiveViewIndex = 1;
            FillStudent();
        }

    }
    protected void BtnGenerateAc_Click(object sender, EventArgs e)
    {
        AllInOne();
    }

    protected void FillStudent()
    {
        objData = new clsData();


        objData.ReturnDropDown("select StudentId as Id, StudentFName+' '+StudentLName as Name  from Student where ActiveInd='A'", ddlStudentEdit);
        Dt = new System.Data.DataTable();
        Dt = objData.ReturnDataTable("select distinct cast(convert(char(10),  DateOfMeeting , 101)as date) as Date from StdtAcdSheet", false);
        ddlDate.Items.Clear();
        foreach (DataRow dr in Dt.Rows)
        {
            ddlDate.Items.Add(DateTime.Parse(dr[0].ToString()).ToString("MM/dd/yyyy").Replace("-","/"));
        }
        if (ddlDate.Items.Count == 0)
        {
            ddlDate.Items.Add("---------------Select Date--------------");
        }
        else
        {
            ddlDate.Items.Insert(0, "---------------Select Date--------------");
        }

    }
    private void loadDataList()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();

        //select * from StdtLessonPlan inner join LessonPlan on StdtLessonPlan.LessonPlanId=LessonPlan.LessonPlanId inner join Goal on StdtLessonPlan.GoalId=Goal.GoalId where StdtLessonPlan.StudentId=1
        //select LessonPlan.LessonPlanName,Goal.GoalName,StdtLessonPlan.Objective3,(select LookupName from LookUp where  LookupId=DSTempHdr.TeachingProcId ) as 'Type Of Instruction' from StdtLessonPlan inner join LessonPlan on StdtLessonPlan.LessonPlanId=LessonPlan.LessonPlanId inner join Goal on StdtLessonPlan.GoalId=Goal.GoalId inner join DSTempHdr on StdtLessonPlan.LessonPlanId=DSTempHdr.LessonPlanId where StdtLessonPlan.StudentId=2 and StdtLessonPlan.SchoolId=1 and DSTempHdr.SchoolId=1 and DSTempHdr.StudentId=2 and DSTempHdr.StatusId=(select LookupId from LookUp where LookupName='Approved'  and LookupType='TemplateStatus')  and StdtLessonPlan.ActiveInd='A'

        DateTime dateNow8 = System.DateTime.Now;
        DateTime dateNow7 = dateNow8.AddDays(-14);
        DateTime dateNow6 = dateNow7.AddDays(-14);
        DateTime dateNow5 = dateNow6.AddDays(-14);
        DateTime dateNow4 = dateNow5.AddDays(-14);
        DateTime dateNow3 = dateNow4.AddDays(-14);
        DateTime dateNow2 = dateNow3.AddDays(-14);
        DateTime dateNow1 = dateNow2.AddDays(-14);

        string qry = "select DSTempHdr.DSTempHdrId,LessonPlan.LessonPlanName,Goal.GoalName,StdtLessonPlan.Objective3," +
"(select LookupName from LookUp where  LookupId=DSTempHdr.PromptTypeId ) as 'TypeOfInstruction'," +

"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'IOAPer1'," +
"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel1'," +
"(SELECT TOP 1 (select SetName from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set1'," +

"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'IOAPer2'," +
"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel2'," +
"(SELECT TOP 1 (select SetName from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set2'," +

"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'IOAPer3'," +
"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel3'," +
"(SELECT TOP 1 (select SetName from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set3'," +

"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'IOAPer4'," +
"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel4'," +
"(SELECT TOP 1 (select SetName from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set4'," +

"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'IOAPer5'," +
"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel5'," +
"(SELECT TOP 1 (select SetName from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set5'," +

"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'IOAPer6'," +
"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel6'," +
"(SELECT TOP 1 (select SetName from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set6'," +

"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'IOAPer7'," +
"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel7'," +
"(SELECT TOP 1 (select SetName from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(datetime, CreatedOn)  between CONVERT(datetime, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(datetime, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set7'" +


"from StdtLessonPlan inner join LessonPlan on StdtLessonPlan.LessonPlanId=LessonPlan.LessonPlanId inner join Goal on StdtLessonPlan.GoalId=Goal.GoalId " +
"inner join DSTempHdr on StdtLessonPlan.LessonPlanId=DSTempHdr.LessonPlanId " +
"where StdtLessonPlan.StudentId=" + sess.StudentId + " and StdtLessonPlan.SchoolId=" + sess.SchoolId + " and " +
"DSTempHdr.SchoolId=" + sess.SchoolId + " and DSTempHdr.StudentId=" + sess.StudentId + " and" +
" DSTempHdr.StatusId=(select LookupId from LookUp where LookupName='Approved'  and LookupType='TemplateStatus')  and " +
" StdtLessonPlan.ActiveInd='A'";


        string strQuery = qry;
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt.Rows.Count > 0)
        {
            GridViewAccSheet.DataSource = Dt;
            GridViewAccSheet.DataBind();
            btnSave.Visible = true;
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
            GridViewAccSheet.DataSource = null;
            GridViewAccSheet.DataBind();
            btnSave.Visible = false;
        }
    }



    private void loadDataListEdit()
    {

        objData = new clsData();
        Dt = new System.Data.DataTable();



        string qry = "select * from StdtAcdSheet where StudentId=" + sess.StudentId + " and CONVERT(datetime,DateOfMeeting)=CONVERT(datetime,'" + ddlDate.SelectedItem.Text + "')";


        string strQuery = qry;
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                GridViewAccSheetedit.DataSource = Dt;
                GridViewAccSheetedit.DataBind();
                btnUpdate.Visible = true;
                btnImport.Visible = true;

            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
                GridViewAccSheetedit.DataSource = null;
                GridViewAccSheetedit.DataBind();
                btnImport.Visible = false;
                btnUpdate.Visible = false;
            }
        }
    }



    /* private void getReplace(string docText, string position, string wordReplace)
     {
         Regex regexText = new Regex("Placeholder1");
         docText = regexText.Replace(docText, wordReplace);
     }

     private void placed(string Doc)
     {
         string col = "";
         string plc = "";
         //columns.Length
         for (int i = 0; i < 18; i++)
         {
             col = columns[i].ToString().Trim();
             plc = placeHolders[i].ToString().Trim();
             getReplace(Doc, plc, col);
         }

     }*/

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

            string path = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string newpath = path + "\\";
            string newFileName = "AccSheet" + PageNo;
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
            tdMsg.InnerHtml = clsGeneral.warningMsg("Directory or File already Exit !");
            return "";
            throw Ex;
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

        // sess = (clsSession)Session["UserSession"];
        string Path = "";
        string NewPath = "";
        Dt = new System.Data.DataTable();
        ClsAccSheetExport objExport = new ClsAccSheetExport();
        try
        {

            CreateQuery("NE", "XMLAS\\AS1Creations.xml");
            Dt = objExport.getAccSheet(sess.StudentId, 1, ddlDate.SelectedItem.Text);
            int pageCount = 0;
            foreach (DataRow dr in Dt.Rows)
            {
                for (int i = 0; i < placeHolders.Length; i++)
                {
                    if (i == 1)
                    {
                        string[] startDate = dr["Period1"].ToString().Split('-');
                        string[] endDate = dr["Period7"].ToString().Split('-');
                        string dateAccSheet = startDate[0] + "-" + endDate[1];
                        columns[i] = dateAccSheet;
                    }
                    else if (i == 2)
                    {
                        columns[i] = DateTime.Parse(dr["DateOfMeeting"].ToString()).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        columns[i] = dr[columnsToAdd[i]].ToString();
                    }
                }


                Path = Server.MapPath("~\\StudentBinder\\ASTemplates\\ASTemplates1.docx");
                NewPath = CopyTemplate(Path, pageCount.ToString());
                if (NewPath != "")
                {
                    SearchAndReplace(NewPath);
                }

                pageCount++;
            }


            /* if (columns != null)
             {
                 Path = Server.MapPath("~\\Administration\\ASTemplates\\ASTemplates1.docx");
                 NewPath = CopyTemplate(Path, "1");
                 if (NewPath != "")
                 {
                     SearchAndReplace(NewPath);
                 }
             }
             */

            string iepDoneFlg = MergeFiles();

            if (iepDoneFlg == "")
            {
                tdMsgExport.InnerHtml = clsGeneral.failedMsg("Document Creation Failed !");
            }
            else
            {
                tdMsg.InnerHtml = "";
                tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Documents Sucessfully Created ");
                string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#PopDownload').css('top', '5%'); $('#PopDownload').show(); }); $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
                //   btnIEPExport.Text = "Download";
                //   BtnCanel.Visible = true;
            }

        }
        catch (Exception eX)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Failed !");
            throw eX;
        }
    }



    public string MergeFiles()
    {
        try
        {
            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";
            string Temp1 = Server.MapPath("~\\StudentBinder") + "\\ACMerge";
            const string DOC_URL = "/word/document.xml";
            string FolderName = "\\AcademicSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}";
            FolderName = string.Format(FolderName, DateTime.Now);
            string path = Server.MapPath("~\\Administration") + "\\IEPMerged";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string OUTPUT_FILE = Temp1 + "\\AcademicSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}.docx";
            string FIRST_PAGE = Server.MapPath("~\\StudentBinder\\ASTemplates\\Dummy.docx");

            string fileName = string.Format(OUTPUT_FILE, DateTime.Now);
            File.Copy(FIRST_PAGE, fileName);

            string[] filePaths = Directory.GetFiles(Temp);
            int i = 1;
            for (int j = filePaths.Length - 1; j >= 0; j--)
            {
                makeWord(filePaths[j], fileName, i);
                i++;
            }

            ViewState["FileName"] = fileName;
            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }


            return FolderName;

        }
        catch (Exception Ex)
        {
            return "";
        }
    }
    public void makeWord(string filenamePass, string fileName1, int i)
    {

        using (WordprocessingDocument myDoc =
            WordprocessingDocument.Open(fileName1, true))
        {
            string altChunkId = "AltChunkId" + i.ToString();
            MainDocumentPart mainPart = myDoc.MainDocumentPart;
            AlternativeFormatImportPart chunk =
                mainPart.AddAlternativeFormatImportPart(
                AlternativeFormatImportPartType.WordprocessingML, altChunkId);


            using (FileStream fileStream = File.Open(filenamePass, FileMode.Open))
                chunk.FeedData(fileStream);


            AltChunk altChunk = new AltChunk();
            altChunk.Id = altChunkId;
            mainPart.Document
                .Body
                .InsertAfter(altChunk, mainPart.Document.Body
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last());
            mainPart.Document.Save();
        }
    }


    protected void btnLoadData_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        btnBack.Text = "Cancel";
        loadDataList();
    }
    protected void Save_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        btnBack.Text = "Cancel";
        objData = new clsData();
        string testIfPresent = "select AccSheetId from StdtAcdSheet where StudentId=" + sess.StudentId + " and CONVERT(datetime,DateOfMeeting)=CONVERT(datetime,'" + System.DateTime.Now.ToString("MM/dd/yyyy") + "')";

        if (objData.IFExists(testIfPresent) == false)
        {
            foreach (GridViewRow row in GridViewAccSheet.Rows)
            {
                Label lblGoalArea = row.Controls[0].FindControl("lblGoalArea") as Label;
                Label lblGoal = row.Controls[0].FindControl("lblGoal") as Label;
                TextBox txtbenchaMark = row.Controls[0].FindControl("txtbenchaMark") as TextBox;

                Label lblPeriod1 = row.Controls[0].FindControl("lblPeriod1") as Label;
                Label lblPeriod2 = row.Controls[0].FindControl("lblPeriod2") as Label;
                Label lblPeriod3 = row.Controls[0].FindControl("lblPeriod3") as Label;
                Label lblPeriod4 = row.Controls[0].FindControl("lblPeriod4") as Label;
                Label lblPeriod5 = row.Controls[0].FindControl("lblPeriod5") as Label;
                Label lblPeriod6 = row.Controls[0].FindControl("lblPeriod6") as Label;
                Label lblPeriod7 = row.Controls[0].FindControl("lblPeriod7") as Label;

                Label lblTypOfIns1 = row.Controls[0].FindControl("lblTypOfIns1") as Label;

                Label lblStmlsSet1 = row.Controls[0].FindControl("lblStmlsSet1") as Label;
                Label lblStmlsSet2 = row.Controls[0].FindControl("lblStmlsSet2") as Label;
                Label lblStmlsSet3 = row.Controls[0].FindControl("lblStmlsSet3") as Label;
                Label lblStmlsSet4 = row.Controls[0].FindControl("lblStmlsSet4") as Label;
                Label lblStmlsSet5 = row.Controls[0].FindControl("lblStmlsSet5") as Label;
                Label lblStmlsSet6 = row.Controls[0].FindControl("lblStmlsSet6") as Label;
                Label lblStmlsSet7 = row.Controls[0].FindControl("lblStmlsSet7") as Label;

                Label lblprmtLvl1 = row.Controls[0].FindControl("lblprmtLvl1") as Label;
                Label lblprmtLvl2 = row.Controls[0].FindControl("lblprmtLvl2") as Label;
                Label lblprmtLvl3 = row.Controls[0].FindControl("lblprmtLvl3") as Label;
                Label lblprmtLvl4 = row.Controls[0].FindControl("lblprmtLvl4") as Label;
                Label lblprmtLvl5 = row.Controls[0].FindControl("lblprmtLvl5") as Label;
                Label lblprmtLvl6 = row.Controls[0].FindControl("lblprmtLvl6") as Label;
                Label lblprmtLvl7 = row.Controls[0].FindControl("lblprmtLvl7") as Label;

                Label lblIOA1 = row.Controls[0].FindControl("lblIOA1") as Label;
                Label lblIOA2 = row.Controls[0].FindControl("lblIOA2") as Label;
                Label lblIOA3 = row.Controls[0].FindControl("lblIOA3") as Label;
                Label lblIOA4 = row.Controls[0].FindControl("lblIOA4") as Label;
                Label lblIOA5 = row.Controls[0].FindControl("lblIOA5") as Label;
                Label lblIOA6 = row.Controls[0].FindControl("lblIOA6") as Label;
                Label lblIOA7 = row.Controls[0].FindControl("lblIOA7") as Label;

                Label lblNoOfPos1 = row.Controls[0].FindControl("lblNoOfPos1") as Label;
                Label lblNoOfPos2 = row.Controls[0].FindControl("lblNoOfPos2") as Label;
                Label lblNoOfPos3 = row.Controls[0].FindControl("lblNoOfPos3") as Label;
                Label lblNoOfPos4 = row.Controls[0].FindControl("lblNoOfPos4") as Label;
                Label lblNoOfPos5 = row.Controls[0].FindControl("lblNoOfPos5") as Label;
                Label lblNoOfPos6 = row.Controls[0].FindControl("lblNoOfPos6") as Label;
                Label lblNoOfPos7 = row.Controls[0].FindControl("lblNoOfPos7") as Label;

                TextBox txtFreeText = row.Controls[0].FindControl("txtFreetxt") as TextBox;
                TextBox txtPersDissc = row.Controls[0].FindControl("txtPersDissc") as TextBox;
                TextBox txtResAndDeadline = row.Controls[0].FindControl("txtResAndDeadline") as TextBox;


                string sqlQry = "INSERT INTO [dbo].[StdtAcdSheet] ([StudentId],[DateOfMeeting],[GoalArea],[Goal],[Benchmarks],[FeedBack],[PreposalDiss]" +
                    ",[PersonResNdDeadline],[TypeOfInstruction],[Period1],[Set1],[Prompt1],[IOA1],[NoOfTimes1],[Period2],[Set2],[Prompt2],[IOA2]," +
                    "[NoOfTimes2],[Period3],[Set3],[Prompt3],[IOA3],[NoOfTimes3],[Period4],[Set4],[Prompt4],[IOA4],[NoOfTimes4],[Period5],[Set5]," +
                "[Prompt5],[IOA5],[NoOfTimes5],[Period6],[Set6],[Prompt6],[IOA6],[NoOfTimes6],[Period7],[Set7],[Prompt7],[IOA7],[NoOfTimes7]) " +
                "VALUES(" + sess.StudentId + ",'" + System.DateTime.Now.ToString("MM/dd/yyyy") + "'," +
                "'" + lblGoalArea.Text + "'," +
                "'" + lblGoal.Text + "'," +
                "'" + txtbenchaMark.Text + "'," +
                "'" + clsGeneral.convertQuotes(txtFreeText.Text.Trim()) + "'," +
                "'" + clsGeneral.convertQuotes(txtPersDissc.Text.Trim()) + "'," +
                "'" + clsGeneral.convertQuotes(txtResAndDeadline.Text.Trim()) + "'," +
                "'" + lblTypOfIns1.Text + "'," +

                "'" + lblPeriod1.Text + "'," +
                "'" + lblStmlsSet1.Text + "'," +
                "'" + lblprmtLvl1.Text + "'," +
                "'" + lblIOA1.Text + "'," +
                "'" + lblNoOfPos1.Text + "'," +

                "'" + lblPeriod2.Text + "'," +
                "'" + lblStmlsSet2.Text + "'," +
                "'" + lblprmtLvl2.Text + "'," +
                "'" + lblIOA2.Text + "'," +
                "'" + lblNoOfPos2.Text + "'," +

                "'" + lblPeriod3.Text + "'," +
                "'" + lblStmlsSet3.Text + "'," +
                "'" + lblprmtLvl3.Text + "'," +
                "'" + lblIOA3.Text + "'," +
                "'" + lblNoOfPos3.Text + "'," +

                "'" + lblPeriod4.Text + "'," +
                "'" + lblStmlsSet4.Text + "'," +
                "'" + lblprmtLvl4.Text + "'," +
                "'" + lblIOA4.Text + "'," +
                "'" + lblNoOfPos4.Text + "'," +

                "'" + lblPeriod5.Text + "'," +
                "'" + lblStmlsSet5.Text + "'," +
                "'" + lblprmtLvl5.Text + "'," +
                "'" + lblIOA5.Text + "'," +
                "'" + lblNoOfPos5.Text + "'," +

                "'" + lblPeriod6.Text + "'," +
                "'" + lblStmlsSet6.Text + "'," +
                "'" + lblprmtLvl6.Text + "'," +
                "'" + lblIOA6.Text + "'," +
                "'" + lblNoOfPos6.Text + "'," +

                "'" + lblPeriod7.Text + "'," +
                "'" + lblStmlsSet7.Text + "'," +
                "'" + lblprmtLvl7.Text + "'," +
                "'" + lblIOA7.Text + "'," +
                "'" + lblNoOfPos7.Text + "'" +
                ")";


                int insetChk = objData.Execute(sqlQry);
                if (insetChk > 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Inserted Succesfully   ");
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Data not inserted   ");
                }


                // Do something with the textBox's value
            }
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Data allready Present   ");
        }
    }
    protected void GridViewAccSheet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DateTime dateNow8 = System.DateTime.Now;
            DateTime dateNow7 = dateNow8.AddDays(-14);
            DateTime dateNow6 = dateNow7.AddDays(-14);
            DateTime dateNow5 = dateNow6.AddDays(-14);
            DateTime dateNow4 = dateNow5.AddDays(-14);
            DateTime dateNow3 = dateNow4.AddDays(-14);
            DateTime dateNow2 = dateNow3.AddDays(-14);
            DateTime dateNow1 = dateNow2.AddDays(-14);

            Label lblperiod1 = (Label)e.Row.FindControl("lblPeriod1");
            Label lblperiod2 = (Label)e.Row.FindControl("lblPeriod2");
            Label lblperiod3 = (Label)e.Row.FindControl("lblPeriod3");
            Label lblperiod4 = (Label)e.Row.FindControl("lblPeriod4");
            Label lblperiod5 = (Label)e.Row.FindControl("lblPeriod5");
            Label lblperiod6 = (Label)e.Row.FindControl("lblPeriod6");
            Label lblperiod7 = (Label)e.Row.FindControl("lblPeriod7");

            lblperiod1.Text = dateNow1.ToString("MM'/'dd'/'yyyy") + " - " + dateNow2.ToString("MM'/'dd'/'yyyy");
            lblperiod2.Text = dateNow2.ToString("MM'/'dd'/'yyyy") + " - " + dateNow3.ToString("MM'/'dd'/'yyyy");
            lblperiod3.Text = dateNow3.ToString("MM'/'dd'/'yyyy") + " - " + dateNow4.ToString("MM'/'dd'/'yyyy");
            lblperiod4.Text = dateNow4.ToString("MM'/'dd'/'yyyy") + " - " + dateNow5.ToString("MM'/'dd'/'yyyy");
            lblperiod5.Text = dateNow5.ToString("MM'/'dd'/'yyyy") + " - " + dateNow6.ToString("MM'/'dd'/'yyyy");
            lblperiod6.Text = dateNow6.ToString("MM'/'dd'/'yyyy") + " - " + dateNow7.ToString("MM'/'dd'/'yyyy");
            lblperiod7.Text = dateNow7.ToString("MM'/'dd'/'yyyy") + " - " + dateNow8.ToString("MM'/'dd'/'yyyy");
        }
    }
    protected void btnPreAccSheet1_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        btnSave.Visible = false;
        MultiView1.ActiveViewIndex = 0;
        GridViewAccSheet.DataSource = null;
        GridViewAccSheet.DataBind();
        GridViewAccSheetedit.DataSource = null;
        GridViewAccSheetedit.DataBind();
        btnImport.Visible = false;
        FillStudent();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        btnBack.Text = "Cancel";
        btnUpdate.Visible = false;
        // btnImport.Visible = false;
        MultiView1.ActiveViewIndex = 1;
        GridViewAccSheet.DataSource = null;
        GridViewAccSheet.DataBind();
        GridViewAccSheetedit.DataSource = null;
        GridViewAccSheetedit.DataBind();
        FillStudent();
    }
    protected void btnLoadDataEdit_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        if (ddlDate.SelectedIndex != 0)
        {
            loadDataListEdit();
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Date...");
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        objData = new clsData();
        int testupdate = 0;
        foreach (GridViewRow row in GridViewAccSheetedit.Rows)
        {
            HiddenField hdFldAcdId = row.Controls[0].FindControl("hdFldAcdId") as HiddenField;

            TextBox txtFreeText = row.Controls[0].FindControl("txtFreetxtedit") as TextBox;
            TextBox txtPersDissc = row.Controls[0].FindControl("txtPersDisscedit") as TextBox;
            TextBox txtResAndDeadline = row.Controls[0].FindControl("txtResAndDeadlineedit") as TextBox;
            //clsGeneral.convertQuotes( txtFreeText.Text.Trim() )
            string strqury = "update StdtAcdSheet set FeedBack='" + clsGeneral.convertQuotes(txtFreeText.Text.Trim()) + "',PreposalDiss='" + clsGeneral.convertQuotes(txtPersDissc.Text.Trim()) + "',PersonResNdDeadline='" + clsGeneral.convertQuotes(txtResAndDeadline.Text.Trim()) + "' WHERE AccSheetId=" + hdFldAcdId.Value + "";
            testupdate += objData.Execute(strqury);

        }
        if (testupdate > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Succesfully   ");
            loadDataListEdit();
            btnBack.Text = "Back";
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Data Not updated   ");
        }
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        AllInOne();
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        downloadfile();
    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        tdMsgExport.InnerHtml = "";
        string path = Server.MapPath("~\\StudentBinder") + "\\ACMerge";
        Array.ForEach(Directory.GetFiles(path), File.Delete);
        //ClientScript.RegisterStartupScript(GetType(), "", "DownloadDone();", true);
    }
    public void downloadfile()
    {
        try
        {

            string FileName = ViewState["FileName"].ToString();
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileName + "\"");
            byte[] data = req.DownloadData(FileName);
            response.BinaryWrite(data);
            ClientScript.RegisterStartupScript(GetType(), "", "HideWait();", true);
            response.End();


        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "alert('Submission not possible because another user made some changes in this Assessment');", true);
            //ClientScript.RegisterStartupScript(GetType(), "", "alert('sd');", true);
        }
        ViewState["FileName"] = "";
    }
}