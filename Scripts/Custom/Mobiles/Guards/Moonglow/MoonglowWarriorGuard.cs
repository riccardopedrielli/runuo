using Server.Items;

namespace Server.Mobiles
{
	public class MoonglowWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public MoonglowWarriorGuard()
		{
		}

		public MoonglowWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BasePlateArmor plateChest;
			if ( Female )
			{
				plateChest = new FemalePlateChest();
			}
			else
			{
				plateChest = new PlateChest();
			}
			plateChest.Hue = 2425;
			AddItem( plateChest );
			
			BasePlateArmor plateArms = new PlateArms();
			plateArms.Hue = 2425;
			AddItem( plateArms );
			
			BasePlateArmor plateLegs = new PlateLegs();
			plateLegs.Hue = 2425;
			AddItem( plateLegs );
			
			BasePlateArmor plateGloves = new PlateGloves();
			plateGloves.Hue = 2425;
			AddItem( plateGloves );
			
			BasePlateArmor plateGorget = new PlateGorget();
			plateGorget.Hue = 2425;
			AddItem( plateGorget );
			
			BaseRingArmor helmet = new Helmet();
			helmet.Hue = 2425;
			AddItem( helmet );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new LargeBattleAxe();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new BodySash( 413 ) );
			AddItem( new Cloak( 413 ) );
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
