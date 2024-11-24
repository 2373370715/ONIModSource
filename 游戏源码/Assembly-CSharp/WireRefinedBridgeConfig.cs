using System;
using TUNING;
using UnityEngine;

// Token: 0x020005E9 RID: 1513
public class WireRefinedBridgeConfig : WireBridgeConfig
{
	// Token: 0x06001B5D RID: 7005 RVA: 0x000B1D6C File Offset: 0x000AFF6C
	protected override string GetID()
	{
		return "WireRefinedBridge";
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x001AB4C0 File Offset: 0x001A96C0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("utilityelectricbridgeconductive_kanim")
		};
		buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
		return buildingDef;
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x000B1D73 File Offset: 0x000AFF73
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max2000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x0400113E RID: 4414
	public new const string ID = "WireRefinedBridge";
}
