<%@ Page  Language="C#" MasterPageFile="~/MasterPages/User.master" AutoEventWireup="true" CodeBehind="EditSiteInduction.aspx.cs" Title="Manage Site Induction"  Inherits="SOS.Web.EditSiteInduction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  <script language="javascript" type="text/javascript">
     
      window.onload = HideRows;
      
      function HideRows()
      {
          rowsOptlQA = document.getElementById("MainContent_gvOptlQA").rows;
          rowDoc = document.getElementById("MainContent_gvSiteInductionDocuments").rows;
          rowYesNo = document.getElementById("MainContent_gvYesNoQA").rows;

          HideShowColmns(rowDoc);
          HideShowColmns(rowsOptlQA);
          HideShowColmns(rowYesNo);
      }

    function HideShowColmns(rows)
         {
         // rows = document.getElementById("MainContent_gvOptlQA").rows;
           for (i = 0; i < rows.length; i++)
           {
              rows[i].cells[0].style.display = "none";   
           }
        }

</script>



    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <sos:TitleBar ID="TitleBar" runat="server" Title="Manage Site Induction" />

    <table  cellspacing="1" cellpadding="1">
      <tr><td>
             <table class="frmForm" width="100%">
                    <tr>
                        <td class="frmLabel" style="font-weight:bold; width: 200px">Site Induction Booklets/Docs :</td>
                        <td class="frmData">
                           <br /> <sos:FileSelect ID="sfsSiteInductionDocs" runat="server" /> &nbsp; 
                             <asp:Button  ID="btnDocsAdd" Text="Add" runat="server" CssClass="frmButton" Height="25px" Width="56px" OnClick="btnDocsAdd_Click"/>&nbsp;<br /><br /> </td>
                    </tr>
                    <tr>
                        <td class="frmLabel" style="font-weight: bold; width: 200px;">&nbsp;</td>
                        <td class="frmData" style="font-weight:300">
                          <asp:GridView ID="gvSiteInductionDocuments" runat="server"
                            DataKeyNames="Id"
                            AutoGenerateColumns = "False"
                            CellPadding = "4"
                            CellSpacing = "0"
                            CssClass = "lstList"
                            RowStyle-CssClass = "lstItem"
                            EmptyDataText = "None yet."
                            EmptyDataRowStyle-CssClass = "lstSubTitle"
                            AlternatingRowStyle-CssClass = "lstAltItem"
                            OnRowDeleting="gvSiteInductionDocuments_RowDeleting" Width="600px">
                           
                             <Columns> 
                                <asp:BoundField DataField="Id" HeaderText="Id"> <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="1%" />  </asp:BoundField>
                                <asp:BoundField DataField="FilePath" HeaderText="FilePath" SortExpression="FilePath" >
                   
                    <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" />
                   
                    </asp:BoundField>
                                <asp:TemplateField HeaderText="Delete" >
                        <HeaderStyle CssClass="lstHeader" />
                    <ItemStyle Width="1%" />
    <ItemTemplate>
        <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl="~/Images/IconDelete.gif"
                    CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this filepath?');"
                    AlternateText="Delete" />               
    </ItemTemplate>
</asp:TemplateField> 
                   
                            </Columns>
                            <RowStyle CssClass="lstItem" />
                        </asp:GridView>
                        <br />
                        <br />
                        </td>
                    </tr>
             </table>

      </td></tr>

      <tr><td> <br />
             <table class="frmForm" width="100%">
                 <tr>
        <td class="frmLabel" style="font-weight: bold; width: 200px;">Note:</td>
        <td class="frmData"><table><tr>
                <td>
                    <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Width="520px" Height="65px"></asp:TextBox>
               </td>
                <td> <asp:Button ID="btnNote" runat="server" Text="Submit" OnClick="btnNote_Click" /></td>

                 </tr></table>
            <br /></td>
    </tr>
   
                 <tr valign="top">
        <td class="frmLabel" style="font-weight: bold; width: 200px;">
            <br />
            Optional Questions and Answers:<br />
            <br />   &nbsp;</td>
        <td class="frmData">
            <br />
            <asp:TextBox ID="TxtQuestion" runat="server" Height="36px" TextMode="MultiLine" Width="520px" placeholder="Question"></asp:TextBox>
