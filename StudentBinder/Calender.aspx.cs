using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
public partial class StudentBinder_Calender : System.Web.UI.Page
{
    clsSession oSession = null;
    clsData oData = new clsData();

    bool Disable = false;

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




        if (Request.QueryString["mode"] != null)
        {
            if (Request.QueryString["mode"].ToString() == "Day")
                hfMode.Value = "Day";
            if (Request.QueryString["mode"].ToString() == "Week")
                hfMode.Value = "Week";
        }
        else
        {
            hfMode.Value = "Week";
        }
        if (!IsPostBack)
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        oSession = (clsSession)Session["UserSession"];
        clsLessons oLessons = new clsLessons();
        checkResident();
        btn_day.Style.Add("background-color", "#202253");

        oSession.dt_StudList = null;
        oSession.days_4weeks = null;
        oSession.current_week = null;

        btnDay.Style.Add("background-color", "#202253");
        Calendar1.SelectedDate = DateTime.Today;

        oSession.CrrntDate = DateTime.Now.ToString("yyyy-MM-dd");
        hfDate.Value = DateTime.Now.ToString("yyyy-MM-dd");


        oData.ReturnDropDown("SELECT ClassId as Id,ClassName as Name FROM Class", ddlClass);
        oData.ReturnDropDown("SELECT ClassId as Id,ClassName as Name FROM Class", ddlPopClass);

        //DataTable dtLP = new DataTable();
        //dtLP.Columns.Add("Id", typeof(string));
        //dtLP.Columns.Add("Name", typeof(string));
        //DataRow dr0 = dtLP.NewRow();
        //dr0["Id"] = -1;
        //dr0["Name"] = "----------Select Lesson Plan----------";
        //dtLP.Rows.Add(dr0);
        //clsLessons oLessons1 = new clsLessons();
        //HttpContext.Current.Session["datasheet"] = "ViewPriorSession";
        //DataTable dt = oLessons1.getDSLessonPlans(oSession.StudentId.ToString(), "LP.LessonPlanId AS Id, DTmp.DSTemplateName AS Name", "(LU.LookupName='Approved' OR LU.LookupName='Maintenance') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");
        

        DataTable dt = oLessons.getDSLessonPlans1(oSession.StudentId.ToString(), "DTmp.LessonPlanId as Id,DTmp.DSTemplateName as Name", "(LU.LookupName='Approved' OR LU.LookupName='MAINTENANCE') AND (DTmp.DSMode IS NULL OR DTmp.DSMode='MAINTENANCE' OR DTmp.DSMode='')");

        if (dt != null)
        {
            ddlLP.DataSource = dt;
            ddlLP.DataTextField = "Name";
            ddlLP.DataValueField = "Id";
            ddlLP.DataBind();
            ddlEditLP.DataSource = dt;
            ddlEditLP.DataTextField = "Name";
            ddlEditLP.DataValueField = "Id";
            ddlEditLP.DataBind();
            ddlLP.Items.Insert(0, "---------------Select--------------");
            ddlLP.Items.Add(new ListItem("Other", "00000"));
            ddlEditLP.Items.Insert(0, "---------------Select--------------");
            ddlEditLP.Items.Add(new ListItem("Other", "00000"));
        }
        //oData.ReturnDropDown("SELECT LP.LessonPlanId as Id,LP.LessonPlanName as Name FROM StdtLessonPlan SLP INNER JOIN LessonPlan LP ON SLP.LessonPlanId=LP.LessonPlanId WHERE SLP.StudentId='" + oSession.StudentId + "' AND SLP.ActiveInd='A'", ddlLP);
        //oData.ReturnDropDown("SELECT LP.LessonPlanId as Id,LP.LessonPlanName as Name FROM StdtLessonPlan SLP INNER JOIN LessonPlan LP ON SLP.LessonPlanId=LP.LessonPlanId WHERE SLP.StudentId='" + oSession.StudentId + "' AND SLP.ActiveInd='A'", ddlEditLP);

        if (ddlLP.Items.Count == 0)
        {
            ddlLP.Items.Insert(0, "---------------Select--------------");
        }
        if (ddlEditLP.Items.Count == 0)
        {
            ddlEditLP.Items.Insert(0, "---------------Select--------------");
        }

        if (oSession.Classid != null)
        {
            if (oSession != null) ddlClass.SelectedValue = oSession.Classid.ToString();

            if (hfMode.Value == "Week")
            {
                ddlPopClass.SelectedValue = oSession.Classid.ToString();
                ddlStudents.Items.Clear();
                oData.ReturnDropDown("Select std.StudentId as Id,(StudentLname+','+StudentFname) as Name from StdtClass SC inner join Student std on std.StudentId=SC.StdtId where SC.ClassId=" + ddlClass.SelectedValue + "", ddlStudents);
                oData.ReturnDropDown("Select std.StudentId as Id,(StudentLname+','+StudentFname) as Name from StdtClass SC inner join Student std on std.StudentId=SC.StdtId where SC.ClassId=" + ddlPopClass.SelectedValue + "", ddlPopStdnts);
                if (oSession != null)
                {
                    ddlPopStdnts.SelectedValue = oSession.StudentId.ToString();
                    ddlStudents.SelectedValue = oSession.StudentId.ToString();
                }
                Calendar1.VisibleDate = DateTime.Now;
                Calendar1.SelectedDate = DateTime.Now;
                Calendar1.SelectionMode = CalendarSelectionMode.DayWeek;
                System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
                DayOfWeek today = DateTime.Now.DayOfWeek;
                DateTime Date = DateTime.Today;
                int i = 1, j = 1;
                while (today != DayOfWeek.Sunday)
                {
                    Date = DateTime.Today.AddDays(i);
                    today = DateTime.Today.AddDays(i).DayOfWeek;
                    i++;
                }
                while (today != DayOfWeek.Monday)
                {
                    if (i != j)
                        Calendar1.SelectedDates.Add(DateTime.Today.AddDays(i - j));
                    today = DateTime.Today.AddDays(i - j).DayOfWeek;
                    j++;
                }

                if (oSession != null)
                {
                    oSession.days_4weeks = Calendar1.SelectedDates;
                    oSession.current_week = Calendar1.SelectedDates;
                    weekview(Calendar1.SelectedDates);
                }

            }
            if (hfMode.Value == "Day")
            {
                chkStudnts.Items.Clear();
                oData.ReturnCheckBoxList("Select std.StudentId as Id,(StudentLname+','+StudentFname) as Name from StdtClass SC inner join Student std on std.StudentId=SC.StdtId where SC.ClassId=" + ddlClass.SelectedValue, chkStudnts);

                if (oSession != null)
                {
                    oSession.dt_StudList = oData.ReturnDataTable("SELECT StudentId,StudentFname FROM Student Std INNER JOIN StdtClass SC ON SC.StdtId=Std.StudentId WHERE SC.ClassId=" + ddlClass.SelectedValue, false);
                    Calendr(oSession.CrrntDate);
                }

            }
            checkResident();
        }
        ddlClass.Enabled = false;
        ddlStudents.Enabled = false;


