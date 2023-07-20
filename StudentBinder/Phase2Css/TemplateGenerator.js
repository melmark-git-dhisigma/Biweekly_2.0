var flagclocks = 0;
var flagstops = 0;
var stoptimes = 0;
var splitcounter = 0;
var currenttimes;
var splitdate = '';
var output = '';
var clock;
var argIndex = 0;
var permissionStatus = "";


function counterIn(starttime, name) {
    //var id = name + '-' + argIndex;
    clock = document.getElementById(name);
    currenttimes = new Date();
    var timediff = currenttimes.getTime() - starttime;
    if (flagstops == 1) {
        timediff = timediff + stoptimes
    }
    if (flagclocks == 1) {
        clock.value = formattime(timediff, '');
        refresh = setTimeout(counterIn(starttime, name), 10);
    }
    else {
        window.clearTimeout(refresh);
        stoptimes = timediff;
    }
}

function formattime(rawtime, roundtype) {
    if (roundtype == 'round') {
        var ds = Math.round(rawtime / 100) + '';
    }
    else {
        var ds = Math.floor(rawtime / 100) + '';
    }
    var sec = Math.floor(rawtime / 1000);
    var min = Math.floor(rawtime / 60000);
    ds = ds.charAt(ds.length - 1);
    if (min >= 60) {
        startstop();
    }
    sec = sec - 60 * min + '';
    if (sec.charAt(sec.length - 2) != '') {
        sec = sec.charAt(sec.length - 2) + sec.charAt(sec.length - 1);
    }
    else {
        sec = 0 + sec.charAt(sec.length - 1);
    }
    min = min + '';
    if (min.charAt(min.length - 2) != '') {
        min = min.charAt(min.length - 2) + min.charAt(min.length - 1);
    }
    else {
        min = 0 + min.charAt(min.length - 1);
    }
    return min + ':' + sec + ':' + ds;
}

function resetclock(name) {
    flagstops = 0;
    stoptimes = 0;

    window.clearTimeout(refresh);
    //output.value = "";
    splitcounter = 0;
    if (flagclocks == 1) {
        var resetdate = new Date();
        var resettime = resetdate.getTime();
        counterIn(resettime, name);
    }
    else {
        //clock.value = "00:00:0";
    }
}

var link = null;
var startTime = 0;
var Trials = 1;
var arColumnId = new Array();
var arColumnName = new Array();
var arColumnType = new Array();
var arColumCorrRespDesc = new Array();
var arColumnCorrectResp = new Array();
var arStepId = new Array();
var arStepName = new Array();
var arStepCd = new Array();
var arPromptName = new Array();
var arPromptId = new Array();
var arUserId = new Array();
var arUserName = new Array();
var arSetId = new Array();
var arSetName = new Array();
var arMistrial = new Array();
var arScoreAccuracy = new Array();
var arScoreIndependant = new Array();
var arScorePrompt = new Array();
var arScoreTotDuration = new Array();
var arScoreAvgDuration = new Array();
var arScoreFreq = new Array();
var bIOAStatus = false;
var vLesPlanName = "";
var vNextSetId = 0;
var vNextSetName = "";
var vNextStepName = "";
var vSkillType = "";
var vColCount = 0;
var vPromptCount = 0;
var vStepCount = 0;
var vActSession = 0;
var vSessionNumber = 0;
var vCurrentPrompt = "";
var iCurrentPrmtId = 0;
var sesID = 0;
var vMaterials = "";
var sTeachingProc = "";
var vTempStatus = 0;
var SaveTp = "Save";
var IncMistrail = false;


var isVisualTool = "";
var VTLessonId = "";
var crntset = 0;
var crntstep = 0;
var execute = false;


// AutoSave Functions...
$(document).ready(function () {
    // Configure to save every 60 seconds
    //window.setInterval(function () { autoSave() }, 300000);
});
//this function calls automatically after every 60 secs
function autoSave() {

    var btnsubmit = document.getElementById('btnSubmit');   //
    var btnsave = document.getElementById('btnSave');       //
    if ((btnsave != null) && (btnsubmit != null)) {         //both the save and submit button must be disabled before the save function called
        btnsave.setAttribute("disabled", true);             //
        btnsubmit.setAttribute("disabled", true);           //

        btnsave.value = "   Saving.....  ";

        saveSessionValues("Save");
        btnsave.removeAttribute("disabled");
        btnsubmit.removeAttribute("disabled");
        btnsave.value = "Save Scores";
    }
}

