using Server.Items;

namespace Server.Mobiles
{
	public class YewArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public YewArcherGuard()
		{
		}

		public YewArcherGuard( Serial serial ) : base( serial )
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
			AddItem( new BodySash( 1436 ) );
			AddItem( new Cloak( 1436 ) );
			AddItem( new Sandals( 1436 ) );
		}
		
		protected override void GenerateMount()
		{
			new ForestOstard().Rider = this;
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
