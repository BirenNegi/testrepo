<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditProjectFilesPage" Title="Project Files" Codebehind="EditProjectFiles.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<sos:TitleBar ID="TitleBar" runat="server" Title="Project Files" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Maintenance Manual File:</td>
                    <td colspan="4"><sos:FileSelect ID="sfsMaintenanceManualFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Post Project Review File:</td>
                    <td colspan="4"><sos:FileSelect ID="sfsPostProjectReviewFile" runat="server" /></td>
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

