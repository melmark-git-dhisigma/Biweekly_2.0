using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class matchingLessonPreview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ReinforcementRandomizer reinRndmzr = new ReinforcementRandomizer();

       // string result = reinRndmzr.getRandomReinforcemnt(2029, "correct");

       // corrReinTemp.Value = reinRndmzr.getRandomReinforcemnt(2029, "correct");
       // wrongReinTemp.Value = reinRndmzr.getRandomReinforcemnt(2029, "wrong"); ;
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string getLeOptId()
    {
        svc_matchingLesson.ImatchingLessonClient ml = new svc_matchingLesson.ImatchingLessonClient();
        svc_matchingLesson.ml_request request = new svc_matchingLesson.ml_request();

        request.le_detailsId = Convert.ToInt32(HttpContext.Current.Session["lessonDetId"].ToString());

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

}