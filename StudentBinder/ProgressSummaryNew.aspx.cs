using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class StudentBinder_ProgressSummaryNew : System.Web.UI.Page
{
    clsSession sess = null;
    clsData ObjData = null;
    System.Data.DataTable Dt = null;
    clsData oData_ov = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadLessonPlan();
        }
    }

    private void LoadLessonPlan()
    {
        ObjData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            try
            {
                DataTable dtLP = new DataTable();
                dtLP.Columns.Add("Id", typeof(string));
                dtLP.Columns.Add("Name", typeof(string));

                string StatusCheck = "";
                foreach (ListItem item in chkStatus.Items)
                {
                    if (item.Selected == true)
                    {
                        if (item.Text == "Active")
                        {
                            StatusCheck += " LookupName='Approved' ";
                        }
                        else if (item.Text == "Maintenance")
                        {
                            if (StatusCheck != "")
                            {
                                StatusCheck += " OR ";
                            }
                            StatusCheck += " LookupName='Maintenance' ";

                        }
                        else if (item.Text == "Inactive")
                        {
                            if (StatusCheck != "")
                            {
                                StatusCheck += " OR ";
                            }
                            StatusCheck += " LookupName='Inactive' ";
                        }
                    }
                }

                DataTable DTLesson = ObjData.ReturnDataTable("SELECT *,(SELECT TOP 1 DSTemplateName +' ( '+CASE WHEN LookupName='Approved' THEN 'Active' ELSE LookupName END+' )' " +
                    "FROM DSTempHdr INNER JOIN LookUp ON DSTempHdr.StatusId=LookUp.LookupId WHERE DSTempHdr.LessonPlanId=LSN.LessonPlanId AND DSTempHdr.StudentId=" + sess.StudentId + " AND " +
                    "( " + StatusCheck + " ) ORDER BY DSTempHdrId DESC) AS LessonPlanName FROM (SELECT DISTINCT DTmp.LessonPlanId,DTmp.LessonOrder FROM DSTempHdr DTmp " +
                    "INNER JOIN LookUp ON DTmp.StatusId=LookUp.LookupId WHERE StudentId=" + sess.StudentId + " AND ( " + StatusCheck + " )) LSN ORDER BY  LessonOrder", false);

                if (DTLesson != null)
                {
                    if (DTLesson.Rows.Count > 0)
                    {
                        foreach (DataRow drLessn in DTLesson.Rows)
                        {
                            DataRow drr = dtLP.NewRow();
                            drr["Id"] = drLessn.ItemArray[0];
                            drr["Name"] = drLessn.ItemArray[2];
                            dtLP.Rows.Add(drr);
                        }
                    }
                }
                else
                {
                    td1.Visible = true;
                    td1.InnerHtml = clsGeneral.failedMsg("No Lesson Found.");
                }
                ddlLessonplan.DataSource = dtLP;
                ddlLessonplan.DataTextField = "Name";
                ddlLessonplan.DataValueField = "Id";
                ddlLessonplan.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    protected void chkStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadLessonPlan();
    }

    private bool Validation()
    {
        bool result = true;
        int count = 0;
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
        {
            if (item.Selected == true)
            {
                count++;
            }
        }
        if (txtRepStart.Text == "")
        {
            result = false;
            tdMsg.Visible = true;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter Start Date");
            return result;
        }
        else if (txtrepEdate.Text == "")
        {
            result = false;
            tdMsg.Visible = true;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter End Date");
            return result;
        }
        else if (count == 0)
        {
            result = false;
            tdMsg.Visible = true;
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select lesson plan");
        }

        else if (txtRepStart.Text != "")
        {
            DateTime dtst = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > DateTime.Now)
            {
                result = false;
                tdMsg.Visible = true;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid Start date");
            }
            return result;
        }
        else if (txtrepEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            dtst = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > DateTime.Now)
            {
                result = false;
                tdMsg.Visible = true;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Invalid End date");
            }
            return result;
        }
        else if (txtRepStart.Text != "" && txtrepEdate.Text != "")
        {
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim().Replace("-", "/"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtst > dted)
            {
                result = false;
                tdMsg.Visible = true;
                tdMsg.InnerHtml = clsGeneral.warningMsg("Start date is must before the End date");
            }
            return result;
        }

        return result;

    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ClearAll();        
        LoadLessonPlan();
        btnExport.Visible = false;
        dlLesson.Visible = false;
    }

    public void ClearAll()
    {
        txtRepStart.Text = string.Empty;
        txtrepEdate.Text = string.Empty;
        chkStatus.Visible = true;
        chkStatus.Items[0].Selected = true;
        chkStatus.Items[1].Selected = true;
        chkStatus.Items[2].Selected = false;
        tdMsg.Visible = false;
        td1.Visible = false;
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
        {
            item.Selected = false;
            
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validation() == true)
            {
                dlLesson.Visible = true;
                LoadRptLesson();
                chkStatus.Visible = false;
                btnExport.Visible = true;
                tdMsg.Visible = false;
                td1.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public void LoadRptLesson()
    {
        sess = (clsSession)Session["UserSession"];
        oData_ov = new clsData();
        string LPStatus = "";
        Dt = new System.Data.DataTable();
        ObjData = new clsData();

        foreach (ListItem item in chkStatus.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "Active")
                {
                    LPStatus += " 'Approved' ";
                }
                else if (item.Text == "Maintenance")
                {
                    if (LPStatus != "")
                    {
                        LPStatus += ",";
                    }
                    LPStatus += " 'Maintenance' ";
                }
                else if (item.Text == "Inactive")
                {
                    if (LPStatus != "")
                    {
                        LPStatus += ",";
                    }
                    LPStatus += " 'Inactive' ";
                }
            }
        }

        string LessonId = "";
        foreach (System.Web.UI.WebControls.ListItem item in ddlLessonplan.Items)
        {
            if (item.Selected == true)
            {
                LessonId += item.Value + ",";
            }
        }
        LessonId = LessonId.Substring(0, (LessonId.Length - 1));

        string sqlStr = "SELECT *,(SELECT TOP 1 DSTemplateName FROM DSTempHdr DH INNER JOIN LookUp L ON DH.StatusId=L.LookupId WHERE DH.LessonPlanId=LSN.LessonPlanId AND " +
            "DH.StudentId=" + sess.StudentId + " AND L.LookupName IN (" + LPStatus + ") ORDER BY DH.DSTempHdrId DESC) AS LessonName " +
            "FROM (SELECT LessonPlanId,LessonOrder FROM DSTempHdr DS INNER JOIN LookUp LU ON DS.StatusId=LU.LookupId WHERE DS.StudentId=" + sess.StudentId + " AND DS.SchoolId=" + sess.SchoolId + " AND " +
            "LU.LookupName IN (" + LPStatus + ") AND DS.LessonPlanId IN ( " + LessonId + ") GROUP BY DS.LessonPlanId,DS.LessonOrder) LSN ORDER BY LessonOrder";
        DataSet ds = oData_ov.ReturnDataSet(sqlStr, false);
        if (ds != null)
        {
            dlLesson.DataSource = ds;
            dlLesson.DataBind();
        }
        else
        {
            td1.Visible = true;
            td1.InnerHtml = clsGeneral.failedMsg("No Lesson Found.");
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.xls";
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ProgressSummaryReport.xls"));
        StringWriter stringwriter = new StringWriter();
        HtmlTextWriter htmlwrite = new HtmlTextWriter(stringwriter);
        dlLesson.RenderControl(htmlwrite);
        Response.Write(stringwriter.ToString());
        Response.End();
    }

    protected void dlLesson_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjData = new clsData();
            sess = (clsSession)Session["UserSession"];
            DataSet ds = new DataSet();
            oData_ov = new clsData();
            DateTime dtst = new DateTime();
            DateTime dted = new DateTime();
            dtst = DateTime.ParseExact(txtRepStart.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            dted = DateTime.ParseExact(txtrepEdate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string StartDate = dtst.ToString("yyyy-MM-dd");
            string enddate = dted.ToString("yyyy-MM-dd");
            GridView gVSession = (e.Item.FindControl("gVSession") as GridView);
            string LessonId = (e.Item.FindControl("LessonId") as Label).Text;

            string LPStatus = "";
            foreach (ListItem item in chkStatus.Items)
            {
                if (item.Selected == true)
                {
                    if (item.Text == "Active")
                    {
                        LPStatus += "Approved";
                    }
                    else if (item.Text == "Maintenance")
                    {
                        if (LPStatus != "")
                        {
                            LPStatus += ",";
                        }
                        LPStatus += "Maintenance";
                    }
                    else if (item.Text == "Inactive")
                    {
                        if (LPStatus != "")
                        {
                            LPStatus += ",";
                        }
                        LPStatus += "Inactive";
                    }
                }
            }
            //Dt = ObjData.Execute_PSR_Data(LessonId, LPStatus, StartDate, enddate, Convert.ToInt32(sess.StudentId));
            gVSession.DataSource = Dt;
            gVSession.DataBind();

        }
    }
}