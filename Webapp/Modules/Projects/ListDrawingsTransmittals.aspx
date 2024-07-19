<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListDrawingsTransmittalsPage" Title="Drawings/Transmittals" Codebehind="ListDrawingsTransmittals.aspx.cs" %>

<asp:Content ID="Content" ContentPlaceHolderID="Scripts" runat="server">
    <asp:Literal ID="litScriptDeepZoom" runat="server"></asp:Literal>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Project Drawings and Transmittals" />

<table>
    <tr>
        <td><asp:Image ID="imgProposal" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Proposal</td>
    </tr>
</table>

<asp:Panel ID="pnlProposal" runat="server" CssClass="collapsePanelProposal" Height="0">
<table>
    <asp:Panel ID="pnlCopyDrawings" runat="server" Visible="false">
    <tr>
        <td colspan="3"><asp:LinkButton id="cmdCopyDrawings" runat="server" CssClass="frmButton" OnClick="cmdCopyDrawings_Click" OnClientClick="javascript:return confirm('Copy to active drawing register ?');">Copy to active drawing register</asp:LinkButton></td>
    </tr>
    </asp:Panel>
    
    <tr>
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkDrawingsProposal" runat="server" ToolTip="Latest Drawings" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td>
                        <asp:TreeView ID="tvDrawingsProposal" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Drawings"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLink" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
        <td>&nbsp;</td>
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkTransmittalsProposal" runat="server" ToolTip="Transmittals Summary" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td valign="top">
                        <asp:PlaceHolder ID="phAddTransmittalProposal" runat="server" Visible="false">
                        &nbsp;
                        <asp:HyperLink ID="lnkAddTransmittalProposal" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink>
                        </asp:PlaceHolder>
                    </td>
                    <td>
                        <asp:TreeView ID="tvTransmittalsProposal" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Transmittals"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstSubTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<br />

<asp:Panel ID="pnlStatusActive" runat="server">
<table>
    <tr>
        <asp:Panel ID="pnlDeepZoom" runat="server">
            <td valign="top">
                <div id="divDeepZoom" runat="server" class="DeepZoom" visible="false"></div>
                <act:Seadragon ID="actSeadragon" runat="server" CssClass="DeepZoom" Visible="false"></act:Seadragon>
            </td>
            <td>&nbsp;</td>
        </asp:Panel>
        
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkDrawings" runat="server" ToolTip="Latest Drawings" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td>
                        <asp:TreeView ID="tvDrawings" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Drawings"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLink" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
        <td>&nbsp;</td>
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkTransmittals" runat="server" ToolTip="Transmittals Summary" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td valign="top">
                        <asp:PlaceHolder ID="phAddTransmittal" runat="server" Visible="false">
                        &nbsp;
                        <asp:HyperLink ID="lnkAddTransmittal" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink>
                        </asp:PlaceHolder>
                    </td>
                    <td>
                        <asp:TreeView ID="tvTransmittals" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Transmittals"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstSubTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
</asp:Panel>

<act:CollapsiblePanelExtender
    ID="cpe1"
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlProposal"
    ExpandControlID="imgProposal"
    CollapseControlID="imgProposal"
    ImageControlID="imgProposal"         
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

</asp:Content>
