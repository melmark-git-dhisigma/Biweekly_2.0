/////////////////////////////// COOKIES///////////////////////////////

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}






//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function listPrompt(parameter) {



    $.ajax(
			{

			    type: "POST",
			    url: "TemplateCreator.aspx/listPrompt",
			    data: "{'parameter':'" + parameter + "'}",
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async: false,
			    success: function (data) {

			        var combo = document.getElementById("lstPrompt");
			        combo.options.length = 0;
			        var list = jQuery.parseJSON(data.d);
			        $.each(list, function (index, value) {


			            var columnName = value.LookupName;
			            var columnValue = value.LookupValue;




			            var option = document.createElement("option");
			            option.text = columnName;
			            option.value = columnValue;
			            try {
			                combo.add(option, null); //Standard 
			            } catch (error) {
			                combo.add(option); // IE only
			            }

			        });


			    },
			    error: function (request, status, error) {
			        alert("Error");
			    }
			});



}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////// Show SubPrompt Values  //////////////////////////////////////////////////////////

function displaySubPropmpts(sel) {
    var parameter = sel.options[sel.selectedIndex].text;
    var parameter2 = sel.options[sel.selectedIndex].value;

    $.ajax(
			{

			    type: "POST",
			    url: "TemplateCreator.aspx/listSubPrompt",
			    data: "{'parameter':'" + parameter + "','parameter1':'" + parameter2 + "'}",
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async: false,
			    success: function (data) {

			        var combo = document.getElementById("lstSubPrompt");
			        combo.options.length = 0;
			        var list = jQuery.parseJSON(data.d);
			        $.each(list, function (index, value) {

			            var columnName = value.LookupName;

			            var option = document.createElement("option");
			            option.text = columnName;
			            option.value = columnName;
			            try {
			                combo.add(option, null); //Standard 
			            } catch (error) {
			                combo.add(option); // IE only
			            }

			        });


			    },
			    error: function (request, status, error) {
			        alert("Error");
			    }
			});
    var stile = document.getElementById("least-most").style;
    stile.display = "block";
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////Save Complete Template//////////////////////////////////////////////////////////


function SaveCompleteTemplate() {

    //            saveTemplate();
    //            saveSets();
    //            saveSteps();
    //savePrompts();
    // saveMeasure();

}


//var table = document.getElementById("tblSet");
//var currow = table.rows.length;
//if (currow < 3) {


//}
//else {
//    for (var index = 0; index < currow - 2; index++) {
//        setName[index] = table.rows[index + 1].cells[0].innerHTML;

//        setDesc[index] = table.rows[index + 1].cells[1].innerHTML;

//    }
//}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function validateEmpty(fld) {
    var error = "";

    var inputs = document.getElementsByTagName('input');
    var count = 0;
    for (var cpt = 0; cpt < inputs.length; cpt++)
        if (inputs[cpt].type == 'text') count++;
    alert(count);


    if (fld.value.length == 0) {
        fld.style.background = 'Yellow';
        error = "The required field has not been filled in.\n"
    } else {
        fld.style.background = 'White';
    }
    return error;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////// Function For Dynamically Rows For Set and Step /////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function addRowToTableStep() {
    var tbl = document.getElementById('tblStep');

    var lastRow = tbl.rows.length;

    // if there's no header row in the table, then iteration = lastRow + 1
    var iteration = lastRow;
    var row = tbl.insertRow(parseInt(lastRow) - 2);

    // left cell
    var cellLeft = row.insertCell(0);
    var txel = document.createElement('input');
    txel.type = 'text';
    txel.class = 'txtStyle';
    txel.id = "txtStepName" + iteration;
    //el.id = 'optRow' + iteration;
    txel.size = 25;
    cellLeft.appendChild(txel);

    var cellRight2 = row.insertCell(1);
    var txel2 = document.createElement('Textarea');
    txel2.type = 'Textarea';
    //el.id = 'optRow' + iteration;
    txel2.setAttribute('rows', '5');
    txel2.setAttribute('cols', '50');
    txel2.id = "txtStepDescription" + iteration;
    cellRight2.appendChild(txel2);

    var cellRight3 = row.insertCell(2);
    var btn2 = document.createElement('img');
    btn2.src = "images/Minus.png";
    btn2.id = iteration;

    var rwid = btn2.id;
    btn2.onclick = function () {
        $(this).closest('tr').remove();
    }

    cellRight3.appendChild(btn2);
}


