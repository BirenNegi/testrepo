<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ChangePasswordPage" Title="Updating Password" Codebehind="ChangePassword.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar" runat="server" Title="Updating Password" Visible="false" />

<asp:Panel ID="pnlForm" runat="server">
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
                        <td class="frmLabel">First&nbsp;Name:</td>
                        <td class="frmData"><asp:Label ID="lblFirstName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="frmLabel">Last&nbsp;Name:</td>
                        <td class="frmData"><asp:Label ID="lblLastName" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="frmLabel">Email Address:</td>
                        <td class="frmData"><asp:Label ID="lblEmail" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="frmReqLabel">Current Password:</td>
                        <td class="frmText">
                            <asp:RequiredFieldValidator ID="valCurrentPassword" runat="server" ErrorMessage="Required Field!<br />" ControlToValidate="txtCurrentPassword" CssClass="frmError" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valCurrentPassword1" runat="server" ErrorMessage="Invalid length!<br />" ValidationExpression="\w{4,50}" ControlToValidate="txtCurrentPassword" CssClass="frmError" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="valCurrentPassword2" runat="server" ErrorMessage="Invalid password!<br />" CssClass="frmError" Display="Dynamic" OnServerValidate="valCurrentPassword2_ServerValidate"></asp:CustomValidator>
                            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" Width="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="frmReqLabel">New Password:</td>
                        <td class="frmText">
                            <asp:RequiredFieldValidator ID="valNewPassword" runat="server" ErrorMessage="Required Field!<br />" ControlToValidate="txtNewPassword" CssClass="frmError" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valNewPassword1" runat="server" ErrorMessage="Invalid length!<br />" ValidationExpression="\w{4,50}" ControlToValidate="txtNewPassword" CssClass="frmError" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Width="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="frmReqLabel">Confirmation:</td>
                        <td class="frmText">
                            <asp:RequiredFieldValidator ID="valConfirmation" runat="server" ErrorMessage="Required Field!<br />" ControlToValidate="txtConfirmation" CssClass="frmError" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valConfirmation1" runat="server" ErrorMessage="Invalid length!<br />" ValidationExpression="\w{4,50}" ControlToValidate="txtConfirmation" CssClass="frmError" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="valConfirmation2" runat="server" ErrorMessage="Password confirmation does not match!<br />" CssClass="frmError" Display="Dynamic" OnServerValidate="valConfirmation2_ServerValidate"></asp:CustomValidator>
                            <asp:TextBox ID="txtConfirmation" runat="server" TextMode="Password" Width="150"></asp:TextBox>
                        </td>
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
</asp:Panel>

<asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="frmFormMsg">
    <table>
        <tr>
            <td>Your password has been changed.</td>
        </tr>
    </table>
</asp:Panel>

</asp:Content>
