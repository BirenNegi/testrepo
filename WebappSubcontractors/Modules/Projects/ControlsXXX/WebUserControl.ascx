<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl.ascx.cs" Inherits="SOS.Web.WebUserControl1" %>
<%--<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ComparisonCheck.ascx.cs" Inherits="SOS.Web.ComparisonCheckControl"  %>--%>
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