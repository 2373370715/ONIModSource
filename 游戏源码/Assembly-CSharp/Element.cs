using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using STRINGS;

// Token: 0x0200127A RID: 4730
[DebuggerDisplay("{name}")]
[Serializable]
public class Element : IComparable<Element>
{
	// Token: 0x060060F0 RID: 24816 RVA: 0x000DF38C File Offset: 0x000DD58C
	public float PressureToMass(float pressure)
	{
		return pressure / this.defaultValues.pressure;
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x060060F1 RID: 24817 RVA: 0x000DF39B File Offset: 0x000DD59B
	public bool IsSlippery
	{
		get
		{
			return this.HasTag(GameTags.Slippery);
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x060060F2 RID: 24818 RVA: 0x000DF3A8 File Offset: 0x000DD5A8
	public bool IsUnstable
	{
		get
		{
			return this.HasTag(GameTags.Unstable);
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x060060F3 RID: 24819 RVA: 0x000DF3B5 File Offset: 0x000DD5B5
	public bool IsLiquid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Liquid;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x060060F4 RID: 24820 RVA: 0x000DF3C2 File Offset: 0x000DD5C2
	public bool IsGas
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Gas;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x060060F5 RID: 24821 RVA: 0x000DF3CF File Offset: 0x000DD5CF
	public bool IsSolid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Solid;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060060F6 RID: 24822 RVA: 0x000DF3DC File Offset: 0x000DD5DC
	public bool IsVacuum
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Vacuum;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060060F7 RID: 24823 RVA: 0x000DF3E9 File Offset: 0x000DD5E9
	public bool IsTemperatureInsulated
	{
		get
		{
			return (this.state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
		}
	}

	// Token: 0x060060F8 RID: 24824 RVA: 0x000DF3F7 File Offset: 0x000DD5F7
	public bool IsState(Element.State expected_state)
	{
		return (this.state & Element.State.Solid) == expected_state;
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x060060F9 RID: 24825 RVA: 0x000DF404 File Offset: 0x000DD604
	public bool HasTransitionUp
	{
		get
		{
			return this.highTempTransitionTarget != (SimHashes)0 && this.highTempTransitionTarget != SimHashes.Unobtanium && this.highTempTransition != null && this.highTempTransition != this;
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x060060FA RID: 24826 RVA: 0x000DF431 File Offset: 0x000DD631
	// (set) Token: 0x060060FB RID: 24827 RVA: 0x000DF439 File Offset: 0x000DD639
	public string name { get; set; }

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x060060FC RID: 24828 RVA: 0x000DF442 File Offset: 0x000DD642
	// (set) Token: 0x060060FD RID: 24829 RVA: 0x000DF44A File Offset: 0x000DD64A
	public string nameUpperCase { get; set; }

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x060060FE RID: 24830 RVA: 0x000DF453 File Offset: 0x000DD653
	// (set) Token: 0x060060FF RID: 24831 RVA: 0x000DF45B File Offset: 0x000DD65B
	public string description { get; set; }

	// Token: 0x06006100 RID: 24832 RVA: 0x000DF464 File Offset: 0x000DD664
	public string GetStateString()
	{
		return Element.GetStateString(this.state);
	}

	// Token: 0x06006101 RID: 24833 RVA: 0x000DF471 File Offset: 0x000DD671
	public static string GetStateString(Element.State state)
	{
		if ((state & Element.State.Solid) == Element.State.Solid)
		{
			return ELEMENTS.STATE.SOLID;
		}
		if ((state & Element.State.Solid) == Element.State.Liquid)
		{
			return ELEMENTS.STATE.LIQUID;
		}
		if ((state & Element.State.Solid) == Element.State.Gas)
		{
			return ELEMENTS.STATE.GAS;
		}
		return ELEMENTS.STATE.VACUUM;
	}

	// Token: 0x06006102 RID: 24834 RVA: 0x002B138C File Offset: 0x002AF58C
	public string FullDescription(bool addHardnessColor = true)
	{
		string text = this.Description();
		if (this.IsSolid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCSOLID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetHardnessString(this, addHardnessColor));
		}
		else if (this.IsLiquid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCLIQUID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		else if (!this.IsVacuum)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCGAS, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		string text2 = ELEMENTS.THERMALPROPERTIES;
		text2 = text2.Replace("{SPECIFIC_HEAT_CAPACITY}", GameUtil.GetFormattedSHC(this.specificHeatCapacity));
		text2 = text2.Replace("{THERMAL_CONDUCTIVITY}", GameUtil.GetFormattedThermalConductivity(this.thermalConductivity));
		text = text + "\n" + text2;
		if (DlcManager.FeatureRadiationEnabled())
		{
			text = text + "\n" + string.Format(ELEMENTS.RADIATIONPROPERTIES, this.radiationAbsorptionFactor, GameUtil.GetFormattedRads(this.radiationPer1000Mass * 1.1f / 600f, GameUtil.TimeSlice.PerCycle));
		}
		if (this.oreTags.Length != 0 && !this.IsVacuum)
		{
			text += "\n\n";
			string text3 = "";
			for (int i = 0; i < this.oreTags.Length; i++)
			{
				Tag a = new Tag(this.oreTags[i]);
				if (!(a == GameTags.HideFromCodex) && !(a == GameTags.HideFromSpawnTool))
				{
					text3 += a.ProperName();
					if (i < this.oreTags.Length - 1)
					{
						text3 += ", ";
					}
				}
			}
			text += string.Format(ELEMENTS.ELEMENTPROPERTIES, text3);
		}
		if (this.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
			}
		}
		return text;
	}

	// Token: 0x06006103 RID: 24835 RVA: 0x000DF4B0 File Offset: 0x000DD6B0
	public string Description()
	{
		return this.description;
	}

	// Token: 0x06006104 RID: 24836 RVA: 0x000DF4B8 File Offset: 0x000DD6B8
	public bool HasTag(Tag search_tag)
	{
		return this.tag == search_tag || Array.IndexOf<Tag>(this.oreTags, search_tag) != -1;
	}

	// Token: 0x06006105 RID: 24837 RVA: 0x000DF4DC File Offset: 0x000DD6DC
	public Tag GetMaterialCategoryTag()
	{
		return this.materialCategory;
	}

	// Token: 0x06006106 RID: 24838 RVA: 0x000DF4E4 File Offset: 0x000DD6E4
	public int CompareTo(Element other)
	{
		return this.id - other.id;
	}

	// Token: 0x040044D9 RID: 17625
	public const int INVALID_ID = 0;

	// Token: 0x040044DA RID: 17626
	public SimHashes id;

	// Token: 0x040044DB RID: 17627
	public Tag tag;

	// Token: 0x040044DC RID: 17628
	public ushort idx;

	// Token: 0x040044DD RID: 17629
	public float specificHeatCapacity;

	// Token: 0x040044DE RID: 17630
	public float thermalConductivity = 1f;

	// Token: 0x040044DF RID: 17631
	public float molarMass = 1f;

	// Token: 0x040044E0 RID: 17632
	public float strength;

	// Token: 0x040044E1 RID: 17633
	public float flow;

	// Token: 0x040044E2 RID: 17634
	public float maxCompression;

	// Token: 0x040044E3 RID: 17635
	public float viscosity;

	// Token: 0x040044E4 RID: 17636
	public float minHorizontalFlow = float.PositiveInfinity;

	// Token: 0x040044E5 RID: 17637
	public float minVerticalFlow = float.PositiveInfinity;

	// Token: 0x040044E6 RID: 17638
	public float maxMass = 10000f;

	// Token: 0x040044E7 RID: 17639
	public float solidSurfaceAreaMultiplier;

	// Token: 0x040044E8 RID: 17640
	public float liquidSurfaceAreaMultiplier;

	// Token: 0x040044E9 RID: 17641
	public float gasSurfaceAreaMultiplier;

	// Token: 0x040044EA RID: 17642
	public Element.State state;

	// Token: 0x040044EB RID: 17643
	public byte hardness;

	// Token: 0x040044EC RID: 17644
	public float lowTemp;

	// Token: 0x040044ED RID: 17645
	public SimHashes lowTempTransitionTarget;

	// Token: 0x040044EE RID: 17646
	public Element lowTempTransition;

	// Token: 0x040044EF RID: 17647
	public float highTemp;

	// Token: 0x040044F0 RID: 17648
	public SimHashes highTempTransitionTarget;

	// Token: 0x040044F1 RID: 17649
	public Element highTempTransition;

	// Token: 0x040044F2 RID: 17650
	public SimHashes highTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x040044F3 RID: 17651
	public float highTempTransitionOreMassConversion;

	// Token: 0x040044F4 RID: 17652
	public SimHashes lowTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x040044F5 RID: 17653
	public float lowTempTransitionOreMassConversion;

	// Token: 0x040044F6 RID: 17654
	public SimHashes sublimateId;

	// Token: 0x040044F7 RID: 17655
	public SimHashes convertId;

	// Token: 0x040044F8 RID: 17656
	public SpawnFXHashes sublimateFX;

	// Token: 0x040044F9 RID: 17657
	public float sublimateRate;

	// Token: 0x040044FA RID: 17658
	public float sublimateEfficiency;

	// Token: 0x040044FB RID: 17659
	public float sublimateProbability;

	// Token: 0x040044FC RID: 17660
	public float offGasPercentage;

	// Token: 0x040044FD RID: 17661
	public float lightAbsorptionFactor;

	// Token: 0x040044FE RID: 17662
	public float radiationAbsorptionFactor;

	// Token: 0x040044FF RID: 17663
	public float radiationPer1000Mass;

	// Token: 0x04004500 RID: 17664
	public Sim.PhysicsData defaultValues;

	// Token: 0x04004501 RID: 17665
	public float toxicity;

	// Token: 0x04004502 RID: 17666
	public Substance substance;

	// Token: 0x04004503 RID: 17667
	public Tag materialCategory;

	// Token: 0x04004504 RID: 17668
	public int buildMenuSort;

	// Token: 0x04004505 RID: 17669
	public ElementLoader.ElementComposition[] elementComposition;

	// Token: 0x04004506 RID: 17670
	public Tag[] oreTags = new Tag[0];

	// Token: 0x04004507 RID: 17671
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x04004508 RID: 17672
	public bool disabled;

	// Token: 0x04004509 RID: 17673
	public string dlcId;

	// Token: 0x0400450A RID: 17674
	public const byte StateMask = 3;

	// Token: 0x0200127B RID: 4731
	[Serializable]
	public enum State : byte
	{
		// Token: 0x0400450F RID: 17679
		Vacuum,
		// Token: 0x04004510 RID: 17680
		Gas,
		// Token: 0x04004511 RID: 17681
		Liquid,
		// Token: 0x04004512 RID: 17682
		Solid,
		// Token: 0x04004513 RID: 17683
		Unbreakable,
		// Token: 0x04004514 RID: 17684
		Unstable = 8,
		// Token: 0x04004515 RID: 17685
		TemperatureInsulated = 16
	}
}
