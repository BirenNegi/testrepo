<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ListClaimsPage" Title="Progress Claims" Codebehind="ListClaims.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Progress Claims" />

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
                ID = "gvClaims"
                runat = "server"
                DataKeyNames = "Id"
                AutoGenerateColumns = "False"
                OnRowCreated = "gvClaims_RowCreated"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                   <%-- <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewClaim.aspx?ClaimId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
                    <asp:TemplateField HeaderText="Claim No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Date Submitted" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
							<%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("DraftApprovalDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                   <%-- <asp:TemplateField HeaderText="Approval" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
							<%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("InternalApprovalDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="FC" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
							<%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ApprovalDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="Invoice Issued" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("DueDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Due" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ClientDueDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Trades $" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((Decimal?)Eval("TotalClaimTrades"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="%" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#ClientTradesPercentage((SOS.Core.ClaimInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Variations $" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((Decimal?)Eval("TotalClaimVariations"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="%" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#ClientVariationsPercentage((SOS.Core.ClaimInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Adj Notes $" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((Decimal?)Eval("AdjustmentNoteAmount"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Total $" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((Decimal?)Eval("Total"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <%#InfoStatus((SOS.Core.ClaimInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#DateStatus((SOS.Core.ClaimInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="View Claim">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#Eval("IsApproved")%>' runat="server" NavigateUrl='<%#LinkClaim((SOS.Core.ClaimInfo)DataBinder.Eval(Container, "DataItem"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>
