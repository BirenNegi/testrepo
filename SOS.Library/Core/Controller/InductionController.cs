using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Transactions;

using SOS.Data;


namespace SOS.Core
{
 public sealed class InductionController :Controller
    {
        private static InductionController instance;


        #region Public Methods
        public static InductionController GetInstance()
        {
            if (instance == null)
                instance = new InductionController();
            
            return instance;
        }


        #region GI/SI Documents
        public InductionDocumentsInfo CreateInductionDocument(IDataReader dr)
        {
            InductionDocumentsInfo inductionDoc = new InductionDocumentsInfo(Data.Utils.GetDBInt32(dr["Id"]));
            inductionDoc.FileName = Data.Utils.GetDBString(dr["FileName"]);
            inductionDoc.FilePath = Data.Utils.GetDBString(dr["FilePath"]);
            inductionDoc.State = Data.Utils.GetDBString(dr["State"]);
            inductionDoc.Version = Data.Utils.GetDBString(dr["Version"]);
            inductionDoc.IsActive = Data.Utils.GetDBBoolean(dr["Active"]);
            inductionDoc.Type= Data.Utils.GetDBString(dr["Type"]);
            inductionDoc.projectId = Data.Utils.GetDBInt32(dr["ProjectId"]);
            return inductionDoc;
        }

