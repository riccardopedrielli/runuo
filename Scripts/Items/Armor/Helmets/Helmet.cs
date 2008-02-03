using System;
using Server;

namespace Server.Items
{
	/*** MOD_START ***/
	/*
	public class Helmet : BaseArmor
	*/
	public class Helmet : BaseRingArmor
	/*** MOD_END ***/
	{
		/*** DEL_START ***/
		/*
		public override int BasePhysicalResistance{ get{ return 2; } }
		public override int BaseFireResistance{ get{ return 4; } }
		public override int BaseColdResistance{ get{ return 4; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 2; } }
		*/
		/*** DEL_END ***/

		public override int InitMinHits{ get{ return 45; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override int AosStrReq{ get{ return 45; } }
		public override int OldStrReq{ get{ return 40; } }

		public override int ArmorBase{ get{ return 30; } }

		/*** MOD_START ***/
		/*
		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }
		*/
		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Ringmail; } }
		/*** MOD_END ***/

		[Constructable]
		public Helmet() : base( 0x140A )
		{
			Weight = 5.0;
		}

		public Helmet( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( Weight == 1.0 )
				Weight = 5.0;
		}
	}
}
