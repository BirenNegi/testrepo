<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ViewBudgetsPage" Title="Project Budget" Codebehind="ViewBudgets.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar1" runat="server" Title="Budget" />

   <%-- San --%>
 <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.22/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>
 <script src="../../JavaScripts/table2excel.js" type="text/javascript"></script>

<script type="text/javascript">

    $("body").on("click", "#btnExport", function () {
        $("[id*=gvExportBudget]").table2excel({
            filename: "BudgetSummary.xls"
        });
    });

   //$("body").on("click", "#btnExport", function () {
        
   //     html2canvas($('[id*=gvExportBudget]')[0], {
   //         onrendered: function (canvas) {
   //             var data = canvas.toDataURL();
   //             var docDefinition = {
   //                 content: [{
   //                     image: data,
   //                     width: 500
   //                 }]
   //             };
   //             pdfMake.createPdf(docDefinition).download("Table.pdf");
   //         }
        
   //     });
   // });
</script>



  <%-- San --%>


<table>
    <tr>
        <td><asp:Image ID="imgBOQ" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">BOQ</td>
    </tr>
</table>
         
<asp:Panel ID="pnlBOQ" runat="server" CssClass="collapsePanel" Height="0">
    <table>
        <tr>
            <td class="frmText">Budget file:</td>
            <td><input id="fileTrades" type="file" size="40" name="fileTrades" runat="server" /></td>
            <td><asp:linkbutton id="cmdUploadFile" CssClass="frmButton" runat="server" OnClick="cmdUpload_Click">Import</asp:linkbutton></td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upBOQ" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hidSelectedId" runat="server" />
            <sos:BalanceInclude id="sbiBOQ" runat="server"></sos:BalanceInclude>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="frmForm">
                        <table>
                            <tr>
                                <td>
                                    <table id="tblTrades" cellpadding="4" cellspacing="1" runat="server">
                                        <tr>
                                            <td class="lstHeader">Trade</td>
                                            <td class="lstHeader" align="right">Amount $</td>
                                            <td class="lstHeader" align="right" id="tdBalance" runat="server">Balance $</td>
                                            <td class="lstHeader" colspan="2"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<br />

<asp:Panel id="pnlViewSummary" runat="server" Visible="false">
    <table>
        <tr>
            <td><asp:Image ID="imgSummary" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
            <td class="lstTitle">Budget Summary</td>
        </tr>
    </table>
    <asp:Panel ID="pnlSummary" runat="server" CssClass="collapsePanel" Height="0">
        <asp:UpdatePanel ID="upSummary" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
                <table><tr><td>
                <sos:BalanceInclude id="sbiSummary" runat="server"></sos:BalanceInclude>  </td>
                    
                    <%-- San --%>
                         <td align="left"> 
                               <input type="button" id="btnExport" value="Export to Excel " />

                            
                            <div style="display:none">
                                

                                <asp:GridView ID="gvExportBudget" runat="server"></asp:GridView> 

                               
                            </div>
                    
                            </td>
                      <%-- San --%>


                       </tr></table>
                <table id="tblSummary" cellpadding="4" cellspacing="1" runat="server">
                    <tr>
                        <td class="lstHeader">Trade</td>
                        <td class="lstHeader">Name</td>
                        <td class="lstHeader">BOQ</td>
                        <td class="lstHeader">Client<br />Variations</td>
                        <td class="lstHeader">Separate<br />Accounts</td>
                        <td class="lstHeader">Contracts</td>
                        <td class="lstHeader">Variations</td>
                        <td class="lstHeader">Balance</td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
</asp:Panel>

<table>
    <tr>
        <td><asp:Image ID="imgDetails" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Budget Details</td>
    </tr>
</table>

