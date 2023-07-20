<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentCheckin.aspx.cs" Inherits="StudentBinder_Phase2Css_StudentCheckin" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- <link href="Phase2Css/styles.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" />
    <link href="../Administration/CSS/tabss.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
     <script type="text/javascript" src="../Administration/JS/jquery-1.8.0.js"></script>
    <style type="text/css">
        
    </style>
    <script type="text/javascript">

        function adjustStyle() {
            var isiPad = navigator.userAgent.match(/iPad/i);

            if (isiPad!= null) {

                $('#set').css('width', '600px');
                $('.setBox').css('width', '600px');
                $('.btn-blue').css('height', '40px');
                $('.btn-blue').css('width', '60px');
                $('#ImageButton1').css('width', '40px');
                $('.setBox').css('height', '500px');
                
                $('#modal').css('height', '500px');
            }
        }
        $(function () {
            adjustStyle($(this).width());
            $(window).resize(function () {
                adjustStyle();
            });
        });


        function Click1() {
            document.getElementById("hidSearch").value = "0";
            //var img1 = document.getElementById("imgBDay");
            //img1.src = "img/DayB.png";
            //var img2 = document.getElementById("ImgBRes");
            //img2.src = "img/ResG.png";
            //document.getElementById("day").style.backgroundColor = "#E3EAEB";
            //document.getElementById("res").style.backgroundColor = "#ddd";
        }

        function Click2() {

            //var img1 = document.getElementById("imgBDay");
            //img1.src = "img/DayG.png";
            //var img2 = document.getElementById("ImgBRes");
            //img2.src = "img/ResB.png";

            //document.getElementById("res").style.backgroundColor = "#E3EAEB";
            //document.getElementById("day").style.backgroundColor = "#ddd";
            document.getElementById("hidSearch").value = "0";

        }
        function setAlert() {
            document.getElementById("hidSearch").value = "1";
        }
        function setAlert1() {
            document.getElementById("hidSearch").value = "0";
        }
        function changeStatus(img) {
            var imgUrl = img.src;
            var n = imgUrl.indexOf('/StudentBinder/img/out.png');

            if (n >= 0) {
                alert(img.src);
                img.src = "~/StudentBinder/img/in.png";
            }
            else {


            }

        }
    </script>


</head>
<body>
    <form id="form1" runat="server">


        <asp:HiddenField ID="hidSearch" Value="0" runat="server"  />

        <asp:HiddenField ID="hidSetVal" Value="0" runat="server"  />
        <table width="353px" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:ImageButton ID="imgBDay" runat="server" OnClientClick="Click1()" ImageUrl="~/StudentBinder/img/DayG.png" OnClick="imgBDay_Click" />

                    <asp:ImageButton ID="ImgBRes" runat="server" OnClientClick="Click2()" ImageUrl="~/StudentBinder/img/ResB.png" OnClick="ImgBRes_Click" />

                </td>
            </tr>
            <%-- <tr>
                <td width="1%">
                    <div id="day" runat="server" style="background-color: #E3EAEB;" class="tabLinks" onclick="Click1()">
                        

                    </div>
                </td>
                <td width="5%" align="left">
                    <div id="res" style="top: 5px" runat="server" onclick="Click2()" class="tabLinks">
                        
                    </div>
                </td>
                

            </tr>--%>
            <tr>
                <td>
                    <div class="setBox">

                        <div id="set" style="padding: 5px 5px 5px 5px;">



                            <table style="background-color: #FFFFFF; width: 97%" border="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSearch" runat="server" Width="227px" AutoPostBack="True" ></asp:TextBox></td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" OnClientClick="setAlert1();" runat="server" OnClick="ImageButton1_Click" CssClass="btn-black-srch" BackColor="#33ccff" ImageUrl="~/StudentBinder/img/searchbtn.png" />
                                </tr>
                            </table>

                            <div id="modal" style="height: 300px; width: 97%; overflow: auto; background-color: white; margin-top: 5px; margin-right: 3px;">


                                <asp:GridView ID="grdGroup" runat="server" AutoGenerateColumns="False" Width="100%"
                                    ShowHeader="False" AllowSorting="True" EmptyDataText="No Data Found.." OnRowCommand="grdGroup_RowCommand" OnRowEditing="grdGroup_RowEditing" Style="" OnRowDataBound="grdGroup_RowDataBound">
                                    <PagerStyle CssClass="PagerStyle" />
                                    <Columns>
                                        <asp:BoundField DataField="Name" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lbl_status" CommandName="Edit" runat="server" CommandArgument='<%# Eval("StudentId")+";" +Eval("ClassId")%>'
                                                    OnClientClick="setAlert();" ImageUrl="~/StudentBinder/img/out.png" class="btn btn-blue" Width="50px" Height="25px" ></asp:ImageButton>
                                                <asp:HiddenField ID="hidStatus" runat="server" Value='<%# Eval("chkStatus") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>



                    </div>
                </td>
            </tr>
        </table>


    </form>
</body>
</html>
