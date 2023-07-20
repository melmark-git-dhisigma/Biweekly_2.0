using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Phase002_1_StudentBinder_mouseEditor : System.Web.UI.Page
{
    svc_MouseEditor.MouseEditorClient mouse = new svc_MouseEditor.MouseEditorClient();
    svc_MouseEditor.ResponseResult mResponse = new svc_MouseEditor.ResponseResult();
    svc_MouseEditor.classRequest mRequest = new svc_MouseEditor.classRequest();
    svc_setStep.SetStepClient setStep = new svc_setStep.SetStepClient();
    clsSession sess = null;
    clsData oData = null;
    int lessonId = 0;
    int lessonDetailId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //System.Threading.Thread.Sleep(5000);

        if (!IsPostBack)
        {
            oData = new clsData();
            if (Session["LessonDetailId"] != null)
            {
                lessonDetailId = Convert.ToInt32(Session["LessonDetailId"]);
            }
            if (Session["LessonId"] != null)
            {
                lessonId = Convert.ToInt32(Session["LessonId"]);
                object objLP = oData.FetchValue("SELECT '[' + LessonName + '] - [' + Convert(varchar(20),LessonId) + '] - ' + ISNULL(OwnerName,'') FROM LE_Lesson WHERE LessonId=" + lessonId);
                if (objLP != null)
                    td_LP.InnerHtml = "<b>Lesson Name</b> : " + objLP.ToString();
            }
            if (Session["EditValue"] != null)
            {
                Session["LessonId"] = Session["EditValue"];
                lessonId = Convert.ToInt32(Session["LessonId"]);
                object objLP = oData.FetchValue("SELECT '[' + LessonName + '] - [' + Convert(varchar(20),LessonId) + '] - ' + ISNULL(OwnerName,'') FROM LE_Lesson WHERE LessonId=" + lessonId);
                if (objLP != null)
                    td_LP.InnerHtml = "<b>Lesson Name</b> : " + objLP.ToString();
                Session["EditValue"] = null;
                fillSetSteponEdit();

            }
            FillImageDataList();

            svc_setStep.setStep_request request1 = new svc_setStep.setStep_request();
            svc_setStep.setStep_response response1 = new svc_setStep.setStep_response();
            svc_setStep.setStep_request request = new svc_setStep.setStep_request();

            request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            fillSets(request.lessonId);
            lstSets.SelectedIndex = 0;

            request.setValue = Convert.ToInt32(lstSets.SelectedValue.ToString());
            fillSteps(request.lessonId, request.setValue);
            lstSteps.SelectedIndex = 0;


            request1.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            request1.setValue = Convert.ToInt32(lstSets.SelectedValue.ToString());
            request1.stepValue = Convert.ToInt32(lstSteps.SelectedValue.ToString());

            response1 = setStep.getLeDetId(request1);

            fn_orderArrows();

            Session["lessonDetId"] = response1.responseInt;

        }
    }

    protected void FillImageDataList()
    {
        mRequest.itemType = "images";
        mResponse = mouse.getElementTable(mRequest);
        svc_MouseEditor.MediaValue[] listNew = mResponse.responseList;
        //  svc_contentPage.media[] list = response.responseList;
        if (listNew.Length > 0)
        {
            dlImages.DataSource = listNew;
            dlImages.DataBind();
        }
    }
    protected void fillSetSteponEdit()
    {
        lstSets.SelectedIndex = 0;
        lstSteps.SelectedIndex = 0;
    }


    protected void imgBtn_AddSet_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();
        sess = (clsSession)Session["UserSession"];

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.loginId = sess.LoginId;

        response = setStep.AddSet(request);

        fillSets(request.lessonId);
        lstSets.SelectedIndex = lstSets.Items.Count - 1;
        fillSteps(request.lessonId, Convert.ToInt32(lstSets.SelectedValue.ToString()));
        lstSteps.SelectedIndex = lstSteps.Items.Count - 1;

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.stepValue = Convert.ToInt32(lstSteps.SelectedValue);
        request.setValue = Convert.ToInt32(lstSets.SelectedValue);

        response = setStep.getLeDetId(request);


        fn_orderArrows();

        Session["lessonDetId"] = response.responseInt;

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "imgAddClick();listBoxMenu();", true);
    }

    protected void imgBtn_AddStep_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        sess = (clsSession)Session["UserSession"];
        request.loginId = sess.LoginId;

        int x = lstSets.SelectedIndex;

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(lstSets.SelectedValue.ToString());
        response = setStep.AddStep(request);

        fillSets(request.lessonId);
        lstSets.SelectedIndex = x;

        fillSteps(request.lessonId, request.setValue);
        lstSteps.SelectedIndex = lstSteps.Items.Count - 1;


        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(lstSets.SelectedValue);
        request.stepValue = Convert.ToInt32(lstSteps.SelectedValue);

        response = setStep.getLeDetId(request);


        fn_orderArrows();

        Session["lessonDetId"] = response.responseInt;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "imgAddClick();listBoxMenu();", true);  // ???
    }
    protected void imgBtn_setDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (lstSets.Items.Count != 1)
        {
            svc_setStep.setStep_request request = new svc_setStep.setStep_request();
            svc_setStep.setStep_response response = new svc_setStep.setStep_response();

            request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            request.setValue = Convert.ToInt32(lstSets.SelectedValue.ToString());
            response = setStep.DeleteSet(request);

            fillSets(request.lessonId);
            lstSets.SelectedIndex = lstSets.Items.Count - 1;
            fillSteps(request.lessonId, Convert.ToInt32(lstSets.SelectedValue.ToString()));
            lstSteps.SelectedIndex = lstSteps.Items.Count - 1;


            fn_orderArrows();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "deleteFun();", true);
        }

    }
    protected void imgBtn_stepDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (lstSteps.Items.Count != 1)
        {
            svc_setStep.setStep_request request = new svc_setStep.setStep_request();
            svc_setStep.setStep_response response = new svc_setStep.setStep_response();

            request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
            request.setValue = Convert.ToInt32(lstSets.SelectedValue.ToString());
            request.stepValue = Convert.ToInt32(lstSteps.SelectedValue.ToString());
            response = setStep.DelelteStep(request);


            fillSteps(request.lessonId, Convert.ToInt32(lstSets.SelectedValue.ToString()));

            fn_orderArrows();
            lstSteps.SelectedIndex = lstSteps.Items.Count - 1;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "deleteFun();", true);
        }

    }

    private void fillSets(int lessonId)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();


        request.lessonId = lessonId;

        response = setStep.GetSets(request);
        svc_setStep.set_step_list[] list = response.responseList;
        lstSets.DataSource = list;
        lstSets.DataTextField = "text";
        lstSets.DataValueField = "value";
        lstSets.DataBind();
    }

    private void fillSteps(int lessonId, int setValue)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = lessonId;
        request.setValue = setValue;

        response = setStep.GetSteps(request);
        svc_setStep.set_step_list[] list = response.responseList;
        lstSteps.DataSource = list;
        lstSteps.DataTextField = "text";
        lstSteps.DataValueField = "value";
        lstSteps.DataBind();
    }

    [System.Web.Services.WebMethod(EnableSession = true)]

    public static string SaveData(object contents, int numObjects, int setNumber, int stepNumber, int speedLevel)
    {
        svc_MouseEditor.MouseEditorClient mp = new svc_MouseEditor.MouseEditorClient();
        svc_MouseEditor.classRequest mRequest = new svc_MouseEditor.classRequest();

        clsSession sess = null;
        sess = (clsSession)HttpContext.Current.Session["UserSession"];

        mRequest.numberObjects = numObjects;
        mRequest.speedValue = speedLevel;
        // mRequest.lessonDetailId = Convert.ToInt32(HttpContext.Current.Session["LessonDetailId"].ToString());
        mRequest.lessonId = Convert.ToInt32(HttpContext.Current.Session["LessonId"].ToString());
        mRequest.arrData = contents.ToString().Split(',');
        mRequest.setNumber = setNumber;
        mRequest.stepNumber = stepNumber;
        mRequest.loginId = sess.LoginId;
        //string[] value = contents.ToString().Split(',');
        svc_MouseEditor.ResponseResult mResponse = mp.SaveMouseData(mRequest);
        return mResponse.outString;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string GetElementData(int setNumber, int stepNumber)
    {
        clsSession sess = null;
        sess = (clsSession)HttpContext.Current.Session["UserSession"];
        svc_MouseEditor.MouseEditorClient mp = new svc_MouseEditor.MouseEditorClient();
        svc_MouseEditor.classRequest mRequest = new svc_MouseEditor.classRequest();
        svc_MouseEditor.ResponseResult mResponse = new svc_MouseEditor.ResponseResult();

        mRequest.lessonId = Convert.ToInt32(HttpContext.Current.Session["LessonId"].ToString());
        mRequest.setNumber = setNumber;
        mRequest.stepNumber = stepNumber;
        mRequest.loginId = sess.LoginId;
        svc_MouseEditor.ResponseResult response = mp.GetLessonVal(mRequest);
        string[] newVal = response.arryList;
        string contents = "";
        for (int i = 0; i < newVal.Length; i++)
        {
            contents += newVal[i] + ",";
        }

        contents = contents + '^' + response.outInt;
        return contents;

    }


    protected void btn_setUp_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.currNo = Convert.ToInt32(lstSets.SelectedValue);
        request.toNo = Convert.ToInt32(lstSets.Items[lstSets.SelectedIndex - 1].Value);

        response = setStep.ReorderSet(request);

        fillSets(request.lessonId);
        lstSets.SelectedValue = request.currNo.ToString();

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);
    }
    protected void btn_setDown_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.currNo = Convert.ToInt32(lstSets.SelectedValue);
        request.toNo = Convert.ToInt32(lstSets.Items[lstSets.SelectedIndex + 1].Value);

        response = setStep.ReorderSet(request);

        fillSets(request.lessonId);
        lstSets.SelectedValue = request.currNo.ToString();

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);
    }
    protected void btn_stepUp_Click(object sender, ImageClickEventArgs e)
    {

        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(lstSets.SelectedValue);
        request.currNo = Convert.ToInt32(lstSteps.SelectedValue);
        request.toNo = Convert.ToInt32(lstSteps.Items[lstSteps.SelectedIndex - 1].Value);

        response = setStep.ReorderStep(request);
        fillSteps(request.lessonId, request.setValue);
        lstSteps.SelectedValue = request.currNo.ToString();

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);

    }
    protected void btn_stepDown_Click(object sender, ImageClickEventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(lstSets.SelectedValue);
        request.currNo = Convert.ToInt32(lstSteps.SelectedValue);
        request.toNo = Convert.ToInt32(lstSteps.Items[lstSteps.SelectedIndex + 1].Value);

        response = setStep.ReorderStep(request);

        fillSteps(request.lessonId, request.setValue);
        lstSteps.SelectedValue = request.currNo.ToString();

        fn_orderArrows();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);

    }
    protected void lstSets_SelectedIndexChanged(object sender, EventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        int curentVal = lstSets.SelectedIndex;
        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.setValue = Convert.ToInt32(lstSets.SelectedValue.ToString());

        fillSets(request.lessonId);
        lstSets.SelectedIndex = curentVal;
        // Session["SetNumber"] = request.setValue;
        fillSteps(request.lessonId, request.setValue);
        lstSteps.SelectedIndex = 0;

        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "onLoadStepData();setListBoxFun();listBoxMenu();", true);
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);
    }

    protected void lstSteps_SelectedIndexChanged(object sender, EventArgs e)
    {
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();
        svc_setStep.setStep_response response = new svc_setStep.setStep_response();

        int x = lstSteps.SelectedIndex;

        request.lessonId = Convert.ToInt32(Session["lessonId"].ToString());
        request.stepValue = Convert.ToInt32(lstSteps.SelectedValue);
        request.setValue = Convert.ToInt32(lstSets.SelectedValue);

        // response = setStep.getLeDetId(request);

        fillSteps(request.lessonId, request.setValue);
        lstSteps.SelectedIndex = x;


        fn_orderArrows();

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, UpdatePanel1.GetType(), "", "setListBoxFun();listBoxMenu();", true);
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


    private void fn_orderArrows()
    {

        btn_setUp.Visible = true;
        btn_setDown.Visible = true;
        btn_stepUp.Visible = true;
        btn_stepDown.Visible = true;

        if (lstSets.Items.Count == 1)
        {
            btn_setUp.Visible = false;
            btn_setDown.Visible = false;
        }
        if (lstSteps.Items.Count == 1)
        {
            btn_stepUp.Visible = false;
            btn_stepDown.Visible = false;
        }
        if (lstSets.SelectedIndex == 0)
        {
            btn_setUp.Visible = false;
        }
        if (lstSets.SelectedIndex == (lstSets.Items.Count - 1))
        {
            btn_setDown.Visible = false;
        }
        if (lstSteps.SelectedIndex == 0)
        {
            btn_stepUp.Visible = false;
        }
        if (lstSteps.SelectedIndex == (lstSteps.Items.Count - 1))
        {
            btn_stepDown.Visible = false;
        }
    }
}