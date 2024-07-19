using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;



namespace SOS.Data
{
    /// <summary>
    /// Singleton class to provide data access
    /// </summary>
    public sealed class DataProvider
    {

        #region Private Members
        private static DataProvider instance;
        private SqlDatabase database = null;
        #endregion

        #region Private properties
        private String ConnectionString
        {
            get { 
                
                return ConfigurationManager.AppSettings["ConnectionString"].ToString();
              

            }
        }

        private SqlDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new SqlDatabase(ConnectionString);
                   
                }
                 
                return database;
            }
        }
        #endregion

        #region Private Methods
        private DataProvider()
        {
        }

        private Object CheckDBNull(Object obj)
        {
            return obj == null ? DBNull.Value : obj;
        }

        private void CheckDBNull(Object[] obj)
        {
            for (int i = 0; i <= obj.Length - 1; i++)
                obj[i] = CheckDBNull(obj[i]);
        }
        #endregion

        #region Public Methods
        public static DataProvider GetInstance()
        {
            if (instance == null)
                instance = new DataProvider();

            return instance;
        }

        #region JobTypes Methods
        public IDataReader GetJobTypes()
        {
            return (IDataReader)Database.ExecuteReader("JobTypesList");
        }

        public IDataReader GetJobType(int? jobTypeId)
        {
            return (IDataReader)Database.ExecuteReader("JobTypesGet", CheckDBNull(jobTypeId));
        }

        public void UpdateJobType(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("JobTypesUpdate", parameters);
        }

        public int? AddJobType(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("JobTypesAdd", parameters));
        }

        public void DeleteJobType(int? jobTypeId)
        {
            Database.ExecuteNonQuery("JobTypesDelete", CheckDBNull(jobTypeId));
        }
        #endregion

        #region BusinessUnits Methods
        public IDataReader GetBusinessUnits()
        {
            return (IDataReader)Database.ExecuteReader("BusinessUnitsList");
        }

        public IDataReader GetBusinessUnit(int? BusinessUnitId)
        {
            return (IDataReader)Database.ExecuteReader("BusinessUnitsGet", CheckDBNull(BusinessUnitId));
        }

        public void UpdateBusinessUnit(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("BusinessUnitsUpdate", parameters);
        }

        public int? AddBusinessUnit(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("BusinessUnitsAdd", parameters));
        }

        public void DeleteBusinessUnit(int? BusinessUnitId)
        {
            Database.ExecuteNonQuery("BusinessUnitsDelete", CheckDBNull(BusinessUnitId));
        }
        #endregion
         //#----
        #region Contract Approval Limit
        public void UpdateContractApprovalLimit(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ContractApprovalLimitUpdate", parameters);


        }

        public IDataReader GetContractApprovalLimits()
        {
            return (IDataReader)Database.ExecuteReader("ContractApprovalLimitList");
        }



        #endregion


        #region KPIRange 
        public void UpdateKPIRange(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("KPIRangeUpdate", parameters);


        }

        
          public void UpdateKPIPoints(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("KPIPointsUpdate", parameters);


        }


        #endregion


        //#----

        #region DrawingTypes Methods
        public IDataReader GetDrawingTypes()
        {
            return (IDataReader)Database.ExecuteReader("DrawingTypesList");
        }

        public IDataReader GetDrawingTypes(int? tradeId)
        {
            return (IDataReader)Database.ExecuteReader("DrawingTypesGetByTrade", CheckDBNull(tradeId));
        }

        public IDataReader GetDrawingType(int? DrawingTypeId)
        {
            return (IDataReader)Database.ExecuteReader("DrawingTypesGet", CheckDBNull(DrawingTypeId));
        }

        public void UpdateDrawingType(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("DrawingTypesUpdate", parameters);
        }

        public int? AddDrawingType(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("DrawingTypesAdd", parameters));
        }

        public void DeleteDrawingType(int? DrawingTypeId)
        {
            Database.ExecuteNonQuery("DrawingTypesDelete", CheckDBNull(DrawingTypeId));
        }
        #endregion

        #region Drawing Revisions Methods
        public IDataReader GetDrawingRevisionsByDrawing(int? DrawingId)
        {
            return (IDataReader)Database.ExecuteReader("DrawingRevisionsGetByDrawing", CheckDBNull(DrawingId));
        }

        public IDataReader GetDrawingRevisionsByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("DrawingRevisionsGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetDrawingRevision(int? DrawingRevisionId)
        {
            return (IDataReader)Database.ExecuteReader("DrawingRevisionsGet", CheckDBNull(DrawingRevisionId));
        }

        public void UpdateDrawingRevision(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("DrawingRevisionsUpdate", parameters);
        }

        public int? AddDrawingRevision(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("DrawingRevisionsAdd", parameters));
        }

        public void DeleteDrawingRevision(int? DrawingRevisionId)
        {
            Database.ExecuteNonQuery("DrawingRevisionsDelete", CheckDBNull(DrawingRevisionId));
        }

        public void UpdateDrawingRevisionFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("DrawingRevisionsUpdateFiles", parameters);
        }
        #endregion

        #region Drawings Methods
        public IDataReader GetDrawingsByProject(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("DrawingsGetByProject", parameters);
        }

        public IDataReader GetDrawingsByTrade(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("DrawingsGetByTrade", CheckDBNull(TradeId));
        }

        public IDataReader GetDrawing(int? DrawingId)
        {
            return (IDataReader)Database.ExecuteReader("DrawingsGet", CheckDBNull(DrawingId));
        }

        public void UpdateDrawing(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("DrawingsUpdate", parameters);
        }

        public int? AddDrawing(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("DrawingsAdd", parameters));
        }

        public void DeleteDrawing(int? DrawingId)
        {
            Database.ExecuteNonQuery("DrawingsDelete", CheckDBNull(DrawingId));
        }
        #endregion

        #region Transmittals Methods
        public IDataReader GetTransmittalsByDrawing(int? DrawingId)
        {
            return (IDataReader)Database.ExecuteReader("TransmittalsGetByDrawing", CheckDBNull(DrawingId));
        }

        public IDataReader GetTransmittalsByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("TransmittalsGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetTransmittalsByProjectAndSubContractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TransmittalsGetByProjectAndSubContractor", parameters);
        }

        public IDataReader GetTransmittal(int? TransmittalId)
        {
            return (IDataReader)Database.ExecuteReader("TransmittalsGet", CheckDBNull(TransmittalId));
        }

        public void UpdateTransmittal(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsUpdate", parameters);
        }

        public void UpdateTransmittalSentDate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsUpdateSentDate", parameters);
        }

        public int? AddTransmittal(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("TransmittalsAdd", parameters));
        }

        public void DeleteTransmittal(int? TransmittalId)
        {
            Database.ExecuteNonQuery("TransmittalsDelete", CheckDBNull(TransmittalId));
        }

        public IDataReader GetTransmittalRevisionsByTransmittal(int? TransmittalId)
        {
            return (IDataReader)Database.ExecuteReader("TransmittalRevisionsGetByTransmittal", CheckDBNull(TransmittalId));
        }

        public void AddTransmittalRevision(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsDrawingRevisionsAdd", parameters);
        }

        public void UpdateTransmittalRevision(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsDrawingRevisionsUpdate", parameters);
        }

        public void DeleteTransmittalRevision(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsDrawingRevisionsDelete", parameters);
        }

        public IDataReader GetContactsByTransmittal(int? TransmittalId)
        {
            return (IDataReader)Database.ExecuteReader("TransmittalsPeopleGetByTransmittal", CheckDBNull(TransmittalId));
        }

        public void AddTransmittalContact(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsPeopleAdd", parameters);
        }

        public void DeleteTransmittalContact(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsPeopleDelete", parameters);
        }

        //#----
        public IDataReader GetClientContactsByTransmittal(int? TransmittalId)
        {
            return (IDataReader)Database.ExecuteReader("TransmittalsClientContactGetByTransmittal", CheckDBNull(TransmittalId));
        }

        public void AddTransmittalClientContact(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsClientContactAdd", parameters);
        }

        public void DeleteTransmittalClientContact(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TransmittalsClientContactDelete", parameters);
        }

        //#----

        #endregion

        #region Participation Items Methods
        public IDataReader GetParticipationItems(int? TradeItemId)
        {
            return (IDataReader)Database.ExecuteReader("ParticipationItemsGetByTradeItem", CheckDBNull(TradeItemId));
        }

        public IDataReader GetParticipationItemsByTradeParticipation(int? TradeParticipationId)
        {
            return (IDataReader)Database.ExecuteReader("ParticipationItemsGetByTradeParticipation", CheckDBNull(TradeParticipationId));
        }

        public IDataReader GetParticipationItemsByTradeItemCategory(int? TradeItemCategoryId)
        {
            return (IDataReader)Database.ExecuteReader("ParticipationItemsGetByTradeItemCategory", CheckDBNull(TradeItemCategoryId));
        }

        public IDataReader GetParticipationItem(int? ParticipationItemId)
        {
            return (IDataReader)Database.ExecuteReader("ParticipationItemsGet", CheckDBNull(ParticipationItemId));
        }

        public void UpdateParticipationItem(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ParticipationItemsUpdate", parameters);
        }

        public int? AddParticipationItem(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ParticipationItemsAdd", parameters));
        }
        #endregion

        #region Trade Items Methods
        public IDataReader GetTradeItems(int? TradeItemCategoryId)
        {
            return (IDataReader)Database.ExecuteReader("TradeItemsGetByCategory", CheckDBNull(TradeItemCategoryId));
        }

        public IDataReader GetTradeItem(int? TradeItemId)
        {
            return (IDataReader)Database.ExecuteReader("TradeItemsGet", CheckDBNull(TradeItemId));
        }

        public void UpdateTradeItem(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeItemsUpdate", parameters);
        }

        public int? AddTradeItem(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("TradeItemsAdd", parameters));
        }

        public void DeleteTradeItem(int? TradeItemId)
        {
            Database.ExecuteNonQuery("TradeItemsDelete", CheckDBNull(TradeItemId));
        }
        #endregion

        #region Trade Item Categories Methods
        public IDataReader GetTradeItemCategories(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("TradeItemCategoriesGetByTrade", CheckDBNull(TradeId));
        }

        public IDataReader GetTradeItemCategory(int? TradeItemCategoryId)
        {
            return (IDataReader)Database.ExecuteReader("TradeItemCategoriesGet", CheckDBNull(TradeItemCategoryId));
        }

        public void UpdateTradeItemCategory(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeItemCategoriesUpdate", parameters);
        }

        public int? AddTradeItemCategory(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("TradeItemCategoriesAdd", parameters));
        }

        public void DeleteTradeItemCategory(int? TradeItemId)
        {
            Database.ExecuteNonQuery("TradeItemCategoriesDelete", CheckDBNull(TradeItemId));
        }
        #endregion

        #region Trades Methods
        public IDataReader GetTrades(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("TradesGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetTradesBasic(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("TradesGetBasicByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetTradesForDrawing(int? DrawingId)
        {
            return (IDataReader)Database.ExecuteReader("TradesGetByDrawing", CheckDBNull(DrawingId));
        }

        public IDataReader GetTradesForDrawingType(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradesGetByDrawingType", parameters);
        }

        public IDataReader GetTrade(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("TradesGet", CheckDBNull(TradeId));
        }

        //#---Get Trade-Itemcategories-Items-Scope of Work------

        public IDataReader GetTrade_Categories_Item(string TradeCode)
        {
            return (IDataReader)Database.ExecuteReader("TradesItemCategoriesItems", CheckDBNull(TradeCode));

        }

        //#---------------


        //#---Get Rating A subcontractors------

        public IDataReader GetRatingASubContractors(string[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("Trades_RatingA_Subcontractors", parameters);

        }


        //#---------------



        //#---Get ALL the Rating A subcontractors------
        public IDataReader GetALLRatingASubContractors()
        {

            return (IDataReader)Database.ExecuteReader("Trades_RatingA_ALLSubcontractors");

        }

        //#---------------



        //#---Get Subcontractor Variations------
        public IDataReader GetSubContractorsVariations(string[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubcontractorsVariations", parameters);

        }
        //#---Get Subcontractor Variations----




        //#---Get Subcontractor Variations By Type------
        public IDataReader GetSubContractorsVariationsByType(string[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubcontractorsVariationsByType", parameters);

        }
        //#---Get Subcontractor Variations  By Type----




        //#---Get SubContractors Missing Insurance Link-----
        public IDataReader GetSubContractorsMissingInsuranceLink(string[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubcontractorsMissingInsuranceLink", parameters);

        }
        //#---GetSubContractorsMissingInsuranceLink






        //#------
        public IDataReader GetMissingSignedContractFileContracts(Object[] parametrs)
        {
            CheckDBNull(parametrs);
            return (IDataReader)Database.ExecuteReader("Missing_SignedContractFileContracts", parametrs);

        }



        //#----

        public IDataReader GetCVStatusReportData(Object[] parametrs)
        {
            CheckDBNull(parametrs);
            return (IDataReader)Database.ExecuteReader("ClientVariationsStatusReportDataGetByProject", parametrs);

        }

        









        public void UpdateTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesUpdate", parameters);
        }

        public void UpdateTradeDisplayOrder(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesUpdateDisplayOrder", parameters);
        }

        //#-----SignedContractFile--------
        public void UpdateTradeSingnedContractFile(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesUpdateSignedContractFile", parameters);
        }
        //#------------------


        public void UpdateTradeDates(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesUpdateDates", parameters);
        }

        public int? AddTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("TradesAdd", parameters));
        }

        public void DeleteTrade(int? TradeId)
        {
            Database.ExecuteNonQuery("TradesDelete", CheckDBNull(TradeId));
        }

        public void SetTradeContract(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesSetContract", parameters);
        }

        public void SetTradeWorkOrder(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesSetWorkOrder", parameters);
        }

        public void SetTradeComparisonApprovalInfo(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesSetComparisonApprovalInfo", parameters);
        }

        public void AddTradeDrawingType(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesDrawingTypesAdd", parameters);
        }

        public void DeleteTradeDrawingType(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesDrawingTypesDelete", parameters);
        }

        public void AddTradeDrawing(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesDrawingsAdd", parameters);
        }

        public void DeleteTradeDrawing(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesDrawingsDelete", parameters);
        }

        public IDataReader SearchTrades(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradesSearch", parameters);
        }

        public IDataReader SearchWorkOrders(String projectFilter)
        {
            return (IDataReader)Database.ExecuteReader("TradesSearchWorkOrders", CheckDBNull(projectFilter));
        }

        public void UpdateTradeFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradesUpdateFiles", parameters);
        }
#endregion

        #region Trade Templates Methods
        public IDataReader GetTradeTemplates()
        {
            return (IDataReader)Database.ExecuteReader("TradeTemplatesList");
        }

        public IDataReader GetTradeTemplateSubcontractors(int? TradeTemplateId)
        {
            return (IDataReader)Database.ExecuteReader("TradeTemplatesSubContractorsGetByTradeTemplate", CheckDBNull(TradeTemplateId));
        }
        public IDataReader GetTradeTemplateSubcontractorsBU(int? TradeTemplateId, int? BusinessUnitId)  // DS20231108
        {
            return (IDataReader)Database.ExecuteReader("TradeTemplatesSubContractorsGetByTradeTemplateBU", CheckDBNull(TradeTemplateId), CheckDBNull(BusinessUnitId));
        }

        public IDataReader GetTradeTemplate(int? TradeTemplateId)
        {
            return (IDataReader)Database.ExecuteReader("TradeTemplatesGet", CheckDBNull(TradeTemplateId));
        }
        public IDataReader GetTradeTemplateFromCode(string TypeCode)   // DS20230308
        {
            return (IDataReader)Database.ExecuteReader("TradeTemplatesGetFromCode", CheckDBNull(TypeCode));
        }

        public void UpdateTradeTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeTemplatesUpdate", parameters);
        }

        public int? AddTradeTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("TradeTemplatesAdd", parameters));
        }

        public void DeleteTradeTemplate(int? TradeTemplateId)
        {
            Database.ExecuteNonQuery("TradeTemplatesDelete", CheckDBNull(TradeTemplateId));
        }

        public void AddTradeTemplateSubcontractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeTemplatesSubContractorsAdd", parameters);
        }

        public void DeleteTradeTemplateSubcontractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeTemplatesSubContractorsDelete", parameters);
        }

        public void CopyTradeItemCategories(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeTemplatesCopyToTrade", parameters);
        }
