<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditSubContractPage" Title="Variation Order" Codebehind="EditSubContract.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" />

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
                    <td class="frmLabel">Subcontractor:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblSubcontractor" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Title:</td>
                    <td colspan="4"><asp:TextBox ID="txtTitle" runat="server" Width="450"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">GST (%):</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="txtGoodsServicesTax" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quotes File:</td>
                    <td class="frmText" colspan="4"><sos:FileSelect ID="sfsQuotesFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Site Instruction:</td>
                    <td><asp:TextBox ID="txtSiteInstruction" runat="server" Width="150"></asp:TextBox></td>
                    <td class="frmText">&nbsp;</td>
                    <td class="frmLabel">Dated:</td>
                    <td><sos:DateReader ID="sdrSiteInstructionDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">S/Cont.Ref:</td>
                    <td><asp:TextBox ID="txtSubContractorReference" runat="server" Width="150"></asp:TextBox></td>
                    <td class="frmText">&nbsp;</td>
                    <td class="frmLabel">Dated:</td>
                    <td><sos:DateReader ID="sdrSubContractorReferenceDate" runat="server"></sos:DateReader></td>
                </tr>
               <%-- DS20240321 --%>
                <tr>
                    <td class="frmLabel">Orig Contr.Dur:</td>
                     <td class="frmData"><asp:CheckBox ID="chkOrigContractDur" AutoPostBack="true" OnCheckedChanged="chkOrigContractDur_Changed" runat="server" Width="150"></asp:CheckBox></td> 
                    <td class="frmData">&nbsp;</td>
                    <td class="frmLabel">Start Date:</td>
                    <td><sos:DateReader ID="sdrStartDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel"></td>
                    <td class="frmData"></td>
                    <td class="frmData">&nbsp;</td>
                    <td class="frmLabel">Finish Date:</td>
                    <td><sos:DateReader ID="sdrFinishDate" runat="server"></sos:DateReader></td>
                </tr>
                <%-- DS20240321 --%>
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td colspan="4"><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" Width="400"></asp:TextBox></td>
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

