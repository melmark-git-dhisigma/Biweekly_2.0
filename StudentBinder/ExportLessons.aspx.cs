using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Net;
using Ionic.Zip;


public partial class StudentBinder_ExportLessons : System.Web.UI.Page
{
    string strBinder = "";
    DataTable Dt = null;
    object objVal = null;
    string strQuery = "";
    static string[] columns;
    static string[] placeHolders;
    static string[] columnsCheck;
    static int checkCount = 0;
    string advanceSet = "";
    string advanceStep = "";
    string advancePrompt = "";
    string MoveBackSet = "";
    string MoveBackStep = "";
    string MoveBackPrompt = "";
    string ModificationSet = "";
    string ModificationStep = "";
    string ModificationPrompt = "";
    public static bool frequency;
    public static string frequencycnt;
    public static bool status;
    public static clsData objData = null;
    public static clsSession sess = null;
    public static clsRoles role = null;
    public static DataClass objdataClass = null;
    public static ClsTemplateSession ObjTempSess = null;
    public static clsStatus objStatus = null;
    public clsRoles objRole = null;
    int chk = 0;
    int chkpgrs = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillLessonplans();
        }       
    }
    protected void FillLessonplans()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            DataTable dtLP = new DataTable();
            dtLP.Columns.Add("Id", typeof(string));
            dtLP.Columns.Add("Name", typeof(string));
            DataRow dr0 = dtLP.NewRow();
            dr0["Id"] = -1;
            dr0["Name"] = "----------Select Lesson Plan----------";
            dtLP.Rows.Add(dr0);
            DataRow dr = dtLP.NewRow();
            dr["Id"] = 0;
            dr["Name"] = "All Lesson Plan";
            dtLP.Rows.Add(dr);
            DataTable DTLesson = new DataTable();

            clsLessons oLessons = new clsLessons();
            if (rdoinprogress.Text == "In-progress")
            {
                btn1.Visible = false;
                btn2.Visible = true;
                DummyDropDown.Visible = true;
                ddlLessonplan0.Visible = false;
                DummyDropDown.Enabled = false;
                DTLesson = oLessons.getLessonPlans(sess.StudentId.ToString(), "LP.LessonPlanId AS Id, DTmp.DSTemplateName AS Name,DTmp.VerNbr", "(LU.LookupName='In Progress' OR LU.LookupName='Expired')");

            }
            else if (rdoinprogress.Text == "Pending Approval")
            {
                btn1.Visible = false;
                btn2.Visible = true;
                DummyDropDown.Visible = true;
                ddlLessonplan0.Visible = false;
                DummyDropDown.Enabled = false;
                DTLesson = oLessons.getLessonPlans(sess.StudentId.ToString(), "LP.LessonPlanId AS Id, DTmp.DSTemplateName AS Name,DTmp.VerNbr", "(LU.LookupName='Pending Approval')");

            }
            else
            {
                btn1.Visible = true;
                btn2.Visible = false;
                DummyDropDown.Visible = false;
                ddlLessonplan0.Visible = true;
                DTLesson = oLessons.getLessonPlans(sess.StudentId.ToString(), "LP.LessonPlanId AS Id, DTmp.DSTemplateName AS Name,DTmp.VerNbr", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance')");

            } 
            if (DTLesson != null)
            {
                if (DTLesson.Rows.Count > 0)
                {

                    foreach (DataRow drLessn in DTLesson.Rows)
                    {
                        DataRow drr = dtLP.NewRow();
                        drr["Id"] = drLessn.ItemArray[0];
                        drr["Name"] = drLessn.ItemArray[1];
                        dtLP.Rows.Add(drr);
                    }

                }
            }
            ddlLessonplan.DataSource = dtLP;
            ddlLessonplan.DataTextField = "Name";
            ddlLessonplan.DataValueField = "Id";
            ddlLessonplan.DataBind();

        }

    }
    protected void Lessontype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLessonplans();
        if (rdoinprogress.Text == "In-progress")
        {
            btn1.Visible=false;
            btn2.Visible=true;
            DummyDropDown.Visible = true;
            ddlLessonplan0.Visible = false;
            DummyDropDown.Enabled = false;

        }
        else if(rdoinprogress.Text == "Pending Approval")
        {
            btn1.Visible = false;
            btn2.Visible = false;
            DummyDropDown.Visible = true;
            ddlLessonplan0.Visible = false;
            DummyDropDown.Enabled = false;
        }
        else
        {
            btn1.Visible = true;
            btn2.Visible = false;
            DummyDropDown.Visible = false;
            ddlLessonplan0.Visible = true;            
        }
    }
    
    protected void FillVersion(object sender, EventArgs e)
    {
        if (rdoinprogress.Text == "Approved")
        {
            btn1.Visible = true;
            btn2.Visible = false;
            DummyDropDown.Visible = false;
            ddlLessonplan0.Visible = true;
            int lId = Convert.ToInt32(ddlLessonplan.SelectedValue);
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                DataTable dtLP = new DataTable();
                dtLP.Columns.Add("Id", typeof(string));
                dtLP.Columns.Add("Name", typeof(string));
                clsLessons oLessons = new clsLessons();
                DataTable DTLesson = objData.ReturnDataTable("  select DstempHdrId, LessonplanId,case when VerNbr is null then  DSTemplateName else DSTemplateName+VerNbr end  As VersionName,VerNbr from DSTempHdr where LessonPlanId=" + lId + " and  StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('Maintenance','Inactive','Approved')) and StudentId=" + sess.StudentId + " and DSTemplateName<>''", false);
                if (lId == 0)
                {
                    DataRow drr = dtLP.NewRow();
                    drr["Id"] = "0";
                    drr["Name"] = "All latest version";
                    dtLP.Rows.Add(drr);
                    Session["lessonplanId"] = "0";
                }
                else if (DTLesson != null)
                {
                    if (DTLesson.Rows.Count > 0)
                    {

                        foreach (DataRow drLessn in DTLesson.Rows)
                        {
                            DataRow drr = dtLP.NewRow();
                            drr["Id"] = drLessn.ItemArray[0];
                            drr["Name"] = drLessn.ItemArray[2];
                            dtLP.Rows.Add(drr);
                            Session["lessonplanId"] = drLessn.ItemArray[1];
                        }

                    }
                }
                ddlLessonplan0.DataSource = dtLP;
                ddlLessonplan0.DataTextField = "Name";
                ddlLessonplan0.DataValueField = "Id";
                ddlLessonplan0.DataBind();
            }
        }
        else
        {
            btn1.Visible = false;
            btn2.Visible = true;
            DummyDropDown.Visible = true;
            ddlLessonplan0.Visible = false;
            DummyDropDown.Enabled = false;
        }
    }

    protected void btnExportWord_Click1(object sender, EventArgs e)
    {

        ClsErrorLog err = new ClsErrorLog();
        chk = 1;
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        try
        {

            if (ddlLessonplan.SelectedValue == "0")
            {
                chkpgrs = 0;
                DataTable Dt1 = new DataTable();
                int cnt;
                clsData objData = new clsData();
                if (rdoinprogress.Text == "In-progress")
                {
                    Dt1 = objData.ReturnDataTable("SELECT LessonPlanId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + "  and StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('In Progress','Expired'))", false);
                }
                else
                {
                    Dt1 = objData.ReturnDataTable("SELECT LessonPlanId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + "  and StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('Pending Approval'))", false);                
                }
                    cnt = Dt1.Rows.Count;
                string fil = objData.FetchValue("SELECT  FirstName+' '+ LastName FROM StudentPersonal WHERE  StudentPersonalId=" + sess.StudentId).ToString();
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectoryByName(fil + "-All Lesson");
                    foreach (DataRow Dr in Dt1.Rows)
                    {

                        int lessonId = Convert.ToInt32(Dr["LessonPlanId"]);

                        int TemplateId = Convert.ToInt32(objData.FetchValue("SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId = " + lessonId + " and StudentId=" + sess.StudentId + "and StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('In Progress','Pending Approval','Expired'))"));



                        exportToWord(lessonId, TemplateId);
                        zip.AddFile(ViewState["FileName5"].ToString(), fil + "-All Lesson");

                    }
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    //Response.End();
                }

            }
            else
            {
                chkpgrs = 1;
                int lessonId = Convert.ToInt32(ddlLessonplan.SelectedValue);
                int TemplateId = 0;
                if (rdoinprogress.Text == "In-progress")
                {
                     TemplateId = Convert.ToInt32(objData.FetchValue("SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId = " + lessonId + " and StudentId=" + sess.StudentId + "and StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('In Progress','Expired'))"));
                }
                else
                {
                     TemplateId = Convert.ToInt32(objData.FetchValue("SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId = " + lessonId + " and StudentId=" + sess.StudentId + "and StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('Pending Approval'))"));
                }
                string fil = objData.FetchValue("SELECT  FirstName+' '+ LastName FROM StudentPersonal WHERE  StudentPersonalId=" + sess.StudentId).ToString();

                exportToWord(lessonId, TemplateId);

                
            }
        }

        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
    }
    
    protected void btnExportWord_Click(object sender, EventArgs e)
    {

        ClsErrorLog err = new ClsErrorLog();
        chk = 0;
        chkpgrs = 0;
        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        try
        {

            if (ddlLessonplan0.SelectedValue == "0" && ddlLessonplan.SelectedValue=="0")
            {
            DataTable Dt1 = new DataTable();
            int cnt;
            chk = 1;
            clsData objData = new clsData();
            Dt1 = objData.ReturnDataTable("SELECT LessonPlanId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + "  and StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('Maintenance','Approved'))", false);
            cnt = Dt1.Rows.Count;
            string fil = objData.FetchValue("SELECT  FirstName+' '+ LastName FROM StudentPersonal WHERE  StudentPersonalId=" + sess.StudentId).ToString();
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectoryByName(fil + "-All Lesson");
                foreach (DataRow Dr in Dt1.Rows)
                {

                    int lessonId = Convert.ToInt32(Dr["LessonPlanId"]);

                    int TemplateId = Convert.ToInt32(objData.FetchValue("SELECT DSTempHdrId FROM DSTempHdr WHERE LessonPlanId = " + lessonId + " and StudentId=" + sess.StudentId + "and StatusId in(select lookupid from [LookUp] where LookupType='TemplateStatus' and LookupName in('Maintenance','Approved'))"));


                    if(ddlFormat.SelectedIndex==2)
                        exportToWordSupportStrat(lessonId, TemplateId);
                    else
                        exportToWord(lessonId, TemplateId);

                    zip.AddFile(ViewState["FileName5"].ToString(), fil + "-All Lesson");
                   
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                //Response.End();
            }
            
            }
            else{
            int lessonId = Convert.ToInt32(Session["lessonplanId"]);
            string fil = objData.FetchValue("SELECT  FirstName+' '+ LastName FROM StudentPersonal WHERE  StudentPersonalId=" + sess.StudentId).ToString();            
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectoryByName(fil + "_Versions");
                foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan0.Items)
                {
                    if (item.Selected == true)
                    {
                        if (ddlFormat.SelectedIndex == 2)
                            exportToWordSupportStrat(lessonId, Convert.ToInt32(item.Value));
                        else
                            exportToWord(lessonId, Convert.ToInt32(item.Value));

                        zip.AddFile(ViewState["FileName5"].ToString(), fil + "_Versions");

                    }

                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                //Response.End();
            }
            
        }            
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }

    }
    

  

    private void exportToWord(int LessonPlanId, int TempId)
    {
        /*----------------------------------------------------------------------------*/
        clsData objData = new clsData();
        string Path = "";
        string NewPath = "";

        try
        {
            CreateQuery("NE", "..\\Administration\\LesonPlanTemplate\\LessonPlanXML.xml");
            Path = Server.MapPath("~\\Administration\\LesonPlanTemplate\\LessonPlanSampleTemplate.docx");
            string Path2 = Server.MapPath("~\\Administration\\LessonPlanMerg\\Dummy.docx");

            string path = Server.MapPath("~\\Administration") + "\\TempLessonPlan";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            int fileCount = Directory.GetFiles(path).Length + 1;
            string newpathz = Server.MapPath("~\\Administration\\LessonPlanMerg\\");
            string subname = DateTime.Now.Date.ToShortDateString();
            if (subname.Contains('/'))
            {
                subname = subname.Replace('/', '-');
            }
            string LessonPlanName = "";
            string newFileName = "";
            object obj = objData.FetchValue("select DSTemplateName from dstemphdr WHERE  DSTempHdrId=" + TempId + "");
            object obj1 = objData.FetchValue("select VerNbr from dstemphdr WHERE  DSTempHdrId=" + TempId + "");
            string ver = obj1.ToString();
            if (obj != null)
            {
                LessonPlanName = obj.ToString();
                LessonPlanName = LessonPlanName.Replace(":", " ");
                LessonPlanName = LessonPlanName.Replace("*", " ");
                LessonPlanName = LessonPlanName.Replace(@"\", " ");
                LessonPlanName = LessonPlanName.Replace("|", " ");
                LessonPlanName = LessonPlanName.Replace("\"", " ");
                LessonPlanName = LessonPlanName.Replace("?", " ");
                LessonPlanName = LessonPlanName.Replace("/", " "); 
                LessonPlanName = LessonPlanName.Replace("<", " ");
                LessonPlanName = LessonPlanName.Replace(">", " ");
            }

            if (chk == 1)
            {
                newFileName = "LessonPlan" + "_" + LessonPlanName + "_" + sess.StudentId + "-" + subname + "_" + fileCount;
            }
            else
            {
                newFileName = "LessonPlan" + "_" + LessonPlanName + ver + "_" + sess.StudentId + "-" + subname + "_" + fileCount;
            }
            ViewState["FileName2"] = newFileName;
            FileInfo f1 = new FileInfo(Path2);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpathz))
                {
                    Directory.CreateDirectory(newpathz);
                }

                f1.CopyTo(string.Format("{0}{1}{2}", newpathz, newFileName, f1.Extension));
            }

            NewPath = CopyTemplate(Path, "1");
            if (NewPath != "")
            {
                Dt = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt4Reason = new DataTable();
                dt4.Columns.Add("Date");
                dt4.Columns.Add("Reason");
                string reason = "";
                string createVal = "";
                try
                {
                    objData = new clsData();
                    int val = 0;
                    Double versionNum = 0.0;
                    DataTable Dt2 = new DataTable();

                    sess = (clsSession)Session["UserSession"];
                    if (sess != null)
                    {

                        strQuery = "select StimulyActivityId,case when(ActivitiType='STARTED') then ('STARTED') ELSE (case when(ActivitiType='SET') then (Select 'SET - '+SetCd from DSTempSet where  DSTempSetId= Act.ActivityId)"
                             + "ELSE (case when(ActivitiType='STEP') then (Select 'STEP - '+StepCd from DSTempStep where  DSTempStepId= Act.ActivityId)"
                             + "ELSE (case when(ActivitiType='MASTERED') then (Select 'SET - '+SetCd+'' from DSTempSet where  DSTempSetId= Act.ActivityId)"
                             + "ELSE(Select 'PROMPT - '+ LookupName from LookUp where  LookupId= Act.ActivityId)END)END)END)END NAME,"
                             + "(Convert(VARCHAR(50), StartTime,110))StartDate,(Convert(VARCHAR(50), DateMastered,110))DateMastered,(Convert(VARCHAR(50), DateClosed,110))DateClosed"
                             + ",Hdr.Reason_New,Hdr.CreatedOn,Hdr.VerNbr,Hdr.DSTempHdrId from StdtSessStimuliActivity ACT INNER JOIN DSTempHdr Hdr ON ACT.DSTempHdrId=Hdr.DSTempHdrId where ACT.StudentId=" + sess.StudentId + " "
                             + "AND Hdr.LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE  DSTempHdrId=" + TempId + ")  ORDER BY ACT.DSTempHdrId";
                        Dt = objData.ReturnDataTable(strQuery, false);

                        val = (Dt.Rows.Count) - 1;
                        if (Dt != null)
                        {
                            if (Dt.Rows.Count > 0)
                            {
                                DataRow[] dtRw = Dt.Select("DSTempHdrId =" + TempId);
                                if (dtRw != null)
                                {
                                    if (dtRw.Length > 0)
                                    {
                                        if ((dtRw[0]["VerNbr"]) != DBNull.Value)
                                        {
                                            versionNum = Convert.ToDouble(dtRw[0]["VerNbr"]);
                                            DataRow[] dtRw2 = Dt.Select("VerNbr <= '" + versionNum + "'OR VerNbr is null "); //May-29-2020 Fix done
                                            //DataRow[] dtRw2 = Dt.Select("VerNbr <=" + versionNum + "OR VerNbr is null "); //May-29-2020 Above Fix for Cannot perform '<=' operation on System.String and System.Int32. in this string
                                            dt3 = dtRw2.CopyToDataTable();
                                            DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                            dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                            dt4Reason = dt4Reason.DefaultView.ToTable();
                                            dt4Reason.Columns.Remove("StimulyActivityId");
                                            dt4Reason.Columns.Remove("CreatedOn");
                                            dt4Reason.Columns.Remove("VerNbr");
                                            dt4Reason.Columns.Remove("DSTempHdrId");
                                            dt4Reason.Columns.Remove("Reason_New");
                                            foreach (DataRow dr in dt3.Rows)
                                            {
                                                if (dr["NAME"].ToString() == "STARTED" && dr["Reason_New"].ToString() != "")
                                                {
                                                    reason = "(Reason:" + dr["Reason_New"].ToString() + ")";
                                                    if (reason == "" || reason == "()")
                                                    {
                                                        reason = "(No Reason to display)";
                                                    }
                                                    createVal = dr["CreatedOn"].ToString();
                                                    string[] splitVal = createVal.Split(' ');
                                                    createVal = DateTime.Parse(splitVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');
                                                    DataRow dr4 = dt4.NewRow();
                                                    dr4["Date"] = createVal;
                                                    dr4["Reason"] = reason;
                                                    dt4.Rows.Add(dr4);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            dt3 = dtRw.CopyToDataTable();
                                            createVal = Dt.Rows[val]["CreatedOn"].ToString();
                                            string[] splitVal = createVal.Split(' ');
                                            createVal = splitVal[0];
                                            if (reason == "")
                                            {
                                                reason = "(Reason: Initial version. No reason to display)";
                                            }
                                            DataRow dr4 = dt4.NewRow();
                                            dr4["Date"] = "";
                                            dr4["Reason"] = reason;
                                            dt4.Rows.Add(dr4);
                                            DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                            dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                            dt4Reason = dt4Reason.DefaultView.ToTable();
                                            dt4Reason.Columns.Remove("StimulyActivityId");
                                            dt4Reason.Columns.Remove("CreatedOn");
                                            dt4Reason.Columns.Remove("VerNbr");
                                            dt4Reason.Columns.Remove("DSTempHdrId");
                                            dt4Reason.Columns.Remove("Reason_New");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
                    }
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
                fillDataLEssonPlan(LessonPlanId, TempId);

                SearchAndReplace(NewPath);

                AppndTableAssmtTool(NewPath, dt4Reason);
                AppendDateAndReason(NewPath, dt4);



                makeWord(NewPath, newpathz);
            }

            if ((rdoinprogress.Text == "In-progress" || rdoinprogress.Text =="Pending Approval") && chkpgrs == 1)
            {
                string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#PopDownload').css('top', '15%'); $('#PopDownload').show(); }); $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);               
                downloadfile();

            }


        }
        catch (Exception ex)
        {

        }

    }

    private void exportToWordSupportStrat(int LessonPlanId, int TempId)
    {
        /*----------------------------------------------------------------------------*/

        string Path = "";
        string NewPath = "";

        try
        {
            CreateQuery("NE", "..\\Administration\\LesonPlanTemplate\\SupportStrategyXML.xml");
            Path = Server.MapPath("~\\Administration\\LesonPlanTemplate\\AdultSupportStrategyPlanTemplate.docx");
            string Path2 = Server.MapPath("~\\Administration\\LessonPlanMerg\\SupportStrategyDummy.docx");

            string path = Server.MapPath("~\\Administration") + "\\TempLessonPlan";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            int fileCount = Directory.GetFiles(path).Length + 1;
            string newpathz = Server.MapPath("~\\Administration\\LessonPlanMerg\\");
            string subname = DateTime.Now.Date.ToShortDateString();
            if (subname.Contains('/'))
            {
                subname = subname.Replace('/', '-');
            }
            string LessonPlanName = "";
            string newFileName = "";
            object obj = objData.FetchValue("select DSTemplateName from dstemphdr WHERE  DSTempHdrId=" + TempId + "");
            object obj1 = objData.FetchValue("select VerNbr from dstemphdr WHERE  DSTempHdrId=" + TempId + "");
            string ver = obj1.ToString();
            if (obj != null)
            {
                LessonPlanName = obj.ToString();
                LessonPlanName = LessonPlanName.Replace(":", " ");
                LessonPlanName = LessonPlanName.Replace("*", " ");
                LessonPlanName = LessonPlanName.Replace(@"\", " ");
                LessonPlanName = LessonPlanName.Replace("|", " ");
                LessonPlanName = LessonPlanName.Replace("\"", " ");
                LessonPlanName = LessonPlanName.Replace("?", " ");
                LessonPlanName = LessonPlanName.Replace("/", " ");
                LessonPlanName = LessonPlanName.Replace("<", " ");
                LessonPlanName = LessonPlanName.Replace(">", " ");
            }

            if (chk == 1)
            {
                newFileName = "LessonPlan" + "_" + LessonPlanName + "_" + sess.StudentId + "-" + subname + "_" + fileCount;
            }
            else
            {
                newFileName = "LessonPlan" + "_" + LessonPlanName + ver + "_" + sess.StudentId + "-" + subname + "_" + fileCount;
            }
            ViewState["FileName2"] = newFileName;
            FileInfo f1 = new FileInfo(Path2);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpathz))
                {
                    Directory.CreateDirectory(newpathz);
                }

                f1.CopyTo(string.Format("{0}{1}{2}", newpathz, newFileName, f1.Extension));
            }

            NewPath = CopyTemplate(Path, "1");
            if (NewPath != "")
            {
                Dt = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt4Reason = new DataTable();
                dt4.Columns.Add("Date");
                dt4.Columns.Add("Reason");
                string reason = "";
                string createVal = "";
                try
                {
                    objData = new clsData();
                    int val = 0;
                    Double versionNum = 0.0;
                    DataTable Dt2 = new DataTable();

                    sess = (clsSession)Session["UserSession"];
                    if (sess != null)
                    {

                        strQuery = "select StimulyActivityId,case when(ActivitiType='STARTED') then ('STARTED') ELSE (case when(ActivitiType='SET') then (Select 'SET - '+SetCd from DSTempSet where  DSTempSetId= Act.ActivityId)"
                             + "ELSE (case when(ActivitiType='STEP') then (Select 'STEP - '+StepCd from DSTempStep where  DSTempStepId= Act.ActivityId)"
                             + "ELSE (case when(ActivitiType='MASTERED') then (Select 'SET - '+SetCd+'' from DSTempSet where  DSTempSetId= Act.ActivityId)"
                             + "ELSE(Select 'PROMPT - '+ LookupName from LookUp where  LookupId= Act.ActivityId)END)END)END)END NAME,"
                             + "(Convert(VARCHAR(50), StartTime,110))StartDate,(Convert(VARCHAR(50), DateMastered,110))DateMastered,(Convert(VARCHAR(50), DateClosed,110))DateClosed"
                             + ",Hdr.Reason_New,Hdr.CreatedOn,Hdr.VerNbr,Hdr.DSTempHdrId from StdtSessStimuliActivity ACT INNER JOIN DSTempHdr Hdr ON ACT.DSTempHdrId=Hdr.DSTempHdrId where ACT.StudentId=" + sess.StudentId + " "
                             + "AND Hdr.LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE  DSTempHdrId=" + TempId + ")  ORDER BY ACT.DSTempHdrId";
                        Dt = objData.ReturnDataTable(strQuery, false);

                        val = (Dt.Rows.Count) - 1;
                        if (Dt != null)
                        {
                            if (Dt.Rows.Count > 0)
                            {
                                DataRow[] dtRw = Dt.Select("DSTempHdrId =" + TempId);
                                if (dtRw != null)
                                {
                                    if (dtRw.Length > 0)
                                    {
                                        if ((dtRw[0]["VerNbr"]) != DBNull.Value)
                                        {
                                            versionNum = Convert.ToDouble(dtRw[0]["VerNbr"]);
                                            DataRow[] dtRw2 = Dt.Select("VerNbr <= '" + versionNum + "'OR VerNbr is null "); //May-29-2020 Fix done
                                            //DataRow[] dtRw2 = Dt.Select("VerNbr <=" + versionNum + "OR VerNbr is null "); //May-29-2020 Above Fix for Cannot perform '<=' operation on System.String and System.Int32. in this string
                                            dt3 = dtRw2.CopyToDataTable();
                                            DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                            dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                            dt4Reason = dt4Reason.DefaultView.ToTable();
                                            dt4Reason.Columns.Remove("StimulyActivityId");
                                            dt4Reason.Columns.Remove("CreatedOn");
                                            dt4Reason.Columns.Remove("VerNbr");
                                            dt4Reason.Columns.Remove("DSTempHdrId");
                                            dt4Reason.Columns.Remove("Reason_New");
                                            foreach (DataRow dr in dt3.Rows)
                                            {
                                                if (dr["NAME"].ToString() == "STARTED" && dr["Reason_New"].ToString() != "")
                                                {
                                                    reason = "(Reason:" + dr["Reason_New"].ToString() + ")";
                                                    if (reason == "" || reason == "()")
                                                    {
                                                        reason = "(No Reason to display)";
                                                    }
                                                    createVal = dr["CreatedOn"].ToString();
                                                    string[] splitVal = createVal.Split(' ');
                                                    createVal = DateTime.Parse(splitVal[0]).ToString("MM/dd/yyyy").Replace('-', '/');
                                                    DataRow dr4 = dt4.NewRow();
                                                    dr4["Date"] = createVal;
                                                    dr4["Reason"] = reason;
                                                    dt4.Rows.Add(dr4);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            dt3 = dtRw.CopyToDataTable();
                                            createVal = Dt.Rows[val]["CreatedOn"].ToString();
                                            string[] splitVal = createVal.Split(' ');
                                            createVal = splitVal[0];
                                            if (reason == "")
                                            {
                                                reason = "(Reason: Initial version. No reason to display)";
                                            }
                                            DataRow dr4 = dt4.NewRow();
                                            dr4["Date"] = "";
                                            dr4["Reason"] = reason;
                                            dt4.Rows.Add(dr4);
                                            DataRow[] dtRwReason = dt3.Select("NAME<>'STARTED'");
                                            dt4Reason = dtRwReason.CopyToDataTable();
                                            dt4Reason.DefaultView.Sort = "StimulyActivityId";
                                            dt4Reason = dt4Reason.DefaultView.ToTable();
                                            dt4Reason.Columns.Remove("StimulyActivityId");
                                            dt4Reason.Columns.Remove("CreatedOn");
                                            dt4Reason.Columns.Remove("VerNbr");
                                            dt4Reason.Columns.Remove("DSTempHdrId");
                                            dt4Reason.Columns.Remove("Reason_New");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
                    }
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
                fillDataLEssonPlan(LessonPlanId, TempId);

                SearchAndReplace(NewPath);

                AppndTableAssmtTool(NewPath, dt4Reason);
                AppendDateAndReason(NewPath, dt4);



                makeWord(NewPath, newpathz);
            }

            if ((rdoinprogress.Text == "In-progress" || rdoinprogress.Text == "Pending Approval") && chkpgrs == 1)
            {
                string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#PopDownload').css('top', '15%'); $('#PopDownload').show(); }); $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
                downloadfile();

            }


        }
        catch (Exception ex)
        {

        }

    }

    public void downloadfile()
    {
        ClsErrorLog err = new ClsErrorLog();
        try
        {
            string FileName = ViewState["FileName5"].ToString();
            string FileHeader = ViewState["FileName"].ToString() + ".doc";
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + FileHeader + "\"");
            byte[] data = req.DownloadData(FileName);
            Response.AddHeader("Content-Length", data.Length.ToString());
            response.BinaryWrite(data);
            //response.End();           
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            ClientScript.RegisterStartupScript(GetType(), "", "HideWait();", true);
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }

    }

    public void AppndTableAssmtTool(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                if (tablecounter == 1)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[2].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell4 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text())));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell5 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[3].ToString()))));
                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);
                        trXml.Append(tblCell4);
                        trXml.Append(tblCell5);

                        t.Append(trXml);

                    }
                }
                tablecounter++;
            }

            mainPart.Document.Save();

        }

    }

    public void AppendDateAndReason(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);
            Body bod = mainPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {
                if (tablecounter == 2)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        t.Append(trXml);
                    }
                }
                tablecounter++;
            }
            mainPart.Document.Save();
        }
    }

    private void fillDataLEssonPlan(int LessonPlanId, int TempId)
    {
        Dt = new DataTable();
        objData = new clsData();
        string correctRes = "";
        string inCorrecResp = "";
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {

            strQuery = "Select StudentLname +','+StudentFname from  dbo.Student where StudentId =" + sess.StudentId + "";
            objVal = objData.FetchValue(strQuery);
            if (objVal != null) columns[1] = objVal.ToString(); else columns[2] = "";


            if (sess.SchoolId == 1)
            {
                strQuery = "Select TOP 1 CONVERT(VARCHAR, LessonSDate, 101) +' to '+ CONVERT(VARCHAR,LessonEDate,101) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId = " + LessonPlanId + " ORDER BY DSTempHdrId DESC";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null)
                {
                    columns[2] = objVal.ToString();
                }
                else
                {
                    columns[2] = "";
                }
            }
            else if (sess.SchoolId == 2)
            {
                strQuery = "Select TOP 1 CONVERT(VARCHAR, LessonSDate, 101) +' to '+ CONVERT(VARCHAR,LessonEDate,101) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId = " + LessonPlanId + " ORDER BY DSTempHdrId DESC";
                objVal = objData.FetchValue(strQuery);
                if (objVal != null)
                {
                    columns[2] = objVal.ToString();
                }
                else
                {
                    columns[2] = "";
                }
            }
            strQuery = "SELECT GoalName FROM GoalLPRel glLpRel LEFT JOIN Goal gol ON glLpRel.GoalId = gol.GoalId WHERE  " +
                                                       " glLpRel.LessonPlanId = " + LessonPlanId + " AND glLpRel.ActiveInd = 'A'";
            strBinder = "";
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    string goal = Dr["GoalName"].ToString() + " , ";
                    strBinder += goal;
                }
            }
            columns[0] = strBinder.TrimEnd(',');

            string strQuerychk = "Select LU.LookupName from DSTempHdr DST Inner Join LookUp LU On LU.LookupId=DST.PromptTypeId  Where DSTempHdrId=" + TempId + " And LU.LookupType='Datasheet-Prompt Procedures'";
            objVal = objData.FetchValue(strQuerychk);
            if (objVal != null)
            {
                if ((objVal.ToString() == "Least-to-Most") || (objVal.ToString() == "Graduated Guidance"))
                {
                    strBinder = "";
                    strQuery = "select PromptId id from DSTempPrompt where DSTempHdrId=" + TempId + " ORDER BY PromptOrder";            //export selected prompt
                    Dt = objData.ReturnDataTable(strQuery, false);
                }
                else
                {
                    strBinder = "";
                    strQuery = "select PromptId id from DSTempPrompt where DSTempHdrId=" + TempId + " ORDER BY PromptOrder DESC";            //export selected prompt
                    Dt = objData.ReturnDataTable(strQuery, false);
                }
            }
            else
            {
                strBinder = "";
                strQuery = "select PromptId id from DSTempPrompt where DSTempHdrId=" + TempId + "";            //export selected prompt
                Dt = objData.ReturnDataTable(strQuery, false);
            }

            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    for (int j = 0; j < Dt.Rows.Count; j++)
                    {
                        strQuery = "select LookUp.LookupName from LookUp where LookupId=" + Convert.ToInt32(Dt.Rows[j]["id"]) + " ";
                        objVal = objData.FetchValue(strQuery);
                        if (objVal != null) strBinder += objVal.ToString() + "<w:br/>";
                    }
                }
            }
            string strBinder1 = "";

            string strQuery1 = "Select LU.LookUpName,LU.LookUpCode from DSTempHdr DST Inner Join LookUp LU On LU.LookupId=DST.TeachingProcId  Where DSTempHdrId=" + TempId + " And LU.LookupType='Datasheet-Teaching Procedures'";
            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["LookUpName"].ToString() == "Other" && Dt.Rows[0]["LookUpCode"].ToString() == "Other")
                    {
                        strBinder1 = "Other - ";
                    }
                    else
                    {
                        strQuery = "DECLARE @CHAINTYPE VARCHAR(20) SET @CHAINTYPE = (Select SkillType from DSTempHdr Where DSTempHdrId=" + TempId + ") SELECT 	CASE @CHAINTYPE WHEN 'Discrete' THEN 'Discrete Trial'	ELSE 'Task Analysis' END ";
                        objVal = objData.FetchValue(strQuery);

                        if (objVal != null) strBinder1 += objVal.ToString() + " - ";
                        if (objVal.ToString() == "Task Analysis")
                        {
                            // strQuery = "DECLARE @VAL int SET @VAL = (Select ChainType from DSTempHdr Where DSTempHdrId=" + TemplateId + ") SELECT 	CASE @VAL 		WHEN 1 THEN 'Forward chain' 		WHEN 2 THEN 'Backward chain' WHEN 3 THEN 'Total Task' 	ELSE '' END ";
                            strQuery = "Select ChainType from DSTempHdr Where DSTempHdrId=" + TempId + " ";
                            objVal = objData.FetchValue(strQuery);
                            if (objVal.ToString() != null) strBinder1 += objVal.ToString() + " - ";
                        }
                    }
                }
            }
            strQuery = "Select LU.LookupName from DSTempHdr DST Inner Join LookUp LU On LU.LookupId=DST.PromptTypeId  Where DSTempHdrId=" + TempId + " And LU.LookupType='Datasheet-Prompt Procedures'";
            objVal = objData.FetchValue(strQuery);
            if (objVal != null)
            {
                strBinder1 += objVal.ToString();
            }
            columns[7] = strBinder1 + "<w:br/>" + strBinder;

            strQuery = "select DSTemplateName AS LessonPlanName,LessonPlanGoal,FrameandStrand,SpecStandard,SpecEntryPoint,PreReq,Materials,VerNbr from DSTempHdr where DSTempHdrId=" + TempId + " ";
            Dt = new DataTable();
            Dt = objData.ReturnDataTable(strQuery, true);
            if (Dt.Rows.Count > 0)
            {
                columns[5] = Dt.Rows[0]["FrameandStrand"].ToString();
                columns[6] = Dt.Rows[0]["SpecStandard"].ToString();
                columns[4] = Dt.Rows[0]["SpecEntryPoint"].ToString();
                columns[9] = Dt.Rows[0]["PreReq"].ToString();
                columns[10] = Dt.Rows[0]["Materials"].ToString();
                columns[27] = Dt.Rows[0]["LessonPlanName"].ToString() + " " + Dt.Rows[0]["VerNbr"].ToString();
                columns[32] = Dt.Rows[0]["LessonPlanGoal"].ToString();
            }
            strQuery = "select TeachingProcId,SkillType,NbrOfTrials,ChainType,PromptTypeId,Baseline,Objective,GeneralProcedure,CorrRespDef,IncorrRespDef,LessonDefInst,"
                + " StudentReadCrita,TeacherRespReadness,ReinforcementProc,CorrectionProc,MajorSetting,MinorSetting,StudIncorrRespDef,StudCorrRespDef,Mistrial,MistrialResponse,TeacherPrepare,StudentPrepare,StudResponse"
                + " from DSTempHdr where DSTempHdrId = " + TempId + "";

            Dt = objData.ReturnDataTable(strQuery, true);
            if (Dt.Rows.Count > 0)
            {
                columns[13] = Dt.Rows[0]["LessonDefInst"].ToString();
                columns[16] = Dt.Rows[0]["ReinforcementProc"].ToString();
                columns[18] = Dt.Rows[0]["CorrectionProc"].ToString();
                columns[8] = Dt.Rows[0]["MajorSetting"].ToString() + " " + Dt.Rows[0]["MinorSetting"].ToString();
                columns[20] = Dt.Rows[0]["MistrialResponse"].ToString();
                columns[19] = Dt.Rows[0]["Mistrial"].ToString();
                columns[14] = Dt.Rows[0]["StudCorrRespDef"].ToString();
                columns[15] = Dt.Rows[0]["StudIncorrRespDef"].ToString();
                columns[26] = Dt.Rows[0]["Baseline"].ToString();
                columns[3] = Dt.Rows[0]["Objective"].ToString();
                columns[28] = Dt.Rows[0]["TeacherPrepare"].ToString();
                columns[29] = Dt.Rows[0]["StudentPrepare"].ToString();
                columns[30] = Dt.Rows[0]["StudResponse"].ToString();
                columns[31] = Dt.Rows[0]["GeneralProcedure"].ToString();

            }


            strQuery = "SELECT CorrRespDesc,InCorrRespDesc FROM DSTempSetCol WHERE DSTempHdrId = " + TempId + " AND ActiveInd = 'A'";
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Dt.Rows[i]["CorrRespDesc"].ToString() != "")
                        {
                            correctRes += Dt.Rows[i]["CorrRespDesc"].ToString() + ",";
                        }
                        if (Dt.Rows[i]["InCorrRespDesc"].ToString() != "")
                        {
                            inCorrecResp += Dt.Rows[i]["InCorrRespDesc"].ToString() + ",";
                        }

                    }
                    correctRes = correctRes.TrimEnd(',');
                    inCorrecResp = inCorrecResp.TrimEnd(',');

                    columns[12] = correctRes.ToString();
                    columns[17] = inCorrecResp.ToString();


                }
                else
                {
                    columns[12] = "No input Data";
                    columns[17] = "No input Data";
                }
            }
            else
            {
                columns[12] = "No input Data";
                columns[17] = "No input Data";
            }

            strBinder = "";
            strQuery = "SELECT StepName,StepCd FROM DSTempParentStep"
                        + " WHERE DSTempHdrId = " + TempId + " And ActiveInd = 'A' ORDER BY DSTempSetId,SortOrder";

            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<w:br/>" + Dr["StepCd"].ToString() + "-" + Dr["StepName"].ToString() + "<w:br/>";
                }
            }
            columns[22] = strBinder.ToString();
            strBinder = "";
            strQuery = "select SetName,SetCd from dbo.DSTempSet where DSTempHdrId = " + TempId + " AND ActiveInd = 'A' order by SortOrder";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<w:br/>" + Dr["SetCd"].ToString() + "-" + Dr["SetName"].ToString() + "<w:br/>";
                }
            }
            columns[21] = strBinder.ToString();


            strBinder = "";
            strQuery = "select ColTypeCd from DSTempSetCol where DSTempHdrId = " + TempId + " and ActiveInd ='A'";
            Dt = new DataTable();
            Dt = objData.ReturnDataTable(strQuery, false);

            strBinder = "";

            if (Dt != null)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    strBinder += "<w:br/>" + Dr["ColTypeCd"].ToString() + "<w:br/>";
                }
            }
            columns[11] = strBinder.ToString();
            FillCriteria(TempId.ToString());
            string[] criteria = Session["Criteria"].ToString().Split('|');
            columns[23] = criteria[0].ToString() + "<w:br/>" + criteria[1].ToString() + "<w:br/>" + criteria[2].ToString();
            columns[24] = criteria[3].ToString() + "<w:br/>" + criteria[4].ToString() + "<w:br/>" + criteria[5].ToString();
            columns[25] = criteria[6].ToString() + "<w:br/>" + criteria[7].ToString() + "<w:br/>" + criteria[8].ToString();
            //---[For removing ambersand ambersand , < , >]---
            int colcount = Convert.ToInt32(columns.Length.ToString());
            for (int icol = 0; icol < colcount; icol++)
            {
                if (columns[icol].Contains("&"))
                {
                    columns[icol] = columns[icol].ToString().Replace("&", " and "); 
                }
                if (columns[icol].Contains("<"))
                {
                    columns[icol] = columns[icol].ToString().Replace("<", " Less than ");
                }
                if (columns[icol].Contains(">"))
                {
                    columns[icol] = columns[icol].ToString().Replace(">", " Greater than ");
                }
                if (columns[icol].Contains("Less than w:br/ Greater than "))
                {
                    columns[icol] = columns[icol].ToString().Replace("Less than w:br/ Greater than ", " <w:br/> ");
                }
            }
            //---[For removing ambersand , < , >]---
        }
        else
        {
            //tdMsg.InnerHtml = clsGeneral.warningMsg("Session Expired...Please login again");
        }
    }
    public void SearchAndReplace(string document)
    {
        ClsErrorLog err = new ClsErrorLog();
        int m = 0;

        try
        {
            WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true);



            string docText = null;
            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();

                ;
            }

            string col = "";
            string plc = "";

            columnsCheck = new string[checkCount];


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
            wordDoc.Close();
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }


    }
    private string CopyTemplate(string oldPath, string PageNo)
    {
        try
        {
            ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
            sess = (clsSession)Session["UserSession"];
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\Administration") + "\\TempLessonPlan";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            int fileCount = Directory.GetFiles(path).Length + 1;
            string newpath = path + "\\";
            string subname = DateTime.Now.Date.ToShortDateString();
            if (subname.Contains('/'))
            {
                subname = subname.Replace('/', '-');
            }
            string newFileName = "LessonPlan" + "_" + sess.StudentId + "-" + subname + "_" + fileCount;
            ViewState["FileName"] = newFileName;
            FileInfo f1 = new FileInfo(oldPath);
            if (f1.Exists)
            {
                if (!Directory.Exists(newpath))
                {
                    Directory.CreateDirectory(newpath);
                }

                f1.CopyTo(string.Format("{0}{1}{2}", newpath, newFileName, f1.Extension));
            }

            //  ViewState["FileName"] = newpath + newFileName + f1.Extension;
            return newpath + newFileName + f1.Extension;
        }
        catch (Exception Ex)
        {            
            return "";
        }
    }
    private void CreateQuery(string StateName, string Path)
    {
        ClsErrorLog err = new ClsErrorLog();
        try
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

                    int i = 0, j = 0;
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        columns[i] = stMs.Attributes["Column"].Value;
                        i++;
                    }
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        placeHolders[j] = stMs.Attributes["PlaceHolder"].Value;

                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                        }
                        j++;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
    }
    public void makeWord(string filenamePass, string fileName1)
    {
        ClsErrorLog err = new ClsErrorLog();
        fileName1 += ViewState["FileName2"].ToString() + ".docx";
        try
        {
            using (WordprocessingDocument myDoc =
                WordprocessingDocument.Open(fileName1, true))
            {
                string altChunkId = "AltChunkId1";
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
            ViewState["FileName5"] = fileName1;
        }
        catch (Exception ex)
        {
            err.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
        }
    }


    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {

    }
    private void FillCriteria(string TempId)
    {
        advanceSet = "";
        advanceStep = "";
        advancePrompt = "";
        MoveBackSet = "";
        MoveBackStep = "";
        MoveBackPrompt = "";
        Session["Criteria"] = "";
        string caltype = string.Empty;
        int HdrId = 0;
        strQuery = "select DSTempSetColId,ColName from DSTempSetCol where DSTempHdrId = " + TempId + " and ActiveInd ='A'";
        objData = new clsData();
        DataTable DtSet = new DataTable();
        DtSet = objData.ReturnDataTable(strQuery, false);
        if (DtSet.Rows.Count > 0)
        {
            for (int i = 0; i < DtSet.Rows.Count; i++)
            {
                if (DtSet.Rows[0]["DSTempSetColId"] != null)
                {
                    strQuery = " SELECT  DR.RuleType, DR.CriteriaType, DR.TotalInstance, DR.TotCorrInstance,DST.IncMisTrialInd, DST.ColName,DR.CriteriaDetails," +
                               "DC.CalcType,DC.CalcLabel,Dt.DSTempHdrId, DR.ScoreReq,DR.ConsequetiveInd FROM DSTempHdr Dt " +
                               "INNER JOIN DSTempSetCol DST ON Dt.DSTempHdrId = DST.DSTempHdrId " +
                               "INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId " +
                               "INNER JOIN DSTempRule DR ON DR.DSTempSetColCalcId = DC.DSTempSetColCalcId " +
                               "WHERE DR.DSTempSetColId=" + DtSet.Rows[i]["DSTempSetColId"].ToString() + " AND DR.ActiveInd = 'A'";
                    Dt = new DataTable();
                    Dt = objData.ReturnDataTable(strQuery, false);                    

                    if (Dt != null)
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            if (HdrId == 0)
                            {
                                HdrId = Convert.ToInt32(Dt.Rows[0]["DSTempHdrId"]);
                            }

                            for (int j = 0; j < Dt.Rows.Count; j++)
                            {

                                if (Dt.Rows[j]["CalcLabel"].ToString() == "")
                                {
                                    caltype = Dt.Rows[j]["CalcType"].ToString();
                                }
                                else
                                {
                                    caltype = Dt.Rows[j]["CalcLabel"].ToString();
                                }

                                if (Dt.Rows[j]["CriteriaType"].ToString() == "MOVE UP")
                                {
                                    if (Dt.Rows[j]["RuleType"].ToString() == "PROMPT")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            //tdAdvPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            advancePrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                            //advancePrompt += "Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) ;
                                        }
                                        else
                                        {
                                            //tdAdvPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            advancePrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                            //advancePrompt += "Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) ;
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "SET")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            // tdAdvSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            advanceSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            //tdAdvSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            advanceSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "STEP")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            //tdAdvStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            advanceStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            //tdAdvStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            advanceStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                }
                                else if (Dt.Rows[j]["CriteriaType"].ToString() == "MOVE DOWN")
                                {
                                    if (Dt.Rows[j]["RuleType"].ToString() == "PROMPT")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            //tdMovePrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            // tdMovePrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "SET")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            //tdMoveSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            MoveBackSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            //tdMoveSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                    else if (Dt.Rows[j]["RuleType"].ToString() == "STEP")
                                    {
                                        if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                        {
                                            //tdMoveStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                            MoveBackStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                        else
                                        {
                                            //tdMoveStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                            MoveBackStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            strQuery = " Select DR.RuleType, DR.CriteriaType, DR.TotalInstance, DR.TotCorrInstance,DR.CriteriaDetails, " +
        " DR.ScoreReq,DR.ConsequetiveInd,Dt.DSTempHdrId  FROM DSTempHdr Dt INNER JOIN  DSTempRule DR On DR.DSTempHdrId  = Dt.DSTempHdrId " +
        " Where   DR.ActiveInd = 'A' AND DR.CriteriaType='MODIFICATION'  And DR.DSTempHdrId  =" + TempId + "  ";

            Dt = new DataTable();
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    for (int j = 0; j < Dt.Rows.Count; j++)
                    {
                        if (Dt.Rows[j]["CriteriaType"].ToString() == "MODIFICATION")
                        {
                            if (Dt.Rows[j]["RuleType"].ToString() == "PROMPT")
                            {
                                if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                {
                                    // tdModPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                    ModificationPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                                else
                                {
                                    //tdModPrompt.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationPrompt += "<w:br/>Prompt:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                            }
                            else if (Dt.Rows[j]["RuleType"].ToString() == "SET")
                            {
                                if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                {
                                    //tdModSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                                else
                                {
                                    //tdModSet.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<br/>";
                                    ModificationSet += "<w:br/>Set:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                            }
                            else if (Dt.Rows[j]["RuleType"].ToString() == "STEP")
                            {
                                if (Dt.Rows[j]["ConsequetiveInd"].ToString() == "False")
                                {
                                    //tdModStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                                else
                                {
                                    //tdModStep.InnerHtml += Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + " <br/>";
                                    ModificationStep += "<w:br/>Step:" + Convert.ToString(Dt.Rows[j]["CriteriaDetails"]) + "<w:br/>";
                                }
                            }


                        }
                    }
                }
            }
        }

        Session["Criteria"] = advanceSet + "|" + advanceStep + "|" + advancePrompt + "|" + MoveBackSet + "|" + MoveBackStep + "|" + MoveBackPrompt + "|" + ModificationSet + "|" + ModificationStep + "|" + ModificationPrompt;
    }
}
