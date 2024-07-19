using System;
using System.Data;
using System.Collections.Generic;



namespace SOS.Core
{
    public sealed class SubContractorsController : Controller
    {

#region Private Members
        private static SubContractorsController instance;
#endregion

#region Private Methods
        private SubContractorsController()
        {
        }
#endregion

#region Public Methods
        public static SubContractorsController GetInstance()
        {
            if (instance == null)
                instance = new SubContractorsController();

            return instance;
        }

        public int SearchSubContractorsCount(String strSearch, String strBusinessUnit)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));
            try
            {
                return Data.DataProvider.GetInstance().SearchSubContractorsCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Subcontractors Count from database.");
            }
        }

        /// <summary>
        /// Search for SubContractors in the database 
        /// </summary>
        public List<SubContractorInfo> SearchSubContractors(int startRowIndex, int maximumRows, String orderBy, String strSearch, String strBusinessUnit)
        {
            IDataReader dr = null;
            List<SubContractorInfo> subContractorInfoList = new List<SubContractorInfo>();
            List<Object> parameters = new List<Object>();

            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            parameters.Add(UI.Utils.GetFormString(orderBy));
            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSubContractors(parameters.ToArray());
                while (dr.Read())
                    subContractorInfoList.Add(this.GetSubContractor(Data.Utils.GetDBInt32(dr["SubContractorId"])));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Subcontractors from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return subContractorInfoList;
        }
        public int SearchSubContractorsProjCount(String strSearch, String strBusinessUnit, String strProjectId) // DS20230216
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));
            parameters.Add(UI.Utils.GetFormInteger(strProjectId));
            try
            {
                return Data.DataProvider.GetInstance().SearchSubContractorsCountProj(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Subcontractors Count from database.");
            }
        }

        /// <summary>
        /// Search for SubContractors in the database 
        /// </summary>
        public List<SubContractorInfo> SearchSubContractorsProj(int startRowIndex, int maximumRows, String orderBy, String strSearch, String strBusinessUnit, String strProjectId) // DS20230216
        {
            IDataReader dr = null;
            List<SubContractorInfo> subContractorInfoList = new List<SubContractorInfo>();
            List<Object> parameters = new List<Object>();

            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            parameters.Add(UI.Utils.GetFormString(orderBy));
            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));
            parameters.Add(UI.Utils.GetFormInteger(strProjectId));

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSubContractorsProj(parameters.ToArray());
                while (dr.Read())
                {
                    SubContractorInfo SubContractorInfo = this.GetSubContractor(Data.Utils.GetDBInt32(dr["SubContractorId"]));  // ds20230402
                    SubContractorInfo.Name = SubContractorInfo.ShortName;  // ds20230402
                    subContractorInfoList.Add(SubContractorInfo);  // ds20230402
                }
            }

            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Subcontractors from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return subContractorInfoList;
        }
        /// <summary>
        /// Get all SubContractors names for the specified business unit form the database
        /// </summary>
        public List<SubContractorInfo> ListSubContractors(BusinessUnitInfo businessUnitInfo)
        {
            IDataReader dr = null;
            SubContractorInfo subContractorInfo = null;
            List<SubContractorInfo> subContractorInfoList = new List<SubContractorInfo>();
            List<Object> parameters = new List<Object>();

            parameters.Add(GetId(businessUnitInfo));

            try
            {
                dr = Data.DataProvider.GetInstance().ListSubContractors(parameters.ToArray());
                while (dr.Read())
                {
                    subContractorInfo = new SubContractorInfo(Data.Utils.GetDBInt32(dr["SubContractorId"]));
                    subContractorInfo.ShortName = Data.Utils.GetDBString(dr["ShortName"]);

                    subContractorInfoList.Add(subContractorInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Subcontractors from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return subContractorInfoList;
        }

        /// <summary>
        /// Get all SubContractors names form the database
        /// </summary>
        public List<SubContractorInfo> ListSubContractors()
        {
            return ListSubContractors(null);
        }

        /// <summary>
        /// Creates a Subcontractor from a dr
        /// </summary>
        public SubContractorInfo CreateSubContractor(IDataReader dr, Dictionary<int, BusinessUnitInfo> businessUnitDictionary)
        {
            SubContractorInfo subContractorInfo = new SubContractorInfo(Data.Utils.GetDBInt32(dr["SubContractorId"])); ;

            subContractorInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            subContractorInfo.ShortName = Data.Utils.GetDBString(dr["ShortName"]);
            subContractorInfo.Street = Data.Utils.GetDBString(dr["Street"]);
            subContractorInfo.Locality = Data.Utils.GetDBString(dr["Locality"]);
            subContractorInfo.State = Data.Utils.GetDBString(dr["State"]);
            subContractorInfo.PostalCode = Data.Utils.GetDBString(dr["PostalCode"]);
            subContractorInfo.Phone = Data.Utils.GetDBString(dr["Phone"]);
            subContractorInfo.Fax = Data.Utils.GetDBString(dr["Fax"]);
            subContractorInfo.Abn = Data.Utils.GetDBString(dr["ABN"]);
            subContractorInfo.Account = Data.Utils.GetDBString(dr["Account"]);
            subContractorInfo.Website = Data.Utils.GetDBString(dr["Website"]);
            //#---
            subContractorInfo.ACN = Data.Utils.GetDBString(dr["ACN"]);
            subContractorInfo.LicenceNumber = Data.Utils.GetDBString(dr["LicenceNumber"]);
            subContractorInfo.PrequalifiedForm = Data.Utils.GetDBString(dr["PrequalifiedForm"]);
            subContractorInfo.PublicLiabilityInsurance = Data.Utils.GetDBString(dr["PublicLiabilityInsurance"]);
            subContractorInfo.WorkCoverInsurance = Data.Utils.GetDBString(dr["WorkCoverInsurance"]);
            subContractorInfo.DCContractor = Data.Utils.GetDBBoolean(dr["DCContractor"]);
            subContractorInfo.ProfessionalIndemnityInsurance = Data.Utils.GetDBString(dr["ProfessionalIndemnityInsurance"]);

            
           subContractorInfo.BusinessUnitslist = GetSubcontractorBusinessUnitList(subContractorInfo);   //#-----------

            //#--
            subContractorInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);

            AssignAuditInfo(subContractorInfo, dr);

            if (dr["BusinessUnitId"] != DBNull.Value) 
                subContractorInfo.BusinessUnit = ContractsController.GetInstance().CreateBusinessUnit(Data.Utils.GetDBInt32(dr["BusinessUnitId"]), businessUnitDictionary);

            return subContractorInfo;
        }

        /// <summary>
        /// Creates a Subcontractor from a dr
        /// </summary>
        public SubContractorInfo CreateSubContractor(IDataReader dr)
        {
            return CreateSubContractor(dr, null);
        }

        /// <summary>
        /// Creates a SubContractorInfo object from a dr or retrieve it from the dictionary
        /// </summary>
        public SubContractorInfo CreateSubContractor(Object dbId, Dictionary<int, SubContractorInfo> subContractorsDictionary)
        {
            int Id = Data.Utils.GetDBInt32(dbId).Value;

            if (subContractorsDictionary == null)
                return GetInstance().GetSubContractor(Id);
            else if (subContractorsDictionary.ContainsKey(Id))
                return subContractorsDictionary[Id];
            else
                return null;
        }

        /// <summary>
        /// Get a SubContractor from persistent storage
        /// </summary>
        public SubContractorInfo GetSubContractor(int? subContractorId)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();

            parameters.Add(subContractorId);
            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContractor(parameters.ToArray());
                if (dr.Read())
                    return CreateSubContractor(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Subcontractor from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }




        /// <summary>
        /// Gets a Subcontractor with its contacts
        /// </summary>
        public SubContractorInfo GetSubContractorDeep(int? subContractorId)
        {
            SubContractorInfo subContractorInfo = GetSubContractor(subContractorId);
            if (subContractorInfo != null) { 
                subContractorInfo.Contacts = GetContacts(subContractorInfo);
                //#-----
                 subContractorInfo.BusinessUnitslist= GetSubcontractorBusinessUnitList(subContractorInfo);   //#-----------
             }
            return subContractorInfo;
        }
        
        /// <summary>
        /// Updates a SubContractor in the database
        /// </summary>
        /// <param name="subContractorInfo">The SubContractor to update</param>
        public void UpdateSubContractor(SubContractorInfo subContractorInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(subContractorInfo);

            parameters.Add(subContractorInfo.Id);
            parameters.Add(GetId(subContractorInfo.BusinessUnit));
            parameters.Add(subContractorInfo.Name);
            parameters.Add(subContractorInfo.ShortName);
            parameters.Add(subContractorInfo.Street);
            parameters.Add(subContractorInfo.Locality);
            parameters.Add(subContractorInfo.State);
            parameters.Add(subContractorInfo.PostalCode);
            parameters.Add(subContractorInfo.Phone);
            parameters.Add(subContractorInfo.Fax);
            parameters.Add(subContractorInfo.Abn);
            parameters.Add(subContractorInfo.Account);
            parameters.Add(subContractorInfo.Website);
            //#---
            parameters.Add(subContractorInfo.ACN);
            parameters.Add(subContractorInfo.LicenceNumber);
            parameters.Add(subContractorInfo.PrequalifiedForm);
            parameters.Add(subContractorInfo.PublicLiabilityInsurance);
            parameters.Add(subContractorInfo.WorkCoverInsurance);
            parameters.Add(subContractorInfo.ProfessionalIndemnityInsurance);
            parameters.Add(subContractorInfo.DCContractor);
            //#---

            parameters.Add(subContractorInfo.Comments);
           
            parameters.Add(subContractorInfo.ModifiedDate);
            parameters.Add(subContractorInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateSubContractor(parameters.ToArray());


                //#--
                if (subContractorInfo != null)
                {

                    Data.DataProvider.GetInstance().DeleteSubContractorBusinessUnitList(subContractorInfo.Id);


                    foreach (BusinessUnitInfo bUnit in subContractorInfo.BusinessUnitslist)
                    {
                        List<Object> parameters1 = new List<Object>();
                        parameters1.Add(subContractorInfo.Id);
                        parameters1.Add(bUnit.Id);
                        Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                    }
                } //#---

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updation Subcontractor in database");
            }
        }

        //#-------

        public SubContractorInfo GetSubContractorByABN(string subContractorABN)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();

            parameters.Add(subContractorABN);
            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContractorByABN(subContractorABN);//(parameters.ToArray());
                if (dr.Read())
                    return CreateSubContractor(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Subcontractor from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        //#------------







        /// <summary>
        /// Adds a SubContractor to the database
        /// </summary>
        /// <param name="subContractorInfo">The SubContractor to add</param>
        public int? AddSubContractor(SubContractorInfo subContractorInfo)
        {
            int? subContractorId = null;
            List<Object> parameters = new List<Object>();


            //#---- Varify for Duplicate ABN

            SubContractorInfo currentSubcontractor;
            if (subContractorInfo.Abn != null)
            {
                currentSubcontractor = GetSubContractorByABN(subContractorInfo.Abn);
                if (currentSubcontractor != null)
                {
                    throw new Exception("The ABN: " + subContractorInfo.Abn + " is already registered for Subcontractor: " + subContractorInfo.Name);
                }
            }
            //#-------


            SetCreateInfo(subContractorInfo);

            parameters.Add(GetId(subContractorInfo.BusinessUnit));
            parameters.Add(subContractorInfo.Name);
            parameters.Add(subContractorInfo.ShortName);
            parameters.Add(subContractorInfo.Street);
            parameters.Add(subContractorInfo.Locality);
            parameters.Add(subContractorInfo.State);
            parameters.Add(subContractorInfo.PostalCode);
            parameters.Add(subContractorInfo.Phone);
            parameters.Add(subContractorInfo.Fax);
            parameters.Add(subContractorInfo.Abn);
            parameters.Add(subContractorInfo.Account);
            parameters.Add(subContractorInfo.Website);
            //#---
            parameters.Add(subContractorInfo.ACN);
            parameters.Add(subContractorInfo.LicenceNumber);
            parameters.Add(subContractorInfo.PrequalifiedForm);
            parameters.Add(subContractorInfo.PublicLiabilityInsurance);
            parameters.Add(subContractorInfo.WorkCoverInsurance);
            parameters.Add(subContractorInfo.ProfessionalIndemnityInsurance);
            parameters.Add(subContractorInfo.DCContractor);
            //#---
            parameters.Add(subContractorInfo.Comments);

            parameters.Add(subContractorInfo.CreatedDate);
            parameters.Add(subContractorInfo.CreatedBy);

          
            try
            {
                subContractorId = Data.DataProvider.GetInstance().AddSubContractor(parameters.ToArray());

                //#--
                if (subContractorId != null && subContractorInfo.BusinessUnitslist.Count>0)
                {  
                    foreach (BusinessUnitInfo bUnit in subContractorInfo.BusinessUnitslist) {
                     List<Object> parameters1 = new List<Object>();
                        parameters1.Add(subContractorId);
                        parameters1.Add(bUnit.Id);
                    Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                }
               } //#---
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Subcontractor in database");
            }

            return subContractorId;
        }

        /// <summary>
        /// Adds or updates a Subcontractor
        /// </summary>
        public int? AddUpdateSubContractor(SubContractorInfo subContractorInfo)
        {
            if (subContractorInfo != null)
            {
                if (subContractorInfo.Id != null)
                {
                    UpdateSubContractor(subContractorInfo);
                    return subContractorInfo.Id;
                }
                else
                {
                    return AddSubContractor(subContractorInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Subcontractor from persistent storage
        /// </summary>
        public void DeleteSubContractor(SubContractorInfo subContractorInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteSubContractor(subContractorInfo.Id);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Removing Subcontractor from database");
            }
        }

        /// <summary>
        /// Get all the contacts for a SubContractor
        /// </summary>
        public List<ContactInfo> GetContacts(SubContractorInfo subContractorInfo)
        {
            IDataReader dr = null;
            ContactInfo contactInfo;
            List<ContactInfo> contactInfoList = new List<ContactInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContractorContacts(subContractorInfo.Id);
                while (dr.Read())
                {
                    contactInfo = new ContactInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));

                    contactInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    contactInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    contactInfo.Street = Data.Utils.GetDBString(dr["Street"]);
                    contactInfo.Locality = Data.Utils.GetDBString(dr["Locality"]);
                    contactInfo.Phone = Data.Utils.GetDBString(dr["Phone"]);
                    contactInfo.Mobile = Data.Utils.GetDBString(dr["Mobile"]);
                    contactInfo.Email = Data.Utils.GetDBString(dr["Email"]);
                    //#---
                        contactInfo.Position = Data.Utils.GetDBString(dr["EmployeePosition"]);
                        contactInfo.Inactive = Data.Utils.GetDBBoolean(dr["Inactive"]);
                        contactInfo.UserType = Data.Utils.GetDBString(dr["UserType"]);
                    //#---
                    contactInfo.Login = Data.Utils.GetDBString(dr["UserLogin"]);
                    contactInfo.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);

                    contactInfoList.Add(contactInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Getting Contacts for Subcontractor");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return contactInfoList;
        }

        //#---  GetSubcontractorBusinessUnitList
        public List<BusinessUnitInfo> GetSubcontractorBusinessUnitList(SubContractorInfo subContractorInfo)
        {
            IDataReader dr = null;
            BusinessUnitInfo bUnitInfo;
            List<BusinessUnitInfo> bUnitInfoList = new List<BusinessUnitInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContractorBusinessUnitList(subContractorInfo.Id);
                while (dr.Read())
                {
                    bUnitInfo = new BusinessUnitInfo(Data.Utils.GetDBInt32(dr["BusinessUnitId"]));
                    bUnitInfoList.Add(ContractsController.GetInstance().GetDeepBusinessUnit(bUnitInfo.Id));
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Getting businessUnitList for Subcontractor");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return bUnitInfoList;

        }

        //#---  GetSubcontractorBusinessUnitList



        //#----Subcontractor Variations Report
        public DataTable GetSubContractorsVariations(String ProjectName,String BusinessUnit,String Subcontractor)
        {

            List<string> parameters = new List<string>();
            parameters.Add(ProjectName);
            parameters.Add(BusinessUnit);
            parameters.Add(Subcontractor);


            DataTable dt = new DataTable();
           
            IDataReader dr;
            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContractorsVariations(parameters.ToArray());
                 dt.Load(dr);
                
                //if (dr.Read())
                //{ dt.Load(dr); }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting  subcontractors variations from database");
            }


            return dt;
        }

        //#----Subcontractor Variations Report




        //#----Subcontractor Variations By Type Report
        public DataTable GetSubContractorsVariationsByType(String ProjectName)
        {

            List<string> parameters = new List<string>();
            parameters.Add(ProjectName);
            //parameters.Add(BusinessUnit);
            //parameters.Add(Subcontractor);


            DataTable dt = new DataTable();

            IDataReader dr;
            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContractorsVariationsByType(parameters.ToArray());
                dt.Load(dr);

                //if (dr.Read())
                //{ dt.Load(dr); }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting  subcontractors variations by Type from database");
            }


            return dt;
        }

        //#----Subcontractor Variations Report






        //#----Subcontractor insurance link missing
        public DataTable GetSubContractorsMissingInsuranceLink(String BusinessUnit, String Subcontractor)
        {

            List<string> parameters = new List<string>();
            
            parameters.Add(BusinessUnit);
            parameters.Add(Subcontractor);


            DataTable dt = new DataTable();

            IDataReader dr;
            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContractorsMissingInsuranceLink(parameters.ToArray());
                dt.Load(dr);

                //if (dr.Read())
                //{ dt.Load(dr); }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting  subcontractors missing Insurance from database");
            }


            return dt;
        }

        //#----Subcontractor insurance link missing



        #endregion

    }
}
