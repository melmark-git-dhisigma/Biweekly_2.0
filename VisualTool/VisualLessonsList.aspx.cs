using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Phase002_1_VisualLessonsList : System.Web.UI.Page
{
    svc_lessonManagement.LessonManagementClient lp = new svc_lessonManagement.LessonManagementClient();
    svc_lessonManagement.clsRequest req = new svc_lessonManagement.clsRequest();
    public static LinkButton lbutton;
    clsSession sess = null;
    ClsTemplateSession ObjTempSess = null;
    clsData objData = null;
    DataClass oData = null;
    int dsHeaderId = 0;
    int studid = 0;
    int lessonPlanID = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
      //  Button1.Visible = true;

        sess = (clsSession)Session["UserSession"];
        ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        if (ObjTempSess == null) return;
        if (!IsPostBack)
        {
            Session["DomainValue"] = null;
            Session["CatValue"] = null;
            Session["EditValue"] = null;



            if (ObjTempSess.TemplateId != null)
            {
                dsHeaderId = Convert.ToInt32(ObjTempSess.TemplateId);
            }

            FillDomainList();
            FillGridLessonPlan();


            FillColor(btnAllCat);
        }
        
    }


    protected void FillDomainList()
    {
        objData = new clsData();
        string selData = "SELECT DomainId As Id,DomName As Name FROM LE_Domain";
        objData.ReturnDropDownDomain(selData, ddlDomain);
    }
    //protected void FillDomainList() //Fill Domain List
    //{
    //    svc_lessonManagement.ResponseOut domain = new svc_lessonManagement.ResponseOut();    // Create object instance of class used in webservice.
    //    // Integrate the service function to the object instance of class
    //    domain = lp.GetDomainList();
    //    // the list get from service passed to a new list declared here
    //    svc_lessonManagement.output[] dList = domain.outputList;
    //    if (dList.Length > 0)
    //    {
    //        DListDomains.DataSource = dList;
    //        DListDomains.DataBind();
    //    }
    //}


    protected void FillGridLessonPlan()  // Fill Grid Lessons
    {
        lblAlert.Text = "";
        svc_lessonManagement.ResponseOut result = new svc_lessonManagement.ResponseOut();   // Create object instance of class used in webservice.
        // Integrate the service function to the object instance of class
        result = lp.getLessonsList();
        // the list get from service passed to a new list declared here
        svc_lessonManagement.output[] list = result.outputList;
        if (list.Length > 0)
        {
            grdLessonPlans.DataSource = list;
            grdLessonPlans.DataBind();
        }
        else
        {
            grdLessonPlans.DataSource = list;
            grdLessonPlans.DataBind();
        }

    }
    //protected void imgSearch_Click(object sender, ImageClickEventArgs e)
    //{
    //    int dId;
    //    string catName;
    //    string searchTextLesson = clsGeneral.convertQuotes(txtSearch.Text.Trim());
    //    svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
    //    if ((Session["DomainValue"] != null) && (Session["CatValue"] == null) && (searchTextLesson != ""))
    //    {
    //        dId = Convert.ToInt32(Session["DomainValue"]);
    //        req.domId = dId;
    //        req.searchName = searchTextLesson;
    //        response = lp.GetLessonDomainWithSearch(req);
    //        svc_lessonManagement.output[] listResult = response.outputList;
    //        grdLessonPlans.DataSource = listResult;
    //        grdLessonPlans.DataBind();
    //    }
    //    else if ((Session["DomainValue"] == null) && (Session["CatValue"] != null) && (searchTextLesson != ""))
    //    {
    //        catName = Session["CatValue"].ToString();
    //        req.catType = catName;
    //        req.searchName = searchTextLesson;
    //        response = lp.GetCategoryListWithSearch(req);
    //        svc_lessonManagement.output[] listResult = response.outputList;
    //        grdLessonPlans.DataSource = listResult;
    //        grdLessonPlans.DataBind();
    //    }
    //    else if ((Session["DomainValue"] != null) && (Session["CatValue"] != null) && (searchTextLesson != ""))
    //    {
    //        catName = Session["CatValue"].ToString();
    //        dId = Convert.ToInt32(Session["DomainValue"]);
    //        req.catType = catName;
    //        req.domId = dId;
    //        req.searchName = searchTextLesson;
    //        response = lp.GetCatDomainLessonWithSearch(req);
    //        svc_lessonManagement.output[] listData = response.outputList;
    //        grdLessonPlans.DataSource = listData;
    //        grdLessonPlans.DataBind();
    //    }
    //    else if ((Session["DomainValue"] == null) && (Session["CatValue"] == null) && (searchTextLesson == ""))
    //    {
    //        svc_lessonManagement.ResponseOut result = new svc_lessonManagement.ResponseOut();   // Create object instance of class used in webservice.
    //        // Integrate the service function to the object instance of class
    //        result = lp.getLessonsList();
    //        // the list get from service passed to a new list declared here
    //        svc_lessonManagement.output[] list = result.outputList;
    //        if (list.Length > 0)
    //        {
    //            grdLessonPlans.DataSource = list;
    //            grdLessonPlans.DataBind();
    //        }
    //    }
    //    else if ((Session["DomainValue"] == null) && (Session["CatValue"] == null) && (searchTextLesson != ""))
    //    {
    //        svc_lessonManagement.ResponseOut result = new svc_lessonManagement.ResponseOut();   // Create object instance of class used in webservice.
    //        string searchText = searchTextLesson.ToString();
    //        // Create object instance of class used in webservice
    //        req.searchName = searchText;
    //        // Integrate the service function to the object instance of class
    //        result = lp.GetSearchLesson(req);
    //        // the list get from service passed to a new list declared here
    //        svc_lessonManagement.output[] list = result.outputList;


    //        grdLessonPlans.DataSource = list;
    //        grdLessonPlans.DataBind();
    //    }
    //    else if (searchTextLesson == "")
    //    {
    //        Session["DomainValue"] = null;    // Clear the Session value of Selected domain and Category.
    //        Session["CatValue"] = null;
    //        Session["EditValue"] = null;
    //        FillDomainList();         // Load the Datalist Domain.
    //        FillColor();              // deselect the color of selected state of domain and category .
    //        string searchText = searchTextLesson.ToString();
    //        // Create object instance of class used in webservice.
    //        svc_lessonManagement.ResponseOut clFile = new svc_lessonManagement.ResponseOut();
    //        req.searchName = searchText;
    //        // Integrate the service function to the object instance of class
    //        clFile = lp.GetSearchLesson(req);
    //        // the list get from service passed to a new list declared here
    //        svc_lessonManagement.output[] list = clFile.outputList;


    //        grdLessonPlans.DataSource = list;
    //        grdLessonPlans.DataBind();
    //    }


    //}


    protected void grdLessonPlans_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdLessonPlans.PageIndex = e.NewPageIndex;
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "EditDelShow", "$");

        LessonDetailsPaging();
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "EditDelShow();", false);
    }

    protected void LessonDetailsPaging()
    {
        int dId;
        string catName;
        string searchTextLesson = clsGeneral.convertQuotes(txtSearch.Text.Trim());
        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
        if ((Session["DomainValue"] == null) && (Session["CatValue"] == null) && (searchTextLesson == ""))
        {
            svc_lessonManagement.ResponseOut result = new svc_lessonManagement.ResponseOut();   // Create object instance of class used in webservice.
            // Integrate the service function to the object instance of class
            result = lp.getLessonsList();
            // the list get from service passed to a new list declared here
            svc_lessonManagement.output[] list = result.outputList;
            if (list.Length > 0)
            {
                grdLessonPlans.DataSource = list;
                grdLessonPlans.DataBind();
            }
        }
        else if ((Session["DomainValue"] != null) && (Session["CatValue"] == null) && (searchTextLesson == ""))
        {
            dId = Convert.ToInt32(Session["DomainValue"]);
            req.domId = dId;
            response = lp.GetLessonDomain(req);
            svc_lessonManagement.output[] listResult = response.outputList;
            grdLessonPlans.DataSource = listResult;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] == null) && (Session["CatValue"] != null) && (searchTextLesson == ""))
        {
            catName = Session["CatValue"].ToString();
            req.catType = catName;
            response = lp.GetCategoryList(req);
            svc_lessonManagement.output[] listResult = response.outputList;
            grdLessonPlans.DataSource = listResult;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] != null) && (Session["CatValue"] != null) && (searchTextLesson == ""))
        {
            catName = Session["CatValue"].ToString();
            dId = Convert.ToInt32(Session["DomainValue"]);
            req.catType = catName;
            req.domId = dId;
            response = lp.GetCatDomainLesson(req);
            svc_lessonManagement.output[] listData = response.outputList;
            grdLessonPlans.DataSource = listData;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] != null) && (Session["CatValue"] == null) && (searchTextLesson != ""))
        {
            dId = Convert.ToInt32(Session["DomainValue"]);
            req.domId = dId;
            req.searchName = searchTextLesson;
            response = lp.GetLessonDomainWithSearch(req);
            svc_lessonManagement.output[] listResult = response.outputList;
            grdLessonPlans.DataSource = listResult;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] == null) && (Session["CatValue"] != null) && (searchTextLesson != ""))
        {
            catName = Session["CatValue"].ToString();
            req.catType = catName;
            req.searchName = searchTextLesson;
            response = lp.GetCategoryListWithSearch(req);
            svc_lessonManagement.output[] listResult = response.outputList;
            grdLessonPlans.DataSource = listResult;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] != null) && (Session["CatValue"] != null) && (searchTextLesson != ""))
        {
            catName = Session["CatValue"].ToString();
            dId = Convert.ToInt32(Session["DomainValue"]);
            req.catType = catName;
            req.domId = dId;
            req.searchName = searchTextLesson;
            response = lp.GetCatDomainLessonWithSearch(req);
            svc_lessonManagement.output[] listData = response.outputList;
            grdLessonPlans.DataSource = listData;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] == null) && (Session["CatValue"] == null) && (searchTextLesson != ""))
        {
            svc_lessonManagement.ResponseOut result = new svc_lessonManagement.ResponseOut();   // Create object instance of class used in webservice.
            string searchText = searchTextLesson.ToString();
            // Create object instance of class used in webservice
            req.searchName = searchText;
            // Integrate the service function to the object instance of class
            result = lp.GetSearchLesson(req);
            // the list get from service passed to a new list declared here
            svc_lessonManagement.output[] list = result.outputList;


            grdLessonPlans.DataSource = list;
            grdLessonPlans.DataBind();
        }
        else if (searchTextLesson == "")
        {
            Session["DomainValue"] = null;    // Clear the Session value of Selected domain and Category.
            Session["CatValue"] = null;
            Session["EditValue"] = null;
            FillDomainList();         // Load the Datalist Domain.
            FillColor();              // deselect the color of selected state of domain and category .
            string searchText = searchTextLesson.ToString();
            // Create object instance of class used in webservice.
            svc_lessonManagement.ResponseOut clFile = new svc_lessonManagement.ResponseOut();
            req.searchName = searchText;
            // Integrate the service function to the object instance of class
            clFile = lp.GetSearchLesson(req);
            // the list get from service passed to a new list declared here
            svc_lessonManagement.output[] list = clFile.outputList;


            grdLessonPlans.DataSource = list;
            grdLessonPlans.DataBind();
        }


    }

    protected void DListDomains_ItemCommand(object source, DataListCommandEventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        string catName;
        int dId;
        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
        lbutton = (LinkButton)e.CommandSource;
        //lbutton.CssClass = "lnkButtonstyle";
        lbutton.ForeColor = System.Drawing.Color.Red;
        foreach (DataListItem li in DListDomains.Items)
        {
            LinkButton lb = new LinkButton();
            lb = (LinkButton)li.FindControl("lblDomainName");
            if (lb != lbutton)
            {
                lb.ForeColor = System.Drawing.Color.Black;
                lb.Style.Add("font-weight", "normal");
            }
            else
                lb.Style.Add("font-weight", "bold");
        }

        dId = Convert.ToInt32(e.CommandArgument);
        Session["DomainValue"] = dId;
        if ((Session["CatValue"]) != null)
        {
            catName = Session["CatValue"].ToString();
            if (lbutton.Text == "All")
            {
                Session["DomainValue"] = null;
                req.domId = 0;
                req.catType = catName;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] list = response.outputList;
                grdLessonPlans.DataSource = list;
                grdLessonPlans.DataBind();
            }
            else
            {
                req.catType = catName;
                req.domId = dId;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
                //Session["CatValue"] = null;
            }
        }
        else
        {
            if (lbutton.Text == "All")
            {
                Session["DomainValue"] = null;
                req.domId = 0;
                response = lp.GetLessonDomain(req);
                svc_lessonManagement.output[] list = response.outputList;
                grdLessonPlans.DataSource = list;
                grdLessonPlans.DataBind();
            }
            else
            {
                req.domId = dId;
                response = lp.GetLessonDomain(req);
                svc_lessonManagement.output[] listResult = response.outputList;
                grdLessonPlans.DataSource = listResult;
                grdLessonPlans.DataBind();
            }
        }

    }


    protected void FillColor(System.Web.UI.WebControls.Button btnId)
    {
        btnCoin.CssClass = "co";
        btnMouse.CssClass = "mo";
        btnMatch.CssClass = "ma";
        btnTime.CssClass = "ti";
        btnContents.CssClass = "con";
        btnAllCat.CssClass = "al";
        //   btnContent_p.CssClass = "NFButton";

        if (btnId.ID == "btnCoin")
        {
            btnId.CssClass = "selctButtonCoin";
        }
        else if (btnId.ID == "btnMouse")
        {
            btnId.CssClass = "selctButtommouse";
        }
        else if (btnId.ID == "btnMatch")
        {
            btnId.CssClass = "selctButtonMatch";
        }
        else if (btnId.ID == "btnTime")
        {
            btnId.CssClass = "selctButtonTime";
        }
        else if (btnId.ID == "btnContents")
        {
            btnId.CssClass = "selctBtnContent";
        }
        else if (btnId.ID == "btnAllCat")
        {
            btnId.CssClass = "selctButtonAll";
        }
    }

    protected void FillColor()
    {
        btnCoin.CssClass = "co";
        btnMouse.CssClass = "mo";
        btnMatch.CssClass = "ma";
        btnTime.CssClass = "ti";
        btnContents.CssClass = "con";
        btnAllCat.CssClass = "al";
        //  btnContent_p.CssClass = "NFButton";
    }


    protected void btnCoin_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        int domId;
        string catType = "coin";
        FillColor(btnCoin);
        Session["CatValue"] = catType.ToString();
        try
        {
            svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
            if (Session["DomainValue"] != null)
            {
                domId = Convert.ToInt32(Session["DomainValue"]);
                req.catType = catType;
                req.domId = domId;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
                //Session["DomainValue"] = null;
            }
            else
            {
                req.catType = catType;
                response = lp.GetCategoryList(req);
                svc_lessonManagement.output[] listResult = response.outputList;
                grdLessonPlans.DataSource = listResult;
                grdLessonPlans.DataBind();
            }
        }
        catch (Exception Ex)
        {
        }

    }



    protected void btnMouse_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        int domId;
        string catType = "mouse";
        FillColor(btnMouse);
        Session["CatValue"] = catType.ToString();
        try
        {
            svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
            if (Session["DomainValue"] != null)
            {
                domId = Convert.ToInt32(Session["DomainValue"]);
                req.catType = catType;
                req.domId = domId;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
                //Session["DomainValue"] = null;
            }
            else
            {
                req.catType = catType;
                response = lp.GetCategoryList(req);
                svc_lessonManagement.output[] listResult = response.outputList;
                grdLessonPlans.DataSource = listResult;
                grdLessonPlans.DataBind();
            }
        }
        catch (Exception Ex)
        {
        }
    }

    protected void btnMatch_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        int domId;
        string catType = "match";
        FillColor(btnMatch);
        Session["CatValue"] = catType.ToString();
        try
        {
            svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
            if (Session["DomainValue"] != null)
            {
                domId = Convert.ToInt32(Session["DomainValue"]);
                req.catType = catType;
                req.domId = domId;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
                //Session["DomainValue"] = null;
            }
            else
            {
                req.catType = catType;
                response = lp.GetCategoryList(req);
                svc_lessonManagement.output[] listResult = response.outputList;
                grdLessonPlans.DataSource = listResult;
                grdLessonPlans.DataBind();
            }
        }
        catch (Exception Ex)
        {
        }
    }

    protected void btnTime_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        int domId;
        string catType = "time";
        FillColor(btnTime);
        Session["CatValue"] = catType.ToString();
        try
        {

            svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
            if (Session["DomainValue"] != null)
            {
                domId = Convert.ToInt32(Session["DomainValue"]);
                req.catType = catType;
                req.domId = domId;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
                //Session["DomainValue"] = null;
            }
            else
            {
                req.catType = catType;
                response = lp.GetCategoryList(req);
                svc_lessonManagement.output[] listResult = response.outputList;
                grdLessonPlans.DataSource = listResult;
                grdLessonPlans.DataBind();
            }
        }
        catch (Exception Ex)
        {

        }
    }
    protected void btnContents_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        int domId;
        string catType = "content";
        FillColor(btnContents);
        Session["CatValue"] = catType.ToString();
        try
        {
            svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
            if (Session["DomainValue"] != null)
            {
                domId = Convert.ToInt32(Session["DomainValue"]);
                req.catType = catType;
                req.domId = domId;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
                //Session["DomainValue"] = null;
            }
            else
            {
                req.catType = catType;
                response = lp.GetCategoryList(req);
                svc_lessonManagement.output[] listResult = response.outputList;
                grdLessonPlans.DataSource = listResult;
                grdLessonPlans.DataBind();
            }
        }
        catch (Exception Ex)
        {
        }
    }

    protected void btnAllCat_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        int domId;
        FillColor(btnAllCat);
        Session["CatValue"] = null;
        try
        {
            svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
            if (Session["DomainValue"] != null)
            {
                domId = Convert.ToInt32(Session["DomainValue"]);
                req.domId = domId;
                response = lp.GetLessonDomain(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
            }
            else
            {
                response = lp.getLessonsList();
                svc_lessonManagement.output[] list = response.outputList;
                grdLessonPlans.DataSource = list;
                grdLessonPlans.DataBind();
            }
        }
        catch (Exception Ex)
        {
        }

    }




    protected void lblDomainName_Click(object sender, EventArgs e)
    {

    }
    protected void btnCreateNew_Click(object sender, EventArgs e)
    {
        lblAlert.Text = "";
        Response.Redirect("AddLessonProfile.aspx");


    }

    protected void grdLessonPlans_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        objData = new clsData();
        oData = new DataClass();
        lblAlert.Text = "";
        int lessId = 0;
     //   int indexSp = 0;
   //    int newLessonId = 0;
      //  int isStEdit = 0;
    //    int isCcEdit = 0;
        string catName = "";
   //     string selctSpQuerry = "";
        string studentName = "";
        studid = sess.StudentId;
       // dsHeaderId = ObjTempSess.TemplateId;

        string selctStudentName = "SELECT StudentLname + StudentFname As StudentName FROm Student WHERE StudentId = " + studid;
        DataTable dtNew = objData.ReturnDataTable(selctStudentName, false);
        if (dtNew.Rows.Count > 0)
        {
            studentName = dtNew.Rows[0]["StudentName"].ToString();
            ViewState["StudentName"] = studentName;
        }

        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
        if (e.CommandName == "lessonplanValidation")
        {

            int lessonId = Convert.ToInt32(e.CommandArgument);
            ViewState["LessonIdAssign"] = lessonId;
            FindCurrentLesson(lessonId);
            btnRenameDone.Visible = false;
            btnAssgLpCopy.Visible = true;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUpfrIframe(), true);



            //string selctQuerry = "SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId = " + dsHeaderId + "";
            //DataTable dtList = objData.ReturnDataTable(selctQuerry, false);
            //if (dtList.Rows.Count > 0)
            //{
            //    lessonPlanID = Convert.ToInt32(dtList.Rows[0]["LessonPlanId"]);
            //}
            //   lessId = Convert.ToInt32(e.CommandArgument);
            // Session["VTLessonID"] = lessId;

            //try
            //{
            //    isStEdit = 1;
            //    isCcEdit = 0;
            //    selctSpQuerry = "sp_copyLessonPlan";     // Stored Procedure call for duplicate Lessonplan
            //    newLessonId = oData.Execute_SpCopyLesson(selctSpQuerry, lessId, isStEdit, isCcEdit, studentName);
            //    if (newLessonId > 0)
            //    {
            //        string selctLp = "SELECT MAX(LessonId) FROM LE_Lesson";
            //        int newValue = Convert.ToInt32(objData.FetchValue(selctLp));
            //        Session["VTLessonID"] = newValue;
            //        selctSpQuerry = "sp_InsertSetStepForVTool";       //Sp for Writing the no: of sets and steps fetch from Visualtool Lesson
            //        indexSp = oData.EcecuteScalar_SpSetStep(selctSpQuerry, sess.SchoolId, sess.LoginId, dsHeaderId, newValue);
            //    }

            //    if (indexSp == 0)
            //    {

            //        Response.Redirect("~/StudentBinder/TemplateEditor.aspx");
            //    }
            //    else
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The set-step Insertion Criteria Failed!!! Please try again !!!');", true);
            //    }

            //}
            //catch (Exception Ex)
            //{
            //}

        }

        if (e.CommandName == "Edit")
        {
            lessId = Convert.ToInt32(e.CommandArgument);
            Session["EditValue"] = lessId;
            req.lesnId = lessId;
            response = lp.EditLessonData(req);
            svc_lessonManagement.output[] listData = response.outputList;
            catName = listData.ElementAt(0).lpType;

            if ((catName == "content-single"))
            {
                Session["LessonId"] = lessId;
                Response.Redirect("content_pageNew.aspx?edit=1");
            }
            else if (catName == "content-ppt")
            {
                Session["LessonId"] = lessId;
                Response.Redirect("content_page_pptNew.aspx?edit=1");
            }
            else if (catName == "mouse")
            {
                Response.Redirect("mouseLesson_Creator.aspx?edit=1");

            }
            else if (catName == "time")
            {
                Response.Redirect("time_editorPage.aspx?edit=1");
            }
            else if (catName == "match")
            {
                Response.Redirect("matchingLessons.aspx?edit=1");
            }
            else if (catName == "coin")
            {
                Response.Redirect("coinLessons.aspx?edit=1");
            }

            else
            {
                lblAlert.Text = "Sorry !!!  Work On Progresss!!!!!!!!";
                Session["EditValue"] = null;
            }

        }

        if (e.CommandName == "Copy")
        {

            int lessonId = Convert.ToInt32(e.CommandArgument);
            ViewState["LessonIdRename"] = lessonId;
            FindCurrentLesson(lessonId);
            btnAssgLpCopy.Visible = false;
            btnRenameDone.Visible = true;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUpfrIframe(), true);



            //oData = new DataClass();
            //int currentLessonId = Convert.ToInt32(e.CommandArgument);
            //try
            //{
            //    isStEdit = 1;
            //    isCcEdit = 0;
            //    string selctSpQuerryNew = "sp_copyLessonPlan";
            //    int newId = oData.Execute_SpCopyLesson(selctSpQuerryNew, currentLessonId, isStEdit, isCcEdit, studentName);
            //    LessonDetailsPaging();
            //}
            //catch (Exception Ex)
            //{

            //}
        }

    }

    protected void FindCurrentLesson(int currentLessonId)
    {
        objData = new clsData();
        string selctLessonName = "SELECT LessonName FROM LE_Lesson WHERE LessonId = " + currentLessonId;
        string lessonName = (string)objData.FetchValue(selctLessonName);
        lblCurrentLessonName.Text = lessonName;
    }

    protected void grdLessonPlans_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void grdLessonPlans_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void imgGridMenu_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("LandingDesign.aspx");
    }

    protected void grdLessonPlans_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        objData = new clsData();
        //  bool iIndex = true;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button imgbtnEdit = (Button)e.Row.FindControl("imgEdit");
            Button imgDelete = (Button)e.Row.FindControl("imgDelete");
            Label lblDomain = (Label)e.Row.FindControl("lblDomainName");
            Label lblDesc = (Label)e.Row.FindControl("lblLessonDesc");
            Label lblStudentName = (Label)e.Row.FindControl("lblStudentName");

            int lessonId = Convert.ToInt32(imgbtnEdit.CommandArgument);


            imgbtnEdit.Visible = false;
            imgDelete.Visible = false;


            //string selctQuerry = "SELECT IsST_Edit FROM LE_Lesson WHERE LessonId = " + lessonId;
            //object chkNull = objData.FetchValue(selctQuerry);

            //if (chkNull != null)
            //{

            //    iIndex = (bool)(chkNull);
            //}

            //if (iIndex == false)
            //{
            //    //imgbtnEdit.Visible = false;
            //    //imgDelete.Visible = false;
            //}

            if (lblDesc.Text.ToString() != "")
            {
                if (lblDesc.Text.ToString().Length > 150)
                {
                    lblDesc.Text = lblDesc.Text.ToString().Substring(0, 150) + "........";
                }
            }

            if (lblDomain.Text.ToString() != "")
            {
                if (lblDomain.Text.ToString().Length > 20)
                {
                    lblDomain.Text = lblDomain.Text.ToString().Substring(0, 20) + "...";
                }
            }

            //if (lblStudentName.Text.ToString() == "")
            //{
            //    lblStudentName.Text = "Not Assigned";
            //}
        }

    }


    protected void imgSearch_Click(object sender, EventArgs e)
    {
        int dId;
        string catName;
        string searchTextLesson = clsGeneral.convertQuotes(txtSearch.Text.Trim());
        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
        if ((Session["DomainValue"] != null) && (Session["CatValue"] == null) && (searchTextLesson != ""))
        {
            dId = Convert.ToInt32(Session["DomainValue"]);
            req.domId = dId;
            req.searchName = searchTextLesson;
            response = lp.GetLessonDomainWithSearch(req);
            svc_lessonManagement.output[] listResult = response.outputList;
            grdLessonPlans.DataSource = listResult;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] == null) && (Session["CatValue"] != null) && (searchTextLesson != ""))
        {
            catName = Session["CatValue"].ToString();
            req.catType = catName;
            req.searchName = searchTextLesson;
            response = lp.GetCategoryListWithSearch(req);
            svc_lessonManagement.output[] listResult = response.outputList;
            grdLessonPlans.DataSource = listResult;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] != null) && (Session["CatValue"] != null) && (searchTextLesson != ""))
        {
            catName = Session["CatValue"].ToString();
            dId = Convert.ToInt32(Session["DomainValue"]);
            req.catType = catName;
            req.domId = dId;
            req.searchName = searchTextLesson;
            response = lp.GetCatDomainLessonWithSearch(req);
            svc_lessonManagement.output[] listData = response.outputList;
            grdLessonPlans.DataSource = listData;
            grdLessonPlans.DataBind();
        }
        else if ((Session["DomainValue"] == null) && (Session["CatValue"] == null) && (searchTextLesson == ""))
        {
            svc_lessonManagement.ResponseOut result = new svc_lessonManagement.ResponseOut();   // Create object instance of class used in webservice.
            // Integrate the service function to the object instance of class
            result = lp.getLessonsList();
            // the list get from service passed to a new list declared here
            svc_lessonManagement.output[] list = result.outputList;
            if (list.Length > 0)
            {
                grdLessonPlans.DataSource = list;
                grdLessonPlans.DataBind();
            }
        }
        else if ((Session["DomainValue"] == null) && (Session["CatValue"] == null) && (searchTextLesson != ""))
        {
            svc_lessonManagement.ResponseOut result = new svc_lessonManagement.ResponseOut();   // Create object instance of class used in webservice.
            string searchText = searchTextLesson.ToString();
            // Create object instance of class used in webservice
            req.searchName = searchText;
            // Integrate the service function to the object instance of class
            result = lp.GetSearchLesson(req);
            // the list get from service passed to a new list declared here
            svc_lessonManagement.output[] list = result.outputList;


            grdLessonPlans.DataSource = list;
            grdLessonPlans.DataBind();
        }
        else if (searchTextLesson == "")
        {
            Session["DomainValue"] = null;    // Clear the Session value of Selected domain and Category.
            Session["CatValue"] = null;
            Session["EditValue"] = null;
            FillDomainList();         // Load the Datalist Domain.
            FillColor();              // deselect the color of selected state of domain and category .
            string searchText = searchTextLesson.ToString();
            // Create object instance of class used in webservice.
            svc_lessonManagement.ResponseOut clFile = new svc_lessonManagement.ResponseOut();
            req.searchName = searchText;
            // Integrate the service function to the object instance of class
            clFile = lp.GetSearchLesson(req);
            // the list get from service passed to a new list declared here
            svc_lessonManagement.output[] list = clFile.outputList;


            grdLessonPlans.DataSource = list;
            grdLessonPlans.DataBind();
        }

    }

    protected void ddlDomain_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblAlert.Text = "";
        string catName;
        string domValue = "";
        int dId;
        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
        dId = Convert.ToInt32(ddlDomain.SelectedValue);
        Session["DomainValue"] = dId;
        if ((Session["CatValue"]) != null)
        {
            catName = Session["CatValue"].ToString();
            if (ddlDomain.DataTextField == "Select")
            {
                Session["DomainValue"] = null;
                req.domId = 0;
                req.catType = catName;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] list = response.outputList;
                grdLessonPlans.DataSource = list;
                grdLessonPlans.DataBind();
            }
            else
            {
                req.catType = catName;
                req.domId = dId;
                response = lp.GetCatDomainLesson(req);
                svc_lessonManagement.output[] listData = response.outputList;
                grdLessonPlans.DataSource = listData;
                grdLessonPlans.DataBind();
                //Session["CatValue"] = null;
            }
        }
        else
        {
            if (ddlDomain.DataTextField == "Select")
            {
                Session["DomainValue"] = null;
                req.domId = 0;
                response = lp.GetLessonDomain(req);
                svc_lessonManagement.output[] list = response.outputList;
                grdLessonPlans.DataSource = list;
                grdLessonPlans.DataBind();
            }
            else
            {
                req.domId = dId;
                response = lp.GetLessonDomain(req);
                svc_lessonManagement.output[] listResult = response.outputList;
                grdLessonPlans.DataSource = listResult;
                grdLessonPlans.DataBind();
            }
        }
    }

    protected void btnRenameDone_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        oData = new DataClass();
        string studentName = ViewState["StudentName"].ToString();
        int lessonId = Convert.ToInt32(ViewState["LessonIdRename"]);
        string newLessonName = clsGeneral.convertQuotes(txtLessonName.Text.ToString());
        txtLessonName.Text = "";
        try
        {
            int isStEdit = 1;
            int isCcEdit = 0;
            string selctSpQuerryNew = "sp_copyLessonPlanonRename";
            int newId = oData.Execute_SpCopyLessonOnRename(selctSpQuerryNew, lessonId, isStEdit, isCcEdit, studentName, newLessonName);
            LessonDetailsPaging();
        }
        catch (Exception Ex)
        {

        }

        ViewState["StudentName"] = null;


        //oData = new DataClass();
        //int currentLessonId = Convert.ToInt32(e.CommandArgument);
        //try
        //{
        //    isStEdit = 1;
        //    isCcEdit = 0;
        //    string selctSpQuerryNew = "sp_copyLessonPlan";
        //    int newId = oData.Execute_SpCopyLesson(selctSpQuerryNew, currentLessonId, isStEdit, isCcEdit, studentName);
        //    LessonDetailsPaging();
        //}
        //catch (Exception Ex)
        //{

        //}
    }
    protected void btnAssgLpCopy_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        oData = new DataClass();
        //ObjTempSess = (ClsTemplateSession)Session["BiweeklySession"];
        //sess = (clsSession)Session["UserSession"];
        int newLessonId = 0;
        int indexSp = 0;

        if (ObjTempSess.TemplateId != null)
        {

            int templateId = Convert.ToInt32(ObjTempSess.TemplateId);
            string studentName = ViewState["StudentName"].ToString();
            int lessonId = Convert.ToInt32(ViewState["LessonIdAssign"]);
            string newLessonName = clsGeneral.convertQuotes(txtLessonName.Text.ToString());
            txtLessonName.Text = "";

            try
            {
                int isStEdit = 1;
                int isCcEdit = 0;
                // Reasign lessonplan will update the asigned lessonplan to cc edit.
                if (Session["ReAssign"] != null)                       // Validation to check the event is ReAsign.
                {
                    string selData = "SELECT VTLessonId FROM DSTempHdr WHERE DSTempHdrId = " + templateId;
                    int vtLessonId = Convert.ToInt32(objData.FetchValue(selData));
                    string updateQuerry = "UPDATE LE_Lesson SET IsST_Edit = 0,IsCC_Edit = 1 WHERE LessonId = " + vtLessonId;
                    int index = objData.Execute(updateQuerry);
                    Session["ReAssign"] = null;
                }
                string selctSpQuerry = "sp_copyLessonPlanonRename";     // Stored Procedure call for duplicate Lessonplan
                newLessonId = oData.Execute_SpCopyLessonOnRename(selctSpQuerry, lessonId, isStEdit, isCcEdit, studentName, newLessonName);
                if (newLessonId > 0)
                {
                    string selctLp = "SELECT MAX(LessonId) FROM LE_Lesson";
                    int newValue = Convert.ToInt32(objData.FetchValue(selctLp));
                    Session["VTLessonID"] = newValue;
                    selctSpQuerry = "sp_InsertSetStepForVTool";       //Sp for Writing the no: of sets and steps fetch from Visualtool Lesson
                    indexSp = oData.EcecuteScalar_SpSetStep(selctSpQuerry, sess.SchoolId, sess.LoginId, templateId, newValue);
                }

                if (indexSp == 0)
                {

                    Response.Redirect("~/StudentBinder/CustomizeTemplateEditor.aspx?vLessonId="+templateId+"&copy=1");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('The set-step Insertion Criteria Failed!!! Please try again !!!');", true);
                }

            }
            catch (Exception Ex)
            {
            }

            ViewState["StudentName"] = null;
        }
    }
}