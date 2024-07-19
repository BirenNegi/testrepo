<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ReportDrawingsWithoutLinks.aspx.cs" Inherits="SOS.Web.ReportDrawingsWithoutLinks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <sos:TitleBar ID="TitleBar1" runat="server" Title="Drawings Without Links" />
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
            <td class="lstItemTitle" style="text-align:right">Project</td>
            <td>:</td>
            <td class="lstItemTitle"> <asp:DropDownList ID="dpdProjectList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpdProjectList_SelectedIndexChanged"></asp:DropDownList></td> 
        </tr>
        <tr class="lstItemTitle">
            <td class="">&nbsp;</td>
            <td>&nbsp;</td>
            <td class=""> &nbsp;</td> 
        </tr>
        <tr>
            <td colspan="3" >
                
                <rsweb:ReportViewer ID="RptDrawingWithoutLink" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="616px" Width="1087px">
                    <LocalReport ReportPath="Reports\DrawingsWithoutLinks.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Dr" Name="DrawingsWithoutLink" />
                        </DataSources>
                    </LocalReport>
                  
                </rsweb:ReportViewer>
                <br />
                <asp:SqlDataSource ID="SqlDataSource_Dr" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>" SelectCommand="select  T.Name as DrawingType,Number,D.Name,[Description] from [DrawingRevisions] A
inner join [Drawings] D on A.DrawingId= D.drawingId
inner join [DrawingTypes] T on D.DrawingTypeId=T.DrawingTypeId
 where A.Filepath is null

and A.DrawingRevisionId in( select top 1 B.DrawingRevisionId from [DrawingRevisions] B where  A.DrawingId=B.drawingId order by RevisionDate Desc  )

and D. ProjectId=@ProjectId
order by A.DrawingId  desc">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="dpdProjectList" DefaultValue="0" Name="ProjectId" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
               
            </td> 
        </tr>
</table>



</asp:Content>

