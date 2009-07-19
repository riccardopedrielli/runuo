using Server.Items;

namespace Server.Mobiles
{
	public class OclloWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public OclloWarriorGuard()
		{
		}

		public OclloWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new RingmailChest() );
			AddItem( new StuddedArms() );
			AddItem( new StuddedLegs() );
			AddItem( new LeatherGloves() );
			AddItem( new LeatherCap() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new Pitchfork();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new BodySash( 998 ) );
			AddItem( new Boots( 0 ) );
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
