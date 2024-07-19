<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ReportSAsPage" Title="Separate Accounts Report" Codebehind="ReportSAs.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Separate Accounts" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">From:</td>
                    <td><sos:DateReader ID="sdrStartDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">To:</td>
                    <td><sos:DateReader ID="sdrEndDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Status:</td>
                    <td><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList></td>                
                </tr>
                <!---#--Filtered by Active Projects--->
                <tr>
                    <td class="frmLabel">Active Project Name:</td>
                    <td><asp:DropDownList ID="DdlProjectName" runat="server"></asp:DropDownList></td>                
                </tr>
                 <!---#----->

            </table>
        </td>
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
    ID="repSAs" 
    runat="server"
    Visible="false"
    BorderWidth="1"
    Height="520"
    Width="820">
</rsweb:ReportViewer>
<br />
<br />

</asp:Content>
