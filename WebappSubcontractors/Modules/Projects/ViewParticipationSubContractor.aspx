<%@ Page Language="C#" MasterPageFile="~/MasterPages/Subbie.master" AutoEventWireup="True" Inherits="SOS.Web.ViewParticipationSubContractorPage" Title="Projects" Codebehind="ViewParticipationSubContractor.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" CombineScripts="false" />
<script src="../../JQuery/jquery-1.7.1.js" type="text/javascript"></script>

   
<asp:UpdatePanel ID="upMessage" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="cmdSubmitQuote" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="frmFormMsg">
            <asp:Label ID="lblMessage" runat="server" Text="Thank you! Your quote has been submitted successfully. We will contact you with an update in the process."></asp:Label>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Timer ID="tmrClosing" runat="server" Interval="60000" Enabled="false"></asp:Timer>

<sos:TitleBar ID="TitleBar1" runat="server" Title="Project" Visible="false" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table>
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td class="frmText">Status:</td>
                                <td class="frmTitle1">
                                    <asp:UpdatePanel ID="upStatus" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostbackTrigger ControlID="cmdSubmitQuote" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Label ID="lblStatusName" runat="server"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>                                
                                </td>
                            </tr>
                            <tr>
                                <td class="frmText">Closing:</td>
                                <td class="frmTitle1">
                                    <asp:UpdatePanel ID="upClosing" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostbackTrigger ControlID="tmrClosing" EventName="Tick" />
                                            <asp:AsyncPostbackTrigger ControlID="cmdSubmitQuote" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Label ID="lblClosing" runat="server"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td valign="top">
                        <table>
                            <tr>
                                <td class="frmText">Invitation Date:</td>
                                <td class="frmSubTitle"><asp:Label ID="lblInvitationDate" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="frmText">Due Date:</td>
                                <td class="frmSubTitle"><asp:Label ID="lblQuoteDueDate" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="frmText">Submission Date:</td>
                                <td class="frmSubTitle">
                                    <asp:UpdatePanel ID="upDates" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostbackTrigger ControlID="cmdSubmitQuote" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Label ID="lblQuoteDate" runat="server"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td valign="top">
                        <table>
                            <tr>
                                <td><asp:HyperLink ID="lnkInvitation" runat="server" CssClass="frmLink">Invitation to Tender</asp:HyperLink></td>
                            </tr>
                            <tr>
                                <td><asp:HyperLink ID="lnkCheckList" runat="server" CssClass="frmLink">Check list</asp:HyperLink></td>
                            </tr>
                          
                        </table>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td valign="top">
                        <table>
                            <tr>
                                <td class="frmText">CA:</td>
                                <td class="frmText"><asp:HyperLink ID="lnkCA" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                                <td class="frmText">&nbsp;</td>
                                <td class="frmText" valign="middle"><img src="../../Images/IconPhone.png" /></td>
                                <td class="frmText" valign="middle"><asp:Label ID="lblCAPhone" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="frmText">PM:</td>
                                <td class="frmText"><asp:HyperLink ID="lnkPM" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                                <td class="frmText">&nbsp;</td>
                                <td class="frmText" valign="middle"><img src="../../Images/IconPhone.png" /></td>
                                <td class="frmText" valign="middle"><asp:Label ID="lblPMPhone" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<table>
    <tr>
        <td>
            <table>
                <tr>
                    <td><asp:Image ID="imgQuote" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
                    <td class="lstTitle">Quote</td>
                    
                    <asp:PlaceHolder ID="phOnlineQuote" runat="server" Visible="false">
                        <td>&nbsp;</td>
                        <td class="frmText">(Not Online quote)</td>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phPrintQuote" runat="server" Visible="false">
                        <td>&nbsp;</td>
                        <td><asp:HyperLink ID="lnkPrint" runat="server" ToolTip="Print Online Quote" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phStatus" runat="server" Visible="false">
                        <td>&nbsp;</td>
                        <td><asp:Label ID="lblStatus" runat="server" CssClass="frmText"></asp:Label></td>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phEditQuote" runat="server">
                        <td>&nbsp;</td>
                        <td>
                            <asp:UpdatePanel ID="upEditQuote" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostbackTrigger ControlID="tmrClosing" EventName="Tick" />
                                    <asp:AsyncPostbackTrigger ControlID="cmdSubmitQuote" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:HyperLink ID="lnkEditQuote" runat="server" CssClass="frmLink">Edit Quote</asp:HyperLink>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phSubmitQuote" runat="server">
                        <td>&nbsp;</td>
                        <td>
                            <asp:UpdatePanel ID="upSubmitQuote" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostbackTrigger ControlID="tmrClosing" EventName="Tick" />
                                    <asp:AsyncPostbackTrigger ControlID="cmdSubmitQuote" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:LinkButton id="cmdSubmitQuote" CssClass="frmButton" runat="server" OnClick="cmdSubmitQuote_Click">Submit Quote</asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phContract" runat="server" Visible="false">
                        <td>&nbsp;</td>
                        <td><asp:HyperLink ID="lnkContract" runat="server" CssClass="frmLink">Contract</asp:HyperLink></td>
                    </asp:PlaceHolder>

                      <%-- #--21012019 --%>
                      <td>&nbsp;</td>      
                     <asp:PlaceHolder ID="phDraftContract" runat="server" Visible="false">
                     <td>
                        <asp:HyperLink ID="lnkDraftContract" runat="server" CssClass="frmLink">Draft Contract</asp:HyperLink>

                     </td>
                </asp:PlaceHolder>
                     <%-- #--21012019 --%>
                               





                </tr>
            </table>
        </td>
    </tr>
