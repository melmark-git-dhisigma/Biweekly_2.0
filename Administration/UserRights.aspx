<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true" CodeFile="UserRights.aspx.cs" Inherits="Admin_UserRights" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style type="text/css">
        body {
        }

        .content {
            width: 245px;
            height: 600px;
            overflow: auto;
        }

        .content_3 {
            height: 220px;
            border: 1px dashed #09C;
            padding: 10px 5px 10px 5px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
        }

            .content_3 p:nth-child(3n+0) {
                color: #09C;
            }

        .auto-style1 {
            height: 36px;
        }
        .auto-style2 {
            height: 36px;
            width: 151px;
        }
        .auto-style3 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            width: 200px;
        }
        .auto-style4 {
            height: 36px;
            width: 78px;
        }
        .auto-style7 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            width: 78px;
        }
        .auto-style8 {
            font-family: Arial;
            color: #666;
            padding-right: 1px;
            text-align: left;
            width: 151px;
        }
        div.lftPartContainer {
                width: 40%;
                min-height: 300px;
                height: auto !important;
                height: auto;
                margin: 5px 0 5px 5px;
                float: left;
            }
        div.containerMain {
            width: 100%;
            margin: 0 auto;
            min-height: 500px;
            height: auto !important;
            height: auto;
            font-family: Arial, Helvetica, sans-serif;
        }
        div.rightPartContainer {
                width: 40%;
                min-height: 300px;
                height: auto !important;
                height: auto;
                margin: 5px 0 5px 5px;
                float: right;
                /*background: #0d668e;*/
            }
    </style>

    <link href="CSS/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    
    
    <div class="containerMain">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width:100%"><tr><td colspan="2" id="tdMsg" runat="server" style="height:35px" class="tdText"></td></tr><tr><td></td><td></td></tr>
      
    </table>
        <div class="lftPartContainer">
                        <table width="100%">
                <tr>
                    <td align="center" class="auto-style7">&nbsp;</td>
                    <td align="center" class="auto-style8">Description</td>
                    <td style="width:2px"></td>
                    <td align="left">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="textClass" Width="278px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="center" class="auto-style7">&nbsp;</td>
                    <td align="center" class="auto-style8">Select User</td>
                    <td style="width:2px; color:red">*</td>
                    <td align="left">
                        <asp:DropDownList ID="ddlUser" runat="server" CssClass="drpClass"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" Width="295px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="auto-style7">&nbsp;</td>
                    <td align="center" class="auto-style8">Select Groups</td>
                    <td style="width:2px;color:red">*</td>
                    <td align="left">
                        <asp:DropDownList ID="ddlGroups" runat="server" AutoPostBack="True"
                            CssClass="drpClass"
                            OnSelectedIndexChanged="ddlGroups_SelectedIndexChanged" Width="295px">
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td align="center" class="auto-style7" valign="top">
                        &nbsp;</td>
                    <td align="center" class="auto-style8" valign="top">
                        <asp:Label ID="lblrole" runat="server" Text="Select Role" Visible="False"></asp:Label>
                    </td>
                    <td style="width:2px;vertical-align:top">
                        <asp:Label ID="lblrolestar" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:CheckBoxList ID="chkRole" runat="server" CssClass="textClass" Enabled="False" Width="295px">
                        </asp:CheckBoxList>
                    </td>
                </tr>

                <tr>
                    <td align="center" class="auto-style7" valign="top">&nbsp;</td>
                    <td align="center" class="auto-style8" valign="top">&nbsp;</td>
                    <td align="left" colspan="2">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="btnSave_Click" Text="Save" />
                    </td>
                </tr>
                <tr>
                    <td align="center" class="auto-style3" valign="top" colspan="4">&nbsp;</td>
                    
                </tr>

               

            </table>

      </div>
    <div class="rightPartContainer" >
        <table style="width:100%"><tr>
            <td><asp:GridView ID="grdGPRole" runat="server" AutoGenerateColumns="False" GridLines="None">
                <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                    <RowStyle CssClass="RowStyle" />
                    <AlternatingRowStyle CssClass="AltRowStyle" />
                    <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                    <SelectedRowStyle CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="GroupDesc" HeaderText="Group" />
                        <asp:BoundField DataField="RoleCode" HeaderText="Role Code" />
                        <asp:BoundField DataField="RoleDesc" HeaderText="Role Description" />
                </Columns>
                    <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                </asp:GridView></td></tr></table>
        
    </div>
              </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    

    

</asp:Content>

