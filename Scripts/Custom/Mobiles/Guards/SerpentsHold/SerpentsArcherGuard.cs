using Server.Items;

namespace Server.Mobiles
{
	public class SerpentsArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public SerpentsArcherGuard()
		{
		}

		public SerpentsArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new ChainChest() );
			AddItem( new LeatherArms() );
			AddItem( new LeatherLegs() );
			AddItem( new LeatherGloves() );
			AddItem( new ChainCoif() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new HeavyCrossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new Cloak( 983 ) );
			AddItem( new Shoes( 983 ) );
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
