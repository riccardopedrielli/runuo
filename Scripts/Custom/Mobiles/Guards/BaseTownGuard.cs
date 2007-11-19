using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Mobiles
{
	public abstract class BaseTownGuard : BaseCreature
	{
		private bool m_BackUP = false;

		public virtual bool BackUP{ get{ return m_BackUP; } set{ m_BackUP = value; } }
		//public override bool ReaquireOnMovement { get { return true; } } //to make guards more annoying uncomment this line.

		//This makes it so that guards will be able to attack Mobiles without crashing anything
		public override double GetFightModeRanking( Mobile m, FightMode acqType, bool bPlayerOnly )
		{
			bPlayerOnly = false;
			return base.GetFightModeRanking(m, acqType, bPlayerOnly);
		}

		//This was added for guards that do not need a target.  They will find their own.
		public BaseTownGuard( AIType AI ): base( AI, FightMode.Closest, 10, 1, 0.1, 4.0 ) 
		{ 
		}

		//Because of this section it makes it possible for players to create guards with their own AI's
		//See the included ArcherGuard and WarriorGuard for examples on how to do this.
		public BaseTownGuard( Mobile target, AIType AI ): base( AI, FightMode.Closest, 10, 1, 0.1, 4.0 ) 
		{ 
			if ( target != null )
			{
				Location = target.Location;
				Map = target.Map;

				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
			}

			if ( target == null )
				BackUP = false;
		}

		//All guards will default to AI_Melee unless you give them an alternate AI
		public BaseTownGuard( Mobile target ): base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 4.0 ) 
		{ 
			if ( target != null )
			{
				Location = target.Location;
				Map = target.Map;

				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
			}

			if ( target == null )
				BackUP = false;
		}

		public BaseTownGuard( Serial serial ) : base( serial )
		{
		}

		public abstract Mobile Focus{ get; set; }

		/// <summary>
		/// anything you dont want your guards to attack OR even do want them to attack
		/// goes in the IsEnemy Portion of the script.  Obviously since this is for
		/// IsEnemy if you set it true its going to attack it, if its false it will not.
		/// </summary>
		public override bool IsEnemy( Mobile m )
		{
			if (m == null)
				return false;

			// If you add in this line it will be not an enemy
			if ( m is BaseTownGuard || m is BaseVendor || m is BaseHealer || m is TownCrier )
				return false;

			GuardedRegion rgn = null;
			if (m.Region != null)
				rgn = m.Region as GuardedRegion;

			if ( rgn != null && m.Player )
				return ( m.Criminal || ( m.Kills >= 5 && !rgn.AllowReds) );

			BaseCreature c = m as BaseCreature;
			// This section is entirely for things based on BaseCreature
			if ( c != null )
			{
				if (c.Region != null )
					rgn = c.Region as GuardedRegion;

				if (rgn != null)
				{
					if ( c.Criminal || (( c.Kills >= 5 || c.AlwaysMurderer ) && !rgn.AllowReds ) )
						return true;

					//Console.WriteLine("My target is a creature by the name of "+c.Name);
					PlayerMobile pc = c.ControlMaster as PlayerMobile; //These added for protection of pets
					PlayerMobile ps = c.SummonMaster as PlayerMobile; //These added for protection of summoned creatures
					// screw bard pacified creatures they shouldnt be in town anywayz
				}
			}

			return false; //If you can figure out how to connect this back to basecreatures without a crash, post it please.
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

		#region Area for Random stuff for guards
		// If you dont want to use this part of the code you can remove it
		// but it will still be in all new versions of the code
		public virtual void GenerateRandomHair()
		{
			int hairHue = Utility.RandomHairHue();

			if ( Female )
			{
				switch ( Utility.Random( 10 ) )
				{
					case 0: AddItem( new Afro( hairHue ) ); break;
					case 1: AddItem( new KrisnaHair( hairHue ) ); break;
					case 2: AddItem( new PageboyHair( hairHue ) ); break;
					case 3: AddItem( new PonyTail( hairHue ) ); break;
					case 4: AddItem( new ReceedingHair( hairHue ) ); break;
					case 5: AddItem( new TwoPigTails( hairHue ) ); break;
					case 6: AddItem( new ShortHair( hairHue ) ); break;
					case 7: AddItem( new LongHair( hairHue ) ); break;
					case 8: AddItem( new BunsHair( hairHue ) ); break;
					default: break;
				}
			}
			else
			{
				switch ( Utility.Random( 9 ) )
				{
					case 0: AddItem( new Afro( hairHue ) ); break;
					case 1: AddItem( new KrisnaHair( hairHue ) ); break;
					case 2: AddItem( new PageboyHair( hairHue ) ); break;
					case 3: AddItem( new PonyTail( hairHue ) ); break;
					case 4: AddItem( new ReceedingHair( hairHue ) ); break;
					case 5: AddItem( new TwoPigTails( hairHue ) ); break;
					case 6: AddItem( new ShortHair( hairHue ) ); break;
					case 7: AddItem( new LongHair( hairHue ) ); break;
					default: break;
				}

				switch ( Utility.Random( 6 ) )
				{
					case 0: AddItem( new LongBeard( hairHue ) ); break;
					case 1: AddItem( new MediumLongBeard( hairHue ) ); break;
					case 2: AddItem( new Vandyke( hairHue ) ); break;
					case 3: AddItem( new Mustache( hairHue ) ); break;
					case 4: AddItem( new Goatee( hairHue ) ); break;
					default: break;
				}
			}
		}

		public virtual void GenerateBody( bool isFemale, bool randomHair )
		{
			Hue = Utility.RandomSkinHue();

			if ( isFemale )
			{
				Female = true;
				Body = 0x191;
				Name = NameList.RandomName( "female" );
			}
			else
			{
				Female = false;
				Body = 0x190;
				Name = NameList.RandomName( "male" );
			}

			if ( randomHair )
				GenerateRandomHair();
		}

		public virtual int GetRandomHue()
		{
			switch ( Utility.Random( 8 ) )
			{
				default:
				case 0: return 0;
				case 1: return Utility.RandomBlueHue();
				case 2: return Utility.RandomGreenHue();
				case 3: return Utility.RandomRedHue();
				case 4: return Utility.RandomYellowHue();
				case 5: return Utility.RandomNeutralHue();
				case 6: return Utility.RandomNondyedHue();
				case 7: return Utility.RandomMetalHue();
			}
		}

		private static Type[] m_StrongPotions = new Type[]
		{
			typeof( GreaterHealPotion ), typeof( GreaterHealPotion ), typeof( GreaterHealPotion ),
			typeof( GreaterCurePotion ), typeof( GreaterCurePotion ), typeof( GreaterCurePotion ),
			typeof( GreaterStrengthPotion ), typeof( GreaterStrengthPotion ),
			typeof( GreaterAgilityPotion ), typeof( GreaterAgilityPotion ),
			typeof( TotalRefreshPotion ), typeof( TotalRefreshPotion ),
			typeof( GreaterExplosionPotion )
		};

		private static Type[] m_WeakPotions = new Type[]
		{
			typeof( HealPotion ), typeof( HealPotion ), typeof( HealPotion ),
			typeof( CurePotion ), typeof( CurePotion ), typeof( CurePotion ),
			typeof( StrengthPotion ), typeof( StrengthPotion ),
			typeof( AgilityPotion ), typeof( AgilityPotion ),
			typeof( RefreshPotion ), typeof( RefreshPotion ),
			typeof( ExplosionPotion )
		};

		public void PackStrongPotions( int min, int max )
		{
			PackStrongPotions( Utility.RandomMinMax( min, max ) );
		}

		public void PackStrongPotions( int count )
		{
			for ( int i = 0; i < count; ++i )
				PackStrongPotion();
		}

		public void PackStrongPotion()
		{
			PackItem( Loot.Construct( m_StrongPotions ) );
		}

		public void PackWeakPotions( int min, int max )
		{
			PackWeakPotions( Utility.RandomMinMax( min, max ) );
		}

		public void PackWeakPotions( int count )
		{
			for ( int i = 0; i < count; ++i )
				PackWeakPotion();
		}

		public void PackWeakPotion()
		{
			PackItem( Loot.Construct( m_WeakPotions ) );
		}

		public Item Immovable( Item item )
		{
			item.Movable = false;
			return item;
		}

		public Item Newbied( Item item )
		{
			item.LootType = LootType.Newbied;
			return item;
		}

		public Item Rehued( Item item, int hue )
		{
			item.Hue = hue;
			return item;
		}

		public Item Layered( Item item, Layer layer )
		{
			item.Layer = layer;
			return item;
		}

		public Item Resourced( BaseWeapon weapon, CraftResource resource )
		{
			weapon.Resource = resource;
			return weapon;
		}

		public Item Resourced( BaseArmor armor, CraftResource resource )
		{
			armor.Resource = resource;
			return armor;
		}
		// End of removeable code
		#endregion
	}
}
