<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewProjectPage" Title="Project" Codebehind="ViewProject.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Project" />

<table>
    <tr>
        <td><asp:Image ID="imgGeneralInfo" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">General Information</td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkBudgets" CssClass="frmLink" Text="Budget" runat="server"></asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkTradesBreakup" CssClass="frmLink" Text="Client Trades" runat="server"></asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkClientVariations" runat="server" CssClass="frmLink">Client Variations</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkSeparateAccounts" runat="server" CssClass="frmLink">Separate Accounts</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkClaims" runat="server" CssClass="frmLink">Claims</asp:HyperLink></td>        
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkRFIs" runat="server" CssClass="frmLink">RFIs</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkEOTs" runat="server" CssClass="frmLink">EOTs</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkDrawingsTransmittals" runat="server" CssClass="frmLink">Drawings/Transmittals</asp:HyperLink></td>
    </tr>
</table>

<asp:Panel ID="pnlGeneralInfo" runat="server" CssClass="collapsePanel" Height="0">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table cellpadding="1" cellspacing="0">
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton></td>
                    <td>&nbsp;</td>
                    <td><asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%" cellpadding="3">
                <tr>
                    <td colspan="4" class="frmSubSubTitle">General Information</td>
                </tr>
                <tr>
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Business Unit:</td>
                    <td class="frmData"><asp:Label ID="lblBusiniessUnit" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Fax:</td>
                    <td class="frmData"><asp:Label ID="lblFax" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Principal:</td>
                    <td class="frmData"><asp:Label ID="lblPrincipal" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Principal ABN:</td>
                    <td class="frmData"><asp:Label ID="lblPrincipalABN" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Address:</td>
                    <td class="frmData"><asp:Label ID="lblStreet" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td class="frmData"><asp:Label ID="lblLocality" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">State:</td>
                    <td class="frmData"><asp:Label ID="lblState" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td class="frmData"><asp:Label ID="lblPostalCode" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Status:</td>
                    <td class="frmData"><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Year:</td>
                    <td class="frmData"><asp:Label ID="lblYear" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Commencement Date:</td>
                    <td class="frmData"><asp:Label ID="lblCommencementDate" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Completion Date:</td>
                    <td class="frmData"><asp:Label ID="lblCompletionDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Practical Completion Date:</td>
                    <td class="frmData"><asp:Label ID="lblPracticalCompletionDate" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Contract Amount:</td>
                    <td class="frmData"><asp:Label ID="lblContractAmount" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Completion Date including Claimed EOTs</td>
                    <td class="frmData"><asp:Label ID="lblCompletionDateClaimedEOTs" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Completion Date including Approved EOTs</td>
                    <td class="frmData"><asp:Label ID="lblCompletionDateApprovedEOTs" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">First Claim Due Date:</td>
                    <td class="frmData"><asp:Label ID="lblFirstClaimDueDate" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Claim Frequency:</td>
                    <td class="frmData"><asp:Label ID="lblClaimFrequency" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Interest per annum:</td>
                    <td class="frmData"><asp:Label ID="lblInterest" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Payment Terms:</td>
                    <td class="frmData"><asp:Label ID="lblPaymentTerms" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Defects Liability (days):</td>
                    <td class="frmData"><asp:Label ID="lblDefectsLiability" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Liquidated Damages:</td>
                    <td class="frmData"><asp:Label ID="lblLiquidatedDamages" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Retention Amount:</td>
                    <td class="frmData"><asp:Label ID="lblRetention" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Site Allowance:</td>
                    <td class="frmData"><asp:Label ID="lblSiteAllowances" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Retention to LDP:</td>
                    <td class="frmData"><asp:Label ID="lblRetentionToDLP" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Retention to Certification:</td>
                    <td class="frmData"><asp:Label ID="lblRetentionToCertification" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Guarantee 1 Date:</td>
                    <td class="frmData"><asp:Label ID="lblWaranty1Date" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Guarantee 1 Amount ($):</td>
                    <td class="frmData"><asp:Label ID="lblWaranty1Amount" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Guarantee 2 Date:</td>
                    <td class="frmData"><asp:Label ID="lblWaranty2Date" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Guarantee 2 Amount ($):</td>
                    <td class="frmData"><asp:Label ID="lblWaranty2Amount" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Attachments Folder:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblAttachmentsFolder" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Drawings Zoom URL:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblDeepZoomUrl" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Maintenance Manual File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblMaintenanceManualFile" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Post Project Review File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblPostProjectReviewFile" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Description:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblDescription" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Special clause:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblSpecialClause" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="4" class="frmSubSubTitle">Project Roles</td>
                </tr>
                <tr>
                    <td class="frmLabel">Managing Director (MD):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkGM" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Construction Manager (CM):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkCM" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr>
                    <td class="frmLabel">Project Manager (PM):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkPM" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Design Manager (DM):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkDM" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr>
                    <td class="frmLabel">Design Coordinator (DC):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkDC" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Contracts Administrator (CA):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkCA" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr>
                    <td class="frmLabel">Foreman:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkForeman" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Financial Controller (FC):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkFC" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr>
                    <td class="frmLabel">Director Authorization (DA):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkDA" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Budget Administrator (BA):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkBA" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>

                <%-- San  New Role COM and JCA--%>
                <tr>
                    <td class="frmLabel">Commercial Manager(COM):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkCO" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">2nd&nbsp; Contracts Administrator (CA2):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkJC" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <%-- San --%>

                <tr>
                    <td colspan="4" class="frmSubSubTitle">Second Principal</td>
                </tr>
                <tr>
                    <td class="frmLabel">Second Principal:</td>
                    <td class="frmData"><asp:Label ID="lblPrincipal2" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Second Principal ABN:</td>
                    <td class="frmData"><asp:Label ID="lblPrincipal2ABN" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contact First Name:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalFirstName" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Contact Last Name:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalLastName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Address:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalStreet" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalLocality" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">State:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalState" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalPostalCode" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Email:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalEmail" runat="server"></asp:Label></td>
                    <td class="frmInfo">&nbsp;</td>
                    <td class="frmLabel">Phone:</td>
                    <td class="frmData"><asp:Label ID="lblSecondPrincipalPhone" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmSubSubTitle">Documents Distribution List</td>
                </tr>
            </table>
            <table cellpadding="3">
                <tr>
                    <td class="lstBlankItem">&nbsp;</td>
                    <td class="lstHeader">Main Contact</td>
                    <td class="lstHeader">Contact 1</td>
                    <td class="lstHeader">Contact 2</td>
                    <td class="lstHeader">Superintendent</td>
                    <td class="lstHeader">Qty. Surveyor</td>
                </tr>
                <tr>
                    <td class="frmLabel">First Name:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactFirstName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1FirstName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2FirstName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSIFirstName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSFirstName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Last Name:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactLastName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1LastName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2LastName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSILastName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSLastName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Company Name:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactCompanyName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1CompanyName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2CompanyName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSICompanyName" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSCompanyName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Address:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactStreet" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1Street" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2Street" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSIStreet" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSStreet" runat="server" Width="100"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Suburb:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactLocality" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1Locality" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2Locality" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSILocality" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSLocality" runat="server" Width="100"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">State:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactState" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1State" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2State" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSIState" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSState" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Potal Code:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactPostalCode" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1PostalCode" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2PostalCode" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSIPostalCode" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSPostalCode" runat="server" Width="100"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Email:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactEmail" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1Email" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2Email" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSIEmail" runat="server" Width="100"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSEmail" runat="server" Width="100"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Phone:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactPhone" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1Phone" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2Phone" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSIPhone" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSPhone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Fax:</td>
                    <td class="frmData"><asp:Label ID="lblClientContactFax" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact1Fax" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblClientContact2Fax" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblSIFax" runat="server"></asp:Label></td>
                    <td class="frmData"><asp:Label ID="lblQSFax" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Client Variations:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContactCV" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact1CV" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact2CV" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvSICV" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvQSCV" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Separate Accounts:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContactSA" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact1SA" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact2SA" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvSISA" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvQSSA" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Progress Claims:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContactPC" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact1PC" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact2PC" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvSIPC" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvQSPC" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">RFIs:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContactRFI" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact1RFI" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact2RFI" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvSIRFI" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvQSRFI" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">EOTs:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContactEOT" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact1EOT" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvClientContact2EOT" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvSIEOT" runat="server" /></td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvQSEOT" runat="server" /></td>
                </tr>          
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<br />

