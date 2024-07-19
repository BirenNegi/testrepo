<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditProjectPage" Title="Project" Codebehind="EditProject.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" />

<asp:Panel ID="pnlForm" runat="server">
<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td colspan="4" class="frmSubSubTitle">General Information</td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Name:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Number:</td>
                    <td><asp:TextBox ID="txtNumber" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Business Unit:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valBusinessUnit" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlBusinessUnit"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmReqLabel">Fax:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valFax" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtFax"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtFax" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Principal:</td>
                    <td><asp:TextBox ID="txtPrincipal" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Principal ABN:</td>
                    <td><asp:TextBox ID="txtPrincipalABN" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Address:</td>
                    <td><asp:TextBox ID="txtStreet" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td><asp:TextBox ID="txtLocality" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">State:</td>
                    <td><asp:DropDownList ID="ddlState" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td><asp:TextBox ID="txtPostalCode" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Status:</td>
                    <td><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Year (YY):</td>
                    <td><asp:TextBox ID="txtYear" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Commencement Date:</td>
                    <td><sos:DateReader ID="sdrCommencementDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Completion Date:</td>
                    <td><sos:DateReader ID="sdrCompletionDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Practical Completion Date:</td>
                    <td><sos:DateReader ID="sdrPracticalCompletionDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Contract Amount:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valContractAmount" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtContractAmount"></asp:CompareValidator>
                        <asp:TextBox ID="txtContractAmount" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">First Claim Due Date:</td>
                    <td><sos:DateReader ID="sdrFirstClaimDueDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Claim Frequency:</td>
                    <td><asp:DropDownList ID="ddlClaimFrequency" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Interest per annum:</td>
                    <td><asp:TextBox ID="txtInterest" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Payment Terms:</td>
                    <td><asp:DropDownList ID="ddlPaymentTerms" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Defects Liability (days):</td>
                    <td>
                        <asp:CompareValidator ID="valDefectsLiability" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtDefectsLiability"></asp:CompareValidator>
                        <asp:RangeValidator ID="valDefectsLiability1" runat="server" CssCalss="frmError" Type="Integer" MinimumValue="0" ControlToValidate="txtDefectsLiability"></asp:RangeValidator>
                        <asp:TextBox ID="txtDefectsLiability" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Liquidated Damages:</td>
                    <td><asp:TextBox ID="txtLiquidatedDamages" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Retention Amount:</td>
                    <td><asp:TextBox ID="txtRetention" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Site Allowances:</td>
                    <td><asp:TextBox ID="txtSiteAllowances" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Retention to DLP:</td>
                    <td><asp:TextBox ID="txtRetentionToDLP" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Retention to Certification:</td>
                    <td><asp:TextBox ID="txtRetentionToCertification" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Guarantee 1 Date:</td>
                    <td><sos:DateReader ID="sdrWaranty1Date" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Guarantee 1 Amount ($):</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valWaranty1Amount" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtWaranty1Amount"></asp:CompareValidator>
                        <asp:TextBox ID="txtWaranty1Amount" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Guarantee 2 Date:</td>
                    <td><sos:DateReader ID="sdrWaranty2Date" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Guarantee 2 Amount ($):</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valWaranty2Amount" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtWaranty2Amount"></asp:CompareValidator>
                        <asp:TextBox ID="txtWaranty2Amount" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Attachments Folder:</td>
                    <td colspan="4"><sos:FileSelect ID="sfsAttachmentsFolder" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Drawings Zoom URL:</td>
                    <td colspan="4"><asp:TextBox ID="txtDeepZoomUrl" runat="server" Width="480"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Maintenance Manual File:</td>
                    <td colspan="4"><sos:FileSelect ID="sfsMaintenanceManualFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Post Project Review File:</td>
                    <td colspan="4"><sos:FileSelect ID="sfsPostProjectReviewFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Description:</td>
                    <td colspan="4"><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" Width="480"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Special Clause:</td>
                    <td colspan="4"><asp:TextBox ID="txtSpecialClause" runat="server" TextMode="MultiLine" Rows="6" Width="480"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4" class="frmSubSubTitle">Project Roles</td>
                </tr>
                <tr>
                    <td class="frmLabel">Managing Director (MD):</td>
                    <td>
                        <asp:TextBox ID="txtGM" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelGM" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtGMId" type="hidden" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Construction Manager (CM):</td>
                    <td>
                        <asp:TextBox ID="txtCM" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelCM" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtCMId" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Project Manager (PM):</td>
                    <td>
                        <asp:TextBox ID="txtPM" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelPM" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtPMId" type="hidden" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Design Manager (DM):</td>
                    <td>
                        <asp:TextBox ID="txtDM" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelDM" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtDMId" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Design Coordinator (DC):</td>
                    <td>
                        <asp:TextBox ID="txtDC" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelDC" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtDCId" type="hidden" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Contracts Administrator (CA):</td>
                    <td>
                        <asp:TextBox ID="txtCA" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelCA" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtCAId" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Foreman:</td>
                    <td>
                        <asp:TextBox ID="txtForeman" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelForeman" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtForemanId" type="hidden" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Financial Controller (FC):</td>
                    <td>
                        <asp:TextBox ID="txtFC" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelFC" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtFCId" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Director Authorization (DA):</td>
                    <td>
                        <asp:TextBox ID="txtDA" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelDA" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtDAId" type="hidden" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Budget Administrator (BA):</td>
                    <td>
                        <asp:TextBox ID="txtBA" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelBA" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtBAId" type="hidden" runat="server" />
                    </td>
                </tr>
                <%-- San New role and processSteps for COM  and JCA--%>
                <tr>
                    <td class="frmLabel">Commercial Manager(COM): </td>
                    <td>
                        <asp:TextBox ID="txtCO" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelCO" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtCOId" type="hidden" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">2nd Contracts Administrator (CA2): </td>
                    <td>
                        <asp:TextBox ID="txtJC" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelJC" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtJCId" type="hidden" runat="server" />

                    </td>
                </tr>

                <%---------- San ------------%>
                <tr>
                    <td colspan="4" class="frmSubSubTitle">Second Principal</td>
                </tr>
                <tr>
                    <td class="frmLabel">Company:</td>
                    <td><asp:TextBox ID="txtPrincipal2" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">ABN:</td>
                    <td><asp:TextBox ID="txtPrincipal2ABN" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contact First Name:</td>
                    <td><asp:TextBox ID="txtSecondPrincipalFirstName" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Contact Last Name:</td>
                    <td><asp:TextBox ID="txtSecondPrincipalLastName" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Address:</td>
                    <td><asp:TextBox ID="txtSecondPrincipalStreet" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td><asp:TextBox ID="txtSecondPrincipalLocality" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">State:</td>
                    <td><asp:DropDownList ID="ddlSecondPrincipalState" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td><asp:TextBox ID="txtSecondPrincipalPostalCode" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Email:</td>
                    <td><asp:TextBox ID="txtSecondPrincipalEmail" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Phone:</td>
                    <td><asp:TextBox ID="txtSecondPrincipalPhone" runat="server" Width="150"></asp:TextBox></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmSubSubTitle">
                        <asp:Image ID="imgDistributionList" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" />
                        Documents Distribution List
                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlDistributionList" runat="server" CssClass="collapsePanel" Height="0">
            <table>
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
                    <td><asp:TextBox ID="txtClientContactFirstName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1FirstName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2FirstName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSIFirstName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSFirstName" Width="100" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Last Name:</td>
                    <td><asp:TextBox ID="txtClientContactLastName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1LastName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2LastName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSILastName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSLastName" Width="100" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Company Name:</td>
                    <td><asp:TextBox ID="txtClientContactCompanyName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1CompanyName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2CompanyName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSICompanyName" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSCompanyName" Width="100" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Address:</td>
                    <td><asp:TextBox ID="txtClientContactStreet" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1Street" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2Street" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSIStreet" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSStreet" runat="server" Width="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Suburb:</td>
                    <td><asp:TextBox ID="txtClientContactLocality" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1Locality" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2Locality" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSILocality" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSLocality" runat="server" Width="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">State:</td>
                    <td><asp:DropDownList ID="ddlClientContactState" runat="server"></asp:DropDownList></td>
                    <td><asp:DropDownList ID="ddlClientContact1State" runat="server"></asp:DropDownList></td>
                    <td><asp:DropDownList ID="ddlClientContact2State" runat="server"></asp:DropDownList></td>
                    <td><asp:DropDownList ID="ddlSIState" runat="server"></asp:DropDownList></td>
                    <td><asp:DropDownList ID="ddlQSState" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Potal Code:</td>
                    <td><asp:TextBox ID="txtClientContactPostalCode" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1PostalCode" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2PostalCode" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSIPostalCode" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSPostalCode" runat="server" Width="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Email:</td>
                    <td><asp:TextBox ID="txtClientContactEmail" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1Email" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2Email" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSIEmail" runat="server" Width="100"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSEmail" runat="server" Width="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Phone:</td>
                    <td><asp:TextBox ID="txtClientContactPhone" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1Phone" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2Phone" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSIPhone" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSPhone" Width="100" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Fax:</td>
                    <td><asp:TextBox ID="txtClientContactFax" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact1Fax" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtClientContact2Fax" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtSIFax" Width="100" runat="server"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtQSFax" Width="100" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Client Variations:</td>
                    <td><asp:CheckBox ID="chkClientContactCV" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact1CV" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact2CV" runat="server" /></td>
                    <td><asp:CheckBox ID="chkSICV" runat="server" /></td>
                    <td><asp:CheckBox ID="chkQSCV" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Separate Accounts:</td>
                    <td><asp:CheckBox ID="chkClientContactSA" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact1SA" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact2SA" runat="server" /></td>
                    <td><asp:CheckBox ID="chkSISA" runat="server" /></td>
                    <td><asp:CheckBox ID="chkQSSA" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Progress Claims:</td>
                    <td><asp:CheckBox ID="chkClientContactPC" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact1PC" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact2PC" runat="server" /></td>
                    <td><asp:CheckBox ID="chkSIPC" runat="server" /></td>
                    <td><asp:CheckBox ID="chkQSPC" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">RFIs:</td>
                    <td><asp:CheckBox ID="chkClientContactRFI" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact1RFI" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact2RFI" runat="server" /></td>
                    <td><asp:CheckBox ID="chkSIRFI" runat="server" /></td>
                    <td><asp:CheckBox ID="chkQSRFI" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">EOTs:</td>
                    <td><asp:CheckBox ID="chkClientContactEOT" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact1EOT" runat="server" /></td>
                    <td><asp:CheckBox ID="chkClientContact2EOT" runat="server" /></td>
                    <td><asp:CheckBox ID="chkSIEOT" runat="server" /></td>
                    <td><asp:CheckBox ID="chkQSEOT" runat="server" /></td>
                </tr>          
            </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="pnlCreateInfo" runat="server" Visible="false">
<script src="../../JavaScripts/ProcessStatus.js" type="text/javascript"></script>
<table>
    <tr>
        <td><asp:Image ID="imgProgress" runat="server" ImageUrl="~/Images/Progress.gif" /></td>
        <td>&nbsp;</td>
        <td class="frmTextBold"><div id="divRedirect" class="frmTextBold">Creating new project...</div></td>
    </tr>
</table>
<table>    
    <tr>
        <td colspan="3"><textarea id="txtProgress" rows="25" cols="50" readonly="readonly" class="frmProgress"></textarea></td>
    </tr>
</table>
</asp:Panel>

<act:CollapsiblePanelExtender 
    ID="cpe"
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlDistributionList"
    ExpandControlID="imgDistributionList"
    CollapseControlID="imgDistributionList"
    ImageControlID="imgDistributionList"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

</asp:Content>

