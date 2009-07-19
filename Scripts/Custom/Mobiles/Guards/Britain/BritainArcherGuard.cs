using Server.Items;

namespace Server.Mobiles
{
	public class BritainArcherGuard : ArcherTownGuard
	{
		[Constructable]
		public BritainArcherGuard()
		{
		}

		public BritainArcherGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateArmor()
		{
			AddItem( new ChainChest() );
			AddItem( new ChainLegs() );
			AddItem( new LeatherGloves() );
			AddItem( new ChainCoif() );
		}

		protected override void GenerateWeapon()
		{
			BaseWeapon weapon = new HeavyCrossbow();
			weapon.Movable = false;
			AddItem( weapon );
		}
		
		protected override void GenerateClothes()
		{
			AddItem( new Kilt( 1324 ) ); 
			AddItem( new BodySash( 1324 ) );
			AddItem( new Cloak( 1324 ) );
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
