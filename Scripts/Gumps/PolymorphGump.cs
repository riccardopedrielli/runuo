using System;
using Server;
using Server.Network;
using Server.Targets;
using Server.Spells;
using Server.Spells.Seventh;

namespace Server.Gumps
{
	public class PolymorphEntry
    {   /*** MOD_START ***/
        //modificati i PolymorphEntry aggiungendo il nome della trasformazione
		public static readonly PolymorphEntry Chicken =		new PolymorphEntry( "a chicken", 8401, 0xD0, 1015236, 15, 10 );
		public static readonly PolymorphEntry Dog =			new PolymorphEntry( "a dog", 8405, 0xD9, 1015237, 17, 10 );
        public static readonly PolymorphEntry Pig =         new PolymorphEntry( "a pig", 8449, 0xCB, 1018266, 6, 6 ); 
		public static readonly PolymorphEntry Wolf =		new PolymorphEntry( "a wolf", 8426, 0xE1, 1015238, 18, 10 );
		public static readonly PolymorphEntry Panther =		new PolymorphEntry( "a panther", 8473, 0xD6, 1015239, 20, 14 );
		public static readonly PolymorphEntry Gorilla =		new PolymorphEntry( "a gorilla", 8437, 0x1D, 1015240, 23, 10 );
		public static readonly PolymorphEntry BlackBear =	new PolymorphEntry( "a black bear", 8399, 0xD3, 1015241, 22, 10 );
		public static readonly PolymorphEntry GrizzlyBear =	new PolymorphEntry( "a grizzly bear", 8411, 0xD4, 1015242, 22, 12 );
		public static readonly PolymorphEntry PolarBear =	new PolymorphEntry( "a polar bear", 8417, 0xD5, 1015243, 26, 10 );
        //public static readonly PolymorphEntry HumanMale =	new PolymorphEntry( null, 8397, 0x190, 1015244, 29, 8 );
        //public static readonly PolymorphEntry HumanFemale =	new PolymorphEntry( null, 8398, 0x191, 1015254, 29, 10 );
		public static readonly PolymorphEntry Slime =		new PolymorphEntry( "a slime", 8424, 0x33, 1015246, 5, 10 );
		public static readonly PolymorphEntry Orc =			new PolymorphEntry( "an orc", 8416, 0x11, 1015247, 29, 10 );
		public static readonly PolymorphEntry LizardMan =	new PolymorphEntry( "a lizardman", 8414, 0x21, 1015248, 26, 10 );
        public static readonly PolymorphEntry Skeleton =    new PolymorphEntry( "a skeletal knight", 8423, 0x39, 1029661, 26, 10 ); 
		public static readonly PolymorphEntry Gargoyle =	new PolymorphEntry( "a gargoyle", 8409, 0x04, 1015249, 22, 10 );
		public static readonly PolymorphEntry Ogre =		new PolymorphEntry( "an ogre", 8415, 0x01, 1015250, 24, 9 );
		public static readonly PolymorphEntry Troll =		new PolymorphEntry( "a troll", 8425, 0x36, 1015251, 25, 9 );
		public static readonly PolymorphEntry Ettin =		new PolymorphEntry( "an ettin", 8408, 0x02, 1015252, 25, 8 );
        public static readonly PolymorphEntry Daemon =      new PolymorphEntry( "a daemon", 8403, 0x09, 1015253, 25, 8 );


        //aggiunta la variabile m_Name che prima non esisteva
        private int m_Art, m_Body, m_Num, m_X, m_Y;
        private string m_Name;

		private PolymorphEntry( string Name, int Art, int Body, int LocNum, int X, int Y )
		{
            m_Name = Name;
			m_Art = Art;
			m_Body = Body;
			m_Num = LocNum;
			m_X = X;
			m_Y = Y;
		}

        public string Name { 
            get 
            {   
                //il calcolo dei nomi random va messo qui perche se no 
                //li genera soltanto una volta per tutti e rimane quello fino al riavvio del server
                switch (m_Name)
                {
                    case "a lizardman":
                        return NameList.RandomName("lizardman");
                    case "an orc":
                        return NameList.RandomName("orc");
                    case "a daemon":
                        return NameList.RandomName("daemon");
                    default:
                        return m_Name; 
                }               
            } 
        }
        /*** MOD_END ***/
		
