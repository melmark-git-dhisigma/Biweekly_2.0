using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP_PE14 : System.Web.UI.Page
{
    DataTable DTRoles = null;
    public clsData objData = null;
    string strQuery = "";
    DataTable Dt = null;
    DataClass oData = null;
    static string x = "", y = "";
    public clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        clsIEP IEPObj = new clsIEP();
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        if (!IsPostBack)
        {
            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }

            ViewAccReject();
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }

            fillBasicDetails();

           
        }
    }

    private void ViewAccReject()
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        int reject = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Rejected'");
        int approve = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        if (sess.IEPStatus == approve || sess.IEPStatus == reject)
        {
            btnSave.Visible = false;
        }

    }

    private void fillBasicDetails()
    {
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
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
            //display student name
            DataTable dataStud = new DataTable();
            string studentName = "select ST.StudentLname+','+ST.StudentFname as StudentName from Student ST  where StudentId=" + sess.StudentId + ""
                        + "and SchoolId=" + sess.SchoolId;
            dataStud = objData.ReturnDataTable(studentName, false);
            if (dataStud != null)
            {
                if (dataStud.Rows.Count > 0)
                {
                    lblStudentName.Text = dataStud.Rows[0]["StudentName"].ToString().Trim();
                }
            }

            strQuery = "SELECT IEP14Itinerant,IEP14Supplemental,IEP14FullTime,IEP14AutisticSupport,IEP14Blind,IEP14Deaf,IEP14Emotional,IEP14Learning,IEP14LifeSkills, "+
                          "IEP14MultipleDisabilities,IEP14Physical,IEP14Speech,IEP14SchoolDistrict,IEP14SchoolBuilding,IEP14IsNeighbour,IEP14IsNeibhourNo,IEP14SpclEdu,IEP14Other,IEP14OtherDesc FROM dbo.IEP_PE_Details WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    TxtSchoolDisr.Text = Dt.Rows[0]["IEP14SchoolDistrict"].ToString().Trim();
                    TextBuilding.Text = Dt.Rows[0]["IEP14SchoolBuilding"].ToString().Trim();
                    TextBoxOther.Text = Dt.Rows[0]["IEP14OtherDesc"].ToString().Trim();
                    if (Dt.Rows[0]["IEP14Itinerant"].ToString() !="")
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
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
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            if (chkItinerant.Checked == true) IEP14Itinerant = 1; else IEP14Itinerant = 0;
            if (chkSupplemental.Checked == true) IEP14Supplemental = 1; else IEP14Supplemental = 0;

            if (chkFullTime.Checked == true) IEP14FullTime = 1; else IEP14FullTime = 0;
            if (chkAutistic.Checked == true) IEP14AutisticSupport = 1; else IEP14AutisticSupport = 0;
            if (chkBlind.Checked == true) IEP14Blind = 1; else IEP14Blind = 0;
            if (chkDeaf.Checked == true) IEP14Deaf = 1; else IEP14Deaf = 0;
            if (chkEmotional.Checked == true) IEP14Emotional = 1; else IEP14Emotional = 0;
            if (chkLearning.Checked == true) IEP14Learning = 1; else IEP14Learning = 0;
            if (chkLifeskills.Checked == true) IEP14LifeSkills = 1; else IEP14LifeSkills = 0;
            if (chkMultipleDis.Checked == true) IEP14MultipleDisabilities = 1; else IEP14MultipleDisabilities = 0;
            if (chkPhysical.Checked == true) IEP14Physical = 1; else IEP14Physical = 0;
            if (chkSpeech.Checked == true) IEP14Speech = 1; else IEP14Speech = 0;
            if (chkYes.Checked == true) IEP14IsNeighbour = 1; else IEP14IsNeighbour = 0;

            if (ChkNo.Checked == true) IEP14IsNeibhourNo = 1; else IEP14IsNeibhourNo = 0;
            if (ChKSpecialEducation.Checked == true) IEP14SpclEdu = 1; else IEP14SpclEdu = 0;
            if (chkOther.Checked == true) IEP14Other = 1; else IEP14Other = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP14Itinerant=" + IEP14Itinerant + ",IEP14Supplemental=" + IEP14Supplemental + ",IEP14FullTime=" + IEP14FullTime + ",IEP14AutisticSupport = " + IEP14AutisticSupport + "," +
                        "IEP14Blind = " + IEP14Blind + ",IEP14Deaf = " + IEP14Deaf + ",IEP14Emotional = " + IEP14Emotional + ",IEP14Learning = " + IEP14Learning + ",IEP14LifeSkills = " + IEP14LifeSkills + ",IEP14MultipleDisabilities = " + IEP14MultipleDisabilities + ",IEP14Physical = " + IEP14Physical + ",IEP14Speech = " + IEP14Speech + ",IEP14SchoolDistrict = '" + TxtSchoolDisr.Text + "',IEP14SchoolBuilding = '" + TextBuilding .Text+ "'"+
                        ",IEP14IsNeighbour = " + IEP14IsNeighbour + ",IEP14IsNeibhourNo = " + IEP14IsNeibhourNo + ",IEP14SpclEdu = " + IEP14SpclEdu + ",IEP14Other = " + IEP14Other + ",IEP14OtherDesc='" + TextBoxOther.Text + "' WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus();
                //fillBasicDetails();
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
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
        objData = new clsData();
        oData = new DataClass();
        Dt = new DataTable();
        sess = (clsSession)Session["UserSession"];
        try
        {
            if (chkItinerant.Checked == true) IEP14Itinerant = 1; else IEP14Itinerant = 0;
            if (chkSupplemental.Checked == true) IEP14Supplemental = 1; else IEP14Supplemental = 0;

            if (chkFullTime.Checked == true) IEP14FullTime = 1; else IEP14FullTime = 0;
            if (chkAutistic.Checked == true) IEP14AutisticSupport = 1; else IEP14AutisticSupport = 0;
            if (chkBlind.Checked == true) IEP14Blind = 1; else IEP14Blind = 0;
            if (chkDeaf.Checked == true) IEP14Deaf = 1; else IEP14Deaf = 0;
            if (chkEmotional.Checked == true) IEP14Emotional = 1; else IEP14Emotional = 0;
            if (chkLearning.Checked == true) IEP14Learning = 1; else IEP14Learning = 0;
            if (chkLifeskills.Checked == true) IEP14LifeSkills = 1; else IEP14LifeSkills = 0;
            if (chkMultipleDis.Checked == true) IEP14MultipleDisabilities = 1; else IEP14MultipleDisabilities = 0;
            if (chkPhysical.Checked == true) IEP14Physical = 1; else IEP14Physical = 0;
            if (chkSpeech.Checked == true) IEP14Speech = 1; else IEP14Speech = 0;
            if (chkYes.Checked == true) IEP14IsNeighbour = 1; else IEP14IsNeighbour = 0;

            if (ChkNo.Checked == true) IEP14IsNeibhourNo = 1; else IEP14IsNeibhourNo = 0;
            if (ChKSpecialEducation.Checked == true) IEP14SpclEdu = 1; else IEP14SpclEdu = 0;
            if (chkOther.Checked == true) IEP14Other = 1; else IEP14Other = 0;

            strQuery = "UPDATE IEP_PE_Details SET IEP14Itinerant=" + IEP14Itinerant + ",IEP14Supplemental=" + IEP14Supplemental + ",IEP14FullTime=" + IEP14FullTime + ",IEP14AutisticSupport = " + IEP14AutisticSupport + "," +
                        "IEP14Blind = " + IEP14Blind + ",IEP14Deaf = " + IEP14Deaf + ",IEP14Emotional = " + IEP14Emotional + ",IEP14Learning = " + IEP14Learning + ",IEP14LifeSkills = " + IEP14LifeSkills + ",IEP14MultipleDisabilities = " + IEP14MultipleDisabilities + ",IEP14Physical = " + IEP14Physical + ",IEP14Speech = " + IEP14Speech + ",IEP14SchoolDistrict = '" + TxtSchoolDisr.Text + "',IEP14SchoolBuilding = '" + TextBuilding.Text + "'" +
                        ",IEP14IsNeighbour = " + IEP14IsNeighbour + ",IEP14IsNeibhourNo = " + IEP14IsNeibhourNo + ",IEP14SpclEdu = " + IEP14SpclEdu + ",IEP14Other = " + IEP14Other + ",IEP14OtherDesc='" + TextBoxOther.Text + "' WHERE StdtIEP_PEId=" + sess.IEPId;
            int id = oData.ExecuteNonQuery(strQuery);
            if (id > 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                setIEPPEupdateStatus1();
                //fillBasicDetails();
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }

    private void setIEPPEupdateStatus()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page14='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page14) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(15);", true);
    }
    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page14='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page14) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP15('saved');", true);
    }
}