<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewInvitationTemplatePage" Title="Invitation to Tender Template" Codebehind="ViewInvitationTemplate.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Invitation to Tender Template" />

<table cellpadding="0" cellspacing="0" width="750">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td class="frmForm"><asp:Literal ID="litTemplate" runat="server"></asp:Literal></td>
    </tr>
</table>
<br />

</asp:Content>
