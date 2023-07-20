using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
public class clsTemplateP4
{
    clsData objData = null;
    string strQuery = "";
    static string strGETPrompt = "<option value=0>-----Select Prompt-----</option> ";
    public clsTemplateP4()
    {


    }
    public void deleteSetPage4(int Id)
    {
        objData = new clsData();
        strQuery = "Update DSTempSetCol Set ActiveInd='D' where DSTempSetColId=" + Id + "  ";
        objData.Execute(strQuery);

        strQuery = "Update DSTempSetColCalc Set ActiveInd='D' where DSTempSetColId=" + Id + "  ";
        objData.Execute(strQuery);
    }
    private void Getresult(string val, string PromptId)
    {
        objData = new clsData();

        String query = "SELECT LookUp.LookupId as Id, LookUp.LookupName as Name FROM LookUp INNER JOIN DSTempPrompt ON LookUp.LookupId = DSTempPrompt.PromptId WHERE       DSTempPrompt.DSTempHdrId = " + val + " and ActiveInd='A'";
        DataTable dt = objData.ReturnDataTable(query, false);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                strGETPrompt = "<option value=0>-----Select Prompt-----</option> ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Id"].ToString() == PromptId)
                    {
                        strGETPrompt += "<option value=" + dt.Rows[i]["Id"].ToString() + " selected='selected'>" + dt.Rows[i]["Name"] + "</option> ";
                    }
                    else
                    {
                        strGETPrompt += "<option value=" + dt.Rows[i]["Id"].ToString() + ">" + dt.Rows[i]["Name"] + "</option> ";
                    }
                }
            }
        }


    }
    public void expandData(out string Html, out string Count, out string UpdateValues, int TemplateId, int SchoolId)
    {
        Html = "";
        Count = "";
        UpdateValues = "";
        int ColumnId = 0;
        objData = new clsData();
        string Option = "";


        strQuery = "SELECT     DSTempSetColId,ColName, ColTypeCd, CorrResp, CorrRespDesc,  IncMisTrialInd, MisTrialDesc FROM DSTempSetCol  Where DSTempHdrId=" + TemplateId + " And  SchoolId = " + SchoolId + "  And ActiveInd='A' ";


        DataTable Dt = objData.ReturnDataTable(strQuery, false);

        if (Dt != null)
        {
            Count = Dt.Rows.Count.ToString();
        }

        string type = "";
        string html = "";
        string InnerHtml = "";
        int num = 1;
        string resType1 = "";
        string CalcType = "";
        bool InMisTrial = false;
        string InMisTrialChk = "";
        string AccInd = "";
        string strAcc = "";
        string PromptInd = "";
        string strPrompt = "";
        string IndeInd = "";
        string strInde = "";
        string NaInd = "";
        string strNa = "";
        string CustInd = "";
        string strCust = "";
        int corrRes = 0;
        string promptChecked = "";
        string promptChkVisible = "";
        if (Dt == null) return;

        foreach (DataRow Dr in Dt.Rows)
        {
            num = num + 1;
            string txt4 = "hidUpdate" + num;
            UpdateValues += Dr["DSTempSetColId"].ToString() + ",";
            ColumnId = Convert.ToInt32(Dr["DSTempSetColId"]);
            type = Convert.ToString(Dr["ColTypeCd"]);


            InMisTrial = Convert.ToBoolean(Dr["IncMisTrialInd"]);
            if (InMisTrial == true)
            {
                InMisTrialChk = "checked='checked'";
            }
            else
            {
                InMisTrialChk = "";
            }
            if (type == "+/-")
            {
                if (Dr["CorrResp"].ToString() == "+")
                {
                    resType1 = "<input type='radio' name='group2" + num + "' value='+'  checked='checked'> + <input type='radio' name='group2" + num + "' value='-'> -";
                }
                else
                {
                    resType1 = "<input type='radio' name='group2" + num + "' value='+'  > + <input type='radio' name='group2" + num + "' value='-' checked='checked'> -";

                }
            }
            else if (type == "Prompt")
            {
                if (Dr["CorrResp"].ToString() == "")
                {
                    promptChecked = "checked='checked'";
                }
                else
                {
                    promptChkVisible = "style='display:block'";






                }
            }

            strQuery = " Select CalcType,CalcRptLabel from DSTempSetColCalc Where DSTempSetColId=" + ColumnId + " And SchoolId=" + SchoolId + " And ActiveInd='A' ";
            DataTable DtCol = objData.ReturnDataTable(strQuery, false);

            if (type == "+/-")
            {

                if (DtCol != null)
                {
                    if (DtCol.Rows.Count > 0)
                    {
                        Option = "<option value='0' selected='selected'>+/-</option><option value='1' >Prompt</option><option value='2'>Text</option><option value='3' >Duration</option><option value='4'>Frequency</option>";
                        foreach (DataRow DrCol in DtCol.Rows)
                        {

                            CalcType = Convert.ToString(DrCol["CalcType"]);
                            if (CalcType == "% Accuracy")
                            {
                                strAcc = "checked='checked'";
                                AccInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else if (CalcType == "% Independant")
                            {
                                strInde = "checked='checked'";
                                IndeInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            //else if (CalcType == "% Prompted")
                            //{
                            //    strPrompt = "checked='checked'";
                            //    PromptInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            //}
                            else
                            {
                                strPrompt = "";
                                strAcc = "";
                                strInde = "";
                            }
                        }
                        /*<tr><td> <label> <input type='checkbox' name='chkPrompted" + num + "' value='% Prompted'   " + strPrompt + "  /> % Prompted</label></td>    <td></td><td colspan='3'><input type='text' name='txtPromptDesc" + num + "' id='' width='100%' value=" + PromptInd + " ></td>    <td></td>    <td></td> "
                         + " </tr> <tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>(Total Prompted Trials/Total Trials)*100</span></td> </tr>*/

                        InnerHtml = "<table width='100%'>    <tr>    <td colspan=\"5\"><hr /> </td></tr>   <tr>    <td width=\"15%\">Correct Response </td>    <td width=\"13%\">" + resType1 + " </td>    <td width=\"30%\"><input type='text' name='txtCorrResp" + num + "'  value='" + Dr["CorrRespDesc"].ToString() + "' /></td>    <td></td>    <td></td>  </tr>  <tr>    <td colspan=\"5\"><hr /> </td></tr><tr> <td colspan=\"2\" style='font-weight:bold;'>Mis Trial </td></tr><tr><td width='18%' colspan='2'><input type='checkbox' name='chkM1" + num + "' value='1' " + InMisTrialChk + " />Inc.Mis Trial</td>  <td colspan='3'><input type='text' name='txtM1Desc" + num + "' width='100%'  value='" + Dr["MisTrialDesc"].ToString() + "' ></td></tr>   <tr> <td style='font-weight:bold;'>Summary</td>   <td>  </td><td><span style='font-weight:bold;'>Report Label</span> </td>  "
                                             + " </tr> <tr>   <td><label>      <input type='checkbox' name='chkAcc" + num + "'  value='% Accuracy'  " + strAcc + " />% Accuracy</label></td>    <td></td>    <td colspan='3'><input type='text' name='txtAccDesc" + num + "' width='100%' value=" + AccInd + " ></td>    "
                                             + "</tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>(Total Correct Trials/Total Trials)*100</span></td> </tr> <tr><td> <label> <input type='checkbox' name='chkInpend" + num + "' id='' value='% Independant'  " + strInde + " /> % Independant</label></td>    <td></td>    <td colspan='3'><input type='text' name='txtInpendDesc" + num + "' id='' width='100%' value=" + IndeInd + " ></td>"
                                             + "</tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>(Total Independent Trials/Total Trials)*100</span></td> </tr></table>";
                        strPrompt = "";
                        strAcc = "";
                        strInde = "";
                        NaInd = "";
                        CustInd = "";
                        CalcType = "";

                        AccInd = "";
                        PromptInd = "";
                        IndeInd = "";
                    }
                }
            }

            else if (type == "Prompt")
            {
                try
                {
                    corrRes = Convert.ToInt32(Dr["CorrResp"]);
                }
                catch(Exception Ex)
                {
                    ClsErrorLog errlog = new ClsErrorLog();
                    errlog.WriteToLog("Page Name: "+clsGeneral.getPageName()+"\n"+ Ex.ToString());
                }
                strGETPrompt = "";
                Getresult(TemplateId.ToString(), corrRes.ToString());
                Option = "<option value='0' >+/-</option><option value='1' selected='selected' >Prompt</option><option value='2'>Text</option><option value='3' >Duration</option><option value='4'>Frequency</option>";
                if (DtCol != null)
                {
                    if (DtCol.Rows.Count > 0)
                    {

                        foreach (DataRow DrCol in DtCol.Rows)
                        {

                            CalcType = Convert.ToString(DrCol["CalcType"]);
                            if (CalcType == "% Accuracy")
                            {
                                strAcc = "checked='checked'";
                                AccInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else if (CalcType == "% Independant")
                            {
                                strInde = "checked='checked'";
                                IndeInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else if (CalcType == "% Prompted")
                            {
                                strPrompt = "checked='checked'";
                                PromptInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }


                        }
                    }
                }

                InnerHtml = "<table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=\'23%\' >Correct Response </td>    <td width=\'23%\'><input type='checkbox' name='rdoCorrent" + num + "'  value='1'    " + promptChecked + "  onclick='toggleVisibility(this," + num + ");' >Current Prompt</td>    <td width=\'30%\'></td>    <td></td>    <td></td>  </tr>  <tr><td colspan=\'5\'> </td></tr>  <tr width=\"100%\"  > <td colspan=\'4\'><div id='trDdl" + num + "' " + promptChkVisible + " ><table width=\'100%\'><tr>   <td width=\'23%\' >Select Prompt</td>    <td width=\'23%\' style='padding-left:10px;'><select  name='ddlCurrent" + num + "'  onchange='promptTake(this," + num + ");' >" + strGETPrompt + " </select></label></td>    <td width=\'30%\'><input type='text' name='txtCorrResp" + num + "'  value=" + Dr["CorrRespDesc"].ToString() + "  width=\'30%\' /></td>    <td><label  name='lblRes" + num + "' style=\'font-weight:bold;\' > </td>    <td><input type=\'text\' style=\'visibility:hidden\' ID='txtPromptId" + num + "'  name='txtPromptId" + num + "' /></td></tr></table></div></td> </tr>  <tr><td></td></tr><tr> <td colspan=\'1\' style='font-weight:bold;'>Mistrial </td><td style='font-weight:bold;'>Mistrial Label</td></tr><tr><td width='23%'><input type='checkbox' name='chkM1" + num + "' " + InMisTrialChk + " value='1' />Inc.Mis Trial</td> "
                 + "<td colspan='5'><input type='text' name='txtM1Desc" + num + "' width='100%' value='" + Dr["MisTrialDesc"].ToString() + "' ></td>  </tr>   <tr> <td style='font-weight:bold;'></td>   <td></td>   <td></td>    <tr>    <td colspan=\'5\'></td></tr><tr><td><span style='font-weight:bold;'>Summary </span></td><td colspan='2' style='font-weight:bold;'>Report Label</td> <td colspan='2'><span > </span></td>  </tr>  <tr>    <td><label>      <input type='checkbox' name='chkAcc" + num + "'  value='% Accuracy'  " + strAcc + "  />% Accuracy</label></td>    <td colspan='4'><input type='text' name='txtAccDesc" + num + "' width='100%' value='" + AccInd + "' ></td>    </tr><tr><td colspan='2' style='font-style:italic;padding-left:2px;'>(Total Correct Trials/Total Trials)*100</td></tr>  <tr>    <td> <label> <input type='checkbox' name='chkPrompted" + num + "'  value='% Prompted'  " + strPrompt + "  /> % Prompted</label></td>    <td colspan='4'><input type='text' name='txtPromptDesc" + num + "'  value='" + PromptInd + "'  id='' width='100%' ></td></tr> <tr><td colspan='2' style='font-style:italic;padding-left:2px;'>(Total Prompted Trials/Total Trials)*100</td></tr> <tr>    <td> <label> <input type='checkbox' name='chkInpend" + num + "' id='' value='% Independant' " + strInde + "  /> % Independant</label></td>    <td colspan='4'><input type='text' name='txtInpendDesc" + num + "' value='" + IndeInd + "'  id='' width='100%' ></td>    </tr><tr><td colspan='2' style='font-style:italic;padding-left:2px;'>(Total Independent Trials/Total Trials)*100</td></tr></table> ";



                //InnerHtml = "<table width='100%'> <tr><td colspan=\"5\"><hr /> </td></tr>  <tr>    <td width=\"23%\" >Correct Response </td>    <td width='23%'><input type='radio' name='rdoCorrent" + num + "' value='1' />Current Prompt</td>    <td width=\"30%\"><input type='radio' name='rdoCorrent" + num + "' value='0' checked='checked' />Select Prompt</td>    <td></td>    <td></td>  </tr>  <tr><td colspan=\"5\"> </td></tr>  <tr>    <td width=\"23%\" ></td>    <td width=\"23%\"><label  name='lblRes" + num + "' style=\"font-weight:bold;\" ><input type=\'text\' style=\'visibility:hidden\' ID=txtPromptId' + num + '  name=txtPromptId' + num + ' /><select name='ddlCurrent" + num + "'  onchange='promptTake(this," + num + ");' >" + strGETPrompt + " </select></label></td>    <td width=\"30%\"><input type='text' name='txtCorrResp" + num + "'  value=" + Dr["CorrRespDesc"].ToString() + " /></td>    <td></td>    <td></td>  </tr>  <tr><td></td></tr><tr> <td colspan=\"2\" style='font-weight:bold;'>Mis Trial </td></tr><tr><td width='23%'><input type='checkbox' name='chkM1" + num + "' value='1' " + InMisTrialChk + " />Inc.Mis Trial</td> "
                //+ "<td colspan='5'><input type='text' name='txtM1Desc" + num + "' width='100%' value=" + Dr["MisTrialDesc"].ToString() + " /></td>  </tr>   <tr> <td style='font-weight:bold;'></td>   <td></td>   <td></td>    <tr>    <td colspan=\"5\"></td></tr><tr><td><span style='font-weight:bold;'>Summary</span></td><td colspan='2'><span style='font-weight:bold;'>Report Label</span></td> <td colspan='2'></td>  </tr>  <tr>    <td><label>      <input type='checkbox' name='chkAcc" + num + "'  value='% Accuracy'  " + strAcc + "    />% Accuracy</label></td>    <td colspan='4'><input type='text' name='txtAccDesc" + num + "' width='100%' value='" + AccInd + "' /></td>    </tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>Report Label</span></td> </tr>  <tr>    <td> <label> <input type='checkbox' name='chkPrompted" + num + "'  value='% Prompted'  " + strPrompt + " /> % Prompted</label></td>    <td colspan='4'><input type='text' name='txtPromptDesc" + num + "' id='' width='100%' value=" + PromptInd + " /></td></tr> <tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>Report Label</span></td> </tr> <tr>    <td> <label> <input type='checkbox' name='chkInpend" + num + "' id='' value='% Independant'  " + strInde + " /> % Independant</label></td>    <td colspan='4'><input type='text' name='txtInpendDesc" + num + "' id='' width='100%' value='" + IndeInd + "' /></td>    </tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>Report Label</span></td> </tr></table> ";

                strPrompt = "";
                strAcc = "";
                strInde = "";
                NaInd = "";
                CustInd = "";
                CalcType = "";

                AccInd = "";
                PromptInd = "";
                IndeInd = "";
            }

            else if (type == "Text")
            {

                Option = "<option value='0' >+/-</option><option value='1' >Prompt</option><option value='2' selected='selected'>Text</option><option value='3' >Duration</option><option value='4'>Frequency</option>";
                if (DtCol != null)
                {
                    if (DtCol.Rows.Count > 0)
                    {

                        foreach (DataRow DrCol in DtCol.Rows)
                        {


                            CalcType = Convert.ToString(DrCol["CalcType"]);
                            //if (CalcType == "% Accuracy")
                            //{
                            //    strAcc = "checked='checked'";
                            //    AccInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            //}
                            //else if (CalcType == "% Independant")
                            //{
                            //    strInde = "checked='checked'";
                            //    IndeInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            //}
                            //else if (CalcType == "% Prompted")
                            //{
                            //    strPrompt = "checked='checked'";
                            //    PromptInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            //}
                            if (CalcType == "NA")
                            {
                                strNa = "checked='checked'";
                                NaInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else if (CalcType == "Customize")
                            {
                                strCust = "checked='checked'";
                                CustInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else
                            {
                                strPrompt = "";
                                strAcc = "";
                                strInde = "";
                                NaInd = "";
                                CustInd = "";

                            }
                        }
                    }
                }


                InnerHtml = "<table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=\'21%\' >Correct Response </td>    <td colspan='2'><input type='text' name='txtCorrResp" + num + "'  value=" + Dr["CorrRespDesc"].ToString() + " /></td>  <td width='1%'></td>    <td width='0%'></td>  </tr>  <tr><td colspan=\'4\'> </td></tr>  <tr>    <td width=\'21%\' ></td>    <td width=\'29%\'></td>    <td width=\"27%\"></td>    <td></td>  </tr>   <tr>    <td colspan=\'5\'><hr /> </td></tr> <tr> <td style='font-weight:bold;'>Mis Trial </td></tr><tr><td><input type='checkbox' name='chkM1" + num + "' value='1' " + InMisTrialChk + " />Inc.Mis Trial</td>  <td colspan='3'><input type='text' name='txtM1Desc" + num + "' id='' width='100%' value=" + Dr["MisTrialDesc"].ToString() + " ></td></tr><tr><td><span style='font-weight:bold;'>Summary </span></td><td><span style='font-weight:bold;'>Report Label</span></td> <td colspan=\'2\'></td>  "
                     + "</tr> <tr>    <td><label>      <input type='checkbox' name='chkNA" + num + "'  value='NA'  " + strNa + " /> NA</label></td>    <td colspan='3'><input type='text' name='txtNaDesc" + num + "' width='100%' value='" + NaInd + "' ></td>    </tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>No Calculation</span></td> </tr>  <tr>    <td><label>      <input type='checkbox' name='chkCustomize" + num + "'  value='Customize'  " + strCust + " />Customize</label></td>    <td colspan='3'><input type='text' name='txtCustDesc" + num + "'  id='txtCustDesc" + num + "' width='100%'  value='" + CustInd + "' ></td><td align='left'><img class='btn btn-purple' style='width: 17px; height: 17px;' onclick='createEquation(" + num + ",this)' src='../Administration/images/view-icon.png'>  </tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>Customized Calculation</span></td> </tr><tr>    <td></td>    <td colspan='2'></td> </tr></table> ";

                strPrompt = "";
                strAcc = "";
                strInde = "";
                NaInd = "";
                CustInd = "";
                CalcType = "";

                AccInd = "";
                PromptInd = "";
                IndeInd = "";
            }









            else if (type == "Duration")
            {

                Option = "<option value='0' >+/-</option><option value='1' >Prompt</option><option value='2'>Text</option><option value='3'  selected='selected' >Duration</option><option value='4'>Frequency</option>";
                if (DtCol != null)
                {
                    if (DtCol.Rows.Count > 0)
                    {

                        foreach (DataRow DrCol in DtCol.Rows)
                        {


                            CalcType = Convert.ToString(DrCol["CalcType"]);

                            if (CalcType == "AvgDuration")
                            {
                                strNa = "checked='checked'";
                                NaInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else if (CalcType == "TotalDuration")
                            {
                                strCust = "checked='checked'";
                                CustInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else
                            {
                                strPrompt = "";
                                strAcc = "";
                                strInde = "";
                                NaInd = "";
                                CustInd = "";

                            }
                        }
                    }
                }


                InnerHtml = "<table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=0 >Correct Response </td>    <td colspan='2'><input type='text' name='txtCorrResp" + num + "'   value=" + Dr["CorrRespDesc"].ToString() + "  ></td>  <td width='1%'></td>    <td width='0'></td>  </tr>  <tr><td colspan=\'4\'> </td></tr>  <tr>    <td width=\'21%\' ></td>    <td width=\'29%\'></td>    <td width=\"27%\"></td>    <td></td>  </tr>   <tr>    <td colspan=\'5\'><hr /> </td></tr> <tr> <td style='font-weight:bold;'>Mistrial </td><td style='font-weight:bold;'>Mistrial Label</td></tr><tr><td><input type='checkbox' name='chkM1" + num + "' value='1' " + InMisTrialChk + " />Include Mistrial</td>  <td colspan='4'><input type='text' name='txtM1Desc" + num + "' width=\"100%\"  value=" + Dr["MisTrialDesc"].ToString() + " ></td> <td></td> </tr><tr><td><span style='font-weight:bold;'>Summary </span></td><td style='font-weight:bold;'>Report Label</td> <td colspan=\"4\" ></td>  "
    + "</tr><tr><td><label>      <input type='checkbox' name='chkAvgDuration" + num + "'  value='1' " + strNa + "  /> Avg Duration         </label></td>    <td colspan='4'><input type='text' name='txtAvgDesc" + num + "' width='100%'  value='" + NaInd + "' ></td> <td></td>   </tr>  <tr>    <td><label>      <input type='checkbox' name='chkTotalDuration" + num + "' value='1' " + strCust + " /> Total Duration  </label></td>    <td colspan='4'><input type='text' name='txtTotalDesc" + num + "' id='txtTotalDesc" + num + "' width='100%' value='" + CustInd + "' ></td><td align='left'></td>  </tr><tr>    <td></td>    <td colspan='2'></td> </tr><tr><td colspan='2'></td></tr></table> ";

                //InnerHtml = "<table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=\'21%\' >Correct Response </td>    <td colspan='2'><input type='text' name='txtCorrResp" + num + "'  value=" + Dr["CorrRespDesc"].ToString() + " /></td>  <td width='1%'></td>    <td width='0%'></td>  </tr>  <tr><td colspan=\'4\'> </td></tr>  <tr>    <td width=\'21%\' ></td>    <td width=\'29%\'></td>    <td width=\"27%\"></td>    <td></td>  </tr>   <tr>    <td colspan=\'5\'><hr /> </td></tr> <tr> <td style='font-weight:bold;'>Mis Trial </td></tr><tr><td><input type='checkbox' name='chkM1" + num + "' value='1' " + InMisTrialChk + " />Inc.Mis Trial</td>  <td colspan='3'><input type='text' name='txtM1Desc" + num + "' id='' width='100%' value=" + Dr["MisTrialDesc"].ToString() + " ></td></tr><tr><td><span style='font-weight:bold;'>Summary </span></td><td><span style='font-weight:bold;'>Report Label</span></td> <td colspan=\'2\'></td>  "
                //     + "</tr> <tr>    <td><label>      <input type='checkbox' name='chkNA" + num + "'  value='NA'  " + strNa + " /> NA</label></td>    <td colspan='3'><input type='text' name='txtNaDesc" + num + "' width='100%' value='" + NaInd + "' ></td>    </tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>No Calculation</span></td> </tr>  <tr>    <td><label>      <input type='checkbox' name='chkCustomize" + num + "'  value='Customize'  " + strCust + " />Customize</label></td>    <td colspan='3'><input type='text' name='txtCustDesc" + num + "'  id='txtCustDesc" + num + "' width='100%'  value='" + CustInd + "' ></td><td align='left'><img class='btn btn-purple' style='width: 17px; height: 17px;' onclick='createEquation(" + num + ",this)' src='../Administration/images/view-icon.png'>  </tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>Customized Calculation</span></td> </tr><tr>    <td></td>    <td colspan='2'></td> </tr></table> ";

                strPrompt = "";
                strAcc = "";
                strInde = "";
                NaInd = "";
                CustInd = "";
                CalcType = "";

                AccInd = "";
                PromptInd = "";
                IndeInd = "";
            }


            else if (type == "Frequency")
            {

                Option = "<option value='0' >+/-</option><option value='1' >Prompt</option><option value='2'>Text</option><option value='3'   >Duration</option><option value='4' selected='selected' >Frequency</option>";
                if (DtCol != null)
                {
                    if (DtCol.Rows.Count > 0)
                    {

                        foreach (DataRow DrCol in DtCol.Rows)
                        {

                            CalcType = Convert.ToString(DrCol["CalcType"]);

                            if (CalcType == "Frequency")
                            {
                                strNa = "checked='checked'";
                                NaInd = Convert.ToString(DrCol["CalcRptLabel"]);
                            }
                            else
                            {
                                strPrompt = "";
                                strAcc = "";
                                strInde = "";
                                NaInd = "";
                                CustInd = "";

                            }
                        }
                    }
                }


                InnerHtml = "<table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=0 >Correct Response </td>    <td colspan='2'><input type='text' name='txtCorrResp" + num + "'   value=" + Dr["CorrRespDesc"].ToString() + " ></td>  <td width='1%'></td>    <td width='0'></td>  </tr>  <tr><td colspan=\'4\'> </td></tr>  <tr>    <td width=\'21%\' ></td>    <td width=\'29%\'></td>    <td width=\"27%\"></td>    <td></td>  </tr>   <tr>    <td colspan=\'5\'><hr /> </td></tr> <tr> <td style='font-weight:bold;'>Mistrial </td><td style='font-weight:bold;'>Mistrial Label</td></tr><tr><td><input type='checkbox' name='chkM1" + num + "' value='1' " + InMisTrialChk + "  />Include Mistrial</td>  <td colspan='4'><input type='text' name='txtM1Desc" + num + "' width='100%'  value=" + Dr["MisTrialDesc"].ToString() + "  ></td> <td></td> </tr><tr><td><span style='font-weight:bold;'>Summary </span></td><td style='font-weight:bold;'>Report Label</td> <td colspan=\"4\" ></td>  "
                    + "</tr><tr><td><label>      <input type='checkbox' name='chkFrequency" + num + "'  value='1' " + strNa + "  />     Frequency          </label></td>    <td colspan='4'><input type='text' name='txtFrequencyDesc" + num + "' width='100%'  value='" + NaInd + "'></td> <td></td>   </tr>  <tr>    <td><label></label></td>    <td colspan='4'>&nbsp;</td><td align='left'></td>  </tr><tr>    <td></td>    <td colspan='2'></td> </tr></table> ";


                strPrompt = "";
                strAcc = "";
                strInde = "";
                NaInd = "";
                CustInd = "";
                CalcType = "";

                AccInd = "";
                PromptInd = "";
                IndeInd = "";
            }
















            if (InnerHtml == "")
            {
                Option = "<option value='0' selected='selected'>+/-</option><option value='1' >Prompt</option><option value='2'>Text</option>";
                InnerHtml = "<table width='100%'>    <tr>    <td colspan=\"5\"><hr /> </td></tr>   <tr>    <td width=\"15%\">Correct Response </td>    <td width=\"13%\">" + resType1 + " </td>    <td width=\"30%\"><input type='text' name='txtCorrResp" + num + "'  value=" + Dr["CorrRespDesc"].ToString() + " /></td>    <td></td>    <td></td>  </tr>  <tr>    <td colspan=\"5\"><hr /> </td></tr><tr> <td colspan=\"2\" style='font-weight:bold;'>Mis Trial </td></tr><tr><td width='18%' colspan='2'><input type='checkbox' name='chkM1" + num + "' value='1' " + InMisTrialChk + " />Inc.Mis Trial</td>  <td colspan='3'><input type='text' name='txtM1Desc" + num + "' width='100%'  value=" + Dr["MisTrialDesc"].ToString() + " /></td></tr>   <tr> <td style='font-weight:bold;'>Summary</td>   <td>  </td><td><span style='font-weight:bold;'>Report Label</span> </td>  "
                                            + " </tr> <tr>   <td><label>      <input type='checkbox' name='chkAcc" + num + "'  value='% Accuracy'  " + strAcc + "' />% Accuracy</label></td>    <td></td>    <td colspan='3'><input type='text' name='txtAccDesc" + num + "' width='100%' value='" + AccInd + "' /></td>    "
                                            + "</tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>(Total Correct Trials/Total Trials)*100</span></td> </tr>  <tr>    <td> <label> <input type='checkbox' name='chkPrompted" + num + "' value='% Prompted'   " + strPrompt + "  /> % Prompted</label></td>    <td></td><td colspan='3'><input type='text' name='txtPromptDesc" + num + "' id='' width='100%' value='" + PromptInd + "' /></td>    <td></td>    <td></td> "
                                            + " </tr> <tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>(Total Prompted Trials/Total Trials)*100</span></td> </tr> <tr>    <td> <label> <input type='checkbox' name='chkInpend" + num + "' id='' value='% Independant'  " + strInde + " /> % Independant</label></td>    <td></td>    <td colspan='3'><input type='text' name='txtInpendDesc" + num + "' id='' width='100%' value='" + IndeInd + "' /></td>"
                                            + "</tr><tr><td colspan='5'><span style='font-style:italic;font-size:11px;'>(Total Independent Trials/Total Trials)*100</span></td> </tr></table>";
            }

            html = "<div name id='Div" + num + "' class='contentDiv' name='Div" + num + "' > <table style=\"width: 100%;\"><tr><td width=\"17%\">Column Name</td><td  width=\'70%\'><input name='txtName" + num + "'  id='txtName" + num + "' width='150px' type='text' value='" + Dr["ColName"].ToString() + "' />&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  "
                          + "<a href=\'#\' target=\'_self\' onClick=\'decCount(" + num + ")\'>Remove</a></td></tr><tr><td>Type </td><td>  <select name='ddlPrompType" + num + "' id='ddlPrompType" + num + "' onchange='promptChange(this," + num + ")';>" + Option + "</select><input name='txtPromptType" + num + "' id='txtPromptType" + num + "' type='text' value=" + Dr["ColTypeCd"].ToString() + " size='12' style='visibility:hidden;' /></td></tr> "
                          + "<tr><td colspan='2'><input name=" + txt4 + "  type='hidden' value=" + Dr["DSTempSetColId"].ToString() + " /> <div name id='innerDiv" + num + "' name='innerDiv" + num + "' >" + InnerHtml + "</div> </td></tr></table></div> ";



            Html = Html + html;



        }

    }
}

