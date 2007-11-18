/**************************************
*    Killable Guards (GS Versions)    *
*            Version: 3.0             *
*                                     *   
*    Distro files: GuardedRegion.cs   *
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
using System.Collections.Generic;
using System.Xml;
using Server;
using Server.Misc; // Added by 2.0 Shaka큦 GS Killable Guards
using Server.Items; // Added by 2.0 Shaka큦 GS Killable Guards
using Server.Commands;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting; // Added by 2.0 Shaka큦 GS Killable Guards

namespace Server.Regions
{
	public class GuardedRegion : BaseRegion
	{
		private static object[] m_GuardParams = new object[1];
		private Type m_GuardType;
		private bool m_Disabled;
		private bool m_AllowReds = false; // Added by 2.0 Shaka큦 GS Killable Guards
		private bool m_UseRandom = false; // Added by 2.0 Shaka큦 GS Killable Guards
		private static bool RndG = false; // Added by 2.0 Shaka큦 GS Killable Guards

		public bool Disabled{ get{ return m_Disabled; } set{ m_Disabled = value; } }
		public bool Allowed{ get{ return m_AllowReds; } set{ m_AllowReds = value; } } // Added by 2.0 Shaka큦 GS Killable Guards
		public bool UseRandom { get { return m_UseRandom; } set { m_UseRandom = value; } } // Added by 2.0 Shaka큦 GS Killable Guards

		public virtual bool IsDisabled()
		{
			return m_Disabled;
		}

		public static void Initialize()
		{
			CommandSystem.Register( "CheckGuarded", AccessLevel.GameMaster, new CommandEventHandler( CheckGuarded_OnCommand ) );
			CommandSystem.Register( "SetGuarded", AccessLevel.Administrator, new CommandEventHandler( SetGuarded_OnCommand ) );
			CommandSystem.Register( "ToggleGuarded", AccessLevel.Administrator, new CommandEventHandler( ToggleGuarded_OnCommand ) );
			CommandSystem.Register( "CheckReds", AccessLevel.GameMaster, new CommandEventHandler( CheckReds_OnCommand ) ); // Added by 2.0 Shaka큦 GS Killable Guards
			CommandSystem.Register( "SetAllowReds", AccessLevel.Administrator, new CommandEventHandler( SetAllowReds_OnCommand ) ); // Added by 2.0 Shaka큦 GS Killable Guards
			CommandSystem.Register( "ToggleReds", AccessLevel.Administrator, new CommandEventHandler( ToggleReds_OnCommand ) ); // Added by 2.0 Shaka큦 GS Killable Guards
			CommandSystem.Register( "RandGuard", AccessLevel.Administrator, new CommandEventHandler( RandGuard_OnCommand ) ); // Added by 2.0 Shaka큦 GS Killable Guards

		}

		[Usage( "CheckGuarded" )]
		[Description( "Returns a value indicating if the current region is guarded or not." )]
		private static void CheckGuarded_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			GuardedRegion reg = (GuardedRegion) from.Region.GetRegion( typeof( GuardedRegion ) );

			if ( reg == null )
				from.SendMessage( "You are not in a guardable region." );
			else if ( reg.Disabled )
				from.SendMessage( "The guards in this region have been disabled." );
			else
				from.SendMessage( "This region is actively guarded." );
		}

		[Usage( "SetGuarded <true|false>" )]
		[Description( "Enables or disables guards for the current region." )]
		private static void SetGuarded_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Length == 1 )
			{
				GuardedRegion reg = (GuardedRegion) from.Region.GetRegion( typeof( GuardedRegion ) );

				if ( reg == null )
				{
					from.SendMessage( "You are not in a guardable region." );
				}
				else
				{
					reg.Disabled = !e.GetBoolean( 0 );

					if ( reg.Disabled )
						from.SendMessage( "The guards in this region have been disabled." );
					else
						from.SendMessage( "The guards in this region have been enabled." );
				}
			}
			else
			{
				from.SendMessage( "Format: SetGuarded <true|false>" );
			}
		}

		[Usage( "ToggleGuarded" )]
		[Description( "Toggles the state of guards for the current region." )]
		private static void ToggleGuarded_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			GuardedRegion reg = (GuardedRegion) from.Region.GetRegion( typeof( GuardedRegion ) );

			if ( reg == null )
			{
				from.SendMessage( "You are not in a guardable region." );
			}
			else
			{
				reg.Disabled = !reg.Disabled;

				if ( reg.Disabled )
					from.SendMessage( "The guards in this region have been disabled." );
				else
					from.SendMessage( "The guards in this region have been enabled." );
			}
		}

// Adition by 2.0 Shaka큦 GS Killable Guards starts here
		[Usage( "RandGuard" )]
		[Description( "Toggles the state of weather or not guards are random in a guarded area." )]
		private static void RandGuard_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			GuardedRegion reg = from.Region as GuardedRegion;

			if ( reg == null )
			{
				from.SendMessage( "You are not in a guardable region." );
			}
			else
			{
				if ( reg.UseRandom )
					reg.UseRandom = false;
				else
					reg.UseRandom = true;

				RndG = reg.UseRandom;

				if ( reg.UseRandom )
					from.SendMessage( "The guards have been set to random in this region." );
				else
					from.SendMessage( "The guards are no longer random in this region." );
			}
		}

		[Usage( "ToggleReds" )]
		[Description( "Toggles the state of weather or not reds are allowed in guarded area." )]
		private static void ToggleReds_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			GuardedRegion reg = from.Region as GuardedRegion;

			if ( reg == null )
			{
				from.SendMessage( "You are not in a guardable region." );
			}
			else
			{
				if ( reg.Allowed )
					reg.Allowed = false;
				else
					reg.Allowed = true;

				if ( reg.Allowed )
					from.SendMessage( "The guards and vendors in this region allow Murderers." );
				else
					from.SendMessage( "The guards and vendors in this region do not allow Murderers." );
			}
		}

		[Usage( "SetAllowReds <true|false>" )]
		[Description( "Enables or disables allowing reds for the current region." )]
		private static void SetAllowReds_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;

			if ( e.Length == 1 )
			{
				GuardedRegion reg = from.Region as GuardedRegion;

				if ( reg == null )
				{
					from.SendMessage( "You are not in a guardable region." );
				}
				else
				{
					reg.Allowed = !e.GetBoolean( 0 );

					if ( reg.Allowed )
						from.SendMessage( "The guards in this region do not attack reds." );
					else
						from.SendMessage( "The guards in this region now attack reds." );
				}
			}
			else
			{
				from.SendMessage( "Format: SetAllowReds <true|false>" );
			}
		}

		[Usage( "CheckReds" )]
		[Description( "Returns a value indicating if the current region is allows reds (murderes) or not." )]
		private static void CheckReds_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			GuardedRegion reg = from.Region as GuardedRegion;

			if ( reg == null )
				from.SendMessage( "You are not in a guardable region." );
			else if (reg.Allowed)
				from.SendMessage( "This region does allow Reds (Murderers)." );
			else
				from.SendMessage( "This region does NOT allow Reds (Murderers)." );
		}
// Aditions by 2.0 Shaka큦 GS Killable Guards ends here!!!!

		public static GuardedRegion Disable( GuardedRegion reg )
		{
			reg.Disabled = true;
			return reg;
		}

		public virtual bool AllowReds{ get{ return m_AllowReds; } } // Modded by 2.0 Shaka큦 GS Killable Guards


		public virtual bool CheckVendorAccess( BaseVendor vendor, Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster || IsDisabled() )
				return true;

		return ( from.Kills < 5 || Allowed ); // Modded by 2.0 Shaka큦 GS Killable Guards
		}

		public virtual Type DefaultGuardType
		{
			get
			{
				if ( this.Map == Map.Ilshenar || this.Map == Map.Malas )
					return typeof( ArcherGuard );
				else
					return typeof( WarriorGuard );
			}
		}

		public GuardedRegion( string name, Map map, int priority, params Rectangle3D[] area ) : base( name, map, priority, area )
		{
			m_GuardType = DefaultGuardType;
		}

		public GuardedRegion( string name, Map map, int priority, params Rectangle2D[] area )
			: base( name, map, priority, area )
		{
			m_GuardType = DefaultGuardType;
		}
		
		public GuardedRegion( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
			XmlElement el = xml["guards"];

			if ( ReadType( el, "type", ref m_GuardType, false ) )
			{
				if ( !typeof( Mobile ).IsAssignableFrom( m_GuardType ) )
				{
					Console.WriteLine( "Invalid guard type for region '{0}'", this );
					m_GuardType = DefaultGuardType;
				}
			}
			else
			{
				m_GuardType = DefaultGuardType;
			}

			bool disabled = false;
			if ( ReadBoolean( el, "disabled", ref disabled, false ) )
				this.Disabled = disabled;
		}

		public override bool OnBeginSpellCast( Mobile m, ISpell s )
		{
			if ( !IsDisabled() && !s.OnCastInTown( this ) )
			{
				m.SendLocalizedMessage( 500946 ); // You cannot cast this in town!
				return false;
			}

			return base.OnBeginSpellCast( m, s );
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}
// Adition by 2.0 Shaka큦 GS Killable Guards starts here
    	public static Type RandomGuard( Type guard, bool random )
		{
			Type rndG;
			if ( !random )
				rndG = guard;
			else
			{
				switch ( Utility.Random( 8 ) ) // Total Number of guard types
				{
					case 0:
					rndG = typeof( WarriorGuard );
					break;
					case 1:
					rndG = typeof( ArcherGuard );
					break;
					case 2:
					rndG = typeof( MageGuard );
					break;
					case 3:
					rndG = typeof( PaladinGuard );
					break;
					case 4:
					rndG = typeof( GolemGuard );
					break;
					case 5:
					rndG = typeof( LizardGuard );
					break;
					case 6:
					rndG = typeof( ShadowGuard );
					break;
					default: // just in case it falls through
					rndG = typeof( WarriorGuard );
					break;
				}
			}
			return rndG;
		}
// Aditions by 2.0 Shaka큦 GS Killable Guards ends here!!!!

		public override void MakeGuard( Mobile focus )
		{
			BaseGuard useGuard = null;

			foreach ( Mobile m in focus.GetMobilesInRange( 8 ) )
			{
				if ( m is BaseGuard )
				{
					BaseGuard g = (BaseGuard)m;

					if ( g.Focus == null ) // idling
					{
						useGuard = g;
						break;
					}
				}
			}

			if ( useGuard == null )
			{
				m_GuardParams[0] = focus;

			    BaseGuard m_Guard = (BaseGuard)Activator.CreateInstance( GuardedRegion.RandomGuard( m_GuardType, UseRandom ), m_GuardParams ); // Added by 2.0 Shaka큦 GS Killable Guards
				m_Guard.BackUP = false; // Added by 2.0 Shaka큦 GS Killable Guards
				m_Guard.Combatant = focus; // Added by 2.0 Shaka큦 GS Killable Guards
			}
			else
			{
				useGuard.Focus = focus;
                useGuard.Combatant = focus; // Added by 2.0 Shaka큦 GS Killable Guards
            }
		}

		public override void OnEnter( Mobile m )
		{
			if ( IsDisabled() )
				return;
// Adition by 2.0 Shaka큦 GS Killable Guards starts here	
				
			BaseCreature c = m as BaseCreature;

			if ( c != null && ( !Allowed && ( c.Kills >= 5 || c.AlwaysMurderer ) || ( !c.Body.IsHuman && !c.IsBodyMod && c.IsAggressive ) ) )
				CheckGuardCandidate( c );

// Adition by 2.0 Shaka큦 GS Killable Guards ends here

			if ( m != null && !Allowed && m.Kills >= 5 ) // Modded by 2.0 Shaka큦 GS Killable Guards
				CheckGuardCandidate( m );
		}

		public override void OnExit( Mobile m )
		{
			if ( IsDisabled() )
				return;
		}

		public override void OnSpeech( SpeechEventArgs args )
		{
			base.OnSpeech( args );

			if ( IsDisabled() )
				return;

			
			if (args.Mobile != null)
			{
				Mobile m = args.Mobile;

				// Adition by 2.0 Shaka큦 GS Killable Guards starts here
				BaseGuard guard = null;
				foreach ( Mobile g in Map.GetMobilesInRange( m.Location, 14 ) )
				{
					if ( g != null && g is BaseGuard )
					{
						//Console.WriteLine("Do we have a guard in range who is around?");
						guard = g as BaseGuard;
						break;
					}
				}

				if (  m.Alive && args.HasKeyword( 0x0007 ) ) // *guards*
				{
					args.Handled = true;
					if ( guard != null )
					{
						if ( ( guard.Focus == m.Combatant || guard.Combatant == m.Combatant ) )
							m.SendMessage("A Guard is already moving to assist");
						if ( guard.Combatant != null && guard.Combatant != m.Combatant )
							m.SendMessage("A Guard is already in range but is busy, You better keep running!");
					}
					else
						CallGuards( m.Location );
				}
			}
			// Adition by 2.0 Shaka큦 GS Killable Guards ends here
		}
		

		public override void OnAggressed( Mobile aggressor, Mobile aggressed, bool criminal )
		{
            // Adition by 2.0 Shaka큦 GS Killable Guards starts here
			if (aggressor is BaseGuard)
				return;

			if( (aggressed is BaseGuard) && aggressor != aggressed)
			{

				BaseGuard guard = null;
				foreach ( Mobile g in Map.GetMobilesInRange( aggressed.Location, 14 ) )
				{
					if ( g != null &&( g is BaseGuard && g.Combatant == null ) )
					{
						guard = g as BaseGuard;
						break;
					}
				}

				BaseGuard def = aggressed as BaseGuard;
				def.Focus = def.Combatant = aggressor;

				if (guard != null && guard != aggressor)
					guard.Focus = guard.Combatant = aggressor;
				//else
				//def.Say("I'll get to you in a moment!");
			}
			
			if ( !IsDisabled() && aggressor != aggressed && criminal )
				CheckGuardCandidate( aggressor );

			base.OnAggressed( aggressor, aggressed, criminal );

            // Adition by 2.0 Shaka큦 GS Killable Guards ends here

			if ( !IsDisabled() && aggressor != aggressed && criminal )
				CheckGuardCandidate( aggressor );
		}

		public override void OnGotBeneficialAction( Mobile helper, Mobile helped )
		{
			base.OnGotBeneficialAction( helper, helped );

			if ( IsDisabled() )
				return;

			int noto = Notoriety.Compute( helper, helped );

			if ( helper != helped && (noto == Notoriety.Criminal || noto == Notoriety.Murderer) )
				CheckGuardCandidate( helper );
		}

		public override void OnCriminalAction( Mobile m, bool message )
		{
			base.OnCriminalAction( m, message );

			if ( !IsDisabled() )
				CheckGuardCandidate( m );
		}

		private Dictionary<Mobile, GuardTimer> m_GuardCandidates = new Dictionary<Mobile, GuardTimer>();

		public void CheckGuardCandidate( Mobile m )
		{
			if ( IsDisabled() )
				return;

			if ( IsGuardCandidate( m ) )
			{
				GuardTimer timer = null;
				m_GuardCandidates.TryGetValue( m, out timer );

				if ( timer == null )
				{
					timer = new GuardTimer( m, m_GuardCandidates );
					timer.Start();

					m_GuardCandidates[m] = timer;
					m.SendLocalizedMessage( 502275 ); // Guards can now be called on you!

					Map map = m.Map;
		            BaseGuard guard = null; // Greystar -- Added for killable guards
					//double prio = 0.0; // Added by 2.0 Shaka큦 GS Killable Guards

            // Adition by 2.0 Shaka큦 GS Killable Guards starts here
					foreach ( Mobile g in Map.GetMobilesInRange( m.Location, 24 ) )
					{
						if ( g is BaseGuard  )



						{
							//Console.WriteLine("Do we have a guard in range who is around?");
							if ( g != null )
								guard = g as BaseGuard;
							break;
						}
					}
            // Adition by 2.0 Shaka큦 GS Killable Guards ends here
      

					if ( map != null )
					{
						Mobile fakeCall = null;
						double prio = 0.0;

						foreach ( Mobile v in m.GetMobilesInRange( 8 ) )
						{
							if( !v.Player && v != m  && !IsGuardCandidate( v ) && ((v is BaseCreature)? ((BaseCreature)v).IsHumanInTown() : (v.Body.IsHuman && v.Region.IsPartOf( this ))) )
							{
								double dist = m.GetDistanceToSqrt( v );

								if ( fakeCall == null || dist < prio )
								{
									fakeCall = v;
									prio = dist;
								}
							}
						}

                        // Adition by 2.0 Shaka큦 GS Killable Guards starts here

						if ( fakeCall != null )
						{

							if ( guard == null )
							{
								if (fakeCall is BaseVendor)
									fakeCall.Say( Utility.RandomList( 1007037, 501603, 1013037, 1013038, 1013039, 1013041, 1013042, 1013043, 1013052 ) );
								CallGuards( m.Location );
							}
							else if ( guard.Combatant == null )
							{
								guard.Focus = m;
								guard.Combatant = m;
								m_GuardCandidates.Remove( m );
								m.SendLocalizedMessage( 502276 ); // Guards can no longer be called on you.
							}

						// Adition by 2.0 Shaka큦 GS Killable Guards ends here

						}
					}
					else
					{
						timer.Stop();
						timer.Start();
					}
				}
			}
		}
                    // Adition by 2.0 Shaka큦 GS Killable Guards starts here

		public void CallGuards( Point3D p )
		{
			if ( IsDisabled() )
				return;

			BaseGuard guard = null;

			foreach ( Mobile g in Map.GetMobilesInRange( p, 24 ) )
			{
				if ( g is BaseGuard )
				{
					//Console.WriteLine("Do we have a guard in range?");
					guard = g as BaseGuard;
					break;
				}
			}

			foreach ( Mobile mob in Map.GetMobilesInRange( p, 8 ) )
			{
				if ( IsGuardCandidate( mob ) && ((!AllowReds && mob.Kills >= 5 /*&& Mobiles.Contains( mob )*/) || m_GuardCandidates.ContainsKey( mob )) )
				{
					if ( guard == null || guard.Combatant != mob )
					{
						//Console.WriteLine("No guard in range then create one.");
						//MakeGuard( mob );
						m_GuardCandidates.Remove( mob );
						mob.SendLocalizedMessage( 502276 ); // Guards can no longer be called on you.
					}
					else if ( guard.Focus == null && guard.Combatant == null )
					{
						//Console.WriteLine("Guard in range make it have a new target.");
						//Point3D from = guard.Location;
						//Point3D to = mob.Location;
						guard.Focus = mob;
						guard.Combatant = mob;
						m_GuardCandidates.Remove( mob );
						mob.SendLocalizedMessage( 502276 ); // Guards can no longer be called on you.
						//guard.Location = to;

						//Effects.SendLocationParticles( EffectItem.Create( from, guard.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
						//Effects.SendLocationParticles( EffectItem.Create( to, guard.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

						//guard.PlaySound( 0x1FE );
					}
					else if ( guard.Focus == mob && guard.Combatant == null )
						guard.Combatant = mob;
					else if ( guard.Focus == null && guard.Combatant == mob )
						guard.Focus = mob;

					break;
				}
			}
		}                 // Adition by 2.0 Shaka큦 GS Killable Guards ends here


		public bool IsGuardCandidate( Mobile m )
		{
			if ( m is BaseGuard || !m.Alive || m.AccessLevel > AccessLevel.Player || m.Blessed || IsDisabled() )
				return false;
         
            // Adition by 2.0 Shaka큦 GS Killable Guards starts here
          
			BaseCreature c = m as BaseCreature;
			if ( c != null )
			{
				//Greystar -- This section further modded to help prevent guards from being called on pets and summone creatures
				PlayerMobile pc = c.ControlMaster as PlayerMobile;
				PlayerMobile ps = c.SummonMaster as PlayerMobile;

				if (c is BaseMount)
				{
					BaseMount bm = c as BaseMount;
					if (bm !=null)
						if ( !c.Body.IsHuman && c.IsPetSum && bm.IsMount && ( pc!=null && !pc.Criminal) )
							return false;
				}

				if ( pc != null )
				{
					if ( (!c.Body.IsHuman && c.IsPetSum && ( pc.Kills >= 5 && AllowReds ) || pc.Kills < 5 ) && ( !c.Body.IsHuman && c.IsPetSum && !pc.Criminal ) )
						return false;
					if ( ( !c.Body.IsHuman && c.IsPetSum && ( pc.Kills >= 5 && !AllowReds ) ) || ( !c.Body.IsHuman && c.IsPetSum && pc.Criminal ) )
						return true;
				}

				if ( ps != null )
				{
					if ( ( !c.Body.IsHuman && c.IsPetSum && ( ps.Kills >= 5 && AllowReds ) || ps.Kills < 5 ) && ( !c.Body.IsHuman && c.IsPetSum && !ps.Criminal ) )
						return false;
					if ( ( !c.Body.IsHuman && c.IsPetSum && ( ps.Kills >= 5 && !AllowReds ) ) || ( !c.Body.IsHuman && c.IsPetSum && ps.Criminal ) )
						return true;
				}

				if ( !c.Body.IsHuman && !c.IsBodyMod && c.IsAggressive )
					return true;
				//Console.WriteLine("Is Creature a target for the guards?");

				if ( c.Criminal || ( !AllowReds && ( c.Kills >= 5 || c.AlwaysMurderer ) ) )
					return true;

            // Adition by 2.0 Shaka큦 GS Killable Guards ends here
			}

			return (m.Criminal || (!AllowReds && m.Kills >= 5));
		}


		private class GuardTimer : Timer
		{
			private Mobile m_Mobile;
			private Dictionary<Mobile, GuardTimer> m_Table;

			public GuardTimer( Mobile m, Dictionary<Mobile, GuardTimer> table ) : base( TimeSpan.FromSeconds( 15.0 ) )
			{
				Priority = TimerPriority.TwoFiftyMS;

				m_Mobile = m;
				m_Table = table;
			}

			protected override void OnTick()
			{
				if ( m_Table.ContainsKey( m_Mobile ) )
				{
					m_Table.Remove( m_Mobile );
					m_Mobile.SendLocalizedMessage( 502276 ); // Guards can no longer be called on you.
				}
			}
		}
	}
}