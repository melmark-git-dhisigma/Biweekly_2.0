
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

function getMatchToSample(divNo) {

    var txt1 = "txtMatch" + divNo;

    var currentValue = document.getElementById("divMatch").value;
    var newValue = parseInt(parseFloat(currentValue)) + 1;
    document.getElementById("divMatch").value = newValue;

    var txt = "txtSample" + divNo;

    var txtVal = document.getElementById(txt).value;

    document.getElementById(txt1).value = document.getElementById(txt1).value + ',' + txtVal;




    document.getElementById(txt).value = "";
    var newElement = "<div class=\"matchToSampleText\" id=test" + newValue + " >" + txtVal + "<a id=\"close\" onClick=\"removeMatchText(" + newValue + ")\" href=\"#\"> "
    + "<img src='../Administration/images/popUpClose.ico' width=10 height=\"10\" border=\"0\" ></a></div>";

    var div = "Match" + divNo;
    var element = document.getElementById(div);
    var oldTest = element.innerHTML;


    var New = oldTest + newElement;
    element.innerHTML = New;

}

function addTextBox() {
        
    var isMatch = parseInt(document.getElementById('hidMathToSample').value);
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

    newdiv.innerHTML = '<table style=\'width: 100%;\'><tr><td width=\'50%\'>Step Name</td><td><input type="text" size="30" id="txtName' + num + '" name="txtName' + num + '"  CssClass="textClass" >&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'addTextBox()\'>Add</a>&nbsp; &nbsp; &nbsp;  '
         + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Set Description<input name="hidUpdate' + num + '"  id="" type="hidden" value="0" /> </td><td><textarea  name=txtDesc' + num + ' cols=\'50\' rows=\'5\'></textarea></td></tr>'
         + '</table>';
    
    ni.appendChild(newdiv);



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
            newdiv.innerHTML = '<table style=\'width: 68%;\'><tr><td width=\'50%\'>Set Name</td><td><input type="text" size="30" id="txtName' + num + '"  name="txtName' + num + '" >&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'addTextBox()\'>Add</a>&nbsp; &nbsp; &nbsp;  '
              + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Step Description </td><td><textarea   name=txtDesc' + num + ' cols=\'50\' rows=\'5\'></textarea></td></tr>'
              + '<tr><td colspan=\'2\'><div id=Match' + num + ' style=\'width:100%\'></div></td></tr>'
              + '<tr><td>Match to Sample </td><td><input type=\'text\' size="30"  ID=txtSample' + num + ' \>&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'getMatchToSample(' + num + ')\'>Add</a><input type=\'text\' style=\'visibility:hidden\' ID=txtMatch' + num + '  name=txtMatch' + num + ' \></td></tr>'
              + '</table>';
        }
        else {
            newdiv.innerHTML = '<table style=\'width: 68%;\'><tr><td width=\'50%\'>Set Name</td><td><input type="text" size="30" id="txtName' + num + '"  name="txtName' + num + '"  >&nbsp; &nbsp; <a href=\'#\' target=\'_self\' onClick=\'addTextBox()\'>Add</a>&nbsp; &nbsp; &nbsp;  '
             + '<a href=\'#\' target=\'_self\' onClick=\'decCount(' + num + ')\'>Remove</a></td></tr><tr><td>Step Description </td><td><textarea   name=txtDesc' + num + ' cols=\'50\' rows=\'5\'></textarea></td></tr>'
             + '</table>';
        }
        ni.appendChild(newdiv);

    }

}

function NotSelectSet() {

    alert("Please select a Set ");
    return false;
}


