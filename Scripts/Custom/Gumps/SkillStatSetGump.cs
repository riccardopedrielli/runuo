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
		private enum Stats
		{
			Strength,
			Dexterity,
			Intelligence
		}
		
		private Point3D m_Target;
		private Map m_TargetMap;
		private string m_error = null;
		private string[] m_stats = null;
		private bool[] m_skills = null;
		
		//Number of skills to choose
		private const int switches = 3;

		//Value at which selected skills are set
		private const int val = 50;
		
		//Total stat points
		private const int statCap = 100;
		
		//Max points in a stat
		private const int statMax = 60;
		
		//Min points in a stat
		private const int statMin = 10;

		//Gump graphic
		private const string titleColor = "#FFFFFF";
		private const string gdrColor = "#4AC684";
		private const string infoColor = "#BDE784";
		private const string skillColor = "#D6EFEF";
		private const string errorColor = "#FF5533";
		
		private const int uncheckedID = 11410;
		private const int checkedID = 11402;
		
		private const int gumpX = 20;
		private const int gumpY = 41;
		private const int areaWidth = 682;
		private const int areaHeight = 550;
		
		private const int skillOffsetX = 103;
		private const int skillOffsetY = 120;
		private const int skillDistanceX = 155;
		private const int skillDistanceY = 23;
		
		public SkillStatSetGump( Point3D Target, Map TargetMap ) : base( gumpX, gumpY )
		{
			m_Target = Target;
			m_TargetMap = TargetMap;
			
			CreateGump();
		}
		
		public SkillStatSetGump( Point3D Target, Map TargetMap, string error, string[] stats, bool[] skills ) : base( gumpX, gumpY )
		{
			m_Target = Target;
			m_TargetMap = TargetMap;
			
			m_error = error;
			m_stats = stats;
			m_skills = skills;
			
			CreateGump();
		}
		
		private void CreateGump()
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			
			AddImageTiled( 50, 20, areaWidth, areaHeight, 5124 ); //BACKGROUND

			AddImage( 90, 33, 9005 ); //WAX MARK
			AddImageTiled( 130, 65, 200, 1, 9101 ); //UNDERLINE

			AddImageTiled( 50, 29, 30, areaHeight + 10, 10460 ); //SQUARE COLUMN LEFT
			AddImageTiled( 34, 140, 17, areaHeight - 121, 9263 ); //COLUMN BORDER LEFT

			AddImage( 48, 135, 10411 ); //DRAGON TAIL LEFT
			AddImage( -16, 285, 10402 ); //DRAGON END TAIL LEFT
			AddImage( 0, 10, 10421 ); //DRAGON BODY LEFT
			AddImage( 25, 0, 10420 ); //DRAGON HEAD LEFT

			AddImageTiled( 83, 15, areaWidth - 50, 15, 10250 ); //MAIOLICA TOP

			AddImage( 34, areaHeight + 19, 10306 ); //MAIOLICA BOTTOM LEFT CORNER
			AddImage( areaWidth + 42, areaHeight + 19, 10304 ); //MAIOLICA BOTTOM RIGHT CORNER
			AddImageTiled( 51, areaHeight + 19, areaWidth - 8, 17, 10101 ); //MAIOLICA BOTTOM

			AddImageTiled( areaWidth + 15, 29, 44, areaHeight - 10, 2605 );//COLUMN BORDER RIGHT
			AddImageTiled( areaWidth + 15, 29, 30, areaHeight - 10, 10460 ); //SQUARE COLUMN RIGHT
			AddImage( areaWidth + 25, 0, 10441 ); //DRAGON RIGHT

			AddImage( areaWidth - 30, 50, 1417 ); //ROUND STONE
			AddImage( areaWidth - 21, 60, 5504 ); //UO LOGO
			
			this.AddHtml( 132, 45, 270, 20, PrintHtml( "CHARACTER SETUP", titleColor ), false, false );
			
			this.AddHtml( 132, 70, 500, 20, PrintHtml( "<I>Hail adventurer, it is time to tell us about your past.</I>", gdrColor ), false, false );
			
			this.AddHtml( skillOffsetX, 115, 600, 50, PrintHtml( String.Format( "Select {0} skills, they will be set to {1}.0%", switches, val), infoColor), false, false );
			
			int skillX = skillOffsetX;
			int skillY = skillOffsetY;
			
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  0, "Alchemy");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  1, "Anatomy");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  2, "Animal Lore");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  3, "Item Identification");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  4, "Arms Lore");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  5, "Parrying");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  6, "Begging");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  7, "Blacksmithy");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  8, "Bowcraft/Fletching");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY,  9, "Peacemaking");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 10, "Camping");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 11, "Carpentry");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 12, "Cartography");
			
			skillX += skillDistanceX;
			skillY = skillOffsetY;
			
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 13, "Cooking");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 14, "Detecting Hidden");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 15, "Discordance");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 16, "Evaluating Intelligence");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 17, "Healing");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 18, "Fishing");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 19, "Forensic Evaluation");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 20, "Herding");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 21, "Hiding");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 22, "Provocation");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 23, "Inscription");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 24, "Lockpicking");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 25, "Magery");
			
			skillX += skillDistanceX;
			skillY = skillOffsetY;

			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 26, "Resisting Spells");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 27, "Tactics");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 28, "Snooping");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 29, "Musicianship");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 30, "Poisoning");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 31, "Archery");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 32, "Spirit Speak");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 33, "Stealing");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 34, "Tailoring");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 35, "Animal Taming");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 36, "Taste Identification");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 37, "Tinkering");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 38, "Tracking");
			
			skillX += skillDistanceX;
			skillY = skillOffsetY + skillDistanceY * 2;
			
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 39, "Veterinary");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 40, "Swordsmanship");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 41, "Mace Fighting");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 42, "Fencing");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 43, "Wrestling");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 44, "Lumberjacking");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 45, "Mining");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 46, "Meditation");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 47, "Stealth");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 48, "Remove Trap");
			this.AddSkillCheck(skillX, skillY+=skillDistanceY, 50, "Focus");
			
			this.AddHtml( skillOffsetX, 460, 600, 50, PrintHtml( String.Format( "Set your stats with a total of {0} points and a {1} to {2} points in a single stat", statCap, statMin, statMax ), infoColor), false, false );
			
			this.AddHtml( skillOffsetX, 490, 100, 20, PrintHtml( "Strength:", skillColor ), false, false );
			this.AddBackground( skillOffsetX + 65, 488, 40, 24, 3000 );
			this.AddTextEntry( skillOffsetX + 74, 490, 30, 20, 0, (int)Stats.Strength, ( m_stats == null ? "" : m_stats[0] ), 2 );
			
			this.AddHtml( skillOffsetX + 192, 490, 100, 20, PrintHtml( "Dexterity:", skillColor ), false, false );
			this.AddBackground( skillOffsetX + 260, 488, 40, 24, 3000 );
			this.AddTextEntry( skillOffsetX + 269, 490, 30, 20, 0, (int)Stats.Dexterity, ( m_stats == null ? "" : m_stats[1] ), 2 );
			
			this.AddHtml( skillOffsetX + 379, 490, 100, 20, PrintHtml( "Intelligence:", skillColor ), false, false );
			this.AddBackground( skillOffsetX + 455, 488, 40, 24, 3000 );
			this.AddTextEntry( skillOffsetX + 464, 490, 30, 20, 0, (int)Stats.Intelligence, ( m_stats == null ? "" : m_stats[2] ), 2 );
			
			this.AddHtml( skillOffsetX, areaHeight - 20, 500, 20, PrintHtml( m_error, errorColor ), false, false );
			
			this.AddButton( areaWidth - 65, areaHeight - 20, 247, 248,  0, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;

			try
			{
				if ( info.Switches.Length < switches )
				{
					ResendGump( String.Format( "You must pick {0} skills", switches ), m, info );
				}
				else if ( info.Switches.Length > switches )
				{
					ResendGump( String.Format( "You can only pick {0} skills", switches ), m, info  );
				}
				else if ( info.GetTextEntry( (int)Stats.Strength ).Text == ""
						|| info.GetTextEntry( (int)Stats.Dexterity ).Text == ""
						|| info.GetTextEntry( (int)Stats.Intelligence ).Text == "" )
				{
					ResendGump( "You must set a value for each stat", m, info  );
				}
				else if ( Convert.ToInt32( info.GetTextEntry( (int)Stats.Strength ).Text ) < statMin
						|| Convert.ToInt32( info.GetTextEntry( (int)Stats.Dexterity ).Text ) < statMin
			         	|| Convert.ToInt32( info.GetTextEntry( (int)Stats.Intelligence ).Text ) < statMin )
				{
					ResendGump( String.Format( "You must set al least {0} points in a single stat", statMin ), m, info  );
				}
				else if ( Convert.ToInt32( info.GetTextEntry( (int)Stats.Strength ).Text ) > statMax
						|| Convert.ToInt32( info.GetTextEntry( (int)Stats.Dexterity ).Text ) > statMax
			         	|| Convert.ToInt32( info.GetTextEntry( (int)Stats.Intelligence ).Text ) > statMax )
				{
					ResendGump( String.Format( "You cannot set more than {0} points in a single stat", statMax ), m, info  );
				}
				else if ( Convert.ToInt32( info.GetTextEntry( (int)Stats.Strength ).Text )
						+ Convert.ToInt32( info.GetTextEntry( (int)Stats.Dexterity ).Text )
						+ Convert.ToInt32( info.GetTextEntry( (int)Stats.Intelligence ).Text ) > statCap )
				{
					ResendGump( String.Format( "You cannot exceed {0} total stat points", statCap ), m, info  );
				}
				else if ( Convert.ToInt32( info.GetTextEntry( (int)Stats.Strength ).Text )
						+ Convert.ToInt32( info.GetTextEntry( (int)Stats.Dexterity ).Text )
						+ Convert.ToInt32( info.GetTextEntry( (int)Stats.Intelligence ).Text ) < statCap )
				{
					ResendGump( String.Format( "You must set {0} total stat points", statCap ), m, info  );
				}
				else
				{
					m.Str = Convert.ToInt32( info.GetTextEntry( (int)Stats.Strength ).Text );
					m.Dex = Convert.ToInt32( info.GetTextEntry( (int)Stats.Dexterity ).Text );
					m.Int = Convert.ToInt32( info.GetTextEntry( (int)Stats.Intelligence ).Text );
				
					m.Hits = 100;
					m.Mana = 100;
					m.Stam = 100;
				
					Server.Skills skills = m.Skills;

					for ( int i = 0; i < skills.Length; ++i )
					{
						if ( info.IsSwitched( i ))
							m.Skills[i].Base = val;
						else
							m.Skills[i].Base = 0;
					}
					
					m.MoveToWorld( m_Target, m_TargetMap );
					
					Effects.PlaySound( m.Location, m.Map, 0x1FE );
				}
			}
			catch( FormatException )
			{
				ResendGump( "You can only enter numbers in stat fields", m, info  );
			}
		}
		
		private void ResendGump(string error, Mobile m, RelayInfo info)
		{
			string[] stats = new string[3];
			stats[0] = info.GetTextEntry( (int)Stats.Strength ).Text;
			stats[1] = info.GetTextEntry( (int)Stats.Dexterity ).Text;
			stats[2] = info.GetTextEntry( (int)Stats.Intelligence ).Text;
			
			bool[] skills = new bool[51];
			for ( int i = 0; i < skills.Length; ++i )
			{
				skills[i] = info.IsSwitched( i );
			}
			
			m.SendGump( new SkillStatSetGump( m_Target, m_TargetMap, error, stats, skills ) );
		}
		
		private string PrintHtml( string str, string color )
		{
			return String.Format( "<BASEFONT COLOR={0}>{1}</BASEFONT>", color, str );
		}
		
		private void AddSkillCheck( int skillX, int skillY, int skillID, string skillName )
		{
			this.AddCheck( skillX, skillY, uncheckedID, checkedID, ( m_skills == null ? false : m_skills[skillID] ), skillID );
			this.AddHtml( skillX + 28, skillY - 1, 120, 20, PrintHtml( skillName, skillColor ), false, false );
		}
	}
}