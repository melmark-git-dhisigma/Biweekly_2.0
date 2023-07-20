using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class StudentBinder_AssessmentPreview : System.Web.UI.Page
{
    clsSession sess = null;
    public string SortDireaction
    {
        get
        {
            if (ViewState["SortDireaction"] == null)
                return string.Empty;
            else
                return ViewState["SortDireaction"].ToString();
        }
        set
        {
            ViewState["SortDireaction"] = value;
        }
    }
    private string _sortDirection;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            //bool flag = clsGeneral.PageIdentification(sess.perPage);
            //if (flag == false)
            //{
            //    Response.Redirect("Error.aspx?Error=You are not authorized to access this Page.Contact Program Administrator");
            //}
        }
        if (!IsPostBack)
        {
             LoadData();
        }
    }
    private void LoadData()
    {
        int AssessmentId = Convert.ToInt32(Request.QueryString["AssessmentId"].ToString());
        string selQry = "SELECT AsmntName as Name,AsmntCat as Category,AsmntSubCat as SubCategory,GoalName as Goal," +
                        "AsmntQId as Question,AsmntScr as Score,TotScr as TotalScore FROM StdtLPStg Stg " +
                        "FULL JOIN LessonPlan LP ON ISNULL(LP.LessonPlanId,'0')=ISNULL(Stg.LessonPlanId,'0') " +
                        "WHERE AssessmentId=" + AssessmentId + " GROUP BY AsmntName,AsmntCat,AsmntSubCat,GoalName,AsmntQId,TotScr,AsmntScr,SortOrder ORDER BY SortOrder";

        clsData oData = new clsData();
        grdAsmntPreview.DataSource = oData.ReturnDataTable(selQry, false);
        grdAsmntPreview.DataBind();
    }
    protected void lnk_backtolist_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReviewAssessmnt.aspx");
    }

    protected void grdAsmntPreview_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadData();
        SetSortDirection(SortDireaction);
        DataTable dataTable = grdAsmntPreview.DataSource as DataTable;
        if (dataTable != null)
        {
            //Sort the data.
            dataTable.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
            grdAsmntPreview.DataSource = dataTable;
            grdAsmntPreview.DataBind();
            SortDireaction = _sortDirection;
            int columnIndex = 0;
            foreach (DataControlFieldHeaderCell headerCell in grdAsmntPreview.HeaderRow.Cells)
            {
                if (headerCell.ContainingField.SortExpression == e.SortExpression)
                {
                    columnIndex = grdAsmntPreview.HeaderRow.Cells.GetCellIndex(headerCell);
                }
            }

         }
    }
    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "ASC")
        {
            _sortDirection = "DESC";


        }
        else
        {
            _sortDirection = "ASC";

        }
    }
}