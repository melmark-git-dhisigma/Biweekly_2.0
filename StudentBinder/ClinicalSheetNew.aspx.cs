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
using Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Globalization;
using System.Net;
using System.Web.Services;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class StudentBinder_ClinicalSheetNew : System.Web.UI.Page
{
    System.Data.DataTable dtRecChang = new System.Data.DataTable();
    System.Data.DataTable dtAssmtReinfo = new System.Data.DataTable();
    System.Data.DataTable dtAssmntTool = new System.Data.DataTable();
    clsData objData = null;
    System.Data.DataTable dt = null;
    System.Data.DataTable Dt = null;
   clsSession sess = null;
    clsSession Prevsess = null;
    string curesesid = "";
    int preid = 0;
    string preuser = "";
    string Pagepath = "";
    int SessStudentid = 0;
    string Sessstname = "";

    static string[] columns;
    static string[] placeHolders;

    static string[] tblAppnd;
    //static string[] placeHoldersCheck;

    static string[] columnsToAdd;
    //static string[] columnsP4;
    //static string[] placeHoldersP4;

    //static int checkCount = 0;
    //static int P4TotalCount = 1;



    //int StudentId = 2;
    //int SchoolId = 1;
    //int Classid = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        tdMsgDel.InnerHtml = "";
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
                btnExport.Visible = false;
                btnGenDelete.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnExport.Visible = true;
                btnGenDelete.Visible = true;
            }


            PanelMain.Visible = false;
            // loadDrpDates();

            LoadDataList();
            // Fill graph data
            btnExport.Visible = false;
            //tdMsg.InnerHtml = "<span class='tdtext' style='color: #0D668E; font-family:times new roman;margin-left:20px;font-size:18px;'>Please Select a date to load the data</span>";
            btnGenDelete.Visible = false;

            if (sess.SchoolId == 1)
            {
                hdTxtBx1.Visible = false;
                hdTxtBx2.Visible = false;

                hdTxtBx3.Visible = false;
                hdTxtBx4.Visible = false;

                hdTxtBx5.Visible = false;
                hdTxtBx6.Visible = false;

                hdTxtBx7.Visible = false;
                hdTxtBx8.Visible = false;

            }

            //select distinct CONVERT(VARCHAR(10),PgmCordDate,101) as id,CONVERT(VARCHAR(10),PgmCordDate,101) from  StdtClinicalCoverSheet
        }
        Prevsess = (clsSession)Session["PreSession"];
        preid = Convert.ToInt16(Session["Spreid"]);
        preuser = Session["Spreuser"].ToString();
        SessStudentid = Convert.ToInt16(Session["Sprestid"]);
        Sessstname = Session["Sprestname"].ToString();
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: PageLoad";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);

    }

    //public void loadDrpDates()
    //{//select CONVERT(varchar(10),EndDate,101) as EndDate from StdtClinicalCoverSheet order by EndDate desc
    //    dt = new System.Data.DataTable();
    //    objData = new clsData();
    //    dt = objData.ReturnDataTable("select CONVERT(varchar(10),EndDate,101) as EndDate from StdtClinicalCoverSheet order by EndDate desc", false);
    //    drpSelectDate.Items.Clear();
    //    drpSelectDate.Items.Add(DateTime.Now.ToString("MM'/'dd'/'yyyy"));
    //    foreach (DataRow dr in dt.Rows)
    //    {

    //        if (dr[0].ToString() != DateTime.Now.ToString("MM'/'dd'/'yyyy"))
    //        {
    //            drpSelectDate.Items.Add(dr[0].ToString());
    //        }

    //    }

    //}



    private void setWritePermissions()
    {
        bool Disable = false;
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: setWritePermissions";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            btnSave.Visible = false;
            btnExport.Visible = false;
            btnGenDelete.Visible = false;
        }
        else
        {
            btnSave.Visible = true;
            btnExport.Visible = true;
            btnGenDelete.Visible = true;
        }
    }


    protected void FillReportCurrent()
    {
        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        string currentData = dtst.ToString("MM'/'dd'/'yyyy") + "-" + dted.ToString("MM'/'dd'/'yyyy");
        ViewState["CurrentDate"] = currentData;
        LoadReport(currentData);
    }

    protected void FillGraphData()
    {
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: FillGraphData";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);

        DateTime dtst = new DateTime();
        DateTime dted = new DateTime();
        grdGraphData.DataSource = null;
        grdGraphData.DataBind();
        dtst = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        dted = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        objData = new clsData();

         System.Data.DataTable dtNew = new System.Data.DataTable();
        String Mesrid = "select MeasurementId from BehaviourDetails b where b.StudentId= " + sess.StudentId + " and activeind = 'A' ";
        System.Data.DataTable dtNew0 = objData.ReturnDataTable(Mesrid, false);
        System.Data.DataRow dr0 = dtNew.NewRow();
        System.Data.DataTable dtNew05;
        System.Data.DataTable dtNew1;
        System.Data.DataTable dtNew2;

         String Behv = "",  concatStrngMj="" ,concatStrngMn="";
         foreach (DataRow roww in dtNew0.Rows)
         {
             if (roww != null)
             {
                 String Behavi = "SELECT Behaviour  from BehaviourDetails where MeasurementId =" + roww["MeasurementId"];
                 dtNew05 = objData.ReturnDataTable(Behavi, false);
                 Behv = Convert.ToString(dtNew05.Rows[0]["Behaviour"]);

        //string Phaseline = "SELECT 'Phase lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst.ToString("MM/dd/yyyy") + "') AND CONVERT(DATE,'" + dted.ToString("MM/dd/yyyy") + "') AND " +
         //                   " (StdtSessEventType='Major') AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
        string Phaseline = "SELECT 'Major Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst.ToString("MM/dd/yyyy") + "') AND CONVERT(DATE,'" + dted.ToString("MM/dd/yyyy") + "') AND " +
                                                         " (StdtSessEventType='Major' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + roww["MeasurementId"] + ")) AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
                 dtNew1 = objData.ReturnDataTable(Phaseline, false);
                 if (Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != null)
                     concatStrngMj += "<b>" + Behv + " : " + "</b>" + Convert.ToString(dtNew1.Rows[0]["Eventdata"]) + "<br></br>";
            

        //string Conditionline = "SELECT 'Condition lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst.ToString("MM/dd/yyyy") + "') AND CONVERT(DATE,'" + dted.ToString("MM/dd/yyyy") + "') AND " +
         //                   " (StdtSessEventType='Minor') AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
        string Conditionline = "SELECT 'Minor Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst.ToString("MM/dd/yyyy") + "') AND CONVERT(DATE,'" + dted.ToString("MM/dd/yyyy") + "') AND " +
                            " (StdtSessEventType='Minor' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + roww["MeasurementId"] + ")) AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
         dtNew2 = objData.ReturnDataTable(Conditionline, false);
         if (Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != null)
             concatStrngMn += "<b>" + Behv + " : " + "</b>" + Convert.ToString(dtNew2.Rows[0]["Eventdata"]) + "<br></br>";
             }
         }
        string Arrownotes = "SELECT 'Arrow notes' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN CONVERT(DATE,'" + dtst.ToString("MM/dd/yyyy") + "') AND CONVERT(DATE,'" + dted.ToString("MM/dd/yyyy") + "') AND " +
                            " (StdtSessEventType='Arrow notes' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1) AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
        System.Data.DataTable dtNew3 = objData.ReturnDataTable(Arrownotes, false);
        
        dtNew.Columns.Add("Eventname", typeof(string));
        dtNew.Columns.Add("Eventdata", typeof(string));

        System.Data.DataRow dr1 = dtNew.NewRow();

        dr1["Eventname"] = "Major Condition Lines";
        if (concatStrngMj != "")
        {
            dr1["Eventdata"] = concatStrngMj;
        }
        else
        {
        dr1["Eventdata"] = "No Results.";
        }
        dtNew.Rows.Add(dr1);
        System.Data.DataRow dr2 = dtNew.NewRow();
        dr2["Eventname"] = "Minor Condition Lines";
        if (concatStrngMn == "")
        {
        dr2["Eventdata"] = "No Results.";
        }
        else
        {
            dr2["Eventdata"] = concatStrngMn;
        }

        dtNew.Rows.Add(dr2);
       

        String Mesrmntid = "select MeasurementId from BehaviourDetails b where b.StudentId= " + sess.StudentId +" and activeind = 'A' " ;
        System.Data.DataTable dtNew4 = objData.ReturnDataTable(Mesrmntid, false);
        System.Data.DataRow dr4 = dtNew.NewRow();
        System.Data.DataTable dtNew5 ;
        System.Data.DataTable dtNew6;

        String Beh = "", Evnt = "", concatStrng="";
        foreach (DataRow row in dtNew4.Rows)
        {
            if (row != null)
            {
                String Behav = "SELECT Behaviour  from BehaviourDetails where MeasurementId =" + row["MeasurementId"];
                dtNew5=objData.ReturnDataTable(Behav, false);
                Beh=Convert.ToString(dtNew5.Rows[0]["Behaviour"]);
                
                string queryfilter = " SELECT STUFF((select '; ' + CONVERT(VARCHAR(50), outr.EvntTs,101)+','+ outr.eventname from( SELECT * FROM (SELECT * FROM  ((SELECT  SE.MeasurementId, SE.StdtSessEventId,  SE.EventName, " +
              " SE.StdtSessEventType, CONVERT(CHAR(10), SE.EvntTs,101) AS EvntTs, " +
              "  B.Behaviour FROM  [StdtSessEvent] SE LEFT JOIN LessonPlan L ON SE.LessonPlanId = L.LessonPlanId " +
              "LEFT JOIN BehaviourDetails B ON B.MeasurementId=SE.MeasurementId WHERE EventType='EV' AND SE.StudentId=" + sess.StudentId + " AND SE.StdtSessEventType<>'Medication') " +
              "UNION ALL (SELECT NULL AS MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
              "+ ( " +
              "        (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
              "        FROM   [user] US" +
              "       WHERE  US.userid = (SELECT" +
              "             createdby" +
              "                         FROM" +
              "           stdtsessionhdr Hdr" +
              "                       WHERE" +
              "         Hdr.stdtsessionhdrid = SH.ioasessionhdrid" +
              "        AND SH.ioaind = 'Y'))" +
              " + '/'" +
              " + (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
              "   FROM   [user] US" +
              "  WHERE  SH.ioauserid = US.userid) ) AS EventName," +
              " 'Arrow notes'                         AS" +
              " StdtSessEventType," +
              "CONVERT(CHAR(10), SH.endts, 101)      AS EvntTs," +
              "NULL                                  AS Behaviour" +
              " FROM   stdtsessionhdr SH" +
              "       LEFT JOIN lessonplan" +
              "             ON SH.lessonplanid = lessonplan.lessonplanid" +
              " WHERE  SH.ioaperc IS NOT NULL" +
              "      AND SH.ioaind = 'Y'" +
              "     AND SH.sessionstatuscd = 'S'" +
              "    AND SH.studentid =" + sess.StudentId + ")" +
              "UNION ALL (SELECT  BIOA.MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
              "+ CASE WHEN BIOA.normalbehaviorid IS NULL THEN ((SELECT" +
              "      TOP 1" +
              "     Rtrim(" +
              "    Ltrim(Upper(" +
              "   US.userinitial))) FROM behaviour BH INNER JOIN [user] US" +
              "  ON" +
              " BH.createdby" +
              " =" +
              " US.userid WHERE BH.createdon BETWEEN" +
                " Dateadd(minute, -5, BIOA.createdon)" +
                " AND" +
                " BIOA.createdon ORDER BY BH.createdon DESC)+'/'+ (SELECT" +
                " TOP 1" +
                " Rtrim(Ltrim(Upper(US.userinitial))) FROM" +
                " behaviorioadetails BI" +
                " INNER" +
                " JOIN [user]" +
                " US ON BI.createdby =" +
                " US.userid WHERE BI.createdon=BIOA.createdon ORDER BY" +
                " BI.createdon DESC)" +
                " )" +
                " ELSE ((" +
                " SELECT" +
                " Rtrim(Ltrim(Upper(US.userinitial))) FROM behaviour BH" +
                " INNER" +
                " JOIN [user]" +
                " US ON" +
                " BH.createdby = US.userid WHERE" +
                " BIOA.normalbehaviorid=BH.behaviourid)+'/'+ (" +
                " SELECT Rtrim(" +
                " Ltrim(Upper(US.userinitial))) FROM behaviorioadetails BI" +
                " INNER" +
                " JOIN" +
                " [user] US ON" +
                " BI.createdby = US.userid INNER JOIN behaviour BH ON" +
                " BH.behaviourid=BI.normalbehaviorid WHERE" +
                " BIOA.normalbehaviorid=BH.behaviourid))" +
                " END " +
              " AS EventName, " +
              "'Arrow notes' AS StdtSessEventType,  CONVERT(CHAR(10), BIOA.CreatedOn,101) AS EvntTs,  " +
              " BHD.Behaviour FROM BehaviorIOADetails BIOA LEFT JOIN BehaviourDetails BHD ON BIOA.MeasurementId=BHD.MeasurementId " +
              "WHERE BIOA.StudentId=" + sess.StudentId + " AND IOAPerc IS NOT NULL AND BIOA.ActiveInd='A') )IOA ) " +
              "  ad " +
              " WHERE  ( ( ad.behaviour IS  NULL " +
              " AND ad.measurementid = 0 )" +
              "OR ad.behaviour = (SELECT TOP 1 behaviour " +
              "FROM   behaviourdetails " +
              "WHERE  measurementid = " + row["MeasurementId"] + ") ) " +
              " AND ad.stdtsesseventtype IN( 'Arrow notes' ) " +
              " AND CONVERT(DATE, ad.evntts) >=  CONVERT(DATE,'" + dtst.ToString("MM/dd/yyyy") + "') " +
              " AND CONVERT(DATE, ad.evntts) <=  CONVERT(DATE,'" + dted.ToString("MM/dd/yyyy") + "') "+
              " )outr "+
               " FOR XML PATH('')),1,1,'') eventname ";
                dtNew6 = objData.ReturnDataTable(queryfilter, false);
                if (Convert.ToString(dtNew6.Rows[0]["eventname"]) != "" && Convert.ToString(dtNew6.Rows[0]["eventname"]) != null)
                concatStrng += "<b>" + Beh + " : " + "</b>" + Convert.ToString(dtNew6.Rows[0]["eventname"]) + "<br><br>";
            }

        }
        System.Data.DataRow dr5 = dtNew.NewRow();
        dr5["Eventname"] = "Arrow Notes";
        if (concatStrng != "")
        {
        dr5["Eventdata"] = concatStrng;
        }
        else
        {
            dr5["Eventdata"] = "No Results.";
        }
        dtNew.Rows.Add(dr5);
        //String Behavr = "select  '' as Eventname,bd.Behaviour as Eventdata from Behaviour b join BehaviourDetails bd" +
        //" on b.StudentId=bd.StudentId where b.StudentId=" + sess.StudentId;

        //System.Data.DataTable dtNew4 = objData.ReturnDataTable(Behavr, false);
        //System.Data.DataRow dr4 = dtNew.NewRow();
        //dr4["Eventname"] = "Arrow notes";
        //dr4["Eventdata"] = dtNew4.Rows[0]["Eventdata"];
        //dtNew.Rows.Add(dr4);
        //int i = 1;
        //foreach (DataRow row in dtNew4.Rows)
        //{
        //    if (row != null)
        //    {
        //        dr4 = dtNew.NewRow();
        //        dr4["Eventdata"] = dtNew4.Rows[i]["Eventdata"];
        //        dtNew.Rows.Add(dr4);

        //        i++;
        //    }
        //}

       // dtNew.Rows.Add(dr4);

        //System.Data.DataRow dr3 = dtNew.NewRow();
       // dr3["Eventname"] = dtNew3.Rows[0]["Eventname"];
      // dr3["Eventdata"] = dtNew3.Rows[0]["Eventdata"];
       // dtNew.Rows.Add(dr3);
        grdGraphData.DataSource = dtNew;
        grdGraphData.DataBind();

    }
    protected void FillGraphData(string date)
    {
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: FillGraphData()";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);

        grdGraphData.DataSource = null;
        grdGraphData.DataBind();
        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
        objData = new clsData();
        System.Data.DataTable dtNew = new System.Data.DataTable();
        String Mesrid = "select MeasurementId from BehaviourDetails b where b.StudentId= " + sess.StudentId + " and activeind = 'A' ";
        System.Data.DataTable dtNew0 = objData.ReturnDataTable(Mesrid, false);
        System.Data.DataRow dr0 = dtNew.NewRow();
        System.Data.DataTable dtNew05;
        System.Data.DataTable dtNew1;
        System.Data.DataTable dtNew2;

         String Behv = "",  concatStrngMj="" ,concatStrngMn="";
        foreach (DataRow roww in dtNew0.Rows)
        {
            if (roww != null)
            {
                 String Behavi = "SELECT Behaviour  from BehaviourDetails where MeasurementId =" + roww["MeasurementId"];
                dtNew05=objData.ReturnDataTable(Behavi, false);
                Behv=Convert.ToString(dtNew05.Rows[0]["Behaviour"]);
       // string Phaseline = "SELECT 'Phase lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + stDate + "' as date) AND cast('" + endDate + "' as date) AND " +
        string Phaseline = "SELECT 'Major Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + stDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                            " (StdtSessEventType='Major' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + roww["MeasurementId"] + ")) AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
         dtNew1 = objData.ReturnDataTable(Phaseline, false);
         if (Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew1.Rows[0]["Eventdata"]) != null)
             concatStrngMj += "<b>" + Behv + " : " + "</b>" + Convert.ToString(dtNew1.Rows[0]["Eventdata"]) + "<br></br>";
           
        //string Conditionline = "SELECT 'Condition lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + stDate + "' as date) AND cast('" + endDate + "' as date) AND " +
        string Conditionline = "SELECT 'Minor Condition Lines' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + stDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                            " (StdtSessEventType='Minor' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1 AND (MeasurementId=0 OR MeasurementId=" + roww["MeasurementId"] + ")) AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
        dtNew2 = objData.ReturnDataTable(Conditionline, false);
        if (Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != "" && Convert.ToString(dtNew2.Rows[0]["Eventdata"]) != null)
             concatStrngMn += "<b>" + Behv + " : " + "</b>" + Convert.ToString(dtNew2.Rows[0]["Eventdata"]) + "<br></br>";
            }

        }
        string Arrownotes = "SELECT 'Arrow notes' as Eventname,(SELECT STUFF((SELECT ', '+ EventName + ' (' + CONVERT(VARCHAR(50), EvntTs,101)+')' FROM StdtSessEvent WHERE CONVERT(DATE, EvntTs) BETWEEN cast('" + stDate + "' as date) AND cast('" + endDate + "' as date) AND " +
                            " (StdtSessEventType='Arrow notes' AND  MeasurementId IS NOT NULL AND MeasurementId !=-1) AND StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " FOR XML PATH('')),1,1,'')) Eventdata";
        System.Data.DataTable dtNew3 = objData.ReturnDataTable(Arrownotes, false);
        
        dtNew.Columns.Add("Eventname", typeof(string));
        dtNew.Columns.Add("Eventdata", typeof(string));

        System.Data.DataRow dr1 = dtNew.NewRow();
        dr1["Eventname"] = "Major Condition Lines";
        if (concatStrngMj != "")
        {
            dr1["Eventdata"] = concatStrngMj;
        }
        else
        {
        dr1["Eventdata"] = "No Results.";
        }
        dtNew.Rows.Add(dr1);
        System.Data.DataRow dr2 = dtNew.NewRow();
        dr2["Eventname"] = "Minor Condition Lines";
        if (concatStrngMn == "")
        {
        dr2["Eventdata"] = "No Results.";
        }
        else
        {
            dr2["Eventdata"] = concatStrngMn;
        }

        dtNew.Rows.Add(dr2);
       // System.Data.DataRow dr3 = dtNew.NewRow();
       // dr3["Eventname"] = dtNew3.Rows[0]["Eventname"];
       // dr3["Eventdata"] = dtNew3.Rows[0]["Eventdata"];
       // dtNew.Rows.Add(dr3);
         String Mesrmntid = "select MeasurementId from BehaviourDetails b where b.StudentId= " + sess.StudentId +" and activeind = 'A' " ;
        System.Data.DataTable dtNew4 = objData.ReturnDataTable(Mesrmntid, false);
        System.Data.DataRow dr4 = dtNew.NewRow();
        System.Data.DataTable dtNew5 ;
        System.Data.DataTable dtNew6;

        String Beh = "", Evnt = "", concatStrng="";
        foreach (DataRow row in dtNew4.Rows)
        {
            if (row != null)
            {
                String Behav = "SELECT Behaviour  from BehaviourDetails where MeasurementId =" + row["MeasurementId"];
                dtNew5=objData.ReturnDataTable(Behav, false);
                Beh=Convert.ToString(dtNew5.Rows[0]["Behaviour"]);

                string queryfilter = " SELECT STUFF((select '; ' + CONVERT(VARCHAR(50), outr.EvntTs,101)+','+ outr.eventname from( SELECT * FROM (SELECT * FROM  ((SELECT  SE.MeasurementId, SE.StdtSessEventId,  SE.EventName, " +
              " SE.StdtSessEventType, CONVERT(CHAR(10), SE.EvntTs,101) AS EvntTs, " +
              "  B.Behaviour FROM  [StdtSessEvent] SE LEFT JOIN LessonPlan L ON SE.LessonPlanId = L.LessonPlanId " +
              "LEFT JOIN BehaviourDetails B ON B.MeasurementId=SE.MeasurementId WHERE EventType='EV' AND SE.StudentId=" + sess.StudentId + " AND SE.StdtSessEventType<>'Medication') " +
              "UNION ALL (SELECT NULL AS MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
              "+ ( " +
              "        (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
              "        FROM   [user] US" +
              "       WHERE  US.userid = (SELECT" +
              "             createdby" +
              "                         FROM" +
              "           stdtsessionhdr Hdr" +
              "                       WHERE" +
              "         Hdr.stdtsessionhdrid = SH.ioasessionhdrid" +
              "        AND SH.ioaind = 'Y'))" +
              " + '/'" +
              " + (SELECT Rtrim(Ltrim(Upper(userinitial)))" +
              "   FROM   [user] US" +
              "  WHERE  SH.ioauserid = US.userid) ) AS EventName," +
              " 'Arrow notes'                         AS" +
              " StdtSessEventType," +
              "CONVERT(CHAR(10), SH.endts, 101)      AS EvntTs," +
              "NULL                                  AS Behaviour" +
              " FROM   stdtsessionhdr SH" +
              "       LEFT JOIN lessonplan" +
              "             ON SH.lessonplanid = lessonplan.lessonplanid" +
              " WHERE  SH.ioaperc IS NOT NULL" +
              "      AND SH.ioaind = 'Y'" +
              "     AND SH.sessionstatuscd = 'S'" +
              "    AND SH.studentid =" + sess.StudentId + ")" +
              "UNION ALL (SELECT  BIOA.MeasurementId, NULL AS StdtSessEventId,  'IOA '+CONVERT(nvarchar,ROUND(IOAPerc,0),0)+'% '+" +
              "+ CASE WHEN BIOA.normalbehaviorid IS NULL THEN ((SELECT" +
              "      TOP 1" +
              "     Rtrim(" +
              "    Ltrim(Upper(" +
              "   US.userinitial))) FROM behaviour BH INNER JOIN [user] US" +
              "  ON" +
              " BH.createdby" +
              " =" +
              " US.userid WHERE BH.createdon BETWEEN" +
                " Dateadd(minute, -5, BIOA.createdon)" +
                " AND" +
                " BIOA.createdon ORDER BY BH.createdon DESC)+'/'+ (SELECT" +
                " TOP 1" +
                " Rtrim(Ltrim(Upper(US.userinitial))) FROM" +
                " behaviorioadetails BI" +
                " INNER" +
                " JOIN [user]" +
                " US ON BI.createdby =" +
                " US.userid WHERE BI.createdon=BIOA.createdon ORDER BY" +
                " BI.createdon DESC)" +
                " )" +
                " ELSE ((" +
                " SELECT" +
                " Rtrim(Ltrim(Upper(US.userinitial))) FROM behaviour BH" +
                " INNER" +
                " JOIN [user]" +
                " US ON" +
                " BH.createdby = US.userid WHERE" +
                " BIOA.normalbehaviorid=BH.behaviourid)+'/'+ (" +
                " SELECT Rtrim(" +
                " Ltrim(Upper(US.userinitial))) FROM behaviorioadetails BI" +
                " INNER" +
                " JOIN" +
                " [user] US ON" +
                " BI.createdby = US.userid INNER JOIN behaviour BH ON" +
                " BH.behaviourid=BI.normalbehaviorid WHERE" +
                " BIOA.normalbehaviorid=BH.behaviourid))" +
                " END " +
              " AS EventName, " +
              "'Arrow notes' AS StdtSessEventType,  CONVERT(CHAR(10), BIOA.CreatedOn,101) AS EvntTs,  " +
              " BHD.Behaviour FROM BehaviorIOADetails BIOA LEFT JOIN BehaviourDetails BHD ON BIOA.MeasurementId=BHD.MeasurementId " +
              "WHERE BIOA.StudentId=" + sess.StudentId + " AND IOAPerc IS NOT NULL AND BIOA.ActiveInd='A') )IOA ) " +
              "  ad " +
              " WHERE  ( ( ad.behaviour IS  NULL " +
              " AND ad.measurementid = 0 )" +
              "OR ad.behaviour = (SELECT TOP 1 behaviour " +
              "FROM   behaviourdetails " +
              "WHERE  measurementid = " + row["MeasurementId"] + ") ) " +
              " AND ad.stdtsesseventtype IN( 'Arrow notes' ) " +
              " AND CONVERT(DATE, ad.evntts) >=  cast('" + stDate + "' as date) " +
              " AND CONVERT(DATE, ad.evntts) <=  cast('" + endDate + "' as date) " +
              " )outr "+
               " FOR XML PATH('')),1,1,'') eventname ";    
                dtNew6 = objData.ReturnDataTable(queryfilter, false);
                if (Convert.ToString(dtNew6.Rows[0]["eventname"]) != "" && Convert.ToString(dtNew6.Rows[0]["eventname"]) != null)
                concatStrng += "<b>" + Beh + " : " + "</b>" + Convert.ToString(dtNew6.Rows[0]["eventname"]) + "<br></br>";
            }

        }
        System.Data.DataRow dr5 = dtNew.NewRow();
        dr5["Eventname"] = "Arrow Notes";
        if (concatStrng != "")
        {
        dr5["Eventdata"] = concatStrng;
        }
        else
        {
            dr5["Eventdata"] = "No Results.";
        }
        dtNew.Rows.Add(dr5);
        grdGraphData.DataSource = dtNew;
        grdGraphData.DataBind();

    }



    protected void LoadReport(string date)
    {
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: LoadReport";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);


        tdMsg.InnerHtml = "";

        clearDatas();
        PanelMain.Visible = true;
        //  drpSelectDate.Enabled = false;
        //   btnLoadDate.Text = "Select New Date";

        dtAssmtReinfo.Columns.Add("Date");
        dtAssmtReinfo.Columns.Add("ToolUtilized");

        dtRecChang.Columns.Add("Recommendation");
        dtRecChang.Columns.Add("Timeline");
        dtRecChang.Columns.Add("Person Responsible");

        dtAssmntTool.Columns.Add("TargetBehavior");
        dtAssmntTool.Columns.Add("Function");
        dtAssmntTool.Columns.Add("AnalysisTools");

        loadAssmtReinfo();
        loadRecChange();
        loadAssmntTool();
        objData = new clsData();
        // date for sected date
        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }

        if (objData.IFExists("select ClinicalCvId from StdtClinicalCoverSheet where CAST(StartDate as date)= cast('" + stDate + "' as date) and CAST( EndDate as date)= cast('" + endDate + "' as date) and SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + "") == false)
        {
            getheadergenerate(sess.Classid, sess.SchoolId, sess.StudentId, date);
            DataLoad(date, 0);
            btnSave.Text = "Save";
            btnExport.Visible = false;
            btnGenDelete.Visible = false;
            Datacarryover();
        }
        else
        {
            //DataLoad(date);
            FillGraphData(date);
            loadDataTodayTemp(date);

            //--------------
            System.Data.DataTable DtStdetailGrid = new System.Data.DataTable();
            clsClinicalCoverSheet objExport = new clsClinicalCoverSheet();

            //try
            //{
            //    string strClinclCvid = "select ClinicalCvId from StdtClinicalCoverSheet where CAST(StartDate as date)= cast('" + stDate + "' as date) and CAST( EndDate as date)= cast('" + endDate + "' as date) and SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + "";
            //    object getClinclCvid = objData.FetchValue(strClinclCvid);
            //    hdFldCvid.Value = getClinclCvid.ToString();
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}

            try
            {
                int Flcvid = 0;
                if (hdFldCvid.Value != null && hdFldCvid.Value != "")
                {
                    Flcvid = int.Parse(hdFldCvid.Value);
                }
                if (Flcvid > 0 && Flcvid.ToString() != null)
                {
                    DtStdetailGrid = objExport.getGridHeaderValz(sess.StudentId, sess.SchoolId, Flcvid);

                    //string getpgmlst = "select Program from StdtClinicalCoverSheet where ClinicalCvId =" +Flcvid;
                    //string getloctnlst = "select Location from StdtClinicalCoverSheet where ClinicalCvId =" + Flcvid;
                    //string getpgmlstA = objData.FetchValue(getpgmlst).ToString();
                    //string getloctnlstA = objData.FetchValue(getloctnlst).ToString();

                    //string setpgmlst = "SELECT (SELECT STUFF(( SELECT ', '+Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId IN (" + getpgmlstA + ") FOR XML PATH('')), 1, 1, '')) from StdtClinicalCoverSheet where ClinicalCvId =" + Flcvid;
                    //string setloctnlst = "SELECT (SELECT STUFF(( SELECT ', '+ClassName FROM Class WHERE ClassId IN  (" + getloctnlstA + ") FOR XML PATH('')), 1, 1, ''))from StdtClinicalCoverSheet where ClinicalCvId =" + Flcvid;
                    //string setpgmlstA = objData.FetchValue(setpgmlst).ToString();
                    //string setloctnlstA = objData.FetchValue(setloctnlst).ToString();

                    System.Data.DataColumn newProgram = new System.Data.DataColumn("Program", typeof(System.String));
                    newProgram.DefaultValue = getHdrPrograms(Flcvid); //setpgmlstA.ToString();
                    DtStdetailGrid.Columns.Add(newProgram);

                    System.Data.DataColumn newLocation = new System.Data.DataColumn("Location", typeof(System.String));
                    newLocation.DefaultValue = getHdrLocations(Flcvid); //setloctnlstA.ToString();
                    DtStdetailGrid.Columns.Add(newLocation);

                    GridView2.DataSource = DtStdetailGrid;
                    GridView2.DataBind();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            //--------------

            DataLoad(date, int.Parse(hdFldCvid.Value));
            btnSave.Text = "Update";
            btnExport.Visible = true;
            btnGenDelete.Visible = true;
        }

    }


    public void LoadDataList()
    {
        sess = (clsSession)Session["UserSession"];
        //dt = new System.Data.DataTable();
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: LoadDataList";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);
        objData = new clsData();
        //dt = objData.ReturnDataTable("select CONVERT(varchar(10),StartDate,101)+'-'+CONVERT(varchar(10),EndDate,101) as EDate from StdtClinicalCoverSheet WHERE SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " order by StartDate desc", false);
        //dlDateList.DataSource = dt;
        //dlDateList.DataBind();
        System.Data.DataTable dtt = objData.ReturnDataTable("select year(EndDate) as EDate from StdtClinicalCoverSheet WHERE SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " group by year(EndDate) order by year(EndDate) desc", false);
        DataList1.DataSource = dtt;
        DataList1.DataBind();

    }


    public void loadDataTodayTemp(string date)
    {

        dt = new System.Data.DataTable();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: loadDataTodayTemp";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);

        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }


        dt = objData.ReturnDataTable("select ClinicalCvId,SchoolId,ClassId,StudentId,CONVERT(VARCHAR(10),StartDate,101) as StartDate,CONVERT(VARCHAR(10),EndDate,101) as EndDate,Academic,Clinical,Community,Other,FollowUp,Proposals,PgmCord,CONVERT(VARCHAR(10),PgmCordDate,101) as PgmCordDate ,EduCord,CONVERT(VARCHAR(10),EduCordDate,101) as EduCordDate ,BCBA,CONVERT(VARCHAR(10),BCBADate,101) as BCBADate from StdtClinicalCoverSheet where CAST(StartDate as date)= cast('" + stDate + "' as date) AND CAST( EndDate as date)= cast('" + endDate + "' as date) and SchoolId='" + sess.SchoolId + "' and StudentId='" + sess.StudentId + "'", true);
        if (dt.Rows.Count > 0)
        {
            txtAcademic.InnerHtml = dt.Rows[0]["Academic"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            txtClinical.InnerHtml = dt.Rows[0]["Clinical"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            txtCommunity.InnerHtml = dt.Rows[0]["Community"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            txtOther.InnerHtml = dt.Rows[0]["Other"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");

            hfAcademic.Value = dt.Rows[0]["Academic"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            hfClinical.Value = dt.Rows[0]["Clinical"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            hfCommunity.Value = dt.Rows[0]["Community"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            hfOther.Value = dt.Rows[0]["Other"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");

            txtFollowUp.InnerHtml = dt.Rows[0]["FollowUp"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            txtPreposals.InnerHtml = dt.Rows[0]["Proposals"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            hfFollowUp.Value = dt.Rows[0]["FollowUp"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            hfPreposals.Value = dt.Rows[0]["Proposals"].ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            txtPgmCordntr.Text = dt.Rows[0]["PgmCord"].ToString();
            txteduCodntr.Text = dt.Rows[0]["EduCord"].ToString();
            txtBCBA.Text = dt.Rows[0]["BCBA"].ToString();
            txtDatePgnCord.Text = ((dt.Rows[0]["PgmCordDate"].ToString()) == "01/01/1900") ? "" : (dt.Rows[0]["PgmCordDate"].ToString());
            txtDateEduCord.Text = ((dt.Rows[0]["EduCordDate"].ToString()) == "01/01/1900") ? "" : (dt.Rows[0]["EduCordDate"].ToString());
            txtDateBCBACord.Text = ((dt.Rows[0]["BCBADate"].ToString()) == "01/01/1900") ? "" : (dt.Rows[0]["BCBADate"].ToString());

            // drpSelectDate.SelectedItem.Text = dt.Rows[0]["EndDate"].ToString();
            int clvId = int.Parse(dt.Rows[0]["ClinicalCvId"].ToString());
            hdFldCvid.Value = dt.Rows[0]["ClinicalCvId"].ToString();

            dt = objData.ReturnDataTable("select clvAsmtToolId,TrgetBehav as TargetBehavior,Functions as 'Function' ,AnalysisTool as AnalysisTools from clvAsmtTool where StdtCoverSheetId=" + clvId, true);
            if (dt.Rows.Count > 0)
            {
                gVAssmntTool.DataSource = dt;
                gVAssmntTool.DataBind();
            }


            dt = objData.ReturnDataTable("select clvRecChangeId,Recomendation as Recommendation,TimeLine as Timeline, PersonResponsible as 'Person Responsible' from clvRecChange where StdtCoverSheetId=" + clvId, true);
            if (dt.Rows.Count > 0)
            {
                gVRecChange.DataSource = dt;
                gVRecChange.DataBind();
            }


            dt = objData.ReturnDataTable("select clvReinfoSurId,CONVERT(varchar(10),ReinfoDate,101) as 'Date', ToolUtilizd as ToolUtilized from clvReinfoSur where StdtCoverSheetId=" + clvId, true);
            if (dt.Rows.Count > 0)
            {
                gVAssmtReinfo.DataSource = dt;
                gVAssmtReinfo.DataBind();
            }
        }
    }

    //public void loadDataToday()
    //{

    //    dt = new System.Data.DataTable();
    //    objData = new clsData();
    //    sess = (clsSession)Session["UserSession"];
    //    dt = objData.ReturnDataTable("select ClinicalCvId,SchoolId,ClassId,StudentId,CONVERT(VARCHAR(10),StartDate,101) as StartDate,CONVERT(VARCHAR(10),EndDate,101) as EndDate,FollowUp,Proposals,PgmCord,CONVERT(VARCHAR(10),PgmCordDate,101) as PgmCordDate ,EduCord,CONVERT(VARCHAR(10),EduCordDate,101) as EduCordDate ,BCBA,CONVERT(VARCHAR(10),BCBADate,101) as BCBADate from StdtClinicalCoverSheet where CAST( EndDate as date)= cast('" + drpSelectDate.SelectedItem.Text + "' as date) and SchoolId='" + sess.SchoolId + "' and StudentId='" + sess.StudentId + "'", true);
    //    txtFollowUp.Text = dt.Rows[0]["FollowUp"].ToString();
    //    txtPreposals.Text = dt.Rows[0]["Proposals"].ToString();
    //    txtPgmCordntr.Text = dt.Rows[0]["PgmCord"].ToString();
    //    txteduCodntr.Text = dt.Rows[0]["EduCord"].ToString();
    //    txtBCBA.Text = dt.Rows[0]["BCBA"].ToString();
    //    txtDatePgnCord.Text = dt.Rows[0]["PgmCordDate"].ToString();
    //    txtDateEduCord.Text = dt.Rows[0]["EduCordDate"].ToString();
    //    txtDateBCBACord.Text = dt.Rows[0]["BCBADate"].ToString();
    //    drpSelectDate.SelectedItem.Text = dt.Rows[0]["EndDate"].ToString();
    //    int clvId = int.Parse(dt.Rows[0]["ClinicalCvId"].ToString());
    //    hdFldCvid.Value = dt.Rows[0]["ClinicalCvId"].ToString();
    //    dt = objData.ReturnDataTable("select clvAsmtToolId,TrgetBehav as TargetBehavior,Functions as 'Function' ,AnalysisTool as AnalysisTools from clvAsmtTool where StdtCoverSheetId=" + clvId, true);
    //    if (dt.Rows.Count > 0)
    //    {
    //        gVAssmntTool.DataSource = dt;
    //        gVAssmntTool.DataBind();
    //    }


    //    dt = objData.ReturnDataTable("select clvRecChangeId,Recomendation as Recommendation,TimeLine as Timeline, PersonResponsible as 'Person Responsible' from clvRecChange where StdtCoverSheetId=" + clvId, true);
    //    if (dt.Rows.Count > 0)
    //    {
    //        gVRecChange.DataSource = dt;
    //        gVRecChange.DataBind();

    //    }


    //    dt = objData.ReturnDataTable("select clvReinfoSurId,CONVERT(varchar(10),ReinfoDate,101) as 'Date', ToolUtilizd as ToolUtilized from clvReinfoSur where StdtCoverSheetId=" + clvId, true);
    //    if (dt.Rows.Count > 0)
    //    {
    //        gVAssmtReinfo.DataSource = dt;
    //        gVAssmtReinfo.DataBind();
    //    }

    //}




    public void clearDatas()
    {
        txtAcademic.InnerHtml = "";
        txtClinical.InnerHtml = "";
        txtCommunity.InnerHtml = "";
        txtOther.InnerHtml = "";
        hfAcademic.Value = "";
        hfClinical.Value = "";
        hfCommunity.Value = "";
        hfOther.Value = "";
        txtFollowUp.InnerHtml = "";
        txtPreposals.InnerHtml = "";
        hfPreposals.Value = "";
        hfFollowUp.Value = "";
        txtPgmCordntr.Text = "";
        txteduCodntr.Text = "";
        txtBCBA.Text = "";
        txtDatePgnCord.Text = "";
        txtDateEduCord.Text = "";
        txtDateBCBACord.Text = "";
        gVAssmntTool.DataSource = null;
        gVAssmntTool.DataBind();
        gVAssmtReinfo.DataSource = null;
        gVAssmtReinfo.DataBind();
        gVBehavior.DataSource = null;
        gVBehavior.DataBind();
        gVRecChange.DataSource = null;
        gVRecChange.DataBind();

    }
    public void Datacarryover()
    {       
        if (btnSave.Text == "Save")
        {
            objData = new clsData();
            dt = new System.Data.DataTable();
            sess = (clsSession)Session["UserSession"];
            int cvrId = 0;
            int cnt = 0;
            string strcarry;
            string follow;

            strcarry = "select top 1 ClinicalCvId from StdtClinicalCoverSheet where  SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + " order by ClinicalCvId desc";
            cvrId = Convert.ToInt32(objData.FetchValue(strcarry));
            if (cvrId != 0)
            {
                dt = objData.ReturnDataTable("select clvAsmtToolId,TrgetBehav as TargetBehavior,Functions as 'Function' ,AnalysisTool as AnalysisTools from clvAsmtTool where StdtCoverSheetId=" + cvrId, true);
                if (dt.Rows.Count > 0)
                {
                    gVAssmntTool.DataSource = dt;
                    gVAssmntTool.DataBind();
                }

                dt = objData.ReturnDataTable("select clvReinfoSurId,CONVERT(varchar(10),ReinfoDate,101) as 'Date', ToolUtilizd as ToolUtilized from clvReinfoSur where StdtCoverSheetId=" + cvrId, true);
                if (dt.Rows.Count > 0)
                {
                    gVAssmtReinfo.DataSource = dt;
                    gVAssmtReinfo.DataBind();
                }

                dt = objData.ReturnDataTable("select clvRecChangeId,Recomendation as Recommendation,TimeLine as Timeline, PersonResponsible as 'Person Responsible' from clvRecChange where StdtCoverSheetId=" + cvrId, true);
                cnt = dt.Rows.Count;
                string[] Rec = new string[cnt];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        Rec[i] = "<br/>" + " <b>Recommendation:</b>  " + Convert.ToString(dt.Rows[i][1]) +
                                "  <b>Timeline:</b>  " + Convert.ToString(dt.Rows[i][2]) +
                                "  <b>Person Responsible:</b>  " + Convert.ToString(dt.Rows[i][3]);
                    }
                }
                dt = objData.ReturnDataTable("select FollowUp from StdtClinicalCoverSheet where ClinicalCvId=" + cvrId, true);
                follow = dt.Rows[0]["FollowUp"].ToString();
                if (cnt > 0)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        follow = follow + Rec[i];
                    }

                }                
                txtFollowUp.InnerHtml = follow.Replace("&lt;", "<").Replace("&gt;", ">");
                hfFollowUp.Value = follow.Replace("&lt;", "<").Replace("&gt;", ">");
            }

        }
    }
    public void DataLoad(string date,int ClvShtid)
    {
        objData = new clsData();
        dt = new System.Data.DataTable();
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: DataLoad";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);

        string stDate = "";
        string endDate = "";
        if (date != null)
        {
            stDate = date.Split('-')[0];
            endDate = date.Split('-')[1];
        }
        DateTime todayDate = DateTime.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime lastDayDate = DateTime.ParseExact(stDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

        dt = objData.ExecuteCoversheetBehavior(lastDayDate, todayDate, sess.SchoolId, sess.StudentId);

        if (ClvShtid > 0)
        {
            System.Data.DataTable dtbehSumr = new System.Data.DataTable();
            try
            {
                int Flcvid = 0;
                string query = "";
                if (hdFldCvid.Value != null && hdFldCvid.Value != "")
                {
                    Flcvid = int.Parse(hdFldCvid.Value);
                    if (sess.SchoolId == 1)
                    {
                        query = "select StudentId,Measurementid,BehIepObj,BehlvlPerf from clvBehsummary where stdtcoversheetid = " + Flcvid + "";
                    }
                    else
                    {
                        query = "select StudentId,Measurementid,BehIepObj,BehlvlPerf from clvBehsummary where stdtcoversheetid = " + Flcvid + "";
                    }

                    dtbehSumr = objData.ReturnDataTable(query, false);

                    if (dtbehSumr != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtbehSumr.Rows.Count; j++)
                            {
                                object dtM1 = dt.Rows[i]["MeasurementId"];
                                object dtbehSumrM1 = dtbehSumr.Rows[j]["MeasurementId"];

                                int M1 = int.Parse(dtM1.ToString());
                                int M2 = int.Parse(dtbehSumrM1.ToString());
                                if (M1 == M2)
                                {
                                    dt.Rows[i]["IEPObj"] = dtbehSumr.Rows[j]["BehIepObj"];
                                    dt.Rows[i]["BasPerlvl"] = dtbehSumr.Rows[j]["BehlvlPerf"];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        gVBehavior.DataSource = dt;
        gVBehavior.DataBind();

    }
    //public void loadData()
    //{
    //    objData = new clsData();
    //    dt = new System.Data.DataTable();
    //    //  DateTime todayDate=DateTime.Parse(drpSelectDate.SelectedItem.Text);
    //    DateTime todayDate = DateTime.ParseExact(drpSelectDate.SelectedItem.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
    //    DateTime lastDayDate = todayDate.AddDays(-90);

    //    dt = objData.ReturnDataTable("select distinct BehaviourDetails.Behaviour from StdtAggScores inner join BehaviourDetails on StdtAggScores.MeasurementId=BehaviourDetails.MeasurementId where CAST( AggredatedDate as date) between   cast( '" + lastDayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and CAST( '" + todayDate.ToString("yyyy'/'MM'/'dd") + "' as date) and StdtAggScores.MeasurementId is not null and BehaviourDetails.ActiveInd='A'", true);
    //    gVBehavior.DataSource = dt;
    //    gVBehavior.DataBind();
    //}


    //*********** load and gVAssmntTool ***********
    public void loadAssmntTool()
    {

        DataRow dr = dtAssmntTool.NewRow();
        dr["TargetBehavior"] = "";
        dr["Function"] = "";
        dr["AnalysisTools"] = "";
        dtAssmntTool.Rows.Add(dr);
        gVAssmntTool.DataSource = dtAssmntTool;
        gVAssmntTool.DataBind();

    }
    public void addRowAssmntTool()
    {
        dtAssmntTool.Columns.Add("TargetBehavior", typeof(string));
        dtAssmntTool.Columns.Add("Function", typeof(string));
        dtAssmntTool.Columns.Add("AnalysisTools", typeof(string));
        foreach (GridViewRow gdVr in gVAssmntTool.Rows)
        {
            DataRow drtemp = dtAssmntTool.NewRow();

            drtemp["TargetBehavior"] = ((TextBox)gdVr.FindControl("txtTargetBehavior")).Text;
            drtemp["Function"] = ((TextBox)gdVr.FindControl("txtFunction")).Text;
            drtemp["AnalysisTools"] = ((TextBox)gdVr.FindControl("txtAnalysisTools")).Text;
            dtAssmntTool.Rows.Add(drtemp);
        }
        DataRow dr = dtAssmntTool.NewRow();
        dr["TargetBehavior"] = "";
        dr["Function"] = "";
        dr["AnalysisTools"] = "";
        dtAssmntTool.Rows.Add(dr);
        gVAssmntTool.DataSource = dtAssmntTool;
        gVAssmntTool.DataBind();
    }
    public void delRowAssmntTool(int intexRow)
    {
        dtAssmntTool.Columns.Add("TargetBehavior", typeof(string));
        dtAssmntTool.Columns.Add("Function", typeof(string));
        dtAssmntTool.Columns.Add("AnalysisTools", typeof(string));
        foreach (GridViewRow gdVr in gVAssmntTool.Rows)
        {
            DataRow drtemp = dtAssmntTool.NewRow();

            drtemp["TargetBehavior"] = ((TextBox)gdVr.FindControl("txtTargetBehavior")).Text;
            drtemp["Function"] = ((TextBox)gdVr.FindControl("txtFunction")).Text;
            drtemp["AnalysisTools"] = ((TextBox)gdVr.FindControl("txtAnalysisTools")).Text;
            dtAssmntTool.Rows.Add(drtemp);
        }
        if (dtAssmntTool.Rows.Count == 1)
        {
            dtAssmntTool.Rows.RemoveAt(intexRow);
            loadAssmntTool();
        }
        else
        {
            dtAssmntTool.Rows.RemoveAt(intexRow);
        }

        gVAssmntTool.DataSource = dtAssmntTool;
        gVAssmntTool.DataBind();
    }

    protected void gVAssmntTool_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (gVAssmntTool.Rows.Count == 5) return;
            addRowAssmntTool();
        }
    }
    protected void gVAssmntTool_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowAssmntTool(e.RowIndex);
    }

    //*********** load and gVAssmntTool End***********

    //*********** load and gVRecChange ***********
    public void loadRecChange()
    {

        DataRow dr = dtRecChang.NewRow();
        dr["Recommendation"] = "";
        dr["Timeline"] = "";
        dr["Person Responsible"] = "";
        dtRecChang.Rows.Add(dr);
        gVRecChange.DataSource = dtRecChang;
        gVRecChange.DataBind();
        //dtRecChang.Rows.Add(
    }
    public void addRow()
    {
        dtRecChang.Columns.Add("Recommendation", typeof(string));
        dtRecChang.Columns.Add("Timeline", typeof(string));
        dtRecChang.Columns.Add("Person Responsible", typeof(string));
        foreach (GridViewRow gdVr in gVRecChange.Rows)
        {
            DataRow drtemp = dtRecChang.NewRow();

            drtemp["Recommendation"] = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            drtemp["Timeline"] = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            drtemp["Person Responsible"] = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            dtRecChang.Rows.Add(drtemp);
        }
        DataRow dr = dtRecChang.NewRow();
        dr["Recommendation"] = "";
        dr["Timeline"] = "";
        dr["Person Responsible"] = "";
        dtRecChang.Rows.Add(dr);
        gVRecChange.DataSource = dtRecChang;
        gVRecChange.DataBind();
    }
    public void delRow(int intexRow)
    {
        dtRecChang.Columns.Add("Recommendation", typeof(string));
        dtRecChang.Columns.Add("Timeline", typeof(string));
        dtRecChang.Columns.Add("Person Responsible", typeof(string));
        foreach (GridViewRow gdVr in gVRecChange.Rows)
        {
            DataRow drtemp = dtRecChang.NewRow();
            string temp = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            temp = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            temp = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            drtemp["Recommendation"] = ((TextBox)gdVr.FindControl("txtRecomd")).Text;
            drtemp["Timeline"] = ((TextBox)gdVr.FindControl("txtTimeLine")).Text;
            drtemp["Person Responsible"] = ((TextBox)gdVr.FindControl("txtPerRes")).Text;
            dtRecChang.Rows.Add(drtemp);
        }
        if (dtRecChang.Rows.Count == 1)
        {
            dtRecChang.Rows.RemoveAt(intexRow);
            loadRecChange();
        }
        else
        {
            dtRecChang.Rows.RemoveAt(intexRow);
        }

        gVRecChange.DataSource = dtRecChang;
        gVRecChange.DataBind();

    }

    protected void gVRecChange_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (gVRecChange.Rows.Count == 5) return;
            addRow();
        }

    }
    protected void btnAddRow_Click(object sender, EventArgs e)
    {

    }
    protected void gVRecChange_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRow(e.RowIndex);
    }

    //*********** load and gVRecChange End***********

    //*********** load and gVAssmtReinfo ***********
    public void loadAssmtReinfo()
    {

        DataRow dr = dtAssmtReinfo.NewRow();
        dr["Date"] = "";
        dr["ToolUtilized"] = "";

        dtAssmtReinfo.Rows.Add(dr);
        gVAssmtReinfo.DataSource = dtAssmtReinfo;
        gVAssmtReinfo.DataBind();

    }
    public void addRowAssmtReinfo()
    {
        dtAssmtReinfo.Columns.Add("Date", typeof(string));
        dtAssmtReinfo.Columns.Add("ToolUtilized", typeof(string));

        foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
        {
            DataRow drtemp = dtAssmtReinfo.NewRow();

            drtemp["Date"] = ((TextBox)gdVr.FindControl("txtDate")).Text;
            drtemp["ToolUtilized"] = ((TextBox)gdVr.FindControl("txtToolUtilized")).Text;

            dtAssmtReinfo.Rows.Add(drtemp);
        }
        DataRow dr = dtAssmtReinfo.NewRow();
        dr["Date"] = "";
        dr["ToolUtilized"] = "";

        dtAssmtReinfo.Rows.Add(dr);
        gVAssmtReinfo.DataSource = dtAssmtReinfo;
        gVAssmtReinfo.DataBind();
    }
    public void delRowAssmtReinfo(int intexRow)
    {
        dtAssmtReinfo.Columns.Add("Date", typeof(string));
        dtAssmtReinfo.Columns.Add("ToolUtilized", typeof(string));

        foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
        {
            DataRow drtemp = dtAssmtReinfo.NewRow();

            drtemp["Date"] = ((TextBox)gdVr.FindControl("txtDate")).Text;
            drtemp["ToolUtilized"] = ((TextBox)gdVr.FindControl("txtToolUtilized")).Text;

            dtAssmtReinfo.Rows.Add(drtemp);
        }
        if (dtAssmtReinfo.Rows.Count == 1)
        {
            dtAssmtReinfo.Rows.RemoveAt(intexRow);
            loadAssmtReinfo();
        }
        else
        {
            dtAssmtReinfo.Rows.RemoveAt(intexRow);
        }
        gVAssmtReinfo.DataSource = dtAssmtReinfo;
        gVAssmtReinfo.DataBind();
    }

    protected void gVAssmtReinfo_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadDateJqry();", true);
        if (e.CommandName == "AddRow")
        {
            if (gVAssmtReinfo.Rows.Count == 5) return;
            addRowAssmtReinfo();
        }
    }
    protected void gVAssmtReinfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        delRowAssmtReinfo(e.RowIndex);
    }
    //*********** load and gVAssmtReinfo End ***********

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool iepDateCheckStatus = iepdatecheck();
        if (iepDateCheckStatus == true)
        {
            SaveOrUpdateData();
        }
    }

    private void SaveOrUpdateData()
    {
        tdMsg.InnerHtml = "";
        string date = "";
        objData = new clsData();
        DateTime todayDate = new DateTime();
        DateTime lastdate = new DateTime();
        if (btnSave.Text == "Save")
        {
            todayDate = DateTime.ParseExact(txtEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            lastdate = DateTime.ParseExact(txtSdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }
        string iepDate = "";
        string querry = "";
        int iepPending = 0;
        int iepApproval = 0;
        int iepInprogress = 0;
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: SaveOrUpdateData";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);


        if (ViewState["CurrentDate"] != null)
        {
            date = ViewState["CurrentDate"].ToString();
        }

        querry = "SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'In Progress'";
        object objId = objData.FetchValue(querry);
        if (objId != null)
        {
            iepInprogress = Convert.ToInt32(objId);
        }

        querry = "SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'Approved'";
        object objIdApp = objData.FetchValue(querry);
        if (objIdApp != null)
        {
            iepApproval = Convert.ToInt32(objIdApp);
        }

        querry = "SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'Pending Approval'";
        object objIdPend = objData.FetchValue(querry);
        if (objIdPend != null)
        {
            iepPending = Convert.ToInt32(objIdPend);
        }

        string school = ConfigurationManager.AppSettings["School"].ToString();
        if (school == "NE")
        {
            querry = "Select TOP 1 CONVERT(varchar,[EffStartDate],101)+'-'+CONVERT(varchar,[EffEndDate],101) As IEPDATE FROM StdtIEP WHERE StudentId = " + sess.StudentId + " and StatusId IN ( " + iepInprogress + "," + iepApproval + "," + iepPending + ") ORDER BY StdtIEPId DESC";

        }
        else
        {
            querry = "Select TOP 1 CONVERT(varchar,[EffStartDate],101)+'-'+CONVERT(varchar,[EffEndDate],101) As IEPDATE FROM StdtIEP_PE WHERE StudentId = " + sess.StudentId + " and StatusId IN ( " + iepInprogress + "," + iepApproval + "," + iepPending + ") ORDER BY StdtIEP_PEId DESC";

        }
        object objVal = objData.FetchValue(querry);
        if (objVal != null)
        {
            iepDate = objVal.ToString();
        }

        //string Program = Convert.ToString(objData.FetchValue("SELECT Department FROM Placement WHERE StudentPersonalId=" + sess.StudentId + " AND Location=" + sess.Classid + ""));

        string getLoctn = objData.FetchValue("SELECT STUFF(( SELECT ','+CAST(Location AS VARCHAR(500)) FROM Placement WHERE StudentPersonalId = " + sess.StudentId + " AND EndDate IS NULL AND Status > 0 FOR XML PATH('')), 1, 1, '')").ToString();
        string getPgm = "";
        if (getLoctn != "")
        {
            getPgm = objData.FetchValue("SELECT (SELECT STUFF(( SELECT ','+CAST(Department AS VARCHAR(500)) FROM Placement WHERE StudentPersonalId = " + sess.StudentId + "  AND Location IN(" + getLoctn + ") AND EndDate IS NULL AND STATUS > 0 FOR XML PATH('')), 1, 1, ''))").ToString();
        }

        //else
        //{
        //    querry = "Select CONVERT(varchar,[EffStartDate],101)+'-'+CONVERT(varchar,[EffEndDate],101) As IEPDATE FROM StdtIEP WHERE StudentId = " + sess.StudentId + " and StatusId = " + iepApproval;
        //    object objValcheck = objData.FetchValue(querry);
        //    if (objValcheck != null)
        //    {
        //        iepDate = objValcheck.ToString();
        //    }
        //    else
        //    {
        //        querry = "Select CONVERT(varchar,[EffStartDate],101)+'-'+CONVERT(varchar,[EffEndDate],101) As IEPDATE FROM StdtIEP WHERE StudentId = " + sess.StudentId + " and StatusId = " + iepPending;
        //        object objValcheckApp = objData.FetchValue(querry);
        //        if (objValcheckApp != null)
        //        {
        //            iepDate = objValcheckApp.ToString();
        //        }
        //    }
        //}

        if (btnSave.Text == "Save")
        {
            //string PeriodOfAssessment = Convert.ToString(objData.FetchValue("SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A'"));
            string PeriodOfAssessment = lastdate.ToString("MM'/'dd'/'yy") + " - " + todayDate.ToString("MM'/'dd'/'yy");
            //string strquery = "insert into StdtClinicalCoverSheet(StartDate,EndDate,Academic,Clinical,Community,Other,FollowUp,Proposals,PgmCord,PgmCordDate,EduCord,EduCordDate,BCBA,BCBADate,SchoolId,StudentId,ClassId,IepYear,Location,PeriodOfAssmt,Program) values('" + lastdate.ToString("yyyy'/'MM'/'dd") + "','" + todayDate.ToString("yyyy'/'MM'/'dd") + "','" + clsGeneral.convertQuotes(Convert.ToString(hfAcademic.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfClinical.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfCommunity.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfOther.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfFollowUp.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfPreposals.Value)) + "','" + clsGeneral.convertQuotes(txtPgmCordntr.Text) + "','" + clsGeneral.convertQuotes(txtDatePgnCord.Text) + "','" + clsGeneral.convertQuotes(txteduCodntr.Text) + "','" + clsGeneral.convertQuotes(txtDateEduCord.Text) + "','" + clsGeneral.convertQuotes(txtBCBA.Text) + "','" + clsGeneral.convertQuotes(txtDateBCBACord.Text) + "'," + sess.SchoolId + "," + sess.StudentId + "," + sess.Classid + ",'" + iepDate + "'," + sess.Classid + ",'" + PeriodOfAssessment + "','" + Program + "')";
            string strquery = "insert into StdtClinicalCoverSheet(StartDate,EndDate,Academic,Clinical,Community,Other,FollowUp,Proposals,PgmCord,PgmCordDate,EduCord,EduCordDate,BCBA,BCBADate,SchoolId,StudentId,ClassId,IepYear,Location,PeriodOfAssmt,Program) values('" + lastdate.ToString("yyyy'/'MM'/'dd") + "','" + todayDate.ToString("yyyy'/'MM'/'dd") + "','" + clsGeneral.convertQuotes(Convert.ToString(hfAcademic.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfClinical.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfCommunity.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfOther.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfFollowUp.Value)) + "','" + clsGeneral.convertQuotes(Convert.ToString(hfPreposals.Value)) + "','" + clsGeneral.convertQuotes(txtPgmCordntr.Text) + "','" + clsGeneral.convertQuotes(txtDatePgnCord.Text) + "','" + clsGeneral.convertQuotes(txteduCodntr.Text) + "','" + clsGeneral.convertQuotes(txtDateEduCord.Text) + "','" + clsGeneral.convertQuotes(txtBCBA.Text) + "','" + clsGeneral.convertQuotes(txtDateBCBACord.Text) + "'," + sess.SchoolId + "," + sess.StudentId + "," + sess.Classid + ",'" + iepDate + "','" + getLoctn + "','" + PeriodOfAssessment + "','" + getPgm + "')";
            int cvrsheetId = objData.ExecuteWithScope(strquery);

            if (cvrsheetId > 0)
            {
                //insert iepdates
                foreach (GridViewRow gdIepdates in GridView2.Rows)
                {
                    if ((((TextBox)gdIepdates.FindControl("txtDate1")).Text != "") && (((TextBox)gdIepdates.FindControl("txtDate2")).Text != "") && (((HiddenField)gdIepdates.FindControl("iepstdid")).Value != ""))
                    {
                        string iepstdate = ((TextBox)gdIepdates.FindControl("txtDate1")).Text;
                        string iependate = ((TextBox)gdIepdates.FindControl("txtDate2")).Text;
                        string iepstuddid = ((HiddenField)gdIepdates.FindControl("iepstdid")).Value;

                        string strqueryiepdateupdate = "Update StdtClinicalCoverSheet Set ClinicalBehIEPSDate = '" + iepstdate + "', ClinicalBehIEPEDate = '" + iependate + "' Where ClinicalCvId = " + cvrsheetId + " AND StudentId=" + iepstuddid + "";
                        objData.Execute(strqueryiepdateupdate);

                        iepdatesupdate(iepstuddid, iepstdate, iependate);
                    }
                }

                //insert IEp objective and behlvl
                foreach (GridViewRow gdIepobj in gVBehavior.Rows)
                {
                    if (((((TextBox)gdIepobj.FindControl("TextArea1")).Text != "") || (((TextBox)gdIepobj.FindControl("TextArea2")).Text != "")) && (((HiddenField)gdIepobj.FindControl("behvid")).Value != ""))
                    {
                        //strquery = "insert into clvAsmtTool(StdtCoverSheetId,TrgetBehav,Functions,AnalysisTool) values(" + cvrsheetId + ",'" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtTargetBehavior")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtFunction")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtAnalysisTools")).Text) + "')";
                        //objData.Execute(strquery);

                        string iepobj = ((TextBox)gdIepobj.FindControl("TextArea1")).Text;
                        string iepbehlvl = ((TextBox)gdIepobj.FindControl("TextArea2")).Text;
                        string behid = ((HiddenField)gdIepobj.FindControl("behvid")).Value;
                        string iepstuddid = sess.StudentId.ToString();

                        strquery = "insert into clvBehsummary(StdtCoverSheetId,Measurementid,Studentid,BehIepObj,BehlvlPerf) values(" + cvrsheetId + "," + behid + "," + sess.StudentId + ",'" + clsGeneral.convertQuotes(iepobj) + "','" + clsGeneral.convertQuotes(iepbehlvl) + "')";
                        objData.Execute(strquery);

                        iepobj_behlvlupdate(iepstuddid, behid, iepbehlvl, iepobj);

                    }
                }

                // inserting Assmt tools
                foreach (GridViewRow gdVr in gVAssmntTool.Rows)
                {
                    if ((((TextBox)gdVr.FindControl("txtTargetBehavior")).Text != "") || (((TextBox)gdVr.FindControl("txtFunction")).Text != "") || (((TextBox)gdVr.FindControl("txtAnalysisTools")).Text != ""))
                    {
                        strquery = "insert into clvAsmtTool(StdtCoverSheetId,TrgetBehav,Functions,AnalysisTool) values(" + cvrsheetId + ",'" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtTargetBehavior")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtFunction")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtAnalysisTools")).Text) + "')";
                        objData.Execute(strquery);
                    }
                }
                //inserting Reinforcment  insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values()

                foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
                {
                    if ((((TextBox)gdVr.FindControl("txtDate")).Text != "") || (((TextBox)gdVr.FindControl("txtToolUtilized")).Text != ""))
                    {
                        strquery = "insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values(" + cvrsheetId + ",'" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtDate")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtToolUtilized")).Text) + "')";
                        objData.Execute(strquery);
                    }

                }
                //inserting Rec Changes
                foreach (GridViewRow gdVr in gVRecChange.Rows)
                {
                    if ((((TextBox)gdVr.FindControl("txtRecomd")).Text != "") || (((TextBox)gdVr.FindControl("txtTimeLine")).Text != "") || (((TextBox)gdVr.FindControl("txtPerRes")).Text != ""))
                    {
                        strquery = "insert into clvRecChange(StdtCoverSheetId,Recomendation,TimeLine,PersonResponsible) values(" + cvrsheetId + ",'" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtRecomd")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtTimeLine")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtPerRes")).Text) + "')";
                        objData.Execute(strquery);
                    }
                }
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Details Inserted Successfully");
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Details Insertion Failed");
            }

        }

        if (btnSave.Text == "Update")
        {
            //  string s1 = " update StdtClinicalCoverSheet set FollowUp='" + txtFollowUp.Text + "',Proposals='" + txtPreposals.Text + "',PgmCord='" + txtPgmCordntr.Text + "',PgmCordDate='" + txtDatePgnCord.Text + "',EduCord='" + txteduCodntr.Text + "',EduCordDate='" + txtDateEduCord.Text + "',BCBA='" + txtBCBA.Text + "',BCBADate='"+txtDateBCBACord.Text+"'";

            string strquery = "update StdtClinicalCoverSheet Set Academic='" + clsGeneral.convertQuotes(Convert.ToString(hfAcademic.Value)) + "',Clinical='" + clsGeneral.convertQuotes(Convert.ToString(hfClinical.Value)) + "',Community='" + clsGeneral.convertQuotes(Convert.ToString(hfCommunity.Value)) + "',Other='" + clsGeneral.convertQuotes(Convert.ToString(hfOther.Value)) + "',FollowUp='" + clsGeneral.convertQuotes(Convert.ToString(hfFollowUp.Value)) + "',Proposals='" + clsGeneral.convertQuotes(Convert.ToString(hfPreposals.Value)) + "',PgmCord='" + clsGeneral.convertQuotes(txtPgmCordntr.Text) + "',PgmCordDate='" + clsGeneral.convertQuotes(txtDatePgnCord.Text) + "',EduCord='" + clsGeneral.convertQuotes(txteduCodntr.Text) + "',EduCordDate='" + clsGeneral.convertQuotes(txtDateEduCord.Text) + "',BCBA='" + clsGeneral.convertQuotes(txtBCBA.Text) + "',BCBADate='" + clsGeneral.convertQuotes(txtDateBCBACord.Text) + "' where ClinicalCvId=" + int.Parse(hdFldCvid.Value);
            int cvrsheetId = objData.Execute(strquery);

            strquery = " delete from clvBehsummary where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvAsmtTool where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvRecChange where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvReinfoSur where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);

            //insert iepdates
            foreach (GridViewRow gdIepdates in GridView2.Rows)
            {
                if ((((TextBox)gdIepdates.FindControl("txtDate1")).Text != "") && (((TextBox)gdIepdates.FindControl("txtDate2")).Text != "") && (((HiddenField)gdIepdates.FindControl("iepstdid")).Value != ""))
                {
                    string iepstartdate = ((TextBox)gdIepdates.FindControl("txtDate1")).Text;
                    string iependdate = ((TextBox)gdIepdates.FindControl("txtDate2")).Text;
                    string iepstuddid = ((HiddenField)gdIepdates.FindControl("iepstdid")).Value;

                    string strqueryiepdateupdate = "Update StdtClinicalCoverSheet Set ClinicalBehIEPSDate ='" + iepstartdate + "',ClinicalBehIEPEDate='" + iependdate + "' Where ClinicalCvId = " + int.Parse(hdFldCvid.Value) + " AND StudentId=" + iepstuddid + "";
                    objData.Execute(strqueryiepdateupdate);

                    iepdatesupdate(iepstuddid, iepstartdate, iependdate);
                }
            }

            //insert Iep objective and behlvl
            foreach (GridViewRow gdIepobj in gVBehavior.Rows)
            {
                if (((((TextBox)gdIepobj.FindControl("TextArea1")).Text != "") || (((TextBox)gdIepobj.FindControl("TextArea2")).Text != "")) && (((HiddenField)gdIepobj.FindControl("behvid")).Value != ""))
                {
                    string iepobj = ((TextBox)gdIepobj.FindControl("TextArea1")).Text;
                    string iepbehlvl = ((TextBox)gdIepobj.FindControl("TextArea2")).Text;
                    string behid = ((HiddenField)gdIepobj.FindControl("behvid")).Value;
                    string iepstuddid = sess.StudentId.ToString();

                    strquery = "insert into clvBehsummary(StdtCoverSheetId,Measurementid,Studentid,BehIepObj,BehlvlPerf) values(" + int.Parse(hdFldCvid.Value) + "," + behid + "," + sess.StudentId + ",'" + clsGeneral.convertQuotes(iepobj) + "','" + clsGeneral.convertQuotes(iepbehlvl) + "')";
                    objData.Execute(strquery);

                    //do not update based behaviourdetails table on individual coversheet//iepobj_behlvlupdate(iepstuddid, behid, iepbehlvl, iepobj);
                }
            }

            foreach (GridViewRow gdVr in gVAssmntTool.Rows)
            {
                if ((((TextBox)gdVr.FindControl("txtTargetBehavior")).Text != "") || (((TextBox)gdVr.FindControl("txtFunction")).Text != "") || (((TextBox)gdVr.FindControl("txtAnalysisTools")).Text != ""))
                {
                    strquery = "insert into clvAsmtTool(StdtCoverSheetId,TrgetBehav,Functions,AnalysisTool) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtTargetBehavior")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtFunction")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtAnalysisTools")).Text) + "')";
                    objData.Execute(strquery);
                }
            }
            //inserting Reinforcment  insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values()
            foreach (GridViewRow gdVr in gVAssmtReinfo.Rows)
            {
                if ((((TextBox)gdVr.FindControl("txtDate")).Text != "") || (((TextBox)gdVr.FindControl("txtToolUtilized")).Text != ""))
                {
                    strquery = "insert into clvReinfoSur(StdtCoverSheetId,ReinfoDate,ToolUtilizd) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtDate")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtToolUtilized")).Text) + "')";
                    objData.Execute(strquery);
                }

            }
            //inserting Rec Changes
            foreach (GridViewRow gdVr in gVRecChange.Rows)
            {
                if ((((TextBox)gdVr.FindControl("txtRecomd")).Text != "") || (((TextBox)gdVr.FindControl("txtTimeLine")).Text != "") || (((TextBox)gdVr.FindControl("txtPerRes")).Text != ""))
                {
                    strquery = "insert into clvRecChange(StdtCoverSheetId,Recomendation,TimeLine,PersonResponsible) values(" + int.Parse(hdFldCvid.Value) + ",'" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtRecomd")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtTimeLine")).Text) + "','" + clsGeneral.convertQuotes(((TextBox)gdVr.FindControl("txtPerRes")).Text) + "')";
                    objData.Execute(strquery);
                }
            }

            if (cvrsheetId > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Details Updated Successfully");
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Details Updation Failed");
            }
        }

        LoadDataList();
        LoadReport(date);
    }

    //protected void btnLoadDate_Click(object sender, EventArgs e)
    //{
    //    sess = (clsSession)Session["UserSession"];
    //    tdMsg.InnerHtml = "";
    //    if (btnLoadDate.Text == "Load Report")
    //    {
    //        clearDatas();
    //        PanelMain.Visible = true;
    //        drpSelectDate.Enabled = false;
    //        btnLoadDate.Text = "Select New Date";

    //        dtAssmtReinfo.Columns.Add("Date");
    //        dtAssmtReinfo.Columns.Add("ToolUtilized");

    //        dtRecChang.Columns.Add("Recommendation");
    //        dtRecChang.Columns.Add("Timeline");
    //        dtRecChang.Columns.Add("Person Responsible");

    //        dtAssmntTool.Columns.Add("TargetBehavior");
    //        dtAssmntTool.Columns.Add("Function");
    //        dtAssmntTool.Columns.Add("AnalysisTools");

    //        loadAssmtReinfo();
    //        loadRecChange();
    //        loadAssmntTool();
    //        objData = new clsData();
    //        // date for sected date
    //        if (drpSelectDate.SelectedItem.Text == DateTime.Now.ToString("MM'/'dd'/'yyyy"))
    //        {
    //            if (objData.IFExists("select ClinicalCvId from StdtClinicalCoverSheet where CAST( EndDate as date)= cast(GETDATE() as date) and SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + "") == false)
    //            {

    //                loadData();
    //                btnSave.Text = "Save";
    //            }
    //            else
    //            {
    //                loadData();
    //                loadDataToday();
    //                btnSave.Text = "Update";
    //            }
    //        }
    //        else
    //        {
    //            if (objData.IFExists("select ClinicalCvId from StdtClinicalCoverSheet where CAST( EndDate as date)= cast('" + drpSelectDate.SelectedItem.Text + "' as date) and SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + "") == true)
    //            {

    //                loadData();//this is to load behavior
    //                loadDataToday();
    //                btnSave.Text = "Update";
    //            }
    //        }
    //    }
    //    else
    //    {

    //        PanelMain.Visible = false;
    //        drpSelectDate.Enabled = true;
    //        btnLoadDate.Text = "Load Report";

    //    }

    //}


    //protected void btnTodayzReport_Click(object sender, EventArgs e)
    //{
    //    sess = (clsSession)Session["UserSession"];
    //    clearDatas();
    //    PanelMain.Visible = true;
    //    string date = "";

    //    if (ViewState["CurrentDate"] != null)
    //    {
    //        date = ViewState["CurrentDate"].ToString();
    //    }


    //  //  drpSelectDate.SelectedIndex = 0;
    //   // drpSelectDate.Enabled = false;

    //    dtAssmtReinfo.Columns.Add("Date");
    //    dtAssmtReinfo.Columns.Add("ToolUtilized");

    //    dtRecChang.Columns.Add("Recommendation");
    //    dtRecChang.Columns.Add("Timeline");
    //    dtRecChang.Columns.Add("Person Responsible");

    //    dtAssmntTool.Columns.Add("TargetBehavior");
    //    dtAssmntTool.Columns.Add("Function");
    //    dtAssmntTool.Columns.Add("AnalysisTools");

    //    loadAssmtReinfo();
    //    loadRecChange();
    //    loadAssmntTool();
    //    objData = new clsData();
    //    if (objData.IFExists("select ClinicalCvId from StdtClinicalCoverSheet where CAST( EndDate as date)= cast(GETDATE() as date) and SchoolId=" + sess.SchoolId + " and StudentId=" + sess.StudentId + "") == false)
    //    {

    //        DataLoad(date);
    //        btnSave.Text = "Save";
    //    }
    //    else
    //    {
    //        loadDataTodayTemp(date);

    //        DataLoad(date);
    //        btnSave.Text = "Update";
    //    }
    //}



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
                // col = columns[i].ToString().Trim();
                col = Server.HtmlDecode(Regex.Replace(columns[i].ToString().Trim(), "<(.|\n)*?>", ""));

                col = col.Replace("&", "&amp;");

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
            string newFileName = "ClvCovSheet" + PageNo;
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
            tdMsg.InnerHtml = clsGeneral.warningMsg(Ex.Message.ToString() + "Directory or File already Exit !");
            return "";
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
        tblAppnd = new string[1];

        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: AllInOne";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);

        string Path = "";
        string date = "";
        string NewPath = "";
        Dt = new System.Data.DataTable();
        clsClinicalCoverSheet objExport = new clsClinicalCoverSheet();
        try
        {
            if (ViewState["CurrentDate"] != null)
            {
                date = ViewState["CurrentDate"].ToString();
            }

            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";
            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true); ;
            }
            if (!Directory.Exists(Temp))
            {
                Directory.CreateDirectory(Temp);
            }
            
            string Temp2 = Server.MapPath("~\\StudentBinder") + "\\ClinicalMerge";
            if (!Directory.Exists(Temp2))
            {
                Directory.CreateDirectory(Temp2);
            }

            string Temp3 = Server.MapPath("~\\StudentBinder") + "\\Temp2\\";
            if (Directory.Exists(Temp3))
            {
                Directory.Delete(Temp3, true); ;
            }
            if (!Directory.Exists(Temp3))
            {
                Directory.CreateDirectory(Temp3);
            }
            string Pathheader = Temp3 + "\\ClinicalCoverSheet_Header.doc";
            string fileheder = Server.MapPath("~\\StudentBinder\\CsTemplatesTest\\ClinicalCoversheetHeader.docx");
            File.Copy(fileheder, Pathheader);
            Dt = objExport.getHeaderValz(sess.StudentId, sess.SchoolId, int.Parse(hdFldCvid.Value));
            if (Dt != null)
            {
                System.Data.DataColumn newProgram = new System.Data.DataColumn("Program", typeof(System.String));
                newProgram.DefaultValue = getHdrPrograms(int.Parse(hdFldCvid.Value));
                Dt.Columns.Add(newProgram);

                System.Data.DataColumn newLocation = new System.Data.DataColumn("Location", typeof(System.String));
                newLocation.DefaultValue = getHdrLocations(int.Parse(hdFldCvid.Value));
                Dt.Columns.Add(newLocation);
            }

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(Pathheader, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                string content = null;
                using (StreamReader reader = new StreamReader(
                mainPart.HeaderParts.First().GetStream()))
                {
                    content = reader.ReadToEnd();
                }
                System.Text.RegularExpressions.Regex expression = new System.Text.RegularExpressions.Regex("PlcStdName");
                content = expression.Replace(content, "" + Dt.Rows[0][0]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }

                System.Text.RegularExpressions.Regex expression1 = new System.Text.RegularExpressions.Regex("PlcLocation");
                content = expression1.Replace(content, "" + Dt.Rows[0][4]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                System.Text.RegularExpressions.Regex expression2 = new System.Text.RegularExpressions.Regex("PlcProgram");
                content = expression2.Replace(content, "" + Dt.Rows[0][3]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                System.Text.RegularExpressions.Regex expression3 = new System.Text.RegularExpressions.Regex("PlcIepYear");
                content = expression3.Replace(content, "" + Dt.Rows[0][1]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                System.Text.RegularExpressions.Regex expression4 = new System.Text.RegularExpressions.Regex("PlcPeriodOfAssmnt");
                content = expression4.Replace(content, "" + Dt.Rows[0][2]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.First().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                using (StreamReader reader = new StreamReader(
                mainPart.HeaderParts.Last().GetStream()))
                {
                    content = reader.ReadToEnd();
                }
                System.Text.RegularExpressions.Regex expression6 = new System.Text.RegularExpressions.Regex("PlcStdName");
                content = expression6.Replace(content, "" + Dt.Rows[0][0]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }

                System.Text.RegularExpressions.Regex expression7 = new System.Text.RegularExpressions.Regex("PlcLocation");
                content = expression7.Replace(content, "" + Dt.Rows[0][4]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                System.Text.RegularExpressions.Regex expression8 = new System.Text.RegularExpressions.Regex("PlcProgram");
                content = expression8.Replace(content, "" + Dt.Rows[0][3]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                System.Text.RegularExpressions.Regex expression9 = new System.Text.RegularExpressions.Regex("PlcIepYear");
                content = expression9.Replace(content, "" + Dt.Rows[0][1]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                System.Text.RegularExpressions.Regex expression10 = new System.Text.RegularExpressions.Regex("PlcPeriodOfAssmnt");
                content = expression10.Replace(content, "" + Dt.Rows[0][2]);
                using (StreamWriter writer = new StreamWriter(
                mainPart.HeaderParts.Last().GetStream(FileMode.Create)))
                {
                    writer.Write(content);
                }
                
                mainPart.Document.Save();
            }
            
            int pageCount = 0;
            if (sess.SchoolId == 1)
            {
                Path = Server.MapPath("~\\StudentBinder\\CsTemplatesTest\\ClinicalCoversheetNE.docx");
                NewPath = CopyTemplate(Path, pageCount.ToString());
            }
            else
            {
                Path = Server.MapPath("~\\StudentBinder\\CsTemplatesTest\\ClinicalCoversheetPA.docx");
                NewPath = CopyTemplate(Path, pageCount.ToString());
            }

            string stDate = "";
            string endDate = "";
            if (date != null)
            {
                stDate = date.Split('-')[0];
                endDate = date.Split('-')[1];
            }
            DateTime todayDate = DateTime.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime lastDayDate = DateTime.ParseExact(stDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            Dt = objData.ExecuteCoversheetBehavior(lastDayDate, todayDate, sess.SchoolId, sess.StudentId);
            if (Dt != null)
            {
                System.Data.DataTable dtbehSumr = new System.Data.DataTable();
                try
                {
                    int Flcvid = 0;
                    string query = "";
                    if (hdFldCvid.Value != null && hdFldCvid.Value != "")
                    {
                        Flcvid = int.Parse(hdFldCvid.Value);
                        if (sess.SchoolId == 1)
                        {
                            query = "select StudentId,Measurementid,BehIepObj,BehlvlPerf from clvBehsummary where stdtcoversheetid = " + Flcvid + "";
                        }
                        else
                        {
                            query = "select StudentId,Measurementid,BehIepObj,BehlvlPerf from clvBehsummary where stdtcoversheetid = " + Flcvid + "";
                        }

                        dtbehSumr = objData.ReturnDataTable(query, false);

                        if (dtbehSumr != null)
                        {
                            for (int i = 0; i < Dt.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtbehSumr.Rows.Count; j++)
                                {
                                    object dtM1 = Dt.Rows[i]["MeasurementId"];
                                    object dtbehSumrM1 = dtbehSumr.Rows[j]["MeasurementId"];

                                    int M1 = int.Parse(dtM1.ToString());
                                    int M2 = int.Parse(dtbehSumrM1.ToString());
                                    if (M1 == M2)
                                    {
                                        Dt.Rows[i]["IEPObj"] = dtbehSumr.Rows[j]["BehIepObj"];
                                        Dt.Rows[i]["BasPerlvl"] = dtbehSumr.Rows[j]["BehlvlPerf"];
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    exp.ToString();
                }
            }
            AppndTableBehav(NewPath, Dt);
            
            if (sess.SchoolId == 1)
            {
                CreateQuery("NE", "XMLCS\\CS2CreationPA.xml");
                Dt = objExport.SettingEventsandProgramChanges(sess.StudentId, sess.SchoolId, ViewState["CurrentDate"].ToString());
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        for (int i = 0; i < Dt.Columns.Count; i++)
                        {
                            columns[i] = Server.HtmlDecode(Regex.Replace(dr[columnsToAdd[i]].ToString(), "<(.|\n)*?>", ""));                           
                        }
                    }
                }

            }           
            else
            {
                CreateQuery("NE", "XMLCS\\CS2Creation.xml");
                Dt = objExport.SettingEventsandProgramChanges(sess.StudentId, sess.SchoolId, ViewState["CurrentDate"].ToString());
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        for (int i = 0; i < Dt.Columns.Count; i++)
                        {
                            //columns[i] = dr[columnsToAdd[i]].ToString();
                            columns[i] = Server.HtmlDecode(Regex.Replace(dr[columnsToAdd[i]].ToString(), "<(.|\n)*?>", ""));                           
                        }

                    }
                }
            }
            if (NewPath != "")
            {
                SearchAndReplace(NewPath);
            }


            Dt = objExport.getClinicalCvSheetAsmtTool(sess.StudentId, sess.SchoolId, int.Parse(hdFldCvid.Value));
            AppndTableAssmtTool(NewPath, Dt);


            Dt = objExport.getClinicalCvReinfo(sess.StudentId, sess.SchoolId, int.Parse(hdFldCvid.Value));
            AppndTableReinfo(NewPath, Dt);

           
            Dt = objExport.getClinicalCvRecChange(sess.StudentId, sess.SchoolId, int.Parse(hdFldCvid.Value));
            AppndTableRecChange(NewPath, Dt);

            Dt = objExport.getClinicalCvSheet(sess.StudentId, sess.SchoolId, int.Parse(hdFldCvid.Value));

            CreateQuery("NE", "XMLCS\\CS3Creation.xml");

            if (Dt.Rows.Count > 0)
            {
                int tblCount = 0;

                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < placeHolders.Length; i++)
                    {
                        if (columnsToAdd[i] != "TableApp")
                        {
                            columns[i] = dr[columnsToAdd[i]].ToString();
                            if (NewPath != "")
                            {
                                using (WordprocessingDocument theDoc = WordprocessingDocument.Open(NewPath, true))    ///same styles in clinical sheet should be in Export --jis
                                {
                                    if (columns[i] != "")
                                    {
                                        columns[i] = Server.HtmlDecode(Regex.Replace(columns[i].ToString().Trim(), "<(.|\n)*?>", ""));
                                        replaceWithTextsSingle(theDoc.MainDocumentPart, placeHolders[i], columns[i]);

                                    }
                                    else
                                    {
                                        replaceWithTextsSingle(theDoc.MainDocumentPart, placeHolders[i], "");
                                    }
                                }
                            }
                        }
                        else if (columnsToAdd[i] == "TableApp")
                        {
                            tblAppnd[tblCount] = placeHolders[i];
                        }
                    }


                }
                if (NewPath != "")
                {

                    SearchAndReplace(NewPath);
                }
               
            }
             
            CreateQuery("NE", "XMLCS\\CS4Creation.xml");
            Dt = objExport.getClinicalCvSheet(sess.StudentId, sess.SchoolId, int.Parse(hdFldCvid.Value));
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow dr in Dt.Rows)
                {
                    for (int i = 0; i < placeHolders.Length; i++)
                    {
                        columns[i] = dr[columnsToAdd[i]].ToString();

                    }


                    if (NewPath != "")
                    {
                        SearchAndReplace(NewPath);
                    }


                }

            }
            pageCount++;

            string iepDoneFlg = MergeFiles();

            if (iepDoneFlg == "")
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Clinical Coversheet Creation Failed !");
            }
            else
            {
                tdMsg.InnerHtml = "";
                tdMsgExport.InnerHtml = clsGeneral.sucessMsg("Clinical Coversheet Sucessfully Created ");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myfn", "HideWait();", true);
                //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "HideWait();", true);
                //string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#PopDownload').css('top', '5%'); $('#PopDownload').show(); }); $('#close_x').click(function () { $('#PopDownload').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
                //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);
                //   btnIEPExport.Text = "Download";
                //   BtnCanel.Visible = true;
            }

        }
        catch (Exception eX)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
        }
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
                        ParagraphProperties paraProp = new ParagraphProperties();
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
            para.RemoveAllChildren<Run>();
        }
    }



    public void convertDrRowToOpenXmlRow(System.Data.DataRow dRow)
    {

    }


    // public
    public void replaceWithHtml(string fileName, string replace, DocumentFormat.OpenXml.Wordprocessing.Table tableOpenXml)
    {

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);
            Body body = mainPart.Document.Body;

            //     converter.ConsiderDivAsParagraph = false;

            //     SpacingBetweenLines spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Auto, Before = "0", After = "0" };
            ParagraphProperties paragraphProperties = new ParagraphProperties();

            //paragraphProperties.Append(spacing);

            paragraphProperties.RemoveAllChildren<ParagraphStyleId>();
            paragraphProperties.RemoveAllChildren<SpacingBetweenLines>();

            try
            {
                var placeholder = mainPart.Document.Body
                  .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                  .Where(t => t.Text.Contains(replace)).First();

                // var paragraphs = converter.Parse(replaceTest);




                //   for (int i = 0; i < paragraphs.Count; i++)
                //  {
                var parent = placeholder.Parent;
                //      paragraphs[i].Append(paragraphProperties);
                parent.ReplaceChild(tableOpenXml, placeholder);
                //  }
                mainPart.Document.Save();
            }
            catch (Exception eX)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
            }


        }
    }




    public string MergeFiles()
    {
        try
        {
            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";
            string Temp1 = Server.MapPath("~\\StudentBinder") + "\\ClinicalMerge";
            const string DOC_URL = "/word/document.xml";
            string FolderName = "\\ClinicalCoverSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}";
            FolderName = string.Format(FolderName, DateTime.Now);
            //string path = Server.MapPath("~\\Administration") + "\\IEPMerged";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            string OUTPUT_FILE = Temp1 + "\\ClinicalCoverSheet_" + sess.StudentName + "_{0:ddMMyy}-{0:HHmmss}.doc";
            string FIRST_PAGE = Server.MapPath("~\\StudentBinder\\Temp2\\ClinicalCoverSheet_Header.doc");

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

    public Header GetHeader()
    {
        Header h = new Header();
        DocumentFormat.OpenXml.Wordprocessing.Paragraph p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
        Run r = new Run();
        Text t = new Text();
        t.Text = "This is the header.";
        r.Append(t);
        p.Append(r);
        h.Append(p);
        return h;
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            btnSave_Click(sender, e);
            bool iepDateCheckStatus = iepdatecheck();
            if (iepDateCheckStatus == true)
            {
                tdMsg.InnerHtml = "";
                AllInOne();
            }
            //tdMsg.InnerHtml = "";
            //AllInOne();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    //************** Table Appending functions **************

    public void AppndTableBehav(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                if (tablecounter == 0)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();


                        //==============

                        //==- SharpDecrease [9/14]
                        string xVal9 = dr[9].ToString(); 
                        Run run9 = new Run();
                        if (xVal9.ToString().Contains("N/A"))
						{
                            xVal9 = "NA";
                            run9.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run9.AppendChild(new Text(xVal9));
                        } 
						else { run9.AppendChild(new Text(xVal9)); }


                        string xVal14 = dr[14].ToString();
                        Run run14 = new Run();
                        if (xVal14.ToString().Contains("N/A"))
						{
                            xVal14 = "NA";
                            run14.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run14.AppendChild(new Text(xVal14));
                        } 
						else { run14.AppendChild(new Text(xVal14)); }

                        //==- SlightDecrease [10/15]
                        string xVal10 = dr[10].ToString();
                        Run run10 = new Run();
                        if (xVal10.ToString().Contains("N/A")) 
						{
                            xVal10 = "NA";
                            run10.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run10.AppendChild(new Text(xVal10));
                        }
                        else { run10.AppendChild(new Text(xVal10)); }

                        string xVal15 = dr[15].ToString();
                        Run run15 = new Run();
                        if (xVal15.ToString().Contains("N/A")) 
						{
                            xVal15 = "NA";
                            run15.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run15.AppendChild(new Text(xVal15));
                        }
                        else { run15.AppendChild(new Text(xVal15)); }

                        //==- Stable [11/16]
                        string xVal11 = dr[11].ToString();
                        Run run11 = new Run();
                        if (xVal11.ToString().Contains("N/A"))
                        {
                            xVal11 = "NA";
                            run11.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run11.AppendChild(new Text(xVal11));
                        }
                        else { run11.AppendChild(new Text(xVal11)); }

                        string xVal16 = dr[16].ToString();
                        Run run16 = new Run();
                        if (xVal16.ToString().Contains("N/A"))
                        {
                            xVal16 = "NA";
                            run16.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run16.AppendChild(new Text(xVal16));
                        }
                        else { run16.AppendChild(new Text(xVal16)); }

                        //==- SlightIncrease [12/17]
                        string xVal12 = dr[12].ToString();
                        Run run12 = new Run();
                        if (xVal12.ToString().Contains("N/A"))
						{
                            xVal12 = "NA";
                            run12.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run12.AppendChild(new Text(xVal12));
                        }
                        else { run12.AppendChild(new Text(xVal12)); }

                        string xVal17 = dr[17].ToString();
                        Run run17 = new Run();
                        if (xVal17.ToString().Contains("N/A"))
						{
                            xVal17 = "NA";
                            run17.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run17.AppendChild(new Text(xVal17));
                        }
                        else { run17.AppendChild(new Text(xVal17)); }

                        //==- SharpIncrease [13/18]
                        string xVal13 = dr[13].ToString();
                        Run run13 = new Run();
                        if (xVal13.ToString().Contains("N/A"))
                        {
                            xVal13 = "NA";
                            run13.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run13.AppendChild(new Text(xVal13));
                        }
                        else { run13.AppendChild(new Text(xVal13)); }

                        string xVal18 = dr[18].ToString();
                        Run run18 = new Run();
                        if (xVal18.ToString().Contains("N/A"))
                        {
                            xVal18 = "NA";
                            run18.AppendChild(new RunProperties(new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = "8pt" }));
                            run18.AppendChild(new Text(xVal18));
                        }
                        else { run18.AppendChild(new Text(xVal18)); }

                        //tblCell4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new RunProperties(fontSize)));

                        //=======

                       


                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[2].ToString()))));
                        tblCell1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Frequency"))));
                        tblCell1.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text("Duration"))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                        tblCell2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run9));
                        tblCell2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run14));
                        //tblCell2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[9].ToString()))));
                        //tblCell2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[14].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                        tblCell3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run10));
                        tblCell3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run15));
                        //tblCell3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[10].ToString()))));
                        //tblCell3.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[15].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell4
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                        tblCell4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run11));
                        tblCell4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run16));
                        //tblCell4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[11].ToString()))));
                        //tblCell4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[16].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell5
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                        tblCell5.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run12));
                        tblCell5.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run17));
                        //tblCell5.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[12].ToString()))));
                        //tblCell5.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[17].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell6
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                        tblCell6.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run13));
                        tblCell6.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run18));
                        //tblCell6.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[13].ToString()))));
                        //tblCell6.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[18].ToString()))));
                        //DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell7
                        //    = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                        //tblCell7.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[23].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell7
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[23].ToString()))));

                        tblCell7.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[5].ToString()))));
                        tblCell7.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(dr[8].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell8
                           = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[21].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell9
                            = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[20].ToString()))));

                        string Teststr = dr[22].ToString();
                        string IOADate1 = "";
                        string IOAPoint1 = "";
                        string IOADate2 = "";
                        string IOAPoint2 = "";

                        Run run1 = new Run();
                        Run run2 = new Run();
                        Run run3 = new Run();
                        Run run4 = new Run();

                        run1.AppendChild(new RunProperties(new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single }));
                        run3.AppendChild(new RunProperties(new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single }));
                        //run1.AppendChild(new RunProperties(new Bold(), new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single }));
                        //run3.AppendChild(new RunProperties(new Bold(), new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single }));

                        if (Teststr.ToString().Split('-', '_').Length > 0)
                        {
                            if (Teststr.ToString().Split('-', '_') != null)
                            {
                                if (Teststr.ToString().Split('-', '_').Length >= 1) { IOADate1 = Teststr.ToString().Split('-', '_')[0] + System.Environment.NewLine + "\n"; run1.AppendChild(new Text(IOADate1)); } else { IOADate1 = ""; run1.AppendChild(new Text("")); }
                                if (Teststr.ToString().Split('-', '_').Length >= 2) { IOAPoint1 = Teststr.ToString().Split('-', '_')[1] + System.Environment.NewLine + "\n\n\n\n"; run2.AppendChild(new Text(IOAPoint1)); run2.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break()); } else { IOAPoint1 = ""; run2.AppendChild(new Text("")); }
                                if (Teststr.ToString().Split('-', '_').Length >= 3) { IOADate2 = Teststr.ToString().Split('-', '_')[2] + System.Environment.NewLine + "\n"; run3.AppendChild(new Text(IOADate2)); } else { IOADate2 = ""; run3.AppendChild(new Text("")); }
                                if (Teststr.ToString().Split('-', '_').Length >= 4) { IOAPoint2 = Teststr.ToString().Split('-', '_')[3] + System.Environment.NewLine + "\n"; run4.AppendChild(new Text(IOAPoint2)); run4.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break()); } else { IOAPoint2 = ""; run4.AppendChild(new Text("")); }
                            }
                        }
                        else if (Teststr.ToString().Split('-', '_').Length == 0)
                        {
                            IOADate1 = "";
                            IOAPoint1 = "";
                            IOADate2 = "";
                            IOAPoint2 = "";
                        }

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell10
                        = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new DocumentFormat.OpenXml.Wordprocessing.Text(""))));
                        tblCell10.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run1));
                        tblCell10.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run2));
                        tblCell10.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run3));
                        tblCell10.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(run4));

                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);
                        trXml.Append(tblCell4);
                        trXml.Append(tblCell5);
                        trXml.Append(tblCell6);
                        trXml.Append(tblCell7);
                        trXml.Append(tblCell8);
                        trXml.Append(tblCell9);
                        trXml.Append(tblCell10);
                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tablecounter++;
            }

            mainPart.Document.Save();

        }

    }

    //public void AppndTableEvents(string fileName, System.Data.DataTable XmlAppDatatable)
    //{
    //    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
    //    {
    //        MainDocumentPart mainPart = wordDoc.MainDocumentPart;
    //        HtmlConverter converter = new HtmlConverter(mainPart);

    //        Body bod = mainPart.Document.Body;
    //        //Body bod = mainPart.MainDocumentPart.Document.Body;
    //        int tablecounter = 0;
    //        foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
    //        {

    //            //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
    //            //{
    //            //    //t.Append(tr);
    //            //    t.Append(tr);
    //            //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
    //            //}
    //            if (tablecounter == 2)
    //            {
    //                foreach (DataRow dr in XmlAppDatatable.Rows)
    //                {
    //                    DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

    //                    DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr.ToString()))));


    //                    trXml.Append(tblCell1);

    //                    t.Append(trXml);
    //                    //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
    //                }
    //            }
    //            tablecounter++;
    //        }

    //        mainPart.Document.Save();

    //    }

    //}

    public void AppndTableAssmtTool(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);
            
            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tablecounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                if (tablecounter == 2)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[2].ToString()))));


                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);

                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tablecounter++;
            }

            mainPart.Document.Save();

        }

    }

    public void AppndTableReinfo(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tableCount = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}
                if (tableCount == 3)
                {
                    foreach (DataRow dr in XmlAppDatatable.Rows)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));


                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);

                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tableCount++;
            }

            mainPart.Document.Save();

        }

    }

    public void AppndTableRecChange(string fileName, System.Data.DataTable XmlAppDatatable)
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            HtmlConverter converter = new HtmlConverter(mainPart);

            Body bod = mainPart.Document.Body;
            //Body bod = mainPart.MainDocumentPart.Document.Body;
            int tableCounter = 0;
            foreach (DocumentFormat.OpenXml.Wordprocessing.Table t in bod.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())
            {

                //foreach (DocumentFormat.OpenXml.Wordprocessing.TableRow tr in tableXmlCheck.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
                //{
                //    //t.Append(tr);
                //    t.Append(tr);
                //   // t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("test"))))));
                //}

                foreach (DataRow dr in XmlAppDatatable.Rows)
                {
                    if (tableCounter == 6)
                    {
                        DocumentFormat.OpenXml.Wordprocessing.TableRow trXml = new DocumentFormat.OpenXml.Wordprocessing.TableRow();

                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell1 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell2 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[1].ToString()))));
                        DocumentFormat.OpenXml.Wordprocessing.TableCell tblCell3 = new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[2].ToString()))));

                        trXml.Append(tblCell1);
                        trXml.Append(tblCell2);
                        trXml.Append(tblCell3);

                        t.Append(trXml);
                        //t.Append(new DocumentFormat.OpenXml.Wordprocessing.TableRow(new DocumentFormat.OpenXml.Wordprocessing.TableCell(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text(dr[0].ToString()))))));
                    }
                }
                tableCounter++;
            }

            mainPart.Document.Save();

        }

    }
    //*****************************************************


    //*****CreateDoc******
    /*
    public void CreateDocument()
    {
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Create("test.docx", WordprocessingDocumentType.Document))
        {
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            Document d = new Document();
            Body b = new Body();
            Table tbl = new Table();
            TableProperties tblPr = new TableProperties();
            TableCellMarginDefault tblCellMar = new TableCellMarginDefault();
            tblCellMar.TopMargin = new TopMargin { Width = 0, Type = TableWidthUnitValues.Dxa };
            tblCellMar.RightMargin = new RightMargin { Width = 120, Type = TableWidthUnitValues.Dxa };
            tblCellMar.BottomMargin = new BottomMargin { Width = 0, Type = TableWidthUnitValues.Dxa };
            tblCellMar.LeftMargin = new LeftMargin { Width = 120, Type = TableWidthUnitValues.Dxa };
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            Paragraph p = new Paragraph();
            Run r = new Run();
            Text t = new Text();
            t.Text = "This is text in a table cell.";
            r.Append(t);
            p.Append(r);
            tc.Append(p);
            tr.Append(tc);
            tblPr.Append(tblCellMar);
            tbl.Append(tblPr);
            tbl.Append(tr);
            b.Append(tbl);
            d.Append(b);

            HeaderPart hp = mainPart.AddNewPart<HeaderPart>();
            string relId = mainPart.GetIdOfPart(hp);
            SectionProperties sectPr = new SectionProperties();
            HeaderReference headerReference = new HeaderReference();
            headerReference.Id = relId;
            sectPr.Append(headerReference);

            d.Append(sectPr);
            mainPart.Document = d;
            mainPart.Document.Save();
            hp.Header = GetHeader();
            hp.Header.Save();

            wordDoc.Close();
        }
    }*/
    //********************

    //************* Test Mearge ************
    public void TestMergeFiles()
    {
        try
        {
            string Temp = Server.MapPath("~\\StudentBinder") + "\\Temp2\\";
            const string DOC_URL = "/word/document.xml";

            string FolderName = "\\AcademicSheet_{0:ddMMyy}-{0:HHmmss}";
            FolderName = string.Format(FolderName, DateTime.Now);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + FolderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string OUTPUT_FILE = path + "\\AcademicSheet_{0:ddMMyy}-{0:HHmmss}.docx";
            string FIRST_PAGE = Server.MapPath("~\\StudentBinder\\ASTemplates\\Dummy.docx");

            string fileName = string.Format(OUTPUT_FILE, DateTime.Now);
            // File.Copy(FIRST_PAGE, fileName);
            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            {
                //MainDocumentPart mainPart = wordDoc.MainDocumentPart;


                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                DocumentFormat.OpenXml.Wordprocessing.Document d = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body b = new Body();
                DocumentFormat.OpenXml.Wordprocessing.Table tbl = new DocumentFormat.OpenXml.Wordprocessing.Table();
                DocumentFormat.OpenXml.Wordprocessing.TableProperties tblPr = new DocumentFormat.OpenXml.Wordprocessing.TableProperties();
                DocumentFormat.OpenXml.Wordprocessing.TableCellMarginDefault tblCellMar = new DocumentFormat.OpenXml.Wordprocessing.TableCellMarginDefault();

                DocumentFormat.OpenXml.Wordprocessing.TableRow tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                DocumentFormat.OpenXml.Wordprocessing.TableCell tc = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                DocumentFormat.OpenXml.Wordprocessing.Paragraph p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
                Run r = new Run();
                Text t = new Text();


                t.Text = "This is text in a table cell.";

                // t.Text = docText;

                // r.Append(t);
                p.Append(r);
                tc.Append(p);
                tr.Append(tc);
                tblPr.Append(tblCellMar);
                tbl.Append(tblPr);
                tbl.Append(tr);
                b.Append(tbl);
                d.Append(b);

                HeaderPart hp = mainPart.AddNewPart<HeaderPart>();
                string relId = mainPart.GetIdOfPart(hp);
                SectionProperties sectPr = new SectionProperties();
                HeaderReference headerReference = new HeaderReference();
                headerReference.Id = relId;
                sectPr.Append(headerReference);

                d.Append(sectPr);
                mainPart.Document = d;
                mainPart.Document.Save();
                hp.Header = GetHeader();
                hp.Header.Save();

                wordDoc.Close();
            }

            string docText = "";
            int i = 0;
            string[] filePaths = Directory.GetFiles(Temp);
            foreach (string filePath in filePaths)
            {
                // using (WordprocessingDocument wordDoc1 = WordprocessingDocument.Open(fileName, false))
                using (WordprocessingDocument wordDoc2 = WordprocessingDocument.Open(filePath, true))
                {
                    //  XElement tempBody = XElement.Parse(tempDocument.MainDocumentPart.Document.Body.OuterXml);
                    //   XmlElement tempbody=wordDoc2.MainDocumentPart.CustomXmlParts<>;
                    var main = wordDoc2.MainDocumentPart;

                    //replace the fields in the main document
                    // main.Document.InnerXml.

                    using (StreamReader sr = new StreamReader(wordDoc2.MainDocumentPart.GetStream()))
                    {

                        docText += sr.ReadToEnd();
                    }




                }
                i++;
            }
            using (WordprocessingDocument wordDoc3 = WordprocessingDocument.Open(fileName, true))
            {

                using (StreamWriter sw = new StreamWriter(wordDoc3.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
            //MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
            //DocumentFormat.OpenXml.Wordprocessing.Document d = new DocumentFormat.OpenXml.Wordprocessing.Document();
            //Body b = new Body();
            //HeaderPart hp = mainPart.AddNewPart<HeaderPart>();
            //string relId = mainPart.GetIdOfPart(hp);
            //SectionProperties sectPr = new SectionProperties();
            //HeaderReference headerReference = new HeaderReference();
            //headerReference.Id = relId;
            //sectPr.Append(headerReference);


            //d.Append(b);

            //d.Append(sectPr);
            //mainPart.Document = d;
            //mainPart.Document.Save();
            //hp.Header = GetHeader();
            //hp.Header.Save();

            //wordDoc.Close();

            //   public static void Merge(string document1, string document2, string mergedDocPath)
            //{
            //    var combinedSourceList = new List<Source>();

            //    using (var document1XmlDoc = WordprocessingDocument.Open(document1, false))
            //    using (var document2OpenXmlDoc = WordprocessingDocument.Open(document2, false))
            //    {
            //        var analysisSource = new Source(document1XmlDoc, false);
            //        var detailsSource = new Source(document2OpenXmlDoc, true);

            //        combinedSourceList.Add(analysisSource);
            //        combinedSourceList.Add(detailsSource);

            //        // Build and merge the source documents
            //        DocumentBuilder.BuildDocument(combinedSourceList, mergedDocPath);
            //    }
            //}


            //string[] filePaths = Directory.GetFiles(Temp);


            //foreach (string filePath in filePaths)
            //{
            //    using (WordprocessingDocument wordDoc1 = WordprocessingDocument.Open(fileName, false))
            //    using (WordprocessingDocument wordDoc2 = WordprocessingDocument.Open(filePath, true))
            //    {
            //        using (StreamReader sr = new StreamReader(wordDoc2.MainDocumentPart.GetStream()))
            //        {
            //            docText = sr.ReadToEnd();
            //        }
            //       // ThemePart themePart1 = wordDoc1.MainDocumentPart.ThemePart;
            //      //  ThemePart themePart2 = wordDoc2.MainDocumentPart.ThemePart;

            //        using (StreamReader streamReader = new StreamReader(wordDoc1.MainDocumentPart.GetStream()))
            //        using (StreamWriter streamWriter = new StreamWriter(wordDoc2.MainDocumentPart.GetStream(FileMode.Create)))
            //        {
            //            streamWriter.Write(streamReader.ReadToEnd());
            //        }
            //    }
            //}




            /*  using (Package package = Package.Open(fileName, FileMode.Open, FileAccess.ReadWrite))
              {
                  PackagePart docPart = package.GetPart(new Uri(DOC_URL, UriKind.Relative));

                  XmlDocument document = new XmlDocument();



                  document.Load(docPart.GetStream());

                  XmlNamespaceManager nsm = new XmlNamespaceManager(document.NameTable);
                  nsm.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");

                  XmlNode body = document.SelectSingleNode("/w:document/w:body", nsm);            



              }
              */


            if (Directory.Exists(Temp))
            {
                Directory.Delete(Temp, true);
            }

        }

        catch (Exception Ex)
        {

        }
    }
    //**************************************
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        downloadfile();
    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        tdMsgExport.InnerHtml = "";
        //string path = Server.MapPath("~\\StudentBinder") + "\\ACMerge";
        //Array.ForEach(Directory.GetFiles(path), File.Delete);
        //string Temp = Server.MapPath("~\\StudentBinder") + "\\ClinicalMerge\\";
        //if (Directory.Exists(Temp))
        //{
        //    Directory.Delete(Temp, true);
        //}
        string FileName = ViewState["FileName"].ToString();
        if (System.IO.File.Exists(FileName))
        {
            File.Delete(FileName);
        }
        string Temp1 = Server.MapPath("~\\StudentBinder") + "\\Temp1\\";
        if (Directory.Exists(Temp1))
        {
            Directory.Delete(Temp1, true);
        }
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
        //ViewState["FileName"] = "";
    }
    protected void dlDateList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        objData = new clsData();
        string date = "";

        try
        {
            date = e.CommandArgument.ToString();
            ViewState["CurrentDate"] = date;
            LoadReport(date);            // load the selected date report.

            setWritePermissions();           //check permission to read/write

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label catId = e.Item.FindControl("lblCategory") as Label;
        DataList List2 = e.Item.FindControl("dlDateList") as DataList;

        DataTable tbl2 = new DataTable();
        //tbl2 = GetCategoryItemBasedonCategoryID(catId.Text); //         database call function for fetch data to bind the second datalist
        //string qry = "select CONVERT(varchar(10),StartDate,101)+'-'+CONVERT(varchar(10),EndDate,101) as EDate from StdtClinicalCoverSheet WHERE SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " AND year(EndDate)=" + catId.Text.Substring(catId.Text.Length - 4) + " order by EndDate desc";// +" group by year(EndDate) desc";
        tbl2 = objData.ReturnDataTable("select CONVERT(varchar(10),StartDate,101)+'-'+CONVERT(varchar(10),EndDate,101) as EDate from StdtClinicalCoverSheet WHERE SchoolId=" + sess.SchoolId + " AND StudentId=" + sess.StudentId + " AND year(EndDate)=" + catId.Text.Substring(catId.Text.Length - 4) + " order by EndDate desc", false);//+ " group by year(EndDate) desc", false);
        List2.DataSource = tbl2;
        List2.DataBind();
    }
    protected void btnGenNewClinicalSheet_Click(object sender, EventArgs e)  //New clinical sheet generator 
    {
        txtEdate.Text = DateTime.Now.Date.ToString("MM'/'dd'/'yyyy");
        DateTime Edate = DateTime.Now.Date.AddDays(-90);
        txtSdate.Text = Edate.Date.ToString("MM'/'dd'/'yyyy");
        string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '5%'); $('#dialog').show(); }); $('#CancalGen').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), popup, true);

    }
    protected void btnGenDelete_Click(object sender, EventArgs e)  
    {
        string strquery = "";
        objData = new clsData();
        if (hdFldCvid.Value != null || hdFldCvid.Value != "")
        {
            strquery = "delete from StdtClinicalCoverSheet where ClinicalCvId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvBehsummary where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvAsmtTool where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvRecChange where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
            strquery = " delete from clvReinfoSur where StdtCoverSheetId=" + int.Parse(hdFldCvid.Value);
            objData.Execute(strquery);
        }
        PanelMain.Visible = false;
        btnExport.Visible = false;
        btnGenDelete.Visible = false;    
        LoadDataList();
        tdMsgDel.InnerHtml = "<span style='color: green; font-family:times new roman;margin-left:20px;font-size:12px;'>Deleted Successfully...</span>";

    }
    protected void btnGenCLS_Click(object sender, EventArgs e)
    {
        if (validate() == true)
        {
            FillReportCurrent();

            FillGraphData();
            LoadDataList();
        }
    }
    private bool validate()
    {
        objData = new clsData();
        bool result = true;
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        Pagepath = "ClinicalSheetNew: validate";
        sess = clsGeneral.sessioncheck(curesesid, preid, preuser, sess, Prevsess, SessStudentid, Sessstname, Pagepath);


        if (txtSdate.Text == "")
        {
            result = false;
            tdMessage.InnerHtml = clsGeneral.warningMsg("Please Select  Start Date");
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


    public static string iepdatesupdate(string stdids, string iepstdate, string iependate)
    {
        string status = "";
        try
        {
            int stid = Convert.ToInt32(stdids);
            string iepstdatstr = iepstdate;
            string iependatstr = iependate;
            if (stid > 0 && iepstdatstr != "" && iependatstr != "")
            {
                clsData objData = null;
                objData = new clsData();
                string updateIEpdates = "Update BehaviourDetails set BehaviorIEPSDate='" + iepstdatstr + "',BehaviorIEPEDate='" + iependatstr + "' where StudentId=" + stid;
                objData.Execute(updateIEpdates);
                status = "Success";
            }
        }
        catch (Exception exp)
        {
            exp.ToString();
        }
        return status;
    }

    public static string iepobj_behlvlupdate(string stdids, string mids, string behlvlstr, string iepobjstr)
    {
        string status = "";
        try
        {
            int stid = Convert.ToInt32(stdids);
            int msrid = Convert.ToInt32(mids);
            string behstr = behlvlstr;
            string iepstr = iepobjstr;
            if (msrid > 0 && (behstr != "" || iepstr != ""))
            {
                clsData objData = null;
                objData = new clsData();
                string strquery = "UPDATE BehaviourDetails SET BehaviorBasPerfLvl='" + clsGeneral.convertQuotes(behstr.ToString()) + "',BehaviorIEPObjctve='" + clsGeneral.convertQuotes(iepstr.ToString()) + "' WHERE MeasurementId =" + msrid + " AND StudentId=" + stid;
                int cvrsheetId = objData.Execute(strquery);
                status = "Success";
            }
            else
            {
                status = "Failed";
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return status;
    }

    private bool iepdatecheck()
    {
        bool status;
        status = true;
        try
        {
            foreach (GridViewRow gdIepdates in GridView2.Rows)
            {
                if ((((TextBox)gdIepdates.FindControl("txtDate1")).Text != "") && (((TextBox)gdIepdates.FindControl("txtDate2")).Text != ""))
                {
                    string iepstdate = ((TextBox)gdIepdates.FindControl("txtDate1")).Text;
                    string iependate = ((TextBox)gdIepdates.FindControl("txtDate2")).Text;
                    string iepstuddid = ((HiddenField)gdIepdates.FindControl("iepstdid")).Value;

                    if (iepstdate == "" || iependate == null)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter the IEP Start Date");
                        status = false;
                    }
                    else if (iepstdate == "" || iependate == null)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter the IEP End Date");
                        status = false;
                    }
                    if (iepstdate != "" && iependate != "")
                    {
                        DateTime dtiepst = new DateTime();
                        DateTime dtieped = new DateTime();
                        dtiepst = DateTime.ParseExact(iepstdate.Trim().Replace("/", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        dtieped = DateTime.ParseExact(iependate.Trim().Replace("/", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        if (dtiepst > dtieped)
                        {
                            tdMsg.InnerHtml = clsGeneral.warningMsg("Start date is must before the End date");
                            status = false;
                        }
                    }
                }
                else if ((((TextBox)gdIepdates.FindControl("txtDate1")).Text == "") || (((TextBox)gdIepdates.FindControl("txtDate2")).Text == ""))
                {
                    string iepstdate = ((TextBox)gdIepdates.FindControl("txtDate1")).Text;
                    string iependate = ((TextBox)gdIepdates.FindControl("txtDate2")).Text;
                    string iepstuddid = ((HiddenField)gdIepdates.FindControl("iepstdid")).Value;

                    if (iepstdate == "" || iependate == null)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter the IEP Start Date");
                        status = false;
                    }
                    else if (iependate == "" || iependate == null)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter the IEP End Date");
                        status = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            status = false;
            ex.ToString();
        }
        return status;
    }

    public void getheadergenerate(int clsid, int schlid, int stdid, string asmdate)
    {
        System.Data.DataTable dtHdr = new System.Data.DataTable();

        string stDate = "";
        string endDate = "";
        if (asmdate != null)
        {
            stDate = asmdate.Split('-')[0];
            endDate = asmdate.Split('-')[1];
        }

        string getLocations = objData.FetchValue("SELECT STUFF(( SELECT ','+CAST(Location AS VARCHAR(500)) FROM Placement WHERE StudentPersonalId = " + sess.StudentId + " AND EndDate IS NULL AND Status > 0 FOR XML PATH('')), 1, 1, '')").ToString();

        if (getLocations != "")
        {
            string query = "";
            if (sess.SchoolId == 1)
            {
                query = "select Student.StudentPersonalId as StdtId," +
                        "Student.StudentFname+' '+Student.StudentLname as StdName," +
                        "(SELECT ClassName FROM Class WHERE ClassId = " + clsid + ") Location_old," +
                        "(SELECT (SELECT STUFF(( SELECT ','+ClassName FROM Class WHERE ClassId IN (" + getLocations + ") FOR XML PATH('')), 1, 1, ''))) as Location," +
                        "(SELECT TOP 1 convert(varchar,FORMAT (ClinicalBehIEPSDate,'MM/dd/yyyy'), 1) FROM StdtClinicalCoverSheet WHERE StudentId = Student.StudentPersonalId AND ClinicalBehIEPSDate IS NOT NULL order by EndDate desc) AS IepStDate," +
                        "(SELECT TOP 1 convert(varchar,FORMAT (ClinicalBehIEPEDate,'MM/dd/yyyy'), 1) FROM StdtClinicalCoverSheet WHERE StudentId = Student.StudentPersonalId AND ClinicalBehIEPEDate IS NOT NULL order by EndDate desc) AS IepEnDate," +
                        "(SELECT TOP 1 Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId IN (SELECT Department FROM Placement WHERE StudentPersonalId = Student.StudentPersonalId AND Location = " + clsid + " AND EndDate IS NULL AND STATUS > 0)) Program_old," +
                        "(SELECT (SELECT STUFF(( SELECT ','+Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId IN (SELECT Department FROM Placement WHERE StudentPersonalId = Student.StudentPersonalId AND Location IN(" + getLocations + ") AND EndDate IS NULL AND STATUS > 0) FOR XML PATH('')), 1, 1, ''))) AS Program," +
                        "(Select TOP 1 CONVERT(varchar,[EffStartDate],101)+'-'+CONVERT(varchar,[EffEndDate],101) As IEPDATE FROM StdtIEP WHERE StudentId = Student.StudentPersonalId and StatusId IN (" +
                        "(SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'In Progress')," +
                        "(SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'Approved')," +
                        "(SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'Pending Approval')) ORDER BY StdtIEPId DESC) AS IepYear," +
                        "(SELECT convert(varchar,AsmntYearStartDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode IN (SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A')) as Prdstdate," +
                        "(SELECT convert(varchar,AsmntYearEndDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode IN (SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A')) as Prdendate " +
                        "from Student " +
                        "where Student.StudentId = " + stdid + " and Student.SchoolId = " + schlid + "";
            }
            else
            {
                query = "select Student.StudentPersonalId as StdtId," +
                        "Student.StudentFname+' '+Student.StudentLname as StdName," +
                        "(SELECT ClassName FROM Class WHERE ClassId = " + clsid + ") Location_old," +
                        "(SELECT (SELECT STUFF(( SELECT ','+ClassName FROM Class WHERE ClassId IN (" + getLocations + ") FOR XML PATH('')), 1, 1, ''))) as Location," +
                        "(SELECT TOP 1 convert(varchar,FORMAT (ClinicalBehIEPSDate,'MM/dd/yyyy'), 1) FROM StdtClinicalCoverSheet WHERE StudentId = Student.StudentPersonalId AND ClinicalBehIEPSDate IS NOT NULL order by EndDate desc) AS IepStDate," +
                        "(SELECT TOP 1 convert(varchar,FORMAT (ClinicalBehIEPEDate,'MM/dd/yyyy'), 1) FROM StdtClinicalCoverSheet WHERE StudentId = Student.StudentPersonalId AND ClinicalBehIEPEDate IS NOT NULL order by EndDate desc) AS IepEnDate," +
                        "(SELECT TOP 1 Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId IN (SELECT Department FROM Placement WHERE StudentPersonalId = Student.StudentPersonalId AND Location = " + clsid + " AND EndDate IS NULL AND STATUS > 0)) Program_old," +
                        "(SELECT (SELECT STUFF(( SELECT ','+Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId IN (SELECT Department FROM Placement WHERE StudentPersonalId = Student.StudentPersonalId AND Location IN(" + getLocations + ") AND EndDate IS NULL AND STATUS > 0) FOR XML PATH('')), 1, 1, ''))) AS Program," +
                        "(Select TOP 1 CONVERT(varchar,[EffStartDate],101)+'-'+CONVERT(varchar,[EffEndDate],101) As IEPDATE FROM StdtIEP WHERE StudentId = Student.StudentPersonalId and StatusId IN (" +
                        "(SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'In Progress')," +
                        "(SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'Approved')," +
                        "(SELECT LookupId FROM LookUp WHERE LookupType = 'IEP Status' AND LookupName = 'Pending Approval')) ORDER BY StdtIEPId DESC) AS IepYear," +
                        "(SELECT convert(varchar,AsmntYearStartDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode IN (SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A')) as Prdstdate," +
                        "(SELECT convert(varchar,AsmntYearEndDt, 1) FROM AsmntYear WHERE CurrentInd='A' and AsmntYearCode IN (SELECT AsmntYearCode FROM AsmntYear WHERE CurrentInd='A')) as Prdendate " +
                        "from Student " +
                        "where Student.StudentId = " + stdid + " and Student.SchoolId = " + schlid + "";
            }

            dtHdr = objData.ReturnDataTable(query, false);

            if (dtHdr != null)
            {
                System.Data.DataColumn newAsmnStdate = new System.Data.DataColumn("AssmntStdate", typeof(System.String));
                newAsmnStdate.DefaultValue = stDate.ToString();
                dtHdr.Columns.Add(newAsmnStdate);

                System.Data.DataColumn newAsmnEndate = new System.Data.DataColumn("AssmntEnddate", typeof(System.String));
                newAsmnEndate.DefaultValue = endDate.ToString();
                dtHdr.Columns.Add(newAsmnEndate);

                GridView2.DataSource = dtHdr;
                GridView2.DataBind();
            }
        }
    }

    public string getHdrLocations(int Clcvid1)
    {
        int Clid = Clcvid1;
        string setLoctn = "";
        if (Clid > 0)
        {
            string getloctnlst = "select Location from StdtClinicalCoverSheet where ClinicalCvId =" + Clid;
            string getloctnlstA = objData.FetchValue(getloctnlst).ToString();
            string setloctnlst = "SELECT (SELECT STUFF(( SELECT ', '+ClassName FROM Class WHERE ClassId IN  (" + getloctnlstA + ") FOR XML PATH('')), 1, 1, ''))from StdtClinicalCoverSheet where ClinicalCvId =" + Clid;            
            string setloctnlstA = objData.FetchValue(setloctnlst).ToString();
            setLoctn = setloctnlstA;
        }

        return setLoctn;
    }

    public string getHdrPrograms(int Clcvid2)
    {
        int Clid = Clcvid2;
        string setPgm = "";
        if (Clid > 0)
        {
            string getpgmlst = "select Program from StdtClinicalCoverSheet where ClinicalCvId =" + Clid;
            string getpgmlstA = objData.FetchValue(getpgmlst).ToString();
            string setpgmlst = "SELECT (SELECT STUFF(( SELECT ', '+Replace(LookupName, '&', '&amp;') FROM LookUp WHERE LookUpId IN (" + getpgmlstA + ") FOR XML PATH('')), 1, 1, '')) from StdtClinicalCoverSheet where ClinicalCvId =" + Clid;
            string setpgmlstA = objData.FetchValue(setpgmlst).ToString();
            setPgm = setpgmlstA;
        }
        return setPgm;
    }
}