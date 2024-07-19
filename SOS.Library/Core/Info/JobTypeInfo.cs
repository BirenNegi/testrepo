using System;

namespace SOS.Core
{
    [Serializable]
    public class JobTypeInfo : Info
    {

#region Private Members
        private String name;
#endregion

#region Constructors
        public JobTypeInfo()
        {
        }

        public JobTypeInfo(int? jobTypeId)
        {
            Id = jobTypeId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
#endregion

    }
}
