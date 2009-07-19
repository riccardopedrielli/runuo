using Server.Items;

namespace Server.Mobiles
{
	public class MinocMageGuard : MageTownGuard
	{
		[Constructable]
		public MinocMageGuard()
		{
		}

		public MinocMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateArmor()
		{
			BaseLeatherArmor leatherGloves = new LeatherGloves();
			leatherGloves.Hue = 2220;
			AddItem( leatherGloves );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 0 ) ); 
			AddItem( new Robe( 1102 ) );
			AddItem( new Cloak( 1109 ) );
			AddItem( new Boots( 1323 ) );
		}
		
		protected override void GenerateMount()
		{
			new RidableLlama().Rider = this;
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