function addRowToTableSet() {
    var tbl = document.getElementById('tblSet');

    var lastRow = tbl.rows.length;

    // if there's no header row in the table, then iteration = lastRow + 1
    var iteration = lastRow;
    var row = tbl.insertRow(parseInt(lastRow) - 2);

    // left cell
    var cellLeft = row.insertCell(0);
    var txel = document.createElement('input');
    txel.type = 'text';

    txel.id = "txtSetName" + iteration;
    //el.id = 'optRow' + iteration;
    txel.size = 25;
    cellLeft.appendChild(txel);


    var cellRight2 = row.insertCell(1);
    var txel2 = document.createElement('Textarea');
    txel2.type = 'Textarea';
    //el.id = 'optRow' + iteration;
    txel2.setAttribute('rows', '5');
    txel2.setAttribute('cols', '50');
    txel2.id = "txtSetDescription" + iteration;
    cellRight2.appendChild(txel2);

    var cellRight3 = row.insertCell(2);
    var btn2 = document.createElement('img');
    btn2.src = "images/Minus.png";
    btn2.id = iteration;
    var rwid = btn2.id;

    btn2.onclick = function () {
        $(this).closest('tr').remove();
    }

    cellRight3.appendChild(btn2);

}



function addRows() {

    addSets();
    addSteps();
    addRowToTable();
    column();



}

function column() {
    var num = document.getElementById('ddlColumNumber').selectedIndex;
    alert(num);
    var col1 = document.getElementById('column1').style;
    var col2 = document.getElementById('column2').style;
    var col3 = document.getElementById('column3').style;
    col1.display = "none";
    col2.display = "none";
    col3.display = "none";
    if (parseInt(num) == 1) {
        col1.display = "block";
    }
    else if (parseInt(num) == 2) {
        col1.display = "block";
        col2.display = "block";
    }
    else if (parseInt(num) == 3) {
        col1.display = "block";
        col2.display = "block";
        col3.display = "block";
    }

}


function addSets() {

    var loop = document.getElementById('txtNumberofSets').value;

    for (var index = 0; index < loop; index++) {




        var tbl = document.getElementById('tblSet');

        var lastRow = tbl.rows.length;

        // if there's no header row in the table, then iteration = lastRow + 1
        var iteration = lastRow;
        var row = tbl.insertRow(parseInt(lastRow) - 2);

        // left cell
        var cellLeft = row.insertCell(0);
        var txel = document.createElement('input');
        txel.type = 'text';
        txel.class = 'txtStyle';
        txel.id = "txtSetName" + iteration;
        //el.id = 'optRow' + iteration;
        txel.size = 25;
        cellLeft.appendChild(txel);


        var cellRight2 = row.insertCell(1);
        var txel2 = document.createElement('input');
        txel2.type = 'text';
        //el.id = 'optRow' + iteration;
        txel2.size = 25;
        txel2.class = 'txtStyle';
        txel2.id = "txtSetDescription" + iteration;
        cellRight2.appendChild(txel2);

        var cellRight3 = row.insertCell(2);
        var btn2 = document.createElement('img');
        btn2.src = "Minus.png";
        btn2.id = iteration;
        var rwid = btn2.id;

        btn2.onclick = function () {
            $(this).closest('tr').remove();
        }

        cellRight3.appendChild(btn2);



        //cellRight4.appendChild(btn2);

        //            var rownum = parseInt(lastRow - 1);
        //            fillcolumn(rownum);

    }
}


