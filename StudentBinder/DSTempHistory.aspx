<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DSTempHistory.aspx.cs" Inherits="StudentBinder_DSTempHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" />
    <link href="../Administration/CSS/jsDatePick_ltr.min.css" rel="stylesheet" />
    <script src="../Administration/JS/jsDatePick.min.1.3.js"></script>
    <script src="../Administration/JS/jquery.min.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.19.custom.min.js"></script>


    <link rel="stylesheet" type="text/css" media="all" href="../Administration/jsDatePick_ltr.min.css" />
    <script type="text/javascript" src="../Administration/jsDatePick.min.1.3.js"></script>

    <script>
        window.location.hash = "no-back-button";
        window.location.hash = "Again-No-back-button";//again because google chrome don't insert first hash into history
        window.onhashchange = function () { window.location.hash = "no-back-button"; }
    </script> 

    <%--<script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="../Administration/JS/jquery-ui-1.8.23.custom.min.js"></script>--%>

    <script type="text/javascript" language="javascript">
       

        window.onload = function () {
            new JsDatePick({
                useMode: 2,
                target: "<%=inputstart.ClientID%>",
                dateFormat: "%m/%d/%Y",
            });
             new JsDatePick({
                 useMode: 2,
                 target: "<%=inputend.ClientID%>",
                 dateFormat: "%m/%d/%Y",
            });


         };
        



    </script>
    <script>
        var multiClickSearch = false;

        function multiClickCheckSearch(btn) {
            if (!multiClickSearch) {
                multiClickSearch = true;
                btn.style.opacity = '0.5';
                return true;
            }
            return false;
        }
        function enableButton() {
            multiClickSearch = false;
        }
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
    </script>
   

    <style type="text/css">

        input[type=text] {
            
            width: 100px !important;
        }
        .table {
            width: 98%;
        }
   
        .Initial {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            /*background: url("../Images/InitialImage.png") no-repeat right top;*/
            background-color: #0054A0;
            color: Black;
            font-weight: bold;
        }

            .Initial:hover {
                color: White;
                /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
                background-color: #0099B5;
            }

        .Clicked {
            float: left;
            display: block;
            /*background: url("../Images/SelectedButton.png") no-repeat right top;*/
            background-color: #1EB53A;
            padding: 4px 18px 4px 18px;
            color: Black;
            font-weight: bold;
            color: White;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="tdMsg" runat="server">
        </div>

        <div>


            <table class="table">
                <tr>
                    <td style="width: 10%">LessonPlan : </td>
                    <td style="width: 20%"><asp:DropDownList ID="ddlLessonPlans" CssClass="drpClass" runat="server" AutoPostBack="false" ></asp:DropDownList></td>
                    <td style="width: 10%">Start Date : </td>
                    <td style="width: 20%"><asp:TextBox ID="inputstart" runat="server"  CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>

                        
                    </td>
                    <td style="width: 10%">End Date : </td>
                    <td style="width: 20%">
                        <asp:TextBox ID="inputend" runat="server"  CssClass="textClass" onkeypress="return false" onkeydown='allowBackSpace(event,this)' onpaste="return false"></asp:TextBox>
                       
                    </td>
                    <td style="width: 10%">
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" OnClientClick="return multiClickCheckSearch(this);" CssClass="NFButton"  /></td>
                    
                    
                </tr>
            </table>


        </div>
        <div style="height:20px"></div>
        <div>
             <asp:Button Text="Session Details" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                        OnClick="Tab1_Click" ForeColor="White" />
                    &nbsp;&nbsp; 
          <asp:Button Text="Session Score Details" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
              OnClick="Tab2_Click" ForeColor="White" />
            </div>
        <div style="clear:both;height:20px;"></div>
        <div>
            <asp:MultiView ID="MainView" runat="server">
                <asp:View ID="View1" runat="server">
        
        <div>
            <table class="table">
                <tr>

                    <td style="width: 100%" colspan="2">
                        <asp:GridView ID="gvView" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found.." PageSize="12" GridLines="None" BackColor="White" BorderColor="#336666" BorderStyle="None" BorderWidth="3px" OnRowCommand="gvView_RowCommand" Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvView_PageIndexChanging">

                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                            <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />

                            <RowStyle CssClass="RowStyle" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                            <AlternatingRowStyle CssClass="AltRowStyle" />
                            <Columns>
                                <asp:BoundField DataField="SessionNbr" HeaderText="Session Number" />
                                <asp:BoundField DataField="SetName" HeaderText="Current Set" />
                                <asp:BoundField DataField="CurrPrompt" HeaderText="Current Prompt" />
                                <asp:BoundField DataField="LessonPlanName" HeaderText="LessonPlanName" />
                                <asp:BoundField DataField="SessionStatusCd" HeaderText="Status" />
                                <asp:BoundField DataField="StartTs" HeaderText="Start Date" />
                                <asp:BoundField DataField="EndTs" HeaderText="End Date" />
                                <asp:BoundField DataField="User" HeaderText="User" />
                                <asp:CheckBoxField DataField="IsMaintanance" HeaderText="Is Maintanance" ReadOnly="True"></asp:CheckBoxField>
                                <asp:TemplateField HeaderText="View/Edit">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgView" runat="server" class="btn btn-purple" CommandArgument='<%# Eval("StdtSessionHdrId") %>' CommandName="View" Height="18px" ImageUrl="~/Administration/Images/view_02.png" Style="cursor: pointer;" Width="18px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" />
                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle CssClass="RowStyle" />
                            <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                        </asp:GridView>

                    </td>

                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <br />

        </div>
                    </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:GridView ID="GrdScore" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found.." PageSize="12" GridLines="None" BackColor="White" BorderColor="#336666" BorderStyle="None" BorderWidth="3px" Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="GrdScore_PageIndexChanging" OnRowDataBound="GrdScore_RowDataBound">
                            <RowStyle CssClass="RowStyle" />
                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />

                            <Columns>
                                <asp:BoundField DataField="SessionNbr" HeaderText="Session Number" />
                                <asp:BoundField HeaderText="Status" DataField="SessionStatusCd" />
                                <asp:BoundField DataField="StartTs" HeaderText="Start Date" />
                                <asp:BoundField DataField="EndTs" HeaderText="End Date" />
                                <asp:BoundField DataField="User" HeaderText="User" />
                                <asp:TemplateField HeaderText="IOA">
                                    <ItemTemplate>
                                       <asp:CheckBox ID="chkIOA" runat="server" Checked='<%# (Eval("IOA").ToString()=="Y")? true:false %>' Enabled="false"/>
                                        <asp:HiddenField ID="hdnsesshdr" runat="server" Value='<%# Eval("StdtSessionHdrId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:GridView ID="grdCalcscore" runat="server" AutoGenerateColumns="False" ShowHeader="False" GridLines="None" Width="100%">
                                            <RowStyle CssClass="RowStyle" />
                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                                            <Columns>
                                                <asp:BoundField DataField="CalcType" />
                                                <asp:BoundField DataField="Score" />
                                            </Columns>
                                            <AlternatingRowStyle CssClass="AltRowStyle" />
                                        </asp:GridView>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                            <AlternatingRowStyle CssClass="AltRowStyle" />
                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" />
                            <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />

                            <RowStyle CssClass="RowStyle" />
                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                        </asp:GridView>
                     </asp:View>
                </asp:MultiView>
        </div>
    </form>
</body>
</html>
