<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewSubContractPage" Title="Variation Order" Codebehind="ViewSubContract.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Variation Order" />

<asp:Panel ID="pnlErrors" runat="server" Visible="false">
<asp:TreeView ID="TreeView1" runat="server" ShowLines="true">
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

<table>
    <tr>
        <td><asp:Image ID="imgGeneralInfo" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">General Information</td>
    </tr>
</table>

<asp:Panel ID="pnlGeneralInfo" runat="server" CssClass="collapsePanel">
<table cellspacing="0" cellpadding="0">
    <tr>
        <td>
            &nbsp;
            <asp:LinkButton ID="cmdEditTop" runat="server" Visible="false" OnClick="cmdEdit_Click"><asp:Image ID="Image1" runat="server" AlternateText="Edit" ImageUrl="~/Images/IconEdit.gif" /></asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="cmdDeleteTop" runat="server" Visible="false" OnClick="cmdDelete_Click"><asp:Image ID="Image2" runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Subcontractor:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblSubcontractor" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Number:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Title:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblTitle" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">GST:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblGoodsServicesTax" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Quotes File:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblQuotesFileName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">Site Instruction:</td>
                    <td class="frmData"><asp:Label ID="lblSiteInstruction" runat="server" Width="150"></asp:Label></td>
                    <td class="frmData">&nbsp;</td>
                    <td class="frmLabel">Dated:</td>
                    <td class="frmData"><asp:Label ID="lblSiteInstructionDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel">S/Cont.Ref:</td>
                    <td class="frmData"><asp:Label ID="lblSubContractorReference" runat="server" Width="150"></asp:Label></td>
                    <td class="frmData">&nbsp;</td>
                    <td class="frmLabel">Dated:</td>
                    <td class="frmData"><asp:Label ID="lblSubContractorReferenceDate" runat="server"></asp:Label></td>
                </tr>
                <%-- DS20240321 --%>
                <tr>
                    <td class="frmLabel">Orig Contr.Dur:</td>
                    <td class="frmData"><asp:CheckBox ID="chkOrigContractDur" Enabled="False" runat="server" Width="150"></asp:CheckBox></td>
                    <td class="frmData">&nbsp;</td>
                    <td class="frmLabel">Start Date:</td>
                    <td class="frmData"><asp:Label ID="lblStartDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="frmLabel"></td>
                    <td class="frmData"></td>
                    <td class="frmData">&nbsp;</td>
                    <td class="frmLabel">Finish Date:</td>
                    <td class="frmData"><asp:Label ID="lblFinishDate" runat="server"></asp:Label></td>
                </tr>
                <%-- DS20240321 --%>

                <tr>
                    <td class="frmLabel">Comments:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblComments" runat="server"></asp:Label></td>
                </tr>                
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<br />

<table>
<tr>
    <td>
        <table>
            <tr>
                <td class="lstTitle">Approval Process Manager</td>

                <td>&nbsp;&nbsp;</td>
                <td><sos:FileLink ID="sflQuotesFile" runat="server" FileTitle="Quotes File" /></td>
                
                <asp:PlaceHolder ID="phViewContract" runat="server" Visible="false">
                    <td>&nbsp;&nbsp;</td>
                    <td><asp:HyperLink ID="lnkViewContract" runat="server" CssClass="frmLink">Variation Order</asp:HyperLink></td>
                </asp:PlaceHolder>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td><sos:ProcessManager ID="ProcessManagerSubcontract" runat="Server"></sos:ProcessManager></td>
</tr>
</table>
<br />





