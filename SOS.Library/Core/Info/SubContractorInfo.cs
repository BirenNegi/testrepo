using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class SubContractorInfo : Info
    {

#region Private Members
        private String name;
        private String shortName;
        private String street;
        private String locality;
        private String state;
        private String postalCode;
        private String phone;
        private String fax;
        private String mobile;
        private String website;
        private String account;
        private String abn;
        //#----
        private String prequalifiedform;
        private String publicLiabilityInsurance;
        private String workCoverInsurance;
        private String professionalIndemnityInsurance;
        private bool? dcContractor;
        private String acn;
        private String licencenumber;
        private List<BusinessUnitInfo> businessUnits;
        //#----
        private String comments;

        private BusinessUnitInfo businessUnit;
        private List<ContactInfo> contacts;

#endregion

#region Constructors
        public SubContractorInfo()
        {
        }

        public SubContractorInfo(int? subContractorId)
        {
            Id = subContractorId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String ShortName
        {
            get { return shortName; }          
            set { shortName = value; }
        }

        public String Street
        {
            get { return street; }
            set { street = value; }
        }

        public String Locality
        {
            get { return locality; }
            set { locality = value; }
        }

        public String State
        {
            get { return state; }
            set { state = value; }
        }

        public String PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public String Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public String Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public String Website
        {
            get { return website; }
            set { website = value; }
        }

        public String Account
        {
            get { return account; }
            set { account = value; }
        }

        public String Abn
        {
            get { return abn; }
            set { abn = value; }
        }

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        //#-----
        public String PrequalifiedForm
        {
            get { return prequalifiedform; }
            set { prequalifiedform = value; }
        }

        public String PublicLiabilityInsurance
        {
            get { return publicLiabilityInsurance; }
            set { publicLiabilityInsurance = value; }
        }

        public String WorkCoverInsurance
        {
            get { return workCoverInsurance; }
            set { workCoverInsurance = value; }
        }

        public String ProfessionalIndemnityInsurance
        {
            get { return professionalIndemnityInsurance; }
            set { professionalIndemnityInsurance = value; }
        }

        public bool? DCContractor {
            get { return dcContractor; }
            set { dcContractor = value; }
        }

        public String ACN
        {
            get { return acn; }
            set { acn = value; }
        }

        public String LicenceNumber
        {
            get { return licencenumber; }
            set { licencenumber = value; }
        }

        public List<BusinessUnitInfo> BusinessUnitslist
        {
            get { return businessUnits;}
            set { businessUnits = value; }

        }
        //#----


        public BusinessUnitInfo BusinessUnit
        {
            get { return businessUnit; }
            set { businessUnit = value; }
        }

        public List<ContactInfo> Contacts
        {
            get { return contacts; }
            set { contacts = value; }
        }

        public String ShortNameOrName
        {
            get { return ShortName != null ? ShortName : Name; }
        }

        public String BusinessUnitName
        {
            get { return BusinessUnit == null ? null : BusinessUnit.Name; }
        }

        public ContactInfo DefaultContact
        {
            get
            {
                if (Contacts != null)
                    if (Contacts.Count >= 1)
                        return Contacts[0];

                return null;
            }
        }



#endregion

    }
}
