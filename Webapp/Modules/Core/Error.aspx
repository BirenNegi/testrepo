<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ErrorPage" Title="SOS - Error" Codebehind="Error.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<br />
<table width="100%">
    <tr>
        <td align="center">
            <table width="480" class="frmForm">
                <tr>
                    <td align="center" class="frmSubSubTitle">
                        <br />
                        Error:
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
<br /></asp:Content>
