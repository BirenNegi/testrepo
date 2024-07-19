<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ReportBidsChartPage" Title="Bids Chart" CodeBehind="ReportBidsChart.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <sos:TitleBar ID="TitleBar" runat="server" Title="Bids Chart" />
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td class="frmForm">
                <table width="100%">
                    <tr>
                        <td class="frmReqLabel">Trade:</td>
                        <td>
                            <asp:RequiredFieldValidator ID="valTrade" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlTrades"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlTrades" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="frmReqLabel">Business Unit:</td>
                        <td>
                            <asp:RequiredFieldValidator ID="valBusinessUnit" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlBusinessUnit"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="frmReqLabel">Business Unit:</td>
                        <td>
                            <asp:RequiredFieldValidator ID="valTradeParticipationType" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlTradeParticipationType"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlTradeParticipationType" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                        <%-- San --%>
                    <tr>
                        <td class="frmReqLabel">From </td>
                        <td> <sos:DateReader ID="sdrStartDate" runat="server"></sos:DateReader></td>
                    </tr>
                       <%-- San --%>

                </table>
            </td>
        </tr>
        <tr>
            <td class="frmBottomBox">
                <asp:LinkButton ID="cmdGenerateReport" CssClass="frmButton" runat="server" OnClick="cmdGenerateReport_Click">View Report</asp:LinkButton>
                <asp:LinkButton ID="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:LinkButton>
            </td>
        </tr>
    </table>
    <br />

    <rsweb:ReportViewer
        ID="repBidsChart"
        runat="server"
        Visible="false"
        BorderWidth="1"
        Height="555px"
        Width="1398px">
    </rsweb:ReportViewer>
    <br />
    <br />

</asp:Content>

