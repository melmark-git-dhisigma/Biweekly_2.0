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
        #<%= grdGroup.ClientID %> {
          border: 1px solid #e5e7eb;
          border-radius: 8px;
          overflow: hidden;
          width: 100%;
        }

        #<%= grdGroup.ClientID %> th,
        #<%= grdGroup.ClientID %> td {
          padding: 0 !important;
          vertical-align: middle;
        }

        #<%= grdGroup.ClientID %> th {
          background: #f3f4f6;
          color: #111827;
          font-weight: 600;
          border-bottom: 1px solid #e5e7eb;
          text-align: center;
        }

        #<%= grdGroup.ClientID %> thead th:nth-child(1),
        #<%= grdGroup.ClientID %> tbody td:nth-child(1) { text-align: left; width: 60%; min-width: 90px; }
        #<%= grdGroup.ClientID %> thead th:nth-child(2),
        #<%= grdGroup.ClientID %> tbody td:nth-child(2) { width: 200px; text-align: center; }
        #<%= grdGroup.ClientID %> thead th:nth-child(3),
        #<%= grdGroup.ClientID %> tbody td:nth-child(3) { width: 110px; text-align: center; }
        #<%= grdGroup.ClientID %> thead th:nth-child(4),
        #<%= grdGroup.ClientID %> tbody td:nth-child(4) { width: 110px; text-align: center; }

        #<%= grdGroup.ClientID %> .time-input,
        #<%= grdGroup.ClientID %> input[type="time"] {
          max-width: 110px;
          min-width: 95px;
          text-align: center;
          margin: 0 auto;
          height: 30px;
          line-height: 30px;
          padding: 0 !important;
          border-radius: 4px;
        }

        #<%= grdGroup.ClientID %> tbody tr:nth-child(even) td { background: #fcfdff; }
        #<%= grdGroup.ClientID %> tbody tr:hover td { background: #f8fafc; }

        #<%= grdGroup.ClientID %> {
          table-layout: fixed;
          width: 100%;
        }
        #<%= grdGroup.ClientID %> th,
        #<%= grdGroup.ClientID %> td {
          padding: 2px;
          overflow: hidden;
          white-space: nowrap;
          text-overflow: ellipsis;
        }

        #<%= grdGroup.ClientID %> .time-input {
          width: 66px !important;
          min-width: 0;
          display: inline-block;
          padding: 0 4px;
          height: 28px;
          line-height: 28px;
          text-align: center;
          box-sizing: border-box;
        }

        #<%= grdGroup.ClientID %> input.go-btn {
          width: 35px;
          min-width: 0;
          padding: 0;
          height: 26px;
          font-size: 11px;
          text-align: center;
          border-radius: 6px;
          box-sizing: border-box;
          white-space: nowrap;
        }

        .btn-lightblue {
          background-color: #0f9bff;
          border-color: #0092fb;
          color: white;
        }
        .btn-lightblue:hover {
          background-color: #0972bd;
          border-color: #006bb7;
        }
        .att-grid {
          table-layout: fixed;
          width: 100%;
        }
        .grdGroupCustom th,
        .grdGroupCustom td,
        .att-grid th,
        .att-grid td {
          padding: 5px !important;
          vertical-align: middle;
          overflow: hidden;
          white-space: nowrap;
          text-overflow: ellipsis;
        }

        .grdGroupCustom .time-input,
        .att-grid .time-input {
          padding: 4px 6px !important;
          box-sizing: border-box;
          width: auto;
        }

        .grdGroupCustom .go-btn,
          padding: 4px 8px !important;
          height: auto;
          line-height: normal;
          display: inline-block;
          text-align: center;
          box-sizing: border-box;
        }

        .grdGroupCustom thead th { background:#f3f4f6; font-weight:600; }

        .modal-shell {
          width: 820px;
          max-width: calc(100% - 40px);
          background: #fff;
          border: 2px solid #222;
          border-radius: 4px;
          padding: 12px;
          box-sizing: border-box;
          font-family: Arial, Helvetica, sans-serif;
          color: #111;
        }

        /* Header row */
        .modal-header {
          display:flex;
          justify-content:space-between;
          align-items:center;
          margin-bottom:10px;
        }
        .modal-title {
          font-weight:700;
          font-size:18px;
          text-decoration:underline;
        }
        .modal-datetime {
          font-weight:600;
          font-size:13px;
        }

        /* Controls row (Location / Client) */
        .controls-row {
          display:flex;
          gap:12px;
          align-items:center;
          margin-bottom:8px;
        }
        .controls-row label { font-weight:700; margin-right:6px; font-size:13px; }
        .controls-row .select, .controls-row .client-input {
          border:1px solid #333;
          padding:6px 8px;
          height:30px;
          box-sizing:border-box;
          background:#fff;
        }

        /* Grid container */
        .att-grid-wrap {
          border:1px solid #111;
          padding:8px;
          background:#fafafa;
          max-height:360px;
          overflow:auto;
        }

        /* Grid rows layout - use grid for structure */
        .att-row {
          display:grid;
          grid-template-columns: 120px 160px 120px 34px 120px 70px; /* columns: Status|Client|In/Out|+|Code|Save */
          gap:8px;
          align-items:center;
          padding:6px 4px;
          border-bottom:1px solid rgba(0,0,0,0.06);
          box-sizing:border-box;
        }
        .att-row.header {
          font-weight:700;
          border-bottom:2px solid #111;
          background:transparent;
        }

        .status-indicator {
          width:12px;
          height:28px;
          background:#6b7280; /* gray bar - left small strip */
          border-radius:3px;
          box-shadow: inset -3px 0 0 rgba(0,0,0,0.06);
        }
        .att-button {
          display:inline-block;
          padding:6px 8px;
          min-width:70px;
          border-radius:4px;
          font-weight:700;
          cursor:pointer;
          border:1px solid rgba(0,0,0,0.1);
          text-align:center;
        }
        .att-present { background:#16a34a; color:#fff; }   /* green */
        .att-absent  { background:#ef4444; color:#fff; }   /* red */

        /* Client name */
        .client-name {
          text-align: left;
          font-weight: 600;
          font-size: 13px;
          white-space: normal !important;
          overflow: visible !important;
          text-overflow: clip;
          word-break: break-word;
          line-height: 1.3;
        }

        /* Time inputs stacked vertically */
        .time-stack { display:flex; flex-direction:column; gap:6px; }
        .time-input {
          width:100%;
          height:30px;
          padding:4px 6px;
          border:1px solid #333;
          box-sizing:border-box;
          font-size:13px;
        }

        /* Add button */
        .add-btn {
          width: 30px;           /* reduced from 60 or more */
          height: 30px;
          text-align: center;
          border-radius: 6px;
          background: #e6e6e6;
          border: 1px solid #999;
          cursor: pointer;
          font-weight: 700;
          font-size: 16px;
          line-height: 28px;     /* centers the + symbol */
          padding: 0;
        }

        /* Code select */
        .code-select {
          width:100%;
          height:30px;
          box-sizing:border-box;
          border:1px solid #333;
        }

        /* Save button */
        .save-btn {
          width:64px;
          height:30px;
          background:#6ee7b7;
          border:1px solid #16a34a;
          color:#033;
          font-weight:700;
          border-radius:4px;
          cursor:pointer;
        }

        /* small responsive tweaks */
        @media (max-width:860px) {
          .modal-shell { width: calc(100% - 20px); }
          .att-row { grid-template-columns: 110px 1fr 140px 34px 110px 70px; }
        }

        .att-button { padding:6px 8px; border-radius:4px; font-weight:700; cursor:pointer; }
        .att-present { background:#16a34a; color:#fff; border:1px solid #13803d; }
        .att-absent  { background:#ef4444; color:#fff; border:1px solid #b91c1c; }
        .hidStatus { display:none; }


       .att-pill {
         display:flex;
         align-items:center;
         gap:10px;
         background: transparent !important;
         padding: 0;
         margin: 0;
       }

       .att-grid td .att-pill, .grdGroupCustom td .att-pill {
         display:flex;
         align-items:center;
         gap:10px;
         background: transparent !important;
         padding: 0;
         margin: 0;
       }

      /* track (the element that changes color) */
       .att-grid td .att-switch, .grdGroupCustom td .att-switch {
         border-radius: 12px;
         position: relative;
         cursor: pointer;
         display: inline-block;
         transition: background .18s ease, box-shadow .18s ease;
         overflow: visible !important; /* allow knob to be visible */
         box-shadow: inset 0 0 0 1px rgba(0,0,0,0.06);
         border: none;
       }

    /* knob (white tile) - force visible */
        .att-grid td .att-switch .switch-knob,
        .grdGroupCustom td .att-switch .switch-knob {
          border-radius: 8px;
          background: #ffffff !important;
          position: absolute;
          top: 2px;
          left: 2px;
          transition: left .18s ease, transform .12s ease;
          box-shadow: 0 4px 8px rgba(0,0,0,0.18);
          z-index: 9999 !important;
          display:block !important;
        }

        /* labels */
        .att-grid td .att-switch .label-left,
        .att-grid td .att-switch .label-right,
        .grdGroupCustom td .att-switch .label-left,
        .grdGroupCustom td .att-switch .label-right {
          position: absolute;
          top: 50%;
          transform: translateY(-50%);
          font-size: 10px;
          font-weight: 700;
          pointer-events: none;
          z-index: 900;
          white-space: nowrap;
        }
        .att-grid td .att-switch .label-left { left: 45px; color:#111; }
        .att-grid td .att-switch .label-right { right: 45px; color:#fff; }

        /* absent state */
        .att-grid td .att-switch.att-sw-absent,
        .grdGroupCustom td .att-switch.att-sw-absent {
          background: #EF4444 !important;
        }
        .att-grid td .att-switch.att-sw-absent .label-left { opacity:1; }
        .att-grid td .att-switch.att-sw-absent .label-right { opacity:0; }
        .att-grid td .att-switch.att-sw-absent .switch-knob { left:2px !important; }

        /* present state */
        .att-grid td .att-switch.att-sw-present,
        .grdGroupCustom td .att-switch.att-sw-present {
          background: #16a34a !important;
        }
        .att-grid td .att-switch.att-sw-present .label-left { opacity:0; }
        .att-grid td .att-switch.att-sw-present .label-right { opacity:1; }
        .att-grid td .att-switch.att-sw-present .switch-knob { left:54px !important; }

        .att-switch {
          width: 90px;            /* ↓ smaller overall width */
          height: 26px;           /* ↓ smaller height */
          border-radius: 8px;
          background: #e6e6e6;
          position: relative;
          cursor: pointer;
          display: inline-block;
          overflow: hidden;
          transition: background 0.25s ease;
        }


        .switch-knob {
          position: absolute;
          top: 2px;
          left: 2px;
          width: 34px;            /* ↓ narrower knob */
          height: 22px;           /* ↓ lower height */
          background: #fff;
          border-radius: 6px;
          box-shadow: 0 2px 3px rgba(0,0,0,0.25);
          z-index: 3;
          transition: left 0.25s ease;
        }

        .att-sw-present {
          background: #16a34a !important;
        }
        .att-sw-present .label-left { opacity: 1; }
        .att-sw-present .label-right { opacity: 0; }
        .att-sw-present .switch-knob { left: 2px !important; }

        /* ABSENT = red, knob right, show Absent */
        .att-sw-absent {
          background: #ef4444 !important;
        }
        .att-sw-absent .label-left { opacity: 0; }
        .att-sw-absent .label-right { opacity: 1; }
        .att-sw-absent .switch-knob { left: 44px !important; }

        .label-left, .label-right {
          position: absolute;
          top: 50%;
          transform: translateY(-50%);
          font-weight: 600;
          font-size: 11px;        /* ↓ smaller font */
          pointer-events: none;
          transition: opacity 0.25s ease;
          z-index: 2;
        }

        .label-left {  /* Present */
          left: 8px;
          color: #fff;
        }
        .label-right { /* Absent */
          right: 8px;
          color: #fff;
        }
        



        .time-stack { display:flex; flex-direction:column; gap:6px; margin-top:6px; }
        .time-pair { display:flex; gap:6px; align-items:center; }
        .time-pair input.time-input { width:100px; height:28px; padding:2px 6px; box-sizing:border-box; }
        .time-pair .remove-extra {
          width:24px; height:24px; padding:0; border-radius:4px; border:1px solid #999; background:#f3f3f3; cursor:pointer; font-weight:700;
        }
        .extra-pair input.time-input { background:#fff; }




        .time-stack,
        .code-stack {
          display: flex;
          flex-direction: column;
          gap: 1px;         /* same vertical gap for in/out/code */
          margin-top: 6px;  /* same top offset */
          box-sizing: border-box;
        }

        /* ensure each "row" (pair) has consistent height & alignment */
        .time-pair,
        .code-pair {
          display: flex;
          align-items: center;  /* vertically center inputs/select/remove */
          gap: 8px;
          min-height: 34px;     /* forces same vertical size as inputs (adjust if needed) */
          padding: 2px 0;
          box-sizing: border-box;
        }

        /* make inputs and selects same height so visual baseline matches */
        .time-pair input.time-input,
        .time-pair input[type="time"],
        .out-stack .time-pair input.time-input,
        .code-pair select.extra-code,
        select.extra-code,
        .code-select {
          height: 28px;
          line-height: 28px;
          box-sizing: border-box;
        }

        /* style remove button to match heights */
        .remove-extra {
          height: 28px;
          min-width: 28px;
          padding: 0 6px;
          display: inline-flex;
          align-items: center;
          justify-content: center;
          box-sizing: border-box;
        }

        /* ensure table cells don't vertically center the whole cell (we want top alignment) */
        .att-row td, .att-row th, tr td, tr th {
          vertical-align: top;
        }

        /* optional: make code stack use same visual spacing as time stacks */
        .code-stack .code-pair.extra-code-pair { /* optional tweaks */ }

                .attendance-shell { width: 100%; max-width: 980px;<%-- margin: 6px 0 12px;--%> box-sizing: border-box; font-family: Arial, Helvetica, sans-serif; color: #111; position: sticky;
                top: 0; background: #fff; z-index: 50; padding-bottom: 4px;}

        /* ensure table layout cooperates with sticky */
        .att-grid {
          width: 100%;
          border-collapse: collapse; /* OK for sticky */
          table-layout: fixed;        /* keeps columns stable */
        }

        /* sticky table header cells — top uses CSS variable set by JS */
        .att-grid thead th,
        .att-grid thead td {
          position: sticky;
          top: var(--attendance-header-height, 60px); /* fallback if JS didn't run */
          z-index: 80; /* below header (header z-index 100) */
          background: #f3f4f6; /* header background so body doesn't show through */
        }

        /* small visual fix: ensure header row cells don't collapse */
        .att-grid thead th { box-shadow: 0 1px 0 rgba(0,0,0,0.06); }

        /* Header: title left, clock right — sit visually above the underline */
        .attendance-header {
          position: relative;
          <%--padding: 6px 12px 12px;--%>
          display: flex;
          align-items: center;
          justify-content: space-between;
          background: transparent;   /* allow page bg under header */
          z-index: 2;
          color: #111;               /* ensure text is visible (not white) */
        }

        /* underline line under header (keeps header content on top) */
        .attendance-header::after {
          content: "";
          position: absolute;
          left: 0;
          right: 0;
          bottom: 0;
          height: 1px;
          background: #222;
          z-index: -1;
        }

        /* Title & running datetime */
        .attendance-title { font-size: 20px; font-weight:700; padding-left:4px; }
        .attendance-datetime { font-size:20px; font-weight:700; color:#111; white-space:nowrap; }

        /* --- Controls row --- */
        /* Use space-between so left group sits left and right group hugs right (no huge middle flex) */
        .attendance-controls {
          display:flex;
          align-items:center;
          justify-content:space-between; /* left group at left, right group at right */
          padding:5px 6px;
          gap:12px;
          box-sizing:border-box;
          margin-bottom:-4px;
        }

        /* Left group: don't stretch to fill remaining space (keeps controls tight) */
        .controls-left {
          display:flex;
          gap:10px;
          align-items:center;
          flex: 0 0 auto;   /* important: do NOT grow to take free space */
        }

        /* Label / select look */
        .ctrl-label { font-weight:700; font-size:13px; margin-right:6px; }
        .ctrl-select {
          height:32px;
          padding:4px 8px;
          border:1px solid #333;
          background:#fff;
          box-sizing:border-box;
          font-size:13px;
          /* a fixed width keeps layout consistent */
          width:200px;
        }

        /* Client dropdown same width */
        .controls-left .ctrl-select + .ctrl-label + .ctrl-select,
        .controls-left .ctrl-select { width:200px; }

        /* Right group: keep the button pinned to the right and let it fill its column */
        .controls-right {
          display:flex;
          align-items:center;
          justify-content:flex-end;
          gap:8px;
          flex: 0 0 auto;   /* do not expand */
        }

        /* Make the button visually "filled" and match heights — make it fill the small right column */
        .btn-filled {
          height:34px;
          padding:6px 14px;
          background:#ccc;
          border:1px solid #999;
          color:#fff;
          font-weight:700;
          border-radius:4px;
          cursor:pointer;
          box-sizing:border-box;
          white-space:nowrap;
          min-width:120px;  /* ensure reasonable width; reduce if you want smaller button */
        }

        /* remove stray min-width on controls-right if present from previous CSS */
        .controls-right { min-width: 0; }

        /* ensure cells below align to top (avoids vertical centering causing extra gaps) */
        .att-row td, .att-row th, tr td, tr th { vertical-align: top; }

        /* small screens fallback */
        @media (max-width:860px) {
          .ctrl-select { width: 140px; }
          .btn-filled { min-width: 90px; padding:6px 8px; }
        }


                .static-grid-header {
          width: 100%;
          max-width: 100%;
          box-sizing: border-box;
          background: #fff;
          border-bottom: 1px solid rgba(0,0,0,0.12);
          font-family: Arial, Helvetica, sans-serif;
        }

        /* Single header row — uses CSS Grid to align with grid body columns */
        .static-grid-row {
          display: grid;
          grid-template-columns: 87px 223px 103px 103px 48px 79px 82px;
          gap: 8px;
          align-items: center;
          font-weight: 700;
          font-size: 13px;
          background: #f3f4f6; /* header background */
          color: #111;
          box-sizing: border-box;
        }

        /* Ensure grid body rows line up: GridView TD widths should match the header widths.
           Some browsers and table-layout may differ; if needed change GridView column widths to exactly match above. */

        /* Scrollable grid area */
        .attendance-grid-wrap {
          max-height: 71vh;
          overflow-y: auto;
          -webkit-overflow-scrolling: touch;
          background: #fff;
          box-sizing: border-box;
        }

        /* Ensure GridView table uses fixed layout so column widths match the CSS grid */
        .att-grid {
          width: 100%;
          border-collapse: collapse;
          table-layout: fixed; /* critical so columns respect widths */
        }

        /* make client cell wrap text to show full name on multiple lines */
        .att-grid td .client-name { white-space: normal; word-break: break-word; }

        /* Keep table header (if somehow exists) invisible */
        .att-grid thead { display: none; }




                #staticGridHeader .static-grid-row > div {
          text-align: center;        /* center header text */
          display: inline-block;     /* keep width applied by JS while allowing centering */
          vertical-align: middle;
          padding: 0 6px;            /* small horizontal breathing room */
          box-sizing: border-box;
        }

        /* Ensure grid body cells align center under headers */
        .att-grid td {
          text-align: center;        /* center cell contents by default */
          vertical-align: middle;
          word-break: break-word;    /* allow client name to wrap */
        }

        /* But keep some elements (like client name) readable if you want them to wrap:
           override centering for client column content if you prefer left-align for long names: */
        .att-grid td .client-name {
          /* Uncomment the next line if you want client names left-aligned instead of centered:
          text-align: left; */
          display: block;
          white-space: normal;
        }

        /* If you used header-specific classes and want to force center there too */
        .static-grid-row .col-client,
        .static-grid-row .col-in,
        .static-grid-row .col-out,
        .static-grid-row .col-add,
        .static-grid-row .col-code,
        .static-grid-row .col-att,
        .static-grid-row .col-save {
          text-align: center;
        }

        /* Small responsive tweak: keep centered and avoid overflow on small screens */
        @media (max-width: 860px) {
          #staticGridHeader .static-grid-row > div { padding: 0 4px; font-size: 12px; }
          .att-grid td { font-size: 13px; }
        }

                .att-grid td.has-bottom-btn {
          position: relative;
          /* optional: ensure cell tall enough to accommodate extras; tweak as needed */
          min-height: 56px;
          padding-bottom: 2px; /* keep some breathing room */
        }

        /* Position the actual buttons at the bottom.
           change left:50% + transform to right:8px if you want them aligned right. */
        .att-grid td.has-bottom-btn .add-btn,
        .att-grid td.has-bottom-btn .save-btn {
          position: absolute;
          bottom: 2px;
          left: 50%;
          transform: translateX(-50%);
          margin: 0;
          z-index: 50;
          /* make the button look normal, don't let it shrink */
          display: inline-block;
        }

        /* If you want the buttons to be right-aligned, use this instead (uncomment)
        .att-grid td.has-bottom-btn .add-btn,
        .att-grid td.has-bottom-btn .save-btn {
          right: 8px;
          left: auto;
          transform: none;
        }
        */

                .edit-past-panel {
          background: #fff;
          box-shadow: 0 2px 8px rgba(0,0,0,0.08);
          width: 210px;
          font-family: Arial, sans-serif;
        }

        /* heading */
        .edit-past-inner h3 {
          margin: 0 0 8px 0;
          font-size: 16px;
        }

        /* old-style layout: left calendar, right controls */
        .row { margin-top:8px; }

        /* left column (calendar) */
        .col-left {
          display: inline-block;
          vertical-align: top;
          width: 220px;   /* calendar width (adjust if needed) */
        }

        /* right column (messages + buttons) */
        .col-right {
          display: inline-block;
          vertical-align: top;
          width: 120px;   /* controls column */
          padding-left: 8px;
          box-sizing: border-box;
        }

        /* small-note and button spacing */
        .small-note { color: #c00; font-size: 12px; display: block; margin-bottom: 8px; }

        /* clearfix for older browsers */
        .clearfix:after {
          content: "";
          display: block;
          clear: both;
        }

        /* fallback to floats for very old browsers */
        @media all and (max-width: 380px) {
          .col-left, .col-right {
            display: block;
            width: 100%;
            padding-left: 0;
          }
        }

        .calendar-popup {
            border: 1px solid #ccc;
            background: #fff;
            box-shadow: 0 3px 10px rgba(0,0,0,0.12);
            padding: 6px;
            width: auto;
            /* optional rounded corners */
            border-radius: 4px;
          }
          /* optional: ensure calendar table fits */
          .calendar-popup table { margin: 0; }

                .past-selected-date { color: red !important; }


                .calendar-popup td.today,
        .calendar-popup td.today a,
        .calendar-popup td td_today { 
            background: rgba(255,240,140,0.9) !important;
            color: #000 !important;
            opacity: 1 !important;
        }


                .calendar-style td.cal-selected {
            background-color: Silver !important;
            color: White !important;
            font-weight: 700;
        }

        .calendar-style td.cal-today {
            background-color: LightSkyBlue !important;
            color: White !important;
            font-weight: 700;
        }

        .row-absent {
          opacity: 0.55;
          background-color: #f3f4f6 !important;
          color: #6b7280 !important;
        }
        .row-absent input[readonly],
        .row-absent input[disabled],
        .row-absent select[disabled],
        .row-absent button[disabled] {
          background-color: transparent !important;
          color: inherit !important;
          border-color: transparent !important;
          cursor: not-allowed;
        }

        .attendance-grid .table-wrapper {
          max-height: 420px;      /* change as needed */
          overflow: auto;
          position: relative;
        }

        /* grid table */
        .attendance-grid-table {
          width: 100%;
          border-collapse: collapse;
          table-layout: fixed;    /* IMPORTANT: consistent column widths across browsers */
        }

        /* th/td housekeeping */
        .attendance-grid-table th,
        .attendance-grid-table td {
          padding: 8px 10px;
          box-sizing: border-box; /* make widths include padding/border */
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          vertical-align: middle;
        }

        /* sticky header row */
        .attendance-grid-table thead th {
          position: sticky;
          top: 0;
          z-index: 5;
          background: #fff;       /* or match your header bg */
          border-bottom: 1px solid #ddd;
        }

        /* OPTIONAL: set column widths (percent) — adjust to match your layout */
        .attendance-grid-table th:nth-child(1),  /* Student ID */
        .attendance-grid-table td:nth-child(1) { width: 10%; }
        .attendance-grid-table th:nth-child(2),
        .attendance-grid-table td:nth-child(2) { width: 30%; } /* Name */
        .attendance-grid-table th:nth-child(3),
        .attendance-grid-table td:nth-child(3) { width: 15%; } /* Class */
        .attendance-grid-table th:nth-child(4),
        .attendance-grid-table td:nth-child(4) { width: 15%; } /* In Time */
        .attendance-grid-table th:nth-child(5),
        .attendance-grid-table td:nth-child(5) { width: 15%; } /* Out Time */
        .attendance-grid-table th:nth-child(6),
        .attendance-grid-table td:nth-child(6) { width: 15%; } /* Actions */

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
        function showLoaderAndPostBack(btn) {
            document.getElementById('loader').style.display = 'block';
            __doPostBack(btn.name, '');
        }

        function validateGo(btn) {
            try {
                if (!btn) return true;

                var row = (btn.closest && (btn.closest('tr') || btn.closest('.att-row'))) || btn.parentNode || null;
                if (!row) return true;

                var inInputs = Array.prototype.slice.call(row.querySelectorAll('input.in-time, input[id$="txtInTime"]'));
                var outInputs = Array.prototype.slice.call(row.querySelectorAll('input.out-time, input[id$="txtOutTime"]'));

                if (!inInputs.length && row.querySelector('input[id$="txtInTime"]')) {
                    inInputs = [row.querySelector('input[id$="txtInTime"]')];
                }
                if (!outInputs.length && row.querySelector('input[id$="txtOutTime"]')) {
                    outInputs = [row.querySelector('input[id$="txtOutTime"]')];
                }

                if (inInputs.length === 0 && outInputs.length === 0) return true;

                // collect possible code selects in the row (main + extras)
                var codeSelects = Array.prototype.slice.call(row.querySelectorAll('select[id*="ddlCode"], select[id$="ddlCode"]'));

                // helper: get lookup-code (like "LOA" / "SICK") for a given select element
                function getLookupCodeFromSelect(sel) {
                    if (!sel) return null;
                    var val = sel.value;
                    // 1) try window.__attendanceCodes if present (server serialized list with {id, name, code})
                    if (window.__attendanceCodes && Array.isArray(window.__attendanceCodes)) {
                        for (var j = 0; j < window.__attendanceCodes.length; j++) {
                            var o = window.__attendanceCodes[j];
                            if (o && (o.id == val || String(o.id) === String(val))) {
                                // prefer o.code, fallback to name
                                return (o.code || o.name || null);
                            }
                        }
                    }

                    // 2) try data-code attribute on selected option
                    var opt = sel.options[sel.selectedIndex];
                    if (opt) {
                        var dc = opt.getAttribute && (opt.getAttribute('data-code') || opt.getAttribute('data-code'.toUpperCase()));
                        if (dc) return dc;
                        // last fallback: use option text (may be LookupName)
                        return (opt.text || null);
                    }

                    return null;
                }

                // normalize exempt set (case-insensitive)
                var exemptCodes = { 'LOA': true, 'SICK': true };

                var maxLen = Math.max(inInputs.length, outInputs.length);
                for (var i = 0; i < maxLen; i++) {
                    // try to pick corresponding code select:
                    // prefer the select at the same index as inputs, otherwise fallback to first main select
                    var codeSel = (i < codeSelects.length) ? codeSelects[i] : (codeSelects.length ? codeSelects[0] : null);
                    var lookupCode = getLookupCodeFromSelect(codeSel);
                    // normalize to upper for comparison
                    var lookupCodeNorm = lookupCode ? lookupCode.trim().toUpperCase() : null;

                    var inEl = inInputs[i];
                    var outEl = outInputs[i];

                    var inVal = inEl ? (inEl.value || '').trim() : '';
                    var outVal = outEl ? (outEl.value || '').trim() : '';

                    // If this pair is marked with an exempt code (LOA or SICK), skip validation for this pair
                    if (lookupCodeNorm && exemptCodes[lookupCodeNorm]) {
                        // skip validation for this index
                        continue;
                    }

                    // existing validations
                    if (!inVal && !outVal) {
                        showGridBand('Please enter IN and/or OUT time.');
                        if (inEl) inEl.focus();
                        return false;
                    }
                    if (inVal && inVal.indexOf(':') === -1) {
                        if (inEl) inEl.focus();
                        showGridBand('Please enter a valid IN time (e.g. 09:30).');
                        return false;
                    }
                    if (outVal && outVal.indexOf(':') === -1) {
                        if (outEl) outEl.focus();
                        showGridBand('Please enter a valid OUT time (e.g. 17:00).');
                        return false;
                    }

                    // Optional: validate OUT >= IN (simple lexicographic/time check)
                    if (inVal && outVal && inVal.indexOf(':') !== -1 && outVal.indexOf(':') !== -1) {
                        // parse HH:MM -> minutes since midnight
                        function toMinutes(t) {
                            var parts = t.split(':');
                            if (parts.length < 2) return null;
                            var hh = parseInt(parts[0], 10);
                            var mm = parseInt(parts[1], 10);
                            if (isNaN(hh) || isNaN(mm)) return null;
                            return hh * 60 + mm;
                        }
                        var inM = toMinutes(inVal);
                        var outM = toMinutes(outVal);
                        if (inM !== null && outM !== null && outM < inM) {
                            showGridBand('OUT time cannot be earlier than IN time.');
                            if (outEl) outEl.focus();
                            return false;
                        }
                    }
                }

                return true;
            } catch (err) {
                console.error('validateGo error', err);
                return false;
            }
        }

        

        function showSaveLoader(btn) {
            try {
                // ensure the selected date is preserved client-side before the postback
                // prefer the helper if defined (server code registers setHiddenPastDateIso())
                var hid = document.getElementById('<%= hidPastDate.ClientID %>');
                if (typeof setHiddenPastDateIso === 'function') {
                    // ensure server helper knows the iso ('' for today, otherwise yyyy-MM-dd)
                    setHiddenPastDateIso(hid ? hid.value || '' : '');
                } else if (hid && hid.value !== undefined) {
                    // defensive fallback: keep the hidden field value as-is
                    // (this keeps the selected date value available to server on postback)
                }

                // Use the new global loader function (preferred). If older helper exists, call it.
                if (typeof showLoaderAndPostBack === 'function') {
                    // some older code paths used showLoaderAndPostBack(btn)
                    try { showLoaderAndPostBack(btn); } catch (e) { /* ignore */ }
                } else if (typeof showLoader === 'function') {
                    try { showLoader('Saving\u2026'); } catch (e) { /* ignore */ }
                } else {
                    // fallback: show an element with id 'globalLoader' if present
                    var fallback = document.getElementById('globalLoader');
                    if (fallback) fallback.style.display = 'block';
                }

                // disable button to avoid double click (optional)
                try {
                    if (btn && btn.disabled !== undefined) {
                        btn.disabled = true;
                        // keep attribute for server-side postback if required
                        try { btn.setAttribute('data-save-disabled', '1'); } catch (e) { }
                    }
                } catch (e) { }

                // allow the normal postback to proceed (return true in OnClientClick)
                return true;
            } catch (e) {
                console && console.log && console.log('showSaveLoader error', e);
                return true; // do not block postback on loader error
            }
        }




        (function () {
            var gridBandTimer = null;
            var loaderFallbackTimer = null; // fallback to avoid loader stuck forever

            function clearLoaderFallback() {
                if (loaderFallbackTimer) {
                    clearTimeout(loaderFallbackTimer);
                    loaderFallbackTimer = null;
                }
            }

            // showGridBand: shows the banner and schedules hideGridBand,
            // and schedules hideLoader() AFTER the banner duration so loader stays until band finishes.
            window.showGridBand = function (msg, ms, kind) {
                var delay = (typeof ms === 'number' && ms > 0) ? ms : 4000;
                var band = document.getElementById('gridBand');
                var text = document.getElementById('gridBandMsg');
                if (!band || !text) return;

                if (gridBandTimer) { clearTimeout(gridBandTimer); gridBandTimer = null; }

                // kind: "success" | "error" (default error)
                if (kind === 'success') band.style.background = '#16a34a';  // green
                else band.style.background = '#dc2626';                      // red

                text.innerHTML = msg || '';
                band.style.display = 'block';

                // Clear any loader fallback — server is handling the UX now.
                clearLoaderFallback();

                // hide the visual band after its delay
                gridBandTimer = setTimeout(function () {
                    hideGridBand();
                }, delay);

                // hide loader after band duration + small buffer (so loader stays while the band is visible)
                try {
                    setTimeout(function () {
                        if (typeof hideLoader === 'function') hideLoader();
                    }, delay + 150);
                } catch (e) { /* ignore */ }
            };

            window.hideGridBand = function () {
                var band = document.getElementById('gridBand');
                var text = document.getElementById('gridBandMsg');
                if (band) band.style.display = 'none';
                if (text) text.innerHTML = '';
            };

            // showLoader: show loader and start fallback timer (so loader eventually hides if server doesn't act)
            function showLoader(msg) {
                try {
                    var loader = document.getElementById('loader');
                    var msgEl = document.getElementById('loaderMsg');
                    if (!loader) {
                        return;
                    }
                    if (msgEl) msgEl.textContent = msg || 'Loading…';
                    loader.style.display = 'block';
                    try { loader.style.removeProperty('visibility'); } catch (e) { }
                    document.body.setAttribute('aria-busy', 'true');

                    // start a fallback: if nothing clears this, hide loader after X ms
                    clearLoaderFallback();
                    // fallback duration: 15 seconds (tune as needed)
                    loaderFallbackTimer = setTimeout(function () {
                        try { hideLoader(); } catch (e) { /* swallow */ }
                        loaderFallbackTimer = null;
                    }, 15000);
                } catch (e) { console.warn('showLoader error', e); }
            }

            // hideLoader: hide loader and clear fallback timer
            function hideLoader() {
                try {
                    var loader = document.getElementById('loader');
                    var msgEl = document.getElementById('loaderMsg');
                    if (!loader) {
                        return;
                    }
                    try { loader.style.setProperty('display', 'none', 'important'); } catch (e) { loader.style.display = 'none'; }
                    if (msgEl) msgEl.textContent = '';
                    document.body.removeAttribute('aria-busy');
                } catch (e) { console.warn('hideLoader error', e); }
                // always clear fallback once we hide
                clearLoaderFallback();
            }

            window.showLoader = showLoader;
            window.hideLoader = hideLoader;

            // hide on full page load
            if (document.readyState === 'complete') hideLoader();
            else window.addEventListener('load', hideLoader);

            // wire ASP.NET AJAX partial postbacks defensively
            try {
                if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    var prm = null;
                    try { prm = Sys.WebForms.PageRequestManager.getInstance(); } catch (e) { prm = null; }
                    if (prm) {
                        // on begin, show loader immediately
                        prm.add_beginRequest(function () { showLoader('Loading…'); });

                        // on end: DO NOT hide the loader immediately.
                        // Instead, wait briefly for any server-registered startup scripts to run,
                        // and then only hide if the server did NOT display a gridBand.
                        prm.add_endRequest(function () {
                            try {
                                setTimeout(function () {
                                    try {
                                        var band = document.getElementById('gridBand');
                                        if (band && band.style && band.style.display && band.style.display.toLowerCase() !== 'none') {
                                            // server has shown the band; do nothing — showGridBand will clear fallback and hide loader later
                                            return;
                                        }
                                        // otherwise hide loader (short settle)
                                        hideLoader();
                                    } catch (inner) {
                                        try { hideLoader(); } catch (ex) { /* swallow */ }
                                    }
                                }, 300); // small delay to allow server-registered scripts to run (adjust 300->500 if needed)
                            } catch (e) {
                                setTimeout(function () { try { hideLoader(); } catch (ex) { } }, 300);
                            }
                        });
                    }
                }
            } catch (e) { console.warn('PageRequestManager wiring failed', e); }
        })();









        function toggleStatus(el, event) {
            if (!el) return;
            // support passing either the switch element or a child element
            var switchEl = (el.classList && el.classList.contains('att-switch')) ? el : el.closest('.att-switch');
            if (!switchEl) return;

            // if currently absent → switch to present
            var nowAbsent = switchEl.classList.contains('att-sw-absent');

            // flip classes
            switchEl.classList.remove('att-sw-present', 'att-sw-absent');
            switchEl.classList.add(nowAbsent ? 'att-sw-present' : 'att-sw-absent');

            // inline color fallback (keeps existing behavior)
            try { switchEl.style.background = nowAbsent ? '#16a34a' : '#ef4444'; } catch (e) { }

            var knob = switchEl.querySelector('.switch-knob');
            if (knob) {
                // put knob left when present, right when absent (match your existing positions)
                knob.style.left = nowAbsent ? '2px' : '80px';
            }

            // find containing row (tr or .att-row) to detect student id
            var row = switchEl.closest('tr') || switchEl.closest('.att-row') || switchEl.parentNode;

            // attempt to discover studentId (recommended: add data-studentid on row or on the switch element)
            var studentId = null;
            try {
                studentId = switchEl.getAttribute && switchEl.getAttribute('data-studentid')
                         || (row && row.dataset && row.dataset.studentid)
                         || (row && row.getAttribute && row.getAttribute('data-studentid'));
            } catch (e) { studentId = null; }

            // Compose the value: 1 = Present, 0 = Absent
            var value = nowAbsent ? '1' : '0';

            // PRIORITY: try to set hidStatus_<studentId> (if we have a studentId)
            var hid = null;
            if (studentId) {
                // first try direct id lookup (fast and reliable)
                hid = document.getElementById('hidStatus_' + studentId);
                // second try: search within the row for a control whose id ends with that pattern (handles server-generated unique ids)
                if (!hid && row) {
                    try {
                        hid = row.querySelector("input[id$='hidStatus_" + studentId + "']");
                    } catch (e) { hid = null; }
                }
            }

            // FALLBACK: if we couldn't find indexed hidden field, try the old generic hidStatus in the row or document
            if (!hid && row) {
                try { hid = row.querySelector("input[id$='hidStatus']"); } catch (e) { hid = null; }
            }
            if (!hid) {
                try { hid = document.querySelector("input[id$='hidStatus']"); } catch (e) { hid = null; }
            }

            // Finally set value if found
            if (hid) hid.value = value;

            // Prevent default click action if event available
            if (event) {
                if (event.preventDefault) event.preventDefault();
                event.returnValue = false;
            }

            return false;
        }

        function addExtraRow(btn, studentId, codeVal) {
            try {
                if (!btn) return;
                var row = btn.closest('tr') || btn.closest('.att-row') || btn.parentNode;
                if (!row) { alert('Row not found'); return; }

                if (studentId) row.setAttribute('data-studentid', studentId);

                // locate original server inputs
                var txtIn = row.querySelector('input.in-time, input[id$="txtInTime"]');
                var txtOut = row.querySelector('input.out-time, input[id$="txtOutTime"]');

                // locate tds; fallback to guessed indexes
                var inCell = txtIn ? (txtIn.closest('td') || txtIn.parentNode) : null;
                var outCell = txtOut ? (txtOut.closest('td') || txtOut.parentNode) : null;

                var tds = row.querySelectorAll('td');
                // Determine codeCell: try existing element with class 'code-cell' or containing select.extra-code
                var codeCell = row.querySelector('td .code-cell') ? row.querySelector('td .code-cell').closest('td') : null;
                if (!codeCell) {
                    var existingCodeSel = row.querySelector('select.extra-code, select[id$="ddlCode"], .code-cell select');
                    if (existingCodeSel) codeCell = (existingCodeSel.closest('td') || existingCodeSel.parentNode);
                }

                // fallback by index
                if (!inCell || !outCell) {
                    if (tds && tds.length >= 6) { inCell = inCell || tds[2]; outCell = outCell || tds[3]; }
                }
                if (!codeCell) {
                    if (tds && tds.length >= 6) codeCell = tds[5];
                    else if (tds && tds.length >= 5) codeCell = tds[tds.length - 1];
                }

                if (!inCell || !outCell || !codeCell) {
                    alert('Cannot find IN/OUT/Code cells in row. Please confirm markup.');
                    return;
                }

                // ensure in/out stacks exist and move original server inputs
                var inStack = inCell.querySelector('.time-stack');
                var outStack = outCell.querySelector('.time-stack');
                if (!inStack || !outStack) {
                    inStack = document.createElement('div'); inStack.className = 'time-stack in-stack';
                    outStack = document.createElement('div'); outStack.className = 'time-stack out-stack';
                    var firstInPair = document.createElement('div'); firstInPair.className = 'time-pair';
                    var firstOutPair = document.createElement('div'); firstOutPair.className = 'time-pair';
                    if (txtIn) firstInPair.appendChild(txtIn);
                    if (txtOut) firstOutPair.appendChild(txtOut);
                    inStack.appendChild(firstInPair);
                    outStack.appendChild(firstOutPair);
                    inCell.appendChild(inStack);
                    outCell.appendChild(outStack);
                }

                // ensure code stack exists in codeCell and move server code control there if needed
                var codeStack = codeCell.querySelector('.code-stack');
                if (!codeStack) {
                    codeStack = document.createElement('div'); codeStack.className = 'code-stack';
                    var firstCodePair = document.createElement('div'); firstCodePair.className = 'code-pair';
                    var serverCode = codeCell.querySelector('select, input[type="hidden"].ddl-server, .ddl-server');
                    if (serverCode) firstCodePair.appendChild(serverCode);
                    codeStack.appendChild(firstCodePair);
                    codeCell.appendChild(codeStack);
                }

                function showMsg(s) {
                    if (typeof showGridBand === 'function') { showGridBand(s, 4000, 'error'); }
                    else { alert(s); }
                }

                // validate last pair completeness
                var inPairs = inStack.querySelectorAll('.time-pair');
                var outPairs = outStack.querySelectorAll('.time-pair');
                var lastIndex = Math.min(inPairs.length, outPairs.length) - 1;
                if (lastIndex >= 0) {
                    var lastIn = inPairs[lastIndex].querySelector('input.in-time, input[id$="txtInTime"]');
                    var lastOut = outPairs[lastIndex].querySelector('input.out-time, input[id$="txtOutTime"]');
                    var inVal = lastIn && lastIn.value ? lastIn.value.trim() : '';
                    var outVal = lastOut && lastOut.value ? lastOut.value.trim() : '';
                    if (!inVal && !outVal) { showMsg('Please enter IN/OUT for the last entry before adding another.'); if (lastIn) lastIn.focus(); return; }
                    if ((inVal && !outVal) || (!inVal && outVal)) { showMsg('Please complete both IN and OUT for the last entry before adding another.'); (outVal ? lastIn : lastOut).focus(); return; }
                }

                // compute new index for naming
                var existingExtras = inStack.querySelectorAll('.time-pair.extra-pair');
                var newIndex = existingExtras.length + 1; // 1-based index

                // create new aligned pairs
                var newInPair = document.createElement('div'); newInPair.className = 'time-pair extra-pair';
                var newOutPair = document.createElement('div'); newOutPair.className = 'time-pair extra-pair';
                var newCodePair = document.createElement('div'); newCodePair.className = 'code-pair extra-code-pair';

                var inInput = document.createElement('input');
                inInput.type = 'time';
                inInput.className = 'time-input in-time';
                inInput.value = '';
                inInput.name = 'txtIn_extra_' + studentId + '_' + newIndex;
                inInput.id = 'txtIn_extra_' + studentId + '_' + newIndex;

                var outInput = document.createElement('input');
                outInput.type = 'time';
                outInput.className = 'time-input out-time';
                outInput.value = '';
                outInput.name = 'txtOut_extra_' + studentId + '_' + newIndex;
                outInput.id = 'txtOut_extra_' + studentId + '_' + newIndex;

                // build codeSelect robustly
                var codeSelect = document.createElement('select');
                codeSelect.className = 'extra-code code-select';
                codeSelect.name = 'ddlCode_extra_' + studentId + '_' + newIndex;
                codeSelect.id = 'ddlCode_extra_' + studentId + '_' + newIndex;

                // default option
                var defaultOpt = document.createElement('option');
                defaultOpt.value = '';
                defaultOpt.textContent = '---Select---';
                codeSelect.appendChild(defaultOpt);

                // read lookup array
                var codes = (window.__attendanceCodes && window.__attendanceCodes.length) ? window.__attendanceCodes : [];

                for (var ci = 0; ci < codes.length; ci++) {
                    try {
                        var opt = document.createElement('option');
                        // support different key names gracefully
                        var val = (codes[ci].id !== undefined) ? codes[ci].id : (codes[ci].LookupId !== undefined ? codes[ci].LookupId : (codes[ci].value !== undefined ? codes[ci].value : ''));
                        var name = (codes[ci].name !== undefined) ? codes[ci].name : (codes[ci].text !== undefined ? codes[ci].text : String(val));
                        opt.value = val;
                        opt.textContent = name;
                        codeSelect.appendChild(opt);
                    } catch (ex) {
                        // ignore malformed entries
                    }
                }

                // If server provided a code value (e.g. when rendering from existing extras), ensure it's present and selected
                if (typeof codeVal === 'undefined' || codeVal === null) codeVal = '';

                if (codeVal !== '') {
                    var found = false;
                    try {
                        var opts = codeSelect.options;
                        for (var z = 0; z < opts.length; z++) {
                            if (String(opts[z].value) === String(codeVal)) { found = true; break; }
                        }
                    } catch (er) { found = false; }
                    if (!found) {
                        var extraOpt = document.createElement('option');
                        extraOpt.value = codeVal;
                        extraOpt.textContent = String(codeVal);
                        codeSelect.appendChild(extraOpt);
                    }
                    try { codeSelect.value = codeVal; } catch (ee) { /* ignore */ }
                }

                // assemble new pairs
                newInPair.appendChild(inInput);
                newOutPair.appendChild(outInput);
                newCodePair.appendChild(codeSelect);

                // append to stacks (index alignment preserved)
                inStack.appendChild(newInPair);
                outStack.appendChild(newOutPair);
                codeStack.appendChild(newCodePair);

                // attach listeners (your existing helper)
                if (typeof attachInputListenersToInput === 'function') {
                    attachInputListenersToInput(inInput, row, studentId);
                    attachInputListenersToInput(outInput, row, studentId);
                    attachInputListenersToInput(codeSelect, row, studentId);
                }

                // serialize now and sync
                if (typeof serializeExtraTimesForRow === 'function') serializeExtraTimesForRow(row, studentId);
                if (typeof syncRowPairs === 'function') syncRowPairs(row);

                inInput.focus();

                // older code called both serialize functions — keep that for compatibility
                if (typeof serializeExtrasForStudent === 'function') serializeExtrasForStudent(studentId);

            } catch (e) {
                console.error('addExtraRow error', e);
            }
        }



        function safeAttachInputListeners(el, row, studentId) {
            try {
                if (typeof attachInputListenersToInput === 'function') {
                    attachInputListenersToInput(el, row, studentId);
                    return;
                }
                // fallback: add basic listeners to re-serialize extras on change
                el.addEventListener('change', function () { serializeExtraTimesForRow(row, studentId); });
                el.addEventListener('input', function () { serializeExtraTimesForRow(row, studentId); });
            } catch (e) { console && console.warn && console.warn('attach listeners', e); }
        }
        var allowClockUpdate = true;
        (function () {
            (function () {
                function updateClock() {
                    if (!allowClockUpdate) return;
                    var now = new Date();

                    var dateStr = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear();

                    var timeStr = now.toLocaleTimeString('en-US', { hour12: true })
                                     .replace(/([ap])m/, function (match) {
                                         return match.toUpperCase();
                                     });

                    var el = document.getElementById('currentDateTime');
                    if (el) el.textContent = dateStr + '  ' + timeStr;
                }
                updateClock();
                setInterval(updateClock, 1000);
            })();
        })();

        function renderAllExtras() {
            // pick each row - grid rows usually have CSS class or are tr in the grid
            var rows = document.querySelectorAll('#' + '<%= grdGroup.ClientID %> tr'); // ensure server GridView ID used
            if (!rows || rows.length === 0) {
                // fallback: any tr inside .attendance-grid-wrap with data-studentid attribute
                rows = document.querySelectorAll('.attendance-grid-wrap tr, .att-row, .att-grid tr');
            }
            Array.prototype.forEach.call(rows, function (r) {
                try { renderExtrasForRow(r); } catch (e) { console && console.error && console.error('renderExtrasForRow', e); }
            });
        }

        //document.addEventListener('DOMContentLoaded', function () {
        //    try { renderAllExtras(); } catch (e) { console && console.warn && console.warn(e); }
        //});


        function parseExtrasString(raw) {
            if (!raw) return [];
            // Some strings may be partially encoded; split by semicolon first (they may contain literal ';' encoded)
            var rawParts = raw.split(';');
            var out = [];
            for (var i = 0; i < rawParts.length; i++) {
                var p = (rawParts[i] || '').trim();
                if (!p) continue;
                // decodeURIComponent often throws on malformed sequences, wrap in try/catch
                try { p = decodeURIComponent(p); } catch (e) { /* ignore, use original */ }
                // ensure exactly 3 parts
                var pieces = p.split('|');
                // pad to length 3
                while (pieces.length < 3) pieces.push('');
                // trim each
                for (var j = 0; j < 3; j++) pieces[j] = (pieces[j] || '').trim();
                // skip fully empty
                if (!pieces[0] && !pieces[1] && !pieces[2]) continue;
                out.push({ in: pieces[0], out: pieces[1], code: pieces[2] });
            }
            return out;
        }


        function syncRowPairs(row) {
            try {
                if (!row) return;
                var inStack = row.querySelector('.in-stack .time-stack') || row.querySelector('.in-stack');
                var outStack = row.querySelector('.out-stack .time-stack') || row.querySelector('.out-stack');
                var codeStack = row.querySelector('.code-stack');

                if (!inStack || !outStack || !codeStack) return;

                var inPairs = inStack.querySelectorAll('.time-pair');
                var outPairs = outStack.querySelectorAll('.time-pair');
                var codePairs = codeStack.querySelectorAll('.code-pair');

                var count = Math.min(inPairs.length, outPairs.length, Math.max(codePairs.length, inPairs.length));

                for (var i = 0; i < count; i++) {
                    var inH = inPairs[i] ? inPairs[i].getBoundingClientRect().height : null;
                    var outH = outPairs[i] ? outPairs[i].getBoundingClientRect().height : null;
                    var h = Math.max(inH || 0, outH || 0, 34); // min fallback
                    if (codePairs[i]) {
                        codePairs[i].style.minHeight = h + 'px';
                        // ensure vertical centering of children
                        codePairs[i].style.display = 'flex';
                        codePairs[i].style.alignItems = 'center';
                    }
                    if (inPairs[i]) inPairs[i].style.minHeight = h + 'px';
                    if (outPairs[i]) outPairs[i].style.minHeight = h + 'px';
                }
            } catch (e) { console.error('syncRowPairs', e); }
        }

        window.createExtraPairNodes = function (rowOrBtnOrSelector, studentId, opts) {
            opts = opts || {};
            var inVal = (typeof opts.inVal !== 'undefined' && opts.inVal !== null) ? String(opts.inVal) : '';
            var outVal = (typeof opts.outVal !== 'undefined' && opts.outVal !== null) ? String(opts.outVal) : '';
            var codeVal = (typeof opts.codeValue !== 'undefined' && opts.codeValue !== null) ? String(opts.codeValue) : '';

            // Resolve row: element, selector string, or studentId
            var row = null;
            if (rowOrBtnOrSelector && rowOrBtnOrSelector.nodeType) {
                row = (rowOrBtnOrSelector.tagName && rowOrBtnOrSelector.tagName.toLowerCase() === 'tr')
                    ? rowOrBtnOrSelector
                    : (rowOrBtnOrSelector.closest ? (rowOrBtnOrSelector.closest('tr') || rowOrBtnOrSelector.closest('.att-row')) : null);
            }
            if (!row && typeof rowOrBtnOrSelector === 'string') {
                row = document.querySelector(rowOrBtnOrSelector) || document.getElementById(rowOrBtnOrSelector);
                if (row && !(row.tagName && row.tagName.toLowerCase() === 'tr')) {
                    row = (row.closest ? (row.closest('tr') || row.closest('.att-row')) : row);
                }
            }
            if (!row && studentId) {
                row = document.querySelector('[data-studentid="' + studentId + '"]') ||
                      document.getElementById('row_' + studentId) ||
                      (document.getElementById('hidExtraTimes_' + studentId) && document.getElementById('hidExtraTimes_' + studentId).closest ? document.getElementById('hidExtraTimes_' + studentId).closest('tr') : null);
            }

            if (!row) {
                console.warn('createExtraPairNodes: row required or not found', rowOrBtnOrSelector, studentId);
                return null;
            }

            // helper: ensure stack exists and move server control into first pair (if still present)
            function ensureStack(cell, className, firstChildToMove) {
                var stack = cell.querySelector('.' + className);
                if (!stack) {
                    stack = document.createElement('div');
                    stack.className = className;
                    var firstPair = document.createElement('div');
                    firstPair.className = (className.indexOf('code') !== -1) ? 'code-pair' : 'time-pair';
                    if (firstChildToMove && firstChildToMove.parentNode === cell) {
                        try { firstPair.appendChild(firstChildToMove); } catch (e) { /* ignore */ }
                    }
                    stack.appendChild(firstPair);
                    cell.appendChild(stack);
                } else {
                    var pairSelector = (className.indexOf('code') !== -1) ? '.code-pair' : '.time-pair';
                    if (!stack.querySelector(pairSelector)) {
                        var p = document.createElement('div'); p.className = (className.indexOf('code') !== -1) ? 'code-pair' : 'time-pair';
                        stack.appendChild(p);
                    }
                }
                return stack;
            }

            // heuristics to find main inputs and cells
            var txtIn = row.querySelector('input.in-time, input[id$="txtInTime"]');
            var txtOut = row.querySelector('input.out-time, input[id$="txtOutTime"]');

            var inCell = txtIn ? (txtIn.closest ? txtIn.closest('td') : (txtIn.parentNode && txtIn.parentNode.tagName === 'TD' ? txtIn.parentNode : null)) : null;
            var outCell = txtOut ? (txtOut.closest ? txtOut.closest('td') : (txtOut.parentNode && txtOut.parentNode.tagName === 'TD' ? txtOut.parentNode : null)) : null;

            var tds = row.querySelectorAll('td');
            var codeCell = null;
            var codeInner = row.querySelector('td .code-cell');
            if (codeInner) codeCell = codeInner.closest ? codeInner.closest('td') : codeInner.parentNode;

            if (!codeCell) {
                var existingCodeSel = row.querySelector('select.extra-code, select[id$="ddlCode"], .ddl-server, select.code-select');
                if (existingCodeSel) codeCell = (existingCodeSel.closest ? existingCodeSel.closest('td') : existingCodeSel.parentNode);
            }

            // index fallback (best-effort)
            if (!inCell || !outCell) {
                if (tds && tds.length >= 6) { inCell = inCell || tds[2]; outCell = outCell || tds[3]; }
            }
            if (!codeCell) {
                if (tds && tds.length >= 6) codeCell = tds[5];
                else if (tds && tds.length >= 1) codeCell = tds[tds.length - 1];
            }

            if (!inCell || !outCell || !codeCell) {
                console.error('createExtraPairNodes: could not determine IN/OUT/Code cells', { inCell: !!inCell, outCell: !!outCell, codeCell: !!codeCell, row: row });
                return null;
            }

            // ensure stacks and move server controls into first child if appropriate
            var inStack = ensureStack(inCell, 'time-stack', txtIn);
            var outStack = ensureStack(outCell, 'time-stack', txtOut);

            var codeStack = codeCell.querySelector('.code-stack');
            if (!codeStack) {
                codeStack = document.createElement('div'); codeStack.className = 'code-stack';
                var firstCodePair = document.createElement('div'); firstCodePair.className = 'code-pair';
                var serverCode = codeCell.querySelector('select, .ddl-server, input[type="hidden"].ddl-server, select[id$="ddlCode"]');
                if (serverCode && serverCode.parentNode === codeCell) {
                    try { firstCodePair.appendChild(serverCode); } catch (e) { /* ignore */ }
                }
                codeStack.appendChild(firstCodePair);
                codeCell.appendChild(codeStack);
            } else {
                if (!codeStack.querySelector('.code-pair')) {
                    var fp = document.createElement('div'); fp.className = 'code-pair'; codeStack.appendChild(fp);
                }
            }

            // === Prevent duplicates ===
            // Check existing extra pairs for identical in/out/code
            var existingInExtras = Array.prototype.slice.call(inStack.querySelectorAll('.time-pair.extra-pair'));
            var existingOutExtras = Array.prototype.slice.call(outStack.querySelectorAll('.time-pair.extra-pair'));
            var existingCodeExtras = Array.prototype.slice.call(codeStack.querySelectorAll('.code-pair.extra-code-pair'));
            for (var e = 0; e < existingInExtras.length; e++) {
                try {
                    var inp = existingInExtras[e].querySelector('input.in-time');
                    var outp = (existingOutExtras[e] && existingOutExtras[e].querySelector) ? existingOutExtras[e].querySelector('input.out-time') : null;
                    var codep = (existingCodeExtras[e] && existingCodeExtras[e].querySelector) ? existingCodeExtras[e].querySelector('select, input') : null;
                    var inExistingVal = inp && inp.value ? String(inp.value) : '';
                    var outExistingVal = outp && outp.value ? String(outp.value) : '';
                    var codeExistingVal = codep && codep.value ? String(codep.value) : '';
                    if (inExistingVal === inVal && outExistingVal === outVal && codeExistingVal === codeVal) {
                        // duplicate found -> return existing nodes
                        return {
                            inPair: existingInExtras[e],
                            outPair: existingOutExtras[e] || null,
                            codePair: existingCodeExtras[e] || null,
                            inInput: inp,
                            outInput: outp,
                            codeSelect: codep,
                            row: row
                        };
                    }
                } catch (xx) { /* ignore alignment issues */ }
            }

            // === Create new nodes ===
            var inPair = document.createElement('div'); inPair.className = 'time-pair extra-pair';
            var outPair = document.createElement('div'); outPair.className = 'time-pair extra-pair';
            var codePair = document.createElement('div'); codePair.className = 'code-pair extra-code-pair';

            var inInput = document.createElement('input'); inInput.setAttribute('type', 'time'); inInput.className = 'time-input in-time';
            var outInput = document.createElement('input'); outInput.setAttribute('type', 'time'); outInput.className = 'time-input out-time';
            try { inInput.value = inVal || ''; } catch (e) { /* ignore setting */ }
            try { outInput.value = outVal || ''; } catch (e) { /* ignore setting */ }

            // Build code select: default + codes from window.__attendanceCodes (expected [{id,name},...])
            var codeSelect = document.createElement('select'); codeSelect.className = 'extra-code code-select';
            var defaultOpt = document.createElement('option'); defaultOpt.value = ''; defaultOpt.textContent = '---Select---'; codeSelect.appendChild(defaultOpt);

            var codes = (window.__attendanceCodes && window.__attendanceCodes.length) ? window.__attendanceCodes : [];
            for (var ci = 0; ci < codes.length; ci++) {
                try {
                    var opt = document.createElement('option');
                    opt.value = codes[ci].id;
                    opt.textContent = codes[ci].name;
                    codeSelect.appendChild(opt);
                } catch (ex) { /* ignore malformed */ }
            }

            // If server provided a code value not in the list, append it
            if (codeVal !== '') {
                var found = false;
                try {
                    var opts = codeSelect.options;
                    for (var z = 0; z < opts.length; z++) {
                        if (String(opts[z].value) === String(codeVal)) { found = true; break; }
                    }
                } catch (er) { found = false; }
                if (!found) {
                    var extraOpt = document.createElement('option');
                    extraOpt.value = codeVal;
                    extraOpt.textContent = codeVal;
                    codeSelect.appendChild(extraOpt);
                }
                try { codeSelect.value = codeVal; } catch (ee) { /* ignore */ }
            }

            // === Add name attributes for POST (unique deterministic names) ===
            // Use the number of existing pairs + 1 as index to keep names stable in the row
            var index = Math.max(existingInExtras.length, existingOutExtras.length, existingCodeExtras.length) + 1;
            // sanitize studentId for name usage
            var sidSafe = String(studentId || '').replace(/[^\w\-]/g, '_');
            inInput.name = 'txtIn_extra_' + sidSafe + '_' + index;
            outInput.name = 'txtOut_extra_' + sidSafe + '_' + index;
            codeSelect.name = 'ddlCode_extra_' + sidSafe + '_' + index;

            // Append to pairs
            inPair.appendChild(inInput);
            outPair.appendChild(outInput);
            codePair.appendChild(codeSelect);

            inStack.appendChild(inPair);
            outStack.appendChild(outPair);
            codeStack.appendChild(codePair);

            // Attach listeners if helper exists (defensive)
            if (typeof attachInputListenersToInput === 'function') {
                try {
                    attachInputListenersToInput(inInput, row, studentId);
                    attachInputListenersToInput(outInput, row, studentId);
                    attachInputListenersToInput(codeSelect, row, studentId);
                } catch (e) { console.warn('attachInputListenersToInput failed', e); }
            }

            // Serialize & sync hidden value so server sees the latest extras
            try { if (typeof serializeExtraTimesForRow === 'function') serializeExtraTimesForRow(row, studentId); } catch (e) { console.warn('serializeExtraTimesForRow failed', e); }
            try { if (typeof syncRowPairs === 'function') syncRowPairs(row); } catch (e) { /* optional sync */ }

            return {
                inPair: inPair,
                outPair: outPair,
                codePair: codePair,
                inInput: inInput,
                outInput: outInput,
                codeSelect: codeSelect,
                row: row
            };
        };

        window.serializeExtraTimesForRow = function (rowOrBtnOrSelector, studentId) {
            // Resolve row same as createExtraPairNodes
            var row = null;
            if (rowOrBtnOrSelector && rowOrBtnOrSelector.nodeType) {
                row = (rowOrBtnOrSelector.tagName && rowOrBtnOrSelector.tagName.toLowerCase() === 'tr')
                    ? rowOrBtnOrSelector
                    : (rowOrBtnOrSelector.closest ? (rowOrBtnOrSelector.closest('tr') || rowOrBtnOrSelector.closest('.att-row')) : null);
            }
            if (!row && typeof rowOrBtnOrSelector === 'string') {
                row = document.querySelector(rowOrBtnOrSelector) || document.getElementById(rowOrBtnOrSelector);
                if (row && !(row.tagName && row.tagName.toLowerCase() === 'tr')) {
                    row = (row.closest ? (row.closest('tr') || row.closest('.att-row')) : row);
                }
            }
            if (!row && studentId) {
                row = document.querySelector('[data-studentid="' + studentId + '"]') ||
                      document.getElementById('row_' + studentId) ||
                      (document.getElementById('hidExtraTimes_' + studentId) && document.getElementById('hidExtraTimes_' + studentId).closest ? document.getElementById('hidExtraTimes_' + studentId).closest('tr') : null);
            }
            if (!row) {
                console.warn('serializeExtraTimesForRow: row not found', rowOrBtnOrSelector, studentId);
                return '';
            }

            // If studentId not provided, try to read from row attribute or hidden input id
            if (!studentId) {
                var ds = row.getAttribute('data-studentid');
                if (ds) studentId = ds;
                else {
                    var hidGuess = row.querySelector('input[type=hidden][id^="hidExtraTimes_"]');
                    if (hidGuess) studentId = (hidGuess.id || '').replace(/^hidExtraTimes_/, '');
                }
            }

            // Collect pairs in order: first the first pair in stack (server main), then extra pairs
            var inStack = row.querySelector('.time-stack');
            var outStack = row.querySelectorAll('.time-stack')[1] || row.querySelector('.time-stack'); // second stack if present else first
            var codeStack = row.querySelector('.code-stack');

            var inPairs = inStack ? Array.prototype.slice.call(inStack.querySelectorAll('.time-pair')) : [];
            var outPairs = outStack ? Array.prototype.slice.call(outStack.querySelectorAll('.time-pair')) : [];
            var codePairs = codeStack ? Array.prototype.slice.call(codeStack.querySelectorAll('.code-pair')) : [];

            // We'll serialize up to the max length among these lists
            var maxLen = Math.max(inPairs.length, outPairs.length, codePairs.length);
            var segments = [];

            for (var i = 0; i < maxLen; i++) {
                var inEl = inPairs[i] ? inPairs[i].querySelector('input.in-time') : null;
                var outEl = outPairs[i] ? outPairs[i].querySelector('input.out-time') : null;
                var codeEl = codePairs[i] ? (codePairs[i].querySelector('select') || codePairs[i].querySelector('input')) : null;

                var inV = inEl && typeof inEl.value !== 'undefined' ? String(inEl.value).trim() : '';
                var outV = outEl && typeof outEl.value !== 'undefined' ? String(outEl.value).trim() : '';
                var codeV = codeEl && typeof codeEl.value !== 'undefined' ? String(codeEl.value).trim() : '';

                // skip fully empty pairs (both times empty and no code)
                if (!inV && !outV && !codeV) continue;

                // normalize times to HH:mm if they include seconds or are Date strings (best-effort)
                function normalizeHHmm(s) {
                    if (!s) return '';
                    // if looks like HH:mm:ss -> keep HH:mm
                    var m = s.match(/^(\d{1,2}:\d{2})(:\d{2})?/);
                    if (m) return m[1];
                    // try parse as Date
                    var dt = Date.parse(s);
                    if (!isNaN(dt)) {
                        var d = new Date(dt);
                        var hh = ('0' + d.getHours()).slice(-2);
                        var mm = ('0' + d.getMinutes()).slice(-2);
                        return hh + ':' + mm;
                    }
                    // fallback: return as-is
                    return s;
                }

                inV = normalizeHHmm(inV);
                outV = normalizeHHmm(outV);

                // escape pipe and semicolon from code/time so serialization can't break (we'll percent-encode)
                function esc(seg) {
                    if (!seg) return '';
                    return encodeURIComponent(seg);
                }

                segments.push(esc(inV) + '|' + esc(outV) + '|' + esc(codeV));
            }

            var result = segments.join(';');

            // Write to hidden input: id = hidExtraTimes_<studentId>
            if (studentId) {
                var hid = document.getElementById('hidExtraTimes_' + studentId) || row.querySelector('input[type=hidden][id^="hidExtraTimes_"]') || row.querySelector('input[type=hidden][name^="hidExtraTimes_"]');
                if (hid) {
                    hid.value = result;
                } else {
                    // optionally create a hidden input in the row if not present
                    try {
                        var newH = document.createElement('input');
                        newH.type = 'hidden';
                        newH.id = 'hidExtraTimes_' + studentId;
                        newH.name = 'hidExtraTimes_' + studentId;
                        newH.value = result;
                        // append to row (prefer last td)
                        var lastTd = row.querySelector('td:last-child') || row;
                        lastTd.appendChild(newH);
                    } catch (e) { /* ignore creation failure */ }
                }
            }

            return result;
        };

        /* -- renderExtrasForRow (also global) -- */
        window.renderExtrasForRow = function (rowOrBtnOrId, studentId) {
            // Resolve row
            var row = null;
            if (rowOrBtnOrId && rowOrBtnOrId.nodeType) {
                row = (rowOrBtnOrId.tagName && rowOrBtnOrId.tagName.toLowerCase() === 'tr') ?
                    rowOrBtnOrId : (rowOrBtnOrId.closest ? (rowOrBtnOrId.closest('tr') || rowOrBtnOrId.closest('.att-row')) : null);
            }
            if (!row && typeof rowOrBtnOrId === 'string') row = document.querySelector(rowOrBtnOrId) || document.getElementById(rowOrBtnOrId);
            if (!row && studentId) {
                row = document.querySelector('[data-studentid="' + studentId + '"]') ||
                      document.getElementById('row_' + studentId) ||
                      (document.getElementById('hidExtraTimes_' + studentId) && document.getElementById('hidExtraTimes_' + studentId).closest ? document.getElementById('hidExtraTimes_' + studentId).closest('tr') : null);
            }
            if (!row) {
                console.warn('renderExtrasForRow: row not found for', rowOrBtnOrId, studentId);
                return;
            }

            // find hidden field (try canonical client id first, then server hidden field in row)
            var hid = document.getElementById('hidExtraTimes_' + studentId) ||
                      document.querySelector('input[type=hidden][name="hidExtraTimes_' + studentId + '"]') ||
                      row.querySelector('input[id$="_hidExtraTimes"], input[id*="hidExtraTimes"], input[type=hidden][name*="hidExtraTimes"]');

            var raw = hid ? (hid.value || '') : '';

            // Clean existing extra-pairs (preserve the first main pair)
            try {
                var inStacks = row.querySelectorAll('.time-stack');
                if (inStacks && inStacks.length > 0) {
                    // first time-stack is main (in/out pairs for IN); subsequent stacked .time-pair.extra-pair are extras
                    var inStack = inStacks[0];
                    var extras = Array.prototype.slice.call(inStack.querySelectorAll('.time-pair.extra-pair'));
                    extras.forEach(function (n) { n.parentNode.removeChild(n); });
                    // remove extra out-pairs (if second .time-stack exists)
                    var outStack = (inStacks.length > 1) ? inStacks[1] : null;
                    if (outStack) {
                        var extrasOut = Array.prototype.slice.call(outStack.querySelectorAll('.time-pair.extra-pair'));
                        extrasOut.forEach(function (n) { n.parentNode.removeChild(n); });
                    }
                }
                var codeStack = row.querySelector('.code-stack');
                if (codeStack) {
                    var extrasCode = Array.prototype.slice.call(codeStack.querySelectorAll('.code-pair.extra-code-pair'));
                    extrasCode.forEach(function (n) { n.parentNode.removeChild(n); });
                }
            } catch (e) { console.warn('renderExtrasForRow cleanup failed', e); }

            if (!raw) return;

            // helper: safe decode a component (handles %3A and +)
            function safeDecode(s) {
                if (s == null) return '';
                s = String(s).trim();
                if (s === '') return '';
                try {
                    // replace + with space (in case form encoding used +)
                    var alt = s.replace(/\+/g, ' ');
                    // decodeURIComponent fails on strings with stray % sequences, so try-catch
                    return decodeURIComponent(alt);
                } catch (ex) {
                    // fallback: try unescape (old) or return original
                    try { return unescape(s); } catch (e) { return s; }
                }
            }

            // helper: normalize time to HH:MM substring for comparison (07:45:00 -> 07:45)
            function normTime(t) {
                if (!t) return '';
                var v = String(t).trim();
                // if includes seconds like 07:45:00 -> take first 5 chars
                if (v.length >= 5 && v.indexOf(':') >= 0) return v.substr(0, 5);
                return v;
            }

            // get main pair to avoid duplication
            var mainInEl = row.querySelector('.time-stack .time-pair:not(.extra-pair) input.in-time');
            var mainOutEl = row.querySelector('.time-stack .time-pair:not(.extra-pair) input.out-time');
            var mainInVal = mainInEl ? normTime(mainInEl.value || mainInEl.getAttribute('value') || '') : '';
            var mainOutVal = mainOutEl ? normTime(mainOutEl.value || mainOutEl.getAttribute('value') || '') : '';

            // parse: support JSON or semicolon delim "In|Out|Code;..."
            var entries = [];

            var trimmed = raw.trim();
            // try JSON
            if (trimmed.charAt(0) === '[') {
                try {
                    var arr = JSON.parse(trimmed);
                    if (Array.isArray(arr)) {
                        arr.forEach(function (item) {
                            if (!item) return;
                            if (typeof item === 'string') {
                                var parts = item.split('|');
                                entries.push([parts[0] || '', parts[1] || '', parts[2] || '']);
                            } else if (typeof item === 'object') {
                                // tolerant key picks
                                var inv = item.In || item.in || item.Start || item.From || '';
                                var outv = item.Out || item.out || item.End || '';
                                var codev = item.Code || item.code || item.LookupId || '';
                                entries.push([inv, outv, codev]);
                            }
                        });
                    }
                } catch (e) {
                    // fallback to semicolon parsing if JSON.parse fails
                    trimmed = trimmed.replace(/^\s*;|;\s*$/g, '');
                }
            }

            // if no JSON-derived entries, do semicolon parsing
            if (entries.length === 0) {
                // normalize stray semicolons and remove empty segments
                trimmed = trimmed.replace(/^\s*;|;\s*$/g, '');
                var parts = trimmed.split(';');
                for (var i = 0; i < parts.length; i++) {
                    var item = (parts[i] || '').trim();
                    if (!item) continue;
                    var pieces = item.split('|');
                    // decode each piece but push raw (we'll decode before create)
                    entries.push([pieces[0] || '', pieces[1] || '', pieces[2] || '']);
                }
            }

            // Now create nodes but skip duplicates of the main pair
            for (var j = 0; j < entries.length; j++) {
                var p = entries[j];
                var inRaw = p[0] || '';
                var outRaw = p[1] || '';
                var codeRaw = p[2] || '';

                var inDecoded = safeDecode(inRaw);
                var outDecoded = safeDecode(outRaw);
                var codeDecoded = safeDecode(codeRaw);

                // normalize for comparison and skip if duplicates the main pair
                if (normTime(inDecoded) === mainInVal && normTime(outDecoded) === mainOutVal) {
                    // if it's first entry and duplicates main, skip it
                    continue;
                }

                // skip completely empty pairs
                if (!inDecoded && !outDecoded) continue;

                // create nodes, pass decoded values
                var nodes = null;
                try {
                    nodes = window.createExtraPairNodes(row, studentId, {
                        inVal: inDecoded,
                        outVal: outDecoded,
                        codeValue: codeDecoded
                    });
                } catch (e) {
                    console.warn('renderExtrasForRow: createExtraPairNodes threw', e, studentId, inDecoded, outDecoded, codeDecoded);
                }
                if (!nodes) {
                    console.warn('renderExtrasForRow: createExtraPairNodes failed for', studentId, inDecoded, outDecoded, codeDecoded);
                }
            }
        };

        function attachInputListenersToInput(el, row, studentId) {
            if (!el) return;
            if (el.__hasAttachListener) return;
            el.__hasAttachListener = true;
            el.addEventListener('change', function () { try { serializeExtraTimesForRow(row, studentId); } catch (e) { } });
            if (el.tagName && el.tagName.toLowerCase() === 'input') {
                el.addEventListener('input', function () { try { serializeExtraTimesForRow(row, studentId); } catch (e) { } });
            }
        }


        function getHiddenExtrasEl(studentId, row) {
            var hid = null;
            if (row) hid = row.querySelector('#hidExtraTimes_' + studentId) || row.querySelector('input[type=hidden][name^="hidExtraTimes_"]');
            if (!hid) hid = document.getElementById('hidExtraTimes_' + studentId);
            // fallback to grid hidden with numeric id pattern if present
            if (!hid) hid = document.querySelector('input[id^="grdGroup_ctl"][id$="hidExtraTimes"]');
            return hid;
        }

        function buildExtrasStringEncoded(arrayOfTokens) {
            var segs = [];
            for (var i = 0; i < arrayOfTokens.length; i++) {
                var t = arrayOfTokens[i];
                // we still include empty parts to preserve positions: encode each
                var a = encodeURIComponent(t.in || '');
                var b = encodeURIComponent(t.out || '');
                var c = encodeURIComponent(t.code || '');
                segs.push(a + '|' + b + '|' + c);
            }
            return segs.join(';');
        }

        function renderExtrasForRowCanonical(rowOrBtnOrId, studentId) {
            // resolve row same as your other helpers
            var row = null;
            if (rowOrBtnOrId && rowOrBtnOrId.nodeType) {
                row = (rowOrBtnOrId.tagName && rowOrBtnOrId.tagName.toLowerCase() === 'tr') ? rowOrBtnOrId :
                      (rowOrBtnOrId.closest ? (rowOrBtnOrId.closest('tr') || rowOrBtnOrId.closest('.att-row')) : null);
            }
            if (!row && typeof rowOrBtnOrId === 'string') {
                row = document.querySelector(rowOrBtnOrId) || document.getElementById(rowOrBtnOrId);
                if (row && !(row.tagName && row.tagName.toLowerCase() === 'tr')) {
                    row = (row.closest ? (row.closest('tr') || row.closest('.att-row')) : row);
                }
            }
            if (!row && studentId) {
                row = document.querySelector('[data-studentid="' + studentId + '"]') || document.getElementById('row_' + studentId);
                if (!row) {
                    var hidGuess = document.getElementById('hidExtraTimes_' + studentId);
                    if (hidGuess && hidGuess.closest) row = hidGuess.closest('tr');
                }
            }
            if (!row) { console.warn('renderExtrasForRowCanonical: row not found', rowOrBtnOrId, studentId); return; }

            // resolve studentId if not provided
            if (!studentId) {
                var ds = row.getAttribute('data-studentid');
                if (ds) studentId = ds;
                else {
                    var hG = row.querySelector('input[type=hidden][id^="hidExtraTimes_"]');
                    if (hG) studentId = (hG.id || '').replace(/^hidExtraTimes_/, '');
                }
            }
            if (!studentId) { console.warn('renderExtrasForRowCanonical: studentId unknown'); return; }

            var hid = getHiddenExtrasEl(studentId, row);
            var raw = hid ? (hid.value || '') : '';
            // Parse/normalize
            var tokens = parseExtrasString(raw);

            // Determine main values to skip duplicates (main controls in row)
            var mainInEl = row.querySelector('input.in-time, input[id$="txtInTime"]');
            var mainOutEl = row.querySelector('input.out-time, input[id$="txtOutTime"]');
            var mainCodeEl = row.querySelector('select[id$="ddlCode"], select.code-select');
            var mainIn = mainInEl && mainInEl.value ? (mainInEl.value || '').trim() : '';
            var mainOut = mainOutEl && mainOutEl.value ? (mainOutEl.value || '').trim() : '';
            var mainCode = mainCodeEl && mainCodeEl.value ? (mainCodeEl.value || '').trim() : '';

            // Cleanup existing extra pairs (only extra-pair nodes) before re-adding
            try {
                var inStack = row.querySelector('.time-stack');
                if (inStack) {
                    var extras = Array.prototype.slice.call(inStack.querySelectorAll('.time-pair.extra-pair'));
                    extras.forEach(function (n) { n.parentNode && n.parentNode.removeChild(n); });
                }
                var outStacks = row.querySelectorAll('.time-stack');
                var outStack = (outStacks && outStacks.length > 1) ? outStacks[1] : outStacks[0];
                if (outStack) {
                    var extrasOut = Array.prototype.slice.call(outStack.querySelectorAll('.time-pair.extra-pair'));
                    extrasOut.forEach(function (n) { n.parentNode && n.parentNode.removeChild(n); });
                }
                var codeStack = row.querySelector('.code-stack');
                if (codeStack) {
                    var extrasCode = Array.prototype.slice.call(codeStack.querySelectorAll('.code-pair.extra-code-pair'));
                    extrasCode.forEach(function (n) { n.parentNode && n.parentNode.removeChild(n); });
                }
            } catch (e) { console.warn('renderExtrasForRowCanonical cleanup failed', e); }

            // Build filtered list to re-render: skip entries equal to main and skip fully empty
            var filtered = [];
            for (var i = 0; i < tokens.length; i++) {
                var t = tokens[i];
                // compare raw strings after trimming
                var tIn = (t.in || '').trim();
                var tOut = (t.out || '').trim();
                var tCode = (t.code || '').trim();
                // skip empty
                if (!tIn && !tOut && !tCode) continue;
                // skip if equals main
                if (tIn === mainIn && tOut === mainOut && tCode === mainCode) continue;
                filtered.push({ in: tIn, out: tOut, code: tCode });
            }

            // Render each filtered token into the row using createExtraPairNodes
            for (var k = 0; k < filtered.length; k++) {
                var item = filtered[k];
                try {
                    // createExtraPairNodes expects raw values (HH:mm) and will create inputs + select
                    if (typeof window.createExtraPairNodes === 'function') {
                        window.createExtraPairNodes(row, studentId, { inVal: item.in, outVal: item.out, codeValue: item.code });
                    } else {
                        // fallback: create a simple li (if you used fallback renderer earlier)
                        var extraDiv = document.getElementById('extraRow_' + studentId);
                        if (extraDiv) {
                            var li = document.createElement('li');
                            li.textContent = (item.in || '-') + ' — ' + (item.out || '-') + ' (code: ' + (item.code || '-') + ')';
                            extraDiv.appendChild(li);
                        }
                    }
                } catch (ex) {
                    console.error('renderExtrasForRowCanonical: createExtraPairNodes failed', ex, item);
                }
            }

            // Optional: rewrite canonical encoded hidden to avoid mixed formats later
            try {
                var canonical = buildExtrasStringEncoded(filtered);
                if (hid) hid.value = canonical;
                else {
                    // create hidden if not present
                    var newH = document.createElement('input');
                    newH.type = 'hidden';
                    newH.id = 'hidExtraTimes_' + studentId;
                    newH.name = 'hidExtraTimes_' + studentId;
                    newH.value = canonical;
                    // append to last cell
                    var lastTd = row.querySelector('td:last-child') || row;
                    lastTd.appendChild(newH);
                }
            } catch (e) {
                console.warn('renderExtrasForRowCanonical could not rewrite hidden', e);
            }
        }

        function normalizeAllExtras() {
            var hidEls = document.querySelectorAll('input[type=hidden][id^="hidExtraTimes_"]');
            hidEls = Array.prototype.slice.call(hidEls);
            hidEls.forEach(function (h) {
                var studentId = (h.id || '').replace(/^hidExtraTimes_/, '');
                if (!studentId) return;
                var raw = h.value || '';
                if (!raw) return;
                var parts = raw.split(';');
                var normalized = [];
                parts.forEach(function (p) {
                    var seg = p || '';
                    try { seg = decodeURIComponent(seg); } catch (e) { /* ignore */ }
                    var arr = seg.split('|'); while (arr.length < 3) arr.push('');
                    // trim
                    arr = arr.map(function (x) { return (x || '').trim(); });
                    if (!arr[0] && !arr[1] && !arr[2]) return; // skip empty
                    // skip token if matches main inputs (avoid duplicate)
                    var row = document.querySelector('[data-studentid="' + studentId + '"]') || document.getElementById('row_' + studentId);
                    var mainIn = row && row.querySelector('input.in-time') ? (row.querySelector('input.in-time').value || '').trim() : '';
                    var mainOut = row && row.querySelector('input.out-time') ? (row.querySelector('input.out-time').value || '').trim() : '';
                    var mainCode = row && row.querySelector('select[id$=\"ddlCode\"]') ? (row.querySelector('select[id$=\"ddlCode\"]').value || '').trim() : '';
                    if (arr[0] === mainIn && arr[1] === mainOut && arr[2] === mainCode) return;
                    normalized.push(encodeURIComponent(arr[0]) + '|' + encodeURIComponent(arr[1]) + '|' + encodeURIComponent(arr[2]));
                });
                var canonical = normalized.join(';');
                h.value = canonical;
                // call your renderer to re-create UI (it will decode)
                try { window.renderExtrasForRow && window.renderExtrasForRow(null, studentId); } catch (e) { console.error(e); }
            });
        }

        if (document.readyState !== 'loading') {
            normalizeAllExtras();
        } else {
            document.addEventListener('DOMContentLoaded', normalizeAllExtras);
        }

        (function initExtrasNormalizer() {
            function runNormalize() {
                try {
                    if (typeof normalizeAllExtras === 'function') {
                        // small delay so UpdatePanel DOM replacement is complete
                        setTimeout(function () { try { normalizeAllExtras(); } catch (e) { console && console.log && console.log('normalizeAllExtras failed', e); } }, 30);
                    } else {
                        // debug - remove or comment out later
                        if (console && console.log) console.log('normalizeAllExtras not defined yet');
                    }
                } catch (e) { }
            }

            // 1) full-page load (DOMContentLoaded or already-loaded)
            if (document.readyState !== 'loading') {
                runNormalize();
            } else {
                document.addEventListener('DOMContentLoaded', runNormalize);
            }

            // 2) MS AJAX partial postbacks (UpdatePanel)
            try {
                if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    prm.add_endRequest(function (sender, args) {
                        runNormalize();
                    });
                }
            } catch (e) {
                // ignore when MS AJAX not present
            }
        })();
        









        (function () {
            // state kept inside the closure
            var outsideClickHandler = null;
            var calendarClickDelegationAttached = false;
            var prMHooked = false;
            var log = false; // set to true while debugging

            function getPnl() {
                try { return document.getElementById('<%= pnlCalendar.ClientID %>'); } catch (e) { return null; }
    }
            function getBtn() {
                try { return document.getElementById('<%= btnEditPastDates.ClientID %>'); } catch (e) { return null; }
    }

            function isVisible(el) {
                if (!el) return false;
                try {
                    var cs = window.getComputedStyle ? getComputedStyle(el) : null;
                    if (cs) {
                        if (cs.display === 'none' || cs.visibility === 'hidden' || cs.opacity === '0') return false;
                        return el.offsetWidth > 0 && el.offsetHeight > 0;
                    }
                    return el.style && el.style.display !== 'none';
                } catch (e) { return el && el.style && el.style.display !== 'none'; }
            }

            function loader() {
                try { return document.getElementById('loader'); } catch (e) { return null; }
            }

            function showLoader() {
                try {
                    var l = loader();
                    if (!l) return;
                    l.style.display = 'block';
                } catch (e) { if (console) console.log('showLoader error', e); }
            }
            function hideLoader() {
                try {
                    var l = loader();
                    if (!l) return;
                    try { l.style.setProperty('display', 'none', 'important'); } catch (e) { l.style.display = 'none'; }
                } catch (e) { if (console) console.log('hideLoader error', e); }
            }

            // Delegate clicks inside the calendar panel to show the loader before a postback.
            function attachCalendarClickForExistingLoader() {
                try {
                    if (calendarClickDelegationAttached) return;
                    var p = getPnl();
                    if (!p) return;
                    p.addEventListener('click', function (ev) {
                        try {
                            var t = ev.target || ev.srcElement;
                            while (t && t !== p && t.tagName !== 'A') t = t.parentNode;
                            if (!t || t === p) return;

                            // only show loader for numeric day anchors (avoid month nav)
                            var txt = (t.textContent || t.innerText || '').trim();
                            var hasNumeric = /^\d{1,2}$/.test(txt) || (t.querySelector && (function () {
                                var s = t.querySelector('span,div');
                                return s && /^\d{1,2}$/.test((s.textContent || s.innerText || '').trim());
                            })());
                            if (!hasNumeric) {
                                return;
                            }

                            // show the existing page loader before the postback
                            showLoader();
                        } catch (e) { if (console) console.log('calendar click delegation error', e); }
                    }, true);
                    calendarClickDelegationAttached = true;
                } catch (e) { if (console) console.log('attachCalendarClickForExistingLoader failed', e); }
            }

            function removeOutsideClickHandler() {
                try {
                    if (outsideClickHandler) {
                        document.removeEventListener('click', outsideClickHandler, false);
                        outsideClickHandler = null;
                    }
                } catch (e) { }
            }

            function attachOutsideClickHandlerDelayed() {
                removeOutsideClickHandler();
                setTimeout(function () {
                    outsideClickHandler = function (ev) {
                        try {
                            var pnl = getPnl();
                            var btn = getBtn();
                            var target = ev.target || ev.srcElement;
                            if (!pnl || !target) return;
                            if (pnl.contains(target)) return;
                            if (btn && btn.contains(target)) return;
                            // click outside -> hide
                            try { window.hideCalendarPopup && window.hideCalendarPopup(); } catch (ex) { }
                        } catch (err) { }
                    };

                    // attach in bubble phase so button's own click runs first
                    document.addEventListener('click', outsideClickHandler, false);
                }, 120);
            }

            // Hook into MS AJAX PageRequestManager if present, but do it safely and only once.
            function hookPageRequestManagerOnce() {
                try {
                    if (prMHooked) {  }
                    if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        if (prm) {
                            prm.add_beginRequest(function () {
                                try { showLoader(); } catch (e) { }
                            });
                            prm.add_endRequest(function () {
                                try { setTimeout(function () { hideLoader(); }, 200); } catch (e) { }
                            });
                            prMHooked = true;
                        }
                    }
                } catch (e) { }
            }

            // Initialize wiring (run now and also attempt wiring after partial updates)
            function init() {
                try {
                    attachCalendarClickForExistingLoader();
                    attachOutsideClickHandlerDelayed();
                    hookPageRequestManagerOnce();
                } catch (e) {  }
            }

            // Run when DOM ready (supports interactive/complete)
            if (document.readyState === 'complete' || document.readyState === 'interactive') {
                init();
            } else {
                document.addEventListener('DOMContentLoaded', init);
            }

            // Re-run init after ASP.NET partial updates to reattach handlers if DOM nodes were replaced
            try {
                if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    var prm2 = Sys.WebForms.PageRequestManager.getInstance();
                    if (prm2) {
                        prm2.add_endRequest(function () {
                            // small delay to allow DOM replacement to settle
                            setTimeout(function () {
                                // reattach (functions are defensive to avoid duplicates)
                                attachCalendarClickForExistingLoader();
                                attachOutsideClickHandlerDelayed();
                            }, 60);
                        });
                    }
                }
            } catch (e) { }




            function removeOutsideClickHandler() {
                if (outsideClickHandler) {
                    try {
                        document.removeEventListener('click', outsideClickHandler, false);
                    } catch (e) { }
                    outsideClickHandler = null;
                }
            }

            // Wire MS Ajax PRM once: show loader on beginRequest, cleanup on endRequest
            (function hookPageRequestManager() {
                try {
                    if (prMHooked) return;
                    if (typeof (Sys) !== 'undefined' && Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        if (!prm) return;
                        prm.add_beginRequest(function () {
                            try { showLoader(); } catch (e) { }
                        });
                        prm.add_endRequest(function () {
                            try {
                                // hide loader and clean up handlers: server may have re-rendered panel/button nodes
                                try { hideLoader(); } catch (e) { }
                                removeOutsideClickHandler();
                                // If server re-rendered the panel/button, re-attach calendar click delegation on the new node
                                try { calendarClickDelegationAttached = false; /* force re-attach if needed */ } catch (e) { }
                                // Re-enable the Edit Past Dates button (ASP.NET sometimes leaves it disabled after async postback)
                                try {
                                    var b = getBtn();
                                    if (b) { b.disabled = false; b.removeAttribute && b.removeAttribute('disabled'); }
                                } catch (e) { }
                                // Ensure popup hidden (server may have processed selection)
                                try { window.hideCalendarPopup && window.hideCalendarPopup(); } catch (e) { }
                            } catch (ex) { }
                        });
                        prMHooked = true;
                    }
                } catch (ex) { if (console) console.log('page request manager hook failed', ex); }
            })();

            // ESC closes popup
            document.addEventListener('keydown', function (ev) {
                if (ev.key === 'Escape' || ev.keyCode === 27) {
                    try { window.hideCalendarPopup && window.hideCalendarPopup(); } catch (e) { }
                }
            });

            // Expose functions to be sure other code can call them if needed
            // (optional) ensure we don't overwrite existing implementations if present
            if (!window.attachCalendarClickForExistingLoader) {
                window.attachCalendarClickForExistingLoader = function () {
                    // simplistic re-run (will be guarded by calendarClickDelegationAttached)
                    try {
                        calendarClickDelegationAttached = false;
                        (function () {
                            var p = document.getElementById('<%= pnlCalendar.ClientID %>');
                      if (!p) return;
                      p.addEventListener('click', function (ev) {
                          try {
                              var t = ev.target || ev.srcElement;
                              while (t && t !== p && t.tagName !== 'A') t = t.parentNode;
                              if (!t || t === p) return;
                              showLoader();
                          } catch (e) { if (console) console.log('calendar click delegation error', e); }
                      }, true);
                      calendarClickDelegationAttached = true;
                  })();
            } catch (e) { }
        };
    }             })();















        function setFixedDate(dateStr) {
            // store it in a global so your clock display knows what date to show
            window.fixedDate = dateStr;
        }









        if (typeof __doPostBack !== 'function') {
            function __doPostBack(eventTarget, eventArgument) {
                try {
                    var theForm = document.forms && document.forms[0];
                    if (!theForm) return;
                    var t = document.getElementById('__EVENTTARGET');
                    var a = document.getElementById('__EVENTARGUMENT');
                    if (!t) {
                        t = document.createElement('input');
                        t.type = 'hidden';
                        t.id = '__EVENTTARGET';
                        t.name = '__EVENTTARGET';
                        theForm.appendChild(t);
                    }
                    if (!a) {
                        a = document.createElement('input');
                        a.type = 'hidden';
                        a.id = '__EVENTARGUMENT';
                        a.name = '__EVENTARGUMENT';
                        theForm.appendChild(a);
                    }
                    t.value = eventTarget || '';
                    a.value = eventArgument || '';
                    theForm.submit();
                } catch (e) {
                    console && console.warn && console.warn('fallback __doPostBack failed', e);
                }
            }
        }


        function syncCalendarFromInput() {
            try {
                var input = document.getElementById('<%= hidPastDate.ClientID %>');
                var cal = document.getElementById('<%= calPast.ClientID %>'); // if you use client calendar
                if (!input) return;
                var v = input.value; // expected 'yyyy-mm-dd'
                if (!v) return;
                // convert 'yyyy-mm-dd' to Date object
                var parts = v.split('-');
                if (parts.length === 3) {
                    var d = new Date(parts[0], parts[1] - 1, parts[2]);
                    // call your client calendar API to set visible date/selected date
                    if (typeof setClientCalendarDate === 'function') {
                        setClientCalendarDate(d);
                    }
                    // or set a data attribute for server-side scripts
                    input.dataset.synced = '1';
                }
            } catch (e) { console && console.log && console.log('syncCalendarFromInput', e); }
        }


        (function () {
            // helper: is this an anchor representing a calendar day number?
            function isDayAnchor(a) {
                if (!a) return false;
                var txt = (a.textContent || a.innerText || '').trim();
                if (/^\d{1,2}$/.test(txt)) return true;
                // maybe the number is wrapped in a child span: <a><span>12</span></a>
                var span = a.querySelector && a.querySelector('span');
                if (span && /^\d{1,2}$/.test((span.textContent || span.innerText || '').trim())) return true;
                return false;
            }

            // Extract __doPostBack args from a href like: "javascript:__doPostBack('ctl00$...','V123')"
            function extractDoPostBackArgs(href) {
                if (!href) return null;
                // match __doPostBack('arg1','arg2') — allow optional "javascript:" prefix and whitespace
                var m = href.match(/__doPostBack\(\s*'([^']*)'\s*,\s*'([^']*)'\s*\)/);
                if (m) return { target: m[1], arg: m[2] };
                return null;
            }

            function wireCal() {
                var cal = document.getElementById('<%= calPast.ClientID %>');
      if (!cal) return;

      // Use capture to run before ASP.NET handlers
      cal.addEventListener('click', function (ev) {
          try {
              var t = ev.target || ev.srcElement;
              while (t && t !== p && t.tagName !== 'A') t = t.parentNode;
              if (!t || t === p) return;

              // detect numeric day anchor (1-31) — supports <a>12</a> or <a><span>12</span></a>
              var txt = (t.textContent || t.innerText || '').trim();
              var span = t.querySelector && t.querySelector('span,div');
              var isDay = (/^\d{1,2}$/.test(txt)) || (span && /^\d{1,2}$/.test((span.textContent || span.innerText || '').trim()));
              if (!isDay) {
                  return;
              }

              // show loader immediately
              try { showLoader(); } catch (e) {}

              // prevent the default client-side anchor behavior that opens/toggles the calendar UI
              // BUT still perform the postback explicitly if href contains __doPostBack
              try {
                  var href = t.getAttribute('href') || '';
                  if (href.indexOf('__doPostBack') !== -1) {
                      ev.preventDefault(); // stop default navigation / calendar UI toggle
                      var m = href.match(/__doPostBack\(\s*'([^']*)'\s*,\s*'([^']*)'\s*\)/);
                      if (m && typeof __doPostBack === 'function') {
                          __doPostBack(m[1], m[2]);
                      } else {
                          // fallback: eval only if parsing failed
                          try { eval(href); } catch (ex) { }
                      }
                  }
                      // If href does not contain __doPostBack, we still prevented default; but we want the postback.
                      // Try to find onclick that triggers postback or let the normal flow continue:
                  else {
                      // Try to find an inline onclick that calls __doPostBack
                      var onclick = t.getAttribute('onclick') || '';
                      var m2 = onclick.match(/__doPostBack\(\s*'([^']*)'\s*,\s*'([^']*)'\s*\)/);
                      if (m2 && typeof __doPostBack === 'function') {
                          ev.preventDefault();
                          __doPostBack(m2[1], m2[2]);
                      } else {
                          // If nothing found, do not prevent further — allow default (but we already called preventDefault())
                          // To be safe, attempt a synthetic click on the anchor after a tiny delay (so we don't re-open calendar synchronously)
                          setTimeout(function () {
                              try { if (t.click) t.click(); } catch (e) { }
                          }, 20);
                      }
                  }
              } catch (postEx) {  }
          } catch (e) {
          }
      }, true);
  }

            if (document.readyState === 'complete' || document.readyState === 'interactive') {
                wireCal();
            } else {
                document.addEventListener('DOMContentLoaded', wireCal);
            }

            // optionally: ensure loader gets hidden after partial postbacks (if Sys.WebForms is present)
            try {
                if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    if (prm) {
                        prm.add_endRequest(function () { try { setTimeout(function () { if (window.hideLoader) hideLoader(); }, 200); } catch (e) { } });
                    }
                }
            } catch (e) { /* ignore */ }


        })();

        








        (function () {
            // CONFIG: set to your real IDs if different
            window.__dateDisplayId = window.__dateDisplayId || 'currentDateTime';
            window.__hidPastDateId = window.__hidPastDateId || 'hidPastDate';

            // live clock control flag (used by existing code elsewhere)
            window.allowClockUpdate = (typeof window.allowClockUpdate === 'boolean') ? window.allowClockUpdate : true;

            // helper: find hidden input robustly (works across ASP.NET ClientID mangling)
            function findHidden() {
                var h = document.getElementById(window.__hidPastDateId);
                if (h) return h;
                h = document.querySelector('input[type=hidden][name$=\"hidPastDate\"]');
                if (h) return h;
                return document.querySelector('input[type=hidden][id*=\"hidPastDate\"], input[type=hidden][name*=\"hidPastDate\"]');
            }

            // format date nicely for display
            function fmtIso(iso) {
                try {
                    if (!iso) return '';
                    var d = new Date(iso);
                    if (isNaN(d)) return iso;
                    return d.toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'long', day: '2-digit' });
                } catch (e) { return iso; }
            }

            // main updater: sets the on-page timer/display according to hidden selected date or live clock
            window.updateTimerFromHidden = function (force) {
                try {
                    var hid = findHidden();
                    var iso = hid ? (hid.value || '').trim() : '';
                    var el = document.getElementById(window.__dateDisplayId) || document.querySelector('.current-date-display') || null;
                    if (!el) return;

                    if (iso) {
                        // if a past date is selected, show the selected date and stop live clock
                        el.innerHTML = fmtIso(iso);
                        try { el.classList.add('past-selected-date'); } catch (e) { }
                        try { el.style.color = 'red'; } catch (e) { }
                        window.allowClockUpdate = false;
                        // also cache last selected
                        window._lastSelectedIso = iso;
                    } else {
                        // no past date selected -> resume live clock
                        try { el.classList.remove('past-selected-date'); } catch (e) { }
                        try { el.style.color = ''; } catch (e) { }
                        window.allowClockUpdate = true;
                        // if there's a refreshClockNow function use it; otherwise write current local time
                        if (typeof refreshClockNow === 'function') {
                            try { refreshClockNow(); } catch (e) { el.innerHTML = new Date().toLocaleString(); }
                        } else {
                            el.innerHTML = new Date().toLocaleString();
                        }
                        window._lastSelectedIso = null;
                    }
                } catch (err) {
                    console && console.log && console.log('updateTimerFromHidden error', err);
                }
            };

            //window.refreshClockNow = window.refreshClockNow || function () {
            //    try {
            //        if (window.allowClockUpdate === false) return;
            //        var el = document.getElementById(window.__dateDisplayId || 'currentDateTime');
            //        if (!el) return;
            //        var now = new Date();
            //        var formatted = now.toLocaleString(undefined, {
            //            weekday: 'long', year: 'numeric', month: 'long', day: '2-digit',
            //            hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: true
            //        });
            //        el.innerHTML = formatted;
            //    } catch (e) { console && console.log && console.log('refreshClockNow error', e); }
            //};

            // interval loop that updates clock if allowed
            if (typeof window._clockInterval === 'undefined') {
                window._clockInterval = setInterval(function () {
                    try {
                        if (window.allowClockUpdate !== false && typeof refreshClockNow === 'function') {
                            refreshClockNow();
                        }
                    } catch (e) { }
                }, 1000);
            }

            // allow server to set hidden + immediately update
            window.setHiddenPastDateIso = function (iso) {
                try {
                    var hid = findHidden();
                    if (hid) hid.value = iso || '';
                    window.updateTimerFromHidden(true);
                } catch (e) { console && console.log && console.log('setHiddenPastDateIso error', e); }
            };

            // On load run once
            if (document.readyState !== 'loading') updateTimerFromHidden(true);
            else document.addEventListener('DOMContentLoaded', function () { updateTimerFromHidden(true); }, false);

            // Ensure update runs after every MS AJAX UpdatePanel partial postback
            try {
                if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                        try { updateTimerFromHidden(true); } catch (e) { console && console.log && console.log('endRequest updateTimer error', e); }
                    });
                }
            } catch (e) { /* ignore */ }

            // Also if you have a periodic client clock loop, make it respect window.allowClockUpdate
            // Example replacement for such a loop (if you already have one, adapt it):
            if (typeof window._clockInterval === 'undefined') {
                window._clockInterval = setInterval(function () {
                    try {
                        if (window.allowClockUpdate !== false && typeof refreshClockNow === 'function') {
                            refreshClockNow();
                        }
                    } catch (e) { }
                }, 1000);
            }
        })();

















        if (!window.__studentCheckinHelpersInstalled) (function () {
            window.__studentCheckinHelpersInstalled = true;

            // CONFIG - id of the element that displays the current date/time
            window.__dateDisplayId = window.__dateDisplayId || 'currentDateTime';

            // ----- Loader helpers -----
            // showLoader('Loading...') and hideLoader()
            window.__loaderFallbackTimeout = null;

            window.showLoader = window.showLoader || function (msg) {
                try {
                    var id = 'globalPageLoader';
                    var el = document.getElementById(id);
                    if (!el) {
                        el = document.createElement('div');
                        el.id = id;
                        el.style.position = 'fixed';
                        el.style.left = '0';
                        el.style.top = '0';
                        el.style.right = '0';
                        el.style.bottom = '0';
                        el.style.zIndex = '65535';
                        el.style.display = 'flex';
                        el.style.alignItems = 'center';
                        el.style.justifyContent = 'center';
                        el.style.background = 'rgba(0,0,0,0.35)';
                        el.style.fontFamily = 'Segoe UI, Arial, sans-serif';
                        el.style.color = '#fff';
                        el.style.fontSize = '14px';

                        var inner = document.createElement('div');
                        inner.style.background = 'rgba(0,0,0,0.6)';
                        inner.style.padding = '12px 18px';
                        inner.style.borderRadius = '6px';
                        inner.style.boxShadow = '0 2px 8px rgba(0,0,0,0.3)';
                        inner.style.display = 'flex';
                        inner.style.alignItems = 'center';
                        inner.style.gap = '12px';

                        var spinner = document.createElement('div');
                        spinner.className = 'simple-loader-spinner';
                        spinner.style.width = '18px';
                        spinner.style.height = '18px';
                        spinner.style.border = '3px solid rgba(255,255,255,0.2)';
                        spinner.style.borderTopColor = '#fff';
                        spinner.style.borderRadius = '50%';
                        spinner.style.animation = 'spin 1s linear infinite';

                        var msgEl = document.createElement('div');
                        msgEl.className = 'loader-msg';
                        msgEl.textContent = msg || 'Loading…';

                        inner.appendChild(spinner);
                        inner.appendChild(msgEl);
                        el.appendChild(inner);

                        // minimal spinner CSS (insert once)
                        var style = document.createElement('style');
                        style.id = 'globalLoaderStyle';
                        style.appendChild(document.createTextNode(
                          '@keyframes spin { from { transform: rotate(0deg); } to { transform: rotate(360deg); } }'
                        ));
                        document.head.appendChild(style);

                        document.body.appendChild(el);
                    } else {
                        var msgEl = el.querySelector('.loader-msg');
                        if (msgEl) msgEl.textContent = msg || 'Loading…';
                        el.style.display = 'flex';
                    }

                    // safety fallback: auto-hide after 20s to avoid indefinite stuck loader
                    if (window.__loaderFallbackTimeout) clearTimeout(window.__loaderFallbackTimeout);
                    window.__loaderFallbackTimeout = setTimeout(function () {
                        try { window.hideLoader && window.hideLoader(); } catch (e) { }
                    }, 20000);
                } catch (e) { console && console.log && console.log('showLoader error', e); }
            };

            window.hideLoader = window.hideLoader || function () {
                try {
                    var el = document.getElementById('globalPageLoader');
                    if (el) {
                        el.style.display = 'none';
                    }
                    if (window.__loaderFallbackTimeout) {
                        clearTimeout(window.__loaderFallbackTimeout);
                        window.__loaderFallbackTimeout = null;
                    }
                } catch (e) { console && console.log && console.log('hideLoader error', e); }
            };

            // ----- Clock helper -----
            // exact format: M/d/yyyy h:mm:ss tt  (e.g. 9/19/2025 8:36:00 PM)
            //window.refreshClockNow = window.refreshClockNow || function () {
            //    try {
            //        // don't update if paused for a past-date view
            //        if (window.allowClockUpdate === false) return;

            //        var id = window.__dateDisplayId || 'currentDateTime';
            //        var el = document.getElementById(id);
            //        if (!el) return;

            //        var now = new Date();

            //        function pad(v, len) {
            //            v = String(v); while (v.length < (len || 2)) v = '0' + v; return v;
            //        }

            //        var month = now.getMonth() + 1; // 1-12
            //        var day = now.getDate(); // 1-31
            //        var year = now.getFullYear();

            //        var hour24 = now.getHours();
            //        var ampm = hour24 >= 12 ? 'PM' : 'AM';
            //        var hour12 = hour24 % 12;
            //        if (hour12 === 0) hour12 = 12;

            //        var minute = pad(now.getMinutes(), 2);
            //        var second = pad(now.getSeconds(), 2);

            //        var formatted = month + '/' + day + '/' + year + ' ' + hour12 + ':' + minute + ':' + second + ' ' + ampm;

            //        el.innerHTML = formatted;
            //    } catch (e) { console && console.log && console.log('refreshClockNow error', e); }
            //};

            // small tick loop that respects window.allowClockUpdate
            if (typeof window.__clockIntervalId === 'undefined') {
                window.__clockIntervalId = setInterval(function () {
                    try {
                        if (window.allowClockUpdate !== false) refreshClockNow();
                    } catch (e) { }
                }, 1000);
            }

            // small helper used by server scripts to set hid + update immediately
            window.setHiddenPastDateIso = window.setHiddenPastDateIso || function (iso) {
                try {
                    var hid = document.getElementById('hidPastDate') || document.querySelector('input[type=hidden][name$=\"hidPastDate\"]');
                    if (hid) hid.value = iso || '';
                    // if iso empty -> resume clock immediately
                    if (!iso) {
                        window.allowClockUpdate = true;
                        if (typeof refreshClockNow === 'function') refreshClockNow();
                    } else {
                        // if iso present, show date (server often uses separate script to display date text)
                        window.allowClockUpdate = false;
                    }
                } catch (e) { console && console.log && console.log('setHiddenPastDateIso error', e); }
            };

            // run once to initialize display
            try { refreshClockNow(); } catch (e) { }
        })();

        (function () {
            // cached last position
            window._lastCalendarPosition = window._lastCalendarPosition || null;

            // call when showing popup to remember its screen coords
            window.rememberCalendarPosition = function () {
                try {
                    var cal = document.querySelector('.calendar-popup');
                    if (!cal) return;
                    var rect = cal.getBoundingClientRect();
                    window._lastCalendarPosition = {
                        left: Math.round(rect.left + window.pageXOffset),
                        top: Math.round(rect.top + window.pageYOffset)
                    };
                } catch (e) { console && console.log && console.log('rememberCalendarPosition', e); }
            };

            // call after partial postback to apply saved coords to the (new) calendar popup
            window.restoreCalendarPosition = function () {
                try {
                    if (!window._lastCalendarPosition) return;
                    var cal = document.querySelector('.calendar-popup');
                    if (!cal) return;
                    // ensure cal is inside server form so postbacks work
                    try {
                        var pageForm = document.getElementById('<%= this.Form.ClientID %>') || document.forms[0] || document.body;
            if (cal.parentNode !== pageForm) pageForm.appendChild(cal);
        } catch (e) { /* ignore */ }

        cal.style.position = 'absolute';
        cal.style.left = '568px';
        cal.style.top = '68px';
        cal.style.zIndex = '30000';
        // show it (no visibility)
        try { cal.style.setProperty('display', 'block', 'important'); } catch (e) { cal.style.display = 'block'; }
        try { cal.style.setProperty('opacity', '1', 'important'); } catch (e) { cal.style.opacity = '1'; }
        try { cal.style.setProperty('pointer-events', 'auto', 'important'); } catch (e) { cal.style.pointerEvents = 'auto'; }
    } catch (e) { console && console.log && console.log('restoreCalendarPosition', e); }
  };

            // ensure restore runs after any UpdatePanel partial postback
            try {
                if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                        try { window.restoreCalendarPosition(); } catch (e) { }
                    });
                }
            } catch (e) { }

            // make showCalendarPopup call rememberCalendarPosition() before moving/positioning
            // (if you already have showCalendarPopup, add a call to rememberCalendarPosition() near top)
        })();




        function toggleCalendarPopup() {
            try {
                // attempt to find the upCalendar instance first
                var up = document.getElementById('<%= upCalendar.ClientID %>') || document.getElementById('upCalendar');
      var cal = null;
      if (up) cal = up.querySelector('#' + ('<%= pnlCalendar.ClientID %>')) || up.querySelector('.calendar-popup');
    if (!cal) cal = document.getElementById('<%= pnlCalendar.ClientID %>') || document.querySelector('.calendar-popup');

      // detect visible state robustly (use dataset flag if available)
      var visible = false;
      try {
          if (cal) {
              if (cal.dataset && cal.dataset.calendarPersistVisible === '1') visible = true;
              else {
                  var cs = window.getComputedStyle(cal);
                  visible = (cs && cs.display !== 'none' && cs.visibility !== 'hidden' && parseFloat(cs.opacity || 1) > 0);
              }
          }
      } catch (e) { /* ignore */ }

      if (visible) {
          // if calendar is visible, hide it
          if (typeof hideCalendarPopup === 'function') {
              hideCalendarPopup();
          } else {
              // fallback direct hide
              try { if (cal) { cal.style.display = 'none'; cal.style.visibility = 'hidden'; cal.style.pointerEvents = 'none'; } } catch (e) { }
          }
      } else {
          // not visible -> show. Prefer the show function if present.
          if (typeof showCalendarPopup === 'function') {
              showCalendarPopup();
          } else {
              // fallback: direct show and position near button
              try {
                  if (cal) {
                      cal.style.display = 'block';
                      cal.style.visibility = 'visible';
                      cal.style.pointerEvents = 'auto';
                      cal.style.opacity = '1';
                      cal.style.position = 'absolute';
                      cal.style.zIndex = '30000';
                      var btn = document.getElementById('<%= btnEditPastDates.ClientID %>');
            if (btn) {
                var r = btn.getBoundingClientRect();
                cal.style.left = '568px';
                cal.style.top = '68px';
            }
            if (cal.dataset) cal.dataset.calendarPersistVisible = '1';
        }
    } catch (e) { }
}
}

    return false; // prevent postback
} catch (err) {
    console && console.error && console.error('toggleCalendarPopup error', err);
    return false;
}
}















        window.showSaveLoader = window.showSaveLoader || function (msg) {
            try {
                // Prefer a save-specific overlay if your app uses one
                if (typeof showSaveOverlay === 'function') {
                    showSaveOverlay(msg);
                    return;
                }
                // otherwise fall back to the existing generic showLoader
                if (typeof showLoader === 'function') {
                    showLoader(msg);
                    return;
                }
                // minimal fallback: set a small inline element if present
                var el = document.getElementById('saveLoaderFallback');
                if (!el) {
                    el = document.createElement('div');
                    el.id = 'saveLoaderFallback';
                    el.style.position = 'fixed';
                    el.style.left = '50%';
                    el.style.top = '40%';
                    el.style.transform = 'translate(-50%, -50%)';
                    el.style.zIndex = '99999';
                    el.style.padding = '12px 18px';
                    el.style.borderRadius = '6px';
                    el.style.boxShadow = '0 2px 8px rgba(0,0,0,0.25)';
                    el.style.background = 'white';
                    el.style.fontFamily = 'sans-serif';
                    el.style.fontSize = '13px';
                    document.body.appendChild(el);
                }
                el.textContent = msg || 'Saving…';
                el.style.display = 'block';
            } catch (e) { console && console.log && console.log('showSaveLoader error', e); }
        };

        window.hideSaveLoader = window.hideSaveLoader || function () {
            try {
                if (typeof hideSaveOverlay === 'function') { hideSaveOverlay(); return; }
                if (typeof hideLoader === 'function') { hideLoader(); return; }
                var el = document.getElementById('saveLoaderFallback');
                if (el) el.style.display = 'none';
            } catch (e) { console && console.log && console.log('hideSaveLoader error', e); }
        };





        if (!window.__studentCheckinHelpersAdded) {
            window.__studentCheckinHelpersAdded = true;

            // Client fillStudent(dateIso) — wrapper to reload grid for a date.
            // Replace the inner implementation if your page reloads the grid via different mechanism (AJAX/postback).
            function fillStudent(dateIso) {
                try {
                    // prefer explicit param -> hidden field -> today
                    var iso = dateIso;
                    if (!iso) {
                        var hf = document.getElementById('<%= hidPastDate.ClientID %>') || document.getElementById('hidPastDate') || document.getElementById('hfSelectedDate');
                if (hf && hf.value) iso = hf.value;
            }
            if (!iso) iso = (new Date()).toISOString().slice(0, 10);

            // If you use __doPostBack to trigger server rebind, do that and ensure server reads hidPastDate.
            // Example: set the hidden field and call a postback event or call the server callback that you normally use.
            var hf2 = document.getElementById('<%= hidPastDate.ClientID %>') || document.getElementById('hidPastDate');
            if (hf2) hf2.value = iso;

            // If page uses a callback function or postback to reload the grid, call it here.
            // Common patterns:
            // 1) If you have a client function reloadGrid() that triggers update: reloadGrid();
            // 2) If the grid is bound on server you can postback:
            //    __doPostBack('<%= grdGroup.UniqueID %>','reload');
            // 3) If you were previously calling a function with no param, call it with iso:
            if (typeof window._doFillStudentClient === 'function') {
                // some pages define a client-side wrapper — prefer that
                window._doFillStudentClient(iso);
            } else if (typeof __doPostBack === 'function') {
                // fallback: trigger a postback that your server-side code will interpret to rebind.
                // Use a distinct eventTarget/eventArg if needed by your code — adapt if you have a known target.
                __doPostBack('<%= Page.UniqueID %>', 'fillStudent|' + iso);
            } else {
                // last resort: redirect to same page with query string (less desirable)
                // window.location = window.location.pathname + '?date=' + encodeURIComponent(iso);
                console && console.log && console.log('fillStudent: no reload hook found; set hidPastDate to', iso);
            }
    } catch (e) {
        console && console.log && console.log('fillStudent error', e);
    }
}

            // calendar selection handler — set hidPastDate and call fillStudent
    function onCalendarDateSelected(dateObj) {
        try {
            // dateObj can be a Date or an ISO string 'yyyy-mm-dd'
            var iso;
            if (!dateObj) {
                iso = (new Date()).toISOString().slice(0, 10);
            } else if (typeof dateObj === 'string') {
                iso = dateObj;
            } else {
                iso = (dateObj.getFullYear() + '-' +
                      ('0' + (dateObj.getMonth() + 1)).slice(-2) + '-' +
                      ('0' + dateObj.getDate()).slice(-2));
            }

            var hf = document.getElementById('<%= hidPastDate.ClientID %>') || document.getElementById('hidPastDate');
            if (hf) {
                // clear the hidden field for today's selection to preserve the original "today" semantics
                if (iso === (new Date()).toISOString().slice(0, 10)) hf.value = '';
                else hf.value = iso;
            }

            // reload grid for selected date
            if (typeof fillStudent === 'function') fillStudent(iso);
        } catch (e) {
            console && console.log && console.log('onCalendarDateSelected error', e);
        }
    }

            // showSaveLoader fallback (defensive)
    window.showSaveLoader = window.showSaveLoader || function (msg) {
        try {
            if (typeof showSaveOverlay === 'function') { showSaveOverlay(msg); return; }
            if (typeof showLoader === 'function') { showLoader(msg); return; }
            var el = document.getElementById('saveLoaderFallback');
            if (!el) {
                el = document.createElement('div');
                el.id = 'saveLoaderFallback';
                el.style.position = 'fixed';
                el.style.left = '50%';
                el.style.top = '40%';
                el.style.transform = 'translate(-50%, -50%)';
                el.style.zIndex = '99999';
                el.style.padding = '12px 18px';
                el.style.borderRadius = '6px';
                el.style.boxShadow = '0 2px 8px rgba(0,0,0,0.25)';
                el.style.background = 'white';
                el.style.fontFamily = 'sans-serif';
                el.style.fontSize = '13px';
                document.body.appendChild(el);
            }
            el.textContent = msg || 'Saving…';
            el.style.display = 'block';
        } catch (e) { console && console.log && console.log('showSaveLoader error', e); }
    };

    window.hideSaveLoader = window.hideSaveLoader || function () {
        try {
            if (typeof hideSaveOverlay === 'function') { hideSaveOverlay(); return; }
            if (typeof hideLoader === 'function') { hideLoader(); return; }
            var el = document.getElementById('saveLoaderFallback');
            if (el) el.style.display = 'none';
        } catch (e) { console && console.log && console.log('hideSaveLoader error', e); }
    };
}

        function setHiddenPastDateIso(iso) {
            var hid = document.getElementById('<%= hidPastDate.ClientID %>');
            if (!hid) return;
            // iso should be '' for today or 'yyyy-MM-dd' for other days
            hid.value = iso || '';
            // ensure form knows the value immediately (no extra steps required for normal postback)
        }



        (function () {
            // Replace with server-side client ID
            var pnlId = '<%= pnlCalendar.ClientID %>';

            function findPanel() {
                return document.getElementById(pnlId) || document.querySelector('.calendar-popup');
            }

            window.showCalendarPopup = function () {
                try {
                    var pnl = findPanel();
                    if (!pnl) return;
                    pnl.style.display = 'block';
                    pnl.style.visibility = 'visible';
                    pnl.style.opacity = '1';
                    pnl.style.left = '576px';
                    if (pnl.dataset) pnl.dataset.calendarPersistVisible = '1';

                    // attach one global outside-click handler (only once)
                    if (!window._calendarOutsideHandlerAttached) {
                        window._calendarOutsideHandlerAttached = true;
                        document.addEventListener('click', function (ev) {
                            try {
                                var p = findPanel();
                                if (!p) return;
                                var tgt = ev.target || ev.srcElement;
                                if (!tgt) return;
                                if (p.contains && !p.contains(tgt) && tgt.id !== '<%= btnEditPastDates.ClientID %>') {
                            // clicked outside
                            try { window.hideCalendarPopup && window.hideCalendarPopup(); } catch (e) { }
                        }
                    } catch (e) { }
                }, true); // capture phase helps in some edge cases
            }
        } catch (e) { console && console.log && console.log('showCalendarPopup error', e); }
    };

            window.hideCalendarPopup = function () {
                try {
                    var pnl = findPanel();
                    if (!pnl) return;
                    pnl.style.display = 'none';
                    pnl.style.visibility = 'hidden';
                    pnl.style.opacity = '0';
                    if (pnl.dataset) pnl.dataset.calendarPersistVisible = '0';
                } catch (e) { console && console.log && console.log('hideCalendarPopup error', e); }
            };

            window.toggleCalendarPopup = function () {
                try {
                    var pnl = findPanel();
                    if (!pnl) return;
                    var isVisible = (pnl.dataset && pnl.dataset.calendarPersistVisible === '1') ||
                                    (pnl.style && (pnl.style.display === 'block' || pnl.style.visibility === 'visible'));
                    if (isVisible) window.hideCalendarPopup();
                    else window.showCalendarPopup();
                } catch (e) { console && console.log && console.log('toggleCalendarPopup error', e); }
            };

            // helper: when an UpdatePanel replaces the calendar markup the old element is gone;
            // any code that wants to show popup after async update should call showCalendarPopup(),
            // which will re-query the element and succeed (server also registers showCalendarPopup() calls).
        })();


        (function () {
            function applyCalendarSelection() {
                try {
                    // calendar table id: find element by server-side ClientID pattern
                    var cal = document.getElementById('<%= calPast.ClientID %>');
            var hid = document.getElementById('<%= hidPastDate.ClientID %>');
            if (!cal) return;

            // clear previous selected cells
            var sel = cal.querySelectorAll('td.cal-selected');
            for (var i = 0; i < sel.length; i++) { sel[i].classList.remove('cal-selected'); }

            // determine target date string (yyyy-MM-dd) or empty for today
            var iso = hid && hid.value ? hid.value : '';

            // if empty -> target is today
            var target = iso ? new Date(iso) : new Date();
            // normalize to midnight
            target.setHours(0, 0, 0, 0);

            // calendar renders day number inside anchors or spans - do robust match
            var cells = cal.getElementsByTagName('td');
            for (var i = 0; i < cells.length; i++) {
                var td = cells[i];
                var text = (td.innerText || td.textContent || '').trim();
                if (!text) continue;
                // text could be "1", "2" etc. Compare day number and month context:
                // attempt to read data-date attribute if you render it; else fallback to matching number
                var dayNum = parseInt(text, 10);
                if (!isNaN(dayNum)) {
                    // attempt to get month/year from visible header (best-effort)
                    // fallback: if day cell contains class for other month, skip (ASP.NET calendar adds other-month styling)
                    if (td.classList && td.className.indexOf('other-month') !== -1) continue;
                    // build candidate date using visible month from calendar (parse header)
                    // simple heuristic: find visible month/year in calendar header
                    var header = cal.querySelector('.monthYear') || cal.querySelector('caption') || null;
                    var visYear = null, visMonth = null;
                    if (header) {
                        // try to parse "August 2025" style — best-effort, not bulletproof
                        var htxt = header.innerText || header.textContent;
                        var parts = (htxt || '').trim().split(' ');
                        if (parts.length >= 2) {
                            visMonth = parts[0];
                            visYear = parseInt(parts[1], 10);
                        }
                    }
                    // fallback: assume target is in the month already visible (best-effort)
                    var cand = new Date(target.getFullYear(), target.getMonth(), dayNum);
                    cand.setHours(0, 0, 0, 0);
                    if (cand.getTime() === target.getTime()) {
                        td.classList.add('cal-selected');
                    }
                }
            }
        } catch (e) { console && console.log && console.log('applyCalendarSelection error', e); }
    }

            // Wire to MS AJAX endRequest if present (UpdatePanel partial postbacks)
            if (typeof (Sys) !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (prm) {
                    prm.add_endRequest(function () { applyCalendarSelection(); });
                }
            }

            // also run on initial load
            if (document.readyState === 'complete' || document.readyState === 'interactive') {
                setTimeout(applyCalendarSelection, 10);
            } else {
                window.addEventListener('DOMContentLoaded', function () { setTimeout(applyCalendarSelection, 10); });
            }
        })();










        function onPresenceToggle(elToggle, studentId, classId) {
            try {
                // determine current state
                var currentPresent = true;
                try {
                    var hid = document.getElementById('hidStatus_' + studentId);
                    if (hid && hid.value !== '') {
                        currentPresent = (hid.value === '1' || hid.value.toLowerCase() === 'true');
                    } else {
                        currentPresent = elToggle.classList.contains('att-sw-present');
                    }
                } catch (e) {
                    currentPresent = elToggle.classList.contains('att-sw-present');
                }

                var targetPresent = !currentPresent;
                var row = findClosestRow(elToggle);

                // pick date value safely
                var dateIso = '';
                try {
                    var hidDate = document.getElementById('hidPastDate');
                    if (hidDate && hidDate.value)
                        dateIso = hidDate.value.split('|')[0];
                } catch (e) { dateIso = ''; }

                if (!targetPresent) {
                    // ---------- Switch to ABSENT ----------
                    if (row) disableRowExceptAttendance(row);
                    setRowAbsentVisual(elToggle, studentId);

                    // determine dateIso as usual
                    var dateIso = (document.getElementById('hidPastDate') && document.getElementById('hidPastDate').value) ? document.getElementById('hidPastDate').value.split('|')[0] : '';

                    // show confirm when needed then delete
                    confirmThenDelete(elToggle, studentId, classId, row, dateIso);
                } else {
                    // ---------- Switch to PRESENT ----------
                    // don’t mark present until server confirms
                    try { if (typeof showLoader === 'function') showLoader('Saving...'); } catch (e) { }

                    callWebMethod('InsertCheckinForStudent',
                        { studentId: parseInt(studentId, 10), classId: parseInt(classId, 10), dateIso: dateIso },
                        function (res) {
                            try { if (typeof hideLoader === 'function') hideLoader(); } catch (e) { }

                            if (res && res.success) {
                                // rebind grid to get hidden fields for Add/Save
                                triggerServerRefresh();
                            } else {
                                console.error('Insert failed', res);
                                if (row) disableRowExceptAttendance(row);
                                setRowAbsentVisual(elToggle, studentId);
                                alert('Failed to mark present: ' + (res && res.message ? res.message : 'Unknown error'));
                                triggerServerRefresh();
                            }
                        });
                }
            } catch (ex) {
                console.error('onPresenceToggle unexpected error', ex);
                triggerServerRefresh();
            }
        }

        function getSelectedDateIso() {
            try {
                var hf = document.getElementById('hidPastDate') || document.querySelector('input[id*="hidPastDate"]');
                if (hf && hf.value && hf.value.trim() !== '') {
                    var d = new Date(hf.value);
                    if (!isNaN(d)) return d.toISOString().slice(0, 10);
                    if (/^\d{4}-\d{2}-\d{2}$/.test(hf.value.trim())) return hf.value.trim();
                }
                var cal = document.querySelector('input[id*="selectedDate"], input[id*="hfSelectedDate"], input[id*="calPast"]');
                if (cal && cal.value && /^\d{4}-\d{2}-\d{2}$/.test(cal.value.trim())) return cal.value.trim();
                var t = new Date(); return t.getFullYear() + '-' + String(t.getMonth() + 1).padStart(2, '0') + '-' + String(t.getDate()).padStart(2, '0');
            } catch (e) { var t2 = new Date(); return t2.getFullYear() + '-' + String(t2.getMonth() + 1).padStart(2, '0') + '-' + String(t2.getDate()).padStart(2, '0'); }
        }


        function deleteCheckinAndRefresh(studentId, classId, dateIso) {
            try { if (typeof showLoader === 'function') showLoader('Deleting...'); } catch (e) { }

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'StudentCheckin.aspx/DeleteCheckinForStudent', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');

            xhr.onreadystatechange = function () {
                if (xhr.readyState !== 4) return;
                try { if (typeof hideLoader === 'function') hideLoader(); } catch (e) { }

                if (xhr.status >= 200 && xhr.status < 300) {
                    var res = null;
                    try { res = JSON.parse(xhr.responseText); } catch (e) { console.error('parse error', e); }
                    var data = (res && res.d) ? res.d : res;
                    if (data && data.success) {
                        // server confirmed delete -> now request server to rebind full grid
                        triggerServerRefresh();
                    } else {
                        alert('Delete failed: ' + (data && data.message ? data.message : 'Unknown error'));
                        // revert optimistic UI if necessary (full refresh next)
                        triggerServerRefresh();
                    }
                } else {
                    alert('Delete failed (HTTP ' + xhr.status + ')');
                    triggerServerRefresh();
                }
            };

            var payload = JSON.stringify({ studentId: studentId, classId: classId, dateIso: dateIso || '' });
            xhr.send(payload);
        }

        function triggerServerRefresh() {
            try {
                var unique = '<%= btnRefreshGrid.UniqueID %>';
                if (typeof __doPostBack === 'function' && unique) { __doPostBack(unique, ''); return; }
                var b = document.getElementById('btnRefreshGrid');
                if (b && typeof b.click === 'function') b.click();
            } catch (e) { console.error('triggerServerRefresh error', e); }
        }

        function findClosestRow(el) {
            var cur = el;
            while (cur) {
                if (cur.tagName && cur.tagName.toLowerCase() === 'tr') return cur;
                cur = cur.parentElement;
            }
            return null;
        }

        function markRowAbsentVisual(row) {
            try {
                // add class and disable inputs/buttons in this row
                row.classList.add('row-absent');
                var inputs = row.querySelectorAll('input, select, button, textarea');
                inputs.forEach(function (i) {
                    // do not disable the presence toggle itself (so user can toggle back)
                    if (i.classList && (i.classList.contains('att-switch') || i.classList.contains('att-sw-present') || i.classList.contains('att-sw-absent'))) return;
                    try { i.disabled = true; } catch (e) { }
                    try { i.setAttribute('readonly', 'readonly'); } catch (e) { }
                });
            } catch (e) { console.warn('markRowAbsentVisual', e); }
        }


        function enableRowInputs(row) {
            if (!row) return;
            row.classList.remove('row-absent');
            var ctrls = row.querySelectorAll('input, select, button, textarea');
            for (var i = 0; i < ctrls.length; i++) {
                var c = ctrls[i];
                if (typeof c.closest === 'function' && c.closest('.att-switch')) continue;
                try { c.disabled = false; } catch (e) { }
                try { c.removeAttribute('readonly'); } catch (e) { }
            }
        }


        function setRowPresentVisual(elToggle, studentId) {
            try { elToggle.classList.remove('att-sw-absent'); elToggle.classList.add('att-sw-present'); } catch (e) { }
            var hid = document.getElementById('hidStatus_' + studentId);
            if (hid) hid.value = '1';
        }
        function setRowAbsentVisual(elToggle, studentId) {
            try { elToggle.classList.remove('att-sw-present'); elToggle.classList.add('att-sw-absent'); } catch (e) { }
            var hid = document.getElementById('hidStatus_' + studentId);
            if (hid) hid.value = '0';
        }
        function callWebMethod(methodName, payloadObj, cb) {
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'StudentCheckin.aspx/' + methodName, true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.onreadystatechange = function () {
                if (xhr.readyState !== 4) return;
                var parsed = null;
                try { parsed = JSON.parse(xhr.responseText); } catch (e) { }
                var data = (parsed && parsed.d) ? parsed.d : parsed;
                if (cb) cb(data);
            };
            xhr.send(JSON.stringify(payloadObj));
        }

        function disableRowExceptAttendance(row) {
            if (!row) return;
            var ctrls = row.querySelectorAll('input, select, button, textarea');
            for (var i = 0; i < ctrls.length; i++) {
                var c = ctrls[i];
                if (typeof c.closest === 'function' && c.closest('.att-switch')) continue;
                try { c.disabled = true; } catch (e) { }
                try { c.setAttribute('readonly', 'readonly'); } catch (e) { }
            }
            row.classList.add('row-absent');
        }

        function rowHasCheckinData(row, studentId) {
            try {
                if (!row) return false;

                // 1) Check time inputs (class names used in your ASPX: .in-time, .out-time)
                var timeInputs = row.querySelectorAll('input.time-input, input.in-time, input.out-time, input[type="time"], input[type="text"]');
                for (var i = 0; i < timeInputs.length; i++) {
                    var v = (timeInputs[i].value || '').trim();
                    if (v.length > 0) {
                        // ignore empty placeholder like "--:--" if you have one; adapt if needed
                        if (v !== '--:--') return true;
                    }
                }

                // 2) Check any hidden extras field named 'hidExtraTimes_<studentid>' or 'hidExtraTimes'
                try {
                    var hidName = 'hidExtraTimes_' + studentId;
                    var hid = document.getElementById(hidName);
                    if (hid && hid.value && hid.value.trim().length > 0) return true;
                    var generic = row.querySelector('input[id^="hidExtraTimes"], input[name^="hidExtraTimes"]');
                    if (generic && generic.value && generic.value.trim().length > 0) return true;
                } catch (e) { /* ignore */ }

                // 3) Check any other visible inputs in the row (text/select) for non-empty values
                var inputs = row.querySelectorAll('input[type="text"], select, textarea');
                for (var j = 0; j < inputs.length; j++) {
                    var el = inputs[j];
                    if (el.closest && el.closest('.att-switch')) continue; // skip toggle area
                    var val = (el.value || '').trim();
                    if (val.length > 0) return true;
                }
            } catch (ex) {
                console.error('rowHasCheckinData error', ex);
            }
            return false;
        }

        function confirmThenDelete(elToggle, studentId, classId, row, dateIso) {
            try {
                var hasData = rowHasCheckinData(row, studentId);

                // If there's data, ask confirmation. If not, proceed without confirm.
                if (hasData) {
                    var msg = "Check-in data already entered. Okay to delete?";
                    // Use native confirm for reliability. Replace with custom modal if desired.
                    var ok = window.confirm(msg);
                    if (!ok) {
                        // user cancelled: revert UI to Present and do NOT call delete
                        try {
                            // restore visual and inputs
                            enableRowInputs(row);
                            setRowPresentVisual(elToggle, studentId);
                        } catch (e) { console.error('revert after cancel error', e); }
                        return; // stop here
                    }
                    // if ok -> fall through to delete
                }

                // proceed to delete (no confirm or user confirmed)
                callWebMethod('DeleteCheckinForStudent',
                    { studentId: parseInt(studentId, 10), classId: parseInt(classId, 10), dateIso: dateIso },
                    function (res) {
                        if (res && res.success) {
                            // server confirmed delete -> full grid rebind
                            triggerServerRefresh();
                        } else {
                            console.error('Delete failed', res);
                            // restore UI so user isn't left in inconsistent state
                            try { enableRowInputs(row); setRowPresentVisual(elToggle, studentId); } catch (e) { }
                            alert('Failed to delete check-in: ' + (res && res.message ? res.message : 'Unknown'));
                            triggerServerRefresh();
                        }
                    });
            } catch (ex) {
                console.error('confirmThenDelete error', ex);
                // fallback: do not delete; rebind to ensure canonical state
                triggerServerRefresh();
            }
        }

        function parseTimeToMinutes(t) {
            if (!t) return null;
            t = (t + '').trim();
            if (t.length === 0) return null;

            // normalize AM/PM
            var m = t.match(/^(\d{1,2}):(\d{2})(?::\d{2})?\s*([ap]m)?$/i);
            if (!m) {
                // try Date parse fallback (last resort)
                var d = new Date('1970-01-01 ' + t);
                if (!isNaN(d.getTime())) return d.getHours() * 60 + d.getMinutes();
                return null;
            }
            var hh = parseInt(m[1], 10);
            var mm = parseInt(m[2], 10);
            var ampm = m[3];
            if (ampm) {
                var a = ampm.toLowerCase();
                if (a === 'pm' && hh < 12) hh += 12;
                if (a === 'am' && hh === 12) hh = 0;
            }
            if (hh < 0 || hh > 23 || mm < 0 || mm > 59) return null;
            return hh * 60 + mm;
        }

        function markRowInvalid(row) {
            try {
                row.classList.add('row-invalid');
                // scroll into view a little
                if (typeof row.scrollIntoView === 'function') row.scrollIntoView({ block: 'center', behavior: 'smooth' });
            } catch (e) { }
        }

        function clearRowInvalid(row) {
            try { row.classList.remove('row-invalid'); } catch (e) { }
        }

        function onCheckinClick(ev) {
            if (ev && ev.preventDefault) ev.preventDefault();
            if (handled) return;
            handled = true;

            // show loader and disable UI if you have them
            showGlobalLoader && showGlobalLoader();

            if (typeof PageMethods !== 'undefined' && typeof PageMethods.setCheckInn === 'function') {
                PageMethods.setCheckInn(studId,
                    function (result) { // success callback from server
                        try {
                            // PageMethods returns the raw return value from the WebMethod
                            var res = (typeof result === "string") ? result : (result && result.d) ? result.d : result;

                            cleanup(); // remove popup etc.

                            if (res === "OK") {
                                // success — continue to lesson/datasheet
                                onFinish && onFinish();
                            } else if (res && res.indexOf("ERR:SessionExpired") === 0) {
                                // session expired — force reload to login
                                alert("Session expired. Please login again.");
                                window.location = "/Administration/Error.aspx?Error=Your session has expired. Please log-in again";
                            } else {
                                // server returned an error — show to user and DO NOT proceed
                                alert("Check-in failed: " + (res || "Unknown error"));
                                // do not call onFinish so we avoid the infinite loader/datasheet failure
                            }
                        } catch (ex) {
                            console.error("onCheckinClick success handler error", ex);
                            cleanup();
                            alert("Unexpected error. Please try again.");
                        } finally {
                            hideGlobalLoader && hideGlobalLoader();
                        }
                    },
                    function (err) { // failure callback (network/exception)
                        try {
                            cleanup();
                            hideGlobalLoader && hideGlobalLoader();
                            // Show a friendly error
                            alert("Unable to perform check-in. Please try again.");
                            // do not proceed to onFinish because check-in didn't succeed
                        } catch (ex) {
                            console.error("onCheckinClick failure handler error", ex);
                        }
                    }
                );
            } else {
                cleanup();
                hideGlobalLoader && hideGlobalLoader();
                onFinish && onFinish();
            }
        }


    </script>


