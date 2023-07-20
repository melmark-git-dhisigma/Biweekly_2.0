using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Admin_AddStudent : System.Web.UI.Page
{
    private static int intAddressId = 0;
    public static int intStudentId = 0;
    public static int intclassid = 0;
    static Boolean Result;
    // private static string strNumber = "";


    public static clsSession sess = null;
    clsData objData = null;
    DataClass objdataClass = null;
    public static int temp;
    static bool Disable = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            HiddenField1.Value = sess.UserName;
            bool flag = clsGeneral.PageIdentification(sess.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }

        if (!IsPostBack)
        {
            txtStudentId.Focus();
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                Button_Add.Visible = false;

            }
            else
            {
                Button_Add.Visible = true;
                lblmstatus.Visible = false;
                lblstaus.Visible = false;
                ddlStatus.Visible = false;
            }


            FillList();

            //------------------------------------------------------------------------------------------------------------------------------------------//
            // if Session["StudentId"] = "-1" => add new student; if Session["StudentId"] != "-1" => update the student details with current studentid

            if (sess.AdmStudentId == 0)
            {
                intStudentId = 0;
                Button_Add.Text = "Save";
            }
            else
            {
                intStudentId = Convert.ToInt32(sess.AdmStudentId);
                fillStudent();
                Button_Add.Text = "Update";
            }
            //------------------------------------------------------------------------------------------------------------------------------------------//

            if (sess == null)
            {
                Response.Redirect("Login.aspx");
            }


        }



    }
    private string PhotoPreviewandSave()
    {
        string filename = "defaultStudent.png";
        if (fileUpl_stdPhoto.HasFile)
        {
            try
            {
                sess = (clsSession)Session["UserSession"];
                string Ext = fileUpl_stdPhoto.PostedFile.ContentType;
                if (Ext == "image/jpeg" || Ext == "image/jpg" || Ext == "image/png" || Ext == "image/x-png" || Ext == "image/pjpeg")
                {
                    if (fileUpl_stdPhoto.PostedFile.ContentLength < 10240000)
                    {
                        filename = Path.GetFileName(fileUpl_stdPhoto.FileName);
                        string extension = Path.GetExtension(fileUpl_stdPhoto.PostedFile.FileName);
                        string SPath = Server.MapPath("~/Administration/StudentsPhoto/");
                        filename = filename.Replace(extension, "") + sess.SessionID + extension;
                        fileUpl_stdPhoto.SaveAs(SPath + filename);
                        tdMsg.InnerHtml = clsGeneral.sucessMsg("");
                    }
                    else
                        tdMsg.InnerHtml = clsGeneral.warningMsg("Upload status: The file has to be less than 100 kb!");
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Upload status: Only JPEG,JPG,PNG files are accepted!");
                }
            }
            catch (Exception ex)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Upload status: The file could not be uploaded. The following error occured: ");
                throw ex;
            }

        }


        return filename;
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
            tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!! Please try Again!!!!");
            throw Ex;
        }
    }

    protected void fillStudent()
    {
        if (intStudentId > 0)
        {
            if (Convert.ToString(Session["Status"]) == "Inactive")
            {
                lblmstatus.Visible = true;
                lblstaus.Visible = true;
                ddlStatus.Visible = true;
                ddlStatus.SelectedIndex = 2;
            }
            clsData objData = new clsData();
            objdataClass = new DataClass();
            string strQuery = "SELECT   Std.ImageUrl,Std.StudentId, Std.ClassId, Std.StudentNbr, Std.StudentFname, Std.StudentLname,Std.GradeLevel,Std.ResidenceInd, Std.Gender,Std.JoinDt, Std.DOB,  " +
                          "Std.CreatedBy, Std.CreatedOn, Adr.AddressLine1, Adr.AddressLine2, Adr.AddressLine3, Adr.City, Adr.State,Std.SASID,Std.DistrictCode,Std.DistrictType,Std.DistFunction,Std.DistContactPerson," +
                          "Adr.Country, Adr.Zip, Adr.HomePhone, Adr.Mobile, Adr.Email, Adr.CreatedBy AS Expr1, Adr.CreatedOn AS Expr2 ,  " +
                            " Adr.ModifiedBy, Adr.ModifiedOn  ,Adr.AddressId as AddId " +

                            ",Std.Race,Std.Height,Std.Weight,Std.EyeColor,Std.HairColor,Std.DistinguishMark,Std.CaseManagerResidential,Std.CaseManagerEducational,Std.LastPhysicalDate,Std.MedicalConditions,Std.Allergies,Std.CurrentMedications,Std.SelfPresAbility, Std.SignificantBehavior,Std.Capabilities,Std.Limitations,Std.Performances,Std.LegalComp,Std.MaritulStatus,Std.OtherStageAgency,Std.Citizenship,Std.GuardenshipStatus,Std.PrimaryLang" +
                                      " FROM         Student Std INNER JOIN " +
                                           "Address Adr ON Std.AddressId = Adr.AddressId " +
                                                      " where Std.StudentId=" + intStudentId + "";

            DataTable Dt = objdataClass.fillData(strQuery);

            try
            {
                if (Dt.Rows.Count > 0)
                {
                    Button_Add.Text = "UPDATE";
                    string Img = "StudentsPhoto/" + Dt.Rows[0]["ImageUrl"].ToString();
                    sess.AdmStudentId = Convert.ToInt16(Dt.Rows[0]["StudentId"]);
                    intStudentId = Convert.ToInt16(Dt.Rows[0]["StudentId"]);
                    txtStudentId.Text = Dt.Rows[0]["StudentNbr"].ToString().Trim();
                    txtFirstName.Text = Dt.Rows[0]["StudentFname"].ToString().Trim();
                    txtLastName.Text = Dt.Rows[0]["StudentLname"].ToString().Trim();
                    txtSASID.Text = Dt.Rows[0]["SASID"].ToString().Trim();
                    ddlGender.SelectedValue = Dt.Rows[0]["Gender"].ToString();
                    txtJoinDt.Text = Convert.ToDateTime(Dt.Rows[0]["JoinDt"]).ToString("MM/dd/yyyy").Replace("/", "-");
                    txtDOB.Text = Convert.ToDateTime(Dt.Rows[0]["DOB"]).ToString("MM/dd/yyyy").Replace("/", "-");
                    txtAddress1.Text = Dt.Rows[0]["AddressLine1"].ToString().Trim();
                    txtAddress2.Text = Dt.Rows[0]["AddressLine2"].ToString().Trim();
                    txtAddress3.Text = Dt.Rows[0]["AddressLine3"].ToString().Trim();
                    ddlCountry.SelectedValue = Dt.Rows[0]["Country"].ToString().Trim();


                    txtRace.Text = Dt.Rows[0]["Race"].ToString().Trim();
                    txtHeight.Text = Dt.Rows[0]["Height"].ToString().Trim();
                    txtWeight.Text = Dt.Rows[0]["Weight"].ToString().Trim();
                    txtEyeColor.Text = Dt.Rows[0]["EyeColor"].ToString().Trim();
                    txtHairColor.Text = Dt.Rows[0]["HairColor"].ToString().Trim();
                    txtDisMarks.Text = Dt.Rows[0]["DistinguishMark"].ToString().Trim();

                    txtCaseManager.Text = Dt.Rows[0]["CaseManagerResidential"].ToString().Trim();
                    txtCaseManEdu.Text = Dt.Rows[0]["CaseManagerEducational"].ToString().Trim();
                    txtDateofLast.Text = Dt.Rows[0]["LastPhysicalDate"].ToString().Trim();
                    txtMedicalCon.Text = Dt.Rows[0]["MedicalConditions"].ToString().Trim();
                    txtAllergies.Text = Dt.Rows[0]["Allergies"].ToString().Trim();
                    txtCurrentMed.Text = Dt.Rows[0]["CurrentMedications"].ToString().Trim();

                    txtSelfPres.Text = Dt.Rows[0]["SelfPresAbility"].ToString().Trim();
                    txtSignBehv.Text = Dt.Rows[0]["SignificantBehavior"].ToString().Trim();
                    txtCapabilities.Text = Dt.Rows[0]["Capabilities"].ToString().Trim();
                    txtLimitations.Text = Dt.Rows[0]["Limitations"].ToString().Trim();
                    txtPreferences.Text = Dt.Rows[0]["Performances"].ToString().Trim();
                    txtLegal.Text = Dt.Rows[0]["LegalComp"].ToString().Trim();
                    txtMaritulS.Text = Dt.Rows[0]["MaritulStatus"].ToString().Trim();
                    txtOtherStage.Text = Dt.Rows[0]["OtherStageAgency"].ToString().Trim();



                     txtCitizenShip.Text = Dt.Rows[0]["Citizenship"].ToString().Trim();
                    txtGuardianship.Text = Dt.Rows[0]["GuardenshipStatus"].ToString().Trim();
                    txtPrimaryLang.Text = Dt.Rows[0]["PrimaryLang"].ToString().Trim();                   


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

                    ddlState_SelectedIndexChanged(this, EventArgs.Empty);
                    txtDistrictCode.Text = Dt.Rows[0]["DistrictCode"].ToString().Trim();
                    txtDistFunct.Text = Dt.Rows[0]["DistFunction"].ToString().Trim(); ;
                    txtDistType.Text = Dt.Rows[0]["DistrictType"].ToString().Trim(); ;
                    txtContact.Text = Dt.Rows[0]["DistContactPerson"].ToString().Trim(); ;
                    txtCity.Text = Dt.Rows[0]["City"].ToString().Trim();
                    txtZip.Text = Dt.Rows[0]["Zip"].ToString();
                    txtHomePhone.Text = Dt.Rows[0]["HomePhone"].ToString().Trim();
                    txtMobile.Text = Dt.Rows[0]["Mobile"].ToString().Trim();
                    txtEmail.Text = Dt.Rows[0]["Email"].ToString().Trim();
                    intAddressId = Convert.ToInt16(Dt.Rows[0]["AddId"]);
                    txtGrade.Text = Dt.Rows[0]["GradeLevel"].ToString().Trim();
                    Boolean resident = Convert.ToBoolean(Dt.Rows[0]["ResidenceInd"].ToString().Trim());
                    if (resident == true)
                        ddlResident.SelectedIndex = 2;
                    else
                        ddlResident.SelectedIndex = 1;
                    sess.AdmStudentId = 0;
                    Session["Status"] = null;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Fill", "$(document).ready(function(){assignDOBDate('" + Img + "');});", true);
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "Fill", "$(document).ready(function(){setImg('" + Img + "');});", true);
                }
            }

            catch (Exception Ex)
            {

                tdMsg.InnerHtml = clsGeneral.failedMsg("Failed... ");
                throw Ex;
            }
        }
    }

    protected void UpdateStudentAndAddress()
    {
        objData = new clsData();
        objdataClass = new DataClass();

        if (txtHomePhone.Text != "" && clsGeneral.IsItValidPhone(txtHomePhone.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Phone Number Must Be Entered As: (xxx)xxx-xxxx");
            txtHomePhone.Focus();
            return;
        }
        else if (txtZip.Text == "" || clsGeneral.IsItZip(txtZip.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid Zip Code");
            txtZip.Focus();
            return;
        }
        else if (txtMobile.Text != "" && clsGeneral.IsItValidPhone(txtMobile.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Mobile Number Must Be Entered As: (xxx)xxx-xxxx");
            txtMobile.Focus();
            return;
        }


        else

            try
            {
                bool Residence = true;
                if (ddlResident.SelectedItem.Text.Trim() == "Non Resident")
                    Residence = false;
                if (ddlStatus.SelectedItem.Text == "Active")
                {
                    string active = "Update Student Set ActiveInd='A' Where StudentId=" + intStudentId + "";
                    Result = Convert.ToBoolean(objdataClass.ExecuteNonQuery(active));
                }
                string add = " Update Address Set AddressLine1='" + clsGeneral.convertQuotes(txtAddress1.Text.Trim()) + "'  ,AddressLine2='" + clsGeneral.convertQuotes(txtAddress2.Text.Trim()) + "',AddressLine3='" + clsGeneral.convertQuotes(txtAddress3.Text.Trim()) + "', " +
                             "City='" + clsGeneral.convertQuotes(txtCity.Text.Trim()) + "',State='" + ddlState.Text.Trim() + "',Country='" + ddlCountry.Text.Trim() + "',Zip='" + txtZip.Text.Trim() + "', " +
                             " HomePhone='" + txtHomePhone.Text.Trim() + "',Mobile='" + txtMobile.Text.Trim() + "',Email='" + txtEmail.Text.Trim() + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn = (SELECT Convert(Varchar,getdate(),100)) where AddressId=" + intAddressId + "";
                Boolean index = Convert.ToBoolean(objdataClass.ExecuteNonQuery(add));

                add = "Update Student Set AddressId='" + intAddressId + "' ,SchoolId=" + sess.SchoolId + " , SASID='" + txtSASID.Text.Trim() + "'," +
                       "StudentNbr='" + clsGeneral.convertQuotes(txtStudentId.Text) + "' ,StudentFname='" + clsGeneral.convertQuotes(txtFirstName.Text.Trim()) + "' ,StudentLname='" + clsGeneral.convertQuotes(txtLastName.Text.Trim()) + "' , ResidenceInd='" + Residence + "'," +
                        "GradelEVEL='" + clsGeneral.convertQuotes(txtGrade.Text.Trim()) + "' , Gender='" + ddlGender.Text.Trim() + "' , JoinDt= CONVERT(datetime,'" + txtJoinDt.Text.Trim() + "') , DOB=CONVERT(datetime,'" + txtDOB.Text.Trim() + "'), " +
                         "ModifiedBy='" + sess.LoginId + "' ,ModifiedOn=(SELECT Convert(Varchar,getdate(),100))  where StudentId=" + intStudentId + "";
             
                
                index = Convert.ToBoolean(objdataClass.ExecuteNonQuery(add));


                add = "  Update Student Set Race='"+clsGeneral.convertQuotes(txtRace.Text.Trim())+"',Height='"+clsGeneral.convertQuotes(txtHeight.Text.Trim())+"',Weight='"+clsGeneral.convertQuotes(txtWeight.Text.Trim())+"',EyeColor='"+clsGeneral.convertQuotes(txtEyeColor.Text.Trim())+"',HairColor='"+clsGeneral.convertQuotes(txtHairColor.Text.Trim())+"',DistinguishMark='"+clsGeneral.convertQuotes(txtDisMarks.Text.Trim())+"',CaseManagerResidential='"+clsGeneral.convertQuotes(txtCaseManager.Text.Trim())+"',CaseManagerEducational='"+clsGeneral.convertQuotes(txtCaseManEdu.Text.Trim())+"',LastPhysicalDate='"+clsGeneral.convertQuotes(txtDateofLast.Text.Trim())+"',MedicalConditions='"+clsGeneral.convertQuotes(txtMedicalCon.Text.Trim())+"',Allergies='"+clsGeneral.convertQuotes(txtAllergies.Text.Trim())+"',CurrentMedications='"+clsGeneral.convertQuotes(txtCurrentMed.Text.Trim())+"',SelfPresAbility='"+clsGeneral.convertQuotes(txtSelfPres.Text.Trim())+"',SignificantBehavior='"+clsGeneral.convertQuotes(txtSignBehv.Text.Trim())+"',Capabilities='"+clsGeneral.convertQuotes(txtCapabilities.Text.Trim())+"',Limitations='"+clsGeneral.convertQuotes(txtLimitations.Text.Trim())+"',Performances='"+clsGeneral.convertQuotes(txtPreferences.Text.Trim())+"',DistrictCode='"+clsGeneral.convertQuotes(txtDistrictCode.Text.Trim())+"',DistrictType='"+clsGeneral.convertQuotes(txtDistType.Text.Trim())+"',DistFunction='"+clsGeneral.convertQuotes(txtDistFunct.Text.Trim())+"',DistContactPerson='"+clsGeneral.convertQuotes(txtContact.Text.Trim())+"',Citizenship='"+clsGeneral.convertQuotes(txtCitizenShip.Text.Trim())+"',GuardenshipStatus='"+clsGeneral.convertQuotes(txtGuardianship.Text.Trim())+"',PrimaryLang='"+clsGeneral.convertQuotes(txtPrimaryLang.Text.Trim())+"', LegalComp='"+clsGeneral.convertQuotes(txtLegal.Text.Trim())+"',MaritulStatus='"+clsGeneral.convertQuotes(txtMaritulS.Text.Trim())+"',OtherStageAgency='"+clsGeneral.convertQuotes(txtOtherStage.Text.Trim())+"' where StudentId=" + intStudentId + "";


                index = Convert.ToBoolean(objdataClass.ExecuteNonQuery(add));


                //add = "Update StdtClass Set SchoolId=" + sess.SchoolId + ",ClassId='" + Convert.ToInt32(ddlClassName.SelectedItem.Value) + "',ModifiedBy='" + sess.LoginId + "',ModifiedOn=(SELECT Convert(Varchar,getdate(),100)) where StdtId=" + intStudentId + "";
                //index = Convert.ToBoolean(objdataClass.ExecuteNonQuery(add));

                //if (index == false)
                //{
                //    tdMsg.InnerHtml = clsGeneral.failedMsg("Student Updation Failed!");
                //}

                if (fileUpl_stdPhoto.HasFile)
                {
                    string selctImage = "SELECT ImageURL from Student WHERE StudentId = " + intStudentId + "";
                    string imageId = objData.FetchValue(selctImage).ToString();
                    string photo = PhotoPreviewandSave();
                    string updateData = "UPDATE Student Set ImageURL = '" + photo + "' WHERE StudentId = " + intStudentId + "";
                    int idIndex = objData.Execute(updateData);
                    if ((idIndex > 0) && (imageId != "defaultStudent.png"))
                    {
                        File.Delete(Server.MapPath(@"~/Administration/StudentsPhoto/" + imageId));
                    }
                }

                //  tdMsg.InnerHtml = clsGeneral.sucessMsg("Student Details Updated Successfully");
                if (index == true || Result == true)
                {
                    fillStudent();
                    tdMsg.InnerHtml = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "alert",
                    "alert('Student Details Updated Successfully ...');window.location = 'StudentMenu.aspx';", true);
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Student Updation Failed!");
                }
            }

            catch (Exception Ex)
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg("Student Updation Failed!");
                throw Ex;
            }

    }

    protected void SaveStudent()
    {
        clsData.blnTrans = true;
        sess = (clsSession)Session["UserSession"];
        int Id = sess.LoginId;
        SqlTransaction Transs = null;

    
        if (txtStudentId.Text.Trim() != "")
        {
            if (clsGeneral.IsExit("StudentNbr", "Student", txtStudentId.Text.Trim()) == true)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Student Number already exit.Please choose another number.");
                txtStudentId.Focus();
                return;
            }
        }

        if (txtHomePhone.Text != "" && clsGeneral.IsItValidPhone(txtHomePhone.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Phone Number Must Be Entered As: (xxx)xxx-xxxx");
            txtHomePhone.Focus();
            return;
        }
        else if (txtMobile.Text != "" && clsGeneral.IsItValidPhone(txtMobile.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Mobile Number Must Be Entered As: (xxx)xxx-xxxx");
            txtMobile.Focus();
            return;
        }
        else if (txtZip.Text == "" || clsGeneral.IsItZip(txtZip.Text.Trim()) == false)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter a Valid Zip Code");
            txtZip.Focus();
            return;
        }



        objData = new clsData();
        SqlConnection con = objData.Open();

        try
        {

          

            objdataClass = new DataClass();
            clsData.blnTrans = true;
           
            Transs = con.BeginTransaction();
            int index;
            bool Residence = true;
            if (ddlResident.SelectedItem.Text.Trim() == "Non Resident")
                Residence = false;

            string Photo = PhotoPreviewandSave();
            string add = " Insert into Address(AddressLine1,AddressLine2,AddressLine3,City,State,Country,Zip,HomePhone,Mobile,Email,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) Values('" + clsGeneral.convertQuotes(txtAddress1.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtAddress2.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtAddress3.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCity.Text.Trim()) + "','" + ddlState.SelectedValue + "','" + ddlCountry.SelectedValue + "','" + txtZip.Text.Trim() + "','" + txtHomePhone.Text.Trim() + "','" + txtMobile.Text.Trim() + "' ,'" + txtEmail.Text.Trim() + "' ,'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)),'" + sess.LoginId + "' ,(SELECT Convert(Varchar,getdate(),100)))";
            intAddressId = Convert.ToInt32(objData.ExecuteWithScopeandConnection(add, con, Transs));

            string Query = " Insert into Student (AddressId,SchoolId,StudentNbr,SASID,StudentFname,StudentLname,Gender,DOB,JoinDt,GradeLevel,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ImageUrl,ResidenceInd " +
                ",Race,Height,Weight,EyeColor,HairColor,DistinguishMark,CaseManagerResidential,CaseManagerEducational,LastPhysicalDate,MedicalConditions,Allergies,CurrentMedications,SelfPresAbility,SignificantBehavior,Capabilities,Limitations,Performances,DistrictCode,DistrictType,DistFunction,DistContactPerson,Citizenship,GuardenshipStatus,PrimaryLang,OtherStageAgency,LegalComp,MaritulStatus) " +
                               "  Values(" + intAddressId + "," + sess.SchoolId + ",'" + clsGeneral.convertQuotes(txtStudentId.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSASID.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtFirstName.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtLastName.Text.Trim()) + "','" + ddlGender.SelectedItem.Text.Trim() + "', CONVERT(datetime,'" + txtDOB.Text + "'),CONVERT(datetime,'" + txtJoinDt.Text + "'), '" + clsGeneral.convertQuotes(txtGrade.Text.Trim()) + "','A'," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100))," + sess.LoginId + ",(SELECT Convert(Varchar,getdate(),100)),'" + Photo + "','" + Residence + "'," +
            "'" + clsGeneral.convertQuotes(txtRace.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtHeight.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtWeight.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtEyeColor.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtHairColor.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDisMarks.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCaseManager.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCaseManEdu.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDateofLast.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtMedicalCon.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtAllergies.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCurrentMed.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSelfPres.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtSignBehv.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCapabilities.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtLimitations.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtPreferences.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDistrictCode.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDistType.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtDistFunct.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtContact.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtCitizenShip.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtGuardianship.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtPrimaryLang.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtOtherStage.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtLegal.Text.Trim()) + "','" + clsGeneral.convertQuotes(txtMaritulS.Text.Trim()) + "')";

            index = Convert.ToInt32(objData.ExecuteWithScopeandConnection(Query, con, Transs));

            objData.CommitTransation(Transs, con);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "None", "setImg('');", true);

            tdMsg.InnerHtml = clsGeneral.sucessMsg("Student Details Inserted Successfully");
            ClearData();
        }
        catch (Exception Ex)
        {
            objData.RollBackTransation(Transs, con);
            string error = Ex.Message;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Student Insertion Failed! ");
            throw Ex;
        }

    }



    protected void txtAddress1_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Button_Add_Click(object sender, EventArgs e)
    {
        if (intStudentId == 0)
        {
            SaveStudent();
        }
        else
        {
            UpdateStudentAndAddress();

        }
    }


    protected void Button_Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("StudentMenu.aspx");
    }


    protected void ClearData()
    {
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtAddress3.Text = "";
        txtCity.Text = "";
        txtDOB.Text = "";
        txtEmail.Text = "";
        txtFirstName.Text = "";
        txtGrade.Text = "";
        txtHomePhone.Text = "";
        txtJoinDt.Text = "";
        txtLastName.Text = "";
        txtMobile.Text = "";
        txtStudentId.Text = "";
        txtZip.Text = "";
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        txtSASID.Text = "";
        ddlResident.SelectedIndex = 0;
        txtDistrictCode.Text = "";
        txtRace.Text = "";
        txtHeight.Text = "";
        txtWeight.Text = "";
        txtHairColor.Text = "";

        txtEyeColor.Text = "";
        txtDisMarks.Text = "";
        txtCaseManager.Text = "";
        txtCaseManEdu.Text = "";


       

        txtLegal.Text = "";
        txtMaritulS.Text = "";
        txtOtherStage.Text = "";
        txtCitizenShip.Text = "";


        txtGuardianship.Text = "";
        txtPrimaryLang.Text = "";
        txtDateofLast.Text = "";
        txtMedicalCon.Text = "";

        txtAllergies.Text = "";
        txtSelfPres.Text = "";
        txtCapabilities.Text = "";
        txtPreferences.Text = "";

        txtMedicalCon.Text = "";
        txtCurrentMed.Text = "";
        txtSignBehv.Text = "";
        txtLimitations.Text = "";


        txtDistFunct.Text = "";
        txtDistType.Text = "";
        txtContact.Text = "";
        Session["StudentId"] = null;
    }


    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCountry.SelectedIndex != 0)
            {
                ddlState.Items.Clear();
                objData = new clsData();
                int countryId = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
                objData.ReturnDropDown("SELECT LookupId As Id,LookupName As Name from LookUp where ParentLookupId = " + countryId + " and LookupType='State'", ddlState);
                if (ddlState.Items.Count == 0)
                {
                    ddlState.Items.Insert(0, new ListItem("---------------Select--------------", "0"));
                }
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

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }

   
}