function addSteps() {

    var loop = document.getElementById('txtNumberofSteps').value;

    for (var index = 0; index < loop; index++) {




        var tbl = document.getElementById('tblStep');

        var lastRow = tbl.rows.length;

        // if there's no header row in the table, then iteration = lastRow + 1
        var iteration = lastRow;
        var row = tbl.insertRow(parseInt(lastRow) - 2);

        // left cell
        var cellLeft = row.insertCell(0);
        var txel = document.createElement('input');
        txel.type = 'text';
        txel.class = 'txtStyle';
        txel.id = "txtSetName" + iteration;
        //el.id = 'optRow' + iteration;
        txel.size = 25;
        cellLeft.appendChild(txel);


        var cellRight2 = row.insertCell(1);
        var txel2 = document.createElement('input');
        txel2.type = 'text';
        //el.id = 'optRow' + iteration;
        txel2.size = 25;
        txel2.class = 'txtStyle';
        txel2.id = "txtSetDescription" + iteration;
        cellRight2.appendChild(txel2);

        var cellRight3 = row.insertCell(2);
        var btn2 = document.createElement('img');
        btn2.src = "Minus.png";
        btn2.id = iteration;

        var rwid = btn2.id;
        btn2.onclick = function () {
            $(this).closest('tr').remove();
        }

        cellRight3.appendChild(btn2);



        //cellRight4.appendChild(btn2);

        //            var rownum = parseInt(lastRow - 1);
        //            fillcolumn(rownum);

    }
}





////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////// Function For Dynamically Add Columns ///////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var scoreValues = ["Select", "Text Box", "Drop Down"];


