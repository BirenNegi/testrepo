using System;
using System.Data;
using System.Xml;
using System.Text;
using System.Transactions;
using System.IO;
using System.Configuration;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Data;
using SOS.Web;
using SOS.Reports;

namespace SOS.Core
{
    public sealed class TradesController : Controller
    {

#region Private Members
        private static TradesController instance;
#endregion

#region Private Methods
        private TradesController()
        {
        }
#endregion

#region Public Methods

        public static TradesController GetInstance()
        {
            if (instance == null)
                instance = new TradesController();

            return instance;
        }

#region General Methods
        /// <summary>
        /// Modifies the List's display order
        /// </summary>
        public ISortable ChangeDisplayOrder(List<ISortable> sortableList, ISortable sortable, bool moveUp)
        {
            ISortable modyObject = null;
            int? tmp;
            int currIndex = sortableList.IndexOf(sortable);
            
            if (currIndex != -1)
            {
                if (moveUp)
                {
                    if (currIndex != 0)
                        modyObject = sortableList[currIndex - 1];
                }
                else
                {
                    if (currIndex < sortableList.Count - 1)
                        modyObject = sortableList[currIndex + 1];
                }
                
                if (modyObject != null)
                {
                    tmp = modyObject.DisplayOrder;
                    modyObject.DisplayOrder = sortable.DisplayOrder;
                    sortable.DisplayOrder = tmp;
                }
            }
            
            return modyObject;
        }

        /// <summary>
        /// Change display order for Trades
        /// </summary>
        public void ChangeDisplayOrderTrade(List<TradeInfo> tradeInfoList, TradeInfo tradeInfo, bool moveUp)
        {
            List<ISortable> iSortableList = new List<ISortable>();
            foreach (TradeInfo tmpTradeInfo in tradeInfoList)
                iSortableList.Add(tmpTradeInfo);

            TradeInfo modifiedTradeInfo = (TradeInfo)ChangeDisplayOrder(iSortableList, tradeInfo, moveUp);
            if (modifiedTradeInfo != null)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        UpdateTradeDisplayOrder(tradeInfo);
                        UpdateTradeDisplayOrder(modifiedTradeInfo);

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.ToString());
                    throw new Exception("Changing Trade Display Order");
                }
            }
        }

        /// <summary>
        /// Change display order for Trade Items Categories
        /// </summary>
        public void ChangeDisplayOrderTradeItemCategory(List<TradeItemCategoryInfo> tradeItemCategoryInfoList, TradeItemCategoryInfo tradeItemCategoryInfo, bool moveUp)
        {
            List<ISortable> iSortableList = new List<ISortable>();
            foreach (TradeItemCategoryInfo tmptradeItemCategoryInfo in tradeItemCategoryInfoList)
                iSortableList.Add(tmptradeItemCategoryInfo);

            TradeItemCategoryInfo modifiedTradeItemCategoryInfo = (TradeItemCategoryInfo)ChangeDisplayOrder(iSortableList, tradeItemCategoryInfo, moveUp);
            if (modifiedTradeItemCategoryInfo != null)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        UpdateTradeItemCategory(tradeItemCategoryInfo);
                        UpdateTradeItemCategory(modifiedTradeItemCategoryInfo);

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.ToString());
                    throw new Exception("Changing Trade Item Category Display Order");
                }
            }
        }

        /// <summary>
        /// Change display order for Trade Items
        /// </summary>
        public void ChangeDisplayOrderTradeItem(List<TradeItemInfo> tradeItemInfoList, TradeItemInfo tradeItemInfo, bool moveUp)
        {
            List<ISortable> iSortableList = new List<ISortable>();
            foreach (TradeItemInfo tmptradeItemInfoList in tradeItemInfoList)
                iSortableList.Add(tmptradeItemInfoList);

            TradeItemInfo modifiedTradeItemInfo = (TradeItemInfo)ChangeDisplayOrder(iSortableList, tradeItemInfo, moveUp);
            if (modifiedTradeItemInfo != null)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        UpdateTradeItem(tradeItemInfo);
                        UpdateTradeItem(modifiedTradeItemInfo);

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.ToString());
                    throw new Exception("Changing Trade Item Display Order");
                }
            }
        }
#endregion

