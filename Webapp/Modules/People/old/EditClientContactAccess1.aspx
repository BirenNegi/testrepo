<%@ Page Language="C#"  MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditClientContactAccess"Title="Edit Client Contact's Access" CodeBehind="EditClientContactAccess.aspx.cs"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <sos:TitleBar ID="TitleBar" runat="server" />

    <script type="text/javascript">

        window.onload = HideShowColmns;

         function HideShowColmns()
         {
             

            rows = document.getElementById("MainContent_GVDistributionList").rows;
            btn = document.getElementById("MainContent_GVDistributionList_ImgBtn");
                       
            for (i = 0; i < rows.length; i++)
            {
                
                if (rows[i].cells[2].style.display === 'none')
                {
                    btn.setAttribute("src", "/Images/IconRightExpand.jpg");
                    rows[i].cells[2].style.display = "";
                    rows[i].cells[3].style.display = "";
                    rows[i].cells[4].style.display = "";
                    rows[i].cells[5].style.display = "";
                    rows[i].cells[6].style.display = "";
                    rows[i].cells[7].style.display = "";
                    rows[i].cells[9].style.display = "";
                }
                else {
                btn.setAttribute("src", "/Images/IconRightCollaps.jpg");
                rows[i].cells[2].style.display = "none";
                rows[i].cells[3].style.display = "none";
                rows[i].cells[4].style.display = "none";
                rows[i].cells[5].style.display = "none";
                rows[i].cells[6].style.display = "none";
                rows[i].cells[7].style.display = "none";
                rows[i].cells[9].style.display = "none";
                }
                
            }

            var myparmeter=location.search.split('PeopleId=')[1]
            if (myparmeter)
            {
                 var tab = document.getElementById("AddClientsContact");
                 tab.style.display = "";
                 var del = document.getElementById("MainContent_ImgDelete");
                 var edit = document.getElementById("MainContent_lnkAddClient");
                 //alert(del.style.display);
                 del.style.display = "";
                 edit.style.display = "none";//src = "~/Images/Iconedit.gif";
            }
        }


        //To allow to select only one chkbox of type Attention 
         function CheckOne(obj,K) {
           
             var grid = obj.parentNode.parentNode.parentNode;
             //var inputs = grid.getElementsByTagName("input");
             var gridRow = obj.parentNode.parentNode;
             //for (var i = 0; i < inputs.length; i++) {

             
             for (var i = 0; i < grid.rows.length; i++) {
                 inputs = grid.rows[i].cells[K].getElementsByTagName("input");
                 if (inputs.length > 0) {
                     
                         if (inputs[0].type == "checkbox") {
                            // alert(obj.checked);
                                       if (obj.checked && inputs[0]!= obj && inputs[0].checked) {
                                         inputs[0].checked = false;
                                     }
                         }
                 }
             }
         }

         function DisplayAddClients(S) {
             
             if (S) {
                 var tab = document.getElementById("AddClientsContact");
                 tab.style.display = "";
                 //alert('1');
             }
         }

    </script>

   



