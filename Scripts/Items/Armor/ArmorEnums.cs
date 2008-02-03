using System;

namespace Server.Items
{
	public enum ArmorQuality
	{
		Low,
		Regular,
		Exceptional
	}

	public enum ArmorDurabilityLevel
	{
		Regular,
		Durable,
		Substantial,
		Massive,
		Fortified,
		Indestructible
	}

	public enum ArmorProtectionLevel
	{
		Regular,
		Defense,
		Guarding,
		Hardening,
		Fortification,
		Invulnerability,
	}

	public enum ArmorBodyType
	{
		Gorget,
		Gloves,
		Helmet,
		Arms,
		Legs, 
		Chest,
		Shield
	}

	public enum ArmorMaterialType
	{
		Cloth,
		Leather,
		Studded,
		Bone,
		Spined,
		Horned,
		Barbed,
		Ringmail,
		Chainmail,
		Plate,
		Dragon	// On OSI, Dragon is seen and considered its own type.
	}

	public enum ArmorMeditationAllowance
	{
		/*** MOD_START ***/
		
		/*
		All,
		Half,
		None
		*/
		
		All,
		P75,
		P50,
		P25,
		None
			
		/*** MOD_END ***/
	}
}
