using Server.Items;

namespace Server.Mobiles
{
	public class CoveMageGuard : MageTownGuard
	{
		[Constructable]
		public CoveMageGuard()
		{
		}

		public CoveMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateArmor()
		{
			AddItem( new LeatherGloves() );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new Robe( 433 ) );
			AddItem( new Cloak( 48 ) );
			AddItem( new Shoes( 538 ) );
			AddItem( new TallStrawHat( 0 ) );
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
