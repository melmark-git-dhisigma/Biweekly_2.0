using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class profilePreview : System.Web.UI.Page
{
    int domValue = 0;
    string categoryName = "";
    static int lessplanId = 0;
    clsData objData = null;
    DataTable dtList = new DataTable();
    svc_lessonManagement.LessonManagementClient lp = new svc_lessonManagement.LessonManagementClient();
    svc_lessonManagement.clsRequest clReq = new svc_lessonManagement.clsRequest();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            if (!IsPostBack)
            {
               FillDomainDropdown();
            //    FillDropCategory();
                if (Session["EditValue"] != null)
                {
                    lessplanId = Convert.ToInt32(Session["EditValue"]);
                    EditLessonData();
                }
                if (Session["LessonId"] != null)
                {
                    lessplanId = Convert.ToInt32(Session["LessonId"]);
                    EditLessonData();
                }
                else
                {
                    //lblMessage.Text = "";
                    lessplanId = 0;
                }
            }
        }


    }


    protected void FillDomainDropdown()
    {

        objData = new clsData();
        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
        if ((Session["DomainValue"] != null) && (Session["CatValue"] != null))
        {
            domValue = Convert.ToInt32(Session["DomainValue"]);
            clReq.domId = domValue;
            response = lp.FillDropDomainComplete();
            svc_lessonManagement.output[] listData = response.outputList;

            ddlDomain.DataSource = listData;
            ddlDomain.DataValueField = "domainId";
            ddlDomain.DataTextField = "AsmntCatName";
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("..............SELECT.........", "0"));
            response = lp.FillDropDomainWithId(clReq);
            svc_lessonManagement.output[] listData1 = response.outputList;
            if (listData1.Length > 0)
            {
                ddlDomain.SelectedValue = listData1.ElementAt(0).domainId.ToString();
            }

        }
        else if (Session["DomainValue"] != null)
        {
            domValue = Convert.ToInt32(Session["DomainValue"]);
            clReq.domId = domValue;
            response = lp.FillDropDomainComplete();
            svc_lessonManagement.output[] listData = response.outputList;

            ddlDomain.DataSource = listData;
            ddlDomain.DataValueField = "domainId";
            ddlDomain.DataTextField = "AsmntCatName";
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("..............SELECT.........", "0"));
            response = lp.FillDropDomainWithId(clReq);
            svc_lessonManagement.output[] listData1 = response.outputList;
            if (listData1.Length > 0)
            {
                ddlDomain.SelectedValue = listData1.ElementAt(0).domainId.ToString();
            }

        }
        else if (Session["CatValue"] != null)
        {
            response = lp.FillDropDomainComplete();
            svc_lessonManagement.output[] listData = response.outputList;
            ddlDomain.DataSource = listData;
            ddlDomain.DataTextField = "AsmntCatName";
            ddlDomain.DataValueField = "domainId";
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("..............SELECT.........", "0"));

        }
        else
        {
            response = lp.FillDropDomainComplete();
            svc_lessonManagement.output[] listData = response.outputList;
            ddlDomain.DataSource = listData;
            ddlDomain.DataTextField = "AsmntCatName";
            ddlDomain.DataValueField = "domainId";
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("..............SELECT.........", "0"));

        }




    }

    //protected void FillDropCategory()
    //{
    //    svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
    //    if ((Session["DomainValue"] != null) && (Session["CatValue"] != null))
    //    {
    //        categoryName = Session["CatValue"].ToString();
    //        clReq.LessonType = categoryName;

    //        ddlCategory.DataBind();
    //        ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));
    //        ddlCategory.SelectedValue = categoryName;
    //        if (categoryName == "content")
    //        {
    //            ddlCategory.SelectedValue = "content";
    //            rdbtnContents.Visible = true;
    //            rdbtnContents.SelectedValue = "Single";
    //        }


    //    }
    //    else if (Session["DomainValue"] != null)
    //    {

    //        ddlCategory.DataBind();
    //        ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));

    //    }
    //    else if (Session["CatValue"] != null)
    //    {
    //        categoryName = Session["CatValue"].ToString();
    //        clReq.LessonType = categoryName;


    //        ddlCategory.DataBind();
    //        ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));
    //        ddlCategory.SelectedValue = categoryName;
    //        if (categoryName == "content")
    //        {
    //            ddlCategory.SelectedValue = "content";
    //            rdbtnContents.Visible = true;
    //            rdbtnContents.SelectedValue = "Single";
    //        }


    //    }
    //    else
    //    {
    //        ddlCategory.DataBind();
    //        ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));

    //    }

    //}


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lessplanId == 0)
        {
          //  SaveLesson();
        }
        else
        {
            UpdateLessonData();
        }

    }

    //protected void SaveLesson()
    //{
    //    string lpCat;
    //    string lpName = clsGeneral.convertQuotes(txtLessonName.Text.Trim());
    //    string lpDesc = clsGeneral.convertQuotes(txtDescription.Text.Trim());
    //    string lpKeyword = clsGeneral.convertQuotes(txtKeyword.Text.Trim());
    //    int lpDomain = Convert.ToInt32(ddlDomain.SelectedValue);
    //    if (rdbtnContents.SelectedValue == "PPT")
    //    {
    //        lpCat = "content-ppt";
    //    }
    //    else if (rdbtnContents.SelectedValue == "Single")
    //    {
    //        lpCat = "content-single";
    //    }
    //    else
    //    {
    //        lpCat = ddlCategory.SelectedValue;
    //    }
    //    clReq.lesnName = lpName;
    //    clReq.lesnDesc = lpDesc;
    //    clReq.lpKey = lpKeyword;
    //    clReq.LessonType = lpCat;
    //    clReq.domId = lpDomain;
    //    svc_lessonManagement.ResponseOut response = lp.SaveLesson(clReq);

    //    string alert = response.outputString;
    //    if (alert == "Success")
    //    {
    //        int lessId = response.outputInt;
    //        Session["LessonId"] = lessId;
    //        if (lpCat == "content-single")
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(),
    //              "alert",
    //              "alert('LessonPlan Details Saved Successfully ...');window.location = 'content_Page.aspx';", true);
    //        }
    //        else if (lpCat == "content-ppt")
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(),
    //             "alert",
    //             "alert('LessonPlan Details Saved Successfully ...');window.location = 'content_page_ppt.aspx';", true);
    //        }
    //        else
    //        {
    //            lblMessage.Text = alert;
    //        }
    //    }
    //    else
    //    {
    //        lblMessage.Text = alert;
    //    }

    //}
    protected void EditLessonData()
    {
        if (lessplanId > 0)
        {
            string catValue;
            btnSave.Text = "Update";
            btnCancel.Visible = false;           
            clReq.lesnId = lessplanId;
            svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
            response = lp.EditLessonData(clReq);
            svc_lessonManagement.output[] listData = response.outputList;
            if (listData.Length > 0)
            {
                txtLessonName.Text = listData.ElementAt(0).lpName;
                txtDescription.Text = listData.ElementAt(0).desc;
                txtKeyword.Text = listData.ElementAt(0).lpKeywrd;
                ddlDomain.SelectedIndex = listData.ElementAt(0).domainId;
              //  ddlDomain.SelectedValue = listData.ElementAt(0).domainId.ToString();
                // retDomId = Convert.ToInt32(listData.ElementAt(0).domainId);
                //catValue = listData.ElementAt(0).lpType;
                ////   ddlCategory.SelectedValue = listData.ElementAt(0).lpType;

                //if ((catValue == "content-single") || (catValue == "content-ppt"))
                //{
                //    rdbtnContents.Visible = true;
                //    if (catValue == "content-single")
                //    {
                //        rdbtnContents.SelectedValue = "Single";
                //        ddlCategory.SelectedValue = "content";
                //    }
                //    else if (catValue == "content-ppt")
                //    {
                //        rdbtnContents.SelectedValue = "PPT";
                //        ddlCategory.SelectedValue = "content";
                //    }

                //}
            }

           
        }
    }

    protected void UpdateLessonData()
    {
        clReq.lesnId = lessplanId;
        clReq.lesnName = clsGeneral.convertQuotes(txtLessonName.Text.Trim());
        clReq.lesnDesc = clsGeneral.convertQuotes(txtDescription.Text.Trim());
        clReq.lpKey = clsGeneral.convertQuotes(txtKeyword.Text.Trim());
        //clReq.domId = retDomId;
        clReq.domId = Convert.ToInt32(ddlDomain.SelectedValue);
        //if (ddlCategory.SelectedValue == "content")
        //{
        //    if (rdbtnContents.SelectedValue == "Single")
        //    {
        //        clReq.LessonType = "content-single";
        //    }
        //    else if (rdbtnContents.SelectedValue == "PPT")
        //    {
        //        clReq.LessonType = "content-ppt";
        //    }

        //}
        //else
        //{
        //    clReq.LessonType = ddlCategory.SelectedValue;
        //}
        svc_lessonManagement.ResponseOut response = lp.UpdateProfileLesson(clReq);
        string alert = response.outputString;
        if (alert == "Success")
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Lesson Details Updated Successfully");
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!! '" + alert + "'");
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Session["EditValue"] = null;
        Response.Redirect("LessonManagement.aspx");
    }

  
   
}