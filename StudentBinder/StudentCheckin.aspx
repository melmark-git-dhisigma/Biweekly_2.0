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
          display:table-row;
          grid-template-columns: 120px 160px 120px 34px 120px 70px; /* columns: Status|Client|In/Out|+|Code|Save */
          gap:8px;
          align-items:center;
          padding:6px 4px;
          border-bottom:1px solid rgba(0,0,0,0.06);
          box-sizing:border-box;
          opacity: 1;
          background-color: transparent;
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
          grid-template-columns: 147px 223px 103px 103px 48px 79px 82px;
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
            background-color: #fef2f2;
        }

        input:disabled {
           opacity: 0.6;
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


       .att-toggle{
          display:flex;
          gap:3px;
          padding: 0.1px 2.1px;
       }

       .att-btn{
          padding:6px 13px;
          border-radius:16px;
          font-size:12px;
          font-weight:600;
          cursor:pointer;
          transition:all .2s ease;
          opacity:1;
          border:1px solid #d1d5db;
       }

       /* active states */
       .att-btn.active{
          opacity:1;
          color:white;
       }

       .btn-present.active{
          background:#16a34a;
          border-color:#16a34a;
       }

       .btn-absent.active{
          background:#ef4444;
          border-color:#ef4444;
       }

       .active-row {
          opacity: 1;
          background-color: #ecfdf5;
       }
       .row-default {
          opacity: 0.5;
       }

       .save-btn:disabled {
          background: #d1d5db !important;
          color: #6b7280 !important;
          cursor: not-allowed;
       }


       .btn-present {
          background: #ecfdf5;   /* ultra light green */
          color: #065f46;
          border-color: #d1fae5;
          opacity: 0.7;          /* softer look */
       }

       .btn-absent {
          background: #fef2f2;   /* ultra light red */
          color: #7f1d1d;
          border-color: #fecaca;
          opacity: 0.7;          /* softer look */
       }

       /* ACTIVE (STRONG) */
       .btn-present.active {
          background: #16a34a;
          color: #fff;
          border-color: #16a34a;
          opacity: 1;
       }

       .btn-absent.active {
          background: #dc2626;
          color: #fff;
          border-color: #dc2626;
          opacity: 1;
       }

       .att-btn:hover {
          transform: scale(1.03);
          opacity: 0.9;
       }

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
            return true;
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
                var codeSelects = row.querySelectorAll('.code-stack .code-pair select');
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


                var maxLen = inInputs.length;

                for (var i = 0; i < maxLen; i++) {

                    var statusField = row.querySelector('input[type="hidden"][id$="hidStatus"]');
                    var status = statusField ? statusField.value : "";
                    var codeSel = codeSelects[i] || null;
                    var lookupCode = getLookupCodeFromSelect(codeSel);
                    var lookupCodeNorm = lookupCode ? lookupCode.trim().toUpperCase() : null;
                    //ABSENT LOGIC
                    if (status === "0") {

                        // if no reason selected → skip validation (keep old values)
                        if (!lookupCodeNorm) {
                            console.log("Absent with no reason → skipping save validation");
                            continue;
                        }

                        // if reason selected → allow save without time
                        console.log("Absent with reason → allow save");
                        continue;
                    }

                    var inEl = inInputs[i];
                    var outEl = outInputs[i];

                    var inVal = inEl ? (inEl.value || '').trim() : '';
                    var outVal = outEl ? (outEl.value || '').trim() : '';

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

                return true;
            } catch (err) {
                console.error('validateGo error', err);
                return false;
            }
        }

        

        function showSaveLoader(btn) {
            try {
                if (btn && btn.disabled !== undefined) {
                    btn.disabled = true;
                }
                return true;
            } catch (e) {
                return true;
            }
        }

        var allowClockUpdate = true;


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
                    if (window.__skipCalendarLoader) return;
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



            function initCalendarFix() {
                try {
                    if (!window.Sys || !Sys.WebForms || !Sys.WebForms.PageRequestManager) {
                        return; //wait until available
                    }

                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                    if (!prm) return;

                    prm.add_initializeRequest(function (sender, args) {
                        try {
                            var eventArg = document.getElementById("__EVENTARGUMENT");

                            if (eventArg && eventArg.value && eventArg.value.indexOf("V") === 0) {
                                //Month navigation → skip loader
                                window.__skipCalendarLoader = true;

                                if (window.hideLoader) hideLoader();
                            } else {
                                window.__skipCalendarLoader = false;
                            }
                        } catch (e) { }
                    });

                } catch (e) {
                    console.log("Init error:", e);
                }
            }

            //WAIT until ASP.NET AJAX is ready
            if (window.Sys && Sys.Application) {
                Sys.Application.add_load(initCalendarFix);
            } else {
                // fallback (if Sys not ready yet)
                document.addEventListener("DOMContentLoaded", function () {
                    setTimeout(initCalendarFix, 200);
                });
            }




            function updateClock() {
                if (window.__lockClock) return;
                var hid = document.getElementById('hidPastDate') ||
                          document.querySelector('input[name$="hidPastDate"]');

                if (hid && hid.value) return;

                if (!allowClockUpdate) return;

                var now = new Date();

                var dateStr = String(now.getDate()).padStart(2, '0') + '/' + String(now.getMonth() + 1).padStart(2, '0') + '/' + now.getFullYear();

                var timeStr = now.toLocaleTimeString('en-US', { hour12: true })
                    .replace(/([ap])m/, function (match) {
                        return match.toUpperCase();
                    });

                var el = document.getElementById('currentDateTime');
                if (el) el.textContent = dateStr + '  ' + timeStr;
            }

            updateClock();
            setInterval(updateClock, 1000);

            var outsideClickHandler = null;
            var calendarClickDelegationAttached = false;
            var prMHooked = false;
            var log = false;

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
                            var txt = (t.textContent || t.innerText || '').trim();
                            var hasNumeric = /^\d{1,2}$/.test(txt) || (t.querySelector && (function () {
                                var s = t.querySelector('span,div');
                                return s && /^\d{1,2}$/.test((s.textContent || s.innerText || '').trim());
                            })());
                            if (!hasNumeric) {
                                return;
                            }
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


            // Initialize wiring (run now and also attempt wiring after partial updates)
            function init() {
                try {
                    attachCalendarClickForExistingLoader();
                    attachOutsideClickHandlerDelayed();
                    hookPageRequestManagerOnce();
                } catch (e) { }
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


            // Wire MS Ajax PRM once: show loader on beginRequest, cleanup on endRequest
            (function hookPageRequestManager() {
                try {
                    if (prMHooked) return;
                    if (typeof (Sys) !== 'undefined' && Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                        var prm = Sys.WebForms.PageRequestManager.getInstance();
                        if (!prm) return;
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
                                    //showLoader();
                                } catch (e) { if (console) console.log('calendar click delegation error', e); }
                            }, true);
                            calendarClickDelegationAttached = true;
                        })();
                    } catch (e) { }
                };
            }



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

                        // prevent the default client-side anchor behavior that opens/toggles the calendar UI
                        // BUT still perform the postback explicitly if href contains __doPostBack
                        try {
                            var href = t.getAttribute('href') || '';
                            if (href.indexOf('__doPostBack') !== -1) {

                                if (window.__justSaved) {
                                    return;   //STOP SECOND RELOAD
                                }

                                ev.preventDefault();

                                var m = href.match(/__doPostBack\(\s*'([^']*)'\s*,\s*'([^']*)'\s*\)/);
                                if (m && typeof __doPostBack === 'function') {
                                    __doPostBack(m[1], m[2]);
                                }
                            }
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
                        } catch (postEx) { }
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
                    }
                }
            } catch (e) { /* ignore */ }



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

            // main updater: sets the on-page timer/display according to hidden selected date or live clock
            window.updateTimerFromHidden = function (force) {
                try {
                    var hid = findHidden();
                    var iso = hid ? (hid.value || '').trim() : '';

                    var el = document.getElementById(window.__dateDisplayId) ||
                             document.querySelector('.current-date-display') || null;

                    if (!el) return;

                    //helper: safe LOCAL date parse
                    function parseLocalDate(iso) {
                        if (!iso) return new Date();

                        var parts = iso.split('-');
                        if (parts.length !== 3) return new Date();

                        return new Date(
                            parseInt(parts[0], 10),
                            parseInt(parts[1], 10) - 1,
                            parseInt(parts[2], 10)
                        );
                    }

                    if (iso) {

                        var parts = iso.split('-');

                        var d = new Date(
                            parseInt(parts[0], 10),
                            parseInt(parts[1], 10) - 1,
                            parseInt(parts[2], 10)
                        );

                        var fullDate = d.toLocaleDateString('en-US', {
                            weekday: 'long',
                            year: 'numeric',
                            month: 'long',
                            day: 'numeric'
                        });

                        el.innerHTML = fullDate;

                        try { el.classList.add('past-selected-date'); } catch (e) { }
                        try { el.style.color = 'red'; } catch (e) { }

                        window.allowClockUpdate = false;

                        window.__lockClock = true;

                        window._lastSelectedIso = iso;
                    }
                    else {
                        try { el.classList.remove('past-selected-date'); } catch (e) { }
                        try { el.style.color = ''; } catch (e) { }

                        window.allowClockUpdate = true;

                        window.__lockClock = false;

                        if (typeof refreshClockNow === 'function') {
                            try { refreshClockNow(); }
                            catch (e) { el.innerHTML = new Date().toLocaleString(); }
                        } else {
                            el.innerHTML = new Date().toLocaleString();
                        }

                        window._lastSelectedIso = null;
                    }
                } catch (err) {
                    console && console.log && console.log('updateTimerFromHidden error', err);
                }
            };


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
                    function parseIsoLocal(iso) {
                        if (!iso) return new Date();
                        var p = iso.split("-");
                        return new Date(parseInt(p[0], 10), parseInt(p[1], 10) - 1, parseInt(p[2], 10));
                    }

                    var target = iso ? parseIsoLocal(iso) : new Date();
                    // normalize to midnight
                    target.setHours(0, 0, 0, 0);

                    // calendar renders day number inside anchors or spans - do robust match
                    var cells = cal.getElementsByTagName('td');
                    for (var i = 0; i < cells.length; i++) {
                        var td = cells[i];
                        var text = (td.innerText || td.textContent || '').trim();
                        if (!text) continue;
                        // text could be "1", "2" etc. Compare day number and month context:
                        var dayNum = parseInt(text, 10);
                        if (!isNaN(dayNum)) {
                            // attempt to get month/year from visible header (best-effort)
                            if (td.classList && td.className.indexOf('other-month') !== -1) continue;
                            // build candidate date using visible month from calendar
                            var header = cal.querySelector('.monthYear') || cal.querySelector('caption') || null;
                            var visYear = null, visMonth = null;
                            if (header) {
                                var htxt = header.innerText || header.textContent;
                                var parts = (htxt || '').trim().split(' ');
                                if (parts.length >= 2) {
                                    visMonth = parts[0];
                                    visYear = parseInt(parts[1], 10);
                                }
                            }
                            // fallback: assume target is in the month already visible
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
                }
            }

            // also run on initial load
            if (document.readyState === 'complete' || document.readyState === 'interactive') {
                applyCalendarSelection();   //NO setTimeout
            } else {
                window.addEventListener('DOMContentLoaded', function () {
                    applyCalendarSelection();   //NO setTimeout
                });
            }

            function initPRM() {
                if (typeof Sys !== "undefined" &&
                    Sys.WebForms &&
                    Sys.WebForms.PageRequestManager) {

                    var prm = Sys.WebForms.PageRequestManager.getInstance();

                    if (window.__prmHooked) return;
                    window.__prmHooked = true;

                    prm.add_beginRequest(function () {
                        if (window.showLoader)
                            showLoader("Loading...");
                    });

                    prm.add_endRequest(function (sender, args) {

                        if (args.get_error()) {
                            console.error("ASYNC ERROR:", args.get_error().message);
                            args.set_errorHandled(true);
                        }

                        try {
                            safeInit();

                            if (typeof normalizeAllExtras === "function")
                                normalizeAllExtras();

                            if (typeof applyAttendanceUI === "function")
                                applyAttendanceUI();

                            if (typeof applyAttendanceHighlight === "function")
                                applyAttendanceHighlight();

                            if (typeof applyCalendarSelection === "function")
                                applyCalendarSelection();

                            if (typeof updateTimerFromHidden === "function")
                                updateTimerFromHidden(true);

                        } catch (e) {
                            console.log("PRM endRequest error", e);
                        }

                        if (window.hideLoader)
                            setTimeout(hideLoader, 150);
                    });

                } else {
                    setTimeout(initPRM, 100);
                }
            }

            initPRM();


            function safeInit() {
                try {
                    if (typeof initAttendanceButtons === "function") {
                        initAttendanceButtons();
                    }
                } catch (e) {
                    console.error("initAttendanceButtons error:", e);
                }

                try {
                    if (typeof renderAllExtras === "function") {
                        renderAllExtras();
                    }
                } catch (e) {
                    console.error("renderAllExtras error:", e);
                }
            }

            // Initial load
            document.addEventListener("DOMContentLoaded", function () {
                safeInit();
            });













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

        function addExtraRow(btn, studentId, classId, codeVal) {
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

                populateCodeDropdown(codeSelect, codeVal);

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
                    attachInputListenersToInput(inInput, row, studentId,classId);
                    attachInputListenersToInput(outInput, row, studentId,classId);
                    attachInputListenersToInput(codeSelect, row, studentId,classId);
                }

                // serialize now and sync
                if (typeof serializeExtraTimesForRow === 'function') serializeExtraTimesForRow(row, studentId,classId);
                if (typeof syncRowPairs === 'function') syncRowPairs(row);

                inInput.focus();

                // older code called both serialize functions — keep that for compatibility
                if (typeof serializeExtrasForStudent === 'function') serializeExtrasForStudent(studentId);

            } catch (e) {
                console.error('addExtraRow error', e);
            }
        }

        function setSelectedDate(dateStr) {
            var parts = dateStr.split('-');

            selectedDateOverride = new Date(
                parseInt(parts[0]),       // year
                parseInt(parts[1]) - 1,   // month
                parseInt(parts[2])        // day
            );

            allowClockUpdate = false;
        }

        function resetToToday() {
            allowClockUpdate = true;
            selectedDateOverride = null;
        }

        function parseLocalDate(iso) {
            if (!iso) return new Date();

            var parts = iso.split('-');
            if (parts.length !== 3) return new Date();

            return new Date(
                parseInt(parts[0]),
                parseInt(parts[1]) - 1,
                parseInt(parts[2])
            );
        }

        function renderAllExtras() {
            try {

                var hfs = document.querySelectorAll("input[id^='hidExtraTimes_']");
                if (!hfs) return;

                hfs.forEach(function (hf) {

                    try {

                        if (!hf || !hf.id) return;

                        var parts = hf.id.split("_");
                        if (parts.length < 3) return;

                        var studentId = parts[1];
                        var classId = parts[2];

                        var row = document.querySelector(
                            "tr[data-studentid='" + studentId + "'][data-classid='" + classId + "']"
                        );

                        if (!row) return;

                        var val = hf.value;
                        if (!val) return;

                        if (row.__extrasRendered) return;
                        row.__extrasRendered = true;

                        //CLEAR OLD
                        row.querySelectorAll(".extra-row").forEach(function (el) {
                            el.remove();
                        });

                        val.split(";").forEach(function (p) {

                            if (!p) return;

                            try {

                                var parts2 = p.split("|").map(function (x) {
                                    try { return decodeURIComponent(x); } catch (e) { return x || ""; }
                                });

                                createExtraPairNodes(row, studentId, classId, {
                                    inVal: parts2[0] || "",
                                    outVal: parts2[1] || "",
                                    codeValue: parts2[2] || ""
                                });

                            } catch (e) {
                                console.error("Pair error:", e);
                            }

                        });

                    } catch (e) {
                        console.error("HF loop error:", e);
                    }

                });

            } catch (e) {
                console.error("renderAllExtras crash:", e);
            }
        }

        function safeRenderExtras() {
            if (!window.__isPostBackDone) return;   //WAIT until real render
            try { renderAllExtras(); } catch (e) { console.warn(e); }
        }


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

        window.createExtraPairNodes = function (rowOrBtnOrSelector, studentId,classId, opts) {
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
            var baseSelect = codeCell.querySelector('select');

            var codeSelect;

            if (baseSelect) {
                codeSelect = baseSelect.cloneNode(true);   //copies all options
                codeSelect.className = 'extra-code code-select';

                // reset selection
                codeSelect.selectedIndex = 0;
            } else {
                // fallback
                codeSelect = document.createElement('select');
                codeSelect.className = 'extra-code code-select';

                var defaultOpt = document.createElement('option');
                defaultOpt.value = '';
                defaultOpt.textContent = '---Select---';
                codeSelect.appendChild(defaultOpt);
            }

            codeVal = codeVal ? String(codeVal).trim() : '';

            for (var i = 0; i < codeSelect.options.length; i++) {
                if (String(codeSelect.options[i].value) === codeVal) {
                    codeSelect.selectedIndex = i;
                    break;
                }
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
            try { if (typeof serializeExtraTimesForRow === 'function') serializeExtraTimesForRow(row, studentId,classId); } catch (e) { console.warn('serializeExtraTimesForRow failed', e); }
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

        window.serializeExtraTimesForRow = function (rowOrBtnOrSelector, studentId,classId) {
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
                var hid = null;

                if (studentId && classId) {
                    hid = document.getElementById('hidExtraTimes_' + studentId + '_' + classId);
                }

                if (!hid) {
                    console.error("Hidden field not found for:", studentId, classId);
                    return result;
                }
                if (hid) {
                    hid.value = result;
                } else {
                }
            }

            return result;
        };


        function attachInputListenersToInput(el, row, studentId,classId) {
            if (!el) return;
            if (el.__hasAttachListener) return;
            el.__hasAttachListener = true;
            el.addEventListener('change', function () { try { serializeExtraTimesForRow(row, studentId,classId); } catch (e) { } });
            if (el.tagName && el.tagName.toLowerCase() === 'input') {
                el.addEventListener('input', function () { try { serializeExtraTimesForRow(row, studentId,classId); } catch (e) { } });
            }
        }




        function normalizeAllExtras() {

            if (window.__extrasRunning) return;
            window.__extrasRunning = true;

            try {

                var rows = document.querySelectorAll("tr[data-extras]");

                rows.forEach(function (row) {

                    var studentId = row.getAttribute("data-studentid");
                    var classId = row.getAttribute("data-classid");

                    if (!studentId || !classId) return;

                    //CLEAR container FIRST
                    var container = row.querySelector(".extras-container");
                    if (container) {
                        container.innerHTML = "";
                    }

                    //FIND corresponding hidden field
                    var hid = document.getElementById("hidExtraTimes_" + studentId + "_" + classId);
                    if (!hid) return;

                    var raw = hid.value || "";
                    if (!raw) return;

                    var parts = raw.split(';');
                    var normalized = [];

                    //GET MAIN VALUES (NO optional chaining)
                    var inEl = row.querySelector('input.in-time');
                    var outEl = row.querySelector('input.out-time');
                    var codeEl = row.querySelector('select[id$="ddlCode"]');

                    var mainIn = inEl ? (inEl.value || '').trim() : '';
                    var mainOut = outEl ? (outEl.value || '').trim() : '';
                    var mainCode = codeEl ? (codeEl.value || '').trim() : '';

                    parts.forEach(function (p) {

                        var seg = p || '';

                        try { seg = decodeURIComponent(seg); } catch (e) { }

                        var arr = seg.split('|');
                        while (arr.length < 3) arr.push('');

                        arr = arr.map(function (x) { return (x || '').trim(); });

                        // skip empty
                        if (!arr[0] && !arr[1] && !arr[2]) return;

                        //SKIP duplicate of main row
                        if (arr[0] === mainIn && arr[1] === mainOut && arr[2] === mainCode) return;

                        normalized.push(
                            encodeURIComponent(arr[0]) + '|' +
                            encodeURIComponent(arr[1]) + '|' +
                            encodeURIComponent(arr[2])
                        );
                    });

                    //SAVE CLEANED DATA
                    hid.value = normalized.join(';');
                });

                //CALL RENDER ONLY ONCE (VERY IMPORTANT)
                if (typeof renderAllExtras === "function") {
                    renderAllExtras();
                }

            } catch (e) {
                console.error("normalizeAllExtras error", e);
            }

            window.__extrasRunning = false;
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




        window.hideSaveLoader = window.hideSaveLoader || function () {
            try {
                if (typeof hideSaveOverlay === 'function') { hideSaveOverlay(); return; }
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


    window.hideSaveLoader = window.hideSaveLoader || function () {
        try {
            if (typeof hideSaveOverlay === 'function') { hideSaveOverlay(); return; }
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







        function localTodayIso() {
            var d = new Date();
            return d.getFullYear() + "-" +
                   String(d.getMonth() + 1).padStart(2, "0") + "-" +
                   String(d.getDate()).padStart(2, "0");
        }



        function getSelectedDateIso() {
            try {
                var hf = document.getElementById('<%= hidPastDate.ClientID %>') ||
                         document.getElementById('hidPastDate') ||
                         document.querySelector('input[id*="hidPastDate"], input[name$="hidPastDate"]');

                var iso = hf && hf.value ? hf.value.trim() : "";
                if (/^\d{4}-\d{2}-\d{2}$/.test(iso)) {
                    return iso;   // IMPORTANT: return as-is
                }

                return localTodayIso();
            } catch (e) {
                return localTodayIso();
            }
        }


        function triggerServerRefresh() {
            try {
                var unique = '<%= btnRefreshGrid.UniqueID %>';
                if (typeof __doPostBack === 'function' && unique) { __doPostBack(unique, ''); return; }
                var b = document.getElementById('btnRefreshGrid');
                if (b && typeof b.click === 'function') b.click();
            } catch (e) { console.error('triggerServerRefresh error', e); }
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


        function rowHasCheckinData(row, studentId) {
            try {
                if (!row) return false;

                //0. CHECK STATUS FIRST
                if (row.classList.contains("active-row") || row.classList.contains("row-absent")) {
                    return true; //EVEN WITHOUT TIME → DATA EXISTS IN DB
                }

                //1. Check IN/OUT inputs
                var inInputs = row.querySelectorAll('input.in-time, input[id$="txtInTime"]');
                var outInputs = row.querySelectorAll('input.out-time, input[id$="txtOutTime"]');

                for (var i = 0; i < inInputs.length; i++) {
                    if ((inInputs[i].value || "").trim() !== "") return true;
                }

                for (var j = 0; j < outInputs.length; j++) {
                    if ((outInputs[j].value || "").trim() !== "") return true;
                }

                //2. CHECK EXTRAS
                var extras = row.getAttribute("data-extras");
                if (extras && extras.trim() !== "") {
                    return true;
                }

                //3. CHECK CODE DROPDOWN
                var codeSelects = row.querySelectorAll('select.code-select, select.extra-code');

                for (var k = 0; k < codeSelects.length; k++) {
                    if ((codeSelects[k].value || "").trim() !== "") return true;
                }

                return false;

            } catch (e) {
                console.error("rowHasCheckinData error:", e);
                return false;
            }
        }

        function confirmThenDelete(elToggle, studentId, classId, row, dateIso, callback) {

            try {
                var hasData = rowHasCheckinData(row, studentId);

                if (hasData) {
                    if (!confirm("Attendance data already entered. Okay to delete?")) {
                        callback && callback(false);
                        return;
                    }
                }

                callWebMethod('DeleteCheckinForStudent',
                {
                    studentId: parseInt(studentId, 10),
                    classId: parseInt(classId, 10),
                    dateIso: dateIso || ""
                },
                function (res) {

                    console.log("DELETE RESPONSE:", res);

                    var data = (res && res.d) ? res.d : res;

                    if (data && data.success) {
                        callback && callback(true);
                    } else {
                        alert("Delete failed");
                        callback && callback(false);
                    }
                });

            } catch (ex) {
                console.error('confirmThenDelete error', ex);
                callback && callback(false);
            }
        }



        
        function toggleAttendance(el, val) {

            var row = el.closest("tr");
            var container = el.closest(".att-toggle");

            var hidden = container.querySelector("input[id*='hidStatus']");
            if (!hidden) return;

            var presentBtn = container.querySelector(".btn-present");
            var absentBtn = container.querySelector(".btn-absent");

            var current = hidden.value;

            //CASE 1: FIRST TIME SELECT (-1 → 1/0)
            if (current === "-1" || current === "") {

                //APPLY UI IMMEDIATELY
                applySelection(val);

                insertAttendanceToServer(row, val, function (success){

                    if (success) {
                        hidden.value = val.toString(); // set AFTER success
                        triggerServerRefresh();
                    } else {
                        alert("Failed to save");
                    }
                });

                return;
            }

            //CASE 2: SAME CLICK (REMOVE)
            if (current == val.toString()) {
                handleDeleteAndUpdate(null);
                return;
            }

            //CASE 3: SWITCH
            if (current !== val.toString()) {
                handleDeleteAndUpdate(val);
                return;
            }

            function applySelection(v) {

                hidden.value = v;

                presentBtn.classList.remove("active");
                absentBtn.classList.remove("active");

                //REMOVE OLD ROW STATES
                row.classList.remove("active-row", "row-absent", "row-default");

                if (v == 1) {
                    presentBtn.classList.add("active");
                    row.classList.add("active-row");   //ADD THIS
                    enableRow(row);
                } else {
                    absentBtn.classList.add("active");
                    row.classList.add("row-absent");   //ADD THIS
                    handleAbsent(row);
                }
            }

            function handleDeleteAndUpdate(newVal) {

                var oldVal = hidden.value;

                // disable clicks
                presentBtn.style.pointerEvents = "none";
                absentBtn.style.pointerEvents = "none";

                var studentId = row.getAttribute("data-studentid");
                var classId = row.getAttribute("data-classid");
                var dateIso = getSelectedDateIso();

                confirmThenDelete(el, studentId, classId, row, dateIso, function (success) {

                    presentBtn.style.pointerEvents = "";
                    absentBtn.style.pointerEvents = "";

                    if (success) {

                        if (newVal !== null) {

                            //INSERT NEW VALUE (Present or Absent)
                            insertAttendanceToServer(row, newVal, function (insertSuccess) {

                                if (insertSuccess) {

                                    //Update UI after insert
                                    applySelection(newVal);

                                    //Refresh grid
                                    triggerServerRefresh();

                                } else {
                                    alert("Failed to insert attendance");
                                }
                            });

                        } else {
                            // Only delete (no new selection)
                            triggerServerRefresh();
                        }
                    }
                    else {
                        //RESTORE OLD STATE
                        hidden.value = oldVal;

                        presentBtn.classList.remove("active");
                        absentBtn.classList.remove("active");

                        if (oldVal == "1") {
                            presentBtn.classList.add("active");
                            enableRow(row);
                        }
                        else if (oldVal == "0") {
                            absentBtn.classList.add("active");
                            handleAbsent(row);
                        }
                    }
                });
            }

            Sys.Application.add_load(function () {
                console.log("Grid reloaded - UI reapplied");
            });
        }
        function initAttendanceButtons() {

            document.querySelectorAll(".att-toggle").forEach(function (t) {

                var hidden = t.querySelector("input[id$='hidStatus']");
                if (!hidden) return;

                var val = (hidden.value || "").toString().trim().toLowerCase();
                var row = t.closest("tr");

                var presentBtn = t.querySelector(".btn-present");
                var absentBtn = t.querySelector(".btn-absent");

                presentBtn.classList.remove("active");
                absentBtn.classList.remove("active");

                if (val === "1" || val === "true") {
                    hidden.value = "1";
                    presentBtn.classList.add("active");
                    enableRow(row);
                }
                else if (val === "0" || val === "false") {
                    hidden.value = "0";
                    absentBtn.classList.add("active");
                    handleAbsent(row);
                }
                else {
                    hidden.value = "-1";
                    disableRow(row);
                }
            });
        }


        

        function enableRow(row) {
            if (!row) return;

            row.classList.remove("row-default", "row-absent");
            row.classList.add("active-row");

            row.querySelectorAll("input, select, button").forEach(function (el) {

                if (el.closest(".att-toggle")) return;

                //REMOVE ALL DISABLES
                el.disabled = false;
                el.removeAttribute("disabled");
                el.removeAttribute("readonly");
            });
            row.querySelectorAll(".save-btn").forEach(function (el) {
                el.disabled = false;
                el.removeAttribute("disabled");
            });
        }
        function handleAbsent(row) {
            if (!row) return;

            row.classList.remove("active-row", "row-default");
            row.classList.add("row-absent");

            row.querySelectorAll("input, select, button").forEach(function (el) {

                if (el.closest(".att-toggle")) return;

                el.disabled = true;
                el.setAttribute("disabled", "disabled");
            });

            //ONLY enable Code + Save
            row.querySelectorAll("select[id*='ddlCode']").forEach(function (el) {
                el.disabled = false;
                el.removeAttribute("disabled");
            });

            row.querySelectorAll("input[id*='btnSave'], .save-btn").forEach(function (el) {
                el.disabled = false;
                el.removeAttribute("disabled");
            });
        }

        function disableRow(row) {
            if (!row) return;

            // add visual state
            row.classList.add("row-disabled");

            // disable all inputs
            row.querySelectorAll("input, select, button").forEach(function (el) {
                el.disabled = true;
            });
        }

        function applyAttendanceUI() {

            document.querySelectorAll("#<%= grdGroup.ClientID %> tr").forEach(function (row) {

                var hidden = row.querySelector("input[id$='hidStatus']");
                if (!hidden) return;

                var val = hidden.value;

                var presentBtn = row.querySelector(".btn-present");
                var absentBtn = row.querySelector(".btn-absent");

                if (!presentBtn || !absentBtn) return;

                // reset
                presentBtn.classList.remove("active");
                absentBtn.classList.remove("active");

                // apply
                if (val === "1") {
                    presentBtn.classList.add("active");
                }
                else if (val === "0") {
                    absentBtn.classList.add("active");
                }
            });
        }

        if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {

            var prm = Sys.WebForms.PageRequestManager.getInstance();
        }

        function insertAttendanceToServer(row, val, callback) {

            var studentId = row.getAttribute("data-studentid");
            var classId = row.getAttribute("data-classid");
            var dateIso = getSelectedDateIso();

            callWebMethod('InsertCheckinForStudent',
            {
                studentId: parseInt(studentId, 10),
                classId: parseInt(classId, 10),
                dateIso: dateIso || "",
                isPresent: parseInt(val, 10)
            },
            function (res) {
                try {
                    var data = (res && res.d) ? res.d : res;

                    if (data && data.success) {
                        callback(true);
                    } else {
                        console.error("Insert failed response:", data);
                        alert("Insert failed");
                        callback(false);
                    }
                } catch (e) {
                    console.error("Insert exception:", e);
                    alert("Unexpected error");
                    callback(false);
                }
            });
        }

        function updateRowUI(toggleBtn) {
            var row = toggleBtn.closest("tr");

            // remove all states
            row.classList.remove("active-row", "row-absent", "row-default");

            if (toggleBtn.classList.contains("present-active")) {
                row.classList.add("active-row");
            } else {
                row.classList.add("row-absent");
            }
        }



        function renderExtrasFromData() {

            document.querySelectorAll('tr[data-extras]').forEach(function (row) {

                var extras = row.getAttribute('data-extras');
                if (!extras) return;

                var studentId = row.getAttribute('data-studentid');
                var classId = row.getAttribute('data-classid');

                var items = extras.split(';');

                items.forEach(function (item) {

                    if (!item) return;

                    var parts = item.split('|');

                    var exIn = decodeURIComponent(parts[0] || '');
                    var exOut = decodeURIComponent(parts[1] || '');
                    var exCode = decodeURIComponent(parts[2] || '');

                    addExtraRow(
                        row.querySelector('.add-btn'),
                        studentId,
                        classId,
                        exCode
                    );

                    var codeSelects = row.querySelectorAll('.code-stack .code-pair select');
                    var lastCode = codeSelects[codeSelects.length - 1];

                    // store value for later
                    lastCode.setAttribute("data-selected", exCode);

                    // delay population
                    setTimeout(function () {
                        populateCodeDropdown(lastCode, exCode);
                    }, 150);

                    var inInputs = row.querySelectorAll('.in-stack .extra-pair input');
                    var outInputs = row.querySelectorAll('.out-stack .extra-pair input');

                    var last = inInputs.length - 1;

                    if (inInputs[last]) inInputs[last].value = exIn;
                    if (outInputs[last]) outInputs[last].value = exOut;
                });
            });
        }

        function logExtrasFromGrid() {

            document.querySelectorAll('tr[data-extras]').forEach(function (row) {

                var studentId = row.getAttribute('data-studentid');
                var classId = row.getAttribute('data-classid');
                var extras = row.getAttribute('data-extras');

                console.log("Row:", studentId, classId);
                console.log("Extras RAW:", extras);

                if (extras) {
                    var parsed = extras.split(';').map(function (item) {
                        var parts = item.split('|');
                        return {
                            in: parts[0] || "",
                            out: parts[1] || "",
                            code: parts[2] || ""
                        };
                    });

                    console.log("Parsed Extras:", parsed);
                }

                console.log("-----------------------------");
            });
        }


        function debugHidden(studentId, classId) {
            var hid = document.getElementById("hidExtraTimes_" + studentId + "_" + classId);
            console.log("Hidden Value:", hid ? hid.value : "NOT FOUND");
        }

        document.addEventListener("change", function (e) {

            if (e.target.matches('.time-input, .code-select')) {

                var row = e.target.closest('tr');

                var studentId = row.getAttribute('data-studentid');
                var classId = row.getAttribute('data-classid');

                serializeExtraTimesForRow(row, studentId, classId);
            }
        });


        function populateCodeDropdown(select, selectedValue, retryCount) {

            if (!select) return;

            retryCount = retryCount || 0;

            console.log("populateCodeDropdown called");
            console.log("Codes inside dropdown:", window.__attendanceCodes);

            //wait for codes (with safety limit)
            if (!window.__attendanceCodes || window.__attendanceCodes.length === 0) {

                if (retryCount > 10) {
                    console.error("Codes not loaded after retries");
                    return;
                }

                setTimeout(function () {
                    populateCodeDropdown(select, selectedValue, retryCount + 1);
                }, 100);

                return;
            }

            //rebuild dropdown
            select.innerHTML = '<option value="">---Select---</option>';

            window.__attendanceCodes.forEach(function (c) {

                var opt = document.createElement("option");

                opt.value = String(c.id);
                opt.text = c.name;

                select.appendChild(opt);
            });

            //apply selection AFTER options added
            if (selectedValue !== undefined && selectedValue !== null && selectedValue !== "") {
                select.value = String(selectedValue);

                //fallback if value not found in options
                if (select.value !== String(selectedValue)) {
                    console.warn("Value not found in dropdown, adding fallback:", selectedValue);

                    var extraOpt = document.createElement("option");
                    extraOpt.value = String(selectedValue);
                    extraOpt.text = String(selectedValue);
                    select.appendChild(extraOpt);

                    select.value = String(selectedValue);
                }
            }

            console.log("Final selected:", select.value);
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
          <asp:AsyncPostBackTrigger ControlID="grdGroup" EventName="RowCommand" />
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
    SelectionMode="Day"
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
                    <div class="setBox" style="width:860px;">

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
                                    <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                      <ItemTemplate>
                                        <div class="att-toggle">
                                            <span class="att-btn btn-present"
                                                  onclick="toggleAttendance(this,1)">Present</span>

                                            <span class="att-btn btn-absent"
                                                  onclick="toggleAttendance(this,0)">Absent</span>
                                            <asp:HiddenField ID="hidStatus" runat="server" Value='<%# GetStatusHidden(Eval("IsPresent")) %>' />
                                        </div>
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
                                        <input type="hidden" name="hidExtraTimes_<%# Eval("studentid") %>_<%# Eval("ClassId") %>" id='hidExtraTimes_<%# Eval("studentid") %>_<%# Eval("ClassId") %>' value='<%# Eval("Extras") %>' />
                                      </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Add Row" >
                                      <HeaderStyle Width="55px" HorizontalAlign="Center" />
                                      <ItemStyle Width="55px" HorizontalAlign="Center" />
                                      <ItemTemplate>
                                        <asp:Button ID="btnAddTime" runat="server" Text="+"
                                          CssClass="add-btn"
                                          OnClientClick='<%# "try{ addExtraRow(this, " + Eval("studentid") + ", " + Eval("ClassId") + "); } catch(e){ console.error(e); } return false;" %>'
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
                                          CommandName="Save" CommandArgument='<%# Eval("studentid") + "|" + Eval("Classid") %>'
                                          OnClientClick="if(!validateGo(this)) return false;var row = this.closest('tr');var studentId = row.getAttribute('data-studentid');var classId = row.getAttribute('data-classid');serializeExtraTimesForRow(row, studentId, classId);debugHidden(studentId, classId);" CausesValidation="false" UseSubmitBehavior="false" />
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

        <script type="text/javascript">

            function applyAttendanceHighlight() {
                var grid = document.getElementById("<%= grdGroup.ClientID %>");
                if (!grid) return;

                grid.querySelectorAll("tr").forEach(function (row) {

                    var hidden = row.querySelector("input[id*='hidStatus']");
                    if (!hidden) return;

                    var val = hidden.value ? hidden.value.trim() : "";

                    var presentBtn = row.querySelector(".btn-present");
                    var absentBtn = row.querySelector(".btn-absent");

                    // RESET EVERYTHING
                    row.classList.remove("active-row", "row-absent", "row-default");

                    if (presentBtn) presentBtn.classList.remove("active");
                    if (absentBtn) absentBtn.classList.remove("active");

                    // APPLY STATE
                    if (val === "1") {
                        row.classList.add("active-row");
                        if (presentBtn) presentBtn.classList.add("active");
                    }
                    else if (val === "0") {
                        row.classList.add("row-absent");
                        if (absentBtn) absentBtn.classList.add("active");
                    }
                    else {
                        row.classList.add("row-default");
                    }
                });
            }

            //FIRST LOAD
            document.addEventListener("DOMContentLoaded", function () {
                applyAttendanceHighlight();
            });

            //AFTER UPDATE PANEL
            if (window.Sys && Sys.WebForms) {
            }



            var prm = Sys.WebForms.PageRequestManager.getInstance();

            if (prm) {

                prm.add_beginRequest(function () {
                    try {
                        if (window.showSaveLoader)
                            showSaveLoader("Saving...");
                    } catch (e) {}
                });
            }

</script>

    </form>
</body>
</html>
