using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Regions;
using Server.Targeting;

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

		protected override void GenerateClothes()
		{
			int hue = Utility.RandomBlueHue();
			AddItem( new Kilt( hue ) ); 
			AddItem( new BodySash( hue ) );
			AddItem( new Cloak( hue ) );
			AddItem( new Boots( hue ) );
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
