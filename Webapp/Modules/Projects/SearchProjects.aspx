<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SearchProjectsPage" Title="Projects" Codebehind="SearchProjects.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Projects" />

<asp:ObjectDataSource 
    ID="odsProjects" 
    runat="server"      
    TypeName="SOS.Core.ProjectsController" 
    OnObjectCreating="odsProjects_Selecting" 
    SelectMethod="SearchProjects" 
    SelectCountMethod="SearchProjectsCount"
    SortParameterName="orderBy"
    EnablePaging="true">
    <SelectParameters>
        <asp:ControlParameter Name="strSearch" ControlID="txtSearch" />
        <asp:ControlParameter Name="strBusinessUnit" ControlID="ddlBusinessUnit" />
        <asp:ControlParameter Name="strStatus" ControlID="ddlStatus" />
    </SelectParameters>
</asp:ObjectDataSource>

<table cellspacing="1" cellpadding="1">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" NavigateUrl="~/Modules/Projects/EditProject.aspx"></asp:HyperLink></td>
                    <td>&nbsp;&nbsp;</td>
                    </asp:PlaceHolder>
                    
                    <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="cmdSearch" runat="server" OnClick="cmdSearch_Click" Text="Search"></asp:Button></td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Business Unit:</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" AutoPostBack="true" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Status:</td>
                    <td><asp:DropDownList ID="ddlStatus" AutoPostBack="true" runat="server"></asp:DropDownList></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>    
            <asp:GridView 
                ID = "gvProjects"
                runat = "server"
                OnSorting="gvProjects_OnSorting"
                OnRowCreated="gvProjects_OnRowCreated"
                DataSourceID = "odsProjects"
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
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProject.aspx?ProjectId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="FullNumber" HeaderText="Number" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="BusinessUnitName" HeaderText="Business Unit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="StatusName" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Commenced" SortExpression="CommencementDate" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><%#SOS.UI.Utils.SetFormDate((System.DateTime?)Eval("CommencementDate"))%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Completion" SortExpression="CompletionDate" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><%#SOS.UI.Utils.SetFormDate((System.DateTime?)Eval("CompletionDate"))%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="PostalCode" HeaderText="Postal Code" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>

</asp:Content>