function generateTemplate() {

    $.ajax(
         {
             type: "POST",
             url: "TemplateFrame.aspx/TemplateData",
             data: "{'iTempStatus':'" + vTempStatus + "'}",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             async: false,
             success: function (data) {
                 var list = jQuery.parseJSON(data.d);
                 var Th = "";
                 $.each(list, function (index, value) {
                     vLesPlanName = value.LessonPlanName;
                     vNextSetId = value.NextSetId;
                     IncMistrail = value.ColumIncMistrial;
                     vNextSetName = value.NextSetName;
                     vSkillType = value.skillType;
                     vColCount = value.ColCount;
                     vPromptCount = value.PromptCount;
                     vStepCount = value.StepCount;
                     vActSession = value.activeSession;

                     vMaterials = value.sMaterials;
                     bIOAStatus = value.bIOAStatus;
                     sTeachingProc = value.steachingProc;
                     document.getElementById('lpIdTxt').innerHTML = value.LessonPlanName + "&nbsp&nbsp&nbsp Session Number : " + value.iSessNumber;
                     //  document.getElementById('txtSessNbr').innerHTML = value.iSessNumber;
                     document.getElementById('txtStartTime').innerHTML = value.sStartTime;
                     vSessionNumber = value.iSessNumber;
                     var statusMesg = value.sStatusMessage;
                     
                     if (statusMesg == "COMPLETED") {
                         $('#div_StatusInd').css("display", "block");
                     }
                     else {
                         $('#div_StatusInd').css("display", "none");
                     }
                     vCurrentPrompt = value.PromptName;
                     document.getElementById('txtCurrentPrompt').value = value.PromptName;
                     document.getElementById('txtCurrentPrompt').name = value.NextPromptId;
                     iCurrentPrmtId = value.NextPromptId;
                     vNextStepName = value.NextStepName;

                     isVisualTool = value.isVisualTool;
                     VTLessonId = value.VTLessonId;
                     crntstep = value.crntStep;
                     crntset = value.crntSet;

                     for (var iColumn in value.Columns) {
                         arColumnName[iColumn] = value.Columns[iColumn].ColumnName;
                         arColumnId[iColumn] = value.Columns[iColumn].ColumnId;
                         arColumnType[iColumn] = value.Columns[iColumn].ColumnType;
                         arColumCorrRespDesc[iColumn] = value.Columns[iColumn].ColumCorrectRespDesc;
                         Th = Th + '<th style="text-align:center;"></th>';
                     }
                     for (var iColumn in value.Prompts) {
                         arPromptId[iColumn] = value.Prompts[iColumn].promptId;
                         arPromptName[iColumn] = value.Prompts[iColumn].promptName;
                     }
                     for (var iColumn in value.Steps) {
                         arStepId[iColumn] = value.Steps[iColumn].stepId;
                         arStepName[iColumn] = value.Steps[iColumn].stepName;
                         arStepCd[iColumn] = value.Steps[iColumn].stepCd;
                     }

                     for (var iColumn in value.User) {
                         arUserId[iColumn] = value.User[iColumn].UserId;
                         arUserName[iColumn] = value.User[iColumn].UserName;
                     }

                     for (var iColumn in value.Set) {
                         arSetId[iColumn] = value.Set[iColumn].SetId;
                         arSetName[iColumn] = value.Set[iColumn].SetName;
                     }

                     for (var iColumn in value.Mistrial) {
                         arMistrial[iColumn] = value.Mistrial[iColumn].iMistrialStatus;
                     }
                 });
                 if (vNextStepName != "") {
                     var stl1 = document.getElementById('hideStep1').style;
                     stl1.visibility = "visible";
                     var stl2 = document.getElementById('hideStep2').style;
                     stl2.visibility = "visible";
                     var stl3 = document.getElementById('hideStep3').style;
                     stl3.visibility = "visible";
                     document.getElementById('nextStepTxt').value = vNextStepName;
                 }
                 var txtSetName = document.getElementById('nextSetTxt');
                 txtSetName.value = vNextSetName;
                 deleteRows();
                 $('#textcontent').remove();
                 $('#extraNotes').remove();
                 $('#btnDaveSubmit').remove();
                 var preTh = '<table id="tblDiscrete" style="width: 96%; line-height: 3;border: 1px solid #000000;margin: 0 0 0 20px; overflow: scroll;background-color:white;">' +
                     '<thead><tr class="HeaderStyle"><th style="text-align:center;">No</th><th style="text-align:center;width:250px;">Step Name</th>';
                 if (IncMistrail == true) {
                     preTh = preTh + '<th>Mis-Trials</th>';
                 }
                 var postTh = '<th style="text-align:center;">Notes</th></tr></thead><tbody></tbody></table>';
                 $('#discrete').append(preTh + Th + postTh);
                 createDiscreteTemplate();

                 createScoreSheet();
                 $("#tblDiscrete tr:odd").css("background-color", "#F3F3F3");
                 //$("#tblDiscrete tr:even").css("background-color", "black");
                 $("#tblDiscrete").css("border-bottom", "1px solid #000000");
                 $('#discrete').append('<div id="textcontent" style="float:left; width:40%;padding-top:10px; background-color: white; padding-bottom: 10px; margin: 10px 20px;border:1px double">' +
                     '<table style="width:100%;"><tr><td style="vertical-align:middle; width:50px;padding-left:10px;">' +
                     'Notes</td><td><textarea id="txtAreaComments" type="Textarea"' +
                     ' style="resize:none;height:78px;"></textarea></td></tr></table></div></div>');
                 $('#discrete').append('<div id="extraNotes" class="extraNotesCss"><table class="auto-style60" style="width:50%;height:auto;' +
                     '"><tr><td id="instructionType" onclick="showNotify(this.id);" class="auto-style61" rowspan="2" style="vertical-align: top;width: 50%;' +
                     'padding:10px;cursor:pointer;"></td><td id="materials" onclick="showNotify(this.id);" class="auto-style62" style="vertical-align: top;padding:10px;cursor:pointer;"></td>' +
                     '</tr><tr><td id="responseDef" onclick="showNotify(this.id);" style="vertical-align: top;padding:10px;cursor:pointer;"></td></tr></table></div>');
                 if (permissionStatus == "true") {
                     $('#discrete').append('<div id="btnDaveSubmit" style="width: auto; padding-right: 5%;display:block;"><input type="button" name="Save" id="btnSave" value="Save Scores" ' +
                         'class="btn2 btn-green-dark" style="float:right; margin:15px" onclick="saveSessionValues(this.name);" />' +
                         '<input type="button" name="Submit" id="btnSubmit" value="Submit Scores"  class="btn2 btn-green-dark" style="float:right; margin:15px"' +
                         ' onclick="saveSessionValues(this.name);" /></div>');
                 }
                 //saveSessionValues('Save');
                 //getTemplatesDatas(document.getElementById('txtSessNbr').value);
                 document.getElementById('materials').innerHTML = "<h3>MATERIALS </h3> </br>" + vMaterials;
                 document.getElementById('instructionType').innerHTML = "<h3>TYPE OF INSTRUCTION</h3></br>" + sTeachingProc + "</br>";
                 for (var index = 0; index < arPromptName.length; index++) {
                     $('#instructionType').append('*&nbsp' + arPromptName[index] + '</br>');
                 }
                 document.getElementById('responseDef').innerHTML = "<h3>RESPONSE DEFINITION</h3></br>";
                 for (var index = 0; index < arColumnId.length; index++) {
                     $('#responseDef').append('Correct Response of&nbsp' + arColumnName[index] + '&nbspis&nbsp:' + arColumnCorrectResp +
                         '&nbsp,&nbsp' + arColumCorrRespDesc[index] + '</br>');
                 }
                 var d = new Date();
                 d.setDate(new Date().getDate());
                 startTime = d.toTimeString().substr(0, d.toTimeString().lastIndexOf(" "));
                 //var qrStr = window.location.search;
                 //var spQrStr = qrStr.substring(1);
                 //var arrQrStr = new Array();
                 //var arr = spQrStr.split('&');
                 //for (var i = 0; i < arr.length; i++) {
                 //    var queryvalue = arr[i].split('=');
                 //    alert("Name: " + queryvalue[0] + " Value: " + queryvalue[1] + "<br/>");
                 //}
                 //fillUsers();


                 // if condion true,Populate values form history page.
                 if (sesID > 0) {
                     //getTemplatesDatas(sesID);
                 }
                 else {
                     // if condion true,Populate values form Active/Draft session.
                     if (vActSession > 0) {

                         //getTemplatesDatas(vActSession);
                     }
                 }

                 checkScoreSheet();
                 fn_calculateTotalTime();

                 if (execute == true) {
                     //window.open('../VisualTool/TeachPage_Redirecting.aspx?crntSet=' + crntset + '&crntStep=' + crntstep + '', '_blank');
                     //saveSessionValues("Save");
                     window.open('../VisualTool/TeachPage_Redirecting.aspx', '_blank');
                 }
             },
             error: function (request, status, error) {
                 alert("Error");
             }
         });
    //if (sesID == 0) {
    //    IOAEvaluate();
    //}

    // setInterval(TriggerChanges(), 50000);
}

var ajxAutoSaveTimer = null;

function TriggerChanges() {
    //if(ajxAutoSaveTimer)
    //    clearTimeout(ajxAutoSaveTimer);

    //ajxAutoSaveTimer = setTimeout(MakeAutoSave, 20000);

    alert("Timer");
}

function MakeAutoSave() {
    alert("Timer2");
    setTimeout(TriggerChanges(), 5000);
}

