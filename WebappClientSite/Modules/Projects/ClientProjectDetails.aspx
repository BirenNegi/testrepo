<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" CodeBehind="ClientProjectDetails.aspx.cs" Inherits="SOS.Web.DashBoardDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%----%>  <link href="../../Config/StyleSheet.css" rel="stylesheet" />
    
 <style>

.ptHeaderRow a { 
             color:#036bba;
             font-family:Verdana, Arial;
             font-weight:bold;
             font-size:9pt;
             white-space:nowrap;
         }

.ptRow {   
             font-family:Verdana, Arial;
             font-weight:bold;
             font-size:8pt;
             white-space:nowrap;
         }

.ptImageCell {
             content:url("../../Images/building.GIF");
         }

.auto-style3 {
    vertical-align: top;
    height: 25px;
    border: 1px solid #f8f8f8;
    padding-left: 0;
}

.auto-style20 {
            width: 100%;
        }

.auto-style25 {
            width: 290px;
            font-family:Verdana,Arial;
            font-weight: normal;
            font-size: Small;
            color:#000000; /*#747474;*/
        }

.auto-style26 {
            font-family:Verdana,Arial;
            font-weight: normal;
            font-size: Small;
            width:285px;/* */
            color:#000000;
        }
.auto-style27 {
            /*width:205px;*/
            color:#000000;
            text-transform: uppercase;
        }
        
     /*.auto-style33 {
         width: 37%;
         height: 46px;
         vertical-align: bottom;
         font-size:16pt;
         font-family:Verdana,Arial;
     }

     .auto-style34 {
         color: #036bba;
         font-family: Verdana, Arial;
         font-weight: bold;
         font-size: 8pt;
         padding-left: 5px;
         vertical-align: central;
         border: 0.05px solid #f8f8f8;
     }*/

     .auto-style35 {
         vertical-align: top;
     }

     .auto-style36 {
         width: 275px;
         font-family: Verdana,Arial;
         font-weight: normal;
         font-size: Small;
         color: #000000;
         height: 42px;
     }
     .auto-style37 {
         height: 42px;
     }

     .auto-style38 {
         height: 268px;
     }

 </style>



    <table width="100%">

     <%--ROW1   --%> <tr><td class="frmTitle" style="font-size:20px;">Project Summary</td></tr><%--ROW 1  color:#f1761e; #cbcbce--%>
     <%--ROW underline   --%>   <tr> <td style="border-bottom:thin solid #000080;"><img alt="-" src="../../Images/1x1.gif" height="1"/></td></tr>
     
     <%--ROW Links --%>
        <tr><td>
<table>
    <tr>
        <td>
            <%--<asp:Image ID="imgGeneralInfo" runat="server" AlternateText="View/Hide Details" ImageUrl="~/Images/IconCollapse.jpg" style="cursor: pointer;" />--%>

        </td>
        <td class="lstTitle">General Information</td>
        <td>&nbsp;&nbsp;</td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkDrawingsTransmittals" runat="server" CssClass="frmLink">Drawings/Transmittals</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkClientVariations" runat="server" CssClass="frmLink">Variations</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkSeparateAccounts" runat="server" CssClass="frmLink">Separate Accounts</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkClaims" runat="server" CssClass="frmLink">Progress Claims</asp:HyperLink></td>        
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkRFIs" runat="server" CssClass="frmLink">Request For Information(RFIs)</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkEOTs" runat="server" CssClass="frmLink">Extension Of Time(EOTs)</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkMeetingminutes" runat="server" CssClass="frmLink">Meeting Minutes</asp:HyperLink></td>
        <td>&nbsp;&nbsp;</td>
        <td><asp:HyperLink ID="lnkGallery" runat="server" CssClass="frmLink" Visible="true">Gallery</asp:HyperLink></td>
        
    </tr>
