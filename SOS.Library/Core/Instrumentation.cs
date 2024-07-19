using System;
using System.Diagnostics;


namespace SOS.Core
{
	public class Instrumentation
	{

#region Constants
		public const String categoryName = "SOSCounters";
		public const String categoryHelp = "SOS Counters";
		public const String counterLoggedInUsersName = "UsersLoggedIn";
		public const String counterLoggedInUsersHelp = "Number of users logged in";
#endregion

#region Public Static Methods
		/// <summary>
		/// Creates the performance counters.
		/// </summary>
		public static void CreatePerformanceCounters()
		{
			PerformanceCounterCategory performanceCounterCategory;

			if (PerformanceCounterCategory.Exists(categoryName))
				PerformanceCounterCategory.Delete(categoryName);

			performanceCounterCategory = PerformanceCounterCategory.Create(categoryName, categoryHelp, PerformanceCounterCategoryType.SingleInstance, counterLoggedInUsersName, counterLoggedInUsersHelp);
		}

		/// <summary>
		/// Updates UsersLoggedIn counter
		/// </summary>
		public static void UpdateLoggedInUsers(Boolean isLogIn)
		{
			PerformanceCounter performanceCounter = new PerformanceCounter(categoryName, counterLoggedInUsersName, false);

			if (isLogIn)
				performanceCounter.Increment();
			else
				performanceCounter.Decrement();
		}
#endregion

	}
}