function IOAandNormalPopUp() {

    $('#overlay').hide('slow', function () {
        $('#dialogExecute').hide('slow');
    });
    $('#overlay').fadeIn('slow', function () {
        $('#dialog1').fadeIn('slow');
    });
    $.ajax(
  {
      type: "POST",
      url: "TemplateFrame.aspx/GetIOAandNormalSession",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      async: false,
      success: function (data) {
          var dataList = jQuery.parseJSON(data.d);
          $.each(dataList, function (index, value) {
              document.getElementById('IOArad').value = value.ISesHdrId;
              document.getElementById('lblIOA').innerHTML = value.IUser;
              document.getElementById('lblIOAStime').innerHTML = value.IStartTs;
              document.getElementById('lblIOAEtime').innerHTML = value.IEndTs;
              document.getElementById('lblIOASessNo').innerHTML = value.ISesHdrId;
              document.getElementById('Normalrad').value = value.NSesHdrId;
              document.getElementById('lblNormalUsr').innerHTML = value.NUser;
              document.getElementById('lblNormalStime').innerHTML = value.NStartTs;
              document.getElementById('lblNormalEtime').innerHTML = value.NEndTs;
              document.getElementById('lblSessNo').innerHTML = value.NSesHdrId;

              //if (type == "Execute") {
              //    document.getElementById('IOArad').setAttribute("disabled", true);
              //}
          });
      },
      error: function (request, status, error) {
          alert("Error");
      }
  });
}
function GetIOAPopup() {
    
    $.ajax(
         {
             type: "POST",
             url: "TemplateFrame.aspx/GetIOAPopup",
             contentType: "application/json; charset=utf-8",
             async: false,
             dataType: "json",
             success: function (data) {
                 popup = data.d;

             }
         });
    return popup;
}
function IOAEvaluate() {
    //alert("Enter");
    var resultIOA = GetIOAPopup();
    var resIOA = resultIOA.split('-');
    var result = resIOA[0];
    permissionStatus = resIOA[1];

    
    $.ajax(
    {
        type: "POST",
        url: "TemplateFrame.aspx/GetVisualToolStatus",
        contentType: "application/json; charset=utf-8",
        dataType: "json",        
        async: false,
        success: function (data) {            
            isVisualTool = data.d.split('|')[0];
            VTLessonId = data.d.split('|')[1];
        },
        error: function (request, status, error) {            
            alert("Error");
        }
    });
    
    execute = false;
    if (isVisualTool == "0") {

        generateTemplate();
    }
    // 3: , More than one draft exist(IOA user and Normal User).
    // 2: , One draft exist.
    // 1: , No drafts exist.
    if (sesID == 0) {
        var styl = document.getElementById('div_IOAInd').style;
        styl.display = 'none';
        if (result == "3") {

            if (isVisualTool != "0") {                                                  //if this lesson is a visual tool then popup for execute and scoring will appear

                $('#overlay').show('slow');
                $('#dialogExecute').show('slow');
                $('#btnExecute').click(function () {


                    execute = true;

                    //IOAandNormalPopUp();
                    //window.open('../VisualTool/TeachPage_Redirecting.aspx', '_blank');

                    generateTemplate();
                    // var HDR = document.getElementById("txtSessionNumber").value;
                    getTemplatesDatas(sessHdrId);
                    $('#overlay').hide('slow', function () {
                        $('#dialogExecute').hide('slow');
                    });


                });
                $('#btnScoring').click(function () {

                    execute = false;
                    IOAandNormalPopUp();


                });
            }
            else {
                IOAandNormalPopUp();
                execute = false;
            }


        }
        else {
            if (result == "1") {
                //saveSessionValues("Save");
                //var HDR = document.getElementById("txtSessionNumber").value;
                //getTemplatesDatas(HDR);


                if (isVisualTool != "0") {                                                  //if this lesson is a visual tool then popup for execute and scoring will appear

                    $('#overlay').show('slow');
                    $('#dialogExecute').show('slow');
                    $('#btnExecute').click(function () {
                        //saveSessionValues("Save");
                        execute = true;
                        generateTemplate();
                        saveSessionValues("Save");
                        //var HDR = document.getElementById("txtSessionNumber").value;        // if execute is clicked the page will be redirected to the corresponding execute page.

                        $('#overlay').hide('slow', function () {
                            $('#dialogExecute').hide('slow');
                        });
                        //window.open('../VisualTool/TeachPage_Redirecting.aspx', '_blank');
                    });
                    $('#btnScoring').click(function () {
                        execute = false;
                        generateTemplate();
                        saveSessionValues("Save");
                        //var HDR = document.getElementById("txtSessionNumber").value;
                        //getTemplatesDatas(sessHdrId);

                        $('#overlay').hide('slow', function () {
                            $('#dialogExecute').hide('slow');
                        });
                    });
                }
                else {
                    execute = false;
                    if (vActSession > 0) {

                        getTemplatesDatas(vActSession);
                    }
                    generateTemplate();
                    saveSessionValues("Save");
                    getTemplatesDatas(sessHdrId);
                }
            }
            if (result == "2") {

                if (isVisualTool != "0") {                                                  //if this lesson is a visual tool then popup for execute and scoring will appear

                    $('#overlay').show('slow');
                    $('#dialogExecute').show('slow');
                    $('#btnExecute').click(function () {
                        execute = true;
                        generateTemplate();
                        // var HDR = document.getElementById("txtSessionNumber").value;
                        getTemplatesDatas(sessHdrId);
                        $('#overlay').hide('slow', function () {
                            $('#dialogExecute').hide('slow');
                        });
                        //window.open('../VisualTool/TeachPage_Redirecting.aspx', '_blank');

                    });
                    $('#btnScoring').click(function () {
                        execute = false;
                        generateTemplate();
                        $('#overlay').hide('slow', function () {
                            $('#dialogExecute').hide('slow');
                        });
                        $('#overlay').fadeIn('slow', function () {
                            $('#dialog').fadeIn('slow');
                            document.getElementById('ddlUsers').style.visibility = 'hidden';
                            document.getElementById('btnOk').style.visibility = 'hidden';
                        });
                    });
                }
                else {
                    execute = false;
                    $('#overlay').fadeIn('slow', function () {
                        $('#dialog').fadeIn('slow');
                        document.getElementById('ddlUsers').style.visibility = 'hidden';
                        document.getElementById('btnOk').style.visibility = 'hidden';
                    });
                }
            }
            if (result == "4") {
                var styl = document.getElementById('div_IOAInd').style;
                styl.display = 'block';
                if (isVisualTool != "0") {                                                  //if this lesson is a visual tool then popup for execute and scoring will appear

                    $('#overlay').show('slow');
                    $('#dialogExecute').show('slow');
                    $('#btnExecute').click(function () {
                        execute = true;
                        generateTemplate();
                        // var HDR = document.getElementById("txtSessionNumber").value;
                        getTemplatesDatas(vActSession);
                        $('#overlay').hide('slow', function () {
                            $('#dialogExecute').hide('slow');
                        });
                        //window.open('../VisualTool/TeachPage_Redirecting.aspx', '_blank');

                    });
                    $('#btnScoring').click(function () {
                        execute = false;
                        $('#overlay').hide('slow', function () {
                            $('#dialogExecute').hide('slow');
                        });
                        getTemplatesDatas(vActSession);
                    });
                }
                else {
                    execute = false;
                    getTemplatesDatas(vActSession);
                }
            }
            //var userstatus = GetUserStatus();
            //if (userstatus.toString() != "0") {
            //    saveSessionValues("Save");
            //    var HDR = document.getElementById("txtSessionNumber").value;
            //    getTemplatesDatas(HDR);
            //}
            //else {
            //    if (result != 0) {

            //    }

            //}


        }
    }
    else {
        generateTemplate();
        getTemplatesDatas(sesID);
        var stle = document.getElementById('btnDaveSubmit').style;
        stle.display = "none";
        var stle = document.getElementById('btnSubmit').style;
        stle.display = "none";
    }
}


