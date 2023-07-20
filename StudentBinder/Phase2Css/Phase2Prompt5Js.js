
searchReq = getXmlHttpRequestObject();
var strDropdownlist = "";
var strResult = "";
var strResult1 = "";
var divsNo = "";
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

        searchReq.open("GET", 'TempStepCriteria.aspx?Type=fill', true);
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

function fillColAfterUpdate() {

    if (searchReq.readyState == 4 || searchReq.readyState == 0) {
        searchReq.open("GET", 'TempStepCriteria.aspx?Type=fill', true);
        searchReq.onreadystatechange = handleCol;
        searchReq.send(null);

    }

}

function handleCol() {
    if (searchReq.readyState == 4) {
        strDropdownlist = searchReq.responseText.split('---------------');
        strResult = strDropdownlist[0];
        var divMes = "divColSelect" + divsNo;
        document.getElementById(divMes).innerHTML = "<select id='tempCol" + divsNo + "' name='tempCol" + divsNo + "' onchange='colChange(this," + divsNo + ");' style='width: 250px'>" + strResult + "</select>";

    }

}





function fillSuggest1(idx) {

    if (searchReq.readyState == 4 || searchReq.readyState == 0) {
        searchReq.open("GET", 'TempStepCriteria.aspx?Type=Mes&Id=' + idx + '', true);
        searchReq.onreadystatechange = handleFillSuggest1;
        searchReq.send(null);

    }

}

function handleFillSuggest1() {
    if (searchReq.readyState == 4) {
        strDropdownlist = searchReq.responseText.split('---------------');
        strResult1 = strDropdownlist[0];
        var divMes = "divMeasureSelect" + divsNo;
        //divsNo = divsNo.trim()
        document.getElementById(divMes).innerHTML = "<select id='drpMes" + divsNo + "' name='drpMes" + divsNo + "' onchange='calChange(this," + divsNo + ");' style='width: 250px'>" + strResult1 + "</select>";
    }

}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))

        return false;

    return true;
}


function incrementCount() {

    addTextBox("0");
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
    var divTemp = '#' + div;
    var d = document.getElementById(div);
    $(divTemp).remove();
}

function removeMatchText(divNo) {

    var div = "test" + divNo;
    element = document.getElementById(div);
    element.parentNode.removeChild(element);
}


function populateFirst(Column, Measre) {


    strResult = Column;
    strResult1 = Measre;

    addTextBox("0");
}

function addTextBoxU(type) {

    var ni = document.getElementById('content');
    var numi = document.getElementById('divNo');


    var num = 0;

    var tCount = 0;
    tCount = parseInt(document.getElementById('hidTotalCount').value) + 1;
    document.getElementById('hidTotalCount').value = tCount.toString();



    if (type == "1" || type == "2" || type == "3") {
        type == "0";
    }

    if (type == "1") {

        num = (parseInt(document.getElementById('divNo').value)) + 1;
        html = '<div id="Div' + num + '" ><div class="contentDivider" > <table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead1" >TO MOVE UP</div></td><td width="15%"><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="1" runat="server" size="1" style="visibility:hidden;"/><input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /> <input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(1)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><div id="divColSelect' + num + '"  name="divColSelect' + num + '" >' + strResult + '</div></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1"  onClick="enableSession(1,' + num + ')"  />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="0" checked="checked" onClick="enableSession(2,' + num + ')"  />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="select2" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  readOnly="readOnly"  /></td></tr><tr><td>Required Score </td> '
        + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table></div></div> ';

        document.getElementById('newMoveUp').innerHTML = document.getElementById('newMoveUp').innerHTML + html;

        numi.value = num;
    }
    else if (type == "2") {
        num = (parseInt(document.getElementById('divNo').value)) + 1;

        html = '<div id="Div' + num + '" ><div class="contentDivider" ><table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead2" >TO MOVE DOWN</div></td><td width="15%"><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="2" runat="server" size="1" style="visibility:hidden;"  /> <input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /> <input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(2)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><div id="divColSelect' + num + '"  name="divColSelect' + num + '" >' + strResult + '</div></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1"  onClick="enableSession(1,' + num + ')"  />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="0" checked="checked" onClick="enableSession(2,' + num + ')"  />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="drpMes' + num + '" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  readOnly="readOnly"   /></td></tr><tr><td>Required Score </td> '
    + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table> </div></div>';
        document.getElementById('newMoveDown').innerHTML = document.getElementById('newMoveDown').innerHTML + html;
        fillColAfterUpdate();
        numi.value = num;
    }
    else if (type == "3") {
        num = (parseInt(document.getElementById('divNo').value)) + 1;

        html = '<div id="Div' + num + '" ><div class="contentDivider" > <table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead3" >FOR MODIFICATION</div><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="3" runat="server" style="visibility:hidden;" size="1" /></td><td width="15%"> <input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /><input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(3)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><div id="divColSelect' + num + '"  name="divColSelect' + num + '" >' + strResult + '</div></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1" onClick="enableSession(1,' + num + ')"  />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="0" checked="checked" onClick="enableSession(2,' + num + ')"  />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="drpMes' + num + '" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input name="txtNoSes' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"   readOnly="readOnly"  /></td></tr><tr><td>Required Score </td> '
    + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table></div></div> ';
        document.getElementById('newMoveMod').innerHTML = document.getElementById('newMoveMod').innerHTML + html;
        fillColAfterUpdate();
        numi.value = num;
    }


    fillColAfterUpdate();
    divsNo = num;

}

