<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewProjectTradePage" Title="Trade" Codebehind="ViewProjectTrade.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<script src="../../JQuery/jquery-1.7.1.js" type="text/javascript"></script>
<sos:TitleBar ID="TitleBar1" runat="server" Title="Trade" />

<table>
    <tr>
        <td><asp:Image ID="imgGeneralInfo" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">General Information</td>
    </tr>
</table>

<asp:Panel ID="pnlGeneralInfo" runat="server" CssClass="collapsePanel" Height="0">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image ID="Image1" runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image ID="Image2" runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Code:</td>
                    <td class="frmData"><asp:Label ID="lblCode" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Job Type:</td>
                    <td class="frmData"><asp:Label ID="lblJobType" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Requires Tender:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvTenderRequired" runat="server"></sos:BooleanViewer></td>
                </tr>
                <tr>
                    <td class="frmLabel">Description:</td>
                    <td class="frmData"><asp:Label ID="lblDescription" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Days from PCD:</td>
                    <td class="frmData"><asp:Label ID="lblDaysFromPCD" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Project Manager (PM):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkPM" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Contracts Administrator (CA):</td>
                    <td class="frmData"><asp:HyperLink ID="lnkCA" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr>
                    <td class="frmLabel">Flag:</td>
                    <td class="frmData"><asp:Image ID="imgRedFlag" runat="server" Visible="false" ImageUrl="~/images/RedFlag.png" /><asp:Image ID="imgGreenFlag" runat="server" Visible="false" ImageUrl="~/images/GreenFlag.png" /></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Header:</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="txtScopeHeader" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Width="640" Rows="8" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Footer:</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="txtScopeFooter" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Width="640" Rows="8" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quotes File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblQuotesFileName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Order Letting File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblPrelettingFileName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Signed Contract File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblSignedContractFile" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Invitation Date:</td>
                    <td class="frmData"><asp:Label ID="lblInvitationDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Work Order Number:</td>
                    <td class="frmData"><asp:Label ID="lblWorkOrder" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quotes Due Date:</td>
                    <td class="frmData"><asp:Label ID="lblQuotesDueDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Commencement Date:</td>
                    <td class="frmData"><asp:Label ID="lblCommencementDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Comparison Due Date:</td>
                    <td class="frmData"><asp:Label ID="lblComparisonDueDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Completion Date:</td>
                    <td class="frmData"><asp:Label ID="lblCompletionDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contract Due Date:</td>
                    <td class="frmData"><asp:Label ID="lblContractDueDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<br />

<table>
    <tr>
        <td><asp:Image ID="imgItems" ImageUrl="~/Images/IconCollapse.jpg" AlternateText="View/Hide Details" style="cursor: pointer;" runat="server" /></td>
        <td class="lstTitle">Items Categories</td>
        
        <asp:PlaceHolder ID="phAddItemCategory" runat="server" Visible="false">
        <td>&nbsp;</td>
        <td><asp:HyperLink ID="lnkAddItemCategory" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
        </asp:PlaceHolder>
    </tr>
</table>

<asp:Panel ID="pnlItems" runat="server" CssClass="collapsePanel" Height="0">
<table>
    <tr>
        <td>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="aupItemCategories" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostbackTrigger ControlID="gvItemCategories" EventName="RowCommand" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView
                        ID = "gvItemCategories"
                        runat = "server"
                        DataKeyNames="Id"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CellSpacing = "0"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem"
                        OnRowCommand = "gvItemCategories_RowCommand">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProjectTradeItemCategory.aspx?TradeItemCategoryId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="ShortDescription" HeaderText="Short Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="LongDescription" HeaderText="Long Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:ButtonField CommandName="MoveUp" ButtonType="Image" ImageUrl="~/Images/IconUp.gif" Text="Up" ItemStyle-VerticalAlign="Top" Visible="false" />
                            <asp:ButtonField CommandName="MoveDown" ButtonType="Image" ImageUrl="~/Images/IconDown.gif" Text="Down" ItemStyle-VerticalAlign="Top" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
</asp:Panel>
<br />

<table>
    <tr>
        <td><asp:Image ID="imgProposal" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Proposal</td>
    </tr>
</table>

