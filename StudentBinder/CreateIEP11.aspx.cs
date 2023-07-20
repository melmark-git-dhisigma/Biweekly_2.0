using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;

public partial class StudentBinder_CreateIEP11 : System.Web.UI.Page
{
    public static clsData objData = null;
    public static clsSession sess = null;
    static int IEPId = 0;
    string strQuery = "";
    int retVal = 0;
    DataTable Dt = null;
    DataTable Dt1 = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        clsIEP IEPObj = new clsIEP();
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        if (!IsPostBack)
        {
            Fill();
            setInitialGrid();

            bool Disable = false;
            clsGeneral.PageReadAndWrite(sess.LoginId, sess.SchoolId, out Disable);
            if (Disable == true)
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }

            ViewAccReject();
            string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
            if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }
        }
    }
    protected void Fill()
    {
        sess = (clsSession)Session["UserSession"];
        clsIEP IEPObj = new clsIEP();
        objData = new clsData();
        string strQuery = "";
        string strQuery1 = "";
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        if (sess != null)
        {
            strQuery="SELECT StudentPersonal.StudentPersonalId, StudentPersonal.LastName, StudentPersonal.BirthDate, ";
            strQuery+="StdtIEPExt4.PoMEliDeter, StdtIEPExt4.AtndDate, StdtIEPExt4.PoMIEPDev, StdtIEPExt4.PoMPlacement, StdtIEPExt4.PoMInitEval, StdtIEPExt4.PoMInit, StdtIEPExt4.PoMReeval, StdtIEPExt4.PoMAnnRev, StdtIEPExt4.PoMOtherCheck, StdtIEPExt4.PoMOtherText ";
            strQuery += "FROM StudentPersonal JOIN StdtIEP ON StudentPersonal.StudentPersonalId=StdtIEP.StudentId JOIN StdtIEPExt4 ON StdtIEP.StdtIEPId=StdtIEPExt4.StdtIEPId WHERE StdtIEP.SchoolId=" + sess.SchoolId + " AND StdtIEP.StudentId=" + sess.StudentId +" AND StdtIEP.StdtIEPId=" + sess.IEPId;

            strQuery1 = "SELECT StudentPersonal.StudentPersonalId, StudentPersonal.LastName, StudentPersonal.FirstName, StudentPersonal.BirthDate, StudentPersonal.AdmissionDate FROM StudentPersonal JOIN StdtIEP ON StudentPersonal.StudentPersonalId=StdtIEP.StudentId WHERE StdtIEP.SchoolId=" + sess.SchoolId + " AND StdtIEP.StudentId=" + sess.StudentId + " AND StdtIEP.StdtIEPId=" + sess.IEPId;
           
            
            //stQuery = "SELECT StdtIEPExt4.AtndDate WHERE StdtIEP.SchoolId=" + sess.SchoolId + " AND StdtIEP.StudentId=" + sess.StudentId + " AND StdtIEP.StdtIEPId=" + sess.IEPId;
        }

        Dt = objData.ReturnDataTable(strQuery, false);
        Dt1 = objData.ReturnDataTable(strQuery1, false);
        //Dt2=objData.Execute(strQuery2);
        Clear();
        //txtdate.Text=
        if(Dt1.Rows.Count>0)
        {
            //if (Dt1.Rows[0]["AdmissionDate"].ToString().Trim() != null && Dt1.Rows[0]["AdmissionDate"].ToString().Trim() != "")
             //  lblDate.Text = DateTime.Parse(Dt1.Rows[0]["AdmissionDate"].ToString().Trim()).ToString("MM/dd/yyyy");
           // else
            //    lblDate.Text = "";
           
            if (Dt1.Rows[0]["BirthDate"].ToString().Trim() != null && Dt1.Rows[0]["BirthDate"].ToString().Trim() != "")
                lblDOB.Text = DateTime.Parse(Dt1.Rows[0]["BirthDate"].ToString().Trim()).ToString("MM/dd/yyyy");
            else
                lblDOB.Text = "";
            lblStudentName.Text = Dt1.Rows[0]["FirstName"].ToString().Trim() +" "+ Dt1.Rows[0]["LastName"].ToString().Trim();
            lblID.Text = Dt1.Rows[0]["StudentPersonalId"].ToString().Trim();
        }
        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["AtndDate"].ToString().Trim() != null && Dt.Rows[0]["AtndDate"].ToString().Trim() != "")
                txtdate.Text = DateTime.Parse(Dt.Rows[0]["AtndDate"].ToString().Trim()).ToString("MM/dd/yyyy");
            else
                txtdate.Text = "";
            chkEliDeter.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMEliDeter"].ToString());
            chkIEPDev.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMIEPDev"].ToString());
            chkPlacement.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMPlacement"].ToString());
            chkInitEval.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMInitEval"].ToString());
            chkInit.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMInit"].ToString());
            chkReeval.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMReeval"].ToString());
            chkAnnRev.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMAnnRev"].ToString());
            chkOther.Checked = clsGeneral.getChecked(Dt.Rows[0]["PoMOtherCheck"].ToString());

            txtOther.Text = Dt.Rows[0]["PoMOtherText"].ToString().Trim();

            btnSave.Text = "Save and continue";
        }
        string Status = IEPObj.GETIEPStatus(sess.IEPId, sess.StudentId, sess.SchoolId);
        if (Status.Trim() == "Approved" || Status.Trim() == "Expired")
        {
            btnSave.Visible = false;
        }
        else
        {
            btnSave.Visible = true;
        }
    }


    private void Clear()
    {
        txtOther.Text = "";
        chkEliDeter.Checked = false;
        chkIEPDev.Checked = false;
        chkPlacement.Checked = false;
        chkInitEval.Checked = false;
        chkInit.Checked = false;
        chkReeval.Checked = false;
        chkAnnRev.Checked = false;
        chkOther.Checked = false;

        btnSave.Text = "Save and continue";
    }
    private void ViewAccReject()
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        int reject = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Rejected'");
        int approve = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Approved'");
        if (sess.IEPStatus == approve || sess.IEPStatus == reject)
        {
            btnSave.Visible = false;
        }
    }
    protected void Save()
    {
        try
        {
            sess = (clsSession)Session["UserSession"];
            objData = new clsData();

            if (sess.IEPId == null)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
                return;
            }
            if (IEPId > 0)
            {
                String date = txtdate.Text;
                strQuery = "Update  StdtIEPExt4 set StdtIEPId=" + sess.IEPId + ",PoMEliDeter='" + chkEliDeter.Checked + "',PoMIEPDev='" + chkIEPDev.Checked + "',PoMPlacement='" + chkPlacement.Checked + "',PoMInitEval='" + chkInitEval.Checked + "',PoMInit='" + chkInit.Checked + "',PoMReeval='" + chkReeval.Checked + "',PoMAnnRev='" + chkAnnRev.Checked + "',PoMOtherCheck='" + chkOther.Checked + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate(),AtndDate='" + clsGeneral.convertQuotes(txtdate.Text) + "' where StdtIEPId=" + sess.IEPId + "";
                retVal = objData.Execute(strQuery);
            }
            else if (IEPId == 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Please Save IEP Page 1 for " + sess.StudentName + "!");
                return;
            }

            if (retVal != 0)
            {
                tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP4();", true);
            }
            else
            {
                tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation failed...");
            }

        }
        catch(Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed!");
            throw Ex;
        }
    }
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            Update();
            setInitialGrid();   //
            Fill();
        }

    }
    protected void btnSave_hdn_Click(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        DataClass oData = new DataClass();
        objData = new clsData();
        string pendstatus = "";
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }
        int pendingApprove = oData.ExecuteScalar("SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='Pending Approval'");
        pendstatus = objData.FetchValue("Select StatusId from StdtIEP where StdtIEPId=" + sess.IEPId + " ").ToString();
        if (Convert.ToString(pendingApprove) == pendstatus)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP is in Pending State.");
        }
        else
        {
            Update1();
            setInitialGrid();   //
            Fill();
        }

    }
    protected void Update()
    {
        try
        {
            objData = new clsData();
            DataClass oData = new DataClass();
            sess = (clsSession)Session["UserSession"];

            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved" || StatusName == "Rejected")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                ifExist();
                String date = txtdate.Text;
                //DateTime date = CalendarExtender1.SelectedDate.Value;
                if (objData.IFExists("select TableId from StdtIEPExt5 where StdtIEPId=" + sess.IEPId) == true)
                {
                    //String date = (CalendarExtender1.SelectedDate).Value;

                    strQuery = "Update  StdtIEPExt4 set PoMEliDeter='" + chkEliDeter.Checked + "',PoMIEPDev='" + chkIEPDev.Checked + "',PoMPlacement='" + chkPlacement.Checked + "',PoMInitEval='" + chkInitEval.Checked + "',PoMInit='" + chkInit.Checked + "',PoMReeval='" + chkReeval.Checked + "',PoMAnnRev='" + chkAnnRev.Checked + "',PoMOtherCheck='" + chkOther.Checked + "',PoMOtherText='" + clsGeneral.convertQuotes(txtOther.Text) + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate(),AtndDate='" + clsGeneral.convertQuotes(txtdate.Text) + "' where StdtIEPId=" + sess.IEPId + "";
                }
                else
                {
                    strQuery = "INSERT INTO StdtIEPExt4 (StdtIEPId,PoMEliDeter,PoMIEPDev,PoMPlacement,PoMInitEval,PoMInit,PoMReeval,PoMAnnRev,PoMOtherCheck,PoMOtherText,CreatedBy,CreatedOn,AtndDate) " +
                        "VALUES (" + sess.IEPId + ",'" + chkEliDeter.Checked + "','" + chkIEPDev.Checked + "','" + chkPlacement.Checked + "','" + chkInitEval.Checked + "','" + chkInit.Checked + "','" + chkReeval.Checked + "','" + chkAnnRev.Checked + "','" + chkOther.Checked + "','" + clsGeneral.convertQuotes(txtOther.Text) + "','" + sess.LoginId + "','" + clsGeneral.convertQuotes(txtdate.Text) + "'";
                }
                retVal = objData.Execute(strQuery);

                    try
                    {
                        SaveIEPPage();
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                    {
                        objData.Execute("update stdtIEPUpdateStatus set Page11='true' where stdtIEPId=" + sess.IEPId);
                    }
                    else
                    {
                        objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page11) values(" + sess.IEPId + ",'true')");
                    }
                    ClientScript.RegisterStartupScript(GetType(), "", "parent.moveToNextTab(12);", true);
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation Failed!");
            throw Ex;
        }
    }
    protected void Update1()
    {
        try
        {
            objData = new clsData();
            DataClass oData = new DataClass();
            sess = (clsSession)Session["UserSession"];

            string StatusName = objData.FetchValue("Select LookupName from LookUp where LookupId=" + sess.IEPStatus).ToString();

            if (StatusName == "Approved" || StatusName == "Rejected")
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Data IEP Page 1 is " + StatusName + ". Modification Disabled!!!");
                return;
            }
            else
            {
                ifExist();
                String date = txtdate.Text;
                //DateTime date = CalendarExtender1.SelectedDate.Value;
                if (objData.IFExists("select TableId from StdtIEPExt5 where StdtIEPId=" + sess.IEPId) == true)
                {
                    strQuery = "Update  StdtIEPExt4 set PoMEliDeter='" + chkEliDeter.Checked + "',PoMIEPDev='" + chkIEPDev.Checked + "',PoMPlacement='" + chkPlacement.Checked + "',PoMInitEval='" + chkInitEval.Checked + "',PoMInit='" + chkInit.Checked + "',PoMReeval='" + chkReeval.Checked + "',PoMAnnRev='" + chkAnnRev.Checked + "',PoMOtherCheck='" + chkOther.Checked + "',PoMOtherText='" + clsGeneral.convertQuotes(txtOther.Text) + "',ModifiedBy=" + sess.LoginId + ",ModifiedOn=getdate(),AtndDate='" + clsGeneral.convertQuotes(txtdate.Text) + "' where StdtIEPId=" + sess.IEPId + "";
                }
                else
                {
                    strQuery = "INSERT INTO StdtIEPExt4 (StdtIEPId,PoMEliDeter,PoMIEPDev,PoMPlacement,PoMInitEval,PoMInit,PoMReeval,PoMAnnRev,PoMOtherCheck,PoMOtherText,CreatedBy,CreatedOn,AtndDate) " +
                        "VALUES (" + sess.IEPId + ",'" + chkEliDeter.Checked + "','" + chkIEPDev.Checked + "','" + chkPlacement.Checked + "','" + chkInitEval.Checked + "','" + chkInit.Checked + "','" + chkReeval.Checked + "','" + chkAnnRev.Checked + "','" + chkOther.Checked + "','" + clsGeneral.convertQuotes(txtOther.Text) + "','" + sess.LoginId + "',GETDATE(),'" + clsGeneral.convertQuotes(txtdate.Text) + "')";
                }
                retVal = objData.Execute(strQuery);

                try
                {
                    SaveIEPPage();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

                if (retVal != 0)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                    if (objData.IFExists("select stdtIEPUdateStatusId from stdtIEPUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                    {
                        objData.Execute("update stdtIEPUpdateStatus set Page11='true' where stdtIEPId=" + sess.IEPId);
                    }
                    else
                    {
                        objData.Execute("insert into stdtIEPUpdateStatus(stdtIEPId,Page11) values(" + sess.IEPId + ",'true')");
                    }
                    //ClientScript.RegisterStartupScript(GetType(), "", "parent.CreateIEP12();", true);
                }
            }
        }
        catch (Exception Ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg(" Updation Failed!");
            throw Ex;
        }
    }
    protected bool SaveIEPPageb()
    {
        bool getA = true;
        objData = new clsData();
        DataClass oData = new DataClass();
        
        try
        {
            sess = (clsSession)Session["UserSession"];
            foreach (GridViewRow diTypeA in gvAttnSheet.Rows)
            {
                objData = new clsData();

                TextBox txtTMName = diTypeA.FindControl("txtTMName") as TextBox;
                TextBox txtTMRole = diTypeA.FindControl("txtTMRole") as TextBox;
                TextBox txtInitialIfInAttn = diTypeA.FindControl("txtInitialIfInAttn") as TextBox;

                string insDelivery = "";
                int i;

                if (objData.IFExists("select TableId from StdtIEPExt5 where StdtIEPId=" + sess.IEPId) == true)
                {
                    int tableId = oData.ExecuteScalar("SELECT TableId FROM StdtIEPExt5 WHERE StdtIEPId=" + sess.IEPId);

                    insDelivery = "Update StdtIEPExt5 set TMName='" + clsGeneral.convertQuotes(txtTMName.Text) + "',"
                    + "TMRole='" + clsGeneral.convertQuotes(txtTMRole.Text) + "',"
                    + "InitialIfInAttn='" + clsGeneral.convertQuotes(txtInitialIfInAttn.Text) + "' where StdtIEPId=" + tableId;

                    i = objData.Execute(insDelivery);
                }
                else
                {
                    insDelivery = "INSERT INTO StdtIEPExt5 (StdtIEPId,TMName,TMRole,InitialIfInAttn) " +
                                          "VALUES ('" + sess.IEPId + "', '" + txtTMName.Text + "', '" + txtTMRole.Text + "', '" + txtInitialIfInAttn.Text + "')";

                    i = objData.Execute(insDelivery);
                }
                    if (i >= 1)
                {
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Updated Successfully");
                }

                if (objData.IFExists("select stdtIEPPEUpdateStatusId from StdtIEP_PEUpdateStatus where stdtIEPId=" + sess.IEPId) == true)
                {
                    objData.Execute("update StdtIEP_PEUpdateStatus set Page1='true' where stdtIEPId=" + sess.IEPId);
                }
                else
                {
                    objData.Execute("insert into StdtIEP_PEUpdateStatus(stdtIEPId,Page1) values(" + sess.IEPId + ",'true')");
                }

            }
        }
        catch (SqlException Ex)
        {
            getA = false;
            tdMsg.InnerHtml = clsGeneral.failedMsg("Insertion Failed!");
            return false;
            throw Ex;
        }
        return getA;
    }

    protected bool SaveIEPPage()
    {


        foreach (GridViewRow diTypeA in gvAttnSheet.Rows)
        {
            objData = new clsData();

            TextBox txtTMName = diTypeA.FindControl("txtTMName") as TextBox;
            TextBox txtTMRole = diTypeA.FindControl("txtTMRole") as TextBox;
            TextBox txtInitialIfInAttn = diTypeA.FindControl("txtInitialIfInAttn") as TextBox;

            Label lbl_StdtGoalSvcId = diTypeA.FindControl("lbl_svcid") as Label;
            int StdtGoalSvcId = (lbl_StdtGoalSvcId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvcId.Text);
            string a = txtTMName.Text;
            string b = txtTMRole.Text;
            string c = txtInitialIfInAttn.Text;

            string insDelivery = "";
            if (StdtGoalSvcId == 0)
            {
                if (a == "" && b == "" && c == "")
                {
                    //if the 3 fields are null
                }
                else
                {
                    insDelivery = "INSERT INTO StdtIEPExt5 (StdtIEPId,TMName,TMRole,InitialIfInAttn) " +
                                              "VALUES ('" + sess.IEPId + "', '" + txtTMName.Text + "', '" + txtTMRole.Text + "', '" + txtInitialIfInAttn.Text + "')";

                    int intStdtGoalSvcId = objData.ExecuteWithScope(insDelivery);
                }
            }
            else
            {
                insDelivery = "Update StdtIEPExt5 set TMName='" + clsGeneral.convertQuotes(txtTMName.Text) + "'," +
                      "TMRole='" + clsGeneral.convertQuotes(txtTMRole.Text) + "'," +
                      "InitialIfInAttn='" + clsGeneral.convertQuotes(txtInitialIfInAttn.Text) + "' where TableId=" + StdtGoalSvcId;

                int i = objData.Execute(insDelivery);
            }
        }
        return true;
    }
    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        AddNewRowToGrid();
    }

    private void AddNewRowToGrid()
    {
        int rowIndex = 0;

        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];

            if (dtCurrentTable.Rows.Count > 14)
            {
                tdMsg.InnerHtml = clsGeneral.warningMsg("Sorry!!Limited to 15 Rows");
                return;
            }

            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                  
                    TextBox box0 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMName");
                    TextBox box1 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMRole");
                    TextBox box2 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtInitialIfInAttn");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i - 1]["TMName"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["TMRole"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["InitialIfInAttn"] = box2.Text;
                    
                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["PreviousTable"] = dtCurrentTable;

                gvAttnSheet.DataSource = dtCurrentTable;
                gvAttnSheet.DataBind();

                SetPreviousDB();
            }
        }
        else if (ViewState["CurrentTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMName");
                    TextBox box1 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMRole");
                    TextBox box2 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtInitialIfInAttn");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i - 1]["TMName"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["TMRole"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["InitialIfInAttn"] = box2.Text;

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["CurrentTable"] = dtCurrentTable;

                gvAttnSheet.DataSource = dtCurrentTable;
                gvAttnSheet.DataBind();

                SetPreviousData();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }
    }
    private void SetPreviousDB()
    {
        int rowIndex = 0;
        if (ViewState["PreviousTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["PreviousTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMName");
                    TextBox box1 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMRole");
                    TextBox box2 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtInitialIfInAttn");

                    box0.Text = dt.Rows[rowIndex]["TMName"].ToString();
                    box1.Text = dt.Rows[rowIndex]["TMRole"].ToString();
                    box2.Text = dt.Rows[rowIndex]["InitialIfInAttn"].ToString();

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["PreviousTable"];
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["CurrentTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["CurrentTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    
                    TextBox box0 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMName");
                    TextBox box1 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMRole");
                    TextBox box2 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtInitialIfInAttn");

                    box0.Text = dt.Rows[i]["TMName"].ToString();
                    box1.Text = dt.Rows[i]["TMRole"].ToString();
                    box2.Text = dt.Rows[i]["InitialIfInAttn"].ToString();

                    rowIndex++;
                }
            }
        }
        DataTable dt1 = (DataTable)ViewState["CurrentTable"];
        if (dt1.Rows.Count > 1)
        {

            LinkButton LinkButton1 = gvAttnSheet.FooterRow.FindControl("LinkButton1") as LinkButton;
            LinkButton1.Visible = true;
        }
    }
    private void setInitialGrid()
    {
        objData = new clsData();
        DataClass oData = new DataClass();
        sess = (clsSession)Session["UserSession"];
        if (sess.IEPId <= 0) return;
        if (sess.IEPId == null)
        {
            tdMsg.InnerHtml = clsGeneral.warningMsg("IEP not Properly Selected");
            return;
        }

        DataTable dt = new DataTable();
        dt.Columns.Add("TableId", typeof(string));
        dt.Columns.Add("TMName", typeof(string));
        dt.Columns.Add("TMRole", typeof(string));
        dt.Columns.Add("InitialIfInAttn", typeof(string));
        
        string getPageOneGridDetails = "SELECT [TableId], [TMName], [TMRole], [InitialIfInAttn] FROM [dbo].[StdtIEPExt5] WHERE [StdtIEPId]=" + sess.IEPId;

        DataTable dt_goalDetails = objData.ReturnDataTable(getPageOneGridDetails, false);
        if (dt_goalDetails != null)
        {
            if (dt_goalDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_goalDetails.Rows)
                {
                    dt.Rows.Add(dr["TableId"].ToString(), dr["TMName"].ToString(), dr["TMRole"].ToString(), dr["InitialIfInAttn"].ToString());
                }
            }
            else
            {
                dt.Rows.Add("", "", "","");
            }
        }
        else
        {
            dt.Rows.Add("", "", "","");
        }
        ViewState["PreviousTable"] = dt;

        gvAttnSheet.DataSource = dt;
        gvAttnSheet.DataBind();
    }
    protected void gvAttnSheet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int svcGoalId = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbl_svcTblId = (Label)e.Row.Cells[0].FindControl("lbl_svcid");

            if (lbl_svcTblId != null)
            {
                svcGoalId = (lbl_svcTblId.Text == "") ? -1 : Convert.ToInt32(lbl_svcTblId.Text);
            }
        }
    }
    protected void gvAttnSheet_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        int index = 0;
        if (e.CommandArgument.ToString() != "")
        {
            index = int.Parse(e.CommandArgument.ToString());
        }
        else
        {
            return;
        }
        GridViewRow row = gvAttnSheet.Rows[index];

        if (e.CommandName == "remove")
        {
            if (gvAttnSheet.Rows.Count > 1)
            {
                objData = new clsData();
                Label lbl_StdtGoalSvdId = (Label)row.FindControl("lbl_svcid");
                int StdtGoalSvcId = (lbl_StdtGoalSvdId.Text == "") ? 0 : Convert.ToInt32(lbl_StdtGoalSvdId.Text);

                if (StdtGoalSvcId > 0)
                {
                    string delRow = "delete from StdtIEPExt5 where TableId=" + StdtGoalSvcId;

                    int i = objData.Execute(delRow);
                    deleteRow(index);
                }
                else
                {
                    deleteRow(index);
                }
            }
        }
    }
    private void deleteRow(int rowID)
    {
        tdMsg.InnerHtml = "";
        int rowIndex = 0;

        if (ViewState["PreviousTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["PreviousTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    TextBox box0 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMName");
                    TextBox box1 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtTMRole");
                    TextBox box2 = (TextBox)gvAttnSheet.Rows[rowIndex].Cells[0].FindControl("txtInitialIfInAttn");

                    drCurrentRow = dtCurrentTable.NewRow();

                    dtCurrentTable.Rows[i - 1]["TMName"] = box0.Text;
                    dtCurrentTable.Rows[i - 1]["TMRole"] = box1.Text;
                    dtCurrentTable.Rows[i - 1]["InitialIfInAttn"] = box2.Text;

                    rowIndex++;
                }

                dtCurrentTable.Rows.Remove(dtCurrentTable.Rows[rowID]);

                ViewState["PreviousTable"] = dtCurrentTable;

                gvAttnSheet.DataSource = dtCurrentTable;
                gvAttnSheet.DataBind();

                SetPreviousDB();
            }
        }
    }
    protected void ifExist()
    {

        DataClass oData = new DataClass();
        clsSession oSession = (clsSession)Session["UserSession"];
        string selIEPstatus = "SELECT LookupId FROM LookUp WHERE LookupType='IEP Status' AND LookupName='In Progress'";
        int IEPstatus = oData.ExecuteScalar(selIEPstatus);
        string copyIEP = "SELECT MAX(StdtIEPId) AS StdtIEPId FROM StdtIEP WHERE StudentId=" + oSession.StudentId + " ";
        int newIEP = oData.ExecuteScalar(copyIEP);

        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt4 WHERE StdtIEPId=" + newIEP + " ") == false)
        {
            string insIEP4 = "INSERT INTO StdtIEPExt4(StdtIEPId,StatusId,CreatedBy,CreatedOn) VALUES(" + newIEP + "," + IEPstatus + "," + oSession.LoginId + "," +
           "(SELECT convert(varchar, getdate(), 100)))";
            int newVersion = oData.ExecuteNonQuery(insIEP4);
        }
        if (objData.IFExists("SELECT StdtIEPId AS StdtIEPId FROM StdtIEPExt5 WHERE StdtIEPId=" + newIEP + " ") == false)
        {
            string insIEP5 = "INSERT INTO StdtIEPExt5(StdtIEPId) VALUES(" + newIEP + ")";
            oData.ExecuteNonQuery(insIEP5);
        }

    }
}