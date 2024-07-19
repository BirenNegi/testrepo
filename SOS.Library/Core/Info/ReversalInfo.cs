using System;

namespace SOS.Core
{
    [Serializable]
    public class ReversalInfo : Info
    {

#region Private Members
        private String reversalNote;
        private String replyNote;
        private DateTime? reversalDate;
        private DateTime? replyDate;

        private PeopleInfo reversalBy;
        private PeopleInfo replyBy;
        private ProcessStepInfo processStep;
#endregion

#region Constructors
        public ReversalInfo() 
        {
        }

        public ReversalInfo(int? reversalId)
        {
            Id = reversalId;
        }
#endregion

#region Public properties
        public String ReversalNote
        {
            get { return reversalNote; }
            set { reversalNote = value; }
        }

        public String ReplyNote
        {
            get { return replyNote; }
            set { replyNote = value; }
        }

        public DateTime? ReversalDate
        {
            get { return reversalDate; }
            set { reversalDate = value; }
        }

        public DateTime? ReplyDate
        {
            get { return replyDate; }
            set { replyDate = value; }
        }

        public PeopleInfo ReversalBy
        {
            get { return reversalBy; }
            set { reversalBy = value; }
        }

        public PeopleInfo ReplyBy
        {
            get { return replyBy; }
            set { replyBy = value; }
        }

        public ProcessStepInfo ProcessStep
        {
            get { return processStep; }
            set { processStep = value; }
        }

        public String ReversalByName
        {
            get { return reversalBy != null ? reversalBy.Name : null; }
        }

        public String ReplyByName
        {
            get { return replyBy != null ? replyBy.Name : null; }
        }

        public Boolean IsPending
        {
            get { return replyDate == null; }
        }
#endregion

    }
}
