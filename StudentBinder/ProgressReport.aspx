<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgressReport.aspx.cs" Inherits="StudentBinder_ProgressReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <link href="../Administration/CSS/jquery-ui.css" type="text/css" rel="stylesheet" />
    <script src="../Administration/JS/jquery-ui.js" type="text/javascript"></script>
    <link href="../Administration/CSS/popupStyle1.css" type="text/css" rel="stylesheet" />
    <link href="../Administration/CSS/MenuStyle.css" type="text/css" rel="stylesheet" />
   
    <script>
        function allowBackSpace(e, obj) {
            var evt = e || window.event;
            if (evt.keyCode == 8) {
                obj.value = '';
            }
        }
    </script>
    <script type="text/javascript">

       

        function AddBtn_Click(id) {
            var goalid = id.split('_');
            var tableid = "tbl_" + goalid[1];
            var table = document.getElementById(tableid);
            var count = $('#' + tableid + ' tr').length;
            if (count < 17) {// Only 4 progress report information is available
                var prptno = 2;
                if (count == 9||count==10) prptno = 3;
                else if (count == 13||count==14) prptno = 4;
                
                var row1 = table.insertRow(count - 1);
                var cell0 = row1.insertCell(0);
                cell0.innerHTML = "<hr  style='width:100%' border:'solid' />";
                cell0.colSpan = 3;

                var row = table.insertRow(count);
                var cell1 = row.insertCell(0);
                var cell2 = row.insertCell(1);
                var cell3 = row.insertCell(2);
                cell1.innerHTML = "Progress Report Date: <input id='date_" + prptno + "_" + goalid[1] + "' class='datepicker' onkeypress='return false' onkeydown='allowBackSpace(event,this)' onpaste='return false' type='text' value=''>";
                cell2.innerHTML = "";
                cell3.innerHTML = "Progress Report # "+prptno+" of 4";

                var row2 = table.insertRow(count + 1);
                var rwcell1 = row2.insertCell(0);
                rwcell1.innerHTML = "Progress Reports are required to be sent to parents at least as often as parents are informed of their non-disabled children’s progress. Each progress report must describe the student’s progress toward meeting each annual goal.";
                rwcell1.colSpan = 3;

                var row3 = table.insertRow(count + 2);
                var rw3cell1 = row3.insertCell(0);
                rw3cell1.innerHTML = "<input id='info_" + prptno + "_" + goalid[1] + "'  type='text' style='width: 90%; height: 60px; border: 1px solid #ccc' />";
                rw3cell1.colSpan = 3;
            }
            ShowDatePicker();
        }


        function saveProgressReport(Goalids) {
            var goals = Goalids.split(',');
            var goal_date_info = [];
            var datacount=0;
            for (j = 0; j < goals.length; j++) {
                if ($("#tbl_" + goals[j]).length) {
                    var rowcnt = $("#tbl_" + goals[j] + ' tr').length;
                    for (i = 1; i <= 4; i++) {
                        if ($("#date_" + i + "_" + goals[j]).length) {
                            var dateval = $("#date_" + i + "_" + goals[j]).val();
                            var rptinfo = $("#info_" + i + "_" + goals[j]).val();
                            goal_date_info[datacount] = goals[j] + "_" + dateval + "_" + rptinfo;
                            datacount = datacount + 1;
                        }
                    }
                }
            }
           
            $.ajax(
                  {
                      type: "POST",
                      url: "ProgressReport.aspx/SaveProgressReport",
                      //data: "{'Data':'" + goal_date_info + "'}",
                      data: JSON.stringify({ Data2: goal_date_info }),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",            //         
                      success: function (data) {
                          if (data.d != "")
                              alert(data.d);
                      },
                      error: function (request, status, error) {
                          alert(error);
                      }
                  });
        }

        function ProgressReport() {
            
            $('#overlay').fadeIn('slow', function () {
                $('#dialog').css("display", "block");
                $('#dialog').css("top", "6%");
                //$('#dialog').animate({ top: '20%' }, { duration: 'slow', easing: 'linear' })
            });
        }

        function ClosePopup() {
            $('#dialog').animate({ top: "-300%" }, function () {
               $('#overlay').fadeOut('slow');
            });
        }

        function ExportAll(iepID) {
            $("#btnExp2").attr('value', 'Exporting...');
            $("#btnExp2").prop("disabled", true);

            __doPostBack('ExportAll', iepID);

            setTimeout(function () {
                $("#btnExp2").attr('value', 'Export');
                $("#btnExp2").prop("disabled", false);
            }, 3000);
        }

        function NoIEPToExport() {
            alert("No data to export");
        }
       
    </script>
    <style type="text/css">
        div.ui-datepicker {
        font-size:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
             <div id="partialMainArea" style="height:100%;" >
     <div id="ProgressRpt" runat="server" style="border:1px solid;padding:5px;" >
             </div>
    
   
    
           
</div>
        
        
         <div id="overlay" class="web_dialog_overlay">
        </div>
        <div id="dialog" class="web_dialog" style="left: 30%;top:6%">

            <div id="sign_up5">
                <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px; height: 25px; width: 25px;" onclick="ClosePopup()">
                    <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="25" height="25" alt="" /></a>
                <h3>Progress List</h3>
                <hr />
               <div style="overflow-y:scroll;height:350px"> 
                   <asp:GridView ID="GrdProgressList" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data Found..." GridLines="None"   Width="100%" Height="100px"  OnRowCommand="GrdProgressList_RowCommand" >
                                            <Columns>
                                                <asp:BoundField DataField="EffStartDate" HeaderText="IEP Start Date"></asp:BoundField>
                                                <asp:BoundField DataField="EffEndDate" HeaderText="IEP End Date"></asp:BoundField>
                                                <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                                                <asp:TemplateField HeaderText="View">

                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkView" runat="server" Font-Underline="False" CommandName="View" CommandArgument='<%# Eval("StdtIEPId") %>'>View</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                            <HeaderStyle CssClass="HeaderStyle" />
                                            <RowStyle CssClass="RowStyle" />
                                            <AlternatingRowStyle CssClass="AltRowStyle" />
                                            <FooterStyle CssClass="FooterStyle" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#339966" CssClass="SelectedRowStyle" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle CssClass="PagerStyle" ForeColor="White" HorizontalAlign="Center" />
                                            <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                        </asp:GridView>
               </div>
            </div>
        </div>

        </form>
















</body>
    <script type="text/javascript">
        $(document).ready(function () {
            ShowDatePicker();
            $('#dialog').css("display", "none");
        });
        function ShowDatePicker() {
            $('.datepicker').datepicker({
                changeMonth: true,
                changeYear: true
            });
        }
    </script>
</html>
