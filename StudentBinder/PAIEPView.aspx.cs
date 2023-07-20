using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using System.Xml;
using System.IO.Packaging;
using System.Reflection;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading;
using NotesFor.HtmlToOpenXml;
using DocumentFormat.OpenXml;
using System.Net;
using System.Xml.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Configuration;
using System.Globalization;





public partial class StudentBinder_PAIEPView : System.Web.UI.Page
{
    clsData objData = null;
    ClsTemplateSession ObjTempSess = null;
    clsSession Sess = null;

    clsSession sess = null;
    static int intStdtId = 0;
    static int schoolId = 0;
    int intStdtIEPId = 0;
    System.Data.DataTable Dt = null;
    static int checkCount = 0;
    static string[] columns;
    static string[] placeHolders;

    static string[] columnsCheck;

    protected void Page_Load(object sender, EventArgs e)
    {
        Sess = (clsSession)Session["UserSession"];
        if (Sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            bool flag = clsGeneral.PageIdentification(Sess.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        if (!IsPostBack)
        {
            //GetStdtIEPId();

            LoadData();
        }
    }

    private void LoadData()
    {
        try
        {
            if (Request.QueryString["studid"] != null)
            {
                int pageid = Convert.ToInt32(Request.QueryString["pageid"].ToString());
                int studid = Convert.ToInt32(Request.QueryString["studid"].ToString());

                Sess.StudentId = studid;
                Sess.IEPId = pageid;

            }
            FillIEPPage1();
            FillIEPPage2();
            FillIEPPage3();
            FillIEPPage4();
            FillIEPPage5();
            FillIEPPage6();
            FillIEPPage7();
            FillIEPPage8();
            FillIEPPage9();
            FillIEPPage10();
            FillIEPPage11();
            FillIEPPage12();
            FillIEPPage13();
            FillIEPPage14();
            FillIEPPage15();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    protected void GetStdtIEPId()
    {
        objData = new clsData();
        string strQuery = "Select StdtIEPId from StdtIEP where StudentId='" + intStdtId + "'";
        intStdtIEPId = objData.ExecuteWithScope(strQuery);
    }

    protected void FillIEPPage1()
    {
        sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        clsIEP IEPObj = new clsIEP();
        TimeSpan tempDatetime;
        Dt = new System.Data.DataTable();
        try
        {
            string server = ConfigurationManager.AppSettings["BuildName"];
            string strQuery = "";
            if (server == "Integrated")
            {
                //strQuery = "select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS,ST.DOB,ST.GradeLevel,ADR.AddressLine1+'</br>'+ADR.AddressLine2+'</br>'+"
                //        + "ADR.AddressLine3 as Addres,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=1)) as HomePhone,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=2)) AS Mobile from Student ST inner join AddressList ADR on ADR.AddressId=ST.AddressId where StudentId=" + sess.StudentId + ""
                //        + "and SchoolId=" + sess.SchoolId;

                strQuery = "Select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS, ST.DOB,ST.GradeLevel,ADR.ApartmentType,ADR.StreetName,ADR.City,(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=1)) AS Phone,"
           + "(SELECT Phone FROM AddressList WHERE AddressId= (SELECT top(1) AddressId FROM StudentAddresRel  WHERE StudentPersonalId=" + sess.StudentId + " AND ContactSequence=2)) AS Mobile from Student ST Inner Join StudentAddresRel SAR ON ST.StudentId=SAR.StudentPersonalId "
           + "Inner Join AddressList ADR ON SAR.AddressId=ADR.AddressId Where ST.StudentId=" + sess.StudentId + " And ST.SchoolId=" + sess.SchoolId + " And SAR.ContactSequence=0";


            }
            else
            {
                strQuery = "select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,101) as DOBS,ST.DOB,ST.GradeLevel,ADR.AddressLine1+'</br>'+ADR.AddressLine2+'</br>'+"
                        + "ADR.AddressLine3 as Addres,ADR.HomePhone,ADR.Mobile from Student ST inner join Address ADR on ADR.AddressId=ST.AddressId where StudentId=" + sess.StudentId + ""
                        + " and SchoolId=" + sess.SchoolId;
            }


            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblStudentName.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                    lblstudentnme.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                    Label2.Text = Dt.Rows[0]["DOBS"].ToString().Trim();
                    tempDatetime = DateTime.Now - Convert.ToDateTime(Dt.Rows[0]["DOB"].ToString().Trim());
                    double dats = tempDatetime.TotalDays;
                    int age = Convert.ToInt32(dats / 360);
                    if (age > 0)
                    {
                        Label3.Text = age.ToString();
                    }
                    else Label3.Text = "";
                    Label4.Text = Dt.Rows[0]["GradeLevel"].ToString().Trim();
                    Label9.Text = Dt.Rows[0]["Phone"].ToString().Trim();
                    Label10.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                    //  lblOtherInformations.Text = Dt.Rows[0]["StdtIEPId"].ToString().Trim();
                    Label8.Text = Dt.Rows[0]["ApartmentType"].ToString().Trim() + "," + Dt.Rows[0]["StreetName"].ToString().Trim() + "," + Dt.Rows[0]["City"].ToString().Trim();
                    //ADR.ApartmentType,ADR.StreetName,ADR.City
                    // sess.IEPId = Convert.ToInt32(hidIEPId.Value);
                }
            }

            Dt = new System.Data.DataTable();
            strQuery = "SELECT Convert(varchar,IepTeamMeetingDate,101)as IepTeamMeetingDate,Convert(varchar,IepImplementationDate,101)as IepImplementationDate," +
                "AnticipatedDurationofServices,AnticipatedYearOfGraduation,LocalEducationAgency,"
                + " OtherInformation,DocumentedBy,CountyOfResidance FROM StdtIEP_PE WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    Label11.Text = Dt.Rows[0]["OtherInformation"].ToString().Trim();
                    Label11.Text = Label11.Text.Replace("##", "'");
                    Label11.Text = Label11.Text.Replace("?bs;", "\\");
                    string checkDateImpl = Dt.Rows[0]["IepImplementationDate"].ToString();
                    if (checkDateImpl !=null)
                    {
                        try
                        {
                            lblIEPImplementationDate.Text = Dt.Rows[0]["IepImplementationDate"].ToString();
                        }
                        catch(Exception)
                        {
                            lblIEPImplementationDate.Text = "";
                        }
                    }
                    Label7.Text = Dt.Rows[0]["CountyOfResidance"].ToString().Trim();
                    Label5.Text = Dt.Rows[0]["AnticipatedYearOfGraduation"].ToString().Trim();
                    Label6.Text = Dt.Rows[0]["LocalEducationAgency"].ToString().Trim();
                    Label1.Text = Dt.Rows[0]["AnticipatedDurationofServices"].ToString().Trim();
                    Label12.Text = Dt.Rows[0]["DocumentedBy"].ToString().Trim();
                    Label12.Text = Label12.Text.Replace("##", "'");
                    Label12.Text = Label12.Text.Replace("?bs;", "\\");
                    string checkDate = Dt.Rows[0]["IepTeamMeetingDate"].ToString();
                    if (checkDate != null)
                    {
                        try
                        {
                            lblIEPTeamMeetingDate.Text = Dt.Rows[0]["IepTeamMeetingDate"].ToString();
                        }
                        catch(Exception)
                        {
                            lblIEPTeamMeetingDate.Text = "";
                        }
                    }

                }
            }

            Dt = new System.Data.DataTable();
            string strQuery1 = "select [IEPPA1ExtensionId],CONVERT(VARCHAR(50),[DateOfRevisions],101) AS DateOfRevisions,[Participants],[IEPSections] from [dbo].[IEPPA1Extension] where [IepPAId]=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Repeater1.DataSource = Dt;
                    Repeater1.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage2()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        clsIEP IEPObj = new clsIEP();
        Dt = new System.Data.DataTable();
        try
        {
            string strQuery = "SELECT ParentName,ParentSing,StudentName,StudentSign,RegEduTeacherName," +
                         "regEduTeacherSign,SpclEduTeacherName,SpclEduTeacherSign,LocalEdAgencyName,localEdAgencySign," +
                         " CareerEdRepName,careerEdRepSign,CommunityAgencyName,CommunityAgencySign,TeacherGiftedName," +
                         " TeacherGiftedSign,WittenInput FROM dbo.IEP_PE2_Team WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Label13.Text = Dt.Rows[0]["ParentName"].ToString().Trim();
                    Label14.Text = Dt.Rows[0]["ParentSing"].ToString().Trim();

                    Label17.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                    Label18.Text = Dt.Rows[0]["StudentSign"].ToString().Trim();
                    Label19.Text = Dt.Rows[0]["RegEduTeacherName"].ToString().Trim();
                    Label20.Text = Dt.Rows[0]["regEduTeacherSign"].ToString().Trim();
                    Label21.Text = Dt.Rows[0]["SpclEduTeacherName"].ToString().Trim();
                    Label22.Text = Dt.Rows[0]["SpclEduTeacherSign"].ToString().Trim();
                    Label23.Text = Dt.Rows[0]["LocalEdAgencyName"].ToString().Trim();
                    Label24.Text = Dt.Rows[0]["localEdAgencySign"].ToString().Trim();
                    Label25.Text = Dt.Rows[0]["CareerEdRepName"].ToString().Trim();
                    Label26.Text = Dt.Rows[0]["careerEdRepSign"].ToString().Trim();
                    Label27.Text = Dt.Rows[0]["CommunityAgencyName"].ToString().Trim();

                    Label28.Text = Dt.Rows[0]["CommunityAgencySign"].ToString().Trim();
                    Label29.Text = Dt.Rows[0]["TeacherGiftedName"].ToString().Trim();

                    Label30.Text = Dt.Rows[0]["TeacherGiftedSign"].ToString().Trim();
                    Label31.Text = Dt.Rows[0]["WittenInput"].ToString().Trim();

                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage3()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "SELECT IEP3ParentSign FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Label32.Text = Dt.Rows[0]["IEP3ParentSign"].ToString().Trim();

                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage4()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "SELECT IEP4IsBlind,IEP4Isdeaf,IEP4CommNeeded,IEP4AssistiveTechNeeded,IEP4EnglishProficiency,IEP4ImpedeLearning FROM IEP_PE_Details"
                + " WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        if (dr["IEP4IsBlind"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4IsBlind"].ToString()))
                            {
                                Checkbox1.Checked = true;
                            }
                            else
                            {
                                Checkbox2.Checked = true;
                            }
                        }
                        if (dr["IEP4Isdeaf"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4Isdeaf"].ToString()))
                            {
                                Checkbox3.Checked = true;
                            }
                            else
                            {
                                Checkbox4.Checked = true;
                            }
                        }
                        if (dr["IEP4CommNeeded"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4CommNeeded"].ToString()))
                            {
                                Checkbox5.Checked = true;
                            }
                            else
                            {
                                Checkbox6.Checked = true;
                            }
                        }
                        if (dr["IEP4AssistiveTechNeeded"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4AssistiveTechNeeded"].ToString()))
                            {
                                Checkbox7.Checked = true;
                            }
                            else
                            {
                                Checkbox8.Checked = true;
                            }
                        }
                        if (dr["IEP4EnglishProficiency"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4EnglishProficiency"].ToString()))
                            {
                                Checkbox9.Checked = true;
                            }
                            else
                            {
                                Checkbox10.Checked = true;
                            }
                        }
                        if (dr["IEP4ImpedeLearning"].ToString() != "")
                        {
                            if (Convert.ToBoolean(dr["IEP4ImpedeLearning"].ToString()))
                            {
                                Checkbox11.Checked = true;
                            }
                            else
                            {
                                Checkbox12.Checked = true;
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }


    protected void FillIEPPage5()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "SELECT IEP5OtherSpecify,IEP5CIPCode FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        Label33.Text = Dt.Rows[0]["IEP5OtherSpecify"].ToString().Trim();
                        Label33.Text = Label33.Text.Replace("##", "'");
                        Label33.Text = Label33.Text.Replace("?bs;", "\\");
                        Label35.Text = Dt.Rows[0]["IEP5CIPCode"].ToString().Trim();
                        Label35.Text = Label35.Text.Replace("##", "'");
                        Label35.Text = Label35.Text.Replace("?bs;", "\\");
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage6()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        ChkMeasure1.Checked = false;
        ChkMeasure2.Checked = false;
        ChkMeasure3.Checked = false;
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "select IEP6TrainingGoal,IEP6TrainCoursesStudy,IEP6EmploymentGoal,IEP6EmpCoursesStudy,IEP6LivingGoal,IEP6LivingCoursesStudy,IEP6MeasurableCheck1, "
                 + "IEP6MeasurableCheck2,IEP6MeasurableCheck3 from dbo.IEP_PE_Details where StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in Dt.Rows)
                    {
                        Label39.Text = Dt.Rows[0]["IEP6EmpCoursesStudy"].ToString().Trim();
                        Label39.Text = Label39.Text.Replace("##", "'");
                        Label39.Text = Label39.Text.Replace("?bs;", "\\");
                        Label38.Text = Dt.Rows[0]["IEP6EmploymentGoal"].ToString().Trim();
                        Label38.Text = Label38.Text.Replace("##", "'");
                        Label38.Text = Label38.Text.Replace("?bs;", "\\");
                        Label41.Text = Dt.Rows[0]["IEP6LivingCoursesStudy"].ToString().Trim();
                        Label41.Text = Label41.Text.Replace("##", "'");
                        Label41.Text = Label41.Text.Replace("?bs;", "\\");
                        Label40.Text = Dt.Rows[0]["IEP6LivingGoal"].ToString().Trim();
                        Label40.Text = Label40.Text.Replace("##", "'");
                        Label40.Text = Label40.Text.Replace("?bs;", "\\");
                        Label37.Text = Dt.Rows[0]["IEP6TrainCoursesStudy"].ToString().Trim();
                        Label37.Text = Label37.Text.Replace("##", "'");
                        Label37.Text = Label37.Text.Replace("?bs;", "\\");
                        Label36.Text = Dt.Rows[0]["IEP6TrainingGoal"].ToString().Trim();
                        Label36.Text = Label36.Text.Replace("##", "'");
                        Label36.Text = Label36.Text.Replace("?bs;", "\\");
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck1"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck1"].ToString())))
                        {
                            ChkMeasure1.Checked = true;
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck2"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck2"].ToString())))
                        {
                            ChkMeasure2.Checked = true;
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck3"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP6MeasurableCheck3"].ToString())))
                        {
                            ChkMeasure3.Checked = true;
                        }
                    }


                }
            }
            //grid 1
            string getStdGoalSvcDetails1 = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Edu WHERE StdtIEP_PEId=" + sess.IEPId;
            System.Data.DataTable dt_goalDetails1 = objData.ReturnDataTable(getStdGoalSvcDetails1, false);
            if (dt_goalDetails1 != null)
            {
                if (dt_goalDetails1.Rows.Count > 0)
                {
                    Repeater2.DataSource = dt_goalDetails1;
                    Repeater2.DataBind();
                }
            }

            //grid 2
            string getStdGoalSvcDetails2 = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Goal WHERE StdtIEP_PEId=" + sess.IEPId;
            System.Data.DataTable dt_goalDetails2 = objData.ReturnDataTable(getStdGoalSvcDetails2, false);
            if (dt_goalDetails2 != null)
            {
                if (dt_goalDetails2.Rows.Count > 0)
                {
                    Repeater3.DataSource = dt_goalDetails2;
                    Repeater3.DataBind();
                }
            }

            //Grid 3
            string getStdGoalSvcDetails3 = "SELECT Id,Service,Location,Frequency,Convert(varchar,PrjBeginning,101)as PrjBeginning,AnticipatedDur,Person FROM dbo.IEP_PE6_Living WHERE StdtIEP_PEId=" + sess.IEPId;
            System.Data.DataTable dt_goalDetails3 = objData.ReturnDataTable(getStdGoalSvcDetails3, false);
            if (dt_goalDetails3 != null)
            {
                if (dt_goalDetails3.Rows.Count > 0)
                {
                    Repeater4.DataSource = dt_goalDetails3;
                    Repeater4.DataBind();
                }
            }

        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage7()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        Dt = new System.Data.DataTable();
        System.Data.DataTable Dt1 = new System.Data.DataTable();

        try
        {
            string server = ConfigurationManager.AppSettings["BuildName"];
            string strQuery1 = "";
            if (server == "Integrated")
            {
                strQuery1 = "select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,110) as DOBS,ST.DOB,ST.GradeLevel,ADR.AddressLine1+'</br>'+ADR.AddressLine2+'</br>'+"
                                + "ADR.AddressLine3 as Addres,ADR.Phone as HomePhone,ADR.Mobile from Student ST inner join AddressList ADR on ADR.AddressId=ST.AddressId where StudentId=" + sess.StudentId + ""
                                + "and SchoolId=" + sess.SchoolId;
            }
            else
            {
                strQuery1 = "select ST.StudentLname+','+ST.StudentFname as StudentName,convert(varchar,ST.DOB,110) as DOBS,ST.DOB,ST.GradeLevel,ADR.AddressLine1+'</br>'+ADR.AddressLine2+'</br>'+"
                                + "ADR.AddressLine3 as Addres,ADR.HomePhone,ADR.Mobile from Student ST inner join Address ADR on ADR.AddressId=ST.AddressId where StudentId=" + sess.StudentId + ""
                                + "and SchoolId=" + sess.SchoolId;
            }

            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    lblStudentName.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                }
            }

            string strQuery2 = "SELECT IEP8AsmtNotAdministred,IEP8ReadPartcptPSSAWithoutAcmdtn,IEP8ReadPartcptPSSAWithFollowingAcmdtn,IEP8ReadPartcptPSSAModiWithoutAcmdtn,IEP8ReadPartcptPSSAModiWithFollowingAcmdtn,"
                               + "IEP8MathPartcptPSSAWithoutAcmdtn,IEP8MathPartcptPSSAWithFollowingAcmdtn,IEP8MathPartcptPSSAModiWithoutAcmdtn,IEP8MathPartcptPSSAModiWithFollowingAcmdtn FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;

            Dt1 = objData.ReturnDataTable(strQuery2, false);
            if (Dt1 != null)
            {
                if (Dt1.Rows.Count > 0)
                {
                    if (Dt1.Rows[0]["IEP8AsmtNotAdministred"].ToString() != "")
                        IEP8AsmtNotAdministred.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8AsmtNotAdministred"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithoutAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAModiWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithFollowingAcmdtn"].ToString() != "")
                        IEP8ReadPartcptPSSAModiWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8ReadPartcptPSSAModiWithFollowingAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithoutAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAModiWithoutAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithoutAcmdtn"]);

                    if (Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithFollowingAcmdtn"].ToString() != "")
                        IEP8MathPartcptPSSAModiWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt1.Rows[0]["IEP8MathPartcptPSSAModiWithFollowingAcmdtn"]);


                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage8()
    {
        Dt = new System.Data.DataTable();
        objData = new clsData();
        int videoTape = 0;
        int writtenParam = 0;
        sess = (clsSession)Session["UserSession"];
        try
        {

            //    string strQuery = "SELECT IEP8PSSAReading,IEP8PSSAappropriate,IEP8Videotape,IEP8WrittenNarrative FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            //    Dt = objData.ReturnDataTable(strQuery, false);
            //    if (Dt != null)
            //    {
            //        if (Dt.Rows.Count > 0)
            //        {
            //            Label42.Text = Dt.Rows[0]["IEP8PSSAReading"].ToString().Trim();
            //            Label43.Text = Dt.Rows[0]["IEP8PSSAappropriate"].ToString().Trim();
            //            if (Dt.Rows[0]["IEP8Videotape"].ToString() != "")
            //                videoTape = Convert.ToInt32(Dt.Rows[0]["IEP8Videotape"]);
            //            if (Dt.Rows[0]["IEP8WrittenNarrative"].ToString() != "")
            //                writtenParam = Convert.ToInt32(Dt.Rows[0]["IEP8WrittenNarrative"]);

            //            if (videoTape == 1) chkContent2.Checked = true; else chkContent2.Checked = false;
            //            if (writtenParam == 1) Checkbox13.Checked = true; else Checkbox13.Checked = false;
            //        }
            //    }
            //}
            string strQuery = "SELECT IEP8PSSAReading,IEP8PSSAappropriate,IEP8Videotape,IEP8WrittenNarrative,IEP8SciencePartcptPSSAWithoutAcmdtn,IEP8SciencePartcptPSSAWithFollowingAcmdtn,"
            + "IEP8SciencePartcptPSSAModiWithoutAcmdtn,IEP8SciencePartcptPSSAModiWithFollowingAcmdtn,IEP8WritePartcptPSSAWithoutAcmdtn,IEP8WritePartcptPSSAWithFollowingAcmdtn,IEP8PSSAParticipate"
            + " FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Label42.Text = Dt.Rows[0]["IEP8PSSAReading"].ToString().Trim();
                    Label43.Text = Dt.Rows[0]["IEP8PSSAappropriate"].ToString().Trim();
                    if (Dt.Rows[0]["IEP8Videotape"].ToString() != "")
                        videoTape = Convert.ToInt32(Dt.Rows[0]["IEP8Videotape"]);
                    if (Dt.Rows[0]["IEP8WrittenNarrative"].ToString() != "")
                        writtenParam = Convert.ToInt32(Dt.Rows[0]["IEP8WrittenNarrative"]);

                    if (videoTape == 1) chkContent2.Checked = true; else chkContent2.Checked = false;
                    if (writtenParam == 1) Checkbox13.Checked = true; else Checkbox13.Checked = false;

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAWithoutAcmdtn"]);

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithoutAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAModiWithoutAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithoutAcmdtn"]);

                    if (Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithFollowingAcmdtn"].ToString() != "")
                        IEP8SciencePartcptPSSAModiWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8SciencePartcptPSSAModiWithFollowingAcmdtn"]);

                    if (Dt.Rows[0]["IEP8WritePartcptPSSAWithoutAcmdtn"].ToString() != "")
                        IEP8WritePartcptPSSAWithoutAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8WritePartcptPSSAWithoutAcmdtn"]);

                    if (Dt.Rows[0]["IEP8WritePartcptPSSAWithFollowingAcmdtn"].ToString() != "")
                        IEP8WritePartcptPSSAWithFollowingAcmdtn.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8WritePartcptPSSAWithFollowingAcmdtn"]);

                    if (Dt.Rows[0]["IEP8PSSAParticipate"].ToString() != "")
                        IEP8PSSAParticipate.Checked = Convert.ToBoolean(Dt.Rows[0]["IEP8PSSAParticipate"]);
                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage9()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        int alternateAssesment = 0;
        foreach (System.Web.UI.WebControls.ListItem item in CheckBoxListLocalAsssesment.Items)
        {
            item.Selected = false;
        }
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "SELECT IEP8AsmtStdtGrade,IEP8AsmtWithoutAcc,IEP8AssmtAcc,IEP8AsmtWithAcc,IEP9AlernativeAsmt,IEP9NoRegAsmt,IEP9AssmtAppropriate" +
                " from dbo.IEP_PE_Details where StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["IEP9AlernativeAsmt"].ToString() != "")
                        alternateAssesment = Convert.ToInt32(Dt.Rows[0]["IEP9AlernativeAsmt"]);
                    if (alternateAssesment == 1) Checkbox17.Checked = true; else Checkbox17.Checked = false;
                    foreach (DataRow dr in Dt.Rows)
                    {
                        TextBoxDetailsA.Text = Dt.Rows[0]["IEP8AssmtAcc"].ToString().Trim();
                        TextBoxDetailsA.Text = TextBoxDetailsA.Text.Replace("##", "'");
                        TextBoxDetailsA.Text = TextBoxDetailsA.Text.Replace("?bs;", "\\");
                        TextBoxDetailsB.Text = Dt.Rows[0]["IEP9NoRegAsmt"].ToString().Trim();
                        TextBoxDetailsB.Text = TextBoxDetailsB.Text.Replace("##", "'");
                        TextBoxDetailsB.Text = TextBoxDetailsB.Text.Replace("?bs;", "\\");
                        TextBoxDetailsC.Text = Dt.Rows[0]["IEP9AssmtAppropriate"].ToString().Trim();
                        TextBoxDetailsC.Text = TextBoxDetailsC.Text.Replace("##", "'");
                        TextBoxDetailsC.Text = TextBoxDetailsC.Text.Replace("?bs;", "\\");

                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtStdtGrade"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtStdtGrade"].ToString())))
                        {
                            foreach (System.Web.UI.WebControls.ListItem item in CheckBoxListLocalAsssesment.Items)
                            {
                                if (item.Value == "A")
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithoutAcc"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithoutAcc"].ToString())))
                        {
                            foreach (System.Web.UI.WebControls.ListItem item in CheckBoxListLocalAsssesment.Items)
                            {
                                if (item.Value == "B")
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                        if ((Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithAcc"].ToString() != "")) && (Convert.ToBoolean(Dt.Rows[0]["IEP8AsmtWithAcc"].ToString())))
                        {
                            foreach (System.Web.UI.WebControls.ListItem item in CheckBoxListLocalAsssesment.Items)
                            {
                                if (item.Value == "C")
                                {
                                    item.Selected = true;
                                }
                            }
                        }

                    }


                }
            }

        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage10()
    {
        objData = new clsData();
        clsIEP IEPObj = new clsIEP();
        sess = (clsSession)Session["UserSession"];
        try
        {

            string strQuery = "SELECT Benchmark FROM IEP_PE10_Benchmark WHERE StdIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    Benchmarkslist.DataSource = Dt;
                    Benchmarkslist.DataBind();
                }
            }
            Dt = new System.Data.DataTable();
            string getPageOneGridDetails = "select [Id],[MeasureAnualGoal],[StudentsProgress],[DescReportProgress],[ReportProgress] from [dbo].[StdtIEP_PE10_GoalsObj] where [StdtIEP_PEId]=" + sess.IEPId;
            System.Data.DataTable dt_goalDetails = objData.ReturnDataTable(getPageOneGridDetails, false);
            if (dt_goalDetails != null)
            {
                if (dt_goalDetails.Rows.Count > 0)
                {
                    Repeater5.DataSource = dt_goalDetails;
                    Repeater5.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage11()
    {
        Sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        objData = new clsData();
        try
        {
            string strQuery1 = "select [Id],[SchoolPerson],[Location],[Frequency],CONVERT(VARCHAR(50),[PrjBeginning],101) AS PrjBeginning,[AnticipatedDur],[Person] from [dbo].[IEP_PE11_SchoolPer] where [StdtIEP_PEId]=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Repeater9.DataSource = Dt;
                    Repeater9.DataBind();
                }
            }
            Dt = new System.Data.DataTable();

            string strQuery2 = "select [Id],[Service],[Location],[Frequency],CONVERT(VARCHAR(50),[PrjBeginning],101) AS PrjBeginning,[AnticipatedDur],[Person] from [dbo].[IEP_PE11_Service] where [StdtIEP_PEId]=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery2, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Repeater8.DataSource = Dt;
                    Repeater8.DataBind();
                }
            }
            Dt = new System.Data.DataTable();

            string strQuery3 = "select [Id],[SDI],[Location],[Frequency],CONVERT(VARCHAR(50),[PrjBeginning],101) AS PrjBeginning,[AnticipatedDur],[Person] from [dbo].[IEP_PE11_SDI] where [StdtIEP_PEId]=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery3, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Repeater7.DataSource = Dt;
                    Repeater7.DataBind();
                }
            }
            Dt = new System.Data.DataTable();
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage12()
    {
        sess = (clsSession)Session["UserSession"];
        intStdtId = Sess.StudentId;
        schoolId = Sess.SchoolId;
        int eligible = 0;
        int notEligible = 0;
        objData = new clsData();
        try
        {
            string strQuery1 = "select [Id],[SupportService] from [dbo].[IEP_PE12_SupportService] where [StdIEP_PEId]=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    rptrsupportServices.DataSource = Dt;
                    rptrsupportServices.DataBind();
                }
            }
            Dt = new System.Data.DataTable();

            string strQuery2 = "select [Id],[ESY],[Location],[Frequency],[PrjBeginning],[AnticipatedDur],[Person] from [dbo].[IEP_PE12_ESY] where [StdtIEP_PEId]=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery2, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Repeater10.DataSource = Dt;
                    Repeater10.DataBind();
                }
            }
            Dt = new System.Data.DataTable();

            string strQuery = "SELECT IEP12ElegibleForESY,IEP12ElegibleForESYInfo,IEP12NotElegibleForESY,IEP12NotElegibleForESYInfo,IEP12ShortTermObjectives FROM IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Label47.Text = Convert.ToString(Dt.Rows[0]["IEP12ElegibleForESYInfo"]).Trim().Replace("#gt;", ">").Replace("#lt;", "<");
                    Label48.Text = Convert.ToString(Dt.Rows[0]["IEP12NotElegibleForESYInfo"]).Trim().Replace("#gt;", ">").Replace("#lt;", "<");
                    Label49.Text = Convert.ToString(Dt.Rows[0]["IEP12ShortTermObjectives"]).Trim().Replace("#gt;", ">").Replace("#lt;", "<");
                    if (Dt.Rows[0]["IEP12ElegibleForESY"].ToString() != "")
                        eligible = Convert.ToInt32(Dt.Rows[0]["IEP12ElegibleForESY"]);
                    if (Dt.Rows[0]["IEP12NotElegibleForESY"].ToString() != "")
                        notEligible = Convert.ToInt32(Dt.Rows[0]["IEP12NotElegibleForESY"]);

                    if (eligible == 1) CheckBox18.Checked = true; else CheckBox18.Checked = false;
                    if (notEligible == 1) CheckBox19.Checked = true; else CheckBox19.Checked = false;
                }
            }

        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage13()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "SELECT IEP13RegularEdu,IEP13GeneralEdu FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Label50.Text = Dt.Rows[0]["IEP13RegularEdu"].ToString().Trim();
                    Label51.Text = Dt.Rows[0]["IEP13GeneralEdu"].ToString().Trim();

                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage14()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        int IEP14Itinerant = 0;
        int IEP14Supplemental = 0;
        int IEP14FullTime = 0;
        int IEP14AutisticSupport = 0;
        int IEP14Blind = 0;
        int IEP14Deaf = 0;
        int IEP14Emotional = 0;
        int IEP14Learning = 0;
        int IEP14LifeSkills = 0;
        int IEP14MultipleDisabilities = 0;
        int IEP14Physical = 0;
        int IEP14Speech = 0;
        int IEP14IsNeighbour = 0;
        int IEP14IsNeibhourNo = 0;
        int IEP14SpclEdu = 0;
        int IEP14Other = 0;
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "SELECT IEP14Itinerant,IEP14Supplemental,IEP14FullTime,IEP14AutisticSupport,IEP14Blind,IEP14Deaf,IEP14Emotional,IEP14Learning,IEP14LifeSkills, " +
                          "IEP14MultipleDisabilities,IEP14Physical,IEP14Speech,IEP14SchoolDistrict,IEP14SchoolBuilding,IEP14IsNeighbour,IEP14IsNeibhourNo,IEP14SpclEdu,IEP14Other FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    Label53.Text = Dt.Rows[0]["IEP14SchoolDistrict"].ToString().Trim();
                    Label52.Text = Dt.Rows[0]["IEP14SchoolBuilding"].ToString().Trim();
                    if (Dt.Rows[0]["IEP14Itinerant"].ToString() != "")
                        IEP14Itinerant = Convert.ToInt32(Dt.Rows[0]["IEP14Itinerant"]);
                    if (Dt.Rows[0]["IEP14Supplemental"].ToString() != "")
                        IEP14Supplemental = Convert.ToInt32(Dt.Rows[0]["IEP14Supplemental"]);
                    if (Dt.Rows[0]["IEP14FullTime"].ToString() != "")
                        IEP14FullTime = Convert.ToInt32(Dt.Rows[0]["IEP14FullTime"]);
                    if (Dt.Rows[0]["IEP14AutisticSupport"].ToString() != "")
                        IEP14AutisticSupport = Convert.ToInt32(Dt.Rows[0]["IEP14AutisticSupport"]);
                    if (Dt.Rows[0]["IEP14Blind"].ToString() != "")
                        IEP14Blind = Convert.ToInt32(Dt.Rows[0]["IEP14Blind"]);
                    if (Dt.Rows[0]["IEP14Deaf"].ToString() != "")
                        IEP14Deaf = Convert.ToInt32(Dt.Rows[0]["IEP14Deaf"]);
                    if (Dt.Rows[0]["IEP14Emotional"].ToString() != "")
                        IEP14Emotional = Convert.ToInt32(Dt.Rows[0]["IEP14Emotional"]);
                    if (Dt.Rows[0]["IEP14Learning"].ToString() != "")
                        IEP14Learning = Convert.ToInt32(Dt.Rows[0]["IEP14Learning"]);
                    if (Dt.Rows[0]["IEP14LifeSkills"].ToString() != "")
                        IEP14LifeSkills = Convert.ToInt32(Dt.Rows[0]["IEP14LifeSkills"]);
                    if (Dt.Rows[0]["IEP14MultipleDisabilities"].ToString() != "")
                        IEP14MultipleDisabilities = Convert.ToInt32(Dt.Rows[0]["IEP14MultipleDisabilities"]);
                    if (Dt.Rows[0]["IEP14Physical"].ToString() != "")
                        IEP14Physical = Convert.ToInt32(Dt.Rows[0]["IEP14Physical"]);
                    if (Dt.Rows[0]["IEP14Speech"].ToString() != "")
                        IEP14Speech = Convert.ToInt32(Dt.Rows[0]["IEP14Speech"]);
                    if (Dt.Rows[0]["IEP14IsNeighbour"].ToString() != "")
                        IEP14IsNeighbour = Convert.ToInt32(Dt.Rows[0]["IEP14IsNeighbour"]);
                    if (Dt.Rows[0]["IEP14IsNeibhourNo"].ToString() != "")
                        IEP14IsNeibhourNo = Convert.ToInt32(Dt.Rows[0]["IEP14IsNeibhourNo"]);
                    if (Dt.Rows[0]["IEP14SpclEdu"].ToString() != "")
                        IEP14SpclEdu = Convert.ToInt32(Dt.Rows[0]["IEP14SpclEdu"]);
                    if (Dt.Rows[0]["IEP14Other"].ToString() != "")
                        IEP14Other = Convert.ToInt32(Dt.Rows[0]["IEP14Other"]);

                    if (IEP14Itinerant == 1) chkItinerant.Checked = true; else chkItinerant.Checked = false;
                    if (IEP14Supplemental == 1) chkSupplemental.Checked = true; else chkSupplemental.Checked = false;

                    if (IEP14FullTime == 1) chkFullTime.Checked = true; else chkFullTime.Checked = false;
                    if (IEP14AutisticSupport == 1) chkAutistic.Checked = true; else chkAutistic.Checked = false;
                    if (IEP14Blind == 1) chkBlind.Checked = true; else chkBlind.Checked = false;
                    if (IEP14Deaf == 1) chkDeaf.Checked = true; else chkDeaf.Checked = false;
                    if (IEP14Emotional == 1) chkEmotional.Checked = true; else chkEmotional.Checked = false;
                    if (IEP14Learning == 1) chkLearning.Checked = true; else chkLearning.Checked = false;
                    if (IEP14LifeSkills == 1) chkLifeskills.Checked = true; else chkLifeskills.Checked = false;
                    if (IEP14MultipleDisabilities == 1) chkMultipleDis.Checked = true; else chkMultipleDis.Checked = false;
                    if (IEP14Physical == 1) chkPhysical.Checked = true; else chkPhysical.Checked = false;
                    if (IEP14Speech == 1) chkSpeech.Checked = true; else chkSpeech.Checked = false;
                    if (IEP14IsNeighbour == 1) chkYes.Checked = true; else chkYes.Checked = false;
                    if (IEP14IsNeibhourNo == 1) ChkNo.Checked = true; else ChkNo.Checked = false;
                    if (IEP14SpclEdu == 1) ChKSpecialEducation.Checked = true; else ChKSpecialEducation.Checked = false;
                    if (IEP14Other == 1) chkOther.Checked = true; else chkOther.Checked = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void FillIEPPage15()
    {
        objData = new clsData();
        Dt = new System.Data.DataTable();
        int IEP15RegularCls80 = 0;
        int IEP15Regular79 = 0;
        int IEP15Regular40 = 0;
        int IEP15ApprovePrivateSchool = 0;
        int IEP15OtherPublic = 0;
        int IEP15ApproveResidential = 0;
        int IEP15Hospital = 0;
        int IEP15PrivateFacility = 0;
        int IEP15CorrectionalFacility = 0;
        int IEP15PrivateResi = 0;
        int IEP15ChkoutState = 0;
        int IEP15PublicFacility = 0;
        int IEP15InstructionConducted = 0;
        sess = (clsSession)Session["UserSession"];
        try
        {
            string strQuery = "SELECT IEP15RegularCls80,IEP15Regular79,IEP15Regular40,IEP15ApprovePrivateSchool,IEP15ApprovePrivateText,IEP15OtherPublic,IEP15OtherPublicText," +
                        "IEP15ApproveResidential,IEP15ApproveResidentialText, " +
                           "IEP15Hospital,IEP15HospitalText,IEP15PrivateFacility,IEP15PrivateFacilityText,IEP15CorrectionalFacility,IEP15CorrectionText," +
                            " IEP15PrivateResi,IEP15PrivateResText,IEP15ChkoutState,IEP15ChkoutStateText,IEP15PublicFacility,IEP15PublicFacilityText,IEP15InstructionConducted,IEP15InstructionText FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["IEP15RegularCls80"].ToString() != "")
                        IEP15RegularCls80 = Convert.ToInt32(Dt.Rows[0]["IEP15RegularCls80"]);
                    if (Dt.Rows[0]["IEP15Regular79"].ToString() != "")
                        IEP15Regular79 = Convert.ToInt32(Dt.Rows[0]["IEP15Regular79"]);
                    if (Dt.Rows[0]["IEP15Regular40"].ToString() != "")
                        IEP15Regular40 = Convert.ToInt32(Dt.Rows[0]["IEP15Regular40"]);
                    if (Dt.Rows[0]["IEP15ApprovePrivateSchool"].ToString() != "")
                        IEP15ApprovePrivateSchool = Convert.ToInt32(Dt.Rows[0]["IEP15ApprovePrivateSchool"]);
                    if (Dt.Rows[0]["IEP15OtherPublic"].ToString() != "")
                        IEP15OtherPublic = Convert.ToInt32(Dt.Rows[0]["IEP15OtherPublic"]);
                    if (Dt.Rows[0]["IEP15ApproveResidential"].ToString() != "")
                        IEP15ApproveResidential = Convert.ToInt32(Dt.Rows[0]["IEP15ApproveResidential"]);
                    if (Dt.Rows[0]["IEP15Hospital"].ToString() != "")
                        IEP15Hospital = Convert.ToInt32(Dt.Rows[0]["IEP15Hospital"]);
                    if (Dt.Rows[0]["IEP15PrivateFacility"].ToString() != "")
                        IEP15PrivateFacility = Convert.ToInt32(Dt.Rows[0]["IEP15PrivateFacility"]);
                    if (Dt.Rows[0]["IEP15CorrectionalFacility"].ToString() != "")
                        IEP15CorrectionalFacility = Convert.ToInt32(Dt.Rows[0]["IEP15CorrectionalFacility"]);
                    if (Dt.Rows[0]["IEP15PrivateResi"].ToString() != "")
                        IEP15PrivateResi = Convert.ToInt32(Dt.Rows[0]["IEP15PrivateResi"]);
                    if (Dt.Rows[0]["IEP15ChkoutState"].ToString() != "")
                        IEP15ChkoutState = Convert.ToInt32(Dt.Rows[0]["IEP15ChkoutState"]);
                    if (Dt.Rows[0]["IEP15PublicFacility"].ToString() != "")
                        IEP15PublicFacility = Convert.ToInt32(Dt.Rows[0]["IEP15PublicFacility"]);
                    if (Dt.Rows[0]["IEP15InstructionConducted"].ToString() != "")
                        IEP15InstructionConducted = Convert.ToInt32(Dt.Rows[0]["IEP15InstructionConducted"]);


                    txtApprovePrivate.Text = Dt.Rows[0]["IEP15ApprovePrivateText"].ToString().Trim();
                    txtotherpublic.Text = Dt.Rows[0]["IEP15OtherPublicText"].ToString().Trim();
                    txtapproveresidential.Text = Dt.Rows[0]["IEP15ApproveResidentialText"].ToString().Trim();
                    txthospital.Text = Dt.Rows[0]["IEP15HospitalText"].ToString().Trim();
                    txtprivatefacility.Text = Dt.Rows[0]["IEP15PrivateFacilityText"].ToString().Trim();
                    txtcorrectfacility.Text = Dt.Rows[0]["IEP15CorrectionText"].ToString().Trim();
                    txtprivateresidential.Text = Dt.Rows[0]["IEP15PrivateResText"].ToString().Trim();
                    txtoutofstate.Text = Dt.Rows[0]["IEP15ChkoutStateText"].ToString().Trim();
                    txtpublicfacility.Text = Dt.Rows[0]["IEP15PublicFacilityText"].ToString().Trim();
                    txtinstructionalconduct.Text = Dt.Rows[0]["IEP15InstructionText"].ToString().Trim();



                    if (IEP15RegularCls80 == 1) chkregularcls80.Checked = true; else chkregularcls80.Checked = false;
                    if (IEP15Regular79 == 1) chkregularcls79.Checked = true; else chkregularcls79.Checked = false;
                    if (IEP15Regular40 == 1) chkregularcls40.Checked = true; else chkregularcls40.Checked = false;
                    if (IEP15ApprovePrivateSchool == 1) chkApproveprivateschool.Checked = true; else chkApproveprivateschool.Checked = false;
                    if (IEP15OtherPublic == 1) chkotherpublic.Checked = true; else chkotherpublic.Checked = false;
                    if (IEP15ApproveResidential == 1) chkapproveresidential.Checked = true; else chkapproveresidential.Checked = false;
                    if (IEP15Hospital == 1) chkhospital.Checked = true; else chkhospital.Checked = false;
                    if (IEP15PrivateFacility == 1) chkprivatefacility.Checked = true; else chkprivatefacility.Checked = false;
                    if (IEP15CorrectionalFacility == 1) chkcorrectionalfacility.Checked = true; else chkcorrectionalfacility.Checked = false;
                    if (IEP15PrivateResi == 1) chkprivateresidential.Checked = true; else chkprivateresidential.Checked = false;
                    if (IEP15ChkoutState == 1) chkoutofstate.Checked = true; else chkoutofstate.Checked = false;
                    if (IEP15PublicFacility == 1) chkpublicfacility.Checked = true; else chkpublicfacility.Checked = false;
                    if (IEP15InstructionConducted == 1) chkInstructionconducted.Checked = true; else chkInstructionconducted.Checked = false;

                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        tdMsgExportNew.InnerHtml = "";

        try
        {

            if (objData.IFExists("Select * from binaryFiles  Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And IEPId=" + sess.IEPId + " And Type='BW' And ModuleName='IEP'") == false)
            {
                ExportAll();
            }
            else
            {

                string fileName = "", contentType = "";
                clsDocumentasBinary objBinary = new clsDocumentasBinary();
                string strQuery = "Select Data,ContentType,DocumentName from binaryFiles Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And IEPId=" + sess.IEPId + " And Type='BW'  And ModuleName='IEP'";
                byte[] bytes = objBinary.getDocument(strQuery, out contentType, out fileName);
                objBinary.ShowDocument(fileName, bytes, contentType);
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
        }


    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        LoadData();
    }

    //******************************************************************************************************
    protected System.Data.DataTable getGoalData4()
    {
        //Page Number 4 
        System.Data.DataTable Dt = null;
        objData = new clsData();

        Dt = new System.Data.DataTable();
        string strQuery = "select distinct  Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEP_PEId=" + sess.IEPId + " and StdtLessonPlan.IncludeIEP=1";
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

        strQuery = "SELECT    dbo.StdtLessonPlan.StudentId,dbo.LessonPlan.LessonPlanName,(SELECT StdtGoalId FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEP_PEId=" + sess.IEPId + ") StdtGoalId, dbo.StdtLessonPlan.GoalId,StdtIEP_PE.AsmntYearId,(SELECT IEPGoalNo FROM StdtGoal WHERE GoalId=StdtLessonPlan.GoalId AND StdtIEP_PEId=" + sess.IEPId + ") IEPGoalNo,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, dbo.Goal.GoalName   FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId inner join dbo.StdtIEP_PE ON dbo.StdtLessonPlan.StdtIEP_PEId=dbo.StdtIEP_PE.StdtIEP_PEId where StdtLessonPlan.GoalId in (" + GoalIdZ + ") AND dbo.StdtLessonPlan.StdtIEP_PEId =  '" + sess.IEPId + "'    ORDER BY dbo.StdtLessonPlan.GoalId ";


        Dt = objData.ReturnDataTable(strQuery, false);


        System.Data.DataTable dtRep = Dt;
        dtRep.Columns.Remove("StudentId");
        dtRep.Columns.Remove("StdtGoalId");
        dtRep.Columns.Remove("AsmntYearId");
        //  dtRep.Columns.Remove("GoalId");
        return dtRep;
    }




    [STAThread]
    public static void ConvertHTMLTOWORD(string url)
    {

        var file = new FileInfo(url);
        Microsoft.Office.Interop.Word.Application app
            = new Microsoft.Office.Interop.Word.Application();
        try
        {
            app.Visible = true;
            object missing = Missing.Value;
            object visible = true;
            _Document doc = app.Documents.Add(ref missing,
                     ref missing,
                     ref missing,
                     ref visible);
            var bookMark = doc.Words.First.Bookmarks.Add("entry");
            bookMark.Range.InsertFile(file.FullName);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            app.Quit();
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

            string path = Server.MapPath("~\\Administration") + "\\IEPTemp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid g;

            g = Guid.NewGuid();

            sess = (clsSession)Session["UserSession"];
            string newpath = path + "\\";
            string ids = g.ToString();
            ids = ids.Replace("-", "");

            string newFileName = "IEPTemporyTemplate" + ids.ToString();
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

            // tdMsg1.InnerHtml = clsGeneral.warningMsg(Ex.Message.ToString() + "Directory or File already Exit !");
            return "";
        }
    }

    static int xmlcolumn = 0;


    private void ExportToWord(string htmlData, string newPath)
    {
        try
        {
            StringBuilder strBody = new StringBuilder();
            strBody.Append(htmlData);
            Response.AppendHeader("Content-Type", "application/msword");
            Response.AppendHeader("Content-disposition", "attachment; filename=IEPDocument.doc");
            Response.Write(strBody);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string replaceWithTexts(string HtmlData, string[] plcT, string[] TextT)
    {
        TimeSpan tempDatetime;
        int count = plcT.Count();

        for (int i = 0; i < count; i++)
        {
            if (TextT[i] != null)
            {
                TextT[i] = TextT[i].Replace("##", "'");
                TextT[i] = TextT[i].Replace("?bs;", "\\");
                if (plcT[i] == "plcDOB")
                {
                    DateTime Datetime = Convert.ToDateTime(TextT[i]);

                    TextT[i] = Datetime.ToShortDateString();
                }
                HtmlData = HtmlData.Replace(plcT[i], TextT[i]);
            }
            else
            {
                if (plcT[i] == "plcAge")
                {
                    tempDatetime = DateTime.Now - Convert.ToDateTime(TextT[2].ToString());
                    double dats = tempDatetime.TotalDays;
                    int age = Convert.ToInt32(dats / 360);
                    HtmlData = HtmlData.Replace(plcT[i], age.ToString());
                }
                HtmlData = HtmlData.Replace(plcT[i], "");
            }
        }
        return HtmlData;
    }

    public static void PageConvert(string input, string output, WdSaveFormat format)
    {
        try
        {
            // Create an instance of Word.exe
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();

            // Make this instance of word invisible (Can still see it in the taskmgr).
            oWord.Visible = false;

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object bConfirmDialog = false;
            object isVisible = true;
            object readOnly = false;
            object oInput = input;
            object oOutput = output;
            object oFormat = format;
            object oFileShare = true;

            // Load a document into our instance of word.exe
            Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Open(ref oInput, ref bConfirmDialog, ref readOnly, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Make this document the active document.
            oDoc.Activate();

            // Save this document in Word 2003 format.
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);


        }
        catch (IOException ex)
        {
            throw ex;
        }




    }

    public static String URLTOHTML(string Url)
    {
        string result = "";
        try
        {

            using (StreamReader reader = new StreamReader(Url))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    result += line;
                }
            }
        }
        catch (IOException ex)
        {
            throw ex;
        }
        return result;

    }

    public void ExportAll()
    {

        Hashtable ht = new Hashtable();
        clsDocumentasBinary objBinary = new clsDocumentasBinary();
        //  ht = bindData();
        string[] plcT, TextT, plcC, chkC;
        string strQry = "SELECT count(*) as Count FROM sys.columns AS c INNER JOIN sys.types AS t ON c.user_type_id=t.user_type_id INNER JOIN sys.tables ts"
            + " ON ts.OBJECT_ID = c.OBJECT_ID Where  t.name='bit' And ts.name In ('StdtIEP_PE','StdtIEP_PEExt1','StdtIEP_PEExt2','StdtIEP_PEExt3')";
        string[] totChkBox = new string[68];
        string Path = Server.MapPath("~\\Administration\\IEPTemplates\\PA IEP Template.docx");
        string TemporyPath = Server.MapPath("~\\Administration\\IEPTemp\\");
        string NewPath = CopyTemplate(Path, "0");
        int x = 0, count = 0, lastCount = 0;

        for (int k = 1; k < 17; k++)
        {
            //  k = 13;
            if (k != 9)
            {
                // count=chkC.Length;
                CreateQuery1("NE", "..\\Administration\\XMlIEP_PA\\IEP_PA" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                chkC.CopyTo(totChkBox, count);
                count += lastCount;
                //if (!ht.ContainsKey(k))
                //{
                //    ht.Add(k, chkC);
                //}
                // 
            }
        }
        setCheckBox(NewPath, ht, totChkBox);
        Guid g = Guid.NewGuid();

        sess = (clsSession)Session["UserSession"];
        string ids = g.ToString();

        Thread thread = new Thread(new ThreadStart(WorkThreadFunction));

        string hPath = Server.MapPath("~\\Administration") + "\\IEPTemp\\HTML" + ids + ".html";
        PageConvert(NewPath, hPath, WdSaveFormat.wdFormatHTML);

        System.Threading.Thread.Sleep(7000);


        thread.Abort();

        string HtmlFileName = "";
        string HtmlData = objBinary.ConvertToHtml(NewPath, TemporyPath, out HtmlFileName);

        string Temp = Server.MapPath("~\\Administration") + "\\XMlIEP_PA\\";

        string[] filePaths = Directory.GetFiles(Temp);
        string[] totCkeckColums = new string[0];




        for (int k = 1; k < 17; k++)
        {
            if (k != 9)
            {

                CreateQuery1("NE", "..\\Administration\\XMlIEP_PA\\IEP_PA" + k + ".xml", k, out plcT, out  TextT, out plcC, out chkC, true, out lastCount);
                HtmlData = replaceWithTexts(HtmlData, plcT, TextT);
            }
        }

        string strquery = "";
        string fileName = "";
        clsData objData = new clsData();
        strquery = "Select 'IEP Doc :'+ S.StudentLname +'-'+AY.AsmntYearDesc+'-V'+IE.Version from StdtIEP_PE IE Inner Join AsmntYear AY on IE.AsmntYearId=AY.AsmntYearId INNER JOIN Student S ON IE.StudentId =S.StudentId  Where IE.StdtIEP_PEId=" + sess.IEPId + " And IE.StudentId=" + sess.StudentId + " And IE.SchoolId=" + sess.SchoolId + " ";
        fileName = Convert.ToString(objData.FetchValue(strquery));
        fileName = fileName + ".doc";
        byte[] contents = objBinary.ConvertToByte(HtmlData, TemporyPath, HtmlFileName);

        strquery = "select version from stdtiep where  schoolid=" + sess.SchoolId + " and studentid=" + sess.StudentId + " and stdtiepid=" + sess.IEPId + " ";
        object ver = objData.FetchValue(strquery);
        string ver1 = "";
        if (ver != null) ver1 = Convert.ToString(ver);
        if (ver1 == "") ver1 = "0";
        strquery = "select lookupid from LookUp where LookupName='IEP' ";
        object lookupId = objData.FetchValue(strquery);
        int lookupId1 = Convert.ToInt32(lookupId);
        int docId = objBinary.saveDoc(sess.SchoolId, sess.StudentId, sess.LoginId, lookupId1);
        int binaryid = objBinary.saveDocument(contents, fileName, ver1, "BW", docId, sess.IEPId, "IEP", sess.SchoolId, sess.StudentId, sess.LoginId);
        //objBinary.ShowDocument(fileName, contents, "application/msword");
        FileInfo f1 = new FileInfo(NewPath);
        if (f1.Exists)
        {
            f1.Delete();
        }

        f1 = new FileInfo(hPath);
        if (f1.Exists)
        {
            f1.Delete();
        }
        System.Threading.Thread.Sleep(7000);
        string path = Server.MapPath("~\\Administration") + "\\IEPTemp";
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(path);
        //foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        //{
        //    file.Delete();
        //}
        //foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
        //{
        //    dir.Delete(true);
        //}



    }

    public void WorkThreadFunction()
    {
        try
        {

            Thread.Sleep(5000);
        }
        catch (Exception ex)
        {
            // log errors
        }
    }

    private Hashtable bindData()
    {
        Hashtable htColumns = new Hashtable();
        sess = (clsSession)Session["UserSession"];
        clsExport objExport = new clsExport();
        string[] data;
        string[] data2;

        objExport.getIEP1(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(1))
        {
            htColumns.Add(1, data2);
            data2 = null;
        }
        objExport.getIEP2(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(2))
        {
            htColumns.Add(2, data2);
            data2 = null;
        }
        objExport.getIEP3(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (!htColumns.ContainsKey(3))
        {
            htColumns.Add(3, data2);
            data2 = null;
        }

        System.Data.DataTable dtIEP4 = new System.Data.DataTable();
        bool Odd = false;
        dtIEP4 = objExport.getIEP4Data(sess.StudentId, sess.SchoolId, sess.IEPId, out Odd);
        int RowsCount = dtIEP4.Rows.Count;
        dtIEP4.TableName = "Table";

        for (int Round = 0; Round < dtIEP4.Rows.Count; Round += 2)
        {
            System.Data.DataTable Dt4 = new System.Data.DataTable();
            Dt4 = objExport.ReturnRows(Round, dtIEP4);
            objExport.getIEP4(out data, Dt4);
        }
        if (!htColumns.ContainsKey(4))
        {
            htColumns.Add(4, data2);
            data2 = null;
        }
        objExport.getIEP5(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(5))
        {
            htColumns.Add(5, data2);
            data2 = null;
        }

        objExport.getIEP6(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (!htColumns.ContainsKey(6))
        {
            htColumns.Add(6, data2);
            data2 = null;
        }

        objExport.getIEP7(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        if (!htColumns.ContainsKey(7))
        {
            htColumns.Add(7, data2);
            data2 = null;
        }
        objExport.getIEP8(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (!htColumns.ContainsKey(8))
        {
            htColumns.Add(8, data2);
            data2 = null;
        }
        return htColumns;
    }

    private string[] getCheckBoxes(string StateName, string Path, int PageNo, string[] chkData)
    {
        string[] chkC = new string[0];

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");

        checkCount = 0;

        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                int chkCount = 0;

                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        chkCount++;
                    }
                }

                chkC = new string[chkCount];

                columns = getColumns(PageNo, chkCount + 1);

                int j = 0, l = 0;


                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        checkCount++;

                        chkC[j] = columns[l];
                        j++;
                        //l++;
                    }
                    else
                    {
                        j++;
                        //l++;
                    }
                }
            }
        }
        Array.Copy(chkC, chkData, chkC.Length);
        //return chkData.Join<Array>(chkC[j], chkData);

        return chkData;
    }

    private void CreateQuery1(string StateName, string Path, int PageNo, out string[] plcT, out string[] TextT, out string[] plcC, out string[] chkC, bool check, out int lastValue)
    {



        lastValue = 0;
        chkC = new string[0];
        plcC = new string[0];

        TextT = new string[0];
        plcT = new string[0];

        //  if (PageNo == 4) return;
        string[] textValues;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath(Path));

        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("State");
        checkCount = 0;
        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == StateName)
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;

                int chkCount = 0, textCount = 0;

                foreach (XmlNode stMs in xmlListColumns)
                {
                    if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                    {
                        chkCount++;
                    }
                    else
                    {
                        textCount++;
                    }
                }



                chkC = new string[chkCount];
                plcC = new string[chkCount];

                TextT = new string[textCount];
                plcT = new string[textCount];

                columns = getColumns(PageNo, textCount);


                int j = 0, k = 0, l = 0;

                if (check == true)
                {
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                            chkC[j] = columns[l];
                            plcC[j] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            j++;
                            lastValue++;
                        }
                        else
                        {
                            TextT[k] = columns[l];
                            plcT[k] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            k++;
                        }
                        l++;
                    }
                }
                else
                {
                    foreach (XmlNode stMs in xmlListColumns)
                    {
                        if (stMs.Attributes["PlaceHolder"].Value == "abcdefgh")
                        {
                            checkCount++;
                            chkC[j] = columns[l];
                            plcC[j] = stMs.Attributes["PlaceHolder"].Value.ToString().Trim();
                            j++;
                        }
                        else
                        {
                            j++;
                        }


                    }
                }
                columns = null;
            }
        }

    }

    private string[] getColumns(int PageNo, int Count)
    {
        sess = (clsSession)Session["UserSession"];
        clsExport objExport = new clsExport();
        string[] data = new string[Count];
        string[] data2 = new string[2];
        string temp = "";
        int counter = 0;
        if (PageNo == 1) objExport.getIEP_PE1(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 2) objExport.getIEP_PE2(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 3) objExport.getIEP_PE3(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 4) objExport.getIEP_PE4(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        //if (PageNo == 4)
        //{
        //    System.Data.DataTable dtIEP4 = new System.Data.DataTable();
        //    bool Odd = false;

        //    dtIEP4 = objExport.getIEP4Data(sess.StudentId, sess.SchoolId, sess.IEPId, out Odd);
        //    int RowsCount = dtIEP4.Rows.Count;
        //    dtIEP4.TableName = "Table";

        //    for (int Round = 0; Round < dtIEP4.Rows.Count; Round += 2)
        //    {

        //        System.Data.DataTable Dt4 = new System.Data.DataTable();
        //        Dt4 = objExport.ReturnRows(Round, dtIEP4);
        //        objExport.getIEP4(out data, Dt4);


        //    }

        //}

        if (PageNo == 5) objExport.getIEP_PE5(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 6) objExport.getIEP_PE6(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (PageNo == 7) objExport.getIEP_PE7(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 8) objExport.getIEP_PE8(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        //if (PageNo == 9) objExport.getIEP_PE9(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 10) objExport.getIEP_PE10(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId, 67);
        if (PageNo == 11) objExport.getIEP_PE11(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 12) objExport.getIEP_PE12(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 13) objExport.getIEP_PE13(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 14) objExport.getIEP_PE14(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 15) objExport.getIEP_PE15(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);
        if (PageNo == 16) objExport.getIEP_PE16(out data, out data2, sess.StudentId, sess.SchoolId, sess.IEPId);

        return data;
    }

    public void SearchAndReplace(string document)
    {
        int m = 0;


        string col = "";
        string plc = "";

        columnsCheck = new string[checkCount];

        for (int i = 0; i < columns.Length; i++)
        {
            plc = placeHolders[i].ToString().Trim();
            col = columns[i].ToString().Trim();


            if (plc == "abcdefgh")
            {
                columnsCheck[m] = col;
                m++;
            }
            else
            {

                if (col == null) col = "";

                replaceWithHtml(document, plc, col);
            }
        }
        if (document.Contains("IEP1") == false)
        {
            if (document.Contains("IEP5") == false)
            {
                if (document.Contains("IEP8") == false)
                {

                    using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
                    {
                        MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                        NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
                        Body body = mainPart.Document.Body;

                        DocumentFormat.OpenXml.Wordprocessing.Paragraph para = new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run((new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = DocumentFormat.OpenXml.Wordprocessing.BreakValues.Page })));


                        mainPart.Document.Body.InsertAfter(para, mainPart.Document.Body.LastChild);
                        mainPart.Document.Save();
                    }
                }
            }
        }



    }

    private void setCheckBox(string document, Hashtable ht, string[] columnsChks)
    {

        bool IsCheck = false;
        int i = 0;

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        {

            foreach (DocumentFormat.OpenXml.Wordprocessing.CheckBox cb in wordDoc.MainDocumentPart.Document.Body.Descendants<DocumentFormat.OpenXml.Wordprocessing.CheckBox>())
            {
                if (cb != null)
                {

                    FormFieldName cbName = cb.Parent.ChildElements.First<FormFieldName>();

                    try
                    {
                        DefaultCheckBoxFormFieldState defaultState = cb.GetFirstChild<DefaultCheckBoxFormFieldState>();
                        if (i < columnsChks.Length)
                        {
                            if (columnsChks[i] != "") IsCheck = Convert.ToBoolean(columnsChks[i]);
                            if (IsCheck == true) defaultState.Val = true;
                        }
                        i++;
                    }
                    catch
                    {

                    }
                }
            }
        }
    }

    public void replaceWithHtml(string fileName, string replace, string replaceTest)
    {

        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            NotesFor.HtmlToOpenXml.HtmlConverter converter = new NotesFor.HtmlToOpenXml.HtmlConverter(mainPart);
            Body body = mainPart.Document.Body;

            ParagraphProperties paragraphProperties = new ParagraphProperties();

            DocumentFormat.OpenXml.Wordprocessing.Paragraph par = wordDoc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().ElementAt(0); //hardcoded a paragraph index for testing

            paragraphProperties.RemoveAllChildren<ParagraphStyleId>();
            paragraphProperties.RemoveAllChildren<SpacingBetweenLines>();
            //      paragraphProperties.TextAlignment.

            if (replaceTest == "") replaceTest = "&nbsp;";
            if (replaceTest != "") replaceTest = replaceTest.Trim();
            try
            {

                var placeholder = mainPart.Document.Body
                  .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                  .Where(t => t.Text.Contains(replace)).First();
                string[] ParaList = replaceTest.Split(new string[] { "\n\n" }, StringSplitOptions.None);
                string orginal = "";
                IList<OpenXmlCompositeElement> element = new List<OpenXmlCompositeElement>();
                foreach (var item in ParaList)
                {
                    var paragraphs = converter.Parse(item);
                    Run[] runArr = par.Descendants<Run>().ToArray();

                    // foreach each run

                    foreach (var eachel in paragraphs)
                    {
                        //if (eachel.InnerXml.Contains("table"))
                        //{
                        //}

                        element.Add(eachel);
                    }
                }
                bool flag = false;

                string txt = "";
                char vt = (char)13;

                //         OpenXmlElement elm;
                //     Microsoft.Office.Interop.Word.Paragraph para;
                string modifiedString = "";
                var parent = placeholder.Parent;
                for (int i = 0; i < element.Count; i++)
                {
                    if (element.Count == 1)
                    {
                        parent.ReplaceChild(element[i], placeholder);
                    }
                    else if (element[i].InnerText.Trim() != "")
                    {
                        Run text = (Run)placeholder.Descendants();
                        //foreach (Run run in text)
                        //{
                        string innerText = text.InnerText;
                        modifiedString = text.InnerText.Replace(innerText, "My New Text");
                        // if the InnerText doesn't modify
                        if (modifiedString != text.InnerText)
                        {
                            Text t = new Text(modifiedString);
                            text.RemoveAllChildren<Text>();
                            text.AppendChild<Text>(t);
                        }
                        // }
                        txt = txt + element[i].InnerText + vt;

                        //     elm.InnerText.Concat(elm.InnerText, element[i].InnerText);
                    }
                    // element[i].Append(paragraphProperties);

                }


                // get all runs under the paragraph and convert to be array






                if (element.Count > 1)
                {

                    // element[0].InnerText = txt;
                    placeholder.Text = txt;
                    flag = true;

                    //parent.ReplaceChild(element[i], placeholder);
                }
                //if (!flag)
                //{
                //    parent.ReplaceChild(element[i], placeholder);
                //}
                mainPart.Document.Save();
            }
            catch (Exception eX)
            {
                //dddd.InnerHtml = "";
                //tdMsgExportNew.InnerHtml = clsGeneral.failedMsg(eX.Message.ToString() + "....Failed !");
                ClientScript.RegisterStartupScript(GetType(), "", "alert('Failed To Export Data..!')", true);
            }
            /**/


            //  byte[] byteArray = File.ReadAllBytes(DocxFilePath);


            /**/



        }
    }




    //******************************************************************************************************

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        downloadfile();
    }

    public void btnDone_Click(object sender, EventArgs e)
    {
        //tdMsgExportNew.InnerHtml = "";
        string path = Server.MapPath("~\\Administration") + "\\IEPMerged";
        Array.ForEach(Directory.GetFiles(path), File.Delete);
        string Temp = Server.MapPath("~\\Administration") + "\\Temp\\";

        if (Directory.Exists(Temp))
        {
            Directory.Delete(Temp, true);
        }
        ClientScript.RegisterStartupScript(GetType(), "", "DownloadDone();", true);

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

        }
        ViewState["FileName"] = "";
    }

    protected void btnSign_Click(object sender, EventArgs e)
    {
        sess = sess = (clsSession)Session["UserSession"];
        clsSignature objSign = new clsSignature();
        objSign.SignDocument(sess.StudentId, sess.IEPId, sess.SchoolId, sess.LoginId);
    }
    protected void btnSignDetails_Click(object sender, EventArgs e)
    {
        sess = sess = (clsSession)Session["UserSession"];
        clsSignature objSign = new clsSignature();
        string users = objSign.getSignedUsers(sess.StudentId, sess.IEPId, sess.SchoolId);
        string[] temp = users.Split('|');
        string[] signeduser = new string[temp.Length - 1];
        int i = 0;

        foreach (var item in temp)
        {

            if (item != "")
            {
                string[] username = item.Split('=');
                signeduser[i] = username[1];
            }
            i++;
        }


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Server.MapPath("~/StudentBinder/XMLSign/signXML.xml"));
        string[] userXML = null;
        string[] certificateXMl = null;
        XmlNodeList xmlList = null;
        xmlList = xmlDoc.GetElementsByTagName("Document");
        checkCount = 0;
        int index = 0;
        foreach (XmlNode st in xmlList)
        {
            if (st.Attributes["Name"].Value == "NEIEP")
            {
                XmlNodeList xmlListColumns = null;
                xmlListColumns = st.ChildNodes.Item(0).ChildNodes;
                userXML = new string[xmlListColumns.Count];
                certificateXMl = new string[xmlListColumns.Count];
                foreach (XmlNode stMs in xmlListColumns)
                {
                    userXML[index] = stMs.Attributes["Role"].Value;
                    certificateXMl[index] = stMs.Attributes["signature"].Value;
                    index++;
                }
            }
        }

        bool[] signed = new bool[certificateXMl.Length];


        string table = "<table style='width:100%'><tr><th>User Roles</th><th>Sign Status</th></tr>";
        for (int indexi = 0; indexi < certificateXMl.Length; indexi++)
        {
            for (int indexj = 0; indexj < signeduser.Length; indexj++)
            {
                if (certificateXMl[indexi].ToString() == signeduser[indexj].ToString())
                {
                    signed[indexi] = true;

                    break;
                }

            }

        }

        for (int isign = 0; isign < signed.Length; isign++)
        {
            if (signed[isign] == true)

                table = table + "<tr><td style='text-align:center'>" + userXML[isign].ToString() + "</td><td style='text-align:center'>Signed</td></tr>";

            else
                table = table + "<tr><td style='text-align:center'>" + userXML[isign].ToString() + "</td><td style='text-align:center'>Unsigned</td></tr>";
        }
        table = table + "</table>";
        tdUSers.InnerHtml = table;
        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "listUsers();", true);
    }

    #region BSP Form View 05-05-2015 Hari

    protected void btnBSP_Click(object sender, EventArgs e)
    {

        try
        {
            Sess = (clsSession)Session["UserSession"];
            if (Sess != null)
            {
                int headerId = Sess.IEPId;
                FillDoc(headerId);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){Prompt();});", true);
            }
        }
        catch (Exception EX)
        {

            throw EX;
        }
    }

    private void FillDoc(int stdtIEPID)
    {
        try
        {
            divMessage.InnerHtml = "";
            objData = new clsData();
            string strQuery = "";
            strQuery = "Select ROW_NUMBER() OVER (ORDER BY BSPDoc) AS No,BSPDocUrl as Document, BSPDoc FROM BSPDoc Where BSPDocUrl<>'' And StdtIEPId = " + stdtIEPID + "";
            System.Data.DataTable Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    grdFile.DataSource = Dt;
                    grdFile.DataBind();
                }
                else
                    divMessage.InnerHtml = clsGeneral.warningMsg("No Data Found");
            }
        }
        catch (Exception)
        {


        }
    }
    protected void grdFile_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void grdFile_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        string file = Convert.ToString(e.CommandArgument);
        clsData objData = new clsData();
        if (e.CommandName == "download")
        {
            if (Sess != null)
            {
                try
                {
                    HttpResponse Response = HttpContext.Current.Response;
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Byte[] data = (Byte[])objData.FetchValue("SELECT Data FROM BSPDoc WHERE BSPDoc='" + file + "'");
                    string docURL = Convert.ToString(objData.FetchValue("SELECT BSPDocUrl FROM BSPDoc WHERE BSPDoc='" + file + "'"));
                    string contentType = GetContentType(Path.GetExtension(docURL).ToLower().ToString());
                    Response.AddHeader("Content-type", contentType);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + docURL);
                    Response.BinaryWrite(data);
                    Response.Flush();
                    Response.End();
                }
                catch (Exception Ex)
                {

                    throw Ex;
                }
            }
        }

    }

    private string GetContentType(string extension)
    {
        try
        {
            string ContentType = "";
            switch (extension)
            {
                case ".txt":
                    ContentType = "text/plain";
                    break;
                case ".doc":
                    ContentType = "application/msword";
                    break;
                case ".docx":
                    ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".pdf":
                    ContentType = "application/pdf";
                    break;
                case ".xls":
                    ContentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".csv":
                    ContentType = "application/vnd.ms-excel";
                    break;
            }
            return ContentType;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    protected void grdFile_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void grdFile_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
}
    #endregion