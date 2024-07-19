<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ShowBudgetTradeSummary.aspx.cs" Inherits="SOS.Web.ShowBudgetTradeSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

       <br />
    <table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <asp:ScriptManager ID="scriptManager1" runat="server"></asp:ScriptManager>
                
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="frmReqLabel" id="tdProject" runat="server">Project:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlProjects"></asp:RequiredFieldValidator>

                        <asp:UpdatePanel ID="upProjects" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlProjects" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>  

                        &nbsp;
                        <asp:CheckBox ID="chkAll" Checked="false" AutoPostBack="true" runat="server" CausesValidation="false" OnCheckedChanged="chkAll_CheckedChanged" />
                        <span class="frmTextSmall">List All</span>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right"style="padding-right:50px;">
                        <asp:LinkButton ID="cmdGenerateReport" CssClass="frmButton" runat="server" OnClick="cmdGenerateReport_Click">View</asp:LinkButton>

                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="frmBottomBox">
            
            <%--<asp:linkbutton id="cmdCancelBottom" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>--%>
        </td>
    </tr>
</table>

    <br />

  <br />








  <table>
      <tr align="right" style="padding-right:20px;">
           <td>
    <%--<asp:Button ID="Button1" CssClass="frmButton" runat="server" Text="Excel" OnClick="Button1_Click" />--%>
               <asp:ImageButton ID="ImageButton1" runat="server" CssClass="frmButton" OnClick="Button1_Click" ImageUrl="~/Images/excel1.png" Height="36px" Width="39px" ToolTip="Export to excel" />

           </td>
      </tr>
      <tr align="right">
           <td><sos:BalanceInclude id="sbiBOQ" runat="server"></sos:BalanceInclude></td>
      </tr>
    <tr>
        <td runat="server"> 




            <asp:GridView ID="gvBOQTrade" runat="server" AlternatingRowStyle-CssClass="lstAltItem" AutoGenerateColumns="False" CellPadding="4" CssClass="lstList" DataKeyNames="Code" OnRowDataBound="OnRowDataBound" RowStyle-CssClass="lstItem">
                <AlternatingRowStyle CssClass="lstAltItem" />
                <Columns>


                    <%-- -------------------------- --%>









                   <%----%> 

                    <asp:BoundField DataField="Code" HeaderStyle-CssClass="lstHeader" HeaderText="Trade Code" ItemStyle-VerticalAlign="Top">
                    <HeaderStyle CssClass="lstHeader" />
                    <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>

                    <asp:BoundField DataField="BudgetName" HeaderStyle-CssClass="lstHeader" HeaderText="Name" ItemStyle-VerticalAlign="Top">
                    <HeaderStyle CssClass="lstHeader" />
                    <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>


                   <%-- <asp:BoundField DataField="BudgetAmount" HeaderStyle-CssClass="lstHeader" HeaderText="BOQ" ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="False">
                    <HeaderStyle CssClass="lstHeader" />
                    <ItemStyle VerticalAlign="Top" Wrap="False" />
                    </asp:BoundField>--%>


                     <asp:TemplateField HeaderText="BOQ" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("BudgetAmount"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Trade   -   Amount   -   Balance">
                        <EditItemTemplate>
                        </EditItemTemplate>
                        <ItemTemplate>
                        <%--<img alt = "" style="cursor: pointer" src="~/Images/IconCollapse.jpg" />--%>
                            <%--<asp:Panel ID="pnlOrders" runat="server"></asp:Panel>--%>
                                <asp:GridView ID="gvTrades" runat="server" AutoGenerateColumns="False"  GridLines="Horizontal" Width="100%">
                                    <Columns>


                                     <%--   <asp:BoundField DataField="Amount" HeaderText="Contract" ItemStyle-Width="150px">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="150px" />
                                        </asp:BoundField>--%>


                                        <%--<asp:BoundField DataField="Balance" HeaderText="Balance" ItemStyle-Width="150px">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="150px" />
                                        </asp:BoundField>--%>

                                         <asp:BoundField DataField="Date" HeaderText="Date" >
                                         <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="Type" HeaderText="Type " >
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="TradeCode" HeaderText="Trade " ItemStyle-Wrap="true" ItemStyle-CssClass="WrappedText">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle  />
                                        </asp:BoundField>


                                         <asp:BoundField DataField="Amount" HeaderText="Amount" >
                                         <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="Balance" HeaderText="Balance" >


                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>


                                        <asp:BoundField DataField="Allocation" HeaderText="Comparison" >
                                        <HeaderStyle HorizontalAlign="Right"  />
                                        <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>


                                          <%--<asp:BoundField DataField="Contract" HeaderText="Contract" >
                                        <HeaderStyle HorizontalAlign="Right" Width="20%" Wrap="True" />
                                        <ItemStyle Width="20%" HorizontalAlign="Right" />
                                        </asp:BoundField>--%>

                                        <asp:BoundField DataField="WinLoss" HeaderText="Win/Loss">
                                        <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                        <ItemStyle Width="20%" HorizontalAlign="Right" />
                                        </asp:BoundField>

                                    </Columns>
                                </asp:GridView>
                            
                        </ItemTemplate>
                        <HeaderStyle CssClass="lstHeader" />
                        <ItemStyle />
                    </asp:TemplateField>



                   <asp:TemplateField HeaderText="Balance" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("BudgetAmountTradeInitial"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>



                    

                      <asp:TemplateField HeaderText="Unallocated" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("BudgetUnallocated"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>







                </Columns>
                <RowStyle CssClass="lstItem" />
            </asp:GridView>


        </td>
    </tr>
</table>


</asp:Content>
