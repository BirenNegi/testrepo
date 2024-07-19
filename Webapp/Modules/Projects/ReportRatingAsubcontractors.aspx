<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ReportRatingAsubcontractors.aspx.cs" Inherits="SOS.Web.ReportRatingAsubcontractors" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rs1web" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <sos:TitleBar ID="TitleBar" runat="server" Title="Rating A Subcontractors" />
       
    
    <table cellspacing="0" cellpadding="0">
    <tr>
        <td class="auto-style2">
            <table style="width: 249%">
                <%--<tr>
                    <td class="frmLabel">Trade Name:</td>
                    <td><asp:DropDownList ID="DropDownList2" runat="server"></asp:DropDownList></td>
                </tr>--%>
                <tr>
                    <td class="auto-style1"> Trade Code:</td>
                    <td><asp:DropDownList ID="DdlTradeCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlTradeCode_SelectedIndexChanged"></asp:DropDownList> &nbsp;<br /> </td>
                </tr>
                <tr>
                    <td class="auto-style1"> Trade Name:</td>
                    <td><asp:DropDownList ID="ddlTradeName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTradeName_SelectedIndexChanged"></asp:DropDownList> </td>
                </tr>
                <tr>
                    <td class="auto-style1">Business Unit</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList></td>                
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="auto-style3">
            <asp:LinkButton ID="cmdGenerateReport" CssClass="frmButton" runat="server" OnClick="cmdGenerateReport_Click">View Report</asp:LinkButton>
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
            <br />
            <br />
        </td>
    </tr>
</table>
    <rsweb:ReportViewer ID="RvRatingA" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="775px" Width="1079px">
    </rsweb:ReportViewer>
    <%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=VC-SQL-01;Initial Catalog=SOSTest;Integrated Security=True" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [View_RatingA_Subcontractors] where Code='01.04.01'"></asp:SqlDataSource>--%>
    <br /><br />
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            background-color: #DDDDDD;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 107px;
        }
        .auto-style2 {
            background-color: #FFFFFF;
            border: #333333 1px solid;
            width: 429px;
        }
        .auto-style3 {
            padding-top: 2px;
            text-align: center;
            width: 429px;
        }
    </style>
</asp:Content>


