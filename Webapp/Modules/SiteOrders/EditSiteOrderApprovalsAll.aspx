<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditSiteOrderApprovalsAll" Title="Orders" Codebehind="EditSiteOrderApprovalsAll.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <sos:TitleBar ID="TitleBar1" runat="server" Title="Site Order Approvals" />

    <asp:ObjectDataSource ID="odsSiteOrderApprovals" runat="server" EnablePaging="true" OnObjectCreating="odsSiteOrderApprovals_Selecting" SelectCountMethod="SearchSiteOrderAprovalsCount" SelectMethod="SearchSiteOrderApprovals" SortParameterName="orderBy" TypeName="SOS.Core.SiteOrdersController">
        <SelectParameters>
       <%--   <asp:ControlParameter ControlID="txtSearch" Name="strSearch" />--%>
        <%--<asp:ControlParameter Name="strType" ControlID="ddlBusinessUnit" />--%>
        </SelectParameters>
    </asp:ObjectDataSource>



<table>
    <tr>
    <td>
        <asp:Label ID="lblMessage" ForeColor="Red" runat="server" ></asp:Label>
   </td>
    </tr>
    <tr>
        <td>
                
 
        
                  <asp:LinkButton ID="cmdCancelTop" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdCancel_Click">Exit</asp:LinkButton>
        
        
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
 
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                           <asp:LinkButton ID="lnkViewOrder" runat="server" CausesValidation="false" CommandArgument='<%#String.Format("{0};{1};{2}", Eval("Id"), Eval("ProjectId"), Eval("OrderId"))%>' onclick="cmdViewOrder_Click">
                              <asp:Image ID="imgdelete" runat="server" ImageUrl="~/Images/IconView.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProjectName" HeaderText="Project" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="OrderTitle" HeaderText="Order" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Step" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
<%--                    <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>--%>
                    <asp:BoundField DataField="AssignedDate" HeaderText="Assigned Date" DataFormatString="{0:d}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" DataFormatString="{0:d}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="StatusDescription" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
