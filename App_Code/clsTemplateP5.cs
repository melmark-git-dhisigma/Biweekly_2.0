using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
public class clsTemplateP5
{
    clsData objData = null;
    string strQuery = "";
    static string strGETColumns = "<option value=0>-----Select Column-----</option> ";
    static string strGETMeasure = "<option value=0>-----Select Measure-----</option> ";
    public clsTemplateP5()
    {


    }
    public void deleteSetPage4(int Id)
    {
        objData = new clsData();
        strQuery = "Update DSTempRule Set ActiveInd='D' where DSTempRuleId=" + Id + "  ";
        objData.Execute(strQuery);
    }
    private void Getresult(string val, string ColId)
    {
        objData = new clsData();
        String query = "Select DSTempSetColId as Id, ColName as Name from dbo.DSTempSetCol Where ActiveInd='A' And DSTempHdrId=" + val + "";
        DataTable dt = objData.ReturnDataTable(query, false);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                strGETColumns = "<option value=0>------Select Column------</option> ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (ColId == dt.Rows[i]["Id"].ToString())
                    {
                        strGETColumns += "<option selected='selected' value=" + dt.Rows[i]["Id"].ToString() + ">" + dt.Rows[i]["Name"] + "</option> ";
                    }
                    else
                    {
                        strGETColumns += "<option value=" + dt.Rows[i]["Id"].ToString() + ">" + dt.Rows[i]["Name"] + "</option> ";
                    }
                }
            }
        }
    }

    private void GetMeasure(string colVal, string Val)
    {
        strGETMeasure = "";
        objData = new clsData();
        String query = "Select DISTINCT DSTempSetColCalcId as Id,CalcType as Name from DSTempSetColCalc where DSTempSetColId=" + colVal + "";
        DataTable dt = objData.ReturnDataTable(query, false);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                strGETMeasure = "<option value=0>------Select Measure------</option> ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Val == dt.Rows[i]["Id"].ToString())
                    {
                        strGETMeasure += "<option selected='selected' value=" + dt.Rows[i]["Id"].ToString() + ">" + dt.Rows[i]["Name"] + "</option> ";
                    }
                    else
                    {
                        strGETMeasure += "<option value=" + dt.Rows[i]["Id"].ToString() + ">" + dt.Rows[i]["Name"] + "</option> ";
                    }
                }

            }
        }
    }

    public void expandData(out string Html, out string Count, out string UpdateValues, int TemplateId, int SchoolId, int Type)
    {
        Html = "";
        Count = "";
        UpdateValues = "";
        objData = new clsData();
        string RuleType = "";
        if (Type == 1)
        {
            RuleType = "SET";
        }
        else if (Type == 2)
        {
            RuleType = "STEP";
        }
        else if (Type == 3)
        {
            RuleType = "PROMPT";
        }


        strQuery = "Select DSR.DSTempRuleId,DSCol.DSTempHdrId,DSR.DSTempSetColId,DSR.DSTempSetColCalcId,DSR.RuleType,DSR.CriteriaType,DSR.ScoreReq,DSR.TotalInstance,DSR.TotCorrInstance,DSR.ConsequetiveInd,DSR.IOAReqInd,DSR.MultiTeacherReqInd from DSTempRule DSR "
                    + "Inner Join DSTempSetCol DSCol ON   DSR.DSTempSetColId=DSCol.DSTempSetColId Where DSR.ActiveInd='A' And DSR.SchoolId=" + SchoolId + " And DSCol.DSTempHdrId=" + TemplateId + " And DSR.RuleType='" + RuleType + "' Order By CriteriaType Desc";
        DataTable Dt = objData.ReturnDataTable(strQuery, false);




        if (Dt != null)
        {
            Count = Dt.Rows.Count.ToString();
        }

        string type = "";
        string html = "";
        int num = 0;
        string ColumnVal = "0";
        string ColumnId = "";
        string MeasureVal = "0";
        bool IsConInd = false;

        string strConInd1 = "";
        string strConInd2 = "";

        bool IsIOAInd = false;
        string strIOA1 = "";
        string strIOA2 = "";

        bool IsMultiInd = false;
        string strMulti1 = "";
        string strMulti2 = "";

        string divClass = "";

        string DIV1 = "";
        string DIV2 = "";
        string DIV3 = "";


        string readOnly1 = "";
        string readOnly2 = "";

        string totalInstance1 = "";
        string totalInstance2 = "";


        if (Dt == null) return;

        foreach (DataRow Dr in Dt.Rows)
        {
            type = Convert.ToString(Dr["CriteriaType"]);




            ColumnVal = Convert.ToString(Dr["DSTempSetColCalcId"]);

            IsConInd = Convert.ToBoolean(Dr["ConsequetiveInd"]);
            if (IsConInd == true)
            {
                strConInd1 = "checked='checked'";
                strConInd2 = "";
                readOnly2 = "readOnly='readOnly'";
                readOnly1 = "";
                totalInstance1 = Dr["TotalInstance"].ToString();
                totalInstance2 = "";
            }
            else
            {
                strConInd2 = "checked='checked'";
                strConInd1 = "";
                readOnly1 = "readOnly='readOnly'";
                readOnly2 = "";
                totalInstance2 = Dr["TotalInstance"].ToString();
                totalInstance1 = "";
            }





            IsIOAInd = Convert.ToBoolean(Dr["IOAReqInd"]);
            if (IsIOAInd == true)
            {
                strIOA1 = "checked='checked'";
                strIOA2 = "";
            }
            else
            {
                strIOA2 = "checked='checked'";
                strIOA1 = "";
            }

            IsMultiInd = Convert.ToBoolean(Dr["MultiTeacherReqInd"]);
            if (IsMultiInd == true)
            {
                strMulti1 = "checked='checked'";
                strMulti2 = "";
            }
            else
            {
                strMulti2 = "checked='checked'";
                strMulti1 = "";
            }



            if (type == "MOVE UP")
            {
                divClass = "<div class='contentDividerHead1' >TO MOVE UP</div>";
            }
            else if (type == "MOVE DOWN")
            {
                divClass = "<div class='contentDividerHead2' >TO MOVE DOWN</div>";
            }
            else if (type == "MODIFICATION")
            {
                divClass = "<div class='contentDividerHead3' >FOR MODIFICATION</div>";
            }

            num = num + 1;
            string txt4 = "hidUpdate" + num;
            UpdateValues += Dr["DSTempRuleId"].ToString() + ",";

            ColumnVal = Convert.ToString(Dr["DSTempHdrId"]);
            ColumnId = Convert.ToString(Dr["DSTempSetColId"]);
            MeasureVal = Convert.ToString(Dr["DSTempSetColCalcId"]);
            Getresult(ColumnVal, ColumnId);
            GetMeasure(ColumnId, MeasureVal);

            if (type == "MOVE UP")
            {
                html = "<div id='Div" + num + "' ><div class='contentDivider' > <table width='100%'  cellspacing='0' cellpadding='0'><tr><td colspan='2'>" + divClass + "</td><td width='15%'><input name=" + txt4 + "  type='hidden' value=" + Dr["DSTempRuleId"].ToString() + " /><input name='txtType" + num + "' Id='txtType" + num + "' type='text' value='1' runat='server' size='1' style='visibility:hidden;'/><input name='txtCol" + num + "' Id='txtCol" + num + "' type='text' size='1' style='visibility:hidden;' value='" + ColumnId + "'  > <input name='txtCal" + num + "' Id='txtCal" + num + "' type='text'  size='1' style='visibility:hidden;' value='" + MeasureVal + "' /></td> <td width='25%'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='right' width='3%'></td><td  width='3%' align='right'><a href=\"#\" target=\"_self\" onClick=\"addTextBoxU(1)\">Add</a> <a href=\"#\" target=\"_self\" onClick=\"decCount(" + num + ")\">Remove</a></td></tr></table></td> </tr><tr><td width='15%'>IOA Required</td><td width='25%'><input Id='rdoIOA' name='rdoIOA" + num + "' type='radio' value='1' " + strIOA1 + " />Yes<input Id='rdoIOA' name='rdoIOA" + num + "' type='radio' value='0' " + strIOA2 + " />No</td><td width='15%'>Multiteacher Required</td> <td width='25%'><input Id='rdoMTeacher' name='rdoMTeacher" + num + "' type='radio' value='1' " + strMulti1 + " />Yes<input Id='rdoMTeacher' name='rdoMTeacher" + num + "' type='radio' value='0' " + strMulti2 + " />No</td></tr><tr><td width='15%'>Template Column </td><td width='25%'><select Id='select' name='tempCol" + num + "' onchange='colChange(this," + num + ");' style='width: 250px'>" + strGETColumns + "</select></td><td width='15%'>Consecutive Session</td> <td width='25%'><input Id='rdoConsecutiveup' name='rdoConsecutiveup" + num + "' type='radio' value='1' " + strConInd1 + "  onClick='enableSession(1," + num + ")' />Yes<input Id='rdoConsecutiveup' name='rdoConsecutiveup" + num + "' type='radio' onClick='enableSession(2," + num + ")' value='0' " + strConInd2 + " />No</td></tr><tr><td>Measure</td><td><div Id='divMeasureSelect" + num + "'><select id='drpMes" + num + "' name='drpMes" + num + "' style='width: 250px' onchange=calChange(this," + num + ");'>" + strGETMeasure + "</select></div></td><td>Number Of Sessions</td><td><input name='txtNoSes" + num + "' " + readOnly1 + " type='text' size='10' onkeypress='return isNumberKey(event)' value=" + totalInstance1 + "  ></td></tr><tr><td>Required Score </td> "
      + "<td><input name='txtReqScore" + num + "' type='text' size='35' onkeypress='return isNumberKey(event)'  value=" + Dr["ScoreReq"].ToString() + "  /></td><td>Instance</td><td><table width='93%' cellpadding='0' cellspacing='0' ><tr><td><input name='txtOutFirst" + num + "' type='text' size='8' " + readOnly2 + " value=" + Dr["TotCorrInstance"].ToString() + " /></td><td>Out Of </td><td><input name='txtOutSecup" + num + "' type='text' size='8' onkeypress='return isNumberKey(event)'  " + readOnly2 + " value=" + totalInstance2 + "   ></td></tr></table></td></tr></table></div></div> ";

                DIV1 = DIV1 + html;
            }
            else if (type == "MOVE DOWN")
            {
                html = "<div id='Div" + num + "' ><div class='contentDivider' > <table width='100%'  cellspacing='0' cellpadding='0'><tr><td colspan='2'>" + divClass + "</td><td width='15%'><input name=" + txt4 + "  type='hidden' value=" + Dr["DSTempRuleId"].ToString() + " /><input name='txtType" + num + "' Id='txtType" + num + "' type='text' value='2' runat='server' size='1' style='visibility:hidden;'/><input name='txtCol" + num + "' Id='txtCol" + num + "' type='text' size='1' style='visibility:hidden;'  value='" + ColumnId + "' > <input name='txtCal" + num + "' Id='txtCal" + num + "' type='text'  size='1' style='visibility:hidden;' value='" + MeasureVal + "' /></td> <td width='25%'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='right' width='3%'></td><td  width='3%' align='right'><a href=\"#\" target=\"_self\" onClick=\"addTextBoxU(2)\">Add</a> <a href=\"#\" target=\"_self\" onClick=\"decCount(" + num + ")\">Remove</a></td></tr></table></td> </tr><tr><td width='15%'>IOA Required</td><td width='25%'><input Id='rdoIOA' name='rdoIOA" + num + "' type='radio' value='1' " + strIOA1 + " />Yes<input Id='rdoIOA' name='rdoIOA" + num + "' type='radio' value='0' " + strIOA2 + " />No</td><td width='15%'>Multiteacher Required</td> <td width='25%'><input Id='rdoMTeacher' name='rdoMTeacher" + num + "' type='radio' value='1' " + strMulti1 + " />Yes<input Id='rdoMTeacher' name='rdoMTeacher" + num + "' type='radio' value='0' " + strMulti2 + " />No</td></tr><tr><td width='15%'>Template Column </td><td width='25%'><select Id='select' name='tempCol" + num + "' onchange='colChange(this," + num + ");' style='width: 250px'>" + strGETColumns + "</select></td><td width='15%'>Consecutive Session</td> <td width='25%'><input Id='rdoConsecutiveup' name='rdoConsecutiveup" + num + "' type='radio' value='1' " + strConInd1 + "  onClick='enableSession(1," + num + ")' />Yes<input Id='rdoConsecutiveup' name='rdoConsecutiveup" + num + "' type='radio' onClick='enableSession(2," + num + ")' value='0' " + strConInd2 + " />No</td></tr><tr><td>Measure</td><td><div Id='divMeasureSelect" + num + "'><select id='drpMes" + num + "' name='drpMes" + num + "' style='width: 250px' onchange=calChange(this," + num + ");'>" + strGETMeasure + "</select></div></td><td>Number Of Sessions</td><td><input name='txtNoSes" + num + "' " + readOnly1 + " type='text' size='10' onkeypress='return isNumberKey(event)' value=" + totalInstance1 + "  ></td></tr><tr><td>Required Score </td> "
      + "<td><input name='txtReqScore" + num + "' type='text' size='35' onkeypress='return isNumberKey(event)'  value=" + Dr["ScoreReq"].ToString() + "  /></td><td>Instance</td><td><table width='93%' cellpadding='0' cellspacing='0' ><tr><td><input name='txtOutFirst" + num + "' type='text' size='8' " + readOnly2 + " value=" + Dr["TotCorrInstance"].ToString() + " /></td><td>Out Of </td><td><input name='txtOutSecup" + num + "' type='text' size='8' onkeypress='return isNumberKey(event)' " + readOnly2 + "  value=" + totalInstance2 + "   ></td></tr></table></td></tr></table></div></div> ";

                DIV2 = DIV2 + html;
            }
            else if (type == "MODIFICATION")
            {
                html = "<div id='Div" + num + "' ><div class='contentDivider' > <table width='100%'  cellspacing='0' cellpadding='0'><tr><td colspan='2'>" + divClass + "</td><td width='15%'><input name=" + txt4 + "  type='hidden' value=" + Dr["DSTempRuleId"].ToString() + " /><input name='txtType" + num + "' Id='txtType" + num + "' type='text' value='3' runat='server' size='1' style='visibility:hidden;'/><input name='txtCol" + num + "' Id='txtCol" + num + "' type='text'  size='1' style='visibility:hidden;' value='" + ColumnId + "'  > <input name='txtCal" + num + "' Id='txtCal" + num + "' type='text'  size='1' style='visibility:hidden;' value='" + MeasureVal + "' /></td> <td width='25%'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='right' width='3%'></td><td  width='3%' align='right'><a href=\"#\" target=\"_self\" onClick=\"addTextBoxU(3)\">Add</a> <a href=\"#\" target=\"_self\" onClick=\"decCount(" + num + ")\">Remove</a></td></tr></table></td> </tr><tr><td width='15%'>IOA Required</td><td width='25%'><input Id='rdoIOA' name='rdoIOA" + num + "' type='radio' value='1' " + strIOA1 + " />Yes<input Id='rdoIOA' name='rdoIOA" + num + "' type='radio' value='0' " + strIOA2 + " />No</td><td width='15%'>Multiteacher Required</td> <td width='25%'><input Id='rdoMTeacher' name='rdoMTeacher" + num + "' type='radio' value='1' " + strMulti1 + " />Yes<input Id='rdoMTeacher' name='rdoMTeacher" + num + "' type='radio' value='0' " + strMulti2 + " />No</td></tr><tr><td width='15%'>Template Column </td><td width='25%'><select Id='select' name='tempCol" + num + "' onchange='colChange(this," + num + ");' style='width: 250px'>" + strGETColumns + "</select></td><td width='15%'>Consecutive Session</td> <td width='25%'><input Id='rdoConsecutiveup' name='rdoConsecutiveup" + num + "' type='radio' value='1' " + strConInd1 + " onClick='enableSession(1," + num + ")' />Yes<input Id='rdoConsecutiveup' name='rdoConsecutiveup" + num + "' type='radio' onClick='enableSession(2," + num + ")' value='0' " + strConInd2 + " />No</td></tr><tr><td>Measure</td><td><div Id='divMeasureSelect" + num + "'><select id='drpMes" + num + "' name='drpMes" + num + "' style='width: 250px' onchange=calChange(this," + num + ");'>" + strGETMeasure + "</select></div></td><td>Number Of Sessions</td><td><input name='txtNoSes" + num + "' " + readOnly1 + " type='text' size='10' onkeypress='return isNumberKey(event)' value=" + totalInstance1 + "  ></td></tr><tr><td>Required Score </td> "
      + "<td><input name='txtReqScore" + num + "' type='text' size='35' onkeypress='return isNumberKey(event)'  value=" + Dr["ScoreReq"].ToString() + "  /></td><td>Instance</td><td><table width='93%' cellpadding='0' cellspacing='0' ><tr><td><input name='txtOutFirst" + num + "' type='text' size='8' " + readOnly2 + " value=" + Dr["TotCorrInstance"].ToString() + " /></td><td>Out Of </td><td><input name='txtOutSecup" + num + "' type='text' size='8' onkeypress='return isNumberKey(event)' " + readOnly2 + "  value=" + totalInstance2 + "   ></td></tr></table></td></tr></table></div></div> ";

                DIV3 = DIV3 + html;
            }


            strGETMeasure = "";
            strGETColumns = "";



        }
        DIV1 = "<div class='contentDiv' ID='newMoveUp' >" + DIV1 + "</div>";
        DIV2 = "<div class='contentDiv' ID='newMoveDown' >" + DIV2 + "</div>";
        DIV3 = "<div class='contentDiv' ID='newMoveMod' >" + DIV3 + "</div>";

        Html = DIV1 + DIV2 + DIV3;
    }
}