function noIOA() {
    //generateTemplate();
   
    getTemplatesDatas(vActSession);
   
    //$('#dialog').fadeOut('slow');
    //$('#overlay').fadeOut('slow');
}

function SubmitSession() {

    generateTemplate();
    var IOAradiobtn = document.getElementById("IOArad");
    var Normalradiobtn = document.getElementById("Normalrad");
    if (IOAradiobtn.checked == true) {
        var styl = document.getElementById('div_IOAInd').style;
        styl.display = 'block';
        getTemplatesDatas(IOAradiobtn.value);
    }
    else if (Normalradiobtn.checked == true) {
        var styl = document.getElementById('div_IOAInd').style;
        styl.display = 'none';
        getTemplatesDatas(Normalradiobtn.value);
    }
    $('#overlay').fadeOut('slow', function () {
        $('#dialog1').fadeOut('slow');
    });


}
var aryScores = ["_Accuracy", "_Independant", "_Prompted", "Total Duration", "Average Duration", "Frequency"];

function includeMistrialStatus() {
    for (var count = 1; count <= arStepId.length; count++) {
        if (IncMistrail == true) {
            var misTrial = document.getElementById("mistrial-" + count);
            if (misTrial.checked == true) {
                misTrials[count - 1] = "Y";
            }
            if (misTrial.checked == false) {
                misTrials[count - 1] = "N";
            }
        }
        else {

            misTrials[count - 1] = "NAV";
        }
    }
}
var popup = "";


var UserStatus = "";

function GetUserStatus() {
    $.ajax(
        {


            type: "POST",
            url: "TemplateFrame.aspx/GetUserStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                var list = jQuery.parseJSON(data.d);
                var Th = "";
                UserStatus = data.d;
                $.each(list, function (index, value) {

                });

            }
        });
    return UserStatus;
}

function createScoreSheet() {
    var tbl = document.getElementById('tblDiscrete');
    var lastRow = document.getElementById("tblDiscrete").rows.length;
    var row = tbl.insertRow(lastRow);
    row.setAttribute("style", "border-top:1px groove black;margin:2px;border-bottom:4px double black;");
    var cellRight0 = row.insertCell(0);
    cellRight0.setAttribute('colSpan', 4 + parseInt(arColumnType.length));
    var el = document.createElement('input');
    el.setAttribute('type', 'button');
    el.setAttribute("style", "font-weight:bold;");
    el.setAttribute("value", 'Total Score');
    el.setAttribute("name", arColumnId[index]);
    el.setAttribute('id', lastRow);
    el.setAttribute('class', 'gridButton');
    cellRight0.appendChild(el);
    for (var index = 0; index < 6; index++) {
        lastRow = document.getElementById("tblDiscrete").rows.length;
        var row = tbl.insertRow(lastRow);
        //row.id = aryScores[index];
        row.setAttribute('class', aryScores[index].replace(/ /g, '_'));
        row.setAttribute("style", "display:none;width:100%;");
        var cellRight = row.insertCell(0);
        //cellRight.setAttribute('colSpan', 2);
        var el = document.createElement('input');
        el.setAttribute('type', 'button');
        el.setAttribute("value", aryScores[index].replace(/_/g, '%'));
        el.setAttribute("name", aryScores[index]);
        el.setAttribute('id', lastRow);
        el.setAttribute('class', 'gridButton');
        cellRight.appendChild(el);
        var cellRight = row.insertCell(1);
        for (var count = 0; count < arColumnType.length; count++) {
            var cellRight1 = row.insertCell(2 + count);
            var el = document.createElement('input');
            el.setAttribute('type', 'text');
            el.setAttribute('value', '------');
            el.setAttribute("name", aryScores[index]);
            el.setAttribute('id', aryScores[index] + count);
            el.setAttribute("style", "width:auto;border:none;text-align:center;background-color:transparent;");
            el.setAttribute("disabled", true);
            cellRight1.appendChild(el);
        }
        row.insertCell(2 + arColumnType.length);
        row.insertCell(3 + arColumnType.length);

    }
}

function checkScoreSheet() {
    for (var counter = 0; counter < arColumnType.length; counter++) {

        if ((arColumnType[counter] == "Prompt") || (arColumnType[counter] == "+/-")) {
            $('.' + aryScores[0].replace(/ /g, '_')).show();
            $('.' + aryScores[1].replace(/ /g, '_')).show();
            $('.' + aryScores[2].replace(/ /g, '_')).show();
        }
        if (arColumnType[counter] == "Duration") {
            $('.' + aryScores[3].replace(/ /g, '_')).show();
            $('.' + aryScores[4].replace(/ /g, '_')).show();
        }
        if (arColumnType[counter] == "Frequency") {
            $('.' + aryScores[5].replace(/ /g, '_')).show();
        }
    }

}

function calculateScore() {

    includeMistrialStatus();
    var accuracy = 0;
    var independant = 0;
    var prompt = 0;
    var percent = 0;
    var i = 0;
    for (var index = 0; index < 6; index++) {
        for (var counter = 0; counter < arColumnType.length; counter++) {
            i = 1;
            percent = 0;
            accuracy = 0;
            independant = 0;
            prompt = 0;
            for (var count = 0; count < arStepId.length; count++) {
                if (arColumnType[counter] == "Prompt") {
                    var promptType = document.getElementById(arColumnName[counter] + "-" + [i]);

                    if (parseInt(promptType.options[promptType.selectedIndex].value) >= parseInt(document.getElementById('txtCurrentPrompt').name)) {
                        if (arMistrial[counter] == 0) {
                            if ((misTrials[count] == 'N') || (misTrials[count] == 'NAV')) {
                                accuracy++;
                            }
                        }
                        else {
                            accuracy++;
                        }
                    }
                    if (promptType.options[promptType.selectedIndex].text == "Independent") {
                        if (arMistrial[counter] == 0) {
                            if ((misTrials[count] == 'N') || (misTrials[count] == 'NAV')) {
                                independant++;
                            }
                        }
                        else {
                            independant++;
                        }
                    }
                }
                if (arColumnType[counter] == "+/-") {
                    var promptType = document.getElementById(arColumnName[counter] + "-" + [i]);
                    if (document.getElementById(arColumnName[counter] + "-" + [i]).checked == true) {
                        if (arMistrial[counter] == 0) {

                            if ((misTrials[count] == 'N') || (misTrials[count] == 'NAV')) {
                                accuracy++;
                                independant++;
                            }
                        }
                        else {
                            accuracy++;
                            independant++;
                        }
                    }
                    else {

                    }
                }
                if (arColumnType[counter] == "Duration") {

                }
                if (arColumnType[counter] == "Frequency") {

                }
                i++;
            }
            if (index == 0) {
                percent = parseInt((accuracy / arStepId.length) * 100);
                document.getElementById(aryScores[index] + counter).value = percent;
            }
            if (index == 1) {
                percent = parseInt((independant / arStepId.length) * 100);
                document.getElementById(aryScores[index] + counter).value = percent;
            }
            if (index == 2) {

                percent = 100 - (parseInt((independant / arStepId.length) * 100));
                document.getElementById(aryScores[index] + counter).value = percent;
            }

            if (index == 5) {

                percent = 100 - (parseInt((independant / arStepId.length) * 100));
                document.getElementById(aryScores[index] + counter).value = percent;
            }
        }
    }
}

