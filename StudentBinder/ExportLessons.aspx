<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportLessons.aspx.cs" Inherits="StudentBinder_ExportLessons" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="asp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style3 {
            width: 247px;
            height: 62px;
        }
        .auto-style5 {
            width: 178px;
            height: 62px;
        }

        .HeaderStyleLE {
    font-family: Arial;
    color: #fff;
    background: #03507d;
    height: 30px;
    line-height: 12px;
    padding: 0 8px!important;
    text-align: center;
    vertical-align: middle;
    font-size: 18px;
    letter-spacing: 0;
    text-decoration: none;
    /*font-weight:bold;*/
   
        }
        .tdTextWithoutWidthLP {
    font-family: Calibri;
    color: #000;
    line-height: 22px;
    /*font-weight: bold;*/
    font-size: 15px;
    padding-right: 1px;
    text-align: right;
        }
        .auto-style8 {
            height: 62px;
        }
        .auto-style10 {
            font-family: Calibri;
            color: #000;
            line-height: 22px;
            /*font-weight: bold;*/
            font-size: 15px;
            padding-right: 1px;
            text-align: right;
            width: 107px;
        }
        .auto-style11 {
            font-family: Calibri;
            color: #000;
            line-height: 22px; /*font-weight: bold;*/;
            font-size: 15px;
            padding-right: 1px;
            text-align: right;
            height: 31px;
        }
        .auto-style12 {
            width: 247px;
            height: 31px;
        }
        .auto-style13 {
            height: 31px;
        }
        .auto-style14 {
            font-family: Calibri;
            color: #000;
            line-height: 22px; /*font-weight: bold;*/
            font-size: 15px;
            padding-right: 1px;
            text-align: right;
            width: 88px;
        }
        .drpClassShortdm {
            border: 1px solid #d7cece;
            width: 175px ;
            height:22px !important;
            font-family: Arial, Helvetica, sans-serif !important;
        }
        
    </style>
</head>
    
<body>

        <script>     
            
            function checkPostbackExport()
            {
                var e = document.getElementById("ddlFormat");
                    var value = e.selectedIndex;
                    if (value == 0) {
                        alert('Please select a format.');
                        return false;
                    }
                alert("Please wait it will take less than few minutes....");
            }

        </script>

    <form id="form1" runat="server">    
                                
            <div id="LessonPlanExportOpt" style="width:1200px"> 

                <br />                                
                <table>
                                                                                
                <tr>
                <td class="HeaderStyleLE" style="font-family:Calibri" colspan="5">Export</td>
                </tr>
                                            
                <tr>
                <td class="auto-style11" style="font-family:Calibri" colspan="3">
                    <asp:RadioButtonList ID="rdoinprogress" runat="server" RepeatDirection="Horizontal"  AutoPostBack="True" OnSelectedIndexChanged="Lessontype_SelectedIndexChanged">
                        <asp:ListItem Selected="True">Approved</asp:ListItem>
                        <asp:ListItem>In-progress</asp:ListItem>
                        <asp:ListItem>Pending Approval</asp:ListItem>
                    </asp:RadioButtonList>
                    </td>
                <td style="padding:10px" class="auto-style12">
                </td>

                <td class="auto-style13">
                </td>
                </tr>
                                            
                <tr>
                <td class="auto-style14" style="font-family:Calibri">Lesson Plan&nbsp;&nbsp;&nbsp; </td>
                <td style="padding:10px" class="auto-style5"><asp:DropDownList ID="ddlLessonplan" runat="server" CssClass="drpClassShort" Height="26px" Width="250px"  AutoPostBack="true" OnSelectedIndexChanged="FillVersion" Font-Names="Arial">
                </asp:DropDownList></td>
                                        
                <td class="auto-style10" style="font-family:Calibri">Select Version</td>
                <td style="padding:10px" class="auto-style3">
                <asp:DropDownCheckBoxes ID="ddlLessonplan0" runat="server" TabIndex="1" AddJQueryReference="True" UseButtons="false" UseSelectAllNode="false" style="top: 0px; left: -53px; width: 303px" Font-Names="Arial" CssClass="drpClassShort" Font-Size="Small">
                <Style SelectBoxWidth="250px" DropDownBoxBoxWidth="250px" DropDownBoxBoxHeight="350px"   DropDownBoxCssClass="ddchkLesson" selectboxcssclass="drpClassShort"/>
                <Style2 DropDownBoxCssClass="ddchkLesson" SelectBoxWidth="250px" DropDownBoxBoxWidth="250px" DropDownBoxBoxHeight="350px" selectboxcssclass="drpClassShort"></Style2><Texts SelectBoxCaption="------- Select Version--------"/>
                </asp:DropDownCheckBoxes>                                  
                

                    <asp:DropDownList ID="DummyDropDown" runat="server" CssClass="drpClassShortdm" Height="30px" Width="253px" Visible="False" Font-Names="Arial" Font-Size="Small" Enabled="False">
                        <asp:ListItem>------- Select Version--------</asp:ListItem>                       
                    </asp:DropDownList>
                </td>
                    <td class="auto-style10" style="font-family:Calibri">Format&nbsp;&nbsp;&nbsp;</td>
                    <td style="padding:10px" class="auto-style3">
                        <asp:DropDownList ID="ddlFormat" runat="server" AutoPostBack="true" CssClass="drpClassFormat" style="width:250px;">
                            <asp:ListItem Value="0">......Select Format......</asp:ListItem>
                            <asp:ListItem Selected="True">Lesson Plan</asp:ListItem>
                            <asp:ListItem>Support Strategy</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                <td class="auto-style8">
                <asp:Button ID="btn1" runat="server" ToolTip="Export To Word" CssClass="ExportWord" Text="" OnClick="btnExportWord_Click" OnClientClick="return checkPostbackExport();" Height="35px" Width="37px"/>
                <asp:Button ID="btn2" runat="server" visible="false" ToolTip="Export To Word" CssClass="ExportWord" Text="" OnClick="btnExportWord_Click1" OnClientClick="checkPostbackExport();" Height="35px" Width="37px"/>
                </td>
                </tr>
                                            
                </table>
            </div>
    </form>
</body>
</html>
