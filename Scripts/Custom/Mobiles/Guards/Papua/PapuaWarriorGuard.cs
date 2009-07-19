using Server.Items;

namespace Server.Mobiles
{
	public class PapuaWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public PapuaWarriorGuard()
		{
		}

		public PapuaWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BaseDragonArmor dragonChest = new DragonChest();
			dragonChest.Hue = 1109;
			AddItem( dragonChest );
			
			BaseDragonArmor dragonArms = new DragonArms();
			dragonArms.Hue = 1109;
			AddItem( dragonArms );
			
			BaseDragonArmor dragonLegs = new DragonLegs();
			dragonLegs.Hue = 1109;
			AddItem( dragonLegs );
			
			BaseDragonArmor dragonGloves = new DragonGloves();
			dragonGloves.Hue = 1109;
			AddItem( dragonGloves );
			
			BasePlateArmor plateGorget = new PlateGorget();
			plateGorget.Hue = 2406;
			AddItem( plateGorget );
			
			BaseDragonArmor dragonHelm = new DragonHelm();
			dragonHelm.Hue = 1109;
			AddItem( dragonHelm );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Pike();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new Cloak( 768 ) );
			AddItem( new Shoes( 138 ) );
		}
		
		protected override void GenerateMount()
		{
			ScaledSwampDragon dragon = new ScaledSwampDragon();
			dragon.Hue = 2207;
			dragon.Rider = this;
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
