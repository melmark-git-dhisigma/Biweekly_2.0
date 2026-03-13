using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Linq;

public partial class StudentBinder_TimeCycleChart : System.Web.UI.Page
{
    clsData objData = null;
    static clsData ObjData = null;
    clsSession sess = null;
    DataTable Dt = null;
    ArrayList arraylist1 = new ArrayList();
    ArrayList arraylist2 = new ArrayList();
    ClsTemplateSession ObjTempSess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            sess = (clsSession)Session["UserSession"];
            if (sess == null)
            {
                Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
            }
            else
            {
                
            }
            foreach (ListItem item in rblOption.Items)
            {
                item.Attributes["onclick"] = "updateTimeFields(this);";
            }
            if (!IsPostBack)
            {

                PopulateTimeDropdown(ddlStartTime);
                PopulateTimeDropdown(ddlEndTime);
                ddlStartTime.SelectedIndex = 14;
                ddlEndTime.SelectedIndex = 46;
                rblOption.SelectedIndex = 0;
                FILLBEHAVIOR();
                txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtStartDate.Text = DateTime.Today.AddDays(-91).ToString("yyyy-MM-dd");

            }
           
        }
        catch (Exception Ex)
        {
            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(Ex.ToString());
            throw Ex;
        }
    }
    private void PopulateTimeDropdown(DropDownList ddl)
    {
        DateTime start = DateTime.Today.AddHours(0);
        DateTime end = DateTime.Today.AddHours(23).AddMinutes(30);

        while (start <= end)
        {
            string displayTime = start.ToString("h:mm tt");  // e.g., 7:00 AM
            ddl.Items.Add(new ListItem(displayTime, displayTime));
            start = start.AddMinutes(30);
        }
    }
    private void FILLBEHAVIOR()
    {
        string strQuery = "";
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        if (chkActive.Checked == true && chkInactive.Checked == false)
        {
            strQuery = "SELECT  Bdet.MeasurementId  as Id,Bdet.Behaviour as Name " +
                                "FROM BehaviourDetails Bdet LEFT JOIN BehaviourLPRel BlpRel on Bdet.MeasurementId = BlpRel.MeasurementId " +
                                 "WHERE Bdet.StudentId = " + sess.StudentId + " AND (Bdet.ActiveInd = 'A') order by Bdet.CreatedOn";

        }
        else if (chkActive.Checked == false && chkInactive.Checked == true)
        {
            strQuery = "SELECT  Bdet.MeasurementId  as Id,Bdet.Behaviour as Name " +
                               "FROM BehaviourDetails Bdet LEFT JOIN BehaviourLPRel BlpRel on Bdet.MeasurementId = BlpRel.MeasurementId " +
                                "WHERE Bdet.StudentId = " + sess.StudentId + " AND (Bdet.ActiveInd = 'N') order by Bdet.CreatedOn";

        }
        else if (chkActive.Checked == true && chkInactive.Checked == true)
        {
            strQuery = "SELECT  Bdet.MeasurementId  as Id,Bdet.Behaviour as Name " +
                              "FROM BehaviourDetails Bdet LEFT JOIN BehaviourLPRel BlpRel on Bdet.MeasurementId = BlpRel.MeasurementId " +
                               "WHERE Bdet.StudentId = " + sess.StudentId + " AND (Bdet.ActiveInd = 'N' OR Bdet.ActiveInd = 'A') order by Bdet.CreatedOn";

        }
        else
        {
            strQuery = "SELECT  Bdet.MeasurementId  as Id,Bdet.Behaviour as Name " +
                              "FROM BehaviourDetails Bdet LEFT JOIN BehaviourLPRel BlpRel on Bdet.MeasurementId = BlpRel.MeasurementId " +
                               "WHERE Bdet.StudentId = " + sess.StudentId + " AND (Bdet.ActiveInd = 'N' AND Bdet.ActiveInd = 'A') order by Bdet.CreatedOn";

        }
       
        Dt = objData.ReturnDataTable(strQuery, false);
        behDropdownList.InnerHtml = "";
        if (Dt != null && Dt.Rows.Count > 0)
        {
            behDropdownList.InnerHtml += "<div><input type='checkbox' id='chkAll' onchange='toggleAll(this)' /> <label for='chkAll'>All behaviors</label></div>";

            foreach (DataRow row in Dt.Rows)
            {
                string id = row["Id"].ToString();
                string name = row["Name"].ToString();

                behDropdownList.InnerHtml += string.Format(
                    "<div><input type='checkbox' name='behavior' id='chk{0}' value='{0}' onchange='updateAllCheckbox()' /> <label for='chk{0}'>{1}</label></div>",
                    id, name);
            }
        }
        else
        {
            behDropdownList.InnerHtml = "<div class='no-data'>No data available</div>";
        }
    }
    protected void chkActive_CheckedChanged(object sender, EventArgs e)
    {
        string script = "updateTimeFields();";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "updatetime", script, true);
        FILLBEHAVIOR();
    }
    protected void btnsubmit_Click(object sender, EventArgs e) 
    {
        bool stat = validation();
        if (stat == true)
        {
           ViewState["Starttime"]= ddlStartTime.SelectedIndex;
            ViewState["Endtime"] = ddlEndTime.SelectedIndex;
            string script = "showOverlay();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showOverlay", script, true);
            string[] selectedIds = Request.Form.GetValues("behavior");
            bool isAllChecked = Request.Form["chkAll"] == "on";
            DataTable selectedTable = new DataTable();
            selectedTable.Columns.Add("Id", typeof(string));
            selectedTable.Columns.Add("Name", typeof(string));
            if (selectedIds != null && selectedIds.Length > 0)
            {
                string selectedIdsstr = string.Join(",", selectedIds);
                string strQuery = "";
                sess = (clsSession)Session["UserSession"];
                objData = new clsData();
                DataTable dt = new DataTable();
                    strQuery = "SELECT  MeasurementId  as Id,Behaviour as Name " +
                                        "FROM BehaviourDetails Where StudentId = " + sess.StudentId + " AND MeasurementId in (" + selectedIdsstr+")";
                    dt = objData.ReturnDataTable(strQuery, false);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (string id in selectedIds)
                        {
                            // Look up the corresponding name in your original DataTable
                            DataRow[] rows = dt.Select("Id = '" + id + "'");
                            if (rows.Length > 0)
                            {
                                string name = rows[0]["Name"].ToString();

                                // Add new row to selected DataTable
                                DataRow newRow = selectedTable.NewRow();
                                newRow["Id"] = id;
                                newRow["Name"] = name;
                                selectedTable.Rows.Add(newRow);

                            }
                        }
                        ddlLessonplan.DataSource = selectedTable;
                        ddlLessonplan.DataTextField = "Name";
                        ddlLessonplan.DataValueField = "Id";
                        ddlLessonplan.DataBind();
                        Loaddata(ddlLessonplan.SelectedValue.ToString(),false);
                    }
                    FILLBEHAVIOR();
                    behDropdownList.InnerHtml = "";

                    if (Dt != null && Dt.Rows.Count > 0)
                    {
                        bool allSelected = Dt.Rows.Count == selectedIds.Length;
                        
                        string allChecked = allSelected ? "checked" : "";
    //behDropdownList.InnerHtml += "<div><input type='checkbox' id='chkAll' onchange='toggleAll(this)' {allChecked} /> <label for='chkAll'>All behaviors</label></div>";
                        behDropdownList.InnerHtml += "<div><input type='checkbox' id='chkAll' onchange='toggleAll(this)' "+allChecked+" /> <label for='chkAll'>All behaviors</label></div>";

                        foreach (DataRow row in Dt.Rows)
                        {
                            string id = row["Id"].ToString();
                            string name = row["Name"].ToString();

                            // Check if ID exists in selectedIds array
                            string isChecked = selectedIds.Contains(id) ? "checked" : "";

                            behDropdownList.InnerHtml += string.Format(
                                "<div><input type='checkbox' name='behavior' id='chk{0}' value='{0}' {2} onchange='updateAllCheckbox()' /> <label for='chk{0}'>{1}</label></div>",
                                id, name, isChecked
                            );
                        }
                    }
                    string scripta = "updateTimeFields();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "updateTimeFields", scripta, true);
                    
            }


           

            string script2 = "hideOverlay();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideOverlay", script2, true);

        }
    }
    protected bool validation()
    {
       

        if (string.IsNullOrWhiteSpace(txtStartDate.Text) || string.IsNullOrWhiteSpace(txtEndDate.Text))
        {
            lblerror.Text = "Start Date and End Date are required.";
            return false;
        }

        DateTime startDate, endDate;
        if (!DateTime.TryParse(txtStartDate.Text, out startDate) ||
            !DateTime.TryParse(txtEndDate.Text, out endDate))
        {
            lblerror.Text = "Please enter valid dates in correct format (e.g., yyyy-MM-dd).";
            return false;
        }

        if (startDate > endDate)
        {
            lblerror.Text = "Start Date cannot be greater than End Date.";
            return false;
        }

        if (endDate > DateTime.Today)
        {
            lblerror.Text = "End Date cannot be greater than today's date.";
            return false;
        }
        if (startDate > DateTime.Today)
        {
            lblerror.Text = "Start Date cannot be greater than today's date.";
            return false;
        }
        if (chk24Hours.Checked == false)
        {
            if (string.IsNullOrEmpty(ddlStartTime.SelectedValue) || string.IsNullOrEmpty(ddlEndTime.SelectedValue))
            {
                lblerror.Text = "Please select both Start Time and End Time.";
                return false;
            }

            DateTime startTime = DateTime.Parse(ddlStartTime.SelectedValue);
            DateTime endTime = DateTime.Parse(ddlEndTime.SelectedValue);

            if (startTime > endTime)
            {
                lblerror.Text = "Start Time cannot be greater than End Time.";
                return false;
            }
        }
        string[] selectedBehaviors = Request.Form.GetValues("behavior");

        if (selectedBehaviors == null || selectedBehaviors.Length <= 0)
        {
            lblerror.Text = "No behaviors selected.";
            return false;
        }
        lblerror.Text = "";
        return true;
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        int id = ddlLessonplan.SelectedIndex;
        if (id >= 0)
        {
            if (id > 0)
            {
                ddlLessonplan.SelectedIndex = id - 1;
            }
            string behid = ddlLessonplan.SelectedValue.ToString();
            Loaddata(behid, false);
            string script = "hidewait();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop2", script, true);
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        int id = ddlLessonplan.SelectedIndex;
        int count = ddlLessonplan.Items.Count - 1;
        if (id <= count)
        {
            if (id < count)
            {
                ddlLessonplan.SelectedIndex = id + 1;
            }
            string behid = ddlLessonplan.SelectedValue.ToString();
            Loaddata(behid, false);
            string script = "hidewait();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop", script, true);
            
        }
    }
    protected void ddlLessonplan_SelectedIndexChanged(object sender, EventArgs e)
    {
        string behid = ddlLessonplan.SelectedValue.ToString();
        Loaddata(behid, false);
        string script = "hidewait();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hidepop3", script, true);
        
    }
    protected void Loaddata(string behid, bool exp)
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
          DateTime  dtst = DateTime.ParseExact(txtStartDate.Text.Trim().Replace("-", "/"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
          DateTime dted = DateTime.ParseExact(txtEndDate.Text.Trim().Replace("-", "/"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
          DateTime endOfDay = dted.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(900);
            string startDate = dtst.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string endDate = endOfDay.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string query = "SELECT bd.MeasurementId,bd.Behaviour,bd.Frequency AS IsFrequency,bd.Duration AS IsDuration,bd.YesOrNo AS IsYesNo, CASE WHEN b.Duration IS NULL THEN 0  ELSE b.Duration END  AS DurationCount,"
              + "  CASE  WHEN b.YesOrNo IS NULL THEN 0  ELSE b.FrequencyCount   END AS YesOrNoCount, CASE  WHEN b.YesOrNo IS NULL  THEN ISNULL(b.FrequencyCount, 0)   ELSE 0  END AS FrequencyCount,"
              + "  b.TimeOfEvent  FROM BehaviourDetails bd INNER JOIN Behaviour b  ON bd.MeasurementId = b.MeasurementId WHERE bd.StudentId = " + sess.StudentId + " AND"
            + " bd.SchoolId = " + sess.SchoolId + "AND b.TimeOfEvent BETWEEN '" +startDate+ "' AND '" +endDate+ "' AND bd.MeasurementId IN (" + behid + ") ORDER BY MeasurementId, TimeOfEvent";
            DataTable dt = objData.ReturnDataTable(query, false);
            if (exp == false)
            {
                String title = "";
                if (dt != null && dt.Rows.Count > 0)
                {


                    title = dt.Rows[0]["Behaviour"].ToString() + "<br/>" + dtst.ToString("MM/dd/yyyy") + "-" + dted.ToString("MM/dd/yyyy");

                    string[] Category;
                    if (rblOption.SelectedValue.ToString() == "TimeofDay")
                    {
                        dt.Columns.Add("RoundedTime", typeof(string));
                        foreach (DataRow row in dt.Rows)
                        {
                            DateTime eventTime = (DateTime)row["TimeOfEvent"];
                            DateTime rounded = RoundToNearestHalfHour(eventTime);
                            row["RoundedTime"] = rounded.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        if (chk24Hours.Checked == true)
                        {

                            Category = Enumerable.Range(0, 48)
                            .Select(i => DateTime.Today.AddMinutes(30 * i).ToString("hh:mm tt"))
                            .ToArray();
                        }
                        else
                        {
                            int stime = ddlStartTime.SelectedIndex;
                            int etime = ddlEndTime.SelectedIndex;
                            Category = ddlStartTime.Items
                          .Cast<ListItem>()
                          .Skip(stime)
                          .Take(etime - stime + 1)
                          .Select(item =>
                          {
                              DateTime t;
                              // Safely parse each item and reformat to "hh:mm tt"
                              if (DateTime.TryParse(item.Text, out t))
                                  return t.ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                              else
                                  return item.Text.Trim(); // fallback if not parseable
                          })
                          .ToArray();
                        }
                        List<TimeSpan> categoryTimes = Category
                       .Select(c => DateTime.ParseExact(c, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay)
                       .OrderBy(t => t)
                       .ToList();

                        TimeSpan minCat = categoryTimes.First();
                        TimeSpan maxCat = categoryTimes.Last();
                        List<DataRow> rowsToDelete = new List<DataRow>();
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["TimeOfEvent"] != DBNull.Value)
                            {
                                DateTime eventTime = Convert.ToDateTime(row["TimeOfEvent"]);
                                DateTime rounded = RoundToNearestHalfHour(eventTime);

                                TimeSpan roundedTimeSpan = rounded.TimeOfDay;

                                if (roundedTimeSpan < minCat || roundedTimeSpan > maxCat)
                                {
                                    rowsToDelete.Add(row);
                                }
                            }
                        }

                        foreach (DataRow r in rowsToDelete)
                        {
                            dt.Rows.Remove(r);
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            bool isfreq = Convert.ToBoolean(dt.Rows[0]["IsFrequency"]);
                            bool isdur = Convert.ToBoolean(dt.Rows[0]["IsDuration"]);
                            bool isyesno = Convert.ToBoolean(dt.Rows[0]["IsYesNo"]);
                            String primaryaxis = "";
                            string secondaryaxis = "";
                            if (isdur == true)
                            {
                                secondaryaxis = "dur";
                            }
                            int[] sumArrayfreq = new int[Category.Length];
                            if (isfreq == true)
                            {
                                primaryaxis = "Frequency";
                                for (int i = 0; i < Category.Length; i++)
                                {
                                    string timeSlot = Category[i];
                                    sumArrayfreq[i] = dt.AsEnumerable()
                                        .Where(row => Convert.ToString(row["RoundedTime"]).Trim() == timeSlot.Trim())
                                        .Sum(row => Convert.ToInt32(row["FrequencyCount"] == DBNull.Value ? 0 : row["FrequencyCount"]));
                                }
                            }
                            if (isyesno == true && isfreq == false)
                            {
                                primaryaxis = "YesOrNo";
                                for (int i = 0; i < Category.Length; i++)
                                {
                                    string timeSlot = Category[i];
                                    sumArrayfreq[i] = dt.AsEnumerable()
                                        .Where(row => Convert.ToString(row["RoundedTime"]).Trim() == timeSlot.Trim())
                                        .Sum(row => Convert.ToInt32(row["YesOrNoCount"] == DBNull.Value ? 0 : row["YesOrNoCount"]));
                                }
                            }

                            int[] sumArraydur = new int[Category.Length];
                            for (int i = 0; i < Category.Length; i++)
                            {
                                string timeSlot = Category[i];
                                sumArraydur[i] = dt.AsEnumerable()
                                    .Where(row => Convert.ToString(row["RoundedTime"]).Trim() == timeSlot.Trim())
                                    .Sum(row => Convert.ToInt32(row["DurationCount"] == DBNull.Value ? 0 : row["DurationCount"]));
                            }
                            container.Visible = true;
                            var serializer = new JavaScriptSerializer();
                            string jsArr1 = serializer.Serialize(Category);
                            string jsArr2 = serializer.Serialize(sumArrayfreq);
                            string jsArr3 = serializer.Serialize(sumArraydur);
                            string prime = serializer.Serialize(primaryaxis);
                            string sec = serializer.Serialize(secondaryaxis);
                            string tit = serializer.Serialize(title);

                            string script = string.Format("loadchart({0}, {1}, {2},{3},{4},{5});", jsArr1, jsArr2, jsArr3, prime, sec, tit);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "loadChartScript", script, true);
                        }
                                    else
                                {
                                    title = ddlLessonplan.SelectedItem + "<br/>" + dtst.ToString("MM/dd/yyyy") + "-" + dted.ToString("MM/dd/yyyy");
                                    container.Visible = true;
                                    container.InnerHtml = "<div style='text-align:center; line-height:1.4;'>" +
                                        "<span style='font-size:16px;'>" +
                                            title.Split(new string[] { "<br/>" }, StringSplitOptions.None)[0] +
                                        "</span><br/>" +
                                        "<span style='font-size:12px; color:#666;'>" +
                                            title.Split(new string[] { "<br/>" }, StringSplitOptions.None)[1] +
                                        "</span><br/>" +
                                        "<span style='font-size:18px; font-weight:bold; color:#333;'>No Data Available</span>" +
                                    "</div>";
                                }
                    }

                        else
                        {
                            bool isfreq = Convert.ToBoolean(dt.Rows[0]["IsFrequency"]);
                            bool isdur = Convert.ToBoolean(dt.Rows[0]["IsDuration"]);
                            bool isyesno = Convert.ToBoolean(dt.Rows[0]["IsYesNo"]);
                            String primaryaxis = "";
                            string secondaryaxis = "";
                            if (isdur == true)
                            {
                                secondaryaxis = "dur";
                            }
                            Category = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                            dt.Columns.Add("Day", typeof(string));
                            foreach (DataRow row in dt.Rows)
                            {
                                DateTime eventTime = (DateTime)row["TimeOfEvent"];
                                row["Day"] = eventTime.ToString("ddd");
                            }
                            int[] sumArrayfreq = new int[Category.Length];
                            if (isfreq == true)
                            {
                                primaryaxis = "Frequency";
                                for (int i = 0; i < Category.Length; i++)
                                {
                                    string timeSlot = Category[i];
                                    sumArrayfreq[i] = dt.AsEnumerable()
                                .Where(row => row.Field<string>("Day") == timeSlot)
                                .Sum(row => row.Field<int?>("FrequencyCount") ?? 0);
                                }
                            }
                            if (isyesno == true && isfreq == false)
                            {
                                primaryaxis = "YesOrNo";
                                for (int i = 0; i < Category.Length; i++)
                                {
                                    string timeSlot = Category[i];
                                    sumArrayfreq[i] = dt.AsEnumerable()
                                .Where(row => row.Field<string>("Day") == timeSlot)
                                .Sum(row => row.Field<int?>("YesOrNoCount") ?? 0);
                                }
                            }
                            int[] sumArraydur = new int[Category.Length];
                            for (int i = 0; i < Category.Length; i++)
                            {
                                string timeSlot = Category[i];
                                sumArraydur[i] = dt.AsEnumerable()
                             .Where(row => row.Field<string>("Day") == timeSlot)
                             .Sum(row => row.Field<int?>("DurationCount") ?? 0);
                            }
                            container.Visible = true;
                            var serializer = new JavaScriptSerializer();
                            string jsArr1 = serializer.Serialize(Category);
                            string jsArr2 = serializer.Serialize(sumArrayfreq);
                            string jsArr3 = serializer.Serialize(sumArraydur);
                            string prime = serializer.Serialize(primaryaxis);
                            string sec = serializer.Serialize(secondaryaxis);
                            string tit = serializer.Serialize(title);

                        string script = string.Format("loadchart({0}, {1}, {2},{3},{4},{5});", jsArr1, jsArr2, jsArr3, prime, sec, tit);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "loadChartScript", script, true);
                    }
                }
                else
                {
                    title = ddlLessonplan.SelectedItem + "<br/>" + dtst.ToString("MM/dd/yyyy") + "-" + dted.ToString("MM/dd/yyyy");
                    container.Visible = true;
                    container.InnerHtml = "<div style='text-align:center; line-height:1.4;'>" +
                        "<span style='font-size:16px;'>" +
                            title.Split(new string[] { "<br/>" }, StringSplitOptions.None)[0] +
                        "</span><br/>" +
                        "<span style='font-size:12px; color:#666;'>" +
                            title.Split(new string[] { "<br/>" }, StringSplitOptions.None)[1] +
                        "</span><br/>" +
                        "<span style='font-size:18px; font-weight:bold; color:#333;'>No Data Available</span>" +
                    "</div>";
                }

            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
        {
    var chartList = new List<object>();
    var serializer = new JavaScriptSerializer();

    var measurementIds = dt.AsEnumerable()
        .Select(r => r.Field<int>("MeasurementId"))
        .Distinct()
        .ToList();

    foreach (var mid in measurementIds)
    {
        DataTable dtFiltered = dt.AsEnumerable()
            .Where(r => r.Field<int>("MeasurementId") == mid)
            .CopyToDataTable();

        string behaviour = dtFiltered.Rows[0]["Behaviour"].ToString();
        string title = behaviour + "<br/>" + dtst.ToString("MM/dd/yyyy") + "-" + dted.ToString("MM/dd/yyyy");

        string[] Category;
        int[] sumArrayfreq;
        int[] sumArraydur;
        string primaryaxis = "";
        string secondaryaxis = "";

        if (rblOption.SelectedValue.ToString() == "TimeofDay")
        {
            dtFiltered.Columns.Add("RoundedTime", typeof(string));
            foreach (DataRow row in dtFiltered.Rows)
            {
                DateTime eventTime = (DateTime)row["TimeOfEvent"];
                DateTime rounded = RoundToNearestHalfHour(eventTime);
                row["RoundedTime"] = rounded.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            }

                            if (chk24Hours.Checked)
                            {
                                Category = Enumerable.Range(0, 48)
                                    .Select(i => DateTime.Today.AddMinutes(30 * i).ToString("hh:mm tt"))
                                    .ToArray();
                            }
                            else
                            {
                                int stime = ddlStartTime.SelectedIndex;
                                int etime = ddlEndTime.SelectedIndex;
                                Category = ddlStartTime.Items.Cast<ListItem>()
                                    .Skip(stime)
                                    .Take(etime - stime + 1)
                                    .Select(item =>
                                    {
                                        DateTime t;
                                        if (DateTime.TryParse(item.Text, out t))
                                            return t.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                                        else
                                            return item.Text.Trim();
                                    })
                                    .ToArray();
                            }
                                                    List<TimeSpan> categoryTimes = Category
                        .Select(c => DateTime.ParseExact(c, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay)
                        .OrderBy(t => t)
                        .ToList();

                                                    TimeSpan minCat = categoryTimes.First();
                                                    TimeSpan maxCat = categoryTimes.Last();
                            List<DataRow> rowsToDelete = new List<DataRow>();
                            foreach (DataRow row in dtFiltered.Rows)
                            {
                                if (row["TimeOfEvent"] != DBNull.Value)
                                {
                                    DateTime eventTime = Convert.ToDateTime(row["TimeOfEvent"]);
                                    DateTime rounded = RoundToNearestHalfHour(eventTime);

                                    TimeSpan roundedTimeSpan = rounded.TimeOfDay;

                                    if (roundedTimeSpan < minCat || roundedTimeSpan > maxCat)
                                    {
                                        rowsToDelete.Add(row);
                                    }
                                }
                            }

                            foreach (DataRow r in rowsToDelete)
                            {
                                dtFiltered.Rows.Remove(r);
                            }

                        }
                        else
                        {
                            Category = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                            dtFiltered.Columns.Add("Day", typeof(string));
                            foreach (DataRow row in dtFiltered.Rows)
                            {
                                DateTime eventTime = (DateTime)row["TimeOfEvent"];
                                row["Day"] = eventTime.ToString("ddd");
                            }
                        }
                       
                        if (dtFiltered != null && dtFiltered.Rows.Count > 0)
                        {
                            bool isfreq = Convert.ToBoolean(dtFiltered.Rows[0]["IsFrequency"]);
                            bool isdur = Convert.ToBoolean(dtFiltered.Rows[0]["IsDuration"]);
                            bool isyesno = Convert.ToBoolean(dtFiltered.Rows[0]["IsYesNo"]);

                            if (isdur) secondaryaxis = "dur";

                            sumArrayfreq = new int[Category.Length];
                            sumArraydur = new int[Category.Length];

                            if (isfreq)
                            {
                                primaryaxis = "Frequency";
                                for (int i = 0; i < Category.Length; i++)
                                {
                                    string slot = Category[i];
                                    if (rblOption.SelectedValue == "TimeofDay")
                                    {
                                        sumArrayfreq[i] = dtFiltered.AsEnumerable()
                                            .Where(r => r.Field<string>("RoundedTime") == slot)
                                            .Sum(r => r.Field<int?>("FrequencyCount") ?? 0);
                                    }
                                    else
                                    {
                                        sumArrayfreq[i] = dtFiltered.AsEnumerable()
                                            .Where(r => r.Field<string>("Day") == slot)
                                            .Sum(r => r.Field<int?>("FrequencyCount") ?? 0);
                                    }
                                }
                            }
                            else if (isyesno)
                            {
                                primaryaxis = "YesOrNo";
                                for (int i = 0; i < Category.Length; i++)
                                {
                                    string slot = Category[i];
                                    if (rblOption.SelectedValue == "TimeofDay")
                                    {
                                        sumArrayfreq[i] = dtFiltered.AsEnumerable()
                                            .Where(r => r.Field<string>("RoundedTime") == slot)
                                            .Sum(r => r.Field<int?>("YesOrNoCount") ?? 0);
                                    }
                                    else
                                    {
                                        sumArrayfreq[i] = dtFiltered.AsEnumerable()
                                            .Where(r => r.Field<string>("Day") == slot)
                                            .Sum(r => r.Field<int?>("YesOrNoCount") ?? 0);
                                    }
                                }
                            }

                            for (int i = 0; i < Category.Length; i++)
                            {
                                string slot = Category[i];
                                if (rblOption.SelectedValue == "TimeofDay")
                                {
                                    sumArraydur[i] = dtFiltered.AsEnumerable()
                                        .Where(r => r.Field<string>("RoundedTime") == slot)
                                        .Sum(r => r.Field<int?>("DurationCount") ?? 0);
                                }
                                else
                                {
                                    sumArraydur[i] = dtFiltered.AsEnumerable()
                                        .Where(r => r.Field<string>("Day") == slot)
                                        .Sum(r => r.Field<int?>("DurationCount") ?? 0);
                                }
                            }
                            string sname = "";
                            // Add this chart to list
                            chartList.Add(new
                            {
                                id = mid,
                                jsArr1 = Category,
                                jsArr2 = sumArrayfreq,
                                jsArr3 = sumArraydur,
                                primaryaxis = primaryaxis,
                                secondaryaxis = secondaryaxis,
                                title = title,
                                sname = sess.StudentName.ToString()

                            });
                        }
                    }
                            JavaScriptSerializer serializer1 = new JavaScriptSerializer();
                            string json = serializer1.Serialize(chartList);
                            string script = "var jsonChartData = " + json + "; renderMultipleCharts(jsonChartData);";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "renderCharts", script, true);
                        
                        
                    
                }
            }
    }
    private DateTime RoundToNearestHalfHour(DateTime dt)
    {
        int minutes = dt.Minute;
        int newMinutes = 0;

        if (minutes < 15)
            newMinutes = 0;
        else if (minutes < 45)
            newMinutes = 30;
        else
        {
            dt = dt.AddHours(1);
            newMinutes = 0;
        }

        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, newMinutes, 0);
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (ViewState["Starttime"]!=null && ViewState["Endtime"]!=null)
        {
             ddlStartTime.SelectedIndex= Convert.ToInt32(ViewState["Starttime"]);
            ddlEndTime.SelectedIndex = Convert.ToInt32(ViewState["Endtime"]);
        }
        ScriptManager.RegisterStartupScript(this,GetType(),Guid.NewGuid().ToString(),"loadwait();",true);
        container.Visible = false;
        chartArea.Visible = true;
        string allValues = string.Join(",", ddlLessonplan.Items.Cast<ListItem>().Select(i => i.Value));
        Loaddata(allValues, true);
    }
  


}

