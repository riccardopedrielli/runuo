using System;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseBoneArmor : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 3; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 1; } }
		public override int BasePoisonResistance{ get{ return 1; } }
		public override int BaseEnergyResistance{ get{ return 1; } }
		
		public override double MeditationFactor{ get{ return 0.5; } }
		
		public BaseBoneArmor( Serial serial ) :  base( serial )
		{
		}
		
		public BaseBoneArmor( int itemID ) :  base( itemID )
		{
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
