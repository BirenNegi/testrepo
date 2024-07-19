<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewSiteOrderPageHire" Title="Project" Codebehind="ViewSiteOrderHire.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="View Site Order " />



<table>
    <tr>
        <td><asp:Image ID="imgGeneralInfo" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">General Information</td>
    </tr>
</table>

<asp:Panel ID="pnlGeneralInfo" runat="server" CssClass="collapsePanel" Height="0">
<table cellpadding="0" cellspacing="0">
    <tr>
       <td colspan="4">
           <asp:Label ID="lblMessage" runat="server" Text=" " Visible="false" ForeColor="DarkRed"> </asp:Label>
      </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="1" cellspacing="0">
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton></td>
                    <td>&nbsp;</td>
                    <td><asp:LinkButton ID="cmdPrintOrder" runat="server" Visible="true" ToolTip="Print Site Order" OnClick="cmdPrintOrder_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconDocument.gif" /></asp:LinkButton></td>
                    <td>&nbsp;</td>
                   <td>
                    <asp:LinkButton ID="cmdEmail" runat="server" UseSubmitBehavior="true" ToolTip="Email Site Order" OnClientClick="return confirm('Please confirm sending of e-mail - Status will be marked as Sent?');" onclick="cmdEmailOrder_Click">
                        <asp:Image ID="imgdelete" runat="server" ImageUrl="~/Images/IconEmail.gif" />
                    </asp:LinkButton>
                    </td>
                    <td>
                    <asp:LinkButton ID="cmdSendOrder" runat="server" ToolTip="Email Site Order" CssClass="frmButton" OnClientClick="return confirm('Please confirm sending of e-mail - Status will be marked as Sent?');" OnClick="cmdEmailOrder_Click">Send Order</asp:LinkButton>
                    </td>
                    <td><asp:LinkButton ID="cmdCancel" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdCancel_Click">Exit</asp:LinkButton> </td>
                    <%--</td>                 </tr>--%>
              <%--      <td>&nbsp;</td>
 <td><asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton></td> --%>
           </table>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
           <table class="auto-style39">
                <tr>
                    <td colspan="4" class="frmSubSubTitle">General Information</td>
                </tr>
                   <tr>
                        <td class="auto-styleLHL">Title:</td>
                        <td colspan="2" class="auto-styleTXT">
                            <asp:TextBox ID="txtTitle" ReadOnly="True" runat="server" BackColor="#DDDDDD" Width="400px"></asp:TextBox>
                       </td>
                         <td colspan="2" class="auto-styleTXT">
                            <asp:DropDownList ID="ddlCodeType" runat="server" ReadOnly="True" BackColor="#DDDDDD" AutoPostBack="false" Width="150px"> </asp:DropDownList>
                            <asp:TextBox ID="txtTypeInfo" ReadOnly="True" runat="server" BackColor="#DDDDDD" Width="200px"></asp:TextBox>
                       </td>
                        <%--    <td>&nbsp;</td>/>--%>
                    </tr>
                <tr>
                    <td class="auto-styleLHL">Type:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtTyp_Desc" runat="server" Height="18px" ReadOnly="True" Width="176px" BackColor="#DDDDDD"></asp:TextBox>
                    </td>
                    <td class="auto-styleMid">&nbsp;</td>
                    <td class="auto-styleLHL">Date:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtOrderDateShow" runat="server" ReadOnly="True" Width="150px" BackColor="#DDDDDD"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-styleLHL">Provider:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtSubContractorName" runat="server" ReadOnly="True" Width="300px" BackColor="#DDDDDD"></asp:TextBox>
                    </td>
                    <td class="auto-styleMid">&nbsp;</td>
                    <td class="auto-styleLHL">Foreman:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtPeopleForeman" runat="server" ReadOnly="True" Width="300px" BackColor="#DDDDDD"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-styleLH1">ABN:</td>
                    <td class="auto-style41">
                        <asp:TextBox ID="txtABN" runat="server" Height="18px" ReadOnly="True" Width="176px" BackColor="#DDDDDD"></asp:TextBox>
                    </td>
                    <td class="auto-style42"></td>
                        <td class="auto-styleLHL">Given By:</td>
                        <td class="auto-style42">
                            <asp:TextBox ID="txtGivenByName" runat="server" Width="300" BackColor="#DDDDDD" ReadOnly="True"></asp:TextBox>
                             <input id="txtGivenBy" type="hidden" runat="server" />
                        </td>
                </tr>
                <tr>
                    <td class="auto-styleLHL">&nbsp;</td>
                    <td class="auto-styleTXT">
                        &nbsp;</td>
                    <td class="auto-styleMid"></td>
                    <td class="auto-styleLHL">Date Start:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtOrderDateStartShow" runat="server" BackColor="#DDDDDD" ReadOnly="True" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-styleLHL">Site Address:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtSubContractorAddr" runat="server" Height="16px" ReadOnly="True" Width="300px" BackColor="#DDDDDD"></asp:TextBox>
                    </td>
                    <td class="auto-styleMid">&nbsp;</td>
                    <td class="auto-styleLHL">Date End:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtOrderDateEndShow" runat="server" BackColor="#DDDDDD" ReadOnly="True" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-styleLHL">Contact:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtContact" runat="server" BackColor="#DDDDDD" ReadOnly="True" Width="300px"></asp:TextBox>
                    </td>
                    <td class="auto-styleMid">&nbsp;</td>
                    <td class="auto-styleLHL">OrderItems:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtItemsTotal" runat="server" BackColor="#DDDDDD" ReadOnly="True" style="text-align: right" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-styleLHL">e-mail:</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtEmail" runat="server" BackColor="#DDDDDD" Height="16px" ReadOnly="True" Width="300px"> </asp:TextBox>
                    </td>
                    <td class="auto-styleMid"></td>
                    <td class="auto-styleLHL">Budget Code:</td>
                    <td class="auto-styleTXT">
                             <asp:DropDownList ID="ddTradesCode" runat="server"  ReadOnly="True" BackColor="#DDDDDD" AutoPostBack="false" Width="300px"> </asp:DropDownList>
                            <input id="txtTradesCode" type="hidden" runat="server" />
                   </td>
                </tr>
                <tr>
                    <td class="auto-styleLHL">&nbsp;</td>
                    <td class="auto-styleTXT">&nbsp;</td>
                    <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLHL">SUB-TOTAL :</td>
                        <td class="auto-styleTXT">
                         <asp:TextBox ID="txtSubTotal" runat="server" style="text-align: right" Width="150" BackColor="#DDDDDD" ReadOnly="True" ></asp:TextBox>
                       </td>
                </tr>

                <tr>
                    <td class="auto-styleLHL overflow-wrap: break-word;">Description (Visible Externally):</td>
                    <td class="auto-styleTXT">
                        <asp:TextBox ID="txtNotes" runat="server" BackColor="#DDDDDD" ReadOnly="True" Width="300px" Height="60" TextMode="MultiLine" Rows="5"> </asp:TextBox>
                    </td>
                        <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLHL">Comments (Visible Internally):</td>
                        <td class="auto-styleTXT">
                        <asp:TextBox ID="txtComments" runat="server"  ReadOnly="True" BackColor="#DDDDDD" Width="300px" Height="60" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            &nbsp;</td>
                </tr>

                <%-- San --%><%-- San --%><%-- San New role and processSteps for COM  and JCA--%><%---------- San ------------%>
            </table>
        </td>

    </tr>
