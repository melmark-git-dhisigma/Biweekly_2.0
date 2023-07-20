<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SharePointBehavior.aspx.cs" Inherits="StudentBinder_SharePointBehavior" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="jsScripts/jQuery.js"></script>
    <script type="text/javascript">
        $(function () {
            window.open('<%=System.Configuration.ConfigurationManager.AppSettings["BehaviorSharePointRpt"] %>', '_blank');
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
