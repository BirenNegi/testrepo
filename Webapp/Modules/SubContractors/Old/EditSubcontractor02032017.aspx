<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditSubContractorPage" Title="Edit SubContractor" Codebehind="EditSubcontractor.aspx.cs" %>



    




<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<%-- SAN --%>
    <script language="javascript";type="text/javascript">
        function AddvalueControl(control)
        {
           //alert("Hisan");
            
           // var ctrl = document.getElementById('MainContent_FileUpload1');
           // alert(ctrl);
           // ctrl.value= "xyz";
           // document.getElementById('MainContent_FileUpload1').value = "ABC";
           // alert("123");
           // alert(document.getElementById('MainContent_FileUpload1').value);

        }
        function PreQualificationchange(fileCtrl)
        {
            var appname = window.navigator.appName;
            
            var ctrl = document.getElementById(fileCtrl);
            //alert(ctrl.value.length);
            if (ctrl.value.length > 0) {
                if (appname == "Microsoft Internet Explorer")
                {  //alert(appname);
                    ctrl.style.width = 350;                 
                }
                else{ctrl.style = "width:350";}
                
            }
            else {
                ctrl.style.width = 86;
                ctrl.style = "86";
            }
        }

        function hidePI(x)
        {
            var lbl = document.getElementById('MainContent_lblProfessionalIndemnity');
            var fileupload=document.getElementById('MainContent_FuPIinsurance');
            
            if (x != "") {
                
                lbl.style.display = "block";
                //lbl.style.width = "200px"; 
                fileupload.style.width = "600px";//visibility = "hidden";
            }
            else {
                //alert(x);
                //lbl.style.display = "block";//visibility = "visible";
                //lbl.style.textAlign = "left";
                fileupload.style.width = "86px";
            }
        }

</script>

    <%-- SAN --%>


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
                    <td class="frmReqLabel">Name:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtName" runat="server" Width="185"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Fax:</td>
                    <td><asp:TextBox ID="txtFax" runat="server" Width="185"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">ShortName:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valShortName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br>" ControlToValidate="txtShortName"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtShortName" runat="server" Width="185"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Address:</td>
                    <td><asp:TextBox ID="txtStreet" runat="server" Width="185"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmReqLabel">Business Unit:</td>
                    <td>
                        <asp:RequiredFieldValidator ID="valBusinessUnit" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="ddlBusinessUnit"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="ddlBusinessUnit" runat="server"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Suburb:</td>
                    <td><asp:TextBox ID="txtLocality" runat="server" Width="185"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">ABN:</td>
                    <td><asp:TextBox ID="txtAbn" runat="server" Width="185"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">State:</td>
                    <td><asp:DropDownList ID="ddlState" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Account:</td>
                    <td><asp:TextBox ID="txtAccount" runat="server" Width="185"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Postal Code:</td>
                    <td><asp:TextBox ID="txtPostalCode" runat="server" Width="185"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="frmLabel">Phone:</td>
                    <td><asp:TextBox ID="txtPhone" runat="server" Width="185"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Website:</td>
                    <td><asp:TextBox ID="txtWebsite" runat="server" Width="185"></asp:TextBox> </td>
                </tr>
                <%-- San --%>
                 <tr>
                    <td class="frmLabel" valign="middle">Prequalification form:</td>
                    <td colspan="4">
                        <asp:Label ID="lblPrequalification" runat="server" class="frmLabel" Height="23px"></asp:Label>&nbsp;<asp:FileUpload ID="FileUpload1" runat="server" Width="458px" />
                    </td>
                </tr>

                <tr>
                    <td class="frmLabel" valign="middle">Public liability Insurance:</td>
                    <td colspan="4">
                          <asp:Label ID="lblPublicliability" runat="server" class="frmLabel" Height="23px"></asp:Label>&nbsp;<asp:FileUpload ID="FuPublicliability" runat="server" Width="458px" />
                    </td>
                </tr>

                <tr>
                    <td class="frmLabel" valign="middle">Workcover Insurance:</td>
                    <td colspan="4">
                        <asp:Label ID="lblWorkcover" runat="server" class="frmLabel" Style="text-align:left;" Height="23px"></asp:Label>&nbsp;<asp:FileUpload ID="FuWorkcover" runat="server" Width="458px" />
                    </td>
                </tr>
                
               
                <tr>
                    <td class="frmLabel" valign="middle">D&amp;C Contractor ? </td>
                    <td colspan="4">
                        <asp:CheckBox ID="chkDc" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td class="frmLabel" valign="middle">PI Insurance:</td>
                    <td colspan="4"> <asp:Label ID="lblProfessionalIndemnity" runat="server" class="frmLabel" Style="text-align:left;" Height="23px"></asp:Label>
                        <asp:FileUpload ID="FuPIinsurance" runat="server" Width="458px"  onchange="hidePI(this.value);"/>
                    </td>
                </tr>

                 <%-- San --%>
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td colspan="4"><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" Width="480"></asp:TextBox></td>
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
