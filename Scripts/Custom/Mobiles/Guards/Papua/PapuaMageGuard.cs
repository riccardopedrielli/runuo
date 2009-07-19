using Server.Items;

namespace Server.Mobiles
{
	public class PapuaMageGuard : MageTownGuard
	{
		[Constructable]
		public PapuaMageGuard()
		{
		}

		public PapuaMageGuard( Serial serial ) : base( serial )
		{
		}
		
		protected override void GenerateWeapon()
		{
			Spellbook spellbook = new Spellbook( ulong.MaxValue );
			spellbook.Hue = 768;
			AddItem( spellbook );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new WizardsHat( 768 ) ); 
			AddItem( new Robe( 2117 ) );
			AddItem( new Cloak( 768 ) );
			AddItem( new Shoes( 138 ) );
		}
		
		protected override void GenerateMount()
		{
			new SwampDragon().Rider = this;
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
