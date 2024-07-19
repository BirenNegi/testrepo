<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListRDOsPage" Title="RODs" Codebehind="ListRDOs.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="sosTitleBar" Title="RDOs" runat="server" />

<table cellspacing="1" cellpadding="1">
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:Button ID="butAddNew" runat="server" OnClick="butAddNew_Click" Text="Add" /></td>
                    <td>&nbsp;</td>
                    <td><sos:DateReader ID="sdrRDODate" runat="server"></sos:DateReader></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>

    <tr>
        <td>
            <asp:GridView 
                ID="gvRDOs" 
                runat="server" 
                AutoGenerateColumns="False" 
                OnRowDeleting="gvRDOs_RowDeleting"
                OnPageIndexChanging="gvRDOs_PageIndexChanging"
                DataKeyNames="Date"
                AllowPaging="True"
                PageSize="25"
                PagerStyle-CssClass="lstPager"
                CellPadding="2" 
                CellSpacing="0" 
                CssClass="lstList" 
                RowStyle-CssClass="lstItem" 
                AlternatingRowStyle-CssClass="lstAltItem">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete RDO?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Year" HeaderText="Year" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Month" HeaderText="Month" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="Day" HeaderText="Day" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>
