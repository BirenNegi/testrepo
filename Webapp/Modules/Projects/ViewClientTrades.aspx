<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewClientTradesPage" Title="Client Trades" Codebehind="ViewClientTrades.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<sos:TitleBar ID="TitleBar1" runat="server" Title="Client Trades" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td>
                        <table id="tblTrades" cellpadding="4" cellspacing="1" runat="server">
                            <tr>
                                <td class="lstBlankItem" colspan="2" id="cellSort" visible="false"></td>
                                <td class="lstHeader">Trade Name</td>
                                <td class="lstHeader" align="center">%</td>
                                <td class="lstHeader" align="center">Amount $</td>
                                <td class="lstHeader" align="center">Claimed $</td>
                                <td class="lstHeader" colspan="2"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblError" runat="server" CssClass="frmError" Visible="false"></asp:Label>
                                </td>
                                <td align="right">
                                    <table cellpadding="4" cellspacing="1">
                                        <tr>
                                            <td class="lstHeader">Contract Amount:</td>
                                            <td class="lstAltItem" align="right"><asp:Label ID="lblContractAmount" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="lstHeader">Trades Amount:</td>
                                            <td class="lstAltItem" align="right"><asp:Label ID="lblTradesAmount" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="lstHeader">Difference:</td>
                                            <td class="lstAltItem" align="right" id="tdDifference" runat="server"><asp:Label ID="lblDifference" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>                        
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

</asp:Content>
