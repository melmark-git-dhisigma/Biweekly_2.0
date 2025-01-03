using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web;
using System.Drawing;


public class DiscreteTrials
{
    public ArrayList arTrials = new ArrayList();
    public string[] arPrompt = null;
    public int sessionCount = 0;
    public int trialsCount = 0;
    public int totalSet = 0;
    public string value = "";
}

public class DiscreteSession
{
    clsData objData = new clsData();
    clsSession oSession = null;


    public DiscreteTrials GetTrialLists(int studentId, int templateID, int iCrntSet, int iCrntStep, int nmbrOfSession, string colName, bool flag, string correctResponse, string coltypeCode, string chainedtype, SqlTransaction trans, SqlConnection con)
    {


        DiscreteTrials objTrails = new DiscreteTrials();
        string strSql = "";

        string cond = "", cond2 = "";
        if (flag)
            cond = " AND DStp.SortOrder<>" + iCrntStep;
        if (chainedtype == "Backward chain")
            cond2 = ",StdtSessionStepId DESC";
        else
            cond2 = ",ss.StdtSessionStepId";
        //strSql = "SELECT S.StdtSessionStepId,S.StepVal, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +
        //             " FROM StdtSessionDtl S" +
        //             " INNER JOIN StdtSessionStep SS" +
        //              " ON S.StdtSessionStepId = SS.StdtSessionStepId" +
        //             " INNER JOIN StdtSessionHdr SH" +
        //              " on SS.StdtSessionHdrId = sh.StdtSessionHdrId" +
        //             " INNER JOIN DSTempSetCol DCol" +
        //              " ON S.DSTempSetColId = DCol.DSTempSetColId" +
        //             " WHERE SH.StdtSessionHdrId IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK()" +
        //             " OVER (ORDER BY (CASE(IOAInd) WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END),EndTs DESC) as RNK  FROM StdtSessionHdr hdr" +
        //             " WHERE StudentId = " + studentId + " AND DSTempHdrId = " + templateID + " AND CurrentSetId=" + iCrntSet +
        //             ") as Rk WHERE RNK <= " + nmbrOfSession + ") AND SH.IOAInd<>'Y' AND DCol.ColName = '" + colName + "'AND StartTs >" +
        //             " (SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent" +
        //             " WHERE DSTempHdrId =" + templateID + "AND StudentId = " + studentId + " AND StdtSessEventType!='Minor' AND EventName!='ProbeMode' ) ORDER BY SS.StdtSessionHdrId ";

        //string sesnnmbr = HttpContext.Current.Session["sesnnmbr"].ToString();
        //string sessnmbr = "SELECT MAX(SessionNbr) AS SessionNbr FROM StdtSessEvent WHERE DSTempHdrId =" + templateID + "AND StudentId=" + studentId;
        string lessonplanid = "SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + templateID;
        SqlDataReader rdr1 = null;
        rdr1 = objData.ReturnDataReader(lessonplanid, false);
        int lessonplannid = 0;
        while (rdr1.Read())
        {
            lessonplannid = Convert.ToInt32(rdr1["LessonPlanId"].ToString());
        }
        string sessnmbr = "SELECT ISNULL((SELECT MAX(SessionNbr) AS SessionNbr FROM StdtSessEvent WHERE LessonPlanId =" + lessonplannid + "AND StudentId=" + studentId + "),0) AS SessionNbr";
        //string sessnmbr = "SELECT MAX(SessionNbr) AS SessionNbr FROM StdtSessEvent WHERE LessonPlanId =" + lessonplannid + "AND StudentId=" + studentId;

        SqlDataReader rdr = null;
        SqlCommand cmd = con.CreateCommand();
        cmd.Transaction = trans;
        cmd.CommandText = sessnmbr;
        rdr = cmd.ExecuteReader(CommandBehavior.Default);
        //rdr = objData.ReturnDataReader(sessnmbr, false);
        int sesnnmbr = 0;
        while (rdr.Read())
        {
            if (rdr["SessionNbr"].ToString() != "NULL")
            {
                sesnnmbr = Convert.ToInt32(rdr["SessionNbr"].ToString());
            }
            else
            {
                sesnnmbr = 1;
            }
        }
        if (sesnnmbr == 1 )
        {
            //strSql = "SELECT DStp.SortOrder,S.StdtSessionStepId,CASE WHEN S.stepval='' THEN '+' ELSE  S.stepval END AS stepval, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +
            strSql = "SELECT DStp.SortOrder,S.StdtSessionStepId, S.stepval AS stepval, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +
                         " FROM StdtSessionDtl S" +
                         " INNER JOIN StdtSessionStep SS" +
                         " ON S.StdtSessionStepId = SS.StdtSessionStepId" +
                         " INNER JOIN StdtSessionHdr SH" +
                         " on SS.StdtSessionHdrId = sh.StdtSessionHdrId" +
                         " INNER JOIN DSTempSetCol DCol" +
                         " ON S.DSTempSetColId = DCol.DSTempSetColId" +
                         " LEFT JOIN DSTempStep DStp ON SS.DSTempStepId = DStp.DSTempStepId " +
                         " WHERE SH.StdtSessionHdrId IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK()" +
                         " OVER (ORDER BY (CASE(IOAInd) WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END),EndTs DESC) as RNK  FROM StdtSessionHdr hdr" +
                         " WHERE SessMissTrailStus!='Y' AND StudentId = " + studentId + " AND DSTempHdrId = " + templateID + " AND CurrentSetId=" + iCrntSet +
                         ") as Rk WHERE RNK <= " + nmbrOfSession + ") AND SH.IOAInd<>'Y' AND REPLACE(dcol.colname,'''','^') = '" + colName.Replace("'", "^") + "'AND StartTs >= " +
                         " (SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent" +
                         " WHERE SessMissTrailStus!='Y' AND DSTempHdrId =" + templateID + "AND StudentId = " + studentId + " AND (IsMaintanace=0 OR IsMaintanace=NULL) AND StdtSessEventType!='Minor' AND EventName!='ProbeMode' ) " +
                         " AND SH.SessionNbr>=ISNULL((SELECT MAX(SessionNbr) FROM StdtSessEvent " +
                         "WHERE DSTempHdrId=" + templateID + " AND StudentId=" + studentId + "),0)" + cond + " ORDER BY SS.StdtSessionHdrId " + cond2;
        }

        else
        {
            //strSql = "SELECT DStp.SortOrder,S.StdtSessionStepId,CASE WHEN S.stepval='' THEN '+' ELSE  S.stepval END AS stepval, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +
            strSql = "SELECT DStp.SortOrder,S.StdtSessionStepId, S.stepval AS stepval, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +
                                    " FROM StdtSessionDtl S" +
                                    " INNER JOIN StdtSessionStep SS" +
                                    " ON S.StdtSessionStepId = SS.StdtSessionStepId" +
                                    " INNER JOIN StdtSessionHdr SH" +
                                    " on SS.StdtSessionHdrId = sh.StdtSessionHdrId" +
                                    " INNER JOIN DSTempSetCol DCol" +
                                    " ON S.DSTempSetColId = DCol.DSTempSetColId" +
                                    " LEFT JOIN DSTempStep DStp ON SS.DSTempStepId = DStp.DSTempStepId " +
                                    " WHERE SH.StdtSessionHdrId IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK()" +
                                    " OVER (ORDER BY (CASE(IOAInd) WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END),EndTs DESC) as RNK  FROM StdtSessionHdr hdr" +
                                    " WHERE SessMissTrailStus!='Y' AND StudentId = " + studentId + " AND DSTempHdrId = " + templateID + " AND CurrentSetId=" + iCrntSet +
                                    ") as Rk WHERE RNK <= " + nmbrOfSession + ") AND SH.IOAInd<>'Y' AND REPLACE(dcol.colname,'''','^') = '" + colName.Replace("'", "^") + "'AND StartTs >" +
                                    " (SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent" +
                                    " WHERE SessMissTrailStus!='Y' AND DSTempHdrId =" + templateID + "AND StudentId = " + studentId + " AND (IsMaintanace=0 OR IsMaintanace=NULL) AND StdtSessEventType!='Minor' AND EventName!='ProbeMode' ) " +
                                    " AND SH.SessionNbr>ISNULL((SELECT MAX(SessionNbr) FROM StdtSessEvent " +
                                    "WHERE DSTempHdrId=" + templateID + " AND StudentId=" + studentId + "),0)" + cond + " ORDER BY SS.StdtSessionHdrId " + cond2;
        }
        rdr.Close();
        /*
         SELECT S.StdtSessionStepId,S.StepVal, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName FROM StdtSessionDtl S 
INNER JOIN StdtSessionStep SS ON S.StdtSessionStepId = SS.StdtSessionStepId 
INNER JOIN StdtSessionHdr SH on SS.StdtSessionHdrId = sh.StdtSessionHdrId 
INNER JOIN DSTempSetCol DCol ON S.DSTempSetColId = DCol.DSTempSetColId
INNER JOIN DSTempStep DStep ON DStep.DSTempStepId=SS.DSTempStepId WHERE DStep.SortOrder<=2 AND SH.StdtSessionHdrId 
IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK() OVER (ORDER BY (CASE(IOAInd) 
WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END),EndTs DESC) as RNK  FROM StdtSessionHdr hdr WHERE StudentId = 1 
AND DSTempHdrId = 27544 AND CurrentSetId=15315) as Rk WHERE RNK <= 2) AND SH.IOAInd<>'Y' 
AND DCol.ColName = 'Col 1'AND StartTs > (SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent 
WHERE DSTempHdrId =27544 AND StudentId = 1 ) ORDER BY SS.StdtSessionHdrId 
         
         */

        string line = "";
        string stepval = "";
        SqlDataReader reader = null;
        DataTable dt = null;
        try
        {
            int previousHdrId = 0;

            objTrails.sessionCount = 0;

            SqlCommand cmd2 = new SqlCommand(strSql, con);
            cmd2.Transaction = trans;
            reader = cmd2.ExecuteReader(CommandBehavior.Default);

            //reader = objData.ReturnDataReader(strSql, false);

            //dt = objData.ReturnDataTable(strSql, false);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    //if (dr["StepCd"].ToString() != "" && dr["StepCd"].ToString() != null)
            //}


            while (reader.Read())
            {
                int hdrId = Convert.ToInt32(reader["StdtSessionHdrId"].ToString());
                if (hdrId != previousHdrId)
                {
                    line = "Trial,Score,Duration,Mistrial";
                    objTrails.arTrials.Add(line);
                    objTrails.sessionCount++;
                }
                if (coltypeCode == "+/-")
                {
                    if (correctResponse.ToString() == "-")
                    {
                        string newValue = "";
                        if (reader["StepVal"].ToString() == "-")
                        {
                            newValue = "+";
                        }
                        if (reader["StepVal"].ToString() == "+")
                        {
                            newValue = "-";
                        }

                        line = reader["StdtSessionStepId"].ToString() + "," + newValue + "," + reader["SessionStatusCd"].ToString() + ",10,";
                    }
                    else
                        line = reader["StdtSessionStepId"].ToString() + "," + reader["StepVal"].ToString() + "," + reader["SessionStatusCd"].ToString() + ",10,";
                }
                else
                    line = reader["StdtSessionStepId"].ToString() + "," + reader["StepVal"].ToString() + "," + reader["SessionStatusCd"].ToString() + ",10,";
                stepval += reader["StepVal"].ToString() + ",";

                objTrails.arTrials.Add(line);
                objTrails.trialsCount++;


                previousHdrId = Convert.ToInt32(reader["StdtSessionHdrId"].ToString());
            }

            objTrails.value = stepval;
            if (objTrails.sessionCount != 0)
            {
                objTrails.trialsCount = objTrails.trialsCount / objTrails.sessionCount;
            }
            else
            {
                objTrails.trialsCount = 0;
            }

            reader.Close();
        }
        catch (Exception exp)
        {
            reader.Close();
        }

        strSql = "Select count(DSTempSetId) from DSTempSet where DSTempHdrId=" + templateID + " AND ActiveInd='A'";
        objTrails.totalSet = Convert.ToInt32(objData.FetchValue(strSql));

        return objTrails;
    }

