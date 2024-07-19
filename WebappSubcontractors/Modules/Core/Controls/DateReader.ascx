<%@ Control Language="C#" AutoEventWireup="true" Inherits="SOS.Web.DateReaderControl" Codebehind="DateReader.ascx.cs" %>
<table cellpadding="1" cellspacing="0">
    <tr>
        <td>
            <asp:CompareValidator ID="valCalendar" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Invalid date.<br />" ControlToValidate="txtCalendar" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
            <asp:TextBox ID="txtCalendar" runat="server" Width="150" ontextchanged="txtCalendar_TextChanged"></asp:TextBox>
        </td>
        <td><asp:Image ID="imgCalendar" runat="server" ImageUrl="~/Images/IconCalendar.gif" /></td>
    </tr>
</table>
<act:CalendarExtender ID="actCalendar" runat="server" TargetControlID="txtCalendar" PopupButtonID="imgCalendar" FirstDayOfWeek="Monday"></act:CalendarExtender>
