<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ACSheet.aspx.cs" Inherits="StudentBinder_ACSheet"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Untitled Document</title>
    

    <%-- <link href="CSS/CustomizeTemplate.css" rel="stylesheet" id="sized" />--%>


    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="CSS/AcSheet.css" rel="stylesheet" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <%--<script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="../Administration/JS/jquery.ui.datepicker.js"></script>--%>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
   <%-- <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>--%>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script src="../Administration/JS/jquery-ui.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />

     <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.min.1.3.js"></script>
    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.jquery.min.1.3.js"></script>

    <script src="jsScripts/jq1.js" type="text/javascript" charset="utf-8"></script>     
<script type="text/javascript">

    function updateColor(el) {
        el.parentNode.style.color = el.checked ? "green" : "#808080"
        el.parentNode.style.fontWeight = el.checked ? "bold" : "normal";
    }
    function updateColor2(el) {
        el.parentNode.style.color = el.checked ? "red" : "#808080"
        el.parentNode.style.fontWeight = el.checked ? "bold" : "normal";
    }

    function changeColorRadioBtnList(radioButtonList) {
        var selectedValue = radioButtonList.querySelector("input:checked").value;
        var listItems = radioButtonList.getElementsByTagName("label");
        switch (selectedValue) {
            case "Yes":
                listItems[0].parentNode.style.color = "lightgreen";
                listItems[1].parentNode.style.color = "";
                listItems[2].parentNode.style.color = "";
                break;
            case "No":
                listItems[0].parentNode.style.color = "";
                listItems[1].parentNode.style.color = "red";
                listItems[2].parentNode.style.color = "";

                break;
            case "No Change":
                listItems[0].parentNode.style.color = "";
                listItems[1].parentNode.style.color = "";
                listItems[2].parentNode.style.color = "yellow";
                break;
            default:
                break;
        }
    }
    function handleCheckboxChange(checkbox) {
        console.log("Checkbox ID: " + checkbox.id);
        var labelId = checkbox.id.replace("Checkbox", "c");
        var label = document.getElementById(labelId);
           
        
        if (checkbox.checked) {
            label.style.color = "Green";
            label.style.fontWeight = "bold";
        } else {
            label.style.color = "Gray";
            label.style.fontWeight = "normal";
        }
    }
