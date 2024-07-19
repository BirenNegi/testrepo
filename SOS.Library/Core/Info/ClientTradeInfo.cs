using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ClientTradeInfo : Info, ISortable
    {

#region Private Members
        private String name;
        private Decimal? amount;
        private Decimal? claimed;
        private int? displayOrder;

        private ProjectInfo project;
#endregion

#region Constructors
        public ClientTradeInfo() 
        {
        }

        public ClientTradeInfo(int? clientTradeId)
        {
            Id = clientTradeId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Decimal? Claimed
        {
            get { return claimed; }
            set { claimed = value; }
        }

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        public int? DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public Decimal? ProjectPercentage
        {
            get
            {
                if (Project != null)
                    if (Project.ContractAmount != null)
                        if (project.ContractAmount != 0)
                            if (Amount != null)
                                return (Amount * 100) / project.ContractAmount;
                            else
                                return 0;

                return null;
            }


        }
#endregion

    }
}
