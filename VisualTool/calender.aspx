<%@ Page Language="VB" AutoEventWireup="false" CodeFile="calender.aspx.vb" Inherits="calender" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en">
<head>
	<meta charset="utf-8">
	<title>jQuery UI Datepicker - Default functionality</title>
	<link rel="stylesheet" href="calenderItems/jquery.ui.all.css">
	<script src="calenderItems/jquery-1.8.0.js"></script>
	<script src="calenderItems/jquery.ui.core.js"></script>
	<script src="calenderItems/jquery.ui.widget.js"></script>
	<script src="calenderItems/jquery.ui.datepicker.js"></script>
	<link rel="stylesheet" href="calenderItems/demos.css">
    <meta http-equiv="X-UA-Compatible" content="IE=10,9" />
	<script>
	    $(function () {
	        $("#datepicker").datepicker();
	        x = 5 + 2;
	    });
	</script>
</head>
<body>

<div class="demo">

<p>Date: <input type="text" id="datepicker"></p>

</div><!-- End demo -->



<div class="demo-description">
<p>The datepicker is tied to a standard form input field.  Focus on the input (click, or use the tab key) to open an interactive calendar in a small overlay.  Choose a date, click elsewhere on the page (blur the input), or hit the Esc key to close. If a date is chosen, feedback is shown as the input's value.</p>
</div><!-- End demo-description -->

</body>
</html>
