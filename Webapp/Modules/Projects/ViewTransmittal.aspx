<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewTransmittalPage" Title="Transmittal" Codebehind="ViewTransmittal.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Transmittal" />

<asp:UpdatePanel ID="upMessage" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="cmdSendByEmail" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="frmFormMsg">
            <asp:Label ID="lblMessage" runat="server" Text="Transmittal sent."></asp:Label>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Panel ID="pnlProposal" runat="server">
<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            &nbsp;
            <asp:HyperLink ID="lnkPrint" runat="server" ToolTip="Print" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink>
            
            &nbsp;
            <asp:LinkButton ID="cmdCopy" runat="server" Visible="false" OnClientClick="javascript:return confirm('Make a copy of transmittal ?');" OnClick="cmdCopy_Click"><asp:Image runat="server" AlternateText="Make a copy" ImageUrl="~/Images/IconCopy.png" /></asp:LinkButton>

            <asp:UpdatePanel ID="upSendByEmail" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="cmdSendByEmail" EventName="Click" />
                    <asp:AsyncPostbackTrigger ControlID="butAddRecipient" EventName="Click" />
                    <asp:AsyncPostbackTrigger ControlID="gvRecipients" EventName="RowDeleting" />
                    <asp:AsyncPostBackTrigger ControlID="cmdSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="repDrawings" EventName="ItemCommand" />
                </Triggers>
                <ContentTemplate>
                    <asp:PlaceHolder ID="phSendByEmail" runat="server" Visible="false">
                        &nbsp;
                        <asp:LinkButton ID="cmdSendByEmail" OnClick="cmdSendByEmail_Click" runat="server"><asp:Image runat="server" AlternateText="Send by email" ImageUrl="~/Images/IconEmail.gif" /></asp:LinkButton>
                    </asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>

            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>

            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClientClick="javascript:return confirm('Delete transmittal ?');" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
        <td>&nbsp;</td>                    
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="upRecepients" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostbackTrigger ControlID="butAddRecipient" EventName="Click" />
                    <asp:AsyncPostbackTrigger ControlID="gvRecipients" EventName="RowDeleting" />
                    <asp:AsyncPostBackTrigger ControlID="cmdSave" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="repDrawings" EventName="ItemCommand" />
                </Triggers>
                <ContentTemplate>
                    <asp:Panel ID="pnlErrors" runat="server" Visible="false">
                        <br />
                        <asp:TreeView ID="TreeViewCheck" runat="server" ShowLines="true">
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="frmSubTitle" />
                                <asp:TreeNodeStyle CssClass="frmText" />
                                <asp:TreeNodeStyle CssClass="frmError" />
                                <asp:TreeNodeStyle CssClass="frmError" />
                            </LevelStyles>
                        </asp:TreeView>
                        <br />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>



    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Type:</td>
                    <td class="frmData"><asp:Label ID="lblType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Subcontractor:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkSubcontractor" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Delivery Method:</td>
                    <td class="frmData"><asp:Label ID="lblDevilvery" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Contact:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkContact" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Required Action:</td>
                    <td class="frmData"><asp:Label ID="lblAction" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Date:</td>
                    <td class="frmData"><asp:Label ID="lblDate" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date Sent:</td>
                    <td class="frmData">
                         <asp:UpdatePanel ID="upSentDate" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmdSendByEmail" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblSentDate" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Comments:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblComments" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmSubSubTitle">Recipients From Distribution List</td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GVDistributionList" runat="server"
                        DataKeyNames="Id"
                        OnRowDeleting="gvRecipients_RowDeleting"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem">
                            <AlternatingRowStyle CssClass="lstAltItem" />
                            <Columns>
                                <asp:BoundField DataField="FirstName" HeaderText="Name" HeaderStyle-CssClass="lstHeader"  />
                                <asp:BoundField DataField="Position" HeaderText="Role" HeaderStyle-CssClass="lstHeader"  />
                                <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="lstHeader"  />
                            </Columns>
                            <RowStyle CssClass="lstItem" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
           <%--#----
               
                <table>
                <tr>
                    <td class="lstHeader">Contact</td>
                    <td class="lstHeader">Company</td>
                    <td class="lstHeader">Name</td>
                    <td class="lstHeader">Email</td>
                </tr>
                <tr id="rowClientContact" runat="server">
                    <td class="lstItem">Main Contact</td>
                    <td class="lstItem"><asp:Label ID="lblClientContactCompany" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblClientContactName" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:HyperLink ID="lnkClientContactEmail" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr id="rowClientContact1" runat="server">
                    <td class="lstItem">Contact 1</td>
                    <td class="lstItem"><asp:Label ID="lblClientContact1Company" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblClientContact1Name" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:HyperLink ID="lnkClientContact1Email" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr id="rowClientContact2" runat="server">
                    <td class="lstItem">Contact 2</td>
                    <td class="lstItem"><asp:Label ID="lblClientContact2Company" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblClientContact2Name" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:HyperLink ID="lnkClientContact2Email" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr id="rowSuperintendent" runat="server">
                    <td class="lstItem">Superintendent</td>
                    <td class="lstItem"><asp:Label ID="lblSuperintendentCompany" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblSuperintendentName" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:HyperLink ID="lnkSuperintendentEmail" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <tr id="rowQuantitySurveyor" runat="server">
                    <td class="lstItem">Qty. Surveyor</td>
                    <td class="lstItem"><asp:Label ID="lblQuantitySurveyorCompany" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblQuantitySurveyorName" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:HyperLink ID="lnkQuantitySurveyorEmail" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
            </table>
               
               --#-- --%>
        </td>
    </tr>                
