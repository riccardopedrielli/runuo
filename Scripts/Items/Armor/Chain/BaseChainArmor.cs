using System;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseChainArmor : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 5; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }
		
		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.None; } }
		
		public BaseChainArmor( Serial serial ) :  base( serial )
		{
		}
		
		public BaseChainArmor( int itemID ) :  base( itemID )
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
