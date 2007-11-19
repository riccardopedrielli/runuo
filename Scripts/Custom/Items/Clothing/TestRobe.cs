using System;
using Server;
using Server.Items;
using Server.Accounting;

namespace Server.Items
{
	public class TestRobe : BaseOuterTorso
	{
		[Constructable]
		public TestRobe() : base( 0x204F )
		{
			Weight = 0.0;
			Name = "Test Robe";
			Hue=503;
			Layer = Layer.OuterTorso;
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( Parent != m )
				m.SendMessage( "You must be wearing the robe to use it!" );
			else
			{
				foreach ( Mobile check in m.GetMobilesInRange( 30 ) )
				{
					if ( check.Combatant == m )
						check.Combatant=null;
				}
				m.Combatant = null;
				if ( ItemID == 0x204F )
				{
					m.AccessLevel = AccessLevel.Player;
					Name = "Robe";
					ItemID = 0x1F03;
					m.RemoveItem(this);
					m.EquipItem(this);
				}
				else if ( ItemID == 0x1F03 )
				{
					Account status = m.Account as Account;
					if( status.AccessLevel != AccessLevel.Player )
					{
						m.AccessLevel = status.AccessLevel;
						Name = "Test Robe";
						ItemID = 0x204F;
						m.RemoveItem(this);
						m.EquipItem(this);
					}
				}
			}
		}

		public TestRobe( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}

