using System;
using Server.Items;

namespace Server.Items
{
	public abstract class BaseDragonArmor : BaseArmor
	{
		public override int BasePhysicalResistance{ get{ return 3; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 3; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 3; } }
		
		public override ArmorMeditationAllowance DefMedAllowance{ get{ return ArmorMeditationAllowance.None; } }
		
		public BaseDragonArmor( Serial serial ) :  base( serial )
		{
		}
		
		public BaseDragonArmor( int itemID ) :  base( itemID )
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
