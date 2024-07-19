using System;
using System.Web;
using SOS.Core;
using System.Web.UI.WebControls;

using System.IO;
using System.Collections.Generic;


namespace SOS.Web
{
    public partial class EditClientContactAccess : SOSPage    //--System.Web.UI.Page 
    {


        #region Members
        private ClientContactInfo clientContactInfo = null;
        private String Projectid;
        private ProjectInfo projInfo = null;
        #endregion

        #region Private Methods

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;
   

            int? ProjectId = int.Parse(Request.QueryString["ProjectId"].ToString());

            tempNode.ParentNode.Url += "?ProjectId=" + ProjectId.ToString();
            projInfo = ProjectsController.GetInstance().GetProject(ProjectId);
            tempNode.ParentNode.Title = projInfo.Name;

            return currentNode;
        }




        private void ObjectsToForm()
        {
            if (clientContactInfo.Id == null)
            {
                TitleBar.Title = "Manage Client's Contact";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Client's Contact";
            }

            txtEmail.Text = UI.Utils.SetFormString(clientContactInfo.Email);
            txtFax.Text = UI.Utils.SetFormString(clientContactInfo.Fax);
            txtFirstName.Text = UI.Utils.SetFormString(clientContactInfo.FirstName);
            txtLastName.Text = UI.Utils.SetFormString(clientContactInfo.LastName);
            txtLocality.Text = UI.Utils.SetFormString(clientContactInfo.Locality);
            txtMobile.Text = UI.Utils.SetFormString(clientContactInfo.Mobile);
            txtPhone.Text = UI.Utils.SetFormString(clientContactInfo.Phone);
            txtPostalCode.Text = UI.Utils.SetFormString(clientContactInfo.PostalCode);
            txtStreet.Text = UI.Utils.SetFormString(clientContactInfo.Street);
            
            //txtPosition.Text 
             DpdPosition.SelectedItem.Value = UI.Utils.SetFormString(clientContactInfo.Position);
            chkInactive.Checked = clientContactInfo.IsInactive;

            txtCompanyName.Text= UI.Utils.SetFormString(clientContactInfo.CompanyName);
            //---if (contactInfo.SubContractor != null)
            //{
            //    txtCompanyId.Value = contactInfo.SubContractor.IdStr;
            //    txtCompanyName.Text = contactInfo.SubContractor.Name;
            //}


           

            Utils.GetConfigListAddEmpty("Global", "States", ddlState, clientContactInfo.State);

           // cmdSelCompany.NavigateUrl = Utils.PopupCompany(this, txtCompanyId.ClientID, txtCompanyName.ClientID, null);
        }

        private void FormToObjects()
        {
            clientContactInfo.Email = UI.Utils.GetFormString(txtEmail.Text);
            clientContactInfo.Fax = UI.Utils.GetFormString(txtFax.Text);
            clientContactInfo.FirstName = UI.Utils.GetFormString(txtFirstName.Text);
            clientContactInfo.LastName = UI.Utils.GetFormString(txtLastName.Text);
            clientContactInfo.Locality = UI.Utils.GetFormString(txtLocality.Text);
            clientContactInfo.Mobile = UI.Utils.GetFormString(txtMobile.Text);
            clientContactInfo.Phone = UI.Utils.GetFormString(txtPhone.Text);
            clientContactInfo.PostalCode = UI.Utils.GetFormString(txtPostalCode.Text);
            clientContactInfo.Street = UI.Utils.GetFormString(txtStreet.Text);
            clientContactInfo.State = UI.Utils.GetFormString(ddlState.SelectedValue);
            
            clientContactInfo.Position = UI.Utils.GetFormString(DpdPosition.SelectedValue);// UI.Utils.GetFormString(txtPosition.Text);
            clientContactInfo.Inactive = chkInactive.Checked;
            clientContactInfo.CompanyName= UI.Utils.GetFormString(txtCompanyName.Text);
            //--clientContactInfo.SubContractor = txtCompanyId.Value != "" ? new SubContractorInfo(Convert.ToInt32(txtCompanyId.Value)) : null;
        }

