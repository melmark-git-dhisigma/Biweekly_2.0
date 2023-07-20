
searchReq = getXmlHttpRequestObject();
var strDropdownlist = "";
var strResult = "";
function getXmlHttpRequestObject() {
    if (window.XMLHttpRequest) {
        return new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        return new ActiveXObject("Microsoft.XMLHTTP");
    }
    else {
        alert("Your Browser Sucks!\nIt's about time to upgrade don't you think?");
    }
}
function fillSuggest() {
    if (searchReq.readyState == 4 || searchReq.readyState == 0) {

        searchReq.open("GET", 'TempColumnDef.aspx?Type=fill', true);
        searchReq.onreadystatechange = handleFillSuggest;
        searchReq.send(null);
    }

}

function handleFillSuggest() {
    if (searchReq.readyState == 4) {
        strDropdownlist = searchReq.responseText.split('---------------');
        strResult = strDropdownlist[0];

    }
}


function incrementCount() {

    addTextBox();
}

function decCount(divNo) {

    removeTextBox(divNo);
}
function removeFromCode(divNo) {

    tCount = parseInt(document.getElementById('hidTotalCount').value) - 1;
    document.getElementById('hidTotalCount').value = tCount.toString();

    if (tCount == 0) {
        document.getElementById('btnSave').setAttribute("Display", "none");
    }
    else {
        document.getElementById('btnSave').setAttribute("Display", "block");
    }

    var div = "Div" + divNo;
    var ni = document.getElementById('content');
    var d = document.getElementById(div);
    ni.removeChild(d);
}
function removeTextBox(divNo) {

    tCount = parseInt(document.getElementById('hidTotalCount').value) - 1;

    document.getElementById('hidTotalCount').value = tCount.toString();

    var div = "Div" + divNo;
    var ni = document.getElementById('content');
    var d = document.getElementById(div);
    ni.removeChild(d);
}

function removeMatchText(divNo) {

    var div = "test" + divNo;
    element = document.getElementById(div);
    element.parentNode.removeChild(element);
}


function addTextBox() {



    var ni = document.getElementById('content');
    var numi = document.getElementById('divNo');
    var num = (parseInt(document.getElementById('divNo').value)) + 1;

    var tCount = 0;
    tCount = parseInt(document.getElementById('hidTotalCount').value) + 1;
    document.getElementById('hidTotalCount').value = tCount.toString();


    numi.value = num;
    var newdiv = document.createElement('div');
    var divIdName = 'Div' + num;
    newdiv.setAttribute('id', divIdName);

    newdiv.setAttribute('class', "contentDiv1");

    html = '<table width=\'100%\' >    <tr>    <td colspan=\'5\'><hr /> </td></tr>   <tr>    <td width=\'15%\'>Correct Response </td>    <td width=\'13%\'><input type="radio" name="group2' + num + '" value="+"> + <input type="radio" name="group2' + num + '" value="-" checked="checked"> -</td>    <td width=\'30%\'><input type="text" name="txtCorrResp' + num + '" /></td>    <td></td>    <td></td>  </tr>  <tr>    <td colspan=\'5\'><hr /> </td></tr><tr> <td colspan=\'2\' style="font-weight:bold;">Mistrial </td><td style="font-weight:bold;">Mistrial Label</td></tr><tr><td width="18%" colspan="2"><input type="checkbox" name="chkM1' + num + '" value="1" />Include Mistrial</td>  <td colspan="3"><input type="text" name="txtM1Desc' + num + '" width="100%" /></td></tr>   <tr> <td style="font-weight:bold;">Summary</td>   <td></td>   <td style="font-weight:bold;">Report labels</td>  '
        + ' </tr> <tr>   <td><label>      <input type="checkbox" name="chkAcc' + num + '"  value="% Accuracy" />% Accuracy</label></td>    <td></td>    <td colspan="3"><input type="text" name="txtAccDesc' + num + '" width="100%" /></td>    '
        + '</tr>  <tr>    <td  colspan="4"  style="font-style:italic;padding-left:2px;">(Total Correct Trials/Total Trials)*100</td></tr>  <tr>    <td> <label> <input type="checkbox" name="chkPrompted' + num + '"  value="% Prompted" /> % Prompted</label></td>    <td></td><td colspan="3"><input type="text" name="txtPromptDesc' + num + '" id="" width="100%" /></td>    <td></td>    <td>\</td>'
        + ' </tr> <tr>    <td  colspan="4"  style="font-style:italic;padding-left:2px;">(Total Prompted Trials/Total Trials)*100</td></tr>  <tr>    <td> <label> <input type="checkbox" name="chkInpend' + num + '" id="" value="% Independant" /> % Independant</label></td>    <td></td>    <td colspan="3"><input type="text" name="txtInpendDesc' + num + '" id="" width="100%" /></td>'
        + '</tr><tr>    <td colspan="4"  style="font-style:italic;padding-left:2px;">(Total Independent Trials/Total Trials)*100</td></tr> </table>';

    newdiv.innerHTML = ' <div name id="Div' + num + '" name="Div' + num + '" > <table style=\'width: 100%;\'><tr><td width=\'17%\'>Column Name</td><td  width=\'70%\'><input type="text" cssClass="txtCommon"  name="txtName' + num + '"  id="txtName' + num + '" >&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  '
      + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Type </td><td>  <select name="ddlPrompType' + num + '" onchange="promptChange(this,' + num + ');"> <option value="0">+/-</option> <option value="1">Prompt</option> <option value="2">Text</option><option value="3">Duration</option><option value="4">Frequency</option></select><input name="txtPromptType' + num + '" id="txtPromptType' + num + '" type="text" value="+/-" size="12" style="visibility:hidden;"  /></td></tr>'

      + '<tr><td colspan="2"> <div name id="innerDiv' + num + '" name="innerDiv' + num + '" >' + html + '</div> </td></tr>'


      + '</table></div>';

    ni.appendChild(newdiv);



}

