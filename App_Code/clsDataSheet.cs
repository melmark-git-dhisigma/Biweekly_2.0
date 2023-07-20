using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public class clsDataSheet : ISerializable
{
    public clsDataSheet()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Session Datas
    protected clsDataSheet(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");



        try
        {


            skillType = info.GetString("skillType");
            teachProc = info.GetString("teachProc");
            chainTyp = info.GetString("chainTyp");
            numOFtrials = info.GetInt32("numOFtrials");
            crntSet = info.GetInt32("crntSet");
            crntStep = info.GetInt32("crntStep");
            crntSetNbr = info.GetInt32("crntSetNbr");
            crntPrompt = info.GetInt32("crntPrompt");
            setName = info.GetString("setName");
            stepName = info.GetString("stepName");
            promptName = info.GetString("promptName");
            statusMsg = info.GetString("statusMsg");
            lessonPlan = info.GetString("lessonPlan");
            lessonID = info.GetInt32("lessonID");
            materials = info.GetString("materials");
            promptProc = info.GetString("promptProc");
            totalTaskFormat = info.GetString("totalTaskFormat");
            matchToSampleType = info.GetString("matchToSampleType");


            sessNbr = info.GetInt32("sessNbr");
            dt_Steps = (DataTable)info.GetValue("dt_Steps", typeof(object));
            dt_Columns = (DataTable)info.GetValue("dt_Columns", typeof(object));
            misTrial = info.GetBoolean("misTrial");
            avg_Duration = info.GetInt32("avg_Duration");



            ioaSessHdr = info.GetInt32("ioaSessHdr");
            IOA_Ind = info.GetString("IOA_Ind");
            isVT = info.GetInt32("isVT");
            VT_LessnId = info.GetInt32("VT_LessnId");
        }
        catch (Exception ex)
        {

        }
    }
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {

        info.AddValue("skillType", skillType);
        info.AddValue("teachProc", teachProc);
        info.AddValue("chainTyp", chainTyp);
        info.AddValue("numOFtrials", numOFtrials);
        info.AddValue("crntSet", crntSet);
        info.AddValue("crntStep", crntStep);
        info.AddValue("crntSetNbr", crntSetNbr);
        info.AddValue("crntPrompt", crntPrompt);

        info.AddValue("setName", setName);
        info.AddValue("stepName", stepName);
        info.AddValue("promptName", promptName);
        info.AddValue("statusMsg", statusMsg);

        info.AddValue("lessonPlan", lessonPlan);
        info.AddValue("lessonID", lessonID);
        info.AddValue("materials", materials);
        info.AddValue("promptProc", promptProc);
        info.AddValue("totalTaskFormat", totalTaskFormat);
        info.AddValue("matchToSampleType", matchToSampleType);

        info.AddValue("sessNbr", sessNbr);
        info.AddValue("dt_Steps", dt_Steps);
        info.AddValue("dt_Columns", dt_Columns);
        info.AddValue("misTrial", misTrial);
        info.AddValue("avg_Duration", avg_Duration);


        info.AddValue("ioaSessHdr", ioaSessHdr);
        info.AddValue("IOA_Ind", IOA_Ind);
        info.AddValue("isVT", isVT);
        info.AddValue("VT_LessnId", VT_LessnId);




    }
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            throw new ArgumentNullException("info");

        GetObjectData(info, context);
    }
    private string skillType = "";
    public string SkillType
    {
        get { return skillType; }
        set { skillType = value; }
    }
    private string teachProc = "";
    public string TeachProc
    {
        get { return teachProc; }
        set { teachProc = value; }
    }
    private string chainTyp = "";
    public string ChainType
    {
        get { return chainTyp; }
        set { chainTyp = value; }
    }
    private int numOFtrials;
    public int NbrOfTrials
    {
        get { return numOFtrials; }
        set { numOFtrials = value; }
    }


    private int crntSet;
    public int CrntSet
    {
        get { return crntSet; }
        set { crntSet = value; }
    }



    private int crntStep;
    public int CrntStep
    {
        get { return crntStep; }
        set { crntStep = value; }
    }



    private int crntSetNbr;
    public int CrntSetNbr
    {
        get { return crntSetNbr; }
        set { crntSetNbr = value; }
    }



    private int crntPrompt;
    public int CrntPrompt
    {
        get { return crntPrompt; }
        set { crntPrompt = value; }
    }


    private string setName = "";
    public string SetName
    {
        get { return setName; }
        set { setName = value; }
    }



    private string stepName = "";
    public string StepName
    {
        get { return stepName; }
        set { stepName = value; }
    }



    private string promptName = "";
    public string PromptName
    {
        get { return promptName; }
        set { promptName = value; }
    }



    private string statusMsg = "";
    public string StatusMsg
    {
        get { return statusMsg; }
        set { statusMsg = value; }
    }
    private string lessonPlan;
    public string LessonPlan
    {
        get { return lessonPlan; }
        set { lessonPlan = value; }
    }

    private int lessonID;
    public int LessonPlanID
    {
        get { return lessonID; }
        set { lessonID = value; }
    }
    private string materials = "";
    public string Materials
    {
        get { return materials; }
        set { materials = value; }
    }
    private string promptProc = "";
    public string PromptProc
    {
        get { return promptProc; }
        set { promptProc = value; }
    }
    private string totalTaskFormat = "";
    public string TotalTaskFormat
    {
        get { return totalTaskFormat; }
        set { totalTaskFormat = value; }
    }
    private string matchToSampleType = "";
    public string MatchToSampleType
    {
        get { return matchToSampleType; }
        set { matchToSampleType = value; }
    }





    private int sessNbr;
    public int SessNbr
    {
        get { return sessNbr; }
        set { sessNbr = value; }
    }






    private DataTable dt_Steps = null;
    public DataTable dtSteps
    {
        get { return dt_Steps; }
        set { dt_Steps = value; }
    }





    private DataTable dt_Columns = null;
    public DataTable dtColumns
    {
        get { return dt_Columns; }
        set { dt_Columns = value; }
    }

    private bool misTrial = false;
    public bool MisTrail
    {
        get { return misTrial; }
        set { misTrial = value; }
    }
    private bool sessMistrial = false;
    public bool SessionMistrial
    {
        get { return sessMistrial; }
        set { sessMistrial = value; }
    }

    public string sessionMistrialRsn;
    public string SessionMistrialRsn
    {
        get { return sessionMistrialRsn; }
        set { sessionMistrialRsn = value; }
    }

    private int avg_Duration = 0;
    public int avgDurationId
    {
        get { return avg_Duration; }
        set { avg_Duration = value; }
    }
    private int ioaSessHdr = 0;
    public int IOASessHdr
    {
        get { return ioaSessHdr; }
        set { ioaSessHdr = value; }
    }



    private string IOA_Ind = "N";
    public string IOAInd
    {
        get { return IOA_Ind; }
        set { IOA_Ind = value; }
    }



    private int isVT = 0;
    public int ISVTool
    {
        get { return isVT; }
        set { isVT = value; }
    }
    private int VT_LessnId = 0;
    public int VTLessonId
    {
        get { return VT_LessnId; }
        set { VT_LessnId = value; }
    }
    protected void generateGrid()
    {

    }
}
public class Rules
{
    public int count = 0;

