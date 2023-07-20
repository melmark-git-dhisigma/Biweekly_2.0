using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsTemplate
/// </summary>
public class clsTemplate
{
    clsData objData = null;
    string strQuery = "";

    public clsTemplate()
    {

    }

    public bool PromptExit(int DSTempHdrId, int PromptId)
    {
        objData = new clsData();
        bool val = objData.IFExists("Select DSTempPromptId from DSTempPrompt where DSTempHdrId=" + DSTempHdrId + " and PromptId=" + PromptId + " ");
        return val;
    }
    public string GetADPrompt(int DSTempHdrId, int PromptId)
    {
        objData = new clsData();
        string val = objData.FetchValue("Select ActiveInd from DSTempPrompt where DSTempHdrId=" + DSTempHdrId + " and PromptId=" + PromptId + " ").ToString();
        return val;
    }

    public static void PromptOrderSplit(string Val, out int PId, out int PrId)
    {
        string[] words = Val.Split(',');
        PId = Convert.ToInt32(words[0]);
        PrId = Convert.ToInt32(words[1]);
    }
    public void fillUserCheckBox(CheckBoxList chkUser, int ClassId, int SchoolId)
    {
        objData = new clsData();
        strQuery = "Select UserId from dbo.UserClass";
        strQuery += " WHERE  ActiveInd = 'A' AND classId=" + ClassId + " AND  SchoolId = " + SchoolId + "";


        DataTable Dt = objData.ReturnDataTable(strQuery, false);


        if (Dt != null && Dt.Rows.Count > 0)
        {
            string[] s = new string[Dt.Rows.Count];
            for (int h = 0; h < Dt.Rows.Count; h++)
            {
                s[h] = Dt.Rows[h]["UserId"].ToString();

            }
            int length = s.Length;
            for (int i = 0; i <= s.Length - 1; i++)
            {

                for (int j = 0; j <= chkUser.Items.Count - 1; j++)
                {
                    if (chkUser.Items[j].Value == s[i])
                    {
                        chkUser.Items[j].Selected = true;
                        break;
                    }
                }
            }

        }

    }
    public void FillPrompUsed(int TemplateId, CheckBoxList chk)
    {
        objData = new clsData();
        string dVal = "";

        DataTable Dt = objData.ReturnDataTable("Select PromptId from dbo.DSTempPrompt where DSTempHdrId=" + TemplateId + " And ActiveInd='A'", false);
        int PId = 0;
        int PrId = 0;

        if (Dt != null && Dt.Rows.Count > 0)
        {
            string[] s = new string[Dt.Rows.Count];
            for (int h = 0; h < Dt.Rows.Count; h++)
            {
                s[h] = Dt.Rows[h]["PromptId"].ToString();

            }
            int length = s.Length;
            for (int i = 0; i <= s.Length - 1; i++)
            {

                for (int j = 0; j <= chk.Items.Count - 1; j++)
                {
                    PromptOrderSplit(chk.Items[j].Value, out PId, out PrId);
                    if (PId.ToString() == s[i])
                    {
                        chk.Items[j].Selected = true;
                        break;
                    }
                }
            }

        }
    }

    public void deleteSetPage2(int Id)
    {
        objData = new clsData();
        strQuery = "Update DSTempSet Set ActiveInd='D' where DSTempSetId=" + Id + "  ";
        objData.Execute(strQuery);
    }

    public void saveSetPage2(string SetName, string SetDesc, string SetMatch, int DSTempHdrId, int SchoolId, int SortOrder, int CreatedBy, int ModifiedBy, out int SetId)
    {
        objData = new clsData();

        strQuery = "Insert Into DSTempSet(SchoolId,DSTempHdrId,SetCd,SetName,Samples,SortOrder,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
        strQuery += " Values(" + SchoolId + "," + DSTempHdrId + ",'" + SetName + "','" + SetDesc + "','" + SetMatch + "'," + SortOrder + ",'A'," + CreatedBy + ",getdate()," + ModifiedBy + " ,getdate()) ";

        SetId = objData.ExecuteWithScope(strQuery);
    }
    public void updateSetPage2(int Id, string SetName, string SetDesc, string SetMatch, int DSTempHdrId, int SchoolId, int SortOrder, int ModifiedBy)
    {
        objData = new clsData();
        strQuery = "Update DSTempSet Set SchoolId=" + SchoolId + ",DSTempHdrId=" + DSTempHdrId + ",SetCd='" + SetName + "',SetName='" + SetDesc + "',Samples='" + SetMatch + "',SortOrder=" + SortOrder + ",ActiveInd='A',ModifiedBy=" + ModifiedBy + ",ModifiedOn=getdate() where DSTempSetId=" + Id + "  ";
        objData.Execute(strQuery);
    }
    public void expandData(out string Html, out string Count, out string UpdateValues, int TemplateId, string Match)
    {
        Html = "";
        Count = "";
        UpdateValues = "";
        objData = new clsData();
        strQuery = "Select DSTempSetId,SetCd,SetName,Samples from dbo.DSTempSet Where DSTempHdrId=" + TemplateId + " And ActiveInd='A' order by SortOrder ";
        DataTable Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt != null)
        {
            Count = Dt.Rows.Count.ToString();
        }
        int matchId = 0;
        int num = 1;

