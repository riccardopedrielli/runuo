using System;
using Server;

namespace Server.Items
{
	public class Buckler : BaseShield
	{
		/*** MOD_START ***/
		/*
		public override int BasePhysicalResistance{ get{ return 0; } }
		*/
		public override int BasePhysicalResistance{ get{ return 1; } }
		/*** MOD_END ***/
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		/*** MOD_START ***/
		/*
		public override int BasePoisonResistance{ get{ return 1; } }
		*/
		public override int BasePoisonResistance{ get{ return 0; } }
		/*** MOD_END ***/
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 50; } }

		public override int AosStrReq{ get{ return 20; } }

		public override int ArmorBase{ get{ return 7; } }
		
		/*** ADD_START ***/
		public override double MeditationFactor{ get{ return 0.5; } }
		/*** ADD_END ***/

		[Constructable]
		public Buckler() : base( 0x1B73 )
		{
			Weight = 5.0;
		}

		public Buckler( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}
	}
}
