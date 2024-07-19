<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewProjectTradeItemPage" Title="Trade Item" Codebehind="ViewProjectTradeItem.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Trade Item" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>

    <asp:Panel ID="pnlDeleteErrors" runat="server" Visible="false">
    <tr>
        <td>
            <asp:TreeView ID="TreeViewDeleteErrors" runat="server" ShowLines="true">
                <LevelStyles>
                    <asp:TreeNodeStyle CssClass="frmSubTitle" />
                    <asp:TreeNodeStyle CssClass="frmText" />
                    <asp:TreeNodeStyle CssClass="frmError" />
                </LevelStyles>
                <Nodes>
                    <asp:TreeNode></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
            <br />
        </td>
    </tr>
    </asp:Panel>

    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server" Width="150"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Units:</td>
                    <td class="frmData"><asp:Label ID="lblUnits" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quantity Check:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvRequiresQuantityCheck" runat="server"></sos:BooleanViewer></td>
                </tr>
                <tr>
                    <td class="frmLabel">Required in proposal:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvRequiredInProposal" runat="server"></sos:BooleanViewer></td>
                </tr>
                <tr>
                    <td class="frmLabel">Scope of works:</td>
                    <td class="frmData"><asp:TextBox ID="txtScope" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Width="640" Rows="6" runat="server"></asp:TextBox></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />

</asp:Content>
