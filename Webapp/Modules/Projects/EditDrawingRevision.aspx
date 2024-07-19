<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditDrawingRevisionPage" Title="Drawing Revision" Codebehind="EditDrawingRevision.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" />

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
                    <td class="frmReqLabel">Revision:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valNumber" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtNumber"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtNumber" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Date:</td>
                    <td><sos:DateReader ID="sdrDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">File: <span class="frmTextSmall">(In Drawings Folder)</span></td>
                    <td class="frmText"><sos:FileSelect ID="sfsFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td colspan="4"><asp:TextBox ID="txtComments" runat="server" Width="450"></asp:TextBox></td>
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
