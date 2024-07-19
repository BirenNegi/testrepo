<%@ Page Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" Inherits="SOS.Web.ListDrawingsTransmittalsPage" Title="Drawings/Transmittals" Codebehind="ListDrawingsTransmittals.aspx.cs" %>

<asp:Content ID="Content" ContentPlaceHolderID="Scripts" runat="server">
    <asp:Literal ID="litScriptDeepZoom" runat="server"></asp:Literal>



</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <act:toolkitscriptmanager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" CombineScripts="false" />
<script src="../../JQuery/jquery-1.7.1.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

    function UpdateDownloadInfo()
    {
        
       
        totalSize = 0;
        numFiles = 0;
        Ids = "";
       // alert(arrayLinks.length);

        for (var i = 0; i < arrayLinks.length; i++)
        {
            //alert(arrayLinks[i]);
            //alert($(#arrayLinks[i]).prop("checked));
            if ($(arrayLinks[i]).prop("checked"))
            {
                //alert(numFiles);
                Ids = Ids == "" ? arrayIds[i] : Ids + "," + arrayIds[i];
                totalSize = totalSize + arraySizes[i];
                numFiles++;
            }
        }

       

        if (numFiles > 1)
        {
            //alert(numFiles)
            if (totalSize <= maxSize)
            {
                $(lnkDownloadId).prop("href", lnkDownloadURL + Ids);
                $(lnkDownloadId).prop("title", "Download");
                $(lnkDownloadId).show("slow")
                $(lblErrorId).hide("slow");
            }
            else
            {
                $(lnkDownloadId).hide("slow");
                $(lblErrorId).show("slow");
            }
                        
            $(lblSelectedId).text(numFiles);
            $(lblSizeId).text(FormatFileSize(totalSize));
        }
        else
        {
            $(lnkDownloadId).hide("slow");
            $(lblErrorId).hide("slow");
            $(lblSelectedId).text("");
            $(lblSizeId).text("");
            $(lnkDownloadId).prop("href", "");
            $(lnkDownloadId).prop("title", "");
        }
    }


</script>




<sos:TitleBar ID="TitleBar1" runat="server" Title="Project Drawings and Transmittals" />

 <table><tr valign="top"> 
     <td id="drawings">
         <table>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="lstTitle">Drawings</td>
                    <td>&nbsp;&nbsp;</td>
                    <td class="frmText">&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>

         <table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="aupDrawings" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <table cellpadding="2" cellspacing="0" border="0" style="height:36px;">
                                    <tr>
                                        <td class="frmSubTitle">Download Multiple Drawings</td>
                                        <td>&nbsp;</td>
                                        <td class="frmText">Selected:</td>
                                        <td class="frmSubTitle"><asp:Label ID="lblSelected" runat="server"></asp:Label></td>
                                        <td>&nbsp;</td>
                                        <td class="frmText">Total Size:</td>
                                        <td class="frmSubTitle"><asp:Label ID="lblSize" runat="server" CssClass="frmSubTitle"></asp:Label></td>
                                        <td class="frmError"><asp:Label ID="lblSizeError" runat="server"></asp:Label></td>
                                        <td>&nbsp;</td>
                                        <td><asp:HyperLink ID="lnkDownloadAll" runat="server"><asp:Image ID="imglnkDownloadAll" runat="server" ImageUrl="~/Images/IconDownload.gif" /></asp:HyperLink></td>
                                    </tr>
                                </table>        
                            </td>
                        </tr>
                    </table>

                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <sos:GridPageSize ID="GridPageSizeDrawings" runat="server" Visible="true" /><%----%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <%-- <div style="height:700px;overflow:auto">--%>
                                          <asp:GridView 
                                    OnPageIndexChanging="gvDrawings_OnPageIndexChanging"
                                    OnSorting="gvDrawings_OnSorting"
                                    OnRowCreated="gvDrawings_OnRowCreated"
                                    OnRowDataBound="gvDrawings_OnRowDataBound"
                                    ID="gvDrawings"
                                    runat="server"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="1000"
                                    PagerStyle-CssClass="lstPager"
                                    DataKeyNames="Id"
                                    AutoGenerateColumns="False" 
                                    CellPadding="2" 
                                    CellSpacing="0" 
                                    CssClass="lstList" 
                                    RowStyle-CssClass="lstItem" 
                                    AlternatingRowStyle-CssClass="lstAltItem">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" ToolTip="Select All" OnClick="SelectAllDrawings()" />
                                            </HeaderTemplate>
                                            <ItemTemplate>                                    
                                                    <asp:CheckBox ID="chkSelect" runat="server" OnClick="UpdateDownloadInfo()" Visible='<%#FormatFileLink(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" ImageUrl="~/Images/IconDocument.gif" ToolTip="Download" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowDrawingRevision.aspx?DrawingRevisionIds={0}", Eval("LastRevisionIdStr"))%>' Visible='<%#FormatFileLink(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField SortExpression="DrawingType" HeaderText="Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("DrawingType")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField SortExpression="Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("Name")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField SortExpression="LastRevisionNumber" HeaderText="Revision" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("LastRevisionNumber")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField SortExpression="LastRevisionDate" HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#SOS.UI.Utils.SetFormDate(SOS.Data.Utils.GetDBDateTime(Eval("LastRevisionDate")))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="File Size" SortExpression="LastRevisionFileSize" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <span class="<%#(String)Eval("CssClass")%>">
                                                <%#FormatFileSize(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--</div>--%>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
    </td>
    
     
      
    <td id="Transmittals" style="padding-left:20px;">
      <table>
    <tr>
        <td class="lstTitle">Transmittals</td>
    </tr>