<table cellspacing="0" cellpadding="0">
     <asp:PlaceHolder ID="phAddNew" runat="server">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td>
                        <asp:ImageButton ID="lnkAddClient" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New" OnClientClick="DisplayAddClients(true)"></asp:ImageButton>
                        <%--<asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/Images/IconDelete.gif" ToolTip="Delete Contact" style="display:none"  OnClientClick="DisplayAddClients(true)"></asp:ImageButton>--%>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr><td>

    <table id="AddClientsContact" style="display:none;">

            <tr>
                <td class="frmTopBox">
                    <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
                    <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
                </td>
            </tr>
            <tr>
                <td class="frmForm">
                    <table width="100%">
                        <tr>
                            <td class="frmReqLabel">First&nbsp;Name:</td>
                            <td class="frmText">
                                <asp:RequiredFieldValidator ID="valFirstName" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtFirstName" runat="server" Width="150"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td class="frmLabel">Address:</td>
                            <td class="frmText"><asp:TextBox ID="txtStreet" runat="server" Width="150"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="frmReqLabel">Last&nbsp;Name:</td>
                            <td class="frmText">
                                <asp:RequiredFieldValidator ID="valLastName" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtLastName" runat="server" Width="150"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                            <td class="frmLabel">Suburb:</td>
                            <td class="frmText"><asp:TextBox ID="txtLocality" runat="server" Width="150"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="frmLabel">Phone:</td>
                            <td class="frmText"><asp:TextBox ID="txtPhone" runat="server" Width="150"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td class="frmLabel">State:</td>
                            <td class="frmText"><asp:DropDownList ID="ddlState" runat="server" Width="152"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="frmLabel">Mobile Phone:</td>
                            <td class="frmText"><asp:TextBox ID="txtMobile" runat="server" Width="150"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td class="frmLabel">Postal Code:</td>
                            <td class="frmText"><asp:TextBox ID="txtPostalCode" runat="server" Width="150"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="frmLabel">Fax:</td>
                            <td class="frmText"><asp:TextBox ID="txtFax" runat="server" Width="150"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <td class="frmLabel">Company:</td>
                            <td class="frmText">
                                <asp:TextBox ID="txtCompanyName" Width="150" runat="server"  ></asp:TextBox>
                                <%--<input id="txtCompanyId" type="hidden" runat="server" />
                                <asp:HyperLink ID="cmdSelCompany" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="frmReqLabel">Email Address:</td>
                            <td class="frmText"><asp:TextBox ID="txtEmail" runat="server" Width="150"></asp:TextBox></td>
                            <td>&nbsp;</td>
                            <%--San--%>
                            <td class="frmLabel">Position</td>
                            <td class="frmText">
                                <asp:DropDownList ID="DpdPosition" runat="server" Width="152">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>Contact</asp:ListItem>
                                    <asp:ListItem>Superintendent</asp:ListItem>
                                    <asp:ListItem>Surveyor</asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtPosition" runat="server" Width="15"></asp:TextBox>--%>

                            </td>
                            <%--San--%>
                        </tr>
                        <tr>
                            <td class="frmLabel">Inactive:</td>
                            <td class="frmText"><asp:CheckBox ID="chkInactive" runat="server" /></td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
        <td class="frmBottomBox">
            <asp:linkbutton id="cmdUpdateBottom" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    </table>

    </td></tr>
</table>



    <br />
    <br />
