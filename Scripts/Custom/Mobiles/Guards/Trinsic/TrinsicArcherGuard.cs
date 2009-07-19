using Server.Items;

namespace Server.Mobiles
{
	public class TrinsicArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public TrinsicArcherGuard()
		{
		}

		public TrinsicArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			BaseChainArmor chainChest = new ChainChest();
			chainChest.Hue = 2418;
			AddItem( chainChest );
			
			BaseChainArmor chainLegs = new ChainLegs();
			chainLegs.Hue = 2418;
			AddItem( chainLegs );
			
			BaseLeatherArmor leatherGloves = new LeatherGloves();
			leatherGloves.Hue = 2418;
			AddItem( leatherGloves );
			
			BaseChainArmor chainCoif = new ChainCoif();
			chainCoif.Hue = 2418;
			AddItem( chainCoif );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new HeavyCrossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 733 ) );
			AddItem( new Cloak( 733 ) );
			AddItem( new Boots( 733 ) );
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
