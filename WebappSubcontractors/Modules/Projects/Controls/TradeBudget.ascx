<%@ Control Language="C#" AutoEventWireup="true" Inherits="SOS.Web.TradeBudgetControl" Codebehind="TradeBudget.ascx.cs" %>

<asp:HiddenField ID="hidSelectedId" runat="server" />

<sos:BalanceInclude id="sbiTrade" runat="server"></sos:BalanceInclude>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="frmForm">
                        <table id="tblTrades" cellpadding="4" cellspacing="1" runat="server">
                            <tr>
                                <td class="lstHeader" align="center" colspan="5">Budget Source</td>

                                <%-- Uncoment to show trade code budget 
                                <td class="lstHeaderTop" align="center" colspan="2">Revised Budget</td>
                                --%>

                                <td class="lstHeader" align="center" colspan="2">Allocation</td>
                                <td class="lstHeader" align="center" colspan="2">Win/Loss</td>
                                <td class="lstHeader" colspan="2" rowspan="2"></td>
                            </tr>
                            <tr>
                                <td class="lstHeader">Type</td>
                                <td class="lstHeader">Name</td>
                                <td class="lstHeader">Code</td>
                                <td class="lstHeader" align="center">Original</td>
                                <td class="lstHeader" align="center">Unallocated</td>

                                <%-- Uncoment to show budget source current balance
                                <td class="lstHeader" align="center">Current</td>
                                --%>

                                <%-- Uncoment to show trade code budget
                                <td class="lstHeader" align="center">Original</td>
                                <td class="lstHeader" align="center">Current</td>
                                --%>

                                <td class="lstHeader" align="center">Comparison</td>
                                <td class="lstHeader" align="center">Contract</td>
                                <td class="lstHeader" align="center">Comparison</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <!-- Uncomment to show trade codes allocation
        <td>&nbsp;</td>
        <td valign="top">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="frmForm">
                        <table id="tblCodes" cellpadding="4" cellspacing="1" runat="server">
                            <tr>
                                <td class="lstHeader" colspan="2">Trade Codes</td>
                            </tr>
                            <tr>
                                <td class="lstHeader">Code</td>
                                <td class="lstHeader">Allocation</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        -->
    </tr>
</table>

<asp:Panel ID="pnlOldBudget" runat="server" Visible="false">
<table style="background-color:yellow;">
    <tr>
        <td class="frmText">Budget (old system):</td>
        <td class="frmTextBold"><asp:Label ID="lblBudget" runat="server"></asp:Label></td>
    </tr>
</table>
</asp:Panel>
