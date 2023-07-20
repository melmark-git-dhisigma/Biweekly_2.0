<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP4.aspx.cs" Inherits="StudentBinder_CreateIEP4" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <meta charset="utf-8">
    <title></title>
    <script src="../Administration/JS/jquery-1.8.0.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.ui.core.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.ui.sortable.js" type="text/javascript"></script>

    <style type="text/css">
        #sortable {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 60%;
        }

        .txtStyle {
            display: none;
        }

        #sortable li {
            margin: 0 3px 3px 3px;
            padding: 0.4em;
            padding-left: 1.5em;
            font-size: 1.4em;
            height: 18px;
        }

            #sortable li span {
                position: absolute;
                margin-left: -1.3em;
            }

        .auto-style4 {
            font-family: Arial;
            color: #666666;
            line-height: 22px;
            font-weight: bold;
            font-size: 12px;
            padding-right: 1px;
            text-align: left;
            height: 35px;
            width: 190px;
        }

        .auto-style5 {
            height: 41px;
        }


        .FreeTextDivContent {
            width: 97%;
            min-height: 100px;
            height: 100px;
            padding: 0 5px 0 10px;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
            overflow: auto;
        }

        #dlGoals .goalTitle {
            background-color: #0D668E;
            color: #FFFFFF;
            font-size: small;
            padding: 5px 0;
            width: 832px;
        }
        #dlGoals2 .goalTitle {
            background-color: #0D668E;
            color: #FFFFFF;
            font-size: small;
            padding: 5px 0;
            width: 500px;
            float:left;
        }
        .goalBox {
        height:30px;
        }

        .web_dialog22 {
            display: none;
            position: fixed;
            min-width: 630px;
            min-height: 200px;
            left: 0;
            top: 0;
            margin: 1%;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            width: 92%;
            height: auto;
            top: 9%;
        }

        .web_dialog33 {
            display: none;
            position: fixed;
            min-width: 630px;
            min-height: 75px;
            margin-left: -315px;
            margin-top: 15px;
            font-size: 100%;
            font-family: Arial, Helvetica, sans-serif;
            color: #333;
            z-index: 1001;
            background: #f8f7fc url(../Administration/images/smalllgomlmark.JPG) right bottom no-repeat;
            padding: 5px 5px 30px 5px;
            border: 5px solid #b2ccca;
            width: 640px;
            height: auto;
            top: 1%;
            left:50%;

        }
    </style>
    <script type="text/javascript">

        $(function () {
            $("#sortable").sortable();
            $("#sortable").disableSelection();
        });
    </script>


    <script type="text/javascript">

        function GetFreetextval(content, divid, textid) {
            document.getElementById(divid).innerHTML = "";
            document.getElementById(divid).innerHTML = content;
            content = content.replace(/\>/g, '&gt;');
            content = content.replace(/\</g, '&lt;');
            document.getElementById(textid).value = "";
            //document.getElementById(textid).value = window.escape(content);
            document.getElementById(textid).value = content;

        }


        jQuery(document).ready(function () {
            jQuery(".content").hide();
            //toggle the componenet with class msg_body
            jQuery(".heading").click(function () {
                jQuery(this).next(".content").slideToggle(500);
            });
        });

        function DrpChangeOrder(id) {

            var sortNum = parseInt($(id).find("option:selected").text());
            var alldrp = parseInt($('#dlGoals').find("option:selected").text());




            var prevSortnumber = parseInt($(id).parent().find('.divHfPrev').find('input').val());
            var flag = 0;
            var alldrpIds = $('.clsDropSort');

            if (prevSortnumber < sortNum) {
                for (var i = 0; i < alldrpIds.length; i++) {

                    if ($(alldrpIds[i]).val() > prevSortnumber && $(alldrpIds[i]).val() <= sortNum) {

                        $(alldrpIds[i]).val(parseInt($(alldrpIds[i]).val()) - 1);

                        $(alldrpIds[i]).parent().find('.divHfPrev').find('input').val(parseInt($(alldrpIds[i]).val()));
                    }
                }
            }
            else {

                for (var i = 0; i < alldrpIds.length; i++) {


                    if ($(alldrpIds[i]).val() < prevSortnumber && $(alldrpIds[i]).val() >= sortNum) {


                        $(alldrpIds[i]).val(parseInt($(alldrpIds[i]).val()) + 1);

                        $(alldrpIds[i]).parent().find('.divHfPrev').find('input').val(parseInt($(alldrpIds[i]).val()));
                    }
                }
            }
            $(id).val(sortNum);
            $(id).parent().find('.divHfPrev').find('input').val(sortNum);



        }

        // function ResetGoalSortOrder() {


        //   }
        function BtnSortGoal() {
            //alert($("#divListPopupSort").length);
            $("#divListPopupSort").show();
            //$("#divListPopupSort").slideToggle();

        }
        function CloseGoalSortPop() {

            $("#divListPopupSort").hide();
        }

        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 100);
        }

    </script>


