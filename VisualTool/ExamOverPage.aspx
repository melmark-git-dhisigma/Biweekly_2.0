<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExamOverPage.aspx.cs" Inherits="VisualTool_ExamOverPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Untitled Document</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            border: 0;
        }

        #maincontainer {
            width: 1000px;
            min-width: 900px;
            min-height: 600px;
            height: auto !important;
            height: 600px;
            margin: 0 auto;
            padding: 0;
        }

            #maincontainer div.logo {
                width: 301px;
                height: 48px;
                margin: 100px auto 0 auto;
            }

                #maincontainer div.logo img {
                    margin: 0;
                    padding: 0;
                    border: 0;
                }

            #maincontainer h1 {
                font-family: Arial, Helvetica, sans-serif;
                font-size: 62px;
                text-align: center;
                color: #424242;
                text-decoration: none;
                font-weight: normal;
                display: block;
                margin: 75px 0 0 0;
                display: block;
            }

            #maincontainer h3 {
                font-family: Arial, Helvetica, sans-serif;
                font-size: 16px;
                text-align: center;
                color: #424242;
                text-decoration: none;
                font-weight: normal;
                display: block;
                margin: 15px 0 0 0;
                display: block;
            }

            #maincontainer h5.stud {
                font-family: Arial, Helvetica, sans-serif;
                font-size: 12px;
                text-align: left;
                color: #424242;
                text-decoration: none;
                font-weight: bold;
                letter-spacing: 1px;
                background: url(images/boy.PNG) left top no-repeat;
                width: 300px;
                margin: 35px auto 0 auto;
                padding: 0 0 0 25px;
            }

            #maincontainer hr {
                border: 1px solid #e6e2e2;
            }

            #maincontainer h5.id {
                font-family: Arial, Helvetica, sans-serif;
                font-size: 12px;
                text-align: left;
                color: #424242;
                text-decoration: none;
                font-weight: bold;
                letter-spacing: 1px;
                background: url(images/id.PNG) left top no-repeat;
                width: 300px;
                margin: 5px auto 0 auto;
                padding: 0 0 0 25px;
            }

            #maincontainer hr {
                border: 1px solid #e6e2e2;
            }

            #maincontainer a, #maincontainer a:link, #maincontainer a:visited {
                display: block;
                margin: 50px auto 0 auto;
                width: 197px;
                height: 46px;
                background: url(images/logout.png) left top no-repeat;
            }

                #maincontainer a:hover {
                    background-position: 0 -61px;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="maincontainer">
            <div class="logo">
                <img src="images/mellogo.JPG" width="301" height="78" /></div>
            <br clear="all" />
            <h1>Congratulations !! </h1>
            <hr />
            <h3>You are successfully completed your exam</h3>
            <h5 class="stud">Name of  Student  : <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label> </h5>
            <h5 class="id">Class : <asp:Label ID="lblClassName" runat="server" Text=""></asp:Label></h5>


            <a href="../Logout.aspx"></a>

        </div>

        </div>
    </form>
</body>
</html>
