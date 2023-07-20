<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LessonPlanNew.aspx.cs" Inherits="StudentBinder_LessonPlanNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Untitled Document</title>
    <script src="../Administration/JS/jquery-1.8.0.js"></script>
    <script src="jsScripts/jq1.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript" src="jsScripts/eye.js"></script>
    <script type="text/javascript" src="jsScripts/layout.js"></script>
    <link href="../Administration/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Administration/CSS/popupStyle1.css" rel="stylesheet" />


    <style>
        .clear {
            margin: 0;
            padding: 0;
            border: 0;
            clear: both;
        }

        .border {
            border-right: thin solid #CCCCCC;
        }

        .borderLeft {
            border-left: thin solid #CCCCCC;
        }

        .boxContainerContainer {
            width: 94%;
            min-height: 500px;
            height: auto !important;
            height: 500px;
            margin: 0 auto;
            font-family: Arial, Helvetica, sans-serif;
        }

            .boxContainerContainer div.itlepartContainer {
                width: 100%;
                height: 35px;
                margin: 0;
                padding: 0;
            }

                .boxContainerContainer div.itlepartContainer h2 {
                    background: url(img/bluerdesign.png) left top no-repeat;
                    color: #fff;
                    font-size: 12px;
                    font-weight: bold;
                    height: 23px;
                    padding: 8px 0 5px 12px;
                    margin: 0 100px 0 0;
                    float: left;
                }

                .boxContainerContainer div.itlepartContainer h3.cf {
                    color: #0d668e;
                    font-size: 12px;
                    font-weight: bold;
                    height: 23px;
                    padding: 8px 0 5px 12px;
                    margin: 0 10px 0 0;
                    float: right;
                }


                .boxContainerContainer div.itlepartContainer h2 span {
                    margin: 0 0 0 20px;
                    color: #0d668e;
                }

            .boxContainerContainer div.btContainerPart {
                width: 100%;
                margin: 5px auto 0 auto;
                min-height: 200px;
                height: auto !important;
                height: 200px;
            }

                .boxContainerContainer div.btContainerPart div.lBxpartContainer {
                    width: 18%;
                    min-height: 300px;
                    height: auto !important;
                    height: 300px;
                    float: left;
                    padding: 10px;
                    background: #f7f5f5;
                    border: 5px solid #e4e4e4;
                    margin: 0 10px 0 0;
                }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer h3 {
                        font-size: 12px;
                        font-weight: bold;
                        color: #0d668e;
                        margin: 5px 0 5px 0;
                    }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer input.radio {
                        float: left !important;
                        margin: 5px 0 0 0;
                        padding: 0;
                        width: 20px;
                        height: 20px;
                        display: block;
                    }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer a.lpb {
                        background: url(img/greenbtn.png) left top no-repeat;
                        width: 176px;
                        height: 22px;
                        font-size: 12px;
                        font-weight: normal;
                        color: #444545;
                        background-position: 0 -0;
                        display: block;
                        text-decoration: none;
                        padding: 6px 0 0 25px;
                        float: right;
                        margin: 0 0 3px 0;
                    }

                        .boxContainerContainer div.btContainerPart div.lBxpartContainer a.lpb span {
                            font-size: 11px;
                            font-weight: bold;
                        }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer a.grb {
                        background: url(img/graybtn.png) left top no-repeat;
                        width: 176px;
                        height: 23px;
                        font-size: 12px;
                        font-weight: normal;
                        color: #444545;
                        background-position: 0 -0;
                        display: block;
                        text-decoration: none;
                        padding: 6px 0 0 25px;
                        float: right;
                        margin: 0 0 3px 0;
                    }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer input.txtfld {
                        border-radius: 3px;
                        border: 1px solid #c8cfcf;
                        margin: 0 0 0 44px;
                        height: 24px;
                        color: #666;
                        font-size: 12px;
                        width: 160px;
                        padding: 0 3px;
                    }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer input.btn {
                        width: 27px;
                        height: 28px;
                        float: left;
                        background: url(img/zoomlens.png) left top no-repeat;
                        cursor: pointer;
                        background-position: 0 -0;
                        float: right !important;
                        border: none;
                        margin: -1px 0 0 0;
                    }

                        .boxContainerContainer div.btContainerPart div.lBxpartContainer input.btn:hover {
                            background-position: 0 -31px;
                        }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer p {
                        font-size: 12px;
                        color: #505050;
                        margin: 5px 0 5px 42px;
                    }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer input.smltxtfld {
                        border-radius: 3px;
                        border: 1px solid #c8cfcf;
                        margin: 0 5px 0 44px;
                        height: 24px;
                        color: #666;
                        font-size: 12px;
                        width: 67px;
                        padding: 0 3px;
                        float: left;
                    }

                    .boxContainerContainer div.btContainerPart div.lBxpartContainer input.smltxt {
                        border-radius: 3px;
                        border: 1px solid #c8cfcf;
                        margin: 0 5px 0 5px;
                        height: 24px;
                        color: #666;
                        font-size: 12px;
                        width: 70px;
                        padding: 0 3px;
                        float: left;
                    }

                .boxContainerContainer div.btContainerPart div.mBxContainer {
                    width: 20%;
                    height: 530px;
                    float: left !important;
                    padding: 10px;
                    background: #f7f5f5;
                    border: 5px solid #e4e4e4;
                    margin: 0 10px 0 0;
                    overflow-y: scroll;
                    overflow-x: hidden;
                }

                    .boxContainerContainer div.btContainerPart div.mBxContainer h3 {
                        font-size: 12px;
                        font-weight: bold;
                        color: #0d668e;
                        margin: 5px 0 5px 0;
                    }

                    .boxContainerContainer div.btContainerPart div.mBxContainer a.gmb {
                        background: url(img/lbtngrn.png) left top no-repeat;
                        width: 200px;
                        height: 22px;
                        font-size: 12px;
                        font-weight: normal;
                        color: #444545;
                        background-position: 0 -0;
                        display: block;
                        text-decoration: none;
                        padding: 7px 0 0 25px;
                        float: left;
                        margin: 0 0 3px 0;
                    }

                    .boxContainerContainer div.btContainerPart div.mBxContainer a.grbmb {
                        background: url(img/lbtngrngray.png) left top no-repeat;
                        width: 200px;
                        height: 22px;
                        font-size: 12px;
                        font-weight: normal;
                        color: #444545;
                        background-position: 0 -0;
                        display: block;
                        text-decoration: none;
                        padding: 6px 0 0 25px;
                        float: left;
                        margin: 0 0 3px 0;
                    }

                    .boxContainerContainer div.btContainerPart div.mBxContainer a.grmb {
                        background: url(img/lbtngrngray.png) left top no-repeat;
                        width: 200px;
                        height: 22px;
                        font-size: 11px;
                        font-weight: normal;
                        color: #0d668e;
                        background-position: 0 -0;
                        display: block;
                        text-decoration: none;
                        padding: 6px 0 0 25px;
                        float: left;
                        margin: 0 0 3px 0;
                    }

                    .boxContainerContainer div.btContainerPart div.mBxContainer input.butn {
                        width: 220px;
                        height: 34px;
                        background: url(img/blbtn.png) left top no-repeat;
                        color: #fff;
                        font-weight: bold;
                        font-size: 11px;
                        border: none;
                        cursor: pointer;
                    }

                .boxContainerContainer div.btContainerPart div.righMainContainer {
                    width: 55%;
                    min-height: 543px;
                    height: auto !important;
                    height: 300px;
                    float: right !important;
                    padding: 4px;
                    background: #fff;
                    border: 5px solid #e4e4e4;
                    margin: 0 0px 0 0;
                }

                    .boxContainerContainer div.btContainerPart div.righMainContainer h3 {
                        font-size: 12px;
                        font-weight: bold;
                        color: #0d668e;
                        margin: 5px 0 5px 0;
                    }


        /*----------------------------------------------------------- JQuery Start ------------------------------------------------------*/

        h1, h2, h3, h4, h5, h6, p, ul, li {
            margin: 0;
            padding: 0;
            border: 0;
            vertical-align: baseline;
            list-style: none;
        }

            h2 a {
                padding: 7px 7px 7px 20px;
            }

            h3 a {
                padding: 7px 7px 7px 30px;
            }

            h2 a, h3, a {
                display: block;
                text-decoration: none;
            }

            .accordion li, li.accordion {
                border: none;
            }

        .accordion h2 a, .accordion h3 a {
            text-decoration: none;
            background: url(img/computer.png) 5px 6px no-repeat !important;
            z-index: 999;
            padding: 9px 0 2px 30px;
            font-size: 11px;
            font-weight: bold;
            color: #1f1f1f;
            width: 48%;
            float: left;
        }

            .accordion h2 a.kk, .accordion h3 a.kk {
                text-decoration: none;
                background: none !important;
                z-index: 999;
                padding: 2px 0 6px 0;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                width: 78%;
                float: left;
                border: none;
                text-align: left;
                cursor: pointer;
            }

        .accordion h2 span {
            cursor: pointer;
        }

        .accordion .wrapper {
            padding: 5px;
            line-height: 18px;
            width: 92%;
        }

        .accordion .nomar {
            padding: 0px;
            width: 100%;
        }

        .accordion h2,
        .accordion h3 {
            height: 31px;
            background: url(img/longgreenbar.png) right top no-repeat;
            font-size: 12px;
            font-weight: bold;
            color: #000;
            margin: 0 0 2px 0;
            padding: 0;
            width: 100%;
        }

            .accordion h2 span.dd,
            .accordion h3 span.dd {
                width: 4px;
                height: 31px;
                float: left;
                margin: 0;
                padding: 0;
                background: url(img/llbar.jpg) left top no-repeat;
                display: block;
            }

            .accordion h2.BG,
            .accordion h3.BG {
                height: 23px;
                background: url(img/dwngrenbg.png) left top no-repeat;
                width: 230px;
                font-size: 12px;
                font-weight: normal;
                color: #444545;
                margin: 0 0 3px 0;
                padding: 6px 0 0 25px;
                border: none;
                background-position: 0 -0;
            }

                .accordion h2.BG:hover,
                .accordion h3.BG:hover {
                    background-position: 0 -32px;
                    color: #000 !important;
                }



            .accordion h2 div.container, .accordion h3 div.container {
                float: right;
                width: 35%;
                height: 20px;
                margin: 9px 0 0 0;
                padding: 0;
                background: none;
            }

                .accordion h2 div.container span, .accordion h3 div.container span {
                    float: left;
                    font-size: 12px;
                    font-weight: bold;
                    color: #1f1f1f;
                    display: block;
                    z-index: 999;
                    margin: 0 0 0 0;
                    width: 180px;
                }

                    .accordion h2 div.container span input.rdo, .accordion h3 div.container span input.rdo {
                        float: left !important;
                        display: block;
                        width: 20px;
                        height: 20px;
                        margin: -2px 10px 0 0;
                        margin: -6px 10px 0 0 \9;
                    }

                .accordion h2 div.container a.dlt, .accordion h3 div.container a.dlt {
                    float: right;
                    width: 20px;
                    height: 17px;
                    display: block;
                    margin: 0 15px 0 0;
                    padding: 0;
                    background: url(img/delet0_2.png) left top no-repeat !important;
                    background-position: 0 -0;
                    border-bottom: 0;
                }

                    .accordion h2 div.container a.dlt:hover, .accordion h3 div.container a.dlt:hover {
                        background-position: 0 -21px !important;
                    }


        .accordion .wrapper div.ingrContainer {
            border-radius: 5px;
            margin: 5px 0 0 0;
            min-height: 75px;
            height: auto !important;
            height: 75px;
            width: 104%;
            border: 1px solid #a1a1a1;
            background: #efefef;
            padding: 10px;
            font-size: 12px;
            font-weight: normal;
            color: #1f1f1f;
        }


        .accordion .wrapper div.nobdrrcontainer {
            margin: 1px 0 0 0;
            min-height: 25px;
            height: auto !important;
            height: 25px;
            width: 99%;
            font-size: 12px;
            font-weight: normal;
            color: #1f1f1f;
        }

        .accordion .wrapper div.ingrContainer h3 {
            font-size: 12px;
            color: #0d668e;
            font-weight: bold;
            float: left;
            background: none;
            margin: 0;
            padding: 0;
            width: 60%;
            float: left;
        }

        .accordion .wrapper div.ingrContainer p {
            font-size: 12px;
            font-weight: normal;
            color: #1f1f1f;
        }

        .accordion .wrapper div.ingrContainer div.container {
            float: right;
            width: 35%;
            height: 20px;
            margin: 0;
            padding: 0;
        }

            .accordion .wrapper div.ingrContainer div.container span {
                float: left;
                font-size: 12px;
                font-weight: bold;
                color: #1f1f1f;
                display: block;
                margin: 0 0 0 0;
                width: 180px;
            }

                .accordion .wrapper div.ingrContainer div.container span input.rdo {
                    float: left !important;
                    display: block;
                    width: 20px;
                    height: 20px;
                    margin: -2px 10px 0 0;
                    margin: -6px 10px 0 0 \9;
                }


            .accordion .wrapper div.ingrContainer div.container a.dlt {
                float: right;
                width: 20px;
                height: 17px;
                display: block;
                margin: 0 15px 0 0;
                padding: 0;
                background: url(img/delet0_2.png) left top no-repeat;
                background-position: 0 -0;
                border-bottom: 0;
            }

                .accordion .wrapper div.ingrContainer div.container a.dlt:hover {
                    background-position: 0 -21px;
                }





        .boxContainerContainer div.btContainerPart div.mBxContainer a.grmb div.grapgContainer {
            width: 50px;
            height: 18px;
            border: 1px solid #9ba1a1;
            border-radius: 3px;
            float: right;
            margin: -2px 28px 0 0;
            font-size: 9px;
            font-weight: normal;
            color: #0d668e;
        }

            .boxContainerContainer div.btContainerPart div.mBxContainer a.grmb div.grapgContainer span {
                width: 16px;
                height: 16px;
                margin: 0 0 0 3px;
                float: left;
                background: url(img/loder.png) left top no-repeat;
                background-position: -21px 0;
            }

            .boxContainerContainer div.btContainerPart div.mBxContainer a.grmb div.grapgContainer p {
                margin: 2px 0 0 22px;
                margin: 5px 0 0 22px\9;
            }

        .boxContainerContainer div.btContainerPart div.large {
            width: 75%;
            float: right !important;
        }
        /*-----------------------------------------------------------JQuery End-------------------------------------------*/


        /*-----------------------------------------------Second Page Start--------------------------------------------------*/
        fieldset, img {
            border: 0;
        }

        address, caption, cite, code, dfn, em, strong, th, var {
            font-style: normal;
            font-weight: normal;
        }

        ol, ul {
            list-style: none;
            width: 100%;
        }

        caption, th {
            text-align: left;
        }

        abbr, acronym {
            border: 0;
        }

        .topper {
            width: 100%;
            margin: 0 auto;
            text-align: left;
            background: #eeeeee;
        }

        h1 {
            font-size: 21px;
            height: 43px;
            line-height: 47px;
            text-transform: uppercase;
        }

        .navigationTabs {
            height: 35px;
            line-height: 23px;
            background: url(img/blrbg.jpg) right top no-repeat;
            margin: 0;
            padding: 0;
        }

            .navigationTabs span.bgb {
                margin: 0;
                padding: 0;
                background: url(img/lblbtn.png) left top no-repeat;
                float: left;
                display: block;
                width: 7px;
                height: 35px;
            }

            .navigationTabs li {
                float: left;
                height: 35px;
                line-height: 23px;
                padding-right: 0px;
                background: url(img/linecut.jpg) right top no-repeat;
            }

                .navigationTabs li.nobg {
                    float: left;
                    height: 35px;
                    line-height: 23px;
                    padding-right: 0px;
                    background: none;
                }

                .navigationTabs li a {
                    float: left;
                    dispaly: block;
                    height: 28px;
                    line-height: 23px;
                    padding: 7px 11px 0 11px;
                    overflow: hidden;
                    width: 115px;
                    margin: 0 1px;
                    color: #fff;
                    position: relative;
                    text-decoration: none;
                    font-family: Arial, Helvetica, sans-serif;
                    font-size: 11px;
                    text-align: center;
                    font-weight: bold;
                    background: url(img/warrow.png) center bottom no-repeat;
                }

                    .navigationTabs li a:hover {
                        background: url(img/hoverbggreen.jpg) left top repeat-x;
                        background-color: transparent !important;
                        border-bottom: none;
                        color: #000 !important;
                    }

                    .navigationTabs li a.active {
                        background: #86b2c6;
                        border-bottom: none;
                        color: #0d668e;
                    }

        .tabsContent {
            border-top: 0px solid;
            width: 100%;
            overflow: hidden;
            padding: 0 -2px !important;
        }

        .tab {
            padding: 10px;
            display: none;
            min-height: 485px;
            height: auto !important;
        }

            .tab h2 {
                font-size: 14px;
                font-weight: bold;
                color: #0d668e !important;
                margin: 5px 0 5px 0;
                letter-spacing: 1px;
            }

            .tab h3 {
                font-weight: bold;
                font-size: 12px;
                margin-top: 20px;
                color: #514f4f !important;
            }

            .tab p {
                margin-top: 16px;
                clear: both;
            }

            .tab ul {
                margin-top: 16px;
                list-style: disc;
            }

            .tab li {
                margin: 10px 0 0 35px;
            }

            .tab a {
                color: #8FB0CF;
            }

            .tab strong {
                font-weight: bold;
            }

            .tab pre {
                font-size: 11px;
                margin-top: 20px;
                width: 668px;
                overflow: auto;
                clear: both;
            }

            .tab table {
                width: 100%;
            }

                .tab table td {
                    padding: 0px 10px 0px 0;
                    vertical-align: middle;
                }

                    .tab table td input.rdoo {
                        width: 16px;
                        height: 16px;
                        float: left;
                        margin: 10px 5px 0 0;
                        margin: 6px 5px 0 0 \9;
                        border: none;
                        display: block;
                    }

        /*.tab table td label {
                        font-size: 12px;
                        font-weight: normal;
                        color: #514f4f;
                        margin: 0 0 0 0;
                        padding: 10px 0 0 0;
                        /*display: block;*/
        }

        */ .tab table td input.tx {
            border-radius: 3px;
            border: 1px solid #b2b2b2;
            width: 300px;
            height: 23px;
            float: left;
            background: #f5f5f5;
            margin: 5px 0 0 0;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            text-align: left;
            color: #666;
        }

        .tab table td textarea.tarea {
            border-radius: 5px;
            border: 1px solid #b2b2b2;
            width: 300px;
            height: 75px;
            float: left;
            background: #f5f5f5;
            margin: 5px 0 0 0;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            text-align: left;
            color: #666;
        }

        .tab table td input.bTnn {
            background: url(img/btnbg.PNG) left top no-repeat;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: #fff;
            font-weight: bold;
            margin: 5px 0 0 0;
            padding: 0;
            border: 0;
            width: 97px;
            height: 29px;
            cursor: pointer;
        }

            .tab table td input.bTnn:hover {
                color: #000;
            }

        /*-----------------------------------------------Second Page End--------------------------------------------------*/



        /*-----------------------------------------------Third Page Start--------------------------------------------------*/
        .tab div.itomesContainer {
            width: 96%;
            min-height: 25px;
            height: auto !important;
            height: 25px;
            padding: 5px;
            margin: 0 10px;
            border: 5px solid #cccccc;
            background: #e1e1e1;
            text-align: left;
        }

        .tab table td p {
            font-size: 12px;
            font-weight: normal;
            color: #514f4f;
            margin: 0 0 0 0;
            padding: 5px 0 0 0;
            display: block;
            text-align: left;
            vertical-align:middle;
        }

        .tab table td a.smlbt {
            background: url(img/smlbtn.png) left top no-repeat;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: #fff;
            font-weight: bold;
            margin: 0 auto 0 auto;
            padding: 4px 0 0 0;
            border: 0;
            width: 69px;
            height: 20px;
            cursor: pointer;
            display: block;
            text-align: center !important;
        }

            .tab table td a.smlbt:hover {
                color: #000;
            }


        .tab table td input.smlbt {
            background: url(img/smlbtn.png) left top no-repeat;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: #fff;
            font-weight: bold;
            margin: 0 auto 0 auto;
            padding: 0px 0 0 0;
            border: 0;
            width: 69px;
            height: 24px;
            cursor: pointer;
            display: block;
            text-align: center !important;
        }

            .tab table td input.smlbt:hover {
                color: #000;
            }

        .tab input.rbtn {
            background: url(img/btnbg.PNG) left top no-repeat;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: #fff;
            font-weight: bold;
            margin: 5px 10px 5px 0;
            padding: 0;
            border: 0;
            width: 97px;
            height: 29px;
            float: right;
            cursor: pointer;
        }

            .tab input.rbtn:hover {
                color: #000;
            }

        .tab table td input.dx {
            border-radius: 3px;
            border: 1px solid #b2b2b2;
            width: 300px;
            height: 23px;
            float: left;
            background: #f5f5f5;
            margin: 0 0 0 0;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            text-align: left;
            color: #666;
        }

        .tab table td input.tp {
            margin: 0 0 5px -2px;
        }

        .tab table td input.ti {
            margin: 0 0 5px 0;
            margin: 0 0 5px 12px \9;
        }

        .tab table td input.rdo1 {
            width: 16px;
            height: 16px;
            float: left;
            margin: 10px 5px 0 3px;
            margin: 6px 5px 0 3px \9;
            border: none;
            display: block;
        }

        .tab hr {
            border: 1px dotted #0d668e;
            margin: 10px 0;
        }


        .tab table td a.smlRbt {
            background: url(img/smlbtn.png) left top no-repeat;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: #fff;
            font-weight: bold;
            margin: 5px auto 0 auto;
            padding: 4px 0 0 0;
            border: 0;
            width: 69px;
            height: 20px;
            cursor: pointer;
            display: block;
            text-align: center !important;
        }

            .tab table td a.smlRbt:hover {
                color: #000;
            }


        .tab table td input.smlRbt {
            background: url(img/smlbtn.png) left top no-repeat;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px;
            color: #fff;
            font-weight: bold;
            margin: 5px auto 0 auto;
            padding: 0px 0 0 0;
            border: 0;
            width: 69px;
            height: 25px;
            cursor: pointer;
            display: block;
            text-align: center !important;
        }

        .tab table td a.input:hover {
            color: #000;
        }


        /*-----------------------------------------------Third Page End--------------------------------------------------*/


        /*-----------------------------------------------Forth Page Tart--------------------------------------------------*/
        .tab textarea.tareaLong {
            border-radius: 5px;
            border: 1px solid #b2b2b2;
            width: 100%;
            height: 75px;
            float: left;
            background: #f5f5f5;
            margin: 5px auto 0 auto;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            text-align: left;
            color: #666;
        }
        /*-----------------------------------------------Forth Page End--------------------------------------------------*/


        /*-----------------------------------------------Fifth  Page Start--------------------------------------------------*/
        .tab select.txx {
            border-radius: 3px;
            border: 1px solid #b2b2b2;
            width: 250px;
            height: 23px;
            float: left;
            background: #f5f5f5;
            margin: 5px 0 0 0;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            text-align: left;
            color: #666;
        }

        .tab label.ii {
            font-size: 12px;
            font-weight: normal;
            color: #514f4f;
            margin: 0 0 0 0;
            padding: 10px 0 0 0;
            display: block;
            width: 430px;
            float: left;
        }

        .tab textarea.srBox {
            border-radius: 5px;
            border: 1px solid #b2b2b2;
            width: 100%;
            height: 110px !important;
            float: left;
            background: #f5f5f5;
            margin: 5px auto 0 auto;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            text-align: left;
            color: #666;
        }
        /*-----------------------------------------------Fifth  Page End--------------------------------------------------*/
        #AddSetDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 840px;
            min-height: 215px;
            height: auto !important;
        }

        #AddStepDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 905px;
            min-height: 215px;
            height: auto !important;
        }

        #AddMeasureDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 905px;
            min-height: 440px;
            height: auto !important;
        }


        #AddCriteriaDiv {
            background-color: #fff;
            padding: 5px;
            position: absolute;
            top: 10px;
            background-color: White;
            display: none;
            margin: auto;
            z-index: 10000;
            width: 974px;
            height: 270px;
        }


        .fullOverlay {
            display: none;
            top: 0px;
            left: 0px;
            position: fixed;
            z-index: 2000;
            width: 100%;
            height: 100%;
            background-image: url("../VisualTool/images/overlay.png");
        }

        #Close1 {
            display: block;
            height: 23px;
            overflow: hidden;
            position: absolute;
            right: 4px;
            top: 2px;
            width: 24px;
        }

        #close_Step {
            display: block;
            height: 23px;
            overflow: hidden;
            position: absolute;
            right: 4px;
            top: 2px;
            width: 24px;
        }

        #close_CriteriaPopup {
            display: block;
            height: 23px;
            overflow: hidden;
            position: absolute;
            right: 4px;
            top: 2px;
            width: 24px;
        }

        a {
            text-decoration: none;
        }


        .waitBlank {
            background-color: #FFFFFF;
            display: block;
            width: 10% !important;
            opacity: 0.6;
            margin: 0 auto !important;
            position: absolute;
            z-index: 1;
        }

            .waitBlank img {
                margin: 0 auto;
            }
    </style>


    <style>
        input.lbtn {
            width: 96px;
            height: 31px;
            background: url(img/longbtn.png) left top no-repeat;
            font-size: 0px;
            color: #333;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 11px !important;
            font-weight: bold;
            text-align: center;
            margin: 0 0 3px 0;
            padding: 0;
            border: 0;
            cursor: pointer;
            letter-spacing: 1px;
            background-position: 0 -40px;
            line-height: 5px;
        }

            input.lbtn:hover {
                background-position: 0 0px;
                color: #fff;
            }

        input.smllbtn {
            width: 31px;
            height: 31px;
            background: url(img/smallbtn.png) left top no-repeat;
            font-size: 0px;
            color: #333;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 14px !important;
            font-weight: bold;
            text-align: center;
            margin: 0 0 3px 0;
            padding: 0;
            border: 0;
            line-height: 0;
            cursor: pointer;
            letter-spacing: 1px;
            background-position: 0 -40px;
        }

            input.smllbtn:hover {
                background-position: 0 0px;
                color: #fff;
            }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <div class="boxContainerContainer">
                <div class="clear"></div>
                <table style="width: 100%;">
                    <tr>
                        <td id="tdReadMsg" runat="server"></td>
                    </tr>
                </table>
                <div class="clear"></div>
                <!-------------------------Top Container Start----------------------->
                <div class="itlepartContainer">
                    <h2>1 <span>Choose Lesson Plan</span></h2>
                    <h2>2 <span>Customize Lesson Plan</span></h2>
                    <h3 class="cf">
                        <asp:Label ID="lblCaptnLesson" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblcurrntLessonName" runat="server"></asp:Label>
                        <span style="float: right; margin-left: 25px;">

                            <asp:Button ID="BtnAddNewLessonPlan" runat="server" Text="Add New" CssClass="NFButton" OnClick="BtnAddNewLessonPlan_Click" />

                        </span>
                    </h3>

                </div>
                <!-------------------------Top Container End----------------------->

                <!-------------------------Middle Container start----------------------->
                <div class="btContainerPart">
                    <!------------------------------------MContainer Start----------------------------------->

                    <div class="mBxContainer">
                        <h3>Lessons - Approved </h3>
                        <div class="nobdrrcontainer">
                            <%--      <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>

                            <asp:DataList ID="dlApprovedLessons" runat="server" OnItemDataBound="dlApprovedLessons_ItemDataBound">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkApprovedLessons" CssClass="grmb" runat="server" OnClick="lnkApprovedLessons_Click" Text='<%# Eval("Name") %>' ToolTip='<%# Eval("Name") %>' CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>

                                </ItemTemplate>
                            </asp:DataList>
                            <%--   </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>




                        <%--  <a class="kk"><%# Eval("GoalName") %></a></h2>--%>
                        <%--  <div style="display: none;" class="wrapper nomar">
                                                    <div class="nobdrrcontainer">--%>
                     





                        <%--   <div class="clear"></div>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        --%>










                        <%--    <input name="" type="button" class="butn" value="Create new Lesson Plan  and Assign " />--%>
                        <div class="clear"></div>
                    </div>
                    <!------------------------------------MContainer End----------------------------------->

                    <!------------------------------------End Container Start----------------------------------->





                    <div class="righMainContainer large" id="MainDiv" runat="server" visible="true">
                        <div class="topper">
                            <ul class="navigationTabs">
                                <span class="bgb"></span>
                                <li><a href="#" rel="info">Lesson Info</a></li>
                                <li><a href="#" rel="about">Type of instruction  </a></li>
                                <li><a href="#" rel="download">Measurement Systems</a></li>
                                <li><a href="#" rel="implement" style="width: 75px;">Sets</a></li>
                                <li><a href="#" rel="implement" style="width: 75px;">Steps</a></li>
                                <li><a href="#" rel="download" style="width: 75px;">Prompts</a></li>
                                <li class="nobg"><a href="#" rel="implement">Lesson procedure</a></li>
                            </ul>

                            <div class="tabsContent">


                                <div class="tab">

                                    <div class="clear"></div>

                                  
                                            <div class="itomesContainer">

                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                     <tr>
                                                        <td ><br /></td>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td ></td>
                                                    </tr>

                                                    <tr>
                                                        <td width="15%">Lesson Plan</td>
                                                        <td width="30%">  <asp:TextBox ID="txtLessonName"  runat="server" MaxLength="300" Width="233px" ></asp:TextBox></td>
                                                        <td width="38%">Framework and Strand</td>
                                                        <td width="30%"> <asp:TextBox ID="txtFramework" runat="server" MaxLength="300" Width="233px" ></asp:TextBox></td>
                                                    </tr>


                                                     <tr>
                                                        <td ><br /></td>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td ></td>
                                                    </tr>


                                                     <tr>
                                                        <td >Specific Standard</td>
                                                        <td >  <asp:TextBox ID="txtSpecStandrd" runat="server" MaxLength="300" Width="233px" ></asp:TextBox></td>
                                                        <td >Specific Entry Point</td>
                                                        <td > <asp:TextBox ID="txtSpecEntrypoint" runat="server"  MaxLength="300" Width="233px" ></asp:TextBox></td>
                                                    </tr>

                                                     <tr>
                                                        <td ><br /></td>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td ></td>
                                                    </tr>

                                                     <tr>
                                                        <td width="25%">Pre-requisite Skills</td>
                                                        <td width="75%" colspan="3">  <asp:TextBox ID="txtPreSkills" runat="server" MaxLength="300" Width="650px" ></asp:TextBox></td>                                                      
                                                    </tr>

                                                     <tr>
                                                        <td ><br /></td>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td ></td>
                                                    </tr>

                                                     <tr>
                                                        <td width="25%">Materials</td>
                                                        <td width="75%" colspan="3">  <asp:TextBox ID="txtMaterials"  runat="server" MaxLength="300" Width="650px" ></asp:TextBox></td>                                                      
                                                    </tr>
                                                       <tr>
                                                        <td ><br /></td>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td ></td>
                                                    </tr>
                                                     <tr>
                                                        <td ><br /></td>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td  style="text-align:right;"><asp:Button ID="BtnUpdateLessonPlan" CssClass="bTnn" runat="server" Text="Update"  OnClick="BtnUpdateLessonPlan_Click"  /></td>
                                                    </tr>

                                                     <tr>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td ></td>
                                                        <td ></td>
                                                    </tr>

                                                </table>

                                            </div>

                             

                                    <br />
                                </div>


                                <div class="tab">
                                    <h2>Type Of Instructions</h2>
                                    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                        <ContentTemplate>

                                            <div class="itomesContainer">




                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                                                    <tr>
                                                        <td width="30%">
                                                            <h3>Teaching Procedure</h3>
                                                        </td>
                                                        <td width="35%"></td>
                                                        <td width="20%"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList CssClass="drpClass" ID="drpTeachingProc" runat="server">
                                                            </asp:DropDownList>

                                                        </td>
                                                        <td>


                                                            <asp:RadioButtonList ID="rdolistDatasheet" runat="server"
                                                                RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdolistDatasheet_SelectedIndexChanged">
                                                                <asp:ListItem Selected="True" Value="1">Task Analysis</asp:ListItem>
                                                                <asp:ListItem Value="0">Discrete Trial</asp:ListItem>
                                                            </asp:RadioButtonList>



                                                        </td>
                                                        <td>
                                                            <div id="taskAnalysis" runat="server">
                                                                <asp:DropDownList ID="drpTasklist" runat="server" CssClass="drpClass" Style="width: 150px;">
                                                                    <asp:ListItem Value="0">---- Select ----</asp:ListItem>
                                                                    <asp:ListItem>Forward chain</asp:ListItem>
                                                                    <asp:ListItem>Backward chain</asp:ListItem>
                                                                    <asp:ListItem>Total Task</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div id="showtrailid" runat="server" class="showtrail tdText" visible="false">
                                                                No of Trials
            <asp:TextBox ID="txtNoofTrail" onkeypress="return isNumber(event)" onpaste="return false" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                                                            </div>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <%-- <label>Specific Entry Point</label>--%></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%-- <asp:TextBox ID="txtspecEntrypoint" runat="server" MaxLength="100"></asp:TextBox>--%></td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>

                                                     <tr>
                                                        <td>
                                                         <br />  </td>
                                                        <td colspan="2">
                                                           </td>

                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <label>Major Setting :</label></td>
                                                        <td colspan="2">
                                                            <label>Minor Setting :</label></td>

                                                    </tr>
                                                     <tr>
                                                        <td>
                                                         <br />  </td>
                                                        <td colspan="2">
                                                           </td>

                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtmajorset"
                                                                Style="resize: vertical; max-height: 200px; min-height: 50px" Width="93%"
                                                                runat="server" TextMode="MultiLine"
                                                                Height="70px" MaxLength="200"></asp:TextBox></td>
                                                        <td>
                                                            <asp:TextBox ID="txtminorset"
                                                                Style="resize: vertical; max-height: 200px; min-height: 50px" Width="93%"
                                                                runat="server" TextMode="MultiLine"
                                                                Height="70px" MaxLength="200"></asp:TextBox></td>
                                                        <td></td>
                                                    </tr>
                                                    <%--        <tr>
                                                        <td width="25%">
                                                            <label>Pre-requisite Skills:</label></td>
                                                        <td width="25%">
                                                            <label>Meterials:</label></td>
                                                        <td width="25%"></td>
                                                    </tr>--%>
                                                    <%-- <tr>
                                                        <td width="25%">
                                                            <asp:TextBox Style="resize: vertical; max-height: 200px; min-height: 50px"
                                                                ID="txtPrerequistskill" runat="server" TextMode="MultiLine"
                                                                Width="97%" Height="50px" MaxLength="400"></asp:TextBox></td>
                                                        <td width="25%">
                                                            <asp:TextBox Style="resize: vertical; max-height: 200px; min-height: 50px"
                                                                ID="txtMaterials" runat="server" TextMode="MultiLine" Width="97%"
                                                                Height="50px" MaxLength="400"></asp:TextBox></td>
                                                        <td width="25%"></td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td width="25%"></td>
                                                        <td></td>
                                                        <td style="text-align: right;">
                                                            <asp:Button ID="BtnUpdate" CssClass="bTnn" runat="server" Text="Update" OnClick="BtnUpdate_Click" /></td>
                                                    </tr>
                                                </table>
                                            </div>



                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="clear"></div>
                                    <br />
                                </div>


                                <div class="tab">
                                    <h2>Measurement Systems</h2>

                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>

                                            <asp:DataList ID="dlMeasureData" runat="server" OnItemDataBound="dlMeasureData_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td style="width: 70%">
                                                                    <table style="width: 100%;">


                                                                        <tr><td colspan="2"><br /></td></tr>

                                                                        <tr>
                                                                            <td style="width: 40%;">
                                                                                Column Name
                                                                            </td>
                                                                            <td style="width: 60%;">
                                                                                <asp:HiddenField ID="hdnColId" Value='<%#Eval("DSTempSetColId") %>' runat="server" />
                                                                                <p>
                                                                                    <asp:Label ID="lblColumnName" runat="server" Text='<%#Eval("ColName") %>'></asp:Label>
                                                                                </p>

                                                                            </td>
                                                                        </tr>

                                                                        <tr><td colspan="2"><br /></td></tr>


                                                                        <tr>
                                                                            <td>
                                                                               Column Type 
                                                                               
                                                                            </td>
                                                                            <td>
                                                                                <p>

                                                                                    <asp:Label ID="lblColumnType" runat="server" Text='<%#Eval("ColTypeCd") %>'></asp:Label>

                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr><td colspan="2"><br /></td></tr>
                                                                        <tr>
                                                                            <td>
                                                                                Correct Response Data
                                                                                
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    <asp:Label ID="lblCorrectResponse" runat="server" Text='<%#Eval("CorrRespDesc") %>'></asp:Label>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr><td colspan="2"><br /></td></tr>
                                                                        <tr>
                                                                            <td>
                                                                               InCorrect Response Data
                                                                               
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    <asp:Label ID="lblIncorrectResponse" runat="server" Text='<%#Eval("InCorrRespDesc") %>'></asp:Label>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr><td colspan="2"><br /></td></tr>
                                                                        <tr>
                                                                            <td>
                                                                               Mistrial
                                                                               
                                                                            </td>
                                                                            <td>
                                                                                <p>
                                                                                    <asp:Label ID="lblMistrialDesc" runat="server" Text='<%#Eval("MisTrialDesc") %>'></asp:Label>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr><td colspan="2"><br /></td></tr>
                                                                        <tr>
                                                                            <td>
                                                                                Summary
                                                                                
                                                                            </td>
                                                                            <td>
                                                                                <p>

                                                                                    <asp:Literal ID="ltMeasureCaegory" runat="server"></asp:Literal>


                                                                                    <%-- <asp:Label ID="lblData1" runat="server"></asp:Label>  <br />

                                                                <asp:Label ID="lblData2" runat="server"></asp:Label>  <br />

                                                                <asp:Label ID="lblData3" runat="server"></asp:Label>--%>
                                                                                </p>
                                                                            </td>
                                                                        </tr>
                                                                        <tr><td colspan="2"><br /></td></tr>
                                                                    </table>

                                                                </td>
                                                                <td style="width: 30%; vertical-align:top">
                                                                    <table style="width: 100%;">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                                    <ContentTemplate>

                                                                                        <asp:Button ID="btnEditMeasure" runat="server" CssClass="smlbt" Text="Edit" OnClick="btnEditMeasure_Click" CommandArgument='<%# Eval("DSTempSetColId") %>' />

                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                                    <ContentTemplate>

                                                                                        <asp:Button ID="BtnRemove" runat="server" CssClass="smlbt" Text="Remove" OnClick="BtnRemove_Click" OnClientClick="javascript:return deleteSystem();" CommandArgument='<%# Eval("DSTempSetColId") %>' />

                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>


                                                                </td>
                                                            </tr>

                                                        </table>
                                                        <div class="clear"></div>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>
                                        </ContentTemplate>

                                    </asp:UpdatePanel>

                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnAddMeasure" runat="server" class="rbtn" Text="Add Measure" OnClick="BtnAddMeasure_Click" />
                                        </ContentTemplate>

                                    </asp:UpdatePanel>




                                    <%-- <input id="BtnAddMeasure" class="rbtn" type="button" value="Add Measure" />--%>

                                    <%--     <input class="rbtn" name="" value="Add Measure" type="button" />--%>
                                    <div class="clear"></div>


                                    <br />
                                    <div class="clear"></div>
                                </div>

                                <div class="tab">
                                    <%--<h2>
                                        <asp:Label ID="lblLessonNameSet" runat="server" Text=""></asp:Label></h2>--%>
                                    <h3>Add Set</h3>
                                    <hr />
                                    <div class="clear"></div>

                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>

                                            <asp:DataList ID="dlSetDetails" runat="server" OnItemDataBound="dlSetDetails_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="10%">
                                                                    <h3>
                                                                        <asp:Label ID="lblSetName" runat="server" Text='<%# Eval("SetCd") %>'></asp:Label></h3>
                                                                </td>
                                                                <td width="10%">
                                                                    <p></p>
                                                                </td>
                                                                <td width="30%">
                                                                    <p>
                                                                        <asp:Label ID="lblSetDesc" runat="server" Text='<%# Eval("SetName") %>'></asp:Label>

                                                                    </p>
                                                                </td>
                                                                <td width="4%"></td>
                                                                <td width="20%">
                                                                    <p></p>
                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="btnEditSet" runat="server" CssClass="smlbt" Text="Edit" OnClick="btnEditSet_Click" CommandArgument='<%# Eval("DSTempSetId") %>' />

                                                                    <%-- <a class="smlbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">


                                                                    <asp:Button ID="btnRemoveSet" runat="server" Text="Remove" CssClass="smlbt" OnClientClick="javascript:return deleteSet();" OnClick="btnRemoveSet_Click" CommandArgument='<%# Eval("DSTempSetId") %>' />

                                                                    <%-- <a class="smlbt" href="#">Remove</a>--%>


                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>
                                        </ContentTemplate>

                                    </asp:UpdatePanel>



                                    <br clear="all" />

                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>

                                            <asp:Button ID="btnAddSet" class="rbtn" runat="server" Text="Add Set" OnClick="btnAddSet_Click" />

                                        </ContentTemplate>

                                    </asp:UpdatePanel>


                                    <%--   <input id="btnAddSet" class="rbtn" type="button" value="Add Set" />--%>

                                    <%--  <input class="rbtn" name="" value="Add Set" type="button" />--%>
                                    <div class="clear"></div>
                                    <hr />
                                    <h3>Set Criteria</h3>

                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>

                                            <asp:DataList ID="dlSetCriteria" runat="server" OnItemDataBound="dlSetCriteria_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <h3>
                                                                        <asp:HiddenField ID="hdnSetCritVal" runat="server" Value='<%#Eval("DSTempRuleId") %>' />
                                                                        <asp:Label ID="lblCriteriaType" runat="server"></asp:Label></h3>
                                                                </td>
                                                                <td width="70%">
                                                                    <p>


                                                                        <asp:Label ID="lblCriteriaDef" runat="server"></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td width="4%"></td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnEditSetCriteria" runat="server" CssClass="smlbt" Text="Edit" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnEditSetCriteria_Click" />

                                                                    <%--<a class="smlRbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnRemoveSetCriteria" runat="server" CssClass="smlbt" Text="Remove" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnRemoveSetCriteria_Click" OnClientClick="javascript:return deleteSetCriteria();" />



                                                                </td>
                                                            </tr>
                                                            
                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>


                                        </ContentTemplate>


                                    </asp:UpdatePanel>



                                    <br clear="all" />
                                   

                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                        <ContentTemplate>

                                            <asp:Button ID="btnAddSetCriteria" runat="server" Text="Add Criteria" CssClass="rbtn" OnClick="btnAddSetCriteria_Click" />
                                        </ContentTemplate>

                                    </asp:UpdatePanel>





                                    
                                    <div class="clear"></div>
                                   
                                    <div class="clear"></div>
                                </div>


                                <div class="tab">


                                   
                                    <h3>Add Step</h3>
                                    <hr />
                                    <div class="clear"></div>

                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                        <ContentTemplate>

                                            <asp:DataList ID="dlStepDetails" runat="server" OnItemDataBound="dlStepDetails_ItemDataBound">
                                                <ItemTemplate>

                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="10%">
                                                                    <h3>
                                                                        <asp:Label ID="lblStepName" runat="server" Text='<%#Eval("StepCd") %>'></asp:Label>

                                                                    </h3>
                                                                </td>
                                                                <td width="10%">
                                                                    <p></p>
                                                                </td>
                                                                <td width="30%">
                                                                    <p>

                                                                        <asp:Label ID="lblStepDesc" runat="server" Text='<%#Eval("StepName") %>'></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td width="4%"></td>
                                                                <td width="20%">
                                                                    <p>
                                                                        <b>Set Name:</b><asp:Label ID="lblParntSet" runat="server" Text='<%#Eval("SetCd") %>'></asp:Label>

                                                                    </p>
                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="btnEditStep" runat="server" Text="Edit" CssClass="smlbt" OnClick="btnEditStep_Click" CommandArgument='<%# Eval("DSTempStepId") %>' />

                                                                    <%--  <a class="smlbt" href="#">Edit</a>--%>


                                                                </td>
                                                                <td width="8%">


                                                                    <asp:Button ID="btnRemoveStep" runat="server" Text="Remove" CssClass="smlbt" OnClick="btnRemoveStep_Click" CommandArgument='<%# Eval("DSTempStepId") %>' OnClientClick="javascript:return deleteStep();" />
                                                                  


                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </ItemTemplate>
                                            </asp:DataList>
                                        </ContentTemplate>



                                    </asp:UpdatePanel>


                                    <br clear="all" />

                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                        <ContentTemplate>

                                            <asp:Button ID="BtnAddStep" runat="server" Text="Add Step" CssClass="rbtn" OnClick="BtnAddStep_Click" />

                                        </ContentTemplate>


                                    </asp:UpdatePanel>



                                    <div class="clear"></div>
                                    <hr />
                                    <h3>Step Criteria</h3>

                                    <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                        <ContentTemplate>




                                            <asp:DataList ID="dlStepCriteria" runat="server" OnItemDataBound="dlStepCriteria_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <h3>
                                                                        <asp:HiddenField ID="hdnStepCritVal" runat="server" Value='<%#Eval("DSTempRuleId") %>' />
                                                                        <asp:Label ID="lblCriteriaTypeStep" runat="server"></asp:Label></h3>
                                                                </td>
                                                                <td width="70%">
                                                                    <p>


                                                                        <asp:Label ID="lblCriteriaDefStep" runat="server"></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td width="4%"></td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnEditStepCriteria" runat="server" CssClass="smlbt" Text="Edit" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnEditStepCriteria_Click" />

                                                                    <%--<a class="smlRbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnRemoveStepCriteria" runat="server" CssClass="smlbt" Text="Remove" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnRemoveStepCriteria_Click" OnClientClick="javascript:return deleteStepCriteria();" />



                                                                </td>
                                                            </tr>
                                                            
                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>


                                    <br clear="all" />


                                    <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                        <ContentTemplate>

                                            <asp:Button ID="btnAddStepCriteria" runat="server" Text="Add Criteria" CssClass="rbtn" OnClick="btnAddStepCriteria_Click" />
                                        </ContentTemplate>

                                    </asp:UpdatePanel>

                                    <%-- <input class="rbtn" name="" value="Add Criteria" type="button" />--%>
                                    <div class="clear"></div>
                                    <%--       <input class="rbtn" name="" value="Next >>" type="button" />--%>
                                    <div class="clear"></div>
                                    <div class="clear"></div>

                                </div>


                                <div class="tab">
                                    <%--  <h2>
                                        <asp:Label ID="lblLessonNamePrompt" runat="server" Text=""></asp:Label></h2>--%>
                                    <h3>Add Prompt</h3>
                                    <hr />
                                    <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                        <ContentTemplate>

                                            <div class="itomesContainer">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <%--   <tr>
                                                <td width="25%">
                                                    <label>Prompt Required</label></td>
                                                <td width="5%">
                                                    <input name="" class="rdoo" type="radio" value="" /><label>Yes</label></td>
                                                <td width="25%">
                                                    <input name="" class="rdoo" type="radio" value="" /><label>No</label></td>
                                                <td width="25%"></td>
                                            </tr>--%>
                                                </table>
                                                <br clear="all" />
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="31%">
                                                            <label>Prompt Procedure</label></td>
                                                        <td width="45%">


                                                            <asp:DropDownList ID="ddlPromptProcedure" runat="server" CssClass="drpClass" OnSelectedIndexChanged="ddlPromptProcedure_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>



                                                            <td></td>
                                                            <td></td>
                                                    </tr>
                                                </table>
                                                <br clear="all" />
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="22%">
                                                            <%--<label>Select Prompt</label>--%>

                                                            <asp:Label ID="lblSelctPrompt" runat="server" Text="Select Prompt"></asp:Label>

                                                        </td>
                                                        <td width="20%">

                                                            <asp:ListBox ID="lstCompletePrompts" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 120px; width: 100%;"></asp:ListBox>

                                                        </td>
                                                        <td width="10%">

                                                            <asp:Button ID="BtnAddPromptSelctd" runat="server" CssClass="smlRbt" Text="&gt;" OnClick="BtnAddPromptSelctd_Click" />
                                                            <asp:Button ID="BtnAddAllPrompt" runat="server" CssClass="smlRbt" Text="&gt;&gt;" OnClick="BtnAddAllPrompt_Click" />
                                                            <asp:Button ID="BtnRemvePrmptSelctd" runat="server" CssClass="smlRbt" Text="&lt;" OnClick="BtnRemvePrmptSelctd_Click" />
                                                            <asp:Button ID="BtnRemoveAllPrmpt" runat="server" CssClass="smlRbt" Text="&lt;&lt;" OnClick="BtnRemoveAllPrmpt_Click" />

                                                            <%--     <a class="smlRbt" href="#">>> </a>
                                                            <a class="smlRbt" href="#">> </a>

                                                            <a class="smlRbt" href="#"><< </a>
                                                            <a class="smlRbt" href="#">< </a>--%>
                                                        </td>
                                                        <td width="20%">

                                                            <asp:ListBox ID="lstSelectedPrompts" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 120px; width: 100%;"></asp:ListBox>
                                                            <%-- <textarea class="srBox" name="" cols="" rows=""></textarea>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="clear"></div>


                                                <asp:Button ID="BtnSavePrompt" class="rbtn" runat="server" Text="Save" OnClick="BtnSavePrompt_Click" />
                                                <%-- <input class="rbtn" name="" value="Save" type="button" />--%>
                                                <div class="clear"></div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="clear"></div>
                                    <hr />
                                    <h3>Prompt Criteria</h3>

                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                        <ContentTemplate>

                                            <asp:DataList ID="dlPromptCriteria" runat="server" OnItemDataBound="dlPromptCriteria_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="itomesContainer">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td width="20%">
                                                                    <h3>
                                                                        <asp:HiddenField ID="hdnPromptCritVal" runat="server" Value='<%#Eval("DSTempRuleId") %>' />
                                                                        <asp:Label ID="lblCriteriaTypePrompt" runat="server"></asp:Label></h3>
                                                                </td>
                                                                <td width="70%">
                                                                    <p>


                                                                        <asp:Label ID="lblCriteriaDefPrompt" runat="server"></asp:Label>
                                                                    </p>
                                                                </td>
                                                                <td width="4%"></td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnEditPromptCriteria" runat="server" CssClass="smlbt" Text="Edit" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnEditPromptCriteria_Click" />

                                                                    <%--<a class="smlRbt" href="#">Edit</a>--%>



                                                                </td>
                                                                <td width="8%">

                                                                    <asp:Button ID="BtnRemovePromptCriteria" runat="server" CssClass="smlbt" Text="Remove" CommandArgument='<%# Eval("DSTempRuleId") %>' OnClick="BtnRemovePromptCriteria_Click" OnClientClick="javascript:return deletePromptCriteria();" />



                                                                </td>
                                                            </tr>
                                                           
                                                        </table>
                                                    </div>

                                                </ItemTemplate>


                                            </asp:DataList>


                                        </ContentTemplate>



                                    </asp:UpdatePanel>


                                    <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                                        <ContentTemplate>

                                            <asp:Button ID="btnAddPrompt" runat="server" Text="Add Prompt" CssClass="rbtn" OnClick="btnAddPrompt_Click" />
                                        </ContentTemplate>

                                    </asp:UpdatePanel>


                                    <br clear="all" />

                                </div>



                                <div class="tab">
                                    <%-- <h2>
                                        <asp:Label ID="lblLessonNameProcedure" runat="server" Text=""></asp:Label></h2>--%>
                                    <h3>Add Lesson Procedure</h3>
                                    <hr />

                                    <asp:UpdatePanel ID="UpdatePanel31" runat="server">
                                        <ContentTemplate>

                                            <div class="itomesContainer">

                                                <table style="width: 100%;">

                                                    <tr>
                                                        <td style="width: 30%; text-align: center;" class="border">
                                                            <strong>Teacher (Sd/instruction)
                                                            </strong>

                                                        </td>
                                                        <td style="width: 30%; text-align: center;" class="border">
                                                            <strong>Student (Response/Desired Outcome)
                                                            </strong>

                                                        </td>
                                                        <td style="width: 30%; text-align: center;">
                                                            <strong>Consequence
                                                            </strong>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="border">
                                                            <label class="ii" style="width: 170px;">Teacher (Sd/instruction)</label>
                                                        </td>
                                                        <td class="border">
                                                            <label class="ii" style="width: 170px;">Student (Response/Desired Outcome)</label>
                                                        </td>
                                                        <td>
                                                            <label class="ii" style="width: 170px;">Correction Procedure</label>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="border">
                                                            <asp:TextBox ID="txtSDInstruction" runat="server" MaxLength="100" Width="233px"></asp:TextBox></td>
                                                        <td class="border">
                                                            <asp:TextBox ID="txtResponseOutcome" runat="server" MaxLength="100" Width="233px" TextMode="MultiLine" Rows="3" Columns="5"></asp:TextBox></td>
                                                        <td>
                                                            <asp:TextBox ID="txtCorrectionProcedure" runat="server" MaxLength="100" Width="233px" TextMode="MultiLine" Rows="3" Columns="5"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="border"></td>
                                                        <td class="border">
                                                            <label class="ii" style="width: 170px;">Correct Response</label></td>
                                                        <td>
                                                            <label class="ii" style="width: 170px;">Reinforcement Procedure</label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="border"></td>
                                                        <td class="border">
                                                            <asp:TextBox ID="txtCorrectResponse" runat="server" MaxLength="100" Width="233px" TextMode="MultiLine" Rows="3" Columns="5" ReadOnly="True"></asp:TextBox>

                                                            <%-- <asp:Literal ID="litrlCrctResp" runat="server"></asp:Literal>--%>

                                                            <%-- <asp:Label ID="lblCorrctResp" runat="server" Text="" CssClass="tdText"></asp:Label>--%>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtReinforcementProc" runat="server" MaxLength="100" Width="233px" TextMode="MultiLine" Rows="3" Columns="5"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="border"></td>
                                                        <td class="border">
                                                            <label class="ii" style="width: 170px;">Incorrect Response</label></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="border"></td>
                                                        <td class="border">

                                                            <%--   <asp:Literal ID="ltrlIncrctResp" runat="server"></asp:Literal>--%>

                                                            <%--    <asp:Label ID="lblIncrctResp" runat="server" Text="" CssClass="tdText"></asp:Label>--%>
                                                            <asp:TextBox ID="txtIncorrectResponse" runat="server" MaxLength="100" Width="233px" TextMode="MultiLine" Rows="3" Columns="5" ReadOnly="True"></asp:TextBox>


                                                        </td>
                                                        <td></td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="3" style="text-align: left;">
                                                            <asp:Button ID="btUpdateLessonProc" runat="server" class="rbtn" Text="Update" OnClick="btUpdateLessonProc_Click" />

                                                        </td>
                                                    </tr>

                                                </table>







                                                <%--  <input class="rbtn" name="" value="Summary" type="button" />--%>
                                                <div class="clear"></div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="clear"></div>
                                </div>








                            </div>

                        </div>
                        <div id="Div7">
                        </div>






                        <div class="clear"></div>
                    </div>




                    <!------------------------------------End Container End----------------------------------->

                    <div class="clear"></div>
                </div>
                <!-------------------------Middle Container End----------------------->


                <!------------------------Pop up Windows----------------------->



                <div id="SetContainer" style="width: 100%; height: 100%;">
                    <div id="AddSetDiv" class="web_dialog" style="top: 6%; left: 18%;">

                        <div id="sign_up5">
                            <a id="close_x" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <h3>Add Set
               
                            </h3>
                            <hr />

                            <table style="width: 100%;">


                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                                            <ContentTemplate>

                                                <span id="tdMsgSet" runat="server"></span>
                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </td>
                                </tr>



                                <tr>
                                    <td style="width: 30%;" class="tdText">Set Name:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtBoxAddSet" runat="server" MaxLength="50" Width="300px"></asp:TextBox>

                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </td>
                                </tr>




                                <tr>
                                    <td class="tdText">Set Description:
                                    </td>

                                    <td style="text-align: left;">
                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtSetDescription" runat="server" TextMode="MultiLine" Columns="4" Rows="3" Width="300px"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>



                                <tr>
                                    <td class="tdText">
                                        <asp:UpdatePanel ID="UpdatePanel35" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblMatchtoSamples" runat="server" Text="Match to Sample" Visible="false"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtMatcSamples" runat="server" Visible="false" MaxLength="30" Width="300px"></asp:TextBox>
                                                <asp:ImageButton ID="BtnAddSamples" runat="server" ImageUrl="~/StudentBinder/img/AddMatch.png" OnClick="BtnAddSamples_Click" Visible="false" Height="20px" Width="20px" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </td>
                                </tr>


                                <tr>
                                    <td class="tdText"></td>
                                    <td>

                                        <asp:UpdatePanel ID="UpdatePanel36" runat="server">
                                            <ContentTemplate>
                                                <asp:ListBox ID="lstMatchSamples" runat="server" CssClass="textClass" Style="color: black; font-weight: normal; height: 120px; width: 317px;" Visible="false"></asp:ListBox>
                                                <asp:ImageButton ID="btnDeltSamples" runat="server" ImageUrl="~/StudentBinder/img/DeleteMatch.png" OnClick="btnDeltSamples_Click" Visible="false" Height="20px" Width="20px" Style="vertical-align: top;" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>


                                    </td>
                                </tr>


                                <tr>
                                    <td colspan="2" style="text-align: center;">

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>

                                                <asp:Button ID="btnAddSetDetails" runat="server" Text="Save" CssClass="NFButton" OnClick="btnAddSetDetails_Click" OnClientClick=" return ValidateSet()" />

                                                <asp:Button ID="BtnUpdateSetDetails" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateSetDetails_Click" OnClientClick=" return ValidateSet()" />
                                            </ContentTemplate>

                                        </asp:UpdatePanel>


                                    </td>

                                </tr>

                            </table>


                            <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                        </div>
                        <div id="previewClose"></div>

                    </div>

                </div>


                <div id="StepContainer" style="width: 100%; height: 100%;">
                    <div id="AddStepDiv" class="web_dialog" style="top: 6%; left: 18%;">

                        <div id="SignUp_Step">
                            <a id="close_Step" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <h3>Add Step
               
                            </h3>
                            <hr />

                            <table style="width: 100%;">


                                <tr>
                                    <td colspan="2">

                                        <asp:UpdatePanel ID="UpdatePanel34" runat="server">
                                            <ContentTemplate>
                                                <span id="tdMsgStep" runat="server"></span>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 30%;" class="tdText">Select Parent Set:
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                            <ContentTemplate>

                                                <asp:DropDownList ID="ddlSetData" runat="server" CssClass="drpClass" Width="318px"></asp:DropDownList>

                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <tr>
                                    <td style="width: 30%;" class="tdText">Step Name:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtStepName" runat="server" MaxLength="50" Width="300px"></asp:TextBox>

                                            </ContentTemplate>

                                        </asp:UpdatePanel>
                                    </td>
                                </tr>




                                <tr>
                                    <td class="tdText">Step Description:
                                    </td>

                                    <td style="text-align: left;">
                                        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtStepDesc" runat="server" TextMode="MultiLine" Columns="4" Rows="3" Width="300px"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2" style="text-align: center;">

                                        <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                            <ContentTemplate>

                                                <asp:Button ID="btnAddStepDetails" runat="server" Text="Save" CssClass="NFButton" OnClick="btnAddStepDetails_Click" OnClientClick=" return ValidateStep()" />

                                                <asp:Button ID="BtnUpdateStep" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateStep_Click" OnClientClick=" return ValidateStep()" />
                                            </ContentTemplate>

                                        </asp:UpdatePanel>


                                    </td>

                                </tr>



                            </table>


                            <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                        </div>
                        <div id="Div3"></div>

                    </div>

                </div>


                <div id="CriteriaContainer" style="width: 100%; height: 100%;">
                    <div id="AddCriteriaDiv" class="web_dialog" style="top: 6%; left: 18%;">

                        <div id="signUp_Criteria">
                            <a id="close_CriteriaPopup" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <h3>
                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblCriteriaName" runat="server"></asp:Label>
                                    </ContentTemplate>

                                </asp:UpdatePanel>


                            </h3>
                            <hr />


                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                            <ContentTemplate>

                                                <table style="width: 100%;">

                                                    <tr>
                                                        <td colspan="4">
                                                            <span id="tdMsgCriteria" runat="server"></span>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdText">Select Criteria Type
                                                        </td>

                                                        <td colspan="3">
                                                            <asp:DropDownList ID="ddlCriteriaType" runat="server" CssClass="drpClass">
                                                                <asp:ListItem Value="0">..............Select.................</asp:ListItem>
                                                                <asp:ListItem>MOVE UP</asp:ListItem>
                                                                <asp:ListItem>MOVE DOWN</asp:ListItem>
                                                                <asp:ListItem>MODIFICATION</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>



                                                    <tr>
                                                        <td style="width: 20%;" class="tdText">IOA Required

                                                        </td>
                                                        <td style="width: 30%;">
                                                            <asp:RadioButtonList ID="rbtnIoaReq" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>

                                                            </asp:RadioButtonList>
                                                        </td>

                                                        <td style="width: 20%;" class="tdText">Multiteacher Required
                                                        </td>
                                                        <td style="width: 30%;">
                                                            <asp:RadioButtonList ID="rbtnMultitchr" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdText">Template Column
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTempColumn" runat="server" CssClass="drpClass" OnSelectedIndexChanged="ddlTempColumn_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="0">---------------Select Column--------------</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="tdText">Consecutive Session
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rbtnConsectiveSes" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbtnConsectiveSes_SelectedIndexChanged">
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="tdText">Measure
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTempMeasure" runat="server" CssClass="drpClass">
                                                                <asp:ListItem Value="0">---------------Select Measure--------------</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="tdText">Number of Sessions
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNumbrSessions" runat="server" ReadOnly="true" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="2"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdText">Required Score
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRequiredScore" runat="server" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="3"></asp:TextBox>
                                                        </td>
                                                        <td class="tdText">Instance
                                                        </td>
                                                        <td style="border: 0px;">
                                                            <table style="width: 100%;" cellpading="0px" cellspacing="0px">
                                                                <tr>
                                                                    <td style="width: 30%;">
                                                                        <asp:TextBox ID="txtIns1" runat="server" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="2"></asp:TextBox>
                                                                    </td>
                                                                    <td class="tdText" style="width: 14%; padding-left: 4px;">out of
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtIns2" runat="server" Width="75px" onkeypress="return isNumber(event)" onpaste="return false" MaxLength="6"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="4" style="text-align: center;">
                                                            <asp:Button ID="BtnAddSetDCriteria" runat="server" Text="Save" CssClass="NFButton" OnClick="BtnAddSetDCriteria_Click" OnClientClick=" return ValidateCriteria()" />
                                                            <asp:Button ID="BtnAddStepDCriteria" runat="server" Text="Save" CssClass="NFButton" OnClick="BtnAddStepDCriteria_Click" OnClientClick=" return ValidateCriteria()" />
                                                            <asp:Button ID="BtnAddPromptDCriteria" runat="server" Text="Save" CssClass="NFButton" OnClick="BtnAddPromptDCriteria_Click" OnClientClick=" return ValidateCriteria()" />
                                                            <asp:Button ID="BtnUpdateSetDCriteria" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateSetDCriteria_Click" OnClientClick=" return ValidateCriteria()" />
                                                            <asp:Button ID="BtnUpdateStepDCriteria" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdateStepDCriteria_Click" OnClientClick=" return ValidateCriteria()" />
                                                            <asp:Button ID="BtnUpdatePromptDCriteria" runat="server" Text="Update" CssClass="NFButton" Visible="false" OnClick="BtnUpdatePromptDCriteria_Click" OnClientClick=" return ValidateCriteria()" />
                                                        </td>
                                                    </tr>


                                                </table>

                                            </ContentTemplate>


                                        </asp:UpdatePanel>



                                    </td>
                                </tr>

                            </table>




                            <%--<input id="btn_previewOk" type="button" value="Ok" />--%>
                        </div>
                        <div id="Div5"></div>

                    </div>

                </div>



                <div id="MeasureContainer" style="width: 100%; height: 100%;">
                    <div id="AddMeasureDiv" class="web_dialog" style="top: 6%; left: 18%;">

                        <div id="SignUpNew">
                            <a id="Close1" class="close sprited1" href="#" style="margin-top: -13px; margin-right: -14px;">
                                <img src="../Administration/images/clb.PNG" style="float: right; margin-right: 0px; margin-top: 0px; z-index: 300" width="18" height="18" alt="" /></a>
                            <h3>Add Measure
               
                            </h3>
                            <hr />

                            <table style="width: 100%;">
                                <tr>

                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpdatePanel32" runat="server">
                                            <ContentTemplate>

                                                <span id="tdMsgMeasure" runat="server"></span>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </td>


                                </tr>

                                <tr>
                                    <td style="width: 30%;" class="tdText" id="tdMsg" runat="server">&nbsp;Column Name:
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtColumnName" runat="server" MaxLength="50"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>

                                </tr>

                                <tr>
                                    <td class="tdText">&nbsp;Column Type:
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>

                                                <asp:DropDownList ID="ddlColumnType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlColumnType_SelectedIndexChanged" CssClass="drpClass">
                                                    <asp:ListItem>+/-</asp:ListItem>
                                                    <asp:ListItem>Prompt</asp:ListItem>
                                                    <asp:ListItem>Text</asp:ListItem>
                                                    <asp:ListItem>Duration</asp:ListItem>
                                                    <asp:ListItem>Frequency</asp:ListItem>
                                                </asp:DropDownList>

                                            </ContentTemplate>

                                        </asp:UpdatePanel>


                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="2">

                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>



                                                <div id="PlusMinusDiv" runat="server" visible="true">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 30%;" class="tdText"></td>
                                                            <td style="width: 70%;">
                                                                <asp:RadioButtonList ID="rdbplusMinus" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem>+</asp:ListItem>
                                                                    <asp:ListItem>-</asp:ListItem>
                                                                </asp:RadioButtonList>


                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="tdText">Correct Response</td>
                                                            <td>
                                                                <asp:TextBox ID="txtplusCorrectResponse" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">InCorrectResponse
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlusIncorrectResp" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkplusIncludeMistrial" Text="Include Mistrial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtplusIncludeMistrial" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="tdText">
                                                                <b>Summary</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkplusAccuracy" Text="%Accuracy" runat="server" CssClass="tdText" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtplusAccuracy" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>



                                                        <tr>
                                                            <td colspan="2" class="tdText">(Total Correct Trials/Total Trials)*100
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkPlusPromptPerc" Text="%Prompted" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlusPromptPerc" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="tdText">(Total Prompted Trials/Total Trials)*100
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkplusindependent" runat="server" Text="%Independent" CssClass="tdText" />

                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtplusIndependent" runat="server" CssClass="tdText" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="tdText">(Total Independent Trials/Total Trials)*100
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </div>

                                                <div id="promptDiv" runat="server" visible="false">

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">Correct Response:
                                                            </td>
                                                            <td>
                                                                <%--<asp:CheckBox ID="chkCurrentPrompt" Text="Current Prompt" runat="server" />--%>

                                                                <asp:TextBox ID="txtpromptSelectPrompt" runat="server" MaxLength="50"></asp:TextBox>

                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">InCorrect Response:
                                                            </td>
                                                            <td>
                                                                <%--<asp:CheckBox ID="chkCurrentPrompt" Text="Current Prompt" runat="server" />--%>

                                                                <asp:TextBox ID="txtPromptIncrctResp" runat="server" MaxLength="50"></asp:TextBox>

                                                            </td>

                                                        </tr>
                                                        <%--  <tr>
                                                            <td class="tdText" style="width: 30%;">Select Prompt:
                                                            </td>
                                                            <td style="width: 30%;">
                                                                <asp:DropDownList ID="ddlPromptList" CssClass="drpClass" runat="server"></asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                 <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                            </td>

                                                        </tr>--%>
                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkPromptInclMisTrial" Text="Inc.Mis trial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPromptIncMisTrial" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Summary</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Report Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkPrompPercAccuracy" Text="%Accuracy" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPromptAccuracy" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="tdText">(Total Correct Trials/Total Trials)*100
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkPromptPercPrompt" Text="%Prompted" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPromptpecPrompt" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="tdText">(Total Prompted Trials/Total Trials)*100
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkPercIndependent" Text="%Independent" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPromptIndependent" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="tdText">(Total Independent Trials/Total Trials)*100
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </div>


                                                <div id="TextDiv" runat="server" visible="false">

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">Correct Response:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTextCrctResponse" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">InCorrect Response:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTextInCrctResp" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkTxtIncMisTrial" Text="Inc.Mis trial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTxtIncMisTrial" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Summary</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Report Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkTxtNa" Text="NA" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTxtNA" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="tdText">No Calculation
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkTextCustomize" Text="Customize" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTxtCustomize" runat="server" MaxLength="50"></asp:TextBox>
                                                                <asp:ImageButton ID="imgCreateEqutn" runat="server" Height="20px" Width="20px" Style="vertical-align: middle;" ImageUrl="~/StudentBinder/img/Plus.png" OnClick="imgCreateEqutn_Click" />
                                                                <asp:ImageButton ID="imageCollapseDiv" Height="20px" Width="20px" Style="vertical-align: middle;" ImageUrl="~/StudentBinder/img/minus.png" runat="server" OnClick="imageCollapseDiv_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">Customized Calculation
                                                            </td>

                                                            <td>
                                                                <div id="divBtn" style="width: 550px; min-height: 70px; height: auto !important; height: 70px; padding: 10px; margin: 25px auto 15px auto; border: 5px solid #b2ccca; background: #EFEFEF; float: left;" runat="server" visible="false">


                                                                    <%--<input type="button" id="Button19" style="width: 162px" value="OK" alt="OK" />--%>


                                                                    <div id="divColumn">
                                                                    </div>

                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>

                                                </div>


                                                <div id="DurationDiv" runat="server" visible="false">

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">Correct Response:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurCorrectResponse" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">InCorrect Response:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurIncrctResp" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkDurIncludeMistrial" Text="Inc.Mis trial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurInclMisTrial" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Summary</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Report Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkDurAverage" Text="Avg Duration" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurAverage" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkDurTotalDur" Text="Total Duration" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDurTotalDuration" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>


                                                    </table>

                                                </div>


                                                <div id="FrequencyDiv" runat="server" visible="false">

                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">Correct Response:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFreqCorrectResponse" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>


                                                        <tr>
                                                            <td style="width: 30%;" class="tdText">InCorrect Response:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtfreqIncrctResp" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Mistrial</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Mistrial Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkFreqIncludeMistrial" Text="Include Mistrial" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFreqIncludeMistrial" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <b>Summary</b>
                                                            </td>
                                                            <td class="tdText">
                                                                <b>Report Label</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdText">
                                                                <asp:CheckBox ID="chkFrequency" Text="Frequency" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFrequency" runat="server" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>



                                                    </table>

                                                </div>




                                            </ContentTemplate>

                                        </asp:UpdatePanel>

                                    </td>


                                </tr>

                                <tr>
                                    <td colspan="2" style="text-align: center">

                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>

                                                <asp:Button ID="BtnSaveMeasure" runat="server" CssClass="NFButton" Text="Save" OnClick="BtnSaveMeasure_Click" OnClientClick=" return ValidateMeasure()" />
                                                <asp:Button ID="BtnUpdateMeasure" runat="server" CssClass="NFButton" Text="Update" Visible="false" OnClick="BtnUpdateMeasure_Click" OnClientClick=" return ValidateMeasure()" />

                                            </ContentTemplate>


                                        </asp:UpdatePanel>


                                    </td>

                                </tr>






                            </table>



                        </div>


                    </div>

                </div>



                <div id="loadingDiv" style="height: 200px; width: 60%; left: 20%; display: none;">
                    <img src="img/Loadimg.gif" style="height: 50px; width: 50px;" />
                </div>










                <div class="fullOverlay">
                </div>

                <div class="clear"></div>
            </div>
        </div>
    </form>
