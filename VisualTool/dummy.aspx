<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dummy.aspx.cs" Inherits="dummy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
</head>

<body>
<!-- top panel -->
    <form runat="server" id="form1">
        <object type="application/x-shockwave-flash" data="player_mp3.swf" width="200" height="20">
							<param name="movie" value="player_mp3.swf" />
							<param name="FlashVars" value="mp3=test.mp3" />
						</object>
        <br />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <br />
        <br />
        <asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>
        <br />
        <br />
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/VisualTool/icons/audio2.png" />
        </form>
    </body>
</html>
