<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photogallery3.aspx.cs" Inherits="SOS.Web.Photogallery3" %>

<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <%--San--<title>SlidesJS Standard Code Example</title>
  <meta name="description" content="SlidesJS is a simple slideshow plugin for jQuery. Packed with a useful set of features to help novice and advanced developers alike create elegant and user-friendly slideshows.">
  <meta name="author" content="Nathan Searles">--%>

  <!-- SlidesJS Required (if responsive): Sets the page width to the device width. -->
  <meta name="viewport" content="width=device-width">
  <!-- End SlidesJS Required -->

  <!-- CSS for slidesjs.com example -->
    <link href="../../Content/font-awesome.min.css" rel="stylesheet" />
    <link href="../../Content/example.css" rel="stylesheet" />
 <!-- End CSS for slidesjs.com example -->

  <!-- SlidesJS Optional: If you'd like to use this design -->
  <style>
    body {
      -webkit-font-smoothing: antialiased;
      font: normal 15px/1.5 "Helvetica Neue", Helvetica, Arial, sans-serif;
      color: #232525;
      padding-top:70px;
    }

    #slides {
      display: none
    }

    #slides .slidesjs-navigation {
      margin-top:5px;
    }

      a.slidesjs-next,
      a.slidesjs-previous,
      a.slidesjs-play,
      a.slidesjs-stop {
         
          background-image:url(/Image/btns-next-prev.png); /*url(../Image/btns-next-prev.png);*/
          background-repeat: no-repeat;
          display: block;
          width: 12px;
          height: 18px;
          overflow: hidden;
          text-indent: -9999px;
          float: left;
          margin-right: 5px;
      }

    a.slidesjs-next {
      margin-right:10px;
      background-position: -12px 0;
    }

    a:hover.slidesjs-next {
      background-position: -12px -18px;
    }

    a.slidesjs-previous {
      background-position: 0 0;
    }

    a:hover.slidesjs-previous {
      background-position: 0 -18px;
    }

    a.slidesjs-play {
      width:15px;
      background-position: -25px 0;
    }

    a:hover.slidesjs-play {
      background-position: -25px -18px;
    }

    a.slidesjs-stop {
      width:18px;
      background-position: -41px 0;
    }

    a:hover.slidesjs-stop {
      background-position: -41px -18px;
    }

    .slidesjs-pagination {
      margin: 7px 0 0;
      float: right;
      list-style: none;
    }

    .slidesjs-pagination li {
      float: left;
      margin: 0 1px;
    }

    .slidesjs-pagination li a {
      display: block;
      width: 13px;
      height: 0;
      padding-top: 13px;
      background-image: url(/Image/pagination.png);
      background-position: 0 0;
      float: left;
      overflow: hidden;
    }

    .slidesjs-pagination li a.active,
    .slidesjs-pagination li a:hover.active {
      background-position: 0 -13px
    }

    .slidesjs-pagination li a:hover {
      background-position: 0 -26px
    }

    #slides a:link,
    #slides a:visited {
      color: #333
    }

    #slides a:hover,
    #slides a:active {
      color: #9e2020
    }

    .navbar {
      overflow: hidden
    }
  </style>
  <!-- End SlidesJS Optional-->

  <!-- SlidesJS Required: These styles are required if you'd like a responsive slideshow -->
  <style>
    #slides {
      display: none
    }

    .container {
      margin: 0 auto
    }

    /* For tablets & smart phones */
    @media (max-width: 767px) {
      body {
        padding-left: 20px;
        padding-right: 20px;
      }
      .container {
        width: auto
      }
    }

    /* For smartphones */
    @media (max-width: 480px) {
      .container {
        width: auto
      }
    }

    /* For smaller displays like laptops */
    @media (min-width: 768px) and (max-width: 979px) {
      .container {
        width: 724px
      }
    }

    /* For larger displays */
    @media (min-width: 1200px) {
      .container {
        width: 1170px
      }
    }
  </style>
  <!-- SlidesJS Required: -->
