<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewClaimPage" Title="Claim" Codebehind="ViewClaim.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<sos:TitleBar ID="TitleBar1" runat="server" Title="Claim" />

<table>
    <tr>
        <td class="lstTitle">General Information</td>
    </tr>
    <tr>
		<td>
			<table cellspacing="0" cellpadding="0">
				<asp:Panel ID="pnlEdit" runat="server" Visible="false">
				<tr>
					<td>
						&nbsp;
						<asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
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
								<td class="frmLabel">Write Date:</td>
								<td class="frmData"><asp:Label ID="lblWriteDate" runat="server"></asp:Label></td>
							</tr>
							<tr>
								<td class="frmLabel">Draft Approval Date:</td>
								<td class="frmData"><asp:Label ID="lblDraftApprovalDate" runat="server"></asp:Label></td>
							</tr>
							<tr>
								<td class="frmLabel">Internal Approval Date:</td>
								<td class="frmData"><asp:Label ID="lblInternalApprovalDate" runat="server"></asp:Label></td>
							</tr>
							<tr>
								<td class="frmLabel">Client Approval Date:</td>
								<td class="frmData"><asp:Label ID="lblApprovalDate" runat="server"></asp:Label></td>
							</tr>
							<tr>
								<td class="frmLabel">Issue Date:</td>
								<td class="frmData"><asp:Label ID="lblDueDate" runat="server"></asp:Label></td>
							</tr>
							<tr>
								<td class="frmLabel">Client Due Date:</td>
								<td class="frmData"><asp:Label ID="lblClientDueDate" runat="server"></asp:Label></td>
							</tr>               
							<tr>
								<td class="frmLabel">Adjustment Note Number(s):</td>
								<td class="frmData"><asp:Label ID="lblAdjustmentNoteName" runat="server"></asp:Label></td>
							</tr>               
							<tr>
								<td class="frmLabel">Adjustment Note Amount:</td>
								<td class="frmData"><asp:Label ID="lblAdjustmentNoteAmount" runat="server"></asp:Label></td>
							</tr>   
							<tr>
                                <td class="frmLabel">Client Backup File 1:</td>
                                <td class="frmText"><sos:FileSelect ID="sfsBackupFile1" runat="server" /></td>
			                    <td> <asp:ImageButton ID="btnSave1" runat="server" ImageUrl="~/Images/IconDisk16.png" ToolTip="Save to Claim" OnClick="cmdSaveDoc_Click" /> </td>
                            </tr>
							<tr>
                                <td class="frmLabel">Client Backup File 2:</td>
                                <td class="frmText"><sos:FileSelect ID="sfsBackupFile2" runat="server" /></td>
			                    <td> <asp:ImageButton ID="btnSave2" runat="server" ImageUrl="~/Images/IconDisk16.png" ToolTip="Save to Claim" OnClick="cmdSaveDoc_Click" /> </td>
                            </tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
    </tr>
</table>
<br />

<asp:Panel ID="pnlErrors" runat="server" Visible="false">
<asp:TreeView ID="TreeViewMissingFields" runat="server" ShowLines="true" Visible="false">
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="frmText" />
    </LevelStyles>
</asp:TreeView>
<br />
</asp:Panel>

<asp:Panel ID="pnlClientVariations" runat="server" Visible="false">
<table>
    <tr>
        <td class="lstSubTitle">Client Variations not fully approved</td>
    </tr>
    <tr>
        <td>
            <asp:GridView
                ID = "gvClientVariations"
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
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
<br />
</asp:Panel>

<table>
	<tr>
		<td>
			<table>
				<tr>
					<td class="lstTitle">Approval Process Manager</td>

					<asp:PlaceHolder ID="phViewClaim" runat="server" Visible="false">
						<td>&nbsp;&nbsp;</td>
						<td><asp:HyperLink ID="lnkViewClaim" runat="server" CssClass="frmLink">Claim</asp:HyperLink></td>
					</asp:PlaceHolder>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                     <td><sos:FileLink ID="sflBackupFile1" runat="server" FileTitle="Backup File 1" /></td>


                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                     <td><sos:FileLink ID="sflBackupFile2" runat="server" FileTitle="Backup File 2" /></td>

				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td><sos:ProcessManager ID="ProcessManagerClaim" runat="Server"></sos:ProcessManager></td>
	</tr>

    <%-- San --%>

    <tr><td>&nbsp;&nbsp; &nbsp;</td></tr>

    <tr>
        <asp:PlaceHolder ID="PlFinalPaymentComments" runat="server" Visible="false">
		<td class="lstTitle">Final Payment Comments 
            <br/> <asp:TextBox ID="TxtFPComments" runat="server" TextMode="MultiLine" Width="450px" Height="60px" ></asp:TextBox></td>
        </asp:PlaceHolder>
	</tr>


    <%-- San --%>
</table>
<br />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td colspan="3" class="lstTitle">Trades and Variations</td>
    </tr>
    <tr>
        <td class="frmForm"><table id="tblClaim" runat="server" cellpadding="2" cellspacing="1"></table></td>
    </tr>
</table>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    </asp:Content>

