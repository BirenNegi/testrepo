<%@ Page Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" Inherits="SOS.Web.ListMeetingMinutes" Title="MeetingMinutes" Codebehind="ListMeetingMinutes.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<sos:TitleBar ID="TitleBar1" runat="server" Title="MeetingMinutes" />

<table>
    <asp:PlaceHolder ID="phAddNew" runat="server" Visible="false">
    <tr>
        <td>
            <table cellspacing="1" cellpadding="1">
                <tr>
                    <td><asp:HyperLink ID="lnkAddMeetings" runat="server" ImageUrl="~/Images/IconAdd.gif" ToolTip="Add New"></asp:HyperLink></td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:PlaceHolder>
    <tr>
        <td>
            <asp:GridView
                ID = "gvMeetings"
                runat = "server"
                DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                AlternatingRowStyle-CssClass = "lstAltItem">
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconView.gif" ToolTip="Open" runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ViewMeetingMinutes.aspx?MeetingId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>

<ItemStyle VerticalAlign="Top"></ItemStyle>
                    </asp:TemplateField>
                


<%--                    <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("Number"))%>
                        </ItemTemplate>

<HeaderStyle CssClass="lstHeader"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>
--%>


            <asp:TemplateField HeaderText="No" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.UI.Utils.SetFormInteger((int?)Eval("TypeNumber"))%>
                        </ItemTemplate>

                        <HeaderStyle CssClass="lstHeader"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False"></ItemStyle>
          </asp:TemplateField>




                
                    <asp:BoundField DataField="Subject" HeaderText="Subject/Type" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                            <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>

                   <%-- <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="lstHeader" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <%#SOS.Web.Utils.GetConfigListItemName("RFI", "Status", (String)Eval("Status"))%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="Meeting Date" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#SOS.UI.Utils.SetFormDate((DateTime?)Eval("MeetingDate"))%>
                                </ItemTemplate>

                            <HeaderStyle CssClass="lstHeader"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:TemplateField>



                       <asp:BoundField DataField="MeetingTime" HeaderText="Time" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                            <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                     </asp:BoundField>


                        <asp:BoundField DataField="Location" HeaderText="Location" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                            <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                     </asp:BoundField>


                       <asp:BoundField DataField="Attendees" HeaderText="Attendees" HeaderStyle-CssClass="lstHeader" ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top">
                            <HeaderStyle CssClass="lstHeader"></HeaderStyle>
                            <ItemStyle VerticalAlign="Top" Wrap="False"></ItemStyle>
                    </asp:BoundField>

                   <asp:TemplateField HeaderStyle-CssClass="lstHeader" ItemStyle-HorizontalAlign="Center" HeaderText="Minutes">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Images/IconDocument.gif" Visible='<%#IsComplete((SOS.Core.MeetingMinutesInfo)DataBinder.Eval(Container, "DataItem"))%>' runat="server" NavigateUrl='<%#String.Format("~/Modules/Projects/ShowMeetingMinutes.aspx?MeetingId={0}", Eval("IdStr"))%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                 
                </Columns>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>