        if (Dt == null) return;

        foreach (DataRow Dr in Dt.Rows)
        {
            num = num + 1;

            string txt1 = "txtName" + num;
            string txt2 = "txtDesc" + num;
            string txt3 = "txtSample" + num;
            string txt4 = "hidUpdate" + num;
            string txt5 = "txtMatch" + num;

            string match = "";
            string mName = "";
            string matchVal = "";
            UpdateValues += Dr["DSTempSetId"].ToString() + ",";
            string[] arr = Dr["Samples"].ToString().Split(',');
            for (int k = 1; k < arr.Length; k++)
            {
                if (arr[k] != "")
                {
                    matchId++;
                    matchVal = matchVal + arr[k] + ",";
                    mName = "test" + matchId;
                    match += "<div class=\'matchToSampleText\' id='" + mName + "'>" + arr[k] + "<a id=\'close\' onClick=\'removeMatchText(" + matchId + ")\' href=\'#\'><img src='../Administration/images/popUpClose.ico' width=10 height=\'10\' border=\'0\'></a></div>";
                }
            }

            string divId = "Div" + num;

            string html = "";
            if (Match == "1")
            {
                html = " <div class='contentDiv'  id=" + divId + ">  <table><tr><td width=\"50%\">Set Name</td><td><input type='text' size='30' id=" + txt1 + "  name=" + txt1 + "  value='" + Dr["SetCd"].ToString() + "'>&nbsp; &nbsp; <a href=\"#\" target=\"_self\" onClick=\"addTextBox()\">Add</a>&nbsp; &nbsp; &nbsp;  "
                     + "<a href=\"#\" target=\"_self\" onClick=\"removeFromCode(" + num + ")\">Remove</a></td></tr><tr><td width=\"50%\">Set Description </td><td><textarea   name=" + txt2 + "  cols=\"50\" rows=\"5\">" + Dr["SetName"].ToString() + "</textarea></td></tr>"
                     + "<tr><td  colspan=\"2\"><div id=Match" + num + " style=\"width:100%\">" + match + "</div></td></tr>"
                     + "<tr><td width=\"50%\">Match to Sample <input name=" + txt4 + "  type='hidden' value=" + Dr["DSTempSetId"].ToString() + " /> </td><td><input type=\"text\" size='30' ID=" + txt3 + "   name=" + txt3 + " >&nbsp; &nbsp; <a href=\"#\" target=\"_self\" onClick=\"getMatchToSample(" + num + ")\">Add</a><input type=\"text\" style=\"visibility:hidden\" ID='" + txt5 + "' value='" + matchVal + "'  name='" + txt5 + "' ></td></tr>"
                     + "</table></div>";
            }
            else
            {
                html = " <div class='contentDiv'  id=" + divId + ">  <table><tr><td width=\"50%\">Set Name</td><td><input type='text' size='30'  id=" + txt1 + "  name=" + txt1 + "  value='" + Dr["SetCd"].ToString() + "'>&nbsp; &nbsp; <a href=\"#\" target=\"_self\" onClick=\"addTextBox()\">Add</a>&nbsp; &nbsp; &nbsp;  "
                  + "<a href=\"#\" target=\"_self\" onClick=\"removeFromCode(" + num + ")\">Remove</a></td></tr><tr><td width=\"50%\">Set Description <input name=" + txt4 + "  type='hidden' value=" + Dr["DSTempSetId"].ToString() + " /> </td><td><textarea   name=" + txt2 + "  cols=\"50\" rows=\"5\">" + Dr["SetName"].ToString() + "</textarea></td></tr>"
                  + "</table></div>";
            }



            Html = Html + html;
        }




    }


    public static bool IsExit(string[] ar, string Val)
    {
        bool flag = false;
        for (int i = 0; i < ar.Length; i++)
        {
            if (ar[i] == Val)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }



}