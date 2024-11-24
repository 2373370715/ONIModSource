using System;
using Klei.AI;

[Serializable]
public struct SpiceInstance
{
	public Tag Id;

	public float TotalKG;

	public AttributeModifier CalorieModifier => SpiceGrinder.SettingOptions[Id].Spice.CalorieModifier;

	public AttributeModifier FoodModifier => SpiceGrinder.SettingOptions[Id].Spice.FoodModifier;

	public Effect StatBonus => SpiceGrinder.SettingOptions[Id].StatBonus;
}
