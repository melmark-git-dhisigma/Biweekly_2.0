using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


public partial class StudentBinder_BehaviarCalc : System.Web.UI.Page
{
    clsData objData = new clsData();
    DataTable Dt = new DataTable();
    DataClass objdataClass = null;
    clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
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
            bool Disable = false;

            //HiddenFieldStudentId.Value = sess.StudentId.ToString();
			Session["ALERT_COUNT"] = 0;	
            Session["CHECKED_ITEMS"]=new ArrayList();
            if (!IsPostBack)
            {
                Disable = LoadData(Disable);

            }
            FillData(Disable);
        }
    }

    private bool LoadData(bool Disable)
    {
        sess = (clsSession)Session["UserSession"];
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
        if (Disable == true)
        {
            ButtonSave.Visible = false;
        }
        else
        {
            ButtonSave.Visible = true;
        }
        fillBehavior();
        fillBehaviorData();
        ViewTab1();
        return Disable;
    }

    public void FillData(bool check)
    {
        Session["ALERT_COUNT"] = 0;
        tdMsg.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        //checkin weather there is vale all ready entered in behavior calc table
        int stdid = sess.SchoolId;
        string strqry = "select BehaviourDetails.MeasurementId from BehaviourDetails inner join BehaviourCalc on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId where BehaviourCalc.StudentId=" + sess.StudentId + " and BehaviourDetails.PartialInterval='True' and BehaviourDetails.ActiveInd='A' and cast(BehaviourCalc.Date as date)=cast(GETDATE() as date) and BehaviourCalc.ActiveInd='A' and BehaviourCalc.IsPartial='True'";
        if (objData.IFExists("select BehaviourDetails.MeasurementId from BehaviourDetails inner join BehaviourCalc on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId where BehaviourCalc.StudentId=" + sess.StudentId + " and BehaviourDetails.PartialInterval='True' and BehaviourDetails.ActiveInd='A' and cast(BehaviourCalc.Date as date)=cast(GETDATE() as date) and BehaviourCalc.ActiveInd='A' and BehaviourCalc.IsPartial='True'") == false)
        {
            tdMsg.Width = "800px";
            tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found For This Date ");

            ButtonSave.Visible = false;
            //chkAllBox.Visible = false;
			check_All.Visible = false;
            ButtonOn.Enabled = false;
            ButtonOff.Enabled = false;
            ButtonOn.Style.Add("background-color", "#a7a7a7");
            noDataMsg.Visible = true;
        }
        else
        {
            if (check == true)
            {
                ButtonSave.Visible = false;
            }
            else
            {
                ButtonSave.Visible = true;
            }
            createTablesWithData();
            noDataMsg.Visible = false;
        }
		ArrayList Remainder = new ArrayList();	
        Remainder = (ArrayList)Session["CHECKED_ITEMS"];	
        int ReminderCount = 0;// Remainder.Count;	
        var AlertCount = Session["ALERT_COUNT"];	
        if (check_All.Checked)	
            SelectAllBehavr();	
        else if ((Convert.ToInt32(ReminderCount).Equals(Convert.ToInt32(AlertCount))) && check_All.Checked == false)	
            DeselectAllBehavr();
    }
    public void fillBehaviorData()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (objData.IFExists("SELECT BehaviourCalcId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False' AND ActiveInd='A'") == true)
        {
            DataTable DTbhr = objData.ReturnDataTable("SELECT MeasurmentId,StartTime,EndTime,IOAUser FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False'", false);
            if (DTbhr != null)
            {
                foreach (DataRow dr in DTbhr.Rows)
                {
                    int MeasureId = Convert.ToInt32(dr["MeasurmentId"].ToString());
                    string starttime = dr["StartTime"].ToString();
                    string endtime = dr["EndTime"].ToString();
                    int IOA = Convert.ToInt32(dr["IOAUser"].ToString());
                    foreach (GridViewRow row in grd_Behavior.Rows)
                    {
                        DropDownList ddlHourstart = (DropDownList)row.FindControl("ddlHourstart");
                        DropDownList ddlMinutestart = (DropDownList)row.FindControl("ddlMinutestart");
                        DropDownList ddlSecondstart = (DropDownList)row.FindControl("ddlSecondstart");
                        DropDownList ddlHourEnd = (DropDownList)row.FindControl("ddlHourEnd");
                        DropDownList ddlMinuteEnd = (DropDownList)row.FindControl("ddlMinuteEnd");
                        DropDownList ddlSecondEnd = (DropDownList)row.FindControl("ddlSecondEnd");
                        DropDownList DropDownListAMPMstart = (DropDownList)row.FindControl("DropDownListAMPMstart");
                        DropDownList DropDownListAMPMEnd = (DropDownList)row.FindControl("DropDownListAMPMEnd");
                        DropDownList DdlUSER = (DropDownList)row.FindControl("ddlUser");
                        ImageButton DeleteText = (ImageButton)row.FindControl("lb_delete");
                        int BehaviorId = Convert.ToInt32(grd_Behavior.DataKeys[row.RowIndex].Value);
                        if (BehaviorId == MeasureId)
                        {
                            FillStartEndTime(starttime, DropDownListAMPMstart, ddlHourstart, ddlMinutestart, ddlSecondstart);
                            FillStartEndTime(endtime, DropDownListAMPMEnd, ddlHourEnd, ddlMinuteEnd, ddlSecondEnd);
                            DdlUSER.SelectedValue = IOA.ToString();
                        }

                    }
                }
            }
        }
    }


    protected void grd_Behavior_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_Behavior.PageIndex = e.NewPageIndex;
        fillBehavior();
    }

    private static void FillStartEndTime(string starttime, DropDownList DropDownListAMPMstart, DropDownList ddlHour, DropDownList ddlMinute, DropDownList ddlSecond)
    {
        string[] Alltime = starttime.Split(':');
        string h = Convert.ToString(Alltime[0]);
        string m = Convert.ToString(Alltime[1]);
        string s = Convert.ToString(Alltime[2]);
        int hour = Convert.ToInt32(h);
        if (Convert.ToInt32(h) > 12)
        {
            DropDownListAMPMstart.SelectedIndex = 1;
            hour = Convert.ToInt32(h) - 12;
        }
        else
        {
            DropDownListAMPMstart.SelectedIndex = 0;
        }
        if (hour.ToString().Length == 1)
        {
            ddlHour.SelectedIndex = ddlHour.Items.IndexOf(ddlHour.Items.FindByText("0" + hour.ToString()));
        }
        else
        {
            ddlHour.SelectedIndex = ddlHour.Items.IndexOf(ddlHour.Items.FindByText(hour.ToString()));
        }
        ddlMinute.SelectedIndex = ddlMinute.Items.IndexOf(ddlMinute.Items.FindByText(m));
        ddlSecond.SelectedIndex = ddlSecond.Items.IndexOf(ddlSecond.Items.FindByText(s));
    }
    public void fillBehavior()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        string StrQuery = "SELECT MeasurementId,Behaviour FROM [dbo].[BehaviourDetails] WHERE StudentId=" + sess.StudentId + " AND SchoolId=" + sess.SchoolId + " AND ClassId=" + sess.Classid + " AND PartialInterval='False' AND ActiveInd='A' AND (Frequency <>0 OR Duration <>0)";
        DataTable dtbehavior = objData.ReturnDataTable(StrQuery, false);
        if (dtbehavior != null)
        {
            btnSave.Visible = true;
            grd_Behavior.DataSource = dtbehavior;
            grd_Behavior.DataBind();
        }
        else
        {
            btnSave.Visible = false;
        }
    }

    public void loadTime(DropDownList dr)
    {
        //to load time to drpdownlist   
        // i Counter
        for (int i = 1; i <= 12; i++)
        {
            for (int j = 0; j <= 50; j = j + 10)
            {
                if (j == 0)
                    dr.Items.Add(i + ":00");

                else
                    dr.Items.Add(i + ":" + j);
            }
        }

    }
    public int retSelIndex(string val, DropDownList dr)
    {
        //check the selected index of a dropdownlist
        int i = 0;

        // flg to check the index is present
        int flg = 0;
        for (i = 0; i < dr.Items.Count; i++)
        {
            DateTime itTime = DateTime.Parse(dr.Items[i].Text);
            DateTime valTime = DateTime.Parse(val);
            if (itTime.ToShortTimeString() == valTime.ToShortTimeString())
            {
                flg = 1;
                break;
            }
        }
        if (flg == 1)
        {
            return i;
        }
        else
        {
            return 0;
        }

    }

    public int retSelIndexForIoa(int val, DropDownList dr)
    {
        //check the selected index of a dropdownlist
        int i = 0;

        // flg to check the index is present
        int flg = 0;
        for (i = 0; i < dr.Items.Count; i++)
        {

            if (int.Parse(dr.Items[i].Value) == val)
            {
                flg = 1;
                break;
            }
        }
        if (flg == 1)
        {
            return i;
        }
        else
        {
            return 0;
        }

    }

    public void createTablesWithData()
    {
        //Table table = new Table();
        //table.ID = "Table1";
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();



        //DismissStatus
        //string strQuery = "select BehaviourCalc.BehaviourCalcId,BehaviourDetails.MeasurementId,BehaviourDetails.Behaviour,BehaviourDetails.Period,BehaviourCalc.StartTime,BehaviourCalc.EndTime,NumOfTimes,BehaviorReminder.BehaviourReminderId,BehaviorReminder.DismissStatus,BehaviourCalc.IOAFlag,BehaviourCalc.IOAUser from BehaviourDetails inner join BehaviourCalc  on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId left join BehaviorReminder on BehaviourCalc.BehaviourCalcId=BehaviorReminder.BehaviourCalcId where BehaviourCalc.StudentId=" + sess.StudentId + " and DateDiff(day, BehaviourCalc.Date, getdate()) = 0 and BehaviourCalc.ActiveInd='A' and BehaviourDetails.PartialInterval='True' and BehaviourCalc.IsPartial='True'";
        //Dt = objData.ReturnDataTable(strQuery, true);

        string Behaviorid = "SELECT MeasurementId,Behaviour FROM BehaviourDetails WHERE MeasurementId IN (SELECT DISTINCT MeasurmentId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE())) AND ActiveInd='A'";
        Dt = objData.ReturnDataTable(Behaviorid, false);

        DataList1.DataSource = Dt;
        DataList1.DataBind();

        //PlaceHolderTime.Controls.Add(table);

        //LabelHidden.Text = "";
        //LabelHiddenNo.Text = "";

        //int check = 0;
        //int count = 0;
        //foreach (DataRow dr in Dt.Rows)
        //{
        //    if (check != int.Parse(dr["MeasurementId"].ToString()))
        //    {
        //        TableCell cell1 = new TableCell();

        //        TableRow row1 = new TableRow();

        //        //Heading Lable
        //        Label lblHeading = new Label();
        //        lblHeading.ID = "Lable_Head" + dr["MeasurementId"].ToString();
        //        lblHeading.Text = dr["Behaviour"].ToString();
        //        lblHeading.CssClass = "head_box";
        //        //BehaviourCalcId
        //        //  HidFldBehaveCalcId.Value = HidFldBehaveCalcId.Value + dr["BehaviourCalcId"].ToString() + ",";
        //        //lblHeading.Width = 790;
        //        lblHeading.EnableViewState = true;

        //        cell1.Controls.Add(lblHeading);
        //        row1.Cells.Add(cell1);
        //        table.Rows.Add(row1);

        //        //Hidden lable to keep mesurment Id's
        //        LabelHidden.Text = LabelHidden.Text + dr["MeasurementId"].ToString() + ",";
        //        LabelHiddenNo.Text = LabelHiddenNo.Text + count.ToString() + ",";
        //        count = 0;
        //        check = int.Parse(dr["MeasurementId"].ToString());

        //    }

        //    // Start Time for the behaviour
        //    DateTime StartTime = new DateTime();

        //    TableCell cell = new TableCell();

        //    TableRow row = new TableRow();
        //    row.ID = "tbRow" + count.ToString() + dr["MeasurementId"].ToString();
        //    row.EnableViewState = true;

        //    //************* Start time lable and dropdown creted dynamically ************** 
        //    Label lblStartTime = new Label();
        //    lblStartTime.ID = "Lable_" + count.ToString() + dr["MeasurementId"].ToString();
        //    lblStartTime.Text = "Start Time";
        //    lblStartTime.Width = 80;
        //    lblStartTime.EnableViewState = true;

        //    //lable to keep behaveCalcId

        //    HiddenField hdFldBeHaveId = new HiddenField();
        //    hdFldBeHaveId.ID = "HdFld_BehaveCalc" + count.ToString() + dr["MeasurementId"].ToString();
        //    hdFldBeHaveId.Value = dr["BehaviourCalcId"].ToString();
        //    // HdnFldBehavCalcId.Value =HdnFldBehavCalcId.Value + dr["MeasurementId"].ToString()+",";





        //    DropDownList dRpStart = new DropDownList();
        //    dRpStart.ID = "DrpStartTime" + count.ToString() + dr["MeasurementId"].ToString();
        //    dRpStart.CssClass = "drpClass drpStart";
        //    dRpStart.Width = 150;
        //    loadTime(dRpStart);
        //    dRpStart.Attributes.Add("onchange", "return fn_checkTime(this,'drpStart')");
        //    dRpStart.Attributes.Add("onfocus", "dropFocus(this)");


        //    DropDownList dRpStartAMPM = new DropDownList();
        //    dRpStartAMPM.ID = "DrpStartTimeAMPM" + count.ToString() + dr["MeasurementId"].ToString();
        //    dRpStartAMPM.Items.Add("AM");
        //    dRpStartAMPM.Items.Add("PM");
        //    dRpStartAMPM.Width = 80;
        //    dRpStartAMPM.CssClass = "drpClass drpStartAP";
        //    dRpStartAMPM.Attributes.Add("onchange", "return fn_checkTime(this,'drpStartAP')");
        //    dRpStartAMPM.Attributes.Add("onfocus", "dropFocus(this)");


        //    StartTime = DateTime.Parse(dr["StartTime"].ToString());
        //    if (StartTime.Hour > 12)
        //    {
        //        dRpStartAMPM.SelectedIndex = 1;
        //        StartTime = StartTime.AddHours(12);
        //        dRpStart.SelectedIndex = retSelIndex(StartTime.ToShortTimeString(), dRpStart);


        //    }
        //    else
        //    {

        //        string s = StartTime.ToShortTimeString();
        //        dRpStart.SelectedIndex = retSelIndex(StartTime.ToShortTimeString(), dRpStart);

        //    }

        //    TextBox txtStartime = new TextBox();
        //    txtStartime.ID = "txtStTime" + count.ToString() + dr["MeasurementId"].ToString();
        //    // txtStartime.Text = StartTime.ToLongTimeString();
        //    txtStartime.Text = dr["StartTime"].ToString();
        //    txtStartime.Width = 80;
        //    txtStartime.EnableViewState = true;
        //    txtStartime.Attributes.Add("Class", "txtTime");
        //    txtStartime.Attributes.Add("onblur", "blurFortxt(this.id)");

        //    //*************************** 



        //    //************* End time lable and dropdown creted dynamically ************** 
        //    Label lblEndTime = new Label();
        //    lblEndTime.ID = "Lable_end" + count.ToString() + dr["MeasurementId"].ToString();
        //    lblEndTime.Text = "End Time";
        //    lblEndTime.Width = 80;
        //    lblEndTime.EnableViewState = true;

        //    DropDownList dRpEndTime = new DropDownList();
        //    dRpEndTime.ID = "dRpEndTime" + count.ToString() + dr["MeasurementId"].ToString();
        //    loadTime(dRpEndTime);
        //    dRpEndTime.CssClass = "drpClass drpEnd";
        //    dRpEndTime.Width = 150;
        //    dRpEndTime.Attributes.Add("onchange", "return fn_checkTime(this,'drpEnd')");
        //    dRpEndTime.Attributes.Add("onfocus", "dropFocus(this)");

        //    DropDownList dRpEndTimeAMPM = new DropDownList();
        //    dRpEndTimeAMPM.ID = "dRpEndTimeAMPM" + count.ToString() + dr["MeasurementId"].ToString();
        //    dRpEndTimeAMPM.Items.Add("AM");
        //    dRpEndTimeAMPM.Items.Add("PM");
        //    dRpEndTimeAMPM.CssClass = "drpClass drpEndAP";
        //    dRpEndTimeAMPM.Width = 80;
        //    dRpEndTimeAMPM.Attributes.Add("onchange", "return fn_checkTime(this,'drpEndAP')");

        //    dRpEndTimeAMPM.Attributes.Add("onfocus", "dropFocus(this)");


        //    //tmie in drp dynamic
        //    StartTime = DateTime.Parse(dr["EndTime"].ToString());
        //    if (StartTime.Hour > 12)
        //    {
        //        dRpEndTimeAMPM.SelectedIndex = 1;
        //        StartTime = StartTime.AddHours(12);
        //        dRpEndTime.SelectedIndex = retSelIndex(StartTime.ToShortTimeString(), dRpEndTime);


        //    }
        //    else
        //    {
        //        //DropDownListTime.SelectedValue = sTime.ToShortTimeString();
        //        string s = StartTime.ToShortTimeString();
        //        dRpEndTime.SelectedIndex = retSelIndex(StartTime.ToShortTimeString(), dRpEndTime);

        //    }

        //    TextBox txtEndtime = new TextBox();
        //    txtEndtime.ID = "txtEndtime" + count.ToString() + dr["MeasurementId"].ToString();
        //    //txtEndtime.Text = StartTime.ToLongTimeString();
        //    txtEndtime.Text = dr["EndTime"].ToString();
        //    txtEndtime.Width = 80;
        //    txtEndtime.EnableViewState = true;
        //    txtEndtime.Attributes.Add("Class", "txtTime");
        //    txtEndtime.Attributes.Add("onblur", "blurFortxt(this.id)");

        //    //*************************** 


        //    //************ Delete Button created dynamically ***********
        //    Image img = new Image();
        //    img.ImageUrl = "~/Administration/images/trash.png";
        //    img.ID = "" + count.ToString() + dr["MeasurementId"].ToString();
        //    img.CssClass = "btn btn-red";
        //    img.Attributes.Add("onclick", "delDynRow(this.id)");
        //    img.Attributes.Add("style", "Top:10px");
        //    img.EnableViewState = true;

        //    HiddenFieldtxtIdz.Value = HiddenFieldtxtIdz.Value + count.ToString() + dr["MeasurementId"].ToString() + ",";
        //    //******************************    
        //    //*******************period*********
        //    //  Period
        //    HiddenField Hdperiod = new HiddenField();
        //    Hdperiod.ID = "hdPeriod" + count.ToString() + dr["MeasurementId"].ToString();
        //    //txtEndtime.Text = StartTime.ToLongTimeString();
        //    Hdperiod.Value = dr["Period"].ToString();


        //    //*************************************

        //    //************* Reminder Checkbox Created dynamically *********
        //    CheckBox chek = new CheckBox();
        //    chek.Text = "Remind Me";
        //    chek.ID = "ChkReminder" + count.ToString() + dr["MeasurementId"].ToString();
        //    chek.EnableViewState = true;
        //    chek.Attributes.Add("Class", "chkRem");
        //    // checking weather if reminder present or not  DismissStatus
        //    if ((dr["BehaviourReminderId"].ToString() == "") || (dr["DismissStatus"].ToString() != "True"))
        //    {
        //        chek.Checked = false;
        //    }
        //    else
        //    {
        //        chek.Checked = true;
        //    }
        //    //************************************


        //    //*********** IOA Checkbox and DropDownList *************
        //    CheckBox chekIoa = new CheckBox();
        //    chekIoa.Text = "IOA";
        //    chekIoa.ID = "ChkIOA," + count.ToString() + dr["MeasurementId"].ToString();
        //    //  chekIoa.EnableViewState = true ;
        //    chekIoa.Attributes.Add("onclick", "IOAMarker(this.id)");



        //    DropDownList DllUsers = new DropDownList();
        //    DllUsers.ID = "dRpUserName" + count.ToString() + dr["MeasurementId"].ToString();
        //    objData.ReturnDropDown("select UserId as Id, UserFName+' '+UserLName as Name  from [User] where ActiveInd='A'", DllUsers);
        //    DllUsers.CssClass = "drpClass drpEnd";
        //    DllUsers.Height = 23;



        //    if (dr["IOAFlag"].ToString() == "True")
        //    {
        //        chekIoa.Checked = true;
        //        DllUsers.Attributes.Add("style", "Display:inline");
        //        DllUsers.SelectedIndex = retSelIndexForIoa(int.Parse(dr["IOAUser"].ToString()), DllUsers);
        //    }
        //    else
        //    {
        //        DllUsers.Attributes.Add("style", "Display:none");
        //    }


        //    //*******************************************************

        //    //lables for space
        //    Label lblplace = new Label();
        //    lblplace.Width = 20;
        //    Label lblplace2 = new Label();
        //    lblplace2.Width = 10;
        //    Label lblplace3 = new Label();
        //    lblplace3.Width = 10;
        //    Label lblplace4 = new Label();
        //    lblplace4.Width = 10;
        //    Label lblplace5 = new Label();
        //    lblplace5.Width = 10;
        //    Label lblplace6 = new Label();
        //    lblplace6.Width = 10;
        //    Label lblplace7 = new Label();
        //    lblplace7.Width = 10;


        //    // Adding all controlz to table
        //    cell.Controls.Add(lblStartTime);

        //    cell.Controls.Add(hdFldBeHaveId);

        //    //  cell.Controls.Add(dRpStart);
        //    //  cell.Controls.Add(lblplace3);
        //    //     cell.Controls.Add(dRpStartAMPM);
        //    cell.Controls.Add(txtStartime);


        //    cell.Controls.Add(lblplace);
        //    cell.Controls.Add(lblEndTime);

        //    //  cell.Controls.Add(dRpEndTime);
        //    //  cell.Controls.Add(lblplace4);
        //    //  cell.Controls.Add(dRpEndTimeAMPM);

        //    cell.Controls.Add(txtEndtime);

        //    cell.Controls.Add(lblplace5);

        //    cell.Controls.Add(img);
        //    cell.Controls.Add(lblplace2);
        //    cell.Controls.Add(chek);

        //    cell.Controls.Add(lblplace6);
        //    cell.Controls.Add(chekIoa);
        //    cell.Controls.Add(lblplace7);
        //    cell.Controls.Add(DllUsers);
        //    cell.Controls.Add(Hdperiod);


        //    row.Cells.Add(cell);
        //    table.Rows.Add(row);
        //    count++;

        //}
        //LabelHiddenNo.Text = LabelHiddenNo.Text + count.ToString() + ",";
    }

    protected void SaveBehaviorRemainder()
    {
        ViewTab1();
        ArrayList Remainder = (ArrayList)Session["CHECKED_ITEMS"];
        ArrayList ioachk = (ArrayList)Session["CHECKED_IOA"];
        Hashtable chkIOAUsr = (Hashtable)Session["CHECKED_IOAUSR"];
        SqlTransaction Transs = null;
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        objdataClass = new DataClass();
        clsData.blnTrans = true;
        SqlConnection con = objData.Open();
        Transs = con.BeginTransaction();
        try
        {
            
            if (ioachk != null)
            {
                string IOAStatus = "UPDATE BehaviourCalc SET IOAFlag='false' WHERE StudentId=" + sess.StudentId + " AND Date=CONVERT(DATE,GETDATE())";
                objData.ExecuteWithTrans(IOAStatus, con, Transs);

                foreach (var ioa in ioachk)
                {
                    string UPDATECALC = "UPDATE BehaviourCalc SET IOAFlag='true' WHERE BehaviourCalcId=" + ioa;
                    objData.ExecuteWithTrans(UPDATECALC, con, Transs);
                }
            }

            if (Remainder != null)
            {
                string deRemider = " DELETE FROM BehaviorReminder WHERE StudentId=" + sess.StudentId;  // UserId=" + sess.LoginId + " and
                objData.ExecuteWithTrans(deRemider, con, Transs);
                if (Remainder.Count > 0)
                {
                    ButtonOn.Style.Add("background-color", "#a7a7a7");
                    ButtonOff.Style.Add("background-color", "#4CAF50");
                    status.InnerHtml = "Current Status: Timers On";
                }
                else
                {
                    ButtonOff.Style.Add("background-color", "#a7a7a7");
                    ButtonOn.Style.Add("background-color", "#4CAF50");
                    status.InnerHtml = "Current Status: Timers Off";
                }
                foreach (var remainder in Remainder)
                {
                    if (Convert.ToInt32(objData.FetchValueTrans("SELECT COUNT(*) FROM BehaviorReminder WHERE UserId=" + sess.LoginId + " AND  StudentId=" + sess.StudentId + " AND BehaviourCalcId=" + remainder, Transs, con)) == 0)
                    {
                        string Query = "INSERT INTO BehaviorReminder(UserId,StudentId,BehaviourCalcId,DismissStatus) values(" + sess.LoginId + "," + sess.StudentId + "," + remainder + ",'true')";
                        objData.ExecuteWithTrans(Query, con, Transs);
                    }
                }
            }

            if (chkIOAUsr != null)
            {
                foreach (DictionaryEntry ioausr in chkIOAUsr)
                {
                    string UPDATECALC = "UPDATE BehaviourCalc SET IOAUser=" + ioausr.Value + " WHERE BehaviourCalcId=" + ioausr.Key;
                    objData.ExecuteWithTrans(UPDATECALC, con, Transs);
                }
            }
            //if (CHKIOA.Checked == true)
            //{
                //if (ddlUser.SelectedIndex == 0)
                //{
                //    ddlUser.Focus();
                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Values For IOA Not Done');", true);
                //    return;
                //}
                //else
                //{
                    //string UPDATECALC = "UPDATE BehaviourCalc SET IOAFlag='" + CHKIOA.Checked + "',IOAUser=" + ddlUser.SelectedItem.Value + " WHERE BehaviourCalcId=" + ImgDelete.CommandArgument;
                    //objData.ExecuteWithTrans(UPDATECALC, con, Transs);
                //}
            //}
            //if (chkRM.Checked == true)
            //{
            //    string Query = "INSERT INTO BehaviorReminder(UserId,StudentId,BehaviourCalcId,DismissStatus) values(" + sess.LoginId + "," + sess.StudentId + "," + ImgDelete.CommandArgument + ",'true')";
            //    objData.ExecuteWithTrans(Query, con, Transs);
            //}
            objData.CommitTransation(Transs, con);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "succesMsg('Student Reminder Saved Successfully');", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "succesMsg('Student Reminder Saved Successfully');", true);
        }
        catch (Exception ex)
        {
            objData.RollBackTransation(Transs, con);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Student Reminder Saving  Failed!');", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Student Reminder Saving  Failed!');", true);
            throw ex;
        }
        FillData(false);
    }


    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        SaveBehaviorRemainder();
        //ViewTab1();
        //string[] mIdz = LabelHidden.Text.Split(',');
        //string[] NoOfTimes = LabelHiddenNo.Text.Split(',');
        //SqlTransaction Transs = null;
        //int chkFlagBehaveNull = 0;
        //for (int j = 0; j < mIdz.Length - 1; j++)
        //{
        //    for (int i = 0; i < int.Parse(NoOfTimes[j + 1]); i++)
        //    {

        //        TextBox txtStartTime = (TextBox)this.FindControl("txtStTime" + i + int.Parse(mIdz[j]));
        //        TextBox txtEndTime = (TextBox)this.FindControl("txtEndtime" + i + int.Parse(mIdz[j]));
        //        CheckBox chkBxRem = (CheckBox)this.FindControl("ChkReminder" + i + int.Parse(mIdz[j]));
        //        if ((txtStartTime.Text == "") || (txtEndTime.Text == ""))
        //        {
        //            chkFlagBehaveNull++;
        //        }
        //    }
        //}
        //if (chkFlagBehaveNull == 0)
        //{
            //Checking weather data present in Table  
            //if data present Delete and Insert is done 
            //else data is inserted

            //sess = (clsSession)Session["UserSession"];
            //objData = new clsData();
            //objdataClass = new DataClass();
            //clsData.blnTrans = true;
            //SqlConnection con = objData.Open();
            //Transs = con.BeginTransaction();


            //try
            //{
            //    string deRemider = " DELETE FROM BehaviorReminder WHERE StudentId=" + sess.StudentId;  // UserId=" + sess.LoginId + " and
            //    objData.ExecuteWithTrans(deRemider, con, Transs);
            //    foreach (DataListItem itm in DataList1.Items)
            //    {
            //        GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");
            //        grdbehav.AllowPaging = false;
            //        int Measurmtid = Convert.ToInt32(grdbehav.DataKeys[0].Value);
                    //int BehaviourCalc = Convert.ToInt32(objData.FetchValueTrans("SELECT COUNT(*) FROM BehaviourCalc WHERE MeasurmentId=" + Measurmtid + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE()) AND StudentId=" + sess.StudentId + " AND IsPartial='True' ", Transs, con));
                    //for (int i = 0; i < BehaviourCalc; i++)
                    //{
                    //    GridViewRow row = grdbehav.Rows[i];
                //    foreach (GridViewRow row in grdbehav.Rows)
                //    {
                //        CheckBox CHKIOA = (CheckBox)row.FindControl("chkIOA");
                //        DropDownList ddlUser = (DropDownList)row.FindControl("ddlUser0");
                //        ImageButton ImgDelete = (ImageButton)row.FindControl("lb_delete0");
                //        CheckBox chkRM = (CheckBox)row.FindControl("chkRM");
                //        TextBox txtStime = (TextBox)row.FindControl("txtStime");
                //        TextBox txtEtime = (TextBox)row.FindControl("txtEtime");
                //        if (txtStime.Text == "")
                //        {
                //            txtStime.Focus();
                //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Time should not be empty...');", true);
                //            return;
                //        }
                //        else if (txtEtime.Text == "")
                //        {
                //            txtEtime.Focus();
                //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Time should not be empty...');", true);
                //            return;
                //        }
                //        else if (Convert.ToDateTime(amPmTo24hourConverter(txtStime.Text)) > Convert.ToDateTime(amPmTo24hourConverter(txtEtime.Text)))
                //        {
                //            txtStime.Focus();
                //            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Start time should be less than end time...');", true);
                //            return;
                //        }
                //        if (CHKIOA.Checked == true)
                //        {
                //            if (ddlUser.SelectedIndex == 0)
                //            {
                //                ddlUser.Focus();
                //                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Values For IOA Not Done');", true);
                //                return;
                //            }
                //            else
                //            {
                //                string UPDATECALC = "UPDATE BehaviourCalc SET IOAFlag='" + CHKIOA.Checked + "',IOAUser=" + ddlUser.SelectedItem.Value + ",StartTime='" + amPmTo24hourConverter(txtStime.Text) + "',EndTime='" + amPmTo24hourConverter(txtEtime.Text) + "' WHERE BehaviourCalcId=" + ImgDelete.CommandArgument;
                //                objData.ExecuteWithTrans(UPDATECALC, con, Transs);
                //            }
                //        }
                //        if (chkRM.Checked == true)
                //        {
                //            string Query = "INSERT INTO BehaviorReminder(UserId,StudentId,BehaviourCalcId,DismissStatus) values(" + sess.LoginId + "," + sess.StudentId + "," + ImgDelete.CommandArgument + ",'true')";
                //            objData.ExecuteWithTrans(Query, con, Transs);
                //        }
                //    }
                    
                //}



                //string adBcalc = " update BehaviourCalc set ActiveInd='D' where StudentId=" + sess.StudentId + " and DateDiff(day,Date, getdate()) = 0 and IsPartial='True'";
                //objData.ExecuteWithTrans(adBcalc, con, Transs);
                //string deRemider = " delete from BehaviorReminder where UserId=" + sess.LoginId + " and StudentId=" + sess.StudentId;
                //objData.ExecuteWithTrans(deRemider, con, Transs);
                //int compareFlagUpdate1 = 0;
                //int compareFlagUpdate2 = 0;

                //for (int j = 0; j < mIdz.Length - 1; j++)
                //{
                //    for (int i = 0; i < int.Parse(NoOfTimes[j + 1]); i++)
                //    {
                //        objData = new clsData();
                //        int BehaviourCalcId = 0;
                //        TextBox txtStartTime = (TextBox)this.FindControl("txtStTime" + i + int.Parse(mIdz[j]));
                //        TextBox txtEndTime = (TextBox)this.FindControl("txtEndtime" + i + int.Parse(mIdz[j]));
                //        CheckBox chkBxRem = (CheckBox)this.FindControl("ChkReminder" + i + int.Parse(mIdz[j]));

                //        //  DropDownList drpStart = (DropDownList)this.FindControl("DrpStartTime" + i + int.Parse(mIdz[j]));
                //        //  DropDownList drpStartAmPm = (DropDownList)this.FindControl("DrpStartTimeAMPM" + i + int.Parse(mIdz[j]));

                //        //  DropDownList drpend = (DropDownList)this.FindControl("dRpEndTime" + i + int.Parse(mIdz[j]));
                //        //   DropDownList drpendAmPm = (DropDownList)this.FindControl("dRpEndTimeAMPM" + i + int.Parse(mIdz[j]));

                //        HiddenField hdFildBhId = (HiddenField)this.FindControl("HdFld_BehaveCalc" + i + int.Parse(mIdz[j]));

                //        CheckBox chkbIoA = (CheckBox)this.FindControl("ChkIOA," + i + int.Parse(mIdz[j]));
                //        DropDownList drpUserName = (DropDownList)this.FindControl("dRpUserName" + i + int.Parse(mIdz[j]));


                //        //  hdFldBeHaveId.ID = "HdFld_BehaveCalc" + count.ToString() + dr["MeasurementId"].ToString();
                //        //  hdFldBeHaveId.Value = dr["BehaviourCalcId"].ToString();



                //        /* DateTime StartTime = DateTime.Parse(drpStart.SelectedItem.Text);
                //         if (drpStartAmPm.SelectedIndex == 1)
                //             StartTime = StartTime.AddHours(12);

                //         DateTime EndTime = DateTime.Parse(drpend.SelectedItem.Text);
                //         if (drpendAmPm.SelectedIndex == 1)
                //             EndTime = EndTime.AddHours(12);*/


                //        DateTime StartTime = DateTime.Parse(txtStartTime.Text);


                //        DateTime EndTime = DateTime.Parse(txtEndTime.Text);




                //        string testcase = "";

                //        testcase = testcase + i + int.Parse(mIdz[j]);
                //        string[] test = HiddenFieldDelStatus.Value.Split(',');
                //        int DeleteCheckinCounter;
                //        for (DeleteCheckinCounter = 0; DeleteCheckinCounter < test.Length - 1; DeleteCheckinCounter++)
                //        {
                //            if (int.Parse(test[DeleteCheckinCounter]) == int.Parse(testcase))
                //            {
                //                break;
                //            }
                //        }
                //        if (DeleteCheckinCounter == test.Length - 1)
                //        {
                //            int testDeleteCheck = DeleteCheckinCounter;

                //            BehaviourCalcId = int.Parse(hdFildBhId.Value);

                //            int index;

                //            // string add = "insert into BehaviourCalc(MeasurmentId,StudentId,StartTime,EndTime,Date) values(" + int.Parse(mIdz[j]) + "," + sess.StudentId + ",'" + StartTime.TimeOfDay + "','" + EndTime.TimeOfDay + "',(SELECT Convert(Varchar,getdate(),100)))";
                //            string add = "";
                //            int flgUpdate = 0;

                //            if (chkbIoA.Checked == false)
                //            {

                //                add = " update BehaviourCalc set ActiveInd='A',StartTime='" + StartTime.TimeOfDay + "',EndTime='" + EndTime.TimeOfDay + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) ,ModifiedBy=(SELECT Convert(Varchar,getdate(),100)),IOAFlag='false',IsPartial='True' where BehaviourCalcId=" + BehaviourCalcId;
                //                compareFlagUpdate1++;
                //            }
                //            else
                //            {
                //                // drpUserName
                //                if (drpUserName.SelectedIndex != 0)
                //                {
                //                    add = " update BehaviourCalc set ActiveInd='A',StartTime='" + StartTime.TimeOfDay + "',EndTime='" + EndTime.TimeOfDay + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) ,ModifiedBy=(SELECT Convert(Varchar,getdate(),100)),IOAFlag='true',IOAUser=" + drpUserName.SelectedItem.Value + ",IsPartial='True' where BehaviourCalcId=" + BehaviourCalcId;
                //                    compareFlagUpdate1++;
                //                }
                //                else
                //                {
                //                    flgUpdate = 1;
                //                }
                //            }
                //            // update BehaviourCalc set ActiveInd='A',StartTime='" + StartTime.TimeOfDay + "',EndTime='" + EndTime.TimeOfDay + "' where BehaviourCalcId="+BehaviourCalcId+"

                //            //   objData.Execute(add, con, Transs);
                //            if (flgUpdate == 0)
                //            {
                //                objData.ExecuteWithTrans(add, con, Transs);

                //                if (chkBxRem.Checked == true)
                //                {
                //                    string Query = "insert into BehaviorReminder(UserId,StudentId,BehaviourCalcId,DismissStatus) values(" + sess.LoginId + "," + sess.StudentId + "," + BehaviourCalcId + ",'true')";

                //                    index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(Query, con, Transs));

                //                }
                //            }


                //            compareFlagUpdate2++;
                //        }
                //        else
                //        {
                //            int testk = DeleteCheckinCounter;

                //        }


                //    }
                //}


                // LabelMsg.Text = "Student Details Inserted Successfully";

                // tdMsg.InnerHtml = clsGeneral.sucessMsg("Student reminder saved Successfull);

                //if (compareFlagUpdate1 != compareFlagUpdate2)
                //{
                //    objData.RollBackTransation(Transs, con);
                //ErrorMsg
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Values For IOA Not Done');", true);
                //  tdMsg.InnerHtml = clsGeneral.failedMsg("No Data Found For This Date ");                                 

                //}
                //else
                //{
                //objData.CommitTransation(Transs, con);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "succesMsg('Student Reminder Saved Successfully');", true);

                //}

        //    }
        //    catch (Exception Ex)
        //    {
        //        objData.RollBackTransation(Transs, con);

        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Student Reminder Saving  Failed!');", true);
        //        throw Ex;

        //    }
        //}
        //else
        //{
        //    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Time should not be empty...');", true);
        //}


    }


    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        Response.Redirect(Request.Url.AbsoluteUri);
        ViewTab1();
    }
    private void fillHourMinSec(DropDownList ddlHour, DropDownList ddlMinute, DropDownList ddlSecond)
    {
        string hour = "";
        string minute = "";
        string Sec = "";
        for (int hr = 1; hr <= 12; hr++)
        {
            if (hr.ToString().Length == 1)
            {
                hour = "0" + hr.ToString();
            }
            else
            {
                hour = hr.ToString();
            }
            ddlHour.Items.Add(hour);
        }
        for (int min = 0; min <= 60; min++)
        {
            if (min.ToString().Length == 1)
            {
                minute = "0" + min.ToString();
                Sec = "0" + min.ToString();
            }
            else
            {
                minute = min.ToString();
                Sec = min.ToString();
            }

            ddlMinute.Items.Add(minute);
            ddlSecond.Items.Add(Sec);
        }

    }
    protected string TimeConverter(string TimeValue)
    {
        string[] TimeEntry = TimeValue.Split(':');
        string ResultTime = "";
        string hr = "";
        if (Convert.ToInt32(TimeEntry[0]) > 12)
        {
            int Hour = Convert.ToInt32(TimeEntry[0]) - 12;
            if (Hour < 10) hr = "0" + Hour.ToString();
            else hr = Hour.ToString();
            ResultTime = hr + ":" + TimeEntry[1] + ":" + TimeEntry[2] + ":" + "PM";
        }
        else if (Convert.ToInt32(TimeEntry[0]) < 12)
        {
            int Hour = Convert.ToInt32(TimeEntry[0]);
            if (Hour < 10) hr = "0" + Hour.ToString();
            else hr = Hour.ToString();
            ResultTime = hr + ":" + TimeEntry[1] + ":" + TimeEntry[2] + ":" + "AM";
        }
        else
        {
            ResultTime = TimeEntry[0] + ":" + TimeEntry[1] + ":" + TimeEntry[2] + ":" + "PM";
        }
        return ResultTime;

    }
    protected void grd_Behavior_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlUser = (DropDownList)e.Row.FindControl("ddlUser");
            fillUser(ddlUser);
            DropDownList ddlHourstart = (DropDownList)e.Row.FindControl("ddlHourstart");
            DropDownList ddlMinutestart = (DropDownList)e.Row.FindControl("ddlMinutestart");
            DropDownList ddlSecondstart = (DropDownList)e.Row.FindControl("ddlSecondstart");
            DropDownList DropDownListAMPMstart = (DropDownList)e.Row.FindControl("DropDownListAMPMstart");

            DropDownList ddlHourEnd = (DropDownList)e.Row.FindControl("ddlHourEnd");
            DropDownList ddlMinuteEnd = (DropDownList)e.Row.FindControl("ddlMinuteEnd");
            DropDownList ddlSecondEnd = (DropDownList)e.Row.FindControl("ddlSecondEnd");
            DropDownList DropDownListAMPMEnd = (DropDownList)e.Row.FindControl("DropDownListAMPMEnd");
            fillHourMinSec(ddlHourstart, ddlMinutestart, ddlSecondstart);
            fillHourMinSec(ddlHourEnd, ddlMinuteEnd, ddlSecondEnd);

            int BehaviorId = Convert.ToInt32(grd_Behavior.DataKeys[e.Row.RowIndex].Value);
            if (objData.IFExists("SELECT BehaviourCalcId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False' AND ActiveInd='A' AND MeasurmentId=" + BehaviorId + "") == true)
            {
                DataTable dtbehavior = objData.ReturnDataTable("SELECT StartTime,EndTime,IOAUser FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False' AND ActiveInd='A' AND MeasurmentId=" + BehaviorId + "", false);
                if (dtbehavior != null)
                {
                    string STime = TimeConverter(dtbehavior.Rows[0]["StartTime"].ToString());
                    string ETime = TimeConverter(dtbehavior.Rows[0]["EndTime"].ToString());
                    ddlUser.SelectedValue = dtbehavior.Rows[0]["IOAUser"].ToString();
                    string[] StartTime = STime.Split(':');
                    string[] EndTime = ETime.Split(':');
                    ddlHourstart.SelectedValue = StartTime[0].ToString();
                    ddlMinutestart.SelectedValue = StartTime[1].ToString();
                    ddlSecondstart.SelectedValue = StartTime[2].ToString();
                    DropDownListAMPMstart.SelectedValue = StartTime[3].ToString();

                    ddlHourEnd.SelectedValue = EndTime[0].ToString();
                    ddlMinuteEnd.SelectedValue = EndTime[1].ToString();
                    ddlSecondEnd.SelectedValue = EndTime[2].ToString();
                    DropDownListAMPMEnd.SelectedValue = EndTime[3].ToString();
                }
            }
        }
    }
    protected void grd_Behavior_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ViewTab2();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (hdndelete.Value.ToString() == "true")
        {
            int BehaviorId = Convert.ToInt32(e.CommandArgument);
            string strQuery = " UPDATE BehaviourCalc SET ActiveInd='D' WHERE StudentId=" + sess.StudentId + " AND MeasurmentId=" + BehaviorId + " AND [Date]=CONVERT(DATE,GETDATE()) AND IsPartial='False'";
            objData.Execute(strQuery);
        }
        fillBehavior();
        fillBehaviorData();
        hdndelete.Value = "notpartial";
    }
    public void fillUser(DropDownList ddluser)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        objData.ReturnDropDown("SELECT UserId as Id, UserFName+' '+UserLName as Name  FROM [User] WHERE ActiveInd='A' order by UserFName asc ", ddluser);
    }
    private string amPmTo24hourConverter(string time)
    {

        string[] Alltime = time.Split(':');
        int h = Convert.ToInt16(Alltime[0]);
        int m = Convert.ToInt16(Alltime[1]);
        int s = Convert.ToInt16(Alltime[2].Substring(0, 2));
        string startAMPM = Alltime[2].Substring(2, 2);
        if (startAMPM == "PM")
        {
            if (h != 12)
            {
                h += 12;
            }

        }
        else if (startAMPM == "AM")
        {
            if (h == 12)
            {
                h = 00;
            }
        }
        return (h.ToString() + ":" + m.ToString() + ":" + s.ToString());

    }
    private bool Validation(DropDownList ddlhrstart, DropDownList ddlminstart, DropDownList ddlsecstart, DropDownList ddlhrend, DropDownList ddlminend, DropDownList ddlsecend)
    {
        bool result = true;
        if (ddlhrstart.SelectedItem.Text != "Hr" || ddlminstart.SelectedItem.Text != "Min" || ddlsecstart.SelectedItem.Text != "Sec" || ddlhrend.SelectedItem.Text != "Hr" || ddlminend.SelectedItem.Text != "Min" || ddlsecend.SelectedItem.Text != "Sec")
        {
            if (ddlhrstart.SelectedItem.Text == "Hr")
            {
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please select hour");
                ddlhrstart.Focus();
                return result = false;
            }
            else if (ddlminstart.SelectedItem.Text == "Min")
            {
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please select minute");
                ddlminstart.Focus();
                return result = false;
            }
            else if (ddlsecstart.SelectedItem.Text == "Sec")
            {
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please select second");
                ddlsecstart.Focus();
                return result = false;
            }
            else if (ddlhrend.SelectedItem.Text == "Hr")
            {
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please select hour");
                ddlhrend.Focus();
                return result = false;
            }
            else if (ddlminend.SelectedItem.Text == "Min")
            {
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please select minute");
                ddlminend.Focus();
                return result = false;
            }
            else if (ddlsecend.SelectedItem.Text == "Sec")
            {
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Please select second");
                ddlsecend.Focus();
                return result = false;
            }
        }
        else
        {
            return result = false;
        }
        return result;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ViewTab2();
        tdMsg1.InnerHtml = "";
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        foreach (GridViewRow row in grd_Behavior.Rows)
        {
            DropDownList ddlHourstart = (DropDownList)row.FindControl("ddlHourstart");
            DropDownList ddlMinutestart = (DropDownList)row.FindControl("ddlMinutestart");
            DropDownList ddlSecondstart = (DropDownList)row.FindControl("ddlSecondstart");
            DropDownList ddlHourEnd = (DropDownList)row.FindControl("ddlHourEnd");
            DropDownList ddlMinuteEnd = (DropDownList)row.FindControl("ddlMinuteEnd");
            DropDownList ddlSecondEnd = (DropDownList)row.FindControl("ddlSecondEnd");
            DropDownList DropDownListAMPMstart = (DropDownList)row.FindControl("DropDownListAMPMstart");
            DropDownList DropDownListAMPMEnd = (DropDownList)row.FindControl("DropDownListAMPMEnd");

            if (Validation(ddlHourstart, ddlMinutestart, ddlSecondstart, ddlHourEnd, ddlMinuteEnd, ddlSecondEnd) == true)
            {
                string StartTime = amPmTo24hourConverter(ddlHourstart.SelectedItem.Text + ":" + ddlMinutestart.SelectedItem.Text + ":" + ddlSecondstart.SelectedItem.Text + DropDownListAMPMstart.SelectedItem.Text);
                string EndTime = amPmTo24hourConverter(ddlHourEnd.SelectedItem.Text + ":" + ddlMinuteEnd.SelectedItem.Text + ":" + ddlSecondEnd.SelectedItem.Text + DropDownListAMPMEnd.SelectedItem.Text);

                DropDownList DdlUSER = (DropDownList)row.FindControl("ddlUser");
                int BehaviorId = Convert.ToInt32(grd_Behavior.DataKeys[row.RowIndex].Value);

                if (validateNonPartial(StartTime, EndTime, DdlUSER) == true)
                {
                    if (objData.IFExists("SELECT BehaviourCalcId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False' AND ActiveInd='A' AND MeasurmentId=" + BehaviorId + "") == true)
                    {
                        int BCalcID = Convert.ToInt32(objData.FetchValue("SELECT BehaviourCalcId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False' AND ActiveInd='A' AND MeasurmentId=" + BehaviorId + ""));
                        string strQuery = " UPDATE BehaviourCalc SET StudentId=" + sess.StudentId + ",MeasurmentId=" + BehaviorId + ",StartTime=CONVERT(TIME,'" + StartTime + "'),EndTime=CONVERT(TIME,'" + EndTime + "'),[Date]=CONVERT(DATE,GETDATE()),ActiveInd='A',IsPartial='False',ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE(),IOAFlag='True',IOAUser=" + Convert.ToInt32(DdlUSER.SelectedItem.Value) + " WHERE BehaviourCalcId=" + BCalcID + "";
                        objData.Execute(strQuery);
                    }
                    else
                    {
                        if (objData.IFExists("SELECT BehaviourCalcId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False' AND ActiveInd='D' AND MeasurmentId=" + BehaviorId + "") == true)
                        {
                            int BCalcID = Convert.ToInt32(objData.FetchValue("SELECT BehaviourCalcId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND IsPartial='False' AND ActiveInd='D' AND MeasurmentId=" + BehaviorId + ""));
                            string strQuery = " UPDATE BehaviourCalc SET StudentId=" + sess.StudentId + ",MeasurmentId=" + BehaviorId + ",StartTime=CONVERT(TIME,'" + StartTime + "'),EndTime=CONVERT(TIME,'" + EndTime + "'),[Date]=CONVERT(DATE,GETDATE()),ActiveInd='A',IsPartial='False',ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE(),IOAFlag='True',IOAUser=" + Convert.ToInt32(DdlUSER.SelectedItem.Value) + " WHERE BehaviourCalcId=" + BCalcID + "";
                            objData.Execute(strQuery);
                        }
                        else
                        {
                            string strQuery = " INSERT INTO BehaviourCalc(StudentId,MeasurmentId,StartTime,EndTime,[Date],ActiveInd,IsPartial,CreatedBy,CreatedOn,IOAFlag,IOAUser) VALUES " +
                                "(" + sess.StudentId + "," + BehaviorId + ",CONVERT(TIME,'" + StartTime + "'),CONVERT(TIME,'" + EndTime + "'),CONVERT(DATE,GETDATE()),'A','False'," + sess.LoginId + ",GETDATE(),'True'," + Convert.ToInt32(DdlUSER.SelectedItem.Value) + ")";
                            objData.Execute(strQuery);
                        }
                    }
                    tdMsg1.InnerHtml = clsGeneral.sucessMsg("Saved Successfully...");
                }
            }
        }

    }
    public bool validateNonPartial(string StartTime, string EndTime, DropDownList DdlBehavior)
    {
        bool result = true;
        if (StartTime == "Hr:Min:SecAM")
        {
            tdMsg1.InnerHtml = clsGeneral.warningMsg("Please Enter Start Time");
            result = false;
            return result;
        }
        else if (EndTime == "Hr:Min:SecAM")
        {
            tdMsg1.InnerHtml = clsGeneral.warningMsg("Please Enter End Time");
            result = false;
            return result;
        }
        else if (DdlBehavior.SelectedIndex == 0)
        {
            tdMsg1.InnerHtml = clsGeneral.warningMsg("Please Select IOA User");
            DdlBehavior.Focus();
            result = false;
            return result;
        }
        else if (StartTime != "Hr:Min:SecAM" && EndTime != "Hr:Min:SecAM")
        {
            DateTime start = Convert.ToDateTime(StartTime);
            DateTime end = Convert.ToDateTime(EndTime);
            if (start > end)
            {
                tdMsg1.InnerHtml = clsGeneral.warningMsg("Start Time must be Less than End Time");
                result = false;
                return result;
            }
        }
        return result;
    }
    protected void grd_Behavior_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        grd_Behavior.EditIndex = -1;
        grd_Behavior.DataBind();
    }

    protected void btnrefeshing_Click(object sender, ImageClickEventArgs e)
    {
        bool Disable = false;
        Disable = LoadData(Disable);
    }
    protected void Tab1_Click(object sender, EventArgs e)
    {
        ViewTab1();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "$('.txtTime').timeEntry({ showSeconds: true, beforeShow: customRange });", true);
    }

    private void ViewTab1()
    {
        Tab1.CssClass = "Clicked";
        //Tab2.CssClass = "Initial";
        MainView.ActiveViewIndex = 0;
    }
    protected void Tab2_Click(object sender, EventArgs e)
    {
        ViewTab2();
    }

    private void ViewTab2()
    {
        Tab1.CssClass = "Initial";
        //Tab2.CssClass = "Clicked";
        MainView.ActiveViewIndex = 1;
        tdMsg1.InnerHtml = "";
    }

    protected void grd_Behaviorpartial_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        var AlertCount = Session["ALERT_COUNT"];
        ArrayList Remainder = new ArrayList();
        Remainder = (ArrayList)Session["CHECKED_ITEMS"];
        if (hdndelete1.Value.ToString() == "true")
        {
            int BehaviorId = Convert.ToInt32(e.CommandArgument);
            string strQuery = " UPDATE BehaviourCalc SET ActiveInd='D' WHERE StudentId=" + sess.StudentId + " AND BehaviourCalcId=" + BehaviorId + " AND [Date]=CONVERT(DATE,GETDATE()) AND IsPartial='True'";
            objData.Execute(strQuery);
            string RemainderQry = "DELETE FROM BehaviorReminder WHERE StudentId=" + sess.StudentId +" AND BehaviourCalcId=" + BehaviorId ;
            objData.Execute(RemainderQry);
            var ReminderCount = Remainder.Count;
            int alertCount = Convert.ToInt32(AlertCount) - 1;
            if (Convert.ToInt32(ReminderCount).Equals(Convert.ToInt32(alertCount)))
                check_All.Checked = true;
            else
                check_All.Checked = false;
            removeReminderCount(alertCount, BehaviorId);
            createTablesWithData();
        }
        hdndelete1.Value = "false";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "$('.txtTime').timeEntry({ showSeconds: true, beforeShow: customRange });", true);
    }
    protected void grd_Behaviorpartial_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void grd_Behaviorpartial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlUser = (DropDownList)e.Row.FindControl("ddlUser0");
            fillUser(ddlUser);
            CheckBox chkIOA = (CheckBox)e.Row.FindControl("chkIOA");
            HiddenField IOAUser = (HiddenField)e.Row.FindControl("IOAUser");
            if (chkIOA.Checked == true)
            {
                ddlUser.SelectedValue = IOAUser.Value;
                ddlUser.Enabled = true;
            }
        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            HiddenField hdnmeasureid = (HiddenField)e.Item.FindControl("hdnid");
            GridView grd_Behaviorpartial = (GridView)e.Item.FindControl("grd_Behaviorpartial");
            FillGridBehavior(Convert.ToInt32(hdnmeasureid.Value), grd_Behaviorpartial, "");
        }
    }

    private void FillGridBehavior(int hdnmeasureid, GridView grd_Behaviorpartial, string paging)
    {
        objData = new clsData();
        ArrayList Remainder = new ArrayList();
        ArrayList chkIOA = new ArrayList();
        Hashtable chkIOAUsr = new Hashtable();
        sess = (clsSession)Session["UserSession"];
        string BehaviourCalc = "SELECT BehaviourCalcId,MeasurmentId,StudentId,CONVERT(VARCHAR(10),StartTime,108) StartTime,CONVERT(VARCHAR(10),EndTime,108) EndTime,IOAFlag,IOAUser,Date,ActiveInd,IsPartial,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Yes_No,CASE WHEN (SELECT COUNT(*) FROM BehaviorReminder WHERE BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId)=0 THEN 'false' ELSE 'true' END Remainder FROM BehaviourCalc WHERE MeasurmentId=" + hdnmeasureid + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE()) AND StudentId=" + sess.StudentId + " AND IsPartial='True' ORDER BY StartTime ";
        DataTable DT = objData.ReturnDataTable(BehaviourCalc, false);
        foreach (DataRow dr in DT.Rows)
        {
            string starttime = dr["StartTime"].ToString();
            string[] Stime= starttime.Split(':');
            int TimeStart = Convert.ToInt32(Stime[0]);
            if (TimeStart < 12)
            {
                starttime = starttime + "AM";
            }
            else if (TimeStart == 12)
            {
                starttime = starttime + "PM";
            }
            else
            {
                starttime = Convert.ToDateTime(starttime).AddHours(-12).ToString("HH:mm") + ":00PM";
            }

            dr.SetField("StartTime", starttime);


            string Endtime = dr["EndTime"].ToString();
            string[] Etime = Endtime.Split(':');
            int TimeEnd = Convert.ToInt32(Etime[0]);
            if (TimeEnd < 12)
            {
                Endtime = Endtime + "AM";
            }
            else if (TimeEnd == 12)
            {
                Endtime = Endtime + "PM";
            }
            else
            {
                Endtime = Convert.ToDateTime(Endtime).AddHours(-12).ToString("HH:mm") + ":00PM";
            }

            dr.SetField("EndTime", Endtime);
        }
        grd_Behaviorpartial.DataSource = DT;
        grd_Behaviorpartial.DataBind();
		
		int AlertCount = 0;	
        int ReminderCount = 0;	
        int PagingReminderCount = 0;
		
        if (paging != "paging")
        {
            foreach (DataRow row in DT.Rows)
            {
                bool Remainders = Convert.ToBoolean(row["Remainder"]);
                bool IOAFlag = Convert.ToBoolean(row["IOAFlag"]);
                int BehaviourCalcId = Convert.ToInt32(row["BehaviourCalcId"]);
                int IOAUser = 0;
                if (IOAFlag == true)
                {
                    IOAUser = Convert.ToInt32(row["IOAUser"]);
                }
                if (Session["CHECKED_ITEMS"] != null)
                {
                    Remainder = (ArrayList)Session["CHECKED_ITEMS"];
                }
                if (Session["CHECKED_IOA"] != null)
                {
                    chkIOA = (ArrayList)Session["CHECKED_IOA"];
                }
                if (Session["CHECKED_IOAUSR"] != null)
                {
                    chkIOAUsr = (Hashtable)Session["CHECKED_IOAUSR"];
                }
                if (Remainders)
                {
                    if (!Remainder.Contains(BehaviourCalcId))
                        Remainder.Add(BehaviourCalcId);
                }
                else
                {
                    Remainder.Remove(BehaviourCalcId);
                }
                if (IOAFlag)
                {
                    if (!chkIOA.Contains(Convert.ToInt32(BehaviourCalcId)))
                    {
                        chkIOA.Add(Convert.ToInt32(BehaviourCalcId));
                        if (!chkIOAUsr.Contains(Convert.ToInt32(BehaviourCalcId)))
                        chkIOAUsr.Add(Convert.ToInt32(BehaviourCalcId), Convert.ToInt32(IOAUser));
                    }
                }
                else
                {
                    chkIOA.Remove(Convert.ToInt32(BehaviourCalcId));
                    chkIOAUsr.Remove(Convert.ToInt32(BehaviourCalcId));
                }

            }
            if (Remainder != null && Remainder.Count > 0)
            {
                Session["CHECKED_ITEMS"] = Remainder;
            }
            if (chkIOA != null && chkIOA.Count > 0)
            {
                Session["CHECKED_IOA"] = chkIOA;
            }
            if (chkIOAUsr != null && chkIOAUsr.Count > 0)
            {
                Session["CHECKED_IOAUSR"] = chkIOAUsr;
            }
        	
            AlertCount = Convert.ToInt32(Session["ALERT_COUNT"]) + DT.Rows.Count;	
            Session["ALERT_COUNT"] = AlertCount;	
            AlertCount = Convert.ToInt32(Session["ALERT_COUNT"]);	
            ReminderCount = Remainder.Count;	
            if (Convert.ToInt32(ReminderCount).Equals(Convert.ToInt32(AlertCount)))	
            {	
                check_All.Checked = true;
            }
            else
            {
                check_All.Checked = false;
            }

            if (Convert.ToInt32(ReminderCount)>0)
            {
                ButtonOn.Style.Add("background-color", "#a7a7a7");
                ButtonOff.Style.Add("background-color", "#4CAF50");
                status.InnerHtml = "Current Status: Timers On";
            }
            else
            {
                ButtonOff.Style.Add("background-color", "#a7a7a7");
                ButtonOn.Style.Add("background-color", "#4CAF50");
                status.InnerHtml = "Current Status: Timers Off";
            }
            //if (Convert.ToInt32(ReminderCount).Equals(0))	
            //{	
            //    check_All.Checked = true;	
            //    SelectAllBehavr();	
            //}
        }	
        //Session["ALERT_COUNT"] = DT.Rows.Count;	
        AlertCount = Convert.ToInt32(Session["ALERT_COUNT"]);	
        ReminderCount = Remainder.Count;	
        if (ReminderCount.Equals(0) && paging.Equals("paging"))	
        {	
            ArrayList RemCount = (ArrayList)Session["CHECKED_ITEMS"];	
            ReminderCount = RemCount.Count;	
        }
        if (check_All.Checked)	
            SelectAllBehavr();	
        else if ((Convert.ToInt32(ReminderCount).Equals(Convert.ToInt32(AlertCount))||Convert.ToInt32(ReminderCount).Equals(0)) && check_All.Checked == false)	
            DeselectAllBehavr();

    }
    protected void chkIOA_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row1 = (GridViewRow)(((CheckBox)sender).NamingContainer);
        ImageButton behavCalcId = (ImageButton)row1.FindControl("lb_delete0");
        CheckBox chkRM = (CheckBox)row1.FindControl("chkIOA");
        ArrayList chkIOA = new ArrayList();
        if (Session["CHECKED_IOA"] != null)
        {
            chkIOA = (ArrayList)Session["CHECKED_IOA"];
        }
        if (chkRM.Checked)
        {
            if (!chkIOA.Contains(Convert.ToInt32(behavCalcId.CommandArgument)))
                chkIOA.Add(Convert.ToInt32(behavCalcId.CommandArgument));
        }
        else
        {
            chkIOA.Remove(Convert.ToInt32(behavCalcId.CommandArgument));
        }
        if (chkIOA != null && chkIOA.Count > 0)
        {
            Session["CHECKED_IOA"] = chkIOA;
        }
        foreach (DataListItem itm in DataList1.Items)
        {
            GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");
            foreach (GridViewRow row in grdbehav.Rows)
            {
                CheckBox CHKIOA = (CheckBox)row.FindControl("chkIOA");
                DropDownList ddlUser = (DropDownList)row.FindControl("ddlUser0");
                if (CHKIOA.Checked == true)
                {
                    ddlUser.Enabled = true;
                }
                else
                {
                    ddlUser.Enabled = false;
                    ddlUser.SelectedIndex = 0;
                }
            }
        }

    }
    protected void grd_Behaviorpartial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gvOrders = (sender as GridView);
        int returnvalue= SaveCheckedValues(gvOrders);
        if (returnvalue != 1)
        {
            gvOrders.PageIndex = e.NewPageIndex;
            int Measurmtid = Convert.ToInt32(gvOrders.DataKeys[0].Value);
            FillGridBehavior(Measurmtid, gvOrders, "paging");
            PopulateCheckedValues(gvOrders);            
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "$('.txtTime').timeEntry({ showSeconds: true, beforeShow: customRange });", true);
    }

    private void PopulateCheckedValues(GridView gvdetails)
    {
        ArrayList Remainder = (ArrayList)Session["CHECKED_ITEMS"];
        ArrayList ioa = (ArrayList)Session["CHECKED_IOA"];
        Hashtable chkIOAUsr = (Hashtable)Session["CHECKED_IOAUSR"];
        foreach (GridViewRow gvrow in gvdetails.Rows)
        {
            int index = Convert.ToInt32(((ImageButton)gvrow.FindControl("lb_delete0")).CommandArgument);
            if (Remainder != null && Remainder.Count > 0)
            {
                if (Remainder.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkRM");
                    myCheckBox.Checked = true;
                }
				else	
                {	
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkRM");	
                    myCheckBox.Checked = false;	
            }
            }
            if (ioa != null && ioa.Count > 0)
            {
                if (ioa.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkIOA");
                    myCheckBox.Checked = true;
                    DropDownList mydrp = (DropDownList)gvrow.FindControl("ddlUser0");
                    mydrp.Enabled = true;
                }
                if (chkIOAUsr != null && chkIOAUsr.Count > 0)
                {
                    if (chkIOAUsr.Contains(index))
                    {
                        DropDownList mydrp = (DropDownList)gvrow.FindControl("ddlUser0");
                        foreach (DictionaryEntry url in chkIOAUsr)
                        {
                            string v = url.Key.ToString();
                            if (url.Key.ToString().Contains(index.ToString()))
                            {
                                mydrp.SelectedValue = url.Value.ToString();
                                break;
                            }
                        }
                    }
                }
            }
        }

    }

    private int SaveCheckedValues(GridView gvdetails)
    {
        ArrayList Remainder = new ArrayList();
        ArrayList chkIOA = new ArrayList();
        Hashtable chkIOAUsr = new Hashtable();
        int index = -1;
        foreach (GridViewRow gvrow in gvdetails.Rows)
        {
            index = Convert.ToInt32(((ImageButton)gvrow.FindControl("lb_delete0")).CommandArgument);
            bool result = ((CheckBox)gvrow.FindControl("chkRM")).Checked;
            bool IOAresult = ((CheckBox)gvrow.FindControl("chkIOA")).Checked;
            DropDownList IOAUsrresult = ((DropDownList)gvrow.FindControl("ddlUser0"));

            if (Session["CHECKED_ITEMS"] != null)
            {
                Remainder = (ArrayList)Session["CHECKED_ITEMS"];
            }
            if (Session["CHECKED_IOA"] != null)
            {
                chkIOA = (ArrayList)Session["CHECKED_IOA"];
            }
            if (Session["CHECKED_IOAUSR"] != null)
            {
                chkIOAUsr = (Hashtable)Session["CHECKED_IOAUSR"];
            }
            if (result)
            {
                if (!Remainder.Contains(index))
                    Remainder.Add(index);
            }
            else
            {
                Remainder.Remove(index);
            }
            if (IOAresult)
            {
                if (!chkIOA.Contains(index))
                {
                    chkIOA.Add(index);
                    if (IOAUsrresult.SelectedValue != "0")
                    {
                        chkIOAUsr.Add(Convert.ToInt32(index),Convert.ToInt32(IOAUsrresult.SelectedValue));
                    }
                    else
                    {
                        IOAUsrresult.Focus();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "ErrorMsg('Select IOA user');", true);
                        return 1;
                    }
                }
            }
            else
            {
                chkIOA.Remove(index);
                chkIOAUsr.Remove(index);
            }
        }
        if (Remainder != null && Remainder.Count > 0)
        {
            Session["CHECKED_ITEMS"] = Remainder;
        }
        if (chkIOA != null && chkIOA.Count > 0)
        {
            Session["CHECKED_IOA"] = chkIOA;
        }
        if (chkIOAUsr != null && chkIOAUsr.Count > 0)
        {
            Session["CHECKED_IOAUSR"] = chkIOAUsr;
        }
        return 0;
    }
    protected void ddlUser0_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row1 = (GridViewRow)(((DropDownList)sender).NamingContainer);
        ImageButton behavCalcId = (ImageButton)row1.FindControl("lb_delete0");
        DropDownList chkRM = (DropDownList)row1.FindControl("ddlUser0");
        Hashtable chkIOAUsr = new Hashtable();
        if (Session["CHECKED_IOAUSR"] != null)
        {
            chkIOAUsr = (Hashtable)Session["CHECKED_IOAUSR"];
        }
        if (chkRM.SelectedIndex!=0)
        {
            if (!chkIOAUsr.Contains(Convert.ToInt32(behavCalcId.CommandArgument)))
            {
                chkIOAUsr.Add(Convert.ToInt32(behavCalcId.CommandArgument), Convert.ToInt32(chkRM.SelectedValue));
            }
            else
            {
                chkIOAUsr.Remove(Convert.ToInt32(behavCalcId.CommandArgument));
                chkIOAUsr.Add(Convert.ToInt32(behavCalcId.CommandArgument), Convert.ToInt32(chkRM.SelectedValue));
            }
        }
        else
        {
            chkIOAUsr.Remove(Convert.ToInt32(behavCalcId.CommandArgument));
        }
        if (chkIOAUsr != null && chkIOAUsr.Count > 0)
        {
            Session["CHECKED_IOAUSR"] = chkIOAUsr;
        }
    }
    protected void chkRM_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
        ImageButton behavCalcId = (ImageButton)row.FindControl("lb_delete0");
        string BehaCalcId = behavCalcId.CommandArgument.ToString();	
        int BehaviourCalcId = Convert.ToInt32(BehaCalcId);	
        CheckBox chkRM = (CheckBox)row.FindControl("chkRM");        
        ArrayList Remainder = new ArrayList();
        var remove = BehaviourCalcId;	
        if (Session["CHECKED_ITEMS"] != null)
        {
            Remainder = (ArrayList)Session["CHECKED_ITEMS"];
        }
        if (chkRM.Checked)
        {
            if (!Remainder.Contains(BehaviourCalcId))	
                Remainder.Add(BehaviourCalcId);	
        }
        else
        {
            Remainder.Remove(BehaviourCalcId);	
        }
        if (Remainder != null && Remainder.Count > 0)
        {
            Session["CHECKED_ITEMS"] = Remainder;
        }
        var AlertCount = Session["ALERT_COUNT"];	
        var ReminderCount = Remainder.Count;	
        if (Convert.ToInt32(ReminderCount).Equals(Convert.ToInt32(AlertCount)))	
            check_All.Checked = true;	
        else	
            check_All.Checked = false;
    }
        
    public void checkAll(object sender, EventArgs e)	
    {	
        if (check_All.Checked)	
        {	
            ArrayList Remainder = new ArrayList();	
            //foreach (GridViewRow row in grd_Behavior.Rows)	
            //{	
            foreach (DataListItem itm in DataList1.Items)	
            {	
                GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");	
                HiddenField hdnmeasid = (HiddenField)itm.FindControl("hdnid");	
                int hdnmeasureid = Convert.ToInt32(hdnmeasid.Value);	
                sess = (clsSession)Session["UserSession"];	
                string BehaviourCalc = "SELECT BehaviourCalcId,MeasurmentId,StudentId,CONVERT(VARCHAR(10),StartTime,108) StartTime,CONVERT(VARCHAR(10),EndTime,108) EndTime,IOAFlag,IOAUser,Date,ActiveInd,IsPartial,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Yes_No,CASE WHEN (SELECT COUNT(*) FROM BehaviorReminder WHERE BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId)=0 THEN 'false' ELSE 'true' END Remainder FROM BehaviourCalc WHERE MeasurmentId=" + hdnmeasureid + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE()) AND StudentId=" + sess.StudentId + " AND IsPartial='True' ORDER BY StartTime ";	
                DataTable DT = objData.ReturnDataTable(BehaviourCalc, false);	
                if (Session["CHECKED_ITEMS"] != null)	
                {	
                    Remainder = (ArrayList)Session["CHECKED_ITEMS"];	
    }
                foreach (DataRow dr in DT.Rows)	
                {	
                    string BehCalcId = dr["BehaviourCalcId"].ToString();	
                    int BehaviourCalcId = Convert.ToInt32(BehCalcId);	
                    if (!Remainder.Contains(BehaviourCalcId))	
                        Remainder.Add(BehaviourCalcId);	
                }	
                foreach (GridViewRow row in grdbehav.Rows)	
                {	
                    //index = Convert.ToInt32(((ImageButton)row.FindControl("lb_delete0")).CommandArgument);	
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkRM");	
                    myCheckBox.Checked = true;	
                }	
            }	
            if (Remainder != null && Remainder.Count > 0)	
            {	
                Session["CHECKED_ITEMS"] = Remainder;	
            }	
        }	
        else	
        {	
            foreach (DataListItem itm in DataList1.Items)	
            {	
                GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");	
                foreach (GridViewRow row in grdbehav.Rows)	
                {	
                    //index = Convert.ToInt32(((ImageButton)row.FindControl("lb_delete0")).CommandArgument);	
                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkRM");	
                    myCheckBox.Checked = false;	
                }	
            }	
            ArrayList Remainder = new ArrayList();	
            Session["CHECKED_ITEMS"] = Remainder;	
        }	
    }	
    public void SelectAllBehavr()	
    {	
        ArrayList Remainder = new ArrayList();	
        //foreach (GridViewRow row in grd_Behavior.Rows)	
        //{	
        check_All.Checked = true;	
        foreach (DataListItem itm in DataList1.Items)	
        {	
            //var hdnId = itm.FindControl("hdnid");	
            //var id = itm.FindControl("BehaviourCalcId");	
            GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");	
            HiddenField hdnmeasid = (HiddenField)itm.FindControl("hdnid");	
            int hdnmeasureid = Convert.ToInt32(hdnmeasid.Value);	
            sess = (clsSession)Session["UserSession"];	
            string BehaviourCalc = "SELECT BehaviourCalcId,MeasurmentId,StudentId,CONVERT(VARCHAR(10),StartTime,108) StartTime,CONVERT(VARCHAR(10),EndTime,108) EndTime,IOAFlag,IOAUser,Date,ActiveInd,IsPartial,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Yes_No,CASE WHEN (SELECT COUNT(*) FROM BehaviorReminder WHERE BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId)=0 THEN 'false' ELSE 'true' END Remainder FROM BehaviourCalc WHERE MeasurmentId=" + hdnmeasureid + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE()) AND StudentId=" + sess.StudentId + " AND IsPartial='True' ORDER BY StartTime ";	
            DataTable DT = objData.ReturnDataTable(BehaviourCalc, false);	
            foreach (DataRow dr in DT.Rows)	
            {	
                string BehaCalcId = dr["BehaviourCalcId"].ToString();	
                int BehaviourCalcId = Convert.ToInt32(BehaCalcId);	
                if (Session["CHECKED_ITEMS"] != null)	
                {	
                    Remainder = (ArrayList)Session["CHECKED_ITEMS"];	
                }	
                //if (result)	
                //{	
                if (!Remainder.Contains(BehaviourCalcId))	
                    Remainder.Add(BehaviourCalcId);	
            }	
            foreach (GridViewRow row in grdbehav.Rows)	
            {	
                //index = Convert.ToInt32(((ImageButton)row.FindControl("lb_delete0")).CommandArgument);	
                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRM");	
                myCheckBox.Checked = true;	
            }	
        }	
        if (Remainder != null && Remainder.Count > 0)	
        {	
            Session["CHECKED_ITEMS"] = Remainder;	
        }	
    }	
    public void DeselectAllBehavr()	
    {	
        ArrayList Remainder = new ArrayList();	
        int index = -1;	
        //foreach (GridViewRow row in grd_Behavior.Rows)	
        //{	
        check_All.Checked = false;	
        foreach (DataListItem itm in DataList1.Items)	
        {	
            //var hdnId = itm.FindControl("hdnid");	
            //var id = itm.FindControl("BehaviourCalcId");	
            GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");	
            HiddenField hdnmeasid = (HiddenField)itm.FindControl("hdnid");	
            int hdnmeasureid = Convert.ToInt32(hdnmeasid.Value);
            foreach (GridViewRow row in grdbehav.Rows)	
            {	
                //index = Convert.ToInt32(((ImageButton)row.FindControl("lb_delete0")).CommandArgument);	
                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRM");	
                myCheckBox.Checked = false;	
            }	
        }	
        if (Remainder != null && Remainder.Count > 0)	
        {	
            Session["CHECKED_ITEMS"] = Remainder;	
        }	
    }
    public void removeReminderCount(int alertCount,int BehaviorId)
    {
        ArrayList Remainder = new ArrayList();
        Remainder = (ArrayList)Session["CHECKED_ITEMS"];
        Remainder.Remove(BehaviorId);
        Session["CHECKED_ITEMS"] = Remainder;
        Session["ALERT_COUNT"] = alertCount;
    }

    protected void AllTimersOnClick(object sender, EventArgs e)
    {
        //check_All(sender, e);
        ArrayList Remainder = new ArrayList();
        //foreach (GridViewRow row in grd_Behavior.Rows)	
        //{	
        foreach (DataListItem itm in DataList1.Items)
        {
            GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");
            HiddenField hdnmeasid = (HiddenField)itm.FindControl("hdnid");
            int hdnmeasureid = Convert.ToInt32(hdnmeasid.Value);
            sess = (clsSession)Session["UserSession"];
            string BehaviourCalc = "SELECT BehaviourCalcId,MeasurmentId,StudentId,CONVERT(VARCHAR(10),StartTime,108) StartTime,CONVERT(VARCHAR(10),EndTime,108) EndTime,IOAFlag,IOAUser,Date,ActiveInd,IsPartial,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Yes_No,CASE WHEN (SELECT COUNT(*) FROM BehaviorReminder WHERE BehaviorReminder.BehaviourCalcId=BehaviourCalc.BehaviourCalcId)=0 THEN 'false' ELSE 'true' END Remainder FROM BehaviourCalc WHERE MeasurmentId=" + hdnmeasureid + " AND ActiveInd='A' AND Date=CONVERT(DATE,GETDATE()) AND StudentId=" + sess.StudentId + " AND IsPartial='True' ORDER BY StartTime ";
            DataTable DT = objData.ReturnDataTable(BehaviourCalc, false);
            if (Session["CHECKED_ITEMS"] != null)
            {
                Remainder = (ArrayList)Session["CHECKED_ITEMS"];
            }
            foreach (DataRow dr in DT.Rows)
            {
                string BehCalcId = dr["BehaviourCalcId"].ToString();
                int BehaviourCalcId = Convert.ToInt32(BehCalcId);
                if (!Remainder.Contains(BehaviourCalcId))
                    Remainder.Add(BehaviourCalcId);
            }
            foreach (GridViewRow row in grdbehav.Rows)
            {
                //index = Convert.ToInt32(((ImageButton)row.FindControl("lb_delete0")).CommandArgument);	
                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRM");
                myCheckBox.Checked = true;
            }
        }
        if (Remainder != null && Remainder.Count > 0)
        {
            Session["CHECKED_ITEMS"] = Remainder;
        }
        SaveBehaviorRemainder();
        check_All.Checked = true;
    }

    protected void AllTimersOffClick(object sender, EventArgs e)
    {
        //check_All(sender, e);
        foreach (DataListItem itm in DataList1.Items)
        {
            GridView grdbehav = (GridView)itm.FindControl("grd_Behaviorpartial");
            foreach (GridViewRow row in grdbehav.Rows)
            {
                //index = Convert.ToInt32(((ImageButton)row.FindControl("lb_delete0")).CommandArgument);	
                CheckBox myCheckBox = (CheckBox)row.FindControl("chkRM");
                myCheckBox.Checked = false;
            }
        }
        ArrayList Remainder = new ArrayList();
        Session["CHECKED_ITEMS"] = Remainder;
        SaveBehaviorRemainder();
        check_All.Checked = false;
    }

}