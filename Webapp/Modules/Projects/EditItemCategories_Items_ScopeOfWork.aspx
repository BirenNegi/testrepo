<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="EditItemCategories_Items_ScopeOfWork.aspx.cs" Inherits="SOS.Web.EditItemCategories_Items_ScopeOfWork" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />


 <sos:TitleBar ID="TitleBar1" runat="server" Title="Trade ItemCategories and Items" />
  
    
    
    <table>

      <tr>
          <td></td>   
          <td></td>

      </tr>

    <tr>
        <td>


            <asp:Panel ID="pnlDeleteErrors" runat="server" Visible="false">  
    
            <asp:TreeView ID="TreeViewDeleteErrors" runat="server" ShowLines="true">
                <LevelStyles>
                    <asp:TreeNodeStyle CssClass="frmSubTitle" />
                    <asp:TreeNodeStyle CssClass="frmText" />
                    <asp:TreeNodeStyle CssClass="frmError" />
                </LevelStyles>
                <Nodes>
                    <asp:TreeNode></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
            <br />
       
    </asp:Panel>




        </td>
        <td></td>
    </tr>
     <tr valign="top">
     <td>
        <asp:UpdatePanel ID="aupItemCategories" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostbackTrigger ControlID="gvItemCategories" EventName="RowCommand" />
                   
                </Triggers>
                <ContentTemplate>
             
                    <table>
                        <tr valign="top">
                             <td>                        
                                 &nbsp;</td>
                             <td class="auto-style1">
                                 <asp:Label ID="LblTradeCategory" runat="server" Text="" CssClass="frmTitle1"></asp:Label>
                                 <asp:Label ID="LblTradeCategoryId" runat="server" Text="" style="display:none;" ></asp:Label>
                             </td>

                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:GridView ID="gvItemCategories" runat="server" AlternatingRowStyle-CssClass="lstAltItem" AutoGenerateColumns="False" CellPadding="4" CssClass="lstList" DataKeyNames="Id" OnRowCancelingEdit="gvItemCategories_RowCanceling" OnRowCommand="gvItemCategories_RowCommand" OnRowDeleting="gvItemCategories_RowDeleting" OnRowEditing="gvItemCategories_RowEditing" OnRowUpdating="gvItemCategories_RowUpdating" RowStyle-CssClass="lstItem" ShowFooter="True" ShowHeaderWhenEmpty="True">
                                    <AlternatingRowStyle CssClass="lstAltItem" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <FooterTemplate>
                                                <asp:Button ID="BtnAddCategories" runat="server" CausesValidation="true" CommandName="AddCategory" Font-Bold="True" Font-Underline="True" Text="Add" ValidationGroup="NewRecord" />
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ImageUrl="~/Images/IconView.gif" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProjectTradeItemCategory.aspx?TradeItemCategoryId={0}", Eval("IdStr"))%>' ToolTip="Open"></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtUpdateName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UpdateRequiredFieldValidator1" runat="server" ControlToValidate="TxtUpdateName" ErrorMessage="Please enter valid Name" ForeColor="Red" ValidationGroup="UpdateRecord"></asp:RequiredFieldValidator>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNewName" runat="server"></asp:TextBox>
                                                <br />
                                                <asp:RequiredFieldValidator ID="AddRequiredFieldValidator1" runat="server" ControlToValidate="TxtNewName" ErrorMessage="Please enter valid Name" ForeColor="Red" ValidationGroup="NewRecord"></asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Short Description">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtUpdateShortDesc" runat="server" Text='<%# Bind("ShortDescription") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNewShortDesc" runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("ShortDescription") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Long Description">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtUpdateLongDesc" runat="server" Text='<%# Eval("LongDescription") %>' TextMode="MultiLine" Height="16px" Width="220px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNewLongDesc" runat="server" TextMode="MultiLine" Width="96%" ></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="Label3" runat="server" TextMode="MultiLine" ReadOnly="true"  Width="96%" Text='<%# Eval("LongDescription")%>'></asp:TextBox>
                                                <%-- <%# Eval("LongDescription").ToString().Replace("\n","<br/>") %>
                                                                                                                   --%>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="True" />
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Image" CommandName="MoveUp" ImageUrl="~/Images/IconUp.gif" ItemStyle-VerticalAlign="Top" Text="Up" Visible="false">
                                        <HeaderStyle CssClass="lstHeader" />
                                        <ItemStyle VerticalAlign="Top" />
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" CommandName="MoveDown" ImageUrl="~/Images/IconDown.gif" ItemStyle-VerticalAlign="Top" Text="Down" Visible="false">
                                        <HeaderStyle CssClass="lstHeader" />
                                        <ItemStyle VerticalAlign="Top" />
                                        </asp:ButtonField>
                                        <asp:CommandField ShowEditButton="True">
                                        <HeaderStyle CssClass="lstHeader" />
                                             <ItemStyle VerticalAlign="Top" Wrap="True" />
                                        </asp:CommandField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="cmdDeleteTop" runat="server"  Visible="true">
                                        <asp:ImageButton  ID="Image2" runat="server" OnClientClick="return confirm('Delete Trade Item Category and All its Items?');" 
                                            CommandName="Delete" CommandArgument='<%# Eval("Id") %>'  
                                            AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" />

                                    </asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="True" />
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="Manage Items" SelectText="Edit Items" ShowSelectButton="True">
                                        <HeaderStyle CssClass="lstHeader" />
                                             <ItemStyle VerticalAlign="Top" Wrap="True" />
                                        </asp:CommandField>
                                    </Columns>
                                    <FooterStyle BackColor="#F0F0F0" VerticalAlign="Top" />
                                    <RowStyle CssClass="lstItem" />
                                </asp:GridView>
                            </td>
                            <td class="auto-style2">
                                <asp:GridView ID="gvTradeItems" runat="server" AlternatingRowStyle-CssClass="lstAltItem" AutoGenerateColumns="False"
                                     CellPadding="4" CssClass="lstList" DataKeyNames="Id"
                                     OnRowCommand="gvTradeItems_RowCommand" RowStyle-CssClass="lstItem" ShowFooter="True" ShowHeaderWhenEmpty="True" 
                                     OnRowDeleting ="gvTradeItems_RowDeleting" OnRowEditing="gvTradeItems_RowEditing" 
                                     OnRowUpdating="gvTradeItems_RowUpdating" 
                                     OnRowCancelingEdit="gvTradeItems_RowCancelingEdit">
                                    <AlternatingRowStyle CssClass="lstAltItem" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <FooterTemplate>
                                                <asp:Button ID="btbAddItem" runat="server" Text="Add" CommandName="AddTradeItem" />
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ImageUrl="~/Images/IconView.gif" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewProjectTradeItem.aspx?TradeItemId={0}", Eval("IdStr"))%>' ToolTip="Open"></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtItemName" runat="server" Height="22px" Width="150px"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Units">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtUnit" runat="server" Text='<%# Bind("Units") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtItemUnit" runat="server" Height="22px" Width="90px"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Units") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="False" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="QC" ItemStyle-VerticalAlign="Top">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="dpdQC" runat="server" Height="26px" Width="54px" Selectedvalue='<%#Bind("RequiresQuantityCheck") %>'>
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="False">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>

                                            <FooterTemplate>
                                                <asp:DropDownList ID="dpdItemQC" runat="server" Height="26px"  Width="53px">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="False">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </FooterTemplate>


                                            <ItemTemplate>
                                                <sos:BooleanViewer ID="sbvRequiresQuantityCheck" runat="server" Checked='<%#Eval("RequiresQuantityCheck")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="RP" ItemStyle-VerticalAlign="Top">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="dpdRP" runat="server" Height="26px" Width="53px" Selectedvalue='<%#Bind("RequiredInProposal") %>'>
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="False">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="dpdItemRP" runat="server" Height="26px"  Width="53px">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="False">No</asp:ListItem>
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <sos:BooleanViewer ID="sbvRequiredInProposal" runat="server" Checked='<%#Eval("RequiredInProposal")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Scope of Works" ItemStyle-VerticalAlign="Top">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtScopeHeader1" runat="server"  Height="22px"  Rows="3" Text='<%#Eval("Scope")%>' TextMode="MultiLine" Width="96%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtItemScopeWork" runat="server" TextMode="MultiLine" Width="150px" Height="24px"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtScopeHeader" runat="server"  ReadOnly="true" Rows="3" Text='<%#Eval("Scope")%>' TextMode="MultiLine" Width="98%" Height="24px"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Image" CommandName="MoveUp" ImageUrl="~/Images/IconUp.gif" ItemStyle-VerticalAlign="Top" Text="Up" Visible="false">
                                        <HeaderStyle CssClass="lstHeader" />
                                        <ItemStyle VerticalAlign="Top" />
                                        </asp:ButtonField>
                                        <asp:ButtonField ButtonType="Image" CommandName="MoveDown" ImageUrl="~/Images/IconDown.gif" ItemStyle-VerticalAlign="Top" Text="Down" Visible="false">
                                        <HeaderStyle CssClass="lstHeader" />
                                        <ItemStyle VerticalAlign="Top" />
                                        </asp:ButtonField>
                                        <asp:CommandField ShowEditButton="True">
                                        <HeaderStyle CssClass="lstHeader" />
                                        <ItemStyle VerticalAlign="Top" />
                                        </asp:CommandField>

                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="cmdDeleteTradeItem" runat="server"  Visible="true">
                                        <asp:ImageButton  ID="Image21" runat="server" OnClientClick="return confirm('Delete Trade Item?');" 
                                            CommandName="Delete" CommandArgument='<%# Eval("Id") %>'  
                                            AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" />

                                    </asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle VerticalAlign="Top" Wrap="True" />
                                        </asp:TemplateField>


                                    </Columns>
                                    <RowStyle CssClass="lstItem" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
            

          </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <td>
            

        </td> 
        
    </tr>

                    


</table>





</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            text-align: center;
            width: 653px;
        }
        .auto-style2 {
            width: 653px;
        }
    </style>
</asp:Content>


