<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.CreateAccountContactPage" Title="Contact Account" Codebehind="CreateAccountContact.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar" runat="server" Title="Subcontractor Contact - Creating Access Account" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Create Account</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">First Name:</td>
                    <td class="frmData"><asp:Label ID="lblFirstName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Last&nbsp;Name:</td>
                    <td class="frmData"><asp:Label ID="lblLastName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Email Address:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valEmail" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtEmail" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5" class="frmSection">Access Infomation</td>
                </tr>
                <tr>
                    <td class="frmReqLabel">User Name:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valUserName" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtUserName" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmReqLabel">User Type:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valAccessLevel" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="ddlAccessLevel"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddlAccessLevel" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Create Account</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>

</asp:Content>

