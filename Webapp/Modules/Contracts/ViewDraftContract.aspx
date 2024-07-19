<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ViewDraftContract.aspx.cs" Inherits="SOS.Web.ViewDraftContract" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <sos:TitleBar ID="TitleBar1" runat="server" Title="Draft Contract" />

  <table cellpadding="0" cellspacing="0" width="750">
      <tr><td>
          <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="Print" ImageUrl="~/Images/IconPrint.gif" BorderStyle="Solid" BorderColor="#FFFFFF" BorderWidth="5" OnClick="ImageButton1_Click" />
          <%--<asp:HyperLink ID="lnkPrint" runat="server"  ToolTip="Print" ImageUrl="~/Images/IconPrint.gif" BorderStyle="Solid" BorderColor="#FFFFFF" BorderWidth="5" Visible="false"></asp:HyperLink>--%>

          </td>
       </td></tr>
      
    <tr> 
        <td class="frmForm"><asp:Literal ID="litContract" runat="server"></asp:Literal></td>
    </tr>
</table>


</asp:Content>

