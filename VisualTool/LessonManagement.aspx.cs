using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class LessonManagement : System.Web.UI.Page
{
    // Creating the object instance of service name. 
    svc_lessonManagement.LessonManagementClient lp = new svc_lessonManagement.LessonManagementClient();
    svc_lessonManagement.clsRequest req = new svc_lessonManagement.clsRequest();
    public static LinkButton lbutton;
    clsData objData = null;
    DataClass oData = null;
    clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }

        // System.Threading.Thread.Sleep(5000);
        if (!IsPostBack)
        {
            if (sess != null)
            {
                lblLoginName.Text = sess.UserName;
            }
            setTitle();

            Session["DomainValue"] = null;
            Session["CatValue"] = null;
            Session["EditValue"] = null;
            //lblAlert.Text = "Under Construction";
            FillDomainList();
            FillGridLessonPlan();

            FillColor(btnAllCat);
        }

    }


    private void setTitle()
    {
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            object obj = objData.FetchValue("Select SchoolDesc from School Where SchoolId=" + sess.SchoolId + "");
            if (obj != null)
            {
                TitleName.InnerText = obj.ToString();


            }
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
    //        ddlDomain.DataTextField = "AsmntCat";
    //        ddlDomain.DataValueField = "AsmntLPRelId";
    //        ddlDomain.DataSource = dList;
    //        ddlDomain.DataBind();
    //        //DListDomains.DataSource = dList;
    //       // DListDomains.DataBind();
    //    }
    //}


    protected void FillGridLessonPlan()  // Fill Grid Lessons
    {
        try
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
        catch (Exception ex)
        {

        }

    }


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
    //[System.Web.Services.WebMethod(EnableSession = true)]
    //public static void ListCategory(string parameter1)
    //{
    //    svc_lessonManagement.LessonManagementClient lp = new svc_lessonManagement.LessonManagementClient();
    //    svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
    //    response = lp.GetCategoryList(parameter1);
    //    svc_lessonManagement.output[] listResult = response.outputList;
    //    //grdLessonPlans.DataSource = listResult;
    //    //grdLessonPlans.DataBind();
    //}




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

    //protected void btnContent_p_Click(object sender, EventArgs e)
    //{
    //    int domId;
    //    string catType = "content-ppt";
    //    FillColor(btnContent_p);
    //    Session["CatValue"] = catType.ToString();
    //    try
    //    {
    //        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
    //        if (Session["DomainValue"] != null)
    //        {
    //            domId = Convert.ToInt32(Session["DomainValue"]);
    //            req.catType = catType;
    //            req.domId = domId;
    //            response = lp.GetCatDomainLesson(req);
    //            svc_lessonManagement.output[] listData = response.outputList;
    //            grdLessonPlans.DataSource = listData;
    //            grdLessonPlans.DataBind();
    //            //Session["DomainValue"] = null;
    //        }
    //        else
    //        {
    //            req.catType = catType;
    //            response = lp.GetCategoryList(req);
    //            svc_lessonManagement.output[] listResult = response.outputList;
    //            grdLessonPlans.DataSource = listResult;
    //            grdLessonPlans.DataBind();
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //    }
    //}


    protected void lblDomainName_Click(object sender, EventArgs e)
    {

    }
    protected void btnCreateNew_Click(object sender, EventArgs e)
    {
        Session["EditValue"] = null;
        lblAlert.Text = "";
        Response.Redirect("AddLessonProfile.aspx");


    }

    protected void grdLessonPlans_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        // txtSearch.Text = "";
        lblAlert.Text = "";
        int lessId = 0;
        string catName;
        //  int isStEdit = 0;
        //  int isCcEdit = 0;
        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
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
                displayAlert("Sorry !!!  Work On Progresss!!!!!!!!",1,0);

                Session["EditValue"] = null;
            }

        }
        if (e.CommandName == "Delete")
        {
            lessId = Convert.ToInt32(e.CommandArgument);
            req.lesnId = lessId;
            response = lp.EditLessonData(req);
            svc_lessonManagement.output[] listData = response.outputList;
            if (listData.Length > 0)
            {
                catName = listData.ElementAt(0).lpType;
                req.catType = catName;
                response = lp.DeleteLessonPlan(req);
                string alert = response.outputString;
               
                LessonDetailsPaging();
                FillGridLessonPlan();

                //lblAlert.Text = alert;
                if (alert.Trim() == ("Deleted Successfully").Trim())
                {
                    displayAlert(alert, 0, 1);
                }
                else
                {
                    displayAlert(alert, 1, 0);
                }
                //response = lp.GetCategoryList(req);
                //svc_lessonManagement.output[] list = response.outputList;
                //grdLessonPlans.DataSource = list;
                //grdLessonPlans.DataBind();
                //   FillGridLessonPlan();
            }

        }

        if (e.CommandName == "Copy")
        {
            int lessonId = Convert.ToInt32(e.CommandArgument);
            ViewState["LessonIdRename"] = lessonId;
            FindCurrentLesson(lessonId);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), Guid.NewGuid().ToString(), clsGeneral.popUp(), true);


            //oData = new DataClass();
            //string oName = "CC";
            //int currentLessonId = Convert.ToInt32(e.CommandArgument);
            //try
            //{
            //    isStEdit = 0;
            //    isCcEdit = 1;
            //    string selctSpQuerry = "sp_copyLessonPlan";
            //    int newLessonId = oData.Execute_SpCopyLesson(selctSpQuerry, currentLessonId, isStEdit, isCcEdit, oName);
            //    LessonDetailsPaging();
            //}
            //catch (Exception Ex)
            //{

            //}
        }
    }
    //protected void grdLessonPlans_RowCommand1(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Edit")
    //    {
    //        int lessId = Convert.ToInt32(e.CommandArgument);
    //        Session["EditValue"] = lessId;
    //        Response.Redirect("AddLessonProfile.aspx");
    //    }
    //}





    protected void btnRenameDone_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        oData = new DataClass();
        int lessonId = Convert.ToInt32(ViewState["LessonIdRename"]);
        string newLessonName = clsGeneral.convertQuotes(txtLessonName.Text.ToString());
        txtLessonName.Text = "";
        string oName = "CC";

        try
        {
            int isStEdit = 0;
            int isCcEdit = 1;
            string selctSpQuerry = "sp_copyLessonPlanonRename";
            int newLessonId = oData.Execute_SpCopyLessonOnRename(selctSpQuerry, lessonId, isStEdit, isCcEdit, oName, newLessonName);
            LessonDetailsPaging();
        }
        catch (Exception Ex)
        {

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
        bool iIndex = true;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDesc = (Label)e.Row.FindControl("lblLessonDesc");
            Label lblDomain = (Label)e.Row.FindControl("lblDomainName");
            Button imgbtnEdit = (Button)e.Row.FindControl("imgEdit");
            Button imgDelete = (Button)e.Row.FindControl("imgDelete");
            int lessonID = Convert.ToInt32(imgbtnEdit.CommandArgument);
            string selData = "SELECT IsCC_Edit FROM LE_Lesson WHERE LessonId = " + lessonID;
            object chkNull = objData.FetchValue(selData);

            if (chkNull != null)
            {

                iIndex = (bool)(chkNull);
            }

            if (iIndex == false)
            {
                imgbtnEdit.Visible = false;
                imgDelete.Visible = false;
            }

            if (lblDesc.Text != null)
            {
                if (lblDesc.Text.ToString().Length > 150)
                {
                    lblDesc.Text = lblDesc.Text.ToString().Substring(0, 150) + "........";
                }
            }

            if (lblDomain.Text != null)
            {
                if (lblDomain.Text.ToString().Length > 20)
                {
                    lblDomain.Text = lblDomain.Text.ToString().Substring(0, 20) + "...";
                }
            }



        }


    }
    protected void imgHome_Click(object sender, ImageClickEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        sess.AdminView = 1;
        Response.Redirect("~/Administration/AdminHome.aspx");
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


    protected void grdLessonPlans_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Logout.aspx");
    }

    #region code by pramod
    // Code by: pramod
    // descp: funtion to alert message

    /// <summary>
    /// Funtion fo displaying alert box on the top.
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="status">Integer: to denote whether Green(0) or Red(1) box.</param>
    /// <param name="effect">Integer: to denote whether fading effect(1) or not(0).</param>
    public void displayAlert(string message, int status, int effect)
    {
        if (effect == 0)             // Effect 0: no fading. visible always until clicked.
        {
            lblAlert.Visible = true;
            lblAlert.Text = message;
        }
        if (effect == 1)             // Effect 1: fading. Will fade out withing specified time.
        {
           
            lblAlert.Visible = true;
            lblAlert.Text = message;

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "fadeAlert();", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel2, UpdatePanel2.GetType(), "Any Name you Like", "fadeAlert();", true);
        }


        switch (status) { 
            case 0:
                lblAlert.CssClass = "alertGreen";
                break;
            case 1: lblAlert.CssClass = "alertRed";
                break;
            default:
                break;
        }
        
    }

   
    // end

    #endregion
}