using Server.Items;

namespace Server.Mobiles
{
	public class MoonglowMageGuard : MageTownGuard
	{
		[Constructable]
		public MoonglowMageGuard()
		{
		}

		public MoonglowMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 413 ) ); 
			AddItem( new Robe( 410 ) );
			AddItem( new Cloak( 413 ) );
			AddItem( new Shoes( 733 ) );
		}
		
		protected override void GenerateMount()
		{
			new Ridgeback().Rider = this;
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