<asp:Panel ID="pnlProposal" runat="server" CssClass="collapsePanelProposal" Height="0">
<asp:UpdatePanel ID="upParticipantsProposal" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="butAddParticipantProposal" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Tender Invitation List</td>

                            <asp:PlaceHolder ID="phAddParticipantProposal" runat="server" Visible="false">
                            <td>&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtParticipantProposalName" Width="150" runat="server" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                                &nbsp;
                                <asp:HyperLink ID="cmdSelParticipantProposal" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                                &nbsp;
                                <input id="txtParticipantProposalId" type="hidden" runat="server" />
                            </td>
                            <td><asp:Button ID="butAddParticipantProposal" runat="server" OnClick="butAddParticipantProposal_Click" Text="Add" /></td>
                            </asp:PlaceHolder>
                            
                            <td>&nbsp;</td>
                            <td><asp:HyperLink ID="lnkComparisonProposal" runat="server" CssClass="frmLink">Comparison</asp:HyperLink></td>
                            <td>&nbsp;</td>
                            <td><asp:HyperLink ID="lnkCheckListProposal" runat="server" CssClass="frmLink">Check List</asp:HyperLink></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView 
                        ID = "gvParticipantsProposal"
                        runat = "server"
                        DataKeyNames = "Id"
                        OnRowCommand = "gvParticipantsProposal_RowCommand"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CellSpacing = "0"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewParticipation.aspx?ParticipationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:LinkButton Runat="server" OnClientClick="return confirm('Copy to active tender invitation list?');" CommandName="Copy" CommandArgument="<%#Container.DataItemIndex%>"><asp:Image runat="server" AlternateText="Copy to active" ImageUrl="~/Images/IconCopy.png" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subcontractor" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <span class="<%#StyleNameSelectedParticipation((int?)Eval("Rank"))%>">
                                        <%#SOS.UI.Utils.SetFormString((String)Eval("SubcontractorName"))%>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invitation Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("InvitationDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reminder Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ReminderDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quote Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("QuoteDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("QuoteDueDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormString((String)Eval("StatusName"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comparison" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#ComparisonTotal((SOS.Core.TradeParticipationInfo)(Container.DataItem))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                        <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Rank"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<br />

<asp:UpdatePanel ID="upDrawingsProposal" runat="server" RenderMode="Inline" UpdateMode="Conditional">
<Triggers>
    <asp:AsyncPostbackTrigger ControlID="butAddDrawingType" EventName="Click" />
    <asp:AsyncPostbackTrigger ControlID="gvDrawingTypes" EventName="RowDeleting" />
    <asp:AsyncPostbackTrigger ControlID="cmdDrawingsProposal" EventName="Click" />
</Triggers>
<ContentTemplate>
<table>
    <tr>
        <td id="tdDrawingsProposal">
            <asp:TreeView ID="tvDrawingsProposal" runat="server">
                <LevelStyles>
                    <asp:TreeNodeStyle CssClass="lstTitle" />
                    <asp:TreeNodeStyle CssClass="lstLinkBig" />
                    <asp:TreeNodeStyle CssClass="lstLinkBig" />
                    <asp:TreeNodeStyle CssClass="lstLink" />
                </LevelStyles>
            </asp:TreeView>
        </td>
        <td>&nbsp;</td>
        <td valign="bottom"><asp:Button ID="cmdDrawingsProposal" runat="server" Text="Update" OnClick="cmdDrawingsProposal_Click" Visible="False" /></td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Panel>
<br />



<!-----San-------To update Subctractor's signed contract file path of ---->


<table id="TblSignedContract" runat ="server">
    <tr>
        <td><asp:Image ID="ImgSignedContract" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Attach Subcontractor's signed Contract</td>
    </tr>
    <tr><td colspan="2" >


    <asp:Panel ID="pnlSignedContract" runat="server" CssClass="collapsePanel" Height="0">

        <table class="frmGroup">
            <tr><td></td><td>&nbsp;</td><td></td></tr>
            <tr><td class="frmLabel">Subcontractor's signed Contract File:</td>
                        <td class="frmText" ><sos:FileSelect ID="sfsSignedContractFile" runat="server" /></td><td></td></tr>
            <tr><td></td><td>
                <asp:Button ID="btnSignedContractFile" runat="server" OnClick="btnSignedContractFile_Click" Text="Update" /></td><td></td></tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>

    </asp:Panel>



</td></tr>
</table>    


<!------San---------------->







<asp:Panel ID="pnlStatusActive" runat="server">

<asp:UpdatePanel ID="upBudget" runat="server" RenderMode="Inline" UpdateMode="Conditional">
<ContentTemplate>
    <table>
        <tr>
            <td class="lstTitle">Budget</td>
        </tr>
        <tr>
            <td id="tdBudget"><sos:TradeBudget id="tradeBudget" runat="server"></sos:TradeBudget></td>
        </tr>
    </table>
    <br />
</ContentTemplate>
</asp:UpdatePanel>

