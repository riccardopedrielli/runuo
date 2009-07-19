using Server.Items;

namespace Server.Mobiles
{
	public class TrinsicMageGuard : MageTownGuard
	{
		[Constructable]
		public TrinsicMageGuard()
		{
		}

		public TrinsicMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateArmor()
		{
			AddItem( new LeatherChest() );
			AddItem( new LeatherArms() );
			AddItem( new LeatherLegs() );
			AddItem( new LeatherGloves() );
			AddItem( new LeatherGorget() );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 733 ) ); 
			AddItem( new BodySash( 733 ) );
			AddItem( new Cloak( 733 ) );
			AddItem( new Boots( 733 ) );
		}
		
		protected override void GenerateMount()
		{
			new DesertOstard().Rider = this;
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
