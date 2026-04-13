<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/AdminMaster.master" AutoEventWireup="true"
    CodeFile="StudentAttendance.aspx.cs" Inherits="Administration_StudentAttendance" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <title>MPA Attendance</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContent" runat="Server">
    <asp:ScriptManagerProxy ID="smProxy" runat="server" />
    <asp:Panel ID="pnlFilters" runat="server" DefaultButton="btnGenerate">
        <%--<asp:TextBox ID="txtSearch" runat="server" placeholder="Student # / Name" style="visibility:hidden"/>--%>
        <div class="mpa-toolbar">
          <div class="mpa-toolbar">
          <div class="mpa-tool">
            <span class="ico ico--month"></span>
            <span class="mpa-label">Month</span>
            <asp:TextBox ID="txtMonth" runat="server" CssClass="mpa-month" />
            <asp:RequiredFieldValidator ID="rfvMonth" runat="server"
            ControlToValidate="txtMonth"
            ErrorMessage="Please select a month"
            ValidationGroup="Generate" Display="Dynamic" />
          </div>

          <asp:Button ID="btnGenerate" runat="server"
            Text="Generate" CssClass="btn btn-primary"
            OnClick="btnGenerate_Click"
            UseSubmitBehavior="true"
            CausesValidation="true" ValidationGroup="Generate"
            OnClientClick="
                if (typeof(Page_ClientValidate)==='function' && !Page_ClientValidate('Generate')) {
                    if (window.hideLoader) hideLoader(); return false;
                }
                return (window.showLoader ? window.showLoader() : true);" />

        <asp:Button ID="btnExport" runat="server"
            Text="Export to Excel"
            CssClass="btn btn-secondary"
            OnClick="btnExport_Click"
            CausesValidation="false"
            UseSubmitBehavior="true"
            OnClientClick="return startExport();" />
        </div>
      </div>
    </asp:Panel>
    <iframe id="downloadMonitor" style="display:none;"></iframe>
    <style>

    .sheet { 
        width:100%;
        height:600px;
        overflow:auto;
        border:1px solid #d9d9d9;
        border-radius:4px;
        margin-top:10px;
    }
    .sheet table { 
        border-collapse:collapse; 
        font-family:'Segoe UI', Tahoma, Arial; 
        font-size:12px;
        table-layout:fixed;
        width:100%;
    }
    .sheet th { 
        background:#e9eef7; 
        font-weight:600; 
        border:1px solid #cbd5e1; 
        padding:6px 4px; 
        white-space:nowrap;
        text-align:center;
    }
    .sheet td { 
        border:1px solid #e5e7eb; 
        padding:4px 6px; 
        white-space:nowrap; 
        text-align:center;
    }
    .idcol { 
        /*font-weight:bold;*/ 
        background:#f9fafb; 
    }



    table thead th {
    padding: 0px 5px 0px 5px;
    background: #bdd6ee;
    border-bottom: 3px none #333333;
    border: 1px solid #333333;
    height: 30px;
    color: black;
    font-size: 13px;
    text-align: center;
    font-weight: normal;
    line-height: 17px;
    text-shadow: 0 0px 0 #000;
    }
    #gvReport td, 
    #gvReport th {
        padding:5px !important;
    }
    #gvReport td, #gvReport th { padding:5px !important; }

    .mpaTitleCell{
    border:none !important;
    border-bottom:none !important;
    background:white;
    }

    .centerTitle{
    text-align:center !important;
}

    .nobordercell{
    border:none !important;
}

    .mpaTitleWrap{
        border:2px solid #000 !important;
    }

    .attWrap { width:99%; overflow-x:auto; overflow-y:hidden; }

    .attOuter{
      width:100%;
      border:1px solid #000;
      box-sizing:border-box;
      margin-bottom: 5px;
    }

    .attScroll{
      width:100%;
      overflow-x:auto;
      overflow-y:hidden;
    }

    .attGrid{
      width:auto !important;
      table-layout:auto;
      border-collapse:separate;
      border-spacing:0;
    }

    .attGrid th,
    .attGrid td{
      white-space:nowrap;
      padding:5px !important;
      box-sizing:border-box;
    }

    .attGrid .idcol    { min-width:120px; }
    .attGrid .emailcol { min-width:160px; }
    .attGrid .daycol   { min-width:44px; }
    .attGrid .sum-present,
    .attGrid .sum-tardy,
    .attGrid .sum-totalpt,
    .attGrid .sum-absent,
    .attGrid .sum-days { min-width:72px; }

    .sec-present { background:#C5E0B3; font-weight:600; color:black;}
  .sec-absent  { background:#F7CAAC; font-weight:600; color:black;}
  .sec-days    { background:#BDD6EE; font-weight:600; color:black;}

  .col-present   { background:#C5E0B3; }
  .col-tardy     { background:#E2EFD9; }
  .col-totalpt   { background:#C5E0B3; }
  .col-absent    { background:#F7CAAC; }
  .col-days      { background:#BDD6EE; }

  .sum-present { background:#C5E0B3; }
  .sum-tardy   { background:#E2EFD9; }
  .sum-totalpt { background:#C5E0B3; font-weight:600; }
  .sum-absent  { background:#F7CAAC; }
  .sum-days    { background:#BDD6EE; font-weight:600; }

  .dayLetterHeader { background-color: #deeaf6 !important;}

  .hdr { text-transform:none; }
  .idcol { background:#F5F5F5; }


  .mpa-toolbar {
    text-align:center;
    padding:14px 10px;
    margin:10px 0 16px;
    background:#f8fafc;
    border:1px solid #e5e7eb;
    border-radius:12px;
    box-shadow:0 1px 2px rgba(0,0,0,.04);
  }
  .mpa-tool {
    display:inline-block;
    vertical-align:middle;
    margin:6px 8px;
  }

  .mpa-label {
    font-weight:600;
    color:#334155;
    margin-right:6px;
  }

  .mpa-month {
    height:40px;
    min-width: 190px;
    padding:6px 12px;
    border:1px solid #cbd5e1;
    border-radius:10px;
    background:#fff;
    color:#0f172a;
    font-size:14px;
    outline:none;
    box-shadow:inset 0 1px 2px rgba(0,0,0,.03);
  }
  .mpa-month:focus {
    border-color:#3b82f6;
    box-shadow:0 0 0 3px rgba(59,130,246,.15);
  }

  .btn {
    display:inline-block;
    height:48px;
    line-height:48px;
    min-width:130px;
    font-size:15px;
    border-radius:12px;
}

  .btn:active { transform:translateY(1px); }

  .btn-primary {
    background:linear-gradient(#3b82f6,#2563eb);
    color:#fff;
  }
  .btn-primary:hover { box-shadow:0 4px 10px rgba(37,99,235,.25); }
  .btn-primary:focus { box-shadow:0 0 0 3px rgba(37,99,235,.35); }

  .btn-secondary {
    background:#fff;
    color:#0f172a;
    border-color:#cbd5e1;
  }
  .btn-secondary:hover {
    background:#f1f5f9;
    box-shadow:0 4px 10px rgba(15,23,42,.08);
  }
  .btn-secondary:focus { box-shadow:0 0 0 3px rgba(2,6,23,.08); }

  .btn[disabled], .aspNetDisabled.btn {
    opacity:.55;
    cursor:not-allowed;
    box-shadow:none;
  }

  .ico { display:inline-block; margin-right:8px; font-size:16px; vertical-align:middle; }
  .ico--gen::before   { content:"▶️"; }
  .ico--export::before{ content:"⬇️"; }
  .ico--month::before { content:"📅"; margin-right:6px; }

</style>
    <div class="attOuter">
      <div class="attScroll">
          <div id="noDataBox" runat="server" visible="false" 
             style="padding:40px;text-align:center;font-size:22px;
                    color:#a00;font-weight:bold;border:2px dashed #a00;
                    border-radius:10px;margin:20px auto;width:60%;
                    background:#fff3f3;">
            No Attendance Data Found For Selected Month
        </div>
            <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="true" CssClass="attGrid" />
      </div>
   </div>
    <script type="text/javascript">
        (function () {
            // Ensure the loader element exists; create it if missing
            function ensureLoaderEl() {
                var el = document.getElementById('pageLoader');
                if (!el) {
                    el = document.createElement('div');
                    el.id = 'pageLoader';
                    el.style.cssText = 'position:fixed;top:0;left:0;right:0;bottom:0;background:rgba(0,0,0,.35);z-index:99999;display:none;';

                    var box = document.createElement('div');
                    box.id = 'pageLoaderBox';
                    box.style.cssText = 'position:absolute;top:50%;left:50%;width:280px;margin-left:-140px;height:90px;margin-top:-45px;background:#fff;border:1px solid #c7c7c7;border-radius:6px;text-align:center;box-shadow:0 6px 18px rgba(0,0,0,.2);';
                    box.innerHTML = ''
                      + '<div style="font-family:Segoe UI, Arial, sans-serif;font-size:14px;font-weight:600;padding:16px 10px 6px;">Working…</div>'
                      + '<div style="font-family:Segoe UI, Arial, sans-serif;font-size:12px;color:#555;padding-bottom:8px;">Please wait while we prepare your report</div>'
                      + '<div style="width:36px;height:36px;margin:0 auto 14px;border:4px solid #e5e7eb;border-top-color:#6b7280;border-radius:50%;animation:spin 1s linear infinite;"></div>';

                    var style = document.createElement('style');
                    style.textContent = '@keyframes spin{from{transform:rotate(0)}to{transform:rotate(360deg)}}';

                    el.appendChild(box);
                    document.body.appendChild(el);
                    document.head.appendChild(style);
                }
                return el;
            }

            // Expose globals used by OnClientClick and server scripts
            window.showLoader = function () {
                var el = ensureLoaderEl();
                el.style.display = 'block';
                return true; // keep postback going
            };
            window.hideLoader = function () {
                var el = document.getElementById('pageLoader');
                if (el) el.style.display = 'none';
            };

            // If UpdatePanel is present, wire PRM events
            if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(function () { window.showLoader(); });
                prm.add_endRequest(function () { window.hideLoader(); });
            }

            // Safety: hide after full load
            window.addEventListener('load', function () { window.hideLoader(); });
        })();

        function startExport() {

            if (window.showLoader)
                showLoader();

            var monitor = document.getElementById("downloadMonitor");

            // detect when server response finishes
            monitor.onload = function () {
                if (window.hideLoader)
                    hideLoader();
            };

            // request a small completion ping
            monitor.src = "DownloadMonitor.aspx?t=" + new Date().getTime();

            return true; // allow export postback
        }
    </script>
</asp:Content>