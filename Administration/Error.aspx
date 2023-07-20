<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Admin_Error1" %>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="imagetoolbar" content="no" />
    <link rel="icon" type="image/x-icon" href="/images/favicon.ico" />
    <link rel="shortcut icon" type="image/x-icon" href="" />
    <meta name="robots" content="noindex,nofollow" />
    <title>Memark Error Page</title>

    <style>
        div.errorPage {
            width: 860px;
            height: 420px;
            margin: 80px auto 0 auto;
            border-radius: 15px;
            background: #f1f8f6;
            border: 1px solid #CCC;
            box-shadow: 1px 1px 2px 2px #888888;
        }

            div.errorPage img.iconimg {
                float: left;
                margin: 130px 40px 0 50px;
                padding: 0;
            }

            div.errorPage img.line {
                float: left;
            }

            div.errorPage img.smllgo {
                float: right;
                margin: 0 15px 0 0;
            height: 14px;
        }

            div.errorPage ul {
                width: 450px;
                height: 150px;
                float: right;
                display: block;
                padding: 0;
                font-family: Arial, Helvetica, sans-serif;
                color: #2f47a5;
                font-size: 18px;
                line-height: 30px;
                list-style: none;
            }

        .head {
            width: 480px;
            margin: 100px 0 0 0;
            float: right;
            padding: 5px 0 0 10px;
            ;
            font-family: Arial, Helvetica, sans-serif;
            color: #F00;
            font-size: 18px;
        }

        div.errorPage ul li {
            list-style: url(images/bldot.JPG);
            letter-spacing: -1px;
        }

            div.errorPage ul li.last {
                list-style: none !important;
                font-size: 14px;
                color: #097c5e;
                margin: 60px 0 0 0;
            }

                div.errorPage ul li.last a,
                div.errorPage ul li.lasta :link,
                div.errorPage ul li.last a:visited {
                    font-weight: bold;
                    color: #2f47a5;
                    text-decoration: none;
                }

                    div.errorPage ul li.last a:hover {
                        color: #F60;
                    }

        .footer {
            font-size: 14px;
            color: #097c5e;
            width: 480px;
            float: right;
        }
    </style>

</head>

<body class="login">
    <div class="errorPage">
<img src="images/messengererror.JPG" width="233" class="iconimg" height="185" />
<img src="images/line.JPG" width="3" height="420" class="line" /> 

<div class="head" ><font color="#2f47a5" >
    <br />
    Perhaps you here because : 
    <br />
    </font> </div>

<ul>

	<li>The page you were looking for could not be found.</li>
    <li>No permission.</li>
    <li>Your session has expired. </li>
    
</ul>
<div class="footer" >Please Contact Programme Administrator. Return to the <a href="../../../../Login.aspx">Login</a> page.</div>
<img src="images/melmark-logo.png" width="88" class="smllgo" /> </div>
</body>
</html>
