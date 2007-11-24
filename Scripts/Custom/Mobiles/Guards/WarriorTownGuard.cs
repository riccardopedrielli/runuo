using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;

namespace Server.Mobiles
{
	public abstract class WarriorTownGuard : BaseTownGuard
	{
		protected int MeleeSkill = Utility.Random( 8 );

		public WarriorTownGuard() : base( AIType.AI_WarriorGuard )
		{
		}

		public WarriorTownGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			// Pick a helm
			switch( Utility.Random( 3 ) )
			{
				case 0: AddItem( new CloseHelm() ); break;
				case 1: AddItem( new NorseHelm() ); break;
				case 2: AddItem( new PlateHelm() ); break;
			}
			// Pick an armour
			if ( Female )
			{
				AddItem( new FemalePlateChest() );
			}
			else
			{
				AddItem( new PlateChest() );
			}
			AddItem( new PlateArms() );
			AddItem( new PlateLegs() );
			AddItem( new PlateGloves() );
			AddItem( new PlateGorget() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon;
			
			switch ( MeleeSkill )
			{
				case 0:
					switch ( Utility.Random( 5 ) ) 
					{ 
						case 0: weapon = new WarAxe(); break;
						case 1: weapon = new Mace(); break;
						case 2: weapon = new Maul(); break;
						case 3: weapon = new WarMace(); break;
						default:weapon = new WarHammer(); break;
					}
					break;

				case 1:
				case 2:
					switch ( Utility.Random( 5 ) ) 
					{ 
						case 0: weapon = new ShortSpear(); break;
						case 1: weapon = new Kryss(); break;
						case 3: weapon = new WarFork(); break;
						default:weapon = new Spear(); break;
					}
					break;

				default:
					switch ( Utility.Random( 6 ) ) 
					{ 
						case 0: weapon = new Longsword(); break;
						case 1: weapon = new VikingSword(); break;
						case 2: weapon = new TwoHandedAxe(); break;
						case 3: weapon = new Katana(); break;
						case 4: weapon = new Bardiche(); break;
						default:weapon = new Halberd(); break;
					}
					break;
			}

			weapon.Movable = false;
			weapon.Crafter = this;
			weapon.Quality = WeaponQuality.Exceptional;
			AddItem( weapon );

			if ( FindItemOnLayer( Layer.TwoHanded ) == null )
			{
				switch ( Utility.Random( 4 ) ) 
				{ 
					case 0: AddItem( new BronzeShield() ); break; 
					case 1: AddItem( new HeaterShield() ); break; 
					case 2: AddItem( new MetalKiteShield() ); break; 
					case 3: AddItem( new MetalShield() ); break;  
				} 
			}
		}

		protected override void SetStats()
		{
			Str = 150;
			Dex = 100;
			Int = 100;
			Hits = HitsMax;
			Stam = StamMax;
			Mana = ManaMax;
		}

		protected override void SetSkills()
		{
			switch ( MeleeSkill )
			{
				case 0: SetSkill( SkillName.Macing, 110, 120 ); break;
				case 1: SetSkill( SkillName.Fencing, 110, 120 ); break;
				default:SetSkill( SkillName.Swords, 110, 120 ); break;
			}
			SetSkill( SkillName.Tactics, 60, 80 );
			SetSkill( SkillName.Anatomy, 80, 100 );
			SetSkill( SkillName.DetectHidden, 90, 100 );
			SetSkill( SkillName.MagicResist, 80, 100 );
			SetSkill( SkillName.Focus, 80, 100 );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}
