using Server.Items;

namespace Server.Mobiles
{
	public class SerpentsWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public SerpentsWarriorGuard()
		{
		}

		public SerpentsWarriorGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			if ( Female )
			{
				AddItem( new FemalePlateChest() );
			}
			else
			{
				AddItem( new PlateChest() );
			}
			AddItem( new PlateArms() );
			AddItem( new PlateLegs() );
			AddItem( new PlateGloves() );
			AddItem( new PlateGorget() );
			AddItem( new PlateHelm() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new WarAxe();
			weapon.Movable = false;
			AddItem( weapon );
			
			AddItem( new OrderShield() );
		}
		
		protected override void GenerateClothes()
		{ 
			AddItem( new Cloak( 983 ) );
			AddItem( new Shoes( 983 ) );
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
