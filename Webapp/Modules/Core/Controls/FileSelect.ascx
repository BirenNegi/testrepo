<%@ Control Language="C#" AutoEventWireup="true" Inherits="SOS.Web.FileSelectControl" Codebehind="FileSelect.ascx.cs" %>
<asp:RequiredFieldValidator ID="valFileName" CssClass="frmError" runat="server" Display="Dynamic" ErrorMessage="Required Field.<br />" ControlToValidate="txtFileName"></asp:RequiredFieldValidator>
<asp:TextBox ID="txtFileName" runat="server" BackColor="#DDDDDD" Width="480"></asp:TextBox>
<asp:HyperLink ID="cmdSelFile" runat="server" ImageUrl="~/images/IconSearch.gif"></asp:HyperLink>
<input id="inputFilePath" type="hidden" runat="server" />

