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
	public class WarriorTownGuard : BaseTownGuard
	{
		private static object[] m_GuardParams = new object[1];

		private Timer m_AttackTimer, m_IdleTimer;

		private Mobile m_Focus;
		private WarriorTownGuard m_Guard;

		[Constructable]
		public WarriorTownGuard() : this(null)
		{
		}

		public WarriorTownGuard( Mobile target ) : base( target, AIType.AI_Melee )
		{
			GenerateBody( Utility.RandomBool(), Utility.RandomBool() );

			SetFameLevel( Utility.Random(1,5) );
			SetKarmaLevel( Utility.Random(1,5) );
			Karma *= -1; //this added so that guards have positive Karma

			InitStats( 150, 150, 100 );
			Title = "the guard";

			SpeechHue = Utility.RandomDyedHue();

			int hue = GetRandomHue(); //Insert your hue choice here
			AddItem( new FancyShirt( hue ) ); 
			AddItem( new BodySash( hue ) );
			AddItem( new Boots( hue ) );

			// Pick a random sword
			switch ( Utility.Random( 7 )) 
			{ 
				case 0:
				{
					Longsword weapon = new Longsword(); 
					weapon.Movable = false;
					weapon.Crafter = this;
					weapon.Quality = WeaponQuality.Exceptional;

					AddItem( weapon );
					break;
				}
				case 1:
				{
					Broadsword weapon = new Broadsword(); 
					weapon.Movable = false;
					weapon.Crafter = this;
					weapon.Quality = WeaponQuality.Exceptional;

					AddItem( weapon );
					break;
				}
				case 2:
				{
					VikingSword weapon = new VikingSword(); 
					weapon.Movable = false;
					weapon.Crafter = this;
					weapon.Quality = WeaponQuality.Exceptional;

					AddItem( weapon );
					break;
				}
				case 3:
				{
					BattleAxe weapon = new BattleAxe(); 
					weapon.Movable = false;
					weapon.Crafter = this;
					weapon.Quality = WeaponQuality.Exceptional;

					AddItem( weapon );
					break;
				}
				case 4:
				{
					TwoHandedAxe weapon = new TwoHandedAxe(); 
					weapon.Movable = false;
					weapon.Crafter = this;
					weapon.Quality = WeaponQuality.Exceptional;

					AddItem( weapon );
					break;
				}
				case 5:
				{
					Katana weapon = new Katana(); 
					weapon.Movable = false;
					weapon.Crafter = this;
					weapon.Quality = WeaponQuality.Exceptional;

					AddItem( weapon );
					break;
				}
				default:
				{
					Halberd weapon = new Halberd();
					weapon.Movable = false;
					weapon.Crafter = this;
					weapon.Quality = WeaponQuality.Exceptional;

					AddItem( weapon );
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
					case 0: AddItem( new BronzeShield() ); break; 
					case 1: AddItem( new HeaterShield() ); break; 
					case 2: AddItem( new MetalKiteShield() ); break; 
					case 3: AddItem( new MetalShield() ); break; 
					case 4: AddItem( new WoodenKiteShield() ); break; 
					case 5: AddItem( new WoodenShield() ); break; 
					case 6: AddItem( new OrderShield() ); break;
					case 7: AddItem( new ChaosShield() ); break;
				} 
			}
          
			switch( Utility.Random( 5 ) )
			{
				case 0: break;
				case 1: AddItem( new Bascinet() ); break;
				case 2: AddItem( new CloseHelm() ); break;
				case 3: AddItem( new NorseHelm() ); break;
				case 4: AddItem( new Helmet() ); break;

			}
			// Pick some armour
			switch( Utility.Random( 5 ) )
			{
				case 0: // Leather
					if ( Female )
					{
						switch( Utility.Random( 3 ) )
						{
							case 0: AddItem( new LeatherSkirt() ); break;
							case 1: AddItem( new LeatherShorts() ); break;
							case 2: AddItem( new LeatherLegs() ); break;
						}

						AddItem( new FemaleLeatherChest() );
					}
					else
					{
						AddItem( new LeatherChest() );
						AddItem( new LeatherLegs() );
					}
					AddItem( new LeatherArms() );
					AddItem( new LeatherGloves() );
					AddItem( new LeatherGorget() );
					break;

				case 1: // Studded Leather
					if ( Female )
					{
						AddItem( new FemaleStuddedChest() );
					}
					else
					{
						AddItem( new StuddedChest() );
					}
					AddItem( new StuddedArms() );
					AddItem( new StuddedLegs() );
					AddItem( new StuddedGloves() );
					AddItem( new StuddedGorget() );
					break;

				case 2: // Ringmail
					AddItem( new RingmailChest() );
					AddItem( new RingmailArms() );
					AddItem( new RingmailGloves() );
					AddItem( new RingmailLegs() );
					break;

				case 3: // Chain
					AddItem( new ChainChest() );
					AddItem( new ChainCoif() );
					AddItem( new ChainLegs() );
					break;

				case 4: // Plate
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
					break;
			}

			Container pack = new Backpack();

			pack.Movable = false;

			pack.DropItem( new Gold( 10, 25 ) );
			pack.DropItem( new Bandage( Utility.RandomMinMax( 10, 20 ) ) );
			AddItem( pack );

			SetSkill( SkillName.Swords, 105, 120 );
			SetSkill( SkillName.Tactics, 46, 87 );
			SetSkill( SkillName.Anatomy, 46, 87 );
			SetSkill( SkillName.DetectHidden, 64, 100 );
			SetSkill( SkillName.MagicResist, 60, 82 );
			SetSkill( SkillName.Focus, 36, 67 );
			SetSkill( SkillName.Wrestling, 25, 47 );

			new Horse().Rider = this;

			this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
			this.Focus = target;
		}

		public WarriorTownGuard( Serial serial ) : base( serial )
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

					if ( value != null )

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
						if ( oldFocus != null && !oldFocus.Alive )
							Say( "Thou hast suffered thy punishment, scoundrel." );
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
			private WarriorTownGuard m_Guard;

			public AvengeTimer( WarriorTownGuard guard ) : base( TimeSpan.FromSeconds( 2.5 ), TimeSpan.FromSeconds( 1.0 ), 3 ) // change this 3 to whatever you want for a backup call for guards 3 = 3 guards called
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
					//BaseTownGuard wg = (BaseTownGuard)Activator.CreateInstance( GuardedRegion.RandomGuard( typeof (WarriorTownGuard), ((GuardedRegion)m_Guard.Region).UseRandom ), m_GuardParams );
					//wg.BackUP = true; // this prevents backup from calling backup
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
			private WarriorTownGuard m_Owner;

			public AttackTimer( WarriorTownGuard owner ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 0.1 ) )
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
			private WarriorTownGuard m_Owner;
			private int m_Stage;

			public IdleTimer( WarriorTownGuard owner ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.5 ) )
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
