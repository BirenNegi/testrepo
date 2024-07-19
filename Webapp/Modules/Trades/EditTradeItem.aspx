<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditTradeItemPage" Title="Trade Item" Codebehind="EditTradeItem.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar" runat="server" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmReqLabel">Name:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" runat="server" Width="450"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Units:</td>
                    <td class="frmText"><asp:TextBox ID="txtUnits" runat="server" Width="450"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quantity Check:</td>
                    <td class="frmData"><sos:BooleanReader ID="sbrRequiresQuantityCheck" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Required in proposal:</td>
                    <td class="frmData"><sos:BooleanReader ID="sbrRequiredInProposal" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope of Works:</td>
                    <td class="frmText" colspan="4"><asp:TextBox ID="txtScope" TextMode="MultiLine" Rows="6" runat="server" Width="640"></asp:TextBox></td>
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

