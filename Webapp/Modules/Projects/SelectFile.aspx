<%@ Page Language="C#" AutoEventWireup="true" Inherits="SOS.Web.SelectFilePage" Title="File" Codebehind="SelectFile.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select File</title>
    <link href="../../Config/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>    

    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />

    <sos:TitleBar ID="TitleBar1" runat="server" />

    <table cellspacing="1" cellpadding="1">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="frmTextSmall">System Path:</td>
                        <td class="frmTextSmall"><asp:Label ID="lblSystemPath" runat="server"></asp:Label></td>
                    </tr>
                    
                    <asp:Panel ID="pnlProjectPath" runat="server">
                    <tr>
                        <td class="frmTextSmall">Project Path:</td>
                        <td class="frmTextSmall"><asp:Label ID="lblProjectPath" runat="server"></asp:Label></td>
                    </tr>
                    </asp:Panel>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="1" cellpadding="1">
                    <tr>
                        <td class="frmText"><asp:HyperLink ID="lnkNoneSelected" runat="server" Text="None Selected" CssClass="frmLink"></asp:HyperLink></td>
                            
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td class="frmText"><a class="frmLink" href="#" onclick="window.close();">Cancel</a></td>
                        
                        <asp:Panel ID="pnlFolder" runat="server" Visible="false">
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td class="frmText">
                                <asp:UpdatePanel ID="aupFolder" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="tvFiles" EventName="SelectedNodeChanged" />
                                    </Triggers>
                                    
                                    <ContentTemplate>
                                        <asp:HyperLink ID="lnkThisSelected" runat="server" Text="Select current" CssClass="frmLink" Visible="false"></asp:HyperLink>
                                        <asp:Label ID="lblThisSelected" runat="server" Text="Select current" Visible="false"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </asp:Panel>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="aupFiles" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="tvFiles" EventName="SelectedNodeChanged" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:TreeView ID="tvFiles" runat="server" ShowExpandCollapse="false" ShowLines="true" OnSelectedNodeChanged="tvFiles_SelectedNodeChanged" >
                            <LevelStyles>
                                <asp:TreeNodeStyle CssClass="frmTextBold" />
                                <asp:TreeNodeStyle CssClass="lstLink" />
                            </LevelStyles>
                        </asp:TreeView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    </div>
    </form>
</body>
</html>
