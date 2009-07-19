using Server.Items;

namespace Server.Mobiles
{
	public class SkaraArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public SkaraArcherGuard()
		{
		}

		public SkaraArcherGuard( Serial serial ) : base( serial )
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
			BaseWeapon weapon = new CompositeBow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 148 ) );
			AddItem( new Cloak( 148 ) );
			AddItem( new Shoes( 148 ) );
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
