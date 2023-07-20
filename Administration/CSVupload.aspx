<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="CSVupload.aspx.cs" Inherits="Admin_CSVupload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .divGrid {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
        }

        .auto-style2 {
            width: 419px;
        }

        .auto-style3 {
            width: 188px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <table width="100%">

        <tr>
            <td colspan="4" runat="server" id="tdMsg"></td>
        </tr>
        <tr>
            <td class="tdText" style="text-align: center">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>


            <td class="auto-style3">&nbsp;</td>


            <td style="vertical-align: bottom">&nbsp;</td>


        </tr>
        <tr>
            <td class="tdText" style="text-align: center">&nbsp;</td>
            <td class="auto-style2">&nbsp;</td>


            <td class="auto-style3">&nbsp;</td>


            <td style="vertical-align: bottom">&nbsp;</td>


        </tr>
        <tr>
            <td class="tdText" style="text-align: center">Select File
                       
            </td>
            <td class="auto-style2">
                <asp:FileUpload ID="fileSelect" runat="server" CssClass="textClass" class="validate[required]" />

                &nbsp&nbsp&nbsp<asp:Button ID="btnRead" runat="server" ValidationGroup="a" Text="Read" OnClick="btnRead_Click1"
                    CssClass="NFButton" />


            </td>


            <td class="auto-style3">
                <asp:Button ID="btn_Save" runat="server" CssClass="NFButton" OnClick="btn_Save_Click"
                    Text="Save" ValidationGroup="B" Visible="False" />


            </td>


            <td style="vertical-align: bottom">Download the template
                <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click" Font-Underline="True" ForeColor="#3333CC"><b>here</b></asp:LinkButton>


            </td>


        </tr>
        <tr>
            <td class="tdText" style="text-align: center" colspan="4">&nbsp;</td>
        </tr>
    </table>

    <div runat="server" style="overflow: auto;width:100%">

        <asp:GridView ID="gdSample" runat="server" ForeColor="#333333" Font-Names="Arial,Helvetica,sans-serif"
            Font-Size="Small" HorizontalAlign="Justify" Style="margin-left: 0px;" Height="40px" PageSize="2">
            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

            <RowStyle CssClass="RowStyle" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
            <PagerStyle CssClass="PagerStyle" BackColor="#ccccff" HorizontalAlign="Center" />
            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#487575" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#275353" />
        </asp:GridView>
        </div>
  
   
</asp:Content>
