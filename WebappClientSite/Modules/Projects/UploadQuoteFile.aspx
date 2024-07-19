<%@ Page Language="C#" MasterPageFile="~/MasterPages/Subbie.master" AutoEventWireup="true" Inherits="SOS.Web.UploadQuoteFilePage" Title="Upload Quote File" Codebehind="UploadQuoteFile.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar" runat="server" Title="Uploading Quote File" Visible="false" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td colspan="2"><asp:CustomValidator ID="valQuotefile" runat="server" CssClass="frmError" OnServerValidate="ServerValidation" Display="Dynamic"></asp:CustomValidator></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quote File:</td>
                    <td class="frmData"><input id="fileQuote" type="file" size="60" name="fileQuote" runat="server" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpload_Click">Upload File</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>
<br />

</asp:Content>
