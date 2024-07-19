using System;
using System.Data;
using System.Collections.Generic;

using SOS.UI;
using SOS.Data;

using System.Threading.Tasks;

namespace SOS.Core
{
    public sealed class PeopleController : Controller
    {

#region Private Members
        private static PeopleController instance;
#endregion

#region Private Methods
        private PeopleController()
        {
        }
#endregion

#region Public Methods
        public static PeopleController GetInstance()
        {
            if (instance == null)
                instance = new PeopleController();

            return instance;
        }

        public PeopleInfo GetPeopleObject(String peopleType, Object peopleId)
        {
            switch (peopleType)
            {
                case PeopleInfo.PeopleTypeEmployee:
                    return new EmployeeInfo(Data.Utils.GetDBInt32(peopleId));
                case PeopleInfo.PeopleTypeContact:
                    return new ContactInfo(Data.Utils.GetDBInt32(peopleId));
                case PeopleInfo.PeopleTypeClientContact:
                    return new ClientContactInfo(Data.Utils.GetDBInt32(peopleId));
            }

            throw new Exception("Invalid people type");
        }

        public int SearchPeopleCount(String peopleType, String strSearch, String strBusinessUnit, Boolean inactive)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));
            parameters.Add(inactive);

            try
            {
                if (peopleType == PeopleInfo.PeopleTypeEmployee)
                    return Data.DataProvider.GetInstance().SearchEmployeeCount(parameters.ToArray());
                //#--
                else if (peopleType == PeopleInfo.PeopleTypeClientContact)
                    return Data.DataProvider.GetInstance().SearchClientContactCount(parameters.ToArray());
                //#--
                else
                    return Data.DataProvider.GetInstance().SearchContactCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting People Count from database.");
            }
        }
        public int SearchPeopleCountSub(String peopleType, String strSearch, String strSubContractorId, Boolean inactive)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strSubContractorId));
            parameters.Add(inactive);

            try
            {
                //if (peopleType == PeopleInfo.PeopleTypeEmployee)
                //    return Data.DataProvider.GetInstance().SearchEmployeeCount(parameters.ToArray());
                ////#--
                //else if (peopleType == PeopleInfo.PeopleTypeClientContact)
                return Data.DataProvider.GetInstance().SearchClientContactCountSub(parameters.ToArray());
                //#--
                //else
                //    return Data.DataProvider.GetInstance().SearchContactCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting People Count from database.");
            }
        }
        /// <summary>
        /// Search for people in the database 
        /// </summary>
        public List<PeopleInfo> SearchPeople(int startRowIndex, int maximumRows, String orderBy, String peopleType, String strSearch, String strBusinessUnit, Boolean inactive)
        {
            IDataReader dr = null;
            List<PeopleInfo> peopleInfoList = new List<PeopleInfo>();
            List<Object> parameters = new List<Object>();

            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            parameters.Add(UI.Utils.GetFormString(orderBy));
            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));
            parameters.Add(inactive);

            try
            {
                if (peopleType == PeopleInfo.PeopleTypeEmployee)
                    dr = Data.DataProvider.GetInstance().SearchEmployee(parameters.ToArray());

                else if (peopleType == PeopleInfo.PeopleTypeClientContact)
                    dr = Data.DataProvider.GetInstance().SearchClientContact(parameters.ToArray());

                else
                    dr = Data.DataProvider.GetInstance().SearchContact(parameters.ToArray());


                if(dr!=null)
                while (dr.Read())
                    peopleInfoList.Add(GetPersonById(Data.Utils.GetDBInt32(dr["PeopleId"])));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting People from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return peopleInfoList;
        }

