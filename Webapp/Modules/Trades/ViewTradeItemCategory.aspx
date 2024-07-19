<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewTradeItemCategoryPage" Title="Trade Item Category" Codebehind="ViewTradeItemCategory.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Trade Item Category" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
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

                    <asp:PlaceHolder ID="phAddTradeItem" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkAddTradeItem" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                    </asp:PlaceHolder>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="aupItems" runat="server" UpdateMode="Conditional" RenderMode="Inline">
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
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Trades/ViewTradeItem.aspx?TradeItemId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
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
</table>
<br />

</asp:Content>
