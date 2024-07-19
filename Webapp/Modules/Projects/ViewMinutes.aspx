<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewMinutesPage" Title="Order Letting Minutes" Codebehind="ViewMinutes.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Order Letting Minutes" />

<asp:Panel ID="pnlErrors" runat="server" Visible="false">
<asp:TreeView ID="TreeView1" runat="server" ShowLines="true">
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
</asp:Panel>

<table cellpadding="0" cellspacing="0" width="750">
    <tr>
        <td>
            <asp:HyperLink ID="lnkPrint" runat="server" ToolTip="Print" ImageUrl="~/Images/IconPrint.gif"></asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td><asp:Literal ID="litMinutes" runat="server"></asp:Literal></td>
    </tr>
</table>
<br />

</asp:Content>
