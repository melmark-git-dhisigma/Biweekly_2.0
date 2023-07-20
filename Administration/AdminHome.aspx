<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminHome.aspx.cs" Inherits="Admin_AdminHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="CSS/AdminTabletHome.css" rel="stylesheet" id="sized" />

    <script type="text/javascript">


        function adjustStyle(width) {
            width = parseInt(width);

            if (width >= 1200) {
                $("#sized").attr("href", "CSS/AdminTabletHome.css");
                return;
            }
            if (width < 1200) {
                $("#sized").attr("href", "CSS/AdminTablet.css");
                $('#slider').css("top", '73%');
            }
        }



        $(function () {
            adjustStyle($(this).width());
            $(window).resize(function () {
                adjustStyle($(this).width());
            });


        });

        function ExecuteBatch() {
            $('.loading').css("display", "block");

            PageMethods.GetBatchResult(OnSucceeded, OnFailed);


        }

        function OnSucceeded(result, userContext, methodName) {
            $('.loading').css("display", "none");
            $('#lnkExecute').html(result);
            
        }


        function OnFailed(error, userContext, methodName) {
            $('.loading').css("display", "none");
            $('#lnkExecute').html("Execute batch [Failed]");
            //$get('lnkExecute').innerHTML = "An error occured.";
        }
    </script>
    <style type="text/css">
        .loading {
            display: none;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            background-image: url("../Administration/images/overlay.png");
            /*background: repeat-x scroll center top transparent;*/
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
                height: 10px;
                width: 100px;
            }

        </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    .<%-- <div>
        <iframe src="LandingPage.aspx" runat="server" id="ifGraph" frameborder="0" scrolling="no" marginwidth="0" width="100%" height="380px"></iframe>
    </div>--%><div style="padding-right: 5px; width: 100%">
        <table width="100%">

            <tr>
                <td style="padding: 1%" runat="server" id="tdMenu" visible="false">

                    <div>
                        <asp:Label ID="lblAdministration" runat="server" class="mainheadingpanel"></asp:Label>
                    </div>
                    <br />
                    <br />
                    <div style="border-bottom: 1px groove gray; width: 100%;"></div>
                    <br />
                    <asp:DataList ID="dlAdmin" runat="server" RepeatColumns="4" ItemStyle-HorizontalAlign="Left"
                        RepeatDirection="Horizontal" OnItemDataBound="dlMenu_ItemDataBound">

                        <ItemTemplate>
                            <asp:HiddenField ID="hidValue" Value='<%# Eval("Id") %>' runat="server" />
                            <div class="rounded-corners1">

                                <div class="prod-img-cont" style="text-align: center;">
                                    <asp:Image ID="imageProduct" Width="120" Height="120" ImageUrl='<%# "~/Administration/Images/" + Eval("ImagePath") %>' runat="server" />
                                </div>
                                <br />

                                <asp:Label ID="lblHeading" runat="server" Text='<%# Eval("Heading") %>' ForeColor="#006c0e" Font-Bold="True" Font-Names="Oswald"></asp:Label></strong> 
                   <br />
                                <asp:DataList ID="dlSub" runat="server" RepeatColumns="1" CellPadding="0" ItemStyle-HorizontalAlign="Left" Width="100%">
                                    <ItemTemplate>
                                        » &nbsp; <a href='<%# Eval("Url") %>' class="SubLinks"><%# Eval("SubHead")%></a>

                                    </ItemTemplate>
                                </asp:DataList>





                            </div>
                        </ItemTemplate>

                    </asp:DataList>
                </td>
                <td class="auto-style1"></td>
                <td runat="server" id="tdMenu1" visible="false">

                    <div>
                        <asp:Label ID="LblAppConfig" runat="server" class="mainheadingpanel"></asp:Label>
                    </div>
                    <br />
                    <br />
                    <div style="border-bottom: 1px groove gray; width: 95%;"></div>
                    <br />
                    <asp:DataList ID="dlAppConfig" runat="server" RepeatColumns="4" ItemStyle-HorizontalAlign="Left"
                        RepeatDirection="Horizontal" OnItemDataBound="dlAppConfig_ItemDataBound">

                        <ItemTemplate>
                            <asp:HiddenField ID="hidValue" Value='<%# Eval("Id") %>' runat="server" />
                            <div class="rounded-corners1">

                                <div class="prod-img-cont" style="text-align: center;">
                                    <asp:Image ID="imageProduct" Width="120" Height="120" ImageUrl='<%# "~/Administration/Images/" + Eval("ImagePath") %>' runat="server" />
                                </div>
                                <br />

                                <asp:Label ID="lblHeading" runat="server" Text='<%# Eval("Heading") %>' ForeColor="#006c0e" Font-Bold="True" Font-Names="Oswald"></asp:Label></strong> 
                   <br />
                                <asp:DataList ID="dlSub" runat="server" RepeatColumns="1" CellPadding="0" ItemStyle-HorizontalAlign="Left" Width="100%">
                                    <ItemTemplate>
                                        » &nbsp; <a href='<%# Eval("Url") %>' class="SubLinks"><%# Eval("SubHead")%></a>

                                    </ItemTemplate>
                                </asp:DataList>





                            </div>
                        </ItemTemplate>

                    </asp:DataList>
                </td>
            </tr>
        </table>
        <div class="loading">
            <div class="innerLoading">
                <img src="../Administration/images/load.gif" alt="loading" />
                Please Wait...
            </div>
        </div>
    </div>


</asp:Content>

