<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="EditHomePagePendingTaskList.aspx.cs" Inherits="SOS.Web.EditHomePagePendingTaskList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />

<sos:TitleBar ID="TitleBar1" runat="server" Title="My Pending Tasks / Approvals" />
<%--<table id="tblTasks" runat="server" cellpadding="4" cellspacing="1"></table>--%>
    <table class="frmForm">
        <tr>
            <td class="lstItemTitle" colspan="3">&nbsp;</td>
            
        </tr>
        <tr>
            <td class="lstItemTitle">Project</td>
            <td>:</td>
            <td class="lstItemTitle"> <asp:DropDownList ID="dpdProjectList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dpdProjectList_SelectedIndexChanged"></asp:DropDownList></td> 
        </tr>

        <tr class="lstItemTitle">
            <td colspan="3" class="lstTitle">
                <br />
                Comparison&nbsp;

            </td>
             
        </tr>

        <tr>
            <td colspan="3" >
                
                <asp:GridView ID="gvComparison" runat="server" AutoGenerateColumns="False">
                <AlternatingRowStyle CssClass="lstAltItem" />
                <Columns>
                    <asp:BoundField DataField="Process.Id" HeaderText="ProcessId" ReadOnly="True" SortExpression="Process.Id" Visible="true" HeaderStyle-CssClass="lstHeader" />
                    <asp:BoundField DataField="Process.Project.Trades[0].Name" HeaderText="TradeName" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Pending Task" ReadOnly="True" SortExpression="Name"  HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="targetDate" HeaderText="TargetDate" ReadOnly="True" SortExpression="targetDate" DataFormatString="{0:dd/M/yyyy}" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hide" HeaderStyle-CssClass="lstHeader" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkHide" runat="server" Checked='<%# Convert.ToBoolean(Eval("Process.Hide")) %>' AutoPostBack="True" OnCheckedChanged="chkHide_CheckedChanged" />
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="lstItem" />
                </asp:GridView>

            </td> 
        </tr>
        <tr>
            <td colspan="2" align="right">
               </td>
            
            <td>
                <br />
            </td> 
        </tr>

        <tr class="lstItemTitle">
            <td colspan="3" class="lstTitle">
                Contracts</td>
            
           
        </tr>

        <tr>
            <td colspan="3" align="left">
                
                <asp:GridView ID="gvContract" runat="server" AutoGenerateColumns="False">
                <AlternatingRowStyle CssClass="lstAltItem" />
                <Columns>
                    <asp:BoundField DataField="Process.Id" HeaderText="ProcessId" ReadOnly="True" SortExpression="Process.Id" Visible="true" HeaderStyle-CssClass="lstHeader" />
                    <asp:BoundField DataField="Process.Project.Trades[0].Name" HeaderText="TradeName" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Pending Task" ReadOnly="True" SortExpression="Name"  HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="targetDate" HeaderText="TargetDate" ReadOnly="True" SortExpression="targetDate" DataFormatString="{0:dd/M/yyyy}" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hide" HeaderStyle-CssClass="lstHeader" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkHide0" runat="server" Checked='<%# Convert.ToBoolean(Eval("Process.Hide")) %>' AutoPostBack="True" OnCheckedChanged="chkHide_CheckedChanged" />
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="lstItem" />
                </asp:GridView>

            </td>
            
           
        </tr>

        <tr>
            <td colspan="2" align="right">
                &nbsp;</td>
            
            <td>&nbsp;</td> 
        </tr>

        <tr class="lstItemTitle">
            <td colspan="3"  class="lstTitle">
                Variation Orders </td>
            
           
        </tr>
        <tr>
            <td colspan="3">
               <asp:GridView ID="gvVariationOrders" runat="server" AutoGenerateColumns="False">
                <AlternatingRowStyle CssClass="lstAltItem" />
                <Columns>
                    <asp:BoundField DataField="Process.Id" HeaderText="ProcessId" ReadOnly="True" SortExpression="Process.Id" Visible="true" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Process.Project.Trades[0].Name" HeaderText="TradeName" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Title" DataField="Process.Project.Trades[0].Contract.Description" ReadOnly="True" HeaderStyle-CssClass="lstHeader"/>
                    <asp:BoundField DataField="Name" HeaderText="Pending Task" ReadOnly="True" SortExpression="Name"  HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="targetDate" HeaderText="TargetDate" ReadOnly="True" SortExpression="targetDate" DataFormatString="{0:dd/M/yyyy}" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hide" HeaderStyle-CssClass="lstHeader" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkVOHide" runat="server" Checked='<%# Convert.ToBoolean(Eval("Process.Hide")) %>' AutoPostBack="True" OnCheckedChanged="chkHide_CheckedChanged" />
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="lstItem" />
                </asp:GridView>  
            </td>
            
             
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;</td> </tr>


        <tr class="lstItemTitle"><td colspan="3" class="lstTitle"> Client Variations</td></tr>
        <tr> <td colspan="3">
               <asp:GridView ID="gvClientVariations" runat="server" AutoGenerateColumns="False">
                <AlternatingRowStyle CssClass="lstAltItem" />
                <Columns>
                    <asp:BoundField DataField="Process.Id" HeaderText="ProcessId" ReadOnly="True" SortExpression="Process.Id" Visible="true" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Process.Project.ClientVariations[0].Number" HeaderText="Number" HeaderStyle-CssClass="lstHeader" > <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="Process.Project.ClientVariations[0].Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" > <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Pending Task" ReadOnly="True" SortExpression="Name"  HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="targetDate" HeaderText="TargetDate" ReadOnly="True" SortExpression="targetDate" DataFormatString="{0:dd/M/yyyy}" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hide" HeaderStyle-CssClass="lstHeader" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkVOHide0" runat="server" Checked='<%# Convert.ToBoolean(Eval("Process.Hide")) %>' AutoPostBack="True" OnCheckedChanged="chkHide_CheckedChanged" />
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="lstItem" />
                </asp:GridView>  
            </td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>

         <tr class="lstItemTitle"><td colspan="3" class="lstTitle"> Separate Accounts</td></tr>
        <tr> <td colspan="3">
               <asp:GridView ID="gvSeparateAccounts" runat="server" AutoGenerateColumns="False">
                <AlternatingRowStyle CssClass="lstAltItem" />
                <Columns>
                    <asp:BoundField DataField="Process.Id" HeaderText="ProcessId" ReadOnly="True" SortExpression="Process.Id" Visible="true" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Process.Project.ClientVariations[0].Number" HeaderText="Number" HeaderStyle-CssClass="lstHeader" > <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>

                    <asp:BoundField DataField="Process.Project.ClientVariations[0].Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" > <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Pending Task" ReadOnly="True" SortExpression="Name"  HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="targetDate" HeaderText="TargetDate" ReadOnly="True" SortExpression="targetDate" DataFormatString="{0:dd/M/yyyy}" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hide" HeaderStyle-CssClass="lstHeader" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkVOHide1" runat="server" Checked='<%# Convert.ToBoolean(Eval("Process.Hide")) %>' AutoPostBack="True" OnCheckedChanged="chkHide_CheckedChanged" />
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="lstItem" />
                </asp:GridView>  
            </td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>

        <tr class="lstItemTitle"><td colspan="3" class="lstTitle"> Claims</td></tr>
        <tr> <td colspan="3">
               <asp:GridView ID="gvClaims" runat="server" AutoGenerateColumns="False">
                <AlternatingRowStyle CssClass="lstAltItem" />
                <Columns>
                    <asp:BoundField DataField="Process.Id" HeaderText="ProcessId" ReadOnly="True" SortExpression="Process.Id" Visible="true" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Process.Project.Claims[0].Number" HeaderText="Number" HeaderStyle-CssClass="lstHeader" > <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>

                    <%--<asp:BoundField DataField="Process.Project.ClientVariations[0].Name" HeaderText="Name" HeaderStyle-CssClass="lstHeader" > <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="Name" HeaderText="Pending Task" ReadOnly="True" SortExpression="Name"  HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="targetDate" HeaderText="TargetDate" ReadOnly="True" SortExpression="targetDate" DataFormatString="{0:dd/M/yyyy}" HeaderStyle-CssClass="lstHeader" >