        private void BindTreeview()
        {

            List<ProjectImage> projectImageList;

            projectImageList = ProjectsController.GetInstance().GetProjectImages(projInfo);

          

            if (projectImageList.Count > 0)
            {
                // DirectoryInfo rootInfo = new DirectoryInfo("");
                TreeNode ProjectNode = new TreeNode();

                ProjectNode.Text = projectImageList[0].ProjectName.ToString();
                TreeView1.Nodes.Add(ProjectNode);


                foreach(ProjectImage img in projectImageList)
                {
                    TreeNode imgNode = new TreeNode();
                    imgNode.Text = img.ImageName;
                    imgNode.Value = img.IdStr; //Convert.ToBase64String(img.ImageData);
                    imgNode.SelectAction = TreeNodeSelectAction.None;
                    if (img.FolderName != null)
                    {

                        TreeNode ParentNode = FindNode(ProjectNode, img.FolderName);

                        if (ParentNode == null)
                        {
                            ParentNode = new TreeNode();
                            ParentNode.Text = img.FolderName;
                            //ParentNode.Value = img.Parent;

                            ParentNode.ChildNodes.Add(imgNode);
                            ProjectNode.ChildNodes.Add(ParentNode);
                        }
                        else
                        {

                            ParentNode.ChildNodes.Add(imgNode);

                        }

                    }

                    else
                        ProjectNode.ChildNodes.Add(imgNode);
                }
                ProjectNode.CollapseAll();
            }


        }

