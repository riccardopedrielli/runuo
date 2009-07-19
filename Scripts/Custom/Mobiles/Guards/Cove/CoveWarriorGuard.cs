using Server.Items;

namespace Server.Mobiles
{
	public class CoveWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public CoveWarriorGuard()
		{
		}

		public CoveWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new RingmailChest() );
			AddItem( new RingmailArms() );
			AddItem( new RingmailLegs() );
			AddItem( new RingmailGloves() );
			AddItem( new NorseHelm() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Axe();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new BodySash( 35 ) );
			AddItem( new Cloak( 48 ) );
			AddItem( new Shoes( 538 ) );
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
