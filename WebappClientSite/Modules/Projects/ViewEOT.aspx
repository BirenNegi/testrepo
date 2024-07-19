<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ViewEOTPage" Title="EOT" Codebehind="ViewEOT.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <sos:TitleBar ID="TitleBar1" runat="server" Title="EOT" />

<asp:Panel ID="pnlErrors" runat="server" Visible="false">
<asp:TreeView ID="TreeViewMissingFields" runat="server" ShowLines="true" Visible="false">
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="frmSubTitle" />
        <asp:TreeNodeStyle CssClass="frmText" />
    </LevelStyles>
</asp:TreeView>
<br />
</asp:Panel>

    <%-- San --%>
    <script language="javascript" type="text/javascript">

        function EOTNOD()
        {
            var x = '';
            var lblClaim = document.getElementById('MainContent_lblDaysClaimed');
            var lblapproved = document.getElementById('MainContent_lblDaysApproved')
            if (lblapproved.textContent.length > 0) {
               x= 'Send EOT ?';
            }
            else { x = 'Send NOD ?'; }
            //alert(x);
            return confirm(x);
            
        }
    </script>


    <%-- San --%>


<table cellspacing="0" cellpadding="0">
	<tr>
	    <td>
	        <table>
	            <tr>
		            <asp:Panel ID="pnlEdit" runat="server" Visible="false">
		            <td>
			            &nbsp;
			            <asp:LinkButton ID="cmdEditTop" runat="server" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
		            </td>
                    <td>&nbsp;&nbsp;</td>
		            </asp:Panel>
	            
                    <td>&nbsp;&nbsp;</td>
                    <td><sos:FileLink ID="sflClientApprovalFile" runat="server" FileTitle="Approval File" /></td>

                    <td>&nbsp;&nbsp;</td>
                    <td>
                        <asp:HyperLink ID="lnkEOT" runat="server" CssClass="frmLink" Visible="false">CEOT</asp:HyperLink>
                        <asp:Label ID="lblEOT" runat="server" CssClass="frmTextDis" Visible="false">CEOT</asp:Label>
                    </td>
                    
                    <asp:PlaceHolder ID="phSendEmail" runat="server" Visible="false">
                        <td>&nbsp;&nbsp;</td>
                        <td>
                            <%--<asp:LinkButton ID="cmdSendEmail" runat="server" OnClick="cmdSendEmail_Click" OnClientClick="javascript:return confirm('Send EOT ?');" CssClass="frmLink" Visible="false">Send by email</asp:LinkButton>--%>
                            <asp:LinkButton ID="cmdSendEmail" runat="server" OnClick="cmdSendEmail_Click" OnClientClick="javascript:return EOTNOD();" CssClass="frmLink" Visible="false">Send by email</asp:LinkButton>
                            
                            
                            <asp:Label ID="lblSendEmail" runat="server" CssClass="frmTextDis" Visible="false">Send by email</asp:Label>
                            </td>
                        <td>

                            

                            <%-- San------------------------ --%>
                            
<asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" RepeatDirection="Horizontal">
                            <ItemTemplate>
                                                               
                                 <asp:HyperLink ID="HyperLink1" runat="server" CssClass="frmLink" NavigateUrl='<%# "~/Modules/Projects/ShowEOT.aspx?EOTId="+Eval("EOTID")+"&NODID="+Eval("NODID").ToString() %>' Text='<%# Eval("Result") %>'></asp:HyperLink>
                                 &nbsp;
                            </ItemTemplate>
                        </asp:DataList>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=VC-SQL-01;Initial Catalog=SOSTest;User ID=SOSApp;Password=!Pr0j3ctS0S" ProviderName="System.Data.SqlClient" SelectCommand="SELECT Result + convert(varchar(5),slno)as Result,[EOTID],[NODID] from  (SELECT Row_Number() over(order by Nodid)as Slno, [EOTID],[NODID],'Result'=case when DaysApproved is Null then'NOD' Else 'EOT' End  FROM [NODs_EOTs] WHERE ([EOTId] = @EOTId))A">
        <SelectParameters>
            <asp:QueryStringParameter Name="EOTId" QueryStringField="EOTId" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

 <%-- San------------------------ --%>


                        </td>
                    </asp:PlaceHolder>
	            </tr>
	        </table>
	    </td>
	</tr>
	<tr>
		<td class="frmForm">
			<table>
				<tr>
					<td class="frmLabel">Number:</td>
					<td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Start date:</td>
					<td class="frmData"><asp:Label ID="lblStartDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Days claimed:</td>
					<td class="frmData"><asp:Label ID="lblDaysClaimed" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">End date:</td>
					<td class="frmData"><asp:Label ID="lblEndDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Days approved:</td>
					<td class="frmData"><asp:Label ID="lblDaysApproved" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">First notice date:</td>
					<td class="frmData"><asp:Label ID="lblFirstNoticeDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Cost:</td>
					<td class="frmData"><asp:Label ID="lblCostCode" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Written notice date:</td>
					<td class="frmData"><asp:Label ID="lblWriteDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">&nbsp;</td>
					<td class="frmData">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Send Date:</td>
					<td class="frmData"><asp:Label ID="lblSendDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">&nbsp;</td>
					<td class="frmData">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Approval date:</td>
					<td class="frmData"><asp:Label ID="lblApprovalDate" runat="server"></asp:Label></td>
				</tr>
			</table>
            <table>
                <tr>
                    <td class="frmLabel">Approval File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblClientApprovalFile" runat="server"></asp:Label></td>                            
                </tr>
                <tr>
                    <td class="frmLabel">Cause of delay:</td>
                    <td class="frmData"><asp:Label ID="lblCause" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Nature of delay:</td>
                    <td class="frmData"><asp:TextBox ID="txtNature" runat="server" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Rows="8" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Period of delay:</td>
                    <td><asp:TextBox ID="txtPeriod" runat="server" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Rows="8" Width="640"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Works affected:</td>
                    <td><asp:TextBox ID="txtWorks" runat="server" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Rows="4" Width="640"></asp:TextBox></td>
                </tr>
            </table>
		</td>
	</tr>
</table>


<br />

</asp:Content>
