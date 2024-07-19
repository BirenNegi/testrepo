<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SearchSubContractorsPage" Title="Subcontractors" Codebehind="SearchSubContractors.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Subcontractors" />

<asp:ObjectDataSource 
    ID="odsSubContractors" 
    runat="server" 
    TypeName="SOS.Core.SubContractorsController" 
    OnObjectCreating="odsSubContractors_Selecting" 
    SelectMethod="SearchSubContractors"
    SelectCountMethod="SearchSubContractorsCount"
    SortParameterName="orderBy"
    EnablePaging="true">
    <SelectParameters>
        <asp:ControlParameter Name="strSearch" ControlID="txtSearch" />
        <asp:ControlParameter Name="strBusinessUnit" ControlID="ddlBusinessUnit" />
    </SelectParameters>
</asp:ObjectDataSource>

<table cellspacing="1" cellpadding="1">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" NavigateUrl="~/Modules/SubContractors/EditSubContractor.aspx"></asp:HyperLink> </td>
                    <td>&nbsp;&nbsp;</td>
                    </asp:PlaceHolder>
                    <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="cmdSearch" runat="server" OnClick="cmdSearch_Click" Text="Search"></asp:Button></td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Business Unit:</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>            
            <asp:GridView 
                ID = "gvSubContractors"
                runat = "server"
                OnSorting="gvSubContractors_OnSorting"
                OnRowCreated="gvSubContractors_OnRowCreated"
                DataSourceID = "odsSubContractors"
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
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/SubContractors/ViewSubContractor.aspx?subContractorId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="BusinessUnitName" HeaderText="Business Unit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Locality" SortExpression="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="PostalCode" HeaderText="Postal Code" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Fax" HeaderText="Fax" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="ABN" HeaderText="Abn" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Website" HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkWebsite" runat="server" NavigateUrl='<%#SOS.UI.Utils.SetFormURL((String)Eval("Website"))%>' Text='<%#Eval("Website")%>' CssClass="lstLink" Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>

</asp:Content>

