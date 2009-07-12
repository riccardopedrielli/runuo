/*****************************************************/
/*   Skill and stat setter gump for new characters   */
/*****************************************************/

using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Misc;

namespace Server
{
	public class SkillPickGump : Gump
	{
		//Number of skills to choose
		private int switches = 3;

		//Value at which selected skills are set
		private double val = 50;

		public SkillPickGump()
			: base( 20, 40 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;

			this.AddBackground(   0,   0, 670, 552, 5054 );
			this.AddImageTiled(  10,  10, 650,  22, 2624 );
			this.AddImageTiled(  10,  37, 650,  62, 2624 );
			this.AddImageTiled(  10,  99, 650, 366, 2624 );
			this.AddImageTiled(  10, 475, 650,  40, 2624 );
			this.AddImageTiled(  10, 520, 140,  22, 2624 );
			this.AddImageTiled( 155, 520, 505,  22, 2624 );
			this.AddAlphaRegion( 10,  10, 650, 532 );

			this.AddHtml( 10, 10, 650, 22, printHtml("<CENTER>CHARACTER SETUP</CENTER>"), false, false );

			this.AddHtml( 10, 37, 650, 62, printHtml( @"Here you can set your starting skills and stats.<BR>
														Select 3 skills, they will be set to 50.0%<BR>
														Set your stats with a total of 100 points and a limit of 60 points in a single stat.
														"), false, false );

			this.AddCheck(20,  47, 210, 211, false, 0);
			this.AddCheck(20,  72, 210, 211, false, 1);
			this.AddCheck(20,  97, 210, 211, false, 2);
			this.AddCheck(20, 122, 210, 211, false, 3);
			this.AddCheck(20, 147, 210, 211, false, 4);
			this.AddCheck(20, 172, 210, 211, false, 5);
			this.AddCheck(20, 197, 210, 211, false, 6);
			this.AddCheck(20, 222, 210, 211, false, 7);
			this.AddCheck(20, 247, 210, 211, false, 8);
			this.AddCheck(20, 272, 210, 211, false, 9);
			this.AddCheck(20, 297, 210, 211, false, 10);
			this.AddCheck(20, 322, 210, 211, false, 11);
			this.AddCheck(20, 347, 210, 211, false, 12);
			this.AddCheck(20, 372, 210, 211, false, 13);

			this.AddCheck(180,  47, 210, 211, false, 14);
			
			this.AddCheck(340,  47, 210, 211, false, 28);
			
			this.AddCheck(500,  47, 210, 211, false, 42);
			
			this.AddButton( 10, 480, 4005, 4007, (int)Buttons.FinishButton, GumpButtonType.Reply, 0 );
			this.AddHtml( 50, 482, 110, 20, printHtml("CONFIRM"), false, false );

			/*
		Alchemy = 0,
		Anatomy = 1,
		AnimalLore = 2,
		ItemID = 3,
		ArmsLore = 4,
		Parry = 5,
		Begging = 6,
		Blacksmith = 7,
		Fletching = 8,
		Peacemaking = 9,
		Camping = 10,
		Carpentry = 11,
		Cartography = 12,
		Cooking = 13,
		DetectHidden = 14,
		Discordance = 15,
		EvalInt = 16,
		Healing = 17,
		Fishing = 18,
		Forensics = 19,
		Herding = 20,
		Hiding = 21,
		Provocation = 22,
		Inscribe = 23,
		Lockpicking = 24,
		Magery = 25,
		MagicResist = 26,
		Tactics = 27,
		Snooping = 28,
		Musicianship = 29,
		Poisoning = 30,
		Archery = 31,
		SpiritSpeak = 32,
		Stealing = 33,
		Tailoring = 34,
		AnimalTaming = 35,
		TasteID = 36,
		Tinkering = 37,
		Tracking = 38,
		Veterinary = 39,
		Swords = 40,
		Macing = 41,
		Fencing = 42,
		Wrestling = 43,
		Lumberjacking = 44,
		Mining = 45,
		Meditation = 46,
		Stealth = 47,
		RemoveTrap = 48,
		Focus = 50,
			*/
			
			/*
			this.AddLabel(80, 65, 0, @"Tactics");
			this.AddLabel(80, 90, 0, @"Anatomy");
			this.AddLabel(80, 115, 0, @"Swordsmanship");
			this.AddLabel(80, 140, 0, @"Fencing");
			this.AddLabel(80, 165, 0, @"Archery");
			this.AddLabel(80, 190, 0, @"Macefighting");
			this.AddLabel(80, 215, 0, @"Parry");
			this.AddLabel(80, 240, 0, @"Arms Lore");
			this.AddLabel(80, 265, 0, @"Wrestling");
			this.AddLabel(80, 290, 0, @"Bushido");


			this.AddLabel(80, 65, 0, @"Blacksmithing");
			this.AddLabel(80, 90, 0, @"Carpentry");
			this.AddLabel(80, 115, 0, @"Tinkering");
			this.AddLabel(80, 140, 0, @"Tailoring");
			this.AddLabel(80, 165, 0, @"Fishing");
			this.AddLabel(80, 190, 0, @"Cooking");
			this.AddLabel(80, 215, 0, @"Fletching");
			this.AddLabel(80, 240, 0, @"Cartography");
			this.AddLabel(80, 265, 0, @"Mining");
			this.AddLabel(80, 290, 0, @"Lumberjacking");


			this.AddLabel(80, 65, 0, @"Alchemy");
			this.AddLabel(80, 90, 0, @"Inscription");
			this.AddLabel(80, 115, 0, @"Spellweaving");
			this.AddLabel(80, 140, 0, @"Magery");
			this.AddLabel(80, 165, 0, @"Necromancy");
			this.AddLabel(80, 190, 0, @"Spirit Speak");
			this.AddLabel(80, 215, 0, @"Evaluating Intelligence");
			this.AddLabel(80, 240, 0, @"Chivalry");
			this.AddLabel(80, 265, 0, @"Focus");
			this.AddLabel(80, 290, 0, @"Meditation");


			this.AddLabel(80, 65, 0, @"Hiding");
			this.AddLabel(80, 90, 0, @"Stealth");
			this.AddLabel(80, 115, 0, @"Snooping");
			this.AddLabel(80, 140, 0, @"Stealing");
			this.AddLabel(80, 165, 0, @"Lockpicking");
			this.AddLabel(80, 190, 0, @"Ninjitsu");
			this.AddLabel(80, 215, 0, @"Detecting Hidden");
			this.AddLabel(80, 240, 0, @"Remove Trap");
			this.AddLabel(80, 265, 0, @"Tracking");
			this.AddLabel(80, 290, 0, @"Poisoning");


			this.AddLabel(80, 65, 0, @"Animal Taming");
			this.AddLabel(80, 90, 0, @"Animal Lore");
			this.AddLabel(80, 115, 0, @"Camping");
			this.AddLabel(80, 140, 0, @"Musicianship");
			this.AddLabel(80, 165, 0, @"Discordance");
			this.AddLabel(80, 190, 0, @"Provocation");
			this.AddLabel(80, 215, 0, @"Peacemaking");
			this.AddLabel(80, 240, 0, @"Item Identification");
			this.AddLabel(80, 265, 0, @"Taste Identification");
			this.AddLabel(80, 290, 0, @"Foresic Evaluation");

			this.AddLabel(80, 65, 0, @"Begging");
			this.AddLabel(80, 90, 0, @"Healing");
			this.AddLabel(80, 115, 0, @"Herding");
			this.AddLabel(80, 140, 0, @"Resisting Spells");
			this.AddLabel(80, 165, 0, @"Veterinary");
			*/

		}
		
		public enum Buttons
		{
			FinishButton
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;

			switch( info.ButtonID )
			{
				case 0:
				{
					if ( info.Switches.Length < switches )
					{
						m.SendGump( new SkillPickGump() );
						m.SendMessage( 0, "You must pick {0} skills.", switches );
						break;
					}
					else if ( info.Switches.Length > switches )
					{
						m.SendGump( new SkillPickGump() );
						m.SendMessage( 0, "You can only pick {0} skills", switches);
						break;
					}
					else
					{
						Server.Skills skills = m.Skills;

						for ( int i = 0; i < skills.Length; ++i )
						{
							if ( info.IsSwitched( i ))
								m.Skills[i].Base = val;
							else
								m.Skills[i].Base = 0;
						}
					}
					break;
				}
			}
		}
		
		private string printHtml(string str)
		{
			return "<BASEFONT COLOR=White>"+str+"</BASEFONT>";
		}
	}

	public class SkillBall : Item
	{ 
		[Constructable] 
		public SkillBall() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 1194; 
			Name = "skill ball"; 
			Movable =  false;
		}

		public override void OnDoubleClick( Mobile m ) 
		{				
			m.CloseGump( typeof( SkillPickGump ) );
			m.SendGump ( new SkillPickGump() );
			this.Delete();
		} 

		public SkillBall( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}
}