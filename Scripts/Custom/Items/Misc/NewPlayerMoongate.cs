/***************************************************************************************/
/*   Moongate to set skills and stats on new characters and let them enter the world   */
/***************************************************************************************/

using Server.Mobiles;

namespace Server.Items
{
	public class NewPlayerMoongate : Item
	{
		private Point3D m_Target;
		private Map m_TargetMap;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Target
		{
			get{ return m_Target; }
			set{ m_Target = value; }
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Map TargetMap
		{
			get{ return m_TargetMap; }
			set{ m_TargetMap = value; }
		}

		[Constructable]
		public NewPlayerMoongate() : base( 0xF6C )
		{
			Movable = false;
			Hue = 1172;
			Name = "Enter the world of Sosaria";
			Light = LightType.NorthWestBig;
			Target = new Point3D( 1496, 1628, 10 );
			TargetMap = Map.Felucca;;
		}

		public NewPlayerMoongate( Serial serial ) : base( serial )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			m.CloseGump( typeof( SkillStatSetGump ) );
			m.SendGump ( new SkillStatSetGump( Target, TargetMap ) );
			Effects.PlaySound( this.Location, this.Map, 0x20E );
			
			return true;
		}

		public override bool OnMoveOff( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				m.CloseGump( typeof( SkillStatSetGump ) );
			}
			
			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write( m_Target );
			writer.Write( m_TargetMap );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 0:
				{
					m_Target = reader.ReadPoint3D();
					m_TargetMap = reader.ReadMap();
				
					break;
				}
			}
		}
	}
}