#region Drawing Types Methods
        /// <summary>
        /// Creates a Drawing Type from a dr
        /// </summary>
        public DrawingTypeInfo CreateDrawingType(IDataReader dr)
        {
            DrawingTypeInfo drawingTypeInfo = new DrawingTypeInfo(Data.Utils.GetDBInt32(dr["DrawingTypeId"]));
            drawingTypeInfo.Name = Data.Utils.GetDBString(dr["Name"]);

            AssignAuditInfo(drawingTypeInfo, dr);

            return drawingTypeInfo;
        }

        /// <summary>
        /// Get a Drawing Type from persistent storage
        /// </summary>
        public DrawingTypeInfo GetDrawingType(int? drawingTypeId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingType(drawingTypeId);
                if (dr.Read())
                    return CreateDrawingType(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Drawing Type from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get all the Drawing Types from persistent storage sorted by name
        /// </summary>
        public List<DrawingTypeInfo> GetDrawingTypes()
        {
            IDataReader dr = null;
            List<DrawingTypeInfo> drawingTypeInfoList = new List<DrawingTypeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingTypes();
                while (dr.Read())
                    drawingTypeInfoList.Add(CreateDrawingType(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Drawing Types from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingTypeInfoList;
        }

        /// <summary>
        /// Get a trade's Drawing Types from persistent storage
        /// </summary>
        public List<DrawingTypeInfo> GetDrawingTypes(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            List<DrawingTypeInfo> drawingTypeInfoList = new List<DrawingTypeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingTypes(tradeInfo.Id);
                while (dr.Read())
                    drawingTypeInfoList.Add(CreateDrawingType(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade's Drawing Types from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingTypeInfoList;
        }

        /// <summary>
        /// Updates a Drawing Type in the database
        /// </summary>
        public void UpdateDrawingType(DrawingTypeInfo drawingTypeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(drawingTypeInfo);

            parameters.Add(drawingTypeInfo.Id);
            parameters.Add(drawingTypeInfo.Name);

            parameters.Add(drawingTypeInfo.ModifiedDate);
            parameters.Add(drawingTypeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateDrawingType(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Drawing Type in database");
            }
        }

        /// <summary>
        /// Adds a Drawing Type to the database
        /// </summary>
        public int? AddDrawingType(DrawingTypeInfo drawingTypeInfo)
        {
            int? drawingTypeId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(drawingTypeInfo);

            parameters.Add(drawingTypeInfo.Name);

            parameters.Add(drawingTypeInfo.CreatedDate);
            parameters.Add(drawingTypeInfo.CreatedBy);

            try
            {
                drawingTypeId = Data.DataProvider.GetInstance().AddDrawingType(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Drawing Type to database");
            }

            return drawingTypeId;
        }

        /// <summary>
        /// Adds or updates a DrawingType
        /// </summary>
        public int? AddUpdateDrawingType(DrawingTypeInfo drawingTypeInfo)
        {
            if (drawingTypeInfo != null)
            {
                if (drawingTypeInfo.Id != null)
                {
                    UpdateDrawingType(drawingTypeInfo);
                    return drawingTypeInfo.Id;
                }
                else
                {
                    return AddDrawingType(drawingTypeInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Drawing Type from persistent storage
        /// </summary>
        public void DeleteDrawingType(DrawingTypeInfo drawingTypeInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteDrawingType(drawingTypeInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Drawing Type from database");
            }
        }
#endregion

#region Client Trade Types Methods
        /// <summary>
        /// Creates a Client Trade Type from a dr
        /// </summary>
        public ClientTradeTypeInfo CreateClientTradeType(IDataReader dr)
        {
            ClientTradeTypeInfo clientTradeTypeInfo = new ClientTradeTypeInfo(Data.Utils.GetDBInt32(dr["ClientTradeTypeId"]));
            clientTradeTypeInfo.Name = Data.Utils.GetDBString(dr["Name"]);

            AssignAuditInfo(clientTradeTypeInfo, dr);

            return clientTradeTypeInfo;
        }

        /// <summary>
        /// Get a Client Trade Type from persistent storage
        /// </summary>
        public ClientTradeTypeInfo GetClientTradeType(int? clientTradeTypeId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientTradeType(clientTradeTypeId);
                if (dr.Read())
                    return CreateClientTradeType(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Trade Type from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get all the Client Trade Types from persistent storage sorted by name
        /// </summary>
        public List<ClientTradeTypeInfo> GetClientTradeTypes()
        {
            IDataReader dr = null;
            List<ClientTradeTypeInfo> clientTradeTypeInfoList = new List<ClientTradeTypeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientTradeTypes();
                while (dr.Read())
                    clientTradeTypeInfoList.Add(CreateClientTradeType(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Trade Types from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return clientTradeTypeInfoList;
        }

        /// <summary>
        /// Updates a Client Trade Type in the database
        /// </summary>
        public void UpdateClientTradeType(ClientTradeTypeInfo clientTradeTypeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(clientTradeTypeInfo);

            parameters.Add(clientTradeTypeInfo.Id);
            parameters.Add(clientTradeTypeInfo.Name);

            parameters.Add(clientTradeTypeInfo.ModifiedDate);
            parameters.Add(clientTradeTypeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateClientTradeType(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Trade Type in database");
            }
        }

        /// <summary>
        /// Adds a Client Trade Type to the database
        /// </summary>
        public int? AddClientTradeType(ClientTradeTypeInfo clientTradeTypeInfo)
        {
            int? clientTradeTypeId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(clientTradeTypeInfo);

            parameters.Add(clientTradeTypeInfo.Name);

            parameters.Add(clientTradeTypeInfo.CreatedDate);
            parameters.Add(clientTradeTypeInfo.CreatedBy);

            try
            {
                clientTradeTypeId = Data.DataProvider.GetInstance().AddClientTradeType(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Client Trade Type to database");
            }

            return clientTradeTypeId;
        }

        /// <summary>
        /// Adds or updates a ClientTradeType
        /// </summary>
        public int? AddUpdateClientTradeType(ClientTradeTypeInfo clientTradeTypeInfo)
        {
            if (clientTradeTypeInfo != null)
            {
                if (clientTradeTypeInfo.Id != null)
                {
                    UpdateClientTradeType(clientTradeTypeInfo);
                    return clientTradeTypeInfo.Id;
                }
                else
                {
                    return AddClientTradeType(clientTradeTypeInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Client Trade Type from persistent storage
        /// </summary>
        public void DeleteClientTradeType(ClientTradeTypeInfo clientTradeTypeInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteClientTradeType(clientTradeTypeInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Client Trade Type from database");
            }
        }
#endregion       

#region Drawing Revisions Methods
        /// <summary>
        /// Creates a DrawingRevision from a dr
        /// </summary>
        public DrawingRevisionInfo CreateDrawingRevision(IDataReader dr)
        {
            DrawingRevisionInfo drawingRevisionInfo = new DrawingRevisionInfo(Data.Utils.GetDBInt32(dr["DrawingRevisionId"]));
            drawingRevisionInfo.Number = Data.Utils.GetDBString(dr["Number"]);
            drawingRevisionInfo.RevisionDate = Data.Utils.GetDBDateTime(dr["RevisionDate"]);
            drawingRevisionInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);
            drawingRevisionInfo.File = Data.Utils.GetDBString(dr["FilePath"]);

            AssignAuditInfo(drawingRevisionInfo, dr);

            if (dr["DrawingId"] != DBNull.Value)
            {
                drawingRevisionInfo.Drawing = new DrawingInfo(Data.Utils.GetDBInt32(dr["DrawingId"]));
                drawingRevisionInfo.Drawing.Type = Data.Utils.GetDBString(dr["DrawingType"]);
                drawingRevisionInfo.Drawing.Name = Data.Utils.GetDBString(dr["DrawingName"]);

                if (dr["DrawingTypeId"] != DBNull.Value)
                {
                    drawingRevisionInfo.Drawing.DrawingType = new DrawingTypeInfo(Data.Utils.GetDBInt32(dr["DrawingTypeId"]));
                    drawingRevisionInfo.Drawing.DrawingType.Name = Data.Utils.GetDBString(dr["DrawingTypeName"]);
                }

                if (dr["ProjectId"] != DBNull.Value)
                {
                    drawingRevisionInfo.Drawing.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    drawingRevisionInfo.Drawing.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    drawingRevisionInfo.Drawing.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
                }
            }

            return drawingRevisionInfo;
        }

        /// <summary>
        /// Get a Drawing Revision from persistent storage
        /// </summary>
        public DrawingRevisionInfo GetDrawingRevision(int? drawingRevisionId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingRevision(drawingRevisionId);
                if (dr.Read())
                    return CreateDrawingRevision(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Drawing Revision from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Returns a list of drawing revisions for a drawing
        /// </summary>
        public List<DrawingRevisionInfo> GetDrawingRevisions(DrawingInfo drawingInfo)
        {
            IDataReader dr = null;
            List<DrawingRevisionInfo> drawingRevisionList = new List<DrawingRevisionInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingRevisionsByDrawing(drawingInfo.Id);
                while (dr.Read())
                    drawingRevisionList.Add(CreateDrawingRevision(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Drawing Revisions from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingRevisionList;
        }

        /// <summary>
        /// Returns a list of drawings with revisions for a project
        /// </summary>
        public List<DrawingInfo> GetDrawingRevisions(ProjectInfo projectInfo)
        {
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();
            DrawingInfo drawingInfo = new DrawingInfo();
            DrawingRevisionInfo drawingRevisionInfo;
            Int32 currentDrawingId = 0;
            Int32 drawingId = 0;
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingRevisionsByProject(projectInfo.Id);
                while (dr.Read())
                {
                    drawingId = (Int32)Data.Utils.GetDBInt32(dr["DrawingId"]);

                    if (drawingId != currentDrawingId)
                    {
                        drawingInfo = new DrawingInfo(drawingId);
                        drawingInfo.Type = Data.Utils.GetDBString(dr["DrawingType"]);
                        drawingInfo.Project = projectInfo;
                        drawingInfo.DrawingRevisions = new List<DrawingRevisionInfo>();
                        drawingInfo.Name = Data.Utils.GetDBString(dr["DrawingName"]);
                        drawingInfo.Description = Data.Utils.GetDBString(dr["DrawingDescription"]);

                        if (dr["DrawingTypeId"] != DBNull.Value)
                            drawingInfo.DrawingType = new DrawingTypeInfo(Data.Utils.GetDBInt32(dr["DrawingTypeId"]));

                        drawingInfoList.Add(drawingInfo);

                        currentDrawingId = drawingId;
                    }

                    if (dr["DrawingRevisionId"] != DBNull.Value)
                    {
                        drawingRevisionInfo = new DrawingRevisionInfo(Data.Utils.GetDBInt32(dr["DrawingRevisionId"]));
                        drawingRevisionInfo.Number = Data.Utils.GetDBString(dr["Number"]);
                        drawingRevisionInfo.RevisionDate = Data.Utils.GetDBDateTime(dr["RevisionDate"]);
                        drawingRevisionInfo.Drawing = drawingInfo;

                        drawingInfo.DrawingRevisions.Add(drawingRevisionInfo);
                    }
                }

                drawingInfoList.Sort(delegate(DrawingInfo drawingInfo1, DrawingInfo drawingInfo2) { return drawingInfo1.Name.CompareTo(drawingInfo2.Name); });
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Drawing Revisions from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingInfoList;
        }

        /// <summary>
        /// Updates a Drawing Revision in the database
        /// </summary>
        public void UpdateDrawingRevision(DrawingRevisionInfo drawingRevisionInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(drawingRevisionInfo);

            parameters.Add(drawingRevisionInfo.Id);
            parameters.Add(drawingRevisionInfo.Number);
            parameters.Add(drawingRevisionInfo.RevisionDate);
            parameters.Add(drawingRevisionInfo.Comments);
            parameters.Add(drawingRevisionInfo.File);

            parameters.Add(drawingRevisionInfo.ModifiedDate);
            parameters.Add(drawingRevisionInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateDrawingRevision(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Drawing Revision in database");
            }
        }

        /// <summary>
        /// Adds a Drawing Revision to the database
        /// </summary>
        public int? AddDrawingRevision(DrawingRevisionInfo drawingRevisionInfo)
        {
            int? drawingRevisionId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(drawingRevisionInfo);

            parameters.Add(GetId(drawingRevisionInfo.Drawing));
            parameters.Add(drawingRevisionInfo.Number);
            parameters.Add(drawingRevisionInfo.RevisionDate);
            parameters.Add(drawingRevisionInfo.Comments);
            parameters.Add(drawingRevisionInfo.File);

            parameters.Add(drawingRevisionInfo.CreatedDate);
            parameters.Add(drawingRevisionInfo.CreatedBy);

            try
            {
                drawingRevisionId = Data.DataProvider.GetInstance().AddDrawingRevision(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Drawing Revision to database");
            }

            return drawingRevisionId;
        }
        
        /// <summary>
        /// Adds or updates a Drawing Revision
        /// </summary>
        public int? AddUpdateDrawingRevision(DrawingRevisionInfo drawingRevisionInfo)
        {
            if (drawingRevisionInfo != null)
            {
                if (drawingRevisionInfo.Id != null)
                {
                    UpdateDrawingRevision(drawingRevisionInfo);
                    return drawingRevisionInfo.Id;
                }
                else
                {
                    return AddDrawingRevision(drawingRevisionInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Drawing Revision from persistent storage
        /// </summary>
        public void DeleteDrawingRevision(DrawingRevisionInfo drawingRevisionInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteDrawingRevision(drawingRevisionInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Drawing Revision from database. The revision might be in use.");
            }
        }
#endregion

#region Drawings Methods
        /// <summary>
        /// Creates a Drawing from a dr
        /// </summary>
        public DrawingInfo CreateDrawing(IDataReader dr)
        {
            DrawingInfo drawingInfo = new DrawingInfo(Data.Utils.GetDBInt32(dr["DrawingId"]));
            drawingInfo.Type = Data.Utils.GetDBString(dr["DrawingType"]);
            drawingInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            drawingInfo.Description = Data.Utils.GetDBString(dr["Description"]);

            AssignAuditInfo(drawingInfo, dr);

            if (dr["ProjectId"] != DBNull.Value)
            {
                drawingInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                drawingInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
            }

            if (dr["DrawingTypeId"] != DBNull.Value)
            {
                drawingInfo.DrawingType = new DrawingTypeInfo(Data.Utils.GetDBInt32(dr["DrawingTypeId"]));
                drawingInfo.DrawingType.Name = Data.Utils.GetDBString(dr["DrawingTypeName"]); ;
            }

            return drawingInfo;
        }

        /// <summary>
        /// Get a Drawing from persistent storage
        /// </summary>
        public DrawingInfo GetDrawing(int? drawingId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawing(drawingId);
                if (dr.Read())
                    return CreateDrawing(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Drawing from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Transmittals for the specified Drawing
        /// </summary>
        public List<TransmittalInfo> GetDrawingTransmittals(DrawingInfo drawingInfo)
        {
            IDataReader dr = null;
            TransmittalInfo transmittalInfo;
            List<TransmittalInfo> transmittalInfoList = new List<TransmittalInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTransmittalsByDrawing(drawingInfo.Id);
                while (dr.Read())
                {
                    transmittalInfo = new TransmittalInfo();
                    transmittalInfo.Contact = new ContactInfo();
                    transmittalInfo.Contact.SubContractor = new SubContractorInfo();
                    transmittalInfo.TransmittalRevisions = new List<TransmittalRevisionInfo>();
                    transmittalInfo.TransmittalRevisions.Add(new TransmittalRevisionInfo());
                    transmittalInfo.TransmittalRevisions[0].Revision = new DrawingRevisionInfo();

                    transmittalInfo.Id = Data.Utils.GetDBInt32(dr["TransmittalId"]);
                    transmittalInfo.TransmittalNumber = Data.Utils.GetDBInt32(dr["TransmittalNumber"]);
                    transmittalInfo.TransmissionDate = Data.Utils.GetDBDateTime(dr["TransmissionDate"]);
                    transmittalInfo.TransmittalType = Data.Utils.GetDBString(dr["TransmittalType"]);
                    transmittalInfo.TransmittalTypeOther = Data.Utils.GetDBString(dr["TransmittalTypeOther"]);
                    transmittalInfo.SentDate = Data.Utils.GetDBDateTime(dr["SentDate"]);

                    transmittalInfo.Contact.FirstName = Data.Utils.GetDBString(dr["PeopleFirstName"]);
                    transmittalInfo.Contact.LastName = Data.Utils.GetDBString(dr["PeopleLastName"]);

                    transmittalInfo.Contact.SubContractor.Name = Data.Utils.GetDBString(dr["SubContractorName"]);
                    transmittalInfo.Contact.SubContractor.ShortName = Data.Utils.GetDBString(dr["SubContractorsShortName"]);

                    transmittalInfo.TransmittalRevisions[0].Revision.Number = Data.Utils.GetDBString(dr["DrawingRevisionNumber"]);

                    transmittalInfoList.Add(transmittalInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Drawing Transmittals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return transmittalInfoList;
        }

        /// <summary>
        /// Get a Drawing with revisions from persistent storage.
        /// </summary>
        public DrawingInfo GetDeepDrawing(int? drawingId)
        {
            DrawingInfo drawingInfo = GetDrawing(drawingId);

            drawingInfo.DrawingRevisions = GetDrawingRevisions(drawingInfo);

            return drawingInfo;
        }

        /// <summary>
        /// Get a Drawing with revisions and transmittals from persistent storage.
        /// </summary>
        public DrawingInfo GetDeepDrawingWithTransmittalsAndTrades(int? drawingId)
        {
            DrawingInfo drawingInfo = GetDeepDrawing(drawingId);
            drawingInfo.Transmittals = GetDrawingTransmittals(drawingInfo);
            drawingInfo.Trades = GetTrades(drawingInfo);
            return drawingInfo;
        }

        /// <summary>
        /// Get a Drawing with revisions from persistent storage.
        /// </summary>
        public DrawingInfo GetDeepDrawing(IDataReader dr)
        {
            DrawingInfo drawingInfo = CreateDrawing(dr);

            drawingInfo.DrawingRevisions = GetDrawingRevisions(drawingInfo);

            return drawingInfo;
        }

        /// <summary>
        /// Get the Drawings for the specified Trade
        /// </summary>
        public List<DrawingInfo> GetDrawings(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingsByTrade(tradeInfo.Id);
                while (dr.Read())
                    drawingInfoList.Add(CreateDrawing(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Drawings from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingInfoList;
        }

        /// <summary>
        /// Get the Drawings for the specified Project
        /// </summary>
        public List<DrawingInfo> GetDrawings(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();
            List<Object> parameters = new List<Object>();

            parameters.Add(null);
            parameters.Add(projectInfo.Id);

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingsByProject(parameters.ToArray());
                while (dr.Read())
                    drawingInfoList.Add(CreateDrawing(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Drawings from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingInfoList;
        }
        
        /// <summary>
        /// Returns the trade drawings for the specified drawing type
        /// </summary>
        public List<DrawingInfo> GetDrawings(TradeInfo tradeInfo, DrawingTypeInfo drawingTypeInfo)
        {
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();

            if (tradeInfo.Drawings != null)
                foreach (DrawingInfo drawingInfo in tradeInfo.Drawings)
                    if (drawingInfo.DrawingType.Equals(drawingTypeInfo))
                        drawingInfoList.Add(drawingInfo);

            return drawingInfoList;
        }

        /// <summary>
        /// Get the Drawings with revisions for the specified Trade
        /// </summary>
        public List<DrawingInfo> GetDeepDrawings(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingsByTrade(tradeInfo.Id);
                while (dr.Read())
                    drawingInfoList.Add(GetDeepDrawing(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Drawings from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingInfoList;
        }

        /// <summary>
        /// Returns the project drawings for the specified drawing type
        /// </summary>
        public List<DrawingInfo> GetDrawings(ProjectInfo projectInfo, DrawingTypeInfo drawingTypeInfo)
        {
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();

            if (projectInfo.Drawings != null) 
                foreach (DrawingInfo drawingInfo in projectInfo.Drawings)
                    if (drawingInfo.DrawingType.Equals(drawingTypeInfo))
                        drawingInfoList.Add(drawingInfo);

            return drawingInfoList;
        }

        /// <summary>
        /// Returns the project drawings for the specified drawing type and type of drawing
        /// </summary>
        public List<DrawingInfo> GetDrawings(ProjectInfo projectInfo, DrawingTypeInfo drawingTypeInfo, String drawingType)
        {
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();

            if (projectInfo.Drawings != null)
                foreach (DrawingInfo drawingInfo in projectInfo.Drawings)
                    if (drawingInfo.DrawingType.Equals(drawingTypeInfo) && drawingInfo.Type == drawingType)
                        drawingInfoList.Add(drawingInfo);

            return drawingInfoList;
        }

        /// <summary>
        /// Get the Drawings with revisions for the specified Project
        /// </summary>
        public List<DrawingInfo> GetDeepDrawings(ProjectInfo projectInfo, String type)
        {
            IDataReader dr = null;
            List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();
            List<Object> parameters = new List<Object>();

            parameters.Add(type);
            parameters.Add(projectInfo.Id);

            try
            {
                dr = Data.DataProvider.GetInstance().GetDrawingsByProject(parameters.ToArray());
                while (dr.Read())
                    drawingInfoList.Add(GetDeepDrawing(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Drawings from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return drawingInfoList;
        }

        /// <summary>
        /// Get the Drawings with revisions for the specified Project
        /// </summary>
        public List<DrawingInfo> GetDeepDrawings(ProjectInfo projectInfo)
        {
            return GetDeepDrawings(projectInfo, null);
        }

        /// <summary>
        /// Get the Active Drawings with revisions for the specified Project
        /// </summary>
        public List<DrawingInfo> GetDeepDrawingsActive(ProjectInfo projectInfo)
        {
            return GetDeepDrawings(projectInfo, Info.TypeActive);
        }

        /// <summary>
        /// Get the Proposal Drawings with revisions for the specified Project
        /// </summary>
        public List<DrawingInfo> GetDeepDrawingsProposal(ProjectInfo projectInfo)
        {
            return GetDeepDrawings(projectInfo, Info.TypeProposal);
        }

        /// <summary>
        /// Updates a Drawing in the database
        /// </summary>
        public void UpdateDrawing(DrawingInfo drawingInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(drawingInfo);

            parameters.Add(drawingInfo.Id);
            parameters.Add(GetId(drawingInfo.DrawingType));
            parameters.Add(drawingInfo.Name);
            parameters.Add(drawingInfo.Description);

            parameters.Add(drawingInfo.ModifiedDate);
            parameters.Add(drawingInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateDrawing(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Drawing in database");
            }
        }

        /// <summary>
        /// Adds a Drawing to the database
        /// </summary>
        public int? AddDrawing(DrawingInfo drawingInfo)
        {
            int? drawingId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(drawingInfo);

            parameters.Add(drawingInfo.Type);
            parameters.Add(GetId(drawingInfo.DrawingType));
            parameters.Add(GetId(drawingInfo.Project));
            parameters.Add(drawingInfo.Name);
            parameters.Add(drawingInfo.Description);

            parameters.Add(drawingInfo.CreatedDate);
            parameters.Add(drawingInfo.CreatedBy);

            try
            {
                drawingId = Data.DataProvider.GetInstance().AddDrawing(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Drawing to database");
            }

            return drawingId;
        }

        /// <summary>
        /// Adds or updates a Drawing
        /// </summary>
        public int? AddUpdateDrawing(DrawingInfo drawingInfo)
        {
            if (drawingInfo != null)
            {
                if (drawingInfo.Id != null)
                {
                    UpdateDrawing(drawingInfo);
                    return drawingInfo.Id;
                }
                else
                {
                    return AddDrawing(drawingInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Drawing from persistent storage
        /// </summary>
        public void DeleteDrawing(DrawingInfo drawingInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteDrawing(drawingInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Drawing from database");
            }
        }
#endregion

#region Participation Items Methods
        /// <summary>
        /// Creates a Participation Item from a dr
        /// </summary>
        public ParticipationItemInfo CreateParticipationItem(IDataReader dr)
        {
            ParticipationItemInfo participationItemInfo = new ParticipationItemInfo(Data.Utils.GetDBInt32(dr["ParticipationItemId"]));

            participationItemInfo.IsIncluded = Data.Utils.GetDBBoolean(dr["IsIncluded"]);
            participationItemInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            participationItemInfo.Confirmed = Data.Utils.GetDBBoolean(dr["Confirmed"]);
            participationItemInfo.Quantity = Data.Utils.GetDBString(dr["Quantity"]);
            participationItemInfo.Notes = Data.Utils.GetDBString(dr["Notes"]);

            AssignAuditInfo(participationItemInfo, dr);

            if (dr["TradeParticipationId"] != DBNull.Value)
            {
                participationItemInfo.TradeParticipation = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["TradeParticipationId"]));
                participationItemInfo.TradeParticipation.Type = Data.Utils.GetDBString(dr["TradeParticipationType"]);

                if (dr["SubContractorId"] != DBNull.Value)
                {
                    participationItemInfo.TradeParticipation.SubContractor = new SubContractorInfo(Data.Utils.GetDBInt32(dr["SubContractorId"]));
                    participationItemInfo.TradeParticipation.SubContractor.Name = Data.Utils.GetDBString(dr["SubContractorName"]);
                }

                if (dr["ComparisonTradeParticipationId"] != DBNull.Value)
                    participationItemInfo.TradeParticipation.ComparisonParticipation = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["ComparisonTradeParticipationId"]));
            }

            if (dr["TradeItemId"] != DBNull.Value)
            {
                participationItemInfo.TradeItem = new TradeItemInfo(Data.Utils.GetDBInt32(dr["TradeItemId"]));
                participationItemInfo.TradeItem.Name = Data.Utils.GetDBString(dr["TradeItemName"]);
                participationItemInfo.TradeItem.RequiredInProposal = Data.Utils.GetDBBoolean(dr["RequiredInProposal"]);

                if (dr["TradeItemCategoryId"] != DBNull.Value)
                {
                    participationItemInfo.TradeItem.TradeItemCategory = new TradeItemCategoryInfo(Data.Utils.GetDBInt32(dr["TradeItemCategoryId"]));
                    participationItemInfo.TradeItem.TradeItemCategory.Name = Data.Utils.GetDBString(dr["TradeItemCategoryName"]);

                    if (dr["TradeId"] != DBNull.Value)
                    {
                        participationItemInfo.TradeItem.TradeItemCategory.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                        participationItemInfo.TradeItem.TradeItemCategory.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                        if (dr["ProjectId"] != DBNull.Value)
                        {
                            participationItemInfo.TradeItem.TradeItemCategory.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                            participationItemInfo.TradeItem.TradeItemCategory.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                        }
                    }
                }
            }

            return participationItemInfo;
        }

        /// <summary>
        /// Get a Participation Item from persistent storage
        /// </summary>
        public ParticipationItemInfo GetParticipationItem(int? participationItemId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetParticipationItem(participationItemId);
                if (dr.Read())
                    return CreateParticipationItem(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Participation Item from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Return the Participation Item for the specified Trade Participation and Trade Item
        /// </summary>
        public ParticipationItemInfo GetParticipationItem(TradeParticipationInfo tradeParticipationInfo, TradeItemInfo tradeItemInfo)
        {
            return tradeParticipationInfo.ParticipationItems.Find(delegate(ParticipationItemInfo participationItemInfoInList) { return participationItemInfoInList.TradeItem.Equals(tradeItemInfo); });
        }

        /// <summary>
        /// Get Participation Items for the specified Trade Item
        /// </summary>
        public List<ParticipationItemInfo> GetParticipationItems(TradeItemInfo tradeItemInfo)
        {
            IDataReader dr = null;
            List<ParticipationItemInfo> participationItemInfoList = new List<ParticipationItemInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetParticipationItems(tradeItemInfo.Id);
                while (dr.Read())
                    participationItemInfoList.Add(CreateParticipationItem(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Participation Items for Trade Item from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return participationItemInfoList;
        }

        /// <summary>
        /// Get Participation Items for the specified Trade Participation
        /// </summary>
        public List<ParticipationItemInfo> GetParticipationItems(TradeParticipationInfo tradeParticipationInfo)
        {
            IDataReader dr = null;
            List<ParticipationItemInfo> participationItemInfoList = new List<ParticipationItemInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetParticipationItemsByTradeParticipation(tradeParticipationInfo.Id);
                while (dr.Read())
                    participationItemInfoList.Add(CreateParticipationItem(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Participation Items for Trade Participation from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return participationItemInfoList;
        }

        /// <summary>
        /// Get Participation Items for the specified Trade Item Category
        /// </summary>
        public List<ParticipationItemInfo> GetParticipationItems(TradeItemCategoryInfo tradeItemCategoryInfo)
        {
            IDataReader dr = null;
            List<ParticipationItemInfo> participationItemInfoList = new List<ParticipationItemInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetParticipationItemsByTradeItemCategory(tradeItemCategoryInfo.Id);
                while (dr.Read())
                    participationItemInfoList.Add(CreateParticipationItem(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Participation Items for Trade Item Category from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return participationItemInfoList;
        }

        /// <summary>
        /// Updates a Participation Item in the database
        /// </summary>
        public void UpdateParticipationItem(ParticipationItemInfo participationItemInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(participationItemInfo);

            parameters.Add(participationItemInfo.Id);
            parameters.Add(participationItemInfo.IsIncluded);
            parameters.Add(participationItemInfo.Amount);
            parameters.Add(participationItemInfo.Confirmed);
            parameters.Add(participationItemInfo.Quantity);
            parameters.Add(participationItemInfo.Notes);

            parameters.Add(participationItemInfo.ModifiedDate);
            parameters.Add(participationItemInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateParticipationItem(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Participation Item in database");
            }
        }

        /// <summary>
        /// Updates a Participation Item and the trade process in the database
        /// </summary>
        public void UpdateParticipationItemAndProcess(ParticipationItemInfo participationItemInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    UpdateParticipationItem(participationItemInfo);
                    UpdateTradeProcess(participationItemInfo.TradeParticipation.Trade);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Participation Item and process in database");
            }
        }

        /// <summary>
        /// Adds a ParticipationItem to the database
        /// </summary>
        public int? AddParticipationItem(ParticipationItemInfo participationItemInfo)
        {
            int? participationItemId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(participationItemInfo);

            parameters.Add(GetId(participationItemInfo.TradeParticipation));
            parameters.Add(GetId(participationItemInfo.TradeItem));
            parameters.Add(participationItemInfo.IsIncluded);
            parameters.Add(participationItemInfo.Amount);
            parameters.Add(participationItemInfo.Confirmed);
            parameters.Add(participationItemInfo.Quantity);
            parameters.Add(participationItemInfo.Notes);

            parameters.Add(participationItemInfo.CreatedDate);
            parameters.Add(participationItemInfo.CreatedBy);

            try
            {
                participationItemId = Data.DataProvider.GetInstance().AddParticipationItem(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Participation Item to database");
            }

            return participationItemId;
        }

        /// <summary>
        /// Adds or updates a ParticipationItem
        /// </summary>
        public int? AddUpdateParticipationItem(ParticipationItemInfo participationItemInfo)
        {
            if (participationItemInfo != null)
            {
                if (participationItemInfo.Id != null)
                {
                    UpdateParticipationItem(participationItemInfo);
                    return participationItemInfo.Id;
                }
                else
                {
                    return AddParticipationItem(participationItemInfo);
                }
            }
            else
            {
                return null;
            }
        }
#endregion

#region Trade Items Methods
        /// <summary>
        /// Create a Trade Item from a dr
        /// </summary>
        public TradeItemInfo CreateTradeItem(IDataReader dr)
        {
            TradeItemInfo tradeItemInfo = new TradeItemInfo(Data.Utils.GetDBInt32(dr["TradeItemId"]));

            tradeItemInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            tradeItemInfo.Units = Data.Utils.GetDBString(dr["Units"]);
            tradeItemInfo.Scope = Data.Utils.GetDBString(dr["ScopeOfWorks"]);
            tradeItemInfo.DisplayOrder = Data.Utils.GetDBInt32(dr["DisplayOrder"]);
            tradeItemInfo.RequiresQuantityCheck = Data.Utils.GetDBBoolean(dr["RequiresQuantityCheck"]);
            tradeItemInfo.RequiredInProposal = Data.Utils.GetDBBoolean(dr["RequiredInProposal"]);

            AssignAuditInfo(tradeItemInfo, dr);

            if (dr["TradeItemCategoryId"] != DBNull.Value)
            {
                tradeItemInfo.TradeItemCategory = new TradeItemCategoryInfo(Data.Utils.GetDBInt32(dr["TradeItemCategoryId"]));
                tradeItemInfo.TradeItemCategory.Name = Data.Utils.GetDBString(dr["TradeItemCategoryName"]);

                if (dr["TradeId"] != DBNull.Value)
                {
                    tradeItemInfo.TradeItemCategory.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeItemInfo.TradeItemCategory.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                    if (dr["TradeTemplateId"] != DBNull.Value) tradeItemInfo.TradeItemCategory.Trade.TradeTemplate = new TradeTemplateInfo(Data.Utils.GetDBInt32(dr["TradeTemplateId"]));

                    if (dr["ProjectId"] != DBNull.Value)
                    {
                        tradeItemInfo.TradeItemCategory.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                        tradeItemInfo.TradeItemCategory.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    }
                }
            }
                
            return tradeItemInfo;
        }
        
        /// <summary>
        /// Get a Trade Item from persistent storage
        /// </summary>
        public TradeItemInfo GetTradeItem(int? tradeItemId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeItem(tradeItemId);
                if (dr.Read())
                    return CreateTradeItem(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Item from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Trade Items for the specified Trade Item Category
        /// </summary>
        public List<TradeItemInfo> GetTradeItems(TradeItemCategoryInfo tradeItemCategoryInfo)
        {
            IDataReader dr = null;
            List<TradeItemInfo> tradeItemInfoList = new List<TradeItemInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeItems(tradeItemCategoryInfo.Id);
                while (dr.Read())
                    tradeItemInfoList.Add(CreateTradeItem(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Items for Trade Item Category from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeItemInfoList;
        }

        /// <summary>
        /// Updates a Trade Item in the database
        /// </summary>
        public void UpdateTradeItem(TradeItemInfo tradeItemInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeItemInfo);

            parameters.Add(tradeItemInfo.Id);
            parameters.Add(tradeItemInfo.Name);
            parameters.Add(tradeItemInfo.Units);
            parameters.Add(tradeItemInfo.Scope);
            parameters.Add(tradeItemInfo.DisplayOrder);
            parameters.Add(tradeItemInfo.RequiresQuantityCheck);
            parameters.Add(tradeItemInfo.RequiredInProposal);

            parameters.Add(tradeItemInfo.ModifiedDate);
            parameters.Add(tradeItemInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeItem(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Item in database");
            }
        }

        /// <summary>
        /// Adds a Trade Item to the database
        /// </summary>
        public int? AddTradeItem(TradeItemInfo tradeItemInfo)
        {
            int? tradeItemId = null;
            ParticipationItemInfo participationItemInfo;
            List<Object> parameters = new List<Object>();
            List<TradeParticipationInfo> tradeParticipations = new List<TradeParticipationInfo>();

            SetCreateInfo(tradeItemInfo);

            parameters.Add(GetId(tradeItemInfo.TradeItemCategory));
            parameters.Add(tradeItemInfo.Name);
            parameters.Add(tradeItemInfo.Units);
            parameters.Add(tradeItemInfo.Scope);
            parameters.Add(tradeItemInfo.RequiresQuantityCheck);
            parameters.Add(tradeItemInfo.RequiredInProposal);

            parameters.Add(tradeItemInfo.CreatedDate);
            parameters.Add(tradeItemInfo.CreatedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    tradeItemId = Data.DataProvider.GetInstance().AddTradeItem(parameters.ToArray());
                    tradeItemInfo.Id = tradeItemId;

                    if (tradeItemInfo.TradeItemCategory.Trade.Participations != null)
                    {
                        foreach (TradeParticipationInfo tradeParticipation in tradeItemInfo.TradeItemCategory.Trade.Participations)
                        {
                            tradeParticipations.Add(tradeParticipation);

                            if (tradeParticipation.QuoteParticipation != null)
                                tradeParticipations.Add(tradeParticipation.QuoteParticipation);
                        }

                        foreach (TradeParticipationInfo tradeParticipation in tradeParticipations)
                        {
                            participationItemInfo = new ParticipationItemInfo();
                            SetCreateInfo(participationItemInfo);
                            participationItemInfo.TradeItem = tradeItemInfo;
                            participationItemInfo.TradeParticipation = tradeParticipation;
                            AddParticipationItem(participationItemInfo);
                        }
                    }
             
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Trade Item Type to database");
            }

            return tradeItemId;
        }

        /// <summary>
        /// Adds or updates a TradeItem
        /// </summary>
        public int? AddUpdateTradeItem(TradeItemInfo tradeItemInfo)
        {
            if (tradeItemInfo != null)
            {
                if (tradeItemInfo.Id != null)
                {
                    UpdateTradeItem(tradeItemInfo);
                    return tradeItemInfo.Id;
                }
                else
                {
                    return AddTradeItem(tradeItemInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Trade Item from persistent storage
        /// </summary>
        public void DeleteTradeItem(TradeItemInfo tradeItemInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteTradeItem(tradeItemInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Trade Item from database");
            }
        }
#endregion

#region Trade Item Categories Methods
        /// <summary>
        /// Creates a Trade Item Category from a dr
        /// </summary>
        public TradeItemCategoryInfo CreateTradeItemCategory(IDataReader dr)
        {
            TradeItemCategoryInfo tradeItemCategoryInfo = new TradeItemCategoryInfo(Data.Utils.GetDBInt32(dr["TradeItemCategoryId"]));
            tradeItemCategoryInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            tradeItemCategoryInfo.ShortDescription = Data.Utils.GetDBString(dr["ShortDescription"]);
            tradeItemCategoryInfo.LongDescription = Data.Utils.GetDBString(dr["LongDescription"]);
            tradeItemCategoryInfo.DisplayOrder = Data.Utils.GetDBInt32(dr["DisplayOrder"]);

            AssignAuditInfo(tradeItemCategoryInfo, dr);

            if (dr["TradeId"] != DBNull.Value)
            {
                tradeItemCategoryInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                tradeItemCategoryInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                if (dr["TradeTemplateId"] != DBNull.Value) 
                    tradeItemCategoryInfo.Trade.TradeTemplate = new TradeTemplateInfo(Data.Utils.GetDBInt32(dr["TradeTemplateId"]));

                if (dr["ProjectId"] != DBNull.Value)
                {
                    tradeItemCategoryInfo.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    tradeItemCategoryInfo.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                }
            }
            
            return tradeItemCategoryInfo;
        }

        /// <summary>
        /// Get a Trade Item Category from persistent storage
        /// </summary>
        public TradeItemCategoryInfo GetTradeItemCategory(int? tradeItemCategoryId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeItemCategory(tradeItemCategoryId);
                if (dr.Read())
                    return CreateTradeItemCategory(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Item Category from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a Trade Item Category including its Trade Items from persistent storage
        /// </summary>
        public TradeItemCategoryInfo GetDeepTradeItemCategory(int? tradeItemCategoryId)
        {
            TradeItemCategoryInfo tradeItemCategoryInfo = GetTradeItemCategory(tradeItemCategoryId);
            tradeItemCategoryInfo.TradeItems = GetTradeItems(tradeItemCategoryInfo);
            return tradeItemCategoryInfo;
        }

        /// <summary>
        /// Get a Trade Item Category including its Trade Items from persistent storage
        /// </summary>
        public TradeItemCategoryInfo GetDeepTradeItemCategory(IDataReader dr)
        {
            TradeItemCategoryInfo tradeItemCategoryInfo = CreateTradeItemCategory(dr);
            tradeItemCategoryInfo.TradeItems = GetTradeItems(tradeItemCategoryInfo);
            return tradeItemCategoryInfo;
        }
       
        /// <summary>
        /// Get the Trade Item Categories for the specified Trade from persistent storage
        /// </summary>
        public List<TradeItemCategoryInfo> GetTradeItemCategories(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            List<TradeItemCategoryInfo> tradeItemCategoryInfoList = new List<TradeItemCategoryInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeItemCategories(tradeInfo.Id);
                while (dr.Read())
                    tradeItemCategoryInfoList.Add(CreateTradeItemCategory(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Item Categories from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeItemCategoryInfoList;
        }

        /// <summary>
        /// Get the TradeItem Categories with its Trade Items for the specified Trade from persistent storage
        /// </summary>
        public List<TradeItemCategoryInfo> GetDeepTradeItemCategories(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            List<TradeItemCategoryInfo> tradeItemCategoryInfoList = new List<TradeItemCategoryInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeItemCategories(tradeInfo.Id);
                while (dr.Read())
                    tradeItemCategoryInfoList.Add(GetDeepTradeItemCategory(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Item Categories with items from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeItemCategoryInfoList;
        }

        /// <summary>
        /// Updates a Trade Item Category in the database
        /// </summary>
        public void UpdateTradeItemCategory(TradeItemCategoryInfo tradeItemCategoryInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeItemCategoryInfo);

            parameters.Add(tradeItemCategoryInfo.Id);
            parameters.Add(tradeItemCategoryInfo.Name);
            parameters.Add(tradeItemCategoryInfo.ShortDescription);
            parameters.Add(tradeItemCategoryInfo.LongDescription);
            parameters.Add(tradeItemCategoryInfo.DisplayOrder);

            parameters.Add(tradeItemCategoryInfo.ModifiedDate);
            parameters.Add(tradeItemCategoryInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeItemCategory(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Item Category in database");
            }
        }

        /// <summary>
        /// Adds a Trade Item Category to the database
        /// </summary>
        public int? AddTradeItemCategory(TradeItemCategoryInfo tradeItemCategoryInfo)
        {
            int? tradeItemCategoryId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(tradeItemCategoryInfo);

            parameters.Add(GetId(tradeItemCategoryInfo.Trade));
            parameters.Add(tradeItemCategoryInfo.Name);
            parameters.Add(tradeItemCategoryInfo.ShortDescription);
            parameters.Add(tradeItemCategoryInfo.LongDescription);

            parameters.Add(tradeItemCategoryInfo.CreatedDate);
            parameters.Add(tradeItemCategoryInfo.CreatedBy);

            try
            {
                tradeItemCategoryId = Data.DataProvider.GetInstance().AddTradeItemCategory(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Trade Item Category to database");
            }

            return tradeItemCategoryId;
        }

        /// <summary>
        /// Adds or updates a Trade Item Category
        /// </summary>
        public int? AddUpdateTradeItemCategory(TradeItemCategoryInfo tradeItemCategoryInfo)
        {
            if (tradeItemCategoryInfo != null)
            {
                if (tradeItemCategoryInfo.Id != null)
                {
                    UpdateTradeItemCategory(tradeItemCategoryInfo);
                    return tradeItemCategoryInfo.Id;
                }
                else
                {
                    return AddTradeItemCategory(tradeItemCategoryInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Trade Item Category from persistent storage
        /// </summary>
        public void DeleteTradeItemCategory(TradeItemCategoryInfo tradeItemCategoryInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteTradeItemCategory(tradeItemCategoryInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Trade Item Category from database");
            }
        }

        /// <summary>
        /// Creates a trade based on a template 
        /// </summary>
        public void CopyTradeTemplate(ProjectInfo projectInfo, TradeTemplateInfo tradeTemplateInfo)
        {
            TradeParticipationInfo tradeParticipationInfo;
            ProcessController processController = ProcessController.GetInstance();
            ProcessInfo processInfo;
            int? templateId = null;

            try
            {
                if (tradeTemplateInfo.Trade == null)
                    return;

                templateId = tradeTemplateInfo.Trade.Id;
                SetCreateInfo(tradeTemplateInfo.Trade);
                processInfo = processController.GetDeepProcessTemplate(projectInfo.BusinessUnit, tradeTemplateInfo.Trade.JobType, ProcessTemplateInfo.ProcessTypeTrade);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (processInfo != null)
                    {
                        processInfo.Id = processController.AddProcess(processInfo);

                        foreach (ProcessStepInfo processStepInfo in processInfo.Steps)
                            processStepInfo.Id = processController.AddProcessStep(processStepInfo);

                        tradeTemplateInfo.Trade.Process = processInfo;
                    }

                    tradeTemplateInfo.Trade.Project = projectInfo;
                    tradeTemplateInfo.Trade.Id = AddTrade(tradeTemplateInfo.Trade);
                    tradeTemplateInfo.Trade.Participations = new List<TradeParticipationInfo>();

                    tradeParticipationInfo = new TradeParticipationInfo();
                    SetCreateInfo(tradeParticipationInfo);
                    tradeParticipationInfo.Type = Info.TypeActive;
                    tradeParticipationInfo.Trade = tradeTemplateInfo.Trade;
                    tradeParticipationInfo.Id = AddTradeParticipationWithNoItems(tradeParticipationInfo);
                    tradeTemplateInfo.Trade.Participations.Add(tradeParticipationInfo);
                    
                    foreach (SubContractorInfo subContractorInfo in tradeTemplateInfo.DefaultSubContractors)
                    {
                      //#---     if (subContractorInfo.BusinessUnit.Equals(projectInfo.BusinessUnit))

                      if (subContractorInfo.BusinessUnit.Equals(projectInfo.BusinessUnit) || subContractorInfo.BusinessUnitslist.Find(x => x.Equals(projectInfo.BusinessUnit))!=null  ) //#---
                            {
                            tradeParticipationInfo = new TradeParticipationInfo();
                            SetCreateInfo(tradeParticipationInfo);
                            tradeParticipationInfo.Type = Info.TypeActive;
                            tradeParticipationInfo.Trade = tradeTemplateInfo.Trade;
                            tradeParticipationInfo.SubContractor = subContractorInfo;
                            tradeParticipationInfo.Id = AddTradeParticipationWithNoItems(tradeParticipationInfo);

                            tradeTemplateInfo.Trade.Participations.Add(tradeParticipationInfo);

                            if (projectInfo.IsStatusProposal)
                            {
                                tradeParticipationInfo = new TradeParticipationInfo();
                                SetCreateInfo(tradeParticipationInfo);
                                tradeParticipationInfo.Type = Info.TypeProposal;
                                tradeParticipationInfo.Trade = tradeTemplateInfo.Trade;
                                tradeParticipationInfo.SubContractor = subContractorInfo;
                                tradeParticipationInfo.Id = AddTradeParticipationWithNoItems(tradeParticipationInfo);

                                tradeTemplateInfo.Trade.Participations.Add(tradeParticipationInfo);
                            }
                        }
                    }

                    foreach (DrawingTypeInfo drawingTypeInfo in tradeTemplateInfo.Trade.DrawingTypes)
                        AddTradeDrawingType(tradeTemplateInfo.Trade, drawingTypeInfo);

                    CopyTradeItemCategories(tradeTemplateInfo.Trade, templateId);

                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Copying Trade Template.");
            }
        }
        
        /// <summary>
        /// Copies all trade item categories with items from a template to a trade and create participation items.
        /// </summary>
        public void CopyTradeItemCategories(TradeInfo tradeInfo, int? templateId)
        {
            List<Object> parameters = new List<Object>();

            try
            {
                parameters.Add(templateId);
                parameters.Add(tradeInfo.Id);
                parameters.Add(tradeInfo.CreatedDate);
                parameters.Add(tradeInfo.CreatedBy);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().CopyTradeItemCategories(parameters.ToArray());
                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Copying Trade Item Categories.");
            }
        }
        
        /// <summary>
        /// Makes a copy of a TradeItemCategory in the database.
        /// </summary>
        public void CopyTradeItemCategory(TradeInfo tradeInfo, TradeItemCategoryInfo tradeItemCategoryInfo)
        {
            try
            {
                SetCreateInfo(tradeItemCategoryInfo);
                tradeItemCategoryInfo.Trade = tradeInfo;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    tradeItemCategoryInfo.Id = AddTradeItemCategory(tradeItemCategoryInfo);

                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        CopyTradeItem(tradeItemCategoryInfo, tradeItemInfo);

                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Copying Trade Item Category.");
            }
        }

        /// <summary>
        /// Makes a copies of a a TradeItem in the database adding the participation records 
        /// </summary>
        public void CopyTradeItem(TradeItemCategoryInfo tradeItemCategoryInfo, TradeItemInfo tradeItemInfo)
        {
            try
            {
                SetCreateInfo(tradeItemInfo);
                tradeItemInfo.TradeItemCategory = tradeItemCategoryInfo;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    tradeItemInfo.Id = AddTradeItem(tradeItemInfo);

                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Copying Trade Item.");
            }
        }
#endregion

#region Trade Budget Methods
        /// <summary>
        /// Creates a Trade Budget from a dr.
        /// </summary>
        public TradeBudgetInfo CreateTradeBudget(IDataReader dr, IBudget budgetProvider)
        {
            TradeInfo tradeInfo = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
            tradeInfo.Name = Data.Utils.GetDBString(dr["TradeName"]);
            tradeInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
            tradeInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
            tradeInfo.Contract = new ContractInfo();
            tradeInfo.Contract.ApprovalDate = Data.Utils.GetDBDateTime(dr["BudgetDate"]);

            TradeBudgetInfo tradeBudgetInfo = new TradeBudgetInfo(tradeInfo, budgetProvider);

            tradeBudgetInfo.Amount = Data.Utils.GetDBDecimal(dr["BudgetAmount"]);
            tradeBudgetInfo.BudgetAmountInitial = Data.Utils.GetDBDecimal(dr["BudgetAmountInitial"]);
            tradeBudgetInfo.BudgetAmountTradeInitial = Data.Utils.GetDBDecimal(dr["BudgetAmountTradeInitial"]);
            tradeBudgetInfo.BudgetAmountAllowance = Data.Utils.GetDBDecimal(dr["BudgetAmountAllowance"]);

            AssignAuditInfo(tradeBudgetInfo, dr);

            return tradeBudgetInfo;
        }

        /// <summary>
        /// Get a Trade Budget from persistent storage
        /// </summary>
        public TradeBudgetInfo GetTradeBudget(int? tradeId, IBudget budgetProvider)
        {
            IDataReader dr = null;

            try
            {
                if (budgetProvider is BudgetInfo)
                    dr = Data.DataProvider.GetInstance().GetTradeBudget(budgetProvider.Id, tradeId);
                else if (budgetProvider is ClientVariationTradeInfo)
                    dr = Data.DataProvider.GetInstance().GetTradeClientVariationTrade(budgetProvider.Id, tradeId);
                else
                    throw new Exception("Invalid budget provider");

                if (dr.Read())
                    return CreateTradeBudget(dr, budgetProvider);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Budget from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get all Trade Budgets for the specified Trade from persistent storage
        /// </summary>
        public List<TradeBudgetInfo> GetTradeBudgets(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            IDataReader dr1 = null;
            String clientVariationType;
            BudgetInfo budgetInfo;
            ClientVariationTradeInfo clientVariationTradeInfo;
            List<TradeBudgetInfo> tradeBudgetInfoList = new List<TradeBudgetInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeBudgets(tradeInfo.Id);
                while (dr.Read())
                {
                    budgetInfo = new BudgetInfo(Data.Utils.GetDBInt32(dr["BudgetId"]));
                    budgetInfo.Name = Data.Utils.GetDBString(dr["TradeName"]);
                    budgetInfo.Code = Data.Utils.GetDBString(dr["BudgetCode"]);
                    tradeBudgetInfoList.Add(CreateTradeBudget(dr, budgetInfo));
                }

                dr1 = Data.DataProvider.GetInstance().GetTradeClientVariationTrades(tradeInfo.Id);
                while (dr1.Read())
                {
                    clientVariationType = Data.Utils.GetDBString(dr1["ClientVariationType"]);
                    clientVariationTradeInfo = new ClientVariationTradeInfo(Data.Utils.GetDBInt32(dr1["ClientVariationTradeId"]));
                    clientVariationTradeInfo.TradeCode = Data.Utils.GetDBString(dr1["TradeCode"]);
                    clientVariationTradeInfo.ClientVariation = clientVariationType == ClientVariationInfo.VariationTypeClient ? new ClientVariationInfo(): new SeparateAccountInfo();
                    clientVariationTradeInfo.ClientVariation.Number = Data.Utils.GetDBInt32(dr1["ClientVariationNumber"]);

                    tradeBudgetInfoList.Add(CreateTradeBudget(dr1, clientVariationTradeInfo));
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade budgets from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();

                if (dr1 != null)
                    dr1.Close();
            }

            return tradeBudgetInfoList;
        }

        /// <summary>
        /// Updates a Trade Budget in the database
        /// </summary>
        public void UpdateTradeBudget(TradeBudgetInfo tradeBudgetInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeBudgetInfo);

            parameters.Add(tradeBudgetInfo.BudgetProviderId);
            parameters.Add(tradeBudgetInfo.TradeId);
            parameters.Add(tradeBudgetInfo.Amount);
            parameters.Add(tradeBudgetInfo.BudgetAmountInitial);
            parameters.Add(tradeBudgetInfo.BudgetAmountTradeInitial);
            parameters.Add(tradeBudgetInfo.BudgetAmountAllowance);
            parameters.Add(tradeBudgetInfo.BudgetDate);

            parameters.Add(tradeBudgetInfo.ModifiedDate);
            parameters.Add(tradeBudgetInfo.ModifiedBy);

            try
            {
                if (tradeBudgetInfo.BudgetProvider is BudgetInfo)
                    Data.DataProvider.GetInstance().UpdateTradeBudget(parameters.ToArray());
                else if (tradeBudgetInfo.BudgetProvider is ClientVariationTradeInfo)
                    Data.DataProvider.GetInstance().UpdateTradeClientVariationTrade(parameters.ToArray());
                else
                    throw new Exception("Invalid budget provider");
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Budget in database");
            }
        }

        /// <summary>
        /// Updates a Trade Budget Client Variation Trade in the database
        /// </summary>
        public void UpdateTradeBudgetClientVariationTrade(Int32? oldClientVariationTradeId, Int32? newClientVariationTradeId)
        {
            TradeBudgetInfo tradeBudgetInfo = new TradeBudgetInfo();
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeBudgetInfo);

            parameters.Add(oldClientVariationTradeId);
            parameters.Add(newClientVariationTradeId);
            parameters.Add(tradeBudgetInfo.ModifiedDate);
            parameters.Add(tradeBudgetInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeClientVariationTradeId(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Budget' Client Variation Trade in database");
            }
        }

        /// <summary>
        /// Adds a Trade Budget to the database
        /// </summary>
        public int? AddTradeBudget(TradeBudgetInfo tradeBudgetInfo)
        {
            int? tradeItemCategoryId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(tradeBudgetInfo);

            parameters.Add(tradeBudgetInfo.BudgetProviderId);
            parameters.Add(tradeBudgetInfo.TradeId);
            parameters.Add(tradeBudgetInfo.Amount);
            parameters.Add(tradeBudgetInfo.BudgetAmountInitial);
            parameters.Add(tradeBudgetInfo.BudgetAmountTradeInitial);
            parameters.Add(tradeBudgetInfo.BudgetAmountAllowance);
            parameters.Add(tradeBudgetInfo.BudgetDate);

            parameters.Add(tradeBudgetInfo.CreatedDate);
            parameters.Add(tradeBudgetInfo.CreatedBy);

            try
            {
                if (tradeBudgetInfo.BudgetProvider is BudgetInfo)
                    Data.DataProvider.GetInstance().AddTradeBudget(parameters.ToArray());
                else if (tradeBudgetInfo.BudgetProvider is ClientVariationTradeInfo)
                    Data.DataProvider.GetInstance().AddTradeClientVariationTrade(parameters.ToArray());
                else
                    throw new Exception("Invalid budget provider");
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Trade Budget to database");
            }

            return tradeItemCategoryId;
        }

        /// <summary>
        /// Adds or updates a Trade Budget
        /// </summary>
        public int? AddUpdateTradeBudget(TradeBudgetInfo tradeBudgetInfo)
        {
            if (tradeBudgetInfo != null)
            {
                if (tradeBudgetInfo.Id != null)
                {
                    UpdateTradeBudget(tradeBudgetInfo);
                    return tradeBudgetInfo.Id;
                }
                else
                {
                    return AddTradeBudget(tradeBudgetInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Trade Budget from persistent storage
        /// </summary>
        public void DeleteTradeBudget(TradeBudgetInfo tradeBudgetInfo)
        {
            try
            {
                if (tradeBudgetInfo.BudgetProvider is BudgetInfo)
                    Data.DataProvider.GetInstance().DeleteTradeBudget(tradeBudgetInfo.BudgetProviderId, tradeBudgetInfo.TradeId);
                else if (tradeBudgetInfo.BudgetProvider is ClientVariationTradeInfo)
                    Data.DataProvider.GetInstance().DeleteTradeClientVariationTrade(tradeBudgetInfo.BudgetProviderId, tradeBudgetInfo.TradeId);
                else
                    throw new Exception("Invalid budget provider");
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Trade Budget from database");
            }
        }

        /// <summary>
        /// Set the value of the property Amount for the Budgetparticipation.
        /// If there are no TradeBudgets then budgetParticipation.Amount keeps its original value (Old projects)
        /// </summary>
        /// <param name="trade"></param>
        public void SetBudgetParticipationAmount(TradeInfo tradeInfo) 
        {
            TradeParticipationInfo budgetParticipation = tradeInfo.BudgetParticipation;

            if (budgetParticipation != null)
            {
                if (tradeInfo.TradeBudgets != null && tradeInfo.TradeBudgets.Count > 0)
                {
                    budgetParticipation.Amount = tradeInfo.TotalBudgetAllowance;
                }
            }
        }
#endregion

#region Trades Methods
        /// <summary>
        /// Create a Trade from a dr and entities dictionaries
        /// </summary>
        public TradeInfo CreateTrade(IDataReader dr, Dictionary<int, ContractInfo> contractsDictionary, Dictionary<int, JobTypeInfo> jobTypesDictionary, Dictionary<int, ProcessInfo> processesDictionary, Dictionary<int, PeopleInfo> peopleDictionary)
        {
            TradeInfo tradeInfo = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));

            tradeInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            tradeInfo.Code = Data.Utils.GetDBString(dr["Code"]);
            tradeInfo.TenderRequired = Data.Utils.GetDBBoolean(dr["TenderRequired"]);
            tradeInfo.Description = Data.Utils.GetDBString(dr["Description"]);
            tradeInfo.ScopeHeader = Data.Utils.GetDBString(dr["ScopeHeader"]);
            tradeInfo.ScopeFooter = Data.Utils.GetDBString(dr["ScopeFooter"]);
            tradeInfo.DisplayOrder = Data.Utils.GetDBInt32(dr["DisplayOrder"]);
            tradeInfo.DaysFromPCD = Data.Utils.GetDBInt32(dr["DaysFromPCD"]);
            tradeInfo.InvitationDate = Data.Utils.GetDBDateTime(dr["InvitationDate"]);
            tradeInfo.DueDate = Data.Utils.GetDBDateTime(dr["DueDate"]);
            tradeInfo.ComparisonDueDate = Data.Utils.GetDBDateTime(dr["ComparisonDueDate"]);
            tradeInfo.ContractDueDate = Data.Utils.GetDBDateTime(dr["ContractDueDate"]);
            tradeInfo.ComparisonApprovalDate = Data.Utils.GetDBDateTime(dr["ComparisonApprovalDate"]);
            tradeInfo.ComparisonApprovalAmount = Data.Utils.GetDBDecimal(dr["ComparisonApprovalAmount"]);
            tradeInfo.CommencementDate = Data.Utils.GetDBDateTime(dr["CommencementDate"]);
            tradeInfo.CompletionDate = Data.Utils.GetDBDateTime(dr["CompletionDate"]);
            tradeInfo.WorkOrderNumber = Data.Utils.GetDBString(dr["WorkOrderNumber"]);
            tradeInfo.QuotesFile = Data.Utils.GetDBString(dr["QuotesFile"]);
            tradeInfo.PrelettingFile = Data.Utils.GetDBString(dr["PrelettingFile"]);

            //#-------
            if (dr["SignedContractFile"] != null) { 
            tradeInfo.SignedContractFile = Data.Utils.GetDBString(dr["SignedContractFile"]);
            }
            //#-------

            tradeInfo.Flag = Data.Utils.GetDBInt32(dr["Flag"]);

            AssignAuditInfo(tradeInfo, dr);

            if (dr["ProjectId"] != DBNull.Value)
            {
                tradeInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                tradeInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                tradeInfo.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
            }

            if (dr["TradeTemplateId"] != DBNull.Value) tradeInfo.TradeTemplate = new TradeTemplateInfo(Data.Utils.GetDBInt32(dr["TradeTemplateId"]));

            if (dr["ContractId"] != DBNull.Value) tradeInfo.Contract = ContractsController.GetInstance().CreateContract(dr["ContractId"], contractsDictionary);
            if (dr["JobTypeId"] != DBNull.Value) tradeInfo.JobType = ContractsController.GetInstance().CreateJobType(dr["JobTypeId"], jobTypesDictionary);
            if (dr["ProcessId"] != DBNull.Value) tradeInfo.Process = ProcessController.GetInstance().CreateProcess(dr["ProcessId"], processesDictionary);
            if (dr["CAPeopleId"] != DBNull.Value) tradeInfo.ContractsAdministrator = (EmployeeInfo)PeopleController.GetInstance().CreatePerson(dr["CAPeopleId"], peopleDictionary);
            if (dr["PMPeopleId"] != DBNull.Value) tradeInfo.ProjectManager = (EmployeeInfo)PeopleController.GetInstance().CreatePerson(dr["PMPeopleId"], peopleDictionary);

            return tradeInfo;
        }

        /// <summary>
        /// Create a Trade from a dr
        /// </summary>
        public TradeInfo CreateTrade(IDataReader dr)
        {
            return CreateTrade(dr, null, null, null, null);
        }

        /// <summary>
        /// Get a Trade from persistent storage
        /// </summary>
        public TradeInfo GetTrade(int? TradeId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTrade(TradeId);
                if (dr.Read())
                    return CreateTrade(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a Trade with participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithParticipations(int? tradeId, String participationType)
        {
            TradeInfo tradeInfo = GetTrade(tradeId);
            tradeInfo.Participations = GetDeepTradeParticipations(tradeInfo, participationType);
            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithParticipationsAndBudget(int? tradeId)
        {
            TradeInfo tradeInfo = GetTradeWithParticipations(tradeId, null);
            tradeInfo.TradeBudgets = GetTradeBudgets(tradeInfo);

            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithParticipations(int? tradeId)
        {
            return GetTradeWithParticipations(tradeId, null);
        }

        /// <summary>
        /// Get a Trade with active participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithParticipationsActive(int? tradeId)
        {
            return GetTradeWithParticipations(tradeId, Info.TypeActive);
        }

        /// <summary>
        /// Get a Trade with proposal participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithParticipationsProposal(int? tradeId)
        {
            return GetTradeWithParticipations(tradeId, Info.TypeProposal);
        }

        /// <summary>
        /// Get a Trade with items from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithItems(int? tradeId)
        {
            TradeInfo tradeInfo = GetTrade(tradeId);
            tradeInfo.ItemCategories = GetDeepTradeItemCategories(tradeInfo);
            return tradeInfo;
        }

        /// <summary>
        /// <summary>
        /// Get a Trade with items and participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithItemsAndParticipations(int? tradeId, String participationType)
        {
            TradeInfo tradeInfo = GetTrade(tradeId);

            tradeInfo.ItemCategories = GetDeepTradeItemCategories(tradeInfo);

            if (participationType == Info.TypeActive)
                tradeInfo.Participations = GetDeepTradeParticipationsActive(tradeInfo);
            else if (participationType == Info.TypeProposal)
                tradeInfo.Participations = GetDeepTradeParticipationsProposal(tradeInfo);
            else
                throw new Exception("Getting trade with items and participations. Invalid participation type");

            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with items and participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithItemsAndParticipations(int? tradeId)
        {
            return GetTradeWithItemsAndParticipations(tradeId, null);
        }

        /// <summary>
        /// Get a Trade with items and participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithItemsAndParticipationsActive(int? tradeId)
        {
            return GetTradeWithItemsAndParticipations(tradeId, Info.TypeActive);
        }

        /// <summary>
        /// Get a Trade with items and participations from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithItemsAndParticipationsProposal(int? tradeId)
        {
            return GetTradeWithItemsAndParticipations(tradeId, Info.TypeProposal);
        }
        
        /// <summary>
        /// Get a Trade with categories, items, drawings, participations and contracts from persistent storage.
        /// </summary>
        public TradeInfo GetDeepTrade(int? tradeId)
        {
            TradeInfo tradeInfo = GetTrade(tradeId);

            tradeInfo.DrawingTypes = GetDrawingTypes(tradeInfo);
            tradeInfo.Drawings = GetDeepDrawings(tradeInfo);
            tradeInfo.ItemCategories = GetDeepTradeItemCategories(tradeInfo);
            tradeInfo.Participations = GetDeepTradeParticipations(tradeInfo);

            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with categories, items, drawings active, participations and contracts active from persistent storage.
        /// </summary>
        public TradeInfo GetDeepTradeActive(int? tradeId)
        {
            TradeInfo tradeInfo = GetTradeWithItemsAndParticipationsActive(tradeId);

            tradeInfo.DrawingTypes = GetDrawingTypes(tradeInfo);
            tradeInfo.Drawings = GetDeepDrawings(tradeInfo);
            tradeInfo.TradeBudgets = GetTradeBudgets(tradeInfo);
            SetBudgetParticipationAmount(tradeInfo);

            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with categories, items, drawings, participations and contracts from persistent storage.
        /// </summary>
        public TradeInfo GetDeepTradeWithAddendums(int? tradeId)
        {
            TradeInfo tradeInfo = GetDeepTrade(tradeId);
            tradeInfo.Addendums = GetAddendumsWithAttachments(tradeInfo);
            return tradeInfo;
        }

        /// <summary>
        /// Get deep Trade 
        /// </summary>
        public TradeInfo GetDeepTradeWithBudget(int? tradeId)
        {
            TradeInfo tradeInfo = GetDeepTrade(tradeId);
            tradeInfo.TradeBudgets = GetTradeBudgets(tradeInfo);
            SetBudgetParticipationAmount(tradeInfo);
            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with drawings from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithDrawings(int? tradeId)
        {
            TradeInfo tradeInfo = GetTrade(tradeId);

            tradeInfo.DrawingTypes = GetDrawingTypes(tradeInfo);
            tradeInfo.Drawings = GetDeepDrawings(tradeInfo);

            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with drawings and items from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithDrawingsAndItems(int? tradeId)
        {
            TradeInfo tradeInfo = GetTrade(tradeId);

            tradeInfo.DrawingTypes = GetDrawingTypes(tradeInfo);
            tradeInfo.Drawings = GetDeepDrawings(tradeInfo);
            tradeInfo.ItemCategories = GetDeepTradeItemCategories(tradeInfo);

            return tradeInfo;
        }
               
        /// <summary>
        /// Get a Trade with addendums from persistent storage.
        /// </summary>
        public TradeInfo GetTradeWithAddendums(int? tradeId)
        {
            TradeInfo tradeInfo = GetTrade(tradeId);
            tradeInfo.Addendums = GetAddendums(tradeInfo);
            return tradeInfo;
        }

        /// <summary>
        /// Get a Trade with categories, items, drawings, participations and contracts from persistent storage.
        /// </summary>
        public TradeInfo GetDeepTrade(IDataReader dr)
        {
            TradeInfo tradeInfo = CreateTrade(dr);

            tradeInfo.DrawingTypes = GetDrawingTypes(tradeInfo);
            tradeInfo.Drawings = GetDeepDrawings(tradeInfo);
            tradeInfo.ItemCategories = GetDeepTradeItemCategories(tradeInfo);
            tradeInfo.Participations = GetDeepTradeParticipations(tradeInfo);

            return tradeInfo;
        }

        /// <summary>
        /// Search trades
        /// </summary>
        public List<TradeInfo> SearchTrades(String projectFilter, String tradeFilter, int? subbieFilter)
        {
            IDataReader dr = null;
            TradeInfo tradeInfo = null;
            List<Object> parameters = new List<Object>();
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            ContractsController contractsController = ContractsController.GetInstance();

            parameters.Add(projectFilter);
            parameters.Add(tradeFilter);
            parameters.Add(subbieFilter);

            try
            {
                dr = Data.DataProvider.GetInstance().SearchTrades(parameters.ToArray());
                while (dr.Read())
                {
                    tradeInfo = GetTrade(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeInfo.Participations = new List<TradeParticipationInfo>();

                    if (dr["tradeParticipationId"] != DBNull.Value)
                        tradeInfo.Participations.Add(GetTradeParticipation(Data.Utils.GetDBInt32(dr["tradeParticipationId"])));
                    
                    if (tradeInfo.Contract != null)
                        tradeInfo.Contract.Subcontracts = contractsController.GetSubContracts(tradeInfo.Contract);

                    tradeInfoList.Add(tradeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Searching Trades in database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Search work orders
        /// </summary>
        public List<TradeInfo> SearchWorkOrders(String projectFilter)
        {
            IDataReader dr = null;
            IDataReader dr1 = null;
            TradeInfo tradeInfo = null;
            TradeParticipationInfo tradeParticipationInfo = null;
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            ContractsController contractsController = ContractsController.GetInstance();

            try
            {
                dr = Data.DataProvider.GetInstance().SearchWorkOrders(projectFilter);
                while (dr.Read())
                {
                    tradeInfo = GetTrade(Data.Utils.GetDBInt32(dr["TradeId"]));

                    tradeInfo.TradeBudgets = GetTradeBudgets(tradeInfo);

                    if (tradeInfo.Contract != null)
                        tradeInfo.Contract.Subcontracts = contractsController.GetSubContractsWithVariations(tradeInfo.Contract);

                    tradeInfo.Participations = new List<TradeParticipationInfo>();

                    dr1 = Data.DataProvider.GetInstance().GetTradeParticipationBudgetAndSelected(tradeInfo.Id);
                    while (dr1.Read())
                    {
                        tradeParticipationInfo = CreateTradeParticipation(dr1);
                        tradeParticipationInfo.ParticipationItems = GetParticipationItems(tradeParticipationInfo);
                        tradeInfo.Participations.Add(tradeParticipationInfo);
                    }
                    dr1.Close();

                    SetBudgetParticipationAmount(tradeInfo);

                    tradeInfoList.Add(tradeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Searching Trades in database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();

                if (dr1 != null)
                    if (!dr1.IsClosed)
                        dr1.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Get the Trades for the specified project
        /// </summary>
        public List<TradeInfo> GetTradesBasic(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            TradeInfo tradeInfo;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradesBasic(projectInfo.Id);
                while (dr.Read())
                {
                    tradeInfo = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    tradeInfo.Code = Data.Utils.GetDBString(dr["Code"]);

                    if (dr["ContractId"] != DBNull.Value)
                    {
                        tradeInfo.Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));
                        tradeInfo.Contract.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
                    }

                    tradeInfoList.Add(tradeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades Basic from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Get the Trades for the specified Drawing.
        /// </summary>
        public List<TradeInfo> GetTrades(DrawingInfo drawingInfo)
        {
            List<Object> parameters = new List<Object>();

            IDataReader dr = null;
            IDataReader dr1 = null;
            TradeInfo tradeInfo;
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            List<TradeInfo> tradeInfoList1 = new List<TradeInfo>();
            List<TradeInfo> tradeInfoList2 = new List<TradeInfo>();

            try
            {
                parameters.Add(drawingInfo.Project.Id);
                parameters.Add(drawingInfo.DrawingType.Id);

                dr = Data.DataProvider.GetInstance().GetTradesForDrawingType(parameters.ToArray());
                while (dr.Read())
                {
                    tradeInfo = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeInfo.Name = Data.Utils.GetDBString(dr["Name"]);

                    tradeInfoList.Add(tradeInfo);
                }

                dr1 = Data.DataProvider.GetInstance().GetTradesForDrawing(drawingInfo.Id);
                while (dr1.Read())
                {
                    tradeInfo = new TradeInfo(Data.Utils.GetDBInt32(dr1["TradeId"]));
                    tradeInfo.Name = Data.Utils.GetDBString(dr1["Name"]);

                    tradeInfoList1.Add(tradeInfo);
                }

                foreach (TradeInfo trade in tradeInfoList)
                    if (tradeInfoList1.Find(delegate(TradeInfo tradeInfoInList) { return tradeInfoInList.Equals(trade); }) == null)
                        tradeInfoList2.Add(trade);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Drawing Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();

                if (dr1 != null)
                    dr1.Close();
            }

            return tradeInfoList2;
        }

        /// <summary>
        /// Get Deep the Trades for the specified project
        /// </summary>
        public List<TradeInfo> GetDeepTrades(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTrades(projectInfo.Id);
                while (dr.Read())
                    tradeInfoList.Add(GetDeepTrade(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Updates the trade process dates
        /// </summary>
        public void UpdateTradeProcessDates(TradeInfo tradeInfo)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            DateTime? finalDate;

            if (tradeInfo.Process != null)
            {
                projectsController.InitializeHolidays();
                finalDate = projectsController.AddbusinessDaysToDate(tradeInfo.ComparisonDueDate, Int32.Parse(Web.Utils.GetConfigListItemValue("Trades", "Settings", "DaysToFinalDate")));
                processController.UpdateProcessDates(projectsController, tradeInfo.Process, finalDate);
            }
        }

        /// <summary>
        /// Updates the trade dates
        /// </summary>
        public void UpdateTradeDates(TradeInfo tradeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeInfo);

            parameters.Add(tradeInfo.Id);
            parameters.Add(tradeInfo.InvitationDate);
            parameters.Add(tradeInfo.DueDate);
            parameters.Add(tradeInfo.ComparisonDueDate);
            parameters.Add(tradeInfo.ContractDueDate);

            parameters.Add(tradeInfo.ModifiedDate);
            parameters.Add(tradeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeDates(parameters.ToArray());

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating trade dates in database");
            }
        }

        /// <summary>
        /// Updates a Trade process based on conditions
        /// </summary>
        public void UpdateTradeProcess(TradeInfo tradeInfo)
        {
            TradeParticipationInfo tradeParticipationInfo;
            TradeParticipationInfo budgetParticipationInfo;
            ProcessStepInfo processStepInfo;
            String stepComparisonApproval = String.Empty;
            decimal comparisonTotal;
            decimal budget;

            //#---
            decimal budgetPercent;
            decimal overBudget;
            //#---

            try
            {
                if (!tradeInfo.ComparisonApproved)
                {
                    tradeParticipationInfo = tradeInfo.SelectedParticipation;
                    budgetParticipationInfo = tradeInfo.BudgetParticipation;

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        if (tradeParticipationInfo != null && tradeParticipationInfo.Amount != null)
                        {
                            comparisonTotal = GetComparisonTotal(tradeParticipationInfo);

                            #region Old

                            //#------
                            //if (tradeInfo.Project.BusinessUnit.TradeAmountApproval != null)
                            //{
                            //    processStepInfo = tradeInfo.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeComparisonUM); });

                            //    if (processStepInfo != null)
                            //    {
                            //        if (comparisonTotal > (decimal)tradeInfo.Project.BusinessUnit.TradeAmountApproval)
                            //        {
                            //            if (processStepInfo.SkipStep)
                            //            {
                            //                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonUM;
                            //                processStepInfo.Skip = false;
                            //                ProcessController.GetInstance().UpdateProcessStepSkip(processStepInfo);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            if (!processStepInfo.SkipStep)
                            //            {
                            //                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonCM;
                            //                processStepInfo.Skip = true;
                            //                ProcessController.GetInstance().UpdateProcessStepSkip(processStepInfo);
                            //            }
                            //        }
                            //    }
                            //}

                            //#-----
                            #endregion Old

                            if (budgetParticipationInfo != null && budgetParticipationInfo.Amount != null && tradeInfo.Project.BusinessUnit.TradeOverbudgetApproval != null)
                            {
                                budget = (decimal)budgetParticipationInfo.Amount;


                                //#-------
                                   
                                    // for COM Approval---comparison Amt > bz unit trade com amt or over budget by more than 10 %

                                    //COM Approval---------If comparisontotal>49,999 0r Overbudget> 10% of budget ex: 50000 budget and Comparison Total 58000 then over budget 8000 is greater than 10% of budget(50000 is 5000) 
                              
                              if (tradeInfo.Project.BusinessUnit.TradeComAmountApproval != null|| tradeInfo.Project.BusinessUnit.TradeComOverBudget!=null)
                                {
                                    processStepInfo = tradeInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeComparisonCO); });

                                    if (processStepInfo != null)
                                    {
                                        budgetPercent = (budget * (decimal)(tradeInfo.Project.BusinessUnit.TradeComOverBudget)) / 100;
                                        overBudget = comparisonTotal - budget;


                                        if (comparisonTotal > (decimal)tradeInfo.Project.BusinessUnit.TradeComAmountApproval || overBudget > budgetPercent)
                                        {
                                            if (processStepInfo.SkipStep)
                                            {
                                                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonCO;
                                                processStepInfo.Skip = false;
                                                ProcessController.GetInstance().UpdateProcessStepSkipStepComparisonApproval(processStepInfo);
                                            }
                                        }
                                        else
                                        {
                                            if (!processStepInfo.SkipStep)
                                            {
                                                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonCM;
                                                processStepInfo.Skip = true;
                                                ProcessController.GetInstance().UpdateProcessStepSkipStepComparisonApproval(processStepInfo);
                                            }
                                        }
                                    }
                                }



                                //#-------for COM Approval



                                //For UM Approval------ ex:if overbudget >9,999

                                if (tradeInfo.Project.BusinessUnit.TradeUMOverbudgetApproval != null)
                                {
                                    processStepInfo = tradeInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeComparisonUM); });

                                    if (processStepInfo != null)
                                    {
                                        if ((comparisonTotal - budget) > (decimal)tradeInfo.Project.BusinessUnit.TradeUMOverbudgetApproval)
                                        {
                                            if (processStepInfo.SkipStep)
                                            {
                                                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonUM;
                                                processStepInfo.Skip = false;
                                                ProcessController.GetInstance().UpdateProcessStepSkipStepComparisonApproval(processStepInfo);
                                            }
                                        }
                                        else
                                        {
                                            if (!processStepInfo.SkipStep)
                                            {
                                                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonCM;
                                                processStepInfo.Skip = true;
                                                ProcessController.GetInstance().UpdateProcessStepSkipStepComparisonApproval(processStepInfo);
                                            }
                                        }
                                    }
                                }











                                //For DA Approval-----------if comparison Total>499,999 or overbudget> 49,9999

                                processStepInfo = tradeInfo.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeComparisonDA); });

                                if (processStepInfo != null)
                                {
                                    //#---
                                    decimal daApprovalAmt = 0;

                                    if (tradeInfo.Project.BusinessUnit.TradeDAAmountApproval != null) { 
                                        daApprovalAmt = (decimal)tradeInfo.Project.BusinessUnit.TradeDAAmountApproval;}
                                    else { 
                                        daApprovalAmt = comparisonTotal;}

                                    //#---- if (comparisonTotal - budget > (decimal)tradeInfo.Project.BusinessUnit.TradeOverbudgetApproval)
                                    if (comparisonTotal - budget > (decimal)tradeInfo.Project.BusinessUnit.TradeOverbudgetApproval || comparisonTotal > daApprovalAmt)
                                    //#----
                                    {
                                        if (processStepInfo.SkipStep)
                                        {
                                            if (stepComparisonApproval != ProcessStepInfo.StepTypeComparisonUM)
                                                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonDA;

                                            processStepInfo.Skip = false;
                                            ProcessController.GetInstance().UpdateProcessStepSkipStepComparisonApproval(processStepInfo);
                                        }
                                    }
                                    else
                                    {
                                        if (!processStepInfo.SkipStep)
                                        {
                                            if (stepComparisonApproval == String.Empty)
                                                stepComparisonApproval = ProcessStepInfo.StepTypeComparisonCM;

                                            processStepInfo.Skip = true;
                                            ProcessController.GetInstance().UpdateProcessStepSkipStepComparisonApproval(processStepInfo);
                                        }
                                    }
                                }
                            }









                            if (stepComparisonApproval != String.Empty)
                                ProcessController.GetInstance().UpdateProcessStepComparisonApproval(tradeInfo.Process, stepComparisonApproval);
                        }

                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Process in database");
            }
        }

        /// <summary>
        /// Updates a Trade in the database
        /// </summary>
        public void UpdateTrade(TradeInfo tradeInfo)
        {
            List<Object> parameters = new List<Object>();
            TradeParticipationInfo budgetParticipationInfo;
            TradeInfo originalTradeInfo;

            SetModifiedInfo(tradeInfo);

            parameters.Add(tradeInfo.Id);
            parameters.Add(GetId(tradeInfo.JobType));
            parameters.Add(tradeInfo.Name);
            parameters.Add(tradeInfo.Code);
            parameters.Add(tradeInfo.TenderRequired);
            parameters.Add(tradeInfo.Description);
            parameters.Add(tradeInfo.ScopeHeader);
            parameters.Add(tradeInfo.ScopeFooter);
            parameters.Add(tradeInfo.DisplayOrder);
            parameters.Add(tradeInfo.DaysFromPCD);
            parameters.Add(tradeInfo.InvitationDate);
            parameters.Add(tradeInfo.DueDate);
            parameters.Add(tradeInfo.ComparisonDueDate);
            parameters.Add(tradeInfo.ContractDueDate);
            parameters.Add(tradeInfo.ComparisonApprovalDate);
            parameters.Add(tradeInfo.ComparisonApprovalAmount);
            parameters.Add(tradeInfo.CommencementDate);
            parameters.Add(tradeInfo.CompletionDate);
            parameters.Add(tradeInfo.WorkOrderNumber);
            parameters.Add(tradeInfo.QuotesFile);
            parameters.Add(tradeInfo.PrelettingFile);
            parameters.Add(tradeInfo.Flag);
            parameters.Add(GetId(tradeInfo.ContractsAdministrator));
            parameters.Add(GetId(tradeInfo.ProjectManager));

            parameters.Add(tradeInfo.ModifiedDate);
            parameters.Add(tradeInfo.ModifiedBy);

            try
            {
                budgetParticipationInfo = tradeInfo.BudgetParticipation;

                if (budgetParticipationInfo != null)
                    budgetParticipationInfo.Trade = tradeInfo;

                originalTradeInfo = GetTrade(tradeInfo.Id);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (budgetParticipationInfo != null)
                        UpdateTradeParticipation(budgetParticipationInfo);

                    Data.DataProvider.GetInstance().UpdateTrade(parameters.ToArray());

                    if (originalTradeInfo.ComparisonDueDate == null)
                    {
                        if (tradeInfo.ComparisonDueDate != null)
                            UpdateTradeProcessDates(tradeInfo);
                    }
                    else
                        if (tradeInfo.ComparisonDueDate == null)
                            UpdateTradeProcessDates(tradeInfo);
                        else
                            if (!originalTradeInfo.ComparisonDueDate.Equals(tradeInfo.ComparisonDueDate))
                                UpdateTradeProcessDates(tradeInfo);                                        
                    
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade in database");
            }
        }



        //#-------- Update Trade with  Subcontractor's signed contract file path


        public void UpdateTradeSignedContractFile(TradeInfo tradeInfo)
        {

            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeInfo);

            parameters.Add(tradeInfo.Id);
            parameters.Add(tradeInfo.SignedContractFile);

            parameters.Add(tradeInfo.ModifiedDate);
            parameters.Add(tradeInfo.ModifiedBy);


            try
            {
                Data.DataProvider.GetInstance().UpdateTradeSingnedContractFile(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Signed Contract File path in database");
            }




        }



        //#-------- 



        /// <summary>
        /// Updates trade display order
        /// </summary>
        public void UpdateTradeDisplayOrder(TradeInfo tradeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeInfo);

            parameters.Add(tradeInfo.Id);
            parameters.Add(tradeInfo.DisplayOrder);

            parameters.Add(tradeInfo.ModifiedDate);
            parameters.Add(tradeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeDisplayOrder(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade diaplay order in database");
            }
        }

        /// <summary>
        /// Sets the contract for a Trade
        /// </summary>
        public void SetTradeContract(TradeInfo tradeInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeInfo.Id);
            parameters.Add(GetId(tradeInfo.Contract));

            try
            {
                Data.DataProvider.GetInstance().SetTradeContract(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Setting Trade Contract in database");
            }
        }

        /// <summary>
        /// Sets the work order number for a Trade
        /// </summary>
        public void SetTradeWorkOrder(TradeInfo tradeInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeInfo.Id);
            parameters.Add(tradeInfo.WorkOrderNumber);

            try
            {
                Data.DataProvider.GetInstance().SetTradeWorkOrder(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Setting Trade Work Order Number in database");
            }
        }

        /// <summary>
        /// Sets the comparison approval date for a Trade
        /// </summary>
        public void SetTradeComparisonApprovalInfo(TradeInfo tradeInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeInfo.Id);
            parameters.Add(tradeInfo.ComparisonApprovalDate);
            parameters.Add(tradeInfo.ComparisonApprovalAmount);

            try
            {
                Data.DataProvider.GetInstance().SetTradeComparisonApprovalInfo(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Setting Trade Comparison Approval Information in database");
            }
        }

        /// <summary>
        /// Adds a Trade to the database
        /// </summary>
        public int? AddTrade(TradeInfo tradeInfo)
        {
            int? tradeId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(tradeInfo);

            parameters.Add(GetId(tradeInfo.Project));
            parameters.Add(GetId(tradeInfo.JobType));
            parameters.Add(GetId(tradeInfo.Process));
            parameters.Add(tradeInfo.Name);
            parameters.Add(tradeInfo.Code);
            parameters.Add(tradeInfo.TenderRequired);
            parameters.Add(tradeInfo.Description);
            parameters.Add(tradeInfo.ScopeHeader);
            parameters.Add(tradeInfo.ScopeFooter);
            parameters.Add(tradeInfo.DaysFromPCD);
            parameters.Add(tradeInfo.InvitationDate);
            parameters.Add(tradeInfo.DueDate);
            parameters.Add(tradeInfo.ComparisonDueDate);
            parameters.Add(tradeInfo.ContractDueDate);
            parameters.Add(tradeInfo.QuotesFile);
            parameters.Add(tradeInfo.PrelettingFile);
            parameters.Add(GetId(tradeInfo.ContractsAdministrator));
            parameters.Add(GetId(tradeInfo.ProjectManager));

            parameters.Add(tradeInfo.CreatedDate);
            parameters.Add(tradeInfo.CreatedBy);

            try
            {
                tradeId = Data.DataProvider.GetInstance().AddTrade(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Trade to database");
            }

            return tradeId;
        }

        /// <summary>
        /// Adds or updates a Trade
        /// </summary>
        public int? AddUpdateTrade(TradeInfo tradeInfo)
        {
            if (tradeInfo != null)
            {
                if (tradeInfo.Id != null)
                {
                    UpdateTrade(tradeInfo);
                    return tradeInfo.Id;
                }
                else
                {
                    return AddTrade(tradeInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Trade from persistent storage
        /// </summary>
        public void DeleteTrade(TradeInfo tradeInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (tradeInfo.Contract != null)
                    {
                        ContractInfo contractInfo = tradeInfo.Contract;
                        tradeInfo.Contract = null;
                        SetTradeContract(tradeInfo);

                        ContractsController.GetInstance().DeleteContract(contractInfo);
                    }

                    Data.DataProvider.GetInstance().DeleteTrade(tradeInfo.Id);

                    if (tradeInfo.Process != null)
                        ProcessController.GetInstance().DeleteProcess(tradeInfo.Process);

                    // Trade participations are deleted in cascade but quote files must be removed programmatically
                    if (tradeInfo.Participations != null)
                    {
                        foreach (TradeParticipationInfo tradeParticipationInfo in tradeInfo.Participations)
                        {
                            tradeParticipationInfo.Trade = tradeInfo;
                            DeleteTradeParticipationQuoteFile(tradeParticipationInfo);
                        }
                    }

                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Trade from database");
            }
        }

        /// <summary>
        /// Add DrawingType to Trade in persistent storage
        /// </summary>
        public void AddTradeDrawingType(TradeInfo tradeInfo, DrawingTypeInfo drawingTypeInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeInfo.Id);
            parameters.Add(drawingTypeInfo.Id);

            try
            {
                Data.DataProvider.GetInstance().AddTradeDrawingType(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Drawing Type to Trade in database");
            }
        }

        /// <summary>
        /// Delete a Drawing Type and All the excluded Drawings for that type of a Trade from persistent storage
        /// </summary>
        public void DeleteTradeDrawingType(TradeInfo tradeInfo, DrawingTypeInfo drawingTypeInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeInfo.Id);
            parameters.Add(drawingTypeInfo.Id);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().DeleteTradeDrawingType(parameters.ToArray());

                    if (tradeInfo.Drawings != null)
                        foreach (DrawingInfo drawingInfo in tradeInfo.Drawings)
                            if (drawingInfo.DrawingType.Equals(drawingTypeInfo))
                                this.DeleteTradeDrawing(tradeInfo, drawingInfo);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Deleting Drawing Type from Trade in database");
            }
        }

        /// <summary>
        /// Add Drawing to Trade in persistent storage
        /// </summary>
        public void AddTradeDrawing(TradeInfo tradeInfo, DrawingInfo drawingInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeInfo.Id);
            parameters.Add(drawingInfo.Id);

            try
            {
                Data.DataProvider.GetInstance().AddTradeDrawing(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Drawing to Trade in database");
            }
        }

        /// <summary>
        /// Delete a Drawing from a Trade from persistent storage
        /// </summary>
        public void DeleteTradeDrawing(TradeInfo tradeInfo, DrawingInfo drawingInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeInfo.Id);
            parameters.Add(drawingInfo.Id);

            try
            {
                Data.DataProvider.GetInstance().DeleteTradeDrawing(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Deleting Drawing from Trade in database");
            }
        }
#endregion

#region Trade Templates Methods
        /// <summary>
        /// Create a Trade Template from persistent storage
        /// </summary>
        public TradeTemplateInfo CreateTradeTemplate(IDataReader dr)
        {
            TradeTemplateInfo tradeTemplateInfo = new TradeTemplateInfo(Data.Utils.GetDBInt32(dr["TradeTemplateId"]));

            tradeTemplateInfo.IsStandard = Data.Utils.GetDBBoolean(dr["IsStandard"]);

            AssignAuditInfo(tradeTemplateInfo, dr);

            if (dr["TradeId"] != DBNull.Value)
            {
                tradeTemplateInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));

                tradeTemplateInfo.Trade.Name = Data.Utils.GetDBString(dr["Name"]);
                tradeTemplateInfo.Trade.Code = Data.Utils.GetDBString(dr["Code"]);
                tradeTemplateInfo.Trade.TenderRequired = Data.Utils.GetDBBoolean(dr["TenderRequired"]);
                tradeTemplateInfo.Trade.DaysFromPCD = Data.Utils.GetDBInt32(dr["DaysFromPCD"]);
                tradeTemplateInfo.Trade.Description = Data.Utils.GetDBString(dr["Description"]);
                tradeTemplateInfo.Trade.ScopeHeader = Data.Utils.GetDBString(dr["ScopeHeader"]);
                tradeTemplateInfo.Trade.ScopeFooter = Data.Utils.GetDBString(dr["ScopeFooter"]);
                tradeTemplateInfo.Trade.DisplayOrder = Data.Utils.GetDBInt32(dr["DisplayOrder"]);

                tradeTemplateInfo.Trade.TradeTemplate = tradeTemplateInfo;

                if (dr["JobTypeId"] != DBNull.Value) tradeTemplateInfo.Trade.JobType = ContractsController.GetInstance().GetJobType(Data.Utils.GetDBInt32(dr["JobTypeId"]));
            }

            return tradeTemplateInfo;
        }

        /// <summary>
        /// Get the default subcontractors for a Trade Template
        /// </summary>
        public List<SubContractorInfo> GetTradeTemplateSubcontractors(TradeTemplateInfo tradeTemplateInfo)
        {
            IDataReader dr = null;
            SubContractorsController subContractorsController = SubContractorsController.GetInstance();
            List<SubContractorInfo> subContractorsList = new List<SubContractorInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplateSubcontractors(tradeTemplateInfo.Id);
                while (dr.Read())
                    subContractorsList.Add(subContractorsController.GetSubContractor(Data.Utils.GetDBInt32(dr["SubContractorId"])));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Template Subcontractors from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return subContractorsList;
        }
        /// <summary>
        /// Get the default subcontractors for a Trade Template by BusinessUNit // DS20231108
        /// </summary>
        public List<SubContractorInfo> GetTradeTemplateSubcontractors(TradeTemplateInfo tradeTemplateInfo,int BusinessUnitId)
        {
            IDataReader dr = null;
            SubContractorsController subContractorsController = SubContractorsController.GetInstance();
            List<SubContractorInfo> subContractorsList = new List<SubContractorInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplateSubcontractorsBU(tradeTemplateInfo.Id, BusinessUnitId);
                while (dr.Read())
                    subContractorsList.Add(subContractorsController.GetSubContractor(Data.Utils.GetDBInt32(dr["SubContractorId"])));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Template Subcontractors from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return subContractorsList;
        }
        /// <summary>
        /// Get the default subcontractors with contacts for a Trade Template
        /// </summary>
        public List<SubContractorInfo> GetDeepTradeTemplateSubcontractors(TradeTemplateInfo tradeTemplateInfo)
        {
            IDataReader dr = null;
            SubContractorsController subContractorsController = SubContractorsController.GetInstance();
            List<SubContractorInfo> subContractorsList = new List<SubContractorInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplateSubcontractors(tradeTemplateInfo.Id);
                while (dr.Read())
                    subContractorsList.Add(subContractorsController.GetSubContractorDeep(Data.Utils.GetDBInt32(dr["SubContractorId"])));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Template Subcontractors with Contacts from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return subContractorsList;
        }

        /// <summary>
        /// Add SubContractor to Trade Template in persistent storage
        /// </summary>
        public void AddTradeTemplateSubContractor(TradeTemplateInfo tradeTemplateInfo, SubContractorInfo subContractorInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeTemplateInfo.Id);
            parameters.Add(subContractorInfo.Id);

            try
            {
                Data.DataProvider.GetInstance().AddTradeTemplateSubcontractor(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Subcontractor to Trade Template in database");
            }
        }

        /// <summary>
        /// Delete a SubContractor to a Trade Template in persistent storage
        /// </summary>
        public void DeleteTradeTemplateSubContractor(TradeTemplateInfo tradeTemplateInfo, SubContractorInfo subContractorInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeTemplateInfo.Id);
            parameters.Add(subContractorInfo.Id);

            try
            {
                Data.DataProvider.GetInstance().DeleteTradeTemplateSubcontractor(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Deleting Subcontractor from Trade Template in database");
            }
        }

        /// <summary>
        /// Get a Trade Template from persistent storage
        /// </summary>
        public TradeTemplateInfo GetTradeTemplate(int? TradeTemplateId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplate(TradeTemplateId);
                if (dr.Read())
                    return CreateTradeTemplate(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Template from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get Trade Template with subcontractos and full Trade info from persistent storage
        /// </summary>
        public TradeTemplateInfo GetDeepTradeTemplate(int? TradeTemplateId)
        {
            TradeTemplateInfo tradeTemplateInfo = null;
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplate(TradeTemplateId);
                if (dr.Read())
                {
                    tradeTemplateInfo = CreateTradeTemplate(dr);
                    if (tradeTemplateInfo.Trade != null)
                    {
                        tradeTemplateInfo.Trade.DrawingTypes = GetDrawingTypes(tradeTemplateInfo.Trade);
                        tradeTemplateInfo.Trade.ItemCategories = GetDeepTradeItemCategories(tradeTemplateInfo.Trade);
                    }

                    tradeTemplateInfo.DefaultSubContractors = GetDeepTradeTemplateSubcontractors(tradeTemplateInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Template from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeTemplateInfo;
        }

        /// <summary>
        /// Get Trade Template with subcontractos and full Trade info from persistent storage
        /// </summary>
        public TradeTemplateInfo GetDeepTradeTemplate(IDataReader dr)
        {
            TradeTemplateInfo tradeTemplateInfo = tradeTemplateInfo = CreateTradeTemplate(dr);

            if (tradeTemplateInfo.Trade != null)
            {
                tradeTemplateInfo.Trade.DrawingTypes = GetDrawingTypes(tradeTemplateInfo.Trade);
                tradeTemplateInfo.Trade.ItemCategories = GetDeepTradeItemCategories(tradeTemplateInfo.Trade);
            }

            tradeTemplateInfo.DefaultSubContractors = GetDeepTradeTemplateSubcontractors(tradeTemplateInfo);

            return tradeTemplateInfo;
        }
        /// <summary>
        /// Get all the Trade Templates from persistent storage          DS20230309 
        /// </summary>
        public List<TradeTemplateInfo> GetTradeTemplatesFromCode(string TypeCode)
        {
            IDataReader dr = null;
            List<TradeTemplateInfo> tradeTemplateInfoList = new List<TradeTemplateInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplateFromCode(TypeCode);
                while (dr.Read())
                    tradeTemplateInfoList.Add(CreateTradeTemplate(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Templates from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeTemplateInfoList;
        }
        /// <summary>
        /// Get all the Trade Templates from persistent storage
        /// </summary>
        public List<TradeTemplateInfo> GetTradeTemplates()
        {
            IDataReader dr = null;
            List<TradeTemplateInfo> tradeTemplateInfoList = new List<TradeTemplateInfo>();
            
            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplates();
                while (dr.Read())
                    tradeTemplateInfoList.Add(CreateTradeTemplate(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Templates from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeTemplateInfoList;
        }

        /// <summary>
        /// Return the standard trade templates list from the templates list
        /// </summary>
        public List<TradeTemplateInfo> GetTradeTemplatesStandard(List<TradeTemplateInfo> tradeTemplateInfoList)
        {
            List<TradeTemplateInfo> tradeTemplateInfoStdList = new List<TradeTemplateInfo>();
            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                if (tradeTemplateInfo.IsStandard != null)
                    if ((bool)tradeTemplateInfo.IsStandard)
                        tradeTemplateInfoStdList.Add(tradeTemplateInfo);

            return tradeTemplateInfoStdList;
        }

        /// <summary>
        /// Return the non-standard trade template list from the templates list
        /// </summary>
        public List<TradeTemplateInfo> GetTradeTemplatesNonStandard(List<TradeTemplateInfo> tradeTemplateInfoList)
        {
            List<TradeTemplateInfo> tradeTemplateNonStdInfoList = new List<TradeTemplateInfo>();
            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                if (tradeTemplateInfo.IsStandard != null)
                    if (!(bool)tradeTemplateInfo.IsStandard)
                        tradeTemplateNonStdInfoList.Add(tradeTemplateInfo);

            return tradeTemplateNonStdInfoList;
        }

        /// <summary>
        /// Get all the Trade Templates with subcontractors and full trade info from persistent storage
        /// </summary>
        public List<TradeTemplateInfo> GetDeepTradeTemplates()
        {
            IDataReader dr = null;
            List<TradeTemplateInfo> tradeTemplateInfoList = new List<TradeTemplateInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeTemplates();
                while (dr.Read())
                    tradeTemplateInfoList.Add(GetDeepTradeTemplate(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Templates from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeTemplateInfoList;
        }

        /// <summary>
        /// Updates a Trade Template in the database
        /// </summary>
        public void UpdateTradeTemplate(TradeTemplateInfo tradeTemplateInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeTemplateInfo);

            parameters.Add(tradeTemplateInfo.Id);
            parameters.Add(GetId(tradeTemplateInfo.Trade.JobType));
            parameters.Add(tradeTemplateInfo.Trade.Name);
            parameters.Add(tradeTemplateInfo.Trade.Code);
            parameters.Add(tradeTemplateInfo.Trade.TenderRequired);
            parameters.Add(tradeTemplateInfo.Trade.Description);
            parameters.Add(tradeTemplateInfo.Trade.ScopeHeader);
            parameters.Add(tradeTemplateInfo.Trade.ScopeFooter);
            parameters.Add(tradeTemplateInfo.Trade.DisplayOrder);
            parameters.Add(tradeTemplateInfo.Trade.DaysFromPCD);
            parameters.Add(tradeTemplateInfo.IsStandard);
            parameters.Add(tradeTemplateInfo.CreatedDate);
            parameters.Add(tradeTemplateInfo.CreatedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeTemplate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Template in database. " + ex.Message);
            }
        }

        /// <summary>
        /// Adds a Trade Template to the database
        /// </summary>
        public int? AddTradeTemplate(TradeTemplateInfo tradeTemplateInfo)
        {
            int? tradeTemplateId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(tradeTemplateInfo);

            parameters.Add(GetId(tradeTemplateInfo.Trade.JobType));
            parameters.Add(tradeTemplateInfo.Trade.Name);
            parameters.Add(tradeTemplateInfo.Trade.Code);
            parameters.Add(tradeTemplateInfo.Trade.TenderRequired);
            parameters.Add(tradeTemplateInfo.Trade.Description);
            parameters.Add(tradeTemplateInfo.Trade.ScopeHeader);
            parameters.Add(tradeTemplateInfo.Trade.ScopeFooter);
            parameters.Add(tradeTemplateInfo.Trade.DaysFromPCD);
            parameters.Add(tradeTemplateInfo.IsStandard);
            parameters.Add(tradeTemplateInfo.CreatedDate);
            parameters.Add(tradeTemplateInfo.CreatedBy);

            try
            {
                tradeTemplateId = Data.DataProvider.GetInstance().AddTradeTemplate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Trade Template to database. " + ex.Message);
            }

            return tradeTemplateId;
        }

        /// <summary>
        /// Adds or updates a Trade Template
        /// </summary>
        public int? AddUpdateTradeTemplate(TradeTemplateInfo tradeTemplateInfo)
        {
            if (tradeTemplateInfo != null)
            {
                if (tradeTemplateInfo.Id != null)
                {
                    UpdateTradeTemplate(tradeTemplateInfo);
                    return tradeTemplateInfo.Id;
                }
                else
                {
                    return AddTradeTemplate(tradeTemplateInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Trade Template from persistent storage
        /// </summary>
        public void DeleteTradeTemplate(TradeTemplateInfo tradeTemplateInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteTrade(tradeTemplateInfo.Trade.Id);
                Data.DataProvider.GetInstance().DeleteTradeTemplate(tradeTemplateInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Trade Template from database");
            }
        }
#endregion

#region Trade Participations Methods
        /// <summary>
        /// Crate a Trade Participation from a dr and dictionaries
        /// </summary>
        public TradeParticipationInfo CreateTradeParticipation(IDataReader dr, Dictionary<int, SubContractorInfo> subContractorsDictionary, Dictionary<int, PeopleInfo> peopleDictionary)
        {
            TradeParticipationInfo tradeParticipationInfo = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["TradeParticipationId"]));

            tradeParticipationInfo.Type = Data.Utils.GetDBString(dr["TradeParticipationType"]);
            tradeParticipationInfo.InvitationDate = Data.Utils.GetDBDateTime(dr["InvitationDate"]);
            tradeParticipationInfo.QuoteDate = Data.Utils.GetDBDateTime(dr["QuoteDate"]);
            tradeParticipationInfo.QuoteDueDate = Data.Utils.GetDBDateTime(dr["QuoteDueDate"]);
            tradeParticipationInfo.QuoteFile = Data.Utils.GetDBString(dr["QuoteFile"]);
            tradeParticipationInfo.OpenDate = Data.Utils.GetDBDateTime(dr["OpenDate"]);
            tradeParticipationInfo.ReminderDate = Data.Utils.GetDBDateTime(dr["ReminderDate"]);
            tradeParticipationInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            tradeParticipationInfo.PulledOut = Data.Utils.GetDBBoolean(dr["PulledOut"]);
            tradeParticipationInfo.Rank = Data.Utils.GetDBInt32(dr["Rank"]);
            tradeParticipationInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);
            //#---
            tradeParticipationInfo.InternalComments= Data.Utils.GetDBString(dr["InternalComments"]);
            tradeParticipationInfo.safetyRisk= Data.Utils.GetDBString(dr["SafetyRating"]);
            tradeParticipationInfo.PaymentTerms = Data.Utils.GetDBString(dr["PaymentTerms"]);   //DS20230920

            //#---
            AssignAuditInfo(tradeParticipationInfo, dr);

            if (dr["TradeId"] != DBNull.Value)
            {
                tradeParticipationInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                tradeParticipationInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                if (dr["ProjectId"] != DBNull.Value)
                {
                    tradeParticipationInfo.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    tradeParticipationInfo.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                }
            }

            if (dr["ComparisonTradeParticipationId"] != DBNull.Value)
                tradeParticipationInfo.ComparisonParticipation = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["ComparisonTradeParticipationId"]));

            if (dr["SubContractorId"] != DBNull.Value)
                tradeParticipationInfo.SubContractor = SubContractorsController.GetInstance().CreateSubContractor(Data.Utils.GetDBInt32(dr["SubContractorId"]), subContractorsDictionary);
            
            if (dr["ContactPeopleId"] != DBNull.Value)
                tradeParticipationInfo.Contact = (ContactInfo)PeopleController.GetInstance().CreatePerson(Data.Utils.GetDBInt32(dr["ContactPeopleId"]), peopleDictionary);

            if (dr["TransmittalId"] != DBNull.Value)
                tradeParticipationInfo.Transmittal = new TransmittalInfo(Data.Utils.GetDBInt32(dr["TransmittalId"]));

            return tradeParticipationInfo;
        }

        /// <summary>
        /// Crate a Trade Participation from a dr
        /// </summary>
        public TradeParticipationInfo CreateTradeParticipation(IDataReader dr)
        {
            return CreateTradeParticipation(dr, null, null);
        }

        /// <summary>
        /// Get a Trade Participation from persistent storage
        /// </summary>
        public TradeParticipationInfo GetTradeParticipation(int? tradeParticipationId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeParticipation(tradeParticipationId);
                if (dr.Read())
                    return CreateTradeParticipation(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Participation from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get Quote Trade Participation from persistent storage
        /// </summary>
        public TradeParticipationInfo GetQuoteTradeParticipation(TradeParticipationInfo tradeParticipationInfo)
        {
            IDataReader dr = null;
            TradeParticipationInfo quoteTradeParticipationInfo =null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetQuoteTradeParticipation(tradeParticipationInfo.Id);

                if (dr.Read())
                    quoteTradeParticipationInfo = GetTradeParticipation(Data.Utils.GetDBInt32(dr["TradeParticipationId"]));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Quote Trade Participation from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return quoteTradeParticipationInfo;
        }

        /// <summary>
        /// Get Quote Trade Participation from persistent storage
        /// </summary>
        public TradeParticipationInfo GetQuoteTradeParticipationWithItems(TradeParticipationInfo tradeParticipationInfo)
        {
            TradeParticipationInfo quoteTradeParticipationInfo = GetQuoteTradeParticipation(tradeParticipationInfo);

            if (quoteTradeParticipationInfo != null)
                quoteTradeParticipationInfo.ParticipationItems = GetParticipationItems(quoteTradeParticipationInfo);

            return quoteTradeParticipationInfo;
        }

        /// <summary>
        /// Get a Trade Participation with subcontractor contacts
        /// </summary>
        public TradeParticipationInfo GetDeepTradeParticipationWithContacts(int? tradeParticipationId)
        {
            TradeParticipationInfo tradeParticipationInfo = GetDeepTradeParticipation(tradeParticipationId);

            if (tradeParticipationInfo.SubContractor != null)
                tradeParticipationInfo.SubContractor.Contacts = SubContractorsController.GetInstance().GetContacts(tradeParticipationInfo.SubContractor);

            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get a Trade Participation with transmittal
        /// </summary>
        public TradeParticipationInfo GetDeepTradeParticipationWithTransmittal(int? tradeParticipationId)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            TradeParticipationInfo tradeParticipationInfo = GetDeepTradeParticipation(tradeParticipationId);

            if (tradeParticipationInfo.Transmittal != null)
                tradeParticipationInfo.Transmittal = projectsController.GetTransmittalWithRevisions(tradeParticipationInfo.Transmittal.Id);

            return tradeParticipationInfo;
        }
        
        /// <summary>
        /// Get a Trade Participation with the trade
        /// </summary>
        public TradeParticipationInfo GetTradeParticipationWithTradeAndProject(int? tradeParticipationId, Boolean getDeep)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            TradeParticipationInfo tradeParticipationInfo = getDeep ? GetDeepTradeParticipationWithQuoteAndTransmittal(tradeParticipationId) : GetTradeParticipationWithQuoteAndTransmittal(tradeParticipationId);

            if (tradeParticipationInfo != null)
            {
                tradeParticipationInfo.Trade = GetTradeWithDrawingsAndItems(tradeParticipationInfo.Trade.Id);
                tradeParticipationInfo.Trade.Project = tradeParticipationInfo.IsActive ? projectsController.GetProjectWithDrawingsActive(tradeParticipationInfo.Trade.Project.Id) : projectsController.GetProjectWithDrawingsProposal(tradeParticipationInfo.Trade.Project.Id);

                if (tradeParticipationInfo.Trade.ContractsAdministrator != null)
                    tradeParticipationInfo.Trade.Project.ContractsAdministrator = tradeParticipationInfo.Trade.ContractsAdministrator;

                if (tradeParticipationInfo.Trade.ProjectManager != null)
                    tradeParticipationInfo.Trade.Project.ProjectManager = tradeParticipationInfo.Trade.ProjectManager;

                tradeParticipationInfo.Trade.Participations = new List<TradeParticipationInfo>();
                tradeParticipationInfo.Trade.Participations.Add(tradeParticipationInfo);

                tradeParticipationInfo.Trade.Project.Trades = new List<TradeInfo>();
                tradeParticipationInfo.Trade.Project.Trades.Add(tradeParticipationInfo.Trade);
            }

            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get a Trade Participation with the trade
        /// </summary>
        public TradeParticipationInfo GetTradeParticipationWithTradeAndProject(int? tradeParticipationId)
        {
            return GetTradeParticipationWithTradeAndProject(tradeParticipationId, false);
        }

        /// <summary>
        /// Get deep a Trade Participation with the trade
        /// </summary>
        public TradeParticipationInfo GetDeepTradeParticipationWithTradeAndProject(int? tradeParticipationId)
        {
            return GetTradeParticipationWithTradeAndProject(tradeParticipationId, true);
        }

        /// <summary>
        /// Get a Trade Participation with transmittal from persistent storage.
        /// </summary>
        public TradeParticipationInfo GetTradeParticipationWithTransmittal(int? tradeParticipationId)
        {
            TradeParticipationInfo tradeParticipationInfo = GetTradeParticipation(tradeParticipationId);

            if (tradeParticipationInfo.Transmittal != null)
                tradeParticipationInfo.Transmittal = ProjectsController.GetInstance().GetTransmittalWithRevisions(tradeParticipationInfo.Transmittal.Id);

            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get a Trade Participation with quote and transmittal from persistent storage.
        /// </summary>
        public TradeParticipationInfo GetTradeParticipationWithQuoteAndTransmittal(int? tradeParticipationId)
        {
            TradeParticipationInfo tradeParticipationInfo = GetTradeParticipation(tradeParticipationId);
            tradeParticipationInfo.QuoteParticipation = GetQuoteTradeParticipationWithItems(tradeParticipationInfo);

            if (tradeParticipationInfo.Transmittal != null)
                tradeParticipationInfo.Transmittal = ProjectsController.GetInstance().GetTransmittalWithRevisions(tradeParticipationInfo.Transmittal.Id);

            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get a Trade Participation with Participation Items from persistent storage.
        /// </summary>
        public TradeParticipationInfo GetDeepTradeParticipation(int? tradeParticipationId)
        {            
            TradeParticipationInfo tradeParticipationInfo = GetTradeParticipation(tradeParticipationId);
            tradeParticipationInfo.ParticipationItems = GetParticipationItems(tradeParticipationInfo);
            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get a Trade Participation with Participation Items from persistent storage.
        /// </summary>
        public TradeParticipationInfo GetDeepTradeParticipation(IDataReader dr)
        {
            TradeParticipationInfo tradeParticipationInfo = CreateTradeParticipation(dr);
            tradeParticipationInfo.ParticipationItems = GetParticipationItems(tradeParticipationInfo);
            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get a Trade Participation with Quote and Participation Items from persistent storage.
        /// </summary>
        public TradeParticipationInfo GetDeepTradeParticipationWithQuoteAndTransmittal(int? tradeParticipationId)
        {
            TradeParticipationInfo tradeParticipationInfo = GetTradeParticipation(tradeParticipationId);
            tradeParticipationInfo.ParticipationItems = GetParticipationItems(tradeParticipationInfo);
            tradeParticipationInfo.QuoteParticipation = GetQuoteTradeParticipationWithItems(tradeParticipationInfo);

            if (tradeParticipationInfo.Transmittal != null)
                tradeParticipationInfo.Transmittal = ProjectsController.GetInstance().GetTransmittalWithRevisions(tradeParticipationInfo.Transmittal.Id);

            if (tradeParticipationInfo.QuoteParticipation != null && tradeParticipationInfo.QuoteParticipation.Transmittal != null)
                tradeParticipationInfo.QuoteParticipation.Transmittal = ProjectsController.GetInstance().GetTransmittalWithRevisions(tradeParticipationInfo.QuoteParticipation.Transmittal.Id);
            
            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get the Trade Participations for the specified Trade and type
        /// </summary>
        public List<TradeParticipationInfo> GetTradeParticipations(TradeInfo tradeInfo, String participationType)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();
            TradeParticipationInfo tradeParticipationInfo;
            List<TradeParticipationInfo> tradeParticipationInfoList = new List<TradeParticipationInfo>();
            List<TradeParticipationInfo> tradeParticipationInfoQuoteList = new List<TradeParticipationInfo>();
            Dictionary<Int32, TradeParticipationInfo> tradeParticipationInfoDictionary = new Dictionary<int,TradeParticipationInfo>();

            parameters.Add(participationType);
            parameters.Add(tradeInfo.Id);

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeParticipations(parameters.ToArray());
                while (dr.Read())
                {
                    tradeParticipationInfo = CreateTradeParticipation(dr);

                    if (tradeParticipationInfo.ComparisonParticipation == null)
                    {
                        tradeParticipationInfoList.Add(tradeParticipationInfo);
                        tradeParticipationInfoDictionary.Add((Int32)tradeParticipationInfo.Id, tradeParticipationInfo);
                    }
                    else
                        tradeParticipationInfoQuoteList.Add(tradeParticipationInfo);
                }

                foreach (TradeParticipationInfo tradeParticipation in tradeParticipationInfoQuoteList)
                    tradeParticipationInfoDictionary[(Int32)tradeParticipation.ComparisonParticipation.Id].QuoteParticipation = tradeParticipation;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Participations from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeParticipationInfoList;
        }

        public List<TradeParticipationInfo> GetTradeParticipationsActive(TradeInfo tradeInfo)
        {
            return GetTradeParticipations(tradeInfo, Info.TypeActive);
        }

        public List<TradeParticipationInfo> GetTradeParticipationsProposal(TradeInfo tradeInfo)
        {
            return GetTradeParticipations(tradeInfo, Info.TypeProposal);
        }

        public List<TradeParticipationInfo> GetTradeParticipations(TradeInfo tradeInfo)
        {
            return GetTradeParticipations(tradeInfo, null);
        }

        /// <summary>
        /// Get Deep the Trade Participations for the specified Trade
        /// </summary>
        public List<TradeParticipationInfo> GetDeepTradeParticipations(TradeInfo tradeInfo, String participationType)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();
            TradeParticipationInfo tradeParticipationInfo;
            List<TradeParticipationInfo> tradeParticipationInfoList = new List<TradeParticipationInfo>();
            List<TradeParticipationInfo> tradeParticipationInfoQuoteList = new List<TradeParticipationInfo>();
            Dictionary<Int32, TradeParticipationInfo> tradeParticipationInfoDictionary = new Dictionary<int, TradeParticipationInfo>();

            parameters.Add(participationType);
            parameters.Add(tradeInfo.Id);

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeParticipations(parameters.ToArray());
                while (dr.Read())
                {
                    tradeParticipationInfo = GetDeepTradeParticipation(dr);

                    if (tradeParticipationInfo.ComparisonParticipation == null)
                    {
                        tradeParticipationInfoList.Add(tradeParticipationInfo);
                        tradeParticipationInfoDictionary.Add((Int32)tradeParticipationInfo.Id, tradeParticipationInfo);
                    }
                    else
                        tradeParticipationInfoQuoteList.Add(tradeParticipationInfo);
                }

                foreach (TradeParticipationInfo tradeParticipation in tradeParticipationInfoQuoteList)
                    tradeParticipationInfoDictionary[(Int32)tradeParticipation.ComparisonParticipation.Id].QuoteParticipation = tradeParticipation;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Deep Trade Participations from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeParticipationInfoList;
        }

        public List<TradeParticipationInfo> GetDeepTradeParticipationsActive(TradeInfo tradeInfo)
        {
            return GetDeepTradeParticipations(tradeInfo, Info.TypeActive);
        }

        public List<TradeParticipationInfo> GetDeepTradeParticipationsProposal(TradeInfo tradeInfo)
        {
            return GetDeepTradeParticipations(tradeInfo, Info.TypeProposal);
        }

        public List<TradeParticipationInfo> GetDeepTradeParticipations(TradeInfo tradeInfo)
        {
            return GetDeepTradeParticipations(tradeInfo, null);
        }

        /// <summary>
        /// Updates a Trade Participation in the database
        /// </summary>
        public void UpdateTradeParticipation(TradeParticipationInfo tradeParticipationInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(tradeParticipationInfo);

            parameters.Add(tradeParticipationInfo.Id);
            parameters.Add(GetId(tradeParticipationInfo.SubContractor));
            parameters.Add(GetId(tradeParticipationInfo.Contact));
            parameters.Add(GetId(tradeParticipationInfo.Transmittal));
            parameters.Add(tradeParticipationInfo.InvitationDate);
            parameters.Add(tradeParticipationInfo.QuoteDate);
            parameters.Add(tradeParticipationInfo.QuoteDueDate);
            parameters.Add(tradeParticipationInfo.QuoteFile);
            parameters.Add(tradeParticipationInfo.OpenDate);
            parameters.Add(tradeParticipationInfo.ReminderDate);
            parameters.Add(tradeParticipationInfo.Amount);
            parameters.Add(tradeParticipationInfo.PulledOut);
            parameters.Add(tradeParticipationInfo.Rank);
            parameters.Add(tradeParticipationInfo.Comments);
            //#----
            parameters.Add(tradeParticipationInfo.InternalComments);
            parameters.Add(tradeParticipationInfo.safetyRisk);
            parameters.Add(tradeParticipationInfo.PaymentTerms);  //DS20230921
            //#----
            parameters.Add(tradeParticipationInfo.ModifiedDate);
            parameters.Add(tradeParticipationInfo.ModifiedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    UpdateTradeProcess(tradeParticipationInfo.Trade);
                    Data.DataProvider.GetInstance().UpdateTradeParticipation(parameters.ToArray());

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Participation in database");
            }
        }

        /// <summary>
        /// Updates a Trade Participation open date in the database
        /// </summary>
        public void UpdateTradeParticipationOpenDate(TradeParticipationInfo tradeParticipationInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeParticipationInfo.Id);
            parameters.Add(tradeParticipationInfo.OpenDate);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeParticipationOpenDate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Participation Open Date in database");
            }
        }

        /// <summary>
        /// Updates a Trade Participation quote file in the database
        /// </summary>
        public void UpdateTradeParticipationQuoteFile(TradeParticipationInfo tradeParticipationInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeParticipationInfo.Id);
            parameters.Add(tradeParticipationInfo.QuoteFile);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeParticipationQuoteFile(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Participation Quote File in database");
            }
        }

        /// <summary>
        /// Updates a Trade Participation reminder date in the database
        /// </summary>
        public void UpdateTradeParticipationReminderDate(TradeParticipationInfo tradeParticipationInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(tradeParticipationInfo.Id);
            parameters.Add(tradeParticipationInfo.ReminderDate);

            try
            {
                Data.DataProvider.GetInstance().UpdateTradeParticipationReminderDate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Participation Reminder Date in database");
            }
        }

        /// <summary>
        /// Updates all the Trade Participation Items
        /// </summary>
        public void UpdateTradeParticipationItems(TradeParticipationInfo tradeParticipationInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (tradeParticipationInfo.ParticipationItems != null)
                        foreach (ParticipationItemInfo participationItemInfo in tradeParticipationInfo.ParticipationItems)
                            UpdateParticipationItem(participationItemInfo);

                    UpdateTradeProcess(tradeParticipationInfo.Trade);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Participation items in database");
            }
        }

        /// <summary>
        /// Updates a Trade Participation and its Items
        /// </summary>
        public void UpdateTradeParticipationWithItems(TradeParticipationInfo tradeParticipationInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    UpdateTradeParticipation(tradeParticipationInfo);
                    UpdateTradeParticipationItems(tradeParticipationInfo);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Trade Participation with items in database");
            }
        }
        
        /// <summary>
        /// Adds a Trade Participation to the database
        /// </summary>
        public int? AddTradeParticipation(TradeParticipationInfo tradeParticipationInfo, Boolean includeItems)
        {
            int? tradeParticipationId = null;
            ParticipationItemInfo participationItemInfo;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(tradeParticipationInfo);

            if (tradeParticipationInfo.SubContractor != null)
                if (tradeParticipationInfo.SubContractor.Contacts != null)
                    if (tradeParticipationInfo.SubContractor.Contacts.Count == 1)
                        tradeParticipationInfo.Contact = tradeParticipationInfo.SubContractor.Contacts[0];

            parameters.Add(tradeParticipationInfo.Type);
            parameters.Add(GetId(tradeParticipationInfo.Trade));
            parameters.Add(GetId(tradeParticipationInfo.ComparisonParticipation));
            parameters.Add(GetId(tradeParticipationInfo.SubContractor));
            parameters.Add(GetId(tradeParticipationInfo.Contact));
            parameters.Add(GetId(tradeParticipationInfo.Transmittal));
            parameters.Add(tradeParticipationInfo.InvitationDate);
            parameters.Add(tradeParticipationInfo.QuoteDate);
            parameters.Add(tradeParticipationInfo.QuoteDueDate);
            parameters.Add(tradeParticipationInfo.QuoteFile);
            parameters.Add(tradeParticipationInfo.OpenDate);
            parameters.Add(tradeParticipationInfo.ReminderDate);
            parameters.Add(tradeParticipationInfo.Amount);
            parameters.Add(tradeParticipationInfo.PulledOut);
            parameters.Add(tradeParticipationInfo.Rank);
            parameters.Add(tradeParticipationInfo.Comments);
            //#----
            parameters.Add(tradeParticipationInfo.InternalComments);
            parameters.Add(tradeParticipationInfo.safetyRisk);
            //#---
            parameters.Add(tradeParticipationInfo.CreatedDate);
            parameters.Add(tradeParticipationInfo.CreatedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    tradeParticipationId = Data.DataProvider.GetInstance().AddTradeParticipation(parameters.ToArray());
                    tradeParticipationInfo.Id = tradeParticipationId;

                    if (includeItems && tradeParticipationInfo.Trade.ItemCategories != null)
                        foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeParticipationInfo.Trade.ItemCategories)
                        {
                            foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                            {
                                participationItemInfo = new ParticipationItemInfo();
                                SetCreateInfo(participationItemInfo);
                                participationItemInfo.TradeItem = tradeItemInfo;
                                participationItemInfo.TradeParticipation = tradeParticipationInfo;
                                AddParticipationItem(participationItemInfo);
                            }
                        }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Trade Participation to database");
            }

            return tradeParticipationId;
        }

        /// <summary>
        /// Adds a Trade Participation to the database including participation items
        /// </summary>
        public int? AddTradeParticipation(TradeParticipationInfo tradeParticipationInfo)
        {
            return AddTradeParticipation(tradeParticipationInfo, true);
        }

        /// <summary>
        /// Adds a Trade Participation to the database no including participation items
        /// </summary>
        public int? AddTradeParticipationWithNoItems(TradeParticipationInfo tradeParticipationInfo)
        {
            return AddTradeParticipation(tradeParticipationInfo, false);
        }

        /// <summary>
        /// Adds or updates a Trade Participation
        /// </summary>
        public int? AddUpdateTradeParticipation(TradeParticipationInfo tradeParticipationInfo)
        {
            if (tradeParticipationInfo != null)
            {
                if (tradeParticipationInfo.Id != null)
                {
                    UpdateTradeParticipation(tradeParticipationInfo);
                    return tradeParticipationInfo.Id;
                }
                else
                {
                    return AddTradeParticipation(tradeParticipationInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Trade Participation quote file
        /// </summary>
        public void DeleteTradeParticipationQuoteFile(TradeParticipationInfo tradeParticipationInfo)
        {
            try
            {
                if (tradeParticipationInfo.QuoteFile != null)
                {
                    FileInfo fileInfo = new FileInfo(UI.Utils.PathQuotesFile(tradeParticipationInfo.Trade.Project.AttachmentsFolder, tradeParticipationInfo.QuoteFileName));

                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Deleting Trade Participation Quote File");
            }
        }

        /// <summary>
        /// Remove a Trade Participation from persistent storage
        /// </summary>
        public void DeleteTradeParticipation(TradeParticipationInfo tradeParticipationInfo)
        {            
            try
            {
                Data.DataProvider.GetInstance().DeleteTradeParticipation(tradeParticipationInfo.Id);
                DeleteTradeParticipationQuoteFile(tradeParticipationInfo);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Trade Participation from database");
            }
        }

        /// <summary>
        /// Get a participation for subcontractor
        /// </summary>
        public TradeParticipationInfo GetParticipationForSubcontractor(int? tradeParticipationId, PeopleController peopleController)
        {
            IDataReader dr = null;
            TradeParticipationInfo tradeParticipationInfo = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradeParticipationsForSubContractor(tradeParticipationId);
                if (dr.Read())
                {
                    tradeParticipationInfo = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["TradeParticipationId"]));
                    tradeParticipationInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeParticipationInfo.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    tradeParticipationInfo.Trade.Project.BusinessUnit = new BusinessUnitInfo(Data.Utils.GetDBInt32(dr["BusinessUnitId"]));

                    tradeParticipationInfo.Type = Data.Utils.GetDBString(dr["TradeParticipationType"]);
                    tradeParticipationInfo.PulledOut = Data.Utils.GetDBBoolean(dr["PulledOut"]);
                    tradeParticipationInfo.InvitationDate = Data.Utils.GetDBDateTime(dr["InvitationDate"]);
                    tradeParticipationInfo.QuoteDate = Data.Utils.GetDBDateTime(dr["QuoteDate"]);
                    tradeParticipationInfo.QuoteDueDate = Data.Utils.GetDBDateTime(dr["QuoteDueDate"]);
                    tradeParticipationInfo.QuoteFile = Data.Utils.GetDBString(dr["QuoteFile"]);
                    tradeParticipationInfo.OpenDate = Data.Utils.GetDBDateTime(dr["OpenDate"]);
                    tradeParticipationInfo.Rank = Data.Utils.GetDBInt32(dr["Rank"]);

                    tradeParticipationInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);
                    tradeParticipationInfo.Trade.ComparisonApprovalDate = Data.Utils.GetDBDateTime(dr["ComparisonApprovalDate"]);

                    tradeParticipationInfo.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    tradeParticipationInfo.Trade.Project.Number = Data.Utils.GetDBString(dr["ProjectNumber"]);
                    tradeParticipationInfo.Trade.Project.Year = Data.Utils.GetDBString(dr["ProjectYear"]);

                    tradeParticipationInfo.Trade.Project.BusinessUnit.ProjectNumberFormat = Data.Utils.GetDBString(dr["ProjectNumberFormat"]);

                    if (dr["ContractId"] != DBNull.Value)
                    {
                        tradeParticipationInfo.Trade.Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));
                        tradeParticipationInfo.Trade.Contract.ApprovalDate = Data.Utils.GetDBDateTime(dr["ContractApprovalDate"]);
                    }

                    if (dr["TradeCAPeopleId"] != DBNull.Value)
                        tradeParticipationInfo.Trade.Project.ContractsAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["TradeCAPeopleId"]));
                    else if (dr["ProjectCAPeopleId"] != DBNull.Value)
                        tradeParticipationInfo.Trade.Project.ContractsAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["ProjectCAPeopleId"]));

                    if (dr["TradePMPeopleID"] != DBNull.Value)
                        tradeParticipationInfo.Trade.Project.ContractsAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["TradePMPeopleID"]));
                    else if (dr["ProjectPMPeopleID"] != DBNull.Value)
                        tradeParticipationInfo.Trade.Project.ContractsAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["ProjectPMPeopleID"]));

                    tradeParticipationInfo.QuoteParticipation = GetQuoteTradeParticipation(tradeParticipationInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Participation for subcontractor from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeParticipationInfo;
        }

        /// <summary>
        /// Get a list of participations for an specified subcontractor
        /// </summary>
        public List<TradeParticipationInfo> SearchParticipations(int startRowIndex, int maximumRows, String orderBy, String strSubContractorId, String strSearch, String strOnlyActive)
        {
            Int32 subContractorId = Int32.Parse(strSubContractorId);
            Boolean onlyActive = strOnlyActive == ProjectInfo.StatusActive;
            PeopleController peopleController = PeopleController.GetInstance();
            TradeParticipationInfo tradeParticipationInfo = null;
            List<TradeParticipationInfo> tradeParticipationInfoList = new List<TradeParticipationInfo>();
            HashSet<int?> tradeIdHashSet = new HashSet<int?>();
            List<Object> parameters = new List<Object>();
            List<Object> parameters1 = new List<Object>();
            IDataReader dr = null;
            IDataReader dr1 = null;
            StringBuilder stringBuilder = new StringBuilder(250);

            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            parameters.Add(UI.Utils.GetFormString(orderBy));
            parameters.Add(subContractorId);
            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(onlyActive);

            try
            {
                dr = Data.DataProvider.GetInstance().SearchTradeParticipationsBySubContractor(parameters.ToArray());
                while (dr.Read())
                {
                    tradeParticipationInfo = GetParticipationForSubcontractor(Data.Utils.GetDBInt32(dr["TradeParticipationId"]), peopleController);

                    if (tradeParticipationInfo != null)
                    {
                        tradeParticipationInfoList.Add(tradeParticipationInfo);

                        if (tradeParticipationInfo.Type == Info.TypeProposal)
                            stringBuilder.Append(tradeParticipationInfo.Trade.IdStr).Append(",");
                    }
                }

                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Length = stringBuilder.Length - 1;

                    parameters1.Add(subContractorId);
                    parameters1.Add(stringBuilder.ToString());

                    dr1 = Data.DataProvider.GetInstance().GetActiveTradeIds(parameters1.ToArray());

                    while (dr1.Read())
                        tradeIdHashSet.Add(Data.Utils.GetDBInt32(dr1["TradeId"]));
                }

                foreach (TradeParticipationInfo tradeParticipation in tradeParticipationInfoList)
                    tradeParticipation.HasActiveParticipation = tradeParticipation.Type == Info.TypeProposal && tradeIdHashSet.Contains(tradeParticipation.Trade.Id);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Participations for subcontractor from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();

                if (dr1 != null)
                    dr1.Close();
            }

            return tradeParticipationInfoList;
        }

        /// <summary>
        /// Get a list of participations for an specified subcontractor
        /// </summary>
        public int SearchParticipationsCount(String strSubContractorId, String strSearch, String strOnlyActive)
        {
            List<Object> parameters = new List<Object>();
            Int32 subContractorId = Int32.Parse(strSubContractorId);
            Boolean onlyActive = strOnlyActive == ProjectInfo.StatusActive;

            parameters.Add(subContractorId);
            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(onlyActive);

            try
            {
                return Data.DataProvider.GetInstance().SearchTradeParticipationsBySubContractorCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Participations for subcontractor count from database.");
            }
        }

        /// <summary>
        /// Sends a submision notification to project manajer
        /// </summary>
        public void SendSubmisionNotification(TradeParticipationInfo tradeParticipation)
        {
            ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();
             

            Byte[] attachment =GenerateQuoteReport(tradeParticipation);


            String attachmentName = String.Format("Quote_{0}_{1}_{2}.pdf", tradeParticipation.ProjectName, tradeParticipation.TradeName, tradeParticipation.SubcontractorShortName);

              


            String subject = String.Format("Submission - {0} - {1} - {2}", tradeParticipation.ProjectName, tradeParticipation.TradeName, tradeParticipation.SubContractor.ShortName);

                String message = "" +
                   "The following quote has been submitted." + "<br />" +
                   "<br />" +
                   "Project: <b>" + UI.Utils.SetFormString(tradeParticipation.ProjectName) + "</b><br />" +
                   "Trade: <b>" + UI.Utils.SetFormString(tradeParticipation.TradeName) + "</b><br />" +
                   "Subcontractor: <b>" + UI.Utils.SetFormString(tradeParticipation.SubContractor.Name) + "</b><br />" +
                   "Date/Time: <b>" + UI.Utils.SetFormDateTime(tradeParticipation.QuoteDate) + "</b><br />" +
                   "Quote amount: <b>" + UI.Utils.SetFormDecimal(GetQuoteTotal(tradeParticipation.QuoteParticipation)) + "</b><br />" +
                   "Comments: <b>" + UI.Utils.SetFormString(tradeParticipation.QuoteParticipation.Comments) + "</b><br />" +
                   "<br />" +
                   "<i>SOS++</i><br />";

          
            if (tradeParticipation.Trade.Project.ContractsAdministrator != null)
                    Utils.SendEmail(tradeParticipation.Trade.Project.ContractsAdministrator, subject, message, attachment, attachmentName);

                if (tradeParticipation.Trade.Project.ProjectManager != null)
                    Utils.SendEmail(tradeParticipation.Trade.Project.ProjectManager, subject, message, attachment, attachmentName);

                if (contactInfo != null)
                    Utils.SendEmail(contactInfo, subject, message, attachment, attachmentName);
           
        }


      




        /// <summary>
        /// Submits a trade participation
        /// </summary>
        public void SubmitTradeParticipation(TradeParticipationInfo tradeParticipation)
        {
            XmlDocument xmlDocument = CheckParticipation(tradeParticipation.QuoteParticipation);

            if (xmlDocument.DocumentElement != null)
                throw new Exception("Check trade participation for errors or missing fields.");

            if (tradeParticipation.QuoteDate != null)
                throw new Exception("The trade participation has been previously submitted.");

            tradeParticipation.QuoteDate = DateTime.Now;

            UpdateTradeParticipation(tradeParticipation);

            SendSubmisionNotification(tradeParticipation);
           
        }








        /// <summary>
        /// Submits a subbies questions
        /// </summary>
        public void SubmitQuestion(String question)
        {
            ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();

            String subject = String.Format("Online question {0}", contactInfo.SubContractorName);

            String message = "" +
               "Online question received." + "<br />" +
               "<br />" +
               "Compnay: <b>" + UI.Utils.SetFormString(contactInfo.SubContractorName) + "</b><br />" +
               "User: <b>" + UI.Utils.SetFormString(contactInfo.Name) + "</b><br />" +
               "Phone: <b>" + UI.Utils.SetFormString(contactInfo.Phone) + "</b><br />" +
               "Email: <b>" + UI.Utils.SetFormString(contactInfo.Email) + "</b><br />" +
               "Question: <b>" + UI.Utils.SetFormString(question) + "</b><br />" +
               "<br />" +
               "<i>SOS++</i><br />";

            Utils.SendEmailToSupport(contactInfo, subject, message);
        }

        /// <summary>
        /// Copy trade participation Amount, comments and items
        /// </summary>
        private void CopyTradeParticipationQuote(TradeParticipationInfo sourceTradeParticipation, TradeParticipationInfo targetTradeParticipation, ref List<String> notFoudList, Boolean matchByItemId)
        {
            ParticipationItemInfo participationItemInfo;

            if (sourceTradeParticipation == null)
                throw new Exception("source participation not found.");

            if (!targetTradeParticipation.IsEmpty)
                throw new Exception("Target participation is not empty.");

            targetTradeParticipation.Amount = sourceTradeParticipation.Amount;
            targetTradeParticipation.Comments = sourceTradeParticipation.Comments;

            if (targetTradeParticipation.ParticipationItems != null && targetTradeParticipation.ParticipationItems != null)
            {
                foreach (ParticipationItemInfo sourceParticipationItemInfo in sourceTradeParticipation.ParticipationItems)
                {
                    participationItemInfo = null;

                    foreach (ParticipationItemInfo targetParticipationItemInfo in targetTradeParticipation.ParticipationItems)
                    {
                        if (matchByItemId)
                        {
                            if (targetParticipationItemInfo.TradeItem.Equals(sourceParticipationItemInfo.TradeItem))
                            {
                                participationItemInfo = targetParticipationItemInfo;
                                break;
                            }
                        }
                        else
                        {
                            if (targetParticipationItemInfo.TradeItem.TradeItemCategory.Name == sourceParticipationItemInfo.TradeItem.TradeItemCategory.Name && targetParticipationItemInfo.TradeItem.Name == sourceParticipationItemInfo.TradeItem.Name && targetParticipationItemInfo.TradeItem.Units == sourceParticipationItemInfo.TradeItem.Units)
                            {
                                participationItemInfo = targetParticipationItemInfo;
                                break;
                            }
                        }
                    }

                    if (participationItemInfo != null)
                    {
                        participationItemInfo.Quantity = sourceParticipationItemInfo.Quantity;
                        participationItemInfo.IsIncluded = sourceParticipationItemInfo.IsIncluded;
                        participationItemInfo.Amount = sourceParticipationItemInfo.Amount;
                        participationItemInfo.Confirmed = sourceParticipationItemInfo.Confirmed;
                        participationItemInfo.Notes = sourceParticipationItemInfo.Notes;
                    }
                    else
                        notFoudList.Add(sourceParticipationItemInfo.TradeItem.TradeItemCategory.ShortDescription + " / " + sourceParticipationItemInfo.TradeItem.Name + " (" + sourceParticipationItemInfo.TradeItem.Units + ")");
                }

                UpdateTradeParticipationWithItems(targetTradeParticipation);
            }
        }

        /// <summary>
        /// Copy trade participation Amount, comments and items matching items by category name, item name and item units
        /// </summary>
        public void CopyTradeParticipationQuoteToComparison(TradeParticipationInfo tradeParticipationInfo, ref List<String> notFoudList)
        {
            CopyTradeParticipationQuote(tradeParticipationInfo.QuoteParticipation, tradeParticipationInfo, ref notFoudList, true);
        }

        /// <summary>
        /// Copy trade participation Amount, comments and items matching items by category name, item name and item units
        /// </summary>
        public void CopyTradeParticipationQuote(TradeParticipationInfo sourceTradeParticipation, TradeParticipationInfo targetTradeParticipation, ref List<String> notFoudList)
        {
            CopyTradeParticipationQuote(sourceTradeParticipation, targetTradeParticipation, ref notFoudList, false);
        }

        /// <summary>
        /// Copies a participation transmittal from proposal to active
        /// </summary>
        public TransmittalInfo CopyParticipationTransmittalToActive(ProjectInfo project, TransmittalInfo sourceTransmittal, ref List<String> notFoudList)
        {
            TransmittalRevisionInfo transmittalRevisionInfo;
            List<DrawingInfo> drawingsActive = project.DrawingsActive;
            TransmittalInfo targetTransmittal = new TransmittalInfo();
            targetTransmittal.Type = Info.TypeActive;
            targetTransmittal.TransmittalRevisions = new List<TransmittalRevisionInfo>();

            if (sourceTransmittal.TransmittalRevisions != null && drawingsActive != null)
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (TransmittalRevisionInfo sourceTransmittalRevision in sourceTransmittal.TransmittalRevisions)
                    {
                        transmittalRevisionInfo = new TransmittalRevisionInfo();

                        foreach (DrawingInfo drawing in drawingsActive)
                            if (drawing.Type.Equals(sourceTransmittalRevision.DrawingType) && drawing.Name.Equals(sourceTransmittalRevision.DrawingName) && drawing.DrawingRevisions != null)
                                foreach (DrawingRevisionInfo drawingRevision in drawing.DrawingRevisions)
                                    if (drawingRevision.Number.Equals(sourceTransmittalRevision.RevisionName))
                                        transmittalRevisionInfo.Revision = drawingRevision;

                        if (transmittalRevisionInfo.Revision != null)
                        {
                            transmittalRevisionInfo.NumCopies = 1;
                            targetTransmittal.TransmittalRevisions.Add(transmittalRevisionInfo);
                        }
                        else
                            notFoudList.Add(sourceTransmittalRevision.DrawingType.Name + " - " + sourceTransmittalRevision.DrawingName + " - " + sourceTransmittalRevision.RevisionName);
                    }

                    targetTransmittal.Id = ProjectsController.GetInstance().AddTransmittal(targetTransmittal);

                    scope.Complete();
                }

            return targetTransmittal;
        }

        /// <summary>
        /// Copies a trade participation from proposal to active
        /// </summary>
        public XmlDocument CopyTradeParticipationToActive(int? tradeParticipationId)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElement;

            List<String> notFoundListTransmittal = new List<string>();
            List<String> notFoundListTransmittalQuote = new List<string>();
            List<String> notFoundListItems = new List<string>();
            List<String> notFoundListItemsQuote = new List<string>();

            ProjectsController projectsController = ProjectsController.GetInstance();
            TradeParticipationInfo sourceTradeParticipation = GetDeepTradeParticipationWithQuoteAndTransmittal(tradeParticipationId);
            sourceTradeParticipation.Trade = GetTradeWithItems(sourceTradeParticipation.Trade.Id);
            ProjectInfo projectInfo = projectsController.GetProjectWithDrawingsActive(sourceTradeParticipation.Trade.Project.Id);
            TradeParticipationInfo targetTradeParticipation = new TradeParticipationInfo();

            targetTradeParticipation.Type = Info.TypeActive;

            targetTradeParticipation.Trade = sourceTradeParticipation.Trade;
            targetTradeParticipation.SubContractor = sourceTradeParticipation.SubContractor;
            targetTradeParticipation.Contact = sourceTradeParticipation.Contact;
            targetTradeParticipation.InvitationDate = sourceTradeParticipation.InvitationDate;
            targetTradeParticipation.QuoteDate = sourceTradeParticipation.QuoteDate;
            targetTradeParticipation.QuoteDueDate = sourceTradeParticipation.QuoteDueDate;
            targetTradeParticipation.QuoteFile = sourceTradeParticipation.QuoteFile;
            targetTradeParticipation.OpenDate = sourceTradeParticipation.OpenDate;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                if (sourceTradeParticipation.Transmittal != null)
                    targetTradeParticipation.Transmittal = CopyParticipationTransmittalToActive(projectInfo, sourceTradeParticipation.Transmittal, ref notFoundListTransmittal);

                targetTradeParticipation.Id = AddTradeParticipation(targetTradeParticipation);
                targetTradeParticipation.ParticipationItems = GetParticipationItems(targetTradeParticipation);

                CopyTradeParticipationQuote(sourceTradeParticipation, targetTradeParticipation, ref notFoundListItems);

                if (sourceTradeParticipation.QuoteParticipation != null)
                {
                    targetTradeParticipation.QuoteParticipation = new TradeParticipationInfo();
                    targetTradeParticipation.QuoteParticipation.Type = targetTradeParticipation.Type;
                    targetTradeParticipation.QuoteParticipation.Trade = targetTradeParticipation.Trade;
                    targetTradeParticipation.QuoteParticipation.ComparisonParticipation = targetTradeParticipation;
                    targetTradeParticipation.QuoteParticipation.Id = AddTradeParticipation(targetTradeParticipation.QuoteParticipation);
                    targetTradeParticipation.QuoteParticipation.ParticipationItems = GetParticipationItems(targetTradeParticipation.QuoteParticipation);

                    if (sourceTradeParticipation.QuoteParticipation.Transmittal != null)
                        targetTradeParticipation.QuoteParticipation.Transmittal = CopyParticipationTransmittalToActive(projectInfo, sourceTradeParticipation.QuoteParticipation.Transmittal, ref notFoundListTransmittalQuote);

                    CopyTradeParticipationQuote(sourceTradeParticipation.QuoteParticipation, targetTradeParticipation.QuoteParticipation, ref notFoundListItemsQuote);
                }

                scope.Complete();

                xmlElementRoot = xmlDocument.CreateElement("Root", null);
                xmlElementRoot.SetAttribute("name", "Copy participation Check");

                if (notFoundListTransmittal.Count > 0)
                {
                    xmlElement = xmlDocument.CreateElement("Errors", null);
                    xmlElement.SetAttribute("name", "Drawings not found copying comparison");

                    foreach (String strItem in notFoundListTransmittal)
                        Utils.AddErrorMessageNode(xmlDocument, xmlElement, strItem);

                    xmlElementRoot.AppendChild(xmlElement);
                }

                if (notFoundListItems.Count > 0)
                {
                    xmlElement = xmlDocument.CreateElement("Errors", null);
                    xmlElement.SetAttribute("name", "Items not found copying comparison");

                    foreach (String strItem in notFoundListItems)
                        Utils.AddErrorMessageNode(xmlDocument, xmlElement, strItem);

                    xmlElementRoot.AppendChild(xmlElement);
                }

                if (notFoundListTransmittalQuote.Count > 0)
                {
                    xmlElement = xmlDocument.CreateElement("Errors", null);
                    xmlElement.SetAttribute("name", "Drawings not found copying quote");

                    foreach (String strItem in notFoundListTransmittalQuote)
                        Utils.AddErrorMessageNode(xmlDocument, xmlElement, strItem);

                    xmlElementRoot.AppendChild(xmlElement);
                }

                if (notFoundListItemsQuote.Count > 0)
                {
                    xmlElement = xmlDocument.CreateElement("Errors", null);
                    xmlElement.SetAttribute("name", "Items not found copying quote");

                    foreach (String strItem in notFoundListItemsQuote)
                        Utils.AddErrorMessageNode(xmlDocument, xmlElement, strItem);

                    xmlElementRoot.AppendChild(xmlElement);
                }

                if (xmlElementRoot.HasChildNodes)
                    xmlDocument.AppendChild(xmlElementRoot);

                return xmlDocument;
            }
        }

        /// <summary>
        /// Generate quote report
        /// </summary>
        public Byte[] GenerateQuoteReport(TradeParticipationInfo tradeParticipationInfo)
        {
           
            List<TradeItemInfo> tradeItemInfoList = new List<TradeItemInfo>();
            List<DrawingRevisionInfo> drawingRevisionInfoList = new List<DrawingRevisionInfo>();
            List<DrawingInfo> includedDrawings = tradeParticipationInfo.Trade.IncludedDrawings;
            TransmittalRevisionInfo transmittalRevisionInfoQuote;
            ParticipationItemInfo participationItemInfo;
            LocalReport localReport = new LocalReport();
            Decimal? totalQuote = GetQuoteTotal(tradeParticipationInfo.QuoteParticipation);

            
            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeParticipationInfo.Trade.ItemCategories)
                if (tradeItemCategoryInfo.TradeItems != null)
                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                    {
                        participationItemInfo = GetParticipationItem(tradeParticipationInfo.QuoteParticipation, tradeItemInfo);

                        if (participationItemInfo == null)
                            throw new Exception("The participation item does not exist.");

                        if (tradeParticipationInfo.Type == Info.TypeActive || tradeItemInfo.IsRequiredInProposal)
                        {
                            tradeItemInfo.TradeItemCategory = tradeItemCategoryInfo;
                            tradeItemInfo.IsIncluded = participationItemInfo.IsIncluded;
                            tradeItemInfo.Amount = participationItemInfo.Amount;
                            tradeItemInfo.Quantity = participationItemInfo.Quantity;
                            tradeItemInfo.Notes = participationItemInfo.Notes;

                            tradeItemInfoList.Add(tradeItemInfo);
                        }
                    }
            

            if (tradeParticipationInfo.QuoteParticipation.Transmittal != null && tradeParticipationInfo.QuoteParticipation.Transmittal.TransmittalRevisions != null)
                foreach (DrawingInfo drawingInfo in includedDrawings)
                {
                    transmittalRevisionInfoQuote = tradeParticipationInfo.QuoteParticipation.Transmittal.TransmittalRevisions.Find(delegate(TransmittalRevisionInfo TransmittalRevisionInfoInList) { return drawingInfo.Equals(TransmittalRevisionInfoInList.Drawing); });

                    if (transmittalRevisionInfoQuote != null)
                        drawingRevisionInfoList.Add(transmittalRevisionInfoQuote.Revision);
                }

          

            List<ReportParameter> reportParameters = new List<ReportParameter>();
             reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(tradeParticipationInfo.ProjectName)));
             reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(tradeParticipationInfo.TradeName)));
             reportParameters.Add(new ReportParameter("SubcontractorName", UI.Utils.SetFormString(tradeParticipationInfo.SubContractor.Name)));
             reportParameters.Add(new ReportParameter("InvitationDate", UI.Utils.SetFormDate(tradeParticipationInfo.InvitationDate)));
             reportParameters.Add(new ReportParameter("DueDate", UI.Utils.SetFormDate(tradeParticipationInfo.QuoteDueDate)));
             reportParameters.Add(new ReportParameter("SubmissionDateTime", UI.Utils.SetFormDateTime(tradeParticipationInfo.QuoteDate)));
             reportParameters.Add(new ReportParameter("BaseQuote", UI.Utils.SetFormDecimal(tradeParticipationInfo.Amount)));
             reportParameters.Add(new ReportParameter("TotalQuote", UI.Utils.SetFormDecimal(totalQuote)));
             reportParameters.Add(new ReportParameter("Comments", UI.Utils.SetFormString(tradeParticipationInfo.Comments)));

            localReport.ReportPath = Web.Utils.ReportsPath + "\\Quote.rdlc";
            localReport.DataSources.Add(new ReportDataSource("SOS_Core_TradeItemInfo", tradeItemInfoList));
            localReport.DataSources.Add(new ReportDataSource("SOS_Core_DrawingRevisionInfo", drawingRevisionInfoList));
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
              
                throw new Exception("Generating Quote Report");
               
            }

            

        }







#endregion

#region Addendums methods
        /// <summary>
        /// Sets initial state of a new addendum
        /// </summary>
        public AddendumInfo InitializeAddendum(TradeInfo tradeInfo)
        {
            AddendumInfo addendumInfo = new AddendumInfo();
            addendumInfo.Trade = tradeInfo;
            addendumInfo.Number = tradeInfo.Addendums != null ? tradeInfo.Addendums.Count + 1 : 1;

            return addendumInfo;
        }

        /// <summary>
        /// Creates an Addendum from a dr. Must have tadre, project, attachment group
        /// </summary>
        public AddendumInfo CreateAddendum(IDataReader dr)
        {
            PeopleController peopleController = PeopleController.GetInstance();

            AddendumInfo addendumInfo = new AddendumInfo(Data.Utils.GetDBInt32(dr["AddendumId"]));

            addendumInfo.Number = Data.Utils.GetDBInt32(dr["Number"]);
            addendumInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            addendumInfo.Description = Data.Utils.GetDBString(dr["Description"]);
            addendumInfo.AddendumDate = Data.Utils.GetDBDateTime(dr["AddendumDate"]);

            AssignAuditInfo(addendumInfo, dr);

            if (dr["AttachmentsGroupId"] != DBNull.Value)
                addendumInfo.AttachmentsGroup = new AttachmentsGroupInfo(Data.Utils.GetDBInt32(dr["AttachmentsGroupId"]));

            if (dr["TradeId"] != DBNull.Value)
            {
                addendumInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                addendumInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                if (dr["ProjectId"] != DBNull.Value)
                {
                    addendumInfo.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    addendumInfo.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    addendumInfo.Trade.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
                }
            }            

            return addendumInfo;
        }

        /// <summary>
        /// Get an Addendum from persistent storage
        /// </summary>
        public AddendumInfo GetAddendum(int? AddendumId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetAddendum(AddendumId);
                if (dr.Read())
                    return CreateAddendum(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Addendum from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get an Addendum with attachments from persistent storage
        /// </summary>
        public AddendumInfo GetAddendumWithAttachments(int? AddendumId)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            AddendumInfo addendumInfo = GetAddendum(AddendumId);
            projectsController.GetAttachments(addendumInfo);

            return addendumInfo;
        }

        /// <summary>
        /// Get the Addendums for the specified trade
        /// </summary>
        public List<AddendumInfo> GetAddendums(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            List<AddendumInfo> addendumInfoList = new List<AddendumInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetAddendumsByTrade(tradeInfo.Id);
                while (dr.Read())
                    addendumInfoList.Add(CreateAddendum(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Addendums for trade from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return addendumInfoList;
        }

        /// <summary>
        /// Get the Addendums with attachments for the specified trade
        /// </summary>
        public List<AddendumInfo> GetAddendumsWithAttachments(TradeInfo tradeInfo)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<AddendumInfo> addendumInfoList = GetAddendums(tradeInfo);

            foreach (AddendumInfo addendumInfo in addendumInfoList)
                projectsController.GetAttachments(addendumInfo);

            return addendumInfoList;
        }

        /// <summary>
        /// Updates an Addendum in the database
        /// </summary>
        public void UpdateAddendum(AddendumInfo addendumInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(addendumInfo);

            parameters.Add(addendumInfo.Id);
            parameters.Add(addendumInfo.Number);
            parameters.Add(addendumInfo.Name);
            parameters.Add(addendumInfo.Description);
            parameters.Add(addendumInfo.AddendumDate);

            parameters.Add(addendumInfo.ModifiedDate);
            parameters.Add(addendumInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateAddendum(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Addendum in database");
            }
        }

        /// <summary>
        /// Adds an Addendum to the database
        /// </summary>
        public int? AddAddendum(AddendumInfo addendumInfo)
        {
            int? addendumId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(addendumInfo);

            parameters.Add(GetId(addendumInfo.Trade));
            parameters.Add(addendumInfo.Number);
            parameters.Add(addendumInfo.Name);
            parameters.Add(addendumInfo.Description);
            parameters.Add(addendumInfo.AddendumDate);

            parameters.Add(addendumInfo.CreatedDate);
            parameters.Add(addendumInfo.CreatedBy);

            try
            {
                addendumId = Data.DataProvider.GetInstance().AddAddendum(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Addendum to database");
            }

            return addendumId;
        }

        

        /// <summary>
        /// Adds or updates a Addendum
        /// </summary>
        public int? AddUpdateAddendum(AddendumInfo addendumInfo)
        {
            if (addendumInfo != null)
            {
                if (addendumInfo.Id != null)
                {
                    UpdateAddendum(addendumInfo);
                    return addendumInfo.Id;
                }
                else
                    return AddAddendum(addendumInfo);
            }
            else
                return null;
        }

        /// <summary>
        /// Remove an Addendum from persistent storage
        /// </summary>
        public void DeleteAddendum(AddendumInfo addendumInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteAddendum(addendumInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Addendum from database");
            }
        }
#endregion

#region Minutes Templates
        /// <summary>
        /// Creates a Minutes Template from a dr
        /// </summary>
        public MinutesTemplateInfo CreateMinutesTemplate(IDataReader dr)
        {
            MinutesTemplateInfo minutesTemplateInfo = new MinutesTemplateInfo(Data.Utils.GetDBInt32(dr["MinutesTemplateId"]));
            minutesTemplateInfo.Template = Data.Utils.GetDBString(dr["Template"]);

            AssignAuditInfo(minutesTemplateInfo, dr);

            return minutesTemplateInfo;
        }

        /// <summary>
        /// Get a Minutes Template from persistent storage
        /// </summary>
        public MinutesTemplateInfo GetMinutesTemplate(int? minutesTemplateId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetMinutesTemplate(minutesTemplateId);
                if (dr.Read())
                    return CreateMinutesTemplate(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Minutes Template from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Updates a Minutes Template in the database
        /// </summary>
        public void UpdateMinutesTemplate(MinutesTemplateInfo minutesTemplateInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(minutesTemplateInfo);

            parameters.Add(minutesTemplateInfo.Id);
            parameters.Add(minutesTemplateInfo.Template);

            parameters.Add(minutesTemplateInfo.ModifiedDate);
            parameters.Add(minutesTemplateInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateMinutesTemplate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Minutes Template in database");
            }
        }
#endregion

#region Invitation Templates
        /// <summary>
        /// Creates an Invitation Template from a dr
        /// </summary>
        public InvitationTemplateInfo CreateInvitationTemplate(IDataReader dr)
        {
            InvitationTemplateInfo invitationTemplateInfo = new InvitationTemplateInfo(Data.Utils.GetDBInt32(dr["InvitationTemplateId"]));
            invitationTemplateInfo.Template = Data.Utils.GetDBString(dr["Template"]);

            AssignAuditInfo(invitationTemplateInfo, dr);

            return invitationTemplateInfo;
        }

        /// <summary>
        /// Get an Invitation Template from persistent storage
        /// </summary>
        public InvitationTemplateInfo GetInvitationTemplate(int? invitationTemplateId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetInvitationTemplate(invitationTemplateId);
                if (dr.Read())
                    return CreateInvitationTemplate(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Invitation Template from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Updates an Invitation Template in the database
        /// </summary>
        public void UpdateInvitationTemplate(InvitationTemplateInfo invitationTemplateInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(invitationTemplateInfo);

            parameters.Add(invitationTemplateInfo.Id);
            parameters.Add(invitationTemplateInfo.Template);

            parameters.Add(invitationTemplateInfo.ModifiedDate);
            parameters.Add(invitationTemplateInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateInvitationTemplate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Invitation Template in database");
            }
        }
#endregion

#region Advanced Trade Methods
        /// <summary>
        /// Returns the Comparison total for a trade participation
        /// </summary>
        public Decimal GetComparisonTotal(TradeParticipationInfo tradeParticipationInfo)
        {
            Decimal total = tradeParticipationInfo.Amount == null ? 0 : (Decimal)tradeParticipationInfo.Amount;

            if (tradeParticipationInfo.ParticipationItems != null)
                foreach (ParticipationItemInfo participationItemInfo in tradeParticipationInfo.ParticipationItems)
                    if (participationItemInfo.Amount != null)
                        total = total + (Decimal)participationItemInfo.Amount;

            return total;
        }

        /// <summary>
        /// Checks if a participation item is included in the contract
        /// </summary>
        public Boolean IsItemIncluded(ParticipationItemInfo participationItemInfo)
        {
            Boolean isIncludeCase = false;

            if (participationItemInfo.IsIncluded != null)
            {
                if ((Boolean)participationItemInfo.IsIncluded)
                    isIncludeCase = true;
                else
                    if (participationItemInfo.Confirmed != null)
                        if ((Boolean)participationItemInfo.Confirmed)
                            isIncludeCase = true;
            }

            return isIncludeCase;
        }

        /// <summary>
        /// Returns the Contract total for a trade participation. The trade must include all its Items and Participations
        /// </summary>
        public Decimal GetQuoteTotal(TradeParticipationInfo tradeParticipationInfo)
        {
            Decimal total = tradeParticipationInfo.Amount == null ? 0 : (Decimal)tradeParticipationInfo.Amount;

            if (tradeParticipationInfo.ParticipationItems != null)
                foreach (ParticipationItemInfo participationItemInfo in tradeParticipationInfo.ParticipationItems)
                    if (IsItemIncluded(participationItemInfo))
                        if (participationItemInfo.Amount != null)
                            total = total + (Decimal)participationItemInfo.Amount;

            return total;
        }

        /// <summary>
        /// Check to see if a budget item is included
        /// </summary>
        public Boolean ItemBudgetIncluded(ParticipationItemInfo participationItemInfo)
        {
            if (participationItemInfo.IsIncluded != null)
                if ((bool)participationItemInfo.IsIncluded)
                {
                    if (participationItemInfo.Amount == null)
                        return true;
                }
                else
                    if (participationItemInfo.Amount != null)
                        if ((decimal)participationItemInfo.Amount >= 0)
                            return true;

            return false;
        }

        /// <summary>
        /// Check to see if a budget item is excluded
        /// </summary>
        public Boolean ItemBudgetExcluded(ParticipationItemInfo participationItemInfo)
        {
            if (participationItemInfo.IsIncluded != null)
                if (!(bool)participationItemInfo.IsIncluded)
                {
                    if (participationItemInfo.Amount == null)
                        return true;
                }
                else
                    if (participationItemInfo.Amount != null)
                        if ((decimal)participationItemInfo.Amount <= 0)
                            return true;

            return false;
        }

        /// <summary>
        /// Check to see if a participation item is included
        /// </summary>
        public Boolean ItemIncluded(ParticipationItemInfo participationItemInfo)
        {
            if (participationItemInfo.IsIncluded != null)
                if ((bool)participationItemInfo.IsIncluded)
                    return true;
                else
                    if (participationItemInfo.Amount != null)
                        if ((decimal)participationItemInfo.Amount >= 0)
                            return true;

            return false;
        }

        /// <summary>
        /// Check to see if a participation item is excluded
        /// </summary>
        public Boolean ItemExcluded(ParticipationItemInfo participationItemInfo)
        {
            if (participationItemInfo.IsIncluded != null)
                if (!(bool)participationItemInfo.IsIncluded)
                {
                    if (participationItemInfo.Amount == null)
                        return true;
                }
                else
                    if (participationItemInfo.Amount != null)
                        if ((decimal)participationItemInfo.Amount <= 0)
                            return true;

            return false;
        }

        /// <summary>
        /// Checks a project trade for errors
        /// </summary>
        public XmlDocument CheckTrade(TradeInfo tradeInfo, TradeParticipationInfo tradeParticipation, Boolean ignoreRankAssignment)
        {
            Decimal quoteTotal;
            String companyName = Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyShortName");
            List<TradeParticipationInfo> tradeParticipationInfoList = null;
            ParticipationItemInfo participationItemInfo;
            ParticipationItemInfo budgetItemInfo;
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElementColumn;
            XmlElement xmlElementError;
            XmlElement xmlElementError1;
            XmlElement xmlElementItem;
            XmlElement xmlElementItem1;
            FileInfo fileInfo;

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));
            xmlElementRoot = xmlDocument.CreateElement("errors", null);

            if (tradeParticipation == null)
            {
                xmlElementColumn = xmlDocument.CreateElement("error", null);
                xmlElementColumn.SetAttribute("text", "General");

                // checking the whole Trade or just one participation
                if (tradeInfo.SelectedParticipation == null)
                {
                    xmlElementError = xmlDocument.CreateElement("errorGeneral", null);
                    xmlElementError.SetAttribute("text", "Winning quote not specified");
                    xmlElementColumn.AppendChild(xmlElementError);
                }
                else
                {
                    if (tradeInfo.TradeBudgets != null && tradeInfo.TradeBudgets.Count != 0)
                    {
                        quoteTotal = GetQuoteTotal(tradeInfo.SelectedParticipation);

                        if (tradeInfo.TotalBudgetAmount != quoteTotal)
                        {
                            xmlElementError = xmlDocument.CreateElement("errorGeneral", null);
                            xmlElementError.SetAttribute("text", "Budget assignment must match quote amount");
                            xmlElementColumn.AppendChild(xmlElementError);
                        }
                    }
                }

                if (!ignoreRankAssignment && !tradeInfo.AllRanksAssigned)
                {
                    xmlElementError = xmlDocument.CreateElement("errorGeneral", null);
                    xmlElementError.SetAttribute("text", "Not all ranks are assigned");
                    xmlElementColumn.AppendChild(xmlElementError);
                }

                if (tradeInfo.QuotesFile == null)
                {
                    xmlElementError = xmlDocument.CreateElement("errorGeneral", null);
                    xmlElementError.SetAttribute("text", "Quotes file not specified");
                    xmlElementColumn.AppendChild(xmlElementError);
                }
                else
                {
                    fileInfo = new FileInfo(UI.Utils.FullPath(tradeInfo.Project.AttachmentsFolder, tradeInfo.QuotesFile));
                    if (!fileInfo.Exists)
                    {
                        xmlElementError = xmlDocument.CreateElement("errorGeneral", null);
                        xmlElementError.SetAttribute("text", "Quotes file does not exist");
                        xmlElementColumn.AppendChild(xmlElementError);
                    }
                }

                if (xmlElementColumn.HasChildNodes)
                    xmlElementRoot.AppendChild(xmlElementColumn);

                tradeParticipationInfoList = tradeInfo.Participations;
            }
            else
            {
                tradeParticipationInfoList = new List<TradeParticipationInfo>();
                tradeParticipationInfoList.Add(tradeParticipation);
            }

            foreach (TradeParticipationInfo tradeParticipationInfo in tradeParticipationInfoList)
                if (!tradeParticipationInfo.IsPulledOut)
                {
                    xmlElementColumn = xmlDocument.CreateElement("column", null);

                    // Y/N are completed and Quantity check 
                    xmlElementError = xmlDocument.CreateElement("error", null);
                    xmlElementError.SetAttribute("text", "Y/N Columns not filled out");

                    xmlElementError1 = xmlDocument.CreateElement("error", null);
                    xmlElementError1.SetAttribute("text", "Quantity field not filled out");

                    xmlElementItem = null;
                    xmlElementItem1 = null;

                    foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                        foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        {
                            participationItemInfo = GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                            if (participationItemInfo == null)
                                throw new Exception("The participation item does not exist");

                            if (tradeParticipationInfo.IsActive || tradeItemInfo.IsRequiredInProposal)
                            {
                                if (participationItemInfo.IsIncluded == null)
                                {
                                    xmlElementItem = xmlDocument.CreateElement("item", null);
                                    xmlElementItem.SetAttribute("id", participationItemInfo.IdStr);
                                    xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                                    xmlElementError.AppendChild(xmlElementItem);
                                }

                                if (tradeItemInfo.RequiresQuantityCheck != null && (Boolean)tradeItemInfo.RequiresQuantityCheck && participationItemInfo.Quantity == null)
                                {
                                    xmlElementItem1 = xmlDocument.CreateElement("item", null);
                                    xmlElementItem1.SetAttribute("id", participationItemInfo.IdStr);
                                    xmlElementItem1.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                                    xmlElementError1.AppendChild(xmlElementItem1);
                                }

                            }
                        }

                    if (xmlElementError.HasChildNodes)
                        xmlElementColumn.AppendChild(xmlElementError);

                    if (xmlElementError1.HasChildNodes)
                        xmlElementColumn.AppendChild(xmlElementError1);

                    // Budget column cases
                    if (tradeParticipationInfo.Equals(tradeInfo.Participations[0]))
                    {
                        // Budget not entered
                        if (tradeParticipationInfo.Amount == null)
                        {
                            xmlElementError = xmlDocument.CreateElement("errorBudget", null);
                            xmlElementError.SetAttribute("text", "Budget not entered");
                            xmlElementError.SetAttribute("id", tradeInfo.IdStr);

                            xmlElementColumn.AppendChild(xmlElementError);
                        }

                        // Included in the budget and adding value
                        xmlElementError = xmlDocument.CreateElement("error", null);
                        xmlElementError.SetAttribute("text", "Item included in budget can not add more value");

                        xmlElementItem = null;

                        foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                            foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                            {
                                participationItemInfo = GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                                if (participationItemInfo == null)
                                    throw new Exception("The budget item does not exist");

                                if (participationItemInfo.IsIncluded != null && (bool)participationItemInfo.IsIncluded && participationItemInfo.Amount != null && (decimal)participationItemInfo.Amount > 0)
                                {
                                    xmlElementItem = xmlDocument.CreateElement("item", null);
                                    xmlElementItem.SetAttribute("id", participationItemInfo.IdStr);
                                    xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                                    xmlElementError.AppendChild(xmlElementItem);
                                }
                            }

                        if (xmlElementError.HasChildNodes)
                            xmlElementColumn.AppendChild(xmlElementError);

                        // Not Included in the budget and subtracting value
                        xmlElementError = xmlDocument.CreateElement("error", null);
                        xmlElementError.SetAttribute("text", "Item not included in budget can not subtract value");

                        xmlElementItem = null;

                        foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                            foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                            {
                                participationItemInfo = GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                                if (participationItemInfo == null)
                                    throw new Exception("The budget item does not exist");

                                if (participationItemInfo.IsIncluded != null && !(bool)participationItemInfo.IsIncluded && participationItemInfo.Amount != null && (decimal)participationItemInfo.Amount < 0)
                                {
                                    xmlElementItem = xmlDocument.CreateElement("item", null);
                                    xmlElementItem.SetAttribute("id", participationItemInfo.IdStr);
                                    xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                                    xmlElementError.AppendChild(xmlElementItem);
                                }
                            }

                        if (xmlElementError.HasChildNodes)
                            xmlElementColumn.AppendChild(xmlElementError);

                        if (tradeInfo.IsUsingBudgetModule)
                        {
                            // If using the budget module Amounts should be empty
                            xmlElementError = xmlDocument.CreateElement("error", null);
                            xmlElementError.SetAttribute("text", "Item cannot specify an amount when using the Budget Module");

                            xmlElementItem = null;

                            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                                foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                                {
                                    budgetItemInfo = GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                                    if (budgetItemInfo == null)
                                        throw new Exception("The budget item does not exist");

                                    if (budgetItemInfo.Amount != null)
                                    {
                                        xmlElementItem = xmlDocument.CreateElement("item", null);
                                        xmlElementItem.SetAttribute("id", budgetItemInfo.IdStr);
                                        xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + budgetItemInfo.TradeItem.Name);

                                        xmlElementError.AppendChild(xmlElementItem);
                                    }
                                }

                            if (xmlElementError.HasChildNodes)
                                xmlElementColumn.AppendChild(xmlElementError);
                        }
                    }

                    // Subbies cases
                    else
                    {
                        // Quote not entered
                        if (tradeParticipationInfo.Amount == null)
                        {
                            xmlElementError = xmlDocument.CreateElement("error", null);
                            xmlElementError.SetAttribute("text", "Quote not entered");
                            xmlElementError.SetAttribute("id", tradeParticipationInfo.IdStr);

                            xmlElementColumn.AppendChild(xmlElementError);
                        }

                        // Included in budget but excluded in the subbies
                        xmlElementError = xmlDocument.CreateElement("error", null);
                        xmlElementError.SetAttribute("text", "Item included in budget must be included");

                        xmlElementItem = null;

                        foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                            foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                            {
                                budgetItemInfo = GetParticipationItem(tradeInfo.Participations[0], tradeItemInfo);
                                participationItemInfo = GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                                if (budgetItemInfo == null)
                                    throw new Exception("The budget item does not exist");

                                if (participationItemInfo == null)
                                    throw new Exception("The participation item does not exist");

                                if (ItemBudgetIncluded(budgetItemInfo) && !ItemIncluded(participationItemInfo))
                                {
                                    xmlElementItem = xmlDocument.CreateElement("item", null);
                                    xmlElementItem.SetAttribute("id", participationItemInfo.IdStr);
                                    xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                                    xmlElementError.AppendChild(xmlElementItem);
                                }
                            }

                        if (xmlElementError.HasChildNodes)
                            xmlElementColumn.AppendChild(xmlElementError);

                        // Excluded from budget but included in the subbies
                        xmlElementError = xmlDocument.CreateElement("error", null);
                        xmlElementError.SetAttribute("text", "Item excluded from budget must be excluded");

                        xmlElementItem = null;

                        foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                            foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                            {
                                budgetItemInfo = GetParticipationItem(tradeInfo.Participations[0], tradeItemInfo);
                                participationItemInfo = GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                                if (budgetItemInfo == null)
                                    throw new Exception("The budget item does not exist");

                                if (participationItemInfo == null)
                                    throw new Exception("The participation item does not exist");

                                if (ItemBudgetExcluded(budgetItemInfo) && !ItemExcluded(participationItemInfo))
                                {
                                    xmlElementItem = xmlDocument.CreateElement("item", null);
                                    xmlElementItem.SetAttribute("id", participationItemInfo.IdStr);
                                    xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                                    xmlElementError.AppendChild(xmlElementItem);
                                }
                            }

                        if (xmlElementError.HasChildNodes)
                            xmlElementColumn.AppendChild(xmlElementError);
                    }

                    if (xmlElementColumn.HasChildNodes)
                    {
                        xmlElementColumn.SetAttribute("id", tradeParticipationInfo.IdStr);

                        if (tradeParticipationInfo.Equals(tradeInfo.Participations[0]))
                            xmlElementColumn.SetAttribute("name", companyName);
                        else
                            xmlElementColumn.SetAttribute("name", tradeParticipationInfo.SubContractor.Name);

                        xmlElementRoot.AppendChild(xmlElementColumn);
                    }

                }

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }

        /// <summary>
        /// Checks a trade participation for errors
        /// </summary>
        public XmlDocument CheckParticipation(TradeParticipationInfo tradeParticipationInfo)
        {
            ParticipationItemInfo participationItemInfo;
            List<DrawingInfo> includedDrawings = tradeParticipationInfo.Trade.IncludedDrawings;
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElementError;
            XmlElement xmlElementError1;
            XmlElement xmlElementError2;
            XmlElement xmlElementItem;

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));
            xmlElementRoot = xmlDocument.CreateElement("errors", null);

            // Quote not entered
            if (tradeParticipationInfo.Amount == null)
            {
                xmlElementError = xmlDocument.CreateElement("error", null);
                xmlElementError.SetAttribute("text", "Quote amount not entered");

                xmlElementRoot.AppendChild(xmlElementError);
            }

            // Uncomment to make quote file required
            // Quote file not uploaded
            //if (tradeParticipationInfo.QuoteFile == null)
            //{
            //    xmlElementError = xmlDocument.CreateElement("error", null);
            //    xmlElementError.SetAttribute("text", "Quote file not uploaded");

            //    xmlElementRoot.AppendChild(xmlElementError);
            //}

            // Y/N are completed and Quantity check 
            xmlElementError = xmlDocument.CreateElement("error", null);
            xmlElementError.SetAttribute("text", "Y/N Columns not filled out");

            xmlElementError1 = xmlDocument.CreateElement("error", null);
            xmlElementError1.SetAttribute("text", "Quantity field not filled out");

            xmlElementError2 = xmlDocument.CreateElement("error", null);
            xmlElementError2.SetAttribute("text", "Drawings revisions not selected");

            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeParticipationInfo.Trade.ItemCategories)
                foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                {
                    participationItemInfo = GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                    if (participationItemInfo == null)
                        throw new Exception("The participation item does not exist");

                    if (tradeParticipationInfo.IsActive || tradeItemInfo.IsRequiredInProposal)
                    {
                        if (participationItemInfo.IsIncluded == null)
                        {
                            xmlElementItem = xmlDocument.CreateElement("item", null);
                            xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                            xmlElementError.AppendChild(xmlElementItem);
                        }

                        if (tradeItemInfo.RequiresQuantityCheck != null && (Boolean)tradeItemInfo.RequiresQuantityCheck && participationItemInfo.Quantity == null)
                        {
                            xmlElementItem = xmlDocument.CreateElement("item", null);
                            xmlElementItem.SetAttribute("name", tradeItemCategoryInfo.ShortDescription + " / " + participationItemInfo.TradeItem.Name);

                            xmlElementError1.AppendChild(xmlElementItem);
                        }

                    }
                }

            if (tradeParticipationInfo.Transmittal != null && tradeParticipationInfo.Transmittal.TransmittalRevisions != null)
                foreach (DrawingInfo drawingInfo in includedDrawings)
                {
                    if (tradeParticipationInfo.Transmittal.TransmittalRevisions.Find(delegate(TransmittalRevisionInfo TransmittalRevisionInfoInList) { return drawingInfo.Equals(TransmittalRevisionInfoInList.Drawing); }) == null)
                    {
                        xmlElementItem = xmlDocument.CreateElement("item", null);
                        xmlElementItem.SetAttribute("name", drawingInfo.Name);

                        xmlElementError2.AppendChild(xmlElementItem);
                    }
                }

            if (xmlElementError.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElementError);

            if (xmlElementError1.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElementError1);

            if (xmlElementError2.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElementError2);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }

        private String GetParaticipationDetails(ParticipationItemInfo participationItem)
        {
            String itemDetails;

            if (participationItem.TradeParticipation.SubContractor != null)
                itemDetails = participationItem.TradeParticipation.SubcontractorName;
            else
                itemDetails = "BOQ";

            if (participationItem.TradeParticipation.ComparisonParticipation != null)
                itemDetails = itemDetails + " - Quote";
            else
                itemDetails = itemDetails + " - Comparison";

            if (participationItem.TradeParticipation.Type == Info.TypeProposal)
                itemDetails = itemDetails + " - Proposal";
            else
                itemDetails = itemDetails + " - Active";

            return itemDetails;
        }

        /// <summary>
        /// Creates the scope of works for a contract. The trade must have a selected participant (Rank = 1)
        /// </summary>
        public String CreateScopeOfWorks(TradeInfo tradeInfo)
        {
            TradeParticipationInfo tradeParticipationInfo = tradeInfo.SelectedParticipation;
            StringBuilder scope = new StringBuilder();
            StringBuilder itemInfo;

            if (tradeInfo.ScopeHeader != null)
                scope.Append(tradeInfo.ScopeHeader).Append("\n\n");

            if (tradeInfo.ItemCategories != null)
                foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                {
                    itemInfo = new StringBuilder();
                    if (tradeItemCategoryInfo.TradeItems != null)
                    {
                        foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        {
                            if (tradeItemInfo.Scope != null)
                                //#--- for DraftContract--17-01-2019
                                if (tradeParticipationInfo != null)   //#--- #--- for DraftContract
                                {
                                    if (IsItemIncluded(GetParticipationItem(tradeParticipationInfo, tradeItemInfo)))
                                        itemInfo.Append(tradeItemInfo.Scope).Append("\n");
                                }
                                else
                                {   //#--- for DraftContract--17-01-2019

                                    
                                        itemInfo.Append(tradeItemInfo.Scope).Append("\n");

                                }  //#--- for DraftContract--17-01-2019
                        }
                    }          

                    if (itemInfo.Length > 0)
                    {
                        if (tradeItemCategoryInfo.LongDescription != null)
                            scope.Append(tradeItemCategoryInfo.LongDescription).Append("\n\n");

                        scope.Append(itemInfo.ToString());
                        scope.Append("\n");
                    }
                }

            if (tradeInfo.ScopeFooter != null)
                scope.Append(tradeInfo.ScopeFooter);

            if (tradeInfo.Project != null)
                if (tradeInfo.Project.SpecialClause != null)
                    scope.Append("\n" + tradeInfo.Project.SpecialClause);

            return scope.ToString();
        }

        /// <summary>
        /// Verifies if a Trade item can be removed
        /// </summary>
        public XmlDocument CheckTradeItemForDelete(TradeItemInfo tradeItem)
        {
            List<ParticipationItemInfo> participationItemInfoList = GetParticipationItems(tradeItem);
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElementError;
            XmlElement xmlElementItem;

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));

            xmlElementRoot = xmlDocument.CreateElement("Root", null);
            xmlElementRoot.SetAttribute("name", "Errors deleting item");

            xmlElementError = xmlDocument.CreateElement("error", null);
            xmlElementError.SetAttribute("name", "Trade Participations containing data");

            foreach (ParticipationItemInfo participationItem in participationItemInfoList)
            {
                if (!participationItem.IsEmpty)
                {
                    xmlElementItem = xmlDocument.CreateElement("item", null);
                    xmlElementItem.SetAttribute("name", GetParaticipationDetails(participationItem));

                    xmlElementError.AppendChild(xmlElementItem);
                }
            }

            if (xmlElementError.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElementError);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }

        /// <summary>
        /// Verifies if a Trade item category can be removed
        /// </summary>
        public XmlDocument CheckTradeItemCateforyForDelete(TradeItemCategoryInfo tradeItemCategory)
        {
            List<ParticipationItemInfo> participationItemInfoList = GetParticipationItems(tradeItemCategory);
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElementError;
            XmlElement xmlElementItem;

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));

            xmlElementRoot = xmlDocument.CreateElement("Root", null);
            xmlElementRoot.SetAttribute("name", "Errors deleting category");

            xmlElementError = xmlDocument.CreateElement("error", null);
            xmlElementError.SetAttribute("name", "Trade Items/Participations containing data");

            foreach (ParticipationItemInfo participationItem in participationItemInfoList)
            {
                if (!participationItem.IsEmpty)
                {
                    xmlElementItem = xmlDocument.CreateElement("item", null);
                    xmlElementItem.SetAttribute("name", UI.Utils.SetFormString(participationItem.TradeItem.Name) + " - " + GetParaticipationDetails(participationItem));

                    xmlElementError.AppendChild(xmlElementItem);
                }
            }

            if (xmlElementError.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElementError);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }
#endregion

#region Reports Methods for Bids and Trades
        /// <summary>
        /// Get the bids chart report records
        /// </summary>
       //#--- public List<BidsChart> GetBidsChart(TradeTemplateInfo tradeTemplateInfo, BusinessUnitInfo businessUnitInfo, String participationType)

        public List<BidsChart> GetBidsChart(TradeTemplateInfo tradeTemplateInfo, BusinessUnitInfo businessUnitInfo, String participationType, DateTime? Fromdate)    //#---
        {
            IDataReader dr = null;
            BidsChart bidsChart;
            int currentId;
            int numOnes;
            int numTotal;
            List<Object> parameters = new List<Object>();
            List<BidsChart> bidsChartList = new List<BidsChart>();
            Dictionary<int, int> dictionaryStrikeRate = new Dictionary<int, int>();

            parameters.Add(participationType);
            parameters.Add(tradeTemplateInfo.TradeCode);
            parameters.Add(businessUnitInfo.Id);
            
            parameters.Add(Fromdate);//#--

            try
            {
                dr = Data.DataProvider.GetInstance().GetBidsChart(parameters.ToArray());
                while (dr.Read())
                {
                    bidsChart = new BidsChart();
                    
                    bidsChart.ProjectName = Data.Utils.GetDBString(dr["ProjectName"]);
                    bidsChart.ProjectsCommencementDate = Data.Utils.GetDBDateTime(dr["ProjectsCommencementDate"]);
                    bidsChart.SubcontractorName = Data.Utils.GetDBString(dr["SubContractorName"]);
                    bidsChart.SubcontractorId = Data.Utils.GetDBInt32(dr["SubContractorId"]);
                    bidsChart.Rank = Data.Utils.GetDBInt32(dr["Rank"]);
                    bidsChart.StrikeRate = 0;

                    bidsChartList.Add(bidsChart);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade Bids from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            if (bidsChartList.Count > 0)
            {
                currentId = (int)bidsChartList[0].SubcontractorId;
                numTotal = 0;
                numOnes = 0;

                foreach (BidsChart bidsChart1 in bidsChartList)
                {
                    if ((int)bidsChart1.SubcontractorId != currentId)
                    {
                        dictionaryStrikeRate.Add(currentId, (int)Math.Round(((decimal)numOnes / (decimal)numTotal) * 100));
                        currentId = (int)bidsChart1.SubcontractorId;
                        numTotal = 0;
                        numOnes = 0;
                    }

                    numTotal = numTotal + 1;

                    if (bidsChart1.Rank != null && (int)bidsChart1.Rank == 1)
                        numOnes = numOnes + 1;
                }

                dictionaryStrikeRate.Add(currentId, (int)Math.Round(((decimal)numOnes / (decimal)numTotal) * 100));

                foreach (BidsChart bidsChart1 in bidsChartList)
                    bidsChart1.StrikeRate = dictionaryStrikeRate[(int)bidsChart1.SubcontractorId];

                bidsChartList.Sort();
                bidsChartList.Reverse();
            }

            return bidsChartList;
        }

        public List<BidsChart> GetBidsChartActive(TradeTemplateInfo tradeTemplateInfo, BusinessUnitInfo businessUnitInfo)
        {
            //#--- return GetBidsChart(tradeTemplateInfo, businessUnitInfo, Info.TypeActive);
            return GetBidsChart(tradeTemplateInfo, businessUnitInfo, Info.TypeActive,null);   //#---
        }

        public List<BidsChart> GetBidsChartProposal(TradeTemplateInfo tradeTemplateInfo, BusinessUnitInfo businessUnitInfo)
        {
            //# return GetBidsChart(tradeTemplateInfo, businessUnitInfo, Info.TypeProposal);    
            return GetBidsChart(tradeTemplateInfo, businessUnitInfo, Info.TypeProposal,null);
            //  #--
        }


        //#-For Trade Report to get Item categories, Items and Scope of work
        public DataTable GetTrade_TradeItemCategories_TradeItem(string TradeCode)
        {
            DataTable dt = new DataTable();
            IDataReader dr;
            try
            {
               dr= Data.DataProvider.GetInstance().GetTrade_Categories_Item(TradeCode);
                if (dr.Read())
                {
                    dt.Load(dr);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade and its Item Categories, Items andScopeof work from database");
            }


            return dt;
        }

        //#------


        //#---- For Rating A Report --- To get Rating A Subcontractors and their contacts list
        public DataTable GetRatingASubContractors(string TradeCode,string BusinessUnit)//string[] parameters
        {
            List<string> parameters = new List<string>();
            parameters.Add(TradeCode);
            parameters.Add(BusinessUnit);


            DataTable dt = new DataTable();
            IDataReader dr;
            try
            {
                dr = Data.DataProvider.GetInstance().GetRatingASubContractors(parameters.ToArray());
                if (dr.Read())
                {
                    dt.Load(dr);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Rating A subcontractors from database");
            }


            return dt;
        }

        //#---- For Ratin A Report ---


        //#---- to get All Rating A sucontractors Report --- To get all Rating A Subcontractors and their contacts list
        public DataTable GetAllRatingASubContractors()
        {
           
            DataTable dt = new DataTable();
            IDataReader dr;
            try
            {
                dr = Data.DataProvider.GetInstance().GetALLRatingASubContractors();
                if (dr.Read())
                {
                    dt.Load(dr);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting All Rating A subcontractors from database");
            }


            return dt;
        }

        //#----------------

        #endregion

        #endregion

    }
}
