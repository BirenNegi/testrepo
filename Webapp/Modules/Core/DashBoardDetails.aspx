<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="DashBoardDetails.aspx.cs" Inherits="SOS.Web.DashBoardDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
     <link href="../../Config/StyleSheet.css" rel="stylesheet" />
    
    <table width="100%">

     <%--ROW1   --%> <tr><td class="frmTitle" style="font-size:20px;">Project Summary</td></tr><%--ROW 1  color:#f1761e; #cbcbce--%>
     <%--ROW underline   --%>   <tr> <td style="border-bottom:thin solid #000080;"><img alt="-" src="../../Images/1x1.gif" height="1"/></td></tr>
   
         <%--ROW 2   --%>
        <tr>
            <td>
                  <table class="auto-style7">
                   <tr>
                       <td class="auto-style14">
                             <%-- project Generalinfo --%>
                              <div class="auto-style12">
                              <table class="auto-style19">
                                 <tr class="ptHeaderRow">
                        <td colspan="3" class="auto-style18">
                            <asp:HyperLink ID="HyperLink1" runat="server">221 Pelham | 17-612</asp:HyperLink>
                            </td>
                   </tr>
                                 <tr >
                        <td colspan="3" class="auto-style13">

                            <div class="auto-style11">

                                <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="#747474" ShowFooter="False" ShowHeader="False" Width="402px" Font-Names="Verdana" Font-Size="Small">
                                    <AlternatingItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Verdana,Arial" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemTemplate>
                                        <table style="width:100%">
                                            <tr><td style="border:0.05px solid #f8f8f8;">Name:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Street:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="StreetLabel" runat="server" Text='<%# Eval("Street") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Locality:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="LocalityLabel" runat="server" Text='<%# Eval("Locality") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">State:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="StateLabel" runat="server" Text='<%# Eval("State") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Postalcode:</td>
                                                <td style="border:0.05px solid #f8f8f8;"> <asp:Label ID="PostalCodeLabel" runat="server" Text='<%# Eval("PostalCode") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">CommencementDate:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="CommencementDateLabel" runat="server" Text='<%# Eval("CommencementDate") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">CompletionDate:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="CompletionDateLabel" runat="server" Text='<%# Eval("CompletionDate") %>' /></td></tr>
                               
                                            <tr><td style="border:0.05px solid #f8f8f8;">Name:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label10" runat="server" Text='<%# Eval("Name") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Street:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label11" runat="server" Text='<%# Eval("Street") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Name:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label12" runat="server" Text='<%# Eval("Name") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Street:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label13" runat="server" Text='<%# Eval("Street") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Name:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label14" runat="server" Text='<%# Eval("Name") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Street:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label15" runat="server" Text='<%# Eval("Street") %>' /></td></tr>
                                    
                                             </table>
                              
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>    
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>" SelectCommand="SELECT [Number], [Name], [Street], [Locality], [State], [PostalCode], [RetentionToCertification], [Retention], [CompletionDate], [CommencementDate] FROM [Projects] WHERE ([ProjectId] = @ProjectId)">
                                <SelectParameters>
                                    <asp:QueryStringParameter DefaultValue="344" Name="ProjectId" QueryStringField="ProjectId" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>


                            </td>
                   </tr>
                                 <%--<tr  class="ptFooterRow">
            
                
                                <td style="width:30%"></td>
                                <td style="width:40%"> Active|NSW</td>
                                <td style="width:30%;text-align:right">
                                    <asp:HyperLink ID="ImageButton1" runat="server" ImageUrl="~/Images/IconAdd.gif" NavigateUrl="~/Modules/Core/DashBoardDetails.aspx" /> </td>
                            </tr> --%>
                              </table>
                              </div>
                             <%-- project Generalinfo --%>
                       </td>
                
                        <td class="auto-style6">
                            <%-- project Role --%>
                            <div class="auto-style9">
                              <table class="auto-style16" >
                                 <tr class="ptHeaderRow">
                        <td colspan="3" class="auto-style15">
                            <asp:HyperLink ID="HyperLink2" runat="server">Project Role</asp:HyperLink>
                            </td>
                   </tr>
                                 <tr >
                        <td colspan="3" class="auto-style17">

                            <div class="auto-style10">

                                <asp:DataList ID="DataList2" runat="server" DataSourceID="SqlDataSource1" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="#747474" ShowFooter="False" ShowHeader="False" Width="419px" Font-Names="Verdana" Font-Size="Small">
                                    <AlternatingItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Verdana,Arial" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemTemplate>
                                        <table style="width:100%">
                                            <tr><td style="border:0.05px solid #f8f8f8;">Junior Contracts Administrator (JCA):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="NameLabel" runat="server" Text='SANTOSH NAYAK' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Contracts Administrator (CA):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="StreetLabel" runat="server" Text='ALEX' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Design Coordinator (DC):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="LocalityLabel" runat="server" Text='SANTOSH NAYAK' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Budget Administrator (BA):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="StateLabel" runat="server" Text='ALEX' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Project Manager (PM):</td>
                                                <td style="border:0.05px solid #f8f8f8;"> <asp:Label ID="PostalCodeLabel" runat="server" Text='ALEJANDRO ROJAS' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Financial Controller (FC):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="CommencementDateLabel" runat="server" Text='ALEJANDRO ROJAS' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Construction Manager (CM):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="CompletionDateLabel" runat="server" Text='ALEX' /></td></tr>
                               
                                            <tr><td style="border:0.05px solid #f8f8f8;">Design Manager (DM):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label10" runat="server" Text='SANTOSH NAYAK' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Commercial Manager(COM):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label11" runat="server" Text='ALEJANDRO ROJAS' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Managing Director (MD):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label12" runat="server" Text='ANDREW NOBLE' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Director Authorization (DA):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label13" runat="server" Text='ALEJANDRO ROJAS' /></td></tr>
                                            <%--<tr><td style="border:0.05px solid #f8f8f8;">Name:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label14" runat="server" Text='<%# Eval("Name") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Street:</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="Label15" runat="server" Text='<%# Eval("Street") %>' /></td></tr>--%>
                                    
                                             </table>
                              
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>    
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>" SelectCommand="SELECT [Number], [Name], [Street], [Locality], [State], [PostalCode], [RetentionToCertification], [Retention], [CompletionDate], [CommencementDate] FROM [Projects] WHERE ([ProjectId] = @ProjectId)">
                                <SelectParameters>
                                    <asp:QueryStringParameter DefaultValue="344" Name="ProjectId" QueryStringField="ProjectId" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>


                            </td>
                   </tr>
                                
                              </table>
                                </div>
                        </td>
                    </tr>


                  </table>
                     


            </td></tr> <%--ROW 2   --%>

    <%--ROW 3   --%><tr><td>
            &nbsp;</td></tr> <%--ROW 3   --%>

    </table>
    <table cellpadding="0" cellspacing="0" class="auto-style1">
        <tr >
            <td class="auto-style2"></td>
            <td class="auto-style3" style="text-align:center;font-family:Verdana, Geneva, Tahoma, sans-serif;color:#020257;font-size:12px;">
                <br />
                <asp:Label ID="Label16" runat="server" Text="S." BackColor="Red" ForeColor="Red"></asp:Label>&nbsp;Overdue&nbsp;&nbsp;
                <asp:Label ID="Label17" runat="server" Text="S." BackColor="Orange" ForeColor="Orange"></asp:Label> &nbsp;Next 7 Days&nbsp;&nbsp;
                <asp:Label ID="Label18" runat="server" Text="S." BackColor="Green" ForeColor="Green"></asp:Label> &nbsp;> 7 Days
                <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td class="auto-style4">Total</td>
        </tr>
        <tr>
            <td class="auto-style2">Comparison</td>
            <td class="auto-style3">
                <asp:Label ID="Label1" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label3" runat="server" Text="Label" class="lbl" ></asp:Label>
            </td>
            <td class="auto-style4">13</td>
        </tr>
        <tr>
            <td class="auto-style2">Variation Orders </td>
            <td class="auto-style3">
                <asp:Label ID="Label4" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label5" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label6" runat="server" Text="Label" class="lbl" ></asp:Label>

            </td>
            <td class="auto-style4">11</td>
        </tr>
        <tr>
            <td class="auto-style2">RFIs</td>
            <td class="auto-style3">
                <asp:Label ID="Label7" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label8" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label9" runat="server" Text="Label" class="lbl" ></asp:Label>

            </td>
            <td class="auto-style4">8</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
    </table>
    <br />
    <br />



