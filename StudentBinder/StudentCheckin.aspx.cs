using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

public partial class StudentBinder_Phase2Css_StudentCheckin : System.Web.UI.Page
{

    clsData objData = null;
    static int StudentId = 0;
    static int ClassId = 0; 
    string strQuery = "";
    clsSession sess = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            imgBDay.ImageUrl = "~/StudentBinder/img/DayB.png";
            ImgBRes.ImageUrl = "~/StudentBinder/img/ResG.png";
            hidSetVal.Value = "0";
            fillStudent("0", false);
        }
        else
        {
            if (hidSearch.Value != "1") bindDetails();
       }
    }
    private void bindDetails()           
    {
        if (txtSearch.Text == "")
        {
            fillStudent(hidSetVal.Value.ToString(), false);
        }
        else
        {
            fillStudent(hidSetVal.Value.ToString(), true);
        }

    }

    protected void grdGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton imgStatus = (ImageButton)e.Row.FindControl("lbl_status");
            HiddenField hidChkStatus = (HiddenField)e.Row.FindControl("hidStatus");
            if (Convert.ToInt32( hidChkStatus.Value)==0)
            {
                imgStatus.ImageUrl = "~/StudentBinder/img/out.png";                
            }
            else
            {
                imgStatus.ImageUrl = "~/StudentBinder/img/in.png";
            }

        }
    }



    protected void grdGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
    }

    protected void grdGroup_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        string isactivestatus = Convert.ToString(e.CommandArgument);
        string[] arg = new string[2];
        arg = isactivestatus.Split(';');

        StudentId = Convert.ToInt32(arg[0]);
        ClassId = Convert.ToInt32(arg[1]);

        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        ImageButton imgStatus = (ImageButton)row.Cells[0].FindControl("lbl_status");
        SaveCheckIn(imgStatus, StudentId, ClassId,sess.SchoolId,sess.LoginId);
    }

    /////-------FUNCTION FOR ENTERING 0 INTO THE BEHAVIOR------
    /////
    //[System.Web.Services.WebMethod]
    //private void SaveBehaviorForStudent(int StudentId) 
    // {
    //     objData = new clsData();
    //     sess = (clsSession)Session["UserSession"];
    //     string InsertBehavior = "SELECT MeasurementId FROM BehaviourDetails WHERE StudentId="+StudentId+" AND ActiveInd='A' AND MeasurementId NOT IN (SELECT DISTINCT MeasurementId FROM Behaviour WHERE StudentId="+StudentId+" AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()))";
    //     DataTable DTBehavior= objData.ReturnDataTable(InsertBehavior, false);
    //     string InsertQuery = "";
    //     int insertresult = 0;
    //     foreach (DataRow Behaviour in DTBehavior.Rows)
    //     {
    //         InsertQuery = "Insert into Behaviour(MeasurementId,StudentId,FrequencyCount,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,ObserverId) Values('" + Behaviour["MeasurementId"] + "','" + StudentId + "','" + 0 + "','A','" + sess.LoginId + "',getdate(),'" + sess.LoginId + "',getdate(),'" + sess.LoginId + "')";
    //         insertresult = objData.Execute(InsertQuery);
    //     }
    // }
    private void SaveCheckIn(ImageButton Img, int StudentId, int ClassId,int Schoolid,int Userid)
    {
        objData = new clsData();
        if (Img.ImageUrl == "~/StudentBinder/img/in.png")
        {
            bool blExist = objData.IFExists("SELECT StudentId FROM StdtSessEvent WHERE StudentId=" + StudentId + " and ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE()) ");
            if (blExist == true)
            {
                objData.Execute("Update [StdtSessEvent] SET CheckStatus='False',CheckoutTime=GETDATE() Where StudentId=" + StudentId + "  And ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
               // SaveBehaviorForStudent(StudentId);
                Img.ImageUrl = "~/StudentBinder/img/out.png";
            }
        }
        else
        {
            bool blExist = objData.IFExists("SELECT StudentId FROM StdtSessEvent WHERE StudentId=" + StudentId + " and ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
            if (blExist == true)
            {
                objData.Execute("Update [StdtSessEvent] SET CheckStatus='True' Where StudentId=" + StudentId + "  And ClassId=" + ClassId + "and EventType='CH' AND SchoolId=" + Schoolid + " AND CONVERT(DATE,CreatedOn)=CONVERT(DATE,GETDATE())");
                Img.ImageUrl = "~/StudentBinder/img/in.png";
            }
            else
            {
                strQuery = "insert into StdtSessEvent (SchoolId,StudentId,EvntTs,CheckStatus,ClassId,CreatedBy,CreatedOn,ModifiedOn,EventType,CheckinTime)values("+Schoolid+"," + StudentId + ",getdate(),'True'," + ClassId + ","+Userid+",getdate(),getdate(),'CH',GETDATE()) ";
                objData.Execute(strQuery);
                Img.ImageUrl = "~/StudentBinder/img/in.png";
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (txtSearch.Text == "")
        {
            fillStudent(hidSetVal.Value.ToString(), false);
        }
        else
        {
            fillStudent(hidSetVal.Value.ToString(), true);
        }

    }


    private void fillStudent(string type, bool search)
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        string DayNRes = (type == "1") ? "OR c.ResidenceInd='0'" : "";
        if (search == true)
        {
            strQuery = "SELECT StudClass.StudentId,StudClass.name,StudClass.ClassId,chkStatus =CASE WHEN (SELECT COUNT(*) FROM StdtSessEvent WHERE "+
                        "StudentId=StudClass.StudentId AND EventType='CH' AND  CONVERT(DATE,CheckinTime)=CONVERT(DATE,GETDATE()) AND CheckStatus='True' and ClassId=StudClass.ClassId AND SchoolId="+sess.SchoolId+")=0 THEN 0 ELSE 1 END " +
                        "FROM (Select s.StudentId,s.StudentLname+'  '+s.StudentFname+'-'+s.StudentNbr+'   '+'('+c.ClassName+')' as name,c.ClassId "+
                        "from Student s Inner Join StdtClass sc on s.StudentId=sc.StdtId Inner Join Class c "+
                        "on c.ClassId=sc.ClassId where s.ActiveInd='A' AND sc.ActiveInd='A' AND  (c.ResidenceInd='" + hidSetVal.Value + "' "+DayNRes+" ) AND s.StudentLname like'%" + txtSearch.Text + "%') AS StudClass ";
            //strQuery = "Select s.StudentId,s.StudentLname+'  '+s.StudentFname+'-'+s.StudentNbr+'   '+'('+c.ClassName+')' as name,c.ClassId,chkStatus = CASE Ss.CheckStatus WHEN 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END   from Student s Inner Join StdtClass sc on s.StudentId=sc.StdtId Inner Join Class c on c.ClassId=sc.ClassId Left Join StdtSessEvent Ss on s.StudentId=Ss.StudentId AND c.ClassId =Ss.ClassId where s.ActiveInd='A' AND sc.ActiveInd='A' AND (EventType='CH' OR c.ResidenceInd='" + hidSetVal.Value + "') And s.StudentLname like'%" + txtSearch.Text + "%'  ";
        }
        else
        {
            strQuery = "SELECT StudClass.StudentId,StudClass.name,StudClass.ClassId,chkStatus =CASE WHEN (SELECT COUNT(*) FROM StdtSessEvent WHERE " +
                        "StudentId=StudClass.StudentId AND EventType='CH' AND  CONVERT(DATE,CheckinTime)=CONVERT(DATE,GETDATE()) AND CheckStatus='True' and ClassId=StudClass.ClassId AND SchoolId="+sess.SchoolId+")=0 THEN 0 ELSE 1 END " +
                        "FROM (Select s.StudentId,s.StudentLname+'  '+s.StudentFname+'-'+s.StudentNbr+'   '+'('+c.ClassName+')' as name,c.ClassId " +
                        "from Student s Inner Join StdtClass sc on s.StudentId=sc.StdtId Inner Join Class c " +
                        "on c.ClassId=sc.ClassId where s.ActiveInd='A' AND sc.ActiveInd='A' AND  (c.ResidenceInd='" + hidSetVal.Value + "' " + DayNRes + " ) ) AS StudClass ";
            //strQuery = "Select s.StudentId,s.StudentLname+'  '+s.StudentFname+'-'+s.StudentNbr+'   '+'('+c.ClassName+')' as name,c.ClassId,chkStatus = CASE Ss.CheckStatus WHEN 1 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END   from Student s Inner Join StdtClass sc on s.StudentId=sc.StdtId Inner Join Class c on c.ClassId=sc.ClassId Left Join StdtSessEvent Ss on s.StudentId=Ss.StudentId AND c.ClassId =Ss.ClassId  where s.ActiveInd='A' AND sc.ActiveInd='A' AND (EventType='CH' OR c.ResidenceInd='" + hidSetVal.Value + "')";
        }


        DataTable Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                grdGroup.DataSource = Dt;
                grdGroup.DataBind();
            }
            else
            {
                grdGroup.DataSource = Dt;
                grdGroup.DataBind();
            }
        }

    }
    private void checkSearch()
    {
        if (txtSearch.Text == "")
        {
            fillStudent(hidSetVal.Value.ToString(), false);
        }
        else
        {
            fillStudent(hidSetVal.Value.ToString(), true);
        }
    }
    protected void imgBDay_Click(object sender, ImageClickEventArgs e)
    {
        //day.Style["background-color"] = "#E3EAEB";
        imgBDay.ImageUrl = "~/StudentBinder/img/DayB.png";
        ImgBRes.ImageUrl = "~/StudentBinder/img/ResG.png";
        //res.Style["background-color"] = "#ddd";
        hidSetVal.Value = "0";
        txtSearch.Text = "";
        checkSearch();
    }
    protected void ImgBRes_Click(object sender, ImageClickEventArgs e)
    {
        imgBDay.ImageUrl = "~/StudentBinder/img/DayG.png";
        ImgBRes.ImageUrl = "~/StudentBinder/img/ResB.png";
        //res.Style["background-color"] = "#E3EAEB";
        //day.Style["background-color"] = "#ddd";
        hidSetVal.Value = "1";
        txtSearch.Text = "";
        checkSearch();
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (txtSearch.Text == "")
        {
            fillStudent(hidSetVal.Value.ToString(), false);
        }
        else
        {
            fillStudent(hidSetVal.Value.ToString(), true);
        }
    }




   
}