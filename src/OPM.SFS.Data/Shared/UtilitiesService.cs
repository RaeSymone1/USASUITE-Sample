using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Shared
{
	public interface IUtilitiesService
	{
		DateTime ConvertUtcToEastern(DateTime utc);
	}

	public class UtilitiesService : IUtilitiesService
	{
		public DateTime ConvertUtcToEastern(DateTime utc)
		{
			var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
			return TimeZoneInfo.ConvertTimeFromUtc(utc, easternZone);
		}
	}
}