    public int moveup = 0;
    public int movedown = 0;

    public bool NASetMoveup = false;
    public bool NASetMoveDown = false;
    public bool NAStepMoveup = false;
    public bool NAStepMoveDown = false;
    public bool NAPromptMoveup = false;
    public bool NAPromptMoveDown = false;

    public RuleDetails pctAccyMoveUp = new RuleDetails();
    public RuleDetails pctAccyMoveDown = new RuleDetails();
    public RuleDetails pctAccyMod = new RuleDetails();

    public RuleDetails pctIndMoveUp = new RuleDetails();
    public RuleDetails pctIndMoveDown = new RuleDetails();
    public RuleDetails pctIndMod = new RuleDetails();

    public RuleDetails pctIndAllMoveUp = new RuleDetails();
    public RuleDetails pctIndAllMoveDown = new RuleDetails();
    public RuleDetails pctIndAllMod = new RuleDetails();

    public RuleDetails pctPrmtMoveUp = new RuleDetails();
    public RuleDetails pctPrmtMoveDown = new RuleDetails();
    public RuleDetails pctPrmtMod = new RuleDetails();

    public RuleDetails pctCustomMoveUp = new RuleDetails();
    public RuleDetails pctCustomMoveDown = new RuleDetails();
    public RuleDetails pctCustomMod = new RuleDetails();

