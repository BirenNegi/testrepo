<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SearchSiteOrderDetailInst" Title="Orders" Codebehind="SearchSiteOrderDetailInst.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <sos:TitleBar ID="TitleBar1" runat="server" Title="View Site Order Items " />

 <table>
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1">
                <tr>
 <td><asp:LinkButton ID="lnkAddItems" runat="server" Visible="True" OnClick="cmdAddItem_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconAdd.gif" /></asp:LinkButton></td>                   
        <tr>
             <td class="auto-styleBTN">

                <asp:LinkButton ID="cmdCancelTop" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdCancel_Click">Exit</asp:LinkButton>
            </td>
        </tr>
                     <%-- <td class="lstTitle">Site Order Items</td> --%>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
     
            <asp:GridView
                ID = "gvSiteOrderItems"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                   <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
<asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/SiteOrders/EditSiteOrderDetailInst.aspx?ProjectId={0}&OrderId={1}&ItemId={2}&OrderTyp={3}", Request.QueryString["ProjectId"].ToString(),Request.QueryString["OrderId"].ToString(), Eval("IdStr"),Request.QueryString["OrderTyp"])%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Title" HeaderText="Item" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C2}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

               <asp:TemplateField>
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    <ItemTemplate>
                       
                         <asp:LinkButton ID="lnkDelete" runat="server" UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to delete the item?');" CommandArgument='<%#String.Format("{0}", Eval("Id")) %>' onclick="cmdDeleteItem_Click">
                          <asp:Image ID="imgdelete" runat="server" ImageUrl="~/Images/IconDelete.gif" />
                         </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>
<br />

</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            width: 362px;
        }
        .auto-style3 {
            width: 190px;
        }
        .auto-style4 {
            width: 131px;
        }
        .auto-style5 {
            width: 11px;
        }
        .auto-style6 {
            width: 506px;
        }
                .auto-styleBTN {
            padding-bottom: 3px;
            text-align: center;
            width: 506px;
        }
    </style>
</asp:Content>


