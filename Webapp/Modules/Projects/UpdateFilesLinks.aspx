<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.UpdateFilesLinksPage" Title="Project Files' Links Update" Codebehind="UpdateFilesLinks.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
<sos:TitleBar ID="TitleBar" runat="server" Title="Project Files' Links Update" />

<asp:Timer ID="tmrProgress" runat="server" Interval="500" OnTick="tmrProgress_Tick" Enabled="false"></asp:Timer>

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table>
                <tr>
                    <td class="frmLabel" id="tdProject" runat="server">Project:</td>
                    <td><asp:DropDownList ID="ddlProjects" runat="server" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />

<asp:UpdatePanel ID="upProgress" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="tmrProgress" EventName="Tick" />
    </Triggers>

    <ContentTemplate>
        <table cellspacing="0" cellpadding="0">
            <tr>
                <td class="frmTitle">Project files</td>
            </tr>

            <asp:Panel ID="pnlFiles" runat="server" Visible="false">
            <tr>
                <td>
                    <asp:UpdatePanel ID="upFiles" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="tmrProgress" EventName="Tick" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:GridView
                                ID = "gvFiles" runat = "server" AllowPaging="true" AllowSorting="true" EmptyDataText="No files" AutoGenerateColumns = "False"
                                CellPadding = "4" CellSpacing = "0" PageSize = "25" OnPageIndexChanging="gvFiles_OnPageIndexChanging" OnSorting="gvFiles_OnSorting"
                                HeaderStyle-CssClass = "lstHeader" RowStyle-CssClass = "lstItem" AlternatingRowStyle-CssClass = "lstAltItem" PagerStyle-CssClass = "lstPager" EmptyDataRowStyle-CssClass = "lstSubTitle">
                                <Columns>
                                    <asp:BoundField DataField="No" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                                    <asp:BoundField DataField="Entity" SortExpression="Entity" HeaderText="Entity"></asp:BoundField>
                                    <asp:BoundField DataField="Attachment" SortExpression="Attachment" HeaderText="Attachment"></asp:BoundField>
                                    <asp:BoundField DataField="File" SortExpression="File" HeaderText="File"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            </asp:Panel>
            
        </table>
        <br />

        <table cellspacing="0" cellpadding="0">
            <tr>
                <td class="frmForm">
                    <table>
                        <tr>
                            <td class="frmReqLabel">Current base path:</td>
                            <td class="frmText" colspan="4">
                                <asp:RequiredFieldValidator ID="valName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtCurrentPath"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtCurrentPath" runat="server" Width="480"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="frmLabel">New base path:</td>
                            <td class="frmText" colspan="4"><asp:TextBox ID="txtNewPath" runat="server" Width="480"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <asp:Panel ID="pnlProgress" runat="server" Visible="false">
            <tr>
                <td class="frmFormBelow" align="center">
                    <div style="width:100%; position:relative;">
                        <div id="divProgressBar" runat="server" style="position:absolute; top:0; left:0; width:0%;"></div>
                        <div id="divProgressText" runat="server" style="width:100%; position:relative;"></div>
                    </div>
                </td>
            </tr>
            </asp:Panel>

            <tr><td>&nbsp;</td></tr>
            <tr>
                <td class="frmBottomBox"><asp:linkbutton id="cmdUpdatePath" CssClass="frmButton" runat="server" OnClick="cmdUpdatePath_Click">Update base path</asp:linkbutton>&nbsp;&nbsp;</td>
            </tr>
        </table>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
