<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ReportPendingTasksPage" Title="Pending Tasks Report" Codebehind="ReportPendingTasks.aspx.cs" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Pending Tasks Report" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td><sos:FilterSelector ID="sosFilterSelector" runat="server" ShowDates="true" ShowOptions="true" ShowEmployees="true" /></td>
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
    ID="repPendingTasks" 
    runat="server"
    Visible="true"
    BorderWidth="1"
    Height="520"
    Width="820">
</rsweb:ReportViewer>

<br />
<br />

</asp:Content>
