using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for clsGetGoalData
/// </summary>
public class clsGetGoalData
{
    int count = 0;
    clsData objData = null;
    DataTable Dts = null;
    DataTable DtObjectives = null;
    string strQuery = "";
    public clsGetGoalData()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static int[] removeDuplicates(int[] arr)
    {

        int end = arr.Length;

        for (int i = 0; i < end; i++)
        {
            for (int j = i + 1; j < end; j++)
            {
                if (arr[i] == arr[j])
                {
                    int shiftLeft = j;
                    for (int k = j + 1; k < end; k++, shiftLeft++)
                    {
                        arr[shiftLeft] = arr[k];
                    }
                    end--;
                    j--;
                }
            }
        }

        int[] whitelist = new int[end];
        for (int i = 0; i < end; i++)
        {
            whitelist[i] = arr[i];
        }
        return whitelist;
    }
    public string getGoals(DataTable Dt, int IEPId, int StudentId)
    {
        string strHtml = "";
        string GoalNo = "";
        string plcCurrentPerfoGoal = "";
        string[] GoalNames = null;
        string[] GoalNumbers = null;
        string[] GoalNotes = null;
        string plcLessonName = "";
        int flag = 0;
        string Condition = "";
        string temp = "";
        objData = new clsData();
        Dts = new DataTable();
        int[] arrGoalNos=null;
        string GoalIds = "";

        if (Dt != null && Dt.Rows.Count > 0)
        {
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                GoalIds += Dt.Rows[i]["GoalId"].ToString() + ",";
                
            }
           
            GoalIds = GoalIds.TrimEnd(',');

            strQuery = "select Goal.GoalId as Id, Goal.GoalCode,IEPGoalNo,GoalIEPNote from Goal INNER JOIN StdtGoal  on StdtGoal.GoalId =Goal.GoalId WHERE StdtGoal.StdtIEPId=  '" + IEPId + "' and  Goal.GoalId In (" + GoalIds + ") and StdtGoal.IncludeIEP=1 order by StdtGoal.IEPGoalNo ";
            Dts = objData.ReturnDataTable(strQuery, false);

            count = Dt.Rows.Count;
            arrGoalNos = new int[count];
            
            for (int i = 0; i < count; i++)
            {
                if (Dt.Rows[i]["IEPGoalNo"] != null && Dt.Rows[i]["IEPGoalNo"] != DBNull.Value)
                {
                    arrGoalNos[i] = Convert.ToInt16(Dt.Rows[i]["IEPGoalNo"]);
                }
            }
        }

        arrGoalNos = removeDuplicates(arrGoalNos);


        if (Dts != null && Dts.Rows.Count > 0)
        {
            count = Dts.Rows.Count;
            GoalNames = new string[count];
            GoalNumbers = new string[count];
            GoalNotes = new string[count];
            for (int i = 0; i < count; i++)
            {
                GoalNames[i] = Dts.Rows[i]["GoalCode"].ToString();
                GoalNotes[i] = Dts.Rows[i]["GoalIEPNote"].ToString();
            }

        }