</head>
<body>
    <form id="form1" runat="server">

    <div id="loader" style="display:none;
     position:fixed; top:0; left:0; width:100%; height:100%;
     background:rgba(255,255,255,0.7); z-index:99999;
     text-align:center; padding-top:200px; font-size:20px;">
  <div id="loaderMsg" style="margin-top:12px;font-weight:600;color:#1f2937;"></div>
  <img id="loaderIcon" src="<%= ResolveUrl("~/Administration/images/load.gif") %>" alt="loading" style="vertical-align:middle;" />
</div>

        <asp:HiddenField ID="hidSearch" Value="0" runat="server"  />

        <asp:HiddenField ID="hidSetVal" Value="0" runat="server"  />

        <asp:HiddenField ID="hidPastDate" runat="server" />
        
            <div class="attendance-shell">
  <div class="attendance-header">
    <div class="attendance-title">Attendance</div>
      <asp:ScriptManager ID="sm" runat="server" />
    <div id="currentDateTime" runat="server" class="attendance-datetime"><%# DateTime.Now.ToString("M/d/yyyy  h:mm:ss tt").ToUpper() %></div>
  </div>

  <div class="attendance-controls">

    <asp:UpdatePanel ID="upFilters" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <div class="controls-left">
          <label class="ctrl-label">Location:</label>
          <asp:DropDownList ID="ddlLocation" runat="server" CssClass="ctrl-select"
             AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" />
          &nbsp;&nbsp;&nbsp;&nbsp;
          <label class="ctrl-label">Client:</label>
          <asp:DropDownList ID="ddlClient" runat="server" CssClass="ctrl-select"
             AutoPostBack="true" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged" />
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>

    <div class="controls-right">
      <!-- Make button ClientID static so JS can reliably find it -->
      <asp:Button ID="btnEditPastDates" runat="server" Text="Edit Past Dates" CssClass="btn-filled"
          OnClientClick="return toggleCalendarPopup();" UseSubmitBehavior="false" ClientIDMode="Static" />
    </div>

  </div> <!-- .attendance-controls -->

  <div class="attendance-grid">
    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:GridView ID="GridView1" runat="server" CssClass="attendance-grid-table"
            AutoGenerateColumns="false" OnRowDataBound="grdGroup_RowDataBound">
          <Columns>
            <asp:BoundField DataField="StudentId" HeaderText="Student ID" />
            <asp:BoundField DataField="Name" HeaderText="Student Name" />
            <asp:BoundField DataField="ClassName" HeaderText="Class" />
            <asp:BoundField DataField="InTime" HeaderText="In Time" />
            <asp:BoundField DataField="OutTime" HeaderText="Out Time" />
            <asp:TemplateField HeaderText="Actions">
              <ItemTemplate>
                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditRow" />
              </ItemTemplate>
            </asp:TemplateField>
          </Columns>
        </asp:GridView>
      </ContentTemplate>

      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="calPast" EventName="SelectionChanged" />
          <asp:AsyncPostBackTrigger ControlID="btnRefreshGrid" EventName="Click" />
      </Triggers>
    </asp:UpdatePanel>
  </div> 
