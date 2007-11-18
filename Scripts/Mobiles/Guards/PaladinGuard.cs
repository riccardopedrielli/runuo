/**************************************
*    Killable Guards (GS Versions)    *
*            Version: 3.0             *
*                                     *   
*        Created by Admin_Shaka       *
*              07/07/2007             *
*                                     *
*          D I M E N S I O N S        * 
*          hell is only a word        *
*                                     *
*         www.dimensions.com.br       *
*                                     *
*      Original Script and Ideas by   *
*               Greystar              *
*                                     *
* Anyone can modify/redistribute this *
*  DO NOT REMOVE/CHANGE THIS HEADER!  *
**************************************/
using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;

namespace Server.Mobiles
{
	public class PaladinGuard : BaseGuard
	{
		private static object[] m_GuardParams = new object[1];

		private Timer m_AttackTimer, m_IdleTimer;
		private PaladinGuard m_Guard;
		private Mobile m_Focus;

		[Constructable]
		public PaladinGuard() : this(null)
		{
		}

		public PaladinGuard( Mobile target ) : base( target, AIType.AI_Paladin )
		{
			GenerateBody( Utility.RandomBool(), Utility.RandomBool() );

			SetFameLevel( Utility.Random(1,5) );
			SetKarmaLevel( Utility.Random(1,5) );
			Karma *= -1; //this added so that guards have positive Karma

			InitStats( 150, 125, 75 );
			Title = "the guard";

			SetMana( 600 );

			SpeechHue = Utility.RandomDyedHue();

			int hue = GetRandomHue(); //Insert your hue choice here
			AddItem( new FancyShirt( hue ) ); 
			AddItem( new BodySash( hue ) );
			AddItem( new Boots( hue ) );

			#region Pack And Stuff In Pack
			Container pack = new Backpack();

			pack.Movable = false;

			pack.DropItem( new Gold( 10, 25 ) );
			pack.DropItem( new Bandage( Utility.RandomMinMax( 10, 20 ) ) );
			AddItem( pack );

			// Added for use with Paladin AI
			Spellbook book = new BookOfChivalry( (ulong)0x3FF );			
			book.LootType = LootType.Blessed;			
			PackItem( book );
			#endregion

			// Pick a random sword
			switch ( Utility.Random( 7 )) 
			{ 
				case 0:
				{
					Longsword weapon = new Longsword(); 
					weapon.Quality = WeaponQuality.Exceptional;
					PackItem( Resourced(weapon,CraftResource.Valorite) );
					weapon.Movable = false;
					weapon.Crafter = this;

					AddItem( Resourced(weapon,CraftResource.Valorite) );
					break;
				}
				case 1:
				{
					Broadsword weapon = new Broadsword(); 
					weapon.Quality = WeaponQuality.Exceptional;
					PackItem( Resourced(weapon,CraftResource.Valorite) );
					weapon.Movable = false;
					weapon.Crafter = this;

					AddItem( Resourced(weapon,CraftResource.Valorite) );
					break;
				}
				case 2:
				{
					VikingSword weapon = new VikingSword(); 
					weapon.Quality = WeaponQuality.Exceptional;
					PackItem( Resourced(weapon,CraftResource.Valorite) );
					weapon.Movable = false;
					weapon.Crafter = this;

					AddItem( Resourced(weapon,CraftResource.Valorite) );
					break;
				}
				case 3:
				{
					BattleAxe weapon = new BattleAxe(); 
					weapon.Quality = WeaponQuality.Exceptional;
					PackItem( Resourced(weapon,CraftResource.Valorite) );
					weapon.Movable = false;
					weapon.Crafter = this;

					AddItem( Resourced(weapon,CraftResource.Valorite) );
					break;
				}
				case 4:
				{
					TwoHandedAxe weapon = new TwoHandedAxe(); 
					weapon.Quality = WeaponQuality.Exceptional;
					PackItem( Resourced(weapon,CraftResource.Valorite) );
					weapon.Movable = false;
					weapon.Crafter = this;

					AddItem( Resourced(weapon,CraftResource.Valorite) );
					break;
				}
				case 5:
				{
					Katana weapon = new Katana(); 
					weapon.Quality = WeaponQuality.Exceptional;
					PackItem( Resourced(weapon,CraftResource.Valorite) );
					weapon.Movable = false;
					weapon.Crafter = this;

					AddItem( Resourced(weapon,CraftResource.Valorite) );
					break;
				}
				default:
				{
					Halberd weapon = new Halberd();
					weapon.Quality = WeaponQuality.Exceptional;
					PackItem( Resourced(weapon,CraftResource.Valorite) );
					weapon.Movable = false;
					weapon.Crafter = this;

					AddItem(  Resourced(weapon,CraftResource.Valorite) );
					break;
				}
			} 

			Item handOne = this.FindItemOnLayer( Layer.OneHanded );
			Item handTwo = this.FindItemOnLayer( Layer.TwoHanded );

			if ( handTwo is BaseWeapon )
				handOne = handTwo;

			if (handTwo == null)
			{
				// Pick a random shield
				switch ( Utility.Random( 8 )) 
				{ 
					case 0: AddItem( Resourced(new BronzeShield(),CraftResource.Valorite) ); break; 
					case 1: AddItem( Resourced(new HeaterShield(),CraftResource.Valorite) ); break; 
					case 2: AddItem( Resourced(new MetalKiteShield(),CraftResource.Valorite) ); break; 
					case 3: AddItem( Resourced(new MetalShield(),CraftResource.Valorite) ); break; 
					case 4: AddItem( new WoodenKiteShield() ); break; 
					case 5: AddItem( new WoodenShield() ); break; 
					case 6: AddItem( Resourced(new OrderShield(),CraftResource.Valorite) ); break;
					case 7: AddItem( Resourced(new ChaosShield(),CraftResource.Valorite) ); break;
				} 
			}

			switch( Utility.Random( 5 ) )
			{
				case 0: break;
				case 1: AddItem( Resourced(new Bascinet(),CraftResource.Valorite) ); break;
				case 2: AddItem( Resourced(new CloseHelm(),CraftResource.Valorite) ); break;
				case 3: AddItem( Resourced(new NorseHelm(),CraftResource.Valorite) ); break;
				case 4: AddItem( Resourced(new Helmet(),CraftResource.Valorite) ); break;

			}
			// Pick some armour
			switch( Utility.Random( 3 ) )
			{
				case 0: // Ringmail
					AddItem( Resourced(new RingmailChest(),CraftResource.Valorite) );
					AddItem( Resourced(new RingmailArms(),CraftResource.Valorite) );
					AddItem( Resourced(new RingmailGloves(),CraftResource.Valorite) );
					AddItem( Resourced(new RingmailLegs(),CraftResource.Valorite) );
					break;

				case 1: // Chain
					AddItem( Resourced(new ChainChest(),CraftResource.Valorite) );
					AddItem( Resourced(new ChainCoif(),CraftResource.Valorite) );
					AddItem( Resourced(new ChainLegs(),CraftResource.Valorite) );
					break;

				case 2: // Plate
					if ( Female )
					{
						AddItem( Resourced(new FemalePlateChest(),CraftResource.Valorite) );
					}
					else
					{
						AddItem( Resourced(new PlateChest(),CraftResource.Valorite) );
					}
					AddItem( Resourced(new PlateArms(),CraftResource.Valorite) );
					AddItem( Resourced(new PlateLegs(),CraftResource.Valorite) );
					AddItem( Resourced(new PlateGloves(),CraftResource.Valorite) );
					AddItem( Resourced(new PlateGorget(),CraftResource.Valorite) );
					break;
			}

			// Added for Use with Paladin AI
			TithingPoints = 0x3E8;

			SetSkill( SkillName.Swords, 105, 120 );
			SetSkill( SkillName.Tactics, 46, 87 );
			SetSkill( SkillName.Anatomy, 46, 87 );
			SetSkill( SkillName.DetectHidden, 64, 100 );
			SetSkill( SkillName.Wrestling, 25, 47 );
			SetSkill( SkillName.Chivalry, 115.1, 120.1 );
			SetSkill( SkillName.Focus, 90.1, 100.1 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Magery, 95.1, 100.0 );

			new Horse().Rider = this;

			this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
			this.Focus = target;
		}

