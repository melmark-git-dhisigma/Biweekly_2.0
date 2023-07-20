using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Administration_LookUpConf : System.Web.UI.Page
{
    clsSession sess;
    ClsTemplateSession ObjTempSess;
    bool Disabled = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];

        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];


        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            //HiddenField1.Value = sess.UserName;
            bool flag = clsGeneral.PageIdentification(sess.perPage);
            if (flag == false)
            {
                Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            }
        }
        // FillGrid();
        pnlMessage.Visible = false;
        pnlMessageGoal.Visible = false;

        clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disabled);
        if (Disabled == true)
        {
            hdnTeachProcStatus.Value = "false";
            btnAdd.Visible = false;
            btnAddGoal.Visible = false;
            btnAddAssessment.Visible = false;
        }
        else
        {
            hdnTeachProcStatus.Value = "true";
            btnAdd.Visible = true;
            btnAddGoal.Visible = true;
            btnAddAssessment.Visible = true;
        }
    }

    protected void grdViewAssessment_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        string commandArgument = e.CommandArgument.ToString();
        string commandName = e.CommandName.ToString();

        switch (commandName)
        {
            case "DeleteAssessment":
                pnlMessage.CssClass = "pnlMessage_green";
                lblMessage.Text = "Just Testing";

                int inUsecount = isDeletable_goal(Convert.ToInt32(commandArgument));

                if (inUsecount == 0)
                {

                    string strQuerry = "";
                    try
                    {
                        clsData oData = new clsData();
                        clsSession sess = (clsSession)Session["UserSession"];
                        strQuerry = "DELETE FROM Assessment WHERE  AsmntId = '" + commandArgument + "' ";
                        oData.Execute2(strQuerry);

                        pnlMessage.CssClass = "pnlMessage_green";
                        lblMessage.Text = "Deleted Successfully";
                        pnlMessage.Visible = true;

                        FillGrid();
                    }
                    catch (Exception Ex)
                    {
                        pnlMessage.CssClass = "pnlMessage_red";
                        lblMessage.Text = Ex.ToString();
                        pnlMessage.Visible = true;
                        //throw Ex;
                    }
                }
                else
                {
                    pnlMessage.CssClass = "pnlMessage_red";
                    lblMessage.Text = (inUsecount == 1) ? "Can't Delete since it is currently mapped to a lesson plan." : "Can't Delete since it is currently mapped to " + inUsecount + " lesson plans";
                    pnlMessage.Visible = true;
                }
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel2, UpdatePanel2.GetType(), "Any Name you Like", "$(document).ready(function(){ setInterval(function(){ $('.pnlMessage_green,.pnlMessage_red').slideUp('slow',function(){$(this).remove();}); }, 3000); });", true);

                break;

            case "EditAssessment":

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdViewGoals.Rows[index];

                hdnAssessmentId.Value = ((HiddenField)row.FindControl("hdnLookupId")).Value;
                hdnAssessmentName.Value = ((Label)row.FindControl("lblAssessmentName")).Text;
                // lblAssessmentCode_popup.Text = ((Label)row.FindControl("lblAssessmentCode")).Text;

                txtEditAssessment.Text = ((Label)row.FindControl("lblAssessmentName")).Text;
                pnlOverlay2.Visible = true;
                pnlAssessmentEditBox.Visible = true;
                lblEditAssessmentMessage.Text = "";
                break;


            default:
                break;

        }


    }
    protected void grdViewTeachingProc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string commandArgument = e.CommandArgument.ToString();
        string commandName = e.CommandName.ToString();

        switch (commandName)
        {
            case "DeleteTeachProc":
                pnlMessage.CssClass = "pnlMessage_green";
                lblMessage.Text = "Just Testing";

                int inUsecount = isDeletable_techProc(Convert.ToInt32(commandArgument));
                string strQuerry = "";
                clsData oData = new clsData();
                if (inUsecount == 0)
                {

                    try
                    {

                        clsSession sess = (clsSession)Session["UserSession"];
                        strQuerry = "DELETE FROM LookUp WHERE  LookupId = '" + commandArgument + "' ";
                        oData.Execute2(strQuerry);

                        pnlMessage.CssClass = "pnlMessage_green";
                        lblMessage.Text = "Deleted Successfully";
                        pnlMessage.Visible = true;

                        grdViewTeachingProc.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        pnlMessage.CssClass = "pnlMessage_red";
                        lblMessage.Text = Ex.ToString();
                        pnlMessage.Visible = true;
                        //throw Ex;
                    }
                }
                else
                {
                    //pnlMessage.CssClass = "pnlMessage_red";
                    //lblMessage.Text = (inUsecount == 1) ? "Can't Delete since it is used in a template" : "Can't Delete since it is used in " + inUsecount + " templates";
                    //pnlMessage.Visible = true;

                    strQuerry = "UPDATE  Lookup SET ActiveInd = 'D' WHERE  LookupId=" + commandArgument;
                    oData.Execute2(strQuerry);
                    pnlMessage.CssClass = "pnlMessage_green";
                    lblMessage.Text = "Deleted Successfully";
                    pnlMessage.Visible = true;
                    grdViewTeachingProc.DataBind();
                }

                // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "Any Name you Like", "$(document).ready(function(){ setInterval(function(){ //$('.pnlMessage_green,.pnlMessage_red').slideUp('slow',function(){$(this).remove();}); }, 3000); });", true);

                break;
            case "EditTeachProc":
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdViewTeachingProc.Rows[index];

                hdnTeachProcId.Value = ((HiddenField)row.FindControl("hdnTPId")).Value;
                hdnTeachProcName.Value = ((Label)row.FindControl("Label2")).Text;
                txtEditTeachingProc.Text = ((Label)row.FindControl("Label2")).Text;
                pnlOverlay.Visible = true;
                pnlTeachingProcEdit.Visible = true;
                lblEditTeachingProcMessage.Text = "";
                break;
            default:
                break;

        }
    }


    protected int isDeletable_techProc(int teachingProcId)
    {
        int retCount = 0;
        string strQuerry = "";
        try
        {
            clsData oData = new clsData();
            clsSession sess = (clsSession)Session["UserSession"];
            strQuerry = "SELECT COUNT(*) FROM DSTempHdr where TeachingProcId=" + teachingProcId;
            retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        }
        catch (Exception Ex)
        {
            //throw Ex;
        }

        return retCount;
    }

    protected int isDeletable_goal(int goalId)
    {
        int retCount = 0;
        string strQuerry = "";
        try
        {
            clsData oData = new clsData();
            clsSession sess = (clsSession)Session["UserSession"];
            strQuerry = "SELECT COUNT(*) FROM GoalLPRel where GoalId=" + goalId;
            retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        }
        catch (Exception Ex)
        {
            //throw Ex;
        }

        return retCount;
    }

    protected int isDeletable_Assessment(String assmtname)
    {
        int retCount = 0;
        string strQuerry = "";
        try
        {
            clsData oData = new clsData();
            clsSession sess = (clsSession)Session["UserSession"];
            strQuerry = "SELECT COUNT(*) FROM AsmntLPRel where AsmntName=" + assmtname;
            retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        }
        catch (Exception Ex)
        {
            //throw Ex;
        }

        return retCount;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtAdd.Text.Trim() != "")
        {
            clsData oData = new clsData();
            string strQuerry = "";
            if (isDuplicateTP(txtAdd.Text.Trim()))
            {
                pnlMessage.CssClass = "pnlMessage_green";
                lblMessage.Text = "Just Testing";


                try
                {

                    clsSession sess = (clsSession)Session["UserSession"];
                    strQuerry = "INSERT INTO LookUp (SchoolId,LookupType,LookupName,LookupCode,LookupDesc,ActiveInd,CreatedBy,CreateOn,isDynamic) values('" + sess.SchoolId + "','Datasheet-Teaching Procedures','" + clsGeneral.convertQuotes(txtAdd.Text) + "','" + clsGeneral.convertQuotes(txtAdd.Text) + "','" + clsGeneral.convertQuotes(txtAdd.Text) + "','A','" + sess.LoginId + "',GETDATE(),1)";
                    oData.Execute2(strQuerry);

                    pnlMessage.CssClass = "pnlMessage_green";
                    lblMessage.Text = "Added Successfully";
                    pnlMessage.Visible = true;

                    txtAdd.Text = "";

                    grdViewTeachingProc.DataBind();
                }
                catch (Exception Ex)
                {
                    pnlMessage.CssClass = "pnlMessage_red";
                    lblMessage.Text = Ex.ToString();
                    pnlMessage.Visible = true;
                    //throw Ex;
                }
            }
            else
            {
                if (isDuplicateDeltdTP(txtAdd.Text.Trim()))
                {
                    pnlMessageAssessment.CssClass = "pnlMessage_green";
                    lblMessageAssessment.Text = "Just Testing";
                    try
                    {
                        Guid g;
                        g = Guid.NewGuid();
                        string tp_code = g + "_" + txtAdd.Text;
                        clsSession sess = (clsSession)Session["UserSession"];
                        strQuerry = "INSERT INTO LookUp (SchoolId,LookupType,LookupName,LookupCode,LookupDesc,ActiveInd,CreatedBy,CreateOn,isDynamic) values('" + sess.SchoolId + "','Datasheet-Teaching Procedures','" + clsGeneral.convertQuotes(tp_code) + "','" + clsGeneral.convertQuotes(txtAdd.Text) + "','" + clsGeneral.convertQuotes(txtAdd.Text) + "','A','" + sess.LoginId + "',GETDATE(),1)";
                        oData.Execute2(strQuerry);

                        pnlMessage.CssClass = "pnlMessage_green";
                        lblMessage.Text = "Added Successfully";
                        pnlMessage.Visible = true;

                        txtAdd.Text = "";

                        grdViewTeachingProc.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        pnlMessage.CssClass = "pnlMessage_red";
                        lblMessage.Text = Ex.ToString();
                        pnlMessage.Visible = true;
                        //throw Ex;
                    }
                }
                else
                {
                    pnlMessage.CssClass = "pnlMessage_red";
                    lblMessage.Text = "Teaching Procedure Name already exist";
                    pnlMessage.Visible = true;
                }
            }
        }
        else
        {

            pnlMessage.CssClass = "pnlMessage_red";
            lblMessage.Text = "Please enter a Teaching Procedure Name";
            pnlMessage.Visible = true;

        }

        // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "Any Name you Like", "$(document).ready(function(){ setInterval(function(){ $('.pnlMessage_green,.pnlMessage_red').slideUp('slow',function(){$(this).remove();}); }, 3000); });", true);

    }

    public bool isDuplicateTP(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "select count(*) from LookUp where LookupType = 'Datasheet-Teaching Procedures' and (LookupName ='" + Name + "' OR LookupCode = '" + Name + "')";
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {
            return false;
        }

        return true;


    }
    public bool isDuplicateUpdateTP(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "SELECT COUNT(*) FROM LookUp WHERE SchoolId =" + sess.SchoolId + " and LookupType = 'Datasheet-Teaching Procedures' and (LookupCode = '" + Name + "' and ActiveInd='A')"; 
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {
            return false;
        }

        return true;
    }
    public bool isDuplicateDeltdTP(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "select count(*) from LookUp where LookupType = 'Datasheet-Teaching Procedures' and LookupCode ='" + Name + "' and ActiveInd='A'";
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));
        if (retCount > 0)
        {
            return false;
        }
        strQuerry = "select count(*) from LookUp where LookupType = 'Datasheet-Teaching Procedures' and LookupCode ='" + Name + "' and ActiveInd='D'";
        retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {

            return true;
        }

        return false;
    }
    

    public bool isDuplicateAssessment(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "SELECT COUNT(*) FROM Lookup WHERE SchoolId =" + sess.SchoolId + " and LookupType = 'Assessment Name' and (LookupName = '" + Name + "' OR LookupCode = '" + Name + "')";
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {
            return false;
        }

        return true;
    }
    public bool isDuplicateDeltdAssessment(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "select count(*) from LookUp where LookupType = 'Assessment Name' and LookupName ='" + Name + "' and ActiveInd='A'";
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));
        if (retCount > 0)
        {
            return false;
        }
        strQuerry = "select count(*) from LookUp where LookupType = 'Assessment Name' and LookupName ='" + Name + "' and ActiveInd='D'";
        retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {

            return true;
        }

        return false;
    }
    
    public bool isDuplicateAssessmentOnUpdate(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "SELECT COUNT(*) FROM LookUp WHERE SchoolId =" + sess.SchoolId + " and LookupType = 'Assessment Name' and (LookupName = '" + Name + "' and ActiveInd='A')"; 
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {
            return false;
        }

        return true;
    }
    public bool isDuplicateGoal(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "SELECT COUNT(*) FROM Goal WHERE SchoolId =" + sess.SchoolId + "and GoalTypeId = 1 and GoalLevelId = 1 and GoalDesc = 'AssessmentGoal' and (GoalCode = '" + Name + "' OR GoalName = '" + Name + "')";
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {
            return false;
        }

        return true;
    }

    public bool isDuplicateGoalOnUpdate(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "SELECT COUNT(*) FROM Goal WHERE SchoolId =" + sess.SchoolId + "and GoalTypeId = 1 and GoalLevelId = 1 and GoalDesc = 'AssessmentGoal' and GoalCode = '" + Name + "' and ActiveInd='A' ";
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {
            return false;
        }

        return true;
    }

    public bool isDuplicateDeltdGoal(string Name)
    {

        clsData oData = new clsData();
        clsSession sess = (clsSession)Session["UserSession"];
        string strQuerry = "SELECT COUNT(*) FROM Goal WHERE SchoolId =" + sess.SchoolId + "and GoalTypeId = 1 and GoalLevelId = 1 and GoalDesc = 'AssessmentGoal' and GoalCode = '" + Name + "' and ActiveInd='A'";
        int retCount = Convert.ToInt32(oData.FetchValue(strQuerry));
        if (retCount > 0)
        {
            return false;
        }
        strQuerry = "SELECT COUNT(*) FROM Goal WHERE SchoolId =" + sess.SchoolId + "and GoalTypeId = 1 and GoalLevelId = 1 and GoalDesc = 'AssessmentGoal' and GoalCode = '" + Name + "' and ActiveInd='D'";
        retCount = Convert.ToInt32(oData.FetchValue(strQuerry));

        if (retCount > 0)
        {

            return true;
        }

        return false;
    }

    protected void grdViewGoals_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        string commandArgument = e.CommandArgument.ToString();
        string commandName = e.CommandName.ToString();

        switch (commandName)
        {
            case "DeleteGoal":
                pnlMessageGoal.CssClass = "pnlMessage_green";
                lblMessageGoal.Text = "Just Testing";

                int inUsecount = isDeletable_goal(Convert.ToInt32(commandArgument));
                clsData oData = new clsData();
                string strQuerry = "";
                if (inUsecount == 0)
                {


                    try
                    {

                        clsSession sess = (clsSession)Session["UserSession"];
                        strQuerry = "DELETE FROM Goal WHERE  GoalId = '" + commandArgument + "' ";
                        oData.Execute2(strQuerry);

                        pnlMessageGoal.CssClass = "pnlMessage_green";
                        lblMessageGoal.Text = "Deleted Successfully";
                        pnlMessageGoal.Visible = true;

                        grdViewGoals.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        pnlMessageGoal.CssClass = "pnlMessage_red";
                        lblMessageGoal.Text = Ex.ToString();
                        pnlMessageGoal.Visible = true;
                        //throw Ex;
                    }
                }
                else
                {
                    //pnlMessageGoal.CssClass = "pnlMessage_red";
                    //lblMessageGoal.Text = (inUsecount == 1) ? "Can't Delete since it is currently mapped to a lesson plan." : "Can't Delete since it is currently mapped to " + inUsecount + " lesson plans";
                    //pnlMessageGoal.Visible = true;
                    strQuerry = "UPDATE Goal SET ActiveInd = 'D' WHERE  GoalId=" + commandArgument;
                    oData.Execute2(strQuerry);
                    pnlMessage.CssClass = "pnlMessage_green";
                    lblMessage.Text = "Deleted Successfully";
                    pnlMessage.Visible = true;
                    grdViewGoals.DataBind();
                }
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel2, UpdatePanel2.GetType(), "Any Name you Like", "$(document).ready(function(){ setInterval(function(){ $('.pnlMessage_green,.pnlMessage_red').slideUp('slow',function(){$(this).remove();}); }, 3000); });", true);

                break;

            case "EditGoal":

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdViewGoals.Rows[index];

                hdnEditGoal.Value = ((HiddenField)row.FindControl("hdnGoalId")).Value;
                hdnEditGoalName.Value = ((Label)row.FindControl("lblGoalName")).Text;
                lblGoalCode_popup.Text = ((Label)row.FindControl("lblGoalCode")).Text;

                txtEditGoal.Text = ((Label)row.FindControl("lblGoalName")).Text;
                pnlOverlay2.Visible = true;
                pnlGoalEditBox.Visible = true;
                lblEditGoal.Text = "";
                break;


            default:
                break;

        }


    }
    protected void btnAddGoal_Click(object sender, EventArgs e)
    {
        if (txtAddGoal.Text.Trim() != "")
        {
            if (isDuplicateGoal(txtAddGoal.Text.Trim()))
            {
                pnlMessageGoal.CssClass = "pnlMessage_green";
                lblMessageGoal.Text = "Just Testing";

                string strQuerry = "";
                try
                {
                    clsData oData = new clsData();
                    clsSession sess = (clsSession)Session["UserSession"];
                    strQuerry = "INSERT INTO Goal(SchoolId, GoalTypeId, GoalLevelId, GoalName, GoalCode, GoalDesc, GoalPic, ActiveInd, CreatedBy, CreatedOn, isDynamic) VALUES(" + sess.SchoolId + ",1,1,'" + clsGeneral.convertQuotes(txtAddGoal.Text) + "','" + clsGeneral.convertQuotes(txtAddGoal.Text) + "','AssessmentGoal','~/StudentBinder/img/CustomDefault.png','A'," + sess.LoginId + ",GETDATE(), 1)";
                    oData.Execute2(strQuerry);

                    pnlMessageGoal.CssClass = "pnlMessage_green";
                    lblMessageGoal.Text = "Added Successfully";
                    pnlMessageGoal.Visible = true;

                    txtAddGoal.Text = "";

                    grdViewGoals.DataBind();
                }
                catch (Exception Ex)
                {
                    pnlMessageGoal.CssClass = "pnlMessage_red";
                    lblMessageGoal.Text = Ex.ToString();
                    pnlMessageGoal.Visible = true;
                    //throw Ex;
                }
            }
            else
            {
                if (isDuplicateDeltdGoal(txtAddGoal.Text.Trim()))
                {
                    pnlMessageGoal.CssClass = "pnlMessage_green";
                    lblMessageGoal.Text = "Just Testing";

                    string strQuerry = "";
                    try
                    {
                        Guid g;
                        g = Guid.NewGuid();
                        string tp_code = g + "_" + txtAddGoal.Text;
                        clsData oData = new clsData();
                        clsSession sess = (clsSession)Session["UserSession"];
                        strQuerry = "INSERT INTO Goal(SchoolId, GoalTypeId, GoalLevelId, GoalName, GoalCode, GoalDesc, GoalPic, ActiveInd, CreatedBy, CreatedOn, isDynamic) VALUES(" + sess.SchoolId + ",1,1,'" + clsGeneral.convertQuotes(tp_code) + "','" + clsGeneral.convertQuotes(txtAddGoal.Text) + "','AssessmentGoal','~/StudentBinder/img/CustomDefault.png','A'," + sess.LoginId + ",GETDATE(), 1)";
                        oData.Execute2(strQuerry);

                        pnlMessageGoal.CssClass = "pnlMessage_green";
                        lblMessageGoal.Text = "Added Successfully";
                        pnlMessageGoal.Visible = true;

                        txtAddGoal.Text = "";

                        grdViewGoals.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        pnlMessageGoal.CssClass = "pnlMessage_red";
                        lblMessageGoal.Text = Ex.ToString();
                        pnlMessageGoal.Visible = true;
                        //throw Ex;
                    }
                }
                else
                {
                    pnlMessageGoal.CssClass = "pnlMessage_red";
                    lblMessageGoal.Text = "Goal Name already exist";
                    pnlMessageGoal.Visible = true;
                }
            }
        }
        else
        {
            pnlMessageGoal.CssClass = "pnlMessage_red";
            lblMessageGoal.Text = "Please enter a Goal Name";
            pnlMessageGoal.Visible = true;
        }

        // ScriptManager.RegisterClientScriptBlock(UpdatePanel2, UpdatePanel2.GetType(), "Any Name you Like", "$(document).ready(function(){ setInterval(function(){ $('.pnlMessage_green,.pnlMessage_red').slideUp('slow',function(){$(this).remove();}); }, 3000); });", true);

    }

    protected void btnAddAssessment_Click(object sender, EventArgs e)
    {
        if (txtAddAssessment.Text.Trim() != "")
        {
            string strQuerry = "";
            clsData oData = new clsData();
            if (isDuplicateAssessment(txtAddAssessment.Text.Trim()))
            {
                pnlMessageAssessment.CssClass = "pnlMessage_green";
                lblMessageAssessment.Text = "Just Testing";


                try
                {
                    
                    clsSession sess = (clsSession)Session["UserSession"];
                    strQuerry = "INSERT INTO Lookup(SchoolId, LookupType,LookupName,LookupCode,LookupDesc,ActiveInd,CreatedBy, CreateOn, isDynamic) VALUES(" + sess.SchoolId + ",'Assessment Name','" + clsGeneral.convertQuotes(txtAddAssessment.Text) + "','" + clsGeneral.convertQuotes(txtAddAssessment.Text) + "','" + clsGeneral.convertQuotes(txtAddAssessDesc.Text) + "','A'," + sess.LoginId + ",GETDATE(), 1)";
                    oData.Execute2(strQuerry);

                    pnlMessageAssessment.CssClass = "pnlMessage_green";
                    lblMessageAssessment.Text = "Added Successfully";
                    pnlMessageAssessment.Visible = true;

                    txtAddAssessment.Text = "";
                    txtAddAssessDesc.Text = "";

                    grdAssessment.DataBind();
                }
                catch (Exception Ex)
                {
                    pnlMessageAssessment.CssClass = "pnlMessage_red";
                    lblMessageAssessment.Text = Ex.ToString();
                    pnlMessageAssessment.Visible = true;
                    //throw Ex;
                }
            }
            else
            {
                if (isDuplicateDeltdAssessment(txtAddAssessment.Text.Trim()))
                {
                   
                    pnlMessageAssessment.CssClass = "pnlMessage_green";
                    lblMessageAssessment.Text = "Just Testing";


                    try
                    {
                        Guid g;
                        g = Guid.NewGuid();
                        string tp_code = g + "_" + txtAddAssessment.Text;
                        clsSession sess = (clsSession)Session["UserSession"];
                        strQuerry = "INSERT INTO Lookup(SchoolId, LookupType,LookupName,LookupCode,LookupDesc,ActiveInd,CreatedBy, CreateOn, isDynamic) VALUES(" + sess.SchoolId + ",'Assessment Name','" + clsGeneral.convertQuotes(txtAddAssessment.Text) + "','" + clsGeneral.convertQuotes(tp_code) + "','" + clsGeneral.convertQuotes(txtAddAssessDesc.Text) + "','A'," + sess.LoginId + ",GETDATE(), 1)";
                        oData.Execute2(strQuerry);

                        pnlMessageAssessment.CssClass = "pnlMessage_green";
                        lblMessageAssessment.Text = "Added Successfully";
                        pnlMessageAssessment.Visible = true;

                        txtAddAssessment.Text = "";
                        txtAddAssessDesc.Text = "";

                        grdAssessment.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        pnlMessageAssessment.CssClass = "pnlMessage_red";
                        lblMessageAssessment.Text = Ex.ToString();
                        pnlMessageAssessment.Visible = true;
                        //throw Ex;
                    }
                }
                else
                {
                    pnlMessageAssessment.CssClass = "pnlMessage_red";
                    lblMessageAssessment.Text = "Assessment Name already exist";
                    pnlMessageAssessment.Visible = true;
                }
            }
        }
        else
        {
            
                pnlMessageAssessment.CssClass = "pnlMessage_red";
                lblMessageAssessment.Text = "Please enter a Assessment Name";
                pnlMessageAssessment.Visible = true;
            
        }

        // ScriptManager.RegisterClientScriptBlock(UpdatePanel2, UpdatePanel2.GetType(), "Any Name you Like", "$(document).ready(function(){ setInterval(function(){ $('.pnlMessage_green,.pnlMessage_red').slideUp('slow',function(){$(this).remove();}); }, 3000); });", true);

    }
    protected void btnEditTeachingProc_Click(object sender, EventArgs e)
    {
        if (txtEditTeachingProc.Text.Trim() != "")
        {
            lblEditTeachingProcMessage.Text = "";
            string strQuerry = "";
            try
            {
                clsData oData = new clsData();
                clsSession sess = (clsSession)Session["UserSession"];
                int lookUpId = Convert.ToInt32(hdnTeachProcId.Value);
                string teachProcName = hdnTeachProcName.Value;
                if (teachProcName != txtEditTeachingProc.Text.Trim())
                {
                    if (isDuplicateUpdateTP(txtEditTeachingProc.Text.Trim()))
                    {
                        strQuerry = "UPDATE LookUp SET LookupCode='" + clsGeneral.convertQuotes(txtEditTeachingProc.Text.Trim()) + "',LookupDesc='" + clsGeneral.convertQuotes(txtEditTeachingProc.Text.Trim()) + "'  WHERE  LookupId = " + lookUpId + " ";
                        oData.Execute2(strQuerry);


                        pnlMessage.CssClass = "pnlMessage_green";
                        lblMessage.Text = "Updated Successfully";
                        pnlMessage.Visible = true;

                        pnlOverlay.Visible = false;
                        pnlTeachingProcEdit.Visible = false;

                        grdViewTeachingProc.DataBind();
                    }
                    else
                    {
                        lblEditTeachingProcMessage.Text = "Name already exist. Please select different name.";
                    }

                }
                else
                {
                    pnlMessage.CssClass = "pnlMessage_green";
                    lblMessage.Text = "Updated Successfully";
                    pnlMessage.Visible = true;

                    pnlOverlay.Visible = false;
                    pnlTeachingProcEdit.Visible = false;
                }
            }
            catch (Exception Ex)
            {

                lblEditTeachingProcMessage.Text = "Updation failed. Please try again...";
                //throw Ex;
            }
        }
        else
        {
            lblEditTeachingProcMessage.Text = "Plase enter a valid name";
        }
    }
    protected void btnEditAssessment_Click(object sender, EventArgs e)
    {
        if (txtEditAssessment.Text.Trim() != "")
        {
            lblEditAssessmentMessage.Text = "";
            string strQuerry = "";
            try
            {
                clsData oData = new clsData();
                clsSession sess = (clsSession)Session["UserSession"];
                int lookUpId = Convert.ToInt32(hdnAssessmentId.Value);
                string AssessmentName = hdnAssessmentName.Value;
                string AssessmentDesc = hdnAssessmentDesc.Value;
                if (AssessmentName != txtEditAssessment.Text.Trim())
                {
                    if (isDuplicateAssessmentOnUpdate(txtEditAssessment.Text.Trim()))
                    {
                        strQuerry = "UPDATE LookUp SET LookupName='" + clsGeneral.convertQuotes(txtEditAssessment.Text.Trim()) + "',LookupDesc='" + clsGeneral.convertQuotes(txtEditAssDesc.Text.Trim()) + "'  WHERE  LookupId = " + lookUpId + " ";
                        oData.Execute2(strQuerry);


                        pnlMessageAssessment.CssClass = "pnlMessage_green";
                        lblMessageAssessment.Text = "Updated Successfully";
                        pnlMessageAssessment.Visible = true;

                        pnlOverlay3.Visible = false;
                        pnlAssessmentEditBox.Visible = false;

                        grdAssessment.DataBind();
                    }
                    else
                    {
                        lblEditAssessmentMessage.Text = "Name already exist. Please select different name.";
                    }

                }
                else
                {
                    strQuerry = "UPDATE LookUp SET LookupDesc='" + clsGeneral.convertQuotes(txtEditAssDesc.Text.Trim()) + "'  WHERE  LookupId = " + lookUpId + " ";
                    oData.Execute2(strQuerry);
                    pnlMessageAssessment.CssClass = "pnlMessage_green";
                    lblMessageAssessment.Text = "Updated Successfully";
                    pnlMessageAssessment.Visible = true;
                    grdAssessment.DataBind();
                    pnlOverlay3.Visible = false;
                    pnlAssessmentEditBox.Visible = false;
                }
            }
            catch (Exception Ex)
            {

                lblEditAssessmentMessage.Text = "Updation failed. Please try again...";
                //throw Ex;
            }
        }
        else
        {
            lblEditAssessmentMessage.Text = "Plase enter a valid name";
        }
    }
    protected void imbBtnClose_Click(object sender, ImageClickEventArgs e)
    {
        pnlOverlay.Visible = false;
        pnlTeachingProcEdit.Visible = false;
    }
    protected void imbBtnClose1_Click(object sender, ImageClickEventArgs e)
    {
        pnlOverlay2.Visible = false;
        pnlGoalEditBox.Visible = false;
    }
    protected void imbBtnClose2_Click(object sender, ImageClickEventArgs e)
    {
        pnlOverlay3.Visible = false;
        pnlAssessmentEditBox.Visible = false;
    }
    protected void Assessment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataClass objDataClass = new DataClass();
        try
        {
            string selSchool = "SELECT LookupName,LookupCode,LookupDesc from Lookup where LookupType='Assessment Name'  ";

            DataTable gridLesson = objDataClass.fillData(selSchool);
            //GV_Student.DataSource = gridLesson;
            //GV_Student.DataBind();
            //linkActive.ForeColor = System.Drawing.Color.Red;
            //lnkInactive.ForeColor = System.Drawing.Color.Blue;
        }
        catch (Exception Ex)
        {
            //tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try After Sometime!!!!!!!!!");
            //throw Ex;
        }
        if (e.CommandName == "EditAssessment")
        {
            Session["EData"] = e.CommandArgument;
            Response.Redirect("AddSchool.aspx");
        }
        if (e.CommandName == "DeleteAssessment")
        {
            string delStud = "UPDATE  School SET ActiveInd = 'D' WHERE  SchoolId=" + e.CommandArgument;
            Boolean index = Convert.ToBoolean(objDataClass.ExecuteNonQuery(delStud));

            if (index == true)
            {
                //tdMsg.InnerHtml = clsGeneral.sucessMsg("School Deleted Successfully");
                //FillGrid();
            }


        }
    }
    public void FillGrid()
    {
        DataClass objDataClass = new DataClass();
        try
        {
            string selSchool = "SELECT LookupId,LookupName,LookupCode,LookupDesc,isDynamic from Lookup where LookupType='Assessment Name' and ActiveInd='A'  ";

            DataTable gridLesson = objDataClass.fillData(selSchool);
            grdAssessment.DataSource = gridLesson;
            grdAssessment.DataBind();
            //linkActive.ForeColor = System.Drawing.Color.Red;
            //lnkInactive.ForeColor = System.Drawing.Color.Blue;
        }
        catch (Exception Ex)
        {
            //tdMsg.InnerHtml = clsGeneral.failedMsg("Please Try After Sometime!!!!!!!!!");
            //throw Ex;
        }

    }
    protected void btnEditGoal_Click(object sender, EventArgs e)
    {
        if (txtEditGoal.Text.Trim() != "")
        {
            lblEditGoal.Text = "";
            string strQuerry = "";
            try
            {
                clsData oData = new clsData();
                clsSession sess = (clsSession)Session["UserSession"];
                int goalId = Convert.ToInt32(hdnEditGoal.Value);
                string goalName = hdnEditGoalName.Value;

                if (goalName != txtEditGoal.Text.Trim())
                {

                    if (isDuplicateGoalOnUpdate(txtEditGoal.Text.Trim()))
                    {
                        strQuerry = "UPDATE Goal SET GoalCode = '" + clsGeneral.convertQuotes(txtEditGoal.Text) + "'  WHERE  GoalId = " + goalId + " ";
                        oData.Execute2(strQuerry);


                        pnlMessageGoal.CssClass = "pnlMessage_green";
                        lblMessageGoal.Text = "Updated Successfully";
                        pnlMessageGoal.Visible = true;

                        pnlOverlay2.Visible = false;
                        pnlGoalEditBox.Visible = false;

                        grdViewGoals.DataBind();
                    }
                    else
                    {
                        lblEditGoal.Text = "Name already exist. Please select different name.";
                    }
                }
                else
                {
                    pnlMessageGoal.CssClass = "pnlMessage_green";
                    lblMessageGoal.Text = "Updated Successfully";
                    pnlMessageGoal.Visible = true;

                    pnlOverlay2.Visible = false;
                    pnlGoalEditBox.Visible = false;
                }

            }
            catch (Exception Ex)
            {

                lblEditGoal.Text = "Updation failed. Please try again...";
                //throw Ex;
            }
        }
        else
        {
            lblEditGoal.Text = "Plase enter a valid name";
        }
    }


    protected void grdAssessment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string commandArgument = e.CommandArgument.ToString();
        string commandName = e.CommandName.ToString();

        switch (commandName)
        {
            case "DeleteAssessment":
                pnlMessageAssessment.CssClass = "pnlMessage_green";
                lblMessageAssessment.Text = "Just Testing";

                int inUsecount = isDeletable_Assessment(commandArgument);
                string strQuerry = "";
                clsData oData = new clsData();
                if (inUsecount == 0)
                {


                    try
                    {

                        clsSession sess = (clsSession)Session["UserSession"];
                        strQuerry = "DELETE FROM Lookup WHERE  LookupId = '" + commandArgument + "' ";
                        oData.Execute2(strQuerry);

                        pnlMessageAssessment.CssClass = "pnlMessage_green";
                        lblMessageAssessment.Text = "Deleted Successfully";
                        pnlMessageAssessment.Visible = true;

                        grdAssessment.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        pnlMessageAssessment.CssClass = "pnlMessage_red";
                        lblMessageAssessment.Text = Ex.ToString();
                        pnlMessageAssessment.Visible = true;
                        //throw Ex;
                    }
                }
                else
                {
                    //pnlMessageAssessment.CssClass = "pnlMessage_red";
                    //lblMessageAssessment.Text = (inUsecount == 1) ? "Can't Delete since it is currently mapped to a lesson plan." : "Can't Delete since it is currently mapped to " + inUsecount + " lesson plans";
                    //pnlMessageAssessment.Visible = true;
                    strQuerry = "UPDATE  Lookup SET ActiveInd = 'D' WHERE  LookupId=" + commandArgument;
                    oData.Execute2(strQuerry);
                    pnlMessageAssessment.CssClass = "pnlMessage_green";
                    lblMessageAssessment.Text = "Deleted Successfully";
                    pnlMessageAssessment.Visible = true;
                    grdAssessment.DataBind();

                }
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel2, UpdatePanel2.GetType(), "Any Name you Like", "$(document).ready(function(){ setInterval(function(){ $('.pnlMessage_green,.pnlMessage_red').slideUp('slow',function(){$(this).remove();}); }, 3000); });", true);

                break;

            case "EditAssessment":

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdAssessment.Rows[index];

                hdnAssessmentId.Value = ((HiddenField)row.FindControl("hdnLookupId")).Value;
                hdnAssessmentName.Value = ((Label)row.FindControl("lblAsmtName")).Text;
                hdnAssessmentDesc.Value = ((Label)row.FindControl("lblAsmtCode")).Text;
                // lblAssessmentCode_popup.Text = ((Label)row.FindControl("lblAssessmentCode")).Text;

                txtEditAssessment.Text = ((Label)row.FindControl("lblAsmtName")).Text;
                txtEditAssDesc.Text = ((Label)row.FindControl("lblAsmtCode")).Text;
                pnlOverlay3.Visible = true;
                pnlAssessmentEditBox.Visible = true;
                lblEditAssessmentMessage.Text = "";
                break;


            default:
                break;

        }
    }


}