</table>

<asp:Panel ID="pnlQuote" runat="server" CssClass="collapsePanel" Height="0">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td class="auto-style1">
            <asp:TreeView ID="TreeView1" runat="server" ShowLines="true">
                <Nodes>
                    <asp:TreeNode Text="Quote Check...Ok" SelectAction="None" />
                </Nodes>
                <LevelStyles>                
                    <asp:TreeNodeStyle CssClass="frmSubTitle" />
                    <asp:TreeNodeStyle CssClass="frmError" />
                    <asp:TreeNodeStyle CssClass="frmText" />
                </LevelStyles>
            </asp:TreeView>
        </td>    
    </tr>
    <tr><td>&nbsp;</td></tr>
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel" valign="top">Quote File: </td>
                    <td>
                        <asp:Panel ID="pnlViewQuoteFile" runat="server" Visible="false">
                            <span>
                                <asp:HyperLink ID="lnkViewQuoteFile" runat="server" CssClass="frmLink"></asp:HyperLink>
                                &nbsp;
                                <asp:Label ID="lblFileSize" runat="server" CssClass="frmText"></asp:Label>
                            </span>
                            
                            <span ID="spnRemoveQuoteFile" runat="server" Visible="true">
                                &nbsp;
                                &nbsp;
                                <asp:LinkButton ID="lnkRemoveQuoteFile" runat="server" CssClass="frmLink" onclick="lnkRemoveQuoteFile_Click" OnClientClick="javascript:return confirm('Remove Quote File ?');">Remove</asp:LinkButton>
                            </span>
                        </asp:Panel>
                        
                        <asp:Panel ID="pnlUploadQuoteFile" runat="server" Visible="false">
                            <asp:HyperLink ID="lnkUploadQuoteFile" runat="server" CssClass="frmLink">Upload</asp:HyperLink>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <asp:UpdatePanel ID="upViewComparison" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostbackTrigger ControlID="cmdSubmitQuote" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <sos:ViewComparison ID="ViewComparison1" runat="Server"></sos:ViewComparison>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td class="frmData"><asp:TextBox ID="txtComments" runat="server" CssClass="frmData" ReadOnly="true" TextMode="MultiLine" Rows="4" Width="480"></asp:TextBox></td>
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

<table>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="lstTitle">Drawings</td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">*&nbsp;<span class="lstTextHighlighted">New revisions: light red.</span>&nbsp;<span class="lstTextGrayedOut">Old revisions: dark red.</span></td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="aupDrawings" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <table cellpadding="2" cellspacing="0" border="0" style="height:36px;">
                                    <tr>
                                        <td class="frmSubTitle">Download Multiple Drawings</td>
                                        <td>&nbsp;</td>
                                        <td class="frmText">Selected:</td>
                                        <td class="frmSubTitle"><asp:Label ID="lblSelected" runat="server"></asp:Label></td>
                                        <td>&nbsp;</td>
                                        <td class="frmText">Total Size:</td>
                                        <td class="frmSubTitle"><asp:Label ID="lblSize" runat="server" CssClass="frmSubTitle"></asp:Label></td>
                                        <td class="frmError"><asp:Label ID="lblSizeError" runat="server"></asp:Label></td>
                                        <td>&nbsp;</td>
                                        <td><asp:HyperLink ID="lnkDownloadAll" runat="server"><asp:Image ID="imglnkDownloadAll" runat="server" ImageUrl="~/Images/IconDownload.gif" /></asp:HyperLink></td>
                                    </tr>
                                </table>        
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <sos:GridPageSize ID="GridPageSizeDrawings" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView 
                                    OnPageIndexChanging="gvDrawings_OnPageIndexChanging"
                                    OnSorting="gvDrawings_OnSorting"
                                    OnRowCreated="gvDrawings_OnRowCreated"
                                    OnRowDataBound="gvDrawings_OnRowDataBound"
                                    ID="gvDrawings"
                                    runat="server"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="10"
                                    PagerStyle-CssClass="lstPager"
                                    DataKeyNames="Id"
                                    AutoGenerateColumns="False" 
                                    CellPadding="2" 
                                    CellSpacing="0" 
                                    CssClass="lstList" 
                                    RowStyle-CssClass="lstItem" 
                                    AlternatingRowStyle-CssClass="lstAltItem">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" ToolTip="Select All" OnClick="SelectAllDrawings()" />
                                            </HeaderTemplate>
                                            <ItemTemplate>                                    
                                                    <asp:CheckBox ID="chkSelect" runat="server" OnClick="UpdateDownloadInfo()" Visible='<%#FormatFileLink(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" ImageUrl="~/Images/IconDocument.gif" ToolTip="Download" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowDrawingRevision.aspx?DrawingRevisionIds={0}", Eval("LastRevisionIdStr"))%>' Visible='<%#FormatFileLink(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField SortExpression="DrawingType" HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("DrawingType")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField SortExpression="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("Name")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField SortExpression="LastRevisionNumber" HeaderText="Revision" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("LastRevisionNumber")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField SortExpression="LastRevisionDate" HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormDate(SOS.Data.Utils.GetDBDateTime(Eval("LastRevisionDate")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="File Size" SortExpression="LastRevisionFileSize" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#FormatFileSize(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td class="lstTitle">Transmittals</td>
    </tr>
