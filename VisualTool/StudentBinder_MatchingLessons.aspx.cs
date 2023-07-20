using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Phase002_1_StudentBinder_MatchingLessons : System.Web.UI.Page
{
    svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
    svc_setStep.SetStepClient setStep = new svc_setStep.SetStepClient();
    clsSession sess = null;
    clsData oData = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            sess = (clsSession)Session["UserSession"];
            if (sess == null)
            {
                Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
            }
            if (sess != null)
            {
                // lblLoginName.Text = sess.UserName;
            }
            oData = new clsData();
            svc_setStep.setStep_request request1 = new svc_setStep.setStep_request();
            svc_setStep.setStep_response response1 = new svc_setStep.setStep_response();

            Session["lessonId"] = Session["EditValue"];
            getElementByType("images");

            if (Session["lessonId"] != null)
            {
                object objLP = oData.FetchValue("SELECT '[' + LessonName + '] - [' + Convert(varchar(20),LessonId) + '] - ' + ISNULL(OwnerName,'') FROM LE_Lesson WHERE LessonId=" + Session["lessonId"].ToString());
                if (objLP != null)
                    td_LP.InnerHtml = "<b>Lesson Name</b> : " + objLP.ToString();
            }

            Session["type"] = "images";

            svc_setStep.setStep_request request = new svc_setStep.setStep_request();

            request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            fillSets(request.lessonId);
            ListBox1.SelectedIndex = 0;

            request.setValue = Convert.ToInt32(ListBox1.SelectedValue.ToString());
            fillSteps(request.lessonId, request.setValue);
            ListBox2.SelectedIndex = 0;


            request1.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            request1.setValue = Convert.ToInt32(ListBox1.SelectedValue.ToString());
            request1.stepValue = Convert.ToInt32(ListBox2.SelectedValue.ToString());

            response1 = setStep.getLeDetId(request1);

            Session["lessonDetId"] = response1.responseInt;
            fn_orderArrows();
        }
        // ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "makeAllDraggable();", true);
    }

    protected void optImage_Click(object sender, ImageClickEventArgs e)
    {
        getElementByType("images");
        Session["type"] = "images";
    }
    protected void optVideo_Click(object sender, ImageClickEventArgs e)
    {
        getElementByType("videos");
        Session["type"] = "videos";
    }
    protected void optAudio_Click(object sender, ImageClickEventArgs e)
    {
        getElementByType("audios");
        Session["type"] = "audios";
    }


    /// <summary>
    ///  Function to get the list of all the elements from database with the specified type.
    /// </summary>
    /// <param name="type"></param>
    private void getElementByType(string type)
    {
        svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        request.elemType = type;
        response = cp.getElementTable(request);
        svc_contentPage.media[] list = response.responseList;

        if (list.Length > 0)
        {
            DataList1.DataSource = list;
            DataList1.DataBind();
        }

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "makeAllDraggable();", true);
    }

    private void getSearchElements(string type, string name)
    {
        svc_contentPage.IcontentPageClient cp = new svc_contentPage.IcontentPageClient();
        svc_contentPage.clsResponse response = new svc_contentPage.clsResponse();
        svc_contentPage.content_clsRequest request = new svc_contentPage.content_clsRequest();

        if (type == "images")
        {
            request.elemType = "images";
            request.searchWord = name;

            response = cp.getSearchElements(request);
            svc_contentPage.media[] list = response.responseList;


            if (list.Length > 0)
            {
                DataList1.DataSource = list;
                DataList1.DataBind();
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "makeAllDraggable();", true);
        }
        else if (type == "videos")
        {
            request.elemType = "videos";
            request.searchWord = name;

            response = cp.getSearchElements(request);
            svc_contentPage.media[] list = response.responseList;

            if (list.Length > 0)
            {
                DataList1.DataSource = list;
                DataList1.DataBind();
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "makeAllDraggable();", true);
        }
        else if (type == "audios")
        {
            request.elemType = "audios";
            request.searchWord = name;

            response = cp.getSearchElements(request);
            svc_contentPage.media[] list = response.responseList;

            if (list.Length > 0)
            {
                DataList1.DataSource = list;
                DataList1.DataBind();
            }

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "makeAllDraggable();", true);
        }



    }

    protected void imgBtn_setDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (ListBox1.Items.Count != 1)
        {
            svc_setStep.setStep_request request = new svc_setStep.setStep_request();
            svc_setStep.setStep_response response = new svc_setStep.setStep_response();

            request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            request.setValue = Convert.ToInt32(ListBox1.SelectedValue.ToString());
            response = setStep.DeleteSet(request);

            fillSets(request.lessonId);
            ListBox1.SelectedIndex = ListBox1.Items.Count - 1;
            fillSteps(request.lessonId, Convert.ToInt32(ListBox1.SelectedValue.ToString()));
            ListBox2.SelectedIndex = ListBox2.Items.Count - 1;

            fn_orderArrows();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "makeAllDraggable();listBoxMenu();", true);
        }

        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "deleteFun();", true);
        }
    }
    protected void imgBtn_setAdd_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();
        sess = (clsSession)Session["UserSession"];

        request.loginId = sess.LoginId;

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        response = setStep.AddSet(request);

        fillSets(request.lessonId);
        ListBox1.SelectedIndex = ListBox1.Items.Count - 1;
        fillSteps(request.lessonId, Convert.ToInt32(ListBox1.SelectedValue.ToString()));
        ListBox2.SelectedIndex = ListBox2.Items.Count - 1;

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.stepValue = Convert.ToInt32(ListBox2.SelectedValue);
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue);

        response = setStep.getLeDetId(request);

        Session["lessonDetId"] = response.responseInt;

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "fn_getAllLeOptIds();fn_clearAll();fn_addDist('Q');fn_addDist('W');makeAllDraggable();listBoxMenu();", true);
    }
    protected void imgBtn_stepDelete_Click(object sender, ImageClickEventArgs e)
    {

        if (ListBox2.Items.Count != 1)
        {
            svc_setStep.setStep_request request = new svc_setStep.setStep_request();
            svc_setStep.setStep_response response = new svc_setStep.setStep_response();

            request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            request.setValue = Convert.ToInt32(ListBox1.SelectedValue.ToString());
            request.stepValue = Convert.ToInt32(ListBox2.SelectedValue.ToString());
            response = setStep.DelelteStep(request);


            fillSteps(request.lessonId, Convert.ToInt32(ListBox1.SelectedValue.ToString()));
            ListBox2.SelectedIndex = ListBox2.Items.Count - 1;

            fn_orderArrows();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "makeAllDraggable();listBoxMenu();", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "deleteFun();", true);
        }
    }
    protected void imgBtn_stepAdd_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        sess = (clsSession)Session["UserSession"];

        int x = ListBox1.SelectedIndex;

        request.loginId = sess.LoginId;

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue.ToString());
        response = setStep.AddStep(request);

        fillSets(request.lessonId);
        ListBox1.SelectedIndex = x;

        fillSteps(request.lessonId, request.setValue);
        ListBox2.SelectedIndex = ListBox2.Items.Count - 1;


        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue);
        request.stepValue = Convert.ToInt32(ListBox2.SelectedValue);

        response = setStep.getLeDetId(request);

        Session["lessonDetId"] = response.responseInt;

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "fn_getAllLeOptIds();fn_clearAll();fn_addDist('Q');fn_addDist('W');makeAllDraggable();listBoxMenu();", true);

    }


    /// <summary>
    /// Fill all sets providing lessonId
    /// </summary>
    /// <param name="lessonId"></param>
    private void fillSets(int lessonId)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();


        request.lessonId = lessonId;

        response = setStep.GetSets(request);
        svc_setStep.set_step_list[] list = response.responseList;
        ListBox1.DataSource = list;
        ListBox1.DataTextField = "text";
        ListBox1.DataValueField = "value";
        ListBox1.DataBind();
    }

    /// <summary>
    /// This Function is used to fill all the steps providing lessonId and setValue
    /// </summary>
    /// <param name="lessonId"></param>
    /// <param name="setValue"></param>
    private void fillSteps(int lessonId, int setValue)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = lessonId;
        request.setValue = setValue;

        response = setStep.GetSteps(request);
        svc_setStep.set_step_list[] list = response.responseList;
        ListBox2.DataSource = list;
        ListBox2.DataTextField = "text";
        ListBox2.DataValueField = "value";
        ListBox2.DataBind();
    }
    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();



        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue.ToString());
        fillSteps(request.lessonId, request.setValue);

        ListBox2.SelectedIndex = 0;

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.stepValue = Convert.ToInt32(ListBox2.SelectedValue);
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue);

        response = setStep.getLeDetId(request);

        int x = ListBox1.SelectedIndex;
        fillSets(request.lessonId);
        ListBox1.SelectedIndex = x;

        Session["lessonDetId"] = response.responseInt;


        fn_orderArrows();
        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "fn_getAllLeOptIds();makeAllDraggable();listBoxMenu()", true);

    }
    protected void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();



        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.stepValue = Convert.ToInt32(ListBox2.SelectedValue);
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue);

        response = setStep.getLeDetId(request);

        int x = ListBox2.SelectedIndex;
        fillSteps(request.lessonId, request.setValue);
        ListBox2.SelectedIndex = x;

        Session["lessonDetId"] = response.responseInt;

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "fn_getAllLeOptIds();makeAllDraggable();listBoxMenu()", true);
    }
    protected void btn_setUp_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.currNo = Convert.ToInt32(ListBox1.SelectedValue);
        request.toNo = Convert.ToInt32(ListBox1.Items[ListBox1.SelectedIndex - 1].Value);

        response = setStep.ReorderSet(request);

        fillSets(request.lessonId);
        ListBox1.SelectedValue = request.currNo.ToString();

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "listBoxMenu()", true);
    }
    protected void btn_setDown_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.currNo = Convert.ToInt32(ListBox1.SelectedValue);
        request.toNo = Convert.ToInt32(ListBox1.Items[ListBox1.SelectedIndex + 1].Value);

        response = setStep.ReorderSet(request);

        fillSets(request.lessonId);
        ListBox1.SelectedValue = request.currNo.ToString();

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "listBoxMenu()", true);

    }
    protected void btn_stepUp_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue);
        request.currNo = Convert.ToInt32(ListBox2.SelectedValue);
        request.toNo = Convert.ToInt32(ListBox2.Items[ListBox2.SelectedIndex - 1].Value);

        response = setStep.ReorderStep(request);

        fillSteps(request.lessonId, request.setValue);
        ListBox2.SelectedValue = request.currNo.ToString();

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "listBoxMenu()", true);
    }
    protected void btn_stepDown_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(ListBox1.SelectedValue);
        request.currNo = Convert.ToInt32(ListBox2.SelectedValue);
        request.toNo = Convert.ToInt32(ListBox2.Items[ListBox2.SelectedIndex + 1].Value);

        response = setStep.ReorderStep(request);

        fillSteps(request.lessonId, request.setValue);
        ListBox2.SelectedValue = request.currNo.ToString();

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(up_setStep, up_setStep.GetType(), "", "listBoxMenu()", true);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string saveDist(string objects, string status, string leOptId)
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        request.le_detailsId = Convert.ToInt32(HttpContext.Current.Session["lessonDetId"].ToString());
        request.objects = objects;
        request.status = status;
        request.le_optionId = Convert.ToInt32(leOptId);

        svc_matchingLesson.ml_response response = ml.saveDist(request);

        return response.responseString;
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string createDist(string status)
    {
        clsSession sess = null;
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        request.le_detailsId = Convert.ToInt32(HttpContext.Current.Session["lessonDetId"].ToString());
        request.objects = "";
        request.status = status;
        request.loginId = sess.LoginId;

        svc_matchingLesson.ml_response response = ml.createDist(request);

        return response.responseString;
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string deleteDist(string distId)
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        request.le_optionId = Convert.ToInt32(distId);

        svc_matchingLesson.ml_response response = ml.deleteDist(request);

        return response.responseString;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getTempData(string leOptId)
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        request.le_optionId = Convert.ToInt32(leOptId);

        svc_matchingLesson.ml_response response = ml.getDist(request);
        svc_matchingLesson.ml_request[] list = response.responseList;


        return list.ElementAt(0).objects + "^" + list.ElementAt(0).status;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getLeOptId()
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        request.le_detailsId = Convert.ToInt32(HttpContext.Current.Session["lessonDetId"].ToString());

        svc_matchingLesson.ml_response response = ml.getLeOptIds(request);

        return response.responseString2;
    }



    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getAllTempData(string leOptId)
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        string resultString = "";

        request.le_optionIdList = leOptId;

        svc_matchingLesson.ml_response response = ml.getDistAll(request);
        svc_matchingLesson.ml_request[] list = response.responseList;

        for (int i = 0; i < list.Length; i++)
        {
            resultString += list.ElementAt(i).objects + "^" + list.ElementAt(i).status + "^" + list.ElementAt(i).le_optionId + "☺";
        }

        return resultString;
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string rename(string S_No, string value)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();
        svc_setStep.SetStepClient setStep = new svc_setStep.SetStepClient();

        request.toNo = Convert.ToInt32(S_No);
        request.lessonId = Convert.ToInt32(HttpContext.Current.Session["lessonId"].ToString());
        request.changeValue = value;

        response = setStep.rename(request);

        return response.responseString;
    }
    protected void imgSearch_Click(object sender, ImageClickEventArgs e)
    {
        getSearchElements(Session["type"].ToString(), TextBox1.Text);
    }

    private void fn_orderArrows()
    {

        btn_setUp.Visible = true;
        btn_setDown.Visible = true;
        btn_stepUp.Visible = true;
        btn_stepDown.Visible = true;

        if (ListBox1.Items.Count == 1)
        {
            btn_setUp.Visible = false;
            btn_setDown.Visible = false;
        }
        if (ListBox2.Items.Count == 1)
        {
            btn_stepUp.Visible = false;
            btn_stepDown.Visible = false;
        }
        if (ListBox1.SelectedIndex == 0)
        {
            btn_setUp.Visible = false;
        }
        if (ListBox1.SelectedIndex == (ListBox1.Items.Count - 1))
        {
            btn_setDown.Visible = false;
        }
        if (ListBox2.SelectedIndex == 0)
        {
            btn_stepUp.Visible = false;
        }
        if (ListBox2.SelectedIndex == (ListBox2.Items.Count - 1))
        {
            btn_stepDown.Visible = false;
        }
    }
    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Logout.aspx");
    }
}