using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Calculate;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Drawing;


public partial class StudentBinder_DatasheetPreview : System.Web.UI.Page
{

    public static clsData objData = null;
    clsData oData = null;
    ClsTemplateSession oTemp = null;
    clsSession oSession = null;
    clsDataSheet oDS = null;
    DiscreteSession oDisc = null;
    string DatasheetKey = "";
    int repeatNo = 0;
    static bool Disable = false;
    Dictionary<string, string> samcnt = new Dictionary<string, string>();
    string[] limtReachedSamples = new string[0];
    string preSampleString = "";
    string[] QuestnAary = new string[0];
    bool updateCheck = true;


    protected void Page_Load(object sender, EventArgs e)
    {
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        if (!IsPostBack)
        {
            hfProbe.Value = "No Probe";
            if (Request.QueryString["pageid"] != null)
            {
                oTemp.TemplateId = Convert.ToInt32(Request.QueryString["pageid"]);
            }
            else
            {
                oTemp.TemplateId = 0;

            }
            hdnTemplateId.Value = oTemp.TemplateId.ToString();
            DatasheetKey = "DataSht_Sess-" + hdnTemplateId.Value.ToString();
            if (Request.QueryString["isMaint"] != null && Request.QueryString["isMaint"] == "true")
            {
                hdn_isMaintainance.Value = "true";
            }
            else
            {
                hdn_isMaintainance.Value = "false";
            }
            if (Request.QueryString["SRMode"] != null && Request.QueryString["SRMode"] == "true")
            {
                //if submit & repeat, display repeat count
                lblSubmitAndRepeatText.Visible = true;
                lblSubmitAndRepeatCount.Visible = true;
                lblSubmitAndRepeatCount.Text = Request.QueryString["repeatNo"];
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "probe();", true);
                //btnProbe_Click(sender, e);
            }
            Session[DatasheetKey] = null;


            if (Request.QueryString["SessHdrID"] != null)
            {

                oData = new clsData();
                oDS = new clsDataSheet();

                string strqry = "SELECT DSTempHdrId FROM StdtSessionHdr WHERE StdtSessionHdrId=" + Request.QueryString["SessHdrID"];
                object objSessHdrID = oData.FetchValue(strqry);
                if (objSessHdrID != null)
                {
                    oTemp.TemplateId = Convert.ToInt32(objSessHdrID);

                }


                hdnTemplateId.Value = oTemp.TemplateId.ToString();
                DatasheetKey = "DataSht_Sess-" + hdnTemplateId.Value.ToString();
                Session[DatasheetKey] = oDS;
                ViewState["IsHistory"] = true;
                ViewState["StdtSessHdr"] = Convert.ToInt32(Request.QueryString["SessHdrID"]);
                object objIOAInd = oData.FetchValue("SELECT IOAInd FROM StdtSessionHdr WHERE StdtSessionHdrId=" + Request.QueryString["SessHdrID"].ToString());
                if (objIOAInd != null)
                {
                    object objMistrial = oData.FetchValue("SELECT SessMissTrailStus FROM StdtSessionHdr WHERE StdtSessionHdrId=" + Request.QueryString["SessHdrID"].ToString());
                    if (objMistrial != null)
                    {
                        if (objMistrial.ToString() == "Y")
                        {
                            oDS.SessionMistrial = true;
                        }
                    }

                    object objMistrialRsn = oData.FetchValue("SELECT SessMissTrailRsn FROM StdtSessionHdr WHERE StdtSessionHdrId=" + Request.QueryString["SessHdrID"].ToString());
                    if (objMistrialRsn != null)
                    {
                        oDS.SessionMistrialRsn = objMistrialRsn.ToString();
                    }

                    oDS.IOAInd = objIOAInd.ToString();


                    setTempData(Request.QueryString["SessHdrID"]);

                    //generateSheet();                   
                    LoadData(Convert.ToInt32(Request.QueryString["SessHdrID"]), false);

                    loadSetsOverride();


                    calculateFormula();
                }
            }
            else
            {
                if (Request.QueryString["SessionHdr"] != null)
                {
                    setTempData(Request.QueryString["SessionHdr"]);
                }
                ViewState["IsHistory"] = false;

                checkStat(sender, e);
            }


            FillDoc(oTemp.TemplateId);
            //loadSetsOverride();
            //if (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"))
            //{
            //    IfrmTimer.Attributes.Add("src", "DatasheetTimerIpad.aspx");
            //}
            //else {
            //    IfrmTimer.Attributes.Add("src", "dataSheetTimer.aspx");//dataSheetTimer
            //}

            string sqlStr = "SELECT ChainType,TotalTaskType FROM DSTempHdr where DSTempHdrId=" + oTemp.TemplateId;
            DataTable dataT = oData.ReturnDataTable(sqlStr, false);

            string type = dataT.Rows[0]["ChainType"].ToString();
            string totalTasktype = dataT.Rows[0]["TotalTaskType"].ToString();

            if (totalTasktype == "1")
            {
                totalTaskOverride.Visible = true;
                normalOverride.Visible = false;
            }
            if (totalTasktype == "0" || totalTasktype == "")
            {
                totalTaskOverride.Visible = false;
                normalOverride.Visible = true;
            }

            clsGeneral.Btn_Write(oSession.LoginId, oSession.SchoolId, out Disable);
            if (Disable == true)
            {
                ImgBtn_Override.Visible = false;
            }
            else
            {
                ImgBtn_Override.Visible = true;
            }

        }
        else
        {
            bool ContrlEnable = true;
            if (Session["NewTemplateId"] == null)
            {
                DatasheetKey = "DataSht_Sess-" + hdnTemplateId.Value.ToString();
                oTemp.TemplateId = Convert.ToInt32(hdnTemplateId.Value);
                oDS = (clsDataSheet)Session[DatasheetKey];
            }
            else
            {
                DatasheetKey = "DataSht_Sess-" + hdnTemplateId.Value.ToString();
                oDS = (clsDataSheet)Session[DatasheetKey];
            }
            if (oDS != null)
                if (oDS.dtColumns != null)
                {
                    if (oDS.ISVTool == 1) { ContrlEnable = false; }
                    int count = grdDataSht.Columns.Count;
                    for (int i = 0; i < count; i++)
                        grdDataSht.Columns.RemoveAt(0);
                    string[] colnames = new string[] { "Step / Sample / Sd", "Mistrial", "Notes" }; int index = 0;
                    foreach (string colnam in colnames)
                    {
                        TemplateField ItemTmpFld = new TemplateField();
                        // create HeaderTemplate
                        ItemTmpFld.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "SessStepID,StepCd,StepId",
                                                                      colnam, colnam, "", "", true, 0);
                        // create ItemTemplate
                        ItemTmpFld.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "SessStepID,StepCd,StepId",
                                                                      colnam, colnam, "", "", true, 0);
                        ItemTmpFld.HeaderStyle.Width = Unit.Percentage(20);
                        ItemTmpFld.HeaderStyle.CssClass = "clr"; ItemTmpFld.ItemStyle.CssClass = "clr";
                        if (index == 1)
                        {
                            ItemTmpFld.HeaderStyle.Width = Unit.Percentage(7);
                        }
                        if (index == 2) { ItemTmpFld.ItemStyle.CssClass = "nobdr"; ItemTmpFld.HeaderStyle.CssClass = "nobdr"; }
                        grdDataSht.Columns.Insert(index, ItemTmpFld);
                        index++;
                    }
                    foreach (DataRow dr in oDS.dtColumns.Rows)
                    {
                        TemplateField ItemTmpField = new TemplateField();
                        // create HeaderTemplate

                        ItemTmpField.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "",
                                                                      dr["ColName"].ToString(), dr["ColControl"].ToString(), dr["DSTempSetColId"].ToString(), dr["ColTypeCd"].ToString(), ContrlEnable, 0);
                        // create ItemTemplate
                        ItemTmpField.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "",
                                                                      dr["ColName"].ToString(), dr["ColControl"].ToString(), dr["DSTempSetColId"].ToString(), dr["ColTypeCd"].ToString(), ContrlEnable, 0);
                        ItemTmpField.HeaderStyle.CssClass = "clr"; ItemTmpField.ItemStyle.CssClass = "clr";
                        if (dr["ColControl"].ToString() == "DropDown")
                            ItemTmpField.HeaderStyle.Width = Unit.Percentage(14);
                        if (dr["ColControl"].ToString() == "Radio")
                            ItemTmpField.HeaderStyle.Width = Unit.Percentage(8);
                        if (dr["ColControl"].ToString() == "Timer")
                            ItemTmpField.HeaderStyle.Width = Unit.Percentage(132);
                        if (dr["ColControl"].ToString() == "Text")
                            ItemTmpField.HeaderStyle.Width = Unit.Percentage(12);
                        if (dr["ColControl"].ToString() == "Freq")
                            ItemTmpField.HeaderStyle.Width = Unit.Percentage(12);
                        // then add to the GridView
                        grdDataSht.Columns.Insert(grdDataSht.Columns.Count - 2, ItemTmpField);
                    }
                    int colmncount = 8 - grdDataSht.Columns.Count;
                    for (int i = 1; i <= colmncount; i++)
                    {
                        // add templated fields to the GridView

                        TemplateField ItemTmpNote = new TemplateField();
                        // create HeaderTemplate
                        ItemTmpNote.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "",
                                                                      "NA", "NA", "", "", true, 0);
                        // create ItemTemplate
                        ItemTmpNote.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "",
                                                                      "", "", "", "", true, 0);
                        ItemTmpNote.HeaderStyle.CssClass = "clr"; ItemTmpNote.ItemStyle.CssClass = "clr";
                        //ItemTmpNote.ItemStyle.BackColor = System.Drawing.Color.FromName("");

                        ItemTmpNote.HeaderStyle.Width = Unit.Percentage(15); ItemTmpNote.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        // then add to the GridView
                        grdDataSht.Columns.Insert(grdDataSht.Columns.Count - 2, ItemTmpNote);
                    }
                    fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);
                    //grdDataSht.DataSource = oDS.dtSteps;
                    //grdDataSht.DataBind();

                }
            oSession = (clsSession)Session["UserSession"];
            clsGeneral.Btn_Write(oSession.LoginId, oSession.SchoolId, out Disable);
            if (Disable == true)
            {
                ImgBtn_Override.Visible = false;
            }
            else
            {
                ImgBtn_Override.Visible = true;
            }

        }

        object SkillType = oData.FetchValue("SELECT SkillType FROM DSTempHdr WHERE DSTempHdrId='" + oTemp.TemplateId + "'");
        if (Convert.ToString(SkillType) == "Discrete" && btnSubmit.Visible == true)
        {
            btnAddTrial.Visible = true;
        }
        else
        {
            btnAddTrial.Visible = false;
        }
        string Maintanance = Convert.ToString(oData.FetchValue("SELECT [DSMode] FROM [dbo].[DSTempHdr] WHERE DSTempHdrId=" + oTemp.TemplateId));
        if (Maintanance == "MAINTENANCE" && btnSubmit.Visible == true)
        {
            ImgBtn_Inactive.Visible = true;
            ImgBtn_Override.Visible = false;
        }
        else if (btnSubmit.Visible == false)
        {
            ImgBtn_Inactive.Visible = false;
            ImgBtn_Override.Visible = false;
            btnPriorSessn.Visible = false;
            btnSubmitAndRepeat1.Visible = false;
            btnSubmitAndRepeat2.Visible = false;
        }
        else
        {
            ImgBtn_Inactive.Visible = false;
        }

        hdnSessionHdr.Value = Convert.ToString(ViewState["StdtSessHdr"]);
    }

    public string getFormulae(string calId)
    {

        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];

        string sqlStr = " SELECT DST.DSTempSetColId,DST.ColName, DC.CalcType,DC.CalcRptLabel,DC.CalcFormula,DST.CalcuData,DC.DSTempSetColCalcId,DT.MultiSetsInd, DT.DSTempHdrId FROM DSTempHdr DT" +
                               " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId  " +
                               " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId  " +
                               " WHERE (DT.DSTempHdrId =" + oTemp.TemplateId + " and DST.DSTempSetColId = " + calId + ")";
        DataTable dt = oData.ReturnDataTable(sqlStr, false);

        foreach (DataRow dr in dt.Rows)
        {
            if (dr["CalcuData"] != null && dr["CalcuData"].ToString() != "")
            {
                return dr["CalcuData"].ToString();
            }
        }

        return "";
    }

    public void calculateFormula()
    {
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oSession = (clsSession)Session["UserSession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oSession != null)
        {
            if (oTemp != null)
            {
                if (oDS != null)
                {
                    /*
                     * Creation and insertion to a new Session  
                     */
                    Dictionary<string, string[]> ht = LoadStepVals_toDict();
                    if (ht != null)
                    {


                        string sqlStr = " SELECT DST.DSTempSetColId,DST.ColName, DC.CalcType,DC.CalcRptLabel,DC.CalcFormula,DST.CalcuData,DC.DSTempSetColCalcId,DT.MultiSetsInd, DT.DSTempHdrId FROM DSTempHdr DT" +
                                " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId  " +
                                " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId  " +
                                " WHERE (DT.DSTempHdrId =" + oTemp.TemplateId + ")";
                        DataTable dt = oData.ReturnDataTable(sqlStr, false);
                        int indexi = 0;
                        int icount = 0;
                        int count = dt.Rows.Count;
                        int[] arColcalId = new int[count];
                        int[] arColId = new int[count];
                        string[] arColName = new string[oDS.dtColumns.Rows.Count];
                        string custom = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            arColcalId[icount] = Convert.ToInt32(dr["DSTempSetColCalcId"]);
                            if (dr["CalcType"].ToString() == "Customize")
                            {
                                custom += dr["CalcuData"].ToString() + "#";
                            }

                            icount++;
                        }
                        int colIndex = 0;
                        foreach (DataRow dr in oDS.dtColumns.Rows)
                        {
                            sqlStr = " select dbo.DSTempSetCol.ColName from DSTempSetCol where DSTempSetCol.DSTempSetColId=" + Convert.ToInt32(dr["DSTempSetColId"].ToString());
                            arColName[colIndex] = oData.FetchValue(sqlStr).ToString();
                            colIndex++;
                        }
                        string names = "";
                        float custResult = 0;
                        string[] sEquation = custom.Split('#');
                        string colmnId = "", result = "";
                        foreach (var item in sEquation)
                        {
                            Calculate.Calculate oCalc = new Calculate.Calculate();
                            if (item != "")
                            {
                                PreProcessedExpression expResult = oCalc.PreProcessExpression(item);
                                int expCount = expResult.ColumnDatas.Length;
                                for (int indexj = 0; indexj < expCount; indexj++)
                                {
                                    names = "";
                                    for (int i = 0; i < oDS.dtColumns.Rows.Count; i++)
                                    {
                                        if (ht.ContainsKey(oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()))
                                        {
                                            if (!names.Contains(arColName[i]))
                                            {
                                                if (arColName[i].ToUpper() == expResult.ColumnDatas[indexj].ColumnName.Trim())
                                                {
                                                    expResult.ColumnDatas[indexj].Data = new float[oDS.dtSteps.Rows.Count - 1];
                                                    names += arColName[i] + ",";
                                                    expResult.ColumnDatas[indexj].Data = parseFloat(ht[oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()]);

                                                    result += oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString() + "*";
                                                }
                                            }
                                        }
                                    }
                                }
                                if (expResult != null)
                                {
                                    string exp = oCalc.CreateExpressionToEvaluate(expResult);
                                    string[] tempString = exp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] postfixArray = oCalc.InfixToPostfix(tempString);
                                    custResult = oCalc.EvaluatePostfix(postfixArray);
                                }
                                result += custResult + "|";
                            }
                        }
                        if (result.Length > 1)
                        {
                            result = result.Substring(0, result.Length - 1);

                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "scoreForText1('" + result + "');", true);

                    }
                }
            }
        }
    }




    public void setTempData(string stdtSessionHdrId)
    {
        oData = new clsData();
        string sqlStr = "select ssh.CurrentSetId,dsts.SortOrder from StdtSessionHdr as ssh LEFT JOIN DSTempSet dsts on ssh.CurrentSetId = dsts.DSTempSetId where ssh.StdtSessionHdrId ='" + stdtSessionHdrId + "'";
        DataTable dt = oData.ReturnDataTable(sqlStr, false);

        if (dt.Rows.Count > 0)
        {
            hdn_currTempSetNmbr.Value = dt.Rows[0]["SortOrder"].ToString();
            hdn_currTempSet.Value = dt.Rows[0]["CurrentSetId"].ToString();
        }

    }

    protected void fillStepGrid(string TeachingProc, string SkillType, string matchToSampleType)
    {
        ClsErrorLog clError = new ClsErrorLog();
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        DataTable dt = new DataTable();
        if ((oTemp != null) && (oDS != null))
            try
            {
                bool updatestatus = false;
                if (ViewState["StdtSessHdr"] != null)
                {

                    //SqlDataReader reader = oData.ReturnDataReader("SELECT StdtSessionStepId FROM StdtSessionStep WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"].ToString(), true);
                    string sqlQry = "SELECT * FROM StdtSessionStep Step INNER JOIN DSTempStep Stp ON Stp.DSTempStepId=Step.DSTempStepId WHERE StdtSessionHdrId="
                        + ViewState["StdtSessHdr"].ToString() + " and ActiveInd='A' AND (Stp.DSTempSetId=" + oDS.CrntSet + ") AND IsDynamic=0 ORDER BY Stp.SortOrder";

                    if (SkillType == "Discrete")
                    {
                        sqlQry = "SELECT * FROM StdtSessionStep WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"].ToString();
                    }
                    DataTable dtstepIDs = new DataTable();
                    dtstepIDs = oData.ReturnDataTable(sqlQry, false);
                    DataTable dtstps = oDS.dtSteps;


                    /// Code Added By Arun. Code Change is for Match-to-Sample teaching procedure automatically yields a “Positional
                    /// Datasheet” (random presentation) so maybe it should indicate that.
                    if (oDS.TeachProc == "Match-to-Sample")
                    {
                        //string sqlStr = "SELECT [DSTempStepId],[StepName] as StepCd,[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                        //                        " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + oDS.CrntSet + " AND ActiveInd='A' ORDER BY [SortOrder]";

                        string sqlStr = "SELECT Step.StdtSessionStepId as SessStepID, TempStep.[DSTempStepId],TempStep.[StepName] as StepCd,TempStep.[StepName],TempStep.SortOrder as StepId  FROM [dbo].[DSTempStep] TempStep  INNER JOIN StdtSessionStep Step " +
                                         " ON TempStep.DSTempStepId=Step.DSTempStepId  WHERE TempStep.DSTempHdrId=" + oTemp.TemplateId + " AND TempStep.DsTempSetId=" + oDS.CrntSet + " AND TempStep.ActiveInd='A' AND IsDynamic=0 AND  Step.StdtSessionHdrId=" + ViewState["StdtSessHdr"].ToString() + " ORDER BY NEWID()";


                        dt = oData.ReturnDataTable(sqlStr, false);

                        string[] distractorSamples = getDistractors(oTemp.TemplateId,oDS.CrntSet);

                        if (ViewState["StdtSessHdr"] != null && ViewState["StdtSessHdr"] != "")
                        {
                            string IsTrial = Convert.ToString(oData.FetchValue("SELECT IsTrial FROM StdtSessionHdr WHERE StdtSessionHdrId='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "'"));
                            if (IsTrial != "")
                            {
                                sqlStr = "SELECT Step.StdtSessionStepId as SessStepID, TempStep.[DSTempStepId],TempStep.[StepName] as StepCd,TempStep.[StepName],TempStep.SortOrder as StepId  FROM [dbo].[DSTempStep] TempStep  INNER JOIN StdtSessionStep Step " +
                                             " ON TempStep.DSTempStepId=Step.DSTempStepId  WHERE TempStep.DSTempStepId IN (" + IsTrial + ") ORDER BY NEWID()";
                                DataTable dtTrialSample = oData.ReturnDataTable(sqlStr, false);

                                foreach (DataRow dr in dtTrialSample.Rows)
                                {
                                    DataRow newTrialRow = dt.NewRow();
                                    newTrialRow["SessStepID"] = dr["SessStepID"];
                                    newTrialRow["DSTempStepId"] = dr["DSTempStepId"];
                                    newTrialRow["StepCd"] = dr["StepCd"];
                                    newTrialRow["StepName"] = dr["StepName"];
                                    newTrialRow["StepId"] = dr["StepId"];
                                    dt.Rows.Add(newTrialRow);
                                }
                            }
                        }
                        oDS.dtSteps = dtstps = dt;
                        int sampleCount = 0;
                        if (dt != null)
                        {
                            clsMathToSamples.Step[] steps = null;

                            if (dt.Rows.Count > 0)
                            {
                                string ansString = dt.Rows[0]["StepName"].ToString();
                                do
                                {
                                ansString = ansString.Replace(", ", ",").ToString();
                                } while (ansString.Contains(", "));
                             //   ansString = ansString.Replace(", ", ",").ToString();
                                string[] ansList = clsMathToSamples.GetAnsList(ansString);
                                string[] Questions = new string[dt.Rows.Count];
                                QuestnAary = new string[dt.Rows.Count];
                                samcnt = new Dictionary<string, string>();


                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string tempString = dt.Rows[i]["StepName"].ToString();
                                    tempString = tempString.Replace(", ", ",").ToString();

                                    if (distractorSamples != null)
                                    {
                                        if (distractorSamples.Length > 0)
                                        {
                                            string matchDistractors = String.Join(",", distractorSamples);
                                            tempString = getNormalSample(distractorSamples, matchDistractors, tempString);
                                        }
                                    }

                                    Questions[i] = clsMathToSamples.GetQuestion(tempString);
                                    QuestnAary[i] = clsMathToSamples.GetQuestion(tempString);
                                }
                                if (matchToSampleType == "Randomized")
                                {
                                    if (ansList.Length > 1)
                                    {
                                        steps = clsMathToSamples.FormStepsWithAnsNew(ansList, Questions.Length, Questions);
                                    }
                                    else
                                    {
                                        steps = clsMathToSamples.FormStepsWithAns(ansList, Questions.Length, Questions);
                                    }
                                }
                                else
                                {
                                    steps = clsMathToSamples.FormStepsInOrderWithAns(ansList, Questions.Length, Questions);
                                }




                            }



                            if (dt.Rows.Count > 0)
                            {
                                if (dtstps != null)
                                {
                                    if (dtstps.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            string stepname = dr["StepName"].ToString();
                                            string[] samplesCollection = stepname.Split('[');
                                            string[] samples = samplesCollection[1].Split(',');
                                            sampleCount = samples.Length;
                                        }
                                        int j = 0;
                                        //if (dtstps.Columns.Count > 4)
                                        // {

                                        //}
                                        //else
                                        //{

                                        for (int i = 0; i < sampleCount; i++)
                                        {
                                            if (!dtstps.Columns.Contains("Samples" + i))
                                                dtstps.Columns.Add("Samples" + i);
                                        }
                                        // }
                                        foreach (DataRow drst in dtstps.Rows)
                                        {

                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                if (i == j)
                                                {
                                                    DataRow dr = dt.Rows[i];
                                                    // do something with dr


                                                    //string stepname = dr["StepName"].ToString();
                                                    string stepname = steps[i].TrialText.Trim();
                                                    string[] samplesCollection = stepname.Split('[');
                                                    string[] samples = samplesCollection[1].Split(',');
                                                    //Keep the same order for qustn and samples
                                                        
                                                    drst["StepCd"] = stepname;
                                                    drst["StepName"] = stepname;


                                                    //need to set value to MyRow column
                                                    // or set it to some other value

                                                    for (int index = 0; index < samples.Length; index++)
                                                    {
                                                        samples[index] = samples[index].Replace("]", "");
                                                        drst["Samples" + index] = samples[index].Trim();


                                                    }
                                                }

                                            }
                                            j++;

                                        }
                                    }
                                }
                            }
                        }

                    }

                    ///Code Ended Here. Arun.

                    if (dtstps != null)
                    {
                        if (!dtstps.Columns.Contains("SessStepID"))
                        {
                            dtstps.Columns.Add("SessStepID", System.Type.GetType("System.String"));
                            int i = 0;
                            if (dtstepIDs.Rows.Count > 0)
                                foreach (DataRow dr in dtstepIDs.Rows)
                                //while (reader.Read())
                                {
                                    if (dtstps.Rows.Count > 0)
                                    {
                                        //dtstps.Rows[i]["SessStepID"] = dr["StdtSessionStepId"].ToString();
                                        dtstps.Rows[i]["SessStepID"] = dr["StdtSessionStepId"].ToString();
                                        i++;
                                    }
                                }
                            //reader.Close();
                        }

                        // bind and display the data

                        grdDataSht.DataSource = null;
                        grdDataSht.DataBind();
                        grdDataSht.DataSource = oDS.dtSteps;
                        grdDataSht.DataBind();
                    }
                    string selPrmpt = "";
                    if ((oDS.PromptProc == "Least-to-Most")||(oDS.PromptProc == "Graduated Guidance"))
                    {
                        selPrmpt = "SELECT LU.LookupId as Id,LU.LookupName as Name FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                                                " DS.ActiveInd='A' AND DS.DSTempHdrId=" + oTemp.TemplateId + " ORDER BY SortOrder DESC";
                    }
                    else
                    {
                        selPrmpt = "SELECT LU.LookupId as Id,LU.LookupName as Name FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                                    " DS.ActiveInd='A' AND DS.DSTempHdrId=" + oTemp.TemplateId + " ORDER BY SortOrder ";
                    }
                    DataTable dtPrompts = new DataTable();
                    dtPrompts = oData.ReturnDataTable(selPrmpt, false);
                    if (dtPrompts != null)
                    {

                        foreach (DataRow dr in dtPrompts.Rows)
                        {
                            lblPromtProc.Text += "<br/>" + dr["Name"].ToString();
                        }
                        foreach (GridViewRow gr in grdDataSht.Rows)
                        {
                            if (gr.RowType == DataControlRowType.DataRow)
                            {
                                if (oDS.dtColumns != null)
                                    foreach (DataRow drcol in oDS.dtColumns.Rows)
                                    {
                                        if (drcol["ColTypeCd"].ToString() == "Prompt")
                                        {
                                            DropDownList ddlPrompt = (DropDownList)gr.FindControl("ddlPrompt_" + drcol["DSTempSetColId"].ToString());
                                            ddlPrompt.DataSource = dtPrompts;
                                            ddlPrompt.DataTextField = "Name";
                                            ddlPrompt.DataValueField = "Id";
                                            ddlPrompt.DataBind();
                                            ddlPrompt.Items.Insert(0, new ListItem("--- Select ---", "0"));
                                        }
                                    }
                            }
                        }
                    }

                    object IOAStat = oData.FetchValue("SELECT IOAInd FROM StdtSessionHdr WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"]);
                    if (Convert.ToString(IOAStat) == "Y")
                    {
                        btnSubmitAndRepeat1.Visible = false;
                        btnSubmitAndRepeat2.Visible = false;
                    }
                    else
                    {
                        if (Session["DupNewTemplateId"] != null)
                        {
                            btnSubmitAndRepeat1.Visible = false;
                            btnSubmitAndRepeat2.Visible = false;
                        }
                        else
                        {
                            btnSubmitAndRepeat1.Visible = true;
                            btnSubmitAndRepeat2.Visible = true;
                        }
                        Session["DupNewTemplateId"] = null;
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "", "temp();", true);

                }
            }
            catch (Exception ex)
            {
                //LIJU:  Do the Rollback abd Connection close only in the Parent where its declared in a Try/Catch
                //oData.RollBackTransation(trans, con);
                //con.Close();
                clError.WriteToLog(ex.ToString());
                throw ex;
            }
    }
    //protected void fillSteps(int TempHdrId, int NbrOfTrials, string SkillTyp, int SetId, string TeachingProc, int VTLessonId, string ChainType)
    //{
    //    oData = new clsData();
    //    oTemp = (ClsTemplateSession)Session["BiweeklySession"];
    //    oDS = (clsDataSheet)Session[DatasheetKey];
    //    DataTable dt = new DataTable();
    //    if (oTemp != null)
    //    {
    //        if (oDS != null)
    //        {

    //            if (SkillTyp == "Discrete")
    //            {
    //                if (TeachingProc == "Match-to-Sample")
    //                {
    //                    string sqlStr = "SELECT [DSTempStepId],[StepName] as StepCd,[StepName]  FROM [dbo].[DSTempStep] " +
    //                                    " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + SetId + " AND ActiveInd='A' ORDER BY [SortOrder]";
    //                    dt = oData.ReturnDataTable(sqlStr, false);
    //                }
    //                else if (VTLessonId > 0)
    //                {
    //                    string sqlStr = "SELECT [DSTempStepId],[StepCd],[StepName]  FROM [dbo].[DSTempStep] " +
    //                                    " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + SetId + " AND ActiveInd='A' ORDER BY [SortOrder]";
    //                    dt = oData.ReturnDataTable(sqlStr, false);
    //                }
    //                else
    //                {
    //                    dt.Columns.Add("DSTempStepId", System.Type.GetType("System.String"));
    //                    dt.Columns.Add("StepCd", System.Type.GetType("System.String"));
    //                    for (int i = 1; i <= NbrOfTrials; i++)
    //                    {
    //                        DataRow dr = dt.NewRow();
    //                        dr["DSTempStepId"] = "0";
    //                        dr["StepCd"] = i.ToString();
    //                        dt.Rows.Add(dr);
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                string sqlStr = "SELECT [DSTempStepId],[StepCd],[StepName]  FROM [dbo].[DSTempStep] " +
    //                " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND (DsTempSetId=0 OR DsTempSetId=" + SetId + ") AND ActiveInd='A' ORDER BY [SortOrder]";
    //                if (ChainType == "Backward chain")
    //                {
    //                    sqlStr = "SELECT [DSTempStepId],[StepCd],[StepName]  FROM [dbo].[DSTempStep] " +
    //                             "WHERE DSTempHdrId=" + oTemp.TemplateId + " AND (DsTempSetId=0 OR DsTempSetId=" + SetId + ") AND ActiveInd='A' ORDER BY [SortOrder] DESC";
    //                }

    //                dt = oData.ReturnDataTable(sqlStr, false);
    //            }
    //            oDS.dtSteps = dt;
    //            Session[DatasheetKey] = oDS;
    //        }
    //    }
    //}


    protected void fillSteps(int TempHdrId, int NbrOfTrials, string SkillTyp, int SetId, string TeachingProc, int VTLessonId, string ChainType, string totalTaskType, string matchToSampleType)
    {
        //TeachingProc = getTeachingMethod(TeachingProc);

        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        DataTable dt = new DataTable();
        DataTable dtUpdated = new DataTable();

        if (oTemp != null)
        {
            if (oDS != null)
            {

                if (SkillTyp == "Discrete")
                {
                    if (TeachingProc == "Match-to-Sample")
                    {
                        //if(ViewState["StdtSessHdr"] != null)
                        {
                            string sqlStr = "SELECT [DSTempStepId],[StepName] as StepCd,[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                                            " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + SetId + " AND ActiveInd='A' AND IsDynamic=0 ORDER BY [SortOrder]";
                            // string sqlStr = "SELECT Step.StdtSessionStepId as SessStepID, TempStep.[DSTempStepId],TempStep.[StepName] as StepCd,TempStep.[StepName],TempStep.SortOrder as StepId  FROM [dbo].[DSTempStep] TempStep  INNER JOIN StdtSessionStep Step " +
                            //                 " ON TempStep.DSTempStepId=Step.DSTempStepId  WHERE TempStep.DSTempHdrId=" + oTemp.TemplateId + " AND TempStep.DsTempSetId=" + SetId + " AND TempStep.ActiveInd='A' AND  Step.StdtSessionHdrId=" + ViewState["StdtSessHdr"].ToString() + " ORDER BY NEWID()";



                            dt = oData.ReturnDataTable(sqlStr, false);
                        }

                        string[] distractorSamples = getDistractors(oTemp.TemplateId, oDS.CrntSet);

                        if (ViewState["StdtSessHdr"] != null && ViewState["StdtSessHdr"] != "")
                        {
                            string IsTrial = Convert.ToString(oData.FetchValue("SELECT IsTrial FROM StdtSessionHdr WHERE StdtSessionHdrId='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "'"));
                            if (IsTrial != "")
                            {
                                string sqlStr = "SELECT Step.StdtSessionStepId as SessStepID, TempStep.[DSTempStepId],TempStep.[StepName] as StepCd,TempStep.[StepName],TempStep.SortOrder as StepId  FROM [dbo].[DSTempStep] TempStep  INNER JOIN StdtSessionStep Step " +
                                             " ON TempStep.DSTempStepId=Step.DSTempStepId  WHERE TempStep.DSTempStepId IN (" + IsTrial + ") ORDER BY NEWID()";
                                DataTable dtTrialSample = oData.ReturnDataTable(sqlStr, false);

                                foreach (DataRow dr in dtTrialSample.Rows)
                                {
                                    DataRow newTrialRow = dt.NewRow();
                                    newTrialRow["DSTempStepId"] = dr["DSTempStepId"];
                                    newTrialRow["StepCd"] = dr["StepCd"];
                                    newTrialRow["StepName"] = dr["StepName"];
                                    newTrialRow["StepId"] = dr["StepId"];
                                    dt.Rows.Add(newTrialRow);
                                }
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            string ansString = dt.Rows[0]["StepName"].ToString();
                            do
                            {
                                ansString = ansString.Replace(", ", ",").ToString();
                            } while (ansString.Contains(", "));
                           // ansString = ansString.Replace(", ", ",").ToString(); // Space Removal May 5 2020
                            string[] ansList = clsMathToSamples.GetAnsList(ansString);
                            string[] Questions = new string[dt.Rows.Count];
                            QuestnAary = new string[dt.Rows.Count];
                            samcnt = new Dictionary<string, string>();
                            clsMathToSamples.Step[] steps = null;

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string tempString = dt.Rows[i]["StepName"].ToString();
                                tempString = tempString.Replace(", ", ",").ToString(); // Space Removal May 5 2020

                                if (distractorSamples != null)
                                {
                                    if (distractorSamples.Length > 0)
                                    {
                                        string matchDistractors = String.Join(",", distractorSamples);
                                        tempString = getNormalSample(distractorSamples, matchDistractors, tempString);
                                    }
                                }

                                Questions[i] = clsMathToSamples.GetQuestion(tempString);
                                QuestnAary[i] = clsMathToSamples.GetQuestion(tempString);
                            }
                            if (matchToSampleType == "Randomized")
                            {
                                if (ansList.Length > 1)
                                {
                                    steps = clsMathToSamples.FormStepsWithAnsNew(ansList, Questions.Length, Questions);
                                }
                                else
                                {
                                    steps = clsMathToSamples.FormStepsWithAns(ansList, Questions.Length, Questions);
                                }
                            }
                            else
                            {
                                steps = clsMathToSamples.FormStepsInOrderWithAns(ansList, Questions.Length, Questions);
                            }

                            dtUpdated = dt.Clone();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                foreach (DataColumn col in dt.Columns)
                                {
                                    if (col.ColumnName == "StepCd")
                                    {

                                        dt.Rows[i][col] = steps[i].Questions;
                                    }
                                    if (col.ColumnName == "StepName")
                                    {
                                        dt.Rows[i][col] = steps[i].Questions;
                                    }
                                }
                                dtUpdated.ImportRow(dt.Rows[i]);
                            }
                            oDS.dtSteps = dtUpdated;
                            Session[DatasheetKey] = oDS;
                            return;
                        }
                    }
                    else if (VTLessonId > 0)
                    {
                        string sqlStr = "SELECT [DSTempStepId],[StepCd],[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                                        " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + SetId + " AND ActiveInd='A' AND IsDynamic=0 ORDER BY [SortOrder]";
                        dt = oData.ReturnDataTable(sqlStr, false);
                    }
                    else
                    {
                        int TrialNo = Convert.ToInt32(oData.FetchValue("SELECT COUNT(*) FROM StdtSessionStep WHERE StdtSessionHdrId='" + ViewState["StdtSessHdr"] + "'"));
                        if (TrialNo == 0)
                        {
                            TrialNo = Convert.ToInt32(oData.FetchValue("SELECT  NbrOfTrials FROM DSTempHdr WHERE DSTempHdrId='" + oTemp.TemplateId + "'"));
                        }

                        dt.Columns.Add("DSTempStepId", System.Type.GetType("System.String"));
                        dt.Columns.Add("StepCd", System.Type.GetType("System.String"));
                        dt.Columns.Add("StepId", System.Type.GetType("System.String"));

                        for (int i = 1; i <= TrialNo; i++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DSTempStepId"] = "0";
                            dr["StepCd"] = i.ToString();
                            dr["StepId"] = "0";
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    string sqlStr = "SELECT [DSTempStepId],[StepCd]+' - '+[StepName] as StepCd,[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                    " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND  DsTempSetId=" + SetId + " AND ActiveInd='A' AND IsDynamic=0 ORDER BY [SortOrder]";
                    if (TeachingProc == "Total Task")
                    {
                        if (totalTaskType == "Randomized")
                        {
                            if (Convert.ToBoolean(ViewState["IsHistory"]) != true)
                            {

                                sqlStr = "SELECT [DSTempStepId],[StepCd]+' - '+[StepName] as StepCd,[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                            " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + SetId + " AND ActiveInd='A'  AND IsDynamic=0 ORDER BY NEWID()";
                            }
                            else
                                sqlStr = "SELECT [DSTempStepId],[StepCd]+' - '+[StepName] as StepCd,[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                                " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND  DsTempSetId=" + SetId + "  AND ActiveInd='A'  AND IsDynamic=0 ORDER BY SortOrder";
                        }
                    }

                    if (ChainType == "Backward chain")
                    {
                        sqlStr = "SELECT [DSTempStepId],[StepCd]+' - '+[StepName] as StepCd,[StepName],RANK() OVER(ORDER BY SortOrder DESC) as StepId  FROM [dbo].[DSTempStep] " +
                                 "WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + SetId + " AND ActiveInd='A'  AND IsDynamic=0 ORDER BY [SortOrder] ";

                    }

                    dt = oData.ReturnDataTable(sqlStr, false);
                }
                dtUpdated = dt.Clone();

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                if (col.ColumnName == "StepCd")
                                {

                                    dr[col] = dr[col].ToString().Replace("&qt;", "'");
                                    dr[col] = dr[col].ToString().Replace("?s", "'s");
                                    dr[col] = dr[col].ToString().Replace("&lqt", "‘");
                                    dr[col] = dr[col].ToString().Replace("?", "");
                                    dr[col] = dr[col].ToString().Replace("&rqt", "’");
                                }
                                if (col.ColumnName == "StepName")
                                {
                                    dr[col] = dr[col].ToString().Replace("&qt;", "'");
                                    dr[col] = dr[col].ToString().Replace("?s", "'s");
                                    dr[col] = dr[col].ToString().Replace("&lqt", "‘");
                                    dr[col] = dr[col].ToString().Replace("?", "");
                                    dr[col] = dr[col].ToString().Replace("&rqt", "’");
                                }
                            }
                            dtUpdated.ImportRow(dr);
                        }

                    }
                }
                oDS.dtSteps = dtUpdated;
                Session[DatasheetKey] = oDS;
            }
        }
    }

    private string getTeachingMethod(string TeachingProc)
    {
        string sqlStr = "SELECT LookupDesc FROM LookUp WHERE LookupName = '" + TeachingProc + "'";
        DataTable dt = oData.ReturnDataTable(sqlStr, false);

        return dt.Rows[0]["LookupDesc"].ToString();
    }

    protected void checkStat(object sender, EventArgs e)
    {
        SqlConnection con = null;
        SqlTransaction trans = null;
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oSession = (clsSession)Session["UserSession"];
        oDS = new clsDataSheet();



        if (Session["NewTemplateId"] != null)
        {
            oTemp.TemplateId = Convert.ToInt32(Session["NewTemplateId"]);

            btnSubmit1.Visible = false;
            btnSave1.Visible = false;
            btnPriorSessn.Visible = false;
            btnSave.Visible = false;
            btnSubmit.Visible = false;
            btnProbe.Visible = false;
            btnSubmitAndRepeat1.Visible = false;
            btnSubmitAndRepeat2.Visible = false;
            ImgBtn_Override.Visible = false;
            ImgBtn_Inactive.Visible = false;
            // Session["NewTemplateId"] = null;
        }
        int hasPrevSet = fillTempOverride(oTemp.TemplateId);
        Session[DatasheetKey] = oDS;
        try
        {
            if ((oTemp != null) && (oSession != null))
            {
                oData = new clsData();
                string sel = "";

                if (Session["NewTemplateId"] != null)
                {

                    //selPrev = "SELECT * FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND StdtClassId=" + oSession.Classid + " AND DSTempHdrId=" + oTemp.TemplateId + " AND SessionStatusCd='P' AND IsMaintanace ='False'";
                    //DataTable dtHdrsPrev = oData.ReturnDataTable(sel, false);
                    oData = new clsData();
                    oData.ReturnDropDown("SELECT UserId as Id,UserLName+' '+UserFName AS Name FROM [dbo].[User] WHERE ActiveInd='A'", ddlIOAUsers);
                    if (ddlIOAUsers.Items.Count > 0)
                    {
                        ddlIOAUsers.Items[0].Text = "---- Select User ----";

                        string strQuery = "SELECT UserId as Id,UserLName+' '+UserFName AS Name FROM [dbo].[User] WHERE ActiveInd='A' AND UserId=" + oSession.LoginId;
                        DataTable dtSess = oData.ReturnDataTable(strQuery, false);
                        ddlIOAUsers.SelectedValue = dtSess.Rows[0]["Id"].ToString();
                        ddlIOAUsers.Enabled = false;

                    }
                    //Create new Draft for teacher..

                    hdn_isMaintainance.Value = "false";
                    if ((Request.QueryString["exc"] != null && Request.QueryString["exc"] == "true") || (hasPrevSet == 0))
                    {
                        con = oData.Open();
                        trans = con.BeginTransaction();
                        generateSheet(true);
                        bool reslt = SaveDraft("P", "N", "insert", con, trans);
                        ImgBtn_Inactive.Visible = false;

                        if (reslt)
                        {
                            bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                            if (reslt2) oData.CommitTransation(trans, con);
                            else if (trans.Connection.State == ConnectionState.Open) oData.RollBackTransation(trans, con);
                            con.Close();
                            getStepPrompts();
                            fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);


                        }
                        else if (trans.Connection.State == ConnectionState.Open)
                        {
                            oData.RollBackTransation(trans, con);
                            con.Close();
                        }

                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "showTempOverride();", true);

                        //if submit and repeat, trigger continue button

                        //---
                        if (Request.QueryString["SRMode"] != null && Request.QueryString["SRMode"] == "true")
                        {
                            if (Request.QueryString["isMaint"] != null && Request.QueryString["isMaint"] == "true")
                            {
                                foreach (RepeaterItem rItem in rptr_tempOverride.Items)
                                {
                                    CheckBox chkTempOverride = (CheckBox)rItem.FindControl("tempOverrideCheckBox");
                                    HiddenField hdn_tempSetId = (HiddenField)rItem.FindControl("hdn_tempSetId");
                                    chkTempOverride.Checked = false;
                                    string currSetIdTemp = Request.QueryString["currSetIdTemp"];
                                    if (hdn_tempSetId.Value == currSetIdTemp)
                                    {
                                        chkTempOverride.Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                int iCrntSet = 0;
                                string sqlStr = "select NextSetId from StdtDSStat where DSTempHdrId = " + oTemp.TemplateId;
                                DataTable dt = oData.ReturnDataTable(sqlStr, false);
                                if (dt != null)
                                {
                                    iCrntSet = Convert.ToInt32(dt.Rows[0]["NextSetId"]);
                                }
                                foreach (RepeaterItem rItem in rptr_tempOverride.Items)
                                {
                                    CheckBox chkTempOverride = (CheckBox)rItem.FindControl("tempOverrideCheckBox");
                                    HiddenField hdn_tempSetId = (HiddenField)rItem.FindControl("hdn_tempSetId");
                                    chkTempOverride.Checked = false;
                                    string currSetIdTemp = Request.QueryString["currSetIdTemp"];
                                    if (hdn_tempSetId.Value == iCrntSet.ToString())
                                    {
                                        chkTempOverride.Checked = true;
                                    }
                                }
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showTempOverrideCheck", "showTempOverrideCheck();", true);
                        }
                        //---

                        //if hashtable has set, load it

                        //???
                        Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
                        if (htLpList != null)
                        {
                            string lpId = oTemp.TemplateId.ToString();
                            LinkedList<KeyValuePair<string, Hashtable>> llKvpSetList = (LinkedList<KeyValuePair<string, Hashtable>>)htLpList[lpId];
                            if (llKvpSetList != null)
                            {
                                if (llKvpSetList.Count > 0)
                                {
                                    foreach (KeyValuePair<string, Hashtable> abc in llKvpSetList)
                                    {
                                        string checkedId = abc.Key;
                                        foreach (RepeaterItem rItem in rptr_tempOverride.Items)
                                        {
                                            CheckBox chkTempOverride = (CheckBox)rItem.FindControl("tempOverrideCheckBox");
                                            HiddenField hdn_tempSetId = (HiddenField)rItem.FindControl("hdn_tempSetId");
                                            if (checkedId == hdn_tempSetId.Value)
                                            {
                                                chkTempOverride.Checked = true;
                                            }
                                        }
                                    }
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showTempOverrideCheck", "showTempOverrideCheck();", true);
                                }
                            }
                            //???
                        }
                    }

                }
                else
                {
                    if ((Request.QueryString["exc"] != null && Request.QueryString["exc"] == "true"))
                    {
                        //
                        sel = "SELECT * FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND StdtClassId=" + oSession.Classid + " AND DSTempHdrId=" + oTemp.TemplateId + " AND SessionStatusCd='D' AND IsMaintanace ='False'";
                        object objSessNbr = oData.FetchValue("SELECT ISNULL(MAX(SessionNbr),0)+1 FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId +/* " AND StdtClassId=" + oSession.Classid +*/ " AND LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId + ")");
                        if (objSessNbr == null)
                            objSessNbr = 0;
                        oDS.SessNbr = (int)objSessNbr;
                        DataTable dtHdrs = oData.ReturnDataTable(sel, false);
                        oData.ReturnDropDown("SELECT UserId as Id,UserLName+' '+UserFName AS Name FROM [dbo].[User] WHERE ActiveInd='A'", ddlIOAUsers);
                        if (ddlIOAUsers.Items.Count > 0)
                        {
                            ddlIOAUsers.Items[0].Text = "---- Select User ----";
                            string strQuery = "SELECT UserId as Id,UserLName+' '+UserFName AS Name FROM [dbo].[User] WHERE ActiveInd='A' AND UserId=" + oSession.LoginId;
                            DataTable dtSess = oData.ReturnDataTable(strQuery, false);
                            ddlIOAUsers.SelectedValue = dtSess.Rows[0]["Id"].ToString();
                            ddlIOAUsers.Enabled = false;
                        }
                        if (dtHdrs != null)
                        {
                            if (dtHdrs.Rows.Count == 0)
                            {
                                //Create new Draft for teacher..
                                con = oData.Open();
                                //Preview template Exists
                                if (oDS.SessNbr > 0)
                                {
                                    sel = "SELECT * FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND StdtClassId=" + oSession.Classid + " AND DSTempHdrId=" + oTemp.TemplateId + " AND SessionStatusCd='P' AND SessionNbr=0 ";
                                    DataTable dtPreview = oData.ReturnDataTable(sel, false);
                                    if (dtPreview != null)
                                    {
                                        if (dtPreview.Rows.Count > 0)
                                        {
                                            ViewState["StdtSessHdr"] = dtPreview.Rows[0]["StdtSessionHdrId"].ToString();
                                        }
                                        //-----
                                        if (ViewState["StdtSessHdr"] != null)
                                        {
                                            string updQry = "DELETE FROM StdtSessionHdr WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"];
                                            int retrn = oData.Execute(updQry);
                                            string selqry = "SELECT StdtDSStatId FROM StdtDSStat WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND DSTempHdrId=" + oTemp.TemplateId + "";
                                            DataTable dtDSStat = oData.ReturnDataTable(selqry, false);
                                            if (dtDSStat.Rows.Count > 0)
                                            {
                                                string dltQry = "DELETE FROM StdtDSStat WHERE DSTempHdrId=" + oTemp.TemplateId + " ";
                                                int dtlRetrn = oData.Execute(dltQry);
                                            }
                                        }
                                    }
                                }
                                trans = con.BeginTransaction();
                                generateSheet(true);
                                bool reslt = SaveDraft("P", "N", "insert", con, trans);
                                if (reslt)
                                {
                                    bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                                    if (reslt2) oData.CommitTransation(trans, con);
                                    else if (trans.Connection.State == ConnectionState.Open) oData.RollBackTransation(trans, con);
                                    con.Close();
                                    getStepPrompts();
                                    fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);
                                }
                                else if (trans.Connection.State == ConnectionState.Open)
                                {
                                    oData.RollBackTransation(trans, con);
                                    con.Close();
                                }
                            }
                            if (dtHdrs.Rows.Count == 2)
                            {
                                foreach (DataRow dr in dtHdrs.Rows)
                                {
                                    DataTable dtUsr = new DataTable();
                                    if (dr["IOAInd"].ToString() == "N")
                                    {
                                        string selQry = "SELECT UserLName+' '+UserFName AS Name,Hdr.CreatedOn,SessionNbr FROM StdtSessionHdr Hdr INNER JOIN [User] Usr ON Usr.UserId=Hdr.CreatedBy " +
                                            "WHERE StdtSessionHdrId=" + dr["StdtSessionHdrId"].ToString();
                                        dtUsr = oData.ReturnDataTable(selQry, false);
                                        if ((dtUsr != null) && (dtUsr.Rows.Count > 0))
                                        {
                                            hfSessIDNorm.Value = dr["StdtSessionHdrId"].ToString();
                                            lblNormalUsr.InnerHtml = dtUsr.Rows[0]["Name"].ToString();
                                            lblNormalStime.InnerHtml = dtUsr.Rows[0]["CreatedOn"].ToString();
                                            lblSessNo.InnerHtml = dtUsr.Rows[0]["SessionNbr"].ToString();
                                        }
                                    }
                                    if (dr["IOAInd"].ToString() == "Y")
                                    {
                                        string selQry = "SELECT UserLName+' '+UserFName AS Name,Hdr.CreatedOn,SessionNbr,IOASessionHdrId FROM StdtSessionHdr Hdr INNER JOIN [User] Usr ON Usr.UserId=Hdr.IOAUserId " +
                                            "WHERE StdtSessionHdrId=" + dr["StdtSessionHdrId"].ToString();
                                        dtUsr = oData.ReturnDataTable(selQry, false);
                                        if ((dtUsr != null) && (dtUsr.Rows.Count > 0))
                                        {
                                            hfSessIDIOA.Value = dr["StdtSessionHdrId"].ToString();
                                            lblIOAUsr.InnerHtml = dtUsr.Rows[0]["Name"].ToString();
                                            lblIOAStime.InnerHtml = dtUsr.Rows[0]["CreatedOn"].ToString();
                                            lblIOASessNo.InnerHtml = dtUsr.Rows[0]["SessionNbr"].ToString();
                                            // oDS.IOASessHdr = Convert.ToInt32(dtUsr.Rows[0]["IOASessionHdrId"].ToString());
                                        }
                                    }
                                }


                                //Open ioa or teacher's draft...
                                if (Request.QueryString["IOA_Status"] == null)
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "showPop(1);", true);
                                }
                                else
                                {
                                    string IOA_Stat = Convert.ToString(Request.QueryString["IOA_Status"]);
                                    if (IOA_Stat == "'N'")
                                    {
                                        btnNormal_Click(sender, e);
                                    }
                                    else if (IOA_Stat == "'Y'")
                                    {
                                        btnIOA_Click(sender, e);
                                    }
                                }

                            }
                            if (dtHdrs.Rows.Count == 1)
                            {
                                if (dtHdrs.Rows[0]["IOAInd"].ToString() == "Y")
                                {
                                    if (dtHdrs.Rows[0]["IOAInd"].ToString() == "Y")
                                    {
                                        //open ioa's draft...
                                        ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                                        oDS.IOAInd = "Y";
                                        oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["IOASessionHdrId"].ToString());
                                        LoadData(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), true);
                                    }
                                }
                                else
                                {   //ask whether to create an ioa draft or open existing draft...
                                    ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                                    oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["StdtSessionHdrId"].ToString());
                                    if (Request.QueryString["AddTrial"] == null)
                                    {
                                        ClientScript.RegisterStartupScript(this.GetType(), "", "showPop(2);", true);
                                    }
                                    else
                                    {
                                        btnNoIOA_Click(sender, e);
                                    }
                                    string selQry = "SELECT UserLName+' '+UserFName AS Name,Hdr.CreatedOn,SessionNbr FROM StdtSessionHdr Hdr INNER JOIN [User] Usr ON Usr.UserId=Hdr.CreatedBy " +
                                            "WHERE StdtSessionHdrId=" + dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                                    DataTable dtUsr = oData.ReturnDataTable(selQry, false);
                                    if ((dtUsr != null) && (dtUsr.Rows.Count > 0))
                                    {
                                        lblUName1.InnerHtml = dtUsr.Rows[0]["Name"].ToString();
                                        LblStrtTime1.InnerHtml = dtUsr.Rows[0]["CreatedOn"].ToString();
                                        lblSessNo1.InnerHtml = dtUsr.Rows[0]["SessionNbr"].ToString();
                                    }
                                }
                            }
                        }
                        //
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "showTempOverride();", true);

                        //if submit and repeat, trigger continue button

                        //---
                        if (Request.QueryString["SRMode"] != null && Request.QueryString["SRMode"] == "true")
                        {
                            if (Request.QueryString["isMaint"] != null && Request.QueryString["isMaint"] == "true")
                            {
                                foreach (RepeaterItem rItem in rptr_tempOverride.Items)
                                {
                                    CheckBox chkTempOverride = (CheckBox)rItem.FindControl("tempOverrideCheckBox");
                                    HiddenField hdn_tempSetId = (HiddenField)rItem.FindControl("hdn_tempSetId");
                                    chkTempOverride.Checked = false;
                                    string currSetIdTemp = Request.QueryString["currSetIdTemp"];
                                    if (hdn_tempSetId.Value == currSetIdTemp)
                                    {
                                        chkTempOverride.Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                int iCrntSet = 0;
                                string sqlStr = "select NextSetId from StdtDSStat where DSTempHdrId = " + oTemp.TemplateId;
                                DataTable dt = oData.ReturnDataTable(sqlStr, false);
                                if (dt != null)
                                {
                                    iCrntSet = Convert.ToInt32(dt.Rows[0]["NextSetId"]);
                                }
                                foreach (RepeaterItem rItem in rptr_tempOverride.Items)
                                {
                                    CheckBox chkTempOverride = (CheckBox)rItem.FindControl("tempOverrideCheckBox");
                                    HiddenField hdn_tempSetId = (HiddenField)rItem.FindControl("hdn_tempSetId");
                                    chkTempOverride.Checked = false;
                                    string currSetIdTemp = Request.QueryString["currSetIdTemp"];
                                    if (hdn_tempSetId.Value == iCrntSet.ToString())
                                    {
                                        chkTempOverride.Checked = true;
                                    }
                                }
                            }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "showTempOverrideCheck", "showTempOverrideCheck();", true);
                        }
                        //---

                        //if hashtable has set, load it

                        //???
                        Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
                        if (htLpList != null)
                        {
                            string lpId = oTemp.TemplateId.ToString();
                            LinkedList<KeyValuePair<string, Hashtable>> llKvpSetList = (LinkedList<KeyValuePair<string, Hashtable>>)htLpList[lpId];
                            if (llKvpSetList != null)
                            {
                                if (llKvpSetList.Count > 0)
                                {
                                    foreach (KeyValuePair<string, Hashtable> abc in llKvpSetList)
                                    {
                                        string checkedId = abc.Key;
                                        foreach (RepeaterItem rItem in rptr_tempOverride.Items)
                                        {
                                            CheckBox chkTempOverride = (CheckBox)rItem.FindControl("tempOverrideCheckBox");
                                            HiddenField hdn_tempSetId = (HiddenField)rItem.FindControl("hdn_tempSetId");
                                            if (checkedId == hdn_tempSetId.Value)
                                            {
                                                chkTempOverride.Checked = true;
                                            }
                                        }
                                    }
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showTempOverrideCheck", "showTempOverrideCheck();", true);
                                }
                            }
                            //???
                        }
                    }
                    // Session["NewTemplateId"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            if (trans != null && trans.Connection != null && trans.Connection.State == ConnectionState.Open)
            {
                oData.RollBackTransation(trans, con);

            }
            if (con != null)
                con.Close();
            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(ex.ToString());
            throw ex;
        }
    }
    protected void getStepPrompts()
    {
        ClsErrorLog clError = new ClsErrorLog();
        oDS = (clsDataSheet)Session[DatasheetKey];

        if (oDS != null)
        {
            try
            {
                if (oDS.ChainType == "Total Task")
                {
                    hfcrntPrompt.Value = "";

                    if (oDS.dtColumns != null)
                        foreach (DataRow drCol in oDS.dtColumns.Rows)
                        {
                            if (drCol["ColTypeCd"].ToString() == "Prompt")
                            {
                                hfcrntPrompt.Value += drCol["DSTempSetColId"].ToString() + "*";
                                string selStepPrompts = "SELECT PromptId FROM StdtDSStepStat StpStat INNER JOIN StdtSessionStep Step ON Step.DSTempStepId=StpStat.DSTempStepId " +
                                    "WHERE DSTempSetColId=" + drCol["DSTempSetColId"].ToString() + " AND StdtSessionHdrId=" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString());
                                DataTable dtStepPrompts = new DataTable();
                                dtStepPrompts = oData.ReturnDataTable(selStepPrompts, false);
                                foreach (DataRow drStepPrmpt in dtStepPrompts.Rows)
                                {
                                    hfcrntPrompt.Value += drStepPrmpt["PromptId"].ToString() + ",";
                                }
                                hfcrntPrompt.Value += "|";
                            }
                        }
                    //if (hfcrntPrompt.Value.Length > 0)
                    //    hfcrntPrompt.Value = hfcrntPrompt.Value.Substring(0, (hfcrntPrompt.Value.Length - 1));
                    generateStepPrompts();
                }
                else
                {
                    hfcrntPrompt.Value = "";
                    if (oDS.dtColumns != null)
                        foreach (DataRow drCol in oDS.dtColumns.Rows)
                        {
                            if (drCol["ColTypeCd"].ToString() == "Prompt")
                            {
                                hfcrntPrompt.Value += drCol["DSTempSetColId"].ToString() + "*";
                                string selStepPrompts = "SELECT StdtSessionStepId FROM StdtSessionStep WHERE StdtSessionHdrId=" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString());
                                DataTable dtStepPrompts = new DataTable();
                                dtStepPrompts = oData.ReturnDataTable(selStepPrompts, false);
                                for (int i = 0; i < dtStepPrompts.Rows.Count; i++)
                                {
                                    hfcrntPrompt.Value += oDS.CrntPrompt + ",";
                                }
                                hfcrntPrompt.Value += "|";
                            }
                        }
                    //if (hfcrntPrompt.Value.Length > 0)
                    //    hfcrntPrompt.Value = hfcrntPrompt.Value.Substring(0, (hfcrntPrompt.Value.Length - 1));
                }
            }
            catch (Exception ex)
            {
                // valid = false;
                //LIJU:  Do the Rollback abd Connection close only in the Parent where its declared in a Try/Catch
                //oData.RollBackTransation(tran, con);
                //con.Close();
                clError.WriteToLog(ex.ToString());
                throw ex;
            }
        }

    }

    protected void generateStepPrompts()
    {
        oDS = (clsDataSheet)Session[DatasheetKey];
        string selSteps = "SELECT DSS.StepCd,Step.DSTempStepId FROM StdtSessionStep Step INNER JOIN DSTempStep DSS ON DSS.DSTempStepId=Step.DSTempStepId WHERE StdtSessionHdrId=" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "  AND IsDynamic=0 order by SortOrder";
        DataTable dtSteps = new DataTable();
        dtSteps = oData.ReturnDataTable(selSteps, false);
        string table = "<table>";
        for (int i = 0; i < dtSteps.Rows.Count + 1; i++)
        {
            if (i == 0)
            {
                table += "<tr>";
                if (oDS != null)
                {
                    table += "<td><b>Step Name</b></td>";
                    for (int j = 0; j < oDS.dtColumns.Rows.Count; j++)
                    {
                        if (oDS.dtColumns.Rows[j]["ColTypeCd"].ToString() == "Prompt")
                            table += "<td><b>" + oDS.dtColumns.Rows[j]["ColName"].ToString() + "</b></td>";
                    }
                }
                table += "</tr>";
            }
            else
            {
                table += "<tr>";
                if (oDS != null)
                {
                    table += "<td>" + dtSteps.Rows[i - 1]["StepCd"].ToString() + "</td>";
                    for (int j = 0; j < oDS.dtColumns.Rows.Count; j++)
                    {
                        if (oDS.dtColumns.Rows[j]["ColTypeCd"].ToString() == "Prompt")
                        {
                            object objPrmpt = "";
                            objPrmpt = oData.FetchValue("SELECT LookupName FROM StdtDSStepStat Stp INNER JOIN LookUp LU ON LU.LookupId=Stp.PromptId WHERE DSTempSetColId=" + oDS.dtColumns.Rows[j]["DSTempSetColId"].ToString() + " AND DSTempStepId=" + dtSteps.Rows[i - 1]["DSTempStepId"].ToString() + "");
                            if (objPrmpt != null)
                                table += "<td>" + objPrmpt.ToString() + "</td>";
                        }
                    }
                }
                table += "</tr>";
            }
        } table += "</table>";
        divStpPrmpts.InnerHtml = table;
    }
    protected void generateSheet(bool VTPopupInd)
    {
        Session["StepLevelPrompt"] = false;
        ClsErrorLog clError = new ClsErrorLog();
        oData = new clsData();
        try
        {
            oTemp = (ClsTemplateSession)Session["BiweeklySession"];
            oDS = (clsDataSheet)Session[DatasheetKey];
            if (oTemp != null)
            {
                if (oDS != null)
                {

                    string sqlStr = "";
                    sqlStr = "SELECT DH.LessonPlanId,DH.LessonDefInst,ISNULL(LU.LookupName,'') AS TeachingProc,ISNULL(LUp.LookupName,'') as PromptProc ,SkillType,ISNULL(NbrOfTrials,0) as NbrOfTrials," +
                            "DH.DSTemplateName AS LessonPlanName,ISNULL(DH.Materials,'') as Mat,ISNULL(DH.TotalTaskType,0) as TType,ISNULL(DH.TaskOther,0)as TOther,ISNULL(ChainType,'') AS ChainType,DH.DSMode,DH.IsVisualTool,ISNULL(DH.VTLessonId,0) as VTLessonId,ISNULL(ModificationInd,0) as ModificationInd,TotalTaskFormat,MatchToSampleType,StudIncorrRespDef,StudentReadCrita,Mistrial,TeacherPrepare  FROM DSTempHdr DH JOIN LessonPlan LP ON LP.LessonPlanId=DH.LessonPlanId LEFT " +
                            "JOIN LookUp LU ON TeachingProcId=LU.LookUpId INNER JOIN Lookup LUp ON LUp.LookupId=PromptTypeId WHERE DSTempHdrId=" + oTemp.TemplateId;
                    DataTable dtTmpHdrDtls = new DataTable();
                    dtTmpHdrDtls = oData.ReturnDataTable(sqlStr, false);
                    if (dtTmpHdrDtls != null)
                    {
                        if (dtTmpHdrDtls.Rows.Count > 0)
                        {
                            oDS.ISVTool = Convert.ToInt32(dtTmpHdrDtls.Rows[0]["IsVisualTool"].ToString());
                            oDS.VTLessonId = Convert.ToInt32(dtTmpHdrDtls.Rows[0]["VTLessonId"].ToString());
                            if (oDS.ISVTool == 1)
                            {
                                if (VTPopupInd)
                                    ClientScript.RegisterStartupScript(this.GetType(), "", "showPop(3);", true);
                            }


                            if (Convert.ToBoolean(dtTmpHdrDtls.Rows[0]["ModificationInd"]) == true)
                            {
                                lblModificationStat.Text = "Need Modification";
                            }

                            lblCorrectRespData.Text = dtTmpHdrDtls.Rows[0]["StudentReadCrita"].ToString();
                            lblInCorrectRespData.Text = dtTmpHdrDtls.Rows[0]["StudIncorrRespDef"].ToString();
                            lblMistrial.Text = dtTmpHdrDtls.Rows[0]["Mistrial"].ToString();
                            oDS.NbrOfTrials = Convert.ToInt32(dtTmpHdrDtls.Rows[0]["NbrOfTrials"].ToString());
                            lblLessonPrep.Text = dtTmpHdrDtls.Rows[0]["TeacherPrepare"].ToString();
                            //object objSess = 0;
                            //if (ViewState["StdtSessHdr"] != null)
                            //{
                            //    objSess = oData.FetchValue("SELECT SessionNbr FROM StdtSessionHdr WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"].ToString());
                            //    if (objSess == null) objSess = 0;
                            //    lblSession.Text = objSess.ToString();
                            //}
                            oDS.ChainType = dtTmpHdrDtls.Rows[0]["ChainType"].ToString();
                            oDS.SkillType = dtTmpHdrDtls.Rows[0]["SkillType"].ToString();
                            if (oDS.SkillType != "Chained")
                            {
                                btnProbe.Visible = false;
                                //btnProbe1.Visible = false;
                            }//Session["StepLevelPrompt"] TType TOther
                            else if (oDS.ChainType == "Total Task")
                            {
                                btnProbe.Visible = false;
                                //btnProbe1.Visible = false;
                            }
                            if (dtTmpHdrDtls.Rows[0]["TType"] != null && Convert.ToInt32(dtTmpHdrDtls.Rows[0]["TType"]) == 1)//|| Convert.ToInt32(dtTmpHdrDtls.Rows[0]["TOther"]) == 1)
                            {
                                Session["StepLevelPrompt"] = true;
                            }
                            lblSd.Text = dtTmpHdrDtls.Rows[0]["LessonDefInst"].ToString();
                            oDS.TeachProc = dtTmpHdrDtls.Rows[0]["TeachingProc"].ToString();
                            //lblTeachProc.Text = oDS.TeachProc;
                            lblTypOfIns.Text = oDS.TeachProc;
                            oDS.Materials = dtTmpHdrDtls.Rows[0]["Mat"].ToString();
                            lblMaterials.Text = oDS.Materials;
                            oDS.PromptProc = dtTmpHdrDtls.Rows[0]["PromptProc"].ToString();
                            lblPromtProc.Text = oDS.PromptProc;
                            oDS.TotalTaskFormat = dtTmpHdrDtls.Rows[0]["TotalTaskFormat"].ToString();
                            oDS.MatchToSampleType = dtTmpHdrDtls.Rows[0]["MatchToSampleType"].ToString();
                            //oDS.CrntSet = 0;

                            oDS.LessonPlan = dtTmpHdrDtls.Rows[0]["LessonPlanName"].ToString();
                            h_LPname.InnerHtml = oDS.LessonPlan;
                            if (dtTmpHdrDtls.Rows[0]["DSMode"].ToString() == "MAINTENANCE")
                            {
                                if (!h_LPname.InnerHtml.ToString().Contains("MAINTENANCE"))
                                {
                                    h_LPname.InnerHtml += " - MAINTENANCE MODE";
                                }

                            }
                            oDS.LessonPlanID = Convert.ToInt32(dtTmpHdrDtls.Rows[0]["LessonPlanId"].ToString());
                            GetDSStat(oDS.SkillType, oDS.TeachProc, oDS.LessonPlanID);
                            oDS.TeachProc = getTeachingMethod(oDS.TeachProc);
                            GetColumnDetls();
                            fillSteps(oTemp.TemplateId, oDS.NbrOfTrials, oDS.SkillType, oDS.CrntSet, oDS.TeachProc, oDS.VTLessonId, oDS.ChainType, oDS.TotalTaskFormat, oDS.MatchToSampleType);

                            //check whether IOA required for any criterias like move up,move down or modification....
                            string multiTechr = "";
                            lblIOA.Text = "IOA Not Required";
                            string chkIOA = "SELECT IOAReqInd,RuleType,CriteriaType,MultiTeacherReqInd " +
                                        "FROM DSTempSetCol Col LEFT OUTER JOIN DSTempRule Rul ON Rul.DSTempSetColId=Col.DSTempSetColId WHERE " +
                                        "Col.DSTempHdrId=" + oTemp.TemplateId + " AND Col.SchoolId = " + oSession.SchoolId + "  AND Col.ActiveInd='A' AND Rul.ActiveInd='A' ORDER BY Col.DSTempSetColId";
                            DataTable dtIOARqrd = new DataTable();
                            dtIOARqrd = oData.ReturnDataTable(chkIOA, false);
                            if (dtIOARqrd != null)
                                if (dtIOARqrd.Rows.Count > 0)
                                {
                                    foreach (DataRow drIOA in dtIOARqrd.Rows)
                                    {
                                        if (Convert.ToBoolean(drIOA["IOAReqInd"]) == true)
                                            lblIOA.Text = "IOA to Advance";
                                        if (Convert.ToBoolean(drIOA["MultiTeacherReqInd"]) == true)
                                            multiTechr = "Multiteacher to Advance";
                                    }
                                }
                            // FillSetStepDetails(oTemp.TemplateId, lblIOA.Text, multiTechr); //fill set,step and prompt detials in popup
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ColorIt", "ColorIt();", true);
                }

            }

        }
        catch (Exception ex)
        {
            //valid = false;
            //LIJU:  Do the Rollback abd Connection close only in the Parent where its declared in a Try/Catch
            //oData.RollBackTransation(tran, con);
            //con.Close();
            clError.WriteToLog(ex.ToString());


            throw ex;
        }
    }
    /// <summary>
    /// Retreive and Save all the Datas from DSStat table to Session...
    /// </summary>
    /// <param name="SkillType">Skilltype of the current ds</param>
    /// <param name="TeachingProc">teaching procedure</param>
    /// <param name="LesPlanID">crnt Lessonplan id</param>
    protected void GetDSStat(string SkillType, string TeachingProc, int LesPlanID)
    {
        int iCrntSet = 0;
        int iCrntStep = 0;
        int iCrntSetNbr = 0;
        int iCrntPrompt = 0;
        string StatusMessage = "";
        string stepName = "";
        string sqlStr = "";
        Session["TargetPrompt"] = null;
        lblPromptData.Visible = true;
        string PromptName = "";
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oSession = (clsSession)Session["UserSession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oSession != null)
        {
            if (oTemp != null)
            {
                if (oDS != null)
                {
                    iCrntSetNbr = Convert.ToInt32(hdn_currTempSetNmbr.Value);
                    iCrntSet = Convert.ToInt32(hdn_currTempSet.Value);

                    DataTable dt = fn_getStepList(iCrntSet);
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            iCrntStep = Convert.ToInt32(dt.Rows[0]["SortOrder"].ToString());
                        }
                        catch
                        {
                            iCrntStep = 0;
                        }
                    }

                    if (oDS.PromptProc != "NA")
                    {
                        if ((oDS.PromptProc == "Least-to-Most")||(oDS.PromptProc == "Graduated Guidance"))
                        {
                            sqlStr = "SELECT ISNULL(PromptId,0)PromptId FROM DSTempPrompt WHERE PromptOrder = (SELECT MIN(PromptOrder) FROM DSTempPrompt WHERE DSTempHdrId = " + oTemp.TemplateId + ") AND DSTempHdrId = " + oTemp.TemplateId;
                            iCrntPrompt = Convert.ToInt32(oData.FetchValue(sqlStr));
                            Session["TargetPrompt"] = iCrntPrompt.ToString();
                        }
                        else
                        {
                            sqlStr = "SELECT ISNULL(PromptId,0)PromptId,LookupName FROM DSTempPrompt,lookup WHERE " +
                           "PromptId=LookupId AND PromptOrder = (SELECT MAX(PromptOrder) FROM DSTempPrompt WHERE DSTempHdrId = " + oTemp.TemplateId + ") AND DSTempHdrId = " + oTemp.TemplateId;

                            DataTable dtPrompt = oData.ReturnDataTable(sqlStr, false);
                            if (dtPrompt != null)
                            {
                                iCrntPrompt = Convert.ToInt32(dtPrompt.Rows[0]["PromptId"]);
                                PromptName = dtPrompt.Rows[0]["LookupName"].ToString();
                                Session["TargetPrompt"] = iCrntPrompt.ToString();
                            }
                        }
                    }

                    ImgBtn_Override.Visible = false;
                    btnProbe.Visible = false;
                    //ImgBtn_Inactive.Visible = false;


                    ////if page is loaded from history...change the stepid to the one in that session....


                    /// If the condition is temporary overriding then change the current set number to the selected one temporarily.
                    if (Convert.ToBoolean(hdn_isMaintainance.Value) == true)
                    {

                        iCrntSetNbr = Convert.ToInt32(hdn_currTempSetNmbr.Value);
                        iCrntSet = Convert.ToInt32(hdn_currTempSet.Value);
                    }

                    //TeachingProcId
                    //Lesson.NextPromptId = iCrntPrompt;
                    if (SkillType == "Chained")
                    {
                        object Stepid = oData.FetchValue("SELECT MIN(sortOrder) FROM DSTempStep WHERE DSTempHdrId = " + oTemp.TemplateId + " AND DSTempSetId=" + iCrntSet + " AND ActiveInd='A'  AND IsDynamic=0");
                        if (Stepid != null && Stepid.ToString() != "")
                        {
                            iCrntStep = Convert.ToInt32(Stepid);
                        }
                        string tempStep = "";
                        sqlStr = "select StepCd+' - '+StepName as StepCd from DSTempStep where DSTempHdrId = " + oTemp.TemplateId + " AND (DSTempSetId=" + iCrntSet + ") AND ActiveInd='A'  AND IsDynamic=0 AND SortOrder=" + iCrntStep;
                        if (oDS.ChainType == "Backward chain")
                            sqlStr = "SELECT * FROM(select StepCd+' - '+StepName as StepCd,SortOrder,RANK() OVER(ORDER BY SortOrder DESC) Rnk from DSTempStep " +
                                        "where DSTempHdrId=" + oTemp.TemplateId + " AND ActiveInd='A'  AND IsDynamic=0  AND (DSTempSetId=" + iCrntSet + ")) A WHERE Rnk=" + iCrntStep;
                        if (oData.FetchValue(sqlStr) != null)
                        {
                            tempStep = oData.FetchValue(sqlStr).ToString().Replace("?s", "'s");
                            tempStep = oData.FetchValue(sqlStr).ToString().Replace("?", "");
                            stepName = tempStep;
                        }
                    }
                    if (iCrntPrompt != 0)
                    {
                        sqlStr = "select LookupName from Lookup where LookupId=" + iCrntPrompt;
                        if (oData.FetchValue(sqlStr) != null)
                        {
                            PromptName = oData.FetchValue(sqlStr).ToString();
                        }
                    }
                    sqlStr = "select SetCd,SetName,SortOrder from DSTempSet where DSTempSetId = " + iCrntSet + " and DSTempHdrId=" + oTemp.TemplateId;
                    DataTable dtSet = oData.ReturnDataTable(sqlStr, false);

                    string setName = "";
                    string tempsetname = "";
                    if (dtSet != null)
                    {
                        if (dtSet.Rows.Count > 0)
                        {
                            tempsetname = dtSet.Rows[0]["SetCd"].ToString().Replace("?s", "'s") + " - " + dtSet.Rows[0]["SetName"].ToString().Replace("?s", "'s");
                            tempsetname = dtSet.Rows[0]["SetCd"].ToString().Replace("?", "") + " - " + dtSet.Rows[0]["SetName"].ToString().Replace("?", "");
                            setName = tempsetname;
                        }
                    }
                    //Assign values to Properties...
                    oDS.CrntSet = iCrntSet;
                    oDS.CrntSetNbr = iCrntSetNbr;
                    oDS.CrntStep = iCrntStep;
                    oDS.CrntPrompt = iCrntPrompt;
                    oDS.StepName = stepName;
                    oDS.SetName = setName;
                    oDS.PromptName = PromptName;
                    oDS.StatusMsg = StatusMessage;

                    //hfcrntPrompt.Value = iCrntPrompt.ToString();
                    lblSet.Text = setName;
                    lblStep_Sample.Text = stepName;
                    lblPromt.Text = PromptName;
                    lblPromptData.Text = PromptName;
                    if (StatusMessage == "COMPLETED")
                    {
                        lblSet.Text = setName + " <b>&nbsp(MASTERED)</b>";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "probe();", true);
                        //btnProbe_Click(sender, e);
                        if (!h_LPname.InnerHtml.ToString().Contains("MAINTENANCE"))
                        {
                            h_LPname.InnerHtml += " - MAINTENANCE MODE";
                        }
                        ImgBtn_Inactive.Visible = true;
                    }
                    else
                    {
                        if (h_LPname.InnerHtml.ToString().Contains("MAINTENANCE"))
                        {
                            if (Convert.ToInt32(oData.FetchValue("SELECT COUNT(*) FROM StdtDSStat WHERE statusMessage='COMPLETED' AND DSTempHdrId='" + oTemp.TemplateId + "'")) > 0)
                            {
                                lblSet.Text = setName + " <b>&nbsp(MASTERED)</b>";
                            }
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "probe();", true);
                        }
                    }

                    //else ImgBtn_Inactive.Visible = false;


                    if (oDS.SessionMistrial)
                    {
                        lblSet.Text += "  <b>(MISTRIAL)</b>";
                        hdnMissTrialRsn.Value = oDS.SessionMistrialRsn;
                    }


                    hfcrntStep.Value = iCrntStep.ToString();


                    string strQry = "SELECT DSTempSetColId,ColName,ColTypeCd,CalcuType from DSTempSetCol WHERE DSTempHdrId=" + oTemp.TemplateId + " And  SchoolId = " + oSession.SchoolId + "  And ActiveInd='A'";
                    DataTable dt_stepList = oData.ReturnDataTable(strQry, false);
                    if (dt_stepList != null)
                    {
                        if (dt_stepList.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_stepList.Rows)
                            {
                                if (dr["ColTypeCd"].ToString() == "Prompt")
                                {
                                    int iColId = Convert.ToInt32(dr["DSTempSetColId"].ToString());
                                    Rules TempRules = new Rules();
                                    TempRules = GetPromptRules(oTemp.TemplateId, iColId);

                                    //if (TempRules.pctAccyMoveUp.iScoreRequired == 0 || TempRules.pctAccyMoveUp.iScoreRequired == 100)
                                    //    Session["StepLevelPrompt"] = true;

                                }
                            }
                        }
                    }

                    if (oDS.ChainType == "Total Task" && Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        lblStep_Sample.Text = ""; lblPromt.Text = "";
                        lblPromptData.Visible = false;
                    }
                    else
                    {
                        lblPromt.Visible = false;
                        div_StepPrompts.Visible = false;
                    }

                }
            }
        }
    }

    protected void GetColumnDetls()
    {
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oSession = (clsSession)Session["UserSession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        bool ContrlEnable = true;
        if (oSession != null)
        {
            if (oTemp != null)
            {
                if (oDS != null)
                {
                    if (oDS.ISVTool == 1) { ContrlEnable = false; }
                    string sqlStr = "SELECT DSTempSetColId,ColName,ColTypeCd,CorrRespDesc,CorrResp,InCorrRespDesc,IncMisTrialInd,MisTrialDesc,CalcuType," +
                                    "CASE(ColTypeCd) WHEN '+/-' THEN '' WHEN 'Prompt' THEN '0' WHEN 'Duration' THEN '00:00:00' " +
                                    "WHEN 'Frequency' THEN '0' WHEN 'Text' THEN '0' END as ColValue, " +
                                    "CASE(ColTypeCd) WHEN '+/-' THEN 'Radio' WHEN 'Prompt' THEN 'DropDown' WHEN 'Duration' THEN 'Timer' " +
                                    "WHEN 'Frequency' THEN 'Freq' WHEN 'Text' THEN 'Text' END as ColControl " +
                                    "FROM DSTempSetCol WHERE " +
                                    "DSTempHdrId=" + oTemp.TemplateId + " AND SchoolId = " + oSession.SchoolId + "  AND ActiveInd='A' ORDER BY DSTempSetColId";
                    DataTable dt = oData.ReturnDataTable(sqlStr, false);
                    oDS.dtColumns = dt;
                    int count = dt.Rows.Count;
                    if (dt != null)
                    {
                        lblResDef.Text = "";
                        lblResIncorrect.Text = "";


                        string[] colnames = new string[] { "Step / Sample / Sd", "Mistrial", "Notes" }; int index = 0;
                        foreach (string colnam in colnames)
                        {
                            TemplateField ItemTmpFld = new TemplateField();
                            // create HeaderTemplate
                            ItemTmpFld.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "SessStepID,StepCd,StepId",
                                                                          colnam, colnam, "", "", true, 0);
                            // create ItemTemplate
                            ItemTmpFld.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "SessStepID,StepCd,StepId",
                                                                          colnam, colnam, "", "", true, 0);
                            ItemTmpFld.ItemStyle.CssClass = "clr"; ItemTmpFld.HeaderStyle.CssClass = "clr";
                            ItemTmpFld.HeaderStyle.Width = Unit.Percentage(20);
                            if (index == 1)
                                ItemTmpFld.HeaderStyle.Width = Unit.Percentage(7);
                            if (index == 2) { ItemTmpFld.ItemStyle.CssClass = "nobdr"; ItemTmpFld.HeaderStyle.CssClass = "nobdr"; }
                            grdDataSht.Columns.Insert(index, ItemTmpFld);
                            index++;
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Convert.ToBoolean(dr["IncMisTrialInd"]) == true)
                            {
                                oDS.MisTrail = true;
                            }
                            if (dr["ColTypeCd"].ToString() == "+/-")
                            {
                                hfPlusMinusResp.Value = dr["CorrResp"].ToString();
                            }
                            string resposeVal = dr["CorrResp"].ToString();
                            string incResposeVal = "";
                            if (dr["ColTypeCd"].ToString() == "+/-")
                            {
                                if (resposeVal == "+")
                                {
                                    incResposeVal = "-";
                                }
                                else
                                {
                                    incResposeVal = "+";
                                }
                            }
                            else
                            {
                                incResposeVal = dr["CorrResp"].ToString();
                            }
                            //if(resposeVal!=null && resposeVal=="+")
                            //{
                            // lblResDef.Text += "Correct Response of is : " + resposeVal + "<br/>" + "Correct Response Data of  is : " + dr["CorrRespDesc"].ToString() + "<br/>";

                            lblResDef.Text += "<b><font size='2'>" + dr["ColName"].ToString() + "</font></b><br/>  " + dr["CorrRespDesc"].ToString() + "<br/> " + dr["InCorrRespDesc"].ToString() + "  <br/> ";
                            if (Convert.ToBoolean(dr["IncMisTrialInd"]) == true)
                            {
                                lblResDef.Text += "Record mistrials" + "<br/> ";
                            }
                            else
                                lblResDef.Text += "Do not record mistrials <br/>";
                            //}
                            //if(resposeVal!=null && resposeVal=="-")
                            //{
                            //lblResIncorrect.Text += "Incorrect Response of " + dr["ColName"].ToString() + " is : " + incResposeVal + "<br/>" + "Incorrect Response Data of " + dr["ColName"].ToString() + " is : " + dr["InCorrRespDesc"].ToString() + "<br/>";
                            //lblResIncorrect.Text += " " + dr["InCorrRespDesc"].ToString() + "<br/>";
                            //}

                            TemplateField ItemTmpField = new TemplateField();
                            // create HeaderTemplate
                            ItemTmpField.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "",
                                                                          dr["ColName"].ToString(), dr["ColControl"].ToString(), dr["DSTempSetColId"].ToString(), dr["ColTypeCd"].ToString(), ContrlEnable, 0);

                            //####
                            bool bFreeTest = false;
                            if (dr["CalcuType"] != null)
                            {
                                if (dr["CalcuType"].ToString().Trim() == "1")
                                    bFreeTest = true;
                            }
                            // create ItemTemplate
                            ItemTmpField.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "",
                                                                          dr["ColName"].ToString(), dr["ColControl"].ToString(), dr["DSTempSetColId"].ToString(), "", ContrlEnable, 0, bFreeTest);
                            ItemTmpField.ItemStyle.CssClass = "clr"; ItemTmpField.HeaderStyle.CssClass = "clr";
                            if (dr["ColControl"].ToString() == "DropDown")
                                ItemTmpField.HeaderStyle.Width = Unit.Percentage(14);
                            if (dr["ColControl"].ToString() == "Radio")
                                ItemTmpField.HeaderStyle.Width = Unit.Percentage(8);
                            if (dr["ColControl"].ToString() == "Timer")
                                ItemTmpField.HeaderStyle.Width = Unit.Pixel(132);
                            if (dr["ColControl"].ToString() == "Text")
                                ItemTmpField.HeaderStyle.Width = Unit.Percentage(12);
                            if (dr["ColControl"].ToString() == "Freq")
                                ItemTmpField.HeaderStyle.Width = Unit.Percentage(12);
                            // then add to the GridView
                            grdDataSht.Columns.Insert(grdDataSht.Columns.Count - 2, ItemTmpField);
                        }
                        int colmncount = 8 - grdDataSht.Columns.Count;

                        ////// COde change for Match To Sample Dynamic Datasheet---------------Start Here--------Arun M----
                        int j = 0;
                        int tempCount = 1;
                        bool row = false;
                        if (oDS.TeachProc == "Match-to-Sample")
                        {

                            sqlStr = "SELECT [DSTempStepId],[StepName] as StepCd,[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                                        " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + oDS.CrntSet + " AND ActiveInd='A' AND IsDynamic=0 ORDER BY [SortOrder]";
                            dt = oData.ReturnDataTable(sqlStr, false);
                            if (ViewState["StdtSessHdr"] != null && ViewState["StdtSessHdr"] != "")
                            {
                                string IsTrial = Convert.ToString(oData.FetchValue("SELECT IsTrial FROM StdtSessionHdr WHERE StdtSessionHdrId='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "'"));
                                if (IsTrial != "")
                                {
                                    sqlStr = "SELECT Step.StdtSessionStepId as SessStepID, TempStep.[DSTempStepId],TempStep.[StepName] as StepCd,TempStep.[StepName],TempStep.SortOrder as StepId  FROM [dbo].[DSTempStep] TempStep  INNER JOIN StdtSessionStep Step " +
                                                 " ON TempStep.DSTempStepId=Step.DSTempStepId  WHERE TempStep.DSTempStepId IN (" + IsTrial + ") ORDER BY NEWID()";
                                    DataTable dtTrialSample = oData.ReturnDataTable(sqlStr, false);

                                    foreach (DataRow dr in dtTrialSample.Rows)
                                    {
                                        DataRow newTrialRow = dt.NewRow();
                                        newTrialRow["DSTempStepId"] = dr["DSTempStepId"];
                                        newTrialRow["StepCd"] = dr["StepCd"];
                                        newTrialRow["StepName"] = dr["StepName"];
                                        newTrialRow["StepId"] = dr["StepId"];
                                        dt.Rows.Add(newTrialRow);
                                    }
                                }
                            }
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    string stepname = dr["StepName"].ToString();
                                    string[] samplesCollection = stepname.Split('[');
                                    string[] samples = samplesCollection[1].Split(',');
                                    colmncount = 8 + samples.Length - grdDataSht.Columns.Count;
                                    for (int i = 0; i < samples.Length; i++)
                                    {
                                        if (!row)
                                        {

                                            if (colmncount >= tempCount)
                                            {
                                                //hereash
                                                if (oDS.MatchToSampleType != "MTSisExpressive")
                                                {
                                                    string Samplename = samples[i].Replace("]", "");

                                                    TemplateField ItemTmpFld = new TemplateField();
                                                    // create HeaderTemplate
                                                    ItemTmpFld.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "",
                                                                                                  "Samples", "Samples", "", "", true, 0);
                                                    // create ItemTemplate
                                                    ItemTmpFld.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "Samples" + i,
                                                                                                  Samplename, "Samples", "", "", true, j);

                                                    ItemTmpFld.ItemStyle.CssClass = "clr"; ItemTmpFld.HeaderStyle.CssClass = "clr";
                                                    ItemTmpFld.HeaderStyle.Width = Unit.Percentage(20);

                                                    grdDataSht.Columns.Insert(grdDataSht.Columns.Count - 2, ItemTmpFld);
                                                    // grdDataSht.Columns.Insert(grdDataSht.Columns.Count - 2, ItemTmpNote);
                                                }
                                            }
                                        }
                                        j++;
                                        tempCount++;
                                    }
                                    row = true;


                                }
                            }
                        }
                        else
                        {
                            ////// COde change for Match To Sample Dynamic Datasheet----------------End Here--------Arun M-----
                            for (int i = 1; i <= colmncount; i++)
                            {
                                // add templated fields to the GridView

                                TemplateField ItemTmpNote = new TemplateField();
                                // create HeaderTemplate
                                ItemTmpNote.HeaderTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Header, "",
                                                                              "NA", "NA", "", "", true, 0);
                                // create ItemTemplate
                                ItemTmpNote.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.Item, "",
                                                                              "", "", "", "", true, 0);
                                ItemTmpNote.ItemStyle.CssClass = "clr"; ItemTmpNote.HeaderStyle.CssClass = "clr";
                                //ItemTmpNote.ItemStyle.BackColor = System.Drawing.Color.FromName("#ECECEC");

                                ItemTmpNote.HeaderStyle.Width = Unit.Percentage(15); ItemTmpNote.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                                // then add to the GridView
                                grdDataSht.Columns.Insert(grdDataSht.Columns.Count - 2, ItemTmpNote);
                            }
                        }
                    }
                    generateMeasurmntTable();
                }
            }
        }
    }

    protected void generateMeasurmntTable()
    {
        oDS = (clsDataSheet)Session[DatasheetKey];
        oData = new clsData();
        if (oDS != null)
        {
            HtmlTableRow trHead = new HtmlTableRow();
            HtmlTableCell tdHead1 = new HtmlTableCell("th"); tdHead1.InnerHtml = "Measurement Label"; tdHead1.Width = "22%"; trHead.Cells.Add(tdHead1);
            HtmlTableCell tdHead2 = new HtmlTableCell("th"); tdHead2.InnerHtml = "Formula"; tdHead2.Width = "18%"; trHead.Cells.Add(tdHead2);
            foreach (DataRow drcol in oDS.dtColumns.Rows)
            {
                HtmlTableCell tdCols = new HtmlTableCell("th");
                tdCols.InnerHtml = drcol["ColName"].ToString();
                tdCols.ID = drcol["DSTempSetColId"].ToString();
                trHead.Cells.Add(tdCols);
            }
            tbl_Measure.Rows.Add(trHead);
            int index = oDS.dtColumns.Rows.Count;
            foreach (DataRow drcol in oDS.dtColumns.Rows)
            {
                string sel = "SELECT CalcType,CalcLabel,CalcRptLabel,CalcFormula FROM DSTempSetColCalc WHERE DSTempSetColId=" + drcol["DSTempSetColId"].ToString();
                DataTable dtColCalc = oData.ReturnDataTable(sel, false);
                if (dtColCalc != null)
                    if (dtColCalc.Rows.Count > 0)
                    {
                        foreach (DataRow drCalc in dtColCalc.Rows)
                        {
                            HtmlTableRow trRow = new HtmlTableRow();
                            HtmlTableCell tdCol1 = new HtmlTableCell("td"); tdCol1.InnerHtml = drCalc["CalcRptLabel"].ToString(); trRow.Cells.Add(tdCol1);
                            HtmlTableCell tdCol2 = new HtmlTableCell("td"); tdCol2.InnerHtml = drCalc["CalcType"].ToString(); trRow.Cells.Add(tdCol2);

                            for (int j = 0; j < oDS.dtColumns.Rows.Count - index; j++)
                            {
                                HtmlTableCell tdCol3 = new HtmlTableCell("td"); trRow.Cells.Add(tdCol3);
                            }

                            HtmlTableCell tdCol = new HtmlTableCell("td"); tdCol.ColSpan = index;
                            Label lbl = new Label(); lbl.ID = (drCalc["CalcType"].ToString().Replace("%", "") + "_" + drcol["DSTempSetColId"].ToString()).Replace(" ", "");
                            lbl.Text = drcol["ColValue"].ToString();
                            if (drCalc["CalcType"].ToString().StartsWith("%"))
                                lbl.Text = "0%";
                            tdCol.Controls.Add(lbl);
                            trRow.Cells.Add(tdCol);

                            tbl_Measure.Rows.Add(trRow);
                        }
                        index--;
                    }
            }
        }
    }

    protected bool SaveDraft(string status, string IOAInd, string action, SqlConnection con, SqlTransaction trans)
    {
        bool rtrn = true;
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        if (oTemp != null)
        {
            oSession = (clsSession)Session["UserSession"];
            if (oSession != null)
            {
                oDS = (clsDataSheet)Session[DatasheetKey];
                if (oDS != null)
                {
                    try
                    {
                        string sessMistrial = "N";
                        if (chkSessMistrial.Checked == true)
                        {
                            sessMistrial = "Y";
                            status = "S";
                            oDS.SessionMistrial = true;
                            oDS.SessionMistrialRsn = hdnMissTrialRsn.Value;
                            Session[DatasheetKey] = oDS;
                            mistrialRsn.Text = hdnMissTrialRsn.Value;
                        }
                        string insertSessionQuery = "";
                        if (hdn_isMaintainance.Value != "true")
                        {
                            insertSessionQuery = "INSERT INTO StdtSessionHdr (SchoolId,StudentId,DSTempHdrId,StdtClassId,LessonPlanId,IOASessionHdrId,SessionNbr,StartTs,SessMissTrailStus,"
                                        + "SessMissTrailRsn,AssignedToId,CurrentSetId,CurrentStepId,CurrentPromptId,SessionStatusCd,Comments,IOAInd,IOAUserId,CreatedBy,CreatedOn,IsMaintanace) "
                                        + "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + oTemp.TemplateId + "," + oSession.Classid + "," + oDS.LessonPlanID + ","
                                        + "" + oDS.IOASessHdr + "," + oDS.SessNbr + ",(GETDATE()),'" + sessMistrial + "','" + hdnMissTrialRsn.Value + "',1," + oDS.CrntSet + "," + oDS.CrntStep + "," + oDS.CrntPrompt + ",'"
                                        + status + "','" + txtNote.Text.Trim() + "','" + IOAInd + "'," + ddlIOAUsers.SelectedValue + "," + oSession.LoginId + ",GETDATE(),'false')";
                        }
                        else
                        {

                            insertSessionQuery = "INSERT INTO StdtSessionHdr (SchoolId,StudentId,DSTempHdrId,StdtClassId,LessonPlanId,IOASessionHdrId,SessionNbr,StartTs,SessMissTrailStus,"
                                            + "SessMissTrailRsn,AssignedToId,CurrentSetId,CurrentStepId,CurrentPromptId,SessionStatusCd,Comments,IOAInd,IOAUserId,CreatedBy,CreatedOn,IsMaintanace) "
                                            + "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + oTemp.TemplateId + "," + oSession.Classid + "," + oDS.LessonPlanID + ","
                                            + "" + oDS.IOASessHdr + "," + oDS.SessNbr + ",(GETDATE()),'" + sessMistrial + "','" + hdnMissTrialRsn.Value + "',1," + oDS.CrntSet + "," + oDS.CrntStep + "," + oDS.CrntPrompt + ",'"
                                            + status + "','" + txtNote.Text.Trim() + "','" + IOAInd + "',1," + oSession.LoginId + ",GETDATE(),'true')";
                        }
                        int sessHdrID = oData.ExecuteWithScopeandConnection(insertSessionQuery, con, trans);
                        if (sessHdrID > 0)
                        {
                            if (Session["NewTemplateId"] != null)
                            {
                                lblSession.Text = "";
                                Session["DupNewTemplateId"] = Session["NewTemplateId"];
                                //Session["NewTemplateId"] = null;
                            }
                            else
                            {
                                lblSession.Text = oDS.SessNbr.ToString();

                            }
                            ViewState["StdtSessHdr"] = sessHdrID.ToString();
                            DataTable dtsteps = oDS.dtSteps;

                            //string strQuery = "SELECT COUNT(1) from StdtSessStimuliActivity where DSTempHdrId=" + oTemp.TemplateId;
                            //int stimulyCount = Convert.ToInt32(oData.FetchValue(strQuery));
                            //if (stimulyCount == 0)
                            //{
                            //    strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,CreatedBy,CreatedOn)VALUES" +
                            //                      "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'STARTED',GETDATE()," + oSession.LoginId + ",GETDATE())";
                            //    int excId = oData.ExecuteWithScopeandConnection(strQuery, con, trans);
                            //    if (excId > 0)
                            //    {
                            //        strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
                            //                      "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'SET',GETDATE()," + oDS.CrntSet + "," + oSession.LoginId + ",GETDATE())";
                            //        int exId = oData.ExecuteWithScopeandConnection(strQuery, con, trans);
                            //        //if (excId > 0)
                            //        //{
                            //        //    if (oDS.SkillType == "Chained")
                            //        //    {
                            //        //        strQuery = "Select DSTempStepId from DSTempStep where  SortOrder= " + oDS.CrntStep + " and DSTempSetId=" + oDS.CrntSet + " and DSTempHdrId=" + oTemp.TemplateId;
                            //        //        int retunID = Convert.ToInt32(oData.FetchValueTrans(strQuery, trans, con));
                            //        //        strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
                            //        //                   "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'STEP',GETDATE()," + retunID + "," + oSession.LoginId + ",GETDATE())";
                            //        //        int execId = oData.ExecuteWithScopeandConnection(strQuery, con, trans);
                            //        //        if (execId > 0)
                            //        //        {
                            //        //            if (oDS.CrntPrompt > 0)
                            //        //            {
                            //        //                strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
                            //        //                           "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'PROMPT',GETDATE()," + oDS.CrntPrompt + "," + oSession.LoginId + ",GETDATE())";
                            //        //                oData.ExecuteWithScopeandConnection(strQuery, con, trans);
                            //        //            }
                            //        //        }
                            //        //    }
                            //        //    else
                            //        //    {
                            //        //        if (oDS.CrntPrompt > 0)
                            //        //        {
                            //        //            strQuery = "INSERT INTO StdtSessStimuliActivity(SchoolId,ClassId,StudentId,DSTempHdrId,ActivitiType,StartTime,ActivityId,CreatedBy,CreatedOn)VALUES" +
                            //        //                       "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'PROMPT',GETDATE()," + oDS.CrntPrompt + "," + oSession.LoginId + ",GETDATE())";
                            //        //            oData.ExecuteWithScopeandConnection(strQuery, con, trans);
                            //        //        }
                            //        //    }
                            //        //}
                            //    }
                            //}

                            if (dtsteps != null)
                            {
                                int index = 0;
                                dtsteps.Columns.Add("SessStepID", System.Type.GetType("System.String"));

                                foreach (DataRow drStep in dtsteps.Rows)
                                {

                                    string mistrial = "NAV";
                                    if (oDS.MisTrail == true)
                                        mistrial = "N";
                                    string insertSessionSetQuery = "INSERT INTO StdtSessionStep (StdtSessionHdrId,DSTempStepId,TrialNbr,TrialName,SessionStatusCd,CreatedBy,CreatedOn) "
                                       + "VALUES(" + sessHdrID + "," + drStep["DSTempStepId"].ToString() + "," + index.ToString() + ",'','" + mistrial + "'," + oSession.LoginId + ",GETDATE())";
                                    int stepID = oData.ExecuteWithScopeandConnection(insertSessionSetQuery, con, trans);
                                    //#####
                                    drStep["SessStepID"] = stepID.ToString();

                                    if (stepID > 0)
                                    {

                                        DataTable dtcolumns = oDS.dtColumns;
                                        if (dtcolumns != null)
                                        {
                                            foreach (DataRow drColmn in dtcolumns.Rows)
                                            {
                                                if (oDS.ChainType == "Total Task")
                                                {
                                                    object objColType = oData.FetchValueTrans("SELECT ColTypeCd FROM DSTempSetCol WHERE DSTempSetColId=" + drColmn["DSTempSetColId"].ToString(), trans, con);
                                                    if (objColType != null)
                                                    {
                                                        if (objColType.ToString() == "Prompt")
                                                        {
                                                            string selStepStat = "SELECT StdtDSStepStatId FROM StdtDSStepStat WHERE DSTempStepId=" + drStep["DSTempStepId"].ToString() + " AND " +
                                                                "  StudentId=" + oSession.StudentId + "";
                                                            if (!oData.IFExistsWithTranss(selStepStat, trans, con))
                                                            {
                                                                string insertStepStats = "INSERT INTO StdtDSStepStat (SchoolId,StudentId,DSTempStepId,DSTempSetColId,PromptId,CreatedBy,CreatedOn) " +
                                                                    "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + drStep["DSTempStepId"].ToString() + "," + drColmn["DSTempSetColId"].ToString() + "," +
                                                                    "" + oDS.CrntPrompt + "," + oSession.LoginId + ",GETDATE())";
                                                                oData.ExecuteWithScopeandConnection(insertStepStats, con, trans);
                                                            }
                                                            else
                                                            {
                                                                selStepStat = "SELECT StdtDSStepStatId FROM StdtDSStepStat WHERE DSTempStepId=" + drStep["DSTempStepId"].ToString() + " AND " +
                                                               "DSTempSetColId is NULL AND StudentId=" + oSession.StudentId + "  ";
                                                                int stepStatId = Convert.ToInt32(oData.FetchValueTrans(selStepStat, trans, con));
                                                                if (stepStatId > 0)
                                                                {
                                                                    string query = "UPDATE StdtDSStepStat set DSTempSetColId=" + drColmn["DSTempSetColId"].ToString() + " ,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() where StdtDSStepStatId = " + stepStatId;
                                                                    int id = oData.Execute(query);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                int crntPrmpt = Convert.ToInt32(Session["TargetPrompt"]);
                                                if (oDS.ChainType == "Total Task")
                                                {
                                                    string selStepStat = "SELECT PromptId FROM StdtDSStepStat WHERE DSTempStepId=" + drStep["DSTempStepId"].ToString() + " AND " +
                                                               "DSTempSetColId=" + drColmn["DSTempSetColId"].ToString() + " AND StudentId=" + oSession.StudentId + "";
                                                    crntPrmpt = Convert.ToInt32(oData.FetchValueTrans(selStepStat, trans, con));
                                                }
                                                string insertSessionSetDtlQuery = "INSERT INTO StdtSessionDtl(StdtSessionStepId,DSTempSetColId,StepVal,CurrentPrompt,RowNumber,CreatedBy,CreatedOn)"
                                                + " VALUES(" + stepID + "," + drColmn["DSTempSetColId"].ToString() + ",'" + drColmn["ColValue"].ToString() + "'," + crntPrmpt + "," + (index + 1) + "," + oSession.LoginId + ",GETDATE())";
                                                int dtlId = oData.ExecuteWithScopeandConnection(insertSessionSetDtlQuery, con, trans);
                                                if (dtlId <= 0)
                                                    return false;
                                            }
                                        }
                                        else return false;
                                    }
                                    else return false;
                                    index++;
                                }
                                oDS.dtSteps = dtsteps;
                            }
                            else
                                rtrn = false;

                        }
                        else
                            rtrn = false;
                    }
                    catch (Exception ex)
                    {
                        rtrn = false;
                        //LIJU:  Do the Rollback abd Connection close only in the Parent where its declared in a Try/Catch
                        //oData.RollBackTransation(trans, con);
                        //con.Close();
                        ClsErrorLog clError = new ClsErrorLog();
                        clError.WriteToLog(ex.ToString());
                        throw ex;
                    }
                }
            }
        }
        return rtrn;
    }

    protected void updateDatas(int sessHdrId, SqlConnection con, SqlTransaction trans)
    {
        oSession = (clsSession)Session["UserSession"];
        oData = new clsData();
        int crntPrmpt = 0;
        if (oSession != null)
        {
            Dictionary<string, string[]> dictSteps = LoadStepVals_toDict();
            if (dictSteps != null)
            {
                int index = 0;
                foreach (var pair in dictSteps)
                {
                    int scoreIndex = 0;
                    oDS = (clsDataSheet)Session[DatasheetKey];
                    if (oDS != null)
                    {
                        foreach (GridViewRow gr in grdDataSht.Rows)
                        {
                            int cnt = 0;
                            TextBox txtStepCmnts = (TextBox)gr.FindControl("txtStepNotes");


                            if (txtStepCmnts.Enabled == true)
                            {

                                HiddenField hfStepid = (HiddenField)gr.FindControl("hfSessStepID");

                                HiddenField hfSample = (HiddenField)gr.FindControl("hfSampleID");

                                // RadioButton rdoSample = (RadioButton)gr.FindControl("rdbStepName_" + hfstp.Value);
                                string mistrial = "NAV";
                                CheckBox chkMistrial = (CheckBox)gr.FindControl("chkMistrial");
                                if (index == 0)
                                {

                                    if (oDS.MisTrail == true)
                                    {
                                        //mistrial = "N";

                                        //if (pair.Value[scoreIndex] == "")
                                        //{
                                        //    mistrial = "Y";
                                        //}
                                        //if (pair.Value[scoreIndex] == "0")
                                        //{
                                        //    mistrial = "Y";
                                        //}
                                        //if (pair.Value[scoreIndex] == "00:00:00")
                                        //{
                                        //    mistrial = "Y";
                                        //}
                                        //// DropDownList drpPrompt = (DropDownList)gr.FindControl("ddlPrompt"); ;
                                        ////HtmlInputRadioButton rdoPlusMinus = (HtmlInputRadioButton)gr.FindControl("rdbRespPlus");
                                        if (chkMistrial.Checked == true) mistrial = "Y";
                                    }
                                    string updSteps = "UPDATE StdtSessionStep SET Comments='" + txtStepCmnts.Text + "',SessionStatusCd='" + mistrial + "',SelectedSample='" + hfSample.Value.Trim() + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() " +
                                        "WHERE StdtSessionStepId=" + hfStepid.Value + " AND StdtSessionHdrId=" + sessHdrId + "";
                                    oData.ExecuteWithTrans(updSteps, con, trans);

                                }
                                mistrial = "N";


                                crntPrmpt = Convert.ToInt32(Session["TargetPrompt"]);
                                if (pair.Value[scoreIndex] == "")
                                {
                                    mistrial = "Y";
                                }
                                if (pair.Value[scoreIndex] == "0")
                                {
                                    mistrial = "Y";
                                }
                                if (pair.Value[scoreIndex] == "00:00:00")
                                {
                                    mistrial = "Y";
                                }
                                // DropDownList drpPrompt = (DropDownList)gr.FindControl("ddlPrompt"); ;
                                //HtmlInputRadioButton rdoPlusMinus = (HtmlInputRadioButton)gr.FindControl("rdbRespPlus");
                                if (chkMistrial.Checked == true)
                                    mistrial = "Y";
                                if (mistrial == "Y" && chkMistrial.Checked != true)
                                {
                                    if (Session["ISAddTrial"] == "True")
                                    {
                                        mistrial = "N";
                                    }
                                }

                                string updStpDtls = "UPDATE StdtSessionDtl SET StepVal='" + pair.Value[scoreIndex] + "',CurrentPrompt='" + crntPrmpt + "',SessionStatusCd='" + mistrial + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() " +
                                    "WHERE StdtSessionStepId=" + hfStepid.Value + " AND DSTempSetColId=" + pair.Key + "";
                                oData.ExecuteWithTrans(updStpDtls, con, trans);


                                scoreIndex++;
                            }
                        }
                        index++;
                    }
                }
            }
            if (oDS.ChainType == "Total Task")
            {

                crntPrmpt = 0;
                DataTable dtsteps = oDS.dtSteps;
                if (dtsteps != null)
                {
                    foreach (DataRow drStep in dtsteps.Rows)
                    {
                        DataTable dtcolumns = oDS.dtColumns;
                        if (dtcolumns != null)
                        {
                            foreach (DataRow drColmn in dtcolumns.Rows)
                            {
                                // crntPrmpt = Convert.ToInt32(Session["TargetPrompt"]);
                                if (oDS.ChainType == "Total Task")
                                {
                                    string selStepStat = "SELECT PromptId FROM StdtDSStepStat WHERE DSTempStepId=" + drStep["DSTempStepId"].ToString() + " AND " +
                                               "DSTempSetColId=" + drColmn["DSTempSetColId"].ToString() + " AND StudentId=" + oSession.StudentId + "";
                                    crntPrmpt = Convert.ToInt32(oData.FetchValueTrans(selStepStat, trans, con));

                                    string updStpDtls = "UPDATE StdtSessionDtl SET CurrentPrompt='" + crntPrmpt + "' WHERE StdtSessionStepId=" + drStep["SessStepID"] + " AND DSTempSetColId=" + drColmn["DSTempSetColId"].ToString() + "";
                                    oData.ExecuteWithTrans(updStpDtls, con, trans);
                                }
                            }
                        }
                    }
                }
            }


        }
    }
    protected bool updateDraft(int sessHdrId, string updateMode, SqlConnection con, SqlTransaction trans)
    {
        bool valid_Ind = false;
        oSession = (clsSession)Session["UserSession"];
        oData = new clsData();
        string updQry = "";
        bool Ismaint = Convert.ToBoolean(hdn_isMaintainance.Value);
        if (oSession != null)
        {
            if (updateMode == "Save")
            {
                oDS = (clsDataSheet)Session[DatasheetKey];
                if (oDS != null)
                {
                    if ((oDS.dtSteps != null) && (oDS.dtSteps.Rows.Count > 0))
                    {
                        string sessMistrial = "N";
                        string status = "D";
                        if (chkSessMistrial.Checked == true)
                        {
                            sessMistrial = "Y";
                            status = "S";
                            oDS.SessionMistrial = true;
                            oDS.SessionMistrialRsn = hdnMissTrialRsn.Value;
                            Session[DatasheetKey] = oDS;
                            mistrialRsn.Text = hdnMissTrialRsn.Value;
                        }
                        if (Convert.ToBoolean(ViewState["IsHistory"]) == true)
                        {
                            updQry = "update StdtSessionHdr SET AssignedToId=1,SessMissTrailStus='" + sessMistrial + "',SessMissTrailRsn='" + hdnMissTrialRsn.Value + "',"
                        + "Comments='" + txtNote.Text.Trim() + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE(),IsUpdate=1 WHERE StdtSessionHdrId=" + sessHdrId + "";

                        }
                        else
                        {
                            string where = "";
                            updQry = "SELECT CASE WHEN Convert(date,StartTs) < Convert(date,GETDATE()) THEN 1 ELSE 0 END AS status " +
                                      "FROM stdtsessionhdr where StdtSessionHdrId=" + sessHdrId;
                            int idStatus = Convert.ToInt32(oData.FetchValueTrans(updQry, trans, con));
                            if (idStatus == 1)
                                where = "IsUpdate=1";
                            else
                                where = "IsUpdate=0";
                            updQry = "update StdtSessionHdr SET AssignedToId=1,SessionStatusCd='" + status + "',SessMissTrailStus='" + sessMistrial + "',SessMissTrailRsn='" + hdnMissTrialRsn.Value + "',"
                               + "Comments='" + txtNote.Text.Trim() + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE()," + where + " WHERE StdtSessionHdrId=" + sessHdrId + "";
                        }
                        if (oData.ExecuteWithTrans(updQry, con, trans) > 0)
                        {

                            updateDatas(sessHdrId, con, trans);
                            valid_Ind = true;
                                updateCheck = true;
                        }
                    }
                }
            }
            if (updateMode == "Submit")
            {
                oDS = (clsDataSheet)Session[DatasheetKey];
                oData = new clsData();
                if (oDS != null)
                {
                    if (oDS.IOAInd == "N")
                    {
                        string sessMistrial = "N";
                        int iIsmaintain = 0;//Ismaint
                        if (chkSessMistrial.Checked == true)
                        {
                            oDS.SessionMistrial = true;
                            oDS.SessionMistrialRsn = hdnMissTrialRsn.Value;
                            Session[DatasheetKey] = oDS;
                            sessMistrial = "Y";
                            mistrialRsn.Text = hdnMissTrialRsn.Value;
                        }
                        if (Ismaint)
                            iIsmaintain = 1;
                        updQry = "update StdtSessionHdr SET IsMaintanace=" + iIsmaintain + ", AssignedToId=1,EndTs=GETDATE(),SessionStatusCd='D',SessMissTrailStus='" + sessMistrial + "',SessMissTrailRsn='" + hdnMissTrialRsn.Value + "',"
                        + "Comments='" + clsGeneral.convertQuotes(txtNote.Text.Trim()) + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE StdtSessionHdrId=" + sessHdrId + "";
                        int retrn = oData.ExecuteWithTrans(updQry, con, trans);
                        if (retrn > 0) { updateDatas(sessHdrId, con, trans); valid_Ind = true; updateCheck = true; }
                        else
                        {
                            oData.RollBackTransation(trans, con);
                            tdMsg.InnerHtml = clsGeneral.failedMsg("Submission Failed");
                            updateCheck = false;
                            valid_Ind = false;
                        }
                    }
                    else if (oDS.IOAInd == "Y")
                    {
                        int sessid = oDS.IOASessHdr;

                        string SelectSessionIOA = "SELECT IOASessionHdrId FROM StdtSessionHdr WHERE StdtSessionHdrId=" + sessHdrId + "";
                        object objIOAid = oData.FetchValueTrans(SelectSessionIOA, trans, con);
                        if (objIOAid == null) objIOAid = 0;
                        int ioaid = Convert.ToInt32(objIOAid);
                        if (ioaid != 0)
                            sessid = ioaid;
                        string SelectSessionDetail = "SELECT SessionStatusCd AS ISExistSession FROM StdtSessionHdr WHERE StdtSessionHdrId=" + sessid + "";
                        object objStatus = oData.FetchValueTrans(SelectSessionDetail, trans, con);
                        if (objStatus != null)
                        {
                            if (objStatus.ToString() != "S")
                            {
                                tdMsg.InnerHtml = clsGeneral.warningMsg("IOA Draft Submission not Possible when Teacher Session currently in Progress");
                                valid_Ind = false;
                            }
                            else
                            {
                                valid_Ind = true;
                                updateCheck = true;
                            }

                        }

                    }
                }
                else
                {
                    valid_Ind = false;
                    updateCheck = false;
                }
            }
        }
        hdn_isMaintainance.Value = "false";
        return valid_Ind;
    }
    protected Dictionary<string, string[]> LoadStepVals_toDict()
    {
        Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oDS != null)
        {
            if ((oDS.dtColumns != null) && (oDS.dtColumns.Rows.Count > 0))
            {
                foreach (DataRow dr in oDS.dtColumns.Rows)
                {
                    string sessionValues = "";
                    foreach (GridViewRow gr in grdDataSht.Rows)
                    {
                        HiddenField hfDusr = (HiddenField)gr.FindControl("hfSessStepID");
                        if (dr["ColTypeCd"].ToString() == "Duration")
                        {
                            HiddenField hfDur = (HiddenField)gr.FindControl("hfDuration_" + dr["DSTempSetColId"].ToString());
                            if (hfDur.Value != "")
                            {
                                sessionValues += hfDur.Value + "|";
                            }
                        }
                        if (dr["ColTypeCd"].ToString() == "Text")
                        {
                            TextBox txtText = (TextBox)gr.FindControl("txtText_" + dr["DSTempSetColId"].ToString());
                            if (txtText.Enabled == true)
                            {
                                if (txtText.Text == "") sessionValues += "0|";
                                else sessionValues += txtText.Text + "|";
                            }
                        }
                        if (dr["ColTypeCd"].ToString() == "Frequency")
                        {
                            TextBox txtFrq = (TextBox)gr.FindControl("txtFrequency_" + dr["DSTempSetColId"].ToString());
                            if (txtFrq.Enabled == true)
                            {
                                if (txtFrq.Text == "") sessionValues += "0|";
                                else sessionValues += txtFrq.Text + "|";
                            }
                        }
                        if (dr["ColTypeCd"].ToString() == "Prompt")
                        {
                            DropDownList ddlPrmpt = (DropDownList)gr.FindControl("ddlPrompt_" + dr["DSTempSetColId"].ToString());
                            if (ddlPrmpt.Enabled == true)
                            {
                                sessionValues += ddlPrmpt.SelectedValue + "|";
                            }
                        }
                        if (dr["ColTypeCd"].ToString() == "+/-")
                        {
                            HtmlInputRadioButton rdbRespplus = (HtmlInputRadioButton)gr.FindControl("rdbRespPlus_" + dr["DSTempSetColId"].ToString());
                            HtmlInputRadioButton rdbRespminus = (HtmlInputRadioButton)gr.FindControl("rdbRespMinus_" + dr["DSTempSetColId"].ToString());
                            if (rdbRespplus != null && rdbRespminus != null)
                            {
                                if ((rdbRespminus.Disabled == false) && (rdbRespplus.Disabled == false))
                                {
                                    if (rdbRespplus.Checked == true) sessionValues += "+|";
                                    else if (rdbRespminus.Checked == true) sessionValues += "-|";
                                    else sessionValues += "|";
                                }
                            }
                        }

                    }
                    if (sessionValues.Length > 0)
                    {
                        sessionValues = sessionValues.Substring(0, (sessionValues.Length - 1));
                        dict.Add(dr["DSTempSetColId"].ToString(), sessionValues.Split('|'));
                    }
                }
            }
            else return null;
        }
        else return null;
        return dict;
    }

    protected bool SaveMeasuremnts(int sessHdrId, SqlConnection con, SqlTransaction tran)
    {
        bool valid = false;
        string value = "";
        try
        {
            oData = new clsData();
            oTemp = (ClsTemplateSession)Session["BiweeklySession"];
            oSession = (clsSession)Session["UserSession"];
            oDS = (clsDataSheet)Session[DatasheetKey];
            if (oSession != null)
            {
                if (oTemp != null)
                {
                    if (oDS != null)
                    {
                        /*
                         * Creation and insertion to a new Session  
                         */
                        //Dictionary<string, string[]> ht = LoadStepVals_toDict();
                        //if (ht != null)
                        {

                            string deleteSessStepScore = "DELETE FROM StdtSessColScore WHERE StdtSessionHdrId=" + sessHdrId + "";
                            oData.ExecuteWithTrans(deleteSessStepScore, con, tran);
                            string sqlStr = "";
                            DataTable dt = new DataTable();
                            //string sqlStr = " SELECT DST.DSTempSetColId,DST.ColName, DC.CalcType,DC.CalcFormula,DC.DSTempSetColCalcId,DT.MultiSetsInd, DT.DSTempHdrId FROM DSTempHdr DT" +
                            //        " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId  " +
                            //        " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId  " +
                            //        " WHERE (DT.DSTempHdrId =" + oTemp.TemplateId + ")";
                            //DataTable dt = oData.ReturnDataTable(sqlStr, tran, false);
                            //int indexi = 0;
                            //int icount = 0;
                            //int count = dt.Rows.Count;
                            //int[] arColcalId = new int[count];
                            //int[] arColId = new int[count];
                            //string[] arColName = new string[oDS.dtColumns.Rows.Count];
                            //string custom = "";
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    arColcalId[icount] = Convert.ToInt32(dr["DSTempSetColCalcId"]);
                            //    if (dr["CalcType"].ToString() == "Customize")
                            //    {
                            //        custom += dr["CalcFormula"].ToString() + "#";
                            //    }
                            //    icount++;
                            //}
                            //ViewState["Custom_Formula"] = custom;
                            //int colIndex = 0;
                            //foreach (DataRow dr in oDS.dtColumns.Rows)
                            //{
                            //    sqlStr = " select dbo.DSTempSetCol.ColName from DSTempSetCol where DSTempSetCol.DSTempSetColId=" + Convert.ToInt32(dr["DSTempSetColId"].ToString());
                            //    arColName[colIndex] = oData.FetchValueTrans(sqlStr, tran, con).ToString();
                            //    colIndex++;
                            //}
                            //string names = "";
                            //float custResult = 0;
                            //string[] sEquation = custom.Split('#');
                            //foreach (var item in sEquation)
                            //{
                            //    Calculate.Calculate oCalc = new Calculate.Calculate();
                            //    if (item != "")
                            //    {
                            //        PreProcessedExpression expResult = oCalc.PreProcessExpression(item);
                            //        int expCount = expResult.ColumnDatas.Length;
                            //        for (int indexj = 0; indexj < expCount; indexj++)
                            //        {
                            //            for (int i = 0; i < oDS.dtColumns.Rows.Count; i++)
                            //            {
                            //                if (ht.ContainsKey(oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()))
                            //                {
                            //                    if (!names.Contains(arColName[i]))
                            //                    {
                            //                        if (arColName[i].ToUpper() == expResult.ColumnDatas[indexj].ColumnName.Trim())
                            //                        {
                            //                            expResult.ColumnDatas[indexj].Data = new float[oDS.dtSteps.Rows.Count - 1];
                            //                            names += arColName[i] + ",";
                            //                            expResult.ColumnDatas[indexj].Data = parseFloat(ht[oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()]);
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //        }
                            //        if (expResult != null)
                            //        {
                            //            string exp = oCalc.CreateExpressionToEvaluate(expResult);
                            //            string[] tempString = exp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            //            string[] postfixArray = oCalc.InfixToPostfix(tempString);
                            //            custResult = oCalc.EvaluatePostfix(postfixArray);
                            //        }
                            //    }
                            //}
                            for (int i = 0; i < oDS.dtColumns.Rows.Count; i++)
                            {
                                sqlStr = " SELECT DST.DSTempSetColId,DST.ColName, DC.CalcType,DC.CalcFormula,DST.ColTypeCd,DC.DSTempSetColCalcId,DT.MultiSetsInd, DT.DSTempHdrId FROM DSTempHdr DT" +
                                   " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId  " +
                                   " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId  " +
                                   " WHERE (DT.DSTempHdrId =" + oTemp.TemplateId + "AND DST.DSTempSetColId=" + Convert.ToInt32(oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()) + ")";
                                dt = oData.ReturnDataTable(sqlStr, con, tran, false);
                                //liju
                                string score = "0";
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "Customize")
                                            {
                                                // if (hfTextScore.Value == "") score = "0";
                                                //else score = hfTextScore.Value.Trim();

                                                score = ReturnScore(hfTextScore, dr["DSTempSetColId"].ToString());
                                            }
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatTrainingStep")
                                            {
                                                if (dr["ColTypeCd"].ToString() == "+/-")
                                                {

                                                    score = ReturnScore(hfResultStep_Acc, dr["DSTempSetColId"].ToString());
                                                }
                                            }
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatPreviouslyLearnedSteps")
                                            {
                                                if (dr["ColTypeCd"].ToString() == "+/-")
                                                {
                                                    score = ReturnScore(hfRslt1_ExcludeCrntStep_Acc, dr["DSTempSetColId"].ToString());
                                                }
                                                if (dr["ColTypeCd"].ToString() == "Prompt")
                                                {
                                                    score = ReturnScore(hfRslt2_ExcludeCrntStep_Acc, dr["DSTempSetColId"].ToString());
                                                }
                                            }
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "%Accuracy")
                                            {
                                                if (dr["ColTypeCd"].ToString() == "+/-")
                                                {
                                                    //if (hfRslt1_Acc.Value == "") score = "0";
                                                    //else score = hfRslt1_Acc.Value.Trim().Replace("%", "");

                                                    score = ReturnScore(hfRslt1_Acc, dr["DSTempSetColId"].ToString());
                                                }
                                                if (dr["ColTypeCd"].ToString() == "Prompt")
                                                {
                                                    //if (hfRslt2_Acc.Value == "") score = "0";
                                                    //else score = hfRslt2_Acc.Value.Trim().Replace("%", "");

                                                    score = ReturnScore(hfRslt2_Acc, dr["DSTempSetColId"].ToString());
                                                }
                                            }
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "%Prompted")
                                            {
                                                if (dr["ColTypeCd"].ToString() == "+/-")
                                                {
                                                    //if (hfRslt1_Prmt.Value == "") score = "0";
                                                    //else score = hfRslt1_Prmt.Value.Trim().Replace("%", "");

                                                    score = ReturnScore(hfRslt1_Prmt, dr["DSTempSetColId"].ToString());
                                                }
                                                if (dr["ColTypeCd"].ToString() == "Prompt")
                                                {
                                                    //if (hfRslt2_Prmt.Value == "") score = "0";
                                                    //else score = hfRslt2_Prmt.Value.Trim().Replace("%", "");

                                                    score = ReturnScore(hfRslt2_Prmt, dr["DSTempSetColId"].ToString());
                                                }
                                            }
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "%Independent")
                                            {
                                                if (dr["ColTypeCd"].ToString() == "+/-")
                                                {
                                                    //if (hfRslt1_Ind.Value == "") score = "0";
                                                    //else score = hfRslt1_Ind.Value.Trim().Replace("%", "");

                                                    score = ReturnScore(hfRslt1_Ind, dr["DSTempSetColId"].ToString());
                                                }
                                                if (dr["ColTypeCd"].ToString() == "Prompt")
                                                {
                                                    //if (hfRslt2_Ind.Value == "") score = "0";
                                                    //else score = hfRslt2_Ind.Value.Trim().Replace("%", "");

                                                    score = ReturnScore(hfRslt2_Ind, dr["DSTempSetColId"].ToString());
                                                }
                                            }
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "AvgDuration")
                                            {
                                                if (hfAvgDur.Value == "") score = TimeSpan.Parse("00:00:00").TotalSeconds.ToString();
                                                else score = TimeSpan.Parse(ReturnScore(hfAvgDur, dr["DSTempSetColId"].ToString())).TotalSeconds.ToString();
                                            }
                                            if (dr["CalcType"].ToString().Replace(" ", "") == "TotalDuration")
                                            {
                                                if (hfTotDur.Value == "") score = TimeSpan.Parse("00:00:00").TotalSeconds.ToString();
                                                else score = TimeSpan.Parse(ReturnScore(hfTotDur, dr["DSTempSetColId"].ToString())).TotalSeconds.ToString();
                                            }
                                            if (dr["CalcType"].ToString() == "Frequency")
                                            {
                                                //if (hf_Freq.Value == "") score = "0";
                                                //else score = hf_Freq.Value.Trim();

                                                score = ReturnScore(hf_Freq, dr["DSTempSetColId"].ToString());
                                            }
                                            //Total correct and incorrect jis

                                            if (dr["CalcType"].ToString() == "Total Correct")
                                            {
                                                if (dr["ColTypeCd"].ToString() == "+/-")
                                                {
                                                    if (hfTotCorct.Value != "")
                                                    {
                                                        score = hfTotCorct.Value;
                                                    }
                                                    else
                                                        score = "0";
                                                }
                                            }
                                            if (dr["CalcType"].ToString() == "Total Incorrect")
                                            {
                                                if (dr["ColTypeCd"].ToString() == "+/-")
                                                {
                                                    if (hfInTotCorct.Value != "")
                                                    {
                                                        score = hfInTotCorct.Value;
                                                    }
                                                    else
                                                        score = "0";
                                                }
                                            }


                                            // Code By Arun for update Datasheet Scores.


                                            string[] arguments = new string[8];

                                            //if (Convert.ToBoolean(ViewState["IsHistory"]) == false)
                                            //{
                                            string insertSessionScoreQuery = "INSERT INTO StdtSessColScore (SchoolId,StudentId,DSTempSetColId,DSTempSetColCalcId,StdtSessionHdrId,Score,CreatedBy,CreatedOn)"
                                                + "values(" + oSession.SchoolId + "," + oSession.StudentId + "," + Convert.ToInt32(oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()) + ","
                                                + Convert.ToInt32(dr["DSTempSetColCalcId"].ToString()) + "," + sessHdrId + ","
                                                + "" + float.Parse(score) + "," + oSession.LoginId + ",GETDATE())";
                                            if (oData.ExecuteWithScopeandConnection(insertSessionScoreQuery, con, tran) > 0) valid = true;
                                            //}

                                            //else
                                            //{
                                            //    string updateQry = "UPDATE StdtSessColScore SET Score= " + float.Parse(score) + ",ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE " +
                                            //        "StdtSessionHdrId=" + sessHdrId + " AND DSTempSetColId=" + Convert.ToInt32(oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()) + " AND " +
                                            //        " DSTempSetColCalcId=" + Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                                            //    oData.ExecuteWithTrans(updateQry, con, tran); valid = true;
                                            //}



                                        }
                                    }
                                }
                            }
                        }
                        //else valid = false;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            valid = false;
            //LIJU:  Do the Rollback abd Connection close only in the Parent where its declared in a Try/Catch
            //oData.RollBackTransation(tran, con);
            //con.Close();
            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(ex.ToString());
            throw ex;
        }
        return valid;
    }

    public string ReturnScore(HiddenField hf, string colid)
    {
        string score = "0";
        if (hf.Value.Length >= 1)
        {
            string reslt = hf.Value;
            string[] items = reslt.Split('|');
            foreach (string item in items)
            {
                if (colid == item.Split('*')[0])
                {
                    score = item.Split('*')[1].Replace("%", "");
                }
            }
        }
        else
        {
            score = "0";
        }
        //int outrslt;
        //if (!Int32.TryParse(score, out outrslt))
        //{
        //    score = "0";
        //}

        return score;
    }

    protected float[] parseFloat(string[] p)
    {
        float[] temp = new float[p.Length];
        for (int i = 0; i < p.Length; i++)
        {
            temp[i] = float.Parse(p[i]);
        }
        return temp;
    }

    protected void LoadData(int SessHdrID, bool VTPopupInd)
    {
        ClsErrorLog clError = new ClsErrorLog();
        oDS = (clsDataSheet)Session[DatasheetKey];
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oData = new clsData();

        //SqlConnection con = null;
        //SqlTransaction trans = null;

        try
        {
            //con = oData.Open();
            //trans = con.BeginTransaction();
            string strqry = "SELECT DSTempHdrId FROM StdtSessionHdr WHERE StdtSessionHdrId=" + SessHdrID;
            object objSessHdrID = oData.FetchValue(strqry);
            if (objSessHdrID != null)
            {
                oTemp.TemplateId = Convert.ToInt32(objSessHdrID);
                lblSession.Text = oDS.SessNbr.ToString();

            }
            generateSheet(VTPopupInd);
            getStepPrompts();


            if (oTemp != null)
            {
                if (oDS != null)
                {
                    fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);
                    object objSessNbr = oData.FetchValue("SELECT SessionNbr FROM StdtSessionHdr WHERE StdtSessionHdrId=" + SessHdrID);
                    if (objSessNbr != null)
                    {
                        oDS.SessNbr = Convert.ToInt32(objSessNbr);
                        lblSession.Text = oDS.SessNbr.ToString();
                        if (Session["NewTemplateId"] != null)
                        {
                            lblSession.Text = "";
                            Session["DupNewTemplateId"] = Session["NewTemplateId"];
                            //Session["NewTemplateId"] = null;
                        }
                    }


                    object objSessStat = oData.FetchValue("SELECT SessionStatusCd FROM StdtSessionHdr WHERE StdtSessionHdrId=" + SessHdrID);
                    if (objSessStat != null)
                    {
                        if (objSessStat.ToString() == "S")
                        { /* btnSave.Visible = false;*/
                            btnSave.Text = "Update";
                            btnSave1.Text = "Update";
                            btnSubmit.Visible = false;
                            btnSubmit1.Visible = false;
                            chkSessMistrial.Visible = false;
                            ImgBtn_Override.Visible = false;
                            btnSubmitAndRepeat1.Visible = false;
                            btnSubmitAndRepeat2.Visible = false;
                            btnProbe.Visible = false;
                            if (oDS.VTLessonId > 0)
                            {
                                btnSave.Visible = false;
                                btnSave1.Visible = false;
                                LabelvisualToolEdit.Visible = true;
                                LabelvisualToolEdit.Text = "VisualTool Score Editing Is Not Possible..";
                            }
                        }
                        //
                    }
                    //DataTable dtstp = new DataTable();
                    //dtstp = oDS.dtSteps;

                    foreach (GridViewRow grStep in grdDataSht.Rows)
                    {
                        HiddenField hfStepid = (HiddenField)grStep.FindControl("hfSessStepID");
                        string qry = "SELECT Step.Comments StpCmts,Step.SelectedSample,Dtl.StdtSessionStepId,Dtl.DSTempSetColId,Col.ColTypeCd,StepVal,Dtl.SessionStatusCd as SessMisStat,RowNumber,Hdr.Comments,Step.SessionStatusCd FROM StdtSessionStep Step " +
                                "INNER JOIN StdtSessionHdr Hdr ON Hdr.StdtSessionHdrId=Step.StdtSessionHdrId " +
                                "INNER JOIN StdtSessionDtl Dtl INNER JOIN DSTempSetCol Col ON Col.DSTempSetColId=Dtl.DSTempSetColId " +
                                "ON Dtl.StdtSessionStepId=Step.StdtSessionStepId WHERE Hdr.StdtSessionHdrId=" + SessHdrID + " AND Dtl.StdtSessionStepId=" + hfStepid.Value;
                        DataTable dtColmns = oData.ReturnDataTable(qry, false);
                        int statusFlag = 0;
                        if (dtColmns != null)
                        {
                            if (dtColmns.Rows.Count > 0)
                            {

                                CheckBox chkMistrial = (CheckBox)grStep.FindControl("chkMistrial");


                                foreach (DataRow drcolmn in dtColmns.Rows)
                                {
                                    if (oDS.TeachProc == "Match-to-Sample")
                                    {

                                        HiddenField hdfSample = (HiddenField)grStep.FindControl("hfSampleID");
                                        hdfSample.Value = drcolmn["SelectedSample"].ToString();
                                    }

                                    if (drcolmn["SessMisStat"].ToString() == "Y")
                                    {
                                        statusFlag++;

                                    }


                                    if (drcolmn["ColTypeCd"].ToString() == "Duration")
                                    {
                                        HiddenField hfDur = (HiddenField)grStep.FindControl("hfDuration_" + drcolmn["DSTempSetColId"].ToString());
                                        HtmlInputText txtdur = (HtmlInputText)grStep.FindControl("txtDuratn_" + drcolmn["DSTempSetColId"].ToString());
                                        hfDur.Value = drcolmn["StepVal"].ToString();
                                        txtdur.Value = drcolmn["StepVal"].ToString();
                                    }
                                    if (drcolmn["ColTypeCd"].ToString() == "Text")
                                    {
                                        TextBox txtText = (TextBox)grStep.FindControl("txtText_" + drcolmn["DSTempSetColId"].ToString());
                                        if (drcolmn["StepVal"].ToString() != "0")
                                            txtText.Text = drcolmn["StepVal"].ToString();

                                        // calculateFormula();
                                    }
                                    if (drcolmn["ColTypeCd"].ToString() == "Frequency")
                                    {
                                        TextBox txtFrq = (TextBox)grStep.FindControl("txtFrequency_" + drcolmn["DSTempSetColId"].ToString());
                                        if (drcolmn["StepVal"].ToString() != "0")
                                            txtFrq.Text = drcolmn["StepVal"].ToString();
                                    }
                                    if (drcolmn["ColTypeCd"].ToString() == "Prompt")
                                    {
                                        DropDownList ddlPrmpt = (DropDownList)grStep.FindControl("ddlPrompt_" + drcolmn["DSTempSetColId"].ToString());
                                        if (drcolmn["StepVal"].ToString() != "")
                                            ddlPrmpt.SelectedValue = drcolmn["StepVal"].ToString();
                                    }
                                    if (drcolmn["ColTypeCd"].ToString() == "+/-")
                                    {
                                        HtmlInputRadioButton rdbRespplus = (HtmlInputRadioButton)grStep.FindControl("rdbRespPlus_" + drcolmn["DSTempSetColId"].ToString());
                                        HtmlInputRadioButton rdbRespminus = (HtmlInputRadioButton)grStep.FindControl("rdbRespMinus_" + drcolmn["DSTempSetColId"].ToString());
                                        if (rdbRespplus != null && rdbRespminus != null)
                                        {
                                            if (drcolmn["StepVal"].ToString() != "")
                                            {
                                                if (drcolmn["StepVal"].ToString() == "+")
                                                    rdbRespplus.Checked = true;
                                                if (drcolmn["StepVal"].ToString() == "-")
                                                    rdbRespminus.Checked = true;
                                            }
                                        }
                                    }
                                    TextBox txtnotes = (TextBox)grStep.FindControl("txtStepNotes");
                                    if (drcolmn["StpCmts"] != null)
                                        txtnotes.Text = drcolmn["StpCmts"].ToString();
                                    if (drcolmn["Comments"] != null)
                                        txtNote.Text = drcolmn["Comments"].ToString();
                                }
                                if (statusFlag == dtColmns.Rows.Count)
                                    chkMistrial.Checked = true;
                                else
                                    chkMistrial.Checked = false;
                            }
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "fillMatchtosapmpleval();", true);

                }
            }
            /// oData.CommitTransation(trans, con);
            //con.Close();
        }
        catch (Exception ex)
        {
            //if (trans != null && trans.Connection.State == ConnectionState.Open)
            //{
            //    oData.RollBackTransation(trans, con);

            //}
            //if (con != null)
            //    con.Close();
            clError.WriteToLog(ex.ToString());
            Response.Redirect("Error.aspx");
        }
    }


    protected void btnIOASelect_Click(object sender, EventArgs e)
    {
        oData = new clsData();

        SqlConnection con = null;
        SqlTransaction trans = null;

        oDS = (clsDataSheet)Session[DatasheetKey];
        try
        {
            if (oDS != null)
            {


                generateSheet(true);
                con = oData.Open();
                trans = con.BeginTransaction();


                bool reslt;
                //if (Session["NewTemplateId"] != null)
                //    reslt = SaveDraft("P", "Y", "insert", con, trans);
                //else
                reslt = SaveDraft("D", "Y", "insert", con, trans);
                if (reslt)
                {
                    oDS.IOAInd = "Y";
                    bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                    if (reslt2) oData.CommitTransation(trans, con);
                    else if (trans.Connection.State == ConnectionState.Open) oData.RollBackTransation(trans, con);

                    con.Close();

                    getStepPrompts();
                    fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);

                }
                else if (trans.Connection.State == ConnectionState.Open)
                {
                    oData.RollBackTransation(trans, con);
                    con.Close();
                }


            }

            btnSubmitAndRepeat1.Visible = false;
            btnSubmitAndRepeat2.Visible = false;
        }
        catch (Exception ex)
        {
            if (trans != null && trans.Connection != null && trans.Connection.State == ConnectionState.Open)
            {
                oData.RollBackTransation(trans, con);

            }
            if (con != null)
                con.Close();

            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(ex.ToString());
            throw ex;
        }
    }
    protected void btnNoIOA_Click(object sender, EventArgs e)
    {
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oDS != null)
        {
            oDS.IOAInd = "N";
            LoadData(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), true);
        }
    }

    public int fillTempOverride(int templateId)
    {
        int retValue = 1;
        clsData oData = new clsData();
        ClsTemplateSession oTemp = (ClsTemplateSession)HttpContext.Current.Session["BiweeklySession"];
        string sqlStr = "";
        DataTable ds;
        //string teachProcedure = "";
        //sqlStr = "SELECT LookupName from Lookup where LookupId=(Select TeachingProcId  from DSTempHdr where DSTempHdrId=" + oTemp.TemplateId + ")";
        //teachProcedure = oData.FetchValue(sqlStr).ToString();
        //if (teachProcedure == "Task Analysis - Forward Chain")
        //{
        //    sqlStr = "select Distinct DSTempSet.DSTempSetId,SetCd,SetName,DSTempSet.SortOrder from DSTempSet inner join DSTempStep on DSTempSet.DSTempSetId=DSTempStep.DSTempSetId where DSTempSet.DSTempHdrId=147905 AND DSTempSet.ActiveInd = 'A' order by SortOrder";
        //    ds = oData.ReturnDataTable(sqlStr, false);
        //}
        //else
        //{
            sqlStr = "select DSTempSetId,SetCd,SetName,SortOrder from DSTempSet inn where DSTempHdrId=" + oTemp.TemplateId + " AND ActiveInd = 'A' order by SortOrder";
            ds = oData.ReturnDataTable(sqlStr, false);
        //}
        if (ds != null)
        {
            if (ds.Rows.Count > 0)
            {
                rptr_tempOverride.DataSource = ds;
                rptr_tempOverride.DataBind();

                lblDefMsg.Visible = false;
            }
        }





        return retValue;
    }

    //protected void grdSteps_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    oDS = (clsDataSheet)Session[DatasheetKey];
    //    DataTable dtColmns = oDS.dtColumns;
    //    if (oDS != null)
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            if (oDS.MisTrail == true)
    //            {
    //                CheckBox chkMistrial = (CheckBox)e.Row.FindControl("chkMistrial");
    //                chkMistrial.Enabled = true;
    //            }
    //            foreach (DataRow dr in dtColmns.Rows)
    //            {
    //                if (dr["ColTypeCd"].ToString() == "Duration")
    //                {
    //                    var cntrls = e.Row.Controls[4].Controls[1];
    //                    HtmlInputButton btn = (HtmlInputButton)cntrls;
    //                    btn.Disabled = false;
    //                }
    //                if (dr["ColTypeCd"].ToString() == "Text")
    //                {
    //                    TextBox txtText = (TextBox)e.Row.FindControl("txtText");
    //                    txtText.Enabled = true;
    //                }
    //                if (dr["ColTypeCd"].ToString() == "Frequency")
    //                {
    //                    TextBox txtFrq = (TextBox)e.Row.FindControl("txtFrequency");
    //                    txtFrq.Enabled = true;
    //                }
    //                if (dr["ColTypeCd"].ToString() == "Prompt")
    //                {
    //                    DropDownList ddlPrmpt = (DropDownList)e.Row.FindControl("ddlPrompt");
    //                    ddlPrmpt.Enabled = true;
    //                }
    //                if (dr["ColTypeCd"].ToString() == "+/-")
    //                {
    //                    HtmlInputRadioButton rdbRespplus = (HtmlInputRadioButton)e.Row.FindControl("rdbRespPlus");
    //                    HtmlInputRadioButton rdbRespminus = (HtmlInputRadioButton)e.Row.FindControl("rdbRespMinus");
    //                    rdbRespminus.Disabled = false;
    //                    rdbRespplus.Disabled = false;
    //                }
    //                //txtStepNotes
    //            }

    //        }
    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveDraft();
        if (Convert.ToBoolean(ViewState["IsHistory"]) == true)
        {
            oDS.VTLessonId = 0;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe1(" + oSession.StudentId + ");", true);
        }

    }

    private void SaveDraft()
    {
        btnSave.Enabled = false;
        btnSave1.Enabled = false;

        SqlConnection con = null;
        SqlTransaction trans = null;
        try
        {
            oData = new clsData();
            con = oData.Open();
            trans = con.BeginTransaction();
            if (updateDraft(Convert.ToInt32(ViewState["StdtSessHdr"]), "Save", con, trans))
            {               
                bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                if (reslt2)
                {
                    oData.CommitTransation(trans, con);
                    tdMsg.InnerHtml = clsGeneral.sucessMsg("Data Successfully Saved");

                    oDS = (clsDataSheet)Session[DatasheetKey];
                    if (oDS != null)
                        if (oDS.SessionMistrial)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe1();", true);
                        }
                }
                else
                {
                    oData.RollBackTransation(trans, con);
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Updation Failed");
                }
            }
            btnSave.Enabled = true;
            btnSave1.Enabled = true;
            // oData.CommitTransation(trans, con);
            con.Close();
        }
        catch (Exception ex)
        {
            if (trans != null && trans.Connection != null && trans.Connection.State == ConnectionState.Open)
            {
                oData.RollBackTransation(trans, con);

            }
            if (con != null)
                con.Close();

            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(ex.ToString());
            throw ex;
        }
    }

    private void SaveIOAPercentage(SqlConnection con, SqlTransaction trans)
    {
        oData = new clsData();
        oSession = (clsSession)Session["UserSession"];
        int sessHdrId = Convert.ToInt32(ViewState["StdtSessHdr"]);
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oDS.IOAInd == "Y")
        {
            SqlCommand cmd = new SqlCommand("SELECT IOASessionHdrId FROM StdtSessionHdr WHERE StdtSessionHdrId=" + sessHdrId, con);
            cmd.Transaction = trans;
            int IOAsesshdrId = Convert.ToInt32(cmd.ExecuteScalar());
            string UpdateIOA = "UPDATE StdtSessionHdr SET EndTs=GETDATE(),SessionStatusCd='S',Comments='" + clsGeneral.convertQuotes(txtNote.Text.Trim()) + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE StdtSessionHdrId=" + sessHdrId;
            int retrn = oData.ExecuteWithTrans(UpdateIOA, con, trans);
            if (retrn > 0)
            {
                updateDatas(sessHdrId, con, trans);
                oData.ExecuteIOAPercCalculation(IOAsesshdrId, sessHdrId, con, trans);
            }
            else { tdMsg.InnerHtml = clsGeneral.failedMsg("Submission Failed"); }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SqlConnection con = null;
        SqlTransaction trans = null;
        SqlConnection con2 = null;
        SqlTransaction trans2 = null;
        bool IsMaintanace = false;
        oData = new clsData();
        bool transCheck1 = false;
        bool transCheck2 = false;
        IsMaintanace = Convert.ToBoolean(hdn_isMaintainance.Value);
        try
        {
            string sqlStr = "select statusMessage from StdtDSStat where DSTempHdrId=" + oTemp.TemplateId;
            if (oData.FetchValue(sqlStr) != null)
            {
                string StatusMessage = oData.FetchValue(sqlStr).ToString();
                if (StatusMessage == "COMPLETED")
                    IsMaintanace = true;
                else
                    IsMaintanace = false;
            }
            con = oData.Open();
            trans = con.BeginTransaction();
            if (updateDraft(Convert.ToInt32(ViewState["StdtSessHdr"]), "Submit", con, trans))
            {
                oSession = (clsSession)Session["UserSession"];
                bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                if (reslt2)
                {
                    oDS = (clsDataSheet)Session[DatasheetKey];
                    if (!oDS.SessionMistrial)
                    {
                        if (!IsMaintanace)
                        {
                            SaveIOAPercentage(con, trans);
                            oData.CommitTransation(trans, con);
                            con.Close();
                            transCheck1 = true;
                            updateCheck = true;
                            con2 = oData.Open();
                            trans2 = con2.BeginTransaction();
                            checkScore(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                            oData.CommitTransation(trans2, con2);
                            con2.Close();
                            transCheck2 = true;
                        }
                        else
                        {
                            oData.CommitTransation(trans, con);
                            con.Close();
                            transCheck1 = true;
                            transCheck2 = true;
                            updateCheck = true;
                        }


                    }
                    else
                    {
                        oData.CommitTransation(trans, con);
                        con.Close();
                        transCheck1 = true;
                        transCheck2 = true;
                        updateCheck = true;
                    }
                    //code_start
                    //delete llKvpSetList[0] and put it in the session
                    //check if llKvpSetList has any value
                    //if yes, call LoadDatasheet(lpId)
                    //else proceed below
                    Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
                    LinkedList<KeyValuePair<string, Hashtable>> llKvpSetList = new LinkedList<KeyValuePair<string, Hashtable>>();
                    if (htLpList != null)
                    {
                        string lpId = oTemp.TemplateId.ToString();
                        llKvpSetList = (LinkedList<KeyValuePair<string, Hashtable>>)htLpList[lpId];
                        if (llKvpSetList != null)
                        {
                            if (llKvpSetList.Count > 0)
                            {
                                llKvpSetList.RemoveFirst();
                                if (llKvpSetList.Count == 0)
                                {
                                    htLpList.Remove(lpId);
                                }
                            }
                        }
                        Session["tempOverrideHT"] = htLpList;
                    }
                    //code_end

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe1(" + oSession.StudentId + ");", true);
                    //Session.Remove(DatasheetKey);

                    //code_start
                    if (htLpList != null)
                    {
                        if (llKvpSetList != null)
                        {
                            if (llKvpSetList.Count > 0)
                            {
                                Response.Redirect("DatasheetPreview.aspx?pageid=" + oTemp.TemplateId + "&studid=" + oSession.StudentId, false);
                            }
                        }
                    }
                    if(transCheck1 && transCheck2)
                    {
                        string updQry = "update StdtSessionHdr SET SessionStatusCd='S' WHERE StdtSessionHdrId=" + Convert.ToInt32(ViewState["StdtSessHdr"]) + "";
                        int retrn = oData.Execute(updQry);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe1(" + oSession.StudentId + ");", true);
                        Session.Remove(DatasheetKey);
                    }
                    else
                    {
                        updateCheck = false;
                        tdMsg.InnerHtml = clsGeneral.failedMsg("Submission Failed");
                    }
                    
                    //code_end
                }
                else
                {
                    updateCheck = false;
                    oData.RollBackTransation(trans, con);
                    tdMsg.InnerHtml = clsGeneral.failedMsg("Submission Failed");
                }
            }
        }
        catch (Exception ex)
        {
            if (trans != null && trans.Connection != null && trans.Connection.State == ConnectionState.Open)
                oData.RollBackTransation(trans, con);

            if (con != null)
                con.Close();

            if (trans2 != null && trans2.Connection != null && trans2.Connection.State == ConnectionState.Open)
                oData.RollBackTransation(trans2, con2);

            if (con2 != null)
                con2.Close();

            string qry = "Update StdtSessionHdr set SessionStatusCd = 'D' where StdtSessionHdrId = " + Convert.ToInt32(ViewState["StdtSessHdr"]);
            oData.Execute(qry);
            updateCheck = false;
            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(ex.ToString());
            tdMsg.InnerHtml = clsGeneral.failedMsg("Submission Failed");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe1(" + oSession.StudentId + ");", true);
        }
    }
    [System.Web.Services.WebMethod]
    //public static string ScoreForText(string[] txts, string colId)
    //{
    //    clsData oData = new clsData();
    //    clsDataSheet oDS = (clsDataSheet)HttpContext.Current.Session[DatasheetKey];
    //    ClsTemplateSession oTemp = (ClsTemplateSession)HttpContext.Current.Session["BiweeklySession"];
    //    string sqlStr = " SELECT DST.DSTempSetColId,DST.ColName, DC.CalcType,DC.CalcRptLabel,DC.DSTempSetColCalcId,DT.MultiSetsInd, DT.DSTempHdrId FROM DSTempHdr DT" +
    //                                " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId  " +
    //                                " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId  " +
    //                                " WHERE (DT.DSTempHdrId =" + oTemp.TemplateId + ")";
    //    DataTable dt = oData.ReturnDataTable(sqlStr, false);
    //    int indexi = 0;
    //    int icount = 0;
    //    int count = dt.Rows.Count;
    //    int[] arColcalId = new int[count];
    //    int[] arColId = new int[count];
    //    string[] arColName = new string[oDS.dtColumns.Rows.Count];
    //    string custom = "";
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        arColcalId[icount] = Convert.ToInt32(dr["DSTempSetColCalcId"]);
    //        if (dr["CalcType"].ToString() == "Customize")
    //        {
    //            custom += dr["CalcRptLabel"].ToString() + "#";
    //        }
    //        icount++;
    //    }
    //    //ViewState["Custom_Formula"] = custom;
    //    int colIndex = 0;
    //    foreach (DataRow dr in oDS.dtColumns.Rows)
    //    {
    //        sqlStr = " select dbo.DSTempSetCol.ColName from DSTempSetCol where DSTempSetCol.DSTempSetColId=" + Convert.ToInt32(dr["DSTempSetColId"].ToString());
    //        arColName[colIndex] = oData.FetchValue(sqlStr).ToString();
    //        colIndex++;
    //    }
    //    string names = "";
    //    float custResult = 0;
    //    string[] sEquation = custom.Split('#');
    //    foreach (var item in sEquation)
    //    {
    //        //Calculate.Calculate oCalc = new Calculate.Calculate();
    //        //if (item != "")
    //        //{
    //        //    PreProcessedExpression expResult = oCalc.PreProcessExpression(item);
    //        //    int expCount = expResult.ColumnDatas.Length;
    //        //    for (int indexj = 0; indexj < expCount; indexj++)
    //        //    {
    //        //        for (int i = 0; i < oDS.dtColumns.Rows.Count; i++)
    //        //        {
    //        //            if (!names.Contains(arColName[i]))
    //        //            {
    //        //                if (arColName[i].ToUpper() == expResult.ColumnDatas[indexj].ColumnName.Trim())
    //        //                {
    //        //                    expResult.ColumnDatas[indexj].Data = new float[oDS.dtSteps.Rows.Count - 1];
    //        //                    names += arColName[i] + ",";
    //        //                    expResult.ColumnDatas[indexj].Data = parseFloat(txts);
    //        //                }
    //        //            }
    //        //        }
    //        //    }



    //        //    if (expResult != null)
    //        //    {
    //        //        string exp = oCalc.CreateExpressionToEvaluate(expResult);
    //        //        string[] tempString = exp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    //        //        string[] postfixArray = oCalc.InfixToPostfix(tempString);
    //        //        custResult = oCalc.EvaluatePostfix(postfixArray);
    //        //    }
    //        //}
    //    }
    //    return custResult.ToString();
    //}
    // [System.Web.Services.WebMethod]
    private void txtText_TextChanged(object sender, System.EventArgs e)
    {
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oSession = (clsSession)Session["UserSession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oSession != null)
        {
            if (oTemp != null)
            {
                if (oDS != null)
                {
                    /*
                     * Creation and insertion to a new Session  
                     */
                    Dictionary<string, string[]> ht = LoadStepVals_toDict();
                    if (ht != null)
                    {


                        string sqlStr = " SELECT DST.DSTempSetColId,DST.ColName, DC.CalcType,DC.CalcRptLabel,DC.CalcFormula,DST.CalcuData,DC.DSTempSetColCalcId,DT.MultiSetsInd, DT.DSTempHdrId FROM DSTempHdr DT" +
                                " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId  " +
                                " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId  " +
                                " WHERE (DT.DSTempHdrId =" + oTemp.TemplateId + ")";
                        DataTable dt = oData.ReturnDataTable(sqlStr, false);
                        int indexi = 0;
                        int icount = 0;
                        int count = dt.Rows.Count;
                        int[] arColcalId = new int[count];
                        int[] arColId = new int[count];
                        string[] arColName = new string[oDS.dtColumns.Rows.Count];
                        string custom = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            arColcalId[icount] = Convert.ToInt32(dr["DSTempSetColCalcId"]);
                            if (dr["CalcType"].ToString() == "Customize")
                            {
                                custom += dr["CalcuData"].ToString() + "#";
                            }
                            icount++;
                        }
                        int colIndex = 0;
                        foreach (DataRow dr in oDS.dtColumns.Rows)
                        {
                            sqlStr = " select dbo.DSTempSetCol.ColName from DSTempSetCol where DSTempSetCol.DSTempSetColId=" + Convert.ToInt32(dr["DSTempSetColId"].ToString());
                            arColName[colIndex] = oData.FetchValue(sqlStr).ToString();
                            colIndex++;
                        }
                        string names = "";
                        float custResult = 0;
                        string[] sEquation = custom.Split('#');
                        string colmnId = "", result = "";
                        foreach (var item in sEquation)
                        {
                            Calculate.Calculate oCalc = new Calculate.Calculate();
                            if (item != "")
                            {
                                PreProcessedExpression expResult = oCalc.PreProcessExpression(item);
                                int expCount = expResult.ColumnDatas.Length;
                                for (int indexj = 0; indexj < expCount; indexj++)
                                {
                                    names = "";
                                    for (int i = 0; i < oDS.dtColumns.Rows.Count; i++)
                                    {
                                        if (ht.ContainsKey(oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()))
                                        {
                                            if (!names.Contains(arColName[i]))
                                            {
                                                if (arColName[i].ToUpper() == expResult.ColumnDatas[indexj].ColumnName.Trim())
                                                {
                                                    expResult.ColumnDatas[indexj].Data = new float[oDS.dtSteps.Rows.Count - 1];
                                                    names += arColName[i] + ",";
                                                    expResult.ColumnDatas[indexj].Data = parseFloat(ht[oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString()]);

                                                    result += oDS.dtColumns.Rows[i]["DSTempSetColId"].ToString() + "*";
                                                }
                                            }
                                        }
                                    }
                                }
                                if (expResult != null)
                                {
                                    string exp = oCalc.CreateExpressionToEvaluate(expResult);
                                    string[] tempString = exp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    string[] postfixArray = oCalc.InfixToPostfix(tempString);
                                    custResult = oCalc.EvaluatePostfix(postfixArray);
                                }
                                result += custResult + "|";
                            }


                        }
                        if (result.Length > 1)
                        {
                            result = result.Substring(0, result.Length - 1);
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "scoreForText('" + result + "');", true);

                    }
                }
            }
        }
    }


    bool bSetIOA = false;
    bool bPromptIOA = false;
    bool bStepIOA = false;
    int avgDurationId = 0;
    int totDuraionId = 0;
    int freqId = 0;
    int iSessionNmbr = 0;
    int colCalId = 0;

    protected void checkScore(int StdtSessHdrId, SqlConnection con, SqlTransaction trans)
    {
        oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        oTemp = (ClsTemplateSession)HttpContext.Current.Session["BiweeklySession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        oData = new clsData();
        oDisc = new DiscreteSession();

        iSessionNmbr = oDS.SessNbr;
        int studentid = oSession.StudentId;
        int schoolid = oSession.SchoolId;
        //string result = "";
        SqlDataReader reader = null;
        bool bPrompt = false;
        bool bMultyTchr = false;
        bool bIOA = false;
        bool bRuleStatusIOA = false;
        bool bRuleStatusMultiTchr = false;
        //Input Data

        int sLessonPlanId = 0;
        int iCurrentSetId = 0;
        int iCurrentSetNbr = 0;
        int iCurrentStep = 0;
        string sCurrentPrompt = "";
        string sCurrentLessonPrompt = "";
        string sNextLessonPrompt = "";
        string sLessonPrompt = "";
        bool bPromtHirchy = false;
        bool bSetCompleted = false;
        string sSkillType = "";
        string[] columns = null;
        string[] promptUsed = null;
        string[] LessonpromptUsed = null;
        string[] LessonpromptUsedOther = null;
        //if (oDS.IOAInd == "Y")
        //{
        //    int IOASessHdrId = iSessionNmbr;
        //}
        //else
        {
            //string strQry = " SELECT Hdr.SkillType, ISNULL(MAX(NextSetId),0) NextSetId,ISNULL(MAX(NextSetNmbr),0) NextSetNmbr " +
            //     " ,ISNULL(MAX(NextStepId),0)NextStepId,ISNULL(MAX(NextPromptId),0)NextPromptId" +
            //     " FROM DSTempHdr Hdr LEFT JOIN StdtDSStat Stat  ON Hdr.DSTempHdrId = Stat.DSTempHdrId " +
            //      " WHERE Hdr.DSTempHdrId= " + oTemp.TemplateId + " GROUP BY Hdr.SkillType ";

            //reader = oData.ReturnDataReader(strQry, false);
            //if (reader.Read())
            //{
            //    iCurrentSetId = Convert.ToInt32(reader["NextSetId"]);
            //    iCurrentSetNbr = Convert.ToInt32(reader["NextSetNmbr"]);
            //    iCurrentStep = Convert.ToInt32(reader["NextStepId"]);
            //    sCurrentPrompt = reader["NextPromptId"].ToString();
            //    sSkillType = reader["SkillType"].ToString();
            //}
            //reader.Close();
            DiscreetTrial.InputData discreteInptData = null;
            Chained.InputData chainedInptData = null;
            ArrayList Trials = new ArrayList();
            Dictionary<string, DiscreetTrial.InputData> discreteCols = new Dictionary<string, DiscreetTrial.InputData>();
            Dictionary<string, Chained.InputData> chainedCols = new Dictionary<string, Chained.InputData>();
            Hashtable ht = new Hashtable(); ;
            string TargetPrompt = "0";
            string LessonTargetPrompt = "0";
            string prompt = "-,+";

            string prompts = "-,+";
            string strQry = "";
            ResetIOAStatus(oTemp.TemplateId);

            strQry = "SELECT DSTempSetColId,ColName,ColTypeCd from DSTempSetCol WHERE DSTempHdrId=" + oTemp.TemplateId + " And  SchoolId = " + oSession.SchoolId + "  And ActiveInd='A'";
            DataTable dt = oData.ReturnDataTable(strQry, false);
            int count = dt.Rows.Count;
            int index = 0;
            int loop = 0;
            int freqdureloop = 0;
            bool bStepCountCriteria = false;
            int crntPrmtIndex = 0;

            DiscreetTrial.Result[] sesResult = null;
            Chained.Result[] sesResultchain = null;

            columns = new string[count];
            bool bSetMoveUp = true;
            bool bSetMoveBack = true;
            bool bStepMoveUp = true;
            bool bStepMoveBack = true;
            bool bPromptMoveUp = false;
            bool bPromptMoveBack = false;
            bool bDurationMoveUp = true;
            bool bDurationMoveBack = true;
            bool bpromptColumn = false;
            string CompletionStatusSet = "";

            //Liju
            bool bSetMoveUpIOA = false;
            bool bSetMoveUpMultiTecher = false;
            bool bSetMoveDownIOA = false;
            bool bSetMoveDownMultiTecher = false;
            bool bStepMoveUpIOA = false;
            bool bStepMoveUpMultiTecher = false;
            bool bStepMoveDownIOA = false;
            bool bStepMoveDownMultiTecher = false;
            bool bPromptMoveUpIOA = false;
            bool bPromptMoveUpMultiTecher = false;
            bool bPromptMoveDownIOA = false;
            bool bPromptMoveDownMultiTecher = false;
            bool bTotalDurationMoveUpbIOAReqd = false;
            bool bTotalDurationMoveUpbMultiTchr = false;
            bool bAvgDurationMoveUpbIOAReqd = false;
            bool bAvgDurationMoveUpbMultiTchr = false;
            bool bTotalDurationMoveDownbIOAReqd = false;
            bool bTotalDurationMoveDownbMultiTchr = false;
            bool bAvgDurationMoveDownbIOAReqd = false;
            bool bAvgDurationMoveDownbMultiTchr = false;
            bool bFrequencyMoveUpbIOAReqd = false;
            bool bFrequencyMoveUpbMultiTchr = false;
            bool bFrequencyMoveDownbIOAReqd = false;
            bool bFrequencyMoveDownbMultiTchr = false;
            bool bCustomMoveupIOA = false;
            bool bCustomMovedownIOA = false;
            bool bCustomMoveupMultiTchr = false;
            bool bCustomMovedownMultiTchr = false;

            int nextSet = 0;
            int nextStep = 0;
            foreach (DataRow dr in dt.Rows)
            {
                int set_moveupCount = 0;
                int set_movedownCount = 0;
                int step_moveupCount = 0;
                int step_movedownCount = 0;



                //bSetMoveUp = true;
                //bSetMoveBack = true;
                //bPromptMoveUp = true;
                //bPromptMoveBack = true;
                strQry = " SELECT Hdr.SkillType, Hdr.LessonPlanId, ISNULL(MAX(NextSetId),0) NextSetId,ISNULL(MAX(NextSetNmbr),0) NextSetNmbr " +
                 " ,ISNULL(MAX(NextStepId),0)NextStepId,ISNULL(MAX(NextPromptId),0)NextPromptId" +
                 " FROM DSTempHdr Hdr LEFT JOIN StdtDSStat Stat  ON Hdr.DSTempHdrId = Stat.DSTempHdrId " +
                  " WHERE Hdr.DSTempHdrId= " + oTemp.TemplateId + " GROUP BY Hdr.SkillType, Hdr.LessonPlanId ";

                reader = oData.ReturnDataReaderWithTrans(strQry, false, trans, con);
                if (reader.Read())
                {
                    iCurrentSetId = Convert.ToInt32(reader["NextSetId"]);
                    Session["iCurrentSetId"] = iCurrentSetId;
                    iCurrentSetNbr = Convert.ToInt32(reader["NextSetNmbr"]);
                    iCurrentStep = Convert.ToInt32(reader["NextStepId"]);
                    Session["iCurrentStep"] = iCurrentStep;
                    sCurrentPrompt = reader["NextPromptId"].ToString();
                    sLessonPrompt = reader["NextPromptId"].ToString();
                    Session["sCurrentPrompt"] = sCurrentPrompt;
                    sCurrentLessonPrompt = sCurrentPrompt;
                    sSkillType = reader["SkillType"].ToString();
                    sLessonPlanId = Convert.ToInt32(reader["LessonPlanId"]);
                }
                reader.Close();
                int iColId = Convert.ToInt32(dr["DSTempSetColId"].ToString());
                string sColName = dr["ColName"].ToString();
                string coltypeCode = dr["ColTypeCd"].ToString();
                if (sSkillType == "Chained")
                {
                    //if (chainedInptData == null)
                    chainedInptData = new Chained.InputData();
                    Rules TempRules = new Rules();
                    TempRules = GetSetRules(oTemp.TemplateId, iColId);
                    if (TempRules != null)
                    {
                        set_moveupCount = TempRules.moveup;
                        set_movedownCount = TempRules.movedown;
                        if (TempRules.count > 0)
                        {

                            //Liju
                            bSetMoveUpIOA = bSetMoveUpIOA | TempRules.pctIndMoveUp.bIOARequird | TempRules.pctAccyMoveUp.bIOARequird;
                            bSetMoveUpMultiTecher = bSetMoveUpMultiTecher | TempRules.pctIndMoveUp.bMultiTeacherRequired | TempRules.pctAccyMoveUp.bMultiTeacherRequired;
                            bSetMoveDownIOA = bSetMoveDownIOA | TempRules.pctIndMoveDown.bIOARequird | TempRules.pctAccyMoveDown.bIOARequird;
                            bSetMoveDownMultiTecher = bSetMoveDownMultiTecher | TempRules.pctIndMoveDown.bMultiTeacherRequired | TempRules.pctAccyMoveDown.bMultiTeacherRequired;


                            chainedInptData.PercentAccuracy.BarCondition = TempRules.pctAccyMoveUp.iScoreRequired;
                            chainedInptData.PercentAccuracy.ConsecutiveSuccess = TempRules.pctAccyMoveUp.bConsequetiveIndex;
                            chainedInptData.PercentAccuracy.TotalTrial = TempRules.pctAccyMoveUp.iTotalInstance;
                            chainedInptData.PercentAccuracy.SuccessNeeded = TempRules.pctAccyMoveUp.iTotalCorrectInstance;
                            chainedInptData.PercentAccuracy.bIOAReqd = TempRules.pctAccyMoveUp.bIOARequird;
                            chainedInptData.PercentAccuracy.bMultiTchr = TempRules.pctAccyMoveUp.bMultiTeacherRequired;



                            chainedInptData.IncludeMistrials = TempRules.bIncludeMisTrail;

                            chainedInptData.PercentIndependence.BarCondition = TempRules.pctIndMoveUp.iScoreRequired;
                            chainedInptData.PercentIndependence.ConsecutiveSuccess = TempRules.pctIndMoveUp.bConsequetiveIndex;
                            chainedInptData.PercentIndependence.TotalTrial = TempRules.pctIndMoveUp.iTotalInstance;
                            chainedInptData.PercentIndependence.SuccessNeeded = TempRules.pctIndMoveUp.iTotalCorrectInstance;
                            chainedInptData.PercentIndependence.bIOAReqd = TempRules.pctIndMoveUp.bIOARequird;
                            chainedInptData.PercentIndependence.bMultiTchr = TempRules.pctIndMoveUp.bMultiTeacherRequired;

                            chainedInptData.MoveBackPercentAccuracy.BarCondition = TempRules.pctAccyMoveDown.iScoreRequired;
                            chainedInptData.MoveBackPercentAccuracy.ConsecutiveFailures = TempRules.pctAccyMoveDown.bConsequetiveIndex;
                            chainedInptData.MoveBackPercentAccuracy.TotalTrial = TempRules.pctAccyMoveDown.iTotalInstance;
                            chainedInptData.MoveBackPercentAccuracy.FailureNeeded = TempRules.pctAccyMoveDown.iTotalCorrectInstance;
                            chainedInptData.MoveBackPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveDown.bIOARequird;
                            chainedInptData.MoveBackPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveDown.bMultiTeacherRequired;

                            chainedInptData.CustomPercent.BarCondition = TempRules.pctCustomMoveUp.iScoreRequired;
                            chainedInptData.CustomPercent.ConsecutiveSuccess = TempRules.pctCustomMoveUp.bConsequetiveIndex;
                            chainedInptData.CustomPercent.TotalTrial = TempRules.pctCustomMoveUp.iTotalInstance;
                            chainedInptData.CustomPercent.SuccessNeeded = TempRules.pctCustomMoveUp.iTotalCorrectInstance;
                            chainedInptData.CustomPercent.bIOAReqd = TempRules.pctCustomMoveUp.bIOARequird;
                            chainedInptData.CustomPercent.bMultiTchr = TempRules.pctCustomMoveUp.bMultiTeacherRequired;

                            chainedInptData.MoveBackCustom.BarCondition = TempRules.pctCustomMoveDown.iScoreRequired;
                            chainedInptData.MoveBackCustom.ConsecutiveFailures = TempRules.pctCustomMoveDown.bConsequetiveIndex;
                            chainedInptData.MoveBackCustom.TotalTrial = TempRules.pctCustomMoveDown.iTotalInstance;
                            chainedInptData.MoveBackCustom.FailureNeeded = TempRules.pctCustomMoveDown.iTotalCorrectInstance;
                            chainedInptData.MoveBackCustom.bIOAReqd = TempRules.pctCustomMoveDown.bIOARequird;
                            chainedInptData.MoveBackCustom.bMultiTchr = TempRules.pctCustomMoveDown.bMultiTeacherRequired;

                            chainedInptData.MoveBackPercentIndependence.BarCondition = TempRules.pctIndMoveDown.iScoreRequired;
                            chainedInptData.MoveBackPercentIndependence.ConsecutiveFailures = TempRules.pctIndMoveDown.bConsequetiveIndex;
                            chainedInptData.MoveBackPercentIndependence.TotalTrial = TempRules.pctIndMoveDown.iTotalInstance;
                            chainedInptData.MoveBackPercentIndependence.FailureNeeded = TempRules.pctIndMoveDown.iTotalCorrectInstance;
                            chainedInptData.MoveBackPercentIndependence.bIOAReqd = TempRules.pctIndMoveDown.bIOARequird;
                            chainedInptData.MoveBackPercentIndependence.bMultiTchr = TempRules.pctIndMoveDown.bMultiTeacherRequired;

                            chainedInptData.AvgDurationMoveUp.BarCondition = TempRules.pctAvgDurationMoveUp.iScoreRequired;
                            chainedInptData.AvgDurationMoveUp.ConsecutiveSuccess = TempRules.pctAvgDurationMoveUp.bConsequetiveIndex;
                            chainedInptData.AvgDurationMoveUp.TotalTrial = TempRules.pctAvgDurationMoveUp.iTotalInstance;
                            chainedInptData.AvgDurationMoveUp.SuccessNeeded = TempRules.pctAvgDurationMoveUp.iTotalCorrectInstance;
                            chainedInptData.AvgDurationMoveUp.bIOAReqd = TempRules.pctAvgDurationMoveUp.bIOARequird;
                            chainedInptData.AvgDurationMoveUp.bMultiTchr = TempRules.pctAvgDurationMoveUp.bMultiTeacherRequired;

                            chainedInptData.TotalDurationMoveUp.BarCondition = TempRules.pctTotalDurationMoveUp.iScoreRequired;
                            chainedInptData.TotalDurationMoveUp.ConsecutiveSuccess = TempRules.pctTotalDurationMoveUp.bConsequetiveIndex;
                            chainedInptData.TotalDurationMoveUp.TotalTrial = TempRules.pctTotalDurationMoveUp.iTotalInstance;
                            chainedInptData.TotalDurationMoveUp.SuccessNeeded = TempRules.pctTotalDurationMoveUp.iTotalCorrectInstance;
                            chainedInptData.TotalDurationMoveUp.bIOAReqd = TempRules.pctTotalDurationMoveUp.bIOARequird;
                            chainedInptData.TotalDurationMoveUp.bMultiTchr = TempRules.pctTotalDurationMoveUp.bMultiTeacherRequired;

                            chainedInptData.FrequencyMoveUp.BarCondition = TempRules.pctFrequencyMoveUp.iScoreRequired;
                            chainedInptData.FrequencyMoveUp.ConsecutiveSuccess = TempRules.pctFrequencyMoveUp.bConsequetiveIndex;
                            chainedInptData.FrequencyMoveUp.TotalTrial = TempRules.pctFrequencyMoveUp.iTotalInstance;
                            chainedInptData.FrequencyMoveUp.SuccessNeeded = TempRules.pctFrequencyMoveUp.iTotalCorrectInstance;
                            chainedInptData.FrequencyMoveUp.bIOAReqd = TempRules.pctFrequencyMoveUp.bIOARequird;
                            chainedInptData.FrequencyMoveUp.bMultiTchr = TempRules.pctFrequencyMoveUp.bMultiTeacherRequired;

                            chainedInptData.AvgDurationMoveDown.BarCondition = TempRules.pctAvgDurationMoveDown.iScoreRequired;
                            chainedInptData.AvgDurationMoveDown.ConsecutiveFailures = TempRules.pctAvgDurationMoveDown.bConsequetiveIndex;
                            chainedInptData.AvgDurationMoveDown.TotalTrial = TempRules.pctAvgDurationMoveDown.iTotalInstance;
                            chainedInptData.AvgDurationMoveDown.FailureNeeded = TempRules.pctAvgDurationMoveDown.iTotalCorrectInstance;
                            chainedInptData.AvgDurationMoveDown.bIOAReqd = TempRules.pctAvgDurationMoveDown.bIOARequird;
                            chainedInptData.AvgDurationMoveDown.bMultiTchr = TempRules.pctAvgDurationMoveDown.bMultiTeacherRequired;

                            chainedInptData.TotalDurationMoveDown.BarCondition = TempRules.pctTotalDurationMoveDown.iScoreRequired;
                            chainedInptData.TotalDurationMoveDown.ConsecutiveFailures = TempRules.pctTotalDurationMoveDown.bConsequetiveIndex;
                            chainedInptData.TotalDurationMoveDown.TotalTrial = TempRules.pctTotalDurationMoveDown.iTotalInstance;
                            chainedInptData.TotalDurationMoveDown.FailureNeeded = TempRules.pctTotalDurationMoveDown.iTotalCorrectInstance;
                            chainedInptData.TotalDurationMoveDown.bIOAReqd = TempRules.pctTotalDurationMoveDown.bIOARequird;
                            chainedInptData.TotalDurationMoveDown.bMultiTchr = TempRules.pctTotalDurationMoveDown.bMultiTeacherRequired;

                            chainedInptData.FrequencyMoveDown.BarCondition = TempRules.pctFrequencyMoveDown.iScoreRequired;
                            chainedInptData.FrequencyMoveDown.ConsecutiveFailures = TempRules.pctFrequencyMoveDown.bConsequetiveIndex;
                            chainedInptData.FrequencyMoveDown.TotalTrial = TempRules.pctFrequencyMoveDown.iTotalInstance;
                            chainedInptData.FrequencyMoveDown.FailureNeeded = TempRules.pctFrequencyMoveDown.iTotalCorrectInstance;
                            chainedInptData.FrequencyMoveDown.bIOAReqd = TempRules.pctFrequencyMoveDown.bIOARequird;
                            chainedInptData.FrequencyMoveDown.bMultiTchr = TempRules.pctFrequencyMoveDown.bMultiTeacherRequired;

                            chainedInptData.SetLearnedStepMoveUp.BarCondition = TempRules.SetlearnedStepMoveUp.iScoreRequired;
                            chainedInptData.SetLearnedStepMoveUp.ConsecutiveSuccess = TempRules.SetlearnedStepMoveUp.bConsequetiveIndex;
                            chainedInptData.SetLearnedStepMoveUp.TotalTrial = TempRules.SetlearnedStepMoveUp.iTotalInstance;
                            chainedInptData.SetLearnedStepMoveUp.SuccessNeeded = TempRules.SetlearnedStepMoveUp.iTotalCorrectInstance;
                            chainedInptData.SetLearnedStepMoveUp.bIOAReqd = TempRules.SetlearnedStepMoveUp.bIOARequird;
                            chainedInptData.SetLearnedStepMoveUp.bMultiTchr = TempRules.SetlearnedStepMoveUp.bMultiTeacherRequired;

                            chainedInptData.SetLearnedStepMoveBack.BarCondition = TempRules.SetlearnedStepMoveDown.iScoreRequired;
                            chainedInptData.SetLearnedStepMoveBack.ConsecutiveFailures = TempRules.SetlearnedStepMoveDown.bConsequetiveIndex;
                            chainedInptData.SetLearnedStepMoveBack.TotalTrial = TempRules.SetlearnedStepMoveDown.iTotalInstance;
                            chainedInptData.SetLearnedStepMoveBack.FailureNeeded = TempRules.SetlearnedStepMoveDown.iTotalCorrectInstance;
                            chainedInptData.SetLearnedStepMoveBack.bIOAReqd = TempRules.SetlearnedStepMoveDown.bIOARequird;
                            chainedInptData.SetLearnedStepMoveBack.bMultiTchr = TempRules.SetlearnedStepMoveDown.bMultiTeacherRequired;

                            //chainedInptData.IOARequired = TempRules.bIOARequird;
                            //chainedInptData.MultiTeacherRequired = TempRules.bMultiTeacherRequired;
                        }
                    }
                    TempRules = new Rules();
                    TempRules = GetPromptRules(oTemp.TemplateId, Convert.ToInt32(dr["DSTempSetColId"].ToString()));
                    if (TempRules != null)
                        if (TempRules.count > 0)
                        {
                            bPrompt = true;
                            chainedInptData.PromptHirecharchy = true;
                            //Liju
                            bPromptMoveUpIOA = bPromptMoveUpIOA | TempRules.pctIndMoveUp.bIOARequird | TempRules.pctAccyMoveUp.bIOARequird;
                            bPromptMoveUpMultiTecher = bPromptMoveUpMultiTecher | TempRules.pctIndMoveUp.bMultiTeacherRequired | TempRules.pctAccyMoveUp.bMultiTeacherRequired;
                            bPromptMoveDownIOA = bPromptMoveDownIOA | TempRules.pctIndMoveDown.bIOARequird | TempRules.pctAccyMoveDown.bIOARequird;
                            bPromptMoveDownMultiTecher = bPromptMoveDownMultiTecher | TempRules.pctIndMoveDown.bMultiTeacherRequired | TempRules.pctAccyMoveDown.bMultiTeacherRequired;


                            chainedInptData.PromptPercentAccuracy.BarCondition = TempRules.pctAccyMoveUp.iScoreRequired;
                            chainedInptData.PromptPercentAccuracy.ConsecutiveSuccess = TempRules.pctAccyMoveUp.bConsequetiveIndex;
                            chainedInptData.PromptPercentAccuracy.TotalTrial = TempRules.pctAccyMoveUp.iTotalInstance;
                            chainedInptData.PromptPercentAccuracy.SuccessNeeded = TempRules.pctAccyMoveUp.iTotalCorrectInstance;
                            chainedInptData.PromptPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveUp.bIOARequird;
                            chainedInptData.PromptPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveUp.bMultiTeacherRequired;

                            chainedInptData.IncludeMistrials = TempRules.bIncludeMisTrail;

                            chainedInptData.PromptPercentIndependence.BarCondition = TempRules.pctIndMoveUp.iScoreRequired;
                            chainedInptData.PromptPercentIndependence.ConsecutiveSuccess = TempRules.pctIndMoveUp.bConsequetiveIndex;
                            chainedInptData.PromptPercentIndependence.TotalTrial = TempRules.pctIndMoveUp.iTotalInstance;
                            chainedInptData.PromptPercentIndependence.SuccessNeeded = TempRules.pctIndMoveUp.iTotalCorrectInstance;
                            chainedInptData.PromptPercentIndependence.bIOAReqd = TempRules.pctIndMoveUp.bIOARequird;
                            chainedInptData.PromptPercentIndependence.bMultiTchr = TempRules.pctIndMoveUp.bMultiTeacherRequired;

                            chainedInptData.MoveBackPromptPercentAccuracy.BarCondition = TempRules.pctAccyMoveDown.iScoreRequired;
                            chainedInptData.MoveBackPromptPercentAccuracy.ConsecutiveFailures = TempRules.pctAccyMoveDown.bConsequetiveIndex;
                            chainedInptData.MoveBackPromptPercentAccuracy.TotalTrial = TempRules.pctAccyMoveDown.iTotalInstance;
                            chainedInptData.MoveBackPromptPercentAccuracy.FailureNeeded = TempRules.pctAccyMoveDown.iTotalCorrectInstance;
                            chainedInptData.MoveBackPromptPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveDown.bIOARequird;
                            chainedInptData.MoveBackPromptPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveDown.bMultiTeacherRequired;

                            chainedInptData.MoveBackPromptPercentIndependence.BarCondition = TempRules.pctIndMoveDown.iScoreRequired;
                            chainedInptData.MoveBackPromptPercentIndependence.ConsecutiveFailures = TempRules.pctIndMoveDown.bConsequetiveIndex;
                            chainedInptData.MoveBackPromptPercentIndependence.TotalTrial = TempRules.pctIndMoveDown.iTotalInstance;
                            chainedInptData.MoveBackPromptPercentIndependence.FailureNeeded = TempRules.pctIndMoveDown.iTotalCorrectInstance;
                            chainedInptData.MoveBackPromptPercentIndependence.bIOAReqd = TempRules.pctIndMoveDown.bIOARequird;
                            chainedInptData.MoveBackPromptPercentIndependence.bMultiTchr = TempRules.pctIndMoveDown.bMultiTeacherRequired;


                            chainedInptData.PromptExcludeCrntStepMoveUp.BarCondition = TempRules.PromptExcludeCrntStepMoveUp.iScoreRequired;
                            chainedInptData.PromptExcludeCrntStepMoveUp.ConsecutiveSuccess = TempRules.PromptExcludeCrntStepMoveUp.bConsequetiveIndex;
                            chainedInptData.PromptExcludeCrntStepMoveUp.TotalTrial = TempRules.PromptExcludeCrntStepMoveUp.iTotalInstance;
                            chainedInptData.PromptExcludeCrntStepMoveUp.SuccessNeeded = TempRules.PromptExcludeCrntStepMoveUp.iTotalCorrectInstance;
                            chainedInptData.PromptExcludeCrntStepMoveUp.bIOAReqd = TempRules.PromptExcludeCrntStepMoveUp.bIOARequird;
                            chainedInptData.PromptExcludeCrntStepMoveUp.bMultiTchr = TempRules.PromptExcludeCrntStepMoveUp.bMultiTeacherRequired;

                            chainedInptData.PromptExcludeCrntStepMoveBack.BarCondition = TempRules.PromptExcludeCrntStepMoveDown.iScoreRequired;
                            chainedInptData.PromptExcludeCrntStepMoveBack.ConsecutiveFailures = TempRules.PromptExcludeCrntStepMoveDown.bConsequetiveIndex;
                            chainedInptData.PromptExcludeCrntStepMoveBack.TotalTrial = TempRules.PromptExcludeCrntStepMoveDown.iTotalInstance;
                            chainedInptData.PromptExcludeCrntStepMoveBack.FailureNeeded = TempRules.PromptExcludeCrntStepMoveDown.iTotalCorrectInstance;
                            chainedInptData.PromptExcludeCrntStepMoveBack.bIOAReqd = TempRules.PromptExcludeCrntStepMoveDown.bIOARequird;
                            chainedInptData.PromptExcludeCrntStepMoveBack.bMultiTchr = TempRules.PromptExcludeCrntStepMoveDown.bMultiTeacherRequired;

                            chainedInptData.PromptLearnedStepMoveUp.BarCondition = TempRules.PromptlearnedStepMoveUp.iScoreRequired;
                            chainedInptData.PromptLearnedStepMoveUp.ConsecutiveSuccess = TempRules.PromptlearnedStepMoveUp.bConsequetiveIndex;
                            chainedInptData.PromptLearnedStepMoveUp.TotalTrial = TempRules.PromptlearnedStepMoveUp.iTotalInstance;
                            chainedInptData.PromptLearnedStepMoveUp.SuccessNeeded = TempRules.PromptlearnedStepMoveUp.iTotalCorrectInstance;
                            chainedInptData.PromptLearnedStepMoveUp.bIOAReqd = TempRules.PromptlearnedStepMoveUp.bIOARequird;
                            chainedInptData.PromptLearnedStepMoveUp.bMultiTchr = TempRules.PromptlearnedStepMoveUp.bMultiTeacherRequired;

                            chainedInptData.PromptLearnedStepMoveBack.BarCondition = TempRules.PromptlearnedStepMoveDown.iScoreRequired;
                            chainedInptData.PromptLearnedStepMoveBack.ConsecutiveFailures = TempRules.PromptlearnedStepMoveDown.bConsequetiveIndex;
                            chainedInptData.PromptLearnedStepMoveBack.TotalTrial = TempRules.PromptlearnedStepMoveDown.iTotalInstance;
                            chainedInptData.PromptLearnedStepMoveBack.FailureNeeded = TempRules.PromptlearnedStepMoveDown.iTotalCorrectInstance;
                            chainedInptData.PromptLearnedStepMoveBack.bIOAReqd = TempRules.PromptlearnedStepMoveDown.bIOARequird;
                            chainedInptData.PromptLearnedStepMoveBack.bMultiTchr = TempRules.PromptlearnedStepMoveDown.bMultiTeacherRequired;
                            /*if (!chainedInptData.IOARequired)
                                chainedInptData.IOARequired = TempRules.bIOARequird;
                            if (!chainedInptData.MultiTeacherRequired)
                                chainedInptData.MultiTeacherRequired = TempRules.bMultiTeacherRequired;*/
                        }
                    TempRules = new Rules();
                    TempRules = GetStepRules(oTemp.TemplateId, iColId);
                    if (TempRules != null)
                    {
                        step_moveupCount = TempRules.moveup;
                        step_movedownCount = TempRules.movedown;
                        if (TempRules.count > 0)
                        {

                            //Liju
                            bStepMoveUpIOA = bStepMoveUpIOA | TempRules.pctIndMoveUp.bIOARequird | TempRules.pctAccyMoveUp.bIOARequird;
                            bStepMoveUpMultiTecher = bStepMoveUpMultiTecher | TempRules.pctIndMoveUp.bMultiTeacherRequired | TempRules.pctAccyMoveUp.bMultiTeacherRequired;
                            bStepMoveDownIOA = bStepMoveDownIOA | TempRules.pctIndMoveDown.bIOARequird | TempRules.pctAccyMoveDown.bIOARequird;
                            bStepMoveDownMultiTecher = bStepMoveDownMultiTecher | TempRules.pctIndMoveDown.bMultiTeacherRequired | TempRules.pctAccyMoveDown.bMultiTeacherRequired;


                            chainedInptData.StepPercentAccuracy.BarCondition = TempRules.pctAccyMoveUp.iScoreRequired;
                            chainedInptData.StepPercentAccuracy.ConsecutiveSuccess = TempRules.pctAccyMoveUp.bConsequetiveIndex;
                            chainedInptData.StepPercentAccuracy.TotalTrial = TempRules.pctAccyMoveUp.iTotalInstance;
                            chainedInptData.StepPercentAccuracy.SuccessNeeded = TempRules.pctAccyMoveUp.iTotalCorrectInstance;
                            chainedInptData.StepPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveUp.bIOARequird;
                            chainedInptData.StepPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveUp.bMultiTeacherRequired;

                            chainedInptData.IncludeMistrials = TempRules.bIncludeMisTrail;

                            chainedInptData.StepPercentIndependence.BarCondition = TempRules.pctIndMoveUp.iScoreRequired;
                            chainedInptData.StepPercentIndependence.ConsecutiveSuccess = TempRules.pctIndMoveUp.bConsequetiveIndex;
                            chainedInptData.StepPercentIndependence.TotalTrial = TempRules.pctIndMoveUp.iTotalInstance;
                            chainedInptData.StepPercentIndependence.SuccessNeeded = TempRules.pctIndMoveUp.iTotalCorrectInstance;
                            chainedInptData.StepPercentIndependence.bIOAReqd = TempRules.pctIndMoveUp.bIOARequird;
                            chainedInptData.StepPercentIndependence.bMultiTchr = TempRules.pctIndMoveUp.bMultiTeacherRequired;

                            chainedInptData.StepMoveBackPercentAccuracy.BarCondition = TempRules.pctAccyMoveDown.iScoreRequired;
                            chainedInptData.StepMoveBackPercentAccuracy.ConsecutiveFailures = TempRules.pctAccyMoveDown.bConsequetiveIndex;
                            chainedInptData.StepMoveBackPercentAccuracy.TotalTrial = TempRules.pctAccyMoveDown.iTotalInstance;
                            chainedInptData.StepMoveBackPercentAccuracy.FailureNeeded = TempRules.pctAccyMoveDown.iTotalCorrectInstance;
                            chainedInptData.StepMoveBackPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveDown.bIOARequird;
                            chainedInptData.StepMoveBackPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveDown.bMultiTeacherRequired;

                            chainedInptData.StepMoveBackPercentIndependence.BarCondition = TempRules.pctIndMoveDown.iScoreRequired;
                            chainedInptData.StepMoveBackPercentIndependence.ConsecutiveFailures = TempRules.pctIndMoveDown.bConsequetiveIndex;
                            chainedInptData.StepMoveBackPercentIndependence.TotalTrial = TempRules.pctIndMoveDown.iTotalInstance;
                            chainedInptData.StepMoveBackPercentIndependence.FailureNeeded = TempRules.pctIndMoveDown.iTotalCorrectInstance;
                            chainedInptData.StepMoveBackPercentIndependence.bIOAReqd = TempRules.pctIndMoveDown.bIOARequird;
                            chainedInptData.StepMoveBackPercentIndependence.bMultiTchr = TempRules.pctIndMoveDown.bMultiTeacherRequired;

                            chainedInptData.LearnedStepMoveUp.BarCondition = TempRules.learnedStepMoveUp.iScoreRequired;
                            chainedInptData.LearnedStepMoveUp.ConsecutiveSuccess = TempRules.learnedStepMoveUp.bConsequetiveIndex;
                            chainedInptData.LearnedStepMoveUp.TotalTrial = TempRules.learnedStepMoveUp.iTotalInstance;
                            chainedInptData.LearnedStepMoveUp.SuccessNeeded = TempRules.learnedStepMoveUp.iTotalCorrectInstance;
                            chainedInptData.LearnedStepMoveUp.bIOAReqd = TempRules.learnedStepMoveUp.bIOARequird;
                            chainedInptData.LearnedStepMoveUp.bMultiTchr = TempRules.learnedStepMoveUp.bMultiTeacherRequired;

                            chainedInptData.LearnedStepMoveBack.BarCondition = TempRules.learnedStepMoveDown.iScoreRequired;
                            chainedInptData.LearnedStepMoveBack.ConsecutiveFailures = TempRules.learnedStepMoveDown.bConsequetiveIndex;
                            chainedInptData.LearnedStepMoveBack.TotalTrial = TempRules.learnedStepMoveDown.iTotalInstance;
                            chainedInptData.LearnedStepMoveBack.FailureNeeded = TempRules.learnedStepMoveDown.iTotalCorrectInstance;
                            chainedInptData.LearnedStepMoveBack.bIOAReqd = TempRules.learnedStepMoveDown.bIOARequird;
                            chainedInptData.LearnedStepMoveBack.bMultiTchr = TempRules.learnedStepMoveDown.bMultiTeacherRequired;


                            chainedInptData.ExcludeCrntStepMoveUp.BarCondition = TempRules.excludeCrntStepMoveUp.iScoreRequired;
                            chainedInptData.ExcludeCrntStepMoveUp.ConsecutiveSuccess = TempRules.excludeCrntStepMoveUp.bConsequetiveIndex;
                            chainedInptData.ExcludeCrntStepMoveUp.TotalTrial = TempRules.excludeCrntStepMoveUp.iTotalInstance;
                            chainedInptData.ExcludeCrntStepMoveUp.SuccessNeeded = TempRules.excludeCrntStepMoveUp.iTotalCorrectInstance;
                            chainedInptData.ExcludeCrntStepMoveUp.bIOAReqd = TempRules.excludeCrntStepMoveUp.bIOARequird;
                            chainedInptData.ExcludeCrntStepMoveUp.bMultiTchr = TempRules.excludeCrntStepMoveUp.bMultiTeacherRequired;

                            chainedInptData.ExcludeCrntStepMoveBack.BarCondition = TempRules.excludeCrntStepMoveDown.iScoreRequired;
                            chainedInptData.ExcludeCrntStepMoveBack.ConsecutiveFailures = TempRules.excludeCrntStepMoveDown.bConsequetiveIndex;
                            chainedInptData.ExcludeCrntStepMoveBack.TotalTrial = TempRules.excludeCrntStepMoveDown.iTotalInstance;
                            chainedInptData.ExcludeCrntStepMoveBack.FailureNeeded = TempRules.excludeCrntStepMoveDown.iTotalCorrectInstance;
                            chainedInptData.ExcludeCrntStepMoveBack.bIOAReqd = TempRules.excludeCrntStepMoveDown.bIOARequird;
                            chainedInptData.ExcludeCrntStepMoveBack.bMultiTchr = TempRules.excludeCrntStepMoveDown.bMultiTeacherRequired;




                            /*if (!chainedInptData.IOARequired)
                                chainedInptData.IOARequired = TempRules.bIOARequird;
                            if (!chainedInptData.MultiTeacherRequired)
                                chainedInptData.MultiTeacherRequired = TempRules.bMultiTeacherRequired;*/
                        }
                    }
                    chainedCols.Add(sColName, chainedInptData);
                    //if (chainedInptData.PromptHirecharchy == false)
                    //{
                    //    prompt = "-,+";
                    //    //promptUsed = prompt.Split(',');
                    //    sCurrentPrompt = "+";
                    //    TargetPrompt = "+";
                    //    promptUsed = new string[1];
                    //    promptUsed[0] = "+";

                    //}
                    //else
                    //{
                    Prompt[] arPromtList = GetPrompts(oTemp.TemplateId);
                    promptUsed = new string[arPromtList.Count()];
                    LessonpromptUsed = new string[arPromtList.Count()];
                    LessonpromptUsedOther = new string[arPromtList.Count()];
                    bPromtHirchy = true;
                    for (int iCount = 0; iCount < arPromtList.Count(); iCount++)
                    {
                        promptUsed[iCount] = arPromtList[iCount].promptId.ToString();
                        LessonpromptUsed[iCount] = arPromtList[iCount].promptId.ToString();
                        LessonpromptUsedOther[iCount] = arPromtList[iCount].promptId.ToString();
                        if (!String.IsNullOrEmpty(sCurrentLessonPrompt) && sCurrentLessonPrompt != "0")
                        {
                            if (arPromtList[iCount].promptId.ToString() == sCurrentLessonPrompt)
                                crntPrmtIndex = iCount;
                        }
                    }

                    if (!chainedInptData.PromptHirecharchy)
                        bPromtHirchy = false;


                    if (promptUsed.Length > 0)
                    {
                        TargetPrompt = promptUsed[promptUsed.Length - 1];
                        LessonTargetPrompt = TargetPrompt;

                        if (String.IsNullOrEmpty(sCurrentLessonPrompt) || sCurrentLessonPrompt == "0")
                        {
                            sCurrentPrompt = promptUsed[0];
                            Session["sCurrentPrompt"] = sCurrentPrompt;
                            sCurrentLessonPrompt = sCurrentPrompt;
                        }
                    }
                    //}

                    if (dr["ColTypeCd"].ToString() == "+/-")
                    {
                        prompt = "-,+";
                        //promptUsed = prompt.Split(',');
                        sCurrentPrompt = "+";
                        TargetPrompt = "+";
                        promptUsed = new string[1];
                        promptUsed[0] = "+";
                    }
                    if (dr["ColTypeCd"].ToString() == "Prompt")
                    {
                        bpromptColumn = true;
                    }
                    DiscreteTrials TrialLists = new DiscreteTrials();
                    reader.Close();
                    int counter = chainedCols.Count;
                    int ind = 0;


                    oDisc = new DiscreteSession();
                    TrialLists = oDisc.GetTrialLists(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iCurrentStep, chainedCols[sColName].RequiredSession(), sColName, false, hfPlusMinusResp.Value, coltypeCode, oDS.ChainType, trans, con);

                    //if (chainedInptData.MultiTeacherRequired)
                    bMultyTchr = oDisc.MultiTeacherStatus(oSession.StudentId, oTemp.TemplateId, trans, con);
                    // else
                    //    bMultyTchr = false;

                    // if (chainedInptData.IOARequired)
                    bIOA = oDisc.IOAStats(oSession.StudentId, oTemp.TemplateId, trans, con);
                    // else
                    //    bIOA = false;
                    //Trials = trails.GetTrialLists(8, 1, ht[key].RequiredSession(), key);
                    string stepValue = TrialLists.value;
                    int reqSess = chainedCols[sColName].RequiredSession();
                    chainedCols[sColName].SessionCount = TrialLists.sessionCount;
                    chainedCols[sColName].StepCount = TrialLists.trialsCount;
                    chainedCols[sColName].PromptsUsed = promptUsed;
                    chainedCols[sColName].TotalSets = TrialLists.totalSet;
                    chainedCols[sColName].NoPromptsUsed = LessonpromptUsed;
                    chainedCols[sColName].sCurrentLessonPrompt = sCurrentLessonPrompt;
                    chainedCols[sColName].StepPrompts = oDisc.GetStepPrompts(Convert.ToInt32(dr["DSTempSetColId"].ToString()), StdtSessHdrId, trans, con);

                    string crctResponse = "+";
                    if ((hfPlusMinusResp.Value == "+") || (hfPlusMinusResp.Value == "-"))
                    {
                        crctResponse = hfPlusMinusResp.Value;
                    }
                    chainedCols[sColName].CorrectResp = crctResponse;

                    int iTrailCount = TrialLists.trialsCount;
                    string sEventType = "";
                    bool bcustMoveUp = false;
                    bool bCustMoveDown = false;
                    bool bTotDurationMoveUp = true;
                    bool bAvgDurationMoveUp = true;
                    bool bFrequencyMoveUp = true;
                    bool bTotDurationMoveDown = true;
                    bool bAvgDurationMoveDown = true;
                    bool bFrequencyMoveDown = true;
                    if (chainedCols[sColName].StepCount == 0)
                    {
                        return;
                    }
                    if (chainedCols[sColName].StepCount > 0)
                    {
                        sesResultchain = new Chained.Result[chainedCols.Count];
                        if (dr["ColTypeCd"].ToString() == "Text")
                        {
                            bool status = false;
                            TempRules = GetSetRules(oTemp.TemplateId, iColId);
                            colCalId = Convert.ToInt32(ViewState["colCalId"]);
                            if (TempRules.pctCustomMoveUp.iScoreRequired > 0)
                            {
                                bcustMoveUp = ValidateUp(oSession.StudentId, colCalId, TempRules.pctCustomMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctCustomMoveUp.iTotalCorrectInstance, TempRules.pctCustomMoveUp.bConsequetiveIndex, status);
                            }
                            if (TempRules.pctCustomMoveDown.iScoreRequired > 0)
                            {
                                bCustMoveDown = ValidateDown(oSession.StudentId, colCalId, TempRules.pctCustomMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctCustomMoveDown.iTotalCorrectInstance, TempRules.pctCustomMoveDown.bConsequetiveIndex, status);
                            }
                        }
                        else if (dr["ColTypeCd"].ToString() == "Duration")
                        {
                            bool status = true;
                            TempRules = GetSetRules(oTemp.TemplateId, iColId);
                            avgDurationId = Convert.ToInt32(ViewState["avgDurationId"]);
                            totDuraionId = Convert.ToInt32(ViewState["totDuraionId"]);
                            if (TempRules.pctAvgDurationMoveUp.iScoreRequired > 0)
                            {
                                bAvgDurationMoveUp = ValidateUp(oSession.StudentId, avgDurationId, TempRules.pctAvgDurationMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctAvgDurationMoveUp.iTotalCorrectInstance, TempRules.pctAvgDurationMoveUp.bConsequetiveIndex, status);
                            }
                            if (TempRules.pctAvgDurationMoveDown.iScoreRequired > 0)
                            {
                                bAvgDurationMoveDown = ValidateDown(oSession.StudentId, avgDurationId, TempRules.pctAvgDurationMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctAvgDurationMoveDown.iTotalCorrectInstance, TempRules.pctAvgDurationMoveDown.bConsequetiveIndex, status);
                            }
                            if (TempRules.pctTotalDurationMoveUp.iScoreRequired > 0)
                            {
                                bTotDurationMoveUp = ValidateUp(oSession.StudentId, totDuraionId, TempRules.pctTotalDurationMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctTotalDurationMoveUp.iTotalCorrectInstance, TempRules.pctTotalDurationMoveUp.bConsequetiveIndex, status);
                            }
                            if (TempRules.pctTotalDurationMoveDown.iScoreRequired > 0)
                            {
                                bTotDurationMoveDown = ValidateDown(oSession.StudentId, totDuraionId, TempRules.pctTotalDurationMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctTotalDurationMoveDown.iTotalCorrectInstance, TempRules.pctTotalDurationMoveDown.bConsequetiveIndex, status);
                            }
                            if ((bAvgDurationMoveUp == false) || (bTotDurationMoveUp == false))
                            {
                                bDurationMoveUp = false;
                            }
                            if ((bAvgDurationMoveDown == false) || (bTotDurationMoveDown == false))
                            {
                                bDurationMoveBack = false;
                            }

                        }
                        else if (dr["ColTypeCd"].ToString() == "Frequency")
                        {
                            bool status = false;
                            TempRules = GetSetRules(oTemp.TemplateId, iColId);
                            freqId = Convert.ToInt32(ViewState["freqId"]);
                            if (TempRules.pctFrequencyMoveUp.iScoreRequired > 0)
                            {
                                bFrequencyMoveUp = ValidateUp(oSession.StudentId, freqId, TempRules.pctFrequencyMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctFrequencyMoveUp.iTotalCorrectInstance, TempRules.pctFrequencyMoveUp.bConsequetiveIndex, status);
                            }
                            if (TempRules.pctFrequencyMoveDown.iScoreRequired > 0)
                            {
                                bFrequencyMoveDown = ValidateDown(oSession.StudentId, freqId, TempRules.pctFrequencyMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctFrequencyMoveDown.iTotalCorrectInstance, TempRules.pctFrequencyMoveDown.bConsequetiveIndex, status);
                            }

                        }

                        if (oDS.ChainType == "Total Task")
                        {
                            bool stepLevelPrompt = false;
                            if (Session["StepLevelPrompt"] != null && Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                            {
                                stepLevelPrompt = true;
                            }
                            bStepMoveUp = false; bStepMoveBack = false;
                            //if (chainedCols[sColName].SessionCount >= chainedCols[sColName].RequiredSession())
                            //{
                            chainedCols[sColName].TotalTaskMode = true;
                            chainedCols[sColName].SetInputData(iCurrentStep.ToString(), sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                            sesResultchain[index] = Chained.Model.ExecuteForTotalTask(chainedCols[sColName], stepLevelPrompt, bpromptColumn);
                            bRuleStatusIOA = oDisc.checkConditionIOA(chainedInptData.IOARequired, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(chainedInptData.MultiTeacherRequired, bMultyTchr);
                            if (sesResultchain[index].StepPrompts != null)
                            {
                                DataTable dtstepIDs = oData.ReturnDataTable("SELECT DSTempStepId FROM StdtSessionStep WHERE StdtSessionHdrId=" + StdtSessHdrId, false);
                                int indexB = 0;
                                foreach (DataRow drstepID in dtstepIDs.Rows)
                                {
                                    string updStepStat = "UPDATE StdtDSStepStat SET PromptId=" + sesResultchain[index].StepPrompts[indexB] + ",ModifiedBy=" + oSession.LoginId + " ,ModifiedOn=GETDATE() WHERE " +
                                        "DSTempSetColId=" + dr["DSTempSetColId"].ToString() + " AND DSTempStepId=" + drstepID["DSTempStepId"].ToString() + "";
                                    oData.Execute(updStepStat);
                                    indexB++;
                                }
                            }
                            //}
                        }
                        else
                        {
                            ////////////////////////// New Step level criteria execution////////////////////// Strats Here ///// Arun M//////
                            #region start New Step level criteria execution

                            int iCurrentStepExecuting = Convert.ToInt32(iCurrentStep);
                            bool bPrevStepFailed = false;
                            //Check for all previous step that it all succeeds to move forward
                            for (int istep = 1; istep < iCurrentStepExecuting; istep++)
                            {
                                TrialLists = oDisc.GetTrialListsForPreStep(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, istep, chainedCols[sColName].RequiredSession(), sColName, false, hfPlusMinusResp.Value, coltypeCode, oDS.ChainType, trans, con);
                                chainedCols[sColName].SessionCount = TrialLists.sessionCount;
                                sesResultchain[index] = null;
                                chainedCols[sColName].SetInputData(istep.ToString(), TargetPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                                sesResultchain[index] = Chained.Model.Execute(chainedCols[sColName], true, bpromptColumn, oDS.ChainType);

                                //inputData.SetInputData(istep.ToString(), TargetPrompt, TargetPrompt, CurrentSet, TotalSets, Trials);
                                //res = Model.Execute(inputData);

                                //if it dosent move forward then we need to fall back to this step


                                // if (TrialLists.sessionCount == chainedCols[sColName].RequiredSession())
                                // {
                                if (sesResultchain[index].MovedBackStep || sesResultchain[index].MovedBackPrompt)
                                {
                                    sesResultchain[index].MovedBackPrompt = false;
                                    sesResultchain[index].MovedForwardPrompt = false;
                                    sesResultchain[index].MovedBackStep = false;
                                    sesResultchain[index].MovedForwardStep = false;
                                    sesResultchain[index].MovedBackSet = false;
                                    sesResultchain[index].MovedForwardSet = false;
                                    sesResultchain[index].MoveForwardPromptStep = false;
                                    sesResultchain[index].MoveBackPromptStep = false;

                                    if (!bStepCountCriteria)
                                    {
                                        bPrevStepFailed = true;


                                        //########
                                        //Move Back Current Step to istep
                                        int iPrompts = 0;
                                        string sEventAlertStatu = "";

                                        //bRuleStatusIOA = oDisc.checkConditionIOA(chainedInptData.IOARequired, bIOA);
                                        // bRuleStatusMultiTchr = oDisc.checkConditionIOA(chainedInptData.MultiTeacherRequired, bMultyTchr);
                                        if (sesResultchain[0] != null)
                                        {
                                            //if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                            //{

                                            DataTable dtModificatn = GetModificationDetails("STEP", oTemp.TemplateId);
                                            if (dtModificatn != null)
                                            {
                                                if (dtModificatn.Rows.Count > 0)
                                                {
                                                    bool mod_flag = CheckStepModification(Convert.ToInt32(dtModificatn.Rows[0]["ModificationRule"].ToString()), oTemp.TemplateId, iCurrentSetId, iCurrentStep);

                                                    if (mod_flag)
                                                    {
                                                        oData = new clsData();
                                                        string selqry = "SELECT ISNULL(ModificationInd, 0) as ModificationInd FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                        object mod = oData.FetchValue(selqry);
                                                        if (mod != null)
                                                        {
                                                            if (Convert.ToBoolean(mod) != true)
                                                            {
                                                                string insqry = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
                                                                "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'MODIFICATION',GETDATE()," + iSessionNmbr + ",'EV')";
                                                                oData.ExecuteWithTrans(insqry, con, trans);

                                                                string updqry = "UPDATE DSTempHdr SET ModificationInd=1,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                                oData.ExecuteWithTrans(updqry, con, trans);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (bPrompt)
                                            {
                                                if (oDS.PromptProc != "NA")
                                                {
                                                    if ((oDS.PromptProc == "Least-to-Most") || (oDS.PromptProc == "Graduated Guidance"))
                                                    {
                                                        iPrompts = Convert.ToInt32(LessonpromptUsed[LessonpromptUsed.Length - 1]);
                                                        sesResultchain[0].NextPrompt = iPrompts.ToString();
                                                    }
                                                    else
                                                    {
                                                        iPrompts = Convert.ToInt32(LessonpromptUsed[0]);

                                                    }
                                                }
                                            }
                                            string strQuery = "Select DSTempStepId from DSTempStep where  SortOrder= " + istep + " and DSTempSetId=" + oDS.CrntSet + " AND IsDynamic=0 and DSTempHdrId=" + oTemp.TemplateId;
                                            int retunID = Convert.ToInt32(oData.FetchValue(strQuery));
                                            sEventType = "STEP MOVEDOWN";
                                            if (bPrompt)
                                                oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, retunID, iCurrentSetId, oTemp.TemplateId, iPrompts, istep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                            else
                                                oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, retunID, iCurrentSetId, oTemp.TemplateId, istep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                            //oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].NextStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId);
                                            //if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                            //{
                                            //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                            //}
                                            bStepCountCriteria = true;
                                            // }
                                        }
                                        else
                                        {
                                            sesResultchain[0].CompletionStatus = "NOT COMPLETED";
                                            if (!bRuleStatusIOA)
                                            {
                                                // Functionto reset rule type values in StdtEvent Table
                                                oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                                sEventAlertStatu = "IOAEvntStatus=true,Step_MoveDown=true";
                                                // Functionto Update rule Events values in StdtEvent Table
                                                oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatu);
                                            }
                                            if (!bRuleStatusMultiTchr)
                                            {
                                                // Functionto reset rule type values in StdtEvent Table
                                                oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                                sEventAlertStatu = "MultiTchrEvntStatus=true,Step_MoveDown=true";
                                                // Functionto Update rule Events values in StdtEvent Table
                                                oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatu);
                                            }
                                        }








                                        //Update the status table
                                        //############

                                        //break from the loop
                                        break;
                                    }
                                }
                                //}
                            }

                            //Check for current step only if the Previous Step succeeds
                            if (!bPrevStepFailed)
                            {
                                bStepCountCriteria = false;
                                sesResultchain[index] = null;
                                TrialLists = oDisc.GetTrialLists(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iCurrentStep, chainedCols[sColName].RequiredSession(), sColName, false, hfPlusMinusResp.Value, coltypeCode, oDS.ChainType, trans, con);
                                chainedCols[sColName].SessionCount = TrialLists.sessionCount;
                                chainedCols[sColName].SetInputData(iCurrentStep.ToString(), sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                                sesResultchain[index] = Chained.Model.Execute(chainedCols[sColName], false, bpromptColumn, oDS.ChainType);

                            }
                            ////////////////////////// New Step level criteria execution////////////////////// Ends Here ///// Arun M//////
                            #endregion
                            //chainedCols[sColName].SetInputData(iCurrentStep.ToString(), sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                            //sesResultchain[index] = Chained.Model.Execute(chainedCols[sColName]);

                            //For Accuracy Excluding Current Step......

                            if (iCurrentStep > 1)
                            {
                                if ((chainedInptData.ExcludeCrntStepMoveUp.BarCondition > 0) || (chainedInptData.ExcludeCrntStepMoveBack.BarCondition > 0))
                                {
                                    int stepmoveupCond = chainedInptData.ExcludeCrntStepMoveUp.BarCondition;
                                    int stepmovedownCond = chainedInptData.ExcludeCrntStepMoveBack.BarCondition;
                                    chainedCols[sColName].StepPercentAccuracy.BarCondition = chainedInptData.ExcludeCrntStepMoveUp.BarCondition;
                                    chainedCols[sColName].StepPercentAccuracy.ConsecutiveSuccess = chainedInptData.ExcludeCrntStepMoveUp.ConsecutiveSuccess;
                                    chainedCols[sColName].StepPercentAccuracy.TotalTrial = chainedInptData.ExcludeCrntStepMoveUp.TotalTrial;
                                    chainedCols[sColName].StepPercentAccuracy.SuccessNeeded = chainedInptData.ExcludeCrntStepMoveUp.SuccessNeeded;
                                    chainedCols[sColName].StepPercentAccuracy.bIOAReqd = chainedInptData.ExcludeCrntStepMoveUp.bIOAReqd;
                                    chainedCols[sColName].StepPercentAccuracy.bMultiTchr = chainedInptData.ExcludeCrntStepMoveUp.bMultiTchr;

                                    chainedCols[sColName].StepMoveBackPercentAccuracy.BarCondition = chainedInptData.ExcludeCrntStepMoveBack.BarCondition;
                                    chainedCols[sColName].StepMoveBackPercentAccuracy.ConsecutiveFailures = chainedInptData.ExcludeCrntStepMoveBack.ConsecutiveFailures;
                                    chainedCols[sColName].StepMoveBackPercentAccuracy.TotalTrial = chainedInptData.ExcludeCrntStepMoveBack.TotalTrial;
                                    chainedCols[sColName].StepMoveBackPercentAccuracy.FailureNeeded = chainedInptData.ExcludeCrntStepMoveBack.FailureNeeded;
                                    chainedCols[sColName].StepMoveBackPercentAccuracy.bIOAReqd = chainedInptData.ExcludeCrntStepMoveBack.bIOAReqd;
                                    chainedCols[sColName].StepMoveBackPercentAccuracy.bMultiTchr = chainedInptData.ExcludeCrntStepMoveBack.bMultiTchr;


                                    if ((stepmoveupCond > 0) || (stepmovedownCond > 0))
                                    {
                                        if (sesResultchain[index].MovedForwardStep || sesResultchain[index].MovedBackStep)
                                        {
                                            bool stepMoveUpflag = sesResultchain[index].MovedForwardStep;
                                            bool stepMoveDownflag = sesResultchain[index].MovedBackStep;
                                            TrialLists = oDisc.GetTrialLists(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iCurrentStep, chainedCols[sColName].RequiredSession(), sColName, true, hfPlusMinusResp.Value, coltypeCode, oDS.ChainType, trans, con);
                                            chainedCols[sColName].StepCount = TrialLists.trialsCount;

                                            chainedCols[sColName].SetInputData((iCurrentStep - 1).ToString(), sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                                            sesResultchain[index] = Chained.Model.Execute(chainedCols[sColName], false, bpromptColumn, oDS.ChainType);

                                            //Check move up
                                            if (chainedInptData.StepPercentAccuracy.BarCondition > 0)
                                            {
                                                if (sesResultchain[index].MovedForwardStep)
                                                {
                                                    sesResultchain[index].MovedForwardStep = stepMoveUpflag;
                                                }
                                            }
                                            else if (stepmoveupCond > 0)
                                            {
                                                sesResultchain[index].MovedForwardStep = stepMoveUpflag;
                                            }

                                            //Check move down
                                            if (chainedInptData.StepMoveBackPercentAccuracy.BarCondition > 0)
                                            {
                                                if (sesResultchain[index].MovedBackStep)
                                                {
                                                    sesResultchain[index].MovedBackStep = stepMoveDownflag;
                                                }
                                            }
                                            else if (stepmovedownCond > 0)
                                            {
                                                sesResultchain[index].MovedBackStep = stepMoveDownflag;
                                            }
                                            if (sesResultchain[index].MovedBackStep)
                                            {
                                                if ((iCurrentStep - 1) == 1)
                                                {
                                                    sesResultchain[index].NextStep = sesResultchain[index].NextStep - 1;
                                                }
                                            }

                                            if (iCurrentStep == 2)
                                            {
                                                if (sesResultchain[index].MovedBackSet)
                                                {
                                                    //if (TrialLists.totalSet > 1)
                                                    //{
                                                    //    sesResultchain[index].MovedBackSet = true;
                                                    //    sesResultchain[index].NextSet = chainedCols[sColName].CurrentSet - 1;

                                                    //}
                                                    //else
                                                    //{
                                                    sesResultchain[index].MovedBackSet = false;
                                                    sesResultchain[index].MovedBackStep = true;
                                                    sesResultchain[index].NextStep = sesResultchain[index].NextStep - 1;
                                                    //  }

                                                }
                                            }

                                            sesResultchain[index].NextStep++;
                                            if (sesResultchain[index].NextStep > (chainedCols[sColName].StepCount + 1))
                                            {
                                                sesResultchain[index].NextStep--;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        TrialLists = oDisc.GetTrialLists(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iCurrentStep, chainedCols[sColName].RequiredSession(), sColName, true, hfPlusMinusResp.Value, coltypeCode, oDS.ChainType, trans, con);
                                        chainedCols[sColName].StepCount = TrialLists.trialsCount;

                                        chainedCols[sColName].SetInputData((iCurrentStep - 1).ToString(), sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                                        sesResultchain[index] = Chained.Model.Execute(chainedCols[sColName], false, bpromptColumn, oDS.ChainType);


                                        if (sesResultchain[index].MovedBackStep)
                                        {
                                            if ((iCurrentStep - 1) == 1)
                                            {
                                                sesResultchain[index].NextStep = sesResultchain[index].NextStep - 1;
                                            }
                                        }

                                        if (iCurrentStep == 2)
                                        {
                                            if (sesResultchain[index].MovedBackSet)
                                            {
                                                //if (TrialLists.totalSet > 1)
                                                //{
                                                //    sesResultchain[index].MovedBackSet = true;
                                                //    sesResultchain[index].NextSet = chainedCols[sColName].CurrentSet - 1;

                                                //}
                                                //else
                                                //{
                                                sesResultchain[index].MovedBackSet = false;
                                                sesResultchain[index].MovedBackStep = true;
                                                sesResultchain[index].NextStep = sesResultchain[index].NextStep - 1;
                                                // }
                                            }
                                        }


                                        sesResultchain[index].NextStep++;

                                        if (sesResultchain[index].NextStep > (chainedCols[sColName].StepCount + 1))
                                        {
                                            sesResultchain[index].NextStep--;
                                        }
                                    }

                                }
                                //if ((chainedInptData.PromptExcludeCrntStepMoveUp.BarCondition > 0) || (chainedInptData.PromptExcludeCrntStepMoveBack.BarCondition > 0))
                                //{
                                //    int promptmoveupCond = chainedInptData.PromptPercentAccuracy.BarCondition;
                                //    int promptmovedownCond = chainedInptData.MoveBackPromptPercentAccuracy.BarCondition;
                                //    chainedCols[sColName].PromptPercentAccuracy.BarCondition = chainedInptData.PromptExcludeCrntStepMoveUp.BarCondition;
                                //    chainedCols[sColName].PromptPercentAccuracy.ConsecutiveSuccess = chainedInptData.PromptExcludeCrntStepMoveUp.ConsecutiveSuccess;
                                //    chainedCols[sColName].PromptPercentAccuracy.TotalTrial = chainedInptData.PromptExcludeCrntStepMoveUp.TotalTrial;
                                //    chainedCols[sColName].PromptPercentAccuracy.SuccessNeeded = chainedInptData.PromptExcludeCrntStepMoveUp.SuccessNeeded;
                                //    chainedCols[sColName].PromptPercentAccuracy.bIOAReqd = chainedInptData.PromptExcludeCrntStepMoveUp.bIOAReqd;
                                //    chainedCols[sColName].PromptPercentAccuracy.bMultiTchr = chainedInptData.PromptExcludeCrntStepMoveUp.bMultiTchr;

                                //    chainedCols[sColName].MoveBackPromptPercentAccuracy.BarCondition = chainedInptData.PromptExcludeCrntStepMoveBack.BarCondition;
                                //    chainedCols[sColName].MoveBackPromptPercentAccuracy.ConsecutiveFailures = chainedInptData.PromptExcludeCrntStepMoveBack.ConsecutiveFailures;
                                //    chainedCols[sColName].MoveBackPromptPercentAccuracy.TotalTrial = chainedInptData.PromptExcludeCrntStepMoveBack.TotalTrial;
                                //    chainedCols[sColName].MoveBackPromptPercentAccuracy.FailureNeeded = chainedInptData.PromptExcludeCrntStepMoveBack.FailureNeeded;
                                //    chainedCols[sColName].MoveBackPromptPercentAccuracy.bIOAReqd = chainedInptData.PromptExcludeCrntStepMoveBack.bIOAReqd;
                                //    chainedCols[sColName].MoveBackPromptPercentAccuracy.bMultiTchr = chainedInptData.PromptExcludeCrntStepMoveBack.bMultiTchr;


                                //    if ((promptmoveupCond > 0) || (promptmovedownCond > 0))
                                //    {
                                //        if (sesResultchain[index].MovedForwardPrompt || sesResultchain[index].MovedBackPrompt)
                                //        {
                                //            TrialLists = oDisc.GetTrialLists(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iCurrentStep, chainedCols[sColName].RequiredSession(), sColName, true);
                                //            chainedCols[sColName].StepCount = TrialLists.trialsCount;

                                //            chainedCols[sColName].SetInputData((iCurrentStep - 1).ToString(), sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                                //            sesResultchain[index] = Chained.Model.Execute(chainedCols[sColName]);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        TrialLists = oDisc.GetTrialLists(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iCurrentStep, chainedCols[sColName].RequiredSession(), sColName, true);
                                //        chainedCols[sColName].StepCount = TrialLists.trialsCount;

                                //        chainedCols[sColName].SetInputData((iCurrentStep - 1).ToString(), sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                                //        sesResultchain[index] = Chained.Model.Execute(chainedCols[sColName]);
                                //    }

                                //}
                            }
                        }

                        if (dr["ColTypeCd"].ToString() == "Duration")
                        {
                            if (bDurationMoveUp && bSetMoveUp)
                                bSetMoveUp = true;
                            if (bDurationMoveBack && bSetMoveBack)
                                bSetMoveBack = true;
                        }
                        else if (dr["ColTypeCd"].ToString() == "Frequency")
                        {
                            if (bFrequencyMoveUp && bSetMoveUp)
                                bSetMoveUp = true;
                            if (bFrequencyMoveDown && bSetMoveBack)
                                bSetMoveBack = true;
                        }
                        else if (dr["ColTypeCd"].ToString() == "Text")
                        {
                            if (bcustMoveUp && bSetMoveUp)
                                bSetMoveUp = true;
                            if (bCustMoveDown && bSetMoveBack)
                                bSetMoveBack = true;
                        }
                        else
                        {
                            if (sesResultchain[index] != null)
                            {
                                if (set_moveupCount > 0)
                                {
                                    if (bSetMoveUp && chainedCols[sColName].IsInfluencedBy(MoveType.SetMoveUp))
                                    {
                                        bSetMoveUp = sesResultchain[0].MovedForwardSet;
                                        nextSet = sesResultchain[0].NextSet;
                                        CompletionStatusSet = sesResultchain[index].CompletionStatus;
                                        if (CompletionStatusSet == "COMPLETED")
                                        {
                                            bSetCompleted = true;
                                            bSetMoveUp = true;
                                        }
                                        else
                                            bSetCompleted = false;

                                        if (bSetMoveUp) bSetMoveBack = false;
                                    }
                                }
                                if (set_movedownCount > 0)
                                {
                                    if (bSetMoveBack && chainedCols[sColName].IsInfluencedBy(MoveType.SetMoveDown))
                                    {
                                        if (iCurrentSetNbr > 1)
                                        {
                                            bSetMoveBack = sesResultchain[0].MovedBackSet;
                                            nextSet = sesResultchain[0].NextSet;
                                            if (bSetMoveBack)
                                                bSetCompleted = false;
                                            if (bSetMoveBack) bSetMoveUp = false;
                                        }
                                        else
                                        {
                                            bSetMoveBack = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (sesResultchain[index] != null)
                        {
                            if (sesResultchain[index].MovedForwardPrompt && chainedCols[sColName].IsInfluencedBy(MoveType.PromptMoveup))
                            {
                                bPromptMoveUp = true;
                                bStepMoveUp = false;
                                sNextLessonPrompt = sesResultchain[index].NextPrompt;
                                if (!bpromptColumn)
                                {
                                    if (LessonpromptUsedOther.Length > crntPrmtIndex + 1)
                                        sNextLessonPrompt = LessonpromptUsedOther[crntPrmtIndex + 1];
                                    else
                                        bPromptMoveUp = false;
                                }
                                //bStepMoveUp = sesResultchain[0].MovedForwardStep;
                                //bSetMoveUp = sesResultchain[0].MovedForwardSet;
                                //bStepMoveBack = sesResultchain[0].MovedBackStep;
                                //bSetMoveBack = sesResultchain[0].MovedBackSet;
                            }
                            if (sesResultchain[index].MovedBackPrompt && chainedCols[sColName].IsInfluencedBy(MoveType.PromptMoveDown))
                            {
                                bPromptMoveBack = true;
                                sNextLessonPrompt = sesResultchain[index].NextPrompt;
                                if (!bpromptColumn)
                                {
                                    if (crntPrmtIndex > 0)
                                        sNextLessonPrompt = LessonpromptUsedOther[crntPrmtIndex - 1];
                                    else
                                        bPromptMoveBack = false;
                                }
                                //bStepMoveUp = sesResultchain[0].MovedForwardStep;
                                //bSetMoveUp = sesResultchain[0].MovedForwardSet;
                                //bStepMoveBack = sesResultchain[0].MovedBackStep;
                                //bSetMoveBack = sesResultchain[0].MovedBackSet;
                            }
                            if (oDS.ChainType == "Total Task")
                            {
                                //bool flag = true;
                                //if (sesResultchain[index].StepPrompts != null)
                                //{
                                //    foreach (string iprompt in sesResultchain[index].StepPrompts)
                                //    {
                                //        if (iprompt != TargetPrompt)
                                //        {
                                //            flag = false;
                                //            break;
                                //        }
                                //    }
                                //}
                                //if (!flag)
                                //{
                                //    if (sesResultchain[index].MoveForwardPromptStep)
                                //    {
                                //        bPromptMoveUp = false;
                                //        bStepMoveUp = false;
                                //        bSetMoveUp = false;
                                //        bStepMoveBack = false;
                                //        bSetMoveBack = false;
                                //    }
                                //    if (sesResultchain[index].MoveBackPromptStep)
                                //    {
                                //        bPromptMoveBack = false;
                                //        bStepMoveUp = false;
                                //        bSetMoveUp = false;
                                //        bStepMoveBack = false;
                                //        bSetMoveBack = false;
                                //    }
                                //}
                            }
                            if (step_moveupCount > 0)
                            {
                                if (bStepMoveUp && chainedCols[sColName].IsInfluencedBy(MoveType.StepMoveUp))
                                {
                                    bStepMoveUp = sesResultchain[0].MovedForwardStep;
                                    nextStep = sesResultchain[0].NextStep;
                                }
                            }
                            if (step_movedownCount > 0)
                            {
                                if (bStepMoveBack && chainedCols[sColName].IsInfluencedBy(MoveType.StepMoveDown))
                                {
                                    bStepMoveBack = sesResultchain[0].MovedBackStep;
                                    nextStep = sesResultchain[0].NextStep;
                                }
                            }

                        }
                        loop++;

                    }
                    else
                    {
                        loop++;
                        //bSetMoveUp = false;
                        //bSetMoveBack = false;
                        //bPromptMoveUp = false;
                        //bPromptMoveBack = false;
                        //bStepMoveUp = false;
                        //bStepMoveBack = false;
                    }
                    //if ((!bPrompt) && (sesResult[0].NextPrompt.ToString() == "*"))
                    //{
                    string sEventAlertStatus = "";
                    int iPrompt = 0;
                    if (count == loop)
                    {
                        if (bSetMoveBack && chainedCols[sColName].IsInfluencedBy(MoveType.SetMoveDown))
                        {
                            bStepMoveUp = false;
                            bStepMoveBack = false;
                        }
                        if (bStepMoveUp && chainedCols[sColName].IsInfluencedBy(MoveType.StepMoveUp))
                        {
                            bSetMoveUp = false;
                            bSetMoveBack = false;
                        }
                        if (bStepMoveBack && chainedCols[sColName].IsInfluencedBy(MoveType.StepMoveDown))
                        {
                            bSetMoveUp = false;
                            bSetMoveBack = false;
                        }
                        if ((bPromtHirchy && (LessonTargetPrompt.Trim() != sCurrentLessonPrompt.Trim())))
                        {
                            if (oDS.ChainType == "Total Task")
                            {
                                if (Session["StepLevelPrompt"] != null && Convert.ToBoolean(Session["StepLevelPrompt"]) == false)
                                {
                                    bStepMoveUp = false;
                                    bSetMoveUp = false;
                                    bSetCompleted = false;
                                }
                            }
                            else
                            {
                                if (bStepMoveUp)
                                {
                                    bPromptMoveUp = true;
                                    bStepMoveUp = false;
                                    bSetMoveUp = false;
                                    bSetCompleted = false;
                                    if (LessonpromptUsedOther.Length > crntPrmtIndex + 1)
                                        sNextLessonPrompt = LessonpromptUsedOther[crntPrmtIndex + 1];
                                    else
                                    {
                                        bPromptMoveUp = false;
                                        bStepMoveUp = true;
                                    }
                                }



                            }
                        }
                        oDisc = new DiscreteSession();
                        if (bSetMoveUp)
                        {
                            string sLPused = "";
                            if (LessonpromptUsed.Length == 0)
                                sLPused = "0";
                            else
                                sLPused = LessonpromptUsed[LessonpromptUsed.Length - 1];
                            if ((bPromtHirchy && oDS.ChainType == "Total Task" &&
                                Session["StepLevelPrompt"] != null && Convert.ToBoolean(Session["StepLevelPrompt"]) == false
                                && LessonpromptUsed != null && sLessonPrompt == sLPused)
                                || !bPromtHirchy || (bPromtHirchy && oDS.ChainType == "Total Task" &&
                                Session["StepLevelPrompt"] != null && Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                                || (bPromtHirchy && oDS.ChainType != "Total Task" && LessonpromptUsed != null &&
                                sLessonPrompt == LessonpromptUsed[LessonpromptUsed.Length - 1]))
                            {
                                if (sesResultchain[0] != null)
                                {
                                    bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveUpIOA, bIOA);
                                    bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveUpMultiTecher, bMultyTchr);

                                    //bRuleStatusIOA = oDisc.checkConditionIOA(chainedInptData.IOARequired, bIOA);
                                    //bRuleStatusMultiTchr = oDisc.checkConditionIOA(chainedInptData.MultiTeacherRequired, bMultyTchr);
                                    if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                    {
                                        if (nextSet == 0) { nextSet = 1; }
                                        else
                                        {
                                            if (bPrompt)
                                            {
                                                if (oDS.PromptProc != "NA")
                                                {
                                                    if ((oDS.PromptProc == "Least-to-Most")||(oDS.PromptProc == "Graduated Guidance"))
                                                    {

                                                        iPrompt = Convert.ToInt32(LessonpromptUsed[LessonpromptUsed.Length - 1]);
                                                        sesResultchain[0].NextPrompt = iPrompt.ToString();
                                                    }
                                                    else
                                                    {
                                                        iPrompt = Convert.ToInt32(LessonpromptUsed[0]);

                                                    }
                                                }
                                            }
                                            sEventType = "SET MOVEUP";
                                            if (bPrompt)
                                            {
                                                if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                                {
                                                    oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, Convert.ToInt32(LessonTargetPrompt), nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);

                                                }
                                                else
                                                {
                                                    oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iPrompt, nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                                }
                                            }
                                            else
                                            {
                                                oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                            }
                                            bPromptMoveUp = false;
                                            //oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].NextSet.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId);
                                            //if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                            //{
                                            //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                            //}
                                            if (oDS.ChainType == "Total Task")
                                            {
                                                if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                                {
                                                    oDisc.updateStepPromptForTotalTask(oTemp.TemplateId, oSession.StudentId, oSession.LoginId, Convert.ToInt32(LessonTargetPrompt), con, trans);
                                                }
                                                else
                                                {
                                                    oDisc.updateStepPromptForTotalTask(oTemp.TemplateId, oSession.StudentId, oSession.LoginId, iPrompt, con, trans);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sesResultchain[0].CompletionStatus = "NOT COMPLETED";
                                        bSetCompleted = false;
                                        if (!bRuleStatusIOA)
                                        {

                                            // Functionto reset rule type values in StdtEvent Table
                                            oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                            sEventAlertStatus = "IOAEvntStatus=true,Set_MoveUp=true";
                                            // Functionto Update rule Events values in StdtEvent Table
                                            oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                        }
                                        if (!bRuleStatusMultiTchr)
                                        {
                                            // Functionto reset rule type values in StdtEvent Table
                                            oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                            sEventAlertStatus = "MultiTchrEvntStatus=true,Set_MoveUp=true";
                                            // Functionto Update rule Events values in StdtEvent Table
                                            oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bSetCompleted = false;
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntSet, bSetMoveUp, "Set");
                        }
                        if (bSetMoveBack)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveDownIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveDownMultiTecher, bMultyTchr);
                            if (sesResultchain[0] != null)
                            {
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {
                                    if (nextSet == 0) { nextSet = 1; }
                                    else
                                    {
                                        DataTable dtModificatn = GetModificationDetails("SET", oTemp.TemplateId);
                                        if (dtModificatn != null)
                                        {
                                            if (dtModificatn.Rows.Count > 0)
                                            {
                                                bool mod_flag = CheckSetModification(Convert.ToInt32(dtModificatn.Rows[0]["ModificationRule"].ToString()), oTemp.TemplateId, iCurrentSetId);

                                                //if (mod_flag)
                                                //{
                                                //    oData = new clsData();
                                                //    string updqry = "UPDATE DSTempHdr SET ModificationInd=1,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                //    oData.Execute(updqry);
                                                //}
                                                if (mod_flag)
                                                {
                                                    oData = new clsData();
                                                    string selqry = "SELECT ISNULL(ModificationInd, 0) as ModificationInd FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                    object mod = oData.FetchValue(selqry);
                                                    if (mod != null)
                                                    {
                                                        if (Convert.ToBoolean(mod) != true)
                                                        {
                                                            string insqry = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
                                                            "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'MODIFICATION',GETDATE()," + iSessionNmbr + ",'EV')";
                                                            oData.ExecuteWithTrans(insqry, con, trans);

                                                            string updqry = "UPDATE DSTempHdr SET ModificationInd=1,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                            oData.ExecuteWithTrans(updqry, con, trans);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (bPrompt)
                                        {
                                            if (oDS.PromptProc != "NA")
                                            {
                                                if ((oDS.PromptProc == "Least-to-Most")||(oDS.PromptProc == "Graduated Guidance"))
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[LessonpromptUsed.Length - 1]);
                                                    sesResultchain[0].NextPrompt = iPrompt.ToString();
                                                }
                                                else
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[0]);

                                                }
                                            }
                                        }
                                        sEventType = "SET MOVEDOWN";
                                        if (bPrompt)
                                            oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iPrompt, nextSet.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                        else
                                            oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, nextSet.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                        //oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].NextSet.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId);
                                        //if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                        //{
                                        //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                        //}
                                        if (oDS.ChainType == "Total Task")
                                        {
                                            oDisc.updateStepPromptForTotalTask(oTemp.TemplateId, oSession.StudentId, oSession.LoginId, Convert.ToInt32(iPrompt), con, trans);
                                        }
                                    }
                                }
                                else
                                {
                                    sesResultchain[0].CompletionStatus = "NOT COMPLETED";
                                    bSetCompleted = false;
                                    if (!bRuleStatusIOA)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "IOAEvntStatus=true,Set_MoveDown=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                    }
                                    if (!bRuleStatusMultiTchr)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "MultiTchrEvntStatus=true,Set_MoveDown=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                    }
                                }
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntSet, bSetMoveBack, "Set");
                        }
                        //}

                        if (bStepMoveUp)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bStepMoveUpIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bStepMoveUpMultiTecher, bMultyTchr);
                            if (sesResultchain[0] != null)
                            {
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {
                                    if (nextStep == 0) { nextStep = 1; }
                                    else
                                    {
                                        if (bPrompt)
                                        {
                                            if (oDS.PromptProc != "NA")
                                            {
                                                if ((oDS.PromptProc == "Least-to-Most") || (oDS.PromptProc == "Graduated Guidance"))
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[LessonpromptUsed.Length - 1]);
                                                    sesResultchain[0].NextPrompt = iPrompt.ToString();
                                                }
                                                else
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[0]);
                                                }
                                            }
                                        }
                                        sEventType = "STEP MOVEUP";
                                        if (bPrompt)
                                            oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, iCurrentStep, iCurrentSetId, oTemp.TemplateId, iPrompt, nextStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                        else
                                            oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, iCurrentStep, iCurrentSetId, oTemp.TemplateId, nextStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                        bPromptMoveUp = false;
                                        //oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].NextStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId);
                                        //if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                        //{
                                        //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                        //}
                                    }
                                }
                                else
                                {
                                    sesResultchain[0].CompletionStatus = "NOT COMPLETED";
                                    bSetCompleted = false;
                                    if (!bRuleStatusIOA)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "IOAEvntStatus=true,Step_MoveUp=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                    }
                                    if (!bRuleStatusMultiTchr)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "MultiTchrEvntStatus=true,Step_MoveUp=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                    }
                                }
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntStep, bStepMoveUp, "Step");
                        }
                        if (bStepMoveBack)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bStepMoveDownIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bStepMoveDownMultiTecher, bMultyTchr);
                            if (sesResultchain[0] != null)
                            {
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {
                                    if (nextStep == 0) { nextStep = 1; }
                                    else
                                    {
                                        DataTable dtModificatn = GetModificationDetails("STEP", oTemp.TemplateId);
                                        if (dtModificatn != null)
                                        {
                                            if (dtModificatn.Rows.Count > 0)
                                            {
                                                bool mod_flag = CheckStepModification(Convert.ToInt32(dtModificatn.Rows[0]["ModificationRule"].ToString()), oTemp.TemplateId, iCurrentSetId, iCurrentStep);

                                                if (mod_flag)
                                                {
                                                    oData = new clsData();
                                                    string selqry = "SELECT ISNULL(ModificationInd, 0) as ModificationInd FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                    object mod = oData.FetchValue(selqry);
                                                    if (mod != null)
                                                    {
                                                        if (Convert.ToBoolean(mod) != true)
                                                        {
                                                            string insqry = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
                                                            "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'MODIFICATION',GETDATE()," + iSessionNmbr + ",'EV')";
                                                            oData.ExecuteWithTrans(insqry, con, trans);

                                                            string updqry = "UPDATE DSTempHdr SET ModificationInd=1,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                            oData.ExecuteWithTrans(updqry, con, trans);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (bPrompt)
                                        {
                                            if (oDS.PromptProc != "NA")
                                            {
                                                if ((oDS.PromptProc == "Least-to-Most") || (oDS.PromptProc == "Graduated Guidance"))
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[LessonpromptUsed.Length - 1]);
                                                    sesResultchain[0].NextPrompt = iPrompt.ToString();
                                                }
                                                else
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[0]);
                                                }
                                            }
                                        }
                                        sEventType = "STEP MOVEDOWN";
                                        if (bPrompt)
                                            oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, iCurrentStep, iCurrentSetId, oTemp.TemplateId, iPrompt, nextStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                        else
                                            oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, iCurrentStep, iCurrentSetId, oTemp.TemplateId, nextStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                        //oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].NextStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId);
                                        //if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                        //{
                                        //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                        //}
                                    }
                                }
                                else
                                {
                                    sesResultchain[0].CompletionStatus = "NOT COMPLETED";
                                    bSetCompleted = false;
                                    if (!bRuleStatusIOA)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "IOAEvntStatus=true,Step_MoveDown=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                    }
                                    if (!bRuleStatusMultiTchr)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "MultiTchrEvntStatus=true,Step_MoveDown=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                    }
                                }
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntStep, bStepMoveBack, "Step");
                        }
                        if (bPromptMoveUp)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bPromptMoveUpIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bPromptMoveUpMultiTecher, bMultyTchr);
                            if (sesResultchain[0] != null)
                            {
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {
                                    sEventType = "PROMPT MOVEUP";
                                    if (sCurrentLessonPrompt == sNextLessonPrompt)
                                    { }
                                    else
                                        oDisc.updatePromptStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sNextLessonPrompt, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, Convert.ToInt32(sCurrentLessonPrompt), iCurrentSetId, iCurrentStep, sLessonPlanId, con, trans);
                                    //if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                    //{
                                    //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                    //}
                                }
                                else
                                {
                                    //sesResultchain[0].CompletionStatus = "NOT COMPLETED";
                                    bSetCompleted = false;
                                    if (!bRuleStatusIOA)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "IOAEvntStatus=true,Prompt_MoveUp=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                    }
                                    if (!bRuleStatusMultiTchr)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "MultiTchrEvntStatus=true,Prompt_MoveUp=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                    }
                                }
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntPrompt, bPromptMoveUp, "Prompt");
                        }
                        if (bPromptMoveBack)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bPromptMoveDownIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bPromptMoveDownMultiTecher, bMultyTchr);
                            if (sesResultchain[0] != null)
                            {
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {
                                    DataTable dtModificatn = GetModificationDetails("PROMPT", oTemp.TemplateId);
                                    if (dtModificatn != null)
                                    {
                                        if (dtModificatn.Rows.Count > 0)
                                        {
                                            bool mod_flag = CheckPromptModification(Convert.ToInt32(dtModificatn.Rows[0]["ModificationRule"].ToString()), oTemp.TemplateId, iCurrentSetId, iCurrentStep, Convert.ToInt32(sCurrentLessonPrompt));

                                            if (mod_flag)
                                            {
                                                oData = new clsData();
                                                string selqry = "SELECT ISNULL(ModificationInd, 0) as ModificationInd FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                object mod = oData.FetchValue(selqry);
                                                if (mod != null)
                                                {
                                                    if (Convert.ToBoolean(mod) != true)
                                                    {
                                                        string insqry = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
                                                        "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'MODIFICATION',GETDATE()," + iSessionNmbr + ",'EV')";
                                                        oData.ExecuteWithTrans(insqry, con, trans);

                                                        string updqry = "UPDATE DSTempHdr SET ModificationInd=1,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                        oData.ExecuteWithTrans(updqry, con, trans);
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    sEventType = "PROMPT MOVEDOWN";
                                    oDisc.updatePromptStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sNextLessonPrompt, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, Convert.ToInt32(sCurrentLessonPrompt), iCurrentSetId, iCurrentStep, sLessonPlanId, con, trans);
                                    //if (sesResultchain[0].CompletionStatus == "COMPLETED")
                                    //{
                                    //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                    //}
                                }
                                else
                                {
                                    //sesResultchain[0].CompletionStatus = "NOT COMPLETED";
                                    bSetCompleted = false;
                                    if (!bRuleStatusIOA)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "IOAEvntStatus=true,Prompt_MoveDown=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                    }
                                    if (!bRuleStatusMultiTchr)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "MultiTchrEvntStatus=true,Prompt_MoveDown=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                    }
                                }
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntPrompt, bPromptMoveBack, "Prompt");
                        }
                        if (sesResultchain[0] != null)
                        {
                            if (bSetCompleted)
                            {

                                //oDisc = new DiscreteSession();
                                //oDS.CrntStep = sesResultchain[0].NextStep - 1;
                                //if (bPrompt)
                                //    oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, Convert.ToInt32(sesResultchain[0].NextPrompt), oTemp.TemplateId, oDS.CrntStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                //else
                                //    oDisc.updateStepStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, oDS.CrntStep.ToString(), sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                //oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr);
                                oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, "COMPLETED", sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                //oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResultchain[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId);

                            }
                            else if (bSetMoveUp == true)
                            {
                                bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveUpIOA, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveUpMultiTecher, bMultyTchr);
                                DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, 0, false, "Complete");
                            }
                        }
                    }



                }
                else if (sSkillType == "Discrete")
                {

                    discreteInptData = new DiscreetTrial.InputData();
                    Rules TempRules = new Rules();
                    TempRules = GetSetRules(oTemp.TemplateId, iColId);
                    if (TempRules != null)
                    {
                        set_moveupCount = TempRules.moveup;
                        set_movedownCount = TempRules.movedown;
                        if (TempRules.count > 0)
                        {
                            //Liju
                            bSetMoveUpIOA = bSetMoveUpIOA | TempRules.pctIndMoveUp.bIOARequird | TempRules.pctAccyMoveUp.bIOARequird | TempRules.pctTotalDurationMoveUp.bIOARequird | TempRules.pctAvgDurationMoveUp.bIOARequird | TempRules.pctFrequencyMoveUp.bIOARequird | TempRules.pctCustomMoveUp.bIOARequird;
                            bSetMoveUpMultiTecher = bSetMoveUpMultiTecher | TempRules.pctIndMoveUp.bMultiTeacherRequired | TempRules.pctAccyMoveUp.bMultiTeacherRequired | TempRules.pctTotalDurationMoveUp.bMultiTeacherRequired | TempRules.pctAvgDurationMoveUp.bMultiTeacherRequired | TempRules.pctFrequencyMoveUp.bMultiTeacherRequired | TempRules.pctCustomMoveUp.bMultiTeacherRequired;
                            bSetMoveDownIOA = bSetMoveDownIOA | TempRules.pctIndMoveDown.bIOARequird | TempRules.pctAccyMoveDown.bIOARequird | TempRules.pctTotalDurationMoveDown.bIOARequird | TempRules.pctAvgDurationMoveDown.bIOARequird | TempRules.pctFrequencyMoveDown.bIOARequird | TempRules.pctCustomMoveDown.bIOARequird;
                            bSetMoveDownMultiTecher = bSetMoveDownMultiTecher | TempRules.pctIndMoveDown.bMultiTeacherRequired | TempRules.pctAccyMoveDown.bMultiTeacherRequired | TempRules.pctTotalDurationMoveDown.bMultiTeacherRequired | TempRules.pctAvgDurationMoveDown.bMultiTeacherRequired | TempRules.pctFrequencyMoveDown.bMultiTeacherRequired | TempRules.pctCustomMoveDown.bMultiTeacherRequired;
                            bTotalDurationMoveUpbIOAReqd = bTotalDurationMoveUpbIOAReqd | TempRules.pctTotalDurationMoveUp.bIOARequird;
                            bTotalDurationMoveUpbMultiTchr = bTotalDurationMoveUpbMultiTchr | TempRules.pctTotalDurationMoveUp.bMultiTeacherRequired;
                            bAvgDurationMoveUpbIOAReqd = bAvgDurationMoveUpbIOAReqd | TempRules.pctAvgDurationMoveUp.bIOARequird;
                            bAvgDurationMoveUpbMultiTchr = bAvgDurationMoveUpbMultiTchr | TempRules.pctAvgDurationMoveUp.bMultiTeacherRequired;
                            bTotalDurationMoveDownbIOAReqd = bTotalDurationMoveDownbIOAReqd | TempRules.pctTotalDurationMoveDown.bIOARequird;
                            bTotalDurationMoveDownbMultiTchr = bTotalDurationMoveDownbMultiTchr | TempRules.pctTotalDurationMoveDown.bMultiTeacherRequired;
                            bAvgDurationMoveDownbIOAReqd = bAvgDurationMoveDownbIOAReqd | TempRules.pctAvgDurationMoveDown.bIOARequird;
                            bAvgDurationMoveDownbMultiTchr = bAvgDurationMoveDownbMultiTchr | TempRules.pctAvgDurationMoveDown.bMultiTeacherRequired;
                            bFrequencyMoveUpbIOAReqd = bFrequencyMoveUpbIOAReqd | TempRules.pctFrequencyMoveUp.bIOARequird;
                            bFrequencyMoveUpbMultiTchr = bFrequencyMoveUpbMultiTchr | TempRules.pctFrequencyMoveUp.bMultiTeacherRequired;
                            bFrequencyMoveDownbIOAReqd = bFrequencyMoveDownbIOAReqd | TempRules.pctFrequencyMoveDown.bIOARequird;
                            bFrequencyMoveDownbMultiTchr = bFrequencyMoveDownbMultiTchr | TempRules.pctFrequencyMoveDown.bMultiTeacherRequired;
                            bCustomMoveupIOA = bCustomMoveupIOA || TempRules.pctCustomMoveUp.bIOARequird;
                            bCustomMovedownIOA = bCustomMovedownIOA || TempRules.pctCustomMoveDown.bIOARequird;
                            bCustomMoveupMultiTchr = bCustomMoveupMultiTchr || TempRules.pctCustomMoveUp.bMultiTeacherRequired;
                            bCustomMovedownMultiTchr = bCustomMovedownMultiTchr || TempRules.pctCustomMoveDown.bMultiTeacherRequired;


                            discreteInptData.PercentAccuracy.BarCondition = TempRules.pctAccyMoveUp.iScoreRequired;
                            discreteInptData.PercentAccuracy.ConsecutiveSuccess = TempRules.pctAccyMoveUp.bConsequetiveIndex;
                            discreteInptData.PercentAccuracy.TotalTrial = TempRules.pctAccyMoveUp.iTotalInstance;
                            discreteInptData.PercentAccuracy.SuccessNeeded = TempRules.pctAccyMoveUp.iTotalCorrectInstance;
                            discreteInptData.PercentAccuracy.bIOAReqd = TempRules.pctAccyMoveUp.bIOARequird;
                            discreteInptData.PercentAccuracy.bMultiTchr = TempRules.pctAccyMoveUp.bMultiTeacherRequired;

                            discreteInptData.IncludeMistrials = TempRules.bIncludeMisTrail;

                            discreteInptData.PercentIndependence.BarCondition = TempRules.pctIndMoveUp.iScoreRequired;
                            discreteInptData.PercentIndependence.ConsecutiveSuccess = TempRules.pctIndMoveUp.bConsequetiveIndex;
                            discreteInptData.PercentIndependence.TotalTrial = TempRules.pctIndMoveUp.iTotalInstance;
                            discreteInptData.PercentIndependence.SuccessNeeded = TempRules.pctIndMoveUp.iTotalCorrectInstance;
                            discreteInptData.PercentIndependence.bIOAReqd = TempRules.pctIndMoveUp.bIOARequird;
                            discreteInptData.PercentIndependence.bMultiTchr = TempRules.pctIndMoveUp.bMultiTeacherRequired;

                            discreteInptData.CustomPercent.BarCondition = TempRules.pctCustomMoveUp.iScoreRequired;
                            discreteInptData.CustomPercent.ConsecutiveSuccess = TempRules.pctCustomMoveUp.bConsequetiveIndex;
                            discreteInptData.CustomPercent.TotalTrial = TempRules.pctCustomMoveUp.iTotalInstance;
                            discreteInptData.CustomPercent.SuccessNeeded = TempRules.pctCustomMoveUp.iTotalCorrectInstance;
                            discreteInptData.CustomPercent.bIOAReqd = TempRules.pctCustomMoveUp.bIOARequird;
                            discreteInptData.CustomPercent.bMultiTchr = TempRules.pctCustomMoveUp.bMultiTeacherRequired;

                            discreteInptData.MoveBackCustom.BarCondition = TempRules.pctCustomMoveDown.iScoreRequired;
                            discreteInptData.MoveBackCustom.ConsecutiveFailures = TempRules.pctCustomMoveDown.bConsequetiveIndex;
                            discreteInptData.MoveBackCustom.TotalTrial = TempRules.pctCustomMoveDown.iTotalInstance;
                            discreteInptData.MoveBackCustom.FailureNeeded = TempRules.pctCustomMoveDown.iTotalCorrectInstance;
                            discreteInptData.MoveBackCustom.bIOAReqd = TempRules.pctCustomMoveDown.bIOARequird;
                            discreteInptData.MoveBackCustom.bMultiTchr = TempRules.pctCustomMoveDown.bMultiTeacherRequired;

                            discreteInptData.MoveBackPercentAccuracy.BarCondition = TempRules.pctAccyMoveDown.iScoreRequired;
                            discreteInptData.MoveBackPercentAccuracy.ConsecutiveFailures = TempRules.pctAccyMoveDown.bConsequetiveIndex;
                            discreteInptData.MoveBackPercentAccuracy.TotalTrial = TempRules.pctAccyMoveDown.iTotalInstance;
                            discreteInptData.MoveBackPercentAccuracy.FailureNeeded = TempRules.pctAccyMoveDown.iTotalCorrectInstance;
                            discreteInptData.MoveBackPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveDown.bIOARequird;
                            discreteInptData.MoveBackPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveDown.bMultiTeacherRequired;

                            discreteInptData.MoveBackPercentIndependence.BarCondition = TempRules.pctIndMoveDown.iScoreRequired;
                            discreteInptData.MoveBackPercentIndependence.ConsecutiveFailures = TempRules.pctIndMoveDown.bConsequetiveIndex;
                            discreteInptData.MoveBackPercentIndependence.TotalTrial = TempRules.pctIndMoveDown.iTotalInstance;
                            discreteInptData.MoveBackPercentIndependence.FailureNeeded = TempRules.pctIndMoveDown.iTotalCorrectInstance;
                            discreteInptData.MoveBackPercentIndependence.bIOAReqd = TempRules.pctIndMoveDown.bIOARequird;
                            discreteInptData.MoveBackPercentIndependence.bMultiTchr = TempRules.pctIndMoveDown.bMultiTeacherRequired;

                            discreteInptData.AvgDurationMoveUp.BarCondition = TempRules.pctAvgDurationMoveUp.iScoreRequired;
                            discreteInptData.AvgDurationMoveUp.ConsecutiveSuccess = TempRules.pctAvgDurationMoveUp.bConsequetiveIndex;
                            discreteInptData.AvgDurationMoveUp.TotalTrial = TempRules.pctAvgDurationMoveUp.iTotalInstance;
                            discreteInptData.AvgDurationMoveUp.SuccessNeeded = TempRules.pctAvgDurationMoveUp.iTotalCorrectInstance;
                            discreteInptData.AvgDurationMoveUp.bIOAReqd = TempRules.pctAvgDurationMoveUp.bIOARequird;
                            discreteInptData.AvgDurationMoveUp.bMultiTchr = TempRules.pctAvgDurationMoveUp.bMultiTeacherRequired;

                            discreteInptData.TotalDurationMoveUp.BarCondition = TempRules.pctTotalDurationMoveUp.iScoreRequired;
                            discreteInptData.TotalDurationMoveUp.ConsecutiveSuccess = TempRules.pctTotalDurationMoveUp.bConsequetiveIndex;
                            discreteInptData.TotalDurationMoveUp.TotalTrial = TempRules.pctTotalDurationMoveUp.iTotalInstance;
                            discreteInptData.TotalDurationMoveUp.SuccessNeeded = TempRules.pctTotalDurationMoveUp.iTotalCorrectInstance;
                            discreteInptData.TotalDurationMoveUp.bIOAReqd = TempRules.pctTotalDurationMoveUp.bIOARequird;
                            discreteInptData.TotalDurationMoveUp.bMultiTchr = TempRules.pctTotalDurationMoveUp.bMultiTeacherRequired;

                            discreteInptData.FrequencyMoveUp.BarCondition = TempRules.pctFrequencyMoveUp.iScoreRequired;
                            discreteInptData.FrequencyMoveUp.ConsecutiveSuccess = TempRules.pctFrequencyMoveUp.bConsequetiveIndex;
                            discreteInptData.FrequencyMoveUp.TotalTrial = TempRules.pctFrequencyMoveUp.iTotalInstance;
                            discreteInptData.FrequencyMoveUp.SuccessNeeded = TempRules.pctFrequencyMoveUp.iTotalCorrectInstance;
                            discreteInptData.FrequencyMoveUp.bIOAReqd = TempRules.pctFrequencyMoveUp.bIOARequird;
                            discreteInptData.FrequencyMoveUp.bMultiTchr = TempRules.pctFrequencyMoveUp.bMultiTeacherRequired;

                            discreteInptData.AvgDurationMoveDown.BarCondition = TempRules.pctAvgDurationMoveDown.iScoreRequired;
                            discreteInptData.AvgDurationMoveDown.ConsecutiveFailures = TempRules.pctAvgDurationMoveDown.bConsequetiveIndex;
                            discreteInptData.AvgDurationMoveDown.TotalTrial = TempRules.pctAvgDurationMoveDown.iTotalInstance;
                            discreteInptData.AvgDurationMoveDown.FailureNeeded = TempRules.pctAvgDurationMoveDown.iTotalCorrectInstance;
                            discreteInptData.AvgDurationMoveDown.bIOAReqd = TempRules.pctAvgDurationMoveDown.bIOARequird;
                            discreteInptData.AvgDurationMoveDown.bMultiTchr = TempRules.pctAvgDurationMoveDown.bMultiTeacherRequired;

                            discreteInptData.TotalDurationMoveDown.BarCondition = TempRules.pctTotalDurationMoveDown.iScoreRequired;
                            discreteInptData.TotalDurationMoveDown.ConsecutiveFailures = TempRules.pctTotalDurationMoveDown.bConsequetiveIndex;
                            discreteInptData.TotalDurationMoveDown.TotalTrial = TempRules.pctTotalDurationMoveDown.iTotalInstance;
                            discreteInptData.TotalDurationMoveDown.FailureNeeded = TempRules.pctTotalDurationMoveDown.iTotalCorrectInstance;
                            discreteInptData.TotalDurationMoveDown.bIOAReqd = TempRules.pctTotalDurationMoveDown.bIOARequird;
                            discreteInptData.TotalDurationMoveDown.bMultiTchr = TempRules.pctTotalDurationMoveDown.bMultiTeacherRequired;

                            discreteInptData.FrequencyMoveDown.BarCondition = TempRules.pctFrequencyMoveDown.iScoreRequired;
                            discreteInptData.FrequencyMoveDown.ConsecutiveFailures = TempRules.pctFrequencyMoveDown.bConsequetiveIndex;
                            discreteInptData.FrequencyMoveDown.TotalTrial = TempRules.pctFrequencyMoveDown.iTotalInstance;
                            discreteInptData.FrequencyMoveDown.FailureNeeded = TempRules.pctFrequencyMoveDown.iTotalCorrectInstance;
                            discreteInptData.FrequencyMoveDown.bIOAReqd = TempRules.pctFrequencyMoveDown.bIOARequird;
                            discreteInptData.FrequencyMoveDown.bMultiTchr = TempRules.pctFrequencyMoveDown.bMultiTeacherRequired;

                            /* discreteInptData.IOARequired = TempRules.bIOARequird;
                             discreteInptData.MultiTeacherRequired = TempRules.bMultiTeacherRequired;*/
                        }
                    }
                    TempRules = new Rules();
                    TempRules = GetPromptRules(oTemp.TemplateId, iColId);
                    if (TempRules.count > 0)
                    {
                        bPrompt = true;
                        discreteInptData.PromptHirecharchy = true;

                        //Liju
                        bPromptMoveUpIOA = bPromptMoveUpIOA | TempRules.pctIndMoveUp.bIOARequird | TempRules.pctAccyMoveUp.bIOARequird | TempRules.pctTotalDurationMoveUp.bIOARequird | TempRules.pctAvgDurationMoveUp.bIOARequird | TempRules.pctFrequencyMoveUp.bIOARequird | TempRules.pctCustomMoveUp.bIOARequird;
                        bPromptMoveUpMultiTecher = bPromptMoveUpMultiTecher | TempRules.pctIndMoveUp.bMultiTeacherRequired | TempRules.pctAccyMoveUp.bMultiTeacherRequired | TempRules.pctTotalDurationMoveUp.bMultiTeacherRequired | TempRules.pctAvgDurationMoveUp.bMultiTeacherRequired | TempRules.pctFrequencyMoveUp.bMultiTeacherRequired | TempRules.pctCustomMoveUp.bMultiTeacherRequired;
                        bPromptMoveDownIOA = bPromptMoveDownIOA | TempRules.pctIndMoveDown.bIOARequird | TempRules.pctAccyMoveDown.bIOARequird | TempRules.pctTotalDurationMoveDown.bIOARequird | TempRules.pctAvgDurationMoveDown.bIOARequird | TempRules.pctFrequencyMoveDown.bIOARequird | TempRules.pctCustomMoveDown.bIOARequird;
                        bPromptMoveDownMultiTecher = bPromptMoveDownMultiTecher | TempRules.pctIndMoveDown.bMultiTeacherRequired | TempRules.pctAccyMoveDown.bMultiTeacherRequired | TempRules.pctTotalDurationMoveDown.bMultiTeacherRequired | TempRules.pctAvgDurationMoveDown.bMultiTeacherRequired | TempRules.pctFrequencyMoveDown.bMultiTeacherRequired | TempRules.pctCustomMoveDown.bMultiTeacherRequired;


                        discreteInptData.PromptPercentAccuracy.BarCondition = TempRules.pctAccyMoveUp.iScoreRequired;
                        discreteInptData.PromptPercentAccuracy.ConsecutiveSuccess = TempRules.pctAccyMoveUp.bConsequetiveIndex;
                        discreteInptData.PromptPercentAccuracy.TotalTrial = TempRules.pctAccyMoveUp.iTotalInstance;
                        discreteInptData.PromptPercentAccuracy.SuccessNeeded = TempRules.pctAccyMoveUp.iTotalCorrectInstance;
                        discreteInptData.PromptPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveUp.bIOARequird;
                        discreteInptData.PromptPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveUp.bMultiTeacherRequired;

                        discreteInptData.PromptPercentIndependence.BarCondition = TempRules.pctIndMoveUp.iScoreRequired;
                        discreteInptData.PromptPercentIndependence.ConsecutiveSuccess = TempRules.pctIndMoveUp.bConsequetiveIndex;
                        discreteInptData.PromptPercentIndependence.TotalTrial = TempRules.pctIndMoveUp.iTotalInstance;
                        discreteInptData.PromptPercentIndependence.SuccessNeeded = TempRules.pctIndMoveUp.iTotalCorrectInstance;
                        discreteInptData.PromptPercentIndependence.bIOAReqd = TempRules.pctIndMoveUp.bIOARequird;
                        discreteInptData.PromptPercentIndependence.bMultiTchr = TempRules.pctIndMoveUp.bMultiTeacherRequired;

                        discreteInptData.MoveBackPromptPercentAccuracy.BarCondition = TempRules.pctAccyMoveDown.iScoreRequired;
                        discreteInptData.MoveBackPromptPercentAccuracy.ConsecutiveFailures = TempRules.pctAccyMoveDown.bConsequetiveIndex;
                        discreteInptData.MoveBackPromptPercentAccuracy.TotalTrial = TempRules.pctAccyMoveDown.iTotalInstance;
                        discreteInptData.MoveBackPromptPercentAccuracy.FailureNeeded = TempRules.pctAccyMoveDown.iTotalCorrectInstance;
                        discreteInptData.MoveBackPromptPercentAccuracy.bIOAReqd = TempRules.pctAccyMoveDown.bIOARequird;
                        discreteInptData.MoveBackPromptPercentAccuracy.bMultiTchr = TempRules.pctAccyMoveDown.bMultiTeacherRequired;

                        discreteInptData.MoveBackPromptPercentIndependance.BarCondition = TempRules.pctIndMoveDown.iScoreRequired;
                        discreteInptData.MoveBackPromptPercentIndependance.ConsecutiveFailures = TempRules.pctIndMoveDown.bConsequetiveIndex;
                        discreteInptData.MoveBackPromptPercentIndependance.TotalTrial = TempRules.pctIndMoveDown.iTotalInstance;
                        discreteInptData.MoveBackPromptPercentIndependance.FailureNeeded = TempRules.pctIndMoveDown.iTotalCorrectInstance;
                        discreteInptData.MoveBackPromptPercentIndependance.bIOAReqd = TempRules.pctIndMoveDown.bIOARequird;
                        discreteInptData.MoveBackPromptPercentIndependance.bMultiTchr = TempRules.pctIndMoveDown.bMultiTeacherRequired;

                        /*  if (!discreteInptData.IOARequired)
                              discreteInptData.IOARequired = TempRules.bIOARequird;
                          if (!discreteInptData.MultiTeacherRequired)
                              discreteInptData.MultiTeacherRequired = TempRules.bMultiTeacherRequired;*/
                    }
                    discreteCols.Add(sColName, discreteInptData);

                    //if (discreteInptData.PromptHirecharchy == false)
                    //{
                    //    prompt = "-,+";
                    //    //promptUsed = prompt.Split(',');
                    //    sCurrentPrompt = "+";
                    //    TargetPrompt = "+";
                    //    promptUsed = new string[1];
                    //    promptUsed[0] = "+";
                    //}
                    //else
                    //{
                    Prompt[] arPromtList = GetPrompts(oTemp.TemplateId);
                    promptUsed = new string[arPromtList.Count()];
                    LessonpromptUsed = new string[arPromtList.Count()];
                    LessonpromptUsedOther = new string[arPromtList.Count()];
                    bPromtHirchy = true;
                    for (int iCount = 0; iCount < arPromtList.Count(); iCount++)
                    {
                        promptUsed[iCount] = arPromtList[iCount].promptId.ToString();
                        LessonpromptUsed[iCount] = arPromtList[iCount].promptId.ToString();
                        LessonpromptUsedOther[iCount] = arPromtList[iCount].promptId.ToString();
                        if (!String.IsNullOrEmpty(sCurrentLessonPrompt) && sCurrentLessonPrompt != "0")
                        {
                            if (arPromtList[iCount].promptId.ToString() == sCurrentLessonPrompt)
                                crntPrmtIndex = iCount;
                        }
                    }
                    if (promptUsed.Length > 0)
                    {
                        TargetPrompt = promptUsed[promptUsed.Length - 1];
                        LessonTargetPrompt = TargetPrompt;

                        if (String.IsNullOrEmpty(sCurrentLessonPrompt) || sCurrentLessonPrompt == "0")
                        {
                            sCurrentPrompt = promptUsed[0];
                            Session["sCurrentPrompt"] = sCurrentPrompt;
                            sCurrentLessonPrompt = sCurrentPrompt;
                        }
                    }
                    //}

                    if (dr["ColTypeCd"].ToString() == "+/-")
                    {
                        prompt = "-,+";
                        //promptUsed = prompt.Split(',');
                        sCurrentPrompt = "+";
                        TargetPrompt = "+";
                        promptUsed = new string[1];
                        promptUsed[0] = "+";
                    }
                    string sEventType = "";
                    DiscreteTrials TrialLists = new DiscreteTrials();
                    reader.Close();
                    int counter = discreteCols.Count;
                    int ind = 0;
                    oDisc = new DiscreteSession();
                    TrialLists = oDisc.GetTrialLists(oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iCurrentStep, discreteCols[sColName].RequiredSession(), sColName, false, hfPlusMinusResp.Value, coltypeCode, oDS.ChainType, trans, con);
                    //if (discreteInptData.MultiTeacherRequired)
                    bMultyTchr = oDisc.MultiTeacherStatus(oSession.StudentId, oTemp.TemplateId, trans, con);
                    // else
                    //    bMultyTchr = false;

                    // if (discreteInptData.IOARequired)
                    bIOA = oDisc.IOAStats(oSession.StudentId, oTemp.TemplateId, trans, con);
                    // else
                    //     bIOA = false;
                    //Trials = trails.GetTrialLists(8, 1, ht[key].RequiredSession(), key);
                    discreteCols[sColName].SessionCount = TrialLists.sessionCount;
                    discreteCols[sColName].TrialCount = TrialLists.trialsCount;
                    discreteCols[sColName].PromptsUsed = promptUsed;
                    discreteCols[sColName].NoPromptsUsed = LessonpromptUsed;
                    discreteCols[sColName].sCurrentLessonPrompt = sCurrentLessonPrompt;
                    discreteCols[sColName].TotalSets = TrialLists.totalSet;
                    int reqSess = discreteCols[sColName].RequiredSession();
                    bool bcustMoveUp = false;
                    bool bCustMoveDown = false;
                    bool bTotDurationMoveUp = true;
                    bool bAvgDurationMoveUp = true;
                    bool bFrequencyMoveUp = true;
                    bool bTotDurationMoveDown = true;
                    bool bAvgDurationMoveDown = true;
                    bool bFrequencyMoveDown = true;
                    string CompletionStatus = "";
                    // CompletionStatusSet = "";
                    bool FreqDurTextFlag = false;

                    if (dr["ColTypeCd"].ToString() == "Text")
                    {
                        FreqDurTextFlag = true;


                    }

                    if (discreteCols[sColName].TrialCount == 0)
                    {
                        if (dr["ColTypeCd"].ToString() == "Text")
                        {

                            if (count - 1 == loop)
                            {

                            }
                            else
                            {
                                loop++;
                                continue;
                            }

                        }
                        else
                        {
                            return;
                        }
                    }


                    //if (discreteCols[sColName].TrialCount == 0)
                    //{
                    //    return;
                    //}
                    if (discreteCols[sColName].TrialCount > 0)
                    {
                        sesResult = new DiscreetTrial.Result[discreteCols.Count];
                        if (dr["ColTypeCd"].ToString() == "Text")
                        {

                            TempRules = GetSetRules(oTemp.TemplateId, iColId);
                            colCalId = Convert.ToInt32(ViewState["colCalId"]);
                            if (TempRules.pctCustomMoveUp.iScoreRequired > 0)
                            {
                                CompletionStatusSet = "";
                                //int iStudentId, int colCalId, float iScoreRequired, int iSessonNumber,int iTotalCorrectInstance,bool bConsequetiveIndex, bool flag
                                bool status = false;
                                bcustMoveUp = ValidateUp(oSession.StudentId, colCalId, TempRules.pctCustomMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctCustomMoveUp.iTotalCorrectInstance, TempRules.pctCustomMoveUp.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bCustomMoveupIOA, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bCustomMoveupMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bcustMoveUp && bSetMoveUp)
                                    {
                                        if (oDS.CrntSetNbr < setcount)
                                            nextSet = oDS.CrntSetNbr + 1;
                                        if (oDS.CrntSetNbr == setcount)
                                        {
                                            nextSet = setcount;
                                            CompletionStatusSet = "COMPLETED";
                                        }
                                        bSetMoveUp = true;
                                        bSetMoveBack = false;
                                    }
                                    else
                                        bSetMoveUp = false;
                                }
                            }
                            if (TempRules.pctCustomMoveDown.iScoreRequired > 0)
                            {
                                bool status = false;
                                bCustMoveDown = ValidateDown(oSession.StudentId, colCalId, TempRules.pctCustomMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctCustomMoveDown.iTotalCorrectInstance, TempRules.pctCustomMoveDown.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bCustomMovedownIOA, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bCustomMovedownMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bCustMoveDown && bSetMoveBack)
                                    {
                                        if (oDS.CrntSetNbr <= setcount)
                                            nextSet = oDS.CrntSetNbr - 1;
                                        if (oDS.CrntSetNbr == 1)
                                            nextSet = 1;
                                        bSetMoveBack = true;
                                        bSetMoveUp = false;
                                    }
                                    else
                                        bSetMoveBack = false;
                                }


                            }
                            FreqDurTextFlag = true;

                        }
                        else if (dr["ColTypeCd"].ToString() == "Duration")
                        {
                            freqdureloop++;
                            bool status = true;
                            CompletionStatusSet = "";
                            TempRules = GetSetRules(oTemp.TemplateId, iColId);
                            avgDurationId = Convert.ToInt32(ViewState["avgDurationId"]);
                            totDuraionId = Convert.ToInt32(ViewState["totDuraionId"]);
                            if (TempRules.pctAvgDurationMoveUp.iScoreRequired > 0)
                            {
                                bAvgDurationMoveUp = ValidateUp(oSession.StudentId, avgDurationId, TempRules.pctAvgDurationMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctAvgDurationMoveUp.iTotalCorrectInstance, TempRules.pctAvgDurationMoveUp.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bAvgDurationMoveUpbIOAReqd, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bAvgDurationMoveUpbMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bAvgDurationMoveUp)
                                    {
                                        if (oDS.CrntSetNbr < setcount)
                                            nextSet = oDS.CrntSetNbr + 1;
                                        if (oDS.CrntSetNbr == setcount)
                                        {
                                            nextSet = setcount;
                                            CompletionStatusSet = "COMPLETED";
                                        }
                                        if (CompletionStatusSet == "COMPLETED")
                                        {
                                            bSetCompleted = true;
                                        }
                                        else
                                        {
                                            bSetCompleted = false;
                                        }
                                        if (bSetMoveUp && bAvgDurationMoveUp)
                                            bSetMoveUp = true;
                                    }
                                }



                            }
                            if (TempRules.pctAvgDurationMoveDown.iScoreRequired > 0)
                            {
                                bAvgDurationMoveDown = ValidateDown(oSession.StudentId, avgDurationId, TempRules.pctAvgDurationMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctAvgDurationMoveDown.iTotalCorrectInstance, TempRules.pctAvgDurationMoveDown.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bAvgDurationMoveDownbIOAReqd, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bAvgDurationMoveDownbMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bAvgDurationMoveDown)
                                    {
                                        if (oDS.CrntSetNbr <= setcount)
                                            nextSet = oDS.CrntSetNbr - 1;
                                        if (oDS.CrntSetNbr == 1)
                                            nextSet = 1;
                                        if (bSetMoveBack && bAvgDurationMoveDown)
                                            bSetMoveBack = true;
                                    }
                                }

                            }
                            if (TempRules.pctTotalDurationMoveUp.iScoreRequired > 0)
                            {
                                CompletionStatusSet = "";
                                bTotDurationMoveUp = ValidateUp(oSession.StudentId, totDuraionId, TempRules.pctTotalDurationMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctTotalDurationMoveUp.iTotalCorrectInstance, TempRules.pctTotalDurationMoveUp.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bTotalDurationMoveUpbIOAReqd, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bTotalDurationMoveUpbMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bTotDurationMoveUp)
                                    {
                                        if (oDS.CrntSetNbr < setcount)
                                            nextSet = oDS.CrntSetNbr + 1;
                                        if (oDS.CrntSetNbr == setcount)
                                        {
                                            nextSet = setcount;
                                            CompletionStatusSet = "COMPLETED";
                                        }
                                        if (bSetMoveUp && bTotDurationMoveUp)
                                            bSetMoveUp = true;
                                    }
                                }


                            }
                            if (TempRules.pctTotalDurationMoveDown.iScoreRequired > 0)
                            {
                                bTotDurationMoveDown = ValidateDown(oSession.StudentId, totDuraionId, TempRules.pctTotalDurationMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctTotalDurationMoveDown.iTotalCorrectInstance, TempRules.pctTotalDurationMoveDown.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bTotalDurationMoveDownbIOAReqd, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bTotalDurationMoveDownbMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bTotDurationMoveDown)
                                    {
                                        if (oDS.CrntSetNbr <= setcount)
                                            nextSet = oDS.CrntSetNbr - 1;
                                        if (oDS.CrntSetNbr == 1)
                                            nextSet = 1;
                                        if (bSetMoveBack && bTotDurationMoveDown)
                                            bSetMoveBack = true;
                                    }
                                }




                            }

                            FreqDurTextFlag = true;

                        }
                        else if (dr["ColTypeCd"].ToString() == "Frequency")
                        {
                            freqdureloop++;
                            bool status = true;
                            CompletionStatusSet = "";
                            TempRules = GetSetRules(oTemp.TemplateId, iColId);
                            freqId = Convert.ToInt32(ViewState["freqId"]);
                            if (TempRules.pctFrequencyMoveUp.iScoreRequired > 0)
                            {
                                bFrequencyMoveUp = ValidateUp(oSession.StudentId, freqId, TempRules.pctFrequencyMoveUp.iScoreRequired, reqSess,
                                    TempRules.pctFrequencyMoveUp.iTotalCorrectInstance, TempRules.pctFrequencyMoveUp.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bFrequencyMoveUpbIOAReqd, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bFrequencyMoveUpbMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bFrequencyMoveUp)
                                    {

                                        if (oDS.CrntSetNbr < setcount)
                                            nextSet = oDS.CrntSetNbr + 1;
                                        if (oDS.CrntSetNbr == setcount)
                                        {
                                            nextSet = setcount;
                                            CompletionStatusSet = "COMPLETED";
                                        }
                                        if (bSetMoveUp && bFrequencyMoveUp)
                                            bSetMoveUp = true;
                                    }
                                }
                            }
                            if (TempRules.pctFrequencyMoveDown.iScoreRequired > 0)
                            {
                                bFrequencyMoveDown = ValidateDown(oSession.StudentId, freqId, TempRules.pctFrequencyMoveDown.iScoreRequired, reqSess,
                                    TempRules.pctFrequencyMoveDown.iTotalCorrectInstance, TempRules.pctFrequencyMoveDown.bConsequetiveIndex, status);
                                int setcount = TrialLists.totalSet;

                                bRuleStatusIOA = oDisc.checkConditionIOA(bFrequencyMoveDownbIOAReqd, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bFrequencyMoveDownbMultiTchr, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {

                                    if (bFrequencyMoveDown)
                                    {
                                        if (oDS.CrntSetNbr <= setcount)
                                            nextSet = oDS.CrntSetNbr - 1;
                                        if (oDS.CrntSetNbr == 1)
                                            nextSet = 1;
                                        if (bSetMoveBack && bTotDurationMoveDown)
                                            bSetMoveBack = true;
                                    }
                                }
                            }
                            FreqDurTextFlag = true;

                        }
                        else
                        {
                            discreteCols[sColName].SetInputData(sCurrentPrompt, TargetPrompt, iCurrentSetNbr.ToString(), TrialLists.totalSet.ToString(), TrialLists.arTrials);
                            sesResult[index] = DiscreetTrial.Model.Execute(discreteCols[sColName], bpromptColumn);

                            if (bSetMoveUp && discreteCols[sColName].IsInfluencedBy(DiscreteMoveType.SetMoveUp))
                            {
                                bSetMoveUp = sesResult[index].MovedForwardSet;
                                nextSet = sesResult[index].NextSet;
                                CompletionStatusSet = sesResult[index].CompletionStatus;
                                if (CompletionStatusSet == "COMPLETED")
                                {
                                    bSetCompleted = true;
                                }
                                else
                                {
                                    bSetCompleted = false;
                                }
                            }
                            if (bSetMoveBack && discreteCols[sColName].IsInfluencedBy(DiscreteMoveType.SetMoveDown))
                            {
                                if (iCurrentSetNbr > 1)
                                {
                                    bSetMoveBack = sesResult[index].MovedBackSet;
                                    nextSet = sesResult[index].NextSet;
                                    if (bSetMoveBack)
                                        bSetCompleted = false;
                                }
                                else
                                {
                                    bSetMoveBack = false;
                                }


                            }


                            if (sesResult[index].MovedForwardPrompt && discreteCols[sColName].IsInfluencedBy(DiscreteMoveType.PromptMoveup))
                            {
                                bPromptMoveUp = true;
                                sNextLessonPrompt = sesResult[index].NextPrompt;
                                if (sesResult[index].NextPrompt == "+")
                                {
                                    if (LessonpromptUsedOther.Length > crntPrmtIndex + 1)
                                    {
                                        sNextLessonPrompt = LessonpromptUsedOther[crntPrmtIndex + 1];
                                        sesResult[index].CompletionStatus = "NOT COMPLETED";
                                    }
                                    else
                                        bPromptMoveUp = false;
                                }
                                //bSetMoveUp = sesResult[index].MovedForwardSet;
                                //bSetMoveBack = sesResult[index].MovedBackSet;
                            }
                            if (sesResult[index].MovedBackPrompt && discreteCols[sColName].IsInfluencedBy(DiscreteMoveType.PromptMoveDown))
                            {
                                bPromptMoveBack = true;
                                sNextLessonPrompt = sesResult[index].NextPrompt;
                                if (sesResult[index].NextPrompt == "+")
                                {
                                    if (crntPrmtIndex > 0)
                                    {
                                        sNextLessonPrompt = LessonpromptUsedOther[crntPrmtIndex - 1];
                                        sesResult[index].CompletionStatus = "NOT COMPLETED";
                                    }
                                    else
                                        bPromptMoveBack = false;
                                }
                                //bSetMoveUp = sesResult[index].MovedForwardSet;
                                //bSetMoveBack = sesResult[index].MovedBackSet;
                            }
                        }

                        loop++;
                    }
                    else
                    {
                        loop++;

                        //bSetMoveUp = false;
                        //bSetMoveBack = false;
                        //bPromptMoveUp = false;
                        //bPromptMoveBack = false;
                    }


                    ///-----------------------------------------------------------------------------------


                    if (bSetMoveUp && (bTotDurationMoveUp && bAvgDurationMoveUp && bFrequencyMoveUp))
                    {
                        bSetMoveUp = true;
                        bSetMoveBack = false;
                        if (CompletionStatusSet == "COMPLETED")
                        {
                            bSetCompleted = true;
                        }
                        else
                        {
                            bSetCompleted = false;
                        }
                    }
                    else
                        bSetMoveUp = false;

                    if (bSetMoveBack && (bTotDurationMoveDown && bAvgDurationMoveDown && bFrequencyMoveDown))
                    {
                        bSetMoveUp = false;
                        bSetMoveBack = true;
                    }
                    else
                        bSetMoveBack = false;







                    ////----------------------------------------------------------------------------------




                    if (bPrompt == false && promptUsed.Length > 0)
                    {
                        promptUsed[0] = "0";
                    }
                    int iPrompt = 0;
                    string sEventAlertStatus = "";
                    if (count == loop)
                    {
                        oDisc = new DiscreteSession();

                        if (bPromtHirchy && (LessonTargetPrompt.Trim() != sCurrentLessonPrompt.Trim()))
                        {

                            bSetMoveUp = false;
                            bSetCompleted = false;
                        }
                        if (bSetMoveUp)
                        {
                            //CompletionStatusSet = CompletionStatus;
                            bSetMoveBack = false;
                            string sLPused = "";
                            if (LessonpromptUsed.Length == 0)
                                sLPused = "0";
                            else
                                sLPused = LessonpromptUsed[LessonpromptUsed.Length - 1];
                            if ((bPromtHirchy && LessonpromptUsed != null && sLessonPrompt == sLPused) || !bPromtHirchy)
                            {
                                bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveUpIOA, bIOA);
                                bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveUpMultiTecher, bMultyTchr);

                                //bRuleStatusIOA = oDisc.checkConditionIOA(discreteInptData.IOARequired, bIOA);
                                //bRuleStatusMultiTchr = oDisc.checkConditionIOA(discreteInptData.MultiTeacherRequired, bMultyTchr);
                                if (bRuleStatusIOA && bRuleStatusMultiTchr)
                                {
                                    if (nextSet == 0) { nextSet = 1; }
                                    else
                                    {
                                        if (bPrompt)
                                        {
                                            if (oDS.PromptProc != "NA")
                                            {
                                                if ((oDS.PromptProc == "Least-to-Most") || (oDS.PromptProc == "Graduated Guidance"))
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[LessonpromptUsed.Length - 1]);
                                                    sesResult[0].NextPrompt = iPrompt.ToString();
                                                }
                                                else
                                                {
                                                    iPrompt = Convert.ToInt32(LessonpromptUsed[0]);
                                                }
                                            }
                                        }
                                        sEventType = "SET MOVEUP";

                                        if (sesResult[0] != null)
                                        {
                                            if (sesResult[0].CompletionStatus == "COMPLETED")
                                            {
                                                oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, Convert.ToInt32(LessonTargetPrompt), nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                            }
                                            else
                                            {
                                                oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iPrompt, nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                            }
                                        }
                                        else
                                        {
                                            if (FreqDurTextFlag == true)
                                            {
                                                if (CompletionStatusSet == "COMPLETED")
                                                {
                                                    oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, Convert.ToInt32(LessonTargetPrompt), nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                                }
                                                else
                                                {
                                                    oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iPrompt, nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                                }
                                            }
                                        }



                                        bPromptMoveUp = false;
                                        //if (sesResult[0].CompletionStatus == "COMPLETED")
                                        //{
                                        //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResult[0].CompletionStatus, sEventType, iSessionNmbr);
                                        //}                                        
                                    }
                                }
                                else
                                {
                                    //sesResult[0].CompletionStatus = "NOT COMPLETED";
                                    bSetCompleted = false;
                                    if (!bRuleStatusIOA)
                                    {
                                        // Function to reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "IOAEvntStatus=true,Set_MoveUp=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                    }
                                    if (!bRuleStatusMultiTchr)
                                    {
                                        // Functionto reset rule type values in StdtEvent Table
                                        oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                        sEventAlertStatus = "MultiTchrEvntStatus=true,Set_MoveUp=true";
                                        // Functionto Update rule Events values in StdtEvent Table
                                        oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                    }


                                }



                            }
                            else
                            {
                                bSetCompleted = false;
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntSet, bSetMoveUp, "Set");
                        }

                        if (bSetMoveBack)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveDownIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveDownMultiTecher, bMultyTchr);

                            //bRuleStatusIOA = oDisc.checkConditionIOA(discreteInptData.IOARequired, bIOA);
                            //bRuleStatusMultiTchr = oDisc.checkConditionIOA(discreteInptData.MultiTeacherRequired, bMultyTchr);
                            if (bRuleStatusIOA && bRuleStatusMultiTchr)
                            {
                                if (nextSet == 0) { nextSet = 1; }
                                else
                                {
                                    DataTable dtModificatn = GetModificationDetails("SET", oTemp.TemplateId);
                                    if (dtModificatn != null)
                                    {
                                        if (dtModificatn.Rows.Count > 0)
                                        {
                                            bool mod_flag = CheckSetModification(Convert.ToInt32(dtModificatn.Rows[0]["ModificationRule"].ToString()), oTemp.TemplateId, iCurrentSetId);

                                            if (mod_flag)
                                            {
                                                oData = new clsData();
                                                string selqry = "SELECT ISNULL(ModificationInd, 0) as ModificationInd FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                object mod = oData.FetchValue(selqry);
                                                if (mod != null)
                                                {
                                                    if (Convert.ToBoolean(mod) != true)
                                                    {
                                                        string insqry = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
                                                        "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'MODIFICATION',GETDATE()," + iSessionNmbr + ",'EV')";
                                                        oData.ExecuteWithTrans(insqry, con, trans);

                                                        string updqry = "UPDATE DSTempHdr SET ModificationInd=1,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                        oData.ExecuteWithTrans(updqry, con, trans);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (bPrompt)
                                    {
                                        if (oDS.PromptProc != "NA")
                                        {
                                            if ((oDS.PromptProc == "Least-to-Most") || (oDS.PromptProc == "Graduated Guidance"))
                                            {
                                                iPrompt = Convert.ToInt32(LessonpromptUsed[LessonpromptUsed.Length - 1]);
                                                sesResult[0].NextPrompt = iPrompt.ToString();
                                            }
                                            else
                                            {
                                                iPrompt = Convert.ToInt32(LessonpromptUsed[0]);
                                            }
                                        }
                                    }
                                    sEventType = "SET MOVEDOWN";
                                    if (sesResult[0] != null) //liju
                                        oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iPrompt, nextSet.ToString(), sesResult[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                    else
                                    {
                                        if (FreqDurTextFlag)
                                        {
                                            oDisc.updateSetStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, iPrompt, nextSet.ToString(), CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                        }
                                    }
                                    bPromptMoveUp = false;
                                    //if (sesResult[0].CompletionStatus == "COMPLETED")
                                    //{
                                    //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResult[0].CompletionStatus, sEventType, iSessionNmbr);
                                    //}
                                }
                            }
                            else
                            {
                                //sesResult[0].CompletionStatus = "NOT COMPLETED";
                                bSetCompleted = false;
                                if (!bRuleStatusIOA)
                                {
                                    // Functionto reset rule type values in StdtEvent Table
                                    oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                    sEventAlertStatus = "IOAEvntStatus=true,Set_MoveDown=true";
                                    // Functionto Update rule Events values in StdtEvent Table
                                    oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                }
                                if (!bRuleStatusMultiTchr)
                                {
                                    // Functionto reset rule type values in StdtEvent Table
                                    oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                    sEventAlertStatus = "MultiTchrEvntStatus=true,Set_MoveDown=true";
                                    // Functionto Update rule Events values in StdtEvent Table
                                    oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                }

                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntSet, bSetMoveBack, "Set");
                        }
                        if (bPromptMoveUp && bPrompt)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bPromptMoveUpIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bPromptMoveUpMultiTecher, bMultyTchr);

                            //bRuleStatusIOA = oDisc.checkConditionIOA(discreteInptData.IOARequired, bIOA);
                            //bRuleStatusMultiTchr = oDisc.checkConditionIOA(discreteInptData.MultiTeacherRequired, bMultyTchr);
                            if (bRuleStatusIOA && bRuleStatusMultiTchr)
                            {
                                sEventType = "PROMPT MOVEUP";
                                if (sCurrentLessonPrompt == sNextLessonPrompt)
                                { }
                                else
                                    if (sesResult[0] != null)
                                        oDisc.updatePromptStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sNextLessonPrompt, sesResult[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, Convert.ToInt32(sCurrentLessonPrompt), iCurrentSetId, iCurrentStep, sLessonPlanId, con, trans);
                                else
                                    {
                                        if (FreqDurTextFlag)
                                        {
                                            oDisc.updatePromptStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sNextLessonPrompt, CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, Convert.ToInt32(sCurrentLessonPrompt), iCurrentSetId, iCurrentStep, sLessonPlanId, con, trans);
                                    }
                                    }
                                //if (sesResult[0].CompletionStatus == "COMPLETED")
                                //{
                                //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResult[0].CompletionStatus, sEventType, iSessionNmbr);
                                //}
                            }
                            else
                            {
                                //sesResult[0].CompletionStatus = "NOT COMPLETED";
                                bSetCompleted = false;
                                if (!bRuleStatusIOA)
                                {
                                    // Functionto reset rule type values in StdtEvent Table
                                    oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                    sEventAlertStatus = "IOAEvntStatus=true,Prompt_MoveUp=true";
                                    // Functionto Update rule Events values in StdtEvent Table
                                    oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                }
                                if (!bRuleStatusMultiTchr)
                                {
                                    // Functionto reset rule type values in StdtEvent Table
                                    oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                    sEventAlertStatus = "MultiTchrEvntStatus=true,Prompt_MoveUp=true";
                                    // Functionto Update rule Events values in StdtEvent Table
                                    oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                }
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntPrompt, bPromptMoveUp, "Prompt");
                        }
                        if (bPromptMoveBack && bPrompt)
                        {
                            bRuleStatusIOA = oDisc.checkConditionIOA(bPromptMoveDownIOA, bIOA);
                            bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bPromptMoveDownMultiTecher, bMultyTchr);

                            //bRuleStatusIOA = oDisc.checkConditionIOA(discreteInptData.IOARequired, bIOA);
                            //bRuleStatusMultiTchr = oDisc.checkConditionIOA(discreteInptData.MultiTeacherRequired, bMultyTchr);
                            if (bRuleStatusIOA && bRuleStatusMultiTchr)
                            {
                                iCurrentStep = 0;
                                DataTable dtModificatn = GetModificationDetails("PROMPT", oTemp.TemplateId);
                                if (dtModificatn != null)
                                {
                                    if (dtModificatn.Rows.Count > 0)
                                    {
                                        bool mod_flag = CheckPromptModification(Convert.ToInt32(dtModificatn.Rows[0]["ModificationRule"].ToString()), oTemp.TemplateId, iCurrentSetId, iCurrentStep, Convert.ToInt32(sCurrentLessonPrompt));

                                        if (mod_flag)
                                        {
                                            oData = new clsData();
                                            string selqry = "SELECT ISNULL(ModificationInd, 0) as ModificationInd FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId;
                                            object mod = oData.FetchValue(selqry);
                                            if (mod != null)
                                            {
                                                if (Convert.ToBoolean(mod) != true)
                                                {
                                                    string insqry = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
                                                    "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",'MODIFICATION',GETDATE()," + iSessionNmbr + ",'EV')";
                                                    oData.ExecuteWithTrans(insqry, con, trans);

                                                    string updqry = "UPDATE DSTempHdr SET ModificationInd=1,ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                                                    oData.ExecuteWithTrans(updqry, con, trans);
                                                }
                                            }
                                        }
                                    }
                                }


                                sEventType = "PROMPT MOVEDOWN";
                                if (sesResult[0] != null)
                                    oDisc.updatePromptStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sNextLessonPrompt, sesResult[0].CompletionStatus, sEventType, iSessionNmbr, oSession.LoginId, Convert.ToInt32(sCurrentLessonPrompt), iCurrentSetId, iCurrentStep, sLessonPlanId, con, trans);
                                else
                                {
                                    if (FreqDurTextFlag)
                                    {
                                        oDisc.updatePromptStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sNextLessonPrompt, CompletionStatusSet, sEventType, iSessionNmbr, oSession.LoginId, Convert.ToInt32(sCurrentLessonPrompt), iCurrentSetId, iCurrentStep, sLessonPlanId, con, trans);
                                    }
                                }
                                //if (sesResult[0].CompletionStatus == "COMPLETED")
                                //{
                                //    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, sesResult[0].CompletionStatus, sEventType, iSessionNmbr);
                                //}
                            }
                            else
                            {
                                //sesResult[0].CompletionStatus = "NOT COMPLETED";
                                bSetCompleted = false;
                                if (!bRuleStatusIOA)
                                {
                                    // Functionto reset rule type values in StdtEvent Table
                                    oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                    sEventAlertStatus = "IOAEvntStatus=true,Prompt_MoveDown=true";
                                    // Functionto Update rule Events values in StdtEvent Table
                                    oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventAlertStatus);
                                }
                                if (!bRuleStatusMultiTchr)
                                {
                                    // Functionto reset rule type values in StdtEvent Table
                                    oDisc.resetEvntStatus(oSession.Classid, oSession.StudentId, oTemp.TemplateId);
                                    sEventAlertStatus = "MultiTchrEvntStatus=true,Prompt_MoveDown=true";
                                    // Functionto Update rule Events values in StdtEvent Table
                                    oDisc.UpdateAlertEvent(oTemp.TemplateId, sEventType);
                                }
                            }

                            DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, oDS.CrntPrompt, bPromptMoveBack, "Prompt");
                        }
                        if (sesResult.Count() > 0)
                        {
                            if (sesResult[0] != null)
                            {
                                if (bSetCompleted)
                                {
                                    //bStatusFlag = true;
                                    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, "COMPLETED", sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                }
                                else if (bSetMoveUp == true)
                                {
                                    bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveUpIOA, bIOA);
                                    bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveUpMultiTecher, bMultyTchr);
                                    DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, 0, false, "Complete");
                                }
                            }
                            else
                            {
                                if (FreqDurTextFlag)
                                {
                                    if (bSetCompleted)
                                    {
                                        oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, "COMPLETED", sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                                    }
                                    else if (bSetMoveUp == true)
                                    {
                                        bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveUpIOA, bIOA);
                                        bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveUpMultiTecher, bMultyTchr);
                                        DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, 0, false, "Complete");
                                    }
                                }
                            }
                        }
                        else
                            if (FreqDurTextFlag)
                            {
                                if (bSetCompleted)
                                {
                                    oDisc.insertEventStatus(oSession.SchoolId, oSession.Classid, oSession.StudentId, oTemp.TemplateId, iCurrentSetId, "COMPLETED", sEventType, iSessionNmbr, oSession.LoginId, sLessonPlanId, con, trans);
                            }
                                else if (bSetMoveUp == true)
                                {
                                    bRuleStatusIOA = oDisc.checkConditionIOA(bSetMoveUpIOA, bIOA);
                                    bRuleStatusMultiTchr = oDisc.checkConditionMultiTchr(bSetMoveUpMultiTecher, bMultyTchr);
                                    DisplayIOA_MTstatus(bRuleStatusIOA, bRuleStatusMultiTchr, 0, false, "Complete");
                                }
                            }
                    }
                }
            }
        }
    }

    private void ResetIOAStatus(int Temphdrid)
    {
        oData = new clsData();
        string strQry = "UPDATE DSTempHdr SET IsMT_IOA=0 WHERE DSTempHdrId=" + Temphdrid;
        oData.Execute(strQry);
    }
    //To display IOA / Multiteacher required for MOVEUP or MOVEDOWN
    private void DisplayIOA_MTstatus(bool bRuleStatusIOA, bool bRuleStatusMultiTchr, int Id, bool Completed, string Type)
    {
        if (Type == "Set")
        {
            if (Session["iCurrentSetId"] != null)
            {
                if (Convert.ToInt32(Session["iCurrentSetId"]) == Id)
                {
                    if (Completed == true)
                    {
                        UpdateRequirementCriteria(bRuleStatusIOA, bRuleStatusMultiTchr);
                    }
                }
            }
        }
        else if (Type == "Step")
        {
            if (Session["iCurrentStep"] != null)
            {
                if (Convert.ToInt32(Session["iCurrentStep"]) == Id)
                {
                    if (Completed == true)
                    {
                        UpdateRequirementCriteria(bRuleStatusIOA, bRuleStatusMultiTchr);
                    }
                }
            }

        }
        else if (Type == "Prompt")
        {
            if (Session["sCurrentPrompt"] != null)
            {
                if (Convert.ToInt32(Session["sCurrentPrompt"]) == Id)
                {
                    if (Completed == true)
                    {
                        UpdateRequirementCriteria(bRuleStatusIOA, bRuleStatusMultiTchr);
                    }
                }
            }
        }
        else if (Type == "Complete")
        {
            UpdateRequirementCriteria(bRuleStatusIOA, bRuleStatusMultiTchr);
        }

    }

    private void UpdateRequirementCriteria(bool bRuleStatusIOA, bool bRuleStatusMultiTchr)
    {
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        string UpdateQuery = "";
        string message = "";
        bool IsMaintanace = Convert.ToBoolean(hdn_isMaintainance.Value);
        if (bRuleStatusIOA == true && bRuleStatusMultiTchr == true)
        {
            UpdateQuery = "UPDATE DSTempHdr SET IsMT_IOA=0 WHERE DSTempHdrId=" + oTemp.TemplateId;
        }
        else
        {
            if (bRuleStatusIOA == false && bRuleStatusMultiTchr == false)
            {
                UpdateQuery = "UPDATE DSTempHdr SET IsMT_IOA=3 WHERE DSTempHdrId=" + oTemp.TemplateId;
                message = "Both IOA and Multi Teacher Required";
            }
            else if (bRuleStatusIOA == false && bRuleStatusMultiTchr == true)
            {
                UpdateQuery = "UPDATE DSTempHdr SET IsMT_IOA=1 WHERE DSTempHdrId=" + oTemp.TemplateId;
                message = "IOA Required";
            }
            else if (bRuleStatusIOA == true && bRuleStatusMultiTchr == false)
            {
                UpdateQuery = "UPDATE DSTempHdr SET IsMT_IOA=2 WHERE DSTempHdrId=" + oTemp.TemplateId;
                message = "Multi Teacher Required";
            }
            if (IsMaintanace == true)
            {
                UpdateQuery = "UPDATE DSTempHdr SET IsMT_IOA=0 WHERE DSTempHdrId=" + oTemp.TemplateId;
            }
        }
        oData.Execute(UpdateQuery);

        Session["ioa_mt_message"] = message;
    }


    public DataTable GetModificationDetails(string RuleType, int TemplateId)
    {
        oData = new clsData();
        DataTable dt = new DataTable();
        string sqlstr = "SELECT CriteriaDetails,IsComment,ModificationComment,ModificationRule FROM DSTempRule WHERE DSTempHdrId=" + TemplateId +
                        " AND ActiveInd='A' AND CriteriaType='MODIFICATION'";
        dt = oData.ReturnDataTable(sqlstr, false);

        return dt;
    }
    public bool CheckSetModification(int ModificationRule, int TemplateId, int SetId)
    {
        bool flag = false;
        string selstr = "SELECT COUNT(*)+1 FROM StdtSessEvent WHERE StdtSessEventType='SET MOVEDOWN' AND DSTempHdrId=" + TemplateId + " AND SetId=" + SetId + "";
        oData = new clsData();
        object count = oData.FetchValue(selstr);
        if (count != null)
        {
            if (Convert.ToInt32(count) >= ModificationRule)
            {
                flag = true;
            }
        }
        return flag;
    }
    public bool CheckStepModification(int ModificationRule, int TemplateId, int SetId, int StepId)
    {
        bool flag = false;
        string selstr = "SELECT COUNT(*)+1 FROM StdtSessEvent WHERE StdtSessEventType='STEP MOVEDOWN' AND DSTempHdrId=" + TemplateId + " AND SetId=" + SetId +
                        " AND StepId=" + StepId + "";
        oData = new clsData();
        object count = oData.FetchValue(selstr);
        if (count != null)
        {
            if (Convert.ToInt32(count) >= ModificationRule)
            {
                flag = true;
            }
        }
        return flag;
    }
    public bool CheckPromptModification(int ModificationRule, int TemplateId, int SetId, int StepId, int PromptId)
    {
        bool flag = false;
        string selstr = "SELECT COUNT(*)+1 FROM StdtSessEvent WHERE StdtSessEventType='PROMPT MOVEDOWN' AND DSTempHdrId=" + TemplateId + " AND SetId=" + SetId +
                        " AND StepId=" + StepId + " AND PromptId=" + PromptId + "";
        oData = new clsData();
        object count = oData.FetchValue(selstr);
        if (count != null)
        {
            if (Convert.ToInt32(count) >= ModificationRule)
            {
                flag = true;
            }
        }
        return flag;
    }

    protected bool ValidateUp(int iStudentId, int colCalId, float iScoreRequired, int iSessonNumber, int iTotalCorrectInstance, bool bConsequetiveIndex, bool flag)
    {
        oData = new clsData();
        bool status = false;
        int consecutiveCount = 0;
        float barCondition = iScoreRequired;
        DataTable dt = new DataTable();
        string sqlstr = "SELECT Score FROM StdtSessColScore SC INNER JOIN StdtSessionHdr SH on SC.StdtSessionHdrId = sh.StdtSessionHdrId " +
                        "WHERE SH.StdtSessionHdrId IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK()" +
                        "OVER (ORDER BY (CASE(IOAInd) WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END), EndTs DESC) as RNK  FROM StdtSessionHdr hdr " +
                        "WHERE StudentId = " + iStudentId + " AND SC.DSTempSetColCalcId=" + colCalId + " ) as Rk WHERE RNK <= " + iSessonNumber + ") AND SH.IOAInd<>'Y' AND StartTs > " +
                        "(SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent " +
                        "WHERE  StudentId =" + iStudentId + " ) ORDER BY SH.StdtSessionHdrId";

        dt = oData.ReturnDataTable(sqlstr, false);
        int length = dt.Rows.Count;
        float[] values = new float[length];
        int itreator = 0;
        foreach (DataRow dr in dt.Rows)
        {
            values[itreator] = float.Parse(dr["Score"].ToString());
            itreator++;
        }
        if (flag == true)
        {
            for (int index = 0; index < values.Length; index++)
            {
                values[index] = barCondition - values[index];
            }
            barCondition = 0;
        }
        if (flag == false)
        {
            for (int index = 0; index < values.Length; index++)
            {
                values[index] = values[index] - barCondition;
            }
            barCondition = 0;
        }

        if (bConsequetiveIndex)
        {
            consecutiveCount = Chained.Model.ConsecutiveCount(values, barCondition, true);
            if (iTotalCorrectInstance <= consecutiveCount)
            {
                status = true;
            }
        }
        else
        {
            consecutiveCount = Chained.Model.SuccessORFailureCount(values, barCondition, iTotalCorrectInstance, true);
            if (iTotalCorrectInstance <= consecutiveCount)
            {
                status = true;
            }
        }
        return status;
    }

    protected bool ValidateDown(int iStudentId, int colCalId, float iScoreRequired, int iSessonNumber, int iTotalCorrectInstance, bool bConsequetiveIndex, bool flag)
    {
        oData = new clsData();
        bool status = false;
        int consecutiveCount = 0;
        float barCondition = iScoreRequired;
        DataTable dt = new DataTable();
        string sqlstr = "SELECT Score FROM StdtSessColScore SC INNER JOIN StdtSessionHdr SH on SC.StdtSessionHdrId = sh.StdtSessionHdrId " +
                        "WHERE SH.StdtSessionHdrId IN (select StdtSessionHdrId from ( select hdr.StdtSessionHdrId, RANK()" +
                        "OVER (ORDER BY (CASE(IOAInd) WHEN 'N' THEN 1 WHEN 'Y' THEN 2 END), EndTs DESC) as RNK  FROM StdtSessionHdr hdr " +
                        "WHERE StudentId = " + iStudentId + " AND SC.DSTempSetColCalcId=" + colCalId + " ) as Rk WHERE RNK <= " + iSessonNumber + ") AND SH.IOAInd<>'Y' AND StartTs > " +
                        "(SELECT ISNULL(MAX(EvntTs),'1900-01-01') FROM StdtSessEvent " +
                        "WHERE  StudentId =" + iStudentId + " ) ORDER BY SH.StdtSessionHdrId";

        dt = oData.ReturnDataTable(sqlstr, false);
        int length = dt.Rows.Count;
        float[] values = new float[length];
        int itreator = 0;
        foreach (DataRow dr in dt.Rows)
        {
            values[itreator] = float.Parse(dr["Score"].ToString());
            itreator++;
        }
        if (flag == true)
        {
            for (int index = 0; index < values.Length; index++)
            {
                values[index] = values[index] - barCondition;
            }
            barCondition = 0;
        }
        if (flag == false)
        {
            for (int index = 0; index < values.Length; index++)
            {
                values[index] = barCondition - values[index];
            }
            barCondition = 0;
        }
        if (bConsequetiveIndex)
        {
            consecutiveCount = Chained.Model.ConsecutiveCount(values, barCondition, true);
            if (iTotalCorrectInstance <= consecutiveCount)
            {
                status = true;
            }
        }
        else
        {
            consecutiveCount = Chained.Model.SuccessORFailureCount(values, barCondition, iTotalCorrectInstance, true);
            if (iTotalCorrectInstance <= consecutiveCount)
            {
                status = true;
            }
        }

        return status;
    }

    protected Rules GetSetRules(int tempId, int colId)
    {
        oData = new clsData();
        //bSetMoveUp = true;
        //bSetMoveBack = true;
        colCalId = 0;
        bool bIOAReq = false;
        bool bMultiTeacher = false;
        bool bMultiSet = false;
        bool bIncludeMisTrail = false;
        Rules ruleData = new Rules();
        RuleDetails setRuleMoveUpDetails = new RuleDetails();
        string sql = " SELECT  DR.RuleType, DR.CriteriaType, DR.IOAReqInd, DR.DSTempSetColCalcId, DR.MultiTeacherReqInd, DR.TotalInstance, DR.TotCorrInstance,DST.IncMisTrialInd, DST.ColName," +
                   " DC.CalcType,DC.CalcFormula,DT.MultiSetsInd, DT.DSTempHdrId, DR.ScoreReq,DR.ConsequetiveInd FROM DSTempHdr DT " +
                   " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId " +
                   " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId " +
                   " INNER JOIN DSTempRule DR ON DR.DSTempSetColCalcId = DC.DSTempSetColCalcId " +
                   " WHERE (DT.DSTempHdrId =" + tempId + ")AND DR.RuleType='SET' AND DR.DSTempSetColId=" + colId + " AND DR.ActiveInd='A'";
        DataTable dt = new DataTable();
        dt = oData.ReturnDataTable(sql, false);
        foreach (DataRow dr in dt.Rows)
        {
            ruleData.count++;
            //reader = objData.ReturnDataReader(sql, false);
            if (dr["CriteriaType"].ToString() == "MOVE UP")
            {
                ruleData.moveup++;
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Accuracy")
                {
                    ruleData.pctAccyMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctAccyMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAccyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAccyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAccyMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctAccyMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bSetIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAccyMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }

                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Independent")
                {
                    ruleData.pctIndMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctIndMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctIndMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctIndMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctIndMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctIndMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctIndMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Prompted")
                {
                    ruleData.pctPrmtMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctPrmtMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctPrmtMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctPrmtMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctPrmtMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctPrmtMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctPrmtMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString() == "Customize")
                {
                    ruleData.pctCustomMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctCustomMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctCustomMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctCustomMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctCustomMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctCustomMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctCustomMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    //custom += dr["CalcFormula"].ToString() + "#";
                    colCalId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["colCalId"] = colCalId;
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "AvgDuration")
                {
                    ruleData.pctAvgDurationMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctAvgDurationMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAvgDurationMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAvgDurationMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAvgDurationMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctAvgDurationMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAvgDurationMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    avgDurationId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["avgDurationId"] = avgDurationId;
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "TotalDuration")
                {
                    ruleData.pctTotalDurationMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctTotalDurationMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctTotalDurationMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctTotalDurationMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctTotalDurationMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctTotalDurationMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctTotalDurationMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    totDuraionId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["totDuraionId"] = totDuraionId;
                }
                if (dr["CalcType"].ToString() == "Frequency")
                {
                    ruleData.pctFrequencyMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctFrequencyMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctFrequencyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctFrequencyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctFrequencyMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctFrequencyMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctFrequencyMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    freqId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["freqId"] = freqId;
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatTrainingStep")
                {
                    ruleData.SetlearnedStepMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.SetlearnedStepMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.SetlearnedStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.SetlearnedStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.SetlearnedStepMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.SetlearnedStepMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.SetlearnedStepMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
            }
            if (dr["CriteriaType"].ToString() == "MOVE DOWN")
            {
                ruleData.movedown++;
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Accuracy")
                {
                    ruleData.pctAccyMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctAccyMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAccyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAccyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAccyMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctAccyMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAccyMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Independent")
                {
                    ruleData.pctIndMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctIndMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctIndMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctIndMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctIndMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctIndMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctIndMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }

                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Prompted")
                {
                    ruleData.pctPrmtMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctPrmtMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctPrmtMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctPrmtMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctPrmtMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctPrmtMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctPrmtMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }

                }
                if (dr["CalcType"].ToString() == "Customize")
                {
                    ruleData.pctCustomMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctCustomMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctCustomMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctCustomMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctCustomMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctCustomMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctCustomMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    //custom += dr["CalcFormula"].ToString() + "#";

                    colCalId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["colCalId"] = colCalId;
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "AvgDuration")
                {
                    ruleData.pctAvgDurationMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctAvgDurationMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAvgDurationMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAvgDurationMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAvgDurationMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctAvgDurationMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAvgDurationMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    avgDurationId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["avgDurationId"] = avgDurationId;
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "TotalDuration")
                {
                    ruleData.pctTotalDurationMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctTotalDurationMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctTotalDurationMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctTotalDurationMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctTotalDurationMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctTotalDurationMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctTotalDurationMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    totDuraionId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["totDuraionId"] = totDuraionId;
                }
                if (dr["CalcType"].ToString() == "Frequency")
                {
                    ruleData.pctFrequencyMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctFrequencyMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctFrequencyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctFrequencyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctFrequencyMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctFrequencyMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctFrequencyMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                    freqId = Convert.ToInt32(dr["DSTempSetColCalcId"].ToString());
                    ViewState["freqId"] = freqId;
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatTrainingStep")
                {
                    ruleData.SetlearnedStepMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.SetlearnedStepMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.SetlearnedStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.SetlearnedStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.SetlearnedStepMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.SetlearnedStepMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.SetlearnedStepMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
            }

            //ruleData.bIOARequird = bIOAReq;
            // ruleData.bMultiTeacherRequired = bMultiTeacher;
            //reader.Close();

            // DiscreteSession.updatePromptStatus(sess.SchoolId,sess.StudentId,tempId, sesResult[0].NextSet.ToString(), sesResult[0].CompletionStatus, sEventType,);

        }
        dt = null;
        return ruleData;

    }

    protected Rules GetPromptRules(int tempId, int colId)
    {
        oData = new clsData();
        bool bIOAReq = false;
        bool bMultiTeacher = false;
        bool bMultiSet = false;
        bool bIncludeMisTrail = false;
        Rules ruleData = new Rules();
        RuleDetails setRuleMoveUpDetails = new RuleDetails();
        string sql = " SELECT  DR.RuleType, DR.IOAReqInd, DR.MultiTeacherReqInd, DR.CriteriaType, DR.TotalInstance, DR.TotCorrInstance,DST.IncMisTrialInd, DST.ColName," +
                   " DC.CalcType,DT.MultiSetsInd, DT.DSTempHdrId, DR.ScoreReq,DR.ConsequetiveInd FROM DSTempHdr DT " +
                   " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId " +
                   " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId " +
                   " INNER JOIN DSTempRule DR ON DR.DSTempSetColCalcId = DC.DSTempSetColCalcId " +
                   " WHERE (DT.DSTempHdrId =" + tempId + ")AND DR.RuleType='PROMPT' AND DR.DSTempSetColId=" + colId + " AND DR.ActiveInd='A'";
        DataTable dt = oData.ReturnDataTable(sql, false);
        foreach (DataRow dr in dt.Rows)
        {
            ruleData.count++;
            if (dr["CriteriaType"].ToString() == "MOVE UP")
            {
                ruleData.moveup++;
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Accuracy")
                {
                    ruleData.pctAccyMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                        //ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.pctAccyMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAccyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAccyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAccyMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctAccyMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        // bPromptIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAccyMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "").Contains("%Indepen"))
                {
                    ruleData.pctIndMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        //ruleData.pctIndMoveUp.iScoreRequired = 0;
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.pctIndMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctIndMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctIndMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctIndMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctIndMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctIndMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Prompted")
                {
                    ruleData.pctPrmtMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.pctPrmtMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctPrmtMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctPrmtMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctPrmtMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctPrmtMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctPrmtMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString() == "Customize")
                {
                    if (!bIOAReq)
                    {
                        ruleData.pctCustomMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctCustomMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }

                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatPreviouslyLearnedSteps")
                {
                    ruleData.PromptExcludeCrntStepMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        //ruleData.PromptExcludeCrntStepMoveUp.iScoreRequired = 0;
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.PromptExcludeCrntStepMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.PromptExcludeCrntStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.PromptExcludeCrntStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.PromptExcludeCrntStepMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.PromptExcludeCrntStepMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.PromptExcludeCrntStepMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatTrainingStep")
                {
                    ruleData.PromptlearnedStepMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.PromptlearnedStepMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.PromptlearnedStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.PromptlearnedStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.PromptlearnedStepMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.PromptlearnedStepMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.PromptlearnedStepMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
            }
            if (dr["CriteriaType"].ToString() == "MOVE DOWN")
            {
                ruleData.movedown++;
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Accuracy")
                {
                    ruleData.pctAccyMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        //ruleData.pctAccyMoveDown.iScoreRequired = 0;
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.pctAccyMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAccyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAccyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAccyMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctAccyMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAccyMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "").Contains("%Indepen"))
                {
                    ruleData.pctIndMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        //ruleData.pctIndMoveDown.iScoreRequired = 0;
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.pctIndMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctIndMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctIndMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctIndMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctIndMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctIndMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Prompted")
                {
                    ruleData.pctPrmtMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        //ruleData.pctPrmtMoveDown.iScoreRequired = 0;
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.pctPrmtMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctPrmtMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctPrmtMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctPrmtMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());

                    if (!bIOAReq)
                    {
                        ruleData.pctPrmtMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctPrmtMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString() == "Customize")
                {
                    if (!bIOAReq)
                    {
                        ruleData.pctCustomMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctCustomMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }

                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatPreviouslyLearnedSteps")
                {
                    ruleData.PromptExcludeCrntStepMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    if (Convert.ToBoolean(Session["StepLevelPrompt"]) == true)
                    {
                        //ruleData.PromptExcludeCrntStepMoveDown.iScoreRequired = 0;
                        ruleData.pctAccyMoveUp.iScoreRequired = 0;
                    }
                    else
                    {
                        if (ruleData.pctAccyMoveUp.iScoreRequired == 0)
                            ruleData.pctAccyMoveUp.iScoreRequired = 1;
                        else if (ruleData.pctAccyMoveUp.iScoreRequired == 100)
                            ruleData.pctAccyMoveUp.iScoreRequired = 99;
                    }
                    ruleData.PromptExcludeCrntStepMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.PromptExcludeCrntStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.PromptExcludeCrntStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.PromptExcludeCrntStepMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.PromptExcludeCrntStepMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.PromptExcludeCrntStepMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatTrainingStep")
                {
                    ruleData.PromptlearnedStepMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.PromptlearnedStepMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.PromptlearnedStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.PromptlearnedStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.PromptlearnedStepMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.PromptlearnedStepMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.PromptlearnedStepMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
            }
            /*ruleData.bIOARequird = bIOAReq;
            ruleData.bMultiTeacherRequired = bMultiTeacher;*/
        }
        dt = null;
        return ruleData;
    }

    protected Rules GetStepRules(int tempId, int colId)
    {
        oData = new clsData();
        Rules ruleData = new Rules();
        RuleDetails setRuleMoveUpDetails = new RuleDetails();
        bool bIOAReq = false;
        bool bMultiTeacher = false;
        bool bMultiSet = false;
        bool bIncludeMisTrail = false;
        string sql = " SELECT  DR.RuleType, DR.IOAReqInd, DR.MultiTeacherReqInd, DR.CriteriaType, DR.TotalInstance, DR.TotCorrInstance,DST.IncMisTrialInd, DST.ColName," +
                   " DC.CalcType,DT.MultiSetsInd, DT.DSTempHdrId, DR.ScoreReq,DR.ConsequetiveInd FROM DSTempHdr DT " +
                   " INNER JOIN DSTempSetCol DST ON DT.DSTempHdrId = DST.DSTempHdrId " +
                   " INNER JOIN DSTempSetColCalc DC ON DST.DSTempSetColId = DC.DSTempSetColId " +
                   " INNER JOIN DSTempRule DR ON DR.DSTempSetColCalcId = DC.DSTempSetColCalcId " +
                   " WHERE (DT.DSTempHdrId =" + tempId + ")AND DR.RuleType='STEP' AND DR.DSTempSetColId=" + colId + " AND DR.ActiveInd='A'";
        DataTable dt = new DataTable();
        dt = oData.ReturnDataTable(sql, false);
        foreach (DataRow dr in dt.Rows)
        {
            ruleData.count++;
            if (dr["CriteriaType"].ToString() == "MOVE UP")
            {
                ruleData.moveup++;
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Accuracy")
                {
                    ruleData.pctAccyMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctAccyMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAccyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAccyMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAccyMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctAccyMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        // bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAccyMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Independent")
                {
                    ruleData.pctIndMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctIndMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctIndMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctIndMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctIndMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctIndMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctIndMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Prompted")
                {
                    ruleData.pctPrmtMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctPrmtMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctPrmtMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctPrmtMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctPrmtMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctPrmtMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctPrmtMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString() == "Customize")
                {
                    if (!bIOAReq)
                    {
                        ruleData.pctCustomMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctCustomMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }

                }


                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatTrainingStep")
                {
                    ruleData.learnedStepMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.learnedStepMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.learnedStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.learnedStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.learnedStepMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.learnedStepMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.learnedStepMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }

                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatPreviouslyLearnedSteps")
                {
                    ruleData.excludeCrntStepMoveUp.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.excludeCrntStepMoveUp.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.excludeCrntStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.excludeCrntStepMoveUp.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.excludeCrntStepMoveUp.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.excludeCrntStepMoveUp.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.excludeCrntStepMoveUp.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }

            }
            if (dr["CriteriaType"].ToString() == "MOVE DOWN")
            {
                ruleData.movedown++;
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Accuracy")
                {
                    ruleData.pctAccyMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctAccyMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctAccyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctAccyMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctAccyMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctAccyMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctAccyMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Independent")
                {
                    ruleData.pctIndMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctIndMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctIndMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctIndMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctIndMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctIndMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctIndMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString().Replace(" ", "") == "%Prompted")
                {
                    ruleData.pctPrmtMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.pctPrmtMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.pctPrmtMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.pctPrmtMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.pctPrmtMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.pctPrmtMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctPrmtMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }
                if (dr["CalcType"].ToString() == "Customize")
                {
                    if (!bIOAReq)
                    {
                        ruleData.pctCustomMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.pctCustomMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }

                }


                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatTrainingStep")
                {
                    ruleData.learnedStepMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.learnedStepMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.learnedStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.learnedStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.learnedStepMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.learnedStepMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.learnedStepMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }

                if (dr["CalcType"].ToString().Replace(" ", "") == "%AccuracyatPreviouslyLearnedSteps")
                {
                    ruleData.excludeCrntStepMoveDown.iScoreRequired = Convert.ToInt32(dr["ScoreReq"].ToString());
                    ruleData.excludeCrntStepMoveDown.bConsequetiveIndex = Convert.ToBoolean(dr["ConsequetiveInd"].ToString());
                    if (Convert.ToBoolean(dr["ConsequetiveInd"].ToString()))
                    {
                        ruleData.excludeCrntStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    }
                    else
                    {
                        ruleData.excludeCrntStepMoveDown.iTotalCorrectInstance = Convert.ToInt32(dr["TotCorrInstance"].ToString());
                    }
                    ruleData.excludeCrntStepMoveDown.iTotalInstance = Convert.ToInt32(dr["TotalInstance"].ToString());
                    if (!bIOAReq)
                    {
                        ruleData.excludeCrntStepMoveDown.bIOARequird = Convert.ToBoolean(dr["IOAReqInd"].ToString());
                        //bStepIOA = bIOAReq;
                    }
                    if (!bMultiTeacher)
                    {
                        ruleData.excludeCrntStepMoveDown.bMultiTeacherRequired = Convert.ToBoolean(dr["MultiTeacherReqInd"].ToString());
                    }
                    if (!bIncludeMisTrail)
                    {
                        bIncludeMisTrail = Convert.ToBoolean(dr["IncMisTrialInd"].ToString());
                    }
                }

            }
            //ruleData.bIOARequird = bIOAReq;
            //ruleData.bMultiTeacherRequired = bMultiTeacher;
            //DiscreteSession.updateSetStatus(sess.SchoolId,sess.StudentId,tempId, sesResult[0].NextSet.ToString(), sesResult[0].CompletionStatus, sEventType);

        }
        dt = null;
        return ruleData;
    }

    protected Prompt[] GetPrompts(int iTempHdrId)
    {
        oData = new clsData();
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oDS != null)
        {
            string where = "";

            if ((oDS.TeachProc == "Least-to-Most") || (oDS.TeachProc == "Graduated Guidance"))    ///////----------------CHANGE FOR PROMPT ORDER-----------------////////
            // if (oDS.PromptProc == "Most-to-Least")      //-------------------------------------------------------------------//
            {
                where = "desc";

            }
            else
            {
                where = " ";
            }
            Prompt[] arPrompts = null;
            int index = 0, count = 0;
            string sqlStr = "SELECT lu.LookupName, lu.LookupId FROM LookUp lu INNER JOIN DSTempPrompt DS ON DS.PromptId=lu.LookupId WHERE" +
                " ds.ActiveInd='A' AND DS.DSTempHdrId=" + iTempHdrId + " ORDER BY SortOrder " + where;
            DataTable dt = oData.ReturnDataTable(sqlStr, false);
            index = 0;
            count = dt.Rows.Count;
            arPrompts = new Prompt[count];
            foreach (DataRow dr in dt.Rows)
            {
                Prompt PromptData = new Prompt();
                PromptData.promptName = dr["LookupName"].ToString();
                PromptData.promptId = Convert.ToInt32(dr["LookupId"]);
                arPrompts[index] = PromptData;
                index++;
            }
            return arPrompts;
        }
        else return null;
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        oData = new clsData();
        SqlConnection con = null;
        SqlTransaction trans = null;
        try
        {
            con = oData.Open();
            trans = con.BeginTransaction();
            if (Convert.ToInt32(hfAutoSaveCount.Value) > 0)
            {
                updateDraft(Convert.ToInt32(ViewState["StdtSessHdr"]), "Save", con, trans);
                oData.CommitTransation(trans, con);
                con.Close();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "stopTimer();", true);
        }
        catch (Exception ex)
        {
            if (trans != null && trans.Connection != null && trans.Connection.State == ConnectionState.Open)
            {
                oData.RollBackTransation(trans, con);

            }
            if (con != null)
                con.Close();

            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(ex.ToString());
            throw ex;
        }
    }


    protected void btnIOA_Click(object sender, EventArgs e)
    {
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (hfSessIDIOA.Value != "")
        {
            ViewState["StdtSessHdr"] = Convert.ToInt32(hfSessIDIOA.Value);
            LoadData(Convert.ToInt32(hfSessIDIOA.Value), true);
        }
        oDS.IOASessHdr = Convert.ToInt32(hfSessIDNorm.Value);
        oDS.IOAInd = "Y";
        btnSubmitAndRepeat1.Visible = false;
        btnSubmitAndRepeat2.Visible = false;
        //generateSheet();

    }
    protected void btnNormal_Click(object sender, EventArgs e)
    {
        oDS = (clsDataSheet)Session[DatasheetKey];
        ViewState["StdtSessHdr"] = Convert.ToInt32(hfSessIDNorm.Value);
        oDS.IOAInd = "N";
        //generateSheet();
        LoadData(Convert.ToInt32(hfSessIDNorm.Value), true);
    }
    protected void btnPriorSessn_Click(object sender, EventArgs e)
    {
        string lpId = "";
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oDS != null)
            lpId = oTemp.TemplateId.ToString();
        clearOverrideSession(lpId);
        Response.Redirect("DSTempHistory.aspx?LPid=" + oDS.LessonPlanID);
    }
    //protected void grdDataSht_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    oDS = (clsDataSheet)Session[DatasheetKey];
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (oDS != null)
    //        {
    //            CheckBox chkMis = (CheckBox)e.Row.FindControl("chkMistrial");
    //            if (oDS.MisTrail == true) { chkMis.Visible = true; }
    //            else chkMis.Visible = false;

    //            if ((oDS.dtColumns != null) && (oDS.dtColumns.Rows.Count > 0))
    //            {
    //                foreach (DataRow dr in oDS.dtColumns.Rows)
    //                {
    //                    if (dr["ColTypeCd"].ToString() == "Text")
    //                    {
    //                        TextBox txtText = (TextBox)e.Row.FindControl("txtText_" + dr["DSTempSetColId"].ToString());
    //                        if (txtText.Enabled == true)
    //                        {
    //                            txtText.TextChanged += new EventHandler(txtText_TextChanged);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    protected void grdDataSht_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        oDS = (clsDataSheet)Session[DatasheetKey];
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (oDS != null)
                {
                    if (oDS.ChainType == "Total Task")
                    {
                        CheckBox chkMis = (CheckBox)e.Row.FindControl("chkMistrial");
                        if (oDS.MisTrail == true) { chkMis.Visible = true; }
                        else chkMis.Visible = false;

                        if ((oDS.dtColumns != null) && (oDS.dtColumns.Rows.Count > 0))
                        {
                            foreach (DataRow dr in oDS.dtColumns.Rows)
                            {
                                if (dr["ColTypeCd"].ToString() == "Text")
                                {
                                    TextBox txtText = (TextBox)e.Row.FindControl("txtText_" + dr["DSTempSetColId"].ToString());
                                    if (txtText.Enabled == true)
                                    {

                                        if (getFormulae(dr["DSTempSetColId"].ToString()) != "")
                                        {
                                            txtText.TextChanged += new EventHandler(txtText_TextChanged);
                                        }
                                        else
                                        {
                                            txtText.Attributes.Remove("onchange");
                                            txtText.AutoPostBack = false;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else if (hfProbe.Value == "Probe")
                    {
                        CheckBox chkMis = (CheckBox)e.Row.FindControl("chkMistrial");
                        if (oDS.MisTrail == true) { chkMis.Visible = true; }
                        else chkMis.Visible = false;

                        if ((oDS.dtColumns != null) && (oDS.dtColumns.Rows.Count > 0))
                        {
                            foreach (DataRow dr in oDS.dtColumns.Rows)
                            {
                                if (dr["ColTypeCd"].ToString() == "Text")
                                {
                                    TextBox txtText = (TextBox)e.Row.FindControl("txtText_" + dr["DSTempSetColId"].ToString());
                                    if (txtText.Enabled == true)
                                    {

                                        if (getFormulae(dr["DSTempSetColId"].ToString()) != "")
                                        {
                                            txtText.TextChanged += new EventHandler(txtText_TextChanged);
                                        }
                                        else
                                        {
                                            txtText.Attributes.Remove("onchange");
                                            txtText.AutoPostBack = false;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else if (oDS.SkillType == "Chained")
                    {
                        HiddenField hfSetId = (HiddenField)e.Row.FindControl("hfStepID");
                        int step = 1;
                        //if (oDS.CrntStep < Convert.ToInt32(hfSetId.Value))
                        if (step < Convert.ToInt32(hfSetId.Value))
                        {
                            CheckBox chkMis = (CheckBox)e.Row.FindControl("chkMistrial");
                            if (oDS.MisTrail == true) { chkMis.Visible = true; }
                            else chkMis.Visible = false;

                            chkMis.Enabled = false;
                            if ((oDS.dtColumns != null) && (oDS.dtColumns.Rows.Count > 0))
                            {
                                foreach (DataRow dr in oDS.dtColumns.Rows)
                                {
                                    if (dr["ColTypeCd"].ToString() == "Text")
                                    {
                                        TextBox txtText = (TextBox)e.Row.FindControl("txtText_" + dr["DSTempSetColId"].ToString());
                                        txtText.Enabled = false;

                                    }//txtDuratn_
                                    if (dr["ColTypeCd"].ToString() == "Duration")
                                    {
                                        HtmlInputText txtText = (HtmlInputText)e.Row.FindControl("txtDuratn_" + dr["DSTempSetColId"].ToString());
                                        txtText.Disabled = true;
                                        HtmlInputButton btnText = (HtmlInputButton)e.Row.FindControl("btnDuration_" + dr["DSTempSetColId"].ToString());
                                        btnText.Disabled = true;

                                    }
                                    if (dr["ColTypeCd"].ToString() == "Frequency")
                                    {
                                        TextBox txtText = (TextBox)e.Row.FindControl("txtFrequency_" + dr["DSTempSetColId"].ToString());
                                        txtText.Enabled = false;
                                    }
                                    if (dr["ColTypeCd"].ToString() == "Prompt")
                                    {
                                        DropDownList ddlText = (DropDownList)e.Row.FindControl("ddlPrompt_" + dr["DSTempSetColId"].ToString());
                                        ddlText.Enabled = false;

                                    }
                                    if (dr["ColTypeCd"].ToString() == "+/-")
                                    {
                                        HtmlInputRadioButton rdbRespplus = (HtmlInputRadioButton)e.Row.FindControl("rdbRespPlus_" + dr["DSTempSetColId"].ToString());
                                        HtmlInputRadioButton rdbRespminus = (HtmlInputRadioButton)e.Row.FindControl("rdbRespMinus_" + dr["DSTempSetColId"].ToString());
                                        if (rdbRespplus != null && rdbRespminus != null)
                                        {
                                            rdbRespminus.Disabled = true;
                                            rdbRespplus.Disabled = true;
                                        }
                                    }
                                }
                            }

                            TextBox txtMessage = (TextBox)e.Row.FindControl("txtStepNotes");
                            txtMessage.Enabled = false;

                        }
                        else
                        {
                            CheckBox chkMis = (CheckBox)e.Row.FindControl("chkMistrial");
                            if (oDS.MisTrail == true) { chkMis.Visible = true; }
                            else chkMis.Visible = false;

                            if ((oDS.dtColumns != null) && (oDS.dtColumns.Rows.Count > 0))
                            {
                                foreach (DataRow dr in oDS.dtColumns.Rows)
                                {
                                    if (dr["ColTypeCd"].ToString() == "Text")
                                    {
                                        TextBox txtText = (TextBox)e.Row.FindControl("txtText_" + dr["DSTempSetColId"].ToString());
                                        if (txtText.Enabled == true)
                                        {
                                            if (getFormulae(dr["DSTempSetColId"].ToString()) != "")
                                            {
                                                txtText.TextChanged += new EventHandler(txtText_TextChanged);
                                            }
                                            else
                                            {
                                                txtText.Attributes.Remove("onchange");
                                                txtText.AutoPostBack = false;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        CheckBox chkMis = (CheckBox)e.Row.FindControl("chkMistrial");
                        if (oDS.MisTrail == true) { chkMis.Visible = true; }
                        else chkMis.Visible = false;

                        if ((oDS.dtColumns != null) && (oDS.dtColumns.Rows.Count > 0))
                        {
                            foreach (DataRow dr in oDS.dtColumns.Rows)
                            {
                                if (dr["ColTypeCd"].ToString() == "Text")
                                {
                                    TextBox txtText = (TextBox)e.Row.FindControl("txtText_" + dr["DSTempSetColId"].ToString());
                                    if (txtText.Enabled == true)
                                    {

                                        if (getFormulae(dr["DSTempSetColId"].ToString()) != "")
                                        {
                                            txtText.TextChanged += new EventHandler(txtText_TextChanged);
                                        }
                                        else
                                        {
                                            txtText.Attributes.Remove("onchange");
                                            txtText.AutoPostBack = false;
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }




    //protected void ImgBtn_Inactive_Click1(object sender, ImageClickEventArgs e)
    //{
    //    oData = new clsData();
    //    oTemp = (ClsTemplateSession)Session["BiweeklySession"];
    //    if (oTemp != null)
    //    {
    //        oSession = (clsSession)HttpContext.Current.Session["UserSession"];
    //        if (oSession != null)
    //        {
    //            oDS = (clsDataSheet)Session[DatasheetKey];
    //            if (oDS != null)
    //            {
    //                string strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,EventName,StdtSessEventType,EvntTs,SessionNbr,EventType,CreatedBy,CreatedOn)VALUES" +
    //               "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",(SELECT LP.LessonPlanName FROM DSTempHdr HDR INNER JOIN LessonPlan LP ON HDR.LessonPlanId=LP.LessonPlanId WHERE HDR.DSTempHdrId='" + oTemp.TemplateId + "'),'INACTIVE',GETDATE()," + oDS.SessNbr + ",'EV'," + oSession.LoginId + ",GETDATE())";
    //                int rtrnVal = oData.Execute(strQuery);
    //                if (rtrnVal > 0)
    //                {
    //                    strQuery = "SELECT LookupId from Lookup where LookupType='TemplateStatus' and LookupName='Inactive'";
    //                    int statusId = oData.Execute(strQuery);
    //                    string updQry = "UPDATE DSTempHdr SET DSMode='INACTIVE',StatusId=" + statusId + ",ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
    //                    oData.Execute(updQry);
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "refreshPage();", true);

    //                }
    //            }
    //        }
    //    }
    //}
    protected void btnExecute_Click(object sender, EventArgs e)
    {
        oDS = (clsDataSheet)Session[DatasheetKey];
        oData = new clsData();
        if (oDS != null)
        {
            Session["setvalue"] = oData.FetchValue("SELECT VTSetId FROM DSTempSet WHERE DSTempSetId=" + oDS.CrntSet);
            Session["VTLessonId"] = oDS.VTLessonId;

            if (Session["VTLessonId"] != null) { Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('../VisualTool/TeachPage_Redirecting.aspx','_top');", true); }
            // Response.Redirect("~/VisualTool/TeachPage_Redirecting.aspx");

        }
    }
    protected void btnProbe_Click(object sender, EventArgs e)
    {
        clsData objData = new clsData();
        oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        if (oSession != null)
        {
            oTemp = (ClsTemplateSession)Session["BiweeklySession"];
            if (oTemp != null)
            {
                oDS = (clsDataSheet)Session[DatasheetKey];
                if (oDS != null)
                {
                    string strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,EventName,DSTempHdrId,LessonPlanId,StdtSessEventType,EvntTs,SessionNbr,EventType)VALUES" +
                       "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + ",'ProbeMode'," + oTemp.TemplateId + ",(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId + "),'Minor',GETDATE()," + oDS.SessNbr + ",'EV')";
                    objData.ExecuteWithScope(strQuery);
                }
            }
        }
    }
    protected void grdDataSht_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void btnMistrial_Click(object sender, EventArgs e)
    {
        //oSession = (clsSession)HttpContext.Current.Session["UserSession"];
        //if (oSession != null)
        //{
        //    oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        //    if (oTemp != null)
        //    {
        //        oDS = (clsDataSheet)Session[DatasheetKey];
        //        if (oDS != null)
        //        {
        //oDS.SessionMistrial = true;
        //Session[DatasheetKey] = oDS;
        //        }
        //    }
        //}
    }

    protected void grdFile_RowEditing(object sender, GridViewEditEventArgs e)
    {
        e.Cancel = true;
        e.NewEditIndex = -1;
        grdFile.DataBind();
    }
    protected void FillDoc(int templateId)
    {
        oData = new clsData();
        string strQuery = "";
        strQuery = "Select ROW_NUMBER() OVER (ORDER BY LPDoc) AS No,DocURL as Document, LPDoc FROM LPDoc Where DocURL<>'' And DSTempHdrId = " + templateId + "";
        DataTable Dt = oData.ReturnDataTable(strQuery, false);

        if (Dt != null)
        {
            if (Dt.Rows.Count > 0)
            {
                Dt.Columns.Add("Name", typeof(string));


                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string name = Dt.Rows[i]["Document"].ToString();
                    string[] ext = name.Split('.');
                    string ext_name = ext[1];
                    if (name != "")
                    {
                        if (name.Length > 50)
                        {
                            name = name.Substring(0, 30) + "....";
                            name += ext_name;
                        }
                    }
                    Dt.Rows[i]["Name"] = name;
                }
                grdFile.DataSource = Dt;
                grdFile.DataBind();
            }
            else
                divMessage.InnerHtml = clsGeneral.warningMsg("No Documents Found");
        }



    }

    protected void grdFile_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdFile.PageIndex = e.NewPageIndex;
        int headerid = Convert.ToInt32(ViewState["HeaderId"]);
        FillDoc(headerid);
    }
    protected void grdFile_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string file = Convert.ToString(e.CommandArgument);
            oData = new clsData();
            if (e.CommandName == "download")
            {
                try
                {
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    HttpResponse Response = HttpContext.Current.Response;
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Byte[] data = (Byte[])oData.FetchValue("SELECT Data FROM binaryFiles WHERE DocId='" + file + "' AND type='LP_DOC' ");
                    string docURL = Convert.ToString(oData.FetchValue("SELECT DocURL FROM LPDoc WHERE LPDoc='" + file + "'"));
                    string contentType = GetContentType(System.IO.Path.GetExtension(docURL).ToLower().ToString());
                    Response.AddHeader("Content-type", contentType);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + docURL);
                    Response.BinaryWrite(data);
                    Response.Flush();
                    Response.End();

                }
                catch (Exception ex)
                {
                }
                // DownloadFile(file);
            }

            //ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popPrompts();});", true);

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected string GetContentType(string extension)
    {
        string ContentType = "";
        switch (extension)
        {
            case ".txt":
                ContentType = "text/plain";
                break;
            case ".doc":
                ContentType = "application/msword";
                break;
            case ".docx":
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;
            case ".pdf":
                ContentType = "application/pdf";
                break;
            case ".xls":
                ContentType = "application/vnd.ms-excel";
                break;
            case ".xlsx":
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ".csv":
                ContentType = "application/vnd.ms-excel";
                break;
        }
        return ContentType;
    }
    public void deltDoccuments(string fileName)
    {
        string strQuerry = "";
        try
        {
            oData = new clsData();
            clsSession sess = (clsSession)Session["UserSession"];
            strQuerry = "DELETE FROM LPDoc WHERE  LPDoc = '" + fileName + "' ";
            oData.Execute(strQuerry);
            strQuerry = "DELETE FROM binaryFiles WHERE  DocId = '" + fileName + "' AND type='LP_DOC' ";
            oData.Execute(strQuerry);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    protected void grdFile_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbn = e.Row.FindControl("lnkDownload") as LinkButton;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lbn);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void ImgBtn_Inactive_Click(object sender, EventArgs e)
    {
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        if (oTemp != null)
        {
            oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (oSession != null)
            {
                oDS = (clsDataSheet)Session[DatasheetKey];
                if (oDS != null)
                {
                    string strQuery = "INSERT INTO StdtSessEvent(SchoolId,ClassId,StudentId,DSTempHdrId,EventName,StdtSessEventType,EvntTs,SessionNbr,EventType,CreatedBy,CreatedOn)VALUES" +
                   "(" + oSession.SchoolId + "," + oSession.Classid + "," + oSession.StudentId + "," + oTemp.TemplateId + ",(SELECT LP.LessonPlanName FROM DSTempHdr HDR INNER JOIN LessonPlan LP ON HDR.LessonPlanId=LP.LessonPlanId WHERE HDR.DSTempHdrId='" + oTemp.TemplateId + "'),'INACTIVE',GETDATE()," + oDS.SessNbr + ",'EV'," + oSession.LoginId + ",GETDATE())";
                    int rtrnVal = oData.Execute(strQuery);
                    if (rtrnVal > 0)
                    {
                        strQuery = "SELECT LookupId from Lookup where LookupType='TemplateStatus' and LookupName='Inactive'";
                        int statusId = Convert.ToInt32(oData.FetchValue(strQuery));
                        string updQry = "UPDATE DSTempHdr SET DSMode='INACTIVE',StatusId=" + statusId + ",ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId;
                        oData.Execute(updQry);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe(" + oSession.StudentId + ");", true);
                    }
                }
            }
        }
    }


    public void loadSetsOverride()
    {

        int iCurrentSetId = 0;

        SqlDataReader reader = null;
        string strQry = " SELECT Hdr.SkillType, ISNULL(MAX(NextSetId),0) NextSetId,ISNULL(MAX(NextSetNmbr),0) NextSetNmbr " +
                 " ,ISNULL(MAX(NextStepId),0)NextStepId,ISNULL(MAX(NextPromptId),0)NextPromptId" +
                 " FROM DSTempHdr Hdr LEFT JOIN StdtDSStat Stat  ON Hdr.DSTempHdrId = Stat.DSTempHdrId " +
                  " WHERE Hdr.DSTempHdrId= " + oTemp.TemplateId + " GROUP BY Hdr.SkillType ";

        reader = oData.ReturnDataReader(strQry, false);
        if (reader.Read())
        {

            Session["iCurrentSetId"] = Convert.ToInt32(reader["NextSetId"]);
            Session["iCurrentStep"] = Convert.ToInt32(reader["NextStepId"]);
            string sCurrentPrompt = reader["NextPromptId"].ToString();
            if (!String.IsNullOrEmpty(sCurrentPrompt))
                Session["sCurrentPrompt"] = Convert.ToInt32(sCurrentPrompt);

        }
        reader.Close();
        if (Session["iCurrentSetId"] != null && Session["iCurrentSetId"] != "")
        {

            iCurrentSetId = Convert.ToInt32(Session["iCurrentSetId"]);

        }
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        if (oTemp != null)
        {
            oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (oSession != null)
            {

                string sqlStr = "select DSTempSetId,SetCd,SetName,SortOrder from DSTempSet where DSTempHdrId=" + oTemp.TemplateId + " AND ActiveInd = 'A' order by SortOrder";
                DataSet ds = oData.ReturnDataSet(sqlStr, false);
                RadioButtonListSets.DataSource = ds;
                RadioButtonListSets.DataTextField = "SetCd";
                RadioButtonListSets.DataValueField = "DSTempSetId";
                RadioButtonListSets.DataBind();

                RadioButtonListSets_tt.DataSource = ds;
                RadioButtonListSets_tt.DataTextField = "SetCd";
                RadioButtonListSets_tt.DataValueField = "DSTempSetId";
                RadioButtonListSets_tt.DataBind();

                rptr_ListSets.DataSource = ds;
                rptr_ListSets.DataBind();



            }
        }
        if (iCurrentSetId > 0)
        {
            for (int i = 0; i < RadioButtonListSets.Items.Count; i++)
            {
                if (Convert.ToInt32(RadioButtonListSets.Items[i].Value) == iCurrentSetId)
                {
                    RadioButtonListSets.Items[i].Selected = true;
                    RadioButtonListSets_tt.Items[i].Selected = true;
                    loadStepOverrid(iCurrentSetId);
                }
            }

        }

        loadPromptOverrid();

    }

    private DataTable fn_getStepList(int setid)
    {

        string sqlStr = "SELECT ChainType,TotalTaskType FROM DSTempHdr where DSTempHdrId=" + oTemp.TemplateId;
        DataTable dataT = oData.ReturnDataTable(sqlStr, false);

        string type = dataT.Rows[0]["ChainType"].ToString();
        string totalTasktype = dataT.Rows[0]["TotalTaskType"].ToString();

        if (totalTasktype != "1")
        {
            ///UPDATE SORTORDER OF DSTempStep
            ///
            string selQuerry = "";
            string updateQry = "";
            int serial = 0;
            selQuerry = "SELECT COUNT(DSTempStepId) FROM DSTempStep WHERE DSTempHdrId=" + oTemp.TemplateId + " and DSTempSetId=" + setid + "  and ActiveInd = 'A' AND IsDynamic=0 AND SortOrder IS NOT NULL";
            int countID = Convert.ToInt32(oData.FetchValue(selQuerry));
            if (countID == 0)
            {
                selQuerry = "SELECT DSTempStepId, ROW_NUMBER() Over (Order by DSTempHdrId, SortOrder) As [Sln],SortOrder FROM DSTempStep WHERE DSTempHdrId=" + oTemp.TemplateId + " and DSTempSetId=" + setid + "  and ActiveInd = 'A' " +
                    "AND IsDynamic=0 AND SortOrder IS NULL ORDER BY DSTempHdrId, SortOrder";
                DataTable dtList = oData.ReturnDataTable(selQuerry, false);
                if (dtList.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtList.Rows)
                    {
                        serial = Convert.ToInt32(dr["Sln"].ToString());
                        updateQry = "UPDATE DSTempStep SET SortOrder=" + serial + " WHERE DSTempStepId=" + Convert.ToInt32(dr["DSTempStepId"].ToString());
                        oData.Execute(updateQry);

                    }
                }
            }
            sqlStr = "select DSTempHdrId,DSTempStepId,StepCd,StepName,SortOrder from DSTempStep where DSTempHdrId=" + oTemp.TemplateId + " and DSTempSetId=" + setid + "  and ActiveInd = 'A' AND IsDynamic=0 order by SortOrder";
        }
        else
        {
            sqlStr = "select dsts.DSTempHdrId,dsts.DSTempStepId,dsts.StepCd,dsts.StepName,dsts.SortOrder, sdsss.PromptId from DSTempStep as dsts LEFT JOIN StdtDSStepStat sdsss on dsts.DSTempStepId = sdsss.DSTempStepId where dsts.DSTempHdrId=" + oTemp.TemplateId + " AND IsDynamic=0 and dsts.DSTempSetId=" + setid + "  and dsts.ActiveInd = 'A' order by dsts.SortOrder";
        }
        if (type == "Backward chain")
        {
            if (totalTasktype != "1")
            {
                sqlStr = "SELECT [DSTempStepId],[StepCd],[StepName],RANK() OVER(ORDER BY SortOrder ASC) as StepId,SortOrder  FROM [dbo].[DSTempStep] " +
                         "WHERE DSTempHdrId=" + oTemp.TemplateId + " AND  DsTempSetId=" + setid + "  AND ActiveInd='A' AND IsDynamic=0 ORDER BY [SortOrder] DESC";
            }
            else
            {

                sqlStr = "SELECT dsts.DSTempStepId,dsts.StepCd,dsts.StepName,RANK() OVER(ORDER BY dsts.SortOrder ASC) as StepId,dsts.SortOrder, sdsss.PromptId " +
                         "FROM [DSTempStep] as dsts LEFT JOIN StdtDSStepStat sdsss ON dsts.DSTempStepId = sdsss.DSTempStepId " +
                         "WHERE dsts.DSTempHdrId= " + oTemp.TemplateId + " AND  dsts.DsTempSetId=" + setid + "  AND dsts.ActiveInd='A' AND IsDynamic=0 ORDER BY dsts.SortOrder DESC";
            }
        }
        DataTable dt = oData.ReturnDataTable(sqlStr, false);

        return dt;
    }

    private void loadStepOverrid(int setid)
    {
        int iCurrentStep = 0;
        if (Session["iCurrentStep"] != null && Session["iCurrentStep"] != "")
        {
            iCurrentStep = Convert.ToInt32(Session["iCurrentStep"]);

        }
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        string type = "";
        if (oTemp != null)
        {
            oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (oSession != null)
            {
                string sqlStr = "SELECT ChainType,TotalTaskType FROM DSTempHdr where DSTempHdrId=" + oTemp.TemplateId;
                DataTable dataT = oData.ReturnDataTable(sqlStr, false);

                type = dataT.Rows[0]["ChainType"].ToString();
                string totalTasktype = dataT.Rows[0]["TotalTaskType"].ToString();

                //if (totalTasktype == "1") {
                //    totalTaskOverride.Visible = true;
                //    normalOverride.Visible = false;
                //}
                //if (totalTasktype == "2") {
                //    totalTaskOverride.Visible = false;
                //    normalOverride.Visible = true;
                //}
                if (totalTasktype != "1")
                {
                    sqlStr = "select DSTempHdrId,DSTempStepId,StepCd,StepName,SortOrder from DSTempStep where DSTempHdrId=" + oTemp.TemplateId + " and DSTempSetId=" + setid + " and ActiveInd = 'A' AND IsDynamic=0 order by SortOrder";

                    //sqlStr = "select DSTempHdrId,DSTempStepId,StepCd,StepName,SortOrder from DSTempStep where DSTempHdrId=" + oTemp.TemplateId + " and (DSTempSetId=" + setid + " OR DSTempSetId=0) and ActiveInd = 'A' order by SortOrder";
                }
                else
                {
                    sqlStr = "select dsts.DSTempHdrId,dsts.DSTempStepId,dsts.StepCd,dsts.StepName,dsts.SortOrder, sdsss.PromptId from DSTempStep as dsts LEFT JOIN StdtDSStepStat sdsss on dsts.DSTempStepId = sdsss.DSTempStepId where dsts.DSTempHdrId=" + oTemp.TemplateId + " and dsts.DSTempSetId=" + setid + " and dsts.ActiveInd = 'A' AND IsDynamic=0 order by dsts.SortOrder";

                    //sqlStr = "select dsts.DSTempHdrId,dsts.DSTempStepId,dsts.StepCd,dsts.StepName,dsts.SortOrder, sdsss.PromptId from DSTempStep as dsts LEFT JOIN StdtDSStepStat sdsss on dsts.DSTempStepId = sdsss.DSTempStepId where dsts.DSTempHdrId=" + oTemp.TemplateId + " and (dsts.DSTempSetId=" + setid + " OR dsts.DSTempSetId=0) and dsts.ActiveInd = 'A' order by dsts.SortOrder";
                }
                if (type == "Backward chain")
                {
                    if (totalTasktype != "1")
                    {
                        //sqlStr = "SELECT [DSTempHdrId],[DSTempStepId],[StepCd],[StepName],RANK() OVER(ORDER BY SortOrder ASC) as StepId  FROM [dbo].[DSTempStep] " +
                        //         "WHERE DSTempHdrId=" + oTemp.TemplateId + " AND (DsTempSetId=0 OR DsTempSetId=" + setid + ") AND ActiveInd='A' ORDER BY [SortOrder] DESC";
                        sqlStr = "SELECT [DSTempHdrId],[DSTempStepId],[StepCd],[StepName],RANK() OVER(ORDER BY SortOrder ASC) as StepId  FROM [dbo].[DSTempStep] " +
                                 "WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + setid + " AND ActiveInd='A' AND IsDynamic=0 ORDER BY [SortOrder] DESC";

                    }
                    else
                    {
                        //sqlStr = "SELECT dsts.DSTempHdrId,dsts.DSTempStepId,dsts.StepCd,dsts.StepName,RANK() OVER(ORDER BY dsts.SortOrder ASC) as StepId,dsts.SortOrder, sdsss.PromptId " +
                        //         "FROM [DSTempStep] as dsts LEFT JOIN StdtDSStepStat sdsss ON dsts.DSTempStepId = sdsss.DSTempStepId " +
                        //         "WHERE dsts.DSTempHdrId= " + oTemp.TemplateId + " AND (dsts.DsTempSetId=0 OR dsts.DsTempSetId=" + setid + ") AND dsts.ActiveInd='A' ORDER BY dsts.SortOrder DESC";

                        sqlStr = "SELECT dsts.DSTempHdrId,dsts.DSTempStepId,dsts.StepCd,dsts.StepName,RANK() OVER(ORDER BY dsts.SortOrder ASC) as StepId,dsts.SortOrder, sdsss.PromptId " +
                                 "FROM [DSTempStep] as dsts LEFT JOIN StdtDSStepStat sdsss ON dsts.DSTempStepId = sdsss.DSTempStepId " +
                                 "WHERE dsts.DSTempHdrId= " + oTemp.TemplateId + " AND dsts.DsTempSetId=" + setid + " AND dsts.ActiveInd='A' AND IsDynamic=0 ORDER BY dsts.SortOrder DESC";
                    }
                }
                DataTable dt = oData.ReturnDataTable(sqlStr, false);
                // DataSet ds = oData.ReturnDataSet(sqlStr, false);

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["StepCd"].ToString() != "" && dr["StepCd"].ToString() != null)
                    {
                        if (totalTasktype != "1")
                        {
                            RadioButtonListSteps.DataSource = dt;
                            RadioButtonListSteps.DataTextField = "StepCd";
                            RadioButtonListSteps.DataValueField = "DSTempStepId";
                            RadioButtonListSteps.DataBind();
                        }
                        else
                        {

                            rptr_listStep.DataSource = dt;
                            rptr_listStep.DataBind();
                        }

                    }
                }
                if (dt.Rows.Count == 0)
                {
                    RadioButtonListSteps.Items.Clear();
                    rptr_listStep.DataSource = null;
                    rptr_listStep.DataBind();

                }
            }
        }

        if (iCurrentStep > 0)
        {
            if (type == "Backward chain")
            {
                iCurrentStep = (RadioButtonListSteps.Items.Count - iCurrentStep) + 1;
            }
            string sqlStr = "select DSTempStepId from DSTempStep where DSTempHdrId=" + oTemp.TemplateId + " "
                + "and DSTempSetId=" + setid + " and SortOrder=" + iCurrentStep + " AND IsDynamic=0 order by SortOrder";
            iCurrentStep = Convert.ToInt32(oData.FetchValue(sqlStr));
            for (int i = 0; i < RadioButtonListSteps.Items.Count; i++)
            {
                if (Convert.ToInt32(RadioButtonListSteps.Items[i].Value) == iCurrentStep)
                {
                    RadioButtonListSteps.Items[i].Selected = true;

                }
            }

        }
        else
        {
            if (RadioButtonListSteps.Items.Count > 0)
            {
                RadioButtonListSteps.Items[0].Selected = true;
            }
        }


    }

    private void loadPromptOverrid()
    {
        int iCurrentPrompt = 0;
        string sqlStr = "";
        string type = "";
        if (Session["sCurrentPrompt"] != null && Session["sCurrentPrompt"] != "")
        {
            iCurrentPrompt = Convert.ToInt32(Session["sCurrentPrompt"]);

        }
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        if (oTemp != null)
        {
            sqlStr = "SELECT lk.LookupName FROM DSTempHdr hd inner join lookup lk on lk.LookupId=hd.PromptTypeId where DSTempHdrId=" + oTemp.TemplateId;
            type = oData.FetchValue(sqlStr).ToString();
            oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (oSession != null)
            {

                if ((type == "Least-to-Most")||(type == "Graduated Guidance"))
                {
                    sqlStr = "SELECT LU.LookupId as Id,LU.LookupName as Name,DS.DSTempPromptid FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                                            " DS.ActiveInd='A' AND DS.DSTempHdrId=" + oTemp.TemplateId + " ORDER BY SortOrder DESC";
                }
                else
                {
                    sqlStr = "SELECT LU.LookupId as Id,LU.LookupName as Name,DS.DSTempPromptid FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                                " DS.ActiveInd='A' AND DS.DSTempHdrId=" + oTemp.TemplateId + " ORDER BY SortOrder ";
                }
                //sqlStr = "select DP.DSTempPromptid,LK.LookupName,DP.PromptOrder from DSTempPrompt DP join LookUp LK on LK.LookupId=DP.PromptId where DSTempHdrId=" + oTemp.TemplateId + " order by DP.PromptOrder";
                DataSet ds = oData.ReturnDataSet(sqlStr, false);
                RadioButtonListPrompts.DataSource = ds;
                RadioButtonListPrompts.DataTextField = "Name";
                RadioButtonListPrompts.DataValueField = "DSTempPromptid";
                RadioButtonListPrompts.DataBind();

            }
        }
        //
        if (iCurrentPrompt > 0)
        {
            sqlStr = "select DP.DSTempPromptid from DSTempPrompt DP join LookUp LK on LK.LookupId=DP.PromptId where DSTempHdrId=" + oTemp.TemplateId + ""
                + " and LK.LookupId=" + iCurrentPrompt + " order by DP.PromptOrder";
            iCurrentPrompt = Convert.ToInt32(oData.FetchValue(sqlStr));
            for (int i = 0; i < RadioButtonListPrompts.Items.Count; i++)
            {
                if (Convert.ToInt32(RadioButtonListPrompts.Items[i].Value) == iCurrentPrompt)
                {
                    RadioButtonListPrompts.Items[i].Selected = true;

                }
            }
        }
        Session["iCurrentSetId"] = null;
        Session["iCurrentStep"] = null;
        Session["sCurrentPrompt"] = null;
    }

    protected void ImgBtn_Override_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), Guid.NewGuid().ToString(), "$(document).ready(function(){popOverride();});", true);
    }
    protected void RadioButtonListSets_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadStepOverrid(Convert.ToInt32(RadioButtonListSets.SelectedValue.ToString()));
    }
    protected void btnOverride_Click(object sender, EventArgs e)
    {

        string teachingProc = getTeachingMethod(oDS.TeachProc);

        string sqlStr = "SELECT ChainType,TotalTaskType FROM DSTempHdr where DSTempHdrId=" + oTemp.TemplateId;
        DataTable dataT = oData.ReturnDataTable(sqlStr, false);


        string totalTasktype = dataT.Rows[0]["TotalTaskType"].ToString();


        string sessMistrial = "N";
        sqlStr = "";

        string nextStep = "1";
        int promptId = 0;
        if (chkSessMistrial.Checked == true)
        {
            sessMistrial = "Y";
            mistrialRsn.Text = hdnMissTrialRsn.Value;
        }
        try
        {
            oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            oTemp = (ClsTemplateSession)Session["BiweeklySession"];
            oData = new clsData();
            int iSetId = Convert.ToInt32(RadioButtonListSets.SelectedItem.Value);
            int iSetId_tt = Convert.ToInt32(RadioButtonListSets_tt.SelectedItem.Value);

            if (totalTasktype != "1")
            {

                sqlStr = "SELECT ChainType FROM DSTempHdr where DSTempHdrId=" + oTemp.TemplateId;
                string type = oData.FetchValue(sqlStr).ToString();
                sqlStr = "select count(1) from DSTempStep where DSTempSetId=" + iSetId + " AND IsDynamic=0";
                int stepcount = Convert.ToInt32(oData.FetchValue(sqlStr));
                if ((teachingProc != "Match-to-Sample" && type.Contains(" chain")) || teachingProc == "Total Task")
                {
                    if (RadioButtonListSteps.Items.Count > 0)
                    {
                        int iStepId = Convert.ToInt32(RadioButtonListSteps.SelectedItem.Value);
                        sqlStr = "select SortOrder from DSTempStep where DSTempHdrId=" + oTemp.TemplateId + " "
                                    + "and DSTempSetId=" + iSetId + " and DSTempStepId=" + iStepId + " AND IsDynamic=0";
                        nextStep = oData.FetchValue(sqlStr).ToString();
                        if (type == "Backward chain")
                        {
                            nextStep = (stepcount - Convert.ToInt32(nextStep) + 1).ToString();
                        }
                        if (nextStep == null && nextStep == "0")
                            nextStep = "1";
                    }
                    else
                    {
                        tdMsg.InnerHtml = clsGeneral.failedMsg("Override Failed. No steps found.");//no steps for the set
                        return;
                    }
                }
                sqlStr = "select SortOrder from DSTempSet where DSTempHdrId=" + oTemp.TemplateId + " "
                        + "and DSTempSetId=" + iSetId;
                int nextSet = Convert.ToInt32(oData.FetchValue(sqlStr).ToString());
                if (nextSet == null && nextSet == 0)
                    nextSet = 1;
                if (RadioButtonListPrompts.Items.Count > 0)
                {
                    promptId = Convert.ToInt32(RadioButtonListPrompts.SelectedItem.Value);
                    sqlStr = "select PromptId from DSTempPrompt where DSTempHdrId=" + oTemp.TemplateId + " "
                            + "and DSTempPromptId=" + promptId;
                    promptId = Convert.ToInt32(oData.FetchValue(sqlStr).ToString());
                }
                int id = 0;
                //if (RadioButtonListSteps.Items.Count > 0)
                //{
                string strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId + ",NextStepId=" + nextStep + ", NextSetNmbr=" + nextSet + ","
                    + "NextPromptId='" + promptId + "',ModifiedBy=" + oSession.LoginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId + "";
                id = oData.Execute(strQuery);
                // }
                //else
                //{
                //    tdMsg.InnerHtml = clsGeneral.failedMsg("Override Failed. No steps found.");//no steps for the set
                //    return;
                //}
                if (id > 0)
                {
                    //string updQry = "update StdtSessionHdr SET AssignedToId=1,EndTs=GETDATE(),SessionStatusCd='S',SessMissTrailStus='" + sessMistrial + "',"
                    //    + " CurrentSetId=" + iSetId + ", CurrentStepId=" + nextStep + ", CurrentPromptId=" + promptId + ","
                    //               + "Comments='" + txtNote.Text.Trim() + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"] + "";
                    string updQry = "DELETE FROM StdtSessionHdr WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"];
                    int retrn = oData.Execute(updQry);
                    if (retrn > 0)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe(" + oSession.StudentId + ");", true);
                    else tdMsg.InnerHtml = clsGeneral.failedMsg("Override Failed");
                }
                //else tdMsg.InnerHtml = clsGeneral.failedMsg("Override Failed");
            }
            else
            {
                sqlStr = "select SortOrder from DSTempSet where DSTempHdrId=" + oTemp.TemplateId + " "
                           + "and DSTempSetId=" + iSetId_tt;
                int nextSet = Convert.ToInt32(oData.FetchValue(sqlStr).ToString());
                if (nextSet == null && nextSet == 0)
                    nextSet = 1;


                string strQuery = "UPDATE StdtDSStat SET NextSetId=" + iSetId_tt + ", NextSetNmbr=" + nextSet + ","
                    + "ModifiedBy=" + oSession.LoginId + " ,ModifiedOn=GETDATE() WHERE DSTempHdrId=" + oTemp.TemplateId + "";
                int id = oData.Execute(strQuery);
                if (id > 0)
                {

                    foreach (RepeaterItem ri in rptr_listStep.Items)
                    {

                        if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                        {
                            CheckBox checkBoxInRepeater = ri.FindControl("stepCheckBox") as CheckBox;

                            if (checkBoxInRepeater.Checked == true)
                            {
                                HiddenField stepId = ri.FindControl("hdn_stepId") as HiddenField;
                                DropDownList promptDDL = ri.FindControl("stepDDL") as DropDownList;

                                if (promptDDL.SelectedValue != "")
                                {
                                    if (oData.IFExists("select StdtDSStepStatid from StdtDSStepStat where DSTempStepId = " + stepId.Value + "") == true)
                                    {
                                        string query = "UPDATE StdtDSStepStat set PromptId = " + promptDDL.SelectedValue + " where DSTempStepId = " + stepId.Value;
                                        id = oData.Execute(query);
                                    }
                                    else
                                    {

                                        string insertStepStats = "INSERT INTO StdtDSStepStat (SchoolId,StudentId,DSTempStepId,PromptId,CreatedBy,CreatedOn) " +
                                                                    "VALUES(" + oSession.SchoolId + "," + oSession.StudentId + "," + stepId.Value + "," +
                                                                    "" + promptDDL.SelectedValue + "," + oSession.LoginId + ",GETDATE())";
                                        id = oData.Execute(insertStepStats);
                                    }
                                }
                            }


                        }
                    }


                    //string updQry = "update StdtSessionHdr SET AssignedToId=1,EndTs=GETDATE(),SessionStatusCd='S',SessMissTrailStus='" + sessMistrial + "',"
                    //    + " CurrentSetId=" + iSetId + ", CurrentStepId=" + nextStep + ", CurrentPromptId=" + promptId + ","
                    //               + "Comments='" + txtNote.Text.Trim() + "',ModifiedBy=" + oSession.LoginId + ",ModifiedOn=GETDATE() WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"] + "";
                    string updQry = "DELETE FROM StdtSessionHdr WHERE StdtSessionHdrId=" + ViewState["StdtSessHdr"];
                    int retrn = oData.Execute(updQry);
                    if (retrn > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe(" + oSession.StudentId + ");", true);
                    }
                    else
                    {
                        tdMsg.InnerHtml = clsGeneral.failedMsg("Override Failed");
                    }


                }

            }
            ResetIOAStatus(oTemp.TemplateId);
            clearOverrideSession(oTemp.TemplateId.ToString());
        }

        catch (Exception ex)
        {
            tdMsg.InnerHtml = clsGeneral.failedMsg("Override Failed");
            ClsErrorLog error = new ClsErrorLog();
            error.WriteToLog("Override Failed" + ex);
        }


    }
    protected void rptr_listStep_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DropDownList promptList = e.Item.FindControl("stepDDL") as DropDownList;
            HiddenField hdn_dsTempHdrId = e.Item.FindControl("hdn_dsTempHdrId") as HiddenField;
            HiddenField hdn_promptIdValue = e.Item.FindControl("hdn_stepDDLValue") as HiddenField;

            System.Data.DataSet ds = new System.Data.DataSet();
            ds = getPromptList(hdn_dsTempHdrId.Value);

            promptList.DataSource = ds.Tables[0];
            promptList.DataTextField = "LookupName";
            promptList.DataValueField = "PromptId";

            promptList.DataBind();

            promptList.Items.Insert(0, new ListItem("--- Select ---", ""));

            promptList.SelectedValue = hdn_promptIdValue.Value;
        }
    }

    public System.Data.DataSet getPromptList(string dsTempHdrId)
    {

        string sqlStr = "";
        string type = "";
        DataSet ds = new DataSet();
        ds = null;
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        if (oTemp != null)
        {
            sqlStr = "SELECT lk.LookupName FROM DSTempHdr hd inner join lookup lk on lk.LookupId=hd.PromptTypeId where DSTempHdrId=" + oTemp.TemplateId;
            type = oData.FetchValue(sqlStr).ToString();
            oSession = (clsSession)HttpContext.Current.Session["UserSession"];
            if (oSession != null)
            {


                if ((type == "Least-to-Most")||(type == "Graduated Guidance"))
                {
                    sqlStr = "SELECT dstp.DSTempPromptId, dstp.DSTempHdrId, dstp.PromptId, dstp.PromptOrder, lu.LookupName FROM DSTempPrompt as dstp join [LookUp] as lu on dstp.PromptId = lu.LookupId WHERE (dstp.DSTempHdrId = " + dsTempHdrId + " AND dstp.ActiveInd = 'A') ORDER BY SortOrder DESC";
                    //sqlStr = "SELECT LU.LookupId as Id,LU.LookupName as Name,DS.DSTempPromptid FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                    //                        " DS.ActiveInd='A' AND DS.DSTempHdrId=" + oTemp.TemplateId + " ORDER BY PromptOrder";
                }
                else
                {
                    sqlStr = "SELECT dstp.DSTempPromptId, dstp.DSTempHdrId, dstp.PromptId, dstp.PromptOrder, lu.LookupName FROM DSTempPrompt as dstp join [LookUp] as lu on dstp.PromptId = lu.LookupId WHERE (dstp.DSTempHdrId = " + dsTempHdrId + " AND dstp.ActiveInd = 'A') ORDER BY SortOrder";
                    //sqlStr = "SELECT LU.LookupId as Id,LU.LookupName as Name,DS.DSTempPromptid FROM LookUp LU INNER JOIN DSTempPrompt DS ON DS.PromptId=LU.LookupId WHERE" +
                    //           " DS.ActiveInd='A' AND DS.DSTempHdrId=" + oTemp.TemplateId + " ORDER BY PromptOrder DESC";
                }
                //string sqlStr = "SELECT dstp.DSTempPromptId, dstp.DSTempHdrId, dstp.PromptId, dstp.PromptOrder, lu.LookupName FROM DSTempPrompt as dstp join [LookUp] as lu on dstp.PromptId = lu.LookupId WHERE (DSTempHdrId = " + dsTempHdrId + ")";
                //sqlStr = "select DP.DSTempPromptid,LK.LookupName,DP.PromptOrder from DSTempPrompt DP join LookUp LK on LK.LookupId=DP.PromptId where DSTempHdrId=" + oTemp.TemplateId + " order by DP.PromptOrder";
                ds = oData.ReturnDataSet(sqlStr, false);


            }
        }

        return ds;
        //string sqlStr = "SELECT dstp.DSTempPromptId, dstp.DSTempHdrId, dstp.PromptId, dstp.PromptOrder, lu.LookupName FROM DSTempPrompt as dstp join [LookUp] as lu on dstp.PromptId = lu.LookupId WHERE (DSTempHdrId = " + dsTempHdrId + ")";
        //System.Data.DataSet ds = new System.Data.DataSet();
        //ds = oData.ReturnDataSet(sqlStr, false);
        //int x = ds.Tables[0].Rows.Count;
        //return ds;
    }
    protected void btn_sets_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        loadStepOverrid(Convert.ToInt32(btn.ToolTip));
    }
    protected void RadioButtonListSets_tt_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadStepOverrid(Convert.ToInt32(RadioButtonListSets_tt.SelectedValue.ToString()));
    }
    protected void rptr_tempOverride_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            oData = new clsData();

            //HiddenField hdn_currSetId = e.Item.FindControl("hdn_currSetId") as HiddenField;
            HiddenField hdn_tempSetId = e.Item.FindControl("hdn_tempSetId") as HiddenField;
            //HiddenField hdn_currSetNmbr = e.Item.FindControl("hdn_currSetNmbr") as HiddenField;
            HiddenField hdn_tempSetNmbr = e.Item.FindControl("hdn_tempSetNmbr") as HiddenField;
            Button btn_go = e.Item.FindControl("btn_go") as Button;
            Label btn_list_crr = e.Item.FindControl("btn_list_crr") as Label;
            Label lblDraft = e.Item.FindControl("lblDraft") as Label;

            Panel pnl_set = e.Item.FindControl("pnl_set") as Panel;





            //string query = "select * from StdtSessionHdr where SessionStatusCd = 'D' and CurrentSetId='" + hdn_tempSetId.Value + "'";
            //DataTable dt = oData.ReturnDataTable(query, false);
            //if (dt.Rows.Count > 0)
            //{
            //    lblDraft.Visible = true;
            //}

            //if (Convert.ToInt32(hdn_tempSetNmbr.Value) > Convert.ToInt32(hdn_currSetNmbr.Value))
            //{
            //    pnl_set.Visible = false;
            //}
            //else if (Convert.ToInt32(hdn_tempSetNmbr.Value) == Convert.ToInt32(hdn_currSetNmbr.Value))
            //{
            //    btn_go.Visible = false;
            //    //btn_list_crr.Visible = true;
            //    btn_list_crr.Visible = false;
            //    lblDraft.Visible = false;
            //}



        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {

        Button btn = (Button)sender;
        RepeaterItem ritem = (RepeaterItem)btn.NamingContainer;
        HiddenField hdn_tempSetNmbr = (HiddenField)ritem.FindControl("hdn_tempSetNmbr");
        HiddenField hdn_tempSetId = (HiddenField)ritem.FindControl("hdn_tempSetId");


        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oDS != null)
        {
            oDS.IOAInd = "N";

            hdn_currTempSetNmbr.Value = hdn_tempSetNmbr.Value;
            hdn_currTempSet.Value = hdn_tempSetId.Value;
            SqlConnection con = null;
            SqlTransaction trans = null;

            oData = new clsData();
            oSession = (clsSession)Session["UserSession"];
            oTemp = (ClsTemplateSession)Session["BiweeklySession"];



            oData = new clsData();
            string sel = "SELECT * FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND StdtClassId=" + oSession.Classid + " AND DSTempHdrId=" + oTemp.TemplateId + " AND SessionStatusCd='D' AND CurrentSetId = '" + hdn_tempSetId.Value + "'";
            object objSessNbr = oData.FetchValue("SELECT ISNULL(MAX(SessionNbr),0)+1 FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + /*" AND StdtClassId=" + oSession.Classid +*/ " AND LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId + ")");
            if (objSessNbr == null)
                objSessNbr = 0;
            oDS.SessNbr = (int)objSessNbr;
            DataTable dtHdrs = oData.ReturnDataTable(sel, false);

            if (dtHdrs != null)
            {
                if (dtHdrs.Rows.Count == 0)
                {
                    //Create new Draft for teacher; IsMaintanance = true

                    hdn_isMaintainance.Value = "true";

                    con = oData.Open();
                    trans = con.BeginTransaction();
                    generateSheet(true);
                    bool reslt = SaveDraft("D", "N", "insert", con, trans);
                    if (reslt)
                    {
                        bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                        if (reslt2) oData.CommitTransation(trans, con);
                        else if (trans.Connection.State == ConnectionState.Open) oData.RollBackTransation(trans, con);
                        con.Close();
                        getStepPrompts();
                        fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);

                    }
                    else if (trans.Connection.State == ConnectionState.Open)
                    {
                        oData.RollBackTransation(trans, con);
                        con.Close();
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "", "probe();", true);
                }
            }
            if (dtHdrs.Rows.Count == 1)
            {
                if (dtHdrs.Rows[0]["IsMaintanace"].ToString() == "True")
                {
                    hdn_isMaintainance.Value = "true";

                    if (dtHdrs.Rows[0]["IOAInd"].ToString() == "Y")
                    {
                        //open ioa's draft...
                        ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        oDS.IOAInd = "Y";
                        oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["IOASessionHdrId"].ToString());
                        LoadData(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), true);
                    }
                    else
                    {
                        // Automatically load the drafted Data Sheet


                        Response.Redirect("DatasheetPreview.aspx?SessHdrID=" + dtHdrs.Rows[0]["StdtSessionHdrId"].ToString() + "&isMaint=true");
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "", "probe();", true);
                }
                else
                {
                    //A draft which is not "IsMaintanance" is available.

                    hdn_isMaintainance.Value = "false";

                    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('There is already a draft available. Opening it in normal mode...');", true);

                    if (dtHdrs.Rows[0]["IOAInd"].ToString() == "Y")
                    {
                        //open ioa's draft...
                        ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        oDS.IOAInd = "Y";
                        oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["IOASessionHdrId"].ToString());
                        LoadData(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), true);
                    }
                    else
                    {   //ask whether to create an ioa draft or open existing draft...

                        ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["StdtSessionHdrId"].ToString());
                        ClientScript.RegisterStartupScript(this.GetType(), "", "showPop(2);alertBox('There is already a draft available. Opening it in normal mode...');", true);


                        string selQry = "SELECT UserLName+' '+UserFName AS Name,Hdr.CreatedOn,SessionNbr FROM StdtSessionHdr Hdr INNER JOIN [User] Usr ON Usr.UserId=Hdr.CreatedBy " +
                                "WHERE StdtSessionHdrId=" + dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        DataTable dtUsr = oData.ReturnDataTable(selQry, false);
                        if ((dtUsr != null) && (dtUsr.Rows.Count > 0))
                        {
                            lblUName1.InnerHtml = dtUsr.Rows[0]["Name"].ToString();
                            LblStrtTime1.InnerHtml = dtUsr.Rows[0]["CreatedOn"].ToString();
                            lblSessNo1.InnerHtml = dtUsr.Rows[0]["SessionNbr"].ToString();
                        }
                    }
                }
            }
        }
    }

    protected void btn_continue_Click(object sender, EventArgs e)
    {
        SqlConnection con = null;
        SqlTransaction trans = null;
        oData = new clsData();

        //Create new Draft for teacher..

        hdn_isMaintainance.Value = "false";

        con = oData.Open();
        trans = con.BeginTransaction();
        generateSheet(true);
        bool reslt = SaveDraft("D", "N", "insert", con, trans);
        if (reslt)
        {
            bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
            if (reslt2) oData.CommitTransation(trans, con);
            else if (trans.Connection.State == ConnectionState.Open) oData.RollBackTransation(trans, con);
            con.Close();
            getStepPrompts();
            fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);

        }
        else if (trans.Connection.State == ConnectionState.Open)
        {
            oData.RollBackTransation(trans, con);
            con.Close();
        }
    }
    protected void btn_contIOASess_Click(object sender, EventArgs e)
    {
        //open ioa's draft...
        ViewState["StdtSessHdr"] = hdn_StdtSessionHdrId.Value;
        oDS.IOAInd = "Y";
        oDS.IOASessHdr = Convert.ToInt32(hdn_IOASessionHdrId.Value);
        LoadData(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), true);
    }
    protected void btnSubmitAndRepeat_Click(object sender, EventArgs e)
    {
        oData = new clsData();
        string currSetIdTemp = "";
        Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
        if (htLpList != null)
        {
            string lpId = oTemp.TemplateId.ToString();
            LinkedList<KeyValuePair<string, Hashtable>> llKvpSetList = (LinkedList<KeyValuePair<string, Hashtable>>)htLpList[lpId];
            if (llKvpSetList != null)
            {
                if (llKvpSetList.Count > 0)
                {
                    currSetIdTemp = llKvpSetList.First.Value.Value["tempSetId"].ToString();
                }
            }
        }
        if (currSetIdTemp == "")
        {
            if (Request.QueryString["currSetId"] != null)
            {
                currSetIdTemp = Request.QueryString["currSetId"].ToString();
            }
        }
        //submit the datasheet, increment the repeat count, reload the datasheet
        string isMaintStatus = hdn_isMaintainance.Value;
        btnSubmit_Click(sender, e);
        if (lblSubmitAndRepeatCount.Text != "")
        {
            repeatNo = Convert.ToInt32(lblSubmitAndRepeatCount.Text);
        }
        repeatNo++;

        Response.Redirect("DatasheetPreview.aspx?pageid=" + oTemp.TemplateId + "&studid=" + oSession.StudentId + "&SRMode=true&repeatNo=" + repeatNo + "&isMaint=" + isMaintStatus + "&currSetIdTemp=" + currSetIdTemp + "&exc=false");
    }
    protected void btn_new_continue_Click(object sender, EventArgs e)
    {
        string isMaintVal = "";
        string LpId = oTemp.TemplateId.ToString();
        Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
        if (htLpList == null)
        {
            htLpList = new Hashtable();
        }
        Hashtable htSetList = new Hashtable();
        LinkedList<KeyValuePair<string, Hashtable>> llKvpSetList = new LinkedList<KeyValuePair<string, Hashtable>>();

        //add LP ids to htLpList
        //add checked sets' ids to llKvpSetList
        //add set details to htSetDetails
        foreach (RepeaterItem rItem in rptr_tempOverride.Items)
        {
            Hashtable htSetDetails = new Hashtable();
            CheckBox chkTempOverride = (CheckBox)rItem.FindControl("tempOverrideCheckBox");

            if (chkTempOverride.Checked)
            {
                hdn_displayTempOverride.Value = "true";

                //HiddenField hdn_currSetId = (HiddenField)rItem.FindControl("hdn_currSetId");
                HiddenField hdn_tempSetId = (HiddenField)rItem.FindControl("hdn_tempSetId");
                //HiddenField hdn_currSetNmbr = (HiddenField)rItem.FindControl("hdn_currSetNmbr");
                HiddenField hdn_tempSetNmbr = (HiddenField)rItem.FindControl("hdn_tempSetNmbr");

                //if (Convert.ToInt32(hdn_tempSetNmbr.Value) < Convert.ToInt32(hdn_currSetNmbr.Value))
                //{
                isMaintVal = "true";
                //}
                //else if (Convert.ToInt32(hdn_tempSetNmbr.Value) == Convert.ToInt32(hdn_currSetNmbr.Value))
                //{
                //    isMaintVal = "false";
                //}

                //fill htSetDetails
                // htSetDetails.Add("currSetId", hdn_currSetId.Value);
                htSetDetails.Add("tempSetId", hdn_tempSetId.Value);
                // htSetDetails.Add("currSetNmbr", hdn_currSetNmbr.Value);
                htSetDetails.Add("tempSetNmbr", hdn_tempSetNmbr.Value);
                htSetDetails.Add("isMaintVal", isMaintVal);

                //fill setList
                llKvpSetList.AddLast(new KeyValuePair<string, Hashtable>(hdn_tempSetId.Value, htSetDetails));
            }
        }
        //fill htLpList
        if (!htLpList.ContainsKey(LpId))
        {
            htLpList.Add(LpId, llKvpSetList);
        }
        else
        {
            htLpList.Remove(LpId);
            htLpList.Add(LpId, llKvpSetList);
        }

        //assign the hash table to session
        Session["tempOverrideHT"] = htLpList;
        LoadDatasheet(LpId);
        int MaintStatusId = Convert.ToInt16(oData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Maintenance' "));
        int AprovdStatusId = Convert.ToInt16(oData.FetchValue("SELECT  LookupId FROM LookUp WHERE LookupType='TemplateStatus' And LookupName='Approved' "));
        int LsnStatusId = Convert.ToInt16(oData.FetchValue("select statusid from DSTempHdr where DSTempHdrId =" + oTemp.TemplateId + " "));
       // if ((MaintStatusId == LsnStatusId) || (AprovdStatusId == LsnStatusId))
        // if ((MaintStatusId == LsnStatusId))
       // {
            string dtDelQry = null;
            string DelQry = "SELECT StdtSessionHdrId FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND StdtClassId=" + oSession.Classid + " AND DSTempHdrId=" + oTemp.TemplateId + " AND SessionStatusCd='P'";
            DataTable dtDelQrys = oData.ReturnDataTable(DelQry, false);
            if (dtDelQrys.Rows.Count > 0)
            {
                dtDelQry = dtDelQrys.Rows[0]["StdtSessionHdrId"].ToString();
                string check = null;
                check = dtDelQry.ToString();
                if (check != null)
                {
                    string updQry = "DELETE FROM StdtSessionHdr WHERE StdtSessionHdrId=" + check;
                    int retrn = oData.Execute(updQry);
                }
            }
       // }
            htLpList = new Hashtable();
            Session["tempOverrideHT"] = htLpList;
       

    }


    public void Continue_IsM_True(string tempSetId, string tempSetNmbr)
    {
        oDS = (clsDataSheet)Session[DatasheetKey];
        if (oDS != null)
        {
            oDS.IOAInd = "N";

            hdn_currTempSetNmbr.Value = tempSetNmbr;
            hdn_currTempSet.Value = tempSetId;
            SqlConnection con = null;
            SqlTransaction trans = null;

            oData = new clsData();
            oSession = (clsSession)Session["UserSession"];
            oTemp = (ClsTemplateSession)Session["BiweeklySession"];

            oData = new clsData();
            string sel = "SELECT * FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + " AND StdtClassId=" + oSession.Classid + " AND DSTempHdrId=" + oTemp.TemplateId + " AND SessionStatusCd='P' AND CurrentSetId = '" + tempSetId + "'";
            //object objSessNbr = oData.FetchValue("SELECT ISNULL(MAX(SessionNbr),0)+1 FROM StdtSessionHdr WHERE StudentId=" + oSession.StudentId + " AND SchoolId=" + oSession.SchoolId + /*" AND StdtClassId=" + oSession.Classid +*/ " AND LessonPlanId=(SELECT LessonPlanId FROM DSTempHdr WHERE DSTempHdrId=" + oTemp.TemplateId + ")");
            // if (objSessNbr == null)
            object objSessNbr = 0;
            oDS.SessNbr = (int)objSessNbr;
            DataTable dtHdrs = oData.ReturnDataTable(sel, false);

            if (dtHdrs != null)
            {
                if (dtHdrs.Rows.Count == 0)
                {
                    //Create new Draft for teacher; IsMaintanance = true
                    //hdn_isMaintainance.Value = "true";

                    con = oData.Open();
                    trans = con.BeginTransaction();
                    generateSheet(true);
                    bool reslt = SaveDraft("P", "N", "insert", con, trans);
                    if (reslt)
                    {
                        bool reslt2 = SaveMeasuremnts(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), con, trans);
                        if (reslt2) oData.CommitTransation(trans, con);
                        else if (trans.Connection.State == ConnectionState.Open) oData.RollBackTransation(trans, con);
                        con.Close();
                        getStepPrompts();
                        fillStepGrid(oDS.TeachProc, oDS.SkillType, oDS.MatchToSampleType);

                    }
                    else if (trans.Connection.State == ConnectionState.Open)
                    {
                        oData.RollBackTransation(trans, con);
                        con.Close();
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "", "probe();", true);
                }
            }
            if (dtHdrs.Rows.Count == 1)
            {
                if (dtHdrs.Rows[0]["IsMaintanace"].ToString() == "True")
                {
                    hdn_isMaintainance.Value = "true";

                    if (dtHdrs.Rows[0]["IOAInd"].ToString() == "Y")
                    {
                        //open ioa's draft...
                        ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        oDS.IOAInd = "Y";
                        oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["IOASessionHdrId"].ToString());
                        LoadData(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), true);
                    }
                    else
                    {
                        //Automatically load the drafted Data Sheet
                        Response.Redirect("DatasheetPreview.aspx?SessHdrID=" + dtHdrs.Rows[0]["StdtSessionHdrId"].ToString() + "&isMaint=true&exc=true");
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "", "probe();", true);
                }
                else
                {
                    //A draft which is not "IsMaintanance" is available.
                    hdn_isMaintainance.Value = "false";

                    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('There is already a draft available. Opening it in normal mode...');", true);
                    if (dtHdrs.Rows[0]["IOAInd"].ToString() == "Y")
                    {
                        //open ioa's draft...
                        ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        oDS.IOAInd = "Y";
                        oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["IOASessionHdrId"].ToString());
                        LoadData(Convert.ToInt32(ViewState["StdtSessHdr"].ToString()), true);
                    }
                    else
                    {   //ask whether to create an ioa draft or open existing draft...
                        ViewState["StdtSessHdr"] = dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        oDS.IOASessHdr = Convert.ToInt32(dtHdrs.Rows[0]["StdtSessionHdrId"].ToString());
                        ClientScript.RegisterStartupScript(this.GetType(), "", "showPop(2);alertBox('There is already a draft available. Opening it in normal mode...');", true);

                        string selQry = "SELECT UserLName+' '+UserFName AS Name,Hdr.CreatedOn,SessionNbr FROM StdtSessionHdr Hdr INNER JOIN [User] Usr ON Usr.UserId=Hdr.CreatedBy " +
                                "WHERE StdtSessionHdrId=" + dtHdrs.Rows[0]["StdtSessionHdrId"].ToString();
                        DataTable dtUsr = oData.ReturnDataTable(selQry, false);
                        if ((dtUsr != null) && (dtUsr.Rows.Count > 0))
                        {
                            lblUName1.InnerHtml = dtUsr.Rows[0]["Name"].ToString();
                            LblStrtTime1.InnerHtml = dtUsr.Rows[0]["CreatedOn"].ToString();
                            lblSessNo1.InnerHtml = dtUsr.Rows[0]["SessionNbr"].ToString();
                        }
                    }
                }
            }
        }
    }
    public void Continue_IsM_False(string currSetId, string tempSetId, string currSetNmbr, string tempSetNmbr)
    {
        string hasRNo = Request.QueryString["repeatNo"];
        string strSRMode = "false";
        if (hasRNo != null)
        {
            strSRMode = "true";
        }
        //pramod
        Response.Redirect("DatasheetPreview.aspx?pageid=" + oTemp.TemplateId + "&studid=" + oSession.StudentId + "&exc=true&SRMode=" + strSRMode + "&repeatNo=" + hasRNo + "");
    }

    public void LoadDatasheet(string LPId)
    {
        oSession = (clsSession)Session["UserSession"];
        Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
         if (htLpList != null)
         {
            if (htLpList.Count != 0)
            {
                LinkedList<KeyValuePair<string, Hashtable>> llKvpSetList = (LinkedList<KeyValuePair<string, Hashtable>>)htLpList[LPId];
                if (llKvpSetList != null)
                {
                    if (llKvpSetList.Count > 0)
                    {
                        //gets the value of first element in the linked list
                        Hashtable htSetDetails = llKvpSetList.First.Value.Value;

                    //string currSetId = htSetDetails["currSetId"].ToString();
                    string tempSetId = htSetDetails["tempSetId"].ToString();
                    //string currSetNmbr = htSetDetails["currSetNmbr"].ToString();
                    string tempSetNmbr = htSetDetails["tempSetNmbr"].ToString();
                    string tempIsMaint = htSetDetails["isMaintVal"].ToString();

                    if (tempIsMaint == "true")
                    {
                        Continue_IsM_True(tempSetId, tempSetNmbr);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closewindow", "closeIframe1(" + oSession.StudentId + ");", true);
                }
            }
		  }
        }
    }


    [System.Web.Services.WebMethod]
    public static void resetOverrideSession(string sheetId)
    {
        StudentBinder_DatasheetPreview obj = new StudentBinder_DatasheetPreview();
        obj.clearOverrideSession(sheetId);
    }

    public void clearOverrideSession(string lpId)
    {
        Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
        if (htLpList != null)
        {
            if (htLpList.Count > 0)
            {
                htLpList.Remove(lpId);
            }
        }
    }

    [WebMethod]
    public static string ViewLessonPlanData(string Id)
    {
        objData = new clsData();
        DataTable dtLPDetail = objData.ReturnDataTable("SELECT DSTempHdrId,StudentId,LessonPlanId,(SELECT GoalId FROM StdtLessonplan SLP WHERE SLP.StdtLessonplanId=DSTempHdr.StdtLessonplanId) GoalId,StdtLessonplanId FROM DSTempHdr WHERE DSTempHdrId=(SELECT DSTempHdrId FROM StdtSessionHdr WHERE StdtSessionHdrId='" + Id + "')", false);

        return "LessonPlanAttributes.aspx?pageid=" + dtLPDetail.Rows[0]["DSTempHdrId"] + "&studid=" + dtLPDetail.Rows[0]["StudentId"] + "&ViewPopUp=" + 1;
        //+ "&lessonId=" + dtLPDetail.Rows[0]["LessonPlanId"] + "&goalId=" + dtLPDetail.Rows[0]["GoalId"] + "&delLpID=" + dtLPDetail.Rows[0]["StdtLessonplanId"];

    }
    protected void btnVLP_Click(object sender, EventArgs e)
    {
    }
    protected void btnAddTrial_Click(object sender, EventArgs e)
    {
        oData = new clsData();
        oTemp = (ClsTemplateSession)Session["BiweeklySession"];
        oSession = (clsSession)Session["UserSession"];
        oDS = (clsDataSheet)Session[DatasheetKey];
        Hashtable htLpList = (Hashtable)Session["tempOverrideHT"];
        int CurrentSet = 0;

        SqlConnection con = null;
        SqlTransaction trans = null;
        try
        {
            Session["ISAddTrial"] = "True";
            SaveDraft();
            tdMsg.InnerHtml = "";

            string sqlStr = "SELECT DSTempSetColId,ColName,ColTypeCd,CorrRespDesc,CorrResp,InCorrRespDesc,IncMisTrialInd,CalcuType," +
                                   "CASE(ColTypeCd) WHEN '+/-' THEN '' WHEN 'Prompt' THEN '0' WHEN 'Duration' THEN '00:00:00' " +
                                   "WHEN 'Frequency' THEN '0' WHEN 'Text' THEN '0' END as ColValue, " +
                                   "CASE(ColTypeCd) WHEN '+/-' THEN 'Radio' WHEN 'Prompt' THEN 'DropDown' WHEN 'Duration' THEN 'Timer' " +
                                   "WHEN 'Frequency' THEN 'Freq' WHEN 'Text' THEN 'Text' END as ColControl " +
                                   "FROM DSTempSetCol WHERE " +
                                   "DSTempHdrId=" + oTemp.TemplateId + " AND SchoolId = " + oSession.SchoolId + "  AND ActiveInd='A' ORDER BY DSTempSetColId";
            DataTable dt = oData.ReturnDataTable(sqlStr, false);
            oDS.dtColumns = dt;

            con = oData.Open();
            trans = con.BeginTransaction();



            int crntPrmpt = Convert.ToInt32(Session["TargetPrompt"]);
            int DSTempStep = 0;
            //string UpdateQuery = "UPDATE DSTempHdr SET NbrOfTrials=((SELECT NbrOfTrials FROM DSTempHdr WHERE DSTempHdrId='" + oTemp.TemplateId + "')+1) WHERE DSTempHdrId='" + oTemp.TemplateId + "' ";
            //int dstmphdr = oData.ExecuteWithTrans(UpdateQuery, con, trans);

            if (oDS.TeachProc == "Match-to-Sample")
            {

                if (htLpList != null)
                {
                    string lpId = oTemp.TemplateId.ToString();
                    LinkedList<KeyValuePair<string, Hashtable>> llKvpSetList = (LinkedList<KeyValuePair<string, Hashtable>>)htLpList[lpId];
                    if (llKvpSetList != null)
                    {
                        if (llKvpSetList.Count > 0)
                        {
                            int cnt = 0;
                            foreach (KeyValuePair<string, Hashtable> abc in llKvpSetList)
                            {
                                if (cnt == 0)
                                {
                                    string checkedId = abc.Key;
                                    string tempSetId = abc.Value["tempSetId"].ToString();
                                    CurrentSet = Convert.ToInt32(tempSetId);
                                    string currSetId = abc.Value["currSetId"].ToString();
                                }
                                cnt++;
                            }
                        }
                    }
                }

                if (CurrentSet == 0)
                {
                    CurrentSet = oDS.CrntSet;
                }


                sqlStr = "SELECT [DSTempStepId],[StepName] as StepCd,[StepName],SortOrder as StepId  FROM [dbo].[DSTempStep] " +
                            " WHERE DSTempHdrId=" + oTemp.TemplateId + " AND DsTempSetId=" + CurrentSet + " AND ActiveInd='A' ORDER BY [SortOrder]";
                dt = oData.ReturnDataTableWithTransaction(sqlStr, con, trans, false);
                string[] stepname = new string[dt.Rows.Count];

                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        stepname[j] = dt.Rows[j]["StepName"].ToString();
                    }
                    string StpName = "";
                    int length = 0;
                    string setMatch = "";
                    Random rnd = new Random();
                    if (oDS.MatchToSampleType == "Randomized")
                    {
                        string[] shuffleItem = Shuffle(stepname);
                        string[] shuffleItemdetail = shuffleItem[(dt.Rows.Count - 1)].Split('[');
                        string[] samples = Shuffle((shuffleItemdetail[1].Split(']'))[0].Split(','));
                        for (int arryInt = 0; arryInt < samples.Count(); arryInt++)
                        {
                            setMatch += samples[arryInt].ToString().Trim() + ", ";
                        }
                        length = setMatch.Length;
                        setMatch = setMatch.ToString().Substring(0, length - 2);
                        int FindIndex = rnd.Next(0, samples.Length);
                        StpName = "Find " + samples[FindIndex] + ":  [" + setMatch + "]";

                    }
                    else
                    {
                        string[] shuffleItem = Shuffle(stepname);
                        string[] shufItemdetail = shuffleItem[(dt.Rows.Count - 1)].Split('[');
                        string[] shuffleItemdetail = stepname[(dt.Rows.Count - 1)].Split('[');
                        string[] samples = (shuffleItemdetail[1].Split(']'))[0].Split(',');
                        setMatch = samples[(samples.Count() - 1)].ToString() + ", ";

                        for (int arryInt = 1; arryInt < samples.Count(); arryInt++)
                        {
                            setMatch += samples[arryInt].ToString().Trim() + ", ";
                        }
                        length = setMatch.Length;
                        setMatch = setMatch.ToString().Substring(0, length - 2);
                        int FindIndex = rnd.Next(0, samples.Length);
                        StpName = "Find " + samples[FindIndex] + ":  [" + setMatch + "]";
                        //StpName = shufItemdetail[0] + "[" + setMatch + "]";
                    }

                    sqlStr = "INSERT INTO DSTempStep (SchoolId,DSTempHdrId,DSTempSetId,StepName,SortOrder,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,IsDynamic) " +
                    "Values(" + oSession.SchoolId + "," + oTemp.TemplateId + "," + CurrentSet + ",'" + StpName + "'," + ((dt.Rows.Count) + 1) + ",'A'," + oSession.LoginId + ",getdate()," + oSession.LoginId + ",getdate(),1) ";
                    DSTempStep = oData.ExecuteWithScopeandConnection(sqlStr, con, trans);
                }
            }

            int Trial = 0;
            Trial = Convert.ToInt32(oData.FetchValueTrans("SELECT COUNT(*) FROM StdtSessionStep WHERE StdtSessionHdrId='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "'", trans, con));

            string InsertQry = "INSERT INTO [dbo].[StdtSessionStep](StdtSessionHdrId,DSTempStepId,TrialNbr,SessionStatusCd,CreatedBy,CreatedOn) VALUES ('" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "','" + DSTempStep + "','" + Trial + "','NAV','" + oSession.LoginId + "',GETDATE())";
            int stdtsessstepId = oData.ExecuteWithScopeandConnection(InsertQry, con, trans);

            DataTable dtcolumns = oDS.dtColumns;
            int dtlId = 0;
            if (dtcolumns != null)
            {
                foreach (DataRow drColmn in dtcolumns.Rows)
                {
                    string insertSessionSetDtlQuery = "INSERT INTO StdtSessionDtl(StdtSessionStepId,DSTempSetColId,StepVal,CurrentPrompt,RowNumber,CreatedBy,CreatedOn)"
                                                        + " VALUES(" + stdtsessstepId + "," + drColmn["DSTempSetColId"].ToString() + ",'" + drColmn["ColValue"].ToString() + "'," + crntPrmpt + ",(SELECT (NbrOfTrials) FROM DSTempHdr WHERE DSTempHdrId='" + oTemp.TemplateId + "')," + oSession.LoginId + ",GETDATE())";
                    dtlId = oData.ExecuteWithScopeandConnection(insertSessionSetDtlQuery, con, trans);
                }
            }


            if (DSTempStep > 0)
            {
                string IsTrial = Convert.ToString(oData.FetchValueTrans("SELECT IsTrial FROM StdtSessionHdr WHERE StdtSessionHdrId='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "'", trans, con));

                if (IsTrial != null && IsTrial != "")
                {
                    IsTrial = IsTrial + "," + DSTempStep.ToString();
                }
                else
                {
                    IsTrial = DSTempStep.ToString();
                }
                string UpdateQuery = "UPDATE StdtSessionHdr SET IsTrial='" + IsTrial + "' WHERE StdtSessionHdrId='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "' ";
                int sesshdr = oData.ExecuteWithTrans(UpdateQuery, con, trans);
            }

            //if (Convert.ToInt32(oData.FetchValue("SELECT COUNT(*) FROM StdtSessionHdr WHERE [IOASessionHdrId]='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "'")) > 0)
            //{
            //    int IOASessHdr = Convert.ToInt32(oData.FetchValue("SELECT StdtSessionHdrId FROM StdtSessionHdr WHERE [IOASessionHdrId]='" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()) + "'"));
            //    string InsertIOAQry = "INSERT INTO [dbo].[StdtSessionStep](StdtSessionHdrId,DSTempStepId,TrialNbr,SessionStatusCd,CreatedBy,CreatedOn) VALUES ('" + IOASessHdr + "',0,(SELECT (NbrOfTrials-1) FROM DSTempHdr WHERE DSTempHdrId='" + oTemp.TemplateId + "'),'NAV','" + oSession.LoginId + "',GETDATE())";
            //    int stdtsessStpId= oData.ExecuteWithScope(InsertIOAQry);

            //    if (dtcolumns != null)
            //    {
            //        foreach (DataRow drColmn in dtcolumns.Rows)
            //        {
            //            string insertSessionSetDtlQuery = "INSERT INTO StdtSessionDtl(StdtSessionStepId,DSTempSetColId,StepVal,CurrentPrompt,RowNumber,CreatedBy,CreatedOn)"
            //                                                + " VALUES(" + stdtsessStpId + "," + drColmn["DSTempSetColId"].ToString() + ",'" + drColmn["ColValue"].ToString() + "'," + crntPrmpt + ",(SELECT (NbrOfTrials) FROM DSTempHdr WHERE DSTempHdrId='" + oTemp.TemplateId + "')," + oSession.LoginId + ",GETDATE())";
            //            int dtlId = oData.ExecuteWithScope(insertSessionSetDtlQuery);
            //        }
            //    }
            //}
            if (stdtsessstepId > 0 && dtlId > 0)
            {
                oData.CommitTransation(trans, con);
            }
            else
            {
                oData.RollBackTransation(trans, con);
            }
            con.Close();
            string isMaintStatus = Convert.ToString(oData.FetchValue("SELECT DSMode FROM DSTempHdr WHERE DSTempHdrId='" + oTemp.TemplateId + "'"));
            isMaintStatus = (isMaintStatus == "MAINTENANCE" ? "true" : "false");
            if (lblSubmitAndRepeatCount.Text != "")
            {
                repeatNo = Convert.ToInt32(lblSubmitAndRepeatCount.Text);
            }

            //remya
            object IOA_Status = oData.FetchValue("SELECT IOAInd FROM StdtSessionHdr WHERE StdtSessionHdrId=" + Convert.ToInt32(ViewState["StdtSessHdr"].ToString()));
            Session["ISAddTrial"] = "";
            Response.Redirect("DatasheetPreview.aspx?pageid=" + oTemp.TemplateId + "&studid=" + oSession.StudentId + "&exc=true&isMaint=" + isMaintStatus + "&AddTrial='" + true + "'&IOA_Status='" + IOA_Status + "&SessionHdr=" + ViewState["StdtSessHdr"].ToString() + "");


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string[] Shuffle(string[] Objects)
    {
        Random rand = new Random();

        for (int i = 0; i < Objects.Length; i++)
        {
            int rVal = rand.Next(Objects.Length - 1);

            string temp = Objects[i];
            Objects[i] = Objects[rVal];
            Objects[rVal] = temp;

        }

        return Objects;
    }
    protected void FillSetStepDetails(int templateId, string ioaStat, string multiTeacher)
    {

        oData = new clsData();
        string strQuery = "";
        string sortNum = "";
        string ioaMultiStat = "";
        oDisc = new DiscreteSession();
        if (oDS.ChainType == "Backward chain")
        {
            strQuery = "Select (Select SortOrder from DSTempSet where DSTempSetId=NextSetId) as SortOrder,(Select SetCd from DSTempSet where DSTempSetId=NextSetId) as SETName,(SELECT distinct(StepCd) FROM(select StepCd as StepCd,SortOrder,RANK() OVER(ORDER BY SortOrder DESC) Rnk from DSTempStep where DSTempHdrId=" + templateId + " AND ActiveInd='A' AND  DSTempSetId=StdtDSStat.NextSetId) A WHERE Rnk=StdtDSStat.NextStepId) as STEP,(select LookupName from LookUp where LookupId=NextPromptId) as PROMPT,statusMessage as Status FROM StdtDSStat Where  DSTempHdrId= " + templateId + "";
        }
        else
        {
            strQuery = "Select (Select SortOrder from DSTempSet where DSTempSetId=NextSetId) as SortOrder,(Select SetCd from DSTempSet where DSTempSetId=NextSetId) as SETName,(Select distinct(StepCd) from DSTempStep where SortOrder=StdtDSStat.NextStepId and DSTempSetId=StdtDSStat.NextSetId and DSTempHdrId=" + templateId + ") as STEP,(select LookupName from LookUp where LookupId=NextPromptId) as PROMPT,statusMessage as Status FROM StdtDSStat Where  DSTempHdrId= " + templateId + "";
        }
        DataTable Dt = oData.ReturnDataTable(strQuery, false);

        if (Dt != null && Dt.Rows.Count > 0)
        {
            strQuery = "select (Select sortorder from DSTempSet where DSTempSetId=NextSetId) FROM StdtDSStat Where   DSTempHdrId= " + templateId + " ";
            sortNum = oData.FetchValue(strQuery).ToString();

            Dt.Columns.Add("IOA/MT Status", typeof(string));
            strQuery = "select IsMT_IOA from DSTempHdr where  DSTempHdrId= " + templateId + "";
            ioaMultiStat = oData.FetchValue(strQuery).ToString();

            if (Convert.ToInt16(ioaMultiStat) == 1)
            {
                Dt.Rows[0]["IOA/MT Status"] = "IOA to Advance";
            }
            else if (Convert.ToInt16(ioaMultiStat) == 2)
            {
                Dt.Rows[0]["IOA/MT Status"] = "MT to Advance";
            }
            else if (Convert.ToInt16(ioaMultiStat) == 3)
            {
                Dt.Rows[0]["IOA/MT Status"] = "IOA & MT to Advance";
            }
            else
                Dt.Rows[0]["IOA/MT Status"] = "---";

            //if (ioaStat == "IOA to Advance")
            //{
            //    if (multiTeacher == "MT to Advance")
            //    {
            //        Dt.Rows[0]["IOA/MT Status"] = "IOA & MT to Advance";
            //    }
            //    else
            //        Dt.Rows[0]["IOA/MT Status"] = ioaStat;
            //}
            //else if (multiTeacher == "MT to Advance")
            //{
            //    Dt.Rows[0]["IOA/MT Status"] = multiTeacher;
            //}
            //else
            //    Dt.Rows[0]["IOA/MT Status"] = "---";
            if (Dt.Rows[0]["Status"].ToString() == "COMPLETED")
            {
                Dt.Rows[0]["Status"] = "Mastered";
                Dt.Rows[0]["PROMPT"] = "Mastered";
                Dt.Rows[0]["STEP"] = "Mastered";

            }
            else
                Dt.Rows[0]["Status"] = "Current";
        }

        strQuery = "select SortOrder,SetCd as SETName from DSTempSet where DSTempHdrId=" + templateId + " and sortorder<" + Convert.ToInt32(sortNum) + "";
        DataTable dtOtherSetBelow = oData.ReturnDataTable(strQuery, false);
        if (dtOtherSetBelow != null)
        {
            if (dtOtherSetBelow.Rows.Count > 0)
            {

                dtOtherSetBelow.Columns.Add("STEP", typeof(string));
                dtOtherSetBelow.Columns.Add("PROMPT", typeof(string));
                dtOtherSetBelow.Columns.Add("Status", typeof(string));
                dtOtherSetBelow.Columns.Add("IOA/MT Status", typeof(string));
                foreach (DataRow item in dtOtherSetBelow.Rows)
                {
                    if (Dt.Rows[0]["STEP"] != System.DBNull.Value)
                        item["STEP"] = "Mastered";
                    if (Dt.Rows[0]["PROMPT"] != System.DBNull.Value)
                        item["PROMPT"] = "Mastered";

                    item["Status"] = "Mastered";
                    item["IOA/MT Status"] = "---";
                    Dt.ImportRow(item);
                }
            }
        }
        strQuery = "select SortOrder,SetCd as SETName from DSTempSet where DSTempHdrId=" + templateId + " and sortorder>" + Convert.ToInt32(sortNum) + "";
        DataTable dtOtherSetAbove = oData.ReturnDataTable(strQuery, false);
        if (dtOtherSetAbove != null)
        {
            if (dtOtherSetAbove.Rows.Count > 0)
            {

                dtOtherSetAbove.Columns.Add("STEP", typeof(string));
                dtOtherSetAbove.Columns.Add("PROMPT", typeof(string));
                dtOtherSetAbove.Columns.Add("Status", typeof(string));
                dtOtherSetAbove.Columns.Add("IOA/MT Status", typeof(string));
                foreach (DataRow item in dtOtherSetAbove.Rows)
                {
                    if (Dt.Rows[0]["STEP"] != System.DBNull.Value)
                        item["STEP"] = "Not Started";
                    if (Dt.Rows[0]["PROMPT"] != System.DBNull.Value)
                        item["PROMPT"] = "Not Started";
                    item["Status"] = "Not Started";
                    item["IOA/MT Status"] = "---";
                    Dt.ImportRow(item);
                }
            }
        }

        DataView dataview = Dt.DefaultView;
        dataview.Sort = "SortOrder";
        Dt = dataview.ToTable();
        Dt.Columns.RemoveAt(0);
        Dt.Columns[0].ColumnName = "SET";
        grdSetDetails.DataSource = Dt;
        grdSetDetails.DataBind();
    }

    //Code added for Distractor Functinality [28-oct-2021] Start--
    private string[] getDistractors(int templateid,int setid)
    {
        int templateidcopy = templateid;
        string[] discopy = new string[0];
        if (templateid > 0)
        {
            object getDisSam = oData.FetchValue("SELECT DistractorSamples FROM DSTempSet where dstemphdrid = " + templateidcopy + " AND DSTempSetId = " + setid);
            object distractordt = oData.FetchValue("SELECT DistractorSamplesCount FROM DSTempSet where dstemphdrid = " + templateidcopy + " AND DSTempSetId = " + setid);
            if ((getDisSam.ToString() != "") && (distractordt.ToString() != "")) //if (getDisSam != null && distractordt != null)
            {
                int disCount = Convert.ToInt32(distractordt);
                string disSam = getDisSam.ToString();
                disSam = disSam.TrimEnd(',');
                discopy = new string[disCount];
                discopy = disSam.Split(',');
            }
        }
        return discopy;
    }

    protected void continue_btn_Click(object sender, EventArgs e)
    {
        hdnMissTrialRsn.Value = hdnChkdRsn.Value;
        mistrialRsn.Text = hdnMissTrialRsn.Value;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "UncheckAll", "UncheckAll();", true);
    }

    private string getNormalSample(string[] disSamplearry, string disSamples, string steptempstring)
    {
        try
        {
        string matchDistractors = disSamples;
        string[] distractorSamplesArry = disSamplearry;
        string tempString = steptempstring;
        if (matchDistractors != null)
        {
            string tempStringcopy = steptempstring;
            string firstpart = tempStringcopy.Substring(0, tempStringcopy.IndexOf(":"));
                firstpart = firstpart.Trim();
            string lastpart = tempStringcopy.Substring(tempStringcopy.IndexOf(":") + 1);
            tempStringcopy = tempStringcopy.Substring(tempStringcopy.IndexOf("[") + 1);
            tempStringcopy = tempStringcopy.Substring(0, tempStringcopy.IndexOf("]"));

            string matchSelctd = tempStringcopy;
            string[] nondistractors = new string[0];
                string[] nondistractorsExceptional = new string[0];
            string output = "";

            if (matchSelctd != null)
            {
                List<string> diff;
                IEnumerable<string> set1 = matchSelctd.Split(',').Distinct();
                IEnumerable<string> set2 = matchDistractors.Split(',').Distinct();

                if (set2.Count() > set1.Count())
                {
                    diff = set2.Except(set1).ToList();
                }
                else
                {
                    diff = set1.Except(set2).ToList();
                }
                if (diff != null)
                {
                    output = String.Join(",", diff);
                    nondistractors = output.Split(',');
                        nondistractorsExceptional = output.Split(',');
                }
            }

                string[] CopyQuestnAary = QuestnAary.Where(c => c != null).ToArray();
                int SampleLimitCount = 0;
                    int AryLen = QuestnAary.Length;
                int DisLen = nondistractorsExceptional.Length;
                if ((AryLen > 0) && (DisLen > 1))
                {
                    if (AryLen >= (DisLen * 2))
                    {
                        decimal SampleLimitCount2 = Convert.ToDecimal(AryLen) / Convert.ToDecimal(DisLen);
                        SampleLimitCount = Convert.ToInt32(Math.Round(SampleLimitCount2));
                    }
                    else if (AryLen == DisLen)
                    {
                        decimal SampleLimitCount2 = Convert.ToDecimal(AryLen) / Convert.ToDecimal(DisLen);
                        SampleLimitCount = Convert.ToInt32(Math.Round(SampleLimitCount2));
                    }
                    else if (AryLen > DisLen)
                    {
                        SampleLimitCount = AryLen;
                    }
                    else if (DisLen > AryLen)
                    {
                        SampleLimitCount = DisLen;
                    }
                }
                else
                {
                    SampleLimitCount = AryLen;
                }

                var list = new List<string>(nondistractors);
                int tescnt = 0;
                for (int i = 0; i < nondistractors.Length; i++)
                {
                    string[] test = QuestnAary.Where(c => c != null && c == nondistractors[i]).ToArray();
                    int test2 = test.Length;
                    tescnt = test2;

                    if (tescnt >= SampleLimitCount && SampleLimitCount != 0)
                    {
                        if (!samcnt.ContainsKey(nondistractors[i]))
                        {
                            samcnt.Add(nondistractors[i], nondistractors[i]);
                        }
                        if (samcnt.ContainsKey(nondistractors[i]))
                        {
                            list.Remove(nondistractors[i]);
                        }
                    }
                }
                nondistractors = list.ToArray();

                int disindex = 0;
                string disindexString = "";
                string disStringnew = "";
                Random disrand = new Random();
                if (nondistractorsExceptional.Length <= 1 && nondistractors.Length <= 1)
                {
                    if (distractorSamplesArry.Contains(firstpart))
                    {
                        disindex = disrand.Next(nondistractorsExceptional.Length);
                        disindexString = nondistractorsExceptional[disindex].ToString();
                        disStringnew = disindexString + ": " + lastpart;
                        tempString = disStringnew;
                    }
                    else
                    {
                        tempString = firstpart + ": " + lastpart;
                    }
                }
                else if (nondistractorsExceptional.Length > 1 && nondistractors.Length > 0)
                {
                    if (distractorSamplesArry.Contains(firstpart))
                    {
                        int LoopInc = 0;
                        do
                        {
                            LoopInc++;
                            disindex = disrand.Next(nondistractors.Length);
                            disindexString = nondistractors[disindex].ToString();
                            if (preSampleString != disindexString)
                            {
                                break;
                            }
                            if ((LoopInc > 5) && (nondistractors.Length <= 1))
                            {
                                for (int i = 0; i <= QuestnAary.Length; i++)
                                {
                                    if (QuestnAary[i] != disindexString)
                                    {
                                        string swapString = QuestnAary[i];
                                        QuestnAary[i] = disindexString;
                                        disindexString = swapString;
                                        break;
                                    }
                                }
                            }
                        } while ((preSampleString == disindexString));
                        disStringnew = disindexString + ": " + lastpart;
                        tempString = disStringnew;
                        preSampleString = disindexString;
                    }
                    else
                    {
                        if (preSampleString == firstpart)
                        {
                            int LoopInc = 0;
                            do
                            {
                                LoopInc++;
                                disindex = disrand.Next(nondistractors.Length);
                                disindexString = nondistractors[disindex].ToString();
                                if (preSampleString != disindexString)
                                {
                                    break;
                                }
                                if ((LoopInc > 5) && (nondistractors.Length <= 1))
                                {
                                    for (int i = 0; i <= QuestnAary.Length; i++)
                                    {
                                        if (QuestnAary[i] != disindexString)
                                        {
                                            string swapString = QuestnAary[i];
                                            QuestnAary[i] = disindexString;
                                            disindexString = swapString;
                                            break;
                                        }
                                    }
                                }
                            } while (preSampleString == disindexString);
                            disStringnew = disindexString + ": " + lastpart;
                            tempString = disStringnew;
                            preSampleString = disindexString;
                        }
                        else
                        {
                            if (preSampleString != firstpart && !samcnt.ContainsKey(firstpart))
                            {
                disStringnew = firstpart + ": " + lastpart;
                tempString = disStringnew;
                                preSampleString = firstpart;
            }
                            else
                            {
                                int LoopInc = 0;
                                do
                                {
                                    LoopInc++;
                                    disindex = disrand.Next(nondistractors.Length);
                                    disindexString = nondistractors[disindex].ToString();
                                    if (preSampleString != disindexString)
                                    {
                                        break;
        }
                                    if ((LoopInc > 5) && (nondistractors.Length <= 1))
                                    {
                                        for (int i = 0; i <= QuestnAary.Length; i++)
                                        {
                                            if (QuestnAary[i] != disindexString)
                                            {
                                                string swapString = QuestnAary[i];
                                                QuestnAary[i] = disindexString;
                                                disindexString = swapString;
                                                break;
                                            }
                                        }
                                    }
                                } while (preSampleString == disindexString);
                                disStringnew = disindexString + ": " + lastpart;
                                tempString = disStringnew;
                                preSampleString = disindexString;
                            }
                        }
                    }
                }
                else
                {
                    if (distractorSamplesArry.Contains(firstpart))
                    {                        
                        do
                        {                            
                            disindex = disrand.Next(nondistractorsExceptional.Length);
                            disindexString = nondistractorsExceptional[disindex].ToString();
                            if (preSampleString != disindexString)
                            {
                                break;
                            }
                        } while (preSampleString == disindexString);
                        disStringnew = disindexString + ": " + lastpart;
                        tempString = disStringnew;
                        preSampleString = disindexString;
                    }
                    else
                    {
                        if (preSampleString == firstpart)
                        {                            
                            do
                            {                                
                                disindex = disrand.Next(nondistractorsExceptional.Length);
                                disindexString = nondistractorsExceptional[disindex].ToString();
                                if (preSampleString != disindexString)
                                {
                                    break;
                                }
                            } while (preSampleString == disindexString);
                            disStringnew = disindexString + ": " + lastpart;
                            tempString = disStringnew;
                            preSampleString = disindexString;
                        }
                        else
                        {
                    tempString = firstpart + ": " + lastpart;
                }

            }
                }
            }
        return tempString;
    }
        catch (Exception ex)
        {
            ClsErrorLog clError = new ClsErrorLog();
            clError.WriteToLog(ex.ToString());
            throw ex;
        }
    }
    //Code added for Distractor Functinality [28-oct-2021] End--
}

