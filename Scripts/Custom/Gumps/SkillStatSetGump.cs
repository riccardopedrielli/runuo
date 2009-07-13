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
	public class SkillStatSetGump : Gump
	{
		//Number of skills to choose
		private int switches = 3;

		//Value at which selected skills are set
		private double val = 50;

		public SkillStatSetGump()
			: base( 60, 50 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;

			this.AddBackground(   0,   0, 670, 552, 5054 );
			/*this.AddImageTiled(  10,  10, 650,  22, 2624 );
			this.AddImageTiled(  10,  37, 650,  62, 2624 );
			this.AddImageTiled(  10, 104, 650, 366, 2624 );
			this.AddImageTiled(  10, 475, 650,  40, 2624 );
			this.AddImageTiled(  10, 520, 140,  22, 2624 );
			this.AddImageTiled( 155, 520, 505,  22, 2624 );*/
			//this.AddAlphaRegion( 10,  10, 650, 532 );

			this.AddHtml( 10, 12, 650, 20, printHtmlColor("<CENTER>CHARACTER SETUP</CENTER>", "#FFCC22"), false, false );

			this.AddHtml( 20, 39, 640, 62, printHtmlColor( @"Here you can set your starting skills and stats
														Select 3 skills, they will be set to 50.0%
														Set your stats with a total of 100 points and a limit of 60 points in a single stat
														", "#060630"), false, false );

			this.AddCheck(20, 114, 30008, 30009, false, 0);
			this.AddCheck(20, 139, 30008, 30009, false, 1);
			this.AddCheck(20, 164, 30008, 30009, false, 2);
			this.AddCheck(20, 189, 30008, 30009, false, 3);
			this.AddCheck(20, 214, 30008, 30009, false, 4);
			this.AddCheck(20, 239, 30008, 30009, false, 5);
			this.AddCheck(20, 264, 30008, 30009, false, 6);
			this.AddCheck(20, 289, 30008, 30009, false, 7);
			this.AddCheck(20, 314, 30008, 30009, false, 8);
			this.AddCheck(20, 339, 30008, 30009, false, 9);
			this.AddCheck(20, 364, 30008, 30009, false, 10);
			this.AddCheck(20, 389, 30008, 30009, false, 11);
			this.AddCheck(20, 414, 30008, 30009, false, 12);
			this.AddCheck(20, 439, 30008, 30009, false, 13);

			this.AddCheck(180, 114, 30008, 30009, false, 14);
			this.AddCheck(180, 139, 30008, 30009, false, 15);
			this.AddCheck(180, 164, 30008, 30009, false, 16);
			this.AddCheck(180, 189, 30008, 30009, false, 17);
			this.AddCheck(180, 214, 30008, 30009, false, 18);
			this.AddCheck(180, 239, 30008, 30009, false, 19);
			this.AddCheck(180, 264, 30008, 30009, false, 20);
			this.AddCheck(180, 289, 30008, 30009, false, 21);
			this.AddCheck(180, 314, 30008, 30009, false, 22);
			this.AddCheck(180, 339, 30008, 30009, false, 23);
			this.AddCheck(180, 364, 30008, 30009, false, 24);
			this.AddCheck(180, 389, 30008, 30009, false, 25);
			this.AddCheck(180, 414, 30008, 30009, false, 26);
			this.AddCheck(180, 439, 30008, 30009, false, 27);
			
			this.AddCheck(340, 114, 30008, 30009, false, 28);
			this.AddCheck(340, 139, 30008, 30009, false, 29);
			this.AddCheck(340, 164, 30008, 30009, false, 30);
			this.AddCheck(340, 189, 30008, 30009, false, 31);
			this.AddCheck(340, 214, 30008, 30009, false, 32);
			this.AddCheck(340, 239, 30008, 30009, false, 33);
			this.AddCheck(340, 264, 30008, 30009, false, 34);
			this.AddCheck(340, 289, 30008, 30009, false, 35);
			this.AddCheck(340, 314, 30008, 30009, false, 36);
			this.AddCheck(340, 339, 30008, 30009, false, 37);
			this.AddCheck(340, 364, 30008, 30009, false, 38);
			this.AddCheck(340, 389, 30008, 30009, false, 39);
			this.AddCheck(340, 414, 30008, 30009, false, 40);
			this.AddCheck(340, 439, 30008, 30009, false, 41);
			
			this.AddCheck(500, 264, 30008, 30009, false, 42);
			this.AddCheck(500, 289, 30008, 30009, false, 43);
			this.AddCheck(500, 314, 30008, 30009, false, 44);
			this.AddCheck(500, 339, 30008, 30009, false, 45);
			this.AddCheck(500, 364, 30008, 30009, false, 46);
			this.AddCheck(500, 389, 30008, 30009, false, 47);
			this.AddCheck(500, 414, 30008, 30009, false, 48);
			this.AddCheck(500, 439, 30008, 30009, false, 50);
			
			this.AddImage(510, 133, 9000);
			
			this.AddHtml( 50, 115, 120, 20, printHtml("Alchemy"), false, false );
			this.AddHtml( 50, 140, 120, 20, printHtml("Anatomy"), false, false );
			this.AddHtml( 50, 165, 120, 20, printHtml("Animal Lore"), false, false );
			this.AddHtml( 50, 190, 120, 20, printHtml("Item Identification"), false, false );
			this.AddHtml( 50, 215, 120, 20, printHtml("Arms Lore"), false, false );
			this.AddHtml( 50, 240, 120, 20, printHtml("Parrying"), false, false );
			this.AddHtml( 50, 265, 120, 20, printHtml("Begging"), false, false );
			this.AddHtml( 50, 290, 120, 20, printHtml("Blacksmithy"), false, false );
			this.AddHtml( 50, 315, 120, 20, printHtml("Bowcraft/Fletching"), false, false );
			this.AddHtml( 50, 340, 120, 20, printHtml("Peacemaking"), false, false );
			this.AddHtml( 50, 365, 120, 20, printHtml("Camping"), false, false );
			this.AddHtml( 50, 390, 120, 20, printHtml("Carpentry"), false, false );
			this.AddHtml( 50, 415, 120, 20, printHtml("Cartography"), false, false );
			this.AddHtml( 50, 440, 120, 20, printHtml("Cooking"), false, false );
			
			this.AddHtml( 210, 115, 120, 20, printHtml("Detecting Hidden"), false, false );
			this.AddHtml( 210, 140, 120, 20, printHtml("Discordance"), false, false );
			this.AddHtml( 210, 165, 120, 20, printHtml("Evaluating Intelligence"), false, false );
			this.AddHtml( 210, 190, 120, 20, printHtml("Healing"), false, false );
			this.AddHtml( 210, 215, 120, 20, printHtml("Fishing"), false, false );
			this.AddHtml( 210, 240, 120, 20, printHtml("Forensic Evaluation"), false, false );
			this.AddHtml( 210, 265, 120, 20, printHtml("Herding"), false, false );
			this.AddHtml( 210, 290, 120, 20, printHtml("Hiding"), false, false );
			this.AddHtml( 210, 315, 120, 20, printHtml("Provocation"), false, false );
			this.AddHtml( 210, 340, 120, 20, printHtml("Inscription"), false, false );
			this.AddHtml( 210, 365, 120, 20, printHtml("Lockpicking"), false, false );
			this.AddHtml( 210, 390, 120, 20, printHtml("Magery"), false, false );
			this.AddHtml( 210, 415, 120, 20, printHtml("Resisting Spells"), false, false );
			this.AddHtml( 210, 440, 120, 20, printHtml("Tactics"), false, false );
			
			this.AddHtml( 370, 115, 120, 20, printHtml("Snooping"), false, false );
			this.AddHtml( 370, 140, 120, 20, printHtml("Musicianship"), false, false );
			this.AddHtml( 370, 165, 120, 20, printHtml("Poisoning"), false, false );
			this.AddHtml( 370, 190, 120, 20, printHtml("Archery"), false, false );
			this.AddHtml( 370, 215, 120, 20, printHtml("Spirit Speak"), false, false );
			this.AddHtml( 370, 240, 120, 20, printHtml("Stealing"), false, false );
			this.AddHtml( 370, 265, 120, 20, printHtml("Tailoring"), false, false );
			this.AddHtml( 370, 290, 120, 20, printHtml("Animal Taming"), false, false );
			this.AddHtml( 370, 315, 120, 20, printHtml("Taste Identification"), false, false );
			this.AddHtml( 370, 340, 120, 20, printHtml("Tinkering"), false, false );
			this.AddHtml( 370, 365, 120, 20, printHtml("Tracking"), false, false );
			this.AddHtml( 370, 390, 120, 20, printHtml("Veterinary"), false, false );
			this.AddHtml( 370, 415, 120, 20, printHtml("Swordsmanship"), false, false );
			this.AddHtml( 370, 440, 120, 20, printHtml("Mace Fighting"), false, false );
			
			this.AddHtml( 530, 265, 120, 20, printHtml("Fencing"), false, false );
			this.AddHtml( 530, 290, 120, 20, printHtml("Wrestling"), false, false );
			this.AddHtml( 530, 315, 120, 20, printHtml("Lumberjacking"), false, false );
			this.AddHtml( 530, 340, 120, 20, printHtml("Mining"), false, false );
			this.AddHtml( 530, 365, 120, 20, printHtml("Meditation"), false, false );
			this.AddHtml( 530, 390, 120, 20, printHtml("Stealth"), false, false );
			this.AddHtml( 530, 415, 120, 20, printHtml("Remove Trap"), false, false );
			this.AddHtml( 530, 440, 120, 20, printHtml("Focus"), false, false );
			
			this.AddButton( 10, 520, 4005, 4007, (int)Buttons.FinishButton, GumpButtonType.Reply, 0 );
			this.AddHtml( 50, 522, 110, 20, printHtmlColor("CONFIRM", "#060630"), false, false );
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
						m.SendGump( new SkillStatSetGump() );
						m.SendMessage( 0, "You must pick {0} skills.", switches );
						break;
					}
					else if ( info.Switches.Length > switches )
					{
						m.SendGump( new SkillStatSetGump() );
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
			return "<BASEFONT COLOR=#FFFFBB>"+str+"</BASEFONT>";
		}
		
		private string printHtmlColor(string str, string color)
		{
			return "<BASEFONT COLOR="+color+">"+str+"</BASEFONT>";
		}
	}
}