<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ShowProjectSnapShot.aspx.cs" Inherits="SOS.Web.ShowProjectSnapShot" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Project Snapshot" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmReqLabel" id="tdProject" runat="server">Project:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlProjects"></asp:RequiredFieldValidator>

                        <asp:UpdatePanel ID="upProjects" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlProjects" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        &nbsp;
                        <asp:CheckBox ID="chkAll" Checked="false" AutoPostBack="true" runat="server" CausesValidation="false" OnCheckedChanged="chkAll_CheckedChanged" />
                        <span class="frmTextSmall">List All</span>
                    </td>
                </tr>
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
    ID="repPROJECTSNAPSHOT" 
    runat="server"
    Visible="False"
    BorderWidth="1px"
    Height="850px"
    Width="1200px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowBackButton="False" ShowPageNavigationControls="False">
</rsweb:ReportViewer>
<br />
<br />
    </asp:Content>