using System;
using Server.TimeSystem;
using Server.Commands;

namespace Server
{
	public static class UODateTime
	{
		public struct DateTimeInfo
		{
			public short Minute;
			public short Hour;
			public short Day;
			public short Month;
			public short Year;
			public short Season;
			public short MoonPhase;
		}
		
		public static DateTimeInfo Now()
		{
			long Seconds = (long)(new TimeSpan(DateTime.Now.Ticks - (new DateTime(1934, 12, 22, 8, 0, 0)).Ticks)).TotalSeconds;
			long Minutes = Seconds / 5;
			long Hours = Minutes / 60;
			long Days = Hours / 24;
			long Months = Days / 73;
			long Years = Months / 12;
			
			DateTimeInfo info;
			info.Minute = (short)(Minutes % 60);
			info.Hour = (short)(Hours % 24);
			info.Day = (short)(Days % 73 + 1);
			info.Month = (short)(Months % 12 + 1);
			info.Year = (short)(Years);
			info.MoonPhase = (short)(Days % 16);
			
			short DayOfTheYear = (short)(Days % 876);
			if(DayOfTheYear < 189)
				info.Season = 0; //Winter
			else if(DayOfTheYear < 408)
				info.Season = 1; //Spring
			else if(DayOfTheYear < 627)
				info.Season = 1; //Summer
			else if(DayOfTheYear < 846)
				info.Season = 1; //Fall
			else
				info.Season = 0; //Winter
			
			return info;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register("UOTime", AccessLevel.Seer, new CommandEventHandler(UOTime_OnCommand));
		}

		public static void UOTime_OnCommand(CommandEventArgs e)
		{
			DateTimeInfo info = Now();
			
			e.Mobile.SendMessage("{0}/{1}/{2} {3}:{4} Season:{5} MoonPhase:{6}", info.Day, info.Month, info.Year, info.Hour, info.Minute, info.Season, info.MoonPhase);
		}
	}
}
