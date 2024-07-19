<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewProjectTradeItemCategoryPage" Title="Trade Item Category" Codebehind="ViewProjectTradeItemCategory.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Trade Item Category" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image ID="Image1" runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image ID="Image2" runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
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
                    <td class="frmLabel">Short Description:</td>
                    <td class="frmData"><asp:Label ID="lblShortDescription" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Long Description:</td>
                    <td class="frmData"><asp:Label ID="lblLongDescription" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td>
            <table>
                <tr>
                    <td class="lstTitle">Items</td>

                    <asp:PlaceHolder ID="phAddTradeItem" runat="server" >
                    <td></td>
                    <td><asp:HyperLink ID="lnkAddTradeItem" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" Visible="false"></asp:HyperLink></td>
                    
                        
                    <td> <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="btnExport_Click" /></td><td>
                    &nbsp;</td>
                    <td class="auto-styleMAND">
                        <asp:Label ID="lblMessageBrowse" runat="server" Visible="false">Select Document to import:</asp:Label>
                    </td>
                    <td class="auto-style10">
                        <asp:FileUpload ID="ExcelUpload" runat="server" Width="239px"  Visible="false" />
                    </td>
                     <td><asp:Button ID="btnImport" runat="server" Text="Import From Excel" OnClick="btnImport_Click"  Visible="false" /></td>
                     <td><asp:Label ID="lblMessageResult" runat="server" /></td>

                    </asp:PlaceHolder>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="aupTradeItems" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostbackTrigger ControlID="gvTradeItems" EventName="RowCommand" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView
                        ID = "gvTradeItems"
                        runat = "server"
                        DataKeyNames="Id"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CellSpacing = "0"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem"
                        OnRowCommand = "gvTradeItems_RowCommand">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:HyperLink  ImageUrl="~/Images/IconView.gif" runat="server"  ToolTip="Open" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProjectTradeItem.aspx?TradeItemId={0}", Eval("IdStr"))%>'></asp:HyperLink>
 <%--                                   <asp:HyperLink  ImageUrl='<%#Server.MapPath("~/Images/IconView.gif")%>' runat="server"  ToolTip="Open" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProjectTradeItem.aspx?TradeItemId={0}", Eval("IdStr"))%>'></asp:HyperLink>
 --%>                               </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="Units" HeaderText="Units" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:TemplateField HeaderText="QC" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <sos:BooleanViewer ID="sbvRequiresQuantityCheck" Checked='<%#Eval("RequiresQuantityCheck")%>' runat="server"></sos:BooleanViewer>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RP" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <sos:BooleanViewer ID="sbvRequiredInProposal" Checked='<%#Eval("RequiredInProposal")%>' runat="server"></sos:BooleanViewer>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Scope of Works" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtScopeHeader" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Width="100%" Rows="3" runat="server" Text='<%#Eval("Scope")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField CommandName="MoveUp" ButtonType="Image" ImageUrl="~/Images/IconUp.gif" Text="Up" ItemStyle-VerticalAlign="Top" Visible="false" />
                            <asp:ButtonField CommandName="MoveDown" ButtonType="Image" ImageUrl="~/Images/IconDown.gif" Text="Down" ItemStyle-VerticalAlign="Top" Visible="false" />
                        </Columns>
                    </asp:GridView>    
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>


    <tr><td></td>
 

    </tr>


</table>
<br />

</asp:Content>
