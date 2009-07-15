using System;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Items;

namespace Server.Spells.Third
{
	public class TelekinesisSpell : MagerySpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Telekinesis", "Ort Por Ylem",
				203,
				9031,
				Reagent.Bloodmoss,
				Reagent.MandrakeRoot
			);

		public override SpellCircle Circle { get { return SpellCircle.Third; } }

		public TelekinesisSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

        /*** DEL_START ***/
        /*
		public void Target( ITelekinesisable obj )
		{
			if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, obj );

				obj.OnTelekinesis( Caster );
			}

			FinishSequence();
		}

		public void Target( Container item )
		{
			if ( CheckSequence() )
			{
				SpellHelper.Turn( Caster, item );

				object root = item.RootParent;

				if ( !item.IsAccessibleTo( Caster ) )
				{
					item.OnDoubleClickNotAccessible( Caster );
				}
				else if ( !item.CheckItemUse( Caster, item ) )
				{
				}
				else if ( root != null && root is Mobile && root != Caster )
				{
					item.OnSnoop( Caster );
				}
				else if ( item is Corpse && !((Corpse)item).CheckLoot( Caster, null ) )
				{
				}
				else if ( Caster.Region.OnDoubleClick( Caster, item ) )
				{
					Effects.SendLocationParticles( EffectItem.Create( item.Location, item.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5022 );
					Effects.PlaySound( item.Location, item.Map, 0x1F5 );

					item.DisplayTo( Caster );
					item.OnItemUsed( Caster, item );
				}
			}

			FinishSequence();
		}
        */
        /*** DEL_END ***/

        /*** ADD_START ***/
        public void Target(Item item)
        {
            if (CheckSequence()) 
            {        
                Item retItem = null;
                int spellMaxWeight = (int)(Caster.Skills[SkillName.Magery].Value / 3),
                    backpackMaxWeight = (Caster.Backpack.MaxWeight - Caster.Backpack.TotalWeight);

                if (backpackMaxWeight <= 0)
                {
                    Caster.SendMessage("Your Backpack cannot hold more weight.");
                    return;
                }

                if (item.Weight > spellMaxWeight)
                {
                    Caster.SendMessage("This item is too heavy to move.");
                    return;
                }

                if (spellMaxWeight > backpackMaxWeight)
                    spellMaxWeight = backpackMaxWeight;

                if (item.Stackable && item.Amount > 1)
                {
                    int maxAmount = (int)(spellMaxWeight / item.Weight);

                    if (maxAmount >= item.Amount)
                        retItem = item;
                    else
                        retItem = Mobile.LiftItemDupe(item, item.Amount - maxAmount);
                }
                else
                {
                    retItem = item;
                }

                Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);
                Effects.PlaySound(item.Location, item.Map, 0x1F5);

                Caster.AddToBackpack(retItem);

                FinishSequence();                
            }
            else
            {
                Caster.SendLocalizedMessage(501857); // This spell won't work on that!
            }
        }
        /*** ADD_END ***/

		public class InternalTarget : Target
		{
			private TelekinesisSpell m_Owner;

			public InternalTarget( TelekinesisSpell owner ) : base( 12, false, TargetFlags.None )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
                /*** ADD_START ***/
                // i gold dentro un container se spostati con telecinesi creano un bug che duplica migliaia di gp
                if (o is Item && !(o is Container) && !(((Item)o).RootParent is Container) && ((Item)o).Movable)
                    m_Owner.Target( (Item)o );
                else
                    from.SendLocalizedMessage(501857); // This spell won't work on that!
                /*** ADD_END ***/

                /*** DEL_START ***/
                /*
				if ( o is ITelekinesisable )
					m_Owner.Target( (ITelekinesisable)o );
				else if ( o is Container )
					m_Owner.Target( (Container)o );
				else
					from.SendLocalizedMessage( 501857 ); // This spell won't work on that!
                */
                /*** DEL_END ***/
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}

namespace Server
{
	public interface ITelekinesisable : IPoint3D
	{
		void OnTelekinesis( Mobile from );
	}
}