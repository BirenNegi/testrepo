<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ViewRFIPage" Title="RFI" Codebehind="ViewRFI.aspx.cs"  EnableEventValidation="false"%>
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


    <style>
body {
    font-family: "Lato", sans-serif;
}

        .SelectedRow {
            border-right-color:red;
            background-color:#e8e8e8;
        }


.sidenav {
    height: 100%;
    width: 0;
    position: fixed;
    z-index: 1;
    top: 0;
    left: 0;
    background-color: #111;
    overflow-x: hidden;
    transition: 0.5s;
    padding-top: 60px;
}

.sidenav a {
    padding: 8px 8px 8px 32px;
    text-decoration: none;
    font-size: 25px;
    color: #818181;
    display: block;
    transition: 0.3s;
}

.sidenav a:hover, .offcanvas a:focus{
    color: #f1f1f1;
}

.sidenav .closebtn {
    position: absolute;
    top: 0;
    right: 25px;
    font-size: 36px;
    margin-left: 50px;
}

.hide {
    display:none;
 }

.HlRef {
color: #0000FF;
text-decoration: underline;
font-weight: normal;
font-family: Verdana, Arial;
        }


#main {
    transition: margin-left .5s;
    padding: 16px;
}

@media screen and (max-height: 450px) {
  .sidenav {padding-top: 15px;}
  .sidenav a {font-size: 18px;}
}
</style>



  <script type="text/javascript">
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("main").style.marginLeft = "250px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("main").style.marginLeft= "0";
}
</script>




<div id="mySidenav" class="sidenav">
  <a href="javascript:void(0)" class="closebtn" onclick="closeNav()">&times;</a>
  <a href="#">About</a>
  <a href="#">Services</a>
  <a href="#">Clients</a>
  <a href="#">Contact</a>
</div>


 

    <table cellspacing="0" cellpadding="0">
	<tr>
	    <td colspan="2">
	        <table>
	            <tr>
		            <asp:Panel ID="pnlEdit" runat="server" Visible="false">
		            <td>
			            &nbsp;
			            <asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image ID="Image1" runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
		            </td>
		            </asp:Panel>
	            
                    <td>&nbsp;&nbsp;</td>
                    <td><%--<sos:FileLink ID="sflReferenceFile" runat="server" FileTitle="Reference File" />--%>
                        <asp:HyperLink ID="HlReferenceFile" runat="server" CssClass="HlRef">Reference File</asp:HyperLink>
                    </td>
        
                   <%-- <td>&nbsp;&nbsp;</td>
                    <td><sos:FileLink ID="sflClientResponseFile" runat="server" FileTitle="Client Response File" /></td>--%>

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
                    </asp:PlaceHolder>
	            </tr>
	        </table>
	    </td>
	</tr>
	<tr>
            <td style="vertical-align:top" Width="220px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="200px" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField>
                            
                            <HeaderTemplate>
                                <asp:Label ID="LblSubject" runat="server"></asp:Label>
                            </HeaderTemplate>
                            
                            <ItemTemplate>
                                <asp:Label ID="LblBy" runat="server" Text='<%# Eval("From") %>' CssClass="frmSubSubTitle"></asp:Label>
                                <br />
                                
                                <asp:Label ID="LblDate" runat="server" Text='<%# Eval("RaiseDate") %>' CssClass="frmTextSmall"></asp:Label>
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ResponseId" HeaderText="ResponseId" SortExpression="ResponseId" >
                        <HeaderStyle CssClass="hide" />
                        <ItemStyle CssClass="hide" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RFIId" HeaderText="RFIId" SortExpression="RFIId">
                        <HeaderStyle CssClass="hide" />
                        <ItemStyle CssClass="hide" />
                        </asp:BoundField>
                    </Columns>
                    <SelectedRowStyle BackColor="#CCCCCC" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString2 %>" >

                </asp:SqlDataSource>
            </td>
             
		    <td  >
			    <table width="100%" class="frmForm" style="padding-left:5px;" runat="server">
				<tr>
					<td class="frmLabel">NumbNumber:</td>
					<td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date Raised:</td>
					<td class="frmData"><asp:Label ID="lblRaiseDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Created by:</td>
					<td class="frmData"><asp:Label ID="lblSigner" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Answer by date:</td>
					<td class="frmData"><asp:Label ID="lblTargetAnswerDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Status:</td>
					<td class="frmData"><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Date Answered:
					<td class="frmData"><asp:Label ID="lblActualAnswerDate" runat="server"></asp:Label></td>
				</tr>
                <%--<tr>
                    <td class="frmLabel">Reference File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblReferenceFileName" runat="server"></asp:Label></td>                            
                </tr>--%>
                <tr>
                    <td class="frmLabel">Subject:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblSubject" runat="server"></asp:Label></td>
                </tr>
               <%-- <tr>
                    <td class="frmLabel">Response File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblClientResponseFile" runat="server"></asp:Label></td>                            
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Response Summary:</td>
                    <td colspan="4"><asp:TextBox ID="txtClientResponseSummary" runat="server" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Rows="4" Width="640"></asp:TextBox></td>
                </tr>--%>
                <tr id="attachmentrow" runat="server">
                    <td class="frmLabel" valign="top">
                        <br />
                        Attachments:</td>
                    <td colspan="4" id="AttachmentCell" runat="server" class="frmData"></td>
                </tr>

                <tr>
                    <td class="frmLabel" valign="top">Message:</td>
                    <td colspan="4"><asp:TextBox ID="txtDescription" runat="server" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Rows="16" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="4" style="text-align:right;padding-right:50px;" >
     <asp:Button ID="BtnReply" runat="server" Text="Reply" OnClick="BtnReply_Click" />
                    </td></tr>
            </table>
		    </td>
    
	</tr>

</table>
    <br />





</asp:Content>

