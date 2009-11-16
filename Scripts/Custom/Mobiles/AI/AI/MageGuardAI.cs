using System;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Misc;
using Server.Regions;
using Server.SkillHandlers;

namespace Server.Mobiles
{
	public class MageGuardAI : MageAI
	{
		private DateTime m_NextCastTime;
		private DateTime m_NextHealTime;

		public MageGuardAI( BaseCreature m ) : base( m )
		{
		}

		private TimeSpan GetDelay()
		{
			double del = ScaleByMagery( 3.0 );
			double min = 6.0 - (del * 0.75);
			double max = 6.0 - (del * 1.25);

			return TimeSpan.FromSeconds( min + ((max - min) * Utility.RandomDouble()) );
		}

		public override bool DoActionCombat()
		{
			Mobile c = m_Mobile.Combatant;
			m_Mobile.Warmode = true;

			if ( c == null || c.Deleted || !c.Alive || c.IsDeadBondedPet || !m_Mobile.CanSee( c ) || !m_Mobile.CanBeHarmful( c, false ) || c.Map != m_Mobile.Map )
			{
				// Our combatant is deleted, dead, hidden, or we cannot hurt them
				// Try to find another combatant

				if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
				{
					if ( m_Mobile.Debug )
						m_Mobile.DebugSay( "Something happened to my combatant, so I am going to fight {0}", m_Mobile.FocusMob.Name );

					m_Mobile.Combatant = c = m_Mobile.FocusMob;
					m_Mobile.FocusMob = null;
				}
				else
				{
					m_Mobile.DebugSay( "Something happened to my combatant, and nothing is around. I am on guard." );
					Action = ActionType.Guard;
					return true;
				}
			}

			if ( !m_Mobile.InLOS( c ) )
			{
				m_Mobile.DebugSay( "I can't see my target" );

				if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
				{
					m_Mobile.DebugSay( "Nobody else is around" );
					m_Mobile.Combatant = c = m_Mobile.FocusMob;
					m_Mobile.FocusMob = null;
				}
			}

			if ( SmartAI && !m_Mobile.StunReady && m_Mobile.Skills[SkillName.Wrestling].Value >= 80.0 && m_Mobile.Skills[SkillName.Anatomy].Value >= 80.0 )
				EventSink.InvokeStunRequest( new StunRequestEventArgs( m_Mobile ) );

			if ( !m_Mobile.InRange( c, m_Mobile.RangePerception ) )
			{
				// They are somewhat far away, can we find something else?

				if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
				{
					m_Mobile.Combatant = m_Mobile.FocusMob;
					m_Mobile.FocusMob = null;
				}
				else if ( !m_Mobile.InRange( c, m_Mobile.RangePerception * 3 ) )
				{
					m_Mobile.Combatant = null;
				}

				c = m_Mobile.Combatant;

				if ( c == null )
				{
					m_Mobile.DebugSay( "My combatant has fled, so I am on guard" );
					Action = ActionType.Guard;

					return true;
				}
			}

			if ( !m_Mobile.Controlled && !m_Mobile.Summoned && !m_Mobile.IsParagon )
			{
				if ( m_Mobile.Hits < m_Mobile.HitsMax * 20/100 )
				{
					// We are low on health, should we flee?

					bool flee = false;

					if ( m_Mobile.Hits < c.Hits )
					{
						// We are more hurt than them

						int diff = c.Hits - m_Mobile.Hits;

						flee = ( Utility.Random( 0, 100 ) > (10 + diff) ); // (10 + diff)% chance to flee
					}
					else
					{
						flee = Utility.Random( 0, 100 ) > 10; // 10% chance to flee
					}
					
					if ( flee )
					{
						if ( m_Mobile.Debug )
							m_Mobile.DebugSay( "I am going to flee from {0}", c.Name );

						Action = ActionType.Flee;
						return true;
					}
				}
			}

			if ( m_Mobile.Spell == null && DateTime.Now > m_NextCastTime && m_Mobile.InRange( c, 12 ) )
			{
				// We are ready to cast a spell

				Spell spell = null;
				Mobile toDispel = FindDispelTarget( true );

				if ( m_Mobile.Poisoned ) // Top cast priority is cure
				{
					m_Mobile.DebugSay( "I am going to cure myself" );

					spell = new CureSpell( m_Mobile, null );
				}
				else if ( toDispel != null ) // Something dispellable is attacking us
				{
					m_Mobile.DebugSay( "I am going to dispel {0}", toDispel );

					spell = DoDispel( toDispel );
				}
				else if ( SmartAI && m_Combo != -1 ) // We are doing a spell combo
				{
					spell = DoCombo( c );
				}
				else if ( SmartAI && (c.Spell is HealSpell || c.Spell is GreaterHealSpell) && !c.Poisoned ) // They have a heal spell out
				{
					spell = new PoisonSpell( m_Mobile, null );
				}
				else
				{
					spell = ChooseSpell( c );
				}

				// Now we have a spell picked
				// Move first before casting

				if ( SmartAI && toDispel != null )
				{
					if ( m_Mobile.InRange( toDispel, 10 ) )
						RunFrom( toDispel );
					else if ( !m_Mobile.InRange( toDispel, 12 ) )
						RunTo( toDispel );
				}
				else
				{
					RunTo( c );
				}

				if ( spell != null )
					spell.Cast();

				TimeSpan delay;

				if ( SmartAI || ( spell is DispelSpell ) )
					delay = TimeSpan.FromSeconds( m_Mobile.ActiveSpeed );
				else
					delay = GetDelay();

				m_NextCastTime = DateTime.Now + delay;
			}
			else if ( m_Mobile.Spell == null || !m_Mobile.Spell.IsCasting )
			{
				RunTo( c );
			}

			return true;
		}
	}
}
