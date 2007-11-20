using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Multis;
using Server.Spells;
using Server.Spells.Sixth;
using Server.Guilds;
using Server.Gumps;
/*** ADD_START ***/
using Server.Targeting;
/*** ADD_END ***/


namespace Server.Regions
{
	public class HouseRegion : BaseRegion
	{
		public static readonly int HousePriority = Region.DefaultPriority + 1;

		private BaseHouse m_House;

		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( OnLogin );
		}

		public static void OnLogin( LoginEventArgs e )
		{
			BaseHouse house = BaseHouse.FindHouseAt( e.Mobile );

			if ( house != null && !house.Public && !house.IsFriend( e.Mobile ) )
				e.Mobile.Location = house.BanLocation;
		}

		public HouseRegion( BaseHouse house ) : base( null, house.Map, HousePriority, GetArea( house ) )
		{
			m_House = house;

			Point3D ban = house.RelativeBanLocation;

			this.GoLocation = new Point3D( house.X + ban.X, house.Y + ban.Y, house.Z + ban.Z );
		}

		private static Rectangle3D[] GetArea( BaseHouse house )
		{
			int x = house.X;
			int y = house.Y;
			int z = house.Z;

			Rectangle2D[] houseArea = house.Area;
			Rectangle3D[] area = new Rectangle3D[houseArea.Length];

			for ( int i = 0; i < area.Length; i++ )
			{
				Rectangle2D rect = houseArea[i];
				area[i] = Region.ConvertTo3D( new Rectangle2D( x + rect.Start.X, y + rect.Start.Y, rect.Width, rect.Height ) );
			}

			return area;
		}

		public override bool SendInaccessibleMessage( Item item, Mobile from )
		{
			if ( item is Container )
				item.SendLocalizedMessageTo( from, 501647 ); // That is secure.
			else
				item.SendLocalizedMessageTo( from, 1061637 ); // You are not allowed to access this.

			return true;
		}

		public override bool CheckAccessibility( Item item, Mobile from )
		{
			return m_House.CheckAccessibility( item, from );
		}

		private bool m_Recursion;

		// Use OnLocationChanged instead of OnEnter because it can be that we enter a house region even though we're not actually inside the house
		public override void OnLocationChanged( Mobile m, Point3D oldLocation )
		{
			if ( m_Recursion )
				return;

			base.OnLocationChanged( m, oldLocation );

			m_Recursion = true;

			if ( m is BaseCreature && ((BaseCreature)m).NoHouseRestrictions )
			{
			}
			else if ( m is BaseCreature && ((BaseCreature)m).IsHouseSummonable && (BaseCreature.Summoning || m_House.IsInside( oldLocation, 16 )) )
			{
			}
			else if ( (m_House.Public || !m_House.IsAosRules) && m_House.IsBanned( m ) && m_House.IsInside( m ) )
			{
				m.Location = m_House.BanLocation;

				if( !Core.SE )
					m.SendLocalizedMessage( 501284 ); // You may not enter.
			}
			else if ( m_House.IsAosRules && !m_House.Public && !m_House.HasAccess( m ) && m_House.IsInside( m ) )
			{
				m.Location = m_House.BanLocation;

				if( !Core.SE )
					m.SendLocalizedMessage( 501284 ); // You may not enter.
			}
			else if ( m_House.IsCombatRestricted( m ) && m_House.IsInside( m ) && !m_House.IsInside( oldLocation, 16 ) )
			{
				m.Location = m_House.BanLocation;
				m.SendLocalizedMessage( 1061637 ); // You are not allowed to access this.
			}
			else if ( m_House is HouseFoundation )
			{
				HouseFoundation foundation = (HouseFoundation)m_House;

				if ( foundation.Customizer != null && foundation.Customizer != m && m_House.IsInside( m ) )
					m.Location = m_House.BanLocation;
			}

			if ( m_House.InternalizedVendors.Count > 0 && m_House.IsInside( m ) && !m_House.IsInside( oldLocation, 16 ) && m_House.IsOwner( m ) && m.Alive && !m.HasGump( typeof( NoticeGump ) ) )
			{
				/* This house has been customized recently, and vendors that work out of this
				 * house have been temporarily relocated.  You must now put your vendors back to work.
				 * To do this, walk to a location inside the house where you wish to station
				 * your vendor, then activate the context-sensitive menu on your avatar and
				 * select "Get Vendor".
				 */
				m.SendGump( new NoticeGump( 1060635, 30720, 1061826, 32512, 320, 180, null, null ) );
			}

			m_Recursion = false;
		}

