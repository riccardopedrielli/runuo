using System;
using Server.Network;
using Server;

namespace Server.Misc
{
	public class FoodDecayTimer : Timer
	{
		public static void Initialize()
		{
			new FoodDecayTimer().Start();
		}

		public FoodDecayTimer() : base( TimeSpan.FromMinutes( 5 ), TimeSpan.FromMinutes( 5 ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			FoodDecay();			
		}

		public static void FoodDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
				HungerDecay( state.Mobile );
				ThirstDecay( state.Mobile );
			}
		}

		public static void HungerDecay( Mobile m )
		{
            /*** MOD_START ***/
            //if ( m != null && m.Hunger >= 1 )
            //    m.Hunger -= 1;
            if (m != null && m.AccessLevel == AccessLevel.Player)
            {
                if(m.Hunger >= 1 )
                    m.Hunger -= 1;

                switch (m.Hunger)
                {
                    case 3:
                        m.SendMessage(133, "You become hungry.");
                        break;
                    case 2:
                        m.SendMessage(133, "You are very hungry.");
                        break;
                    case 1:
                        m.SendMessage(133, "You are extremely hungry!!!");
                        break;
                    case 0:
                        m.Kill();
                        m.SendMessage(133, "You are dead by hunger!!!");
                        m.Hunger = 5;
                        break;
                }
            }
            /*** MOD_END ***/
		}

		public static void ThirstDecay( Mobile m )
		{
            /*** MOD_START ***/
            //if ( m != null && m.Thirst >= 1 )
            //    m.Thirst -= 1;
            if (m != null && m.AccessLevel == AccessLevel.Player)
            {
                if (m.Thirst >= 1)
                    m.Thirst -= 1;

                switch (m.Thirst)
                {
                    case 3:
                        m.SendMessage(133, "You become thirsty.");
                        break;
                    case 2:
                        m.SendMessage(133, "You are very thirsty.");
                        break;
                    case 1:
                        m.SendMessage(133, "You are extremely thirsty!!!");
                        break;
                    case 0:
                        m.Kill();
                        m.SendMessage(133, "You are dead by thirst!!!");
                        m.Hunger = 5;
                        break;
                }
            }
            /*** MOD_END ***/
		}
	}

    /*** ADD_START ***/
    //DOT che toglie vita e stamina a seconda della fame o sete
    public class DOTFoodThirstTimer : Timer
    {
        public static void Initialize()
        {
            new DOTFoodThirstTimer().Start();
        }

        public DOTFoodThirstTimer()
            : base(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3))
        {
            Priority = TimerPriority.OneSecond;
        }

        protected override void OnTick()
        {
            FoodDecay();
        }

        public static void FoodDecay()
        {
            foreach (NetState state in NetState.Instances)
            {
                HungerDecay(state.Mobile);
                ThirstDecay(state.Mobile);
            }
        }

        public static void HungerDecay(Mobile m)
        {
            if (m != null && m.Alive && m.AccessLevel == AccessLevel.Player)
            {
                int DOT = 0;
                
                switch (m.Hunger)
                {
                    //case 4:
                    //    DOT = Convert.ToInt16((m.Str * 1) / 100);
                    //    break;
                    case 3:
                        DOT = Convert.ToInt16((m.Str * 3) / 100);
                        break;
                    case 2:
                        DOT = Convert.ToInt16((m.Str * 6) / 100);
                        break;
                    case 1:
                        DOT = Convert.ToInt16((m.Str * 10) / 100);
                        break;                    
                }

                if (m.Hits < DOT)
                {
                    m.Hits = 0;
                    m.Kill();
                    m.SendMessage(133, "You are dead by hunger!!!");
                    m.Hunger = 5;
                }
                
                m.Hits -= DOT; //se hits va in negativo viene impostato a 0
            }
        }

        public static void ThirstDecay(Mobile m)
        {
            if (m != null && m.Stam > 0 && m.Alive && m.AccessLevel == AccessLevel.Player)
            {
                switch (m.Thirst)
                {
                    //case 4:
                    //    m.Stam -= Convert.ToInt16((m.Dex * 6) / 100);
                    //    break;
                    case 3:
                        m.Stam -= Convert.ToInt16((m.Dex * 9) / 100);
                        break;
                    case 2:
                        m.Stam -= Convert.ToInt16((m.Dex * 12) / 100);
                        break;
                    case 1:
                        m.Stam -= Convert.ToInt16((m.Dex * 15) / 100);
                        break;                    
                }
            }
        }
    }
    /*** ADD_END ***/
}