<%@ Page Language="C#" MasterPageFile="~/MasterPages/Subbie.master" AutoEventWireup="true" Inherits="SOS.Web.SearchContactsPage" Title="Contacts" Codebehind="SearchContacts.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Subcontractors Contacts/Employees" />

<%--<asp:ObjectDataSource 
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
</asp:ObjectDataSource>--%>

<table cellspacing="1" cellpadding="1">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
                    <td><asp:HyperLink ID="lnkAddNew" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" NavigateUrl="~/Modules/People/EditContact.aspx"></asp:HyperLink></td>
                    <td>&nbsp;&nbsp;</td>
                    </asp:PlaceHolder>
                   <%-- 
                    <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="cmdSearch" runat="server" Text="Search" OnClick="cmdSearch_Click"></asp:Button></td>
                    
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Business Unit:</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged"></asp:DropDownList></td>
                    --%>
                    <td class="frmLink"><asp:HyperLink ID="lnkAddNew1" runat="server" ToolTip="Add New" NavigateUrl="~/Modules/People/EditContact.aspx">  Add Contacts/Employees</asp:HyperLink></td>
                    <td class="frmTextSmall"></td><%--Inactive--%>
                    <td><asp:CheckBox ID="chkInactive" AutoPostBack="true" OnCheckedChanged="chkInactive_OnCheckedChanged" runat="server" Visible="False" /></td>
                </tr>
               
            </table>
        </td>
    </tr>
    <tr>
        <td>
         <span class="frmTitle"> Contacts </span> 
            <asp:GridView 
                ID = "gvPeople"
                runat = "server"
                OnSorting="gvPeople_OnSorting"
                OnRowCreated="gvPeople_OnRowCreated"
               
                AllowPaging = "True"
                AllowSorting = "True"
                PageSize = "25"
                PagerStyle-CssClass = "lstPager"
                CellPadding = "4"
                CssClass = "lstList"
                AutoGenerateColumns = "False"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <PagerSettings mode="NumericFirstLast" firstpagetext="First" lastpagetext="Last" pagebuttoncount="10" position="Bottom" />
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/People/ViewContact.aspx?peopleId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" SortExpression="FullName" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CompanyName" SortExpression="CompanyName" HeaderText="Company" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" Visible="False">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="BusinessUnitName" HeaderText="Business Unit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" Visible="False">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkEmail" runat="server" NavigateUrl='<%#"mailto:"+Eval("Email")%>' Text='<%#Eval("Email")%>' CssClass="lstLink"></asp:HyperLink>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" CssClass="lstLink"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Login" SortExpression="UserLogin" Visible="false" HeaderText="Login" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Last Login" SortExpression="LastLogin" Visible="false" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblLoginDate" runat="server" Text='<%#SOS.UI.Utils.SetFormDateTime((System.DateTime?)Eval("LastLoginDate"))%>'></asp:Label>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                </Columns>

<PagerStyle CssClass="lstPager"></PagerStyle>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>    
        </td>
    </tr>
         <br />
      <%-- Employee --%>

   <tr>
        <td>
            &nbsp;</td>
    </tr>


   <tr>
        <td>
         <span class="frmTitle"> Employees </span> 
            <asp:GridView 
                ID = "gvEMployee"
                runat = "server"
                OnSorting="gvEMployee_OnSorting"
                OnRowCreated="gvEMployee_OnRowCreated"
               
                AllowPaging = "True"
                AllowSorting = "True"
                PageSize = "25"
                PagerStyle-CssClass = "lstPager"
                CellPadding = "4"
                CssClass = "lstList"
                AutoGenerateColumns = "False"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <PagerSettings mode="NumericFirstLast" firstpagetext="First" lastpagetext="Last" pagebuttoncount="10" position="Bottom" />
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/People/ViewContact.aspx?peopleId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" SortExpression="FullName" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CompanyName" SortExpression="CompanyName" HeaderText="Company" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" Visible="False">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="BusinessUnitName" HeaderText="Business Unit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" Visible="False">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkEmail" runat="server" NavigateUrl='<%#"mailto:"+Eval("Email")%>' Text='<%#Eval("Email")%>' CssClass="lstLink"></asp:HyperLink>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" CssClass="lstLink"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Login" SortExpression="UserLogin" Visible="false" HeaderText="Login" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Last Login" SortExpression="LastLogin" Visible="false" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblLoginDate" runat="server" Text='<%#SOS.UI.Utils.SetFormDateTime((System.DateTime?)Eval("LastLoginDate"))%>'></asp:Label>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                </Columns>

<PagerStyle CssClass="lstPager"></PagerStyle>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>    
        </td>
    </tr>


</table>

</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            height: 3px;
        }
    </style>
</asp:Content>


