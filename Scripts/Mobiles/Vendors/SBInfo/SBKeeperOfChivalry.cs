using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBKeeperOfChivalry : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBKeeperOfChivalry()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
                /*** DEL_START ***/
                //no libro pala
				//Add( new GenericBuyInfo( typeof( BookOfChivalry ), 140, 20, 0x2252, 0 ) );
                /*** DEL_END ***/
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}