using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using CSVParser;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
public partial class Admin_UserCreate : System.Web.UI.Page
{
    public static int successCount = 0;
    public static int failureCount = 0;
    clsSession sess = null;
    clsRoles objRole = null;
    private static int intAddressId = 0;
    //private static int intUserId = 0;
    public int intUserId = 0;
    private static int intSchoolId = 1;
    //  private static int intUserStudentId = 0
    DataClass objDataClass = new DataClass();
    Boolean retVal = false;
    Boolean updateCode = false;
    string strQuery = "";
    clsData objData = null;
    clsClass objclass = null;
    static bool Disable = false;
    static Boolean ResultStatus;
    public static string val = "";
    DataTable usrdata = new DataTable();
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


        if (!(String.IsNullOrEmpty(txtPassword.Text.Trim())))
        {
            txtPassword.Attributes["value"] = txtPassword.Text;
        }
        if (!(String.IsNullOrEmpty(txtConfirmPassword.Text.Trim())))
        {
            txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text;
        }

        if (!usrdata.Columns.Contains("ESesSId"))
        {
            usrdata.Columns.Add("ESesSId");
        }
        if (!usrdata.Columns.Contains("EUserId"))
        {
            usrdata.Columns.Add("EUserId");
        }

        if (Session["UserEdit"] != null)
        {
            UserIdToUpdate.Value = Session["UserEdit"].ToString();
            SessnIDToUpdate.Value = sess.SessionID;
            Session["UpdateData"] = SessnIDToUpdate.Value + "_" + UserIdToUpdate.Value;
        }


        if (Session["UpdateData"] != null)
        {
            string tessss = Session["UpdateData"].ToString();
            string[] t1 = tessss.Split('_');
            if (t1.Length >= 2)
            {
                DataRow usrow = usrdata.NewRow();
                usrow[0] = t1[0].ToString();
                usrow[1] = Convert.ToInt32(t1[1].ToString());
                usrdata.Rows.Add(usrow);
            }

            if (btnAdd.Text == "Update")
            {
                intUserId = Convert.ToInt32(Session["UpdateData"].ToString().Split('_').Last());
            }
        }

        if (!IsPostBack)
        {
            fillDrpGroup();
            fillGroups();
            FillClass();
            rolestar.Visible = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                BtnSubmit.Visible = false;
                submitdiv.Visible = false;
            }
            else
            {
                BtnSubmit.Visible = true;
                submitdiv.Visible = true;

            }

            FillList();
           
            if (Session["UserEdit"] != null)
            {
                if (Session["UpdateData"] != null)
                {
                    intUserId = Convert.ToInt32(Session["UpdateData"].ToString().Split('_').Last());
                }
                else
                {
                    intUserId = Convert.ToInt32(Session["UserEdit"]);
                }
                ClientScript.RegisterStartupScript(this.GetType(), "", "$(document).ready(function(){$('#btnReset').fadeIn('fast');});", true);
            }
            else if (Request.QueryString["MyProfile"] != null)
            {
                intUserId = sess.LoginId;

            }
            else
            {
                intUserId = 0;
                ClientScript.RegisterStartupScript(this.GetType(), "", "resetbtnhide1();", true);
               
            }

