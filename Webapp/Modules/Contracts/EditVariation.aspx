<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditVariationPage" Title="Variation" Codebehind="EditVariation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" />

<table cellspacing="0" cellpadding="0">
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
                    <td class="frmReqLabel">Header:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valHeader" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtHeader"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtHeader" runat="server" Width="600"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Description:</td>
                    <td><asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="8" Width="600"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Amount:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valAmount" runat="server" CssClass="frmError" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtAmount"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="valAmount1" runat="server" CssClass="frmError" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtAmount" Operator="DataTypeCheck" Type="Currency"></asp:CompareValidator>
                        <asp:TextBox ID="txtAmount" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                   
                    <td class="frmLabel">Allocation:</td>

                    <td>
                        <asp:CompareValidator ID="valAllowance" runat="server" CssClass="frmError" Display="Dynamic"
                           ErrorMessage="Invalid number!<br />" ControlToValidate="txtAllowance" Operator="DataTypeCheck" Type="Currency"></asp:CompareValidator>
                        
                        <asp:CustomValidator ID="valBOQAllowance" runat="server" OnServerValidate="valBOQAllowance_ServerValidate" CssClass="frmError" 
                            Display="Dynamic" ValidateRequestMode="Enabled"></asp:CustomValidator>

                        <asp:TextBox ID="txtAllowance" runat="server" Width="150"></asp:TextBox>
                    </td>

                   
                </tr>

                <asp:PlaceHolder ID="phTrade" runat="server" Visible="false">
                <tr>
                    <td class="frmReqLabel">Trade:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valTrade" runat="server" CssClass="frmError" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlTrade"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddlTrade" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                </asp:PlaceHolder>

                <tr>
                    <td class="frmLabel">Related Trade: <samp class="frmTextSmall">(* Back Charge only)</samp></td>
                    <td>
                        <asp:UpdatePanel ID="aupBudgetRelatedTrade" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostbackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:CustomValidator ID="valRealtedTrade" runat="server" OnServerValidate="valRealtedTrade_ServerValidate" Enabled="false" CssClass="frmError" Display="Dynamic" ErrorMessage="Related trade required for Back Charge.<br />"></asp:CustomValidator>
                                <asp:DropDownList ID="ddlRelatedTrade" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>

                <asp:PlaceHolder ID="phBudget" runat="server" Visible="false">
                <tr>
                    <td class="frmReqLabel">Type:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valType" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlType"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddlType" AutoPostBack="true" runat="server" onselectedindexchanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Budget source:</td>
                    <td>
                        <asp:CustomValidator ID="valBudgetSource" runat="server" OnServerValidate="valBudgetSource_ServerValidate" Enabled="false" CssClass="frmError" Display="Dynamic" ErrorMessage="Budget source is required.<br />"></asp:CustomValidator>
                        <asp:UpdatePanel ID="aupBudgetSources" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostbackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlBudgetSource" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBudgetSource_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>



                <tr>
                    <td class="frmLabel">Budget balance:</td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="aupBudgetBalances" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostbackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostbackTrigger ControlID="ddlBudgetSource" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                        <ContentTemplate>

                                            <sos:BalanceInclude id="sbiTrade" runat="server"></sos:BalanceInclude>

                                            <table cellpadding="4" cellspacing="1" runat="server" id="tblBudgetBalances">
                                                <tr>
                                                    <td class="lstHeader" align="center" colspan="3">Budget</td>
                                                    <%-- Uncoment to show trade code budget 
                                                    <td class="lstHeader" align="center" colspan="2">Trade Budget</td>
                                                    --%>
                                                </tr>
                                                <tr>
                                                    <td class="lstHeader" align="center">Original</td>
                                                    <td class="lstHeader" align="center">Current</td>
                                                    <%--SAn--%>
                                                    <td class="lstHeader" align="center">Unallocated</td>
                                                    <%--SAn--%>

                                                    <%-- Uncoment to show trade code budget 
                                                    <td class="lstHeader" align="center">Original</td>
                                                    <td class="lstHeader" align="center">Current</td>
                                                    --%>
                                                </tr>
                                                <tr>
                                                    <td class="frmLabel" align="center"><asp:Label ID="lblBudgetOriginal" runat="server"/></td>
                                                    <td class="frmLabel" align="center"><asp:Label ID="lblBudgetCurrent" runat="server"/></td>
                                                  <%--SAn--%>
                                                      <td class="frmLabel" align="center"><asp:Label ID="lblBudgetunallocated" runat="server"/></td>
                                                    
                                                      <td class="frmLabel" align="center"><asp:Label ID="lblUnallocated" runat="server" Visible="false"/></td>
                                                  <%--SAn--%>
                                                    
                                                      <%-- Uncoment to show trade code budget 
                                                    <td class="frmLabel" align="center"><asp:Label ID="lblTradeBudgetOriginal" runat="server"/></td>
                                                    <td class="frmLabel" align="center"><asp:Label ID="lblTradeBudgetCurrent" runat="server"/></td>
                                                    --%>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </asp:PlaceHolder>





                <asp:PlaceHolder ID="phCVNumber" runat="server" Visible="false">
                <tr>

                    <td class="frmLabel" id="tdType" runat="server">Client Variation Number:</td>
                    <td class="frmTextDis">
                        <asp:UpdatePanel ID="aupItemCategories" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostbackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                            </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCVNumber" runat="server" Visible="false"></asp:DropDownList>
                                    <asp:DropDownList ID="ddlSANumber" runat="server" Visible="false"></asp:DropDownList>
                                    <%-- San --%>
                                     <asp:DropDownList ID="ddlTVNumber" runat="server" Visible="false"></asp:DropDownList>
                                      <%-- San --%>
                                    <asp:TextBox ID="txtNumber" runat="server" Width="150" Visible="false"></asp:TextBox>
                                    <asp:Label ID="lblNumber" runat="server" Visible="false">Select type</asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                </asp:PlaceHolder>
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

<act:UpdatePanelAnimationExtender ID="upaeType" runat="server" TargetControlID="aupItemCategories">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdType" Duration="0.5" Fps="20" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:UpdatePanelAnimationExtender ID="upaeBudgetBalances" runat="server" TargetControlID="aupBudgetBalances">
    <Animations>
        <OnUpdated>
            <FadeIn AnimationTarget="tblBudgetBalances" Duration="0.5" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

</asp:Content>