        private TreeNode FindNode(TreeNode ParentNode, string matchText)
        {
            foreach (TreeNode node in ParentNode.ChildNodes)
            {
                if (node.Text == matchText)
                {
                    return node;
                }

            }
            return (TreeNode)null;
        }


        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {

            Linkbutton1.Attributes.Add("onClick", "displaySync();");


            String parameterPeopleId = Request.Params["PeopleId"];
            Projectid= Request.Params["ProjectId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditClientAccess);

                if (parameterPeopleId == null)
                {
                    clientContactInfo = new ClientContactInfo();
                }
                else
                {
                    clientContactInfo = (ClientContactInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);
                    Core.Utils.CheckNullObject(clientContactInfo, parameterPeopleId, "Contact");
                }


                



                if (!Page.IsPostBack)
                {
                    ObjectsToForm();

                    projInfo = ProjectsController.GetInstance().GetProject(int.Parse(Projectid));

                    hplExistingClient.NavigateUrl = Utils.PopupPeople(this, txtClientId.ClientID, txtClientName.ClientID, PeopleInfo.PeopleTypeClientContact, projInfo.BusinessUnit);




                    SqlDataSource1.SelectCommand = @"SELECT C.[PeopleId], [FirstName]+' '+[LastName] as Name, [CompanyName], [Street]+' '+[Locality] as Address, [State], [Phone], [Email], [EmployeePosition], [UserLogin], [UserLastLogin], 
                                                     isnull(DistEOTs,'false')as DistEOTs, isnull(DistRFIs,'false')as DistRFIs, isnull(DistClaims,'false') as DistClaims, isnull(DistSaparateAccounts,'false')as DistSaparateAccounts, isnull(DistClientVariations,'false')as DistClientVariations, isnull(AttentionEOT,'false')as AttentionEOT, isnull(AttentionRFI,'false')as AttentionRFI, isnull(AttentionClaim,'false')as AttentionClaim
                                                     FROM[People] P inner join[ClientAccess] C on P.peopleId = C.peopleId
                                                     WHERE(P.[Type] = 'CL') and isnull(P.Inactive,0)<>1 and C.ProjectId =" + Projectid;
                   GVDistributionList.DataBind();



                    SqlDataSource2.SelectCommand = @"SELECT C.[PeopleId], [FirstName]+' '+[LastName] as Name, [CompanyName], [Street]+' '+[Locality] as Address, [State], [Phone], [Email], [EmployeePosition], [UserLogin], [UserLastLogin], 
                                                     isnull(WebDocs,'false')as WebDocs,isnull(WebEots,'false')as WebEots, isnull(WebRFIs,'false')as WebRFIs, isnull(WebClaims,'false')as WebClaims, isnull(WebSeparateAccounts,'false')as WebSeparateAccounts, isnull(WebClientVariations,'false')as WebClientVariations, isnull(WebPhotos,'false')as WebPhotos,UserPassword
                                                     FROM[People] P inner join[ClientAccess] C on P.peopleId = C.peopleId
                                                     WHERE(P.[Type] = 'CL') and isnull(P.Inactive,0)<>1 and (P.[UserLogin]!='' or P.[UserLogin]is not null)  and C.ProjectId =" + Projectid ;

                    GVWebsiteAccess.DataBind();
                    if (GVWebsiteAccess.Rows.Count > 0)

                        BtnWebAccessUpdate.Visible = true;

                    else BtnWebAccessUpdate.Visible = false;



                    TxtPath.Text = projInfo.AttachmentsFolder + "\\Photos\\ClientPhotos";
                    BindTreeview();




                }

               
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();

                    String mode = null;
                    if (clientContactInfo.Id == null)
                        mode = "Add";

                    clientContactInfo.Id = PeopleController.GetInstance().AddUpdatePerson(clientContactInfo);
                    if (mode=="Add" && clientContactInfo.Id!=null)
                    {
                        int sanid = clientContactInfo.Id.Value;
                        PeopleController.GetInstance().AddClientAccess(sanid, int.Parse(Projectid));
                    }

                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId="+ Projectid);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {          
                Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId="+ Projectid);
        }


        protected void cmdExisting_Click(object sender, EventArgs e)
        {
            try
            {
                if (Projectid!=null)
                {

                    String mode = null;
                    if (txtClientName.Text != string.Empty && txtClientId.Value != null)
                        PeopleController.GetInstance().AddClientAccess(int.Parse(txtClientId.Value), int.Parse(Projectid));

                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            
                Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId=" + Projectid);

        }



        protected void BtnSync_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(3000);
            try
            {
                String documentsFolder = UI.Utils.FullPath(TxtPath.Text, "");
                DirectoryInfo dirInfo = new DirectoryInfo(documentsFolder);
                ProjectImage projectImage;
                if (!dirInfo.Exists)
                {
                    lblpath.Visible = true;
                    lblpath.Text = "Folder doesn't exist...";
                   return;
                }
                if(Projectid != null)
                {

                    projInfo = ProjectsController.GetInstance().GetProject(int.Parse(Projectid));

                    foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                    {
                        FileInfo[] FileList = dir.GetFiles();

                        foreach(FileInfo imgFile in FileList)
                        {
                            string FileExt = Path.GetExtension(imgFile.FullName).ToLower();
                            if(FileExt == ".png" || FileExt==".jpg")
                                {
                                    FileStream fStream = new FileStream(imgFile.FullName, FileMode.Open, FileAccess.Read);
                                    BinaryReader bReader = new BinaryReader(fStream);
                                    byte[] bytes = bReader.ReadBytes((int)fStream.Length);

                                    projectImage = new ProjectImage();
                                    projectImage.ProjectId = int.Parse(Projectid);
                                    projectImage.ProjectName = projInfo.Name;
                                    projectImage.ParentFolder = dir.Parent.Name;
                                    projectImage.FolderName = dir.Name;
                                    projectImage.ImageName = imgFile.Name;
                                    projectImage.ImageData = bytes;

                                    ProjectsController.GetInstance().AddProjectImage(projectImage);
                                }
                         }
                      }
                    BindTreeview();
                }

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            //SyncIMG.Visible = false;
            Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId=" + Projectid);

        }





        protected void GVDistributionList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "onGridEditClick")
            {
                string Id = e.CommandArgument.ToString();
                Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId="+ Projectid+"&PeopleId="+Id);
            }

            if (e.CommandName == "ManageAccount")
            {
                string Id = e.CommandArgument.ToString().Split(';')[1].ToString();
                Button Btn = ((Button)e.CommandSource);

                        if (Btn.Text == "Create Account")//To create Account
                        {
                            Response.Redirect("~/Modules/People/CreateClientContactAccount.aspx?ProjectId=" + Projectid + "&PeopleId=" + Id);
                        }

                        else if (Btn.Text == "Delete Account")//-- delete account
                        {
                            clientContactInfo = (ClientContactInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(Id), null, null);
                            try
                            {
                                clientContactInfo.Login = null;
                                clientContactInfo.Password = null;
                                clientContactInfo.LastLoginDate = null;
                                PeopleController.GetInstance().UpdatePerson(clientContactInfo);
                            }
                            catch (Exception Ex)
                            {
                                Utils.ProcessPageLoadException(this, Ex);
                            }

                           Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId=" + Projectid);

                        }

            }


           

        }

  
        protected void GVDistributionList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btn =(Button)e.Row.FindControl("btnCreateAccount");
                String []Args=btn.CommandArgument.ToString().Split(';');

                if (Args[0].ToString() !=String.Empty)
                {
                    btn.Text = "Delete Account";
                }
                else { btn.Text = "Create Account"; }

            }
        }

        protected void GVGVWebsiteAccess_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void BtnDistUpdate_Click(object sender, EventArgs e)
        {
           //bool EOts = false, RFIs = false, Claims = false, SAs = false, CVs = false, AtEOTs=false,AtRFIs=false,ATClaims=false;
            int ClientId;
            Projectid = Request.Params["ProjectId"];
            foreach (GridViewRow gRow in GVDistributionList.Rows)
            {
                

                if (gRow.RowType == DataControlRowType.DataRow)
                {
                    ClientId = int.Parse(((Label)gRow.Cells[2].Controls[3]).Text.ToString());

                    clientContactInfo = (ClientContactInfo)PeopleController.GetInstance().GetPerson(ClientId, null, null);
                    clientContactInfo.SendEOTs = ((CheckBox)gRow.Cells[11].Controls[1]).Checked;
                    clientContactInfo.SendRFIs = ((CheckBox)gRow.Cells[12].Controls[1]).Checked;
                    clientContactInfo.SendClaims = ((CheckBox)gRow.Cells[13].Controls[1]).Checked;
                    clientContactInfo.SendSAs = ((CheckBox)gRow.Cells[14].Controls[1]).Checked;
                    clientContactInfo.SendCVs = ((CheckBox)gRow.Cells[15].Controls[1]).Checked;
                    clientContactInfo.AttentionToEots = ((CheckBox)gRow.Cells[16].Controls[1]).Checked;
                    clientContactInfo.AttentionToRFIs = ((CheckBox)gRow.Cells[17].Controls[1]).Checked;
                    clientContactInfo.AttentionToClaims = ((CheckBox)gRow.Cells[18].Controls[1]).Checked;
                    /*
                                        EOts =((CheckBox)gRow.Cells[12].Controls[1]).Checked;
                                        RFIs = ((CheckBox)gRow.Cells[13].Controls[1]).Checked;
                                        Claims = ((CheckBox)gRow.Cells[14].Controls[1]).Checked;
                                        SAs = ((CheckBox)gRow.Cells[15].Controls[1]).Checked;
                                        CVs = ((CheckBox)gRow.Cells[16].Controls[1]).Checked;
                                        AtEOTs = ((CheckBox)gRow.Cells[17].Controls[1]).Checked;
                                        AtRFIs = ((CheckBox)gRow.Cells[18].Controls[1]).Checked;
                                        ATClaims = ((CheckBox)gRow.Cells[19].Controls[1]).Checked;
                    
                                        //Update Client Access
                                        PeopleController.GetInstance().UpdateClientDistAccess(ClientId,int.Parse(Projectid), EOts, RFIs, Claims, SAs, CVs, AtEOTs, AtRFIs, ATClaims);
                    */
                    PeopleController.GetInstance().UpdateClientDistAccess(clientContactInfo,int.Parse(Projectid));


                }

            }
        }

        protected void BtnWebAccessUpdate_Click(object sender, EventArgs e)
        {
            //bool EOts = false, RFIs = false, Claims = false, SAs = false, CVs = false, Docs = false, Photos = false;
            int ClientId;
            Projectid = Request.Params["ProjectId"];
            foreach (GridViewRow gRow in GVWebsiteAccess.Rows)
            {


                if (gRow.RowType == DataControlRowType.DataRow)
                {
                    ClientId = int.Parse(((Label)gRow.Cells[1].Controls[3]).Text.ToString());

                    clientContactInfo = (ClientContactInfo)PeopleController.GetInstance().GetPerson(ClientId, null, null);
                    clientContactInfo.WebAccessToEOTs = ((CheckBox)gRow.Cells[3].Controls[1]).Checked;
                    clientContactInfo.WebAccessToRFIs = ((CheckBox)gRow.Cells[4].Controls[1]).Checked;
                    clientContactInfo.WebAccessToClaims = ((CheckBox)gRow.Cells[5].Controls[1]).Checked;
                    clientContactInfo.WebAccessToSAs = ((CheckBox)gRow.Cells[6].Controls[1]).Checked;
                    clientContactInfo.WebAccessToCVs = ((CheckBox)gRow.Cells[7].Controls[1]).Checked;
                    clientContactInfo.WebAccessToDocs = ((CheckBox)gRow.Cells[8].Controls[1]).Checked;
                    clientContactInfo.WebAccessToPhotos = ((CheckBox)gRow.Cells[9].Controls[1]).Checked;
                    /*
                    EOts = ((CheckBox)gRow.Cells[3].Controls[1]).Checked;
                    RFIs = ((CheckBox)gRow.Cells[4].Controls[1]).Checked;
                    Claims = ((CheckBox)gRow.Cells[5].Controls[1]).Checked;
                    SAs = ((CheckBox)gRow.Cells[6].Controls[1]).Checked;
                    CVs = ((CheckBox)gRow.Cells[7].Controls[1]).Checked;
                    Docs = ((CheckBox)gRow.Cells[8].Controls[1]).Checked;
                    Photos = ((CheckBox)gRow.Cells[9].Controls[1]).Checked;
                    

                    //Update Client Access
                    PeopleController.GetInstance().UpdateClientWebAccess(ClientId, int.Parse(Projectid), EOts, RFIs, Claims, SAs, CVs, Docs, Photos);
                    */

                    PeopleController.GetInstance().UpdateClientWebAccess(clientContactInfo, int.Parse(Projectid));


                }

            }
        }


        #endregion

    }
}