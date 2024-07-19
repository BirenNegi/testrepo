<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ListRFIsPage" Title="RFIs" Codebehind="ListRFIs.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Request For Information (RFIs)" />

<table>
  <%-- San --%> 
     <tr>
       <td  style="text-align:right">
        <asp:Button ID="BtnDwdRFIsSummary" runat="server" Text="RFIs Summary" OnClick="BtnDwdRFIsSummary_Click" /></td>  
      </tr>
   <%-- San --%>
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddRFI" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                   
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            <asp:GridView
                ID = "gvRFIs"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                   <%----%> <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewRFI.aspx?RFIId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:BoundField DataField="Subject" HeaderText="Subject" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>

                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.Web.Utils.GetConfigListItemName("RFI", "Status", (String)Eval("Status"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Raised date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("RaiseDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Answer by Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <span class='<%#(Boolean)Eval("IsOverdue") ? "lstItemError" : ""%>'><%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("TargetAnswerDate"))%></span>
                        </ItemTemplate>
                    </asp:TemplateField>                    

                    <asp:TemplateField HeaderText="Answered on Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ActualAnswerDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="RFI">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#IsComplete((SOS.Core.RFIInfo)DataBinder.Eval(Container, "DataItem"))%>' runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowRFI.aspx?RFIId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>

