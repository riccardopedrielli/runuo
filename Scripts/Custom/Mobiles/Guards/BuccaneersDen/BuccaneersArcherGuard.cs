using Server.Items;

namespace Server.Mobiles
{
	public class BuccaneersArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public BuccaneersArcherGuard()
		{
		}

		public BuccaneersArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new DaemonChest() );
			AddItem( new DaemonArms() );
			AddItem( new DaemonLegs() );
			AddItem( new DaemonGloves() );
			AddItem( new DaemonHelm() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new RepeatingCrossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new FancyShirt ( 2406 ) );
			AddItem( new Cloak( 133 ) );
			AddItem( new Shoes( 2406 ) );
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
