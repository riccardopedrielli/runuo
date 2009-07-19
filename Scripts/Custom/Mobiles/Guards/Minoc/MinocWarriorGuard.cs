using Server.Items;

namespace Server.Mobiles
{
	public class MinocWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public MinocWarriorGuard()
		{
		}

		public MinocWarriorGuard( Serial serial ) : base( serial )
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
			plateChest.Hue = 2219;
			AddItem( plateChest );
			
			BasePlateArmor plateArms = new PlateArms();
			plateArms.Hue = 2219;
			AddItem( plateArms );
			
			BasePlateArmor plateLegs = new PlateLegs();
			plateLegs.Hue = 2219;
			AddItem( plateLegs );
			
			BasePlateArmor plateGloves = new PlateGloves();
			plateGloves.Hue = 2219;
			AddItem( plateGloves );
			
			BasePlateArmor plateGorget = new PlateGorget();
			plateGorget.Hue = 2219;
			AddItem( plateGorget );
			
			BasePlateArmor closeHelm = new CloseHelm();
			closeHelm.Hue = 2219;
			AddItem( closeHelm );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new WarHammer();
			weapon.Movable = false;
			weapon.Hue = 2219;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new BodySash( 1109 ) );
			AddItem( new Cloak( 1109 ) );
		}
		
		protected override void GenerateMount()
		{
			new RidableLlama().Rider = this;
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