</table>
        <table style="height:36px;"><tr><td></td></tr></table>
      <table cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="aupTransmittals" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <sos:GridPageSize ID="GridPageSizeTransmittals" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView 
                                    OnPageIndexChanging="gvTransmittals_OnPageIndexChanging"
                                    OnSorting="gvTransmittals_OnSorting"
                                    OnRowCreated="gvTransmittals_OnRowCreated"
                                    ID="gvTransmittals"
                                    runat="server"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="10"
                                    PagerStyle-CssClass="lstPager"
                                    DataKeyNames="Id"
                                    AutoGenerateColumns="False" 
                                    CellPadding="2" 
                                    CellSpacing="0" 
                                    CssClass="lstList" 
                                    RowStyle-CssClass="lstItem" 
                                    AlternatingRowStyle-CssClass="lstAltItem">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" ToolTip="View" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowTransmittal.aspx?TransmittalId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField SortExpression="Number" HeaderText="Number" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#SOS.Data.Utils.GetDBInt32(Eval("Number"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Title" HeaderStyle-CssClass="lstHeader"></asp:BoundField>

                                        <asp:TemplateField SortExpression="TransmissionDate" HeaderText="Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#SOS.UI.Utils.SetFormDate(SOS.Data.Utils.GetDBDateTime(Eval("TransmissionDate")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="# Drawings" SortExpression="NumDrawings" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#SOS.UI.Utils.SetFormInteger(SOS.Data.Utils.GetDBInt32(Eval("NumDrawings")))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
    </td>       

   </tr></table>   



<br />







<%--

<table>
    <tr>
        <td><asp:Image ID="imgProposal" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
        <td class="lstTitle">Proposal</td>
    </tr>
</table>

<asp:Panel ID="pnlProposal" runat="server" CssClass="collapsePanelProposal" Height="0">
<table>
    <asp:Panel ID="pnlCopyDrawings" runat="server" Visible="false">
    <tr>
        <td colspan="3"><asp:LinkButton id="cmdCopyDrawings" runat="server" CssClass="frmButton" OnClick="cmdCopyDrawings_Click" OnClientClick="javascript:return confirm('Copy to active drawing register ?');">Copy to active drawing register</asp:LinkButton></td>
    </tr>
    </asp:Panel>
    
    <tr>
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkDrawingsProposal" runat="server" ToolTip="Latest Drawings" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td>
                        <asp:TreeView ID="tvDrawingsProposal" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Drawings"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLink" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
        <td>&nbsp;</td>
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkTransmittalsProposal" runat="server" ToolTip="Transmittals Summary" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td valign="top">
                        <asp:PlaceHolder ID="phAddTransmittalProposal" runat="server" Visible="false">
                        &nbsp;
                        <asp:HyperLink ID="lnkAddTransmittalProposal" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink>
                        </asp:PlaceHolder>
                    </td>
                    <td>
                        <asp:TreeView ID="tvTransmittalsProposal" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Transmittals"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstSubTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>--%>
</asp:Panel>
<br />

<asp:Panel ID="pnlStatusActive" runat="server">
<%--<table>
    <tr>
        <asp:Panel ID="pnlDeepZoom" runat="server">
            <td valign="top">
                <div id="divDeepZoom" runat="server" class="DeepZoom" visible="false"></div>
                <act:Seadragon ID="actSeadragon" runat="server" CssClass="DeepZoom" Visible="false"></act:Seadragon>
            </td>
            <td>&nbsp;</td>
        </asp:Panel>
        
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkDrawings" runat="server" ToolTip="Latest Drawings" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td>
                        <asp:TreeView ID="tvDrawings" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Drawings"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                                <asp:TreeNodeStyle CssClass="lstLink" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
        <td>&nbsp;</td>
        <td valign="top">
            <table>
                <tr>
                    <td valign="top"><asp:HyperLink ID="lnkTransmittals" runat="server" ToolTip="Transmittals Summary" ImageUrl="~/Images/IconPrint.gif" CssClass="frmLink"></asp:HyperLink></td>
                    <td valign="top">
                        <asp:PlaceHolder ID="phAddTransmittal" runat="server" Visible="false">
                        &nbsp;
                        <asp:HyperLink ID="lnkAddTransmittal" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink>
                        </asp:PlaceHolder>
                    </td>
                    <td>
                        <asp:TreeView ID="tvTransmittals" runat="server" ShowLines="true" >
                            <Nodes>
                                <asp:TreeNode Expanded="True" SelectAction="None" Text="Transmittals"></asp:TreeNode>
                            </Nodes>
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="lstTitle" />
                                <asp:TreeNodeStyle CssClass="lstSubTitle" />
                                <asp:TreeNodeStyle CssClass="lstLinkBig" />
                            </LevelStyles>
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>--%>
<br />
</asp:Panel>

<%--<act:CollapsiblePanelExtender
    ID="cpe1"
    runat="Server"
    Collapsed="True"
    CollapsedSize="0"
    TargetControlID="pnlProposal"
    ExpandControlID="imgProposal"
    CollapseControlID="imgProposal"
    ImageControlID="imgProposal"         
    ExpandedImage="~/Images/IconCollapse.jpg"
    CollapsedImage="~/Images/IconExpand.jpg"
    ExpandDirection="Vertical">
</act:CollapsiblePanelExtender>--%>

</asp:Content>