</div> 
<asp:UpdatePanel ID="upCalendar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
<ContentTemplate>
<asp:Panel ID="pnlCalendar" runat="server" CssClass="calendar-popup" Style="display:none; position:absolute; z-index:9999;" ClientIDMode="Static">
<div class="edit-past-panel">
<div class="edit-past-inner">
<asp:Calendar ID="calPast" runat="server"
    ClientIDMode="Static"
    OnSelectionChanged="calPast_SelectionChanged"
    OnVisibleMonthChanged="calPast_VisibleMonthChanged"
    OnDayRender="calPast_DayRender"
    ShowNextPrevMonth="true" />
</div>
</div>
</asp:Panel>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="calPast" EventName="SelectionChanged" />
</Triggers>
</asp:UpdatePanel>
        <asp:Button ID="btnRefreshGrid" ClientIDMode="Static" UseSubmitBehavior="false" runat="server" Style="display:none" OnClick="btnRefreshGrid_Click" />
        <table width="353px" cellpadding="0" cellspacing="0">
            <%--<tr>
                <td>
                    <asp:ImageButton ID="imgBDay" runat="server" OnClientClick="Click1()" ImageUrl="~/StudentBinder/img/DayG.png" OnClick="imgBDay_Click" />

                    <asp:ImageButton ID="ImgBRes" runat="server" OnClientClick="Click2()" ImageUrl="~/StudentBinder/img/ResB.png" OnClick="ImgBRes_Click" />

                </td>
            </tr>--%>
            <tr>
                <td>
                    <div class="setBox" style="width:800px;">

                        <div id="set" style="padding: 5px 5px 5px 5px;">



                            <%--<table style="background-color: #FFFFFF; width: 100%" border="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSearch" runat="server" style="width:95%;" AutoPostBack="True" ></asp:TextBox></td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" OnClientClick="setAlert1();showLoader('Searching…');" runat="server" OnClick="ImageButton1_Click" CssClass="btn-black-srch" BackColor="#33ccff" ImageUrl="~/StudentBinder/img/searchbtn.png" />
                                </tr>
                            </table>--%>
                            <div id="gridBand" style="display:none;
                                 background:#dc2626; color:#fff; padding:6px 10px;
                                 font:14px/1.3 Arial, Helvetica, sans-serif; margin:6px 0;">
                              <span id="gridBandMsg"></span>
                              <a href="javascript:void(0)" onclick="hideGridBand()"
                                 style="color:#fff; text-decoration:none; margin-left:12px; font-weight:bold;">×</a>
                            </div>
                            <asp:UpdatePanel ID="upModalGrid" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
    <div id="modal" style="height: 300px; width: 100%; overflow: auto; background-color: white; margin-top: 3px; margin-right: 3px;">
      <!-- STATIC header must be OUTSIDE the scrollable grid wrapper -->
      <div id="staticGridHeader" class="static-grid-header">
                                <div class="static-grid-row">
                                  <div class="col-att">Attendance</div>
                                  <div class="col-client">Client</div>
                                  <div class="col-in">In</div>
                                  <div class="col-out">Out</div>
                                  <div class="col-add">Add Row</div>
                                  <div class="col-code">Code</div>
                                  <div class="col-save"></div>
                                </div>
                              </div>
                                <div class="attendance-grid-wrap">
                                    <asp:GridView ID="grdGroup" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="studentid,classid"
                                    ShowHeader="False" HeaderStyle-BackColor="#f3f4f6" HeaderStyle-Font-Bold="true" CssClass="att-grid"
                                    OnRowDataBound="grdGroup_RowDataBound" OnDataBound="grdGroup_DataBound" OnRowCommand="grdGroup_RowCommand">
                                  <Columns>
                                    <asp:TemplateField HeaderText="Attendance">
                                    <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                    <ItemStyle Width="90px" HorizontalAlign="Center" />
                                      <ItemTemplate>
                                        <div class="att-switch <%# (Convert.ToBoolean(Eval("IsPresent")) ? "att-sw-present" : "att-sw-absent") %>"
       onclick="onPresenceToggle(this, '<%# Eval("StudentId") %>', '<%# Eval("ClassId") %>')"
       data-student='<%# Eval("StudentId") %>'>
                                          <span class="switch-knob"></span>
                                          <span class="label-right">Present</span>
                                          <span class="label-left">Absent</span>
                                        </div>
                                          <asp:HiddenField ID="hidExtraTimes" runat="server" Value='<%# Eval("Extras") %>' />
                                        <asp:HiddenField ID="hidStatus" runat="server" Value='<%# GetStatusHidden(Eval("IsPresent")) %>' />
                                      </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Client">
                                        <HeaderStyle Width="230px" HorizontalAlign="Left" />
                                        <ItemStyle Width="230px" HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <div class="client-name"><%# Eval("Name") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="In">
                                      <ItemTemplate>
                                        <asp:TextBox ID="txtInTime" runat="server" CssClass="time-input in-time" Text='<%# BindTime(Eval("InTime")) %>' />
                                      </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Out">
                                      <ItemTemplate>
                                        <asp:TextBox ID="txtOutTime" runat="server" CssClass="time-input out-time" Text='<%# BindTime(Eval("OutTime")) %>' />
                                        <input type="hidden" name="hidExtraTimes_<%# Eval("studentid") %>" id='hidExtraTimes_<%# Eval("studentid") %>' value='<%# Eval("Extras") %>' />
                                      </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Add Row" >
                                      <HeaderStyle Width="55px" HorizontalAlign="Center" />
                                      <ItemStyle Width="55px" HorizontalAlign="Center" />
                                      <ItemTemplate>
                                        <asp:Button ID="btnAddTime" runat="server" Text="+"
                                          CssClass="add-btn"
                                          OnClientClick='<%# "try{ addExtraRow(this, " + Eval("studentid") + "); } catch(e){ console.error(e); } return false;" %>'
                                          UseSubmitBehavior="false" CausesValidation="false" />
                                      </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Code">
                                      <ItemTemplate>
                                        <asp:DropDownList ID="ddlCode" runat="server" CssClass="code-select">
                                        </asp:DropDownList>
                                      </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" />
                                      <ItemTemplate>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="save-btn"
                                          CommandName="Save" CommandArgument='<%# Eval("studentid") %>'
                                          OnClientClick="if(!validateGo(this)) return false; return showSaveLoader(this);" CausesValidation="false" UseSubmitBehavior="false" />
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                  </Columns>
                                </asp:GridView>

                            </div>
                        </div>
      </ContentTemplate>
                                <Triggers>
    <asp:AsyncPostBackTrigger ControlID="ddlLocation" EventName="SelectedIndexChanged" />
    <asp:AsyncPostBackTrigger ControlID="ddlClient" EventName="SelectedIndexChanged" />
    <asp:AsyncPostBackTrigger ControlID="calPast" EventName="SelectionChanged" />
    <asp:AsyncPostBackTrigger ControlID="btnRefreshGrid" EventName="Click" />
  </Triggers>
      </asp:UpdatePanel>



                    </div>
                </td>
            </tr>
        </table>


    </form>
</body>
</html>