        if (Dts != null && Dts.Rows.Count > 0)
        {
            int goalCount = Dts.Rows.Count;
            strHtml += "<table style='width: 100%; border-top: 6px solid black; padding: 0cm 5.4pt 0cm 5.4pt'>" +
                "<tr><td style='text-align: center;font-size: 14.0pt;" +
                " font-weight: bold;'><span lang='EN-US' style='font-size: 14.0pt; font-weight: bold; mso-bidi-font-size: 10.0pt; font-family: 'Helvetica','sans-serif';" +
                " mso-bidi-font-family: 'Times New Roman'>Current  Performance Levels/Measurable Annual Goals<span lang='EN-US' style='font-size: 14.0pt;" +
                " mso-bidi-font-size: 10.0pt; font-family: 'Helvetica','sans-serif'; mso-bidi-font-family: 'Times New Roman'></td> </tr>";

            //strHtml += "<table style='width: 100%; border-top: 6px solid black; padding: 0cm 5.4pt 0cm 5.4pt'>";
            flag = 0;
            for (int index = 0; index < goalCount; index++)
            {
                flag = 0;

                if (Dts.Rows[index]["IEPGoalNo"] != null && Dts.Rows[index]["IEPGoalNo"] != DBNull.Value)
                {
                    GoalNo = Dts.Rows[index]["IEPGoalNo"].ToString();
                }

                strHtml += "<tr><td><table style='border: 3px solid black; width: 100%; font-size: 10.0pt;'><tr><td style='width: 10%;'>Goal #</td><td style='width: 20%;'>" + GoalNo + "" +
                       "</td><td style='width: 20%;'>Specific Goal Focus:</td><td style='width: 50%;'>" + GoalNames[index] + "</td></tr></table></td></tr>";

                strHtml += "<tr><td>" + clsGeneral.StringToHtml(GoalNotes[index]) + "</td></tr>";

                //strHtml += "<tr><td style='text-align: center;font-size: 14.0pt;" +
                //        " font-weight: bold;'><span lang='EN-US' style='font-size: 14.0pt; font-weight: bold; mso-bidi-font-size: 10.0pt; font-family: 'Helvetica','sans-serif';" +
                //        " mso-bidi-font-family: 'Times New Roman'>Current  Performance Levels/Measurable Annual Goals<span lang='EN-US' style='font-size: 14.0pt;" +
                //        " mso-bidi-font-size: 10.0pt; font-family: 'Helvetica','sans-serif'; mso-bidi-font-family: 'Times New Roman'></td> </tr> <tr><td> " +
                //       "<table style='border: 3px solid black; width: 100%; font-size: 10.0pt;'><tr><td style='width: 10%;'>Goal #</td><td style='width: 20%;'>" + GoalNo + "" +
                //       "</td><td style='width: 20%;'>Specific Goal Focus:</td><td style='width: 50%;'>" + GoalNames[index] + "</td></tr></table></td></tr>";

                GoalNo = "";
                //do
                //{
                    if (flag == 0)
                    {
                        Condition = "dbo.StdtLessonPlan.Objective1";
                    //}
                    //if (flag == 1)
                    //{
                    //    Condition = "dbo.StdtLessonPlan.Objective2";
                    //}
                    //if (flag == 2)
                    //{
                    //    Condition = "dbo.StdtLessonPlan.Objective3";
                    //}


                        string strqry = "SELECT dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName, dbo.StdtLessonPlan.GoalId,StdtIEP.AsmntYearId,(SELECT Top 1 IEPGoalNo FROM StdtGoal "
                                    + "WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + IEPId + " And StudentId=" + StudentId + ") IEPGoalNo, " + Condition + " as Objective, dbo.Goal.GoalName,"
                                    + "(select top 1 LessonOrder from DSTempHdr hdr where StudentId=" + StudentId + " and hdr.LessonPlanId=dbo.StdtLessonPlan.LessonPlanId order by hdr.LessonOrder desc) as LPOrder FROM dbo.StdtLessonPlan "
                                    + "INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId "
                                    + "INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId "
                                    + "inner join dbo.StdtIEP ON dbo.StdtLessonPlan.StdtIEPId=dbo.StdtIEP.StdtIEPId where StdtLessonPlan.GoalId in (" + Dts.Rows[index]["Id"] + ") "
                                    + "AND dbo.StdtLessonPlan.StdtIEPId =  '" + IEPId + "' and dbo.StdtLessonPlan.IncludeIEP=1 and dbo.StdtLessonPlan.ActiveInd = 'A' ORDER BY IEPGoalNo,LPOrder";
                    DtObjectives = objData.ReturnDataTable(strqry, false);

                    if (DtObjectives != null && DtObjectives.Rows.Count > 0)
                    {
                        count = DtObjectives.Rows.Count;
                        if (flag == 0)
                        {
                            int counter = 0;
                            for (int i = 0; i < count; i++)
                            {
                                plcLessonName = DtObjectives.Rows[i]["LessonPlanName"].ToString();
                                plcCurrentPerfoGoal = DtObjectives.Rows[i]["Objective"].ToString();
                                //if (DtObjectives.Rows[i]["IEPGoalNo"] != null)
                                //{
                                //    plcGlId1 = DtObjectives.Rows[i]["IEPGoalNo"].ToString();
                                //}
                                //else
                                //{
                                //    plcGlId1 = "";
                                //}
                                //strHtml = strHtml.Replace("plcGlId1", plcGlId1);
                                temp += "<tr><td style='font-size: 13px;'><strong style='font-size: 14px;'>Current Performance Level:</strong>"
                                        + "What can the student currently do?</td></tr>"
                                        + "<tr><td style='font-size: 13px;'>" + plcLessonName + "</td></tr>"
                                        + "<tr><td style='font-size: 13px; padding: 5px 0 5px 8px'>" + plcCurrentPerfoGoal + " </td> </tr>";
                                if (counter > 0)
                                {
                                    // HtmlData = HtmlData.Replace("’", "'");
                                    temp = temp.Replace("Current Performance Level:", "");
                                    temp = temp.Replace("What can the student currently do?", "");
                                }
                                counter++;
                                strHtml += temp;
                                temp = "";
                            }
                        }
                    }
                    //flag++;

                }
                //while (flag == 1);
                flag++;
                if (flag == 1)
                {

                    Condition = "dbo.StdtLessonPlan.Objective2";
                    string strqry = "SELECT dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName, dbo.StdtLessonPlan.GoalId,StdtIEP.AsmntYearId,(SELECT Top 1 IEPGoalNo FROM StdtGoal "
                                + "WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + IEPId + " And StudentId=" + StudentId + ") IEPGoalNo, " + Condition + " as Objective, dbo.Goal.GoalName,"
                                + "(select top 1 LessonOrder from DSTempHdr hdr where StudentId=" + StudentId + " and hdr.LessonPlanId=dbo.StdtLessonPlan.LessonPlanId order by hdr.LessonOrder desc) as LPOrder FROM dbo.StdtLessonPlan "
                                + "INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId "
                                + "INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId "
                                + "inner join dbo.StdtIEP ON dbo.StdtLessonPlan.StdtIEPId=dbo.StdtIEP.StdtIEPId where StdtLessonPlan.GoalId in (" + Dts.Rows[index]["Id"] + ") "
                                + "AND dbo.StdtLessonPlan.StdtIEPId =  '" + IEPId + "' and dbo.StdtLessonPlan.IncludeIEP=1 and dbo.StdtLessonPlan.ActiveInd = 'A' ORDER BY dbo.StdtLessonPlan.GoalId,LPOrder ";
                    DtObjectives = objData.ReturnDataTable(strqry, false);

                    if (DtObjectives.Rows.Count > 0)
                    {
                        int counter = 0;
                        //plcLessonName = DtObjectives.Rows[0]["LessonPlanName"].ToString();
                        plcCurrentPerfoGoal = DtObjectives.Rows[0]["Objective"].ToString();
                        //plcGlId1 = DtObjectives.Rows[i]["IEPGoalNo"].ToString();
                        //strHtml = strHtml.Replace("plcGlId1", plcGlId1);
                        temp += "<tr> <td> <table style='border-top: 3px solid black; width: 100%; font-size: 10.0pt; font-family: sans-serif; line-height: 115%;'><tbody>"
                             + " <tr><td><strong style='font-size: 14px;'>Measurable Annual Goal: </strong>What challenging, yet attainable, goal can we expect the student "
                                + " to meet by the end of this IEP period? How will we know that the student has reached this goal? </td></tr>"
                              //  + "<tr><td style='font-size: 13px;'> " + plcLessonName + "</td></tr>"
                                + " <tr><td style='font-size: 13px; padding: 5px 0 5px 0'>" + plcCurrentPerfoGoal + " </td>"
                                + " </tr> </tbody></table></td> </tr>";
                        if (counter > 0)
                        {
                            // HtmlData = HtmlData.Replace("’", "'");
                            temp = temp.Replace("Measurable Annual Goal: ", "");
                            temp = temp.Replace("What challenging, yet attainable, goal can we expect the student "
                                + " to meet by the end of this IEP period? How will we know that the student has reached this goal? ", "");
                        }
                        counter++;
                        strHtml += temp;
                        temp = "";
                    }
                }
                flag++;
                if (flag == 2)
                {
                    Condition = "dbo.StdtLessonPlan.Objective3";
                    string strqry = "SELECT dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName, dbo.StdtLessonPlan.GoalId,StdtIEP.AsmntYearId,(SELECT Top 1 IEPGoalNo FROM StdtGoal "
                                + "WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + IEPId + " And StudentId=" + StudentId + ") IEPGoalNo, " + Condition + " as Objective, dbo.Goal.GoalName,"
                                + "(select top 1 LessonOrder from DSTempHdr hdr where StudentId=" + StudentId + " and hdr.LessonPlanId=dbo.StdtLessonPlan.LessonPlanId order by hdr.LessonOrder desc) as LPOrder FROM dbo.StdtLessonPlan "
                                + "INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId "
                                + "INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId "
                                + "inner join dbo.StdtIEP ON dbo.StdtLessonPlan.StdtIEPId=dbo.StdtIEP.StdtIEPId where StdtLessonPlan.GoalId in (" + Dts.Rows[index]["Id"] + ") "
                                + "AND dbo.StdtLessonPlan.StdtIEPId =  '" + IEPId + "' and dbo.StdtLessonPlan.IncludeIEP=1 and dbo.StdtLessonPlan.ActiveInd = 'A' ORDER BY dbo.StdtLessonPlan.GoalId,LPOrder ";
                    DtObjectives = objData.ReturnDataTable(strqry, false);
                    if (DtObjectives.Rows.Count > 0)
                    {
                        int counter = 0;
                        //plcLessonName = DtObjectives.Rows[0]["LessonPlanName"].ToString();
                        plcCurrentPerfoGoal = DtObjectives.Rows[0]["Objective"].ToString();
                        //plcGlId1 = DtObjectives.Rows[i]["IEPGoalNo"].ToString();
                        //strHtml = strHtml.Replace("plcGlId1", plcGlId1);
                        temp += "<tr><td> <table style='border-top: 3px solid black; width: 100%; font-size: 10.0pt; line-height: 115%;'><tbody><tr> <td>"
                                + " <strong style='font-size: 17px;'>Benchmark/Objectives: </strong>What will the student need to do to complete this goal? </td></tr>"
                              //  + " <tr><td style='font-size: 13px;'>" + plcLessonName + "</td></tr>"
                                + " <tr> <td style='font-size: 13px; padding: 5px 0 5px 0'> " + plcCurrentPerfoGoal + ""
                                + " </td></tr></tbody></table></td> </tr>";
                        if (counter > 0)
                        {
                            // HtmlData = HtmlData.Replace("’", "'");
                            temp = temp.Replace("Benchmark/Objectives: ", "");
                            temp = temp.Replace("What will the student need to do to complete this goal?", "");
                        }
                        counter++;
                        strHtml += temp;
                        temp = "";
                    }
                }
            }//end of for loop

        }
        //if (Dts != null && Dts.Rows.Count > 0)
        //{
        //    int goalCount = Dts.Rows.Count;
        //    strHtml += "<table style='width: 100%; border-top: 6px solid black; padding: 0cm 5.4pt 0cm 5.4pt'>";
        //    for (int index = 0; index < goalCount; index++)
        //    {
        //        flag = 0;

        //        if (Dt.Rows[index]["IEPGoalNo"] != null)
        //        {
        //            GoalNo = arrGoalNos[index].ToString();
        //        }



        //        strHtml += "<tr><td style='text-align: center;font-size: 14.0pt;" +
        //                " font-weight: bold;'><span lang='EN-US' style='font-size: 14.0pt; font-weight: bold; mso-bidi-font-size: 10.0pt; font-family: 'Helvetica','sans-serif';" +
        //                " mso-bidi-font-family: 'Times New Roman'>Current  Performance Levels/Measurable Annual Goals<span lang='EN-US' style='font-size: 14.0pt;" +
        //                " mso-bidi-font-size: 10.0pt; font-family: 'Helvetica','sans-serif'; mso-bidi-font-family: 'Times New Roman'></td> </tr> <tr><td> " +
        //               "<table style='border: 3px solid black; width: 100%; font-size: 10.0pt;'><tr><td style='width: 10%;'>Goal #</td><td style='width: 20%;'>" + GoalNo + "" +
        //               "</td><td style='width: 20%;'>Specific Goal Focus:</td><td style='width: 50%;'>" + GoalNames[index] + "</td></tr></table></td></tr>";

        //        GoalNo = "";
        //        do
        //        {
        //            if (flag == 0)
        //            {
        //                Condition = "dbo.StdtLessonPlan.Objective1";
        //            }
        //            if (flag == 1)
        //            {
        //                Condition = "dbo.StdtLessonPlan.Objective2";
        //            }
        //            if (flag == 2)
        //            {
        //                Condition = "dbo.StdtLessonPlan.Objective3";
        //            }


        //            string strqry = "SELECT dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName, dbo.StdtLessonPlan.GoalId,StdtIEP.AsmntYearId,(SELECT Top 1 IEPGoalNo FROM StdtGoal "
        //                        + "WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEPId=" + IEPId + " And StudentId=" + StudentId + ") IEPGoalNo, " + Condition + " as Objective, dbo.Goal.GoalName   FROM dbo.StdtLessonPlan "
        //                        + "INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId "
        //                        + "INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId "
        //                        + "inner join dbo.StdtIEP ON dbo.StdtLessonPlan.StdtIEPId=dbo.StdtIEP.StdtIEPId where StdtLessonPlan.GoalId in (" + Dts.Rows[index]["Id"] + ") "
        //                        + "AND dbo.StdtLessonPlan.StdtIEPId =  '" + IEPId + "' ORDER BY dbo.StdtLessonPlan.GoalId ";
        //            DtObjectives = objData.ReturnDataTable(strqry, false);

        //            if (DtObjectives != null && DtObjectives.Rows.Count > 0)
        //            {
        //                count = DtObjectives.Rows.Count;
        //                if (flag == 0)
        //                {
        //                    int counter = 0;
        //                    for (int i = 0; i < count; i++)
        //                    {
        //                        plcLessonName = DtObjectives.Rows[i]["LessonPlanName"].ToString();
        //                        plcCurrentPerfoGoal = DtObjectives.Rows[i]["Objective"].ToString();
        //                        //if (DtObjectives.Rows[i]["IEPGoalNo"] != null)
        //                        //{
        //                        //    plcGlId1 = DtObjectives.Rows[i]["IEPGoalNo"].ToString();
        //                        //}
        //                        //else
        //                        //{
        //                        //    plcGlId1 = "";
        //                        //}
        //                        //strHtml = strHtml.Replace("plcGlId1", plcGlId1);
        //                        temp += "<tr><td style='font-size: 13px;'><strong style='font-size: 14px;'>Current Performance Level:</strong>"
        //                                + "What can the student currently do?</td></tr>"
        //                                + "<tr><td style='font-size: 13px;'>" + plcLessonName + "</td></tr>"
        //                                + "<tr><td style='font-size: 13px; padding: 5px 0 5px 8px'>" + plcCurrentPerfoGoal + " </td> </tr>";
        //                        if (counter > 0)
        //                        {
        //                            // HtmlData = HtmlData.Replace("’", "'");
        //                            temp = temp.Replace("Current Performance Level:", "");
        //                            temp = temp.Replace("What can the student currently do?", "");
        //                        }
        //                        counter++;
        //                        strHtml += temp;
        //                        temp = "";
        //                    }
        //                }
        //                if (flag == 1)
        //                {
        //                    int counter = 0;
        //                    for (int i = 0; i < count; i++)
        //                    {
        //                        plcLessonName = DtObjectives.Rows[i]["LessonPlanName"].ToString();
        //                        plcCurrentPerfoGoal = DtObjectives.Rows[i]["Objective"].ToString();
        //                        //plcGlId1 = DtObjectives.Rows[i]["IEPGoalNo"].ToString();
        //                        //strHtml = strHtml.Replace("plcGlId1", plcGlId1);
        //                        temp += "<tr> <td> <table style='border-top: 3px solid black; width: 100%; font-size: 10.0pt; font-family: sans-serif; line-height: 115%;'><tbody>"
        //                             + " <tr><td><strong style='font-size: 14px;'>Measurable Annual Goal: </strong>What challenging, yet attainable, goal can we expect the student "
        //                                + " to meet by the end of this IEP period? How will we know that the student has reached this goal? </td></tr>"
        //                                + "<tr><td style='font-size: 13px;'> " + plcLessonName + "</td></tr>"
        //                                + " <tr><td style='font-size: 13px; padding: 5px 0 5px 0'>" + plcCurrentPerfoGoal + " </td>"
        //                                + " </tr> </tbody></table></td> </tr>";
        //                        if (counter > 0)
        //                        {
        //                            // HtmlData = HtmlData.Replace("’", "'");
        //                            temp = temp.Replace("Measurable Annual Goal: ", "");
        //                            temp = temp.Replace("What challenging, yet attainable, goal can we expect the student "
        //                                + " to meet by the end of this IEP period? How will we know that the student has reached this goal? ", "");
        //                        }
        //                        counter++;
        //                        strHtml += temp;
        //                        temp = "";
        //                    }
        //                }
        //                if (flag == 2)
        //                {
        //                    int counter = 0;
        //                    for (int i = 0; i < count; i++)
        //                    {
        //                        plcLessonName = DtObjectives.Rows[i]["LessonPlanName"].ToString();
        //                        plcCurrentPerfoGoal = DtObjectives.Rows[i]["Objective"].ToString();
        //                        //plcGlId1 = DtObjectives.Rows[i]["IEPGoalNo"].ToString();
        //                        //strHtml = strHtml.Replace("plcGlId1", plcGlId1);
        //                        temp += "<tr><td> <table style='border-top: 3px solid black; width: 100%; font-size: 10.0pt; line-height: 115%;'><tbody><tr> <td>"
        //                                + " <strong style='font-size: 17px;'>Benchmark/Objectives: </strong>What will the student need to do to complete this goal? </td></tr>"
        //                                + " <tr><td style='font-size: 13px;'>" + plcLessonName + "</td></tr>"
        //                                + " <tr> <td style='font-size: 13px; padding: 5px 0 5px 0'> " + plcCurrentPerfoGoal + ""
        //                                + " </td></tr></tbody></table></td> </tr>";
        //                        if (counter > 0)
        //                        {
        //                            // HtmlData = HtmlData.Replace("’", "'");
        //                            temp = temp.Replace("Benchmark/Objectives: ", "");
        //                            temp = temp.Replace("What will the student need to do to complete this goal?", "");
        //                        }
        //                        counter++;
        //                        strHtml += temp;
        //                        temp = "";
        //                    }
        //                }
        //            }
        //            flag++;
        //        }
        //        while (flag <= 2);
        //    }
        //}
        if (strHtml != null && strHtml != "")
        {
            strHtml += "</table>";
        }
        return strHtml;
    }
}