</asp:Content>




<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            width: 904px;
            border: 1px solid #DDDDDD;
        }
        .auto-style2 {
            width: 15%;
            color:#036bba; 
            font-family:Verdana, Arial;
            font-weight:bold;
            font-size:8pt;
            padding-left:5px;
            vertical-align:central;
            border:0.05px solid #f8f8f8;
        }
        .auto-style3 {
            vertical-align:central;
            height:25px;
            border: 1px solid #f8f8f8;
            padding-left:15px;
        }
         .auto-style4 {
            width: 15%;
            border: 1px solid #f8f8f8;
            text-align:center;font-family:Verdana, Geneva, Tahoma, sans-serif;color:#020257;font-size:12px;
        }
        .lbl {
            float: left;margin:0px;padding:0px;border:1px;
            color:white;
            font-family:Verdana, Arial;/*'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;*/
            text-align:center;
            font-size:12px;
            border-color:#838181;
        }
        
        .auto-style6 {
            width: 43%;
            height: 212px;
            vertical-align:bottom;
        }
        .auto-style7 {
            width: 953px;
            vertical-align:top;
        }
        .auto-style9 {
            float: right;
            width: 485px;
            height: 190px;
            margin-right: 0px;
        }
        .auto-style10 {
            width: 429px;
            height: 128px;
            overflow: auto;
        }
        .auto-style11 {
            width: 418px;
            height: 128px;
            overflow: auto;
        }
        .auto-style12 {
            float: left;
            width: 394px;
            height: 186px;
        }
        .auto-style13 {
            width: 352px;
            height: 163px;
        }
        .auto-style14 {
            width: 37%;
            height: 212px;
            vertical-align: bottom;
        }
        .auto-style15 {
            width: 399px;
        }
        .auto-style16 {
            border-collapse: collapse;
            width: 414px;
            height: 100px;
            border: 1px solid #e6e6e6;
        }
        .auto-style17 {
            width: 399px;
            height: 163px;
        }
        .auto-style18 {
            width: 352px;
        }
        .auto-style19 {
            border-collapse: collapse;
            width: 433px;
            height: 100px;
            border: 1px solid #e6e6e6;
        }
    </style>
</asp:Content>