#endregion

        #region Trade Participations Methods
        public IDataReader GetTradeParticipations(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradeParticipationsGetByTrade", parameters);
        }

        public IDataReader GetTradeParticipationBudgetAndSelected(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("TradeParticipationsGetBudgetAndSelectedByTrade", CheckDBNull(TradeId));
        }

        public IDataReader GetTradeParticipation(int? TradeParticipationId)
        {
            return (IDataReader)Database.ExecuteReader("TradeParticipationsGet", CheckDBNull(TradeParticipationId));
        }

        public IDataReader GetQuoteTradeParticipation(int? TradeParticipationId)
        {
            return (IDataReader)Database.ExecuteReader("TradeParticipationsGetIdByComparisonId", CheckDBNull(TradeParticipationId));
        }

        public void UpdateTradeParticipation(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeParticipationsUpdate", parameters);
        }

        public void UpdateTradeParticipationOpenDate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeParticipationsUpdateOpenDate", parameters);
        }

        public void UpdateTradeParticipationQuoteFile(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeParticipationsUpdateQuoteFile", parameters);
        }

        public void UpdateTradeParticipationReminderDate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("TradeParticipationsUpdateReminderDate", parameters);
        }

        public int? AddTradeParticipation(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("TradeParticipationsAdd", parameters));
        }

        public void DeleteTradeParticipation(int? TradeItemId)
        {
            Database.ExecuteNonQuery("TradeParticipationsDelete", CheckDBNull(TradeItemId));
        }

        public IDataReader GetTradeParticipationsForSubContractor(int? TradeParticipationId)
        {
            return (IDataReader)Database.ExecuteReader("TradeParticipationsGetForSubContractor", CheckDBNull(TradeParticipationId));
        }

        public IDataReader SearchTradeParticipationsBySubContractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradeParticipationsSearchBySubContractor", parameters);
        }

        public int SearchTradeParticipationsBySubContractorCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("TradeParticipationsSearchBySubContractorCountGet", parameters));
        }

        public IDataReader GetActiveTradeIds(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradeParticipationsGetActiveByTrades", parameters);
        }