<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Hide" HeaderStyle-CssClass="lstHeader" >
                        <ItemTemplate>
                            <asp:CheckBox ID="chkVOHide2" runat="server" Checked='<%# Convert.ToBoolean(Eval("Process.Hide")) %>' AutoPostBack="True" OnCheckedChanged="chkHide_CheckedChanged" />
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="lstItem" />
                </asp:GridView>  
            </td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>

    <%--
      
        <tr class="lstItemTitle"><td colspan="3" class="lstTitle"> RFIs</td></tr>
        <tr> <td colspan="3">
               &nbsp;</td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>

       <tr class="lstItemTitle"><td colspan="3" class="lstTitle"> EOTs</td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>

        <tr class="lstItemTitle"><td colspan="3" class="lstTitle"> Transmittals</td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>
        <tr> <td colspan="3">&nbsp;</td></tr>--%>
        <tr>
            <td colspan="3">
                &nbsp;</td>
            
             
        </tr>
    </table>
   
   

<asp:HiddenField ID="hidRolesInfo" runat="server" Value="" />
    
<div id="divNoTasks" runat="server" visible="false">
    <br />
    <table>
        <tr>
            <td><asp:Image ID="imgNoTasks" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" /></td>
            <td class="lstTitle">Projects with no pending tasks</td>
        </tr>
    </table>
    <asp:Panel ID="pnlNoTasks" runat="server" CssClass="collapsePanel" Height="0">
    <table id="tblNoTasks" runat="server" cellpadding="4" cellspacing="1"></table>
    </asp:Panel>
    <act:CollapsiblePanelExtender
        ID="cpe" 
        runat="Server" 
        Collapsed="True" 
        CollapsedSize="0" 
        TargetControlID="pnlNoTasks" 
        ExpandControlID="imgNoTasks" 
        CollapseControlID="imgNoTasks" 
        ImageControlID="imgNoTasks" 
        ExpandedImage="~/Images/IconCollapse.jpg" 
        CollapsedImage="~/Images/IconExpand.jpg" 
        ExpandDirection="Vertical">
    </act:CollapsiblePanelExtender>
</div>

<asp:Label ID="lblProjectsInfo" runat="server" Visible="false" CssClass="lstTitle">You don't have active projects.</asp:Label>

</asp:Content>

