using Server.Items;

namespace Server.Mobiles
{
	public class JhelomMageGuard : MageTownGuard
	{
		[Constructable]
		public JhelomMageGuard()
		{
		}

		public JhelomMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new Robe( 1158 ) );
			AddItem( new Cloak( 253 ) );
			AddItem( new Shoes( 338 ) );
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
