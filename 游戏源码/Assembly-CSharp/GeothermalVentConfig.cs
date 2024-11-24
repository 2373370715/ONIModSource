using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200036B RID: 875
public class GeothermalVentConfig : IEntityConfig
{
	// Token: 0x06000E30 RID: 3632 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0017765C File Offset: 0x0017585C
	public virtual GameObject CreatePrefab()
	{
		string id = "GeothermalVentEntity";
		string name = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geospout_kanim"), "off", Grid.SceneLayer.BuildingBack, 3, 4, tier, tier2, SimHashes.Unobtanium, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddComponent<GeothermalVent>();
		gameObject.AddComponent<GeothermalPlantComponent>();
		gameObject.AddComponent<Operational>();
		gameObject.AddComponent<UserNameable>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showCapacityAsMainStatus = false;
		storage.showCapacityStatusItem = false;
		storage.showDescriptor = false;
		return gameObject;
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x00177730 File Offset: 0x00175930
	public void OnPrefabInit(GameObject inst)
	{
		LogicPorts logicPorts = inst.AddOrGet<LogicPorts>();
		logicPorts.inputPortInfo = new LogicPorts.Port[0];
		logicPorts.outputPortInfo = new LogicPorts.Port[]
		{
			LogicPorts.Port.OutputPort("GEOTHERMAL_VENT_STATUS_PORT", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GEOTHERMALVENT.LOGIC_PORT_INACTIVE, false, false)
		};
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A35 RID: 2613
	public const string ID = "GeothermalVentEntity";

	// Token: 0x04000A36 RID: 2614
	public const string OUTPUT_LOGIC_PORT_ID = "GEOTHERMAL_VENT_STATUS_PORT";

	// Token: 0x04000A37 RID: 2615
	private const string ANIM_FILE = "gravitas_geospout_kanim";

	// Token: 0x04000A38 RID: 2616
	public const string OFFLINE_ANIM = "off";

	// Token: 0x04000A39 RID: 2617
	public const string QUEST_ENTOMBED_ANIM = "pooped";

	// Token: 0x04000A3A RID: 2618
	public const string IDLE_ANIM = "on";

	// Token: 0x04000A3B RID: 2619
	public const string OBSTRUCTED_ANIM = "over_pressure";

	// Token: 0x04000A3C RID: 2620
	public const int EMISSION_RANGE = 1;

	// Token: 0x04000A3D RID: 2621
	public const float EMISSION_INTERVAL_SEC = 0.2f;

	// Token: 0x04000A3E RID: 2622
	public const float EMISSION_MAX_PRESSURE_KG = 120f;

	// Token: 0x04000A3F RID: 2623
	public const float EMISSION_MAX_RATE_PER_TICK = 3f;

	// Token: 0x04000A40 RID: 2624
	public static string TOGGLE_ANIMATION = "working_loop";

	// Token: 0x04000A41 RID: 2625
	public static HashedString TOGGLE_ANIM_OVERRIDE = "anim_interacts_geospout_kanim";

	// Token: 0x04000A42 RID: 2626
	public const float TOGGLE_CHORE_DURATION_SECONDS = 10f;

	// Token: 0x04000A43 RID: 2627
	public static MathUtil.MinMax INITIAL_DEBRIS_VELOCIOTY = new MathUtil.MinMax(1f, 5f);

	// Token: 0x04000A44 RID: 2628
	public static MathUtil.MinMax INITIAL_DEBRIS_ANGLE = new MathUtil.MinMax(200f, 340f);

	// Token: 0x04000A45 RID: 2629
	public static MathUtil.MinMax DEBRIS_MASS_KG = new MathUtil.MinMax(30f, 34f);

	// Token: 0x04000A46 RID: 2630
	public const string BAROMETER_ANIM = "meter";

	// Token: 0x04000A47 RID: 2631
	public const string BAROMETER_TARGET = "meter_target";

	// Token: 0x04000A48 RID: 2632
	public static string[] BAROMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x04000A49 RID: 2633
	public const string CONNECTED_ANIM = "meter_connected";

	// Token: 0x04000A4A RID: 2634
	public const string CONNECTED_TARGET = "meter_connected_target";

	// Token: 0x04000A4B RID: 2635
	public static string[] CONNECTED_SYMBOLS = new string[]
	{
		"meter_connected_target"
	};

	// Token: 0x04000A4C RID: 2636
	public const float CONNECTED_PROGRESS = 1f;

	// Token: 0x04000A4D RID: 2637
	public const float DISCONNECTED_PROGRESS = 0f;
}
