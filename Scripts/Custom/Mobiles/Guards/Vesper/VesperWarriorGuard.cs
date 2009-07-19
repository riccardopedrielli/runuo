using Server.Items;

namespace Server.Mobiles
{
	public class VesperWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public VesperWarriorGuard()
		{
		}

		public VesperWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new RingmailChest() );
			AddItem( new PlateArms() );
			AddItem( new PlateLegs() );
			AddItem( new LeatherGloves() );
			AddItem( new PlateHelm() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new WarMace();
			weapon.Movable = false;
			AddItem( weapon );
			
			AddItem( new WoodenKiteShield() );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new Cloak( 655 ) );
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