</table>
<br />

<asp:UpdatePanel ID="upRecipients" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="butAddRecipient" EventName="Click" />
        <asp:AsyncPostbackTrigger ControlID="gvRecipients" EventName="RowDeleting" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Additional Recipients/or Additional Transmittals</td>
                            
                            <asp:PlaceHolder ID="phAddRecipient" runat="server" Visible="false">
                            <td>&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtRecipientName" Width="150" runat="server" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                                &nbsp;
                                <asp:HyperLink ID="cmdSelRecipient" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                                &nbsp;
                                <input id="txtRecipientId" type="hidden" runat="server" />
                            </td>
                            <td>
                                <%--#--facilitated new option to add multiple contacts at a time ----butAddRecipient  Visible="false"  --#--%>
                                    <asp:Button ID="butAddRecipient" runat="server" OnClick="butAddRecipient_Click" Text="Add"  Visible="false"/>
                                    

                            </td>
                            <td>Create individual transmittals   <asp:CheckBox ID="ChkIndvidual" runat="server" /></td>
                            </asp:PlaceHolder>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:GridView 
                        ID = "gvRecipients"
                        runat = "server"
                        DataKeyNames="Id"
                        OnRowDeleting="gvRecipients_RowDeleting"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CellSpacing = "0"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem">
                        <Columns>
                            <asp:HyperLinkField HeaderText="Subcontractor" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ControlStyle-CssClass="frmLink" DataNavigateUrlFormatString="~/Modules/SubContractors/ViewSubContractor.aspx?SubContractorId={0}" DataNavigateUrlFields="SubContractorIdStr" DataTextField="SubContractorName" />
                            <asp:HyperLinkField HeaderText="Contact" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ControlStyle-CssClass="frmLink" DataNavigateUrlFormatString="~/Modules/People/ViewContact.aspx?PeopleId={0}" DataNavigateUrlFields="IdStr" DataTextField="Name" />

                            <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                   <asp:HyperLink runat="server" CssClass="frmLink" NavigateUrl='<%#String.Format("mailto:{0}", Eval("Email"))%>'><%#SOS.UI.Utils.SetFormString((String)Eval("Email"))%></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete recipient from transmittal?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upDrawings" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="cmdSave" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="repDrawings" EventName="ItemCommand" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Drawings</td>
                        </tr>
                    </table>
                </td>

            </tr>
            <tr>
                <td>        
                    <table cellpadding="4" cellspacing="1">
                        <tr>
                            <td class="lstHeader">Drawing</td>
                            <td class="lstHeader">Description</td>
                            <td class="lstHeader">Revision</td>
                            <td class="lstHeader">Copies</td>
                            <td class="lstHeader">Date</td>
                            <td class="lstHeader">File</td>
                            <td class="lstHeader">&nbsp;</td>
                        </tr>            

                <asp:Repeater ID="repDrawings" runat="server" OnItemCommand="repDrawings_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td class="lstItem"><asp:HyperLink runat="server" CssClass="frmLink" NavigateUrl='<%#"~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + Eval("Revision.Drawing.IdStr")%>'><%#SOS.UI.Utils.SetFormString((String)Eval("DrawingName"))%></asp:HyperLink></td>
                            <td class="lstItem"><%#SOS.UI.Utils.SetFormString((String)Eval("DrawingDescription"))%></td>
                            <td class="lstItem" align="center"><asp:HyperLink runat="server" CssClass="frmLink" NavigateUrl='<%#"~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + Eval("Revision.IdStr")%>'><%#SOS.UI.Utils.SetFormString((String)Eval("RevisionName"))%></asp:HyperLink></td>
                            <td class="lstItem" align="center"><%#SOS.UI.Utils.SetFormInteger((Int32)Eval("NumCopies"))%></td>
                            <td class="lstItem" align="center"><%#SOS.UI.Utils.SetFormDate((DateTime)Eval("RevisionDate"))%></td>
                            <td class="lstItem"><sos:FileLabel DrawingRevision='<%#(SOS.Core.DrawingRevisionInfo)Eval("Revision")%>' runat="server"></sos:FileLabel></td>
                            <td class="lstItem" align="center"><asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="javascript:return confirm('Delete drawing from transmittal?');" CommandName="Delete" CommandArgument='<%#Eval("Revision.IdStr")%>'><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td class="lstAltItem"><asp:HyperLink runat="server" CssClass="frmLink" NavigateUrl='<%#"~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + Eval("Revision.Drawing.IdStr")%>'><%#SOS.UI.Utils.SetFormString((String)Eval("DrawingName"))%></asp:HyperLink></td>
                            <td class="lstAltItem"><%#SOS.UI.Utils.SetFormString((String)Eval("DrawingDescription"))%></td>
                            <td class="lstAltItem" align="center"><asp:HyperLink runat="server" CssClass="frmLink" NavigateUrl='<%#"~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + Eval("Revision.IdStr")%>'><%#SOS.UI.Utils.SetFormString((String)Eval("RevisionName"))%></asp:HyperLink></td>
                            <td class="lstAltItem" align="center"><%#SOS.UI.Utils.SetFormInteger((Int32?)Eval("NumCopies"))%></td>
                            <td class="lstAltItem" align="center"><%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("RevisionDate"))%></td>
                            <td class="lstAltItem"><sos:FileLabel DrawingRevision='<%#(SOS.Core.DrawingRevisionInfo)Eval("Revision")%>' runat="server"></sos:FileLabel></td>
                            <td class="lstAltItem" align="center"><asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="javascript:return confirm('Delete drawing from transmittal?');" CommandName="Delete" CommandArgument='<%#Eval("Revision.IdStr")%>'><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>

                        <tr>
                            <td colspan="3" class="lstBlankItem"><asp:DropDownList ID="ddlDrawings" runat="server" Visible="false"></asp:DropDownList></td>
                            <td class="lstBlankItem"><asp:TextBox ID="txtCopies" runat="server" Width="24" Visible="false"></asp:TextBox></td>
                            <td class="lstBlankItem"><asp:Button ID="cmdSave" runat="server" Text="Add" Visible="false" OnClick="cmdSaveDrawing_Click"/></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>

<br />
</asp:Panel>
<br />

</asp:Content>
