<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewComparisonPage" Title="Comparison" Codebehind="ViewComparison.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Comparison" />

<asp:Panel ID="pnlProposal" runat="server">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <sos:CheckComparison ID="CheckComparison1" runat="Server"></sos:CheckComparison>
        </td>
    </tr>
    <tr>
        <td><div style="height:8px"></div></td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <table>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Image ID="imgEdit" runat="server" AlternateText="Edit ->" ImageUrl="~/Images/IconEdit.gif" /></td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:DropDownList ID="ddlParticipants" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlParticipants_SelectedIndexChanged">
                            <asp:ListItem Text="- Select to Edit -" Value="" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <sos:ViewComparison ID="ViewComparison1" runat="Server"></sos:ViewComparison>
        </td>
    </tr>
    <tr>
        <td class="frmTextSmall">&nbsp;</td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="lstItemAssumed" style="width:15px;"><span class="frmTextSmall">&nbsp;</span></td>
                            </tr>
                        </table>
                    </td>
                    <td class="frmTextSmall">Assumed/Transfered from BOQ</td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="lstItemConfirmed" style="width:15px;"><span class="frmTextSmall">&nbsp;</span></td>
                            </tr>
                        </table>
                    </td>
                    <td class="frmTextSmall">Confirmed by Subcontractor</td>
                </tr>
                <%-- San --%>
                <tr>
                    <td><table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="sanBorder" style="width:12px;"><span class="frmTextSmall">&nbsp;</span></td>
                            </tr>
                        </table></td>
                    <td class="frmTextSmall">Missinginformation /Comparison error</td>
                </tr>
                 <tr>
                    <td> <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="sBorder" style="width:12px;"><span class="frmTextSmall">&nbsp;</span></td>
                            </tr>
                        </table></td>
                    <td class="frmTextSmall">Missmatch in quantities</td>
                </tr>
                <tr>
                    <td><table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="lstHeaderTopPulledout" style="width:15px;"><span class="frmTextSmall">&nbsp;</span></td>
                            </tr>
                        </table></td>
                    <td class="frmTextSmall">Subcontractor not pricing</td>
                </tr>
                <%-- San --%>
            </table>
        </td>
    </tr>
</table>
<br />
</asp:Panel>
<br />

</asp:Content>