#endregion

        #region People Methods
        public IDataReader ListPeople(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("PeopleGetByProjectStatusAndBusinessUnit", parameters);
        }

        public IDataReader ListPeopleWithApprovalRoles()
        {
            return (IDataReader)Database.ExecuteReader("PeopleListApprovalRoles");
        }

        public IDataReader SearchEmployee(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("PeopleSearchEmployee", parameters);
        }

        public int SearchEmployeeCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("PeopleSearchEmployeeCountGet", parameters));
        }

        public IDataReader SearchContact(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("PeopleSearchContact", parameters);
        }


        public int SearchContactCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("PeopleSearchContactCountGet", parameters));
        }

       
        //#---
        public IDataReader SearchClientContact(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("PeopleSearchClientContact", parameters);
        }
        public IDataReader SearchClientContactSub(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("PeopleSearchClientContactSub", parameters);
        }
        public IDataReader SearchClientContactProj(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("PeopleSearchClientContactProj", parameters);
        }
        public int SearchClientContactCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("PeopleSearchContactCountGet", parameters));
        }
        //#--
        public int SearchClientContactCountProj(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("PeopleSearchContactCountGetProj", parameters));
        }

        public int SearchClientContactCountSub(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("PeopleSearchContactCountGetSub", parameters));
        }

        public IDataReader GetPerson(Object[] parameters)
        {
            CheckDBNull(parameters);
            
            return (IDataReader)Database.ExecuteReader("PeopleGet", parameters);
        }

        public void UpdatePerson(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("PeopleUpdate", parameters);
        }

        public void UpdatePersonLastLogin(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("PeopleUpdateLastLogin", parameters);
        }

        public int? AddPerson(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("PeopleAdd", parameters));
        }

        public void DeletePerson(int? peopleId)
        {
            Database.ExecuteNonQuery("PeopleDelete", CheckDBNull(peopleId));
        }

        //#---Client Access
        public void AddClientAccess(Object[] parameters)
        {
            Database.ExecuteNonQuery("ClientAccessAdd", parameters);
        }

        public Boolean RegisteredClientAccess(Object[] parameters)
        {
            int cnt= (int)Convert.ToInt32(Database.ExecuteScalar("ClientAccessRegistered", parameters));
            if (cnt > 0) return true;
            else return false;
        }

        public void UpdateClientDistAccess(Object[] parameters)
        {
            Database.ExecuteNonQuery("ClientAccessDistUpdate", parameters);
        }


        public void UpdateClientWebAccess(Object[] parameters)
        {
            Database.ExecuteNonQuery("ClientAccessWebUpdate", parameters);
        }

        public IDataReader GetClientAccess(Object[] parameters)
        {
            CheckDBNull(parameters);

            return (IDataReader)Database.ExecuteReader("ClientAccessGet", parameters);
        }

        public IDataReader GetClientProjects(Object[] parameters)
        {
            CheckDBNull(parameters);

            return (IDataReader)Database.ExecuteReader("ClientProjectsGet", parameters);
        }


        //#--




        #endregion

        #region SubContractor Methods
        public IDataReader ListSubContractors(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubContractorsList", parameters);
        }

        public int SearchSubContractorsCountProj(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("SubContractorsSearchCountProjGet", parameters));
        }
        public int SearchSubContractorsCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("SubContractorsSearchCountGet", parameters));
        }

        public IDataReader SearchSubContractorsProj(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubContractorsSearchProj", parameters);
        }
        public IDataReader SearchSubContractors(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubContractorsSearch", parameters);
        }

        public IDataReader GetSubContractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubContractorsGet", parameters);
        }

        //#-----
        public IDataReader GetSubContractorByABN(String parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubContractorsGetByABN", parameters);
        }
        //#------



        public void UpdateSubContractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("SubContractorsUpdate", parameters);
        }

        public int? AddSubContractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("SubContractorsAdd", parameters));
        }
        //#---
        public void AddBusinessUnitsToSubContractor(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteScalar("SubcontractorsBusinessUnitListAdd", parameters);
        }


        public IDataReader GetSubContractorBusinessUnitList(int? subContractorId)
        {
            return (IDataReader)Database.ExecuteReader("SubcontractorsBusinessUnitListGet", CheckDBNull(subContractorId));

        }

        public void DeleteSubContractorBusinessUnitList(int? subContractorId)
        {
           Database.ExecuteScalar("SubcontractorsBusinessUnitListDelete", CheckDBNull(subContractorId));

        }

        //#---

        public void DeleteSubContractor(int? subContractorId)
        {
            Database.ExecuteNonQuery("SubContractorsDelete", CheckDBNull(subContractorId));
        }

        public IDataReader GetSubContractorContacts(int? subContractorId)
        {
            return (IDataReader)Database.ExecuteReader("PeopleGetBySubContractor", CheckDBNull(subContractorId));
        }

        #endregion

      //#----
        #region Qualifications
        public IDataReader GetQualificationsByContactId(int? contactId)
        {
           
            return (IDataReader)Database.ExecuteReader("QualificationsGetByContactId", CheckDBNull(contactId));
        }

        public void UpdateQualifiaction(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("QualificationUpdate", parameters);
        }

        public void UpdateQualifiactionFile(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("QualificationUpdateFile", parameters);
        }

        public int? AddQualification(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("QualificationAdd", parameters));
        }

        public void DeleteQualification(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("QualificationDelete", parameters);
        }

        #endregion
        //#----


        #region ContractTemplate
        public IDataReader GetContractTemplates()
        {
            return (IDataReader)Database.ExecuteReader("ContractTemplatesList");
        }

        public IDataReader GetContractTemplate(int? ContractTemplateId)
        {
            return (IDataReader)Database.ExecuteReader("ContractTemplatesGet", CheckDBNull(ContractTemplateId));
        }

        public IDataReader GetContractTemplate(int? JobTypeId, int? BusinessUnitId)
        {
            return (IDataReader)Database.ExecuteReader("ContractTemplatesGetByJtBu", CheckDBNull(JobTypeId), CheckDBNull(BusinessUnitId));
        }

        public void UpdateContractTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ContractTemplatesUpdate", parameters);
        }

        public int? AddContractTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ContractTemplatesAdd", parameters));
        }

        public void DeleteContractTemplate(int? ContractTemplateId)
        {
            Database.ExecuteNonQuery("ContractTemplatesDelete", CheckDBNull(ContractTemplateId));
        }
