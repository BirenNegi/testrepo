<%@ Page Title="Projects" Language="C#" MasterPageFile="~/MasterPages/Client.master" AutoEventWireup="true" CodeBehind="ClientProjects.aspx.cs" Inherits="SOS.Web.ClientProjects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../../Style/StyleSheet.css" rel="stylesheet" />
     <style>

         .ptHeaderRow a { 
             color:#036bba;
             font-family:Verdana, Arial;
             font-weight:bold;
             font-size:9pt;
             white-space:nowrap;
         }

        .ptRow {   
             font-family:Verdana, Arial;
             font-weight:bold;
             font-size:8pt;
             white-space:nowrap;
         }
         .ptImageCell {
             content:url("../../Images/building.GIF");
         }

    </style>
    <script type="text/javascript">

        function RedirectToNextPage()
        {

            alert("Hi");}

    </script>


    <table runat="server" id="maintable" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <br />
            </td>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
            <td id="td2"><br />

              <%--  <table class="ptTable">
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
                </table>--%>

            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
            <td>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>


