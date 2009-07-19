using Server.Items;

namespace Server.Mobiles
{
	public class OclloMageGuard : MageTownGuard
	{
		[Constructable]
		public OclloMageGuard()
		{
		}

		public OclloMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new TallStrawHat( 0 ) ); 
			AddItem( new Tunic( 0 ) );
			AddItem( new Kilt( 0 ) );
			AddItem( new ThighBoots( 0 ) );
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
