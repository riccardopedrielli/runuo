using Server.Items;

namespace Server.Mobiles
{
	public class MinocArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public MinocArcherGuard()
		{
		}

		public MinocArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BaseChainArmor chainChest = new ChainChest();
			chainChest.Hue = 2219;
			AddItem( chainChest );
			
			BaseRingArmor ringmailArms = new RingmailArms();
			ringmailArms.Hue = 2219;
			AddItem( ringmailArms );
			
			BaseRingArmor ringmailLegs = new RingmailLegs();
			ringmailLegs.Hue = 2219;
			AddItem( ringmailLegs );
			
			BaseRingArmor ringmailGloves = new RingmailGloves();
			ringmailGloves.Hue = 2219;
			AddItem( ringmailGloves );
			
			BaseLeatherArmor leatherCap = new LeatherCap();
			leatherCap.Hue = 2220;
			AddItem( leatherCap );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new HeavyCrossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 1109 ) );
			AddItem( new Cloak( 1109 ) );
			AddItem( new Shoes( 1323 ) );
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
