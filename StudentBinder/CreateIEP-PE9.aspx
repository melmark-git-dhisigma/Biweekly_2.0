<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateIEP-PE9.aspx.cs" Inherits="StudentBinder_CreateIEP_PE9" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/StylePE.css" rel="stylesheet" />
    <script src="js/jquery-1.8.0.min.js"></script>
        <link href="CSS/StylePE15.css" rel="stylesheet" />
    <style>
        .FreeTextDivContent {
            border: 1px solid #E4E4E4;
            border-radius: 8px 8px 8px 8px;
            height: auto;
            margin: 1%;
            min-height: 70px;
            padding: 4%;
            width: 100%;
        }
    </style>


    <script type="text/javascript">
        //TextBoxTrainingGoal TextBoxTrainingCourse TextBoxEmploymentGoal TextBoxEmploymentCourse TextBoxIndependentLivingGoal TextBoxIndependentLivingCourse
        var txt1 = "", txt2 = "", txt3 = "", txt4 = "", txt5 = "", txt6 = "";
        function GetFreetextval(content, divid) {
            if (divid == 'TextBoxDetailsA') {
                document.getElementById('<%=TextBoxDetailsA.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxDetailsA.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxDetailsA_hdn.ClientID %>').value = window.escape(content);
                txt1 = content;
            }
            else if (divid == 'TextBoxDetailsB') {
                document.getElementById('<%=TextBoxDetailsB.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxDetailsB.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxDetailsB_hdn.ClientID %>').value = window.escape(content);
                txt2 = content;
            }
                //        
            else if (divid == 'TextBoxDetailsC') {
                document.getElementById('<%=TextBoxDetailsC.ClientID %>').innerHTML = "";
                document.getElementById('<%=TextBoxDetailsC.ClientID %>').innerHTML = content;
                document.getElementById('<%=TextBoxDetailsC_hdn.ClientID %>').value = window.escape(content);
                txt3 = content;
            }

        }

function loadValues() {
    if (document.getElementById('<%=TextBoxDetailsA.ClientID %>').innerHTML != "") {
        txt1 = document.getElementById('<%=TextBoxDetailsA.ClientID %>').innerHTML;
        txt1 = txt1.replace(/'/g, '##');
        txt1 = txt1.replace(/\\/g, '?bs;');
    }
    if (document.getElementById('<%=TextBoxDetailsB.ClientID %>').innerHTML != "") {
        txt2 = document.getElementById('<%=TextBoxDetailsB.ClientID %>').innerHTML;
        txt2 = txt2.replace(/'/g, '##');
        txt2 = txt2.replace(/\\/g, '?bs;');
    }
    if (document.getElementById('<%=TextBoxDetailsC.ClientID %>').innerHTML != "") {
        txt3 = document.getElementById('<%=TextBoxDetailsC.ClientID %>').innerHTML;
        txt3 = txt3.replace(/'/g, '##');
        txt3 = txt3.replace(/\\/g, '?bs;');
    }

}

function submitClick() {
    loadValues();

    $.ajax(
  {

      type: "POST",
      url: "CreateIEP-PE9.aspx/submitIepPE9",
      data: "{'arg1':'" + txt1 + "','arg2':'" + txt2 + "','arg3':'" + txt3 + "'}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      async: false,
      success: function (data) {

          var contents = data.d;

      },
      error: function (request, status, error) {
          alert("Error");
      }
  });
}

function scrollToTop() {
    window.scrollTo(0, 0);
    window.parent.parent.scrollTo(0, 100);
}

</script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="divIEPP1">
             <div class="ContentAreaContainer">
                 <br />
                 <div class="clear"></div>
            <table cellpadding="0" cellspacing="0" width="96%">
                 <tr>
                    <td id="tdMsg" runat="server" class="top righ"></td>
                </tr>

                <tr>
                    <td class="righ" >INDIVIDUALISED EDUCATION PROGRAM(IEP)
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0"> 
                <tr>
                    <td colspan="2" class="righ">Individual's Name : <asp:Label runat="server" id="lblStudentName"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2" class="righ" style="text-align: center">

                        <h2 class="simble">Local Assessments</h2></td>
                </tr>
               
                <tr>
                   
                    <td class="righ"colspan="2">

                        <asp:CheckBoxList ID="CheckBoxListLocalAsssesment" runat="server" >
                            <asp:ListItem Value="A">Local assessment is not administered at this Individual’s grade level; OR</asp:ListItem>
                            <asp:ListItem Value="B">Individual will participate in local assessments without accommodations; OR</asp:ListItem>
                            <asp:ListItem Value="C">Individual will participate in local assessments with the following accommodations; OR</asp:ListItem>
                        </asp:CheckBoxList></td>
                </tr>
                <tr>
                   

                    <td class="righ"colspan="2" >
                        <asp:TextBox runat="server" ID="TextBoxDetailsA_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxDetailsA" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE9.aspx',this); "></div>
                        <%-- <asp:TextBox ID="TextBox3" runat="server" Height="60px" TextMode="MultiLine" Width="888px"></asp:TextBox>--%>

                                
                    </td>
                </tr>
                <tr>
                    <td><asp:CheckBox ID="CheckBox1" runat="server" /></td>
                    <td class="auto-style1 righ">
                        The Individual will take an alternate local assessment.
                    </td>
                </tr>
                <tr>
                   
                    <td class="righ"colspan="2" >Explain why the Individual cannot participate in the regular assessment:</td>
                </tr>
                <tr>
                   
                   <td class="righ"colspan="2" >
                        <%-- <asp:TextBox ID="TextBox1" runat="server" Height="60px" TextMode="MultiLine" Width="888px"></asp:TextBox> --%>
                       <asp:TextBox runat="server" ID="TextBoxDetailsB_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxDetailsB" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE9.aspx',this); "></div>
                    </td>
                </tr>
                <tr>
                    
                    <td class="righ"colspan="2" >Explain why the alternate assessment is appropriate:</td>
                </tr>
                <tr>
                   
                    <td class="righ"colspan="2" >
                        <asp:TextBox runat="server" ID="TextBoxDetailsC_hdn" Text="" style="display:none;"></asp:TextBox>
                        <div id="TextBoxDetailsC" runat="server" style="overflow:scroll;" class="FreeTextDivContent" onclick="scrollToTop(); parent.freeTextPopup('CreateIEP-PE9.aspx',this); "></div>
                        <%-- <asp:TextBox ID="TextBox2" runat="server" Height="60px" TextMode="MultiLine" Width="888px"></asp:TextBox>    --%>
                    </td>
                </tr>
           

                <tr>
                    <td colspan="2" class="righ">
                        <asp:Button ID="btnSave" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_Click" Text="Save and continue" OnClientClick=""/>

                        <%-- <asp:Button ID="btnSave_hdn" runat="server" CssClass="NFButtonWithNoImage"
                            OnClick="btnSave_hdn_Click" Text="dummy" OnClientClick="" style="display:none;"/>--%>
                    </td>
                </tr>
            </table>
                  <div class="clear"></div>
                 </div>
        </div>
    </form>
</body>
</html>
