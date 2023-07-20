using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class StudentBinder_CreateIEP_PE2 : System.Web.UI.Page
{
    clsData objData = new clsData();
    clsSession sess = new clsSession();
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
            fillBasicDetails();

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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        DataTable Dt = new DataTable();
        string strQuery = "";

        sess = (clsSession)Session["UserSession"];


        try
        {

           string strQuery1 = "SELECT ParentName,ParentSing,StudentName,StudentSign,RegEduTeacherName," +
                       "regEduTeacherSign,SpclEduTeacherName,SpclEduTeacherSign,LocalEdAgencyName,localEdAgencySign," +
                       " CareerEdRepName,careerEdRepSign,CommunityAgencyName,CommunityAgencySign,TeacherGiftedName," +
                       " TeacherGiftedSign,WittenInput FROM dbo.IEP_PE2_Team WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    strQuery = "UPDATE IEP_PE2_Team SET ParentName='" + TxtParent.Text + "',ParentSing='" + TextBoxParentSign.Text + "',StudentName='" + TxtStudent.Text + "',StudentSign = '" + TextBoxStudentSign.Text + "'," +
                               "RegEduTeacherName='" + TxtRegularTeacher.Text + "',regEduTeacherSign='" + TextBoxRegularTeacherSign.Text + "',SpclEduTeacherName='" + TxtSpecialTeacher.Text + "'," +
                                "SpclEduTeacherSign='" + TextBoxSpecialTeacherSign.Text + "',LocalEdAgencyName='" + TxtLocalEd.Text + "',localEdAgencySign='" + TextBoxLocalEdSign.Text + "'," +
                                 "CareerEdRepName='" + TxtCareer.Text + "',careerEdRepSign='" + TextBoxCareerSign.Text + "',CommunityAgencyName='" + TxtCommunity.Text + "'," +
                                  "CommunityAgencySign='" + TextBoxCommunitySign.Text + "',TeacherGiftedName='" + TxtTeacherGifted.Text + "',TeacherGiftedSign='" + TextBoxTeacherGiftedSign.Text + "'," +
                                   "WittenInput='" + TextBoxWritteninput.Text + "'WHERE StdtIEP_PEId=" + sess.IEPId;

                    int id = objData.Execute(strQuery);
                    if (id > 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        setIEPPEupdateStatus();
                        //fillBasicDetails();
                    }
                }
                else
                {
                    string strQuerryInsert = "insert into [dbo].[IEP_PE2_Team] ([StdtIEP_PEId],[ParentName],[ParentSing],[StudentName],[StudentSign],[RegEduTeacherName],[regEduTeacherSign],[SpclEduTeacherName],[SpclEduTeacherSign], " +
                                          "[LocalEdAgencyName],[localEdAgencySign],[CareerEdRepName],[careerEdRepSign],[CommunityAgencyName],[CommunityAgencySign],[TeacherGiftedName],[TeacherGiftedSign],[WittenInput]) VALUES "+
                                          "('" + sess.IEPId + "', '" + TxtParent.Text + "', '" + TextBoxParentSign.Text + "', '" + TxtStudent.Text + "', '" + TextBoxStudentSign.Text + "', '" + TxtRegularTeacher.Text + "', '" + TextBoxRegularTeacherSign.Text + "'," +
                                          "'" + TxtSpecialTeacher.Text + "' ,'" + TextBoxSpecialTeacherSign.Text + "', '" + TxtLocalEd.Text + "', '" + TextBoxLocalEdSign.Text + "','" + TxtCareer.Text + "', '" + TextBoxCareerSign.Text + "', '" + TxtCommunity.Text + "',"+
                                          "'" + TextBoxCommunitySign.Text + "' ,'" + TxtTeacherGifted.Text + "','" + TextBoxTeacherGiftedSign.Text + "' ,'" + TextBoxWritteninput.Text + "')";
                    int id = objData.Execute(strQuerryInsert);
                    if (id > 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        setIEPPEupdateStatus();
                        //fillBasicDetails();
                    }
                }

                
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }

    }

    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        DataTable Dt = new DataTable();
        string strQuery = "";

        sess = (clsSession)Session["UserSession"];


        try
        {

            string strQuery1 = "SELECT ParentName,ParentSing,StudentName,StudentSign,RegEduTeacherName," +
                        "regEduTeacherSign,SpclEduTeacherName,SpclEduTeacherSign,LocalEdAgencyName,localEdAgencySign," +
                        " CareerEdRepName,careerEdRepSign,CommunityAgencyName,CommunityAgencySign,TeacherGiftedName," +
                        " TeacherGiftedSign,WittenInput FROM dbo.IEP_PE2_Team WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery1, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    strQuery = "UPDATE IEP_PE2_Team SET ParentName='" + TxtParent.Text + "',ParentSing='" + TextBoxParentSign.Text + "',StudentName='" + TxtStudent.Text + "',StudentSign = '" + TextBoxStudentSign.Text + "'," +
                               "RegEduTeacherName='" + TxtRegularTeacher.Text + "',regEduTeacherSign='" + TextBoxRegularTeacherSign.Text + "',SpclEduTeacherName='" + TxtSpecialTeacher.Text + "'," +
                                "SpclEduTeacherSign='" + TextBoxSpecialTeacherSign.Text + "',LocalEdAgencyName='" + TxtLocalEd.Text + "',localEdAgencySign='" + TextBoxLocalEdSign.Text + "'," +
                                 "CareerEdRepName='" + TxtCareer.Text + "',careerEdRepSign='" + TextBoxCareerSign.Text + "',CommunityAgencyName='" + TxtCommunity.Text + "'," +
                                  "CommunityAgencySign='" + TextBoxCommunitySign.Text + "',TeacherGiftedName='" + TxtTeacherGifted.Text + "',TeacherGiftedSign='" + TextBoxTeacherGiftedSign.Text + "'," +
                                   "WittenInput='" + TextBoxWritteninput.Text + "'WHERE StdtIEP_PEId=" + sess.IEPId;

                    int id = objData.Execute(strQuery);
                    if (id > 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        setIEPPEupdateStatus1();
                        //fillBasicDetails();
                    }
                }
                else
                {
                    string strQuerryInsert = "insert into [dbo].[IEP_PE2_Team] ([StdtIEP_PEId],[ParentName],[ParentSing],[StudentName],[StudentSign],[RegEduTeacherName],[regEduTeacherSign],[SpclEduTeacherName],[SpclEduTeacherSign], " +
                                          "[LocalEdAgencyName],[localEdAgencySign],[CareerEdRepName],[careerEdRepSign],[CommunityAgencyName],[CommunityAgencySign],[TeacherGiftedName],[TeacherGiftedSign],[WittenInput]) VALUES " +
                                          "('" + sess.IEPId + "', '" + TxtParent.Text + "', '" + TextBoxParentSign.Text + "', '" + TxtStudent.Text + "', '" + TextBoxStudentSign.Text + "', '" + TxtRegularTeacher.Text + "', '" + TextBoxRegularTeacherSign.Text + "'," +
                                          "'" + TxtSpecialTeacher.Text + "' ,'" + TextBoxSpecialTeacherSign.Text + "', '" + TxtLocalEd.Text + "', '" + TextBoxLocalEdSign.Text + "','" + TxtCareer.Text + "', '" + TextBoxCareerSign.Text + "', '" + TxtCommunity.Text + "'," +
                                          "'" + TextBoxCommunitySign.Text + "' ,'" + TxtTeacherGifted.Text + "','" + TextBoxTeacherGiftedSign.Text + "' ,'" + TextBoxWritteninput.Text + "')";
                    int id = objData.Execute(strQuerryInsert);
                    if (id > 0)
                    {
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                        setIEPPEupdateStatus1();
                        //fillBasicDetails();
                    }
                }


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
            objData.Execute("update StdtIEP_PEUpdateStatus set Page2='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page2) values(" + sess.IEPId + ",'true')");
        }

        ClientScript.RegisterStartupScript(this.GetType(), "", "parent.moveToNextTab(3);", true);
    }

    private void setIEPPEupdateStatus1()
    {
        if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
        {
            objData.Execute("update StdtIEP_PEUpdateStatus set Page2='true' where stdtIEPId=" + sess.IEPId);
        }
        else
        {
            objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page2) values(" + sess.IEPId + ",'true')");
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "parent.CreateIEP3('saved');", true);
    }

    private void fillBasicDetails()
    {
        objData = new clsData();       
        DataTable Dt = new DataTable();
        string strQuery = "";
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

            strQuery = "SELECT ParentName,ParentSing,StudentName,StudentSign,RegEduTeacherName," +
                       "regEduTeacherSign,SpclEduTeacherName,SpclEduTeacherSign,LocalEdAgencyName,localEdAgencySign," +
                       " CareerEdRepName,careerEdRepSign,CommunityAgencyName,CommunityAgencySign,TeacherGiftedName," +
                       " TeacherGiftedSign,WittenInput FROM dbo.IEP_PE2_Team WHERE StdtIEP_PEId=" + sess.IEPId;
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    TxtParent.Text = Dt.Rows[0]["ParentName"].ToString().Trim();
                    TextBoxParentSign.Text = Dt.Rows[0]["ParentSing"].ToString().Trim();

                    TxtStudent.Text = Dt.Rows[0]["StudentName"].ToString().Trim();
                    TextBoxStudentSign.Text = Dt.Rows[0]["StudentSign"].ToString().Trim();
                    TxtRegularTeacher.Text = Dt.Rows[0]["RegEduTeacherName"].ToString().Trim();
                    TextBoxRegularTeacherSign.Text = Dt.Rows[0]["regEduTeacherSign"].ToString().Trim();
                    TxtSpecialTeacher.Text = Dt.Rows[0]["SpclEduTeacherName"].ToString().Trim();
                    TextBoxSpecialTeacherSign.Text = Dt.Rows[0]["SpclEduTeacherSign"].ToString().Trim();
                    TxtLocalEd.Text = Dt.Rows[0]["LocalEdAgencyName"].ToString().Trim();
                    TextBoxLocalEdSign.Text = Dt.Rows[0]["localEdAgencySign"].ToString().Trim();
                    TxtCareer.Text = Dt.Rows[0]["CareerEdRepName"].ToString().Trim();
                    TextBoxCareerSign.Text = Dt.Rows[0]["careerEdRepSign"].ToString().Trim();
                    TxtCommunity.Text = Dt.Rows[0]["CommunityAgencyName"].ToString().Trim();

                    TextBoxCommunitySign.Text = Dt.Rows[0]["CommunityAgencySign"].ToString().Trim();
                    TxtTeacherGifted.Text = Dt.Rows[0]["TeacherGiftedName"].ToString().Trim();

                    TextBoxTeacherGiftedSign.Text = Dt.Rows[0]["TeacherGiftedSign"].ToString().Trim();
                    TextBoxWritteninput.Text = Dt.Rows[0]["WittenInput"].ToString().Trim();
                  
                }
            }
        }
        catch (Exception Ex)
        {
            throw (Ex);
        }
    }
}