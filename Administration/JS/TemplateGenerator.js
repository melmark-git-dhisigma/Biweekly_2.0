

var flagclocks = 0;
var flagstops = 0;
var stoptimes = 0;
var splitcounter = 0;
var currenttimes;
var splitdate = '';
var output = '';
var clock;
var argIndex = 0;


function counterIn(starttime) {
    var id = "measureTime" + argIndex;
    clock = document.getElementById(id);
    currenttimes = new Date();
    var timediff = currenttimes.getTime() - starttime;
    if (flagstops == 1) {
        timediff = timediff + stoptimes
    }
    if (flagclocks == 1) {
        clock.value = formattime(timediff, '');
        refresh = setTimeout('counterIn(' + starttime + ');', 10);
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

function resetclock() {
    flagstops = 0;
    stoptimes = 0;
    
    window.clearTimeout(refresh);
    //output.value = "";
    splitcounter = 0;
    if (flagclocks == 1) {
        var resetdate = new Date();
        var resettime = resetdate.getTime();
        counterIn(resettime);
    }
    else {
        //clock.value = "00:00:0";
    }
}





var tempId;
var tempName;
var tempDesc;

function CallMyWebService() {
   
    $.ajax(
			{

			    type: "POST",
			    url: "Template.aspx/MyMethod",
			    data: "",
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async: false,
			    success: function (data) {
			       
			        var list = jQuery.parseJSON(data.d);
			        $.each(list, function (index, value) {


			            addRowToTable(value.TempId, value.TempName, value.TempDesc);

			        });


			    },
			    error: function (request, status, error) {
			        alert("Error");
			    }
			});

}


var startTime = 0;
var Trials = 1;
var measureId = new Array();
var measureName = new Array();
var measureType = new Array();

var scoreValues = ["Select", "First", "Second", "Third"];

function addRowToTable(id, name, desc) {

    createDiscreteTemplate
    temp_id = id;
    temp_name = name;
    temp_desc = desc;

    var tbl = document.getElementById('tblTempList');

    var lastRow = tbl.rows.length;

    var iteration = lastRow;
    var row = tbl.insertRow(lastRow);


    var cellRight = row.insertCell(0);
    var el = document.createElement('input');
    //Assign different attributes to the element.
    el.setAttribute('type', 'button');
    el.setAttribute("value", temp_name);
    el.setAttribute("name", temp_id);
    el.setAttribute('id', temp_id);
    el.setAttribute('class', 'gridButtonClick');
    el.onclick = function () {

        $.ajax(
			{

			    type: "POST",
			    url: "Template.aspx/loadTemplate",
			    data: "{'parameter1':'" + this.id + "'}",
			    contentType: "application/json; charset=utf-8",
			    dataType: "json",
			    async: false,
			    success: function (data) {

			        var list = jQuery.parseJSON(data.d);

			        $.each(list, function (index, value) {

			            Trials = value.ColumnTrials;

			            measureId[index] = value.ColumnId;
			            measureName[index] = value.ColumnName;
			            measureType[index] = value.ColumnType;



			        });
			        deleteRows();
			        $('#discrete').append('<table id="tblDiscrete" style="width: 800px"><thead><tr><th>No</th><th>Name</th><th></th><th></th><th></th><th>Mis-Trials</th><th>Timer</th><th>Elapsed Time</th><th>Notes</th></tr></thead><tbody></tbody></table>');
			        createDiscreteTemplate();
			       
			        var d = new Date();

			        d.setDate(new Date().getDate());

			        startTime = d.toTimeString().substr(0, d.toTimeString().lastIndexOf(" "));

			    },
			    error: function (request, status, error) {
			        alert("Error");
			    }
			});
    }

    cellRight.appendChild(el);

    var cellRights = row.insertCell(1);
    var el2 = document.createElement('input');
    el2.setAttribute('type', 'text');
    el2.setAttribute("value", temp_desc);
    el2.setAttribute("name", temp_id);
    el2.setAttribute('id', temp_id + "txt");
    el2.setAttribute('class', 'gridButton');


    cellRights.appendChild(el2);
}


function addRows() {
    Trials = 1;
    createDiscreteTemplate();
}




function deleteRows() {

    $('#tblDiscrete').remove();

   


}

var colName = new Array();
var firstColumn = new Array();
var secondColumn = new Array();
var thirdColumn = new Array();
var fourthColumn = new Array();
var time = new Array();
var msg = new Array();


var popUpInd = null;

function createDiscreteTemplate()
{
    var cnt = 0;
    var lastRow = 1;

    document.getElementById('tblDiscrete').rows[0].cells[2].innerHTML = measureName[0];
    document.getElementById('tblDiscrete').rows[0].cells[3].innerHTML = measureName[1];
    document.getElementById('tblDiscrete').rows[0].cells[4].innerHTML = measureName[2];

    var tbl = document.getElementById('tblDiscrete');

    for (var index = 0; index < Trials; index++) {

        cnt = parseInt(index + 1);




        lastRow = document.getElementById("tblDiscrete").rows.length;
        var row = tbl.insertRow(lastRow);


        var cellRight = row.insertCell(0);
        var el = document.createElement('input');
        el.setAttribute('type', 'button');
        el.setAttribute("value", lastRow);
        el.setAttribute("name", measureId[index]);
        el.setAttribute('id', lastRow);
        el.setAttribute('class', 'gridButton');

        cellRight.appendChild(el);


        var cellRight0= row.insertCell(1);
        var el = document.createElement('input');
        el.setAttribute('type', 'text');
        //el.setAttribute("value", lastRow);
        el.setAttribute("name", measureName[index]);
        el.setAttribute('id', "measureName"+lastRow);
       // el.setAttribute('class', 'gridButton');

        cellRight0.appendChild(el);


        var cellRight1 = row.insertCell(2);

        if (measureType[0] == "Prompt List") {


            var sel = document.createElement('select');
            sel.type = 'select';
            sel.name = 'select' + lastRow;
            sel.id = 'optRow1-' + lastRow;
            sel.size = 1;
            sel.width = 177;
            for (var key in scoreValues) {


                var option = document.createElement("option");
                option.text = scoreValues[key];
                option.id = scoreValues[key].id;



                try {

                    sel.add(option, null);
                    sel.enable = false;  //Standard 
                } catch (error) {

                    sel.add(option); // IE only
                }
            }
            sel.onchange = function () {
                
                
                var itId = this.id;
                var str = itId.split('-');
               
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                firstColumn[ind] = itemName.options[itemName.selectedIndex].value;
                alert(firstColumn[ind]);
                
            }
            cellRight1.appendChild(sel);
        }

        else if (measureType[0] == "+ / -") {
            var radioYes = document.createElement("input");
            radioYes.setAttribute("type", "radio");

            /*Set id of new created radio button*/
            radioYes.setAttribute("id", "radioPlus1-" + lastRow);

            /*set unique group name for pair of Yes / No */
            radioYes.setAttribute("name", "Measure1" + lastRow);

            radioYes.onclick = function () {

                var itId = this.id;
                var str = itId.split('-');
               
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                firstColumn[ind] = "+";

               
            }

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
            radioNo.setAttribute("id", "radioMinus1-" + lastRow);
            radioNo.setAttribute("name", "Measure1" + lastRow);

            radioNo.onclick = function () {
                var itId = this.id;
                var str = itId.split('-');
              
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                firstColumn[ind] = "-";

              
            }


            var lblNo = document.createElement("label");
            lblNo.innerHTML = "-";
            cellRight1.appendChild(radioNo);
            cellRight1.appendChild(lblNo);

        }

        var cellRight2 = row.insertCell(3);


        if (measureType[1] == "Prompt List") {

            //var textNode1 = document.createTextNode(temp_desc);
            var sel = document.createElement('select');
            sel.type = 'select';
            sel.name = 'select' + lastRow;
            sel.id = 'optRow2-' + lastRow;
            sel.id.enable = false;
            sel.class = "drpClass";
            sel.size = 1;
            sel.width = 177;
            for (var key in scoreValues) {


                var option = document.createElement("option");
                option.text = scoreValues[key];
                option.id = scoreValues[key].id;


                //option.value = key.value;
                try {

                    sel.add(option, null);
                    sel.enable = false;  //Standard 
                } catch (error) {

                    sel.add(option); // IE only
                }
            }

            sel.onchange = function () {
                var itId = this.id;
                var str = itId.split('-');
               
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                secondColumn[ind] = itemName.options[itemName.selectedIndex].value;
                alert(secondColumn[ind]);

            }

            cellRight2.appendChild(sel);
        }

        else if (measureType[1] == "+ / -") {
            var radioYes = document.createElement("input");
            radioYes.setAttribute("type", "radio");

            /*Set id of new created radio button*/
            radioYes.setAttribute("id", "radioPlus2-" + lastRow);

            /*set unique group name for pair of Yes / No */
            radioYes.setAttribute("name", "Measure2" + lastRow);

            radioYes.onclick = function () {

                var itId = this.id;
                var str = itId.split('-');
              
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                secondColumn[ind] = "+";

               

            }

            /*creating label for Text to Radio button*/
            var lblYes = document.createElement("lable");

            /*create text node for label Text which display for Radio button*/
            var textYes = document.createTextNode("+");

            /*add text to new create lable*/
            lblYes.appendChild(textYes);

            /*add radio button to Div*/
            cellRight2.appendChild(radioYes);

            /*add label text for radio button to Div*/
            cellRight2.appendChild(lblYes);

            /*add space between two radio buttons*/
            var space = document.createElement("span");
            space.setAttribute("innerHTML", "&nbsp;&nbsp");
            cellRight2.appendChild(space);
            cellRight2.appendChild(space);

            var radioNo = document.createElement("input");
            radioNo.setAttribute("type", "radio");
            radioNo.setAttribute("id", "radioMinus2-" + lastRow);
            radioNo.setAttribute("name", "Measure2" + lastRow);


            radioNo.onclick = function () {

                var itId = this.id;
                var str = itId.split('-');
              
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                secondColumn[ind] = "-";

            }

            var lblNo = document.createElement("label");
            lblNo.innerHTML = "-";
            cellRight2.appendChild(radioNo);
            cellRight2.appendChild(lblNo);



        }


        var cellRight3 = row.insertCell(4);


        if (measureType[2] == "Prompt List") {

            //var textNode1 = document.createTextNode(temp_desc);
            var sel = document.createElement('select');
            sel.type = 'select';
            sel.name = 'select' + lastRow;
            sel.id = 'optRow3-' + lastRow;
            sel.id.enable = false;
            sel.class = "drpClass";
            sel.size = 1;
            sel.width = 177;
            for (var key in scoreValues) {


                var option = document.createElement("option");
                option.text = scoreValues[key];
                option.id = scoreValues[key].id;


                //option.value = key.value;
                try {

                    sel.add(option, null);
                    sel.enable = false;  //Standard 
                } catch (error) {

                    sel.add(option); // IE only
                }
            }

            sel.onchange = function () {

                var itId = this.id;
                var str = itId.split('-');
             
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                thirdColumn[ind] = itemName.options[itemName.selectedIndex].value;

            }

            cellRight3.appendChild(sel);
        }

        else if (measureType[2] == "+ / -") {
            var radioYes = document.createElement("input");
            radioYes.setAttribute("type", "radio");

            /*Set id of new created radio button*/
            radioYes.setAttribute("id", "radioPlus3-" + lastRow);

            /*set unique group name for pair of Yes / No */
            radioYes.setAttribute("name", "Measure3" + lastRow);

            radioYes.onclick = function () {

                var itId = this.id;
                var str = itId.split('-');
              
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                thirdColumn[ind] = "+";

            }

            /*creating label for Text to Radio button*/
            var lblYes = document.createElement("lable");

            /*create text node for label Text which display for Radio button*/
            var textYes = document.createTextNode("+");

            /*add text to new create lable*/
            lblYes.appendChild(textYes);

            /*add radio button to Div*/
            cellRight3.appendChild(radioYes);

            /*add label text for radio button to Div*/
            cellRight3.appendChild(lblYes);

            /*add space between two radio buttons*/
            var space = document.createElement("span");
            space.setAttribute("innerHTML", "&nbsp;&nbsp");
            cellRight3.appendChild(space);
            cellRight3.appendChild(space);

            var radioNo = document.createElement("input");
            radioNo.setAttribute("type", "radio");
            radioNo.setAttribute("id", "radioMinus3-" + lastRow);
            radioNo.setAttribute("name", "Measure3" + lastRow);

            radioNo.onclick = function () {


                var itId = this.id;
                var str = itId.split('-');
               
                var itemName = document.getElementById(this.id);
                ind = parseInt(str[1]) - 1;
                thirdColumn[ind] = "-";

             
            }

            var lblNo = document.createElement("label");
            lblNo.innerHTML = "-";
            cellRight3.appendChild(radioNo);
            cellRight3.appendChild(lblNo);



        }


        var cellRight4 = row.insertCell(5);
        var checkbox = document.createElement('input');
        checkbox.setAttribute("type", "checkbox");
        //        checkbox.name = "name";
        //        checkbox.value = "value";
        checkbox.setAttribute("id", "chk-" + lastRow);

        checkbox.onclick = function () {

            var itId = this.id;
            var str = itId.split('-');
          
            var itemName = document.getElementById(this.id);
            
            ind = parseInt(str[1]) - 1;
            if (itemName.checked == true)
            {
                fourthColumn[ind] = "Y";
            }
            if (itemName.checked == false) {
                fourthColumn[ind] = "N";
            }

           
        }
        cellRight4.appendChild(checkbox);


        var cellRight5 = row.insertCell(6);
        var el = document.createElement('input');
        el.setAttribute('type', 'text');
        el.setAttribute("value", "00:00:0");
        //el.setAttribute("name", measureTime[index]);
        el.setAttribute('id', "measureTime" + lastRow);
        // el.setAttribute('class', 'gridButton');
        
        cellRight5.appendChild(el);

        var timeId = null;

        var cellRight6= row.insertCell(7);

        var button = document.createElement('input');
        button.setAttribute('type', 'button');
        button.setAttribute("value", "S t a r t");
        button.setAttribute("name", "btnTimer" + lastRow);
        button.setAttribute('id', "btnStartStop-" + lastRow);
        button.onclick = function () {
            
            var btnId = this.id;
            var startstop = document.getElementById(btnId);

            var str = btnId.split('-');
          
            argIndex = str[1];
            ind = parseInt(str[1]) - 1;
            var timeId = document.getElementById("measureTime" + argIndex);
           
            var startdate = new Date();
            var starttime = startdate.getTime();
            if (flagclocks == 0) {
                startstop.value = 'S t o p';
                startstop.innerHTML = "S t o p";
                flagclocks = 1;
                counterIn(starttime);
            }
            else {
                
                startstop.value = 'S t a r t';
                startstop.innerHTML = "S t a r t";
                flagclocks = 0;
                flagstops = 1;
                time[ind] = timeId.value;
              
                resetclock();
                timeId.value = time[ind];
            }

        }

        cellRight6.appendChild(button);


       

        var cellRight7 = row.insertCell(8);
        var button = document.createElement('input');
        button.setAttribute('type', 'button');
        button.setAttribute("value", "Note");
        button.setAttribute("name", "btnNote");
        button.setAttribute('id', "btnNote-" + lastRow);
        button.onclick = function () {

            var btnId = this.id;
            var startstop = document.getElementById(btnId);
            var str = btnId.split('-');
            popUpInd = parseInt(str[1]) - 1;
            //centering with css
            centerPopup();
            //load popup
            loadPopup();

        }
        cellRight7.appendChild(button);

        lastRow = tbl.rows.length;
    }



}








    var popupStatus = 0;

    function loadPopup() {
        if (popupStatus == 0) {
            $("#backgroundPopup").css({
                "opacity": "0.7"
            });
            $("#backgroundPopup").fadeIn("slow");
            $("#myPopup").fadeIn("slow");
            popupStatus = 1;
        }
    }

    function disablePopup() {
        if (popupStatus == 1) {
            $("#backgroundPopup").fadeOut("slow");
            $("#myPopup").fadeOut("slow");
            popupStatus = 0;
        }
    }

    //centering popup
    function centerPopup() {
        //request data for centering
        var windowWidth = document.documentElement.clientWidth;
        var windowHeight = document.documentElement.clientHeight;
        var popupHeight = $("#myPopup").height();
        var popupWidth = $("#myPopup").width();


        $("#myPopup").css({
            "position": "absolute",
            "top": windowHeight / 2 - popupHeight / 2,
            "left": windowWidth / 2 - popupWidth / 2
        });

        $("#backgroundPopup").css({
            "height": windowHeight
        });
    }


    $(document).ready(function () {

        var popupBtn = document.getElementById('btnDone');
        popupBtn.onclick = function () {

            disablePopup();
            var value = document.getElementById('txtNote').value;
            //alert(value);
            msg[popUpInd] = value;
          
            document.getElementById('txtNote').value = "";
        }


        $("#displaypopup").click(function () {
            //centering with css
            centerPopup();
            //load popup
            loadPopup();
        });

        //CLOSING POPUP
        //Click the x event!
        $("#popupClose").click(function () {
            disablePopup();
        });
        //Click out event!
        $("#backgroundPopup").click(function () {
            disablePopup();
        });
        //Press Escape event!
        $(document).keypress(function (e) {
            if (e.keyCode == 27 && popupStatus == 1) {
                disablePopup();
            }
        });

    });




    function checks() {
        if (document.getElementById('chkMissSession').checked == true) {
            parameter[3] = "Y";
            alert("Y");
        }
        else {
            parameter[3] = "N";
            alert("N");
        }
    }

    

    

    var parameter=new Array();

    function saveSessionHeader() {

        var d = new Date();

        d.setDate(new Date().getDate());

        var endTime = d.toTimeString().substr(0, d.toTimeString().lastIndexOf(" "));
        var strings = document.getElementById('txtAreaComments').value;
        var comment = strings.replace(/,/g, "^");
        //alert(comment);

        if (document.getElementById('chkMissSession').checked == true) {
            parameter[3] = "Y";
           
        }
        else {
            parameter[3] = "N";
          
        }


        parameter[0] = startTime;
        parameter[1] = endTime;
        parameter[2] = comment;

       
        $.ajax(
           {

               type: "POST",
               url: "Template.aspx/submitSession",
               data: "{'parameter1':'" + parameter + "'}",
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               async: false,
               success: function (data) {

                   saveSessionValues();

                   var list = jQuery.parseJSON(data.d);

                   $.each(list, function (index, value) {

                     
                       





                   });
                   

               },
               error: function (request, status, error) {
                   alert("Error");
               }
           });

    }


    function saveSessionValues() {
        var tbl = document.getElementById('tblDiscrete');
        var rowCount = tbl.rows.length;
        $.ajax(
          {

              type: "POST",
              url: "Template.aspx/submitSessionValues",
              data: "{'parameter':'" + rowCount + "','parameter1':'" + colName + "','parameter2':'" + firstColumn + "','parameter3':'" + secondColumn + "','parameter4':'" + thirdColumn + "','parameter5':'" + fourthColumn + "','parameter6':'" + time + "','parameter7':'" + msg + "'}",
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              async: false,
              success: function (data) {

                 
                  var list = jQuery.parseJSON(data.d);

                  $.each(list, function (index, value) {








                  });


              },
              error: function (request, status, error) {
                  alert("Error");
              }
          });

    }