function fn_convertTime(sec) {

    var totalSec = sec;
    hours = parseInt(totalSec / 3600) % 24;
    minutes = parseInt(totalSec / 60) % 60;
    seconds = totalSec % 60;

    result = (hours < 10 ? "0" + hours : hours) + ":" + (minutes < 10 ? "0" + minutes : minutes) + ":" + (seconds < 10 ? "0" + seconds : seconds);

    return result;
}
var avgTim = 0;
var totalTim = 0;

function fn_calculateTotalTime() {
    Time = new Date();
    for (var counter = 0; counter < arColumnType.length; counter++) {
        var totTime = 0;
        if (arColumnType[counter] == "Duration") {
            var timerCollection = $('.col' + arColumnName[counter].replace(/ /g, ''));

            for (var index = 0; index < timerCollection.length; index++) {
                totTime += parseInt($(timerCollection[index]).val());
            }
            var avgTime = fn_convertTime(parseInt(totTime / timerCollection.length));
            var totalTime = fn_convertTime(totTime);
            avgTim = totTime / timerCollection.length;
            totalTim = totTime;
            document.getElementById(aryScores[3] + counter).value = totalTim;
            document.getElementById(aryScores[4] + counter).value = avgTim;


        }

    }

} setInterval(fn_calculateTotalTime, 1000);

function getScores() {
    for (var index = 0; index < 6; index++) {
        for (var count = 0; count < arColumnType.length; count++) {
            if (index == 0) {
                arScoreAccuracy[count] = document.getElementById(aryScores[index] + count).value;
            }
            if (index == 1) {
                arScoreIndependant[count] = document.getElementById(aryScores[index] + count).value;
            }
            if (index == 2) {
                arScorePrompt[count] = document.getElementById(aryScores[index] + count).value;
            }
            if (index == 3) {
                arScoreTotDuration[count] = document.getElementById(aryScores[index] + count).value;
            }
            if (index == 4) {
                arScoreAvgDuration[count] = document.getElementById(aryScores[index] + count).value;
            }
            if (index == 5) {
                arScoreFreq[count] = document.getElementById(aryScores[index] + count).value;
            }
        }
    }
}

function getTemplatesDatas(session) {
  
    $.ajax(
       {

           type: "POST",
           url: "TemplateFrame.aspx/GetDatasheetValues",
           data: "{'iSessionHeaderId':'" + session + "'}",
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           async: false,
           success: function (data) {
               var dataList = jQuery.parseJSON(data.d);
               $.each(dataList, function (index, value) {
                   document.getElementById('txtSessionNumber').value = value.iSessionNbr;
                   document.getElementById('lpIdTxt').innerHTML = value.sLessonPlanName + "&nbsp&nbsp&nbsp Session Number  : " + value.iSessionNbr;
                   document.getElementById('nextSetTxt').value = value.sCurSetName;
                   document.getElementById('txtCurrentPrompt').value = value.sCurPromptName;
                   document.getElementById('txtCurrentPrompt').name = value.iCurPromptId;
                   //  document.getElementById('txtSessNbr').innerHTML = value.iSessionNbr;
                   document.getElementById('txtStartTime').innerHTML = value.sStartTs;
                   document.getElementById('txtEndTime').innerHTML = value.sEndTs;
                   document.getElementById('chkMissSession').value = value.sSesMisTrial;
                   document.getElementById('txtAreaComments').value = value.sComnts;
                   document.getElementById('txtCreatedBy').innerHTML = value.sCreatedBy;
                   document.getElementById('txtCreatedOn').innerHTML = value.sCreatedOn;
                   document.getElementById('txtModifiedBy').innerHTML = value.sModifiedBy;
                   document.getElementById('txtModifiedOn').innerHTML = value.sModifiedOn;
                   if (permissionStatus == "true") {
                       var stle = document.getElementById('btnDaveSubmit').style;
                       stle.display = "block";
                       var stle = document.getElementById('btnSubmit').style;
                       stle.display = "block";
                   }
                   //var stle = document.getElementById('btnHdrPopup').style;
                   //stle.display = "none";
                   //vSessionNumber = session;
                   vSessionNumber = value.iSessionNbr;
                   sessHdrId = value.iSesHdrId;
                   if (value.sSesMisTrial == 'True') {
                       document.getElementById('chkMissSession').checked = 'checked';
                   }
                   var count = 1;
                   for (var iStepValues in value.oStepValues) {

                       if (value.oStepValues[iStepValues].sSessinStatus == "Y") {
                           document.getElementById("mistrial-" + count).checked = 'checked';
                       }
                       document.getElementById('Message' + count).value = value.oStepValues[iStepValues].sComment;
                       count++;
                   }
                   count = 1;
                   for (var iColumnValues in value.oColumnDatas) {
                       if (value.oColumnDatas[iColumnValues].sColValue == '+') {
                           document.getElementById(value.oColumnDatas[iColumnValues].sColName + "-" + value.oColumnDatas[iColumnValues].iRowNumber).checked = 'checked';
                       }
                       else if (value.oColumnDatas[iColumnValues].sColValue == '-') {
                           document.getElementById(value.oColumnDatas[iColumnValues].sColName + "N-" + value.oColumnDatas[iColumnValues].iRowNumber).checked = 'checked';
                       }
                       else if (value.oColumnDatas[iColumnValues].sColValue > 0) {
                           //   alert(value.oColumnDatas[iColumnValues].sColName + '-' + value.oColumnDatas[iColumnValues].iRowNumber);
                           var prompType = document.getElementById(value.oColumnDatas[iColumnValues].sColName + '-' + value.oColumnDatas[iColumnValues].iRowNumber);
                           for (var i = 0; i < prompType.options.length; i++) {
                               if (prompType.options[i].value == value.oColumnDatas[iColumnValues].sColValue) {
                                   prompType.options[i].setAttribute('selected', 'selected');
                                   break
                               }
                           }
                           //prompType.selectedIndex = value.oColumnDatas[iColumnValues].sColValue;
                           // $('#prompType').attr('SelectedIndex', value.oColumnDatas[iColumnValues].sColValue);
                           //prompType.options[prompType.selectedIndex] = value.oColumnDatas[iColumnValues].sColValue;

                       }
                       else if (value.oColumnDatas[iColumnValues].sColValue >= '00:00:00') {
                           document.getElementById(value.oColumnDatas[iColumnValues].sColName + '-' + value.oColumnDatas[iColumnValues].iRowNumber).value = value.oColumnDatas[iColumnValues].sColValue;
                       }
                       else {
                           document.getElementById(value.oColumnDatas[iColumnValues].sColName + '-' + value.oColumnDatas[iColumnValues].iRowNumber).value = value.oColumnDatas[iColumnValues].sColValue;
                       }
                   }
                   calculateScore();
                   if (isVisualTool != "0") {

                       // alert('go to execution')
                   }
               });
           },
           error: function (request, status, error) {
               alert("Error");
           }
       });
}

