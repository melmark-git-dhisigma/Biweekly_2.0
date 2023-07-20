using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Web.UI;


public class clsExport
{
    clsData objData = null;
    DataTable Dt = null;
    DataTable Dt2 = null;
    string strQuery = "";
    string[] IEPC = null;
    string[] IEPP = null;
    string[] IEPCHK = null;
    string[] Common = null;
    string[] aC = null;
    string[] aP = null;
    int Count = 0, objcnt = 0, RCount = 0;

    public clsExport()
    {
        Common = new string[3];
        Common[0] = "plcID";
        Common[1] = "plcStudentName";
        Common[2] = "plcDOB";

    }

    public void getIEP(out string[] C, out string[] P, int StudentId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT S.StudentNbr AS IDNO, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,St1.EffStartDate as StartDate,St1.EffEndDate as EndDate  FROM  Student S  Inner Join School Sc On s.SchoolId=Sc.SchoolId Inner Join Address Ad   on Ad.AddressId=Sc.DistAddrId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId where S.SchoolId=1 And S.StudentId=1";

            Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Count = Dt.Columns.Count;
                    IEPC = new string[Count];
                    IEPP = new string[Count];

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            IEPP[i] = Common[i].ToString();
                        }
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C = new string[Count];
        P = new string[Count];
        Array.Copy(IEPC, C, Count);
        Array.Copy(IEPP, P, Count);

    }


    public void getIEP1(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT Sc.DistrictName,Ad.AddressLine1+','+Ad.AddressLine2+','+Ad.AddressLine3 as Address,Sc.DistPhone,CONVERT(VARCHAR(10),St1.EffStartDate, 101) as StartDate,CONVERT(VARCHAR(10),St1.EffEndDate, 101) as EndDate, ";
            //strQuery += "S.StudentFname+','+S.StudentLname AS Name,CONVERT(VARCHAR(101),S.DOB, 101) AS DOB,S.StudentNbr AS IDNO,S.GradeLevel as Grade,St1.Concerns,St1.Strength, ";
            //strQuery += "St1.Vision FROM  Student S  Inner Join School Sc On s.SchoolId=Sc.SchoolId Inner Join Address Ad ";
            //strQuery += "on Ad.AddressId=Sc.DistAddrId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId where S.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And St1.StdtIEPId=" + IEPId + " ";
            strQuery = "SELECT SP.IEPReferralSourceofTuition,Ad.AddressLine1+','+Ad.AddressLine2+','+Ad.AddressLine3 as Address,Sc.DistPhone,CONVERT(VARCHAR(10),St1.EffStartDate, 101) as StartDate,CONVERT(VARCHAR(10),St1.EffEndDate, 101) as EndDate, ";
            strQuery += "S.StudentFname+','+S.StudentLname AS Name,CONVERT(VARCHAR(101),S.DOB, 101) AS DOB,S.StudentNbr AS IDNO,S.GradeLevel as Grade,St1.Concerns,St1.Strength, ";
            strQuery += "St1.Vision FROM  Student S  Inner Join School Sc On s.SchoolId=Sc.SchoolId Inner Join Address Ad ";
            strQuery += "on Ad.AddressId=Sc.DistAddrId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId INNER JOIN StudentPersonal SP ON SP.StudentPersonalId = St1.StudentId where S.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And St1.StdtIEPId=" + IEPId + " ";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "true") || (Dr[i].ToString() == "false"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP2(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr, IE1.EngLangInd, IE1.HistInd, IE1.TechInd, IE1.MathInd, IE1.OtherInd, IE1.OtherDesc, IE1.AffectDesc, IE1.AccomDesc, IE1.ContentModInd, ";
            strQuery += "IE1.ContentModDesc, IE1.MethodModInd, IE1.MethodModDesc,IE1.PerfModInd, IE1.PerfModDesc FROM  StdtIEPExt1 IE1 Inner Join StdtIEP IE ON IE.StdtIEPId=IE1.StdtIEPId ";
            strQuery += "Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId ";
            strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And IE1.StdtIEPId=" + IEPId + "  ";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP3(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr, ";
            strQuery += "PEInd, TechDevicesInd, BehaviorInd, BrailleInd, CommInd, CommDfInd, ExtCurInd, LEPInd, NonAcdInd, SocialInd, TravelInd, VocInd, OtherInd, OtherDesc, ";
            strQuery += "AgeBand1Ind, AgeBand2Ind, AgeBand3Ind, AffectDesc,AccomDesc, ContentModInd, ContentModDesc,MethodModInd,MethodModDesc,PerfModInd, PerfModDesc FROM StdtIEPExt2 IE2 ";
            strQuery += "Inner Join StdtIEP IE ON IE.StdtIEPId=IE2.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.StudentId=IE.StudentId ";
            strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And IE2.StdtIEPId=" + IEPId + "  ";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public DataTable ReturnRows(int numbr, DataTable dttemp)
    {
        DataTable dt = new DataTable();
        dt = dttemp.Clone();

        try
        {
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                if (i.ToString() == numbr.ToString())
                {
                    DataRow dr2 = dt.NewRow();
                    dr2 = dttemp.Rows[i];
                    dt.Rows.Add(dr2.ItemArray);
                    DataRow dr3 = dt.NewRow();
                    dr3 = dttemp.Rows[i + 1];
                    dt.Rows.Add(dr3.ItemArray);
                }

            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
        return dt;
    }

    public DataTable getIEP4Data(int StudentId, int SchoolId, int IEPId, out bool Odd)
    {
        Odd = false;
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            strQuery = "select distinct  Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId=" + IEPId + " and StdtLessonPlan.IncludeIEP=1";
            Dt = objData.ReturnDataTable(strQuery, false);
            string GoalIdZ = "";
            int countForGoalId = 0;
            foreach (DataRow dr in Dt.Rows)
            {
                if (countForGoalId == 0)
                {
                    GoalIdZ += dr["Id"].ToString();
                }
                else if (countForGoalId > 0)
                {
                    GoalIdZ += "," + dr["Id"].ToString();
                }
                countForGoalId++;
            }

            if (Dt.Rows.Count == 0)
            {
                GoalIdZ = "0";
            }
            //  strQuery = "select distinct Goal.GoalName as Name, Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId='" + sess.IEPId + "' and StdtLessonPlan.IncludeIEP=1";

            //strQuery = "Select CONVERT(VARCHAR(10),IE.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 101) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr,GL.GoalName,SG.Objective1, SG.Objective2 ,SG.Objective3 FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + "";
            //Select CONVERT(VARCHAR(10),IE.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 101) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr,GL.GoalName,dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3 FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId inner join StdtLessonPlan on IE.StdtIEPId=StdtLessonPlan.StdtIEPId where Sc.SchoolId=1 And S.StudentId=2  And IE.StdtIEPId=2 and dbo.StdtLessonPlan.ActiveInd = 'A'
            //  strQuery = "select CONVERT(VARCHAR(10),StdtIEP.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),StdtIEP.EffEndDate, 101) as EndDate, Student.StudentFname+' '+Student.StudentLname AS Name, CONVERT(VARCHAR(10),Student.DOB, 101) AS DOB,Student.StudentNbr,Goal.GoalName,StdtLessonPlan.Objective1,StdtLessonPlan.Objective2,StdtLessonPlan.Objective3 from StdtLessonPlan inner join StdtIEP on StdtIEP.StdtIEPId=StdtLessonPlan.StdtIEPId inner join Student on Student.StudentId=StdtLessonPlan.StudentId inner join Goal on Goal.GoalId=StdtLessonPlan.GoalId where StdtLessonPlan.StudentId=" + StudentId + " and StdtLessonPlan.ActiveInd = 'A' and StdtLessonPlan.StdtIEPId=" + IEPId + " and StdtLessonPlan.SchoolId=" + SchoolId + "";

            //strQuery = "SELECT CONVERT(VARCHAR(10),StdtIEP.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),StdtIEP.EffEndDate, 101) as EndDate," +
            //            "Student.StudentFname+' '+Student.StudentLname AS Name, CONVERT(VARCHAR(10),Student.DOB, 101) AS DOB,Student.StudentNbr,StdtGoal.IEPGoalNo,Goal.GoalName," +
            //               " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3" +
            //              " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
            //              "    dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId inner join StdtIEP on StdtIEP.StdtIEPId=StdtLessonPlan.StdtIEPId inner join Student on Student.StudentId=StdtLessonPlan.StudentId inner join StdtGoal on StdtIEP.StdtIEPId=StdtGoal.StdtIEPId" +
            //            "  where StdtLessonPlan.GoalId in (" + GoalIdZ + ") AND dbo.StdtLessonPlan.StdtIEPId =  " + IEPId + "  AND  dbo.StdtLessonPlan.ActiveInd = 'A' and StdtLessonPlan.StudentId=" + StudentId + " and StdtLessonPlan.SchoolId=" + SchoolId + "   ORDER BY " +
            //           "   dbo.StdtLessonPlan.GoalId ";

            strQuery = "SELECT    dbo.StdtLessonPlan.StudentId,(SELECT StdtGoalId FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + IEPId + ")"
          + "Id, dbo.StdtLessonPlan.GoalId,StdtIEP.AsmntYearId,(SELECT IEPGoalNo FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + IEPId + ")" +
          " IEPGoalNo,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, dbo.Goal.GoalName,CASE WHEN (SELECT TOP 1 DSTemplateName "+
          " FROM DSTempHdr WHERE StdtLessonplanId=dbo.StdtLessonPlan.StdtLessonPlanId ORDER BY DSTempHdrId DESC) IS NULL THEN LessonPlan.LessonPlanName ELSE (SELECT TOP 1 "+
          " DSTemplateName FROM DSTempHdr WHERE StdtLessonplanId=dbo.StdtLessonPlan.StdtLessonPlanId ORDER BY DSTempHdrId DESC) END LessonPlanName" +
          "   FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON " +
          "dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId inner join dbo.StdtIEP ON dbo.StdtLessonPlan.StdtIEPId=dbo.StdtIEP.StdtIEPId where " +
          "StdtLessonPlan.GoalId in (" + GoalIdZ + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + IEPId + "'    ORDER BY dbo.StdtLessonPlan.GoalId ";


            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count % 2 == 1)
            {
                Odd = true;
                Dt.Rows.Add(null, null, null, null, null, " ", " ", " ", " ", " ");
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
        return Dt;
    }

    public void getIEP4(out string[] C4, DataTable Dt)
    {
        int iepCount = 0;
        try
        {

            int i = 0;

            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Count = Dt.Columns.Count;

                    iepCount = Count * Dt.Rows.Count;
                    IEPC = new string[iepCount];
                    int k = 0;
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        if (k < 10)
                        {
                            for (i = 0; i < Count; i++)
                            {
                                IEPC[k] = Dr[i].ToString();

                                k++;

                            }
                        }
                        else
                        {
                            k = 10;
                            for (i = 5; i < Count; i++)
                            {
                                IEPC[k] = Dr[i].ToString();
                                k++;

                            }
                        }
                    }

                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }

        C4 = new string[iepCount];

        Array.Copy(IEPC, C4, iepCount);


    }



    //public void getIEP5(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    //{
    //    try
    //    {

    //        objData = new clsData();
    //        Dt = new DataTable();
    //        int i = 0;
    //        strQuery = " select Convert(varchar(50),StdtIEP.EffStartDate,101) as StartDate,Convert(varchar(50),StdtIEP.EffEndDate,101)  as EndDate,";
    //        strQuery += " Student.StudentLname+' , '+Student.StudentFname AS Name,  CONVERT(VARCHAR(10),Student.DOB, 101) AS DOB, Student.StudentNbr,";
    //        strQuery += " StdtIEPExt2.OtherDesc1,StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,''";
    //        strQuery += " from StdtIEP inner join Student on Student.StudentId=StdtIEP.StudentId ";
    //        strQuery += " inner join StdtIEPExt2 on StdtIEPExt2.StdtIEPId=StdtIEP.StdtIEPId ";
    //        strQuery += " where StdtIEP.StdtIEPId=" + IEPId + " AND StdtIEP.SchoolId =" + SchoolId + " AND StdtIEP.StudentId = " + StudentId + " ";

    //        Dt = objData.ReturnDataTable(strQuery, false);

    //        Count = Dt.Columns.Count;
    //        IEPC = new string[Count];
    //        IEPCHK = new string[Count];
    //        int index = 0;
    //        if (Dt != null)
    //        {
    //            if (Dt.Rows.Count > 0)
    //            {

    //                foreach (DataRow Dr in Dt.Rows)
    //                {
    //                    for (i = 0; i < Count; i++)
    //                    {
    //                        IEPC[i] = Dr[i].ToString();
    //                        if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
    //                        {
    //                            index++;
    //                            IEPCHK[i] = Dr[i].ToString();
    //                        }
    //                    }
    //                }

    //            }
    //        }
    //        for (i = 0; i < IEPC.Length; i++)
    //        {
    //            if (IEPC[i] == null)
    //            {
    //                IEPC[i] = " ";
    //            }
    //        }

    //    }
    //    catch (Exception Ex)
    //    {
    //        ClsErrorLog errlog = new ClsErrorLog();
    //        errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
    //    }


    //    C1 = new string[Count];
    //    C2 = new string[Count];
    //    if (IEPC != null) Array.Copy(IEPC, C1, Count);
    //    if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    //}


    public void getIEP5(out string[] C5, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT DISTINCT Convert(varchar(50),IEP.EffStartDate,101) as StartDate,Convert(varchar(50),IEP.EffEndDate,101)  as EndDate, S.StudentLname+' , '+S.StudentFname AS Name, ";
            //strQuery += " CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr,IE2.OtherDesc1,'False','True','False','True' FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId INNER JOIN StdtIEPExt2 ";
            //strQuery += " IE2 ON IE2.StdtIEPId=IEP.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " ";


            strQuery = " select Convert(varchar(50),StdtIEP.EffStartDate,101) as StartDate,Convert(varchar(50),StdtIEP.EffEndDate,101)  as EndDate,";
            strQuery += " Student.StudentLname+' , '+Student.StudentFname AS Name,  CONVERT(VARCHAR(10),Student.DOB, 101) AS DOB, Student.StudentNbr,";
            strQuery += " StdtIEPExt2.OtherDesc1,StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,''";
            strQuery += " from StdtIEP inner join Student on Student.StudentId=StdtIEP.StudentId ";
            strQuery += " inner join StdtIEPExt2 on StdtIEPExt2.StdtIEPId=StdtIEP.StdtIEPId ";
            strQuery += " where StdtIEP.StdtIEPId=" + IEPId + " AND StdtIEP.SchoolId =" + SchoolId + " AND StdtIEP.StudentId = " + StudentId + " ";


            Dt = objData.ReturnDataTable(strQuery, false);

            IEPC = new string[119];

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (i = 0; i < Count; i++)
                    {
                        IEPC[i] = Dr[i].ToString();
                    }
                }

            }


            //strQuery = "SELECT G.GoalName as GoalName,  SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate";
            //strQuery += " FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  Inner Join Goal G On G.GoalId=SGS.StdtGoalId ";
            //strQuery += " WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " and SGS.SvcDelTyp='A'";

            // 
            strQuery = "select StdtGoalSvc.StdtGoalId as 'GoalName',";
            strQuery += "StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc, StdtGoalSvc.FreqDurDesc,convert(varchar(50), StdtGoalSvc.StartDate,101) as StartDate,";
            strQuery += "convert(varchar(50),StdtGoalSvc.EndDate,101) as EndDate ";
            strQuery += "from StdtGoalSvc  inner join ";
            strQuery += "StdtIEP on StdtIEP.StdtIEPId=StdtGoalSvc.StdtIEPId  where StdtIEP.StdtIEPId=" + IEPId + " ";
            strQuery += "and StdtIEP.StudentId=" + StudentId + " and StdtIEP.SchoolId=" + SchoolId + " and StdtGoalSvc.SvcDelTyp='A'";

            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ", " ");
                }
            }


            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }



            //strQuery = "SELECT G.GoalName as GoalName,  SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate";
            //strQuery += " FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  Inner Join Goal G On G.GoalId=SGS.StdtGoalId ";
            //strQuery += " WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " and SGS.SvcDelTyp='B'";

            strQuery = "select StdtGoalSvc.StdtGoalId as 'GoalName',";
            strQuery += "StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc, StdtGoalSvc.FreqDurDesc,convert(varchar(50), StdtGoalSvc.StartDate,101) as StartDate,";
            strQuery += "convert(varchar(50),StdtGoalSvc.EndDate,101) as EndDate ";
            strQuery += "from StdtGoalSvc  inner join ";
            strQuery += "StdtIEP on StdtIEP.StdtIEPId=StdtGoalSvc.StdtIEPId  where StdtIEP.StdtIEPId=" + IEPId + " ";
            strQuery += "and StdtIEP.StudentId=" + StudentId + " and StdtIEP.SchoolId=" + SchoolId + " and StdtGoalSvc.SvcDelTyp='B'";


            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count < 6)
            {
                int c = 6 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ", " ");
                }
            }

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }



            //strQuery = "SELECT G.GoalName as GoalName,  SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate";
            //strQuery += " FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  Inner Join Goal G On G.GoalId=SGS.StdtGoalId ";
            //strQuery += " WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " and SGS.SvcDelTyp='C'";
            strQuery = "select StdtGoalSvc.StdtGoalId as 'GoalName',";
            strQuery += "StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc, StdtGoalSvc.FreqDurDesc,convert(varchar(50), StdtGoalSvc.StartDate,101) as StartDate,";
            strQuery += "convert(varchar(50),StdtGoalSvc.EndDate,101) as EndDate ";
            strQuery += "from StdtGoalSvc  inner join ";
            strQuery += "StdtIEP on StdtIEP.StdtIEPId=StdtGoalSvc.StdtIEPId  where StdtIEP.StdtIEPId=" + IEPId + " ";
            strQuery += "and StdtIEP.StudentId=" + StudentId + " and StdtIEP.SchoolId=" + SchoolId + " and StdtGoalSvc.SvcDelTyp='C'";

            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count < 7)
            {
                int c = 7 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ", " ");
                }
            }

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }



        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }

        for (int i = 0; i < IEPC.Length; i++)
        {
            if (IEPC[i] == null)
            {
                IEPC[i] = " ";
            }
        }

        C5 = new string[119];
        Array.Copy(IEPC, C5, 119);
        C2 = new string[119];
        Array.Copy(IEPC, C2, 119);

    }

    public void getIEP6(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId, int StatusId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "Select IEP.EffStartDate as StartDate,IEP.EffEndDate as EndDate, St1.StudentFname+','+St1.StudentLname AS Name,  CONVERT(VARCHAR(10),St1.DOB, 101) AS DOB,St1.StudentNbr, ";
            strQuery += "IEP3.RemovedInd1,IEP3.RemovedInd2,IEP3.RemovedDesc,IEP3.ShorterCd1,IEP3.ShorterCd2,IEP3.ShorterCd3,IEP3.LongerCd1,IEP3.LongerCd2,IEP3.LongerCd3,IEP3.SchedModDesc,IEP3.TransportInd1,IEP3.TransportInd2,IEP3.RegTransInd,IEP3.RegTransDesc,IEP3.SpTransInd ,";
            strQuery += "IEP3.SpTransDesc from StdtIEPExt3 IEP3  INNER JOIN StdtIEP IEP  ON IEP.StdtIEPId  =IEP3.StdtIEPId   ";
            strQuery += "Inner Join School Sc On IEP.SchoolId=Sc.SchoolId Inner Join Student St1 On IEP.StudentId=St1.StudentId ";
            strQuery += "where IEP.SchoolId=" + SchoolId + " And IEP.StudentId=" + StudentId + " And IEP.StdtIEPId=" + IEPId + "";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP7(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "Select IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr, ";
            strQuery += "AsmntPlanned,EngCol1,EngCol2,EngCol3,HistCol1,HistCol2 ,HistCol3,MathCol1,MathCol2,MathCol3 ,TechCol1,TechCol2,TechCol3,ReadCol1,ReadCol2,ReadCol3,InfoCol2,InfoCol3 ";
            strQuery += "from StdtIEPExt3 E3 Inner Join StdtIEP IE ON IE.StdtIEPId=E3.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId ";
            strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + " ";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP8(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "Select IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr, ";
            strQuery += "AddInfoCol1,AddInfoCol2,AddInfoCol3,AddInfoCol3Desc from StdtIEPExt3 IE3 Inner Join StdtIEP IE ON IE.StdtIEPId=IE3.StdtIEPId Inner Join School Sc On ";
            strQuery += "IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId  ";
            strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + "  ";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }
    #region IEP Export Page from 9 to 12--Hari
    public void getIEP9(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT distinct PlOneEarlyPgm,PlOneSeparatePgm,PlOneBothPgm,PlOneEnrolledPrnt,PlOnePlcdTeam,PlOneTimeMore,PlOneTimeTwo,PlOneTimeThree,PlOneSeparateClass," +
             "PlOneSeparateDayScl,PlOneSeparatePublic,PlOneSeparatePvt,PlOneResidentialFacility,PlOneHome,PlOneServiceLctn,PlOnePsychiatric,PlOneMassachusetts," +
             "PlOneMassachusettsDay,PlOneMassachusettsRes,PlOneDoctorHme,PlOneDoctorHsptl,PlOneConsent,PlOneRefuse,PlOnePlacement,PlTwoFullInclusionPgm," +
            "PlTwoPartialPgm,PlTwoSubstantially,PlTwoSeparateScl,PlTwoPublicScl,PlTwoPrivateScl,PlTwoResScl,PlTwoOther,PlTwoYouth,PlTwoPsychiatric,PlTwoMassachusetts," +
            "PlTwoMassachusettsDay,PltwoMassachusettsRes,PlTwoCorrectionFacility,PlTwoDoctorHome,PlTwoDoctorHsptl,PlTwoConsent,PlTwoPlacement,PlTwoFullPgm," +
            "PlOneHoursWkPgm,PlOneServiceLocation,PlOneServiceLocation2,PlOneSignParent,PlTwoSignParent,FORMAT(PlOneDate,'MM/dd/yyyy'),FORMAT(PlTwoDate,'MM/dd/yyyy'),PlTwoOtherDesc " +
            "FROM StdtIEPExt4 IE4 INNER JOIN StdtIEP IE ON IE.StdtIEPId=IE4.StdtIEPId INNER JOIN School SC ON IE.SchoolId=SC.SchoolId INNER JOIN Student S ON S.SchoolId=SC.SchoolId " +
             "INNER JOIN StdtIEP ON S.StudentId=IE.StudentId WHERE SC.SchoolId='" + SchoolId + "' AND S.StudentId='" + StudentId + "' AND IE.StdtIEPId='" + IEPId + "'";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }
    public void getIEP10(out string[] C5, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            Dt2 = new DataTable();
            int i = 0;
            int k = 0;

            strQuery = "Select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS,case when ((MONTH(ST.DOB) * 100) + DAY(ST.DOB)) > ((MONTH(getdate()) * 100) + DAY(getdate()))then DATEDIFF(year,ST.DOB,getdate()) - 1 else DATEDIFF(year,ST.DOB,getdate()) End As Age,SP.MostRecentGradeLevel,ST.SchoolId,SP.SASID,ADR.ApartmentType+','+ADR.StreetName+','+ISNULL(ADR.City,'') AS Address,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + StudentId + " AND ContactSequence=1)) AS Phone,SP.PlaceOfBirth,SP.PrimaryLanguage,(SELECT CASE WHEN ST.Gender=1 THEN cast (1 as bit) ELSE cast (0 as bit) END )AS GENDERMALE,(SELECT CASE WHEN ST.Gender=2 THEN cast (1 as bit) ELSE cast (0 as bit) END )AS GENDERFEMALE"
            + " from Student ST Inner Join StudentAddresRel SAR ON ST.StudentId=SAR.StudentPersonalId "
            + "Inner Join AddressList ADR ON SAR.AddressId=ADR.AddressId inner join StudentPersonal SP ON SP.StudentPersonalId=ST.StudentId Where ST.StudentId=" + StudentId + " And ST.SchoolId=" + SchoolId + " And SAR.ContactSequence=0";

            Dt = objData.ReturnDataTable(strQuery, false);
            IEPC = new string[43];
            IEPCHK = new string[43];
            int index = 0;
            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (i = 0; i < Count; i++)
                    {
                        IEPC[i] = Dr[i].ToString();
                        k++;
                        if (Dr[i].ToString() == "True" || Dr[i].ToString() == "False")
                        {
                            index++;
                            IEPCHK[i] = Dr[i].ToString();

                        }
                    }
                }

            }



            strQuery = "SELECT IEP4.LanguageofInst,IEP4.RoleDesc,IEP4.ActonOwnBehalfCk,IEP4.CourtAppGrdCk,IEP4.SharedDecMakingCk,IEP4.DelegateDeciMakCk,IEP4.CourtAppGuardian,IEP4.PrLanguageGrd1,IEP4.PrLanguageGrd2,FORMAT(IEP4.DateOfMeeting,'MM/dd/yyyy') AS DateOfMeeting,IEP4.TypeOfMeeting,IEP4.AnnualReviewMeeting,IEP4.ReevaluationMeeting,(SELECT CASE WHEN IEP4.CostSharedPnt='False' THEN cast (0 as bit) ELSE cast (1 as bit) END )AS CostSharedPnt,(SELECT CASE WHEN IEP4.CostSharedPnt='True' THEN cast (0 as bit) ELSE cast (1 as bit) END )AS CostSharedPntYes,IEP4.SpecifyAgency FROM StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE IEP1.StdtIEPId = " + IEPId + " ";

            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                        if (Dr[j].ToString() == "True" || Dr[j].ToString() == "False")
                        {
                            index++;
                            IEPCHK[k] = Dr[j].ToString();
                            k++;

                        }
                    }
                }

            }


            strQuery = "Select distinct CP.LastName+','+CP.FirstName As GuardianName,'Legal Guardian' As LegalGuardian1,AL.StreetName+','+AL.ApartmentType+','+AL.City AS PrimaryContactAddress,AL.Phone AS HomePhone,AL.OtherPhone AS WorkPhone from AddressList AL"
                        + " Inner Join StudentAddresRel ADR on AL.AddressId=ADR.AddressId"
                        + " Inner Join StudentPersonal SP on SP.StudentPersonalId=ADR.StudentPersonalId"
                        + " Inner join ContactPersonal CP ON CP.ContactPersonalId=ADR.ContactPersonalId"
                        + " Inner join StudentContactRelationship SCR on SCR.ContactPersonalId=CP.ContactPersonalId"
                        + " Inner join LookUp LK on LK.LookupId=SCR.RelationshipId"
                       + " where ADR.ContactSequence=1  AND SP.StudentPersonalId=" + StudentId + " And SP.StudentType='Client' "
                        + " And SP.SchoolId=" + SchoolId + "  And LK.LookupName='Legal Guardian' AND CP.Status=1";


            Dt = objData.ReturnDataTable(strQuery, false);



            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }
            if (Dt.Rows.Count == 0)
            {

                for (int j = 0; j < 5; j++)
                {
                    i++;
                }

            }




            strQuery = "Select distinct CP.LastName+','+CP.FirstName As GuardianName,'Parent' As LegalGuardian2,AL.StreetName+','+AL.ApartmentType+','+AL.City AS PrimaryContactAddress,AL.Phone AS HomePhone,AL.OtherPhone AS WorkPhone from AddressList AL"
                        + " Inner Join StudentAddresRel ADR on AL.AddressId=ADR.AddressId"
                        + " Inner Join StudentPersonal SP on SP.StudentPersonalId=ADR.StudentPersonalId"
                        + " Inner join ContactPersonal CP on CP.ContactPersonalId=ADR.ContactPersonalId"
                        + " Inner join StudentContactRelationship SCR on SCR.ContactPersonalId=CP.ContactPersonalId"
                        + " Inner join LookUp LK on LK.LookupId=SCR.RelationshipId"
                       + "  where ADR.ContactSequence=1  AND SP.StudentPersonalId=" + StudentId + " And SP.StudentType='Client' "
                        + " And SP.SchoolId=" + SchoolId + "  And LK.LookupName='Parent' AND CP.Status=1";

            Dt = objData.ReturnDataTable(strQuery, false);



            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }
            if (Dt.Rows.Count == 0)
            {

                for (int j = 0; j < 5; j++)
                {
                    i++;
                }

            }

            string srQuery = "SELECT sch.SchoolName As SName,sch.DistPhone As Phone,Adr.AddressLine1+','+Adr.AddressLine2+','+Adr.AddressLine3 As ScAdd,sch.DistContact As cont,sch.DistPhone AS WorkPhone " +
                                " from School Sch " +
                                  "   LEFT JOIN (Address Adr INNER JOIN LookUp lu ON lu.LookupId=Adr.State) " +
                                             "  ON Sch.DistAddrId = Adr.AddressId  WHERE sch.ActiveInd = 'A'  And sch.SchoolId=" + SchoolId + " ";
            Dt2 = objData.ReturnDataTable(srQuery, false);
            strQuery = "SELECT SchoolName,SchoolPhone,SchAddress,SchContact,SchTelephone from StdtIEPExt4 join StudentPersonal s on s.StudentPersonalId='" + StudentId + "' and StdtIEPId='" + IEPId + "' where SchoolName is not null or SchoolPhone is not null or SchAddress is not null or SchContact is not null or SchTelephone is not null";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt != null)
            {

                if (Dt.Rows.Count > 0)
                {
                    Count = Dt.Columns.Count;

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            i++;
                        }
                    }

                }
                else
                {
                    if (Dt2.Rows.Count > 0)
                    {
                        Count = Dt2.Columns.Count;

                        foreach (DataRow Dr in Dt2.Rows)
                        {
                            for (int j = 0; j < Count; j++)
                            {
                                IEPC[i] = Dr[j].ToString();
                                i++;
                            }
                        }

                    }
                }
            }
        }

        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }

        for (int i = 0; i < IEPC.Length; i++)
        {
            if (IEPC[i] == null)
            {
                IEPC[i] = " ";
            }
        }

        C5 = new string[43];
        C2 = new string[43];
        if (IEPC != null) Array.Copy(IEPC, C5, 43);
        if (IEPC != null) Array.Copy(IEPCHK, C2, 43);

    }
    public void getIEP11(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT FORMAT(StudentPersonal.AdmissionDate,'MM/dd/yyyy') AS AdmissionDate,StudentPersonal.LastName+',' +StudentPersonal.FirstName As Name,FORMAT(StudentPersonal.BirthDate,'MM/dd/yyyy') AS BirthDate,StudentPersonal.StudentPersonalId,IEP4.PoMOtherText,IEP4.PoMEliDeter,IEP4.PoMIEPDev,IEP4.PoMPlacement,IEP4.PoMInitEval,IEP4.PoMInit,IEP4.PoMReeval,IEP4.PoMAnnRev,IEP4.PoMOtherCheck FROM StudentPersonal JOIN StdtIEP ON StudentPersonal.StudentPersonalId=StdtIEP.StudentId INNER JOIN StdtIEPExt4 IEP4 ON IEP4.StdtIEPId=StdtIEP.StdtIEPId WHERE StdtIEP.SchoolId=" + SchoolId + " AND StdtIEP.StudentId=" + StudentId + " AND StdtIEP.StdtIEPId=" + IEPId + "";
            strQuery = "SELECT FORMAT(IEP4.AtndDate,'MM/dd/yyyy') AS AdmissionDate,StudentPersonal.LastName+',' +StudentPersonal.FirstName As Name,FORMAT(StudentPersonal.BirthDate,'MM/dd/yyyy') AS BirthDate,StudentPersonal.StudentPersonalId,IEP4.PoMOtherText,IEP4.PoMEliDeter,IEP4.PoMIEPDev,IEP4.PoMPlacement,IEP4.PoMInitEval,IEP4.PoMInit,IEP4.PoMReeval,IEP4.PoMAnnRev,IEP4.PoMOtherCheck FROM StudentPersonal JOIN StdtIEP ON StudentPersonal.StudentPersonalId=StdtIEP.StudentId INNER JOIN StdtIEPExt4 IEP4 ON IEP4.StdtIEPId=StdtIEP.StdtIEPId WHERE StdtIEP.SchoolId=" + SchoolId + " AND StdtIEP.StudentId=" + StudentId + " AND StdtIEP.StdtIEPId=" + IEPId + "";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[61];
            IEPCHK = new string[61];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (int j = 0; j < IEPC.Length; j++)
            {
                if (IEPC[j] == null)
                {
                    IEPC[j] = " ";
                }
            }

            strQuery = "SELECT IEP5.TMName,IEP5.TMRole,IEP5.InitialIfInAttn FROM StdtIEPExt5 IEP5 INNER JOIN StdtIEP IEP ON IEP.StdtIEPId=IEP5.StdtIEPId INNER JOIN StudentPersonal SP ON SP.StudentPersonalId=IEP.StudentId WHERE IEP.SchoolId=" + SchoolId + " AND IEP.StudentId=" + StudentId + "and IEP5.StdtIEPId=" + IEPId + "";
            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count < 16)
            {
                int c = 16 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(" ", " ", " ");
                }
            }

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
        for (int i = 0; i < IEPC.Length; i++)
        {
            if (IEPC[i] == null)
            {
                IEPC[i] = " ";
            }
        }


        C1 = new string[61];
        C2 = new string[61];
        if (IEPC != null) Array.Copy(IEPC, C1, 61);
        if (IEPC != null) Array.Copy(IEPCHK, C2, 61);

    }
    public void getIEP12(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT IEP4.SigRoleLEARep,FORMAT(IEP4.SigRep_date,'MM/dd/yyyy'),IEP4.ParntAccptIEP,IEP4.ParntRejctIEP,IEP4.ParntDontRejctIEP,IEP4.ParntDontRejctDesc,IEP4.ParntReqMeetig,IEP4.SigParnt,FORMAT(IEP4.SigParnt_date,'MM/dd/yyyy'),IEP4.ParntComnt FROM   StdtIEP IEP1 INNER JOIN StdtIEPExt4 IEP4 ON IEP1.StdtIEPId = IEP4.StdtIEPId WHERE  IEP1.StdtIEPId =" + IEPId + "AND  IEP1.SchoolId=" + SchoolId + " AND IEP1.StudentId=" + StudentId + "AND IEP1.StdtIEPId=" + IEPId + "";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    #endregion

    public void getIEP2(out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            int i = 0;
            objData = new clsData();
            Dt = new DataTable();
            strQuery = "SELECT IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr, IE1.EngLangInd, IE1.HistInd, IE1.TechInd, IE1.MathInd, IE1.OtherInd, IE1.OtherDesc, IE1.AffectDesc, IE1.AccomDesc, IE1.ContentModInd, ";
            strQuery += "IE1.ContentModDesc, IE1.MethodModInd, IE1.MethodModDesc,IE1.PerfModInd, IE1.PerfModDesc FROM  StdtIEPExt1 IE1 Inner Join StdtIEP IE ON IE.StdtIEPId=IE1.StdtIEPId ";
            strQuery += "Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId ";
            strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And IE1.StdtIEPId=" + IEPId + "  ";


            Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Count = Dt.Columns.Count;
                    IEPC = new string[Count];

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                        }
                    }

                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C2 = new string[Count];
        Array.Copy(IEPC, C2, Count);


    }

    public void getIEP3(out string[] C3, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;

            strQuery = "SELECT IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr, ";
            strQuery += "PEInd, TechDevicesInd, BehaviorInd, BrailleInd, CommInd, CommDfInd, ExtCurInd, LEPInd, NonAcdInd, SocialInd, TravelInd, VocInd, OtherInd, OtherDesc, ";
            strQuery += "AgeBand1Ind, AgeBand2Ind, AgeBand3Ind, AffectDesc,AccomDesc, ContentModInd, ContentModDesc,MethodModInd,MethodModDesc,PerfModInd, PerfModDesc FROM StdtIEPExt2 IE2 ";
            strQuery += "Inner Join StdtIEP IE ON IE.StdtIEPId=IE2.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.StudentId=IE.StudentId ";
            strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And IE2.StdtIEPId=" + IEPId + "  ";


            Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Count = Dt.Columns.Count;
                    IEPC = new string[Count];

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                        }
                    }

                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C3 = new string[Count];
        Array.Copy(IEPC, C3, Count);


    }

    //public DataTable ReturnRows(int numbr, DataTable dttemp)
    //{
    //    DataTable dt = new DataTable();
    //    dt = dttemp.Clone();

    //    try
    //    {
    //        for (int i = 0; i < dttemp.Rows.Count; i++)
    //        {
    //            if (i.ToString() == numbr.ToString())
    //            {
    //                DataRow dr2 = dt.NewRow();
    //                dr2 = dttemp.Rows[i];
    //                dt.Rows.Add(dr2.ItemArray);
    //                DataRow dr3 = dt.NewRow();
    //                dr3 = dttemp.Rows[i + 1];
    //                dt.Rows.Add(dr3.ItemArray);
    //            }

    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        ClsErrorLog errlog = new ClsErrorLog();
    //        errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
    //    }
    //    return dt;
    //}

    //public DataTable getIEP4Data(int StudentId, int SchoolId, int IEPId, out bool Odd)
    //{
    //    Odd = false;
    //    try
    //    {

    //        objData = new clsData();
    //        Dt = new DataTable();
    //        strQuery = "select distinct  Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId=" + IEPId + " and StdtLessonPlan.IncludeIEP=1";
    //        Dt = objData.ReturnDataTable(strQuery, false);
    //        string GoalIdZ = "";
    //        int countForGoalId = 0;
    //        foreach (DataRow dr in Dt.Rows)
    //        {
    //            if (countForGoalId == 0)
    //            {
    //                GoalIdZ += dr["Id"].ToString();
    //            }
    //            else if (countForGoalId > 0)
    //            {
    //                GoalIdZ += "," + dr["Id"].ToString();
    //            }
    //            countForGoalId++;
    //        }

    //        if (Dt.Rows.Count == 0)
    //        {
    //            GoalIdZ = "0";
    //        }
    //        //  strQuery = "select distinct Goal.GoalName as Name, Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId='" + sess.IEPId + "' and StdtLessonPlan.IncludeIEP=1";

    //        //strQuery = "Select CONVERT(VARCHAR(10),IE.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 101) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr,GL.GoalName,SG.Objective1, SG.Objective2 ,SG.Objective3 FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + "";
    //        //Select CONVERT(VARCHAR(10),IE.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),IE.EffEndDate, 101) as EndDate, S.StudentFname+' '+S.StudentLname AS Name, CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr,GL.GoalName,dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3 FROM StdtIEP IE Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtGoal SG on SG.StdtIEPId=IE.StdtIEPId Inner Join Goal GL on GL.GoalId=SG.GoalId inner join StdtLessonPlan on IE.StdtIEPId=StdtLessonPlan.StdtIEPId where Sc.SchoolId=1 And S.StudentId=2  And IE.StdtIEPId=2 and dbo.StdtLessonPlan.ActiveInd = 'A'
    //        //  strQuery = "select CONVERT(VARCHAR(10),StdtIEP.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),StdtIEP.EffEndDate, 101) as EndDate, Student.StudentFname+' '+Student.StudentLname AS Name, CONVERT(VARCHAR(10),Student.DOB, 101) AS DOB,Student.StudentNbr,Goal.GoalName,StdtLessonPlan.Objective1,StdtLessonPlan.Objective2,StdtLessonPlan.Objective3 from StdtLessonPlan inner join StdtIEP on StdtIEP.StdtIEPId=StdtLessonPlan.StdtIEPId inner join Student on Student.StudentId=StdtLessonPlan.StudentId inner join Goal on Goal.GoalId=StdtLessonPlan.GoalId where StdtLessonPlan.StudentId=" + StudentId + " and StdtLessonPlan.ActiveInd = 'A' and StdtLessonPlan.StdtIEPId=" + IEPId + " and StdtLessonPlan.SchoolId=" + SchoolId + "";

    //        strQuery = "SELECT CONVERT(VARCHAR(10),StdtIEP.EffStartDate, 101) AS StartDate,CONVERT(VARCHAR(10),StdtIEP.EffEndDate, 101) as EndDate," +
    //                    "Student.StudentFname+' '+Student.StudentLname AS Name, CONVERT(VARCHAR(10),Student.DOB, 101) AS DOB,Student.StudentNbr,StdtGoal.IEPGoalNo,Goal.GoalName," +
    //                       " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3" +
    //                      " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
    //                      "    dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId inner join StdtIEP on StdtIEP.StdtIEPId=StdtLessonPlan.StdtIEPId inner join Student on Student.StudentId=StdtLessonPlan.StudentId inner join StdtGoal on StdtIEP.StdtIEPId=StdtGoal.StdtIEPId" +
    //                    "  where StdtLessonPlan.GoalId in (" + GoalIdZ + ") AND dbo.StdtLessonPlan.StdtIEPId =  " + IEPId + "  AND  dbo.StdtLessonPlan.ActiveInd = 'A' and StdtLessonPlan.StudentId=" + StudentId + " and StdtLessonPlan.SchoolId=" + SchoolId + "   ORDER BY " +
    //                   "   dbo.StdtLessonPlan.GoalId ";
    //        Dt = objData.ReturnDataTable(strQuery, false);

    //        if (Dt.Rows.Count % 2 == 1)
    //        {
    //            Odd = true;
    //            Dt.Rows.Add(" ", " ", " ", " ", " ", null, " ", " ", " ", " ");
    //        }

    //    }
    //    catch (Exception Ex)
    //    {
    //        ClsErrorLog errlog = new ClsErrorLog();
    //        errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
    //    }
    //    return Dt;
    //}

    //public void getIEP4(out string[] C4, DataTable Dt)
    //{
    //    try
    //    {

    //        int i = 0;

    //        if (Dt != null)
    //        {
    //            if (Dt.Rows.Count > 0)
    //            {
    //                Count = Dt.Columns.Count;
    //                IEPC = new string[15];
    //                int k = 0;
    //                foreach (DataRow Dr in Dt.Rows)
    //                {
    //                    if (k < 10)
    //                    {
    //                        for (i = 0; i < Count; i++)
    //                        {
    //                            IEPC[k] = Dr[i].ToString();
    //                            k++;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        k = 10;
    //                        for (i = 5; i < Count; i++)
    //                        {
    //                            IEPC[k] = Dr[i].ToString();
    //                            k++;
    //                        }
    //                    }
    //                }

    //            }
    //        }

    //    }
    //    catch (Exception Ex)
    //    {
    //        ClsErrorLog errlog = new ClsErrorLog();
    //        errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
    //    }

    //    C4 = new string[15];
    //    Array.Copy(IEPC, C4, 15);

    //}


    public void getIEP5(out string[] C5, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT DISTINCT Convert(varchar(50),IEP.EffStartDate,101) as StartDate,Convert(varchar(50),IEP.EffEndDate,101)  as EndDate, S.StudentLname+' , '+S.StudentFname AS Name, ";
            //strQuery += " CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr,IE2.OtherDesc1,'False','True','False','True' FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId INNER JOIN StdtIEPExt2 ";
            //strQuery += " IE2 ON IE2.StdtIEPId=IEP.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " ";


            strQuery = " select Convert(varchar(50),StdtIEP.EffStartDate,101) as StartDate,Convert(varchar(50),StdtIEP.EffEndDate,101)  as EndDate,";
            strQuery += " Student.StudentLname+' , '+Student.StudentFname AS Name,  CONVERT(VARCHAR(10),Student.DOB, 101) AS DOB, Student.StudentNbr,";
            strQuery += " StdtIEPExt2.OtherDesc1,StdtIEPExt2.[5DayInd],StdtIEPExt2.[6DayInd],StdtIEPExt2.[10DayInd],StdtIEPExt2.DayOther,''";
            strQuery += " from StdtIEP inner join Student on Student.StudentId=StdtIEP.StudentId ";
            strQuery += " inner join StdtIEPExt2 on StdtIEPExt2.StdtIEPId=StdtIEP.StdtIEPId ";
            strQuery += " where StdtIEP.StdtIEPId=" + IEPId + " AND StdtIEP.SchoolId =" + SchoolId + " AND StdtIEP.StudentId = " + StudentId + " ";


            Dt = objData.ReturnDataTable(strQuery, false);

            IEPC = new string[119];

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (i = 0; i < Count; i++)
                    {
                        IEPC[i] = Dr[i].ToString();
                    }
                }

            }


            //strQuery = "SELECT G.GoalName as GoalName,  SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate";
            //strQuery += " FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  Inner Join Goal G On G.GoalId=SGS.StdtGoalId ";
            //strQuery += " WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " and SGS.SvcDelTyp='A'";


            strQuery = "select (select IEPGoalNo from StdtGoal where StdtGoalId=StdtGoalSvc.StdtGoalId) as 'GoalName',";
            strQuery += "StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc, StdtGoalSvc.FreqDurDesc,convert(varchar(50), StdtGoalSvc.StartDate,101) as StartDate,";
            strQuery += "convert(varchar(50),StdtGoalSvc.EndDate,101) as EndDate ";
            strQuery += "from StdtGoalSvc  inner join ";
            strQuery += "StdtIEP on StdtIEP.StdtIEPId=StdtGoalSvc.StdtIEPId  where StdtIEP.StdtIEPId=" + IEPId + " ";
            strQuery += "and StdtIEP.StudentId=" + StudentId + " and StdtIEP.SchoolId=" + SchoolId + " and StdtGoalSvc.SvcDelTyp='A'";

            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ", " ");
                }
            }


            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }



            //strQuery = "SELECT G.GoalName as GoalName,  SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate";
            //strQuery += " FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  Inner Join Goal G On G.GoalId=SGS.StdtGoalId ";
            //strQuery += " WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " and SGS.SvcDelTyp='B'";

            strQuery = "select (select IEPGoalNo from StdtGoal where StdtGoalId=StdtGoalSvc.StdtGoalId) as 'GoalName',";
            strQuery += "StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc, StdtGoalSvc.FreqDurDesc,convert(varchar(50), StdtGoalSvc.StartDate,101) as StartDate,";
            strQuery += "convert(varchar(50),StdtGoalSvc.EndDate,101) as EndDate ";
            strQuery += "from StdtGoalSvc  inner join ";
            strQuery += "StdtIEP on StdtIEP.StdtIEPId=StdtGoalSvc.StdtIEPId  where StdtIEP.StdtIEPId=" + IEPId + " ";
            strQuery += "and StdtIEP.StudentId=" + StudentId + " and StdtIEP.SchoolId=" + SchoolId + " and StdtGoalSvc.SvcDelTyp='B'";


            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count < 6)
            {
                int c = 6 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ", " ");
                }
            }

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }



            //strQuery = "SELECT G.GoalName as GoalName,  SGS.SvcTypDesc, SGS.PersonalTypDesc, SGS.FreqDurDesc,convert(varchar(50), SGS.StartDate,101) as StartDate,convert(varchar(50),SGS.EndDate,101) as EndDate";
            //strQuery += " FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  Inner Join Goal G On G.GoalId=SGS.StdtGoalId ";
            //strQuery += " WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " and SGS.SvcDelTyp='C'";
            strQuery = "select (select IEPGoalNo from StdtGoal where StdtGoalId=StdtGoalSvc.StdtGoalId) as 'GoalName',";
            strQuery += "StdtGoalSvc.SvcTypDesc,StdtGoalSvc.PersonalTypDesc, StdtGoalSvc.FreqDurDesc,convert(varchar(50), StdtGoalSvc.StartDate,101) as StartDate,";
            strQuery += "convert(varchar(50),StdtGoalSvc.EndDate,101) as EndDate ";
            strQuery += "from StdtGoalSvc  inner join ";
            strQuery += "StdtIEP on StdtIEP.StdtIEPId=StdtGoalSvc.StdtIEPId  where StdtIEP.StdtIEPId=" + IEPId + " ";
            strQuery += "and StdtIEP.StudentId=" + StudentId + " and StdtIEP.SchoolId=" + SchoolId + " and StdtGoalSvc.SvcDelTyp='C'";

            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count < 7)
            {
                int c = 7 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ", " ");
                }
            }

            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }

            }



        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }

        for (int i = 0; i < IEPC.Length; i++)
        {
            if (IEPC[i] == null)
            {
                IEPC[i] = " ";
            }
        }

        C5 = new string[119];
        Array.Copy(IEPC, C5, 119);

    }


    public void getIEP6(out string[] C6, int StudentId, int SchoolId, int IEPId, int StatusId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "Select IEP.EffStartDate as StartDate,IEP.EffEndDate as EndDate, St1.StudentFname+','+St1.StudentLname AS Name,  CONVERT(VARCHAR(10),St1.DOB, 101) AS DOB,St1.StudentNbr, ";
            strQuery += "IEP3.RemovedInd1,IEP3.RemovedInd2,IEP3.RemovedDesc,IEP3.ShorterCd1,IEP3.ShorterCd2,IEP3.ShorterCd3,IEP3.LongerCd1,IEP3.LongerCd2,IEP3.LongerCd3,IEP3.SchedModDesc,IEP3.TransportInd1,IEP3.TransportInd2,IEP3.RegTransInd,IEP3.RegTransDesc,IEP3.SpTransInd ,";
            strQuery += "IEP3.SpTransDesc from StdtIEPExt3 IEP3  INNER JOIN StdtIEP IEP  ON IEP.StdtIEPId  =IEP3.StdtIEPId   ";
            strQuery += "Inner Join School Sc On IEP.SchoolId=Sc.SchoolId Inner Join Student St1 On IEP.StudentId=St1.StudentId ";
            strQuery += "where IEP.SchoolId=" + SchoolId + " And IEP.StudentId=" + StudentId + " And IEP.StdtIEPId=" + IEPId + "";

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];

            if (Dt.Rows.Count > 0)
            {


                foreach (DataRow Dr in Dt.Rows)
                {
                    for (i = 0; i < Count; i++)
                    {
                        IEPC[i] = Dr[i].ToString();
                    }
                }

            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }

        for (int i = 0; i < IEPC.Length; i++)
        {
            if (IEPC[i] == null)
            {
                IEPC[i] = " ";
            }
        }

        C6 = new string[Count];
        Array.Copy(IEPC, C6, Count);

    }



    public void getIEP7(out string[] C7, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "";

            strQuery = "Select IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr, ";
            strQuery += "AsmntPlanned,EngCol1,EngCol2,EngCol3,HistCol1,HistCol2 ,HistCol3,MathCol1,MathCol2,MathCol3 ,TechCol1,TechCol2,TechCol3,ReadCol1,ReadCol1,ReadCol1,InfoCol2,InfoCol3 ";
            strQuery += "from StdtIEPExt3 E3 Inner Join StdtIEP IE ON IE.StdtIEPId=IE.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId ";
            strQuery += "Inner Join StdtIEP St1 On S.StudentId=St1.StudentId where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + " ";


            Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;
                IEPC = new string[Count];

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (i = 0; i < Count; i++)
                    {
                        IEPC[i] = Dr[i].ToString();
                    }
                }

            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C7 = new string[Count];
        Array.Copy(IEPC, C7, Count);

    }



    public void getIEP8(out string[] C8, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "";


            strQuery = "Select IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+','+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr, ";
            strQuery += "AddInfoCol1,AddInfoCol2,AddInfoCol3,AddInfoCol3Desc from StdtIEPExt3 IE3 Inner Join StdtIEP IE ON IE.StdtIEPId=IE3.StdtIEPId Inner Join School Sc On ";
            strQuery += "IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId  ";
            strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + "  ";



            Dt = objData.ReturnDataTable(strQuery, false);


            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;
                IEPC = new string[Count];

                foreach (DataRow Dr in Dt.Rows)
                {
                    for (i = 0; i < Count; i++)
                    {
                        IEPC[i] = Dr[i].ToString();
                    }
                }

            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C8 = new string[Count];
        Array.Copy(IEPC, C8, Count);

    }



    //------------------------------------ IEP Binding For PA ------------------------------------------------

    public void getIEP_PE1(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "Select  ST.StudentNbr AS IDNO, ST.StudentLname+','+ST.StudentFname as Name,ST.DOB AS DOB,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel ";
            strQuery += "WHERE StudentPersonalId=" + StudentId + " AND ContactSequence=1)) AS Phone,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + StudentId + " AND ContactSequence=2)) AS Mobile, ";
            strQuery += "ADR.ApartmentType+','+ADR.StreetName+','+ADR.City AS Address,ST.GradeLevel AS Grade,CONVERT(VARCHAR(101),SIEP.IepTeamMeetingDate, 101),CONVERT(VARCHAR(101),SIEP.IepImplementationDate, 101),SIEP.AnticipatedDurationofServices,SIEP.LocalEducationAgency, ";
            strQuery += "CONVERT(VARCHAR(101),SIEP.AnticipatedYearOfGraduation, 101), SIEP.CountyOfResidance, SIEP.DocumentedBy,SIEP.OtherInformation ";
            strQuery += "from Student ST Inner Join StudentAddresRel SAR ON ST.StudentId=SAR.StudentPersonalId INNER JOIN StdtIEP_PE SIEP ON ST.StudentId=SIEP.StudentId ";
            strQuery += "Inner Join AddressList ADR ON SAR.AddressId=ADR.AddressId Where ST.StudentId=" + StudentId + " And ST.SchoolId=" + SchoolId + " And SAR.ContactSequence=0 AND SIEP.StdtIEP_PEId=" + IEPId + "";
            //strQuery = "SELECT S.StudentNbr AS IDNO, S.StudentFname+' '+S.StudentLname AS Name,  S.DOB AS DOB,Ad.HomePhone,Ad.Mobile, ";
            //strQuery += "Ad.AddressLine1+','+Ad.AddressLine2+','+Ad.AddressLine3 as Address, S.GradeLevel as Grade, ";
            //strQuery += "CONVERT(VARCHAR(101),St1.IepTeamMeetingDate, 101),CONVERT(VARCHAR(101),St1.IepImplementationDate, 101),St1.AnticipatedDurationofServices," +
            //"St1.LocalEducationAgency,CONVERT(VARCHAR(101),St1.AnticipatedYearOfGraduation, 101), St1.CountyOfResidance, St1.DocumentedBy,St1.OtherInformation FROM  Student S  Inner Join" +
            //" School Sc On s.SchoolId=Sc.SchoolId Inner Join Address Ad ";
            //strQuery += "on Ad.AddressId=Sc.DistAddrId Inner Join StdtIEP_PE St1 On S.StudentId=St1.StudentId where S.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And St1.StdtIEP_PEId=" + IEPId + " ";
            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count + 1];
            IEPCHK = new string[Count + 1];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "true") || (Dr[i].ToString() == "false"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count + 1];
        C2 = new string[Count + 1];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE2(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+' '+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr, IE1.EngLangInd, IE1.HistInd, IE1.TechInd, IE1.MathInd, IE1.OtherInd, IE1.OtherDesc, IE1.AffectDesc, IE1.AccomDesc, IE1.ContentModInd, ";
            //strQuery += "IE1.ContentModDesc, IE1.MethodModInd, IE1.MethodModDesc,IE1.PerfModInd, IE1.PerfModDesc FROM  StdtIEPExt1 IE1 Inner Join StdtIEP IE ON IE.StdtIEPId=IE1.StdtIEPId ";
            //strQuery += "Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId ";
            //strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And IE1.StdtIEPId=" + IEPId + "  ";


            strQuery = "SELECT CONVERT(VARCHAR(101),IE1.DateOfRevisions, 101), IE1.Participants, IE1.IEPSections FROM  IEPPA1Extension IE1 Inner Join StdtIEP_PE IE ";
            strQuery += "ON IE.StdtIEP_PEId=IE1.IepPAId Inner Join School Sc On IE.SchoolId=Sc.SchoolId ";
            strQuery += "Inner Join Student S on S.SchoolId=Sc.SchoolId where Sc.SchoolId=" + SchoolId + " And ";
            strQuery += "S.StudentId=" + StudentId + " And IE.StdtIEP_PEId=" + IEPId + "  ";


            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, "", "");
                }
            }

            Count = Dt.Columns.Count;
            IEPC = new string[15];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                            {

                                IEPCHK[index] = Dr[j].ToString();
                                index++;
                            }
                            i++;
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[15];
        C2 = new string[3];
        if (IEPC != null) Array.Copy(IEPC, C1, 15);
        if (IEPC != null) Array.Copy(IEPCHK, C2, 3);

    }

    public void getIEP_PE3(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+' '+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr, ";
            //strQuery += "PEInd, TechDevicesInd, BehaviorInd, BrailleInd, CommInd, CommDfInd, ExtCurInd, LEPInd, NonAcdInd, SocialInd, TravelInd, VocInd, OtherInd, OtherDesc, ";
            //strQuery += "AgeBand1Ind, AgeBand2Ind, AgeBand3Ind, AffectDesc,AccomDesc, ContentModInd, ContentModDesc,MethodModInd,MethodModDesc,PerfModInd, PerfModDesc FROM StdtIEPExt2 IE2 ";
            //strQuery += "Inner Join StdtIEP IE ON IE.StdtIEPId=IE2.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.StudentId=IE.StudentId ";
            //strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And IE2.StdtIEPId=" + IEPId + "  ";

            strQuery = "SELECT ParentName,ParentSing,StudentName,StudentSign,RegEduTeacherName," +
                       "regEduTeacherSign,SpclEduTeacherName,SpclEduTeacherSign,LocalEdAgencyName,localEdAgencySign," +
                       " CareerEdRepName,careerEdRepSign,CommunityAgencyName,CommunityAgencySign,TeacherGiftedName," +
                       " TeacherGiftedSign,WittenInput FROM dbo.IEP_PE2_Team WHERE StdtIEP_PEId=" + IEPId;

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                            {

                                IEPCHK[index] = Dr[j].ToString();
                                index++;
                            }
                            i++;
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE4(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+' '+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB,S.StudentNbr, ";
            //strQuery += "PEInd, TechDevicesInd, BehaviorInd, BrailleInd, CommInd, CommDfInd, ExtCurInd, LEPInd, NonAcdInd, SocialInd, TravelInd, VocInd, OtherInd, OtherDesc, ";
            //strQuery += "AgeBand1Ind, AgeBand2Ind, AgeBand3Ind, AffectDesc,AccomDesc, ContentModInd, ContentModDesc,MethodModInd,MethodModDesc,PerfModInd, PerfModDesc FROM StdtIEPExt2 IE2 ";
            //strQuery += "Inner Join StdtIEP IE ON IE.StdtIEPId=IE2.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.StudentId=IE.StudentId ";
            //strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + " And IE2.StdtIEPId=" + IEPId + "  ";
            strQuery = "SELECT IEP3ParentSign FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);


            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE5(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "SELECT DISTINCT Convert(varchar(50),IEP.EffStartDate,101) as StartDate,Convert(varchar(50),IEP.EffEndDate,101)  as EndDate, S.StudentLname+' , '+S.StudentFname AS Name, ";
            //strQuery += " CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr,IE2.OtherDesc1,'False','True','False','True' FROM  StdtIEP IEP INNER JOIN  StdtGoalSvc SGS ON IEP.StdtIEPId = SGS.StdtIEPId INNER JOIN StdtIEPExt2 ";
            //strQuery += " IE2 ON IE2.StdtIEPId=IEP.StdtIEPId Inner Join Student S ON S.StudentId=IEP.StudentId  WHERE     IEP.StdtIEPId = " + IEPId + " AND IEP.SchoolId =" + SchoolId + " AND IEP.StudentId = " + StudentId + " ";


            //strQuery = "SELECT IEP4IsBlind,IEP4IsNotBlind,IEP4Isdeaf,IEP4IsNotdeaf,IEP4CommNeeded,IEP4NotCommNeeded,IEP4AssistiveTechNeeded,IEP4NotAssistiveTechNeeded,IEP4EnglishProficiency,IEP4NotEnglishProficiency,IEP4ImpedeLearning,IEP4NotImpedeLearning FROM IEP_PE_Details"
            //    + " WHERE StdtIEP_PEId=" + IEPId;

            strQuery = " Select " +
   " CASE IEP4IsBlind   WHEN 1 THEN 'True' WHEN 0 THEN 'False'     ELSE 'False'   END as 'IsBlind',CASE IEP4IsBlind   WHEN 1 THEN 'False' WHEN 0 THEN 'True'     ELSE 'False'   END as 'IsNotBlind'," +
   "CASE IEP4Isdeaf   WHEN 1 THEN 'True' WHEN 0 THEN 'False'     ELSE 'False'   END as 'IEP4Isdeaf',CASE IEP4Isdeaf   WHEN 1 THEN 'False' WHEN 0 THEN 'True'     ELSE 'False'   END as 'IEP4IsNotdeaf'," +
   "CASE IEP4CommNeeded   WHEN 1 THEN 'True' WHEN 0 THEN 'False'     ELSE 'False'   END as 'IEP4CommNeeded',CASE IEP4CommNeeded   WHEN 1 THEN 'False' WHEN 0 THEN 'True'     ELSE 'False'   END as 'IEP4NotCommNeeded'," +
   "CASE IEP4AssistiveTechNeeded   WHEN 1 THEN 'True' WHEN 0 THEN 'False'     ELSE 'False'   END as 'IEP4AssistiveTechNeeded',CASE IEP4AssistiveTechNeeded   WHEN 1 THEN 'False' WHEN 0 THEN 'True'     ELSE 'False'   END as 'IEP4NotAssistiveTechNeeded'," +
   "CASE IEP4EnglishProficiency   WHEN 1 THEN 'True' WHEN 0 THEN 'False'     ELSE 'False'   END as 'IEP4EnglishProficiency',CASE IEP4EnglishProficiency   WHEN 1 THEN 'False' WHEN 0 THEN 'True'     ELSE 'False'   END as 'IEP4NotEnglishProficiency'," +
   "CASE IEP4ImpedeLearning   WHEN 1 THEN 'True' WHEN 0 THEN 'False'     ELSE 'False'   END as 'IEP4ImpedeLearning',CASE IEP4ImpedeLearning   WHEN 1 THEN 'False' WHEN 0 THEN 'True'     ELSE 'False'   END as 'IEP4NotImpedeLearning'" +
   "FROM IEP_PE_Details WHERE StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);


            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE6(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId, int StatusId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT IEP5CIPCode,IEP5OtherSpecify,IEP5Disability FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + IEPId;

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                            {

                                IEPCHK[index] = Dr[j].ToString();
                                index++;
                            }
                            i++;
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE7(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;

            strQuery = "select IEP6TrainingGoal,IEP6TrainCoursesStudy,IEP6EmploymentGoal,IEP6EmpCoursesStudy,IEP6LivingGoal,IEP6LivingCoursesStudy,IEP6MeasurableCheck1, "
               + "IEP6MeasurableCheck2,IEP6MeasurableCheck3 from dbo.IEP_PE_Details where StdtIEP_PEId=" + IEPId;

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[99];
            IEPCHK = new string[3];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                            {
                                IEPCHK[index] = Dr[j].ToString();
                                index++;
                            }
                            i++;
                        }
                    }

                }
            }
            //Training Goal
            Dt = new DataTable();
            strQuery = "";

            strQuery = "SELECT Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Edu WHERE StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(" ", " ", " ", " ", " ", " ");
                }
            }
            Count = Dt.Columns.Count;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();

                            i++;
                        }
                    }

                }
            }
            //Educational Goal 
            Dt = new DataTable();
            strQuery = "";

            strQuery = "SELECT Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Goal WHERE StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(" ", " ", " ", " ", " ", " ");
                }
            }
            Count = Dt.Columns.Count;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();

                            i++;
                        }
                    }

                }
            }
            //Living Goal
            Dt = new DataTable();
            strQuery = "";
            strQuery = "SELECT Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Living WHERE StdtIEP_PEId=" + IEPId;

            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(" ", " ", " ", " ", " ", " ");
                }
            }
            Count = Dt.Columns.Count;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();

                            i++;
                        }
                    }

                }
            }

            //Set the null value fields
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[99];
        C2 = new string[3];
        if (IEPC != null) Array.Copy(IEPC, C1, 99);
        if (IEPC != null) Array.Copy(IEPCHK, C2, 3);

    }

    public void getIEP_PE8(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        string[] IEPCHKB = new string[20];
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "Select IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+' '+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr, ";
            //strQuery += "AddInfoCol1,AddInfoCol2,AddInfoCol3,AddInfoCol3Desc from StdtIEPExt3 IE3 Inner Join StdtIEP IE ON IE.StdtIEPId=IE3.StdtIEPId Inner Join School Sc On ";
            //strQuery += "IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId Inner Join StdtIEP St1 On S.StudentId=St1.StudentId  ";
            //strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + "  ";

            strQuery = "SELECT IEP8PSSAReading,IEP8PSSAappropriate,";
            strQuery += "IEP8AsmtNotAdministred,IEP8ReadPartcptPSSAWithoutAcmdtn,IEP8ReadPartcptPSSAWithFollowingAcmdtn,";
            strQuery += "IEP8ReadPartcptPSSAModiWithoutAcmdtn,IEP8ReadPartcptPSSAModiWithFollowingAcmdtn,IEP8MathPartcptPSSAWithoutAcmdtn,IEP8MathPartcptPSSAWithFollowingAcmdtn";
            strQuery += ",IEP8MathPartcptPSSAModiWithoutAcmdtn,IEP8MathPartcptPSSAModiWithFollowingAcmdtn,IEP8SciencePartcptPSSAWithoutAcmdtn,IEP8SciencePartcptPSSAWithFollowingAcmdtn,IEP8SciencePartcptPSSAModiWithoutAcmdtn,";
            strQuery += "IEP8SciencePartcptPSSAModiWithFollowingAcmdtn";
            strQuery += ",IEP8WritePartcptPSSAWithoutAcmdtn";
            strQuery += ",IEP8WritePartcptPSSAWithFollowingAcmdtn,IEP8Videotape,IEP8WrittenNarrative,IEP8PSSAParticipate";
            strQuery += " from dbo.IEP_PE_Details where StdtIEP_PEId=" + IEPId;

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];

            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                            {

                                IEPCHKB[i] = Dr[j].ToString();
                                index++;
                            }
                            i++;
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[20];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHKB, C2, 20);

    }

    //public void getIEP_PE9(out string[] C5, out string[] C2, int StudentId, int SchoolId, int IEPId)
    //{
    //    try
    //    {
    //        objData = new clsData();
    //        Dt = new DataTable();
    //        int i = 0;

    //        string strQuery = "SELECT Id,SDI,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE11_SDI"
    //           + " WHERE StdtIEP_PEId=" + IEPId;


    //        IEPC = new string[119];

    //        Dt = objData.ReturnDataTable(strQuery, false);
    //        if (Dt.Rows.Count < 3)
    //        {
    //            int c = 5 - Dt.Rows.Count;
    //            for (int h = 0; h < c; h++)
    //            {
    //                Dt.Rows.Add(null, " ", " ", " ", " ", " ");
    //            }
    //        }


    //        if (Dt.Rows.Count > 0)
    //        {
    //            Count = Dt.Columns.Count;

    //            foreach (DataRow Dr in Dt.Rows)
    //            {
    //                for (int j = 0; j < Count; j++)
    //                {
    //                    IEPC[i] = Dr[j].ToString();
    //                    i++;
    //                }
    //            }

    //        }


    //        strQuery = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE11_Service"
    //           + " WHERE StdtIEP_PEId=" + IEPId;

    //        Dt = objData.ReturnDataTable(strQuery, false);

    //        if (Dt.Rows.Count < 5)
    //        {
    //            int c = 6 - Dt.Rows.Count;
    //            for (int h = 0; h < c; h++)
    //            {
    //                Dt.Rows.Add(null, " ", " ", " ", " ", " ");
    //            }
    //        }

    //        if (Dt.Rows.Count > 0)
    //        {
    //            Count = Dt.Columns.Count;

    //            foreach (DataRow Dr in Dt.Rows)
    //            {
    //                for (int j = 0; j < Count; j++)
    //                {
    //                    IEPC[i] = Dr[j].ToString();
    //                    i++;
    //                }
    //            }

    //        }



    //        strQuery = "SELECT Id,SchoolPerson,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE11_SchoolPer"
    //         + " WHERE StdtIEP_PEId=" + IEPId;


    //        Dt = objData.ReturnDataTable(strQuery, false);

    //        if (Dt.Rows.Count < 2)
    //        {
    //            int c = 7 - Dt.Rows.Count;
    //            for (int h = 0; h < c; h++)
    //            {
    //                Dt.Rows.Add(null, " ", " ", " ", " ", " ");
    //            }
    //        }

    //        if (Dt.Rows.Count > 0)
    //        {
    //            Count = Dt.Columns.Count;

    //            foreach (DataRow Dr in Dt.Rows)
    //            {
    //                for (int j = 0; j < Count; j++)
    //                {
    //                    IEPC[i] = Dr[j].ToString();
    //                    i++;
    //                }
    //            }

    //        }



    //    }
    //    catch (Exception Ex)
    //    {
    //        ClsErrorLog errlog = new ClsErrorLog();
    //        errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
    //    }

    //    for (int i = 0; i < IEPC.Length; i++)
    //    {
    //        if (IEPC[i] == null)
    //        {
    //            IEPC[i] = " ";
    //        }
    //    }

    //    C5 = new string[119];
    //    Array.Copy(IEPC, C5, 119);
    //    C2 = new string[119];
    //    Array.Copy(IEPC, C2, 119);

    //}

    public void getIEP_PE10(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId, int StatusId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "Select IEP.EffStartDate as StartDate,IEP.EffEndDate as EndDate, St1.StudentFname+' '+St1.StudentLname AS Name,  CONVERT(VARCHAR(10),St1.DOB, 101) AS DOB,St1.StudentNbr, ";
            //strQuery += "IEP3.RemovedInd1,IEP3.RemovedInd2,IEP3.RemovedDesc,IEP3.ShorterCd1,IEP3.ShorterCd2,IEP3.ShorterCd3,IEP3.LongerCd1,IEP3.LongerCd2,IEP3.LongerCd3,IEP3.SchedModDesc,IEP3.TransportInd1,IEP3.TransportInd2,IEP3.RegTransInd,IEP3.RegTransDesc,IEP3.SpTransInd ,";
            //strQuery += "IEP3.SpTransDesc from StdtIEPExt3 IEP3  INNER JOIN StdtIEP IEP  ON IEP.StdtIEPId  =IEP3.StdtIEPId   ";
            //strQuery += "Inner Join School Sc On IEP.SchoolId=Sc.SchoolId Inner Join Student St1 On IEP.StudentId=St1.StudentId ";
            //strQuery += "where IEP.SchoolId=" + SchoolId + " And IEP.StudentId=" + StudentId + " And IEP.StdtIEPId=" + IEPId + "";

            strQuery = "SELECT IEP9NoRegAsmt,IEP8AssmtAcc,IEP9AssmtAppropriate, IEP8AsmtStdtGrade,IEP8AsmtWithoutAcc,IEP8AsmtWithAcc,IEP9AlernativeAsmt" +
               " from dbo.IEP_PE_Details where StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE11(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            //strQuery = "Select IE.EffStartDate as StartDate,IE.EffEndDate as EndDate, S.StudentFname+' '+S.StudentLname AS Name,  CONVERT(VARCHAR(10),S.DOB, 101) AS DOB, S.StudentNbr, ";
            //strQuery += "AsmntPlanned,EngCol1,EngCol2,EngCol3,HistCol1,HistCol2 ,HistCol3,MathCol1,MathCol2,MathCol3 ,TechCol1,TechCol2,TechCol3,ReadCol1,ReadCol2,ReadCol3,InfoCol2,InfoCol3 ";
            //strQuery += "from StdtIEPExt3 E3 Inner Join StdtIEP IE ON IE.StdtIEPId=E3.StdtIEPId Inner Join School Sc On IE.SchoolId=Sc.SchoolId Inner Join Student S on S.SchoolId=Sc.SchoolId ";
            //strQuery += "where Sc.SchoolId=" + SchoolId + " And S.StudentId=" + StudentId + "  And IE.StdtIEPId=" + IEPId + " ";

            string strQuery = "select [MeasureAnualGoal],[StudentsProgress],[DescReportProgress],[ReportProgress] from [dbo].[StdtIEP_PE10_GoalsObj] "
                            + "where [StdtIEP_PEId]=" + IEPId;


            Dt = objData.ReturnDataTable(strQuery, false);

            if (Dt.Rows.Count != 5)
            {
                if (Dt.Rows.Count > 0 && Dt.Rows.Count < 5)
                {
                    int c = 5 - Dt.Rows.Count;
                    for (int h = 0; h < c; h++)
                    {
                        Dt.Rows.Add("", "", "", "");
                    }
                }
            }

            string getStdGoalSvcDetails = "SELECT Benchmark FROM dbo.IEP_PE10_Benchmark WHERE StdIEP_PEId=" + IEPId;

            Dt2 = objData.ReturnDataTable(getStdGoalSvcDetails, false);

            if (Dt2.Rows.Count != 5)
            {
                if (Dt2.Rows.Count > 5)
                {

                }
                else
                {
                    int c = 5 - Dt2.Rows.Count;
                    for (int h = 0; h < c; h++)
                    {
                        Dt2.Rows.Add("");
                    }
                }
            }

            Count = Dt.Columns.Count;
            IEPC = new string[25];

            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                            {

                                index++;
                            }
                            i++;
                        }
                    }

                }
            }



            Count = Dt2.Columns.Count;

            index = 0;
            int xcount = 0;
            if (Dt2 != null)
            {
                if (Dt2.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt2.Rows)
                    {
                        if (xcount < 5)
                        {
                            for (int j = 0; j < Count; j++)
                            {
                                IEPC[i] = Dr[j].ToString();
                                if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                                {

                                    index++;
                                }
                                i++;
                            }
                            xcount++;
                        }
                    }

                }
            }


            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[25];
        C2 = new string[0];
        if (IEPC != null) Array.Copy(IEPC, C1, 25);


    }

    public void getIEP_PE12(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {
            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            string strQuery = "SELECT SDI,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur FROM dbo.IEP_PE11_SDI"
               + " WHERE StdtIEP_PEId=" + IEPId;
            IEPC = new string[52];
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 3)
            {
                int c = 3 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ");
                }
            }
            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;
                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }
            }
            strQuery = "SELECT Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur FROM dbo.IEP_PE11_Service"
               + " WHERE StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ");
                }
            }
            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;
                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }
            }
            strQuery = "SELECT SchoolPerson,Person,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur FROM dbo.IEP_PE11_SchoolPer"
             + " WHERE StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 2)
            {
                int c = 2 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(null, " ", " ", " ", " ", " ");
                }
            }
            if (Dt.Rows.Count > 0)
            {
                Count = Dt.Columns.Count;
                foreach (DataRow Dr in Dt.Rows)
                {
                    for (int j = 0; j < Count; j++)
                    {
                        IEPC[i] = Dr[j].ToString();
                        i++;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }
        for (int i = 0; i < IEPC.Length; i++)
        {
            if (IEPC[i] == null)
            {
                IEPC[i] = " ";
            }
        }
        C1 = new string[52];
        Array.Copy(IEPC, C1, 52);
        C2 = new string[52];
        Array.Copy(IEPC, C2, 52);

    }

    public void getIEP_PE13(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        int Counts = 0;
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            IEPC = new string[33];
            IEPCHK = new string[2];
            int index = 0;

            //Support Service
            string strQuery = "SELECT SupportService FROM dbo.IEP_PE12_SupportService WHERE StdIEP_PEId=" + IEPId;

            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 3)
            {
                int c = 3 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(" ");
                }
            }
            Count = Dt.Columns.Count;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();

                            i++;
                        }
                    }

                }
            }

            Dt = new DataTable();
            strQuery = "";
            strQuery = "SELECT IEP12ElegibleForESY,IEP12NotElegibleForESY,IEP12ElegibleForESYInfo,IEP12NotElegibleForESYInfo,IEP12ShortTermObjectives FROM IEP_PE_Details WHERE StdtIEP_PEId=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);

            Counts = Dt.Columns.Count;

            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Counts; j++)
                        {
                            IEPC[i] = Dr[j].ToString();
                            if ((Dr[j].ToString() == "True") || (Dr[j].ToString() == "False"))
                            {
                                IEPCHK[index] = Dr[j].ToString();
                                index++;
                            }
                            i++;
                        }
                    }

                }
            }

            //ESY grid
            Dt = new DataTable();
            strQuery = "";
            strQuery = "select [ESY],[Location],[Frequency],Convert(varchar,PrjBeginning,101)as PrjBeginning,[AnticipatedDur] from [dbo].[IEP_PE12_ESY] where [StdtIEP_PEId]=" + IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count < 5)
            {
                int c = 5 - Dt.Rows.Count;
                for (int h = 0; h < c; h++)
                {
                    Dt.Rows.Add(" ", " ", " ", null, " ");
                }
            }

            Count = Dt.Columns.Count;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (int j = 0; j < Count; j++)
                        {
                            IEPC[i] = Dr[j].ToString();

                            i++;
                        }
                    }

                }
            }

            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[33];
        C2 = new string[Counts];
        if (IEPC != null) Array.Copy(IEPC, C1, 33);
        if (IEPC != null) Array.Copy(IEPCHK, C2, 2);

    }

    public void getIEP_PE14(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT IEP13RegularEdu,IEP13GeneralEdu FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + IEPId;

            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE15(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT IEP14Itinerant,IEP14Supplemental,IEP14FullTime,IEP14AutisticSupport,IEP14Blind,IEP14Deaf,IEP14Emotional,IEP14Learning,IEP14LifeSkills, " +
                          "IEP14MultipleDisabilities,IEP14Physical,IEP14Speech,IEP14SchoolDistrict,IEP14SchoolBuilding,IEP14OtherDesc,IEP14IsNeighbour,IEP14IsNeibhourNo,IEP14SpclEdu," +
                          "IEP14Other FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + IEPId;


            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }

    public void getIEP_PE16(out string[] C1, out string[] C2, int StudentId, int SchoolId, int IEPId)
    {
        try
        {

            objData = new clsData();
            Dt = new DataTable();
            int i = 0;
            strQuery = "SELECT IEP15RegularCls80,IEP15Regular79,IEP15Regular40,IEP15ApprovePrivateSchool,IEP15ApprovePrivateText,IEP15OtherPublic,IEP15OtherPublicText," +
                      "IEP15ApproveResidential,IEP15ApproveResidentialText, " +
                         "IEP15Hospital,IEP15HospitalText,IEP15PrivateFacility,IEP15PrivateFacilityText,IEP15CorrectionalFacility,IEP15CorrectionText," +
                          " IEP15PrivateResi,IEP15PrivateResText,IEP15ChkoutState,IEP15ChkoutStateText,IEP15PublicFacility,IEP15PublicFacilityText," +
                          "IEP15InstructionConducted,IEP15InstructionText FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + IEPId;


            Dt = objData.ReturnDataTable(strQuery, false);

            Count = Dt.Columns.Count;
            IEPC = new string[Count];
            IEPCHK = new string[Count];
            int index = 0;
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    foreach (DataRow Dr in Dt.Rows)
                    {
                        for (i = 0; i < Count; i++)
                        {
                            IEPC[i] = Dr[i].ToString();
                            if ((Dr[i].ToString() == "True") || (Dr[i].ToString() == "False"))
                            {
                                index++;
                                IEPCHK[i] = Dr[i].ToString();
                            }
                        }
                    }

                }
            }
            for (i = 0; i < IEPC.Length; i++)
            {
                if (IEPC[i] == null)
                {
                    IEPC[i] = " ";
                }
            }

        }
        catch (Exception Ex)
        {
            ClsErrorLog errlog = new ClsErrorLog();
            errlog.WriteToLog("Page Name: " + clsGeneral.getPageName() + "\n" + Ex.ToString());
        }


        C1 = new string[Count];
        C2 = new string[Count];
        if (IEPC != null) Array.Copy(IEPC, C1, Count);
        if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

    }



}