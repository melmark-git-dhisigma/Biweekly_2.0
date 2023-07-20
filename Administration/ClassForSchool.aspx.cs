using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Collections;
using System.Web.Services;
using System.Configuration;

public partial class Admin_ClassForSchool : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    clsClass objClass = null;
    static int ClassId = 0;
    string strQuery = "";
    static bool retVal = false;
    static bool Disabled = false;

    protected void Page_Load(object sender, EventArgs e)
    {

        sess = (clsSession)Session["UserSession"];

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
        grdGroup.PageSize = sess.GridPagingSize;
        if (!IsPostBack)
        {
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disabled);
            if (Disabled == true)
            {
                btnSave.Visible = false;
                btnAdd.Visible = false;
                if (grdGroup.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdGroup.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                        lb_delete.Visible = false;
                    }
                }
            }
            else
            {
                btnSave.Visible = true;
                btnAdd.Visible = true;
                if (grdGroup.Rows.Count > 0)
                {
                    foreach (GridViewRow rows in grdGroup.Rows)
                    {
                        ImageButton lb_delete = ((ImageButton)rows.FindControl("lb_delete"));
                        lb_delete.Visible = true;
                    }
                }
            }

            lnk_active.ForeColor = System.Drawing.Color.Red;
            LoadGroup();
            FillUsers();
            FillStudents();


        }

    }



    private void FillUsers()
    {
        objData = new clsData();
        string query = "SELECT  UserId as Id,UserLName+' , '+UserFName as Name from [dbo].[User] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' order by UserLName Asc";
        objData.ReturnCheckBoxList(query, chkUesrs);
    }
    public void FillStudents()
    {
        string query = "";
        objData = new clsData();
        string buildName = ConfigurationManager.AppSettings["BuildName"].ToString();
        if (buildName == "Local")
        {
            query = "SELECT  StudentId as Id,StudentLname+' , '+StudentFName as Name from Student where SchoolId=" + sess.SchoolId + " and ActiveInd='A' order by StudentLname Asc ";
        }
        else
            query = "SELECT  StudentId as Id,StudentLname+' , '+StudentFName as Name from Student where SchoolId=" + sess.SchoolId + " and ActiveInd='A' order by StudentLname Asc ";
        objData.ReturnCheckBoxList(query, chkStudents);

    }
    private void Clear()
    {
        rdoRadio.SelectedIndex = -1;
        foreach (ListItem ri in chkUesrs.Items)
        {
            ri.Selected = false;
        }
        foreach (ListItem ri in chkStudents.Items)
        {
            ri.Selected = false;
        }
        txtClassCode.Text = "";
        txtCode.Text = "";
        txtDescription.Text = "";
        ClassId = 0;
        txtMaxStudents.Text = "";

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            objData = new clsData();
            if (btnSave.Text == "Save" || btnSave.Text == "Update")
            {


                if (txtCode.Text.Trim() != "")
                {

                    int selectedStud = 0;
                    foreach (ListItem ri in chkStudents.Items)
                    {
                        if (ri.Selected == true)
                        {
                            selectedStud = selectedStud + 1;
                        }

                    }

                    if (ClassId == 0 && btnSave.Text == "Save")
                    {
                        if (clsGeneral.IsExit("ClassName", "Class", txtCode.Text.Trim()) == true)
                        {
                            tdMsg.InnerHtml = clsGeneral.warningMsg("Class Name already exit.Please choose another name.");
                            txtCode.Focus();
                            return;
                        }
                        if (txtMaxStudents.Text == "")
                        {
                            Save();
                            tdMsg.InnerHtml = clsGeneral.sucessMsg("Class Saved Successfully");
                        }
                        else
                        {
                            int classLimit = Convert.ToInt32(txtMaxStudents.Text);
                            if (classLimit >= selectedStud)
                            {
                                Save();
                                tdMsg.InnerHtml = clsGeneral.sucessMsg("Class Saved Successfully");
                            }
                            else
                            {
                                tdMsg.InnerHtml = clsGeneral.warningMsg("This Class is Full");
                                return;
                            }
                        }

                    }
                    else if (ClassId != 0 && btnSave.Text == "Update")
                    {



                        //strQuery = "select count(ClassId) from StdtClass where ClassId='" + ClassId + "' and ActiveInd='A'";
                        //int nofStudent = Convert.ToInt32(objData.FetchValue(strQuery));
                        if (txtMaxStudents.Text == "")
                        {
                            Update();
                            UpdateStudentClass(ClassId);
                            UpdateUserClass(ClassId);
                            tdMsg.InnerHtml = clsGeneral.sucessMsg("Class Updated Successfully");
                        }
                        else
                        {
                            int classLimit = Convert.ToInt32(txtMaxStudents.Text);
                            if (classLimit >= selectedStud)
                            {
                                Update();
                                UpdateStudentClass(ClassId);
                                UpdateUserClass(ClassId);
                                tdMsg.InnerHtml = clsGeneral.sucessMsg("Class Updated Successfully");
                            }
                            else
                            {
                                tdMsg.InnerHtml = clsGeneral.warningMsg("This Class is Full");
                                return;
                            }


                        }
                    }
                }
            }
            if (ClassId != 0 && btnSave.Text == "Delete")
            {
                Delete();
            }
            Clear();
            btnSave.Text = "Save";
            if (lnk_inactive.ForeColor == System.Drawing.Color.Red)
                LoadInactive();
            else
                LoadGroup();
            ClassId = 0;
            Master.fillclass();
        }
        catch (SqlException Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try Again");
            throw Ex;
        }

    }

    private int SaveUserClass(int ClsId, SqlConnection con, SqlTransaction Transs, int userid)
    {
        int retVal = 0;
        objData = new clsData();
        string stQuery = "Insert into UserClass(SchoolId, UserId, ClassId,PrimaryInd, ActiveInd, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) Values (" + sess.SchoolId + ", " + userid + ", " + ClsId + ",'A','A', '" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)))";
        retVal = objData.ExecuteWithTrans(stQuery, con, Transs);
        return retVal;
    }
    private int SaveStudentClass(int ClsId, SqlConnection con, SqlTransaction Transs)
    {
        int retVal = 0;
        objData = new clsData();

        foreach (ListItem li in chkStudents.Items)
        {

            if (li.Selected == true)
            {


                strQuery = "Insert into StdtClass(SchoolId, StdtId, ClassId,PrimaryInd, ActiveInd, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) Values (" + sess.SchoolId + ", " + Convert.ToInt32(li.Value) + ", " + ClsId + ",'A','A', '" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)))";
                retVal = objData.ExecuteWithTrans(strQuery, con, Transs);
            }
        }
        return retVal;
    }
    private void UpdateStudentClass(int ClassId)
    {
        try
        {
            objData = new clsData();
            objClass = new clsClass();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                if (ClassId > 0)
                {

                    foreach (ListItem ri in chkStudents.Items)
                    {
                        if (ri.Selected == true)
                        {
                            if (objClass.StudentClassExit(Convert.ToInt32(ri.Value), ClassId) == false)
                            {
                                strQuery = "Insert into StdtClass(SchoolId, StdtId, ClassId,PrimaryInd, ActiveInd, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) Values (" + sess.SchoolId + ", " + ri.Value + ", " + ClassId + ",'A','A', '" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)))";
                                objData.Execute(strQuery);
                            }
                            else
                            {
                                if (objClass.GetADClassStd(Convert.ToInt32(ri.Value), ClassId) == "D")
                                {
                                    strQuery = "Update StdtClass set ActiveInd='A' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where StdtId=" + ri.Value + " and ClassId=" + ClassId + "";
                                    objData.Execute(strQuery);
                                }
                            }


                        }
                        else
                        {
                            if (objClass.StudentClassExit(Convert.ToInt32(ri.Value), ClassId) == true)
                            {
                                if (objClass.GetADClassStd(Convert.ToInt32(ri.Value), ClassId) == "A")
                                {
                                    strQuery = "Update StdtClass set ActiveInd='D' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where StdtId=" + ri.Value + " and ClassId=" + ClassId + "";
                                    objData.Execute(strQuery);
                                }
                            }
                        }
                    }


                }


            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try Again");
            throw Ex;
        }

    }
    private void UpdateUserClass(int ClassId)
    {
        try
        {
            objData = new clsData();
            objClass = new clsClass();
            SqlTransaction Transs = null;
            SqlConnection con = objData.Open();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                if (ClassId > 0)
                {


                    foreach (ListItem ri in chkUesrs.Items)
                    {
                        if (ri.Selected == true)
                        {
                            if (objClass.UserClassExit(Convert.ToInt32(ri.Value), ClassId) == false)
                            {
                                SaveUserClass(ClassId, con, Transs, Convert.ToInt32(ri.Value));
                            }
                            else
                            {
                                if (objClass.GetADClass(Convert.ToInt32(ri.Value), ClassId) == "D")
                                {
                                    strQuery = "Update UserClass set ActiveInd='A' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where UserId=" + ri.Value + " and ClassId=" + ClassId + "";
                                    objData.Execute(strQuery);
                                }
                            }


                        }
                        else
                        {
                            if (objClass.UserClassExit(Convert.ToInt32(ri.Value), ClassId) == true)
                            {
                                if (objClass.GetADClass(Convert.ToInt32(ri.Value), ClassId) == "A")
                                {
                                    strQuery = "Update UserClass set ActiveInd='D' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=getdate()  where UserId=" + ri.Value + " and ClassId=" + ClassId + "";
                                    objData.Execute(strQuery);
                                }
                            }
                        }
                    }


                }


            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try Again");
            throw Ex;
        }

    }
    private int Save()
    {
        int retVal = 0;
        objData = new clsData();
        SqlTransaction Transs = null;
        SqlConnection con = objData.Open();
        try
        {

            string strMax = "";
            sess = (clsSession)Session["UserSession"];
            clsData.blnTrans = true;

            Transs = con.BeginTransaction();
            if (sess != null)
            {
                int ClsId;
                if (txtMaxStudents.Text != "")
                {
                    string strQuery = "Insert into Class(SchoolId, ClassCd, ClassName,ClassDesc,ResidenceInd,MaxStudents,ActiveInd, CreatedBy, CreateOn, ModifiedBy, ModifiedOn) Values (" + sess.SchoolId + ", '" + txtClassCode.Text.Trim() + "','" + txtCode.Text.Trim() + "','" + txtDescription.Text.Trim() + "','" + rdoRadio.SelectedValue + "','" + Convert.ToInt32(txtMaxStudents.Text) + "','A', '" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)))";
                    ClsId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, Transs));
                }
                else
                {
                    string strQuery = "Insert into Class(SchoolId, ClassCd, ClassName,ClassDesc,ResidenceInd,ActiveInd, CreatedBy, CreateOn, ModifiedBy, ModifiedOn) Values (" + sess.SchoolId + ", '" + txtClassCode.Text.Trim() + "','" + txtCode.Text.Trim() + "','" + txtDescription.Text.Trim() + "','" + rdoRadio.SelectedValue + "','A', '" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)))";
                    ClsId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, Transs));
                }

                foreach (ListItem li in chkUesrs.Items)
                {
                    if (li.Selected == true)
                    {
                        retVal = SaveUserClass(ClsId, con, Transs, Convert.ToInt32(li.Value));
                    }
                }

                retVal = SaveStudentClass(ClsId, con, Transs);
            }
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Class Details Saved Successfully");
            objData.CommitTransation(Transs, con);
        }
        catch (Exception ex)
        {
            objData.RollBackTransation(Transs, con);
            string error = ex.Message;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Class Insertion Failed!");
            throw ex;
        }
        return retVal;
    }
    private bool Update()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            string strMax = "";
            if (txtMaxStudents.Text != "")
            {
                strMax = ",MaxStudents='" + Convert.ToInt32(txtMaxStudents.Text) + "'";
            }
            else
                strMax = ",MaxStudents=''";
            strQuery = "Update [Class] SET SchoolId=" + sess.SchoolId + ",ClassName = '" + txtCode.Text.Trim().ToUpper() + "',ClassCd='" + txtClassCode.Text.Trim().ToUpper() + "',ClassDesc='" + txtDescription.Text.Trim() + "',ResidenceInd='" + rdoRadio.SelectedValue + "'" + strMax + ",ModifiedBy=" + sess.LoginId + ",ModifiedOn=GETDATE() Where ClassId=" + ClassId + "";
            retVal = Convert.ToBoolean(objData.Execute(strQuery));
        }
        return retVal;
    }
    private bool Delete()
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            // string selQry = "select PlacementId from placement where Location=" + ClassId + " AND Status='1'";
            string selQry = "select plc.PlacementId from placement plc where plc.Location=" + ClassId + " and (plc.EndDate is  null or convert(DATE,plc.EndDate) >= convert(DATE,getdate())) and plc.Status=1";
            int exists = Convert.ToInt32(objData.IFExists(selQry));
            if (exists > 0)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Class cannot be Deleted, Placement assigned to Class");
                return false;
            }
            strQuery = "Update [Class] set ActiveInd='D' Where ClassId=" + ClassId + "";
            retVal = Convert.ToBoolean(objData.Execute(strQuery));

            strQuery = "Update StdtClass set ActiveInd='D' Where ClassId=" + ClassId + "";
            retVal = Convert.ToBoolean(objData.Execute(strQuery));

            strQuery = "Update UserClass set ActiveInd='D' Where ClassId=" + ClassId + "";
            retVal = Convert.ToBoolean(objData.Execute(strQuery));

            tdMsg.InnerHtml = clsGeneral.sucessMsg("Class Deleted Successfully");

        }
        return retVal;
    }
    private void LoadGroup()
    {
        if (lnk_active.ForeColor == System.Drawing.Color.Red)
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                DataTable Dt = objData.ReturnDataTable("select Usr.UserLName+ ','+Usr.UserFName as ModifiedUser ,Cls.ClassId,Cls.ClassCd,Cls.ClassName,Cls.ClassDesc,Cls.ClassCd,Cls.ModifiedOn  from (Class Cls INNER JOIN [User] Usr ON Usr.UserId=Cls.CreatedBy) where Cls.ActiveInd<>'D'  And Cls.SchoolId=" + sess.SchoolId + " order by Cls.ClassName", false);
                grdGroup.DataSource = Dt;
                grdGroup.DataBind();
            }
        }
    }
    private void LoadInactive()
    {
        if (lnk_inactive.ForeColor == System.Drawing.Color.Red)
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            if (sess != null)
            {
                DataTable Dt = objData.ReturnDataTable("select Usr.UserLName+ ','+Usr.UserFName as ModifiedUser ,Cls.ClassId,Cls.ClassCd,Cls.ClassName,Cls.ClassDesc,Cls.ClassCd,Cls.ModifiedOn  from (Class Cls JOIN [User] Usr ON Usr.UserId=Cls.CreatedBy) where Cls.ActiveInd<>'A'  And Cls.SchoolId=" + sess.SchoolId + "  order by ClassName", false);
                grdGroup.DataSource = Dt;
                grdGroup.DataBind();
            }
        }
    }
    private void LoadGroupById()
    {
        //Clear();
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null && ClassId != 0)
        {
            DataTable Dt = objData.ReturnDataTable("Select ClassCd,ClassName,ClassDesc,ResidenceInd,SchoolId,MaxStudents from [Class] Where ClassId=" + ClassId + " AND SchoolId=" + sess.SchoolId + " order by ClassName", false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    txtCode.Text = Dt.Rows[0]["ClassName"].ToString();
                    txtDescription.Text = Dt.Rows[0]["ClassDesc"].ToString();
                    txtClassCode.Text = Dt.Rows[0]["ClassCd"].ToString();
                    lblclass.Text = txtCode.Text;
                    lblclass2.Text = txtCode.Text;
                    if (Dt.Rows[0]["MaxStudents"].ToString() == "0" || Dt.Rows[0]["MaxStudents"].ToString() == "")
                    {
                        txtMaxStudents.Text = "";
                    }
                    else
                        txtMaxStudents.Text = Dt.Rows[0]["MaxStudents"].ToString();

                    if (Convert.ToBoolean(Dt.Rows[0]["ResidenceInd"]) == true)
                    {
                        rdoRadio.SelectedIndex = 1;
                    }
                    else
                    {
                        rdoRadio.SelectedIndex = 0;
                    }

                }

                objClass = new clsClass();


                objClass.fillUserCheckBox(chkUesrs, ClassId, sess.SchoolId);
                objClass.fillStudentCheckBox(chkStudents, ClassId, sess.SchoolId);

            }
        }
    }
    protected void grdGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
    }
    protected void grdGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGroup.PageIndex = e.NewPageIndex;
        LoadGroup();
        Clear();
    }
    protected void grdGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            tdMsg.InnerHtml = "";
            btnSave.Text = "Update";
            btnAdd.Visible = true;
        }
        if (e.CommandName == "Delete")
        {
            tdMsg.InnerHtml = "";
            btnSave.Text = "Delete";
        }
        ClassId = Convert.ToInt32(e.CommandArgument);
        LoadGroupById();
        tdMsg.InnerHtml = "";

    }
    protected void grdGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }




    protected void grdGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Disabled == true)
        {

            grdGroup.Columns[5].Visible = false;
            grdGroup.Columns[6].Visible = false;
        }
        else
        {
            grdGroup.Columns[5].Visible = true;
            grdGroup.Columns[6].Visible = true;

        }
    }
    protected void lnk_active_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        lnk_active.ForeColor = System.Drawing.Color.Red;
        lnk_inactive.ForeColor = System.Drawing.Color.Blue;
        LoadGroup();
        foreach (GridViewRow row in grdGroup.Rows)
        {
            ImageButton edit = (ImageButton)row.FindControl("lb_Edit");
            ImageButton delete = (ImageButton)row.FindControl("lb_delete");
            edit.Enabled = true;
            delete.Enabled = true;
        }
    }
    protected void lnk_inactive_Click(object sender, EventArgs e)
    {
        tdMsg.InnerHtml = "";
        lnk_active.ForeColor = System.Drawing.Color.Blue;
        lnk_inactive.ForeColor = System.Drawing.Color.Red;
        LoadInactive();
        foreach (GridViewRow row in grdGroup.Rows)
        {
            ImageButton edit = (ImageButton)row.FindControl("lb_Edit");
            ImageButton delete = (ImageButton)row.FindControl("lb_delete");
            edit.Enabled = false;
            delete.Enabled = false;
        }
    }


    //protected void lbtnClassforStudent_Click(object sender, EventArgs e)
    //{
    //    if (validate() == true)
    //    {
    //        lblclass2.Text = txtCode.Text.Trim();
    //        FillStudents();
    //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);
    //    }
    //}
    //protected void lbtnclassforusers_Click(object sender, EventArgs e)
    //{
    //    if (validate() == true)
    //    {
    //        lblclass.Text = txtCode.Text.Trim();
    //        FillUers();
    //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp1(), true);
    //    }
    //}


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Clear();
        btnSave.Text = "Save";
        tdMsg.InnerHtml = "";
    }
    protected void txtsearch_TextChanged(object sender, EventArgs e)
    {
        objData = new clsData();
        string query = "SELECT  StudentId as Id,StudentLname+' , '+StudentFName as Name from [dbo].[Student] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND StudentLname LIKE '" + txtsearch.Text + "%' order by StudentLname Asc";
        objData.ReturnCheckBoxList(query, chkStudents);
    }
    //----------------------------------------
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        clsData objData = new clsData();
        List<string> name = new List<string>();

        String query = "Select StudentId, StudentLName, StudentFname from Student";
        SqlDataReader drStudent = objData.ReturnDataReader(query, false);
        while (drStudent.Read())
        {
            name.Add(drStudent["StudentLName"].ToString() + "," + drStudent["StudentFname"].ToString());
            var slastname = drStudent["StudentLName"].ToString();

        }

        if (prefixText == "*")
        {
            return (from m in name select m).Take(count).ToArray();
        }
        else
        {
            return (from m in name where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
        }
    }
    //----------------------------------------
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string strNames = txtsearch.Text;
        string[] ArrNames = strNames.Split(',');

        if (ArrNames.Length == 1)
        {
            string query = "SELECT  StudentId as Id,StudentLname+' , '+StudentFName as Name from [dbo].[Student] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND StudentLname LIKE '" + ArrNames[0] + "%' OR StudentFname LIKE '" + ArrNames[0] + "%' order by StudentLname Asc";
            objData.ReturnCheckBoxList(query, chkStudents);
        }
        else if (ArrNames.Length == 2)
        {
            string query = "SELECT  StudentId as Id,StudentLname+' , '+StudentFName as Name from [dbo].[Student] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND StudentLname LIKE '" + ArrNames[0] + "%' OR StudentFname LIKE '" + ArrNames[1] + "%' order by StudentLname Asc";
            objData.ReturnCheckBoxList(query, chkStudents);
        }
        else
        {
            string query = "SELECT  StudentId as Id,StudentLname+' , '+StudentFName as Name from [dbo].[Student] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND StudentLname LIKE '" + strNames + "%' OR StudentFname LIKE '" + strNames + "%' order by StudentLname Asc";
            objData.ReturnCheckBoxList(query, chkStudents);
        }
        objClass = new clsClass(); if (txtClassCode.Text != null) if (ClassId != 0) objClass.fillStudentCheckBox(chkStudents, ClassId, sess.SchoolId);
    }

    //----------------------------------------

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList2(string prefixText, int count, string contextKey)
    {
        clsData objData = new clsData();
        List<string> name = new List<string>();

        String query = "Select UserId, UserLName, UserFname from [User]";
        SqlDataReader drUser = objData.ReturnDataReader(query, false);
        while (drUser.Read())
        {
            name.Add(drUser["UserLName"].ToString() + "," + drUser["UserFname"].ToString());
        }

        if (prefixText == "*")
        {
            return (from m in name select m).Take(count).ToArray();
        }
        else
        {
            return (from m in name where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
        }
    }
    //----------------------------------------

    protected void txtSearchUser_TextChanged(object sender, EventArgs e)
    {
        objData = new clsData();
        string query = "SELECT  UserId as Id,UserLName+' , '+UserFName as Name from [dbo].[User] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND UserLName LIKE '" + txtSearchUser.Text + "%' OR UserFName LIKE '" + txtSearchUser.Text + "%' order by UserLName Asc";
        objData.ReturnCheckBoxList(query, chkUesrs);
    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {

        objData = new clsData();
        string strNames = txtSearchUser.Text;
        string[] ArrNames = strNames.Split(',');

        if (ArrNames.Length == 1)
        {
            string query = "SELECT  UserId as Id,UserLName+' , '+UserFName as Name from [dbo].[User] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND UserLName LIKE '" + ArrNames[0] + "%' OR UserFName LIKE '" + ArrNames[0] + "%' order by UserLName Asc";
            objData.ReturnCheckBoxList(query, chkUesrs);
        }
        else if (ArrNames.Length == 2)
        {
            string query = "SELECT  UserId as Id,UserLName+' , '+UserFName as Name from [dbo].[User] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND UserLName LIKE '" + ArrNames[0] + "%' OR UserFName LIKE '" + ArrNames[1] + "%' order by UserLName Asc";
            objData.ReturnCheckBoxList(query, chkUesrs);
        }
        else
        {
            string query = "SELECT  UserId as Id,UserLName+' , '+UserFName as Name from [dbo].[User] where SchoolId=" + sess.SchoolId + " and ActiveInd='A' AND UserLName LIKE '" + strNames + "%' OR UserFName LIKE '" + strNames + "%' order by UserLName Asc";
            objData.ReturnCheckBoxList(query, chkUesrs);
        }
        objClass = new clsClass(); if (txtClassCode.Text != null) if (ClassId != 0) objClass.fillUserCheckBox(chkUesrs, ClassId, sess.SchoolId);
    }


    //[WebMethod]
    //public static void setUserList()
    //{

    //    objClass = new clsClass();
    //    FillUsers();
    //    if (txtClassCode.Text != null) if (ClassId != 0) objClass.fillUserCheckBox(chkUesrs, ClassId, sess.SchoolId);


    //}
    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    ArrayList ar = new ArrayList();
    //    ht = new Hashtable();
    //    htFalse = new Hashtable();
    //    objClass = new clsClass();
    //    foreach (ListItem item in chkUesrs.Items)
    //    {
    //        if (item.Selected)
    //        {
    //            string id = item.Value;
    //            if (!ht.ContainsKey(id))
    //            {
    //                ht.Add(id, item.Text);
    //            }

    //        }
    //        //else
    //        //{
    //        //    string id = item.Value;
    //        //    if (!htFalse.ContainsKey(id))
    //        //    {
    //        //        htFalse.Add(id, item.Text);
    //        //    }
    //        //}
    //    }
    //   // FillStudents();
    //    if (txtClassCode.Text != null) if (ClassId != 0) objClass.fillStudentCheckBox(chkStudents, ClassId, sess.SchoolId);

    //}

}