function addRowsToTable(id, name, desc) {

    $('#sidebar .content').append('<div id="lessonPlan"> </div>');
    //createDiscreteTemplate OplesnPlan
    temp_id = id;
    temp_name = name;
    temp_desc = desc;

    var tbl = document.getElementById('lessonPlan');

    var lesPlan = document.createElement('label');
    //Assign different attributes to the element.
    //lesPlan.setAttribute('type', 'button');
    lesPlan.setAttribute("text", temp_name);
    lesPlan.setAttribute("name", temp_id);
    lesPlan.setAttribute('id', temp_id);
    lesPlan.setAttribute('class', 'divLbl');
    var el = document.createElement('label');
    //Assign different attributes to the element.
    // el.setAttribute('type', 'button');
    el.setAttribute("value", temp_name);
    el.setAttribute("text", temp_id);
    el.setAttribute('id', temp_id);
    el.setAttribute('class', 'divLbl');
    el.onclick = function () {
        $('IFRAME').hide();
        if ($('#IFRAME-' + this.id).length) {
            alert('Iframe exists');
        }
        else {
            ifrm = document.createElement("IFRAME");
            ifrm.setAttribute("src", "TemplateFrame.aspx?id=" + this.id);
            ifrm.setAttribute('class', 'clsFrame');
            ifrm.setAttribute('scrolling', 'no');
            ifrm.id = "IFRAME-" + this.id;
            $('#frameDiv').append(ifrm);

        }
        deleteRows();

        $('#discrete').append('<div/>');


    }
    tbl.appendChild(el);


}

function listPrompts(id) {


    var sel = document.createElement('select');
    sel.type = 'select';
    sel.name = id;
    sel.id = id;
    sel.size = 1;
    sel.width = 177;

    var option = document.createElement("option");
    option.text = "Select"
    option.value = "0";

    try {

        sel.add(option, null);
        sel.enable = false;  //Standard 
    } catch (error) {

        sel.add(option); // IE only
    }

    for (var count = 0; count < arPromptId.length; count++) {

        var option = document.createElement("option");
        option.text = arPromptName[count];
        option.value = arPromptId[count];

        try {

            sel.add(option, null);
            sel.enable = false;  //Standard 
        } catch (error) {

            sel.add(option); // IE only
        }
    }
    return (sel);
}

function checkIOA(chk) {
    var styl = document.getElementById('dialog').style;
    styl.marginTop = '-490px';
    var drpUser = document.getElementById('ddlUsers');
    var chkIOA = document.getElementById('IOAEnable');
    var btndisable = document.getElementById('IOADisable');
    var donebtn = document.getElementById('btnOk');
    var ioalbl = document.getElementById('LblIOAEnable');

    if (chk.id == "IOAEnable") {

        chk.style.visibility = 'hidden';
        drpUser.style.visibility = 'visible';
        donebtn.style.visibility = 'visible';
        ioalbl.style.visibility = 'hidden';
        btndisable.style.visibility = 'hidden';
    }
    else {
        chk.style.visibility = 'visible';
        drpUser.style.visibility = 'hidden';
        donebtn.style.visibility = 'hidden';
        ioalbl.style.visibility = 'visible';
    }
}

function fillUsers() {
    generateTemplate();
    var drpUser = document.getElementById('ddlUsers');
    //var chkIOA = document.getElementById('IOAEnable');
    //var donebtn = document.getElementById('btnOk');
    //var ioalbl = document.getElementById('LblIOAEnable');
    //if (vActSession > 0) {
    //    chkIOA.style.visibility = 'visible';
    //    drpUser.style.visibility = 'hidden';
    //    donebtn.style.visibility = 'hidden';
    //    ioalbl.style.visibility = 'visible';
    //}
    //else {
    //    chkIOA.style.visibility = 'hidden';
    //    drpUser.style.visibility = 'visible';
    //    donebtn.style.visibility = 'visible';
    //    ioalbl.style.visibility = 'hidden';
    //}

    $("#ddlUsers").empty();
    var option = document.createElement("option");
    option.text = "Select User"
    option.value = "0";
    option.setAttribute('selected', 'selected');
    try {

        drpUser.add(option, null);
        drpUser.enable = false;  //Standard 
    } catch (error) {

        drpUser.add(option); // IE only
    }

    for (var index = 0; index < arUserId.length; index++) {
        var option = document.createElement("option");
        option.text = arUserName[index];
        option.value = arUserId[index];

        try {

            drpUser.add(option, null);
            drpUser.enable = false;  //Standard 
        } catch (error) {

            drpUser.add(option); // IE only
        }

    }
}

function fillSetDetails() {
    var drpSet = document.getElementById('ddlSet');

    var length = drpSet.options.length;
    for (i = 0; i < length; i++) {
        drpSet.options[i] = null;
    }

    var option = document.createElement("option");
    option.text = "Select Set"
    option.value = "0";
    option.setAttribute('selected', 'selected');
    try {

        drpSet.add(option, null);
        drpSet.enable = false;  //Standard 
    } catch (error) {

        drpSet.add(option); // IE only
    }

    for (var index = 0; index < arSetId.length; index++) {
        var option = document.createElement("option");
        option.text = arSetName[index];
        option.value = arSetId[index];

        try {

            drpSet.add(option, null);
            drpSet.enable = false;  //Standard 
        } catch (error) {

            drpSet.add(option); // IE only
        }

    }
}

function fillStepDetails() {
    var drpStep = document.getElementById('ddlStep');

    var length = drpStep.options.length;
    for (i = 0; i < length; i++) {
        drpStep.options[i] = null;
    }
    var option = document.createElement("option");
    option.text = "Select Step"
    option.value = "0";
    option.setAttribute('selected', 'selected');
    try {

        drpStep.add(option, null);
        drpStep.enable = false;  //Standard 
    } catch (error) {

        drpStep.add(option); // IE only
    }

    for (var index = 0; index < arStepId.length; index++) {
        var option = document.createElement("option");
        option.text = arStepName[index];
        option.value = arStepId[index];

        try {

            drpStep.add(option, null);
            drpStep.enable = false;  //Standard 
        } catch (error) {

            drpStep.add(option); // IE only
        }

    }
}

function fillPromptDetails() {
    var drpPrmt = document.getElementById('ddlPrompt');
    var length = drpPrmt.options.length;
    for (i = 0; i < length; i++) {
        drpPrmt.options[i] = null;
    }
    var option = document.createElement("option");
    option.text = "Select Prompt"
    option.value = "0";
    option.setAttribute('selected', 'selected');
    try {

        drpPrmt.add(option, null);
        drpPrmt.enable = false;  //Standard 
    } catch (error) {

        drpPrmt.add(option); // IE only
    }

    for (var index = 0; index < arPromptId.length; index++) {
        var option = document.createElement("option");
        option.text = arPromptName[index];
        option.value = arPromptId[index];

        try {

            drpPrmt.add(option, null);
            drpPrmt.enable = false;  //Standard 
        } catch (error) {

            drpPrmt.add(option); // IE only
        }

    }
}