        clsGeneral.PageReadAndWrite(oSession.LoginId, oSession.SchoolId, out Disable);
        if (Disable == true)
        {
            btnReplicate.Visible = false;
        }
        else
        {
            btnReplicate.Visible = true;
        }
        if (oSession.current_week.Contains(Calendar1.SelectedDates[1]))
        {
            btnReplicate.Visible = false;
        }
        else
        {
            btnReplicate.Visible = true;
        }
    }

    protected void CheckStudList()
    {
        oSession = (clsSession)Session["UserSession"];
        if (oSession != null)
        {
            if (oSession.dt_StudList != null)
            {
                foreach (DataRow drStud in oSession.dt_StudList.Rows)
                {
                    foreach (ListItem liStd in chkStudnts.Items)
                    {
                        if (liStd.Value == drStud["StudentId"].ToString())
                        {
                            liStd.Selected = true;
                        }
                    }
                }
            }
        }
    }

    Random random = new Random();
    protected void Calendr(string selDate)
    {
        int resInd = 0;
        if (hidRes_Value.Value.ToString() == "Resident") resInd = 1;
        oSession = (clsSession)Session["UserSession"];
        clsData ObjData = new clsData();
        if (oSession != null)
        {
            int startTime = Convert.ToInt32(ObjData.FetchValue("SELECT ISNULL(SUBSTRING(CONVERT(VARCHAR(50), MIN(SCAL.StartTime)),1,2),0) FROM SchoolCal SCAL INNER JOIN Class CLS ON SCAL.SchoolId=CLS.SchoolId WHERE CLS.ClassId='" + oSession.Classid + "' AND CLS.ActiveInd='A'  And SCAL.ResidenceInd=" + resInd + ""));

            btnReplicate.Visible = true;
            string day = Convert.ToDateTime(selDate).DayOfWeek.ToString();
            lblDate.Text = Convert.ToDateTime(selDate).DayOfWeek.ToString() + " / " + Convert.ToDateTime(selDate).ToString("D");
            hfMode.Value = "Day";
            chkStudnts.Enabled = true;
            btnAddStud.Enabled = true;

            DataTable dtStud = new DataTable();


            if (oSession != null)
            {
                if (oSession.dt_StudList != null)
                    if (oSession.dt_StudList.Rows.Count > 0)
                    {
                        dtStud = oSession.dt_StudList;
                        CheckStudList();
                    }
            }
            for (int index = 0; index < dtStud.Rows.Count; index++)
            {
                float width = (Convert.ToSingle(95) / Convert.ToSingle(dtStud.Rows.Count));
                Literal ltStud = new Literal();
                ltStud.ID = "ltStud" + index.ToString();
                HtmlTableCell td = new HtmlTableCell();
                HtmlTableCell td2 = new HtmlTableCell();
                td.Width = width.ToString() + "%"; td2.Width = width.ToString() + "%";
                td.Align = "left"; td2.Align = "center";
                td2.BgColor = "#ECE9D8"; td2.Height = "30px"; td2.Style.Add("font-weight", "Bold");
                td2.InnerText = dtStud.Rows[index]["StudentFname"].ToString();

                tblCalndr.Rows[0].Cells.Add(td2);
                tblCalndr.Rows[1].Cells.Add(td);
                tblCalndr.Rows[1].Cells[index + 1].Align = "left";
                tblCalndr.Rows[1].Cells[index + 1].Controls.Add(ltStud);
            }
            if (dtStud.Rows.Count == 0)
            {
                ShowMessage();
            }

            string selHoliday = "SELECT HolName FROM SchoolHoliday WHERE HolDate='" + selDate + "' AND SchoolId=" + oSession.SchoolId + "";

            object Holiday = null;
            Holiday = oData.FetchValue(selHoliday);
            for (int index = 0; index < dtStud.Rows.Count; index++)
            {
                Literal lt_Stud = (Literal)tblCalndr.FindControl("ltStud" + index.ToString());
                string innerDiv = "<div id='" + dtStud.Rows[index]["StudentId"].ToString() + "' runat='server' " +
                                "style='width:auto;height:auto;'>";
                string divCell = "";

                string selStartTime = "";
                string selEndTime = "";
                //if (hidRes_Value.Value.ToString() == "Day")
                //{
                //    //"SELECT ISNULL(SUBSTRING(CONVERT(VARCHAR(50), MIN(SCAL.StartTime)),1,2),0) FROM SchoolCal SCAL INNER JOIN Class CLS ON SCAL.SchoolId=CLS.SchoolId WHERE CLS.ClassId='" + oSession.Classid + "' AND CLS.ActiveInd='A'  And SCAL.ResidenceInd=" + resInd + "";
                //    selStartTime = "SELECT StartTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=1 AND SchoolId=" + oSession.SchoolId + "";
                //    selEndTime = "SELECT EndTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=1 AND SchoolId=" + oSession.SchoolId + "";
                //}
                //else if (hidRes_Value.Value.ToString() == "Resident")
                //{
                //    selStartTime = "SELECT StartTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=0 AND SchoolId=" + oSession.SchoolId + "";
                //    selEndTime = "SELECT EndTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=0 AND SchoolId=" + oSession.SchoolId + "";
                //}
                
                object strtTime = null;
                object endTime = null;

                strtTime = oData.FetchValue(selStartTime);
                endTime = oData.FetchValue(selEndTime);
                strtTime = "07:00:00.0000000";
                endTime = "23:59:59.0000000";
                TimeSpan tstart = new TimeSpan();
                if (strtTime != null)
                {
                    tstart = Convert.ToDateTime(strtTime.ToString()).TimeOfDay;
                }
                TimeSpan tend = new TimeSpan();
                if (endTime != null)
                {
                    tend = Convert.ToDateTime(endTime.ToString()).TimeOfDay;
                }
                TimeSpan tsInit = new TimeSpan(startTime, 0, 0);
                TimeSpan intrvl = new TimeSpan(0, 30, 0);

                for (int count = 0; count < 48; count++)
                {
                    string time = "";
                    string time2 = "";
                    string time1 = "";
                    if (tsInit.ToString().Substring(0, 5) == "1.00:")
                    {
                        tsInit = tsInit.Subtract(tsInit);
                    }
                    time = tsInit.ToString().Substring(0, 5);
                    time1 = tsInit.ToString().Substring(0, 5);
                    DateTime dateTime = DateTime.ParseExact(time, "HH:mm", CultureInfo.InvariantCulture);
                    string TimeN = dateTime.ToString("hh:mm tt", CultureInfo.CurrentCulture);
                    //string tt = dateTime.ToString(" tt ", CultureInfo.CurrentCulture);
                    time2 = TimeN.ToString().Substring(0, 8);
                    time = TimeN.ToString().Substring(0, 5);
                    string Events = LoadDay(dtStud.Rows[index]["StudentId"].ToString(), time, selDate);

                    if ((strtTime == null) || (endTime == null))
                    {
                        Events = Events.Replace("onclick=editEvent(this,event); ", " ");
                        divCell = divCell + "<div id='" + dtStud.Rows[index]["StudentId"].ToString() + "//" + time + "' runat='server' class='holiday' " +
                                    "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                    }
                    else if (Holiday != null)
                    {
                        divCell = divCell + "<div id='" + dtStud.Rows[index]["StudentId"].ToString() + "//" + time + "' runat='server' class='holiday' " +
                                    "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                    }
                    else
                    {
                        if (tsInit < tstart)
                        {
                            Events = Events.Replace("onclick=editEvent(this,event); ", " ");
                            divCell = divCell + "<div id='" + dtStud.Rows[index]["StudentId"].ToString() + "//" + time + "' runat='server' class='holiday' " +
                                    "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                        }
                        else if (tsInit >= tend)
                        {
                            Events = Events.Replace("onclick=editEvent(this,event); ", " ");
                            divCell = divCell + "<div id='" + dtStud.Rows[index]["StudentId"].ToString() + "//" + time + "' runat='server' class='holiday' " +
                                    "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                        }
                        else
                        {
                            divCell = divCell + "<div id='" + dtStud.Rows[index]["StudentId"].ToString() + "//" + time2 + "' runat='server' onmouseover=mouseOver(this); onmouseout=mouseOut(this); onclick=popup(this,'" + time + "','" + time1 + "'); " +
                                    "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                        }
                    }
                    tsInit = tsInit.Add(intrvl);
                }
                innerDiv = innerDiv + divCell + "</div>";
                lt_Stud.Text = innerDiv;

            }

            string timeDiv = "";
            TimeSpan init = new TimeSpan(startTime, 0, 0);
            TimeSpan intr = new TimeSpan(0, 30, 0);
            for (int count = 0; count < 48; count++)
            {
                if (init.ToString().Substring(0, 5) == "1.00:")
                {
                    init = init.Subtract(init);
                }
                timeDiv = timeDiv + "<div style='width:auto;height:30px;border:1px solid White;background-color:#ECE9D8;text-align:right;font-size:small;'>" + init.ToString().Substring(0, 5) + "</div>";
                init = init.Add(intr);
            }
            tdTime.InnerHtml = timeDiv;
        }
    }
    protected void weekview(SelectedDatesCollection dates)
    {
        int resInd = 0;
        if (hidRes_Value.Value.ToString() == "Resident") resInd = 1;

        if (dates.Count == 0) return;
        oSession = (clsSession)Session["UserSession"];
        clsData ObjData = new clsData();
        if (oSession != null)
        {
            int startTime = Convert.ToInt32(ObjData.FetchValue("SELECT ISNULL(SUBSTRING(CONVERT(VARCHAR(50), MIN(SCAL.StartTime)),1,2),0) FROM SchoolCal SCAL INNER JOIN Class CLS ON SCAL.SchoolId=CLS.SchoolId WHERE CLS.ClassId='" + oSession.Classid + "' AND CLS.ActiveInd='A' "));
            btnReplicate.Visible = true;
            hfMode.Value = "Week";
            chkStudnts.Enabled = false;
            btnAddStud.Enabled = false;
            lblDate.Text = dates[0].ToString("MM/dd/yyyy") + " to " + dates[6].ToString("MM/dd/yyyy");
            lblDate.Text = lblDate.Text.Replace("-", "/");

            if (ddlStudents.SelectedIndex > 0)
                for (int index = 0; index < dates.Count; index++)
                {
                    double width = (100 / dates.Count) - 1;
                    Literal ltStud = new Literal();
                    ltStud.ID = "ltDay" + index.ToString();
                    HtmlTableCell td = new HtmlTableCell();
                    HtmlTableCell td2 = new HtmlTableCell();
                    td.Width = width.ToString() + "%"; td2.Width = width.ToString() + "%";
                    td.Align = "left"; td2.Align = "center";
                    td2.BgColor = "#ECE9D8"; td2.Height = "30px"; td2.Style.Add("font-weight", "Bold");

                    string selHoliday = "SELECT HolName FROM SchoolHoliday WHERE HolDate='" + dates[index].ToString("yyyy-MM-dd") + "' AND SchoolId=" + oSession.SchoolId + "";
                    object Holiday = null;
                    Holiday = oData.FetchValue(selHoliday);

                    td2.InnerHtml = dates[index].DayOfWeek.ToString() + " " + dates[index].ToString("MM/dd");

                    tblCalndr.Rows[0].Cells.Add(td2);
                    tblCalndr.Rows[1].Cells.Add(td);
                    tblCalndr.Rows[1].Cells[index + 1].Controls.Add(ltStud);
                    tblCalndr.Rows[1].Cells[index + 1].VAlign = "top";
                }
            if ((ddlStudents.SelectedIndex == 0) || (ddlPopClass.SelectedIndex == 0))
            {
                ShowMessage();
            }
            if (ddlStudents.SelectedIndex > 0)
                for (int index = 0; index < dates.Count; index++)
                {
                    Literal lt_Stud = (Literal)tblCalndr.FindControl("ltDay" + index.ToString());
                    string innerDiv = "<div id='" + dates[index].ToString("MM-dd-yyyy") + "' runat='server' " +
                                    "style='width:auto;height:auto;'>";

                    string selHoliday = "SELECT HolName FROM SchoolHoliday WHERE HolDate='" + dates[index].ToString("yyyy-MM-dd") + "' AND SchoolId=" + oSession.SchoolId + "";
                    object Holiday = null;
                    Holiday = oData.FetchValue(selHoliday);

                    string divCell = "";
                    string day = dates[index].DayOfWeek.ToString();

                    //string selStartTime = ""; //---Commmented for Fixing Errorlog production log 2020
                    //string selEndTime = ""; //---Commmented for Fixing Errorlog production log 2020

                    //if (hidRes_Value.Value.ToString() == "Day")
                    //{
                    //    selStartTime = "SELECT StartTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=1 AND SchoolId=" + oSession.SchoolId + "";
                    //    selEndTime = "SELECT EndTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=1 AND SchoolId=" + oSession.SchoolId + "";
                    //}
                    //else if (hidRes_Value.Value.ToString() == "Resident")
                    //{
                    //    selStartTime = "SELECT StartTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=0 AND SchoolId=" + oSession.SchoolId + "";
                    //    selEndTime = "SELECT EndTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=0 AND SchoolId=" + oSession.SchoolId + "";
                    //}
                    //selStartTime = "07:00:00";
                    //selEndTime = "24:00:00";
                    object strtTime = null;
                    object endTime = null;

                    //strtTime = oData.FetchValue(selStartTime); //---Commmented for Fixing Errorlog production log 2020
                    //endTime = oData.FetchValue(selEndTime); //---Commmented for Fixing Errorlog production log 2020
                    strtTime = "07:00:00.0000000";
                    endTime = "23:59:59.0000000";

                    TimeSpan tstart = new TimeSpan();
                    if (strtTime != null)
                    {
                        tstart = Convert.ToDateTime(strtTime.ToString()).TimeOfDay;
                    }
                    TimeSpan tend = new TimeSpan();
                    if (endTime != null)
                    {
                        tend = Convert.ToDateTime(endTime.ToString()).TimeOfDay;
                    }
                    TimeSpan tsInit = new TimeSpan(startTime, 0, 0);
                    TimeSpan intrvl = new TimeSpan(0, 30, 0);

                    for (int count = 0; count < 48; count++)
                    {
                        string time = "";
                        string time3 = "";
                        string time2 = "";
                        if (tsInit.ToString().Substring(0, 5) == "1.00:")
                        {
                            tsInit = tsInit.Subtract(tsInit);
                        }
                        time = tsInit.ToString().Substring(0, 5);
                        time2 = tsInit.ToString().Substring(0, 5);
                        DateTime dateTime = DateTime.ParseExact(time, "HH:mm",CultureInfo.InvariantCulture);
                        //TimeSpan tsInit1 = new TimeSpan(dateTime, 0, 0);
                        string TimeN = dateTime.ToString("hh:mm tt ", CultureInfo.CurrentCulture);
                        //string tt = dateTime.ToString(" tt ", CultureInfo.CurrentCulture);
                        time = TimeN.ToString().Substring(0, 5);
                        time3 = TimeN.ToString().Substring(0, 8);
                        string Events = LoadDay(ddlStudents.SelectedValue, time2, dates[index].ToString("yyyy-MM-dd"));
                        if ((strtTime == null) || (endTime == null))
                        {
                            Events = Events.Replace("onclick=editEvent(this,event); ", " ");
                            divCell = divCell + "<div id='" + dates[index].ToString("MM-dd-yyyy") + "//" + time + "' runat='server' class='holiday' " +
                                        "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                        }
                        else if (Holiday != null)
                            divCell = divCell + "<div id='" + dates[index].ToString("MM-dd-yyyy") + "//" + time + "' runat='server' class='holiday' " +
                                        "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                        else
                        {
                            if (tsInit < tstart)
                            {
                                Events = Events.Replace("onclick=editEvent(this,event); ", " ");
                                divCell = divCell + "<div id='" + dates[index].ToString("MM-dd-yyyy") + "//" + time + "' runat='server' class='holiday' " +
                                        "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                            }
                            else if (tsInit >= tend)
                            {
                                Events = Events.Replace("onclick=editEvent(this,event); ", " ");
                                divCell = divCell + "<div id='" + dates[index].ToString("MM-dd-yyyy") + "//" + time + "' runat='server' class='holiday' " +
                                        "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                            }
                            else
                            {
                                divCell = divCell + "<div id='" + dates[index].ToString("MM-dd-yyyy") + "//" + time2 + "' runat='server' onmouseover=mouseOver(this); onmouseout=mouseOut(this); onclick=popup(this,'" + time + "','" + time2 + "'); " +
                                        "style='width:auto;height:30px;border:1px solid #EBE3E3;position:relative;text-align:left;'>" + Events + "</div>";
                            }
                        }
                        tsInit = tsInit.Add(intrvl);

                    }
                    innerDiv = innerDiv + divCell + "</div>";
                    lt_Stud.Text = innerDiv;

                }
            string timeDiv = "";
            string time1 = "";
            string tt1 = "";
            string t1 = "";
            TimeSpan init = new TimeSpan(startTime, 0, 0);
            //float x = float.Parse(startTime.ToString());
            //DateTime dt = DateTime.Today.Add(init);
            TimeSpan intr = new TimeSpan(0, 30, 0);
            //float b = 0.5f;
            for (int count = 0; count < 48; count++)
            {
                if (init.ToString().Substring(0, 5) == "1.00:")
                {
                    init = init.Subtract(init);
                }
                
                //x = x + b;
                //if (x == 25.5)
                //{
                  //  x = float.Parse(startTime.ToString());
                //}
               
               //     if (init.ToString() == "13:00:00")
                 //   {
                   // TimeSpan init1 = new TimeSpan(1, 0, 0);
                     //   init = init1;
                    //}                    
               
             //   if (x > 12)
               // {
                time1 = init.ToString().Substring(0, 5);
                if (time1 == "08:00")
                {
                }
                DateTime dateTime = DateTime.ParseExact(time1, "HH:mm", CultureInfo.InvariantCulture);
                string TimeN = dateTime.ToString("hh:mm", CultureInfo.CurrentCulture);
                time1 = TimeN.ToString().Substring(0, 5);
                tt1 = dateTime.ToString("tt", CultureInfo.CurrentCulture);
                if (time1 == "11:00")
                {
                     t1 = dateTime.ToString("tt", CultureInfo.CurrentCulture);
                }
                //if (time1 == "12:00" || time1 == "12:30")
                //{
                //   tt1 = t1;
                //}
                timeDiv = timeDiv + "<div style='width:60px;height:30px;border:1px solid White;background-color:#ECE9D8;text-align:right;font-size:small;'>" + time1 + " " + tt1 + "</div>";
                //}
                //else
                  //  timeDiv = timeDiv + "<div style='width:60px;height:30px;border:1px solid White;background-color:#ECE9D8;text-align:right;font-size:small;'>" + init.ToString().Substring(0, 5) + "AM"+ "</div>";
                    init = init.Add(intr);
                

            }
            tdTime.InnerHtml = timeDiv;
        }
    }
    protected string LoadDay(string studentId, string strtTime, string selDate)
    {
        //string setLpid = "SELECT LPId FROM StdtLPSched LPSched WHERE Day='" + selDate + "' AND StdtId=" + studentId + " AND StartTime='" + strtTime + "'";
        //int getlpid = Convert.ToInt32(oData.FetchValue(setLpid));
        string selEvents = "";

        //if (getlpid > 0)
        //{
        //    selEvents = " SELECT StdtLPSchedId,StdtId,StartTime,EndTime,CASE WHEN (SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=LPSched.LPId AND StudentId=LPSched.StdtId " +
        //                  " AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus')) >0 THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr " +
        //                  " WHERE LessonPlanId=LPSched.LPId AND StudentId=LPSched.StdtId AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupName IN ('Approved','Maintenance') " +
        //                  " AND LookupType='TemplateStatus') ORDER BY DSTempHdrId DESC) ELSE (SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE LessonPlanId=LPSched.LPId AND StudentId=LPSched.StdtId " +
        //                  " ORDER BY DSTempHdrId DESC) END LessonPlanName,Day FROM StdtLPSched LPSched INNER JOIN LessonPlan LP ON LP.LessonPlanId=LPSched.LPId " +
        //                  " WHERE Day='" + selDate + "' AND StdtId=" + studentId + " AND StartTime='" + strtTime + "'";
        //}
        //else
        //{
        //    selEvents = " SELECT StdtLPSchedId,StdtId,StartTime,EndTime,SchOtherDesc AS LessonPlanName,Day FROM StdtLPSched LPSched " +
        //                 " WHERE Day='" + selDate + "' AND StdtId=" + studentId + " AND StartTime='" + strtTime + "'";
        //}
        selEvents = " SELECT StdtLPSchedId,StdtId,StartTime,EndTime,CASE WHEN (SELECT COUNT(*) FROM DSTempHdr WHERE LessonPlanId=LPSched.LPId AND StudentId=LPSched.StdtId " +
                          " AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupName IN ('Approved','Maintenance') AND LookupType='TemplateStatus')) >0 THEN (SELECT TOP 1 DSTemplateName FROM DSTempHdr " +
                          " WHERE LessonPlanId=LPSched.LPId AND StudentId=LPSched.StdtId AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupName IN ('Approved','Maintenance') " +
                          " AND LookupType='TemplateStatus') ORDER BY DSTempHdrId DESC) ELSE (SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE LessonPlanId=LPSched.LPId AND StudentId=LPSched.StdtId " +
                          " ORDER BY DSTempHdrId DESC) END LessonPlanName,Day FROM StdtLPSched LPSched INNER JOIN LessonPlan LP ON LP.LessonPlanId=LPSched.LPId " +
                          " WHERE Day='" + selDate + "' AND StdtId=" + studentId + " AND StartTime='" + strtTime + "'UNION " +
                          " SELECT StdtLPSchedId,StdtId,StartTime,EndTime,SchOtherDesc AS LessonPlanName,Day FROM StdtLPSched LPSched " +
                          " WHERE Day='" + selDate + "' AND StdtId=" + studentId + " AND StartTime='" + strtTime + "' AND SchOtherDesc is not null";
        DataTable dtEvents = new DataTable();
        dtEvents = oData.ReturnDataTable(selEvents, false);
        string innerDiv = "";
        if (dtEvents != null)
            for (int index = 0; index < dtEvents.Rows.Count; index++)
            {
                string divEvent = "";
                string endTime = dtEvents.Rows[index]["EndTime"].ToString().Substring(0, 5);
                int sHour = Convert.ToInt32(strtTime.Split(':')[0]);
                int sMin = Convert.ToInt32(strtTime.Split(':')[1]);
                int eHour = Convert.ToInt32(endTime.Split(':')[0]);
                int eMin = Convert.ToInt32(endTime.Split(':')[1]);
                int Diff = ((eHour * 60) + eMin) - ((sHour * 60) + sMin);
                int Height = Diff + (Diff / 30) + (Diff / 60);
                int partitn = (75 / dtEvents.Rows.Count);
                int left = partitn * index;
                //var color = String.Format("#{0:X6}", random.Next(0x1000000));
                string[] colorcodes = { "F8D8D8", "F2CDEE", "EACDF2", "DCCDF2", "CDD7F2", "CDEEF2", "7AEAB6", "B5EAAF", "D0EAAF", "F7F72F", "F7C22F", "FADABA", "C1E1E3" };
                var color = "#" + colorcodes[random.Next(0, 12)];
                divEvent = "<div id='" + dtEvents.Rows[index]["StdtLPSchedId"].ToString() + "' title='" + dtEvents.Rows[index]["LessonPlanName"].ToString() + "' style='width: " + partitn + "%;left:" + left + "%;overflow: hidden;" +
                        "word-wrap: break-word;z-index:" + (index + 1) + "; height: " + Height + "px;position:absolute;font-size:11px;font-weight:bold;" +
                        "text-align:left; border: 1px solid #9FC6E7;background: " + color + ";opacity:0.6; cursor:pointer;'" +
                        "onclick=editEvent(this,event); onmouseover=mouseOvrEvent(this,event); onmouseout=mouseOutEvent(this,event);>" +
                        dtEvents.Rows[index]["LessonPlanName"].ToString() + "</div>";
                innerDiv = innerDiv + divEvent;

            }
        return innerDiv;
    }
    [WebMethod]
    public static int SaveEvent(string from, string to, string studId, string LPid, string mode, string selDate, string othSchDesc)
    {
        clsData oData = new clsData();
        int rtrn = 0;
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (oSession != null)
        {
            string setOthDsc = othSchDesc;
            if (oSession != null)
            {
                if (mode == "Week")
                {
                    oSession.CrrntDate = selDate;
                    studId = oSession.StudentId.ToString();
                }
                string[] a = from.Split(' ');
                string[] stime = a[0].Split(':');
                string shour = stime[0];
                string smin = stime[1];
                int af;
                int bt;
                if (a[1] == "AM")
                {
                    from=shour+":"+smin;
                }
                else
                {
                    if (int.Parse(shour) != 12)
                    {
                        af = int.Parse(shour) + 12;
                    }
                    else
                        af = int.Parse(shour);
                     if (shour.Length == 1)
                    shour = '0' + shour;
                    from=af.ToString()+":"+smin;
                }
            
                string[] b = to.Split(' ');
                string[] stimet = b[0].Split(':');
                string shourt = stimet[0];
                string smint = stimet[1];

                if (b[1] == "AM" && shourt != "12")
                {
                    to=shourt+":"+smint;
                }
                else if (b[1] == "AM" && shourt == "12")
                {
                    af = int.Parse(shourt) - 12;
                    to = af.ToString() + ":" + smint;
                }
                else
                {
                    if (int.Parse(shourt) != 12)
                    {
                        af = int.Parse(shourt) + 12;
                    }
                    else
                        af = int.Parse(shourt);
                    
                     if (shourt.Length == 1)
                    shourt = '0' + shourt;
                    to=af.ToString()+":"+smint;
                }

               string ins = "";
               if (setOthDsc != "" && setOthDsc != null)
               {
                   ins = "INSERT INTO StdtLPSched(SchoolId,StdtId,StartTime,LPId,EndTime,Day,CreatedBy,CreatedOn,SchOtherDesc) " +
                      "VALUES(" + oSession.SchoolId + "," + studId + ",'" + from + "'," + LPid + ",'" + to + "','" + oSession.CrrntDate + "'," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100))," + "'" + othSchDesc.ToString() + "')" +
                      "\t\tSELECT SCOPE_IDENTITY();";
               }
               else
               {
                   ins = "INSERT INTO StdtLPSched(SchoolId,StdtId,StartTime,LPId,EndTime,Day,CreatedBy,CreatedOn) " +
                  "VALUES(" + oSession.SchoolId + "," + studId + ",'" + from + "'," + LPid + ",'" + to + "','" + oSession.CrrntDate + "'," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))" +
                  "\t\tSELECT SCOPE_IDENTITY();";
               }
                object val = oData.FetchValue(ins);
                if (val != null)
                    rtrn = Convert.ToInt32(val);
            }
        }
        return rtrn;
    }
    [WebMethod]
    public static int DeleteEvent(string EventID, string studID, string mode)
    {
        clsData oData = new clsData();
        int rtrn = 0;
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (oSession != null)
        {
            if (mode == "Week")
            {
                studID = oSession.StudentId.ToString();
            }
            string del = "DELETE FROM StdtLPSched WHERE StdtLPSchedId=" + EventID + " AND StdtId=" + studID + "";
            rtrn = oData.Execute(del);
        }
        return rtrn;
    }
    [WebMethod]
    public static int UpdateEvent(string EventID, string studID, string endTime, string LPid, string mode, string othSchDescEdit)
    {
        clsData oData = new clsData();
        int rtrn = 0;
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (oSession != null)
        {
            string setOthDscEdit = othSchDescEdit;
            if (mode == "Week")
            {
                studID = oSession.StudentId.ToString();
            }

            string upd = "";
            if (setOthDscEdit != "" && setOthDscEdit != null)
            {
                upd = "UPDATE StdtLPSched SET EndTime='" + endTime + "',LPId=" + LPid + ",ModifiedBy=" + oSession.LoginId + "," +
                      "ModifiedOn=(SELECT convert(varchar, getdate(), 100)),SchOtherDesc = " + "'" + setOthDscEdit + "'" + " WHERE StdtLPSchedId=" + EventID + " AND StdtId=" + studID + "";
            }
            else
            {
                upd = "UPDATE StdtLPSched SET EndTime='" + endTime + "',LPId=" + LPid + ",ModifiedBy=" + oSession.LoginId + "," +
                   "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StdtLPSchedId=" + EventID + " AND StdtId=" + studID + "";
            }
            //string upd = "UPDATE StdtLPSched SET EndTime='" + endTime + "',LPId=" + LPid + ",ModifiedBy=" + oSession.LoginId + "," +
            //    "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StdtLPSchedId=" + EventID + " AND StdtId=" + studID + "";
            rtrn = oData.Execute(upd);
        }
        return rtrn;
    }
    [WebMethod]
    public static string GetValues(string qry)
    {
        clsData oData = new clsData();
        object strtTime = oData.FetchValue(qry);
        if (strtTime != null)
            return strtTime.ToString();
        else
            return "No datas found!! Please update the School Calendar Table.";
    }
    [WebMethod]
    public static string GetValues2(string qry, string getqryid)
    {
        clsData oData = new clsData();

        int qrytest = Convert.ToInt32(getqryid);
        string retResult = "";
        if (qrytest > 0)
        {
            string testqyr = "SELECT LPId FROM StdtLPSched WHERE StdtLPSchedId=" + qrytest;
            int gettestqyr = Convert.ToInt32(oData.FetchValue(testqyr));

            if (gettestqyr > 0)
            {
                object strtTime = oData.FetchValue(qry);
                if (strtTime != null)
                {
                    retResult = strtTime.ToString();
                }
                else
                {
                    retResult = "No datas found!! Please update the School Calendar Table.";
                }
            }
            else
            {
                if (qry.Contains("LessonPlanName"))
                {
                    qry = "SELECT SchOtherDesc AS LessonPlanName FROM StdtLPSched WHERE StdtLPSchedId = " + qrytest;
                    object strtTime = oData.FetchValue(qry);
                    if (strtTime != null)
                    {
                        retResult = "Other_" + strtTime.ToString();
                    }
                    else
                    {
                        retResult = "No datas found!! Please update the School Calendar Table.";

                    }
                }
                else
                {
                    object strtTime = oData.FetchValue(qry);
                    if (strtTime != null)
                    {
                        retResult = strtTime.ToString();
                    }
                    else
                    {
                        retResult = "No datas found!! Please update the School Calendar Table.";

                    }
                }
            }
        }
        return retResult;
    }
    [WebMethod]
    public static string GetEndTime(DateTime dtDate, string mode, string res)
    {
        clsData oData = new clsData();
        string endTime = "";
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (oSession != null)
        {
            string day = "";
            if (mode == "Week")
                day = dtDate.DayOfWeek.ToString();
            if (mode == "Day")
            {
                if (oSession != null)
                    day = Convert.ToDateTime(oSession.CrrntDate).DayOfWeek.ToString();
            }
            string selEndTime = "";

            if (res.ToString() == "Day")
                selEndTime = "SELECT EndTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=1 AND SchoolId=" + oSession.SchoolId + "";
            if (res.ToString() == "Resident")
                selEndTime = "SELECT EndTime FROM SchoolCal WHERE Weekday='" + day + "' AND ResidenceInd=0 AND SchoolId=" + oSession.SchoolId + "";
            object val = oData.FetchValue(selEndTime);
            if (val != null)
                endTime = val.ToString();
        }
        return endTime;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void btnDay_Click(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];

        if (oSession != null)
        {
            if (oSession.dt_StudList != null)
                oSession.dt_StudList.Clear();

            ddlClass.SelectedIndex = 0;
            ddlStudents.Items.Clear();
            chkStudnts.Items.Clear();

            btnDay.Style.Add("background-color", "#202253");
            btnWeek.Style.Add("background-color", "none");
            Calendar1.VisibleDate = DateTime.Now;
            Calendar1.SelectedDate = DateTime.Now;
            Calendar1.SelectionMode = CalendarSelectionMode.Day;
            oSession.CrrntDate = DateTime.Now.ToString("yyyy-MM-dd");
            hfDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            checkResident();
            Calendr(oSession.CrrntDate);
        }
    }
    protected void btnWeek_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "CallJS", "HideDialog();", true);
        ddlClass.SelectedIndex = ddlPopClass.SelectedIndex;
        oData.ReturnDropDown("SELECT StudentId as Id,StudentFname as Name FROM Student Std INNER JOIN StdtClass SC ON SC.StdtId=Std.StudentId WHERE SC.ClassId=" + ddlClass.SelectedValue + "", ddlStudents);
        ddlStudents.SelectedIndex = ddlPopStdnts.SelectedIndex;
        btnWeek.Style.Add("background-color", "#202253");
        btnDay.Style.Add("background-color", "none");
        Calendar1.VisibleDate = DateTime.Now;
        Calendar1.SelectedDate = DateTime.Now;
        Calendar1.SelectionMode = CalendarSelectionMode.DayWeek;
        System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
        DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
        DayOfWeek today = DateTime.Now.DayOfWeek;
        DateTime Date = DateTime.Today;
        int i = 1, j = 1;

        while (today != DayOfWeek.Sunday)
        {
            Date = DateTime.Today.AddDays(i);
            today = DateTime.Today.AddDays(i).DayOfWeek;
            i++;
        }

        while (today != DayOfWeek.Monday)
        {
            if (i != j)
                Calendar1.SelectedDates.Add(DateTime.Today.AddDays(i - j));
            today = DateTime.Today.AddDays(i - j).DayOfWeek;
            j++;
        }
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (oSession != null)
        {
            oSession.days_4weeks = Calendar1.SelectedDates;
            weekview(Calendar1.SelectedDates);
        }
        checkResident();
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        SelectedDatesCollection dt = Calendar1.SelectedDates;
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (dt.Count == 1)
        {
            if (oSession != null)
            {
                oSession.CrrntDate = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
                hfDate.Value = oSession.CrrntDate;
                Calendr(oSession.CrrntDate);
            }
        }
        else
        {
            if (oSession != null)
            {
                oSession.days_4weeks = dt;
                weekview(dt);
            }
        }
        if (oSession.current_week.Contains(Calendar1.SelectedDates[1]))
        {
            btnReplicate.Visible = false;
        }
        else
        {
            btnReplicate.Visible = true;
        }
    }
    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        if (Calendar1.SelectionMode == CalendarSelectionMode.DayWeek)
        {
            e.Day.IsSelectable = false;
        }
        else
        {
            e.Day.IsSelectable = true;
            if (e.Day.IsSelected == true)
                e.Day.IsSelectable = false;
        }
    }
    protected void Calendar1_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        SelectedDatesCollection dt = Calendar1.SelectedDates;
        oSession = (clsSession)Session["UserSession"];
        if (dt.Count == 1)
        {
            if (oSession != null)
            {
                lblDate.Text = Calendar1.SelectedDate.DayOfWeek.ToString() + " / " + Calendar1.SelectedDate.ToString("dd / MMM");
                oSession.CrrntDate = Calendar1.SelectedDate.ToString("yyyy-MM-dd");
                hfDate.Value = oSession.CrrntDate;
                Calendr(oSession.CrrntDate);
            }
        }
        else
        {
            if (oSession != null)
            {
                oSession.days_4weeks = dt;
                weekview(dt);
            }
        }
    }
    protected void ddlStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        Calendar1.VisibleDate = DateTime.Now;
        Calendar1.SelectedDate = DateTime.Now;
        Calendar1.SelectionMode = CalendarSelectionMode.DayWeek;
        System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
        DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
        DayOfWeek today = DateTime.Now.DayOfWeek;
        DateTime Date = DateTime.Today;
        int i = 1, j = 1;
        while (today != DayOfWeek.Sunday)
        {
            Date = DateTime.Today.AddDays(i);
            today = DateTime.Today.AddDays(i).DayOfWeek;
            i++;
        }
        while (today != DayOfWeek.Monday)
        {
            if (i != j)
                Calendar1.SelectedDates.Add(DateTime.Today.AddDays(i - j));
            today = DateTime.Today.AddDays(i - j).DayOfWeek;
            j++;
        }
        ddlPopStdnts.SelectedIndex = ddlStudents.SelectedIndex;
        oSession = (clsSession)Session["UserSession"];
        if (oSession != null)
        {
            oSession.days_4weeks = Calendar1.SelectedDates;
            weekview(Calendar1.SelectedDates);
        }
        checkResident();
    }
    protected void btnAddStud_Click(object sender, EventArgs e)
    {
        checkResident();
        DataTable dtStudList = new DataTable();
        dtStudList.Rows.Clear();
        dtStudList.Columns.Add("StudentId", typeof(string));
        dtStudList.Columns.Add("StudentFname", typeof(string));

        foreach (ListItem liStud in chkStudnts.Items)
        {
            if (liStud.Selected == true)
            {
                DataRow drStud = dtStudList.NewRow();
                drStud["StudentId"] = liStud.Value;
                drStud["StudentFname"] = liStud.Text.Split(',')[1];
                dtStudList.Rows.Add(drStud);
            }
        }
        oSession = (clsSession)Session["UserSession"];
        if (oSession != null)
        {
            oSession.dt_StudList = dtStudList;
            if (oSession.CrrntDate == "")
                oSession.CrrntDate = DateTime.Now.ToString("yyyy-MM-dd");
            Calendr(oSession.CrrntDate);
        }
    }

    protected void ddlPopClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlPopStdnts.Items.Clear();
        ddlStudents.Items.Clear();
        ddlClass.SelectedIndex = ddlPopClass.SelectedIndex;
        oData.ReturnDropDown("Select std.StudentId as Id,(StudentLname+','+StudentFname) as Name from StdtClass SC inner join Student std on std.StudentId=SC.StdtId where SC.ClassId=" + ddlPopClass.SelectedValue + "", ddlPopStdnts);
        oData.ReturnDropDown("Select std.StudentId as Id,(StudentLname+','+StudentFname) as Name from StdtClass SC inner join Student std on std.StudentId=SC.StdtId where SC.ClassId=" + ddlPopClass.SelectedValue + "", ddlStudents);
        if (ddlStudents.Items.Count > 0)
            ddlStudents.SelectedIndex = 0;
    }
    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (hfMode.Value == "Week")
        {
            ddlStudents.Items.Clear();
            ddlPopClass.SelectedIndex = ddlClass.SelectedIndex;
            oData.ReturnDropDown("Select std.StudentId as Id,(StudentLname+','+StudentFname) as Name from StdtClass SC inner join Student std on std.StudentId=SC.StdtId where SC.ClassId=" + ddlClass.SelectedValue + "", ddlStudents);
            oSession = (clsSession)Session["UserSession"];
            if (oSession != null)
            {
                oSession.days_4weeks = Calendar1.SelectedDates;
                weekview(Calendar1.SelectedDates);
            }
        }
        if (hfMode.Value == "Day")
        {
            chkStudnts.Items.Clear();
            oData.ReturnCheckBoxList("Select std.StudentId as Id,(StudentLname+','+StudentFname) as Name from StdtClass SC inner join Student std on std.StudentId=SC.StdtId where SC.ClassId=" + ddlClass.SelectedValue, chkStudnts);
            oSession = (clsSession)Session["UserSession"];
            if (oSession != null)
            {
                oSession.dt_StudList = oData.ReturnDataTable("SELECT StudentId,StudentFname FROM Student Std INNER JOIN StdtClass SC ON SC.StdtId=Std.StudentId WHERE SC.ClassId=" + ddlClass.SelectedValue, false);
                Calendr(oSession.CrrntDate);
            }
        }
        checkResident();
    }
    protected void btnReplicate_Click(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];
        if (oSession != null)
        {
            if (hfMode.Value == "Week")
            {
                oSession = (clsSession)Session["UserSession"];
                if (oSession != null)
                {
                    for (int index = 0; index < oSession.days_4weeks.Count; index++)
                    {
                        //DateTime date = oSession.days_4weeks[index].Date.AddDays(-7);
                        DateTime date = oSession.days_4weeks[index];
                        string studID = ddlStudents.SelectedValue;
                        studID = oSession.StudentId.ToString();
                        DataTable dtEvents = new DataTable();
                        dtEvents = oData.ReturnDataTable("SELECT StartTime,EndTime,LPId,SchOtherDesc FROM StdtLPSched WHERE StdtId=" + studID + " AND Day='" + date.ToString("yyyy-MM-dd") + "'", false);
                        if (dtEvents != null)
                            foreach (DataRow drEvnt in dtEvents.Rows)
                            {
                                int LPId = Convert.ToInt32(drEvnt["LPId"]);
                                int check = 0;
                                if (LPId == 0)
                                {
                                    check = Convert.ToInt32(oData.FetchValue("SELECT StdtLPSchedId FROM StdtLPSched WHERE StartTime='" + drEvnt["StartTime"].ToString() + "'AND " +
                                        "EndTime='" + drEvnt["EndTime"].ToString() + "' AND LPId=" + drEvnt["LPId"].ToString() + " AND StdtId=" + studID + " AND " +
                                        "Day='" + oSession.current_week[index].Date.ToString("yyyy-MM-dd") + "' AND SchOtherDesc= '" + drEvnt["SchOtherDesc"].ToString() + "'"));
                                }
                                else {
                                    check = Convert.ToInt32(oData.FetchValue("SELECT StdtLPSchedId FROM StdtLPSched WHERE StartTime='" + drEvnt["StartTime"].ToString() + "'AND " +
                                        "EndTime='" + drEvnt["EndTime"].ToString() + "' AND LPId=" + drEvnt["LPId"].ToString() + " AND StdtId=" + studID + " AND " +
                                        "Day='" + oSession.current_week[index].Date.ToString("yyyy-MM-dd") + "'"));
                                }
                                 if (check == 0){
                                    
                                    if (LPId==0)
                                    oData.Execute("INSERT INTO StdtLPSched(SchoolId,StdtId,StartTime,EndTime,LPId,Day,CreatedBy,CreatedOn,SchOtherDesc) " +
                                        "VALUES(" + oSession.SchoolId + "," + studID + ",'" + drEvnt["StartTime"].ToString() + "','" + drEvnt["EndTime"].ToString() + "'," +
                                        "" + drEvnt["LPId"].ToString() + ",'" + oSession.current_week[index].Date.ToString("yyyy-MM-dd") + "',1," +
                                        "(SELECT convert(varchar, getdate(), 100)),'" + drEvnt["SchOtherDesc"].ToString() + "')");
                                    else
                                        oData.Execute("INSERT INTO StdtLPSched(SchoolId,StdtId,StartTime,EndTime,LPId,Day,CreatedBy,CreatedOn) " +
                                        "VALUES(" + oSession.SchoolId + "," + studID + ",'" + drEvnt["StartTime"].ToString() + "','" + drEvnt["EndTime"].ToString() + "'," +
                                        "" + drEvnt["LPId"].ToString() + ",'" + oSession.current_week[index].Date.ToString("yyyy-MM-dd") + "',1," +
                                        "(SELECT convert(varchar, getdate(), 100)))");
                                }
                            }
                    }
                    //weekview(oSession.days_4weeks);
                    //weekview(oSession.current_week);
                    LoadData();
                }
            }
            if (hfMode.Value == "Day")
            {
                oSession = (clsSession)Session["UserSession"];
                if (oSession != null)
                {
                    if (oSession.dt_StudList != null)
                        foreach (DataRow drStd in oSession.dt_StudList.Rows)
                        {
                            DateTime dtDate = Convert.ToDateTime(oSession.CrrntDate).AddDays(-7);
                            DataTable dtEvents = new DataTable();
                            dtEvents = oData.ReturnDataTable("SELECT StartTime,EndTime,LPId FROM StdtLPSched WHERE StdtId=" + drStd["StudentId"].ToString() + " AND Day='" + dtDate.ToString("yyyy-MM-dd") + "'", false);
                            if (dtEvents != null)
                                foreach (DataRow drEvnt in dtEvents.Rows)
                                {
                                    int check = Convert.ToInt32(oData.FetchValue("SELECT StdtLPSchedId FROM StdtLPSched WHERE StartTime='" + drEvnt["StartTime"].ToString() + "'AND " +
                                        "EndTime='" + drEvnt["EndTime"].ToString() + "' AND LPId=" + drEvnt["LPId"].ToString() + " AND StdtId=" + drStd["StudentId"].ToString() + " AND " +
                                        "Day='" + oSession.CrrntDate + "'"));
                                    if (check == 0)
                                        oData.Execute("INSERT INTO StdtLPSched(SchoolId,StdtId,StartTime,EndTime,LPId,Day,CreatedBy,CreatedOn) " +
                                            "VALUES(" + oSession.SchoolId + "," + drStd["StudentId"].ToString() + ",'" + drEvnt["StartTime"].ToString() + "','" + drEvnt["EndTime"].ToString() + "'," +
                                            "" + drEvnt["LPId"].ToString() + ",'" + oSession.CrrntDate + "',1," +
                                            "(SELECT convert(varchar, getdate(), 100)))");
                                }
                        }
                    Calendr(oSession.CrrntDate);
                }
            }
        }

    }
    protected void ShowMessage()
    {
        btnReplicate.Visible = false;
        HtmlTableCell td2 = new HtmlTableCell();
        HtmlTableCell td = new HtmlTableCell();
        td2.Width = "100%";
        td2.Align = "center";
        td2.VAlign = "top";
        td2.BgColor = "#ECE9D8"; td2.Style.Add("font-weight", "Bold");
        td2.Style.Add("font-size", "x-large");
        td2.InnerText = "Please Select a Student";
        td.Width = "100%";
        td.Align = "center";
        td.BgColor = "#ECE9D8"; td.Style.Add("font-weight", "Bold");
        td.InnerText = "No Student Selected";
        tblCalndr.Rows[1].Cells.Add(td2);
        tblCalndr.Rows[0].Cells.Add(td);
        resident.Visible = false;

    }
    protected void checkResident()
    {
        clsData oData = new clsData();
        oSession = (clsSession)Session["UserSession"];
        if (oSession != null)
        {
            string sel = "SELECT ResidenceInd FROM Class WHERE ClassId=" + oSession.Classid;
            object obj = oData.FetchValue(sel);
            int val = new int();
            if (obj != null)
                val = Convert.ToInt32(obj);
            if (val == 0)
            {
                hidRes_Value.Value = "Day";
                btn_day.Style.Add("background-color", "#202253");
                btnResident.Style.Add("background-color", "none");
                resident.Visible = false;
            }
            else
            {
                hidRes_Value.Value = "Resident";
            }
        }
    }
    protected void btn_day_Click(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];
        hidRes_Value.Value = "Day";
        btn_day.Style.Add("background-color", "#202253");
        btnResident.Style.Add("background-color", "none");
        if (hfMode.Value == "Day")
            if (oSession != null)
            {
                Calendr(oSession.CrrntDate);
            }
        if (hfMode.Value == "Week")
        {
            if (oSession != null)
            {
                weekview(oSession.days_4weeks);
            }
        }
    }
    protected void btnResident_Click(object sender, EventArgs e)
    {
        oSession = (clsSession)Session["UserSession"];
        hidRes_Value.Value = "Resident";
        btnResident.Style.Add("background-color", "#202253");
        btn_day.Style.Add("background-color", "none");
        if (hfMode.Value == "Day")
            if (oSession != null)
            {
                Calendr(oSession.CrrntDate);
            }
        if (hfMode.Value == "Week")
        {
            if (oSession != null)
            {
                weekview(oSession.days_4weeks);
            }
        }
    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
    }
}