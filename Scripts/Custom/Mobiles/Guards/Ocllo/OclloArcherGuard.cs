using Server.Items;

namespace Server.Mobiles
{
	public class OclloArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public OclloArcherGuard()
		{
		}

		public OclloArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new LeatherChest() );
			AddItem( new LeatherGloves() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Bow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new StrawHat( 0 ) );
			AddItem( new FancyShirt( 0 ) );
			AddItem( new BodySash( 998 ) );
			AddItem( new LongPants ( 2418 ) );
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
