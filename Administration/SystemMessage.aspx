

<%@ Page   Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Administration/AdminMaster.master"  CodeFile="SystemMessage.aspx.cs" Inherits="Administration_SystemMessage" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


   

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
                document.getElementById("<%=lblMesg.ClientID %>").style.display = "none";
            }, seconds * 1000);


        }

    </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">


    <table width="99%" cellpadding="0" cellspacing="2" border="1">

     
             <tr>
                 <td>
            <div >
                  <asp:TextBox id="TAMessage"   TextMode="MultiLine" Height="200" width="400" name="TAMessage"  runat="server"  placeholder="Enter Message"></asp:TextBox>&nbsp;&nbsp;	
                  <%--
                   style="display:none;" --%>	
                   <ItemTemplate> <asp:Button name="BtnSetMessage" Class="NFButton" id="BtnSetMessage" runat="server" value="Save"   onclick="BtnSetMessage_Click" Text="Save" />	
                    </ItemTemplate>	
                   <ItemTemplate>	 <asp:Label ID="lblMesg" runat="server"  ></asp:Label>  	</ItemTemplate>		     
            </div>
    
     </td>
             </tr>
       

    </table>


</asp:Content>