&nbsp;<br />
            <br />
            
            (a)<asp:TextBox ID="txtA" runat="server" Width="140px" placeholder="Option A" CssClass="frmText"></asp:TextBox>
            &nbsp; &nbsp; &nbsp; &nbsp; 
           
            (b)<asp:TextBox ID="txtB" runat="server" Width="140px" placeholder="Option B" CssClass="frmText"></asp:TextBox>
            &nbsp; 
            <br />
            <br />
            (c)<asp:TextBox ID="txtC" runat="server" Width="140px" placeholder="Option C" CssClass="frmText"></asp:TextBox>
            &nbsp; &nbsp; &nbsp; &nbsp; 
           
            (d)<asp:TextBox ID="txtD" runat="server" Width="140px" placeholder="Option D" CssClass="frmText"></asp:TextBox>
           &nbsp; &nbsp; 
             Right Answer:  <asp:DropDownList ID="dpdRightA" runat="server" Height="24px" Width="65px" CssClass="frmTextBold">
                <asp:ListItem>Select</asp:ListItem>
                <asp:ListItem>a</asp:ListItem>
                <asp:ListItem>b</asp:ListItem>
                <asp:ListItem>c</asp:ListItem>
                <asp:ListItem>d</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            &nbsp;&nbsp;
            
            <asp:Button ID="BtnOptlQA" runat="server" Text="Add" Width="125px" CssClass="frmButton" Height="26px" OnClick="BtnOptlQA_Click" />
           
            <br />
            <br />
            </td>
    </tr>

                 <tr>
        <td class="frmLabel" style="font-weight: bold; width: 200px;">&nbsp;</td>
        <td class="frmData" style="font-weight:300">
            <asp:GridView ID="gvOptlQA" runat="server" DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                AlternatingRowStyle-CssClass = "lstAltItem" OnRowDeleting="gvOptlQA_RowDeleting" Width="600px">
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" >
                        <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="1%" /> </asp:BoundField >
                    <%--<asp:TemplateField HeaderText="Question" SortExpression="Question">
                        
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Question") %>'></asp:Label>
                            <asp:TextBox ID="txtQuestion" runat="server" Text='<%#Eval("Question")%>' TextMode="MultiLine" ReadOnly="true" style="" BackColor="Transparent" BorderStyle="None" Width="100%"></asp:TextBox> 

                        </ItemTemplate>
                        <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" />
                        <ItemStyle Width="35%" />
                    </asp:TemplateField>--%>
                     <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question">
                         <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="40%" Wrap="True" CssClass="sanwrap" VerticalAlign="Top" /> </asp:BoundField >
                
                    <asp:BoundField DataField="Opt1" HeaderText="(a)" SortExpression="Opt1">
                         <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="10%" Wrap="True" CssClass="sanwrap" VerticalAlign="Top" /> </asp:BoundField >
                    <asp:BoundField DataField="Opt2" HeaderText="(b)" SortExpression="Opt2">
                           <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="10%" Wrap="True" CssClass="sanwrap" VerticalAlign="Top" /> </asp:BoundField > 
                    <asp:BoundField DataField="Opt3" HeaderText="(c)" SortExpression="Opt3">
                        <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="10%" Wrap="True" CssClass="sanwrap" VerticalAlign="Top" /> </asp:BoundField >
                    <asp:BoundField DataField="Opt4" HeaderText="(d)" SortExpression="Opt4">
                         <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="10%" Wrap="True" CssClass="sanwrap" VerticalAlign="Top"/> </asp:BoundField >
                    <asp:BoundField DataField="RightAnswer" HeaderText="Ans" SortExpression="RightAnswer" >
                         <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="2%" /> </asp:BoundField >


                    <asp:TemplateField HeaderText="Delete" >
                        <HeaderStyle CssClass="lstHeader" />
                        <ItemStyle Width="1%" />
                          <ItemTemplate>
                            <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl="~/Images/IconDelete.gif"
                                CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this Question?');"
                                AlternateText="Delete" />               
                            </ItemTemplate>
                    </asp:TemplateField> 

                </Columns>

