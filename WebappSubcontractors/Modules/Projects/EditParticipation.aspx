<%@ Page Language="C#" MasterPageFile="~/MasterPages/Subbie.master" AutoEventWireup="True" Inherits="SOS.Web.EditParticipationPage" Title="Quote" Codebehind="EditParticipation.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <sos:TitleBar ID="TitleBar" runat="server" Title="Editing Quote" Visible="false" />

    <script language="javascript" type="text/javascript">
        function Check(rdo)
        { //alert(rdo.value + "--" + rdo.id);
             var str = rdo.id;
             var str = str.substring(28, str.length - 2);
             // alert(str);
             checkAll(rdo.value, str);
        }

     function CheckSan(rdolist) {
         alert(rdolist.id + "---" + rdolist);

     }


     function checkAll(val, ID)
     {
        // alert("1");
         var radioBtns = document.getElementsByTagName('INPUT');

             for (i = 0; i < radioBtns.length; i++)
             {
                     if ((radioBtns[i].type == "radio") && (radioBtns[i].value == val) && (radioBtns[i].id.indexOf(ID) > 0))
                     {
                        
                         radioBtns[i].checked = true;
                     }
             }
     }




</script>



<asp:Panel ID="pnlProposal" runat="server">
<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmText">
            <b>Notes:</b>
            <table>
                <tr>
                    <td>Total quote equals base quote plus confirmed item prices.</td>
                </tr>
                <tr>
                    <td><b>Y</b> must be selected for confirmed items.</td>
                </tr>
                <tr>
                    <td><span class="auto-style1">*</span> denotes compulsory items. Enter &quot;0&quot; to items not included marked as &quot;N&quot;.</td>
                </tr>
                <tr>
                    <td>Comments for specific items can be provided by clicking on the Y/N column in the previous screen.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
             <table cellspacing="0" cellpadding="0">
                <tr>
                    <td class="frmForm">
                        <table>
                            <tr>
                                <td class="frmLabel">Base Quote $:</td>
                                <td class="frmData">
                                    <asp:CompareValidator ID="valQuoteAmount" runat="server" Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ErrorMessage="Invalid number!<br />" ControlToValidate="txtQuoteAmount"></asp:CompareValidator>
                                    <asp:TextBox ID="txtQuoteAmount" runat="server" Width="150"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="frmFormBelow"><sos:EditComparison ID="EditComparison1" runat="Server"></sos:EditComparison></td>
                </tr>
                <tr>
                    <td class="frmFormBelow">
                        <table>
                            <tr>
                                <td class="frmLabel" valign="top">Comments:</td>
                                <td class="frmData"><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="8" Width="480"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="frmFormBelow">
                        <table id="tblDrawings" runat="server" cellpadding="4" cellspacing="2">
                            <tr>
                                <td class="lstHeader">Drawing</td>
                                <td class="lstHeader">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>Revision</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Button id="cmdSelectLatestDrawings" runat="server" Text="Assign Latest" CssClass="frmTextSmall"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
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
<br />
</asp:Panel>
<br />
    
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            color: #FF3300;
            font-weight:bolder;
        }
    </style>
</asp:Content>

