using System;
using System.Collections;

using Server;
using Server.Items;
using Server.Network;

namespace Server.Commands
{
    public enum FireEffect
    {
        FlameStrike,
        MagicArrow,
        Cure,
        Lightning,
        Teleport,
        Telekinesis,
        Poison,
        Explosion,
        PoisonField,
        FireField,
        EnergyField,
        WallOfStone,
        ParalyzeField,
        xX_Devious_Xx,
    }

    public class FireFeet
    {
        private static FireFeet m_Instance = new FireFeet();
        public static FireFeet Instance { get { return m_Instance; } }

        private Hashtable m_FireRunners;
        private FireEffect m_FireEffect;
        private int m_Hue;

        [CommandProperty( AccessLevel.Administrator )]
        public FireEffect FireEffect { get { return m_FireEffect; } set { m_FireEffect = value; } }
        [CommandProperty( AccessLevel.Administrator )]
        public int Hue { get { return m_Hue; } set { m_Hue = value; } }

        public static void Initialize()
        {
            Instance.m_FireRunners = new Hashtable();
            CommandHandlers.Register( "FireFeet", AccessLevel.Administrator, new CommandEventHandler( FireFeet_OnCommand ) );
            CommandHandlers.Register( "FireFeetSettings", AccessLevel.Administrator, new CommandEventHandler( FireFeetSettings_OnCommand ) );
            EventSink.Movement += new MovementEventHandler( EventSink_Movement );
        }

        private static void EventSink_Movement( MovementEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            if( Instance.m_FireRunners.ContainsKey( m.Serial ) )
            {
                PlayEffect( m );
            }
        }

        private static void PlayEffect( Mobile m )
        {
            switch( Instance.FireEffect )
            {
                case FireEffect.FlameStrike:
                    {
                        Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, Instance.Hue, 0, 5052, 0 );
                        Effects.PlaySound( m.Location, m.Map, 0x225 );
                        break;
                    }
                case FireEffect.MagicArrow:
                    {
                        foreach( Object obj in m.GetObjectsInRange( 10 ) )
                        {
                            if( obj != null && obj is IEntity )
                            {
                                m.MovingParticles( obj as IEntity, 0x36E4, 5, 0, false, true, Instance.Hue, 0, 3006, 4006, 0, EffectLayer.Waist, 0 );
                                m.PlaySound( 0x1E5 );
                            }
                        }
                        break;
                    }
                case FireEffect.Cure:
                    {
                        Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x373A, 10, 15, Instance.Hue, 0, 5012, 0 );
                        Effects.PlaySound( m.Location, m.Map, 0x1E0 );
                        break;
                    }
                case FireEffect.Lightning:
                    {
                        foreach( Mobile obj in m.GetMobilesInRange( 10 ) )
                        {
                            if( obj != null )
                                obj.BoltEffect( 0 );
                        }
                        break;                        
                    }
                case FireEffect.Teleport:
                    {
                        Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, Instance.Hue, 0, 2023, 0 );
                        Effects.PlaySound( m.Location, m.Map, 0x1F5 );
                        break;
                    }
                case FireEffect.Telekinesis:
                    {
                        Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, Instance.Hue, 0, 5022, 0 );
                        Effects.PlaySound( m.Location, m.Map, 0x1FE );
                        break;
                    }
                case FireEffect.Poison:
                    {
                        Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x374A, 10, 15, Instance.Hue, 0, 5021, 0 );
                        Effects.PlaySound( m.Location, m.Map, 0x474 );
                        break;
                    }
                case FireEffect.Explosion:
                    {
                        Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, Instance.Hue, 0, 5044, 0 );
                        Effects.PlaySound( m.Location, m.Map, 0x474 );
                        break;
                    }
                case FireEffect.xX_Devious_Xx:
                    {
                        Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x376A, 100, 1, Instance.Hue, 0, 5555, 0 );
                        Effects.PlaySound( m.Location, m.Map, 0x474 );
                        break;
                    }
            }
        }

        [Usage( "FireFeetSettings" )]
        [Description( "Allows you to set the global effect for the FireFeet command." )]
        private static void FireFeetSettings_OnCommand( CommandEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            m.SendGump( new Server.Gumps.PropertiesGump( m, m_Instance ) );
        }        

        [Usage( "FireFeet [true|false]" )]
        [Description( "Enables a fire feet for the invoker.  Disable with paramaters." )]
        private static void FireFeet_OnCommand( CommandEventArgs e )
        {
            Mobile from = e.Mobile;

            if( e.Length <= 1 )
            {
                if( e.Length == 1 && !e.GetBoolean( 0 ) )
                {
                    from.SendMessage( "Fire feet has been disabled." );
                    if( Instance.m_FireRunners.ContainsKey( from.Serial ) )
                        Instance.m_FireRunners.Remove( from.Serial );
                }
                else
                {
                    from.SendMessage( "Fire feet has been enabled." );
                    if( !Instance.m_FireRunners.ContainsKey( from.Serial ) )
                        Instance.m_FireRunners.Add( from.Serial, from );
                }
            }
            else
            {
                from.SendMessage( "Format: FireFeet [true|false]" );
            }
        }
    }      
}