<EmptyDataRowStyle CssClass="lstSubTitle"></EmptyDataRowStyle>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>
            <br />
            </td>
    </tr>

                 
            </table>

      </td></tr>

      <tr><td> <br />
             <table class="frmForm" width="100%">
                <tr valign="top">
        <td class="frmLabel" style="font-weight: bold; width: 200px;">YES/NO Questions :<br />
            &nbsp;
            <br />
            <br />
            <br />
            <br />
            Comments on No : </td>
        <td class="frmData"  >
           <table><tr><td> 
            <asp:TextBox ID="TxtYesNo" runat="server" Height="40px" TextMode="MultiLine" Width="474px" placeholder="Question"></asp:TextBox>
               <br />
            </td><td valign="top" style="vertical-align: top" rowspan="2">
                   <br />
                   <br />
            <asp:Button  ID="BtnYesNo" Text="Add" runat="server" CssClass="frmButton" Height="28px" Width="56px" OnClick="BtnYesNo_Click" />
             </td>
           </tr><tr><td> 
            <asp:TextBox ID="TxtComments" runat="server" Height="40px" TextMode="MultiLine" Width="474px" placeholder="Comments"></asp:TextBox>
                   <br />
            </td>
           </tr></table>
            <br />    
            

<%--<asp:TextBox ID="txtTinyMCE" runat="server" TextMode="MultiLine" Width="517px"></asp:TextBox>
<br />
<asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click"  />
<hr />
<asp:Label ID="lblContent" runat="server"></asp:Label>
<script type="text/javascript" src="https://tinymce.cachefly.net/4.0/tinymce.min.js"></script>
<script type="text/javascript">
    tinymce.init({ selector: 'textarea', width: 500 });
</script>--%>





         </td>
    </tr>
   
                <tr>
        <td class="frmLabel">&nbsp;</td>
        <td class="frmData" style="font-weight:300">&nbsp;&nbsp;
            <asp:GridView ID="gvYesNoQA" runat="server" DataKeyNames="Id"
                AutoGenerateColumns = "False"
                CellPadding = "4"
                CssClass = "lstList"
                RowStyle-CssClass = "lstItem"
                EmptyDataText = "None yet."
                EmptyDataRowStyle-CssClass = "lstSubTitle"
                AlternatingRowStyle-CssClass = "lstAltItem" OnRowDeleting="gvYesNoQA_RowDeleting" width="600px">
<AlternatingRowStyle CssClass="lstAltItem"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" >
                        <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="1%" /> </asp:BoundField >
                    <%--<asp:TemplateField HeaderText="Question" SortExpression="Question">
                        
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Question") %>'></asp:Label>
                            <asp:TextBox ID="txtQuestion" runat="server" Text='<%#Eval("Question")%>' TextMode="MultiLine" ReadOnly="true" style="" BackColor="Transparent" BorderStyle="None" Width="100%"></asp:TextBox> 

                        </ItemTemplate>
                        <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" />
                        <ItemStyle Width="55%" />
                    </asp:TemplateField>--%>

                    <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question" >
                        <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="45%"  Wrap="True" CssClass="sanwrap" VerticalAlign="Top"/> </asp:BoundField >

                     <asp:BoundField DataField="Comments" HeaderText="Question" SortExpression="Comments" >
                        <HeaderStyle CssClass="lstHeader" HorizontalAlign="Left" /> <ItemStyle Width="45%"  Wrap="True" CssClass="sanwrap" VerticalAlign="Top"/> </asp:BoundField >


                    <asp:TemplateField HeaderText="Delete" >
                        <HeaderStyle CssClass="lstHeader" />
                        <ItemStyle Width="1%" />
                          <ItemTemplate>
                            <asp:ImageButton ID="DeleteBtn" runat="server" ImageUrl="~/Images/IconDelete.gif"
                                CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this Question?');"
                                AlternateText="Delete" />               
                            </ItemTemplate>
                    </asp:TemplateField> 

                </Columns>

<EmptyDataRowStyle CssClass="lstSubTitle"></EmptyDataRowStyle>

<RowStyle CssClass="lstItem"></RowStyle>
            </asp:GridView>
            </td>
    </tr>
   
            </table>

      </td></tr>
   
</table>

</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="Scripts">
    <style type="text/css">
        .sanwrap {
            word-wrap:break-word;
            white-space:normal;
        }
    </style>
</asp:Content>
