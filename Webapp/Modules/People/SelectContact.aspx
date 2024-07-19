<%@ Page Language="C#" AutoEventWireup="true" Inherits="SOS.Web.SelectContactPage" Title="Contact" Codebehind="SelectContact.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select Contact</title>
    <link href="../../Config/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>    

    <sos:TitleBar ID="TitleBar1" runat="server" Title="Selecting Contact" />

    <asp:ObjectDataSource 
        ID="odsPeople" 
        runat="server" 
        TypeName="SOS.Core.PeopleController" 
        OnObjectCreating="odsPeople_Selecting" 
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
                        <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                        <td>&nbsp;</td>
                        <td><asp:Button ID="cmdSearch" runat="server" Text="Search" OnClick="cmdSearch_Click"></asp:Button></td>
                        <td>&nbsp;&nbsp;</td>
                        <td class="frmText">Business Unit:</td>
                        <td><asp:DropDownList ID="ddlBusinessUnit" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged"></asp:DropDownList></td>
                        <td>&nbsp;&nbsp;</td>
                        <td class="frmTextSmall">Inactive</td>
                        <td><asp:CheckBox ID="chkInactive" AutoPostBack="true" OnCheckedChanged="chkInactive_OnCheckedChanged" runat="server" /></td>
                        <td>&nbsp;&nbsp;</td>
                        <td class="frmText"><asp:HyperLink ID="lnkNoneSelected" runat="server" Text="None Selected" CssClass="frmLink"></asp:HyperLink></td>
                        <td>&nbsp;&nbsp;</td>
                        <td class="frmText"><a class="frmLink" href="#" onclick="window.close();">Cancel</a></td>
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
                    DataSourceID = "odsPeople"
                    AllowPaging = "True"
                    AllowSorting = "True"
                    PageSize = "10"
                    PagerStyle-CssClass = "lstPager"
                    CellPadding = "4"
                    CellSpacing = "0"
                    CssClass = "lstList"
                    AutoGenerateColumns = "False"
                    RowStyle-CssClass = "lstItem"
                    AlternatingRowStyle-CssClass = "lstAltItem">
                    <PagerSettings mode="NumericFirstLast" firstpagetext="First" lastpagetext="Last" pagebuttoncount="10" position="Bottom" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkSelect" runat="server" NavigateUrl='<%#SOS.Web.Utils.PopupSendPeople((System.Web.UI.Page)this.Page, (String)Eval("IdStr"), (String)Eval("Name"))%>' Text="Select"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" SortExpression="FullName" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        <asp:BoundField DataField="SubContractorName" SortExpression="CompanyName" HeaderText="Company" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        <asp:BoundField DataField="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    </Columns>
                </asp:GridView>    
            </td>
        </tr>
    </table>

    </div>
    </form>
</body>
</html>
