using Server.Items;

namespace Server.Mobiles
{
	public abstract class MageTownGuard : BaseTownGuard
	{
		public MageTownGuard() : base( AIType.AI_MageGuard )
		{
		}

		public MageTownGuard( Serial serial ) : base( serial )
		{
		}

		protected override void GenerateWeapon()
		{
			AddItem( new Spellbook( ulong.MaxValue ) );
		}
		
		protected override void SetStats()
		{
			Str = 100;
			Dex = 40;
			Int = 200;
			Hits = HitsMax;
			Stam = StamMax;
			Mana = ManaMax;
		}
		
		protected override void SetSkills()
		{
			SetSkill( SkillName.Magery, 110, 120 );
			SetSkill( SkillName.EvalInt, 80, 100 );
			SetSkill( SkillName.Wrestling, 110, 120 );
			SetSkill( SkillName.DetectHidden, 90, 100 );
			SetSkill( SkillName.MagicResist, 90, 100 );
			SetSkill( SkillName.Focus, 130, 150 );
			SetSkill( SkillName.Meditation, 500, 600 );
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
