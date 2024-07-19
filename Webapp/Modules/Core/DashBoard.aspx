<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="SOS.Web.DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <link href="~/Config/StyleSheet.css" rel="stylesheet" />
    
    
    <table width="100%">

     <%--ROW1   --%> <tr><td class="frmTitle" style="font-size:20px;">Project Thumbnails</td></tr><%--ROW 1  color:#f1761e; #cbcbce--%>
     <%--ROW underline   --%>   <tr> <td style="border-bottom:thin solid #000080;"><img alt="-" src="../../Images/1x1.gif" height="1"/></td></tr>
    <%--ROW 2   --%><tr><td></td></tr> <%--ROW 2   --%>

    <%--ROW 3   --%><tr><td></td></tr> <%--ROW 3   --%>

    </table>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <table class="ptTable">
        <tr class="ptHeaderRow">
            <td colspan="3">
                <asp:HyperLink ID="HyperLink1" runat="server">221 Pelham</asp:HyperLink>
            </td>
            
        </tr>
        <tr>
            <td class="ptImageCell">
                <img alt="" src="../../Images/building.GIF" class="ptImage" />
            </td>
            <td class="ptDetailsCell" colspan="2">
                <br />223 Pelham Street
                <br />VIC 3053
            </td>
        </tr>
        <tr  class="ptFooterRow">
            
                
                        <td style="width:30%"></td>
                        <td style="width:40%"> Active|17-612</td>
                        <td style="width:30%;text-align:right">
                            <asp:HyperLink ID="ImageButton1" runat="server" ImageUrl="~/Images/IconAdd.gif" NavigateUrl="~/Modules/Core/DashBoardDetails.aspx" /> </td>
                    </tr>
        </table>

      <br /><br />
    <br />



    <table class="ptTable">
        <tr class="ptHeaderRow">
            <td colspan="3">
                <asp:HyperLink ID="HyperLink2" runat="server">221 Pelham</asp:HyperLink>
            &nbsp;XYZ</td>
            
        </tr>
        <tr>
            <td class="ptImageCell">
                <img alt="" src="../../Images/building.GIF" class="ptImage" />
            </td>
            <td class="ptDetailsCell" colspan="2">
                <br />223 Pelham Street
                <br />VIC 3053
            </td>
        </tr>
        <tr  class="ptFooterRow111">
            
                
                        <td style="width:30%"></td>
                        <td style="width:40%"> Active|17-612</td>
                        <td style="width:30%;text-align:right">
                            <asp:HyperLink ID="HyperLink3" runat="server" ImageUrl="~/Images/IconAdd.gif" NavigateUrl="~/Modules/Core/DashBoardDetails.aspx" /> </td>
                    </tr>
        </table>


     <br />
<%--    <table cellpadding="0" cellspacing="0" class="auto-style1">
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">
                <asp:Image ID="Image1" runat="server" />
                <asp:Image ID="Image2" runat="server" />
                <asp:Image ID="Image3" runat="server" />
            </td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">Comparison</td>
            <td class="auto-style3">
                <asp:Label ID="Label1" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label3" runat="server" Text="Label" class="lbl" ></asp:Label>
            </td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">Variation Orders </td>
            <td class="auto-style3">
                <asp:Label ID="Label4" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label5" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label6" runat="server" Text="Label" class="lbl" ></asp:Label>

            </td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">RFIs</td>
            <td class="auto-style3">
                <asp:Label ID="Label7" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label8" runat="server" Text="Label" class="lbl"></asp:Label>
                <asp:Label ID="Label9" runat="server" Text="Label" class="lbl" ></asp:Label>

            </td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style4">&nbsp;</td>
        </tr>
    </table>--%>
    <br />
    <br />



</asp:Content>




<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .lbl {
            float: left;margin:0px;padding:0px;border:1px;
            color:white;
            font-family:Verdana, Arial;/*'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif;*/
            text-align:center;
            font-size:12px;
            border-color:#838181;
        }
    </style>
</asp:Content>





