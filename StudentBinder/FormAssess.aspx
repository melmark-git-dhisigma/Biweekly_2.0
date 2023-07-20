<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormAssess.aspx.cs" Inherits="StudentBinder_NewFormAssess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <title></title>

    <script src="js_asmnt/jquery-1.8.3.js"></script>
    <script src="js_asmnt/jq1.js"></script>


    <script type="text/javascript" src="JS/tabber.js"></script>
    <link rel="stylesheet" href="../Administration/CSS/tabmenu.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="../Administration/CSS/MenuStyle.css" type="text/css" />
    <script type="text/javascript">

        /* Optional: Temporarily hide the "tabber" class so it does not "flash"
        on the page as plain HTML. After tabber runs, the class is changed
        to "tabberlive" and it will appear. */

    </script>
    <style type="text/css">
        .divBackgrnd {
            width: 150px;
            text-align: center;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 10px 10px 10px 10px;
            background: white;
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

        <table width="100%">
            <tr>
                <td colspan="3" id="tdMsg" runat="server" style="text-align: center"></td>
            </tr>
            
            <tr>
                <td class="tdText">

                    <label for="email" style="visibility:hidden;">&nbsp;Accademic Year : </label>
                    <asp:Label ID="lblYear" runat="server" Font-Bold="True" ForeColor="Black" Visible="false"></asp:Label>
                </td>
                <td class="tdText">

                    <asp:Button ID="btnPrior" runat="server" CssClass="NFButtonWithNoImage" OnClick="btnPrior_Click" Text="Prior Assessments" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>

            <tr>
                <td style="vertical-align:top;" colspan="2">
             <br /></td>
                <td style="vertical-align:top;">
                    &nbsp;</td>
            </tr>

            <tr>
                <td style="vertical-align:top;" colspan="2">
                    &nbsp;</td>
                <td style="vertical-align:top;">
                    &nbsp;</td>
            </tr>

            <tr>
                <td style="vertical-align:top;" colspan="2">
                    <div class="tdTextCenter">Assessment by Type</div>
                    <div align="center" style="height:650px;">


                        <asp:DataList ID="dlFormMenu" runat="server" CellSpacing="15"
                            RepeatColumns="3" OnItemCommand="dlFormMenu_ItemCommand"
                            Style="text-align: right" OnItemDataBound="dlFormMenu_ItemDataBound">
                            <ItemTemplate>
                                <div class="divBackgrnd">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="imgbtnIcon" runat="server" Width="55px" Height="65px"
                                                    ImageUrl="~/Administration/images/Assessment.png"
                                                    CommandArgument='<%# Eval("AsmntId") %>' CommandName='<%# Eval("AsmntName") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblAssess" runat="server"
                                                    Text='<%# Eval("AsmntName") %>' Font-Size="Small"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                            </ItemTemplate>
                        </asp:DataList>


                    </div>



                </td>
                <td style="vertical-align:top;">
                    <div class="tdTextCenter">Assessment by Skill</div>
                    <div align="center" style="height:650px;">


                        <asp:DataList ID="dlSkillMenu" runat="server" CellSpacing="15"
                            RepeatColumns="3" OnItemCommand="dlSkillMenu_ItemCommand">
                            <ItemTemplate>

                                <div class="divBackgrnd">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="imgbtnIcon2" runat="server" Width="74px" Height="65px"
                                                    ImageUrl='<%# Eval("Image") %>' CommandArgument='<%# Eval("Goal") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblAssess2" runat="server" title='<%#Eval("Goal") %>' Text='<%# Eval("Goal") %>'
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
    
    <script type="text/javascript">
        //Used to Trim
        $(document).ready(function () {
            var assesmt = $('#dlSkillMenu');
            var assesmtPic = $(assesmt).find("span");
            for (var i = 0; i < assesmtPic.length; i++) {

                var inner = $(assesmtPic[i]).html().length;
                $(assesmtPic[i]).attr($(assesmtPic[i]).html());

                if(inner>18)
                {
                    $(assesmtPic[i]).html($(assesmtPic[i]).html().substring(0, 18) + "...");
                    
                }

            }

        });

    </script>
    





</body>
</html>

