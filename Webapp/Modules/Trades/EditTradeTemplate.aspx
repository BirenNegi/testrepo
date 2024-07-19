<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditTradeTemplatePage" Title="Trade Template" Codebehind="EditTradeTemplate.aspx.cs" %>
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
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Code:</td>
                    <td class="frmText"><asp:TextBox ID="txtCode" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Job Type:</td>
                    <td class="frmText"><asp:DropDownList ID="ddlJobTypes" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Standard Trade:</td>
                    <td class="frmText"><sos:BooleanReader ID="sbrIsStandard" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Description:</td>
                    <td class="frmText"><asp:TextBox ID="txtDescription" runat="server" Width="300"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Requires Tender:</td>
                    <td class="frmText"><sos:BooleanReader ID="sbrTenderRequired" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Days from PCD:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valDaysFromPCD" runat="server" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtDaysFromPCD"></asp:CompareValidator>
                        <asp:TextBox ID="txtDaysFromPCD" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Header:</td>
                    <td class="frmText" colspan="4"><asp:TextBox ID="txtScopeHeader" TextMode="MultiLine" Rows="12" runat="server" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Footer:</td>
                    <td class="frmText" colspan="4"><asp:TextBox ID="txtScopeFooter" TextMode="MultiLine" Rows="12" runat="server" Width="640"></asp:TextBox></td>
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