#endregion

#region Contracts
        public IDataReader GetContracts(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("ContractsGetByTrade", CheckDBNull(TradeId));
        }

        public IDataReader GetSubContracts(int? ContractId)
        {
            return (IDataReader)Database.ExecuteReader("ContractsGetByContract", CheckDBNull(ContractId));
        }

        public IDataReader GetContract(int? ContractId)
        {
            return (IDataReader)Database.ExecuteReader("ContractsGet", CheckDBNull(ContractId));
        }

        public void UpdateContract(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ContractsUpdate", parameters);
        }

        public void UpdateContractGeneralInfo(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ContractsUpdateGeneralInfo", parameters);
        }

        public void UpdateTemplateInContract(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ContractsUpdateTemplate", parameters);
        }

        public void SetContractApprovalDate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ContractsSetApprovalDate", parameters);
        }

        public int? AddContract(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ContractsAdd", parameters));
        }

        public void DeleteContract(int? ContractId)
        {
            Database.ExecuteNonQuery("ContractsDelete", CheckDBNull(ContractId));
        }

        public void UpdateContractFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ContractsUpdateFiles", parameters);
        }
#endregion

#region ContractModification
        public IDataReader GetContractModifications(int? ContractId)
        {
            return (IDataReader)Database.ExecuteReader("ContractModificationsGetByContract", CheckDBNull(ContractId));
        }

        public IDataReader GetContractModification(int? ContractModificationId)
        {
            return (IDataReader)Database.ExecuteReader("ContractModificationsGet", CheckDBNull(ContractModificationId));
        }

        public int? AddContractModification(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ContractModificationsAdd", parameters));
        }
#endregion

#region Variations
        public IDataReader GetVariations(int? VariationId)
        {
            return (IDataReader)Database.ExecuteReader("VariationsGetByContract", CheckDBNull(VariationId));
        }

        public IDataReader GetVariation(int? VariationId)
        {
            return (IDataReader)Database.ExecuteReader("VariationsGet", CheckDBNull(VariationId));
        }

        public void UpdateVariation(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("VariationsUpdate", parameters);
        }

        public int? AddVariation(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("VariationsAdd", parameters));
        }

        public void DeleteVariation(int? VariationId)
        {
            Database.ExecuteNonQuery("VariationsDelete", CheckDBNull(VariationId));
        }

        public void UpdateVariationClientVariationTradeId(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("VariationsUpdateClientVariationTradeId", parameters);
        }
#endregion

#region Projects Mehotds
        public IDataReader ListProjects()
        {
            return (IDataReader)Database.ExecuteReader("ProjectsList");
        }

        public IDataReader ListActiveProjects()
        {
            return (IDataReader)Database.ExecuteReader("ProjectsListActive");
        }

        public int SearchProjectCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("ProjectsSearchCountGet", parameters));
        }

        public IDataReader SearchProjects(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ProjectsSearch", parameters);
        }

        public IDataReader GetProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGet", CheckDBNull(ProjectId));
        }

        public IDataReader GetProjectPeople(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGetPeople", CheckDBNull(ProjectId));
        }

        public IDataReader GetActiveProjects(int? PeopleId)  // DS20230823
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGetActive", CheckDBNull(PeopleId));
        }

        public IDataReader GetActiveProjectsForEmployee(int? PeopleId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGetActiveByPeople", CheckDBNull(PeopleId));
        }

        public IDataReader GetProjectsPeopleNames(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGetPeopleInfo", CheckDBNull(ProjectId));
        }

        public IDataReader GetProjectTradesCAs(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradesGetCAsByProjectAndPM", parameters);
        }

        public IDataReader GetProjectTradesCAsWithNoPM(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("TradesGetCAsWithNoPMsByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetTradesForSubcontractorAndProject(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradesGetByProjectAndSubcontractor", parameters);
        }

        public void UpdateProject(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProjectsUpdate", parameters);
        }

        public void UpdateProjectFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProjectsUpdateFiles", parameters);
        }

        public int? AddProject(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ProjectsAdd", parameters));
        }

        public void DeleteProject(int? ProjectId)
        {
            Database.ExecuteNonQuery("ProjectsDelete", CheckDBNull(ProjectId));
        }

        public IDataReader GetFilesForProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsRelatedTablesGetFileNames", CheckDBNull(ProjectId));
        }

        public IDataReader GetProjectAll(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGetAll", CheckDBNull(ProjectId));
        }

        //#---Project SnapShot report
        public IDataReader GetProjectSnapShot(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGetSnapShot", CheckDBNull(ProjectId));

        }

        public IDataReader GetProjectCVSA(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectsGetCVSA", CheckDBNull(ProjectId));

        }

        public IDataReader GetWorkingDaysInPercent(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ProjectsGetWorkingDaysInPercentage", parameters);
        }


        //#---
        #endregion

 #region Process Methods
        public IDataReader GetProcess(int? ProcessId)
        {
            return (IDataReader)Database.ExecuteReader("ProcessesGet", CheckDBNull(ProcessId));
        }

        public IDataReader GetProcessInfoForTrade(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("TradesGetProcessInfo", CheckDBNull(TradeId));
        }

        public IDataReader GetProcessInfoForContract(int? ContractId)
        {
            return (IDataReader)Database.ExecuteReader("ContractsGetProcessInfo", CheckDBNull(ContractId));
        }

        public IDataReader GetProcessInfoForClientVariation(int? ClientVariationId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsGetProcessInfo", CheckDBNull(ClientVariationId));
        }

        public IDataReader GetProcessInfoForClaim(int? ClaimId)
        {
            return (IDataReader)Database.ExecuteReader("ClaimsGetProcessInfo", CheckDBNull(ClaimId));
        }

        public IDataReader GetProcessTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ProcessesGetTemplate", parameters);
        }

        public IDataReader GetProcessTemplates()
        {
            return (IDataReader)Database.ExecuteReader("ProcessesGetTemplates");
        }

        public IDataReader GetProcessTemplatesForBusinessUnit(int? BusinessUnitId)
        {
            return (IDataReader)Database.ExecuteReader("BusinessUnitsJobTypesProcessesGet", CheckDBNull(BusinessUnitId));
        }

        public IDataReader GetProcessTemplatesByType(String templateType)
        {
            return (IDataReader)Database.ExecuteReader("ProcessesGetByType", CheckDBNull(templateType));
        }
        
        public IDataReader GetPendingSteps(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ProcessesGetByProjectAndUserType", parameters);
        }


        //#--------------

        public IDataReader GetPendingStepsForActivityReport(Object[] parameters)
        {
            CheckDBNull(parameters);
           
             return (IDataReader)Database.ExecuteReader("ProcessesGetByProjectAndUserType_ActivitySummaryReport", parameters);
        }

        //#--------------






        public IDataReader GetExecutedSteps(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ProcessesGetCompletedByProjectAndUser", parameters);
        }

        public void DeleteProcessTemplates(int? BusinessUnitId)
        {
            Database.ExecuteNonQuery("BusinessUnitsJobTypesProcessesDelete", CheckDBNull(BusinessUnitId));
        }

        public void AddProcessTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteScalar("BusinessUnitsJobTypesProcessesAdd", parameters);
        }

        public int? AddProcess(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ProcessesAdd", parameters));
        }

        public void UpdateProcessStepComparisonApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProcessesUpdateStepComparisonApproval", parameters);
        }
        // DS0240321
        public void UpdateProcessStepContractApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProcessesUpdateStepContractApproval", parameters);
        }

        //#--14-11-2018
        public void UpdateProcessHideorShow(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProcessesUpdateHideorShow", parameters);
        }
        //#--14-11-2018

        public void DeleteProcess(int? ProcessId)
        {
            Database.ExecuteNonQuery("ProcessesDelete", CheckDBNull(ProcessId));
        }
