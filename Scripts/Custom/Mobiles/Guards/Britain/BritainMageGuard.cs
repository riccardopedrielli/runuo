using Server.Items;

namespace Server.Mobiles
{
	public class BritainMageGuard : MageTownGuard
	{
		[Constructable]
		public BritainMageGuard()
		{
		}

		public BritainMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 1324 ) ); 
			AddItem( new Robe( 1324 ) );
			AddItem( new Cloak( 1324 ) );
			AddItem( new Boots( 1324 ) );
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
