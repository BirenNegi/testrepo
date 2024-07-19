<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SOS.Modules.Gallery.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Basic jQuery Slider - Demo</title>

    <!-- bjqs.css contains the *essential* css needed for the slider to work -->
     <link href="../../Content/CSS/bjqs.css" rel="stylesheet" />
    <link href="../../Content/CSS/demo.css" rel="stylesheet" />
    <%--<link href="bjqs.css" rel="stylesheet" />--%>
    <!-- some pretty fonts for this demo page - not required for the slider -->
    <link href='http://fonts.googleapis.com/css?family=Source+Code+Pro|Open+Sans:300' rel='stylesheet' type='text/css'>

    <!-- demo.css contains additional styles used to set up this demo page - not required for the slider -->
    
    <%--<link href="demo.css" rel="stylesheet" />--%>
    <!-- load jQuery and the plugin -->
    <script src="http://code.jquery.com/jquery-1.7.1.min.js"></script>
    <script src="js/bjqs-1.3.min.js"></script>







</head>
<body>
    <form id="form1" runat="server">
        <div id="container">

            <h2>Slider from Folder ASP With Arka</h2>

            <!--  Outer wrapper for presentation only, this can be anything you like -->
            <div id="banner-fade">

                <!-- start Basic Jquery Slider -->
                <ul class="bjqs" id="bjqs1">
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <li>
                                <img src='<%# DataBinder.Eval(Container.DataItem,"Value") %>' title='<%# (DataBinder.Eval(Container.DataItem,"Text").ToString()).Split('.')[0].ToString() %>' alt="">

                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <!-- end Basic jQuery Slider -->

            </div>
            <!-- End outer wrapper -->

            <script class="secret-source" style="display:none;">
                jQuery(document).ready(function ($) {
                    alert("1");
//san--
                    var Slides = document.getElementById("bjqs1").children;
                    alert(Slides.length);


//San--
                      $('#banner-fade').bjqs({
                        height: 320,
                        width: 620,
                        responsive: true
                    });
                    alert("3");
                });
      </script>
        </div>
        <script src="js/libs/jquery.secret-source.min.js"></script>
        <script>
            jQuery(function ($) {
                alert("2");
                $('.secret-source').secretSource({
                    includeTag: false
                });

            });
    </script>
    </form>
</body>
</html>
