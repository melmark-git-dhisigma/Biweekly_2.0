using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text.RegularExpressions;

public partial class Administration_SystemMessage : System.Web.UI.Page
{
    DataClass objDataClass = new DataClass();
    clsData objData = null; 
    clsSession sess = null;
  
    public bool AccessInd;
    public int errorStatus = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        tdReadMsg.Visible = false;
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
            objData = new clsData();
            string fetchqry = "SELECT UserEmail,UserPassword,SMTPServer,SMTPPort FROM EmailDetails";
            DataTable DtEmail = new DataTable();
            DtEmail = objData.ReturnDataTable(fetchqry, true);

            foreach (DataRow DtRow in DtEmail.Rows)
            {
                txtEmail.Text = DtRow["UserEmail"].ToString();
                txtPassword.Text = DtRow["UserPassword"].ToString();
                txtPwdVisible.Text = DtRow["UserPassword"].ToString();
                txtSMTPadrs.Text = DtRow["SMTPServer"].ToString();
                txtSMTPport.Text = DtRow["SMTPPort"].ToString();
            } 

        }
    }
    public void BtnSetMessage_Click(object sender, EventArgs e)
    {
        objData = new clsData();     

        string useremails = txtEmail.Text;
        string userpswd = txtPassword.Text;
        string smtpserver = txtSMTPadrs.Text;
        string smtpport = txtSMTPport.Text;
        string updtqry = "";

        if (Validation() == true)
        {
            try
            {
                SqlConnection con = objData.Open();

                string msgval = TAMessage.Text;
                object set;
                updtqry = "UPDATE EmailDetails SET UserEmail = '" + useremails + "', UserPassword = '" + userpswd + "', SMTPServer = '" + smtpserver + "', SMTPPort = '" + smtpport + "', ModifiedOn = GETDATE(), ModifiedBy = " + sess.LoginId + "";
                set = objData.Execute(updtqry);
                bool status = Convert.ToBoolean(set);

                if (status)
                {
                    //lblMesg.Text = "Email Details Saved successfully...";
                    //TAMessage.Text = "";
                    //TAMessage.Text = msgval;
                    tdReadMsg.Visible = true;
                    tdReadMsg.InnerHtml = clsGeneral.sucessMsg("Email Details Saved successfully...");
                    lblMesg.Visible = true;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "MessageUpdated();", true);

                }
                else
                {
                    tdReadMsg.InnerHtml = clsGeneral.failedMsg("Email Details Not Saved successfully...");
                }
            }
            catch (Exception exe)
            {
                throw exe;
            }
        }
        else
        {
            tdReadMsg.Visible = true;
            if (errorStatus == 1 || errorStatus == 2 || errorStatus == 3 || errorStatus == 4)
            {
                tdReadMsg.InnerHtml = clsGeneral.failedMsg("Please fill all mandatory fields...");
            }
            else if(errorStatus == 5)
            {
                tdReadMsg.InnerHtml = clsGeneral.failedMsg("Please check the email entered...");
            }
            else if (errorStatus == 6)
            {
                tdReadMsg.InnerHtml = clsGeneral.failedMsg("Please check the host address entered...");
            }
             else if (errorStatus == 7)
            {
                tdReadMsg.InnerHtml = clsGeneral.failedMsg("Please check the port Number.. Only Numeric are allowed...");
            }
        }
    }
    protected void txtPassword_TextChanged(object sender, EventArgs e)
    {
        txtPwdVisible.Text = txtPassword.Text;
    }

    public bool Validation()
    {
        bool status = true;

        string vuseremails = txtEmail.Text;
        string vuserpswd = txtPassword.Text;
        string vsmtpserver = txtSMTPadrs.Text;
        string vsmtpport = txtSMTPport.Text;

        if (vuseremails == null || vuseremails == "")
        {
            status = false;
            errorStatus = 1;
        }
        else if (vuserpswd == null || vuserpswd == "")
        {
            status = false;
            errorStatus = 2;
        }
        else if (vsmtpserver == null || vsmtpserver == "")
        {
            status = false;
            errorStatus = 3;
        }
        else if (vsmtpport == null || vsmtpport == "")
        {
            status = false;
            errorStatus = 4;
        }
        else
        {
            if (status == true)
            {
                if (vuseremails != null && vuseremails != "")
                {
                    string emailrgx = vuseremails;
                    if (IsValidMail(emailrgx))
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                        errorStatus = 5;
                    }
                }
            }

            if (status == true)
            {
                if (vsmtpserver != null && vsmtpserver != "")
                {
                    string hostAdd = vsmtpserver;
                    if (IsValidHost(hostAdd))
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                        errorStatus = 6;
                    }
                }
            }

            if (status == true)
            {
                if (vsmtpport != null || vsmtpport != "")
                {
                    string portAdd = vsmtpport;
                    if (IsValidPort(portAdd))
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                        errorStatus = 7;
                    }
                }
            }
        }
        return status;
    }

    public bool IsValidMail(string emailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailaddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public bool IsValidHost(string hostaddress)
    {
        try
        {
            bool hostStatus;
            Regex regex = new Regex("^(?!-)[A-Za-z0-9-]+([\\-\\.]{1}[a-z0-9]+)*\\.[A-Za-z]{2,6}$");
            Match match = regex.Match(hostaddress);
            if (match.Success)
            {
                hostStatus = true;
            }
            else
            {
                hostStatus = false;
            }
            return hostStatus;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public bool IsValidPort(string portaddress)
    {
        try
        {
            bool portStatus;
            Regex regex = new Regex("^[0-9]+$");
            Match match = regex.Match(portaddress);
            if (match.Success)
            {
                portStatus = true;
            }
            else
            {
                portStatus = false;
            }
            return portStatus;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}