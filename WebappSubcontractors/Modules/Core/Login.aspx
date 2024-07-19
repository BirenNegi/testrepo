<%@ Page Language="C#" AutoEventWireup="true" Inherits="SOS.Web.LoginPage" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SOS++ - Login</title>    
    <link href="../../Config/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body style='background-image:url("../../Images/VaughanBackground.gif")'>
    <form id="form1" runat="server">
    <div>
    
    <br />
    <br />
    <br />
    <br />
    <br />
    <table width="100%">
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:PlaceHolder ID="phSessionError" runat="server" Visible="false">
                            <table width="80%">
                                <tr>
                                    <td class="frmForm">
                                        <table width="100%">
                                            <tr>
                                                <td align="center" class="frmSubSubTitle">
                                                    <br />
                                                    Your session has expired due to inactivity<br />
                                                    <br />
                                                    Please log in again.<br />
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            </asp:PlaceHolder>

                            <asp:PlaceHolder ID="phSecurityError" runat="server" Visible="false">
                            <table width="80%">
                                <tr>
                                    <td class="frmForm">
                                        <table width="100%">
                                            <tr>
                                                <td align="center" class="frmSubSubTitle">
                                                    <br />
                                                    Your tried to access a restricted page<br />
                                                    <br />
                                                    Please log in again.<br />
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            </asp:PlaceHolder>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle"><img src="../../Images/VaughanLogo.gif" alt="VaughanLogo" width="140" height="125" vspace="10" /></td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <table>
                                <tr>
                                    <td><span style="font-size: 12pt; color: #ff0000; font-family: Verdana, Arial"><strong>Subcontract Order System</strong></span></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;<span style="font-size: 16pt; color: #ff0000; font-family: Verdana, Arial"><strong>SOS++</strong></span></td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="frmForm">
                                        <table cellspacing="2">
                                            <tr>
                                                <td colspan="2" align="left"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="frmError"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="frmSubSubTitle">* User Name:</td>
                                                <td align="left" class="frmText">
                                                    <asp:RequiredFieldValidator ID="valLogin" runat="server" ErrorMessage="Required Field!<br />" ControlToValidate="txtLogin" CssClass="frmError" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valLogin1" runat="server" ErrorMessage="Invalid length!<br />" ValidationExpression="\w{4,50}" ControlToValidate="txtLogin" CssClass="frmError" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtLogin" runat="server" Width="150"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td align="right" class="frmSubSubTitle">* Password:</td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="valPassword" runat="server" ErrorMessage="Required Field!<br />" ControlToValidate="txtPassword" CssClass="frmError" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valPassword1" runat="server" ErrorMessage="Invalid length!<br />" ValidationExpression="\w{4,50}" ControlToValidate="txtPassword" CssClass="frmError" Display="Dynamic"></asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right"><asp:Button ID="butLogin" Text="Log In" runat="server" OnClick="butLogin_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left" class="frmText"><a href="SendPassword.aspx" class="frmLink">Forgot your password?</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
