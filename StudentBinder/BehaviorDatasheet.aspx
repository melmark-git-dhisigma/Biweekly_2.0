<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BehaviorDatasheet.aspx.cs" Inherits="StudentBinder_BehaviorDatasheet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .MainContainer {
            width: 100%;
        }

        .lftContainer {
            width: 80%;
        }

        .rightPartContainer {
            border: 3px solid #0D668E;
            float: right;
            height: auto !important;
            margin: 0 auto 0 0.2%;
            min-height: 500px;
            width: 22.5%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="MainContainer">

            <div class="rightPartContainer">
                <iframe id="IfrmTimer" scrolling="no" frameborder="0" style="height: auto; min-height: 497px; width: 100%;overflow-x:hidden;overflow-y:auto" runat="server" src="dataSheetTimer.aspx"></iframe>
            </div>
        </div>
    </form>
</body>
</html>
