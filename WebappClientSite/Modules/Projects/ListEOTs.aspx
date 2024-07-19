<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ListEOTsPage" Title="EOTs" Codebehind="ListEOTs.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Extension Of Time (EOTs)" />

<table>
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddEOT" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            <asp:GridView
                ID = "gvEOTs"
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
                             <%--San --  <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewEOT.aspx?EOTId={0}", Eval("IdStr"))%>'></asp:HyperLink>--%>
                        
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewEOT.aspx?EOTId={0}&NODID=null", Eval("IdStr"))%>'></asp:HyperLink>
                        <%-- San --%>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:BoundField DataField="Cause" HeaderText="Cause" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="DelayPeriod" HeaderText="Delay Period" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    
                    <asp:TemplateField HeaderText="First Notice Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("FirstNoticeDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Written Notice Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("WriteDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Approval Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ApprovalDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Days Claimed" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormFloat((float?)Eval("DaysClaimed"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:TemplateField HeaderText="Days Approved" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormFloat((float?)Eval("DaysApproved"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="EOT">
                        <ItemTemplate>
                            <%--San--Passed editional parameter &NODID="+null
                                
                                <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#IsComplete((SOS.Core.EOTInfo)DataBinder.Eval(Container, "DataItem"))%>' runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowEOT.aspx?EOTId={0}, Eval("IdStr"))%>'></asp:HyperLink>
                       
                                
                            --%>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#IsComplete((SOS.Core.EOTInfo)DataBinder.Eval(Container, "DataItem"))%>' runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowEOT.aspx?EOTId={0}&NODID="+null, Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>

