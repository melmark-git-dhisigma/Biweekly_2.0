<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GraphTileGrid.aspx.cs" Inherits="StudentBinder_GraphTileGrid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GraphDashboard</title>
    <script> 
        function PopupAcademic() { window.parent.setName1(1); }
        function PopupClinical() { window.parent.setName1(2); }
        function PopupSessionBased() { window.parent.setName1(3); }
        function PopupChained() {window.parent.setName1(4);}
        function PopupExcelView() { window.parent.setName1(5); }
        function PopupAcademicPGSummary() { window.parent.setName1(6); }
        function PopupClinicalPGSummary() { window.parent.setName1(7); }
        function PopupMaintenance() { window.parent.setName1(8); }

    </script>
</head>
<body>
    <form id="form1" runat="server">
     
    <div >
    <table id = "GraphGrid" style="margin-left:210px; margin-top:50px">
	    <tr>
            <td><div><asp:LinkButton ID="Academic1" runat="server" CommandName="1"  OnClick="loadgraphMenu"><img src="images/Academic.jpg" alt="Academic"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></div></td>
		    <td> <asp:LinkButton ID="Clinical" runat="server" CommandName="2" OnClick="loadgraphMenu"><img src="images/Clinical.jpg" alt="Clinical"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></td>
            <td><div><asp:LinkButton ID="Session_Based_Academic" runat="server" CommandName="3" OnClick="loadgraphMenu"><img src="images/Session_Based.jpg" alt="Session-Based-Academic"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></div></td>
            <td><div><asp:LinkButton ID="Chained" runat="server" CommandName="4" OnClientClick="PopupChained();"><img src="images/Session_Chained.jpg" alt="Session-Based-Chained"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></div></td>
		  
	    </tr>
	    <tr>
		     <td><div><asp:LinkButton ID="Excel_View" runat="server" CommandName="5" OnClick="loadgraphMenu"><img src="images/Excel_View.jpg" alt="Excel_view"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></div></td>
		    <td><div><asp:LinkButton ID="Progress_Summary_Academic" runat="server" CommandName="6" OnClick="loadgraphMenu"><img src="images/Progress_Summary.jpg" alt="Progress_Summary_Academic"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></div></td>
            <td><div><asp:LinkButton ID="Progress_Summary_Clinical" runat="server" CommandName="7" OnClick="loadgraphMenu"><img src="images/ProgressSummay_Clinical.jpg" alt="Progress_Summary_clinical"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></div></td>
             <td><div><asp:LinkButton ID="Maintenance" runat="server" CommandName="8" OnClientClick="PopupMaintenance()"><img src="images/Maintenance.jpg"  alt="Maintanance"  border="0" style="width:180px;height:180px;"/></asp:LinkButton></div></td>
		    
	    </tr>
    </table>
    </div>
    </form>
</body>
</html>
