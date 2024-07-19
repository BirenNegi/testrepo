<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ReportProjectContacts.aspx.cs" Inherits="SOS.Web.ReportProjectContacts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    <sos:TitleBar ID="TitleBar1" runat="server" Title="Project Contacts" />
      <table class="frmForm">
        <tr>
            <td class="lstItemTitle" colspan="3">&nbsp;<br />
                &nbsp;</td>
             <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="lstItemTitle" colspan="3">&nbsp;<br />
                 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            </td>
             
        </tr>
        <tr>
            <td class="lstItemTitle" style="text-align:right;width:80px">Project</td>
            <td class="lstItemTitle">:</td>
            <td class="lstItemTitle"> <asp:DropDownList ID="dpdProjectList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpdProjectList_SelectedIndexChanged"></asp:DropDownList></td> 
        </tr>
        <tr class="lstItemTitle">
            <td >&nbsp;</td>
            <td>&nbsp;</td>
            <td class=""> &nbsp;</td> 
        </tr>
        <tr>
            <td colspan="3" ><br />
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1158px" Height="765px">
                    <LocalReport ReportPath="Reports\ProjectContacts.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Trade" Name="TradeContacts" />
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Client" Name="ClientContacts" />
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Inhouse" Name="InhouseContacts" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:SqlDataSource ID="SqlDataSource_Inhouse" runat="server" ConnectionString="Data Source=VC-SQL-02;Initial Catalog=SOS;User ID=SOSApp;Password=V@ugh@ns;Pooling=False" ProviderName="System.Data.SqlClient" SelectCommand="SELECT People.FirstName, People.LastName, People.Phone, People.Mobile, People.Email, People.EmployeePosition, People.Street, People.Locality, People.State, People.PostalCode, Projects.MDPeopleId, Projects.CAPeopleId, Projects.PMPeopleId, Projects.CMPeopleId, Projects.ForemanPeopleId, Projects.DMPeopleId, Projects.DCPeopleId, Projects.COPeopleId, Projects.JCPeopleId FROM People INNER JOIN Projects ON People.PeopleId = Projects.DAPeopleId OR People.PeopleId = Projects.BAPeopleId OR People.PeopleId = Projects.MDPeopleId OR People.PeopleId = Projects.CAPeopleId OR People.PeopleId = Projects.PMPeopleId OR People.PeopleId = Projects.CMPeopleId OR People.PeopleId = Projects.ForemanPeopleId OR People.PeopleId = Projects.DMPeopleId OR People.PeopleId = Projects.DCPeopleId OR People.PeopleId = Projects.FCPeopleId WHERE (Projects.ProjectId = @ProjectId)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="dpdProjectList" DefaultValue="0" Name="ProjectId" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource_Client" runat="server" ConnectionString="Data Source=VC-SQL-02;Initial Catalog=SOS;User ID=SOSApp;Password=V@ugh@ns;Pooling=False" ProviderName="System.Data.SqlClient" SelectCommand="SELECT        dbo.People.FirstName, dbo.People.LastName, dbo.People.Phone, dbo.People.Mobile, dbo.People.Email, dbo.People.CompanyName, dbo.People.EmployeePosition, 
                         dbo.People.Street, dbo.People.Locality, dbo.People.State, dbo.People.PostalCode
FROM            dbo.ClientAccess INNER JOIN
                         dbo.People ON dbo.ClientAccess.PeopleId = dbo.People.PeopleId

where ProjectId=@ProjectId">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="dpdProjectList" Name="ProjectId" PropertyName="SelectedValue" DefaultValue="0" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource_Trade" runat="server" ConnectionString="Data Source=VC-SQL-02;Initial Catalog=SOS;User ID=SOSApp;Password=V@ugh@ns;Pooling=False" ProviderName="System.Data.SqlClient" SelectCommand="SELECT        dbo.Trades.Code, dbo.Trades.Name, dbo.SubContractors.ShortName, dbo.People.FirstName, dbo.People.LastName, dbo.People.Phone, dbo.People.Mobile, 
                         dbo.People.Email, dbo.People.Street, dbo.People.Locality, dbo.People.State, dbo.People.PostalCode , Projects.Name AS ProjectName, Projects.Number, Projects.Year
FROM            dbo.Trades INNER JOIN
                         dbo.TradeParticipations ON dbo.Trades.TradeId = dbo.TradeParticipations.TradeId INNER JOIN
                         dbo.SubContractors ON dbo.TradeParticipations.SubContractorId = dbo.SubContractors.SubContractorId INNER JOIN
                         dbo.People ON  dbo.SubContractors.SubContractorId = dbo.People.SubContractorId and  dbo.TradeParticipations.ContactPeopleId=dbo.People.PeopleId INNER JOIN
                         Projects ON Trades.ProjectId = Projects.ProjectId

WHERE Trades.ProjectId=@ProjectId and TradeParticipations.Rank=1 
ORDER BY Trades.Code 
">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="dpdProjectList" Name="ProjectId" PropertyName="SelectedValue" DefaultValue="0" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td> 
        </tr>
</table>
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            background-color: #EEEEEE;
            font-family: Verdana, Arial;
            font-weight: bold;
            font-size: 8pt;
            width: 170px;
        }
        .auto-style2 {
            width: 170px;
        }
    </style>
    </asp:Content>


