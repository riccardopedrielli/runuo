using Server.Items;

namespace Server.Mobiles
{
	public class NujelmWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public NujelmWarriorGuard()
		{
		}

		public NujelmWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new RingmailChest() );
			AddItem( new RingmailArms() );
			AddItem( new RingmailLegs() );
			AddItem( new PlateGloves() );
			AddItem( new Bascinet() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new CrescentBlade();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new Surcoat( 0 ) );
			AddItem( new Cloak( 133 ) );
			AddItem( new Shoes( 638 ) );
		}
		
		protected override void GenerateMount()
		{
			new Beetle().Rider = this;
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
