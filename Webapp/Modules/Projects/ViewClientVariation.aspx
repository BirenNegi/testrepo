<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewClientVariationPage" Title="" Codebehind="ViewClientVariation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="" />

<table>
    <tr>
        <td><asp:Image ID="imgGeneralInfo" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">General Information</td>
    </tr>
</table>

<asp:Panel ID="pnlGeneralInfo" runat="server" CssClass="collapsePanel">
<table cellspacing="0" cellpadding="0">
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" OnClick="cmdDelete_Click" Visible="false"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
    </asp:Panel>
    
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Revision:</td>
                    <td class="frmData"><asp:Label ID="lblRevisionName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Status:</td>
                    <td class="frmData"><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Write Date:</td>
                    <td class="frmData"><asp:Label ID="lblWriteDate" runat="server"></asp:Label></td>
                </tr>
                
                <asp:Panel ID="pnlVerbalApprovalDate" runat="server">
                <tr>
                    <td class="frmLabel">Verbal Approval:</td>
                    <td class="frmData"><asp:Label ID="lblVerbalApprovalDate" runat="server"></asp:Label></td>
                </tr>
                </asp:Panel>
                
                <tr>
                    <td class="frmLabel">Final Approval:</td>
                    <td class="frmData"><asp:Label ID="lblApprovalDate" runat="server"></asp:Label></td>
                </tr>

                <asp:PlaceHolder ID="phCancelDate" runat="server" Visible="false">
                <tr>
                    <td class="frmLabel">Cancel Date:</td>
                    <td class="frmData"><asp:Label ID="lblCancelDate" runat="server"></asp:Label></td>
                </tr>
                </asp:PlaceHolder>
                
                <tr>
                    <td class="frmLabel">VC Quotes File:</td>
                    <td class="frmData"><asp:Label ID="lblQuotesFileName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Client Backup File:</td>
                    <td class="frmData"><asp:Label ID="lblBackupFileName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Client Approval File:</td>
                    <td class="frmData"><asp:Label ID="lblClientApprovalFileName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Show Cost Details:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvShowCostDetails" runat="server"></sos:BooleanViewer></td>
                </tr>

                <asp:PlaceHolder ID="phUseSecondPrincipal" runat="server">
                <tr>
                    <td class="frmLabel">Use Second Principal:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvUseSecondPrincipal" runat="server"></sos:BooleanViewer></td>
                </tr>
                </asp:PlaceHolder>

                <tr>
                    <td class="frmLabel">Comments:</td>
                    <td class="frmData"><asp:Label ID="lblComments" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    
    <asp:PlaceHolder ID="phInvoice" runat="server">
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmSubSubTitle">Invoice/Adjustment note </td>
                </tr>
            </table>

            <table width="100%">
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblInvoiceNumber" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date:</td>
                    <td class="frmData"><asp:Label ID="lblInvoiceDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Date Sent:</td>
                    <td class="frmData"><asp:Label ID="lblInvoiceSentDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date Due:</td>
                    <td class="frmData"><asp:Label ID="lblInvoiceDueDate" runat="server"></asp:Label></td>
                </tr>
                <tr style="display:none">
                    <td class="frmLabel">Date Paid:</td>
                    <td class="frmData"><asp:Label ID="lblInvoicePaidDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
</table>
</asp:Panel>
<br />

<asp:Panel ID="pnlErrors" runat="server" Visible="false">
<asp:TreeView ID="TreeViewMissingFields" runat="server" ShowLines="true" Visible="false">
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="frmText" />
    </LevelStyles>
</asp:TreeView>
<asp:TreeView ID="TreeViewErrors" runat="server" ShowLines="true" Visible="false">
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="frmError" />
    </LevelStyles>
</asp:TreeView>
</asp:Panel>

