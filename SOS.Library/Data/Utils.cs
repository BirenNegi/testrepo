using System;

namespace SOS.Data 
{
    public class Utils
    {

#region Public Static Methods
        public static bool? GetDBBoolean(Object obj)
        {
            return obj != DBNull.Value ? (bool?)Convert.ToBoolean(obj) : null;
        }

        public static int? GetDBInt32(Object obj)
        {
            return obj != DBNull.Value ? (int?)Convert.ToInt32(obj) : null;
        }

        public static decimal? GetDBDecimal(Object obj)
        {
            return obj != DBNull.Value ? (decimal?)Convert.ToDecimal(obj) : null;
        }

        public static float? GetDBFloat(Object obj)
        {
            return obj != DBNull.Value ? (float?)Convert.ToSingle(obj) : null;
        }

        public static String GetDBString(Object obj)
        {
            return obj != DBNull.Value ? Convert.ToString(obj) : null;
        }

        public static DateTime? GetDBDateTime(Object obj)
        {
            return obj != DBNull.Value ? (DateTime?)Convert.ToDateTime(obj) : null;
        }

#endregion

    }
}
