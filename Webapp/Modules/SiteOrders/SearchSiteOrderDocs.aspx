<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.SearchSiteOrderDocs" Title="Orders" Codebehind="SearchSiteOrderDocs.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
                                               <script type="text/javascript">
                                                   function ValidateSave() {
                                                       var t = document.getElementById('<%=txtDocName.ClientID%>').value;
                                                     if (t == "") {
                                                         alert("Document Title is mandatory");
                                                         return false;
                                                     }
                                                 }
                                                 function ValidateExit() {
                                                     var t = document.getElementById('<%=txtDocName.ClientID%>').value;
                                                       if (t != "") {
                                                           if (confirm("Are you sure you want to exit without saving your document!", "") != true) { return false; }
                                                       }
                                                   }
                                               </script>   

    <sos:TitleBar ID="TitleBar1" runat="server" Title="Orders" />

<table cellspacing="1" cellpadding="1">
    <tr>
        <td class="auto-styleMSG">
            <table cellspacing="1" cellpadding="1" class="auto-style6" >
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                     <td>
                        &nbsp;</td>
                   <td class="auto-style7">
<asp:LinkButton ID="cmdUpdateTop" runat="server" CssClass="frmButton" UseSubmitBehavior="true"  OnClientClick="return ValidateSave()"  OnClick="cmdAddNew_Click">Save & Exit</asp:LinkButton>
&nbsp;&nbsp;
<asp:LinkButton ID="cmdSave" runat="server" CssClass="frmButton" UseSubmitBehavior="true"  OnClientClick="return ValidateSave()"  OnClick="cmdSave_Click" >Save</asp:LinkButton>

&nbsp;&nbsp;
<asp:LinkButton ID="cmdCancelTop" runat="server" CausesValidation="False" CssClass="frmButton" UseSubmitBehavior="true"  OnClientClick="return ValidateExit()"  OnClick="cmdCancel_Click">Exit</asp:LinkButton>
&nbsp;&nbsp;                   </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    </tr>
                <tr>
                    <%--<asp:ControlParameter Name="strType" ControlID="ddlBusinessUnit" />--%><%--<asp:ControlParameter Name="strType" ControlID="ddlBusinessUnit" />--%><%--<asp:ControlParameter Name="strType" ControlID="ddlBusinessUnit" />--%><%--<asp:ControlParameter Name="strType" ControlID="ddlBusinessUnit" />--%>
                    <td class="auto-styleMAND">
                        <asp:Label ID="lblMessageBrowse" runat="server">Select Document</asp:Label>
                    </td>
                    <td class="auto-style10">
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="239px" />
                    </td>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style3">
                        &nbsp;&nbsp;<asp:TextBox ID="txtSearch" runat="server" Visible="False" Width="16px"></asp:TextBox>
                    </td>
                    <td class="frmText">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                 
                <tr>
                    <td class="auto-styleMAND">
                        Document Title</td>
                    <td class="auto-style10">
                        <asp:TextBox ID="txtDocName" runat="server" Width="225px"></asp:TextBox>
 <%--                       <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" OnClick="cmdAddNew_Click"  />--%>
                        </td>
                    <td class="auto-style5">
                        &nbsp;</td>
                    <td class="auto-style3">
                        &nbsp;</td>
                    <td class="frmText">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                 
                <tr>
                    <td class="auto-style9">
                        &nbsp;</td>
                    <td class="auto-style10">
                        &nbsp;</td>
                    <td class="auto-style5">&nbsp;</td>
                    <td class="auto-style3">
                        &nbsp;</td>
                    <td class="frmText">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                 <tr>
                      <td>
                        &nbsp;</td>
                  <td class="auto-style7">
<asp:LinkButton ID="LinkButton1" runat="server" CssClass="frmButton" UseSubmitBehavior="true"  OnClientClick="return ValidateSave()"  OnClick="cmdAddNew_Click">Save & Exit</asp:LinkButton>
&nbsp;&nbsp;
<asp:LinkButton ID="LinkButton2" runat="server" CssClass="frmButton" UseSubmitBehavior="true"  OnClientClick="return ValidateSave()"  OnClick="cmdSave_Click" >Save</asp:LinkButton>

&nbsp;&nbsp;
<asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CssClass="frmButton" UseSubmitBehavior="true"  OnClientClick="return ValidateExit()"  OnClick="cmdCancel_Click">Exit</asp:LinkButton>
&nbsp;&nbsp;                   </td>


 </tr>
            
            </table>
        </td>
    </tr>
    <tr>
        <td class="auto-style1">            
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
                    <asp:BoundField DataField="DocTitle" SortExpression="DocTitle" HeaderText="Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>


<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DocName" Visible="False" HeaderText="Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"> </asp:BoundField>
                    <asp:BoundField DataField="DocDate" HeaderText="Date" DataFormatString="{0:d}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"> </asp:BoundField>
                    <asp:BoundField DataField="LastName" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">  </asp:BoundField>
<%--                   <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:LinkButton DataField="ShowDoc" ImageUrl="~/Images/IconDocument.gif" ToolTip="Open" runat="server" OnClick="cmdShowDoc_Click" ></asp:LinkButton>
                        </ItemTemplate>
                    <ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>--%>
                
                <asp:TemplateField ItemStyle-VerticalAlign="Top">
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="true" CommandArgument='<%#String.Format("{0};{1}", Eval("Id"), Eval("DocName")) %>' onclick="cmdShowDoc_Click">
                             <asp:Image ID="imgDocument" runat="server" ImageUrl="~/Images/IconDocument.gif" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

               <asp:TemplateField>
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    <ItemTemplate>
                       
                         <asp:LinkButton ID="lnkDelete" runat="server" UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to delete the item?');" CommandArgument='<%#String.Format("{0};{1}", Eval("Id"), Eval("DocName")) %>' onclick="cmdDeleteDoc_Click">
                          <asp:Image ID="imgdelete" runat="server" ImageUrl="~/Images/IconDelete.gif" />
                         </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

<%--                   <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" ToolTip="Open" runat="server" OnClick="cmdShowDoc_Click" ></asp:HyperLink>
                        </ItemTemplate>
                    <ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>--%>

                </Columns>

<PagerStyle CssClass="lstPager"></PagerStyle>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>    
        </td>
    </tr>
</table>

</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            width: 700px;
        }
         .auto-styleMSG {
            color: #0010FF;
            width: 700px;
        }
        .auto-style3 {
            width: 190px;
        }
        .auto-style5 {
            width: 11px;
        }
        .auto-styleLH1 {
            background-color: #A00000;
            color: #FFFFFF;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 73px;
        }
        .auto-style6 {
            width: 700px;
        }
                .auto-styleBTN {
            padding-bottom: 3px;
            text-align: center;
            width: 700px;
        }
        .auto-style7 {
            padding-bottom: 3px;
            text-align: center;
            width: 219px;
        }
        .auto-style9 {
            width: 219px;
        }
        .auto-style10 {
            width: 289px;
        }
        .auto-styleMAND{
            background-color: #A00000;
            color: #FFFFFF;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: left;
            width: 112px;
        }
    </style>
</asp:Content>