</table>
</asp:Panel>
<br />
<table>
    <tr>
        <td><asp:Image ID="imgItems" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Items</td>
        <td>
           <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" OnClick="cmdAddNew_Click"  />
        </td>
    </tr>
</table>

<asp:Panel ID="pnlItems" runat="server" CssClass="collapsePanel" Height="0">
<table>
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1">
                <tr>
 <%--<td><asp:LinkButton ID="lnkEditItems" runat="server" Visible="True" OnClick="cmdEditItems_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton></td>                   --%>

                     <%-- <td class="lstTitle">Site Order Items</td> --%>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
     
            <asp:GridView
                ID = "gvSiteOrderItems"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                   <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
<asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/SiteOrders/EditSiteOrderDetailHire.aspx?ProjectId={0}&OrderId={1}&ItemId={2}", Request.QueryString["ProjectId"].ToString(),Request.QueryString["OrderId"].ToString(), Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Title" HeaderText="Item" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Qty" HeaderText="Quantity" DataFormatString="{0:N2}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                    <asp:BoundField DataField="UM" HeaderText="U/M" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="Price" HeaderText="Unit Price" DataFormatString="{0:C2}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C2}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>
<br />
</asp:Panel>

<br />
<table>
    <tr>
        <td><asp:Image ID="imgDocs" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Documents</td>
         <td>
           <asp:ImageButton ID="btnShowDocs" runat="server" ImageUrl="~/Images/IconDocument.gif" ToolTip="Download Order + ALL documents for this order" OnClick="cmdShowDocs_Click"  />
        </td>    </tr>
</table>
    <asp:Panel ID="pnlDocs" runat="server" CssClass="collapsePanel" Height="0">
 