#endregion

#region Step Methods
        public IDataReader GetProcessStep(int? StepId)
        {
            return (IDataReader)Database.ExecuteReader("ProcessStepsGet", CheckDBNull(StepId));
        }

        public IDataReader GetProcessStepsByProcess(int? ProcessId)
        {
            return (IDataReader)Database.ExecuteReader("ProcessStepsGetByProcess", CheckDBNull(ProcessId));
        }

        public void UpdateProcessStep(Object[] parameters)
        {
            CheckDBNull(parameters);
                        
            Database.ExecuteNonQuery("ProcessStepsUpdate", parameters);



        }

        public void UpdateProcessStepSkip(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProcessStepsUpdateSkip", parameters);
        }

        //#----------to Update Process Step Skip and StepComparison Approval, StepContractApproval
        //For Contract
        public void UpdateProcessStepSkipStepContractApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProcessStepsUpdateSkipContractApproval", parameters);
        }

        //For Comparison
        public void UpdateProcessStepSkipStepComparisonApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProcessStepsUpdateSkipComparisonApproval", parameters);
        }

        //#----------to Update Process Step Skip and StepComparison Approval, StepContractApproval


        public int? AddProcessStep(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ProcessStepsAdd", parameters));
        }
#endregion

#region Reversal Methods
        public IDataReader GetReversalsByProcess(int? ProcessId)
        {
            return (IDataReader)Database.ExecuteReader("ReversalsGetByProcess", CheckDBNull(ProcessId));
        }

        public void UpdateReversal(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ReversalsUpdate", parameters);
        }

        public int? AddReversal(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ReversalsAdd", parameters));
        }
#endregion

#region Minutes templates
        public IDataReader GetMinutesTemplate(int? minutesTemplateId)
        {
            return (IDataReader)Database.ExecuteReader("MinutesTemplatesGet", CheckDBNull(minutesTemplateId));
        }

        public void UpdateMinutesTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("MinutesTemplatesUpdate", parameters);
        }
#endregion

#region Invitation templates
        public IDataReader GetInvitationTemplate(int? invitationTemplateId)
        {
            return (IDataReader)Database.ExecuteReader("InvitationTemplatesGet", CheckDBNull(invitationTemplateId));
        }

        public void UpdateInvitationTemplate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InvitationTemplatesUpdate", parameters);
        }
#endregion

#region Work Orders Methods
        public int? AddWorkOrder(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("WorkOrdersAdd", parameters));
        }
#endregion

#region Holidays
        public IDataReader GetHolidays()
        {
            return (IDataReader)Database.ExecuteReader("HolidaysList");
        }

        
        public void AddHoliday(DateTime dateTime)
        {
            Database.ExecuteNonQuery("HolidaysAdd", dateTime);
        }

        public void DeleteHoliday(DateTime dateTime)
        {
            Database.ExecuteNonQuery("HolidaysDelete", dateTime);
        }
#endregion

#region RDOs
        public IDataReader GetRDOs()
        {
            return (IDataReader)Database.ExecuteReader("RDOsList");
        }

        public void AddRDO(DateTime dateTime)
        {
            Database.ExecuteNonQuery("RDOsAdd", dateTime);
        }

        public void DeleteRDO(DateTime dateTime)
        {
            Database.ExecuteNonQuery("RDOsDelete", dateTime);
        }
#endregion

#region Reports
        public IDataReader GetBidsChart(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradeParticipationsGetRanks", parameters);
        }

        public IDataReader GetTradesReport(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TradesGetByBusinessUnitProjectSubcontractorAndPeople", parameters);
        }

          //#--TurnOberVsTime Report
        public IDataReader GetTurnOverVsTime(Object[] parameters) {

            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("TurnoverVsTime", parameters);

        }

        //#--KPIAnalysis Report
        public IDataReader GetKPIAnalysis(Object[] parameters)
        {

            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("KPIAnalysis_New", parameters);

        }
        //#---Report SubcontractorsVvDV
        public IDataReader GetSubcontractorsVvDV(Object[] parameters)
        {

            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SubcontractorsVvDV", parameters);

        }



        //#---Report GetDesignVariation
        public IDataReader GetDesignVariation(Object[] parameters)
        {

            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("DesignVariation", parameters);

        }


        #endregion

#region ClientTrades Methods
        public IDataReader GetClientTradesByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ClientTradesGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetClientTrade(int? ClientTradeId)
        {
            return (IDataReader)Database.ExecuteReader("ClientTradesGet", CheckDBNull(ClientTradeId));
        }

        public void UpdateClientTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientTradesUpdate", parameters);
        }

        public int? AddClientTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClientTradesAdd", parameters));
        }

        public void DeleteClientTrade(int? ClientTradeId)
        {
            Database.ExecuteNonQuery("ClientTradesDelete", CheckDBNull(ClientTradeId));
        }
#endregion

#region Budget Methods
        public IDataReader GetBudgetsByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("BudgetsGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetBudget(int? BudgetId)
        {
            return (IDataReader)Database.ExecuteReader("BudgetsGet", CheckDBNull(BudgetId));
        }

        public void UpdateBudget(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("BudgetsUpdate", parameters);
        }

        public int? AddBudget(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("BudgetsAdd", parameters));
        }

        public void DeleteBudget(int? ClientTradeId)
        {
            Database.ExecuteNonQuery("BudgetsDelete", CheckDBNull(ClientTradeId));
        }

        public IDataReader GetBudgetByProject(int? ProjectId, Boolean includeExisting)
        {
            return (IDataReader)Database.ExecuteReader("BudgetGetByProject", CheckDBNull(ProjectId), includeExisting);
        }
#endregion

#region Budgets trades Methods
        public IDataReader GetTradeBudgets(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("BudgetsTradesGetByTrade", CheckDBNull(TradeId));
        }

        public IDataReader GetTradeBudget(int? BudgetId, int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("BudgetsTradesGet", CheckDBNull(BudgetId), CheckDBNull(TradeId));
        }

        public void UpdateTradeBudget(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("BudgetsTradesUpdate", parameters);
        }

        public int? AddTradeBudget(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("BudgetsTradesAdd", parameters));
        }

        public void DeleteTradeBudget(int? BudgetId, int? TradeId)
        {
            Database.ExecuteNonQuery("BudgetsTradesDelete", CheckDBNull(BudgetId), CheckDBNull(TradeId));
        }
#endregion

