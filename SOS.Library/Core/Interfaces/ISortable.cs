using System;

namespace SOS.Core
{
    public interface ISortable
    {

#region Public properties
        int? DisplayOrder
        {
            get;
            set;
        }

        int? Id
        {
            get;
            set;
        }
#endregion

    }
}