function promptTake(selectObj, divNo) {
   
    var txtPromptId = "txtPromptId" + divNo;
    var idx = selectObj.value;
    document.getElementById(txtPromptId).value = idx;   
}
function promptChange(selectObj, divNo) {
    document.getElementById('dvEquation').style.display = "none";
    var inDiv = "innerDiv" + divNo;
    var num = divNo;

    var txtPromptType = "txtPromptType" + divNo;

    var html = "";

    var idx = selectObj.options[selectObj.selectedIndex].text;
    document.getElementById(txtPromptType).value = idx;

    idx = selectObj.selectedIndex;


    if (idx == "0") {

        html = '<table width=\'100%\' >    <tr>    <td colspan=\'5\'><hr /> </td></tr>   <tr>    <td width=\'15%\'>Correct Response </td>    <td width=\'13%\'><input type="radio" name="group2' + num + '" value="+"> + <input type="radio" name="group2' + num + '" value="-" checked="checked"> -</td>    <td width=\'30%\'><input type="text" name="txtCorrResp' + num + '" /></td>    <td></td>    <td></td>  </tr>  <tr>    <td colspan=\'5\'><hr /> </td></tr><tr> <td  style="font-weight:bold;">Mistrial </td><td></td><td style="font-weight:bold;">Mistrial Label</td></tr><tr><td width="18%" colspan="2"><input type="checkbox" name="chkM1' + num + '" value="1" />Include Mistrial</td>  <td colspan="3"><input type="text" name="txtM1Desc' + num + '" width="100%" /></td></tr>   <tr> <td style="font-weight:bold;">Summary</td>   <td></td>   <td></td>  '
               + ' </tr>    <td><label>      <input type="checkbox" name="chkAcc' + num + '"  value="% Accuracy" />% Accuracy</label></td>    <td></td>    <td colspan="3"><input type="text" name="txtAccDesc' + num + '" width="100%" /></td>    '
               + '</tr><tr><td colspan="3" style="font-style:italic;padding-left:2px;">(Total Correct Trials/Total Trials)*100</td></tr>  <tr>    <td> <label> <input type="checkbox" name="chkInpend' + num + '" id="" value="% Independant" /> % Independant</label></td>    <td></td>    <td colspan="3"><input type="text" name="txtInpendDesc' + num + '" id="" width="100%" /></td>'
               + '</tr><tr><td colspan="2" style="font-style:italic;padding-left:2px;">(Total Independent Trials/Total Trials)*100</td></tr></table>';
        //<tr>    <td> <label> <input type="checkbox" name="chkPrompted' + num + '"  value="% Prompted" /> % Prompted</label></td>    <td></td><td colspan="3"><input type="text" name="txtPromptDesc' + num + '" id="" width="100%" /></td>    <td></td>    <td>\</td>'
        //+ ' </tr> <tr><td colspan="2" style="font-style:italic;padding-left:2px;">(Total Prompted Trials/Total Trials)*100</td></tr> 
        document.getElementById(inDiv).innerHTML = html;


    } else if (idx == "1") {
        strResult = "";
        fillSuggest();
        alert("Please Wait....");
        html = '<table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=\'23%\' >Correct Response </td>    <td width=\'23%\'><input type="checkbox" name="rdoCorrent' + num + '"  value="1" checked="checked"  onclick="toggleVisibility(this,' + num + ');" >Current Prompt</td>    <td width=\'30%\'></td>    <td></td>    <td></td>  </tr>  <tr><td colspan=\'5\'> </td></tr>  <tr width=\"100%\"  > <td colspan=\'4\'><div id="trDdl' + num + '" style="display:none"><table width=\'100%\'><tr>   <td width=\'23%\' >Select Prompt</td>    <td width=\'23%\' style="padding-left:10px;"><select  name="ddlCurrent' + num + '"  onchange="promptTake(this,' + num + ');" >' + strResult + ' </select></label></td>    <td width=\'30%\'><input type="text" name="txtCorrResp' + num + '" width=\'30%\' /></td>    <td><label  name="lblRes' + num + '" style=\'font-weight:bold;\' > </td>    <td><input type=\'text\' style=\'visibility:hidden\' ID="txtPromptId' + num + '"  name="txtPromptId' + num + '" \></td></tr></table></div></td> </tr>  <tr><td></td></tr><tr> <td colspan=\'1\' style="font-weight:bold;">Mistrial </td><td style="font-weight:bold;">Mistrial Label</td></tr><tr><td width="23%"><input type="checkbox" name="chkM1' + num + '" value="1" />Inc.Mis Trial</td> '
  + '<td colspan="5"><input type="text" name="txtM1Desc' + num + '" width="100%" /></td>  </tr>   <tr> <td style="font-weight:bold;"></td>   <td></td>   <td></td>    <tr>    <td colspan=\'5\'></td></tr><tr><td><span style="font-weight:bold;">Summary </span></td><td colspan="2" style="font-weight:bold;">Report Label</td> <td colspan="2"><span > </span></td>  </tr>  <tr>    <td><label>      <input type="checkbox" name="chkAcc' + num + '"  value="% Accuracy" />% Accuracy</label></td>    <td colspan="4"><input type="text" name="txtAccDesc' + num + '" width="100%" /></td>    </tr><tr><td colspan="2" style="font-style:italic;padding-left:2px;">(Total Correct Trials/Total Trials)*100</td></tr>  <tr>    <td> <label> <input type="checkbox" name="chkPrompted' + num + '"  value="% Prompted" /> % Prompted</label></td>    <td colspan="4"><input type="text" name="txtPromptDesc' + num + '" id="" width="100%" /></td></tr> <tr><td colspan="2" style="font-style:italic;padding-left:2px;">(Total Prompted Trials/Total Trials)*100</td></tr> <tr>    <td> <label> <input type="checkbox" name="chkInpend' + num + '" id="" value="% Independant" /> % Independant</label></td>    <td colspan="4"><input type="text" name="txtInpendDesc' + num + '" id="" width="100%" /></td>    </tr><tr><td colspan="2" style="font-style:italic;padding-left:2px;">(Total Independent Trials/Total Trials)*100</td></tr></table>';


        document.getElementById(inDiv).innerHTML = html;


    }
    else if (idx == "2") {

        html = '<table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=\'20%\' >Correct Response </td>    <td colspan="2"><input type="text" name="txtCorrResp' + num + '" /></td>  <td width="1%"></td>    <td width="0%"></td>  </tr>  <tr><td colspan=\'4\'> </td></tr>  <tr>    <td width=\'21%\' ></td>    <td width=\'29%\'></td>    <td width=\"27%\"></td>    <td></td>  </tr>   <tr>    <td colspan=\'5\'><hr /> </td></tr> <tr> <td style="font-weight:bold;">Mistrial </td><td style="font-weight:bold;">Mistrial Label</td></tr><tr><td><input type="checkbox" name="chkM1' + num + '" value="1" />Include Mistrial</td>  <td colspan="4"><input type="text" name="txtM1Desc' + num + '" width=\"100%\" /></td> <td></td> </tr><tr><td><span style="font-weight:bold;">Summary </span></td><td style="font-weight:bold;">Report Label</td> <td colspan=\"4\" ></td>  '
+ '</tr><tr><td><label>      <input type="checkbox" name="chkNA' + num + '"  value="1" /> NA</label></td>    <td colspan="4"><input type="text" name="txtNaDesc' + num + '" width="100%" /></td> <td></td>   </tr><tr><td colspan="4" style="font-style:italic;padding-left:2px;">No Calculation</td></tr>  <tr>    <td><label>      <input type="checkbox" name="chkCustomize' + num + '"  value="1" />Customize</label></td>    <td colspan="4"><input type="text" name="txtCustDesc' + num + '" id="txtCustDesc' + num + '" width="100%" /></td><td align="left"><img class="btn btn-purple" style="width: 17px; height: 17px;" onclick="createEquation(' + num + ',this)" src="../Administration/images/view-icon.png"></td>  </tr><tr><td colspan="2" style="font-style:italic;padding-left:2px;">Customized Calculation</td></tr><tr>    <td></td>    <td colspan="2"></td> </tr></table>'




        document.getElementById(inDiv).innerHTML = html;
    }
    else if (idx == "3") {

        html = ' <table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=0 >Correct Response </td>    <td colspan="2"><input type="text" name="txtCorrResp' + num + '" /></td>  <td width="1%"></td>    <td width="0"></td>  </tr>  <tr><td colspan=\'4\'> </td></tr>  <tr>    <td width=\'21%\' ></td>    <td width=\'29%\'></td>    <td width=\"27%\"></td>    <td></td>  </tr>   <tr>    <td colspan=\'5\'><hr /> </td></tr> <tr> <td style="font-weight:bold;">Mistrial </td><td style="font-weight:bold;">Mistrial Label</td></tr><tr><td><input type="checkbox" name="chkM1' + num + '" value="1" />Include Mistrial</td>  <td colspan="4"><input type="text" name="txtM1Desc' + num + '" width=\"100%\" /></td> <td></td> </tr><tr><td><span style="font-weight:bold;">Summary </span></td><td style="font-weight:bold;">Report Label</td> <td colspan=\"4\" ></td>  '
    + '</tr><tr><td><label>      <input type="checkbox" name="chkAvgDuration' + num + '"  value="1" /> Avg Duration         </label></td>    <td colspan="4"><input type="text" name="txtAvgDesc' + num + '" width="100%" /></td> <td></td>   </tr>  <tr>    <td><label>      <input type="checkbox" name="chkTotalDuration' + num + '"  value="1" /> Total Duration  </label></td>    <td colspan="4"><input type="text" name="txtTotalDesc' + num + '" id="txtTotalDesc' + num + '" width="100%" /></td><td align="left"></td>  </tr><tr>    <td></td>    <td colspan="2"></td> </tr><tr><td colspan="2"></td></tr></table> '
        document.getElementById(inDiv).innerHTML = html;
    }

    else if (idx == "4") {
        html = ' <table width=\'100%\' > <tr><td colspan=\'5\'><hr /> </td></tr>  <tr>    <td width=0 >Correct Response </td>    <td colspan="2"><input type="text" name="txtCorrResp' + num + '" /></td>  <td width="1%"></td>    <td width="0"></td>  </tr>  <tr><td colspan=\'4\'> </td></tr>  <tr>    <td width=\'21%\' ></td>    <td width=\'29%\'></td>    <td width=\"27%\"></td>    <td></td>  </tr>   <tr>    <td colspan=\'5\'><hr /> </td></tr> <tr> <td style="font-weight:bold;">Mistrial </td><td style="font-weight:bold;">Mistrial Label</td></tr><tr><td><input type="checkbox" name="chkM1' + num + '" value="1" />Include Mistrial</td>  <td colspan="4"><input type="text" name="txtM1Desc' + num + '" width=\"100%\" /></td> <td></td> </tr><tr><td><span style="font-weight:bold;">Summary </span></td><td style="font-weight:bold;">Report Label</td> <td colspan=\"4\" ></td>  '
+ '</tr><tr><td><label>      <input type="checkbox" name="chkFrequency' + num + '"  value="1" />     Frequency          </label></td>    <td colspan="4"><input type="text" name="txtFrequencyDesc' + num + '" width="100%" /></td> <td></td>   </tr>  <tr>    <td><label></label></td>    <td colspan="4">&nbsp;</td><td align="left"></td>  </tr><tr>    <td></td>    <td colspan="2"></td> </tr></table>';

        document.getElementById(inDiv).innerHTML = html;
    }


}
function closeEquation() {

    document.getElementById('divResult').innerHTML = "";
    document.getElementById('dvEquation').style.display = "none";
}


