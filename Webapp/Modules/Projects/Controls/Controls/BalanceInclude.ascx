<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BalanceInclude.ascx.cs" Inherits="SOS.Web.BalanceInclude" %>
<table cellpadding="0" cellspacing="0" style="width: 640px;">
    <tr>
        <td class="frmText" valign="center">Balance include: </td>
        <td class="frmText" valign="center">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="center">
                        <asp:RadioButtonList ID="rblIncludeCVSA" OnSelectedIndexChanged="rblInclude_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Text="Approved CV/SA" Selected="True" Value="Approved"></asp:ListItem>
                            <asp:ListItem Text="All CV/SA" Selected="False" Value="All"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td valign="center" class="frmSubTitle"> + </td>
                    <td valign="center">
                        <asp:RadioButtonList ID="rblIncludeOVO" OnSelectedIndexChanged="rblInclude_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Text="Approved Orders/Variation" Selected="False" Value="Approved"></asp:ListItem>
                            <asp:ListItem Text="All Orders/Variation" Selected="True" Value="All"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
