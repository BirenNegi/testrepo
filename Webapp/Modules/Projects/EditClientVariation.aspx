<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditClientVariationPage" Title="" Codebehind="EditClientVariation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Name:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" runat="server" Width="580"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">VC Quotes File:</td>
                    <td class="frmText"><sos:FileSelect ID="sfsQuotesFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Client Backup File:</td>
                    <td class="frmText"><sos:FileSelect ID="sfsBackupFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Client Approval File:</td>
                    <td class="frmText"><sos:FileSelect ID="sfsClientApprovalFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Show Cost Details:</td>
                    <td><sos:BooleanReader ID="sbrShowCostDetails" runat="server"></sos:BooleanReader></td>
                </tr>
                
                <asp:PlaceHolder ID="phUseSecondPrincipal" runat="server">
                <tr>
                   <td class="frmLabel">Use Second Principal:</td>
                   <td><sos:BooleanReader ID="sbrUseSecondPrincipal" runat="server"></sos:BooleanReader></td>
               </tr>
               </asp:PlaceHolder>
                
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" Width="400"></asp:TextBox></td>
                </tr>
            </table>
        </td>
    </tr>
    
    <asp:PlaceHolder ID="phInvoice" runat="server">
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmSubSubTitle">Invoice/Adjustment note</td>
                </tr>
            </table>

            <table>
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td><asp:TextBox ID="txtInvoiceNumber" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date:</td>
                    <td><sos:DateReader ID="sdrInvoiceDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Date Sent:</td>
                    <td class="frmData"><asp:Label ID="lblInvoiceSentDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date Due:</td>
                    <td><sos:DateReader ID="sdrInvoiceDueDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr style="display:none">
                    <td class="frmLabel">Date Paid:</td>
                    <td><sos:DateReader ID="sdrInvoicePaidDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
    
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>

</asp:Content>
