<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditComparisonPage" Title="Comparison" Codebehind="EditComparison.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    
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
<sos:TitleBar ID="TitleBar" runat="server" Title="Updating Comparison" />

<asp:Panel ID="pnlProposal" runat="server">
<table cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <sos:CheckComparison ID="CheckComparison1" runat="Server"></sos:CheckComparison>        
        </td>    
    </tr>
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <sos:EditComparison ID="EditComparison1" runat="Server"></sos:EditComparison>
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
