<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ReportRatingA_Allsubcontractors.aspx.cs" Inherits="SOS.Web.ReportRatingA_Allsubcontractors" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rs1web" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <sos:TitleBar ID="TitleBar" runat="server" Title=" All Rating A Subcontractors" />
    <br /><br />


     <rsweb:ReportViewer ID="RvRatingA" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="800px" Width="1679px">
    </rsweb:ReportViewer>


</asp:Content>

