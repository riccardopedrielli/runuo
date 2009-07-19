using Server.Items;

namespace Server.Mobiles
{
	public class BuccaneersMageGuard : MageTownGuard
	{
		[Constructable]
		public BuccaneersMageGuard()
		{
		}

		public BuccaneersMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new SkullCap( 133 ) ); 
			AddItem( new Shirt( 0 ) );
			AddItem( new ShortPants( 133 ) );
			AddItem( new Shoes( 0 ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
		}
	}
}
