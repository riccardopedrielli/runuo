using Server.Items;

namespace Server.Mobiles
{
	public class VesperArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public VesperArcherGuard()
		{
		}

		public VesperArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new StuddedChest() );
			AddItem( new StuddedArms() );
			AddItem( new StuddedLegs() );
			AddItem( new StuddedGloves() );
			AddItem( new StuddedGorget() );
			AddItem( new ChainCoif() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new RepeatingCrossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 655 ) );
			AddItem( new Cloak( 655 ) );
			AddItem( new Sandals( 0 ) );
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