<asp:UpdatePanel ID="upSucontracts" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="gvVariations" EventName="RowDeleting" />
    </Triggers>
    <ContentTemplate>
        <table>
            <tr>
                <td class="lstTitle">Variations</td>
        
                <asp:PlaceHolder ID="phAddVariation" runat="server" Visible="false">
                <td>&nbsp;&nbsp;</td>
                <td><asp:HyperLink ID="lnkAddVariation" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                <td>&nbsp;&nbsp;</td>
                </asp:PlaceHolder>

                <td><sos:BalanceInclude id="sbiTrade" runat="server"></sos:BalanceInclude></td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:GridView
                        ID = "gvVariations"
                        runat = "server"
                        DataKeyNames="Id"
                        AutoGenerateColumns = "False"
                        OnRowDeleting="gvVariations_RowDeleting"
                        OnDataBound="gvVariations_DataBound"
                        CellPadding = "4"
                        CssClass = "lstList"
                        RowStyle-CssClass = "lstItem"
                        ShowFooter = "True"
                        AlternatingRowStyle-CssClass = "lstAltItem">
                        <AlternatingRowStyle CssClass="lstAltItem" />
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Contracts/EditVariation.aspx?VariationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField ItemStyle-VerticalAlign="Top" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton Runat="server" OnClientClick="return confirm('Delete Variation?');" CommandName="Delete"><asp:Image runat="server" AlternateText="Delete" ImageUrl="~/Images/IconDelete.gif" /></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Header/Description" FooterText="Total" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" FooterStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <b><%#Eval("Header")%></b>
                                    <br />
                                    <%-- #---  <%#Eval("Description")%> --To display multiple lines--%>

                                    <%#Eval("Description")!=null?Eval("Description").ToString().Replace(Environment.NewLine, "<br/>"):null%>

                                    <%-- San --%>

                                </ItemTemplate>
                                <FooterStyle CssClass="lstFooter" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("Amount"))%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%#TotalVariations()%>
                                </FooterTemplate>
                                <FooterStyle CssClass="lstFooter" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Allocation" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("Allowance"))%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%#TotalAllocations()%>
                                </FooterTemplate>
                                <FooterStyle CssClass="lstFooter" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Win/Loss" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("BudgetWinLoss"))%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%#TotalWinLoss()%>
                                </FooterTemplate>
                                <FooterStyle CssClass="lstFooter" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#BudgetTypeInfo((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#BudgetNameInfo((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>

                            <asp:BoundField DataField="TradeCode" HeaderText="Code" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                            <HeaderStyle CssClass="lstHeader" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />
                            </asp:BoundField>
                            
                            <asp:TemplateField HeaderText="Original" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <span class="<%#BudgetOriginalStyleName((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%>"><%#BudgetOriginalInfo((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%></span>
                                </ItemTemplate>
                                <FooterStyle CssClass="lstFooter" />
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Unallocated" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <span class="<%#BudgetUnallocatedlStyleName((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%>"><%#BudgetUnallocatedlInfo((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%></span>
                                </ItemTemplate>
                                <FooterStyle CssClass="lstFooter" />
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>

                            



                                 <%--Uncoment to show trade current and trade code budget 

                            <asp:TemplateField HeaderText="Current" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <span class="<%#StyleName((decimal?)Eval("AllowanceBudget"))%>"><%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("AllowanceBudget"))%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                             
                            <asp:TemplateField HeaderText="Original" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <span class="<%#TradeBudgetOriginalStyleName((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%>"><%#TradeBudgetOriginalInfo((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%></span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Current" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <span class="<%#TradeBudgetCurrentStyleName((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%>"><%#TradeBudgetCurrentInfo((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                               
                            --%>

                            <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#SOS.Web.Utils.GetConfigListItemName("Global", "VariationTypes", (String)Eval("Type"))%>
                                </ItemTemplate>
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="CV/SA Status" HeaderStyle-CssClass="lstHeader" FooterStyle-CssClass="lstFooter" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <%#CVSAStatus((SOS.Core.VariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                                </ItemTemplate>
                                <FooterStyle CssClass="lstFooter" />
                                <HeaderStyle CssClass="lstHeader" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="lstItem" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<br />

<act:UpdatePanelAnimationExtender ID="upaeBudgetBalances" runat="server" TargetControlID="upSucontracts">
    <Animations>
        <OnUpdated>
            <FadeIn AnimationTarget="gvVariations" Duration="0.5" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:CollapsiblePanelExtender
    ID="cpe" 
    runat="Server"
    Collapsed="False"     
    TargetControlID="pnlGeneralInfo"
    ExpandControlID="imgGeneralInfo"
    CollapseControlID="imgGeneralInfo"
    ImageControlID="imgGeneralInfo"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

</asp:Content>

