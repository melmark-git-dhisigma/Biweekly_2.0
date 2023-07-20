using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ReinforcementRandomizer
/// </summary>
public class ReinforcementRandomizer
{
    clsData oData = null;
    
   
	public ReinforcementRandomizer()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public reinformentProp getStudentReinforcement(int studentId)
    {
        DataTable dt = new DataTable();
        oData = new clsData();
        reinformentProp reinfProp = new reinformentProp();

        string qry = "select CorrectAns,WrongAns from LE_ReinforcementAssgn where StudentId =" + studentId;

        dt = oData.ReturnDataTable(qry, false);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                string correct = dt.Rows[0]["CorrectAns"].ToString();
                reinfProp.correctAnsStr = Convert.ToString(dt.Rows[0]["CorrectAns"]);
                reinfProp.wrongAnsStr =Convert.ToString( dt.Rows[0]["WrongAns"]);
            }
        }

        return reinfProp;
    }

    public string getRandomReinforcemnt(int studentId, string type)
    {
        string result = "default-" + type;


        reinformentProp reinProp = getStudentReinforcement(studentId);
        Random rnd = new Random();
        int randNo = 0;
        if (reinProp.correctAnsStr != null && reinProp.wrongAnsStr != null)
        {
            switch (type)
            {
                case "correct":
                    string[] correctAnsSplit = reinProp.correctAnsStr.Split(',');
                    randNo = rnd.Next(0, correctAnsSplit.Count() - 1);

                    if(correctAnsSplit[randNo] != ""){

                        result = correctAnsSplit[randNo];
                    };
                    break;
                case "wrong":
                    string[] wrongAnsSplit = reinProp.wrongAnsStr.Split(',');
                    randNo = rnd.Next(0, wrongAnsSplit.Count() - 1);

                    if (wrongAnsSplit[randNo] != "")
                    {

                        result = wrongAnsSplit[randNo];
                    }
                    break;

                default: break;
            }
        }
        return result;
    }
}

/// <summary>
/// Class for Reinforcement parameters
/// </summary>
public class reinformentProp
{
    public string correctAnsStr { get; set; }
    public string wrongAnsStr { get; set; }

}