        public int ArtID { get { return m_Art; } }
		public int BodyID { get { return m_Body; } }
		public int LocNumber{ get { return m_Num; } }
		public int X{ get{ return m_X; } }
		public int Y{ get{ return m_Y; } }
	}


	public class PolymorphGump : Gump
	{
		private class PolymorphCategory
		{
			private int m_Num;
			private PolymorphEntry[] m_Entries;

			public PolymorphCategory( int num, params PolymorphEntry[] entries )
			{
				m_Num = num;
				m_Entries = entries;
			}

			public PolymorphEntry[] Entries{ get { return m_Entries; } }
			public int LocNumber{ get { return m_Num; } }
		}

        /*** ADD_START ***/
        //le voci con "new" come commento sono le trasformazioni aggiunte a quelle di def
		private static PolymorphCategory[] Categories = new PolymorphCategory[]
			{
				new PolymorphCategory( 1015235, // Animals
					PolymorphEntry.Chicken,
					PolymorphEntry.Dog,
                    PolymorphEntry.Pig, //new
					PolymorphEntry.Wolf,
					PolymorphEntry.Panther,
					PolymorphEntry.Gorilla,
					PolymorphEntry.BlackBear,
					PolymorphEntry.GrizzlyBear,
					PolymorphEntry.PolarBear ),
					//PolymorphEntry.HumanMale ), // rimosso perche il mago non puo trasformarsi in umani

				new PolymorphCategory( 1015245, // Monsters
					PolymorphEntry.Slime,
					PolymorphEntry.Orc,
					PolymorphEntry.LizardMan,
                    PolymorphEntry.Skeleton, //new
					PolymorphEntry.Gargoyle,
					PolymorphEntry.Ogre,
					PolymorphEntry.Troll,
					PolymorphEntry.Ettin,
					PolymorphEntry.Daemon )
					//PolymorphEntry.HumanFemale ) // rimosso perche il mago non puo trasformarsi in umani
			};
        /*** ADD_END ***/

		private Mobile m_Caster;
		private Item m_Scroll;

		public PolymorphGump( Mobile caster, Item scroll ) : base( 50, 50 )
		{
			m_Caster = caster;
			m_Scroll = scroll;

			int x,y;
			AddPage( 0 );
			AddBackground( 0, 0, 585, 393, 5054 );
			AddBackground( 195, 36, 387, 275, 3000 );
			AddHtmlLocalized( 0, 0, 510, 18, 1015234, false, false ); // <center>Polymorph Selection Menu</center>
			AddHtmlLocalized( 60, 355, 150, 18, 1011036, false, false ); // OKAY
			AddButton( 25, 355, 4005, 4007, 1, GumpButtonType.Reply, 1 );
			AddHtmlLocalized( 320, 355, 150, 18, 1011012, false, false ); // CANCEL
			AddButton( 285, 355, 4005, 4007, 0, GumpButtonType.Reply, 2 );

			y = 35;
			for ( int i=0;i<Categories.Length;i++ )
			{
				PolymorphCategory cat = (PolymorphCategory)Categories[i];
				AddHtmlLocalized( 5, y, 150, 25, cat.LocNumber, true, false );
				AddButton( 155, y, 4005, 4007, 0, GumpButtonType.Page, i+1 );
				y += 25;
			}

			for ( int i=0;i<Categories.Length;i++ )
			{
				PolymorphCategory cat = (PolymorphCategory)Categories[i];
				AddPage( i+1 );

				for ( int c=0;c<cat.Entries.Length;c++ )
				{
					PolymorphEntry entry = (PolymorphEntry)cat.Entries[c];
					x = 198 + (c%3)*129;
					y = 38 + (c/3)*67;

					AddHtmlLocalized( x, y, 100, 18, entry.LocNumber, false, false );
					AddItem( x+20, y+25, entry.ArtID );
					AddRadio( x, y+20, 210, 211, false, (c<<8) + i );
				}
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 1 && info.Switches.Length > 0 )
			{
				int cnum = info.Switches[0];
				int cat = cnum%256;
				int ent = cnum>>8;

				if ( cat >= 0 && cat < Categories.Length )
				{
					if ( ent >= 0 && ent < Categories[cat].Entries.Length )
					{
                        Spell spell = new PolymorphSpell(m_Caster, m_Scroll, Categories[cat].Entries[ent].BodyID, Categories[cat].Entries[ent].Name);
						spell.Cast();
					}
				}
			}
		}
	}


