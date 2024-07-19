<%@ Control Language="C#" AutoEventWireup="true" Inherits="SOS.Web.FilterSelectorControl" Codebehind="FilterSelector.ascx.cs" %>

<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Business Unit:</td>
                    <td><asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" onselectedindexchanged="ddlBusinessUnit_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel" id="tdProject" runat="server">Project:</td>
                    <td>
                        <asp:UpdatePanel ID="upProjects" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlBusinessUnit" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="B" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Complete" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Complete with tasks" Value="D"></asp:ListItem>
                                    <asp:ListItem Text="Active and Complete with tasks" Value="E"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>

                <asp:Panel ID="pnlEmployees" runat="server">
                    <tr>
                        <td class="frmLabel" id="tdEmployee" runat="server">Employee:</td>
                        <td>
                            <asp:UpdatePanel ID="upEmployee" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlBusinessUnit" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlProjects" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlEmployee" runat="server" AutoPostBack="True" onselectedindexchanged="ddlEmployee_SelectedIndexChanged">
                                        <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Budget Administrators" Value="BA"></asp:ListItem>
                                        <asp:ListItem Text="Constructions Managers" Value="CM"></asp:ListItem>
                                        <asp:ListItem Text="Contracts Administrators" Value="CA"></asp:ListItem>
                                        <asp:ListItem Text="Directors Authorizacion" Value="DA"></asp:ListItem>
                                        <asp:ListItem Text="Design Coordinators" Value="DC"></asp:ListItem>
                                        <asp:ListItem Text="Design Managers" Value="DM"></asp:ListItem>
                                        <asp:ListItem Text="Financial Controllers" Value="FC"></asp:ListItem>
                                        <asp:ListItem Text="Managing Directors" Value="MD"></asp:ListItem>
                                        <asp:ListItem Text="Project Managers" Value="PM"></asp:ListItem>
                                        <asp:ListItem Text="Unit Managers" Value="UM"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </asp:Panel>
                
                <asp:Panel ID="pnlTradesRange" runat="server">
                    <asp:Panel ID="pnlErrorTradesRange" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" class="frmError" align="center">'From' trade must be lower than 'To' trade.</td>
                        </tr>
                    </asp:Panel>
                
                    <tr>
                        <td class="frmLabel">From Trade:</td>
                        <td><asp:DropDownList ID="ddlTradesStart" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTradesStart_SelectedIndexChanged"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="frmLabel">To Trade:</td>
                        <td><asp:DropDownList ID="ddlTradesEnd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTradesEnd_SelectedIndexChanged"></asp:DropDownList></td>
                    </tr>
                </asp:Panel>

                <asp:Panel ID="pnlDates" runat="server">
                    <asp:Panel ID="pnlErrorDate" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" class="frmError" align="center">End date must be greater than start date.</td>
                        </tr>
                    </asp:Panel>
                
                    <tr>
                        <td class="frmLabel">From:</td>
                        <td><sos:DateReader ID="sdrStartDate" runat="server"></sos:DateReader></td>
                    </tr>
                    <tr>
                        <td class="frmLabel">To:</td>
                        <td>
                            <sos:DateReader ID="sdrEndDate" runat="server"></sos:DateReader>
                        </td>
                    </tr>
                </asp:Panel>

                <asp:Panel ID="pnlOptions" runat="server">
                    <tr>
                        <td class="frmLabel"></td>
                        <td class="frmText">
                            <asp:RadioButtonList ID="rbuOptions" runat="server">
                                <asp:ListItem Selected="True" Value="Pending" Text="Only next pending tasks" />
                                <asp:ListItem Text="All pending tasks" Value="All" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        </td>
    </tr>
</table>
<br />

<act:UpdatePanelAnimationExtender ID="upaeProjects" runat="server" TargetControlID="upProjects">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdProject" Duration="0.5" Fps="20" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>

<act:UpdatePanelAnimationExtender ID="upaeEmployee" runat="server" TargetControlID="upEmployee">
    <Animations>
        <OnUpdated>
           <FadeIn AnimationTarget="tdEmployee" Duration="0.5" Fps="20" />
        </OnUpdated>
    </Animations>
</act:UpdatePanelAnimationExtender>
