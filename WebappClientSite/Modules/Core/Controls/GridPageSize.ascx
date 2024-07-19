<%@ Control Language="C#" AutoEventWireup="true" Inherits="SOS.Web.GridPageSizeControl" Codebehind="GridPageSize.ascx.cs" %>

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="border-top: solid 1px black; border-left: solid 1px black; border-right: solid 1px black;">
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="frmText">Records per page:</td>
                    <td><asp:LinkButton ID="lnkPage10" runat="server" OnClick="lnkPage10_Click" CssClass="frmLinkExtraSmall" Visible="false">10</asp:LinkButton><asp:Label ID="lblPage10" runat="server" CssClass="frmSubTitle" Visible="false">10</asp:Label></td>
                    <td>&nbsp;</td>
                    <td valign="middle"><asp:LinkButton ID="lnkPage25" runat="server" OnClick="lnkPage25_Click" CssClass="frmLinkExtraSmall" Visible="false">25</asp:LinkButton><asp:Label ID="lblPage25" runat="server" CssClass="frmSubTitle" Visible="false">25</asp:Label></td>
                    <td>&nbsp;</td>
                    <td><asp:LinkButton ID="lnkPage50" runat="server" OnClick="lnkPage50_Click" CssClass="frmLinkExtraSmall" Visible="false">50</asp:LinkButton><asp:Label ID="lblPage50" runat="server" CssClass="frmSubTitle" Visible="false">50</asp:Label></td>
                    <td>&nbsp;</td>
                    <td><asp:LinkButton ID="lnkPageAll" runat="server" OnClick="lnkPageAll_Click" CssClass="frmLinkExtraSmall" Visible="false">All</asp:LinkButton><asp:Label ID="lblPageAll" runat="server" CssClass="frmSubTitle" Visible="false">All</asp:Label></td>
                </tr>
            </table>        
        </td>
    </tr>
</table>


