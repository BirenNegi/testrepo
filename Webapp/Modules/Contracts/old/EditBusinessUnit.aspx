<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditBusinessUnitPage" Title="Business Unit" Codebehind="EditBusinessUnit.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

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
            <table>
                <tr>
                    <td class="frmReqLabel">Name:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Job Number format:</td>
                    <td class="frmText">
                        <asp:RequiredFieldValidator ID="valProjectNumberFormat" CssClass="frmReqMessage" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="ddlProjectNumberFormat"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddlProjectNumberFormat" runat="server">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Project Number - Year" Value="P-Y"></asp:ListItem>
                            <asp:ListItem Text="Year - Project Number" Value="Y-P"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <%-- San --%>
                <tr>
                    <td class="frmLabel">Trade amount COM authority:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtTradeComAmountApproval"></asp:CompareValidator>
                        <asp:TextBox ID="txtTradeComAmountApproval" runat="server" Width="150"></asp:TextBox>
                    &nbsp;<span class="frmLabel" style="vertical-align:text-top;background-color:transparent">OR overbudget by more than
                      <asp:CompareValidator ID="CompareValidator2" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtTradeComOverbudget"></asp:CompareValidator>
                          <asp:TextBox ID="txtTradeComOverbudget" runat="server" Width="23px" MaxLength="2"></asp:TextBox>
                    %</span>&nbsp;</td>
                </tr>
               


                <tr>
                    <td class="frmLabel">Trade amount DA authority:</td>
                    <td class="frmText"><asp:CompareValidator ID="CompareValidator3" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtTradeDAamountApproval"></asp:CompareValidator>
                        <asp:TextBox ID="txtTradeDAamountApproval" runat="server" Width="150"></asp:TextBox>
                    
                        &nbsp;</td>
                </tr>

            <%-- San --%>




                <tr>
                    <td class="frmLabel">Trade overbudget DA authority:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valTradeOverbudgetApproval" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtTradeOverbudgetApproval"></asp:CompareValidator>
                        <asp:TextBox ID="txtTradeOverbudgetApproval" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="frmLabel">Trade amount UM authority:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="valTradeAmountApproval" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtTradeAmountApproval"></asp:CompareValidator>
                        <asp:TextBox ID="txtTradeAmountApproval" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <%-- San --%>
                <tr>
                   <td class="frmLabel">Trade overbudget UM authority:</td>
                    <td class="frmText">
                        <asp:CompareValidator ID="CompareValidator4" runat="server" CssClass="frmError" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtTradeUMOverbudgetApproval"></asp:CompareValidator>
                        <asp:TextBox ID="txtTradeUMOverbudgetApproval" runat="server" Width="150"></asp:TextBox>
                    </td>
                </tr>
                <%-- San --%>
                <tr>
                    <td class="frmLabel">Unit Manager:</td>
                    <td>
                        <asp:TextBox ID="txtUM" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelUM" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtUMId" type="hidden" runat="server" />
                    </td>
                </tr>

                <%-- San --%>
                 <tr>
                    <td class="frmLabel">Estimating Director:</td>
                    <td>
                        <asp:TextBox ID="TxtED" runat="server" Width="150" ReadOnly="True" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelED" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtEDId" type="hidden" runat="server" />
                    </td>
                </tr>

                <%-- San --%>

                <tr>
                    <td class="frmLabel" valign="top">Claims Special Note:</td>
                    <td><asp:TextBox ID="txtClaimSpecialNote" runat="server" TextMode="MultiLine" Rows="6" Width="330"></asp:TextBox></td>
                </tr>
            </table>
            <table id="tblProcesses" cellpadding="4" cellspacing="1" runat="server">
                <tr>
                    <td class="lstHeader" align="center">Job Type</td>
                    <td class="lstHeader" align="center">Trades Process</td>
                    <td class="lstHeader" align="center">Contracts Process</td>
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

</asp:Content>

