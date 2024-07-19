using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ContactInfo : PeopleInfo
    {

#region Private Members
        private SubContractorInfo subContractor;

        //#---
        public const String SubcontractorAdmin = "SA";
        //#--

        #endregion

        #region Constructors
        public ContactInfo()
        {
            Type = PeopleInfo.PeopleTypeContact;
        }

        public ContactInfo(int? contactId)
        {
            Type = PeopleInfo.PeopleTypeContact;
            Id = contactId;
        }
#endregion

#region Public properties
        public SubContractorInfo SubContractor
        {
            get { return subContractor; }
            set { subContractor = value; }
        }

        public String SubContractorName
        {
            get { return subContractor != null ? SubContractor.Name : String.Empty; }
        }

        public String SubContractorIdStr
        {
            get { return subContractor != null ? SubContractor.IdStr : String.Empty; }
        }

        public String BusinessUnitName
        {
            get { return SubContractor != null ? SubContractor.BusinessUnitName : String.Empty; }
        }

        //#---13/01/2020

        public DateTime? DOB { get; set; }
        public String Allergies { get; set; }

        public String EmergencyContactName { get; set; }

        public String EmergenctContactNumber { get; set; }

        public List<QualificationInfo> Qualifications { get; set; }


        public bool IsSubcontractorAdmin
        {
            get { return UserType!=null? UserType == SubcontractorAdmin ? true : false:false; }
        } 
        //#---13/01/2020
        #endregion

    }
}
