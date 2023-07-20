<%@ Page Title="" Language="C#" AutoEventWireup="true"
    CodeFile="AssessmentBySkill.aspx.cs" Inherits="Administration_GoalAssess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <script src="../Administration/JS/jquery1.4.2.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/DisplayStyles.css" rel="stylesheet" />
  

    <script type="text/javascript">
        function showNestedGridView(obj) {
            var nestedGridView = document.getElementById(obj);
            var imageID = document.getElementById('image' + obj);

            if (nestedGridView.style.display == "none") {
                nestedGridView.style.display = "inline";
                imageID.src = "../Administration/images/minus.png";
            } else {
                nestedGridView.style.display = "none";
                imageID.src = "../Administration/images/plus.png";
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
                var row = txt.parentNode.nextSibling.getElementsByTagName('input');
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
                        img[0].style.width = "15px";
                        img[0].style.height = "15px";
                        img[0].src = "../Administration/Images/symbol-error.png";

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
                    //PageMethods.IsValidStatus(box.value, txt.value, OnSuccess, OnFailure);

                }
                else {
                    img[0].style.width = "0px";
                    img[0].style.height = "0px";
                    img[0].src = "";
                    //test = next('img');
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
            var r = confirm("Are you sure want to complete the Assessment");

            var hf = document.getElementById('<%=hfConfirm.ClientID %>');
            hf.value = r;

        }


        function scrollToTop() {
            window.scrollTo(0, 0);
            window.parent.parent.scrollTo(0, 0);
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
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <table width="100%">
                        <tr>
                            <td>
                                <table width="100%">

                                    <asp:HiddenField ID="hf_GoalName" runat="server" />
                                    <tr>
                                        <td class="tdText"></td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <table width="88%">
                                                <tr>
                                                    <td class="tdText">
                                                        <table style="width:99%;">
                                                            <tr>
                                                                <td class="tdText" style="width: 10%;">
                                                                    <label for="email">
                                                                        Year :
                                        <asp:Label ID="lblYear" runat="server" Text="Label"></asp:Label>
                                                                    </label>
                                                                </td>
                                                                <td></td>
                                                                <td class="tdText"  style="text-align:right;">Assessment Name:</td>
                                                                <td align="left">

                                                                    <asp:TextBox ID="txt_AssmntName" runat="server" CssClass="textClass" ReadOnly="true"></asp:TextBox></td>
                                                                <td></td>
                                                                <td class="tdText" style="text-align:right;">Status: <asp:Label ID="lblStatus" runat="server" ></asp:Label></td>
                                                                
                                                            </tr>
                                                        </table>








                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:Panel ID="pnlnote" runat="server" Visible="false">
                                                        <td style="height: 70px;padding-left:3px; vertical-align: top;" class="tdText">Notes<br />
                                                            <div style="height: 75%; overflow: auto;">
                                                                <asp:Label ID="lblNote" runat="server"></asp:Label>
                                                            </div>
                                                        </td>
                                                    </asp:Panel>
                                                </tr>
                                                <tr>
                                                    <td class="tdText">Enter Your Notes Here<br />
                                                        <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Rows="5" Columns="5"
                                                            
                                                            BorderWidth="1px" Width="99%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center" style="font-family: Arial; font-weight: bold; font-size: 16px; color: gray;">
                                            <br />
                                            <asp:Label ID="lbl_Goal" runat="server" Text="---"></asp:Label></td>
                                    </tr>
                                </table>

                                <table width="100%">

                                    <tr>
                                        <td align="center">
                                            <!--<div class="divBackgrnd">-->
                                            <asp:Panel ID="Panel1" runat="server" Style="overflow-y: hidden;" Width="100%">
                                                <br />
                                                <div id="Msg" runat="server" style="font-size: larger; font-weight: bold; color: Blue; text-align: center;"></div>
                                                <div>

                                                <asp:DataList ID="dl_Assessmnts" runat="server" OnItemDataBound="dl_Assessmnts_ItemDataBound"
                                                    Width="90%" Font-Bold="True" Font-Size="Medium" >
                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlClick" runat="server">
                                                            <div >
                                                                <div style="cursor: pointer; padding-right: 5px; text-align: left;">
                                                                    <table width="100%" style="background: #ededed url(../Administration/images/topbtmline.JPG) right top repeat-y;border-bottom: 1px dotted #116c90;">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:LinkButton ID="lb_Assess" runat="server" Text='<%#Eval("AsmntName") %>' Font-Underline="False"
                                                                                    Enabled="false" Font-Names="Arial" Font-Size="12px" Font-Bold="true" ></asp:LinkButton>
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
                                                            <asp:DataList ID="dl_Sections" runat="server" OnItemDataBound="dl_Sections_ItemDataBound"
                                                                Width="100%">
                                                                <ItemTemplate>
                                                                    <asp:Panel ID="pnlClick2" runat="server" CssClass="pnlCSS">
                                                                        <div style="background-image: url('~/green_bg.gif'); height: 22px; vertical-align: middle">
                                                                            <div style="color: White; cursor: pointer; padding-right: 5px; text-align: left;">
                                                                                <table width="100%" style="border-bottom: 1px dotted #116c90;" >
                                                                                    <tr>
                                                                                        <td style="background:url('../Administration/images/dotblk.JPG') no-repeat scroll 0 center transparent ">
                                                                                            <asp:LinkButton ID="lb_Section" Font-Bold="true" Font-Size="Small" runat="server" Text='<%# Eval("name") %>'
                                                                                                Enabled="false" style="padding:0 0 5px 10px;"></asp:LinkButton>
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
                                                                        <asp:GridView ID="grd_SubSection" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_SubSection_RowDataBound"
                                                                            Width="99.5%" ShowHeader="True" HorizontalAlign="Justify" Style=""
                                                                            AllowPaging="False" AllowSorting="True" GridLines="none">
                                                                            <Columns>
                                                                                <asp:TemplateField ItemStyle-Width="10px">
                                                                                    <ItemTemplate>
                                                                                        <a tabindex="-1" href="javascript:showNestedGridView('<%# Eval("ID") %>');">
                                                                                            <img id="image<%# Eval("ID") %>" alt="Click to show/hide orders" border="0" src="../images/plus.png" />
                                                                                        </a>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="10px" />
                                                                                </asp:TemplateField>
                                                                                <%--ItemStyle-Width="440px"--%>
                                                                                <asp:BoundField DataField="Code" HeaderText="Code" >
                                                                                    <ItemStyle Width="150px" />
                                                                                </asp:BoundField>
                                                                                <%--ItemStyle-Width="250px"--%>
                                                                                <asp:TemplateField HeaderText="Point" ItemStyle-HorizontalAlign="Left" >
                                                                                    <ItemTemplate>
                                                                                        <%--Width="200px"--%>
                                                                                        <asp:TextBox ID="txtSectPoint" runat="server" onchange="javascript:CheckUnCheckAll(this);" ></asp:TextBox>
                                                                                        <asp:HiddenField ID="hfBoxSect" runat="server" Value='<% #Eval("Box")%>' />
                                                                                        <img width="0px" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="N.A" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox TabIndex="-1" ID="chkSectAppl" runat="server" />
                                                                                        <asp:HiddenField ID="hfAsmnt" runat="server" />
                                                                                        <asp:HiddenField ID="hfSectn" runat="server" />
                                                                                        <asp:HiddenField ID="hfSubSectn" runat="server" />
                                                                                        <asp:HiddenField ID="hfQtn" runat="server" Value='<% #Eval("Code")%>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="Box" HeaderText="Box" />
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td colspan="100%">
                                                                                                <div id='<%# Eval("ID") %>' class="divGrid1">
                                                                                                    <asp:GridView ID="nestedGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                                                                                                        OnRowDataBound="nestedGridView_RowDataBound" ShowHeader="false" Width="600px" GridLines="none">
                                                                                                        <RowStyle BackColor="White" Font-Names="Consolas" Font-Size="Small" ForeColor="#330099"
                                                                                                            VerticalAlign="Top" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Question" InsertVisible="False" SortExpression="ID">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox ID="chkChoice" runat="server" onclick="chkGroup(this)" />
                                                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                               <%-- <ItemStyle Width="432px" />--%>
                                                                                                                <ItemStyle Width="150px" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtSubPoint" runat="server" onchange="javascript:CheckUnCheckAll(this);" Width="200px"></asp:TextBox>
                                                                                                                    <asp:HiddenField ID="hfBoxSect" runat="server" Value='<% #Eval("Box")%>' />
                                                                                                                    <img width="0px"/>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Not Applicable" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox TabIndex="-1" ID="chkQuesAppl" runat="server" />
                                                                                                                    <asp:HiddenField ID="hfAsmnt" runat="server" />
                                                                                                                    <asp:HiddenField ID="hfSectn" runat="server" />
                                                                                                                    <asp:HiddenField ID="hfSubSectn" runat="server" />
                                                                                                                    <asp:HiddenField ID="hfQtn" runat="server" Value='<% #Eval("Code")%>' />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="Box" HeaderText="Box" />
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
                                                                                    <ItemStyle Width="10px" />
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
                                                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="pnlClick2"
                                                                        Collapsed="true" ExpandControlID="pnlClick2" TextLabelID="lblMessage" CollapsedText="Show"
                                                                        ExpandedText="Hide" ImageControlID="imgArrows" CollapsedImage="../Administration/Images/downarrow.jpg"
                                                                        ExpandedImage="../Administration/Images/uparrow.jpg" ExpandDirection="Vertical" TargetControlID="pnlCollapsableB"
                                                                        ScrollContents="false">
                                                                    </asp:CollapsiblePanelExtender>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                           
                                                        </asp:Panel>
                                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="pnlClick"
                                                            Collapsed="true" ExpandControlID="pnlClick" TextLabelID="lblMessage" CollapsedText="Show"
                                                            ExpandedText="Hide" ImageControlID="imgArrows" CollapsedImage="../Administration/Images/downarrow.jpg"
                                                            ExpandedImage="../Administration/Images/uparrow.jpg" ExpandDirection="Vertical" TargetControlID="pnlCollapsable"
                                                            ScrollContents="false">
                                                        </asp:CollapsiblePanelExtender>
                                                    </ItemTemplate>
                                                </asp:DataList>

                                                </div><br />
                                            </asp:Panel>
                                            <!--</div>-->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <input id="btn_Save" runat="server" type="button" value="Mark as Completed" class="NFButtonWithNoImage" onclick="javascript: CheckError(this, 2); scrollToTop();" />
                                            <asp:Button ID="btnSave" runat="server" Text="Mark" CssClass="NFButton" OnClick="btnSave_Click" OnClientClick="scrollToTop();"/>
                                            &nbsp;&nbsp;
                                    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" CssClass="NFButton"/>
                                            <input id="btn_Submit" runat="server" type="button" value="Save as Draft" class="NFButtonWithNoImage" onclick="javascript: CheckError(this, 1);" />
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
                                            <asp:HiddenField ID="hfCheckError" runat="server" />
                                            <asp:HiddenField ID="hfConfirm" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </form>
</body>
</html>