		public override bool OnMoveInto( Mobile from, Direction d, Point3D newLocation, Point3D oldLocation )
		{
			if ( !base.OnMoveInto( from, d, newLocation, oldLocation ) )
				return false;

			if ( from is BaseCreature && ((BaseCreature)from).NoHouseRestrictions )
			{
			}
			else if ( from is BaseCreature && ((BaseCreature)from).IsHouseSummonable && (BaseCreature.Summoning || m_House.IsInside( oldLocation, 16 )) )
			{
			}
			else if ( (m_House.Public || !m_House.IsAosRules) && m_House.IsBanned( from ) && m_House.IsInside( newLocation, 16 ) )
			{
				from.Location = m_House.BanLocation;

				if( !Core.SE )
					from.SendLocalizedMessage( 501284 ); // You may not enter.

				return false;
			}
			else if ( m_House.IsAosRules && !m_House.Public && !m_House.HasAccess( from ) && m_House.IsInside( newLocation, 16 ) )
			{
				if( !Core.SE )
					from.SendLocalizedMessage( 501284 ); // You may not enter.

				return false;
			}
			else if ( m_House.IsCombatRestricted( from ) && !m_House.IsInside( oldLocation, 16 ) && m_House.IsInside( newLocation, 16 ) )
			{
				from.SendLocalizedMessage( 1061637 ); // You are not allowed to access this.
				return false;
			}
			else if ( m_House is HouseFoundation )
			{
				HouseFoundation foundation = (HouseFoundation)m_House;

				if ( foundation.Customizer != null && foundation.Customizer != from && m_House.IsInside( newLocation, 16 ) )
					return false;
			}

			if ( m_House.InternalizedVendors.Count > 0 && m_House.IsInside( from ) && !m_House.IsInside( oldLocation, 16 ) && m_House.IsOwner( from ) && from.Alive && !from.HasGump( typeof( NoticeGump ) ) )
			{
				/* This house has been customized recently, and vendors that work out of this
				 * house have been temporarily relocated.  You must now put your vendors back to work.
				 * To do this, walk to a location inside the house where you wish to station
				 * your vendor, then activate the context-sensitive menu on your avatar and
				 * select "Get Vendor".
				 */
				from.SendGump( new NoticeGump( 1060635, 30720, 1061826, 32512, 320, 180, null, null ) );
			}

			return true;
		}

		public override bool OnDecay( Item item )
		{
			if ( (m_House.IsLockedDown( item ) || m_House.IsSecure( item )) && m_House.IsInside( item ) )
				return false;
			else
				return base.OnDecay(item );
		}

		public static TimeSpan CombatHeatDelay = TimeSpan.FromSeconds( 30.0 );

		public override TimeSpan GetLogoutDelay( Mobile m )
		{
			if ( m_House.IsFriend( m ) && m_House.IsInside( m ) )
			{
				for ( int i = 0; i < m.Aggressed.Count; ++i )
				{
					AggressorInfo info = m.Aggressed[i];

					if ( info.Defender.Player && (DateTime.Now - info.LastCombatTime) < CombatHeatDelay )
						return base.GetLogoutDelay( m );
				}

				return TimeSpan.Zero;
			}

			return base.GetLogoutDelay( m );
		}