</script>
    
    <script type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        
        
        function GetTextBoxValueTwo() {
            var result = true;
            $(".txtdeno").each(function () {                
                if ($(this).val() == "") {
                    $(this).focus();
                    $(this).css("border-color", "red");
                    $(this).css("border-style", "solid");
                    //txtnooftime.Style.Add("border-color", "red");
                    //txtnooftime.BorderStyle = BorderStyle.Solid;
                    result = false;
                }
                else {
                    $(this).css("border-style", "none");
                    //txtnooftime.BorderStyle = BorderStyle.None;
                }
            });
            return result;
        }

        window.onload = function () {
            $(function () {
                $(".DatePick").datepicker();
            });
        };

        function loadDateJqry() {
            $(function () {
                $(".DatePick").datepicker();
            });
        }

        function showWait() {
            $('#fullContents').fadeOut('fast');
            $('.loading').fadeIn('fast');

        }
        function HideWait() {
            $('#fullContents').fadeIn('fast');
            $('.loading').fadeOut('fast');

            $('#overlay').fadeIn('fast', function () {
                $('#PopDownload').css('top', '5%');
                $('#PopDownload').show();
            });


        }


       

         $(function () {

             adjustStyle();
             $(window).resize(function () {
                 adjustStyle();
             });
         });


         function adjustStyle() {
             var isiPad = navigator.userAgent.match(/iPad/i);

             if (isiPad != null) {


                 $('.area').css("width", '75%');
                 $('textarea').css('width', '80%');

             }
             else {


             }

         }

         


         $(document).ready(function () {
             $('#CancalGen').click(function () {
                 $('#dialog').animate({ top: "-300%" }, function () {
                     $('#overlay').fadeOut('slow');
                 });
             });
             //Upadte end date after start date change
             $("#txtSdate").datepicker({
                 dateFormat: "mm/dd/yy",
                 maxDate:0,
                 onSelect: function () {
                     var dt2 = $('#txtEdate');
                     var today = new Date();
                     var startDate = $(this).datepicker('getDate');
                     //add 98 days to selected date
                     startDate.setDate(startDate.getDate() + 98);
                     var minDate = dt2.datepicker('getDate');
                     if (new Date(today) < new Date(startDate)) {
                         dt2.datepicker('setDate', today);
                         dt2.datepicker('option', 'maxDate', today);
                     }
                     else
                         dt2.datepicker('setDate', startDate);
                     
                 }
             });
             //Upadte start date after end date change
             $('#txtEdate').datepicker({
                 dateFormat: "mm/dd/yy",
                 onSelect: function () {
                     var dt1 = $('#txtSdate');
                     var endDate = $(this).datepicker('getDate');
                     //add 30 days to selected date
                     endDate.setDate(endDate.getDate() - 98);
                     var minDate = $(this).datepicker('getDate');
                     //minDate of dt2 datepicker = dt1 selected day
                     dt1.datepicker('setDate', endDate);
                 }

             });

         });               

         function scrollToTop() {
             var result = GetTextBoxValueTwo(); 
             if (result == true) {
                 window.scrollTo(0, 0);
                 window.parent.parent.scrollTo(0, 0);
             }
             return result;
             
         }
         function showMessage() {
             $('.loading').show();
             $('#dialog').hide();
         }
         function Delete() {
             var flag;
             flag = confirm("Are you sure you want to delete this coversheet?");
             return flag;
         }

         function h2click(h2, IEPid) {
            document.getElementById('<%=tdMsg.ClientID %>').innerHTML = "";
            // if section is already open, return false
            if ($(h2).is('.active')) {
            return false;
            }
            else {
                $(h2).addClass("active");
            }

            // open request and close open
            $(h2).parent().parent().find('.accordion > h2, .accordion > h3').removeClass("active");
            // fix IE 6 bug.
            if (jQuery.browser.msie && jQuery.browser.version < 7) {
                $('.accordion div').addClass('iefix');
            }
            return false;
        }

        

    </script>


    <style type="text/css">
         .ui-datepicker {
            font-size: 8pt !important;
        }
        .auto-style2 {
            height: 23px;
        }

        #divAllContainer {
            width: 100%;
            margin-left: auto;
            margin-right: auto;
            min-width: 900px;            
        }

        .gridAlgn {
            width: 100%;
            /*margin-left:4%;*/
            margin-right: auto;
        }

        .loading {
            display: none;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
        }

        #fullContents {
            display: block;
        }

        .innerLoading {
            margin: auto;
            height: 50px;
            width: 250px;
            text-align: center;
            font-weight: bold;
            font-size: large;
        }
        
        .innerLoading img {
            margin-top: 200px;
            height: 10px;
            width: 100px;
        }

        .bor {
            border-collapse: collapse;
            border: 1px solid #fff; 
        }

        .grmb {
            background: url("images/lbtngrngray1.PNG") no-repeat scroll 0 0 rgba(0, 0, 0, 0);
            height: 22px;
            padding-top: 8px;
            padding-left: 20px;
            text-decoration: none;
            width: 210px;
        }

        .auto-style3 {
            height: 93px;
        }
        .auto-style4 {
            height: 93px;
            width: 411px;
        }

        </style>
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
        

       
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       
        <div>

            <div class="boxContainerContainer">
                <div class="clear"></div>
                <table style="width: 100%;">
                    <tr>
                        <td id="tdReadMsg" runat="server"></td>
                    </tr>
                </table>
                <div class="clear"></div>
                <!-------------------------Top Container Start----------------------->
                <div class="itlepartContainer">

                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 20%;">
                                <h2>1 <span>Choose Date</span></h2>
                            </td>
                            <td style="width: 30%;">
                                <h2>2 <span>Academic Coversheets</span></h2>
                            </td>

                            <td style="width: 40%; text-align: right;">
                                <img runat="server" alt="Please Wait..." id="PlzWait" src="../Administration/images/PleaseWait.gif" style="width: 40px; height: 40px; display: none" />                              
                                <%--<asp:Button ID="btnImport" Style="float: right; display: inline; margin:1px;" runat="server" CssClass="NFButtonWithNoImage" OnClientClick="showWait();" OnClick="btnImport_Click" Text="Export To Word" />                           --%>
                               
                             </td>
                             <td>
                                <asp:Button ID="btnUpdateNew" Style="float: right; display: inline; margin:1px;" runat="server" CssClass="NFButtonWithNoImage" OnClick="btnUpdate_Click" Text="Update" ValidationGroup="Group1" OnClientClick="return scrollToTop();"/>                                 
                                <asp:Button ID="btnSaveNew" Style="float: right; display: inline; margin:1px;" runat="server" CssClass="NFButtonWithNoImage" OnClick="Save_Click" Text="Save" OnClientClick="return scrollToTop();"/>                                
                            </td>
                             
                                <td style="width: 30%; text-align: right;">
                                        <asp:Button ID="btnImport" runat="server" Text="Export" CssClass="NFButton" Style="float: right" OnClick="btnImport_Click" OnClientClick="showWait();" />

                                    </td>
                                    <td>
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="NFButton" Style="float: right;" OnClick="btnGenDelete_Click" OnClientClick="javascript:return Delete();"/>
									</td>
                            <td>
                                <asp:Button ID="btnGenNewSheet0" runat="server" CssClass="NFButton" ToolTip="Create new sheet" alt="Create New Document" OnClick="btnPreAccSheet1_Click" Text="Create New Coversheet" Style="width: auto;" />
                                <%--  <asp:Button ID="btnBack" runat="server" CssClass="Cancel" OnClick="btnBack_Click" ToolTip="Cancel" Text="" Style="float: right;" />--%>
                            </td>
                        </tr>
                    </table>

                    <%--  <h2>1 <span>Choose Date</span></h2>
                    <h2 style="margin-left:38px;">2 <span>Academic Coversheets</span></h2>
                    <h3 class="cf">


                         <asp:Button ID="btnGenNewSheet0" runat="server" style="margin-right:99px; display:inline;" CssClass="NFButtonWithNoImage" OnClick="btnPreAccSheet1_Click" Text="Generate New Sheets" />

                        <img runat="server" alt="Please Wait..." id="PlzWait" src="../Administration/images/PleaseWait.gif" style="width: 40px; height: 40px; display: none" />
                        <asp:Button ID="btnImport" Style="float: right; display: inline" runat="server" CssClass="NFButtonWithNoImage" OnClientClick="showWait();" OnClick="btnImport_Click" Text="Export To Word" />
                    </h3>--%>
                </div>
                <!-------------------------Top Container End----------------------->

                <!-------------------------Middle Container start----------------------->
                <div class="btContainerPart">
                    <!------------------------------------MContainer Start----------------------------------->

                    <div class="mBxContainer">
                        <h3>Date List</h3>
                        <div class="nobdrrcontainer">
                            <%--      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>
                               <asp:DataList ID="Dataacdsheetlist" runat="server" onitemdatabound="Dataacdsheetlist_ItemDataBound">
                                    <ItemTemplate>
                                    <asp:Label ID="lblAcCategory" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"EDate") %>' style="display:none;"></asp:Label>
                                        <li style='position: static;' class='accordion'>
                                            <h2 class='BG' onclick='h2click(this,'<%# DataBinder.Eval(Container.DataItem,"EDate") %>');'><a id='<%# DataBinder.Eval(Container.DataItem,"EDate") %>' class='kk' href='#' ><%# DataBinder.Eval(Container.DataItem,"EDate") %></a></h2>
                                                <div id='te' style='display: none;' class='wrapper nomar'>
                                                    <div class='nobdrrcontainer'>
                                                        <asp:DataList ID="dlAcdate" runat="server" OnItemCommand="lnkDate_Click">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDate" CssClass="grmb" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                            <div class='clear'></div>
                                                    </div>
                                                            <div class='clear'></div>
                                                </div>
                                       </li>
                                    </ItemTemplate>
                                    </asp:DataList>
                            <%--<asp:DataList ID="dlAcdate" runat="server">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDate" CssClass="grmb" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' OnClick="lnkDate_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>--%>

                            <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>

                        <div class="clear"></div>
                    </div>
                    <!------------------------------------MContainer End----------------------------------->

                    <!------------------------------------End Container Start----------------------------------->





                    <div class="righMainContainer large" id="MainDiv" runat="server" visible="true">
                            <tr>
                                <td id="tdMsg" runat="server" colspan="2">
                                </td>
                            </tr>
                        
                        <tr>
                                <td class="auto-style4" rowspan="2">
                                <asp:RadioButtonList ID="rbtnLsnClassTypeAc" runat="server" AutoPostBack="true" 
                                    OnSelectedIndexChanged="LessonTypeSelect" Width="222px" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Day">Day</asp:ListItem>
                                    <asp:ListItem Value="Residence">Residence</asp:ListItem>
                                    <asp:ListItem Value="Day,Residence" Selected="True">Both</asp:ListItem>
                                </asp:RadioButtonList>
                                &nbsp;</td>
                                </tr>

                        <table style="width: 100%">
                            
                            
                            
                            <tr>
                                <td id="tdMsg1" runat="server" class="auto-style4">
                                    <asp:Label ID="AttendeesLabel" runat="server" Text="Attendees :"></asp:Label>
                                    &nbsp;<br />
                                    <asp:TextBox ID="AttendeesText" runat="server" Height="57px" TextMode="MultiLine" Width="502px"></asp:TextBox>
                                </td>
                                <td id="tdMsg2" runat="server" class="auto-style3">
                                    <asp:Label ID="IepyearLabel" runat="server" Text="IEP/ISP Year"></asp:Label>
                                   &nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<asp:TextBox ID="Iepyeartxt" runat="server" Height="30px"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Label ID="Ieplbl" runat="server" Text="IEP/ISP Signature Date"></asp:Label>
                                    &nbsp;:&nbsp;
                                    <asp:TextBox ID="Ieptxt" runat="server" Height="30px"></asp:TextBox>
                                    </td>
                            </tr>
                            
                            </table>
                        <asp:UpdatePanel ID="UpdatePanelAgendaItem" runat="server">
                            <ContentTemplate>

                                                    <asp:GridView ID="gvAgendaItem" runat="server" Visible ="false"  AutoGenerateColumns="False" OnRowCommand="gvAgendaItem_RowCommand" 
                                                         BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="None" >
    <Columns>
        <asp:TemplateField HeaderText="Agenda Item Id" Visible ="false">
    <ItemTemplate>
        <asp:Label ID="LblAgendaItemId" runat="server" Text='<%# Eval("AgendaItemId") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
        <asp:TemplateField HeaderText="Agenda Item">
            <ItemTemplate>
                <asp:TextBox ID="txtAgendaItem" runat="server" Text='<%# Eval("AgendaItem") %>'></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Staff Initials">
    <ItemTemplate>
        <asp:TextBox ID="txtStaffInitials" runat="server" Text='<%# Eval("StaffInitials") %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>
        <asp:TemplateField HeaderText="Date Added">
    <ItemTemplate>
        <asp:TextBox ID="txtDateAdded" runat="server" Text='<%# Eval("DateAdded") %>'></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>
                <asp:TemplateField HeaderText="Discussed">
    <ItemTemplate>
        <asp:RadioButtonList ID="RBLDoneCarry" class="radBtnClass" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" >
            <asp:ListItem Text="Done" Value="Done" />
            <asp:ListItem Text="Carry-over" Value="Carryover" />
            </asp:RadioButtonList>
    </ItemTemplate>
