<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Admin_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">


<head>
    <%--<link rel="icon" type="image/x-icon" href="images/metaIcon.ico" />--%>
    <link rel="shortcut icon" type="image/x-icon" href="#" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Melmark Pennsylvania</title>
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/popupStyle1.css" rel="stylesheet" type="text/css" />



    <script src="JS/jquery-1.8.0.js" type="text/javascript"></script>



    <style type="text/css">
        .drpClass {
            border: 1px solid #d7cece;
            background-color: white;
            color: #676767;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 13px;
            line-height: 26px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            padding: 2px 2px 0px 10px;
            width: 250px;
            height: 35px;
            margin: 30px 0 0 10px;
        }
        .adminLimkBtn {
            background: url("images/1371730581_admin.PNG") left top no-repeat;
            border: 0 ;
            color: #FFFFFF;
            cursor: pointer;
            float: right;
            
            font-size: 12px;
            font-weight: bold;
            height: 52px;
          
            margin-right:91px;

            padding: 0;
            text-align: left;
            width: 50px;
        }
    </style>
    <link rel="stylesheet" href="new-dropdownlist/example/example.css" type="text/css" />
    <link rel="stylesheet" href="new-dropdownlist/dropkick.css" type="text/css" />
    <script src="new-dropdownlist/jquery.dropkick-1.0.0.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">
        $(function () {
            $('.newDDL').dropkick();
        });

    </script>
    <script type="text/javascript">

        window.onload = maxWindow;

        function maxWindow() {
            window.moveTo(0, 0);


            if (document.all) {
                top.window.resizeTo(screen.availWidth, screen.availHeight);
            }

            else if (document.layers || document.getElementById) {
                if (top.window.outerHeight < screen.availHeight || top.window.outerWidth < screen.availWidth) {
                    top.window.outerHeight = screen.availHeight;
                    top.window.outerWidth = screen.availWidth;
                }
            }
        }
</script>
</head>

<body>
    <form id="form1" runat="server">
        <div id="container">
            <div id="logo">
                <img src="images/logo.jpg" width="297" height="76" align="middle">
            </div>
            <div id="login-panel">
                <div id="Logindiv" runat="server" visible="true">
                    <ul class="loginul">
                        <li class="login">
                            <asp:TextBox ID="txtUserName" runat="server" value="User Name" onBlur="if(this.value=='') this.value='User Name'" autocomplete="off" onFocus="if(this.value =='User Name' ) this.value=''" class="user-textbox"></asp:TextBox>
                        </li>
                        <li class="login">
                            <asp:TextBox ID="txtPassword" runat="server" value="Password" onBlur="if(this.value=='') this.value='Password'" autocomplete="off"  onFocus="if(this.value =='Password' ) this.value=''" type="password" class="pwd-textbox"></asp:TextBox>
                        </li>
                        <li>
                            <div id="tdMessage" runat="server" style="color: white; font-weight: bold;"></div>
                        </li>
                        <li class="button-panel">
                            <asp:LinkButton ID="lnkLogin" CssClass="login-button" Text="" runat="server" OnClick="lnkLogin_Click"></asp:LinkButton></li>
                       <%-- <li class="forgot-panel">Forgot your password? <a href="#">Click here</a></li>--%>
                    </ul>
                </div>
                <div id="Classdiv" runat="server" visible="false">
                    <table width="100%" cellpadding="0" cellspacing="0" style="text-align: left;">
                        <tr>
                            <td style="height: 30px"></td>
                        </tr>
                                            
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkBtnAdmin"  CssClass="adminLimkBtn"  runat="server" OnClick="lnkBtnAdmin_Click1"  ></asp:LinkButton>
                            </td>
                        </tr>  
                        <tr>
                            <td>
                                <asp:DropDownList ID="DdlClass" runat="server" Width="300px" CssClass="newDDL" ></asp:DropDownList></td>
                        </tr> 
                         <tr>
                            <td>

                            </td>
                        </tr>  
                        <tr>
                            <td style="text-align:center;padding:0px 0px 0px 140px">
                                <asp:ImageButton ID="imgLoginSel" runat="server" style="outline:none" OnClick="imgLoginSel_Click" ImageUrl="~/Administration/images/lgn.PNG"  />
                                
                            </td>
                        </tr>                       
                                              
                        <tr>
                            <td style="text-align: center">
                                <asp:Label ID="LblError" runat="server" Text="" ForeColor="White" Font-Bold="true"></asp:Label></td>
                        </tr>
                    </table>
                </div>

            </div>



        </div>



    </form>
</body>
</html>
