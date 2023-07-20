<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Admin_Error" %>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
  <meta http-equiv="imagetoolbar" content="no" />
  <link rel="icon" type="image/x-icon" href="/images/favicon.ico" />
  <link rel="shortcut icon" type="image/x-icon" href="" />
  <meta name="robots" content="noindex,nofollow" />
  <title>Memark Error Page</title>

		<style>
		body {background: #FFFFFF url(images/ErrorBg.jpg) repeat-x;  margin: 0; padding: 20px; text-align:center; font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#666666;}
		.error_page {width: auto; padding: 50px; margin: auto;}
		.error_page h1 {margin: 20px 0 0;}
		.error_page p {margin: 10px 0; padding: 0;}		
		a {color: #FF0000; text-decoration:none;}
		a:hover {color: #9caa6d; text-decoration:underline;}
		</style>

</head>

<body class="login">
  <div class="error_page">
    <img alt="Error" src="images/ErrorImage.gif" />
    <h1 style="color: #0099CC">We're sorry...</h1>
    <p id="pError" runat="server" 
          style="color: #003399; font-weight: bold; font-family: 'Browallia New'; font-size: x-large;"></p>
    <p><a href="" style="color: #FF0000">Return to the Login Page</a></p>

   
  </div>
</body>
</html>
