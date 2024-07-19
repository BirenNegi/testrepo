<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ReportTurnoverVsTime" Title="Report Turnover Vs Time" Codebehind="ReportTurnoverVsTime.aspx.cs" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Report Turnover Vs Time" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td><sos:FilterSelector ID="sosFilterSelector" runat="server" ShowDates="true" ShowEmployees="true" /></td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:LinkButton ID="cmdGenerateReport" CssClass="frmButton" runat="server" OnClick="cmdGenerateReport_Click">View Report</asp:LinkButton>
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>
<br />

<rsweb:ReportViewer 
    ID="repTurnOver" 
    runat="server"
    Visible="false"
    BorderWidth="1"
    Height="820px"
    Width="1550px">
</rsweb:ReportViewer>
<br />
<br />

</asp:Content>