var gDivNo;

function copyEquation() {

    var txt = "txtCustDesc" + gDivNo;

    document.getElementById(txt).value = (document.getElementById('divResult').innerText).trim();
    document.getElementById('divResult').innerHTML = "";
    lastValue = "";
    document.getElementById('dvEquation').style.display = "none";
}
function assignEquation(divNo) {
    var txt = "txtCustDesc" + divNo;
    //alert(document.getElementById('txtEquation').value);
    document.getElementById(txt).value = document.getElementById('txtEquation').value;
}

function createEquation(divNo, elem) {

    gDivNo = divNo;
    var txt = "txtCustDesc" + gDivNo;
    document.getElementById('divResult').innerText = document.getElementById(txt).value;
    var left = $(elem).position().left;
    var top = $(elem).position().top;
    $('#dvEquation').css({ left: (left - 500) + 'px', top: (top - 185) + 'px' }).show();
    fillColumns();
}
var lastValue = "";
function fillColumns() {
    var divCount = document.getElementById("divNo").value;
    var divCol = document.getElementById('divColumn');
    divCol.innerHTML = "";

    for (var i = 2; i <= divCount; i++) {

        var name = "txtName" + i.toString() + "";

        var nameN = document.getElementById(name).value;

        var txtPromptType = "txtPromptType" + i.toString() + "";

        if (nameN != "" && document.getElementById(txtPromptType).value == "Text") {
            divCol.innerHTML = divCol.innerHTML + " <input type='button' id='NFButton' style='width: auto;margin-right:4px;' class='column' value='" + nameN + "'  alt='" + nameN + "' />";

            //alert(divCol);

            $(divCol).find('.column').click(function () {
                var val = $(this).attr('alt');
                var current = document.getElementById('divResult').innerHTML;
                // if (val != lastValue) {
                divResult.innerHTML = current + val;
                lastValue = val;
                FillArray();
                // }
                //else {
                //alert("U cannot insert this again !!!");
                //lastValue = "";
                // }
            });

        }
    }
}



