<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditSubContractorPage" Title="Edit SubContractor" Codebehind="EditSubcontractor.aspx.cs" %>
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
                    <td class="frmReqLabel">Name:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Fax:</td>
                    <td><asp:TextBox ID="txtFax" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">ShortName:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valShortName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtShortName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtShortName" runat="server" Width="150"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Address:</td>
                    <td><asp:TextBox ID="txtStreet" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Business Unit:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valBusinessUnit" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlBusinessUnit"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td><asp:TextBox ID="txtLocality" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">ABN:</td>
                    <td><asp:TextBox ID="txtAbn" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">State:</td>
                    <td><asp:DropDownList ID="ddlState" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Account:</td>
                    <td><asp:TextBox ID="txtAccount" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td><asp:TextBox ID="txtPostalCode" runat="server" Width="150"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Phone:</td>
                    <td><asp:TextBox ID="txtPhone" runat="server" Width="150"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Website:</td>
                    <td><asp:TextBox ID="txtWebsite" runat="server"></asp:TextBox> </td>
                </tr>
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
