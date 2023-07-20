using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class StudentIntakeAssessment : System.Web.UI.Page
{

    clsSession sess = null;
    clsData objData = null;
    public static int studId = 0;
    static bool isUpd = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Wizard1.PreRender += new EventHandler(Wizard1_PreRender);
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        //else
        //{
        //    bool flag = clsGeneral.PageIdentification(sess.perPage);
        //    if (flag == false)
        //    {
        //        Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
        //    }

        //}
        if (!IsPostBack)
        {
            bool Disable = false;
            //clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Request.QueryString["Permission"] != null)
                Disable =Convert.ToBoolean(Request.QueryString["Permission"]);
            if (Disable == true)
            {
                btnSave.Visible = false;
                btnSave1.Visible = false;
                BtnSave2.Visible = false;
                BtnSave3.Visible = false;
                BtnSave4.Visible = false;
                BtnSave5.Visible = false;

            }
            else
            {
                btnSave.Visible = true;
                btnSave1.Visible = true;
                BtnSave2.Visible = true;
                BtnSave3.Visible = true;
                BtnSave4.Visible = true;
                BtnSave5.Visible = true;
            }

            if (Session["StudentIntakeId"] != null)
            {
                objData = new clsData();
                studId = Convert.ToInt32(Session["StudentIntakeId"]);
                lblStNameAttend.Text = objData.FetchValue("Select  StudentLname+','+ StudentFname as Name from Student where StudentId=" + studId + "").ToString();
                //lblStNameExpressive.Text = objData.FetchValue("Select  StudentLname+','+ StudentFname as Name from Student where StudentId=" + studId + "").ToString();
                //lblStNameInitation.Text = objData.FetchValue("Select  StudentLname+','+ StudentFname as Name from Student where StudentId=" + studId + "").ToString();
                //lblStNamePreAcademic.Text = objData.FetchValue("Select  StudentLname+','+ StudentFname as Name from Student where StudentId=" + studId + "").ToString();
                //lblStNameSelf.Text = objData.FetchValue("Select  StudentLname+','+ StudentFname as Name from Student where StudentId=" + studId + "").ToString();
                //lblStudentNameReceptive.Text = objData.FetchValue("Select  StudentLname+','+ StudentFname as Name from Student where StudentId=" + studId + "").ToString();
                //string selctDob = "SELECT DOB from Student where StudentId=" + studId + "";
                //DataTable dtBirth = objData.ReturnDataTable(selctDob, false);
                lblAttendDob.Text = Convert.ToDateTime(objData.FetchValue("SELECT DOB from Student where StudentId=" + studId + "")).ToString("MM/dd/yyyy").Replace("-", "/");
                // lblAttendDob.Text = objData.FetchValue("Select DOB from Student where StudentId=" + studId + "").ToString();
                lblAttendGender.Text = objData.FetchValue("Select Gender from Student where StudentId=" + studId + "").ToString();
            }


            FillAttending();
            FillImitation();
            FillReceptive();
            FillExpressive();
            FillPreAcademic();
            FillSelfHelp();



        }

        
    }
    protected DataTable CreateDatatable()
    {
        ClassDatatable oDt = new ClassDatatable();
        DataTable dtQues = new DataTable();
        dtQues = oDt.CreateColumn("QId", dtQues);
        dtQues = oDt.CreateColumn("Ques", dtQues);
        dtQues = oDt.CreateColumn("Score", dtQues);
        dtQues = oDt.CreateColumn("Commnt", dtQues);
        return dtQues;
    }
    protected void FillAttending()
    {
        AttendingSkills.InnerHtml = "";
        isUpd = false;
        objData = new clsData();
        string selctDate = " SELECT  CASE WHEN Si.ModifiedOn is null then Si.CreatedOn ELSE Si.ModifiedOn END AS ModifiedDate,St.StudentLname+','+St.StudentFname As Name,Si.QType FROM (StdtIntAsmnt Si FULL JOIN Student St ON Si.StudentId = St.StudentId) WHERE Si.StudentId = " + studId + " AND Si.QType = 'Attending Skills'";
        DataTable dtList = objData.ReturnDataTable(selctDate, false);
        if (dtList.Rows.Count > 0)
        {
            string date = Convert.ToDateTime(dtList.Rows[dtList.Rows.Count - 1]["ModifiedDate"].ToString()).ToString("MM-dd-yyyy hh:mm:ss");
            //string name = dtList.Rows[0]["Name"].ToString();
            //lblStNameAttend.Text = name;
            lblLastModified.Text = date;
        }

        ClassDatatable oDt = new ClassDatatable();
        DataTable dtQtn = new DataTable();
        dtQtn = CreateDatatable();
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "1", "Sits in a chair independently" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "2", "Makes eye contact in response to name" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "3", "Makes eye contact when given the instruction 'Look at me'" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "4", "Response to the direction 'Hands Down'" });
        grdAttending.DataSource = dtQtn;
        grdAttending.DataBind();
        foreach (GridViewRow gr in grdAttending.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            string sel = "SELECT QResult,QComment FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Attending Skills' AND QCode='" + gr.Cells[0].Text + "'";
            if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
            {
                btnSave.Text = "UPDATE";
                DataTable dt = objData.ReturnDataTable(sel, false);
                cmt.Text = dt.Rows[0]["QComment"].ToString();
                foreach (ListItem li in rdbScore.Items)
                {
                    if (li.Text == dt.Rows[0]["QResult"].ToString())
                    {
                        li.Selected = true;
                    }
                }
            }
        }
    }

    protected void FillImitation()
    {
        ImitationSkills.InnerHtml = "";
        isUpd = false;
        objData = new clsData();
        string selctDate = " SELECT  CASE WHEN Si.ModifiedOn is null then Si.CreatedOn ELSE Si.ModifiedOn END AS ModifiedDate,St.StudentLname+','+St.StudentFname As Name,Si.QType FROM (StdtIntAsmnt Si FULL JOIN Student St ON Si.StudentId = St.StudentId) WHERE Si.StudentId = " + studId + " AND Si.QType = 'ImitationSkills'";
        DataTable dtList = objData.ReturnDataTable(selctDate, false);
        if (dtList.Rows.Count > 0)
        {
            string date = Convert.ToDateTime(dtList.Rows[dtList.Rows.Count - 1]["ModifiedDate"].ToString()).ToString("MM/dd/yyyy hh:mm:ss");
            // string name = dtList.Rows[0]["Name"].ToString();
            //lblStNameInitation.Text = name;
            lblDateModImitation.Text = date;
        }
        ClassDatatable oDt = new ClassDatatable();
        DataTable dtQtn = new DataTable();
        dtQtn = CreateDatatable();
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "1", "Imitates gross motor movements" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "2", "Imitates action with objects" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "3", "Imitates fine motor movements" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "4", "Imitates oral motor movements" });
        grdImitation.DataSource = dtQtn;
        grdImitation.DataBind();
        foreach (GridViewRow gr in grdImitation.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            string sel = "SELECT QResult,QComment FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='ImitationSkills' AND QCode='" + gr.Cells[0].Text + "'";
            if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
            {
                btnSave1.Text = "UPDATE";
                DataTable dt = objData.ReturnDataTable(sel, false);
                cmt.Text = dt.Rows[0]["QComment"].ToString();
                foreach (ListItem li in rdbScore.Items)
                {
                    if (li.Text == dt.Rows[0]["QResult"].ToString())
                    {
                        li.Selected = true;
                    }
                }
            }
        }

    }


    protected void FillReceptive()
    {
        ReceptiveLanguageSkills.InnerHtml = "";
        isUpd = false;
        objData = new clsData();
        string selctDate = " SELECT  CASE WHEN Si.ModifiedOn is null then Si.CreatedOn ELSE Si.ModifiedOn END AS ModifiedDate,St.StudentLname+','+St.StudentFname As Name,Si.QType FROM (StdtIntAsmnt Si FULL JOIN Student St ON Si.StudentId = St.StudentId) WHERE Si.StudentId = " + studId + "AND Si.QType = 'Receptive Language Skills'";
        DataTable dtList = objData.ReturnDataTable(selctDate, false);
        if (dtList.Rows.Count > 0)
        {
            string date = Convert.ToDateTime(dtList.Rows[dtList.Rows.Count - 1]["ModifiedDate"].ToString()).ToString("MM/dd/yyyy hh:mm:ss");
            // string name = dtList.Rows[0]["Name"].ToString();
            //lblStudentNameReceptive.Text = name;
            lblDateModifiedReceptive.Text = date;
        }
        ClassDatatable oDt = new ClassDatatable();
        DataTable dtQtn = new DataTable();
        dtQtn = CreateDatatable();
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "1", "Follows one step Instructions" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "2", "Identifies Body Part" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "3", "Identifies Objects" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "4", "Identifies Pictures" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "5", "Identifies Familiar People" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "6", "Follows verb Instructions" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "7", "Identifies verb in Pictures" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "8", "Identifies objects in the environment" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "9", " Points to Pictures in a book" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "10", "Identifies object by function" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "11", "Identifies possession" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "12", "Identifies environmental sounds" });

        grdReceptive.DataSource = dtQtn;
        grdReceptive.DataBind();

        foreach (GridViewRow gr in grdReceptive.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            string sel = "SELECT QResult,QComment FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Receptive Language Skills' AND QCode='" + gr.Cells[0].Text + "'";
            if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
            {
                BtnSave2.Text = "UPDATE";
                DataTable dt = objData.ReturnDataTable(sel, false);
                cmt.Text = dt.Rows[0]["QComment"].ToString();
                foreach (ListItem li in rdbScore.Items)
                {
                    if (li.Text == dt.Rows[0]["QResult"].ToString())
                    {
                        li.Selected = true;
                    }
                }
            }
        }

    }

    protected void FillExpressive()
    {
        ExpressiveLanguageSkills.InnerHtml = "";
        isUpd = false;
        objData = new clsData();
        string selctDate = " SELECT  CASE WHEN Si.ModifiedOn is null then Si.CreatedOn ELSE Si.ModifiedOn END AS ModifiedDate,St.StudentLname+','+St.StudentFname As Name,Si.QType FROM (StdtIntAsmnt Si FULL JOIN Student St ON Si.StudentId = St.StudentId) WHERE Si.StudentId = " + studId + "AND Si.QType = 'Expressive Language Skills'";
        DataTable dtList = objData.ReturnDataTable(selctDate, false);
        if (dtList.Rows.Count > 0)
        {
            // string name = dtList.Rows[0]["Name"].ToString();
            //lblStNameExpressive.Text = name;
            string date = Convert.ToDateTime(dtList.Rows[dtList.Rows.Count - 1]["ModifiedDate"].ToString()).ToString("MM/dd/yyyy hh:mm:ss");
            lblDateModifiedExpressive.Text = date;
        }
        ClassDatatable oDt = new ClassDatatable();
        DataTable dtQtn = new DataTable();
        dtQtn = CreateDatatable();
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "1", "Points to desired Items in response to :What do you want?" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "2", "Points to desired items spontaneously" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "3", "Imitates Sounds and Words ?" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "4", "Label Objects" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "5", "Label Pictures" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "6", "Verbally requests desired Items" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "7", "States or Gestures Yes or No for preferred and Non preferred Items" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "8", "Labels Familiar People" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "9", "Makes a choice" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "10", " Reciprocates greetings  " });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "11", "Answers Social Questions" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "12", "Labels verb in Pictures,others,self" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "13", "Labels object by function" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "14", "Label Possessions" });

        grdExpressive.DataSource = dtQtn;
        grdExpressive.DataBind();

        foreach (GridViewRow gr in grdExpressive.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            string sel = "SELECT QResult,QComment FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Expressive Language Skills' AND QCode='" + gr.Cells[0].Text + "'";
            if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
            {
                BtnSave3.Text = "UPDATE";
                DataTable dt = objData.ReturnDataTable(sel, false);
                cmt.Text = dt.Rows[0]["QComment"].ToString();
                foreach (ListItem li in rdbScore.Items)
                {
                    if (li.Text == dt.Rows[0]["QResult"].ToString())
                    {
                        li.Selected = true;
                    }
                }
            }
        }

    }

    protected void FillPreAcademic()
    {
        PreAcademicSkills.InnerHtml = "";
        isUpd = false;
        objData = new clsData();
        string selctDate = " SELECT  CASE WHEN Si.ModifiedOn is null then Si.CreatedOn ELSE Si.ModifiedOn END AS ModifiedDate,St.StudentLname+','+St.StudentFname As Name,Si.QType FROM (StdtIntAsmnt Si FULL JOIN Student St ON Si.StudentId = St.StudentId) WHERE Si.StudentId = " + studId + "AND Si.QType = 'Preacademic Skills'";
        DataTable dtList = objData.ReturnDataTable(selctDate, false);
        if (dtList.Rows.Count > 0)
        {
            // string name = dtList.Rows[0]["Name"].ToString();
            //lblStNamePreAcademic.Text = name;
            string date = Convert.ToDateTime(dtList.Rows[dtList.Rows.Count - 1]["ModifiedDate"].ToString()).ToString("MM/dd/yyyy hh:mm:ss");
            lblModifiedPreAcademic.Text = date;
        }
        ClassDatatable oDt = new ClassDatatable();
        DataTable dtQtn = new DataTable();
        dtQtn = CreateDatatable();
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "1", "Matches - Identical objects" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "2", "Matches - Identical Pictures" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "3", "Matches - Objects to Pictures" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "4", "Matches- Pictures to Objects" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "5", "Matches - Colors,Shapes,Letters,Numbers" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "6", "Matches - NonIdentical Objects" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "7", "Matches - Objects by Associations" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "8", "Simple Activity - Identifies Colors" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "9", "Simple Activity - Identifies Shapes" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "10", " Simple Activity - Identifies Letters" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "11", " Simple Activity - Identifies Numbers" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "12", " Simple Activity - Counts by rote to 10" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "13", " Simple Activity - Count Objects" });
        grdPreacademics.DataSource = dtQtn;
        grdPreacademics.DataBind();

        foreach (GridViewRow gr in grdPreacademics.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            string sel = "SELECT QResult,QComment FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Preacademic Skills' AND QCode='" + gr.Cells[0].Text + "'";
            if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
            {
                BtnSave4.Text = "UPDATE";
                DataTable dt = objData.ReturnDataTable(sel, false);
                cmt.Text = dt.Rows[0]["QComment"].ToString();
                foreach (ListItem li in rdbScore.Items)
                {
                    if (li.Text == dt.Rows[0]["QResult"].ToString())
                    {
                        li.Selected = true;
                    }
                }
            }
        }

    }

    protected void FillSelfHelp()
    {
        SelfHelpSkills.InnerHtml = "";
        isUpd = false;
        objData = new clsData();
        string selctDate = "SELECT  CASE WHEN Si.ModifiedOn is null then Si.CreatedOn ELSE Si.ModifiedOn END AS ModifiedDate,St.StudentLname+','+St.StudentFname As Name,Si.QType FROM (StdtIntAsmnt Si FULL JOIN Student St ON Si.StudentId = St.StudentId) WHERE Si.StudentId = " + studId + "AND Si.QType = 'Self Help Skills'";
        DataTable dtList = objData.ReturnDataTable(selctDate, false);
        if (dtList.Rows.Count > 0)
        {
            //string name = dtList.Rows[0]["Name"].ToString();
            //lblStNameSelf.Text = name;
            string date = Convert.ToDateTime(dtList.Rows[dtList.Rows.Count - 1]["ModifiedDate"].ToString()).ToString("MM/dd/yyyy hh:mm:ss");
            lblSelf.Text = date;
        }
        ClassDatatable oDt = new ClassDatatable();
        DataTable dtQtn = new DataTable();
        dtQtn = CreateDatatable();
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "1", "Drinks from a cup" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "2", "Uses fork and spoon when eating" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "3", "Removes Shoes" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "4", "Removes Socks" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "5", "Removes Pants" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "6", "Removes Shirts " });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "7", "Use Napkin/tissue" });
        dtQtn = oDt.CreateAssessmntsTable(new string[] { "QId", "Ques" }, dtQtn, new string[] { "8", "Is toilet trained for urination" });

        grdSelfHelp.DataSource = dtQtn;
        grdSelfHelp.DataBind();

        foreach (GridViewRow gr in grdSelfHelp.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            string sel = "SELECT QResult,QComment FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType= 'Self Help Skills' AND QCode='" + gr.Cells[0].Text + "'";
            if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
            {
                BtnSave5.Text = "UPDATE";
                DataTable dt = objData.ReturnDataTable(sel, false);
                cmt.Text = dt.Rows[0]["QComment"].ToString();
                foreach (ListItem li in rdbScore.Items)
                {
                    if (li.Text == dt.Rows[0]["QResult"].ToString())
                    {
                        li.Selected = true;
                    }
                }
            }
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveAttendingSkills();
        Page_Load(sender, e);
    }
    protected void btnSave1_Click(object sender, EventArgs e)
    {
        SaveImitationSkills();
    }
    protected void BtnSave2_Click(object sender, EventArgs e)
    {
        SaveReceptive();
    }
    protected void BtnSave3_Click(object sender, EventArgs e)
    {
        SaveExpressive();
    }
    protected void BtnSave4_Click(object sender, EventArgs e)
    {
        SavePreAcademic();
    }
    protected void BtnSave5_Click(object sender, EventArgs e)
    {
        SaveSelfHelp();
    }


    protected void SaveAttendingSkills()
    {

        sess = (clsSession)Session["UserSession"];
        int schoolId = sess.SchoolId;
        int loginId = sess.LoginId;
        int radio = 0;
        objData = new clsData();
        foreach (GridViewRow gr in grdAttending.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            if (rdbScore.SelectedItem != null)
            {
                radio = radio + 1;
                string comment = cmt.Text;
                string Qid = gr.Cells[0].Text;
                string section = "Attending Skills";
                string score = rdbScore.SelectedItem.Text;


                string sel = "SELECT QCode FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Attending Skills' AND QCode='" + Qid + "'";
                if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
                {
                    isUpd = true;
                }


                if (isUpd == true)
                {
                    string upd = "UPDATE StdtIntAsmnt SET QResult='" + rdbScore.SelectedItem.Text + "',QComment='" + clsGeneral.convertQuotes(cmt.Text) + "',ModifiedBy=" + loginId + "," +
                        "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StudentId=" + studId + " AND QType='Attending Skills' AND QCode='" + Qid + "'";
                    int index = objData.Execute(upd);
                    if (index > 0)
                    {
                        AttendingSkills.InnerHtml = clsGeneral.sucessMsg("Attending Skills of Student Updated Successfully");

                    }
                    else
                    {
                        AttendingSkills.InnerHtml = clsGeneral.failedMsg("Attending Skills of Student Updation Error");
                    }
                    isUpd = false;
                }
                else
                {


                    string insertQuerry = "INSERT into StdtIntAsmnt(StudentId,SchoolId,QType,QCode,QResult,QComment,CreatedBy,CreatedOn) Values " +
                                             "(" + studId + "," + schoolId + ",'" + section + "','" + Qid + "','" + score + "','" + clsGeneral.convertQuotes(comment) + "'," + loginId + ",(SELECT convert(varchar, getdate(), 100))) ";

                    int index = objData.Execute(insertQuerry);
                    if (index > 0)
                    {
                        AttendingSkills.InnerHtml = clsGeneral.sucessMsg("Attending Skills of Student Saved Successfully");

                    }
                    else
                    {
                        AttendingSkills.InnerHtml = clsGeneral.failedMsg("Attending Skills of Student Insertion Error");
                    }
                }
            }
            else if (radio <= 0)
            {
                AttendingSkills.InnerHtml = clsGeneral.warningMsg("Select any Score!!!!");
            }
        }
    }

    protected void SaveImitationSkills()
    {
        sess = (clsSession)Session["UserSession"];
        int schoolId = sess.SchoolId;
        int loginId = sess.LoginId;
        int radio = 0;
        objData = new clsData();
        foreach (GridViewRow gr in grdImitation.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            if (rdbScore.SelectedItem != null)
            {
                radio = radio + 1;
                string comment = cmt.Text;
                string Qid = gr.Cells[0].Text;
                string section = "ImitationSkills";
                string score = rdbScore.SelectedItem.Text;
                string sel = "SELECT QCode FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='ImitationSkills' AND QCode='" + Qid + "'";
                if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
                {
                    isUpd = true;
                }


                if (isUpd == true)
                {

                    string upd = "UPDATE StdtIntAsmnt SET QResult='" + rdbScore.SelectedItem.Text + "',QComment='" + clsGeneral.convertQuotes(cmt.Text) + "',ModifiedBy=" + loginId + "," +
                        "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StudentId=" + studId + " AND QType='ImitationSkills' AND QCode='" + Qid + "'";
                    int index = objData.Execute(upd);
                    if (index > 0)
                    {
                        ImitationSkills.InnerHtml = clsGeneral.sucessMsg("Imitation Skills of Student Updated Successfully");

                    }
                    else
                    {
                        // tdMsgImitation.InnerHtml = clsGeneral.failedMsg("Imitation Skills of Student Updation Error");
                    }
                    isUpd = false;
                }

                else
                {


                    string insertQuerry = "INSERT into StdtIntAsmnt(StudentId,SchoolId,QType,QCode,QResult,QComment,CreatedBy,CreatedOn) Values " +
                                             "(" + studId + "," + sess.SchoolId + ",'" + section + "','" + Qid + "','" + score + "','" + clsGeneral.convertQuotes(comment) + "'," + loginId + ",(SELECT convert(varchar, getdate(), 100))) ";

                    int index = objData.Execute(insertQuerry);
                    if (index > 0)
                    {
                        ImitationSkills.InnerHtml = clsGeneral.sucessMsg("Imitation Skills of Student Inserted Successfully");
                    }
                    else
                    {
                        ImitationSkills.InnerHtml = clsGeneral.failedMsg("  Error  : Imitation Skills Insertion Failed");
                    }
                }
            }

            else if (radio <= 0)
            {
                ImitationSkills.InnerHtml = clsGeneral.warningMsg("Select any Score!!!!");
            }
        }


    }



    protected void SaveReceptive()
    {
        sess = (clsSession)Session["UserSession"];
        int schoolId = sess.SchoolId;
        int loginId = sess.LoginId;
        objData = new clsData();
        int radio = 0;
        foreach (GridViewRow gr in grdReceptive.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            if (rdbScore.SelectedItem != null)
            {
                radio = radio + 1;
                string comment = cmt.Text;
                string Qid = gr.Cells[0].Text;
                string section = "Receptive Language Skills";
                string score = rdbScore.SelectedItem.Text;

                string sel = "SELECT QCode FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType= 'Receptive Language Skills' AND QCode='" + Qid + "'";
                if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
                {
                    isUpd = true;
                }


                if (isUpd == true)
                {

                    string upd = "UPDATE StdtIntAsmnt SET QResult='" + rdbScore.SelectedItem.Text + "',QComment='" + clsGeneral.convertQuotes(cmt.Text) + "',ModifiedBy=" + loginId + "," +
                        "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StudentId=" + studId + " AND QType='Receptive Language Skills' AND QCode='" + Qid + "'";
                    int index = objData.Execute(upd);
                    if (index > 0)
                    {
                        ReceptiveLanguageSkills.InnerHtml = clsGeneral.sucessMsg("Receptive Language Skills of Student Updated Successfully");

                    }
                    else
                    {
                        ReceptiveLanguageSkills.InnerHtml = clsGeneral.failedMsg("Receptive Language Skills of Student Updation Error");
                    }
                    isUpd = false;
                }

                else
                {

                    string insertQuerry = "INSERT into StdtIntAsmnt(StudentId,SchoolId,QType,QCode,QResult,QComment,CreatedBy,CreatedOn) Values " +
                                             "(" + studId + "," + sess.SchoolId + ",'" + section + "','" + Qid + "','" + score + "','" + clsGeneral.convertQuotes(comment) + "'," + loginId + ",(SELECT convert(varchar, getdate(), 100))) ";

                    int index = objData.Execute(insertQuerry);
                    if (index > 0)
                    {
                        ReceptiveLanguageSkills.InnerHtml = clsGeneral.sucessMsg("Receptive Skills of Student Inserted Successfully");
                    }
                    else
                    {
                        ReceptiveLanguageSkills.InnerHtml = clsGeneral.failedMsg("  Error  :    Receptive Skills  Insertion Failed");

                    }
                }
            }

            else if (radio <= 0)
            {
                ReceptiveLanguageSkills.InnerHtml = clsGeneral.warningMsg("Select any Score!!!!");

            }
        }

    }


    protected void SaveExpressive()
    {
        sess = (clsSession)Session["UserSession"];
        int schoolId = sess.SchoolId;
        int radio = 0;
        int loginId = sess.LoginId;
        objData = new clsData();
        foreach (GridViewRow gr in grdExpressive.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            if (rdbScore.SelectedItem != null)
            {
                radio = radio + 1;
                string comment = cmt.Text;
                string Qid = gr.Cells[0].Text;
                string section = "Expressive Language Skills";
                string score = rdbScore.SelectedItem.Text;

                string sel = "SELECT QCode FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Expressive Language Skills' AND QCode='" + Qid + "'";
                if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
                {
                    isUpd = true;
                }


                if (isUpd == true)
                {

                    string upd = "UPDATE StdtIntAsmnt SET QResult='" + rdbScore.SelectedItem.Text + "',QComment='" + clsGeneral.convertQuotes(cmt.Text) + "',ModifiedBy=" + loginId + "," +
                        "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StudentId=" + studId + " AND QType='Expressive Language Skills' AND QCode='" + Qid + "'";
                    int index = objData.Execute(upd);
                    if (index > 0)
                    {
                        ExpressiveLanguageSkills.InnerHtml = clsGeneral.sucessMsg("Expressive Skills of Student Updated Successfully");

                    }
                    else
                    {
                        ExpressiveLanguageSkills.InnerHtml = clsGeneral.failedMsg("Expressive Skills of Student Updation Error");
                    }
                    isUpd = false;
                }

                else
                {

                    string insertQuerry = "INSERT into StdtIntAsmnt(StudentId,SchoolId,QType,QCode,QResult,QComment,CreatedBy,CreatedOn) Values " +
                                             "(" + studId + "," + sess.SchoolId + ",'" + section + "','" + Qid + "','" + score + "','" + clsGeneral.convertQuotes(comment) + "'," + sess.LoginId + ",(SELECT convert(varchar, getdate(), 100))) ";

                    int index = objData.Execute(insertQuerry);
                    if (index > 0)
                    {
                        ExpressiveLanguageSkills.InnerHtml = clsGeneral.sucessMsg("Expressive Skills of Student   Inserted Successfully");
                    }
                    else
                    {
                        ExpressiveLanguageSkills.InnerHtml = clsGeneral.failedMsg("  Error  :    Expressive Skills  Insertion Failed");
                    }
                }
            }

            else if (radio <= 0)
            {
                ExpressiveLanguageSkills.InnerHtml = clsGeneral.warningMsg("Select any Score!!!!");
            }
        }

    }


    protected void SavePreAcademic()
    {
        sess = (clsSession)Session["UserSession"];
        int radio = 0;
        int schoolId = sess.SchoolId;
        int loginId = sess.LoginId;
        objData = new clsData();
        foreach (GridViewRow gr in grdPreacademics.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            if (rdbScore.SelectedItem != null)
            {
                radio = radio + 1;
                string comment = cmt.Text;
                string Qid = gr.Cells[0].Text;
                string section = "Preacademic Skills";
                string score = rdbScore.SelectedItem.Text;


                string sel = "SELECT QCode FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Preacademic Skills' AND QCode='" + Qid + "'";
                if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
                {
                    isUpd = true;
                }


                if (isUpd == true)
                {

                    string upd = "UPDATE StdtIntAsmnt SET QResult='" + rdbScore.SelectedItem.Text + "',QComment='" + clsGeneral.convertQuotes(cmt.Text) + "',ModifiedBy=" + loginId + "," +
                        "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StudentId=" + studId + " AND QType= 'Preacademic Skills' AND QCode='" + Qid + "'";
                    int index = objData.Execute(upd);
                    if (index > 0)
                    {
                        PreAcademicSkills.InnerHtml = clsGeneral.sucessMsg("PreAcademic Skills  of Student Updated Successfully");

                    }
                    else
                    {
                        PreAcademicSkills.InnerHtml = clsGeneral.failedMsg("PreAcademic Skills  of Student Updation Error");
                    }
                    isUpd = false;
                }

                else
                {

                    string insertQuerry = "INSERT into StdtIntAsmnt(StudentId,SchoolId,QType,QCode,QResult,QComment,CreatedBy,CreatedOn) Values " +
                                             "(" + studId + "," + sess.SchoolId + ",'" + section + "','" + Qid + "','" + score + "','" + clsGeneral.convertQuotes(comment) + "'," + sess.LoginId + ",(SELECT convert(varchar, getdate(), 100))) ";

                    int index = objData.Execute(insertQuerry);
                    if (index > 0)
                    {
                        PreAcademicSkills.InnerHtml = clsGeneral.sucessMsg("PreAcademic Skills of Student   Inserted Successfully");
                    }
                    else
                    {
                        PreAcademicSkills.InnerHtml = clsGeneral.failedMsg("  Error  :    PreAcademic Skills Insertion Failed");
                    }
                }
            }

            else if (radio <= 0)
            {
                PreAcademicSkills.InnerHtml = clsGeneral.warningMsg("Select any Score!!!!");
            }
        }

    }



    protected void SaveSelfHelp()
    {
        sess = (clsSession)Session["UserSession"];
        int schoolId = sess.SchoolId;
        int loginId = sess.LoginId;
        objData = new clsData();
        int radio = 0;
        foreach (GridViewRow gr in grdSelfHelp.Rows)
        {
            TextBox cmt = (TextBox)gr.FindControl("txtComment");
            RadioButtonList rdbScore = (RadioButtonList)gr.FindControl("rdbScore");
            if (rdbScore.SelectedItem != null)
            {
                radio = radio + 1;
                string comment = cmt.Text;
                string Qid = gr.Cells[0].Text;
                string section = "Self Help Skills";
                string score = rdbScore.SelectedItem.Text;

                string sel = "SELECT QCode FROM StdtIntAsmnt WHERE StudentId=" + studId + " AND QType='Self Help Skills' AND QCode='" + Qid + "'";
                if (objData.ReturnDataTable(sel, false).Rows.Count > 0)
                {
                    isUpd = true;
                }


                if (isUpd == true)
                {

                    string upd = "UPDATE StdtIntAsmnt SET QResult='" + rdbScore.SelectedItem.Text + "',QComment='" + clsGeneral.convertQuotes(cmt.Text) + "',ModifiedBy=" + loginId + "," +
                        "ModifiedOn=(SELECT convert(varchar, getdate(), 100)) WHERE StudentId=" + studId + " AND QType= 'Self Help Skills' AND QCode='" + Qid + "'";
                    int index = objData.Execute(upd);
                    if (index > 0)
                    {
                        SelfHelpSkills.InnerHtml = clsGeneral.sucessMsg("Self Help Skills  of Student Updated Successfully");

                    }
                    else
                    {
                        SelfHelpSkills.InnerHtml = clsGeneral.failedMsg("Self Help  Skills  of Student Updation Error");
                    }
                    isUpd = false;
                }
                else
                {

                    string insertQuerry = "INSERT into StdtIntAsmnt(StudentId,SchoolId,QType,QCode,QResult,QComment,CreatedBy,CreatedOn) Values " +
                                             "(" + studId + "," + sess.SchoolId + ",'" + section + "','" + Qid + "','" + score + "','" + clsGeneral.convertQuotes(comment) + "'," + sess.LoginId + ",(SELECT convert(varchar, getdate(), 100))) ";

                    int index = objData.Execute(insertQuerry);
                    if (index > 0)
                    {
                        SelfHelpSkills.InnerHtml = clsGeneral.sucessMsg("Self Help  Skills of Student Inserted Successfully");
                    }
                    else
                    {
                        SelfHelpSkills.InnerHtml = clsGeneral.failedMsg("  Error  :    Self Help  Skills Insertion Failed");
                    }
                }
            }

            else if (radio <= 0)
            {
                SelfHelpSkills.InnerHtml = clsGeneral.warningMsg("Select any Score!!!!");
            }
        }

    }

    







    //protected void Wizard1_PreRender(object sender, EventArgs e)
    //{
    //    Repeater SideBarList = Wizard1.FindControl("HeaderContainer").FindControl("SideBarList") as Repeater;
    //    SideBarList.DataSource = Wizard1.WizardSteps;
    //    SideBarList.DataBind();

    //}
    //protected string GetClassForWizardStep(object wizardStep)
    //{
    //    WizardStep step = wizardStep as WizardStep;

    //    if (step == null)
    //    {
    //        return "";
    //    }
    //    int stepIndex = Wizard1.WizardSteps.IndexOf(step);

    //    if (stepIndex < Wizard1.ActiveStepIndex)
    //    {
    //        return "prevStep";
    //    }
    //    else if (stepIndex > Wizard1.ActiveStepIndex)
    //    {
    //        return "nextStep";
    //    }
    //    else
    //    {
    //        return "currentStep";
    //    }
    //}

    //protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    //{
    //    tdMsg.InnerHtml = "";
    //}
    //protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    //{
    //    tdMsg.InnerHtml = "";
    //}
    //protected void Wizard1_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    //{
    //    tdMsg.InnerHtml = "";
    //}
    //protected void SideBarList_ItemCommand(object source, RepeaterCommandEventArgs e)
    //{
    //    //lnkBack.Text = (e.Item.ItemIndex).ToString();

    //    Wizard1.ActiveStepIndex = e.Item.ItemIndex;
    //    tdMsg.InnerHtml = "";
    //}
    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("StudentMenu.aspx");
    }
}