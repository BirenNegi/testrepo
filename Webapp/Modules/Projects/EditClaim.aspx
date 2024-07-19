<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditClaimPage" Title="Claim" Codebehind="EditClaim.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Updating Claim" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table cellpadding="2" cellspacing="1">
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Draft Approval Date:</td>
                    <td class="frmData"><asp:Label ID="lblDraftApprovalDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Internal Approval Date:</td>
                    <td class="frmData"><asp:Label ID="lblInternalApprovalDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Final Approval Date:</td>
                    <td class="frmData"><asp:Label ID="lblApprovalDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Issue Date:</td>
                    <td><sos:DateReader ID="sdrDueDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Client Due Date:</td>
                    <td><sos:DateReader ID="sdrClientDueDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Adjustment Note Number(s):</td>
                    <td><asp:TextBox ID="txtAdjustmentNoteName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Adjustment Note Amount:</td>
                    <td>
                        <asp:CompareValidator ID="valAdjustmentNoteAmount" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtAdjustmentNoteAmount"></asp:CompareValidator>
                        <asp:TextBox ID="txtAdjustmentNoteAmount" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
            </table>
			<table id="tblClaim" runat="server" cellpadding="2" cellspacing="1">
				<tr>
					<td colspan="2" class="lstHeaderTop">Trade</td>
					<td colspan="2" class="lstHeaderTop">Previous Claim</td>
					<td colspan="2" class="lstHeaderTop">Updates This Claim</td>
				</tr>
				<tr>
					<td class="lstHeader">Name</td>
					<td class="lstHeader">Total</td>
					<td class="lstHeader">%</td>
					<td class="lstHeader">Amount</td>
					<td class="lstHeader">%</td>
					<td class="lstHeader">Amount</td>
				</tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>

</asp:Content>