var userName = "";
var userId = 0;
var IOA = false;

function getUserData1() {

    var drpUser = document.getElementById('ddlUsers');

    userName = drpUser.options[drpUser.selectedIndex].text;
    userId = drpUser.options[drpUser.selectedIndex].value;
    if (userId > 0) {



        IOA = true;
        //generateTemplate();
        saveSessionValues("Save");
        //generateTemplate(sessHdrId);
        IOA = false;
    }
    $('#dialog').fadeOut('slow');
    $('#overlay').fadeOut('slow');

}

function getHdrData() {
    var drpSet = document.getElementById('ddlSet');
    var drpStep = document.getElementById('ddlStep');
    var drpPrmt = document.getElementById('ddlPrompt');
    document.getElementById('nextSetTxt').value = drpSet.options[drpSet.selectedIndex].text;
    document.getElementById('nextSetTxt').name = drpSet.options[drpSet.selectedIndex].value;
    document.getElementById('txtCurrentPrompt').value = drpPrmt.options[drpPrmt.selectedIndex].text;
    document.getElementById('txtCurrentPrompt').name = drpPrmt.options[drpPrmt.selectedIndex].value;
    document.getElementById('nextStepTxt').value = drpStep.options[drpStep.selectedIndex].text;
    document.getElementById('nextStepTxt').name = drpStep.options[drpStep.selectedIndex].value;

    //  

    $('#divHderDtls').fadeOut('slow');
    $('#overlay').fadeOut('slow');
}

function isNumberKey(value) {
    //    var charCode = (evt.which) ? evt.which : event.keyCode
    //    if (charCode > 31 && (charCode < 48 || charCode > 57)) return false; return true;
    if (value == NaN) {
        alert("Not a Number");
    }
}

function deleteRows() {

    $('#tblDiscrete').remove();

}

var time = new Array();
var msg = new Array();

function check(id) {

    var item = document.getElementById(id);

    return true;
}

var popUpInd = null;

function createDiscreteTemplate() {
    var cnt = 0;
    var lastRow = 1;
    for (var count = 0; count < arColumnName.length; count++) {
        document.getElementById('tblDiscrete').rows[0].cells[count + 2].innerHTML = arColumnName[count];
    }
    var tbl = document.getElementById('tblDiscrete');
    for (var index = 0; index < arStepId.length; index++) {
        cnt = parseInt(index + 1);
        lastRow = document.getElementById("tblDiscrete").rows.length;
        var row = tbl.insertRow(lastRow);
        var cellRight = row.insertCell(0);
        cellRight.setAttribute('style', 'text-align:center;margin-left:5px;');
        cellRight.id = lastRow;
        cellRight.name = arColumnId[index];
        cellRight.innerHTML = lastRow;

        var cellRight0 = row.insertCell(1);
        cellRight0.id = lastRow;
        cellRight0.setAttribute('style', 'text-align:center;');
        cellRight0.name = arColumnName[index];
        cellRight0.innerHTML = arStepName[index];

        for (var count = 0; count < arColumnType.length; count++) {
            var cellRight1 = row.insertCell(2 + count);
            cellRight1.setAttribute('style', 'text-align:center;');
            cellRight1.setAttribute('class', arColumnName[count] + '-' + lastRow);
            if (arColumnType[count] == "Prompt") {
                var ret = listPrompts(arColumnName[count] + '-' + lastRow);
                ret.onchange = function () {
                    calculateScore();
                }
                cellRight1.appendChild(ret);
            }
            else if (arColumnType[count] == "+/-") {
                var radioYes = document.createElement("input");
                radioYes.setAttribute("type", "radio");

                /*Set id of new created radio button*/
                radioYes.setAttribute("id", arColumnName[count] + "-" + lastRow);

                /*set unique group name for pair of Yes / No */
                radioYes.setAttribute("name", arColumnName[count] + lastRow);

                if (isVisualTool != '0') {
                    radioYes.setAttribute("disabled", true);
                }

                radioYes.onclick = function () {
                    calculateScore();
                }
                //radioYes.setAttribute("class","regular-radio big-radio");
                /*creating label for Text to Radio button*/
                var lblYes = document.createElement("lable");

                /*create text node for label Text which display for Radio button*/
                var textYes = document.createTextNode("+");

                /*add text to new create lable*/
                lblYes.appendChild(textYes);

                /*add radio button to Div*/
                cellRight1.appendChild(radioYes);

                /*add label text for radio button to Div*/
                cellRight1.appendChild(lblYes);

                /*add space between two radio buttons*/
                var space = document.createElement("span");
                space.setAttribute("innerHTML", "&nbsp;&nbsp");
                cellRight1.appendChild(space);
                cellRight1.appendChild(space);
                var radioNo = document.createElement("input");
                radioNo.setAttribute("type", "radio");
                radioNo.setAttribute("id", arColumnName[count] + "N-" + lastRow);
                radioNo.setAttribute("name", arColumnName[count] + lastRow);

                if (isVisualTool != '0') {
                    radioNo.setAttribute("disabled", true);
                }

                radioNo.onclick = function () {
                    calculateScore();
                }
                var lblNo = document.createElement("label");
                lblNo.innerHTML = "-";
                cellRight1.appendChild(radioNo);
                cellRight1.appendChild(lblNo);

            }
            else if (arColumnType[count] == "Duration") {
                //addStopwatch(cellRight1, arColumnName[count] + '-' + lastRow, arColumnName[count]);
                addStopwatch(cellRight1, arColumnName[count] + '-' + lastRow, arColumnName[count], isVisualTool);
            }
            else if (arColumnType[count] == "Frequency") {

                //$('.' + arColumnName[count] + '-' + lastRow).append('<input type="text" id=' + arColumnName[count] + '-' + lastRow + ' style="width:auto;" onkeyup="return check(this.id);/>');

                var el = document.createElement('input');
                el.setAttribute('style', 'width:auto');
                el.setAttribute('type', 'text');
                //el.setAttribute("name", measureTime[index]);
                el.setAttribute('id', arColumnName[count] + '-' + lastRow);

                if (isVisualTool != '0') {
                    el.setAttribute('disabled', 'true');
                }

                el.onkeydown = function () {

                    //check(this.id);

                }
                cellRight1.appendChild(el);
            }
            else if (arColumnType[count] == "Text") {

                var el = document.createElement('input');
                el.setAttribute('style', 'width:auto');
                el.setAttribute('type', 'text');
                //el.setAttribute("name", measureTime[index]);
                el.setAttribute('id', arColumnName[count] + '-' + lastRow);

                if (isVisualTool != '0') {
                    el.setAttribute('disabled', 'true');
                }

                cellRight1.appendChild(el);
            }
        }
        if (IncMistrail == true) {
            var colCount = arColumnType.length + 2;
            var cellRight4 = row.insertCell(colCount);
            cellRight4.setAttribute('style', 'text-align:center');
            var checkbox = document.createElement('input');
            checkbox.setAttribute("type", "checkbox");
            checkbox.setAttribute("id", "mistrial-" + lastRow);
            checkbox.onclick = function () {
                calculateScore();
            }
            cellRight4.appendChild(checkbox);


            var cellRight7 = row.insertCell(colCount + 1);
            cellRight7.setAttribute('style', 'text-align:center');
            //var textarea = document.createElement('Textarea');
            //textarea.setAttribute('type', 'Textarea');
            var textarea = document.createElement('input');
            textarea.setAttribute('type', 'text');
            //textarea.setAttribute('rows', '2');
            //textarea.setAttribute('cols', '20');
            textarea.setAttribute('resize', 'false');
            textarea.setAttribute('id', "Message" + lastRow);
            cellRight7.appendChild(textarea);
            lastRow = tbl.rows.length;
        }
        else {
            var colCount = arColumnType.length + 2;
            var cellRight7 = row.insertCell(colCount);
            cellRight7.setAttribute('style', 'text-align:center');
            //var textarea = document.createElement('Textarea');
            //textarea.setAttribute('type', 'Textarea');
            var textarea = document.createElement('input');
            textarea.setAttribute('type', 'text');
            //textarea.setAttribute('rows', '2');
            //textarea.setAttribute('cols', '20');
            textarea.setAttribute('resize', 'false');
            textarea.setAttribute('id', "Message" + lastRow);
            cellRight7.appendChild(textarea);
            lastRow = tbl.rows.length;
        }
    }
}

