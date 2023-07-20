<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dopostback_tesing.aspx.cs" Inherits="dopostback_tesing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
    <script>
        function __doPostBack(eventTarget, eventArgument) {
            document.Form1.__EVENTTARGET.value = eventTarget;
            document.Form1.__EVENTARGUMENT.value = eventArgument;
            document.Form1.submit();
        }
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <div>
    <a id="LButton3" href="javascript:__doPostBack('Button2','');">LinkButton</a>
    <asp:Button ID="Button2" runat="server" Text="Button" onload="Button2_Click" />
    </div>
    </form>
</body>
</html>
