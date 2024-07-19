﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ReportDesignVariation.aspx.cs" Inherits="SOS.Web.ReportDesignVariation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Design Variation" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Business Unit:</td>
                    <td>
                        <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="true" onselectedindexchanged="ddlBusinessUnit_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel" id="tdProject" runat="server">Project</td>
                    <td>
                        <asp:UpdatePanel ID="upProjects" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlBusinessUnit" EventName="SelectedIndexChanged" />
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
                    <td class="frmLabel">Trade:</td>
                    <td><asp:DropDownList ID="ddlTrades" runat="server"></asp:DropDownList></td>
                </tr>
               <%-- <tr>
                    <td class="frmLabel" id="tdSubcontractors" runat="server">Subcontractor:</td>
                    <td>                    
                        <asp:UpdatePanel ID="upSubcontractors" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlBusinessUnit" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSubbies" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Project Manager:</td>
                    <td><asp:DropDownList ID="ddlPerson" runat="server"></asp:DropDownList></td>
                </tr>--%>
                <tr>
                    <td class="frmLabel">From:</td>
                    <td><sos:DateReader ID="sdrStartDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">To:</td>
                    <td><sos:DateReader ID="sdrEndDate" runat="server"></sos:DateReader></td>
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
    ID="repDesignVariation" 
    runat="server"
    Visible="false"
    BorderWidth="1"
    Height="603px"
    Width="1252px">
</rsweb:ReportViewer>
<br />
<br />

<act:UpdatePanelAnimationExtender ID="upaeProjects" runat="server" TargetControlID="upProjects">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdProject" Duration="0.5" Fps="20" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<%--<act:UpdatePanelAnimationExtender ID="upaeSubcontractors" runat="server" TargetControlID="upSubcontractors">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdSubcontractors" Duration="0.5" Fps="20" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>--%>

</asp:Content>