<%@ Page Title="Trade Report" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ReportTrade.aspx.cs" Inherits="SOS.Web.ReportTrade" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rs1web" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Trades with Item Cetegories,Items and Scope of work" />
       
    
    <table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <%--<tr>
                    <td class="frmLabel">Trade Name:</td>
                    <td><asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList></td>
                </tr>--%>
                <tr>
                    <td class="frmLabel"> Trade Code:</td>
                    <td><asp:DropDownList ID="DdlTradeCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlTradeCode_SelectedIndexChanged"></asp:DropDownList> <br /> </td>
                </tr>
                <tr>
                    <td class="frmLabel">Trade Name:</td>
                    <td><asp:DropDownList ID="ddlTradeName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTradeName_SelectedIndexChanged"></asp:DropDownList></td>                
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:LinkButton ID="cmdGenerateReport" CssClass="frmButton" runat="server" OnClick="cmdGenerateReport_Click">View Report</asp:LinkButton>
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
            <br />
            <br />
        </td>
    </tr>
</table>
    
    
    
     
    <rs1web:ReportViewer ID="RvTrade" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="830px" Height="820px">
        <%--<LocalReport ReportPath="Reports\Trades.rdlc">
        <DataSources>
            <rs1web:ReportDataSource DataSourceId="SqlDataSource1" Name="TradeDataSet" />
        </DataSources>
        </LocalReport>--%>
</rs1web:ReportViewer>
    <br />
    <br />

    <br />

</asp:Content>
