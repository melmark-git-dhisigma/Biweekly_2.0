<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testing_static.aspx.cs" Inherits="testing_static" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        static int
    
        x =
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br />
        int y =
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Add x" OnClick="Button1_Click" />
         <asp:Button ID="Button3" runat="server" Text="Add y" OnClick="Button3_Click" />
        <asp:Button ID="Button2" runat="server" Text="Refresh" OnClick="Button2_Click" />
    
    </div>
    </form>
</body>
</html>