<table>
<tr>
    <td>
        <table>
            <tr>
                <td class="lstTitle" rowspan="2" valign="bottom">Approval Process Manager</td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td><sos:FileLink ID="sflQuotesFile" runat="server" FileTitle="VC Quotes File" /></td>
                            
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td><sos:FileLink ID="sflBackupFile" runat="server" FileTitle="Backup File" /></td>

                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td><sos:FileLink ID="sflClientApprovalFile" runat="server" FileTitle="Client Approval File" /></td>

                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td><asp:HyperLink ID="lnkInvoice" runat="server" CssClass="frmLink" Visible="false">Invoice/Adjustment note</asp:HyperLink></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <asp:PlaceHolder ID="phSubClientVariations" runat="server" Visible="false">
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td class="frmTextBold">Revision</td>
                                <td>&nbsp;</td>
                                <td><asp:DropDownList ID="ddlSubClientVariations" runat="server" OnSelectedIndexChanged="ddlSubClientVariations_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="phLastSubClientVariation" runat="server" Visible="false">
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><asp:HyperLink ID="lnkLastSubClientVariation" runat="server" CssClass="frmLink">Last Revision</asp:HyperLink></td>
                            </asp:PlaceHolder>
                                            
                            <asp:PlaceHolder ID="phViewClientVariation" runat="server" Visible="false">
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><asp:HyperLink ID="lnkViewClientVariation" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="phSubClientVariation" runat="server" Visible="false">
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><asp:LinkButton ID="cmdSubClientVariation" runat="server" OnClick="cmdSubClientVariation_Click" OnClientClick="javascript:return confirm('Create New Revision?');" CssClass="frmLink">New Revision</asp:LinkButton></td>
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="phCancel" runat="server" Visible="false">
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><asp:LinkButton ID="cmdCancel" runat="server" OnClick="cmdCancel_Click" CssClass="frmLink">Cancel</asp:LinkButton></td>
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="phRestore" runat="server" Visible="false">
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><asp:LinkButton ID="cmdRestore" runat="server" OnClick="cmdRestore_Click" CssClass="frmLink">Restore</asp:LinkButton></td>
                            </asp:PlaceHolder>                        
                        </tr>
                    </table>                
                </td>            
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td><sos:ProcessManager ID="ProcessManagerCV" runat="Server"></sos:ProcessManager></td>
</tr>
</table>
<br />

<table>
    <tr>
        <td>
            <table id="tblItems" cellpadding="4" cellspacing="1" runat="server" width="640">
                <tr>
                    <td colspan="3" class="lstTitle">Items</td>
                </tr>
                <tr>
                    <td class="lstHeader"></td>
                    <td class="lstHeader" align="center">Description</td>
                    <td class="lstHeader" align="center">Amount $</td>
                    <%-- San --%>
                     <td class="lstHeader" align="center"></td>
                    <td class="lstHeader" align="center"></td>
                    <%-- San --%>

                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="4" cellspacing="1">
                <asp:Panel ID="pnlTotals" runat="server" Visible="false">
                <tr>
                    <td class="lstErrItem">Items - Trades = <asp:Label ID="lblDifference" runat="server"></asp:Label></td>
                </tr>
                </asp:Panel>
            </table>
        </td>
    </tr>
    <tr>
        <td id="tdTrades">
            <asp:UpdatePanel ID="upTrades" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="lstTitle">Trades</td>
                            <td>&nbsp;&nbsp;</td>
                            <td><sos:BalanceInclude id="sbiTrades" runat="server"></sos:BalanceInclude></td>
                        </tr>
                    </table>
                    <table id="tblTrades" cellpadding="4" cellspacing="1" runat="server">
                        <tr>
                            <td class="auto-style1"></td>
                            <td class="auto-style1" align="center">Trade Code</td>
                            <td class="auto-style1" align="center">Amount $</td>
                            <td class="auto-style1" align="center">Balance $</td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<br />

<act:CollapsiblePanelExtender ID="cpe" runat="Server" Collapsed="False" TargetControlID="pnlGeneralInfo" 
             ExpandControlID="imgGeneralInfo" CollapseControlID="imgGeneralInfo" ImageControlID="imgGeneralInfo" 
             ExpandedImage="~/Images/IconCollapse.jpg" CollapsedImage="~/Images/IconExpand.jpg" ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

<act:UpdatePanelAnimationExtender ID="upaeTrades" runat="server" TargetControlID="upTrades">
    <Animations>
        <OnUpdated>
            <FadeIn AnimationTarget="tblTrades" Duration="0.5" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            color: #FFFFFF;
            background-color: #000080;
            font-family: Verdana, Arial;
            font-weight: bold;
            font-size: 8pt;
            white-space: nowrap;
            height: 22px;
        }
    </style>
</asp:Content>