#region ClientTradeTypes Methods
        public IDataReader GetClientTradeTypes()
        {
            return (IDataReader)Database.ExecuteReader("ClientTradeTypesList");
        }

        public IDataReader GetClientTradeType(int? ClientTradeTypeId)
        {
            return (IDataReader)Database.ExecuteReader("ClientTradeTypesGet", CheckDBNull(ClientTradeTypeId));
        }

        public void UpdateClientTradeType(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientTradeTypesUpdate", parameters);
        }

        public int? AddClientTradeType(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClientTradeTypesAdd", parameters));
        }

        public void DeleteClientTradeType(int? ClientTradeTypeId)
        {
            Database.ExecuteNonQuery("ClientTradeTypesDelete", CheckDBNull(ClientTradeTypeId));
        }
#endregion

#region Client Variations Methods
        public IDataReader GetClientVariationsByProject(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ClientVariationsGetByProject", parameters);
        }

        public IDataReader GetClientVariationsByClientVariation(int? ClientVariationId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsGetByClientVariation", CheckDBNull(ClientVariationId));
        }

        public IDataReader GetClientVariation(int? ClientVariationId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsGet", CheckDBNull(ClientVariationId));
        }

        public IDataReader GetClientVariationByNumber(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ClientVariationsGetByNumber", parameters);
        }

        public void UpdateClientVariation(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdate", parameters);
        }

        public void UpdateClientVariationInternalApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdateInternalApproval", parameters);
        }

        public void UpdateClientVariationVerbalApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdateVerbalApproval", parameters);
        }

        public void UpdateClientVariationInvoiceSent(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdateInvoiceSent", parameters);
        }

        public void UpdateClientVariationWorksCompleted(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdateWorksCompleted", parameters);
        }

        public void UpdateClientVariationFinalApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdateApproval", parameters);
        }

        public void UpdateClientVariationCancel(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdateCancel", parameters);
        }

        public int? AddClientVariation(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClientVariationsAdd", parameters));
        }

        public void DeleteClientVariation(int? ClientVariationId)
        {
            Database.ExecuteNonQuery("ClientVariationsDelete", CheckDBNull(ClientVariationId));
        }

        public int? GetClientVariationNumber(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClientVariationsGetNextNumber", parameters));
        }

        public IDataReader GetClientVariationsByDate(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ClientVariationsGetByDate", parameters);
        }

        public IDataReader GetClientVariationMaxId(int? ParentClientVariationId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsGetMaxId", CheckDBNull(ParentClientVariationId));
        }

        public void UpdateClientVariationFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsUpdateFiles", parameters);
        }
#endregion

#region Client Variation Items Methods
        public IDataReader GetClientVariationItemsByClientVariation(int? ClientVariationId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsDetailsGetByClientVariation", CheckDBNull(ClientVariationId));
        }

        public IDataReader GetClientVariationItem(int? ClientVariationItemId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsDetailsGet", CheckDBNull(ClientVariationItemId));
        }

        public void UpdateClientVariationItem(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsDetailsUpdate", parameters);
        }
        public void UpdateClientVariationSkipCVSUM(Object[] parameters)  //DS20231125
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ProcessStepsUpdateSkipCVSUM", parameters);
        }

        public int? AddClientVariationItem(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClientVariationsDetailsAdd", parameters));
        }

        public void DeleteClientVariationItem(int? ClientVariationItemId)
        {
            Database.ExecuteNonQuery("ClientVariationsDetailsDelete", CheckDBNull(ClientVariationItemId));
        }
#endregion

#region Claims Methods
        public IDataReader GetClaimsByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ClaimsGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetClaim(int? ClaimId)
        {
            return (IDataReader)Database.ExecuteReader("ClaimsGet", CheckDBNull(ClaimId));
        }

        public void UpdateClaim(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsUpdate", parameters);
        }

        public void UpdateClaimDraftApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsUpdateDraftApproval", parameters);
        }

        public void UpdateClaimInternalApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsUpdateInternalApproval", parameters);
        }

        public void UpdateClaimApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsUpdateApproval", parameters);
        }
        public void UpdateClaimBackupFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsUpdateBackupFiles", parameters);
        }

        public int? AddClaim(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClaimsAdd", parameters));
        }

        public void DeleteClaim(int? ClaimId)
        {
            Database.ExecuteNonQuery("ClaimsDelete", CheckDBNull(ClaimId));
        }
        #endregion

#region Induction

  #region  Induction

        public IDataReader GetInductionDocuments(Object[]  parameters)
        {
            CheckDBNull(parameters);           
            return (IDataReader)Database.ExecuteReader("InductionDocumentsGet", parameters);
        }
                                                    
        public int? AddInductionDocuments(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("InductionDocumentsAdd", parameters));
        }

        public void DeleteInductionDocuments(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InductionDocumentsDelete", parameters);
        }




        public IDataReader GetInductionOptinalQAs(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("InductionOptinalQAsGet", parameters);
        }
        public IDataReader GetInductionOptinalQA(int? optionalQAId)
        {
            return (IDataReader)Database.ExecuteReader("InductionOptinalQAsGetById", CheckDBNull(optionalQAId));
        }

        public int? AddOptionalQA(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("InductionOptinalQAAdd", parameters));
        }

        public void UpdateOptionalQA(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InductionOptinalQAUpdate", parameters);
        }
        public void DeleteOptionalQA(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InductionOptinalQADelete", parameters);
        }



        public IDataReader GetInductionYesNoQAs(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("InductionYesNoQAGet", parameters);
        }
        public IDataReader GetInductionYesNoQAs(int? YesNoId)
        {
            return (IDataReader)Database.ExecuteReader("InductionYesNoQAsGetById", CheckDBNull(YesNoId));
        }

        public int? AddYesNoQA(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("InductionYesNoQAAdd", parameters));
        }

        public void UpdateYesNoQA(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InductionYesNoQAUpdate", parameters);
        }
        public void DeleteYesNoQA(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InductionYesNoQADelete", parameters);
        }

       
        public IDataReader GetInductionNote(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("InductionNoteGet", parameters);
        }
       

        public int? AddInductionNote(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("InductionNoteAdd", parameters));
        }

        public void UpdateInductionNote(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InductionNoteUpdate", parameters);
        }
        public void DeleteInductionNote(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("InductionNoteDelete", parameters);
        }






        public int? AddInductionResult(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("InductionResultAdd", parameters));
        }

        public IDataReader GetInductionResultByPeopleId(int? Id)
        {             
            return (IDataReader)Database.ExecuteReader("InductionResultGetByPeopleId", CheckDBNull(Id));
        }

        #endregion




        #endregion




        #region Claim Variations Methods
        public IDataReader GetClaimVariationsByClaim(int? ClaimId)
        {
            return (IDataReader)Database.ExecuteReader("ClaimsClientVariationsGetByClaim", CheckDBNull(ClaimId));
        }

        public IDataReader GetClaimVariation(int? ClaimId, int? ClientVariationId)
        {
            return (IDataReader)Database.ExecuteReader("ClaimsClientVariationsGet", CheckDBNull(ClaimId), CheckDBNull(ClientVariationId));
        }

        public void UpdateClaimVariation(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsClientVariationsUpdate", parameters);
        }

        public void AddClaimVariation(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsClientVariationsAdd", parameters);
        }

        public void DeleteClaimVariation(int? ClaimId, int? ClientVariationId)
        {
            Database.ExecuteNonQuery("ClaimsClientVariationsDelete", CheckDBNull(ClaimId), CheckDBNull(ClientVariationId));
        }
#endregion