<table>
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1">
                <tr>
 <td><asp:LinkButton ID="btnEditDoc" runat="server" ToolTip="Goto Document Upload/Delete Screen" Visible="True" OnClick="cmdEditDoc_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton></td>                   
                 </tr>
            </table>
        </td>
    </tr> 
    <tr>
        <td>
     
           <asp:GridView 
                ID = "gvSiteOrderDocs"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <PagerSettings mode="NumericFirstLast" firstpagetext="First" lastpagetext="Last" pagebuttoncount="10" position="Bottom" />
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>

                     <asp:BoundField DataField="DocName" Visible="False" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"> </asp:BoundField>
                     <asp:BoundField DataField="DocTitle" HeaderText="Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"> </asp:BoundField>
                    <asp:BoundField DataField="DocDate" HeaderText="Date" HeaderStyle-CssClass="lstHeader" DataFormatString="{0:d}" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"> </asp:BoundField>
                    <asp:BoundField DataField="LastName" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">  </asp:BoundField>

                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="true" CommandArgument='<%#String.Format("{0};{1}", Eval("Id"), Eval("DocName")) %>' onclick="cmdShowDoc_Click">
                             <asp:Image ID="imgDocument" runat="server" ImageUrl="~/Images/IconDocument.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

<PagerStyle CssClass="lstPager"></PagerStyle>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>    
        </td>
    </tr>
</table>
<br />
</asp:Panel>
<table>
    <tr>
        <td><asp:Image ID="imgApprovals" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Approvals</td>
    </tr>
</table>


<asp:Panel ID="pnlApprovals" runat="server" CssClass="collapsePanel" Height="0">

<table>
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1">
                <tr>
                <td><asp:LinkButton ID="btnApprovals"  ToolTip="Goto approvals update screen" runat="server" Visible="True" OnClick="cmdEditApproval_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton></td>                     <%-- <td class="lstTitle">Site Order Items</td> --%>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
     
            <asp:GridView
                ID = "gvSiteOrderApprovals"
                OnRowDataBound = "gvSiteOrderApprovals_RowDataBound"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CellSpacing = "0"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                AlternatingRowStyle-CssClass = "lstAltItem">
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Step" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="AssignedDate" HeaderText="Assigned Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="StatusDescription" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                      <asp:templatefield>
                   <headerstyle backcolor="Navy" forecolor="White"/>
                   <itemtemplate>
                     <asp:CheckBox ID="Approve" HeaderText="Ok" runat="server" ShowHeader="true" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"/>
                   </itemtemplate>
                   <headertemplate>
                     <asp:Image ImageUrl="~/Images/iconConfirmed.gif" runat="server" />
                   </headertemplate> 
                 </asp:templatefield>                     
 
                 <asp:templatefield>
                   <headerstyle backcolor="Navy" forecolor="White"/>
                   <itemtemplate>
                     <asp:CheckBox ID="Reject" HeaderText="Ok" runat="server" ShowHeader="true" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"/>
                   </itemtemplate>
                   <headertemplate>
                     <asp:Image ImageUrl="~/Images/iconDelete.gif" runat="server" />
                   </headertemplate> 
                 </asp:templatefield>     
                 <asp:BoundField DataField="isApprovalCurrent" HeaderText="Allow Edit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:BoundField DataField="AssignedPeopleId" HeaderText="Id" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:BoundField DataField="OrderId" HeaderText="OrderId" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>

                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>
<br />
</asp:Panel>


<act:CollapsiblePanelExtender
    ID="cpeApprovals" 
    runat="Server"
    Collapsed="False"
    CollapsedSize="0"
    TargetControlID="pnlApprovals"
    ExpandControlID="imgApprovals"
    CollapseControlID="imgApprovals"
    ImageControlID="imgApprovals"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender
    ID="CollapsiblePanelExtender1" 
    runat="Server"
    Collapsed="False"
    CollapsedSize="0"
    TargetControlID="pnlItems"
    ExpandControlID="imgItems"
    CollapseControlID="imgItems"
    ImageControlID="imgItems"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender
    ID="cpe2" 
    runat="Server"
    Collapsed="False"
    CollapsedSize="0"
    TargetControlID="pnlDocs"
    ExpandControlID="imgDocs"
    CollapseControlID="imgDocs"
    ImageControlID="imgDocs"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>
    <act:CollapsiblePanelExtender
    ID="cpe" 
    runat="Server"
    Collapsed="False"
    CollapsedSize="0"
    TargetControlID="pnlGeneralInfo"
    ExpandControlID="imgGeneralInfo"
    CollapseControlID="imgGeneralInfo"
    ImageControlID="imgGeneralInfo"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-styleLH1 {
            
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 73px;
        }
        .auto-styleTXT {
            width: 300px;
            background-color: #EEEEEE;
        }

        .auto-styleMid {
            width: 3px;
        }
        
        .auto-styleLHL {
            background-color: #DDDDDD;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 90px;
        }

        .auto-styleLHN {
           
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 73px;
        }
        .auto-style39 {
            width: 900px;
        }
        
        </style>
</asp:Content>

