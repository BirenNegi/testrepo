<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditSiteOrderDetailHire" Title="Orders" Codebehind="EditSiteOrderDetailHire.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:PlaceHolder ID="phAddNew" runat="server"></asp:PlaceHolder>
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <sos:TitleBar ID="TitleBar" runat="server" Title="Site Orders" />

<asp:Panel ID="pnlForm" runat="server" Height="840px" Width="599px">
   <table cellspacing="0" cellpadding="0">
       <tr>
            <td class=".auto-styleMSG" colspan="2">
               <asp:Label ID="lblMessage" Text="Enter Details & press Save or click Exit to go back" runat="server" Width="334px"></asp:Label>
            </td>
       </tr>

   </table>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <table cellpadding="0" cellspacing="0" class="auto-style41">
        <tr>
            <td class="auto-style42">
                <asp:LinkButton ID="cmdUpdateTop" runat="server" CssClass="frmButton" OnClick="cmdUpdate_Click">Save & Exit</asp:LinkButton>
                 &nbsp;&nbsp;
                <asp:LinkButton ID="cmdSaveTop" runat="server" CssClass="frmButton" OnClick="cmdSave_Click">Save</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="cmdCancelTop" runat="server" CausesValidation="False" CssClass="frmButton"  OnClick="cmdCancel_Click">Exit</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="cmdDeleteTop" runat="server"  UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to delete the item?');" CausesValidation="False" CssClass="frmButton" OnClick="cmdDelete_Click">Delete</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class="auto-style2">
                <table class="auto-style39">
                    <tr>
                        <td class="frmSubSubTitle" colspan="4">Edit order Item</td>
                    </tr>
                    <tr>
                        <td class="auto-styleLH1">Item:</td>
                        <td colspan="4" class="auto-style46">
                            <asp:TextBox ID="txtTitle" runat="server" Width="334px"></asp:TextBox>
                             <input id="txtItemId" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-styleLH1">Quantity:</td>
                        <td class="auto-style46">
                            <asp:TextBox ID="txtQuantity" runat="server" Width="180px" style="text-align: right"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="auto-styleLH1">Unit Price:</td>
                        <td class="auto-style46">
                            <asp:TextBox ID="txtPrice" runat="server" Height="18px" Width="180px" style="text-align: right"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="auto-styleLH1">UM:</td>
                        <td class="auto-style14">
                            <asp:TextBox ID="txtUM" runat="server" Height="16px" Width="80px"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <td class="auto-style48">Amount:</td>
                        <td class="auto-style46">
                            <asp:TextBox ID="txtAmount" runat="server" Height="16px" Width="180px" BackColor="#DDDDDD" ReadOnly="True" style="text-align: right"></asp:TextBox>
                        </td>

                    </tr>
 
 

                    <%-- San --%>
                    <%-- San --%>
                    <%-- San New role and processSteps for COM  and JCA--%>
                    <%---------- San ------------%>
                </table>
            </td>
        </tr>
          
        <tr>
            <td class="auto-style42">
                <asp:LinkButton ID="cmdUpdateBottom" runat="server" CssClass="frmButton" OnClick="cmdUpdate_Click">Save & Exit</asp:LinkButton>
                 &nbsp;&nbsp;
                <asp:LinkButton ID="cmdSaveBottom" runat="server" CssClass="frmButton" OnClick="cmdSave_Click">Save</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="cmdCancelBottom" runat="server" CausesValidation="False" CssClass="frmButton" OnClick="cmdCancel_Click">Exit</asp:LinkButton>
               &nbsp;&nbsp;
                <asp:LinkButton ID="cmdDeleteBottom" runat="server"  UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to delete the item?');" CausesValidation="False" CssClass="frmButton" OnClick="cmdDelete_Click">Delete</asp:LinkButton>
            </td>
        </tr>
    </table>
        <table>
    <tr>
        <td>
            <table cellpadding="1" cellspacing="1">
                <tr>
                      <td class="auto-styleHDG">
                          <br />
                          <br />
                          Existing Items:</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
     
            <asp:GridView
                ID = "gvSiteOrderItems"
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
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Item" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top"></asp:BoundField>
                    <asp:BoundField DataField="Qty" HeaderText="Quantity" DataFormatString="{0:N2}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                    <asp:BoundField DataField="UM" HeaderText="U/M" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:BoundField DataField="Price" HeaderText="Unit Price" DataFormatString="{0:C2}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C2}" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                </Columns>
            </asp:GridView>    
        </td>
    </tr>
</table>
 </asp:Panel>

</asp:Content>


<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-styleBTN {
            padding-bottom: 3px;
            text-align: center;
            width: 900px;
        }
        .auto-style2 {
            background-color: #FFFFFF;
            border: #333333 1px solid;
            width: 630px;
        }
         .auto-style14 {
            width: 118px;
            height: 28px;
        }

        .auto-styleLHL {
            background-color: #DDDDDD;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 90px;
        }

        .auto-style39 {
            width: 436px;
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
        .auto-styleLHN {
            background-color: #DDDDDD;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 73px;
        }
        .auto-styleTXT {
            width: 300px;
        }

        .auto-styleMid {
            width: 3px;
        }
        .auto-style41 {
            width: 422px;
            height: 242px;
        }
        .auto-style42 {
            padding-bottom: 3px;
            text-align: center;
            width: 630px;
        }
        .auto-style46 {
            width: 118px;
        }
        .auto-style47 {
            background-color: #DDDDDD;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 116px;
        }
        .auto-style48 {
            background-color: #DDDDDD;
            font-family: Verdana, Arial;
            font-size: 8pt;
            white-space: nowrap;
            text-align: right;
            width: 116px;
        }
               .auto-styleHDG {
            font-family: Verdana, Arial, Bold;
            font-size: 11pt;
            white-space: nowrap;
            text-align: right;
            width: 116px;
        }

        </style>
</asp:Content>



