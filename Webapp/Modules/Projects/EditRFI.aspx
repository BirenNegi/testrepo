<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditRFIPage" Title="RFI" Codebehind="EditRFI.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Updating RFI" />

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table cellpadding="2" cellspacing="1">
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date Raised:</td>
                    <td><sos:DateReader ID="sdrRaiseDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Created by:</td>
                    <td class="frmData"><asp:Label ID="lblSigner" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Answer by date:</td>
                    <td><sos:DateReader ID="sdrTargetAnswerDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Status:</td>
                    <td><asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date Answered:</td>
                    <td><sos:DateReader ID="sdrActualAnswerDate" runat="server"></sos:DateReader></td>                    
                </tr>
                <tr>
                    <td class="frmLabel">Reference File:</td>
                    <td class="frmText" colspan="4"><sos:FileSelect ID="sfsReferenceFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Subject:</td>
                    <td colspan="4"><asp:TextBox ID="txtSubject" runat="server" Width="480"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Response File:</td>
                    <td class="frmText" colspan="4"><sos:FileSelect ID="sfsClientResponseFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Response Summary:</td>
                    <td colspan="4"><asp:TextBox ID="txtClientResponseSummary" runat="server" TextMode="MultiLine" Rows="4" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Description:</td>
                    <td colspan="4"><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="16" Width="640"></asp:TextBox></td>
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

</asp:Content>