function enableSession(type, Id) {
    var txtSess = "txtNoSes" + Id;
    var txt1 = "txtOutFirst" + Id;
    var txt2 = "txtOutSecup" + Id;

    var xs1 = document.getElementsByName(txtSess);    
    var xs2 = document.getElementsByName(txt1);
    var xs3 = document.getElementsByName(txt2);

    var xs4 = document.getElementsByName(txtSess);
    var xs5 = document.getElementsByName(txt1);
    var xs6 = document.getElementsByName(txt2);

    xs4.value = "";
    xs5.value = "";
    xs6.value = "";

    if (type == 1) {
        $('#' + txtSess).removeAttr('readonly');
        $('#' + txt1).attr('readonly', 'readonly');
        $('#' + txt2).attr('readonly', 'readonly');
    }
    else {

        $('#' + txtSess).attr('readonly', 'readonly');
        $('#' + txt1).removeAttr('readonly');
        $('#' + txt2).removeAttr('readonly');

    }

}


function addTextBox(type) {

    var ni = document.getElementById('content');
    var numi = document.getElementById('divNo');


    var num = 0;

    var tCount = 0;
    tCount = parseInt(document.getElementById('hidTotalCount').value) + 1;
    document.getElementById('hidTotalCount').value = tCount.toString();

    fillSuggest();

    if (type == "1" || type == "2" || type == "3") {
        type == "0";
    }



    if (type == "0") {

        num = num + 1;
        var newMoveUp = document.createElement('div');

        newMoveUp.setAttribute('id', "newMoveUp");
        newMoveUp.setAttribute('class', "contentDiv");

        newMoveUp.innerHTML = '<div id="Div' + num + '" ><div class="contentDivider" ><table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead1">TO MOVE UP</div></td><td width="15%"> <input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /><input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /> <input name="txtType' + num + '" id="txtType' + num + '" type="text" value="1" runat="server" size="1" style="visibility:hidden;"  /> </td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(1)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><select id="select" onchange="colChange(this,' + num + ');"  name="tempCol' + num + '" style="width: 250px">' + strResult + '</select></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1" onClick="enableSession(1,' + num + ')" />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="0" checked="checked" onClick="enableSession(2,' + num + ')"  />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="drpMes' + num + '" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10"  readOnly="readOnly"  onkeypress="return isNumberKey(event)"  /></td></tr><tr><td>Required Score </td> '
        + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table></div></div> ';

        ni.appendChild(newMoveUp);



        num = num + 1;
        var newMoveDown = document.createElement('div');
        newMoveDown.setAttribute('id', "newMoveDown");
        newMoveDown.setAttribute('class', "contentDiv");
        newMoveDown.innerHTML = '<div id="Div' + num + '" ><div class="contentDivider" ><table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead2" >TO MOVE DOWN</div></td><td width="15%"><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="2" runat="server" size="1" style="visibility:hidden;"  /> <input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /> <input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(2)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><select id="select" name="tempCol' + num + '" onchange="colChange(this,' + num + ');" style="width: 250px">' + strResult + '</select></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1"  onClick="enableSession(1,' + num + ')" />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio"  checked="checked" value="0" />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="drpMes' + num + '" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  readOnly="readOnly"   /></td></tr><tr><td>Required Score </td> '
    + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table> </div></div>';
        ni.appendChild(newMoveDown);




        num = num + 1;
        var newMoveMod = document.createElement('div');
        newMoveMod.setAttribute('id', "newMoveMod");
        newMoveMod.setAttribute('class', "contentDiv");

        newMoveMod.innerHTML = '<div id="Div' + num + '" ><div class="contentDivider" > <table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead3" >FOR MODIFICATION</div><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="3" runat="server" style="visibility:hidden;" size="1" /></td><td width="15%"> <input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /><input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(3)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><select id="select" name="tempCol' + num + '" style="width: 250px" onchange="colChange(this,' + num + ');" >' + strResult + '</select></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1"  onClick="enableSession(1,' + num + ')" />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="0" checked="checked" onClick="enableSession(2,' + num + ')" />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="drpMes' + num + '" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10"  readOnly="readOnly"  onkeypress="return isNumberKey(event)"  /></td></tr><tr><td>Required Score </td> '
    + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table></div></div> ';
        ni.appendChild(newMoveMod);


        numi.value = num;

    }
    else if (type == "1") {

        num = (parseInt(document.getElementById('divNo').value)) + 1;
        html = '<div id="Div' + num + '" ><div class="contentDivider" > <table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead1" >TO MOVE UP</div></td><td width="15%"><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="1" runat="server" size="1" style="visibility:hidden;"/><input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /> <input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(1)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><select id="select" name="tempCol' + num + '" style="width: 250px">' + strResult + '</select></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1"  onClick="enableSession(1,' + num + ')" />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" checked="checked" value="0" onClick="enableSession(2,' + num + ')"  />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="select2" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"   readOnly="readOnly"  /></td></tr><tr><td>Required Score </td> '
        + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table></div></div> ';

        document.getElementById('newMoveUp').innerHTML = document.getElementById('newMoveUp').innerHTML + html;

        numi.value = num;
    }
    else if (type == "2") {
        num = (parseInt(document.getElementById('divNo').value)) + 1;

        html = '<div id="Div' + num + '" ><div class="contentDivider" ><table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead2" >TO MOVE DOWN</div></td><td width="15%"><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="2" runat="server" size="1" style="visibility:hidden;"  /> <input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /> <input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(2)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><select id="select" name="tempCol' + num + '" onchange="colChange(this,' + num + ');" style="width: 250px">' + strResult + '</select></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1" onClick="enableSession(1,' + num + ')" />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="0"  checked="checked" onClick="enableSession(2,' + num + ')"  />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="drpMes' + num + '" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  readOnly="readOnly"   /></td></tr><tr><td>Required Score </td> '
    + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table> </div></div>';
        document.getElementById('newMoveDown').innerHTML = document.getElementById('newMoveDown').innerHTML + html;
        numi.value = num;
    }
    else if (type == "3") {
        num = (parseInt(document.getElementById('divNo').value)) + 1;

        html = '<div id="Div' + num + '" ><div class="contentDivider" > <table width="100%"  cellspacing="0" cellpadding="0"><tr><td colspan="2"><div class="contentDividerHead3" >FOR MODIFICATION</div><input name="txtType' + num + '" id="txtType' + num + '" type="text" value="3" runat="server" style="visibility:hidden;" size="1" /></td><td width="15%"> <input name="txtCol' + num + '" id="txtCol' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /><input name="txtCal' + num + '" id="txtCal' + num + '" type="text" value="0" size="1" style="visibility:hidden;"  /></td> <td width="25%"><table width="100%" border="0" cellspacing="0" cellpadding="0"><tr><td align="right" width="3%"></td><td  width="3%" align="right"><a href=\'#\' target=\'_self\' onClick=\'addTextBox(3)\'>Add</a> <a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr></table></td> </tr><tr><td width="15%">IOA Required</td><td width="25%"><input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoIOA" name="rdoIOA' + num + '" type="radio" value="0" />No</td><td width="15%">Multiteacher Required</td> <td width="25%"><input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="1" checked="checked" />Yes<input id="rdoMTeacher" name="rdoMTeacher' + num + '" type="radio" value="0" />No</td></tr><tr><td width="15%">Template Column </td><td width="25%"><select id="select" name="tempCol' + num + '" style="width: 250px" onchange="colChange(this,' + num + ');" >' + strResult + '</select></td><td width="15%">Consecutive Session</td> <td width="25%"><input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="1"  onClick="enableSession(1,' + num + ')" />Yes<input id="rdoConsecutiveup" name="rdoConsecutiveup' + num + '" type="radio" value="0" checked="checked" onClick="enableSession(2,' + num + ')"  />No</td></tr><tr><td>Measure</td><td><div id="divMeasureSelect' + num + '"><select id="drpMes' + num + '" name="drpMes' + num + '" style="width: 250px" onchange="calChange(this,' + num + ');"><option value="0">----------Select Measure-----------</option></select></div></td><td>Number Of Sessions</td><td><input Id="txtNoSes' + num + '" name="txtNoSes' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  readOnly="readOnly"   /></td></tr><tr><td>Required Score </td> '
    + '<td><input name="txtReqScore' + num + '" type="text" size="35" onkeypress="return isNumberKey(event)"  /></td><td>Instance</td><td><table width="93%" cellpadding="0" cellspacing="0" ><tr><td><input Id="txtOutFirst' + num + '" name="txtOutFirst' + num + '" type="text" size="10" /></td><td>Out Of </td><td><input Id="txtOutSecup' + num + '" name="txtOutSecup' + num + '" type="text" size="10" onkeypress="return isNumberKey(event)"  /></td></tr></table></td></tr></table></div></div> ';
        document.getElementById('newMoveMod').innerHTML = document.getElementById('newMoveMod').innerHTML + html;
        numi.value = num;
    }



}


