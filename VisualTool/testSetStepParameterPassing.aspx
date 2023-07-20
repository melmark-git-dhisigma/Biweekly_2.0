<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testSetStepParameterPassing.aspx.cs" Inherits="testSetStepParameterPassing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Set Number<asp:TextBox ID="txtSets" runat="server"></asp:TextBox>
        Step Number<asp:TextBox ID="txtSteps" runat="server"></asp:TextBox>
       No: of Attempts Needed <asp:TextBox ID="txtNoOfAttempts" runat="server"></asp:TextBox>
        <asp:Button ID="btnParameter" runat="server" Text="Redirect" OnClick="btnParameter_Click" />
    </div>
    </form>
</body>
</html>