function toggleVisibility(theCheckbox, divNo) {

    var div = "trDdl" + divNo;
    if (theCheckbox.checked)
        document.getElementById(div).style.display = "none";
    else
        document.getElementById(div).style.display = "block";
}

function UpdateTextBox(count) {

    for (var i = 0; i < count; i++) {

        var isMatch = parseInt(document.getElementById('hidMathToSample').value);
        var ni = document.getElementById('content');
        var numi = document.getElementById('divNo');
        var num = (parseInt(document.getElementById('divNo').value)) + 1;
        numi.value = num;
        var newdiv = document.createElement('div');
        var divIdName = 'Div' + num;
        newdiv.setAttribute('id', divIdName);

        newdiv.setAttribute('class', "contentDiv1");

        if (isMatch == 1) {
            newdiv.innerHTML = '<table style=\'width: 68%;\'><tr><td width=\'50%\'>Set Name</td><td><input type="text" size="30"  name="txtName' + num + '"  id="txtName' + num + '" >&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  '
              + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Step Description </td><td><textarea   name=txtDesc' + num + ' cols=\'50\' rows=\'5\'></textarea></td></tr>'
              + '<tr><td colspan=\'2\'><div id=Match' + num + ' style=\'width:100%\'></div></td></tr>'
              + '<tr><td>Match to Sample </td><td><input type=\'text\' size="30"  ID=txtSample' + num + ' \>&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'getMatchToSample(' + num + ')\'>Add</a><input type=\'text\' style=\'visibility:hidden\' ID=txtMatch' + num + '  name=txtMatch' + num + ' \></td></tr>'
              + '</table>';
        }
        else {
            newdiv.innerHTML = '<table style=\'width: 68%;\'><tr><td width=\'50%\'>Set Name</td><td><input type="text" size="30"  name="txtName' + num + '"  id="txtName' + num + '" >&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;  '
             + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Step Description </td><td><textarea   name=txtDesc' + num + ' cols=\'50\' rows=\'5\'></textarea></td></tr>'
             + '</table>';
        }
        ni.appendChild(newdiv);

    }

}


