<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="ReplyRFI.aspx.cs" Inherits="SOS.Web.ReplyRFI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Scripts" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 50%;
            padding-left:15px;
        }
        
        .auto-style2 {
            height: 25px;
        }
        
    </style>

      <script type="text/jscript">
     


          function displayFile() {
              var fileUpload = document.getElementById("MainContent_FileUpload1");
              var td = document.getElementById("Tdfile");
              var FileSize = 0;
              td.innerHTML = "";
              if (fileUpload.files.length > 0)
              {
                  td.innerHTML = "<br /> <span style='text-decoration: underline'><Strong> Selected Files</Strong></span> <br /> <br /> ";
                  for (i = 0; i < fileUpload.files.length; i++) {
                      td.innerHTML += "<li>" + fileUpload.files[i].name + ",&nbsp;&nbsp&nbsp;&nbsp; size:" + (fileUpload.files[i].size / (1024 * 1024)).toFixed(2) + 'MB' + " </ li> <br />";
                      FileSize += (fileUpload.files[i].size / (1024 * 1024));
                  }

                  td.innerHTML += "<br /> <Strong>Files count:" + fileUpload.files.length + "&nbsp;&nbsp&nbsp;&nbsp; size:" + FileSize.toFixed(2) + 'MB' + "</Strong><br /><br />  ";

                  if (FileSize > 10)
                  {
                      alert("File size is >10 MB,Please use alternative way to send the files...");
                      td.innerHTML = "";
                      fileUpload.value="";
                      
                  }
              }


          }


      </script>



</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



    <table cellspacing="1" class="auto-style1">
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="frmForm">
                 <table width="100%">
				<tr>
					<td class="frmLabel">Number:</td>
					<td class="frmData"><asp:Label ID="lblNumber" runat="server"></asp:Label></td>
                    <%--<td class="frmLabel"></td>--%>
                    <td class="frmLabel">Date Raised:</td>
					<td class="frmData"><asp:Label ID="lblRaiseDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Created by:</td>
					<td class="frmData"><asp:Label ID="lblSigner" runat="server"></asp:Label></td>
                    <%--<td class="frmLabel"></td>--%>
                    <td class="frmLabel">Answer by date:</td>
					<td class="frmData"><asp:Label ID="lblTargetAnswerDate" runat="server"></asp:Label></td>
				</tr>
				<tr>
					<td class="frmLabel">Status:</td>
					<td class="frmData"><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                    <%--<td class="frmLabel"></td>--%>
                    <td class="frmLabel">Date Answered:</td>
					<td class="frmData"><asp:Label ID="lblActualAnswerDate" runat="server"></asp:Label></td>
				</tr>
               
                <tr>
                    <td class="frmLabel">Subject:</td>
                    <td class="frmData" colspan="4"><asp:Label ID="lblSubject" runat="server"></asp:Label></td>
                </tr>

                 <%--<tr>
                     <td  colspan="5" ></td>
                 </tr>
                 
                 <tr>
                     <td  colspan="5"></td>
                </tr>--%>
                <tr>
                   <td  class="frmLabel">Attach File:</td>
                   <td colspan="4" class="frmData">   
                     <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="True" Width="539px"  onchange="displayFile();"/>
                    </td>
                </tr>



            <tr>
               <td  colspan="5" id="Tdfile"  class="frmData">  </td>
            </tr>
            <tr>
                <td  class="frmLabel" style="vertical-align:top">Message:</td>
                <td colspan="4" class="frmData">
                    <asp:TextBox ID="Txtmessage" runat="server" Height="285px" Width="790px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

             <tr class="frmData">  <td class="auto-style2"></td>
            <td style="text-align:right; padding-right:50px;" colspan="4" class="auto-style2">
                 </td>
        </tr>

             <tr class="frmData">  <td></td>
            <td style="text-align:right; padding-right:50px;" colspan="4">
                <asp:Button ID="btnSend" runat="server" Text="Send" Width="89px" OnClick="btnSend_Click" />
            </td>
        </tr>

            </table>
          
            </td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
        </tr>
        
        
        <tr>
            <td>
                <%--<asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />--%>
            </td>
        </tr>

        <tr>
            <td>
<%--                <asp:FileUpload ID="FileUpload2" runat="server" />
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Upload" />--%>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
    </table>



</asp:Content>
