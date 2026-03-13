using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.HtmlControls;

public partial class Administration_StudentAttendance : System.Web.UI.Page
{
    clsSession oSession = null;
    clsData objData = null;
    DataClass objDataClass = new DataClass();
    public static string sub, selCommand, name;
    public static int intStdId = 0;
    clsSession sess = null;
    static bool Disable = false;
    public static bool showImage = false;


    protected void Page_Load(object sender, EventArgs e)
    {
        var sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
            return;
        }
        if (!clsGeneral.PageIdentification(sess.perPage))
        {
            Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            return;
        }

        txtMonth.Attributes["type"] = "month";

        if (!IsPostBack)
        {
            txtMonth.Text = DateTime.Today.ToString("yyyy-MM");
            txtMonth.Attributes.Add("max", DateTime.Now.ToString("yyyy-MM"));
            btnGenerate_Click(null, null);
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            clsData oData = new clsData();
            oSession = (clsSession)Session["UserSession"];

            string raw = (txtMonth.Text ?? "").Trim();
            if (raw.Length == 7 && raw[4] == '-') raw += "-01";

            DateTime reportMonth;
            bool ok = DateTime.TryParseExact(
                raw,
                new[] { "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy", "yyyy/MM/dd", "MM-yyyy", "MM/yyyy" },
                System.Globalization.CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out reportMonth
            );
            if (!ok) reportMonth = DateTime.Today;

            reportMonth = new DateTime(reportMonth.Year, reportMonth.Month, 1);
            txtMonth.Text = reportMonth.ToString("yyyy-MM");

            using (var con = new SqlConnection(oData.ConnectionString))
            using (var cmd = new SqlCommand("[dbo].[usp_MPA_SchoolAttendanceSheet_Pivot]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ReportMonth", SqlDbType.Date).Value = reportMonth.Date;
                bool testAllSchools = true;
                cmd.Parameters.Add("@SchoolId", SqlDbType.Int).Value =
                    testAllSchools ? (object)DBNull.Value : (object)oSession.SchoolId;
                cmd.Parameters.Add("@ClassId", SqlDbType.Int).Value = DBNull.Value;
                // cmd.Parameters.Add("@SearchText", SqlDbType.NVarChar, 100).Value = ...

                con.Open();
                var dt = new DataTable();
                using (var adp = new SqlDataAdapter(cmd)) adp.Fill(dt);

                btnExport.Enabled = (dt != null && dt.Rows.Count > 0);

                if (dt == null || dt.Rows.Count == 0)
                {
                    noDataBox.Visible = true;
                    gvReport.Visible = false;
                    DeferHideLoader("hideLoader_NoData");
                    return;
                }

                noDataBox.Visible = false;
                gvReport.Visible = true;

                int daysInMonth = DateTime.DaysInMonth(reportMonth.Year, reportMonth.Month);

                if (!dt.Columns.Contains("Present")) dt.Columns.Add("Present", typeof(int));
                if (!dt.Columns.Contains("Tardy")) dt.Columns.Add("Tardy", typeof(int));
                if (!dt.Columns.Contains("Total P/T")) dt.Columns.Add("Total P/T", typeof(int));

                foreach (DataRow r in dt.Rows)
                {
                    int p = 0, t = 0;
                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        string col = d.ToString("00");
                        if (!dt.Columns.Contains(col)) continue;

                        string v = (r[col] == null ? null : r[col].ToString()).Trim().ToUpperInvariant();
                        if (v == "P" || v == "PR" || v == "PRESENT") p++;
                        else if (v == "T" || v == "TR" || v == "TARDY") t++;
                    }
                    r["Present"] = p;
                    r["Tardy"] = t;
                    r["Total P/T"] = p + t;
                }

                var order = new List<string> { "Student", "Staffing", "Program", "4010", "1306", "District", "LEA Representative", "Send Email", "Room" };
                for (int d = 1; d <= daysInMonth; d++) order.Add(d.ToString("00"));
                order.AddRange(new[] { "Present", "Tardy", "Total P/T", "Absent", "Days" });

                var keep = order.Where(c => dt.Columns.Contains(c)).ToArray();
                var ordered = keep.Length > 0 ? dt.DefaultView.ToTable(false, keep) : dt;

                gvReport.ShowHeader = true;
                gvReport.ShowHeaderWhenEmpty = true;

                gvReport.DataSource = ordered;
                gvReport.DataBind();
                ViewState["MPAReportDT"] = ordered;
                //ScriptManager.RegisterStartupScript(this, GetType(), "dbgRows",
                    //"console.log('MPA rows bound: ' + " + gvReport.Rows.Count + ");", true);


                foreach (GridViewRow row in gvReport.Rows)
                {
                    int total = row.Cells.Count;
                    if (total >= 5)
                    {
                        row.Cells[total - 5].CssClass += " sum-present";
                        row.Cells[total - 4].CssClass += " sum-tardy";
                        row.Cells[total - 3].CssClass += " sum-totalpt";
                        row.Cells[total - 2].CssClass += " sum-absent";
                        row.Cells[total - 1].CssClass += " sum-days";
                    }
                }

                gvReport.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                gvReport.RowStyle.HorizontalAlign = HorizontalAlign.Center;

                foreach (GridViewRow rr in gvReport.Rows)
                    foreach (TableCell c in rr.Cells)
                    {
                        c.HorizontalAlign = HorizontalAlign.Center;
                        c.Style["text-align"] = "center";
                    }

                gvReport.UseAccessibleHeader = true;
                if (gvReport.HeaderRow != null)
                {
                    gvReport.HeaderRow.TableSection = TableRowSection.TableHeader;
                    gvReport.HeaderRow.HorizontalAlign = HorizontalAlign.Center;
                    foreach (TableCell c in gvReport.HeaderRow.Cells)
                    {
                        c.HorizontalAlign = HorizontalAlign.Center;
                        c.Style["text-align"] = "center";
                    }
                }

                if (gvReport.HeaderRow != null)
                {
                    int headerColCount = gvReport.HeaderRow.Cells.Count;
                    var titleRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                    var title1 = new TableCell
                    {
                        Text = "Melmark School Attendance",
                        ColumnSpan = headerColCount,
                        HorizontalAlign = HorizontalAlign.Center,
                        CssClass = "mpaTitleCell"
                    };
                    title1.Font.Bold = true;
                    title1.Font.Size = FontUnit.Point(14);
                    titleRow1.Cells.Add(title1);
                    gvReport.HeaderRow.Parent.Controls.AddAt(0, titleRow1);
                    titleRow1.TableSection = TableRowSection.TableHeader;

                    var titleRow2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                    var title2 = new TableCell
                    {
                        Text = reportMonth.ToString("MMMM yyyy"),
                        ColumnSpan = headerColCount,
                        HorizontalAlign = HorizontalAlign.Center,
                        CssClass = "mpaTitleCell centerTitle"
                    };
                    title2.HorizontalAlign = HorizontalAlign.Center;
                    title2.Attributes["style"] = "text-align:center;";
                    title2.Font.Bold = true;
                    title2.Font.Size = FontUnit.Point(12);
                    titleRow2.Cells.Add(title2);
                    gvReport.HeaderRow.Parent.Controls.AddAt(1, titleRow2);
                    titleRow2.TableSection = TableRowSection.TableHeader;
                    titleRow2.HorizontalAlign = HorizontalAlign.Center;

                    if (gvReport.Rows.Count >= 0 && daysInMonth > 0)
                    {
                        int summaryCols = 5;
                        int totalHeaderCells = gvReport.HeaderRow.Cells.Count;
                        int fixedCols = totalHeaderCells - daysInMonth - summaryCols;
                        if (fixedCols < 0) fixedCols = 0;

                        var dowRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

                        for (int i = 0; i < fixedCols; i++)
                        {
                            var blank = new TableCell { Text = string.Empty };
                            blank.Attributes["class"] = "hdr nobordercell";
                            blank.Style.Add("border", "none");
                            blank.HorizontalAlign = HorizontalAlign.Center;
                            dowRow.Cells.Add(blank);
                        }

                        for (int d = 1; d <= daysInMonth; d++)
                        {
                            var cell = new TableCell();
                            var dt2 = new DateTime(reportMonth.Year, reportMonth.Month, d);
                            cell.Text = dt2.ToString("ddd");
                            cell.Attributes["class"] = "hdr daycol";
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#DEEAF6");
                            dowRow.Cells.Add(cell);
                        }

                        var secPresent = new TableCell { Text = "Present", HorizontalAlign = HorizontalAlign.Center };
                        secPresent.Attributes["class"] = "hdr sec-present";
                        secPresent.ColumnSpan = 3;
                        dowRow.Cells.Add(secPresent);

                        var secAbsent = new TableCell { Text = "Absent", HorizontalAlign = HorizontalAlign.Center };
                        secAbsent.Attributes["class"] = "hdr sec-absent";
                        secAbsent.ColumnSpan = 1;
                        dowRow.Cells.Add(secAbsent);

                        var secDays = new TableCell { Text = "Total", HorizontalAlign = HorizontalAlign.Center };
                        secDays.Attributes["class"] = "hdr sec-days";
                        secDays.ColumnSpan = 1;
                        dowRow.Cells.Add(secDays);

                        gvReport.HeaderRow.Parent.Controls.AddAt(2, dowRow);
                        dowRow.TableSection = TableRowSection.TableHeader;
                    }
                    var hdr = gvReport.HeaderRow;
                    if (hdr != null)
                    {
                        int total = hdr.Cells.Count;

                        for (int d = 1; d <= daysInMonth; d++)
                        {
                            int colIndex = order.IndexOf(d.ToString("00"));
                            if (colIndex >= 0 && colIndex < hdr.Cells.Count)
                                hdr.Cells[colIndex].CssClass += " daycol";
                        }

                        if (total >= 5)
                        {
                            hdr.Cells[total - 5].Text = "Present";
                            hdr.Cells[total - 4].Text = "Tardy";
                            hdr.Cells[total - 3].Text = "Total P/T";
                            hdr.Cells[total - 2].Text = "Absent";
                            hdr.Cells[total - 1].Text = "Days";

                            hdr.Cells[total - 5].CssClass += " col-present";
                            hdr.Cells[total - 4].CssClass += " col-tardy";
                            hdr.Cells[total - 3].CssClass += " col-totalpt";
                            hdr.Cells[total - 2].CssClass += " col-absent";
                            hdr.Cells[total - 1].CssClass += " col-days";
                        }
                    }

                    for (int i = 0; i < gvReport.HeaderRow.Cells.Count; i++)
                        gvReport.HeaderRow.Cells[i].Attributes["style"] = "white-space:nowrap";
                }

                foreach (GridViewRow row in gvReport.Rows)
                {
                    for (int d = 1; d <= daysInMonth; d++)
                    {
                        int colIndex = order.IndexOf(d.ToString("00"));
                        if (colIndex >= 0 && colIndex < row.Cells.Count)
                        {
                            var cell = row.Cells[colIndex];
                            cell.CssClass += " daycol";

                            string val = (cell.Text ?? "").Trim().ToUpperInvariant();
                            if (val == "P")
                                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#C5E0B3"); // Present
                            else if (val == "T")
                                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#E2EFD9"); // Tardy
                            else if (val == "A")
                                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#F7CAAC"); // Absent
                            else
                                cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#A5A5A5"); // default day background
                        }
                    }
                }

                // done: hide loader after successful bind/render
                DeferHideLoader("hideLoader_Ok");
            }
        }
        catch (Exception ex)
        {
            noDataBox.Visible = true;
            gvReport.Visible = false;
            //ScriptManager.RegisterStartupScript(this, GetType(), "errToast",
                //"console.error('Generate failed: " + DateTime.Now.Ticks + "');", true);
        }
        finally
        {
            // ALWAYS hide the loader, even on early returns/errors
            DeferHideLoader("hideLoader_Finally");
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            var token = Request.Form[hdnDownloadToken.UniqueID];
            if (!string.IsNullOrEmpty(token))
            {
                var ck = new HttpCookie("downloadToken", token)
                {
                    Path = "/",
                    Expires = DateTime.UtcNow.AddMinutes(5)
                };
                Response.Cookies.Add(ck);
            }
        }
        catch {}
        var dt = ViewState["MPAReportDT"] as DataTable;
        if (dt == null || dt.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "expNoData",
                "if(window.hideLoader) hideLoader();", true);
            return;
        }

        Func<System.Drawing.Color, string> ToHtml = c =>
        {
            if (c.IsEmpty) return "";
            return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(c.R, c.G, c.B));
        };
        Action<TableCell, string> PaintBg = (cell, hex) =>
        {
            if (string.IsNullOrEmpty(hex)) return;
            string style = cell.Attributes["style"] ?? "";
            if (style.IndexOf("background-color", StringComparison.OrdinalIgnoreCase) < 0)
                cell.Attributes["style"] = style + ";background-color:" + hex + " !important;";
            cell.Attributes["bgcolor"] = hex;
        };
        Action<TableCell, string> AddStyle = (cell, extra) =>
        {
            string style = cell.Attributes["style"] ?? "";
            cell.Attributes["style"] = style + ";" + extra;
        };
        Action<TableCell, int> SetWidth = (cell, px) =>
            AddStyle(cell, string.Format("width:{0}px;min-width:{0}px;max-width:{0}px;", px));

        Action<TableCell> Center = cell =>
        {
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Attributes["align"] = "center";
            string s = cell.Attributes["style"] ?? "";
            cell.Attributes["style"] = s + ";text-align:center;vertical-align:middle;";
        };
        Action<TableCell> LeftAlign = cell =>
        {
            cell.HorizontalAlign = HorizontalAlign.Left;
            cell.Attributes["align"] = "left";
            string s = cell.Attributes["style"] ?? "";
            cell.Attributes["style"] = s + ";text-align:left;vertical-align:middle;";
        };

        string cDayHdr = "#DEEAF6";
        string cPresent = "#C5E0B3";
        string cTardy = "#E2EFD9";
        string cAbsent = "#F7CAAC";
        string cTotalPT = "#D9E1F2";
        string cTotalDays = "#D0CECE";
        string cNeutralDay = "#A5A5A5";

        var leftWidths = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
        { "Student", 220 }, { "Staffing", 110 }, { "Program", 120 },
        { "4010", 80 }, { "1306", 80 }, { "District", 120 },
        { "LEA Representative", 160 }, { "Send Email", 120 }, { "Room", 90 }
    };
        int dayWidth = 40;
        int sumWidth = 80;

        var leftAlignedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Student", "Staffing", "Program"
    };

        var gv = new GridView
        {
            AutoGenerateColumns = true,
            ShowHeader = true,
            EnableViewState = false
        };
        gv.DataSource = dt;
        gv.DataBind();

        gv.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        gv.RowStyle.HorizontalAlign = HorizontalAlign.Center;

        DateTime reportMonth;
        var raw = (txtMonth.Text ?? "").Trim();
        if (raw.Length == 7 && raw[4] == '-') raw += "-01";
        if (!DateTime.TryParseExact(raw,
            new[] { "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy", "yyyy/MM/dd", "MM-yyyy", "MM/yyyy" },
            System.Globalization.CultureInfo.InvariantCulture,
            DateTimeStyles.None, out reportMonth))
            reportMonth = DateTime.Today;
        reportMonth = new DateTime(reportMonth.Year, reportMonth.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(reportMonth.Year, reportMonth.Month);

        var table = gv.HeaderRow != null ? (gv.HeaderRow.Parent as System.Web.UI.WebControls.Table) : null;
        if (table != null)
        {
            int headerColCount = gv.HeaderRow.Cells.Count;

            var titleRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            var title1 = new TableCell { Text = "Melmark School Attendance", ColumnSpan = headerColCount };
            AddStyle(title1, "font-weight:bold;font-size:14pt;");
            Center(title1);
            titleRow1.Cells.Add(title1);
            table.Controls.AddAt(0, titleRow1);

            var titleRow2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            var title2 = new TableCell { Text = reportMonth.ToString("MMMM yyyy"), ColumnSpan = headerColCount };
            AddStyle(title2, "font-weight:bold;font-size:12pt;");
            Center(title2);
            titleRow2.Cells.Add(title2);
            table.Controls.AddAt(1, titleRow2);

            int summaryCols = 5;
            int fixedCols = headerColCount - daysInMonth - summaryCols;
            if (fixedCols < 0) fixedCols = 0;

            var dowRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

            for (int i = 0; i < fixedCols; i++)
            {
                var blank = new TableCell { Text = string.Empty };
                AddStyle(blank, "border:1.5pt solid #6b7280;padding:5px 7px;");
                Center(blank);
                dowRow.Cells.Add(blank);
            }

            for (int d = 1; d <= daysInMonth; d++)
            {
                var dt2 = new DateTime(reportMonth.Year, reportMonth.Month, d);
                var cell = new TableCell { Text = dt2.ToString("ddd") };
                PaintBg(cell, cDayHdr);
                AddStyle(cell, "border:1.5pt solid #6b7280;padding:5px 7px;font-weight:bold;white-space:nowrap;");
                SetWidth(cell, dayWidth);
                Center(cell);
                dowRow.Cells.Add(cell);
            }

            var secPresent = new TableCell { Text = "Present", ColumnSpan = 3 };
            PaintBg(secPresent, cPresent);
            AddStyle(secPresent, "border:1.5pt solid #6b7280;padding:5px 7px;font-weight:bold;white-space:nowrap;");
            Center(secPresent);
            dowRow.Cells.Add(secPresent);

            var secAbsent = new TableCell { Text = "Absent", ColumnSpan = 1 };
            PaintBg(secAbsent, cAbsent);
            AddStyle(secAbsent, "border:1.5pt solid #6b7280;padding:5px 7px;font-weight:bold;white-space:nowrap;");
            Center(secAbsent);
            dowRow.Cells.Add(secAbsent);

            var secDays = new TableCell { Text = "Total", ColumnSpan = 1 };
            PaintBg(secDays, cTotalDays);
            AddStyle(secDays, "border:1.5pt solid #6b7280;padding:5px 7px;font-weight:bold;white-space:nowrap;");
            Center(secDays);
            dowRow.Cells.Add(secDays);

            table.Controls.AddAt(2, dowRow);
        }

        Func<string, bool> IsTwoDigitDay = s =>
        {
            if (string.IsNullOrEmpty(s)) return false;
            s = System.Web.HttpUtility.HtmlDecode(s).Trim();
            if (s.Length != 2) return false;
            int n;
            return int.TryParse(s, out n) && n >= 1 && n <= 31;
        };

        if (gv.HeaderRow != null)
        {
            for (int i = 0; i < gv.HeaderRow.Cells.Count; i++)
            {
                var hc = gv.HeaderRow.Cells[i];
                string text = System.Web.HttpUtility.HtmlDecode(hc.Text ?? "").Trim();

                if (leftAlignedHeaders.Contains(text)) LeftAlign(hc); else Center(hc);

                if (IsTwoDigitDay(text))
                {
                    SetWidth(hc, dayWidth); PaintBg(hc, cDayHdr);
                }
                else if (text.Equals("Present", StringComparison.OrdinalIgnoreCase))
                {
                    SetWidth(hc, sumWidth); PaintBg(hc, cPresent);
                }
                else if (text.Equals("Tardy", StringComparison.OrdinalIgnoreCase))
                {
                    SetWidth(hc, sumWidth); PaintBg(hc, cTardy);
                }
                else if (text.Equals("Total P/T", StringComparison.OrdinalIgnoreCase))
                {
                    SetWidth(hc, sumWidth); PaintBg(hc, cTotalPT);
                }
                else if (text.Equals("Absent", StringComparison.OrdinalIgnoreCase))
                {
                    SetWidth(hc, sumWidth); PaintBg(hc, cAbsent);
                }
                else if (text.Equals("Days", StringComparison.OrdinalIgnoreCase))
                {
                    SetWidth(hc, sumWidth); PaintBg(hc, cTotalDays);
                }
                else
                {
                    int w; if (!leftWidths.TryGetValue(text, out w)) w = 140;
                    SetWidth(hc, w);
                }

                AddStyle(hc, "border:1.5pt solid #6b7280;padding:5px 7px;font-weight:bold;white-space:nowrap;");
            }
        }

        foreach (GridViewRow r in gv.Rows)
        {
            for (int i = 0; i < r.Cells.Count; i++)
            {
                var c = r.Cells[i];
                AddStyle(c, "height:22px;");

                string headerText = gv.HeaderRow != null
                    ? System.Web.HttpUtility.HtmlDecode((gv.HeaderRow.Cells[i].Text ?? "")).Trim()
                    : "";
                string val = (c.Text ?? "").Trim().ToUpperInvariant();

                if (leftAlignedHeaders.Contains(headerText)) LeftAlign(c); else Center(c);

                if (IsTwoDigitDay(headerText))
                {
                    if (val == "P" || val == "PR" || val == "PRESENT") PaintBg(c, cPresent);
                    else if (val == "T" || val == "TR" || val == "TARDY") PaintBg(c, cTardy);
                    else if (val == "A" || val == "ABSENT") PaintBg(c, cAbsent);
                    else PaintBg(c, cNeutralDay);
                }
                else if (headerText.Equals("Present", StringComparison.OrdinalIgnoreCase)) PaintBg(c, cPresent);
                else if (headerText.Equals("Tardy", StringComparison.OrdinalIgnoreCase)) PaintBg(c, cTardy);
                else if (headerText.Equals("Total P/T", StringComparison.OrdinalIgnoreCase)) PaintBg(c, cTotalPT);
                else if (headerText.Equals("Absent", StringComparison.OrdinalIgnoreCase)) PaintBg(c, cAbsent);
                else if (headerText.Equals("Days", StringComparison.OrdinalIgnoreCase)) PaintBg(c, cTotalDays);

                AddStyle(c, "border:1pt solid #9aa0a6;padding:4px 6px;white-space:nowrap;mso-number-format:\"@\";");
            }
        }

        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.ContentType = "application/vnd.ms-excel";
        string fname = "MPA_School_Attendance_" + (txtMonth.Text ?? DateTime.Today.ToString("yyyy-MM")) + ".xls";
        Response.AddHeader("Content-Disposition", "attachment;filename=" + fname);

        using (var sw = new System.IO.StringWriter())
        using (var htw = new HtmlTextWriter(sw))
        {
            htw.Write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>");
            htw.Write("<style>table{border-collapse:collapse}th,td{white-space:nowrap;text-align:center;vertical-align:middle;mso-number-format:\"@\"}</style>");
            htw.Write("</head><body>");
            gv.RenderControl(htw);
            htw.Write("</body></html>");
            Response.Write(sw.ToString());
        }

        Response.Flush();
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    private static string ToHex(System.Drawing.Color c)
    {
        if (c.IsEmpty) return "";
        return System.Drawing.ColorTranslator.ToHtml(
            System.Drawing.Color.FromArgb(c.R, c.G, c.B)
        );
    }

    private void DeferHideLoader(string key)
    {
        string js =
            "(function(){"
          + "  function run(){ try{ if(window.hideLoader) hideLoader(); }catch(e){} }"
          + "  if (window.Sys && Sys.Application) { Sys.Application.add_load(run); }"
          + "  else if (document.readyState === 'complete') { setTimeout(run,0); }"
          + "  else if (window.addEventListener) { window.addEventListener('load', run); }"
          + "  else { window.attachEvent && window.attachEvent('onload', run); }"
          + "})();";
        ScriptManager.RegisterStartupScript(this, GetType(), key, js, true);
    }

}