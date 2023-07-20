<%@ Page Title="" Language="C#" AutoEventWireup="true"
    CodeFile="AssessmentByType.aspx.cs" Inherits="Administration_FormAssess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script src="../Administration/JS/jquery1.4.2.js"></script>
    <link href="../Administration/CSS/DisplayStyles.css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />


     <script lang="JavaScript" type="text/javascript">

         function errorHandler(message, url, line) {
             // message == text-based error description
             // url     == url which exhibited the script error
             // line    == the line number being executed when the error occurred

             // handle the error here

             // stop the event from bubbling up to the default window.onerror handler
             // (see the "For More Info" section for an article on event bubbling)
             return true;
         }

         // install the global error-handler
         window.onerror = errorHandler;
</script>



    <script type="text/javascript">

        function showNestedGridView(obj) {

            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "../Administration/images/minusicon.png";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "../Administration/images/Addicon.png";
            }
        }
        function chkGroup(chkBox) {
            var grd = chkBox.parentNode.parentNode.parentNode.parentNode;

            var Elements = grd.getElementsByTagName('input');


            for (var i = 0; i < Elements.length; i++) {
                if (Elements[i].type == 'checkbox' && Elements[i].id != chkBox.id && chkBox.checked && Elements[i].id.endsWith('chkChoice') == true)
                    Elements[i].checked = false;
            }
        }

        var img = null;
        var index = 0;
        var lblCollctn = new Array();
        var test = null;
        function CheckUnCheckAll(txt) {
            if (txt.value >= 0) {
                //Onload();
                var row = txt.parentNode.nextSibling.getElementsByTagName('input');
                //test = txt.next('input');
                var boxes = txt.parentNode.getElementsByTagName('input');
                img = txt.parentNode.getElementsByTagName('img');
                for (var i = 0; i < boxes.length; i++) {
                    if (boxes[i].type == "hidden")
                        var box = boxes[i];
                }
                var chklist = row[0];
                chklist.checked = false;
                if (txt.value.length > 0) {
                    if (parseInt(txt.value) > parseInt(box.value)) {
                        //alert("Invalid Score");
                        var isAvail = false;
                        for (var check = 0; check < lblCollctn.length; check++) {
                            if (lblCollctn[check] == img[0]) {
                                isAvail = true;
                            }
                        }
                        if (isAvail == false)
                            lblCollctn[lblCollctn.length] = img[0];
                        img[0].src = "../Administration/Images/symbol-error.png";
                        img[0].style.width = "15px";
                        img[0].style.height = "15px";

                        var hferror = document.getElementById('<%=hfCheckError.ClientID %>');
                        hferror.value = lblCollctn.length;
                        index++;
                    }
                    else {
                        img[0].style.width = "15px";
                        img[0].style.height = "15px";
                        img[0].src = "../Administration/Images/tick.png";
                        for (var check = 0; check < lblCollctn.length; check++) {
                            if (lblCollctn[check] == img[0]) {
                                lblCollctn.splice(check, 1);
                            }
                        }
                        var hferror = document.getElementById('<%=hfCheckError.ClientID %>');
                        hferror.value = lblCollctn.length;
                    }
                }
                else {
                    img[0].style.width = "0px";
                    img[0].style.height = "0px";
                    img[0].src = "";
                    for (var check = 0; check < lblCollctn.length; check++) {
                        if (lblCollctn[check] == img[0]) {
                            lblCollctn.splice(check, 1);
                        }
                    }
                    var hferror = document.getElementById('<%=hfCheckError.ClientID %>');
                    hferror.value = lblCollctn.length;
                }
                if (txt.value.length == 0) {
                    chklist.checked = true;
                }
            }
            else {
                //alert("Numbers only allowed");
                img = txt.parentNode.getElementsByTagName('img');
                img[0].style.width = "0px";
                img[0].style.height = "0px";
                img[0].src = "";
                var row = txt.parentNode.nextSibling.getElementsByTagName('input');
                var chklist = row[0];
                chklist.checked = true;

                for (var check = 0; check < lblCollctn.length; check++) {
                    if (lblCollctn[check] == img[0]) {
                        lblCollctn.splice(check, 1);
                    }
                }
                var hferror = document.getElementById('<%=hfCheckError.ClientID %>');
                hferror.value = lblCollctn.length;

                txt.value = "";
                txt.focus();
            }

        }
        function OnSuccess(result) {
            if (result) {
                //alert("Score > Box");
                var isAvail = false;
                for (var check = 0; check < lblCollctn.length; check++) {
                    if (lblCollctn[check] == img[0]) {
                        isAvail = true;
                    }
                }
                if (isAvail == false)
                    lblCollctn[lblCollctn.length] = img[0];
                img[0].style.width = "15px";
                img[0].style.height = "15px";
                img[0].src = "../Administration/Images/symbol-error.png";

                var hferror = document.getElementById('<%=hfCheckError.ClientID %>');
                hferror.value = lblCollctn.length;
                index++;

            }
            else {
                //alert("Score < Box");
                img[0].style.width = "15px";
                img[0].style.height = "15px";
                img[0].src = "../Administration/Images/tick.png";
                for (var check = 0; check < lblCollctn.length; check++) {
                    if (lblCollctn[check] == img[0]) {
                        lblCollctn.splice(check, 1);
                    }
                }
                var hferror = document.getElementById('<%=hfCheckError.ClientID %>');
                hferror.value = lblCollctn.length;
            }
        }
        function OnFailure(error) {
            if (error) {
                alert("Error!!!!!!!");

            }
        }

        function CheckError(txtSubmit, identity) {

            if (document.getElementById('<%=hfCheckError.ClientID %>').value == 0) {
                if (identity == 1)
                    document.getElementById('<%=btnSubmit.ClientID %>').click();
                else {
                    Confirm();
                    document.getElementById('<%=btnSave.ClientID %>').click();
                }
            }
            else {
                document.getElementById('<%=lbl_Msg.ClientID %>').innerHTML = "<div class='valid_box'>" + document.getElementById('<%=hfCheckError.ClientID %>').value + ' Invalid Score ' + ".</div>";
                //alert(document.getElementById('<%=hfCheckError.ClientID %>').value + ' Invalid Score ');
            }

        }


        function Confirm() {
            var r = confirm("Please make sure that the Lesson Plan mappings are available for the assessment before marking as completed. Do you want to Continue?");

            var hf = document.getElementById('<%=hfConfirm.ClientID %>');
            hf.value = r;

        }


        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 100);
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
            background: rgba(87,197,239,0.2);
            -webkit-box-shadow: #68A1B3 8px 8px 8px;
            -moz-box-shadow: #68A1B3 8px 8px 8px;
            box-shadow: #68A1B3 8px 8px 8px;
        }

        .pnlCSS {
        }
        /* FOR LOADING IMAGE AT PAGE LOAD */
        .loading {
            display: block;
            position: absolute;
            width: 100%;
            height: 800px;
            top: 0px;
            left: 0px;
            z-index: 1000;
            /*background-image: url("images/overlay.png");*/
            /*background: repeat-x scroll center top transparent;*/
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
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"></asp:ToolkitScriptManager>
        <div id="fullContents">
            <asp:UpdatePanel ID="updatepanel1" runat="server">
                <ContentTemplate>
                    
                    <table width="100%">
                        <tr>
                            <td>
                                <table width="100%">


                                    <tr>
                                        <td class="tdText"></td>

                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">

                                            <table width="90%">
                                                <tr>
                                                    <td class="tdText">
                                                        <table style="width:99%;">
                                                            <tr>
                                                                <td class="tdText" style="width: 10%;">
                                                                    <label for="email">
                                                                        Year :
                                        <asp:Label ID="lblYear" runat="server" Text=""></asp:Label>
                                                                    </label>
                                                                </td>
                                                                <td></td>
                                                                <td class="tdText" style="text-align:right;">Assessment Name:</td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txt_AssmntName" runat="server" CssClass="textClass" ReadOnly="true"></asp:TextBox></td>
                                                                <td></td>
                                                                <td class="tdText" style="text-align:right;">Status: <asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                                                                
                                                            </tr>
                                                        </table>


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:Panel ID="notepnl" runat="server" Visible="false">

                                                        <td style="height: 70px;padding-left:3px; vertical-align: top;" class="tdText">Notes<br />
                                                            <div style="height: 75%; overflow: auto;">
                                                                <asp:Label ID="lblNote" runat="server" ></asp:Label>
                                                            </div>
                                                        </td>
                                                    </asp:Panel>
                                                </tr>
                                                <tr>
                                                    <td class="tdText">Enter Your Notes Here<br />
                                                        <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Rows="5" Columns="5"
                                                             Width="99%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="font-family: Arial; font-weight: bold; font-size: 16px; color: gray;">
                                            <br />
                                            <asp:Label ID="lbl_Asmnt" runat="server" Text="Label"></asp:Label></td>
                                    </tr>
                                </table>

                                <table width="100%">

                                    <tr>
                                        <td align="center" colspan="2">
                                            <!--<div class="divBackgrnd">-->
                                            <asp:Panel ID="Panel1" runat="server"  Width="100%">
                                                <br />
                                                <div id="Msg" runat="server" style="font-size: larger; font-weight: bold; color: Blue; text-align: center;"></div>
                                                <asp:DataList ID="dl_Sections" runat="server" OnItemCommand="dl_Sections_ItemCommand"
                                                    OnItemDataBound="dl_Sections_ItemDataBound" Width="90%" Font-Bold="True" Font-Size="Medium" style="border-collapse:inherit;">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfSkill" runat="server" Value='<%# Eval("Skill") %>' />
                                                        <asp:Panel ID="pnlClick" runat="server" CssClass="pnlCSS" HorizontalAlign="Center">
                                                            <div style="background-image: url('green_bg.gif'); height: 22px; vertical-align: middle; text-align: left;">
                                                                <div style="color: White; cursor: pointer; padding:0; text-align: left;border-bottom:2px solid #fff;">
                                                                    <table width="100%" style="background: #ededed url(../images/topbtmline.JPG) right top repeat-y;border-bottom: 1px dotted #116c90;">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:LinkButton ID="lb_Section" runat="server" CommandName='<%# Eval("name") %>'
                                                                                    Text='<%# Eval("name") %>' Enabled="false" Font-Names="Arial" Font-Size="12px" Font-Bold="true" ForeColor="#116C90"></asp:LinkButton>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Image ID="imgArrows" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlCollapsable" runat="server" Height="0" CssClass="pnlCSS">
                                                            <asp:DataList ID="dl_SubSections" runat="server" OnItemCommand="dl_SubSections_ItemCommand"
                                                                Style="text-align: left" Visible="True" Width="100%" OnSelectedIndexChanged="dl_SubSections_SelectedIndexChanged">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnlClickB" runat="server" CssClass="pnlCSS">
                                                                        <div style="background-image: url('green_bg.gif'); height: 22px; vertical-align: left;">
                                                                            <div style="color: White; cursor: pointer; padding-right: 5px; text-align: left;">
                                                                                <table width="100%" style="border-bottom: 1px dotted #116c90;">
                                                                                    <tr>
                                                                                        <td style="background:url('../Administration/images/dotblk.JPG') no-repeat scroll 0 center transparent ">
                                                                                            <asp:LinkButton ID="lb_SubSection" runat="server" CommandName='<%# Eval("name") %>'
                                                                                                Text='<%# Eval("name") %>' Enabled="false" Font-Bold="true" Font-Size="Small" style="padding:0 0 5px 10px;"></asp:LinkButton>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Image ID="imgArrows" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="pnlCollapsableB" runat="server" Height="0" CssClass="pnlCSS">
                                                                        <asp:GridView ID="grd_Questions" runat="server" AutoGenerateColumns="False" HorizontalAlign="Justify"
                                                                            Style="margin-left: 0px;text-align: left;"
                                                                            Width="100%" OnRowDataBound="grd_Questions_RowDataBound" GridLines="None">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <a tabindex="-1" href="javascript:showNestedGridView('<%# Eval("ID") %>');">
                                                                                            <img id="image<%# Eval("ID") %>" width="0px"  border="0" src="images/Addicon.png" />
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="10px" />
                                                                                </asp:TemplateField>
                                                                              <%--  ItemStyle-Width="440px"--%>
                                                                                <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="150px">
                                                                                   <%-- <ItemStyle Width="440px" />--%>
                                                                                  
                                                                                </asp:BoundField>
                                                                                <%--ItemStyle-Width="250px"--%>
                                                                                <asp:TemplateField HeaderText="Point" >
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_Question" runat="server" onchange="javascript:CheckUnCheckAll(this);" ></asp:TextBox>
                                                                                        <asp:HiddenField ID="hfBoxSect" runat="server" Value='<% #Eval("Box")%>' />
                                                                                        <img width="0px" />
                                                                                    </ItemTemplate>
                                                                                    <%--<ItemStyle Width="250px" />--%>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox TabIndex="-1" ID="chkQuesAppl" runat="server" />
                                                                                        <asp:HiddenField ID="hfSectn" runat="server" />
                                                                                        <asp:HiddenField ID="hfSubSectn" runat="server" />
                                                                                        <asp:HiddenField ID="hfQtn" runat="server" Value='<% #Eval("Code")%>' />
                                                                                        <asp:HiddenField ID="hfSkill" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="Box" HeaderText="Box" />
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td colspan="100%">
                                                                                                <div id="<%# Eval("ID") %>" class="divGrid1">
                                                                                                    <asp:GridView ID="grd_SubQuestion" runat="server" ShowHeader="false" AutoGenerateColumns="False"
                                                                                                        DataKeyNames="ID" Width="590px" GridLines="none">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Question" InsertVisible="False" SortExpression="ID">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox ID="chkChoice" runat="server" onclick="chkGroup(this)" />
                                                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                                                                                </ItemTemplate>                                                                                                              
                                                                                                               <%-- <ItemStyle Width="432px" />--%>
                                                                                                            </asp:TemplateField>
                                                                                                           <%-- ItemStyle-Width="250px"--%>
                                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" >
                                                                                                                <ItemTemplate>
                                                                                                                    <%--Width="200px"--%>
                                                                                                                    <asp:TextBox ID="txt_SubQuestion" runat="server" onchange="javascript:CheckUnCheckAll(this);" ></asp:TextBox>
                                                                                                                    <asp:HiddenField ID="hfBoxSect" runat="server" Value='<% #Eval("Box")%>' />
                                                                                                                    <img width="0px" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="Box" HeaderText="Box" />
                                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox TabIndex="-1" ID="chkSubQuesAppl" runat="server" />
                                                                                                                    <asp:HiddenField ID="hfSectn" runat="server" />
                                                                                                                    <asp:HiddenField ID="hfSubSectn" runat="server" />
                                                                                                                    <asp:HiddenField ID="hfQtn" runat="server" Value='<% #Eval("Code")%>' />
                                                                                                                    <asp:HiddenField ID="hfSkill" runat="server" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <HeaderStyle CssClass="HeaderStyle" Height="25px" ForeColor="White" Width="5%" />

                                                                                                        <RowStyle CssClass="RowStyle" Width="5px" />
                                                                                                        <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                                                        <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                                                                        <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                                                                        <PagerStyle CssClass="PagerStyle" BackColor="#FFFFFF" HorizontalAlign="Center" />
                                                                                                        <EmptyDataRowStyle CssClass="EmptyDataRowStyle" ForeColor="White" />
                                                                                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                                                                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <HeaderStyle CssClass="HeaderStyle" Height="25px" ForeColor="White" Width="5%" />

                                                                            <RowStyle CssClass="RowStyle" Width="5px" />
                                                                            <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                                            <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                                            <PagerStyle CssClass="PagerStyle" BackColor="#FFFFFF" HorizontalAlign="Center" />
                                                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" ForeColor="White" />
                                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="pnlClickB"
                                                                        Collapsed="true" ExpandControlID="pnlClickB" TextLabelID="lblMessage" CollapsedText="Show"
                                                                        ExpandedText="Hide" ImageControlID="imgArrows" CollapsedImage="~/Administration/Images/downarrow.jpg"
                                                                        ExpandedImage="~/Administration/Images/uparrow.jpg" ExpandDirection="Vertical" TargetControlID="pnlCollapsableB"
                                                                        ScrollContents="false">
                                                                    </asp:CollapsiblePanelExtender>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                            <asp:GridView ID="grd_SubSections" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                HorizontalAlign="Justify" Style="margin-left: 0px;" GridLines="None">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="150px" />
                                                                    <%--ItemStyle-Width="250px"--%>
                                                                    <asp:TemplateField HeaderText="Point" ItemStyle-HorizontalAlign="Left" >
                                                                        <ItemTemplate>
                                                                             <%--Width="200px"--%>
                                                                            <asp:TextBox ID="txt_Section" runat="server" onchange="javascript:CheckUnCheckAll(this);"
                                                                                OnTextChanged="txt_Section_TextChanged"></asp:TextBox>
                                                                            <asp:HiddenField ID="hfBoxSect" runat="server" Value='<% #Eval("Box")%>' />
                                                                            <img width="0px" />
                                                                        </ItemTemplate>
                                                                        <%--Width="250px"--%>
                                                                        <ItemStyle HorizontalAlign="Left"  />
                                                                    </asp:TemplateField>
                                                                    <%--ItemStyle-Width="20px"--%>
                                                                    <asp:TemplateField  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="left">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox TabIndex="-1" ID="chkSectAppl" runat="server" />
                                                                            <asp:HiddenField ID="hfSectn" runat="server" />
                                                                            <asp:HiddenField ID="hfSubSectn" runat="server" />
                                                                            <asp:HiddenField ID="hfQtn" runat="server" Value='<% #Eval("Code")%>' />
                                                                            <asp:HiddenField ID="hfSkill" runat="server" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <%--Width="20px"--%>
                                                                        <ItemStyle HorizontalAlign="Center"  />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Box" HeaderText="Box">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="HeaderStyle" Height="25px" ForeColor="White" Width="5%" />

                                                                <RowStyle CssClass="RowStyle" Width="5px" />
                                                                <AlternatingRowStyle CssClass="AltRowStyle" />
                                                                <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                                                <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                                <PagerStyle CssClass="PagerStyle" BackColor="#FFFFFF" HorizontalAlign="Center" />
                                                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" ForeColor="White" />
                                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                <SortedAscendingHeaderStyle BackColor="#487575" />
                                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                <SortedDescendingHeaderStyle BackColor="#275353" />
                                                            </asp:GridView>
                                                            
                                                        </asp:Panel>
                                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="pnlClick"
                                                            Collapsed="true" ExpandControlID="pnlClick" TextLabelID="lblMessage" CollapsedText="Show"
                                                            ExpandedText="Hide" ImageControlID="imgArrows" CollapsedImage="~/Administration/Images/downarrow.jpg"
                                                            ExpandedImage="~/Administration/Images/uparrow.jpg" ExpandDirection="Vertical" TargetControlID="pnlCollapsable"
                                                            ScrollContents="false">
                                                        </asp:CollapsiblePanelExtender>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                                <br />
                                            </asp:Panel>
                                            <!--</div>-->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <input id="btn_Save" runat="server" type="button" value="Mark as Completed" class="NFButtonWithNoImage" onclick="javascript: CheckError(this, 2); scrollToTop();" />
                                                        <asp:Button ID="btnSave" runat="server" CssClass="NFButton" OnClick="btnSave_Click" Text="Mark" />
                                                    </td>
                                                    <td>
                                                        <input id="btn_Submit" runat="server" type="button" value="Save as Draft" class="NFButtonWithNoImage" onclick="javascript: CheckError(this, 1);" />
                                                        <asp:Button ID="btnSubmit" runat="server" CssClass="NFButton" OnClick="btnSubmit_Click" Text="Save" />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>

                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="100"
                                                AssociatedUpdatePanelID="UpdatePanel1">
                                                <ProgressTemplate>
                                                    <img src="../Administration/images/load.gif" alt="loading" />
                                                    Please Wait...
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td id="lbl_Msg" runat="server" style="text-align: center"></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hfCheckError" runat="server" Value="0" />
                                            <asp:HiddenField ID="hfConfirm" runat="server" Value="0" />
                                        </td>
                                    </tr>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>

    </form>
</body>
</html>


