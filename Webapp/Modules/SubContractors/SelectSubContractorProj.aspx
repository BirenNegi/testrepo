<%@ Page Language="C#" AutoEventWireup="true" Inherits="SOS.Web.SelectSubContractorProjPage" Title="Subcontractors" Codebehind="SelectSubContractorProj.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select Subcontractor</title>
    <link href="../../Config/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>    

    <sos:TitleBar ID="TitleBar1" runat="server" Title="Selecting Subcontractor" />

    <asp:ObjectDataSource 
        ID="odsSubContractors" 
        runat="server" 
        TypeName="SOS.Core.SubContractorsController" 
        OnObjectCreating="odsSubContractors_Selecting" 
        SelectMethod="SearchSubContractorsProj"
        SelectCountMethod="SearchSubContractorsProjCount"
        SortParameterName="orderBy"
        EnablePaging="true">
        <SelectParameters>
            <asp:ControlParameter Name="strSearch" ControlID="txtSearch" />
            <asp:ControlParameter Name="strBusinessUnit" ControlID="ddlBusinessUnit" />
            <asp:ControlParameter Name="strProjectId" ControlID="strProjectId" />
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
                        <td>&nbsp;</td>
                        <%--<td><asp:HyperLink ID="lnkNoneSelected" runat="server" Text="None Selected" CssClass="frmLink"> </asp:HyperLink></td>--%>
                        <%--<td><asp:HyperLink ID="lnkNoneSelected" runat="server" Text="None Selected" NavigateUrl='<%#SOS.Web.Utils.PopupSendCompany((System.Web.UI.Page)this.Page, "0", "")%>'</asp:HyperLink></td>--%>
        <%--                <td><asp:HyperLink ID="lnkNoneSelected" runat="server" NavigateUrl='<%#SOS.Web.Utils.PopupSendCompany((System.Web.UI.Page)this.Page, (string)"0", (string)"None Selected")%>' Text="None Selected" CssClass="lstLink"></asp:HyperLink></td>
        --%>                <td>&nbsp;&nbsp;</td>
                        <td class="frmText"><a class="frmLink" href="#" onclick="window.close();">Cancel</a></td>
                        <td><asp:TextBox ID="strProjectId" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>            
                <asp:GridView 
                    ID = "GridViewSubContractors"
                    runat = "server"
                    DataSourceID = "odsSubContractors"
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
                                <asp:HyperLink ID="lnkSelect" runat="server" NavigateUrl='<%#SOS.Web.Utils.PopupSendCompany((System.Web.UI.Page)this.Page, (String)Eval("IdStr"), (String)Eval("Name"))%>' Text="Select" CssClass="lstLink"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        <asp:BoundField DataField="Locality" SortExpression="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
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
