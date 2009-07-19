using Server.Items;

namespace Server.Mobiles
{
	public class SkaraWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public SkaraWarriorGuard()
		{
		}

		public SkaraWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new ChainChest() );
			AddItem( new PlateLegs() );
			AddItem( new PlateGloves() );
			AddItem( new CloseHelm() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Longsword();
			weapon.Movable = false;
			AddItem( weapon );
			
			AddItem( new BronzeShield() );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new BodySash( 148 ) );
			AddItem( new Cloak( 148 ) );
		}
		
		protected override void GenerateMount()
		{
			new Ridgeback().Rider = this;;
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