<table cellspacing="0" cellpadding="0" >

        <tr>
            <td class="lstTitle" style="text-align:right; padding-right:10px; background-color:gainsboro">Documents Distribution List</td>
            <td>&nbsp; &nbsp;</td>
            <td class="lstTitle" style="text-align:right; padding-right:80px;background-color:gainsboro; display:none">Website Access</td>

        </tr>

        <tr>
            <%--Distributionlist--%>
                <td class="frmBottomBox">
                    <asp:GridView ID="GVDistributionList" runat="server" DataSourceID="SqlDataSource1" 
                        AutoGenerateColumns="False" DataKeyNames="PeopleId" AllowPaging = "True"
                        PageSize = "25"
                        PagerStyle-CssClass = "lstPager"
                        CellPadding = "4"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem" OnRowCommand="GVDistributionList_RowCommand" OnRowDataBound="GVDistributionList_RowDataBound">
        <AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                        <Columns>

                            <asp:TemplateField >
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgEdit" runat="server" ImageUrl="~/Images/IconEdit.gif" OnClientClick="DisplayAddClients(true)"  CommandName="onGridEditClick" 
                                     CommandArgument  = '<%# DataBinder.Eval(Container,"DataItem.PeopleId")%>' CausesValidation="False" ToolTip="Edit docs" />
                                 </ItemTemplate>
                                 <HeaderStyle HorizontalAlign="Left"  Width="1%" />
                             </asp:TemplateField>
                    
                    

                            <asp:BoundField DataField="PeopleId" HeaderText="PeopleId" ReadOnly="True" SortExpression="PeopleId" HeaderStyle-CssClass="lstHeader" Visible="false">
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:BoundField>


                            <asp:TemplateField HeaderStyle-CssClass="lstHeader">
                                <HeaderTemplate> Name <asp:ImageButton ID="ImgBtn"   runat="server" OnClientClick="HideShowColmns();" ImageUrl="~/Images/IconRightExpand.jpg" />
                                </HeaderTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="LblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>  
                                    <asp:Label ID="HiddenPeopleID" runat="server"  Text='<%# Bind("PeopleId") %>' style="display:none;" /></ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>


                   
                            <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" HeaderStyle-CssClass="lstHeader" >
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" HeaderStyle-CssClass="lstHeader" >
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" HeaderStyle-CssClass="lstHeader" >
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" HeaderStyle-CssClass="lstHeader" >
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-CssClass="lstHeader" >
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmployeePosition" HeaderText="EmployeePosition" SortExpression="EmployeePosition" HeaderStyle-CssClass="lstHeader" >
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserLogin" HeaderText="UserLogin" SortExpression="UserLogin" HeaderStyle-CssClass="lstHeader" >
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserLastLogin" HeaderText="UserLastLogin" SortExpression="UserLastLogin" HeaderStyle-CssClass="lstHeader" >
                    
        <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:BoundField>
                    
                            <asp:TemplateField HeaderText="EOTs">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkEot" runat="server"  Checked='<%# Eval("DistEOTs") %>'/>
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="RFIs" >
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkRFI" runat="server" Checked='<%# Eval("DistRFIs") %>'/>
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Claims">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkClaims" runat="server" Checked='<%# Eval("DistClaims") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>



                            <asp:TemplateField HeaderText="SAs">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSA" runat="server" Checked='<%# Eval("DistSaparateAccounts") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CVs">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkCV" runat="server" Checked='<%# Eval("DistClientVariations") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="EOTs&lt;br/&gt; Attention">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkATEOT" runat="server" Checked='<%# Eval("AttentionEOT") %>' onclick="CheckOne(this,15)" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="RFIs&lt;br/&gt; Attention ">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkATRFI" runat="server" Checked='<%# Eval("AttentionRFI") %>' onclick="CheckOne(this,16)" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Claims&lt;br/&gt; Attention ">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkATClaims" runat="server" Checked='<%# Eval("AttentionClaim") %>' onclick="CheckOne(this,17)" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Manage Website&lt;br/&gt; Access Account" HeaderStyle-CssClass="lstHeader" Visible="False">
                                  <ItemTemplate>
                                     <asp:Button ID="btnCreateAccount" runat="server" 
                                        CommandArgument  = '<%# DataBinder.Eval(Container,"DataItem.UserLogin") + ";" +Eval("PeopleId")%>' CausesValidation="False" ToolTip="Account"  
                                        CommandName="ManageAccount" 
                                            Text="" />
                                  </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                             </asp:TemplateField>

                        </Columns>

        <PagerStyle CssClass="lstPager"></PagerStyle>

        <RowStyle CssClass="lstItem"></RowStyle>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>">
                    </asp:SqlDataSource>
                    <br />
                    <asp:Button ID="BtnDistUpdate" runat="server" Text="Update Distribution List" OnClick="BtnDistUpdate_Click" CausesValidation="false"/>
                </td>
            <td></td>
            <%-- Website access --%>
                <td class="frmBottomBox" style="vertical-align:top">
              <asp:GridView ID="GVWebsiteAccess" runat="server" DataSourceID="SqlDataSource2" 
                AutoGenerateColumns="False" DataKeyNames="PeopleId" AllowPaging = "True"
                PageSize = "25"
                PagerStyle-CssClass = "lstPager"
                CellPadding = "4"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem" OnRowCommand="GVGVWebsiteAccess_RowCommand">
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>

                   
                    
                    

                    <asp:BoundField DataField="PeopleId" HeaderText="PeopleId" InsertVisible="False" ReadOnly="True" SortExpression="PeopleId" HeaderStyle-CssClass="lstHeader" Visible="false">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>


                    <asp:TemplateField HeaderStyle-CssClass="lstHeader">
                        <HeaderTemplate> Name 
                        </HeaderTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="LblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label> 
                            <asp:Label ID="HiddenPeopleID" runat="server"  Text='<%# Bind("PeopleId") %>' style="display:none;" /> 
                        </ItemTemplate>
                    <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>


                   
                    <asp:BoundField DataField="UserLogin" HeaderText="UserLogin" SortExpression="UserLogin" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="EOTs">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkEot" runat="server" Checked='<%# Eval("WebEots") %>'/>
                        </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="RFIs">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkRFI" runat="server" Checked='<%# Eval("WebRFIs") %>' />
                        </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Claims">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkClaims" runat="server"  Checked='<%# Eval("WebClaims") %>'/>
                        </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="SAs">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkSA" runat="server" Checked='<%# Eval("WebSeparateAccounts") %>' />
                        </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="CVs">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkCV" runat="server" Checked='<%# Eval("WebClientVariations") %>' />
                        </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Docs">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkDocs" runat="server" Checked='<%# Eval("WebDocs") %>' />
                        </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Photos">
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkPhotos" runat="server" Checked='<%# Eval("WebPhotos") %>' />
                        </ItemTemplate><HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>

                </Columns>

<PagerStyle CssClass="lstPager"></PagerStyle>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>
              <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>">
              </asp:SqlDataSource>
                    <br />
                    <asp:Button ID="BtnWebAccessUpdate" runat="server" Text="Update Website Access" CausesValidation="false" OnClick="BtnWebAccessUpdate_Click" />
        </td>
       </tr>


  </table>
    


</asp:Content>
