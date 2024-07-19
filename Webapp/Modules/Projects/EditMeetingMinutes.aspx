<%@ Page  Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditMeetingMinutes" Title="Meeting Minutes" Codebehind="EditMeetingMinutes.aspx.cs" %>
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
               <%-- <tr>
                    <td class="frmLabel">Meeting Number:</td>
                    <td class="frmData" colspan="4" ><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                    </tr> --%>
                  <tr>
                    <td class="frmLabel">Meeting Number:</td>
                    <td class="frmData" colspan="4" ><asp:TextBox ID="txtTypeNumber" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTypeNumber" ErrorMessage="Value must be a whole number" ForeColor="Red" Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                      </td>
                    </tr>
                  <tr>  <td class="frmLabel">Meeting Date:</td>
                    <td class="frmData" colspan="4"><sos:DateReader ID="sdrRaiseDate" runat="server"></sos:DateReader></td> </tr>
                </tr>
                <tr>
                    <td class="frmLabel">Meeting Time:</td>
                    <td class="frmData" colspan="4">
                        <asp:TextBox ID="txtTime" runat="server" TextMode="DateTime"></asp:TextBox></td>
                    <%--<td>&nbsp;</td>
                    <td class="frmLabel"></td>
                    <td class="frmData"><asp:Label ID="lblSigner" runat="server" Visible="false"></asp:Label></td>--%>
                    
                   
                </tr>
                <tr>
                    <td class="frmLabel"> Meeting Subject/Type:</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="Txtsubject" runat="server"></asp:TextBox>
                        <%--<asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>--%>

                    </td> </tr>

                    <tr>
                    <td class="frmLabel">Meeting Location:</td>
                    <td class="frmData">
                        <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>                    
                </tr>
                <tr>
                    <td class="frmLabel"> File:</td>
                    <td class="frmText" colspan="4"><sos:FileSelect ID="sfsReferenceFile" runat="server" /></td>
                </tr>
                <tr>
                    <td class="frmLabel">Attendees:</td>
                    <td colspan="4"><asp:TextBox ID="txtAttendees" runat="server" Width="480"></asp:TextBox></td>
                </tr>
               <%--  <tr>
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
                </tr>--%>
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


