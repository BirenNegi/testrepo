<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListClientVariationsPage" Title="" Codebehind="ListClientVariations.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="" />

<table cellspacing="0" cellpadding="0" width="100%">
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            <asp:GridView
                ID = "gvClientVariations"
                runat = "server"
                DataKeyNames = "Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RevisionName" HeaderText="R" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>                    
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Write Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("WriteDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Verbal Approval" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("VerbalApprovalDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Final Approval" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ApprovalDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("TotalAmount"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Process Status" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#InfoStatus((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#DateStatus((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#Eval("IsInternallyApproved")%>' runat="server" NavigateUrl='<%#LinkClientVariation((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="True" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>
