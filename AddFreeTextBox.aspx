<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFreeTextBox.aspx.cs" ValidateRequest="false" Inherits="AddFreeTextBox" %>

<meta http-equiv="X-UA-Compatible" content="IE=9">
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Administration/JS/jquery-1.8.0.js"></script>
    <title></title>

    <style type="text/css">
        .FreeTextBox1_OuterTable select {
            border-radius: 2px !important;
            border: 1px solid #808080;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div>


            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

          
                    <FTB:FreeTextBox ID="FreeTextBox1" Focus="true" SupportFolder="FreeTextBox/" Width="100%" Height="55px"
                        JavaScriptLocation="ExternalFile" ButtonImagesLocation="ExternalFile" ToolbarImagesLocation="ExternalFile"
                        ToolbarStyleConfiguration="OfficeXP" ToolbarLayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,                                   
FontForeColorPicker,FontBackColorsMenu,FontBackColorPicker,Bold,Italic,Underline,RemoveFormat,JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList"
                        runat="Server" DesignModeCss="designmode.css" ButtonSet="Office2000" BackColor="White" EnableHtmlMode="False" />

                    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
                
        </div>
    </form>
</body>
</html>
