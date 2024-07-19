<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.EditTransmittalPage" Title="Transmittal" Codebehind="EditTransmittal.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script language="javascript" type="text/javascript">

        function ConfirmUpdate() {
            var sentdate = document.getElementById("MainContent_sdrSentDate_txtCalendar").value;
            var deliverytype = document.getElementById("MainContent_ddlDevilvery").value;

            if (sentdate != '')//&& deliverytype != 'DLS' && deliverytype != 'EM'
            {
                if (confirm("Do you want to save this Transmittal with  Date sent : " + sentdate + "  ?   Please note that, once the Date sent is saved then this Transmittal is not editable."))
                { return true; }

                else { return false; }
            }
            else { return true; }
        }
    </script>



    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></act:ToolkitScriptManager>
<sos:TitleBar ID="TitleBar" runat="server" />

<asp:Panel ID="pnlProposal" runat="server">
<table cellspacing="0" cellpadding="0">
    <tr>
        <td class="frmTopBox">
            <asp:linkbutton id="cmdUpdateTop" CssClass="frmButton" runat="server" OnClick="cmdUpdate_Click" OnClientClick="return ConfirmUpdate();">Update</asp:linkbutton>&nbsp;&nbsp;
            <asp:linkbutton id="cmdCancelTop" CssClass="frmButton" runat="server" CausesValidation="False" OnClick="cmdCancel_Click">Cancel</asp:linkbutton>
        </td>
    </tr>
    <tr>
        <td class="frmForm">
            <table width="100%">
                <tr>
                    <td class="frmLabel">Contact:</td>
                    <td>
                        <asp:TextBox ID="txtContactName" Width="150" runat="server" ReadOnly="false" BackColor="#DDDDDD"></asp:TextBox>
                        <asp:HyperLink ID="cmdSelContact" runat="server" ImageUrl="~/Images/IconSearch.gif"></asp:HyperLink>
                        <input id="txtContactId" type="hidden" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Type:</td>
                    <td><sos:ComboAndTextReader ID="ctrType" runat="server"></sos:ComboAndTextReader></td>
                </tr>
                <tr>
                    <td class="frmLabel">Date:</td>
                    <td><sos:DateReader ID="sdrDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Delivery Method:</td>
                    <td><asp:DropDownList ID="ddlDevilvery" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="frmLabel">Date Sent:</td>
                    <td><sos:DateReader ID="sdrSentDate" runat="server"></sos:DateReader></td>
                    <td>&nbsp;</td>
                    <td class="frmLabel">Required Action:</td>
                    <td><sos:ComboAndTextReader ID="ctrAction" runat="server"></sos:ComboAndTextReader></td>
                </tr>
                <tr>
                    <td class="frmLabel" valign="top">Comments:</td>
                    <td colspan="4"><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="3" Width="480"></asp:TextBox></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="frmFormBelow">
            <table cellpadding="4" cellspacing="1">
                <tr>
                    <td colspan="5" class="frmSubSubTitle">Recipients From Distribution List</td>
                </tr>
                <tr>
                    <td class="lstHeader">Role</td>
                    <td class="lstHeader">Company</td>
                    <td class="lstHeader">Name</td>
                    <td class="lstHeader">Email</td>
                    <td class="lstHeader">Add</td>
                </tr>
                <tr>
                    <td class="lstItem">Main Contact</td>
                    <td class="lstItem"><asp:Label ID="lblClientContactCompany" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblClientContactName" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblClientContactEmail" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:CheckBox ID="chkClientContact" runat="server" /></td>
                </tr>
                <tr>
                    <td class="lstAltItem">Contact 1</td>
                    <td class="lstAltItem"><asp:Label ID="lblClientContact1Company" runat="server"></asp:Label></td>
                    <td class="lstAltItem"><asp:Label ID="lblClientContact1Name" runat="server"></asp:Label></td>
                    <td class="lstAltItem"><asp:Label ID="lblClientContact1Email" runat="server"></asp:Label></td>
                    <td class="lstAltItem"><asp:CheckBox ID="chkClientContact1" runat="server" /></td>
                </tr>
                <tr>
                    <td class="lstItem">Contact 2</td>
                    <td class="lstItem"><asp:Label ID="lblClientContact2Company" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblClientContact2Name" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblClientContact2Email" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:CheckBox ID="chkClientContact2" runat="server" /></td>
                </tr>
                <tr>
                    <td class="lstAltItem">Superintendent</td>
                    <td class="lstAltItem"><asp:Label ID="lblSuperintendentCompany" runat="server"></asp:Label></td>
                    <td class="lstAltItem"><asp:Label ID="lblSuperintendentName" runat="server"></asp:Label></td>
                    <td class="lstAltItem"><asp:Label ID="lblSuperintendentEmail" runat="server"></asp:Label></td>
                    <td class="lstAltItem"><asp:CheckBox ID="chkSuperintendent" runat="server" /></td>
                </tr>
                <tr>
                    <td class="lstItem">Qty. Surveyor</td>
                    <td class="lstItem"><asp:Label ID="lblQuantitySurveyorCompany" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblQuantitySurveyorName" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:Label ID="lblQuantitySurveyorEmail" runat="server"></asp:Label></td>
                    <td class="lstItem"><asp:CheckBox ID="chkQuantitySurveyor" runat="server" /></td>
                </tr>
            </table>
        </td>    
    </tr>
    <tr>
        <td class="frmFormBelow">
            <table>
                <tr>
                    <td class="frmSubSubTitle">Drawings</td>
                </tr>


                <!--//San------     -->

                <tr><td>

                    <script language="javascript" type="text/javascript">
                        function CheckAll(oCheckbox)
                        {
                           
                            //alert("Hi");
                          
                            var GridView2 = document.getElementById("<%=gvDrawings.ClientID %>");
                           
                                for (i = 1; i < GridView2.rows.length; i++) {
                                    GridView2.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
                                  }
                            
                        }



                    </script>

                    

                     <asp:GridView 
                                    OnPageIndexChanging="gvDrawings_OnPageIndexChanging"
                                    OnSorting="gvDrawings_OnSorting"
                                    OnRowCreated="gvDrawings_OnRowCreated"
                                    OnRowDataBound="gvDrawings_OnRowDataBound"
                                    EnableViewState="true"
                                    ID="gvDrawings"
                                    runat="server"
                                    AllowPaging="True"
                                    AllowSorting="True"
                                    PageSize="2000"
                                    PagerStyle-CssClass="lstPager"
                                    DataKeyNames="IdStr"
                                    AutoGenerateColumns="False" 
                                    CellPadding="2" 
                                    CssClass="lstList" 
                                    RowStyle-CssClass="lstItem" 
                                    AlternatingRowStyle-CssClass="lstAltItem" Width="761px">
                                    <AlternatingRowStyle CssClass="lstAltItem" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" onclick="CheckAll(this)" runat="server" ToolTip="Select All" /> <%--OnClick="SelectAllDrawings()" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true"  --%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server"/> <%--Visible=' <%#FormatFileLink(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>'--%>
                                                <asp:TextBox ID="TxtDrawingId" Text='<%#Eval("IdStr") %>' runat="server" Height="21px" Width="23px" Visible="false"> </asp:TextBox>
                                                <asp:TextBox ID="TxtDrawingRevisionId" Text='<%#Eval("LastRevisionIdStr") %>' runat="server" Height="21px" Width="23px" Visible="false"> </asp:TextBox>
                                               
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="lstHeader" HeaderText="Copies" SortExpression="NumberOfCopies">
                                            <ItemTemplate>
                                                <%--<asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/Images/IconDocument.gif" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowDrawingRevision.aspx?DrawingRevisionIds={0}", Eval("LastRevisionIdStr"))%>' ToolTip="Download" Visible='<%#FormatFileLink(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>'></asp:HyperLink>--%>
                                           
                                                
                                                 <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="TxtNumberOfCopies" ErrorMessage="Invalid number!<br />" SetFocusOnError="True" CssClass="frmError" Display="Dynamic" Operator="GreaterThan" Type="Integer" ValueToCompare="0"></asp:CompareValidator>
                                           
                                                
                                                 <asp:TextBox ID="TxtNumberOfCopies" Text='<%#Eval("NumberOfCopies") %>' runat="server" Height="21px" Width="23px"></asp:TextBox>
                                            
                                            
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Type" ItemStyle-Wrap="False" SortExpression="DrawingType">
                                            <ItemTemplate>
                                                <span class='<%#(String)Eval("CssClass")%>'><%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("DrawingType")))%></span>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Name" ItemStyle-Wrap="False" SortExpression="Name">
                                            <ItemTemplate>
                                                <span class='<%#(String)Eval("CssClass")%>'><%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("Name")))%></span>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Revision" ItemStyle-Wrap="False" SortExpression="LastRevisionNumber">
                                            <ItemTemplate>
                                                <span class='<%#(String)Eval("CssClass")%>'><%#SOS.UI.Utils.SetFormString(SOS.Data.Utils.GetDBString(Eval("LastRevisionNumber")))%></span>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="Date" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="False" SortExpression="LastRevisionDate">
                                            <ItemTemplate>
                                                <span class='<%#(String)Eval("CssClass")%>'><%#SOS.UI.Utils.SetFormDate(SOS.Data.Utils.GetDBDateTime(Eval("LastRevisionDate")))%></span>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="lstHeader" HeaderText="File Size" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="False" SortExpression="LastRevisionFileSize" Visible="false">
                                            <ItemTemplate>
                                               <%-- <span class='<%#(String)Eval("CssClass")%>'><%#FormatFileSize(Eval("LastRevisionIdStr"), Eval("LastRevisionFileSize"))%>  </span>--%>
                                                <span class='<%#(String)Eval("CssClass")%>'></span>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="lstHeader" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="lstPager" />
                                    <RowStyle CssClass="lstItem" />
                                </asp:GridView>

                    </td></tr>


                <!--   //San----   -->

                <tr>
                    <td>
                        <script src="../../JavaScripts/FormsUtils.js" type="text/javascript"></script>
                        <asp:PlaceHolder ID="phDrawings" runat="server"></asp:PlaceHolder>
                    </td>
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
<br />
</asp:Panel>
<br />

</asp:Content>
