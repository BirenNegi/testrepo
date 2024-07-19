using System;

namespace SOS.Core
{
    [Serializable]
    public class ClientTradeTypeInfo : Info
    {

#region Private Members
        private String name;
#endregion

#region Constructors
        public ClientTradeTypeInfo() 
        {
        }

        public ClientTradeTypeInfo(int? clientTradeTypeId)
        {
            Id = clientTradeTypeId;
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