</table>


        </td></tr> 
    <%--ROW Links   --%>

     <%--ROW 2   --%>
        <tr>
            <td class="auto-style38">
                  <table class="auto-style7">


                   <tr>
                       <td class="auto-style33"> <br /> <br />
                             <asp:HyperLink ID="hlProjectHeader" runat="server">[hlProjectHeader]</asp:HyperLink>
                           <br />
                       </td>
                
                       
                    </tr>


                   <tr>
                       <td class="auto-style14">
                             <%-- project Generalinfo --%>
                              <div class="auto-style12">
                                    <table class="auto-style19">
                                        <tr class="ptHeaderRow">
                                            <td colspan="3" class="auto-style18" align="left">
                                                <asp:HyperLink ID="hlP" runat="server">Summary</asp:HyperLink>
                                            </td>
                                        </tr>

                                        <tr style="vertical-align:top" >
                                            <td colspan="3" class="auto-style13">
                                                <div class="auto-style11">      <%--class="auto-style11"--%>
                                                  <asp:DataList ID="DataList1" runat="server" DataSourceID=""
                                    Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="#747474" ShowFooter="False" ShowHeader="False" Width="876px" Font-Names="Verdana" Font-Size="Small">
                                    <AlternatingItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Verdana,Arial" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemTemplate>
                                        <table style="width:100%">
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Name:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Street:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="StreetLabel" runat="server" Text='<%# Eval("Street") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Locality:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="LocalityLabel" runat="server" Text='<%# Eval("Locality") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">State:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="StateLabel" runat="server" Text='<%# Eval("State") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Postalcode:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"> <asp:Label ID="PostalCodeLabel" runat="server" Text='<%# Eval("PostalCode") %>' /></td></tr>
                                            <%--<tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">CommencementDate:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="CommencementDateLabel" runat="server" Text='<%# Eval("CommencementDate") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">CompletionDate:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="CompletionDateLabel" runat="server" Text='<%# Eval("CompletionDate") %>' /></td></tr>
                               
                                             <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Completion Date including Claimed EOTs:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="Label12" runat="server" Text='<%# Eval("CompletionDate") %>' /></td></tr>
                                             <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Completion Date including Approved EOTs:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="Label13" runat="server" Text='<%# Eval("CompletionDate") %>' /></td></tr>
                               

                                             
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Defects Liability (days):</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="LiabilityLabel" runat="server" Text='<%# Eval("DefectsLiability") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Retention Amount:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="AmountLabel" runat="server" Text='<%# Eval("Retention") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Retention to Certification:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="CertificationLabel" runat="server" Text='<%# Eval("RetentionToCertification") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Retention to LDP:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="LDPLabel" runat="server" Text='<%# Eval("RetentionToDLP") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Site Allowance:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="AllowanceLabel" runat="server" Text='<%# Eval("SiteAllowances") %>' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style26">Liquidated Damages:</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:Label ID="DamagesLabel" runat="server" Text='<%# Eval("LiquidatedDamages") %>' /></td></tr>
                                             --%>
                                             </table>
                              
                                    </ItemTemplate>
                                </asp:DataList>
                                               </div>    
                                         <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>">
                                         </asp:SqlDataSource> <%----%>


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
                
                       
                    </tr>


                  </table>
                     


            </td></tr> 
     <%--ROW 2   --%>

     <%--ROW 3   --%>
        <tr><td>&nbsp;</td></tr> 
     <%--ROW 3   --%>
     
     <%-- Row4 --%>
        <tr><td>
            <table cellpadding="0" cellspacing="0" class="auto-style1">
        <tr class="ptHeaderRow">
            
            <td align="left"> <asp:HyperLink ID="HyperLink8" runat="server">Overview</asp:HyperLink></td>
        </tr>
        <tr>
            <td class="auto-style3">
                <table class="auto-style20">
                    <tr>
                        <td style="text-align: left" class="auto-style25" colspan="2">
                            <strong>Extensions Of Time</strong><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">Commencement Date:</td>
                        <td>
                            <asp:Label ID="LblCommencementDate" runat="server" Text="Label" class="lbl"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">Completion Date :</td>
                        <td>
                <asp:Label ID="LblCompletionDate" runat="server" Text="Label" class="lbl"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">Completion Date incliding claimed EOTs :</td>
                        <td>
                <asp:Label ID="LblClaimedEots" runat="server" Text="Label" class="lbl"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">Completion Date including approved EOTs :</td>
                        <td>
                <asp:Label ID="LblApprovedEots" runat="server" Text="Label" class="lbl"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td class="auto-style3">


                <table class="auto-style20" runat="server" id="TblClaim">
                    <tr>
                        <td style="text-align: left" class="auto-style25" colspan="2">
                            <strong>Progress Claims</strong><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">ContractAmount :</td>
                        <td >
                <asp:Label ID="LblContractAmt" runat="server" Text="Label" class="lbl" style="text-align:right"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style36">Approved Variation :<br />
                            <br />
                        </td>
                        <td class="auto-style37" >
                <asp:Label ID="LblVariationAmount" runat="server" Text="Label" class="lbl" style="text-align:right"></asp:Label>
                            <br />
                            <strong>_____________________</strong></td>
                    </tr>
                    <tr>
                        <td style="text-align: left;vertical-align:top;" class="auto-style25">Revised Contract Sum :<br />
                        </td>
                        <td style="vertical-align:central;"> 
                <asp:Label ID="LblRevisedContractSum" runat="server" Text="Label" class="lbl" style="text-align:right" Height="16px"></asp:Label>
                            <br />
                            _____________________</td>
                    </tr>
                    
                    <tr>
                        <td style="text-align: left" class="auto-style25">Toal Claimed Amount :</td>
                        <td>
                            <asp:Label ID="LblTotalLastClaimed" runat="server" Text="Label" class="lbl" style="text-align:right"/></td>
                    </tr>
                    
                </table>
                <br />
                </td>
        </tr>
        <%--<tr class="ptHeaderRow">
            <td colspan="2" style="vertical-align: central;text-align:left"><asp:HyperLink ID="HyperLink8" runat="server">RFIs</asp:HyperLink></td>
            
        </tr>--%>
        <tr>
            <td class="auto-style3">
                <table class="auto-style20">
                    <tr>
                        <td style="text-align: left" class="auto-style25" colspan="2">
                            <strong>RFIs</strong><br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">Replied RFIs count :</td>
                        <td>
                <asp:Label ID="LblRepliedRFIs" runat="server" Text="Label" class="lbl"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">Pending RFIs count :</td>
                        <td>
                <asp:Label ID="LblPendingRFIs" runat="server" Text="Label" class="lbl"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left" class="auto-style25">Total RFIs:</td>
                        <td>
                <asp:Label ID="LblTotalRFIs" runat="server" Text="Label" class="lbl"></asp:Label>
                        </td>
                    </tr>
                    
                </table>
                <br />
            </td>
        </tr>
        <tr>
            <td class="auto-style3">&nbsp;</td>
        </tr>
    </table>

            </td></tr> 
     <%-- Row 4 --%>

     <%--ROW 5   --%>
        <tr><td>&nbsp;</td></tr> 
     <%--ROW 5  --%>


     <%--ROW 6   --%>
        <tr><td align="left" class="auto-style35"> 
            <%-- project Role --%>
             <div>
                  <table cellpadding="0" cellspacing="0" class="auto-style1">
                     <tr class="ptHeaderRow">
                        <td colspan="3" align="left">
                            <asp:HyperLink ID="HyperLink2" runat="server">Project Role</asp:HyperLink>
                        </td>
                     </tr>
                     <tr >
                        <td colspan="3">

                            <div class="auto-style10">



                                <asp:DataList ID="DataList2" runat="server"  Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" ForeColor="#747474" ShowFooter="False" ShowHeader="False" Width="888px" Font-Names="Verdana" Font-Size="Small">
                                    <AlternatingItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Names="Verdana,Arial" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                    <ItemTemplate>
                                        <table style="width:100%">
                                            <%--
                                            <tr><td style="border:0.05px solid #f8f8f8;">Junior Contracts Administrator (JCA):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="NameLabel" runat="server" Text='SANTOSH NAYAK' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Design Coordinator (DC):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="LocalityLabel" runat="server" Text='SANTOSH NAYAK' /></td></tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;">Budget Administrator (BA):</td>
                                                <td style="border:0.05px solid #f8f8f8;"><asp:Label ID="StateLabel" runat="server" Text='ALEX' /></td></tr>
                                            
                                             --%>

                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style25">Contracts Administrator (CA):</td>
                                                <td style="border:0.05px solid #f8f8f8; color: #000000;" class="auto-style27">
                                                    <%--<asp:HyperLink ID="StreetLabel" runat="server" NavigateUrl=<%#Eval("CAPeopleId","~/Modules/People/ViewEmployee.aspx?PeopleId={0}") %>' Text='<%# Eval("CAName") %>' />--%>
                                                   <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl='<%# string.Format("~/Modules/People/ViewEmployee.aspx?PeopleId={0}&ProjectId={1}",Eval("CAPeopleId"),Eval("ProjectId")) %>' Text='<%# Eval("CAName") %>' ForeColor="Black" />

                                                </td>
                                            </tr>
                                            
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style25">Project Manager (PM):</td>
                                                      <td style="border:0.05px solid #f8f8f8;" class="auto-style27">
                                                         <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# string.Format("~/Modules/People/ViewEmployee.aspx?PeopleId={0}&ProjectId={1}",Eval("PMPeopleId"),Eval("ProjectId")) %>' Text='<%# Eval("PMName") %>' ForeColor="Black" />

                                                     </td>
                                                  </tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style25">Construction Manager (CM):</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl='<%# string.Format("~/Modules/People/ViewEmployee.aspx?PeopleId={0}&ProjectId={1}",Eval("CMPeopleId"),Eval("ProjectId")) %>' Text='<%# Eval("CMName") %>' ForeColor="Black" />
                                                </td>
                                            </tr>
                               
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style25">Design Manager (DM):</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl='<%# string.Format("~/Modules/People/ViewEmployee.aspx?PeopleId={0}&ProjectId={1}",Eval("DMPeopleId"),Eval("ProjectId")) %>' Text='<%# Eval("DMName") %>' ForeColor="Black" />
                                               </td>
                                            </tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style25">Commercial Manager(COM):</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl='<%# string.Format("~/Modules/People/ViewEmployee.aspx?PeopleId={0}&ProjectId={1}",Eval("COPeopleId"),Eval("ProjectId")) %>' Text='<%# Eval("COMName") %>' ForeColor="Black" />
                                               </td>
                                            </tr>
                                            <tr><td style="border:0.05px solid #f8f8f8;" class="auto-style25">Financial Controller (FC):</td>
                                                <td style="border:0.05px solid #f8f8f8;" class="auto-style27"><asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl='<%# string.Format("~/Modules/People/ViewEmployee.aspx?PeopleId={0}&ProjectId={1}",Eval("FCPeopleId"),Eval("ProjectId")) %>' Text='<%# Eval("FCName") %>' ForeColor="Black" />
                                               </td>
                                            </tr>
                                           
                                             </table>
                              
                                    </ItemTemplate>
                                </asp:DataList>





                            </div>    
                              <%--  <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>" SelectCommand="SELECT [Number], [Name], [Street], [Locality], [State], [PostalCode], [RetentionToCertification], [Retention], [CompletionDate], [CommencementDate] FROM [Projects] WHERE ([ProjectId] = @ProjectId)">
                                <SelectParameters>
                                    <asp:QueryStringParameter DefaultValue="344" Name="ProjectId" QueryStringField="ProjectId" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>    --%>


                            </td>
                   </tr>
                                
                              </table>
             
             </div>
            <%-- project Role --%>

            </td></tr> 
     <%--ROW 6   --%>




     <%--ROW 7   --%>
        <tr><td>&nbsp;</td></tr> 
     <%--ROW 7  --%>

    </table>


