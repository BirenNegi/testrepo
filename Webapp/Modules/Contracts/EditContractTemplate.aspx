<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditContractTemplatePage" Title="Contract Template" ValidateRequest="false" Codebehind="EditContractTemplate.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" />

<table cellspacing="0" cellpadding="0" width="100%">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Business Unit:</td>
                    <td class="frmText"><asp:Label ID="lblBusninessUnit" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Job Type:</td>
                    <td class="frmText"><asp:Label ID="lblJobType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contract Type:</td>
                    <td class="frmText"><asp:Label ID="lblContractType" runat="server"></asp:Label></td>
                </tr>
            </table>
            <asp:CustomValidator ID="valTemplate" runat="server" CssClass="frmError" Display="Dynamic" OnServerValidate="valTemplate_Validate"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="frmForm"><HTMLEditor:Editor ID="htmlEditor" runat="server" Width="100%" Height="480px" AutoFocus="false" ActiveMode="Preview" /></td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
</table>

</asp:Content>

