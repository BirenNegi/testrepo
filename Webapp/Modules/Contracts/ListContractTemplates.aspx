<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListContractTemplatesPage" Title="Contract Templates" Codebehind="ListContractTemplates.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="sosTitleBar" Title="Contract Templates" runat="server" />
<br />
<asp:PlaceHolder ID="phContratTemplases" runat="server"></asp:PlaceHolder>
<br />
<table>
    <tr>
        <td valign="top">
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="frmText"><b>Std:</b></td>
                    <td class="frmText">Standard</td>
                </tr>
                <tr>
                    <td class="frmText"><b>Sim:</b></td>
                    <td class="frmText">Simplified</td>
                </tr>
                <tr>
                    <td class="frmText"><b>Var:</b></td>
                    <td class="frmText">Variations</td>
                </tr>
            </table>
        </td>
        <td>&nbsp;&nbsp;</td>
        <td valign="top">
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td class="frmText"><b>(?):</b></td>
                    <td class="frmText">Not created</td>
                </tr>
                <tr>
                    <td class="frmText"><b>(*):</b></td>
                    <td class="frmText">Empty</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
