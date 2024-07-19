<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewContractPage" Title="Contract" Codebehind="ViewContract.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<sos:TitleBar ID="TitleBar1" runat="server" Title="Contract" />

<asp:PlaceHolder ID="phChanges" runat="server" Visible="false">
<asp:TreeView ID="TreeView1" runat="server" ShowLines="true">
    <Nodes>
        <asp:TreeNode Expanded="False" Text="Changes" Value="Changes" SelectAction="None" />
    </Nodes>
    <LevelStyles>                
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="lstLinkBig" />
        <asp:TreeNodeStyle CssClass="frmText" />
    </LevelStyles>
</asp:TreeView>
<br />
</asp:PlaceHolder>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td><asp:HyperLink ID="lnkPrint" runat="server" ToolTip="Print" ImageUrl="~/Images/IconPrint.gif" BorderStyle="Solid" BorderColor="#FFFFFF" BorderWidth="5" Visible="false"></asp:HyperLink></td>
        <td>
            <asp:Panel ID="pnlCheckList" runat="server" Visible="false">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="frmForm">
                        <table cellpadding="1" cellspacing="0">
                            <tr>
                                <td class="frmLabel">Quotes:</td>
                                <td class="frmLabel"><sos:BooleanViewer ID="sbvQuotes" runat="server"></sos:BooleanViewer></td>
                                <td class="frmLabel">&nbsp;&nbsp;</td>
                                <td class="frmLabel">Winning Quote:</td>
                                <td class="frmLabel"><sos:BooleanViewer ID="sbvWinningQuote" runat="server"></sos:BooleanViewer></td>
                                <td class="frmLabel">&nbsp;&nbsp;</td>
                                <td class="frmLabel">Comparison:</td>
                                <td class="frmLabel"><sos:BooleanViewer ID="sbvComparison" runat="server"></sos:BooleanViewer></td>
                                <td class="frmLabel">&nbsp;&nbsp;</td>
                                <td class="frmLabel">Check List:</td>
                                <td class="frmLabel"><sos:BooleanViewer ID="sbvCheckList" runat="server"></sos:BooleanViewer></td>
                                <td class="frmLabel">&nbsp;&nbsp;</td>
                                <td class="frmLabel">Order Letting Minutes:</td>
                                <td class="frmLabel"><sos:BooleanViewer ID="sbvPrelettingMinutes" runat="server"></sos:BooleanViewer></td>
                                <td class="frmLabel">&nbsp;&nbsp;</td>
                                <td class="frmLabel">Amendments to Comparison:</td>
                                <td class="frmLabel"><sos:BooleanViewer ID="sbvAmendments" runat="server"></sos:BooleanViewer></td>
                            </tr>
                            <tr>
                                <td class="frmData" colspan="14"><asp:Label ID="lblComments" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>        
        </td>
    </tr>
</table>

<table cellpadding="0" cellspacing="0" width="750">
    <tr>
        <td class="frmForm"><asp:Literal ID="litContract" runat="server"></asp:Literal></td>
    </tr>
</table>
<br />

</asp:Content>

