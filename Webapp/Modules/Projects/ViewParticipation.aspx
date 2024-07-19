<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewParticipationPage" Title="Trade Subcontractor" Codebehind="ViewParticipation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Trade Subcontractor" />

<asp:Panel ID="pnlProposal" runat="server">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="frmFormMsg">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>                
            </asp:Panel>
        
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                        <td><asp:LinkButton ID="cmdSendByEmail" OnClick="cmdSendByEmail_Click" runat="server"><asp:Image runat="server" AlternateText="Send invitation by email" ImageUrl="~/Images/IconEmail.gif" /></asp:LinkButton></td>
                        <td>&nbsp;&nbsp;</td>
                        <td><asp:LinkButton ID="cmdSendReminder" OnClick="cmdSendReminder_Click" runat="server"><asp:Image runat="server" AlternateText="Send quote reminder by email" ImageUrl="~/Images/IconEmailAlt.png" /></asp:LinkButton></td>
                        <td>&nbsp;&nbsp;</td>
                        <td><asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton></td>
                        <td>&nbsp;&nbsp;</td>
                        <td><asp:LinkButton ID="cmdDeleteTop" runat="server" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton></td>
                        <td>&nbsp;&nbsp;</td>
                    </asp:Panel>
                    <td>
                        <table cellpadding="4">
                            <tr>
                                <td><asp:HyperLink ID="lnkInvitation" runat="server" CssClass="frmLink">Invitation to Tender</asp:HyperLink></td>
                            </tr>
                        </table>                    
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlErrors" runat="server" Visible="false">
                            <asp:TreeView ID="TreeViewCheck" runat="server" ShowLines="true">
                                <LevelStyles>
                                    <asp:TreeNodeStyle CssClass="frmSubTitle" />
                                    <asp:TreeNodeStyle CssClass="frmText" />
                                    <asp:TreeNodeStyle CssClass="frmError" />
                                </LevelStyles>
                                <Nodes>
                                    <asp:TreeNode></asp:TreeNode>
                                </Nodes>
                            </asp:TreeView>
                        <br />
                        </asp:Panel>
                    </td>
                </tr>            
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Short Name:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkSubcontractor" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Invitation Date:</td>
                    <td class="frmData"><asp:Label ID="lblInvitationDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Status:</td>
                    <td class="frmData"><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Reminder Date:</td>
                    <td class="frmData"><asp:Label ID="lblReminderDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quote Amount:</td>
                    <td class="frmData">
                        <asp:UpdatePanel ID="upQuoteAmount" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="cmdCopyQuote" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblQuoteAmount" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Quote Date:</td>
                    <td class="frmData"><asp:Label ID="lblQuoteDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Rank:</td>
                    <td class="frmData"><asp:Label ID="lblRank" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Due Date:</td>
                    <td class="frmData"><asp:Label ID="lblDueDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contact:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkContact" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td>&nbsp;</td>
                    <%-- # --%>
                    <td class="frmLabel">Safety Rating:</td>
                    <td class="frmData"><asp:Label ID="lblSafetyRating" runat="server"></asp:Label></td>
                    <%-- # --%>
                </tr>
                <tr>
                    <td class="frmLabel"></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Payment Terms:</td>
                    <td class="frmData"><asp:DropDownList ID="ddlPaymentTerms" runat="server" Height="24px" Width="128px" Enabled="false">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Subcontractor&#39;s Comments:</td>
                    <td class="frmData" colspan="4">
                        <asp:UpdatePanel ID="upComments" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="cmdCopyQuote" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblComments" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">VC Internal Comments</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblInternalComments" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
</asp:Panel>

<asp:Panel ID="pnlQuote" runat="server" Visible="false">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table>
                <tr>
                    <td class="lstTitle" valign="top">Quote</td>
                    <td>
                        <asp:UpdatePanel ID="upCopyQuote" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="cmdCopyQuote" />
                            </Triggers>
                            <ContentTemplate>
                                <td>&nbsp;</td>
                                <td><asp:HyperLink ID="lnkPrint" runat="server" ToolTip="Print Online Quote" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                            
                                <td>&nbsp;</td>
                                <td><sos:FileLink ID="sflQuoteFile" runat="server" FileTitle="Quote File" /></td>

                                <asp:Panel ID="pnlCopyQuote" runat="server" Visible="true">
                                    <td>&nbsp;</td>
                                    <td><asp:LinkButton id="cmdCopyQuote" CssClass="frmButton" runat="server" OnClick="cmdCopyQuote_Click" OnClientClick="javascript:return confirm('Copy to comparison ?');">Copy to Comparison</asp:LinkButton></td>
                                </asp:Panel>
                                
                                <asp:Panel ID="pnlCopyMessage" runat="server" Visible="false">
                                    <td>&nbsp;</td>
                                    <td class="frmText">
                                        <table>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="frmText">The quote has been copied to the</td>
                                                        <td><asp:HyperLink ID="lnkComparison" CssClass="frmLink" runat="server">Comparison</asp:HyperLink></td>
                                                    </tr>
                                                </table>
                                            </td>
                                            
                                            <asp:Panel ID="pnlCopyErrors" runat="server" Visible="false">
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td class="frmText">Items not found:</td
                                                            <td>
                                                                <asp:Repeater ID="repErrorsCopy" runat="server">
                                                                    <ItemTemplate><li class="frmError"><%#SOS.UI.Utils.SetFormString((String)Eval("Text"))%></li></ItemTemplate>
                                                                </asp:Repeater>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </asp:Panel>
                                        </table>
                                    </td>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmForm"><sos:ViewComparison ID="ViewComparison1" runat="Server"></sos:ViewComparison></td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmLabel">Quote Amount $:</td>
                    <td class="frmData"><asp:Label ID="lblQuoteAmountQuote" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td class="frmData"><asp:TextBox ID="txtCommentsQuote" runat="server" CssClass="frmData" ReadOnly="true" TextMode="MultiLine" Rows="4" Width="480"></asp:TextBox></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow"><sos:ViewQuoteDrawings ID="ViewQuoteDrawings1" runat="Server"></sos:ViewQuoteDrawings></td>
    </tr>
</table>
</asp:Panel>
<br />

</asp:Content>
