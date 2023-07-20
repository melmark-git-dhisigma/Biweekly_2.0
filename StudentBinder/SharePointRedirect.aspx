<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SharePointRedirect.aspx.cs" Inherits="StudentBinder_SharePointRedirect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="jsScripts/jQuery.js"></script>
    <script type="text/javascript">
        $(function () {
            window.open('<%=System.Configuration.ConfigurationManager.AppSettings["DataSheetSharePointRpt"] %>', '_blank');
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
