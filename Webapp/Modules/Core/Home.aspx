<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits=" SOS.Web.HomePage" Title="SOS - Welcome" Codebehind="Home.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />

<sos:TitleBar ID="TitleBar1" runat="server" Title="My Pending Tasks / Approvals" />
  <asp:LinkButton ID="cmdApprovals" runat="server" Visible="true" ToolTip="Show Site Orders awaiting my approval" OnClick="cmdApprovals_Click"> <asp:Image runat="server" AlternateText="Approve" ImageUrl="~/Images/RedFlag.png" /></asp:LinkButton>

                        <asp:Label ID="lblApprovals" Visible="true" runat="server"></asp:Label>
<asp:LinkButton ID="cmdSiteOrders" runat="server" Visible="true" ToolTip="Show my open and recent Site Orders" OnClick="cmdSiteOrders_Click"> <asp:Image runat="server" AlternateText="View" ImageUrl="~/Images/RedFlag.png" /></asp:LinkButton>
                        <asp:Label ID="lblOrders" Visible="true" runat="server"></asp:Label>
<asp:LinkButton ID="cmdSiteOrdersAll" runat="server" Visible="true" ToolTip="Show all Site Orders" OnClick="cmdSiteOrdersAll_Click"> <asp:Image runat="server" AlternateText="View" ImageUrl="~/Images/RedFlag.png" /></asp:LinkButton>
    <asp:Label ID="lblOrdersAll" Visible="true" runat="server"></asp:Label>
<%-- <asp:LinkButton ID="cmdSiteOrderSub" runat="server" Visible="true" ToolTip="Site Order Sub Contractor View" OnClick="cmdSiteOrdersSub_Click"> <asp:Image runat="server" AlternateText="View" ImageUrl="~/Images/RedFlag.png" /></asp:LinkButton>
  
    <asp:Label ID="lblOrdersSub" Visible="true" runat="server"></asp:Label>--%>
                      
<table id="tblTasks" runat="server" cellpadding="4" cellspacing="1"></table>
<asp:HiddenField ID="hidRolesInfo" runat="server" Value="" />

<div id="divNoTasks" runat="server" visible="false">
    <br />
    <table>
        <tr>
            <td><asp:Image ID="imgNoTasks" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
            <td class="lstTitle">Projects with no pending tasks</td>
        </tr>
    </table>
    <asp:Panel ID="pnlNoTasks" runat="server" CssClass="collapsePanel" Height="0">
    <table id="tblNoTasks" runat="server" cellpadding="4" cellspacing="1"></table>
    </asp:Panel>
    <act:CollapsiblePanelExtender
        ID="cpe" 
        runat="Server" 
        Collapsed="True" 
        CollapsedSize="0" 
        TargetControlID="pnlNoTasks" 
        ExpandControlID="imgNoTasks" 
        CollapseControlID="imgNoTasks" 
        ImageControlID="imgNoTasks" 
        ExpandedImage="~/Images/IconCollapse.jpg" 
        CollapsedImage="~/Images/IconExpand.jpg" 
        ExpandDirection="Vertical">
    </act:CollapsiblePanelExtender>
</div>

<asp:Label ID="lblProjectsInfo" runat="server" Visible="false" CssClass="lstTitle">You don't have active projects.</asp:Label>

</asp:Content>
