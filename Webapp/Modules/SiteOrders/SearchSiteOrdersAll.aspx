<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SearchSiteOrdersAll" Title="Orders" Codebehind="SearchSiteOrdersAll.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="View All Site Orders" />

<asp:ObjectDataSource
    ID="odsSiteOrders"
    runat="server" 
    TypeName="SOS.Core.SiteOrdersController"
    OnObjectCreating="odsSiteOrders_Selecting"
    SelectMethod="SearchSiteOrdersAll"  
    SelectCountMethod="SearchSiteOrdersAllCount"
    SortParameterName="orderBy"
    EnablePaging="true">
    <SelectParameters>
        <asp:ControlParameter Name="strSearch" ControlID="txtSearch" />
        <asp:ControlParameter Name="strSiteOrderType" ControlID="ddlSiteOrderType" />
        <asp:ControlParameter Name="chkHistory" ControlID="chkHistory" />
    </SelectParameters>
</asp:ObjectDataSource>

<table cellspacing="1" cellpadding="1">
       <tr>
        <td class="auto-styleMSG" color="#0010FF">
            <table cellspacing="1" cellpadding="1" colspan="2">
                <tr>
                      <td class="frmText">When searching: Separate terms with commas   </td><td><asp:Label ID="lblMessage" Visible="true" runat="server"></asp:Label></td>
               </tr>
            </table>
        </td>
    </tr>

    <tr>
        <td class="auto-style2">
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td><asp:CheckBox ID="chkHistory" runat="server"></asp:CheckBox>Show History</td>
                   <td><asp:Button ID="cmdSearch" runat="server" OnClick="cmdSearch_Click" Text="Search"></asp:Button></td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Type:</td>
                    <td><asp:DropDownList ID="ddlSiteOrderType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSiteOrderType_SelectedIndexChanged"></asp:DropDownList></td>
  
                    <td><asp:LinkButton ID="cmdApprovals" runat="server" Visible="true" ToolTip="My Approvals" OnClick="cmdApprovals_Click"><asp:Image runat="server" AlternateText="Approve" ImageUrl="~/Images/RedFlag.png" /></asp:LinkButton></td>
                    <td><asp:Label ID="lblApprovals" Visible="true" runat="server"></asp:Label></td>
                    <td><asp:LinkButton ID="cmdCancel" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdCancel_Click">Exit</asp:LinkButton> </td>
                    <td class="frmText"></td>
                 
               </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="auto-style2">
            <asp:GridView 
                ID = "gvSiteOrders"
                runat = "server"
                OnSorting="gvSiteOrders_OnSorting"
                OnRowDataBound = "gvSiteOrders_RowDataBound"
                OnRowCreated="gvSiteOrders_OnRowCreated"
                DataSourceID = "odsSiteOrders"
                AllowPaging = "True"
                AllowSorting = "True"
                PageSize = "25"
                PagerStyle-CssClass = "lstPager"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                AutoGenerateColumns = "False"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <PagerSettings mode="NumericFirstLast" firstpagetext="First" lastpagetext="Last" pagebuttoncount="10" position="Bottom" />
                <Columns>

                   <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkViewOrder" runat="server" CausesValidation="false" CommandArgument='<%#String.Format("{0};{1};{2}", Eval("Id"), Eval("Typ"), Eval("ProjectId"))%>' onclick="cmdViewOrder_Click">
                             <asp:Image ID="imgdelete" runat="server" ImageUrl="~/Images/IconView.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="ProjectName" HeaderText="Project" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Typ" HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                      <asp:BoundField DataField="TypDescription" HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                   <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Info" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="OrderDate" HeaderText="Date" HeaderStyle-CssClass="lstHeader" DataFormatString="{0:d}" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="ProjectId" HeaderText="ProjectId" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="SubContractorName" HeaderText="SubContractor" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="ForemanName" HeaderText="Foreman" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="StatusDescription" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="ID" HeaderText="Order No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" DataFormatString="{0:'D'000000}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="SubTotal" HeaderText="Order Total" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                  <asp:TemplateField ItemStyle-VerticalAlign="Top">
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="true" ToolTip="Download Order + ALL documents for this order" CommandArgument='<%#String.Format("{0}", Eval("Id")) %>' onclick="cmdPrintOrder_Click">
                             <asp:Image ID="imgDocument" runat="server" ImageUrl="~/Images/IconDocument.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="DocCount" HeaderText="Docs" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                   <asp:BoundField DataField="BusinessUnitName" HeaderText="BU" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="GivenByName" HeaderText="Given By" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
               </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>
<%--    Eval("ProjectId")--%>
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style2 {
            width: 602px;
        }
        .auto-styleMSG {
            color: #0010FF;
        }
    </style>
</asp:Content>