#region Claim Trades Methods
        public IDataReader GetClaimTradesByClaim(int? ClaimId)
        {
            return (IDataReader)Database.ExecuteReader("ClaimsClientTradesGetByClaim", CheckDBNull(ClaimId));
        }

        public IDataReader GetClaimTrade(int? ClaimId, int? ClientTradeId)
        {
            return (IDataReader)Database.ExecuteReader("ClaimsClientTradesGet", CheckDBNull(ClaimId), CheckDBNull(ClientTradeId));
        }

        public void UpdateClaimTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsClientTradesUpdate", parameters);
        }

        public void AddClaimTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClaimsClientTradesAdd", parameters);
        }

        public void DeleteClaimTrade(int? ClaimId, int? ClientTradeId)
        {
            Database.ExecuteNonQuery("ClaimsClientTradesDelete", CheckDBNull(ClaimId), CheckDBNull(ClientTradeId));
        }



#endregion

#region Client Variation Trades Methods
        public IDataReader GetClientVariationTradesByClientVariation(int? ClientVariationId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsTradesGetByClientVariation", CheckDBNull(ClientVariationId));
        }

        public IDataReader GetClientVariationTrade(int? ClientVariationTradeId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsTradesGet", CheckDBNull(ClientVariationTradeId));
        }

        public void UpdateClientVariationTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsTradesUpdate", parameters);
        }

        public int? AddClientVariationTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClientVariationsTradesAdd", parameters));
        }

        public void DeleteClientVariationTrade(int? ClientVariationTradeId)
        {
            Database.ExecuteNonQuery("ClientVariationsTradesDelete", CheckDBNull(ClientVariationTradeId));
        }
#endregion
       
#region Client Trades Client Variation trades Methods
        public IDataReader GetTradeClientVariationTrades(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsTradesTradesGetByTrade", CheckDBNull(TradeId));
        }

        public IDataReader GetTradeClientVariationTrade(int? ClientVariationTradeId, int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("ClientVariationsTradesTradesGet", CheckDBNull(ClientVariationTradeId), CheckDBNull(TradeId));
        }

        public void UpdateTradeClientVariationTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsTradesTradesUpdate", parameters);
        }

        public int? AddTradeClientVariationTrade(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ClientVariationsTradesTradesAdd", parameters));
        }

        public void DeleteTradeClientVariationTrade(int? ClientVariationTradeId, int? TradeId)
        {
            Database.ExecuteNonQuery("ClientVariationsTradesTradesDelete", CheckDBNull(ClientVariationTradeId), CheckDBNull(TradeId));
        }

        public void UpdateTradeClientVariationTradeId(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("ClientVariationsTradesTradesUpdateClientVariationTradeId", parameters);
        }
#endregion

#region RFI Methods
        public IDataReader GetRFIsByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("RFIsGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetRFI(int? RFIId)
        {
            return (IDataReader)Database.ExecuteReader("RFIsGet", CheckDBNull(RFIId));
        }

        public void UpdateRFI(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("RFIsUpdate", parameters);
        }

        public void UpdateRFIStatus(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("RFIsUpdateStatus", parameters);
        }

        public int? AddRFI(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("RFIsAdd", parameters));
        }

        public void DeleteRFI(int? RFIId)
        {
            Database.ExecuteNonQuery("RFIsDelete", CheckDBNull(RFIId));
        }

        public void UpdateRFIFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("RFIsUpdateFiles", parameters);
        }

        //#---RFI Response--
        public int GetRFIsResponseNumber(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int)Convert.ToInt32(Database.ExecuteScalar("RFIsResponseNumberGet", parameters));
        }

        public int? AddRFIsResponse(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("RFIsResponseAdd", parameters));
        }

        public IDataReader GetRFISResponse(int? RFIResponseId)
        {
            return (IDataReader)Database.ExecuteReader("RFIsResponseGet", CheckDBNull(RFIResponseId));
        }


        public IDataReader GetRFISWithResponse(int? RFIId)
        {              
            return(IDataReader)Database.ExecuteReader("RFIsWithResponseGet", CheckDBNull(RFIId));
           
        }


        public int? AddRFIsResponseAttachments(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("RFIsResponseAttachmentsAdd", parameters));
        }

        public IDataReader GetRFISResponseAttachments(int? RFIResponseId)
        {
            return (IDataReader)Database.ExecuteReader("RFIsResponseAttachmentsGet", CheckDBNull(RFIResponseId));
        }



        //#---RFI Response--


        #endregion

#region ProjectImage
        public int? AddProjectImage(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("ProjectImageAdd", parameters));
        }


        public IDataReader GetProjectImagesByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("ProjectImagesGet", CheckDBNull(ProjectId));
        }


        public IDataReader GetProjectImageById(int? Id)
        {
            return (IDataReader)Database.ExecuteReader("ProjectImagesGetById", CheckDBNull(Id));
        }



        #endregion


 //#---
#region MeetingMinutes Method
        public IDataReader GetMeetingsByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("MeetingsGetByProject", CheckDBNull(ProjectId));
        }


        public IDataReader GetMeeting(int? MeetingId)
        {
            return (IDataReader)Database.ExecuteReader("MeetingGet", CheckDBNull(MeetingId));
        }

        public void UpdateMeeting(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("MeetingUpdate", parameters);
        }

       

        public int? AddMeeting(Object[] parameters)
        {
            CheckDBNull(parameters);
           return (int?)Convert.ToInt32(Database.ExecuteScalar("MeetingAdd", parameters));
        }

        public void DeleteMeeting(int? MeetingId)
        {
            Database.ExecuteNonQuery("MeetingDelete", CheckDBNull(MeetingId));
        }

        public void UpdateMeetingFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            //Database.ExecuteNonQuery("MeetingUpdateFiles", parameters);
        }




        #endregion     



#region EOT Methods
        public IDataReader GetEOTsByProject(int? ProjectId)
        {
            return (IDataReader)Database.ExecuteReader("EOTsGetByProject", CheckDBNull(ProjectId));
        }

        public IDataReader GetEOT(int? EOTId)
        {
            return (IDataReader)Database.ExecuteReader("EOTsGet", CheckDBNull(EOTId));
        }

        public IDataReader GetNODEOT(int? EOTId,int? NODId)
        {
            return (IDataReader)Database.ExecuteReader("NODEOTsGet", CheckDBNull(EOTId), CheckDBNull(NODId));
        }

        public void UpdateEOT(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("EOTsUpdate", parameters);
        }

        public void UpdateEOTSendDate(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("EOTsUpdateSendDate", parameters);
        }

        public int? AddEOT(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("EOTsAdd", parameters));
        }

        //#--to add NODEOTs
        public int? AddNODEOT(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("NODEOTsAdd", parameters));
        }


        public void DeleteEOT(int? EOTId)
        {
            Database.ExecuteNonQuery("EOTsDelete", CheckDBNull(EOTId));
        }

        public void UpdateEOTFiles(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("EOTsUpdateFiles", parameters);
        }
#endregion

#region Addendum Methods
        public IDataReader GetAddendumsByTrade(int? TradeId)
        {
            return (IDataReader)Database.ExecuteReader("AddendumsGetByTrade", CheckDBNull(TradeId));
        }

        public IDataReader GetAddendum(int? AddendumId)
        {
            return (IDataReader)Database.ExecuteReader("AddendumsGet", CheckDBNull(AddendumId));
        }

        public void UpdateAddendum(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("AddendumsUpdate", parameters);
        }

        public int? AddAddendum(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("AddendumsAdd", parameters));
        }

        public void DeleteAddendum(int? AddendumId)
        {
            Database.ExecuteNonQuery("AddendumsDelete", CheckDBNull(AddendumId));
        }
#endregion

