using Server.Items;

namespace Server.Mobiles
{
	public class BuccaneersWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public BuccaneersWarriorGuard()
		{
		}

		public BuccaneersWarriorGuard( Serial serial ) : base( serial )
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
			plateChest.Hue = 2406;
			AddItem( plateChest );
			
			BasePlateArmor plateArms = new PlateArms();
			plateArms.Hue = 2406;
			AddItem( plateArms );
			
			BasePlateArmor plateLegs = new PlateLegs();
			plateLegs.Hue = 2406;
			AddItem( plateLegs );
			
			BasePlateArmor plateGloves = new PlateGloves();
			plateGloves.Hue = 2406;
			AddItem( plateGloves );
			
			BasePlateArmor plateGorget = new PlateGorget();
			plateGorget.Hue = 2406;
			AddItem( plateGorget );
			
			BasePlateArmor plateHelm = new PlateHelm();
			plateHelm.Hue = 2406;
			AddItem( plateHelm );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Scythe();
			weapon.Movable = false;
			weapon.Hue = 2406;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 133 ) );
			AddItem( new Cloak( 133 ) );
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
