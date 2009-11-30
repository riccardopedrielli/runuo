using Server.Network;

namespace Server.Commands
{
	public class SetSeason
	{
		public static void Initialize()
		{
			CommandSystem.Register("Season", AccessLevel.Seer, new CommandEventHandler(Season_OnCommand));
		}

		public static void Season_OnCommand(CommandEventArgs e)
		{
			Map map;

			if(e.Length == 1)
			{
				for(int i = 0; i < 5; i++)
				{
					map = Map.AllMaps[i];
					map.Season = (e.GetInt32(0));

					foreach(NetState state in NetState.Instances)
					{
						Mobile m = state.Mobile;
						if(m != null)
						{
							state.Send(SeasonChange.Instantiate(m.GetSeason(), true));
							m.SendEverything();
						}
					}
				}
			}
		}
	}
	
	public class UOTime
	{
		public static void Initialize()
		{
			CommandSystem.Register("UOTime", AccessLevel.Seer, new CommandEventHandler(UOTime_OnCommand));
		}

		public static void UOTime_OnCommand(CommandEventArgs e)
		{
			long sec = (long)(new TimeSpan(DateTime.Now.Ticks - (new DateTime(1934, 12, 22, 8, 0, 0)).Ticks)).TotalSeconds;
			
			long minutes = sec / 5;
			long hours = minutes / 60;
			long days = hours / 24;
			long months = days / 73;
			long years = months / 12;
			
			long minute = minutes % 60;
			long hour = hours % 24;
			long day = days % 73 + 1;
			long month = months % 12 + 1;
			long year = years;
			
			e.Mobile.SendMessage("Britannian Time: {0}/{1}/{2} {3}:{4}", day, month, year, hour, minute);
		}
	}
}
