<%@ Control Language="C#" AutoEventWireup="true" Inherits="SOS.Web.ComparisonCheckControl" Codebehind="ComparisonCheck.ascx.cs" %>
<asp:TreeView ID="TreeView1" runat="server" ShowLines="true">
    <Nodes>
        <asp:TreeNode Expanded="False" Text="Comparison Check...Ok" SelectAction="None" />
    </Nodes>
    <LevelStyles>                
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="lstLinkBig" />
        <asp:TreeNodeStyle CssClass="frmError" />
        <asp:TreeNodeStyle CssClass="lstLink" />
    </LevelStyles>
</asp:TreeView>
