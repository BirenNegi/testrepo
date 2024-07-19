using System;
using System.IO;
using System.ServiceModel;
using System.Collections.Generic;

using SOS.Core;

using Client = SOS.FileTransferService.Client;

namespace SOS.Web
{
    public partial class ShowDrawingRevisionPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            TradeParticipationInfo tradeParticipationInfo = null;
            ProjectInfo projectInfo = null;
            DrawingRevisionInfo drawingRevisionInfo;
            String parameterDrawingRevisionIds;
            String parameterProjectId;
            Byte[] fileData = null;
            String fileName = null;
            List<String> fileNames = new List<string>();
            String responseFileName = null;
            String[] drawingRevisionIds;

            try
            {
                Security.CheckAccess(Security.userActions.ViewProjectsDrawings);
                parameterDrawingRevisionIds = Utils.CheckParameter("DrawingRevisionIds");
                parameterProjectId = Request.Params["ProjectId"];
                drawingRevisionIds = parameterDrawingRevisionIds.Split(',');

                if (parameterProjectId != null)
                {
                    projectInfo = projectsController.GetProject(Convert.ToInt32(parameterProjectId));
                    projectInfo = projectsController.GetProjectWithDrawingsAndTransmittals(Int32.Parse(parameterProjectId));
                    //tradeParticipationInfo = tradesController.GetTradeParticipation(Convert.ToInt32(parameterParticipationId));
                    //tradeParticipationInfo.Trade = tradesController.GetTradeWithDrawings(tradeParticipationInfo.Trade.Id);
                    //tradeParticipationInfo.Trade.Project = projectsController.GetProjectWithDrawingsAndTrades(tradeParticipationInfo.Trade.Project.Id);
                }

                for (int i = 0; i < drawingRevisionIds.Length; i++)
                {
                    drawingRevisionInfo = tradesController.GetDrawingRevision(Int32.Parse(drawingRevisionIds[i]));
                    Core.Utils.CheckNullObject(drawingRevisionInfo, drawingRevisionIds[i], "Drawing Revision");

                    if (projectInfo != null)
                        projectsController.CheckViewCurrentUser(drawingRevisionInfo);
                    //    projectsController.CheckViewCurrentUser(drawingRevisionInfo, tradeParticipationInfo);
                    //else
                    //    projectsController.CheckViewCurrentUser(drawingRevisionInfo);

                    fileName = UI.Utils.Path(drawingRevisionInfo.Drawing.Project.AttachmentsFolder, drawingRevisionInfo.File);
                    fileNames.Add(fileName);
                }

                if (drawingRevisionIds.Length == 1)
                {
                    fileData = Client.Utils.GetFileData(fileName);
                    responseFileName = fileName;
                }
                else
                {
                    fileData = Client.Utils.GetFileData(fileNames.ToArray());
                    responseFileName = "Drawings.zip";
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendFile(fileData, responseFileName);
        }
#endregion

    }
}
