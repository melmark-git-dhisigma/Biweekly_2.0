<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true" CodeFile="RolePermissions.aspx.cs" Inherits="Admin_UserRights1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <%--<script type="text/javascript">
        $(document).ready(function () {
            // Hide all Modal Boxes
            $('div.modal-box').hide();
            // Display appropriate box on click - adjust this as required for your website
            $('span.modal-link').click(function () {
                var modalBox = $(this).attr('rel');
                $('div' + modalBox).fadeIn('slow');
            });
            // Multiple ways to close a Modal Box
            $('span.modal-close').click(function () {
                $(this).parents('div.modal-box').fadeOut('slow');
            });

        });
    </script>--%>

   <%-- <script type="text/javascript">

        //function toggle_visibility(id) {
        //    var e = document.getElementById(id);
        //    if (e.style.display == 'block')
        //        e.style.display = 'none';
        //    else
        //        e.style.display = 'block';
        //}

    </script>--%>


    <script type="text/javascript">




        function showsubmenu(id) {
            var subcat = document.getElementById(id).style;
            if (subcat.display == "none") {
                subcat.display = "block";
            }
            else {
                subcat.display = "none";
            }

        }

        function checkBoxSelector(elm) {
            var elmClass = $(elm).parent().attr('class');
            var elmId = $(elm).attr('Id');

            switch (elmClass) {
                case "mainRead":
                    if ($(elm).prop("checked")) {

                        var chkList = $(elm).parents('.mainTable').parent().find('.subPanel').find('.subRead');
                        for (var i = 0; i < chkList.length; i++) {
                            $(chkList[i]).find('input').prop('checked', true);
                        }

                    }
                    else {

                        var chkList = $(elm).parents('.mainTable').parent().find('.subPanel').find('.subRead');
                        for (var i = 0; i < chkList.length; i++) {
                            $(chkList[i]).find('input').prop('checked', false);
                        }
                    }


                    break;

                case "mainWrite":
                    if ($(elm).prop("checked")) {

                        var chkList = $(elm).parents('.mainTable').parent().find('.subPanel').find('.subWrite');
                        for (var i = 0; i < chkList.length; i++) {
                            $(chkList[i]).find('input').prop('checked', true);
                        }

                    }
                    else {

                        var chkList = $(elm).parents('.mainTable').parent().find('.subPanel').find('.subWrite');
                        for (var i = 0; i < chkList.length; i++) {
                            $(chkList[i]).find('input').prop('checked', false);
                        }
                    }


                    break;

                case "mainAppr":
                    if ($(elm).prop("checked")) {

                        var chkList = $(elm).parents('.mainTable').parent().find('.subPanel').find('.subAppr');
                        for (var i = 0; i < chkList.length; i++) {
                            $(chkList[i]).find('input').prop('checked', true);
                        }

                    }
                    else {

                        var chkList = $(elm).parents('.mainTable').parent().find('.subPanel').find('.subAppr');
                        for (var i = 0; i < chkList.length; i++) {
                            $(chkList[i]).find('input').prop('checked', false);
                        }
                    }


                    break;

                    //-----------------------


                case "subRead":

                    var chkList = $(elm).parents('.subPanel').find('.subRead');
                    var mainChk = $(elm).parents('.subPanel').parent().find('.mainRead').find('input');

                    $(mainChk).prop("checked", true);

                    for (var i = 0; i < chkList.length; i++) {

                        if (!$(chkList[i]).find('input').prop('checked')) {
                            $(mainChk).prop("checked", false);
                            break;
                        }

                    }



                    break;

                case "subWrite":

                    var chkList = $(elm).parents('.subPanel').find('.subWrite');
                    var mainChk = $(elm).parents('.subPanel').parent().find('.mainWrite').find('input');

                    $(mainChk).prop("checked", true);

                    for (var i = 0; i < chkList.length; i++) {

                        if (!$(chkList[i]).find('input').prop('checked')) {
                            $(mainChk).prop("checked", false);
                            break;
                        }

                    }



                    break;

                case "subAppr":

                    var chkList = $(elm).parents('.subPanel').find('.subAppr');
                    var mainChk = $(elm).parents('.subPanel').parent().find('.mainAppr').find('input');

                    $(mainChk).prop("checked", true);

                    for (var i = 0; i < chkList.length; i++) {

                        if (!$(chkList[i]).find('input').prop('checked')) {
                            $(mainChk).prop("checked", false);
                            break;
                        }

                    }



                    break;
            }

        }


        
    </script>









