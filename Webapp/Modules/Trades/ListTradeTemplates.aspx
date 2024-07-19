<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListTradeTemplatesPage" Title="Trade Templates" Codebehind="ListTradeTemplates.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="sosTitleBar" Title="Trade Templates" runat="server" />

<table cellspacing="1" cellpadding="1">
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Modules/Trades/EditTradeTemplate.aspx" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
   
    <tr>
        <td>
            <asp:UpdatePanel ID="aupTrades" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                <Triggers>
                    <asp:AsyncPostbackTrigger ControlID="gvTradesTemplates" EventName="RowCommand" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView 
                        ID="gvTradesTemplates" 
                        runat="server" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="Id"
                        CellPadding="2" 
                        CellSpacing="0" 
                        CssClass="lstList" 
                        RowStyle-CssClass="lstItem" 
                        AlternatingRowStyle-CssClass="lstAltItem"
                        OnRowCommand = "gvTradesTemplates_RowCommand">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Trades/ViewTradeTemplate.aspx?TradeTemplateId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TradeName" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>
                            <asp:BoundField DataField="TradeCode" HeaderText="Code" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>
                            <asp:BoundField DataField="TradeDescription" HeaderText="Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>
                            <asp:BoundField DataField="TradeJobTypeName" HeaderText="Job Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>
                            <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Std">
                                <ItemTemplate>
                                    <sos:BooleanViewer ID="sbvIsStandar" runat="server" Checked='<%#Eval("IsStandard")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Days PCD" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormInteger((int?)Eval("TradeDaysFromPCD"))%>
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

</asp:Content>
