using Server.Items;

namespace Server.Mobiles
{
	public class NujelmArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public NujelmArcherGuard()
		{
		}

		public NujelmArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BaseStuddedArmor studdedChest = new StuddedChest();
			studdedChest.Hue = 2220;
			AddItem( studdedChest );
			
			BaseStuddedArmor studdedArms = new StuddedArms();
			studdedArms.Hue = 2220;
			AddItem( studdedArms );
			
			BaseStuddedArmor studdedLegs = new StuddedLegs();
			studdedLegs.Hue = 2220;
			AddItem( studdedLegs );
			
			BaseStuddedArmor studdedGloves = new StuddedGloves();
			studdedGloves.Hue = 2220;
			AddItem( studdedGloves );
			
			BaseStuddedArmor studdedGorget = new StuddedGorget();
			studdedGorget.Hue = 2220;
			AddItem( studdedGorget );
			
			BaseLeatherArmor leatherCap = new LeatherCap();
			leatherCap.Hue = 2220;
			AddItem( leatherCap );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new RepeatingCrossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new Surcoat( 0 ) );
			AddItem( new Cloak( 133 ) );
			AddItem( new Shoes( 638 ) );
		}
		
		protected override void GenerateMount()
		{
			new Beetle().Rider = this;
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