<%--                    <asp:CheckBoxField DataField="Approve" HeaderText="Approve" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:CheckBoxField>
                    <asp:CheckBoxField DataField="Reject" HeaderText="Reject" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:CheckBoxField>--%>

                 <asp:BoundField DataField="isApprovalCurrent" HeaderText="Allow Edit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:BoundField DataField="AssignedPeopleId" HeaderText="Id" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:BoundField DataField="OrderId" HeaderText="OrderId" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"  DataFormatString="{0:'D'000000}"  ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:BoundField DataField="OrderTyp" HeaderText="Typ" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                 <asp:BoundField DataField="SubTotal" HeaderText="Order Total" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                 <asp:BoundField DataField="ValueLimit" HeaderText="Lim" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                 <asp:templatefield>
                   <headerstyle backcolor="Navy" forecolor="White"/>
                   <itemtemplate>
                     <asp:CheckBox ID="Approve" runat="server" OnClick="return ValidateAccept(this);" OnCheckedChanged="chkApproveChanged"  /> 
                       <script type="text/javascript">
                           function ValidateAccept(lnk) {
                               var row = lnk.parentNode.parentNode;
                               var ApproveTitle = row.cells[3].innerHTML;
                               //var Limit = row.cells[10].innerHTML;
                               var Limit1 = row.cells[11].innerHTML;
                               if (ApproveTitle == "Off Hire") {
                                   if (confirm("By clicking this all equipment has been returned?  Click OK to proceed", "") != true) { lnk.checked == false; return false; }
                               }
                               else {
                                   //Confirm(ApproveTitle, "");
                                   if (Limit1 == "Z")
                                   {
                                       if (confirm("This order has ZERO value. Please Confirm Approval?  Click OK to proceed", "") != true) { lnk.checked == false; return false; }
                                   }
                                   else
                                   {
                                       if (Limit1 == "L")
                                       {
                                          if (confirm("This order is above $3000. Have you got PM approval?  Click OK to proceed", "") != true) { lnk.checked == false; return false; }
                                       }
                                       else
                                       {
                                          if (confirm("Please Confirm Approval?", "") != true) { lnk.checked == false; return false; }
                                       }
                                   }
                               }
                               __doPostBack('chkApproveChanged', '');
                           }
                       </script>     
                  </itemtemplate>
                   <headertemplate>
                     <asp:Image ImageUrl="~/Images/iconConfirmed.gif" runat="server" />
                   </headertemplate> 
                 </asp:templatefield>                     
 
                 <asp:templatefield>
                   <headerstyle backcolor="Navy" forecolor="White"/>
                   <itemtemplate>
                 <%-- //AutoPostBack="True"   <asp:CheckBox ID="Reject" HeaderText="Ok" runat="server" UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to Reject the item?');" ShowHeader="true" AutoPostBack="true" OnCheckedChanged="chkRejectChanged" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"/> --%>
                 <%--      <asp:CheckBox ID="Reject" HeaderText="Ok" runat="server" UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to Reject the item?');" ShowHeader="true" onclick="chkRejectChanged" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"/> --%>
                       <asp:CheckBox ID="Reject" runat="server" OnClick="return ValidateReject(this);" OnCheckedChanged="chkRejectChanged"  />  
                       <script type="text/javascript">
                           function ValidateReject(lnk) {
                               var row = lnk.parentNode.parentNode;
                               var rowIndexs = row.rowIndex - 1;
                               var ri = parseInt(rowIndexs);
                               var vv = row.cells[8].innerHTML;
                               var chkState = "";
                               if (lnk.checked == true) {
                                   let reason = prompt("Are you sure you want to reject this item?", "");
                                   if (reason != null) {
                                       if (reason == "") {
                                           lnk.checked == false;
                                           return false;
                                       }
                                       row.cells[8].innerHTML = reason;
                                       __doPostBack('chkRejectChanged', reason);
                                   }
                                   else {
                                       lnk.checked == false;
                                       return false;
                                   }
                               } else {
                                   if (confirm("Are you sure you want to CANCEL rejection of this item?") == true) {
                                       row.cells[8].innerHTML = "";

                                       __doPostBack('chkRejectChanged', '');
                                   }
                                   else {
                                       lnk.checked == true;
                                       // row.cells[8].innerHTML = "nothing done";
                                       return false;
                                   }

                               }

                           }
                       </script>     

                   </itemtemplate>

                   <headertemplate>
                     <asp:Image ImageUrl="~/Images/iconDelete.gif" runat="server" />
                   </headertemplate> 
                 </asp:templatefield>                       
                  <asp:BoundField DataField="Reason" HeaderText="Reject Reason" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                  <asp:BoundField DataField="OrderDateEnd" HeaderText="End Date" DataFormatString="{0:d}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                  <asp:BoundField DataField="RowColor" HeaderText="Colour" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                  <asp:TemplateField ItemStyle-VerticalAlign="Top">
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="true" ToolTip="Download Order + ALL documents for this order" CommandArgument='<%#String.Format("{0}", Eval("OrderId")) %>' onclick="cmdPrintOrder_Click">
                             <asp:Image ID="imgDocument" runat="server" ImageUrl="~/Images/IconDocument.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <asp:BoundField DataField="DocCount" HeaderText="Docs" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                  <asp:BoundField DataField="BusinessUnitName" HeaderText="BU" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>

           </Columns>

            </asp:GridView>    
        </td>
    </tr>
</table>

</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
         .auto-styleBTN {
            padding-bottom: 3px;
            text-align: center;
            width: 506px;
        }
         .auto-style1 {
            width: 362px;
        }
        .auto-style3 {
            width: 190px;
        }
        .auto-style4 {
            width: 131px;
        }
        .auto-style5 {
            width: 11px;
        }
        .auto-style6 {
            width: 506px;
        }
    </style>
</asp:Content>


