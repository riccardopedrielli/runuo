using Server.Items;

namespace Server.Mobiles
{
	public class SkaraMageGuard : MageTownGuard
	{
		[Constructable]
		public SkaraMageGuard()
		{
		}

		public SkaraMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 0 ) ); 
			AddItem( new Robe( 48 ) );
			AddItem( new Cloak( 148 ) );
			AddItem( new Shoes( 148 ) );
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
