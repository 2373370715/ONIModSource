using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200002C RID: 44
public abstract class BaseWireConfig : IBuildingConfig
{
	// Token: 0x060000B6 RID: 182
	public abstract override BuildingDef CreateBuildingDef();

	// Token: 0x060000B7 RID: 183 RVA: 0x00141098 File Offset: 0x0013F298
	public BuildingDef CreateBuildingDef(string id, string anim, float construction_time, float[] construction_mass, float insulation, EffectorValues decor, EffectorValues noise)
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 1, anim, 10, construction_time, construction_mass, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.Anywhere, decor, noise, 0.2f);
		buildingDef.ThermalConductivity = insulation;
		BuildingDef buildingDef2;
		(buildingDef2 = buildingDef).Floodable = false;
		BuildingDef buildingDef3 = buildingDef2;
		buildingDef3.Overheatable = false;
		buildingDef3.Entombable = false;
		buildingDef3.ViewMode = OverlayModes.Power.ID;
		buildingDef3.ObjectLayer = ObjectLayer.Wire;
		buildingDef3.TileLayer = ObjectLayer.WireTile;
		buildingDef3.ReplacementLayer = ObjectLayer.ReplacementWire;
		buildingDef3.AudioCategory = "Metal";
		buildingDef3.AudioSize = "small";
		buildingDef3.BaseTimeUntilRepair = -1f;
		buildingDef3.SceneLayer = Grid.SceneLayer.Wires;
		buildingDef3.isKAnimTile = true;
		buildingDef3.isUtility = true;
		buildingDef3.DragBuild = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, id);
		return buildingDef3;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000A60E2 File Offset: 0x000A42E2
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<Wire>();
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.isPhysicalBuilding = true;
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Electrical;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000A6119 File Offset: 0x000A4319
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().isDiggingRequired = false;
		KAnimGraphTileVisualizer kanimGraphTileVisualizer = go.AddOrGet<KAnimGraphTileVisualizer>();
		kanimGraphTileVisualizer.isPhysicalBuilding = false;
		kanimGraphTileVisualizer.connectionSource = KAnimGraphTileVisualizer.ConnectionSource.Electrical;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00141154 File Offset: 0x0013F354
	protected void DoPostConfigureComplete(Wire.WattageRating rating, GameObject go)
	{
		go.GetComponent<Wire>().MaxWattageRating = rating;
		float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(rating);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.MAX_WATTAGE, GameUtil.GetFormattedWattage(maxWattageAsFloat, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MAX_WATTAGE, Array.Empty<object>()), Descriptor.DescriptorType.Effect);
		BuildingDef def = go.GetComponent<Building>().Def;
		if (def.EffectDescription == null)
		{
			def.EffectDescription = new List<Descriptor>();
		}
		def.EffectDescription.Add(item);
	}
}
