<%@ Page Language="C#" AutoEventWireup="true" Inherits="SOS.Web.SendPasswordPage" Codebehind="SendPassword.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>SOS - Login</title>
    
    <style type="text/css">
        .LoginPageTitle {
          color:#FF0000;
          font-family:Arial,Helvetica,Sans-Serif;
          font-size:18pt;
          font-weight:bold;
        }
    </style>
    
    <link href="../../Config/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body background="../../Images/VaughanBackground.gif">
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
                        <td valign="top"><img src="../../Images/VaughanLogo.gif" alt="VaughanLogo" width="140" height="125" vspace="10" /></td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <span style="font-size: 16pt; color: #ff0000; font-family: Verdana, Arial"><strong>Subcontract Order System</strong></span>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td class="frmForm">
                                        <table cellspacing="2">
                                            <asp:Panel ID="pnlForm" runat="server">
                                                <tr>
                                                    <td align="right" class="frmSubSubTitle">* Email:</td>
                                                    <td align="left">
                                                        <asp:Label ID="lblError" runat="server" CssClass="frmError"></asp:Label>
                                                        <asp:TextBox ID="txtEmail" Width="300" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right"><asp:Button ID="butEmail" Text="Send password" runat="server" OnClick="butEmail_Click" /></td>
                                                </tr>
                                            </asp:Panel>
                                    
                                            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="frmFormMsg">
                                                <tr>
                                                    <td class="frmSubTitle">Your password has been sent.</td>
                                                </tr>
                                            </asp:Panel>
                                            
                                            <tr>
                                                <td colspan="2" align="left" class="frmText"><a href="Login.aspx" class="frmLink">Go Back to Login Page</a></td>
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
