<%@ Page Language="C#" AutoEventWireup="true" Inherits="SOS.Web.LoginPage" Codebehind="Login.aspx.cs" %>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>

<html><%-- xmlns="http://www.w3.org/1999/xhtml"--%>
<head runat="server">
    <title>SOS++ - Login</title>    
            <link href="../../Config/StyleSheet.css" rel="stylesheet" type="text/css" />
            <link href="../../Style/MasterStyleSheet.css" rel="stylesheet" />

    <style >
    /* to hide video controls attribute */
   video {
         pointer-events: none;
            }

    </style>


    <script type="text/javascript">

        
        function removeControls(video) {
           // alert("hi");
            video.removeAttribute('controls');
        }
        video = document.getElementsByTagName('video');
       // alert(video);
        window.onload = removeControls(video);

    </script>

    </head>




<body style='background-image:url("../../Images/VaughanBackground.gif")'>
    <form id="form1" runat="server">
    <div>
    
    <br />
    <table width="100%" cellpadding="0" cellspacing="0">

        <tr>
            <td style="padding-left:50px;">
                <br />
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Image/Logo3a.png"   />
                 &nbsp;<br />
                <br />
                 
            </td>
            <td>&nbsp;</td></tr>


        <tr>
            <td align="left" style="width:30%;background-color:#000000;border-color:#000000;">   <%--  san  --= center--%>
                 <table>
                    <tr>
                        <td colspan="3">
                            <asp:PlaceHolder ID="phSessionError" runat="server" Visible="false">
                            <table width="80%">
                                <tr>
                                    <td class="frmForm">
                                        <table width="100%">
                                            <tr>
                                                <td align="left" class="frmSubSubTitle" style="color:#fff; padding-left:50px;">
                                                    <br />
                                                    Your session has expired due to inactivity<br />
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
                                                <td align="left" class="frmSubSubTitle" style="color:#fff; padding-left:50px;">
                                                    <br />
                                                    Your tried to access a restricted page
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
                        <td valign="middle" style="width:140px;">
                          
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>


                        <td>
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>



                            <table style="border-style: solid; border-width: 1px; border-color: inherit;">
                                <tr>
                                    <td class="frmForm">
                                        <table cellspacing="2">
                                            <tr>
                                                <td colspan="2" align="left"><asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="frmError" ForeColor="WhiteSmoke"></asp:Label>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="frmSubSubTitle" style="color:#fbf3f3;"></td><%-- * User Name:--%>
                                                <td align="left" class="frmText">
                                                    <asp:RequiredFieldValidator ID="valLogin" runat="server" ErrorMessage="Required Field!<br />" ControlToValidate="txtLogin" CssClass="frmError" Display="Dynamic" ForeColor="#F7F7F7"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valLogin1" runat="server" ErrorMessage="Invalid length!<br />" ValidationExpression="\w{4,50}" ControlToValidate="txtLogin" CssClass="frmError" Display="Dynamic" ForeColor="#F7F7F7"></asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtLogin" runat="server" Width="250px" placeholder="Username" Height="24px" BackColor="White" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td align="right" class="frmSubSubTitle" style="color:#fbf3f3;"></td>
                                                <td align="left">
                                                    <asp:RequiredFieldValidator ID="valPassword" runat="server" ErrorMessage="Required Field!<br />" ControlToValidate="txtPassword" CssClass="frmError" Display="Dynamic" ForeColor="#F7F7F7"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valPassword1" runat="server" ErrorMessage="Invalid length!<br />" ValidationExpression="\w{4,50}" ControlToValidate="txtPassword" CssClass="frmError" Display="Dynamic" ForeColor="#F7F7F7"></asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="250px" placeholder="Password" Height="24px" BackColor="White" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <br />
                                                    <asp:Button ID="butLogin" Text="Login" runat="server" OnClick="butLogin_Click" Height="29px" Width="83px" style="border: 1px solid black ;border-radius:8px;" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left"  class="frmText">&nbsp; <a href="../../Modules/Core/SendPassword.aspx" class="frmLink" style="font-size:8pt;color:#fff;">Forgot password?</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>


            <%--San--%>
            <td style="background-color:#000000;border-color:#000000;">
                <%--<asp:Image ID="Image1" runat="server" ImageUrl="~/Image/Banner.GIF" />--%>
               
                <br />
                
               <table>
                   <tr>
                       <td> 
                           <br /> <br /> <br /> <br/> <br/>
                          <video id="logo_video" class="video" style="pointer-events:none;"  
                               src="../../videos/clouds_intro01.mp4" controls="controls" autoplay="autoplay" loop />

                           </video>
                           <br />
                            
                       </td>


                       <td style="padding-left:80px;">
                          <%-- <div> </div>--%>
                                <h1 class="hometitle">REMARKABILITY<br>DEMANDS THE <br>EXTRAORDINARY</h1>
                             
                                 <br />
                                 <br />

                       </td>
                   </tr>

               </table>

                  <br/> <br/>
                  <br/> <br/><br/> <br/>

            </td>
            <%--San--%>

        </tr>

        <%-- San --%>
        <tr><td colspan="2"; align="center">
            <asp:Image ID="Image3" runat="server" imageurl="~/Image/bg-divider.png" Height="275px" Width="906px"/>
            </td></tr>

        <%-- San --%>
    </table>
    
    </div>
    </form>
</body>
</html>
