<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ReportSubcontractorVariationsOverTime" Title="Subcontractor's Variations VsTime" Codebehind="ReportSubcontractorVariationsOverTime.aspx.cs" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="ReportSubcontractorVariationsOverTime" />


   <table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel" id="tdProject" runat="server">Project:</td>
                    <td>
                        <asp:UpdatePanel ID="upProjects" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlProjects" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                        &nbsp;
                        <asp:CheckBox ID="chkAll" Checked="false" AutoPostBack="true" runat="server" OnCheckedChanged="chkAll_CheckedChanged" />
                        <span class="frmTextSmall">List All</span>
                    </td>
                </tr>
               
                <tr>
                    <td class="frmLabel">Subcontractor:</td>
                    <td><asp:DropDownList ID="ddlSubbies" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Business Unit</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList></td>                
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
    ID="repVariationsOverTime" 
    runat="server"
    Visible="false"
    BorderWidth="1"
    Height="850px"
    Width="1550px">
</rsweb:ReportViewer>
<br />
<br />

</asp:Content>






