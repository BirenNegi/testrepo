<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditParticipationItemPage" Title="Item" Codebehind="EditParticipationItem.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar" runat="server" Title="Updating Item" />

<asp:Panel ID="pnlProposal" runat="server">
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
                    <td class="frmLabel">Subcontractor:</td>
                    <td class="frmData"><asp:Label ID="lblSubcontractor" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Item Category:</td>
                    <td class="frmData"><asp:Label ID="lblCategory" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Item:</td>
                    <td class="frmData"><asp:Label ID="lblItem" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Unit:</td>
                    <td class="frmData"><asp:Label ID="lblUnits" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quantity:</td>
                    <td class="frmText"><asp:TextBox ID="txtQuantity" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Included:</td>
                    <td class="frmText"><sos:BooleanReader ID="sbrIncluded" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Amount:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valAmount" runat="server" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtAmount"></asp:CompareValidator>
                        <asp:TextBox ID="txtAmount" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Confirmed:</td>
                    <td class="frmText"><asp:CheckBox ID="chkConirmed" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Notes:</td>
                    <td class="frmText"><asp:TextBox ID="txtNotes" TextMode="MultiLine" Rows="6" runat="server" Width="450" MaxLength="1000"></asp:TextBox></td>
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
<br />
</asp:Panel>
<br />

</asp:Content>