		public PaladinGuard( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Focus != null && m_Focus.Alive )
			new AvengeTimer( this ).Start(); // If a guard dies, three more guards will spawn
			return base.OnBeforeDeath();
		}

		/* /// Left in for debugging purposes can be removed if you like
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );
			if (this.BackUP)
				Console.WriteLine(" I Am Backup and I died ("+this.Name+")");
			else
				Console.WriteLine(" I Am a Guard and I died ("+this.Name+") after I called backup");
		}
		*/

		[CommandProperty( AccessLevel.GameMaster )]
		public override Mobile Focus
		{
			get
			{
				return m_Focus;
			}
			set
			{
				if ( Deleted )
					return;

				Mobile oldFocus = m_Focus;

				if ( oldFocus != value )
				{
					m_Focus = value;

					if ( value != null )
						this.AggressiveAction( value );

					Combatant = value;

					if ( oldFocus != null && !oldFocus.Alive )
						Say( "Thou hast suffered thy punishment, scoundrel." );

					if ( value != null )
						Say( 500131 ); // Thou wilt regret thine actions, swine!

					if ( m_AttackTimer != null )
					{
						m_AttackTimer.Stop();
						m_AttackTimer = null;
					}

					if ( m_IdleTimer != null )
					{
						m_IdleTimer.Stop();
						m_IdleTimer = null;
					}

					if ( m_Focus != null )
					{
						m_AttackTimer = new AttackTimer( this );
						m_AttackTimer.Start();
						((AttackTimer)m_AttackTimer).DoOnTick();
					}
					else
					{
						m_IdleTimer = new IdleTimer( this );
						m_IdleTimer.Start();
					}
				}
				else if ( m_Focus == null && m_IdleTimer == null )
				{
					m_IdleTimer = new IdleTimer( this );
					m_IdleTimer.Start();
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Focus );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Focus = reader.ReadMobile();

					if ( m_Focus != null )
					{
						m_AttackTimer = new AttackTimer( this );
						m_AttackTimer.Start();
					}
					else
					{
						m_IdleTimer = new IdleTimer( this );
						m_IdleTimer.Start();
					}

					break;
				}
			}
		}

		public override void OnAfterDelete()
		{
			if ( m_AttackTimer != null )
			{
				m_AttackTimer.Stop();
				m_AttackTimer = null;
			}

			if ( m_IdleTimer != null )
			{
				m_IdleTimer.Stop();
				m_IdleTimer = null;
			}

			base.OnAfterDelete();
		}

		// This section of code could probably be cleaned up in the future but it does
		// what I wanted it to at this time.
		private class AvengeTimer : Timer
		{
			private Mobile m_Focus;
			private PaladinGuard m_Guard;

			public AvengeTimer( PaladinGuard guard ) : base( TimeSpan.FromSeconds( 2.5 ), TimeSpan.FromSeconds( 1.0 ), 3 ) // change this 3 to whatever you want for a backup call for guards 3 = 3 guards called
			{
				m_Guard = guard;
				if (guard.Focus != null)
					m_GuardParams[0] = m_Focus = guard.Focus;
				else if (guard.Focus == null && guard.Combatant != null)
					m_GuardParams[0] = m_Focus = guard.Combatant;
			}

			public void CallBackup( Mobile focus )
			{
				if ( !m_Guard.BackUP )
				{
                    BaseGuard wg = (BaseGuard)Activator.CreateInstance( GuardedRegion.RandomGuard( typeof( PaladinGuard ), ( (GuardedRegion)m_Guard.Region ).UseRandom ), m_GuardParams );
						wg.BackUP = true; // this prevents backup from calling backup
						//Console.WriteLine(" I Am Backup and I am Alive ("+wg.Name+")");
				}
				/*else
					Console.WriteLine(" I Am Backup and I died ");*/
			}

			protected override void OnTick()
			{
				//CallBackup(m_Focus);
			}
		}

		private class AttackTimer : Timer
		{
			private PaladinGuard m_Owner;

			public AttackTimer( PaladinGuard owner ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Owner = owner;
			}

			public void DoOnTick()
			{
				OnTick();
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				m_Owner.Criminal = false;
				m_Owner.Kills = 0;
				m_Owner.Stam = m_Owner.StamMax;

				Mobile target = m_Owner.Focus;

				if ( target != null && (target.Deleted || !target.Alive || !m_Owner.CanBeHarmful( target )) )	
				{
					m_Owner.Focus = null;
					Stop();
					return;
				}

				if ( target != null && m_Owner.Combatant != target )
					m_Owner.Combatant = target;

				if ( target == null )
				{
					Stop();
				}
				else if ( !m_Owner.InRange( target, 20 ) )
				{
					m_Owner.Focus = null;
				}
				/*else if ( !m_Owner.InRange( target, 10 ) || !m_Owner.InLOS( target ) )
				{
					TeleportTo( target );
				}
				else if ( !m_Owner.InRange( target, 1 ) )
				{
					if ( !m_Owner.Move( m_Owner.GetDirectionTo( target ) | Direction.Running ) )
						TeleportTo( target );
				}*/
				else if ( !m_Owner.CanSee( target ) )
				{
					if ( !m_Owner.UseSkill( SkillName.DetectHidden ) && Utility.Random( 50 ) == 0 )
						m_Owner.Say( "Reveal!" );
				}
			}

			private void TeleportTo( Mobile target )
			{
				Point3D from = m_Owner.Location;
				Point3D to = target.Location;

				m_Owner.Location = to;

				Effects.SendLocationParticles( EffectItem.Create( from, m_Owner.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				Effects.SendLocationParticles( EffectItem.Create(   to, m_Owner.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

				m_Owner.PlaySound( 0x1FE );
			}
		}

		private class IdleTimer : Timer
		{
			private PaladinGuard m_Owner;
			private int m_Stage;

			public IdleTimer( PaladinGuard owner ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.5 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				//Not exactly sure what purpose this serves
				if ( (m_Stage++ % 4) == 0 || !m_Owner.Move( m_Owner.Direction ) )
					m_Owner.Direction = (Direction)Utility.Random( 8 );

				if ( m_Stage > 16 )
				{
					Effects.SendLocationParticles( EffectItem.Create( m_Owner.Location, m_Owner.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					m_Owner.PlaySound( 0x1FE );

					m_Owner.Delete();
				}
			}
		}
	}
}