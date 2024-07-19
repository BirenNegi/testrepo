using System;

namespace SOS.Core
{
	public interface IClaimDetail
	{

#region Public properties
		Decimal? Amount
		{
			get;
			set;
		}

		ClaimInfo Claim
		{
			get;
			set;
		}
#endregion

	}
}
