<%@ Page Title="" Language="C#" AutoEventWireup="true"
    CodeFile="~/StudentBinder/ReviewAssessmnt.aspx.cs" Inherits="StudentBinder_ReviewAssessmnt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<head id="head1" runat="server">
    
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/buttons.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
   <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="../Administration/JS/jquery-ui.js"></script>
    <script type="text/javascript" src="../Administration/JS/jquery.min.js"></script>




     <script type="text/javascript">

         $(function () {
             // Patch fractional .x, .y form parameters for IE10.
             if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && Sys.Browser.version === 10) {
                 Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
                     if (element.disabled) {
                         return;
                     }
                     this._activeElement = element;
                     this._postBackSettings = this._getPostBackSettings(element, element.name);
                     if (element.name) {
                         var tagName = element.tagName.toUpperCase();
                         if (tagName === 'INPUT') {
                             var type = element.type;
                             if (type === 'submit') {
                                 this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                             }
                             else if (type === 'image') {
                                 this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
                             }
                         }
                         else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
                             this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                         }
                     }
                 };
             }
         });

</script>





    <script type="text/javascript">


       
        function ShowDialog(modal) {
            $("#overlay").show();
            $("#dialog").fadeIn(300);

            if (modal) {
                $("#overlay").unbind("click");
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }


        function HideDialog() {
            $("#overlay").hide();
            $("#dialog").fadeOut(300);

        }
        function ShowDialog2(modal) {
            $("#overlay").show();
            $("#dialog2").fadeIn(300);

            if (modal) {
                $("#overlay").unbind("click");
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }


        function HideDialog2() {
            $("#overlay").hide();
            $("#dialog2").fadeOut(300);

        }
    </script>
     <style type="text/css">
        .divGrid {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
        }

        .divGrid1 {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: auto;
            height: auto;
            display: none;
        }

        .divBackgrnd {
            padding: 26px 16px 16px 16px;
            width: 90%;
            height: 400px;
            overflow-y: scroll;
            overflow-x: hidden;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 24px 24px 24px 24px;
            background: rgba(87,197,239,0.42);
            -webkit-box-shadow: #68A1B3 8px 8px 8px;
            -moz-box-shadow: #68A1B3 8px 8px 8px;
            box-shadow: #68A1B3 8px 8px 8px;
        }

        .pnlCSS {
            padding-top: 2px;
        }
        /* FOR LOADING IMAGE AT PAGE LOAD */
        .loading {
            display: block;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            z-index: 1000;
            /*background-image: url("images/overlay.png");*/
        }

        #fullContents {
            display: none;
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
                height: 30px;
                width: 30px;
            }

        /*LOADING IMAGE CLOSE */
    </style>
    <style type="text/css">
        
        body {
            font-family: Arial;
            font-size: 12px;
        }

        .divGrid {
            border-radius: 14px;
            -moz-border-radius: 14px;
            -webkit-border-radius: 14px;
            border: 5px solid #62BDF6;
            width: 2000px;
            height: auto;
        }

        .divBackgrnd {
            padding: 26px 16px 16px 16px;
            width: 90%;
            height: 250px;
            overflow-y: hidden;
            overflow-x: hidden;
            -webkit-border-radius: 24px 24px 24px 24px;
            -moz-border-radius: 24px 24px 24px 24px;
            border-radius: 24px 24px 24px 24px;
            background: rgba(87,197,239,0.2);
            -webkit-box-shadow: #68A1B3 8px 8px 8px;
            -moz-box-shadow: #68A1B3 8px 8px 8px;
            box-shadow: #68A1B3 8px 8px 8px;
        }

        .web_dialog_overlay {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101;
            display: none;
            text-align: center;
        }

        .web_dialog {
            display: none;
            position: fixed;
            width: 600px;
            height: 400px;
            overflow: auto;
            top: 50%;
            left: 50%;
            margin-left: -300px;
            margin-top: -200px;
            font-size: 100%;
            font: 13px/20px "Helvetica Neue", Helvetica, Arial, sans-serif;
            color: White;
            z-index: 102;
            -moz-border-radius: 6px;
            background: #FFFFFF;
            -webkit-border-radius: 6px;
            border: 1px solid #536376;
        }

        .web_dialog2 {
            display: none;
            position: fixed;
            width: 1200px;
            height: 550px;
            overflow: auto;
            top: 50%;
            left: 50%;
            margin-left: -600px;
            margin-top: -275px;
            font-size: 100%;
            font: 13px/20px "Helvetica Neue", Helvetica, Arial, sans-serif;
            color: White;
            z-index: 102;
            -moz-border-radius: 6px;
            background: #FFFFFF;
            -webkit-border-radius: 6px;
            border: 1px solid #536376;
        }
    </style>
      <script type="text/javascript">

          $(window).load(function () {
              $('.loading').fadeOut('slow', function () {
                  $('#fullContents').fadeIn('fast');
              });
          });


    </script>
</head>
<body>
    <form id="form1" runat="server">
         <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
         <div id="fullContents">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="height: 115px; width: 100%">
                    <tr>
                        <td id="lbl_Msg" runat="server" style="text-align: center" colspan="2"></td>

                    </tr>

                    <tr>
                        <td colspan="2">
                            <table style="width: 100%">
                                <tr>
                                    <td class="tdText" style="text-align: right; font-family: Arial; font-size: 12px; width: 10%">Year :</td>
                                    <td>
                                        <asp:DropDownList ID="ddl_Year" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Year_SelectedIndexChanged1"
                                            CssClass="drpClass">
                                        </asp:DropDownList></td>
                                    <td class="tdText" style="text-align: right; width: 10%; font-family: Arial; font-size: 12px;">Status :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                            CssClass="drpClass">
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:Button ID="btn_GenerateIEP" Visible="false" runat="server" CssClass="NFButtonWithNoImage" Text="Generate IEP Goals" 
                                            OnClick="btn_GenerateIEP_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_manualIEP" Visible="false" runat="server" CssClass="NFButtonWithNoImage" Text="Manual IEP Genarate"
                                            OnClick="btn_manualIEP_Click" />
                                         <asp:ImageButton ID="btnRefresh" runat="server" Text="Refresh" ImageUrl="~/Administration/images/RefreshStudentBinder.png" OnClick="btnRefresh_Click" />
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 5px">
                            <asp:Panel Width="100%" Height="600px" HorizontalAlign="Center" ID="divPanel" runat="server"
                                ScrollBars="Both" Style="overflow: auto">
                                <div id="div_Grid" style="width: 98%;" runat="server">
                                    <asp:GridView ID="grd_ReviewAssess" Width="100%" runat="server" GridLines="None" 
                                        AutoGenerateColumns="False" OnRowCreated="grd_ReviewAssess_RowCreated" OnSelectedIndexChanged="grd_ReviewAssess_SelectedIndexChanged"
                                        OnPageIndexChanging="grdTrades_PageIndexChanging" OnRowDataBound="grd_ReviewAssess_RowDataBound" OnRowCommand="grd_ReviewAssess_RowCommand" AllowSorting="True" OnSorting="grd_ReviewAssess_Sorting">
                                        <Columns>
                                            <asp:BoundField DataField="AsmntName" HeaderText="Assessment Name" ItemStyle-Width="150px" SortExpression="AsmntName">
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AsmntTemplateName" HeaderText="Template Name" ItemStyle-Width="150px" SortExpression="AsmntTemplateName">
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AsmntTyp" HeaderText="Type" ItemStyle-Width="100px" SortExpression="AsmntTyp">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Inc.IEP" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_IncScr" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AsmntStatusId" HeaderText="Status" ItemStyle-Width="80px">
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Username" HeaderText="Modified By" ItemStyle-Width="150px">
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ModifiedOn" HeaderText="Modified On" ItemStyle-Width="150px" SortExpression="ModifiedOn">
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hfAssmntID" runat="server" Value='<%#Eval("AsmntId") %>' />
                                                    <asp:ImageButton ID="imgbtnUpdate" ImageUrl="~/Administration/images/user_edit.png" class="btn btn-blue" runat="server"
                                                        CommandName="Update" CommandArgument='<%#Eval("AsmntId") %>' Height="18px" Width="18px" />
                                                    <!--<asp:CheckBox ID="chk_Select" runat="server" onclick="errorCheckChanged(this)" />-->
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Skills" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnView" runat="server" CommandName="View" Visible="false"
                                                        CommandArgument='<%#Eval("AsmntId") %>' Height="18px" Width="18px" ImageUrl="~/Administration/Images/view_02.png" class="btn btn-purple" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnPreview" ImageUrl="~/Administration/images/Skills.png" runat="server"
                                                        CommandName="Preview" CommandArgument='<%#Eval("AsmntId") %>' Height="28px" Width="30px" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="HeaderStyle" />

                                        <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />
                                        <RowStyle CssClass="RowStyle" />
                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                        <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle CssClass="PagerStyle" HorizontalAlign="Center" />
                                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <div id="overlay" class="web_dialog_overlay">
                            </div>
                            <div id="dialog" class="web_dialog">
                                <table style="width: 100%; border: 0px; height: 100%">
                                    <tr style="height: 10%">
                                        <td colspan="3" align="right">
                                            <a href="javascript:HideDialog()" id="btnClose">
                                                <img src="images/DeleteGray.png" width="20px" height="20px" /></a>
                                        </td>
                                    </tr>
                                    <tr style="height: 10%">
                                        <td colspan="3" align="center">
                                            <b>Skills for this Assessment</b>
                                        </td>
                                    </tr>
                                    <tr style="height: 70%; vertical-align: top;">
                                        <td></td>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:DataList ID="dlSkillView" runat="server" CellSpacing="2" RepeatColumns="1" OnItemDataBound="dlSkillView_ItemDataBound"
                                                            OnItemCommand="dlSkillView_ItemCommand">
                                                            <ItemTemplate>
                                                                <table width="250px" style="border: 1px solid black;">
                                                                    <tr>
                                                                        <td style="width: 220px;">
                                                                            <asp:HiddenField ID="hfAsmntID" runat="server" />
                                                                            <asp:Label ID="lblSkill" runat="server" Text='<%# Eval("GoalName") %>' Font-Names="Comic Sans MS"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 30px;">
                                                                            <asp:ImageButton ID="imgbtnSkill" Width="25px" Height="25px" runat="server" Enabled="true"
                                                                                Visible="true" ImageUrl="~/Administration/images/anti_aliasing_filter_icon.jpg" CommandArgument='<%# Eval("GoalName") %>' />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr style="height: 10%">
                                        <td colspan="3" align="center">Click the filter button to see the Sections coming under the Skill
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="dialog2" class="web_dialog2">
                                <table style="width: 100%; border: 0px; height: 100%">
                                    <tr style="height: 10%">
                                        <td colspan="3" align="right">

                                            <a href="javascript:HideDialog2()" id="A2">
                                                <img src="images/DeleteGray.png" width="20px" height="20px" /></a>
                                        </td>
                                    </tr>
                                    <tr style="height: 10%">
                                        <td colspan="3" align="center">
                                            <b>Assessment Preview</b>
                                        </td>
                                    </tr>
                                    <tr style="height: 70%; vertical-align: top;">
                                        <td></td>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="grdAsmntPreview" runat="server" GridLines="none" Font-Names="Consolas" Font-Size="Small">
                                                            <HeaderStyle CssClass="HeaderStyle" Font-Bold="True" ForeColor="White" />

                                                            <RowStyle CssClass="RowStyle" />
                                                            <AlternatingRowStyle CssClass="AltRowStyle" />
                                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">&nbsp;
                        </td>
                        <td style="text-align: center">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
             </div>
    </form>
</body>
