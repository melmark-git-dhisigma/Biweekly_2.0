<%@ Page Language="C#" AutoEventWireup="true" CodeFile="profilePreview.aspx.cs" Inherits="profilePreview" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Melmark Pennsylvania</title>

    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />
    <link href="styles/buttons.css" rel="stylesheet" type="text/css" />
    <link href="styles/LandDesign.css" rel="stylesheet" />
    <link href="styles/style2.css" rel="stylesheet" type="text/css">
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <script src="scripts/jquery-1.8.0.js"></script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .buttonNew1 {
            border: 1px groove black;
            padding: 5px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function validate() {
            if (document.getElementById("<%=txtLessonName.ClientID%>").value == "") {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Enter your Lesson Plan !!!!</dv> ";
                document.getElementById("<%=txtLessonName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlDomain.ClientID%>").selectedIndex == 0) {
                document.getElementById("<%=tdMsg.ClientID%>").innerHTML = "<div class='warning_box'>Please Select any Domain</dv> ";
                document.getElementById("<%=ddlDomain.ClientID%>").focus();
                return false;
            }
            return true;

        }

    </script>
</head>

<body>
    <!-- top panel -->
    <form runat="server" id="form1">







        <table style="width: 80%;">

            <tr>
                <td id="tdMsg" runat="server" colspan="2"></td>
            </tr>

            <tr>
                <td>

                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%;"></td>
                            <td class="tdText">Lesson Name
                            </td>
                        </tr>
                    </table>

                </td>
                <td style="width: 60%; text-align: left;">
                    <span class="spanStyle">*</span>
                    <asp:TextBox ID="txtLessonName" runat="server" MaxLength="50"></asp:TextBox>

                </td>
            </tr>

            <tr>
                <td>

                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%;"></td>
                            <td class="tdText">Description
                            </td>
                        </tr>

                    </table>


                </td>
                <td style="text-align: left;">
                    <span style="color: white;">*</span>
                    <asp:TextBox ID="txtDescription" runat="server" Columns="20" Rows="4" TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>

            <tr>
                <td>

                    <table style="width: 100%;">

                        <tr>
                            <td style="width: 50%;"></td>
                            <td class="tdText">Keywords
                            </td>
                        </tr>

                    </table>



                </td>
                <td style="text-align: left;">
                    <span style="color: white;">*</span>
                    <asp:TextBox ID="txtKeyword" runat="server" Rows="4" TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>

            <tr>
                <td>

                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%;"></td>
                            <td class="tdText">Domain
                            </td>
                        </tr>
                    </table>


                </td>
                <td align="left" style="text-align: left;">
                    <span class="spanStyle">*</span>
                    <asp:DropDownList ID="ddlDomain" runat="server" CssClass="drpClass">
                    </asp:DropDownList>


                </td>
            </tr>



            <tr>
                <td>&nbsp;</td>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 25%;">

                                <asp:Button ID="btnSave" runat="server" Text="Next" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick=" return validate();" ValidationGroup="a" />

                            </td>
                            <td style="width: 25%;">

                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass=" NFButton" OnClick="btnCancel_Click" />

                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>




    </form>
</body>

</html>
