using System;
using System.Data;

using SOS.Data;
using SOS.Web;

namespace SOS.Core
{
    /// <summary>
    /// Base class for all the controller classes
    /// </summary>
    public abstract class Controller
    {

#region Public Methods
        public int? GetId(Info info)
        {
            if (info != null)
            {
                if (info.Id.HasValue)
                {
                    return info.Id.Value;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void AssignAuditInfo(Info info, IDataReader dr)
        {
            info.CreatedDate = Data.Utils.GetDBDateTime(dr["CreatedDate"]);
            info.ModifiedDate = Data.Utils.GetDBDateTime(dr["ModifiedDate"]);
            info.CreatedBy = Data.Utils.GetDBInt32(dr["CreatedPeopleId"]);
            info.ModifiedBy = Data.Utils.GetDBInt32(dr["ModifiedPeopleId"]);
        }

        public void SetCreateInfo(Info info)
        {
            info.CreatedDate = DateTime.Now;
            info.CreatedBy = Web.Utils.GetCurrentUserId();
        }

        public void SetModifiedInfo(Info info)
        {
            info.ModifiedDate = DateTime.Now;
            info.ModifiedBy = Web.Utils.GetCurrentUserId();
        }
#endregion

    }
}