    public RuleDetails pctAvgDurationMoveUp = new RuleDetails();
    public RuleDetails pctAvgDurationMoveDown = new RuleDetails();
    public RuleDetails pctAvgDurationMod = new RuleDetails();

    public RuleDetails pctTotalDurationMoveUp = new RuleDetails();
    public RuleDetails pctTotalDurationMoveDown = new RuleDetails();
    public RuleDetails pctTotalDurationMod = new RuleDetails();

    public RuleDetails pctFrequencyMoveUp = new RuleDetails();
    public RuleDetails pctFrequencyMoveDown = new RuleDetails();
    public RuleDetails pctFrequencyMod = new RuleDetails();

    public RuleDetails pctlearnedStepMoveUp = new RuleDetails();
    public RuleDetails pctlearnedStepMoveDown = new RuleDetails();
    public RuleDetails pctlearnedStepMod = new RuleDetails();

    public RuleDetails learnedStepMoveUp = new RuleDetails();
    public RuleDetails learnedStepMoveDown = new RuleDetails();

    public RuleDetails PromptlearnedStepMoveUp = new RuleDetails();
    public RuleDetails PromptlearnedStepMoveDown = new RuleDetails();

    public RuleDetails excludeCrntStepMoveUp = new RuleDetails();
    public RuleDetails excludeCrntStepMoveDown = new RuleDetails();

    public RuleDetails PromptExcludeCrntStepMoveUp = new RuleDetails();
    public RuleDetails PromptExcludeCrntStepMoveDown = new RuleDetails();

    public RuleDetails Set_ExcludeCrntStepMoveUp = new RuleDetails();
    public RuleDetails Set_ExcludeCrntStepMoveDown = new RuleDetails();

    public RuleDetails SetlearnedStepMoveUp = new RuleDetails();
    public RuleDetails SetlearnedStepMoveDown = new RuleDetails();

    public RuleDetails pctTotalCorrectMoveUp = new RuleDetails();
    public RuleDetails pctTotalCorrectMoveDown = new RuleDetails();
    public RuleDetails pctTotalCorrectMod = new RuleDetails();

    public RuleDetails pctTotalIncorrectMoveUp = new RuleDetails();
    public RuleDetails pctTotalIncorrectMoveDown = new RuleDetails();
    public RuleDetails pctTotalIncorrectMod = new RuleDetails();
    //public bool bMultiTeacherRequired = false;
    // public bool bIOARequird = false;
    public bool bMultiSet = false;
    public bool bIncludeMisTrail = false;

}
public class RuleDetails
{
    public string sRuleType = "";
    public string sCriteriaType = "";
    public int iScoreRequired = 0;
    public int iTotalInstance = 0;
    public int iTotalCorrectInstance = 0;
    public bool bConsequetiveIndex = false;
    public bool bMultiTeacherRequired = false;
    public bool bIOARequird = false;
    public bool bConsecutiveAvg = false; //--- [New Criteria] May 2020 ---//
    public int iConsecutiveAvgval = 0; //--- [New Criteria] May 2020 ---//


}
public class Prompt
{
    public string promptName = "";
    public int promptId = 0;
}

    #endregion

public class DynamicallyTemplatedGridViewHandler : ITemplate
{
    #region data memebers

