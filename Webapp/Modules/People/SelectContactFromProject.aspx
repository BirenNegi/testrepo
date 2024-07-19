<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectContactFromProject.aspx.cs" Inherits="SOS.Web.SelectContactFromProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Select Contact From Project</title>
    <link href="../../Config/StyleSheet.css" rel="stylesheet" type="text/css" />
   
</head>
<body>
    <form id="form1" runat="server">
        <div>
                <sos:TitleBar ID="TitleBar1" runat="server" Title="Selecting Contact" />
                 <table>
                         <tr>
                             <td> 
                                
                                 <div style="height:265px; width:700px; overflow:scroll;padding-left:10px;">
                                 <asp:DropDownList ID="DpdSubies" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" >
                                        <asp:ListItem Value='1'>Awarded</asp:ListItem>
                                        <asp:ListItem Value='[Rank]  or dbo.TradeParticipations.[Rank] is null'>All</asp:ListItem>
                                 </asp:DropDownList>Subcontractors
                                  <br/><br/>
                                 <asp:GridView 
                    ID = "gvPeople"
                    runat = "server"
                    OnSorting="gvPeople_OnSorting"
                    OnRowCreated="gvPeople_OnRowCreated"
                    AllowPaging = "True"
                    AllowSorting = "True"
                    PagerStyle-CssClass = "lstPager"
                    CellPadding = "4"
                    CssClass = "lstList"
                    AutoGenerateColumns = "False"
                    RowStyle-CssClass = "lstItem"
                    DataKeyNames="PeopleId"

                    AlternatingRowStyle-CssClass = "lstAltItem" PageSize="10000" DataSourceID="SqlDataSource2" Width="100%">
                    <PagerSettings mode="NumericFirstLast" firstpagetext="First" lastpagetext="Last" pagebuttoncount="10" position="Bottom" />
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                    <Columns>
                        <%--<asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkSelect" runat="server" NavigateUrl='<%#SOS.Web.Utils.PopupSendPeople((System.Web.UI.Page)this.Page, (String)Eval("IdStr"), (String)Eval("Name"))%>' Text="Select"></asp:HyperLink>
                            </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle CssClass="lstLink"></ItemStyle>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderStyle-CssClass="lstHeader">
                            
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                        </asp:TemplateField>


                        <asp:BoundField DataField="Names" SortExpression="Names" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="true" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader" HorizontalAlign="Left"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False" HorizontalAlign="Justify" Width="2%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Company" SortExpression="Company" HeaderText="Company" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader" HorizontalAlign="Left"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Address" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False" Width="8%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Suburb" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="true" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PeopleId" HeaderText="PeopleId" Visible="False" />
                        <asp:BoundField DataField="SubContractorId" HeaderText="SubContractorId" Visible="False" />


                    </Columns>

<PagerStyle CssClass="lstPager"></PagerStyle>

<RowStyle CssClass="lstItem"></RowStyle>
                </asp:GridView>    




                </div>
                             </td>
                        </tr>
                        <tr>
                            <td style="padding-left:50px; text-align:center">
                <br />
                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<%--                <asp:Button runat="server" ID="Button2"  Text="Cancel" onclick="window.close();"/>--%>
                <input id="Button1" type="button" value="Cancel" onclick="window.close();"/>
                
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>"                  
                     SelectCommand="
                         SELECT DISTINCT 
                         dbo.People.FirstName+ ' '+dbo.People.LastName as Names, dbo.SubContractors.Name as Company,dbo.SubContractors.Street as Address,dbo.SubContractors.Locality as Suburb, dbo.SubContractors.State, --dbo.Trades.Name AS Expr1, dbo.TradeParticipations.Rank, 
                         dbo.Trades.ProjectId,People.PeopleId,SubContractors.SubContractorId
                         FROM            dbo.Trades INNER JOIN
                                                     dbo.TradeParticipations ON dbo.Trades.TradeId = dbo.TradeParticipations.TradeId INNER JOIN
                                                     dbo.SubContractors ON dbo.TradeParticipations.SubContractorId = dbo.SubContractors.SubContractorId INNER JOIN
                                                     dbo.People ON dbo.SubContractors.SubContractorId = dbo.People.SubContractorId
                         WHERE        (dbo.Trades.ProjectId =@ProjectId) AND isnull(TradeParticipations.[Rank],0) =
                                                                        Case  When @Rank='1' Then '1' 
													                    Else   isnull(dbo.TradeParticipations.[Rank],0) End 
                             Order by Company, Names Asc">  
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectId" QueryStringField="ProjectId" />
                        <%--<asp:QueryStringParameter DefaultValue="" Name="Orderby" QueryStringField="Orderby" />--%>
                        <asp:ControlParameter ControlID="DpdSubies" DefaultValue='1' Name="Rank" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
                        </tr>

                         <tr>
                             <td>
                
            </td>
                        </tr>
                 </table>
        </div>
    </form>
    
</body>
</html>
