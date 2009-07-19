namespace Server.Mobiles
{
	public abstract class WarriorTownGuard : BaseTownGuard
	{
		public WarriorTownGuard() : base( AIType.AI_WarriorGuard )
		{
		}

		public WarriorTownGuard( Serial serial ) : base( serial )
		{
		}

		protected override void SetStats()
		{
			Str = 150;
			Dex = 120;
			Int = 80;
			Hits = HitsMax;
			Stam = StamMax;
			Mana = ManaMax;
		}
		
		protected override void SetSkills()
		{
			SetSkill( SkillName.Swords, 110, 120 );
			SetSkill( SkillName.Macing, 110, 120 );
			SetSkill( SkillName.Fencing, 110, 120 );
			SetSkill( SkillName.Tactics, 80, 100 );
			SetSkill( SkillName.Anatomy, 80, 100 );
			SetSkill( SkillName.DetectHidden, 90, 100 );
			SetSkill( SkillName.MagicResist, 90, 100 );
			SetSkill( SkillName.Focus, 130, 150 );
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
