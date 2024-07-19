 <%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ReportKPIAnalysis" Title="KPI Analysis Report" Codebehind="ReportKPIAnalysis.aspx.cs" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="KPI Analysis" />

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
    ID="RepKPIAnalysis" 
    runat="server"
    Visible="True"
    BorderWidth="1px"
    Height="1620px"
    Width="1284px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
   
</rsweb:ReportViewer>
<br />
<br />

</asp:Content>