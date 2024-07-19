<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditParticipationPage" Title="Trade Subcontractor" Codebehind="EditParticipation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Updating Trade Subcontractor" />

<asp:Panel ID="pnlProposal" runat="server">
<table cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <sos:CheckComparison ID="CheckComparison1" runat="Server" Visible="false"></sos:CheckComparison>
        </td>
    </tr>
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
                    <td class="frmData"><asp:Label ID="lblSubcontractor" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Invitation Date:</td>
                    <td><sos:DateReader ID="sdrInvitationDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Pulled out:</td>
                    <td class="frmData"><sos:BooleanReader ID="sbrPulledOut" runat="server" /></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Reminder Date</td>
                    <td><sos:DateReader ID="sdrReminderDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quote Amount:</td>
                    <td class="frmData">
                        <asp:CompareValidator ID="valQuoteAmount" runat="server" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtQuoteAmount"></asp:CompareValidator>
                        <asp:TextBox ID="txtQuoteAmount" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Quote Date:</td>
                    <td><sos:DateReader ID="sdrQuoteDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Rank:</td>
                    <td class="frmData">
                        <asp:CustomValidator ID="valRank" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Incomplete Comparison.<br />" OnServerValidate="valRank_ServerValidate"></asp:CustomValidator>
                        <asp:DropDownList ID="ddlRank" runat="server"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Due Date:</td>
                    <td><sos:DateReader ID="sdrDueDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contact:</td>
                    <td class="frmData"><asp:DropDownList ID="ddlContact" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <%-- # --%>
                    <td class="frmLabel">Safety Rating:</td>
                    <td class="frmData"><asp:DropDownList ID="ddlSfetyRatings" runat="server" Height="24px" Width="128px">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>High Risk</asp:ListItem>
                        <asp:ListItem>Medium Risk</asp:ListItem>
                        <asp:ListItem>Low Risk</asp:ListItem>
                        </asp:DropDownList>
                        <%-- # --%>
                    </td>
                </tr>
 
                <tr>
                    <td class="frmLabel"></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Payment Terms:</td>
                    <td class="frmData"><asp:DropDownList ID="ddlPaymentTerms" runat="server" Height="24px" Width="128px">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td class="frmLabel">Subcontractor&#39;s Comments:</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" Width="400"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">VC Internal Comments:</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="txtinternalComments" runat="server" TextMode="MultiLine" Rows="4" Width="400"></asp:TextBox></td>
                </tr>
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
<br />
</asp:Panel>
<br />

</asp:Content>

