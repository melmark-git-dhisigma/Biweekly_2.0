using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Script.Services;

public partial class AdminMaster1 : System.Web.UI.MasterPage
{
    public static clsSession sess = null;
    clsData ObjData = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        else
        {
            sess.AdmStudentId = 0;
        }
        
        if (!IsPostBack)
        {
            if (sess != null)
            {
                lblLoginName.Text = sess.UserName;
                if (sess.Gender == "Male")
                {
                    imgUserIcon.ImageUrl = "~/Administration/images/boyIcon.png";
                }
                else
                {
                    imgUserIcon.ImageUrl = "~/Administration/images/GirlIcon.png";
                }
                string studInfo = clsGeneral.PageStudentInfo();
                if (studInfo != "Student Info")
                {
                    string Title = clsGeneral.PageTitle(sess.perPageName);
                    Page.Title = "Melmark :: " + Title;
                    if (Title == "Welcome") Title = "Welcome " + sess.UserName;
                    HeadingDiv.InnerText = Title;
                }
                else
                {
                    Page.Title = "Melmark :: " + studInfo;
                    HeadingDiv.InnerText = studInfo;
                }
                fillclass();
               // for (int year = 2012; year <= (DateTime.Now.Year + 5); year++)
               // {
                    //dropyear.Items.Add(new ListItem(year.ToString(),year.ToString()));
                    //dropyear.Items.Add(new ListItem(year.ToString() + "-" + ((year + 1).ToString()), year.ToString()));

              //  }
                //dropyear.Items.FindByValue(((DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString())).Selected = true;
                string strQuery = "";
                clsData objData = new clsData();
                objData = new clsData();
                strQuery = "select AsmntYearCode from AsmntYear where CurrentInd = 'A'";
                String a = objData.FetchValue(strQuery).ToString();
              //  dropyear.Items.FindByText(a).Selected = true;

            }

        }
        setTitle();
        //for ( int year = 2012; year <= (DateTime.Now.Year + 5); year++)
        //{
        //    //dropyear.Items.Add(new ListItem(year.ToString(),year.ToString()));
        //    dropyear.Items.Add(new ListItem(year.ToString() + "-" + ((year+1).ToString()), year.ToString()));
             
        //}
        ////dropyear.Items.FindByValue(((DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString())).Selected = true;
        //string strQuery = "";
        //clsData objData = new clsData();
        //objData = new clsData();
        //strQuery = "select AsmntYearCode from AsmntYear where CurrentInd = 'A'";
        //String a = objData.FetchValue(strQuery).ToString();
        //dropyear.Items.FindByText(a).Selected = true;
    }

    private void setTitle()
    {
        clsData objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        if (sess != null)
        {
            object obj = objData.FetchValue("Select SchoolDesc from School Where SchoolId='" + sess.SchoolId + "'");
            if (obj != null)
            {
                TitleName.Text = obj.ToString();
            }
        }
    }
    public void fillclass()
    {
        sess = (clsSession)Session["UserSession"];
        clsData objData = new clsData();
        string classdetail = "";
        if (objData.IFExists("SELECT ClassId from UserClass where UserId='" + sess.LoginId + "'") == true)
        {
            classdetail = "SELECT UsrCls.ClassId AS Id,Cls.ClassName AS Name FROM UserClass UsrCls INNER JOIN Class Cls ON Cls.ClassId=UsrCls.ClassId WHERE UsrCls.UserId=" + sess.LoginId + " And Cls.SchoolId=" + sess.SchoolId + " AND Cls.ActiveInd='A' and UsrCls.ActiveInd='A' ORDER BY Name ASC";
        }
        else
            classdetail = "SELECT Cls.ClassId AS Id,Cls.ClassName AS Name FROM Class Cls WHERE Cls.ActiveInd='A'";

        DataTable dt = objData.ReturnDataTable(classdetail, false);
        if (dt.Rows.Count > 0)
        {
            Lblnoclass.Visible = false;
            DlClass.Visible = true;
            lblchoose.Visible = true;
            DlClass.DataSource = dt;
            DlClass.DataBind();
        }
        else
        {
            Lblnoclass.Visible = true;
            DlClass.Visible = false;
            lblchoose.Visible = false;
            Lblnoclass.Text = "Class Not Assigned...";
        }
        //}
    }





    protected void lnk_logout_Click(object sender, EventArgs e)
    {

    }




    protected void DlClass_ItemCommand(object source, DataListCommandEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        sess.Classid = Convert.ToInt32(e.CommandArgument);
        Response.Redirect("~/StudentBinder/Home.aspx");
    }




//    protected void dropyear_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        string strQuery = "";
//        string strQry = "";
//        string strQuy = "";
//        string strQey = "";
//        string sQuery = "";
//        string strQy = "";
//        clsData objData = new clsData();
//        objData = new clsData();
//        DataTable dt = null;
////        String selected = dropyear.SelectedItem.Text; //selected text from drop down
//        int ExistYear =Convert.ToInt32( objData.FetchValue( "select COUNT(*) from AsmntYear WHERE AsmntYearCode='" + selected + "'"));
//        //dt = objData.ReturnDataTable(strQuery, false); //asmntyearcodes from table asmntyear
//        //DataRow[] foundRows;
//        //foundRows = dt.Select("AsmntYearCode='"+selected+"'");
//        if (ExistYear != 0) //selected value exists in table
//        {
//                //check if CurrentInd = A. so no change.
//                //if CurrentInd =D, make it to A and change all others to D
//            strQry = "select CurrentInd from AsmntYear where AsmntYearCode ='" + selected + "'";
//            String curntInd = objData.FetchValue(strQry).ToString(); //returns the curentInd from table
//            if (curntInd == "D")
//              {
//                  strQuy = "update AsmntYear set CurrentInd = 'A', ModifiedBy = '" + sess.LoginId +"', ModifiedOn = GETDATE() where AsmntYearCode ='" + selected +"'";
//                  objData.Execute(strQuy);
//                  strQey = "update AsmntYear set CurrentInd = 'D', ModifiedBy = '" + sess.LoginId + "', ModifiedOn = GETDATE() where AsmntYearCode <>'" + selected + "'";
//                  objData.Execute(strQey);

//              }
//          }
               
//          else
//            {
//                //add the selected value row to table
////                string startdt = (dropyear.SelectedItem.Value).ToString() + "-01-01";
//  //              string enddt = (dropyear.SelectedItem.Value).ToString() + "-12-31";
//                sQuery = "INSERT INTO  AsmntYear (SchoolId, AsmntYearCode, AsmntYearDesc, AsmntYearStartDt,AsmntYearEndDt, CurrentInd, CreatedBy, CreateOn) VALUES ('" + sess.SchoolId + "','" + selected + "','" + selected + "','"+startdt+"','"+enddt+"','A','" + sess.LoginId + "',GETDATE())";
//                objData.Execute(sQuery);
//                strQy = "update AsmntYear set CurrentInd = 'D', ModifiedBy = '" + sess.LoginId + "', ModifiedOn = GETDATE() where AsmntYearCode <>'" + selected + "'";
//                objData.Execute(strQy);
//            }
              
//        }

}