		public override void OnSpeech( SpeechEventArgs e )
		{
			base.OnSpeech( e );
           
			Mobile from = e.Mobile;

			if ( !from.Alive || !m_House.IsInside( from ) || !m_House.IsActive )
				return;

			bool isOwner = m_House.IsOwner( from );
			bool isCoOwner = isOwner || m_House.IsCoOwner( from );
			bool isFriend = isCoOwner || m_House.IsFriend( from );

            /*** ADD_START ***/
            if (e.Speech.ToLower() == "i wish to steal this")
            {
                if (!(from.Skills[SkillName.Stealing].Base >= 100))
                {
                    from.SendAsciiMessage("You don't have enough skill in stealing.");
                    return;
                }
                if (!(from.Skills[SkillName.Lockpicking].Base >= 100))
                {
                    from.SendAsciiMessage("You don't have enough skill in Lockpicking.");
                    return;
                }
                if (!(from.Dex >= 100))
                {
                    from.SendAsciiMessage("You don't have enough Dexterity.");
                    return;
                }

                from.SendAsciiMessage("Select a lockpick");
                
                Item item = from.Backpack.FindItemByType(typeof(Server.Items.Lockpick), true);

                if(item != null)
                    from.Target = new StealedObjectTarget((Lockpick)item, m_House);
                else
                    from.SendAsciiMessage("You don't have enough Lockpicks.");
            }
            /*** ADD_END ***/

			if ( !isFriend )
				return;

			if ( e.HasKeyword( 0x33 ) ) // remove thyself
			{
				if ( isFriend )
				{
					from.SendLocalizedMessage( 501326 ); // Target the individual to eject from this house.
					from.Target = new HouseKickTarget( m_House );
				}
				else
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
			}
			else if ( e.HasKeyword( 0x34 ) ) // I ban thee
			{
				if ( !isFriend )
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
				else if ( !m_House.Public && m_House.IsAosRules )
				{
					from.SendLocalizedMessage( 1062521 ); // You cannot ban someone from a private house.  Revoke their access instead.
				}
				else
				{
					from.SendLocalizedMessage( 501325 ); // Target the individual to ban from this house.
					from.Target = new HouseBanTarget( true, m_House );
				}
			}
			else if ( e.HasKeyword( 0x23 ) ) // I wish to lock this down
			{
				if ( isCoOwner )
				{
					from.SendLocalizedMessage( 502097 ); // Lock what down?
					from.Target = new LockdownTarget( false, m_House );
				}
				else if ( isFriend )
				{
					from.SendLocalizedMessage( 1010587 ); // You are not a co-owner of this house.
				}
				else
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
			}
			else if ( e.HasKeyword( 0x24 ) ) // I wish to release this
			{
				if ( isCoOwner )
				{
					from.SendLocalizedMessage( 502100 ); // Choose the item you wish to release
					from.Target = new LockdownTarget( true, m_House );
				}
				else if ( isFriend )
				{
					from.SendLocalizedMessage( 1010587 ); // You are not a co-owner of this house.
				}
				else
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
			}
			else if ( e.HasKeyword( 0x25 ) ) // I wish to secure this
			{
				if ( isOwner )
				{
					from.SendLocalizedMessage( 502103 ); // Choose the item you wish to secure
					from.Target = new  SecureTarget( false, m_House );
				}
				else
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
			}
			else if ( e.HasKeyword( 0x26 ) ) // I wish to unsecure this
			{
				if ( isOwner )
				{
					from.SendLocalizedMessage( 502106 ); // Choose the item you wish to unsecure
					from.Target = new  SecureTarget( true, m_House );
				}
				else
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
			}
			else if ( e.HasKeyword( 0x27 ) ) // I wish to place a strongbox
			{
				if ( isOwner )
				{
					from.SendLocalizedMessage( 502109 ); // Owners do not get a strongbox of their own.
				}
				else if ( isCoOwner )
				{
					m_House.AddStrongBox( from );
				}
				else if ( isFriend )
				{
					from.SendLocalizedMessage( 1010587 ); // You are not a co-owner of this house.
				}
				else
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
			}
			else if ( e.HasKeyword( 0x28 ) )
			{
				if ( isCoOwner )
				{
					m_House.AddTrashBarrel( from );
				}
				else if ( isFriend )
				{
					from.SendLocalizedMessage( 1010587 ); // You are not a co-owner of this house.
				}
				else
				{
					from.SendLocalizedMessage( 502094 ); // You must be in your house to do this.
				}
			}            
		}        
        
