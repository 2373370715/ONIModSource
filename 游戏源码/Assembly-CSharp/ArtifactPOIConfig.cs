using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class ArtifactPOIConfig : IMultiEntityConfig
{
	// Token: 0x0600124C RID: 4684 RVA: 0x00188834 File Offset: 0x00186A34
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (ArtifactPOIConfig.ArtifactPOIParams artifactPOIParams in this.GenerateConfigs())
		{
			list.Add(ArtifactPOIConfig.CreateArtifactPOI(artifactPOIParams.id, artifactPOIParams.anim, Strings.Get(artifactPOIParams.nameStringKey), Strings.Get(artifactPOIParams.descStringKey), artifactPOIParams.poiType.idHash));
		}
		return list;
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x001888CC File Offset: 0x00186ACC
	public static GameObject CreateArtifactPOI(string id, string anim, string name, string desc, HashedString poiType)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, id, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ArtifactPOIConfigurator>().presetType = poiType;
		ArtifactPOIClusterGridEntity artifactPOIClusterGridEntity = gameObject.AddOrGet<ArtifactPOIClusterGridEntity>();
		artifactPOIClusterGridEntity.m_name = name;
		artifactPOIClusterGridEntity.m_Anim = anim;
		gameObject.AddOrGetDef<ArtifactPOIStates.Def>();
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextSpaceEntry));
		return gameObject;
	}

	// Token: 0x0600124E RID: 4686 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x00188924 File Offset: 0x00186B24
	private List<ArtifactPOIConfig.ArtifactPOIParams> GenerateConfigs()
	{
		List<ArtifactPOIConfig.ArtifactPOIParams> list = new List<ArtifactPOIConfig.ArtifactPOIParams>();
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_1", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation1", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_2", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation2", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_3", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation3", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_4", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation4", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_5", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation5", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_6", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation6", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_7", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation7", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_8", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation8", null, false, 30000f, 60000f, "EXPANSION1_ID")));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("russels_teapot", new ArtifactPOIConfigurator.ArtifactPOIType("RussellsTeapot", "artifact_TeaPot", true, 30000f, 60000f, "EXPANSION1_ID")));
		list.RemoveAll((ArtifactPOIConfig.ArtifactPOIParams poi) => !poi.poiType.dlcID.IsNullOrWhiteSpace() && !DlcManager.IsContentSubscribed(poi.poiType.dlcID));
		return list;
	}

	// Token: 0x04000C7E RID: 3198
	public const string GravitasSpaceStation1 = "GravitasSpaceStation1";

	// Token: 0x04000C7F RID: 3199
	public const string GravitasSpaceStation2 = "GravitasSpaceStation2";

	// Token: 0x04000C80 RID: 3200
	public const string GravitasSpaceStation3 = "GravitasSpaceStation3";

	// Token: 0x04000C81 RID: 3201
	public const string GravitasSpaceStation4 = "GravitasSpaceStation4";

	// Token: 0x04000C82 RID: 3202
	public const string GravitasSpaceStation5 = "GravitasSpaceStation5";

	// Token: 0x04000C83 RID: 3203
	public const string GravitasSpaceStation6 = "GravitasSpaceStation6";

	// Token: 0x04000C84 RID: 3204
	public const string GravitasSpaceStation7 = "GravitasSpaceStation7";

	// Token: 0x04000C85 RID: 3205
	public const string GravitasSpaceStation8 = "GravitasSpaceStation8";

	// Token: 0x04000C86 RID: 3206
	public const string RussellsTeapot = "RussellsTeapot";

	// Token: 0x02000433 RID: 1075
	public struct ArtifactPOIParams
	{
		// Token: 0x06001252 RID: 4690 RVA: 0x00188AE4 File Offset: 0x00186CE4
		public ArtifactPOIParams(string anim, ArtifactPOIConfigurator.ArtifactPOIType poiType)
		{
			this.id = "ArtifactSpacePOI_" + poiType.id;
			this.anim = anim;
			this.nameStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".DESC");
			this.poiType = poiType;
		}

		// Token: 0x04000C87 RID: 3207
		public string id;

		// Token: 0x04000C88 RID: 3208
		public string anim;

		// Token: 0x04000C89 RID: 3209
		public StringKey nameStringKey;

		// Token: 0x04000C8A RID: 3210
		public StringKey descStringKey;

		// Token: 0x04000C8B RID: 3211
		public ArtifactPOIConfigurator.ArtifactPOIType poiType;
	}
}
