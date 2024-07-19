<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewDrawingPage" Title="Drawing" Codebehind="ViewDrawing.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Drawing" />

<asp:Panel ID="pnlProposal" runat="server">
<table cellpadding="0" cellspacing="0">
    <asp:Panel ID="pnlEdit" runat="server" Visible="false">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
    </asp:Panel>
    
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel">Type:</td>
                    <td class="frmData"><asp:Label ID="lblType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server" Width="150"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Description:</td>
                    <td class="frmData"><asp:Label ID="lblDescription" runat="server"></asp:Label></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td>
            <table>
                <tr>
                    <td class="lstTitle">Revisions</td>

                    <asp:PlaceHolder ID="phAddRevision" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkAddRevision" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                    </asp:PlaceHolder>
                </tr>
            </table>
        </td>

    </tr>

    <tr>
        <td>
            <asp:GridView
                ID = "gvRevisions"
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
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Number" HeaderText="Revision" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblRevisionDate" runat="server" Text='<%#SOS.UI.Utils.SetFormDate((System.DateTime?)Eval("RevisionDate"))%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="File" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <sos:FileLabel ID="sfrRevisionFile" runat="server" DrawingRevision=<%#((SOS.Core.DrawingRevisionInfo)(Container.DataItem))%> ></sos:FileLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td class="lstTitle">Transmittals</td>
    </tr>
    <tr>
        <td>
            <asp:GridView
                ID = "gvTransmittals"
                runat = "server"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewTransmittal.aspx?TransmittalId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Number" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormString((System.String)Eval("TransmittalNumberStr"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="FirstRevisionNumber" HeaderText="Revision" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="SubContractorShortName" HeaderText="Subcontractor" HeaderStyle-CssClass="lstHeader"></asp:BoundField>
                    <asp:BoundField DataField="ContactName" HeaderText="Contact" HeaderStyle-CssClass="lstHeader"></asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="lstHeader">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormString(SOS.Web.Utils.GetConfigListItemNameAndOther("Transmittals", "TransmittalType", (System.String)Eval("TransmittalType"), (System.String)Eval("TransmittalTypeOther")))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Transmitted" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((System.DateTime?)Eval("TransmissionDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Sent" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((System.DateTime?)Eval("SentDate"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td valign="top">
            <asp:TreeView ID="tvTrades" runat="server" ShowLines="true" RootNodeStyle-CssClass="lstTitle" LeafNodeStyle-CssClass="lstLinkBig">
                <Nodes>
                    <asp:TreeNode Expanded="True" SelectAction="None" Text="Trades"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </td>
    </tr>
</table>
<br />
</asp:Panel>
<br />

</asp:Content>
