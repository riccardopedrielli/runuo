using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x2B02, 0x2B03 )]
	public class Quiver : BaseQuiver
	{
		[Constructable]
		public Quiver() : this( 0x2B02 )
		{
			if(Quality == ClothingQuality.Exceptional)
				WeightReduction = 30;
			else
				WeightReduction = 50;
		}

		public Quiver( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
