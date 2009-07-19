using Server.Items;

namespace Server.Mobiles
{
	public class VesperMageGuard : MageTownGuard
	{
		[Constructable]
		public VesperMageGuard()
		{
		}

		public VesperMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 655 ) ); 
			AddItem( new Robe( 499 ) );
			AddItem( new Cloak( 655 ) );
			AddItem( new Boots( 0 ) );
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
