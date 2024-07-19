using System;

namespace SOS.Core
{
    [Serializable]
    public class InvitationTemplateInfo : Info
    {

#region Private Members
        private String template;
#endregion

#region Constructors
        public InvitationTemplateInfo() 
        {
        }

        public InvitationTemplateInfo(int? invitationTemplateId)
        {
            Id = invitationTemplateId;
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
