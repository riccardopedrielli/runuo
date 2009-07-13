/***************************************************************************************/
/*   Moongate to set skills and stats on new characters and let them enter the world   */
/***************************************************************************************/

using Server.Mobiles;

namespace Server.Items
{
	public class NewPlayerMoongate : Item
	{
		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } }

		[Constructable]
		public NewPlayerMoongate() : base( 0xF6C )
		{
			Movable = false;
			Hue = 1172;
			Name = "Enter the real world";
			Light = LightType.Circle300;
		}

		public NewPlayerMoongate( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
			m.CloseGump( typeof( SkillStatSetGump ) );
			m.SendGump ( new SkillStatSetGump() );
			
			return true;
		}

		public override bool HandlesOnMovement{ get{ return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m is PlayerMobile )
			{
				if ( !Utility.InRange( m.Location, this.Location, 0 ) && Utility.InRange( oldLocation, this.Location, 0 ) )
					m.CloseGump( typeof( SkillStatSetGump ) );
			}
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
		}
	}
}