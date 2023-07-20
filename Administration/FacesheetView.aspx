<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FacesheetView.aspx.cs" Inherits="Administration_FacesheetView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    <style>
        body {
            border: 0 none;
            color: #6B6B6B;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            margin: 0;
            padding: 0;
            width: 100%;
            text-transform: none;
        }

        .clear {
            border: 0 none;
            clear: both;
            margin: 0;
            padding: 0;
        }

        div.mainContainer {
            height: auto !important;
            min-height: 500px;
            width: 100%;
        }

            div.mainContainer div.topHead {
                background: url("../images/melmarkTop.jpg") repeat-y scroll left top #00539F;
                height: 47px;
                width: 100%;
                min-width: 800px;
            }

        div.imgcorner {
            height: 100px;
            margin: 0;
            padding: 0;
            min-width: 600px;
            width: 100%;
        }

            div.imgcorner a.logo,
            div.imgcorner a.logo:link,
            div.imgcorner a.logo:visited {
                border: 0;
                margin: 15px 0 0 25px;
                display: block;
                float: left;
                width: 2220px;
                height: 58px;
                display: block;
            }

                div.imgcorner a.logo img {
                    border: 0;
                }

        div.ContentAreaContainer {
            background: #FFFFFF;
            height: auto !important;
            margin: 0 auto;
            min-height: 500px;
            min-width: 600px;
            width: 100%;
            z-index: 999;
        }

            div.ContentAreaContainer img.logo {
                display: block;
                float: left;
                margin: 10px 0 10px 10px;
            }

            div.ContentAreaContainer div.titleContainer {
                width: 250px;
                float: right;
                min-height: 50px;
                min-width: 250px;
                margin: 10px 10px 10px 0;
            }

                div.ContentAreaContainer div.titleContainer h2.ri {
                    font-family: Arial, Helvetica, sans-serif;
                    font-size: 16px !important;
                    color: #076859;
                    text-align: left;
                    text-decoration: none;
                    margin: 10px 0 5px 0 !important;
                    background: none;
                    border: none;
                    text-align: left;
                    padding: 0;
                }

                div.ContentAreaContainer div.titleContainer h3.ri {
                    background: none;
                    font-family: Arial, Helvetica, sans-serif;
                    font-size: 14px !important;
                    color: #666;
                    text-align: left;
                    text-decoration: none;
                    text-align: left;
                    padding: 0 !important;
                    margin: 0;
                }



            div.ContentAreaContainer div.middleContainer {
                float: left;
                height: auto !important;
                margin: 0 auto;
                min-height: 500px;
                min-width: 550px;
                width: 98%;
            }


            div.ContentAreaContainer h2 {
                border-bottom: 1px solid #f2f2f2;
                border-top: 1px solid #f2f2f2;
                font-size: 12px;
                font-weight: bold;
                margin: 10px auto 10px auto;
                color: #006351;
                padding: 4px 0 5px 28px;
                text-align: left;
                width: 96%;
                text-transform: none;
                letter-spacing: 1px;
            }

            div.ContentAreaContainer h3 {
                background: url("../images/aro.PNG") no-repeat scroll 8px 4px rgba(0, 0, 0, 0);
                color: #666666;
                font-size: 12px;
                font-weight: bold;
                margin: 0 0 10px 12px;
                padding: 4px 0 5px 28px;
                text-align: left;
                width: 98%;
            }

            div.ContentAreaContainer h4 {
                color: #e23d29;
                font-size: 12px;
                font-weight: bold;
                margin: 0 0 10px 12px;
                padding: 4px 0 5px 15px;
                text-align: left;
                width: 98%;
            }

            div.ContentAreaContainer p {
                font-size: 12px;
                font-weight: normal;
                margin: 10px auto 10px;
                color: #666;
                padding: 4px 0 5px 15px;
                text-align: justify;
                width: 96%;
                line-height: 18px;
            }

            div.ContentAreaContainer table {
                margin: 0 auto !important;
                padding: 0;
                width: 98%;
                min-width: 600px;
            }

                div.ContentAreaContainer table.shot {
                    width: 60%;
                    margin: 0 auto;
                }

                div.ContentAreaContainer table.medium {
                    width: 85%;
                    margin: 0 auto;
                }

                div.ContentAreaContainer table td {
                    border-bottom: 1px solid #EDEDED;
                    border-left: 1px solid #EDEDED;
                    font-size: 11px;
                    font-weight: bold;
                    padding: 5px 1%;
                    min-width: 95px;
                }

                    div.ContentAreaContainer table td span {
                        text-align: right !important;
                        float: right;
                    }

                    div.ContentAreaContainer table td.top {
                        border-top: 1px solid #ededed;
                    }

                    div.ContentAreaContainer table td.bg {
                        background: #f9f9f9;
                        height: 20px;
                    }

                    div.ContentAreaContainer table td.righ {
                        border-right: 1px solid #ededed;
                    }

                    div.ContentAreaContainer table td img.im {
                        margin: 5px auto !important;
                        text-align: center;
                        border: 3px solid #ededed;
                        display: block;
                    }


                    div.ContentAreaContainer table td input.chkbx {
                        margin: 3px 0 0 0;
                        padding: 0;
                        float: left;
                    }

                    div.ContentAreaContainer table td select.dob {
                        background-color: #FFFFFF;
                        border: 1px solid #DADAC8;
                        border-radius: 3px 3px 3px 3px;
                        color: #666666;
                        font-size: 11px;
                        height: 25px !important;
                        padding: 4px 4px 2px;
                        margin: 0 0 0 5px;
                        min-width: 15px;
                        width: 25%;
                    }

                    div.ContentAreaContainer table td select.selbox {
                        background-color: #FFFFFF;
                        border: 1px solid #DADAC8;
                        border-radius: 3px 3px 3px 3px;
                        color: #666666;
                        font-size: 11px;
                        height: 25px !important;
                        padding: 4px 4px 2px;
                        width: 100% !important;
                        margin: 0 0 0 5px;
                    }

                    div.ContentAreaContainer table td textarea.txarea {
                        background-color: #FFFFFF;
                        border: 1px solid #DADAC8;
                        border-radius: 3px 3px 3px 3px;
                        color: #666666;
                        float: left;
                        font-size: 11px;
                        height: 65px;
                        margin: 0 0 0 5px;
                        min-width: 72px !important;
                        padding: 2px 4px;
                        width: 92%;
                    }



                div.ContentAreaContainer table.bdrtble {
                    border: 1px solid #EDEDED;
                    border-bottom: none;
                    border-left: 0;
                    width: 95%;
                    margin: 0 auto !important;
                    min-width: 250px;
                }

                    div.ContentAreaContainer table.bdrtble tr th {
                        background: #91bfbd;
                        color: #000;
                        text-align: left;
                        padding: 5px 5px;
                    }

                div.ContentAreaContainer table.nobder {
                    border: 0;
                }

                div.ContentAreaContainer table bdrtble tr td {
                    border: 0 !important;
                }

                div.ContentAreaContainer table.bdrtble tr td h4.title {
                    font-size: 12px;
                    color: #666;
                    margin: 0 0 0 0;
                    padding: 0;
                    float: left;
                    font-weight: bold !important;
                    text-align: left;
                }



                div.ContentAreaContainer table.bdrtble input.textfield {
                    background-color: #FFFFFF;
                    border: 1px solid #DADAC8;
                    border-radius: 3px 3px 3px 3px;
                    color: #666666;
                    float: left;
                    font-size: 11px;
                    height: 19px;
                    margin: 0 0 0 6px;
                    min-width: 72px !important;
                    padding: 2px 4px;
                    width: 92%;
                }

                div.ContentAreaContainer table.bdrtble td label.title {
                    margin: 0 0 0 5px;
                    padding: 3px 0 5px 0;
                    display: block;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <iframe runat="server"  id="pageFrame1" width="100%" height="3600px" frameborder="0"></iframe>
    </form>
</body>
</html>
