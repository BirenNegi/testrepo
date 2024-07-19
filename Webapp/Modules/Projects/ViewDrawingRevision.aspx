<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewDrawingRevisionPage" Title="Drawing Revision" Codebehind="ViewDrawingRevision.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Drawing Revision" />

<asp:Panel ID="pnlProposal" runat="server">
<table cellpadding="0" cellspacing="0">
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
    </asp:Panel>

    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Revision:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server" Width="150"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Date:</td>
                    <td class="frmData"><asp:Label ID="lblDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Comments:</td>
                    <td class="frmData"><asp:Label ID="lblComments" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">File:</td>
                    <td class="frmData"><sos:FileLabel ID="sfrRevisionFile" runat="server"></sos:FileLabel></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
</asp:Panel>
<br />

</asp:Content>