</head>
<body>


<%--  <div>
        
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="left">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td rowspan="2" style="padding-left:4px; padding-right:4px; padding-top:4px; border-bottom:solid 1px #999999;">
                                <br />
                                <img alt="Logo" src="../../Image/logo.png" /><br />
                                <br />
                            </td>
                            <td align="center" colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td valign="bottom" align="left">
                                <asp:Menu 
                                    ID="menuMain" 
                                    runat="server" 
                                    RenderingMode="Table"
                                    staticdisplaylevels="1"
                                    Orientation="Horizontal"
                                    StaticMenuItemStyle-ItemSpacing="0"
                                    StaticEnableDefaultPopOutImage="true"      
                                    StaticTopSeparatorImageUrl="~/Images/LeftCorner.gif"
                                    StaticBottomSeparatorImageUrl="~/Images/RightCorner.gif"
                                    StaticMenuItemStyle-CssClass="mnuNormal"
                                    StaticHoverStyle-CssClass="mnuHover"
                                    StaticSelectedStyle-CssClass="mnuSelected"
                                    DynamicMenuItemStyle-CssClass="mnuDynamic"
                                    DynamicMenuStyle-CssClass="mnuMain"
                                    DynamicHoverStyle-CssClass="mnuHover">
                                    <Items>
                                        <asp:MenuItem Text="Projects" Value="Project" NavigateUrl="~/Modules/Projects/ClientProjectDetails.aspx" />
                                        <asp:MenuItem Text="Help" Value="Help" NavigateUrl="~/Modules/Help/Help.aspx" />
                                      
                                    </Items>
                                </asp:Menu>
                            </td>
                            <td style="width:45px; height:1px; border-bottom:solid 1px #999999;"><img alt="." src="../../Images/1x1.gif" /></td>
                        </tr>
                    </table>
                </td>
                <td align="right" style="border-bottom:solid 1px #999999; width:100%;">
                    <table cellpadding="0" cellspacing="1" width="100%">
                        <tr>
                            <td rowspan="3" align="center"><strong><span style="font-size:16pt; font-family:Verdana,Arial; white-space:nowrap;" class="lstTextNormal">SOS</span><span style="font-size:16pt; color:#ff0000; font-family:Verdana,Arial; white-space:nowrap;">++</span></strong></td>
                            <td class="frmTextBold"><asp:Label ID="lblSubcontractor" runat="server" CssClass="frmLinkSmall"></asp:Label>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="frmText"><asp:HyperLink ID="lnkUser" runat="server" CssClass="frmLinkSmall"></asp:HyperLink>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="frmText"><asp:LinkButton ID="lnkLogout" runat="server" CssClass="frmLinkSmall" OnClick="lnkLogout_Click">Logout</asp:LinkButton>&nbsp;</td>
                        </tr>
                    </table>                    
                </td>
            </tr>
        </table>


    </div>
--%>



  <!-- SlidesJS Required: Start Slides -->
  <!-- The container is used to define the width of the slideshow -->
  <div class="container">
    <div id="slides" runat="server">
        
    </div>
  </div>
  <!-- End SlidesJS Required: Start Slides -->

  <!-- SlidesJS Required: Link to jQuery -->
  <script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
  <!-- End SlidesJS Required -->

  <!-- SlidesJS Required: Link to jquery.slides.js -->
    <script src="../../JQuery/jquery.slides.min.js"></script>
  <!-- End SlidesJS Required -->

  <!-- SlidesJS Required: Initialize SlidesJS with a jQuery doc ready -->
  <script>
    $(function() {
      $('#slides').slidesjs({
        width: 940,
        height: 528,
        play: {
          active: true,
          auto: true,
          interval: 4000,
          swap: true
        }
      });
    });
  </script>
  <!-- End SlidesJS Required -->



</body>
</html>
