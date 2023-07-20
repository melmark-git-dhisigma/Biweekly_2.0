using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

public partial class Administration_SystemMessage : System.Web.UI.Page
{
    DataClass objDataClass = new DataClass();
    clsData objData = null;
    clsSession sess = null;
  
    public bool AccessInd;

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
        if (!IsPostBack)
        {
            String fetchqry = "select Mesag from Mesag;";
            objData = new clsData();
            SqlConnection con = objData.Open();
            String msg = Convert.ToString(objData.FetchValue(fetchqry));
            TAMessage.Text = msg;
        }
    }
    public void BtnSetMessage_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        string strQuery = "";

        try
        {
            SqlTransaction Transs = null;
            SqlConnection con = objData.Open();
            clsData.blnTrans = true;
            Transs = con.BeginTransaction();
            String updtqry = "";
            String msgval = TAMessage.Text;
            //if (msgval == "" || msgval == null)
            //{
                //lblMesg.Text = "Please Enter Message...";
                //lblMesg.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "NoMessage();", true);
            //}
            //else
            //{
                updtqry = "UPDATE Mesag SET Mesag='" + clsGeneral.convertQuotes(msgval) + "' ,ModifiedOn=getdate() ";
                objData.Execute(updtqry);
                lblMesg.Text = "Message Saved successfully...";
                lblMesg.Visible = true;
                TAMessage.Text = "";
                //  strQuery = "INSERT INTO Mesg (Mesg,CreatedOn) VALUES('" + TAMessage.Text + "',(SELECT Convert(Varchar,getdate(),100)))";
                // int MsgId = objData.ExecuteWithScopeandConnection(updtqry, con, Transs);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), "MessageUpdated();", true);
                TAMessage.Text = msgval;
            //}
        }
        catch (Exception exe)
        {
            throw exe;
        }

    }
}