
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsStatus
/// </summary>
public class clsStatus
{
    public clsStatus()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string biWeeklyStatus(int StudentId)
    {
        string strQuery = "";
        DataClass oData = new DataClass();

        int iepidinprogress = oData.ExecuteScalar("select COUNT(StdtIEPId) from StdtIEP where StudentId=" + StudentId + " and StatusId=" +
                                                "(select LookUpId from LookUp where LookupType='IEP Status' and LookupName='In Progress')");

        int iepidcomplete = oData.ExecuteScalar("select COUNT(StdtIEPId) from StdtIEP where StudentId=" + StudentId + " and StatusId=" +
                                                "(select LookUpId from LookUp where LookupType='IEP Status' and LookupName='Complete')");

        int iepidapproved = oData.ExecuteScalar("select COUNT(StdtIEPId) from StdtIEP where StudentId=" + StudentId + " and StatusId=" +
                                                "(select LookUpId from LookUp where LookupType='IEP Status' and LookupName='Approved')");

        strQuery = "select COUNT(StdtLessonPlanId) from StdtLessonPlan where StudentId=" + StudentId + "  and StatusId=" +
                                                    "(select LookUpId from LookUp where LookupType='TemplateStatus' and LookupName='Complete')";

        int lesplanidcomplete = oData.ExecuteScalar(strQuery);

        int lesplanidinprogress = oData.ExecuteScalar("select COUNT(StdtLessonPlanId) from StdtLessonPlan where StudentId=" + StudentId + "  and StatusId=" +
                                                        "(select LookUpId from LookUp where LookupType='TemplateStatus' and LookupName='In Progress')");

        int lesplanidapproved = oData.ExecuteScalar("select COUNT(StdtLessonPlanId) from StdtLessonPlan where StudentId=1  and StatusId=" +
                                                        "(select LookUpId from LookUp where LookupType='TemplateStatus' and LookupName='Approved')");

        int assmntidcomplete = oData.ExecuteScalar("select COUNT(AsmntId) from Assessment where StudentId=2 and AsmntStatusId=" +
                                                    "(select LookUpId from LookUp where LookupType='Assessment Status' and LookupName='Complete')");
        int assmntidinprogress = oData.ExecuteScalar("select COUNT(AsmntId) from Assessment where StudentId=2 and AsmntStatusId=" +
                                                    "(select LookUpId from LookUp where LookupType='Assessment Status' and LookupName='In Progress')");

        string statuscode = "";
        string a = "welcomeeeeeeee1";
        string message = "Message for IEP";
        statuscode = " <div id='stMenu' style='width: 100%; height: auto; margin-left: 0%; display: block;'>" +
                                                "<div class='divStatusAll'>" +
                                                    "<table class='StatusTable'>" +
                                                        "<tr>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td><a  id='IEPList.aspx-1' href='#' onmouseover='showtip(event,Messsage for IEP)' onmouseout='hidetip()' class='stylishActive' onclick='selSubmenuforStatus(this)'>4</a>Complete<br />" +
                                                               " IEP Form</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                           " <td>&nbsp;</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td><a id='ReviewAssessmnt.aspx-1' style='' href='#' onmouseover='alert('Sucess')' onmouseout='alert('leave')' class='stylish' onclick='selSubmenuforStatus(this)'>3</a>Generate<br />" +
                                                               " IEP Goals</td>" +
                                                            "<td>" +
                                                                "<img src='../Administration/images/arrowStatusRotate.png' alt='RotateIt' style='margin: 0 15% 0 15%;' width='50%' height='73%' /></td>" +
                                                            "<td><a id='IEPList.aspx-2' href='#' onmouseover='showtip(event," + message + ")' onmouseout='hidetip()' class='stylish' onclick='selSubmenuforStatus(this)'>5</a>Submit" +
                                                              " <br />" +
                                                               "for Approval</td>" +
                                                           "<td>" +
                                                                "<img src='../Administration/images/GoArrow-Left.png' alt='GoRight' width='50%' height='50%' /></td>" +
                                                           "<td><a id='StudentLessonplan.aspx-1' href='#' onmouseover='showtip(event," + message + ")' onmouseout='hidetip()' class='stylishActive' onclick='selSubmenuforStatus(this)'>6</a>Assign<br />" +
                                                               " New Lesson Plans</td>" +
                                                           " <td>" +
                                                               " <img src='../Administration/images/GoArrow-Left.png' alt='GoRight' width='50%' height='50%' /></td>" +
                                                           " <td><a id='TemplateEditor.aspx-1' href='#' onmouseover='showtip(event," + message + ")' onmouseout='hidetip()' class='stylish' onclick='selSubmenuforStatus(this)'>7</a>Customize" +
                                                                "<br />" +
                                                               " Lesson Plans</td>" +
                                                           " <td>" +
                                                              "  <img src='../Administration/images/GoArrow-Left.png' alt='GoRight' width='50%' height='50%' /></td>" +
                                                           " <td><a id='ViewLessonplanList.aspx-1' href='#' onmouseover='showtip(event," + message + ")' onmouseout='hidetip()' class='stylish' onclick='selSubmenuforStatus(this)'>8</a>All" +

                                                                "Lesson<br />" +
                                                               " Plans Approved</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                          "  <td colspan='3'>&nbsp;</td>" +
                                                           " <td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                           " <td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                         "   <td colspan='3'>&nbsp;</td>" +
                                                          "  <td>&nbsp;</td>" +
                                                           " <td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                           " <td colspan='3'>" +
                                                            "    <table width='56%' style='margin: 0 auto;'>" +
                                                             "       <tr>" +
                                                              "          <td><a href='#' id='AssessmntList.aspx-2'  style='top:20px' onmouseover='showtip(event,'Message for IEP')' onmouseout='hidetip()' class='stylishActive' onclick='selSubmenuforStatus(this)'>2</a>Score<br />" +
                                                               "             Assessments</td>" +
                                                                "        <td><a href='#' id='AssessmntList.aspx-1' onmouseover='showtip(event,'Message for IEP')' onmouseout='hidetip()' style='margin-left: 9px' class='stylish' onclick='selSubmenuforStatus(this)'>1</a>Choose<br />" +
                                                                 "           Assessments</td>" +
                                                                  "  </tr>" +
                                                                "</table>" +
                                                            "</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                            "<td>&nbsp;</td>" +
                                                        "</tr>" +
                                                    "</table>" +
                                                    "<br />" +
                                                    "<br />" +
                                                    "<hr style='border: groove;' />" +

                                                     "<div id='popup_ChooseAssessments' class='chat-bubble' style='left: 10%; top: 67%;'>" +
                                                     "Student Id:" + StudentId + "</br>" +
                                                      "  Assesment InProgress:" + assmntidinprogress + "</br>" +
                                                      "  Assesment Complete:" + assmntidcomplete +
                                                  "</div>" +

                                                   "<div id='popup_ScoreAssessments' class='chat-bubble' style='left: 10%; top: 67%;'>" +
                                                    "Student Id:" + StudentId + "</br>" +
                                                      "  Assesment InProgress:" + assmntidinprogress + "</br>" +
                                                      "  Assesment Complete:" + assmntidcomplete +
                                                  "</div>" +
                                                   " <div id='popup_GenerateIEP' class='chat-bubble' style='left: 10%; top: 67%;'>" +
                                                   "Student Id:" + StudentId + "</br>" +
                                                    "  IEP InProgress:" + iepidinprogress + "</br>" +
                                                     "  IEP Completed:" + iepidcomplete + "</br>" +
                                                      "  IEP Approved:" + iepidapproved +
                                                  "</div>" +
                                                   "<div id='popup_ViewIEP' class='chat-bubble' style='left: 10%; top: 67%;'>" +

                                                     "Student Id:" + StudentId + "</br>" +
                                                    "  IEP InProgress:" + iepidinprogress + "</br>" +
                                                     "  IEP Completed:" + iepidcomplete + "</br>" +
                                                       "  IEP Approved:" + iepidapproved +
                                                  "</div>" +

                                                   "<div id='popup_Approval' class='chat-bubble' style='left: 10%; top: 67%;'>" +
                                                     "Student Id:" + StudentId + "</br>" +
                                                      "  IEP InProgress:" + iepidinprogress + "</br>" +
                                                       "  IEP Completed:" + iepidcomplete + "</br>" +
                                                         "  IEP Approved:" + iepidapproved +
                                                  "</div>" +
                                                   "<div id='popup_AssignLessonPlans' class='chat-bubble' style='left: 10%; top: 67%;'>" +
                                                     "Student Id:" + StudentId + "</br>" +
                                                     "  Lessonplan InProgress:" + lesplanidinprogress + "</br>" +
                                                       "  Lessonplan Completed:" + lesplanidcomplete + "</br>" +
                                                         "  Lessonplan Approved:" + lesplanidapproved +
                                                  "</div>" +
                                                   "<div id='popup_CustomizeLessonPlans' class='chat-bubble' style='left: 10%; top: 67%;'>" +
                                                     "Student Id:" + StudentId + "</br>" +
                                                    "  Lessonplan InProgress:" + lesplanidinprogress + "</br>" +
                                                       "  Lessonplan Completed:" + lesplanidcomplete + "</br>" +
                                                         "  Lessonplan Approved:" + lesplanidapproved +
                                                  "</div>" +
                                                   "<div id='popup_ApprovedLessonPlans' class='chat-bubble' style='left: 10%; top: 67%;'>" +
                                                     "Student Id:" + StudentId + "</br>" +
                                                    "  Lessonplan InProgress:" + lesplanidinprogress + "</br>" +
                                                       "  Lessonplan Completed:" + lesplanidcomplete + "</br>" +
                                                         "  Lessonplan Approved:" + lesplanidapproved +
                                                  "</div>" +
                                                "</div>" +

                                            "</div>";


        return statuscode;

    }


}