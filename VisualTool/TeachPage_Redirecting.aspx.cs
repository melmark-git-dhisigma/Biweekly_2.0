using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class Phase002_1_TeachPage_Redirecting : System.Web.UI.Page
{
    clsData objData = null;
    int currentLessonId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            if (Session["VTLessonId"] != null)
            {
                currentLessonId = Convert.ToInt32(Session["VTLessonId"]);
                //Session["setvalue"] = Request.QueryString["crntSet"].ToString();
                //Session["stepValue"] = Request.QueryString["crntStep"].ToString();
                //string SessnHdr = Request.QueryString["sessnHdr"].ToString();
                SelectLessonTypeAndRedirecting();
            }             
        }

    }


    protected void SelectLessonTypeAndRedirecting()
    {
        objData = new clsData();
        string selctLessonType = "SELECT LessonType FROM LE_Lesson WHERE LessonId =  " + currentLessonId;
        string type = (string)objData.FetchValue(selctLessonType);
        if ((Session["setvalue"] != null))
        {
            int setValue = Convert.ToInt32(Session["setvalue"]);
            int stepReturn = FindStep(setValue);
            Session["stepValue"] = stepReturn;           


            if (type == "time")
            {
                Response.Redirect("TeachPage_timeEditor.aspx");
            }
            else if (type == "mouse")
            {
                Response.Redirect("TeachPage_mouseEditor.aspx");
            }
            else if (type == "match")
            {
                Response.Redirect("Teach_Page_Match.aspx");
            }
            else if (type == "coin")
            {
                Response.Redirect("TeachPage_Coin.aspx");
            }
        }
    }

    protected int FindStep(int setNumber)
    {
        objData = new clsData();
        string selctList = "SELECT TOP 1 S_No FROM LE_SetStep WHERE SetValue = " + setNumber;
        int sValue = (int)objData.FetchValue(selctList);
        return sValue;
    }
}