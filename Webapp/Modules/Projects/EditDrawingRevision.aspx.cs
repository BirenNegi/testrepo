using System;
using System.IO;
using System.Web;
using System.Configuration;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditDrawingRevisionPage : SOSPage
    {

#region Members
        private DrawingRevisionInfo drawingRevisionInfo = null;

        private String[] foldersLevel1 = new String[] { "Design\\G_Working Drawings", "Design\\Working Drawings" };
        private String[] foldersLevel2 = new String[] { "", "PDF" };

        private Dictionary<String, String[]> foldersLevel3 = new Dictionary<String, String[]>() { 
            { "Architectural", new String[] { "Architectural", "Arch" } }, 
            { "Structural", new String[] { "Structural" } },
            { "Civil", new String[] { "Civil" } },
            { "Electrical", new String[] { "Electrical" } },
            { "Mechanical", new String[] { "Mechanical" } },
            { "Hydraulic", new String[] { "Hydraulic" } },
        };

        private String[] fileFormat1 = new String[] { "", "00", "000", "0000", "00000" };
        private String[] fileFormat2 = new String[] { "", "00", "000", "0000", "00000" };
        private String[] fileFormat3 = new String[] { "A", "B", "C", "D" };
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (drawingRevisionInfo == null)
                return null;

            tempNode.ParentNode.Title = drawingRevisionInfo.Drawing.Name;
            tempNode.ParentNode.Url += "?DrawingId=" + drawingRevisionInfo.Drawing.IdStr;

            tempNode.ParentNode.ParentNode.Title = drawingRevisionInfo.Drawing.DrawingType.Name + " Drawings";
            tempNode.ParentNode.ParentNode.Url += "?Type=" + drawingRevisionInfo.Drawing.Type + "&ProjectId=" + drawingRevisionInfo.Drawing.Project.IdStr + "&DrawingTypeId=" + drawingRevisionInfo.Drawing.DrawingType.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + drawingRevisionInfo.Drawing.Project.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Title = drawingRevisionInfo.Drawing.Project.Name + (drawingRevisionInfo.Drawing.IsProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + drawingRevisionInfo.Drawing.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            if (drawingRevisionInfo.Drawing.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            if (drawingRevisionInfo.Id == null)
            {
                TitleBar.Title = "Adding Drawing Revision";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Drawing Revision";
            }

            txtNumber.Text = UI.Utils.SetFormString(drawingRevisionInfo.Number);
            sdrDate.Date = drawingRevisionInfo.RevisionDate;
            txtComments.Text = UI.Utils.SetFormString(drawingRevisionInfo.Comments);

            sfsFile.FilePath = drawingRevisionInfo.File;
            sfsFile.Path = drawingRevisionInfo.Drawing.Project.AttachmentsFolder;
        }

        private void FormToObjects()
        {
            drawingRevisionInfo.Number = UI.Utils.GetFormString(txtNumber.Text);
            drawingRevisionInfo.RevisionDate = sdrDate.Date;
            drawingRevisionInfo.Comments = UI.Utils.GetFormString(txtComments.Text);

            drawingRevisionInfo.File = sfsFile.FilePath;
        }

        private String FindFile()
        {
            FileInfo fileInfo;
            String projectNumber;
            String drawingName;
            String revisionNumber;
            String findFile = String.Empty;
            String fileName;

            projectNumber = drawingRevisionInfo.Drawing.Project.Number;
            drawingName = drawingRevisionInfo.Drawing.Name;
            revisionNumber = drawingRevisionInfo.Number;



            // format both drawing name and revision name with 000 before 
            fileName = projectNumber + "-" + drawingName + "-" + revisionNumber;




            fileInfo = new FileInfo(fileName);
            
            return findFile;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterDrawingRevisionId;

            try
            {
                Security.CheckAccess(Security.userActions.EditDrawing);
                parameterDrawingRevisionId = Request.Params["DrawingRevisionId"];
                if (parameterDrawingRevisionId == null)
                {
                    String parameterDrawingId = Utils.CheckParameter("DrawingId");
                    drawingRevisionInfo = new DrawingRevisionInfo();
                    drawingRevisionInfo.Drawing = tradesController.GetDrawing(Int32.Parse(parameterDrawingId));
                    Core.Utils.CheckNullObject(drawingRevisionInfo.Drawing, parameterDrawingId, "Drawing");
                }
                else
                {
                    drawingRevisionInfo = tradesController.GetDrawingRevision(Int32.Parse(parameterDrawingRevisionId));
                    Core.Utils.CheckNullObject(drawingRevisionInfo, parameterDrawingRevisionId, "Drawing Revision");
                }

                drawingRevisionInfo.Drawing.Project = projectsController.GetProject(drawingRevisionInfo.Drawing.Project.Id);
                processController.CheckUpdateDrawingsCurrentUser(drawingRevisionInfo.Drawing.Project);

                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
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
                    drawingRevisionInfo.Id = TradesController.GetInstance().AddUpdateDrawingRevision(drawingRevisionInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + drawingRevisionInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (drawingRevisionInfo.Id == null)
                Response.Redirect("~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + drawingRevisionInfo.Drawing.IdStr);
            else
                Response.Redirect("~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + drawingRevisionInfo.IdStr);
        }
#endregion

    }
}
