using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;


public partial class Admin_AddSchool : System.Web.UI.Page
{


    clsSession sess = null;
    DataClass listClass = null;
    clsData objData = null;
    private static int intAddressId = 0;
    private static int intDisAddressId = 0;
    public int idSchool = 0;
    public static int schoolId;
    private string strQuery = "";
    static bool Disable = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadJquery();", true);
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

        if (!IsPostBack)
        {

            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);

            if (Disable == true)
            {
                Button_Add.Visible = false;
            }
            else
            {
                Button_Add.Visible = true;
            }
            if (Session["EData"] == null)
            {
                bool SchoolCount = clsGeneral.getSchoolCount();

                if (SchoolCount == true)
                {
                    Button_Add.Visible = false;
                }
                else
                {
                    Button_Add.Visible = true;
                }
            }

            FillList();
            if (Session["EData"] != null)
            {
                schoolId = Convert.ToInt32(Session["EData"]);
                EditData();
            }
            else
            {
                schoolId = 0;
            }
            tdMsg.InnerHtml = "";

            if (sess == null)
            {
                Response.Redirect("Login.aspx");
            }


        }
        else
        {

            /*
              string[] listVal = HdFldListItems.Value.Split(',');
             
              if (listVal.Length > 1)
              {
                  lstbxHolidayDate.Items.Clear();
                  for (int LstItmCounter = 1; LstItmCounter < listVal.Length; LstItmCounter++)
                  {
                      lstbxHolidayDate.Items.Add(listVal[LstItmCounter]);
                  }
              }
             */
        }

        // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadJquery();", true);
    }

    private bool Validation()
    {
        if (txtSchoolName.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter School Name");
            txtSchoolName.Focus();
            return false;
        }
        else if (txtSchoolDesc.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter School Description");
            txtSchoolDesc.Focus();
            return false;
        }
        else if (txtAddress1.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Adress1");
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
        else if (txtDisName.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter District Name");
            txtDisName.Focus();
            return false;
        }
        else if (txtDisContactName.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter District Contact Name");
            txtDisContactName.Focus();
            return false;
        }
        else if (txtDisPhoneNum.Text.Trim() == "")
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter District Phone Number");
            txtDisPhoneNum.Focus();
            return false;
        }
        //else if (txtDistAddress1.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter District Adress1");
        //    txtDistAddress1.Focus();
        //    return false;
        //}
        //else if (txtDisAddress2.Text.Trim() == "")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter District Adress2");
        //    txtDisAddress2.Focus();
        //    return false;
        //}
        else if (ddlDisCountry.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select District Country");
            ddlDisCountry.Focus();
            return false;
        }
        else if (ddlDisState.SelectedIndex == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Select District State");
            ddlDisState.Focus();
            return false;
        }



        else if (txtEmail.Text != "" && clsGeneral.IsItEmail(txtEmail.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid E-Mail");
            txtEmail.Focus();
            return false;
        }
        else if (txtZip.Text != "" && clsGeneral.IsItZip(txtZip.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid Zip Code");
            txtZip.Focus();
            return false;
        }
        else if (txtHomePhone.Text.Trim() != "" && clsGeneral.IsItValidPhone(txtHomePhone.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Phone Number Must Be Entered As: (xxx)xxx-xxxx");
            txtHomePhone.Focus();
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
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a valid E-Mail");
            txtEmail.Focus();
            return false;
        }
        else if (txtDistZip.Text != "" && clsGeneral.IsItZip(txtDistZip.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Valid District Zip Code");
            txtDistZip.Focus();
            return false;
        }
        else if (txtDistHomePhone.Text.Trim() != "" && clsGeneral.IsItValidPhone(txtDistHomePhone.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Enter a valid phone number in the form (xxx)xxx-xxxx");
            txtDistHomePhone.Focus();
            return false;
        }
        else if (txtDistMobile.Text != "" && clsGeneral.IsItValidPhone(txtDistMobile.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid Mobile Number in the form (xxx)xxx-xxxx");
            txtDistMobile.Focus();
            return false;
        }
        else if (txtDisPhoneNum.Text.Trim() != "" && clsGeneral.IsItValidPhone(txtDisPhoneNum.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Enter a valid phone number in the form (xxx)xxx-xxxx");
            txtDisPhoneNum.Focus();
            return false;
        }
        else if (txtDistEmail.Text != "" && clsGeneral.IsItEmail(txtDistEmail.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid E-Mail ");
            txtDistEmail.Focus();
            return false;
        }



        return true;
    }

    protected void Button_Add_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadJquery();", true);
        if (Validation() == true)
        {

            if (schoolId == 0)
            {
                try
                {
                    SqlTransaction Transs = null;
                    objData = new clsData();
                    clsData.blnTrans = true;
                    SqlConnection con = objData.Open();
                    Transs = con.BeginTransaction();
                    sess = (clsSession)Session["UserSession"];

                    var stateId = ddlState.SelectedValue;
                    var countryId = ddlCountry.SelectedValue;
                    var disStateId = ddlDisState.SelectedValue;
                    var disCountryid = ddlDisCountry.SelectedValue;


                    strQuery = "Insert into Address(AddressLine1,AddressLine2,AddressLine3,City,State,StateId,Country,CountryId,Zip,HomePhone,Mobile,Email,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values('" + clsGeneral.convertQuotes(txtAddress1.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtAddress2.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtAddress3.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCity.Text.Trim()) + "','" + ddlState.SelectedValue + "', '" + ddlState.SelectedValue + "' ,'" + ddlCountry.SelectedValue + "','" + ddlCountry.SelectedValue + "' ,'" + txtZip.Text.Trim() + "','" + txtHomePhone.Text.Trim() + "','" + txtMobile.Text.Trim() + "' ,'" + txtEmail.Text.Trim() + "' ," + sess.LoginId + " ,(SELECT Convert(Varchar,getdate(),100)) ," + sess.LoginId + " ,(SELECT Convert(Varchar,getdate(),100)));\nSELECT SCOPE_IDENTITY();";
                    intAddressId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, Transs));



                    strQuery = " Insert into Address(AddressLine1  ,AddressLine2,AddressLine3,City,State,StateId,Country,CountryId,Zip,HomePhone,Mobile,Email,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values('" + clsGeneral.convertQuotes(txtDistAddress1.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDisAddress2.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDisAddress3.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDisCity.Text.Trim()) + "','" + ddlDisState.SelectedValue + "','" + ddlDisState.SelectedValue + "' ,'" + ddlDisCountry.SelectedValue + "','" + ddlDisCountry.SelectedValue + "' ,'" + txtDistZip.Text.Trim() + "','" + txtDistHomePhone.Text.Trim() + "','" + txtDistMobile.Text.Trim() + "' ,'" + txtDistEmail.Text.Trim() + "' ," + sess.LoginId + " ,(SELECT Convert(Varchar,getdate(),100)) ," + sess.LoginId + " ,(SELECT Convert(Varchar,getdate(),100)));\nSELECT SCOPE_IDENTITY();";
                    intDisAddressId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, Transs));


                    strQuery = "Insert into School(SchoolName,SchoolDesc,DistrictName,DistContact,DistPhone,AddressId,DistAddrId,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values('" + clsGeneral.convertQuotes(txtSchoolName.Text) + "','" + clsGeneral.convertQuotes(txtSchoolDesc.Text) + "','" + clsGeneral.convertQuotes(txtDisName.Text) + "','" + clsGeneral.convertQuotes(txtDisContactName.Text) + "','" + txtDisPhoneNum.Text + "'," + intAddressId + "," + intDisAddressId + ",'A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                    schoolId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(strQuery, con, Transs));

                    if (schoolId > 0) AddClass(schoolId);




                    objData.CommitTransation(Transs, con);

                    //Addin School Cal And Timing
                    if (schoolId > 0)
                    {
                        AddScoolCalNdHoliday(schoolId);
                    }
                    /////////////////////////////

                    tdMsg.InnerHtml = clsGeneral.sucessMsg("School Details Saved Successfully.");
                    ClearData();

                }
                catch (SqlException Ex)
                {
                    objData.RollBackTransation();
                    tdMsg.InnerHtml = clsGeneral.failedMsg(" School Details Insertion Failed.");
                    HdFldListItems.Value = "";
                    throw Ex;
                }

            }
            else
            {
                try
                {

                    UpdateSchool();
                    // schoolId = 0;
                    tdMsg.InnerHtml = clsGeneral.sucessMsg(" School Details Updated Successfully.");

                }
                catch (SqlException Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("School Details Updated Failed..");
                    throw Ex;
                }

            }
        }
        else
        {
            HdFldListItems.Value = "";
        }

    }

    private string amPmTo24hourConverter(string time)
    {
        string[] Alltime = time.Split(':');
        int h = Convert.ToInt16(Alltime[0]);
        int m = Convert.ToInt16(Alltime[1]);
        int s = Convert.ToInt16(Alltime[2].Substring(0, 2));
        string startAMPM = Alltime[2].Substring(2, 2);
        if (startAMPM == "PM")
        {
            h += 12;
        }
        string timetest = h.ToString() + ":" + m.ToString() + ":" + s.ToString();
        return (timetest);
    }


    private void AddScoolCalNdHoliday(int schoolId)
    {
        clsData obj = new clsData();
        if (chkDayMon.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Monday','" + txtDayMondayStart.Text + "',1,'" + txtDayMondayEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Monday','00:00:00',1,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }

        if (chkDayTue.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Tuesday','" + txtDayTueStart.Text + "',1,'" + txtDayTueEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Tuesday','00:00:00',1,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkDayWed.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Wednesday','" + txtDayWedStart.Text + "',1,'" + txtDayWedEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Wednesday','00:00:00',1,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkDayThu.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Thursday','" + txtDayThuStart.Text + "',1,'" + txtDayThuEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Thursday','00:00:00',1,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkDayFri.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Friday','" + txtDayFriStart.Text + "',1,'" + txtDayFriEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Friday','00:00:00',1,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkDaySat.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Saturday','" + txtDaySatStart.Text + "',1,'" + txtDaySatEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Saturday','00:00:00',1,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkDaySun.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Sunday','" + txtDaySunStart.Text + "',1,'" + txtDaySunEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Sunday','00:00:00',1,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        ///////////////////////////////////////
        if (chkResMon.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Monday','" + txtResMondayStart.Text + "',0,'" + txtResMondayEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Monday','00:00:00',0,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }

        if (chkResTue.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Tuesday','" + txtResTueStart.Text + "',0,'" + txtResTueEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Tuesday','00:00:00',0,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }

        if (chkResWed.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Wednesday','" + txtResWedStart.Text + "',0,'" + txtResWedEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Wednesday','00:00:00',0,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkResThu.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Thursday','" + txtResThuEnd.Text + "',0,'" + txtResThuEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Thursday','00:00:00',0,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkResFri.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Friday','" + txtResFriStart.Text + "',0,'" + txtResFriEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Friday','00:00:00',0,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkResSat.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Saturday','" + txtResSatStart.Text + "',0,'" + txtResSatEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Saturday','00:00:00',0,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        if (chkResSun.Checked == true)
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Sunday','" + txtResSunStart.Text + "',0,'" + txtResSunEnd.Text + "','True'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }
        else
        {
            strQuery = "insert into SchoolCal(SchoolId,Weekday,StartTime,ResidenceInd,EndTime,DayFlag,CreatedBy,CreatedOn) values('" + schoolId + "','Sunday','00:00:00',0,'00:00:00','False'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
            obj.Execute(strQuery);
        }


        //////
        string[] listVal = HdFldListItems.Value.Split(',');

        if (listVal.Length > 1)
        {
            for (int LstItmCounter = 1; LstItmCounter < listVal.Length; LstItmCounter++)
            {
                string[] dateNdName = listVal[LstItmCounter].Split('-');
                string[] dateOnly = dateNdName[1].Split('/');
                strQuery = "insert into SchoolHoliday(SchoolId,HolName,HolDate,CreatedBy,CreatedOn) values(" + schoolId + ",'" + dateNdName[0].ToString() + "','" + dateOnly[0].ToString() + "/" + dateOnly[1].ToString() + "/" + dateOnly[2].ToString() + "'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                obj.Execute(strQuery);
            }
            HdFldListItems.Value = "";
            if (lstbxHolidayDate.Items.Count != 0)
            {
                lstbxHolidayDate.Items.Clear();
            }
        }
    }



    private void UpdateScoolCalNdHoliday(int SChoolId, SqlTransaction trans, SqlConnection con)
    {
        objData = new clsData();
        if ((chkDayMon.Checked == true) && (txtDayMondayStart.Text.Length != 0) && (txtDayMondayEnd.Text.Length != 0))
        {
            string daStrt = amPmTo24hourConverter(txtDayMondayStart.Text);
            string daEnd = amPmTo24hourConverter(txtDayMondayEnd.Text).ToString();
            int id = int.Parse(HdnDayMon.Value);
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtDayMondayStart.Text.ToString() + "'),EndTime=CONVERT(time(7),'" + txtDayMondayEnd.Text.ToString() + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnDayMon.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnDayMon.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }

        if ((chkDayTue.Checked == true) && (txtDayMondayStart.Text.Length != 0) && (txtDayMondayEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtDayTueStart.Text + "'),EndTime=CONVERT(time(7),'" + txtDayTueEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnDayTue.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnDayTue.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkDayWed.Checked == true) && (txtDayWedStart.Text.Length != 0) && (txtDayWedEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtDayWedStart.Text + "'),EndTime=CONVERT(time(7),'" + txtDayWedEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnDayWed.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnDayWed.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }

        if ((chkDayThu.Checked == true) && (txtDayThuStart.Text.Length != 0) && (txtDayThuEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtDayThuStart.Text + "'),EndTime=CONVERT(time(7),'" + txtDayThuEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnDayThu.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnDayThu.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkDayFri.Checked == true) && (txtDayFriStart.Text.Length != 0) && (txtDayFriEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtDayFriStart.Text + "'),EndTime=CONVERT(time(7),'" + txtDayFriEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnDayFri.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnDayFri.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkDaySat.Checked == true) && (txtDaySatStart.Text.Length != 0) && (txtDaySatEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtDaySatStart.Text + "'),EndTime=CONVERT(time(7),'" + txtDaySatEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnDaySat.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnDaySat.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkDaySun.Checked == true) && (txtDaySunStart.Text.Length != 0) && (txtDaySunEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtDaySunStart.Text + "'),EndTime=CONVERT(time(7),'" + txtDaySunEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnDaySun.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnDaySun.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }

        ///////////////////////////////////////
        if ((chkResMon.Checked == true) && (txtResMondayStart.Text.Length != 0) && (txtResMondayEnd.Text.Length != 0))
        {
            strQuery = "update SchoolCal set StartTime=CONVERT(time(7),'" + txtResMondayStart.Text + "'),EndTime=CONVERT(time(7),'" + txtResMondayEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnResMon.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnResMon.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkResTue.Checked == true) && (txtResTueStart.Text.Length != 0) && (txtResTueEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtResTueStart.Text + "'),EndTime=CONVERT(time(7),'" + txtResTueEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnResTue.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnResTue.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkResWed.Checked == true) && (txtResWedStart.Text.Length != 0) && (txtResWedEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtResWedStart.Text + "'),EndTime=CONVERT(time(7),'" + txtResWedEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnResWed.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnResWed.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkResThu.Checked == true) && (txtResThuStart.Text.Length != 0) && (txtResThuEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtResThuStart.Text + "'),EndTime=CONVERT(time(7),'" + txtResThuEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnResThu.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnResThu.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }

        if ((chkResFri.Checked == true) && (txtResFriStart.Text.Length != 0) && (txtResFriEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtResFriStart.Text + "'),EndTime=CONVERT(time(7),'" + txtResFriEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnResFri.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnResFri.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }


        if ((chkResSat.Checked == true) && (txtResSatStart.Text.Length != 0) && (txtResSatEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtResSatStart.Text + "'),EndTime=CONVERT(time(7),'" + txtResSatEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnResSat.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnResSat.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }

        if ((chkResSun.Checked == true) && (txtResSunStart.Text.Length != 0) && (txtResSunEnd.Text.Length != 0))
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'" + txtResSunStart.Text + "'),EndTime=CONVERT(time(7),'" + txtResSunEnd.Text + "'),DayFlag='True' where SchoolCalId=" + int.Parse(HdnResSun.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        else
        {
            strQuery = " update SchoolCal set StartTime=CONVERT(time(7),'00:00:00'),EndTime=CONVERT(time(7),'00:00:00'),DayFlag='False' where SchoolCalId=" + int.Parse(HdnResSun.Value) + "";
            objData.ExecuteWithTrans(strQuery, con, trans);
        }
        //////

        string[] listVal = HdFldListItems.Value.Split(',');

        strQuery = "delete from SchoolHoliday where SchoolId=" + schoolId + "";
        objData.ExecuteWithTrans(strQuery, con, trans);

        if (listVal.Length > 1)
        {
            for (int LstItmCounter = 1; LstItmCounter < listVal.Length; LstItmCounter++)
            {
                string[] dateNdName = listVal[LstItmCounter].Split('-');
                string[] dateOnly = dateNdName[1].Split('/');
                strQuery = "insert into SchoolHoliday(SchoolId,HolName,HolDate,CreatedBy,CreatedOn) values(" + schoolId + ",'" + dateNdName[0].ToString() + "','" + dateOnly[0].ToString() + "/" + dateOnly[1].ToString() + "/" + dateOnly[2].ToString() + "'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)))";
                objData.ExecuteWithTrans(strQuery, con, trans);
            }
            HdFldListItems.Value = "";
            if (lstbxHolidayDate.Items.Count != 0)
            {
                lstbxHolidayDate.Items.Clear();
            }
        }
        //////



    }

    public void EditData()
    {
        if (schoolId > 0)
        {

            try
            {
                listClass = new DataClass();
                sess = (clsSession)Session["UserSession"];
                //sess.SchoolId = schoolId;
                Session["USchool"] = "Yes";
                listClass = new DataClass();
                objData = new clsData();
                Button_Add.Text = "UPDATE";
                string selectStudent = " select sl.SchoolName,sl.SchoolDesc,sl.DistrictName,sl.DistContact,sl.DistPhone,ad.AddressLine1,ad.AddressLine2,ad.AddressLine3,ad.City,ad.State,ad.Country, " +
                                        " ad.HomePhone,ad.Mobile,ad.Email,ad.Zip from School sl join Address ad on sl.AddressId = ad.AddressId where sl.SchoolId = " + schoolId + "";
                DataTable dataStudent = listClass.fillData(selectStudent);
                if (dataStudent.Rows.Count > 0)
                {
                    txtSchoolName.Text = dataStudent.Rows[0]["SchoolName"].ToString();
                    txtSchoolDesc.Text = dataStudent.Rows[0]["SchoolDesc"].ToString();
                    txtAddress1.Text = dataStudent.Rows[0]["AddressLine1"].ToString();
                    txtAddress2.Text = dataStudent.Rows[0]["AddressLine2"].ToString();
                    txtAddress3.Text = dataStudent.Rows[0]["AddressLine3"].ToString();
                    ddlCountry.SelectedValue = dataStudent.Rows[0]["Country"].ToString();
                    txtCity.Text = dataStudent.Rows[0]["City"].ToString();
                    txtZip.Text = dataStudent.Rows[0]["Zip"].ToString();
                    txtMobile.Text = dataStudent.Rows[0]["Mobile"].ToString();
                    txtEmail.Text = dataStudent.Rows[0]["Email"].ToString();
                    txtHomePhone.Text = dataStudent.Rows[0]["HomePhone"].ToString();
                    txtDisName.Text = dataStudent.Rows[0]["DistrictName"].ToString();
                    txtDisContactName.Text = dataStudent.Rows[0]["DistContact"].ToString();
                    txtDisPhoneNum.Text = dataStudent.Rows[0]["DistPhone"].ToString();
                }

                try
                {
                    objData.ReturnDropDown("SELECT LookupId As Id,LookupName As Name from LookUp WHERE LookupType = 'State' AND ParentLookupId = " + ddlCountry.SelectedValue + "", ddlState);
                    ddlState.SelectedValue = dataStudent.Rows[0]["State"].ToString().Trim();
                }
                catch (Exception Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Error!!!");
                    throw Ex;
                }



                string distAddress = " select ad.AddressLine1,ad.AddressLine2,ad.AddressLine3,ad.City,ad.Country,ad.Email,ad.HomePhone,ad.Mobile,ad.State,ad.Zip " +
                                            "  from School sc join Address ad ON sc.DistAddrId = ad.AddressId where sc.SchoolId = " + schoolId + "";

                DataTable distData = listClass.fillData(distAddress);
                if (distData.Rows.Count > 0)
                {
                    txtDistAddress1.Text = distData.Rows[0]["AddressLine1"].ToString();
                    txtDisAddress2.Text = distData.Rows[0]["AddressLine2"].ToString();
                    txtDisAddress3.Text = distData.Rows[0]["AddressLine3"].ToString();
                    ddlDisCountry.SelectedValue = distData.Rows[0]["Country"].ToString();
                    txtDisCity.Text = distData.Rows[0]["City"].ToString();
                    txtDistHomePhone.Text = distData.Rows[0]["HomePhone"].ToString();
                    txtDistEmail.Text = distData.Rows[0]["Email"].ToString();
                    txtDistMobile.Text = distData.Rows[0]["Mobile"].ToString();
                    txtDistZip.Text = distData.Rows[0]["Zip"].ToString();
                }
                try
                {
                    objData.ReturnDropDown("SELECT LookupId As Id,LookupName As Name from LookUp WHERE LookupType = 'State' AND ParentLookupId = " + ddlDisCountry.SelectedValue + "", ddlDisState);
                    ddlDisState.SelectedValue = distData.Rows[0]["State"].ToString().Trim();
                }
                catch (Exception Ex)
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Error!!!");
                    throw Ex;
                }


                DataTable dataScolDetails = new DataTable();
                string QryScoolTime = "select SchoolCalId,Weekday,StartTime,EndTime,DayFlag from SchoolCal where SchoolId=" + schoolId + " and ResidenceInd=0";
                dataScolDetails = objData.ReturnDataTable(QryScoolTime, true);
                //HdnResMon
                if (dataScolDetails.Rows.Count != 0)
                {
                    foreach (DataRow dr in dataScolDetails.Rows)
                    {
                        if (dr[1].ToString() == "Monday")
                        {

                            HdnResMon.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtResMondayStart.Text = dr[2].ToString();
                                txtResMondayEnd.Text = dr[3].ToString();
                                chkResMon.Checked = true;
                                txtResMondayStart.Enabled = true;
                                txtResMondayEnd.Enabled = true;
                            }

                        }
                        if (dr[1].ToString() == "Tuesday")
                        {
                            HdnResTue.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtResTueStart.Text = dr[2].ToString();
                                txtResTueEnd.Text = dr[3].ToString();
                                chkResTue.Checked = true;
                                txtResTueStart.Enabled = true;
                                txtResTueEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Wednesday")
                        {
                            HdnResWed.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtResWedStart.Text = dr[2].ToString();
                                txtResWedEnd.Text = dr[3].ToString();
                                chkResWed.Checked = true;
                                txtResWedStart.Enabled = true;
                                txtResWedEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Thursday")
                        {
                            HdnResThu.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtResThuStart.Text = dr[2].ToString();
                                txtResThuEnd.Text = dr[3].ToString();
                                chkResThu.Checked = true;
                                txtResThuStart.Enabled = true;
                                txtResThuEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Friday")
                        {
                            HdnResFri.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtResFriStart.Text = dr[2].ToString();
                                txtResFriEnd.Text = dr[3].ToString();
                                chkResFri.Checked = true;
                                txtResFriStart.Enabled = true;
                                txtResFriEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Saturday")
                        {
                            HdnResSat.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtResSatStart.Text = dr[2].ToString();
                                txtResSatEnd.Text = dr[3].ToString();
                                chkResSat.Checked = true;
                                txtResSatStart.Enabled = true;
                                txtResSatEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Sunday")
                        {
                            HdnResSun.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtResSunStart.Text = dr[2].ToString();
                                txtResSunEnd.Text = dr[3].ToString();
                                chkResSun.Checked = true;
                                txtResSunStart.Enabled = true;
                                txtResSunEnd.Enabled = true;
                            }
                        }
                    }
                }

                QryScoolTime = "select SchoolCalId,Weekday,StartTime,EndTime,DayFlag from SchoolCal where SchoolId=" + schoolId + " and ResidenceInd=1";
                dataScolDetails = objData.ReturnDataTable(QryScoolTime, true);
                if (dataScolDetails.Rows.Count != 0)
                {
                    foreach (DataRow dr in dataScolDetails.Rows)
                    {
                        if (dr[1].ToString() == "Monday")
                        {
                            HdnDayMon.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtDayMondayStart.Text = dr[2].ToString();
                                txtDayMondayEnd.Text = dr[3].ToString();
                                chkDayMon.Checked = true;
                                txtDayMondayStart.Enabled = true;
                                txtDayMondayEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Tuesday")
                        {
                            HdnDayTue.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtDayTueStart.Text = dr[2].ToString();
                                txtDayTueEnd.Text = dr[3].ToString();
                                chkDayTue.Checked = true;
                                txtDayTueStart.Enabled = true;
                                txtDayTueEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Wednesday")
                        {
                            HdnDayWed.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtDayWedStart.Text = dr[2].ToString();
                                txtDayWedEnd.Text = dr[3].ToString();
                                chkDayWed.Checked = true;
                                txtDayWedStart.Enabled = true;
                                txtDayWedEnd.Enabled = true;
                            }

                        }
                        if (dr[1].ToString() == "Thursday")
                        {
                            HdnDayThu.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtDayThuStart.Text = dr[2].ToString();
                                txtDayThuEnd.Text = dr[3].ToString();
                                chkDayThu.Checked = true;
                                txtDayThuStart.Enabled = true;
                                txtDayThuEnd.Enabled = true;
                            }

                        }
                        if (dr[1].ToString() == "Friday")
                        {
                            HdnDayFri.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtDayFriStart.Text = dr[2].ToString();
                                txtDayFriEnd.Text = dr[3].ToString();
                                chkDayFri.Checked = true;
                                txtDayFriStart.Enabled = true;
                                txtDayFriEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Saturday")
                        {
                            HdnDaySat.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtDaySatStart.Text = dr[2].ToString();
                                txtDaySatEnd.Text = dr[3].ToString();
                                chkDaySat.Checked = true;
                                txtDaySatStart.Enabled = true;
                                txtDaySatEnd.Enabled = true;
                            }
                        }
                        if (dr[1].ToString() == "Sunday")
                        {
                            HdnDaySun.Value = dr[0].ToString();
                            if (dr[4].ToString() == "True")
                            {
                                txtDaySunStart.Text = dr[2].ToString();
                                txtDaySunEnd.Text = dr[3].ToString();
                                chkDaySun.Checked = true;
                                txtDaySunStart.Enabled = true;
                                txtDaySunEnd.Enabled = true;
                            }
                        }
                    }
                }



                QryScoolTime = "select SchoolHolId,HolName,HolDate from SchoolHoliday where SchoolId=" + schoolId + "";
                dataScolDetails = objData.ReturnDataTable(QryScoolTime, true);
                if (dataScolDetails.Rows.Count != 0)
                {

                    foreach (DataRow dr in dataScolDetails.Rows)
                    {

                        ListItem item = new ListItem();
                        item.Text = dr[1].ToString() + "-" + DateTime.Parse(dr[2].ToString()).ToString("MM'/'dd'/'yyyy");
                        item.Value = dr[1].ToString();
                        lstbxHolidayDate.Items.Add(item);
                    }
                }

                /*
                    select SchoolCalId,StartTime,EndTime from SchoolCal where SchoolId=1 and ResidenceInd=1
                    select SchoolCalId,StartTime,EndTime from SchoolCal where SchoolId=1 and ResidenceInd=0
                    select SchoolHolId,HolDate,HolName from SchoolHoliday where SchoolId=1
                 */


                Session["EData"] = null;
            }

            catch (Exception Ex)
            {
                throw Ex;
                //  tdMsg.InnerHtml = clsGeneral.warningMsg(Ex.Message + "Input Data Entry Error!!!!!");
            }

        }

    }

    public void UpdateSchool()
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        SqlTransaction Transs = null;
        clsData.blnTrans = true;
        SqlConnection conn = objData.Open();
        Transs = conn.BeginTransaction();
        try
        {

            string strQuery = "UPDATE School SET SchoolName = '" + clsGeneral.convertQuotes(txtSchoolName.Text) + "',SchoolDesc = '" + clsGeneral.convertQuotes(txtSchoolDesc.Text) + "',DistrictName = '" + clsGeneral.convertQuotes(txtDisName.Text) + "',DistContact = '" + clsGeneral.convertQuotes(txtDisContactName.Text) + "',DistPhone = '" + txtDisPhoneNum.Text + "',ModifiedBy = " + sess.LoginId + ",ModifiedOn = (SELECT Convert(Varchar,getdate(),100))  where SchoolId = " + schoolId + "";
            Boolean iStudent = false;
            try
            {
                iStudent = Convert.ToBoolean(objData.ExecuteWithTrans(strQuery, conn, Transs));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            strQuery = "SELECT AddressId from School where SchoolId = " + schoolId + "";
            int AddressId = 0;
            try
            {
                AddressId = Convert.ToInt32(objData.FetchValueTrans(strQuery, Transs, conn));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            strQuery = "UPDATE Address SET AddressLine1 = '" + clsGeneral.convertQuotes(txtAddress1.Text) + "',AddressLine2 = '" + clsGeneral.convertQuotes(txtAddress2.Text) + "',AddressLine3 = '" + clsGeneral.convertQuotes(txtAddress3.Text) + "',City = '" + clsGeneral.convertQuotes(txtCity.Text.Trim()) + "',State = '" + ddlState.SelectedValue + "', StateId='" + ddlState.SelectedValue + "' ,Country = '" + ddlCountry.SelectedValue + "',CountryId='" + ddlCountry.SelectedValue + "',Zip = '" + txtZip.Text + "',Mobile = '" + txtMobile.Text + "',Email = '" + txtEmail.Text + "',HomePhone = '" + txtHomePhone.Text + "' where AddressId = " + AddressId + "";
            Boolean iAddress = false;
            try
            {
                iAddress = Convert.ToBoolean(objData.ExecuteWithTrans(strQuery, conn, Transs));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            strQuery = "SELECT DistAddrId from School where SchoolId = " + schoolId + "";

            int DAddressId = 0;
            try
            {
                DAddressId = Convert.ToInt32(objData.FetchValueTrans(strQuery, Transs, conn));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }



            strQuery = "UPDATE Address SET AddressLine1 = '" + clsGeneral.convertQuotes(txtDistAddress1.Text) + "',AddressLine2 = '" + clsGeneral.convertQuotes(txtDisAddress2.Text) + "',AddressLine3 = '" + clsGeneral.convertQuotes(txtDisAddress3.Text) + "',City = '" + clsGeneral.convertQuotes(txtDisCity.Text.Trim()) + "',State = '" + ddlDisState.SelectedValue + "', StateId='" + ddlState.SelectedValue + "',Country = '" + ddlCountry.SelectedValue + "',CountryId='" + ddlCountry.SelectedValue + "',Zip = '" + txtDistZip.Text + "',Mobile = '" + txtDistMobile.Text + "',Email = '" + txtDistEmail.Text + "',HomePhone = '" + txtDistHomePhone.Text + "' where AddressId = " + DAddressId + "";
            Boolean iDistAddr = Convert.ToBoolean(objData.ExecuteWithTrans(strQuery, conn, Transs));            
            UpdateScoolCalNdHoliday(schoolId, Transs, conn);
            objData.CommitTransation(Transs, conn);

            tdMsg.InnerHtml = clsGeneral.sucessMsg("School Updated Successfully");
        }
        catch (Exception Ex)
        {
            objData.RollBackTransation();

            tdMsg.InnerHtml = clsGeneral.failedMsg("Sorry Updation Failed...Please Try again!");
            throw Ex;
        }


    }

    public void ClearData()
    {
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtAddress3.Text = "";
        txtCity.Text = "";
        txtDisCity.Text = "";
        ddlCountry.SelectedIndex = 0;
        ddlDisCountry.SelectedIndex = 0;
        ddlDisState.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        txtEmail.Text = "";
        txtHomePhone.Text = "";
        txtMobile.Text = "";
        txtSchoolDesc.Text = "";
        txtSchoolName.Text = "";
        txtZip.Text = "";
        txtDistAddress1.Text = "";
        txtDisAddress2.Text = "";
        txtDisAddress3.Text = "";
        txtDisContactName.Text = "";
        txtDisName.Text = "";
        txtDisPhoneNum.Text = "";
        txtDistEmail.Text = "";
        txtDistHomePhone.Text = "";
        txtDistMobile.Text = "";
        txtDistZip.Text = "";
        Session["data"] = null;
        Session["USchool"] = null;
        Session["EData"] = null;


        chkDayFri.Checked = false;

        chkDayFri.Checked = false;
        chkDaySat.Checked = false;
        chkDaySun.Checked = false;
        chkDayMon.Checked = false;
        chkDayTue.Checked = false;
        chkDayWed.Checked = false;
        chkDayThu.Checked = false;

        chkResFri.Checked = false;
        chkResSat.Checked = false;
        chkResSun.Checked = false;
        chkResMon.Checked = false;
        chkResTue.Checked = false;
        chkResWed.Checked = false;
        chkResThu.Checked = false;



    }

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListSchool.aspx");
    }
    protected void txtMobile_TextChanged(object sender, EventArgs e)
    {

    }
    public void AddClass(int Id)
    {
        listClass = new DataClass();
        sess = (clsSession)Session["UserSession"];
        if (Id != 0)
        {
            DataTable Dt = (DataTable)Session["data"];
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        string insertClass = "INSERT into Class (SchoolId,ClassName,ClassDesc,CreatedBy,CreateOn,ModifiedBy,ModifiedOn) Values(" + Id + ",'" + Dr["ClassName"].ToString() + "','" + Dr["ClassDes"].ToString() + "'," + sess.LoginId + "," + DateTime.Today.ToShortDateString() + "," + sess.LoginId + "," + DateTime.Today.ToShortDateString() + ");\nSELECT SCOPE_IDENTITY();";
                        int intClassId = Convert.ToInt32(listClass.ExecuteScalar(insertClass));
                    }
                    Session["data"] = null;
                    Dt = null;
                }
                else
                {
                    return;
                }
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadJquery();", true);
        try
        {
            if (ddlCountry.SelectedIndex != 0)
            {

                objData = new clsData();
                ddlState.Items.Clear();
                int countryId = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
                objData.ReturnDropDown("SELECT LookupId As Id,LookupName As Name from LookUp WHERE LookupType = 'State' AND ParentLookupId = " + countryId + "", ddlState);
            }
            else
            {
                ddlState.Items.Clear();
                ddlState.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    public void FillList()
    {
        objData = new clsData();
        objData.ReturnDropDown("SELECT LookupId as Id,LookupName as Name  from LookUp where LookupType = 'Country'", ddlCountry);
        objData.ReturnDropDown("SELECT LookupId as Id,LookupName as Name  from LookUp where LookupType = 'Country'", ddlDisCountry);
    }

    protected void ddlDisCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "loadJquery();", true);
        try
        {
            objData = new clsData();
            ddlDisState.Items.Clear();

            int countryId = Convert.ToInt32(ddlDisCountry.SelectedValue.ToString());
            objData.ReturnDropDown("SELECT LookupId As Id,LookupName As Name from LookUp WHERE LookupType = 'State' AND ParentLookupId = " + countryId + "", ddlDisState);
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        clsData objData = new clsData();

        List<string> name = new List<string>();

        String query = "select StudentId as Id,StudentLname+ ','+StudentFname as Name from Student WHERE ActiveInd = 'A'";
        SqlDataReader drStudent = objData.ReturnDataReader(query, false);
        while (drStudent.Read())
        {
            name.Add(drStudent["Name"].ToString() + "-" + drStudent["Id"].ToString());
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



    protected void txtSearchStudent_TextChanged(object sender, EventArgs e)
    {

    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ListSchool.aspx");
    }
}