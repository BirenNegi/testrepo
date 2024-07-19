<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewAddendumPage" Title="Trade Addendum" Codebehind="ViewAddendum.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Trade Addendum" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td><asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton></td>
                    <td>&nbsp;&nbsp;</td>
                    <td><asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton></td>
                </tr>
            </table>            
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Date:</td>
                    <td class="frmData"><asp:Label ID="lblAddendumDate" runat="server"></asp:Label></td>
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

<asp:UpdatePanel ID="upAttachments" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="butAddAttachment" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Attachments</td>
                            
                            <asp:PlaceHolder ID="phAddAttachment" runat="server" Visible="false">
                            <td>&nbsp;</td>
                            <td><sos:FileSelect ID="sfsAttachment" runat="server" /></td>
                            <td><asp:Button ID="butAddAttachment" runat="server" OnClick="butAddAttachment_Click" Text="Add" /></td>
                            </asp:PlaceHolder>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView 
                        ID="gvAttachments" 
                        runat="server"
                        DataKeyNames="Id"
                        OnRowDeleting="gvAttachments_RowDeleting"
                        AutoGenerateColumns="False" 
                        CellPadding="2" 
                        CellSpacing="0" 
                        CssClass="lstList" 
                        RowStyle-CssClass="lstItem" 
                        AlternatingRowStyle-CssClass="lstAltItem">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete Attachment from Addendum?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="File" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" NavigateUrl='<%#(String)Eval("LinkURL")%>' Text='<%#(String)Eval("LinkText")%>' CssClass='<%#(String)Eval("LinkCSSClass")%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="FileSizeInfo" HeaderText="Size" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="FileDateInfo" HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="AttachDateInfo" HeaderText="Attached" HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
