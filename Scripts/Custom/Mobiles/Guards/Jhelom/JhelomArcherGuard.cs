using Server.Items;

namespace Server.Mobiles
{
	public class JhelomArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public JhelomArcherGuard()
		{
		}

		public JhelomArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new StuddedChest() );
			AddItem( new StuddedArms() );
			AddItem( new StuddedLegs() );
			AddItem( new StuddedGloves() );
			AddItem( new StuddedGorget() );
			AddItem( new LeatherCap() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Crossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
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
