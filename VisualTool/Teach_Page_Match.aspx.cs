using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Phase002_1_Teach_Page_Match : System.Web.UI.Page
{
    clsSession sess = null;
    clsData oData = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        sess = (clsSession)Session["UserSession"];
        if (sess == null)
        {
            Response.Redirect("Error.aspx?Error=Your session has expired. Please log-in again");
        }
        if (!IsPostBack)
        {
            oData = new clsData();
            if (sess != null)
            {
                lblLoginName.Text = sess.UserName;
            }
            setTitle();
            GetLe_DetailId();
        }

       
        int studId = sess.StudentId;


        ReinforcementRandomizer reinRndmzr = new ReinforcementRandomizer();

        //string result = reinRndmzr.getRandomReinforcemnt(studId, "correct");

        corrReinTemp.Value = reinRndmzr.getRandomReinforcemnt(studId, "correct");
        wrongReinTemp.Value = reinRndmzr.getRandomReinforcemnt(studId, "wrong");
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
                TitleName.Text = obj.ToString();


            }
        }
    }

    public int GetLe_DetailId()
    {
        svc_setStep.SetStepClient sp = new svc_setStep.SetStepClient();
        svc_setStep.setStep_request request = new svc_setStep.setStep_request();

        request.setValue = Convert.ToInt32(Session["setValue"]);
        request.stepValue = Convert.ToInt32(Session["stepValue"]);
        request.lessonId = Convert.ToInt32(Session["VTLessonId"]);

        svc_setStep.setStep_response response = sp.getLeDetId(request);
        Session["Le_DetailID"] = (int)response.responseInt;
        return response.responseInt;

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getLeOptId()
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        // request.le_detailsId = Convert.ToInt32(HttpContext.Current.Session["lessonDetId"].ToString());

        request.le_detailsId = (int)HttpContext.Current.Session["Le_DetailID"];

        svc_matchingLesson.ml_response response = ml.getLeOptIds(request);

        return response.responseString2;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getTempData(string leOptId)
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        string resultString = "";

        request.le_optionIdList = leOptId;

        svc_matchingLesson.ml_response response = ml.getDistAll(request);
        svc_matchingLesson.ml_request[] list = response.responseList;

        for (int i = 0; i < list.Length; i++)
        {
            resultString += list.ElementAt(i).objects + "^" + list.ElementAt(i).status + "^" + list.ElementAt(i).le_optionId + "☺";
        }

        return resultString;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static int GetStepNumbrs()
    {

        svc_timeEditor.ItimeEditorClient tp = new svc_timeEditor.ItimeEditorClient();
        svc_timeEditor.classRequestTime request = new svc_timeEditor.classRequestTime();

        //request.lessonId = Convert.ToInt32(HttpContext.Current.Session["VTLessonId"].ToString());

        request.setNumber = Convert.ToInt32(HttpContext.Current.Session["setValue"]);
        request.stepNumber = Convert.ToInt32(HttpContext.Current.Session["stepValue"]);
        svc_timeEditor.ResponseResultTime response = tp.GetStepNumbrs(request);
        int returnData = response.outInt;
        HttpContext.Current.Session["stepValue"] = returnData;
        return returnData;

    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string[] GetCorrectAns()
    {
        clsSession sess = (clsSession)HttpContext.Current.Session["UserSession"];
        int studId = sess.StudentId;
        svc_reinforcementAssg.IreinforcementAssgClient ra = new svc_reinforcementAssg.IreinforcementAssgClient();
        svc_reinforcementAssg.reinfRequest rRequest = new svc_reinforcementAssg.reinfRequest();
        svc_reinforcementAssg.reinfRespond rResponse = new svc_reinforcementAssg.reinfRespond();

        rRequest.studentId = studId;
        svc_reinforcementAssg.reinfRespond response = ra.getReinforcement(rRequest);
        svc_reinforcementAssg.reinforcement[] list = response.reinforcementList;

        List<string> lists = new List<string>();
        //lists.Add(list[0].CorrectAns);
        //lists.Add(list[0].WrongAns);
        if (list != null)
        {
            if (list.Length > 0)
            {
                string[] corrctAns = list[0].CorrectAns.Split(',');
                string[] wrongAns = list[0].WrongAns.Split(',');

                Random random = new Random();
                int indexA = random.Next(corrctAns.Length);
                string randomCorrAns = corrctAns[indexA];

                int indexB = random.Next(wrongAns.Length);
                string randomWrngAns = wrongAns[indexB];

                string[] paths = new string[2];
                rRequest.mediaId = Convert.ToInt32(randomCorrAns);
                response = ra.getMedia(rRequest);
                paths[0] = response.outputString;

                rRequest.mediaId = Convert.ToInt32(randomWrngAns);
                response = ra.getMedia(rRequest);
                paths[1] = response.outputString;

                return paths;
            }
            else
                return null;
        }
        else
            return null;
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static void SaveData(string val1, string val2)
    {
        clsSession sess = (clsSession)HttpContext.Current.Session["UserSession"];
        clsData oData = new clsData();
        int vtStepID = Convert.ToInt32(HttpContext.Current.Session["stepValue"]);
        int stepID = 0;
        object objStep = oData.FetchValue("SELECT DSTempStepId FROM DSTempStep WHERE VTStepId=" + vtStepID);
        if (objStep != null)
        {
            stepID = Convert.ToInt32(objStep);
        }
        DataTable dtStep = new DataTable();
        //dtStep = oData.ReturnDataTable("SELECT StdtSessionDtlId,DSTempSetColId,StepVal FROM StdtSessionDtl Dtl INNER JOIN StdtSessionStep Step " +
        //                                "ON Dtl.StdtSessionStepId=Step.StdtSessionStepId " +
        //                                "WHERE Step.DSTempStepId=" + stepID, false);

        dtStep = oData.ReturnDataTable("SELECT StdtSessionDtlId,DSTempSetColId,StepVal " +
                                        "FROM StdtSessionDtl Dtl " +
                                        "INNER JOIN (StdtSessionStep Step " +
                                        "INNER JOIN StdtSessionHdr Hdr " +
                                        "ON Hdr.StdtSessionHdrId=Step.StdtSessionHdrId) " +
                                        "ON Dtl.StdtSessionStepId=Step.StdtSessionStepId " +
                                        "WHERE Step.DSTempStepId=" + stepID + " AND Hdr.SessionStatusCd='D' AND IOAInd='N' AND Hdr.StudentId=" + sess.StudentId + " AND Hdr.SchoolId=" + sess.SchoolId, false);
        foreach (DataRow drstep in dtStep.Rows)
        {
            object objType = oData.FetchValue("SELECT ColTypeCd FROM DSTempSetCol WHERE DSTempSetColId=" + drstep["DSTempSetColId"].ToString());
            string colType = "";
            if (objType != null)
                colType = objType.ToString();

            if (colType == "Duration")
            {
                double temp = Math.Round(Convert.ToDouble(val1));
                string min = (Convert.ToInt32(temp) / 60).ToString();

                string sec = (Convert.ToInt32(temp) - (Convert.ToInt32(min) * 60)).ToString();

                if (sec.Length == 1)
                    sec = "0" + sec;

                if (min.Length == 1)
                    min = "0" + min;

                string m_val1 = "00:" + min + ":" + sec;

                //val1 = Convert.ToDateTime(val1).Hour.ToString() + ":" + Convert.ToDateTime(val1).Minute.ToString() + ":" + Convert.ToDateTime(val1).Second.ToString();
                oData.Execute("UPDATE StdtSessionDtl SET StepVal='" + m_val1 + "' WHERE StdtSessionDtlId=" + drstep["StdtSessionDtlId"].ToString());
            }
            if (colType == "+/-")
            {
                oData.Execute("UPDATE StdtSessionDtl SET StepVal='" + val2 + "' WHERE StdtSessionDtlId=" + drstep["StdtSessionDtlId"].ToString());
            }
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getNoOfTrials()
    {
        clsSession sess = (clsSession)HttpContext.Current.Session["UserSession"];
        clsData oData = new clsData();
        int vtStepID = Convert.ToInt32(HttpContext.Current.Session["stepValue"]);
        string stepID = "0";
        object objStep = oData.FetchValue("SELECT NbrOfTrials FROM DSTempHdr WHERE DSTempHdrId= (SELECT DSTempHdrId FROM DSTempStep WHERE VTStepId=" + vtStepID + ")");
        if (objStep != null)
        {
            stepID = objStep.ToString();
        }

        return stepID;
    }

    protected void lnk_logout_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Logout.aspx");
    }

}