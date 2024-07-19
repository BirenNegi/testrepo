<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ShowCVsSummary.aspx.cs" Inherits="SOS.Web.ShowCVsSummary" %>
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



  <table>
      <tr align="right" style="padding-right:20px;">
           <td>
    <%--<asp:Button ID="Button1" CssClass="frmButton" runat="server" Text="Excel" OnClick="Button1_Click" />--%>
               <asp:ImageButton ID="ImageButton1" runat="server" CssClass="frmButton" OnClick="Button1_Click" ImageUrl="~/Images/excel1.png" Height="36px" Width="39px" ToolTip="Export to excel" />

           </td>
      </tr>
      <tr align="right">
           <td><sos:BalanceInclude id="sbiTrades" runat="server"></sos:BalanceInclude></td>
      </tr>
    <tr>
        <td runat="server"> 

            <asp:GridView
                ID = "gvClientVariations"
                runat = "server"
                DataKeyNames = "Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem"

                OnRowDataBound="OnRowDataBound">

<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>

                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top" >
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RevisionName" HeaderText="R" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>                    
                    <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Write Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("WriteDate"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Verbal Approval" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("VerbalApprovalDate"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Final Approval" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("ApprovalDate"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormDecimal((decimal?)Eval("TotalAmount"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>


                    <%-- -------------------------- --%>
                   <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Trade   -   Amount   -   Balance">
                       <EditItemTemplate>
                           
                       </EditItemTemplate>
                    <ItemTemplate>
                        <%--<img alt = "" style="cursor: pointer" src="~/Images/IconCollapse.jpg" />--%>
                        <asp:Panel ID="pnlOrders" runat="server">
                            <asp:GridView ID="gvTrades" runat="server" AutoGenerateColumns="False" CssClass = "ChildGrid" Width="100%" GridLines="Horizontal">
                                <Columns> 
                                    <asp:BoundField ItemStyle-Width="150px" DataField="TradeCode" HeaderText="Trade" >                                             
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Amount" HeaderText="Amount" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Balance" HeaderText="Balance" >
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                  </asp:TemplateField>









                   <%----%> <asp:TemplateField HeaderText="Process Status" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top" Visible="False">
                        <ItemTemplate>
                            <%#InfoStatus((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeaderTop"></HeaderStyle>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="lstHeaderTop" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center" Visible="False">
                        <ItemTemplate>
                            <%#DateStatus((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeaderTop"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="" Visible="False">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#Eval("IsInternallyApproved")%>' runat="server" NavigateUrl='<%#LinkClientVariation((SOS.Core.ClientVariationInfo)DataBinder.Eval(Container, "DataItem"))%>'></asp:HyperLink>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comments" HeaderText="Comments" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="True" ItemStyle-VerticalAlign="Top" Visible="False">
<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle VerticalAlign="Top" Wrap="True"></ItemStyle>
                    </asp:BoundField>
                </Columns>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>
            

        </td>
    </tr>
</table>






<br />

</asp:Content>

