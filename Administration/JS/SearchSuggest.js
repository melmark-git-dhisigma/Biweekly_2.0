// JScript File
var maxDivId, currentDivId, strOriginal, setOrig;

//Our XmlHttpRequest object to get the auto suggestvar 
searchReq = getXmlHttpRequestObject();

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


//Called from keyup on the search textbox.//Starts the AJAX request.
function searchSuggest(e) {



    var key = window.event ? e.keyCode : e.which;


    if (key == 40 || key == 38 || key == 13) {
        scrolldiv(key);
    }
    else {
        if (searchReq.readyState == 4 || searchReq.readyState == 0) {
            var str = escape(document.getElementById('ctl00_txtSearch').value);

            strOriginal = str;

            searchReq.open("GET", 'Result.aspx?search=' + str, true);
            searchReq.onreadystatechange = handleSearchSuggest;
            searchReq.send(null);
        }

    }



}


//Called when the AJAX response is returned.
function handleSearchSuggest() {
    if (searchReq.readyState == 4) {
        var ss = document.getElementById('search_suggest');
        ss.innerHTML = '';
        var str = searchReq.responseText.split("~");

        if (str.length > 1) {

            for (i = 0; i < str.length - 1; i++) {
                //Build our element string.  This is cleaner using the DOM, but			
                //IE doesn't support dynamically added attributes.


                var split = str[i].split('***');

                var name = split[0];
                var rem = split[1];

                maxDivId = i;
                currentDivId = -1;
                var suggest = '<div id=div' + i + ' ;  onclick="javascript:setSearch(this.innerHTML);"  onmouseover="javascript:suggestOver(this);"  onmouseout="javascript:suggestOut(this);"  >'

                suggest += '<a>' + split[0] + ' </a>';
                suggest += '  <a style="display:none;">**' + split[1] + ' </a></div> ';
                ss.innerHTML += suggest;

                ss.style.visibility = 'visible';
            }
        }
        else {

            ss.style.visibility = 'hidden';
        }
    }

}

//Mouse over function
function suggestOver(div_value) {
    div_value.className = 'suggest_link_over';

}

function scrollOver(div_value) {

    var name = div_value.innerHTML;
    var split = name.split('</a>');
    var name = split[0].replace("<a>", "");

    var split1 = split[1].split('**');
    
    var fill = split1[1].split('-');
    //document.getElementById('ctl00_lblDob').value ="DOB : "+ fill[1];
   // document.getElementById('ctl00_lblGrade').value = "Grade : "+fill[2];
    document.getElementById('ctl00_lblGender').value = "Gender : " + fill[0];
    
    var Photo = fill[3].replace(" ", "");
    //document.getElementById('ctl00_ImgStudent').src = "StudentsPhoto/" + Photo;
    setOrig = div_value;
    
    div_value.className = 'suggest_link_over';
    var name = name.replace(" ", "");
    document.getElementById('ctl00_txtSearch').value = name;
    document.getElementById('ctl00_txtSearch').focus();
}

//Mouse out function
function suggestOut(div_value) {
    div_value.className = 'suggest_link';
}

function OutDiv() {
    var ss = document.getElementById('search_suggest');
    ss.innerHTML = '';
    ss.style.visibility = 'hidden';
}
//Click function
function setSearch(value) {

    var ss = document.getElementById('search_suggest');
   
    var n = value.replace("<a>", "");
    var n = n.replace("</a>", "");
    var n = n.replace("</a>", "");
    var split = n.split('**');
    var name = split[0];
    var rem = split[1];
    var name = name.replace("<a>", "");
    var var1 = n.split('  ');
    var fill = rem.split('-');
   
   // document.getElementById('ctl00_lblDob').value = "DOB : " + fill[1];
   // document.getElementById('ctl00_lblGrade').value = "Grade :  " + fill[2];
    document.getElementById('ctl00_lblGender').value = "Gender : " + fill[0];
    document.getElementById('ctl00_txtSearch').value = var1[0];
    var Photo = fill[3].replace(" ", "");
   
    //document.getElementById('ctl00_ImgStudent').src = "StudentsPhoto/" + Photo;
   // var ImageUrl = document.getElementById('ctl00_ImgStudent').src;
    document.getElementById('ctl00_txtSearch').focus();
    var str = escape(document.getElementById('ctl00_txtSearch').value);
    ss.innerHTML = '';
    ss.style.visibility = 'hidden';
    var split = str.split('-');
    
    searchReq.open("GET", 'Result.aspx?searchN=' + split[1] + "&dob=" + fill[1] + "&Grade=" + fill[2] + "&Gender=" + fill[0] + "&Photo=" + ImageUrl + "&Name=" + var1[0], true);
    searchReq.onreadystatechange = handleSearchSuggest;
    searchReq.send(null);
   

}

function scrolldiv(key) {
    var tempID;


    if (key == 40) {

        if (currentDivId == -1) {
            scrollOver(div0);
            currentDivId = 0;
        }
        else {
            if (currentDivId == maxDivId) {
                tempID = 'div' + maxDivId;

                var a = document.getElementById(tempID);
                currentDivId = -1;
                suggestOut(a)

                document.getElementById('ctl00_txtSearch').focus();
                document.getElementById('ctl00_txtSearch').value = name;
            }
            else {
                tempID = currentDivId + 1;
                setScroll(currentDivId, tempID)
            }

        }
    }
    else if (key == 38) {
        if (currentDivId == -1) {
            tempID = maxDivId;
            setScroll(maxDivId, maxDivId)
        }
        else {
            if (currentDivId == 0) {
                tempID = 'div' + currentDivId;
                var a = document.getElementById(tempID);
                currentDivId = -1;
                suggestOut(a)
                document.getElementById('ctl00_txtSearch').value = name;

            }
            else {
                tempID = currentDivId - 1;
                setScroll(currentDivId, tempID)

            }

        }


    }
    else if (key == 13) {
        setSearch(setOrig.innerHTML)
        document.getElementById('ctl00_txtSearch').focus();
    }
}
function setScroll(Old, New) {
    var tempDivId;
    currentDivId = New;

    tempDivId = 'div' + Old;
    var a = document.getElementById(tempDivId);
    suggestOut(a)

    tempDivId = 'div' + currentDivId;
    var b = document.getElementById(tempDivId);
    scrollOver(b);

}



