<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewContactPage" Title="Contact" Codebehind="ViewContact.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Subcontractor Contact" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">First&nbsp;Name:</td>
                    <td class="frmData"><asp:Label ID="lblFirstName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Address:</td>
                    <td class="frmData"><asp:Label ID="lblStreet" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Last&nbsp;Name:</td>
                    <td class="frmData"><asp:Label ID="lblLastName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td class="frmData"><asp:Label ID="lblLocality" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Phone:</td>
                    <td class="frmData"><asp:Label ID="lblPhone" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">State:</td>
                    <td class="frmData"><asp:Label ID="lblState" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Mobile Phone:</td>
                    <td class="frmData"><asp:Label ID="lblMobile" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td class="frmData"><asp:Label ID="lblPostalCode" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Fax:</td>
                    <td class="frmData"><asp:Label ID="lblFax" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Company:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkCompany" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr>
                    <td class="frmLabel">Email Address:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkEmail" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td>&nbsp;</td>
                 <%-- San  --%>
                    <td class="frmLabel">Position:</td>
                    <td class="frmData"><asp:Label ID="lblPosition" runat="server"></asp:Label></td>
                 <%-- San  --%>
                </tr>

                <tr>
                     <td class="frmLabel">Inactive:</td>
                    <td class="frmData"><sos:BooleanViewer ID="booInactive" runat="server" /></td>
                </tr>


                
                <asp:PlaceHolder ID="phAccount" runat="server" Visible="false">
                    <tr>
                        <td colspan="5" class="frmSection">Access Infomation</td>
                    </tr>
                    
                    <asp:PlaceHolder ID="phAccountInfo" runat="server" Visible="false">
                        <tr>
                            <td colspan="5" class="frmTopBox"><asp:LinkButton ID="cmdDeleteAccount" runat="server" CssClass="frmButton" OnClick="cmdDeleteAccount_Click" Text="Delete Account"></asp:LinkButton></td>
                        </tr>
                        <tr>
                            <td class="frmLabel">User Name:</td>
                            <td class="frmData"><asp:Label ID="lblUserName" runat="server"></asp:Label></td>
                            <td>&nbsp;</td>
                            <td class="frmLabel">Last Login:</td>
                            <td class="frmData"><asp:Label ID="lblLastLogin" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="frmLabel">Access Level:</td>
                            <td class="frmData"><asp:Label ID="lblAccessLevel" runat="server"></asp:Label></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phNoAccountInfo" runat="server" Visible="false">
                        <tr>
                            <td colspan="5" class="frmTopBox"><asp:LinkButton ID="cmdCreateAccount" runat="server" CssClass="frmButton" OnClick="cmdCreateAccount_Click" Text="Create Account"></asp:LinkButton></td>
                        </tr>
                    </asp:PlaceHolder>

                </asp:PlaceHolder>

            </table>
        </td>
    </tr>
</table>

</asp:Content>