</head>
<body>
    <form id="form1" runat="server">
        <div class="demo">
        </div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

        <asp:HiddenField ID="hfGoalId" runat="server" />
        <asp:HiddenField ID="hfGoalName" runat="server" />
        <asp:HiddenField ID="hfGoalIdAdded" runat="server" />
        <asp:HiddenField ID="hfGoalNameAdded" runat="server" />
        <div id="divIep2" style="width: 97%; border-radius: 3px 3px 3px 3px; padding: 7px;">
            <table cellpadding="0" cellspacing="0" width="95%">


                <tr>
                    <td colspan="2" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 25px;">Current Performance Levels/Measurable Annual Goals
                        
                    </td>
                    <td>

                        <img src="images/sort.png" style="height: 25px; width: 25px; cursor: pointer;" onclick="BtnSortGoal();" />
                    </td>
                </tr>

                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td id="tdMsg" runat="server" colspan="2"></td>
                </tr>
                <tr>
                    <td align="center" colspan="2" class="auto-style5"></td>
                </tr>




                <tr>
                    <td align="center" style="vertical-align: top">

                        <div class="layer1">

                            <asp:RadioButtonList ID="chkGoal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkGoal_SelectedIndexChanged" Visible="False">
                            </asp:RadioButtonList>

                        </div>

                    </td>
                    <td align="center" style="vertical-align: top">

                        <div class="layer1">
                        </div>
                    </td>
                </tr>

                <tr>
                    <%-- Testing --%>

                    <%-- Testing_Close --%>
                </tr>



                <tr>
                    <td align="center" colspan="2">

                        <div id="divIEP4Inner" style="border: 1px double #C2C2C2; width: 836px;">

                            <asp:DataList ID="dlGoals" runat="server" Width="100%" OnItemDataBound="dlGoals_ItemDataBound">
                                <ItemTemplate>
                                    <table width="100%" bgcolor="white" style="overflow: hidden;" cellpadding="0px">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <asp:HiddenField ID="hfStdtLessonPlanId" runat="server" Value='<%#Eval("StdtLessonPlanId") %>' />
                                                <asp:HiddenField ID="hfGoalId" runat="server" Value='<%#Eval("GoalId") %>' />
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hfLessonPlanId" runat="server" Value='<%# Eval("LessonPlanId") %>' />
                                                            <asp:Label ID="lblGoool" runat="server" Font-Names="Arial" Font-Size="12px" Font-Bold="true" ForeColor="#116C90" Text='<%# Eval("Title") %>'></asp:Label>
                                                        </td>
                                                        
                                                         <%--<td>

                                                            <asp:DropDownList ID="drpSortOrder" Class="clsDropSort" runat="server" onchange="DrpChangeOrder(this);"  >
                                                                
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hfIEPGoalNo"  runat="server" Value='<%#Eval("IEPGoalNo") %>' />
                                                            <div class="divHfPrev">
                                                             <asp:HiddenField ID="hfPrevIEPGoalNo"  runat="server" Value='<%#Eval("IEPGoalNo") %>' />
                                                                </div>
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="hfIEPNo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtIEPGoalNote" runat="server" Text='<%#Eval("GoalIEPNote") %>' TextMode="MultiLine" Rows="3" width="97%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:DataList ID="dlCPLevel" runat="server" Width="100%" OnItemDataBound="dlCPLevel_ItemDataBound">
                                                    <ItemTemplate>
                                                        <table width="100%" bgcolor="white" style="overflow: hidden;" cellpadding="0px">
                                                            <tr>
                                                                <td align="left" style="background: #ededed url(../images/topbtmline.JPG) right top repeat-y; border-bottom: 1px dotted #116c90;" colspan="2">
                                                                    <asp:Label ID="lblGoool" runat="server" Font-Bold="true" Font-Names="Arial" Font-Size="12px" ForeColor="#116C90" Text='<%# Eval("LessonPlanName") %>'></asp:Label>
                                                                </td>

                                                            </tr>
                                                            <tr>
                                                                <td class="auto-style4" style="border-left-style: groove; border-top-color: #FFFFFF; border-right-color: #FFFFFF; border-bottom-color: #FFFFFF; border-left-color: C2C2C2; border-width: 1px 0px 0px 0px" vi="">Current Performance Level&nbsp; </td>
                                                                <td class="tdText" style="padding: 0px">
                                                                    <div id="ObjectiveA" runat="server" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP4.aspx',this); ">
                                                                        <asp:TextBox ID="txtObjectiveA1" runat="server" CssClass="txtStyle" TextMode="MultiLine"></asp:TextBox>
                                                                        <asp:HiddenField ID="hfStdtLessonPlanId" runat="server" Value='<%#Eval("StdtLessonPlanId") %>' />



                                                                        <div id="txtObjectiveA" runat="server" class="FreeTextDivContent">
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style4" style="border-left-style: groove; border-top-color: #FFFFFF; border-right-color: #FFFFFF; border-bottom-color: #FFFFFF; border-left-color: C2C2C2; border-width: 1px 0px 0px 0px">Measurable Annual Goal&nbsp; </td>
                                            <td class="tdText" style="padding: 0px">
                                                <div id="ObjectiveB" runat="server" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP4.aspx',this); ">
                                                    <asp:TextBox ID="txtObjectiveB1" runat="server" CssClass="txtStyle" TextMode="MultiLine"></asp:TextBox>
                                                    <div id="txtObjectiveB" runat="server" class="FreeTextDivContent">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="auto-style4" style="border-left-style: groove; border-top-color: #FFFFFF; border-right-color: #FFFFFF; border-bottom-color: #FFFFFF; border-left-color: C2C2C2; border-width: 1px 0px 0px 0px">Benchmark/Objectives&nbsp;
                                            </td>
                                            <td class="tdText" style="padding: 0px">
                                                <div id="ObjectiveC" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP4.aspx',this); " runat="server">
                                                    <asp:TextBox ID="txtObjectiveC1" runat="server" CssClass="txtStyle" TextMode="MultiLine"></asp:TextBox>
                                                    <div runat="server" id="txtObjectiveC" class="FreeTextDivContent"></div>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>

                                </ItemTemplate>
                            </asp:DataList>


                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" height="25" colspan="2"></td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btn_Update" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue"
                            OnClick="btn_Update_Click" />

                        <%-- <asp:Button ID="btn_Update_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy"
                            OnClick="btn_Update_hdn_Click" style="display:none;" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divListPopupSort" class="web_dialog33">
            <a onclick="CloseGoalSortPop();" href="#" style="margin-top: -13px; margin-right: -14px;">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
            <table width="100%">
                <tr>
                    <td>
                   
                    <asp:DataList ID="dlGoals2" runat="server" Width="100%" OnItemDataBound="dlGoals2_ItemDataBound" RepeatLayout="Flow">
                        <ItemTemplate>
                            
                                <div class="goalBox">
                                    <asp:HiddenField ID="hfSortGoalId" runat="server" Value='<%#Eval("GoalId") %>' />
                                    <asp:Label ID="lblGoool" runat="server" Font-Names="Arial" Font-Size="12px" Font-Bold="true" ForeColor="#116C90" Text='<%# Eval("Title") %>'></asp:Label>
                                    <asp:DropDownList ID="drpSortOrder" Class="clsDropSort" runat="server" onchange="DrpChangeOrder(this);" Visible="false"></asp:DropDownList>
                                    <asp:TextBox runat="server"  Class="sortText" ID="drpSortOrder_txt" style="float:left;width:50px;"></asp:TextBox>
                                    <img src="images/UPArrow.png" style="height: 20px; width: 20px; cursor: pointer;" onclick="goUp(this);" />
                                    <img src="images/DownArrow.png" style="height: 20px; width: 20px; cursor: pointer;" onclick="goDown(this);" />



                                    <asp:HiddenField ID="hfIEPGoalNo" runat="server" Value='<%#Eval("IEPGoalNo") %>' />
                                    <div class="divHfPrev">
                                        <div class="divHfPrev">
                                            <asp:HiddenField ID="hfPrevIEPGoalNo" runat="server" Value='<%#Eval("IEPGoalNo") %>' />
                                        </div>
                                    </div>
                                </div>
                        </ItemTemplate>
                        <FooterTemplate>
                        <asp:Label Visible='<%#bool.Parse((dlGoals2.Items.Count==0).ToString())%>' runat="server" ID="lblNoRecord" Text="&nbsp;&nbsp;&nbsp;No Data Found!"></asp:Label>
                        </FooterTemplate>
                    </asp:DataList>
                        <div style="height:20px;"></div>
                        <div>
                            <asp:Button runat="server" ID="btnsaveorder" Text="Save Order" OnClick="btnsaveorder_Click" CssClass="NFButton" />
                        </div>


                    </td>
                </tr>

            </table>
        </div>
    </form>
