using System;
using System.Collections;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
	public class WarriorGuardAI : MeleeAI
	{
		public WarriorGuardAI(BaseCreature m) : base (m)
		{
		}

		public override bool DoActionCombat()
		{
			Mobile combatant = m_Mobile.Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive || combatant.IsDeadBondedPet )
			{
				m_Mobile.DebugSay( "My combatant is gone, so my guard is up" );

				Action = ActionType.Guard;

				return true;
			}

			if ( !m_Mobile.InRange( combatant, m_Mobile.RangePerception ) )
			{
				// They are somewhat far away, can we find something else?

				if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
				{
					m_Mobile.Combatant = m_Mobile.FocusMob;
					m_Mobile.FocusMob = null;
				}
				else if ( !m_Mobile.InRange( combatant, m_Mobile.RangePerception * 3 ) )
				{
					m_Mobile.Combatant = null;
				}

				combatant = m_Mobile.Combatant;

				if ( combatant == null )
				{
					m_Mobile.DebugSay( "My combatant has fled, so I am on guard" );
					Action = ActionType.Guard;

					return true;
				}
			}

			/*if ( !m_Mobile.InLOS( combatant ) )
			{
				if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
				{
					m_Mobile.Combatant = combatant = m_Mobile.FocusMob;
					m_Mobile.FocusMob = null;
				}
			}*/

			if ( WalkMobileRange(m_Mobile.Combatant, 1, true, m_Mobile.RangeFight, m_Mobile.Weapon.MaxRange) )
			{
				m_Mobile.Direction = m_Mobile.GetDirectionTo( combatant );
			}
			else if ( AcquireFocusMob( m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true ) )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name );

				m_Mobile.Combatant = m_Mobile.FocusMob;
				Action = ActionType.Combat;

				return true;
			}
			else if ( m_Mobile.GetDistanceToSqrt( combatant ) > m_Mobile.RangePerception + 1 )
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "I cannot find {0}, so my guard is up", combatant.Name );

				Action = ActionType.Guard;

				return true;
			}
			else
			{
				if ( m_Mobile.Debug )
					m_Mobile.DebugSay( "I should be closer to {0}", combatant.Name );
			}

			/*** MOD_START ***/
			/*
			if ( !m_Mobile.Controlled && !m_Mobile.Summoned && !m_Mobile.IsParagon )
			*/

			if ( !m_Mobile.Controlled && !m_Mobile.Summoned && !m_Mobile.IsParagon && !(m_Mobile is BaseTownGuard) )
			/*** MOD_END ***/

			{
				if ( m_Mobile.Hits < m_Mobile.HitsMax * 20/100 )
				{
					// We are low on health, should we flee?

					bool flee = false;

					if ( m_Mobile.Hits < combatant.Hits )
					{
						// We are more hurt than them

						int diff = combatant.Hits - m_Mobile.Hits;

						flee = ( Utility.Random( 0, 100 ) < (10 + diff) ); // (10 + diff)% chance to flee
					}
					else
					{
						flee = Utility.Random( 0, 100 ) < 10; // 10% chance to flee
					}

					if ( flee )
					{
						if ( m_Mobile.Debug )
							m_Mobile.DebugSay( "I am going to flee from {0}", combatant.Name );

						Action = ActionType.Flee;
					}
				}
			}

			return true;
		}
	}
}
