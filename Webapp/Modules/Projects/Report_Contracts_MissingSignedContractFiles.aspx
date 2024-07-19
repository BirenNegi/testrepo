<%@ Page Title="Contracts without any Signed contract files" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="Report_Contracts_MissingSignedContractFiles.aspx.cs" Inherits="SOS.Web.Report_Contracts_MissingSignedContractFiles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Contracts without any uploaded signed contract files" />

    <table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                
                <tr>
                    <td class="frmLabel">Active Project Name:</td>
                    <td><asp:DropDownList ID="DdlProjectName" runat="server"></asp:DropDownList></td>                
                </tr>
                 <tr>
                    <td class="frmLabel">Business Unit</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList></td>                
                </tr>
                
               
                <!---#--Filtered by Active Projects--->
                
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

<rsweb:ReportViewer ID="RVContracts"     runat="server"    Visible="False"   Height="830px"    Width="820px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
   </rsweb:ReportViewer>
<br />
<br />




</asp:Content>