</table>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="aupTransmittals" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <sos:GridPageSize ID="GridPageSizeTransmittals" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView 
                                    OnPageIndexChanging="gvTransmittals_OnPageIndexChanging"
                                    OnSorting="gvTransmittals_OnSorting"
                                    OnRowCreated="gvTransmittals_OnRowCreated"
                                    ID="gvTransmittals"
                                    runat="server"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="10"
                                    PagerStyle-CssClass="lstPager"
                                    DataKeyNames="Id"
                                    AutoGenerateColumns="False" 
                                    CellPadding="2" 
                                    CellSpacing="0" 
                                    CssClass="lstList" 
                                    RowStyle-CssClass="lstItem" 
                                    AlternatingRowStyle-CssClass="lstAltItem">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" ToolTip="View" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowTransmittal.aspx?TransmittalId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField SortExpression="Number" HeaderText="Number" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#SOS.Data.Utils.GetDBInt32(Eval("Number"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Title" HeaderStyle-CssClass="lstHeader"></asp:BoundField>

                                        <asp:TemplateField SortExpression="TransmissionDate" HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#SOS.UI.Utils.SetFormDate(SOS.Data.Utils.GetDBDateTime(Eval("TransmissionDate")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="# Drawings" SortExpression="NumDrawings" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#SOS.UI.Utils.SetFormInteger(SOS.Data.Utils.GetDBInt32(Eval("NumDrawings")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td class="lstTitle">Variations</td>
    </tr>
</table>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="aupSubContracts" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <sos:GridPageSize ID="GridPageSizeSubContracts" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            <asp:GridView
                                OnPageIndexChanging="gvSubContracts_OnPageIndexChanging"
                                OnSorting="gvSubContracts_OnSorting"
                                OnRowCreated="gvSubContracts_OnRowCreated"
                                ID="gvSubContracts"
                                runat="server"
                                AllowPaging="True"
                                AllowSorting="True"
                                PageSize="10"
                                PagerStyle-CssClass="lstPager"
                                DataKeyNames="Id"
                                AutoGenerateColumns="False" 
                                CellPadding="2" 
                                CellSpacing="0" 
                                CssClass="lstList" 
                                RowStyle-CssClass="lstItem" 
                                AlternatingRowStyle-CssClass="lstAltItem">
                                <Columns>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" ToolTip="View" runat="server" NavigateUrl='<%#String.Format("~/Modules/Contracts/ShowContract.aspx?ContractId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Title" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>

                                    <asp:TemplateField HeaderText="Date" SortExpression="WriteDate" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <%#SOS.UI.Utils.SetFormDate(SOS.Data.Utils.GetDBDateTime(Eval("WriteDate")))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="SiteInstruction" SortExpression="SiteInstruction" HeaderText="Site Instruction" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                                    <asp:BoundField DataField="SubContractorReference" SortExpression="SubContractorReference" HeaderText="S/Cont.Ref" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                                    
                                    <asp:TemplateField HeaderText="# Variations" SortExpression="NumVariations" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <%#SOS.UI.Utils.SetFormInteger(SOS.Data.Utils.GetDBInt32(Eval("NumVariations")))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total" SortExpression="TotalVariations" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <%#SOS.UI.Utils.SetFormDecimal(SOS.Data.Utils.GetDBDecimal(Eval("TotalVariations")))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<br />

<act:CollapsiblePanelExtender
    ID="cpeQuote" 
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlQuote"
    ExpandControlID="imgQuote"
    CollapseControlID="imgQuote"
    ImageControlID="imgQuote"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
    .auto-style1 {
        height: 26px;
    }
</style>
</asp:Content>

