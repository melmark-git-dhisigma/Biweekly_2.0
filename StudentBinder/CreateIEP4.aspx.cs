using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


public partial class StudentBinder_CreateIEP4 : System.Web.UI.Page
{
    clsData objData = null;
    clsSession sess = null;
    DataClass oData = null;
    string strQuery = "";
    DataTable Dt = null;
    string strQuery2 = "";
    DataTable Dt2 = null;
    static int intStdtGoalId = 0;
    int retVal = 0;




    protected void Page_Load(object sender, EventArgs e)
    {

        sess = (clsSession)Session["UserSession"];

        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }

        if (!IsPostBack)
        {
            bool Disable = false;
            clsIEP IEPObj = new clsIEP();
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btn_Update.Visible = false;
                //btnAddGoal1.Visible = false;
            }
            else
            {
                btn_Update.Visible = true;
                //btnAddGoal1.Visible = true;
            }
            if (sess != null)
            {
                //sess.IEPId = getIepIdFromStudentId();

                fillGoalChecked();
                getGoalDetails();
                //FillData();

                //fillGoal();
                fillCPLevel();

                // ViewAccReject();
                //if (ddlStudent.SelectedIndex == 0)
                //{
                //    chkGoal.Enabled = false;
                //}
                //else
                //{
                //    chkGoal.Enabled = true;
                //}

                string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
                if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
                {
                    btn_Update.Visible = false;
                    //btnAddGoal1.Visible = false;
                }
                else
                {
                    btn_Update.Visible = true;
                    //btnAddGoal1.Visible = true;
                }

            }
            ViewAccReject();
        }
    }

    public int getIepIdFromStudentId()
    {

        object pendstatus = null;
        object IepStatus = null;
        object IepId = null;
        int IEP_id = 0;
        clsData oData = new clsData();
        sess = (clsSession)Session["UserSession"];
        sess.IEPId = 0;
        if (oData.IFExists("Select StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A') ") == true)
        {
            pendstatus = oData.FetchValue("Select TOP 1 StatusId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ").ToString();

            if (int.Parse(pendstatus.ToString()) > 0)
            {
                IepStatus = oData.FetchValue("select lookupname from LookUp where LookupId=" + int.Parse(pendstatus.ToString()));
            }
            if ((IepStatus.ToString() == "Approved") || (IepStatus.ToString() == "Expired"))
            {
                IEP_id = 0;
            }

            else
            {
                IepId = oData.FetchValue("Select TOP 1 StdtIEPId from StdtIEP where StudentId=" + sess.StudentId + " and SchoolId=" + sess.SchoolId + " and AsmntYearId=(select AsmntYearId from AsmntYear where CurrentInd='A')  ORDER BY StdtIEPId DESC ");
                IEP_id = int.Parse(IepId.ToString());
            }


        }
        return IEP_id;
    }
    private void ViewAccReject()
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        int reject = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Rejected'");
        int approve = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        if (sess.IEPStatus == approve || sess.IEPStatus == reject)
        {
            btn_Update.Visible = false;
        }

    }
    private int getInProgressId()
    {
        objData = new clsData();
        strQuery = "select LookupId from LookUp where LookupName='In Progress' and LookupType='IEP Status'";
        objData.Execute(strQuery);
        Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt.Rows.Count > 0)
        {
            return Convert.ToInt32(Dt.Rows[0]["LookupId"].ToString());
        }
        else
        {
            return 0;
        }


    }




    private void getDetails()
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.StudentId == 0)
        {
            chkGoal.Enabled = false;
        }
        else
        {
            chkGoal.Enabled = true;
        }
    }
    protected void btn_Update_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.StudentId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
            return;
        }
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        objData = new clsData();
        DataClass oData = new DataClass();
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            //if (validateEditor() == true)
            //{
            Update();
            //}
        }
        //if (btn_Update.Text == "Save")
        //{
        //    Save();
        //}
        //else
        //{

        //}
    }
    protected void btn_Update_hdn_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.StudentId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
            return;
        }
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        objData = new clsData();
        DataClass oData = new DataClass();
        string pendstatus = "";
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            //if (validateEditor() == true)
            //{
            Update();
            //}
        }
        //if (btn_Update.Text == "Save")
        //{
        //    Save();
        //}
        //else
        //{

        //}
    }
    public bool validateEditor()
    {
        int testFlg = 0;
        foreach (DataListItem item in dlGoals.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl txt1 = (System.Web.UI.HtmlControls.HtmlGenericControl)item.FindControl("txtObjectiveA");
                TextBox txt2 = (TextBox)item.FindControl("txtObjectiveB");
                TextBox txt3 = (TextBox)item.FindControl("txtObjectiveC");
                if ((txt1.InnerHtml.Length < 200) && (txt2.Text.Length < 200) && (txt3.Text.Length < 200))
                {

                }
                else
                {
                    testFlg++;
                }
            }
        }
        if (testFlg == 0)
        {
            return true;
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Editor text should be less than 200 characters");
            return false;
        }

    }
    protected void Save()
    {
        sess = (clsSession)Session["UserSession"];

        if (sess != null)
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            int intIEPGoalNo = 1;
            foreach (DataListItem diGoal in dlGoals.Items)
            {

                HiddenField hf_Id = (HiddenField)diGoal.FindControl("hfLessonPlanId");

                TextBox Objective1 = (TextBox)diGoal.FindControl("txtObjectiveA");
                TextBox Objective2 = (TextBox)diGoal.FindControl("txtObjectiveB");
                TextBox Objective3 = (TextBox)diGoal.FindControl("txtObjectiveC");

                string[] args = new string[40];
                if (Objective1.Text.Trim() != "" || Objective2.Text.Trim() != "" || Objective3.Text.Trim() != "")
                {
                    strQuery = "UPDATE StdtLessonPlan SET Objective1='" + clsGeneral.convertQuotes(Objective1.Text.Trim()) + "', Objective2= '" + clsGeneral.convertQuotes(Objective2.Text.Trim()) + "', Objective3= '" + clsGeneral.convertQuotes(Objective3.Text.Trim()) + "', " +
                             "StatusId = '" + sess.IEPStatus + "',IEPGoalNo= " + intIEPGoalNo + ", ModifiedBy = '" + sess.LoginId + "', ModifiedOn= GETDATE() WHERE StdtLessonPlanId=" + hf_Id + " ";



                    retVal = objData.Execute(strQuery);

                    if (intStdtGoalId > 0)
                        intIEPGoalNo++;
                }
                else
                {
                    tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter any Objective");
                    Objective1.Focus();
                }

            }

        }
        if (retVal > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Sucessfully");

        }
    }

    protected void Update()
    {
        //int i = 0;
        //int rowNo = 0;
        //objData = new clsData();
        //DataClass oData = new DataClass();
        //rowNo = oData.ExecuteScalar("Select count(IEPGoalNo) from StdtGoal");

        //sess = (clsSession)Session["UserSession"];

        //string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

        //if (StatusName == "Approved" || StatusName == "Rejected")
        //{
        //    tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
        //    return;
        //}
        //else
        //{

        //    foreach (DataListItem diGoal in dlGoals.Items)
        //    {

        //        HiddenField hf_Id = (HiddenField)diGoal.FindControl("hfGoal");
        //        HiddenField hfIEPNoId = (HiddenField)diGoal.FindControl("hfIEPNo");
        //        TextBox Objective1 = (TextBox)diGoal.FindControl("txtObjectiveA");
        //        TextBox Objective2 = (TextBox)diGoal.FindControl("txtObjectiveB");
        //        TextBox Objective3 = (TextBox)diGoal.FindControl("txtObjectiveC");

        //        string[] args = new string[40];

        //        if (Objective1.Text.Trim() != "" || Objective2.Text.Trim() != "" || Objective3.Text.Trim() != "")
        //        {

        //            sess = (clsSession)Session["UserSession"];
        //            objData = new clsData();

        //            int studentId = sess.StudentId;
        //            int schoolId = sess.SchoolId;
        //            int assmntYearId = sess.YearId;
        //            int studentIEPId = sess.IEPId;

        //            //string stringRowNo = objData.FetchValue("Select max(IEPGoalNo) from StdtGoal").ToString();
        //            //rowNo = Convert.ToInt32(stringRowNo);


        //            rowNo++;

        //            i++;

        //            strQuery = "UPDATE StdtGoal SET IEPGoalNo= " + i + ", AsmntYearId= '" + sess.YearId + "', " +
        //                              "Objective1='" + Objective1.Text.Trim() + "', Objective2= '" + Objective2.Text.Trim() + "', Objective3= '" + Objective3.Text.Trim() + "', " +
        //                              "StatusId = " + sess.IEPStatus + ", ModifiedBy = '" + sess.LoginId + "', ModifiedOn= GETDATE() WHERE StudentId= " + sess.StudentId + " and SchoolId=" + schoolId + " and GoalId=" + hf_Id.Value + "  ";

        //            retVal = objData.Execute(strQuery);

        //        }
        //        else
        //        {
        //            tdMsg.InnerHtml = clsGeneral.warningMsg("Please Enter any Objective");
        //            Objective1.Focus();
        //        }

        //    }
        //    if (retVal > 0)
        //    {
        //        tdMsg.InnerHtml = clsGeneral.sucessMsg("Data IEP Page 4 Updated for " + sess.StudentName);
        //    }
        //}

        sess = (clsSession)Session["UserSession"];


        if (sess != null)
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            int intIEPGoalNo = 1;
            string GoalIds = "";
            string[] arSortNumber = new string[dlGoals.Items.Count];
            string strSort = "";
            int i = 0;
            //foreach (DataListItem diGoal in dlGoals2.Items)
            //{
            //    TextBox drpSortId = (TextBox)diGoal.FindControl("drpSortOrder_txt");
            //    strSort = drpSortId.Text;
            //    arSortNumber[i] = strSort.ToString();
            //    i++;

            //}
            //if (HasDuplicates(arSortNumber) == false)
            //{
                foreach (DataListItem diGoal in dlGoals.Items)
                {


                    Panel pnl = (Panel)diGoal.FindControl("pnlGoalLp");

                    TextBox Objective2 = (TextBox)diGoal.FindControl("txtObjectiveB1");
                    TextBox Objective3 = (TextBox)diGoal.FindControl("txtObjectiveC1");
                    TextBox txtGoalNote = (TextBox)diGoal.FindControl("txtIEPGoalNote");

                    //if (Objective1.Text.Trim() != "" && Objective2.Text.Trim() != "" && Objective3.Text.Trim() != "")
                    //{
                    string Object2 = clsGeneral.HtmlToString(Objective2.Text.Trim());
                    string Object3 = clsGeneral.HtmlToString(Objective3.Text.Trim());
                    string goalNote = clsGeneral.convertQuotes(txtGoalNote.Text.Trim());

                    HiddenField hf_GoalId = (HiddenField)diGoal.FindControl("hfGoalId");
                    strQuery = "Update StdtGoal Set GoalIEPNote='" + goalNote + "' Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + hf_GoalId.Value + "  And StdtIEPId=" + sess.IEPId + "  ";
                    retVal = objData.Execute(strQuery);
                    //Object2 = System.Uri.UnescapeDataString(Object2);
                    //Object3 = System.Uri.UnescapeDataString(Object3);

                    DataList dl1 = (DataList)diGoal.FindControl("dlCPLevel");

                    //DropDownList drpSortId = (DropDownList)diGoal.FindControl("drpSortOrder");
                    //strSort = drpSortId.SelectedItem.Text;
                    //arSortNumber[i] = strSort.ToString();
                    //i++;
                    //if (HasDuplicates(arSortNumber) != false)
                    //{
                    //    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Sucessfully. ");
                    //    return;
                    //}
                    foreach (DataListItem diCPLevel in dl1.Items)
                    {
                        HiddenField hf_Id = (HiddenField)diCPLevel.FindControl("hfStdtLessonPlanId");
                        TextBox Objective1 = (TextBox)diCPLevel.FindControl("txtObjectiveA1");
                        string Object1 = clsGeneral.HtmlToString(Objective1.Text.Trim());

                        //Object1 = System.Uri.UnescapeDataString(Object1);

                        strQuery = "UPDATE StdtLessonPlan SET Objective1='" + clsGeneral.convertQuotes(Object1) + "', Objective2= '" + clsGeneral.convertQuotes(Object2) + "', Objective3= '" + clsGeneral.convertQuotes(Object3) + "', " +
                                 "ModifiedBy = '" + sess.LoginId + "', ModifiedOn= GETDATE() WHERE StdtLessonPlanId=" + hf_Id.Value + " ";



                        //strQuery = "UPDATE StdtLessonPlan SET Objective1='" + clsGeneral.convertQuotes(Object1) + "', Objective2='" + clsGeneral.convertQuotes(txtObjectiveB1.Text) + "', Objective3='" + clsGeneral.convertQuotes(txtObjectiveC1.Text) + "', ModifiedBy = '" + sess.LoginId + "', ModifiedOn= GETDATE() WHERE StdtLessonPlanId=" + hf_Id.Value + " ";

                        retVal = objData.Execute(strQuery);

                        //if (retVal > 0)
                        //{
                        //    strQuery = "Select GoalId from StdtLessonPlan WHERE StdtLessonPlanId=" + hf_Id.Value + " ";
                        //    object obj = objData.FetchValue(strQuery);
                        //    int GoalId = 0;
                        //    if (obj != null)
                        //    {
                        //        GoalId = Convert.ToInt16(obj);
                        //    }


                        //    if (!GoalIds.Contains(GoalId.ToString() + ","))
                        //    {
                        //        strQuery = "Update StdtGoal Set IEPGoalNo=" + Convert.ToInt32(drpSortId.SelectedItem.Text) + " Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + GoalId + "  And StdtIEPId=" + sess.IEPId + "  ";
                        //        retVal = objData.Execute(strQuery);
                        //        GoalIds += GoalId.ToString() + ",";
                        //        intIEPGoalNo++;
                        //    }
                        //}
                    }
                }
                //foreach (DataListItem diGoal in dlGoals2.Items)
                //{
                //    HiddenField hf_GoalId = (HiddenField)diGoal.FindControl("hfSortGoalId");
                //    TextBox drpSortId = (TextBox)diGoal.FindControl("drpSortOrder_txt");
                //    strQuery = "Update StdtGoal Set IEPGoalNo=" + Convert.ToInt32(drpSortId.Text) + " Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + hf_GoalId.Value + "  And StdtIEPId=" + sess.IEPId + "  ";
                //    retVal = objData.Execute(strQuery);
                //}
            //}

            //else
            //{
            //    tdMsg.InnerHtml = clsGeneral.failedMsg("Duplicate Sort Order");
            //    return;
            //}
        }

        if (retVal > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Sucessfully. ");
            if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
            {
                objData.Execute("update stdtIEPUpdateStatus set Page4='true' where stdtIEPId=" + sess.IEPId);
            }
            else
            {
                objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page4) values(" + sess.IEPId + ",'true')");
            }
            ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(5);", true);
        }

        if (retVal == 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Sucessfully. ");
            if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
            {
                objData.Execute("update stdtIEPUpdateStatus set Page4='true' where stdtIEPId=" + sess.IEPId);
            }
            else
            {
                objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page4) values(" + sess.IEPId + ",'true')");
            }
            //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP5();", true);
            ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(5);", true);
        }

    }
    private bool HasDuplicates(string[] arrayList)
    {
        List<string> vals = new List<string>();
        bool returnValue = false;
        foreach (string s in arrayList)
        {
            if (vals.Contains(s))
            {
                returnValue = true;
                break;
            }
            vals.Add(s);
        }


        return returnValue;
    }
    protected void Update1()
    {

        sess = (clsSession)Session["UserSession"];


        if (sess != null)
        {
            objData = new clsData();
            sess = (clsSession)Session["UserSession"];
            int intIEPGoalNo = 1;
            string GoalIds = "";
            foreach (DataListItem diGoal in dlGoals.Items)
            {

                HiddenField hf_Id = (HiddenField)diGoal.FindControl("hfStdtLessonPlanId");
                Panel pnl = (Panel)diGoal.FindControl("pnlGoalLp");


                TextBox Objective1 = (TextBox)diGoal.FindControl("txtObjectiveA1");
                TextBox Objective2 = (TextBox)diGoal.FindControl("txtObjectiveB1");
                TextBox Objective3 = (TextBox)diGoal.FindControl("txtObjectiveC1");
                TextBox txtGoalNote = (TextBox)diGoal.FindControl("txtIEPGoalNote");
                //if (Objective1.Text.Trim() != "" && Objective2.Text.Trim() != "" && Objective3.Text.Trim() != "")
                //{
                HiddenField hf_GoalId = (HiddenField)diGoal.FindControl("hfGoalId");
                strQuery = "Update StdtGoal Set GoalIEPNote=" + txtGoalNote.Text.Trim() + " Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + hf_GoalId.Value + "  And StdtIEPId=" + sess.IEPId + "  ";
                retVal = objData.Execute(strQuery);

                string Object1 = clsGeneral.HtmlToString(Objective1.Text.Trim());
                string Object2 = clsGeneral.HtmlToString(Objective2.Text.Trim());
                string Object3 = clsGeneral.HtmlToString(Objective3.Text.Trim());
                strQuery = "UPDATE StdtLessonPlan SET Objective1='" + clsGeneral.convertQuotes(Object1) + "', Objective2= '" + clsGeneral.convertQuotes(Object2) + "', Objective3= '" + clsGeneral.convertQuotes(Object3) + "', " +
                         "ModifiedBy = '" + sess.LoginId + "', ModifiedOn= GETDATE() WHERE StdtLessonPlanId=" + hf_Id.Value + " ";


                retVal = objData.Execute(strQuery);

                if (retVal > 0)
                {
                    strQuery = "Select GoalId from StdtLessonPlan WHERE StdtLessonPlanId=" + hf_Id.Value + " ";
                    object obj = objData.FetchValue(strQuery);
                    int GoalId = 0;
                    if (obj != null)
                    {
                        GoalId = Convert.ToInt16(obj);
                    }


                    if (!GoalIds.Contains(GoalId.ToString() + ","))
                    {
                        strQuery = "Update StdtGoal Set IEPGoalNo=" + intIEPGoalNo + " Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + GoalId + "  And StdtIEPId=" + sess.IEPId + "  ";
                        retVal = objData.Execute(strQuery);
                        GoalIds += GoalId.ToString() + ",";
                        intIEPGoalNo++;
                    }


                }









            }

        }
        if (retVal > 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Sucessfully. ");
            if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
            {
                objData.Execute("update stdtIEPUpdateStatus set Page4='true' where stdtIEPId=" + sess.IEPId);
            }
            else
            {
                objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page4) values(" + sess.IEPId + ",'true')");
            }
            // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP5();", true);
        }

        if (retVal == 0)
        {
            tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Sucessfully. ");
            if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
            {
                objData.Execute("update stdtIEPUpdateStatus set Page4='true' where stdtIEPId=" + sess.IEPId);
            }
            else
            {
                objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page4) values(" + sess.IEPId + ",'true')");
            }
            // ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP5();", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }
    protected void FillData()
    {

        objData = new clsData();
        sess = (clsSession)Session["UserSession"];

        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }

        foreach (DataListItem diGoal in dlGoals.Items)
        {

            HiddenField hf_Id = (HiddenField)diGoal.FindControl("hfGoal");
            HiddenField hfIEPNoId = (HiddenField)diGoal.FindControl("hfIEPNo");
            TextBox Objective1 = (TextBox)diGoal.FindControl("txtObjectiveA");
            TextBox Objective2 = (TextBox)diGoal.FindControl("txtObjectiveB");
            //TextBox Objective3 = (TextBox)diGoal.FindControl("txtObjectiveC");

            string[] args = new string[40];
            strQuery = "SELECT StdtGoalId,GoalId,AsmntYearId,IEPGoalNo,Objective1, Objective2, Objective3 FROM StdtGoal WHERE StdtIEPId= '" + sess.IEPId + "' AND GoalId='" + Convert.ToInt32(hf_Id.Value) + "'  order by IEPGoalNo  asc ";
            Dt = objData.ReturnDataTable(strQuery, false);
            if (Dt.Rows.Count > 0)
            {
                hfIEPNoId.Value = Dt.Rows[0]["IEPGoalNo"].ToString().Trim();
                Objective1.Text = Dt.Rows[0]["Objective1"].ToString().Trim();
                Objective2.Text = Dt.Rows[0]["Objective2"].ToString().Trim();
                //Objective3.Text = Dt.Rows[0]["Objective3"].ToString().Trim();

                //ddl_Year.SelectedIndex = Convert.ToInt32(Dt.Rows[0]["AsmntYearId"].ToString().Trim());
                btn_Update.Text = "Update";
            }
            else
            {

                Objective1.Text = "";
                Objective2.Text = "";
                //Objective3.Text = "";

            }
        }



    }



    protected void fillGoal()
    {
        objData = new clsData();
        string goalId = chkGoal.SelectedValue;
        string items = "0,";
        int iepId = Convert.ToInt32(ViewState["IEPId"]);

        foreach (ListItem li in chkGoal.Items)
        {
            items += li.Value + ",";
        }

        if (items != "")  //GoalCode is used to display the Edited goals
        {
            items = items.Substring(0, items.Length - 1);
            //strQuery = "SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalName, dbo.StdtLessonPlan.GoalId, dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3 FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId in (" + items + ") ORDER BY dbo.StdtLessonPlan.GoalId";// and StdtLessonPlan.StdtIEPId=" + Convert.ToInt16(sess.IEPId);

            //strQuery = "SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalName, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber," +
            //                " dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, " +
            //                " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3  " +
            //                        " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
            //                             "dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId in (" + items + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'  AND  dbo.StdtLessonPlan.ActiveInd = 'A'   ORDER BY " +
            //                                            "dbo.StdtLessonPlan.GoalId ";

            strQuery = "SELECT CASE A.Row WHEN 1 THEN '<div class=goalTitle>Goal No : '+CONVERT(varchar(10),A.GoalNumber)+' - '+A.GoalCode+'</div>' ELSE A.LessonPlanName END AS Title,* FROM(" +
                "SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber," +
                "ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row," +
                            " dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, " +
                            " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3  " +
                                    " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
                                         "dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId in (" + items + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'  AND  dbo.StdtLessonPlan.ActiveInd = 'A')A ORDER BY A.GoalId";

            Dt = objData.ReturnDataTable(strQuery, false);


            //foreach (DataRow row in Dt.Rows)
            //{
            //}

            string strQuery3 = "SELECT * FROM StdtLessonPlan INNER JOIN Goal ON StdtLessonPlan.GoalId = Goal.GoalId INNER JOIN LessonPlan ON StdtLessonPlan.LessonPlanId = LessonPlan.LessonPlanId WHERE StdtLessonPlan.GoalId in (" + items + ") AND StdtLessonPlan.StdtIEPId = '" + sess.IEPId + "' AND StdtLessonPlan.ActiveInd = 'A' GROUP BY GoalId";

            DataTable Dt3 = objData.ReturnDataTable(strQuery3, false);


            DataView view = new DataView(Dt);
            DataTable distinctValues = view.ToTable(true, "GoalCode");




            dlGoals.DataSource = Dt;
            dlGoals.DataBind();
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Goal");
            return;
        }
    }

    protected void fillCPLevel()
    {
        bind();
        #region commented to revert the changes for Goal duplication(The change affected the movement of goals)
        //ViewState["IEPGoalNo"] = dlGoals.Items.Count;
        //foreach (DataListItem diGoal in dlGoals2.Items)
        //{
        //    HiddenField hf_GoalId = (HiddenField)diGoal.FindControl("hfSortGoalId");
        //    //TextBox drpSortId = (TextBox)diGoal.FindControl("drpSortOrder_txt");
        //    int drpSortId = Convert.ToInt32(ViewState["IEPGoalNo"].ToString());
        //    //strQuery = "Update StdtGoal Set IEPGoalNo=" + Convert.ToInt32(drpSortId.Text) + " Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + hf_GoalId.Value + "  And StdtIEPId=" + sess.IEPId + "  ";
        //    strQuery = "Update StdtGoal Set IEPGoalNo=" + Convert.ToInt32(drpSortId) + " Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + hf_GoalId.Value + "  And StdtIEPId=" + sess.IEPId + "  ";
        //    retVal = objData.Execute(strQuery);
        //}
        //bind();
        #endregion
    }
    protected void fillGoalChecked()
    {

        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        oData = new DataClass();
        //int selIEPID = oData.ExecuteScalar("SELECT StdtIEPid from StdtIEP where StudentId=" + sess.StudentId);
        int selIEPID = sess.IEPId;
        ViewState["IEPId"] = Convert.ToInt32(selIEPID);

        //strQuery = "select  Goal.GoalName as Name, Goal.GoalId as Id from StdtGoal inner join Goal on StdtGoal.GoalId = Goal.GoalId where StdtGoal.StdtIEPId='" + Convert.ToInt32(sess.IEPId) + "' and StdtGoal.IncludeIEP=1 Order by StdtGoal.IEPGoalNo";
        // strQuery = "select distinct Goal.GoalName as Name, Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId='" + selIEPID + "' and StdtLessonPlan.IncludeIEP=1";
        strQuery = "select distinct Goal.GoalName as Name, Goal.GoalId as Id from StdtLessonPlan inner join Goal on StdtLessonPlan.GoalId = Goal.GoalId where StdtLessonPlan.StdtIEPId='" + selIEPID + "' and StdtLessonPlan.IncludeIEP=1";
        objData.ReturnDataTableForRadioList(strQuery, chkGoal);

        string ids = "0,";
        foreach (ListItem item in chkGoal.Items)
        {
            ids += item.Value + ",";
        }

        ids = ids.Substring(0, ids.Length - 1);
        fillNotIncGoalCheck(ids);

    }

    protected void fillNotIncGoalCheck(string ids)
    {
        int inProgressId = getInprogressId();

        objData = new clsData();
        // strQuery = "SELECT GoalId as Id,GoalName as Name FROM Goal WHERE GoalLevelId=1";
        strQuery = "select GoalId as Id, GoalName as Name from Goal where GoalId not in (" + ids + ") and GoalLevelId=1";
        //objData.ReturnCheckBoxList(strQuery, chkGoal);
    }

    private int getInprogressId()
    {
        objData = new clsData();
        strQuery = "select LookupId as Id from Lookup where LookupName='In Progress' and LookupType='Goal Status'";
        Dt = objData.ReturnDataTable(strQuery, false);
        if (Dt.Rows.Count > 0)
        {
            return Convert.ToInt32(Dt.Rows[0]["Id"].ToString());
        }
        else
        {
            return 0;
        }
    }
    protected void dlGoalsAdded_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        sess = (clsSession)Session["UserSession"];
        int count = 0;
        foreach (ListItem item in chkGoal.Items)
        {

            if (item.Selected)
            {
                count++;
                string query = "DELETE FROM StdtGoal WHERE StudentId= '" + sess.StudentId + "' AND GoalId= " + item.Value + " AND StdtIEPId='" + sess.IEPId + "'";
                int i = objData.Execute(query);
            }

            tdMsg.InnerHtml = clsGeneral.warningMsg(count.ToString() + " Items Were Deleted.");

        }

        fillGoalChecked();

        getGoalDetails();
    }

    private void getGoalDetails()
    {
        sess = (clsSession)Session["UserSession"];
        if (sess.StudentId == 0)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Student");
            return;
        }
        //fillGoal();
        // FillData();
    }


    private int getAssYearId()
    {
        objData = new clsData();
        string query = "select AsmntYearId from AsmntYear where CurrentInd='A'";
        Dt = objData.ReturnDataTable(query, false);
        if (Dt.Rows.Count > 0)
        {
            return Convert.ToInt32(Dt.Rows[0]["AsmntYearId"].ToString());
        }
        else
        {
            return 0;
        }
    }


    protected void chkGoal_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGoal();
    }

    protected void dlGoals_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        objData = new clsData();
        HiddenField hf_Id = (HiddenField)e.Item.FindControl("hfStdtLessonPlanId");
        HtmlGenericControl Objective2 = (HtmlGenericControl)e.Item.FindControl("txtObjectiveB");
        HtmlGenericControl Objective3 = (HtmlGenericControl)e.Item.FindControl("txtObjectiveC");

        TextBox txtIEPGoalNote = (TextBox)e.Item.FindControl("txtIEPGoalNote");
        TextBox Object2 = (TextBox)e.Item.FindControl("txtObjectiveB1");
        TextBox Object3 = (TextBox)e.Item.FindControl("txtObjectiveC1");
        //HiddenField hf_SortOrder = (HiddenField)e.Item.FindControl("hfIEPGoalNo");
        //DropDownList drpSortNumber = (DropDownList)e.Item.FindControl("drpSortOrder");
        //drpSortNumber.SelectedValue = hf_SortOrder.Value;
       

        string StrQuery = "SELECT Objective2,Objective3 FROM StdtLessonPlan WHERE StdtLessonPlanId=" + hf_Id.Value + "";
        DataTable DT = objData.ReturnDataTable(StrQuery, false);
        if (DT != null)
        {
            Objective2.InnerHtml = Convert.ToString(DT.Rows[0]["Objective2"]);
            Objective3.InnerHtml = Convert.ToString(DT.Rows[0]["Objective3"]);

            Object2.Text = clsGeneral.StringToHtml(Convert.ToString(Convert.ToString(DT.Rows[0]["Objective2"])));
            Object3.Text = clsGeneral.StringToHtml(Convert.ToString(Convert.ToString(DT.Rows[0]["Objective3"])));

            

            //Object2.Text = System.Uri.EscapeDataString(Object2.Text);
            //Object3.Text = System.Uri.EscapeDataString(Object3.Text);


        }
        HiddenField hf_GoalId = (HiddenField)e.Item.FindControl("hfGoalId");
        /// LP name based LP status 
        /// 
   //     string StrQuery2 = "SELECT  CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row," +
   //                        "A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1,A.Objective2,A.Objective3,LessonOrder FROM(SELECT dbo.LessonPlan.LessonPlanName,dbo.Goal.GoalCode, " +
   //                        "dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber,ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row," +
   //                        " dbo.StdtLessonPlan.LessonPlanId,dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2," +
   //                        " dbo.StdtLessonPlan.Objective3,(SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE StdtLessonplanId= dbo.StdtLessonPlan.StdtLessonPlanId ORDER BY DSTempHdrId DESC) DSTemplateName,(SELECT TOP 1" +
   //                        "LessonOrder FROM DSTempHdr WHERE LessonPlanId = dbo.StdtLessonPlan.LessonPlanId AND DSTempHdr.StudentId = '" + sess.StudentId + "' ORDER BY DSTempHdrId DESC)LessonOrder  FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON " +
   //"dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId = '" + hf_GoalId.Value + "' AND dbo.StdtLessonPlan.StdtIEPId =    '" + sess.IEPId + "'  AND dbo.StdtLessonPlan.ActiveInd = 'A' AND dbo.StdtLessonPlan.IncludeIEP=1)A ORDER BY A.GoalId, LessonOrder";

        ///select LP name based on the status of IEP
        ///
        int statId = Convert.ToInt32(objData.FetchValue("SELECT StatusId FROM StdtIEP WHERE StdtIEPId= (SELECT StdtIEPId FROM StdtLessonPlan WHERE StdtLessonPlanId=" + hf_Id.Value + ") AND StudentId= " + sess.StudentId));
        int statInProg= Convert.ToInt32(objData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress'"));
        int statPending= Convert.ToInt32(objData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'"));       
        string StrQuery2 = "";
        if ((statId == statInProg) || (statId == statPending))
        {
            string strReplace = "StatusId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + " AND LessonPlanId=dbo.StdtLessonPlan.LessonPlanId AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' ";
            string strReplace2 = "DSTemplateName FROM DSTempHdr WHERE StudentId=" + sess.StudentId + "  AND LessonPlanId=dbo.StdtLessonPlan.LessonPlanId AND StatusId = (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus'";

            StrQuery2 = "SELECT  CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row," +
                           "A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1,A.Objective2,A.Objective3,LessonOrder FROM(SELECT dbo.LessonPlan.LessonPlanName, " +
                           "dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber, " +
                           "ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row, dbo.StdtLessonPlan.LessonPlanId,dbo.StdtLessonPlan.StdtIEPId, " +
                           "dbo.StdtLessonPlan.StdtLessonPlanId,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, " +
                           "(SELECT CASE WHEN (SELECT " + strReplace + " AND LookupName='Approved')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Approved')) " +
                           "ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='Pending Approval')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Pending Approval')) " +
                           "ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='In Progress')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='In Progress')) " +
                           "ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='Maintenance')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Maintenance')) " +
                           //"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Expired')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Expired') ORDER BY DSTempHdrId DESC) " +
                           //"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Inactive')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Inactive') ORDER BY DSTempHdrId DESC) END END "+
                            "END END END END) DSTemplateName, " +
                           "(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE LessonPlanId = dbo.StdtLessonPlan.LessonPlanId AND DSTempHdr.StudentId = '" + sess.StudentId + "' " +
                           "ORDER BY DSTempHdrId DESC)LessonOrder  FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId " +
                           "INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId = '" + hf_GoalId.Value + "' " +
                           "AND dbo.StdtLessonPlan.StdtIEPId =    '" + sess.IEPId + "'  AND dbo.StdtLessonPlan.ActiveInd = 'A' AND dbo.StdtLessonPlan.IncludeIEP=1 AND (select COUNT(StatusId) " + "FROM DSTempHdr WHERE LessonPlanId = dbo.StdtLessonPlan.lessonplanid AND StudentId=" + sess.StudentId + " AND StatusId IN " +
                           "(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName IN ('In Progress', 'Pending Approval', 'Approved', 'Maintenance')))>0 )A " +
                           "ORDER BY A.GoalId, LessonOrder";
        }
        else 
        {
            StrQuery2 = "SELECT  CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row, "+
	            "A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1,A.Objective2,A.Objective3,LessonOrder FROM(SELECT dbo.LessonPlan.LessonPlanName, "+
	            "dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber, "+
	            "ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row, dbo.StdtLessonPlan.LessonPlanId,dbo.StdtLessonPlan.StdtIEPId, "+
	            "dbo.StdtLessonPlan.StdtLessonPlanId,  dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, (dbo.DSTempHdr.DSTemplateName) DSTemplateName, "+
                "(SELECT TOP 1 LessonOrder FROM DSTempHdr WHERE LessonPlanId = dbo.StdtLessonPlan.LessonPlanId AND DSTempHdr.StudentId = '" + sess.StudentId + "' " +
	            "ORDER BY DSTempHdrId DESC)LessonOrder  FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId "+
	            "INNER JOIN  dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId INNER JOIN dbo.DSTempHdr on dbo.DSTempHdr.DSTempHdrId=dbo.StdtLessonPlan.DSTempHdrId "+
                "where StdtLessonPlan.GoalId = '" + hf_GoalId.Value + "' AND dbo.StdtLessonPlan.StdtIEPId = '" + sess.IEPId + "' AND dbo.StdtLessonPlan.IncludeIEP=1)A " +
	            "ORDER BY A.GoalId, LessonOrder";
        }

        ///end
        ///
        DataTable DT2 = objData.ReturnDataTable(StrQuery2, false);

        DataList dl1 = (DataList)e.Item.FindControl("dlCPLevel");
        dl1.DataSource = DT2;
        dl1.DataBind();
    }

    protected void dlGoals2_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HiddenField hf_SortOrder = (HiddenField)e.Item.FindControl("hfIEPGoalNo");
        DropDownList drpSortNumber = (DropDownList)e.Item.FindControl("drpSortOrder");
        if(drpSortNumber != null)
        drpSortNumber.SelectedValue = hf_SortOrder.Value;

    }

    protected void dlCPLevel_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        objData = new clsData();
        HiddenField hf_Id = (HiddenField)e.Item.FindControl("hfStdtLessonPlanId");
        HtmlGenericControl Objective1 = (HtmlGenericControl)e.Item.FindControl("txtObjectiveA");

        TextBox Object1 = (TextBox)e.Item.FindControl("txtObjectiveA1");

        string StrQuery = "SELECT Objective1 FROM StdtLessonPlan WHERE StdtLessonPlanId=" + hf_Id.Value + "";
        DataTable DT = objData.ReturnDataTable(StrQuery, false);
        if (DT != null)
        {
            Objective1.InnerHtml = Convert.ToString(DT.Rows[0]["Objective1"]);

            Object1.Text = clsGeneral.StringToHtml(Convert.ToString(Convert.ToString(DT.Rows[0]["Objective1"])));

            //Object1.Text = System.Uri.EscapeDataString(Object1.Text);
        }
    }

    protected void bind()
    {
        objData = new clsData();
        string goalId = chkGoal.SelectedValue;
        string items = "0,";
        int iepId = Convert.ToInt32(ViewState["IEPId"]);
        foreach (ListItem li in chkGoal.Items)
        {
            items += li.Value + ",";
        }
        if (items != "")
        {
            items = items.Substring(0, items.Length - 1);
            /// LP name base on LP status
            /// 
            //strQuery2 = "SELECT DISTINCT CASE A.Row WHEN 1 THEN '<div class=goalTitle>Goal No : <span class=goalTitleNumber>'+CONVERT(varchar(10),IEPGoalNo)+'</span> - '+A.GoalCode+'</div>' ELSE CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END " +
            //            " END AS Title,CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row,A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1, A.Objective2, A.Objective3,IEPGoalNo,GoalIEPNote FROM(" +
            //    "SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber," +
            //    "ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row," +
            //                " dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, " +
            //                " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3,(SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE StdtLessonplanId=" +
            //                "dbo.StdtLessonPlan.StdtLessonPlanId ORDER BY DSTempHdrId DESC) DSTemplateName" +
            //                        " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
            //                             "dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId in (" + items + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'  AND  dbo.StdtLessonPlan.ActiveInd = 'A' AND dbo.StdtLessonPlan.IncludeIEP=1)A " +
            //                             "INNER JOIN StdtGoal  on StdtGoal.GoalId =A.GoalId WHERE StdtGoal.StdtIEPId=  '" + sess.IEPId + "' and StdtGoal.IncludeIEP=1 ORDER BY IEPGoalNo";

            ///select LP name based on the status of IEP
            ///
            int statId = Convert.ToInt32(objData.FetchValue("SELECT StatusId FROM StdtIEP WHERE StdtIEPId= " + iepId + " AND StudentId= " + sess.StudentId));
            int statInProg= Convert.ToInt32(objData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress'"));
            int statPending= Convert.ToInt32(objData.FetchValue("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'"));
            if ((statId == statInProg) || (statId == statPending))
            {
                string strReplace = "StatusId FROM DSTempHdr WHERE StudentId=" + sess.StudentId + " AND LessonPlanId=dbo.StdtLessonPlan.LessonPlanId AND StatusId IN (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' ";
                string strReplace2 = "DSTemplateName FROM DSTempHdr WHERE StudentId=" + sess.StudentId + "  AND LessonPlanId=dbo.StdtLessonPlan.LessonPlanId AND StatusId = (SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus'";

                strQuery2 = "SELECT DISTINCT CASE A.Row WHEN 1 THEN '<div class=goalTitle>Goal No : <span class=goalTitleNumber>'+CONVERT(varchar(10),IEPGoalNo)+'</span> - '+A.GoalCode+'</div>' ELSE CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END " +
                            " END AS Title,CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row,A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1, A.Objective2, A.Objective3,IEPGoalNo,GoalIEPNote FROM(" +
                            "SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber," +
                            "ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row," +
                            " dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, " +
                            " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3," +
                            "(SELECT CASE WHEN (SELECT " + strReplace + " AND LookupName='Approved')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Approved')) " +
                            "ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='Pending Approval')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Pending Approval')) " +
                            "ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='In Progress')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='In Progress')) " +
                            "ELSE CASE WHEN (SELECT " + strReplace + " AND LookupName='Maintenance')) IS NOT NULL THEN (SELECT " + strReplace2 + " AND LookupName='Maintenance')) " +
                            //"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Expired')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Expired') ORDER BY DSTempHdrId DESC) " +
                            //"ELSE CASE WHEN (SELECT TOP 1 " + strReplace + " AND LookupName='Inactive')) IS NOT NULL THEN (SELECT TOP 1 " + strReplace2 + " AND LookupName='Inactive') ORDER BY DSTempHdrId DESC) END END "+
                            "END END END END) DSTemplateName" +
                            " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
                            "dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId in (" + items + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'  AND  dbo.StdtLessonPlan.ActiveInd = 'A' AND dbo.StdtLessonPlan.IncludeIEP=1 " +
                            "AND (select COUNT(StatusId) " + "FROM DSTempHdr WHERE LessonPlanId = dbo.StdtLessonPlan.lessonplanid AND StudentId=" + sess.StudentId + " AND StatusId IN " +
                            "(SELECT LookupId FROM LookUp WHERE LookupType='TemplateStatus' AND LookupName IN ('In Progress', 'Pending Approval', 'Approved', 'Maintenance')))>0)A " +
                            "INNER JOIN StdtGoal  on StdtGoal.GoalId =A.GoalId WHERE StdtGoal.StdtIEPId=  '" + sess.IEPId + "' and StdtGoal.IncludeIEP=1 " +
                            " ORDER BY IEPGoalNo";
            }
            else 
            {
                strQuery2 = "SELECT DISTINCT CASE A.Row WHEN 1 THEN '<div class=goalTitle>Goal No : <span class=goalTitleNumber>'+CONVERT(varchar(10),IEPGoalNo)+'</span> - '+A.GoalCode+'</div>' ELSE CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END " +
	                "END AS Title,CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row,A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1, A.Objective2, A.Objective3,IEPGoalNo,GoalIEPNote " +
	                "FROM( SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber, "+
	                "ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row, dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, "+
	                "dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3, (dbo.DSTempHdr.DSTemplateName) DSTemplateName "+
	                "FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId  INNER JOIN dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId "+
	                "INNER JOIN dbo.DSTempHdr on dbo.DSTempHdr.DSTempHdrId=dbo.StdtLessonPlan.DSTempHdrId where StdtLessonPlan.GoalId in (" + items + ") "+
                    "AND dbo.StdtLessonPlan.StdtIEPId = '" + sess.IEPId + "' AND dbo.StdtLessonPlan.IncludeIEP=1)A " +
                    "INNER JOIN StdtGoal  on StdtGoal.GoalId =A.GoalId WHERE StdtGoal.StdtIEPId='" + sess.IEPId + "' and StdtGoal.IncludeIEP=1 ORDER BY IEPGoalNo";
            }
            Dt2 = objData.ReturnDataTable(strQuery2, false);

            ///end
            ///


            //strQuery2 = "SELECT CASE A.Row WHEN 1 THEN '<div class=goalTitle>Goal No : '+CONVERT(varchar(10),A.GoalNumber)+' - '+A.GoalCode+'</div>' ELSE CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END " +
            //            " END AS Title,CASE WHEN A.DSTemplateName IS NULL THEN A.LessonPlanName ELSE A.DSTemplateName END LessonPlanName,A.GoalCode,A.GoalId,A.GoalNumber,A.Row,A.LessonPlanId,A.StdtIEPId,A.StdtLessonPlanId,A.Objective1, A.Objective2, A.Objective3,IEPGoalNo FROM(" +
            //    "SELECT dbo.LessonPlan.LessonPlanName, dbo.Goal.GoalCode, dbo.StdtLessonPlan.GoalId,DENSE_RANK() OVER (ORDER BY dbo.StdtLessonPlan.GoalId) As GoalNumber," +
            //    "ROW_NUMBER() OVER(PARTITION BY dbo.Goal.GoalCode ORDER BY dbo.Goal.GoalCode DESC) AS Row," +
            //                " dbo.StdtLessonPlan.LessonPlanId, dbo.StdtLessonPlan.StdtIEPId, dbo.StdtLessonPlan.StdtLessonPlanId, " +
            //                " dbo.StdtLessonPlan.Objective1, dbo.StdtLessonPlan.Objective2, dbo.StdtLessonPlan.Objective3,(SELECT TOP 1 DSTemplateName FROM DSTempHdr WHERE StdtLessonplanId=" +
            //                "dbo.StdtLessonPlan.StdtLessonPlanId ORDER BY DSTempHdrId DESC) DSTemplateName  " +
            //                        " FROM dbo.StdtLessonPlan INNER JOIN dbo.Goal ON dbo.StdtLessonPlan.GoalId = dbo.Goal.GoalId INNER JOIN  " +
            //                             "dbo.LessonPlan ON dbo.StdtLessonPlan.LessonPlanId = dbo.LessonPlan.LessonPlanId where StdtLessonPlan.GoalId in (" + items + ") AND dbo.StdtLessonPlan.StdtIEPId =  '" + sess.IEPId + "'  AND  dbo.StdtLessonPlan.ActiveInd = 'A')A " +
            //                             "INNER JOIN StdtGoal  on StdtGoal.GoalId =A.GoalId WHERE StdtGoal.StdtIEPId=  '" + sess.IEPId + "' ORDER BY IEPGoalNo";
            //Dt2 = objData.ReturnDataTable(strQuery2, false);
            DataTable dtGoal = new DataTable();
            if (Dt2 != null)
            {

                dtGoal.Columns.Add(new DataColumn("Title", typeof(string)));
                dtGoal.Columns.Add(new DataColumn("StdtLessonPlanId", typeof(Int32)));
                dtGoal.Columns.Add(new DataColumn("LessonPlanId", typeof(Int32)));
                dtGoal.Columns.Add(new DataColumn("Objective2", typeof(string)));
                dtGoal.Columns.Add(new DataColumn("Objective3", typeof(string)));
                dtGoal.Columns.Add(new DataColumn("GoalId", typeof(Int32)));
                dtGoal.Columns.Add(new DataColumn("IEPGoalNo", typeof(Int32)));
                dtGoal.Columns.Add(new DataColumn("GoalIEPNote", typeof(string)));
                // dtGoal.Columns.Add(new DataColumn("LessonOrder", typeof(Int32)));

                foreach (DataRow row in Dt2.Rows)
                {
                    if (row["Title"].ToString().Contains("</div>"))
                    {
                        dtGoal.Rows.Add(row["Title"].ToString(), row["StdtLessonPlanId"], row["LessonPlanId"], row["Objective2"], row["Objective3"], row["GoalId"], row["IEPGoalNo"], row["GoalIEPNote"]);
                    }

                }
            }

            dlGoals.DataSource = dtGoal;
            dlGoals.DataBind();
            dlGoals2.DataSource = dtGoal;
            dlGoals2.DataBind();

            foreach (DataListItem diGoal2 in dlGoals2.Items)
            {
                int flag = 1;
                DropDownList drpSortNumebr = (DropDownList)diGoal2.FindControl("drpSortOrder");
                //drpSortNumebr.Items.Insert(i,i.ToSt
                for (int i = 0; i < dlGoals.Items.Count; i++)
                {
                    drpSortNumebr.Items.Add(new ListItem(flag.ToString(), flag.ToString(), true));
                    flag++;
                }

                //drpSortNumebr.Attributes.Add
                //drpSortNumebr.DataTextField = "IEPGoalNo";
                //drpSortNumebr.DataValueField = "IEPGoalNo";
                //drpSortNumebr.DataBind();

            }

            //ClientScript.RegisterStartupScript(GetType(), "", "ResetGoalSortOrder();", true);
        }
        else
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("Please select Goal");
            return;
        }
    }
    protected void btnsaveorder_Click(object sender, EventArgs e)
    {
        objData = new clsData();
        foreach (DataListItem diGoal in dlGoals2.Items)
        {
            HiddenField hf_GoalId = (HiddenField)diGoal.FindControl("hfSortGoalId");
            TextBox drpSortId = (TextBox)diGoal.FindControl("drpSortOrder_txt");
            strQuery = "Update StdtGoal Set IEPGoalNo=" + Convert.ToInt32(drpSortId.Text) + " Where SchoolId=" + sess.SchoolId + " And StudentId=" + sess.StudentId + " And GoalId=" + hf_GoalId.Value + "  And StdtIEPId=" + sess.IEPId + "  ";
            retVal = objData.Execute(strQuery);
        }
        fillCPLevel();
    }
}