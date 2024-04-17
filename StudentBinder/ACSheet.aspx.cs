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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using System.Net;
using System.Web.Services;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Data.SqlClient;
using OpenXmlPowerTools;
using Xceed.Words.NET;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Math;
using Xceed.Document.NET;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Runtime.InteropServices.ComTypes;
public partial class StudentBinder_ACSheet : System.Web.UI.Page
{
    static string[] columns;
    static string[] columnsToAdd;
    static string[] placeHolders;

    System.Data.DataTable Dt = null;
    System.Data.DataTable Dtcheck = null;
    System.Data.DataTable Dtcheck2 = null;
    clsData objData = null;
    clsSession sess = null;

    System.Data.DataTable dtAgendaItem = new System.Data.DataTable();
    System.Data.DataTable dtPMeeting = new System.Data.DataTable();
    System.Data.DataTable dtCMeeting = new System.Data.DataTable();

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
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
                btnUpdate.Visible = false;

                btnSaveNew.Visible = false;
                btnUpdateNew.Visible = false;

                //  btnLoadDataEdit.Visible = false;
                btnGenNewSheet0.Visible = false;
                btnImport.Visible = false;
                btnDelete.Visible = false;
                rbtnLsnClassTypeAc.Visible = false;
                tdMsg1.Visible = false;
                tdMsg2.Visible = false;
                tdreview2.Visible = false;
                gvAgendaItem.Visible = false;
                //btnBack.Visible = false;
                //    btnLoadData.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnUpdate.Visible = true;

                btnSaveNew.Visible = true;
                btnUpdateNew.Visible = true;

                //  btnLoadDataEdit.Visible = true;
                btnGenNewSheet0.Visible = true;
                //    btnImport.Visible = true;                
                //   }

                btnUpdate.Visible = false;
                btnImport.Visible = false;
                btnDelete.Visible = false;
                rbtnLsnClassTypeAc.Visible = false;
                tdMsg1.Visible = false;
                tdMsg2.Visible = false;
                tdreview2.Visible = false;
                gvAgendaItem.Visible = false;
                btnSaveNew.Visible = false;
                btnUpdateNew.Visible = false;
                //btnBack.Visible = true;
            }

            tdMsg.InnerHtml = "<span class='tdtext' style='color: #0D668E; font-family:times new roman;margin-left:20px;font-size:18px;'>Please Select a date to load the data</span>";
            MultiView1.ActiveViewIndex = 1;
            // FillStudent();
            FillData();
        }
    }


    private void setWritePermissions()
    {
        bool Disable = false;
        sess = (clsSession)Session["UserSession"];
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            btnSave.Visible = false;
            btnUpdate.Visible = false;

            btnSaveNew.Visible = false;
            btnUpdateNew.Visible = false;

            //  btnLoadDataEdit.Visible = false;
            btnGenNewSheet0.Visible = false;
            // btnImport.Visible = false;
            //btnBack.Visible = false;
            //    btnLoadData.Visible = false;
        }
        else
        {
            if (btnUpdate.Visible == true)
            {
                btnUpdate.Visible = true;
                btnSave.Visible = false;

                btnUpdateNew.Visible = true;
                btnSaveNew.Visible = false;
            }
            else
            {
                btnUpdate.Visible = false;
                btnSave.Visible = true;

                btnUpdateNew.Visible = false;
                btnSaveNew.Visible = true;
            }

            btnGenNewSheet0.Visible = true;
            //btnBack.Visible = true;
        }
    }


    protected void BtnGenerateAc_Click(object sender, EventArgs e)
    {
        AllInOne();



    }

    //protected void FillStudent()
    //{
    //    objData = new clsData();


    //    objData.ReturnDropDown("select StudentId as Id, StudentFName+' '+StudentLName as Name  from Student where ActiveInd='A'", ddlStudentEdit);
    //    Dt = new System.Data.DataTable();
    //    Dt = objData.ReturnDataTable("select distinct cast( DateOfMeeting as date) as Date from StdtAcdSheet", false);
    //    ddlDate.Items.Clear();
    //    foreach (DataRow dr in Dt.Rows)
    //    {
    //        ddlDate.Items.Add(DateTime.Parse(dr[0].ToString()).ToString("MM/dd/yyyy"));
    //    }
    //    if (ddlDate.Items.Count == 0)
    //    {
    //        ddlDate.Items.Add("---------------Select Date--------------");
    //    }
    //    else
    //    {
    //        ddlDate.Items.Insert(0, "---------------Select Date--------------");
    //    }

    //}

    protected void FillData()
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        
        //Dt = objData.ReturnDataTable("select convert(varchar(10),DateOfMeeting, 101)+'-'+convert(varchar(10),EndDate, 101) as EDate from StdtAcdSheet WHERE StudentId = " + sess.StudentId + " AND DateOfMeeting is NOT NULL AND EndDate is NOT NULL group BY DateOfMeeting, EndDate order by DateOfMeeting desc", false);

        System.Data.DataTable Dt = objData.ReturnDataTable("select year(EndDate) as EDate from StdtAcdSheet WHERE EndDate IS NOT NULL AND StudentId=" + sess.StudentId + " group by year(EndDate) order by year(EndDate) desc", false);
        Dataacdsheetlist.DataSource = Dt;
        Dataacdsheetlist.DataBind();

    }
    protected void Dataacdsheetlist_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label catId = e.Item.FindControl("lblAcCategory") as Label;
        DataList ListAcd = e.Item.FindControl("dlAcdate") as DataList;

        DataTable tblAcd = new DataTable();
        tblAcd = objData.ReturnDataTable("select convert(varchar(10),DateOfMeeting, 101)+'-'+convert(varchar(10),EndDate, 101) as EDate from StdtAcdSheet WHERE StudentId = " + sess.StudentId + " AND DateOfMeeting is NOT NULL AND EndDate is NOT NULL AND year(EndDate)=" + catId.Text.Substring(catId.Text.Length - 4) + " group BY DateOfMeeting, EndDate order by DateOfMeeting desc", false);

        ListAcd.DataSource = tblAcd;
        ListAcd.DataBind();
    }
    private void loadDataList()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();

        //select * from StdtLessonPlan inner join LessonPlan on StdtLessonPlan.LessonPlanId=LessonPlan.LessonPlanId inner join Goal on StdtLessonPlan.GoalId=Goal.GoalId where StdtLessonPlan.StudentId=1
        //select LessonPlan.LessonPlanName,Goal.GoalName,StdtLessonPlan.Objective3,(select LookupName from LookUp where  LookupId=DSTempHdr.TeachingProcId ) as 'Type Of Instruction' from StdtLessonPlan inner join LessonPlan on StdtLessonPlan.LessonPlanId=LessonPlan.LessonPlanId inner join Goal on StdtLessonPlan.GoalId=Goal.GoalId inner join DSTempHdr on StdtLessonPlan.LessonPlanId=DSTempHdr.LessonPlanId where StdtLessonPlan.StudentId=2 and StdtLessonPlan.SchoolId=1 and DSTempHdr.SchoolId=1 and DSTempHdr.StudentId=2 and DSTempHdr.StatusId=(select LookupId from LookUp where LookupName='Approved'  and LookupType='TemplateStatus')  and StdtLessonPlan.ActiveInd='A'
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

        DateTime dateNow1 = dtst;
        DateTime dateNow2 = dateNow1.AddDays(+14);
        DateTime dateNow3 = dateNow2.AddDays(+14);
        DateTime dateNow4 = dateNow3.AddDays(+14);
        DateTime dateNow5 = dateNow4.AddDays(+14);
        DateTime dateNow6 = dateNow5.AddDays(+14);
        DateTime dateNow7 = dateNow6.AddDays(+14);
        DateTime dateNow8 = dateNow7.AddDays(+14);
        //DateTime dateNow8 = dted;
        //DateTime dateNow7 = dateNow8.AddDays(-14);
        //DateTime dateNow6 = dateNow7.AddDays(-14);
        //DateTime dateNow5 = dateNow6.AddDays(-14);
        //DateTime dateNow4 = dateNow5.AddDays(-14);
        //DateTime dateNow3 = dateNow4.AddDays(-14);
        //DateTime dateNow2 = dateNow3.AddDays(-14);
        //DateTime dateNow1 = dateNow2.AddDays(-14);

        /// IEP data not needs for selecting Active LP
        /// 
        //string query = "";
        //if (sess.SchoolId == 1)                                 ///load only latest IEP data for the  coversheet --jis
        //{
        //    query = "select max(StdtIEPId) from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND (LookupName='Approved' OR LookupName='Expired'))";
        //}
        //else
        //{
        //    query = "select max(StdtIEP_PEId) from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND (LookupName='Approved' OR LookupName='Expired'))";
        //}

        //object objIEPid = objData.FetchValue(query);
        //int IEPId = 0;
        //if (objIEPid != null && objIEPid.ToString() != "")
        //{
        //    IEPId = Convert.ToInt32(objIEPid);
        //}
        ///end
        ///


        //        string qry = "select distinct DSTempHdr.DSTempHdrId,DSTempHdr.DSTemplateName AS LessonPlanName,DSTempHdr.LessonPlanId,DSTempHdr.VerNbr,DSTempHdr.LessonOrder,Goal.GoalName,StdtLessonPlan.Objective3," +
        //"(select LookupCode from LookUp where  LookupId=DSTempHdr.TeachingProcId )+'('+(select LookupName from LookUp where  LookupId=DSTempHdr.PromptTypeId )+')' as 'TypeOfInstruction'," +

        //"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer1'," +
        //"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
        //"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel1'," +
        //"(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
        //"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set1'," +

        //"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer2'," +
        //"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
        //"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel2'," +
        //"(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
        //"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set2'," +

        //"(SELECT TOP 1 LessonOrder from DSTempHdr where LessonPlanId = dbo.StdtLessonPlan.LessonPlanId AND DSTempHdr.StudentId = '" + sess.StudentId + "' ORDER BY LessonOrder) AS 'LessonOrder',"+

        //"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer3'," +
        //"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
        //"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel3'," +
        //"(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
        //"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set3'," +

        //"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer4'," +
        //"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
        //"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel4'," +
        //"(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
        //"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set4'," +

        //"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer5'," +
        //"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
        //"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel5'," +
        //"(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
        //"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set5'," +

        //"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer6'," +
        //"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
        //"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel6'," +
        //"(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
        //"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set6'," +

        //"(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer7'," +
        //"(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr " +
        //"where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel7'," +
        //"(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' " +
        //"from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=DSTempHdr.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set7'," +

        //"(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow1.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND " +
        // "CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=DSTempHdr.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),NoofTimesTried)) AS NUM1 ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND " +
        // "CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=DSTempHdr.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),NoofTimesTried)) AS NUM2 ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND " +
        // "CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=DSTempHdr.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),NoofTimesTried)) AS NUM3 ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND " +
        // "CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=DSTempHdr.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),NoofTimesTried)) AS NUM4 ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND " +
        // "CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=DSTempHdr.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),NoofTimesTried)) AS NUM5 ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND " +
        // "CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=DSTempHdr.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),NoofTimesTried)) AS NUM6 ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND " +
        // "CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=DSTempHdr.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),NoofTimesTried)) AS NUM7 " +



        //"from StdtLessonPlan inner join LessonPlan on StdtLessonPlan.LessonPlanId=LessonPlan.LessonPlanId inner join Goal on StdtLessonPlan.GoalId=Goal.GoalId " +
        //"inner join DSTempHdr on StdtLessonPlan.LessonPlanId=DSTempHdr.LessonPlanId where StdtLessonPlan.StudentId=" + sess.StudentId + " and StdtLessonPlan.SchoolId=" + sess.SchoolId + " and " +
        //"DSTempHdr.SchoolId=" + sess.SchoolId + " and DSTempHdr.StudentId=" + sess.StudentId + " and " +
        //" StdtLessonPlan.ActiveInd='A' AND StdtLessonPlan.StdtIEPId=" + IEPId + " AND StdtLessonPlan.IncludeIEP=1 ORDER BY DSTempHdr.LessonOrder";

        ///select all Lessonplan that are “Active” status
        ///
        //string qry = "SELECT (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') DSTempHdrId,(SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC) LessonPlanName ,LessonPlanId,(SELECT MAX(VerNbr) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') VerNbr,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC) LessonOrder,(SELECT GoalName FROM Goal WHERE GoalId=StdtLessonPlan.GoalId) GoalName " +
        //             ",Objective3,(select LookupCode from LookUp where  LookupId=(SELECT TOP 1 TeachingProcId FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC) )+'('+(select LookupName from LookUp where  LookupId=(SELECT TOP 1 PromptTypeId FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC) )+')' as 'TypeOfInstruction',(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn) " +
        //             " between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer1',(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel1',(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') " +
        //             " and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set1',(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer2' ,(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') " +
        //             " and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel2',(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set2',(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') " +
        //             " and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer3',(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel3',(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') " +
        //             " and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set3',(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer4',(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') " +
        //             " and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel4',(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set4',(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and " +
        //             " CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer5',(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel5',(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and " +
        //             " CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set5',(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer6',(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and " +
        //             " CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel6',(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set6',(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= (SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and " +
        //             " CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer7',(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel7',(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and " +
        //             " CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set7',(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow1.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT TOP 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC))) AS NUM1 " +
        //             ",(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow1.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS1 " +
        //             " ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT TOP 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC))) AS NUM2 " +
        //             ",(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS2 " +
        //             ",(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS3 " +
        //             ",(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS4 " +
        //             ",(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS5 " +
        //             ",(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS6 " +
        //             ",(SELECT TOP 1 (select CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step1' " +
        //             ",(SELECT TOP 1 (select CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step2' " +
        //             ",(SELECT TOP 1 (select CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step3' " +
        //             ",(SELECT TOP 1 (select CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step4' " +
        //             ",(SELECT TOP 1 (select CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step5' " +
        //             ",(SELECT TOP 1 (select CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step6' " +
        //             ",(SELECT TOP 1 (select CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step7' " +
        //             ",(SELECT TOP 1 (select DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) FROM  StdtSessionHdr WHERE StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid1' " +
        //             ",(SELECT TOP 1 (select DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid2' " +
        //             ",(SELECT TOP 1 (select DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid3' " +
        //             ",(SELECT TOP 1 (select DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid4' " +
        //             ",(SELECT TOP 1 (select DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid5' " +
        //             ",(SELECT TOP 1 (select DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid6' " +
        //             ",(SELECT TOP 1 (select DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=(SELECT MAX(DSTempHdrId) FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "') and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid7' " +
        //             ",(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS7, (SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') " +
        //             " AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT TOP 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC))) AS NUM3  ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT TOP 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC))) AS NUM4 " +
        //             " ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT TOP 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC))) AS NUM5  ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') " +
        //             " AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT TOP 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC))) AS NUM6 ,(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=StdtLessonPlan.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT TOP 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=StdtLessonPlan.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY DSTempHdrId DESC))) AS NUM7 " +
        //             " from StdtLessonPlan where StudentId='" + sess.StudentId + "' and ActiveInd='A' and StdtIEPId=" + IEPId + " and IncludeIEP=1 ";


        string qry = "SELECT DISTINCT HDR.DSTempHdrId, HDR.DSTemplateName AS LessonPlanName, HDR.LessonPlanId, HDR.VerNbr, HDR.LessonOrder," +

                "(STUFF((  SELECT ', ' + G.GoalName FROM Goal G INNER JOIN GoalLPRel GLP ON GLP.GoalId=G.GoalId WHERE GLP.LessonPlanId=HDR.LessonPlanId FOR XML PATH('')), 1, 2, '')) AS GoalName,"+
                
                "(SELECT TOP 1 Objective3 FROM StdtLessonPlan WHERE GoalId=GLP.GoalId AND LessonPlanId=GLP.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY StdtIEPId DESC) AS Objective3, " +
                "(select LookupCode from LookUp where  LookupId= (SELECT TeachingProcId FROM DSTempHdr WHERE DSTempHdr.DSTempHdrId= HDR.DSTempHdrId ))+'('+(select LookupName from LookUp where  LookupId=(SELECT PromptTypeId FROM DSTempHdr WHERE DSTempHdr.DSTempHdrId=HDR.DSTempHdrId ) )+')' as 'TypeOfInstruction', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn) between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer1' , " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId AND CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel1', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set1', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer2', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel2', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set2', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer3', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel3', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set3', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer4', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel4', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set4', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer5', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel5', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set5', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer6', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel6', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set6', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer7', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel7', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set7', " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow1.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM1, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow1.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS1, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM2, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS2, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS3, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS4, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS5, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS6, " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step1', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step2', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step3', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step4', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step5', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step6', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step7', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) FROM  StdtSessionHdr WHERE StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid1', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid2', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid3', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid4', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid5', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid6', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid7', " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS7, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM3, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM4, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM5, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM6, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM7 " +
                "FROM DSTempHdr HDR INNER JOIN GoalLPRel GLP ON HDR.LessonPlanId= GLP.LessonPlanId INNER JOIN Goal G ON GLP.GoalId = G.GoalId WHERE HDR.StudentId='" + sess.StudentId + "' AND HDR.StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName in('Approved','Maintenance')) ";

        ///end
        ///
        string strQuery = qry;
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                GridViewAccSheet.DataSource = Dt;
                GridViewAccSheet.DataBind();
                btnSave.Visible = true;
                btnSaveNew.Visible = true;
                rbtnLsnClassTypeAc.Visible = true;
                tdMsg1.Visible = true;
                tdMsg2.Visible = true;
                tdreview2.Visible = true;
                gvAgendaItem.Visible = true;
                btnUpdate.Visible = false;
                btnUpdateNew.Visible = false;
                btnImport.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
                GridViewAccSheet.DataSource = null;
                GridViewAccSheet.DataBind();
                btnSave.Visible = false;
                btnSaveNew.Visible = false;
                rbtnLsnClassTypeAc.Visible = false;
                tdMsg1.Visible = false;
                tdMsg2.Visible = false;
                tdreview2.Visible = false;
                gvAgendaItem.Visible = false;
                btnUpdate.Visible = false;
                btnUpdateNew.Visible = false;
                btnImport.Visible = false;
                btnDelete.Visible = false;
            }
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
            GridViewAccSheet.DataSource = null;
            GridViewAccSheet.DataBind();
            btnSave.Visible = false;
            btnSaveNew.Visible = false;
            rbtnLsnClassTypeAc.Visible = false;
            tdMsg1.Visible = false;
            tdMsg2.Visible = false;
            tdreview2.Visible = false;
            gvAgendaItem.Visible = false;
            btnUpdate.Visible = false;
            btnUpdateNew.Visible = false;
            btnImport.Visible = false;
            btnDelete.Visible = false;
        }
    }



    protected void lnkDate_Click(object source, DataListCommandEventArgs e)
    {
        objData = new clsData();
        string dateVal = "";
        tdMsg.InnerHtml = "";
        rbtnLsnClassTypeAc.Visible = true;
        tdMsg1.Visible = true;
        tdMsg2.Visible = true;
        tdreview2.Visible = true;
        gvAgendaItem.Visible = true;
        rbtnLsnClassTypeAc.SelectedValue = "Day,Residence";
        ///LinkButton lnkDate = (LinkButton)sender;
        try
        {
            dateVal = e.CommandArgument.ToString();
            ViewState["CurrentDate"] = dateVal;
            LoadPMeetingGV();
            LoadCMeetingGV();
            loadAgendaItemGV();
            loadDataList(dateVal);
            LoadMeetings(dateVal);
            MultiView1.ActiveViewIndex = 1;             ///Set multiview 1 view(update button for already saved academic sheets) --jis
            btnUpdate.Visible = true;
            btnUpdateNew.Visible = true;
            setWritePermissions();

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    private void loadDataList(string date)
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
        Dt = new System.Data.DataTable();
        Dtcheck = new System.Data.DataTable();
        Dtcheck2 = new System.Data.DataTable();
        //        string qry = "select StdtAcdSheet.AccSheetId,StdtAcdSheet.StudentId,StdtAcdSheet.DateOfMeeting,StdtAcdSheet.EndDate,StdtAcdSheet.GoalArea,StdtAcdSheet.Goal,StdtAcdSheet.Benchmarks,StdtAcdSheet.FeedBack,StdtAcdSheet.PreposalDiss, StdtAcdSheet.PersonResNdDeadline,StdtAcdSheet.TypeOfInstruction,"+
        //"StdtAcdSheet.Period1,StdtAcdSheet.Set1,StdtAcdSheet.Prompt1,StdtAcdSheet.IOA1,StdtAcdSheet.NoOfTimes1,StdtAcdSheet.Period2,StdtAcdSheet.Set2,StdtAcdSheet.Prompt2,StdtAcdSheet.IOA2,StdtAcdSheet.NoOfTimes2,StdtAcdSheet.Period3,StdtAcdSheet.Set3,StdtAcdSheet.Prompt3,StdtAcdSheet.IOA3,"+
        //"StdtAcdSheet.NoOfTimes3,StdtAcdSheet.Period4,StdtAcdSheet.Set4,StdtAcdSheet.Prompt4,StdtAcdSheet.IOA4,StdtAcdSheet.NoOfTimes4,StdtAcdSheet.Period5,StdtAcdSheet.Set5,StdtAcdSheet.Prompt5,StdtAcdSheet.IOA5,StdtAcdSheet.NoOfTimes5,StdtAcdSheet.Period6,StdtAcdSheet.Set6,StdtAcdSheet.Prompt6,"+
        //"StdtAcdSheet.IOA6,StdtAcdSheet.NoOfTimes6,StdtAcdSheet.Period7,StdtAcdSheet.Set7,StdtAcdSheet.Prompt7,StdtAcdSheet.IOA7,StdtAcdSheet.NoOfTimes7,StdtAcdSheet.LessonPlanId,DSTempHdr.LessonOrder"+
        //  "  from StdtAcdSheet inner join DSTempHdr on StdtAcdSheet.StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)='" + endDate + "' AND StdtAcdSheet.LessonPlanId = DSTempHdr.LessonPlanId ORDER BY DSTempHdr.LessonOrder ";
        string qry = "";
        if (rbtnLsnClassTypeAc.SelectedItem.Text == "Day")
        {
            qry = "SELECT * FROM (select StdtAcdSheet.AccSheetId,StdtAcdSheet.StudentId,StdtAcdSheet.DateOfMeeting,StdtAcdSheet.EndDate,StdtAcdSheet.GoalArea," +
                  "StdtAcdSheet.Goal,StdtAcdSheet.Benchmarks,StdtAcdSheet.FeedBack,StdtAcdSheet.PreposalDiss, StdtAcdSheet.PersonResNdDeadline," +
                  "StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.Period1,StdtAcdSheet.Set1,StdtAcdSheet.Prompt1,StdtAcdSheet.IOA1,StdtAcdSheet.NoOfTimes1,StdtAcdSheet.Progressing1,StdtAcdSheet.Mistrial1,StdtAcdSheet.Step1 AS stepId1,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step1) step1," +
                  "StdtAcdSheet.Period2,StdtAcdSheet.Set2,StdtAcdSheet.Prompt2,StdtAcdSheet.IOA2,StdtAcdSheet.NoOfTimes2,StdtAcdSheet.Progressing2,StdtAcdSheet.Mistrial2,StdtAcdSheet.Step2 AS stepId2,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step2) step2,StdtAcdSheet.Period3,StdtAcdSheet.Set3," +
                  "StdtAcdSheet.Prompt3,StdtAcdSheet.IOA3,StdtAcdSheet.NoOfTimes3,StdtAcdSheet.Progressing3,StdtAcdSheet.Mistrial3,StdtAcdSheet.Step3 AS stepId3,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step3) step3,StdtAcdSheet.Period4,StdtAcdSheet.Set4,StdtAcdSheet.Prompt4,StdtAcdSheet.IOA4," +
                  "StdtAcdSheet.NoOfTimes4,StdtAcdSheet.Progressing4,StdtAcdSheet.Mistrial4,StdtAcdSheet.Step4 AS stepId4,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step4) step4,StdtAcdSheet.Period5,StdtAcdSheet.Set5,StdtAcdSheet.Prompt5,StdtAcdSheet.IOA5,StdtAcdSheet.NoOfTimes5,StdtAcdSheet.Progressing5,StdtAcdSheet.Mistrial5,StdtAcdSheet.Step5 AS stepId5," +
                  "(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step5) step5,StdtAcdSheet.Period6,StdtAcdSheet.Set6,StdtAcdSheet.Prompt6,StdtAcdSheet.IOA6,StdtAcdSheet.NoOfTimes6,StdtAcdSheet.Progressing6,StdtAcdSheet.Mistrial6,StdtAcdSheet.Step6 AS stepId6,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step6) step6,StdtAcdSheet.Period7,StdtAcdSheet.Set7" +
                  ",StdtAcdSheet.Prompt7,StdtAcdSheet.IOA7,StdtAcdSheet.NoOfTimes7,StdtAcdSheet.Progressing7,StdtAcdSheet.Mistrial7,StdtAcdSheet.Step7 AS stepId7,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step7) step7,StdtAcdSheet.LessonPlanId,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE DSTempHdr.LessonPlanId" +
                  "=StdtAcdSheet.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + ") LessonOrder,StdtAcdSheet.MetObjective,StdtAcdSheet.MetGoal,StdtAcdSheet.NotMaintaining  from StdtAcdSheet " +
                  "WHERE StdtAcdSheet.StudentId=" + sess.StudentId + " and StdtAcdSheet.LessonPlanId in (select s.LessonPlanId from StdtSessionHdr s join Class c on c.ClassId=s.StdtClassId where c.ResidenceInd=0) and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)" +
                  "='" + endDate + "') STDTACSHT ORDER BY LessonOrder";
        }
        if (rbtnLsnClassTypeAc.SelectedItem.Text == "Residence")
        {
            qry = "SELECT * FROM (select StdtAcdSheet.AccSheetId,StdtAcdSheet.StudentId,StdtAcdSheet.DateOfMeeting,StdtAcdSheet.EndDate,StdtAcdSheet.GoalArea," +
                  "StdtAcdSheet.Goal,StdtAcdSheet.Benchmarks,StdtAcdSheet.FeedBack,StdtAcdSheet.PreposalDiss, StdtAcdSheet.PersonResNdDeadline," +
                  "StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.Period1,StdtAcdSheet.Set1,StdtAcdSheet.Prompt1,StdtAcdSheet.IOA1,StdtAcdSheet.NoOfTimes1,StdtAcdSheet.Progressing1,StdtAcdSheet.Mistrial1,StdtAcdSheet.Step1 AS stepId1,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step1) step1," +
                  "StdtAcdSheet.Period2,StdtAcdSheet.Set2,StdtAcdSheet.Prompt2,StdtAcdSheet.IOA2,StdtAcdSheet.NoOfTimes2,StdtAcdSheet.Progressing2,StdtAcdSheet.Mistrial2,StdtAcdSheet.Step2 AS stepId2,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step2) step2,StdtAcdSheet.Period3,StdtAcdSheet.Set3," +
                  "StdtAcdSheet.Prompt3,StdtAcdSheet.IOA3,StdtAcdSheet.NoOfTimes3,StdtAcdSheet.Progressing3,StdtAcdSheet.Mistrial3,StdtAcdSheet.Step3 AS stepId3,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step3) step3,StdtAcdSheet.Period4,StdtAcdSheet.Set4,StdtAcdSheet.Prompt4,StdtAcdSheet.IOA4," +
                  "StdtAcdSheet.NoOfTimes4,StdtAcdSheet.Progressing4,StdtAcdSheet.Mistrial4,StdtAcdSheet.Step4 AS stepId4,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step4) step4,StdtAcdSheet.Period5,StdtAcdSheet.Set5,StdtAcdSheet.Prompt5,StdtAcdSheet.IOA5,StdtAcdSheet.NoOfTimes5,StdtAcdSheet.Progressing5,StdtAcdSheet.Mistrial5,StdtAcdSheet.Step5 AS stepId5," +
                  "(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step5) step5,StdtAcdSheet.Period6,StdtAcdSheet.Set6,StdtAcdSheet.Prompt6,StdtAcdSheet.IOA6,StdtAcdSheet.NoOfTimes6,StdtAcdSheet.Progressing6,StdtAcdSheet.Mistrial6,StdtAcdSheet.Step6 AS stepId6,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step6) step6,StdtAcdSheet.Period7,StdtAcdSheet.Set7" +
                  ",StdtAcdSheet.Prompt7,StdtAcdSheet.IOA7,StdtAcdSheet.NoOfTimes7,StdtAcdSheet.Progressing7,StdtAcdSheet.Mistrial7,StdtAcdSheet.Step7 AS stepId7,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step7) step7,StdtAcdSheet.LessonPlanId,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE DSTempHdr.LessonPlanId" +
                  "=StdtAcdSheet.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + ") LessonOrder,StdtAcdSheet.MetObjective,StdtAcdSheet.MetGoal,StdtAcdSheet.NotMaintaining  from StdtAcdSheet " +
                  "WHERE StdtAcdSheet.StudentId=" + sess.StudentId + " and StdtAcdSheet.LessonPlanId in (select s.LessonPlanId from StdtSessionHdr s join Class c on c.ClassId=s.StdtClassId where c.ResidenceInd=1) and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)" +
                  "='" + endDate + "') STDTACSHT ORDER BY LessonOrder";
        }
        if (rbtnLsnClassTypeAc.SelectedItem.Text == "Both")
        {
            qry = "SELECT * FROM (select StdtAcdSheet.AccSheetId,StdtAcdSheet.StudentId,StdtAcdSheet.DateOfMeeting,StdtAcdSheet.EndDate,StdtAcdSheet.GoalArea," +
                  "StdtAcdSheet.Goal,StdtAcdSheet.Benchmarks,StdtAcdSheet.FeedBack,StdtAcdSheet.PreposalDiss, StdtAcdSheet.PersonResNdDeadline," +
                  "StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.Period1,StdtAcdSheet.Set1,StdtAcdSheet.Prompt1,StdtAcdSheet.IOA1,StdtAcdSheet.NoOfTimes1,StdtAcdSheet.Progressing1,StdtAcdSheet.Mistrial1,StdtAcdSheet.Step1 AS stepId1,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step1) step1," +
                  "StdtAcdSheet.Period2,StdtAcdSheet.Set2,StdtAcdSheet.Prompt2,StdtAcdSheet.IOA2,StdtAcdSheet.NoOfTimes2,StdtAcdSheet.Progressing2,StdtAcdSheet.Mistrial2,StdtAcdSheet.Step2 AS stepId2,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step2) step2,StdtAcdSheet.Period3,StdtAcdSheet.Set3," +
                  "StdtAcdSheet.Prompt3,StdtAcdSheet.IOA3,StdtAcdSheet.NoOfTimes3,StdtAcdSheet.Progressing3,StdtAcdSheet.Mistrial3,StdtAcdSheet.Step3 AS stepId3,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step3) step3,StdtAcdSheet.Period4,StdtAcdSheet.Set4,StdtAcdSheet.Prompt4,StdtAcdSheet.IOA4," +
                  "StdtAcdSheet.NoOfTimes4,StdtAcdSheet.Progressing4,StdtAcdSheet.Mistrial4,StdtAcdSheet.Step4 AS stepId4,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step4) step4,StdtAcdSheet.Period5,StdtAcdSheet.Set5,StdtAcdSheet.Prompt5,StdtAcdSheet.IOA5,StdtAcdSheet.NoOfTimes5,StdtAcdSheet.Progressing5,StdtAcdSheet.Mistrial5,StdtAcdSheet.Step5 AS stepId5," +
                  "(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step5) step5,StdtAcdSheet.Period6,StdtAcdSheet.Set6,StdtAcdSheet.Prompt6,StdtAcdSheet.IOA6,StdtAcdSheet.NoOfTimes6,StdtAcdSheet.Progressing6,StdtAcdSheet.Mistrial6,StdtAcdSheet.Step6 AS stepId6,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step6) step6,StdtAcdSheet.Period7,StdtAcdSheet.Set7" +
                  ",StdtAcdSheet.Prompt7,StdtAcdSheet.IOA7,StdtAcdSheet.NoOfTimes7,StdtAcdSheet.Progressing7,StdtAcdSheet.Mistrial7,StdtAcdSheet.Step7 AS stepId7,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step7) step7,StdtAcdSheet.LessonPlanId,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE DSTempHdr.LessonPlanId" +
                  "=StdtAcdSheet.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + ") LessonOrder,StdtAcdSheet.MetObjective,StdtAcdSheet.MetGoal,StdtAcdSheet.NotMaintaining  from StdtAcdSheet " +
                  "WHERE StdtAcdSheet.StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)" +
                  "='" + endDate + "') STDTACSHT ORDER BY LessonOrder";
        }

