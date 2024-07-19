<%@ Page Language="C#" MasterPageFile="~/MasterPages/Subbie.master" AutoEventWireup="true" Inherits="SOS.Web.ViewHelpPage" Title="Help" Codebehind="Help.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" CombineScripts="false" />

<!-- Intro -->
<table>
    <tr>
        <td class="hlpTitle">How to quote online</td>
    </tr>
</table>

<table>
    <tr>
        <td><iframe width="640" height="360" src="https://www.youtube.com/embed/tYBOlbbC5UA" frameborder="0" allowfullscreen></iframe></td>
    </tr>
</table>

<br />

<table>
    <tr>
        <td class="hlpTitle" colspan="2">Documents</td>
    </tr>
</table>

<table>
        <td><asp:HyperLink ID="HyperLink1" NavigateUrl="~/Modules/Help/Docs/SOSQuoteOnLineUserGuide.pdf" runat="server" CssClass="frmLink" Target="_blank">User guide</asp:HyperLink></td>
    </tr>
</table>

<br />

<table>
    <tr>
        <td class="hlpTitle">Questions or comments ?</td>
    </tr>
</table>

<asp:UpdatePanel ID="upMessage" runat="server" RenderMode="Inline" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostbackTrigger ControlID="cmdSubmit" EventName="Click" />
    </Triggers>
    <ContentTemplate>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false">
            <br />
            <table>
                <tr>
                    <td class="frmFormMsg"><asp:Label ID="lblMessage" runat="server" Text="Thank you! your questions have been received."></asp:Label></td>
                </tr>
            </table>            
            <br />
        </asp:Panel>

        <table cellspacing="0" cellpadding="0">
            <tr>
                <td class="frmText">
                    <asp:RequiredFieldValidator ID="valComment" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtComment"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtComment" TextMode="MultiLine" Rows="6" runat="server" Width="500" MaxLength="1000"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="frmBottomBox">&nbsp;<asp:linkbutton id="cmdSubmit" CssClass="frmButton" runat="server" OnClick="cmdSubmit_Click">Submit</asp:linkbutton>&nbsp;</td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

</asp:Content>
