<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewTradeTemplatePage" Title="Trade Template" Codebehind="ViewTradeTemplate.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Trade Template" />

<table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Name:</td>
                    <td class="frmData"><asp:Label ID="lblName" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Code:</td>
                    <td class="frmData"><asp:Label ID="lblCode" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Job Type:</td>
                    <td class="frmData"><asp:Label ID="lblJobType" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Standard Trade:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvIsStandard" runat="server"></sos:BooleanViewer></td>
                </tr>
                <tr>
                    <td class="frmLabel">Description:</td>
                    <td class="frmData"><asp:Label ID="lblDescription" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Requires Tender:</td>
                    <td class="frmData"><sos:BooleanViewer ID="sbvTenderRequired" runat="server"></sos:BooleanViewer></td>
                </tr>
                <tr>
                    <td class="frmLabel">Days from PCD:</td>
                    <td class="frmData"><asp:Label ID="lblDaysFromPCD" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Header:</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="txtScopeHeader" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Width="640" Rows="12" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Scope Footer:</td>
                    <td class="frmData" colspan="4"><asp:TextBox ID="txtScopeFooter" CssClass="frmData" TextMode="MultiLine" ReadOnly="true" Width="640" Rows="12" runat="server"></asp:TextBox></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />

<table>
    <tr>
        <td>
            <table>
                <tr>
                    <td class="lstTitle">Item Categories</td>
                    
                    <asp:PlaceHolder ID="phAddItemCategory" runat="server" Visible="false">
                    <td>&nbsp;</td>
                    <td><asp:HyperLink ID="lnkAddItemCategory" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                    </asp:PlaceHolder>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="aupITemCategories" runat="server" UpdateMode="Conditional" RenderMode="Inline">
	            <Triggers>
			        <asp:AsyncPostbackTrigger ControlID="gvItemCategories" EventName="RowCommand" />
        	    </Triggers>
	            <ContentTemplate>
                    <asp:GridView
                        ID = "gvItemCategories"
                        runat = "server"
                        DataKeyNames="Id"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CellSpacing = "0"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem"
                        OnRowCommand = "gvItemCategories_RowCommand">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Trades/ViewTradeItemCategory.aspx?TradeItemCategoryId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="ShortDescription" HeaderText="Short Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="LongDescription" HeaderText="Long Description" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:ButtonField CommandName="MoveUp" ButtonType="Image" ImageUrl="~/Images/IconUp.gif" Text="Up" ItemStyle-VerticalAlign="Top" Visible="false" />
                            <asp:ButtonField CommandName="MoveDown" ButtonType="Image" ImageUrl="~/Images/IconDown.gif" Text="Down" ItemStyle-VerticalAlign="Top" Visible="false" />
                        </Columns>
                    </asp:GridView>    
	            </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
<br />

<asp:UpdatePanel ID="upDrawingTypes" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="butAddDrawingType" EventName="Click" />
        <asp:AsyncPostbackTrigger ControlID="gvDrawingTypes" EventName="RowDeleting" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Drawing Types</td>

                            <asp:PlaceHolder ID="phAddDrawingTypes" runat="server" Visible="false">
                            <td>&nbsp;</td>
                            <td><asp:DropDownList ID="ddlDrawingTypes" runat="server"></asp:DropDownList></td>
                            <td><asp:Button ID="butAddDrawingType" runat="server" OnClick="butAddDrawingType_Click" Text="Add" /></td>
                            </asp:PlaceHolder>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView 
                        ID="gvDrawingTypes" 
                        runat="server" 
                        DataKeyNames="Id"
                        OnRowDeleting="gvDrawingTypes_RowDeleting"
                        AutoGenerateColumns="False" 
                        CellPadding="2" 
                        CellSpacing="0" 
                        CssClass="lstList" 
                        RowStyle-CssClass="lstItem" 
                        AlternatingRowStyle-CssClass="lstAltItem">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="False">
                                <ItemTemplate>
                                    <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete Drawing Type from Trade Template?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False"/>                    
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<br />

<asp:UpdatePanel ID="aupSubContractors" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="butAddSubContractor" EventName="Click" />
        <asp:AsyncPostbackTrigger ControlID="gvSubContractors" EventName="RowDeleting" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td class="lstTitle">Rating A Subcontractors</td>
                            
                            <asp:PlaceHolder ID="phAddSubContractor" runat="server" Visible="false">
                            <td>&nbsp;</td>
                                <td class="frmText">Business Unit:</td>
                                <td><asp:DropDownList ID="ddlBusinessUnit" onselectedindexchanged="ddlBusinessUnit_SelectedIndexChanged"  AutoPostBack="true" runat="server"></asp:DropDownList></td>

                            <td class="frmText">
                                <asp:TextBox ID="txtSubContractorName" Width="150" runat="server" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                                <input id="txtSubContractorId" type="hidden" runat="server" />
                                <asp:HyperLink ID="cmdSelSubContractor" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>

                                <td>&nbsp;&nbsp;</td>

                                </td>
                               <td><asp:Button ID="butAddSubContractor" runat="server" OnClick="butAddSubContractor_Click" Text="Add" /></td>
                               </asp:PlaceHolder>
                     </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView 
                        ID = "gvSubContractors"
                        runat = "server"
                        DataKeyNames="Id"
                        OnRowDeleting="gvSubContractors_RowDeleting"
                        AutoGenerateColumns = "False"
                        CellPadding = "4"
                        CellSpacing = "0"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        AlternatingRowStyle-CssClass = "lstAltItem" OnRowDataBound="gvSubContractors_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/SubContractors/ViewSubContractor.aspx?subContractorId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="False">
                                <ItemTemplate>
                                    <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete Default Subcontractor from Trade Template?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="BusinessUnitName" HeaderText="Business Unit" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="Street" HeaderText="Address" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="Locality" HeaderText="Suburb" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <asp:BoundField DataField="State" HeaderText="State" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <%-- San --%>
                                <asp:BoundField  HeaderText="Other Business Units" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                            <%-- San --%>
                            </Columns>
                    </asp:GridView>    
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<br />

</asp:Content>
