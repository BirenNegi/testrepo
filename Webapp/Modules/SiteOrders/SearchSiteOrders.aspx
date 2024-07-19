<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SearchSiteOrders" Title="Orders" Codebehind="SearchSiteOrders.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="View Site Orders for current Project" />

<asp:ObjectDataSource
    ID="odsSiteOrders"
    runat="server" 
    TypeName="SOS.Core.SiteOrdersController"
    OnObjectCreating="odsSiteOrders_Selecting"
    SelectMethod="SearchSiteOrders"
    SelectCountMethod="SearchSiteOrdersCount"
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
        <td class="auto-style2">
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
                        <td>
                         <asp:LinkButton ID="LinkButton6" runat="server" CssClass="frmButton" ToolTip="Add Material Site Order" OnClick="cmdAddMatNS_Click">+MO No Sub</asp:LinkButton>
                         <asp:LinkButton ID="LinkButton5" runat="server" CssClass="frmButton" ToolTip="Add Instruction Site Order" OnClick="cmdAddInstNS_Click">+SI No Sub</asp:LinkButton>        
                         <asp:LinkButton ID="LinkButton4" runat="server" CssClass="frmButton" ToolTip="Add Hire Site Order" OnClick="cmdAddHireNS_Click">+EH No Sub</asp:LinkButton>
                        </td>
                        <td>
                        </td>
                         <td>
                          </td>
                   <td></td>
                   <td >
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="frmButton" ToolTip="Add Material Site Order" OnClick="cmdAddMat_Click">+MO</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="frmButton" ToolTip="Add Instruction Site Order" OnClick="cmdAddInst_Click">+SI</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="frmButton" ToolTip="Add Hire Site Order" OnClick="cmdAddHire_Click">+EH</asp:LinkButton>

                    </td>

<%--                   <td><asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" Visible="false" OnClick="cmdAddNew_Click" NavigateUrl="~/Modules/SiteOrders/RedFlag.aspx?OrderTyp=Mat"></asp:HyperLink> </td>--%>
                    </asp:PlaceHolder>
        <tr>
        <td class="auto-styleMSG" color="#0010FF">
            <table cellspacing="1" cellpadding="1" colspan="2">
                <tr>
                      <td class="frmText">When searching: Separate terms with commas</td>
               </tr>
            </table>
        </td>
    </tr>               </tr>
                  </table>
                   <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                   <td><asp:CheckBox ID="chkHistory" runat="server"></asp:CheckBox>Show History</td>
                    <td><asp:Button ID="cmdSearch" runat="server" OnClick="cmdSearch_Click"  Text="Search"></asp:Button></td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Type:</td>
                    <td><asp:DropDownList ID="ddlSiteOrderType" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSiteOrderType_SelectedIndexChanged"></asp:DropDownList></td>
  
  <td><asp:LinkButton ID="cmdApprovals" runat="server" Visible="true" ToolTip="My Approvals" OnClick="cmdApprovals_Click"><asp:Image runat="server" AlternateText="Approve" ImageUrl="~/Images/RedFlag.png" /></asp:LinkButton></td>
                        <td><asp:Label ID="lblApprovals" Visible="true" runat="server"></asp:Label></td>
                    <td><asp:LinkButton ID="cmdCancel" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdCancel_Click">Exit</asp:LinkButton> </td>

                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="auto-style2">
            <asp:GridView 
                ID = "gvSiteOrders"
                runat = "server"
                OnRowDataBound = "gvSiteOrders_RowDataBound"
                OnSorting="gvSiteOrders_OnSorting"
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
                            <asp:LinkButton ID="lnkViewOrder" runat="server" CausesValidation="false" CommandArgument='<%#String.Format("{0};{1};{2}", Eval("Id"), Eval("Typ"), Eval("SubContractorID")) %>' onclick="cmdViewOrder_Click">
                             <asp:Image ID="imgdelete" runat="server" ImageUrl="~/Images/IconView.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Typ" HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="TypDescription" HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                   <asp:BoundField DataField="Title" SortExpression="Title" HeaderText="Info" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="OrderDate" HeaderText="Date" HeaderStyle-CssClass="lstHeader" DataFormatString="{0:d}" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Status" Visible="false" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="SubContractorName" HeaderText="SubContractor" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="ForemanName" HeaderText="Foreman" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="StatusDescription" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="ID" HeaderText="Order No" HeaderStyle-CssClass="lstHeader" DataFormatString="{0:'D'000000}" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="SubTotal" HeaderText="Order Total" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                     <asp:TemplateField ItemStyle-VerticalAlign="Top">
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="true" CommandArgument='<%#String.Format("{0}", Eval("Id")) %>' onclick="cmdPrintOrder_Click">
                             <asp:Image ID="imgDocument" runat="server" ImageUrl="~/Images/IconDocument.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DocCount" HeaderText="Docs" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="GivenByName" HeaderText="Given By" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>

             </Columns>

            </asp:GridView>    
        </td>
    </tr>
</table>

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

