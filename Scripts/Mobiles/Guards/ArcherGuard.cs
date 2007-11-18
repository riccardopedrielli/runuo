/**************************************
*    Killable Guards (GS Versions)    *
*            Version: 3.0             *
*                                     *   
*      Distro files: ArcherGuard.cs   *
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

/// <summary>
/// Distro ArcherGuard edited by Greystar to make them killable
/// Without having instakill any longer.
/// Verion 1.1.0
/// Date 02/26/2006		Time: 01:54 Central Standard Time
/// Special Thanks to Shadow1980 and TheN for assistance with 
/// testing.  Whole bunch of changes to this file to make things 
/// work correctly.  Added colored items like sashes and boots 
/// and shirts becuase in the future I'm colorcoding my guards 
/// by what city/facet they are from.  These guards also only 
/// have one teleportation thing still uncommented, if you still 
/// want them to teleport just remove either the double slashes 
/// from the area or the /**/ from the section you want to work.
/// Guards now can be random.
/// </summary>

using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Regions;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
	public class ArcherGuard : BaseGuard
	{
		private static object[] m_GuardParams = new object[1];
		private Timer m_AttackTimer, m_IdleTimer;

		private Mobile m_Focus;

		[Constructable]
		public ArcherGuard() : this( null )
		{
		}

		[Constructable]
		public ArcherGuard( Mobile target ) : base( target, AIType.AI_Archer )
		{
			// Uses the routine from BaseGuard.cs to get this
			GenerateBody( Utility.RandomBool(), Utility.RandomBool() );

			SetFameLevel( Utility.Random(1,5) );
			SetKarmaLevel( Utility.Random(1,5) );
			Karma *= -1; //this added so that guards have positive Karma

			InitStats( 75, 150, 125 );
			Title = "the guard";

			SpeechHue = Utility.RandomDyedHue();

			Horse horse = new Horse();
			horse.Rider = this; 

			int hue = GetRandomHue(); //Insert your hue here
			AddItem( new FancyShirt( hue ) ); 
			AddItem( new BodySash( hue ) );
			AddItem( new Boots( hue ) );

			// Pick some armour
			switch( Utility.Random(1,2) ) //Could probably change this to a RandomBool somehow
			{
				case 1: // Leather
					if ( Female )
					{
						switch( Utility.Random( 3 ) )
						{
							case 0: AddItem( new LeatherSkirt() ); break;
							case 1: AddItem( new LeatherShorts() ); break;
							case 2: AddItem( new LeatherLegs() ); break;
						}

						AddItem( new FemaleLeatherChest() );
						AddItem( new LeatherBustierArms() );
					}
					else
					{
						AddItem( new LeatherChest() );
						AddItem( new LeatherArms() );
						AddItem( new LeatherLegs() );
					}
					AddItem( new LeatherGloves() );
					AddItem( new LeatherGorget() );
					break;

				case 2: // Studded Leather
					if ( Female )
					{
						AddItem( new FemaleStuddedChest() );
						AddItem( new StuddedBustierArms() );
					}
					else
					{
						AddItem( new StuddedChest() );
						AddItem( new StuddedArms() );
					}
					AddItem( new StuddedLegs() );
					AddItem( new StuddedGloves() );
					AddItem( new StuddedGorget() );
					break;
			}

			Bow bow = new Bow();
			bow.Movable = false;
			bow.Crafter = this;
			bow.Quality = WeaponQuality.Exceptional;

			AddItem( bow );

			Container pack = new Backpack();

			pack.Movable = false;

			pack.DropItem( new Arrow( 250 ) );
			pack.DropItem( new Gold( 10, 25 ) );
			pack.DropItem( new Bandage( Utility.RandomMinMax( 10, 20 ) ) );

			AddItem( pack );

			SetSkill( SkillName.Archery, 105.0, 120.0 );
			SetSkill( SkillName.Tactics, 46.0, 87.0 );
			SetSkill( SkillName.Anatomy, 46.0, 87.0 );
			SetSkill( SkillName.DetectHidden, 64.0, 100.0 );
			SetSkill( SkillName.MagicResist, 60.0, 82.0 );
			SetSkill( SkillName.Focus, 36.0, 67.0 );
			SetSkill( SkillName.Wrestling, 25.0, 47.0 );

			this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
			this.Focus = target;
		}

		public ArcherGuard( Serial serial ) : base( serial )
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
			private ArcherGuard m_Guard;

			public AvengeTimer( ArcherGuard guard ) : base( TimeSpan.FromSeconds( 2.5 ), TimeSpan.FromSeconds( 1.0 ), 3 ) // change this 3 to whatever you want for a backup call for guards 3 = 3 guards called
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
                    BaseGuard ag = (BaseGuard)Activator.CreateInstance( GuardedRegion.RandomGuard( typeof( ArcherGuard ), ( (GuardedRegion)m_Guard.Region ).UseRandom ), m_GuardParams );
					ag.BackUP = true; // this prevents backup from calling backup
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
			private ArcherGuard m_Owner;
			private bool m_Shooting;

			public AttackTimer( ArcherGuard owner ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 0.1 ) )
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
					m_Shooting = false;
					m_Owner.Focus = null;
				}/*
				else if ( !m_Owner.InLOS( target ) )
				{
					m_Shooting = false;
					TeleportTo( target );
				}*/
				else if ( !m_Owner.CanSee( target ) )
				{
					m_Shooting = false;

					/*if ( !m_Owner.InRange( target, 2 ) )
					{
						if ( !m_Owner.Move( m_Owner.GetDirectionTo( target ) | Direction.Running ) && OutOfMaxDistance( target ) )
							TeleportTo( target );
					}
					else
					{*/
						if ( !m_Owner.UseSkill( SkillName.DetectHidden ) && Utility.Random( 50 ) == 0 )
							m_Owner.Say( "Reveal!" );
					//}
				}
				else
				{
					if ( m_Shooting && (TimeToSpare() || OutOfMaxDistance( target )) )
						m_Shooting = false;
					else if ( !m_Shooting && InMinDistance( target ) )
						m_Shooting = true;

					if ( !m_Shooting )
					{
						/*if ( m_Owner.InRange( target, 1 ) )
						{
							if ( !m_Owner.Move( (Direction)(m_Owner.GetDirectionTo( target ) - 4) | Direction.Running ) && OutOfMaxDistance( target ) ) // Too close, move away
								TeleportTo( target );
						}
						else if ( !m_Owner.InRange( target, 2 ) )
						{
							if ( !m_Owner.Move( m_Owner.GetDirectionTo( target ) | Direction.Running ) && OutOfMaxDistance( target ) )
								TeleportTo( target );
						}*/
					}
				}
			}

			private bool TimeToSpare()
			{
				return (m_Owner.NextCombatTime - DateTime.Now) > TimeSpan.FromSeconds( 1.0 );
			}

			private bool OutOfMaxDistance( Mobile target )
			{
				return !m_Owner.InRange( target, m_Owner.Weapon.MaxRange );
			}

			private bool InMinDistance( Mobile target )
			{
				return m_Owner.InRange( target, 4 );
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
			private ArcherGuard m_Owner;
			private int m_Stage;

			public IdleTimer( ArcherGuard owner ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.5 ) )
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