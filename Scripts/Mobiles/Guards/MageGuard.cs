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
using Server.Regions;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
	public class MageGuard : BaseGuard
	{
		private static object[] m_GuardParams = new object[1];
		private Timer m_AttackTimer, m_IdleTimer;

		private Mobile m_Focus;

		[Constructable]
		public MageGuard() : this( null )
		{
		}

		[Constructable]
		public MageGuard( Mobile target ) : base( target, AIType.AI_Mage )
		{
			// Uses the routine from BaseGuard.cs to get this
			GenerateBody( Utility.RandomBool(), Utility.RandomBool() );

			SetFameLevel( Utility.Random(1,5) );
			SetKarmaLevel( Utility.Random(1,5) );
			Karma *= -1; //this added so that guards have positive Karma

			InitStats( 75, 125, 150 );
			Title = "the guard";

			SpeechHue = Utility.RandomDyedHue();

			int hue = GetRandomHue(); //Insert your hue here
			AddItem( new FancyShirt( hue ) ); 
			AddItem( new BodySash( hue ) );
			AddItem( new Boots( hue ) );
			AddItem( new Robe( hue ) );

			// Pick some armour
			switch( Utility.Random(1,2) ) //Could probably change this to a RandomBool somehow
			{
				case 1: // Leather
					if ( Female )
					{
						switch( Utility.Random( 3 ) )
						{
							case 0: AddItem( Rehued ( new LeatherSkirt(), hue ) ); break;
							case 1: AddItem( Rehued ( new LeatherShorts(), hue ) ); break;
							case 2: AddItem( Rehued ( new LeatherLegs(), hue ) ); break;
						}

						AddItem( Rehued ( new FemaleLeatherChest(), hue ) );
						AddItem( Rehued ( new LeatherBustierArms(), hue ) );
					}
					else
					{
						AddItem( Rehued ( new LeatherChest(), hue ) );
						AddItem( Rehued ( new LeatherArms(), hue ) );
						AddItem( Rehued ( new LeatherLegs(), hue ) );
					}
					AddItem( Rehued ( new LeatherGloves(), hue ) );
					AddItem( Rehued ( new LeatherGorget(), hue ) );
					break;

				case 2: // Studded Leather
					if ( Female )
					{
						AddItem( Rehued ( new FemaleStuddedChest(), hue ) );
						AddItem( Rehued ( new StuddedBustierArms(), hue ) );
					}
					else
					{
						AddItem( Rehued ( new StuddedChest(), hue ) );
						AddItem( Rehued ( new StuddedArms(), hue ) );
					}
					AddItem( Rehued ( new StuddedLegs(), hue ) );
					AddItem( Rehued ( new StuddedGloves(), hue ) );
					AddItem( Rehued ( new StuddedGorget(), hue ) );
					break;
			}

			Container pack = new Backpack();

			pack.Movable = false;

			#region Reagent Section
			Container bag = new Bag();
			int count = Utility.RandomMinMax( 10, 20 );
			for ( int i = 0; i < count; ++i )
			{
				Item item = Loot.RandomReagent();

				if ( item == null )
					continue;

				if ( !bag.TryDropItem( this, item, false ) )
					item.Delete();
			}
			pack.DropItem( Rehued ( bag, hue ) );
			#endregion

			pack.DropItem( new Gold( 10, 25 ) );
			pack.DropItem( new Bandage( Utility.RandomMinMax( 10, 20 ) ) );

			AddItem( pack );

			SetSkill( SkillName.Tactics, 46.0, 87.0 );
			SetSkill( SkillName.DetectHidden, 64.0, 100.0 );
			SetSkill( SkillName.Wrestling, 95.1, 105.0 );
			SetSkill( SkillName.Focus, 90.1, 100.1 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Magery, 115.1, 120.0 );
			SetSkill( SkillName.EvalInt, 75.1, 100.0 );

			new Horse().Rider = this;

			this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
			this.Focus = target;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.MedScrolls, 1 );
		}

		public MageGuard( Serial serial ) : base( serial )
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
			private MageGuard m_Guard;

			public AvengeTimer( MageGuard guard ) : base( TimeSpan.FromSeconds( 2.5 ), TimeSpan.FromSeconds( 1.0 ), 3 ) // change this 3 to whatever you want for a backup call for guards 3 = 3 guards called
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
                    BaseGuard ag = (BaseGuard)Activator.CreateInstance( GuardedRegion.RandomGuard( typeof( MageGuard ), ( (GuardedRegion)m_Guard.Region ).UseRandom ), m_GuardParams );
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
			private MageGuard m_Owner;
			private bool m_Casting;

			public AttackTimer( MageGuard owner ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 0.1 ) )
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
					m_Casting = false;
					m_Owner.Focus = null;
				}/*
				else if ( !m_Owner.InLOS( target ) )
				{
					m_Casting = false;
					TeleportTo( target );
				}*/
				else if ( !m_Owner.CanSee( target ) )
				{
					m_Casting = false;

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
					if ( m_Casting && (TimeToSpare() || OutOfMaxDistance( target )) )
						m_Casting = false;
					else if ( !m_Casting && InMinDistance( target ) )
						m_Casting = true;

					if ( !m_Casting )
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
				return !m_Owner.InRange( target, 10 );
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
			private MageGuard m_Owner;
			private int m_Stage;

			public IdleTimer( MageGuard owner ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.5 ) )
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