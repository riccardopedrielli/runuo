using System;
using Server.Regions;

namespace Server.Mobiles
{
	public abstract class BaseTownGuard : BaseCreature
	{
		protected Timer m_AttackTimer, m_IdleTimer;

		protected Mobile m_Focus;

		//This makes it so that guards will be able to attack Mobiles without crashing anything
		public override double GetFightModeRanking( Mobile m, FightMode acqType, bool bPlayerOnly )
		{
			bPlayerOnly = false;
			return base.GetFightModeRanking(m, acqType, bPlayerOnly);
		}

		public BaseTownGuard( AIType AI ): base( AI, FightMode.Closest, 10, 1, 0.1, 4.0 ) 
		{
			GenerateBody();
			GenerateHair();
			GenerateArmor();
			GenerateWeapon();
			GenerateClothes();
			GenerateMount();
			SetTitle();
			SetNoLoot();
			SetStats();
			SetSkills();
			SetKarmaFame();
			SetSpeechHue();
			SetSpeeds();
		}

		public BaseTownGuard( Serial serial ) : base( serial )
		{
		}

		protected virtual void GenerateBody()
		{
			Hue = Utility.RandomSkinHue();

			if ( Utility.Random( 5 ) == 0 )
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
		}

		protected virtual void GenerateHair()
		{
			int hairHue = Utility.RandomHairHue();

			Utility.AssignRandomHair( this, hairHue );
			
			if ( !Female )
				Utility.AssignRandomFacialHair( this, hairHue );
		}

		protected virtual void GenerateArmor()
		{
		}

		protected virtual void GenerateWeapon()
		{
		}

		protected virtual void GenerateClothes()
		{
		}

		protected virtual void GenerateMount()
		{
			new Horse().Rider = this;
		}

		protected virtual void SetTitle()
		{
			Title = "the guard";
		}

		protected virtual void SetNoLoot()
		{
			foreach ( Item item in Items )
			{
				item.LootType = LootType.Newbied;
			}
		}

		protected virtual void SetStats()
		{
		}

		protected virtual void SetSkills()
		{
		}

		protected virtual void SetKarmaFame()
		{
			Karma = Utility.Random( 0, 5000 );
			Fame = Utility.Random( 0, 5000 );
		}

		protected virtual void SetSpeechHue()
		{
			SpeechHue = Utility.RandomDyedHue();
		}

		protected virtual void SetSpeeds()
		{
			ActiveSpeed = 0.08;
			PassiveSpeed = 1;
			CurrentSpeed = PassiveSpeed;
		}

		public override TimeSpan ReacquireDelay{ get{ return TimeSpan.FromSeconds( 5.0 ); } }

		public override bool OnBeforeDeath()
		{
			((BaseMount) Mount).Kill();
			return base.OnBeforeDeath();
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Focus
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

					PlayerMobile pc = c.ControlMaster as PlayerMobile; //These added for protection of pets
					PlayerMobile ps = c.SummonMaster as PlayerMobile; //These added for protection of summoned creatures
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

			switch ( version )
			{
				case 0:
				{
					m_IdleTimer = new IdleTimer( this );
					m_IdleTimer.Start();
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
		protected class AttackTimer : Timer
		{
			private BaseTownGuard m_Owner;

			public AttackTimer( BaseTownGuard owner ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 0.1 ) )
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

		protected class IdleTimer : Timer
		{
			private BaseTownGuard m_Owner;
			private int m_Stage;

			public IdleTimer( BaseTownGuard owner ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.5 ) )
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
			}
		}
	}
}
