<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GrandRoundCvrsht.aspx.cs" Inherits="StudentBinder_GrandRoundCvrsht" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="CSS/ClinicalSheet.css" rel="stylesheet" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <%--<script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="../Administration/JS/jquery.ui.datepicker.js"></script>--%>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <%-- <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>--%>
    <script src="../Administration/JS/jquery-ui.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />


    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.min.1.3.js"></script>
    <link href="../Administration/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/jsDatePick.jquery.min.1.3.js"></script>


    <script type="text/javascript">
        window.onload = function () {
            $(function () {
                $(".DatePick").datepicker();
            });
        };
        //window.onload = function () {
        //    $(function () {
        //        $(".autosuggest").autocomplete();
        //    });
        //};

        function loadDateJqry() {
            $(function () {
                $(".DatePick").datepicker();
            });
        }

        function ShowDashboard() {
            $("#ddlDashboardRecType").val("All");
            $("#ddlDashboardCmpDate").val("All");
            $("#txtCalendarFrom").val("");
            $("#txtCalendarTo").val("");
            $("#btnDashboardFilter").trigger("click");
            $('#dashMain').fadeIn();
        }

        $(document).ready(function () {
            $('#CancalGen').click(function () {
                $('#dialog').animate({ top: "-300%" }, function () {
                    $('#overlay').fadeOut('slow');
                });
            });



            $("#txtSdate").datepicker({
                dateFormat: "mm/dd/yy",
                maxDate: 0,
                onSelect: function () {
                    var dt2 = $('#txtEdate');
                    var today = new Date();
                    var startDate = $(this).datepicker('getDate');
                    //add 98 days to selected date
                    startDate.setDate(startDate.getDate() + 90);
                    var minDate = dt2.datepicker('getDate');
                    if (new Date(today) < new Date(startDate)) {
                        dt2.datepicker('setDate', today);
                        dt2.datepicker('option', 'maxDate', today);
                    }
                    else
                        dt2.datepicker('setDate', startDate);
                    //sets dt2 maxDate to the last day of 30 days window
                    //dt2.datepicker('option', 'maxDate', startDate);
                    //first day which can be selected in dt2 is selected date in dt1
                    //dt2.datepicker('option', 'minDate', minDate);
                    //same for dt1
                    //$(this).datepicker('option', 'minDate', minDate);
                }
            });
            $('#txtEdate').datepicker({
                dateFormat: "mm/dd/yy",
                maxDate: 0,
                onSelect: function () {
                    var dt1 = $('#txtSdate');
                    var endDate = $(this).datepicker('getDate');
                    //add 30 days to selected date
                    endDate.setDate(endDate.getDate() - 90);
                    var minDate = $(this).datepicker('getDate');
                    //minDate of dt2 datepicker = dt1 selected day
                    dt1.datepicker('setDate', endDate);
                }

            });
        });


        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 0);
        }

    </script>


    <style type="text/css">
        .FreeTextDivContent {
            width: 96%;
            min-height: 100px;
            height: auto;
            padding: 2%;
            border: 1px solid rgb(228, 228, 228);
            border-radius: 8px 8px 8px 8px;
        }

            .FreeTextDivContent ul li {
                list-style: disc outside none !important;
                margin: 0;
                padding: 0;
                text-align: left;
            }

            .FreeTextDivContent ol li {
                list-style: decimal outside none !important;
                margin: 0;
                padding: 0;
                text-align: left;
            }

        ol, ul {
            padding-left: 0;
            width: 100%;
        }

        .auto-style2 {
            height: 23px;
        }

        p.MHead1 {
            margin: 3.0pt 0in;
            page-break-after: avoid;
            punctuation-wrap: simple;
            text-autospace: none;
            font-size: 9.0pt;
            font-family: "Arial Black","sans-serif";
            text-decoration: underline;
            text-underline: single;
        }

        .auto-style3 {
            height: 19px;
        }

        .auto-style4 {
            width: 20%;
            height: 19px;
        }

        .auto-style5 {
            width: 80%;
            height: 19px;
        }

        .mainheadingpanel {
            color: #4F5050;
            float: left;
            font-family: Oswald;
            font-size: 17px;
            font-weight: normal;
            height: 35px;
            line-height: 35px;
            margin: 0;
            text-align: left;
        }
    </style>

    <style type="text/css">
        .ui-datepicker {
            font-size: 8pt !important;
        }
    </style>

    <style type="text/css">
        hr {
            border: 1px dashed gray;
        }

        .style1 {
            width: 864px;
        }

        .style2 {
            width: 20%;
        }

        .widthSettd {
            width: 20px;
        }

        .auto-style6 {
            width: 100%;
            height: 19px;
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

        table {
        }

        input[type="text"] {
            width: 80% !important;
        }

        .RowStyle1 {
            width: 100%;
        }

            .RowStyle1 td {
                border-bottom: 0 solid #e9e9e9;
                text-align: center;
            }
        .ui-autocomplete {
            z-index: auto;
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
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>

        <div id="fullContent">

            <div class="boxContainerContainer">
                <div class="clear"></div>
                <table style="width: 100%;">
                    <tr>
                        <td id="tdMsg2" runat="server"></td>
                    </tr>
                </table>
                <div class="clear"></div>


                <%--Top Container--%>

                <div class="itlepartContainer">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 20%;">
                                <h2>1 <span>Choose Date</span></h2>
                            </td>
                            <td style="width: 30%;">
                                <h2>2 <span>RTF Coversheet Report</span></h2>
                            </td>
                            <td style="width: 30%; text-align: right;">
                                <input type="button" id="btnDashboard" class="NFButton" value="Dashboard" onclick="ShowDashboard();" style="width: auto;" />
                            </td>
                            <td>

                                <asp:Button ID="btnGenNewSheet" runat="server" CssClass="NFButton" ToolTip="Create new sheet" alt="Create New Document" OnClick="btnGenNewRTFSheet_Click" Text="Create New Coversheet" Style="width: auto;" />

                            </td>

                        </tr>
                    </table>



                </div>

                <%--Top Container End--%>
                <div class="btContainerPart">


                    <div class="mBxContainer">
                        <h3>Review Periods</h3>
                        <div class="nobdrrcontainer">
                            <%--      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>

                            <asp:DataList ID="grDateList" runat="server" OnItemCommand="grDateList_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDate" CssClass="grmb" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem,"EDate","{0:MM/dd/yyyy}") %>'></asp:LinkButton>

                                </ItemTemplate>
                            </asp:DataList>
                            <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>




                        <div class="clear"></div>
                    </div>


                    <div id="MainDiv" class="righMainContainer large" runat="server" visible="true">
                        <table style="width: 100%;">
                            <tr>
                                <td id="tdMsg" runat="server"></td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:Panel ID="PanelMain" runat="server">
                                        <div>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td></td>
                                                </tr>


                                                <asp:HiddenField ID="hdFldCvid" runat="server" />
                                                <asp:GridView Width="100%" ID="GVBasicDetails" runat="server" AutoGenerateColumns="false" ShowHeader="false" OnRowDataBound="GVBasicDetails_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <table style="width: 100%;">
                                                                    <tr class="HeaderStyle">
                                                                        <td>Name</td>
                                                                        <td>DOB</td>
                                                                        <td>Age</td>
                                                                        <td>Admission Date</td>
                                                                        <td>Clinical Case Manager</td>
                                                                        <td>Review From</td>
                                                                        <td>Review To</td>
                                                                    </tr>
                                                                    <tr class="RowStyle">
                                                                        <td>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("FirstName") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("BirthDate") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAge" runat="server" Text='<%#Eval("Age") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAdmission" runat="server" Text='<%#Eval("AdmissionDate") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <%--<asp:Label ID="lblCase" runat="server" Text='<%#Eval("CaseManagerEducational") %>'></asp:Label>--%>
                                                                            <%-- <asp:DropDownList ID="ddlCaseManEdu" runat="server"></asp:DropDownList>--%>
                                                                            <div>
                                                                                <asp:TextBox ID="txtCaseManEdu" class="autosuggest" runat="server"></asp:TextBox>
                                                                                <%--<asp:HiddenField ID="hdFldtxtCaseManEdu" Value="" runat="server" />--%>
                                                                                <asp:TextBox ID="hdCaseManEdu" runat="server" style="display:none"  Class="hd" />
                                                                                <%--<asp:HiddenField ID="HiddenField1" Value=" document.getElementById(<%#Eval("txtCaseManEdu.ClientId")%>')" runat="server" />--%>
                                                                                <%--<script type ="text/javascript">
                                                        function SetHiddenField() {
                                                            var vv = '"+txtCaseManEdu.Text+"';
                                                            document.getElementById('<%=hdFldtxtCaseManEdu.ClientId%>').value = vv;
                                                            __doPostBack('<%=hdFldtxtCaseManEdu.ClientId%>', '')
                                                        }
                                                    </script>--%>
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRevFrom" runat="server" Class="DatePick" Text='<%#Eval("RevFromDate") %>' Enabled="false" onkeypress="return false" onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRevTo" runat="server" Class="DatePick" Text='<%#Eval("RevToDate") %>' Enabled="false" onkeypress="return false" onpaste="return false" onkeydown='allowBackSpace(event,this)'></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>



                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>



                                            </table>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="GVBehave" Width="100%" runat="server" AutoGenerateColumns="False" ShowHeader="false">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table style="width: 100%;">
                                                                            <tr class="HeaderStyle">
                                                                                <td>Goal Name</td>
                                                                                <td>No. days of data and summary unit</td>
                                                                                <td style="width: 25%;">Goal Description</td>
                                                                            </tr>

                                                                            <tr class="RowStyle">
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="lbBehave" runat="server" Text='<%#Eval("Behaviour") %>'></asp:Label></td>
                                                                                <td rowspan="3">
                                                                                    <asp:TextBox ID="txtGoalDesc" Rows="3" Columns="10" Enabled="false" Text='<%#Eval("GoalDesc") %>' runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblFre" runat="server" Text="Frequency"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label ID="lbldays" runat="server" Text='<%#Eval("FRQSlope") %>'></asp:Label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblDura" runat="server" Text="Duration (Minutes)"></asp:Label></td>
                                                                                <td>
                                                                                    <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("DURSlope") %>'></asp:Label></td>
                                                                            </tr>

                                                                            <%--<tr class="RowStyle">
                                                    <td>
                                                        <asp:Label ID="lbBehave" runat="server" Text='<%#Eval("Behaviour") %>'></asp:Label>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <asp:Label ID="lblFre" runat="server" Text="Frequency"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lbldays" runat="server" Text='<%#Eval("FRQSlope") %>'></asp:Label>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <asp:Label ID="lblDura" runat="server" Text="Duration (Minutes)"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("DURSlope") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </td>

                                                </tr>--%>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkInjury" runat="server" />Significant Injuries to Self, Peers, or Staff (If yes, describe below)
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox Height="100px" Width="95%" ID="txtInjury" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkChallenge" runat="server" />Challenges with Crisis Management? (If yes, describe below)
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox Height="100px" Width="95%" ID="txtChallenge" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                                                </tr>



                                                <%-- code here--%>





                                                <%--code end--%>



                                                <tr>
                                                    <td class="mainheadingpanel">Previous Recommendations</td>
                                                </tr>
                                                <tr>
                                                    <td><b>Behavioral</b></td>
                                                </tr>

                                                <tr>
                                                    <td class="auto-style2">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GVPrev" runat="server" AutoGenerateColumns="False" OnRowCommand="GVPrev_RowCommand" ShowFooter="true" OnRowDeleting="GVPrev_RowDeleting" GridLines="None" Width="100%">
                                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                                    <RowStyle CssClass="RowStyle" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Recommendation Notes">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecNotes" runat="server" Enabled="false" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Notes") %>' MaxLength="45"></asp:TextBox>
                                                                                <asp:Label ID="hfPrIdBhv" Text='<%# Eval("PrvsId") %>' runat="server" Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Previous Recommendations">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecomd" runat="server" Enabled="false" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Recommendation") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Person Responsible">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtTimeLine" runat="server" Enabled="false" Text='<%# Eval("PersonResponsible") %>' Font-Size="Small" MaxLength="45"></asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completion Date Goal">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtCDG" runat="server" Enabled="false" Text='<%# Eval("CDGoal") %>' Font-Size="Small" MaxLength="45"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Review Period">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtRevPeriod" runat="server" Enabled="false" Text='<%# Eval("RevPeriod") %>' Font-Size="Small" Width="85%" TextMode="MultiLine" Rows="3" MaxLength="45"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Status Update">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtPerRes" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Status") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completed Date">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtCmpDateDate" runat="server" Text='<%# Eval("CmpDate") %>' Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnDelRowBhv" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" CommandArgument='<%# Eval("PrvsId") %>' OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                <%--<asp:ImageButton ID="btnAddRowbhv" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td><b>Medical</b></td>
                                                </tr>

                                                <%--Medical--%>

                                                <tr>
                                                    <td class="auto-style2">
                                                        <asp:UpdatePanel ID="UpdatePanelMed" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GVMedical" runat="server" AutoGenerateColumns="False" OnRowCommand="GVMedical_RowCommand" ShowFooter="true" OnRowDeleting="GVMedical_RowDeleting" GridLines="None" Width="100%">
                                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                                    <RowStyle CssClass="RowStyle" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Recommendation Notes">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecNotesMed" runat="server" Enabled="false" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Notes_Med") %>' MaxLength="45"></asp:TextBox>
                                                                                <asp:Label ID="hfPrIdMed" Text='<%# Eval("PrvsId") %>' runat="server" Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Previous Recommendations">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecomdMed" runat="server" Enabled="false" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Recommendation_Med") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Person Responsible">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtTimeLineMed" runat="server" Enabled="false" Text='<%# Eval("PersonResponsible_Med") %>' Font-Size="Small" MaxLength="45"></asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completion Date Goal">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtCDGMed" runat="server" Enabled="false" Text='<%# Eval("CDGoal_Med") %>' Font-Size="Small" MaxLength="45"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Review Period">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtRevPeriodMed" runat="server" Enabled="false" Text='<%# Eval("RevPeriod_Med") %>' MaxLength="45" Font-Size="Small" TextMode="MultiLine" Rows="3" Width="85%"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Status Update">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtPerResMed" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Status_Med") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completed Date">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtCmpDateDateMed" runat="server" Text='<%# Eval("CmpDate_Med") %>' Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnDelRowMed" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" CommandArgument='<%# Eval("PrvsId") %>' OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                <%--<asp:ImageButton ID="btnAddRowMed" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>


                                                <%--end--%>
                                                <tr>
                                                    <td><b>Psychiatry</b></td>
                                                </tr>
                                                <%--psychatry--%>

                                                <tr>
                                                    <td class="auto-style2">
                                                        <asp:UpdatePanel ID="UpdatePanelPsy" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GVPsy" runat="server" AutoGenerateColumns="False" OnRowCommand="GVPsy_RowCommand" ShowFooter="true" OnRowDeleting="GVPsy_RowDeleting" GridLines="None" Width="100%">
                                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                                    <RowStyle CssClass="RowStyle" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Recommendation Notes">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecNotesPsy" runat="server" Enabled="false" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Notes_Psy") %>' MaxLength="45"></asp:TextBox>
                                                                                <asp:Label ID="hfPrIdPsy" Text='<%# Eval("PrvsId") %>' runat="server" Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Previous Recommendations">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecomdPsy" runat="server" Enabled="false" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Recommendation_Psy") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Person Responsible">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtTimeLineMPsy" runat="server" Enabled="false" Text='<%# Eval("PersonResponsible_Psy") %>' Font-Size="Small" MaxLength="45"></asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completion Date Goal">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtCDGPsy" runat="server" Enabled="false" Text='<%# Eval("CDGoal_Psy") %>' Font-Size="Small" MaxLength="45"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Review Period">
                                                                            <ItemTemplate>

                                                                                <asp:Label ID="txtRevPeriodPsy" runat="server" Enabled="false" Text='<%# Eval("RevPeriod_Psy") %>' TextMode="MultiLine" Font-Size="Small" MaxLength="45" Rows="3" Width="85%"></asp:Label>

                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Status Update">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtPerResPsy" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Status_Psy") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completed Date">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtCmpDateDatePsy" runat="server" Text='<%# Eval("CmpDate_Psy") %>' Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnDelRowPsy" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" CommandArgument='<%# Eval("PrvsId") %>' OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                <%--<asp:ImageButton ID="btnAddRowPsy" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="mainheadingpanel">Current Recommendations</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>Behavioral Update</b>
                                                    </td>
                                                </tr>

                                                <%--end--%>

                                                <%--bhvup--%>
                                                <tr>
                                                    <td class="auto-style2">
                                                        <asp:UpdatePanel ID="UpdatePanelPUp" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GVBhvUp" runat="server" AutoGenerateColumns="False" OnRowCommand="GVBhvUp_RowCommand" ShowFooter="true" OnRowDeleting="GVBhvUp_RowDeleting" OnRowDataBound="GVBhvUp_RowDataBound" GridLines="None" Width="100%">
                                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                                    <RowStyle CssClass="RowStyle" />
                                                                    <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Notes">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecomdBhvUp" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Notes_Bhv") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Recommendations">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtTimeBhvup" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Recommendation_Bhv") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Person Responsible">
                                                                            <ItemTemplate>
                                                                                <%--<asp:TextBox ID="txtPerBhvUp" runat="server" Text='<%# Eval("PersonResponsible_Bhv") %>' MaxLength="45"></asp:TextBox>--%>
                                                                                <%--<asp:DropDownList ID="ddlPerRespBhv" runat="server"></asp:DropDownList>--%>
                                                                                <div class="hdFldClass">
                                                                                    <asp:TextBox ID="txtPerRespBhv" runat="server" Text='<%# Eval("PersonResponsible_Bhv") %>' class="autosuggest"></asp:TextBox>
                                                                                    <%--<asp:HiddenField ID="hdFldtxtPerRespBhv" runat="server" />--%>
                                                                                    <asp:TextBox ID="hdPerRespBhv" runat="server" Text='<%# Eval("hdPerRespBhv") %>' style="display:none"  Class="hd"></asp:TextBox>
                                                                                </div>
                                                                                <asp:Label ID="hfPerRespBhv" Text='<%# Eval("PersonResponsible_Bhv") %>' runat="server" Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completion Date Goal">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDateBhv" runat="server" Text='<%# Eval("Goal_bhv") %>' Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnDelRowbhvUp" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                <asp:ImageButton ID="btnAddRowbhvUp" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                    </Columns>

                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>








                                                <%--bhvupend--%>







                                                <%--medUp--%>
                                                <tr>
                                                    <td>
                                                        <b>Medical Update</b>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="auto-style2">
                                                        <asp:UpdatePanel ID="UpdatePanelMedUp" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GVMedicalUp" runat="server" AutoGenerateColumns="False" OnRowCommand="GVMedicalUp_RowCommand" ShowFooter="true" OnRowDeleting="GVMedicalUp_RowDeleting" OnRowDataBound="GVMedicalUp_RowDataBound" GridLines="None" Width="100%">
                                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                                    <RowStyle CssClass="RowStyle" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Notes">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecomdMedUp" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Notes_Med_Up") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Recommendations">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtTimeLineMedUp" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Recommendation_Med_Up") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Person Responsible">
                                                                            <ItemTemplate>
                                                                                <%--<asp:TextBox ID="txtPerResMedUp" runat="server" Text='<%# Eval("PersonResponsible_Med_Up") %>' MaxLength="45"></asp:TextBox>--%>
                                                                                <%--<asp:DropDownList ID="ddlPerRespMed" runat="server"></asp:DropDownList>--%>
                                                                                <div class="hdFldClass">
                                                                                    <asp:TextBox ID="txtPerRespMed" runat="server" Text='<%#Eval("PersonResponsible_Med_Up") %>' class="autosuggest"></asp:TextBox>
                                                                                    <%--<asp:HiddenField ID="hdFldtxtPerRespMed"  runat="server" />--%>
                                                                                    <asp:TextBox ID="hdPerRespMed" runat="server" Text='<%# Eval("hdPerRespMed") %>' style="display:none" Class="hd"   ></asp:TextBox>
                                                                                </div>
                                                                                <asp:Label ID="hfPerRespMed" Text='<%# Eval("PersonResponsible_Med_Up") %>' runat="server" Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completion Date Goal">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDateMed" runat="server" Text='<%# Eval("Goal_Med_Up") %>' Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnDelRowMedUp" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                <asp:ImageButton ID="btnAddRowMedUp" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>





                                                <%--medUpEnd--%>


                                                <%--PsyUp--%>
                                                <tr>
                                                    <td>
                                                        <b>Psychiatry Update</b>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td class="auto-style2">
                                                        <asp:UpdatePanel ID="UpdatePanelPsyUp" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="GVPsyUp" runat="server" AutoGenerateColumns="False" OnRowCommand="GVPsyUp_RowCommand" ShowFooter="true" OnRowDeleting="GVPsyUp_RowDeleting" OnRowDataBound="GVPsyUp_RowDataBound" GridLines="None" Width="100%">
                                                                    <HeaderStyle CssClass="HeaderStyle" />
                                                                    <RowStyle CssClass="RowStyle" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Notes">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRecomdPsyUp" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Notes_Psy_Up") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Recommendations">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtTimeLineMPsyUp" runat="server" TextMode="MultiLine" Rows="3" Width="90%" Text='<%# Eval("Recommendation_Psy_Up") %>' MaxLength="45"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Person Responsible">
                                                                            <ItemTemplate>
                                                                                <%--<asp:TextBox ID="txtPerResPsyUp" runat="server" Text='<%# Eval("PersonResponsible_Psy_Up") %>' MaxLength="45"></asp:TextBox>--%>
                                                                                <%--<asp:DropDownList ID="ddlPerRespPsy" runat="server"></asp:DropDownList>--%>
                                                                                <div>
                                                                                    <asp:TextBox ID="txtPerRespPsy" runat="server" Text='<%#Eval("PersonResponsible_Psy_Up") %>' class="autosuggest"></asp:TextBox>
                                                                                    <%--<asp:HiddenField ID="hdFldtxtPerRespPsy"  runat="server" />--%>
                                                                                    <asp:TextBox ID="hdPerRespPsy" runat="server" Text='<%# Eval("hdPerRespPsy") %>' style="display:none" Class="hd"></asp:TextBox>
                                                                                </div>
                                                                                <asp:Label ID="hfPerRespPsy" Text='<%# Eval("PersonResponsible_Psy_Up") %>' runat="server" Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completion Date Goal">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtDatePsy" runat="server" Text='<%# Eval("Goal_Psy_Up") %>' Class="DatePick" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnDelRowPsyUp" CssClass="btn btn-blue" ImageUrl="~/Administration/images/trash.png" runat="server" Text="X" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete?');" />
                                                                                <asp:ImageButton ID="btnAddRowPsyUp" runat="server" ImageUrl="~/Administration/images/plusNew.PNG" CommandName="AddRow" CssClass="btn btn-blue" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>




                                                <%--PsyUpEnd--%>


                                                <tr>
                                                    <td>
                                                        <div id="divSubByOn" runat="server">
                                                            <asp:Label Text="Submitted By: " runat="server" Style="margin-left: 10%;"></asp:Label>
                                                            <asp:Label ID="lblSubBy" runat="server"></asp:Label>

                                                            <asp:Label Text="Submitted On: " runat="server" Style="margin-left: 50%;"></asp:Label>
                                                            <asp:Label ID="lblSubOn" runat="server"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <asp:Button ID="btnDraft" runat="server" Text="Save Draft" OnClick="btnSave_Click" CssClass="NFButton" OnClientClick="scrollToTop();" />
                                                        <asp:Button ID="btnSave" runat="server" Text="Submit" OnClick="btnSave_Click" CssClass="NFButton" OnClientClick="scrollToTop();"/>
                                                    </td>
                                                </tr>
                                            </table>











                                            <%-- hello--%>
                                        </div>


                                    </asp:Panel>

                                </td>

                            </tr>
                        </table>
                        <div id="overlay" class="web_dialog_overlay"></div>
                        <div id="dialog" class="web_dialog" style="width: 900px; margin-left: -22%;">

                            <div id="sign_up5" style="width: 100%; margin-left: 10px">
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
                                            <asp:Button ID="btnGenRTF" runat="server" Text="Generate" OnClick="btnGenRTF_Click" CssClass="NFButton" />
                                            <input type="button" id="CancalGen" class="NFButton" value="Cancel" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>

                        <div id="dashMain" style="display: none; position: fixed; top: 0; left: 40px; background-color: white; border: 2px solid black;">
                            <div style="width: 100%; height: 30px;"><a>
                                <img src="../Administration/images/button_red_close.PNG" onclick="javascript:$('#dashMain').fadeOut();" style="float: right;" /></a></div>
                            <div id="dashSub" style="width: 1200px; height: 500px; margin: 10px; overflow-y: scroll; overflow-x: scroll;">
                                <h2>Dashboard (All Recommendations)</h2>
                                <br />
                                <asp:UpdatePanel ID="upDashboard" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>Recommedndation Type</td>
                                                <td>Completed Status</td>
                                                <td>From (Submitted Date)</td>
                                                <td>To (Submitted Date)</td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlDashboardRecType" runat="server" CssClass="drpClass">
                                                        <asp:ListItem>All</asp:ListItem>
                                                        <asp:ListItem>Behavioral</asp:ListItem>
                                                        <asp:ListItem>Medical</asp:ListItem>
                                                        <asp:ListItem>Psychiatry</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDashboardCmpDate" runat="server" CssClass="drpClass">
                                                        <asp:ListItem>All</asp:ListItem>
                                                        <asp:ListItem>Completed</asp:ListItem>
                                                        <asp:ListItem>Not Completed</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCalendarFrom" runat="server" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" CssClass="DatePickNew" Width="100px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCalendarTo" runat="server" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" CssClass="DatePickNew" Width="100px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDashboardFilter" runat="server" CssClass="NFButton" OnClick="btnDashboardFilter_Click" Text="Search" />
                                                </td>
                                            </tr>
                                        </table>
                                        <script type="text/javascript">
                                            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (evt, args) {
                                                $('.DatePickNew').datepicker();
                                            });
                                        </script>
                                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" GridLines="None" EmptyDataText="No Data Found" ForeColor="#333333" Style="width: 100%;">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                        </asp:GridView>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>


                </div>
            </div>

        </div>
        <%--<asp:TextBox ID="txtpramod" BorderColor="Red" runat="server"></asp:TextBox>
        <script type="text/javascript">
            $(document).ready(function () {
                $("#txtpramod").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8',
                            url: 'GrandRoundCvrsht.aspx/GetAutoCompleteData',
                            data: "{'prefix':'" + document.getElementById('txtpramod').value + "'}",
                            dataType: "json",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        label: item.split('-')[0],
                                        val: item.split('-')[1]
                                        //value:item
                                        //label: item
                                        //val:item
                                    }
                                }))
                            }
                        })
                    },
                    select: function (event, ui) {
                        if (ui.item) {

                        }
                    }

                });
            });
          </script>--%>
        <script type="text/javascript">
            $(document).ready(function SearchText() {
                $(".autosuggest").autocomplete({
                    open: function () {
                        $('.ui-menu')
                        .width(180);
                    },
                    source: function (request, response) {
                        
                        $.ajax({
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8',
                            url: 'GrandRoundCvrsht.aspx/GetAutoCompleteData',
                            //data: "{'prefix': '" + $(this).val() + "' }",
                            data: "{'prefix': '" + request.term + "'}",
                            dataType: "json",
                            success: function (data) {
                                response($.map(data.d, function (item) {
                                    return {
                                        //value: item.split('-')[0],
                                        //label: item.split('-')[1]
                                        //value: item
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
            });
        </script>
        
        <script type="text/javascript">
            function destroy() {

                //var hds = $('.hd');
                //for (var i = 0; i < hds.length; i++) {
                //    alert(i + ": " + $(hds[i]).val());
                //}
                
                //alert("in destroy");
                if (jQuery(".autosuggest").data('autocomplete')) {
                    jQuery(".autosuggest").autocomplete("destroy");
                    jQuery(".autosuggest").removeData('autocomplete');
                };
               

                //hds = $('.hd');
                //for (var i = 0; i < hds.length; i++) {
                //    alert($(hds[i]).val());
                //}

                $(".autosuggest").autocomplete({
                    open: function () {
                        $('.ui-menu')
                        .width(180);
                    },
                    source: function (request, response) {
                        //alert(request.term);
                        $.ajax({
                            type: 'POST',
                            contentType: 'application/json; charset=utf-8',
                            url: 'GrandRoundCvrsht.aspx/GetAutoCompleteData',
                            //data: "{'prefix': '" + $(this).val() + "' }",
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
                            
                            //alert($(this).parent().find('input:hidden').length);
                            $(this).val(ui.item.value);
                            $(this).parent().find('.hd').val(ui.item.val);
                            //var divs = $('.hdFldClass');
                            //alert("Length of divs is");
                            //alert(divs.length);
                            //for (i = 0; i < divs.length; i++) {
                            //    alert("In divs function");
                            //    //alert($(divs[i]).find('input:hidden').length);
                            //    //alert($(divs[i]).find('input:hidden').val(ui.item.value).length);
                            //    alert($(divs[i]).find('input:hidden').val(ui.item.val).val);
                            //}
                            //alert($(this).parent().find('input:hidden').val(ui.item.val));
                            //return $(this).parent().find('input:hidden').val(ui.item.val);
                            return false;
                        }
                    }

                });
            }

        </script>
    </form>
</body>
</html>