var colNames = new Array();
var colValues = new Array();
var misTrials = new Array();
var parameter = new Array();
var sessHdrId = 0;
var Ispopupexist = "";

function saveSessionValues(saveType) {

    colValues = new Array();
    colNames = new Array();
    getScores();

    SaveTp = saveType;

    var misSession = "";

    var d = new Date();
    d.setDate(new Date().getDate());
    var endTime = d.toTimeString().substr(0, d.toTimeString().lastIndexOf(" "));
    var strComment = document.getElementById('txtAreaComments').value;
    if (strComment.length == 0) {
        strComment = " ";
    }
    else {
        strComment = strComment.replace(/,/g, "^");
    }
    if (document.getElementById('chkMissSession').checked == true) {
        misSession = "Y";
    }
    else {
        misSession = "N";
    }
    var count = 0;


    if (sessHdrId == 0) {

        sessHdrId = 0;
    }
    else {
        if (IOA == true) {
            sessHdrId = 0;

        }
        //else {
        //    sessHdrId = txtSessHdr;
        //}
    }

    for (var index = 0; index < arStepId.length; index++) {
        count = parseInt(index + 1);
        var trailName = document.getElementById('Message' + count);
        var msgStr = trailName.value;
        if (msgStr.length == 0) {
            msg[index] = " ";

        }
        else if (sessHdrId == 0) {
            msg[index] = " ";
        }
        else {
            msg[index] = trailName.value;
        }
    }


    var tbl = document.getElementById('tblDiscrete');

    var rowCount = tbl.rows.length - 8;
    var op;
    for (var index = 0; index < arColumnName.length; index++) {
        for (var count = 1; count < rowCount + 1; count++) {
            colNames[index] = arColumnId[index];
            op = "";
            switch (arColumnType[index]) {
                case "Prompt":
                    {
                        if (sessHdrId == 0) {
                            op = 0;
                        }
                        else {
                            var itemName = document.getElementById(arColumnName[index] + "-" + count);
                            op = itemName.options[itemName.selectedIndex].value;
                        }
                        break;

                    }
                case "+/-":
                    {
                        if (sessHdrId == 0) {
                            op = "";
                        }
                        else {
                            var itemNamePlus = document.getElementById(arColumnName[index] + "-" + count);
                            var itemNameMinus = document.getElementById(arColumnName[index] + "N-" + count);
                            if (itemNamePlus.checked == true) {
                                op = "+";
                            }
                            else if (itemNameMinus.checked == true) {
                                op = "-";
                            }
                            else {
                                op = "";
                            }
                        }
                        break;
                    }
                case "Duration":
                    {
                        if (sessHdrId == 0) {
                            op = "------";
                        }
                        else {
                            var itemName = document.getElementById(arColumnName[index] + "-" + count);
                            op = itemName.value;
                        }
                        break;
                    }
                case "Frequency":
                    {
                        if (sessHdrId == 0) {
                            op = 0;
                        }
                        else {
                            var itemName = document.getElementById(arColumnName[index] + "-" + count);
                            op = itemName.value;
                        }
                        break;
                    }
                case "Text":
                    {
                        if (sessHdrId == 0) {
                            op = 0;
                        }
                        else {
                            var itemName = document.getElementById(arColumnName[index] + "-" + count);
                            op = itemName.value;
                        }
                        break;
                    }

            }

            if (colValues[index] == null) {
                colValues[index] = op;
            }
            else {
                colValues[index] = colValues[index] + "|" + op;
            }

        }
    }

    for (var count = 1; count < rowCount + 1; count++) {
        if (IncMistrail == true) {
            var misTrial = document.getElementById("mistrial-" + count);
            if (misTrial.checked == true) {
                misTrials[count - 1] = "Y";
            }
            if (misTrial.checked == false) {
                misTrials[count - 1] = "N";
            }
        }
        else {

            misTrials[count - 1] = "NAV";
        }
    }
    var arStepIds = null;

    $.ajax(
      {

          type: "POST",
          url: "TemplateFrame.aspx/submitSessionValues",
          data: "{'parameter':'" + rowCount + "','parameter1':'" + colNames + "','parameter2':'" + colValues + "','parameter3':'" + misTrials + "','parameter4':'" + msg + "'," +
          "'parameter5':'" + strComment + "','parameter6':'" + misSession + "','parameter7':'" + endTime + "','parameter8':'" + saveType + "'," +
          "'parameter9':'" + sessHdrId + "','parameter10':'" + vNextSetId + "','parameter11':'" + vSessionNumber + "','parameter12':'" + iCurrentPrmtId + "'," +
          "'parameter13':'" + userId + "','parameter14':'" + arScoreAccuracy + "','parameter15':'" + arScoreIndependant + "','parameter16':'" + arScorePrompt + "'," +
          "'parameter17':'" + arScoreTotDuration + "','parameter18':'" + arScoreAvgDuration + "','parameter19':'" + arScoreFreq + "','parameter20':'" + arStepId + "'}",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          async: false,
          success: function (data) {
              //  txtSessHdr = data.d;
              var Msg = data.d;
              if (Msg == "Active Session is Not Added. Please Try Again...")
                  alert(Msg);
              sessHdrId = data.d;
              if (SaveTp.toString() == "Submit") {
                  window.location = "DSTempHistory.aspx";
              }
              else if (SaveTp.toString() == "Save") {

                  getTemplatesDatas(sessHdrId);
                  // vActSession = sessHdrId;

              }

          },
          error: function (request, status, error) {
              alert("Error");
          }
      });






}





