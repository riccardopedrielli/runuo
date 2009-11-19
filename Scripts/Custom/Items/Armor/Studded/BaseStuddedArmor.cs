using System;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseStuddedArmor : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 2; } }
		public override int BaseFireResistance{ get{ return 2; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 2; } }
		public override int BaseEnergyResistance{ get{ return 2; } }
		
		public override double MeditationFactor{ get{ return 0.75; } }
		
		public BaseStuddedArmor( Serial serial ) :  base( serial )
		{
		}
		
		public BaseStuddedArmor( int itemID ) :  base( itemID )
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
