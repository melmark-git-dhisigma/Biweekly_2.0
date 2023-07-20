using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AddLessonProfile : System.Web.UI.Page
{
    int domValue = 0;
    string categoryName = "";
    static int lessplanId = 0;
    clsData objData = null;
    cls_data oData = null;
    clsSession sess = null;
    DataTable dtList = new DataTable();
    svc_lessonManagement.LessonManagementClient lp = new svc_lessonManagement.LessonManagementClient();
    svc_lessonManagement.clsRequest clReq = new svc_lessonManagement.clsRequest();
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        if (!IsPostBack)
        {

            if (!IsPostBack)
            {
                if (sess != null)
                {
                    lblLoginName.Text = sess.UserName;
                }
                FillDomainDropdown();
                FillDropCategory();
                if (Session["EditValue"] != null)
                {
                    lessplanId = Convert.ToInt32(Session["EditValue"]);
                    EditLessonData();
                }
                else
                {
                    lblMessage.Text = "";
                    lessplanId = 0;
                }

                setTitle();
            }
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


    //protected void DomainFillDrop()
    //{
    //    objData = new clsData();
    //    svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
    //    response = lp.FillDropDomainComplete();
    //    svc_lessonManagement.output[] listData = response.outputList;
    //    ddlDomain.DataSource = listData;
    //    ddlDomain.DataTextField = "AsmntCatName";
    //    ddlDomain.DataValueField = "domainId";
    //    ddlDomain.DataBind();
    //    ddlDomain.Items.Insert(0, new ListItem("..............SELECT.........", "0"));
    //    if ((Session["DomainValue"] != null) && (Session["CatValue"] != null))
    //    {
    //        domValue = Convert.ToInt32(Session["DomainValue"]);
    //        clReq.domId = domValue;
    //        response = lp.FillDropDomainWithId(clReq);
    //        svc_lessonManagement.output[] list = response.outputList;


    //    }
    //}

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

    protected void FillDropCategory()
    {
        svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
        if ((Session["DomainValue"] != null) && (Session["CatValue"] != null))
        {
            categoryName = Session["CatValue"].ToString();
            clReq.LessonType = categoryName;
            //response = lp.FillDropCategoryComplete();
            //svc_lessonManagement.output[] listData = response.outputList;
            //ddlCategory.DataSource = listData;
            //ddlCategory.DataValueField = "lpType";
            //ddlCategory.DataTextField = "lpType";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));
            ddlCategory.SelectedValue = categoryName;
            if (categoryName == "content")
            {
                ddlCategory.SelectedValue = "content";
                rdbtnContents.Visible = true;
                rdbtnContents.SelectedValue = "Single";
            }
            else if (categoryName == "mouse")
            {
                ddlCategory.SelectedValue = "mouse";
                // rdbtnotherCat.Visible = true;
                //  rdbtnotherCat.SelectedValue = "Discreate";
            }
            else if (categoryName == "time")
            {
                ddlCategory.SelectedValue = "time";
                //  rdbtnotherCat.Visible = true;
                // rdbtnotherCat.SelectedValue = "Discreate";
            }
            else if (categoryName == "match")
            {
                ddlCategory.SelectedValue = "match";
                // rdbtnotherCat.Visible = true;
                // rdbtnotherCat.SelectedValue = "Discreate";
            }
            else if (categoryName == "coin")
            {
                ddlCategory.SelectedValue = "coin";
                //   rdbtnotherCat.Visible = true;
                //  rdbtnotherCat.SelectedValue = "Discreate";
            }
            //response = lp.FillDropCategoryCat(clReq);
            //svc_lessonManagement.output[] listData1 = response.outputList;
            //if (listData1.Length > 0)
            //{
            //    string lptype = listData1.ElementAt(0).lpType;

            //    ddlCategory.SelectedValue = lptype;
            //    if ((lptype == "contents") || (lptype == "content-ppt"))
            //    {
            //        ddlCategory.SelectedValue = "contents";
            //        rdbtnContents.Visible = true;
            //        rdbtnContents.SelectedValue = "Single";
            //    }
            //}
            //else if (lptype == "content-ppt")
            //{
            //    rdbtnContents.Visible = true;
            //    rdbtnContents.SelectedValue = "PPT";
            //}

        }
        else if (Session["DomainValue"] != null)
        {
            //response = lp.FillDropCategoryComplete();
            //svc_lessonManagement.output[] listData = response.outputList;
            //ddlCategory.DataSource = listData;
            //ddlCategory.DataValueField = "lpType";
            //ddlCategory.DataTextField = "lpType";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));

        }
        else if (Session["CatValue"] != null)
        {
            categoryName = Session["CatValue"].ToString();
            clReq.LessonType = categoryName;
            //response = lp.FillDropCategoryComplete();
            //svc_lessonManagement.output[] listData = response.outputList;
            //ddlCategory.DataSource = listData;


            //ddlCategory.DataValueField = "lpType";
            //ddlCategory.DataTextField = "lpType";

            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));
            ddlCategory.SelectedValue = categoryName;
            if (categoryName == "content")
            {
                ddlCategory.SelectedValue = "content";
                rdbtnContents.Visible = true;
                rdbtnContents.SelectedValue = "Single";
            }
            else if (categoryName == "mouse")
            {
                ddlCategory.SelectedValue = "mouse";
                //   rdbtnotherCat.Visible = true;
                //   rdbtnotherCat.SelectedValue = "Discreate";
            }
            else if (categoryName == "time")
            {
                ddlCategory.SelectedValue = "time";
                //   rdbtnotherCat.Visible = true;
                //   rdbtnotherCat.SelectedValue = "Discreate";
            }
            else if (categoryName == "coin")
            {
                ddlCategory.SelectedValue = "coin";
                //   rdbtnotherCat.Visible = true;
                //   rdbtnotherCat.SelectedValue = "Discreate";
            }
            else if (categoryName == "match")
            {
                ddlCategory.SelectedValue = "match";
                //  rdbtnotherCat.Visible = true;
                //  rdbtnotherCat.SelectedValue = "Discreate";
            }

            //  response = lp.FillDropCategoryCat(clReq);
            //svc_lessonManagement.output[] listData1 = response.outputList;
            //if (listData1.Length > 0)
            //{
            //    string lptype = listData1.ElementAt(0).lpType;
            //    ddlCategory.SelectedValue = lptype;
            //    if ((lptype == "contents") || (lptype == "content-ppt"))
            //    {
            //        ddlCategory.SelectedValue = "contents";
            //        rdbtnContents.Visible = true;
            //        rdbtnContents.SelectedValue = "Single";
            //    }
            //}
            //else if (lptype == "content-ppt")
            //{
            //    rdbtnContents.Visible = true;
            //    rdbtnContents.SelectedValue = "PPT";
            //}

        }
        else
        {
            //response = lp.FillDropCategoryComplete();
            //svc_lessonManagement.output[] listData = response.outputList;
            //ddlCategory.DataSource = listData;
            //ddlCategory.DataValueField = "lpType";
            //ddlCategory.DataTextField = "lpType";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("..............SELECT.........", "0"));

        }

    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lessplanId == 0)
        {
            if (rdbtnContents.Visible == true)
            {
                SaveLesson();
            }
            else
            {

                SaveLessonOtherContents();
            }
        }
        else
        {
            UpdateLessonData();
        }

    }

    protected void ClearText()
    {
        txtDescription.Text = "";
        txtKeyword.Text = "";
        txtLessonName.Text = "";
        // txtsetNumber.Text = "";
        // txtstepNumber.Text = "";
        ddlCategory.SelectedIndex = 0;
        ddlDomain.SelectedIndex = 0;

    }

    protected void SaveLesson()
    {
        string lpCat;
        sess = (clsSession)Session["UserSession"];
        string lpName = clsGeneral.convertQuotes(txtLessonName.Text.Trim());
        string lpDesc = clsGeneral.convertQuotes(txtDescription.Text.Trim());
        string lpKeyword = clsGeneral.convertQuotes(txtKeyword.Text.Trim());
        int lpDomain = Convert.ToInt32(ddlDomain.SelectedValue);
        if (rdbtnContents.SelectedValue == "PPT")
        {
            lpCat = "content-ppt";
        }
        else if (rdbtnContents.SelectedValue == "Single")
        {
            lpCat = "content-single";
        }
        else
        {
            lpCat = ddlCategory.SelectedValue;
        }
        clReq.lesnName = lpName;
        clReq.lesnDesc = lpDesc;
        clReq.lpKey = lpKeyword;
        clReq.LessonType = lpCat;
        clReq.domId = lpDomain;
        clReq.loginId = sess.LoginId;
        svc_lessonManagement.ResponseOut response = lp.SaveLesson(clReq);

        string alert = response.outputString;
        if (alert == "Success")
        {
            int lessId = response.outputInt;
            Session["LessonId"] = lessId;
            if (lpCat == "content-single")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                  "alert",
                  "window.location = 'content_PageNew.aspx';", true);
            }
            else if (lpCat == "content-ppt")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                 "alert",
                 "window.location = 'content_page_pptNew.aspx';", true);
            }
            else
            {
                lblMessage.Text = alert;
            }
        }
        else
        {
            lblMessage.Text = alert;
        }

    }

    protected void SaveLessonOtherContents()
    {
        oData = new cls_data();
        sess = (clsSession)Session["UserSession"];
        int returnId;
        string lpCategory;
        string selctQuerry;
        string isDiscreate;
        string lpName = clsGeneral.convertQuotes(txtLessonName.Text.Trim());
        string lpDesc = clsGeneral.convertQuotes(txtDescription.Text.Trim());
        string lpKeyword = clsGeneral.convertQuotes(txtKeyword.Text.Trim());
        int lpDomain = Convert.ToInt32(ddlDomain.SelectedValue);
        int setNumber = 1;
        int stepNumber = 1;
        isDiscreate = "discreate";
        lpCategory = ddlCategory.SelectedValue;
        clReq.lesnName = lpName;
        clReq.lesnDesc = lpDesc;
        clReq.lpKey = lpKeyword;
        clReq.domId = lpDomain;
        clReq.setNmbr = setNumber;
        clReq.stepNmbr = stepNumber;
        clReq.discreate = isDiscreate;
        clReq.catType = lpCategory;
        clReq.loginId = sess.LoginId;

        svc_lessonManagement.ResponseOut response = lp.SaveOtherLessonType(clReq);
        string alert = response.outputString;

        if (alert == "Success")
        {
            int lessonId = response.outputInt;
            int lessonDetailId = response.outputInt2;
            if (lpCategory == "mouse")
            {
                Session["LessonId"] = lessonId;
                Session["LessonDetailId"] = lessonDetailId;
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                  "alert",
                   "window.location = 'mouseLesson_Creator.aspx';", true);

            }
            else if (lpCategory == "time")
            {
                Session["LessonId"] = lessonId;
                Session["LessonDetailId"] = lessonDetailId;
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                  "alert",
                   "window.location = 'time_editorPage.aspx';", true);

            }

            else if (lpCategory == "match")
            {
                Session["LessonId"] = lessonId;
                Session["EditValue"] = lessonId;
                Session["LessonDetailId"] = lessonDetailId;
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                  "alert",
                   "window.location = 'matchingLessons.aspx';", true);

            }

            else if (lpCategory == "coin")
            {
                Session["LessonId"] = lessonId;
                Session["EditValue"] = lessonId;
                Session["LessonDetailId"] = lessonDetailId;
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                  "alert",
                   "window.location = 'coinLessons.aspx';", true);

            }


        }
        else
        {
            lblMessage.Text = "Error!!! Insertion Failed!!!!";
            ClearText();

        }



        //try
        //{
        //    selctQuerry = "sp_InsertLesson";
        //    returnId = oData.ExecuteScalar_sp(selctQuerry, lpName, lpDesc, lpKeyword, lpDomain, lpCategory, isDiscreate, setNumber, stepNumber);

        //    if (returnId > 0)
        //    {
        //        if (lpCategory == "mouse")
        //        {
        //            Session["LessonDetailId"] = returnId;
        //            ScriptManager.RegisterStartupScript(this, this.GetType(),
        //             "alert",
        //             "alert('LessonPlan Details Saved Successfully ...');window.location = 'mouseLesson_Creator.aspx';", true);
        //        }
        //        else
        //        {
        //            lblMessage.Text = ("LessonDetails Inserted Successfully");
        //        }

        //    }
        //    else
        //    {
        //        lblMessage.Text = "Error!!! Insertion Failed!!!!";
        //        ClearText();

        //    }


        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.Text = "Error Insertion!!" + Ex.ToString();
        //}



    }
    protected void EditLessonData()
    {
        try
        {
            if (lessplanId > 0)
            {
                string catValue;
                btnSave.Text = "Update";
                btnCancel.Visible = false;
                lblMessage.Text = "";
                clReq.lesnId = lessplanId;
                svc_lessonManagement.ResponseOut response = new svc_lessonManagement.ResponseOut();
                response = lp.EditLessonData(clReq);
                svc_lessonManagement.output[] listData = response.outputList;
                if (listData.Length > 0)
                {
                    txtLessonName.Text = listData.ElementAt(0).lpName;
                    txtDescription.Text = listData.ElementAt(0).desc;
                    txtKeyword.Text = listData.ElementAt(0).lpKeywrd;
                    //ddlDomain.DataTextField = response.retDomainName;
                    ddlDomain.SelectedValue = listData.ElementAt(0).domainId.ToString();
                    // retDomId = Convert.ToInt32(listData.ElementAt(0).domainId);
                    catValue = listData.ElementAt(0).lpType;
                    //   ddlCategory.SelectedValue = listData.ElementAt(0).lpType;

                    if ((catValue == "content-single") || (catValue == "content-ppt"))
                    {
                        rdbtnContents.Visible = true;
                        if (catValue == "content-single")
                        {
                            rdbtnContents.SelectedValue = "Single";
                            ddlCategory.SelectedValue = "content";
                        }
                        else if (catValue == "content-ppt")
                        {
                            rdbtnContents.SelectedValue = "PPT";
                            ddlCategory.SelectedValue = "content";
                        }

                    }
                }

                //  Session["EditValue"] = null;
                //int dId = Convert.ToInt32(listData.ElementAt(0).domainId);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void UpdateLessonData()
    {
        try
        {
            clReq.lesnId = lessplanId;
            clReq.lesnName = clsGeneral.convertQuotes(txtLessonName.Text.Trim());
            clReq.lesnDesc = clsGeneral.convertQuotes(txtDescription.Text.Trim());
            clReq.lpKey = clsGeneral.convertQuotes(txtKeyword.Text.Trim());
            //clReq.domId = retDomId;
            clReq.domId = Convert.ToInt32(ddlDomain.SelectedValue);
            if (ddlCategory.SelectedValue == "content")
            {
                if (rdbtnContents.SelectedValue == "Single")
                {
                    clReq.LessonType = "content-single";
                }
                else if (rdbtnContents.SelectedValue == "PPT")
                {
                    clReq.LessonType = "content-ppt";
                }

            }
            else
            {
                clReq.LessonType = ddlCategory.SelectedValue;
            }
            svc_lessonManagement.ResponseOut response = lp.UpdateLesson(clReq);
            string alert = response.outputString;
            if (alert == "Success")
            {
                lblMessage.Text = "Details Updated Successfully ";
            }
            else
            {
                lblMessage.Text = alert;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Session["EditValue"] = null;
        Response.Redirect("LessonManagement.aspx");
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedValue == "content")
        {
            rdbtnContents.Visible = true;
            // rdbtnotherCat.Visible = false;
            rdbtnContents.SelectedValue = "Single";
            //  panelSetStepNumber.Visible = false;
            //if (ddlCategory.SelectedValue == "contents")
            //{
            //    rdbtnContents.SelectedValue = "Single";
            //}
            //else if (ddlCategory.SelectedValue == "content-ppt")
            //{
            //    rdbtnContents.SelectedValue = "PPT";
            //}
        }
        else
        {
            rdbtnContents.Visible = false;
            //  rdbtnotherCat.Visible = true;
            //    rdbtnotherCat.SelectedValue = "Discreate";
            //  panelSetStepNumber.Visible = true;
            // rdbtnotherCat.SelectedValue = "Discreate";
        }
        if (ddlCategory.SelectedIndex == 0)
        {
            rdbtnContents.Visible = false;
            rdbtnotherCat.Visible = false;
            //  panelSetStepNumber.Visible = false;
        }
    }
    //protected void rdbtnContents_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //if (rdbtnContents.SelectedValue == "Single")
    //    //{
    //    //    ddlCategory.SelectedValue = "contents";
    //    //}
    //    //else if (rdbtnContents.SelectedValue == "PPT")
    //    //{
    //    //    ddlCategory.SelectedValue = "content-ppt";
    //    //}
    //}
    protected void rdbtnotherCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnotherCat.SelectedValue == "Discreate")
        {
            //panelSetStepNumber.Visible = true;

        }
    }
}