#region Attachment Methods
        public IDataReader GetAttachmentsByGroup(int? AttachmentGroupId)
        {
            return (IDataReader)Database.ExecuteReader("AttachmentsGetByAttachmentsGroup", CheckDBNull(AttachmentGroupId));
        }

        public IDataReader GetAttachment(int? AttachmentId)
        {
            return (IDataReader)Database.ExecuteReader("AttachmentsGet", CheckDBNull(AttachmentId));
        }

        public void UpdateAttachment(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("AttachmentsUpdate", parameters);
        }

        public int? AddAttachment(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("AttachmentsAdd", parameters));
        }

        public void DeleteAttachment(int? AttachmentId)
        {
            Database.ExecuteNonQuery("AttachmentsDelete", CheckDBNull(AttachmentId));
        }
        #endregion

        #endregion

        //* IMX Additions
        #region OrderHeader Methods
        public IDataReader GetOrderHeaderGetActive()
        {
            return (IDataReader)Database.ExecuteReader("OrderHeaderGetActive");
        }

        public IDataReader GetOrderHeader(int? OrderHeaderId)
        {
            return (IDataReader)Database.ExecuteReader("OrderHeaderGet", CheckDBNull(OrderHeaderId));
        }

        public void UpdateOrderHeader(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("OrderHeaderUpdate", parameters);
        }

        public int? AddOrderHeader(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("OrderHeaderAdd", parameters));
        }

        public void DeleteOrderHeader(int? OrderHeaderId)
        {
            Database.ExecuteNonQuery("OrderHeaderDelete", CheckDBNull(OrderHeaderId));
        }
        #endregion

        #region SubContractor Methods



        #endregion
        #region OrderDetail Methods


        public void UpdateSiteOrderDetail(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("SiteOrderDetailUpdate", parameters);
        }

        public int? AddSiteOrderDetail(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("SiteOrderDetailAdd", parameters));
        }

        public int SearchSiteOrdersCountAll(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("SiteOrdersSearchCountAllGet", parameters));
        }

        public IDataReader ListSiteOrders(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrdersList", parameters);
        }
        public void SumSiteOrders(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("SiteOrderSum", parameters);
        }
        public int SearchSiteOrdersCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("SiteOrdersSearchCountGet", parameters));
        }
        public int SearchSiteOrderDocsCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("SiteOrderDocsCountGet", parameters));
        }
        public int SearchSiteOrdersCountUser(Object[] parameters)
        {
            CheckDBNull(parameters);
            return Convert.ToInt32(Database.ExecuteScalar("SiteOrdersSearchCountUserGet", parameters));
        }
        public IDataReader SearchSiteOrdersAll(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrdersSearchAll", parameters);
        }
        public IDataReader SearchSiteOrders(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrdersSearch", parameters);
        }
        public IDataReader SearchSiteOrdersUser(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrdersSearchUser", parameters);
        }
        public IDataReader GetSiteOrderDetails(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderDetails", parameters);
        }
        public IDataReader GetSiteOrderDetail(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderDetailGet", parameters);
        }

        public IDataReader GetSiteOrder(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderGet", parameters);
        }

        public void UpdateSiteOrderApprovalStatus(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("SiteOrderApprovalsUpdateStatus", parameters);
        }
        public void UpdateSiteOrder(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("SiteOrderUpdate", parameters);
        }
        
        public int? AddSiteOrder(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("SiteOrderAdd", parameters));
        }
        public int? AddSiteOrderApprovalsProcess(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("SiteOrderApprovalsAddProcess", parameters));
        }
        public int? AddSiteOrderApprovalsHireProcess(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("SiteOrderApprovalsAddHireProcess", parameters));
        }

        public void DeleteSiteOrder(int? subContractorId)
        {
            Database.ExecuteNonQuery("SiteOrdersDelete", CheckDBNull(subContractorId));
        }

        public IDataReader GetOrderDetailList(int? OrderId)
        {
            return (IDataReader)Database.ExecuteReader("OrderDetailList");
        }

        public IDataReader GetOrderDetail(int? OrderDetailId)
        {
            return (IDataReader)Database.ExecuteReader("OrderDetailGet", CheckDBNull(OrderDetailId));
        }

        public void UpdateOrderDetail(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("OrderDetailUpdate", parameters);
        }

        public int? AddOrderDetail(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("OrderDetailAdd", parameters));
        }

        public void DeleteOrderDetail(int? OrderDetailId)
        {
            Database.ExecuteNonQuery("OrderDetailDelete", CheckDBNull(OrderDetailId));
        }
        #endregion

        #region OrderApprovals Methods
        public IDataReader GetOrderApprovals()
        {
            return (IDataReader)Database.ExecuteReader("OrderApprovalsList");
        }

        public IDataReader GetOrderApproval(int? OrderApprovalId)
        {
            return (IDataReader)Database.ExecuteReader("OrderApprovalsGet", CheckDBNull(OrderApprovalId));
        }

        public void UpdateOrderApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("OrderApprovalsUpdate", parameters);
        }

        public int? AddOrderApproval(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (int?)Convert.ToInt32(Database.ExecuteScalar("OrderApprovalsAdd", parameters));
        }

        public void DeleteOrderApproval(int? OrderApprovalId)
        {
            Database.ExecuteNonQuery("OrderApprovalsDelete", CheckDBNull(OrderApprovalId));
        }
        #endregion

        #region OrderDoc Methods
        public IDataReader SearchSiteOrderApprovalsCount(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderApprovalsSearchCount", parameters);
        }
        public IDataReader SearchSiteOrderApprovalsAll(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderApprovalsSearchAll", parameters);
        }
        public IDataReader SiteOrderApprovalsGetCurrent(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderApprovalsGetCurrent", parameters);
        }

        public IDataReader SiteOrderApprovalsEmailList(Object[] parameters)  // DS20231010
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderApprovalsEmailList", parameters);
        }
        public IDataReader SiteOrderApprovalsUpdateReason(Object[] parameters)  // DS20231010
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderApprovalsUpdateReason", parameters);
        }
        public void MaintenanceDaily(Object[] parameters)  // DS20231025
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("MaintenanceDaily", parameters);
        }
        public IDataReader SearchSiteOrderApprovals(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderApprovalsSearch", parameters);
        }
        public IDataReader SiteOrderApprovalStatus(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderApprovalsStatus", parameters);
        }
        public IDataReader GetProjectTrades(Object[] parameters)  // DS20230821
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ProjectTradesGet", parameters);
        }
        public IDataReader GetProjectTradesAll(Object[] parameters)  // DS20230821
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("ProjectTradesGetAll", parameters);
        }
        public IDataReader GetSiteOrderDoc(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderDocGet", parameters);
        }

        public IDataReader SearchSiteOrderDocs(Object[] parameters)
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderDocsSearch", parameters);
        }
        public IDataReader SiteOrderMobileDocsSearch(Object[] parameters)   //DS202307
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderMobileDocsSearch", parameters);
        }
        public IDataReader SiteOrderDocsRename(Object[] parameters)   //DS202307
        {
            CheckDBNull(parameters);
            return (IDataReader)Database.ExecuteReader("SiteOrderDocsRename", parameters);
        }
        //
        public IDataReader GetOrderDocList(int OrderID)
        {
            return (IDataReader)Database.ExecuteReader("OrderDocList");
        }

        public IDataReader GetOrderDoc(Guid? OrderDocId)
        {
            return (IDataReader)Database.ExecuteReader("OrderDocGet", CheckDBNull(OrderDocId));
        }

        public void UpdateOrderDoc(Object[] parameters)
        {
            CheckDBNull(parameters);
            Database.ExecuteNonQuery("OrderDocUpdate", parameters);
        }

        public int? AddSiteOrderDoc(Object[] parameters)
        {
            CheckDBNull(parameters);
            //return (int)(Database.ExecuteScalar("SiteOrderDocAdd", parameters));
            return (int?)Convert.ToInt32(Database.ExecuteScalar("SiteOrderDocAdd", parameters));
        }

        public void DeleteSiteOrderDoc(Object[] parameters)
        {
            Database.ExecuteNonQuery("SiteOrderDocDelete", parameters);
        }
        public void DeleteSiteOrderDetail(Object[] parameters)
        {
            Database.ExecuteNonQuery("SiteOrderDetailDelete", parameters);
        }
        #endregion
    }
}