<asp:Panel ID="pnlDetails" runat="server" CssClass="collapsePanel" Height="0">
    <asp:UpdatePanel ID="upDetails" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td class="frmTextBold">Trade:</td>
                    <td class="frmText"><asp:DropDownList ID="ddlTrades" runat="server" AutoPostBack="true"></asp:DropDownList></td>
                    <td class="frmText">&nbsp;&nbsp;</td>
                    <td><sos:BalanceInclude id="sbiDetails" runat="server"></sos:BalanceInclude></td>
                </tr>
            </table>
            <table id="tblDetails" cellpadding="4" cellspacing="1" runat="server">
                <tr>
                    <td class="lstHeader">Date</td>
                    <td class="lstHeader">Type</td>
                    <td class="lstHeader">Description</td>
                    <td class="lstHeader">Amount</td>
                    <td class="lstHeader" id="cellBalance">Balance</td>
                </tr>
            </table>

            <%-- San --%>
            <%--Release 11 --Rooled Back--Budget In and Budget Out 
                <br\ /><br\ /><br /><p />
            <table id="tblDetailsIn" cellpadding="4" cellspacing="1" runat="server">
                <tr>
                    <td class="lstHeader">Date</td>
                    <td class="lstHeader">Type</td>
                    <td class="lstHeader">Description</td>
                    <td class="lstHeader">Amount</td>
                    <td class="lstHeader" id="cellBalanceIn">Current </td>
                </tr>
            </table>
            <br\ /><br\ /><br /><p />

            <table><tr><td class="frmTextBold"> Budget source Type:</td>
                <td><asp:DropDownList ID="ddlBudgetsurceType" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="All"> </asp:ListItem>
                    <asp:ListItem>BOQ</asp:ListItem>
                    <asp:ListItem>CV</asp:ListItem>
                    <asp:ListItem>SA</asp:ListItem>
                    </asp:DropDownList></td></tr></table>
            
             <table id="tblDetailsOut" cellpadding="4" cellspacing="1" runat="server">
                <tr>
                    <td class="lstHeader">Date</td>
                    <td class="lstHeader">Type</td>
                    <td class="lstHeader">Description</td>
                    <td class="lstHeader">BudgetType</td>
                    <td class="lstHeader">BudgetCode</td>
                    <td class="lstHeader">Original</td>
                    <td class="lstHeader">Contract Amount</td>
                    <td class="lstHeader">Allocated</td>
                    <td class="lstHeader">Win/Loss</td>
                    <td class="lstHeader">Unallocated</td>
                    <td class="lstHeader">Current</td>
                    
                </tr>
            </table>
                      --%>
            <%-- San --%>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<br />

<act:CollapsiblePanelExtender
    ID="cpe" 
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlBOQ"
    ExpandControlID="imgBOQ"
    CollapseControlID="imgBOQ"
    ImageControlID="imgBOQ"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

<act:CollapsiblePanelExtender
    ID="cpe1"
    runat="Server"
    Collapsed="False"
    CollapsedSize="0"
    TargetControlID="pnlSummary"
    ExpandControlID="imgSummary"
    CollapseControlID="imgSummary"
    ImageControlID="imgSummary"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

<act:CollapsiblePanelExtender
    ID="cpe2"
    runat="Server"
    Collapsed="False"
    CollapsedSize="0"
    TargetControlID="pnlDetails"
    ExpandControlID="imgDetails"
    CollapseControlID="imgDetails"
    ImageControlID="imgDetails"
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"         
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>

<act:UpdatePanelAnimationExtender ID="upaeBOQ" runat="server" TargetControlID="upBOQ">
    <Animations>
        <OnUpdated>
            <FadeIn AnimationTarget="tblTrades" Duration="0.5" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:UpdatePanelAnimationExtender ID="upaeSummary" runat="server" TargetControlID="upSummary">
    <Animations>
        <OnUpdated>
            <FadeIn AnimationTarget="tblSummary" Duration="0.5" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:UpdatePanelAnimationExtender ID="upaeDetails" runat="server" TargetControlID="upDetails">
    <Animations>
        <OnUpdated>
            <FadeIn AnimationTarget="tblDetails" Duration="0.5" Fps="15" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

</asp:Content>