</body>











    <script type="text/javascript">
        //$('#btnAddSet').click(function () {

        //    $('.fullOverlay').empty();
        //    $('.fullOverlay').fadeIn('slow', function () {
        //        //document.getElementById('previewFrame').style.height = 1000;          
        //        //  $('#previewBoard').css('display', 'block');
        //        $('#AddSetDiv').fadeIn();
        //        //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
        //        //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        //    });
        //});


        $('#close_x').click(function () {
            $('#AddSetDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });
        });

        $('#close_Step').click(function () {
            $('#AddStepDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });
        });


        //$('#BtnAddMeasure').click(function () {

        //    $('.fullOverlay').empty();
        //    $('.fullOverlay').fadeIn('slow', function () {
        //        //document.getElementById('previewFrame').style.height = 1000;          
        //        //  $('#previewBoard').css('display', 'block');
        //        $('#AddMeasureDiv').fadeIn();
        //        //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
        //        //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        //    });
        //});
        $('#Close1').click(function () {
            $('#AddMeasureDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });
        });

        $('#close_CriteriaPopup').click(function () {
            $('#AddCriteriaDiv').fadeOut('slow', function () {
                $('.fullOverlay').fadeOut('fast');
            });
        });


        $('#lnkApprovedLessons').click(function () {
          //  alert('asdasdasd');

        });


        //$('#BtnAddMatchSamples').click(function () {
        //    var txtValue = $('#txtMatchtoSamples').val();
        //    var nwDiv = '<div class="mSamples" style="height:auto;width:auto;border:1px black groove;float:left;background-color:gray; margin:4px; font-color:white;">' + txtValue + '</div>';
        //    $('#divMatchtoSamples').append(nwDiv);
        //});

        //$('#btnAddSetCriteria').click(function () {

        //    $('.fullOverlay').empty();
        //    $('.fullOverlay').fadeIn('slow', function () {
        //        //document.getElementById('previewFrame').style.height = 1000;          
        //        //  $('#previewBoard').css('display', 'block');
        //        $('#AddCriteriaDiv').fadeIn();
        //        //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
        //        //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        //    });

        //});



</script>
<script type="text/javascript">

    function CloseMeasure() {

        $('#AddMeasureDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function EditMeasurePopup() {

        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddMeasureDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }


    function AddSet() {

        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddSetDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }

    function AddStep() {

        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddStepDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }

    function CloseSetPopup() {
        $('#AddSetDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function CloseStepPopup() {
        $('#AddStepDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function CloseCriteriaPopup() {
        $('#AddCriteriaDiv').fadeOut('slow', function () {
            $('.fullOverlay').fadeOut('fast');
        });
    }

    function AlertNotDelete() {
        //alert('No Deletion Possible in this Criteria!!!');
        alert('Cannot delete measurement because it is used  in Criteria Calculations. Please remove the measurement from each criteria first by deleting or changing the measurement of the criteria.');

    }

    function AlertSuccessMsg() {
        alert('Data Updated Successfully...');
    }

    function AlertConvertDisc() {
        alert('Sorry..Current lessonplan cannot convert back to chained type..Its defined some values in step details');
    }
    function AlertSelectPrompt() {
        alert('Select any Prompt Procedure');
    }

    function AlertFailedMsg() {
        alert('Sorry..Data Updation Failed...');
    }
    function ValidateNoofTrials() {
        alert('Please enter no: of trials..');
    }

    function ValidateTeachingProc() {
        alert('Please select any Teaching Procedure..');
    }

    function AlertPromptValid() {
        alert('Sorry.. Current LessonPlan is assigned with a prompt measure.\n So deletion not possible...');
    }

    function NoHeaderID() {
        alert('Sorry..Lesson Plan not defined..');
    }

    function NoLessonName() {
        alert('Please enter lesson plan name');
    }



    function ValidateDrpclass() {
        alert('Please select Chain Type..');
    }

    function ValidateSubmit() {
        alert('Please Complete Template Details before submitting');
    }
    function SubmitSuccess() {
        alert('Template Editor Successfully Submitted...');
    }

    function AproveSuccess() {
        alert('Template Editor Successfully Approved...');
    }

    function MsgSessExprd() {
        alert('Sorry...Session Expired...Please try again!!!');
    }

    function deleteSystem() {
        var flag;
        flag = confirm("Are you sure you want to delete this Column ?");
        return flag;
    }


    function deleteSet() {
        var flag;
        flag = confirm("Are you sure you want to delete this Set ?");
        return flag;
    }
    function deleteStep() {
        var flag;
        flag = confirm("Are you sure you want to delete this Step ?");
        return flag;
    }
    function deleteStepCriteria() {
        var flag;
        flag = confirm("Are you sure you want to delete this StepCriteria ?");
        return flag;
    }
    function deletePromptCriteria() {
        var flag;
        flag = confirm("Are you sure you want to delete this PromptCriteria ?");
        return flag;
    }
    function deleteSetCriteria() {
        var flag;
        flag = confirm("Are you sure you want to delete this SetCriteria ?");
        return flag;
    }



    function AddCriteriaPopup() {
        $('.fullOverlay').empty();
        $('.fullOverlay').fadeIn('slow', function () {
            //document.getElementById('previewFrame').style.height = 1000;          
            //  $('#previewBoard').css('display', 'block');
            $('#AddCriteriaDiv').fadeIn();
            //$('#textEditorDiv,#btn_previewPrev,#btn_previewNext').hide();
            //$('#previewBoard').animate({ top: '20px' }, 800, 'linear');
        });
    }



    //function to Restrict input to textbox: allowing only numbers...
    function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    // function to validate Measure Data.

    function ValidateMeasure() {
        if (document.getElementById("<%=txtColumnName.ClientID%>").value == "") {
            document.getElementById("<%=tdMsgMeasure.ClientID%>").innerHTML = "<div class='warning_box'>Column Name Field can not be blank</div> ";
            document.getElementById("<%=txtColumnName.ClientID%>").focus();
            return false;
        }
        return true;

    }

    function ValidateLesson() {
        if (document.getElementById("<%=txtLessonName.ClientID%>").value == "") {
            document.getElementById("<%=txtLessonName.ClientID%>").innerHTML = "<div class='warning_box'>Please enter lesson name</div> ";
            document.getElementById("<%=txtLessonName.ClientID%>").focus();
            return false;
        }
        return true;
    }


    function ValidateSet() {
        if (document.getElementById("<%=txtBoxAddSet.ClientID%>").value == "") {
            document.getElementById("<%=tdMsgSet.ClientID%>").innerHTML = "<div class='warning_box'>Set Name Field can not be blank</div> ";
            document.getElementById("<%=txtBoxAddSet.ClientID%>").focus();
            return false;
        }
        return true;

    }

    function ValidateStep() {
        if (document.getElementById("<%=txtStepName.ClientID%>").value == "") {
            document.getElementById("<%=tdMsgStep.ClientID%>").innerHTML = "<div class='warning_box'>Step Name Field can not be blank</div> ";
            document.getElementById("<%=txtStepName.ClientID%>").focus();
            return false;
        }
        return true;

    }


    function ValidateCriteria() {
        if (document.getElementById("<%=ddlCriteriaType.ClientID%>").value == "") {
            document.getElementById("<%=tdMsgCriteria.ClientID%>").innerHTML = "<div class='warning_box'>Select any Criteria Type</div> ";
            document.getElementById("<%=ddlCriteriaType.ClientID%>").focus();
            return false;
        }
        if (document.getElementById("<%=ddlTempColumn.ClientID%>").value == "") {
            document.getElementById("<%=tdMsgCriteria.ClientID%>").innerHTML = "<div class='warning_box'>Select any Column  Type</div> ";
            document.getElementById("<%=ddlTempColumn.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=txtRequiredScore.ClientID%>").value == "") {
            document.getElementById("<%=tdMsgCriteria.ClientID%>").innerHTML = "<div class='warning_box'>Required Score Field can not be blank</div> ";
            document.getElementById("<%=txtRequiredScore.ClientID%>").focus();
            return false;
        }

        return true;

    }


    function getValueBtn(attrbte) {

        var value = attrbte;
        var textValue = document.getElementById("<%=txtTxtCustomize.ClientID%>").value;
        if (textValue == 0) {
            textValue = "";
            document.getElementById("<%=txtTxtCustomize.ClientID%>").value = textValue + attrbte;
        }
        else {
            document.getElementById("<%=txtTxtCustomize.ClientID%>").value += attrbte;
        }

    }

    function backspace() {
        var length = document.getElementById("<%=txtTxtCustomize.ClientID%>").style.length;

        var newtext = document.getElementById("<%=txtTxtCustomize.ClientID%>").value;
        if (newtext.length > 0) {
            newtext = newtext.substring(0, newtext.length - 1);

            document.getElementById("<%=txtTxtCustomize.ClientID%>").value = newtext;
        }

    }
    function cAsgn() {
        document.getElementById("<%=txtTxtCustomize.ClientID%>").value = 0;

    }




</script>



<script type="text/javascript">

    $(function () {
        // Patch fractional .x, .y form parameters for IE10.
        if (typeof (Sys) !== 'undefined' && Sys.Browser.agent === Sys.Browser.InternetExplorer && Sys.Browser.version === 10) {
            Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function Sys$WebForms$PageRequestManager$_onFormElementActive(element, offsetX, offsetY) {
                if (element.disabled) {
                    return;
                }
                this._activeElement = element;
                this._postBackSettings = this._getPostBackSettings(element, element.name);
                if (element.name) {
                    var tagName = element.tagName.toUpperCase();
                    if (tagName === 'INPUT') {
                        var type = element.type;
                        if (type === 'submit') {
                            this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                        }
                        else if (type === 'image') {
                            this._additionalInput = encodeURIComponent(element.name) + '.x=' + Math.floor(offsetX) + '&' + encodeURIComponent(element.name) + '.y=' + Math.floor(offsetY);
                        }
                    }
                    else if ((tagName === 'BUTTON') && (element.name.length !== 0) && (element.type === 'submit')) {
                        this._additionalInput = encodeURIComponent(element.name) + '=' + encodeURIComponent(element.value);
                    }
                }
            };
        }
    });

</script>
</html>