		public override bool OnDoubleClick( Mobile from, object o )
		{
			if ( o is Container )
			{
				Container c = (Container)o;

				SecureAccessResult res = m_House.CheckSecureAccess( from, c );

				switch ( res )
				{
					case SecureAccessResult.Insecure: break;
					case SecureAccessResult.Accessible: return true;
					case SecureAccessResult.Inaccessible: c.SendLocalizedMessageTo( from, 1010563 ); return false;
				}
			}

			return base.OnDoubleClick( from, o );
		}

		public override bool OnSingleClick( Mobile from, object o )
		{
			if ( o is Item )
			{
				Item item = (Item)o;

				if ( m_House.IsLockedDown( item ) )
					item.LabelTo( from, 501643 ); // [locked down]
				else if ( m_House.IsSecure( item ) )
					item.LabelTo( from, 501644 ); // [locked down & secure]
			}

			return base.OnSingleClick( from, o );
		}

		public BaseHouse House
		{
			get
			{
				return m_House;
			}
		}
	}

    /*** ADD_START ***/
    public class StealedObjectTarget : Target
    {
        private Lockpick m_lockpick;
        private BaseHouse m_house;

        public StealedObjectTarget(Lockpick lockpick, BaseHouse house)
            : base(1, false, TargetFlags.None)
        {
            m_lockpick = lockpick;
            m_house = house;
        }

        protected override void OnTargetNotAccessible(Mobile from, object targeted)
        {
            if (m_lockpick.Deleted)
                return;

            Item item = (Item)targeted;
            from.Direction = from.GetDirectionTo(item);

            if (m_house.IsLockedDown(item)) // locked down                
            {
                new StealingTimer(from, item, m_lockpick, 1, m_house).Start();              
            }
            else if (item is Container)
            {
                new StealingTimer(from, item, m_lockpick, 0, m_house).Start();
            }           
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            from.SendAsciiMessage("The item is already stealable!");
        }
    }
    /*** ADD_END ***/

    /*** ADD_START ***/
    public class StealingTimer : Timer
    {
        private Mobile m_From;
        private Item m_Item;
        private Lockpick m_Lockpick;
        private int m_ToDo;
        private BaseHouse m_house;

        //se ToDO = 1 unlock ToDo = 0 unsecure
        public StealingTimer(Mobile from, Item item, Lockpick lockpick, int ToDo, BaseHouse house)
            : base(TimeSpan.FromSeconds(3.0))
        {
            m_From = from;
            m_Item = item;
            m_Lockpick = lockpick;
            m_ToDo = ToDo;
            m_house = house;
            from.PlaySound(0x241); //suono lockpick
            Priority = TimerPriority.TwoFiftyMS;
        }

        protected void BrokeLockPickTest()
        {
            // When failed, a 25% chance to break the lockpick
            if (Utility.Random(4) == 0)
            {
                // You broke the lockpick.
                m_Item.SendLocalizedMessageTo(m_From, 502074);

                m_From.PlaySound(0x3A4);
                m_Lockpick.Consume();
            }
        }

        protected override void OnTick()
        {
            if (!m_From.InRange(m_Item.GetWorldLocation(), 1) || m_Lockpick.Deleted)
                return;
            
            if (Utility.Random(10) == 5) //probabilit� del 10%
            {
                if (m_ToDo == 0)
                {
                    m_house.StealSecure(m_From, m_Item); //unsecure
                }
                else
                {
                    m_house.StealLockedObj(m_From, m_Item); //unlock
                }
                m_From.PlaySound(0x4A);// Success! Pick the lock!
            }
            else
            {
                // The player failed to pick the lock
                m_Item.SendLocalizedMessageTo(m_From, 502075);// You are unable to pick the lock.
                BrokeLockPickTest();
            }
        }
    }
    /*** ADD_END ***/
}