</body>
<script type="text/javascript">
    $(document).ready(function () {
        var drpList = $('.clsDropSort');

        var txtList = $('.sortText');


        var txtTitileGoalNumPopup = $('#divListPopupSort').find('.goalTitleNumber');
        var txtTitileGoalNum = $('#dlGoals').find('.goalTitleNumber');
        
        var len = drpList.length;
        var flag = 1;
        for (var i = 0; i < len; i++) {

            $(drpList[i]).val(flag);
            $(drpList[i]).parent().find('.divHfPrev').find('input').val(flag);
            $(drpList[i]).parent().find('input').val(flag);
            flag++;
        }

        var len2 = txtList.length;
        var flag2 = 1;
        //alert(len2);
        for (var i = 0; i < len2; i++) {
            
            $(txtTitileGoalNumPopup[i]).text(flag2);
            $(txtTitileGoalNum[i]).text(flag2);
            $(txtList[i]).val(flag2);
            flag2++;
            $(txtList[i]).prop("readonly", true);
        }


        $("#dlGoals2").find('br').remove();
    });

    function goUp(sortId) {
        var thisContainer = $(sortId).parents('.goalBox');
        var clkSortId = $(thisContainer).find('.sortText').val();
        var nextSortId = parseInt(clkSortId) - 1;

        if (nextSortId > 0) {
            var allTxtBoxes = $("#dlGoals2").find(".sortText");

            for (var i = 0; i < allTxtBoxes.length; i++) {
                if ($(allTxtBoxes[i]).val() == nextSortId) {
                    $(allTxtBoxes[i]).val(clkSortId);
                    break;
                }
            }

            $(thisContainer).find('.sortText').val(nextSortId);

            ///
            var $divs = $("div.goalBox").parent();
            // var $divs = $(".goalBox");
            var numericallyOrderedDivs = $divs.sort(function (a, b) {
                return parseInt($(a).find(".sortText").val()) - parseInt($(b).find(".sortText").val());
            });
            $("#dlGoals2").html(numericallyOrderedDivs);
        }

    }

    function goDown(sortId) {
        var thisContainer = $(sortId).parents('.goalBox');
        var clkSortId = $(thisContainer).find('.sortText').val();
        var nextSortId = parseInt(clkSortId) + 1;

        var allTxtBoxes = $("#dlGoals2").find(".sortText");

        if (nextSortId <= allTxtBoxes.length) {

            for (var i = 0; i < allTxtBoxes.length; i++) {
                if ($(allTxtBoxes[i]).val() == nextSortId) {
                    $(allTxtBoxes[i]).val(clkSortId);
                    break;
                }
            }

            $(thisContainer).find('.sortText').val(nextSortId);

            ///
            var $divs = $("div.goalBox").parent();
            // var $divs = $(".goalBox");
            var numericallyOrderedDivs = $divs.sort(function (a, b) {
                return parseInt($(a).find(".sortText").val()) - parseInt($(b).find(".sortText").val());
            });
            $("#dlGoals2").html(numericallyOrderedDivs);
        }

    }
</script>
</html>
