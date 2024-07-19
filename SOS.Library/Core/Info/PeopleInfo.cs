using System;

namespace SOS.Core
{
    [Serializable]
    public abstract class PeopleInfo : Info
    {

#region Constants
        public const String PeopleTypeContact = "CO";
        public const String PeopleTypeClientContact = "CL";
        public const String PeopleTypeEmployee = "EM";
        public const String PeopleTypeSubContractor = "SC";  //DS202211
        #endregion

        #region Private Members
        private String firstName;
        private String lastName;
        private String street;
        private String locality;
        private String state;
        private String postalCode;
        private String phone;
        private String fax;
        private String mobile;
        private String email;
        private String userType;
        private String login;
        private String password;
        private DateTime? lastLoginDate;
        private String signature;
        private Boolean? inactive;
        //#--
        private String position;
        //#--

        #endregion

        #region Constructors
        public PeopleInfo()
        {
        }

        public PeopleInfo(int? peopleId)
        {
            Id = peopleId;
        }
#endregion

#region Public properties
        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String LastName
        {
            get { return lastName; }
            set { lastName = value; }
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

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String UserType
        {
            get { return userType; }
            set { userType = value; }
        }

        public String Login
        {
            get { return login; }
            set { login = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        public DateTime? LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }

        public String Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        public Boolean? Inactive
        {
            get { return inactive; }
            set { inactive = value; }
        }

        public String Position
        {
            get { return position; }
            set { position = value; }
        }

       //#--
        public InductionResultInfo InductionResult { get; set;}    //#--


        public String Name
        {
            get
            {
                if (FirstName != null)
                    if (LastName != null)
                        return FirstName + " " + LastName;
                    else
                        return FirstName;
                else
                    if (LastName != null)
                        return LastName;
                    else
                        return null;
            }
        }

        public String Initials
        {
            get
            {
                if (FirstName != null)
                    if (LastName != null)
                        return FirstName.Substring(0, 1) + lastName.Substring(0, 1);
                    else
                        return FirstName.Substring(0, 1) + "_";
                else
                    if (LastName != null)
                        return "_" + lastName.Substring(0, 1);
                    else
                        return null;
            }
        }

        public Boolean HasAccount
        {
            get { return Login != null; }
        }

        public Boolean IsInactive
        {
            get { return inactive != null && inactive.Value; }
        }
#endregion

    }
}
