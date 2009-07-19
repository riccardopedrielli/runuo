using Server.Items;

namespace Server.Mobiles
{
	public class TrinsicWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public TrinsicWarriorGuard()
		{
		}

		public TrinsicWarriorGuard( Serial serial ) : base( serial )
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
			plateChest.Hue = 2418;
			AddItem( plateChest );
			
			BasePlateArmor plateArms = new PlateArms();
			plateArms.Hue = 2418;
			AddItem( plateArms );
			
			BasePlateArmor plateLegs = new PlateLegs();
			plateLegs.Hue = 2418;
			AddItem( plateLegs );
			
			BasePlateArmor plateGloves = new PlateGloves();
			plateGloves.Hue = 2418;
			AddItem( plateGloves );
			
			BasePlateArmor plateGorget = new PlateGorget();
			plateGorget.Hue = 2418;
			AddItem( plateGorget );
			
			BasePlateArmor plateHelm = new PlateHelm();
			plateHelm.Hue = 2418;
			AddItem( plateHelm );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Bardiche();
			weapon.Movable = false;
			weapon.Hue = 2418;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new BodySash( 733 ) );
			AddItem( new Cloak( 733 ) );
		}
		
		protected override void GenerateMount()
		{
			new DesertOstard().Rider = this;
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
