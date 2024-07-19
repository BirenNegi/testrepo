using System;

namespace SOS.Core
{
    [Serializable]
    public class MinutesTemplateInfo : Info
    {

#region Private Members
        private String template;
#endregion

#region Constructors
        public MinutesTemplateInfo() 
        {
        }

        public MinutesTemplateInfo(int? minutesTemplateId)
        {
            Id = minutesTemplateId;
        }
#endregion

#region Public properties
        public String Template
        {
            get { return template; }
            set { template = value; }
        }
#endregion

    }
}
