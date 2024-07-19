using System;

namespace SOS.Core
{
    /// <summary>
    /// Base class for all the info classes
    /// </summary>
    [Serializable]
    public abstract class Info : IComparable, IEquatable<Info>
    {

#region Constants
        public const String TypeActive = "A";
        public const String TypeProposal = "P";
        #endregion

        #region Private Members
        private Guid? Docid;  // DS202210
        private int? id;
        private String type;
        private DateTime? createdDate;
        private DateTime? modifiedDate;
        private int? createdBy;
        private int? modifiedBy;
#endregion

#region Constructors
        public Info() 
        {
        }
        #endregion

        #region Public properties
        public Guid? DocId   //DS202210
        {
            get { return Docid; }
            set { Docid = value; }
        }
        public int? Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public String IdStr
        {
            get { return id.HasValue ? Convert.ToString(Id.Value) : null; }
        }

        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public DateTime? ModifiedDate
        {
            get { return modifiedDate; }
            set { modifiedDate = value; }
        }

        public int? CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public int? ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; }
        }

        public Boolean IsActive
        {
            get { return Type != null && Type == TypeActive; }
        }

        public Boolean IsProposal
        {
            get { return Type != null && Type == TypeProposal; }
        }
#endregion

#region Public Methods
        public int CompareTo(Object obj)
        {
            if (this.GetType().Name == obj.GetType().Name)
            {                
                if (this.Id != null && ((Info)obj).Id != null)
                {
                    return ((int)this.Id).CompareTo((int)((Info)obj).Id);
                }
                else
                {
                    throw new ArgumentException("Info objects have null Ids.");
                }
            }
            else
            {
                throw new ArgumentException("Info objects are not the same type.");
            }
        }

        public bool Equals(Info other)
        {
            if (other == null)
                return false;

            if (this.GetType().Name == other.GetType().Name)
            {
                if (this.Id != null && other.Id != null)
                {
                     return ((int)this.Id) == ((int)other.Id);
                }
                else
                {
                    throw new ArgumentException("Info objects have null Ids.");
                }
            }
            else
            {
                throw new ArgumentException("Info objects are not the same type.");
            }
        }

        public bool EqualsType(Info other)
        {
            if (other == null)
                return false;

            if (this.GetType().Name == other.GetType().Name)
            {
                if (this.id == null)
                {
                    if (other.id == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (other.id == null)
                    {
                        return false;
                    }
                    else
                    {
                        return ((int)this.id) == ((int)other.id);
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Id == null ? 0 : (int)Id;
        }

		public bool IsNew
		{
			get {return Id == null;}
		}
#endregion

    }
}
