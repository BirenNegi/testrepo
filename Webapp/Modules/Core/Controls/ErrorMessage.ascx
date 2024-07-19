<%@ Control Language="C#" AutoEventWireup="true" Inherits="SOS.Web.ErrorMessage" Codebehind="ErrorMessage.ascx.cs" %>
<asp:PlaceHolder ID="phError" runat="server" Visible="false">
<br />
<table width="100%">
    <tr>
        <td align="center">
            <table width="480" class="frmForm">
                <tr>
                    <td align="center" class="frmSubSubTitle">
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
                        <br />
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
</asp:PlaceHolder>