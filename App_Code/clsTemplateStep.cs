using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for clsTemplate
/// </summary>
public class clsTemplateStep
{
    clsData objData = null;
    string strQuery = "";

    public clsTemplateStep()
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
    //public void fillUserCheckBox(CheckBoxList chkUser, int ClassId, int SchoolId)
    //{
    //    objData = new clsData();
    //    strQuery = "Select UserId from dbo.UserClass";
    //    strQuery += " WHERE  ActiveInd = 'A' AND classId=" + ClassId + " AND  SchoolId = " + SchoolId + "";


    //    DataTable Dt = objData.ReturnDataTable(strQuery, false);


    //    if (Dt != null && Dt.Rows.Count > 0)
    //    {
    //        string[] s = new string[Dt.Rows.Count];
    //        for (int h = 0; h < Dt.Rows.Count; h++)
    //        {
    //            s[h] = Dt.Rows[h]["UserId"].ToString();

    //        }
    //        int length = s.Length;
    //        for (int i = 0; i <= s.Length - 1; i++)
    //        {

    //            for (int j = 0; j <= chkUser.Items.Count - 1; j++)
    //            {
    //                if (chkUser.Items[j].Value == s[i])
    //                {
    //                    chkUser.Items[j].Selected = true;
    //                    break;
    //                }
    //            }
    //        }

    //    }

    //}
    //public void FillPrompUsed(int TemplateId, CheckBoxList chk)
    //{
    //    objData = new clsData();
    //    string dVal = "";

    //    DataTable Dt = objData.ReturnDataTable("Select PromptId from dbo.DSTempPrompt where DSTempHdrId=" + TemplateId + " And ActiveInd='A'", false);
    //    int PId = 0;
    //    int PrId = 0;

    //    if (Dt != null && Dt.Rows.Count > 0)
    //    {
    //        string[] s = new string[Dt.Rows.Count];
    //        for (int h = 0; h < Dt.Rows.Count; h++)
    //        {
    //            s[h] = Dt.Rows[h]["PromptId"].ToString();

    //        }
    //        int length = s.Length;
    //        for (int i = 0; i <= s.Length - 1; i++)
    //        {

    //            for (int j = 0; j <= chk.Items.Count - 1; j++)
    //            {
    //                PromptOrderSplit(chk.Items[j].Value, out PId, out PrId);
    //                if (PId.ToString() == s[i])
    //                {
    //                    chk.Items[j].Selected = true;
    //                    break;
    //                }
    //            }
    //        }

    //    }
    //}

    public void deleteStepPage2(int Id)
    {
        objData = new clsData();
        strQuery = "Update DSTempStep Set ActiveInd='D' where DSTempStepId=" + Id + "  ";
        objData.Execute(strQuery);
    }

    public void saveStepPage2(string StepName, string StepDesc, int StepId, int DSTempHdrId, int SchoolId, int SortOrder, int CreatedBy, int ModifiedBy)
    {
        objData = new clsData();

        strQuery = "Insert Into DSTempStep(SchoolId,DSTempHdrId,StepCd,StepName,DSTempSetId,SortOrder,ActiveInd,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn) ";
        strQuery += " Values(" + SchoolId + "," + DSTempHdrId + ",'" + StepName + "','" + StepDesc + "'," + StepId + "," + SortOrder + ",'A'," + CreatedBy + ",getdate()," + ModifiedBy + " ,getdate()) ";

        objData.Execute(strQuery);
    }
    public void updateStepPage2(int Id, string StepName, string StepDesc, int StepId, int DSTempHdrId, int SchoolId, int SortOrder, int ModifiedBy)
    {
        objData = new clsData();
        strQuery = "Update DSTempStep Set SchoolId=" + SchoolId + ",DSTempHdrId=" + DSTempHdrId + ",StepCd='" + StepName + "',StepName='" + StepDesc + "',DSTempSetId=" + StepId + ",SortOrder=" + SortOrder + ",ActiveInd='A',ModifiedBy=" + ModifiedBy + ",ModifiedOn=getdate() where DSTempStepId=" + Id + "  ";
        objData.Execute(strQuery);
    }
    public void expandData(out string Html, out string Count, out string UpdateValues, int TemplateId, out string SetId,int SchoolId)
    {
        Html = "";
        Count = "";
        SetId = "";
        UpdateValues = "";
        objData = new clsData();
        strQuery = "Select DSTempStepId,StepCd,StepName,DSTempSetId from DSTempStep Where DSTempHdrId=" + TemplateId + " And  SchoolId=" + SchoolId + " And ActiveInd='A' order by SortOrder ";
        DataTable Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt != null)
        {
            Count = Dt.Rows.Count.ToString();            
        }

        int num = 1;

        if (Dt == null) return;

        foreach (DataRow Dr in Dt.Rows)
        {
            num = num + 1;
            SetId = Dr["DSTempSetId"].ToString();
            string txt1 = "txtName" + num;
            string txt2 = "txtDesc" + num;
            string txt4 = "hidUpdate" + num;

            UpdateValues += Dr["DSTempStepId"].ToString() + ",";

            string divId = "Div" + num;

            string html = "";

            html = " <div class='contentDiv'  id=" + divId + ">  <table><tr><td width=\"50%\">Step Name</td><td><input type='text' size='30' id=" + txt1 + "  name=" + txt1 + "  value='" + Dr["StepCd"].ToString() + "'>&nbsp; &nbsp; <a href=\"#\" target=\"_self\" onClick=\"addTextBox()\">Add</a>&nbsp; &nbsp; &nbsp;  "
              + "<a href=\"#\" target=\"_self\" onClick=\"removeFromCode(" + num + ")\">Remove</a></td></tr><tr><td width=\"50%\">Step Description <input name=" + txt4 + "  type='hidden' value=" + Dr["DSTempStepId"].ToString() + " /> </td><td><textarea   name=" + txt2 + "  cols=\"50\" rows=\"5\">" + Dr["StepName"].ToString() + "</textarea></td></tr>"
              + "</table></div>";

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