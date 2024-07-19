<%@ Page Language="C#" AutoEventWireup="true" Inherits="SOS.Web.DefaultPage" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SOS++</title>

  

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SOSTestConnectionString %>" SelectCommand="SELECT [Name] FROM [Addendums]"></asp:SqlDataSource>
    
    </div>





 <table style="width:100%">
<tbody>
<tr><td style="width:50%"><span style="font-size: 8pt;">[SOS:Editable:B1 Title:B1] Stage 1[/SOS:Editable]</span><br /></td><td style="width:50%"><span style="font-size: 8pt;">[SOS:Editable:D1 Title:D1] dd/mm/yyyy [/SOS:Editable]</span><br /> </td></tr>
<tr><td><span style="font-size: 8pt;">[SOS:Editable:B2 Title:B2] [/SOS:Editable]</span><br /></td><td><span style="font-size: 8pt;">[SOS:Editable:D2 Title:D2]  [/SOS:Editable]</span><br /></td></tr>
<tr><td><span style="font-size: 8pt;">[SOS:Editable:B3 Title:B3] [/SOS:Editable]</span><br /></td><td><span style="font-size: 8pt;">[SOS:Editable:D3 Title:D3]  [/SOS:Editable]</span><br /></td></tr>
<tr><td><span style="font-size: 8pt;">[SOS:Editable:B4 Title:B4] [/SOS:Editable]</span><br /></td><td><span style="font-size: 8pt;">[SOS:Editable:D4 Title:D4]  [/SOS:Editable]</span><br /></td></tr>
<tr><td><span style="font-size: 8pt;">[SOS:Editable:B5 Title:B5] [/SOS:Editable]</span><br /></td><td><span style="font-size: 8pt;">[SOS:Editable:D5 Title:D5]  [/SOS:Editable]</span><br /></td></tr>
<tr><td><span style="font-size: 8pt;">[SOS:Editable:B6 Title:B6] [/SOS:Editable]</span><br /></td><td><span style="font-size: 8pt;">[SOS:Editable:D6 Title:D6]  [/SOS:Editable]</span><br /></td></tr>
<tr><td><span style="font-size: 8pt;">[SOS:Editable:B7 Title:B7] [/SOS:Editable]</span><br /></td><td><span style="font-size: 8pt;">[SOS:Editable:D7 Title:D7]  [/SOS:Editable]</span><br /></td></tr>
<tr><td><span style="font-size: 8pt;">[SOS:Editable:B8 Title:B8] [/SOS:Editable]</span><br /></td><td><span style="font-size: 8pt;">[SOS:Editable:D8 Title:D8]  [/SOS:Editable]</span><br /> </td></tr>
</tbody>

 </table>






 <table style="width:100%">
<tbody>
<tr><td style="width:50%">[SOS:Editable:A Title:A] Milestone A[/SOS:Editable]<br /></td> <td style="width:50%">[SOS:Editable:E1 Title:E1] dd/mm/yyyy [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:B Title:B] [/SOS:Editable]<br /></td><td>[SOS:Editable:E2 Title:E2]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:C Title:C] [/SOS:Editable]<br /></td><td>[SOS:Editable:E3 Title:E3]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:D Title:D] [/SOS:Editable]<br /></td><td>[SOS:Editable:E4 Title:E4]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:E Title:E] [/SOS:Editable]<br /></td><td>[SOS:Editable:E5 Title:E5]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:F Title:F] [/SOS:Editable]<br /></td><td>[SOS:Editable:E6 Title:E6]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:G Title:G] [/SOS:Editable]<br /></td><td>[SOS:Editable:E7 Title:E7]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:H Title:H] [/SOS:Editable]<br /></td><td>[SOS:Editable:E8 Title:E8]  [/SOS:Editable]<br /></td></tr>
</tbody>
</table>






<table style="width:100%">
<tbody>
<tr><td style="width:50%">[SOS:Editable:Q1 Title:Q1]Stage 1 liquidated damages [/SOS:Editable]<br /> </td><td style="width:50%">[SOS:Editable:R1 Title:R1]$ [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:Q2 Title:Q2] [/SOS:Editable]<br /></td><td>[SOS:Editable:R2 Title:R2] [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:Q3 Title:Q3] [/SOS:Editable]<br /></td><td>[SOS:Editable:R3 Title:R3] [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:Q4 Title:Q4] [/SOS:Editable]<br /></td><td>[SOS:Editable:R4 Title:R4] [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:Q5 Title:Q5] [/SOS:Editable]<br /></td><td>[SOS:Editable:R5 Title:R5] [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:Q6 Title:Q6] [/SOS:Editable]<br /></td><td>[SOS:Editable:R6 Title:R6] [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:Q7 Title:Q7] [/SOS:Editable]<br /></td><td>[SOS:Editable:R7 Title:R7] [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:Q8 Title:Q8] [/SOS:Editable]<br /></td><td>[SOS:Editable:R8 Title:R8] [/SOS:Editable]<br /></td></tr>
</tbody>

</table>




<table style="width:100%">
<tr><td style="width:50%">[SOS:Editable:G1 Title:G1]Milestone A liquidated damages [/SOS:Editable]<br /> </td> <td style="width:50%">[SOS:Editable:H1 Title:H1]$ [/SOS:Editable]<br /> </td></tr>
<tr><td>[SOS:Editable:G2 Title:G2]  [/SOS:Editable]<br /> </td> <td>[SOS:Editable:H2 Title:H2]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:G3 Title:G3]  [/SOS:Editable]<br /> </td> <td>[SOS:Editable:H3 Title:H3]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:G4 Title:G4]  [/SOS:Editable]<br /> </td> <td>[SOS:Editable:H4 Title:H4]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:G5 Title:G5]  [/SOS:Editable]<br /> </td> <td>[SOS:Editable:H5 Title:H5]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:G6 Title:G6]  [/SOS:Editable]<br /> </td> <td>[SOS:Editable:H6 Title:H6]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:G7 Title:G7]  [/SOS:Editable]<br /> </td> <td>[SOS:Editable:H7 Title:H7]  [/SOS:Editable]<br /></td></tr>
<tr><td>[SOS:Editable:G8 Title:G8]  [/SOS:Editable]<br /> </td> <td>[SOS:Editable:H8 Title:H8]  [/SOS:Editable]<br /></td></tr>

</table>










































        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />










































    </form>
    <p>
&nbsp;&nbsp; o</p>
</body>
</html>
