<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.CreateContractPage" Title="Create Contract" Codebehind="CreateContract.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar" runat="server" Title="Create Contract" />

<asp:Panel ID="pnlErrors" runat="server" Visible="false">
<asp:TreeView ID="TreeView1" runat="server" ShowLines="true">
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="frmText" />
        <asp:TreeNodeStyle CssClass="frmError" />
    </LevelStyles>
</asp:TreeView>
<br />
</asp:Panel>

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Generate Contract</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel" valign="top">Check List</td>
                    <td>
                        <asp:CustomValidator ID="valCheckList" runat="server" CssClass="frmError" Display="Dynamic" ErrorMessage="Check all the items or provide a comment." OnServerValidate="valCheckList_ServerValidate"  ></asp:CustomValidator>
                        <table>
                            <tr>
                                <td class="frmLabel">Quotes:</td>
                                <td><asp:CheckBox ID="chkQuotes" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="frmLabel">Winning Quote:</td>
                                <td><asp:CheckBox ID="chkWinningQuote" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="frmLabel">Comparison:</td>
                                <td><asp:CheckBox ID="chkComparison" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="frmLabel">Check List:</td>
                                <td><asp:CheckBox ID="chkCheckList" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="frmLabel">Order Letting Minutes:</td>
                                <td><asp:CheckBox ID="chkPrelettingMinutes" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="frmLabel">Amendments to comparison per order letting meeting:</td>
                                <td><asp:CheckBox ID="chkAmendments" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="frmLabel">Retention Required:</td>
                                <td><asp:CheckBox ID="chkRetentionReq" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" Width="320"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contract Template:</td>
                    <td>
                        <asp:DropDownList ID="ddlTemplate" runat="server">
                            <asp:ListItem Text="Standard" Value="Standard" Selected="True"></asp:ListItem>
                           <%-- <asp:ListItem Text="Simplified" Value="Simplified"></asp:ListItem> DS20230927--%>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Generate Contract</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>

</asp:Content>

