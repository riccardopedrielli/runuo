using Server.Items;

namespace Server.Mobiles
{
	public class CoveArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public CoveArcherGuard()
		{
		}

		public CoveArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new LeatherChest() );
			AddItem( new LeatherArms() );
			AddItem( new LeatherLegs() );
			AddItem( new LeatherGloves() );
			AddItem( new LeatherGorget() );
			AddItem( new LeatherCap() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Bow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 35 ) );
			AddItem( new Cloak( 48 ) );
			AddItem( new Shoes( 538 ) );
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
