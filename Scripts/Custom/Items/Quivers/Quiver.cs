using System;
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute( 0x2B02, 0x2B03 )]
	public class Quiver : BaseQuiver
	{
		[Constructable]
		public Quiver() : base( 0x2B02 )
		{
			Name = "Quiver";
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
		
		#region ICraftable
		public override int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Quality = (ClothingQuality) quality;

			if ( makersMark )
				Crafter = from;
			
			if(Quality == ClothingQuality.Exceptional)
				DamageIncrease = (int)(from.Skills.ArmsLore.Value / 10);

			return quality;
		}
		#endregion
	}
}
