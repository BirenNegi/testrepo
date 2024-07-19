using System;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListDrawingsTransmittalsPage : SOSPage
    {

#region Members
        private ProjectInfo projectInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.Title = projectInfo.Name;

            tempNode.Title = "Drawings/Transmittals";

            tempNode.ParentNode.Title = projectInfo.Name + (projectInfo.IsStatusProposal ? " (Proposal)" : ""); ;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        private void BindProject()
        {
            TradesController tradesController = TradesController.GetInstance();
            List<DrawingTypeInfo> drawingTypeInfoList = tradesController.GetDrawingTypes();
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();
            TreeNode treeNodeType;
            TreeNode treeNodeTypeProposal;
            TreeNode treeNodeDrawing;
            TreeNode treeNodeRevision;
            TreeNode treeNodeSubcontractor;
            TreeNode treeNodeTransmittal;
            String DeepZoomCode;

            if (projectInfo.IsStatusProposal)
            {
                cpe1.Collapsed = false;
                pnlStatusActive.Visible = false;
            }

            if (!projectInfo.IsEmptyDrawingsActvie)
            {
                cmdCopyDrawings.Enabled = false;
                cmdCopyDrawings.OnClientClick = String.Empty;
            }

            foreach (DrawingTypeInfo drawingTypeInfo in drawingTypeInfoList)
            {
                treeNodeType = new TreeNode(drawingTypeInfo.Name, drawingTypeInfo.IdStr, "", String.Format("~/Modules/Projects/ListDrawings.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", Info.TypeActive, projectInfo.IdStr, drawingTypeInfo.IdStr), "");
                treeNodeTypeProposal = new TreeNode(drawingTypeInfo.Name, drawingTypeInfo.IdStr, "", String.Format("~/Modules/Projects/ListDrawings.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", Info.TypeProposal, projectInfo.IdStr, drawingTypeInfo.IdStr), "");
                
                treeNodeType.Expanded = false;
                treeNodeTypeProposal.Expanded = false;

                drawingInfoList = tradesController.GetDrawings(projectInfo, drawingTypeInfo);
                foreach (DrawingInfo drawingInfo in drawingInfoList)
                {
                    treeNodeDrawing = new TreeNode(drawingInfo.Name + (drawingInfo.Description != null ? "-" + drawingInfo.Description : String.Empty), drawingInfo.IdStr, "", "~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + drawingInfo.IdStr, "");
                    treeNodeDrawing.Expanded = false;

                    foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
                    {
                        treeNodeRevision = new TreeNode(drawingRevisionInfo.Number + (drawingRevisionInfo.RevisionDate != null ? " - " + UI.Utils.SetFormDate(drawingRevisionInfo.RevisionDate) : String.Empty), drawingRevisionInfo.IdStr, "", "~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + drawingRevisionInfo.IdStr, "");
                        treeNodeDrawing.ChildNodes.Add(treeNodeRevision);
                    }

                    if (drawingInfo.IsActive)
                        treeNodeType.ChildNodes.Add(treeNodeDrawing);
                    else
                        treeNodeTypeProposal.ChildNodes.Add(treeNodeDrawing);
                }

                tvDrawings.Nodes[0].ChildNodes.Add(treeNodeType);
                tvDrawingsProposal.Nodes[0].ChildNodes.Add(treeNodeTypeProposal);
            }

            lnkDrawings.NavigateUrl = String.Format("~/Modules/Projects/ShowDrawings.aspx?Type={0}&ProjectId={1}", Info.TypeActive, projectInfo.IdStr);
            lnkDrawingsProposal.NavigateUrl = String.Format("~/Modules/Projects/ShowDrawings.aspx?Type={0}&ProjectId={1}", Info.TypeProposal, projectInfo.IdStr);

            lnkTransmittals.NavigateUrl = String.Format("~/Modules/Projects/ShowTransmittals.aspx?Type={0}&ProjectId={1}", Info.TypeActive, projectInfo.IdStr);
            lnkTransmittalsProposal.NavigateUrl = String.Format("~/Modules/Projects/ShowTransmittals.aspx?Type={0}&ProjectId={1}", Info.TypeProposal, projectInfo.IdStr);

            lnkAddTransmittal.NavigateUrl = String.Format("~/Modules/Projects/EditTransmittal.aspx?Type={0}&ProjectId={1}", Info.TypeActive, projectInfo.IdStr);
            lnkAddTransmittalProposal.NavigateUrl = String.Format("~/Modules/Projects/EditTransmittal.aspx?Type={0}&ProjectId={1}", Info.TypeProposal, projectInfo.IdStr);

            int? currentSubcontractorId = 0;
            treeNodeSubcontractor = null;
            if (projectInfo.Transmittals != null)
            {
                foreach (TransmittalInfo transmittalInfo in projectInfo.Transmittals)
                {
                    if (transmittalInfo.SubContractor == null)
                    {
                        transmittalInfo.Contact = new ContactInfo();
                        transmittalInfo.Contact.SubContractor = new SubContractorInfo();
                        transmittalInfo.Contact.SubContractor.Name = "No Subcontractor";
                    }

                    if (transmittalInfo.SubContractor.Id != currentSubcontractorId)
                    {
                        treeNodeSubcontractor = new TreeNode(transmittalInfo.SubContractor.Name);
                        treeNodeSubcontractor.Expanded = false;
                        treeNodeSubcontractor.SelectAction = TreeNodeSelectAction.None;

                        if (transmittalInfo.IsActive)
                            tvTransmittals.Nodes[0].ChildNodes.Add(treeNodeSubcontractor);
                        else
                            tvTransmittalsProposal.Nodes[0].ChildNodes.Add(treeNodeSubcontractor);

                        currentSubcontractorId = transmittalInfo.SubContractor.Id;
                    }

                    treeNodeTransmittal = new TreeNode(transmittalInfo.Name, transmittalInfo.IdStr, "", "~/Modules/Projects/ViewTransmittal.aspx?TransmittalId=" + transmittalInfo.IdStr, "");
                    treeNodeSubcontractor.ChildNodes.Add(treeNodeTransmittal);
                }
            }

            DeepZoomCode = ConfigurationManager.AppSettings["DeepZoomCode"].ToString();

            if (projectInfo.DeepZoomUrl != null)
            {
                pnlDeepZoom.Visible = true;

                if (DeepZoomCode == DrawingInfo.DeepZoomCodeLocal)
                {
                    actSeadragon.SourceUrl = "~/" + projectInfo.DeepZoomUrl;
                    actSeadragon.Visible = true;
                }
                else if (DeepZoomCode == DrawingInfo.DeepZoomCodeRemote)
                {
                    litScriptDeepZoom.Text = "" +
                    "<script type='text/javascript' src='http://seadragon.com/ajax/0.8/seadragon-min.js'></script>" +
                    "<script type='text/javascript'>" +
                    "var viewer = null;\r" +
                    "function initDeepZoom() {\r" +
                    "  viewer = new Seadragon.Viewer('" + divDeepZoom.ClientID + "');\r" +
                    "  viewer.openDzi('../../" + projectInfo.DeepZoomUrl + "')\r" +
                    "}\r" +
                    "Seadragon.Utils.addEvent(window, 'load', initDeepZoom);" +
                    "</script>\r";

                    divDeepZoom.Visible = true;
                }
            }
            else
            {
                pnlDeepZoom.Visible = false;
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                String parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithDrawingsAndTransmittals(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.ViewProject))
                    {
                        if (processController.AllowUpdateDrawingsCurrentUser(projectInfo))
                        {
                            pnlCopyDrawings.Visible = projectInfo.Status == ProjectInfo.StatusActive;
                            phAddTransmittal.Visible = true;
                            phAddTransmittalProposal.Visible = true;
                        }
                    }

                    BindProject();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCopyDrawings_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().CopyDrawingRegister(projectInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect(String.Format("~/Modules/Projects/ListDrawingsTransmittals.aspx?ProjectId={0}", projectInfo.IdStr));
        }
#endregion

    }
}
