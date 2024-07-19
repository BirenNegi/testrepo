using System;
using System.IO;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewAddendumPage : SOSPage
    {

#region Members
        private AddendumInfo addendumInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (addendumInfo == null)
                return null;

            tempNode.ParentNode.Title = addendumInfo.Trade.Name;
            tempNode.ParentNode.Url += "?TradeId=" + addendumInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = addendumInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + addendumInfo.Trade.Project.IdStr;
            
            return currentNode;
        }

        private void BindAddendum()
        {
            lblNumber.Text = UI.Utils.SetFormInteger(addendumInfo.Number);
            lblName.Text = UI.Utils.SetFormString(addendumInfo.Name);
            lblAddendumDate.Text = UI.Utils.SetFormDate(addendumInfo.AddendumDate);
            lblDescription.Text = UI.Utils.SetFormString(addendumInfo.Description);

            sfsAttachment.Path = addendumInfo.Trade.Project.AttachmentsFolder;

            BindAttachments();
        }

        private void BindAttachments()
        {
            FileInfo fileInfo;
            DataTable dataTable = new DataTable();
            DataRow dataRow;
            String attachmentPage = "~/Modules/Projects/ShowAttachment.aspx?AttachmentId=";

            dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));

            dataTable.Columns.Add("LinkURL", System.Type.GetType("System.String"));
            dataTable.Columns.Add("LinkText", System.Type.GetType("System.String"));
            dataTable.Columns.Add("LinkCSSClass", System.Type.GetType("System.String"));

            dataTable.Columns.Add("FileSizeInfo", System.Type.GetType("System.String"));
            dataTable.Columns.Add("FileDateInfo", System.Type.GetType("System.String"));
            dataTable.Columns.Add("AttachDateInfo", System.Type.GetType("System.String"));

            
            if (addendumInfo.AttachmentsGroup.Attachments != null)
                foreach (AttachmentInfo attachmentInfo in addendumInfo.AttachmentsGroup.Attachments)
                {
                    dataRow = dataTable.NewRow();

                    dataRow["Id"] = (Int32)attachmentInfo.Id;

                    dataRow["LinkURL"] = String.Empty;
                    dataRow["LinkText"] = String.Empty;
                    dataRow["LinkCSSClass"] = String.Empty;
                    dataRow["FileSizeInfo"] = String.Empty;
                    dataRow["FileDateInfo"] = String.Empty;
                    dataRow["AttachDateInfo"] = String.Empty;

                    if (attachmentInfo.FilePath != null)
                    {
                        fileInfo = new FileInfo(UI.Utils.FullPath(addendumInfo.Trade.Project.AttachmentsFolder, attachmentInfo.FilePath));

                        if (fileInfo.Exists)
                        {
                            dataRow["LinkURL"] = attachmentPage + attachmentInfo.IdStr;
                            dataRow["LinkCSSClass"] = "frmLink";
                            dataRow["FileSizeInfo"] = UI.Utils.SetFormFileSize(fileInfo.Length);
                            dataRow["FileDateInfo"] = UI.Utils.SetFormDateTime(fileInfo.LastWriteTime);
                        }
                        else
                            dataRow["LinkCSSClass"] = "frmError";

                        dataRow["LinkText"] = attachmentInfo.FilePath;
                        dataRow["AttachDateInfo"] = UI.Utils.SetFormDateTime(attachmentInfo.AttachDate);
                    }

                    dataTable.Rows.Add(dataRow);
                }

            gvAttachments.DataSource = dataTable;
            gvAttachments.DataBind();

            sfsAttachment.FilePath = null;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewAddendum);
                String parameterAddendumId = Utils.CheckParameter("AddendumId");
                addendumInfo = tradesController.GetAddendumWithAttachments(Int32.Parse(parameterAddendumId));
                Core.Utils.CheckNullObject(addendumInfo, parameterAddendumId, "Addendum");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditAddendum))
                    {
                        if (processController.AllowEditCurrentUser(addendumInfo.Trade))
                        {
                            cmdEditTop.Visible = true;
                            cmdDeleteTop.Visible = true;

                            phAddAttachment.Visible = true;
                            gvAttachments.Columns[0].Visible = true;
                            
                            cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Addendum ?');");
                        }
                    }

                    BindAddendum();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditAddendum.aspx?AddendumId=" + addendumInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteAddendum(addendumInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + addendumInfo.Trade.IdStr);
        }

        protected void butAddAttachment_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            AttachmentInfo attachmentInfo = new AttachmentInfo();

            try
            {
                if (sfsAttachment.FilePath != null)
                {
                    attachmentInfo.AttachmentsGroup = addendumInfo.AttachmentsGroup;
                    attachmentInfo.FilePath = sfsAttachment.FilePath;
                    attachmentInfo.AttachDate = DateTime.Now;
                    attachmentInfo.Type = AttachmentInfo.File;

                    projectsController.AddAttachment(attachmentInfo);
                    projectsController.GetAttachments(addendumInfo);
                    sfsAttachment.FilePath = null;

                    BindAttachments();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvAttachments_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                int attachmentId = (int)gvAttachments.DataKeys[e.RowIndex].Value;
                projectsController.DeleteAttachment(new AttachmentInfo(attachmentId));
                projectsController.GetAttachments(addendumInfo);
                BindAttachments();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}