    public DiscreteTrials GetTrialListsForPreStep(int studentId, int templateID, int iCrntSet, int iCrntStep, int nmbrOfSession, string colName, bool flag, string correctResponse, string coltypeCode, string chainedtype, SqlTransaction trans, SqlConnection con)
    {


        DiscreteTrials objTrails = new DiscreteTrials();
        string strSql = "";

        string cond = "", cond2 = "";
        if (flag)
            cond = " AND DStp.SortOrder<>" + iCrntStep;
        if (chainedtype == "Backward chain")
            cond2 = ",StdtSessionStepId DESC";
        else
            cond2 = ",ss.StdtSessionStepId";
        //strSql = "SELECT S.StdtSessionStepId,S.StepVal, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +
        //             " FROM StdtSessionDtl S" +
        //             " INNER JOIN StdtSessionStep SS" +
        //              " ON S.StdtSessionStepId = SS.StdtSessionStepId" +
        //             " INNER JOIN StdtSessionHdr SH" +
        //              " on SS.StdtSessionHdrId = sh.StdtSessionHdrId" +
        //             " INNER JOIN DSTempSetCol DCol" +
        //              " ON S.DSTempSetColId = DCol.DSTempSetColId" +
        //             " WHERE SH.StdtSessionHdrId IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK()" +
        //             " OVER (ORDER BY (CASE(IOAInd) WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END),EndTs DESC) as RNK  FROM StdtSessionHdr hdr" +
        //             " WHERE StudentId = " + studentId + " AND DSTempHdrId = " + templateID + " AND CurrentSetId=" + iCrntSet +
        //             ") as Rk WHERE RNK <= " + nmbrOfSession + ") AND SH.IOAInd<>'Y' AND DCol.ColName = '" + colName + "'AND StartTs >" +
        //             " (SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent" +
        //             " WHERE DSTempHdrId =" + templateID + "AND StudentId = " + studentId + " AND StdtSessEventType!='Minor' AND EventName!='ProbeMode' ) ORDER BY SS.StdtSessionHdrId ";

        //strSql = "SELECT DStp.SortOrder,S.StdtSessionStepId,CASE WHEN S.stepval='' THEN '+' ELSE  S.stepval END AS stepval, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +        
        strSql = "SELECT DStp.SortOrder,S.StdtSessionStepId, S.stepval AS stepval, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName" +
                     " FROM StdtSessionDtl S" +
                     " INNER JOIN StdtSessionStep SS" +
                     " ON S.StdtSessionStepId = SS.StdtSessionStepId" +
                     " INNER JOIN StdtSessionHdr SH" +
                     " on SS.StdtSessionHdrId = sh.StdtSessionHdrId" +
                     " INNER JOIN DSTempSetCol DCol" +
                     " ON S.DSTempSetColId = DCol.DSTempSetColId" +
                     " LEFT JOIN DSTempStep DStp ON SS.DSTempStepId = DStp.DSTempStepId " +
                     " WHERE SessionNbr>(select Top 1 SessionNbr from StdtSessEvent Where DSTempHdrId = " + templateID + " and StudentId= " + studentId + " order by StdtSessEventId desc) and " + 
                     " SH.StdtSessionHdrId IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK()" +
                     " OVER (ORDER BY (CASE(IOAInd) WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END),EndTs DESC) as RNK  FROM StdtSessionHdr hdr" +
                     " WHERE SessMissTrailStus!='Y' AND StudentId = " + studentId + " AND DSTempHdrId = " + templateID + " AND CurrentSetId=" + iCrntSet +
                     ") as Rk WHERE RNK <= " + nmbrOfSession + ") AND SH.IOAInd<>'Y' AND REPLACE(dcol.colname,'''','^') = '" + colName.Replace("'", "^") + "' ORDER BY SS.StdtSessionHdrId " + cond2;

        /*
         SELECT S.StdtSessionStepId,S.StepVal, SS.SessionStatusCd, SS.StdtSessionHdrId, S.DSTempSetColId,DCol.ColName FROM StdtSessionDtl S 
INNER JOIN StdtSessionStep SS ON S.StdtSessionStepId = SS.StdtSessionStepId 
INNER JOIN StdtSessionHdr SH on SS.StdtSessionHdrId = sh.StdtSessionHdrId 
INNER JOIN DSTempSetCol DCol ON S.DSTempSetColId = DCol.DSTempSetColId
INNER JOIN DSTempStep DStep ON DStep.DSTempStepId=SS.DSTempStepId WHERE DStep.SortOrder<=2 AND SH.StdtSessionHdrId 
IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK() OVER (ORDER BY (CASE(IOAInd) 
WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END),EndTs DESC) as RNK  FROM StdtSessionHdr hdr WHERE StudentId = 1 
AND DSTempHdrId = 27544 AND CurrentSetId=15315) as Rk WHERE RNK <= 2) AND SH.IOAInd<>'Y' 
AND DCol.ColName = 'Col 1'AND StartTs > (SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent 
WHERE DSTempHdrId =27544 AND StudentId = 1 ) ORDER BY SS.StdtSessionHdrId 
         
         */

        string line = "";
        string stepval = "";
        SqlDataReader reader = null;
        try
        {
            SqlCommand cmd3 = con.CreateCommand();
            cmd3.Transaction = trans;
            cmd3.CommandText = strSql;
            reader = cmd3.ExecuteReader(CommandBehavior.Default);
            //reader = objData.ReturnDataReader(strSql, false);

            int previousHdrId = 0;

            objTrails.sessionCount = 0;

            while (reader.Read())
            {
                int hdrId = Convert.ToInt32(reader["StdtSessionHdrId"].ToString());
                if (hdrId != previousHdrId)
                {
                    line = "Trial,Score,Duration,Mistrial";
                    objTrails.arTrials.Add(line);
                    objTrails.sessionCount++;
                }
                if (coltypeCode == "+/-")
                {
                    if (correctResponse.ToString() == "-")
                    {
                        string newValue = "";
                        if (reader["StepVal"].ToString() == "-")
                        {
                            newValue = "+";
                        }
                        if (reader["StepVal"].ToString() == "+")
                        {
                            newValue = "-";
                        }

                        line = reader["StdtSessionStepId"].ToString() + "," + newValue + "," + reader["SessionStatusCd"].ToString() + ",10,";
                    }
                    else
                        line = reader["StdtSessionStepId"].ToString() + "," + reader["StepVal"].ToString() + "," + reader["SessionStatusCd"].ToString() + ",10,";
                }
                else
                    line = reader["StdtSessionStepId"].ToString() + "," + reader["StepVal"].ToString() + "," + reader["SessionStatusCd"].ToString() + ",10,";
                stepval += reader["StepVal"].ToString() + ",";

                objTrails.arTrials.Add(line);
                objTrails.trialsCount++;


                previousHdrId = Convert.ToInt32(reader["StdtSessionHdrId"].ToString());
            }

            objTrails.value = stepval;
            if (objTrails.sessionCount != 0)
            {
                objTrails.trialsCount = objTrails.trialsCount / objTrails.sessionCount;
            }
            else
            {
                objTrails.trialsCount = 0;
            }

            reader.Close();
        }
        catch (Exception exp)
        {
            reader.Close();
        }

        strSql = "Select count(DSTempSetId) from DSTempSet where DSTempHdrId=" + templateID + " AND ActiveInd='A'";
        objTrails.totalSet = Convert.ToInt32(objData.FetchValue(strSql));

        return objTrails;
    }



