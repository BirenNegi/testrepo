using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class TenantVariationInfo : ClientVariationInfo
    {

        #region Constructors
        public TenantVariationInfo()
        {
            Type = ClientVariationInfo.VariationTypeTenant;
        }

        public TenantVariationInfo(int? tenantVariationInfoId)
        {
            Type = ClientVariationInfo.VariationTypeTenant;
            Id = tenantVariationInfoId;
        }
    }
}
#endregion