	public class NewPolymorphGump : Gump
	{
        /*** ADD_START ***/
        //le voci con "new" come commento sono le trasformazioni aggiunte a quelle di def
		private static readonly PolymorphEntry[] m_Entries = new PolymorphEntry[]
			{
				PolymorphEntry.Chicken,
				PolymorphEntry.Dog,
                PolymorphEntry.Pig,  //new
				PolymorphEntry.Wolf,
				PolymorphEntry.Panther,
				PolymorphEntry.Gorilla,
				PolymorphEntry.BlackBear,
				PolymorphEntry.GrizzlyBear,
				PolymorphEntry.PolarBear,
				//PolymorphEntry.HumanMale, // rimosso perche il mago non puo trasformarsi in umani
				//PolymorphEntry.HumanFemale, // rimosso perche il mago non puo trasformarsi in umani
				PolymorphEntry.Slime,
				PolymorphEntry.Orc,
				PolymorphEntry.LizardMan,
                PolymorphEntry.Skeleton, //new
				PolymorphEntry.Gargoyle,
				PolymorphEntry.Ogre,
				PolymorphEntry.Troll,
				PolymorphEntry.Ettin,
				PolymorphEntry.Daemon
			};
        /*** ADD_END ***/

		private Mobile m_Caster;
		private Item m_Scroll;

		public NewPolymorphGump( Mobile caster, Item scroll ) : base( 0, 0 )
		{
			m_Caster = caster;
			m_Scroll = scroll;

			AddPage( 0 );

			AddBackground( 0, 0, 520, 404, 0x13BE );
			AddImageTiled( 10, 10, 500, 20, 0xA40 );
			AddImageTiled( 10, 40, 500, 324, 0xA40 );
			AddImageTiled( 10, 374, 500, 20, 0xA40 );
			AddAlphaRegion( 10, 10, 500, 384 );

			AddHtmlLocalized( 14, 12, 500, 20, 1015234, 0x7FFF, false, false ); // <center>Polymorph Selection Menu</center>

			AddButton( 10, 374, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 45, 376, 450, 20, 1060051, 0x7FFF, false, false ); // CANCEL

			for ( int i = 0; i < m_Entries.Length; i++ )
			{
				PolymorphEntry entry = m_Entries[i];

				int page = i / 10 + 1;
				int pos = i % 10;

				if ( pos == 0 )
				{
					if ( page > 1 )
					{
						AddButton( 400, 374, 0xFA5, 0xFA7, 0, GumpButtonType.Page, page );
						AddHtmlLocalized( 440, 376, 60, 20, 1043353, 0x7FFF, false, false ); // Next
					}

					AddPage( page );

					if ( page > 1 )
					{
						AddButton( 300, 374, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1 );
						AddHtmlLocalized( 340, 376, 60, 20, 1011393, 0x7FFF, false, false ); // Back
					}
				}

				int x = ( pos % 2 == 0 ) ? 14 : 264;
				int y = ( pos / 2 ) * 64 + 44;

				AddImageTiledButton( x, y, 0x918, 0x919, i + 1, GumpButtonType.Reply, 0, entry.ArtID, 0x0, entry.X, entry.Y );
				AddHtmlLocalized( x + 84, y, 250, 60, entry.LocNumber, 0x7FFF, false, false );
			}
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int idx = info.ButtonID - 1;

			if ( idx < 0 || idx >= m_Entries.Length )
				return;

            Spell spell = new PolymorphSpell(m_Caster, m_Scroll, m_Entries[idx].BodyID, m_Entries[idx].Name);
			spell.Cast();
		}
	}
}