            if (usrdata != null)
            {
                for (int i = 0; i < usrdata.Rows.Count; i++)
                {
                    string SesID = usrdata.Rows[i]["ESesSId"].ToString();

                    if (SesID.ToString() == sess.SessionID)
                    {
                        if (intUserId > 0)
                        {
                            txtPassword.Visible = false;
                            star.Visible = false;
                            star2.Visible = false;
                            txtConfirmPassword.Visible = false;
                            lblConfrmPassword.Visible = false;
                            FillUser();
                        }
                    }
                }
            }
        }
        
    }


    private void fillGroups()
    {
        sess = (clsSession)Session["UserSession"];
        objRole = new clsRoles();
        if (sess != null)
        {
            objRole.fillGroups(ddlGroups, sess.SchoolId);
        }
    }
    private void fillDrpGroup()
    {
        objData = new clsData();
        objData.ReturnDropDown("Select GroupId as Id,GroupName as Name from [Group]  order by GroupName Asc", drpGrp);
    }
    private void fillDrpRole()
    {
        if (drpGrp.SelectedIndex > 0)
        {
            objData = new clsData();
            objData.ReturnDropDown("SELECT DISTINCT RoleGroup.RoleGroupId as Id,Role.RoleDesc as Name FROM Role INNER JOIN RoleGroup ON Role.RoleId = RoleGroup.RoleId WHERE RoleGroup.GroupId = " + drpGrp.SelectedValue + "  order by RoleDesc Asc", drpRol);
        }
    }

    private void fillRole()
    {
        chkRole.Items.Clear();
        objData = new clsData();

        objData.ReturnCheckBoxList("SELECT DISTINCT RoleGroup.RoleGroupId as Id,Role.RoleDesc as Name FROM Role INNER JOIN RoleGroup ON Role.RoleId = RoleGroup.RoleId WHERE RoleGroup.GroupId = " + ddlGroups.SelectedValue + " order by RoleDesc Asc", chkRole);

        if (chkRole.Items.Count > 0)
        {
            tdMsg.InnerHtml = "";
            //tdRoles.Visible = true;
        }
        else
        {
            //tdRoles.Visible = false;
            lblrole.Visible = false;
            rolestar.Visible = false;
            tdMsg.InnerHtml = clsGeneral.warningMsg("No Roles Found  Under " + ddlGroups.SelectedItem.Text + " !!!..");
        }

    }



    protected void FillClass()
    {
        try
        {
            objData = new clsData();
            DataTable Classdata = objData.ReturnDataTable("SELECT ClassId as Id,ClassName as Name  from [Class] where ActiveInd = 'A'", true);
            if (Classdata.Rows.Count > 0)
            {
                DLclass.DataSource = Classdata;
                DLclass.DataBind();
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! ..Please Try Later...");
            throw Ex;
        }

    }
    protected void FillList()
    {
        try
        {
            objData = new clsData();
            objData.ReturnDropDown("SELECT LookupId as Id,LookupName as Name  from LookUp where LookupType = 'Country'", ddlCountry);
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! ..Please Try Later...");
            throw Ex;
        }

    }



    private void FillData()
    {
        objRole = new clsRoles();
        sess = (clsSession)Session["UserSession"];
        objRole.fillRolePermissionsUserRights(chkRole, Convert.ToInt32(ddlGroups.SelectedValue), intUserId, sess.SchoolId, txtDescription);

    }
    protected void FillUser()
    {
        if (Convert.ToString(Session["StatusUser"]) == "Inactive")
        {
            lblStatus.Visible = true;
            ddlStatus.Visible = true;
            ddlStatus.SelectedIndex = 2;
        }
        objData = new clsData();
        lblpsword.Visible = false;
        string strQuery = "SELECT  usr.UserFName, usr.UserLName, usr.UserNo,convert(varchar(50), usr.Login) as login, convert(varchar(50), usr.Password) as password,usr.Gender,usr.Position, usr.ManagerId,   " +
                          "usr.CreatedBy, usr.CreatedOn, Adr.AddressLine1, Adr.AddressLine2, Adr.AddressLine3, Adr.Country, Adr.State,  " +
                          "Adr.City, Adr.Zip, Adr.HomePhone, Adr.Mobile, Adr.Email, Adr.CreatedBy AS Expr1, Adr.CreatedOn AS Expr2 ,  " +
                          "Adr.ModifiedBy, Adr.ModifiedOn  ,Adr.AddressId as AddId ,CONVERT(varchar,Password) as password, usr.UserEmail as UsrEmail " +
               "FROM      [User] usr LEFT JOIN Address Adr " +
               "ON        usr.AddressId = Adr.AddressId " +
               "where     usr.UserId=" + intUserId + " ";

        DataTable Dt = objData.ReturnDataTable(strQuery, false);
        string StrClass = "SELECT UsrCls.ClassId AS ClassId FROM UserClass UsrCls INNER JOIN Class Cls ON Cls.ClassId=UsrCls.ClassId WHERE UsrCls.UserId=" + intUserId + " AND Cls.ActiveInd='A' And UsrCls.ActiveInd='A' ";

        DataTable DtClass = new DataTable();
        DtClass = objData.ReturnDataTable(StrClass, false);
        try
        {

            if ((Dt.Rows.Count > 0 && DtClass.Rows.Count > 0) || Dt != null)
            {
                btnAdd.Text = "Update";
                txtFirstName.Text = Dt.Rows[0]["UserFName"].ToString().Trim();
                txtLastName.Text = Dt.Rows[0]["UserLName"].ToString().Trim();
                txtUserNo.Text = Dt.Rows[0]["UserNo"].ToString().Trim();
                txtLogin.Text = Dt.Rows[0]["login"].ToString().Trim();
                txtAddress1.Text = Dt.Rows[0]["AddressLine1"].ToString().Trim();
                txtAddress2.Text = Dt.Rows[0]["AddressLine2"].ToString().Trim();
                txtAddress3.Text = Dt.Rows[0]["AddressLine3"].ToString().Trim();
                ddlGender.Text = Dt.Rows[0]["Gender"].ToString().Trim();
                ddlCountry.SelectedValue = Dt.Rows[0]["Country"].ToString().Trim();
                txtEmail2.Text = Dt.Rows[0]["UsrEmail"].ToString().Trim();
                try
                {
                    objData.ReturnDropDown("SELECT LookupId as Id,LookupName as Name from LookUp where ParentLookupId = " + ddlCountry.SelectedValue + " AND LookupType = 'State'", ddlState);
                    ddlState.SelectedValue = Dt.Rows[0]["State"].ToString().Trim();
                }
                catch (Exception Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Error!!! Please try after Sometime!!!!!");
                    throw Ex;
                }
                txtposition.Text = Dt.Rows[0]["Position"].ToString().Trim();
                txtCity.Text = Dt.Rows[0]["City"].ToString().Trim();
                txtZip.Text = Dt.Rows[0]["Zip"].ToString();
                txtHomePhone.Text = Dt.Rows[0]["HomePhone"].ToString().Trim();
                txtMobile.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                txtEmail.Text = Dt.Rows[0]["Email"].ToString().Trim();
                txtAddressId.Text = Dt.Rows[0]["AddId"].ToString().Trim();
                Session["UserEdit"] = null;
                Session["StatusUser"] = null;
                if (Dt.Rows[0]["password"].ToString().Trim() == "")
                {
                    LBpwrest.Text = "Password will be reset by user upon next login";
                    btnReset.Visible = false;
                }else
                {
                    btnReset.Visible = true;
                }

                FillClass();
                for (int i = 0; i < DtClass.Rows.Count; i++)
                {
                    foreach (DataListItem item in DLclass.Items)
                    {
                        CheckBox chklist = (CheckBox)item.FindControl("chkClass");
                        HiddenField hf = (HiddenField)item.FindControl("hdnClass");

                        if (hf.Value.ToString() == DtClass.Rows[i]["ClassId"].ToString().Trim())
                            chklist.Checked = true;
                    }

                }
                fillGroups();
                string strRole = "SELECT URG.RoleGroupId,GP.GroupId from UserRoleGroup URG INNER JOIN RoleGroup RG ON URG.RoleGroupId=RG.RoleGroupId INNER JOIN [Group] GP ON RG.GroupId=GP.GroupId where URG.UserId=" + intUserId + " ";
                DataTable dtrole = new DataTable();
                dtrole = objData.ReturnDataTable(strRole, false);
                if (dtrole.Rows.Count > 0)
                    ddlGroups.SelectedValue = dtrole.Rows[0]["GroupId"].ToString().Trim();
                ddlGroups_SelectedIndexChanged(this, EventArgs.Empty);
                FillData();

                bool chk = true;
                foreach (DataListItem item in DLclass.Items)
                {
                    CheckBox chklist = (CheckBox)item.FindControl("chkClass");

                    if (chklist.Checked == false)
                    {
                        chk = false;
                        break;
                    }

                }
                if (chk == true)
                {
                    Chkselall.Checked = true;
                }
                else
                {
                    Chkselall.Checked = false;
                }
            }

        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Input Data Entry Error!!!!!");
            throw Ex;
        }

    }

    protected void SaveAddress() //to fill Address table
    {

    }

    private bool Validation()
    {
        clsLoginValues objLogins = null;
        objLogins = (clsLoginValues)Session["ActiveLogin"];

        if (txtFirstName.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter First Name");
            txtFirstName.Focus();
            return false;
        }
        else if (txtLastName.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Last Name");
            txtLastName.Focus();
            return false;
        }
        else if (txtposition.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select/Enter Position");
            txtposition.Focus();
            return false;
        }
        //else if (ddlGender.SelectedIndex == 0)
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Gender");
        //    ddlGender.Focus();
        //    return false;
        //}
        else if (txtUserNo.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter User Initial");
            txtUserNo.Focus();
            return false;
        }
        else if (txtLogin.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Login Name");
            txtLogin.Focus();
            return false;

        }
        else if (txtHomePhone.Text != "" && clsGeneral.IsItValidPhone(txtHomePhone.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Phone Number Must Be Entered As: (xxx)xxx-xxxx");
            txtHomePhone.Focus();
            return false;
        }
        else if (txtPassword.Text.Trim() == "" && intUserId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Password");
            txtPassword.Focus();
            return false;
        }

        else if (txtConfirmPassword.Text.Trim() == "" && intUserId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Confirm Password");
            txtConfirmPassword.Focus();
            return false;
        }


        //else if (txtAddress1.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Adress1");
        //    txtAddress1.Focus();
        //    return false;
        //}

        //else if (ddlCountry.SelectedIndex == 0)
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Country");
        //    ddlCountry.Focus();
        //    return false;
        //}
        //else if (ddlState.SelectedIndex == 0)
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select State");
        //    ddlState.Focus();
        //    return false;
        //}

        else if (ddlGroups.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Group");
            ddlGroups.Focus();
            return false;
        }
        else if (chkRole.SelectedIndex == -1)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Role ");
            chkRole.Focus();
            return false;
        }
        else if (txtPassword.Text != "" && txtPassword.Visible == true && clsGeneral.IsItValidPassword(txtPassword.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid Password of about 6-12 characters ");
            txtPassword.Focus();
            return false;
        }
        else if (txtZip.Text != "" && clsGeneral.IsItZip(txtZip.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Valid Zip Code");
            txtZip.Focus();
            return false;
        }
        else if (txtMobile.Text != "" && clsGeneral.IsItValidPhone(txtMobile.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Mobile Number Must Be Entered As: (xxx)xxx-xxxx");
            txtMobile.Focus();
            return false;
        }
        else if (txtEmail.Text != "" && clsGeneral.IsItEmail(txtEmail.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid E-Mail");
            txtEmail.Focus();
            return false;
        }
        //else if (clsAciveDirectory.IsActiveDirectoryExit(objLogins.LoginVals, objLogins.DomainName.ToString(), txtLogin.Text.ToString().Trim()) == false)
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Username does not exist in the acive directory");
        //    txtLogin.Focus();
        //    return false;
        //}


        return true;

    }
    protected void SaveUser() //to fill User table
    {
        if (Validation() == true)
        {

            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                objData = new clsData();

                if (Convert.ToInt32(objData.FetchValue("SELECT COUNT(*) FROM [User] WHERE Login=CONVERT(varbinary(50),'" + txtLogin.Text.Trim() + "') and ActiveInd='A'")) == 0)
                {
                    clsData.blnTrans = true;
                    SqlConnection con = objData.Open();
                    SqlTransaction Trans = con.BeginTransaction();

                    try
                    {
                        sess = (clsSession)Session["UserSession"];
                        intSchoolId = sess.SchoolId;
                        strQuery = " Insert into Address(AddressLine1 ,AddressLine2 ,AddressLine3 ,City ,State ,Country ,Zip ,HomePhone ,Mobile ,Email ,CreatedBy ,CreatedOn ,ModifiedBy ,ModifiedOn) Values(   '" + clsGeneral.convertQuotes(txtAddress1.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtAddress2.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtAddress3.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCity.Text.Trim()) + "','" + ddlState.SelectedValue + "','" + ddlCountry.SelectedValue + "','" + txtZip.Text.Trim() + "','" + txtHomePhone.Text.Trim() + "','" + txtMobile.Text.Trim() + "' ,'" + clsGeneral.convertQuotes(txtEmail.Text.Trim()) + "' ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) )";
                        intAddressId = objData.ExecuteWithScopeandConnection(strQuery, con, Trans);
                        string UsrEmailid = "";
                        if((txtEmail2.Text != null) && (txtEmail2.Text != ""))
                        {
                            UsrEmailid = txtEmail2.Text;
                        }
                        else{
                            UsrEmailid = "";
                        }
                        strQuery = "Insert into [User](AddressId ,SchoolId,UserNo,UserInitial,UserFName ,UserLName ,Login ,Position,Gender,Password ,ManagerId, ActiveInd ,CreatedBy ,CreatedOn,ModifiedBy ,ModifiedOn, UserEmail) values(" + intAddressId + ", " + intSchoolId + ", '" + clsGeneral.convertQuotes(txtUserNo.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtUserNo.Text.Trim()) + "', '" + clsGeneral.convertQuotes(txtFirstName.Text.Trim()) + "', '" + clsGeneral.convertQuotes(txtLastName.Text.Trim()) + "',convert(varbinary(80), '" + txtLogin.Text.Trim() + "'),'" + txtposition.Text.Trim() + "','" + ddlGender.SelectedItem.Text + "',convert(varbinary(80),'" + txtPassword.Text.Trim() + "'), " + 0 + ",'A' ," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'"+ UsrEmailid +"')";
                        intUserId = objData.ExecuteWithScopeandConnection(strQuery, con, Trans);
                        Save(intUserId, con, Trans);
                        SaveClass(intUserId, con, Trans);
                        objData.CommitTransation(Trans, con);
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("User added Successfully.");
                        TextClear();
                        Response.Redirect("UserList.aspx");
                    }
                    catch (SqlException Ex)
                    {
                        objData.RollBackTransation(Trans, con);
                        tdMsg.InnerHtml = clsGeneral.failedMsg("User Insertion Failed!");
                        throw Ex;
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Login ID already exist!");
                }
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Password does not match!");
            }

        }


    }

    protected void SaveClass(int UserID, SqlConnection con, SqlTransaction Transs) //fill UserClass table
    {
        foreach (DataListItem item in DLclass.Items)
        {
            CheckBox chklist = (CheckBox)item.FindControl("chkClass");
            HiddenField hf = (HiddenField)item.FindControl("hdnClass");
            if (chklist.Checked == true)
            {
                string strQuery = "Insert into UserClass(SchoolId, UserId, ClassId,PrimaryInd, ActiveInd, CreatedBy, CreatedOn,ModifiedBy,ModifiedOn) Values (" + sess.SchoolId + ", " + UserID + ", " + Convert.ToInt16(hf.Value) + ",'A','A', '" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) )";
                int ClassId = objData.ExecuteWithTrans(strQuery, con, Transs);
            }

        }
    }
    protected void USaveClass(int UserID, int classid) //fill UserClass table
    {
        string strQuery = "Insert into UserClass(SchoolId, UserId, ClassId,PrimaryInd, ActiveInd, CreatedBy, CreatedOn,ModifiedBy,ModifiedOn) Values (" + sess.SchoolId + ", " + UserID + ", " + classid + ",'A','A', '" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) )";
        int ClassId = objData.Execute(strQuery);
    }
    protected void USaveRole(int UserID, int rgpid)
    {
        strQuery = "Insert into UserRoleGroup(SchoolId,UserRoleGroupDesc,UserId,RoleGroupId,EffStartDate,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
        strQuery += "values(" + sess.SchoolId + " ,'" + txtDescription.Text.Trim() + "'," + UserID + "," + rgpid + ",(SELECT Convert(Varchar,getdate(),100)),'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))) ";
        objData.Execute(strQuery);
    }


    protected void SaveUserRole() //to fill UserRole table
    {

    }

    protected void updateclass(int Userid)
    {
        objclass = new clsClass();
        clsRoles objRole = new clsRoles();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            if (Userid > 0)
            {
                foreach (DataListItem item in DLclass.Items)
                {
                    CheckBox chklist = (CheckBox)item.FindControl("chkClass");
                    HiddenField hf = (HiddenField)item.FindControl("hdnClass");
                    if (chklist.Checked == true)
                    {
                        if (objclass.UserClassExit(Userid, Convert.ToInt32(hf.Value)) == false)
                        {
                            USaveClass(Userid, Convert.ToInt32(hf.Value));
                        }
                        else
                        {
                            if (objclass.GetADClass(Userid, Convert.ToInt32(hf.Value)) == "D")
                            {
                                strQuery = "Update UserClass set ActiveInd='A' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where UserId=" + Userid + " and ClassId=" + Convert.ToInt32(hf.Value) + "";
                                objData.Execute(strQuery);
                            }
                        }

                    }

                    else
                    {
                        if (objclass.UserClassExit(Userid, Convert.ToInt32(hf.Value)) == true)
                        {
                            if (objclass.GetADClass(Userid, Convert.ToInt32(hf.Value)) == "A")
                            {
                                strQuery = "Update UserClass set ActiveInd='D' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where UserId=" + Userid + " and ClassId=" + Convert.ToInt32(hf.Value) + "";
                                objData.Execute(strQuery);
                            }
                        }
                    }
                }
            }
        }
    }



    protected void UpdateUserAndAddress()
    {
        if (Validation() == true)
        {
            try
            {
                objData = new clsData();
                sess = (clsSession)Session["UserSession"];
                int intSchoolId = sess.SchoolId;
                int addressUpdateId;

                if (ddlStatus.SelectedItem.Text == "Active")
                {
                    string active = "Update [User] Set ActiveInd='A' Where UserId=" + intUserId + "";
                    ResultStatus = Convert.ToBoolean(objData.Execute(active));
                }
                string selctAdress = "SELECT AddressId from [User] where UserId = " + intUserId + "";
                addressUpdateId = Convert.ToInt32(objData.FetchValue(selctAdress));

                strQuery = " Update Address Set AddressLine1='" + clsGeneral.convertQuotes(txtAddress1.Text.Trim()) + "'  ,AddressLine2='" + clsGeneral.convertQuotes(txtAddress2.Text.Trim()) + "',AddressLine3='" + clsGeneral.convertQuotes(txtAddress3.Text.Trim()) + "',City='" + clsGeneral.convertQuotes(txtCity.Text.Trim()) + "',State='" + ddlState.Text.Trim() + "',Country='" + ddlCountry.Text.Trim() + "',Zip='" + txtZip.Text.Trim() + "',HomePhone='" + txtHomePhone.Text.Trim() + "',Mobile='" + txtMobile.Text.Trim() + "',Email='" + clsGeneral.convertQuotes(txtEmail.Text.Trim()) + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn= (SELECT Convert(Varchar,getdate(),100)) where AddressId=" + addressUpdateId + "";

                retVal = Convert.ToBoolean(objData.Execute(strQuery));

                string UsrEmailid = "";
                if ((txtEmail2.Text != null) && (txtEmail2.Text != ""))
                {
                    UsrEmailid = txtEmail2.Text;
                }
                else
                {
                    UsrEmailid = "";
                }
                strQuery = "Update [User] Set AddressId='" + addressUpdateId + "' , SchoolId='" + intSchoolId + "' , UserNo='" + clsGeneral.convertQuotes(txtUserNo.Text.Trim()) + "' ,UserInitial='" + clsGeneral.convertQuotes(txtUserNo.Text.Trim()) + "',UserFname='" + clsGeneral.convertQuotes(txtFirstName.Text.Trim()) + "' ,UserLname='" + clsGeneral.convertQuotes(txtLastName.Text.Trim()) + "', Login= convert(varbinary(80),'" + txtLogin.Text.Trim() + "'), Gender = '" + ddlGender.Text.Trim() + "',Position='" + txtposition.Text.Trim() + "', ManagerId='" + 0 + "' ,ModifiedBy='" + sess.LoginId + "' ,ModifiedOn= (SELECT Convert(Varchar,getdate(),100)), UserEmail='" + UsrEmailid + "'  where UserId=" + intUserId + "";
                updateCode = Convert.ToBoolean(objData.Execute(strQuery));
                updateclass(intUserId);
                UpdateRole(intUserId);
                if (retVal == true && updateCode == true || ResultStatus == true)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg(" User Updated Successfully.");
                }

                else
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg(" User Updation Failed!");
                }




            }
            catch (Exception Ex)
            {
                string error = Ex.Message;
                tdMsg.InnerHtml = clsGeneral.failedMsg(" User Updation Failed!");
                throw Ex;
            }


        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (btnAdd.Text == "Update")
        {
            for (int i = 0; i < usrdata.Rows.Count; i++)
            {
                string SsnID = usrdata.Rows[i]["ESesSId"].ToString();
                string UsrID = usrdata.Rows[i]["EUserId"].ToString();
                if (SsnID.ToString() == sess.SessionID && intUserId == Convert.ToInt32(UsrID.ToString()))
                {
                    if (intUserId > 0)
                    {
                        UpdateUserAndAddress();
                    }
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg(" User Updation Failed!");
                }
            }
        }
        else if (btnAdd.Text == "Save")
        {
            SaveUser();
        }
        else
        {
            tdMsg.InnerHtml = "Wrong Choice";
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
        //if (intUserId > 0)
        //{
        //    //intAddressId = Convert.ToInt32(txtAddressId.Text);
        //    UpdateUserAndAddress();

        //}
        //else
        //{
        //    SaveUser();
        //    //UserId = intUserId;    

        //}

    }

    private void Save(int UserId, SqlConnection con, SqlTransaction Transs)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            objRole = new clsRoles();
            if (sess != null)
            {
                foreach (ListItem ri in chkRole.Items)
                {
                    if (ri.Selected == true)
                    {
                        strQuery = "Insert into UserRoleGroup(SchoolId,UserRoleGroupDesc,UserId,RoleGroupId,EffStartDate,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
                        strQuery += "values(" + sess.SchoolId + " ,'" + txtDescription.Text.Trim() + "'," + UserId + "," + ri.Value + ",(SELECT Convert(Varchar,getdate(),100)),'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))) ";
                        objData.ExecuteWithTrans(strQuery, con, Transs);

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

    private void UpdateRole(int UserId)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            objRole = new clsRoles();
            if (sess != null)
            {
                if (UserId > 0)
                {
                    if (objclass.UserRoleExit(UserId))
                    {
                        strQuery = "Delete from UserRoleGroup where UserId=" + UserId + " ";
                        objData.Execute(strQuery);
                    }
                    foreach (ListItem ri in chkRole.Items)
                    {
                        if (ri.Selected == true)
                        {
                            USaveRole(UserId, Convert.ToInt32(ri.Value));
                        }
                        //else
                        //{
                        //        if (objclass.GetADRole(UserId, Convert.ToInt32(ri.Value)) == "D")
                        //        {
                        //            strQuery = "Update UserRoleGroup set ActiveInd='A' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where UserId=" + UserId + " and RoleGroupId=" + Convert.ToInt32(ri.Value) + "";
                        //            objData.Execute(strQuery);
                        //        }
                        //        else
                        //        {
                        //            if (objclass.UserRoleExit(UserId) == true)
                        //            {
                        //                if (objclass.GetADRole(UserId, Convert.ToInt32(ri.Value)) == "A")
                        //                {
                        //                    strQuery = "Update UserRoleGroup set ActiveInd='D' ,ModifiedBy=" + sess.LoginId + " ,ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where UserId=" + UserId + " and RoleGroupId=" + Convert.ToInt32(ri.Value) + "";
                        //                    objData.Execute(strQuery);
                        //                }
                        //            }
                        //        }
                        //}
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



    protected void BtnSubmit_Click(object sender, EventArgs e)
    {

    }


    [System.Web.Services.WebMethod(EnableSession = true)]

    public static string changePassword(string currPwd, string newPwd, string conPwd, string usrid)
    {
        clsData objData = new clsData();

        int intuservalid = 0;
        if (usrid != "")
        {
            intuservalid = Convert.ToInt32(usrid);
        }

        DataClass objDataClass = new DataClass();

        //bool isPasswordExit = objData.IFExists("SELECT UserId from [User] where convert(varchar(50),Password)='" + currPwd + "' AND UserId = " + intUserId + "");
        bool isPasswordExit = objData.IFExists("SELECT UserId from [User] where convert(varchar(50),Password)='" + currPwd + "' AND UserId = " + intuservalid + "");
        string msg = "";
        if (isPasswordExit == true)
        {
            if (newPwd == conPwd)
            {
                objData = new clsData();
                //string insertQuerry = "UPDATE [User] Set Password = convert(varbinary(80), '" + newPwd + "') where userId = " + intUserId + "";
                string insertQuerry = "UPDATE [User] Set Password = convert(varbinary(80), '" + newPwd + "') where userId = " + intuservalid + "";
                int index = objDataClass.ExecuteNonQuery(insertQuerry);
                if (index > 0)
                {
                    msg = "Password Updated";
                }
                else
                {
                    msg = "Password not Updated...!! Error Found!!!!";
                }

            }
            else
            {
                //tdMsg.InnerHtml = clsGeneral.failedMsg("Given New Password matching Failed");
                msg = "Given New Password matching Failed";
            }

        }
        else
        {
            msg = "Password Matching Failed";
            //tdMsg.InnerHtml = clsGeneral.failedMsg("Password Matching Failed");
        }



        return msg;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]

    public static string ResetPassword(string status, string usrid)
    {
        clsData objData = new clsData();

        int intuservalid = 0;
        if (usrid != "")
        {
            intuservalid = Convert.ToInt32(usrid);
        }

        DataClass objDataClass = new DataClass();
        string msg = "";
      
         
                objData = new clsData();
                //string insertQuerry = "UPDATE [User] Set Password = convert(varbinary(80), '') where userId = " + intUserId;
                string insertQuerry = "UPDATE [User] Set Password = convert(varbinary(80), '') where userId = " + intuservalid;
                int index = objDataClass.ExecuteNonQuery(insertQuerry);
                if (index > 0)
                {
                    msg = "Password will be reset by user upon next login";
                }
                else
                {
                    msg = "Password reset Failed";
                }

        return msg;
    }




    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCountry.SelectedIndex != 0)
            {
                objData = new clsData();
                ddlState.Items.Clear();
                int countryId = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
                objData.ReturnDropDown("SELECT LookupId As Id,LookupName As Name from LookUp where ParentLookupId = " + countryId + " AND LookupType = 'State'", ddlState);
            }
            else
            {
                ddlState.Items.Clear();
                ddlState.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
            }
        }

        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;

        }

    }

    protected void TextClear()
    {
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtAddress3.Text = "";
        txtAddressId.Text = "";
        txtCity.Text = "";
        txtconfirmnewPasword.Text = "";
        txtConfirmPassword.Text = "";
        txtCurrentpsword.Text = "";
        txtEmail.Text = "";
        txtFirstName.Text = "";
        txtHomePhone.Text = "";
        txtLastName.Text = "";
        txtLogin.Text = "";
        txtMobile.Text = "";
        txtnewpaswrd.Text = "";
        txtPassword.Text = "";
        txtUserNo.Text = "";
        txtZip.Text = "";
        txtposition.Text = "";
        ddlCountry.SelectedIndex = 0;
        ddlGroups.SelectedIndex = 0;
        chkRole.Visible = false;
        lblrole.Visible = false;
        ddlGender.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        txtPassword.Attributes["value"] = "";
        txtConfirmPassword.Attributes["value"] = "";
        txtPassword.Text = "";
        txtConfirmPassword.Text = "";
        Session["UserEdit"] = null;
        txtDescription.Text = "";
        rolestar.Visible = false;
        intUserId = 0;
        foreach (DataListItem item in DLclass.Items)
        {
            CheckBox chklist = (CheckBox)item.FindControl("chkClass");
            if (chklist.Checked == true)
            {
                chklist.Checked = false;
            }
        }

    }



    private bool ValidationUpdate()
    {
        if (txtFirstName.Text == "")
        {
            tdMsg.InnerHtml = "Please Enter First Name";
            txtFirstName.Focus();
            return false;
        }
        else if (txtLastName.Text == "")
        {
            tdMsg.InnerHtml = "Please Enter Last Name";
            txtLastName.Focus();
            return false;
        }

        //else if (ddlGender.Text == "")
        //{
        //    tdMsg.InnerHtml = "Please Select Gender";
        //    ddlGender.Focus();
        //    return false;
        //}
        else if (txtUserNo.Text == "")
        {
            tdMsg.InnerHtml = "Please Enter User Initial";
            txtUserNo.Focus();
            return false;
        }
        else if (txtLogin.Text == "")
        {
            tdMsg.InnerHtml = "Please Enter Login Name";
            txtLogin.Focus();
            return false;
        }

        else if (txtAddress1.Text == "")
        {
            tdMsg.InnerHtml = "Please Enter Adress1";
            txtAddress1.Focus();
            return false;
        }

        else if (ddlCountry.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select Country");
            ddlCountry.Focus();
            return false;
        }
        else if (ddlState.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select State");
            ddlState.Focus();
            return false;
        }

        else if (txtZip.Text != "")
        {
            if (clsGeneral.IsItZip(txtZip.Text.Trim()) == false)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid Zip Code ");
                txtZip.Focus();
                return false;
            }
        }
        else if (txtHomePhone.Text != "")
        {
            if (clsGeneral.IsPhoneNumber(txtHomePhone.Text.Trim()) == false)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid Land Phone Number in the form (xxx)xxx-xxx ");
                txtHomePhone.Focus();
                return false;
            }
        }

        else if (txtEmail.Text != "")
        {
            if (clsGeneral.IsItEmail(txtEmail.Text.Trim()) == false)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Valid E-Mail");
                txtEmail.Focus();
                return false;
            }

        }
        return true;

    }

    protected void ddlGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGroups.SelectedIndex != 0)
        {
            rolestar.Visible = true;
            lblrole.Visible = true;
            fillRole();
        }
        else
        {
            lblrole.Visible = false;
            rolestar.Visible = false;
        }


    }
    //protected void DLclass_ItemDataBound(object sender, DataListItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item&&i==0)
    //    {
    //        CheckBoxList cbl = (CheckBoxList)e.Item.FindControl("chkClass");
    //        if (cbl != null)
    //        {
    //            objData = new clsData();
    //            DataTable Classdata = objData.ReturnDataTable("SELECT ClassId as Id,ClassName as Name  from [Class] where ActiveInd = 'A'", true);
    //            if (Classdata.Rows.Count > 0)
    //            {
    //                cbl.DataSource = Classdata;
    //                cbl.DataBind();
    //            }
    //        }
    //        i++;
    //    }
    //}
    protected void txtposition_TextChanged(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        clsData objData = new clsData();
        List<string> name = new List<string>();

        String query = "SELECT LookupName as Name  from LookUp where LookupType = 'Position'";

        SqlDataReader dr = objData.ReturnDataReader(query, false);

        while (dr.Read())
        {

            name.Add(dr["Name"].ToString());

        }

        dr.Close();

        return (from m in name where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();


    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Response.Redirect("UserList.aspx", false);

    }
    protected void btn_bulkUp_Click(object sender, EventArgs e)
    {
        string arr_FName = "";
        int count = 0;
        int countRejected = 0;
        int duplicatecount = 0;
        bool flagCheck = false;
        bool flag = false;
        bool itemRegexFirst = false;
        bool itemRegexLast = false;
        string duplicate = "";
        if (drpGrp.SelectedIndex != 0)
        {
            if (drpRol.SelectedIndex != 0)
            {
                if (fileBulk.HasFile)
                {
                    string ext = System.IO.Path.GetExtension(fileBulk.PostedFile.FileName);
                    if (ext.ToLower() == ".csv")
                    {
                        clsData objData = new clsData();
                        CSVPraser csv = new CSVPraser(',');
                        csv.HasFieldsEnclosedInQuotes = true;
                        List<string> FileParseError = null;
                        Stream file = fileBulk.FileContent;
                        var ListData = csv.CSVToObject<InputData>(file, ",", out FileParseError);
                        if (FileParseError.Count == 0)
                        {
                            foreach (var item in ListData)
                            {
                                string stringFirstName = item.FirstName;
                                string stringLastName = item.LastName;
                                itemRegexFirst = stringFirstName.Any(ch => char.IsSymbol(ch));
                                itemRegexLast = stringFirstName.Any(ch => char.IsSymbol(ch));
                                if (!itemRegexFirst && !itemRegexLast)
                                {
                                    byte[] LoginCheck = Encoding.UTF8.GetBytes(item.LogonName);
                                    bool loginExist = objData.IFExists("SELECT Login FROM [User] WHERE Login=CONVERT(VARBINARY(80),'" + item.LogonName + "') AND ActiveInd='A' ");
                                    if (!loginExist)
                                    {
                                        val = drpRol.SelectedValue;
                                        item.SaveAllUsersReportData();
                                        count++;
                                    }
                                    else
                                    {
                                        duplicatecount++;
                                        duplicate += item.LogonName + ",";
                                    }
                                }
                                else
                                {
                                    flagCheck = true;
                                    countRejected++;
                                }
                            }
                            //if (duplicate != "")
                            //{
                            //    if (count == 0)
                            //    {
                            //        divMessage.InnerHtml = clsGeneral.warningMsg("" + count + " User Inserted" + "<br />" + "Duplicate : " + duplicatecount);
                            //        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
                            //    }
                            //    else
                            //    {
                            //        divMessage.InnerHtml = clsGeneral.warningMsg("Duplicate : " + duplicatecount);
                            //    }
                            //    //DivDuplicate.InnerHtml = clsGeneral.warningMsg("Duplicate : " + duplicatecount);
                            //        //duplicate;
                            //}

                        }
                        else
                        {
                            flag = true;
                            divMessage.InnerHtml = clsGeneral.warningMsg(FileParseError[0]);
                            ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);

                        }
                    }
                    else
                    {
                        flag = true;
                        divMessage.InnerHtml = clsGeneral.warningMsg("Please select a CSV File");
                        ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
                    }
                }
                else
                {
                    flag = true;
                    divMessage.InnerHtml = clsGeneral.warningMsg("Please select a CSV File");
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);

                }
            }
            else
            {
                flag = true;
                divMessage.InnerHtml = clsGeneral.warningMsg("Please select Role");
                ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);

            }

        }
        else
        {
            flag = true;
            divMessage.InnerHtml = clsGeneral.sucessMsg("Please select Group");
            ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);

        }
        if (!flag)
        {
            //if (duplicate != "")
            //{
            //if (count == 0)
            //{
            //divMessage.InnerHtml = clsGeneral.warningMsg("Duplicate : " + duplicatecount);
            //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
            //}
            //else
            //{
            if (duplicatecount == 0)
            {
                lnkDupDownload.Style.Add("display", "none");
                divMessage.InnerHtml = clsGeneral.sucessMsg("" + count + " User Inserted Successfully");
                ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
                
            }
            else
            {
                lnkDupDownload.Style.Add("display", "block");
                GenerateFile(duplicate);
                string Message = "User Inserted : " + count + ". <br />Duplicate : " + duplicatecount;
                divMessage.InnerHtml = clsGeneral.warningMsg(Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
                
            }
            //}
            //DivDuplicate.InnerHtml = clsGeneral.warningMsg("Duplicate : " + duplicatecount);
            //duplicate;
            //}
            //if (count != 0)
            //{
            //    divMessage.InnerHtml = clsGeneral.sucessMsg("" + count + " User Inserted Successfully");
            //    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
            //}
            //else
            //{
            //    divMessage.InnerHtml = clsGeneral.failedMsg("" + count + " User Inserted");
            //    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
            //}
            if (flagCheck)
            {
                nameCheck.InnerHtml = clsGeneral.warningMsg("" + countRejected + " User Rejected");
                ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);
            }

        }

    }


    class InputData : ISaveDb
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LogonName { get; set; }
        public string Title { get; set; }

        private Dictionary<string, List<string>> getdictionaryData(List<string> Headers, List<string> SplitData)
        {
            Dictionary<string, List<string>> resut = new Dictionary<string, List<string>>();
            for (int i = 0; i < Headers.Count; i++)
            {
                if (!resut.ContainsKey(Headers[i]))
                {
                    resut.Add(Headers[i], new List<string> { SplitData[i] });

                }
                else
                {
                    resut[Headers[i]].Add(SplitData[i]);
                }
            }
            return resut;
        }


        public void SaveAllUsersReportData()
        {

            int AddressId = 0;
            int User_Id = 0;
            clsSession sess = null;
            sess = (clsSession)HttpContext.Current.Session["UserSession"];

            clsData objData = new clsData();
            clsData.blnTrans = true;
            SqlConnection con = objData.Open();
            SqlTransaction Trans = con.BeginTransaction();
            try
            {
                bool activeExist = objData.IFExists("SELECT ActiveInd FROM [User] WHERE Login=CONVERT(VARBINARY(80),'" + LogonName + "') AND ActiveInd='D' ");
                if (activeExist)
                {
                    string strQuery = "UPDATE [User] SET ActiveInd='A' WHERE  Login=CONVERT(VARBINARY(80),'" + LogonName + "')";
                    int val = objData.Execute(strQuery);
                }
                else
                {
                    string strQuery = " Insert into Address(AddressLine1 ,AddressLine2 ,AddressLine3 ,City ,State ,Country ,Zip ,HomePhone ,Mobile ,Email ,CreatedBy ,CreatedOn ,ModifiedBy ,ModifiedOn) Values(' ','','','','" + 0 + "','" + 0 + "','','','' ,'' ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)) )";
                    AddressId = objData.ExecuteWithScopeandConnection(strQuery, con, Trans);

                    string strQry = "INSERT INTO [User] (AddressId, SchoolId, UserNo, UserInitial, UserFName, UserLName, Gender, Login, Password, ManagerId, EffStartDate, EffEndDate, ActiveInd,ImageURL, Position, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)";
                    strQry += "VALUES (" + AddressId + "," + sess.SchoolId + ",'new','new','" + clsGeneral.convertQuotes(FirstName.Trim()) + "','" + clsGeneral.convertQuotes(LastName.Trim()) + "','null',CONVERT(varbinary(80), '" + clsGeneral.convertQuotes(LogonName) + "'),CONVERT(varbinary(80), '" + clsGeneral.convertQuotes(LogonName) + "')," + 0 + ",null,null,'A',' ',' " + clsGeneral.convertQuotes(Title) + "'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                    User_Id = objData.ExecuteWithScopeandConnection(strQry, con, Trans);

                    strQuery = "Insert into UserRoleGroup(SchoolId,UserRoleGroupDesc,UserId,RoleGroupId,EffStartDate,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
                    strQuery += "values(" + sess.SchoolId + " ,null," + User_Id + "," + val + ",(SELECT Convert(Varchar,getdate(),100)),'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))) ";
                    objData.ExecuteWithTrans(strQuery, con, Trans);
                    objData.CommitTransation(Trans, con);

                }

            }
            catch (Exception e)
            {
                objData.RollBackTransation(Trans, con);
                throw e;
            }

        }



        public void DeleteUsers()
        {
            clsData objData = new clsData();
            if (LogonName != "")
            {
                string strQuery = "UPDATE [User] SET ActiveInd='D' WHERE  Login=CONVERT(VARBINARY(80),'" + LogonName + "')";
                int val = objData.Execute(strQuery);
            }
        }

    }
    interface ISaveDb
    {
        void SaveAllUsersReportData();
        void DeleteUsers();
    }

    private void SaveRolePopup(int UserId, SqlConnection con, SqlTransaction Transs)
    {
        try
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            objRole = new clsRoles();
            if (sess != null)
            {
                foreach (ListItem ri in chkRole.Items)
                {
                    if (ri.Selected == true)
                    {
                        strQuery = "Insert into UserRoleGroup(SchoolId,UserRoleGroupDesc,UserId,RoleGroupId,EffStartDate,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
                        strQuery += "values(" + sess.SchoolId + " ,'" + txtDescription.Text.Trim() + "'," + UserId + "," + ri.Value + ",(SELECT Convert(Varchar,getdate(),100)),'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))) ";
                        objData.ExecuteWithTrans(strQuery, con, Transs);

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
    protected void btnDelUser_Click(object sender, EventArgs e)
    {
        bool flag = false;
        int count = 0;
        if (fileUploadDel.HasFile)
        {
            string ext = System.IO.Path.GetExtension(fileUploadDel.PostedFile.FileName);
            if (ext.ToLower() == ".csv")
            {
                CSVPraser csv = new CSVPraser(',');
                csv.HasFieldsEnclosedInQuotes = true;
                List<string> FileParseError = null;
                Stream file = fileUploadDel.FileContent;
                var ListData = csv.CSVToObject<InputData>(file, ",", out FileParseError);

                if (FileParseError.Count == 0)
                {
                    foreach (var item in ListData)
                    {
                        ISaveDb Inputa = new InputData();
                        item.DeleteUsers();
                        count++;
                    }

                }
                else
                {
                    flag = true;
                    divMessageDel.InnerHtml = clsGeneral.warningMsg(FileParseError[0]);
                    ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPromptsDel();});", true);
                }

            }
            else
            {
                flag = true;
                divMessageDel.InnerHtml = clsGeneral.warningMsg("Please select CSV File");
                ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPromptsDel();});", true);
            }
        }


        else
        {
            flag = true;
            divMessageDel.InnerHtml = clsGeneral.warningMsg("Please select a CSV File");
            ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPromptsDel();});", true);

        }


        if (!flag)
        {
            divMessageDel.InnerHtml = clsGeneral.sucessMsg("" + count + " User Deleted Successfully");
            ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);
            ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPromptsDel();});", true);
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "enableButtonScript", "enableButton();", true);

    }
    protected void drpGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDrpRole();
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts(); removeDiv();});", true);
    }
    protected void drpRol_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();removeDiv();});", true);
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {
            //string FileLocation = Server.MapPath("~\\Administration\\UserTemplate\\UsersNew.csv");
            string PathNew = Server.MapPath("~\\Administration\\UserTemplate\\UsersNew.csv");

            string filename = Path.GetFileName(PathNew);
            if (filename != "")
            {
                FileInfo fileIo = new FileInfo(PathNew);
                if (fileIo.Exists)
                {
                    if (PathNew.ToLower().EndsWith(".csv"))
                    {
                        Response.Clear();
                        Response.ContentType = "application/ms-excel";
                        Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                        Response.AddHeader("Content-Length", fileIo.Length.ToString());
                        Response.TransmitFile(fileIo.FullName);
                        Response.End();
                    }
                    fileIo.Delete();
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnDupDownload_Click(object sender, EventArgs e)
    {
        try
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            string PathNew = Server.MapPath("~\\Administration\\UserTemplate\\DuplicateLoginList_" + sess.LoginId + ".txt");

            string filename = Path.GetFileName(PathNew);
            if (filename != "")
            {
                FileInfo fileIo = new FileInfo(PathNew);
                if (fileIo.Exists)
                {
                    if (PathNew.ToLower().EndsWith(".txt"))
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                        Response.AddHeader("Content-Length", fileIo.Length.ToString());
                        Response.TransmitFile(fileIo.FullName);
                        Response.End();
                    }
                    fileIo.Delete();
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GenerateFile(string msg)
    {
        if (msg != "")
        {
            sess = (clsSession)HttpContext.Current.Session["UserSession"];
            string[] LoginNmae = msg.Split(',');
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            string FilePathData = strPath + @"Administration/UserTemplate/DuplicateLoginList_" + sess.LoginId + ".txt";
            if (System.IO.File.Exists(FilePathData))
            {
                File.Delete(FilePathData);
            }

            File.Create(FilePathData).Close();
            using (StreamWriter w = File.AppendText(FilePathData))
            {
                w.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < LoginNmae.Count(); i++)
                {
                    w.WriteLine(LoginNmae[i].ToString());
                }
                w.Flush();
                w.Close();
            }

        }
    }
    protected void Chkselall_CheckedChanged(object sender, EventArgs e)
    {
        if (Chkselall.Checked == true)
        {
            foreach (DataListItem item in DLclass.Items)
            {
                CheckBox chklist = (CheckBox)item.FindControl("chkClass");
                chklist.Checked = true;
            }
        }
        else
        {
            foreach (DataListItem item in DLclass.Items)
            {
                CheckBox chklist = (CheckBox)item.FindControl("chkClass");
                chklist.Checked = false;
            }
        }
    }
}