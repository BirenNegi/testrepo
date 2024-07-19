<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="EditSubcontractorQualifications.aspx.cs" Inherits="SOS.Web.EditSubcontractorQualifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />


 <sos:TitleBar ID="TitleBar1" runat="server" Title="Subcontractor Operators Certificates/Qualification" />

    <table cellpadding="0" cellspacing="0">
        <tr><td><br /></td></tr>
        <tr><td></td></tr>
        <tr class="frmForm" >
            <td >

      <asp:GridView ID="gvQualifications" runat="server" 
          AlternatingRowStyle-CssClass="lstAltItem" AutoGenerateColumns="False" CellPadding="4"  CssClass="lstList" 
          DataKeyNames="Id" OnRowEditing="gvQualifications_RowEditing"
          RowStyle-CssClass="lstItem" ShowFooter="True" ShowHeaderWhenEmpty="True" OnRowCancelingEdit="gvQualifications_RowCanceling" OnRowCommand="gvQualifications_RowCommand" OnRowDeleting="gvQualifications_RowDeleting" OnRowUpdating="gvQualifications_RowUpdating">
          <AlternatingRowStyle CssClass="lstAltItem" />
          <Columns>
                           
             <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <FooterTemplate>
                                                <asp:Button ID="BtnAddQualifications" runat="server" CausesValidation="true" CommandName="AddQualification" Font-Bold="True" Font-Underline="True" Text="Add" ValidationGroup="NewRecord" />
                                            </FooterTemplate>

                                                     <%-- NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProjectTradeItemCategory.aspx?TradeItemCategoryId={0}", Eval("Id"))%>'--%>
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ImageUrl="~/Images/IconView.gif" NavigateUrl='' ToolTip="Open"></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>


                                          <asp:TemplateField HeaderText="Certificates/Qualifications">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtUpdateQualificationName" runat="server" Text='<%# Bind("qualificationName") %>' Width="90%"></asp:TextBox>
                                                   
                                                 <asp:RequiredFieldValidator ID="UpdateRequiredFieldValidator1" runat="server" ControlToValidate="TxtUpdateQualificationName" ErrorMessage="Please enter valid Qualification Name" ForeColor="Red" ValidationGroup="UpdateRecord"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNewQualificationName" runat="server"></asp:TextBox>
                                                <br />
                                               <asp:RequiredFieldValidator ID="AddRequiredFieldValidator1" runat="server" ControlToValidate="TxtNewQualificationName" ErrorMessage="Please enter valid Qualification Name" ForeColor="Red" ValidationGroup="NewRecord"></asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("qualificationName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="False" />
                                        </asp:TemplateField>


             <asp:TemplateField HeaderText="Licence/Card Number ">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtUpdatecardNumber" runat="server" Text='<%# Bind("cardNumber") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNewcardNumber" runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("cardNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="False" />
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="ExpiryDate">
                                            <EditItemTemplate>
                                                  <asp:TextBox ID="TxtUpdateexpiryDate" runat="server" Text='<%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("expiryDate")) %>' Placeholder="dd/mm/yyyy" ></asp:TextBox>
                                                   <br /> <asp:CompareValidator id="dateValidator" runat="server"    Type="Date" ForeColor="Red"  Operator="DataTypeCheck"  ControlToValidate="TxtUpdateexpiryDate" 
                                                    ErrorMessage="Please enter a valid date.">   </asp:CompareValidator>
                                            
                                            </EditItemTemplate>




                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNewexpiryDate" runat="server" TextMode="Date" ></asp:TextBox> <br />
                                                <asp:CompareValidator id="dateValidator1" runat="server"    Type="Date" ForeColor="Red"  Operator="DataTypeCheck"  ControlToValidate="TxtNewexpiryDate"  
                                                    ErrorMessage="Please enter a valid date.">  </asp:CompareValidator>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                   <asp:Label ID="Label3" runat="server" Text='<%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("expiryDate")) %>'></asp:Label>
                                                 
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="True" />
                                        </asp:TemplateField>


             <asp:TemplateField HeaderText="File" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                 <asp:LinkButton ID="lblImageName" style="color:blue;" runat="server" Text='<%# Bind("imageName") %>' CommandName="DownloadFile"  CommandArgument='<%# Eval("Id") %>' Font-Underline="True"></asp:LinkButton>
                                                 
                                            </ItemTemplate>

                                             <EditItemTemplate>

                                                 <%--<input id="Fileloader" type="file" runat="server" />--%>
                                                  <asp:FileUpload ID="Fileloader" runat="server" visible='<%# Convert.ToBoolean(Eval("imageName")!=null ? (Eval("imageName").ToString().Length == 0 ? true:false):true) %>' />
                                                  <asp:Label ID="lblUploadImageName" runat="server" Text='<%# Bind("imageName") %>'  visible='<%# Convert.ToBoolean(Eval("imageName")!=null ? (Eval("imageName").ToString().Length >0 ? true:false):false) %>'></asp:Label>
                                                  &nbsp;
                                                  <asp:Button ID="btnRemove" runat="server" Text="Remove" CommandName="RemoveFile"  CommandArgument='<%# Eval("Id") %>' 
                                                      OnClientClick="return confirm('Please confirm to remove the file.');"
                                                       style="color: -webkit-link; cursor: pointer; text-decoration: underline;border-collapse: collapse;border-spacing: 2px; background-color:none; 
                                                              font-family: Verdana, Arial; font-size: 8pt; color:blue; white-space: nowrap;"
                                                       visible='<%# Convert.ToBoolean(Eval("imageName")!=null ? (Eval("imageName").ToString().Length >0 ? true:false):false) %>' BackColor="Transparent" BorderColor="Transparent" BorderStyle="None" />
                                             </EditItemTemplate>

                                            <FooterTemplate>
                                                 <asp:FileUpload ID="FileloaderFooter" runat="server"/>
                                            </FooterTemplate>


                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>

             <asp:CommandField ShowEditButton="True" >  
                <HeaderStyle CssClass="lstHeader" />
               
              <ItemStyle VerticalAlign="Top" />
               
            </asp:CommandField>


             <asp:TemplateField HeaderText="Delete">
                                            <FooterTemplate>
                                                <asp:Button ID="BtnAddCategories" runat="server" CausesValidation="true" CommandName="AddQualification" Font-Bold="True" Font-Underline="True" Text="Add" ValidationGroup="NewRecord" />
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="cmdDeleteTop" runat="server"  Visible="true">
                                        <asp:ImageButton  ID="Image2" runat="server" OnClientClick="return confirm('Please confirm to delete this Qualification?');" 
                                            CommandName="Delete" CommandArgument='<%# Eval("Id") %>'  
                                            AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" />

                                    </asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="True" />
                                        </asp:TemplateField>

              
          </Columns>

           <FooterStyle BackColor="#F0F0F0" VerticalAlign="Top" />
                                    <RowStyle CssClass="lstItem" />
    </asp:GridView>               
            </td></tr>
    </table>

      </asp:Content>




