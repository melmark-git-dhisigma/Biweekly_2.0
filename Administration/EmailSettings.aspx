

<%@ Page   Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Administration/AdminMaster.master"  CodeFile="EmailSettings.aspx.cs" Inherits="Administration_SystemMessage" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


   <style>

       .PasswordFieldStyle
       {
            width: 241px;
            height: 25px !important;
            border-color: #c7c7c3;
            border-radius: 3px;
            border-width: 1px;
            border-style: solid;
       }

       .ViewPswd
       {
           width: 22px;
           vertical-align: top;
           border: 1px solid #c7c7c3;
           padding: 2px 3px 1px 3px;
           border-radius: 3px;
           background-color: #03507d;
           margin-left: 10px;
       }

   </style>

    <script type="text/javascript">



        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMesg.ClientID %>").style.visible = "false";
            }, seconds * 1000);
        };
        function NoMessage() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMesg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        }
        function MessageUpdated() {


            var seconds = 5;
            setTimeout(function () {
                //document.getElementById("<%=lblMesg.ClientID %>").style.display = "none";
                document.getElementById("<%=tdReadMsg.ClientID %>").style.display = "none";
            }, seconds * 1000);


        }

        $(document).ready(function () {
            var Cond = "";
            //alert("1"+Cond);
            $("#ImageHosting").click(function () {
                //alert("2" + Cond);
                //$('#txtPassword').attr('Value', '******************');                
                if (Cond == "false") {
                    //alert("3" + Cond);
                    $('#pPswd').show();
                    $('#pText').hide();
                    Cond = "";
                }
                else {
                    //alert("4" + Cond);
                    $('#pPswd').hide();
                    $('#pText').css('display', 'inline-block');
                    $('#pText').show();
                    Cond = "false";
                }
            });
        });

        function Copy() {
            var val1 = document.getElementById('<%= txtPassword.ClientID %>').value;
            document.getElementById('<%= txtPwdVisible.ClientID %>').value = val1;
        }

    </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <div id="UserEmailDetails" style="width:100%;">
        <ItemTemplate>
            <asp:Label ID="lblMesg" runat="server"  ></asp:Label>
        </ItemTemplate>	
        <table cellpadding="0" cellspacing="10" border="0">
            <tr>
                <td id="tdReadMsg" runat="server" colspan="4"></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label><span id="Span1" runat="server" style="color: red;padding: 3px;">*</span></td>
                <td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label><span id="Span2" runat="server" style="color: red;padding: 3px;">*</span></td>
                <td>
                    <p id="pPswd" style="display:inline-block;margin: 0 !important"><asp:TextBox ID="txtPassword" runat="server" Type="Password" CssClass="PasswordFieldStyle" onkeyup="Copy();"></asp:TextBox></p>
                    <p id="pText" style="display:none;margin: 0 !important"><asp:TextBox ID="txtPwdVisible" runat="server"></asp:TextBox></p>
                    <input type="image" src="images/ShowHideDSTimerWhite.png" id="ImageHosting" class="ViewPswd" Visible="false" value="Show"/>                   
                </td>
                <td></td>
                <td></td>                
            </tr>
            <tr>
                <td><asp:Label ID="lblSMTPadrs" runat="server" Text="SMTP Server"></asp:Label><span id="Span3" runat="server" style="color: red;padding: 3px;">*</span></td>
                <td><asp:TextBox ID="txtSMTPadrs" runat="server"></asp:TextBox></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblSMTPport" runat="server" Text="SMTP Port"></asp:Label><span id="Span4" runat="server" style="color: red;padding: 3px;">*</span></td>
                <td><asp:TextBox ID="txtSMTPport" runat="server"></asp:TextBox></td>
                <td></td>
                <td></td>
            </tr>
            <tr style="display:none;">
                <td><asp:Label ID="lblEmailbody" runat="server" Text="Email Body"></asp:Label></td>
                <td colspan="2"><asp:TextBox id="EmailBody" TextMode="MultiLine" Height="200" width="400" name="EmailBody" runat="server" placeholder="Set Email Content"></asp:TextBox></td>                
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>
                    <ItemTemplate>
                        <asp:Button name="BtnSetMessage" Class="NFButton" id="BtnSetMessage" runat="server" value="Save" OnClick="BtnSetMessage_Click" Text="Save" />	
                    </ItemTemplate>
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
        
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style ="display:none">
        <tr>
            <td>
                <div >
                    <asp:TextBox id="TAMessage" TextMode="MultiLine" Height="200" width="400" name="TAMessage" runat="server" placeholder="Enter Message"></asp:TextBox><br />	
                    <ItemTemplate>
                        <asp:Button name="BtnSetMessage" Class="NFButton" id="BtnSetMessageOld" runat="server" value="Save" OnClick="BtnSetMessage_Click" Text="Save" />	
                    </ItemTemplate>	
                    <ItemTemplate>
                        <asp:Label ID="lblMesgOld" runat="server"  ></asp:Label>
                    </ItemTemplate>		     
                </div>
            </td>
        </tr>
    </table>


</asp:Content>

