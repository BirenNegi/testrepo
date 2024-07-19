<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListReportsPage" Title="Global Reports" Codebehind="ListReports.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="sosTitleBar" runat="server" Title="Global Reports" />

<table cellspacing="1" cellpadding="2">
    <asp:Panel ID="pnlPendingTasks" runat="server" Visible="true">
    <tr>
        <td><asp:HyperLink ID="lnkPendingTasks" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportPendingTasks.aspx" Text="Pending Tasks"></asp:HyperLink></td>
    </tr>
    </asp:Panel>

    <asp:Panel ID="pnlCompletedTasks" runat="server" Visible="false">
    <tr>
        <td><asp:HyperLink ID="lnkCompletedTasks" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportCompletedTasks.aspx" Text="Completed Tasks"></asp:HyperLink></td>
    </tr>
    </asp:Panel>

    <asp:Panel ID="pnlActivitySummary" runat="server" Visible="false">
    <tr>
        <td><asp:HyperLink ID="lnkActiviySummary" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportActivitySummary.aspx" Text="Activity Summary by Projects"></asp:HyperLink></td>
    </tr>
       <!-- #-->
     <tr>
        <td><asp:HyperLink ID="lnkActiviySummaryStaff" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportActivitySummaryStaff.aspx" Text="Activity Summary by Staff"></asp:HyperLink></td>
    </tr>
         <!-- #-->

     <tr>
        <td><asp:HyperLink ID="lnkCVsSummaryDetail" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ShowCVsSummary.aspx" Text="Client Variation -Detail Summary "></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkBudgetSummaryDetail" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ShowBudgetTradeSummary.aspx" Text="Budget Trade- Detail Summary "></asp:HyperLink></td>
    </tr>


    </asp:Panel>

    <tr>
        <td><asp:HyperLink ID="lnkPurchasingSchedule" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportPurchaseSchedule.aspx" Text="Purchasing Schedule"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkSitePurchasingSchedule" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportSitePurchaseSchedule.aspx" Text="Site Purchasing Schedule"></asp:HyperLink></td>
    </tr>

    <asp:Panel ID="pnlReportWorkOrders" runat="server" Visible="false">
    <tr>
        <td><asp:HyperLink ID="lnkOrderNumber" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportWorkOrders.aspx" Text="Work Orders (Old)"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkWorkOrders" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportWorkOrdersNew.aspx" Text="Work Orders"></asp:HyperLink></td>
    </tr>

     <tr>
        <td><asp:HyperLink ID="LnkReporttSnapshot" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ShowProjectSnapShot.aspx" Text="Project Snapshot"></asp:HyperLink></td>        
    </tr>

      <tr>
        <td>
            <asp:HyperLink ID="LnkReportTurnoverVsTime" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportTurnoverVsTime.aspx" Text="Turnover Vs Time"></asp:HyperLink></td>        
    </tr>
     </asp:Panel>
    <asp:Panel ID="pnlReportKPI" runat="server" Visible="false">

       <tr> <td>
            <asp:HyperLink ID="LnkReportKPIAnalysis" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportKPIAnalysis.aspx" Text="KPI Analysis"></asp:HyperLink></td>        
        </tr>
      </asp:Panel>
    
    
    <tr>
        <td><asp:HyperLink ID="lnkBidsChart" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportBidsChart.aspx" Text="Bids Chart"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkRFIs" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportRFIs.aspx" Text="RFIs"></asp:HyperLink></td>    
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkEOTs" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportEOTs.aspx" Text="EOTs"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkCVs" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportCVsProject.aspx" Text="Client Variations"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkCVStatus" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportCVsStatus.aspx" Text="Client Variations Status"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkCVSummary" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportCVs.aspx" Text="Client Variations Summary"></asp:HyperLink></td>
    </tr>
    <%-- DS202401 --%>
    <tr>
        <td><asp:HyperLink ID="HyperLink1" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportTVsProject.aspx" Text="Tenant Variations"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkSAs" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportSAs.aspx" Text="Separate Accounts"></asp:HyperLink></td>
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkClaims" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportClaims.aspx" Text="Progress Claims"></asp:HyperLink></td>        
    </tr>
    <tr>
        <td><asp:HyperLink ID="lnkTrade" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportTrade.aspx" Text="Trade-scope of works"></asp:HyperLink></td>        
    </tr>

     <tr>
        <td><asp:HyperLink ID="lnkRateA" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportRatingAsubcontractors.aspx" Text="Rating A Subcontractors by Trade"></asp:HyperLink></td>        
    </tr>
     <tr>
        <td><asp:HyperLink ID="LnkAllRatingA" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportRatingA_Allsubcontractors.aspx" Text=" All Rating A Subcontractors"></asp:HyperLink></td>        
    </tr>
    <tr>
        <td><asp:HyperLink ID="LnkContractsSignedContractfile" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/Report_Contracts_MissingSignedContractFiles.aspx" Text=" Contracts without link to Signedcontract file"></asp:HyperLink></td>        
    </tr>
     <tr>
        <td><asp:HyperLink ID="LnkSubcontractorVariaton" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportSubcontractorVariation.aspx" Text="Subcontrator Variations"></asp:HyperLink></td>        
    </tr>


     <tr>
        <td><asp:HyperLink ID="LnkSubcontractorVariatonByType" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportSubcontractorVariationByType.aspx" Text="Subcontrator Variations By Type"></asp:HyperLink></td>        
    </tr>
     
      <tr>
        <td><asp:HyperLink ID="LnkSubcontractorVariatonVVDV" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportSubcontractorsVvDv.aspx" Text="Subcontrator Variations(V,DV)"></asp:HyperLink></td>        
    </tr>

      <tr>
        <td><asp:HyperLink ID="LnkDesignVariation" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportDesignVariation.aspx" Text="Report Design Variation"></asp:HyperLink></td>        
    </tr>
     <tr>
        <td>
            <asp:HyperLink ID="LnkSubcontractorsWithoutInsuranceLinks" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportSubcontractorsMissingInsurance.aspx" Text="Subcontrator Without Insurances"></asp:HyperLink></td>        
    </tr>

     

     <tr>
        <td><asp:HyperLink ID="LnkTradesummary" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ShowBudgetTradeSummary.aspx" Text="Budget Trade Summary"></asp:HyperLink></td>        
    </tr>
         
     <tr>
        <td><asp:HyperLink ID="LnkVariationSummary" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ShowCVsSummary.aspx" Text="Client Variations summary"></asp:HyperLink></td>        
    </tr>
      <%-- San --%>
     <tr>
        <td><asp:HyperLink ID="LnkProjectContacts" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportProjectContacts.aspx" Text="Project Contacts"></asp:HyperLink></td>        
    </tr>
     <tr>
        <td><asp:HyperLink ID="LnkDrawingsWithoutLink" runat="server" CssClass="frmLink" NavigateUrl="~/Modules/Projects/ReportDrawingsWithoutLinks.aspx" Text="Drawings Without Link"></asp:HyperLink></td>        
    </tr>
     <%-- San --%>
     <tr>
        <td>&nbsp;</td>        
    </tr>

</table>

</asp:Content>
