<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gallery.aspx.cs" Inherits="SOS.Web.Gallery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    

</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdn.jsdelivr.net/jcarousel/0.2.8/jquery.jcarousel.min.js"></script>
    <link href="http://cdn.jsdelivr.net/jcarousel/0.2.8/skins/tango/skin.css" rel="Stylesheet" />
    <script type="text/javascript">
        $(function () {
            $('#mycarousel').jcarousel();
        });
    </script>
    <ul id="mycarousel" class="jcarousel-skin-tango">
        <asp:Repeater ID="rptImages" runat="server">
            <ItemTemplate>
                <li>
                    <img alt="" style='height: 75px; width: 75px' src='<%# Eval("Value") %>' />
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>

    </form>
</body>
</html>