    public string[] GetStepPrompts(int DSTempColID, int StdtSessHdrId)
    {
        clsData objData = new clsData();
        string[] strPrmpts;

        //string sel = "SELECT StepVal FROM StdtSessionDtl Dtl INNER JOIN StdtSessionStep Step ON Step.StdtSessionStepId=Dtl.StdtSessionStepId " +
        //            "WHERE Step.StdtSessionHdrId=" + StdtSessHdrId + " AND Dtl.DSTempSetColId=" + DSTempColID + "";
        string sel = "SELECT PromptId FROM StdtDSStepStat StpStat INNER JOIN StdtSessionStep Step ON Step.DSTempStepId=StpStat.DSTempStepId " +
            "WHERE StpStat.DSTempSetColId=" + DSTempColID + " AND Step.StdtSessionHdrId=" + StdtSessHdrId + "";

        DataTable dt = objData.ReturnDataTable(sel, false);
        if (dt != null)
        {
            strPrmpts = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strPrmpts[i] = dt.Rows[i]["PromptId"].ToString();
            }
            if (dt.Rows.Count == 0)
            {
                object count = objData.FetchValue("SELECT COUNT(*) FROM StdtSessionStep WHERE StdtSessionHdrId=" + StdtSessHdrId + "");
                if (count != null)
                {
                    strPrmpts = new string[Convert.ToInt32(count)];
                    for (int j = 0; j < Convert.ToInt32(count); j++)
                    {
                        strPrmpts[j] = "+";
                    }
                }
                else return null;
            }
            return strPrmpts;
        }
        else return null;
    }

    public bool MultiTeacherStatus(int studentId, int templateId, SqlTransaction trans, SqlConnection con)
    {
        clsData objData = new clsData();
        bool bMultiTrchr = false;
        string strSql = "select count(distinct ModifiedBy) from StdtSessionHdr where DSTempHdrId=" + templateId + " AND SessionStatusCd='S' AND  EndTs > (SELECT ISNULL(MAX(EvntTs),'1900-01-01') "
        + " FROM StdtSessEvent WHERE DSTempHdrId =" + templateId + " AND StudentId = " + studentId + ") ";
        int userCount = Convert.ToInt32(objData.FetchValueTrans(strSql, trans, con));
        if (userCount > 1)
        {
            bMultiTrchr = true;
        }
        return bMultiTrchr;
    }

    public bool IOAStats(int studentId, int templateId, SqlTransaction trans, SqlConnection con)
    {
        clsData objData = new clsData();
        bool bIOAStatus = false;
        string strSql = "select count(distinct IOAInd) from StdtSessionHdr where DSTempHdrId=" + templateId + " AND SessionStatusCd='S' AND IOAInd='Y' AND EndTs > (SELECT ISNULL(MAX(EvntTs),'1900-01-01') "
        + " FROM StdtSessEvent WHERE DSTempHdrId =" + templateId + " AND StudentId =" + studentId + ")";
        int IOACount = Convert.ToInt32(objData.FetchValueTrans(strSql, trans, con));
        if (IOACount > 0)
        {
            bIOAStatus = true;
        }
        return bIOAStatus;

    }

    public bool checkConditionIOA(bool bIOARqd, bool bIOAStatus)
    {


        if (bIOARqd && !bIOAStatus)
            return false;
        else
            return true;

    }

    public bool checkConditionMultiTchr(bool bMultTchrRqd, bool bMultTchrStatus)
    {

        if (bMultTchrRqd && !bMultTchrStatus)
            return false;

        else
            return true;

    }


    public bool updateSetStatus(int schoolId, int classId, int studentId, int templateId, int SetId, int promptId, string nextSet, string resultMessage, string Type, int iSessionNmbr, int loginId, int sLessonPlanId, SqlConnection con, SqlTransaction trans)
    {
        bool IsMaintanace = false;
        clsData objData = new clsData();
        // IsMaintanace = Convert.ToBoolean(hdn_isMaintainance.Value);

        string sqlString = "select statusMessage from StdtDSStat where DSTempHdrId=" + templateId;
        if (objData.FetchValue(sqlString) != null)
        {
            string StatusMessage = objData.FetchValue(sqlString).ToString();
            if (StatusMessage == "COMPLETED")
                IsMaintanace = true;

        }
        int totalset = Convert.ToInt32(objData.FetchValue("select count(setcd) from DSTempSet where DSTempHdrId=" + templateId + "and ActiveInd='A'"));

        if (IsMaintanace)
        {
            if (totalset > Convert.ToInt32(nextSet))
            {
                string updateqry = "UPDATE DSTempHdr SET LessonStatusforBanner= 'SET MOVEDOWN' WHERE DSTempHdrId=" + templateId + "";
                objData.ExecuteWithTrans(updateqry, con, trans);
            }
            return true;
        }
        else
        {
            bool IsSaved = false;
            string strQuery = "";
            strQuery = "SELECT DSTempSetId FROM (SELECT DSTempSetId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempSet " +
                        " WHERE DSTempHdrId = " + templateId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextSet;
            int iSetId = Convert.ToInt32(objData.FetchValue(strQuery));
            strQuery = "SELECT SetCd FROM [DSTempSet] WHERE [DSTempSet].DSTempSetId=" + iSetId;
            string setName = objData.FetchValue(strQuery).ToString();
            //strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ",NextStepId=1, NextSetNmbr='" + nextSet + "',NextPromptId='" + promptId + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";

            //Code for Not Deleting data from Stdtdsstat Preview Issue in Maintenance [16-jul-2020] - Dev 2
            if (resultMessage == "COMPLETED")
            {
                strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ", NextSetNmbr='" + nextSet + "',NextPromptId='" + promptId + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
            }
            else
            {
                strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ",NextStepId=1, NextSetNmbr='" + nextSet + "',NextPromptId='" + promptId + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
            }
            //Code for Not Deleting data from Stdtdsstat Preview Issue in Maintenance [16-jul-2020] - Dev 2

            if (objData.ExecuteWithTrans(strQuery, con, trans) > 0) IsSaved = true;
            string arrowSymbol = "";
            if (Type == "SET MOVEUP")
            {
                arrowSymbol = "↑ SET ";
            }
            else if (Type == "SET MOVEDOWN")
            {
                arrowSymbol = "↓ SET ";
            }

            if (resultMessage != "COMPLETED")
            {
                strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,EventName,StdtSessEventType,SetId,EvntTs,SessionNbr,EventType,LessonPlanId,TimeStampForReport)VALUES" +
                "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',N'" + arrowSymbol + clsGeneral.convertQuotes(setName) + "','Major'," + SetId + ",GETDATE()," + iSessionNmbr + ",'EV', " + sLessonPlanId + ",DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,GETDATE()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "CONVERT(datetime,GETDATE())))";
                objData.ExecuteWithScopeandConnection(strQuery, con, trans);
            }


            if (Type != "SET MOVEDOWN")
            {

                string strqry = "select max(StimulyActivityId) from StdtSessStimuliActivity where DSTempHdrId=" + templateId + " and ActivitiType='SET'";
                string stimuliId = objData.ExecuteWithTrans(strqry, con, trans).ToString();
                if (stimuliId != null && stimuliId != "")
                {
                    strQuery = "Update StdtSessStimuliActivity Set DateMastered=GETDATE() where StimulyActivityId=" + Convert.ToInt32(stimuliId);
                    objData.ExecuteWithTrans(strQuery, con, trans);
                }

            }
            if (resultMessage != "COMPLETED")
            {
                strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
            "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'SET',GETDATE()," + iSetId + "," + loginId + ",GETDATE())";
                objData.ExecuteWithScopeandConnection(strQuery, con, trans);
            }

            return IsSaved;
        }
    }

    public bool updateSetStatus(int schoolId, int classId, int studentId, int templateId, int SetId, string nextSet, string resultMessage, string Type, int iSessionNmbr, int loginId, int sLessonPlanId, SqlConnection con, SqlTransaction trans)
    {
        bool IsMaintanace = false;
        clsData objData = new clsData();
        // IsMaintanace = Convert.ToBoolean(hdn_isMaintainance.Value);

        string sqlString = "select statusMessage from StdtDSStat where DSTempHdrId=" + templateId;
        if (objData.FetchValue(sqlString) != null)
        {
            string StatusMessage = objData.FetchValue(sqlString).ToString();
            if (StatusMessage == "COMPLETED")
                IsMaintanace = true;

        }
        int totalset = Convert.ToInt32(objData.FetchValue("select count(setcd) from DSTempSet where DSTempHdrId=" + templateId + "and ActiveInd='A'"));

        if (IsMaintanace)
        {
            if (totalset > Convert.ToInt32(nextSet))
            {
                string updateqry = "UPDATE DSTempHdr SET LessonStatusforBanner='SET MOVEDOWN' WHERE DSTempHdrId=" + templateId + "";
                objData.ExecuteWithTrans(updateqry, con, trans);
            }
            return true;
        }
        else
        {
            bool IsSaved = false;
            string strQuery = "";
            string dateTime = DateTime.Now.ToShortDateString();
            strQuery = "SELECT DSTempSetId FROM (SELECT DSTempSetId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempSet " +
                        " WHERE DSTempHdrId = " + templateId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextSet;
            int iSetId = Convert.ToInt32(objData.FetchValue(strQuery));
            if (resultMessage == "COMPLETED")
            {
                strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ", NextSetNmbr='" + nextSet + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
            }
            else
            {
                strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ",NextStepId=1, NextSetNmbr='" + nextSet + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
            }
            if (objData.ExecuteWithTrans(strQuery, con, trans) > 0) IsSaved = true;
            //strQuery = "SELECT SetCd FROM [DSTempSet] WHERE [DSTempSet].DSTempSetId=" + SetId;
            strQuery = " SELECT SetCd FROM [DSTempSet] WHERE [DSTempSet].DSTempSetId=" + iSetId;
            string setName = objData.FetchValue(strQuery).ToString();
            string arrowSymbol = "";
            if (Type == "SET MOVEUP")
            {
                arrowSymbol = "↑ SET ";
            }
            else if (Type == "SET MOVEDOWN")
            {
                arrowSymbol = "↓ SET ";
            }
            strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,EventName,StdtSessEventType,SetId,EvntTs,SessionNbr,EventType,LessonPlanId,TimeStampForReport)VALUES" +
                "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',N'" + arrowSymbol + clsGeneral.convertQuotes(setName) + "','Major'," + SetId + ",GETDATE()," + iSessionNmbr + ",'EV', " + sLessonPlanId + ",DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,getdate()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "CONVERT(datetime,getdate())))";
            objData.ExecuteWithScopeandConnection(strQuery, con, trans);
            if (Type != "SET MOVEDOWN")
            {

                string strqry = "select top(1) StimulyActivityId from StdtSessStimuliActivity where DSTempHdrId=" + templateId + "and ActivitiType='SET'";
                string stimuliId = objData.ExecuteWithTrans(strqry, con, trans).ToString();
                if (stimuliId != null && stimuliId != "")
                {
                    strQuery = "Update StdtSessStimuliActivity Set DateMastered=GETDATE() where StimulyActivityId=" + Convert.ToInt32(stimuliId);
                    objData.ExecuteWithTrans(strQuery, con, trans);
                }
            }
            if (resultMessage != "COMPLETED")
            {
                strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
                    "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'SET',GETDATE()," + iSetId + "," + loginId + ",GETDATE())";
                objData.ExecuteWithScopeandConnection(strQuery, con, trans);
            }
            return IsSaved;
        }
    }

    public void updateStepPromptForTotalTask(int templateId, int studentId, int loginId, int promptid, SqlConnection con, SqlTransaction trans)
    {
        clsData objData = new clsData();
        string selSteps = "SELECT DSTempStepId FROM DSTempStep WHERE DSTempHdrId=" + templateId;
        DataTable dtSteps = objData.ReturnDataTable(selSteps, false);
        if (dtSteps != null)
        {
            if (promptid == 0)
            {
                object promptId = objData.FetchValue("SELECT TOP 1 PromptId FROM DSTempPrompt WHERE DSTempHdrId=" + templateId + " ORDER BY PromptOrder");
                if (promptId != null)
                {
                    promptid = Convert.ToInt32(promptId);
                }
            }
            foreach (DataRow drStp in dtSteps.Rows)
            {
                string strQuery = "UPDATE StdtDSStepStat SET PromptId=" + promptid.ToString() + ",ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() " +
                    "WHERE DSTempStepId=" + drStp["DSTempStepId"].ToString();
                objData.ExecuteWithTrans(strQuery, con, trans);
            }
        }
    }

    public bool updateStepStatus(int schoolId, int classId, int studentId, int StepId, int SetId, int templateId, int promptId, string nextStep, string resultMessage, string Type, int iSessionNmbr, int loginId, int sLessonPlanId, SqlConnection con, SqlTransaction trans)
    {
        bool IsMaintanace = false;
        clsData objData = new clsData();
        // IsMaintanace = Convert.ToBoolean(hdn_isMaintainance.Value);

        string sqlString = "select statusMessage from StdtDSStat where DSTempHdrId=" + templateId;
        if (objData.FetchValue(sqlString) != null)
        {
            string StatusMessage = objData.FetchValue(sqlString).ToString();
            if (StatusMessage == "COMPLETED")
                IsMaintanace = true;

        }
        if (IsMaintanace)
        {
            if (Type == "STEP MOVEDOWN")
            {
                string updateqry = "UPDATE DSTempHdr SET LessonStatusforBanner='" + Type + "' WHERE DSTempHdrId=" + templateId + "";
                objData.ExecuteWithTrans(updateqry, con, trans);
            }
            return true;
        }
        else
        {
        bool IsSaved = false;
        string strQuery = "";
        string strQueryMo = "";
        string dateTime = DateTime.Now.ToShortDateString();
        strQuery = "SELECT DSTempStepId FROM (SELECT DSTempStepId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
                    " WHERE DSTempHdrId = " + templateId + " AND DSTempSetId=" + SetId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;
        int iStepId = Convert.ToInt32(objData.FetchValue(strQuery));
        strQuery = "UPDATE StdtDSStat SET NextStepId='" + nextStep + "',NextPromptId='" + promptId + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
        if (objData.ExecuteWithTrans(strQuery, con, trans) > 0) IsSaved = true;
        string sqlStr = "SELECT ChainType FROM DSTempHdr where DSTempHdrId=" + templateId;
        string chType = objData.FetchValue(sqlStr).ToString();
        strQuery = "SELECT SortOrder FROM (SELECT DSTempStepId,SortOrder,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
                    " WHERE DSTempHdrId = " + templateId + " AND DSTempSetId=" + SetId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;
        if (chType == "Backward chain")
        {
            strQuery = "SELECT SortOrder FROM (SELECT DSTempStepId,SortOrder,RANK() OVER (ORDER BY SortOrder DESC) RNK FROM DSTempStep " +
                    " WHERE DSTempHdrId = " + templateId + " AND DSTempSetId=" + SetId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;
        }
        string stepNumber = objData.FetchValue(strQuery).ToString();


        strQueryMo = "SELECT RNK FROM (SELECT DSTempStepId,SortOrder,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
                    " WHERE DSTempHdrId = " + templateId + " AND DSTempParentStepId IN (SELECT DSTempParentStepId FROM DSTempStep WHERE DSTempHdrId=" + templateId + " AND ActiveInd = 'A') AND ActiveInd='A' ) AS Temp WHERE SortOrder = " + stepNumber;

        object stepNumMo = objData.FetchValue(strQueryMo);
        string MasterOrder = "";
        if (stepNumMo != null && stepNumMo.ToString() != "")
        {
            MasterOrder = stepNumMo.ToString();
        }


        string arrowSymbol = "";
        if (Type == "STEP MOVEUP")
        {
            arrowSymbol = "↑ STEP ";
        }
        else if (Type == "STEP MOVEDOWN")
        {
            arrowSymbol = "↓ STEP ";
        }
        strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,EventName,StdtSessEventType,EvntTs,SessionNbr,EventType,StepId,SetId,LessonPlanId,TimeStampForReport)VALUES" +
            "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',N'" + arrowSymbol + MasterOrder +"','Minor'," + "GETDATE()," + iSessionNmbr + ",'EV'," + iStepId + "," + SetId + "," + sLessonPlanId + ",DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,getdate()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "CONVERT(datetime,getdate())))";
        objData.ExecuteWithScopeandConnection(strQuery, con, trans);
        //strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
        //   "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'STEP',GETDATE()," + iStepId + "," + loginId + ",GETDATE())";
        //objData.ExecuteWithScope(strQuery);
        return IsSaved;
        }
    }

    public bool updateStepStatus(int schoolId, int classId, int studentId, int StepId, int SetId, int templateId, string nextStep, string resultMessage, string Type, int iSessionNmbr, int loginId, int sLessonPlanId, SqlConnection con, SqlTransaction trans)
    {
        bool IsMaintanace = false;
        clsData objData = new clsData();
        // IsMaintanace = Convert.ToBoolean(hdn_isMaintainance.Value);

        string sqlString = "select statusMessage from StdtDSStat where DSTempHdrId=" + templateId;
        if (objData.FetchValue(sqlString) != null)
        {
            string StatusMessage = objData.FetchValue(sqlString).ToString();
            if (StatusMessage == "COMPLETED")
                IsMaintanace = true;

        }
        if (IsMaintanace)
        {
            if (Type == "STEP MOVEDOWN")
            {
                string updateqry = "UPDATE DSTempHdr SET LessonStatusforBanner='" + Type + "' WHERE DSTempHdrId=" + templateId + "";
                objData.ExecuteWithTrans(updateqry, con, trans);
            }
            return true;
        }
        else
        {
            bool IsSaved = false;
            string strQuery = "";
            string strQueryMo = "";
            string dateTime = DateTime.Now.ToShortDateString();
            strQuery = "SELECT DSTempStepId FROM (SELECT DSTempStepId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
                        " WHERE DSTempHdrId = " + templateId + " AND DSTempSetId=" + SetId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;
            int iStepId = Convert.ToInt32(objData.FetchValue(strQuery));
            strQuery = "UPDATE StdtDSStat SET NextStepId='" + nextStep + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
            if (objData.ExecuteWithTrans(strQuery, con, trans) > 0) IsSaved = true;
            string sqlStr = "SELECT ChainType FROM DSTempHdr where DSTempHdrId=" + templateId;
            string chType = objData.FetchValue(sqlStr).ToString();
            strQuery = "SELECT SortOrder FROM (SELECT DSTempStepId,SortOrder,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
                        " WHERE DSTempHdrId = " + templateId + " AND DSTempSetId=" + SetId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;

            
            if (chType == "Backward chain")
            {
             strQuery = "SELECT SortOrder FROM (SELECT DSTempStepId,SortOrder,RANK() OVER (ORDER BY SortOrder DESC) RNK FROM DSTempStep " +
                        " WHERE DSTempHdrId = " + templateId + " AND DSTempSetId=" + SetId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;            
            
             }
           
            object stepNum = objData.FetchValue(strQuery);
            string stepNumber = "";
            if (stepNum != null && stepNum.ToString() != "")
            {
                stepNumber = stepNum.ToString();
            }


            strQueryMo = "SELECT RNK FROM (SELECT DSTempStepId,SortOrder,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
                        " WHERE DSTempHdrId = " + templateId + " AND DSTempParentStepId IN (SELECT DSTempParentStepId FROM DSTempStep WHERE DSTempHdrId=" + templateId + " AND ActiveInd = 'A') AND ActiveInd='A' ) AS Temp WHERE SortOrder = " + stepNumber;
          
            object stepNumMo = objData.FetchValue(strQueryMo);
            string MasterOrder = "";
            if (stepNumMo != null && stepNumMo.ToString() != "")
            {
                MasterOrder = stepNumMo.ToString();
            }


            string arrowSymbol = "";
            if (Type == "STEP MOVEUP")
            {
                arrowSymbol = "↑ STEP ";
            }
            else if (Type == "STEP MOVEDOWN")
            {
                arrowSymbol = "↓ STEP ";
            }
            strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,EventName,StdtSessEventType,EvntTs,SessionNbr,EventType,SetId,StepId,LessonPlanId,TimeStampForReport)VALUES" +
                "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',N'" + arrowSymbol + MasterOrder + "','Minor'," + "GETDATE()," + iSessionNmbr + ",'EV'," + SetId + "," + iStepId + "," + sLessonPlanId + ",DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,getdate()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "CONVERT(datetime,getdate())))";
            objData.ExecuteWithScopeandConnection(strQuery, con, trans);
            //strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
            //   "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'STEP',GETDATE()," + iStepId + "," + loginId + ",GETDATE())";
            //objData.ExecuteWithScope(strQuery);
            return IsSaved;
        }
    }

    public bool updatePromptStatus(int schoolId, int classId, int studentId, int templateId, string nextPrompt, string resultMessage, string Type, int iSessionNmbr, int loginId, int crntPromptId, int crntSetId, int crntStepId, int sLessonPlanId, SqlConnection con, SqlTransaction trans)
    {
        bool IsMaintanace = false;
        clsData objData = new clsData();
        // IsMaintanace = Convert.ToBoolean(hdn_isMaintainance.Value);

        string sqlString = "select statusMessage from StdtDSStat where DSTempHdrId=" + templateId;
        if (objData.FetchValue(sqlString) != null)
        {
            string StatusMessage = objData.FetchValue(sqlString).ToString();
            if (StatusMessage == "COMPLETED")
                IsMaintanace = true;

        }
        if (IsMaintanace)
        {
            if (Type == "PROMPT MOVEDOWN")
            {
                string updateqry = "UPDATE DSTempHdr SET LessonStatusforBanner='" + Type + "' WHERE DSTempHdrId=" + templateId + "";
                objData.ExecuteWithTrans(updateqry, con, trans);
            }
            return true;
        }

        else
        {
            bool IsSaved = false;
            string strQuery = "";
            string dateTime = DateTime.Now.ToShortDateString();
            //strQuery = "UPDATE StdtDSStat SET NextPromptId='" + nextPrompt + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";

            //Code for Not Deleting data from Stdtdsstat Preview Issue in Maintenance [16-jul-2020] - Dev 2
            if (resultMessage == "COMPLETED")
            {
                strQuery = "UPDATE StdtDSStat SET statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
            }
            else
            {
                strQuery = "UPDATE StdtDSStat SET NextPromptId='" + nextPrompt + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
            }
            //Code for Not Deleting data from Stdtdsstat Preview Issue in Maintenance [16-jul-2020] - Dev 2

            if (objData.ExecuteWithTrans(strQuery, con, trans) > 0) IsSaved = true;
            strQuery = "SELECT LookUpName FROM LookUp WHERE lookuptype='DSTempPrompt' and LookupId=" + nextPrompt;
            string promptName = objData.FetchValue(strQuery).ToString();
            string arrowSymbol = "";
            if (Type == "PROMPT MOVEUP")
            {
                arrowSymbol = "↑ ";
            }
            else if (Type == "PROMPT MOVEDOWN")
            {
                arrowSymbol = "↓ ";
            }
            strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,EventName,StdtSessEventType,EvntTs,SessionNbr,EventType,SetId,StepId,PromptId,LessonPlanId,TimeStampForReport)VALUES" +
               "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',N'" + arrowSymbol + promptName + "','Minor'," + "GETDATE()," + iSessionNmbr + ",'EV'," + crntSetId + "," + crntStepId + "," + crntPromptId + "," + sLessonPlanId + ",DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,getdate()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "getdate()))";
            objData.ExecuteWithScopeandConnection(strQuery, con, trans);
            //strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
            //   "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'PROMPT',GETDATE()," + nextPrompt + "," + loginId + ",GETDATE())";
            //objData.ExecuteWithScope(strQuery);
            return IsSaved;
        }
    }

    //public bool insertEventStatus(int schoolId, int classId, int studentId, int templateId, int SetId, string resultMessage, string Type, int iSessionNmbr)
    //{
    //    clsData objData = new clsData();
    //    bool IsSaved = false;
    //    string strQuery = "";
    //    strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,SetId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
    //       "(" + schoolId + "," + classId + "," + studentId + "," + templateId + "," + SetId + ",'" + resultMessage + "',GETDATE()," + iSessionNmbr + ",'EV')";
    //    objData.ExecuteWithScope(strQuery);
    //    strQuery = "UPDATE DSTempHdr SET DSMode='MAINTENANCE' WHERE DSTempHdrId=" + templateId;
    //    objData.Execute(strQuery);

    //    return IsSaved;
    //}
    public bool insertEventStatus(int schoolId, int classId, int studentId, int templateId, int SetId, string resultMessage, string Type, int iSessionNmbr, int loginId, int sLessonPlanId, SqlConnection con, SqlTransaction trans)
    {
        clsData objData = new clsData();
        bool IsSaved = false;
        string tempStatus = "";
        string strQuery = "";
        string lpQuery = "";
        string lpComplete = "";
        int statusId = 0;
        int lpCount = 0;
        if (resultMessage == "COMPLETED")
            tempStatus = "Maintenance";

        else
            tempStatus = "Approved";
        string dateTime = DateTime.Now.ToShortDateString();
        strQuery = "SELECT LookupId from Lookup where LookupType='TemplateStatus' and LookupName='" + tempStatus + "'";
        statusId = Convert.ToInt32(objData.FetchValue(strQuery));
        strQuery = "SELECT  DSTemplateName FROM DSTempHdr WHERE DSTempHdrId=" + templateId;
        string lessonName = objData.FetchValueTrans(strQuery, trans, con).ToString();
        lpQuery = "SELECT COUNT(DISTINCT StdtSessEventId) FROM StdtSessEvent WHERE DSTempHdrId = " + templateId + " AND EventName = 'LP Complete'";
        lpCount = Convert.ToInt32(objData.FetchValue(lpQuery));
        lpQuery = "SELECT DSMode from DSTempHdr ds join StdtLessonPlan slp on slp.StdtLessonPlanId=ds.StdtLessonplanId where ds.LessonPlanId= " + sLessonPlanId;
        lpComplete = Convert.ToString(objData.FetchValue(lpQuery));

        if (resultMessage == "COMPLETED" && (lpCount>0))
            if (lpComplete == "MAINTENANCE")
                strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,SetId,StdtSessEventType,EvntTs,SessionNbr,LessonPlanId,EventType,TimeStampForReport)VALUES" +
                "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "'," + SetId + ",'Major',GETDATE()," + iSessionNmbr + "," + sLessonPlanId + ",'EV',DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,getdate()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "CONVERT(datetime,getdate())))";
            else
                strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,SetId,EventName,StdtSessEventType,EvntTs,SessionNbr,LessonPlanId,EventType,TimeStampForReport)VALUES" +
                "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "'," + SetId + ",'LP Complete','Major',GETDATE()," + iSessionNmbr + "," + sLessonPlanId + ",'EV',DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,getdate()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "CONVERT(datetime,getdate())))";
        else
        strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,CheckUp_Down,SetId,EventName,StdtSessEventType,EvntTs,SessionNbr,LessonPlanId,EventType,TimeStampForReport)VALUES" +
           "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "'," + SetId + ",'LP Complete','Major',GETDATE()," + iSessionNmbr + "," + sLessonPlanId + ",'EV',DATEADD(HH,(SELECT (COUNT(*)+1) FROM StdtSessEvent WHERE CONVERT(DATE,EvntTs)=CONVERT(DATE,getdate()) AND " + "SchoolId=" + schoolId + " AND StudentId=" + studentId + " AND EventType='EV' AND LessonPlanId=" + sLessonPlanId + ")," + "CONVERT(datetime,getdate())))";
        objData.ExecuteWithScopeandConnection(strQuery, con, trans);
        if (resultMessage == "COMPLETED")
        {
            strQuery = "UPDATE DSTempHdr SET DSMode='MAINTENANCE', StatusId=" + statusId + " WHERE DSTempHdrId=" + templateId;
            objData.ExecuteWithTrans(strQuery, con, trans);
            if (Type != "SET MOVEDOWN")
            {
                string strqry = "select max(StimulyActivityId) from StdtSessStimuliActivity where DSTempHdrId=" + templateId + " and ActivitiType='SET'";
                string stimuliId = objData.ExecuteWithTrans(strqry, con, trans).ToString();
                if (stimuliId != null && stimuliId != "")
                {
                    strQuery = "Update StdtSessStimuliActivity Set DateMastered=GETDATE() where StimulyActivityId=" + Convert.ToInt32(stimuliId);
                    objData.ExecuteWithTrans(strQuery, con, trans);
                }
            }
        }
        //strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
        //    "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'MASTERED',GETDATE()," + SetId + "," + loginId + ",GETDATE())";
        //objData.ExecuteWithScope(strQuery);
        return IsSaved;
    }

    //public bool updateSetStatus(int schoolId, int classId, int studentId,int SetId, int templateId, int promptId, string nextSet, string resultMessage, string Type, int iSessionNmbr, int loginId)
    //{

    //    bool IsSaved = false;
    //    string strQuery = "";
    //    clsData objData = new clsData();
    //    strQuery = "SELECT DSTempSetId FROM (SELECT DSTempSetId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempSet " +
    //                " WHERE DSTempHdrId = " + templateId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextSet;
    //    int iSetId = Convert.ToInt32(objData.FetchValue(strQuery));
    //    strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ",NextStepId=1, NextSetNmbr='" + nextSet + "',NextPromptId='" + promptId + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
    //    if (objData.Execute(strQuery) > 1) IsSaved = true;
    //    strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
    //        "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',GETDATE()," + iSessionNmbr + ",'EV')";
    //    objData.ExecuteWithScope(strQuery);
    //    strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
    //        "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'SET',GETDATE()," + SetId + "," + loginId + ",GETDATE())";
    //    objData.ExecuteWithScope(strQuery);
    //    return IsSaved;
    //}

    //public bool updateSetStatus(int schoolId, int classId, int studentId,int SetId, int templateId, string nextSet, string resultMessage, string Type, int iSessionNmbr, int loginId)
    //{
    //    bool IsSaved = false;
    //    string strQuery = "";
    //    clsData objData = new clsData();
    //    strQuery = "SELECT DSTempSetId FROM (SELECT DSTempSetId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempSet " +
    //                " WHERE DSTempHdrId = " + templateId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextSet;
    //    int iSetId = Convert.ToInt32(objData.FetchValue(strQuery));
    //    strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ",NextStepId=1, NextSetNmbr='" + nextSet + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
    //    if (objData.Execute(strQuery) > 1) IsSaved = true;
    //    strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
    //        "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',GETDATE()," + iSessionNmbr + ",'EV')";
    //    objData.ExecuteWithScope(strQuery);
    //    strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,,CreatedBy,CreatedOn)VALUES" +
    //        "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'SET',GETDATE()," + SetId + "," + loginId + ",GETDATE())";
    //    objData.ExecuteWithScope(strQuery);
    //    return IsSaved;
    //}

    //public void updateStepPromptForTotalTask(int templateId, int studentId, int loginId)
    //{
    //    clsData objData=new clsData();
    //    string selSteps = "SELECT DSTempStepId FROM DSTempStep WHERE DSTempHdrId=" + templateId;
    //    DataTable dtSteps = objData.ReturnDataTable(selSteps, false);
    //    if (dtSteps != null)
    //    {
    //        object promptId = objData.FetchValue("SELECT TOP 1 PromptId FROM DSTempPrompt WHERE DSTempHdrId=" + templateId + " ORDER BY PromptOrder");
    //        if(promptId!=null)
    //            foreach (DataRow drStp in dtSteps.Rows)
    //            {
    //                string strQuery = "UPDATE StdtDSStepStat SET PromptId=" + promptId.ToString() + ",ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() " +
    //                    "WHERE DSTempStepId=" + drStp["DSTempStepId"].ToString();
    //                objData.Execute(strQuery);
    //            }
    //    }
    //}

    //public bool updateStepStatus(int schoolId, int classId, int studentId,int StepId, int templateId, int promptId, string nextStep, string resultMessage, string Type, int iSessionNmbr, int loginId)
    //{
    //    bool IsSaved = false;
    //    string strQuery = "";
    //    clsData objData = new clsData();
    //    strQuery = "SELECT DSTempStepId FROM (SELECT DSTempStepId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
    //                " WHERE DSTempHdrId = " + templateId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;
    //    int iStepId = Convert.ToInt32(objData.FetchValue(strQuery));
    //    strQuery = "UPDATE StdtDSStat SET NextStepId='" + nextStep + "',NextPromptId='" + promptId + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
    //    if (objData.Execute(strQuery) > 1) IsSaved = true;
    //    strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
    //        "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',GETDATE()," + iSessionNmbr + ",'EV')";
    //    objData.ExecuteWithScope(strQuery);
    //    strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,,CreatedBy,CreatedOn)VALUES" +
    //       "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'STEP',GETDATE()," + StepId + "," + loginId + ",GETDATE())";
    //    objData.ExecuteWithScope(strQuery);
    //    return IsSaved;
    //}

    //public bool updateStepStatus(int schoolId, int classId, int studentId,int StepId, int templateId, string nextStep, string resultMessage, string Type, int iSessionNmbr, int loginId)
    //{
    //    bool IsSaved = false;
    //    string strQuery = "";
    //    clsData objData = new clsData();
    //    strQuery = "SELECT DSTempStepId FROM (SELECT DSTempStepId,RANK() OVER (ORDER BY SortOrder) RNK FROM DSTempStep " +
    //                " WHERE DSTempHdrId = " + templateId + " AND ActiveInd='A' ) AS Temp WHERE RNK = " + nextStep;
    //    int iStepId = Convert.ToInt32(objData.FetchValue(strQuery));
    //    strQuery = "UPDATE StdtDSStat SET NextStepId='" + nextStep + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
    //    if (objData.Execute(strQuery) > 1) IsSaved = true;
    //    strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
    //        "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',GETDATE()," + iSessionNmbr + ",'EV')";
    //    objData.ExecuteWithScope(strQuery);
    //    strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,,CreatedBy,CreatedOn)VALUES" +
    //       "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'STEP',GETDATE()," + StepId + "," + loginId + ",GETDATE())";
    //    objData.ExecuteWithScope(strQuery);
    //    return IsSaved;
    //}

    //public bool updatePromptStatus(int schoolId, int classId, int studentId, int templateId, string nextPrompt, string resultMessage, string Type, int iSessionNmbr, int loginId)
    //{
    //    bool IsSaved = false;
    //    string strQuery = "";
    //    clsData objData = new clsData();
    //    strQuery = "UPDATE StdtDSStat SET NextPromptId='" + nextPrompt + "',statusMessage='" + resultMessage + "' ,ModifiedBy=" + loginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + templateId + "";
    //    if (objData.Execute(strQuery) > 1) IsSaved = true;
    //    strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
    //       "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'" + Type + "',GETDATE()," + iSessionNmbr + ",'EV')";
    //    objData.ExecuteWithScope(strQuery);
    //    strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,,CreatedBy,CreatedOn)VALUES" +
    //       "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ",'PROMPT',GETDATE()," + nextPrompt + "," + loginId + ",GETDATE())";
    //    objData.ExecuteWithScope(strQuery);
    //    return IsSaved;
    //}

    //public bool insertEventStatus(int schoolId, int classId, int studentId, int templateId,int SetId, string resultMessage, string Type, int iSessionNmbr)
    //{
    //    clsData objData = new clsData();
    //    bool IsSaved = false;
    //    string strQuery = "";
    //    strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,SetId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
    //       "(" + schoolId + "," + classId + "," + studentId + "," + templateId + ","+SetId+",'" + resultMessage + "',GETDATE()," + iSessionNmbr + ",'EV')";
    //    objData.ExecuteWithScope(strQuery);
    //    strQuery = "UPDATE DSTempHdr SET DSMode='MAINTENANCE' WHERE DSTempHdrId=" + templateId;
    //    objData.Execute(strQuery);

    //    return IsSaved;
    //}

    public void resetEvntStatus(int schoolId, int studentId, int templateId)
    {
        string strQuery = "";
        strQuery = "UPDATE StdtDSStat SET Set_MoveUp=false,Set_MoveDown=false,Step_MoveUp=false,Step_MoveDown=false,Prompt_MoveUp=false," +
                    "Prompt_MoveDown=false WHERE DSTempHdrId=" + templateId + "";
    }
    public void UpdateAlertEvent(int templateId, string ruleType)
    {
        string strQuery = "";
        strQuery = "UPDATE StdtDSStat SET " + ruleType + " WHERE DSTempHdrId=" + templateId + "";


    }


    public Chained.Result getLearnedStepResult(Chained.InputData Input, Chained.Result result, Chained.InputData inputData, DiscreteTrials TrialLists, string type, int iCurrentStep)
    {
        Chained.Result sesResultChained = new Chained.Result();
        sesResultChained = result;
        if (type == "STEP")
        {
            int stepmoveupCond = Input.LearnedStepMoveUp.TotalTrial;
            int stepmovedownCond = Input.LearnedStepMoveBack.TotalTrial;
            inputData.StepPercentAccuracy.BarCondition = Input.LearnedStepMoveUp.BarCondition;
            inputData.StepPercentAccuracy.ConsecutiveSuccess = Input.LearnedStepMoveUp.ConsecutiveSuccess;
            inputData.StepPercentAccuracy.TotalTrial = Input.LearnedStepMoveUp.TotalTrial;
            inputData.StepPercentAccuracy.SuccessNeeded = Input.LearnedStepMoveUp.SuccessNeeded;
            inputData.StepPercentAccuracy.bIOAReqd = Input.LearnedStepMoveUp.bIOAReqd;
            inputData.StepPercentAccuracy.bMultiTchr = Input.LearnedStepMoveUp.bMultiTchr;

            inputData.StepMoveBackPercentAccuracy.BarCondition = Input.LearnedStepMoveBack.BarCondition;
            inputData.StepMoveBackPercentAccuracy.ConsecutiveFailures = Input.LearnedStepMoveBack.ConsecutiveFailures;
            inputData.StepMoveBackPercentAccuracy.TotalTrial = Input.LearnedStepMoveBack.TotalTrial;
            inputData.StepMoveBackPercentAccuracy.FailureNeeded = Input.LearnedStepMoveBack.FailureNeeded;
            inputData.StepMoveBackPercentAccuracy.bIOAReqd = Input.LearnedStepMoveBack.bIOAReqd;
            inputData.StepMoveBackPercentAccuracy.bMultiTchr = Input.LearnedStepMoveBack.bMultiTchr;
            if (sesResultChained.MovedForwardStep || sesResultChained.MovedBackStep)
            {
                bool stepMoveUpflag = sesResultChained.MovedForwardStep;
                bool stepMoveDownflag = sesResultChained.MovedBackStep;
                result = Chained.Model.Execute(inputData, false, false, "");

                //Check move up
                if (Input.StepPercentAccuracy.TotalTrial > 0)
                {
                    if (sesResultChained.MovedForwardStep)
                    {
                        sesResultChained.MovedForwardStep = result.MovedForwardStep;
                    }
                }
                if (stepmoveupCond > 0)
                {
                    sesResultChained.MovedForwardStep = result.MovedForwardStep;
                }

                //Check move down
                if (Input.StepMoveBackPercentAccuracy.TotalTrial > 0)
                {
                    if (sesResultChained.MovedBackStep)
                    {
                        sesResultChained.MovedBackStep = result.MovedBackStep;
                    }
                }
                if (stepmovedownCond > 0)
                {
                    sesResultChained.MovedBackStep = result.MovedBackStep;
                }
                if (result.MovedBackStep)
                {
                    if (iCurrentStep == 1)
                    {
                        sesResultChained.NextStep = result.NextStep;
                    }
                    else if (iCurrentStep > 1)
                    {
                        if (result.MovedBackSet)
                        {
                            //if (TrialLists.totalSet > 1)
                            //{
                            //    result.MovedBackSet = true;
                            //    result.NextSet = inputData.CurrentSet - 1;

                            //}
                            //else
                            //{
                            sesResultChained.MovedBackSet = false;
                            sesResultChained.MovedBackStep = true;
                            sesResultChained.NextStep = result.NextStep - 1;
                            //  }

                        }
                    }
                }
                result.NextStep++;
                if (result.NextStep > (inputData.StepCount + 1))
                {
                    result.NextStep--;
                }
                sesResultChained.NextStep = result.NextStep;
            }
        }
        if (type == "SET")
        {
            int stepmoveupCond = Input.SetLearnedStepMoveUp.TotalTrial;
            int stepmovedownCond = Input.SetLearnedStepMoveBack.TotalTrial;
            inputData.StepPercentAccuracy.BarCondition = Input.SetLearnedStepMoveUp.BarCondition;
            inputData.StepPercentAccuracy.ConsecutiveSuccess = Input.SetLearnedStepMoveUp.ConsecutiveSuccess;
            inputData.StepPercentAccuracy.TotalTrial = Input.SetLearnedStepMoveUp.TotalTrial;
            inputData.StepPercentAccuracy.SuccessNeeded = Input.SetLearnedStepMoveUp.SuccessNeeded;
            inputData.StepPercentAccuracy.bIOAReqd = Input.SetLearnedStepMoveUp.bIOAReqd;
            inputData.StepPercentAccuracy.bMultiTchr = Input.SetLearnedStepMoveUp.bMultiTchr;

            inputData.StepMoveBackPercentAccuracy.BarCondition = Input.SetLearnedStepMoveBack.BarCondition;
            inputData.StepMoveBackPercentAccuracy.ConsecutiveFailures = Input.SetLearnedStepMoveBack.ConsecutiveFailures;
            inputData.StepMoveBackPercentAccuracy.TotalTrial = Input.SetLearnedStepMoveBack.TotalTrial;
            inputData.StepMoveBackPercentAccuracy.FailureNeeded = Input.SetLearnedStepMoveBack.FailureNeeded;
            inputData.StepMoveBackPercentAccuracy.bIOAReqd = Input.SetLearnedStepMoveBack.bIOAReqd;
            inputData.StepMoveBackPercentAccuracy.bMultiTchr = Input.SetLearnedStepMoveBack.bMultiTchr;
            if (sesResultChained.MovedForwardStep || sesResultChained.MovedBackStep)
            {
                bool stepMoveUpflag = sesResultChained.MovedForwardStep;
                bool stepMoveDownflag = sesResultChained.MovedBackStep;
                result = Chained.Model.Execute(inputData, false, false, "");

                //Check move up
                if (Input.StepPercentAccuracy.TotalTrial > 0)
                {
                    if (sesResultChained.MovedForwardStep)
                    {
                        sesResultChained.MovedForwardStep = result.MovedForwardStep;
                    }
                }
                if (stepmoveupCond > 0)
                {
                    sesResultChained.MovedForwardStep = result.MovedForwardStep;
                }

                //Check move down
                if (Input.StepMoveBackPercentAccuracy.TotalTrial > 0)
                {
                    if (sesResultChained.MovedBackStep)
                    {
                        sesResultChained.MovedBackStep = result.MovedBackStep;
                    }
                }
                if (stepmovedownCond > 0)
                {
                    sesResultChained.MovedBackStep = result.MovedBackStep;
                }
                if (result.MovedBackStep)
                {
                    if (iCurrentStep == 1)
                    {
                        sesResultChained.NextStep = result.NextStep;
                    }
                    else if (iCurrentStep > 1)
                    {
                        if (result.MovedBackSet)
                        {
                            //if (TrialLists.totalSet > 1)
                            //{
                            //    result.MovedBackSet = true;
                            //    result.NextSet = inputData.CurrentSet - 1;

                            //}
                            //else
                            //{
                            sesResultChained.MovedBackSet = false;
                            sesResultChained.MovedBackStep = true;
                            sesResultChained.NextStep = result.NextStep - 1;
                            //  }

                        }
                    }
                }
                result.NextStep++;
                if (result.NextStep > (inputData.StepCount + 1))
                {
                    result.NextStep--;
                }
                sesResultChained.NextStep = result.NextStep;
            }
        }
        if (type == "PROMPT")
        {
            int stepmoveupCond = Input.PromptLearnedStepMoveUp.TotalTrial;
            int stepmovedownCond = Input.PromptLearnedStepMoveBack.TotalTrial;
            inputData.StepPercentAccuracy.BarCondition = Input.PromptLearnedStepMoveUp.BarCondition;
            inputData.StepPercentAccuracy.ConsecutiveSuccess = Input.PromptLearnedStepMoveUp.ConsecutiveSuccess;
            inputData.StepPercentAccuracy.TotalTrial = Input.PromptLearnedStepMoveUp.TotalTrial;
            inputData.StepPercentAccuracy.SuccessNeeded = Input.PromptLearnedStepMoveUp.SuccessNeeded;
            inputData.StepPercentAccuracy.bIOAReqd = Input.PromptLearnedStepMoveUp.bIOAReqd;
            inputData.StepPercentAccuracy.bMultiTchr = Input.PromptLearnedStepMoveUp.bMultiTchr;

            inputData.StepMoveBackPercentAccuracy.BarCondition = Input.PromptLearnedStepMoveBack.BarCondition;
            inputData.StepMoveBackPercentAccuracy.ConsecutiveFailures = Input.PromptLearnedStepMoveBack.ConsecutiveFailures;
            inputData.StepMoveBackPercentAccuracy.TotalTrial = Input.PromptLearnedStepMoveBack.TotalTrial;
            inputData.StepMoveBackPercentAccuracy.FailureNeeded = Input.PromptLearnedStepMoveBack.FailureNeeded;
            inputData.StepMoveBackPercentAccuracy.bIOAReqd = Input.PromptLearnedStepMoveBack.bIOAReqd;
            inputData.StepMoveBackPercentAccuracy.bMultiTchr = Input.PromptLearnedStepMoveBack.bMultiTchr;
            if (sesResultChained.MovedForwardStep || sesResultChained.MovedBackStep)
            {
                bool stepMoveUpflag = sesResultChained.MovedForwardStep;
                bool stepMoveDownflag = sesResultChained.MovedBackStep;
                result = Chained.Model.Execute(inputData, false, false, "");

                //Check move up
                if (Input.StepPercentAccuracy.TotalTrial > 0)
                {
                    if (sesResultChained.MovedForwardStep)
                    {
                        sesResultChained.MovedForwardStep = result.MovedForwardStep;
                    }
                }
                if (stepmoveupCond > 0)
                {
                    sesResultChained.MovedForwardStep = result.MovedForwardStep;
                }

                //Check move down
                if (Input.StepMoveBackPercentAccuracy.TotalTrial > 0)
                {
                    if (sesResultChained.MovedBackStep)
                    {
                        sesResultChained.MovedBackStep = result.MovedBackStep;
                    }
                }
                if (stepmovedownCond > 0)
                {
                    sesResultChained.MovedBackStep = result.MovedBackStep;
                }
                if (result.MovedBackStep)
                {
                    if (iCurrentStep == 1)
                    {
                        sesResultChained.NextStep = result.NextStep;
                    }
                    else if (iCurrentStep > 1)
                    {
                        if (result.MovedBackSet)
                        {
                            //if (TrialLists.totalSet > 1)
                            //{
                            //    result.MovedBackSet = true;
                            //    result.NextSet = inputData.CurrentSet - 1;

                            //}
                            //else
                            //{
                            sesResultChained.MovedBackSet = false;
                            sesResultChained.MovedBackStep = true;
                            sesResultChained.NextStep = result.NextStep - 1;
                            //  }

                        }
                    }
                }
                result.NextStep++;
                if (result.NextStep > (inputData.StepCount + 1))
                {
                    result.NextStep--;
                }
                sesResultChained.NextStep = result.NextStep;
            }
        }
        return sesResultChained;
    }


}


