<%@ Page Title="Contract Approval Limit" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="EditContractApprovalLimit.aspx.cs" Inherits="SOS.Web.EditContractApprovalLimit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <sos:TitleBar ID="TitleBar" runat="server" />


    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" DataSourceID="SqlDataSource1" BorderColor="#E6E6E6" Height="152px" Width="834px" OnRowUpdating="GridView1_RowUpdating" OnRowCreated="GridView1_RowCreated">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" >
            <HeaderStyle HorizontalAlign="Right" />
            <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Min" SortExpression="min" HeaderStyle-CssClass="lstHeader">
                <EditItemTemplate>
                    <asp:TextBox ID="TxtMin" runat="server" Text='<%# Bind("min") %>' TextMode="Number"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter valid number" ControlToValidate="TxtMin" ForeColor="Red"></asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("min") %>'></asp:Label>
                </ItemTemplate>

<HeaderStyle CssClass="lstHeader" HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Max" SortExpression="max" HeaderStyle-CssClass="lstHeader">
                <EditItemTemplate>
                    <asp:TextBox ID="TxtMax" runat="server" Text='<%# Bind("max") %>' TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtMax" ErrorMessage="Please enter valid number" ForeColor="Red"></asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("max") %>'></asp:Label>
                </ItemTemplate>

<HeaderStyle CssClass="lstHeader" HorizontalAlign="Right"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:BoundField DataField="WinOver5" HeaderText="Win>=5%" SortExpression="WinOver5" ReadOnly="True" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="Winlessthan5" HeaderText="Win<5%" SortExpression="Winlessthan5" ReadOnly="True" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="Losslessthan5" HeaderText="Loss<=5%" SortExpression="Losslessthan5" ReadOnly="True" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="Lossbtw5to50" HeaderText="Loss >5-50%" SortExpression="Lossbtw5to50" ReadOnly="True" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="LossOver50" HeaderText="Loss>50%" SortExpression="LossOver50" ReadOnly="True" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
        </Columns>
        <HeaderStyle CssClass="lstHeader" />
        <RowStyle CssClass="frmLabel" />
    </asp:GridView>



    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=VC-SQL-02;Initial Catalog=SOSTest;User ID=SOSApp;Password=V@ugh@ns" ProviderName="System.Data.SqlClient" SelectCommand="SELECT [Id], [min], [max], [WinOver5], [Winlessthan5], [Losslessthan5], [Lossbtw5to50], [LossOver50] FROM [ContractApprovalLimit]"></asp:SqlDataSource> DS20230918--%>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="Data Source=VC-SQL-02;Initial Catalog=SOS;User ID=SOSApp;Password=V@ugh@ns" ProviderName="System.Data.SqlClient" SelectCommand="SELECT [Id], [min], [max], [WinOver5], [Winlessthan5], [Losslessthan5], [Lossbtw5to50], [LossOver50] FROM [ContractApprovalLimit]"></asp:SqlDataSource>




</asp:Content>

