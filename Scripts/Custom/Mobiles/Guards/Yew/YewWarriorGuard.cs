using Server.Items;

namespace Server.Mobiles
{
	public class YewWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public YewWarriorGuard()
		{
		}

		public YewWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BaseChainArmor chainChest = new ChainChest();
			chainChest.Hue = 2413;
			AddItem( chainChest );
			
			BaseChainArmor chainLegs = new ChainLegs();
			chainLegs.Hue = 2413;
			AddItem( chainLegs );
			
			BaseRingArmor ringmailGloves = new RingmailGloves();
			ringmailGloves.Hue = 2413;
			AddItem( ringmailGloves );
			
			BaseChainArmor chainCoif = new ChainCoif();
			chainCoif.Hue = 2413;
			AddItem( chainCoif );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new BladedStaff();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 1436 ) );
			AddItem( new Cloak( 1436 ) );
			AddItem( new Sandals( 0 ) );
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
