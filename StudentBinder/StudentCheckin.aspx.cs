using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public partial class StudentBinder_Phase2Css_StudentCheckin : System.Web.UI.Page
{

    clsData objData = null;
    static int StudentId = 0;
    static int ClassId = 0; 
    string strQuery = "";
    clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ScriptManager.GetCurrent(this.Page) != null)
        {
            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnRefreshGrid);
        }
        if (!IsPostBack)
        {
            //imgBDay.ImageUrl = "~/StudentBinder/img/DayB.png";
            //ImgBRes.ImageUrl = "~/StudentBinder/img/ResG.png";
            hidSetVal.Value = "0";
            fillStudent("0", false);
            BindLocationDropdown();
            BindClientDropdown();
            pnlCalendar.Style["display"] = "none";
            
            //// optionally prebind student dropdown for initial selection (first class)
            //if (!String.IsNullOrEmpty(ddlLocation.SelectedValue))
            //    BindClientDropdown(ddlLocation.SelectedValue);
            //else
            //    ddlClient.Items.Clear();
        }
        var sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                // Register calendar so its non-standard events (VisibleMonthChanged) do async postbacks
                sm.RegisterAsyncPostBackControl(calPast);
            }
       // else
       // {
       //     if (hidSearch.Value != "1") bindDetails();
       //}
    }
    //private void bindDetails()           
    //{
        //if (txtSearch.Text == "")
        //{
        //    fillStudent(hidSetVal.Value.ToString(), false);
        //}
        //else
        //{
        //    fillStudent(hidSetVal.Value.ToString(), true);
        //}

    //}

    protected void grdGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        var drv = e.Row.DataItem as DataRowView;
        if (drv == null) return;

        // ------------- 0. Put Extras into hidden field (your existing behavior) -------------
        var hidExtras = e.Row.FindControl("hidExtraTimes") as HiddenField;
        if (hidExtras != null)
        {
            hidExtras.Value = Convert.ToString(drv["Extras"]);
        }

        // ------------- 1. Read latest/main values from the datarow -------------
        string inTime = "";
        string outTime = "";
        string latestCode = "";

        object inVal = drv["InTime"];
        if (inVal != DBNull.Value)
        {
            DateTime dt;
            if (DateTime.TryParse(inVal.ToString(), out dt)) inTime = dt.ToString("HH:mm");
        }

        object outVal = drv["OutTime"];
        if (outVal != DBNull.Value)
        {
            DateTime dt;
            if (DateTime.TryParse(outVal.ToString(), out dt)) outTime = dt.ToString("HH:mm");
        }

        if (drv.Row.Table.Columns.Contains("AttendanceCode") && drv["AttendanceCode"] != DBNull.Value)
            latestCode = drv["AttendanceCode"].ToString().Trim();
        else if (drv.Row.Table.Columns.Contains("LeaveCode") && drv["LeaveCode"] != DBNull.Value)
            latestCode = drv["LeaveCode"].ToString().Trim();

        // ------------- 2. Set the main template controls (In / Out / Code) -------------
        TextBox txtIn = e.Row.FindControl("txtInTime") as TextBox;
        if (txtIn != null)
        {
            txtIn.Text = FormatToHHmm(inVal);
            try { txtIn.Attributes["type"] = "time"; txtIn.Attributes["step"] = "60"; }
            catch { }
        }

        TextBox txtOut = e.Row.FindControl("txtOutTime") as TextBox;
        if (txtOut != null)
        {
            txtOut.Text = FormatToHHmm(outVal);
            try { txtOut.Attributes["type"] = "time"; txtOut.Attributes["step"] = "60"; }
            catch { }
        }

        // Prepare lookup once and reuse for main + extras
        var dtLookup = GetAttendanceCodeLookup();

        DropDownList ddlMain = e.Row.FindControl("ddlCode") as DropDownList;
        if (ddlMain != null)
        {
            ddlMain.Items.Clear();
            ddlMain.Items.Add(new ListItem("---Select---", ""));
            if (dtLookup != null)
            {
                foreach (DataRow r in dtLookup.Rows)
                {
                    string text = Convert.ToString(r["LookupName"]);
                    string val = Convert.ToString(r["LookupId"]);
                    ddlMain.Items.Add(new ListItem(text, val));
                }
            }

            // Select the current value if present in data row (AttendanceCode or LeaveCode mapping)
            if (!String.IsNullOrEmpty(latestCode))
            {
                // attempt direct value match first
                var li = ddlMain.Items.FindByValue(latestCode);
                if (li != null) li.Selected = true;
                else
                {
                    // fallback: if lookup table has LookupCode column, map LeaveCode -> LookupId
                    if (dtLookup != null && dtLookup.Columns.Contains("LookupCode"))
                    {
                        DataRow[] matches = dtLookup.Select("LookupCode = '" + latestCode.Replace("'", "''") + "'");
                        if (matches != null && matches.Length > 0)
                        {
                            string lookupId = Convert.ToString(matches[0]["LookupId"]);
                            var li2 = ddlMain.Items.FindByValue(lookupId);
                            if (li2 != null) li2.Selected = true;
                        }
                    }
                }
            }

            if (dtLookup != null && dtLookup.Rows.Count > 0)
            {
                // Build a simple list of { id: ..., name: ... } objects
                var codesList = new List<object>();
                foreach (DataRow r in dtLookup.Rows)
                {
                    codesList.Add(new
                    {
                        id = Convert.ToString(r["LookupId"]),
                        name = Convert.ToString(r["LookupName"]),
                        // optional: include LookupCode if present
                        code = dtLookup.Columns.Contains("LookupCode") ? Convert.ToString(r["LookupCode"]) : null
                    });
                }

                var serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(codesList);

                // Register script once per page - use a fixed key so duplicates are suppressed
                string script = "window.__attendanceCodes = " + json + ";";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "attendanceCodes", script, true);
            }
        }

        // ------------- 3. Parse extras and prepare a list of extras to place -------------
        string extrasRaw = hidExtras != null ? hidExtras.Value : (drv["Extras"] == DBNull.Value ? "" : drv["Extras"].ToString());

        var extrasList = new List<Tuple<string, string, string>>();

        if (!string.IsNullOrWhiteSpace(extrasRaw))
        {
            string[] items = extrasRaw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rawItem in items)
            {
                if (string.IsNullOrWhiteSpace(rawItem)) continue;
                string[] parts = rawItem.Split(new[] { '|' }, StringSplitOptions.None);
                string exIn = parts.Length > 0 ? parts[0].Trim() : "";
                string exOut = parts.Length > 1 ? parts[1].Trim() : "";
                string exCode = parts.Length > 2 ? parts[2].Trim() : "";

                // skip fully empty
                if (string.IsNullOrEmpty(exIn) && string.IsNullOrEmpty(exOut) && string.IsNullOrEmpty(exCode))
                    continue;

                // skip if identical to latest/main (avoids duplication)
                bool inMatches = string.IsNullOrEmpty(inTime) ? string.IsNullOrEmpty(exIn) : inTime == exIn;
                bool outMatches = string.IsNullOrEmpty(outTime) ? string.IsNullOrEmpty(exOut) : outTime == exOut;
                bool codeMatches = string.IsNullOrEmpty(latestCode) ? string.IsNullOrEmpty(exCode) : latestCode == exCode;
                if (inMatches && outMatches && codeMatches) continue;

                extrasList.Add(Tuple.Create(exIn, exOut, exCode));
            }
        }

        // ------------- 4. Find extra controls in the row (flexible detection) -------------
        Func<Control, IEnumerable<Control>> allChildren = null;
        allChildren = (root) =>
        {
            var list = new List<Control>();
            foreach (Control c in root.Controls)
            {
                list.Add(c);
                if (c.HasControls()) list.AddRange(allChildren(c));
            }
            return list;
        };

        var all = allChildren(e.Row);

        var inControls = all
            .Where(c => c is TextBox && (c.ID != null && (c.ID.StartsWith("txtIn_", StringComparison.OrdinalIgnoreCase) || c.ID.StartsWith("txtInTime_", StringComparison.OrdinalIgnoreCase) || (c.ID.StartsWith("txtIn", StringComparison.OrdinalIgnoreCase) && c.ID != "txtInTime"))))
            .Cast<TextBox>()
            .OrderBy(t => t.ID, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var outControls = all
            .Where(c => c is TextBox && (c.ID != null && (c.ID.StartsWith("txtOut_", StringComparison.OrdinalIgnoreCase) || (c.ID.StartsWith("txtOut", StringComparison.OrdinalIgnoreCase) && c.ID != "txtOutTime"))))
            .Cast<TextBox>()
            .OrderBy(t => t.ID, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var codeDdls = all
            .Where(c => c is DropDownList && (c.ID != null && c.ID.StartsWith("ddlCode", StringComparison.OrdinalIgnoreCase) && c.ID != "ddlCode"))
            .Cast<DropDownList>()
            .OrderBy(d => d.ID, StringComparer.OrdinalIgnoreCase)
            .ToList();

        // ------------- 5. Fill extras into discovered controls -------------
        for (int i = 0; i < extrasList.Count; i++)
        {
            var ex = extrasList[i];
            string exIn = ex.Item1;
            string exOut = ex.Item2;
            string exCode = ex.Item3;

            if (i < inControls.Count)
            {
                inControls[i].Text = string.IsNullOrEmpty(exIn) ? "" : exIn;
                try { inControls[i].Attributes["type"] = "time"; inControls[i].Attributes["step"] = "60"; }
                catch { }
            }
            if (i < outControls.Count)
            {
                outControls[i].Text = string.IsNullOrEmpty(exOut) ? "" : exOut;
                try { outControls[i].Attributes["type"] = "time"; outControls[i].Attributes["step"] = "60"; }
                catch { }
            }
            if (i < codeDdls.Count)
            {
                var ddlEx = codeDdls[i];
                ddlEx.Items.Clear();
                ddlEx.Items.Add(new ListItem("---Select---", ""));
                if (dtLookup != null)
                {
                    foreach (DataRow r in dtLookup.Rows)
                    {
                        string text = Convert.ToString(r["LookupName"]);
                        string val = Convert.ToString(r["LookupId"]);
                        ddlEx.Items.Add(new ListItem(text, val));
                    }
                }

                // Try to select by value first
                if (!string.IsNullOrEmpty(exCode))
                {
                    var li = ddlEx.Items.FindByValue(exCode);
                    if (li != null) li.Selected = true;
                    else if (dtLookup != null && dtLookup.Columns.Contains("LookupCode"))
                    {
                        DataRow[] m = dtLookup.Select("LookupCode = '" + exCode.Replace("'", "''") + "'");
                        if (m != null && m.Length > 0)
                        {
                            string lookupId = Convert.ToString(m[0]["LookupId"]);
                            var li2 = ddlEx.Items.FindByValue(lookupId);
                            if (li2 != null) li2.Selected = true;
                        }
                    }
                }
            }
        }

        bool isPresent = true;
        //DataRowView drv = e.Row.DataItem as DataRowView;
        if (drv != null && drv.Row.Table.Columns.Contains("IsPresent"))
        {
            object val = drv["IsPresent"];
            if (val != DBNull.Value)
            {
                bool tmpBool;
                int tmpInt;
                if (val is bool) isPresent = (bool)val;
                else if (int.TryParse(val.ToString(), out tmpInt)) isPresent = tmpInt != 0;
                else if (bool.TryParse(val.ToString(), out tmpBool)) isPresent = tmpBool;
            }
        }

        // If Absent → grey & disable non-attendance columns
        if (!isPresent)
        {
            e.Row.CssClass = (e.Row.CssClass ?? "") + " row-absent";

            // Loop through each cell in this row
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                // Skip the Attendance column (first column = index 0)
                // If your Attendance column is NOT first, change the index accordingly
                if (i == 0) continue;
                if (i == 5) continue;
                if (i == 6) continue;
                TableCell cell = e.Row.Cells[i];
                DisableAllInputs(cell);
            }
        }
        else
        {
            // Ensure no old class remains
            if (!string.IsNullOrEmpty(e.Row.CssClass) && e.Row.CssClass.Contains("row-absent"))
                e.Row.CssClass = e.Row.CssClass.Replace("row-absent", "").Trim();
        }

        // Note: calendar state is intentionally NOT set here (avoid per-row overwrites).
    }


    protected void grdGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
    }

    protected void grdGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Save") return;

        var sm = ScriptManager.GetCurrent(Page);
        string showSaveJs = "try{ if(window.showSaveLoader) showSaveLoader('Saving\\u2026'); else if(window.showLoader) showLoader('Saving\\u2026'); }catch(e){}";
        if (sm != null && sm.IsInAsyncPostBack)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showSaveLoader_" + Guid.NewGuid().ToString("N"), showSaveJs, true);
        else
            ClientScript.RegisterStartupScript(Page.GetType(), "showSaveLoader_" + Guid.NewGuid().ToString("N"), showSaveJs, true);

        Button btn = e.CommandSource as Button;
        if (btn == null) return;
        GridViewRow row = btn.NamingContainer as GridViewRow;
        if (row == null) return;


        // --- existing logic to read studentId, classId ---
        int studentId = 0, classId = 0;
        if (grdGroup.DataKeys != null && grdGroup.DataKeys.Count > row.RowIndex)
        {
            var dk = grdGroup.DataKeys[row.RowIndex];
            if (dk != null)
            {
                int.TryParse(Convert.ToString(dk.Values["studentid"]), out studentId);
                int.TryParse(Convert.ToString(dk.Values["classid"]), out classId);
            }
        }

        if (studentId <= 0 || classId <= 0)
        {
            ShowGridError("Cannot determine student/class id for saving.");
            return;
        }

        // --- session ---
        var sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            ShowGridError("Session expired. Please login again.");
            return;
        }

        int schoolId = sess.SchoolId;
        int userId = sess.LoginId;

        // --- MAIN (primary) IN / OUT ---
        var txtIn = row.FindControl("txtInTime") as TextBox;
        var txtOut = row.FindControl("txtOutTime") as TextBox;

        DateTime? effectiveDate = null;
        try
        {
            string posted = Request.Form["hidPastDate"];
            if (String.IsNullOrEmpty(posted) && hidPastDate != null)
            {
                // use UniqueID too if you prefer: Request.Form[hidPastDate.UniqueID]
                posted = Request.Form[hidPastDate.UniqueID];
                if (String.IsNullOrEmpty(posted)) posted = Request.Form[hidPastDate.ClientID];
            }

            if (!String.IsNullOrEmpty(posted))
            {
                DateTime parsed;
                if (DateTime.TryParse(posted, out parsed)) effectiveDate = parsed.Date;
            }
        }
        catch { /* ignore form-read issues */ }

        // 3) Fall back to server control's value if present
        if (!effectiveDate.HasValue && hidPastDate != null && !String.IsNullOrEmpty(hidPastDate.Value))
        {
            DateTime parsed;
            if (DateTime.TryParse(hidPastDate.Value, out parsed)) effectiveDate = parsed.Date;
        }

        // 4) Final fallback: today
        if (!effectiveDate.HasValue) effectiveDate = DateTime.Today;

        string mainInText = GetPostedOrServerValue(txtIn);
        string mainOutText = GetPostedOrServerValue(txtOut);

        // parse times relative to the selected date
        DateTime? mainIn = ParseTimeToDate(mainInText, effectiveDate);
        DateTime? mainOut = ParseTimeToDate(mainOutText, effectiveDate);

        // --- read code dropdown for the main pair ---
        var ddlCode = row.FindControl("ddlCode") as DropDownList;
        string mainCode = ddlCode != null ? ddlCode.SelectedValue : null;

        // --- EXTRAS: read hidden field that contains serialized extras ---
        string encoded = String.Empty;

        // 1) client-side input named hidExtraTimes_{studentId} (common client naming)
        string hidNameClient = "hidExtraTimes_" + studentId;
        encoded = Convert.ToString(System.Web.HttpContext.Current.Request.Form[hidNameClient] ?? "");

        if (String.IsNullOrEmpty(encoded))
        {
            // 2) maybe the server HiddenField was named just 'hidExtraTimes' in the template
            //    and its rendered name will be the control UniqueID - attempt that:
            var hfBase = row.FindControl("hidExtraTimes") as HiddenField;
            if (hfBase != null)
            {
                // Prefer the actual posted value (unique id) if present
                try
                {
                    string possiblePosted = System.Web.HttpContext.Current.Request.Form[hfBase.UniqueID];
                    if (!String.IsNullOrEmpty(possiblePosted))
                        encoded = possiblePosted;
                    else
                        encoded = hfBase.Value; // fallback
                }
                catch
                {
                    encoded = hfBase.Value;
                }
            }
        }

        // 3) fallback: try control named hidExtraTimes_{studentId} (if you added that server control)
        if (String.IsNullOrEmpty(encoded))
        {
            var hfServer = row.FindControl("hidExtraTimes_" + studentId) as HiddenField;
            if (hfServer != null) encoded = hfServer.Value;
        }

        // 4) final fallback: search for any hiddenfield in row whose ID starts with "hidExtraTimes"
        if (String.IsNullOrEmpty(encoded))
        {
            HiddenField anyHf = null;
            foreach (Control c in row.Controls)
            {
                // recursive search helper
                Func<Control, HiddenField> find = null;
                find = (root) =>
                {
                    foreach (Control cc in root.Controls)
                    {
                        if (cc is HiddenField && cc.ID != null && cc.ID.StartsWith("hidExtraTimes", StringComparison.OrdinalIgnoreCase))
                            return (HiddenField)cc;
                        if (cc.HasControls())
                        {
                            var found = find(cc);
                            if (found != null) return found;
                        }
                    }
                    return null;
                };
                anyHf = find(row);
                break;
            }
            if (anyHf != null) encoded = anyHf.Value;
        }





        // Parse extras robustly: support JSON array or semicolon-delimited "In|Out|Code;..."
        var parsedExtras = new List<string[]>();
        if (!string.IsNullOrEmpty(encoded))
        {
            // decode the whole string first (safe to do even if already decoded)
            encoded = HttpUtility.UrlDecode(encoded).Trim();

            // remove accidental leading/trailing semicolons
            encoded = encoded.Trim(';');

            // now split on ';' into pairs
            var pairStrings = encoded.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var p in pairStrings)
            {
                var trimmed = p.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                // attempt to split into up to 3 parts
                var pieces = trimmed.Split(new[] { '|' }, StringSplitOptions.None);
                string inS = pieces.Length > 0 ? HttpUtility.UrlDecode(pieces[0] ?? "") : "";
                string outS = pieces.Length > 1 ? HttpUtility.UrlDecode(pieces[1] ?? "") : "";
                string codeS = pieces.Length > 2 ? HttpUtility.UrlDecode(pieces[2] ?? "") : "";

                // ignore wholly empty
                if (String.IsNullOrWhiteSpace(inS) && String.IsNullOrWhiteSpace(outS)) continue;

                parsedExtras.Add(new[] { inS, outS, codeS });
            }
        }


        // Convert parsed extras into TimePair objects preserving order
        var allPairs = new System.Collections.Generic.List<TimePair>();

        // --- READ POSTED hidStatus value for this row (try multiple fallbacks) ---
        string postedStatus = "";

        // 1) preferred: client-side named hidStatus_<studentId>
        if (studentId > 0)
            postedStatus = Convert.ToString(System.Web.HttpContext.Current.Request.Form["hidStatus_" + studentId] ?? "");

        // 2) fallback: posted generic hidStatus (server control rendered unique id or client-side control)
        if (String.IsNullOrEmpty(postedStatus))
        {
            // try posted form value by simple name 'hidStatus'
            postedStatus = Convert.ToString(System.Web.HttpContext.Current.Request.Form["hidStatus"] ?? "");
        }

        // 3) fallback: try server HiddenField control inside the row (use UniqueID to get actual posted name)
        if (String.IsNullOrEmpty(postedStatus))
        {
            var hfStatusServer = row.FindControl("hidStatus") as HiddenField;
            if (hfStatusServer != null)
            {
                try
                {
                    string possible = Convert.ToString(System.Web.HttpContext.Current.Request.Form[hfStatusServer.UniqueID] ?? "");
                    if (!String.IsNullOrEmpty(possible)) postedStatus = possible;
                    else postedStatus = hfStatusServer.Value ?? "";
                }
                catch
                {
                    postedStatus = hfStatusServer.Value ?? "";
                }
            }
        }

        // Normalize postedStatus into nullable bool (1/True => true; 0/False => false; empty => null)
        bool? postedIsPresent = null;
        if (!String.IsNullOrEmpty(postedStatus))
        {
            var ps = postedStatus.Trim();
            if (ps == "1" || ps.Equals("True", StringComparison.OrdinalIgnoreCase) || ps.Equals("Present", StringComparison.OrdinalIgnoreCase))
                postedIsPresent = true;
            else if (ps == "0" || ps.Equals("False", StringComparison.OrdinalIgnoreCase) || ps.Equals("Absent", StringComparison.OrdinalIgnoreCase))
                postedIsPresent = false;
            // otherwise leave as null (unknown)
        }

        // add primary (main) pair first — include posted presence
        allPairs.Add(new TimePair
        {
            In = mainIn,
            Out = mainOut,
            Code = mainCode,
            IsPresent = postedIsPresent
        });

        int startIndex = 0;
        if (parsedExtras.Count > 0)
        {
            var p0 = parsedExtras[0];
            var p0In = ParseTimeToDate(HttpUtility.UrlDecode(p0[0] ?? ""), effectiveDate);
            var p0Out = ParseTimeToDate(HttpUtility.UrlDecode(p0[1] ?? ""), effectiveDate);
            if (p0In == mainIn && p0Out == mainOut)
                startIndex = 1; // skip duplicate
        }

        for (int i = startIndex; i < parsedExtras.Count; i++)
        {
            var arr = parsedExtras[i];
            string inStr = arr.Length > 0 ? arr[0] : "";
            string outStr = arr.Length > 1 ? arr[1] : "";
            string codeStr = arr.Length > 2 ? arr[2] : "";

            DateTime? inDt = ParseTimeToDate(inStr, effectiveDate);
            DateTime? outDt = ParseTimeToDate(outStr, effectiveDate);

            if (!inDt.HasValue && !outDt.HasValue) continue;

            allPairs.Add(new TimePair
            {
                In = inDt,
                Out = outDt,
                Code = codeStr,
                IsPresent = postedIsPresent    // keep same row-level presence for extras
            });
        }

        try
        {
            SaveOrderedPairs(studentId, classId, schoolId, userId, allPairs, deleteExtraDbRows: true, targetDate: effectiveDate);
            string js = "setTimeout(function(){ if(typeof normalizeAllExtras === 'function'){ try{ normalizeAllExtras(); }catch(e){console && console.log && console.log('normalizeAllExtras error',e);} } },50);";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "normalizeAfterSave_" + Guid.NewGuid().ToString("N"), js, true);
            // show success band
            ShowGridBand("Saved successfully", 3000, "success");
        }
        catch (Exception ex)
        {
            ShowGridError("Error saving attendance: " + ex.Message);
        }
    }
    private static string GetPostedOrServerValue(TextBox tb)
    {
        if (tb == null) return string.Empty;
        var req = System.Web.HttpContext.Current.Request;
        string v = req.Form[tb.UniqueID];
        if (string.IsNullOrEmpty(v)) v = tb.Text;
        return v ?? string.Empty;
    }
    private static DateTime? ParseHHmmToday(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        var p = s.Trim().Split(':'); int h, m;
        if (p.Length == 2 && int.TryParse(p[0], out h) && int.TryParse(p[1], out m)
            && h >= 0 && h < 24 && m >= 0 && m < 60)
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, h, m, 0);
        return null;
    }
    private void FocusControl(Control ctl)
    {
        if (ctl == null) return;
        var sm = ScriptManager.GetCurrent(Page);
        string js = "var el=document.getElementById('" + ctl.ClientID + "'); if(el){el.focus();}";
        if (sm != null && sm.IsInAsyncPostBack)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "focus_" + ctl.ClientID, js, true);
        else
            ClientScript.RegisterStartupScript(Page.GetType(), "focus_" + ctl.ClientID, js, true);
    }
    private void SaveCheckInCheckOutTime(int studentId, int classId, int schoolId, int userId,
                                     DateTime? checkin, DateTime? checkout)
    {
        objData = new clsData();

        if (!checkin.HasValue && !checkout.HasValue) return;

        if (checkin.HasValue && checkout.HasValue && checkout.Value < checkin.Value)
        {
            checkout = null;
            ShowGridError("OUT time cannot be earlier than IN time.");
            return;
        }

        string checkinSql = checkin.HasValue ? "'" + checkin.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL";
        string checkoutSql = checkout.HasValue ? "'" + checkout.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL";

        string setClause = "";
        if (checkin.HasValue) setClause += "CheckinTime=" + checkinSql;
        if (checkout.HasValue) setClause += (setClause == "" ? "" : ", ") + "CheckoutTime=" + checkoutSql;
        if (setClause == "") return;

        string statusOnInsert = (checkout.HasValue && !checkin.HasValue) ? "False" : "True";

        string pk = "StdtSessEventId";

        string sql = @"UPDATE se
               SET " + setClause + @", ModifiedOn = GETDATE()
            FROM StdtSessEvent AS se
            INNER JOIN (
                SELECT TOP (1) " + pk + @"
                FROM StdtSessEvent
                WHERE SchoolId = " + schoolId + @"
                  AND StudentId = " + studentId + @"
                  AND ClassId   = " + classId + @"
                  AND EventType = 'CH'
                  AND CreatedOn >= CAST(GETDATE() AS date)
                  AND CreatedOn <  DATEADD(day, 1, CAST(GETDATE() AS date))
                ORDER BY ModifiedOn DESC, CreatedOn DESC
            ) AS t ON t." + pk + @" = se." + pk + @";

            IF @@ROWCOUNT = 0
            BEGIN
                INSERT INTO StdtSessEvent
                    (SchoolId, StudentId, EvntTs, CheckStatus, ClassId, CreatedBy, CreatedOn, ModifiedOn, EventType, CheckinTime, CheckoutTime)
                VALUES
                    (" + schoolId + @", " + studentId + @", GETDATE(), '" + statusOnInsert + @"', " + classId + @", " + userId + @",
                     GETDATE(), GETDATE(), 'CH', " + checkinSql + @", " + checkoutSql + @");
            END";

        objData.Execute(sql);
        fillStudent(hidSetVal.Value.ToString(), false);
        ShowGridSaved();
    }

    /////-------FUNCTION FOR ENTERING 0 INTO THE BEHAVIOR------
    /////
    //[System.Web.Services.WebMethod]
    //private void SaveBehaviorForStudent(int StudentId) 
    // {
    //     objData = new clsData();
    //     sess = (clsSession)Session["UserSession"];
    //     string InsertBehavior = "SELECT MeasurementId FROM BehaviourDetails WHERE StudentId="+StudentId+" AND ActiveInd='A' AND MeasurementId NOT IN (SELECT DISTINCT MeasurementId FROM Behaviour WHERE StudentId="+StudentId+" AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()))";
    //     DataTable DTBehavior= objData.ReturnDataTable(InsertBehavior, false);
    //     string InsertQuery = "";
    //     int insertresult = 0;
    //     foreach (DataRow Behaviour in DTBehavior.Rows)
    //     {
    //         InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId) Values('" + Behaviour["MeasurementId"] + "','" + StudentId + "','" + 0 + "','A','" + sess.LoginId + "',getdate(),'" + sess.LoginId + "',getdate(),'" + sess.LoginId + "')";
    //         insertresult = objData.Execute(InsertQuery);
    //     }
    // }
    private void SaveCheckIn(ImageButton Img, int StudentId, int ClassId, int Schoolid, int Userid)
    {
        objData = new clsData();
        if (Img.ImageUrl == "~/StudentBinder/img/in.png")
        {
            bool blExist = objData.IFExists("SELECT StudentId FROM StdtSessEvent WHERE StudentId=" + StudentId + " and ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()) ");
            if (blExist == true)
            {
                objData.Execute("Update [StdtSessEvent] SET CheckStatus='False',IsPresent=0,CheckoutTime=GETDATE() Where StudentId=" + StudentId + "  And ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
                // SaveBehaviorForStudent(StudentId);
                Img.ImageUrl = "~/StudentBinder/img/out.png";
            }
        }
        else
        {
            bool blExist = objData.IFExists("SELECT StudentId FROM StdtSessEvent WHERE StudentId=" + StudentId + " and ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
            if (blExist == true)
            {
                objData.Execute("Update [StdtSessEvent] SET CheckStatus='True', IsPresent=1 Where StudentId=" + StudentId + "  And ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
                Img.ImageUrl = "~/StudentBinder/img/in.png";
            }
            else
            {
                strQuery = "insert into StdtSessEvent (SchoolId,StudentId,EvntTs,CheckStatus,IsPresent,ClassId,CreatedBy,CreatedOn,ModifiedOn,EventType,CheckinTime)values(" + Schoolid + "," + StudentId + ",getdate(),'True',1," + ClassId + "," + Userid + ",getdate(),getdate(),'CH',GETDATE()) ";
                objData.Execute(strQuery);
                Img.ImageUrl = "~/StudentBinder/img/in.png";
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //if (txtSearch.Text == "")
        //{
        //    fillStudent(hidSetVal.Value.ToString(), false);
        //}
        //else
        //{
        //    fillStudent(hidSetVal.Value.ToString(), true);
        //}

    }


    private void fillStudent(string type, bool search)
    {
        try
        {

            sess = (clsSession)Session["UserSession"];
            objData = new clsData();
            string DayNRes = (type == "1") ? " OR c.ResidenceInd='0' " : "";
            string hidVal = (hidSetVal != null) ? (hidSetVal.Value ?? "") : "";
            string safeHidVal = hidVal.Replace("'", "''"); // escape single quotes

            // 2) optional Location filter (from a ddl or other source). If you use a different control, replace accordingly.
            string locationFilter = "";
            int locId;
            try
            {
                // try to read ddlLocation if present on page
                var ddlLoc = FindControlRecursive(this.Page, "ddlLocation") as DropDownList;
                string locVal = ddlLoc != null ? ddlLoc.SelectedValue : "";
                if (!string.IsNullOrEmpty(locVal) && int.TryParse(locVal, out locId))
                {
                    locationFilter = " AND sc2.ClassId = " + locId.ToString();
                }
            }
            catch { /* ignore if control not present */ }

            // 3) optional Student filter
            string studentFilter = "";
            int studId;
            try
            {
                var ddlStud = FindControlRecursive(this.Page, "ddlClient") as DropDownList;
                string studVal = ddlStud != null ? ddlStud.SelectedValue : "";
                if (!string.IsNullOrEmpty(studVal) && int.TryParse(studVal, out studId))
                {
                    studentFilter = " AND s.StudentId = " + studId.ToString();
                }
            }
            catch { /* ignore if control not present */ }

            bool useIsoDate = false;
            string isoDate = ""; // yyyy-MM-dd
            DateTime chosenDate = DateTime.MinValue;

            // A) Prefer explicit calendar selection first (user intent should win)
            try
            {
                var cal = FindControlRecursive(this.Page, "calPast") as System.Web.UI.WebControls.Calendar;
                if (cal != null && cal.SelectedDate != DateTime.MinValue)
                {
                    chosenDate = cal.SelectedDate;
                    useIsoDate = true;
                    isoDate = chosenDate.ToString("yyyy-MM-dd");
                }
            }
            catch { /* ignore calendar lookup errors */ }

            // B) If no calendar selection, try Request.Form for posted hidden field (works in async postbacks)
            if (!useIsoDate)
            {
                try
                {
                    string posted = null;
                    if (hidPastDate != null)
                    {
                        // Use UniqueID (form name) then ClientID fallback
                        posted = Request.Form[hidPastDate.UniqueID];
                        if (String.IsNullOrEmpty(posted))
                            posted = Request.Form[hidPastDate.ClientID];
                    }
                    // common client-side names fallback
                    if (String.IsNullOrEmpty(posted))
                        posted = Request.Form["hidPastDate"] ?? Request.Form["hfSelectedDate"] ?? Request.Form["selectedDate"];

                    if (!String.IsNullOrEmpty(posted) && DateTime.TryParse(posted, out chosenDate))
                    {
                        useIsoDate = true;
                        isoDate = chosenDate.ToString("yyyy-MM-dd");
                    }
                }
                catch { /* ignore form-read errors */ }
            }

            // C) Last fallback: server-side hidden control value (original behavior)
            if (!useIsoDate && hidPastDate != null && !String.IsNullOrEmpty(hidPastDate.Value))
            {
                if (DateTime.TryParse(hidPastDate.Value, out chosenDate))
                {
                    useIsoDate = true;
                    isoDate = chosenDate.ToString("yyyy-MM-dd");
                }
            }

            // convenience SQL snippets that vary depending on whether isoDate is used
            string eventsDateCondition = useIsoDate
                ? "AND CONVERT(date, se.CreatedOn) = CONVERT(date, '" + isoDate + "')"    // or use CheckinTime if preferred
                : "AND CONVERT(date, se.CreatedOn) = CONVERT(date, GETDATE())";

            string presenceDateCondition = useIsoDate
                ? "AND CONVERT(date, x.CheckinTime) = CONVERT(date, '" + isoDate + "')"
                : "AND CONVERT(date, x.CheckinTime) = CONVERT(date, GETDATE())";

            // placement date condition (only if isoDate used) - otherwise keep original plc.EndDate IS NULL requirement (or no date filter)
            string placementDateCondition = useIsoDate
                ? "AND plc.StartDate <= '" + isoDate + "' AND (plc.EndDate IS NULL OR plc.EndDate >= '" + isoDate + "')"
                : "AND plc.EndDate IS NULL"; // original semantics when not filtering by date
            if (search == true)
            {
                //strQuery = "SELECT StudClass.StudentId as studentid,StudClass.name,StudClass.ClassId as classid,chkStatus =CASE WHEN (SELECT COUNT(*) FROM StdtSessEvent WHERE " +
                //            "StudentId=StudClass.StudentId AND EventType='CH' AND  CONVERT(DATE,CheckinTime)=CONVERT(DATE,GETDATE()) AND CheckStatus='True' and ClassId=StudClass.ClassId AND SchoolId=" + sess.SchoolId + ")=0 THEN 0 ELSE 1 END ,ses.CheckinTime as InTime,ses.CheckoutTime as OutTime, 'True' as IsPresent " +
                //            "FROM (Select s.StudentId,s.StudentLname+'  '+s.StudentFname+'-'+s.StudentNbr+'   '+'('+c.ClassName+')' as name,c.ClassId " +
                //            "from Student s Inner Join StdtClass sc on s.StudentId=sc.StdtId Inner Join Class c " +
                //            "on c.ClassId=sc.ClassId where s.ActiveInd='A' AND sc.ActiveInd='A' AND  (c.ResidenceInd='" + hidSetVal.Value + "' " + DayNRes + " ) AND s.StudentLname like'%" + txtSearch.Text + "%') AS StudClass OUTER APPLY( SELECT TOP(1) chkStatus   = CASE WHEN se.CheckStatus = 1 " +
                //            "THEN 1 ELSE 0 END,se.CheckinTime,se.CheckoutTime FROM StdtSessEvent se WHERE se.StudentId = StudClass.StudentId AND se.ClassId = StudClass.ClassId AND se.EventType='CH' AND CONVERT(date, se.CreatedOn) = CONVERT(date, GETDATE())  ORDER BY se.ModifiedOn DESC ) AS ses " +
                //            "GROUP BY StudClass.studentid, StudClass.classid, StudClass.NAME, ses.checkintime, ses.checkouttime";
                //strQuery = "Select s.StudentId,s.StudentLname+'  '+s.StudentFname+'-'+s.StudentNbr+'   '+'('+c.ClassName+')' as name,c.ClassId,chkStatus = CASE Ss.CheckStatus WHEN 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END   from Student s Inner Join StdtClass sc on s.StudentId=sc.StdtId Inner Join Class c on c.ClassId=sc.ClassId Left Join StdtSessEvent Ss on s.StudentId=Ss.StudentId AND c.ClassId =Ss.ClassId where s.ActiveInd='A' AND sc.ActiveInd='A' AND (EventType='CH' OR c.ResidenceInd='" + hidSetVal.Value + "') And s.StudentLname like'%" + txtSearch.Text + "%'  ";

                strQuery = @"
                ;WITH EventsOnDate AS
                (
                    SELECT 
                        se.StudentId,
                        se.ClassId,
                        se.CheckinTime,
                        se.CheckoutTime,
                        se.CheckStatus,
                        se.AttendanceCode,
                        se.CreatedOn,
                        se.ModifiedOn,
                        ISNULL(se.IsPresent, 0) AS IsPresent,
                        ROW_NUMBER() OVER (
                            PARTITION BY se.StudentId, se.ClassId
                            ORDER BY se.CreatedOn ASC, se.ModifiedOn ASC
                        ) AS rn
                    FROM StdtSessEvent se
                    WHERE se.EventType = 'CH'
                      " + eventsDateCondition + @"
                      AND se.SchoolId = '" + sess.SchoolId + @"'
                ),
                FirstEvent AS
                (
                    SELECT StudentId, ClassId, CheckinTime, CheckoutTime, CheckStatus, AttendanceCode, CreatedOn, ModifiedOn, IsPresent
                    FROM EventsOnDate
                    WHERE rn = 1
                ),
                Extras AS
                (
                    SELECT et.StudentId, et.ClassId,
                           STUFF((
                               SELECT ';' +
                                      ISNULL(CONVERT(varchar(5), e2.CheckinTime, 108),'') + '|' +
                                      ISNULL(CONVERT(varchar(5), e2.CheckoutTime,108),'') + '|' +
                                      ISNULL(CAST(e2.AttendanceCode AS varchar(10)),'')
                               FROM EventsOnDate e2
                               WHERE e2.StudentId = et.StudentId
                                 AND e2.ClassId = et.ClassId
                                 AND e2.rn > 1
                                 AND (
                                     e2.CheckinTime IS NOT NULL
                                     OR e2.CheckoutTime IS NOT NULL
                                     OR e2.AttendanceCode IS NOT NULL
                                 )
                               ORDER BY e2.CreatedOn ASC, e2.ModifiedOn ASC
                               FOR XML PATH(''), TYPE
                           ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS ExtraList
                    FROM EventsOnDate et
                    GROUP BY et.StudentId, et.ClassId
                )

                SELECT
                    sc.StudentId AS studentid,
                    sc.Name,
                    sc.ClassId AS classid,
                    CASE WHEN EXISTS (
                         SELECT 1 FROM StdtSessEvent x
                         WHERE x.StudentId = sc.StudentId AND x.ClassId = sc.ClassId
                           AND x.EventType = 'CH' " + presenceDateCondition + @"
                           AND x.CheckStatus = 'True' AND x.SchoolId = '" + sess.SchoolId + @"'
                    ) THEN 1 ELSE 0 END AS chkStatus,
                    fe.CheckinTime AS InTime,
                    fe.CheckoutTime AS OutTime,
                    ISNULL(CASE WHEN fe.IsPresent = 1 THEN 'True' ELSE 'False' END, 'False') AS IsPresent,
                    ISNULL(extras.ExtraList, '') AS Extras,
                    COALESCE(fe.AttendanceCode, NULL) AS AttendanceCode
                FROM
                (
                    SELECT s.StudentId,
                           s.StudentLname + '  ' + s.StudentFname + '-' + s.StudentNbr + '   ' + '(' + c.ClassName + ')' AS Name,
                           c.ClassId
                    FROM Student s
                    INNER JOIN StdtClass sc2 ON s.StudentId = sc2.StdtId
                    INNER JOIN Class c ON c.ClassId = sc2.ClassId
                    INNER JOIN Placement plc 
                        ON s.StudentId = plc.StudentPersonalId 
                       AND plc.Location = sc2.ClassId
                       AND plc.Status = 1
                       " + placementDateCondition + @"
                    WHERE s.ActiveInd = 'A'
                      AND sc2.ActiveInd = 'A'
                      --AND (c.ResidenceInd = '" + safeHidVal + @"' " + DayNRes + @")
                      " + locationFilter + @"
                      " + studentFilter + @"
                ) sc
                LEFT JOIN FirstEvent fe ON fe.StudentId = sc.StudentId AND fe.ClassId = sc.ClassId
                LEFT JOIN Extras extras ON extras.StudentId = sc.StudentId AND extras.ClassId = sc.ClassId
                ORDER BY
                    CASE WHEN fe.CreatedOn IS NULL THEN 1 ELSE 0 END,
                    fe.CreatedOn ASC;
                ";
                            }
                            else
                            {
                                strQuery = @"
                ;WITH EventsToday AS
                (
                    -- CH events for today (CreatedOn date) for this school
                    SELECT 
                        se.StudentId,
                        se.ClassId,
                        se.CheckinTime,
                        se.CheckoutTime,
                        se.CheckStatus,
                        se.AttendanceCode,
                        se.CreatedOn,
                        se.ModifiedOn,
                        ISNULL(se.IsPresent, 0) AS IsPresent,
                        ROW_NUMBER() OVER (
                            PARTITION BY se.StudentId, se.ClassId
                            ORDER BY se.CreatedOn ASC, se.ModifiedOn ASC
                        ) AS rn
                    FROM StdtSessEvent se
                    WHERE se.EventType = 'CH'
                " + eventsDateCondition + @"
                      AND se.SchoolId = '" + sess.SchoolId + @"'
                ),
                FirstEvent AS
                (
                    SELECT StudentId, ClassId, CheckinTime, CheckoutTime, CheckStatus, AttendanceCode, CreatedOn, ModifiedOn, IsPresent
                    FROM EventsToday
                    WHERE rn = 1
                ),
                Extras AS
                (
                    SELECT et.StudentId, et.ClassId,
                           STUFF((
                               SELECT ';' +
                                      ISNULL(CONVERT(varchar(5), e2.CheckinTime, 108),'') + '|' +
                                      ISNULL(CONVERT(varchar(5), e2.CheckoutTime,108),'') + '|' +
                                      ISNULL(CAST(e2.AttendanceCode AS varchar(10)),'')
                               FROM EventsToday e2
                               WHERE e2.StudentId = et.StudentId
                                 AND e2.ClassId = et.ClassId
                                 AND e2.rn > 1
                                 AND (
                                     e2.CheckinTime IS NOT NULL
                                     OR e2.CheckoutTime IS NOT NULL
                                     OR e2.AttendanceCode IS NOT NULL
                                 )
                               ORDER BY e2.CreatedOn ASC, e2.ModifiedOn ASC
                               FOR XML PATH(''), TYPE
                           ).value('.', 'NVARCHAR(MAX)'), 1, 1, '') AS ExtraList
                    FROM EventsToday et
                    GROUP BY et.StudentId, et.ClassId
                )

                SELECT
                    sc.StudentId AS studentid,
                    sc.Name,
                    sc.ClassId AS classid,
                    CASE WHEN EXISTS (
                         SELECT 1 FROM StdtSessEvent x
                         WHERE x.StudentId = sc.StudentId
                           AND x.ClassId = sc.ClassId
                           AND x.EventType = 'CH' " + presenceDateCondition + @"
                           AND x.CheckStatus = 'True'
                           AND x.SchoolId = '" + sess.SchoolId + @"'
                    ) THEN 1 ELSE 0 END AS chkStatus,
                    fe.CheckinTime AS InTime,
                    fe.CheckoutTime AS OutTime,
                    ISNULL(CASE WHEN fe.IsPresent = 1 THEN 'True' ELSE 'False' END, 'False') AS IsPresent,
                    ISNULL(extras.ExtraList, '') AS Extras,
                    COALESCE(fe.AttendanceCode, NULL) AS AttendanceCode
                FROM
                (
                    -- include students who have an active placement (plc.EndDate IS NULL)
                    -- and whose class matches the residence filter
                    SELECT s.StudentId,
                           s.StudentLname + '  ' + s.StudentFname + '-' + s.StudentNbr + '   ' + '(' + c.ClassName + ')' AS Name,
                           c.ClassId
                    FROM Student s
                    INNER JOIN StdtClass sc2 ON s.StudentId = sc2.StdtId
                    INNER JOIN Class c ON c.ClassId = sc2.ClassId
                    INNER JOIN Placement plc 
                        ON s.StudentId = plc.StudentPersonalId 
                       AND plc.Location = sc2.ClassId
                       AND plc.EndDate IS NULL
                       AND plc.Status = 1
                       " + placementDateCondition + @"
                    WHERE s.ActiveInd = 'A'
                      AND sc2.ActiveInd = 'A'
                      --AND (c.ResidenceInd = '" + safeHidVal + @"' " + DayNRes + @")
                ) sc
                LEFT JOIN FirstEvent fe ON fe.StudentId = sc.StudentId AND fe.ClassId = sc.ClassId
                LEFT JOIN Extras extras ON extras.StudentId = sc.StudentId AND extras.ClassId = sc.ClassId
                ORDER BY Name ASC";
//                    CASE WHEN fe.CreatedOn IS NULL THEN 1 ELSE 0 END,
//                    fe.CreatedOn ASC;
//                ";
                //strQuery = "Select s.StudentId,s.StudentLname+'  '+s.StudentFname+'-'+s.StudentNbr+'   '+'('+c.ClassName+')' as name,c.ClassId,chkStatus = CASE Ss.CheckStatus WHEN 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END   from Student s Inner Join StdtClass sc on s.StudentId=sc.StdtId Inner Join Class c on c.ClassId=sc.ClassId Left Join StdtSessEvent Ss on s.StudentId=Ss.StudentId AND c.ClassId =Ss.ClassId  where s.ActiveInd='A' AND sc.ActiveInd='A' AND (EventType='CH' OR c.ResidenceInd='" + hidSetVal.Value + "')";
            }
            DataTable Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {
                    grdGroup.DataSource = Dt;
                    grdGroup.DataBind();
                }
                else
                {
                    grdGroup.DataSource = Dt;
                    grdGroup.DataBind();
                }

                try
                {
                    if (upGrid != null)
                    {
                        upGrid.Update();
                    }
                }
                catch { /* ignore if panel not available in some contexts */ }
            }
        }
        catch (Exception ex)
        {
            //Null Reference Log
            if (ex.ToString().Contains("System.NullReferenceException:"))
            {
                int studId = -1;
                string nullVariable = "None";
                
                if (sess != null)
                    studId = sess.StudentId;
                
                if (sess == null)
                    nullVariable = "sess";

                else if (objData == null)
                    nullVariable = "ObjData";

                else if (hidSetVal == null)
                    nullVariable = "hidSetVal";

                else if (hidSetVal.Value == null)
                    nullVariable = "hidSetVal.Value";

                //else if (txtSearch == null)
                //    nullVariable = "txtSearch";

                string errorLogFilePath = HttpContext.Current.Server.MapPath("~/ErrorLog/log.txt");
                string errorLogMessage = string.Format("[{0}]\nError: {1}\n{2}\n{3}\n{4}\n{5}",
                DateTime.Now,"StudentCheckin Null Reference Log", ex.Message, "Null Variable = " + nullVariable,"StudentId = " + studId, Environment.NewLine);
                File.AppendAllText(errorLogFilePath, errorLogMessage);
            }
            throw ex;
        }
    }
    private void checkSearch()
    {
        //if (txtSearch.Text == "")
        //{
        //    fillStudent(hidSetVal.Value.ToString(), false);
        //}
        //else
        //{
        //    fillStudent(hidSetVal.Value.ToString(), true);
        //}
    }
    protected void imgBDay_Click(object sender, ImageClickEventArgs e)
    {
        //day.Style["background-color"] = "#E3EAEB";
        //imgBDay.ImageUrl = "~/StudentBinder/img/DayB.png";
        //ImgBRes.ImageUrl = "~/StudentBinder/img/ResG.png";
        //res.Style["background-color"] = "#ddd";
        hidSetVal.Value = "0";
        //txtSearch.Text = "";
        checkSearch();
    }
    protected void ImgBRes_Click(object sender, ImageClickEventArgs e)
    {
        //imgBDay.ImageUrl = "~/StudentBinder/img/DayG.png";
        //ImgBRes.ImageUrl = "~/StudentBinder/img/ResB.png";
        //res.Style["background-color"] = "#E3EAEB";
        //day.Style["background-color"] = "#ddd";
        hidSetVal.Value = "1";
        //txtSearch.Text = "";
        checkSearch();
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        ApplySearchAndBind();
    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        ApplySearchAndBind();
    }

    private void ApplySearchAndBind()
    {
        //string q = (txtSearch.Text ?? "").Trim();
        //hidSearch.Value = string.IsNullOrEmpty(q) ? "0" : "1";

        grdGroup.PageIndex = 0;

        var sm = ScriptManager.GetCurrent(Page);
        string js = "showLoader('Searching…');";
        if (sm != null && sm.IsInAsyncPostBack)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showLoader", js, true);
        else
            ClientScript.RegisterStartupScript(Page.GetType(), "showLoader", js, true);

        if (hidSearch.Value == "0")
        {
            fillStudent(hidSetVal.Value.ToString(), false);
        }
        else
        {
            fillStudent(hidSetVal.Value.ToString(), true);
        }

        js = "if(window.hideLoader) hideLoader();";
        if (sm != null && sm.IsInAsyncPostBack)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", js, true);
        else
            ClientScript.RegisterStartupScript(Page.GetType(), "hideLoader", js, true);
    }

    private void ShowGridBand(string message, int milliseconds, string kind)
    {
        if (milliseconds <= 0) milliseconds = 4000;
        if (string.IsNullOrEmpty(kind)) kind = "error";

        string safe = (message ?? string.Empty)
            .Replace("\\", "\\\\").Replace("'", "\\'")
            .Replace("\r", "\\r").Replace("\n", "\\n");

        // This script ensures a fallback showGridBand exists and the #gridBand element exists,
        // then calls it. It also falls back to alert() if anything unexpected happens.
        string js = @"
        (function(){
            try {
                // ensure DOM helper
                function ensureGridBand() {
                    var band = document.getElementById('gridBand');
                    if (!band) {
                        band = document.createElement('div');
                        band.id = 'gridBand';
                        band.style.display = 'none';
                        band.style.position = 'fixed';
                        band.style.top = '6px';
                        band.style.left = '6px';
                        band.style.right = '6px';
                        band.style.zIndex = '999999';
                        band.style.padding = '8px 12px';
                        band.style.borderRadius = '6px';
                        band.style.font = '14px/1.3 Arial, Helvetica, sans-serif';
                        document.body.insertBefore(band, document.body.firstChild);
                    }
                    // message span
                    var txt = document.getElementById('gridBandMsg');
                    if (!txt) {
                        txt = document.createElement('span');
                        txt.id = 'gridBandMsg';
                        band.appendChild(txt);
                    }
                    return band;
                }

                // provide a safe showGridBand if missing
                if (typeof window.showGridBand !== 'function') {
                    window.showGridBand = function(msg, ms, k) {
                        try {
                            var b = ensureGridBand();
                            // style by kind
                            if (k === 'success') {
                                b.style.background = '#16a34a'; b.style.color = '#fff';
                            } else if (k === 'error') {
                                b.style.background = '#dc2626'; b.style.color = '#fff';
                            } else {
                                b.style.background = '#333'; b.style.color = '#fff';
                            }
                            var span = document.getElementById('gridBandMsg');
                            if (span) span.textContent = msg || '';
                            b.style.display = 'block';
                            // remove after ms
                            setTimeout(function(){ try{ b.style.display = 'none'; }catch(e){} }, (ms||4000));
                        } catch (e) {
                            try { alert(msg); } catch (ex) {}
                        }
                    };
                }

                // finally call the function
                window.showGridBand('" + safe + @"', " + milliseconds.ToString() + @", '" + kind + @"');
            } catch (e) {
                try { alert('" + safe + @"'); } catch (ex) {}
            }
        })();
        ";

        var sm = ScriptManager.GetCurrent(Page);
        string key = "gridBandMsg" + Guid.NewGuid().ToString("N");
        if (sm != null && sm.IsInAsyncPostBack)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, js, true);
        else
            ClientScript.RegisterStartupScript(Page.GetType(), key, js, true);
    }

    private void ShowGridError(string msg, int ms = 4000) { ShowGridBand(msg, ms, "error"); }
    private void ShowGridSaved(string msg = "Saved", int ms = 2500) { ShowGridBand(msg, ms, "success"); }

    protected void repRows_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Example: find controls if you need to modify them dynamically
            TextBox txtInTime = (TextBox)e.Item.FindControl("txtInTime");
            TextBox txtOutTime = (TextBox)e.Item.FindControl("txtOutTime");
            DropDownList ddlCode = (DropDownList)e.Item.FindControl("ddlCode");

            // Example: set tooltips or defaults
            if (txtInTime != null)
                txtInTime.ToolTip = "Enter time in HH:mm format";

            // If you have extra logic for each row, put it here.
        }
    }

    protected void btnEditPast_Click(object sender, EventArgs e)
    {
        // Example: Redirect or show message
        // You can later replace this with your actual logic
        Response.Write("<script>alert('Edit Past Dates clicked');</script>");
    }

    protected string BindTime(object val)
    {
        if (val == null || val == DBNull.Value) return String.Empty;

        // If stored as DateTime
        DateTime dt;
        if (DateTime.TryParse(val.ToString(), out dt))
            return dt.ToString("HH:mm"); // 24-hour: "08:30"
        // otherwise return raw string (hoping it's already "HH:mm")
        return val.ToString();
    }

    protected bool ResolveStatus(object isPresentVal)
    {
        if (isPresentVal == null || isPresentVal == DBNull.Value) return false;

        // Handle multiple data formats (bool, string, int)
        if (isPresentVal is bool)
            return (bool)isPresentVal;

        string s = isPresentVal.ToString().Trim();
        return s == "1" || s.Equals("Y", StringComparison.OrdinalIgnoreCase)
            || s.Equals("Present", StringComparison.OrdinalIgnoreCase)
            || s.Equals("True", StringComparison.OrdinalIgnoreCase);
    }

    protected string GetSwitchClass(object isPresentVal)
    {
        return ResolveStatus(isPresentVal) ? "att-sw-present" : "att-sw-absent";
    }
    protected string GetStatusHidden(object isPresentVal)
    {
        if (isPresentVal == null || isPresentVal == DBNull.Value) return "0";
        if (isPresentVal is bool) return ((bool)isPresentVal) ? "1" : "0";
        string s = isPresentVal.ToString().Trim();
        if (s == "1" || s.Equals("True", StringComparison.OrdinalIgnoreCase) || s.Equals("Present", StringComparison.OrdinalIgnoreCase))
            return "1";
        return "0";
    }

    // helper to format time values to HH:mm (returns empty string for null/DBNULL)
    protected string FormatToHHmm(object val)
    {
        if (val == null || val == DBNull.Value) return String.Empty;
        DateTime dt;
        if (DateTime.TryParse(val.ToString(), out dt)) return dt.ToString("HH:mm");
        return val.ToString();
    }

    private DateTime? ParseTimeToToday(string hhmm)
    {
        if (String.IsNullOrWhiteSpace(hhmm)) return null;
        TimeSpan ts;
        if (TimeSpan.TryParse(hhmm.Trim(), out ts))
        {
            return DateTime.Today.Add(ts);
        }
        // Try forgiving parse as DateTime if user provided full timestamp
        DateTime dt;
        if (DateTime.TryParse(hhmm.Trim(), out dt))
        {
            // normalize to today's date + time component
            var timeOnly = dt.TimeOfDay;
            return DateTime.Today.Add(timeOnly);
        }
        return null;
    }

    private DataTable GetAttendanceCodeLookup()
    {
        // Cache in ViewState for the request; you can use Session or static cache if preferred.
        var dt = ViewState["AttendanceLookupDT"] as DataTable;
        if (dt != null) return dt;

        objData = new clsData();
        string sql = "SELECT LookupId, LookupName, LookupCode FROM LookUp " +
                     "WHERE LookupType = 'AttendanceCode' AND ActiveInd = 'A' " +
                     "ORDER BY SortOrder, LookupName";
        dt = objData.ReturnDataTable(sql, false) ?? new DataTable();
        ViewState["AttendanceLookupDT"] = dt;
        return dt;
    }
    private class TimePair
    {
        public DateTime? In { get; set; }
        public DateTime? Out { get; set; }
        public string Code { get; set; }
        public bool? IsPresent { get; set; }
    }

    private void SaveOrderedPairs(int studentId, int classId, int schoolId, int userId, List<TimePair> allPairs, bool deleteExtraDbRows = false, DateTime? targetDate = null)
    {
        if (allPairs == null) allPairs = new List<TimePair>();
        objData = new clsData();

        string dateCondition;
        if (targetDate.HasValue)
        {
            string iso = targetDate.Value.ToString("yyyy-MM-dd");
            dateCondition = "AND CreatedOn >= CAST('" + iso + "' AS date) AND CreatedOn < DATEADD(day,1,CAST('" + iso + "' AS date))";
        }
        else
        {
            dateCondition = "AND CreatedOn >= CAST(GETDATE() AS date) AND CreatedOn < DATEADD(day,1,CAST(GETDATE() AS date))";
        }

        string sqlSelect = @"
        SELECT StdtSessEventId, CheckinTime, CheckoutTime, CreatedOn, ModifiedOn
        FROM StdtSessEvent
        WHERE SchoolId = " + schoolId + @"
          AND StudentId = " + studentId + @"
          AND ClassId = " + classId + @"
          AND EventType = 'CH'
          " + dateCondition + @"
        ORDER BY CreatedOn ASC, ModifiedOn ASC
        ";

        DataTable existing = objData.ReturnDataTable(sqlSelect, false) ?? new DataTable();

        // iterate ordered pairs and update or insert, while saving Code and IsPresent
        for (int i = 0; i < allPairs.Count; i++)
        {
            var p = allPairs[i];

            string checkinSql = p.In.HasValue ? "'" + p.In.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL";
            string checkoutSql = p.Out.HasValue ? "'" + p.Out.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'" : "NULL";

            // determine CheckStatus logic (same as earlier)
            string checkStatus = (p.Out.HasValue && !p.In.HasValue) ? "False" : "True";

            // Prefer explicit posted IsPresent (from hidStatus) if available; otherwise fall back to checkStatus rule
            string isPresentSql;
            if (p.IsPresent.HasValue)
            {
                isPresentSql = p.IsPresent.Value ? "1" : "0";
            }
            else
            {
                // fallback: if OUT exists but IN missing, treat as absent (False); otherwise present
                isPresentSql = (p.Out.HasValue && !p.In.HasValue) ? "0" : "1";
            }


            var exemptCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "LOA", "SICK" };

            // determine whether current pair's code should exempt validation
            bool isExemptCode = false;
            try
            {
                // Get lookup once (GetAttendanceCodeLookup is available in this class)
                var lookupDt = GetAttendanceCodeLookup();

                if (lookupDt != null && !string.IsNullOrEmpty(p.Code))
                {
                    // 1) If p.Code is numeric it might be a LookupId
                    int parsedId;
                    if (int.TryParse(p.Code, out parsedId))
                    {
                        DataRow[] rows = lookupDt.Select("LookupId = " + parsedId);
                        if (rows != null && rows.Length > 0)
                        {
                            string lookupCode = lookupDt.Columns.Contains("LookupCode") ? Convert.ToString(rows[0]["LookupCode"]) : "";
                            string lookupName = Convert.ToString(rows[0]["LookupName"]);
                            if (exemptCodes.Contains(lookupCode) || exemptCodes.Contains(lookupName))
                                isExemptCode = true;
                        }
                    }
                    else
                    {
                        // 2) p.Code might already be the LookupCode or LookupName; try matching both
                        // try LookupCode match first (if column exists)
                        if (lookupDt.Columns.Contains("LookupCode"))
                        {
                            DataRow[] rows = lookupDt.Select("LookupCode = '" + p.Code.Replace("'", "''") + "'");
                            if (rows != null && rows.Length > 0)
                            {
                                string lookupCode = Convert.ToString(rows[0]["LookupCode"]);
                                string lookupName = Convert.ToString(rows[0]["LookupName"]);
                                if (exemptCodes.Contains(lookupCode) || exemptCodes.Contains(lookupName))
                                    isExemptCode = true;
                            }
                        }

                        // fallback: match by LookupName
                        if (!isExemptCode)
                        {
                            DataRow[] rows2 = lookupDt.Select("LookupName = '" + p.Code.Replace("'", "''") + "'");
                            if (rows2 != null && rows2.Length > 0)
                            {
                                string lookupCode = lookupDt.Columns.Contains("LookupCode") ? Convert.ToString(rows2[0]["LookupCode"]) : "";
                                string lookupName = Convert.ToString(rows2[0]["LookupName"]);
                                if (exemptCodes.Contains(lookupCode) || exemptCodes.Contains(lookupName))
                                    isExemptCode = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                // If lookup fails for any reason, default to not exempting (safe behavior).
                isExemptCode = false;
            }

            if (!isExemptCode)
            {
                if (p.In.HasValue && p.Out.HasValue && p.Out.Value < p.In.Value)
                {
                    p.Out = null;
                    ShowGridError("OUT time cannot be earlier than IN time.");
                    return;
                }
            }
            else
            {
                // optional: if you want to clear times for LOA/SICK automatically:
                // p.In = null; p.Out = null;
            }

            // sanitize code and prepare SQL fragment for AttendanceCode
            string codeSql = "NULL";
            int codeValue = 0; // default to 0 if null or parse fails

            if (!string.IsNullOrEmpty(p.Code))
            {
                // basic SQL escaping of single quotes
                string safe = p.Code.Replace("'", "''");
                codeSql = "'" + safe + "'";

                // try to parse numeric code from p.Code
                if (!int.TryParse(p.Code, out codeValue))
                    codeValue = 0; // fallback to 0 if not numeric
            }
            else
            {
                codeSql = "NULL";
                codeValue = 0;
            }

            if (i < existing.Rows.Count)
            {
                // UPDATE existing row at same position
                var pk = existing.Rows[i]["StdtSessEventId"];
                string pkVal = Convert.ToString(pk);

                // Use numeric AttendanceCode when parsed; otherwise NULL or quoted string if required
                string attendanceSqlFragment = (codeValue > 0) ? codeValue.ToString() : "NULL";

                string sqlUpdate = @"
                UPDATE StdtSessEvent
                SET CheckinTime = " + checkinSql + @",
                    CheckoutTime = " + checkoutSql + @",
                    CheckStatus = '" + checkStatus + @"',
                    IsPresent = " + isPresentSql + @",
                    AttendanceCode = " + attendanceSqlFragment + @",
                    ModifiedOn = GETDATE()
                WHERE StdtSessEventId = " + pkVal + @"
            ";
                objData.Execute(sqlUpdate);
            }
            else
            {
                // INSERT a new row, include IsPresent and AttendanceCode
                string attendanceInsertValue = (codeValue > 0) ? codeValue.ToString() : "NULL";

                string createdOnValueSql = "GETDATE()";
                if (targetDate.HasValue)
                {
                    createdOnValueSql = "CAST('" + targetDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' AS datetime)";
                }

                string sqlInsert = @"
                INSERT INTO StdtSessEvent
                    (SchoolId, StudentId, EvntTs, CheckStatus, IsPresent, ClassId, CreatedBy, CreatedOn, ModifiedOn, EventType, CheckinTime, CheckoutTime, AttendanceCode)
                VALUES
                    (" + schoolId + ", " + studentId + ", GETDATE(), '" + checkStatus + "', " + isPresentSql + ", " + classId + ", " + userId + ", " + createdOnValueSql + ", GETDATE(), 'CH', " + checkinSql + ", " + checkoutSql + ", " + attendanceInsertValue + @")
            ";
                objData.Execute(sqlInsert);
            }
        }

        // Optionally delete DB rows beyond UI count (if user removed extras)
        if (existing.Rows.Count > allPairs.Count && deleteExtraDbRows)
        {
            for (int j = allPairs.Count; j < existing.Rows.Count; j++)
            {
                var pk = existing.Rows[j]["StdtSessEventId"];
                string pkVal = Convert.ToString(pk);
                string sqlDel = "DELETE FROM StdtSessEvent WHERE StdtSessEventId = " + pkVal;
                objData.Execute(sqlDel);
            }
        }

        if (targetDate.HasValue)
        {
            try
            {
                if (hidPastDate != null)
                {
                    hidPastDate.Value = targetDate.Value.ToString("yyyy-MM-dd");
                }
            }
            catch { /* ignore if control missing */ }
        }
        else
        {
            // clear if no targetDate (optional)
            try { if (hidPastDate != null) hidPastDate.Value = ""; }
            catch { }
        }

        // Rebind grid to reflect the new data
        try
        {
            fillStudent(hidSetVal.Value.ToString(), false);
        }
        catch { /* ignore rebind exceptions here */ }
    }

    private List<TimePair> ParseEncodedExtraTimes(string encoded)
    {
        var list = new List<TimePair>();
        if (String.IsNullOrWhiteSpace(encoded)) return list;

        try
        {
            // Split by ';' for each time pair
            var items = encoded.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var it in items)
            {
                var parts = it.Split(new[] { '|' });
                string inS = parts.Length > 0 ? HttpUtility.UrlDecode(parts[0]) : "";
                string outS = parts.Length > 1 ? HttpUtility.UrlDecode(parts[1]) : "";
                string code = parts.Length > 2 ? HttpUtility.UrlDecode(parts[2]) : "";

                DateTime? inTime = ParseTimeToToday(inS);
                DateTime? outTime = ParseTimeToToday(outS);

                list.Add(new TimePair
                {
                    In = inTime,    // now DateTime?, not string
                    Out = outTime,  // now DateTime?, not string
                    Code = code
                });
            }
        }
        catch (Exception ex)
        {
            // log or safely ignore bad entries
            System.Diagnostics.Debug.WriteLine("ParseEncodedExtraTimes error: " + ex.Message);
        }

        return list;
    }

    private void BindLocationDropdown()
    {
        objData = new clsData();
        string sql = @"
        SELECT DISTINCT c.ClassId AS LocationId, c.ClassName AS LocationName
        FROM Student s
        INNER JOIN StdtClass sc ON s.StudentId = sc.StdtId
        INNER JOIN Class c ON c.ClassId = sc.ClassId
        INNER JOIN Placement p ON p.StudentPersonalId = s.StudentId
                           AND p.Location = c.ClassId
                           AND p.EndDate IS NULL
                           AND p.Status = 1
        WHERE s.ActiveInd = 'A' AND sc.ActiveInd = 'A' AND c.ActiveInd = 'A'
        ORDER BY c.ClassName, c.ClassId;
    ";

        DataTable dt = objData.ReturnDataTable(sql, false);

        ddlLocation.Items.Clear();
        ddlLocation.Items.Add(new ListItem("--- All Locations ---", ""));
        if (dt != null)
        {
            foreach (DataRow r in dt.Rows)
                ddlLocation.Items.Add(new ListItem(Convert.ToString(r["LocationName"]), Convert.ToString(r["LocationId"])));
        }
    }

    private void BindLocationDropdown(string clientId)
    {
        string filter = "";
        int sid;
        objData = new clsData();
        if (!string.IsNullOrEmpty(clientId) && int.TryParse(clientId, out sid))
        {
            filter = " AND s.StudentId = " + sid.ToString();
        }

        string sql = @"
        SELECT DISTINCT c.ClassId AS LocationId, c.ClassName AS LocationName
        FROM Student s
        INNER JOIN StdtClass sc ON s.StudentId = sc.StdtId
        INNER JOIN Class c ON c.ClassId = sc.ClassId
        INNER JOIN Placement p ON p.StudentPersonalId = s.StudentId
                               AND p.Location = c.ClassId
                               AND p.EndDate IS NULL
                               AND p.Status = 1
        WHERE s.ActiveInd = 'A' AND sc.ActiveInd = 'A' AND c.ActiveInd = 'A'
        " + filter + @"
        ORDER BY c.ClassName, c.ClassId;
    ";

        DataTable dt = objData.ReturnDataTable(sql, false);

        ddlLocation.Items.Clear();
        ddlLocation.Items.Add(new ListItem("--- All Locations ---", ""));
        if (dt != null)
        {
            var added = new HashSet<string>();
            foreach (DataRow r in dt.Rows)
            {
                string lid = Convert.ToString(r["LocationId"]);
                if (added.Contains(lid)) continue;
                added.Add(lid);
                ddlLocation.Items.Add(new ListItem(Convert.ToString(r["LocationName"]), lid));
            }
        }
    }
    private void BindClientDropdown()
    {
        objData = new clsData();
        string sql = @"
        SELECT DISTINCT
            c.ClassId,
            c.ClassName,
            s.StudentId,
            (s.StudentLname + ' ' + s.StudentFname) AS StudentName
        FROM Student s
        INNER JOIN StdtClass sc ON s.StudentId = sc.StdtId
        INNER JOIN Class c ON c.ClassId = sc.ClassId
        INNER JOIN Placement p ON p.StudentPersonalId = s.StudentId
                               AND p.Location = c.ClassId
                               AND p.EndDate IS NULL
                               AND p.Status = 1
        WHERE s.ActiveInd = 'A'
          AND sc.ActiveInd = 'A'
          AND c.ActiveInd = 'A'
        ORDER BY c.ClassId, s.StudentId;
    ";

        DataTable dt = objData.ReturnDataTable(sql, false);

        ddlClient.Items.Clear();
        ddlClient.Items.Add(new ListItem("-- All Students --", "")); // optional
        if (dt != null)
        {
            // as query returns class+student rows, just add unique students
            var added = new HashSet<string>();
            foreach (DataRow r in dt.Rows)
            {
                string sid = Convert.ToString(r["StudentId"]);
                if (added.Contains(sid)) continue;
                added.Add(sid);

                ddlClient.Items.Add(new ListItem(
                    Convert.ToString(r["StudentName"]),
                    sid
                ));
            }
        }
    }
    private void BindClientDropdown(string locationId)
    {
        string filter = "";
        int loc;
        objData = new clsData();
        if (!string.IsNullOrEmpty(locationId) && int.TryParse(locationId, out loc))
        {
            filter = " AND c.ClassId = " + loc.ToString();
        }

        string sql = @"
        SELECT DISTINCT
            s.StudentId,
            (s.StudentLname + ' ' + s.StudentFname) AS StudentName
        FROM Student s
        INNER JOIN StdtClass sc ON s.StudentId = sc.StdtId
        INNER JOIN Class c ON c.ClassId = sc.ClassId
        INNER JOIN Placement p ON p.StudentPersonalId = s.StudentId
                               AND p.Location = c.ClassId
                               AND p.EndDate IS NULL
                               AND p.Status = 1
        WHERE s.ActiveInd = 'A' AND sc.ActiveInd = 'A' AND c.ActiveInd = 'A'
        " + filter + @"
        ORDER BY StudentName, StudentId;
    ";

        DataTable dt = objData.ReturnDataTable(sql, false);

        ddlClient.Items.Clear();
        ddlClient.Items.Add(new ListItem("-- All Clients --", ""));
        if (dt != null)
        {
            var added = new HashSet<string>();
            foreach (DataRow r in dt.Rows)
            {
                string sid = Convert.ToString(r["StudentId"]);
                if (added.Contains(sid)) continue;
                added.Add(sid);
                ddlClient.Items.Add(new ListItem(Convert.ToString(r["StudentName"]), sid));
            }
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedClass = ddlLocation.SelectedValue;
        string selectedClient = ddlClient.SelectedValue;
        if(selectedClient=="")
        BindClientDropdown(selectedClass);

        string param = (hidSetVal != null) ? (hidSetVal.Value ?? "") : "";
        fillStudent(param, true);
    }
    protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedClient = ddlClient.SelectedValue;
        string selectedClass = ddlLocation.SelectedValue;
        if(selectedClass=="")
        BindLocationDropdown(selectedClient);

        string param = (hidSetVal != null) ? (hidSetVal.Value ?? "") : "";
        fillStudent(param, true);
    }

    protected void btnCloseCalendar_Click(object sender, EventArgs e)
    {
        pnlCalendar.Style["display"] = "none";
        if (upCalendar != null) upCalendar.Update();
    }

    protected void btnEditPastDates_Click(object sender, EventArgs e)
    {
        try
        {
            // Register calendar for async month navigation
            var sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null) sm.RegisterAsyncPostBackControl(calPast);

            // Prefer the pnlCalendar inside upCalendar — do NOT use the page-level pnlCalendar if a layout one exists
            Panel targetPanel = null;
            if (upCalendar != null)
            {
                var found = upCalendar.FindControl("pnlCalendar");
                if (found != null && found is Panel) targetPanel = (Panel)found;
            }

            // If not found inside upCalendar, only then fallback to page-level reference;
            // ideally you should ensure markup has pnlCalendar inside upCalendar so this fallback is rarely used.
            if (targetPanel == null) targetPanel = this.pnlCalendar;

            if (targetPanel == null)
            {
                System.Diagnostics.Debug.WriteLine("btnEditPastDates_Click: pnlCalendar not found.");
                return;
            }

            // Show and prepare the *preferred* panel
            targetPanel.Style["display"] = "block";
            targetPanel.Style["visibility"] = "visible";
            // optionally set coordinates here if you need fixed placement
            // targetPanel.Style["left"] = "568px";
            // targetPanel.Style["top"] = "68px";

            // Update the UpdatePanel that contains that panel (very important)
            var parentUp = GetParentUpdatePanel(targetPanel);
            if (parentUp != null) parentUp.Update();
            else if (upCalendar != null) upCalendar.Update(); // defensive

            string js = "setTimeout(function(){ try{ if(typeof showCalendarPopup==='function') showCalendarPopup(); }catch(e){} }, 40);";
            ScriptManager.RegisterStartupScript(upCalendar, upCalendar.GetType(), "showPnlCalendar", js, true);
            // Pause client clock if relevant
            ScriptManager.RegisterStartupScript(this, this.GetType(), "pauseClock", "allowClockUpdate = false;", true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("btnEditPastDates_Click error: " + ex.ToString());
        }
    }

    private Control FindControlRecursive(Control root, string id)
    {
        if (root == null) return null;
        var c = root.FindControl(id);
        if (c != null) return c;
        foreach (Control child in root.Controls)
        {
            var found = FindControlRecursive(child, id);
            if (found != null) return found;
        }
        return null;
    }

    protected void calPast_SelectionChanged(object sender, EventArgs e)
    {
        try
        {
            // show loader on client (defensive)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showLoaderOnDateSelect",
                "try{ if(window.showLoader) showLoader('Loading\\u2026'); }catch(e){}", true);

            // --- 1) Determine the selectedDate robustly ---
            DateTime selectedDate = DateTime.MinValue;
            string posted = null;

            // prefer server control selection
            if (calPast != null && calPast.SelectedDate != DateTime.MinValue)
            {
                selectedDate = calPast.SelectedDate.Date;
            }
            else
            {
                // fallback: posted hidden field
                posted = Request.Form["hidPastDate"] ?? (hidPastDate != null ? hidPastDate.Value : null);
                if (!String.IsNullOrEmpty(posted))
                {
                    DateTime tmp;
                    if (DateTime.TryParse(posted, out tmp)) selectedDate = tmp.Date;
                }
            }

            // final fallback: visible date or today
            if (selectedDate == DateTime.MinValue && calPast != null && calPast.VisibleDate != DateTime.MinValue)
                selectedDate = calPast.VisibleDate.Date;
            if (selectedDate == DateTime.MinValue) selectedDate = DateTime.Today;

            // prevent future dates
            if (selectedDate > DateTime.Today)
            {
                // hide loader before showing error
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoaderOnInvalidDate",
                    "try{ if(window.hideLoader) hideLoader(); }catch(e){}", true);
                ShowGridError("Please select a valid past date.");
                return;
            }

            // --- 2) Persist selected date to hidden field so fillStudent uses it ---
            string iso = (selectedDate.Date == DateTime.Today) ? "" : selectedDate.ToString("yyyy-MM-dd");
            if (hidPastDate != null) hidPastDate.Value = iso;

            // ensure calendar reflects selection
            if (calPast != null)
            {
                calPast.SelectedDate = selectedDate;
                calPast.VisibleDate = selectedDate;
                calPast.TodaysDate = DateTime.Today;
            }

            // --- 3) Bind grid for that date ---
            try
            {
                DateTime sel = calPast.SelectedDate;
                if (hidPastDate != null)
                {
                    if (sel.Date == DateTime.Today)
                        hidPastDate.Value = ""; // clear for today
                    else
                        hidPastDate.Value = sel.ToString("yyyy-MM-dd");
                }

                // existing logic: rebind grid for sel
                fillStudent(hidSetVal.Value.ToString(), false);

                // Update the UpdatePanel that contains the grid so client receives refreshed HTML.
                if (upGrid != null) upGrid.Update();
            }
            catch (Exception bindEx)
            {
                // make sure loader is hidden and report the error
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoaderOnBindError",
                    "try{ if(window.hideLoader) hideLoader(); }catch(e){}", true);
                System.Diagnostics.Trace.WriteLine("calPast_SelectionChanged -> fillStudent error: " + bindEx.ToString());
                ShowGridError("Error loading data: " + bindEx.Message);
                return;
            }

            // Ensure calendar UpdatePanel is also refreshed so client receives the updated pnlCalendar HTML
            try
            {
                if (upCalendar != null) upCalendar.Update();
            }
            catch { /* ignore if not present */ }

            // --- 4) Register client script to pause/resume clock ---
            bool isToday = (selectedDate.Date == DateTime.Today);
            string clientScript;
            if (isToday)
            {
                clientScript = @"
                (function(){
                    try {
                        window.allowClockUpdate = true;
                        if (typeof setHiddenPastDateIso === 'function') setHiddenPastDateIso('');
                        if (typeof refreshClockNow === 'function') refreshClockNow();
                    } catch(e){ console && console.log && console.log('resume clock error', e); }
                })();";
                            }
                            else
                            {
                                clientScript = @"
                (function(){
                    try {
                        window.allowClockUpdate = false;
                        if (typeof setHiddenPastDateIso === 'function') setHiddenPastDateIso('" + iso + @"');
                    } catch(e){ console && console.log && console.log('pause clock error', e); }
                })();";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "resumeOrPauseClock", clientScript, true);

            // --- 5) Cleanup script: prune duplicates, hide popup, clear client-visible flag, hide loader ---
            // Register against upCalendar so it runs when that panel is part of the response.
            string cleanupJs = @"
            (function(){
                try {
                    // re-detect/prune duplicates if available
                    if (window.findAndPruneDuplicates) try{ window.findAndPruneDuplicates(); }catch(e){}

                    // ensure any persisted 'visible' marker on the calendar is cleared (defensive)
                    try {
                        var c = document.getElementById('" + (pnlCalendar != null ? pnlCalendar.ClientID : "pnlCalendar") + @"');
                        if(c && c.dataset) c.dataset.calendarPersistVisible = '0';
                    } catch(e){}

                    // hide the popup (use the global function if present)
                    if (window.hideCalendarPopup) try{ window.hideCalendarPopup(); }catch(e){} 
                    else {
                        // fallback: directly hide known nodes
                        try {
                            var nodes = document.querySelectorAll('.calendar-popup, [id^=""pnlCalendar""]');
                            for(var i=0;i<nodes.length;i++){ var n = nodes[i]; if(n){ n.style.display='none'; n.style.visibility='hidden'; n.style.opacity='0'; n.style.pointerEvents='none'; } }
                        } catch(e){}
                    }

                    // hide loader
                    if (window.hideLoader) try{ window.hideLoader(); }catch(e){}

                    // re-enable Edit Past Dates button if it was disabled
                    try {
                        var b = document.getElementById('" + (btnEditPastDates != null ? btnEditPastDates.ClientID : "btnEditPastDates") + @"');
                        if (b) { b.disabled = false; b.removeAttribute && b.removeAttribute('disabled'); b.type = 'button'; }
                    } catch(e){}
                } catch(e){ console && console.log && console.log('cleanupAfterDateSelect', e); }
            })();";

            if (upCalendar != null)
                ScriptManager.RegisterStartupScript(upCalendar, upCalendar.GetType(), "cleanupAfterDateSelect", cleanupJs, true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cleanupAfterDateSelect", cleanupJs, true);
        }
        catch (Exception ex)
        {
            // ensure loader is hidden if something unexpected happens
            try { ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLoaderOnException", "try{ if(window.hideLoader) hideLoader(); }catch(e){}", true); }
            catch { }
            System.Diagnostics.Trace.WriteLine("calPast_SelectionChanged unexpected error: " + ex.ToString());
            ShowGridError("Unexpected error: " + ex.Message);
        }
    }

    private void EnsureCalendarPopupVisible(DateTime visibleDate)
    {
        try
        {
            // --- Update calendar state ---
            if (calPast != null)
            {
                calPast.VisibleDate = visibleDate.Date;
                calPast.TodaysDate = DateTime.Today;
            }

            // --- Keep popup visible server-side ---
            if (pnlCalendar != null)
            {
                pnlCalendar.Style["display"] = "block";
                // optional fixed position:
                pnlCalendar.Style["left"] = "568px";
                pnlCalendar.Style["top"] = "68px";
            }

            // --- Update only the calendar panel ---
            if (upCalendar != null)
                upCalendar.Update();

            // --- Ensure async postback registration ---
            var sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
                sm.RegisterAsyncPostBackControl(calPast);

            // --- Client script: keep same popup visible ---
            string js = @"
        try {
            var pnl = document.getElementById('" + pnlCalendar.ClientID + @"');
            if (pnl) {
                pnl.style.display = 'block';
                pnl.style.visibility = 'visible';
                pnl.style.opacity = '1';
            }
            var cal = document.getElementById('" + calPast.ClientID + @"');
            if (cal) { try { cal.focus(); } catch(e){} }
        } catch(e){ console && console.log && console.log('EnsureCalendarPopupVisible client script error', e); }";

            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "ensureCalendarPopupVisible", js, true
            );
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("EnsureCalendarPopupVisible error: " + ex.ToString());
        }
    }

    protected void calPast_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {
            DateTime newVisible = e.NewDate; // use e.NewDate
            calPast.VisibleDate = newVisible;
            calPast.TodaysDate = DateTime.Today;

            // keep popup visible if needed
            pnlCalendar.Style["display"] = "block";
            pnlCalendar.Style["left"] = "568px";
            pnlCalendar.Style["top"] = "68px";

            if (upCalendar != null) upCalendar.Update();

//            string js = @"
//            try {
//                // prefer hideCalendarPopup if present
//                if (window.hideCalendarPopup) {
//                    try { window.hideCalendarPopup(); }
//                    catch(e){ console && console.log && console.log('hideCalendarPopup error', e); }
//                } else if (window.findAndPruneDuplicates) {
//                    try { window.findAndPruneDuplicates(); }
//                    catch(e){ console && console.log && console.log('findAndPruneDuplicates error', e); }
//                } else {
//                    // last-resort: remove any .calendar-popup nodes except last
//                    (function(){
//                        var popups = document.querySelectorAll('.calendar-popup');
//                        if (!popups || popups.length <= 1) return;
//                        for (var i = 0; i < popups.length - 1; i++) {
//                            try { popups[i].parentNode.removeChild(popups[i]); } catch(e){}
//                        }
//                        var last = popups[popups.length-1];
//                        if (last) { last.style.display = 'block'; last.style.visibility = 'visible'; last.style.opacity = '1'; }
//                    })();
//                }
//            } catch(e){ console && console.log && console.log('hide-calendar script error', e); }
//        ";
//            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideCalendarAfterMonth", js, true);

            string script = @"
        try{
            setTimeout(function(){
                try{ if(typeof findAndPruneDuplicates === 'function') findAndPruneDuplicates(); }catch(e){}
                try{ if(typeof forceShowCanonical === 'function') forceShowCanonical(); }catch(e){}
                // fallback: call showCalendarPopup if forceShow not present
                try{ if(!window.forceShowCanonical && typeof showCalendarPopup === 'function') showCalendarPopup(); }catch(e){}
            }, 30);
        }catch(e){};";
            ScriptManager.RegisterStartupScript(upCalendar, upCalendar.GetType(), "showCalendarAfterMonthChange", script, true);

        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("calPast_VisibleMonthChanged error: " + ex.ToString());
        }
    }

    private DateTime? ParseTimeToDate(string hhmm, DateTime? dateFor)
    {
        if (String.IsNullOrWhiteSpace(hhmm)) return null;

        // choose base date (defaults to today)
        DateTime baseDate = dateFor ?? DateTime.Today;

        // try HH:mm parse
        TimeSpan ts;
        if (TimeSpan.TryParse(hhmm.Trim(), out ts))
        {
            return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, ts.Hours, ts.Minutes, 0);
        }

        // allow forgiving full DateTime parse but normalize to baseDate's date
        DateTime dt;
        if (DateTime.TryParse(hhmm.Trim(), out dt))
        {
            var timeOnly = dt.TimeOfDay;
            return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, timeOnly.Hours, timeOnly.Minutes, timeOnly.Seconds);
        }

        return null;
    }

    protected void grdGroup_DataBound(object sender, EventArgs e)
    {
        DateTime selDate;
        // Prefer the form value first (client may set it), then fallback to server hidden field if you rely on it.
        string posted = Request.Form["hidPastDate"] ?? (hidPastDate != null ? hidPastDate.Value : null);

        if (!string.IsNullOrEmpty(posted) &&
            DateTime.TryParseExact(posted, new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                CultureInfo.InvariantCulture, DateTimeStyles.None, out selDate))
        {
            calPast.SelectedDate = selDate;
            calPast.VisibleDate = selDate;
            calPast.TodaysDate = DateTime.Today;
        }
        else
        {
            // Only set to Today's month on first load; avoid overwriting user navigation on postbacks
            if (!IsPostBack)
            {
                calPast.SelectedDate = DateTime.MinValue;
                calPast.VisibleDate = DateTime.Today;
                calPast.TodaysDate = DateTime.Today;
            }
            // else: do nothing — let ASP.NET preserve the calendar state from the request
        }

        // Update the calendar UpdatePanel once if available
        try { if (upCalendar != null) upCalendar.Update(); }
        catch { }
    }

    private UpdatePanel GetParentUpdatePanel(Control c)
    {
        Control cur = c;
        while (cur != null)
        {
            if (cur is UpdatePanel) return (UpdatePanel)cur;
            cur = cur.Parent;
        }
        return null;
    }

    protected void calPast_DayRender(object sender, DayRenderEventArgs e)
    {
        // Highlight today's date
        if (e.Day.Date == DateTime.Today)
        {
            e.Cell.BackColor = System.Drawing.Color.LightSkyBlue;
            e.Cell.ForeColor = System.Drawing.Color.White;
            e.Cell.Font.Bold = true;
            e.Cell.CssClass = (e.Cell.CssClass ?? "") + " cal-today";
        }

        // Highlight selected date
        if (calPast.SelectedDate != DateTime.MinValue && e.Day.Date == calPast.SelectedDate.Date)
        {
            e.Cell.BackColor = System.Drawing.Color.Silver;
            e.Cell.ForeColor = System.Drawing.Color.White;
            e.Cell.Font.Bold = true;
            e.Cell.CssClass = (e.Cell.CssClass ?? "") + " cal-today";
        }

        // Disable future dates
        if (e.Day.Date > DateTime.Today)
        {
            e.Day.IsSelectable = false;
            e.Cell.ForeColor = System.Drawing.Color.Gray;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static object DeleteCheckinForStudent(int studentId, int classId, string dateIso /* optional: "yyyy-MM-dd" or empty for today */)
    {
        try
        {
            // get session values (school id and login id) from server session
            var sess = (clsSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (sess == null) return new { success = false, message = "Session expired" };

            int schoolId = sess.SchoolId;

            // determine date condition (match CreatedOn date or CheckinTime date as per your existing logic)
            string dateCondition;
            if (!string.IsNullOrEmpty(dateIso))
            {
                // sanitize iso format assumption - you may validate here if needed
                dateCondition = "AND CreatedOn >= CAST('" + dateIso + "' AS date) AND CreatedOn < DATEADD(day,1,CAST('" + dateIso + "' AS date))";
            }
            else
            {
                dateCondition = "AND CreatedOn >= CAST(GETDATE() AS date) AND CreatedOn < DATEADD(day,1,CAST(GETDATE() AS date))";
            }

            string sql = @"
            DELETE FROM StdtSessEvent
            WHERE SchoolId = " + schoolId + @"
              AND StudentId = " + studentId + @"
              AND ClassId = " + classId + @"
              AND EventType = 'CH'
              " + dateCondition + @";
        ";

            var obj = new clsData();
            obj.Execute(sql);

            return new { success = true, message = "Deleted" };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }

    protected void btnRefreshGrid_Click(object sender, EventArgs e)
    {
        try
        {
            //ClearGridThenRebind();

            //// debug token: put current timestamp into hidden field so client can verify server executed
            //hidPastDate.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //System.Diagnostics.Trace.WriteLine("btnRefreshGrid_Click invoked at " + DateTime.Now.ToString("o"));
            fillStudent("0", false);
            BindLocationDropdown();
            BindClientDropdown();
            pnlCalendar.Style["display"] = "none";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("btnRefreshGrid_Click error: " + ex.ToString());
        }
    }

    protected void ClearGridThenRebind()
    {
        try
        {
            // 1) Remove any cached dataset the binder might reuse (adjust session keys to your app)
            try { Session.Remove("StudentGrid_Data"); }
            catch { }

            // 2) Clear the GridViews immediately so the update sends empty markup first
            if (grdGroup != null)
            {
                grdGroup.DataSource = null;
                grdGroup.DataBind();
            }
            if (GridView1 != null)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

            // 3) Now fetch full fresh data and bind
            FillFullGridFromDb();

            // 4) Update UpdatePanels explicitly
            if (upGrid != null) upGrid.Update();
            if (upModalGrid != null) upModalGrid.Update();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("ClearGridThenRebind error: " + ex.ToString());
        }
    }

    private void FillFullGridFromDb()
    {
        // Example query - adapt columns and joins to your schema
        string sql = @"
        SELECT s.StudentId, s.Name, c.ClassName,
               MIN(CASE WHEN se.EventType='IN' THEN se.EventTime END) AS InTime,
               MAX(CASE WHEN se.EventType='OUT' THEN se.EventTime END) AS OutTime,
               CASE WHEN EXISTS (SELECT 1 FROM StdtSessEvent se2 WHERE se2.StudentId = s.StudentId AND se2.SomeDate = @date) THEN 1 ELSE 0 END AS IsPresent
        FROM Students s
        LEFT JOIN Classes c ON s.ClassId = c.ClassId
        LEFT JOIN StdtSessEvent se ON s.StudentId = se.StudentId -- adjust filters
        WHERE (@clientId IS NULL OR s.ClientId = @clientId)
          AND (@locationId IS NULL OR s.LocationId = @locationId)
          AND (se.SomeDate = @date OR se.SomeDate IS NULL)
        GROUP BY s.StudentId, s.Name, c.ClassName;
    ";

        // pick date/filter values from your controls or hidden fields
        string dateIso = hidPastDate != null && !string.IsNullOrEmpty(hidPastDate.Value)
            ? hidPastDate.Value : DateTime.Now.ToString("yyyy-MM-dd");

        object clientIdObj = null;
        try { clientIdObj = string.IsNullOrEmpty(ddlClient.SelectedValue) ? null : (object)ddlClient.SelectedValue; }
        catch { }

        object locationIdObj = null;
        try { locationIdObj = string.IsNullOrEmpty(ddlLocation.SelectedValue) ? null : (object)ddlLocation.SelectedValue; }
        catch { }

        DataTable dt = new DataTable();

        objData = new clsData();
        clsData.blnTrans = true;
        SqlConnection con = objData.Open();
        using (SqlCommand cmd = new SqlCommand(sql, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@date", dateIso);
            cmd.Parameters.AddWithValue("@clientId", clientIdObj ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@locationId", locationIdObj ?? DBNull.Value);

            da.Fill(dt);
        }

        // bind to both grids appropriately (transform dt if necessary)
        // If your grdGroup expects a particular shape, prepare that DataTable separately.
        grdGroup.DataSource = dt;
        grdGroup.DataBind();

        GridView1.DataSource = dt;
        GridView1.DataBind();

        // optional: cache result if you want (we removed cache earlier)
        // Session["StudentGrid_Data"] = dt;
    }

    private void DisableAllExceptToggle(Control parent)
    {
        foreach (Control cc in parent.Controls)
        {
            // If this control is a WebControl, check its CssClass for the attendance toggle marker
            WebControl webCtrl = cc as WebControl;
            bool isToggle = false;
            if (webCtrl != null && !string.IsNullOrEmpty(webCtrl.CssClass) &&
                webCtrl.CssClass.IndexOf("att-switch", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                isToggle = true;
            }

            if (!isToggle)
            {
                // disable specific control types (old-style casts for compatibility)
                TextBox txt = cc as TextBox;
                if (txt != null)
                {
                    txt.Enabled = false;
                    txt.Attributes["readonly"] = "readonly";
                }
                else
                {
                    DropDownList ddl = cc as DropDownList;
                    if (ddl != null) ddl.Enabled = false;
                    else
                    {
                        Button btn = cc as Button;
                        if (btn != null) btn.Enabled = false;
                        else
                        {
                            CheckBox chk = cc as CheckBox;
                            if (chk != null) chk.Enabled = false;
                        }
                    }
                }
            }

            // Recurse into children
            if (cc.HasControls())
                DisableAllExceptToggle(cc);
        }
    }

    private void DisableAllInputs(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            TextBox txt = c as TextBox;
            DropDownList ddl = c as DropDownList;
            Button btn = c as Button;
            CheckBox chk = c as CheckBox;

            if (txt != null)
            {
                txt.Enabled = false;
                txt.Attributes["readonly"] = "readonly";
            }
            else if (ddl != null)
            {
                ddl.Enabled = false;
            }
            else if (btn != null)
            {
                btn.Enabled = false;
            }
            else if (chk != null)
            {
                chk.Enabled = false;
            }

            if (c.HasControls())
                DisableAllInputs(c);
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static object InsertCheckinForStudent(int studentId, int classId, string dateIso /* optional: "yyyy-MM-dd" or empty for today */)
    {
        try
        {
            var sess = (clsSession)System.Web.HttpContext.Current.Session["UserSession"];
            if (sess == null) return new { success = false, message = "Session expired" };

            var objData = new clsData();
            clsData.blnTrans = true;
            SqlConnection con = objData.Open();
            DateTime dateFilter = DateTime.Now.Date;
            if (!string.IsNullOrWhiteSpace(dateIso))
            {
                DateTime parsed;
                if (DateTime.TryParse(dateIso, out parsed))
                    dateFilter = parsed.Date;
                else
                    return new { success = false, message = "Invalid date format for dateIso" };
            }

                string sql = @"
                -- use a single parameterized batch: check if a CH row exists for given date (by date only)
                IF EXISTS(
                    SELECT 1 FROM StdtSessEvent
                    WHERE SchoolId = @schoolId
                      AND StudentId = @studentId
                      AND ClassId = @classId
                      AND EventType = 'CH'
                      AND CAST(CreatedOn AS date) = @date
                )
                BEGIN
                    UPDATE StdtSessEvent
                    SET CheckStatus = 'True',
                        IsPresent = 1,
                        ModifiedOn = GETDATE()
                    WHERE SchoolId = @schoolId
                      AND StudentId = @studentId
                      AND ClassId = @classId
                      AND EventType = 'CH'
                      AND CAST(CreatedOn AS date) = @date;
                END
                ELSE
                BEGIN
                    INSERT INTO StdtSessEvent
                        (SchoolId, StudentId, EvntTs, CheckStatus, IsPresent, ClassId, CreatedBy, CreatedOn, ModifiedOn, EventType, CheckinTime)
                    VALUES
                        (@schoolId, @studentId, GETDATE(), 'True', 1, @classId, @userId, GETDATE(), GETDATE(), 'CH', GETDATE());
                END
                ";
                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@schoolId", sess.SchoolId);
                    cmd.Parameters.AddWithValue("@studentId", studentId);
                    cmd.Parameters.AddWithValue("@classId", classId);
                    cmd.Parameters.AddWithValue("@userId", sess.LoginId);
                    cmd.Parameters.AddWithValue("@date", dateFilter);

                    cmd.CommandTimeout = 60;
                    cmd.ExecuteNonQuery();
                }

            return new { success = true, message = "Inserted/Updated" };
        }
        catch (Exception ex)
        {
            // return the exception message so client can show (or log it on server)
            return new { success = false, message = ex.Message };
        }
    }



}