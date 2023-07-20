using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
/// <summary>
/// Summary description for clsSubmenu
/// </summary>
public class clsSubmenu
{
    public clsSubmenu()
    {
    }
    private string _subMenu;
    private string _url;
    private string _studname;
    private string _img;
    private int _studId;
    private string _id;
    clsData oData = null;
    public string strQuery = "";

    public string Submenu
    {
        set { _subMenu = value; }
        get { return _subMenu; }
    }
    public string ID
    {
        set { _id = value; }
        get { return _id; }
    }
    public string Url
    {
        set { _url = value; }
        get { return _url; }
    }
    public string StudName
    {
        set { _studname = value; }
        get { return _studname; }
    }
    public string Img
    {
        set { _img = value; }
        get { return _img; }
    }
    public int StudID
    {
        set { _studId = value; }
        get { return _studId; }
    }
    public bool getDatasheetCount(string MenuName, string studID)
    {
        DataTable dtSubmenu = new DataTable();
        oData = new clsData();
        clsLessons oLessons = new clsLessons();
        dtSubmenu = oLessons.getDSLessonPlans(studID, "LP.LessonPlanName as Name,'Datasheet.aspx' as Url,DTmp.DSTempHdrId as ID", "LU.LookupName='Approved'");

        if (dtSubmenu != null)
        {
            if (dtSubmenu.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                strQuery = "Select COUNT(*) from BehaviourDetails Where StudentId=" + studID + " And ActiveInd='A'  ";
                object val = oData.FetchValue(strQuery);
                return true;
            }
        }

        return false;
    }
    public DataTable getSubmenuList(string MenuName, string studID, int SchoolId)
    {
        string value = "";
        if (SchoolId == 1)
        {
            value = "NE";
        }
        else if (SchoolId == 2)
        {
            value = "PA";
        }


        if (studID.Contains("-"))
        {
            string[] ar = studID.Split('-');
            studID = ar[1];
        }
        DataTable dtSubmenu = new DataTable();
        dtSubmenu.Columns.Add("Name", typeof(string));
        dtSubmenu.Columns.Add("Url", typeof(string));
        dtSubmenu.Columns.Add("ID", typeof(string));

        if (MenuName.Trim() == "ASSESSMENTS")
        {
            string menulist = "View Assessments";
            string url = "ReviewAssessmnt.aspx";
            string Ids = "0";
            for (int count = 0; count < menulist.Split(',').Length; count++)
            {
                DataRow dr = dtSubmenu.NewRow();
                dr["Name"] = menulist.Split(',')[count];
                dr["Url"] = url.Split(',')[count];
                dr["ID"] = Ids.Split(',')[count];
                dtSubmenu.Rows.Add(dr);
            }
        }
       
        else if (MenuName.Trim() == "IEPS")
        {


            oData = new clsData();
            string ApproveId = "";
            if (value == "PA")
            {
                object Approved = oData.FetchValue("Select STUFF((SELECT ','+CONVERT(VARCHAR, LookupId) from (Select   LookupId  from LookUp where LookupType='IEP Status'  And (LookupName ='Approved' or LookupName='Expired') group by LookupId) LP FOR XML PATH('')),1,1,'')");
                if (Approved != null) ApproveId = Convert.ToString(Approved);

                string strQuery = "Select top 5 'IEP '+yr.AsmntYearDesc +' V '+ iep.Version AS Name,'PAIEPView.aspx' As  Url,iep.StdtIEP_PEId As ID from [dbo].[StdtIEP_PE] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + studID + "  And iep.StatusId In (" + ApproveId + ")  Order By iep.StdtIEP_PEId Desc";

                dtSubmenu = oData.ReturnDataTable(strQuery, false);
            }
            else if (value == "NE")
            {
                object Approved = oData.FetchValue("Select STUFF((SELECT ','+CONVERT(VARCHAR, LookupId) from (Select   LookupId  from LookUp where LookupType='IEP Status'  And (LookupName ='Approved' or LookupName='Expired') group by LookupId) LP FOR XML PATH('')),1,1,'')");
                if (Approved != null) ApproveId = Convert.ToString(Approved);

                string strQuery = "Select top 5 'IEP '+yr.AsmntYearDesc +' V '+ iep.Version AS Name,'IEPView.aspx' As  Url,iep.StdtIEPId As ID from [dbo].[StdtIEP] iep Inner Join [dbo].[AsmntYear] yr on iep.AsmntYearId=yr.AsmntYearId WHERE iep.StudentId=" + studID + "  And iep.StatusId In (" + ApproveId + ")  Order By iep.StdtIEPId Desc";

                dtSubmenu = oData.ReturnDataTable(strQuery, false);
            }
        }
        else if (MenuName.Trim() == "TIMERS")
        {

            DataRow row = dtSubmenu.NewRow();
            row["Name"] = "<div class='alpha' style='color:#FFFFFF;' >Set Reminder</div>";
            row["Url"] = "BehaviourCalc.aspx";
            row["ID"] = "0";
            dtSubmenu.Rows.InsertAt(row, 0);

            //row = dtSubmenu.NewRow();
            //row["Name"] = "<div class='alpha' style='color:#FFFFFF;' >Behavior Maintenance</div>";
            //row["Url"] = "BehaviourMaintanance.aspx";
            //row["ID"] = "0";
            //dtSubmenu.Rows.InsertAt(row, 0);


        }
        else if (MenuName.Replace(" ","") == "STUDENTINFO")
        {
            DataRow drr = dtSubmenu.NewRow();
            if (SchoolId == 2)
                drr["Name"] = "Protocol Summary";
            else
                drr["Name"] = "Facesheet";
            drr["Url"] = "Demography.aspx";
            drr["ID"] = "0";

            dtSubmenu.Rows.Add(drr);
        }
        else if (MenuName.Trim() == "BSP")
        {
            DataRow drr = dtSubmenu.NewRow();


            drr["Name"] = "<div id='BSPFormsTab'> View BSP Forms</div>";
            drr["Url"] = "DatasheetBSPForms.aspx";
            drr["ID"] = "0";

            dtSubmenu.Rows.Add(drr);
        }
        else if (MenuName.Trim() == "DATASHEETS" || MenuName == "DATASHEETS" || MenuName.Trim().Contains("DATASHEETS") || MenuName.Contains("DATASHEETS") || MenuName.Replace(" ", "") == "DATASHEETS" || MenuName.Replace(" ", "").Contains("DATASHEETS") || MenuName.Trim().Equals("DATASHEETS") || MenuName.Equals("DATASHEETS"))
        {
            oData = new clsData();
            clsLessons oLessons = new clsLessons();
            DataRow dr = dtSubmenu.NewRow();
            dr["Name"] = "Block Schedule";
            dr["Url"] = "Calender.aspx";
            dr["ID"] = "0";
            dtSubmenu.Rows.Add(dr);

            if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
            {
                DataRow rows = dtSubmenu.NewRow();
                rows["Name"] = "Behavior Screen";
                rows["Url"] = "Datasheet.aspx";//"noDatasheetTimerIpad.aspx";
                rows["ID"] = "0";
                dtSubmenu.Rows.Add(rows);
            }
            else
            {
                DataRow rows = dtSubmenu.NewRow();
                rows["Name"] = "Behavior Screen";
                rows["Url"] = "Datasheet.aspx"; //"noDataSheetTimer.aspx";
                rows["ID"] = "0";
                dtSubmenu.Rows.Add(rows);
            }


            DataRow priorSession = dtSubmenu.NewRow();
            priorSession["Name"] = "View Prior Sessions";
            priorSession["Url"] = "DSTempHistory.aspx";
            priorSession["ID"] = "0";
            dtSubmenu.Rows.Add(priorSession);


            DataRow drr = dtSubmenu.NewRow();
            drr["Name"] = "<div id='DatasheetTab' onclick='datasheetFuntion();' >All Lessons</div><script>$('#DatasheetTab').parent().parent().parent().parent().attr('id','submenu_active');</script>";
            drr["Url"] = "Datasheet";
            drr["ID"] = "0";
            dtSubmenu.Rows.Add(drr);


            

            //DataTable dt = oLessons.getDSLessonPlans(studID, "LP.LessonPlanName as Name,'Datasheet.aspx' as Url,DTmp.DSTempHdrId as ID", "LU.LookupName='Approved' AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE')");
            //if (dt != null)
            //{
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        DataRow drr = dtSubmenu.NewRow();
            //        drr["Name"] = row["Name"];
            //        drr["Url"] = row["Url"];
            //        drr["ID"] = row["ID"];
            //        dtSubmenu.Rows.Add(drr);
            //    }
            //}


            //if (dtSubmenu != null)
            //{
            //    if (dtSubmenu.Rows.Count == 0)
            //    {
            //        strQuery = "Select COUNT(*) from BehaviourDetails Where StudentId=" + studID + " And ActiveInd='A'  ";

            //        object val = oData.FetchValue(strQuery);
            //        if (val != null)
            //        {
            //            if (Convert.ToInt32(val) != 0)
            //            {
            //                DataRow row = dtSubmenu.NewRow();
            //                row["Name"] = "<div class='alpha' style='color:#FFFFFF;' >Behavior Screen</div>";
            //                row["Url"] = "dataSheetTimer.aspx";
            //                row["ID"] = "0";
            //                dtSubmenu.Rows.Add(row);
            //            }
            //        }
            //    }
            //}
        }

        else if (MenuName.Replace(" ", "") == "LESSONPLANS")
        {

            DataRow dr = dtSubmenu.NewRow();
            dr["Name"] = "<div id='LessonPlanTab' onclick='PopupLessonPlans(this.id);' >All Lessons</div>";
            dr["Url"] = "Lesson Plans";
            dr["ID"] = "0";
            dtSubmenu.Rows.Add(dr);

            //oData = new clsData();
            //clsLessons oLessons = new clsLessons();
            //dtSubmenu = oLessons.getDSLessonPlans(studID, "LP.LessonPlanName as Name,'LessonPlanAttributes.aspx' as Url, DTmp.DSTempHdrId As ID", "LU.LookupName='Approved'");

            DataRow drex = dtSubmenu.NewRow();
            drex["Name"] = "<div id='EXPTab'> Export Lessons</div>";
            drex["Url"] = "ExportLessons.aspx";
            drex["ID"] = "0";
            dtSubmenu.Rows.Add(drex);
        }
        else if (MenuName.Trim() == "GRAPHS")
        {
           //string menulist = "Events Tracker,Academic,Clinical,Session-based,Dynamic Academic Reports,Dynamic Clinical Reports,Progress Report,Excel View,Progress Summary Report,Clinical Progress Summary Report";
           //string url = "EventTracker.aspx,LessonReportsWithPaging.aspx,BiweeklyBehaviorGraph.aspx,AcademicSessionReport.aspx,SharePointRedirect.aspx,SharePointBehavior.aspx,ProgressReport.aspx,ExcelViewReport.aspx,ProgressSummaryReport.aspx,ProgressSummaryReportClinical.aspx";
           //New Graph landing Page added on 12-08-2020 Dev 1 start ---
            DataRow drr1 = dtSubmenu.NewRow();
            drr1["Name"] = "<div id='GraphDashBoard'></div>";
            drr1["Url"] = "GraphTileGrid.aspx";
            drr1["ID"] = "0";
            dtSubmenu.Rows.Add(drr1);
            //New Graph landing Page added on 12-08-2020 Dev 1 END ---

            string menulist = "Events Tracker,Academic,Clinical,Session-based";
            string url = "EventTracker.aspx,LessonReportsWithPaging.aspx,BiweeklyBehaviorGraph.aspx,AcademicSessionReport.aspx";
            //string url = "EventTracker.aspx,LessonReportsWithPaging.aspx,BiweeklyBehaviorGraph.aspx,AcademicSessionReport.aspx,TestDownload.aspx";
            string Ids = "0,0,0,0";
            for (int count = 0; count < menulist.Split(',').Length; count++)
            {
                String menuName = Regex.Replace(menulist.Split(',')[count], @"[^0-9a-zA-Z]+ ", "");
                menuName = Regex.Replace(menuName, @"\s+", "");
                DataRow dr = dtSubmenu.NewRow();
                dr["Name"] = "<div id='" + menuName + "'>" + menulist.Split(',')[count] + "</div>";
                dr["Url"] = url.Split(',')[count];
                dr["ID"] = Ids.Split(',')[count];
                dtSubmenu.Rows.Add(dr);
            }
            
            DataRow drChain = dtSubmenu.NewRow();
            drChain["Name"] = "<div id='ChainGraphTab' onclick='PopupLessonPlans(this.id);' >Chain Graph</div>";
            drChain["Url"] = "ChainGraph";
            drChain["ID"] = "0";
            dtSubmenu.Rows.Add(drChain);

            menulist = "Progress Report,Excel View,Progress Summary Report,Clinical Progress Summary Report, Step/Trial Data";
            url = "ProgressReport.aspx,ExcelViewReport.aspx,ProgressSummaryReport.aspx,ProgressSummaryReportClinical.aspx,TextBased.aspx";
            Ids = "0,0,0,0,0";
            for (int count = 0; count < menulist.Split(',').Length; count++)
            {
                String menuName = Regex.Replace(menulist.Split(',')[count], @"[^0-9a-zA-Z]+ ", "");
                menuName = Regex.Replace(menuName, @"\s+", "");
                DataRow dr = dtSubmenu.NewRow();
                dr["Name"] = "<div id='" + menuName + "'>" + menulist.Split(',')[count] + "</div>";
                dr["Url"] = url.Split(',')[count];
                dr["ID"] = Ids.Split(',')[count];
                dtSubmenu.Rows.Add(dr);
            }

            DataRow drr = dtSubmenu.NewRow();
            drr["Name"] = "<div id='GraphTab' onclick='PopupLessonPlans(this.id);' >Maintenance Graph</div>";
            drr["Url"] = "Graph";
            drr["ID"] = "0";
            dtSubmenu.Rows.Add(drr);  
            
            

        }

        else if (MenuName.Trim() == "COVERSHEETS")
        {
            string menulist = "Academic,Clinical,RTF Coversheet";
            string url = "ACSheet.aspx,ClinicalSheetNew.aspx,GrandRoundCvrsht.aspx";
            string Ids = "0,0,0";
            for (int count = 0; count < menulist.Split(',').Length; count++)
            {
                DataRow dr = dtSubmenu.NewRow();
                dr["Name"] = menulist.Split(',')[count];
                dr["Url"] = url.Split(',')[count];
                dr["ID"] = Ids.Split(',')[count];
                dtSubmenu.Rows.Add(dr);
            }
        }
        else if (MenuName.Trim() == "EDIT")
        {

            object pendstatus = null;
            object IepStatus = null;
            object IepId = null;
            int IEP_id = 0;
            string menulist = "";
            string url = "";
            oData = new clsData();
            clsSession sess = new clsSession();
            sess.IEPId = 0;
            if (value == "NE")
            {
                if (oData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A') ") == true)
                {
                    pendstatus = oData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ").ToString();
                    if (int.Parse(pendstatus.ToString()) > 0)
                    {
                        IepStatus = oData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
                    }
                    if ((IepStatus.ToString() == "Approved") || (IepStatus.ToString() == "Expired"))
                    {
                        IEP_id = 0;
                    }
                    else
                    {
                        IepId = oData.FetchValue("Select TOP 1 StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ");
                        IEP_id = int.Parse(IepId.ToString());
                    }
                }
                menulist = "Assessments,Create IEP,Behaviors,Customize Goals And Lessons,Customize Lesson Plans";
                url = "FormAssess.aspx,CreateCustomIEP.aspx,BehaviourMaintanance.aspx,AsmntReview.aspx,CustomizeTemplateEditor.aspx";
            }
            else if (value == "PA")
            {
                if (oData.IFExists("Select StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A') ") == true)
                {
                    pendstatus = oData.FetchValue("Select TOP 1 StatusId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEP_PEId DESC ").ToString();
                    if (int.Parse(pendstatus.ToString()) > 0)
                    {
                        IepStatus = oData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
                    }
                    if ((IepStatus.ToString() == "Approved") || (IepStatus.ToString() == "Expired"))
                    {
                        IEP_id = 0;
                    }
                    else
                    {
                        IepId = oData.FetchValue("Select TOP 1 StdtIEP_PEId from StdtIEP_PE where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEP_PEId DESC ");
                        IEP_id = int.Parse(IepId.ToString());
                    }
                }
                menulist = "Assessments,Create IEP,Behaviors,Customize Goals And Lessons,Customize Lesson Plans";
                url = "FormAssess.aspx,CreateCustomIepPE.aspx,BehaviourMaintanance.aspx,AsmntReview.aspx,CustomizeTemplateEditor.aspx";
            }
            //string url = "FormAssess.aspx,CreatePEIEP.aspx,CreateCustomIEP.aspx,AsmntReview.aspx,CustomizeTemplateEditor.aspx";
            string Ids = "0," + IEP_id + ",0,0,0";
            for (int count = 0; count < menulist.Split(',').Length; count++)
            {
                DataRow dr = dtSubmenu.NewRow();
                dr["Name"] = menulist.Split(',')[count];
                dr["Url"] = url.Split(',')[count];
                dr["ID"] = Ids.Split(',')[count];
                dtSubmenu.Rows.Add(dr);
            }
        }

        return dtSubmenu;
    }
}


