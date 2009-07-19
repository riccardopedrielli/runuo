using Server.Items;

namespace Server.Mobiles
{
	public class NujelmMageGuard : MageTownGuard
	{
		[Constructable]
		public NujelmMageGuard()
		{
		}

		public NujelmMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 0 ) ); 
			AddItem( new Robe( 0 ) );
			AddItem( new Cloak( 133 ) );
			AddItem( new Shoes( 638 ) );
		}
		
		protected override void GenerateMount()
		{
			new Beetle().Rider = this;
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
