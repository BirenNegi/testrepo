<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewContractSectionPage" Title="Contract Section" Codebehind="ViewContractSection.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Contract Section" />

<table cellspacing="0" cellpadding="0" width="100%">
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Contract Section:</td>
                    <td class="frmText"><asp:Label ID="lblSection" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td class="frmSubTitle">Original text from template</td>
                </tr>
                <tr>
                    <td class="TemplateEditable"><asp:Literal ID="litOrignialText" runat="server"></asp:Literal></td>
                </tr>
                <asp:Repeater ID="repModifications" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="frmSubTitle"><%#SOS.UI.Utils.SetFormDateTime((DateTime)Eval("CreatedDate"))%>&nbsp;<%#SOS.UI.Utils.SetFormString((String)Eval("Employee.Name"))%></td>
                        </tr>
                        <tr>
                            <td class="ContractModification"><%#SOS.UI.Utils.SetFormString((String)Eval("SectionText"))%></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:HyperLink ID="lnkModify" runat="server" CssClass="frmButton" Visible="false" Text="Modify"></asp:HyperLink>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

</asp:Content>
