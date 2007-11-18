/**************************************
*    Killable Guards (GS Versions)    *
*            Version: 3.0             *
*                                     *   
*      Distro files: BaseMount.cs     *
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
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	public abstract class BaseMount : BaseCreature, IMount
	{
        // Addition by 2.0 Shaka�s GS Killable Guards starts here -- added to make it possible for guardedregion.cs to check to make sure a mount is already owned
        
        private bool m_IsMount;
        public bool IsMount
        {
            get { return m_IsMount; }
            set { m_IsMount = value; InvalidateProperties(); }
        }
        
        // Addition by 2.0 Shaka�s GS Killable Guards ends here

        private Mobile m_Rider;
		private Item m_InternalItem;
		private DateTime m_NextMountAbility;

		public virtual TimeSpan MountAbilityDelay { get { return TimeSpan.Zero; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextMountAbility
		{
			get { return m_NextMountAbility; }
			set { m_NextMountAbility = value; }
		}

		protected Item InternalItem { get { return m_InternalItem; } }

		public virtual bool AllowMaleRider{ get{ return true; } }
		public virtual bool AllowFemaleRider{ get{ return true; } }

		public BaseMount( string name, int bodyID, int itemID, AIType aiType, FightMode fightMode, int rangePerception, int rangeFight, double activeSpeed, double passiveSpeed ) : base ( aiType, fightMode, rangePerception, rangeFight, activeSpeed, passiveSpeed )
		{
			Name = name;
			Body = bodyID;

			m_InternalItem = new MountItem( this, itemID );
		}

		public BaseMount( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

            writer.Write((int)2); // version // Modded by 2.0 Shaka�s GS Killable Guards - Version increased to save new variable

            writer.Write((bool)m_IsMount); // Added by 2.0 Shaka�s GS Killable Guards - New Variable that is checked on in GuardedRegion.cs
			writer.Write( m_NextMountAbility );
			writer.Write( m_Rider );
			writer.Write( m_InternalItem );
		}

		[Hue, CommandProperty( AccessLevel.GameMaster )]
		public override int Hue
		{
			get
			{
				return base.Hue;
			}
			set
			{
				base.Hue = value;

				if ( m_InternalItem != null )
					m_InternalItem.Hue = value;
			}
		}

		public override bool OnBeforeDeath()
		{
			Rider = null;

			return base.OnBeforeDeath();
		}

		public override void OnAfterDelete()
		{
			if ( m_InternalItem != null )
				m_InternalItem.Delete();

			m_InternalItem = null;

			base.OnAfterDelete();
		}

		public override void OnDelete()
		{
			Rider = null;

			base.OnDelete();
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{

                // Adition by 2.0 Shaka�s GS Killable Guards starts here

                case 2:
                    {
                        m_IsMount = reader.ReadBool(); //Same New Variable for checking GuardedRegion.cs
                        goto case 1;
                    }

                // Adition by 2.0 Shaka�s GS Killable Guards starts here

				case 1:
				{
					m_NextMountAbility = reader.ReadDateTime();
					goto case 0;
				}
				case 0:
				{
					m_Rider = reader.ReadMobile();
					m_InternalItem = reader.ReadItem();

					if ( m_InternalItem == null )
						Delete();

					break;
				}
			}
		}

		public virtual void OnDisallowedRider( Mobile m )
		{
			m.SendMessage( "You may not ride this creature." );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsDeadPet )
				return;

			if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				if ( Core.AOS ) // You cannot ride a mount in your current form.
					PrivateOverheadMessage( Network.MessageType.Regular, 0x3B2, 1062061, from.NetState );
				else
					from.SendLocalizedMessage( 1061628 ); // You can't do that while polymorphed.

				return;
			}

			if ( !CheckMountAllowed( from, true ) )
				return;

			if ( from.Mounted )
			{
				from.SendLocalizedMessage( 1005583 ); // Please dismount first.
				return;
			}

			if ( from.Female ? !AllowFemaleRider : !AllowMaleRider )
			{
				OnDisallowedRider( from );
				return;
			}

			if ( !Multis.DesignContext.Check( from ) )
				return;

			if ( from.HasTrade )
			{
				from.SendLocalizedMessage( 1042317, "", 0x41 ); // You may not ride at this time
				return;
			}

			if ( from.InRange( this, 1 ) )
			{
				bool canAccess = ( from.AccessLevel >= AccessLevel.GameMaster )
					|| ( Controlled && ControlMaster == from )
					|| ( Summoned && SummonMaster == from );

				if ( canAccess )
				{
					if ( this.Poisoned )
						PrivateOverheadMessage( Network.MessageType.Regular, 0x3B2, 1049692, from.NetState ); // This mount is too ill to ride.
					else
                    { // Adition by 2.0 Shaka�s GS Killable Guards starts here
                        Rider = from; // <-- was originally right after the else
                        m_IsMount = true; // Shaka�s GS Killable Guards for Protection of Mounts from guards
                    } // Adition by 2.0 Shaka�s GS Killable Guards ends here
				}
				else if ( !Controlled && !Summoned )
				{
					// That mount does not look broken! You would have to tame it to ride it.
					PrivateOverheadMessage( Network.MessageType.Regular, 0x3B2, 501263, from.NetState );
				}
				else
				{
					// This isn't your mount; it refuses to let you ride.
					PrivateOverheadMessage( Network.MessageType.Regular, 0x3B2, 501264, from.NetState );
				}
			}
			else
			{
				from.SendLocalizedMessage( 500206 ); // That is too far away to ride.
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ItemID
		{
			get
			{
				if ( m_InternalItem != null )
					return m_InternalItem.ItemID;
				else
					return 0;
			}
			set
			{
				if ( m_InternalItem != null )
					m_InternalItem.ItemID = value;
			}
		}

		public static void Dismount( Mobile m )
		{
			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Rider
		{
			get
			{
				return m_Rider;
			}
			set
			{
				if ( m_Rider != value )
				{
					if ( value == null )
					{
						Point3D loc = m_Rider.Location;
						Map map = m_Rider.Map;

						if ( map == null || map == Map.Internal )
						{
							loc = m_Rider.LogoutLocation;
							map = m_Rider.LogoutMap;
						}

						Direction = m_Rider.Direction;
						Location = loc;
						Map = map;

						if ( m_InternalItem != null )
							m_InternalItem.Internalize();
					}
					else
					{
						if ( m_Rider != null )
							Dismount( m_Rider );

						Dismount( value );

						if ( m_InternalItem != null )
							value.AddItem( m_InternalItem );

						value.Direction = this.Direction;

						Internalize();
					}

					m_Rider = value;
				}
			}
		}

		private class BlockEntry
		{
			public BlockMountType m_Type;
			public DateTime m_Expiration;

			public bool IsExpired{ get{ return ( DateTime.Now >= m_Expiration ); } }

			public BlockEntry( BlockMountType type, DateTime expiration )
			{
				m_Type = type;
				m_Expiration = expiration;
			}
		}

		private static Hashtable m_Table = new Hashtable();

		public static void SetMountPrevention( Mobile mob, BlockMountType type, TimeSpan duration )
		{
			if ( mob == null )
				return;

			DateTime expiration = DateTime.Now + duration;

			BlockEntry entry = m_Table[mob] as BlockEntry;

			if ( entry != null )
			{
				entry.m_Type = type;
				entry.m_Expiration = expiration;
			}
			else
			{
				m_Table[mob] = entry = new BlockEntry( type, expiration );
			}
		}

		public static void ClearMountPrevention( Mobile mob )
		{
			if ( mob != null )
				m_Table.Remove( mob );
		}

		public static BlockMountType GetMountPrevention( Mobile mob )
		{
			if ( mob == null )
				return BlockMountType.None;

			BlockEntry entry = m_Table[mob] as BlockEntry;

			if ( entry == null )
				return BlockMountType.None;

			if ( entry.IsExpired )
			{
				m_Table.Remove( mob );
				return BlockMountType.None;
			}

			return entry.m_Type;
		}

		public static bool CheckMountAllowed( Mobile mob, bool message )
		{
			BlockMountType type = GetMountPrevention( mob );

			if ( type == BlockMountType.None )
				return true;

			if ( message )
			{
				switch ( type )
				{
					case BlockMountType.Dazed:
					{
						mob.SendLocalizedMessage( 1040024 ); // You are still too dazed from being knocked off your mount to ride!
						break;
					}
					case BlockMountType.BolaRecovery:
					{
						mob.SendLocalizedMessage( 1062910 ); // You cannot mount while recovering from a bola throw.
						break;
					}
					case BlockMountType.DismountRecovery:
					{
						mob.SendLocalizedMessage( 1070859 ); // You cannot mount while recovering from a dismount special maneuver.
						break;
					}
				}
			}

			return false;
		}

		public virtual void OnRiderDamaged( int amount, Mobile from, bool willKill )
		{
			if( m_Rider == null )
				return;

			Mobile attacker = from;
			if( attacker == null )
				attacker = m_Rider.FindMostRecentDamager( true );

			if( !(attacker == this || attacker == m_Rider || willKill || DateTime.Now < m_NextMountAbility) )
			{
				if( DoMountAbility( amount, from ) )
					m_NextMountAbility = DateTime.Now + MountAbilityDelay;

			}
		}

		public virtual bool DoMountAbility( int damage, Mobile attacker )
		{
			return false;
		}
	}

	public class MountItem : Item, IMountItem
	{
		private BaseMount m_Mount;

		public MountItem( BaseMount mount, int itemID ) : base( itemID )
		{
			Layer = Layer.Mount;
			Movable = false;

			m_Mount = mount;
		}

		public MountItem( Serial serial ) : base( serial )
		{
		}

		public override void OnAfterDelete()
		{
			if ( m_Mount != null )
				m_Mount.Delete();

			m_Mount = null;

			base.OnAfterDelete();
		}

		public override DeathMoveResult OnParentDeath(Mobile parent)
		{
			if ( m_Mount != null )
				m_Mount.Rider = null;

			return DeathMoveResult.RemainEquiped;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Mount );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Mount = reader.ReadMobile() as BaseMount;

					if ( m_Mount == null )
						Delete();

					break;
				}
			}
		}

		public IMount Mount
		{
			get
			{
				return m_Mount;
			}
		}
	}

	public enum BlockMountType
	{
		None = -1,
		Dazed,
		BolaRecovery,
		DismountRecovery
	}
}