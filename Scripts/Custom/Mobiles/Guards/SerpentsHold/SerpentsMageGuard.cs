using Server.Items;

namespace Server.Mobiles
{
	public class SerpentsMageGuard : MageTownGuard
	{
		[Constructable]
		public SerpentsMageGuard()
		{
		}

		public SerpentsMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 983 ) );
			AddItem( new Robe( 983 ) );
			AddItem( new Cloak( 983 ) );
			AddItem( new Shoes( 983 ) );
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
