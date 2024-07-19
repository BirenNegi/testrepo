<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ViewMeetingMinutes" Title="View Meeting Minutes" Codebehind="ViewMeetingMinutes.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<sos:TitleBar ID="TitleBar1" runat="server" Title="RFI" />

<asp:Panel ID="pnlErrors" runat="server" Visible="false">
<asp:TreeView ID="TreeViewMissingFields" runat="server" ShowLines="true" Visible="false">
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="frmText" />
    </LevelStyles>
</asp:TreeView>
<br />
</asp:Panel>

<table cellspacing="0" cellpadding="0">
	<tr>
	    <td>
	        <table>
	            <tr>
		            <asp:Panel ID="pnlEdit" runat="server" Visible="false">
		            <td>
			            &nbsp;
			            <asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image ID="Image1" runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
		            </td>
		            </asp:Panel>
	            
                    <td>&nbsp;&nbsp;</td>
                    <%--<td><sos:FileLink ID="sflReferenceFile" runat="server" FileTitle="Reference File" /></td>
        
                    <td>&nbsp;&nbsp;</td>
                    <td><sos:FileLink ID="sflClientResponseFile" runat="server" FileTitle="Client Response File" /></td>

                    <td>&nbsp;&nbsp;</td>
                    <td>
                        <asp:HyperLink ID="lnkRFI" runat="server" CssClass="frmLink" Visible="false">RFI</asp:HyperLink>
                        <asp:Label ID="lblRFI" runat="server" CssClass="frmTextDis" Visible="false">RFI</asp:Label>
                    </td>
                    
                    <asp:PlaceHolder ID="phSendEmail" runat="server" Visible="false">
                        <td>&nbsp;&nbsp;</td>
                        <td>
                            <asp:LinkButton ID="cmdSendEmail" runat="server" OnClick="cmdSendEmail_Click" OnClientClick="javascript:return confirm('Send RFI ?');" CssClass="frmLink" Visible="false">Send by email</asp:LinkButton>
                            <asp:Label ID="lblSendEmail" runat="server" CssClass="frmTextDis" Visible="false">Send by email</asp:Label>
                        </td>                        
                    </asp:PlaceHolder>--%>
	            </tr>
	        </table>
	    </td>
	</tr>
	<tr>
		<td class="frmForm">
			<table width="100%">
				<tr>
					<td class="frmLabel">Meeting Number:</td>
					<td class="frmData">
                        <%--<asp:Label ID="lblNumber" runat="server"></asp:Label>--%>
                        <asp:Label ID="lblTypleNumber" runat="server"></asp:Label>
					</td></tr>
                <tr>
                    <td class="frmLabel">Meeting Date :</td>
					<td class="frmData"><asp:Label ID="lblRaiseDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Meeting Time:</td>
					<td class="frmData"><asp:Label ID="lblTime" runat="server"></asp:Label></td>

				</tr>

                <tr>
                    <td class="frmLabel">Subject/Type:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblSubject" runat="server"></asp:Label></td>
                </tr>

                <tr>
                    <td class="frmLabel">Location:</td>
					<td class="frmData"><asp:Label ID="lblLocation" runat="server"></asp:Label></td>
				</tr>
				
                <tr>
                    <td class="frmLabel">Reference File:</td>
                    <td class="frmData" ><asp:Label ID="lblReferenceFileName" runat="server"></asp:Label></td>                            
                </tr>
               
                <tr>
                    <td class="frmLabel">Attendees:</td>
                    <td class="frmData" ><asp:Label ID="lblAttendees" runat="server"></asp:Label></td>                            
                </tr>
                <%--<tr>
                    <td class="frmLabel" valign="top">Response Summary:</td>
                    <td colspan="4"><asp:TextBox ID="txtClientResponseSummary" runat="server" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Rows="4" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Description:</td>
                    <td colspan="4"><asp:TextBox ID="txtDescription" runat="server" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Rows="16" Width="640"></asp:TextBox></td>
                </tr>--%>
            </table>
		</td>
	</tr>
</table>
<br />

</asp:Content>

