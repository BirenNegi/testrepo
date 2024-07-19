<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListDrawingTypesPage" Title="Drawing Types" Codebehind="ListDrawingTypes.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="sosTitleBar" Title="Drawing Types" runat="server" />

<table cellspacing="1" cellpadding="1">
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" NavigateUrl="~/Modules/Trades/EditDrawingType.aspx" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>

    <tr>
        <td>
            <asp:GridView 
                ID="gvDrawingTypes" 
                runat="server" 
                AutoGenerateColumns="False" 
                OnRowDeleting="gvDrawingTypes_RowDeleting"
                DataKeyNames="Id"
                CellPadding="2" 
                CellSpacing="0" 
                CssClass="lstList" 
                RowStyle-CssClass="lstItem" 
                AlternatingRowStyle-CssClass="lstAltItem">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Trades/EditDrawingType.aspx?DrawingTypeId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete Drawing Type?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>                    
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>

