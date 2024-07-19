<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ListClientVariationsPage" Title="" Codebehind="ListClientVariations.aspx.cs" %>
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
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server"  NavigateUrl='<%#String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RevisionName" HeaderText="Revision" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>                    
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Date Submitted" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("WriteDate"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Verbal Approval" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("VerbalApprovalDate"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Final Approval" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ApprovalDate"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("TotalAmount"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <%----%><asp:TemplateField HeaderText="Process Status" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top" Visible="False">
                        <ItemTemplate>
                            <%#InfoStatus((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeaderTop"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center" Visible="False">
                        <ItemTemplate>
                            <%#DateStatus((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeaderTop"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="View Variations">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#Eval("IsInternallyApproved")%>' runat="server" NavigateUrl='<%#LinkClientVariation((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>'></asp:HyperLink>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="True" ItemStyle-VerticalAlign="Top"></asp:BoundField>--%>
                </Columns>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>
