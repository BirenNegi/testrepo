<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ReportWorkOrdersNewPage" Title="Work Orders Report" Codebehind="ReportWorkOrdersNew.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Work Orders Report" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td><sos:FilterSelector ID="sosFilterSelector" runat="server" ShowTradesRange="true" /></td>
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
    ID="repWorkOrders" 
    runat="server"
    Visible="false"
    BorderWidth="1"
    SizeToReportContent="true" Height="800px" Width="800px">
</rsweb:ReportViewer>
<br />
<br />

</asp:Content>