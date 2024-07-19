<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditSiteOrderNS" Title="Orders" Codebehind="EditSiteOrderNS.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:PlaceHolder ID="phAddNew" runat="server"></asp:PlaceHolder>
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <sos:TitleBar ID="TitleBar" runat="server" Title="Site Orders" />

<asp:Panel ID="pnlForm" runat="server" Height="1563px" Width="900px" CssClass="auto-style43">
   <table cellspacing="0" cellpadding="0">

   </table>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblMessage" runat="server" Visible="False"> </asp:Label>
            </td>

        </tr>
        <tr>
 
            <td class="auto-styleBTN">
                <asp:LinkButton ID="cmdUpdateTop" runat="server" CssClass="frmButton" OnClick="cmdUpdate_Click">Save</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="cmdCancelTop" runat="server" CausesValidation="False" CssClass="frmButton" UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to exit without saving and lose your information?');" OnClick="cmdCancel_Click">Exit</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td class="auto-style2">
                <table class="auto-style39">
                    <tr>
                        <td class="frmSubSubTitle" colspan="4">General Information</td>
                       
                    </tr>
                    <tr>
                        <td class="auto-styleLH1">Title:</td>
                        <td colspan="2" class="auto-styleTI">
                            <asp:TextBox ID="txtTitle" runat="server" Width="400px"></asp:TextBox>
                       </td>
                         <td colspan="2" class="auto-styleTI">
                            <asp:DropDownList ID="ddlCodeType" runat="server" AutoPostBack="false" Width="150px"> </asp:DropDownList>
                            <asp:TextBox ID="txtTypeInfo" runat="server" Width="200px"></asp:TextBox>
                       </td>
                        <%--    <td>&nbsp;</td>/>--%>
                    </tr>
                    <tr>
                        <td class="auto-styleLHL">Order Type:</td>
                        <td class="auto-style42">
                            <asp:TextBox ID="txtTyp_Desc" runat="server" Height="18px" Width="176px" BackColor="#DDDDDD" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLHL">Date:</td>
                        <td class="auto-style41">
                            <asp:TextBox ID="txtDate" runat="server" Height="18px" Width="150px" BackColor="#DDDDDD" ReadOnly="True"></asp:TextBox>
                        <%--    <sos:DateReader ID="sdrDate" runat="server" />--%>

                        </td>
                    </tr>
                    <tr>
                        <td class="auto-styleLH1">Provider:</td>
                        <td class="auto-style42">
                            <asp:TextBox ID="txtName" runat="server" Width="300px" OnTextChanged="txtSubContractorName_TextChanged"></asp:TextBox>
                             <input id="txtSubContractorId" type="hidden" runat="server" onchange="txtSubContractorId_Changed" />
                          
                        </td>
                        <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLHL">Foreman:</td>
                        <td class="auto-style41">
                            <asp:TextBox ID="txtPeopleForeman" runat="server" BackColor="#DDDDDD" ReadOnly="True" Width="300px"></asp:TextBox>
                            <%-- <asp:HyperLink ID="cmdSelPeople" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink> --%>
                            <input id="txtForemanId" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-styleLHL">ABN:</td>
                        <td class="auto-style42">
                            <asp:TextBox ID="txtABN" runat="server" Height="18px" Width="176px" BackColor="#DDDDDD" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="auto-styleMid"></td>
                        <td class="auto-styleLHL">Given By:</td>
                        <td class="auto-style41">
                            <asp:TextBox ID="txtGivenByName" runat="server" Width="300" BackColor="#DDDDDD" ReadOnly="True"></asp:TextBox>
                             <input id="txtGivenBy" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-styleLHL">Site Street:</td>
                        <td class="auto-style42">
                            <asp:TextBox ID="txtStreet" runat="server" Height="16px" Width="300px"></asp:TextBox>
                        </td>
                        <td class="auto-styleMid"></td>
                       <td class="auto-styleLHL">&nbsp;</td>
                        <td class="auto-style41">
                             <input id="txtContactId" type="hidden" runat="server" />
                        </td>

                    </tr>
                    <tr>
                        <td class="auto-styleLHL">Site Suburb:</td>
                        <td class="auto-style42">
                            <asp:TextBox ID="txtLocality" runat="server" Height="18px" Width="176px"></asp:TextBox>
                        </td>
                        <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLHL">Order Items:</td>
                        <td class="auto-style41">
                             <input id="Hidden1" type="hidden" runat="server" />
                            <asp:TextBox ID="txtItemsTotal" runat="server" BackColor="#DDDDDD" ReadOnly="True" style="text-align: right" Width="150"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                       <td class="auto-styleLHL">Site State/Postal:</td>
                        <td class="auto-style42">
                            <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="false" Height="18px" Width="111px">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtPostal" runat="server" Height="18px" Width="176px"></asp:TextBox>
                        </td>
                        <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLH1">Budget Code:</td>
                        <td class="auto-style41">
                            <asp:DropDownList ID="ddTradesCode" runat="server" AutoPostBack="false" Width="300px"> </asp:DropDownList>
                            <input id="txtTradesCode" type="hidden" runat="server" />
                         </td>
                    </tr>
                    <tr>
                        <td class="auto-styleLHL">Contact:</td>
                        <td class="auto-style42">
                            <asp:TextBox ID="txtContact" runat="server" Width="300px"></asp:TextBox>
                        </td>
                        <td class="auto-styleMid"></td>
                        <td class="auto-styleLHL">SUB-TOTAL</td>
                        <td class="auto-style41">
                            <asp:TextBox ID="txtSubTotal" runat="server" style="text-align: right" Width="150"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td class="auto-styleLH1">Email:</td>
                        <td class="auto-styleTXT">
                            <asp:TextBox ID="txtEmail" runat="server" Height="16px" Width="300px"> </asp:TextBox>
                        </td>
                        <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLHL"></td>
                        <td class="auto-style41">
                        </td>
                    </tr>
                   <tr>
                        <td class="auto-styleLHN">Description (Visible Externally):</td>
                         <td class="auto-style42">
                        <asp:TextBox ID="txtNotes" runat="server" Width="300px" Height="60" TextMode="MultiLine" Rows="5"></asp:TextBox>
                             </td>
                        <td class="auto-styleMid">&nbsp;</td>
                        <td class="auto-styleLHL">Comments (Visible Internally):</td>
                        <td class="auto-style42">
                        <asp:TextBox ID="txtComments" runat="server" Width="300px" Height="60" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            &nbsp;</td>
                    </tr>
  

                    <%-- San --%>
                    <%-- San --%>
                    <%-- San New role and processSteps for COM  and JCA--%>
                    <%---------- San ------------%>
                </table>
            </td>
        </tr>
          
        <tr>
            <td class="auto-styleBTN">
                <asp:LinkButton ID="cmdUpdateBottom" runat="server" CssClass="frmButton" OnClick="cmdUpdate_Click">Save</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton ID="cmdCancelBottom" runat="server" CausesValidation="False" CssClass="frmButton" UseSubmitBehavior="true" OnClientClick="return confirm('Are you sure you want to exit without saving and lose your information?');" OnClick="cmdCancel_Click">Exit</asp:LinkButton>
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
            width: 780px;
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
            width: 883px;
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
            background-color: #EEEEEE;
        }

        .auto-styleMid {
            width: 3px;
        }
        .auto-style41 {
            width: 339px;
            background-color: #EEEEEE;
        }
       .auto-styleTI {
            background-color: #EEEEEE;
        }
        .auto-style42 {
            width: 331px;
            background-color: #EEEEEE;
        }
        .auto-style43 {
            margin-right: 0px;
        }
        </style>
</asp:Content>



