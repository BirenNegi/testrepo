<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditEOTPage" Title="EOT" Codebehind="EditEOT.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Updating EOT" />

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
                    <td class="frmLabel">Start date:</td>
                    <td><sos:DateReader ID="sdrStartDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Type:</td>
                    <td>
                        <%-- San --%>
                        <asp:DropDownList ID="dpdType" runat="server" CssClass="frmText" Height="20px" Width="66px" OnSelectedIndexChanged="dpdType_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem>NOD</asp:ListItem>
                            <asp:ListItem>EOT</asp:ListItem>
                        </asp:DropDownList>
                        <%--#--%>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">End date:</td>
                    <td><sos:DateReader ID="sdrEndDate" runat="server"></sos:DateReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Days claimed:</td>
                    <td><asp:TextBox ID="txtDaysClaimed" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">First notice date:</td>
                    <td><sos:DateReader ID="sdrFirstNoticeDate" runat="server"></sos:DateReader></td>                    
                </tr>
                <tr>
                    <td class="frmLabel">Days approved:</td>
                    <td><asp:TextBox ID="txtDaysApproved" runat="server"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Written notice date:</td>
                    <td><sos:DateReader ID="sdrWriteDate" runat="server"></sos:DateReader></td>                    
                </tr>
                <tr>
                    <td class="frmLabel">Cost:</td>
                    <td>
                        <asp:TextBox ID="txtCostCode" runat="server"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Send date:</td>
                    <td><sos:DateReader ID="sdrSendDate" runat="server"></sos:DateReader></td>                    
                </tr>
                <tr>
                    <td class="frmLabel">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Approval date:
                    <td><sos:DateReader ID="sdrApprovalDate" runat="server"></sos:DateReader></td>                    
                </tr>
            </table>
            <table>

                 <tr>
                    <td class="frmLabel">&nbsp;Backup File</td>
                    <td class="frmText"><sos:FileSelect ID="sfsBackupFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Approval File:</td>
                    <td class="frmText"><sos:FileSelect ID="sfsClientApprovalFile" runat="server" /></td>
                </tr>
               
                <tr>
                    <td class="frmLabel">Cause of delay:</td>
                    <td><asp:TextBox ID="txtCause" runat="server" Width="480"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Nature of delay:</td>
                    <td><asp:TextBox ID="txtNature" runat="server" TextMode="MultiLine" Rows="8" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Period of delay:</td>
                    <td><asp:TextBox ID="txtPeriod" runat="server" TextMode="MultiLine" Rows="8" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Works affected:</td>
                    <td><asp:TextBox ID="txtWorks" runat="server" TextMode="MultiLine" Rows="4" Width="640"></asp:TextBox></td>
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

