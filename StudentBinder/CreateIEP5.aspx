<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/StudentBinder/CreateIEP5.aspx.cs" Inherits="StudentBinder_CreateIEP5" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <%-- <link rel="stylesheet" href="jsForCalender/jquery.ui.all.css">--%>


    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="../Administration/JS/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.ui.datepicker.js"></script>
    <link href="../Administration/CSS/demos.css" type="text/css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />

    <link href="../Administration/CSS/jquery.ui.base.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/jquery.ui.theme.css" rel="stylesheet" type="text/css" />

    <script src="../Administration/JS/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.fileupload.js" type="text/javascript"></script>
    <script src="../Administration/JS/jquery.iframe-transport.js" type="text/javascript"></script>

    <script src="../Administration/JS/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../Administration/CSS/jquery-ui.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        $(function () {
            $(".DatePick").datepicker();
        });


    </script>



    <style type="text/css">
        .HeaderStyle {
            background: none repeat scroll 0 0 #7EABBE;
            color: #FFFFFF !important;
            font-weight: bold;
            font-family: Arial;
            font-size: 12px;
            height: 30px;
            letter-spacing: 0;
            line-height: 12px;
            padding: 0 8px !important;
            text-align: left;
            text-decoration: none;
            vertical-align: middle;
        }

        .gridStyle {
            border: 1px solid #d8dbdb;
            margin: 10px 0 10px 0;
            border-spacing: 0;
            border-collapse: collapse;
        }

            .gridStyle tr:nth-child(even) {
                background: #F4F4F4;
                color: #4F5050;
                font-family: Arial;
                font-size: 11px;
                height: 30px;
                letter-spacing: 0;
                line-height: 12px;
                padding: 0 8px !important;
                text-align: left;
                text-decoration: none;
                vertical-align: middle;
            }


            .gridStyle tr:nth-child(odd) {
                font-family: Arial;
                font-size: 11px;
                height: 30px;
                letter-spacing: 0;
                line-height: 12px;
                padding: 0 8px !important;
                text-align: left;
                text-decoration: none;
                vertical-align: middle;
                color: #4F5050;
            }



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
    </style>

    <script type="text/javascript">
        $(function () {
            $("[id$=txtStartDateA]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../StudentBinder/img/Calendar24.png'
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("[id$=txtEndDateA]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: '../StudentBinder/img/Calendar24.png'
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $("[id$=txtStartDateB]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: "../StudentBinder/img/Calendar24.png"
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $("[id$=txtEndDateB]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: "../StudentBinder/img/Calendar24.png"
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("[id$=txtStartDateC]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: "../StudentBinder/img/Calendar24.png"
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("[id$=txtEndDateC]").datepicker({
                showOn: 'button',
                buttonImageOnly: true,
                buttonImage: "../StudentBinder/img/Calendar24.png"
            });
        });
    </script>
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


        <br />

        <div style="height: auto; border-radius: 5px; width: 100%;">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" style="border-bottom: 2px double gray; text-align: center; color: rgb(8, 75, 43); font-family: Arial; font-weight: bold; font-size: 18px; height: 35px;">Service Delivery
                    </td>
                </tr>
                <tr>
                    <td id="tdMsg" runat="server"></td>
                </tr>
                <tr>
                    <td>

                        <table>
                            <tr>
                                <td class="tdText">What are the total service delivery needs of this student?
                    
                                </td>
                                <td>&nbsp;


                                    <asp:TextBox ID="txtOther5" runat="server" CssClass="txtClassShort" MaxLength="45"></asp:TextBox>
                                </td>


                            </tr>

                            <tr>
                                <td class="tdText">School District Cycle</td>
                                <td>

                                    <asp:RadioButtonList ID="rblSchoolDistCycle" runat="server"
                                        RepeatDirection="Horizontal" CssClass="checkBoxStyle">
                                        <asp:ListItem Value="0">5 day cycle</asp:ListItem>
                                        <asp:ListItem Value="1">6 day cycle</asp:ListItem>
                                        <asp:ListItem Value="2">10 day cycle</asp:ListItem>
                                        <asp:ListItem Value="3">Other</asp:ListItem>
                                    </asp:RadioButtonList>

                                </td>
                            </tr>

                        </table>
                        <table style="width: 100%;">
                            <tr>
                                <td class="tdText" style="text-align: center; background-color: #E3E3E3;">
                                    <strong>A. Consultation (Indirect Services to School Personnel and Parents)</strong>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>

                            <tr style="height: 25px;">
                                <td class="tdText" style="text-align: center">Focus&nbsp; on&nbsp; Goal Type&nbsp; of&nbsp; Service&nbsp; Type&nbsp; of&nbsp; 
        Personnel&nbsp; Frequency&nbsp; and&nbsp; Duration/PerCycle Start Date End Date</td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:GridView ID="gvDelTypeA" runat="server" AutoGenerateColumns="False" CssClass="gridStyle" GridLines="None" Width="100%"
                                        ShowFooter="True" OnRowDataBound="gvDelTypeA_RowDataBound"
                                        OnRowCommand="gvDelTypeA_RowCommand" Style="z-index: 1">

                                        <Columns>
                                            <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("StdtGoalSvcId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Goal">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td class="tdText" style="vertical-align: top;">

                                                                <asp:TextBox ID="txtFocusOnGoalA" CssClass="textClass" runat="server" Width="146px" Height="35px" TextMode="MultiLine"></asp:TextBox>
                                                                <br />
                                                                <asp:Label ID="lbl_goalId" runat="server" Text='<%# Eval("GoalId") %>' Visible="false"></asp:Label>
                                                            </td>



                                                        </tr>

                                                    </table>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type of Services">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="text-align: center; vertical-align: top;">
                                                                <asp:TextBox ID="txtTypeOfServiceA" runat="server" CssClass="textClass" Width="146px" Height="35px"
                                                                    Text='<%#Eval("SvcTypDesc") %>' TextMode="MultiLine" Rows="5"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type of Personal">
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td style="text-align: center; vertical-align: top;">
                                                                <asp:TextBox ID="txtTypeOfPersonnelA" runat="server" CssClass="textClass" Width="146px" Height="35px"
                                                                    Text='<%#Eval("PersonalTypDesc") %>' TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>


                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frequency">
                                                <ItemTemplate>


                                                    <table>
                                                        <tr>
                                                            <td style="text-align: center; vertical-align: top;">
                                                                <asp:TextBox ID="txtFrequencyA" runat="server" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px"
                                                                    Text='<%#Eval("FreqDurDesc") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Start Date">
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td style="text-align: center; vertical-align: bottom;">
                                                                <asp:TextBox ID="txtStartDateA" runat="server" Width="80px" Text='<%#Eval("StartDate") %>' CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false" ></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>




                                                </ItemTemplate>


                                            </asp:TemplateField>




                                            <asp:TemplateField HeaderText="End Date" ItemStyle-HorizontalAlign="Right">
                                                <FooterTemplate>
                                                    <asp:Button ID="ButtonAdd" runat="server" OnClick="ButtonAdd_Click"
                                                        Text="Add New Row" CssClass="NFButton" />

                                                </FooterTemplate>
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td style="text-align: center; vertical-align: bottom;">

                                                                <asp:TextBox ID="txtEndDateA" runat="server" CssClass="textClass" Width="80px" Text='<%#Eval("EndDate") %>' onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>

                                                            </td>
                                                        </tr>

                                                    </table>




                                                </ItemTemplate>


                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>


                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lnk_Delete0" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#CCCCCC" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                    <td id="tdMsgB" runat="server"></td>
                </tr>
                            <tr>
                                <td class="tdText" style="text-align: center; background-color: #E3E3E3;">&nbsp;<strong>B. Special Education and Relate Services in General Education Classroom (Direct
                                    Service) </strong>

                                </td>

                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr style="height: 25px;">
                                <td class="tdText" style="text-align: center">

                                    <span class="style8">Focus&nbsp; on Goal </span>&nbsp;<span class="style8">Type&nbsp; of&nbsp; Service</span>
                                    <span class="style8">&nbsp;Type&nbsp; of&nbsp; Personnel </span>&nbsp;<span class="style8">Frequency&nbsp; and&nbsp; Duration/PerCycle </span>&nbsp;<span class="style8">Start Date</span> <span class="style8">End Date</span>

                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:GridView ID="gvDelTypeB" runat="server" AutoGenerateColumns="False" CssClass="gridStyle" Width="100%"
                                        GridLines="None" ShowFooter="True"
                                        OnRowDataBound="gvDelTypeB_RowDataBound"
                                        OnRowCommand="gvDelTypeB_RowCommand">

                                        <Columns>
                                            <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("StdtGoalSvcId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Goal">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:TextBox ID="txtFocusOnGoalB" runat="server" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px">
                                                                </asp:TextBox>
                                                                <br />
                                                                <asp:Label ID="lbl_goalId1" runat="server" Text='<%# Eval("GoalId") %>' Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type of Service">
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTypeOfServiceB" runat="server" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px"
                                                                    Text='<%#Eval("SvcTypDesc") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type of Personal">
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTypeOfPersonnelB" runat="server" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px"
                                                                    Text='<%#Eval("PersonalTypDesc") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frequency">
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtFrequencyB" runat="server" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px"
                                                                    Text='<%#Eval("FreqDurDesc") %>'></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start Date">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStartDateB" runat="server" CssClass="textClass" Width="80px" Text='<%#Eval("StartDate") %>' onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>




                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Date">
                                                <FooterTemplate>
                                                    <asp:Button ID="ButtonAddB" runat="server" CssClass="NFButton"
                                                        OnClick="ButtonAddB_Click" Text="Add New Row" />

                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtEndDateB" runat="server" CssClass="textClass" Width="80px" Text='<%#Eval("EndDate") %>' onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>




                                                </ItemTemplate>


                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lnk_Delete1" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                            </td>
                                                        </tr>

                                                    </table>


                                                </ItemTemplate>


                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#CCCCCC" />

                                    </asp:GridView>

                                </td>
                            </tr>
                            <tr>
                                <td id="tdMsgC" runat="server"></td>
                            </tr>

                            <tr>
                                <td class="tdText" style="text-align: center; background-color: #E3E3E3;">

                                    <strong>C. Special Education and Relate Services in Other Settings (Direct Service)&nbsp;</strong>

                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>

                            <tr style="height: 25px;">
                                <td class="tdText" style="text-align: center">Focus&nbsp; on&nbsp; Goal&nbsp; Type&nbsp; of&nbsp; Service&nbsp; Type&nbsp; of&nbsp; Personnel&nbsp; Frequency&nbsp; and&nbsp;
                    Duration/PerCycle&nbsp; Start Date End Date</td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:GridView ID="gvDelTypeC" runat="server" ShowFooter="True" CssClass="gridStyle" AutoGenerateColumns="False" Width="100%"
                                        GridLines="None" OnRowDataBound="gvDelTypeC_RowDataBound"
                                        OnRowCommand="gvDelTypeC_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="StdtGoalSvcId" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_svcid" runat="server" Text='<%# Eval("StdtGoalSvcId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Goal">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:TextBox ID="txtFocusOnGoalC" runat="server" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px">
                                                                </asp:TextBox>
                                                                <br />
                                                                <asp:Label ID="lbl_goalId" runat="server" Text='<%# Eval("GoalId") %>' Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type of Service">
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtTypeOfServiceC" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px" Text='<%#Eval("SvcTypDesc") %>' runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type of Personal">
                                                <ItemTemplate>


                                                    <table>
                                                        <tr>
                                                            <td class="tdText" style="text-align: center;">
                                                                <asp:TextBox ID="txtTypeOfPersonnelC" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px" Text='<%#Eval("PersonalTypDesc") %>' runat="server"></asp:TextBox></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frequency">
                                                <ItemTemplate>


                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtFrequencyC" CssClass="textClass" Rows="5" TextMode="MultiLine" Width="146px" Height="35px" Text='<%#Eval("FreqDurDesc") %>' runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </ItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start Date">
                                                <ItemTemplate>


                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtStartDateC" CssClass="textClass" Width="80px" Text='<%#Eval("StartDate") %>' runat="server" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>





                                                </ItemTemplate>



                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Date">
                                                <FooterTemplate>
                                                    <asp:Button ID="ButtonAddC" runat="server" CssClass="NFButton" OnClick="ButtonAddC_Click" Text="Add New Row" />



                                                </FooterTemplate>
                                                <ItemTemplate>


                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtEndDateC" CssClass="textClass" Width="80px" Text='<%#Eval("EndDate") %>' runat="server" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>


                                                </ItemTemplate>


                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemTemplate>

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lnk_Delete" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName="remove">X</asp:LinkButton>
                                                            </td>
                                                        </tr>

                                                    </table>


                                                </ItemTemplate>


                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#CCCCCC" />
                                    </asp:GridView>
                                </td>
                            </tr>

                            <tr>
                                <td style="text-align: center">

                                    <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage" Text="Save and continue"
                                        OnClick="btnSave_Click" />

                                     <%--  <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage" Text="dummy"
                                        OnClick="btnSave_hdn_Click" style="display:none;" />--%>
                                </td>
                            </tr>

                        </table>

                    </td>
                </tr>

                <tr>
                    <td>



                        <asp:Label ID="Label2" runat="server" Text="Label" Visible="false"></asp:Label>


                    </td>

                </tr>



            </table>

        </div>
        <asp:HiddenField ID="txtstartDateC" runat="server" Value="" />
        <asp:HiddenField ID="txtendDateC" runat="server" Value="" />

        <asp:HiddenField ID="txtendDateB" runat="server" Value="" />
        <asp:HiddenField ID="txtstartDateB" runat="server" Value="" />

        <asp:HiddenField ID="txtstartDate" runat="server" Value="" />
        <asp:HiddenField ID="txtendDate" runat="server" Value="" />


    </form>



</body>
</html>