<table>
<tr>
    <td>
        <table>
            <tr>
                <td class="lstTitle">Approval Process Manager</td>
                
                <td>&nbsp;</td>
                <td><sos:FileLink ID="sflQuotesFile" runat="server" FileTitle="Quotes File" /></td>

                <td>&nbsp;</td>
                <td><sos:FileLink ID="sflPrelettingFile" runat="server" FileTitle="Order Letting File" /></td>

                <!---San--->
                 <td>&nbsp;</td>
                <td><sos:FileLink ID="sflSignedContractFile" runat="server" FileTitle="Signed Contract File" /></td>

                <!--San--->


                <asp:PlaceHolder ID="phPreletting" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkPreletting" runat="server" CssClass="frmLink">Order Letting Minutes</asp:HyperLink></td>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="phContract" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkContract" runat="server" CssClass="frmLink">Contract</asp:HyperLink></td>
                </asp:PlaceHolder>

                <%--<asp:PlaceHolder ID="phForemanContract" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkForemanContract" runat="server" CssClass="frmLink">Foreman Contract</asp:HyperLink></td>
                </asp:PlaceHolder>--%>

                <asp:PlaceHolder ID="phVariations" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkVariations" runat="server" CssClass="frmLink">Variation Orders</asp:HyperLink></td>
                </asp:PlaceHolder>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td class="frmGroup">
        <table>
            <tr>
                <td class="lstSubTitle">Comparison&nbsp;<asp:Label ID="lblCheckComparisonAmount" runat="server" CssClass="frmWarning"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upProcessTrade" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostbackTrigger ControlID="tradeBudget" EventName="Updated" />
                        </Triggers>
                        <ContentTemplate>
                            <sos:ProcessManager ID="ProcessManagerTrade" runat="Server"></sos:ProcessManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td class="lstSubTitle">Contract</td>
            </tr>
            <tr>
                <td><sos:ProcessManager ID="ProcessManagerContract" runat="Server"></sos:ProcessManager></td>
            </tr>
        </table>
    </td>
</tr>
</table>
<br />

<asp:UpdatePanel ID="upParticipants" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="butAddParticipant" EventName="Click" />
        <asp:AsyncPostbackTrigger ControlID="gvItemCategories" EventName="RowCommand" />
        <asp:AsyncPostbackTrigger ControlID="gvParticipantsProposal" EventName="RowCommand" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Tender Invitation List</td>
                            <asp:PlaceHolder ID="phAddParticipant" runat="server" Visible="false">
                            <td>&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtParticipantName" Width="150" runat="server" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                                &nbsp;
                                <asp:HyperLink ID="cmdSelParticipant" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                                &nbsp;
                                <input id="txtParticipantId" type="hidden" runat="server" />
                            </td>
                            <td><asp:Button ID="butAddParticipant" runat="server" OnClick="butAddParticipant_Click" Text="Add" /></td>
                            </asp:PlaceHolder>
                            
                            <td>&nbsp;</td>
                            <td><asp:HyperLink ID="lnkComparison" runat="server" CssClass="frmLink">Comparison</asp:HyperLink></td>
                            <td>&nbsp;</td>
                            <td><asp:HyperLink ID="lnkCheckList" runat="server" CssClass="frmLink">Check List</asp:HyperLink></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlCopyErrors" runat="server" Visible="false">
                        <asp:TreeView ID="TreeViewCheckCopy" runat="server" ShowLines="true">
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="frmSubTitle" />
                                <asp:TreeNodeStyle CssClass="frmText" />
                                <asp:TreeNodeStyle CssClass="frmError" />
                            </LevelStyles>
                        </asp:TreeView>
                        <br />
                    </asp:Panel>
                
                    <asp:GridView 
                        ID = "gvParticipants"
                        runat = "server"
                        DataKeyNames = "Id"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CellSpacing = "0"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewParticipation.aspx?ParticipationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subcontractor" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <span class="<%#StyleNameSelectedParticipation((int?)Eval("Rank"))%>">
                                        <%#SOS.UI.Utils.SetFormString((String)Eval("SubcontractorName"))%>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invitation Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("InvitationDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reminder Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ReminderDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quote Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("QuoteDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("QuoteDueDate"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormString((String)Eval("StatusName"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comparison" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#ComparisonTotal((SOS.Core.TradeParticipationInfo)(Container.DataItem))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                        <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Rank"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<br />

