<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditProjectTradePage" Title="Trade" Codebehind="EditProjectTrade.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Updating Trade" />

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
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Code:</td>
                    <td class="frmData"><asp:Label ID="lblCode" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Job Type:</td>
                    <td class="frmData"><asp:Label ID="lblJobType" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Requires Tender:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvTenderRequired" runat="server"></sos:BooleanViewer></td>
                </tr>
                <tr>
                    <td class="frmLabel">Description:</td>
                    <td class="frmData"><asp:Label ID="lblDescription" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Days from PCD:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valDaysFromPCD" runat="server" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtDaysFromPCD"></asp:CompareValidator>
                        <asp:TextBox ID="txtDaysFromPCD" runat="server" Width="150"></asp:TextBox>
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
                    <td class="frmLabel">Contracts Administrator (CA):</td>
                    <td>
                        <asp:TextBox ID="txtCA" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelCA" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtCAId" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Flag:</td>
                    <td>
                        <table cellpadding="0" cellspacing="0" style="border:solid 1px darkgray">
                            <tr>
                                <td><asp:RadioButton ID="rbRedFlag" runat="server" GroupName="rbgFlags" /></td>
                                <td><asp:Image runat="server" ImageUrl="~/images/RedFlag.png" /></td>
                                <td>&nbsp;&nbsp;</td>
                                <td><asp:RadioButton ID="rbGreenFlag" runat="server" GroupName="rbgFlags" /></td>
                                <td><asp:Image runat="server" ImageUrl="~/images/GreenFlag.png" /></td>
                                <td>&nbsp;&nbsp;</td>
                                <td><asp:RadioButton ID="rbNoFlag" runat="server" GroupName="rbgFlags" /></td>
                                <td class="frmText">None</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Header:</td>
                    <td class="frmText" colspan="4"><asp:TextBox ID="txtScopeHeader" TextMode="MultiLine" Rows="8" runat="server" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Footer:</td>
                    <td class="frmText" colspan="4"><asp:TextBox ID="txtScopeFooter" TextMode="MultiLine" Rows="8" runat="server" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quotes File:</td>
                    <td class="frmText" colspan="4"><sos:FileSelect ID="sfsQuotesFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Order Letting File:</td>
                    <td class="frmText" colspan="4"><sos:FileSelect ID="sfsPrelettingFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Invitation Date:</td>
                    <td class="frmText"><sos:DateReader ID="sdrInvitationDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Work Order Number:</td>
                    <td class="frmData"><asp:Label ID="lblWorkOrder" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quotes Due Date:</td>
                    <td class="frmText"><sos:DateReader ID="sdrQuotesDueDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Commencement Date:</td>
                    <td class="frmText"><sos:DateReader ID="sdrCommencementDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Comparison Due Date:</td>
                    <td class="frmText"><sos:DateReader ID="sdrComparisonDueDate" runat="server" Enabled="false"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Completion Date:</td>
                    <td class="frmText"><sos:DateReader ID="sdrCompletionDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contract Due Date:</td>
                    <td class="frmText"><sos:DateReader ID="sdrContractDueDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>

</asp:Content>

