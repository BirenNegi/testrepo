<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListClientTradeTypesPage" Title="Client Trade Types" Codebehind="ListClientTradeTypes.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="sosTitleBar" Title="Client Trade Types" runat="server" />

<table cellspacing="1" cellpadding="1">
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Modules/Trades/EditClientTradeType.aspx" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>

    <tr>
        <td>
            <asp:GridView 
                ID="gvClientTradeTypes" 
                runat="server" 
                AutoGenerateColumns="False" 
                OnRowDeleting="gvClientTradeTypes_RowDeleting"
                DataKeyNames="Id"
                CellPadding="2" 
                CellSpacing="0" 
                CssClass="lstList" 
                RowStyle-CssClass="lstItem" 
                AlternatingRowStyle-CssClass="lstAltItem">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Trades/EditClientTradeType.aspx?ClientTradeTypeId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete Client Trade Type?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>                    
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>

