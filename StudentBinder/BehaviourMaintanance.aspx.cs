using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;

public partial class StudentBinder_BehaviourMaintanance : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    clsSession Prevsess = null;
    DataClass objdataClass = null;
    bool Disable = false;
    string Duration = "";
    static int lessonPLId = -1;
    clsGeneral clsGeneralSchk = new clsGeneral();
    int preid = 0;
    string preuser = "";
    int SessStudentid = 0;
    int PreClassid = 0;
    string Sessstname = "";
    string curesesid = "";
    string Pagepath = "";
    string ip = "";
    string HostName = "";
    string userAgent = "";
    DateTime now = DateTime.Now;
    DateTime today = DateTime.Today;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["Visibility"] = "0";
        sess = (clsSession)Session["UserSession"];
        Prevsess = (clsSession)Session["PreSession"];

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
        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            preid = Convert.ToInt16(Session["Spreid"]);
            preuser = Session["Spreuser"].ToString();
            SessStudentid = Convert.ToInt16(Session["Sprestid"]);
            PreClassid = Convert.ToInt16(Session["SpreClsid"]);
            Sessstname = Session["Sprestname"].ToString();
            curesesid = this.Session.SessionID.ToString();
            HostName = Dns.GetHostName();
            userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "BehaviourMaintanance:Page_Load";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
        if (!IsPostBack)
        {
            LoadData();
        }
        else
        {
            //  ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);

            //GrdMeasurement.EditIndex = -1;

        }

        curesesid = this.Session.SessionID.ToString();
        HostName = Dns.GetHostName();
        userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        Pagepath = "BehaviourMaintanance:Page_LoadEnd";        
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
        //---------------------------------------------------------------------

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);

        //----------------------------------------------------------------------
    }
    private void fillHourMinSec()
    {
        string hour = "";
        string minute = "";
        string Sec = "";
        for (int hr = 0; hr <= 11; hr++)
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
            ddlEndHr.Items.Add(hour);
        }
        for (int min = 0; min <= 59; min++)
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
            //ddlSecond.Items.Add(Sec);
            ddlEndMin.Items.Add(minute);
        }

    }
    private void LoadData()
    {
        fillHourMinSec();
        tdMsg.InnerHtml = "";
        filldetails();
        lessonPLId = -1;
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        HostName = Dns.GetHostName();
        userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "BehaviourMaintanance:LoadData";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);

        objData = new clsData();



        Panal_Partia.Visible = false;
        //chkYesOrNo.Visible = false;
        // txtStartDate.Text = DateTime.Today.ToShortDateString();
        ButtonAddLessionPlan.Visible = false;
        CheckBoxLessonPlan.Visible = false;
        chkInactive.Checked = false;
        lblCalc.Visible = false;
        rdoSumTotal.Visible = false;
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;


        if (Disable == true)
        {
            btnSave.Visible = false;
            btnCancel.Visible = false;
            if (GrdMeasurement.Rows.Count > 0)
            {
                foreach (GridViewRow rows in GrdMeasurement.Rows)
                {
                    ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                    lb_delete.Visible = false;
                }
            }
        }
        else
        {
            btnSave.Visible = true;
            btnCancel.Visible = false;
            if (GrdMeasurement.Rows.Count > 0)
            {
                foreach (GridViewRow rows in GrdMeasurement.Rows)
                {
                    ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                    lb_delete.Visible = true;
                }
            }
        }
        btnCancel.Visible = false;
    }






    //public int retSelIndexPeriod(string val)
    //{
    //    int i = 0;
    //    int flg = 0;
    //    for (i = 0; i < DropDownListPeriod.Items.Count; i++)
    //    {
    //        // string it = DropDownListTime.Items[i].Text;
    //        if (int.Parse(DropDownListPeriod.Items[i].Text) == int.Parse(val))
    //        {
    //            flg = 1;
    //            break;
    //        }
    //    }
    //    if (flg == 1)
    //    
    //        return i;
    //    }
    //    else
    //    {
    //        return 0;
    //    }

    //}




    protected void chkDuration_CheckedChanged(object sender, EventArgs e)
    {
        chkFrequency.Checked = false;
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;
        if (chkDuration.Checked)
        {
            // chkhr.Enabled = true;
            // chkmin.Enabled = true;
            //  chksec.Enabled = true;
        }
        else
        {
            if (chkYesOrNo.Checked == true && chkFrequency.Checked == false)
            {
                lblCalcOpp.Visible = true;
                chkOpportunities.Visible = true;
            }
            // chkhr.Checked = false;
            // chkmin.Checked = false;
            // chksec.Checked = false;
            //  chkhr.Enabled = false;
            //  chkmin.Enabled = false;
            //  chksec.Enabled = false;
        }
        GrdMeasurement.EditIndex = -1;
    }


    protected void chkOpportunities_CheckedChanged(object sender, EventArgs e)
    {
        if(chkOpportunities.Checked)
        {
            chkDuration.Enabled = false;
            chkFrequency.Enabled = false;
        }
        else
        {
            chkDuration.Enabled = true;
            chkFrequency.Enabled = true;
        }
    }


    private void clearDataReset()
    {
        txtBehaviour.Text = "";
        txtBehavDefinition.Text = "";
        txtBehavStrategy.Text = "";
        txtGoalDesc.Text = "";
        txtbehBasperlvl.Text = "";
        txtbehIEPobjtve.Text = "";
        //ddlSecond.SelectedIndex = 0;

        txtInterval.Text = "";
        txtNoOfTimes.Text = "";
        txtEndsOn.Text = "";
        rdoAcceleration.SelectedValue = "1";

        Panal_Partia.Visible = false;
        //chkYesOrNo.Visible = false;
        ddlRepeat.SelectedIndex = 0;
        //  ddlStudent.SelectedIndex = 0;
        chkDuration.Checked = false;
        chkDuration.Enabled = true;
        chkFrequency.Checked = false;
        chkFrequency.Enabled = true;
        chkYesOrNo.Checked = false;
        chkOpportunities.Checked = false;
        lblCalc.Visible = false;
        rdoSumTotal.Visible = false;
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;
        chkPartial.Checked = false;
        CheckBoxLessonPlan.Checked = false;
        CheckBoxCostLessPlan.Checked = false;
        ButtonAddLessionPlan.Visible = false;
        CheckBoxLessonPlan.Visible = false;
        btnCancel.Visible = false;
        chkInactive.Checked = false;
        CheckBoxLessonPlan.Text = "";

        txtPeriod.Text = "";
        txtStartDate.Text = "";

        if (chk24Hr.Checked == true)
        {
            chk24Hr.Checked = false;
        }
        ddlHour.Enabled = true;
        ddlMinute.Enabled = true;
        ddlEndHr.Enabled = true;
        ddlEndMin.Enabled = true;
        ddlEndFrmt.Enabled = true;
        DropDownListAMPM.Enabled = true;
        ddlEndHr.SelectedIndex = 0;
        ddlEndMin.SelectedIndex = 0;
        ddlMinute.SelectedIndex = 0;
        ddlHour.SelectedIndex = 0;
        ddlEndFrmt.SelectedIndex = 0;
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
            if (h != 12 && h != 0)
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

    protected void btnSubmitIEP1_Click(object sender, EventArgs e)
    {

        //  DateTime txtst = DateTime.Parse(txtStartDate.Text);
        //  DateTime txtend = DateTime.Parse(txtEndsOn.Text);

        int errorflg = 0;
        bool acceleration = true;
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        HostName = Dns.GetHostName();
        userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "BehaviourMaintanance:btnSubmitIEP1_Click";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);

        objData = new clsData();

        tdMsg.InnerHtml = "";
        int flag = 0;
        DateTime StartTime = new DateTime();
        DateTime EndTime = new DateTime();
        if (chkInactive.Checked != true)
        {
            if (chk24Hr.Checked == true)
            {
                ddlEndHr.Enabled = false;
                ddlEndMin.Enabled = false;
                ddlEndFrmt.Enabled = false;

                ddlHour.Enabled = false;
                ddlMinute.Enabled = false;
                DropDownListAMPM.Enabled = false;
            }
            else
            {
                ddlEndHr.Enabled = true;
                ddlEndMin.Enabled = true;
                ddlEndFrmt.Enabled = true;

                ddlHour.Enabled = true;
                ddlMinute.Enabled = true;
                DropDownListAMPM.Enabled = true;
            }

            if (ddlRepeat.SelectedIndex == 1)
            {
                // checkBxWeek.Style.Remove("display");
                checkBxWeek.Style.Add("display", "block");
            }
            else
            {
                checkBxWeek.Style.Add("display", "none");
            }
            if (rdoAcceleration.SelectedItem.Text == "Acceleration")
            {
                acceleration = true;
            }
            else
            {
                acceleration = false;
            }

            if (chkPartial.Checked == true)
            {
                Panal_Partia.Visible = true;
                chkYesOrNo.Visible = true;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
            }

            if (chkPartial.Checked == true)
            {
                if (txtPeriod.Text == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter period...");
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    return;
                }
                if (txtInterval.Text == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter interval...");
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    return;
                }
                if (ddlHour.SelectedItem.Text == "Hr" || ddlMinute.SelectedItem.Text == "Min")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select start time");
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    return;
                }
                if (ddlEndHr.SelectedItem.Text == "Hr" || ddlEndMin.SelectedItem.Text == "Min")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select End Time");
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    return;
                }
                ///date picker validation
                ///
                if (txtStartDate.Text == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select start date");
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    return;
                }
                if (txtEndsOn.Text == "")
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please select end date");
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    return;
                }
                ///end date picker validation
                ///
                try
                {
                    int interval = Convert.ToInt32(txtInterval.Text);
                    int period = Convert.ToInt32(txtPeriod.Text);
                    if (interval < period)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Period should not be greater than Interval");
                        ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                        return;
                    }
                }
                catch
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                }
            }

            try
            {


                if (chkPartial.Checked == true)
                {
                    string strtimetemp = amPmTo24hourConverter(ddlHour.SelectedItem.Text + ":" + ddlMinute.SelectedItem.Text + ":" + "00" + DropDownListAMPM.SelectedItem.Text);
                    string endTimeTemp = amPmTo24hourConverter(ddlEndHr.SelectedItem.Text + ":" + ddlEndMin.SelectedItem.Text + ":" + "00" + ddlEndFrmt.SelectedItem.Text);
                    StartTime = DateTime.Parse(strtimetemp);
                    EndTime = DateTime.Parse(endTimeTemp);

                    if (StartTime >= EndTime)
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("End time must be after start time");
                        ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                        return;
                    }
                }
                //    StartTime = DateTime.Parse(txtStartime.Text);
                flag++;

            }
            catch (Exception Ex)
            {
                flag = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                //throw Ex;
            }
        }

        if (sess.StudentId > 0)
        {
            if (txtBehaviour.Text != "")
            {
                if (chkInactive.Checked != true)
                {
                    if (!((chkPartial.Checked == true) && ((txtNoOfTimes.Text.Length == 0) || (txtInterval.Text.Length == 0) || (txtEndsOn.Text.Length == 0))))
                    {
                        int chkDatFlg = 0;
                        DateTime dtst = new DateTime();
                        DateTime dtend = new DateTime();
                        if (chkPartial.Checked == true)
                        {
                            dtst = DateTime.ParseExact(txtStartDate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            dtend = DateTime.ParseExact(txtEndsOn.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            chkDatFlg = 1;
                        }
                        //if (((dtst <= dtend) && (dtst.Date >= System.DateTime.Now.Date)) || (chkDatFlg == 1))
                        //{

                        if (((dtst <= dtend) && btnSave.Text == "Update") || ((dtst <= dtend) && (dtst.Date >= System.DateTime.Now.Date)) || (chkDatFlg == 1))
                        {

                            if ((chkDuration.Checked == true) || (chkFrequency.Checked == true) || (CheckBoxCostLessPlan.Checked == true) || (chkYesOrNo.Checked == true))
                            {
                                if (flag != 0)
                                {
                                    if (btnSave.Text == "Save")
                                    {



                                        if (objData.IFExists("Select MeasurementId from BehaviourDetails where StudentId='" + sess.StudentId + "' and Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "' and (ActiveInd='A' OR ActiveInd='N')") == false)
                                        {

                                            Boolean frq = false;
                                            Boolean due = false;
                                            Boolean opp = false;
                                            Boolean format = false;
                                            Boolean yesOrNo = false;                                            
                                            Boolean perInterval=false;


                                            //if (chkFrequency.Checked == true)
                                            //{
                                            //    frq = true;
                                            //}
                                            if (chkDuration.Checked == true)
                                            {
                                                due = true;
                                            }
                                            if(chkOpportunities.Checked == true)
                                            {
                                                opp = true;
                                            }
                                            if (chk24Hr.Checked == true)
                                            {
                                                format = true;
                                            }
                                            if (chkPartial.Checked == true)
                                            {
                                                if (chkFrequency.Checked == true)
                                                {
                                                    frq = true;
                                                    perInterval = false;
                                                    //chkYesOrNo.Checked = false;                                                    
                                                }
                                                else if (chkYesOrNo.Checked == true)
                                                {
                                                    yesOrNo = true;
                                                    //chkFrequency.Checked = false;                                                    
                                                    if (rdoSumTotal.SelectedItem.Text == "%Interval")
                                                    {
                                                        perInterval = true;
                                                    }
                                                    else
                                                    {
                                                        perInterval = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (chkFrequency.Checked == true)
                                                {
                                                    frq = true;
                                                }
                                                if (chkYesOrNo.Checked == true)
                                                {
                                                    yesOrNo = true;
                                                }
                                                perInterval = false;
                                            }

                                            string result;
                                            int Final = 0;




                                            if (chkPartial.Checked == true)
                                            {
                                                DateTime incrementStartTime = StartTime;
                                                string ParaStartTime = "";
                                                string ParaEndTime = "";
                                                for (int j = 0; j < int.Parse(txtNoOfTimes.Text); j++)
                                                {
                                                    ParaStartTime += incrementStartTime.ToString("HH:mm:ss") + ",";
                                                    ParaEndTime += incrementStartTime.AddMinutes(int.Parse(txtPeriod.Text)).ToString("HH:mm:ss") + ",";
                                                    incrementStartTime = incrementStartTime.AddMinutes(int.Parse(txtInterval.Text));
                                                }
                                                //string SpResult = objData.ExecuteDateIntervalExist(ParaStartTime, ParaEndTime, dtst.ToString("yyyy-MM-dd"), dtend.ToString("yyyy-MM-dd"), sess.SchoolId, sess.Classid, int.Parse(txtNoOfTimes.Text));
                                                //if (SpResult == "False")
                                                //{
                                                //    tdMsg.InnerHtml = clsGeneral.warningMsg("Start Time is not within the school time");
                                                //    return;
                                                //}
                                                if (txtEndsOn.Text.Length != 0)
                                                {

                                                    DateTime StartTempDate = DateTime.ParseExact(txtStartDate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                                                    DateTime EndTempDate = DateTime.ParseExact(txtEndsOn.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                                    string DropDownValuetoTable = "";
                                                    if (ddlRepeat.SelectedIndex == 1)
                                                    {
                                                        DropDownValuetoTable = ddlRepeat.SelectedIndex.ToString();
                                                        if (ChbxSun.Checked == true)
                                                        {
                                                            DropDownValuetoTable = DropDownValuetoTable + ",sun";
                                                        }
                                                        if (ChbxMon.Checked == true)
                                                        {
                                                            DropDownValuetoTable = DropDownValuetoTable + ",mon";
                                                        }
                                                        if (ChbxTue.Checked == true)
                                                        {
                                                            DropDownValuetoTable = DropDownValuetoTable + ",tue";
                                                        }
                                                        if (ChbxWed.Checked == true)
                                                        {
                                                            DropDownValuetoTable = DropDownValuetoTable + ",wed";
                                                        }
                                                        if (ChbxThu.Checked == true)
                                                        {
                                                            DropDownValuetoTable = DropDownValuetoTable + ",thu";
                                                        }
                                                        if (ChbxFri.Checked == true)
                                                        {
                                                            DropDownValuetoTable = DropDownValuetoTable + ",fri";
                                                        }
                                                        if (ChbxSat.Checked == true)
                                                        {
                                                            DropDownValuetoTable = DropDownValuetoTable + ",sat";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        DropDownValuetoTable = ddlRepeat.SelectedIndex.ToString();
                                                    }



                                                    //result = " Insert into BehaviourDetails(StudentId,SchoolId,ClassId,Behaviour,Frequency,Duration,PartialInterval,Hr24,YesOrNo,IfPerInterval,StartTime,EndTime,NumOfTimes,Period,Interval,IsAcceleration,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,StartDate,EndDate,Condition,BehavDefinition,BehavStrategy,GoalDesc) Values('" + sess.StudentId + "'," + sess.SchoolId + "," + sess.Classid + ",'" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "','" + frq + "','" + due + "','True','" + format + "','" + yesOrNo + "','" + perInterval + "','" + StartTime.TimeOfDay + "','" + EndTime.TimeOfDay + "','" + int.Parse(txtNoOfTimes.Text) + "','" + int.Parse(txtPeriod.Text) + "','" + int.Parse(txtInterval.Text) + "','" + acceleration + "','A','" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + txtStartDate.Text + "','" + txtEndsOn.Text + "','" + DropDownValuetoTable + "','" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "')";
                                                    result = " Insert into BehaviourDetails(StudentId,SchoolId,ClassId,Behaviour,Frequency,Duration,Opportunities,PartialInterval,Hr24,YesOrNo,IfPerInterval,StartTime,EndTime,NumOfTimes,Period,Interval,IsAcceleration,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,StartDate,EndDate,Condition,BehavDefinition,BehavStrategy,GoalDesc,BehaviorIEPObjctve,BehaviorBasPerfLvl) Values('" + sess.StudentId + "'," + sess.SchoolId + "," + sess.Classid + ",'" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "','" + frq + "','" + due + "','" + opp + "','True','" + format + "','" + yesOrNo + "','" + perInterval + "','" + StartTime.TimeOfDay + "','" + EndTime.TimeOfDay + "','" + int.Parse(txtNoOfTimes.Text) + "','" + int.Parse(txtPeriod.Text) + "','" + int.Parse(txtInterval.Text) + "','" + acceleration + "','A','" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + txtStartDate.Text + "','" + txtEndsOn.Text + "','" + DropDownValuetoTable + "','" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtbehIEPobjtve.Text) + "','" + clsGeneral.convertQuotes(txtbehBasperlvl.Text) + "')";
                                                    Final = objData.ExecuteWithScope(result);


                                                    /////////////////////
                                                    string WhereConditionWeekly = "";
                                                    if (ddlRepeat.SelectedIndex == 0)
                                                    {
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') or (datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') or (datename (dw, Call_Dates) = 'friday' and Weekday='friday') or (datename (dw, Call_Dates) = 'saturday' and Weekday='saturday') or (datename (dw, Call_Dates) = 'saturday' and Weekday='saturday')";
                                                    }
                                                    else if (ddlRepeat.SelectedIndex == 1)
                                                    {
                                                        int flgForOr = 0;
                                                        if (ChbxSun.Checked == true)
                                                        {
                                                            if (flgForOr == 0)
                                                            {
                                                                //(datename (dw, Call_Dates) = 'tuesday'and Weekday='tuesday' )
                                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'sunday' and Weekday='sunday') ";
                                                            }
                                                            else
                                                            {
                                                                WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'sunday' and Weekday='sunday') ";
                                                            }
                                                            flgForOr = 1;
                                                        }
                                                        if (ChbxMon.Checked == true)
                                                        {
                                                            if (flgForOr == 0)
                                                            {
                                                                //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'monday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') ";
                                                            }
                                                            else
                                                            {
                                                                //  WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'monday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'monday' and Weekday='monday') ";
                                                            }
                                                            flgForOr = 1;
                                                        }
                                                        if (ChbxTue.Checked == true)
                                                        {
                                                            if (flgForOr == 0)
                                                            {
                                                                // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'tuesday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') ";
                                                            }
                                                            else
                                                            {
                                                                // WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'tuesday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') ";
                                                            }
                                                            flgForOr = 1;
                                                        }
                                                        if (ChbxWed.Checked == true)
                                                        {
                                                            if (flgForOr == 0)
                                                            {
                                                                //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'wednesday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') ";
                                                            }
                                                            else
                                                            {
                                                                //WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'wednesday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') ";
                                                            }
                                                            flgForOr = 1;
                                                        }
                                                        if (ChbxThu.Checked == true)
                                                        {
                                                            if (flgForOr == 0)
                                                            {
                                                                // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'thursday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') ";
                                                            }
                                                            else
                                                            {
                                                                //WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'thursday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') ";
                                                            }
                                                            flgForOr = 1;
                                                        }
                                                        if (ChbxFri.Checked == true)
                                                        {
                                                            if (flgForOr == 0)
                                                            {
                                                                //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'friday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'friday' and Weekday='friday') ";
                                                            }
                                                            else
                                                            {
                                                                // WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'friday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'friday' and Weekday='friday') ";
                                                            }
                                                            flgForOr = 1;
                                                        }
                                                        if (ChbxSat.Checked == true)
                                                        {
                                                            if (flgForOr == 0)
                                                            {
                                                                //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'saturday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'saturday' and Weekday='saturday') ";
                                                            }
                                                            else
                                                            {
                                                                // WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'saturday' ";
                                                                WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'saturday' and Weekday='saturday') ";
                                                            }
                                                            flgForOr = 1;
                                                        }
                                                    }

                                                    else if (ddlRepeat.SelectedIndex == 2)
                                                    {
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') or (datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') or (datename (dw, Call_Dates) = 'friday' and Weekday='friday') ";
                                                        // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) <> 'sunday' and datename (dw, Call_Dates) <>'saturday' ";

                                                    }
                                                    else if (ddlRepeat.SelectedIndex == 3)
                                                    {
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') or (datename (dw, Call_Dates) = 'friday' and Weekday='friday')";
                                                        // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'monday' or datename (dw, Call_Dates) = 'wednesday' or datename (dw, Call_Dates) = 'friday'";

                                                    }
                                                    else if (ddlRepeat.SelectedIndex == 4)
                                                    {
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday')";
                                                        // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'tuesday' or datename (dw, Call_Dates) = 'thursday'";

                                                    }
                                                    // DateTime date = 
                                                    //DateTime d2 = DateTime.Parse(txtEndsOn.Text);


                                                    // string strProcedureQuery = "select  1,1,startTime,endTime,CONVERT(datetime, Call_Dates),datename (dw, Call_Dates) from #temp1,CalenderAllDate where CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + DateTime.Parse(txtStartDate.Text).ToShortDateString() + "',103)  and CONVERT(datetime, '" + DateTime.Parse(txtEndsOn.Text).ToShortDateString() + "',103) and (" + WhereConditionWeekly + ")";
                                                    //    " insert into BehaviourCalc(StudentId,MeasurmentId,StartTime,EndTime,[Date],ActiveInd,CreatedBy,CreatedOn,IOAFlag,IOAUser)   select  " + Convert.ToInt32(ddlStudent.SelectedItem.Value) + "," + Final + ",startTime,endTime,CONVERT(datetime, Call_Dates),'A','" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)),'false','" + sess.LoginId + "' from #temp1,CalenderAllDate where CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + StartTempDate + "',103)  and CONVERT(datetime, '" + EndTempDate + "',103) and (" + WhereConditionWeekly + ")";


                                                    //  where ResidenceInd=(select ResidenceInd from Student where StudentId=" + Convert.ToInt32(ddlStudent.SelectedItem.Value) + ") and CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + StartTempDate + "',103) and CONVERT(datetime, '" + EndTempDate + "',103) and ((datename (dw, Call_Dates)= 'monday' and Weekday='monday' ) or (datename (dw, Call_Dates) = 'tuesday'and Weekday='tuesday' )) and cast(#temp1.startTime as time) > cast(SchoolCal.StartTime as time) and cast(#temp1.endTime as time) < cast(SchoolCal.EndTime as time) and Call_Dates not in (select HolDate from SchoolHoliday)


                                                    int ResidenceInd = Convert.ToInt16(objData.FetchValue("Select ResidenceInd from Class Where ClassId=" + sess.Classid + " "));
                                                    ResidenceInd = (ResidenceInd == 0) ? 1 : 0;
                                                    string strProcedureQuery = " insert into BehaviourCalc(IsPartial,StudentId,MeasurmentId,StartTime,EndTime,[Date],ActiveInd,CreatedBy,CreatedOn,IOAFlag,IOAUser)   select  'True'," + sess.StudentId + "," + Final + ",#temp1.startTime,#temp1.endTime,cast(Call_Dates as date),'A','" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)),'false','" + sess.LoginId + "' from #temp1,SchoolCal,CalenderAllDate where cast(Call_Dates as date) between  cast('" + StartTempDate.ToString("yyyy-MM-dd") + "' as date) and cast('" + EndTempDate.ToString("yyyy-MM-dd") + "' as date) and (" + WhereConditionWeekly + ") and  cast(#temp1.startTime as datetime) >= cast(SchoolCal.StartTime as datetime) and cast(#temp1.endTime as datetime) <= cast(SchoolCal.EndTime as datetime) and Call_Dates not in (select HolDate from SchoolHoliday where SchoolId=" + sess.SchoolId + ") and SchoolCal.SchoolId=" + sess.SchoolId + " And SchoolCal.ResidenceInd = " + ResidenceInd + "";
                                                    // DateTime date = 
                                                    //DateTime d2 = DateTime.Parse(txtEndsOn.Text);
                                                    // string strProcedureQuery = "select  1,1,startTime,endTime,CONVERT(datetime, Call_Dates),datename (dw, Call_Dates) from #temp1,CalenderAllDate where CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + DateTime.Parse(txtStartDate.Text).ToShortDateString() + "',103)  and CONVERT(datetime, '" + DateTime.Parse(txtEndsOn.Text).ToShortDateString() + "',103) and (" + WhereConditionWeekly + ")";
                                                    //    string strProcedureQuery = " insert into BehaviourCalc(StudentId,MeasurmentId,StartTime,EndTime,[Date],ActiveInd,CreatedBy,CreatedOn,IOAFlag,IOAUser)   select  " + Convert.ToInt32(ddlStudent.SelectedItem.Value) + "," + Final + ",startTime,endTime,CONVERT(datetime, Call_Dates),'A','" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)),'false','" + sess.LoginId + "' from #temp1,CalenderAllDate where CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + StartTempDate + "',103)  and CONVERT(datetime, '" + EndTempDate + "',103) and (" + WhereConditionWeekly + ")";


                                                    clsData oData = new clsData();
                                                    SqlCommand command;
                                                    SqlDataAdapter adp;
                                                    try
                                                    {
                                                        command = new SqlCommand("behaviourPartialAdd", new SqlConnection(oData.ConnectionString));
                                                        command.CommandType = CommandType.StoredProcedure;
                                                        command.Parameters.AddWithValue("@StartTime", ParaStartTime);
                                                        command.Parameters.AddWithValue("@EndTime", ParaEndTime);
                                                        command.Parameters.AddWithValue("@QueryToExe", strProcedureQuery);

                                                        command.Connection.Open();
                                                        command.ExecuteNonQuery();
                                                        //   DataSet dttemp = new DataSet();
                                                        //   adp = new SqlDataAdapter(command);
                                                        //   adp.Fill(dttemp);
                                                        command.Connection.Close();

                                                    }
                                                    catch (Exception Ex)
                                                    {
                                                        ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                                                        errorflg++;
                                                        tdMsg.InnerHtml = clsGeneral.failedMsg("....Partial Interval Entry Failed !");
                                                        throw Ex;
                                                    }

                                                    EnableAllAlerts(Final);
                                                    /////////////
                                                }
                                                else
                                                {
                                                    //tdMsg.InnerHtml = clsGeneral.warningMsg("Calander Not set For Partial Interval");
                                                }
                                            }
                                            else
                                            {

                                                //result = " Insert into BehaviourDetails(StudentId,SchoolId,ClassId,Behaviour,Frequency,Duration,PartialInterval,IsAcceleration,ActiveInd,BehavDefinition,BehavStrategy,GoalDesc,YesOrNo,IfPerInterval,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values('" + sess.StudentId + "'," + sess.SchoolId + "," + sess.Classid + ",'" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "','" + frq + "','" + due + "','False','" + acceleration + "','A','" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "','" + yesOrNo + "','" + perInterval + "','" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)))";
                                                result = " Insert into BehaviourDetails(StudentId,SchoolId,ClassId,Behaviour,Frequency,Duration,Opportunities,PartialInterval,IsAcceleration,ActiveInd,BehavDefinition,BehavStrategy,GoalDesc,YesOrNo,IfPerInterval,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,BehaviorIEPObjctve,BehaviorBasPerfLvl) Values('" + sess.StudentId + "'," + sess.SchoolId + "," + sess.Classid + ",'" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "','" + frq + "','" + due + "','" + opp + "','False','" + acceleration + "','A','" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "','" + yesOrNo + "','" + perInterval + "','" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + clsGeneral.convertQuotes(txtbehIEPobjtve.Text) + "','" + clsGeneral.convertQuotes(txtbehBasperlvl.Text) + "')";
                                                Final = objData.ExecuteWithScope(result);
                                            }

                                            int Finalreturn = 0;

                                            //bool LessonExxists = objData.IFExists("Select BT.MeasurementId from BehaviourDetails BT inner join BehaviourLPRel BLpRel on BT.[MeasurementId]=BLpRel.[MeasurementId] where StudentId=" + sess.StudentId + " and (BT.ActiveInd='A' OR BT.ActiveInd='N') and BLpRel.LessonPlanId=" + lessonPLId + "");


                                            if ((CheckBoxLessonPlan.Checked == true) && (CheckBoxCostLessPlan.Checked == true) && (Final != 0))
                                            {
                                                string queryGoal = "SELECT GoalId from Goal where GoalName='Behavior' and GoalCode='Behavior'";
                                                int goalId = Convert.ToInt32(objData.FetchValue(queryGoal));
                                                string sGl = SaveGoal(goalId.ToString(), true);

                                                try
                                                {

                                                    string BehaviourLPRelQuery = "insert into BehaviourLPRel(MeasurementId,LessonPlanId,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) values(" + Final + "," + lessonPLId + ",'A','" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)))";
                                                    Finalreturn = objData.Execute(BehaviourLPRelQuery);

                                                    string rtn = SaveLessons(lessonPLId.ToString(), goalId.ToString());
                                                    ////query to check current class status;
                                                    //string qryclass = "select ResidenceInd from [dbo].[Class] where classid=" + sess.Classid;
                                                    //bool isDay = Convert.ToBoolean(objData.FetchValue(qryclass));

                                                    //string DayFlag = "NULL";
                                                    //string ResiFlag = "NULL";
                                                    //if (!isDay)
                                                    //    DayFlag = "1";
                                                    //else
                                                    //    ResiFlag = "1";


                                                    //BehaviourLPRelQuery = "Insert Into dbo.StdtLessonPlan (SchoolId,StudentId,LessonPlanId,GoalId,IncludeIEP,AsmntYearId,ActiveInd,LessonPlanTypeDay,LessonPlanTypeResi,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
                                                    //BehaviourLPRelQuery += "Values(" + sess.SchoolId + "," + sess.StudentId + "," + lessonPLId + "," + goalId + ",0," + sess.YearId + ",   'A'," + DayFlag + "," + ResiFlag + "," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate()) ";
                                                    //int check = objData.ExecuteWithScope(BehaviourLPRelQuery);
                                                    //if (check > 0)
                                                    //{

                                                    //    //// creating Template Process                                               

                                                    //    clsAssignLessonPlan AssignLP = new clsAssignLessonPlan();
                                                    //    int dstmpId = AssignLP.SaveTemplateDetails(sess.SchoolId, sess.StudentId, lessonPLId, CheckBoxLessonPlan.Text, sess.LoginId, check);
                                                    //}

                                                    ////

                                                }
                                                catch (Exception eX)
                                                {
                                                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                                                    tdMsg.InnerHtml = clsGeneral.failedMsg("Failed !");
                                                    throw eX;
                                                }

                                            }

                                            if ((Final > 0) && (errorflg == 0))
                                                tdMsg.InnerHtml = clsGeneral.sucessMsg("Behavior Details Inserted Successfully");
                                            HiddenFieldCheckPopup.Value = "0";
                                            clearDataReset();
                                        }
                                        else
                                        {
                                            tdMsg.InnerHtml = clsGeneral.failedMsg("Behaviour Details already exist");
                                        }
                                    }
                                    else if (btnSave.Text == "Update")
                                    {
                                        if (objData.IFExists("Select MeasurementId from BehaviourDetails where StudentId='" + sess.StudentId + "' and Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "' and (ActiveInd='A' OR ActiveInd='N') and MeasurementId <> " + Convert.ToInt32(hdnid.Value) + " ") == false)
                                        {

                                        Boolean frq = false;
                                        Boolean due = false;
                                        Boolean opp = false;
                                        Boolean format = false;
                                        Boolean yesOrNo = false;
                                        Boolean perInterval = false;
                                        string stat = "A";
                                        /// check the interval
                                        /// 
                                        if (chkPartial.Checked == true)
                                        {
                                            if (chkFrequency.Checked == true)
                                            {
                                                frq = true;
                                                perInterval = false;
                                                //chkYesOrNo.Checked = false;
                                            }
                                            else if (chkYesOrNo.Checked == true)
                                            {
                                                yesOrNo = true;
                                                if (rdoSumTotal.SelectedItem.Text == "%Interval")
                                                {
                                                    perInterval = true;
                                                }
                                                else
                                                {
                                                    perInterval = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (chkFrequency.Checked == true)
                                            {
                                                frq = true;
                                            }
                                            if (chkYesOrNo.Checked == true)
                                            {
                                                yesOrNo = true;
                                            }
                                            perInterval = false;
                                        }
                                        if (chk24Hr.Checked == true)
                                        {
                                            format = true;
                                        }

                                        if (chkDuration.Checked == true)
                                        {
                                            due = true;
                                        }
                                        if(chkOpportunities.Checked == true)
                                            {
                                                opp = true;
                                            }
                                        if (chkInactive.Checked == true)
                                        {
                                            stat = "N";
                                        }
                                        string result;
                                        if (chkPartial.Checked == true)
                                        {
                                            DateTime incrementStartTime = StartTime;

                                            string ParaStartTime = "";
                                            string ParaEndTime = "";
                                            for (int j = 0; j < int.Parse(txtNoOfTimes.Text); j++)
                                            {
                                                ParaStartTime = ParaStartTime + incrementStartTime.ToString("HH:mm:ss") + ",";
                                                ParaEndTime = ParaEndTime + incrementStartTime.AddMinutes(int.Parse(txtPeriod.Text)).ToString("HH:mm:ss") + ",";
                                                incrementStartTime = incrementStartTime.AddMinutes(int.Parse(txtInterval.Text));
                                            }
                                            //string SpResult = objData.ExecuteDateIntervalExist(ParaStartTime, ParaEndTime, dtst.ToString("yyyy-MM-dd"), dtend.ToString("yyyy-MM-dd"), sess.SchoolId, sess.Classid, int.Parse(txtNoOfTimes.Text));
                                            //if (SpResult == "False")
                                            //{
                                            //    tdMsg.InnerHtml = clsGeneral.warningMsg("Start Time is not within the school time");
                                            //    return;
                                            //}
                                            if (objData.IFExists("select BehaviourDetails.MeasurementId from BehaviourDetails inner join BehaviourCalc on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId where BehaviourCalc.StudentId='" + sess.StudentId + "'  and BehaviourDetails.ActiveInd='A' and DateDiff(day, BehaviourCalc.Date, getdate()) <1 and BehaviourCalc.MeasurmentId='" + Convert.ToInt32(hdnid.Value) + "' AND BehaviourCalc.IsPartial='True'") == true)
                                            {
                                                string adBcalc = " delete from BehaviourCalc where StudentId=" + sess.StudentId + " and DateDiff(day,Date, getdate()) <1 AND BehaviourCalc.IsPartial='True' and MeasurmentId=" + int.Parse(hdnid.Value);
                                                objData.Execute(adBcalc);
                                                //string deRemider = " delete from BehaviorReminder where br.UserId=" + sess.LoginId ;
                                                string deRemider = " delete br from BehaviorReminder br join BehaviourCalc bc on bc.behaviourcalcid=br.behaviourcalcid where br.UserId=" + sess.LoginId + " and bc.StudentId=" + sess.StudentId + " and DateDiff(day,Date, getdate()) <1 AND bc.IsPartial='True' and bc.MeasurmentId=" + int.Parse(hdnid.Value);
                                                objData.Execute(deRemider);

                                            }

                                            DateTime StartTempDate = DateTime.ParseExact(txtStartDate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

                                            DateTime EndTempDate = DateTime.ParseExact(txtEndsOn.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                            string DropDownValuetoTable = "";
                                            if (ddlRepeat.SelectedIndex == 1)
                                            {
                                                DropDownValuetoTable = ddlRepeat.SelectedIndex.ToString();
                                                if (ChbxSun.Checked == true)
                                                {
                                                    DropDownValuetoTable = DropDownValuetoTable + ",sun";
                                                }
                                                if (ChbxMon.Checked == true)
                                                {
                                                    DropDownValuetoTable = DropDownValuetoTable + ",mon";
                                                }
                                                if (ChbxTue.Checked == true)
                                                {
                                                    DropDownValuetoTable = DropDownValuetoTable + ",tue";
                                                }
                                                if (ChbxWed.Checked == true)
                                                {
                                                    DropDownValuetoTable = DropDownValuetoTable + ",wed";
                                                }
                                                if (ChbxThu.Checked == true)
                                                {
                                                    DropDownValuetoTable = DropDownValuetoTable + ",thu";
                                                }
                                                if (ChbxFri.Checked == true)
                                                {
                                                    DropDownValuetoTable = DropDownValuetoTable + ",fri";
                                                }
                                                if (ChbxSat.Checked == true)
                                                {
                                                    DropDownValuetoTable = DropDownValuetoTable + ",sat";
                                                }
                                            }
                                            else
                                            {
                                                DropDownValuetoTable = ddlRepeat.SelectedIndex.ToString();
                                            }


                                            //result = " Insert into BehaviourDetails(StudentId,Behaviour,Frequency,Duration,PartialInterval,StartTime,NumOfTimes,Period,Interval,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,StartDate,EndDate,Condition) Values('" + Convert.ToInt32(ddlStudent.SelectedItem.Value) + "','" + txtBehaviour.Text + "','" + frq + "','" + due + "','True','" + StartTime.TimeOfDay + "','" + int.Parse(txtNoOfTimes.Text) + "','" + int.Parse(txtPeriod.Text) + "','" + int.Parse(txtInterval.Text) + "','A','" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + StartTempDate.ToShortDateString() + "','" + EndTempDate.ToShortDateString() + "','" + DropDownValuetoTable + "')";
                                            //result = "Update BehaviourDetails set StudentId='" + sess.StudentId + "',SchoolId=" + sess.SchoolId + ",ClassId=" + sess.Classid + ",IsAcceleration='" + acceleration + "',Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "',Frequency='" + frq + "',Duration='" + due + "',PartialInterval='True',Hr24='" + format + "',YesOrNo='" + yesOrNo + "',IfPerInterval='"+perInterval+"',StartTime='" + StartTime.TimeOfDay + "',EndTime='" + EndTime.TimeOfDay + "',NumOfTimes='" + int.Parse(txtNoOfTimes.Text) + "',Period='" + int.Parse(txtPeriod.Text) + "',Interval='" + int.Parse(txtInterval.Text) + "',ActiveInd='" + stat + "',BehavDefinition='" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "',BehavStrategy='" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "',GoalDesc='" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)),StartDate='" + txtStartDate.Text + "',EndDate='" + txtEndsOn.Text + "',Condition='" + DropDownValuetoTable + "' where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                            result = "Update BehaviourDetails set StudentId='" + sess.StudentId + "',SchoolId=" + sess.SchoolId + ",ClassId=" + sess.Classid + ",IsAcceleration='" + acceleration + "',Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "',Frequency='" + frq + "',Duration='" + due + "',Opportunities='" + opp + "',PartialInterval='True',Hr24='" + format + "',YesOrNo='" + yesOrNo + "',IfPerInterval='" + perInterval + "',StartTime='" + StartTime.TimeOfDay + "',EndTime='" + EndTime.TimeOfDay + "',NumOfTimes='" + int.Parse(txtNoOfTimes.Text) + "',Period='" + int.Parse(txtPeriod.Text) + "',Interval='" + int.Parse(txtInterval.Text) + "',ActiveInd='" + stat + "',BehavDefinition='" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "',BehavStrategy='" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "',GoalDesc='" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)),StartDate='" + txtStartDate.Text + "',EndDate='" + txtEndsOn.Text + "',Condition='" + DropDownValuetoTable + "',BehaviorIEPObjctve='" + clsGeneral.convertQuotes(txtbehIEPobjtve.Text) + "',BehaviorBasPerfLvl='" + clsGeneral.convertQuotes(txtbehBasperlvl.Text) + "' where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                            string WhereConditionWeekly = "";
                                            if (ddlRepeat.SelectedIndex == 0)
                                            {
                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') or (datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') or (datename (dw, Call_Dates) = 'friday' and Weekday='friday') or (datename (dw, Call_Dates) = 'saturday' and Weekday='saturday') or (datename (dw, Call_Dates) = 'saturday' and Weekday='saturday')";
                                            }
                                            else if (ddlRepeat.SelectedIndex == 1)
                                            {
                                                int flgForOr = 0;
                                                if (ChbxSun.Checked == true)
                                                {
                                                    if (flgForOr == 0)
                                                    {
                                                        //(datename (dw, Call_Dates) = 'tuesday'and Weekday='tuesday' )
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'sunday' and Weekday='sunday') ";
                                                    }
                                                    else
                                                    {
                                                        WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'sunday' and Weekday='sunday') ";
                                                    }
                                                    flgForOr = 1;
                                                }
                                                if (ChbxMon.Checked == true)
                                                {
                                                    if (flgForOr == 0)
                                                    {
                                                        //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'monday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') ";
                                                    }
                                                    else
                                                    {
                                                        //  WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'monday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'monday' and Weekday='monday') ";
                                                    }
                                                    flgForOr = 1;
                                                }
                                                if (ChbxTue.Checked == true)
                                                {
                                                    if (flgForOr == 0)
                                                    {
                                                        // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'tuesday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') ";
                                                    }
                                                    else
                                                    {
                                                        // WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'tuesday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') ";
                                                    }
                                                    flgForOr = 1;
                                                }
                                                if (ChbxWed.Checked == true)
                                                {
                                                    if (flgForOr == 0)
                                                    {
                                                        //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'wednesday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') ";
                                                    }
                                                    else
                                                    {
                                                        //WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'wednesday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') ";
                                                    }
                                                    flgForOr = 1;
                                                }
                                                if (ChbxThu.Checked == true)
                                                {
                                                    if (flgForOr == 0)
                                                    {
                                                        // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'thursday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') ";
                                                    }
                                                    else
                                                    {
                                                        //WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'thursday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') ";
                                                    }
                                                    flgForOr = 1;
                                                }
                                                if (ChbxFri.Checked == true)
                                                {
                                                    if (flgForOr == 0)
                                                    {
                                                        //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'friday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'friday' and Weekday='friday') ";
                                                    }
                                                    else
                                                    {
                                                        // WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'friday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'friday' and Weekday='friday') ";
                                                    }
                                                    flgForOr = 1;
                                                }
                                                if (ChbxSat.Checked == true)
                                                {
                                                    if (flgForOr == 0)
                                                    {
                                                        //WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'saturday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'saturday' and Weekday='saturday') ";
                                                    }
                                                    else
                                                    {
                                                        // WhereConditionWeekly = WhereConditionWeekly + "or datename (dw, Call_Dates) = 'saturday' ";
                                                        WhereConditionWeekly = WhereConditionWeekly + "or (datename (dw, Call_Dates) = 'saturday' and Weekday='saturday') ";
                                                    }
                                                    flgForOr = 1;
                                                }
                                            }

                                            else if (ddlRepeat.SelectedIndex == 2)
                                            {
                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') or (datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday') or (datename (dw, Call_Dates) = 'friday' and Weekday='friday') ";
                                                // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) <> 'sunday' and datename (dw, Call_Dates) <>'saturday' ";

                                            }
                                            else if (ddlRepeat.SelectedIndex == 3)
                                            {
                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'monday' and Weekday='monday') or (datename (dw, Call_Dates) = 'wednesday' and Weekday='wednesday') or (datename (dw, Call_Dates) = 'friday' and Weekday='friday')";
                                                // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'monday' or datename (dw, Call_Dates) = 'wednesday' or datename (dw, Call_Dates) = 'friday'";

                                            }
                                            else if (ddlRepeat.SelectedIndex == 4)
                                            {
                                                WhereConditionWeekly = WhereConditionWeekly + "(datename (dw, Call_Dates) = 'tuesday' and Weekday='tuesday') or (datename (dw, Call_Dates) = 'thursday' and Weekday='thursday')";
                                                // WhereConditionWeekly = WhereConditionWeekly + "datename (dw, Call_Dates) = 'tuesday' or datename (dw, Call_Dates) = 'thursday'";

                                            }

                                            // DateTime date = 
                                            //DateTime d2 = DateTime.Parse(txtEndsOn.Text);


                                            // string strProcedureQuery = "select  1,1,startTime,endTime,CONVERT(datetime, Call_Dates),datename (dw, Call_Dates) from #temp1,CalenderAllDate where CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + DateTime.Parse(txtStartDate.Text).ToShortDateString() + "',103)  and CONVERT(datetime, '" + DateTime.Parse(txtEndsOn.Text).ToShortDateString() + "',103) and (" + WhereConditionWeekly + ")";
                                            //    " insert into BehaviourCalc(StudentId,MeasurmentId,StartTime,EndTime,[Date],ActiveInd,CreatedBy,CreatedOn,IOAFlag,IOAUser)   select  " + Convert.ToInt32(ddlStudent.SelectedItem.Value) + "," + Final + ",startTime,endTime,CONVERT(datetime, Call_Dates),'A','" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)),'false','" + sess.LoginId + "' from #temp1,CalenderAllDate where CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + StartTempDate + "',103)  and CONVERT(datetime, '" + EndTempDate + "',103) and (" + WhereConditionWeekly + ")";


                                            //  where ResidenceInd=(select ResidenceInd from Student where StudentId=" + Convert.ToInt32(ddlStudent.SelectedItem.Value) + ") and CONVERT(datetime,Call_Dates,103) between CONVERT(datetime, '" + StartTempDate + "',103) and CONVERT(datetime, '" + EndTempDate + "',103) and ((datename (dw, Call_Dates)= 'monday' and Weekday='monday' ) or (datename (dw, Call_Dates) = 'tuesday'and Weekday='tuesday' )) and cast(#temp1.startTime as time) > cast(SchoolCal.StartTime as time) and cast(#temp1.endTime as time) < cast(SchoolCal.EndTime as time) and Call_Dates not in (select HolDate from SchoolHoliday)


                                            int ResidenceInd = Convert.ToInt16(objData.FetchValue("Select ResidenceInd from Class Where ClassId=" + sess.Classid + " "));
                                            ResidenceInd = (ResidenceInd == 0) ? 1 : 0;
                                            string strProcedureQuery = " insert into BehaviourCalc(IsPartial,StudentId,MeasurmentId,StartTime,EndTime,[Date],ActiveInd,CreatedBy,CreatedOn,IOAFlag,IOAUser)   select  'True'," + sess.StudentId + "," + Convert.ToInt32(hdnid.Value) + ",#temp1.startTime,#temp1.endTime,cast(Call_Dates as date),'A','" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)),'false','" + sess.LoginId + "' from #temp1,SchoolCal,CalenderAllDate where cast(Call_Dates as date) between cast('" + StartTempDate.ToString("yyyy-MM-dd") + "' as date)  and cast('" + EndTempDate.ToString("yyyy-MM-dd") + "' as date) and (" + WhereConditionWeekly + ") and  cast(#temp1.startTime as datetime) > cast(SchoolCal.StartTime as datetime) and cast(#temp1.endTime as datetime) < cast(SchoolCal.EndTime as datetime) and Call_Dates not in (select HolDate from SchoolHoliday where SchoolId=" + sess.SchoolId + ") and SchoolCal.SchoolId=" + sess.SchoolId + " And SchoolCal.ResidenceInd=" + ResidenceInd + "";




                                            clsData oData = new clsData();
                                            SqlCommand command;
                                            SqlDataAdapter adp;
                                            try
                                            {
                                                command = new SqlCommand("behaviourPartialAdd", new SqlConnection(oData.ConnectionString));
                                                command.CommandType = CommandType.StoredProcedure;
                                                command.Parameters.AddWithValue("@StartTime", ParaStartTime);
                                                command.Parameters.AddWithValue("@EndTime", ParaEndTime);
                                                command.Parameters.AddWithValue("@QueryToExe", strProcedureQuery);

                                                command.Connection.Open();
                                                command.ExecuteNonQuery();
                                                //   DataSet dttemp = new DataSet();
                                                //   adp = new SqlDataAdapter(command);
                                                //   adp.Fill(dttemp);
                                                command.Connection.Close();
                                            }
                                            catch (Exception Ex)
                                            {
                                                ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                                                errorflg++;
                                                tdMsg.InnerHtml = clsGeneral.failedMsg("Partial Interval Entry Failed !");
                                                throw Ex;
                                            }

                                            EnableAllAlerts(Convert.ToInt32(hdnid.Value));




                                        }
                                        else
                                        {
                                            if (objData.IFExists("select BehaviourDetails.MeasurementId from BehaviourDetails inner join BehaviourCalc on BehaviourDetails.MeasurementId=BehaviourCalc.MeasurmentId where BehaviourCalc.StudentId=" + sess.StudentId + " and BehaviourDetails.PartialInterval='True' and BehaviourDetails.ActiveInd='A' AND BehaviourCalc.IsPartial='False' and DateDiff(day, BehaviourCalc.Date, getdate()) = 0") == false)
                                            {
                                                string adBcalc = " delete from BehaviourCalc where StudentId=" + sess.StudentId + " and DateDiff(day,Date, getdate()) <1 AND BehaviourCalc.IsPartial='False' and  MeasurmentId=" + int.Parse(HiddenFieldMeaId.Value);
                                                objData.Execute(adBcalc);
                                                string deRemider = " delete from BehaviorReminder where UserId=" + sess.LoginId + " ";
                                                objData.Execute(deRemider);

                                            }

                                            if (chkPartial.Checked == false)
                                            {
                                                result = "Update BehaviourDetails set StudentId='" + sess.StudentId + "',SchoolId=" + sess.SchoolId + ",ClassId=" + sess.Classid + ",IsAcceleration='" + acceleration + "',Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "',BehavDefinition='" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "',BehavStrategy='" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "',GoalDesc='" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "',Frequency='" + frq + "',YesOrNo='" + yesOrNo + "',IfPerInterval='" + perInterval + "',Duration='" + due + "',Opportunities='" + opp + "',PartialInterval='False',ActiveInd='" + stat + "',ModifiedBy='" + sess.LoginId + "',BehaviorIEPObjctve='" + clsGeneral.convertQuotes(txtbehIEPobjtve.Text) + "',BehaviorBasPerfLvl='" + clsGeneral.convertQuotes(txtbehBasperlvl.Text) + "',StartTime=null,EndTime=null,NumOfTimes=null,Period=null,Interval=null,StartDate=null,EndDate=null,Condition=null,Hr24=0,ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                            }                                                                                           
                                            else
                                            {
                                            //result = "Update BehaviourDetails set StudentId='" + sess.StudentId + "',SchoolId=" + sess.SchoolId + ",ClassId=" + sess.Classid + ",IsAcceleration='" + acceleration + "',Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "',BehavDefinition='" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "',BehavStrategy='" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "',GoalDesc='" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "',Frequency='" + frq + "',YesOrNo='" + yesOrNo + "',IfPerInterval='"+perInterval+"',Duration='" + due + "',PartialInterval='False',ActiveInd='" + stat + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                            result = "Update BehaviourDetails set StudentId='" + sess.StudentId + "',SchoolId=" + sess.SchoolId + ",ClassId=" + sess.Classid + ",IsAcceleration='" + acceleration + "',Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "',BehavDefinition='" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "',BehavStrategy='" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "',GoalDesc='" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "',Frequency='" + frq + "',YesOrNo='" + yesOrNo + "',IfPerInterval='" + perInterval + "',Duration='" + due + "',Opportunities='" + opp + "',PartialInterval='False',ActiveInd='" + stat + "',ModifiedBy='" + sess.LoginId + "',BehaviorIEPObjctve='" + clsGeneral.convertQuotes(txtbehIEPobjtve.Text) + "',BehaviorBasPerfLvl='" + clsGeneral.convertQuotes(txtbehBasperlvl.Text) + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                            }


                                            // txtStartTime.Text = "";                                        
                                            DropDownListAMPM.SelectedIndex = 0;
                                            txtNoOfTimes.Text = "";
                                            txtInterval.Text = "";
                                            txtEndsOn.Text = "";
                                            chkInactive.Visible = false;
                                            ddlRepeat.SelectedIndex = 0;
                                            rdoAcceleration.SelectedValue = "1";

                                        }
                                        int Final = 0;
                                        try
                                        {
                                            Final = objData.Execute(result);
                                        }
                                        catch (Exception eX)
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                                            errorflg++;
                                            tdMsg.InnerHtml = clsGeneral.failedMsg("Updating Failed !");
                                            throw eX;
                                        }


                                        if (CheckBoxLessonPlan.Text.Length != 0)
                                        {

                                            if (CheckBoxCostLessPlan.Checked == true)
                                            {


                                                if (objData.IFExists("Select MeasurementId from BehaviourLPRel where LessonPlanId=" + lessonPLId + " and MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'") == false)
                                                {
                                                    string strQerry = "";
                                                    string deltQuerry = "";
                                                    string queryGoal = "SELECT GoalId from Goal where GoalName='Behavior' and GoalCode='Behavior'";
                                                    int goalId = Convert.ToInt32(objData.FetchValue(queryGoal));
                                                    string sGl = SaveGoal(goalId.ToString(), true);
                                                    //new code : Delete old asigned lessonplan to current behavior 

                                                    deltQuerry = "DELETE FROM BehaviourLPRel WHERE MeasurementId = " + Convert.ToInt32(hdnid.Value);
                                                    objData.Execute(deltQuerry);

                                                    //

                                                    string BehaviourLPRelQueryupdateIn = "insert into BehaviourLPRel(MeasurementId,LessonPlanId,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) values('" + Convert.ToInt32(hdnid.Value) + "'," + lessonPLId + ",'A','" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "',(SELECT Convert(Varchar,getdate(),100)))";
                                                    int FinalUpdateIn = objData.Execute(BehaviourLPRelQueryupdateIn);

                                                    string rtn = SaveLessons(lessonPLId.ToString(), goalId.ToString());

                                                    //query to check current class status;
                                                    //string qryclass = "select ResidenceInd from [dbo].[Class] where classid=" + sess.Classid;
                                                    //bool isDay = Convert.ToBoolean(objData.FetchValue(qryclass));

                                                    //string DayFlag = "NULL";
                                                    //string ResiFlag = "NULL";
                                                    //if (!isDay)
                                                    //    DayFlag = "1";
                                                    //else
                                                    //    ResiFlag = "1";
                                                    //string queryGoal = "SELECT GoalId from Goal where GoalName='Behavior' and GoalCode='Behavior'";
                                                    //int goalId = Convert.ToInt32(objData.FetchValue(queryGoal));

                                                    //strQerry = "Insert Into dbo.StdtLessonPlan (SchoolId,StudentId,LessonPlanId,GoalId,IncludeIEP,AsmntYearId,ActiveInd,LessonPlanTypeDay,LessonPlanTypeResi,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
                                                    //strQerry += "Values(" + sess.SchoolId + "," + sess.StudentId + "," + lessonPLId + "," + goalId + ",0," + sess.YearId + ",   'A'," + DayFlag + "," + ResiFlag + "," + sess.LoginId + ",getdate()," + sess.LoginId + ",getdate()) ";
                                                    //int check = objData.ExecuteWithScope(strQerry);
                                                    //if (check > 0)
                                                    //{
                                                    //    //new code (while asigning new lesssonplan , create a new copy of lesson plan template(customise lessonplan) to that student)

                                                    //    clsAssignLessonPlan AssignLP = new clsAssignLessonPlan();
                                                    //    int dstmpId = AssignLP.SaveTemplateDetails(sess.SchoolId, sess.StudentId, lessonPLId, CheckBoxLessonPlan.Text, sess.LoginId, check);

                                                    //}

                                                    //
                                                }
                                                else
                                                {
                                                    string BehaviourLPRelQueryupdate = "Update BehaviourLPRel set LessonPlanId=" + lessonPLId + ",ActiveInd='A',ModifiedBy='" + sess.LoginId + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "' ";
                                                    int FinalUpdate = objData.Execute(BehaviourLPRelQueryupdate);
                                                }

                                            }
                                            else
                                            {
                                                if (objData.IFExists("Select MeasurementId from BehaviourLPRel where LessonPlanId=" + lessonPLId + " and MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'") == true)
                                                {
                                                    string BehaviourLPRelQueryupdate = "Update BehaviourLPRel set LessonPlanId=" + lessonPLId + ",ModifiedBy='" + sess.LoginId + "',ActiveInd='D',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                                    int FinalUpdate = objData.Execute(BehaviourLPRelQueryupdate);
                                                }

                                            }
                                        }

                                        if (chkInactive.Checked == true &&
                                                                        (Convert.ToString(objData.FetchValue("Select InactiveEvent from BehaviourDetails Where MeasurementId=" + Convert.ToInt32(hdnid.Value))) == ""))
                                        {
                                            string inactiveevent = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                                            string inactiveevents = "Update BehaviourDetails set InactiveEvent=CONVERT(datetime,'" + inactiveevent + "') where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                            objData.Execute(inactiveevents);

                                        }
                                        else if (chkInactive.Checked == false &&
                                                     (Convert.ToString(objData.FetchValue("Select InactiveEvent from BehaviourDetails Where MeasurementId=" + Convert.ToInt32(hdnid.Value))) != ""))
                                        {
                                            string inactiveevents = "Update BehaviourDetails set InactiveEvent=NULL where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                                            objData.Execute(inactiveevents);
                                        }
                                            

                                        if ((Final > 0) && (errorflg == 0))
                                            tdMsg.InnerHtml = clsGeneral.sucessMsg("Details updated Successfully");
                                        clearDataReset();
                                            btnSave.Text = "Save";

                                        }
                                        else
                                        {
                                            tdMsg.InnerHtml = clsGeneral.failedMsg("Behaviour Details already exist");
                                        }

                                    }

                                    GrdMeasurement.EditIndex = -1;
                                    filldetails();

                                        //btnSave.Text = "Save";
                                }
                                else
                                {
                                    tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid Time");
                                }
                            }
                            else
                            {
                                tdMsg.InnerHtml = clsGeneral.warningMsg("Any one of Measurment should be selected");
                            }
                        }
                        else
                        {
                            tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid Dates");
                        }

                    }
                    
                    else
                    {
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Enter Partial Intervals");
                    }
                }
                else
                {
                    if (objData.IFExists("Select MeasurementId from BehaviourDetails where StudentId='" + sess.StudentId + "' and Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "' and (ActiveInd='A' OR ActiveInd='N') and MeasurementId <> " + Convert.ToInt32(hdnid.Value) +" ") == false)
                    {
                    if (rdoAcceleration.SelectedItem.Text == "Acceleration")
                    {
                        acceleration = true;
                    }
                    else
                    {
                        acceleration = false;
                    }
                    ///Yes/No check box
                    ///
                    Boolean perInterval = false;
                    if (chkYesOrNo.Checked == true)
                    {
                        if (rdoSumTotal.SelectedItem.Text == "%Interval")
                        {
                            perInterval = true;
                        }
                        else
                        {
                            perInterval = false;
                        }
                    }

                    string Inactiveresult = "";
                    if (chkPartial.Checked == true)
                    {
                        string strtimetemp = amPmTo24hourConverter(ddlHour.SelectedItem.Text + ":" + ddlMinute.SelectedItem.Text + ":" + "00" + DropDownListAMPM.SelectedItem.Text);
                        string endTimeTemp = amPmTo24hourConverter(ddlEndHr.SelectedItem.Text + ":" + ddlEndMin.SelectedItem.Text + ":" + "00" + ddlEndFrmt.SelectedItem.Text);
                        StartTime = DateTime.Parse(strtimetemp);
                        EndTime = DateTime.Parse(endTimeTemp);
                        Inactiveresult = "Update BehaviourDetails set IsAcceleration='" + acceleration + "',Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "',Frequency='" + chkFrequency.Checked + "',Duration='" + chkDuration.Checked + "',PartialInterval='" + chkPartial.Checked + "',Hr24='" + chk24Hr.Checked + "',YesOrNo='" + chkYesOrNo.Checked + "',IfPerInterval='"+perInterval+"',StartTime='" + StartTime.TimeOfDay + "',EndTime='" + EndTime.TimeOfDay + "',NumOfTimes='" + int.Parse(txtNoOfTimes.Text) + "',Period='" + int.Parse(txtPeriod.Text) + "',Interval='" + int.Parse(txtInterval.Text) + "',ActiveInd='N',BehavDefinition='" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "',BehavStrategy='" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "',GoalDesc='" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)),StartDate='" + txtStartDate.Text + "',EndDate='" + txtEndsOn.Text + "' where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                    }
                    else
                    {
                        Inactiveresult = "Update BehaviourDetails set IsAcceleration='" + acceleration + "',Behaviour='" + clsGeneral.convertQuotes(txtBehaviour.Text.Trim()) + "',Frequency='" + chkFrequency.Checked + "',Duration='" + chkDuration.Checked + "',PartialInterval='" + chkPartial.Checked + "',YesOrNo='" + chkYesOrNo.Checked + "',IfPerInterval='"+perInterval+"',ActiveInd='N',BehavDefinition='" + clsGeneral.convertQuotes(txtBehavDefinition.Text.Trim()) + "',BehavStrategy='" + clsGeneral.convertQuotes(txtBehavStrategy.Text.Trim()) + "',GoalDesc='" + clsGeneral.convertQuotes(txtGoalDesc.Text.Trim()) + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                    }
                    objData.Execute(Inactiveresult);
                    if (chkInactive.Checked == true &&
                                 (Convert.ToString(objData.FetchValue("Select InactiveEvent from BehaviourDetails Where MeasurementId=" + Convert.ToInt32(hdnid.Value))) == ""))
                    {
                        string inactiveevent = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                        string inactiveevents = "Update BehaviourDetails set InactiveEvent=CONVERT(datetime,'" + inactiveevent + "') where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";                        
                        objData.Execute(inactiveevents);

                    }
                    else if (chkInactive.Checked == false &&
                                 (Convert.ToString(objData.FetchValue("Select InactiveEvent from BehaviourDetails Where MeasurementId=" + Convert.ToInt32(hdnid.Value))) != ""))
                    {
                        string inactiveevents = "Update BehaviourDetails set InactiveEvent=NULL where MeasurementId='" + Convert.ToInt32(hdnid.Value) + "'";
                        objData.Execute(inactiveevents);
                    }
                    
                    GrdMeasurement.EditIndex = -1;
                    filldetails();
                    btnSave.Text = "Save";
                    cleardata();
                    clearDataReset();
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Details updated Successfully");
                }
                    else
                                        {
                                            tdMsg.InnerHtml = clsGeneral.failedMsg("Behaviour Details already exist");
            }
                }
            }

            else
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please enter the Behaviour");
            }

        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Student");
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
    }



    protected string SaveGoal(string goalid, bool flag)
    {
        try
        {
            clsData oData = new clsData();
            clsSession oSession = (clsSession)Session["UserSession"];
            curesesid = this.Session.SessionID.ToString();
            HostName = Dns.GetHostName();
            userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "BehaviourMaintanance:SaveGoal";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            oSession = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
       
            object objIEP = "";
            if (oSession != null)
            {

                //string selQry = "SELECT StdtIEPId FROM(SELECT IEP.StdtIEPId,LookupName,IEP.EffStartDate,RANK() OVER " +
                //                "(PARTITION BY StudentId " +
                //                "ORDER BY CASE(LookupName) WHEN 'In Progress' THEN 1 WHEN 'Pending Approval' " +
                //                "THEN 2 WHEN 'Approved' THEN 3 END,IEP.EffStartDate DESC) as rank " +
                //                "FROM (StdtIEP IEP INNER JOIN AsmntYear Yr ON Yr.AsmntYearId=IEP.AsmntYearId) " +
                //                "INNER JOIN LookUp LU ON LU.LookupId=IEP.StatusId " +
                //                "WHERE StudentId=" + oSession.StudentId + " AND IEP.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' AND LookupName<>'Expired') A WHERE rank=1";
                //
                if (oSession.SchoolId == 1)
                {
                    string selQry = "select isnull(StdtIEPId,0) StdtIEPId from StdtIEP I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                       + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                       + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A'  ";
                    objIEP = oData.FetchValue(selQry);
                }
                else if (oSession.SchoolId == 2)
                {
                    string selQry = "select isnull(StdtIEP_PEId,0) StdtIEP_PEId from StdtIEP_PE I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                       + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                       + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A'  ";
                    objIEP = oData.FetchValue(selQry);
                }
                bool glExists = false;
                if (objIEP == null)
                    objIEP = 0;
                if (Convert.ToInt32(objIEP) == 0)
                {
                    glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A'");
                }
                else
                {
                    glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");

                }
                //  glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");
                if (!glExists)  //check whether the goal is already existed or not.....
                {
                    object objYearId = oData.FetchValue("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
                    if (objYearId != null)
                    {
                        object objStat = oData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='Goal Status' AND LookupName='In Progress'");
                        if (objStat != null)
                        {
                            string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StatusId,ActiveInd,IEPGoalNo," +
                                            "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + objYearId.ToString() + ",0," +
                                            "" + objStat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                            if (flag)
                            {
                                if (Convert.ToInt32(objIEP) == 0)
                                {
                                    insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StatusId,ActiveInd,IEPGoalNo," +
                                            "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + objYearId.ToString() + ",0," +
                                            "" + objStat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                                else
                                {
                                    insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StdtIEPId,StatusId,ActiveInd,IEPGoalNo," +
                                                "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + objYearId.ToString() + ",1," + objIEP.ToString() + "," +
                                                "" + objStat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                            }

                            int stdtglID = oData.ExecuteWithScope(insGoal);
                            if (!flag)
                            {
                                objIEP = 0;
                            }
                            //returns the id..
                            return stdtglID.ToString() + "*" + objIEP.ToString();
                        }
                        else
                        {
                            return "0";
                        }

                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    if (flag)
                    {
                        object objGlId = oData.FetchValue("SELECT StdtGoalId FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + ")");
                        if (objGlId == null)
                        {
                            objGlId = oData.FetchValue("SELECT StdtGoalId FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND IncludeIEP=0");
                        }
                        if (objGlId != null)
                        {
                            if (objIEP != null)
                                if (Convert.ToInt32(objIEP) != 0)
                                {
                                    string updGoal = "UPDATE StdtGoal SET IncludeIEP=1,StdtIEPId=" + objIEP.ToString() + ",ModifiedBy=" + oSession.LoginId + ",ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StdtGoalId=" + objGlId.ToString();
                                    oData.Execute(updGoal);
                                }
                        }
                    }
                    //returns 'exists' if already exists....
                    return "exists";
                }

            }
            else
            {
                return "0";
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public string SaveLessons(string LPid, string goalid)
    {
        SqlTransaction Trans = null;
        SqlConnection Con = new SqlConnection();
        clsData oData = new clsData();
        try
        {

            Con = oData.Open();
            Trans = Con.BeginTransaction();
            object objIEP = "";
            clsSession oSession = (clsSession)Session["UserSession"];
            curesesid = this.Session.SessionID.ToString();
            HostName = Dns.GetHostName();
            userAgent = HttpContext.Current.Request.UserAgent.ToString();
            ip = clsGeneral.GetIPAddress();
            ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
            Pagepath = "BehaviourMaintanance:SaveLessons";
            if (Prevsess != null)
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + oSession.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            else
            {
                sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + oSession.LoginTime.ToString() + ',' + oSession.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + oSession.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + oSession.SchoolId + ',' + "Log" + ',' + oSession.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + oSession.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + oSession.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
            }
            oSession = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, oSession, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
       
            if (oSession != null)
            {
                if (oSession.SchoolId == 2)
                {
                    string selQry = "select isnull(StdtIEP_PEId,0) StdtIEP_PEId from StdtIEP_PE I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                        + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                        + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' ";
                    objIEP = oData.FetchValueTrans(selQry, Trans, Con);
                }
                else if (oSession.SchoolId == 1)
                {
                    string selQry = "select isnull(StdtIEPId,0) StdtIEPId from StdtIEP I inner join AsmntYear yr on I.AsmntYearId=yr.AsmntYearId "
                        + " where StatusId=(select LookUpId from [LookUp] where LookupName='In Progress' and LookupType='IEP Status')"
                        + " and StudentId=" + oSession.StudentId + " AND I.SchoolId=" + oSession.SchoolId + " AND Yr.CurrentInd='A' ";
                    objIEP = oData.FetchValueTrans(selQry, Trans, Con);
                }

                if (objIEP == null)
                    objIEP = 0;
                //query to check current class status;
                string qryclass = "select ResidenceInd from [dbo].[Class] where classid=" + oSession.Classid;
                bool isDay = Convert.ToBoolean(oData.FetchValueTrans(qryclass, Trans, Con));

                bool lpDupExists = oData.IFExistsWithTranss("SELECT * FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " AND ActiveInd='A'", Trans, Con);
                bool lpDelExists = oData.IFExistsWithTranss("SELECT * FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " AND ActiveInd='D'", Trans, Con);
                // bool lpExists = oData.IFExists("SELECT * FROM StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND LessonPlanId=" + LPid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");

                if (!lpDupExists && !lpDelExists)  //chech whether the LP is exists or not.....
                {
                    object objYearId = oData.FetchValueTrans("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'", Trans, Con);
                    if (objYearId != null)
                    {
                        string strQuery = "SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + LPid + " and StudentId IS NULL ";
                        object flag = oData.FetchValueTrans(strQuery, Trans, Con);
                        object objStat = oData.FetchValueTrans("SELECT LookupId FROM LookUp WHERE LookupType='LP Status' AND LookupName='In Progress'", Trans, Con);
                        object objLPname = oData.FetchValueTrans("SELECT LessonPlanName FROM LessonPlan WHERE LessonPlanId=" + LPid + "", Trans, Con);
                        if ((objStat != null) && (objLPname != null))
                        {
                            string insLP = "";
                            if (Convert.ToInt32(objIEP) == 0)
                            {

                                if (Convert.ToInt32(flag) > 0)
                                {
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + ",0," +
                                                                       "" + objStat.ToString() + ",'A',0," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                                else
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + ",0," +
                                                                       "" + objStat.ToString() + ",'A',1," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";

                            }
                            else
                            {
                                if (Convert.ToInt32(flag) > 0)
                                {
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,StdtIEPId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + "," + objIEP.ToString() + ",1," +
                                                                       "" + objStat.ToString() + ",'A',0," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                                }
                                else
                                    insLP = "INSERT INTO StdtLessonPlan(SchoolId,StudentId,LessonPlanId,GoalId,AsmntYearId,StdtIEPId,IncludeIEP,StatusId," +
                                                    "ActiveInd,isDynamic,CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + LPid + "," + goalid + "," + objYearId.ToString() + "," + objIEP.ToString() + ",1," +
                                                                       "" + objStat.ToString() + ",'A',1," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";
                            }

                            int stdtLPID = oData.ExecuteWithScopeandConnection(insLP, Con, Trans);

                            string condtn = "";// 

                            if (!isDay)
                                condtn = "SET LessonPlanTypeDay=1";
                            else
                                condtn = "SET LessonPlanTypeResi=1";

                            oData.ExecuteWithScopeandConnection("UPDATE StdtLessonPlan " + condtn + " WHERE StdtLessonPlanId=" + stdtLPID + "", Con, Trans);

                            //bool glExists = oData.IFExists("SELECT * FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND GoalId=" + goalid + " AND ActiveInd='A' AND (IncludeIEP=0 OR (IncludeIEP=1 AND StdtIEPId=" + objIEP.ToString() + "))");
                            //if (!glExists)  //check whether the goal is already existed or not.....
                            //{
                            //    object obj_YearId = oData.FetchValue("SELECT AsmntYearId FROM AsmntYear WHERE CurrentInd='A'");
                            //    if (obj_YearId != null)
                            //    {
                            //        object obj_Stat = oData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='Goal Status' AND LookupName='In Progress'");
                            //        if (obj_Stat != null)
                            //        {
                            //            string insGoal = "INSERT INTO StdtGoal(SchoolId,StudentId,GoalId,AsmntYearId,IncludeIEP,StatusId,ActiveInd,IEPGoalNo," +
                            //                            "CreatedBy,CreatedOn) VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + goalid + "," + obj_YearId.ToString() + ",0," +
                            //                            "" + obj_Stat.ToString() + ",'A',(SELECT ISNULL(MAX(IEPGoalNo),0)+1 FROM StdtGoal WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND ActiveInd='A')," + oSession.LoginId + ",(SELECT convert(varchar, getdate(), 100)))";

                            //            int stdtglID = oData.ExecuteWithScope(insGoal);
                            //            //returns the id..
                            //            return stdtglID.ToString();
                            //        }
                            //}


                            clsAssignLessonPlan objTemplateAssign = new clsAssignLessonPlan();
                            int TempId = objTemplateAssign.SaveTemplateDetails(oSession.SchoolId, oSession.StudentId, Convert.ToInt32(LPid), objLPname.ToString(), oSession.LoginId, stdtLPID, Con, Trans);
                            oData.ExecuteWithScopeandConnection("UPDATE DSTempHdr SET PrevStatus='IEP-'+'" + objIEP + "' WHERE DSTempHdrId='" + TempId + "'", Con, Trans);
                            clsDocumentasBinary objBinary = new clsDocumentasBinary();
                            int templateId = Convert.ToInt32(oData.FetchValueTrans("SELECT DSTempHdrId FROM dbo.DSTempHdr WHERE LessonPlanId=" + LPid + " and StudentId IS NULL", Trans, Con));
                            DataTable dtdoc = oData.ReturnDataTableWithTransaction("SELECT LPDoc FROM LPDoc WHERE DSTempHdrId=" + templateId + "", Con, Trans, false);
                            if (dtdoc != null)
                            {
                                if (dtdoc.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtdoc.Rows)
                                    {
                                        string strquerry = "INSERT INTO LPDoc(SchoolId,DSTempHdrId,DocURL,CreatedBy,CreatedOn) SELECT SchoolId," + TempId + ",DocURL," + oSession.LoginId + ",GETDATE() FROM LPDoc WHERE LPDoc='" + row["LPDoc"].ToString() + "'";
                                        int docid = oData.ExecuteWithScopeandConnection(strquerry, Con, Trans);
                                        string binarydata = "SELECT Data,DocumentName FROM binaryFiles WHERE DocId=" + row["LPDoc"].ToString() + " AND type='LP_DOC'";
                                        DataTable dtbinary = oData.ReturnDataTableWithTransaction(binarydata, Con, Trans, false);
                                        byte[] myData = (byte[])dtbinary.Rows[0]["Data"];
                                        string filename = Convert.ToString(dtbinary.Rows[0]["DocumentName"]);
                                        int binaryid = objBinary.saveDocument(myData, filename, "", "LP_DOC", docid, "LessonPlanDoc", oSession.SchoolId, 0, oSession.LoginId);
                                    }
                                }
                            }
                            //string insDs = "INSERT INTO DSTempHdr(DSTemplateName,SchoolId,StudentId,LessonPlanId,CreatedBy,CreatedOn,StatusId) " +
                            //               "VALUES ('" + objLPname.ToString().Trim() + "'," + oSession.SchoolId + "," + oSession.StudentId + "," + LPid.ToString() + "," + oSession.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," +
                            //               "(select LookupId from LookUp where LookupName = 'In Progress' and LookupType = 'TemplateStatus'))";
                            //oData.ExecuteWithScope(insDs);
                            ////returns the id ...
                            int isItDay = 0;
                            if (isDay)
                                isItDay = 0;
                            else
                                isItDay = 1;
                            oData.CommitTransation(Trans, Con);
                            return stdtLPID.ToString() + "*" + objIEP.ToString() + "*" + isItDay.ToString();

                        }
                        else
                        {
                            //oData.RollBackTransation(Trans, Con);
                            return "0";
                        }

                    }
                    else
                    {
                        return "0";
                    }

                }
                else
                {
                    if (lpDelExists)
                    {
                        clsAssignLessonPlan objTemplateAssign = new clsAssignLessonPlan();
                        object objLPname = oData.FetchValueTrans("SELECT LessonPlanName FROM LessonPlan WHERE LessonPlanId=" + LPid, Trans, Con);

                        int result = oData.ExecuteWithScopeandConnection("UPDATE StdtLessonPlan SET ActiveInd='A' WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " ", Con, Trans);
                        int stdtLessonPID = Convert.ToInt32(oData.FetchValueTrans("SELECT StdtLessonPlanId FROM  StdtLessonPlan WHERE StudentId=" + oSession.StudentId + " AND LessonPlanId=" + LPid + " AND ActiveInd='A'", Trans, Con));
                        int TempId = objTemplateAssign.SaveTemplateDetails(oSession.SchoolId, oSession.StudentId, Convert.ToInt32(LPid), objLPname.ToString(), oSession.LoginId, stdtLessonPID, Con, Trans);
                        int isItDay = 0;
                        if (isDay)
                            isItDay = 0;
                        else
                            isItDay = 1;
                        oData.CommitTransation(Trans, Con);
                        return stdtLessonPID.ToString() + "*" + objIEP.ToString() + "*" + isItDay.ToString();

                    }
                    //returns 'exists' if already exists....
                    return "exists";
                }
            }
            else
            {
                return "0";
            }

        }
        catch (Exception ex)
        {
            oData.RollBackTransation(Trans, Con);
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + ex.ToString());
            throw ex;
        }


    }

    private void cleardata()
    {
        txtBehaviour.Text = "";
        txtBehavDefinition.Text = "";
        txtBehavStrategy.Text = "";
        txtGoalDesc.Text = "";

        chkFrequency.Checked = false;
        chkDuration.Checked = false;
        chkInactive.Checked = false;
        chkInactive.Visible = false;

        lblCalc.Visible = false;
        rdoSumTotal.Visible = false;
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;

        // chkhr.Checked = false;
        // chkmin.Checked = false;
        // chksec.Checked = false;
        //  chkhr.Enabled = false;
        //  chkmin.Enabled = false;
        //  chksec.Enabled = false;
    }
    private void cleardataWithOutDdl()
    {
        txtBehaviour.Text = "";
        txtBehavDefinition.Text = "";
        txtBehavStrategy.Text = "";
        txtGoalDesc.Text = "";
        txtNoOfTimes.Text = "";
        txtInterval.Text = "";
        ddlHour.SelectedIndex = 0;
        ddlMinute.SelectedIndex = 0;
        //ddlSecond.SelectedIndex = 0;
        DropDownListAMPM.SelectedIndex = 0;

        chkFrequency.Checked = false;
        chkDuration.Checked = false;
        chkYesOrNo.Checked = false;
        lblCalc.Visible = false;
        rdoSumTotal.Visible = false;
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;

        // chkhr.Checked = false;
        // chkmin.Checked = false;
        // chksec.Checked = false;
        //  chkhr.Enabled = false;
        //  chkmin.Enabled = false;
        //  chksec.Enabled = false;
    }




    private void filldetails()
    {
        if (Session["Visibility"] != "1")
            BehNamediv.Visible = false;

        objData = new clsData();
        objdataClass = new DataClass();
        //select BehaviourDetails.MeasurementId,BehaviourDetails.PartialInterval,BehaviourDetails.Duration,BehaviourDetails.Frequency,(select BehaviourLPId from BehaviourLPRel where BehaviourLPRel.MeasurementId=BehaviourDetails.MeasurementId and BehaviourLPRel.ActiveInd='A') as cost from BehaviourDetails where BehaviourDetails.StudentId=1 and BehaviourDetails.ActiveInd='A'
        //"SELECT mst.Behaviour,mst.Frequency,mst.Duration,mst.PartialInterval,mst.MeasurementId,mst.MeasurementId from BehaviourDetails mst where mst.StudentId=" + Convert.ToInt32(ddlStudent.SelectedItem.Value) + " and mst.ActiveInd='A'";
        // string strQuery = "select BehaviourDetails.Behaviour,BehaviourDetails.MeasurementId,BehaviourDetails.PartialInterval,BehaviourDetails.Duration,BehaviourDetails.Frequency,(select BehaviourLPId from BehaviourLPRel where BehaviourLPRel.MeasurementId=BehaviourDetails.MeasurementId and BehaviourLPRel.ActiveInd='A') as cost from BehaviourDetails where BehaviourDetails.StudentId=" + sess.StudentId + " and BehaviourDetails.ActiveInd='A'";

        string strQuery = "SELECT  Bdet.Behaviour,Bdet.MeasurementId,Bdet.PartialInterval,Bdet.YesOrNo,Bdet.Duration,Bdet.Frequency,Bdet.ActiveInd,BlpRel.BehaviourLPId as cost,Bdet.IsAcceleration,Bdet.BehavDefinition,Bdet.BehavStrategy " +
                           "FROM BehaviourDetails Bdet LEFT JOIN BehaviourLPRel BlpRel on Bdet.MeasurementId = BlpRel.MeasurementId " +
                            "WHERE Bdet.StudentId = " + sess.StudentId + " AND (Bdet.ActiveInd = 'A' OR Bdet.ActiveInd = 'N') order by Bdet.CreatedOn";

        DataTable Dt = objdataClass.fillData(strQuery);
        if (Dt.Rows.Count > 0)
        {
            GrdMeasurement.Visible = true;
            GrdMeasurement.DataSource = Dt;
            GrdMeasurement.DataBind();
        }
        else
            GrdMeasurement.Visible = false;
    }


    protected void GrdMeasurement_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Disable == true)
        {
            GrdMeasurement.Columns[6].Visible = false;
        }
        else
        {
            GrdMeasurement.Columns[6].Visible = true;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {/*
            HiddenField hff = (HiddenField)e.Row.FindControl("hdnfrequency");
            CheckBox chkf = (CheckBox)e.Row.FindControl("chkfrequency");
            HiddenField hfd = (HiddenField)e.Row.FindControl("hdnduration");
            CheckBox chkd = (CheckBox)e.Row.FindControl("chkduration");
            try
            {
                if (Convert.ToBoolean(hff.Value) == true)
                {
                    chkf.Checked = true;
                }
                if (hfd.Value != "")
                    chkd.Checked = true;
            }
            catch
            {
                
            }
            */
        }
    }

    protected void GrdMeasurement_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdMeasurement.PageIndex = e.NewPageIndex;
        filldetails();
    }
    protected void chkhr_CheckedChanged(object sender, EventArgs e)
    {
        //  chkmin.Checked = false;
        //  chksec.Checked = false;
        GrdMeasurement.EditIndex = -1;
    }
    protected void chkmin_CheckedChanged(object sender, EventArgs e)
    {
        //   chkhr.Checked = false;
        //   chksec.Checked = false;
        GrdMeasurement.EditIndex = -1;
    }
    protected void chksec_CheckedChanged(object sender, EventArgs e)
    {
        //  chkmin.Checked = false;
        //   chkhr.Checked = false;
        GrdMeasurement.EditIndex = -1;
    }

    protected void GrdMeasurement_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        tdMsg.InnerHtml = "";
        objData = new clsData();
        objdataClass = new DataClass();
        hdnid.Value = e.CommandArgument.ToString();
        if (e.CommandName == "Edit")
        {
            clearDataReset();
            string strQuery = "SELECT * from BehaviourDetails where MeasurementId=" + Convert.ToInt32(e.CommandArgument) + " and (ActiveInd='A' OR ActiveInd='N')";

            DataTable Dt = objdataClass.fillData(strQuery);
            if (Dt.Rows.Count > 0)
            {
                btnSave.Text = "Update";

                btnCancel.Visible = true;
                HiddenFieldMeaId.Value = Dt.Rows[0][1].ToString();
                // bool frequency = Convert.ToBoolean(Dt.Rows[0]["Frequency"]);
                // if (frequency == true) chkFrequency.Checked = true;
                chkFrequency.Checked = Convert.ToBoolean(Dt.Rows[0]["Frequency"]);
                chkDuration.Checked = Convert.ToBoolean(Dt.Rows[0]["Duration"]);
                
                try
                {
                    chkPartial.Checked = Convert.ToBoolean(Dt.Rows[0]["PartialInterval"]);
                    chkYesOrNo.Checked = Convert.ToBoolean(Dt.Rows[0]["YesOrNo"]);
                    ///IF Interval is selected then choose frequency or Yes/No 
                    ///
                    if (chkPartial.Checked == true)
                    {
                        lblCalcOpp.Visible = false;
                        chkOpportunities.Visible = false;
                        if (chkFrequency.Checked == true)
                        {
                            chkYesOrNo.Checked = false;                          
                        }
                        if (chkYesOrNo.Checked == true)
                        {
                            chkFrequency.Checked = false;                          
                            lblCalc.Visible = true;
                            rdoSumTotal.Visible = true;
                            string chkSum = (Dt.Rows[0]["IfPerInterval"]).ToString();
                            if (chkSum == "True")
                            {
                                rdoSumTotal.SelectedValue = "1";
                            }
                            else
                            {
                                rdoSumTotal.SelectedValue = "0";
                            }
                        }
                        //else
                        //{
                        //    lblCalc.Visible = false;
                        //    rdoSumTotal.Visible = false;
                        //}
                    }
                    else
                    {
                        if (chkYesOrNo.Checked == true && chkDuration.Checked == false && chkFrequency.Checked == false)
                        {
                            lblCalcOpp.Visible = true;
                            chkOpportunities.Visible = true;
                            try
                            {
                                chkOpportunities.Checked = Convert.ToBoolean(Dt.Rows[0]["Opportunities"]);
                            }
                            catch
                            { 
                            }
                        }
                        lblCalc.Visible = false;
                        rdoSumTotal.Visible = false;
                    }                   
                }
                catch
                {
                    chkYesOrNo.Checked = false;
                }
                object chckAclDbNull = (Dt.Rows[0]["IsAcceleration"]);
                string chckAcl = (Dt.Rows[0]["IsAcceleration"]).ToString();

                if (chckAclDbNull == DBNull.Value)
                {
                    Dt.Rows[0]["IsAcceleration"] = true;
                    chckAcl = (Dt.Rows[0]["IsAcceleration"]).ToString();
                }
                if (chckAcl == "True")
                {
                    rdoAcceleration.SelectedValue = "1";
                }
                else
                {
                    rdoAcceleration.SelectedValue = "0";
                }
                if (Dt.Rows[0]["ActiveInd"].ToString() == "N")
                {
                    chkInactive.Checked = true;
                }


                chkInactive.Visible = true;
                string duration = Dt.Rows[0]["Duration"].ToString();
                txtBehaviour.Text = Dt.Rows[0]["Behaviour"].ToString();
                txtBehavDefinition.Text = Dt.Rows[0]["BehavDefinition"].ToString();
                txtBehavStrategy.Text = Dt.Rows[0]["BehavStrategy"].ToString();
                txtGoalDesc.Text = Dt.Rows[0]["GoalDesc"].ToString();

                if (Dt.Rows[0]["BehaviorIEPObjctve"].ToString() != "")
                {
                    txtbehIEPobjtve.Text = Dt.Rows[0]["BehaviorIEPObjctve"].ToString();
                }
                else
                {
                    txtbehIEPobjtve.Text = "";
                }

                if (Dt.Rows[0]["BehaviorBasPerfLvl"].ToString() != "")
                {
                    txtbehBasperlvl.Text = Dt.Rows[0]["BehaviorBasPerfLvl"].ToString();
                }
                else
                {
                    txtbehBasperlvl.Text = "";
                }

                if (Convert.ToBoolean(Dt.Rows[0]["PartialInterval"]) == true)
                {
                    int hour = 0;
                    int hourEnd = 0;
                    string starttime = Dt.Rows[0]["StartTime"].ToString();

                    string[] Alltime = starttime.Split(':');
                    string h = Convert.ToString(Alltime[0]);
                    string m = Convert.ToString(Alltime[1]);
                    string s = Convert.ToString(Alltime[2]);

                    string endTime = Dt.Rows[0]["EndTime"].ToString();
                    if (endTime != "")
                    {
                        string[] AlltimeEnd = endTime.Split(':');
                        string hEnd = Convert.ToString(AlltimeEnd[0]);
                        string mEnd = Convert.ToString(AlltimeEnd[1]);
                        string sEnd = Convert.ToString(AlltimeEnd[2]);

                        hourEnd = Convert.ToInt32(hEnd);
                        if (Convert.ToInt32(hEnd) > 12)
                        {
                            ddlEndFrmt.SelectedIndex = 1;
                            hourEnd = Convert.ToInt32(hEnd) - 12;
                        }
                        else if (Convert.ToInt32(hEnd) == 12)
                        {
                            ddlEndFrmt.SelectedIndex = 1;
                        }
                        else
                        {
                            ddlEndFrmt.SelectedIndex = 0;
                        }
                        if (hourEnd.ToString().Length == 1)
                        {
                            ddlEndHr.SelectedIndex = ddlEndHr.Items.IndexOf(ddlEndHr.Items.FindByText("0" + hourEnd.ToString()));
                        }
                        else
                        {
                            ddlEndHr.SelectedIndex = ddlEndHr.Items.IndexOf(ddlEndHr.Items.FindByText(hourEnd.ToString()));
                        }
                        ddlEndMin.SelectedIndex = ddlEndMin.Items.IndexOf(ddlEndMin.Items.FindByText(mEnd));
                    }

                    hour = Convert.ToInt32(h);
                    if (Convert.ToInt32(h) > 12)
                    {
                        DropDownListAMPM.SelectedIndex = 1;
                        hour = Convert.ToInt32(h) - 12;
                    }
                    else if (Convert.ToInt32(h) == 12)
                    {
                        DropDownListAMPM.SelectedIndex = 1;
                    }
                    else
                    {
                        DropDownListAMPM.SelectedIndex = 0;
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


                    //ddlSecond.SelectedIndex = ddlSecond.Items.IndexOf(ddlSecond.Items.FindByText(s));
                    //DateTime sTime = new DateTime();
                    //sTime = DateTime.Parse(Dt.Rows[0]["StartTime"].ToString());
                    //if (sTime.Hour > 12)
                    //{
                    //    DropDownListAMPM.SelectedIndex = 1;
                    //    sTime = sTime.AddHours(12);
                    //}
                    //else
                    //{
                    //    string s = sTime.ToShortTimeString();
                    //}

                    chk24Hr.Checked = Convert.ToBoolean(Dt.Rows[0]["Hr24"]);

                    txtNoOfTimes.Text = Dt.Rows[0]["NumOfTimes"].ToString();

                    txtPeriod.Text = Dt.Rows[0]["Period"].ToString();



                    DateTime today = DateTime.Today; // As DateTime


                    txtStartDate.Text = DateTime.Parse(Dt.Rows[0]["StartDate"].ToString()).ToString("MM/dd/yyyy").Replace("-", "/"); // As String


                    DateTime Enddate = DateTime.Parse(Dt.Rows[0]["EndDate"].ToString());

                    txtEndsOn.Text = Enddate.ToString("MM/dd/yyyy").Replace("-", "/");

                    string[] ddlrepestCondition = Dt.Rows[0]["Condition"].ToString().Split(',');
                    ddlRepeat.SelectedIndex = int.Parse(ddlrepestCondition[0]);
                    if (ddlRepeat.SelectedIndex == 1)
                    {
                        checkBxWeek.Style.Add("display", "block");
                        for (int CounterChkBx = 0; CounterChkBx < ddlrepestCondition.Length; CounterChkBx++)
                        {
                            if (ddlrepestCondition[CounterChkBx] == "sun")
                            {
                                ChbxSun.Checked = true;
                            }
                            if (ddlrepestCondition[CounterChkBx] == "mon")
                            {
                                ChbxMon.Checked = true;
                            }
                            if (ddlrepestCondition[CounterChkBx] == "tue")
                            {
                                ChbxTue.Checked = true;
                            }
                            if (ddlrepestCondition[CounterChkBx] == "wed")
                            {
                                ChbxWed.Checked = true;
                            }
                            if (ddlrepestCondition[CounterChkBx] == "thu")
                            {
                                ChbxThu.Checked = true;
                            }
                            if (ddlrepestCondition[CounterChkBx] == "fri")
                            {
                                ChbxFri.Checked = true;
                            }
                            if (ddlrepestCondition[CounterChkBx] == "sat")
                            {
                                ChbxSat.Checked = true;
                            }


                        }
                    }
                    else
                    {
                        checkBxWeek.Style.Add("display", "none");
                    }




                    txtInterval.Text = Dt.Rows[0]["Interval"].ToString();
                    chkPartial.Checked = true;
                    Panal_Partia.Visible = true;
                    chkYesOrNo.Visible = true;
                    if (chkPartial.Checked == true)
                    {
                        Panal_Partia.Visible = true;
                        chkYesOrNo.Visible = true;
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
                    }

                }
                else
                {
                    chkPartial.Checked = false;
                    Panal_Partia.Visible = false;
                    //chkYesOrNo.Visible = false;
                    DropDownListAMPM.SelectedIndex = 0;
                    txtNoOfTimes.Text = "";
                    txtInterval.Text = "";

                }
                //  if (duration != "")
                // {
                //    chkDuration.Checked = true;
                //         chksec.Enabled = true;
                //         chkmin.Enabled = true;
                //         chkhr.Enabled = true;
                //    if (duration == "H") chkhr.Checked = true;
                //    else if (duration == "M") chkmin.Checked = true;
                //    else if (duration == "S") chksec.Checked = true;
                // }
            }



            string strQueryLessonPlan = "  select LessonPlan.LessonPlanId,LessonPlan.LessonPlanName from BehaviourLPRel  inner join LessonPlan on LessonPlan.LessonPlanId=BehaviourLPRel.LessonPlanId where MeasurementId=" + Convert.ToInt32(e.CommandArgument) + " and BehaviourLPRel.ActiveInd='A'";

            DataTable DtLessonPlan = objdataClass.fillData(strQueryLessonPlan);
            if (DtLessonPlan.Rows.Count > 0)
            {
                ButtonAddLessionPlan.Visible = true;
                CheckBoxLessonPlan.Visible = true;
                CheckBoxCostLessPlan.Checked = true;
                CheckBoxLessonPlan.Checked = true;

                foreach (DataRow dr in DtLessonPlan.Rows)
                {
                    CheckBoxLessonPlan.Text = dr[1].ToString();
                    lessonPLId = int.Parse(dr[0].ToString());

                }

            }
            else
            {
                ButtonAddLessionPlan.Visible = false;
                CheckBoxLessonPlan.Visible = false;
                CheckBoxLessonPlan.Checked = false;
                CheckBoxCostLessPlan.Checked = false;
                CheckBoxLessonPlan.Text = "";

            }

            if (chk24Hr.Checked == true)
            {
                ddlEndHr.Enabled = false;
                ddlEndMin.Enabled = false;
                ddlEndFrmt.Enabled = false;

                ddlHour.Enabled = false;
                ddlMinute.Enabled = false;
                DropDownListAMPM.Enabled = false;
            }
            else
            {
                ddlEndHr.Enabled = true;
                ddlEndMin.Enabled = true;
                ddlEndFrmt.Enabled = true;

                ddlHour.Enabled = true;
                ddlMinute.Enabled = true;
                DropDownListAMPM.Enabled = true;
            }

        }
        else if (e.CommandName == "Delete")
        {
            string strSql = "Select LessonPlanId from dbo.BehaviourLPRel where MeasurementId='" + Convert.ToInt32(e.CommandArgument) + "' And ActiveInd='A'";
            int LId = Convert.ToInt32(objData.FetchValue(strSql));
            strSql = "Update [BehaviourDetails] set ActiveInd='D' where MeasurementId='" + Convert.ToInt32(e.CommandArgument) + "'";
            int Final = objData.Execute(strSql);
            strSql = "Update [BehaviourLPRel] set ActiveInd='D' where MeasurementId='" + Convert.ToInt32(e.CommandArgument) + "'";
            int Final1 = objData.Execute(strSql);
            strSql = " delete from BehaviourCalc where StudentId=" + sess.StudentId + " and DateDiff(day,Date, getdate()) <1 and MeasurmentId=" + Convert.ToInt32(e.CommandArgument);
            objData.Execute(strSql);
            strSql = "Update [BehaviourDetails] set ActiveInd='D' where MeasurementId='" + Convert.ToInt32(e.CommandArgument) + "'";
            Final = objData.Execute(strSql);
            strSql = "Update StdtLessonPlan  Set ActiveInd='D' Where SchoolId=" + sess.SchoolId + " And StudentId= " + sess.StudentId + " And  LessonPlanId=" + LId + " And ActiveInd='A'";
            Final = objData.Execute(strSql);
            if (Final > 0)
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Details deleted Successfully");
            cleardata();
            btnSave.Text = "Save";
            clearDataReset();
        }
        GrdMeasurement.EditIndex = -1;
        filldetails();
        if (chkPartial.Checked == true)
        {
            Panal_Partia.Visible = true;
            chkYesOrNo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
        }
    }
    protected void GrdMeasurement_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GrdMeasurement.EditIndex = -1;
    }
    protected void GrdMeasurement_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GrdMeasurement.EditIndex = -1;
    }

    protected void chkFrequency_CheckedChanged(object sender, EventArgs e)
    {
        chkDuration.Checked = false;
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;
        // chkhr.Checked = false;
        //  chkmin.Checked = false;
        //  chksec.Checked = false;
        //  chkhr.Enabled = false;
        //  chkmin.Enabled = false;
        // chksec.Enabled = false;
        GrdMeasurement.EditIndex = -1;
    }
    protected void CheckBoxCostLessPlan_CheckedChanged(object sender, EventArgs e)
    {
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
        if (chkPartial.Checked == true)
        {
            Panal_Partia.Visible = true;
            chkYesOrNo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
        }

        if (CheckBoxCostLessPlan.Checked == true)
        {
            //  CheckBoxCostLessPlan.Checked = true;

            //  FillLessonPlane();
            ButtonAddLessionPlan.Visible = true;
            CheckBoxLessonPlan.Visible = true;
        }

        else
        {
            //FillLessonPlaneEmpty();
            ButtonAddLessionPlan.Visible = false;
            CheckBoxLessonPlan.Visible = false;

        }

    }

    protected void ButtonAddLessionPlan_Click(object sender, EventArgs e)
    {
        if (chkPartial.Checked == true)
        {
            Panal_Partia.Visible = true;
            chkYesOrNo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
        }

        //tdMsg.InnerHtml = "";
        string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '15%'); $('#dialog').show(); }); $('#close_x').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), popup, true);
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
        // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), popup, true);
    }
    protected void btn_LPSearch_Click(object sender, EventArgs e)
    {
        if (chkPartial.Checked == true)
        {
            Panal_Partia.Visible = true;
            chkYesOrNo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
        }

        string popup = " $(document).ready(function () { $('#overlay').fadeIn('fast',function () { $('#dialog').css('top', '15%'); $('#dialog').show(); }); $('#close_x').click(function () { $('#dialog').animate({ top: '-300%' }, function () { $('#overlay').fadeOut('slow'); }); }); });";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), popup, true);
        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
        filldroplessonplanview();
    }
    private void filldroplessonplanview()
    {
        lstLP.Items.Clear();
        objData = new clsData();
        string qryForSearch;
        if (txtLP.Text.Length == 0)
        {
            qryForSearch = "select Distinct LessonPlan.LessonPlanId,LessonPlan.LessonPlanName,Goal.GoalName from LessonPlan inner join GoalLPRel on LessonPlan.LessonPlanId=GoalLPRel.LessonPlanId inner join Goal on Goal.GoalId=GoalLPRel.GoalId where GoalLPRel.GoalId=4 and LessonPlan.ActiveInd='A'  ORDER BY LessonPlan.LessonPlanName";
        }
        else
        {
            qryForSearch = "select Distinct LessonPlan.LessonPlanId,LessonPlan.LessonPlanName,Goal.GoalName from LessonPlan inner join GoalLPRel on LessonPlan.LessonPlanId=GoalLPRel.LessonPlanId inner join Goal on Goal.GoalId=GoalLPRel.GoalId where GoalLPRel.GoalId=4 and LessonPlan.ActiveInd='A' and LessonPlan.LessonPlanName like +'%'+'" + txtLP.Text.Trim() + "'+'%' ORDER BY LessonPlan.LessonPlanName";
        }

        DataTable dt = objData.ReturnDataTable(qryForSearch, true);
        lstLP.DataSource = dt;
        //essonPlan.LessonPlanId,LessonPlan.LessonPlanName
        lstLP.DataTextField = "LessonPlanName";
        lstLP.DataValueField = "LessonPlanId";
        lstLP.DataBind();
    }
    protected void lstLP_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkPartial.Checked == true)
        {
            Panal_Partia.Visible = true;
            chkYesOrNo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
        }

        lessonPLId = int.Parse(lstLP.SelectedItem.Value.ToString());
        CheckBoxLessonPlan.Text = lstLP.SelectedItem.Text;
        CheckBoxLessonPlan.Checked = true;
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("BehaviarCalc.aspx");
    }
    protected void chkPartial_CheckedChanged(object sender, EventArgs e)
    {
        chkYesOrNo.Checked = false;
        chkFrequency.Checked = false;
        //tdMsg.InnerHtml = "";
        if (chkPartial.Checked == true)
        {
            lblCalcOpp.Visible = false;
            chkOpportunities.Visible = false;
            chkOpportunities.Checked = false;
            chkDuration.Enabled = true;
            chkFrequency.Enabled = true;
            if (chkFrequency.Checked == true)
            {
                chkYesOrNo.Checked = false;
                lblCalc.Visible = false;
                rdoSumTotal.Visible = false;
            }            
            if (chkYesOrNo.Checked == true)
            {
                chkFrequency.Checked = false;
                lblCalc.Visible = true;
                rdoSumTotal.Visible = true;
            }            
            Panal_Partia.Visible = true;
            //chkYesOrNo.Visible = true;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadTime();", true);
            
        }
        else
        {
            if (chkFrequency.Checked == true)
            {
                chkFrequency.Checked = true;
            }
            if (chkYesOrNo.Checked == true)
            {
                if (chkFrequency.Checked != true && chkDuration.Checked != true)
                {
                    lblCalcOpp.Visible = true;
                    chkOpportunities.Visible = true;
                }
                chkYesOrNo.Checked = true;
            }
            lblCalc.Visible = false;
            rdoSumTotal.Visible = false;
            if (chk24Hr.Checked == true)
            {
                chk24Hr.Checked = false;
            }
            ddlHour.Enabled = true;
            ddlMinute.Enabled = true;
            DropDownListAMPM.Enabled = true;

            ddlEndHr.Enabled = true;
            ddlEndMin.Enabled = true;
            ddlEndFrmt.Enabled = true;

            ddlHour.SelectedIndex = 0;
            ddlEndHr.SelectedIndex = 0;
            ddlEndMin.SelectedIndex = 0;
            ddlMinute.SelectedIndex = 0;

            ddlEndFrmt.SelectedIndex = 0;
            DropDownListAMPM.SelectedIndex = 0;

            ddlRepeat.SelectedIndex = 0;
            txtStartDate.Text = "";
            txtEndsOn.Text = "";
            txtPeriod.Text = "";
            txtInterval.Text = "";

            Panal_Partia.Visible = false;
            //chkYesOrNo.Visible = false;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        clearDataReset();
        tdMsg.InnerHtml = "";
    }



    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
        btnSave.Text = "Save";
        clearDataReset();
    }
    protected void chkYesOrNo_CheckedChanged(object sender, EventArgs e)
    {
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;
        if (chkPartial.Checked == true)
        {
            if (chkYesOrNo.Checked == true)
            {
                chkFrequency.Checked = false;
                lblCalc.Visible = true;
                rdoSumTotal.Visible = true;
            }
            else
            {               
                lblCalc.Visible = false;
                rdoSumTotal.Visible = false;
            }
        }
        else
        {
            if (chkYesOrNo.Checked == true && chkDuration.Checked == false && chkFrequency.Checked == false)
            {
                lblCalcOpp.Visible = true;
                chkOpportunities.Visible = true;
            }
            else if (chkYesOrNo.Checked == false)
            {
                chkOpportunities.Checked = false;
                chkDuration.Enabled = true;
                chkFrequency.Enabled = true;
            }
            lblCalc.Visible = false;
            rdoSumTotal.Visible = false;
        }
    }

    protected void chkFrequency_CheckedChanged1(object sender, EventArgs e)
    {
        lblCalcOpp.Visible = false;
        chkOpportunities.Visible = false;
        if (chkPartial.Checked == true)
        {
            if (chkFrequency.Checked == true)
            {
                chkYesOrNo.Checked = false;
                lblCalc.Visible = false;
                rdoSumTotal.Visible = false; 
            } 
        }
        else
        {
            if (chkYesOrNo.Checked == true && chkDuration.Checked == false && chkFrequency.Checked == false)
            {
                lblCalcOpp.Visible = true;
                chkOpportunities.Visible = true;    
            }
            lblCalc.Visible = false;
            rdoSumTotal.Visible = false;
        }
    }
    protected void FillDetails3(object sender, EventArgs e)
    {
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        BehNamediv.Visible = true;
        Session["Visibility"] = "1";
        ImageButton btn = (ImageButton)(sender);
        int Mid = Convert.ToInt32(btn.CommandArgument);
        Session["Mid"] = Mid;
        objData = new clsData();
        objdataClass = new DataClass();
        object objval = "";
        foreach (ListItem item in EditList.Items)
        {
            item.Enabled = true;
            item.Selected = false;
        }
        String Strqry = "Select Behaviour,Frequency,Duration,YesOrNo from behaviourDetails where MeasurementId= " + Mid + "";
        DataTable Dt = objData.ReturnDataTable(Strqry, false);
        if (Dt.Rows[0]["Frequency"].ToString() == "False")
        {
            foreach (ListItem item in EditList.Items)
            {
                if (item.Text == "Frequency")
                {
                    item.Enabled = false;

                }
            }

        }
        if (Dt.Rows[0]["Duration"].ToString() == "False")
        {
            foreach (ListItem item in EditList.Items)
            {
                if (item.Text == "Duration")
                {
                    item.Enabled = false;

                }

            }

        }
        if (Dt.Rows[0]["YesOrNo"].ToString() == "False")
        {
            foreach (ListItem item in EditList.Items)
            {
                if (item.Text == "YesOrNo")
                {
                    item.Enabled = false;

                }

            }

        }

        behName.InnerHtml = "<b>Behavior Name: </b>" + Dt.Rows[0]["Behaviour"].ToString();


    }
    protected void ShowGrid(object sender, EventArgs e)
    {
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        if (EditList.SelectedItem.Text == "Frequency")
        {
            Session["gvSelect"] = 1;

        }
        else if (EditList.SelectedItem.Text == "Duration")
        {
            Session["gvSelect"] = 2;
        }
        else if (EditList.SelectedItem.Text == "YesOrNo")
        {
            Session["gvSelect"] = 3;
        }
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        int Mid = Convert.ToInt32(Session["Mid"]);
        bindData(Mid, gvSel);

    }

    protected void bindData(int Mid, int gvSelect)
    {
        objData = new clsData();
        objdataClass = new DataClass();
        object objval = "";
        if (gvSelect == 1)
        {
            string strQuery = "SELECT  MeasurementId, BehaviourId, FrequencyCount,CreatedOn As EventTime from Behaviour where MeasurementId= " + Mid + "AND (Duration IS  NULL) AND (YesOrNo IS NULL) order by EventTime DESC ";

            DataTable Dt = objdataClass.fillData(strQuery);


            if (Dt.Rows.Count > 0)
            {
                GridView1.Visible = true;
                GridView1.DataSource = Dt;
                GridView1.DataBind();
            }
            else
                GridView1.Visible = false;
        }
        if (gvSelect == 2)
        {
            string strQuery = "SELECT  MeasurementId, BehaviourId, Duration As Duration,Duration As Secs,Duration As min,Duration As Hr,CreatedOn As EventTime from Behaviour where MeasurementId= " + Mid + "AND (FrequencyCount IS  NULL) AND (YesOrNo IS NULL) order by EventTime DESC ";

            DataTable Dt = objdataClass.fillData(strQuery);

            foreach (DataRow Dr in Dt.Rows)
            {
                if (!(Dr["Duration"] is DBNull))
                {
                    double totalSeconds = Convert.ToInt32(Dr["Secs"]);
                    double hours = Math.Floor(totalSeconds / 3600);
                    totalSeconds %= 3600;
                    double minutes = Math.Floor(totalSeconds / 60);
                    double seconds = totalSeconds % 60;
                    Dr["Secs"] = seconds;
                    Dr["min"] = minutes;
                    Dr["Hr"] = hours;


                }
            }
            if (Dt.Rows.Count > 0)
            {
                GridView2.Visible = true;
                GridView2.DataSource = Dt;
                GridView2.DataBind();
            }
            else
                GridView2.Visible = false;
        }
        if (gvSelect == 3)
        {
            string strQuery = "SELECT  MeasurementId, BehaviourId, YesOrNo,CreatedOn As EventTime from Behaviour where MeasurementId= " + Mid + "AND (Duration IS  NULL) AND (FrequencyCount=1 OR FrequencyCount=0) AND (YesOrNo IS NOT NULL) order by EventTime DESC ";

            DataTable Dt = objdataClass.fillData(strQuery);


            if (Dt.Rows.Count > 0)
            {
                GridView3.Visible = true;
                GridView3.DataSource = Dt;
                GridView3.DataBind();
            }
            else
                GridView3.Visible = false;

        }
    }


    protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        int Mid = Convert.ToInt32(Session["Mid"]);
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
    {
        objData = new clsData();
        Label id = GridView1.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        TextBox Frequency = GridView1.Rows[e.RowIndex].FindControl("txt_frq") as TextBox;

        String strSql = "Update Behaviour set FrequencyCount =" + Convert.ToInt32(Frequency.Text) + " where BehaviourId=" + Convert.ToInt32(id.Text) + "";
        int Final = objData.Execute(strSql);
        if (Final > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Details Updated Successfully");
        }
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView1.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
        


    }
    protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView1.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        objData = new clsData();
        Label id = GridView1.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        objData.Execute( "Delete from behaviour where BehaviourId=" + Convert.ToInt32(id.Text) + "");
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView1.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);


    }
    protected void OnPageIndexChanging_1(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        int Mid = Convert.ToInt32(Session["Mid"]);
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        this.bindData(Mid, gvSel);
    }
    protected void OnPageIndexChanging_2(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        int Mid = Convert.ToInt32(Session["Mid"]);
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        this.bindData(Mid, gvSel);
    }
    protected void OnPageIndexChanging_3(object sender, GridViewPageEventArgs e)
    {
        GridView3.PageIndex = e.NewPageIndex;
        int Mid = Convert.ToInt32(Session["Mid"]);
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        this.bindData(Mid, gvSel);
    }
    protected void GridView2_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        GridView2.EditIndex = e.NewEditIndex;
        int Mid = Convert.ToInt32(Session["Mid"]);
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView2_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView2.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView2_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
    {
        int sec = 0, min = 0, hr = 0;
        objData = new clsData();
        Label id = GridView2.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        TextBox Secs = GridView2.Rows[e.RowIndex].FindControl("txt_dur_Sec") as TextBox;
        TextBox Mins = GridView2.Rows[e.RowIndex].FindControl("txt_dur_Min") as TextBox;
        TextBox Hrs = GridView2.Rows[e.RowIndex].FindControl("txt_dur_Hr") as TextBox;
        if (Regex.Replace(Secs.Text, "[^0-9.]", "") != "")
            sec = Convert.ToInt32(Regex.Replace(Secs.Text, "[^0-9.]", ""));
        if (Regex.Replace(Mins.Text, "[^0-9.]", "") != "")
            min = Convert.ToInt32(Regex.Replace(Mins.Text, "[^0-9.]", ""));
        if (Regex.Replace(Hrs.Text, "[^0-9.]", "") != "")
            hr = Convert.ToInt32(Regex.Replace(Hrs.Text, "[^0-9.]", ""));
        int Totaldur = ((hr * 3600) + (min * 60) + sec);
        String strSql = "Update Behaviour set  Duration= " + Totaldur + " where BehaviourId=" + Convert.ToInt32(id.Text) + "";
        int Final = objData.Execute(strSql);
        if (Final > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Details Updated Successfully");
        }
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView2.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        objData = new clsData();
        Label id = GridView2.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        objData.Execute("Delete from behaviour where BehaviourId=" + Convert.ToInt32(id.Text) + "");
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView2.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);


    }
    protected void GridView3_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        GridView3.EditIndex = e.NewEditIndex;
        int Mid = Convert.ToInt32(Session["Mid"]);
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView3_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
    {
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView3.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView3_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
    {
        int yOrn = 0;
        objData = new clsData();
        Label id = GridView3.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        CheckBox yesOrNo = GridView3.Rows[e.RowIndex].FindControl("yesOrNo") as CheckBox;
        bool s = yesOrNo.Checked;
        if (s == true)
        {
            yOrn = 1;
        }
        String strSql = "Update Behaviour set YesOrNo= " + yOrn + " where BehaviourId=" + Convert.ToInt32(id.Text) + "";
        int Final = objData.Execute(strSql);
        if (Final > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Details Updated Successfully");
        }
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView3.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);
    }
    protected void GridView3_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        objData = new clsData();
        Label id = GridView3.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
        objData.Execute("Delete from behaviour where BehaviourId=" + Convert.ToInt32(id.Text) + "");
        int Mid = Convert.ToInt32(Session["Mid"]);
        GridView3.EditIndex = -1;
        int gvSel = Convert.ToInt32(Session["gvSelect"]);
        bindData(Mid, gvSel);


    }
    protected void EnableAllAlerts(int MeasurementId)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        curesesid = this.Session.SessionID.ToString();
        HostName = Dns.GetHostName();
        userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "BehaviourMaintanance:EnableAllAlerts";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);

        string StrQuery = "SELECT BehaviourCalcId FROM BehaviourCalc WHERE StudentId=" + sess.StudentId + " AND CONVERT(DATE,[Date])=CONVERT(DATE,GETDATE()) AND ActiveInd='A' AND MeasurmentId=" + MeasurementId;
        DataTable dtbehavior = objData.ReturnDataTable(StrQuery, false);
        SqlTransaction Transs = null;
        if (dtbehavior != null)
        {
            foreach (DataRow dr in dtbehavior.Rows)
            {
                var BehaviourCalcId = dr["BehaviourCalcId"].ToString();
                string Query = "INSERT INTO BehaviorReminder(UserId,StudentId,BehaviourCalcId,DismissStatus) values(" + sess.LoginId + "," + sess.StudentId + "," + BehaviourCalcId + ",'true')";
                objData.Execute(Query);
            }
        }
    }
}