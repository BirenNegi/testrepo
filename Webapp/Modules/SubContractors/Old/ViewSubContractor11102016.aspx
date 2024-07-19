<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewSubContractorPage" Title="Subcontractor" Codebehind="ViewSubContractor.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="Subcontractor" />

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
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Fax:</td>
                    <td class="frmData"><asp:Label ID="lblFax" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Business Unit:</td>
                    <td class="frmData"><asp:Label ID="lblBusiniessUnit" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Address:</td>
                    <td class="frmData"><asp:Label ID="lblStreet" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Short Name:</td>
                    <td class="frmData"><asp:Label ID="lblShortName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td class="frmData"><asp:Label ID="lblLocality" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">ABN:</td>
                    <td class="frmData"><asp:Label ID="lblAbn" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">State:</td>
                    <td class="frmData"><asp:Label ID="lblState" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Account:</td>
                    <td class="frmData"><asp:Label ID="lblAccount" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td class="frmData"><asp:Label ID="lblPostalCode" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Phone:</td>
                    <td class="frmData"><asp:Label ID="lblPhone" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Website:</td>
                    <td class="frmData"><asp:HyperLink ID="lnkWebsite" runat="server" CssClass="frmLink"></asp:HyperLink></td>
                </tr>
                <%-- San --%>
                <tr>
                    <td class="frmLabel">Prequalification Form:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblPrequalification" runat="server"></asp:Label></td>
                </tr> 
                <%-- San --%>

                <tr>
                    <td class="frmLabel">Comments:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblComments" runat="server"></asp:Label></td>
                </tr>                
            </table>
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td class="lstTitle">Contacts</td>
    </tr>
    <tr>
        <td>
            <asp:GridView
                ID = "GridViewContacts"
                runat = "server"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                AutoGenerateColumns = "False"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/People/ViewContact.aspx?peopleId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Phone" HeaderText="Phone" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Mobile" HeaderText="Mobile" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkEmail" runat="server" NavigateUrl='<%#"mailto:"+Eval("Email")%>' Text='<%#Eval("Email")%>' CssClass="lstLink"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Log in account" HeaderStyle-CssClass="lstHeader" ItemStyle-CssClass="lstLink" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <sos:BooleanViewer ID="sbv" runat="server" Checked='<%#Eval("HasAccount")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                </Columns>
            </asp:GridView>    
        </td>
    </tr>    
</table>

</asp:Content>

