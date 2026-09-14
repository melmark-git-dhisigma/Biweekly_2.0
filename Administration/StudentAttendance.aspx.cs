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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.SS.Util;
using System.Diagnostics;

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
        Stopwatch sw = Stopwatch.StartNew();
        clsReportExecutionLog csrplog = new clsReportExecutionLog();
        try
        {
            csrplog.StartTime = DateTime.Now;
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
            csrplog.ReportName = "Attendance Report";
            csrplog.UserId = sess.LoginId;
            csrplog.ServerID = Environment.MachineName;
            csrplog.Parameters = "ReportMonth=" + reportMonth.ToString();

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
                    csrplog.RowCount = dt.Rows.Count;
                    noDataBox.Visible = true;
                    gvReport.Visible = false;
                    DeferHideLoader("hideLoader_NoData");
                    return;
                }
                else
                    csrplog.RowCount = -1;

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
                csrplog.Status = "Success";
            }
        }
        catch (Exception ex)
        {
            noDataBox.Visible = true;
            gvReport.Visible = false;
            csrplog.Status = "Failed";
            csrplog.ErrorMessage = ex.Message;
            //ScriptManager.RegisterStartupScript(this, GetType(), "errToast",
                //"console.error('Generate failed: " + DateTime.Now.Ticks + "');", true);
        }
        finally
        {
            // ALWAYS hide the loader, even on early returns/errors
            DeferHideLoader("hideLoader_Finally");
            sw.Stop();
            csrplog.EndTime = DateTime.Now;
            csrplog.DurationMs = sw.ElapsedMilliseconds;
            ReportLogger.Save(csrplog);
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        var dt = ViewState["MPAReportDT"] as DataTable;
        if (dt == null || dt.Rows.Count == 0) return;

        string filename = "MPA_School_Attendance_" +
                          (txtMonth.Text ?? DateTime.Today.ToString("yyyy-MM")) + ".xlsx";

        string token = hidExportToken.Value;

        ExportAttendanceToExcel(dt, filename, Response, token);
    }

    private void ExportAttendanceToExcel(DataTable dt, string filename, HttpResponse response, string token)
    {
        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("Attendance");

        string cDayHdr = "#DEEAF6";
        string cPresent = "#C5E0B3";
        string cTardy = "#E2EFD9";
        string cAbsent = "#F7CAAC";
        string cTotalPT = "#D9E1F2";
        string cTotalDays = "#D0CECE";
        string cNeutralDay = "#A5A5A5";

        var leftAlignedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Student", "Staffing", "Program"
    };

        DateTime reportMonth;
        var raw = (txtMonth.Text ?? "").Trim();
        if (raw.Length == 7 && raw[4] == '-') raw += "-01";
        if (!DateTime.TryParseExact(raw,
            new[] { "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy", "yyyy/MM/dd", "MM-yyyy", "MM/yyyy" },
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out reportMonth))
            reportMonth = DateTime.Today;

        reportMonth = new DateTime(reportMonth.Year, reportMonth.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(reportMonth.Year, reportMonth.Month);

        var title1Style = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)14, NPOI.SS.UserModel.BorderStyle.None, null, false);
        var title2Style = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)12, NPOI.SS.UserModel.BorderStyle.None, null, false);

        var blankHdrStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, null, false);

        var dowDayStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#DEEAF6", false);

        var headerFixedStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#BDD7EE", false);

        var headerDayStyle = CreateStyle( workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#BDD7EE", false );

        var secPresentStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#C5E0B3", false);
        var secAbsentStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#F7CAAC", false);
        var secDaysStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#D0CECE", false);

        var headerLeftStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10,
            NPOI.SS.UserModel.BorderStyle.Medium, "#DEEAF6", false);

        var headerCenterStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10,
            NPOI.SS.UserModel.BorderStyle.Medium, "#DEEAF6", false);

        var headerPresentStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#C5E0B3", false);
        var headerTardyStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#E2EFD9", false);
        var headerTotalPTStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10, NPOI.SS.UserModel.BorderStyle.Medium, "#C5E0B3", false);

        var headerAbsentStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10,
            NPOI.SS.UserModel.BorderStyle.Medium, "#F7CAAC", false);

        var headerDaysStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, true, (short)10,
            NPOI.SS.UserModel.BorderStyle.Medium, "#D0CECE", false);

        var dataLeftStyle = CreateStyle(workbook, HorizontalAlignment.Left, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, null, false);
        var dataCenterStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, null, false);
        var dayPresentStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#C5E0B3", false);
        var dayTardyStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#E2EFD9", false);
        var dayAbsentStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#F7CAAC", false);
        var dayNeutralStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#A5A5A5", false);
        var dataPresentStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#C5E0B3", false);
        var dataTardyStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#E2EFD9", false);
        var dataTotalPTStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#C5E0B3", false);
        var dataAbsentSumStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#F7CAAC", false);
        var dataDaysStyle = CreateStyle(workbook, HorizontalAlignment.Center, VerticalAlignment.Center, false, (short)10, NPOI.SS.UserModel.BorderStyle.Thin, "#D0CECE", false);

        int rowIndex = 0;

        IRow titleRow1 = sheet.CreateRow(rowIndex++);
        titleRow1.HeightInPoints = 22f;
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            ICell c = titleRow1.CreateCell(i);
            c.CellStyle = title1Style;
        }
        titleRow1.GetCell(0).SetCellValue("Melmark School Attendance");
        sheet.AddMergedRegion(new CellRangeAddress(titleRow1.RowNum, titleRow1.RowNum, 0, dt.Columns.Count - 1));

        IRow titleRow2 = sheet.CreateRow(rowIndex++);
        titleRow2.HeightInPoints = 20f;

        ICellStyle title2CenteredStyle = workbook.CreateCellStyle();
        title2CenteredStyle.CloneStyleFrom(title2Style);
        title2CenteredStyle.Alignment = HorizontalAlignment.CenterSelection;

        for (int i = 0; i < dt.Columns.Count; i++)
        {
            ICell c = titleRow2.CreateCell(i);
            c.CellStyle = title2CenteredStyle;
        }

        titleRow2.GetCell(0).SetCellValue(reportMonth.ToString("MMMM yyyy"));

        int summaryCols = 5;
        int fixedCols = dt.Columns.Count - daysInMonth - summaryCols;
        if (fixedCols < 0) fixedCols = 0;

        IRow dowRow = sheet.CreateRow(rowIndex++);
        dowRow.HeightInPoints = 20f;

        int col = 0;

        // fixed left columns
        for (int i = 0; i < fixedCols; i++, col++)
        {
            ICell cell = dowRow.CreateCell(col);
            cell.SetCellValue("");
            cell.CellStyle = blankHdrStyle;
        }

        // day wording row: Mon Tue Wed...
        for (int d = 1; d <= daysInMonth; d++, col++)
        {
            ICell cell = dowRow.CreateCell(col);
            cell.SetCellValue(new DateTime(reportMonth.Year, reportMonth.Month, d).ToString("ddd"));
            cell.CellStyle = dowDayStyle;
        }

        // Present block spans 3 columns visually without merge
        int presentStart = col;

        ICellStyle secPresentCenteredStyle = workbook.CreateCellStyle();
        secPresentCenteredStyle.CloneStyleFrom(secPresentStyle);
        secPresentCenteredStyle.Alignment = HorizontalAlignment.CenterSelection;

        for (int i = 0; i < 3 && col < dt.Columns.Count; i++, col++)
        {
            ICell cell = dowRow.CreateCell(col);
            cell.CellStyle = secPresentCenteredStyle;
        }

        dowRow.GetCell(presentStart).SetCellValue("Present");

        // Absent block
        if (col < dt.Columns.Count)
        {
            ICell cell = dowRow.CreateCell(col++);
            cell.SetCellValue("Absent");
            cell.CellStyle = secAbsentStyle;
        }

        // Total block
        if (col < dt.Columns.Count)
        {
            ICell cell = dowRow.CreateCell(col++);
            cell.SetCellValue("Total");
            cell.CellStyle = secDaysStyle;
        }

        IRow headerRow = sheet.CreateRow(rowIndex++);
        headerRow.HeightInPoints = 22;

        for (int i = 0; i < dt.Columns.Count; i++)
        {
            string header = dt.Columns[i].ColumnName;
            ICell cell = headerRow.CreateCell(i);
            cell.SetCellValue(header);

            int n;
            bool isDay = int.TryParse(header, out n) && header.Length == 2 && n >= 1 && n <= 31;

            if (isDay)
                cell.CellStyle = headerDayStyle;
            else if (header.Equals("Present", StringComparison.OrdinalIgnoreCase) ||
                     header.Equals("Total P/T", StringComparison.OrdinalIgnoreCase))
                cell.CellStyle = secPresentStyle;
            else if (header.Equals("Tardy", StringComparison.OrdinalIgnoreCase))
                cell.CellStyle = headerTardyStyle;
            else if (header.Equals("Absent", StringComparison.OrdinalIgnoreCase))
                cell.CellStyle = headerAbsentStyle;
            else if (header.Equals("Days", StringComparison.OrdinalIgnoreCase))
                cell.CellStyle = headerDaysStyle;
            else
                cell.CellStyle = headerFixedStyle;
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            IRow row = sheet.CreateRow(rowIndex++);
            row.HeightInPoints = 18;

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string header = dt.Columns[j].ColumnName;
                string value = dt.Rows[i][j] == DBNull.Value ? "" : dt.Rows[i][j].ToString();

                ICell cell = row.CreateCell(j);
                cell.SetCellValue(value);

                int n;
                bool isDay = int.TryParse(header, out n) && header.Length == 2 && n >= 1 && n <= 31;

                if (isDay)
                {
                    string v = value.Trim().ToUpperInvariant();
                    if (v == "P" || v == "PR" || v == "PRESENT") cell.CellStyle = dayPresentStyle;
                    else if (v == "T" || v == "TR" || v == "TARDY") cell.CellStyle = dayTardyStyle;
                    else if (v == "A" || v == "ABSENT") cell.CellStyle = dayAbsentStyle;
                    else cell.CellStyle = dayNeutralStyle;
                }
                else if (header.Equals("Present", StringComparison.OrdinalIgnoreCase)) cell.CellStyle = dataPresentStyle;
                else if (header.Equals("Tardy", StringComparison.OrdinalIgnoreCase)) cell.CellStyle = dataTardyStyle;
                else if (header.Equals("Total P/T", StringComparison.OrdinalIgnoreCase)) cell.CellStyle = dataTotalPTStyle;
                else if (header.Equals("Absent", StringComparison.OrdinalIgnoreCase)) cell.CellStyle = dataAbsentSumStyle;
                else if (header.Equals("Days", StringComparison.OrdinalIgnoreCase)) cell.CellStyle = dataDaysStyle;
                else if (leftAlignedHeaders.Contains(header)) cell.CellStyle = dataLeftStyle;
                else cell.CellStyle = dataCenterStyle;
            }
        }

        for (int i = 0; i < dt.Columns.Count; i++)
        {
            string colName = dt.Columns[i].ColumnName;

            int maxLength = colName.Length;

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                string cellValue = dt.Rows[r][i] == DBNull.Value ? "" : dt.Rows[r][i].ToString();
                if (cellValue.Length > maxLength)
                    maxLength = cellValue.Length;
            }

            int width = (maxLength + 2) * 256;

            if (colName == "Student") width = Math.Max(width, 25 * 256);
            else if (colName == "Staffing") width = Math.Max(width, 15 * 256);
            else if (colName == "Program") width = Math.Max(width, 18 * 256);
            else if (colName == "4010" || colName == "1306") width = Math.Max(width, 10 * 256);
            else if (colName == "District") width = Math.Max(width, 16 * 256);
            else if (colName == "LEA Representative") width = Math.Max(width, 20 * 256);
            else if (colName == "Send Email") width = Math.Max(width, 18 * 256);
            else if (colName == "Room") width = Math.Max(width, 10 * 256);
            else if (colName == "Present" || colName == "Tardy" || colName == "Total P/T" || colName == "Absent" || colName == "Days")
                width = Math.Max(width, 12 * 256);
            else
                width = Math.Max(width, 8 * 256);

            if (width > 60 * 256)
                width = 60 * 256;

            sheet.SetColumnWidth(i, width);
        }

        sheet.CreateFreezePane(0, 4);

        response.Clear();
        response.ClearContent();
        response.ClearHeaders();
        response.Buffer = true;
        response.Charset = "";
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        if (!string.IsNullOrWhiteSpace(token))
        {
            response.AppendHeader("Set-Cookie", "ExportDone=" + token + "; path=/; SameSite=Lax");
        }

        response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(filename));
        response.Cache.SetCacheability(HttpCacheability.NoCache);
        response.Cache.SetNoStore();

        byte[] fileBytes;
        using (MemoryStream ms = new MemoryStream())
        {
            workbook.Write(ms);
            fileBytes = ms.ToArray();
        }

        response.BinaryWrite(fileBytes);
        response.Flush();
        response.SuppressContent = true;
        HttpContext.Current.ApplicationInstance.CompleteRequest();
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


    private ICellStyle CreateStyle(
    IWorkbook wb,
    HorizontalAlignment hAlign,
    VerticalAlignment vAlign,
    bool bold,
    short fontSize,
    NPOI.SS.UserModel.BorderStyle border,
    string hexFill = null,
    bool wrap = false)
    {
        ICellStyle style = wb.CreateCellStyle();
        style.Alignment = hAlign;
        style.VerticalAlignment = vAlign;
        style.BorderBottom = border;
        style.BorderTop = border;
        style.BorderLeft = border;
        style.BorderRight = border;
        style.WrapText = wrap;

        if (!string.IsNullOrEmpty(hexFill))
        {
            XSSFCellStyle xstyle = (XSSFCellStyle)style;
            xstyle.SetFillForegroundColor(MakeColor(hexFill));
            style.FillPattern = FillPattern.SolidForeground;
        }

        IFont font = wb.CreateFont();
        font.IsBold = bold;
        font.FontHeightInPoints = fontSize;
        style.SetFont(font);

        return style;
    }

    private XSSFColor MakeColor(string hex)
    {
        hex = hex.Replace("#", "");
        return new XSSFColor(new byte[]
    {
        Convert.ToByte(hex.Substring(0, 2), 16),
        Convert.ToByte(hex.Substring(2, 2), 16),
        Convert.ToByte(hex.Substring(4, 2), 16)
    });
    }


}