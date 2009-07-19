using Server.Items;

namespace Server.Mobiles
{
	public class MoonglowArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public MoonglowArcherGuard()
		{
		}

		public MoonglowArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new LeatherChest() );
			AddItem( new StuddedArms() );
			AddItem( new StuddedLegs() );
			AddItem( new StuddedGloves() );
			AddItem( new LeatherGorget() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Bow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new FeatheredHat( 413 ) );
			AddItem( new BodySash( 413 ) );
			AddItem( new Cloak( 413 ) );
			AddItem( new Shoes( 733 ) );
		}
		
		protected override void GenerateMount()
		{
			new Ridgeback().Rider = this;
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
