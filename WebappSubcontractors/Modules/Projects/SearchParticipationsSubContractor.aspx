<%@ Page Language="C#" MasterPageFile="~/MasterPages/Subbie.master" AutoEventWireup="true" Inherits="SOS.Web.SearchParticipationsSubContractorPage" Title="Projects" Codebehind="SearchParticipationsSubContractor.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Projects" Visible="false" />

<asp:ObjectDataSource 
    ID="odsParticipations" 
    runat="server" 
    TypeName="SOS.Core.TradesController" 
    OnObjectCreating="odsParticipations_Selecting" 
    SelectMethod="SearchParticipations" 
    SelectCountMethod="SearchParticipationsCount"
    SortParameterName="orderBy"
    EnablePaging="true">
    <SelectParameters>
        <asp:Parameter Name="strSubContractorId" />
        <asp:ControlParameter Name="strSearch" ControlID="txtSearch" />
        <asp:ControlParameter Name="strOnlyActive" ControlID="ddlStatus" />
    </SelectParameters>
</asp:ObjectDataSource>

<table cellspacing="1" cellpadding="1">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="cmdSearch" runat="server" OnClick="cmdSearch_Click" Text="Search"></asp:Button></td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Show:</td>
                    <td><asp:DropDownList ID="ddlStatus" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>    
            <asp:GridView 
                ID = "gvParticipations"
                runat = "server"
                OnSorting="gvParticipations_OnSorting"
                OnRowCreated="gvParticipations_OnRowCreated"
                DataSourceID = "odsParticipations"
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
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewParticipationSubContractor.aspx?ParticipationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Project" SortExpression="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate><span class="<%#ClassActiveParticipation((System.Boolean)Eval("HasActiveParticipation"))%>"><%#SOS.UI.Utils.SetFormString((System.String)Eval("ProjectName"))%></span></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Number" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><span class="<%#ClassActiveParticipation((System.Boolean)Eval("HasActiveParticipation"))%>"><%#SOS.UI.Utils.SetFormString((System.String)Eval("ProjectNumber"))%></span></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Trade" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><span class="<%#ClassActiveParticipation((System.Boolean)Eval("HasActiveParticipation"))%>"><%#SOS.UI.Utils.SetFormString((System.String)Eval("TradeName"))%></span></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status" SortExpression="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><span class="<%#ClassActiveParticipation((System.Boolean)Eval("HasActiveParticipation"))%>"><%#SOS.UI.Utils.SetFormString((System.String)Eval("StatusNameSubcontractor"))%></span></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Due date" SortExpression="DueDate" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><span class="<%#ClassActiveParticipation((System.Boolean)Eval("HasActiveParticipation"))%>"><%#SOS.UI.Utils.SetFormDate((System.DateTime?)Eval("QuoteDueDate"))%></span></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Closing Time" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><span class="<%#ClassActiveParticipation((System.Boolean)Eval("HasActiveParticipation"))%>"><%#SOS.UI.Utils.SetFormTimeSpan((System.TimeSpan)Eval("ClosingTime"))%></span></ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Notes" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate><span class="lstTextGrayedOut"><%#MessageActiveParticipation((System.Boolean)Eval("HasActiveParticipation"))%></span></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>

</asp:Content>
