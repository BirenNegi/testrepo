<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewContractTemplatePage" Title="Contract Template" Codebehind="ViewContractTemplate.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Contract Template" />

<table cellpadding="0" cellspacing="0" width="750">
    <tr>
        <td>
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
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
        </td>
    </tr>
    <tr>
        <td class="frmForm"><asp:Literal ID="litTemplate" runat="server"></asp:Literal></td>
    </tr>
</table>
<br />

</asp:Content>
