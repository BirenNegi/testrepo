<%--<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" CodeBehind="PhotoGallery1.aspx.cs" Inherits="SOS.Modules.Projects.PhotoGallery1" %>--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotoGallery1.aspx.cs" Inherits="SOS.Web.PhotoGallery1" %>
<html>
<head></head>
<body>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">--%>

   <h2 class="w3-center">Image Slideshow</h2>
    <link href="../../Style/ImageSlider.css" rel="stylesheet" />
<%--<link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css"/>--%>
<div class="w3-content w3-display-container" id="Maindiv" runat="server">
    <%--<img class="mySlides" src="../../ProjectImages/ALDI%20-%20Dandenong%20-%20WH%20&%20DC/banner01.jpg" style="width:100%" alt=""/>
    <img class="mySlides" src="../../ProjectImages/ALDI%20-%20Dandenong%20-%20WH%20&%20DC/banner02.jpg" style="width:100%"  alt=""/>
    <img class="mySlides" src="../../ProjectImages/ALDI%20-%20Dandenong%20-%20WH%20&%20DC/banner03.jpg" style="width:100%"  alt=""/>--%>
  <button class="w3-button w3-black w3-display-left" onclick="plusDivs(-1)">&#10094;</button>
  <button class="w3-button w3-black w3-display-right" onclick="plusDivs(1)">&#10095;</button>
</div>

<script type="text/javascript">
var slideIndex = 1;
showDivs(slideIndex);

function plusDivs(n) {
    //alert(n);
  showDivs(slideIndex += n);
}

function showDivs(n) {
  var i;
  var x = document.getElementsByClassName("mySlides");
  //alert(n);
   
  if (n > x.length) {slideIndex = 1}    
  if (n < 1) { slideIndex = x.length }
  //else {slideIndex += n; }
  for (i = 0; i < x.length; i++) {
     x[i].style.display = "none";  
  }
  slideIndex++;
  if (slideIndex > x.length) { slideIndex = 1; }
  x[slideIndex - 1].style.display = "block";
  setTimeout(showDivs, 5000);
}
</script>
</body>

</html>
