using Server.Items;

namespace Server.Mobiles
{
	public class BritainWarriorGuard : WarriorTownGuard
	{
		[Constructable]
		public BritainWarriorGuard()
		{
		}

		public BritainWarriorGuard( Serial serial ) : base( serial )
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
			BaseWeapon weapon = new Halberd();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new Kilt( 1324 ) ); 
			AddItem( new BodySash( 1324 ) );
			AddItem( new Cloak( 1324 ) );
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
