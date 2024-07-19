<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListDrawingsPage" Title="Drawings" Codebehind="ListDrawings.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" />

<asp:Panel ID="pnlProposal" runat="server">
<table>
    <asp:PlaceHolder ID="phAddDrawing" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddDrawing" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            <asp:GridView
                ID = "gvDrawings"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewDrawing.aspx?DrawingId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Number" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Last Revision" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" CssClass="frmLink" NavigateUrl='<%#"~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + Eval("LastRevisionIdStr")%>'><%#SOS.UI.Utils.SetFormString((String)Eval("LastRevisionNumber"))%></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Last Revision Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("LastRevisionDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Revisions" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("NumRevisions"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<br />
</asp:Panel>
<br />

</asp:Content>