        public List<InductionDocumentsInfo> GetInductionDocuments(string Type, int? ProjectId=null  )
        {
            IDataReader dr = null;
            List<InductionDocumentsInfo> DocumentInfoList = new List<InductionDocumentsInfo>();

            try
            {
                List<Object> parameters = new List<Object>();

                parameters.Add(Type);
                parameters.Add(ProjectId);

                dr = Data.DataProvider.GetInstance().GetInductionDocuments(parameters.ToArray());
                while (dr.Read())
                    DocumentInfoList.Add(CreateInductionDocument(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting  Iduction Documents from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return DocumentInfoList;
        }
        
        public int? AddInductionDocuments(InductionDocumentsInfo documentsInfo)
        {
            int? documentsId;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(documentsInfo);
            parameters.Add(documentsInfo.State);
            parameters.Add(documentsInfo.Version);
            parameters.Add(documentsInfo.FileName);
            parameters.Add(documentsInfo.FilePath);
            parameters.Add(documentsInfo.Type);
            parameters.Add(documentsInfo.projectId);
            
            parameters.Add(documentsInfo.CreatedBy);
            parameters.Add(documentsInfo.CreatedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    documentsId = Data.DataProvider.GetInstance().AddInductionDocuments(parameters.ToArray());
                    documentsInfo.Id = documentsId;


                    scope.Complete();
                }
                
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Adding  Induction documents to database");
            }

            return documentsId;
        }
      
        public void DeleteInductionDocuments(InductionDocumentsInfo documentsInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(documentsInfo);

            parameters.Add(documentsInfo.Id);
            parameters.Add(documentsInfo.ModifiedBy);
            parameters.Add(documentsInfo.ModifiedDate);


            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().DeleteInductionDocuments(parameters.ToArray());
                    


                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Deleting  Induction documents to database");
            }
                        
        }

        #endregion  GI/SI Documents


        #region GI/SI OptinalQA
        public OptionalQAInfo CreateInductionOptinalQA(IDataReader dr)
        {
            OptionalQAInfo optonalQA = new OptionalQAInfo(Data.Utils.GetDBInt32(dr["Id"]));
            optonalQA.Question = Data.Utils.GetDBString(dr["Questions"]);
            optonalQA.Opt1 = Data.Utils.GetDBString(dr["Opt1"]);
            optonalQA.Opt2 = Data.Utils.GetDBString(dr["Opt2"]);
            optonalQA.Opt3 = Data.Utils.GetDBString(dr["Opt3"]);
            optonalQA.Opt4 = Data.Utils.GetDBString(dr["Opt4"]);
            optonalQA.RightAnswer = Data.Utils.GetDBString(dr["RightAnswer"]);
            optonalQA.IsActive = Data.Utils.GetDBBoolean(dr["Active"]);
            optonalQA.Type = Data.Utils.GetDBString(dr["Type"]);
            optonalQA.projectId = Data.Utils.GetDBInt32(dr["ProjectId"]);

            return optonalQA;
        }

        public List<OptionalQAInfo> GetInductionOptinalQAs(string Type, int? ProjectId = null)
        {
            IDataReader dr = null;
            List<OptionalQAInfo> optionalQAInfoList = new List<OptionalQAInfo>();

            try
            {
                List<Object> parameters = new List<Object>();

                parameters.Add(Type);
                parameters.Add(ProjectId);
               
                dr = Data.DataProvider.GetInstance().GetInductionOptinalQAs(parameters.ToArray());
                while (dr.Read())
                    optionalQAInfoList.Add(CreateInductionOptinalQA(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting  Induction OptinalQA from database");
            }
            finally
            {


                if (dr != null)
                    dr.Close();
            }

            return optionalQAInfoList;
        } 
        
        public OptionalQAInfo GetInductionOptinalQA(OptionalQAInfo optQAInfo)
        {
            IDataReader dr = null;
            OptionalQAInfo optionalQAInfo=null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetInductionOptinalQA(optQAInfo.Id);
                while (dr.Read())
                    optionalQAInfo=CreateInductionOptinalQA(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting  Induction OptinalQA from database");
            }
            finally
            {
               
                if (dr != null)
                    dr.Close();
            }

            return optionalQAInfo;
        }

        public int? AddInductionOptinalQA(OptionalQAInfo optionalQAInfo)
        {
            int? optionalQAId;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(optionalQAInfo);
            parameters.Add(optionalQAInfo.Question);
            parameters.Add(optionalQAInfo.Opt1);
            parameters.Add(optionalQAInfo.Opt2);
            parameters.Add(optionalQAInfo.Opt3);
            parameters.Add(optionalQAInfo.Opt4);
            parameters.Add(optionalQAInfo.RightAnswer);
            parameters.Add(optionalQAInfo.Type);
            parameters.Add(optionalQAInfo.projectId);
            parameters.Add(optionalQAInfo.CreatedBy);
            parameters.Add(optionalQAInfo.CreatedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    optionalQAId = Data.DataProvider.GetInstance().AddOptionalQA(parameters.ToArray());
                    optionalQAInfo.Id = optionalQAId;


                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Adding  Induction optional QA to database");
            }

            return optionalQAId;
        }
        
        public void UpdateInductionOptinalQA(OptionalQAInfo optionalQAInfo)
        {
             
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(optionalQAInfo);
            parameters.Add(optionalQAInfo.Id);
            parameters.Add(optionalQAInfo.Question);
            parameters.Add(optionalQAInfo.Opt1);
            parameters.Add(optionalQAInfo.Opt2);
            parameters.Add(optionalQAInfo.Opt3);
            parameters.Add(optionalQAInfo.Opt4);
            parameters.Add(optionalQAInfo.RightAnswer);
            parameters.Add(optionalQAInfo.IsActive);
            parameters.Add(optionalQAInfo.CreatedBy);
            parameters.Add(optionalQAInfo.CreatedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateOptionalQA(parameters.ToArray());
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Updating  Induction optional QA to database");
            }

            
        }
        
        public void DeleteInductionOptionalQA(OptionalQAInfo optionalQAInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(optionalQAInfo);
            parameters.Add(optionalQAInfo.Id);
            parameters.Add(optionalQAInfo.ModifiedBy);
            parameters.Add(optionalQAInfo.ModifiedDate);

            try
            { 
                     Data.DataProvider.GetInstance().DeleteOptionalQA(parameters.ToArray());
             
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Deleting  Induction Optional QA to database");
            }
           
        }

        #endregion  GI OptinalQA


        #region GI/SI YesNoQA
        public YesNoQAInfo CreateInductionYesNoQA(IDataReader dr)
        {
            YesNoQAInfo yesNoQA = new YesNoQAInfo(Data.Utils.GetDBInt32(dr["Id"]));
            yesNoQA.Question = Data.Utils.GetDBString(dr["Questions"]);
            yesNoQA.Comments = Data.Utils.GetDBString(dr["Comments"]);
            yesNoQA.IsActive = Data.Utils.GetDBBoolean(dr["Active"]);
            yesNoQA.Type = Data.Utils.GetDBString(dr["Type"]);
            yesNoQA.projectId =Data.Utils.GetDBInt32(dr["ProjectId"]);

            return yesNoQA;
        }

        public List<YesNoQAInfo> GetInductionYesNoQAs(string Type, int? ProjectId = null)
        {
            IDataReader dr = null;
            List<YesNoQAInfo> yesNoQAInfoList = new List<YesNoQAInfo>();

            try
            {
                List<Object> parameters = new List<Object>();

                parameters.Add(Type);
                parameters.Add(ProjectId);


                dr = Data.DataProvider.GetInstance().GetInductionYesNoQAs(parameters.ToArray());
                while (dr.Read())
                    yesNoQAInfoList.Add(CreateInductionYesNoQA(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting  Induction YesNoQAs from database");
            }
            finally
            {


                if (dr != null)
                    dr.Close();
            }

            return yesNoQAInfoList;
        }


        public YesNoQAInfo GetInductionYesNoQA(int? Id)
        {
            IDataReader dr = null;
            YesNoQAInfo yesNoQAInfo = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetInductionOptinalQA(Id);
                while (dr.Read())
                    yesNoQAInfo = CreateInductionYesNoQA(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting  Induction YesNoQA from database");
            }
            finally
            {

                if (dr != null)
                    dr.Close();
            }

            return yesNoQAInfo;
        }

        public int? AddInductionYesNoQA(YesNoQAInfo yesNoQAInfo)
        {
            int? yesNoQAId;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(yesNoQAInfo);
            parameters.Add(yesNoQAInfo.Question);
            parameters.Add(yesNoQAInfo.Comments);
            parameters.Add(yesNoQAInfo.Type);
            parameters.Add(yesNoQAInfo.projectId);
            parameters.Add(yesNoQAInfo.CreatedBy);
            parameters.Add(yesNoQAInfo.CreatedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    yesNoQAId = Data.DataProvider.GetInstance().AddYesNoQA(parameters.ToArray());
                    yesNoQAInfo.Id = yesNoQAId;


                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Adding Induction YesNoQA to database");
            }

            return yesNoQAId;
        }



        public void  UpdateInductionYesNoQA(YesNoQAInfo yesNoQAInfo)
        {
            
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(yesNoQAInfo);
            parameters.Add(yesNoQAInfo.Id);
            parameters.Add(yesNoQAInfo.Question);
            parameters.Add(yesNoQAInfo.Comments);
            parameters.Add(yesNoQAInfo.IsActive);
            parameters.Add(yesNoQAInfo.ModifiedBy);
            parameters.Add(yesNoQAInfo.ModifiedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                     Data.DataProvider.GetInstance().UpdateYesNoQA(parameters.ToArray());
                   


                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Updating Induction YesNo  QA to database");
            }

            
        }






        public void DeleteInductionYesNoQA(YesNoQAInfo yseNoQAInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(yseNoQAInfo);
            parameters.Add(yseNoQAInfo.Id);
            parameters.Add(yseNoQAInfo.ModifiedBy);
            parameters.Add(yseNoQAInfo.ModifiedDate);

            try
            {
                Data.DataProvider.GetInstance().DeleteYesNoQA(parameters.ToArray());

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Deleting Induction YesNoQA from database");
            }

        }

        #endregion  GI YesNoQA



        #region GI/SI Note
        public InductionNoteInfo CreateInductionNote(IDataReader dr)
        {
            InductionNoteInfo indNoteInfo = new InductionNoteInfo(Data.Utils.GetDBInt32(dr["Id"]));
            indNoteInfo.Note = Data.Utils.GetDBString(dr["Note"]);
            indNoteInfo.IsActive = Data.Utils.GetDBBoolean(dr["Active"]);
            indNoteInfo.Type = Data.Utils.GetDBString(dr["Type"]);
            indNoteInfo.projectId = Data.Utils.GetDBInt32(dr["ProjectId"]);

            return indNoteInfo;
        }

        public InductionNoteInfo GetInductionNote(string Type, int? ProjectId = null)
        {
            IDataReader dr = null;
            InductionNoteInfo indNoteInfoList = new InductionNoteInfo();

            try
            {
                List<Object> parameters = new List<Object>();

                parameters.Add(Type);
                parameters.Add(ProjectId);


                dr = Data.DataProvider.GetInstance().GetInductionNote(parameters.ToArray());
                while (dr.Read())
                    indNoteInfoList=(CreateInductionNote(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting  Induction Note from database");
            }
            finally
            {


                if (dr != null)
                    dr.Close();
            }

            return indNoteInfoList;
        }


      

        public int? AddInductionNote(InductionNoteInfo indNoteInfo)
        {
            int? indNoteId;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(indNoteInfo);
            parameters.Add(indNoteInfo.Note);
            parameters.Add(indNoteInfo.Type);
            parameters.Add(indNoteInfo.projectId);
            parameters.Add(indNoteInfo.CreatedBy);
            parameters.Add(indNoteInfo.CreatedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    indNoteId = Data.DataProvider.GetInstance().AddInductionNote(parameters.ToArray());
                    indNoteInfo.Id = indNoteId;


                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Adding Induction Note to database");
            }

            return indNoteId;
        }



        public void UpdateInductionNote(InductionNoteInfo indNoteInfo)
        {

            List<Object> parameters = new List<Object>();

            SetModifiedInfo(indNoteInfo);
            parameters.Add(indNoteInfo.Id);
            parameters.Add(indNoteInfo.Note);

            
            parameters.Add(indNoteInfo.ModifiedBy);
            parameters.Add(indNoteInfo.ModifiedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateInductionNote(parameters.ToArray());



                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Updating Induction Note to database");
            }


        }






        public void DeleteInductionNote(InductionNoteInfo indNoteInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(indNoteInfo);
            parameters.Add(indNoteInfo.Id);
            parameters.Add(indNoteInfo.ModifiedBy);
            parameters.Add(indNoteInfo.ModifiedDate);

            try
            {
                Data.DataProvider.GetInstance().DeleteInductionNote(parameters.ToArray());

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Deleting Induction Note from database");
            }

        }

        #endregion  GI YesNoQA




        #region Induction Result

        public InductionResultInfo AddInductionResult(InductionResultInfo iInfo)
        {
            int? Id;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(iInfo);

            parameters.Add(iInfo.PeopleId);
            parameters.Add(iInfo.SubcontractorId);
            parameters.Add(iInfo.Name);
            parameters.Add(iInfo.SubcontractorName);
            parameters.Add(iInfo.Mobile);
            parameters.Add(iInfo.Email);
            parameters.Add(iInfo.ResultDate);
            parameters.Add(iInfo.InductionType);
            parameters.Add(iInfo.CreatedBy);
            parameters.Add(iInfo.CreatedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Id = Data.DataProvider.GetInstance().AddInductionResult(parameters.ToArray());
                    iInfo.Id = Id;


                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Adding Induction Result to database");
            }

            return iInfo;
        }


        public InductionResultInfo CreateInductionResult(IDataReader dr)
        {
            InductionResultInfo iInfo = new InductionResultInfo(Data.Utils.GetDBInt32(dr["Id"]));
            iInfo.PeopleId = Data.Utils.GetDBInt32(dr["PeopleId"]);
            iInfo.SubcontractorId = Data.Utils.GetDBInt32(dr["SubcontratorId"]);
            iInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            iInfo.SubcontractorName = Data.Utils.GetDBString(dr["SubcontractorName"]);
            iInfo.Mobile = Data.Utils.GetDBString(dr["Mobile"]);
            iInfo.Email = Data.Utils.GetDBString(dr["Email"]);
            iInfo.ResultDate = Data.Utils.GetDBDateTime(dr["ResultDate"]).Value;  
            iInfo.IsActive = Data.Utils.GetDBBoolean(dr["Active"]);
            iInfo.InductionType = Data.Utils.GetDBString(dr["InductionType"]);
            return iInfo;
        }

        public InductionResultInfo GetInductionResultByPeopleId(int? Id)
        {
            IDataReader dr = null;
            InductionResultInfo iInfo = new InductionResultInfo();
            if (Id != null)
                dr = DataProvider.GetInstance().GetInductionResultByPeopleId(Id);

            while (dr.Read())
                iInfo = CreateInductionResult(dr);

            return iInfo;

        }

        #endregion









        #endregion

    }
}