</asp:TemplateField>
        <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelRowAgendaItem" runat="server" CommandName="deleteRow" CommandArgument='<%# Eval("AgendaItemId") %>' CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" OnClientClick="return confirm('Are you sure you want to delete?');" Text="X" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnAddRowAgendaItem" runat="server" CommandName="AddRow" CssClass="btn btn-blue" ImageUrl="~/Administration/images/plusNew.PNG" />
                        </ItemTemplate>
                    </asp:TemplateField>
    </Columns>
                                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
</asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table style="width: 100%">
                                <tr>
                            </tr>
                                
                            <tr>
                                <td colspan="2">

                                    <table style="width: 100%;">

                                        <tr>
                                            <td colspan="3">
                                                <asp:MultiView ID="MultiView1" runat="server">
                                                    <asp:View ID="first" runat="server">
                                                        <table style="width: 100%;">


                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:GridView Width="100%" ID="GridViewAccSheet" runat="server" 
                                                                        AutoGenerateColumns="False" OnRowDataBound="GridViewAccSheet_RowDataBound" 
                                                                        ShowHeader="False" 
                                                                        onselectedindexchanged="GridViewAccSheet_SelectedIndexChanged">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <table class="bor" style="width: 100%; background-color: #f7f5f5; margin: 5px 0 5px 0;">

                                                                                        <tr>
                                                                                            <td style="width:90px" class="tdText bor">Goal Area :</td>
                                                                                            <td style="text-align:left"  class="bor">
                                                                                                <asp:Label ID="lblGoalArea" style="float:left" runat="server" Text='<%#Eval("LessonPlanName") %>'></asp:Label>
                                                                                                <asp:HiddenField ID="hfLPId" runat="server" Value='<%#Eval("LessonPlanId") %>' />
                                                                                                <%--<asp:HiddenField ID="hdFldAcdIdNew" runat="server" Value='<%#Eval("AccSheetId") %>' />--%>
                                                                                            </td>                                                                                          
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td style="width:90px" class="tdText bor">Goal :</td>
                                                                                            <td style="text-align:left" class="bor">
                                                                                                <asp:Label ID="lblGoal" style="float:left" runat="server" Text='<%#Eval("GoalName") %>'></asp:Label>
                                                                                            </td>                                                                   
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td style="width:90px" class="tdText bor">Benchmarks/ Objectives:</td>
                                                                                            <td style="text-align:left" class="bor">
                                                                                                <div id="txtbenchaMark" style="float:left" runat="server" height="50px" width="640"><%#Eval("Objective3") %></div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width:90px" class="tdText bor">Benchmarks/ Objectives:</td>
                                                                                            <td id="td1" runat="server" colspan="2">
                                                                                                <label id="ch4" runat="server" style="color: #808080; ">
                                                                                                 <input type="Checkbox" runat="server" name="termsChkbx4" id="Checkbox4"  onclick="updateColor(this)"/>
                                                                                                 <span>Met Objective</span>
                                                                                                 </label>  
                                                                                                
                                                                                                <label id="ch5" runat="server" style="color: #808080; ">
                                                                                                 <input type="Checkbox" runat="server" name="termsChkbx5" id="Checkbox5" onclick="updateColor(this)"/>
                                                                                                 <span>Met Goal</span>
                                                                                                 </label>  
                                                                                                
                                                                                                <label id="ch6" runat="server" style="color: #808080; ">
                                                                                                 <input type="Checkbox" runat="server" name="termsChkbx6" id="Checkbox6" onclick="updateColor2(this)"/>
                                                                                                 <span>Not Maintaining</span>
                                                                                                 </label>                                                                                
                                                                                                    </td>
                                                                                        </tr>
                                                                                        </table>
                                                                                        <table style="width: 100%;">
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <table id="tabValReview" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                                                                    <tr class="HeaderStyle">
                                                                                                        <td>Review Period :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod1" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod2" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod3" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod4" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod5" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod6" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod7" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                            <%--<br />--%>
                                                                                                            <td style="background:#03507d; color:#fff;">Progressing :</td>
                                                                                                            <td style="background:#03507d; color:#fff;">
                                                                                                                <asp:RadioButtonList ID="RadioButtonList1" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                    <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                    <asp:ListItem Text="No" Value="No" />
                                                                                                                    <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td style="background:#03507d; color:#fff;">
                                                                                                                 <asp:RadioButtonList ID="RadioButtonList2" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                    <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                    <asp:ListItem Text="No" Value="No" />
                                                                                                                    <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td style="background:#03507d; color:#fff;">
                                                                                                                <asp:RadioButtonList ID="RadioButtonList3" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                    <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                    <asp:ListItem Text="No" Value="No" />
                                                                                                                    <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td style="background:#03507d; color:#fff;">
                                                                                                                <asp:RadioButtonList ID="RadioButtonList4" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                    <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                    <asp:ListItem Text="No" Value="No" />
                                                                                                                    <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td style="background:#03507d; color:#fff;">
                                                                                                                <asp:RadioButtonList ID="RadioButtonList5" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                    <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                    <asp:ListItem Text="No" Value="No" />
                                                                                                                    <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td style="background:#03507d; color:#fff;">
                                                                                                                <asp:RadioButtonList ID="RadioButtonList6" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                    <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                    <asp:ListItem Text="No" Value="No" />
                                                                                                                    <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td style="background:#03507d; color:#fff;">
                                                                                                                <asp:RadioButtonList ID="RadioButtonList7" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                    <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                    <asp:ListItem Text="No" Value="No" />
                                                                                                                    <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    <tr class="AltRowStyle">
                                                                                                        <td>Type of Instruction : </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns1" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns2" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns3" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns4" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns5" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns6" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns7" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="RowStyle">
                                                                                                        <td>Stimulus Set :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet1" runat="server" Text='<%#Eval("set1") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet2" runat="server" Text='<%#Eval("set2") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet3" runat="server" Text='<%#Eval("set3") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet4" runat="server" Text='<%#Eval("set4") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet5" runat="server" Text='<%#Eval("set5") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet6" runat="server" Text='<%#Eval("set6") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet7" runat="server" Text='<%#Eval("set7") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                     <tr class="AltRowStyle">
                                                                                                        <td>Current Training Step :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep1" runat="server" Text='<%#Eval("step1") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep1" Value='<%#Eval("stepId1") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep2" runat="server" Text='<%#Eval("step2") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep2" Value='<%#Eval("stepId2") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="Llblstep3" runat="server" Text='<%#Eval("step3") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep3" Value='<%#Eval("stepId3") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep4" runat="server" Text='<%#Eval("step4") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep4" Value='<%#Eval("stepId4") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep5" runat="server" Text='<%#Eval("step5") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep5" Value='<%#Eval("stepId5") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep6" runat="server" Text='<%#Eval("step6") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep6" Value='<%#Eval("stepId6") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep7" runat="server" Text='<%#Eval("step7") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep7" Value='<%#Eval("stepId7") %>' runat="server" />
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                    <tr class="RowStyle">
                                                                                                        <td>Prompt level :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl1" runat="server" Text='<%#Eval("ProptLevel1") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl2" runat="server" Text='<%#Eval("ProptLevel2") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl3" runat="server" Text='<%#Eval("ProptLevel3") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl4" runat="server" Text='<%#Eval("ProptLevel4") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl5" runat="server" Text='<%#Eval("ProptLevel5") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl6" runat="server" Text='<%#Eval("ProptLevel6") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl7" runat="server" Text='<%#Eval("ProptLevel7") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="AltRowStyle">
                                                                                                        <td>IOA (date/init/%) :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA1" runat="server" Text='<%#Eval("IOAPer1") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA2" runat="server" Text='<%#Eval("IOAPer2") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA3" runat="server" Text='<%#Eval("IOAPer3") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA4" runat="server" Text='<%#Eval("IOAPer4") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA5" runat="server" Text='<%#Eval("IOAPer5") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA6" runat="server" Text='<%#Eval("IOAPer6") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA7" runat="server" Text='<%#Eval("IOAPer7") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="RowStyle">
                                                                                                        <td># times run out of possible :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblNoOfPos1" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM1").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                            <asp:TextBox ID="txtNoOfPos1" runat="server" CssClass="txtdeno"  Text='<%# System.Text.RegularExpressions.Regex.Split(Eval("NUM1").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                            <br />,<asp:Label ID="lblMis1" runat="server" Text='<%#Eval("MIS1") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblNoOfPos2" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM2").ToString(),"/")[0]  %>'></asp:Label>/
                                                                                                            <asp:TextBox ID="txtNoOfPos2" runat="server" CssClass="txtdeno"  Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM2").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                            <br />,<asp:Label ID="lblMis2" runat="server" Text='<%#Eval("MIS2") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos3" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM3").ToString(),"/")[0] %>'></asp:Label>/                                                                                                             
                                                                                                            <asp:TextBox ID="txtNoOfPos3" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM3").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                           <br />,<asp:Label ID="lblMis3" runat="server" Text='<%#Eval("MIS3") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos4" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM4").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos4" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM4").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                           <br />,<asp:Label ID="lblMis4" runat="server" Text='<%#Eval("MIS4") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos5" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM5").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos5" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM5").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                           <br />,<asp:Label ID="lblMis5" runat="server" Text='<%#Eval("MIS5") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblNoOfPos6" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM6").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos6" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM6").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                            <br />,<asp:Label ID="lblMis6" runat="server" Text='<%#Eval("MIS6") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                               <asp:Label ID="lblNoOfPos7" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM7").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                           <asp:TextBox ID="txtNoOfPos7" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NUM7").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                         <br />,<asp:Label ID="lblMis7" runat="server" Text='<%#Eval("MIS7") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                        <%--<tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>Follow Up (from previous meeting):</td>
                                                                                            <td>Proposals and Discussion :</td>
                                                                                            <td>Persons Responsible and Deadlines :</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFreetxt" runat="server" class="area" TextMode="MultiLine" Width="250px" Rows="5"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtPersDissc" runat="server" class="area" TextMode="MultiLine" Width="250px" Rows="5"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtResAndDeadline" runat="server" class="area" TextMode="MultiLine" Width="250px" Rows="5"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>--%>

                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                Setting Events
                                                                                            </td>
                                                                                        </tr>                                                  
                                                                                                             
                                                           
                                                            <td colspan="4">
                                                                <asp:GridView ID="grdGraphDataGen" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Type" HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-CssClass="HeaderStyle" ItemStyle-CssClass="tdText">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblevent" runat="server" Text='<%# Eval("Eventname") %>'></asp:Label>
                                                                            </ItemTemplate>

                                                                            <HeaderStyle CssClass="HeaderStyle" Width="30%" />
                                                                            <ItemStyle CssClass="tdText" Width="30%" />

                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="70%" ItemStyle-Width="70%" HeaderStyle-CssClass="HeaderStyle" ItemStyle-CssClass="tdText">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("Eventdata") %>'></asp:Label>
                                                                            </ItemTemplate>

                                                                            <HeaderStyle CssClass="HeaderStyle" Width="70%" />
                                                                            <ItemStyle CssClass="tdText" Width="70%" />

                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <EditRowStyle BackColor="#2461BF" />
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />

                                                                    <RowStyle CssClass="RowStyle" BackColor="#EFF3FB" />
                                                                    <AlternatingRowStyle CssClass="AltRowStyle" BackColor="White" />
                                                                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />

                                                                </asp:GridView>

                                                            </td>                         
                                                               </tr>


                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                Previous Meeting Section
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <%--<td>--%>
                                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                                <%--<Triggers>--%>
                                                                                                    <%--<asp:AsyncPostBackTrigger controlid="btnDelRowCurMeeting" eventname="Click" />
                                                                                                    <asp:AsyncPostBackTrigger controlid="btnAddRowCurMeeting" eventname="Click" />--%>
                                                                                                    <%--<asp:AsyncPostBackTrigger ControlID="gvPMeeting" />--%>
                                                                                                <%--</Triggers>--%>
                                                                                                    <ContentTemplate>
                                                                                                    <asp:GridView ID="gvPMeeting" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPMeeting_RowCommand" BackColor="White" GridLines="None" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Follow Up">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtPFollowUp" Width="500px" TextMode="MultiLine" Rows="5" Text='<%# Eval("PFollowUp") %>' Enabled="true"></asp:TextBox>
                                                                                                                    <asp:Label ID="lblPMtngId" Text='<%# Eval("PMtngId") %>' runat="server" Visible="false"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Person Responsible">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtPPersonResponsible" CssClass = "autosuggest" Width="175px" Text='<%# Eval("PPersonResponsible") %>' Enabled="true"></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Deadlines">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtPDeadlines" Placeholder="MM/DD/YYYY" Width="100px" ValidationGroup="Group1" Text='<%# Eval("PDeadlines") %>'  Enabled="true"></asp:TextBox>
                                                                                                                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPDeadlines" ValidationExpression="(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$"
                                                                                                                            ErrorMessage="*" ValidationGroup="Group1" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                             <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnDelRowCurMeeting" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete"  OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnAddRowCurMeeting" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

                                                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

                                                                                                        <PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>

                                                                                                        <RowStyle ForeColor="#000066"></RowStyle>

                                                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

                                                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1"></SortedAscendingCellStyle>

                                                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB"></SortedAscendingHeaderStyle>

                                                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9"></SortedDescendingCellStyle>

                                                                                                        <SortedDescendingHeaderStyle BackColor="#00547E"></SortedDescendingHeaderStyle>
                                                                                                    </asp:GridView>
                                                                                                        </ContentTemplate>
                                                                                                </asp:UpdatePanel>

                                                                                                <%--</td>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                Current Meeting Section
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <%--<td>--%>
                                                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                                                <%--<Triggers>--%>
                                                                                                    <%--<asp:AsyncPostBackTrigger controlid="btnAddRowCurMeeting" eventname="Click" />
                                                                                                    <asp:AsyncPostBackTrigger controlid="btnDelRowCurMeeting" eventname="Click" />--%>
                                                                                                    <%--<asp:AsyncPostBackTrigger ControlID="gvCMeeting" />--%>
                                                                                                <%--</Triggers>--%>
                                                                                                    <ContentTemplate>
                                                                                                    <asp:GridView ID="gvCMeeting" runat="server" AutoGenerateColumns="False" OnRowCommand="gvCMeeting_RowCommand" OnRowDeleting="gvCMeeting_RowDeleting" BackColor="White" GridLines="None" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Proposals and Discussion">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtCFollowUp" Width="500px" TextMode="MultiLine" Rows="5" Text='<%# Eval("CFollowUp") %>'></asp:TextBox>
                                                                                                                    <asp:Label ID="lblCMtngId" Text='<%# Eval("CMtngId") %>' runat="server" Visible="false"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Person Responsible">
                                                                                                                <ItemTemplate>
                                                                                                                    <div>
                                                                                                                    <asp:TextBox runat="server" ID="txtPersonResponsible" CssClass = "autosuggest" Width="175px"  Text='<%# Eval("PersonResponsible") %>'></asp:TextBox>
                                                                                                                    <asp:TextBox runat="server" ID="txtCPersonResponsible" CssClass = "autosuggest" Width="175px" class ="hd" Text='<%# Eval("CPersonResponsible") %>' style="display:none"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Deadlines">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtCDeadlines" Placeholder="MM/DD/YYYY" Width="100px" Text='<%# Eval("CDeadlines") %>' ></asp:TextBox>
                                                                                                                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCDeadlines" ValidationExpression="(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$"
                                                                                                                            ErrorMessage="*" ValidationGroup="Group1" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnDelRowCurMeeting" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" CommandArgument='<%# Eval("CMtngId") %>' OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnAddRowCurMeeting" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

                                                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

                                                                                                        <PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>

                                                                                                        <RowStyle ForeColor="#000066"></RowStyle>

                                                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

                                                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1"></SortedAscendingCellStyle>

                                                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB"></SortedAscendingHeaderStyle>

                                                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9"></SortedDescendingCellStyle>

                                                                                                        <SortedDescendingHeaderStyle BackColor="#00547E"></SortedDescendingHeaderStyle>
                                                                                                    </asp:GridView>
                                                                                                        </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                                <%--</td>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="auto-style2"></td>
                                                                                            <td class="auto-style2"></td>
                                                                                            <td class="auto-style2"></td>
                                                                                            <td class="auto-style2"></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                            </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" id="tdreview1" style="text-align: left;">
                                                                    <asp:Label ID="reviewsave" runat="server" Text="Reviewed by and date"></asp:Label>
                                                                    &nbsp;:&nbsp;&nbsp;
                                                                    <br />
                                                                    <asp:TextBox ID="ReviewbydateSave" runat="server" Height="57px" TextMode="MultiLine" Width="502px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="text-align: center;">
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="Save_Click" OnClientClick="return scrollToTop();" Text="Save" Visible="true" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                    <asp:View ID="View2" runat="server">
                                                        <table style="width: 100%;">

                                                            <%--   <tr>
                                                                <td colspan="3" style="text-align: center;">
                                                                    <asp:DropDownList ID="ddlDate" runat="server" CssClass="drpClass">
                                                                    </asp:DropDownList>
                                                                    <asp:DropDownList ID="ddlStudentEdit" runat="server" Visible="False" CssClass="drpClass">
                                                                    </asp:DropDownList>
                                                                    <asp:Button ID="btnLoadDataEdit" runat="server" CssClass="NFButton" OnClick="btnLoadDataEdit_Click" Text="Load Prior Report" Width="175px" />                                                                    
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:GridView Width="100%" ID="GridViewAccSheetedit" runat="server" AutoGenerateColumns="False" ShowHeader="False" OnSelectedIndexChanged="GridViewAccSheetedit_SelectedIndexChanged" OnRowDataBound="GridViewAccSheetedit_RowDataBound">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>

                                                                                    <table class="bor" style="width: 100%; background-color: #f7f5f5; margin: 5px 0 5px 0;" >
                                                                                                                                 
                                                                                         <tr>
                                                                                            <td style="width:90px;" class="tdText bor">Goal Area :</td>
                                                                                            <td style="text-align:left" class="bor">
                                                                                                <asp:HiddenField ID="hdFldAcdId" runat="server" Value='<%#Eval("AccSheetId") %>' />
                                                                                                <asp:HiddenField ID="hfLPIdEdit" runat="server" Value='<%#Eval("LessonPlanId") %>' />
                                                                                                <asp:Label ID="lblGoalAreaedit"  runat="server" Text='<%#Eval("GoalArea") %>'></asp:Label>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td style="width:90px" class="tdText bor">Goal :</td>
                                                                                            <td style="text-align:left" class="bor">
                                                                                                <asp:Label ID="lblGoaledit" runat="server" Text='<%#Eval("Goal") %>'></asp:Label>
                                                                                            </td>           
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td style="width:90px" class="tdText bor">Benchmarks/ Objectives:</td>
                                                                                            <td style="text-align:left" class="bor">
                                                                                                 <div id="txtbenchaMark" runat="server" height="50px" width="640"><%#Eval("Benchmarks") %></div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width:90px" class="tdText bor">Current Progress:</td>
                                                                                                <td id="tdMsgp" runat="server" colspan="2">
                                                                                        <label id="ch1" runat="server" style="color: #808080; ">
                                                                                                 <input type="Checkbox" runat="server" name="termsChkbx1" id="Checkbox1" onclick="updateColor(this)"/>
                                                                                                 <span>Met Objective</span>
                                                                                                 </label>  
                                                                                                
                                                                                                <label id="ch2" runat="server" style="color: #808080; ">
                                                                                                 <input type="Checkbox" runat="server" name="termsChkbx2" id="Checkbox2"  onclick="updateColor(this)"/>
                                                                                                 <span>Met Goal</span>
                                                                                                 </label>  
                                                                                                
                                                                                                <label id="ch3" runat="server" style="color: #808080; ">
                                                                                                 <input type="Checkbox" runat="server" name="termsChkbx3" id="Checkbox3" onclick="updateColor2(this)"/>
                                                                                                 <span>Not Maintaining</span>
                                                                                                 </label>     
                                                                                        </td>

                                                                                        </tr>
                                                                                        </table>
                                                                                        <table style="width: 100%; background-color: #f7f5f5; margin: 5px 0 5px 0;">
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <table id="tabValReviewedit" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                                                                    <tr class="HeaderStyle">
                                                                                                        <td>Review Period :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod8" runat="server" Text='<%#Eval("Period1") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod9" runat="server" Text='<%#Eval("Period2") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod10" runat="server" Text='<%#Eval("Period3") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod11" runat="server" Text='<%#Eval("Period4") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod12" runat="server" Text='<%#Eval("Period5") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod13" runat="server" Text='<%#Eval("Period6") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblPeriod14" runat="server" Text='<%#Eval("Period7") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr
                                                                                                    <tr class="HeaderStyle">
                                                                                                        <br />
                                                                                                        <td style="background:#03507d; color:#fff;">Progressing :</td>
                                                                                                        <td style="background:#03507d; color:#fff;">
                                                                                                            <asp:RadioButtonList ID="RadioButtonList8" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                <asp:ListItem Text="No" Value="No" />
                                                                                                                <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                            </asp:RadioButtonList>
                                                                                                          
                                                                                                        </td>
                                                                                                        <td style="background:#03507d; color:#fff;">
                                                                                                            <asp:RadioButtonList ID="RadioButtonList9" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                <asp:ListItem Text="No" Value="No" />
                                                                                                                <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                            </asp:RadioButtonList>
                                                                                                            
                                                                                                        </td>
                                                                                                        <td style="background:#03507d; color:#fff;">
                                                                                                            <asp:RadioButtonList ID="RadioButtonList10" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                <asp:ListItem Text="No" Value="No" />
                                                                                                                <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                            </asp:RadioButtonList>
                                                                                                           
                                                                                                        </td>
                                                                                                        <td style="background:#03507d; color:#fff;">
                                                                                                            <asp:RadioButtonList ID="RadioButtonList11" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                <asp:ListItem Text="No" Value="No" />
                                                                                                                <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                            </asp:RadioButtonList>
                                                                                                            
                                                                                                        </td>
                                                                                                        <td style="background:#03507d; color:#fff;">
                                                                                                            <asp:RadioButtonList ID="RadioButtonList12" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                <asp:ListItem Text="No" Value="No" />
                                                                                                                <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                            </asp:RadioButtonList>
                                                                                                           
                                                                                                        </td>
                                                                                                        <td style="background:#03507d; color:#fff;">
                                                                                                                <asp:RadioButtonList ID="RadioButtonList13" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                <asp:ListItem Text="No" Value="No" />
                                                                                                                <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                            </asp:RadioButtonList>

                                                                                                            
                                                                                                        </td>
                                                                                                        <td style="background:#03507d; color:#fff;">
                                                                                                            <asp:RadioButtonList ID="RadioButtonList14" class="radBtnClass" runat="server" onchange="changeColorRadioBtnList(this)">
                                                                                                                <asp:ListItem Text="Yes" Value="Yes" />
                                                                                                                <asp:ListItem Text="No" Value="No" />
                                                                                                                <asp:ListItem Text="No Change" Value="No Change" />
                                                                                                            </asp:RadioButtonList>

                                                                                                        </td>
                                                                                                        </tr>

                                                                                                    <tr class="AltRowStyle">
                                                                                                        <td>Type of Instruction : </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns8" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns9" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns10" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns11" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns12" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns13" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblTypOfIns14" runat="server" Text='<%#Eval("TypeOfInstruction") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="RowStyle">
                                                                                                        <td>Stimulus Set :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet8" runat="server" Text='<%#Eval("Set1") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet9" runat="server" Text='<%#Eval("Set2") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet10" runat="server" Text='<%#Eval("Set3") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet11" runat="server" Text='<%#Eval("Set4") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet12" runat="server" Text='<%#Eval("Set5") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet13" runat="server" Text='<%#Eval("Set6") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblStmlsSet14" runat="server" Text='<%#Eval("Set7") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>

                                                                                                     <tr class="AltRowStyle">
                                                                                                        <td>Current Training Step :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep8" runat="server" Text='<%#Eval("step1") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep8" Value='<%#Eval("stepId1") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep9" runat="server" Text='<%#Eval("step2") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep9" Value='<%#Eval("stepId2") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep10" runat="server" Text='<%#Eval("step3") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep10" Value='<%#Eval("stepId3") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep11" runat="server" Text='<%#Eval("step4") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep11" Value='<%#Eval("stepId4") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep12" runat="server" Text='<%#Eval("step5") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep12" Value='<%#Eval("stepId5") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep13" runat="server" Text='<%#Eval("step6") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep13" Value='<%#Eval("stepId6") %>' runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblstep14" runat="server" Text='<%#Eval("step7") %>'></asp:Label>
                                                                                                            <asp:HiddenField ID="hdnstep14" Value='<%#Eval("stepId7") %>' runat="server" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="RowStyle">
                                                                                                        <td>Prompt level :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl8" runat="server" Text='<%#Eval("Prompt1") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl9" runat="server" Text='<%#Eval("Prompt2") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl10" runat="server" Text='<%#Eval("Prompt3") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl11" runat="server" Text='<%#Eval("Prompt4") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl12" runat="server" Text='<%#Eval("Prompt5") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl13" runat="server" Text='<%#Eval("Prompt6") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblprmtLvl14" runat="server" Text='<%#Eval("Prompt7") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="AltRowStyle">
                                                                                                        <td>IOA (date/init/%) :</td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA8" runat="server" Text='<%#Eval("IOA1") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA9" runat="server" Text='<%#Eval("IOA2") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA10" runat="server" Text='<%#Eval("IOA3") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA11" runat="server" Text='<%#Eval("IOA4") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA12" runat="server" Text='<%#Eval("IOA5") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA13" runat="server" Text='<%#Eval("IOA6") %>'></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblIOA14" runat="server" Text='<%#Eval("IOA7") %>'></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr class="RowStyle">
                                                                                                        <td># times run out of possible :</td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos8" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes1").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos8" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes1").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                           <br />,<asp:Label ID="lblMistrial1" runat="server" Text='<%#Eval("Mistrial1") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos9" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes2").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos9" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes2").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                           <br />,<asp:Label ID="lblMistrial2" runat="server" Text='<%#Eval("Mistrial2") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos10" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes3").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                            <asp:TextBox ID="txtNoOfPos10" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes3").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                            <br />,<asp:Label ID="lblMistrial3" runat="server" Text='<%#Eval("Mistrial3") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                                <asp:Label ID="lblNoOfPos11" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes4").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos11" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes4").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                        <br />,<asp:Label ID="lblMistrial4" runat="server" Text='<%#Eval("Mistrial4") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos12" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes5").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos12" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes5").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                           <br />,<asp:Label ID="lblMistrial5" runat="server" Text='<%#Eval("Mistrial5") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                             <asp:Label ID="lblNoOfPos13" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes6").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos13" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes6").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                           <br />,<asp:Label ID="lblMistrial6" runat="server" Text='<%#Eval("Mistrial6") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblNoOfPos14" runat="server" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes7").ToString(),"/")[0] %>'></asp:Label>/
                                                                                                             <asp:TextBox ID="txtNoOfPos14" runat="server" CssClass="txtdeno" Text='<%#System.Text.RegularExpressions.Regex.Split(Eval("NoOfTimes7").ToString(),"/")[1] %>' Width="20px" BorderStyle="None" style="padding-left:0;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                                                            <br />,<asp:Label ID="lblMistrial7" runat="server" Text='<%#Eval("Mistrial7") %>'></asp:Label> Mistrials
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                        <%--<tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>Follow Up (from previous meeting):</td>
                                                                                            <td>Proposals and Discussion :</td>
                                                                                            <td>Persons Responsible and Deadlines :</td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFreetxtedit" runat="server" class="area" TextMode="MultiLine" Text='<%#Eval("FeedBack") %>' Width="250px" Rows="5"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtPersDisscedit" runat="server" class="area" TextMode="MultiLine" Text='<%#Eval("PreposalDiss") %>' Width="250px" Rows="5"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtResAndDeadlineedit" runat="server" class="area" TextMode="MultiLine" Text='<%#Eval("PersonResNdDeadline") %>' Width="250px" Rows="5"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>--%>

                                                                                       <tr>
                                                                                            <td colspan="4">
                                                                                                Setting Events
                                                                                            </td>
                                                                                        </tr>                                                     
                                                      
                                                       
                                                            
                                                            <td colspan="4">
                                                                <asp:GridView ID="grdGraphDataNew" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Type" HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-CssClass="HeaderStyle" ItemStyle-CssClass="tdText">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Eventname") %>'></asp:Label>
                                                                            </ItemTemplate>

                                                                            <HeaderStyle CssClass="HeaderStyle" Width="30%" />
                                                                            <ItemStyle CssClass="tdText" Width="30%" />

                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="70%" ItemStyle-Width="70%" HeaderStyle-CssClass="HeaderStyle" ItemStyle-CssClass="tdText">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Eventdata") %>'></asp:Label>
                                                                            </ItemTemplate>

                                                                            <HeaderStyle CssClass="HeaderStyle" Width="70%" />
                                                                            <ItemStyle CssClass="tdText" Width="70%" />

                                                                        </asp:TemplateField>
                                                                    </Columns>

                                                                    <EditRowStyle BackColor="#2461BF" />
                                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />

                                                                    <RowStyle CssClass="RowStyle" BackColor="#EFF3FB" />
                                                                    <AlternatingRowStyle CssClass="AltRowStyle" BackColor="White" />
                                                                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />

                                                                </asp:GridView>

                                                            </td>                     
                                                       </tr>
                                                            
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                Previous Meeting Section
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <%--<td>--%>
                                                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                                                <%--<Triggers>--%>
                                                                                                   <%-- <asp:AsyncPostBackTrigger controlid="btnAddRowCurMeeting" eventname="Click" />
                                                                                                    <asp:AsyncPostBackTrigger controlid="btnDelRowCurMeeting" eventname="Click" />--%>
                                                                                                    <%--<asp:AsyncPostBackTrigger ControlID="gvPMeetingEdit" />--%>
                                                                                                <%--</Triggers>--%>
                                                                                                    <ContentTemplate>
                                                                                                    <asp:GridView ID="gvPMeetingEdit" runat="server" AutoGenerateColumns="False" OnRowCommand="gvPMeetingEdit_RowCommand" OnRowDeleting="gvPMeetingEdit_RowDeleting" BackColor="White" GridLines="None" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Follow Up">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtPFollowUpEdit" Width="500px" TextMode="MultiLine" Rows="5" Text='<%# Eval("PFollowUpEdit") %>' Enabled="true"></asp:TextBox>
                                                                                                                    <asp:Label ID="lblPMtngIdEdit" Text='<%# Eval("PMtngIdEdit") %>' runat="server" Visible="false"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Person Responsible">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtPPersonResponsibleEdit" CssClass = "autosuggest" Width="175px" Text='<%# Eval("PPersonResponsibleEdit") %>' Enabled="true"></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Deadlines">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtPDeadlinesEdit" Placeholder="MM/DD/YYYY" Width="100px" Text='<%# Eval("PDeadlinesEdit") %>' Enabled="true" >></asp:TextBox>
                                                                                                                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPDeadlinesEdit" ValidationExpression="(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$"
                                                                                                                            ErrorMessage="*" ValidationGroup="Group1" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                             <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnDelRowCurMeeting" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete"  OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnAddRowCurMeeting" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

                                                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

                                                                                                        <PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>

                                                                                                        <RowStyle ForeColor="#000066"></RowStyle>

                                                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

                                                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1"></SortedAscendingCellStyle>

                                                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB"></SortedAscendingHeaderStyle>

                                                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9"></SortedDescendingCellStyle>

                                                                                                        <SortedDescendingHeaderStyle BackColor="#00547E"></SortedDescendingHeaderStyle>
                                                                                                    </asp:GridView>
                                                                                                        </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                                <%--</td>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                Current Meeting Section
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <%--<td>--%>
                                                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                                                <%--<Triggers>--%>
                                                                                                    <%--<asp:AsyncPostBackTrigger controlid="btnAddRowCurMeeting" eventname="Click" />
                                                                                                    <asp:AsyncPostBackTrigger controlid="btnDelRowCurMeeting" eventname="Click" />--%>
                                                                                                    <%--<asp:AsyncPostBackTrigger ControlID="gvCMeetingEdit" />--%>
                                                                                                <%--</Triggers>--%>
                                                                                                    <ContentTemplate>
                                                                                                    <asp:GridView ID="gvCMeetingEdit" runat="server" AutoGenerateColumns="False" OnRowCommand="gvCMeetingEdit_RowCommand" OnRowDeleting="gvCMeetingEdit_RowDeleting" BackColor="White" GridLines="None" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Proposals and Discussion">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtCFollowUpEdit" Width="500px" TextMode="MultiLine" Rows="5" Text='<%# Eval("CFollowUpEdit") %>'></asp:TextBox>
                                                                                                                    <asp:Label ID="lblCMtngIdEdit" Text='<%# Eval("CMtngIdEdit") %>' runat="server" Visible="false"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Person Responsible">
                                                                                                                <ItemTemplate>
                                                                                                                    <div>
                                                                                                                    <asp:TextBox runat="server" ID="txtPersonResponsibleEdit" CssClass = "autosuggest" Width="175px"  Text='<%# Eval("PersonResponsibleEdit") %>'></asp:TextBox>
                                                                                                                    <asp:TextBox runat="server" ID="txtCPersonResponsibleEdit" CssClass = "autosuggest" Width="175px" Class="hd" Text='<%# Eval("CPersonResponsibleEdit") %>' style="display:none"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Deadlines">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox runat="server" ID="txtCDeadlinesEdit" Placeholder="MM/DD/YYYY" Width="100px" Text='<%# Eval("CDeadlinesEdit") %>' ></asp:TextBox>
                                                                                                                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCDeadlinesEdit" ValidationExpression="(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$"
                                                                                                                            ErrorMessage="*" ValidationGroup="Group1" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnDelRowCurMeeting" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" CommandArgument='<%# Eval("CMtngIdEdit") %>' OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:ImageButton ID="btnAddRowCurMeeting" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

                                                                                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

                                                                                                        <PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>

                                                                                                        <RowStyle ForeColor="#000066"></RowStyle>

                                                                                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

                                                                                                        <SortedAscendingCellStyle BackColor="#F1F1F1"></SortedAscendingCellStyle>

                                                                                                        <SortedAscendingHeaderStyle BackColor="#007DBB"></SortedAscendingHeaderStyle>

                                                                                                        <SortedDescendingCellStyle BackColor="#CAC9C9"></SortedDescendingCellStyle>

                                                                                                        <SortedDescendingHeaderStyle BackColor="#00547E"></SortedDescendingHeaderStyle>
                                                                                                    </asp:GridView>
                                                                                                         </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                                <%--</td>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="auto-style2"></td>
                                                                                            <td class="auto-style2"></td>
                                                                                            <td class="auto-style2"></td>
                                                                                            <td class="auto-style2"></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" runat="server" id="tdreview2" style="text-align: left;">
                                                                    <asp:Label ID="reviewLabel" runat="server" Text="Reviewed by and date"></asp:Label>
                                                                    &nbsp;:&nbsp;&nbsp;
                                                                    <br />
                                                                    <asp:TextBox ID="ReviewbydateUpdate" runat="server" Height="57px" TextMode="MultiLine" Width="502px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="text-align: center;">
                                                                    <asp:Button ID="btnUpdate" runat="server" CssClass="NFButton" OnClick="btnUpdate_Click" OnClientClick="return scrollToTop();" Text="Update" ValidationGroup="Group1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: center" colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                        </table>



                        <div id="overlay" class="web_dialog_overlay"></div>
                        <div id="PopDownload" class="web_dialog" style="width: 600px; top: -20%;">

                            <div id="Div53" style="width: 700px;">


                                <table style="width: 97%">
                                    <tr>
                                        <td colspan="2">
                                            <table style="width: 85%">
                                                <tr>
                                                    <td runat="server" id="tdMsgExport" style="height: 50px"></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">

                                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="NFButton" OnClick="btnDownload_Click" />

                                        </td>
                                        <td style="text-align: left">

                                            <asp:Button ID="btnDone" runat="server" Text="Done" CssClass="NFButton" OnClick="btnDone_Click" />

                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                        <div id="dialog" class="web_dialog" style="width: 900px; margin-left: -22%;">

                            <div id="sign_up5" style="width: 100%; margin-left: 10px">
                                <span style="color: red;text-align:center"> *Academic Coversheet reports 7 biweekly periods (14 weeks). If needed, Please alter either the Start or End date.</span>
                                    
                                <table style="width: 100%">
                                    <tr>
                                        <td runat="server" id="tdMessage" class="tdText" colspan="5"></td>
                                    </tr>


                                    <tr>
                                        <td style="width: 20%" class="tdText">Start Date:</td>
                                        <td style="width: 1%"><span style="color: red">*</span></td>
                                        <td style="width: 35%" class="tdText">
                                            <asp:TextBox ID="txtSdate" runat="server" CssClass="textClass" Width="250px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox></td>
                                        <td style="width: 10%" class="tdText">End Date:</td>
                                        <td style="width: 1%"><span style="color: red">*</span></td>
                                        <td style="width: 35%" class="tdText">
                                            <asp:TextBox ID="txtEdate" runat="server" CssClass="textClass" Width="250px" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox></td>
                                    </tr>

                                    <tr>
                                        <td class="tdText" style="text-align: center" colspan="6"></td>

                                    </tr>
                                    <tr>
                                        <td class="tdText" style="text-align: center" colspan="6">
                                            <asp:Button ID="btnGenACD" runat="server" Text="Generate" OnClick="btnGenACD_Click" CssClass="NFButton"  OnClientClick="showMessage()" />
                                            <input type="button" id="CancalGen" class="NFButton" value="Cancel" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>
                       <div class="loading">
                              <div class="innerLoading">
                               <img src="../Administration/images/load.gif" alt="loading" />
                                   Please Wait....
                               </div>
                         </div>



                        <div id="Div7">
                        </div>






                        <div class="clear"></div>
                    </div>




                    <!------------------------------------End Container End----------------------------------->

                    <div class="clear"></div>
                </div>
                <!-------------------------Middle Container End----------------------->


                <!------------------------Pop up Windows----------------------->











                <div class="clear"></div>
            </div>


        </div>
      
        <script type="text/javascript">
            $(document).ready(function SearchText() {
                //alert("in autocomplete function");
                var prm = Sys.WebForms.PageRequestManager.getInstance();    
                prm.add_initializeRequest(InitializeRequest);
                prm.add_endRequest(EndRequest);

                // Place here the first init of the autocomplete
                InitAutoCompl();
            });        
           

            function InitializeRequest(sender, args) {
            }

            function EndRequest(sender, args) {
                // after update occur on UpdatePanel re-init the Autocomplete
                InitAutoCompl();
            }

            function InitAutoCompl() {
                $(".autosuggest").autocomplete({
                    open: function () {
                        $('.ui-menu')
                        .width(180);
                    },
                    source: function (request, response) {

                        $.ajax({
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8',
                            url: 'ACSheet.aspx/GetAutoCompleteData',
                            data: "{'prefix': '" + request.term + "'}",
                            dataType: "json",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        val: item,
                                        value: item
                                    }
                                }))
                            }
                        })
                    },
                    select: function (event, ui) {
                        if (ui.item) {
                            $(this).val(ui.item.value);
                            $(this).parent().find('.hd').val(ui.item.val);
                            return false;

                        }
                    }

                });
            };
        </script>
         <script type="text/javascript">
             function destroy() {
                 if (jQuery(".autosuggest").data('autocomplete')) {
                     jQuery(".autosuggest").autocomplete("destroy");
                     jQuery(".autosuggest").removeData('autocomplete');
                 };

                 $(".autosuggest").autocomplete({
                     open: function () {
                         $('.ui-menu')
                         .width(180);
                     },
                     source: function (request, response) {
                         $.ajax({
                             type: 'POST',
                             contentType: 'application/json; charset=utf-8',
                             url: 'ACSheet.aspx/GetAutoCompleteData',
                             data: "{'prefix': '" + request.term + "'}",
                             dataType: "json",
                             success: function (data) {
                                 response($.map(data.d, function (item) {
                                     return {

                                         val: item.split('-')[0],
                                         value: item.split('-')[1]
                                     }
                                 }))
                             }
                         })
                     },
                     select: function (event, ui) {
                         if (ui.item) {
                             $(this).val(ui.item.value);
                             $(this).parent().find('.hd').val(ui.item.val);
                             return false;
                         }
                     }

                 });
             }

        </script>
         
    </form>
</body>
</html>
