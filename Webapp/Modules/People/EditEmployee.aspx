<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditEmployeePage" Title="Edit Employee" Codebehind="EditEmployee.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

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
                    <td class="frmReqLabel">First&nbsp;Name:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valFirstName" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtFirstName" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Address:</td>
                    <td class="frmText"><asp:TextBox ID="txtStreet" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Last&nbsp;Name:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valLastName" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtLastName" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td class="frmText"><asp:TextBox ID="txtLocality" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Phone:</td>
                    <td class="frmText"><asp:TextBox ID="txtPhone" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">State:</td>
                    <td class="frmText"><asp:DropDownList ID="ddlState" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Mobile Phone:</td>
                    <td class="frmText"><asp:TextBox ID="txtMobile" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td class="frmText"><asp:TextBox ID="txtPostalCode" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Fax:</td>
                    <td class="frmText"><asp:TextBox ID="txtFax" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Position:</td>
                    <td class="frmText"><asp:TextBox ID="txtPosition" runat="server"></asp:TextBox> </td>
                </tr>
                <tr>
                    <td class="frmLabel">Email Address:</td>
                    <td class="frmText"><asp:TextBox ID="txtEmail" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Business Unit:</td>
                    <td class="frmText"><asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Inactive:</td>
                    <td class="frmText"><asp:CheckBox ID="chkInactive" runat="server" /></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
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
