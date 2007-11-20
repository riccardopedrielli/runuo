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
		
		private BaseMount m_Mount;

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

			SpeechHue = Utility.RandomDyedHue();

			int hue = GetRandomHue(); //Insert your hue choice here
			AddItem( new Kilt( hue ) ); 
			AddItem( new BodySash( hue ) );
			AddItem( new Cloak( hue ) );
			AddItem( new Boots( hue ) );

			BaseWeapon weapon;
			// Pick a weapon
			switch ( Utility.Random( 6 ) ) 
			{ 
				case 0: weapon = new Longsword(); break;
				case 1: weapon = new VikingSword(); break;
				case 2: weapon = new TwoHandedAxe(); break;
				case 3: weapon = new Katana(); break;
				case 4: weapon = new Bardiche(); break;
				default:weapon = new Halberd(); break;
			}

			weapon.Movable = false;
			weapon.Crafter = this;
			weapon.Quality = WeaponQuality.Exceptional;
			AddItem( weapon );

			Item handTwo = this.FindItemOnLayer( Layer.TwoHanded );

			if (handTwo == null)
			{
				// Pick a shield
				switch ( Utility.Random( 4 ) ) 
				{ 
					case 0: AddItem( new BronzeShield() ); break; 
					case 1: AddItem( new HeaterShield() ); break; 
					case 2: AddItem( new MetalKiteShield() ); break; 
					case 3: AddItem( new MetalShield() ); break;  
				} 
			}
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

			SetSkill( SkillName.Swords, 105, 120 );
			SetSkill( SkillName.Tactics, 46, 87 );
			SetSkill( SkillName.Anatomy, 46, 87 );
			SetSkill( SkillName.DetectHidden, 64, 100 );
			SetSkill( SkillName.MagicResist, 60, 82 );
			SetSkill( SkillName.Focus, 36, 67 );
			SetSkill( SkillName.Wrestling, 25, 47 );

			m_Mount = new Horse();
			m_Mount.Rider = this;
		}

		public WarriorTownGuard( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			Item handTwo = this.FindItemOnLayer( Layer.TwoHanded );
			RemoveItem( this.FindItemOnLayer( Layer.TwoHanded ) );
			m_Mount.Damage(1000, null);
			return base.OnBeforeDeath();
		}

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
				else if ( !m_Owner.CanSee( target ) )
				{
					if ( !m_Owner.UseSkill( SkillName.DetectHidden ) && Utility.Random( 50 ) == 0 )
						m_Owner.Say( "Reveal!" );
				}
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
