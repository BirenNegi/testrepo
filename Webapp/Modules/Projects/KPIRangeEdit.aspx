<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="KPIRangeEdit.aspx.cs" Inherits="SOS.Web.KPIRangeEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  
         <sos:TitleBar ID="TitleBar" runat="server" />

    <p>
        <br /> <Table>
            
            
            <tr Class="lstHeader"style="background-color:#4ab2fc" ><td>
                KPI Color Range</td><td style="padding-left:20px;vertical-align:top;"   >
                    KPI Color Points Overall Performance</td></tr>
            
            
            <tr><td>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames = "Id" 
            DataSourceID="SqlDataSource1" AutoGenerateEditButton="True" Height="249px" Width="594px"  OnRowUpdating="GridView1_RowUpdating">
            
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" Visible="False"/>
                <asp:BoundField DataField="KPI" HeaderText="KPI" SortExpression="KPI" ReadOnly="True" Visible="false" >
                <ItemStyle Width="25%" />
                </asp:BoundField>
                <asp:BoundField DataField="KPIDisplay" HeaderText="KPIDisplay" SortExpression="KPIDisplay" ReadOnly="True" >

                <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

              <asp:TemplateField HeaderText="KPI" SortExpression="Target">
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtTarget" runat="server" Text='<%# Bind("Target") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Target") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Font-Bold="True" ForeColor="#0000CC" />
                </asp:TemplateField>

                 <asp:BoundField DataField="KPIMeasure" HeaderText="" ReadOnly="True" >  
                     <ItemStyle HorizontalAlign="Left"  ForeColor="#0000CC" Font-Bold="True"/>
                </asp:BoundField>

                <asp:TemplateField HeaderText="RED" SortExpression="MinRange">
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtMin" runat="server" Text='<%# Bind("MinRange") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("MinRange") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>



                <asp:TemplateField HeaderText="GREEN" SortExpression="MaxRange">
                    <EditItemTemplate>
                        <asp:TextBox ID="TxtMax" runat="server" Text='<%# Bind("MaxRange") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("MaxRange") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:BoundField DataField="KPIColor" HeaderText="KPIColor" SortExpression="KPIColor" ReadOnly="True" Visible="False" />
                <asp:BoundField DataField="Colorpoints" HeaderText="Colorpoints" SortExpression="Colorpoints" ReadOnly="True" Visible="False" />
            </Columns>
            <HeaderStyle CssClass="lstHeader" />
        <RowStyle CssClass="frmLabel" />
            <AlternatingRowStyle CssClass="frmInfo" ForeColor="Black" Font-Size="8pt"  HorizontalAlign="Right"/>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>" SelectCommand="SELECT [Id], [KPI], [KPIDisplay],[Target],[KPIMeasure], [MinRange], [MaxRange], [KPIColor], [Colorpoints] FROM [KPIColorRange] Where Active=1 ORDER BY DisplayId"></asp:SqlDataSource>
   
     </td><td style="padding-left:20px;vertical-align:top;"   >
         <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource2" Height="193px" AutoGenerateEditButton="True" OnRowUpdating="GridView2_RowUpdating" Width="459px">
             <Columns>
                 <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                 <asp:BoundField DataField="Color" HeaderText="Color" SortExpression="Color" ReadOnly="True"  />

                 <asp:TemplateField HeaderText="Min" SortExpression="minPoints">
                     <EditItemTemplate>
                         <asp:TextBox ID="TxtminPoints" runat="server" Text='<%# Bind("minPoints") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label2" runat="server" Text='<%# Bind("minPoints") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>


                 <asp:TemplateField HeaderText="Max" SortExpression="Points">
                     <EditItemTemplate>
                         <asp:TextBox ID="TxtPoints" runat="server" Text='<%# Bind("Points") %>'></asp:TextBox>
                     </EditItemTemplate>
                     <ItemTemplate>
                         <asp:Label ID="Label1" runat="server" Text='<%# Bind("Points") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
             </Columns> 
             <HeaderStyle CssClass="lstHeader" />
             <RowStyle CssClass="frmLabel" />
             <AlternatingRowStyle CssClass="frmInfo" ForeColor="Black" Font-Size="8pt" HorizontalAlign="Right" />
                </asp:GridView>

                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>" SelectCommand="SELECT [Id], [Color],[minPoints], [Points] FROM [KPIPoints] where ACTIVE=1"></asp:SqlDataSource>

          </td></tr>

               </Table>
  


</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    </asp:Content>