    ListItemType ItemType;
    string FieldName;
    string ColName;
    string InfoType;
    string CntrolID;
    string ColType;
    int count;
    bool cntrlEnable = true;
    bool bFreeText = false;

    #endregion

    #region constructor

    public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string col_name, string info_type, string cntrl_id, string col_type, bool cntrl_Enabled, int item_count, bool FreeText = false)
    {
        ItemType = item_type;
        FieldName = field_name;
        ColName = col_name;
        InfoType = info_type;
        CntrolID = cntrl_id;
        ColType = col_type;
        count = item_count;
        cntrlEnable = cntrl_Enabled;
        bFreeText = FreeText;
    }

    #endregion

    #region Methods
    int i = 2;
    public void InstantiateIn(System.Web.UI.Control Container)
    {
        switch (ItemType)
        {
            case ListItemType.Header:
                Literal header_ltrl = new Literal();
                header_ltrl.Text = "<b id='" + CntrolID + "'>" + ColName + "</b><span style='display:none;'>" + ColType + "</span>";
                Container.Controls.Add(header_ltrl);
                break;
            case ListItemType.Item:
                switch (InfoType)
                {
                    case "Step / Sample / Sd":
                        HiddenField hfs = new HiddenField();
                        hfs.ID = "hfSessStepID";
                        hfs.DataBinding += new EventHandler(hf_DataBinding);
                        Container.Controls.Add(hfs);


                        HiddenField hfstp = new HiddenField();
                        hfstp.ID = "hfStepID";
                        hfstp.DataBinding += new EventHandler(hfstp_DataBinding);
                        Container.Controls.Add(hfstp);

                        Label lbl = new Label();
                        lbl.ID = "lblStepname";
                        lbl.DataBinding += new EventHandler(lbl_DataBinding);
                        Container.Controls.Add(lbl);

                        HiddenField hfsSamples = new HiddenField();
                        hfsSamples.ID = "hfSampleID";
                        hfsSamples.Value = "";
                        Container.Controls.Add(hfsSamples);

                        break;
                    case "Mistrial":
                        CheckBox chk = new CheckBox();
                        chk.ID = "chkMistrial";
                        chk.Visible = true;
                        chk.Enabled = cntrlEnable;
                        chk.Width = Unit.Percentage(50);
                        chk.Attributes.Add("onchange", "temp();");
                        Container.Controls.Add(chk);
                        break;
                    case "Notes":
                        TextBox txt = new TextBox();
                        txt.ID = "txtStepNotes";
                        txt.TextMode = TextBoxMode.MultiLine;
                        txt.Width = Unit.Pixel(130);
                        txt.Enabled = cntrlEnable;
                        txt.Style.Add("margin", "0 3px 0 3px");
                        Container.Controls.Add(txt);
                        break;
                    case "Radio":
                        HtmlInputRadioButton rdb = new HtmlInputRadioButton();
                        rdb.ID = "rdbRespPlus_" + CntrolID;
                        rdb.Disabled = !(cntrlEnable);
                        rdb.Style.Add("width", "15px");
                        rdb.Attributes.Add("onclick", "rdouncheck(this);");
                        rdb.Attributes.Add("onchange", "temp();");
                        rdb.Attributes.Add("value", "+");
                        rdb.Attributes.Add("name", CntrolID.ToString());
                        Container.Controls.Add(rdb);
                        HtmlGenericControl gc = new HtmlGenericControl("label");
                        string cnt = i.ToString();
                        if (i.ToString().Length == 1) { cnt = "0" + i.ToString(); }
                        gc.Attributes.Add("for", "grdDataSht_ctl" + cnt + "_" + rdb.ID);
                        gc.Attributes.Add("style", "border-radius:10px 0px 0px 10px;");
                        gc.InnerHtml = "+";
                        Container.Controls.Add(gc);
                        HtmlInputRadioButton rdbminus = new HtmlInputRadioButton();
                        rdbminus.ID = "rdbRespMinus_" + CntrolID;
                        rdbminus.Disabled = !(cntrlEnable);
                        rdbminus.Style.Add("width", "15px");
                        rdbminus.Attributes.Add("onclick", "rdouncheck(this);");
                        rdbminus.Attributes.Add("onchange", "temp();");
                        rdbminus.Attributes.Add("value", "-");
                        rdbminus.Attributes.Add("name", CntrolID.ToString());
                        Container.Controls.Add(rdbminus);
                        HtmlGenericControl gc2 = new HtmlGenericControl("label");
                        gc2.Attributes.Add("for", "grdDataSht_ctl" + cnt + "_" + rdbminus.ID);
                        gc2.Attributes.Add("style", "border-radius:0px 10px 10px 0px;");
                        gc2.InnerHtml = "&nbsp;-";
                        Container.Controls.Add(gc2);
                        i++;
                        break;
                    case "DropDown":
                        DropDownList dr = new DropDownList();
                        dr.Attributes.Add("onchange", "temp();");
                        dr.ID = "ddlPrompt_" + CntrolID;
                        dr.Enabled = true;
                        dr.CssClass = "drpClass";
                        dr.Width = Unit.Percentage(95);
                        Container.Controls.Add(dr);
                        break;
                    case "Freq":
                        TextBox txtFrq = new TextBox();
                        txtFrq.ID = "txtFrequency_" + CntrolID;
                        txtFrq.CssClass = "clsFreq";
                        txtFrq.Enabled = cntrlEnable;
                        txtFrq.Attributes.Add("onkeypress", "return isNumber(event)");
                        txtFrq.Attributes.Add("onchange", "temp();");
                        txtFrq.Attributes.Add("onpaste", "return false");
                        Container.Controls.Add(txtFrq);
                        break;
                    case "Text":
                        UpdatePanel uPanel = new UpdatePanel();
                        uPanel.ID = "UpdatePanel_" + CntrolID;
                        uPanel.ChildrenAsTriggers = true;
                        uPanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                        TextBox txtText = new TextBox();
                        txtText.ID = "txtText_" + CntrolID;
                        txtText.CssClass = "clsText";
                        txtText.Enabled = cntrlEnable;
                        txtText.Attributes.Add("onchange", "onchangeTxt(" + CntrolID + ");temp();");
                        if (!bFreeText)
                        {
                            txtText.Attributes.Add("onkeypress", "return isNumber(event)");
                        }

                        //
                        txtText.Attributes.Add("onpaste", "return false");
                        txtText.AutoPostBack = true;
                        uPanel.ContentTemplateContainer.Controls.Add(txtText);
                        Container.Controls.Add(uPanel);
                        break;
                    case "Timer":
                        HtmlInputText txtd = new HtmlInputText();
                        txtd.Attributes.Add("title", "Click to Reset");
                        txtd.Attributes.Add("class", "clsDuratn");
                        //txtd.Attributes.Add("style", "text-align: center; cursor: pointer; width: 100%; padding: 0; margin-right: 2px;");
                        txtd.Attributes.Add("style", "text-align: center; cursor: pointer; width: 100%; padding: 0; margin-right: 2px;");
                        txtd.Attributes.Add("onclick", "resetTime(this);");
                        txtd.Attributes.Add("readonly", "true");
                        txtd.Disabled = !(cntrlEnable);
                        txtd.ID = "txtDuratn_" + CntrolID;
                        txtd.Value = "00:00:00";
                        Container.Controls.Add(txtd);
                        HtmlInputButton btn = new HtmlInputButton();
                        btn.Value = "Start";
                        btn.Disabled = !(cntrlEnable);
                        btn.ID = "btnDuration_" + CntrolID;
						
                        //btn.Attributes.Add("style", "width: 33%; border-radius: 5px 5px 5px 5px; cursor: pointer; height: 29px;");
                         //btn.Attributes.Add("class", "NFButtonWithNoImage");
                        // btn.Attributes.Add("class", "NFButtonNew");
                        // btn.Attributes.Add("style", "width: 50% !important;");
                        btn.Attributes.Add("style", "width: 50%; height: 26px;text-align: center;  border: none;border-radius: 5px 5px 5px 5px; cursor: pointer; height: 29px;" +
                            "color: #fff; font-family: Arial, Helvetica, sans-serif;font-size: 11px;font-weight: bold;text-decoration: none;"+
                            "-webkit-border-radius: 5px;-moz-border-radius: 5px;background-color: #03507D; border-radius: 5px;float: left;");
                        btn.Attributes.Add("onclick", "stopwatch(this);");
                        Container.Controls.Add(btn);
                        HiddenField hf = new HiddenField();
                        hf.ID = "hfDuration_" + CntrolID;
                        hf.Value = "00:00:00";
                        Container.Controls.Add(hf);

						 HtmlInputText txtTimerId = new HtmlInputText();
                        txtTimerId.Attributes.Add("title", "ID");
                        txtTimerId.Attributes.Add("class", "clsDuratn");
                        txtTimerId.Attributes.Add("style", "text-align: center; display:none;cursor: pointer; width: 88%; padding: 0; margin-right: 2px;");
                        txtTimerId.Attributes.Add("readonly", "true");
                        txtTimerId.Disabled = !(cntrlEnable);
                        txtTimerId.ID = "txtTimerId_" + CntrolID;
                        txtTimerId.Value = "";
                        Container.Controls.Add(txtTimerId);
						
						HtmlInputButton btnedit = new HtmlInputButton();
                        btnedit.Value = "Edit";
                        btnedit.Disabled = !(cntrlEnable);
                        btnedit.ID = "btnEditDuration_" + CntrolID;
                        btnedit.Attributes.Add("style", "width: 50%; height: 26px;text-align: center;  border: none;border-radius: 5px 5px 5px 5px; cursor: pointer; height: 29px;" +
                             "color: #fff; font-family: Arial, Helvetica, sans-serif;font-size: 11px;font-weight: bold;text-decoration: none;" +
                            "-webkit-border-radius: 5px;-moz-border-radius: 5px;background-color: #03507D; border-radius: 5px;"); //float: left; text-align: left;
                        //btnedit.Attributes.Add("class", "NFButtonWithNoImage");
                       // btnedit.Attributes.Add("class", "NFButtonNew");
                        //btnedit.Attributes.Add("style", "width: 50% !important;font-size: 10px;");
                        btnedit.Attributes.Add("onclick", "stopwatch(this);");
                        Container.Controls.Add(btnedit);         
						
                         HtmlInputText txtHr = new HtmlInputText();
                         txtHr.Attributes.Add("title", "Hour");
                         txtHr.Attributes.Add("class", "clsDuratn");
                         txtHr.Attributes.Add("style", "text-align: center;display:none; position:relative;width: 23%; padding: 3%;");
                         txtHr.Attributes.Add("onchange", "resetTime(this);"); //oninput onchange   onkeypress 
                         txtHr.Attributes.Add("editable", "true");
                         txtHr.Attributes.Add("visibility", "hidden");
                         txtHr.Disabled = !(cntrlEnable);
                         txtHr.ID = "txtHrDuratn_" + CntrolID;
                         txtHr.Attributes.Add("placeholder", "00"); 
                         txtHr.Attributes.Add("display", "none"); 
                         Container.Controls.Add(txtHr);

                        HtmlInputText txtMin = new HtmlInputText();
                        txtMin.Attributes.Add("title", "Minute");
                        txtMin.Attributes.Add("class", "clsDuratn");
                        txtMin.Attributes.Add("style", "text-align: center; display:none;  position:relative;width: 23%; padding: 3%;");
                        txtMin.Attributes.Add("onchange", "resetTime(this);");
                        txtMin.Attributes.Add("editable", "true");
                        txtMin.Disabled = !(cntrlEnable);
                        txtMin.ID = "txtMinDuratn_" + CntrolID;
                        txtMin.Attributes.Add("placeholder", "00");
                        Container.Controls.Add(txtMin);

                         HtmlInputText txtSec = new HtmlInputText();
                         txtSec.Attributes.Add("title", "Second");
                         txtSec.Attributes.Add("class", "clsDuratn");
                         txtSec.Attributes.Add("style", "text-align: center; display:none; position:relative; width: 23%; padding: 3%;");
                         txtSec.Attributes.Add("onchange", "resetTime(this);");
                         txtSec.Attributes.Add("editable", "true");
                         txtSec.Disabled = !(cntrlEnable);
                         txtSec.ID = "txtSecDuratn_" + CntrolID;
                         txtSec.Attributes.Add("placeholder", "00");                         
                         Container.Controls.Add(txtSec);


                        break;

                    case "Samples":

                        RadioButton rdb_Samples = new RadioButton();
                        rdb_Samples.Style.Add("font-size", "15px!important");
                        rdb_Samples.LabelAttributes.CssStyle.Add("font-size", "15px!important");
                        rdb_Samples.LabelAttributes.CssStyle.Add("display", "block");
                        rdb_Samples.LabelAttributes.CssStyle.Add("margin-right", "10px");
                        rdb_Samples.Attributes.Add("onclick", "rdouncheck2(this);");
                        rdb_Samples.ID = "rdbStepName_" + CntrolID + "_" + count;
                        rdb_Samples.GroupName = "rdbName";
                        rdb_Samples.DataBinding += new EventHandler(rdb_DataBinding);
                        Container.Controls.Add(rdb_Samples);




                        //Label lbl1 = new Label();
                        //lbl1.ID = "lblStepname"+count;
                        //lbl1.DataBinding += new EventHandler(lbl2_DataBinding);
                        //Container.Controls.Add(lbl1);
                        break;

                }
                break;

        }
    }



    #endregion

    #region Event Handlers

    //just sets the insert flag ON so that we ll be able to decide in OnRowUpdating event whether to insert or update
    protected void insert_button_Click(Object sender, EventArgs e)
    {
        new Page().Session["InsertFlag"] = 1;
    }
    //just sets the insert flag OFF so that we ll be able to decide in OnRowUpdating event whether to insert or update 
    protected void edit_button_Click(Object sender, EventArgs e)
    {
        new Page().Session["InsertFlag"] = 0;
    }

    void hf_DataBinding(object sender, EventArgs e)
    {
        HiddenField hdnf = (HiddenField)sender;
        GridViewRow container = (GridViewRow)hdnf.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, FieldName.Split(',')[0]);
        if (dataValue != DBNull.Value)
        {
            hdnf.Value = dataValue.ToString();
        }
    }


    void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow container = (GridViewRow)lbl.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, FieldName.Split(',')[1]);
        if (dataValue != DBNull.Value)
        {
            lbl.Text = dataValue.ToString();
        }
    }

    void hfstp_DataBinding(object sender, EventArgs e)
    {
        HiddenField hdnf = (HiddenField)sender;
        GridViewRow container = (GridViewRow)hdnf.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, FieldName.Split(',')[2]);
        if (dataValue != DBNull.Value)
        {
            hdnf.Value = dataValue.ToString();
        }
    }

    void lbl2_DataBinding(object sender, EventArgs e)
    {
        Label lbl = (Label)sender;
        GridViewRow container = (GridViewRow)lbl.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, FieldName);
        if (dataValue != DBNull.Value)
        {
            lbl.Text = dataValue.ToString();
        }
    }

    private void rdb_DataBinding(object sender, EventArgs e)
    {
        RadioButton rdb_Smpl = (RadioButton)sender;
        GridViewRow container = (GridViewRow)rdb_Smpl.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, FieldName);
        if (dataValue != DBNull.Value)
        {
            rdb_Smpl.Text = dataValue.ToString();
        }
    }

    #endregion


}