<%--    <table cellpadding="0" cellspacing="0" class="auto-style1">
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
            <td class="auto-style2">EOTs</td>
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
    </table>--%>
    <br />
    <br />



</asp:Content>




<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .auto-style1 {
            width: 904px;
            border: 1px solid #DDDDDD;
        }
        .auto-style3 {
            vertical-align:central;
            height:25px;
            border: 1px solid #f8f8f8;
            padding-left:15px;
        }
         .lbl {
            float: left;margin:0px;padding:0px;border:1px;
            /*color:white;*/
            font-family:Verdana, Arial;/*'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;*/
            text-align:center;
            font-size:12px;
            border-color:#838181;
        }
        
        .auto-style7 {
            width: 904px;
            vertical-align:top;
        }
        .auto-style10 {
            width: 898px;
            height: 138px;
            overflow: auto;
        }
        .auto-style11 {
            width: 897px;
            height: 121px;
            overflow: auto;
            /*vertical-align:top;*/
        }
        .auto-style12 {
            float: left;
            width: 394px;
            height: 126px;
        }
        .auto-style13 {
            width: 373px;
            height: 125px;
        }
        .auto-style14 {
            width: 37%;
            height: 150px;
            vertical-align: bottom;
        }
        .auto-style18 {
            width: 373px;
        }
        .auto-style19 {
            border-collapse: collapse;
            width: 433px;
            height: 100px;
            border: 1px solid #e6e6e6;
        }
    </style>
</asp:Content>





