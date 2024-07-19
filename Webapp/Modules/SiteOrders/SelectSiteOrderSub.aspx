<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SelectSiteOrderSub" Title="Orders" Codebehind="SelectSiteOrderSub.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <sos:TitleBar ID="TitleBar1" runat="server" Title="Select SubContractor Order" />

<table cellspacing="1" cellpadding="1">
    <tr>
        <td class="auto-style1">
            <table cellspacing="1" cellpadding="1" class="auto-style6">
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                   <td class="auto-style3">
                       <asp:LinkButton ID="cmdCancelTop" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdCancel_Click">Cancel</asp:LinkButton>
                   </td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        &nbsp;</td>
                    <td class="auto-style5">
                        &nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style3">
                        &nbsp;&nbsp;</td>
                    <td class="frmText">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style8">
                        Site Order</td>
                    <td class="auto-style5">
                        <asp:TextBox ID="txtOrderId" runat="server" Width="82px"></asp:TextBox>
                        </td>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style3">
                       <asp:LinkButton ID="cmdSelect" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdSearch_Click">View</asp:LinkButton>
                        </td>
                    <td class="frmText">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
 
            </table>
        </td>
    </tr>
    <tr>
        <td class="auto-style1">            
            &nbsp;</td>
    </tr>
</table>

</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            width: 362px;
        }
        .auto-style3 {
            width: 190px;
        }
        .auto-style5 {
            width: 11px;
        }
        .auto-style6 {
            width: 506px;
        }
        .auto-style7 {
            padding-bottom: 3px;
            text-align: center;
            width: 161px;
        }
        .auto-style8 {
            width: 161px;
        }
    </style>
</asp:Content>