//        public List<PeopleInfo> SearchPeopleSub(int startRowIndex, int maximumRows, String orderBy, String peopleType, String strSearch, String strSubContractorId, Boolean inactive)
        public List<PeopleInfo> SearchPeopleSub(String peopleType, String strSearch, String strSubContractorId, Boolean inactive, String orderBy, int maximumRows, int startRowIndex)
        {
            //peopleType, strSearch, txtSubContractorId, inactive, orderBy, maximumRows, startRowIndex
            //peopleType, strSearch, txtSubContractorId, inactive, orderBy, maximumRows, startRowIndex
            //peopleType, strSearch, txtSubContractorId, inactive, orderBy, maximumRows, startRowIndex
            
            IDataReader dr = null;
            List<PeopleInfo> peopleInfoList = new List<PeopleInfo>();
            List<Object> parameters = new List<Object>();
            
            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            strSearch = "";
            parameters.Add(UI.Utils.GetFormString(orderBy));
            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strSubContractorId));
            parameters.Add(inactive);

            try
            {
           
                dr = Data.DataProvider.GetInstance().SearchClientContactSub(parameters.ToArray());
                if (dr != null)
                    while (dr.Read())
                      //  peopleInfoList.Add(GetPerson(Data.Utils.GetDBInt32(dr["PeopleId"]),"",""));
                      peopleInfoList.Add(GetPersonById(Data.Utils.GetDBInt32(dr["PeopleId"])));

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting People from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return peopleInfoList;
        }

        /// <summary>
        /// Gets all employees with apporval roles
        /// </summary>
        public List<EmployeeInfo> GetEmployeesWithApprovalRoles()
        {
            IDataReader dr = null;
            EmployeeInfo employeeInfo = null;
            List<EmployeeInfo> employeeInfoList = new List<EmployeeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().ListPeopleWithApprovalRoles();
                while (dr.Read())
                {
                    employeeInfo = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                    employeeInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    employeeInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    employeeInfo.UserType = Data.Utils.GetDBString(dr["UserType"]);

                    employeeInfoList.Add(employeeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting employees with approval roles from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return employeeInfoList;
        }

        /// <summary>
        /// Gets all employees with apporval roles related to projects
        /// </summary>
        public List<EmployeeInfo> ListEmployees(String selectedRoles, ProjectInfo projectInfo, ProjectInfo.combinedStatus combinedStatus, BusinessUnitInfo businessUnit)
        {
            IDataReader dr = null;
            EmployeeInfo employeeInfo = null;
            List<EmployeeInfo> employeeInfoList = new List<EmployeeInfo>();
            List<Object> parameters = new List<Object>();
            String strStatus;

            switch (combinedStatus)
            {
                case ProjectInfo.combinedStatus.Proposal: strStatus = ProjectInfo.StatusProposal; break;
                case ProjectInfo.combinedStatus.Active: strStatus = ProjectInfo.StatusActive; break;
                case ProjectInfo.combinedStatus.Complete: strStatus = ProjectInfo.StatusComplete; break;
                case ProjectInfo.combinedStatus.CompleteWithTasks: strStatus = ProjectInfo.StatusCompleteWithTasks; break;
                case ProjectInfo.combinedStatus.ActiveAndCompleteWithTasks: strStatus = ProjectInfo.StatusActive + "," + ProjectInfo.StatusCompleteWithTasks; break;
                default: strStatus = null; break;
            }

            parameters.Add(GetId(businessUnit));
            parameters.Add(GetId(projectInfo));
            parameters.Add(strStatus);
            parameters.Add(selectedRoles);

            try
            {
                dr = Data.DataProvider.GetInstance().ListPeople(parameters.ToArray());
                while (dr.Read())
                {
                    employeeInfo = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                    employeeInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    employeeInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
                   
                    employeeInfo.Inactive = Data.Utils.GetDBBoolean(dr["Inactive"]); //#----

                    employeeInfoList.Add(employeeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting employees list from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return employeeInfoList;
        }

        /// <summary>
        /// Creates a person from a dr and subcontractorsDictionary
        /// </summary>
        public PeopleInfo CreatePerson(IDataReader dr, Dictionary<int, SubContractorInfo> subcontractorsDictionary)
        {
            PeopleInfo peopleInfo = GetPeopleObject(Data.Utils.GetDBString(dr["Type"]), Data.Utils.GetDBInt32(dr["PeopleId"]));

            peopleInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
            peopleInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
            peopleInfo.Street = Data.Utils.GetDBString(dr["Street"]);
            peopleInfo.Locality = Data.Utils.GetDBString(dr["Locality"]);
            peopleInfo.State = Data.Utils.GetDBString(dr["State"]);
            peopleInfo.PostalCode = Data.Utils.GetDBString(dr["PostalCode"]);
            peopleInfo.Phone = Data.Utils.GetDBString(dr["Phone"]);
            peopleInfo.Fax = Data.Utils.GetDBString(dr["Fax"]);
            peopleInfo.Mobile = Data.Utils.GetDBString(dr["Mobile"]);
            peopleInfo.Email = Data.Utils.GetDBString(dr["Email"]);
            peopleInfo.UserType = Data.Utils.GetDBString(dr["UserType"]);
            peopleInfo.Login = Data.Utils.GetDBString(dr["UserLogin"]);
            peopleInfo.Password = Data.Utils.GetDBString(dr["UserPassword"]);
            peopleInfo.Signature = Data.Utils.GetDBString(dr["UserSignatureFile"]);
            peopleInfo.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);
            peopleInfo.Inactive = Data.Utils.GetDBBoolean(dr["Inactive"]);

            AssignAuditInfo(peopleInfo, dr);

            if (peopleInfo is ContactInfo)
            {
                if (dr["SubContractorId"] != DBNull.Value)
                {
                    ((ContactInfo)peopleInfo).SubContractor = SubContractorsController.GetInstance().CreateSubContractor(dr["SubContractorId"], subcontractorsDictionary);
                }
                //#---
                peopleInfo.Position= Data.Utils.GetDBString(dr["EmployeePosition"]);
                //#---

                //#--13/01/2020
                ((ContactInfo)peopleInfo).DOB = Data.Utils.GetDBDateTime(dr["DOB"]);
                ((ContactInfo)peopleInfo).Allergies = Data.Utils.GetDBString(dr["Allergies"]);
                ((ContactInfo)peopleInfo).EmergencyContactName = Data.Utils.GetDBString(dr["EmergencyContactName"]);
                ((ContactInfo)peopleInfo).EmergenctContactNumber = Data.Utils.GetDBString(dr["EmergencyContactNumber"]);
                //#--13/01/2020
            }
            else
            {
                if (peopleInfo is EmployeeInfo)
                {
                    ((EmployeeInfo)peopleInfo).Position = Data.Utils.GetDBString(dr["EmployeePosition"]);

                    if (dr["BusinessUnitId"] != DBNull.Value)
                    {
                        ((EmployeeInfo)peopleInfo).BusinessUnit = new BusinessUnitInfo(Data.Utils.GetDBInt32(dr["BusinessUnitId"]));
                        ((EmployeeInfo)peopleInfo).BusinessUnit.Name = Data.Utils.GetDBString(dr["BusinessUnitName"]);
                    }
                }
                else
                {
                    if (peopleInfo is ClientContactInfo)
                    {
                        ((ClientContactInfo)peopleInfo).CompanyName = Data.Utils.GetDBString(dr["CompanyName"]);
                        //#---
                        ((ClientContactInfo)peopleInfo).Position = Data.Utils.GetDBString(dr["EmployeePosition"]);//#----
                    }
                }
            }

            return peopleInfo;
        }

        /// <summary>
        /// Creates a person from a dr
        /// </summary>
        public PeopleInfo CreatePerson(IDataReader dr)
        {
            return CreatePerson(dr, null);
        }

        /// <summary>
        /// Creates a PeopleInfo object from a dr or retrieve it from the dictionary
        /// </summary>
        public PeopleInfo CreatePerson(Object dbId, Dictionary<int, PeopleInfo> peopleDictionary)
        {
            int Id = Data.Utils.GetDBInt32(dbId).Value;

            if (peopleDictionary == null)
                return GetInstance().GetPersonById(Id);
            else if (peopleDictionary.ContainsKey(Id))
                return peopleDictionary[Id];
            else
                return null;
        }

        /// <summary>
        /// Get a person from persistent storage
        /// </summary>
        public PeopleInfo GetPerson(int? peopleId, String login, String Email)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();

            parameters.Add(peopleId);
            parameters.Add(login);
            parameters.Add(Email);

            try
            {
                dr = Data.DataProvider.GetInstance().GetPerson(parameters.ToArray());
                if (dr.Read())
                    return CreatePerson(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Person from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }








        /// <summary>
        /// Get a person by ID from persistent storage
        /// </summary>
        public PeopleInfo GetPersonById(int? peopleId)
        {
            return GetPerson(peopleId, null, null);
        }

        /// <summary>
        /// Get a person by email from persistent storage
        /// </summary>
        public PeopleInfo GetPersonByEmail(String email)
        {
            return GetPerson(null, null, email);
        }

       



        /// <summary>
        /// Updates a person in the database
        /// </summary>
        /// <param name="peopleInfo">The person to update</param>
        public void UpdatePerson(PeopleInfo peopleInfo)
        {
            List<Object> parameters = new List<Object>();
            PeopleInfo currentPerson;

            // Verify that a person with same login does not exist
            if (peopleInfo.Login != null)
            {
                currentPerson = GetPerson(null, peopleInfo.Login, "");
                if (currentPerson != null && currentPerson.Id != peopleInfo.Id)
                {
                    throw new Exception("The Login: " + peopleInfo.Login + " is already registered for user: " + currentPerson.Name);
                }
            }

            // Verify that a person with same email does not exist
            if (peopleInfo.Email != null)
            {
                currentPerson = GetPerson(null, null, peopleInfo.Email);
                if (currentPerson != null && currentPerson.Id != peopleInfo.Id)
                {
                    throw new Exception("The Email: " + peopleInfo.Email + " is already registered for user: " + currentPerson.Name);
                }
            }

            SetModifiedInfo(peopleInfo);
            
            parameters.Add(peopleInfo.Id);
            parameters.Add(peopleInfo is ContactInfo ? GetId(((ContactInfo)peopleInfo).SubContractor) : null);
            parameters.Add(peopleInfo is EmployeeInfo ? GetId(((EmployeeInfo)peopleInfo).BusinessUnit) : null);
            parameters.Add(peopleInfo is ClientContactInfo ? ((ClientContactInfo)peopleInfo).CompanyName : null);
            parameters.Add(peopleInfo.FirstName);
            parameters.Add(peopleInfo.LastName);
            parameters.Add(peopleInfo.Street);
            parameters.Add(peopleInfo.Locality);
            parameters.Add(peopleInfo.State);
            parameters.Add(peopleInfo.PostalCode);
            parameters.Add(peopleInfo.Phone);
            parameters.Add(peopleInfo.Fax);
            parameters.Add(peopleInfo.Mobile);
            parameters.Add(peopleInfo.Email);
            //#-- parameters.Add(peopleInfo is EmployeeInfo ? ((EmployeeInfo)peopleInfo).Position : null);
            //#--
            parameters.Add(peopleInfo.Position);
            //#--

            parameters.Add(peopleInfo.UserType);
            parameters.Add(peopleInfo.Login);
            parameters.Add(peopleInfo.Password);
            parameters.Add(peopleInfo.Signature);
            parameters.Add(peopleInfo.LastLoginDate);
            parameters.Add(peopleInfo.Inactive);

            parameters.Add(peopleInfo.ModifiedDate);
            parameters.Add(peopleInfo.ModifiedBy);

            //#---14/01/20
           
            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).DOB:null);
            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).Allergies : null);
            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).EmergencyContactName : null);
            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).EmergenctContactNumber : null);
            
            //#--14/01/20

            try
            {
                Data.DataProvider.GetInstance().UpdatePerson(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Person in database");
            }
        }

        /// <summary>
        /// Updates a person's last login
        /// </summary>
        public void UpdatePersonLastLogin(PeopleInfo peopleInfo)
        {
            List<Object> parameters = new List<Object>();

            peopleInfo.LastLoginDate = DateTime.Now;

            parameters.Add(peopleInfo.Id);
            parameters.Add(peopleInfo.LastLoginDate);

            try
            {
                Data.DataProvider.GetInstance().UpdatePersonLastLogin(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Person's last login in database");
            }
        }

        /// <summary>
        /// Adds a person to the database
        /// </summary>
        /// <param name="peopleInfo">The person to add</param>
        public int? AddPerson(PeopleInfo peopleInfo)
        {
            int? peopleId = null;
            PeopleInfo currentPerson;
            List<Object> parameters = new List<Object>();

            // Verify that a person with same login does not exist
            if (peopleInfo.Login != null)
            {
                currentPerson = GetPerson(null, peopleInfo.Login, "");
                if (currentPerson != null)
                {
                    throw new Exception("The Login: " + peopleInfo.Login + " is already registered for user: " + currentPerson.Name);
                }
            }

            // Verify that a person with same email does not exist
            if (peopleInfo.Email != null)
            {
                currentPerson = GetPerson(null, null, peopleInfo.Email);
                if (currentPerson != null)
                {
                    throw new Exception("The Email: " + peopleInfo.Email + " is already registered for user: " + currentPerson.Name);
                }
            }

            SetCreateInfo(peopleInfo);

            parameters.Add(peopleInfo.Type);
            parameters.Add(peopleInfo is ContactInfo ? GetId(((ContactInfo)peopleInfo).SubContractor) : null);
            parameters.Add(peopleInfo is EmployeeInfo ? GetId(((EmployeeInfo)peopleInfo).BusinessUnit) : null);
            parameters.Add(peopleInfo is ClientContactInfo ? ((ClientContactInfo)peopleInfo).CompanyName : null);
            parameters.Add(peopleInfo.FirstName);
            parameters.Add(peopleInfo.LastName);
            parameters.Add(peopleInfo.Street);
            parameters.Add(peopleInfo.Locality);
            parameters.Add(peopleInfo.State);
            parameters.Add(peopleInfo.PostalCode);
            parameters.Add(peopleInfo.Phone);
            parameters.Add(peopleInfo.Fax);
            parameters.Add(peopleInfo.Mobile);
            parameters.Add(peopleInfo.Email);
            //#---  parameters.Add(peopleInfo is EmployeeInfo ? ((EmployeeInfo)peopleInfo).Position : null);
            //#--
            parameters.Add(peopleInfo.Position);
            //#--
            parameters.Add(peopleInfo.UserType);
            parameters.Add(peopleInfo.Login);
            parameters.Add(peopleInfo.Password);
            parameters.Add(peopleInfo.Signature);
            parameters.Add(peopleInfo.LastLoginDate);
            parameters.Add(peopleInfo.Inactive);

            parameters.Add(peopleInfo.CreatedDate);
            parameters.Add(peopleInfo.CreatedBy);

            //#---14/01/20

            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).DOB : null);
            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).Allergies : null);
            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).EmergencyContactName : null);
            parameters.Add(peopleInfo is ContactInfo ? ((ContactInfo)peopleInfo).EmergenctContactNumber : null);

            //#--14/01/20





            try
            {
                peopleId = Data.DataProvider.GetInstance().AddPerson(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Person in database");
            }

            return peopleId;
        }

        /// <summary>
        /// Adds or updates a person
        /// </summary>
        public int? AddUpdatePerson(PeopleInfo peopleInfo)
        {
            if (peopleInfo != null)
            {
                if (peopleInfo.Id != null)
                {
                    UpdatePerson(peopleInfo);
                    return peopleInfo.Id;
                }
                else
                {
                    return AddPerson(peopleInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a person from persistent storage
        /// </summary>
        public void DeletePerson(PeopleInfo peopleInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeletePerson(peopleInfo.Id);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Removing Person from database. It might be in use.");
            }
        }


        //#--To check existing Client for the Project
        public Boolean RegisteredClientAccess(int PeopleId, int ProjectId)
        {
            List<Object> parameters = new List<Object>();
            parameters.Add(PeopleId);
            parameters.Add(ProjectId);

          return  Data.DataProvider.GetInstance().RegisteredClientAccess(parameters.ToArray());
             
        }


        /// <summary>
        /// //#--  to Add Client contact  person  to  Clientaccess Table
        /// </summary>
        public void AddClientAccess(int PeopleId,int ProjectId)
        {

            bool registeredClient=false;
            List<Object> parameters = new List<Object>();

            parameters.Add(PeopleId);
            parameters.Add(ProjectId);
            parameters.Add(Web.Utils.GetCurrentUserId());//---Created by

            registeredClient = RegisteredClientAccess(PeopleId, ProjectId);

            if(!registeredClient)
            Data.DataProvider.GetInstance().AddClientAccess(parameters.ToArray());

        }

        //public void UpdateClientDistAccess(int PeopleId, int ProjectId,bool EOTs,bool RFIs,bool Claims, bool SAs,bool CVs,bool AtEOTs,bool AtRFIs,bool AtClaims)
        //#---
        public void UpdateClientDistAccess(ClientContactInfo clientInfo, int ProjectId)
        {
            bool registeredClient = false;
            List<Object> parameters = new List<Object>();

            parameters.Add(clientInfo.Id);
            parameters.Add(ProjectId);
            parameters.Add(clientInfo.SendEOTs);
            parameters.Add(clientInfo.SendRFIs);
            parameters.Add(clientInfo.SendClaims);
            parameters.Add(clientInfo.SendSAs);
            parameters.Add(clientInfo.SendCVs);
            parameters.Add(clientInfo.AttentionToEots);
            parameters.Add(clientInfo.AttentionToRFIs);
            parameters.Add(clientInfo.AttentionToClaims);

            parameters.Add(Web.Utils.GetCurrentUserId());//---ModefiedBy 
            parameters.Add(DateTime.Now);

            if (!registeredClient)
                Data.DataProvider.GetInstance().UpdateClientDistAccess(parameters.ToArray());

        }


        //#---
        // public void UpdateClientWebAccess(int PeopleId, int ProjectId, bool EOTs, bool RFIs, bool Claims, bool SAs, bool CVs, bool Docs, bool Photos)
            public void UpdateClientWebAccess(ClientContactInfo clientInfo, int ProjectId)
         {
            bool registeredClient = false;
            List<Object> parameters = new List<Object>();

            parameters.Add(clientInfo.Id);
            parameters.Add(ProjectId);
            parameters.Add(clientInfo.WebAccessToEOTs);
            parameters.Add(clientInfo.WebAccessToRFIs);
            parameters.Add(clientInfo.WebAccessToClaims);
            parameters.Add(clientInfo.WebAccessToSAs);
            parameters.Add(clientInfo.WebAccessToCVs);
            parameters.Add(clientInfo.WebAccessToDocs);
            parameters.Add(clientInfo.WebAccessToPhotos);
            parameters.Add(Web.Utils.GetCurrentUserId());//---ModefiedBy 
            parameters.Add(DateTime.Now);

            if (!registeredClient)
                Data.DataProvider.GetInstance().UpdateClientWebAccess(parameters.ToArray());

        }


        //#---To Get deep Client Contact by Id

        public ClientContactInfo GetDeepClientContact(int? PeopleId,int?ProjectId)
        {
            ClientContactInfo clientContact = (ClientContactInfo)GetPersonById(PeopleId);
            GetClientAccessOnProject(clientContact,ProjectId);

            return clientContact;
        }


        public ClientContactInfo GetClientAccessOnProject(ClientContactInfo clientContact,int?ProjectId)
        {
            List<Object> parameters = new List<Object>();
            if (clientContact.Id != null)
            {
                parameters.Add(clientContact.Id);
                parameters.Add(ProjectId);

                IDataReader dr = null;
                dr = DataProvider.GetInstance().GetClientAccess(parameters.ToArray());
                while (dr.Read())
                {
                    clientContact = (ClientContactInfo)GetPersonById(Data.Utils.GetDBInt32(dr["PeopleId"]));
                    clientContact.SendEOTs = Data.Utils.GetDBBoolean(dr["DistEOTs"]);
                    clientContact.SendRFIs = Data.Utils.GetDBBoolean(dr["DistRFIs"]);
                    clientContact.SendClaims = Data.Utils.GetDBBoolean(dr["DistClaims"]);
                    clientContact.SendSAs = Data.Utils.GetDBBoolean(dr["DistSaparateAccounts"]);
                    clientContact.SendCVs = Data.Utils.GetDBBoolean(dr["DistClientVariations"]);

                    clientContact.AttentionToEots = Data.Utils.GetDBBoolean(dr["AttentionEOT"]);
                    clientContact.AttentionToRFIs = Data.Utils.GetDBBoolean(dr["AttentionRFI"]);
                    clientContact.AttentionToClaims = Data.Utils.GetDBBoolean(dr["AttentionClaim"]);

                    clientContact.WebAccessToEOTs = Data.Utils.GetDBBoolean(dr["WebEots"]);
                    clientContact.WebAccessToRFIs = Data.Utils.GetDBBoolean(dr["WebRFIs"]);
                    clientContact.WebAccessToClaims = Data.Utils.GetDBBoolean(dr["WebClaims"]);
                    clientContact.WebAccessToSAs = Data.Utils.GetDBBoolean(dr["WebSeparateAccounts"]);
                    clientContact.WebAccessToCVs = Data.Utils.GetDBBoolean(dr["WebClientVariations"]);
                    clientContact.WebAccessToDocs = Data.Utils.GetDBBoolean(dr["WebDocs"]);
                    clientContact.WebAccessToPhotos = Data.Utils.GetDBBoolean(dr["WebPhotos"]);
                }
            }
            return clientContact;
        }

        // to get list of project on which clientcontact is working--#--
        public List<ProjectInfo> GetClientProjects(int? PeopleId)
        {
            ProjectInfo projectInfo = null;
            List<ProjectInfo> projectList = new List<ProjectInfo>();
            List<Object> parameters = new List<object>();
            IDataReader dr = null;

            parameters.Add(PeopleId);
            try
            { 
                    if (PeopleId != null)
                        dr = DataProvider.GetInstance().GetClientProjects(parameters.ToArray());
                    while (dr.Read())
                    {
                        projectInfo = ProjectsController.GetInstance().GetProject(Data.Utils.GetDBInt32(dr["ProjectId"]));
                        projectList.Add(projectInfo);
                    }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting project list from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return projectList;



        }

        ///#---

        /// <summary>
        /// Converts a client contact to a contact
        /// </summary>
        public ContactInfo ConvertClientContactToContact(ClientContactInfo clientContactInfo)
        {
            ContactInfo contactInfo = new ContactInfo();
            contactInfo.SubContractor = new SubContractorInfo();

            contactInfo.SubContractor.Name = clientContactInfo.CompanyName;

            contactInfo.FirstName = clientContactInfo.FirstName;
            contactInfo.LastName = clientContactInfo.LastName;
            contactInfo.Street = clientContactInfo.Street;
            contactInfo.Locality = clientContactInfo.Locality;
            contactInfo.State = clientContactInfo.State;
            contactInfo.PostalCode = clientContactInfo.PostalCode;

            return contactInfo;
        }


        #region Qualifications

        public List<QualificationInfo> GetQualificationsByContactId(int? Id) {

            QualificationInfo qualificationInfo = null;
            List<QualificationInfo> qualificationList = new List<QualificationInfo>();
            List<Object> parameters = new List<object>();
            IDataReader dr = null;
            parameters.Add(Id);
            try
            {
                if (Id != null)
                    dr = DataProvider.GetInstance().GetQualificationsByContactId(Id);
                while (dr.Read())
                {
                    qualificationInfo = new QualificationInfo(Data.Utils.GetDBInt32(dr["Id"]));
                    qualificationInfo.qualificationName = Data.Utils.GetDBString(dr["QualificationName"]);
                    qualificationInfo.cardNumber= Data.Utils.GetDBString(dr["CardNumber"]);
                    qualificationInfo.expiryDate = Data.Utils.GetDBDateTime(dr["ExpiryDate"]);
                    qualificationInfo.imageName = Data.Utils.GetDBString(dr["ImageName"]);
                    qualificationInfo.imagePath = Data.Utils.GetDBString(dr["ImagePath"]);


                    qualificationList.Add(qualificationInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting qualification list from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

                     

            return qualificationList;
        }

        public void UpdateQualification(QualificationInfo qInfo)
        {
           
            List<Object> parameters = new List<Object>();

            parameters.Add(qInfo.Id);
            parameters.Add(qInfo.qualificationName);
            parameters.Add(qInfo.cardNumber);
            parameters.Add(qInfo.expiryDate);
            parameters.Add(qInfo.imageName);
            parameters.Add(qInfo.imagePath);
            parameters.Add(Web.Utils.GetCurrentUserId());//---ModefiedBy 
            parameters.Add(DateTime.Now);

           
                Data.DataProvider.GetInstance().UpdateQualifiaction(parameters.ToArray());

        }

        public int? AddQualification(QualificationInfo qInfo)
        {

            List<Object> parameters = new List<Object>();
            parameters.Add(qInfo.contactInfo.Id);           
            parameters.Add(qInfo.qualificationName);
            parameters.Add(qInfo.cardNumber);
            parameters.Add(qInfo.expiryDate);
            parameters.Add(qInfo.imageName);
            parameters.Add(qInfo.imagePath);
            parameters.Add(Web.Utils.GetCurrentUserId());
            //parameters.Add(DateTime.Now);


           return Data.DataProvider.GetInstance().AddQualification(parameters.ToArray());

        }



        public void DeleteQualification(QualificationInfo qInfo)
        {

            List<Object> parameters = new List<Object>();
            parameters.Add(qInfo.Id);
            parameters.Add(Web.Utils.GetCurrentUserId());
            parameters.Add(DateTime.Now);
           
             Data.DataProvider.GetInstance().DeleteQualification(parameters.ToArray());

        }




        #endregion  //#---




        #endregion

    }
}
