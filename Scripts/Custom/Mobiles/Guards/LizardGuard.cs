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
	[CorpseName( "a lizardman corpse" )]
	public class LizardGuard : BaseGuard
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Lizardman; } }
		private static object[] m_GuardParams = new object[1];

		private Timer m_AttackTimer, m_IdleTimer;
		private LizardGuard m_Guard;
		private Mobile m_Focus;

		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public LizardGuard() : this(null)
		{
		}

		[Constructable]
		public LizardGuard( Mobile target ) : base( target, AIType.AI_Melee )
		{
			Name = NameList.RandomName( "lizardman" );
			Body = Utility.RandomList( 35, 36 );
			BaseSoundID = 417;

			InitStats( 150, 125, 75 );

			SetHits( 251, 350 );

			SetDamage( 13, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 10, 20 );

			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Tactics, 60.1, 120.0 );
			SetSkill( SkillName.Wrestling, 60.1, 120.0 );
			SetSkill( SkillName.Anatomy, 60.1, 120.0 );

			SetFameLevel( Utility.Random(1,5) );
			SetKarmaLevel( Utility.Random(1,5) );
			Karma *= -1; //this added so that guards have positive Karma

			VirtualArmor = 28;
			this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
			this.Focus = target;
		}

		public override bool CanRummageCorpses{ get{ return false; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Spined; } }

		public LizardGuard( Serial serial ) : base( serial )
		{
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Focus != null && m_Focus.Alive )
				new AvengeTimer( this ).Start(); // If a guard dies, three more guards will spawn
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
		private class AvengeTimer : Timer
		{
			private Mobile m_Focus;
			private LizardGuard m_Guard;

			public AvengeTimer( LizardGuard guard ) : base( TimeSpan.FromSeconds( 2.5 ), TimeSpan.FromSeconds( 1.0 ), 3 ) // change this 3 to whatever you want for a backup call for guards 3 = 3 guards called
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
                    BaseGuard wg = (BaseGuard)Activator.CreateInstance( GuardedRegion.RandomGuard( typeof( LizardGuard ), ( (GuardedRegion)m_Guard.Region ).UseRandom ), m_GuardParams );
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
			private LizardGuard m_Owner;

			public AttackTimer( LizardGuard owner ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 0.1 ) )
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
				{
					if (target is BaseGuard)
						m_Owner.Combatant = null;
					else
					{
						m_Owner.Combatant = target;
						m_Owner.Say( 500131 ); // Thou wilt regret thine actions, swine!
					}
				}

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
			private LizardGuard m_Owner;
			private int m_Stage;

			public IdleTimer( LizardGuard owner ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.5 ) )
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