function colChange(selectObj, divNo) {
    var txtColType = "txtCol" + divNo;
    var idx = selectObj.value;

    divsNo = divNo;
    // fillSuggest1(idx, divNo);
    document.getElementById(txtColType).value = idx;



    $.ajax({
        type: "POST",
        url: "TempSetCriteria.aspx/GetMeasureDrop",
        data: "{ID:" + idx + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            strResult1 = msg.d;
            // Do whatever with this cool array
        },
        error: function (msg) {
            alert(msg.status + " : " + msg.statusText);
        }
    });
    alert("Please Wait...");
    var divMes = "divMeasureSelect" + divsNo;
    document.getElementById(divMes).innerHTML = "<select id='drpMes" + divsNo + "' name='drpMes" + divsNo + "' onchange='calChange(this," + divsNo + ");' style='width: 250px'>" + strResult1 + "</select>";

}

function calChange(selectObj, divNo) {
    var txtColType = "txtCal" + divNo;
    var idx = selectObj.value;
    document.getElementById(txtColType).value = idx;
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
            newdiv.innerHTML = '<table style=\'width: 68%;\'><tr><td width=\'50%\'>Set Name</td><td><input type="text" size="30"  name="txtName' + num + '" >&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'addTextBox()\'>Add</a>&nbsp; &nbsp; &nbsp;  '
              + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Prompt Description </td><td><textarea   name=txtDesc' + num + ' cols=\'50\' rows=\'5\'></textarea></td></tr>'
              + '<tr><td colspan=\'2\'><div id=Match' + num + ' style=\'width:100%\'></div></td></tr>'
              + '<tr><td>Match to Sample </td><td><input type=\'text\' size="30"  ID=txtSample' + num + ' \>&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'getMatchToSample(' + num + ')\'>Add</a><input type=\'text\' style=\'visibility:hidden\' ID=txtMatch' + num + '  name=txtMatch' + num + ' \></td></tr>'
              + '</table>';
        }
        else {
            newdiv.innerHTML = '<table style=\'width: 68%;\'><tr><td width=\'50%\'>Set Name</td><td><input type="text" size="30"  name="txtName' + num + '"  >&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'addTextBox()\'>Add</a>&nbsp; &nbsp; &nbsp;  '
             + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Prompt Description </td><td><textarea   name=txtDesc' + num + ' cols=\'50\' rows=\'5\'></textarea></td></tr>'
             + '</table>';
        }
        ni.appendChild(newdiv);

    }



}