</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">


    <table width="99%" cellpadding="0" cellspacing="2">

        <tr>
            <td id="tdMsg" runat="server" colspan="5" style="height:25px">&nbsp;</td>
        </tr>



        <tr>
            <td style="width: 12%;">Select Group</td>
            <td style="width: 8%;">
                <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="width: 12%; text-align: right; padding-right: 15px;">Select Role</td>

            <td style="text-align: right; width: 10%; padding-right: 5px;">
                <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="True" CssClass="drpClass" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                    <asp:ListItem Value="0">---------------Select--------------</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="btnSave_Click" Text="Save" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="5">
                <table width="100%" cellpadding="0" cellspacing="0">
                      
                    <tr>

                        <td>
                            <asp:UpdatePanel runat="server" ID="updatetab">
                                <ContentTemplate>

                                    <div class="container">
                                         

                                        <table width="100%" border="0"  cellpadding="0" cellspacing="0" >
                                         
                                            <tr>
                                                <td>
                                                           
                                                               <tr>
                                                                     <td style="width: 40%; min-width: 250px;" class="top">   </td>
                                                                     <td style="width: 15%; min-width: 100px;" class="top">Read</td>
                                                                     <td style="width: 15%; min-width: 100px;" class="top">Write</td>
                                                                     <td style="width: 15%; min-width: 100px;" class="top">Approve/Reject</td>
                                                                    
                                                                </tr>
                                                    <asp:DataList ID="dl_screenPermission" runat="server"
                                                        OnItemDataBound="dl_screenPermission_ItemDataBound" Width="100%" >
                                                        <ItemTemplate>
                                                            <table cellspacing="0" cellpadding="0" width="100%" class="mainTable">
                                                                  

                                                                <tr>
                                                                    <td style="width: 40%; min-width: 250px;" class="top">
                                                                        <asp:Label ID="id" runat="server" Text='<%# Eval("ObjectId") %>' Visible="false"></asp:Label>
                                                                         <asp:HiddenField id="hdnReadAll" runat="server" Value='<%# Eval("ObjectId") %>'></asp:HiddenField>
                                                                        <p>
                                                                            <asp:HyperLink ID="MainCat" Text='<%# Eval("ObjectName") %>' runat="server" NavigateUrl="#" Width="220"></asp:HyperLink>
                                                                        </p>
                                                                    </td>
                                                                    <td style="width: 15%; min-width: 100px;" class="top">                                                                       
                                                                        <asp:CheckBox ID="chkReadAll" runat="server" Text="All" Width="100%" CssClass="mainRead" onclick="checkBoxSelector(this);" AutoPostBack="false" OnCheckedChanged="chkReadAll_CheckedChanged"  Font-Bold="False" EnableTheming="False" ClientIDMode="Static" Enabled="True" /></td>
                                                                    <td style="width: 15%; min-width: 100px;" class="top">
                                                                        <asp:CheckBox ID="chkWriteAll" runat="server" Text="All" ForeColor="White" CssClass="mainWrite" onclick="checkBoxSelector(this);" AutoPostBack="false" OnCheckedChanged="chkWriteAll_CheckedChanged" Font-Bold="True" /></td>
                                                                    <td style="width: 15%; min-width: 100px;" class="top">
                                                                        <asp:CheckBox ID="chkApproveAll" runat="server" Text="All" ForeColor="White" CssClass="mainAppr" onclick="checkBoxSelector(this);" AutoPostBack="false" OnCheckedChanged="chkApproveAll_CheckedChanged" Font-Bold="True" /></td>
                                                                    
                                                                </tr>
                                                            </table>


                                                            <asp:Panel ID="panelsubcat" runat="server" Style="display: none;" Width="100%" CssClass="subPanel">
                                                                <asp:DataList ID="subcat" runat="server" Width="100%">
                                                                    <ItemTemplate>
                                                                        <table width="100%">
                                                                            <tr>

                                                                                <td style="width: 40%; min-width: 250px;" class="second">
                                                                                    <p class="l">
                                                                                        <asp:Label ID="subcat" Text='<%# Eval("ObjectName") %>' runat="server"></asp:Label>
                                                                                    </p>
                                                                                    <asp:HiddenField ID="hidObjectId" runat="server" Value='<%# Eval("ObjectId") %>' />
                                                                                </td>
                                                                                <td style="width: 15%; min-width: 100px" class="second">
                                                                                    <div>
                                                                                        <asp:CheckBox ID="chkRead" runat="server" Text="Read" CssClass="subRead" onclick="checkBoxSelector(this);" AutoPostBack="false" OnCheckedChanged="chkRead_CheckedChanged" />
                                                                                    </div>
                                                                                </td>

                                                                                <td style="width: 15%; min-width: 100px;" class="second">
                                                                                    <asp:CheckBox ID="chkWrite" runat="server" Text="Write" CssClass="subWrite" onclick="checkBoxSelector(this);" AutoPostBack="false" OnCheckedChanged="chkWrite_CheckedChanged" /></td>
                                                                                <td style="width: 15%; min-width: 100px;" class="second">
                                                                                    <asp:CheckBox ID="chkApprove" runat="server" Text="Approve/Reject" CssClass="subAppr" onclick="checkBoxSelector(this);" AutoPostBack="false" OnCheckedChanged="chkApprove_CheckedChanged" /></td>
                                                                                
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </asp:Panel>

                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>




                        </td>
                    </tr>
                </table>
            </td>
        </tr>



    </table>


</asp:Content>