<asp:Panel ID="pnlStatusActive" runat="server">
<table>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="lstTitle">Contract Manager</td>                
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Job Type:</td>
                    <td><asp:DropDownList ID="ddlJobTypes" runat="server" OnSelectedIndexChanged="ddlJobTypes_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td><sos:FileLink ID="sflMaintenanceManual" runat="server" FileTitle="Maintenance Manual" /></td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td><sos:FileLink ID="sflPostProjectReview" runat="server" FileTitle="Post Project Review" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView
                ID = "gvTradesContract"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProjectTrade.aspx?TradeId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Code" HeaderText="Code" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="SelectedSubContractorName" HeaderText="Subcontractor" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#InfoStatus((SOS.Core.TradeInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#DateStatus((SOS.Core.TradeInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contract<br />Due Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((System.DateTime?)Eval("ContractDueDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Due<br />Days" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <span class="<%#StyleName((System.Int32?)Eval("ContractDueDays"))%>"><%#SOS.UI.Utils.SetFormInteger((System.Int32?)Eval("ContractDueDays"))%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="Comparison<br />Approved">
                        <ItemTemplate>
                            <sos:BooleanViewer ID="sbvIsStandar" runat="server" Checked='<%#Eval("ComparisonApproved")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="Contract">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#Eval("ContractApproved")%>' runat="server" NavigateUrl='<%#LinkContract((SOS.Core.TradeInfo)DataBinder.Eval(Container, "DataItem"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="# Variation<br />Orders" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" CssClass="frmLink" NavigateUrl='<%#String.Format("~/Modules/Contracts/ListSubContracts.aspx?ContractId={0}", Eval("ContractIdStr"))%>'><%#SOS.UI.Utils.SetFormIntegerNoZero((System.Int32?)Eval("NumSubContracts"))%></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>
<br />
</asp:Panel>

<table>
    <tr>
        <td id="tdProposal" runat="server">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="lstTitle">Purchasing Schedule</td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">Job Type:</td>
                    <td>&nbsp;</td>
                    <td><asp:DropDownList ID="ddlJobTypes1" runat="server" OnSelectedIndexChanged="ddlJobTypes1_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>

                    <asp:PlaceHolder ID="phAddTrade" runat="server" Visible="false">
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td><asp:DropDownList ID="ddlTradeTemplates" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;&nbsp</td>
                    <td><asp:Button ID="butAddTradeTemplate" runat="server" Text="Add" OnClick="butAddTradeTemplate_Click"></asp:Button></td>
                    </asp:PlaceHolder>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td id="tdProposal1" runat="server"><asp:PlaceHolder ID="phTender" runat="server"></asp:PlaceHolder></td>
    </tr>
</table>
<br />

<act:CollapsiblePanelExtender
    ID="cpe" 
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlGeneralInfo"
    ExpandControlID="imgGeneralInfo"
    CollapseControlID="imgGeneralInfo"
    ImageControlID="imgGeneralInfo"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

</asp:Content>