<asp:UpdatePanel ID="upDrawings" runat="server" RenderMode="Inline" UpdateMode="Conditional">
<Triggers>
    <asp:AsyncPostbackTrigger ControlID="butAddDrawingType" EventName="Click" />
    <asp:AsyncPostbackTrigger ControlID="gvDrawingTypes" EventName="RowDeleting" />
    <asp:AsyncPostbackTrigger ControlID="cmdDrawings" EventName="Click" />
</Triggers>
<ContentTemplate>
<table>
    <tr>
        <td id="tdDrawings">
            <asp:TreeView ID="tvDrawings" runat="server">
                <LevelStyles>
                    <asp:TreeNodeStyle CssClass="lstTitle" />
                    <asp:TreeNodeStyle CssClass="lstLinkBig" />
                    <asp:TreeNodeStyle CssClass="lstLinkBig" />
                    <asp:TreeNodeStyle CssClass="lstLink" />
                </LevelStyles>
            </asp:TreeView>
        </td>
        <td>&nbsp;</td>
        <td valign="bottom"><asp:Button ID="cmdDrawings" runat="server" Text="Update" OnClick="cmdDrawings_Click" Visible="False" /></td>
    </tr>
</table>
<br />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Panel>

<asp:UpdatePanel ID="upDrawingTypes" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="butAddDrawingType" EventName="Click" />
        <asp:AsyncPostbackTrigger ControlID="gvDrawingTypes" EventName="RowDeleting" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Drawing Types</td>

                            <asp:PlaceHolder ID="phAddDrawingTypes" runat="server" Visible="false">
                            <td>&nbsp;</td>
                            <td id="tdDrawingTypes"><asp:DropDownList ID="ddlDrawingTypes" runat="server"></asp:DropDownList></td>
                            <td><asp:Button ID="butAddDrawingType" runat="server" OnClick="butAddDrawingType_Click" Text="Add" /></td>
                            </asp:PlaceHolder>                    
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView 
                        ID="gvDrawingTypes" 
                        runat="server"
                        DataKeyNames="Id"
                        OnRowDeleting="gvDrawingTypes_RowDeleting"
                        AutoGenerateColumns="False" 
                        CellPadding="2" 
                        CellSpacing="0" 
                        CssClass="lstList" 
                        RowStyle-CssClass="lstItem" 
                        AlternatingRowStyle-CssClass="lstAltItem">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" Runat="server" OnClientClick="return confirm('Delete Drawing Type from Trade?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<br />

<asp:PlaceHolder ID="phAddendums" runat="server" Visible="false">
<table>
    <tr>
        <td>
            <table>
                <tr>
                    <td class="lstTitle">Tender Addendums</td>

                    <asp:PlaceHolder ID="phAddAddendum" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkAddAddendum" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                    </asp:PlaceHolder>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView 
                ID = "gvAddendums"
                runat = "server"
                DataKeyNames = "Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewAddendum.aspx?AddendumId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                                        
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("AddendumDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="AttachmentsInfo" HeaderText="Attachments" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
</asp:PlaceHolder>

<act:CollapsiblePanelExtender
    ID="cpe"
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlGeneralInfo"
    ExpandControlID="imgGeneralInfo"
    CollapseControlID="imgGeneralInfo"
    ImageControlID="imgGeneralInfo"         
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>
        
<act:CollapsiblePanelExtender
    ID="cpe1"
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlProposal"
    ExpandControlID="imgProposal"
    CollapseControlID="imgProposal"
    ImageControlID="imgProposal"         
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

<act:CollapsiblePanelExtender
    ID="cpe2" 
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"    
    TargetControlID="pnlItems"
    ExpandControlID="imgItems"
    CollapseControlID="imgItems"
    ImageControlID="imgItems"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

<!----San------>
 
<act:CollapsiblePanelExtender
    ID="CpSignedContract" 
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"    
    TargetControlID="pnlSignedContract"
    ExpandControlID="ImgSignedContract"
    CollapseControlID="ImgSignedContract"
    ImageControlID="ImgSignedContract"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

<!--San---->



<act:UpdatePanelAnimationExtender ID="upaeBudget" runat="server" TargetControlID="upBudget">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdBudget" Duration="0.3" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:UpdatePanelAnimationExtender ID="upaeDrawingTypes" runat="server" TargetControlID="upDrawingTypes">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdDrawingTypes" Duration="0.3" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:UpdatePanelAnimationExtender ID="upaeDrawings" runat="server" TargetControlID="upDrawings">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdDrawings" Duration="0.3" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:UpdatePanelAnimationExtender ID="upaeDrawingsProposal" runat="server" TargetControlID="upDrawingsProposal">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdDrawingsProposal" Duration="0.3" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

</asp:Content>
