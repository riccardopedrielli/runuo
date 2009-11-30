using System;
using Server;
// Time System
/*** ADD_START ***/
using Server.Network;
using Server.Mobiles;
using Server.Items;
/*** ADD_END ***/

namespace Server.Items
{
	public enum MoonPhase
	{
		NewMoon,
		WaxingCrescentMoon,
		FirstQuarter,
		WaxingGibbous,
		FullMoon,
		WaningGibbous,
		LastQuarter,
		WaningCrescent
	}

	[Flipable( 0x104B, 0x104C )]
	public class Clock : Item
	{
		private static DateTime m_ServerStart;

		public static DateTime ServerStart
		{
			get{ return m_ServerStart; }
		}

		public static void Initialize()
		{
			m_ServerStart = DateTime.Now;
		}

		[Constructable]
		public Clock() : this( 0x104B )
		{
		}

		[Constructable]
		public Clock( int itemID ) : base( itemID )
		{
			Weight = 3.0;
		}

		public Clock( Serial serial ) : base( serial )
		{
		}

		public const double SecondsPerUOMinute = 5.0;
		public const double MinutesPerUODay = SecondsPerUOMinute * 24;

		private static DateTime WorldStart = new DateTime( 1997, 9, 1 );

		public static MoonPhase GetMoonPhase( Map map, int x, int y )
		{
			int hours, minutes, totalMinutes;

			GetTime( map, x, y, out hours, out minutes, out totalMinutes );

			if ( map != null )
				totalMinutes /= 10 + (map.MapIndex * 20);

			return (MoonPhase)(totalMinutes % 8);
		}

		public static void GetTime( Map map, int x, int y, out int hours, out int minutes )
		{
			int totalMinutes;

			GetTime( map, x, y, out hours, out minutes, out totalMinutes );
		}

		public static void GetTime( Map map, int x, int y, out int hours, out int minutes, out int totalMinutes )
		{
			// Time System
			/*** ADD_START ***/
   			if (TimeSystem.System.Enabled)
			{
				totalMinutes = 0;
		
				TimeSystem.System.GetTime(x, out hours, out minutes);
			}
			else
			{
			/*** ADD_END ***/
			TimeSpan timeSpan = DateTime.Now - WorldStart;

			totalMinutes = (int)(timeSpan.TotalSeconds / SecondsPerUOMinute);

			if ( map != null )
				totalMinutes += map.MapIndex * 320;

			// Really on OSI this must be by subserver
			totalMinutes += x / 16;

			hours = (totalMinutes / 60) % 24;
			minutes = totalMinutes % 60;
			/*** ADD_START ***/
    		}
			/*** ADD_END ***/
		}

		public static void GetTime( out int generalNumber, out string exactTime )
		{
			GetTime( null, 0, 0, out generalNumber, out exactTime );
		}

		public static void GetTime( Mobile from, out int generalNumber, out string exactTime )
		{
			GetTime( from.Map, from.X, from.Y, out generalNumber, out exactTime );
		}

		public static void GetTime( Map map, int x, int y, out int generalNumber, out string exactTime )
		{
			int hours, minutes;

			GetTime( map, x, y, out hours, out minutes );

			// 00:00 AM - 00:59 AM : Witching hour
			// 01:00 AM - 03:59 AM : Middle of night
			// 04:00 AM - 07:59 AM : Early morning
			// 08:00 AM - 11:59 AM : Late morning
			// 12:00 PM - 12:59 PM : Noon
			// 01:00 PM - 03:59 PM : Afternoon
			// 04:00 PM - 07:59 PM : Early evening
			// 08:00 PM - 11:59 AM : Late at night

			if ( hours >= 20 )
				generalNumber = 1042957; // It's late at night
			else if ( hours >= 16 )
				generalNumber = 1042956; // It's early in the evening
			else if ( hours >= 13 )
				generalNumber = 1042955; // It's the afternoon
			else if ( hours >= 12 )
				generalNumber = 1042954; // It's around noon
			else if ( hours >= 08 )
				generalNumber = 1042953; // It's late in the morning
			else if ( hours >= 04 )
				generalNumber = 1042952; // It's early in the morning
			else if ( hours >= 01 )
				generalNumber = 1042951; // It's the middle of the night
			else
				generalNumber = 1042950; // 'Tis the witching hour. 12 Midnight.

			hours %= 12;

			if ( hours == 0 )
				hours = 12;

			exactTime = String.Format( "{0}:{1:D2}", hours, minutes );
		}

		public override void OnDoubleClick( Mobile from )
		{
			//Time System
			/*** ADD_START ***/
			if (TimeSystem.System.Enabled)
			{
    			NetState state = from.NetState;

    			MessageType type = Network.MessageType.Regular;
    			int hue = 0x3B2;
    			string text = TimeSystem.System.GetTime(from.X, true);

    			state.Send(new UnicodeMessage( Serial, ItemID, type, hue, 3, "ENU", Name, text ));
			}
			else
			{
			/*** ADD_END ***/
			int genericNumber;
			string exactTime;

			GetTime( from, out genericNumber, out exactTime );

			SendLocalizedMessageTo( from, genericNumber );
			SendLocalizedMessageTo( from, 1042958, exactTime ); // ~1_TIME~ to be exact
			/*** ADD_START ***/
			}
			/*** ADD_END ***/
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 2.0 )
				Weight = 3.0;
		}
	}

	[Flipable( 0x104B, 0x104C )]
	public class ClockRight : Clock
	{
		[Constructable]
		public ClockRight() : base( 0x104B )
		{
		}

		public ClockRight( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x104B, 0x104C )]
	public class ClockLeft : Clock
	{
		[Constructable]
		public ClockLeft() : base( 0x104C )
		{
		}

		public ClockLeft( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	//Time System
	/*** ADD_START ***/
    public class WristWatch : Clock
    {
        [Constructable]
        public WristWatch() : base( 0x1F06 )
        {
  	        Name = "a wrist watch";
  	        Layer = Layer.Bracelet;
  	
  		    switch ( Utility.Random( 2 ))
		    {
			    case 0: this.ItemID = 0x1F06; break;
			    case 1: this.ItemID = 0x1086; break;
		    }
        }
  
        public WristWatch( Serial serial ) : base( serial )
        {
        }
  
        public override void Serialize( GenericWriter writer )
	    {
		    base.Serialize( writer );

		    writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	/*** ADD_END ***/
}