using System;
using UnityEngine;

// Token: 0x02001A8F RID: 6799
public class BuildingCellVisualizerResources : ScriptableObject
{
	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x06008E15 RID: 36373 RVA: 0x000FCCC0 File Offset: 0x000FAEC0
	public string heatSourceAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06008E16 RID: 36374 RVA: 0x000FCCC7 File Offset: 0x000FAEC7
	public string heatAnimName
	{
		get
		{
			return "heatfx_a";
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x06008E17 RID: 36375 RVA: 0x000FCCC0 File Offset: 0x000FAEC0
	public string heatSinkAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x06008E18 RID: 36376 RVA: 0x000FCCCE File Offset: 0x000FAECE
	public string heatSinkAnimName
	{
		get
		{
			return "heatfx_b";
		}
	}

	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x06008E19 RID: 36377 RVA: 0x000FCCD5 File Offset: 0x000FAED5
	// (set) Token: 0x06008E1A RID: 36378 RVA: 0x000FCCDD File Offset: 0x000FAEDD
	public Material backgroundMaterial { get; set; }

	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x06008E1B RID: 36379 RVA: 0x000FCCE6 File Offset: 0x000FAEE6
	// (set) Token: 0x06008E1C RID: 36380 RVA: 0x000FCCEE File Offset: 0x000FAEEE
	public Material iconBackgroundMaterial { get; set; }

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x06008E1D RID: 36381 RVA: 0x000FCCF7 File Offset: 0x000FAEF7
	// (set) Token: 0x06008E1E RID: 36382 RVA: 0x000FCCFF File Offset: 0x000FAEFF
	public Material powerInputMaterial { get; set; }

	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x06008E1F RID: 36383 RVA: 0x000FCD08 File Offset: 0x000FAF08
	// (set) Token: 0x06008E20 RID: 36384 RVA: 0x000FCD10 File Offset: 0x000FAF10
	public Material powerOutputMaterial { get; set; }

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x06008E21 RID: 36385 RVA: 0x000FCD19 File Offset: 0x000FAF19
	// (set) Token: 0x06008E22 RID: 36386 RVA: 0x000FCD21 File Offset: 0x000FAF21
	public Material liquidInputMaterial { get; set; }

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06008E23 RID: 36387 RVA: 0x000FCD2A File Offset: 0x000FAF2A
	// (set) Token: 0x06008E24 RID: 36388 RVA: 0x000FCD32 File Offset: 0x000FAF32
	public Material liquidOutputMaterial { get; set; }

	// Token: 0x1700096F RID: 2415
	// (get) Token: 0x06008E25 RID: 36389 RVA: 0x000FCD3B File Offset: 0x000FAF3B
	// (set) Token: 0x06008E26 RID: 36390 RVA: 0x000FCD43 File Offset: 0x000FAF43
	public Material gasInputMaterial { get; set; }

	// Token: 0x17000970 RID: 2416
	// (get) Token: 0x06008E27 RID: 36391 RVA: 0x000FCD4C File Offset: 0x000FAF4C
	// (set) Token: 0x06008E28 RID: 36392 RVA: 0x000FCD54 File Offset: 0x000FAF54
	public Material gasOutputMaterial { get; set; }

	// Token: 0x17000971 RID: 2417
	// (get) Token: 0x06008E29 RID: 36393 RVA: 0x000FCD5D File Offset: 0x000FAF5D
	// (set) Token: 0x06008E2A RID: 36394 RVA: 0x000FCD65 File Offset: 0x000FAF65
	public Material highEnergyParticleInputMaterial { get; set; }

	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x06008E2B RID: 36395 RVA: 0x000FCD6E File Offset: 0x000FAF6E
	// (set) Token: 0x06008E2C RID: 36396 RVA: 0x000FCD76 File Offset: 0x000FAF76
	public Material highEnergyParticleOutputMaterial { get; set; }

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06008E2D RID: 36397 RVA: 0x000FCD7F File Offset: 0x000FAF7F
	// (set) Token: 0x06008E2E RID: 36398 RVA: 0x000FCD87 File Offset: 0x000FAF87
	public Mesh backgroundMesh { get; set; }

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06008E2F RID: 36399 RVA: 0x000FCD90 File Offset: 0x000FAF90
	// (set) Token: 0x06008E30 RID: 36400 RVA: 0x000FCD98 File Offset: 0x000FAF98
	public Mesh iconMesh { get; set; }

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x06008E31 RID: 36401 RVA: 0x000FCDA1 File Offset: 0x000FAFA1
	// (set) Token: 0x06008E32 RID: 36402 RVA: 0x000FCDA9 File Offset: 0x000FAFA9
	public int backgroundLayer { get; set; }

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x06008E33 RID: 36403 RVA: 0x000FCDB2 File Offset: 0x000FAFB2
	// (set) Token: 0x06008E34 RID: 36404 RVA: 0x000FCDBA File Offset: 0x000FAFBA
	public int iconLayer { get; set; }

	// Token: 0x06008E35 RID: 36405 RVA: 0x000FCDC3 File Offset: 0x000FAFC3
	public static void DestroyInstance()
	{
		BuildingCellVisualizerResources._Instance = null;
	}

	// Token: 0x06008E36 RID: 36406 RVA: 0x000FCDCB File Offset: 0x000FAFCB
	public static BuildingCellVisualizerResources Instance()
	{
		if (BuildingCellVisualizerResources._Instance == null)
		{
			BuildingCellVisualizerResources._Instance = Resources.Load<BuildingCellVisualizerResources>("BuildingCellVisualizerResources");
			BuildingCellVisualizerResources._Instance.Initialize();
		}
		return BuildingCellVisualizerResources._Instance;
	}

	// Token: 0x06008E37 RID: 36407 RVA: 0x0036FC24 File Offset: 0x0036DE24
	private void Initialize()
	{
		Shader shader = Shader.Find("Klei/BuildingCell");
		this.backgroundMaterial = new Material(shader);
		this.backgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.iconBackgroundMaterial = new Material(shader);
		this.iconBackgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.powerInputMaterial = new Material(shader);
		this.powerOutputMaterial = new Material(shader);
		this.liquidInputMaterial = new Material(shader);
		this.liquidOutputMaterial = new Material(shader);
		this.gasInputMaterial = new Material(shader);
		this.gasOutputMaterial = new Material(shader);
		this.highEnergyParticleInputMaterial = new Material(shader);
		this.highEnergyParticleOutputMaterial = new Material(shader);
		this.backgroundMesh = this.CreateMesh("BuildingCellVisualizer", Vector2.zero, 0.5f);
		float num = 0.5f;
		this.iconMesh = this.CreateMesh("BuildingCellVisualizerIcon", Vector2.zero, num * 0.5f);
		this.backgroundLayer = LayerMask.NameToLayer("Default");
		this.iconLayer = LayerMask.NameToLayer("Place");
	}

	// Token: 0x06008E38 RID: 36408 RVA: 0x0036FD3C File Offset: 0x0036DF3C
	private Mesh CreateMesh(string name, Vector2 base_offset, float half_size)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		mesh.vertices = new Vector3[]
		{
			new Vector3(-half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(-half_size + base_offset.x, half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, half_size + base_offset.y, 0f)
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x04006AF4 RID: 27380
	[Header("Electricity")]
	public Color electricityInputColor;

	// Token: 0x04006AF5 RID: 27381
	public Color electricityOutputColor;

	// Token: 0x04006AF6 RID: 27382
	public Sprite electricityInputIcon;

	// Token: 0x04006AF7 RID: 27383
	public Sprite electricityOutputIcon;

	// Token: 0x04006AF8 RID: 27384
	public Sprite electricityConnectedIcon;

	// Token: 0x04006AF9 RID: 27385
	public Sprite electricityBridgeIcon;

	// Token: 0x04006AFA RID: 27386
	public Sprite electricityBridgeConnectedIcon;

	// Token: 0x04006AFB RID: 27387
	public Sprite electricityArrowIcon;

	// Token: 0x04006AFC RID: 27388
	public Sprite switchIcon;

	// Token: 0x04006AFD RID: 27389
	public Color32 switchColor;

	// Token: 0x04006AFE RID: 27390
	public Color32 switchOffColor = Color.red;

	// Token: 0x04006AFF RID: 27391
	[Header("Gas")]
	public Sprite gasInputIcon;

	// Token: 0x04006B00 RID: 27392
	public Sprite gasOutputIcon;

	// Token: 0x04006B01 RID: 27393
	public BuildingCellVisualizerResources.IOColours gasIOColours;

	// Token: 0x04006B02 RID: 27394
	[Header("Liquid")]
	public Sprite liquidInputIcon;

	// Token: 0x04006B03 RID: 27395
	public Sprite liquidOutputIcon;

	// Token: 0x04006B04 RID: 27396
	public BuildingCellVisualizerResources.IOColours liquidIOColours;

	// Token: 0x04006B05 RID: 27397
	[Header("High Energy Particle")]
	public Sprite highEnergyParticleInputIcon;

	// Token: 0x04006B06 RID: 27398
	public Sprite[] highEnergyParticleOutputIcons;

	// Token: 0x04006B07 RID: 27399
	public Color highEnergyParticleInputColour;

	// Token: 0x04006B08 RID: 27400
	public Color highEnergyParticleOutputColour;

	// Token: 0x04006B09 RID: 27401
	[Header("Heat Sources and Sinks")]
	public Sprite heatSourceIcon;

	// Token: 0x04006B0A RID: 27402
	public Sprite heatSinkIcon;

	// Token: 0x04006B0B RID: 27403
	[Header("Alternate IO Colours")]
	public BuildingCellVisualizerResources.IOColours alternateIOColours;

	// Token: 0x04006B1A RID: 27418
	private static BuildingCellVisualizerResources _Instance;

	// Token: 0x02001A90 RID: 6800
	[Serializable]
	public struct ConnectedDisconnectedColours
	{
		// Token: 0x04006B1B RID: 27419
		public Color32 connected;

		// Token: 0x04006B1C RID: 27420
		public Color32 disconnected;
	}

	// Token: 0x02001A91 RID: 6801
	[Serializable]
	public struct IOColours
	{
		// Token: 0x04006B1D RID: 27421
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours input;

		// Token: 0x04006B1E RID: 27422
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours output;
	}
}
