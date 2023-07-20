<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoalAssess.aspx.cs" Inherits="StudentBinder_NewGoalAssess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <title></title>
    <script type="text/javascript" src="../Administration/JS/tabber.js"></script>
    <link rel="stylesheet" href="../Administration/CSS/tabmenu.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../Administration/CSS/MenuStyle.css" type="text/css"  />
    <script type="text/javascript">

        /* Optional: Temporarily hide the "tabber" class so it does not "flash"
        on the page as plain HTML. After tabber runs, the class is changed
        to "tabberlive" and it will appear. */

    </script>
    <style type="text/css">
        .divBackgrnd {
            width: 120px;
            text-align: center;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 24px 24px 24px 24px;
            background: White;
            background: url("../Administration/images/box-icon-bg.jpg") no-repeat scroll 0 0 transparent;
        }

            .divBackgrnd:hover {
                cursor: pointer;
                -webkit-box-shadow: #68A1B3 4px 4px 4px;
                -moz-box-shadow: #68A1B3 4px 4px 4px;
                box-shadow: Gray 4px 4px 4px;
            }

        .style4 {
            line-height: 32px;
            font-weight: bold;
            color: Gray;
            font-size: 10px;
            text-align: right;
            width: 371px;
            padding-right: 3px;
        }

        .style5 {
            text-align: left;
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <%--<div id="overlay" class="web_dialog_overlay">
        </div>--%>
         <%--<div id="dialog" class="web_dialog" align="center">
           <div style="width: 700px;" align="center">
                <asp:Image ID="imgwait" runat="server"
                    ImageUrl="~/Administration/images/waiticon.png" />
            </div>

        </div>--%>
        <table width="100%">
           
            <tr>
                <td colspan="2" id="tdMsg" runat="server" style="text-align: center"></td>
            </tr>

            <tr>
                <td class="tdText">
                    <label for="email">Accademic Year : </label>
                    <asp:Label ID="lblYear" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                    </td>
                
                <td>&nbsp;</td>
            </tr>

            <tr>
                <td colspan="2">
                    

                        
                        
                            
                            
                            <div align="center">
                                
                                
                                        <asp:DataList ID="dlSkillMenu" runat="server" CellSpacing="50"
                                            RepeatColumns="4" OnItemCommand="dlSkillMenu_ItemCommand"
                                            OnItemDataBound="dlSkillMenu_ItemDataBound">
                                            <ItemTemplate>
                                               
                                                <div class="divBackgrnd">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ImageButton ID="imgbtnIcon2" runat="server" Width="55px" Height="65px"
                                                                ImageUrl='<%# Eval("Image") %>' CommandArgument='<%# Eval("Goal") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblAssess2" runat="server" Text='<%# Eval("Goal") %>'
                                                                Font-Size="Small"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                            </div>
                       
                    
                </td>
            </tr>
        </table>

    </form>
</body>
</html>

