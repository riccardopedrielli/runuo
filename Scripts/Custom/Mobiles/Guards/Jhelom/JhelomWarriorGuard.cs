using Server.Items;

namespace Server.Mobiles
{
	public class JhelomWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public JhelomWarriorGuard()
		{
		}

		public JhelomWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BaseDragonArmor dragonChest = new DragonChest();
			dragonChest.Hue = 1645;
			AddItem( dragonChest );
			
			BaseDragonArmor dragonArms = new DragonArms();
			dragonArms.Hue = 1645;
			AddItem( dragonArms );
			
			BaseDragonArmor dragonLegs = new DragonLegs();
			dragonLegs.Hue = 1645;
			AddItem( dragonLegs );
			
			BaseDragonArmor dragonGloves = new DragonGloves();
			dragonGloves.Hue = 1645;
			AddItem( dragonGloves );
			
			BaseDragonArmor dragonHelm = new DragonHelm();
			dragonHelm.Hue = 1645;
			AddItem( dragonHelm );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new DoubleBladedStaff();
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
