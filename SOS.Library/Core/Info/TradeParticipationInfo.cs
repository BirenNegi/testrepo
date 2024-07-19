using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class TradeParticipationInfo : Info
    {

#region Constants
        public enum StatusEnum { New, Invited, InvitedOnLine, Tendering, Submitted, Awarded, NotAwarded, PulledOut }
#endregion

#region Private Members
        private DateTime? invitationDate;
        private DateTime? quoteDate;
        private DateTime? quoteDueDate;
        private String quoteFile;
        private DateTime? openDate;
        private DateTime? reminderDate;
        private Decimal? amount;
        private Boolean? pulledOut;
        private int? rank;
        private String comments;
        //#---
        private String internalcomments;
        private String safetyrisk;
        //#---
        private Boolean hasActiveParticipation;
        private String paymentterms;  // DS20230820

        private TradeInfo trade;
        private SubContractorInfo subContractor;
        private ContactInfo contact;
        private TradeParticipationInfo comparisonParticipation;
        private TradeParticipationInfo quoteParticipation;
        private TransmittalInfo transmittal;
        private List<ParticipationItemInfo> participationItems;
#endregion

#region Constructors
        public TradeParticipationInfo() 
        {
        }

        public TradeParticipationInfo(int? tradeParticipationId)
        {
            Id = tradeParticipationId;
        }
#endregion

#region Public properties
        public DateTime? InvitationDate
        {
            get { return invitationDate; }
            set { invitationDate = value; }
        }

        public DateTime? QuoteDate
        {
            get { return quoteDate; }
            set { quoteDate = value; }
        }

        public DateTime? QuoteDueDate
        {
            get { return quoteDueDate; }
            set { quoteDueDate = value; }
        }

        public DateTime? OpenDate
        {
            get { return openDate; }
            set { openDate = value; }
        }

        public String QuoteFile
        {
            get { return quoteFile; }
            set { quoteFile = value; }
        }

        public DateTime? ReminderDate
        {
            get { return reminderDate; }
            set { reminderDate = value; }
        }

        public Decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Boolean? PulledOut
        {
            get { return pulledOut; }
            set { pulledOut = value; }
        }

        public int? Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        //---#
        public String InternalComments
        {
            get { return internalcomments; }
            set { internalcomments = value; }
        }


       public String safetyRisk
        {
            get { return safetyrisk; }
            set { safetyrisk = value;}
        }
        //---#
        public Boolean HasActiveParticipation
        {
            get { return hasActiveParticipation; }
            set { hasActiveParticipation = value; }
        }

        public TradeInfo Trade
        {
            get { return trade; }
            set { trade = value; }
        }

        public SubContractorInfo SubContractor
        {
            get { return subContractor; }
            set { subContractor = value; }
        }

        public ContactInfo Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        public TradeParticipationInfo QuoteParticipation
        {
            get { return quoteParticipation; }
            set { quoteParticipation = value; }
        }

        public TradeParticipationInfo ComparisonParticipation
        {
            get { return comparisonParticipation; }
            set { comparisonParticipation = value; }
        }

        public TransmittalInfo Transmittal
        {
            get { return transmittal; }
            set { transmittal = value; }
        }
        public String PaymentTerms    //DS20230920
        {
            get { return paymentterms; }
            set { paymentterms = value; }
        }

        public List<ParticipationItemInfo> ParticipationItems
        {
            get { return participationItems; }
            set { participationItems = value; }
        }

        public String SubcontractorName
        {
            get { return SubContractor != null ? SubContractor.Name : String.Empty; }
        }

        public String SubcontractorShortName
        {
            get { return SubContractor != null ? SubContractor.ShortName : String.Empty; }
        }

        public String ProjectNumber
        {
            get { return (Trade != null) ? Trade.ProjectNumber : String.Empty; }
        }

        public String ProjectName
        {
            get { return (Trade != null) ? Trade.ProjectName : String.Empty; }
        }

        public String TradeName
        {
            get { return (Trade != null) ? Trade.Name : String.Empty; }
        }

        public Boolean IsClosed
        {
            get { return QuoteDueDate != null && QuoteDueDate <= DateTime.Now; }
        }

        public Boolean IsVisible
        {
            get { return Status == StatusEnum.Invited || Status == StatusEnum.InvitedOnLine || Status == StatusEnum.Tendering || Status == StatusEnum.Submitted || Status == StatusEnum.Awarded || Status == StatusEnum.NotAwarded; }
        }

        public Boolean IsEditable
        {
            get { return (Status == StatusEnum.InvitedOnLine || Status == StatusEnum.Tendering) && !IsClosed; }
        }

        public Boolean IsSubmittable
        {
            get { return Status == StatusEnum.Tendering && !IsClosed; }
        }

        public Boolean IsPulledOut
        {
            get { return PulledOut != null && (Boolean)PulledOut; }
        }

        public Boolean IsInvited
        {
            get { return InvitationDate != null; }
        }

        public Boolean IsReminded
        {
            get { return ReminderDate != null; }
        }

        public Boolean IsOpen
        {
            get { return OpenDate != null; }
        }

        public Boolean IsSubmitted
        {
            get { return QuoteDate != null; }
        }

        public Boolean IsEmpty
        {
            get
            {
                //#--- if (Amount != null || Comments != null)

                if (Amount != null || Comments != null|| InternalComments!=null)//#--
                    return false;

                if (ParticipationItems == null || ParticipationItems.Count == 0)
                    return true;

                foreach (ParticipationItemInfo participationItemInfo in ParticipationItems)
                    if (participationItemInfo.Quantity != null || participationItemInfo.IsIncluded != null || participationItemInfo.Amount != null || participationItemInfo.Confirmed != null || participationItemInfo.Notes != null)
                        return false;
                
                return true;
            }
        }

        public Boolean IsAwarded
        {
            get
            {
                return Rank != null ? Rank == 1 : false;
            }
        }

        public Boolean IsNotAwarded
        {
            get
            {
                return Rank != null ? Rank > 1 : false;
            }
        }

        public Boolean CanSendInvitation
        {
            get
            {
                return Status == StatusEnum.New;
            }
        }

        public bool CanSendReminder
        {
            get
            {
                return Status == StatusEnum.Invited || Status == StatusEnum.InvitedOnLine || Status == StatusEnum.Tendering;
            }
        }

        public StatusEnum Status
        {
            get
            {
                if (IsPulledOut)
                    return StatusEnum.PulledOut;

                if (IsAwarded)
                    return StatusEnum.Awarded;

                if (IsNotAwarded)
                    return StatusEnum.NotAwarded;

                if (IsSubmitted)
                    return StatusEnum.Submitted;

                if (IsOpen)
                    return StatusEnum.Tendering;

                if (quoteParticipation != null)
                    return StatusEnum.InvitedOnLine;

                if (IsInvited)
                    return StatusEnum.Invited;

                return StatusEnum.New;
            }
        }

        public String StatusName
        {
            get
            {
                switch (Status)
                {
                    case StatusEnum.New: return "New";
                    case StatusEnum.Invited: return "Invited";
                    case StatusEnum.InvitedOnLine: return "Invited online";
                    case StatusEnum.Tendering: return "Tendering";
                    case StatusEnum.Submitted: return "Submitted";
                    case StatusEnum.Awarded: return "Awarded";
                    case StatusEnum.NotAwarded: return "Not awarded";
                    case StatusEnum.PulledOut: return "Pulled out";
                    default: return "Unknown";
                }
            }
        }

        public String StatusNameSubcontractor
        {
            get
            {
                if (Status != StatusEnum.Awarded && Status != StatusEnum.NotAwarded || Trade.ContractApproved || Type == Info.TypeProposal)
                {
                    return StatusName;
                }
                else
                {
                    return "Comparison";
                }
            }
        }

        public TimeSpan ClosingTime
        {
            get { return IsSubmittable && QuoteDueDate != null && QuoteDueDate > DateTime.Now ? ((DateTime)QuoteDueDate).Subtract(DateTime.Now) : TimeSpan.Zero; }
        }

        public String QuoteFileName
        {
            get
            {
                if (QuoteFile != null)
                {
                    String[] splitArray = QuoteFile.Split('.');

                    if (splitArray.Length > 1)
                    {
                        return String.Format("{0}.{1}", IdStr, splitArray[splitArray.Length - 1]);
                    }
                    else
                    {
                        return IdStr;
                    }
                }

                return null;
            }
        }
#endregion

    }
}