//        qry = "SELECT * FROM (select StdtAcdSheet.AccSheetId,StdtAcdSheet.StudentId,StdtAcdSheet.DateOfMeeting,StdtAcdSheet.EndDate,StdtAcdSheet.GoalArea," +
//"StdtAcdSheet.Goal,StdtAcdSheet.Benchmarks,StdtAcdSheet.FeedBack,StdtAcdSheet.PreposalDiss, StdtAcdSheet.PersonResNdDeadline," +
//"StdtAcdSheet.TypeOfInstruction,StdtAcdSheet.Period1,StdtAcdSheet.Set1,StdtAcdSheet.Prompt1,StdtAcdSheet.IOA1,StdtAcdSheet.NoOfTimes1,StdtAcdSheet.Mistrial1,StdtAcdSheet.Step1 AS stepId1,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step1) step1," +
//"StdtAcdSheet.Period2,StdtAcdSheet.Set2,StdtAcdSheet.Prompt2,StdtAcdSheet.IOA2,StdtAcdSheet.NoOfTimes2,StdtAcdSheet.Mistrial2,StdtAcdSheet.Step2 AS stepId2,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step2) step2,StdtAcdSheet.Period3,StdtAcdSheet.Set3," +
//"StdtAcdSheet.Prompt3,StdtAcdSheet.IOA3,StdtAcdSheet.NoOfTimes3,StdtAcdSheet.Mistrial3,StdtAcdSheet.Step3 AS stepId3,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step3) step3,StdtAcdSheet.Period4,StdtAcdSheet.Set4,StdtAcdSheet.Prompt4,StdtAcdSheet.IOA4," +
//"StdtAcdSheet.NoOfTimes4,StdtAcdSheet.Mistrial4,StdtAcdSheet.Step4 AS stepId4,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step4) step4,StdtAcdSheet.Period5,StdtAcdSheet.Set5,StdtAcdSheet.Prompt5,StdtAcdSheet.IOA5,StdtAcdSheet.NoOfTimes5,StdtAcdSheet.Mistrial5,StdtAcdSheet.Step5 AS stepId5," +
//"(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step5) step5,StdtAcdSheet.Period6,StdtAcdSheet.Set6,StdtAcdSheet.Prompt6,StdtAcdSheet.IOA6,StdtAcdSheet.NoOfTimes6,StdtAcdSheet.Mistrial6,StdtAcdSheet.Step6 AS stepId6,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step6) step6,StdtAcdSheet.Period7,StdtAcdSheet.Set7" +
//",StdtAcdSheet.Prompt7,StdtAcdSheet.IOA7,StdtAcdSheet.NoOfTimes7,StdtAcdSheet.Mistrial7,StdtAcdSheet.Step7 AS stepId7,(SELECT CASE WHEN StepCd IS NULL THEN StepName ELSE StepCd END FROM DSTempStep WHERE DSTempStepId=StdtAcdSheet.Step7) step7,StdtAcdSheet.LessonPlanId,(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE DSTempHdr.LessonPlanId" +
//"=StdtAcdSheet.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + ") LessonOrder  from StdtAcdSheet " +
//"WHERE StdtAcdSheet.StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)" +
//"='" + endDate + "') STDTACSHT ORDER BY LessonOrder";
        string strQuery = qry;
        Dt = objData.ReturnDataTable(strQuery, false);
        string strQuery2 = "select Top 1 Attendees,IEPYear,IEPSigDate,Reviewed,MetObjective,MetGoal,NotMaintaining from StdtAcdSheet WHERE StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)='" + endDate + "'";
        Dtcheck = objData.ReturnDataTable(strQuery2, false);
        string strQuery3 = "select  MetObjective,MetGoal,NotMaintaining from StdtAcdSheet WHERE StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)='" + endDate + "'";
        Dtcheck2 = objData.ReturnDataTable(strQuery3, false);
        AttendeesText.Text = Dtcheck.Rows[0]["Attendees"].ToString();
        Iepyeartxt.Text = Dtcheck.Rows[0]["IEPYear"].ToString();
        Ieptxt.Text = Dtcheck.Rows[0]["IEPSigDate"].ToString();
        ReviewbydateUpdate.Text = Dtcheck.Rows[0]["Reviewed"].ToString();

        DataTable ac = new DataTable();
        string strQueryfind = "select Top 1 AccSheetId from StdtAcdSheet WHERE StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)='" + endDate + "'";
        ac = objData.ReturnDataTable(strQueryfind, false);
        ViewState["CurrentAccId"] = ac.Rows[0]["AccSheetId"];
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                //foreach (DataRow dr in Dt.Rows)
                //{
                //    if (dr["Benchmarks"] != null && dr["Benchmarks"]!="")
                //    {
                //        dr["Benchmarks"] = clsGeneral.StringToHtml(Convert.ToString(dr["Benchmarks"]));
                //    }
                //}
                GridViewAccSheetedit.DataSource = Dt;
                GridViewAccSheetedit.DataBind();
                btnUpdate.Visible = true;
                btnUpdateNew.Visible = true;
                btnImport.Visible = true;
                btnDelete.Visible = true;
                rbtnLsnClassTypeAc.Visible = true;
                tdMsg1.Visible = true;
                tdMsg2.Visible = true;
                tdreview2.Visible = true;
                gvAgendaItem.Visible = true;
                btnSave.Visible = false;
                btnSaveNew.Visible = false;
                int i = 0;
                        foreach (GridViewRow row in GridViewAccSheetedit.Rows)
                        {
                    RadioButtonList radBtnLst1 = row.FindControl("RadioButtonList8") as RadioButtonList;
                    RadioButtonList radBtnLst2 = row.FindControl("RadioButtonList9") as RadioButtonList;
                    RadioButtonList radBtnLst3 = row.FindControl("RadioButtonList10") as RadioButtonList;
                    RadioButtonList radBtnLst4 = row.FindControl("RadioButtonList11") as RadioButtonList;
                    RadioButtonList radBtnLst5 = row.FindControl("RadioButtonList12") as RadioButtonList;
                    RadioButtonList radBtnLst6 = row.FindControl("RadioButtonList13") as RadioButtonList;
                    RadioButtonList radBtnLst7 = row.FindControl("RadioButtonList14") as RadioButtonList;
                    
                    radBtnLst1.SelectedValue = Dt.Rows[i]["Progressing1"].ToString();
                    if (radBtnLst1.SelectedValue == "Yes")
                        radBtnLst1.Items[0].Attributes["Style"] = "color:lightgreen;";
                    else if(radBtnLst1.SelectedValue == "No")
                        radBtnLst1.Items[1].Attributes["Style"] = "color:red;";
                    else if(radBtnLst1.SelectedValue == "No Change")
                        radBtnLst1.Items[2].Attributes["Style"] = "color:yellow;";

                    radBtnLst2.SelectedValue = Dt.Rows[i]["Progressing2"].ToString();
                    if (radBtnLst2.SelectedValue == "Yes")
                        radBtnLst2.Items[0].Attributes["Style"] = "color:lightgreen;";
                    else if (radBtnLst2.SelectedValue == "No")
                        radBtnLst2.Items[1].Attributes["Style"] = "color:red;";
                    else if (radBtnLst2.SelectedValue == "No Change")
                        radBtnLst2.Items[2].Attributes["Style"] = "color:yellow;";

                    radBtnLst3.SelectedValue = Dt.Rows[i]["Progressing3"].ToString();
                    if (radBtnLst3.SelectedValue == "Yes")
                        radBtnLst3.Items[0].Attributes["Style"] = "color:lightgreen;";
                    else if (radBtnLst3.SelectedValue == "No")
                        radBtnLst3.Items[1].Attributes["Style"] = "color:red;";
                    else if (radBtnLst3.SelectedValue == "No Change")
                        radBtnLst3.Items[2].Attributes["Style"] = "color:yellow;";

                    radBtnLst4.SelectedValue = Dt.Rows[i]["Progressing4"].ToString();
                    if (radBtnLst4.SelectedValue == "Yes")
                        radBtnLst4.Items[0].Attributes["Style"] = "color:lightgreen;";
                    else if (radBtnLst4.SelectedValue == "No")
                        radBtnLst4.Items[1].Attributes["Style"] = "color:red;";
                    else if (radBtnLst4.SelectedValue == "No Change")
                        radBtnLst4.Items[2].Attributes["Style"] = "color:yellow;";

                    radBtnLst5.SelectedValue = Dt.Rows[i]["Progressing5"].ToString();
                    if (radBtnLst5.SelectedValue == "Yes")
                        radBtnLst5.Items[0].Attributes["Style"] = "color:lightgreen;";
                    else if (radBtnLst5.SelectedValue == "No")
                        radBtnLst5.Items[1].Attributes["Style"] = "color:red;";
                    else if (radBtnLst5.SelectedValue == "No Change")
                        radBtnLst5.Items[2].Attributes["Style"] = "color:yellow;";

                    radBtnLst6.SelectedValue = Dt.Rows[i]["Progressing6"].ToString();
                    if (radBtnLst6.SelectedValue == "Yes")
                        radBtnLst6.Items[0].Attributes["Style"] = "color:lightgreen;";
                    else if (radBtnLst6.SelectedValue == "No")
                        radBtnLst6.Items[1].Attributes["Style"] = "color:red;";
                    else if (radBtnLst6.SelectedValue == "No Change")
                        radBtnLst6.Items[2].Attributes["Style"] = "color:yellow;";

                    radBtnLst7.SelectedValue = Dt.Rows[i]["Progressing7"].ToString();
                    if (radBtnLst7.SelectedValue == "Yes")
                        radBtnLst7.Items[0].Attributes["Style"] = "color:lightgreen;";
                    else if (radBtnLst7.SelectedValue == "No")
                        radBtnLst7.Items[1].Attributes["Style"] = "color:red;";
                    else if (radBtnLst7.SelectedValue == "No Change")
                        radBtnLst7.Items[2].Attributes["Style"] = "color:yellow;";

                    if (Dt.Rows[i]["MetObjective"] != null && Dt.Rows[i]["MetGoal"] != null && Dt.Rows[i]["NotMaintaining"] != null&& i<Dt.Rows.Count)
                            {

                                HtmlInputCheckBox checkbox1 = row.FindControl("Checkbox1") as HtmlInputCheckBox;
                                HtmlInputCheckBox checkbox2 = row.FindControl("Checkbox2") as HtmlInputCheckBox;
                                HtmlInputCheckBox checkbox3 = row.FindControl("Checkbox3") as HtmlInputCheckBox;
                                int ch1 = Convert.ToInt32(Dt.Rows[i]["MetObjective"]);
                            int ch2 = Convert.ToInt32(Dt.Rows[i]["MetGoal"]);
                            int ch3 = Convert.ToInt32(Dt.Rows[i]["NotMaintaining"]);
                            if (ch1 == 1)
                            {
                                checkbox1.Checked = true;
                                HtmlGenericControl c1 = row.FindControl("ch1") as HtmlGenericControl;
                                if (c1 != null)
                                {
                                    c1.Style["color"] = "Green";
                                    c1.Style["font-weight"] = "bold";
                                }
                            }
                            if (ch2 == 1)
                            {
                                checkbox2.Checked = true;
                                HtmlGenericControl c2 = row.FindControl("ch2") as HtmlGenericControl;
                                if (c2 != null)
                                {
                                    c2.Style["color"] = "Green";
                                    c2.Style["font-weight"] = "bold";

                                }
                            }
                            if (ch3 == 1)
                            {
                                checkbox3.Checked = true;
                                HtmlGenericControl c3 = row.FindControl("ch3") as HtmlGenericControl;
                                if (c3 != null)
                                {
                                    c3.Style["color"] = "Red";
                                    c3.Style["font-weight"] = "bold";
                                }
                            }
                            }
                            i = i + 1;
                        }
                    

            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
                GridViewAccSheetedit.DataSource = null;
                GridViewAccSheetedit.DataBind();
                btnImport.Visible = false;
                btnDelete.Visible = false;
                rbtnLsnClassTypeAc.Visible = false;
                tdMsg1.Visible = false;
                tdMsg2.Visible = false;
                tdreview2.Visible = false;
                gvAgendaItem.Visible = false;
                btnUpdate.Visible = false;
                btnUpdateNew.Visible = false;
                btnSave.Visible = false;
                btnSaveNew.Visible = false;
            }
        }
    }








    //private void loadDataListEdit()
    //{

    //    objData = new clsData();
    //    Dt = new System.Data.DataTable();



    //    string qry = "select * from StdtAcdSheet where StudentId=" + sess.StudentId + " and CONVERT(datetime,DateOfMeeting)=CONVERT(datetime,'" + ddlDate.SelectedItem.Text + "')";


    //    string strQuery = qry;
    //    Dt = objData.ReturnDataTable(strQuery, false);
    //    if (Dt != null)
    //    {
    //        if (Dt.Rows.Count > 0)
    //        {
    //            GridViewAccSheetedit.DataSource = Dt;
    //            GridViewAccSheetedit.DataBind();
    //            btnUpdate.Visible = true;
    //            btnImport.Visible = true;

    //        }
    //        else
    //        {
    //            tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
    //            GridViewAccSheetedit.DataSource = null;
    //            GridViewAccSheetedit.DataBind();
    //            btnImport.Visible = false;
    //            btnUpdate.Visible = false;
    //        }
    //    }
    //}



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
        PageNo = PageNo + 1;

        try
        {
            string Time = DateTime.Now.TimeOfDay.ToString();
            string[] ar = Time.Split('.');
            Time = ar[0];
            Time = Time.Replace(":", "-");
            string Datet = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year.ToString() + "-" + Time;

            string path = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";



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
            tdMsg.InnerHtml = clsGeneral.warningMsg("Directory or File already Exists!");
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
        string dateVal = "";
        Dt = new System.Data.DataTable();
        ClsAccSheetExport objExport = new ClsAccSheetExport();
        try
        {
            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp2\\";
            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }
            string Temp2 = Server.MapPath("~\\StudentBinder") + "\\ClinicalMerge";
            if (!Directory.Exists(Temp2))
            {
                Directory.CreateDirectory(Temp2);
            }

            if (ViewState["CurrentDate"] != null)
            {
                dateVal = ViewState["CurrentDate"].ToString();
                CreateQuery("NE", "XMLAS\\AS1Creations.xml");
                Dt = objExport.getAccSheet(sess.StudentId, sess.SchoolId, dateVal);
                int pageCount = 0;
                int dtl = Dt.Rows.Count - 1;
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < placeHolders.Length; i++)
                    {
                        if (i == 1)
                        {
                            string[] startDate = dr["IepDates"].ToString().Split(' ');
                            string[] endDate = dr["IepDates"].ToString().Split(' ');

                            if (startDate[0] != "" && endDate[1] != "")
                            {
                                string dateAccSheet = startDate[0] + "-" + endDate[1];
                                columns[i] = dateAccSheet;
                            }
                            //else
                            //{
                            //    tdMsg.InnerHtml = clsGeneral.failedMsg("Failed !");
                            //    return;
                            //}
                        }
                        if (i == 2)
                        {
                            columns[i] = DateTime.Parse(dr["DateOfMeeting"].ToString()).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            //columns[i] = dr[columnsToAdd[i]].ToString();
                            columns[i] = dr[columnsToAdd[i]].ToString().Replace("&", " and ");
                            columns[i] = columns[i].ToString().Replace("<", " Less than ");
                                columns[i] = columns[i].ToString().Replace(">", " Greater than ");
                                columns[i] = columns[i].ToString().Replace("Less than w:br/ Greater than ", " <w:br/> ");
                          
                        }
                    }


                    Path = Server.MapPath("~\\StudentBinder\\ASTemplates\\ASTemplates1.docx");
                    NewPath = CopyTemplate(Path, pageCount.ToString());
                    if (NewPath != "")
                    {
                        using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))    ///same styles in Benchmark should be in Export --jis
                        {
                            if (columns[5] != "")
                            {
                                replaceWithTextsSingle(theDoc.MainDocumentPart, "plcBenchmarkGoal", columns[5]);

                            }
                            else
                            {
                                replaceWithTextsSingle(theDoc.MainDocumentPart, "plcBenchmarkGoal", "");
                            }
                            
                        }
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

                string StartPath = Server.MapPath("~\\StudentBinder\\ASTemplates\\ASTemplateStart.docx");
                string EndPath = Server.MapPath("~\\StudentBinder\\ASTemplates\\ASTemplateEnd.docx");
              
                // Replace placeholders with data
                bool dse = findStartandEnd(StartPath, EndPath, columns[0], columns[1], columns[2], dateVal);

                bool iepDoneFlg = MergeFiles();

                if (iepDoneFlg == false)
                {
                    tdMsgExport.InnerHtml = clsGeneral.failedMsg("Document Creation Failed !");
                }
                else
                {
                    tdMsg.InnerHtml = "";
                    tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Documents Sucessfully Created ");
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myfn", "HideWait();", true);
                    //string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#PopDownload').css('top', '5%'); $('#PopDownload').show(); }); $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
                    //   btnIEPExport.Text = "Download";
                    //   BtnCanel.Visible = true;
                }

            }
        }
        catch (Exception eX)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Failed !");
            throw eX;
        }
    }
   

    private bool findStartandEnd(string StartPath, string EndPath, string sname,string iepdate,string dateofmeet, string dateVal)
    {
        bool retVal = false;
        
        try
        {
            objData = new clsData();
            string Acid = ViewState["CurrentAccId"].ToString();
            string strQueryfind = "select Attendees,IEPYear,IEPSigDate,Reviewed  from StdtAcdSheet WHERE AccSheetId=" + Acid;
            DataTable ac = objData.ReturnDataTable(strQueryfind, false);

            string strtDate = "";
            string endDate = "";
            if (dateVal != null && dateVal != "")
            {
                strtDate = dateVal.Split('-')[0];
                endDate = dateVal.Split('-')[1];
            }
            string strAgendaItemqry = "select AgendaItem, StaffInitials, AgendaAddedDate, DoneCarryOver from AcdSheetMtng where followstatus = 'A' and AccSheetId in " +
                                      " (select StdtAcdSheet.Accsheetid from StdtAcdSheet inner join Student on StdtAcdSheet.StudentId=Student.StudentId where StdtAcdSheet.StudentId = " + sess.StudentId + "" +
                                      " and CONVERT(datetime,DateOfMeeting)=CONVERT(datetime,'" + strtDate + "') and CONVERT(datetime,EndDate)=CONVERT(datetime,'" + endDate + "'))";
            DataTable dtAgItm = objData.ReturnDataTable(strAgendaItemqry, false);

            ViewState["dtable"] = ac;
            string Temp = Server.MapPath("~\\StudentBinder") + "\\ACStartTemp";

            const string DOC_URL = "/word/document.xml";

            if (!Directory.Exists(Temp))
            {
                Directory.CreateDirectory(Temp);
            }

            string output = Temp + "\\AcademicSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}.doc";
            string FIRST_PAGE = Server.MapPath("~\\StudentBinder\\ASTemplates\\Dummy1.docx");

            string fileName = string.Format(output, DateTime.Now);
            File.Copy(StartPath, fileName);


            string Temp2 = Server.MapPath("~\\StudentBinder") + "\\ACEndTemp";

            if (!Directory.Exists(Temp2))
            {
                Directory.CreateDirectory(Temp2);
            }

            string output2 = Temp2 + "\\AcademicSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}.doc";
            string FIRST_PAGE2 = Server.MapPath("~\\StudentBinder\\ASTemplates\\Dummy2.docx");

            string fileName2 = string.Format(output2, DateTime.Now);
            File.Copy(EndPath, fileName2);
            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(fileName, true))   
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcStudentName", sname);
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcIEPDate", iepdate);
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcMeetingDate", dateofmeet);
            if (ac.Rows[0]["Attendees"] != "")
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcAttendees", ac.Rows[0]["Attendees"].ToString());
            }
            else
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcAttendees", "");
            }
            if (ac.Rows[0]["IEPYear"] != "")
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcIEPYear", ac.Rows[0]["IEPYear"].ToString());
                    
            }
            else
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcIEPYear", "");
            }
            if (ac.Rows[0]["IEPSigDate"] != "")
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcIEPSignatureandDate", ac.Rows[0]["IEPSigDate"].ToString());
            }
            else
            {
                replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcIEPSignatureandDate", "");
            }

                MainDocumentPart mainPart = theDoc.MainDocumentPart;
                Body bod = mainPart.Document.Body;
                int tablecounter = 0;
                foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
                {
                    if (tablecounter == 4)
                    {
                        foreach (DataRow dr in dtAgItm.Rows)
                        {
                            DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                            string val1 = dr[0].ToString();
                            DocumentFormat.OpenXml.Wordprocessing.Run run1 = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            if (val1 != "")
                                run1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(val1));
                            else
                                run1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(""));

                            string val2 = dr[1].ToString();
                            DocumentFormat.OpenXml.Wordprocessing.Run run2 = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            if (val2 != "")
                                run2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(val2));
                            else
                                run2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(""));

                            string val3 = dr[2].ToString();
                            DocumentFormat.OpenXml.Wordprocessing.Run run3 = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            if (val3 != "")
                                run3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(val3));
                            else
                                run3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(""));

                            string val4 = dr[3].ToString();
                            DocumentFormat.OpenXml.Wordprocessing.Run run4 = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            if (val4 != "")
                                run4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(val4));
                            else
                                run4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(""));

                            DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1
                                    = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                            tblCell1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run1));
                            DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2
                                    = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                            tblCell2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run2));
                            DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3
                                    = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                            tblCell3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run3));
                            DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell4
                                    = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                            tblCell4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run4));


                            trXml.Append(tblCell1);
                            trXml.Append(tblCell2);
                            trXml.Append(tblCell3);
                            trXml.Append(tblCell4);

                            t.Append(trXml);
                        }
                    }
                    tablecounter++;
                }
                mainPart.Document.Save();
            }
            //Replace end - start
            using (WordprocessingDocument theDoc = WordprocessingDocument.Open(fileName2, true))
            {
                if (ac.Rows[0]["Reviewed"] != "")
                {
                    replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcReviewed", ac.Rows[0]["Reviewed"].ToString());
                }
                else
                {
                    replaceWithTextsSingle(theDoc.MainDocumentPart, "PlcReviewed", "");
                }
            }
            //Replace end - end
            ViewState["StartingPage"] = fileName;
            ViewState["EndingPage"] = fileName2;
        retVal = true;

        return retVal;
    }
    catch (Exception ex)
    {
        return false;
    }
    }



    public bool MergeFiles()
    {
        bool retVal = false;
        try
        {
            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";
            string Temp1 = Server.MapPath("~\\StudentBinder") + "\\ACMerge";

            const string DOC_URL = "/word/document.xml";


            //string FolderName = "\\AcademicSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}";
            //FolderName = string.Format(FolderName, DateTime.Now);
            //string path = Server.MapPath("~\\Administration") + "\\IEPMerged";


            if (!Directory.Exists(Temp1))
            {
                Directory.CreateDirectory(Temp1);
            }

            string OUTPUT_FILE = Temp1 + "\\AcademicSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}.doc";
            string FIRST_PAGE = Server.MapPath("~\\StudentBinder\\ASTemplates\\Dummy.docx");

            string start = ViewState["StartingPage"].ToString();
            string end = ViewState["EndingPage"].ToString();

            string fileName = string.Format(OUTPUT_FILE, DateTime.Now);
            File.Copy(FIRST_PAGE, fileName);

           
            AppendStartingPage(fileName, start, end);
            
            string tempstart=Server.MapPath("~\\StudentBinder") + "\\ACStartTemp";
            string tempend = Server.MapPath("~\\StudentBinder") + "\\ACEndTemp";
            if (Directory.Exists(tempstart))
            {
                Directory.Delete(tempstart, true);
            }
            if (Directory.Exists(tempend))
            {
                Directory.Delete(tempend, true);
            }
            var filePaths = Directory.GetFiles(Temp).Select(f => new FileInfo(f)).OrderByDescending(f => f.CreationTime);
            int i = 1;

            //for (int j = filePaths.Length - 1; j >= 0; j--)
            //{
            //    makeWord(filePaths[j], fileName, i);
            //    i++;
            //}
            foreach (var a in filePaths)
            {
                makeWord(a.ToString(), fileName, i);
                i++;
            }
            ViewState["FileName"] = fileName;
            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }
            retVal = true;

            return retVal;

        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Failed !");
            throw Ex;
        }

    }
    
   

   

    private void AppendStartingPage(string mainDocument, string startingPagePath, string endingpagepath)
    {

        DocX doc1 = DocX.Load(startingPagePath);
        DocX doc2 = DocX.Load(mainDocument);
        doc2.InsertDocument(doc1, false);
        doc2.SaveAs(mainDocument);

        using (WordprocessingDocument oneDocument = WordprocessingDocument.Open(startingPagePath, false))
        using (WordprocessingDocument twoDocument = WordprocessingDocument.Open(mainDocument, true))
        using (WordprocessingDocument threeDocument = WordprocessingDocument.Open(endingpagepath, false))
        {
            // Get the MainDocumentPart of each document
            MainDocumentPart oneMainPart = oneDocument.MainDocumentPart;
            MainDocumentPart twoMainPart = twoDocument.MainDocumentPart;
            MainDocumentPart threeMainPart = threeDocument.MainDocumentPart;

            Body oneBodyClone = new Body(oneMainPart.Document.Body.OuterXml);
            Body threeBodyClone = new Body(threeMainPart.Document.Body.OuterXml);

            //foreach (var additionalBodyElement in oneDocument.MainDocumentPart.Document.Body.Elements())
            //{
            //    twoDocument.MainDocumentPart.Document.Body.AppendChild(additionalBodyElement.CloneNode(true));
            //}
        //    //twoMainPart.Document.Body.InsertBeforeSelf(oneBodyClone);
            twoMainPart.Document.Body.InsertAfterSelf(threeBodyClone);
            twoMainPart.Document.Save();

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


    //protected void btnLoadData_Click(object sender, EventArgs e)
    //{

    //}
    protected void Save_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        //btnBack.Text = "Cancel";
        objData = new clsData();
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string testIfPresent = "select AccSheetId from StdtAcdSheet where StudentId=" + sess.StudentId + " AND CONVERT(datetime,DateOfMeeting)=CONVERT(datetime,'" + dtst.ToString("MM/dd/yyyy") + "') AND CONVERT(datetime,EndDate)=CONVERT(datetime,'" + dted.ToString("MM/dd/yyyy") + "')";
        //bool result = true;
        if (objData.IFExists(testIfPresent) == false)
        {
            foreach (GridViewRow row in GridViewAccSheet.Rows)
            {
                Label lblGoalArea = row.Controls[0].FindControl("lblGoalArea") as Label;
                Label lblGoal = row.Controls[0].FindControl("lblGoal") as Label;
                //TextBox txtbenchaMark = row.Controls[0].FindControl("txtbenchaMark") as TextBox;
                HtmlGenericControl txtbenchaMark = row.Controls[0].FindControl("txtbenchaMark") as HtmlGenericControl;
                HiddenField hfLPId = row.Controls[0].FindControl("hfLPId") as HiddenField;

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

                TextBox txtNoOfPos1 = row.Controls[0].FindControl("txtNoOfPos1") as TextBox;
                TextBox txtNoOfPos2 = row.Controls[0].FindControl("txtNoOfPos2") as TextBox;
                TextBox txtNoOfPos3 = row.Controls[0].FindControl("txtNoOfPos3") as TextBox;
                TextBox txtNoOfPos4 = row.Controls[0].FindControl("txtNoOfPos4") as TextBox;
                TextBox txtNoOfPos5 = row.Controls[0].FindControl("txtNoOfPos5") as TextBox;
                TextBox txtNoOfPos6 = row.Controls[0].FindControl("txtNoOfPos6") as TextBox;
                TextBox txtNoOfPos7 = row.Controls[0].FindControl("txtNoOfPos7") as TextBox;

                RadioButtonList radioButtonList1 = row.Controls[0].FindControl("radioButtonList1") as RadioButtonList;
                RadioButtonList radioButtonList2 = row.Controls[0].FindControl("radioButtonList2") as RadioButtonList;
                RadioButtonList radioButtonList3 = row.Controls[0].FindControl("radioButtonList3") as RadioButtonList;
                RadioButtonList radioButtonList4 = row.Controls[0].FindControl("radioButtonList4") as RadioButtonList;
                RadioButtonList radioButtonList5 = row.Controls[0].FindControl("radioButtonList5") as RadioButtonList;
                RadioButtonList radioButtonList6 = row.Controls[0].FindControl("radioButtonList6") as RadioButtonList;
                RadioButtonList radioButtonList7 = row.Controls[0].FindControl("radioButtonList7") as RadioButtonList;

                //result = true;
                //result = ValidateDenominator(txtNoOfPos1);
                //if (result == false) break;
                //result = ValidateDenominator(txtNoOfPos2);
                //if (result == false) break;
                //result = ValidateDenominator(txtNoOfPos3);
                //if (result == false) break;
                //result = ValidateDenominator(txtNoOfPos4);
                //if (result == false) break;
                //result = ValidateDenominator(txtNoOfPos5);
                //if (result == false) break;
                //result = ValidateDenominator(txtNoOfPos6);
                //if (result == false) break;
                //result = ValidateDenominator(txtNoOfPos7);
                //if (result == false) break;


                Label lblMis1 = row.Controls[0].FindControl("lblMis1") as Label;
                Label lblMis2 = row.Controls[0].FindControl("lblMis2") as Label;
                Label lblMis3 = row.Controls[0].FindControl("lblMis3") as Label;
                Label lblMis4 = row.Controls[0].FindControl("lblMis4") as Label;
                Label lblMis5 = row.Controls[0].FindControl("lblMis5") as Label;
                Label lblMis6 = row.Controls[0].FindControl("lblMis6") as Label;
                Label lblMis7 = row.Controls[0].FindControl("lblMis7") as Label;

                HiddenField hdnstep1 = row.Controls[0].FindControl("hdnstep1") as HiddenField;
                HiddenField hdnstep2 = row.Controls[0].FindControl("hdnstep2") as HiddenField;
                HiddenField hdnstep3 = row.Controls[0].FindControl("hdnstep3") as HiddenField;
                HiddenField hdnstep4 = row.Controls[0].FindControl("hdnstep4") as HiddenField;
                HiddenField hdnstep5 = row.Controls[0].FindControl("hdnstep5") as HiddenField;
                HiddenField hdnstep6 = row.Controls[0].FindControl("hdnstep6") as HiddenField;
                HiddenField hdnstep7 = row.Controls[0].FindControl("hdnstep7") as HiddenField;

                string NumPos1 = lblNoOfPos1.Text + "/" + txtNoOfPos1.Text;
                string NumPos2 = lblNoOfPos2.Text + "/" + txtNoOfPos2.Text;
                string NumPos3 = lblNoOfPos3.Text + "/" + txtNoOfPos3.Text;
                string NumPos4 = lblNoOfPos4.Text + "/" + txtNoOfPos4.Text;
                string NumPos5 = lblNoOfPos5.Text + "/" + txtNoOfPos5.Text;
                string NumPos6 = lblNoOfPos6.Text + "/" + txtNoOfPos6.Text;
                string NumPos7 = lblNoOfPos7.Text + "/" + txtNoOfPos7.Text;

                string radBtnList1 = radioButtonList1.SelectedValue.ToString();
                string radBtnList2 = radioButtonList2.SelectedValue.ToString();
                string radBtnList3 = radioButtonList3.SelectedValue.ToString();
                string radBtnList4 = radioButtonList4.SelectedValue.ToString();
                string radBtnList5 = radioButtonList5.SelectedValue.ToString();
                string radBtnList6 = radioButtonList6.SelectedValue.ToString();
                string radBtnList7 = radioButtonList7.SelectedValue.ToString();


                TextBox txtFreeText = row.Controls[0].FindControl("txtFreetxt") as TextBox;
                TextBox txtPersDissc = row.Controls[0].FindControl("txtPersDissc") as TextBox;
                TextBox txtResAndDeadline = row.Controls[0].FindControl("txtResAndDeadline") as TextBox;
                string txtBenchText = clsGeneral.HtmlToString(txtbenchaMark.InnerHtml); ///Html to db string --jis
                int c1, c2, c3;
                HtmlInputCheckBox checkbox1 = row.FindControl("Checkbox4") as HtmlInputCheckBox;
                HtmlInputCheckBox checkbox2 = row.FindControl("Checkbox5") as HtmlInputCheckBox;
                HtmlInputCheckBox checkbox3 = row.FindControl("Checkbox6") as HtmlInputCheckBox;
                if (checkbox1.Checked)
                {
                    c1 = 1;
                }
                else { c1 = 0; }
                if (checkbox2.Checked)
                {
                    c2 = 1;
                }
                else { c2 = 0; }
                if (checkbox3.Checked)
                {
                    c3 = 1;
                }
                else { c3 = 0; }
                
                string sqlQry = "INSERT INTO [dbo].[StdtAcdSheet] ([StudentId],[DateOfMeeting],[EndDate],[GoalArea],[Goal],[Benchmarks]" +
                    ",[TypeOfInstruction],[Period1],[Set1],[Prompt1],[IOA1],[NoOfTimes1],[Progressing1],[Period2],[Set2],[Prompt2],[IOA2]," +
                    "[NoOfTimes2],[Progressing2],[Period3],[Set3],[Prompt3],[IOA3],[NoOfTimes3],[Progressing3],[Period4],[Set4],[Prompt4],[IOA4],[NoOfTimes4],[Progressing4],[Period5],[Set5]," +
                "[Prompt5],[IOA5],[NoOfTimes5],[Progressing5],[Period6],[Set6],[Prompt6],[IOA6],[NoOfTimes6],[Progressing6],[Period7],[Set7],[Prompt7],[IOA7],[NoOfTimes7],[Progressing7],[LessonPlanId]," + 
                "[Mistrial1],[Mistrial2],[Mistrial3],[Mistrial4],[Mistrial5],[Mistrial6],[Mistrial7],[Step1],[Step2],[Step3],[Step4],[Step5],[Step6],[Step7]," + 
                "[Attendees],[IEPYear],[IEPSigDate],[Reviewed],[MetObjective],[MetGoal],[NotMaintaining]) " +
                "VALUES(" + sess.StudentId + ",'" + dtst.ToString("MM/dd/yyyy") + "','" + dted.ToString("MM/dd/yyyy") + "'," +
                "'" + lblGoalArea.Text + "'," +
                "'" + lblGoal.Text + "'," +

                "'" + clsGeneral.convertQuotes(txtBenchText) + "'," +

                //"'" + clsGeneral.convertQuotes(txtFreeText.Text.Trim()) + "'," +
                    //"'" + clsGeneral.convertQuotes(txtPersDissc.Text.Trim()) + "'," +
                    //"'" + clsGeneral.convertQuotes(txtResAndDeadline.Text.Trim()) + "'," +
                "'" + lblTypOfIns1.Text + "'," +

                "'" + lblPeriod1.Text + "'," +
                "'" + clsGeneral.convertQuotes(lblStmlsSet1.Text) + "'," +
                "'" + lblprmtLvl1.Text + "'," +
                "'" + lblIOA1.Text + "'," +
                "'" + NumPos1 + "'," +
                "'" + radBtnList1 + "'," +

                "'" + lblPeriod2.Text + "'," +
                "'" + clsGeneral.convertQuotes(lblStmlsSet2.Text) + "'," +
                "'" + lblprmtLvl2.Text + "'," +
                "'" + lblIOA2.Text + "'," +
                "'" + NumPos2 + "'," +
                "'" + radBtnList2 + "'," +

                "'" + lblPeriod3.Text + "'," +
                "'" + clsGeneral.convertQuotes(lblStmlsSet3.Text) + "'," +
                "'" + lblprmtLvl3.Text + "'," +
                "'" + lblIOA3.Text + "'," +
                "'" + NumPos3 + "'," +
                "'" + radBtnList3 + "'," +


                "'" + lblPeriod4.Text + "'," +
                "'" + clsGeneral.convertQuotes(lblStmlsSet4.Text) + "'," +
                "'" + lblprmtLvl4.Text + "'," +
                "'" + lblIOA4.Text + "'," +
                "'" + NumPos4 + "'," +
                "'" + radBtnList4 + "'," +


                "'" + lblPeriod5.Text + "'," +
                "'" + clsGeneral.convertQuotes(lblStmlsSet5.Text) + "'," +
                "'" + lblprmtLvl5.Text + "'," +
                "'" + lblIOA5.Text + "'," +
                "'" + NumPos5 + "'," +
                "'" + radBtnList5 + "'," +


                "'" + lblPeriod6.Text + "'," +
                "'" + clsGeneral.convertQuotes(lblStmlsSet6.Text) + "'," +
                "'" + lblprmtLvl6.Text + "'," +
                "'" + lblIOA6.Text + "'," +
                "'" + NumPos6 + "'," +
                "'" + radBtnList6 + "'," +


                "'" + lblPeriod7.Text + "'," +
                "'" + clsGeneral.convertQuotes(lblStmlsSet7.Text) + "'," +
                "'" + lblprmtLvl7.Text + "'," +
                "'" + lblIOA7.Text + "'," +
                "'" + NumPos7 + "'," +
                "'" + radBtnList7 + "'," +


                hfLPId.Value + "," +
                "'" + lblMis1.Text + "'," +
                "'" + lblMis2.Text + "'," +
                "'" + lblMis3.Text + "'," +
                "'" + lblMis4.Text + "'," +
                "'" + lblMis5.Text + "'," +
                "'" + lblMis6.Text + "'," +
                "'" + lblMis7.Text + "'," +

                "'" + hdnstep1.Value + "'," +
                "'" + hdnstep2.Value + "'," +
                "'" + hdnstep3.Value + "'," +
                "'" + hdnstep4.Value + "'," +
                "'" + hdnstep5.Value + "'," +
                "'" + hdnstep6.Value + "'," +
                "'" + hdnstep7.Value + "'," +
                "'" + clsGeneral.convertQuotes(AttendeesText.Text )+ "'," +
                "'" + clsGeneral.convertQuotes(Iepyeartxt.Text) + "'," +
                "'" + clsGeneral.convertQuotes(Ieptxt.Text) + "'," +
                "'" + clsGeneral.convertQuotes(ReviewbydateSave.Text) + "'," +
                "'" + c1 + "'," +
                "'" + c2 + "'," +
                "'" + c3 + "'" +
                ")";


                //int insetChk = objData.Execute(sqlQry);
                int insetChk = objData.ExecuteWithScope(sqlQry);
                int testSave = 0;
                int accSheetId = insetChk;
                DataTable dtAccSheetId = new DataTable();
                DataColumn dc = new DataColumn("AccSheetId", typeof(Int32));
                dtAccSheetId.Columns.Add(dc);
                DataRow dr = dtAccSheetId.NewRow();
                if (accSheetId != 0)
                {
                        foreach (GridViewRow row2 in gvAgendaItem.Rows)
                        {
                            Label lblAgendaItemId = (Label)row2.FindControl("LblAgendaItemId");
                            TextBox txtAgendaItem = (TextBox)row2.FindControl("txtAgendaItem");
                            TextBox txtStaffInitials = (TextBox)row2.FindControl("txtStaffInitials");
                            TextBox txtDateAdded = (TextBox)row2.FindControl("txtDateAdded");
                            RadioButtonList RadBtnListDoneCarryOver = (RadioButtonList)row2.FindControl("RBLDoneCarry");
                            DataTable dtAccSheetId2 = new DataTable();
                            dtAccSheetId2 = null;
                            if (lblAgendaItemId.Text != "")
                            {
                                string accSheetIdQry = "select AccSheetId from AcdSheetMtng where MtngId = " + lblAgendaItemId.Text;
                                dtAccSheetId2 = objData.ReturnDataTable(accSheetIdQry, false);
                            }

                            if (txtAgendaItem.Text != "")
                            {

                                if (lblAgendaItemId.Text != "")
                                {
                                    //update
                                    string strqury = "update AcdSheetMtng set AgendaItem='" + clsGeneral.convertQuotes(txtAgendaItem.Text.Trim()) + "',StaffInitials='" + clsGeneral.convertQuotes(txtStaffInitials.Text.Trim()) +
                                        "',AgendaAddedDate='" + clsGeneral.convertQuotes(txtDateAdded.Text) + "',DoneCarryOver='" + RadBtnListDoneCarryOver.SelectedValue.ToString() + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() where MtngId=" + lblAgendaItemId.Text + "";
                                    testSave += Convert.ToInt32(objData.Execute(strqury));
                                }
                                else
                                {
                                    //save
                                    string strqury = "insert into AcdSheetMtng (AccSheetId,LessonPlanId,AgendaItem,StaffInitials,AgendaAddedDate,DoneCarryOver,ActiveInd,CreatedBy,CreatedOn,followstatus) " +
                                        "values (" + accSheetId + ",(select LessonPlanId from StdtAcdSheet where AccSheetId=" + accSheetId + "),'" + clsGeneral.convertQuotes(txtAgendaItem.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtStaffInitials.Text.Trim()) +
                                        "','" + clsGeneral.convertQuotes(txtDateAdded.Text) + "','" + RadBtnListDoneCarryOver.SelectedValue.ToString() + "', 'A'," + sess.LoginId + ",GETDATE(),'A')";
                                    testSave += Convert.ToInt32(objData.Execute(strqury));
                                    DataTable dt = new DataTable();
                                    dt = objData.ReturnDataTable("select top 1 MtngId from AcdSheetMtng order by 1 desc", false);
                                    lblAgendaItemId.Text = dt.Rows[0]["MtngId"].ToString();
                                }
                            }
                            else
                            {
                                //PersonRespCheck = 0;
                                //testupdate = 0;
                                //break;
                            }


                        }
                    }

                int personResp = 0;
            string textVal = "";
            string userFName = "";
            string userLName = "";
            string userQry = "";
                if (insetChk > 0)
                {
                    GridView gvChild = (GridView)row.FindControl("gvCMeeting");
                    if (gvChild != null)
                    {
                        foreach (GridViewRow row2 in gvChild.Rows)
                        {
                            TextBox txtCFollowUp = (TextBox)row2.FindControl("txtCFollowUp");
                            TextBox txtPersonResponsible = (TextBox)row2.FindControl("txtPersonResponsible");
                            TextBox txtCPersonResponsible = (TextBox)row2.FindControl("txtCPersonResponsible");
                            TextBox txtCDeadlines = (TextBox)row2.FindControl("txtCDeadlines");
                            personResp = 0;
                            if (txtPersonResponsible.Text != "")
                            {
                               textVal = txtPersonResponsible.Text;
                                if(textVal.Contains(","))
                                {
                               userFName = clsGeneral.convertQuotes(textVal.Substring(textVal.IndexOf(",") + 1).Trim());
                               userLName = clsGeneral.convertQuotes(textVal.Substring(0, textVal.IndexOf(",")).Trim());
                                }
                                else
                                {
                                    userFName = userLName = clsGeneral.convertQuotes(textVal.Trim());
                                }
                               userQry = "select UserId from [User] where UserLName = '" + userLName + "' and UserFName = '" + userFName + "'";
                               personResp = Convert.ToInt32(objData.FetchValue(userQry));
                            }
                            if (txtCFollowUp.Text == "" && txtCDeadlines.Text == "" && txtPersonResponsible.Text == "")
                            {

                            }
                            else
                            {
                            string strqury = "insert into AcdSheetMtng (AccSheetId,LessonPlanId,PropAndDisc,PersonResp,Deadlines,ActiveInd,CreatedBy,CreatedOn,followstatus) " +
                                "values (" + insetChk + ",(select LessonPlanId from StdtAcdSheet where AccSheetId=" + insetChk + "),'" + clsGeneral.convertQuotes(txtCFollowUp.Text.Trim()) + "','" + personResp + "',CASE WHEN'" + txtCDeadlines.Text + "' = '' THEN NULL ELSE '" + txtCDeadlines.Text + "' END,'A'," + sess.LoginId + ",GETDATE(),'C')";
                            int status = objData.Execute(strqury);
                        }
                    }
                    }
                    GridView gvCPhild = (GridView)row.FindControl("gvPMeeting");
                    if (gvCPhild != null)
                    {
                        foreach (GridViewRow row2 in gvCPhild.Rows)
                        {
                           
                            TextBox txtPFollowUp = (TextBox)row2.FindControl("txtPFollowUp");
                            TextBox txtPPersonResponsible = (TextBox)row2.FindControl("txtPPersonResponsible");
                            TextBox txtPDeadlines = (TextBox)row2.FindControl("txtPDeadlines");
                            personResp = 0;
                            if (txtPPersonResponsible.Text != "")
                            {
                                textVal = txtPPersonResponsible.Text;
                                if (textVal.Contains(","))
                                {
                                    userFName = clsGeneral.convertQuotes(textVal.Substring(textVal.IndexOf(",") + 1).Trim());
                                    userLName = clsGeneral.convertQuotes(textVal.Substring(0, textVal.IndexOf(",")).Trim());
                                }
                                else
                                {
                                    userFName = userLName = clsGeneral.convertQuotes(textVal.Trim());
                                }
                                userQry = "select UserId from [User] where UserLName = '" + userLName + "' and UserFName = '" + userFName + "'";
                                personResp = Convert.ToInt32(objData.FetchValue(userQry));
                            }
                            if (txtPFollowUp.Text == "" && txtPDeadlines.Text == "" && txtPPersonResponsible.Text == "")
                            {

                            }
                            else
                            {
                                string strqury = "insert into AcdSheetMtng (AccSheetId,LessonPlanId,PropAndDisc,PersonResp,Deadlines,ActiveInd,CreatedBy,CreatedOn,followstatus) " +
                                    "values (" + insetChk + ",(select LessonPlanId from StdtAcdSheet where AccSheetId=" + insetChk + "),'" + clsGeneral.convertQuotes(txtPFollowUp.Text.Trim()) + "','" + personResp + "',CASE WHEN'" + txtPDeadlines.Text + "' = '' THEN NULL ELSE '" + txtPDeadlines.Text + "' END,'A'," + sess.LoginId + ",GETDATE(),'P')";
                                int status = objData.Execute(strqury);
                            }
                        }
                    }
                    
                    FillData();
                    // btnImport.Visible = true;

                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Inserted Succesfully   ");
                    btnUpdate.Visible = true;
                    btnUpdateNew.Visible = true;
                    btnImport.Visible = true;
                    btnDelete.Visible = true;
                    rbtnLsnClassTypeAc.Visible = true;
                    tdMsg1.Visible = true;
                    tdMsg2.Visible = true;
                    tdreview2.Visible = true;
                    gvAgendaItem.Visible = true;
                    rbtnLsnClassTypeAc.SelectedValue = "Day,Residence";
                    btnSave.Visible = false;
                    btnSaveNew.Visible = false;
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
            tdMsg.InnerHtml = clsGeneral.failedMsg("Data already Present   ");
        }
        //if (result == false)
        //{
        //    tdMsg.InnerHtml = clsGeneral.failedMsg("Data not inserted   ");
        //}
        Loadlesson2();
    }
    protected void GridViewAccSheet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //DateTime dateNow8 = dted;
            //DateTime dateNow7 = dateNow8.AddDays(-14);
            //DateTime dateNow6 = dateNow7.AddDays(-14);
            //DateTime dateNow5 = dateNow6.AddDays(-14);
            //DateTime dateNow4 = dateNow5.AddDays(-14);
            //DateTime dateNow3 = dateNow4.AddDays(-14);
            //DateTime dateNow2 = dateNow3.AddDays(-14);
            //DateTime dateNow1 = dateNow2.AddDays(-14);

            DateTime dateNow1 = dtst;
            DateTime dateNow2 = dateNow1.AddDays(+14);
            DateTime dateNow3 = dateNow2.AddDays(+14);
            DateTime dateNow4 = dateNow3.AddDays(+14);
            DateTime dateNow5 = dateNow4.AddDays(+14);
            DateTime dateNow6 = dateNow5.AddDays(+14);
            DateTime dateNow7 = dateNow6.AddDays(+14);
            DateTime dateNow8 = dateNow7.AddDays(+14);

            Label lblperiod1 = (Label)e.Row.FindControl("lblPeriod1");
            Label lblperiod2 = (Label)e.Row.FindControl("lblPeriod2");
            Label lblperiod3 = (Label)e.Row.FindControl("lblPeriod3");
            Label lblperiod4 = (Label)e.Row.FindControl("lblPeriod4");
            Label lblperiod5 = (Label)e.Row.FindControl("lblPeriod5");
            Label lblperiod6 = (Label)e.Row.FindControl("lblPeriod6");
            Label lblperiod7 = (Label)e.Row.FindControl("lblPeriod7");

            lblperiod1.Text = dateNow1.ToString("MM'/'dd'/'yyyy") + " - " + dateNow2.ToString("MM'/'dd'/'yyyy");
            lblperiod2.Text = dateNow2.AddDays(1).ToString("MM'/'dd'/'yyyy") + " - " + dateNow3.ToString("MM'/'dd'/'yyyy");
            lblperiod3.Text = dateNow3.AddDays(1).ToString("MM'/'dd'/'yyyy") + " - " + dateNow4.ToString("MM'/'dd'/'yyyy");
            lblperiod4.Text = dateNow4.AddDays(1).ToString("MM'/'dd'/'yyyy") + " - " + dateNow5.ToString("MM'/'dd'/'yyyy");
            lblperiod5.Text = dateNow5.AddDays(1).ToString("MM'/'dd'/'yyyy") + " - " + dateNow6.ToString("MM'/'dd'/'yyyy");
            lblperiod6.Text = dateNow6.AddDays(1).ToString("MM'/'dd'/'yyyy") + " - " + dateNow7.ToString("MM'/'dd'/'yyyy");
            lblperiod7.Text = dateNow7.AddDays(1).ToString("MM'/'dd'/'yyyy") + " - " + dateNow8.ToString("MM'/'dd'/'yyyy");

           
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView gvP = (GridView)e.Row.FindControl("gvPMeeting");
            gvP.DataSource = dtPMeeting;
            gvP.DataBind();

            GridView gvC = (GridView)e.Row.FindControl("gvCMeeting");
            gvC.DataSource = dtCMeeting;
            gvC.DataBind();
        }
    }
    
    protected void btnPreAccSheet1_Click(object sender, EventArgs e)
    {
        btnUpdateNew.Visible = false;
        txtEdate.Text = DateTime.Now.Date.ToString("MM'/'dd'/'yyyy");
        DateTime Sdate = DateTime.Now.Date.AddDays(-98);
        txtSdate.Text = Sdate.Date.ToString("MM'/'dd'/'yyyy");
        string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
        AttendeesText.Text = "";
        Iepyeartxt.Text = "";
        Ieptxt.Text = "";
        // FillStudent();
    }
    protected void btnGenACD_Click(object sender, EventArgs e)
    {
        if (validate() == true)
        {
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            tdMsg.InnerHtml = "";
            btnSave.Visible = false;
            btnSaveNew.Visible = false;
            MultiView1.ActiveViewIndex = 0;
            GridViewAccSheet.DataSource = null;
            GridViewAccSheet.DataBind();
            GridViewAccSheetedit.DataSource = null;
            GridViewAccSheetedit.DataBind();
            btnImport.Visible = false;
            btnDelete.Visible = false;
            rbtnLsnClassTypeAc.Visible = false;
            tdMsg1.Visible = false;
            tdMsg2.Visible = false;
            tdreview2.Visible = false;
            gvAgendaItem.Visible = false;
            //btnBack.Text = "Cancel";

            string testIfPresent = "select AccSheetId from StdtAcdSheet where StudentId=" + sess.StudentId + " AND CONVERT(datetime,DateOfMeeting)=CONVERT(datetime,'" + dtst.ToString("MM/dd/yyyy") + "') AND CONVERT(datetime,EndDate)=CONVERT(datetime,'" + dted.ToString("MM/dd/yyyy") + "')";
            if (objData.IFExists(testIfPresent) == false)
            {
                loadAgendaItemGV();
                LoadPMeetingGVNew();
                LoadCMeetingGVNew();
                loadDataList();
                string dateOfMtng = dtst.ToString("MM/dd/yyyy").Replace("-", "/") + "-" + dted.ToString("MM/dd/yyyy").Replace("-", "/");
                ViewState["CurrentDate"] = dateOfMtng;
                LoadMeetingsNew(dateOfMtng);
                loadExtraData();
                rbtnLsnClassTypeAc.Visible = true;
                tdMsg1.Visible = true;
                tdMsg2.Visible = true;
                tdreview2.Visible = true;
                gvAgendaItem.Visible = true;
                rbtnLsnClassTypeAc.SelectedValue = "Day,Residence";
            }
            else
            {
                string dateVal = dtst.ToString("MM/dd/yyyy").Replace("-", "/") + "-" + dted.ToString("MM/dd/yyyy").Replace("-", "/");
                ViewState["CurrentDate"] = dateVal;
                LoadPMeetingGV();
                LoadCMeetingGV();
                loadDataList(dateVal);
                LoadMeetings(dateVal);
                MultiView1.ActiveViewIndex = 1;             ///Set multiview 1 view(update button for already saved academic sheets) --jis
                btnUpdate.Visible = true;
                btnUpdateNew.Visible = true;
                setWritePermissions();
            }
        }
    }
    private bool validate()
    {
        objData = new clsData();
        bool result = true;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (txtSdate.Text == "")
        {
            result = false;
            tdMessage.InnerHtml = clsGeneral.warningMsg("Please Select Start Date");
            return result;
        }
        else if (txtEdate.Text == "")
        {
            result = false;
            tdMessage.InnerHtml = clsGeneral.warningMsg("Please Select End Date");
            return result;
        }
        else if (txtSdate.Text != "" && txtEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            if (dtst > dted)
            {
                result = false;
                tdMessage.InnerHtml = clsGeneral.warningMsg("Start date is must before the End date");
                return result;
            }

        }

        return result;
    }

    protected void loadExtraData()
    {
        objData = new clsData();
        DataTable dtOldExtra = null;
        DataTable dtSavedAcd = null;
        DateTime prevAcdStrtDate;
        DateTime prevAcdEndDate;
        if (objData.IFExists("select AccSheetId from StdtAcdSheet where StudentId=" + sess.StudentId + ""))
        {
            string qryFirst = "select DateOfMeeting,EndDate from StdtAcdSheet where StudentId=" + sess.StudentId + "  AND AccSheetId=(select MAX(AccSheetId) from StdtAcdSheet where StudentId=" + sess.StudentId + ") ";

            dtSavedAcd = objData.ReturnDataTable(qryFirst, false);

            string qryNewFollowup = "";
            if (dtSavedAcd != null)
            {
                if (dtSavedAcd.Rows.Count > 0)
                {

                    prevAcdStrtDate = Convert.ToDateTime(dtSavedAcd.Rows[0]["DateOfMeeting"]);
                    string preAcdEndDateformat = "";
                    if (Convert.ToString(dtSavedAcd.Rows[0]["EndDate"]) != "")
                    {
                        prevAcdEndDate = Convert.ToDateTime(dtSavedAcd.Rows[0]["EndDate"]);
                        preAcdEndDateformat = prevAcdEndDate.ToString("yyyy-MM-dd");
                    }
                    string preAcdStrtDateformat = prevAcdStrtDate.ToString("yyyy-MM-dd");

                    qryNewFollowup = "select AccSheetId,goalArea,PersonResNdDeadline FROM  StdtAcdSheet WHERE DateOfMeeting='" + preAcdStrtDateformat + "' AND EndDate='" + preAcdEndDateformat + "' AND StudentId=" + sess.StudentId + "";
                    dtOldExtra = objData.ReturnDataTable(qryNewFollowup, false);

                }
            }

        }
        if (dtOldExtra != null)
        {
            foreach (GridViewRow row in GridViewAccSheet.Rows)
            {
                Label lblGoalArea = row.Controls[0].FindControl("lblGoalArea") as Label;
                TextBox txtFreeText = row.Controls[0].FindControl("txtFreetxt") as TextBox;
                for (int i = 0; i < dtOldExtra.Rows.Count; i++)
                {
                    if (lblGoalArea.Text == dtOldExtra.Rows[i]["goalArea"].ToString())
                    {
                        //txtFreeText.Text = dtOldExtra.Rows[i]["PersonResNdDeadline"].ToString();
                    }
                }


            }
        }

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        //btnBack.Text = "Cancel";
        btnUpdate.Visible = false;
        btnUpdateNew.Visible = false;
        // btnImport.Visible = false;
        MultiView1.ActiveViewIndex = 1;
        GridViewAccSheet.DataSource = null;
        GridViewAccSheet.DataBind();
        GridViewAccSheetedit.DataSource = null;
        GridViewAccSheetedit.DataBind();
        //  FillStudent();
    }
    //protected void btnLoadDataEdit_Click(object sender, EventArgs e)
    //{
    //    tdMsg.InnerHtml = "";
    //    if (ddlDate.SelectedIndex != 0)
    //    {
    //        loadDataListEdit();
    //    }
    //    else
    //    {
    //        tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Date...");
    //    }
    //}

    //public bool ValidateDenominator(TextBox txtnooftime)
    //{
    //    if (txtnooftime.Text == "")
    //    {
    //        txtnooftime.Focus();
    //        txtnooftime.Style.Add("border-color", "red");
    //        txtnooftime.BorderStyle = BorderStyle.Solid;
    //        return false;
            
    //    }
    //    else
    //    {
    //        txtnooftime.BorderStyle = BorderStyle.None;
    //        return true;
    //    }
    //}
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        objData = new clsData();
        int testupdate = 0;
        int deadlineCheck = 1;
        int PersonRespCheck = 1;
        int AccShtId = 0;
        //foreach (GridViewRow row in GridViewAccSheetedit.Rows)
        //{
        //    HiddenField hdFldAcdId = row.Controls[0].FindControl("hdFldAcdId") as HiddenField;
        //    AccShtId = Convert.ToInt32(hdFldAcdId.Value);
        //    TextBox txtFreeText = row.Controls[0].FindControl("txtFreetxtedit") as TextBox;
        //    TextBox txtPersDissc = row.Controls[0].FindControl("txtPersDisscedit") as TextBox;
        //    TextBox txtResAndDeadline = row.Controls[0].FindControl("txtResAndDeadlineedit") as TextBox;
        //    //clsGeneral.convertQuotes( txtFreeText.Text.Trim() )
        //    string strqury = "update StdtAcdSheet set FeedBack='" + clsGeneral.convertQuotes(txtFreeText.Text.Trim()) +
        //        "',PreposalDiss='" + clsGeneral.convertQuotes(txtPersDissc.Text.Trim()) + "',PersonResNdDeadline='"
        //        + clsGeneral.convertQuotes(txtResAndDeadline.Text.Trim()) + "' WHERE AccSheetId=" + hdFldAcdId.Value + "";
        //    testupdate += objData.Execute(strqury);
        //}
            DataTable dtAccSheetId = new DataTable();
            DataColumn dc = new DataColumn("AccSheetId", typeof(Int32));
            dtAccSheetId.Columns.Add(dc);
            DataRow dr = dtAccSheetId.NewRow();
            foreach(GridViewRow row3 in GridViewAccSheetedit.Rows)
            {
                dr = dtAccSheetId.NewRow();
                HiddenField hdFldAcdId = row3.Controls[0].FindControl("hdFldAcdId") as HiddenField;
                AccShtId = Convert.ToInt32(hdFldAcdId.Value);
                dr["AccSheetId"] = AccShtId;
                dtAccSheetId.Rows.Add(dr);
            }
            if (dtAccSheetId != null)
            {
                foreach (DataRow row4 in dtAccSheetId.Rows)
                {
                    foreach (GridViewRow row2 in gvAgendaItem.Rows)
                    {
                    Label lblAgendaItemId = (Label)row2.FindControl("LblAgendaItemId");
                    TextBox txtAgendaItem = (TextBox)row2.FindControl("txtAgendaItem");
                    TextBox txtStaffInitials = (TextBox)row2.FindControl("txtStaffInitials");
                    TextBox txtDateAdded = (TextBox)row2.FindControl("txtDateAdded");
                    RadioButtonList RadBtnListDoneCarryOver = (RadioButtonList)row2.FindControl("RBLDoneCarry");
                    DataTable dtAccSheetId2 = new DataTable();
                    dtAccSheetId2 = null;
                    if (lblAgendaItemId.Text != "")
                    {
                        string accSheetIdQry = "select AccSheetId from AcdSheetMtng where MtngId = " + lblAgendaItemId.Text;
                        dtAccSheetId2 = objData.ReturnDataTable(accSheetIdQry, false);
                    }
                    
                    if (txtAgendaItem.Text != "")
                    {
                        if(dtAccSheetId2 != null)
                        {
                            if (row4["AccSheetId"].ToString() != dtAccSheetId2.Rows[0]["AccSheetId"].ToString())
                                continue;
                        }
                        
                        if (lblAgendaItemId.Text != "")
                        {
                            //update
                            string strqury = "update AcdSheetMtng set AgendaItem='" + clsGeneral.convertQuotes(txtAgendaItem.Text.Trim()) + "',StaffInitials='" + clsGeneral.convertQuotes(txtStaffInitials.Text.Trim()) +
                                "',AgendaAddedDate='" + clsGeneral.convertQuotes(txtDateAdded.Text) + "',DoneCarryOver='" + RadBtnListDoneCarryOver.SelectedValue.ToString() + "', ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() where MtngId=" + lblAgendaItemId.Text + "";
                            testupdate += Convert.ToInt32(objData.Execute(strqury));
                        }
                        else
                        {
                            //save
                            string strqury = "insert into AcdSheetMtng (AccSheetId,LessonPlanId,AgendaItem,StaffInitials,AgendaAddedDate,DoneCarryOver,ActiveInd,CreatedBy,CreatedOn,followstatus) " +
                                "values (" + AccShtId + ",(select LessonPlanId from StdtAcdSheet where AccSheetId=" + AccShtId + "),'" + clsGeneral.convertQuotes(txtAgendaItem.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtStaffInitials.Text.Trim()) +
                                "','" + clsGeneral.convertQuotes(txtDateAdded.Text) + "','" + RadBtnListDoneCarryOver.SelectedValue.ToString() + "', 'A'," + sess.LoginId + ",GETDATE(),'A')";
                            testupdate += Convert.ToInt32(objData.Execute(strqury));
                            DataTable dt = new DataTable();
                            dt = objData.ReturnDataTable("select top 1 MtngId from AcdSheetMtng order by 1 desc",false);
                            lblAgendaItemId.Text = dt.Rows[0]["MtngId"].ToString();
                        }
                    }
                    else
                    {
                        //PersonRespCheck = 0;
                        //testupdate = 0;
                        //break;
                    }


                }
            }
            }

        foreach (GridViewRow row in GridViewAccSheetedit.Rows)
        {
            HiddenField hdFldAcdId = row.Controls[0].FindControl("hdFldAcdId") as HiddenField;
            AccShtId = Convert.ToInt32(hdFldAcdId.Value);
          //  testupdate = 0;

            Label lblNoOfPos1 = row.Controls[0].FindControl("lblNoOfPos8") as Label;
            Label lblNoOfPos2 = row.Controls[0].FindControl("lblNoOfPos9") as Label;
            Label lblNoOfPos3 = row.Controls[0].FindControl("lblNoOfPos10") as Label;
            Label lblNoOfPos4 = row.Controls[0].FindControl("lblNoOfPos11") as Label;
            Label lblNoOfPos5 = row.Controls[0].FindControl("lblNoOfPos12") as Label;
            Label lblNoOfPos6 = row.Controls[0].FindControl("lblNoOfPos13") as Label;
            Label lblNoOfPos7 = row.Controls[0].FindControl("lblNoOfPos14") as Label;

            TextBox txtNoOfPos1 = row.Controls[0].FindControl("txtNoOfPos8") as TextBox;
            TextBox txtNoOfPos2 = row.Controls[0].FindControl("txtNoOfPos9") as TextBox;
            TextBox txtNoOfPos3 = row.Controls[0].FindControl("txtNoOfPos10") as TextBox;
            TextBox txtNoOfPos4 = row.Controls[0].FindControl("txtNoOfPos11") as TextBox;
            TextBox txtNoOfPos5 = row.Controls[0].FindControl("txtNoOfPos12") as TextBox;
            TextBox txtNoOfPos6 = row.Controls[0].FindControl("txtNoOfPos13") as TextBox;
            TextBox txtNoOfPos7 = row.Controls[0].FindControl("txtNoOfPos14") as TextBox;

            RadioButtonList radioButtonList1 = row.Controls[0].FindControl("radioButtonList8") as RadioButtonList;
            RadioButtonList radioButtonList2 = row.Controls[0].FindControl("radioButtonList9") as RadioButtonList;
            RadioButtonList radioButtonList3 = row.Controls[0].FindControl("radioButtonList10") as RadioButtonList;
            RadioButtonList radioButtonList4 = row.Controls[0].FindControl("radioButtonList11") as RadioButtonList;
            RadioButtonList radioButtonList5 = row.Controls[0].FindControl("radioButtonList12") as RadioButtonList;
            RadioButtonList radioButtonList6 = row.Controls[0].FindControl("radioButtonList13") as RadioButtonList;
            RadioButtonList radioButtonList7 = row.Controls[0].FindControl("radioButtonList14") as RadioButtonList;



            //bool result=true;
            //result= ValidateDenominator(txtNoOfPos1);
            //if (result == false) break;
            //result = ValidateDenominator(txtNoOfPos2);
            //if (result == false) break;
            //result = ValidateDenominator(txtNoOfPos3);
            //if (result == false) break;
            //result = ValidateDenominator(txtNoOfPos4);
            //if (result == false) break;
            //result = ValidateDenominator(txtNoOfPos5);
            //if (result == false) break;
            //result = ValidateDenominator(txtNoOfPos6);
            //if (result == false) break;
            //result = ValidateDenominator(txtNoOfPos7);
            //if (result == false) break;
            
            string NumPos1 = lblNoOfPos1.Text + "/" + txtNoOfPos1.Text;
            string NumPos2 = lblNoOfPos2.Text + "/" + txtNoOfPos2.Text;
            string NumPos3 = lblNoOfPos3.Text + "/" + txtNoOfPos3.Text;
            string NumPos4 = lblNoOfPos4.Text + "/" + txtNoOfPos4.Text;
            string NumPos5 = lblNoOfPos5.Text + "/" + txtNoOfPos5.Text;
            string NumPos6 = lblNoOfPos6.Text + "/" + txtNoOfPos6.Text;
            string NumPos7 = lblNoOfPos7.Text + "/" + txtNoOfPos7.Text;

            string radBtnList1 = radioButtonList1.SelectedValue.ToString();
            string radBtnList2 = radioButtonList2.SelectedValue.ToString();
            string radBtnList3 = radioButtonList3.SelectedValue.ToString();
            string radBtnList4 = radioButtonList4.SelectedValue.ToString();
            string radBtnList5 = radioButtonList5.SelectedValue.ToString();
            string radBtnList6 = radioButtonList6.SelectedValue.ToString();
            string radBtnList7 = radioButtonList7.SelectedValue.ToString();

            string attendees = AttendeesText.Text;
            string IEPyear = Iepyeartxt.Text;
            string IEPDate = Ieptxt.Text;
            string review = ReviewbydateUpdate.Text;
            int c1, c2, c3;
            HtmlInputCheckBox checkbox1 = row.FindControl("Checkbox1") as HtmlInputCheckBox;
            HtmlInputCheckBox checkbox2 = row.FindControl("Checkbox2") as HtmlInputCheckBox;
            HtmlInputCheckBox checkbox3 = row.FindControl("Checkbox3") as HtmlInputCheckBox;
            if (checkbox1.Checked)
            {
                c1 = 1;
            }
            else { c1 = 0; }
            if (checkbox2.Checked)
            {
                c2 = 1;
            }
            else { c2 = 0; }
            if (checkbox3.Checked)
            {
                c3 = 1;
            }
            else { c3 = 0; }

            string UpdateAcdsht = "UPDATE StdtAcdSheet SET NoOfTimes1='" + NumPos1 + "',NoOfTimes2='" + NumPos2 + "',NoOfTimes3='" + NumPos3 + "',NoOfTimes4='" + NumPos4 + "',NoOfTimes5='" + NumPos5 + "',NoOfTimes6='" + NumPos6 + "',NoOfTimes7='" + NumPos7 +
                                  "',Attendees='" + clsGeneral.convertQuotes(attendees) + "',IEPYear='" + clsGeneral.convertQuotes(IEPyear) + "',IEPSigDate='" + clsGeneral.convertQuotes(IEPDate) + "',Reviewed='" +clsGeneral.convertQuotes( review) + "',MetObjective='" + c1 + "',MetGoal='" + c2 + "',NotMaintaining='" + c3 +
                                  "',Progressing1='"+radBtnList1+"',Progressing2='"+radBtnList2+"',Progressing3='"+radBtnList3+"',Progressing4='"+radBtnList4+"',Progressing5='"+radBtnList5+"',Progressing6='"+radBtnList6+"',Progressing7='"+radBtnList7+"' " +
                                  "WHERE AccSheetId=" + AccShtId + "";
            objData.Execute(UpdateAcdsht);

            int personResp = 0;
            string textVal = "";
            string userFName = "";
            string userLName = "";
            string userQry = "";
            GridView gvChild = (GridView)row.FindControl("gvCMeetingEdit");
            if (gvChild != null)
            {
                foreach (GridViewRow row2 in gvChild.Rows)
                {
                    Label lblCMtngIdEdit = (Label)row2.FindControl("lblCMtngIdEdit");
                    TextBox txtCFollowUpEdit = (TextBox)row2.FindControl("txtCFollowUpEdit");
                    TextBox txtPersonResponsibleEdit = (TextBox)row2.FindControl("txtPersonResponsibleEdit");
                    TextBox txtCPersonResponsibleEdit = (TextBox)row2.FindControl("txtCPersonResponsibleEdit");
                    TextBox txtCDeadlinesEdit = (TextBox)row2.FindControl("txtCDeadlinesEdit");
                    personResp = 0;
                    
                    DateTime parsed;
                    if (DateTime.TryParseExact(txtCDeadlinesEdit.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed) 
                        || txtCDeadlinesEdit.Text == "")
                    {
                        if (txtPersonResponsibleEdit.Text != "")
                        {
                            textVal = txtPersonResponsibleEdit.Text;
                            if (textVal.Contains(","))
                            {
                                userFName = clsGeneral.convertQuotes(textVal.Substring(textVal.IndexOf(",") + 1).Trim());
                                userLName = clsGeneral.convertQuotes(textVal.Substring(0, textVal.IndexOf(",")).Trim());
                            }
                            else
                            {
                                userFName = userLName = clsGeneral.convertQuotes(textVal.Trim());
                            }
                            userQry = "select UserId from [User] where UserLName = '" + userLName + "' and UserFName = '" + userFName + "'";
                            personResp = Convert.ToInt32(objData.FetchValue(userQry));
                            if (personResp == 0)
                            {
                                PersonRespCheck = 0;
                                testupdate = 0;
                                break;
                            }
                        }
                        if (lblCMtngIdEdit.Text != "")
                        {
                            //update
                            string strqury = "update AcdSheetMtng set PropAndDisc='" + clsGeneral.convertQuotes(txtCFollowUpEdit.Text.Trim()) + "',PersonResp='" + personResp + "',Deadlines=CASE WHEN'" + txtCDeadlinesEdit.Text + "' = '' THEN NULL ELSE '" + txtCDeadlinesEdit.Text + "' END,ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() where MtngId=" + lblCMtngIdEdit.Text + "";
                            testupdate += Convert.ToInt32(objData.Execute(strqury));
                        }
                        else
                        {
                            //save
                            if (txtCFollowUpEdit.Text == "" && txtPersonResponsibleEdit.Text == "" && txtCDeadlinesEdit.Text == "")
                            {
                            }
                            else
                            {
                                string strqury = "insert into AcdSheetMtng (AccSheetId,LessonPlanId,PropAndDisc,PersonResp,Deadlines,ActiveInd,CreatedBy,CreatedOn,followstatus) " +
                                    "values (" + AccShtId + ",(select LessonPlanId from StdtAcdSheet where AccSheetId=" + AccShtId + "),'" + clsGeneral.convertQuotes(txtCFollowUpEdit.Text.Trim()) + "','" + personResp + "',CASE WHEN'" + txtCDeadlinesEdit.Text + "' = '' THEN NULL ELSE '" + txtCDeadlinesEdit.Text + "' END,'A'," + sess.LoginId + ",GETDATE(),'C')";
                                testupdate += Convert.ToInt32(objData.Execute(strqury));
                            }
                        }
                    }
                    else
                    {
                        deadlineCheck = 0;
                        testupdate = 0;
                        break;
                    }
                        
            }
            }
            if (deadlineCheck == 0 || PersonRespCheck == 0)
                break;
            GridView gvCPhild = (GridView)row.FindControl("gvPMeetingEdit");
            if (gvCPhild != null && deadlineCheck==1)
            {
                foreach (GridViewRow row2 in gvCPhild.Rows)
                {
                    Label lblPMtngIdEdit = (Label)row2.FindControl("lblPMtngIdEdit");
                    TextBox txtPFollowUpEdit = (TextBox)row2.FindControl("txtPFollowUpEdit");
                    //TextBox txtPersonResponsibleEdit = (TextBox)row2.FindControl("txtPPersonResponsibleEdit");
                    TextBox txtPPersonResponsibleEdit = (TextBox)row2.FindControl("txtPPersonResponsibleEdit");
                    TextBox txtPDeadlinesEdit = (TextBox)row2.FindControl("txtPDeadlinesEdit");
                    personResp = 0;

                    DateTime parsed;

                    if (DateTime.TryParseExact(txtPDeadlinesEdit.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed) 
                        || txtPDeadlinesEdit.Text == "")
                    {
                        if (txtPPersonResponsibleEdit.Text != "")
                        {
                            textVal = txtPPersonResponsibleEdit.Text;
                            if (textVal.Contains(","))
                            {
                                userFName = clsGeneral.convertQuotes(textVal.Substring(textVal.IndexOf(",") + 1).Trim());
                                userLName = clsGeneral.convertQuotes(textVal.Substring(0, textVal.IndexOf(",")).Trim());
                            }
                            else
                            {
                                userFName = userLName = clsGeneral.convertQuotes(textVal.Trim());
                            }
                            userQry = "select UserId from [User] where UserLName = '" + userLName + "' and UserFName = '" + userFName + "'";
                            personResp = Convert.ToInt32(objData.FetchValue(userQry));
                            if (personResp == 0) 
                            {
                                PersonRespCheck = 0;
                                testupdate = 0;
                                break;
                            }
                        }
                        if (lblPMtngIdEdit.Text != "")
                        {
                            //update
                            string strqury = "update AcdSheetMtng set PropAndDisc='" + clsGeneral.convertQuotes(txtPFollowUpEdit.Text.Trim()) + "',PersonResp='" + personResp + "',Deadlines=CASE WHEN'" + txtPDeadlinesEdit.Text + "' = '' THEN NULL ELSE '" + txtPDeadlinesEdit.Text + "' END,ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() where MtngId=" + lblPMtngIdEdit.Text + "";
                            testupdate += Convert.ToInt32(objData.Execute(strqury));

                        }
                        else
                        {
                            //save
                            if (txtPFollowUpEdit.Text == "" && txtPPersonResponsibleEdit.Text == "" && txtPDeadlinesEdit.Text == "")
                            {

                            }
                            else
                            {
                                string strqury = "insert into AcdSheetMtng (AccSheetId,LessonPlanId,PropAndDisc,PersonResp,Deadlines,ActiveInd,CreatedBy,CreatedOn,followstatus) " +
                                    "values (" + AccShtId + ",(select LessonPlanId from StdtAcdSheet where AccSheetId=" + AccShtId + "),'" + clsGeneral.convertQuotes(txtPFollowUpEdit.Text.Trim()) + "','" + personResp + "',CASE WHEN'" + txtPDeadlinesEdit.Text + "' = '' THEN NULL ELSE '" + txtPDeadlinesEdit.Text + "' END,'A'," + sess.LoginId + ",GETDATE(),'P')";
                                testupdate += Convert.ToInt32(objData.Execute(strqury));
                            }

                        }
                    }
                    else
                    {
                        deadlineCheck = 0;
                        testupdate = 0;
                        break;
                    }
                    
                }
            }

        }

        if (testupdate > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Succesfully   ");
            //  loadDataListEdit();
            //btnBack.Text = "Back";
            FillData();
            Loadlesson2();
        }
        else
        {
            if (PersonRespCheck == 0)
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data Not updated (Invalid Username)");
            else if (deadlineCheck == 0)
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data Not updated (Invalid Date Format)");
            else
            tdMsg.InnerHtml = clsGeneral.warningMsg("Data Not updated");
        }

    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        AllInOne();

        string path = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }


    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        downloadfile();
    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        tdMsgExport.InnerHtml = "";
        //string path = Server.MapPath("~\\StudentBinder") + "\\ACMerge";
        string FileName = ViewState["FileName"].ToString();
        if (System.IO.File.Exists(FileName))
        {
            File.Delete(FileName);
        }
        //Array.ForEach(Directory.GetFiles(path), File.Delete);
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

    public void replaceWithTextsSingle(MainDocumentPart mainPart, string plcT, string TextT)
    {

        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);

        string textData = "";
        if (TextT != null)
        {
            textData = TextT;
        }
        else
        {
            textData = "";
        }

        var paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);

        string textDataNoSpace = textData.Replace(" ", "");
        foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
        {
            var paragraphs = converter.Parse(textData);
            if (paragraphs.Count == 0)
            {
                DocumentFormat.OpenXml.Wordprocessing.Paragraph tempPara = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                para.Parent.Append(tempPara);
            }
            else
            {
                for (int k = 0; k < paragraphs.Count; k++)
                {
                    bool isBullet = false;
                    if (textDataNoSpace.Contains("<li>" + paragraphs[k].InnerText.Trim()))
                        isBullet = true;
                    if (isBullet)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties paraProp = new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties();
                        ParagraphStyleId paraStyleid = new ParagraphStyleId() { Val = "BulletPara" };
                        NumberingProperties numProp = new NumberingProperties();
                        NumberingLevelReference numLvlRef = new NumberingLevelReference() { Val = 0 };
                        NumberingId numID = new NumberingId() { Val = 1 };
                        numProp.Append(numLvlRef);
                        numProp.Append(numID);
                        paraProp.Append(paraStyleid);
                        paraProp.Append(numProp);

                        if (((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties != null)
                        {
                            //Assign Bullet point property to paragraph
                            ((DocumentFormat.OpenXml.Wordprocessing.Paragraph)paragraphs[k]).ParagraphProperties.Append(paraProp);
                        }
                    }
                    para.Parent.Append(paragraphs[k]);
                }
            }
            //para.RemoveAllChildren<Run>();
        }
        paras = mainPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Where(element => element.InnerText == plcT);
        foreach (DocumentFormat.OpenXml.Wordprocessing.Paragraph para in paras)
        {
            para.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Run>();
        }
    }
    //protected void txtSdate_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtSdate.Text != null && txtSdate.Text != "")
    //    {
    //        DateTime strtDate;
    //        strtDate = Convert.ToDateTime(txtSdate.Text);
    //        //txtSdate.Text = DateTime.Now.Date.ToString("MM'/'dd'/'yyyy");
    //        DateTime Edate = strtDate.AddDays(98);
    //        txtEdate.Text = Edate.Date.ToString("MM'/'dd'/'yyyy");
    //    }
    //}
    
    protected void GridViewAccSheet_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridViewAccSheetedit_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GridViewAccSheetedit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView gvP = (GridView)e.Row.FindControl("gvPMeetingEdit");
            gvP.DataSource = dtPMeeting;
            gvP.DataBind();

            GridView gvC = (GridView)e.Row.FindControl("gvCMeetingEdit");
            gvC.DataSource = dtCMeeting;
            gvC.DataBind();
        }
    }
    [WebMethod]
    public static string[] GetAutoCompleteData(string prefix)
    {
        List<string> names = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select top 10 UserLName+', '+UserFName as Name from [User] where ActiveInd='A' and (UserLName like @SearchText + '%' or UserFName like @SearchText + '%')";
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //names.Add(string.Format("{0}-{1}", sdr["Name"], sdr["Id"]));

                        names.Add(sdr["Name"].ToString());

                    }

                }

            }
            conn.Close();
        }
        return names.ToArray();

    }

    public void LoadPMeetingGV()
    {
        dtPMeeting.Columns.Add("PFollowUpEdit");
        dtPMeeting.Columns.Add("PMtngIdEdit");
        dtPMeeting.Columns.Add("PPersonResponsibleEdit");
        dtPMeeting.Columns.Add("PDeadlinesEdit");

        DataRow dr = dtPMeeting.NewRow();
        dr["PFollowUpEdit"] = "";
        dr["PMtngIdEdit"] = "";
        dr["PPersonResponsibleEdit"] = "";
        dr["PDeadlinesEdit"] = "";
        dtPMeeting.Rows.Add(dr);
    }

    public void LoadPMeetingGVNew()
    {
        dtPMeeting.Columns.Add("PFollowUp");
        dtPMeeting.Columns.Add("PMtngId");
        dtPMeeting.Columns.Add("PPersonResponsible");
        dtPMeeting.Columns.Add("PDeadlines");

        DataRow dr = dtPMeeting.NewRow();
        dr["PFollowUp"] = "";
        dr["PMtngId"] = "";
        dr["PPersonResponsible"] = "";
        dr["PDeadlines"] = "";
        dtPMeeting.Rows.Add(dr);
    }

    public void LoadCMeetingGV()
    {
        dtCMeeting.Columns.Add("CFollowUpEdit");
        dtCMeeting.Columns.Add("CMtngIdEdit");
        dtCMeeting.Columns.Add("PersonResponsibleEdit");
        dtCMeeting.Columns.Add("CPersonResponsibleEdit");
        dtCMeeting.Columns.Add("CDeadlinesEdit");

        DataRow dr = dtCMeeting.NewRow();
        dr["CFollowUpEdit"] = "";
        dr["CMtngIdEdit"] = "";
        dr["PersonResponsibleEdit"] = "";
        dr["CPersonResponsibleEdit"] = "";
        dr["CDeadlinesEdit"] = "";
        dtCMeeting.Rows.Add(dr);
    }

    public void LoadCMeetingGVNew()
    {
        dtCMeeting.Columns.Add("CFollowUp");
        dtCMeeting.Columns.Add("CMtngId");
        dtCMeeting.Columns.Add("PersonResponsible");
        dtCMeeting.Columns.Add("CPersonResponsible");
        dtCMeeting.Columns.Add("CDeadlines");

        DataRow dr = dtCMeeting.NewRow();
        dr["CFollowUp"] = "";
        dr["CMtngId"] = "";
        dr["PersonResponsible"] = "";
        dr["CPersonResponsible"] = "";
        dr["CDeadlines"] = "";
        dtCMeeting.Rows.Add(dr);
    }
    protected void gvAgendaItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddRow")
        {
            GridView gvC = (GridView)sender;
            addRowAgendaEdit(gvC);
        }
        if (e.CommandName == "deleteRow")
        {
            GridViewRow gvr = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

            int RowIndex = gvr.RowIndex;
            int AgItmId = 0;
            GridView gvC = (GridView)sender;
            string cArg = e.CommandArgument.ToString();
            if (cArg != "")
            {
                AgItmId = int.Parse(e.CommandArgument.ToString());
                objData = new clsData();
                string sQuery = "delete from AcdSheetMtng where MtngId=" + AgItmId;
                objData.Execute(sQuery);
                delRowAgendaItem(RowIndex);
                //gvAgendaItem.DeleteRow(RowIndex);
            }
            else
            {
                delRowAgendaItem(RowIndex);
            }
        }
    }
    //protected void gvAgendaItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    GridView gvC = (GridView)sender;
    //    int rowIndex = e.RowIndex;
    //    ArrangeGVAgendaItem(gvC, rowIndex);
    //}
    protected void delRowAgendaItem(int id)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("AgendaItemId", typeof(string));
        dt.Columns.Add("AgendaItem", typeof(string));
        dt.Columns.Add("StaffInitials", typeof(string));
        dt.Columns.Add("DateAdded", typeof(string));
        dt.Columns.Add("DoneCarryOver", typeof(string));

        foreach (GridViewRow row2 in gvAgendaItem.Rows)
        {
            Label lblAgendaItemId = (Label)row2.FindControl("LblAgendaItemId");
            TextBox txtAgendaItem = (TextBox)row2.FindControl("txtAgendaItem");
            TextBox txtStaffInitials = (TextBox)row2.FindControl("txtStaffInitials");
            TextBox txtDateAdded = (TextBox)row2.FindControl("txtDateAdded");
            RadioButtonList RadBtnListDoneCarryOver = (RadioButtonList)row2.FindControl("RBLDoneCarry");

            dt.Rows.Add(lblAgendaItemId.Text.ToString(), txtAgendaItem.Text.ToString(), txtStaffInitials.Text.ToString(), txtDateAdded.Text.ToString(), RadBtnListDoneCarryOver.SelectedValue.ToString());
        }

        if(id<dt.Rows.Count)
        dt.Rows[id].Delete();

        gvAgendaItem.DataSource = dt;
        gvAgendaItem.DataBind();
        int i = 0;
        foreach (GridViewRow row3 in gvAgendaItem.Rows)
        {
            RadioButtonList radBtnLst1 = row3.FindControl("RBLDoneCarry") as RadioButtonList;
            if (dt.Rows[i]["DoneCarryOver"].ToString() != "" && i < dt.Rows.Count)
            {
                radBtnLst1.SelectedValue = dt.Rows[i++]["DoneCarryOver"].ToString();
            }
        }

    }
    protected void ArrangeGVAgendaItem(GridView gvC, int rowIndex)
    {
        dtAgendaItem.Columns.Add("AgendaItemId", typeof(string));
        dtAgendaItem.Columns.Add("AgendaItem", typeof(string));
        dtAgendaItem.Columns.Add("StaffInitials", typeof(string));
        dtAgendaItem.Columns.Add("DateAdded", typeof(string));
        dtAgendaItem.Columns.Add("DoneCarryOver", typeof(string));


        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtAgendaItem.NewRow();
            drTemp["AgendaItemId"] = ((Label)gvr.FindControl("LblAgendaItemId")).Text;
            drTemp["AgendaItem"] = ((TextBox)gvr.FindControl("txtAgendaItem")).Text;
            drTemp["StaffInitials"] = ((TextBox)gvr.FindControl("txtStaffInitials")).Text;
            drTemp["DateAdded"] = ((TextBox)gvr.FindControl("txtDateAdded")).Text;
            drTemp["DoneCarryOver"] = ((RadioButtonList)gvr.FindControl("RBLDoneCarry")).SelectedValue;
            dtAgendaItem.Rows.Add(drTemp);
        }
        if (dtAgendaItem.Rows.Count > 0)
        {
            dtAgendaItem.Rows.RemoveAt(rowIndex);
        }
        if (dtAgendaItem.Rows.Count == 0)
        {
            DataRow dr = dtAgendaItem.NewRow();
            dr["AgendaItemId"] = "";
            dr["AgendaItem"] = "";
            dr["StaffInitials"] = "";
            dr["DateAdded"] = "";
            dr["DoneCarryOver"] = "";
            dtAgendaItem.Rows.Add(dr);
        }
        gvC.DataSource = dtAgendaItem;
        gvC.DataBind();
    }
    public void addRowAgendaEdit(GridView gvC)
    {

        dtAgendaItem.Columns.Add("AgendaItemId", typeof(string));
        dtAgendaItem.Columns.Add("AgendaItem", typeof(string));
        dtAgendaItem.Columns.Add("StaffInitials", typeof(string));
        dtAgendaItem.Columns.Add("DateAdded", typeof(string));
        dtAgendaItem.Columns.Add("DoneCarryOver", typeof(string));

        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtAgendaItem.NewRow();
            drTemp["AgendaItemId"] = ((Label)gvr.FindControl("LblAgendaItemId")).Text;
            drTemp["AgendaItem"] = ((TextBox)gvr.FindControl("txtAgendaItem")).Text;
            drTemp["StaffInitials"] = ((TextBox)gvr.FindControl("txtStaffInitials")).Text;
            drTemp["DateAdded"] = ((TextBox)gvr.FindControl("txtDateAdded")).Text;
            drTemp["DoneCarryOver"] = ((RadioButtonList)gvr.FindControl("RBLDoneCarry")).SelectedValue;
            dtAgendaItem.Rows.Add(drTemp);
        }
        DataRow dr = dtAgendaItem.NewRow();
        dr["AgendaItemId"] = "";
        dr["AgendaItem"] = "";
        dr["StaffInitials"] = "";
        dr["DateAdded"] = "";
        dr["DoneCarryOver"] = "";
        dtAgendaItem.Rows.Add(dr);
        gvC.DataSource = dtAgendaItem;
        gvC.DataBind();
        int i = 0;
        foreach (GridViewRow row3 in gvAgendaItem.Rows)
        {
            RadioButtonList radBtnLst1 = row3.FindControl("RBLDoneCarry") as RadioButtonList;
            if (dtAgendaItem.Rows[i]["DoneCarryOver"].ToString() != "" && i < dtAgendaItem.Rows.Count)
            {
                radBtnLst1.SelectedValue = dtAgendaItem.Rows[i++]["DoneCarryOver"].ToString();
            }
        }
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "somekey", "destroy();", true);
    }
    protected void loadAgendaItemGV()
    {
        dtAgendaItem.Columns.Add("AgendaItemId");
        dtAgendaItem.Columns.Add("AgendaItem");
        dtAgendaItem.Columns.Add("StaffInitials");
        dtAgendaItem.Columns.Add("DateAdded");
        dtAgendaItem.Columns.Add("DoneCarryOver");


        DataRow dr = dtAgendaItem.NewRow();
        dr["AgendaItemId"] = "";
        dr["AgendaItem"] = "";
        dr["StaffInitials"] = "";
        dr["DateAdded"] = "";
        dr["DoneCarryOver"] = "";
        dtAgendaItem.Rows.Add(dr);
        DataTable dt = new DataTable();
        dt.Columns.Add("AgendaItemId", typeof(int));
        dt.Columns.Add("AgendaItem", typeof(string));
        dt.Columns.Add("StaffInitials", typeof(string));
        dt.Columns.Add("DateAdded", typeof(string));
        dt.Columns.Add("DoneCarryOver", typeof(string));


        gvAgendaItem.DataSource = dtAgendaItem;
        gvAgendaItem.DataBind();
        gvAgendaItem.Visible = true;
    }
    public void LoadMeetings(string date)
    {
        objData = new clsData();
        string startDate = "";
        string endDate = "";
        if (date != null)
        {
            startDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
        DataTable dtMtngId = new DataTable();
        DataTable Dt1 = new DataTable();
        DataTable Dt2 = new DataTable();
        DataTable dtPMtng = new DataTable();
        dtPMtng.Columns.Add(new DataColumn("PMtngIdEdit", typeof(Int32)));
        dtPMtng.Columns.Add(new DataColumn("PFollowUpEdit", typeof(string)));
        dtPMtng.Columns.Add(new DataColumn("PersonResponsibleEdit", typeof(string)));
        dtPMtng.Columns.Add(new DataColumn("PPersonResponsibleEdit", typeof(string)));
        dtPMtng.Columns.Add(new DataColumn("PDeadlinesEdit", typeof(string)));
        DataTable dtCMtng = new DataTable();
        dtCMtng.Columns.Add(new DataColumn("CMtngIdEdit", typeof(Int32)));
        dtCMtng.Columns.Add(new DataColumn("CFollowUpEdit", typeof(string)));
        dtCMtng.Columns.Add(new DataColumn("PersonResponsibleEdit", typeof(string)));
        dtCMtng.Columns.Add(new DataColumn("CPersonResponsibleEdit", typeof(Int32)));
        dtCMtng.Columns.Add(new DataColumn("CDeadlinesEdit", typeof(string)));
        
        DataTable dtAgItm = new DataTable();
        dtAgItm.Columns.Add(new DataColumn("AgendaItemId", typeof(Int32)));
        dtAgItm.Columns.Add(new DataColumn("AgendaItem", typeof(string)));
        dtAgItm.Columns.Add(new DataColumn("StaffInitials", typeof(string)));
        dtAgItm.Columns.Add(new DataColumn("DateAdded", typeof(string)));
        dtAgItm.Columns.Add(new DataColumn("DoneCarryOver", typeof(string)));
        DataTable Dt3 = new DataTable();


        string agItmQry = "select MtngId from AcdSheetMtng where followstatus = 'A' and ActiveInd='A' and AccSheetId in " +
                          "(select AccSheetId from StdtAcdSheet where studentid=" + sess.StudentId + " and EndDate = '" + endDate + "')";
        DataTable dtAgendaId = objData.ReturnDataTable(agItmQry, false);
        int agendaId = 0;
        if(dtAgendaId != null )
        {
            foreach (DataRow row1 in dtAgendaId.Rows)
            {
                agendaId = Convert.ToInt32(row1["MtngId"]);
                string Aquery = "select MtngId as AgendaItemId, AgendaItem, StaffInitials, AgendaAddedDate as DateAdded, DoneCarryOver from AcdSheetMtng where MtngId =" + agendaId;
                Dt3 = objData.ReturnDataTable(Aquery, false);
                foreach (DataRow row2 in Dt3.Rows)
                    if (row2["AgendaItem"].ToString() != "" || row2["StaffInitials"].ToString() != "")
                    {
                        if (row2["DateAdded"].ToString() != "")
                        {
                            string dateAdded = row2["DateAdded"].ToString().Replace("-", "/");
                            dtAgItm.Rows.Add(Convert.ToInt32(row2["AgendaItemId"]), row2["AgendaItem"].ToString(), row2["StaffInitials"].ToString(), dateAdded, row2["DoneCarryOver"].ToString());
                        }
                        else
                            dtAgItm.Rows.Add(Convert.ToInt32(row2["AgendaItemId"]), row2["AgendaItem"].ToString(), row2["StaffInitials"].ToString(), row2["DoneCarryOver"].ToString());
                    }
            }
            if (dtAgItm != null)
            {
                if (dtAgItm.Rows.Count > 0)
                {
                    gvAgendaItem.DataSource = dtAgItm;
                    gvAgendaItem.DataBind();
                    int i = 0;
                    foreach(GridViewRow row3 in gvAgendaItem.Rows)
                    {
                        RadioButtonList radBtnLst1 = row3.FindControl("RBLDoneCarry") as RadioButtonList;
                        if (dtAgItm.Rows[i]["DoneCarryOver"].ToString() != "" && i < dtAgItm.Rows.Count)
                        {
                            radBtnLst1.SelectedValue = dtAgItm.Rows[i++]["DoneCarryOver"].ToString();
                        }
                    }
                }
            }
        }

        foreach (GridViewRow row in GridViewAccSheetedit.Rows)
        {
            HiddenField hdFldAcdId = (HiddenField)row.FindControl("hdFldAcdId");
            HiddenField hfLPIdEdit = (HiddenField)row.FindControl("hfLPIdEdit");
            int AccShtId = Convert.ToInt32(hdFldAcdId.Value);
            string mtngIdQuery = "select MtngId from AcdSheetMtng where followstatus = 'P' and ActiveInd='A' and AccSheetId in (select AccSheetId from StdtAcdSheet where studentid=" + sess.StudentId + " and EndDate = '" + endDate + "' and LessonPlanId=" + hfLPIdEdit.Value + ")";
            dtMtngId = objData.ReturnDataTable(mtngIdQuery, false);
            string Pquery = "";
            string Cquery = "";
            int mtngId = 0;
            if (dtMtngId != null)
            {
                foreach(DataRow rows in dtMtngId.Rows)
                {
                    mtngId = Convert.ToInt32(rows["MtngId"]);
                    string userQryP = "select PersonResp from AcdSheetMtng where MtngId = " + mtngId;
                    
                    if(Convert.ToInt32(objData.FetchValue(userQryP)) == 0)
                        Pquery = "select MtngId as PMtngIdEdit, PropAndDisc as PFollowUpEdit,'' as PersonResponsibleEdit,'' as PPersonResponsibleEdit,CONVERT(VARCHAR,Deadlines,101) as PDeadlinesEdit from AcdSheetMtng where MtngId = " + mtngId;
                    else
                        Pquery = "select a.MtngId as PMtngIdEdit,a.PropAndDisc as PFollowUpEdit,u.UserLName+', '+u.UserFName as PersonResponsibleEdit,u.UserLName+', '+u.UserFName as PPersonResponsibleEdit,CONVERT(VARCHAR,a.Deadlines,101) as PDeadlinesEdit from AcdSheetMtng a join [User] u on u.UserId=a.PersonResp and a.MtngId = " + mtngId;
                    Dt1 = objData.ReturnDataTable(Pquery, false);
                        foreach (DataRow row1 in Dt1.Rows)
                        {
                            if (row1["PDeadlinesEdit"].ToString() != "" || row1["PersonResponsibleEdit"].ToString() != "" || row1["PPersonResponsibleEdit"].ToString() != "" || row1["PFollowUpEdit"].ToString() != "")
                            {
                                if (row1["PDeadlinesEdit"].ToString() != "")
                                {
                                    string deadlineDate = row1["PDeadlinesEdit"].ToString().Replace("-", "/");
                                    dtPMtng.Rows.Add(Convert.ToInt32(row1["PMtngIdEdit"]), row1["PFollowUpEdit"].ToString(), row1["PersonResponsibleEdit"].ToString(), row1["PPersonResponsibleEdit"].ToString(), deadlineDate);
                                }
                                else
                                    dtPMtng.Rows.Add(Convert.ToInt32(row1["PMtngIdEdit"]), row1["PFollowUpEdit"].ToString(), row1["PersonResponsibleEdit"].ToString(), row1["PPersonResponsibleEdit"].ToString()); 
                            }
                        }
                }
                if (dtPMtng != null)
                {
                    if (dtPMtng.Rows.Count > 0)
                    {
                        GridView gvPMtng = (GridView)row.FindControl("gvPMeetingEdit");
                        gvPMtng.DataSource = dtPMtng;
                        gvPMtng.DataBind();
                    }
                }

                mtngIdQuery = "select MtngId from AcdSheetMtng where  followstatus='C' and ActiveInd='A' and AccSheetId in (select AccSheetId from StdtAcdSheet where studentid=" + sess.StudentId + " and EndDate = '" + endDate + "' and LessonPlanId=" + hfLPIdEdit.Value + ")";
                dtMtngId = objData.ReturnDataTable(mtngIdQuery, false);
                foreach (DataRow rows in dtMtngId.Rows)
                {
                    mtngId = Convert.ToInt32(rows["MtngId"]);
                    string userQryC = "select PersonResp from AcdSheetMtng where MtngId = " + mtngId;

                    if (Convert.ToInt32(objData.FetchValue(userQryC)) == 0)
                        Cquery = "select MtngId as CMtngIdEdit, PropAndDisc as CFollowUpEdit,'' as PersonResponsibleEdit,PersonResp as CPersonResponsibleEdit,CONVERT(VARCHAR,Deadlines,101) as CDeadlinesEdit from AcdSheetMtng where followstatus='C' and ActiveInd='A' and MtngId=" + mtngId;
                    else
                        Cquery = "select a.MtngId as CMtngIdEdit,a.PropAndDisc as CFollowUpEdit,u.UserLName+', '+u.UserFName as PersonResponsibleEdit,u.UserId as CPersonResponsibleEdit, CONVERT(VARCHAR,a.Deadlines,101) as CDeadlinesEdit from AcdSheetMtng a join [User] u on u.UserId=a.PersonResp and a.followstatus='C' AND a.MtngId=" + mtngId + " and a.ActiveInd='A'";
                    Dt2 = objData.ReturnDataTable(Cquery, false);
                    foreach (DataRow row1 in Dt2.Rows)
                    {
                        if (row1["CDeadlinesEdit"].ToString() != "" || row1["PersonResponsibleEdit"].ToString() != "" || row1["CFollowUpEdit"].ToString() != "")
                        {
                            if (row1["CDeadlinesEdit"].ToString() != "")
                            {
                                string deadlineDate = row1["CDeadlinesEdit"].ToString().Replace("-", "/");
                                dtCMtng.Rows.Add(Convert.ToInt32(row1["CMtngIdEdit"]), row1["CFollowUpEdit"].ToString(), row1["PersonResponsibleEdit"].ToString(), Convert.ToInt32(row1["CPersonResponsibleEdit"]), deadlineDate);
                            }
                            else
                                dtCMtng.Rows.Add(Convert.ToInt32(row1["CMtngIdEdit"]), row1["CFollowUpEdit"].ToString(), row1["PersonResponsibleEdit"].ToString(), Convert.ToInt32(row1["CPersonResponsibleEdit"]));
                        }
                    }
                }

                if (dtCMtng != null)
                {
                    if (dtCMtng.Rows.Count > 0)
                    {
                        GridView gvCMtng = (GridView)row.FindControl("gvCMeetingEdit");
                        gvCMtng.DataSource = dtCMtng;
                        gvCMtng.DataBind();
                    }
                }
            }
            string Phaseline = "SELECT 'Major Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + startDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                            " (StdtSessEventType='Major') AND StudentId=" + sess.StudentId + " and LessonPlanId in(" + hfLPIdEdit.Value + ",0) AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
            System.Data.DataTable dtNew1 = objData.ReturnDataTable(Phaseline, false);
            string Conditionline = "SELECT 'Minor Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + startDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                                " (StdtSessEventType='Minor') AND StudentId=" + sess.StudentId + "and LessonPlanId in(" + hfLPIdEdit.Value + ",0) AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
            System.Data.DataTable dtNew2 = objData.ReturnDataTable(Conditionline, false);
            string Arrownotes = "SELECT 'Arrow notes' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + startDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                                " (StdtSessEventType='Arrow notes') AND StudentId=" + sess.StudentId + " and LessonPlanId in(" + hfLPIdEdit.Value + ",0) AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
            System.Data.DataTable dtNew3 = objData.ReturnDataTable(Arrownotes, false);
            System.Data.DataTable dtNew = new System.Data.DataTable();
            dtNew.Columns.Add("Eventname", typeof(string));
            dtNew.Columns.Add("Eventdata", typeof(string));
            System.Data.DataRow dr1 = dtNew.NewRow();
            dr1["Eventname"] = dtNew1.Rows[0]["Eventname"];
            string Result="";
            Result=dtNew1.Rows[0]["Eventdata"].ToString();
            if (Result == "")
            {
                dtNew1.Rows[0]["Eventdata"] = "No Results";
            }
            dr1["Eventdata"] = dtNew1.Rows[0]["Eventdata"];
            dtNew.Rows.Add(dr1);
            System.Data.DataRow dr2 = dtNew.NewRow();
            dr2["Eventname"] = dtNew2.Rows[0]["Eventname"];
            Result = dtNew2.Rows[0]["Eventdata"].ToString();
            if (Result == "")
            {
                dtNew2.Rows[0]["Eventdata"] = "No Results";
            }
            dr2["Eventdata"] = dtNew2.Rows[0]["Eventdata"];
            dtNew.Rows.Add(dr2);
            System.Data.DataRow dr3 = dtNew.NewRow();
            dr3["Eventname"] = dtNew3.Rows[0]["Eventname"];
            Result = dtNew3.Rows[0]["Eventdata"].ToString();
            if (Result == "")
            {
                dtNew3.Rows[0]["Eventdata"] = "No Results";
            }
            dr3["Eventdata"] = dtNew3.Rows[0]["Eventdata"];
            dtNew.Rows.Add(dr3);

            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    GridView GvtAcseting = (GridView)row.FindControl("grdGraphDataNew");
                    GvtAcseting.DataSource = dtNew;
                    GvtAcseting.DataBind();
                }
            }
            dtCMtng.Clear();
            dtPMtng.Clear();
        }
    }

    public void LoadMeetingsNew(string date)
    {
        string startDate = "";
        string endDate = "";
        if (date != null)
        {
            startDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
        DataTable dtAgItm = new DataTable();
        dtAgItm.Columns.Add(new DataColumn("AgendaItemId", typeof(Int32)));
        dtAgItm.Columns.Add(new DataColumn("AgendaItem", typeof(string)));
        dtAgItm.Columns.Add(new DataColumn("StaffInitials", typeof(string)));
        dtAgItm.Columns.Add(new DataColumn("DateAdded", typeof(string)));
        dtAgItm.Columns.Add(new DataColumn("DoneCarryOver", typeof(string)));
        DataTable dtMtngId = new DataTable();
        DataTable Dt3 = new DataTable();


        string agItmQry = "select MtngId from AcdSheetMtng where followstatus = 'A' and ActiveInd='A' and AccSheetId in " +
                          "(select AccSheetId from StdtAcdSheet where studentid=" + sess.StudentId + " and EndDate = '" + endDate + "')";
        DataTable dtAgendaId = objData.ReturnDataTable(agItmQry, false);
        int agendaId = 0;
        if (dtAgendaId != null)
        {
            foreach (DataRow row1 in dtAgendaId.Rows)
            {
                agendaId = Convert.ToInt32(row1["MtngId"]);
                string Aquery = "select MtngId as AgendaItemId, AgendaItem, StaffInitials, AgendaAddedDate as DateAdded, DoneCarryOver from AcdSheetMtng where MtngId =" + agendaId;
                Dt3 = objData.ReturnDataTable(Aquery, false);
                foreach (DataRow row2 in Dt3.Rows)
                    if (row2["AgendaItem"].ToString() != "" || row2["StaffInitials"].ToString() != "")
                    {
                        if (row2["DateAdded"].ToString() != "")
                        {
                            string dateAdded = row2["DateAdded"].ToString().Replace("-", "/");
                            dtAgItm.Rows.Add(Convert.ToInt32(row2["AgendaItemId"]), row2["AgendaItem"].ToString(), row2["StaffInitials"].ToString(), dateAdded, row2["DoneCarryOver"].ToString());
                        }
                        else
                            dtAgItm.Rows.Add(Convert.ToInt32(row2["AgendaItemId"]), row2["AgendaItem"].ToString(), row2["StaffInitials"].ToString(), row2["DoneCarryOver"].ToString());
                    }
            }
            if (dtAgItm != null)
            {
                if (dtAgItm.Rows.Count > 0)
                {
                    gvAgendaItem.DataSource = dtAgItm;
                    gvAgendaItem.DataBind();
                    int i = 0;
                    foreach (GridViewRow row3 in gvAgendaItem.Rows)
                    {
                        RadioButtonList radBtnLst1 = row3.FindControl("RBLDoneCarry") as RadioButtonList;
                        if (dtAgItm.Rows[i]["DoneCarryOver"].ToString() != "" && i < dtAgItm.Rows.Count)
                        {
                            radBtnLst1.SelectedValue = dtAgItm.Rows[i++]["DoneCarryOver"].ToString();
                        }
                    }
                }
            }
        }
        foreach (GridViewRow row in GridViewAccSheet.Rows)
        {
            HiddenField hdFldAcdId = (HiddenField)row.FindControl("hdFldAcdId");
            HiddenField hfLPId = (HiddenField)row.FindControl("hfLPId");

            //string Pquery = "select MtngId as PMtngId,PropAndDisc as PFollowUp,PersonResp as PPersonResponsible,CONVERT(VARCHAR,Deadlines,101) as PDeadlines from AcdSheetMtng where AccSheetId in (select AccSheetId from StdtAcdSheet where studentid=" + sess.StudentId + " and EndDate < '" + endDate + "' and LessonPlanId=" + hfLPId.Value + ")"; //TEST: EndDate<= changed to <
            string Pquery = "select MtngId as PMtngId,PropAndDisc as PFollowUp,u.UserLName+', '+u.UserFName as PPersonResponsible,CONVERT(VARCHAR,Deadlines,101) as PDeadlines from AcdSheetMtng join [User] u on u.UserId=PersonResp and AccSheetId in (select AccSheetId from StdtAcdSheet where studentid=" + sess.StudentId + " and EndDate < '" + endDate + "' and LessonPlanId=" + hfLPId.Value + ")";
            DataTable dtPMtng = objData.ReturnDataTable(Pquery, false);

            if (dtPMtng != null)
            {
                if (dtPMtng.Rows.Count > 0)
                {
                    GridView gvPMtng = (GridView)row.FindControl("gvPMeeting");
                    gvPMtng.DataSource = dtPMtng;
                    gvPMtng.DataBind();
                }
            }
            string Phaseline = "SELECT 'Major Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + startDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                " (StdtSessEventType='Major') AND StudentId=" + sess.StudentId + " and LessonPlanId in(" + hfLPId.Value + ",0) AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
            System.Data.DataTable dtNew1 = objData.ReturnDataTable(Phaseline, false);
            string Conditionline = "SELECT 'Minor Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + startDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                                " (StdtSessEventType='Minor') AND StudentId=" + sess.StudentId + "and LessonPlanId in(" + hfLPId.Value + ",0)  AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
            System.Data.DataTable dtNew2 = objData.ReturnDataTable(Conditionline, false);
            string Arrownotes = "SELECT 'Arrow notes' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + startDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                                " (StdtSessEventType='Arrow notes') AND StudentId=" + sess.StudentId + " and LessonPlanId in(" + hfLPId.Value + ",0)  AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
            System.Data.DataTable dtNew3 = objData.ReturnDataTable(Arrownotes, false);
            System.Data.DataTable dtNew = new System.Data.DataTable();
            dtNew.Columns.Add("Eventname", typeof(string));
            dtNew.Columns.Add("Eventdata", typeof(string));
            System.Data.DataRow dr1 = dtNew.NewRow();
            dr1["Eventname"] = dtNew1.Rows[0]["Eventname"];
            string Result = "";
            Result = dtNew1.Rows[0]["Eventdata"].ToString();
            if (Result == "")
            {
                dtNew1.Rows[0]["Eventdata"] = "No Results";
            }
            dr1["Eventdata"] = dtNew1.Rows[0]["Eventdata"];
            dtNew.Rows.Add(dr1);
            System.Data.DataRow dr2 = dtNew.NewRow();
            dr2["Eventname"] = dtNew2.Rows[0]["Eventname"];
            Result = dtNew2.Rows[0]["Eventdata"].ToString();
            if (Result == "")
            {
                dtNew2.Rows[0]["Eventdata"] = "No Results";
            }
            dr2["Eventdata"] = dtNew2.Rows[0]["Eventdata"];
            dtNew.Rows.Add(dr2);
            System.Data.DataRow dr3 = dtNew.NewRow();
            dr3["Eventname"] = dtNew3.Rows[0]["Eventname"];
            Result = dtNew3.Rows[0]["Eventdata"].ToString();
            if (Result == "")
            {
                dtNew3.Rows[0]["Eventdata"] = "No Results";
            }
            dr3["Eventdata"] = dtNew3.Rows[0]["Eventdata"];
            dtNew.Rows.Add(dr3);

            if (dtNew != null)
            {
                if (dtNew.Rows.Count > 0)
                {
                    GridView GvtAcgseting = (GridView)row.FindControl("grdGraphDataGen");
                    GvtAcgseting.DataSource = dtNew;
                    GvtAcgseting.DataBind();
                }
            }

        }
    }

    public void addRowCurMeeting(GridView gvC)
    {
        dtCMeeting.Columns.Add("CFollowUpEdit", typeof(string));
        dtCMeeting.Columns.Add("CMtngIdEdit", typeof(string));
        dtCMeeting.Columns.Add("PersonResponsibleEdit", typeof(string));
        dtCMeeting.Columns.Add("CPersonResponsibleEdit", typeof(string));
        dtCMeeting.Columns.Add("CDeadlinesEdit", typeof(string));

        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtCMeeting.NewRow();

            drTemp["CFollowUpEdit"] = ((TextBox)gvr.FindControl("txtCFollowUpEdit")).Text;
            drTemp["CMtngIdEdit"] = ((Label)gvr.FindControl("lblCMtngIdEdit")).Text;
            drTemp["PersonResponsibleEdit"] = ((TextBox)gvr.FindControl("txtPersonResponsibleEdit")).Text;
            drTemp["CPersonResponsibleEdit"] = ((TextBox)gvr.FindControl("txtCPersonResponsibleEdit")).Text;
            drTemp["CDeadlinesEdit"] = ((TextBox)gvr.FindControl("txtCDeadlinesEdit")).Text;
            dtCMeeting.Rows.Add(drTemp);
        }
        DataRow dr = dtCMeeting.NewRow();
        dr["CFollowUpEdit"] = "";
        dr["CMtngIdEdit"] = "";
        dr["PersonResponsibleEdit"] = "";
        dr["CPersonResponsibleEdit"] = "";
        dr["CDeadlinesEdit"] = "";
        dtCMeeting.Rows.Add(dr);
        gvC.DataSource = dtCMeeting;
        gvC.DataBind();
    }

    public void addRowPMeetingNew(GridView gvC)
    {
        dtPMeeting.Columns.Add("PFollowUpEdit", typeof(string));
        dtPMeeting.Columns.Add("PMtngIdEdit", typeof(string));
        dtPMeeting.Columns.Add("PPersonResponsibleEdit", typeof(string));
        //dtCMeeting.Columns.Add("CPersonResponsible", typeof(string));
        dtPMeeting.Columns.Add("PDeadlinesEdit", typeof(string));

        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtPMeeting.NewRow();

            drTemp["PFollowUpEdit"] = ((TextBox)gvr.FindControl("txtPFollowUpEdit")).Text;
            drTemp["PMtngIdEdit"] = ((Label)gvr.FindControl("lblPMtngIdEdit")).Text;
            drTemp["PPersonResponsibleEdit"] = ((TextBox)gvr.FindControl("txtPPersonResponsibleEdit")).Text;
            //drTemp["CPersonResponsible"] = ((TextBox)gvr.FindControl("txtCPersonResponsible")).Text;
            drTemp["PDeadlinesEdit"] = ((TextBox)gvr.FindControl("txtPDeadlinesEdit")).Text;
            dtPMeeting.Rows.Add(drTemp);
        }
        DataRow dr = dtPMeeting.NewRow();
        dr["PFollowUpEdit"] = "";
        dr["PMtngIdEdit"] = "";
        dr["PPersonResponsibleEdit"] = "";
        //dr["CPersonResponsible"] = "";
        dr["PDeadlinesEdit"] = "";
        dtPMeeting.Rows.Add(dr);
        gvC.DataSource = dtPMeeting;
        gvC.DataBind();
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "somekey", "destroy();", true);
    }

    public void addRowCurMeetingNew(GridView gvC)
    {
        dtCMeeting.Columns.Add("CFollowUp", typeof(string));
        dtCMeeting.Columns.Add("CMtngId", typeof(string));
        dtCMeeting.Columns.Add("PersonResponsible", typeof(string));
        dtCMeeting.Columns.Add("CPersonResponsible", typeof(string));
        dtCMeeting.Columns.Add("CDeadlines", typeof(string));

        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtCMeeting.NewRow();

            drTemp["CFollowUp"] = ((TextBox)gvr.FindControl("txtCFollowUp")).Text;
            drTemp["CMtngId"] = ((Label)gvr.FindControl("lblCMtngId")).Text;
            drTemp["PersonResponsible"] = ((TextBox)gvr.FindControl("txtPersonResponsible")).Text;
            drTemp["CPersonResponsible"] = ((TextBox)gvr.FindControl("txtCPersonResponsible")).Text;
            drTemp["CDeadlines"] = ((TextBox)gvr.FindControl("txtCDeadlines")).Text;
            dtCMeeting.Rows.Add(drTemp);
        }
        DataRow dr = dtCMeeting.NewRow();
        dr["CFollowUp"] = "";
        dr["CMtngId"] = "";
        dr["PersonResponsible"] = "";
        dr["CPersonResponsible"] = "";
        dr["CDeadlines"] = "";
        dtCMeeting.Rows.Add(dr);
        gvC.DataSource = dtCMeeting;
        gvC.DataBind();
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "somekey", "destroy();", true);
    }

    protected void delRowCurMeeting(int id)
    {
        objData = new clsData();
        string sQuery = "delete from AcdSheetMtng where MtngId=" + id;
        objData.Execute(sQuery);
    }
    protected void ArrangeGVPEdit(GridView gvC, int rowIndex)
    {
        dtPMeeting.Columns.Add("PFollowUpEdit", typeof(string));
        dtPMeeting.Columns.Add("PMtngIdEdit", typeof(string));
        dtPMeeting.Columns.Add("PPersonResponsibleEdit", typeof(string));
        //dtCMeeting.Columns.Add("CPersonResponsibleEdit", typeof(string));
        dtPMeeting.Columns.Add("PDeadlinesEdit", typeof(string));

        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtPMeeting.NewRow();

            drTemp["PFollowUpEdit"] = ((TextBox)gvr.FindControl("txtPFollowUpEdit")).Text;
            drTemp["PMtngIdEdit"] = ((Label)gvr.FindControl("lblPMtngIdEdit")).Text;
            drTemp["PPersonResponsibleEdit"] = ((TextBox)gvr.FindControl("txtPPersonResponsibleEdit")).Text;
            //drTemp["CPersonResponsibleEdit"] = ((TextBox)gvr.FindControl("txtCPersonResponsibleEdit")).Text;
            drTemp["PDeadlinesEdit"] = ((TextBox)gvr.FindControl("txtPDeadlinesEdit")).Text;
            dtPMeeting.Rows.Add(drTemp);
        }
        if (dtPMeeting.Rows.Count > 0)
        {
            dtPMeeting.Rows.RemoveAt(rowIndex);
        }
        if (dtPMeeting.Rows.Count == 0)
        {
            DataRow dr = dtPMeeting.NewRow();
            dr["PFollowUpEdit"] = "";
            dr["PMtngIdEdit"] = "";
            dr["PPersonResponsibleEdit"] = "";
            //dr["CPersonResponsibleEdit"] = "";
            dr["PDeadlinesEdit"] = "";
            dtPMeeting.Rows.Add(dr);
        }
        gvC.DataSource = dtPMeeting;
        gvC.DataBind();
    }

    protected void ArrangeGVEdit(GridView gvC, int rowIndex)
    {
        dtCMeeting.Columns.Add("CFollowUpEdit", typeof(string));
        dtCMeeting.Columns.Add("CMtngIdEdit", typeof(string));
        dtCMeeting.Columns.Add("PersonResponsibleEdit", typeof(string));
        dtCMeeting.Columns.Add("CPersonResponsibleEdit", typeof(string));
        dtCMeeting.Columns.Add("CDeadlinesEdit", typeof(string));

        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtCMeeting.NewRow();

            drTemp["CFollowUpEdit"] = ((TextBox)gvr.FindControl("txtCFollowUpEdit")).Text;
            drTemp["CMtngIdEdit"] = ((Label)gvr.FindControl("lblCMtngIdEdit")).Text;
            drTemp["PersonResponsibleEdit"] = ((TextBox)gvr.FindControl("txtPersonResponsibleEdit")).Text;
            drTemp["CPersonResponsibleEdit"] = ((TextBox)gvr.FindControl("txtCPersonResponsibleEdit")).Text;
            drTemp["CDeadlinesEdit"] = ((TextBox)gvr.FindControl("txtCDeadlinesEdit")).Text;
            dtCMeeting.Rows.Add(drTemp);
        }
        if (dtCMeeting.Rows.Count > 0)
        {
            dtCMeeting.Rows.RemoveAt(rowIndex);
        }
        if (dtCMeeting.Rows.Count == 0)
        {
            DataRow dr = dtCMeeting.NewRow();
            dr["CFollowUpEdit"] = "";
            dr["CMtngIdEdit"] = "";
            dr["PersonResponsibleEdit"] = "";
            dr["CPersonResponsibleEdit"] = "";
            dr["CDeadlinesEdit"] = "";
            dtCMeeting.Rows.Add(dr);
        }
        gvC.DataSource = dtCMeeting;
        gvC.DataBind();
    }

    protected void ArrangeGV(GridView gvC, int rowIndex)
    {
        dtCMeeting.Columns.Add("CFollowUp", typeof(string));
        dtCMeeting.Columns.Add("CMtngId", typeof(string));
        dtCMeeting.Columns.Add("PersonResponsible", typeof(string));
        dtCMeeting.Columns.Add("CPersonResponsible", typeof(string));
        dtCMeeting.Columns.Add("CDeadlines", typeof(string));

        foreach (GridViewRow gvr in gvC.Rows)
        {
            DataRow drTemp = dtCMeeting.NewRow();

            drTemp["CFollowUp"] = ((TextBox)gvr.FindControl("txtCFollowUp")).Text;
            drTemp["CMtngId"] = ((Label)gvr.FindControl("lblCMtngId")).Text;
            drTemp["PersonResponsible"] = ((TextBox)gvr.FindControl("txtPersonresponsible")).Text;
            drTemp["CPersonResponsible"] = ((TextBox)gvr.FindControl("txtCPersonResponsible")).Text;
            drTemp["CDeadlines"] = ((TextBox)gvr.FindControl("txtCDeadlines")).Text;
            dtCMeeting.Rows.Add(drTemp);
        }
        if (dtCMeeting.Rows.Count > 0)
        {
            dtCMeeting.Rows.RemoveAt(rowIndex);
        }
        if (dtCMeeting.Rows.Count == 0)
        {
            DataRow dr = dtCMeeting.NewRow();
            dr["CFollowUp"] = "";
            dr["CMtngId"] = "";
            dr["PersonResponsible"] = "";
            dr["CPersonResponsible"] = "";
            dr["CDeadlines"] = "";
            dtCMeeting.Rows.Add(dr);
        }
        gvC.DataSource = dtCMeeting;
        gvC.DataBind();
    }

    protected void gvPMeeting_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gvCMeeting_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddRow")
        {
            GridView gvC = (GridView)sender;
            if (gvC.Rows.Count == 5) return;
            addRowCurMeetingNew(gvC);
        }
        if (e.CommandName == "delete")
        {
            if (e.CommandArgument != "")
            {
                int rowId = 0;
                string cArg = e.CommandArgument.ToString();
                if (cArg != "")
                {
                    rowId = int.Parse(e.CommandArgument.ToString());
                    delRowCurMeeting(rowId);
                }
            }
        }
    }

    protected void gvPMeetingEdit_RowCommand(object sender, GridViewCommandEventArgs e)
    {                   
            if (e.CommandName == "AddRow")
            {
                GridView gvC = (GridView)sender;
                if (gvC.Rows.Count == 5) return;
                addRowPMeetingNew(gvC);
            }
            if (e.CommandName == "delete")
            {
                int rowId = 0;
                string cArg = e.CommandArgument.ToString();
                if (cArg != "")
                {
                    rowId = int.Parse(e.CommandArgument.ToString());
                    delRowCurMeeting(rowId);
                }
            }
        
    }

    protected void gvCMeetingEdit_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddRow")
        {
            GridView gvC = (GridView)sender;
            if (gvC.Rows.Count == 5) return;
            addRowCurMeeting(gvC);
        }
        if (e.CommandName == "delete")
        {
            int rowId = 0;
            string cArg = e.CommandArgument.ToString();
            if (cArg != "")
            {
                rowId = int.Parse(e.CommandArgument.ToString());
                delRowCurMeeting(rowId);
            }
        }
    }
    protected void gvPMeetingEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView gvC = (GridView)sender;
        int rowIndex = e.RowIndex;
        ArrangeGVPEdit(gvC, rowIndex);
    }
    protected void gvCMeetingEdit_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView gvC = (GridView)sender;
        int rowIndex = e.RowIndex;
        ArrangeGVEdit(gvC, rowIndex);
    }
    protected void gvCMeeting_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView gvC = (GridView)sender;
        int rowIndex = e.RowIndex;
        ArrangeGV(gvC, rowIndex);
    }
    protected void btnGenDelete_Click(object sender, EventArgs e)
    {
        string strquery = "";
        objData = new clsData();
        string date = ViewState["CurrentDate"].ToString();
        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
                strquery = "delete from AcdSheetMtng where AccSheetId in(select AccSheetId from  StdtAcdSheet where StdtAcdSheet.StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)='" + endDate + "')";
                objData.Execute(strquery);
                strquery = "delete from StdtAcdSheet where StdtAcdSheet.StudentId=" + sess.StudentId + " and CONVERT(char(10),DateOfMeeting,101)='" + stDate + "' AND CONVERT(char(10),EndDate,101)='" + endDate +"'";
                objData.Execute(strquery);       

                GridViewAccSheetedit.DataSource = null;
                GridViewAccSheetedit.DataBind();
                btnImport.Visible = false;
                btnDelete.Visible = false;
                rbtnLsnClassTypeAc.Visible = false;
                tdMsg1.Visible = false;
                tdMsg2.Visible = false;
                tdreview2.Visible = false;
                gvAgendaItem.Visible = false;
                btnUpdate.Visible = false;
                btnUpdateNew.Visible = false;
                FillData(); 
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Deleted Successfully...");           
    }
    protected void LessonTypeSelect(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        objData = new clsData();
        if (btnSaveNew.Visible == true)
        {            
            LoadPMeetingGVNew();
            LoadCMeetingGVNew();
            loadDataListFilter();
            string dateOfMtng = ViewState["CurrentDate"].ToString();
            LoadMeetingsNew(dateOfMtng);
            loadExtraData();                      

        }
        else
        {
            Loadlesson();
        }
        rbtnLsnClassTypeAc.Visible = true;
        tdMsg1.Visible = true;
        tdMsg2.Visible = true;
        tdreview2.Visible = true;
        gvAgendaItem.Visible = true;
    }
    protected void Loadlesson()
    {
        objData = new clsData();
        string dateVal = "";
        tdMsg.InnerHtml = "";
        try
        {
            dateVal = ViewState["CurrentDate"].ToString();
            LoadPMeetingGV();
            LoadCMeetingGV();
            loadDataList(dateVal);
            LoadMeetings(dateVal);
            MultiView1.ActiveViewIndex = 1;            
            btnUpdate.Visible = true;
            btnUpdateNew.Visible = true;
            setWritePermissions();

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void Loadlesson2()
    {
        objData = new clsData();
        string dateVal = "";
        try
        {
            dateVal = ViewState["CurrentDate"].ToString();
            LoadPMeetingGV();
            LoadCMeetingGV();
            loadDataList(dateVal);
            LoadMeetings(dateVal);
            MultiView1.ActiveViewIndex = 1;
            btnUpdate.Visible = true;
            btnUpdateNew.Visible = true;
            setWritePermissions();

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    private void loadDataListFilter()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();

        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

        DateTime dateNow1 = dtst;
        DateTime dateNow2 = dateNow1.AddDays(+14);
        DateTime dateNow3 = dateNow2.AddDays(+14);
        DateTime dateNow4 = dateNow3.AddDays(+14);
        DateTime dateNow5 = dateNow4.AddDays(+14);
        DateTime dateNow6 = dateNow5.AddDays(+14);
        DateTime dateNow7 = dateNow6.AddDays(+14);
        DateTime dateNow8 = dateNow7.AddDays(+14);
        string filterclass = "";
        if(rbtnLsnClassTypeAc.SelectedItem.Text == "Day")
        {
            filterclass = "HDR.LessonPlanId IN(select s.LessonPlanId from StdtSessionHdr s join Class c on c.ClassId=s.StdtClassId where c.ResidenceInd=0) AND";
        }
        if(rbtnLsnClassTypeAc.SelectedItem.Text == "Residence")
        {
            filterclass = "HDR.LessonPlanId IN(select s.LessonPlanId from StdtSessionHdr s join Class c on c.ClassId=s.StdtClassId where c.ResidenceInd=1) AND";
        }

        

        string qry = "SELECT DISTINCT HDR.DSTempHdrId, HDR.DSTemplateName AS LessonPlanName, HDR.LessonPlanId, HDR.VerNbr, HDR.LessonOrder," +
                    "(STUFF((  SELECT ', ' + G.GoalName FROM Goal G INNER JOIN GoalLPRel GLP ON GLP.GoalId=G.GoalId WHERE GLP.LessonPlanId=HDR.LessonPlanId FOR XML PATH('')), 1, 2, '')) AS GoalName," +

                "(SELECT TOP 1 Objective3 FROM StdtLessonPlan WHERE GoalId=GLP.GoalId AND LessonPlanId=GLP.LessonPlanId AND StudentId='" + sess.StudentId + "' ORDER BY StdtIEPId DESC) AS Objective3, " +
                "(select LookupCode from LookUp where  LookupId= (SELECT TeachingProcId FROM DSTempHdr WHERE DSTempHdr.DSTempHdrId= HDR.DSTempHdrId ))+'('+(select LookupName from LookUp where  LookupId=(SELECT PromptTypeId FROM DSTempHdr WHERE DSTempHdr.DSTempHdrId=HDR.DSTempHdrId ) )+')' as 'TypeOfInstruction', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn) between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer1' , " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId AND CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel1', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set1', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer2', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel2', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set2', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer3', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel3', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set3', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer4', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'ProptLevel4', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc ) as 'set4', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer5', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel5', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set5', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer6', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel6', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set6', " +
                "(SELECT TOP 1 IOAPerc from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND IOAPerc IS NOT NULL order by CreatedOn desc) as 'IOAPer7', " +
                "(SELECT TOP 1 (select LookupName from LookUp where LookupId=CurrentPromptId) as 'PromptLevel' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'ProptLevel7', " +
                "(SELECT TOP 1 (select SetCd from DSTempSet where DSTempSetId=CurrentSetId) as 'SetName' from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'set7', " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow1.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM1, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow1.ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow2.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS1, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM2, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow3.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS2, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS3, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS4, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS5, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS6, " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step1', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step2', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step3', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step4', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step5', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step6', " +
                "(SELECT TOP 1 (select TOP 1 CASE WHEN StepCd IS NULL OR StepCd='' THEN StepName ELSE StepCd END from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId=HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Step7', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) FROM  StdtSessionHdr WHERE StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow1.ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow2.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid1', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow2.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow3.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid2', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow4.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid3', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow5.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid4', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow6.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid5', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow7.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid6', " +
                "(SELECT TOP 1 (select TOP 1 DSTempStepId from DSTempStep where DSTempSetId=CurrentSetId AND SortOrder=CurrentStepId) from  StdtSessionHdr where StdtSessionHdr.DSTempHdrId= HDR.DSTempHdrId and CONVERT(date, CreatedOn)  between CONVERT(date, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') and CONVERT(date, '" + dateNow8.ToString("MM/dd/yyyy") + "') order by CreatedOn desc) as 'Stepid7', " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE LessonPlanId=HDR.LessonPlanId AND CONVERT(DATE, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND SchoolId='" + sess.SchoolId + "' AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='Y'))) MIS7, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow3.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow4.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM3, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow4.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow5.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM4, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow5.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow6.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM5, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow6.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow7.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM6, " +
                "(SELECT CONVERT(VARCHAR(50),(SELECT COUNT(*) FROM StdtSessionHdr WHERE CONVERT(DATE, '" + dateNow7.AddDays(+1).ToString("MM/dd/yyyy") + "') <= CONVERT(DATE,StartTs) AND CONVERT(DATE,StartTs) <= CONVERT(DATE, '" + dateNow8.ToString("MM/dd/yyyy") + "') AND StudentId='" + sess.StudentId + "' AND SchoolId='" + sess.SchoolId + "' AND LessonPlanId=HDR.LessonPlanId AND SessionStatusCd='S' AND IOAInd='N' AND SessMissTrailStus='N'))  +'/'+CONVERT(VARCHAR(50),(SELECT Top 1 NoofTimesTried FROM DSTempHdr WHERE DSTempHdr.LessonPlanId=HDR.LessonPlanId AND StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName In ('Approved','Maintenance')) AND StudentId='" + sess.StudentId + "' ))) AS NUM7 " +
                "FROM DSTempHdr HDR INNER JOIN GoalLPRel GLP ON HDR.LessonPlanId= GLP.LessonPlanId INNER JOIN Goal G ON GLP.GoalId = G.GoalId WHERE HDR.StudentId='" + sess.StudentId + "' AND " + filterclass + " HDR.StatusId in (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName in('Approved','Maintenance')) ";

        
        string strQuery = qry;
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                GridViewAccSheet.DataSource = Dt;
                GridViewAccSheet.DataBind();
                btnUpdate.Visible = false;
                btnUpdateNew.Visible = false;
                btnImport.Visible = false;
                btnDelete.Visible = false;
                btnSave.Visible = true;
                btnSaveNew.Visible = true;
                rbtnLsnClassTypeAc.Visible = true;
                tdMsg1.Visible = true;
                tdMsg2.Visible = true;
                tdreview2.Visible = true;
                gvAgendaItem.Visible = true;

            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
                GridViewAccSheet.DataSource = null;
                GridViewAccSheet.DataBind();
                btnUpdate.Visible = false;
                btnUpdateNew.Visible = false;
                btnImport.Visible = false;
                btnDelete.Visible = false;
                btnSave.Visible = true;
                btnSaveNew.Visible = true;
                rbtnLsnClassTypeAc.Visible = true;
                tdMsg1.Visible = false;
                tdMsg2.Visible = false;
                tdreview2.Visible = false;
                gvAgendaItem.Visible = false;
            }
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found !");
            GridViewAccSheet.DataSource = null;
            GridViewAccSheet.DataBind();
            btnUpdate.Visible = false;
            btnUpdateNew.Visible = false;
            btnImport.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = true;
            btnSaveNew.Visible = true;
            rbtnLsnClassTypeAc.Visible = true;
            tdMsg1.Visible = false;
            tdMsg2.Visible = false;
            tdreview2.Visible = false;
            gvAgendaItem.Visible = false;
        }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {

    }

   

    //protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (CheckBoxList1.SelectedValue == "Met Objective" || CheckBoxList1.SelectedValue == "Met Goal")
    //    {
    //        //CheckBoxList1.SelectedItem.Attributes["style"] = "color:green";
    //         CheckBoxList1.SelectedItem.Attributes.Add("style", "color: green; font-weight: bold");
    //    }
       
    //    if (CheckBoxList1.SelectedValue == "Not Maintaining")
    //    {
    //        CheckBoxList1.SelectedItem.Attributes.Add("style", "color: red; font-weight: bold");
    //    }
    //    //LoadPMeetingGVNew();
    //    //LoadCMeetingGVNew();
    //    //loadDataListFilter();
    //    //string dateOfMtng = ViewState["CurrentDate"].ToString();
    //    //LoadMeetingsNew(dateOfMtng);
    //    //loadExtraData();     

    //}
}