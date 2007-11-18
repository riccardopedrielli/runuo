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
	[CorpseName( "an ethereal corpse" )]
	public class ShadowGuard : BaseGuard
	{
		public override bool BardImmune{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AlwaysAttackable { get { return true; } }

		private static object[] m_GuardParams = new object[1];

		private Timer m_AttackTimer, m_IdleTimer;

		private Mobile m_Focus;
		private ShadowGuard m_Guard;

		[Constructable]
		public ShadowGuard() : this(null)
		{
		}

		[Constructable]
		public ShadowGuard( Mobile target ) : base( target, AIType.AI_Mage )
		{
			Name = "shadow guard";
			//Hue = 0x4001; //testing Hues
            //Hue = 0x8026; //Normal Hue
            //Hue = 19999;

			Body = 0x190;
		    
			SetStr( 101, 110 ); 
			SetDex( 101, 110 ); 
			SetInt( 101, 110 );

			SetHits( 178, 201 );
			SetStam( 191, 200 );
			SetMana( 600 );

			SetDamage( 10, 22 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Cold, 25 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.Swords, 105, 120 );
			SetSkill( SkillName.Anatomy, 46, 87 );
			SetSkill( SkillName.Focus, 90.1, 100.1 );
			SetSkill( SkillName.Meditation, 120.0 );
			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Magery, 115.1, 120.0 );
			SetSkill( SkillName.Necromancy, 115.1, 120.0 );
			SetSkill( SkillName.SpiritSpeak, 115.1, 120.0 );
			SetSkill( SkillName.EvalInt, 75.1, 100.0 );

			new Horse().Rider = this;

			SetFameLevel( 4 );
			SetKarmaLevel( 4 );
			//Karma *= -1;

			DragonChest tunic = new DragonChest();
			tunic.Quality = ArmorQuality.Exceptional;
			AddItem( Resourced(tunic,CraftResource.ShadowIron) );

			DragonArms arms = new DragonArms();
			arms.Quality = ArmorQuality.Exceptional;
			AddItem( Resourced(arms,CraftResource.ShadowIron) );

			DragonLegs legs = new DragonLegs();
			legs.Quality = ArmorQuality.Exceptional;
			AddItem( Resourced(legs,CraftResource.ShadowIron) );

			DragonGloves gloves = new DragonGloves();
			gloves.Quality = ArmorQuality.Exceptional;
			AddItem( Resourced(gloves,CraftResource.ShadowIron) );

			PlateGorget gorget = new PlateGorget();
			gorget.Quality = ArmorQuality.Exceptional;
			AddItem( Resourced(gorget,CraftResource.ShadowIron) );

			DragonHelm helm = new DragonHelm();
			helm.Quality = ArmorQuality.Exceptional;
			AddItem( Resourced(helm,CraftResource.ShadowIron) );

			Boots boots = new Boots();
			boots.Quality = ClothingQuality.Exceptional;
			boots.Resource = CraftResource.ShadowIron;
			AddItem( boots );

			// Pick a random sword
			BaseWeapon weapon = null;
			switch ( Utility.Random( 7 )) 
			{ 
				case 0:
				{
					weapon = new Longsword() as BaseWeapon; 
					break;
				}
				case 1:
				{
					weapon = new Broadsword() as BaseWeapon; 
					break;
				}
				case 2:
				{
					weapon = new VikingSword() as BaseWeapon; 
					break;
				}
				case 3:
				{
					weapon = new BattleAxe() as BaseWeapon; 
					break;
				}
				case 4:
				{
					weapon = new TwoHandedAxe() as BaseWeapon; 
					break;
				}
				case 5:
				{
					weapon = new Katana() as BaseWeapon; 
					break;
				}
				default:
				{
					weapon = new Halberd() as BaseWeapon;
					break;
				}
			} 
			weapon.Quality = WeaponQuality.Exceptional;
			AddItem( Resourced(weapon,CraftResource.ShadowIron) );

			#region Pack And Stuff In Pack
			Container pack = new Backpack();

			pack.Movable = false;

			pack.DropItem( new Gold( 10, 25 ) );
			pack.DropItem( new Bandage( Utility.RandomMinMax( 10, 20 ) ) );
			AddItem( pack );

			// Added for use with Necromancer AI
			Spellbook book = new NecromancerSpellbook( (UInt64)0xFFFF );			
			book.LootType = LootType.Blessed;			
			PackItem( book );
			Bag bag = new Bag();
			bag.LootType = LootType.Blessed;
			bag.DropItem( new BagOfAllReagents( 50 ) );
			PackItem( bag );
			#endregion

			VirtualArmor = 40;
			this.NextCombatTime = DateTime.Now + TimeSpan.FromSeconds( 0.5 );
			this.Focus = target;
		}
												
		public ShadowGuard( Serial serial ) : base( serial )
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
			private ShadowGuard m_Guard;

			public AvengeTimer( ShadowGuard guard ) : base( TimeSpan.FromSeconds( 2.5 ), TimeSpan.FromSeconds( 1.0 ), 3 ) // change this 3 to whatever you want for a backup call for guards 3 = 3 guards called
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
                    BaseGuard wg = (BaseGuard)Activator.CreateInstance( GuardedRegion.RandomGuard( typeof( ShadowGuard ), ( (GuardedRegion)m_Guard.Region ).UseRandom ), m_GuardParams );
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
			private ShadowGuard m_Owner;

			public AttackTimer( ShadowGuard owner ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 0.1 ) )
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
			private ShadowGuard m_Owner;
			private int m_Stage;

			public IdleTimer( ShadowGuard owner ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.5 ) )
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
