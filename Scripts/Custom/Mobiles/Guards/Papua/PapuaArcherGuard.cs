using Server.Items;

namespace Server.Mobiles
{
	public class PapuaArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public PapuaArcherGuard()
		{
		}

		public PapuaArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BaseStuddedArmor studdedChest = new StuddedChest();
			studdedChest.Hue = 2117;
			AddItem( studdedChest );
			
			BaseStuddedArmor studdedArms = new StuddedArms();
			studdedArms.Hue = 2117;
			AddItem( studdedArms );
			
			BaseStuddedArmor studdedLegs = new StuddedLegs();
			studdedLegs.Hue = 2117;
			AddItem( studdedLegs );
			
			BaseStuddedArmor studdedGloves = new StuddedGloves();
			studdedGloves.Hue = 2117;
			AddItem( studdedGloves );
			
			BaseStuddedArmor studdedGorget = new StuddedGorget();
			studdedGorget.Hue = 2117;
			AddItem( studdedGorget );
			
			BaseLeatherArmor leatherCap = new LeatherCap();
			leatherCap.Hue = 2117;
			AddItem( leatherCap );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Crossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new Surcoat( 1109 ) );
			AddItem( new Cloak( 768 ) );
			AddItem( new Shoes( 138 ) );
		}
		
		protected override void GenerateMount()
		{
			new SwampDragon().Rider = this;
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
