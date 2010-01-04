using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells
{
	public abstract class MagerySpell : Spell
	{
		public MagerySpell( Mobile caster, Item scroll, SpellInfo info )
			: base( caster, scroll, info )
		{
		}

		public abstract SpellCircle Circle { get; }

		public override bool ConsumeReagents()
		{
			if( base.ConsumeReagents() )
				return true;

			if( ArcaneGem.ConsumeCharges( Caster, (Core.SE ? 1 : 1 + (int)Circle) ) )
				return true;

			return false;
		}

		/*** MOD_START ***/
		/*
		private const double ChanceOffset = 20.0, ChanceLength = 100.0 / 7.0;
		*/
		private const double ChanceOffset = 12.5, ChanceLength = 100.0 / 8.0;
		/*** MOD_END ***/

		public override void GetCastSkills( out double min, out double max )
		{
			int circle = (int)Circle;

			if( Scroll != null )
				circle -= 2;

			double avg = ChanceLength * circle;

			min = avg - ChanceOffset;
			max = avg + ChanceOffset;
		}

		private static int[] m_ManaTable = new int[] { 4, 6, 9, 11, 14, 20, 40, 50 };

		public override int GetMana()
		{
			if( Scroll is BaseWand )
				return 0;

			return m_ManaTable[(int)Circle];
		}

		public override double GetResistSkill( Mobile m )
		{
			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if( m.Skills[SkillName.MagicResist].Value < maxSkill )
				m.CheckSkill( SkillName.MagicResist, 0.0, m.Skills[SkillName.MagicResist].Cap );

			return m.Skills[SkillName.MagicResist].Value;
		}

		public virtual bool CheckResisted( Mobile target )
		{
			double n = GetResistPercent( target );

			n /= 100.0;

			if( n <= 0.0 )
				return false;

			if( n >= 1.0 )
				return true;

			int maxSkill = (1 + (int)Circle) * 10;
			maxSkill += (1 + ((int)Circle / 6)) * 25;

			if( target.Skills[SkillName.MagicResist].Value < maxSkill )
				target.CheckSkill( SkillName.MagicResist, 0.0, target.Skills[SkillName.MagicResist].Cap );

			return (n >= Utility.RandomDouble());
		}

		public virtual double GetResistPercentForCircle( Mobile target, SpellCircle circle )
		{
			double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
			double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

			return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
		}

		public virtual double GetResistPercent( Mobile target )
		{
			return GetResistPercentForCircle( target, Circle );
		}

		public override TimeSpan GetCastDelay()
		{
			if( Scroll is BaseWand )
				return TimeSpan.Zero;

			if( !Core.AOS )
				return TimeSpan.FromSeconds( 0.5 + (0.25 * (int)Circle) );

			return base.GetCastDelay();
		}

		public override TimeSpan CastDelayBase
		{
			get
			{
				return TimeSpan.FromSeconds( (3 + (int)Circle) * CastDelaySecondsPerTick );
			}
		}
		
		/*** ADD_START ***/
		public override bool CheckFizzle()
		{
			if ( Scroll is BaseWand )
				return true;

			double minSkill, maxSkill;

			GetCastSkills( out minSkill, out maxSkill );

			if ( DamageSkill != CastSkill )
				Caster.CheckSkill( DamageSkill, 0.0, Caster.Skills[ DamageSkill ].Cap );

			if ( Caster.CheckSkill( CastSkill, minSkill, maxSkill ) == false )
			{
				return false;
			}
			
			ArmorMaterialType armorType = ArmorMaterialType.Cloth;
			
			if ( Caster is PlayerMobile )
			{
				foreach ( Item item in Caster.Items )
				{
					if ( item is BaseArmor )
					{
						if ( ((BaseArmor) item).MaterialType == ArmorMaterialType.Plate )
						{
							armorType = ((BaseArmor) item).MaterialType;
							break;
						}
						else if ( ((BaseArmor) item).MaterialType == ArmorMaterialType.Dragon && armorType < ArmorMaterialType.Dragon )
						{
							armorType = ((BaseArmor) item).MaterialType;
						}
						else if ( ((BaseArmor) item).MaterialType == ArmorMaterialType.Chainmail && armorType < ArmorMaterialType.Chainmail )
						{
							armorType = ((BaseArmor) item).MaterialType;
						}
						else if ( ((BaseArmor) item).MaterialType == ArmorMaterialType.Ringmail && armorType < ArmorMaterialType.Ringmail )
						{
							armorType = ((BaseArmor) item).MaterialType;
						}
						else if ( ((BaseArmor) item).MaterialType == ArmorMaterialType.Bone && armorType < ArmorMaterialType.Bone )
						{
							armorType = ((BaseArmor) item).MaterialType;
						}
						else if ( ((BaseArmor) item).MaterialType == ArmorMaterialType.Studded && armorType < ArmorMaterialType.Studded )
						{
							armorType = ((BaseArmor) item).MaterialType;
						}
						else if ( ((BaseArmor) item).MaterialType == ArmorMaterialType.Leather && armorType < ArmorMaterialType.Leather )
						{
							armorType = ((BaseArmor) item).MaterialType;
						}
					}
				}
			}
			
			int chance = 0;
			
			switch ( armorType )
			{
				case ArmorMaterialType.Cloth:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 100;
							break;
						case SpellCircle.Second:
							chance = 100;
							break;
						case SpellCircle.Third:
							chance = 100;
							break;
						case SpellCircle.Fourth:
							chance = 100;
							break;
						case SpellCircle.Fifth:
							chance = 100;
							break;
						case SpellCircle.Sixth:
							chance = 100;
							break;
						case SpellCircle.Seventh:
							chance = 100;
							break;
						case SpellCircle.Eighth:
							chance = 80;
							break;
					}
					break;
				case ArmorMaterialType.Leather:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 100;
							break;
						case SpellCircle.Second:
							chance = 100;
							break;
						case SpellCircle.Third:
							chance = 100;
							break;
						case SpellCircle.Fourth:
							chance = 100;
							break;
						case SpellCircle.Fifth:
							chance = 100;
							break;
						case SpellCircle.Sixth:
							chance = 100;
							break;
						case SpellCircle.Seventh:
							chance = 80;
							break;
						case SpellCircle.Eighth:
							chance = 60;
							break;
					}
					break;
				case ArmorMaterialType.Studded:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 100;
							break;
						case SpellCircle.Second:
							chance = 100;
							break;
						case SpellCircle.Third:
							chance = 100;
							break;
						case SpellCircle.Fourth:
							chance = 100;
							break;
						case SpellCircle.Fifth:
							chance = 100;
							break;
						case SpellCircle.Sixth:
							chance = 80;
							break;
						case SpellCircle.Seventh:
							chance = 60;
							break;
						case SpellCircle.Eighth:
							chance = 40;
							break;
					}
					break;
				case ArmorMaterialType.Bone:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 100;
							break;
						case SpellCircle.Second:
							chance = 100;
							break;
						case SpellCircle.Third:
							chance = 100;
							break;
						case SpellCircle.Fourth:
							chance = 100;
							break;
						case SpellCircle.Fifth:
							chance = 80;
							break;
						case SpellCircle.Sixth:
							chance = 60;
							break;
						case SpellCircle.Seventh:
							chance = 40;
							break;
						case SpellCircle.Eighth:
							chance = 20;
							break;
					}
					break;
				case ArmorMaterialType.Ringmail:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 100;
							break;
						case SpellCircle.Second:
							chance = 100;
							break;
						case SpellCircle.Third:
							chance = 100;
							break;
						case SpellCircle.Fourth:
							chance = 80;
							break;
						case SpellCircle.Fifth:
							chance = 60;
							break;
						case SpellCircle.Sixth:
							chance = 40;
							break;
						case SpellCircle.Seventh:
							chance = 20;
							break;
						case SpellCircle.Eighth:
							chance = 0;
							break;
					}
					break;
				case ArmorMaterialType.Chainmail:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 100;
							break;
						case SpellCircle.Second:
							chance = 100;
							break;
						case SpellCircle.Third:
							chance = 80;
							break;
						case SpellCircle.Fourth:
							chance = 60;
							break;
						case SpellCircle.Fifth:
							chance = 40;
							break;
						case SpellCircle.Sixth:
							chance = 20;
							break;
						case SpellCircle.Seventh:
							chance = 0;
							break;
						case SpellCircle.Eighth:
							chance = 0;
							break;
					}
					break;
				case ArmorMaterialType.Dragon:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 100;
							break;
						case SpellCircle.Second:
							chance = 80;
							break;
						case SpellCircle.Third:
							chance = 60;
							break;
						case SpellCircle.Fourth:
							chance = 40;
							break;
						case SpellCircle.Fifth:
							chance = 20;
							break;
						case SpellCircle.Sixth:
							chance = 0;
							break;
						case SpellCircle.Seventh:
							chance = 0;
							break;
						case SpellCircle.Eighth:
							chance = 0;
							break;
					}
					break;
				case ArmorMaterialType.Plate:
					switch ( Circle )
					{
						case SpellCircle.First:
							chance = 80;
							break;
						case SpellCircle.Second:
							chance = 60;
							break;
						case SpellCircle.Third:
							chance = 40;
							break;
						case SpellCircle.Fourth:
							chance = 20;
							break;
						case SpellCircle.Fifth:
							chance = 0;
							break;
						case SpellCircle.Sixth:
							chance = 0;
							break;
						case SpellCircle.Seventh:
							chance = 0;
							break;
						case SpellCircle.Eighth:
							chance = 0;
							break;
					}
					break;
				}
				
			return chance > Utility.Random(100);
		}
		/*** ADD_END ***/
	}
}
