<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SearchContactsPage" Title="Contacts" Codebehind="SearchContacts.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Subcontractors Contacts" />

<asp:ObjectDataSource 
    ID="odsContacts" 
    runat="server" 
    TypeName="SOS.Core.PeopleController" 
    OnObjectCreating="odsContacts_Selecting" 
    SelectMethod="SearchPeople" 
    SelectCountMethod="SearchPeopleCount"
    SortParameterName="orderBy"
    EnablePaging="true">
    <SelectParameters>
        <asp:Parameter Name="peopleType" DefaultValue="CO" /> 
        <asp:ControlParameter Name="strSearch" ControlID="txtSearch" />
        <asp:ControlParameter Name="strBusinessUnit" ControlID="ddlBusinessUnit" />
        <asp:ControlParameter Name="inactive" ControlID="chkInactive" />
    </SelectParameters>
</asp:ObjectDataSource>

<table cellspacing="1" cellpadding="1">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" NavigateUrl="~/Modules/People/EditContact.aspx"></asp:HyperLink></td>
                    <td>&nbsp;&nbsp;</td>
                    </asp:PlaceHolder>
                    
                    <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="cmdSearch" runat="server" Text="Search" OnClick="cmdSearch_Click"></asp:Button></td>
                    
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Business Unit:</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged"></asp:DropDownList></td>

                    <td>&nbsp;&nbsp;</td>
                    <td class="frmTextSmall">Inactive</td>
                    <td><asp:CheckBox ID="chkInactive" AutoPostBack="true" OnCheckedChanged="chkInactive_OnCheckedChanged" runat="server" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView 
                ID = "gvPeople"
                runat = "server"
                OnSorting="gvPeople_OnSorting"
                OnRowCreated="gvPeople_OnRowCreated"
                DataSourceID = "odsContacts"
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
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/People/ViewContact.aspx?peopleId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" SortExpression="FullName" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="SubContractorName" SortExpression="CompanyName" HeaderText="Company" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="BusinessUnitName" HeaderText="Business Unit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkEmail" runat="server" NavigateUrl='<%#"mailto:"+Eval("Email")%>' Text='<%#Eval("Email")%>' CssClass="lstLink"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Login" SortExpression="UserLogin" Visible="false" HeaderText="Login" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Last Login" SortExpression="LastLogin" Visible="false" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblLoginDate" runat="server" Text='<%#SOS.UI.Utils.SetFormDateTime((System.DateTime?)Eval("LastLoginDate"))%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>

</asp:Content>