function addRowToTable() {
    var loop = document.getElementById('txtnumberodColumns').value;

    for (var index = 0; index < loop; index++) {

        var tbl = document.getElementById('tableColumn');

        var lastRow = tbl.rows.length;

        // if there's no header row in the table, then iteration = lastRow + 1
        var iteration = lastRow;
        var row = tbl.insertRow(parseInt(lastRow) - 1);

        // left cell
        var cellLeft = row.insertCell(0);
        var txel = document.createElement('input');
        txel.type = 'text';
        txel.class = "txttblStyle";
        txel.id = "txtColumnName" + iteration;
        //el.id = 'optRow' + iteration;
        txel.size = 25;
        cellLeft.appendChild(txel);


        var cellRight = row.insertCell(1);
        var el = document.createElement('select');
        el.type = 'select';
        el.name = 'select' + iteration;
        el.id = 'optRow' + iteration;
        el.id.enable = false;
        el.class = "drpClass";
        el.size = 1;
        el.width = 177;
        for (var key in scoreValues) {


            var option = document.createElement("option");
            option.text = scoreValues[key];
            option.id = scoreValues[key].id;


            //option.value = key.value;
            try {

                el.add(option, null);
                el.enable = false;  //Standard 
            } catch (error) {

                el.add(option); // IE only
            }
        }
        cellRight.appendChild(el);




        var cellRight2 = row.insertCell(2);
        var txel2 = document.createElement('input');
        txel2.type = 'text';
        //el.id = 'optRow' + iteration;
        txel2.size = 25;
        txel2.class = "txttblStyle";
        txel2.id = "txtColumnValues" + iteration;
        cellRight2.appendChild(txel2);




        var cellRight3 = row.insertCell(3);
        var txel3 = document.createElement('input');
        txel3.type = 'text';
        //el.id = 'optRow' + iteration;
        txel3.size = 25;
        txel3.id = "txtColumnDesc" + iteration;
        txel3.class = "txttblStyle";
        cellRight3.appendChild(txel3);

        var cellRight4 = row.insertCell(4);

        var btn2 = document.createElement('img');
        btn2.src = "Slider/Minus.png";
        btn2.id = iteration;
        var rwid = btn2.id;
        btn2.onclick = function () {
            $(this).closest('tr').remove();
        }




        cellRight4.appendChild(btn2);

        var rownum = parseInt(lastRow - 1);
        //fillcolumn(rownum);

    }

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////// Function To Read the Set Details ///////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




function readSetDetails() {


    var setNameRD = new Array();
    var setDescRD = new Array();

    var table = document.getElementById("tblSet");
    var currow = table.rows.length;

    if (currow < 3) {


    }
    else {
        for (var index = 0; index < currow - 3; index++) {

            var curid = parseInt(index + 3);
            setNameRD[index] = document.getElementById("txtSetName" + curid).value;
        }

        setCookie('measure', setNameRD, -1);
        setCookie('measure', setNameRD, 1);

    }

    var elemnt = document.getElementById('ddlMeasure');
    var cookis = getCookie('measure');
    var measureValues = cookis.split(',');
    for (var index = 0; index < measureValues.length; index++) {
        var opt = document.createElement("option");
        opt.text = measureValues[index];
        // opt.value = index;
        // Add an Option object to Drop Down/List Box
        elemnt.options.add(opt);
        // Assign text and value to Option object



    }




}







////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////// Function To Read the Step details///////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var stepName = new Array();
var stepDesc = new Array();

function readStepDetails() {



    var table = document.getElementById("tblStep");
    var currow = table.rows.length;
    if (currow < 3) {


    }
    else {
        for (var index = 0; index < currow - 2; index++) {
            stepName[index] = table.rows[index + 1].cells[0].innerHTML;

            stepDesc[index] = table.rows[index + 1].cells[1].innerHTML;

        }
    }


    //    $.ajax(
    //			{

    //			    type: "POST",
    //			    url: "AddStepsAndRules.aspx/SaveStep",
    //			    data: "{'parameter1':'" + stepName + "','parameter2':'" + stepDesc + "'}",
    //			    contentType: "application/json; charset=utf-8",
    //			    dataType: "json",
    //			    async: true,
    //			    cache: false,
    //			    success: function (result) {
    //			        alert(result);
    //			    },
    //			    error: function (request, status, error) {
    //			        alert("Error");
    //			    }
    //			});


}


////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////                                         Save Template Details                                     ////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

function saveTemplate() {

    var argueArray = new Array();
    var SkillType = "";
    var chainType = "";
    var CompuAided = "N";
    var numberOfTrails = 0;
    var status = false;

    //argueArray[2] = document.getElementById('txtNumOfSets').value;
    //argueArray[3] = document.getElementById('txtNumOfSteps').value;
    if (document.getElementById("radSkillDisc").checked == true) {
        SkillType = "Discrete";
        numberOfTrails = document.getElementById('txtDescrete').value;

        document.getElementById("radChainFrwd").checked = false;
        document.getElementById("radChainBckwd").checked = false;
        document.getElementById("radChainTotal").checked = false;
        chainType = "No Chain Type";


    }
    else if (document.getElementById("radSkillChained").checked == true) {
        SkillType = "Chained";

    }
    if (document.getElementById("radChainFrwd").checked == true) {
        chainType = "Forward Chain";
    }
    if (document.getElementById("radChainBckwd").checked == true) {
        chainType = "Backward Chain";
    }
    if (document.getElementById("radChainTotal").checked == true) {
        chainType = "Total Task";
    }

    if (document.getElementById("chkVisual").checked == true) {
        CompuAided = "Y";
    }

    argueArray[0] = document.getElementById('txtTemplateName').value;
    argueArray[1] = document.getElementById('txtTempDesc').value;
    argueArray[2] = SkillType;
    argueArray[3] = numberOfTrails;
    argueArray[4] = chainType;
    argueArray[5] = CompuAided;


    $.ajax(
			{

			    type: "POST",
			    url: "TemplateCreator.aspx/SaveTemplate",
			    data: "{'parameter1':'" + argueArray + "'}",
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async: true,
			    cache: false,
			    success: function (result) {
			        //alert("Saved Sucessfully");
			        status = true;
			    },
			    error: function (request, status, error) {

			    }
			});
    //return status;
}





///////////////////////////////////////////////////////////////////////////////////////////////////////////



//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE SETS                                               //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////



function saveSets() {

    var setName = new Array();
    var setDesc = new Array();
    var status = false;

    var status = false;
    var table = document.getElementById("tblSet");
    var currow = table.rows.length;
    alert(currow);

    for (var index = 0; index < currow - 3; index++) {

        var curid = parseInt(index + 3);
        setName[index] = document.getElementById("txtSetName" + curid).value;

        setDesc[index] = document.getElementById("txtSetDescription" + curid).value;



    }
    $.ajax(
			{

			    type: "POST",
			    url: "TemplateCreator.aspx/SaveTemplateSets",
			    data: "{'parameter1':'" + setName + "','parameter2':'" + setDesc + "'}",
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async: true,
			    cache: false,
			    success: function (result) {


			    },
			    error: function (request, status, error) {

			    }
			});
    //return status;

}


//////////////////////////////////////////////////////////////////////////////////////////////////////////




//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE STEPS                                              //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////



function saveSteps() {

    var stepName = new Array();
    var stepDesc = new Array();
    var status = false;

    var table = document.getElementById("tblStep");
    var currow = table.rows.length;
    alert(currow);




    for (var index = 0; index < currow - 3; index++) {

        var curid = parseInt(index + 3);
        stepName[index] = document.getElementById("txtStepName" + curid).value;
        alert(stepName[index]);
        stepDesc[index] = document.getElementById("txtStepDescription" + curid).value;
        alert(stepDesc[index]);

    }
    $.ajax(
			{

			    type: "POST",
			    url: "TemplateCreator.aspx/SaveTemplateSteps",
			    data: "{'parameter1':'" + stepName + "','parameter2':'" + stepDesc + "'}",
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async: true,
			    cache: false,
			    success: function (result) {


			    },
			    error: function (request, status, error) {

			    }
			});
    return status;

}


//////////////////////////////////////////////////////////////////////////////////////////////////////////




//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE MEASURE                                            //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
debugger;
function saveMeasure() {
    MeasureOne();
    MeasureTwo();
    MeasureThree();
    //var obj = { "key": ["value1", "value2", "value3"], "key2": ["value1", "value2"] }
    //var ary = {};
    //    ary.push = measure1;
    //    ary= measure2;
    //    ary["third"] = measure3;
    //    ary.push(measure1);
    //    ary.push(measure2);
    //    ary.push(measure3);


    $.ajax(
            			{

            			    type: "POST",
            			    url: "TemplateCreator.aspx/SaveMeasures",
            			    data: "{'parameter1':'" + measure1 + "','parameter2':'" + measure2 + "','parameter3':'" + measure3 + "'}",
            			    //data: "{'parameter1':'" + obj + "'}",
            			    contentType: "application/json; charset=utf-8",
            			    dataType: "json",
            			    async: true,
            			    cache: false,
            			    success: function (result) {


            			    },
            			    error: function (request, status, error) {

            			    }
            			});
    return status;


}



//////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE MEASURE ONE                                        //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
var measure1 = new Array();
function MeasureOne() {



    measure1[0] = document.getElementById('txtMeasure1').value;

    var ddl1 = document.getElementById('ddlMeasure1');
    measure1[1] = ddl1.options[ddl1.selectedIndex].value;

    measure1[2] = document.getElementById('txtCorrectresp1').value;


    measure1[3] = document.getElementById('txtARMeasure1').value;


    measure1[4] = document.getElementById('txtARMeasure11').value;


    //alert(moveUpPrompt);


}
//////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE MEASURE TWO                                        //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
var measure2 = new Array();

function MeasureTwo() {



    measure2[0] = document.getElementById('txtMeasure2').value;

    var ddl2 = document.getElementById('ddlMeasure2');
    measure2[1] = ddl2.options[ddl2.selectedIndex].value;

    measure2[2] = document.getElementById('txtCorrectresp2').value;


    measure2[3] = document.getElementById('txtARMeasure2').value;


    measure2[4] = document.getElementById('txtARMeasure12').value;


    //alert(moveUpPrompt);


}
//////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE MEASURE THRE                                       //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
var measure3 = new Array();
function MeasureThree() {



    measure3[0] = document.getElementById('txtMeasure3').value;

    var ddl3 = document.getElementById('ddlMeasure3');
    measure3[1] = ddl3.options[ddl3.selectedIndex].value;

    measure3[2] = document.getElementById('txtCorrectresp3').value;


    measure3[3] = document.getElementById('txtARMeasure3').value;


    measure3[4] = document.getElementById('txtARMeasure13').value;


    //alert(moveUpPrompt);


}
//////////////////////////////////////////////////////////////////////////////////////////////////////////



//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE PROMPTS                                            //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////



function savePrompts() {
    var tblPrmtMoveUP = document.getElementById("1");

    var tblPrmtMoveDown = document.getElementById("2");
    var tblPrmtModification = document.getElementById("3");

    PromptMovUp(tblPrmtMoveUP);
    PromptMovDwn(tblPrmtMoveDown);
    PromptModi(tblPrmtModification);


    $.ajax(
            			{

            			    type: "POST",
            			    url: "TemplateCreator.aspx/SavePromtRules",
            			    data: "{'parameter1':'" + moveUpPrompt + "','parameter2':'" + moveDownPrompt + "','parameter3':'" + modiPrompt + "'}",
            			    contentType: "application/json; charset=utf-8",
            			    dataType: "json",
            			    async: true,
            			    cache: false,
            			    success: function (result) {


            			    },
            			    error: function (request, status, error) {

            			    }
            			});
    return status;


}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////



//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE PROMPT MOVE UP                                     //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
var moveUpPrompt = new Array();
function PromptMovUp(tableId) {

    var currow = tableId.rows.length;
    if (document.getElementById('chkTchrPrompt').checked == true) {
        moveUpPrompt[0] = "Y";
    }
    else {
        moveUpPrompt[0] = "N";
    }
    if (document.getElementById('chkIOAPrompt').checked == true) {
        moveUpPrompt[1] = "Y";
    }
    else {
        moveUpPrompt[1] = "N";
    }

    var measure = document.getElementById('ddlMeasureUp');
    moveUpPrompt[2] = measure.options[measure.selectedIndex].value;

    var calcRule = document.getElementById('ddlCalcRuleUp');
    moveUpPrompt[3] = calcRule.options[calcRule.selectedIndex].value;

    moveUpPrompt[4] = document.getElementById('txtReqScoreUp').value;

    moveUpPrompt[5] = radPromptUp;

    moveUpPrompt[6] = document.getElementById('txtNumOfSessionUp').value;

    var ddl1 = document.getElementById('ddlUp1');
    moveUpPrompt[7] = ddl1.options[ddl1.selectedIndex].value;

    var ddl2 = document.getElementById('ddlUp2');
    moveUpPrompt[8] = ddl2.options[ddl2.selectedIndex].value;
    //alert(moveUpPrompt);


}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE PROMPT MOVE DOWN                                     //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
var moveDownPrompt = new Array();
function PromptMovDwn(tableId) {

    var currow = tableId.rows.length;
    if (document.getElementById('chkTchrPromptModi').checked == true) {
        moveDownPrompt[0] = "Y";
    }
    else {
        moveDownPrompt[0] = "N";
    }
    if (document.getElementById('chkIOAPromptModi').checked == true) {
        moveDownPrompt[1] = "Y";
    }
    else {
        moveDownPrompt[1] = "N";
    }

    var measure = document.getElementById('ddlMeasureModi');
    moveDownPrompt[2] = measure.options[measure.selectedIndex].value;

    var calcRule = document.getElementById('ddlCalcRuleModi');
    moveDownPrompt[3] = calcRule.options[calcRule.selectedIndex].value;

    moveDownPrompt[4] = document.getElementById('txtReqScoreModi').value;

    moveDownPrompt[5] = radPromptDwn;

    moveDownPrompt[6] = document.getElementById('txtNumOfSessionModi').value;

    var ddl1 = document.getElementById('ddlModi1');
    moveDownPrompt[7] = ddl1.options[ddl1.selectedIndex].value;

    var ddl2 = document.getElementById('ddlModi2');
    moveDownPrompt[8] = ddl2.options[ddl2.selectedIndex].value;


    //alert(moveDownPrompt);
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////




//////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////              SAVE PROMPT MODIFICATION                                //////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////
var modiPrompt = new Array();
function PromptModi(tableId) {

    var currow = tableId.rows.length;
    if (document.getElementById('chkTchrPromptDwn').checked == true) {
        modiPrompt[0] = "Y";
    }
    else {
        modiPrompt[0] = "N";
    }
    if (document.getElementById('chkIOAPromptDwn').checked == true) {
        modiPrompt[1] = "Y";
    }
    else {
        modiPrompt[1] = "N";
    }

    var measure = document.getElementById('ddlMeasureDwn');
    modiPrompt[2] = measure.options[measure.selectedIndex].value;

    var calcRule = document.getElementById('ddlCalcRuleDwn');
    modiPrompt[3] = calcRule.options[calcRule.selectedIndex].value;

    modiPrompt[4] = document.getElementById('txtReqScoreDwn').value;

    modiPrompt[5] = radPromptModi;

    modiPrompt[6] = document.getElementById('txtNumOfSessionDwn').value;

    var ddl1 = document.getElementById('ddlDwn1');
    modiPrompt[7] = ddl1.options[ddl1.selectedIndex].value;

    var ddl2 = document.getElementById('ddlDwn2');
    modiPrompt[8] = ddl2.options[ddl2.selectedIndex].value;


    //alert(modiPrompt);
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////






var radPromptUp;
var radPromptDwn;
var radPromptModi;

function checkes(id) {

    var radio = document.getElementById(id).value;

    if (radio == "V1") {

        var radDisc = document.getElementById("radDiscrete").style;

        radDisc.display = "block";

        var radStyle = document.getElementById("radSkillId").style;

        radStyle.display = "none";

        var table = document.getElementById('tblStepDtl').style;

        table.display = "none";

        var btn1 = document.getElementById('btnTemp3Next').style;

        btn1.display = "none";

        var btn2 = document.getElementById('btnTemp3Next2').style;

        btn2.display = "block";

        var td = document.getElementById('tdMessage').style;

        td.display = "block";
        
    }
    else if (radio == "V2") {

        var radDisc = document.getElementById("radDiscrete").style;

        radDisc.display = "none";

        var radStyle = document.getElementById("radSkillId").style;

        radStyle.display = "block";

        var table = document.getElementById('tblStepDtl').style;

        table.display = "block";

        var btn1 = document.getElementById('btnTemp3Next').style;

        btn1.display = "block";

        var btn2 = document.getElementById('btnTemp3Next2').style;

        btn2.display = "none";

        var td = document.getElementById('tdMessage').style;

        td.display = "none";
    }
    else if (radio == "V4") {

        radPromptUp = "Y";
    }
    else if (radio == "V5") {

        radPromptUp = "N";
    }
    else if (radio == "V100") {

        radPromptDwn = "Y";
    }
    else if (radio == "V101") {

        radPromptDwn = "N";
    }
    else if (radio == "V102") {

        radPromptModi = "Y";
    }
    else if (radio == "V103") {

        radPromptModi = "N";
    }


    else if (radio == "V110") {
        var styl1 = document.getElementById("txtCorrectresp1").style;
        styl1.display = "none";
    }
    else if (radio == "V111") {
        var styl1 = document.getElementById("txtCorrectresp1").style;
        styl1.display = "block";
    }

    else if (radio == "V112") {
        var styl2 = document.getElementById("txtCorrectresp2").style;
        styl2.display = "none";
    }
    else if (radio == "V113") {
        var styl2 = document.getElementById("txtCorrectresp2").style;
        styl2.display = "block";
    }

    else if (radio == "V114") {
        var styl3 = document.getElementById("txtCorrectresp3").style;
        styl3.display = "none";
    }
    else if (radio == "V115") {
        var styl3 = document.getElementById("txtCorrectresp3").style;
        styl3.display = "block";
    }

}

