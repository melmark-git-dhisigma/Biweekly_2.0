using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Web.Script.Serialization;
using System.Net;

public partial class StudentBinder_DatasheetTimerIpad : System.Web.UI.Page
{
    public static clsData objData = null;
    public clsData objDt = null;
    public clsSession sess = null;
    clsSession Prevsess = null;
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

    public static DataTable behaviorDT = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Prevsess = (clsSession)Session["PreSession"];
        sess = (clsSession)Session["UserSession"];

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
        Pagepath = "DatasheetTimerIpd:Page_Load";
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
            Prevsess = (clsSession)Session["PreSession"];
            hdnFldUlId.Value = "";
            loadDataTabs();
        }
    }
    public void loadDataTabs()
    {
        sess = (clsSession)Session["UserSession"];

        curesesid = this.Session.SessionID.ToString();
        HostName = Dns.GetHostName();
        userAgent = HttpContext.Current.Request.UserAgent.ToString();
        ip = clsGeneral.GetIPAddress();
        ClsSessionErrorlog sesserrlog = new ClsSessionErrorlog();
        Pagepath = "DatasheetTimerIpd:loadDataTabs";
        if (Prevsess != null)
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + Prevsess.LoginId.ToString() + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + Prevsess.UserName + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + Prevsess.SessionID + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + Prevsess.Classid + ',' + PreClassid + ',' + sess.StudentId + ',' + Prevsess.StudentId + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        else
        {
            sesserrlog.WriteToLog(today.ToString("yyyy-MM-dd") + ',' + now.ToString("HH:mm:ss") + ',' + sess.LoginTime.ToString() + ',' + sess.LoginId.ToString() + ',' + "PrevSession Null" + ',' + preid.ToString() + ',' + ip + ',' + sess.UserName + ',' + "PrevSession Null" + ',' + preuser + ',' + sess.SchoolId + ',' + "Log" + ',' + sess.SessionID + ',' + "PrevSession Null" + ',' + curesesid + ',' + Pagepath + ',' + sess.Classid + ',' + "PrevSession Null" + ',' + PreClassid + ',' + sess.StudentId + ',' + "PrevSession Null" + ',' + SessStudentid + ',' + HostName + ',' + userAgent);
        }
        sess = clsGeneralSchk.sessioncheck(curesesid, preid, ip, preuser, sess, Prevsess, SessStudentid, PreClassid, Sessstname, Pagepath);
        
        int sid = sess.StudentId;
        if (Request.QueryString["stid"] != null)
        {
            if (Request.QueryString["stid"].Contains("stud"))
            {
                if (Request.QueryString["stid"].Substring(0, 4).ToString() == "stud")
                {
                    sid = Convert.ToInt32(Request.QueryString["stid"].Substring(5));
                }
            }
            else
            {
                sid = Convert.ToInt32(Request.QueryString["stid"]);
            }
        }
        objDt = new clsData();
        DataTable dt = new DataTable();
        DataTable dtbl = new DataTable();
        string sqlQuery = "SELECT BDS.MeasurementId,BDS.Behaviour,BDS.BehavDefinition,BDS.BehavStrategy,BDS.IsAcceleration,BDS.StartTime,BDS.EndTime,BDS.Interval,BDS.Period, " +
            //" CASE WHEN BDS.Frequency='True' AND BDS.Duration='True' THEN 'DuraFreq' ELSE CASE WHEN BDS.Duration='True'" +
            //" THEN 'Duration' ELSE 'Frequency' END END AS BehavorMeasure," +

        " CASE WHEN BDS.YesOrNo='True' AND BDS.Duration='True' AND BDS.Frequency='True' THEN 'DuraFreqYesNo' " +
        " ELSE CASE WHEN BDS.Duration='True' AND BDS.Frequency='True' THEN 'DuraFreq' " +
        " ELSE CASE WHEN BDS.YesOrNo='True' AND BDS.Duration='True' THEN 'DuraYesNo' " +
        " ELSE CASE WHEN BDS.YesOrNo='True' AND BDS.Frequency='True' THEN 'FreqYesNo' " +
        " ELSE CASE WHEN BDS.YesOrNo='True' THEN 'YesNo' " +
        " ELSE CASE WHEN BDS.Duration='True' THEN 'Duration' " +
        " ELSE 'Frequency' END END END END END END AS BehavorMeasure," +

        " CASE WHEN BDS.Frequency='True' AND BDS.Duration='True' THEN ISNULL(COUNT(BR.Duration) + SUM(BR.FrequencyCount),0) ELSE ISNULL(SUM(BR.FrequencyCount),0) END AS Frequncy" +
        " FROM Behaviour BR right JOIN BehaviourDetails BDS ON BR.MeasurementId=BDS.MeasurementId where BDS.StudentId=" + sid + " and" +
        "(BDS.Frequency='True' or BDS.Duration='True' or BDS.YesOrNo='True') and BDS.ActiveInd='A' Group By BDS.MeasurementId,BDS.Behaviour,BDS.Frequency,BDS.Duration,BDS.IsAcceleration,BDS.BehavDefinition,BDS.BehavStrategy,BDS.YesOrNo,BDS.StartTime,BDS.EndTime,BDS.Interval,BDS.Period ORDER BY BDS.Behaviour";
        dt = objDt.ReturnDataTable(sqlQuery, false);

        string sqlqry = "select MeasurementId,CONVERT(char(5), TimeOfEvent, 108) as TimeOfEvent ,StudentId from behaviour where CAST(timeofevent AS DATE)=CAST(GETDATE() AS DATE) and studentId=" + sid;//sess.StudentId;
        dtbl = objDt.ReturnDataTable(sqlqry, false);

        hdnFldStudentId.Value = sid.ToString();//sess.StudentId.ToString();

        hdnFldUl.Value = "";
        hdnFldUlId.Value = "";
        hdnFldFrequencyCnt.Value = "";
        hdnFldBehaveType.Value = "";
        hdnFldIsAcceleration.Value = "";
        hdnFldStudentId.Value = sid.ToString();//sess.StudentId.ToString();
        hdnFldClassId.Value = sess.Classid.ToString();
        hdnFldStartTime.Value = "";
        hdnFldEndTime.Value = "";
        hdnFldInterval.Value = "";
        hdnFldPeriod.Value = "";
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    hdnFldUlId.Value = hdnFldUlId.Value + dr["MeasurementId"].ToString() + "@#$";
                    hdnFldUl.Value = hdnFldUl.Value + dr["Behaviour"].ToString() + "@#$";
                    hdnFldFrequencyCnt.Value = hdnFldFrequencyCnt.Value + dr["Frequncy"].ToString() + "@#$";
                    hdnFldBehaveType.Value = hdnFldBehaveType.Value + dr["BehavorMeasure"].ToString() + "@#$";
                    hdnFldIsAcceleration.Value = hdnFldIsAcceleration.Value + dr["IsAcceleration"].ToString() + "@#$";
                    hdnFldBehavDefinition.Value = hdnFldBehavDefinition.Value + dr["BehavDefinition"].ToString() + "@#$";
                    hdnFldBehavStrategy.Value = hdnFldBehavStrategy.Value + dr["BehavStrategy"].ToString() + "@#$";
                    hdnFldStartTime.Value = hdnFldStartTime.Value + dr["StartTime"].ToString() + "@#$";
                    hdnFldEndTime.Value = hdnFldEndTime.Value + dr["EndTime"].ToString() + "@#$";
                    hdnFldInterval.Value = hdnFldInterval.Value + dr["Interval"].ToString() + "@#$";
                    hdnFldPeriod.Value = hdnFldPeriod.Value + dr["Period"].ToString() + "@#$";

                    DataRow[] dt_list = dtbl.Select("MeasurementId = " + dr["MeasurementId"].ToString());

                    if (dt_list.Count() > 0)
                    {
                        foreach (DataRow dr2 in dt_list)
                        {
                            hdnFldSavedTimes.Value = hdnFldSavedTimes.Value + dr2["MeasurementId"].ToString() + "_" + dr2["TimeOfEvent"].ToString().Replace(":", "") + "#";
                        }
                    }

                    hdnFldSavedTimes.Value = hdnFldSavedTimes.Value + "@#$";

                }
            }
        }



    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getResultDuration(string BehaviourId, string Durationid, string DurationTime)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];                
        int DurationId = Convert.ToInt32(Durationid);
        int durTime = Convert.ToInt32(DurationTime);
        objData = new clsData();


        string updatequery = "Update Behaviour SET EndTime=CONVERT (time, GETDATE()),ModifiedOn=getdate(), Duration='" + DurationTime + "' where BehaviourId='" + BehaviourId + "'";
        int updateresult = objData.Execute(updatequery);
        DurationId = 0;

        return DurationId.ToString();

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string saveDurationStartTime(string MeasurementId, string StudentId, bool chkIOA, string ClassId)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
                
        objData = new clsData();
        int LoginId = 1;

        string InsertQuery = "";
        if (chkIOA == false)
        {
            InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,StartTime,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,ClassId) Values('" + MeasurementId + "','" + StudentId + "',CONVERT (time, GETDATE()),'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + ClassId + "')";
        }
        else
        {
            InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,StartTime,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,ClassId) Values('" + MeasurementId + "','" + StudentId + "',CONVERT (time, GETDATE()),'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + ClassId + "')";
        }

        int insertresult = objData.ExecuteWithScope(InsertQuery);

        if (chkIOA == true)
        {
            object createdon = objData.FetchValue("SELECT TOP 1 CreatedOn FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND Duration IS NOT NULL ORDER BY BehaviorIOAId DESC");
            //string Status = "Duration";
            //objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToDateTime(createdon), Status);
            /// to check whether the normal score submitted within 5 min
            /// 
            DateTime datetime1 = Convert.ToDateTime(createdon);
            DateTime datetime2 = datetime1.AddMinutes(-5);
            string dt1 = datetime1.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string dt2 = datetime2.ToString("yyyy-MM-dd HH:mm:ss.fff");
            object frqCunt = objData.FetchValue("SELECT TOP 1 CONVERT(FLOAT,Duration) FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND Duration IS NOT NULL ORDER BY CreatedOn DESC");
            int frq = 0, yesNo = 0;
            behaviorDT = CheckBehaviorDtls(MeasurementId, StudentId);
            if (behaviorDT != null)
            {
                frq = Convert.ToInt32(behaviorDT.Rows[0]["Frequency"]);
                yesNo = Convert.ToInt32(behaviorDT.Rows[0]["YesOrNo"]);
            }
            if (frqCunt == null && frq == 0 && yesNo == 0)
            {
                return "NoNormalData";
            }
        }

        return insertresult.ToString();

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string saveFrequency(string MeasurementId, string StudentId, int Count, string toe, string isBehUpdate, bool chkIOA, string ClassId)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        objData = new clsData();
        //if (Count > 0)
        //{
        int insertresult = 0;
        string sqlQry = "";
        int bCount = 0, updateStat = 0;
        string condition = "";
        int BehaviorIOAId = 0, NormalBehavId = 0;
        if (chkIOA == false)
        {
            if (isBehUpdate == "False")
            {
                string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + Count + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                insertresult = objData.ExecuteWithScope(InsertQuery);
            }
            else
            {
                object behavId = objData.FetchValue("Select BehaviourId from Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND YesOrNo is NULL AND Duration is NULL");
                if (behavId != null)
                {
                    if (behavId.ToString() != null && behavId.ToString() != "")
                    {
                        string UpdateQuery = "Update Behaviour SET FrequencyCount=" + Count + " WHERE BehaviourId=" + behavId.ToString() + "";
                        int updateresult = objData.Execute(UpdateQuery);
                        updateStat = Convert.ToInt32(objData.FetchValue("SELECT COUNT (BehaviorIOAId) FROM BehaviorIOADetails WHERE NormalBehaviorId=" + behavId));
                        if (updateStat == 1)
                        {
                            string Status = "Frequency";
                            BehaviorIOAId = Convert.ToInt32(objData.FetchValue("SELECT BehaviorIOAId FROM BehaviorIOADetails WHERE NormalBehaviorId=" + behavId));
                            objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, BehaviorIOAId, Convert.ToInt32(behavId), Status);
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + Count + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);
                    }
                }
            }
            if (insertresult > 0)
            {
                condition = "FrequencyCount IS NOT NULL AND YesOrNo IS NULL";
                string Status = "Frequency";
                normalUserForIOACalculation(insertresult, StudentId, MeasurementId, Status, condition);
            }

            sqlQry = "SELECT CASE WHEN BDS.Frequency='True' AND BDS.Duration='True' THEN COUNT(BR.Duration) + SUM(BR.FrequencyCount) ELSE SUM(BR.FrequencyCount) END AS " +
            "Frequncy FROM Behaviour BR INNER JOIN BehaviourDetails BDS ON BR.MeasurementId=BDS.MeasurementId where BDS.StudentId=" + StudentId + " and" +
            "(BDS.Frequency='True' or BDS.Duration='False')	and BDS.MeasurementId=" + MeasurementId + " Group By BDS.MeasurementId,BDS.Behaviour,BDS.Frequency,BDS.Duration ";
        }
        else
        {
            if (isBehUpdate == "False")
            {
                string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + Count + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                insertresult = objData.ExecuteWithScope(InsertQuery);
            }
            else
            {
                object behavId = objData.FetchValue("Select BehaviorIOAId from BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND YesOrNo is NULL AND Duration is NULL");
                if (behavId != null)
                {
                    if (behavId.ToString() != null && behavId.ToString() != "")
                    {
                        string UpdateQuery = "Update BehaviorIOADetails SET FrequencyCount=" + Count + " WHERE BehaviorIOAId=" + behavId.ToString() + "";
                        int updateresult = objData.Execute(UpdateQuery);
                        NormalBehavId = Convert.ToInt32(objData.FetchValue("SELECT ISNULL(CONVERT(INT,NormalBehaviorId),-1) FROM BehaviorIOADetails WHERE BehaviorIOAId=" + behavId));
                        if (NormalBehavId != -1)
                        {
                            string Status = "Frequency";
                            objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToInt32(behavId), NormalBehavId, Status);
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + Count + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);
                    }
                    if (insertresult > 0)
                    {
                        condition = "FrequencyCount IS NOT NULL AND YesOrNo IS NULL";
                        string Status = "Frequency";
                        bCount = IOAUserForIOACalculation(insertresult, StudentId, MeasurementId, Status, condition);
                        int dur = 0, yesNo = 0;
                        behaviorDT = CheckBehaviorDtls(MeasurementId, StudentId);
                        if (behaviorDT != null)
                        {
                            dur = Convert.ToInt32(behaviorDT.Rows[0]["Duration"]);
                            yesNo = Convert.ToInt32(behaviorDT.Rows[0]["YesOrNo"]);
                        }
                        if (bCount == 0 && dur == 0 && yesNo == 0)
                        {
                            return "NoNormalData";
                        }
                    }
                }
            }
            sqlQry = "SELECT CASE WHEN BDS.Frequency='True' AND BDS.Duration='True' THEN COUNT(BR.Duration) + SUM(BR.FrequencyCount) ELSE SUM(BR.FrequencyCount) END AS " +
            "Frequncy FROM BehaviorIOADetails BR INNER JOIN BehaviourDetails BDS ON BR.MeasurementId=BDS.MeasurementId where BDS.StudentId=" + StudentId + " and" +
            "(BDS.Frequency='True' or BDS.Duration='False')	and BDS.MeasurementId=" + MeasurementId + " Group By BDS.MeasurementId,BDS.Behaviour,BDS.Frequency,BDS.Duration ";
        }
        object frqCount = objData.FetchValue(sqlQry);

        if (frqCount != null)
        {
            return frqCount.ToString();
        }
        else
        {
            return "0";
        }
    }
    [WebMethod]
    public static string saveYesOrNo(string MeasurementId, string StudentId, string yesOrNo, string FrequencyCount, string toe, string YNautoSave, string isBehUpdate, bool chkIOA, string ClassId)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        objData = new clsData();
        //if (FrequencyCount == "0")
        //{
        //    FrequencyCount = "1";
        //}
        if (FrequencyCount == "")
        {
            FrequencyCount = "0";
        }
        int insertresult = 0;
        int bCount = 0, updateStat = 0;
        string condition = "";
        int BehaviorIOAId = 0, NormalBehavId = 0;

        if (YNautoSave == "False")
        {
            if (chkIOA == false)
            {
                if (isBehUpdate == "False")
                {
                    string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,YesOrNo,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + FrequencyCount + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + yesOrNo + "','" + toe + "','" + ClassId + "')";
                    insertresult = objData.ExecuteWithScope(InsertQuery);
                }
                else
                {
                    object behavId = objData.FetchValue("Select BehaviourId from Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND YesOrNo is not NULL AND Duration is NULL");
                    if (behavId != null)
                    {
                        if (behavId.ToString() != null && behavId.ToString() != "")
                        {
                            string UpdateQuery = "Update Behaviour SET FrequencyCount=" + FrequencyCount + ",YesOrNo='" + yesOrNo + "' WHERE BehaviourId=" + behavId.ToString() + "";
                            int updateresult = objData.Execute(UpdateQuery);
                            updateStat = Convert.ToInt32(objData.FetchValue("SELECT COUNT (BehaviorIOAId) FROM BehaviorIOADetails WHERE NormalBehaviorId=" + behavId));
                            if (updateStat == 1)
                            {
                                string Status = "YesOrNo";
                                BehaviorIOAId = Convert.ToInt32(objData.FetchValue("SELECT BehaviorIOAId FROM BehaviorIOADetails WHERE NormalBehaviorId=" + behavId));
                                objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, BehaviorIOAId, Convert.ToInt32(behavId), Status);
                            }
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,YesOrNo,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + FrequencyCount + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + yesOrNo + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);
                    }
                }
                if (insertresult > 0)
                {
                    condition = "FrequencyCount IS NOT NULL AND YesOrNo IS NOT NULL";
                    string Status = "YesOrNo";
                    normalUserForIOACalculation(insertresult, StudentId, MeasurementId, Status, condition);
                }
            }
            else
            {
                if (isBehUpdate == "False")
                {
                    string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,YesOrNo,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + FrequencyCount + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + yesOrNo + "','" + toe + "','" + ClassId + "')";
                    insertresult = objData.ExecuteWithScope(InsertQuery);
                }
                else
                {
                    object behavId = objData.FetchValue("Select BehaviorIOAId from BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND YesOrNo is not NULL AND Duration is NULL");
                    if (behavId != null)
                    {
                        if (behavId.ToString() != null && behavId.ToString() != "")
                        {
                            string UpdateQuery = "Update BehaviorIOADetails SET FrequencyCount=" + FrequencyCount + ",YesOrNo='" + yesOrNo + "' WHERE BehaviorIOAId=" + behavId.ToString() + "";
                            int updateresult = objData.Execute(UpdateQuery);
                            NormalBehavId = Convert.ToInt32(objData.FetchValue("SELECT ISNULL(CONVERT(INT,NormalBehaviorId),-1) FROM BehaviorIOADetails WHERE BehaviorIOAId=" + behavId));
                            if (NormalBehavId != -1)
                            {
                                string Status = "YesOrNo";
                                objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToInt32(behavId), NormalBehavId, Status);
                            }
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,YesOrNo,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + FrequencyCount + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + yesOrNo + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);
                    }
                }
                if (insertresult > 0)
                {
                    condition = "FrequencyCount IS NOT NULL AND YesOrNo IS NOT NULL";
                    string Status = "YesOrNo";
                    bCount = IOAUserForIOACalculation(insertresult, StudentId, MeasurementId, Status, condition);

                    if (bCount == 0)
                    {
                        return "NoNormalData";
                    }
                }
            }
        }

        return insertresult.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string saveDurationOther(string MeasurementId, string StudentId, string duration, string FrqSave, string toe, string isBehUpdate, bool chkIOA, string ClassId)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        objData = new clsData();

        int insertresult = 0;

        if (duration != "" && duration != string.Empty && duration != null && FrqSave == "False")
        {
            int frq = 0, yesNo = 0;
            behaviorDT = CheckBehaviorDtls(MeasurementId, StudentId);
            if (behaviorDT != null)
            {
                frq = Convert.ToInt32(behaviorDT.Rows[0]["Frequency"]);
                yesNo = Convert.ToInt32(behaviorDT.Rows[0]["YesOrNo"]);
            }

            if (chkIOA == false)
            {
                if (isBehUpdate == "False")
                {
                    string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,Duration,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + duration + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                    insertresult = objData.ExecuteWithScope(InsertQuery);
                }
                else
                {
                    object behavId = objData.FetchValue("Select BehaviourId from Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND Duration is not NULL");
                    if (behavId != null)
                    {
                        if (behavId.ToString() != null && behavId.ToString() != "")
                        {
                            string UpdateQuery = "Update Behaviour SET Duration=" + duration + " WHERE BehaviourId=" + behavId.ToString() + "";
                            insertresult = objData.Execute(UpdateQuery);
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,Duration,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + duration + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);
                    }
                }
            }
            else
            {
                if (isBehUpdate == "False")
                {
                    string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,Duration,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + duration + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                    insertresult = objData.ExecuteWithScope(InsertQuery);

                    object createdon = objData.FetchValue("SELECT TOP 1 CreatedOn FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND Duration IS NOT NULL ORDER BY BehaviorIOAId DESC");
                    //string Status = "Duration";
                    //objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToDateTime(createdon), Status);
                    /// to check whether the normal score submitted within 5 min
                    /// 
                    DateTime datetime1 = Convert.ToDateTime(createdon);
                    DateTime datetime2 = datetime1.AddMinutes(-5);
                    string dt1 = datetime1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string dt2 = datetime2.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    int interval = Convert.ToInt32(objData.FetchValue("SELECT PartialInterval FROM BehaviourDetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "'"));

                    if (interval == 1)
                    {
                        object timeEV = objData.FetchValue("SELECT TimeOfEvent FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn='" + dt1 + "' AND Duration IS NOT NULL");
                        DateTime dtevnt = Convert.ToDateTime(timeEV);
                        string evntTime = dtevnt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        object frqCunt = objData.FetchValue("SELECT TOP 1 Duration FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND TimeOfEvent='" + toe + "' AND Duration IS NOT NULL ORDER BY CreatedOn DESC");
                        if (frqCunt == null && frq == 0 && yesNo == 0)
                        {
                            return "NoNormalData";
                        }
                    }
                    else
                    {
                        object frqCunt = objData.FetchValue("SELECT TOP 1 Duration FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND Duration IS NOT NULL ORDER BY CreatedOn DESC");
                        if (frqCunt == null && frq == 0 && yesNo == 0)
                        {
                            return "NoNormalData";
                        }
                    }
                }
                else
                {
                    object behavId = objData.FetchValue("Select BehaviorIOAId from BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND Duration is not NULL");
                    if (behavId != null)
                    {
                        if (behavId.ToString() != null && behavId.ToString() != "")
                        {
                            string UpdateQuery = "Update BehaviorIOADetails SET Duration=" + duration + " WHERE BehaviorIOAId=" + behavId.ToString() + "";
                            insertresult = objData.Execute(UpdateQuery);

                            //object createdon = objData.FetchValue("SELECT CreatedOn FROM BehaviorIOADetails WHERE BehaviorIOAId=" + behavId.ToString());
                            //string Status = "Duration";
                            //objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToDateTime(createdon), Status);
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,Duration,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + duration + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);

                        object createdon = objData.FetchValue("SELECT TOP 1 CreatedOn FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND Duration IS NOT NULL ORDER BY BehaviorIOAId DESC");
                        //string Status = "Duration";
                        //objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToDateTime(createdon), Status);
                        /// to check whether the normal score submitted within 5 min
                        /// 
                        DateTime datetime1 = Convert.ToDateTime(createdon);
                        DateTime datetime2 = datetime1.AddMinutes(-5);
                        string dt1 = datetime1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string dt2 = datetime2.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        int interval = Convert.ToInt32(objData.FetchValue("SELECT PartialInterval FROM BehaviourDetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "'"));

                        if (interval == 1)
                        {
                            object timeEV = objData.FetchValue("SELECT TimeOfEvent FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn='" + dt1 + "' AND Duration IS NOT NULL");
                            DateTime dtevnt = Convert.ToDateTime(timeEV);
                            string evntTime = dtevnt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            object frqCunt = objData.FetchValue("SELECT TOP 1 Duration FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND TimeOfEvent='" + toe + "' AND Duration IS NOT NULL ORDER BY CreatedOn DESC");
                            if (frqCunt == null && frq == 0 && yesNo == 0)
                            {
                                return "NoNormalData";
                            }
                        }
                        else
                        {
                            object frqCunt = objData.FetchValue("SELECT TOP 1 Duration FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND Duration IS NOT NULL ORDER BY CreatedOn DESC");
                            if (frqCunt == null && frq == 0 && yesNo == 0)
                            {
                                return "NoNormalData";
                            }
                        }
                    }
                }
            }
        }

        return insertresult.ToString();

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string saveFrequencyOther(string MeasurementId, string StudentId, string frequency, string FrqSave, string toe, string isBehUpdate, bool chkIOA, string ClassId)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        objData = new clsData();
        int insertresult = 0;
        string sqlQry = "";
        int bCount = 0, updateStat = 0;
        string condition = "";
        int BehaviorIOAId = 0, NormalBehavId = 0;

        int yesNo = 0;
        behaviorDT = CheckBehaviorDtls(MeasurementId, StudentId);
        if (behaviorDT != null)
        {
            yesNo = Convert.ToInt32(behaviorDT.Rows[0]["YesOrNo"]);
        }

        if (chkIOA == false)
        {
            if (frequency != string.Empty && frequency != "" && frequency != null && FrqSave == "False")  //frequency != "0"&&
            {
                if (isBehUpdate == "False")
                {
                    string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + frequency + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                    insertresult = objData.ExecuteWithScope(InsertQuery);
                }
                else
                {
                    object behavId = objData.FetchValue("Select BehaviourId from Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND YesOrNo is NULL AND Duration is NULL");
                    if (behavId != null)
                    {
                        if (behavId.ToString() != null && behavId.ToString() != "")
                        {
                            string UpdateQuery = "Update Behaviour SET FrequencyCount=" + frequency + " WHERE BehaviourId=" + behavId.ToString() + "";
                            int updateresult = objData.Execute(UpdateQuery);
                            updateStat = Convert.ToInt32(objData.FetchValue("SELECT COUNT (BehaviorIOAId) FROM BehaviorIOADetails WHERE NormalBehaviorId=" + behavId));
                            if (updateStat == 1)
                            {
                                string Status = "Frequency";
                                BehaviorIOAId = Convert.ToInt32(objData.FetchValue("SELECT BehaviorIOAId FROM BehaviorIOADetails WHERE NormalBehaviorId=" + behavId));
                                objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, BehaviorIOAId, Convert.ToInt32(behavId), Status);
                            }
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + frequency + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);
                    }
                }
                if (insertresult > 0)
                {
                    condition = "FrequencyCount IS NOT NULL AND YesOrNo IS NULL";
                    string Status = "Frequency";
                    normalUserForIOACalculation(insertresult, StudentId, MeasurementId, Status, condition);
                }
            }
            sqlQry = "SELECT CASE WHEN BDS.Frequency='True' AND BDS.Duration='True' THEN COUNT(BR.Duration) + SUM(BR.FrequencyCount) ELSE SUM(BR.FrequencyCount) END AS " +
             "Frequncy FROM Behaviour BR INNER JOIN BehaviourDetails BDS ON BR.MeasurementId=BDS.MeasurementId where BDS.StudentId=" + StudentId + " and" +
             "(BDS.Frequency='True' or BDS.Duration='False')	and BDS.MeasurementId=" + MeasurementId + " Group By BDS.MeasurementId,BDS.Behaviour,BDS.Frequency,BDS.Duration ";
        }
        else
        {
            if (frequency != string.Empty && frequency != "" && frequency != null && FrqSave == "False")  //frequency != "0"&&
            {
                if (isBehUpdate == "False")
                {
                    string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + frequency + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                    insertresult = objData.ExecuteWithScope(InsertQuery);
                }
                else
                {
                    object behavId = objData.FetchValue("Select BehaviorIOAId from BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + toe + "' AND YesOrNo is NULL AND Duration is NULL");
                    if (behavId != null)
                    {
                        if (behavId.ToString() != null && behavId.ToString() != "")
                        {
                            string UpdateQuery = "Update BehaviorIOADetails SET FrequencyCount=" + frequency + " WHERE BehaviorIOAId=" + behavId.ToString() + "";
                            int updateresult = objData.Execute(UpdateQuery);
                            //check the bevid and updatesresult are same
                            NormalBehavId = Convert.ToInt32(objData.FetchValue("SELECT ISNULL(CONVERT(INT,NormalBehaviorId),-1) FROM BehaviorIOADetails WHERE BehaviorIOAId=" + behavId));
                            if (NormalBehavId != -1)
                            {
                                string Status = "Frequency";
                                objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToInt32(behavId), NormalBehavId, Status);
                            }

                            //object createdon = objData.FetchValue("SELECT CreatedOn FROM BehaviorIOADetails WHERE BehaviorIOAId=" + behavId.ToString());
                            //string Status = "Frequency";
                            //objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, Convert.ToDateTime(createdon), Status);
                        }
                    }
                    else
                    {
                        string InsertQuery = "Insert into BehaviorIOADetails(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId,TimeOfEvent,ClassId) Values('" + MeasurementId + "','" + StudentId + "'," + frequency + ",'A','" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "',getdate(),'" + oSession.LoginId + "','" + toe + "','" + ClassId + "')";
                        insertresult = objData.ExecuteWithScope(InsertQuery);
                    }

                }
                if (insertresult > 0)
                {
                    condition = "FrequencyCount IS NOT NULL AND YesOrNo IS NULL";
                    string Status = "Frequency";
                    bCount = IOAUserForIOACalculation(insertresult, StudentId, MeasurementId, Status, condition);

                    if (bCount == 0 && yesNo == 0)
                    {
                        return "NoNormalData";
                    }
                }
            }
            sqlQry = "SELECT CASE WHEN BDS.Frequency='True' AND BDS.Duration='True' THEN COUNT(BR.Duration) + SUM(BR.FrequencyCount) ELSE SUM(BR.FrequencyCount) END AS " +
             "Frequncy FROM BehaviorIOADetails BR INNER JOIN BehaviourDetails BDS ON BR.MeasurementId=BDS.MeasurementId where BDS.StudentId=" + StudentId + " and" +
             "(BDS.Frequency='True' or BDS.Duration='False')	and BDS.MeasurementId=" + MeasurementId + " Group By BDS.MeasurementId,BDS.Behaviour,BDS.Frequency,BDS.Duration ";
        }
        object frqCount = objData.FetchValue(sqlQry);
        if (frqCount != null)
        {
            return frqCount.ToString();
        }
        else
        {
            return "0";
        }
    }

    public static string loadBehaviourData(string MeasurementId, string StudentId, string StartTime, string EndTime, bool chkIOA)
    {
        clsSession oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        objData = new clsData();
        List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
        Dictionary<string, object> dict;
        DateTime behStartDateTime = Convert.ToDateTime(StartTime);
        DateTime behEndDateTime = Convert.ToDateTime(EndTime);

        string sqlQry = "";
        if (chkIOA == false)
        {
            sqlQry = "SELECT CASE WHEN YesOrNo IS NOT NULL THEN 'YesNo' WHEN FrequencyCount IS NOT NULL THEN 'Frequency'  WHEN Duration IS NOT NULL THEN 'Duration'  END as Status," +
            "BehaviourId,FrequencyCount,Duration,YesOrNo FROM Behaviour WHERE  MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + behStartDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND ActiveInd='A'";
        }
        else
        {
            sqlQry = "SELECT CASE WHEN YesOrNo IS NOT NULL THEN 'YesNo' WHEN FrequencyCount IS NOT NULL THEN 'Frequency'  WHEN Duration IS NOT NULL THEN 'Duration'  END as Status," +
            "BehaviorIOAId,FrequencyCount,Duration,YesOrNo FROM BehaviorIOADetails WHERE  MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND TimeOfEvent='" + behStartDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND ActiveInd='A'";
        }

        DataTable dtBehav = objData.ReturnDataTable(sqlQry, false);
        //object[] arr = new object[dtBehav.Rows.Count + 1];

        //for (int i = 0; i <= dtBehav.Rows.Count - 1; i++)
        //{
        //    arr[i] = dtBehav.Rows[i].ItemArray;
        //    dict.Add(dtBehav.Rows[i]["Status"].ToString(), arr[i]);
        //}
        foreach (DataRow dr in dtBehav.Rows)
        {
            dict = new Dictionary<string, object>();
            foreach (DataColumn col in dtBehav.Columns)
            {
                dict.Add(col.ColumnName, dr[col]);
            }
            lst.Add(dict);
        }

        JavaScriptSerializer json = new JavaScriptSerializer();
        return json.Serialize(lst);
    }

    public static void normalUserForIOACalculation(int insertresult, string StudentId, string MeasurementId, string Status, string condition)
    {
        int bCount = 0, bStat = 0;
        DataTable dtBehav = null;
        string selQuerry = "";
        int BehaviorIOAId = 0;
        ///Check any IOA is submitted within 5 min
        ///
        object createdon = objData.FetchValue("SELECT CreatedOn FROM Behaviour WHERE BehaviourId=" + insertresult + " AND StudentId='" + StudentId + "'");
        DateTime datetime1 = Convert.ToDateTime(createdon);
        DateTime datetime2 = datetime1.AddMinutes(-5);
        string dt1 = datetime1.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string dt2 = datetime2.ToString("yyyy-MM-dd HH:mm:ss.fff");

        int interval = Convert.ToInt32(objData.FetchValue("SELECT PartialInterval FROM BehaviourDetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "'"));
        if (interval == 1)
        {
            object timeEV = objData.FetchValue("SELECT TimeOfEvent FROM Behaviour WHERE BehaviourId=" + insertresult + " AND StudentId='" + StudentId + "'");
            DateTime dtevnt = Convert.ToDateTime(timeEV);
            string evntTime = dtevnt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bCount = Convert.ToInt32(objData.FetchValue("SELECT COUNT(BehaviorIOAId) FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND TimeOfEvent='" + evntTime + "' AND " + condition + " "));
            if (bCount > 0)
            {
                selQuerry = "SELECT TOP 1 ISNULL(CONVERT(INT,IOAPerc),-1) AS IOAPerc, BehaviorIOAId FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND TimeOfEvent='" + evntTime + "' AND " + condition + " ORDER BY CreatedOn DESC";
                dtBehav = objData.ReturnDataTable(selQuerry, false);
                if (dtBehav != null && dtBehav.Rows.Count > 0)
                {
                    bStat = Convert.ToInt32(dtBehav.Rows[0]["IOAPerc"]);
                    BehaviorIOAId = Convert.ToInt32(dtBehav.Rows[0]["BehaviorIOAId"]);
                }
            }
        }
        else
        {
            bCount = Convert.ToInt32(objData.FetchValue("SELECT COUNT(BehaviorIOAId) FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND " + condition + " "));
            if (bCount > 0)
            {
                selQuerry = "SELECT TOP 1 ISNULL(CONVERT(INT,IOAPerc),-1) AS IOAPerc, BehaviorIOAId FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND " + condition + " ORDER BY CreatedOn DESC";
                dtBehav = objData.ReturnDataTable(selQuerry, false);
                if (dtBehav != null && dtBehav.Rows.Count > 0)
                {
                    bStat = Convert.ToInt32(dtBehav.Rows[0]["IOAPerc"]);
                    BehaviorIOAId = Convert.ToInt32(dtBehav.Rows[0]["BehaviorIOAId"]);
                }
            }
        }
        if (bStat == -1 && BehaviorIOAId > 0)
        {
            objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, BehaviorIOAId, insertresult, Status);
        }
    }

    public static int IOAUserForIOACalculation(int insertresult, string StudentId, string MeasurementId, string Status, string condition)
    {
        int bCount = 0, bStat = 0;
        int NormalBehavId = 0;
        /// to check whether the normal score submitted within 5 min
        /// 
        object createdon = objData.FetchValue("SELECT TOP 1 CreatedOn FROM BehaviorIOADetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND " + condition + " ORDER BY BehaviorIOAId DESC");
        DateTime datetime1 = Convert.ToDateTime(createdon);
        DateTime datetime2 = datetime1.AddMinutes(-5);
        string dt1 = datetime1.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string dt2 = datetime2.ToString("yyyy-MM-dd HH:mm:ss.fff");

        int interval = Convert.ToInt32(objData.FetchValue("SELECT PartialInterval FROM BehaviourDetails WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "'"));
        if (interval == 1)
        {
            object timeEV = objData.FetchValue("SELECT TimeOfEvent FROM BehaviorIOADetails WHERE BehaviorIOAId='" + insertresult + "' AND StudentId='" + StudentId + "'");
            DateTime dtevnt = Convert.ToDateTime(timeEV);
            string evntTime = dtevnt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            bCount = Convert.ToInt32(objData.FetchValue("SELECT count(BehaviourId) FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND TimeOfEvent='" + evntTime + "' AND " + condition + ""));
            if (bCount > 0)
            {
                NormalBehavId = Convert.ToInt32(objData.FetchValue("SELECT TOP 1 BehaviourId FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND TimeOfEvent='" + evntTime + "' AND " + condition + " ORDER BY CreatedOn DESC"));
                ///Check whether the normal user's score is valied for the current IOA % calculation.
                bStat = Convert.ToInt32(objData.FetchValue("SELECT COUNT (BehaviorIOAId) FROM BehaviorIOADetails WHERE NormalBehaviorId=" + NormalBehavId));
            }
        }
        else
        {
            bCount = Convert.ToInt32(objData.FetchValue("SELECT count(BehaviourId) FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND " + condition + ""));
            if (bCount > 0)
            {
                NormalBehavId = Convert.ToInt32(objData.FetchValue("SELECT TOP 1 BehaviourId FROM Behaviour WHERE MeasurementId='" + MeasurementId + "' AND StudentId='" + StudentId + "' AND CreatedOn BETWEEN '" + dt2 + "' AND '" + dt1 + "' AND " + condition + " ORDER BY CreatedOn DESC"));
                ///Check whether the normal user's score is valied for the current IOA % calculation.
                bStat = Convert.ToInt32(objData.FetchValue("SELECT COUNT (BehaviorIOAId) FROM BehaviorIOADetails WHERE NormalBehaviorId=" + NormalBehavId));
            }
        }
        if (bStat == 0 && NormalBehavId > 0)
        {
            objData.ExecuteIOAPercBehaviorCalc(MeasurementId, StudentId, insertresult, NormalBehavId, Status);
        }
        return bCount;

    }

    public static System.Data.DataTable CheckBehaviorDtls(string MeasurementId, string StudentId)
    {
        DataTable dt = new DataTable();
        string strQuery = "SELECT Frequency,Duration,YesOrNo FROM BehaviourDetails WHERE MeasurementId=" + MeasurementId + " AND StudentId=" + StudentId;
        dt = objData.ReturnDataTable(strQuery, false);
        return dt;
    }
}