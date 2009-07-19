using Server.Items;

namespace Server.Mobiles
{
	public class YewMageGuard : MageTownGuard
	{
		[Constructable]
		public YewMageGuard()
		{
		}

		public YewMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new HoodedShroudOfShadows( 2017 ) );
			AddItem( new Cloak( 1436 ) );
			AddItem( new Sandals( 0 ) );
		}
		
		protected override void GenerateMount()
		{
			new FrenziedOstard().Rider = this;
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
