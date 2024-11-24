using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

// Token: 0x0200186C RID: 6252
[AddComponentMenu("KMonoBehaviour/scripts/SimDebugView")]
public class SimDebugView : KMonoBehaviour
{
	// Token: 0x06008132 RID: 33074 RVA: 0x000F4F81 File Offset: 0x000F3181
	public static void DestroyInstance()
	{
		SimDebugView.Instance = null;
	}

	// Token: 0x06008133 RID: 33075 RVA: 0x000F4F89 File Offset: 0x000F3189
	protected override void OnPrefabInit()
	{
		SimDebugView.Instance = this;
		this.material = UnityEngine.Object.Instantiate<Material>(this.material);
		this.diseaseMaterial = UnityEngine.Object.Instantiate<Material>(this.diseaseMaterial);
	}

	// Token: 0x06008134 RID: 33076 RVA: 0x003371C8 File Offset: 0x003353C8
	protected override void OnSpawn()
	{
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[2].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color3", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[3].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color4", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[4].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color5", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[5].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color6", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[6].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color7", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[7].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[2].colorName));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x06008135 RID: 33077 RVA: 0x00337454 File Offset: 0x00335654
	public void OnReset()
	{
		this.plane = SimDebugView.CreatePlane("SimDebugView", base.transform);
		this.tex = SimDebugView.CreateTexture(out this.texBytes, Grid.WidthInCells, Grid.HeightInCells);
		this.plane.GetComponent<Renderer>().sharedMaterial = this.material;
		this.plane.GetComponent<Renderer>().sharedMaterial.mainTexture = this.tex;
		this.plane.transform.SetLocalPosition(new Vector3(0f, 0f, -6f));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x06008136 RID: 33078 RVA: 0x000F4FB3 File Offset: 0x000F31B3
	public static Texture2D CreateTexture(int width, int height)
	{
		return new Texture2D(width, height)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x06008137 RID: 33079 RVA: 0x000F4FD5 File Offset: 0x000F31D5
	public static Texture2D CreateTexture(out byte[] textureBytes, int width, int height)
	{
		textureBytes = new byte[width * height * 4];
		return new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x06008138 RID: 33080 RVA: 0x003374F4 File Offset: 0x003356F4
	public static Texture2D CreateTexture(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06008139 RID: 33081 RVA: 0x00337534 File Offset: 0x00335734
	public static GameObject CreatePlane(string layer, Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "overlayViewDisplayPlane";
		gameObject.SetLayerRecursively(LayerMask.NameToLayer(layer));
		gameObject.transform.SetParent(parent);
		gameObject.transform.SetPosition(Vector3.zero);
		gameObject.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float y = 2f * (float)Grid.HeightInCells;
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3((float)Grid.WidthInCells, 0f, 0f),
			new Vector3(0f, y, 0f),
			new Vector3(Grid.WidthInMeters, y, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 2f),
			new Vector2(1f, 2f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		Vector2 vector = new Vector2((float)Grid.WidthInCells, y);
		mesh.bounds = new Bounds(new Vector3(0.5f * vector.x, 0.5f * vector.y, 0f), new Vector3(vector.x, vector.y, 0f));
		return gameObject;
	}

	// Token: 0x0600813A RID: 33082 RVA: 0x0033770C File Offset: 0x0033590C
	private void Update()
	{
		if (this.plane == null)
		{
			return;
		}
		bool flag = this.mode != global::OverlayModes.None.ID;
		this.plane.SetActive(flag);
		SimDebugViewCompositor.Instance.Toggle(flag && !GameUtil.IsCapturingTimeLapse());
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds0", new Vector4(0.1f, 0.2f, 0.3f, 0.4f));
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds1", new Vector4(0.5f, 0.6f, 0.7f, 0.8f));
		float x = 0f;
		if (this.mode == global::OverlayModes.ThermalConductivity.ID || this.mode == global::OverlayModes.Temperature.ID)
		{
			x = 1f;
		}
		SimDebugViewCompositor.Instance.material.SetVector("_ThresholdParameters", new Vector4(x, this.thresholdRange, this.thresholdOpacity, 0f));
		if (flag)
		{
			this.UpdateData(this.tex, this.texBytes, this.mode, 192);
		}
	}

	// Token: 0x0600813B RID: 33083 RVA: 0x000F500A File Offset: 0x000F320A
	private static void SetDefaultBilinear(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x0600813C RID: 33084 RVA: 0x000F503A File Offset: 0x000F323A
	private static void SetDefaultPoint(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Point;
	}

	// Token: 0x0600813D RID: 33085 RVA: 0x000F506A File Offset: 0x000F326A
	private static void SetDisease(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.diseaseMaterial;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x0600813E RID: 33086 RVA: 0x00337834 File Offset: 0x00335A34
	public void UpdateData(Texture2D texture, byte[] textureBytes, HashedString viewMode, byte alpha)
	{
		Action<SimDebugView, Texture> action;
		if (!this.dataUpdateFuncs.TryGetValue(viewMode, out action))
		{
			action = new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
		}
		action(this, texture);
		int x;
		int num;
		int x2;
		int num2;
		Grid.GetVisibleExtents(out x, out num, out x2, out num2);
		this.selectedPathProber = null;
		KSelectable selected = SelectTool.Instance.selected;
		if (selected != null)
		{
			this.selectedPathProber = selected.GetComponent<PathProber>();
		}
		this.updateSimViewWorkItems.Reset(new SimDebugView.UpdateSimViewSharedData(this, this.texBytes, viewMode, this));
		int num3 = 16;
		for (int i = num; i <= num2; i += num3)
		{
			int y = Math.Min(i + num3 - 1, num2);
			this.updateSimViewWorkItems.Add(new SimDebugView.UpdateSimViewWorkItem(x, i, x2, y));
		}
		this.currentFrame = Time.frameCount;
		this.selectedCell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		GlobalJobManager.Run(this.updateSimViewWorkItems);
		texture.LoadRawTextureData(textureBytes);
		texture.Apply();
	}

	// Token: 0x0600813F RID: 33087 RVA: 0x000F509A File Offset: 0x000F329A
	public void SetGameGridMode(SimDebugView.GameGridMode mode)
	{
		this.gameGridMode = mode;
	}

	// Token: 0x06008140 RID: 33088 RVA: 0x000F50A3 File Offset: 0x000F32A3
	public SimDebugView.GameGridMode GetGameGridMode()
	{
		return this.gameGridMode;
	}

	// Token: 0x06008141 RID: 33089 RVA: 0x000F50AB File Offset: 0x000F32AB
	public void SetMode(HashedString mode)
	{
		this.mode = mode;
		Game.Instance.gameObject.Trigger(1798162660, mode);
	}

	// Token: 0x06008142 RID: 33090 RVA: 0x000F50CE File Offset: 0x000F32CE
	public HashedString GetMode()
	{
		return this.mode;
	}

	// Token: 0x06008143 RID: 33091 RVA: 0x00337930 File Offset: 0x00335B30
	public static Color TemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, 1f, 1f);
	}

	// Token: 0x06008144 RID: 33092 RVA: 0x0033797C File Offset: 0x00335B7C
	public static Color LiquidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float value = (temperature - minTempExpected) / (maxTempExpected - minTempExpected);
		float num = Mathf.Clamp(value, 0.5f, 1f);
		float s = Mathf.Clamp(value, 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06008145 RID: 33093 RVA: 0x003379D8 File Offset: 0x00335BD8
	public static Color SolidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0.5f, 1f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06008146 RID: 33094 RVA: 0x00337A28 File Offset: 0x00335C28
	public static Color GasTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 0.5f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06008147 RID: 33095 RVA: 0x00337A78 File Offset: 0x00335C78
	public Color NormalizedTemperature(float actualTemperature)
	{
		float num = this.user_temperatureThresholds[0];
		float num2 = this.user_temperatureThresholds[1];
		float num3 = num2 - num;
		if (actualTemperature < num)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName);
		}
		if (actualTemperature > num2)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[this.temperatureThresholds.Length - 1].colorName);
		}
		int num4 = 0;
		float t = 0f;
		Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
		if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
		{
			if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
			{
				float num5 = num;
				for (int i = 0; i < SimDebugView.relativeTemperatureColorIntervals.Length; i++)
				{
					if (actualTemperature < num5 + SimDebugView.relativeTemperatureColorIntervals[i] * num3)
					{
						num4 = i;
						break;
					}
					num5 += SimDebugView.relativeTemperatureColorIntervals[i] * num3;
				}
				t = (actualTemperature - num5) / (SimDebugView.relativeTemperatureColorIntervals[num4] * num3);
			}
		}
		else
		{
			float num6 = num;
			for (int j = 0; j < SimDebugView.absoluteTemperatureColorIntervals.Length; j++)
			{
				if (actualTemperature < num6 + SimDebugView.absoluteTemperatureColorIntervals[j])
				{
					num4 = j;
					break;
				}
				num6 += SimDebugView.absoluteTemperatureColorIntervals[j];
			}
			t = (actualTemperature - num6) / SimDebugView.absoluteTemperatureColorIntervals[num4];
		}
		return Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4 + 1].colorName), t);
	}

	// Token: 0x06008148 RID: 33096 RVA: 0x00337C0C File Offset: 0x00335E0C
	public Color NormalizedHeatFlow(int cell)
	{
		int num = 0;
		int num2 = 0;
		float thermalComfort = GameUtil.GetThermalComfort(GameTags.Minions.Models.Standard, cell, -DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS);
		for (int i = 0; i < this.heatFlowThresholds.Length; i++)
		{
			if (thermalComfort <= this.heatFlowThresholds[i].value)
			{
				num2 = i;
				break;
			}
			num = i;
			num2 = i;
		}
		float num3 = 0f;
		if (num != num2)
		{
			num3 = (thermalComfort - this.heatFlowThresholds[num].value) / (this.heatFlowThresholds[num2].value - this.heatFlowThresholds[num].value);
		}
		num3 = Mathf.Max(num3, 0f);
		num3 = Mathf.Min(num3, 1f);
		Color result = Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num2].colorName), num3);
		if (Grid.Solid[cell])
		{
			result = Color.black;
		}
		return result;
	}

	// Token: 0x06008149 RID: 33097 RVA: 0x000F50D6 File Offset: 0x000F32D6
	private static bool IsInsulated(int cell)
	{
		return (Grid.Element[cell].state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
	}

	// Token: 0x0600814A RID: 33098 RVA: 0x00337D34 File Offset: 0x00335F34
	private static Color GetDiseaseColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.DiseaseIdx[cell] != 255)
		{
			Disease disease = Db.Get().Diseases[(int)Grid.DiseaseIdx[cell]];
			result = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
			result.a = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
		}
		else
		{
			result.a = 0f;
		}
		return result;
	}

	// Token: 0x0600814B RID: 33099 RVA: 0x000F50EA File Offset: 0x000F32EA
	private static Color GetHeatFlowColour(SimDebugView instance, int cell)
	{
		return instance.NormalizedHeatFlow(cell);
	}

	// Token: 0x0600814C RID: 33100 RVA: 0x000F50F3 File Offset: 0x000F32F3
	private static Color GetBlack(SimDebugView instance, int cell)
	{
		return Color.black;
	}

	// Token: 0x0600814D RID: 33101 RVA: 0x00337DB8 File Offset: 0x00335FB8
	public static Color GetLightColour(SimDebugView instance, int cell)
	{
		Color result = GlobalAssets.Instance.colorSet.lightOverlay;
		result.a = Mathf.Clamp(Mathf.Sqrt((float)(Grid.LightIntensity[cell] + LightGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0f, 1f);
		if (Grid.LightIntensity[cell] > DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN)
		{
			float num = ((float)Grid.LightIntensity[cell] + (float)LightGridManager.previewLux[cell] - (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN) / (float)(80000 - DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN);
			num /= 10f;
			result.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, Grid.CellToPos2D(cell).y / 8f + (float)instance.currentFrame / 32f) * num);
		}
		return result;
	}

	// Token: 0x0600814E RID: 33102 RVA: 0x00337EC8 File Offset: 0x003360C8
	public static Color GetRadiationColour(SimDebugView instance, int cell)
	{
		float a = Mathf.Clamp(Mathf.Sqrt(Grid.Radiation[cell]) / 30f, 0f, 1f);
		return new Color(0.2f, 0.9f, 0.3f, a);
	}

	// Token: 0x0600814F RID: 33103 RVA: 0x00337F10 File Offset: 0x00336110
	public static Color GetRoomsColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.IsValidCell(instance.selectedCell))
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null)
			{
				Room room = cavityForCell.room;
				result = GlobalAssets.Instance.colorSet.GetColorByName(room.roomType.category.colorName);
				result.a = 0.45f;
				if (Game.Instance.roomProber.GetCavityForCell(instance.selectedCell) == cavityForCell)
				{
					result.a += 0.3f;
				}
			}
		}
		return result;
	}

	// Token: 0x06008150 RID: 33104 RVA: 0x00337FB0 File Offset: 0x003361B0
	public static Color GetJoulesColour(SimDebugView instance, int cell)
	{
		float num = Grid.Element[cell].specificHeatCapacity * Grid.Temperature[cell] * (Grid.Mass[cell] * 1000f);
		float t = 0.5f * num / (ElementLoader.FindElementByHash(SimHashes.SandStone).specificHeatCapacity * 294f * 1000000f);
		return Color.Lerp(Color.black, Color.red, t);
	}

	// Token: 0x06008151 RID: 33105 RVA: 0x0033801C File Offset: 0x0033621C
	public static Color GetNormalizedTemperatureColourMode(SimDebugView instance, int cell)
	{
		switch (Game.Instance.temperatureOverlayMode)
		{
		case Game.TemperatureOverlayModes.AbsoluteTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.AdaptiveTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.HeatFlow:
			return SimDebugView.GetHeatFlowColour(instance, cell);
		case Game.TemperatureOverlayModes.StateChange:
			return SimDebugView.GetStateChangeProximityColour(instance, cell);
		default:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		}
	}

	// Token: 0x06008152 RID: 33106 RVA: 0x00338074 File Offset: 0x00336274
	public static Color GetStateChangeProximityColour(SimDebugView instance, int cell)
	{
		float temperature = Grid.Temperature[cell];
		Element element = Grid.Element[cell];
		float num = element.lowTemp;
		float num2 = element.highTemp;
		if (element.IsGas)
		{
			num2 = Mathf.Min(num + 150f, num2);
			return SimDebugView.GasTemperatureToColor(temperature, num, num2);
		}
		if (element.IsSolid)
		{
			num = Mathf.Max(num2 - 150f, num);
			return SimDebugView.SolidTemperatureToColor(temperature, num, num2);
		}
		return SimDebugView.TemperatureToColor(temperature, num, num2);
	}

	// Token: 0x06008153 RID: 33107 RVA: 0x003380EC File Offset: 0x003362EC
	public static Color GetNormalizedTemperatureColour(SimDebugView instance, int cell)
	{
		float actualTemperature = Grid.Temperature[cell];
		return instance.NormalizedTemperature(actualTemperature);
	}

	// Token: 0x06008154 RID: 33108 RVA: 0x0033810C File Offset: 0x0033630C
	private static Color GetGameGridColour(SimDebugView instance, int cell)
	{
		Color result = new Color32(0, 0, 0, byte.MaxValue);
		switch (instance.gameGridMode)
		{
		case SimDebugView.GameGridMode.GameSolidMap:
			result = (Grid.Solid[cell] ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.Lighting:
			result = ((Grid.LightCount[cell] > 0 || LightGridManager.previewLux[cell] > 0) ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.DigAmount:
			if (Grid.Element[cell].IsSolid)
			{
				float num = Grid.Damage[cell] / 255f;
				result = Color.HSVToRGB(1f - num, 1f, 1f);
			}
			break;
		case SimDebugView.GameGridMode.DupePassable:
			result = (Grid.DupePassable[cell] ? Color.white : Color.black);
			break;
		}
		return result;
	}

	// Token: 0x06008155 RID: 33109 RVA: 0x000F50FA File Offset: 0x000F32FA
	public Color32 GetColourForID(int id)
	{
		return this.networkColours[id % this.networkColours.Length];
	}

	// Token: 0x06008156 RID: 33110 RVA: 0x003381F0 File Offset: 0x003363F0
	private static Color GetThermalConductivityColour(SimDebugView instance, int cell)
	{
		bool flag = SimDebugView.IsInsulated(cell);
		Color black = Color.black;
		float num = instance.maxThermalConductivity - instance.minThermalConductivity;
		if (!flag && num != 0f)
		{
			float num2 = (Grid.Element[cell].thermalConductivity - instance.minThermalConductivity) / num;
			num2 = Mathf.Max(num2, 0f);
			num2 = Mathf.Min(num2, 1f);
			black = new Color(num2, num2, num2);
		}
		return black;
	}

	// Token: 0x06008157 RID: 33111 RVA: 0x0033825C File Offset: 0x0033645C
	private static Color GetPressureMapColour(SimDebugView instance, int cell)
	{
		Color32 c = Color.black;
		if (Grid.Pressure[cell] > 0f)
		{
			float num = Mathf.Clamp((Grid.Pressure[cell] - instance.minPressureExpected) / (instance.maxPressureExpected - instance.minPressureExpected), 0f, 1f) * 0.9f;
			c = new Color(num, num, num, 1f);
		}
		return c;
	}

	// Token: 0x06008158 RID: 33112 RVA: 0x003382D4 File Offset: 0x003364D4
	private static Color GetOxygenMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.IsLiquid(cell) && !Grid.Solid[cell])
		{
			if (Grid.Mass[cell] > SimDebugView.minimumBreathable && (Grid.Element[cell].id == SimHashes.Oxygen || Grid.Element[cell].id == SimHashes.ContaminatedOxygen))
			{
				float time = Mathf.Clamp((Grid.Mass[cell] - SimDebugView.minimumBreathable) / SimDebugView.optimallyBreathable, 0f, 1f);
				result = instance.breathableGradient.Evaluate(time);
			}
			else
			{
				result = instance.unbreathableColour;
			}
		}
		return result;
	}

	// Token: 0x06008159 RID: 33113 RVA: 0x0033837C File Offset: 0x0033657C
	private static Color GetTileColour(SimDebugView instance, int cell)
	{
		float num = 0.33f;
		Color result = new Color(num, num, num);
		Element element = Grid.Element[cell];
		bool flag = false;
		foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
		{
			if (element.HasTag(search_tag))
			{
				flag = true;
			}
		}
		if (flag)
		{
			result = element.substance.uiColour;
		}
		return result;
	}

	// Token: 0x0600815A RID: 33114 RVA: 0x000F5111 File Offset: 0x000F3311
	private static Color GetTileTypeColour(SimDebugView instance, int cell)
	{
		return Grid.Element[cell].substance.uiColour;
	}

	// Token: 0x0600815B RID: 33115 RVA: 0x0033840C File Offset: 0x0033660C
	private static Color GetStateMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Gas:
			result = Color.yellow;
			break;
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x0600815C RID: 33116 RVA: 0x00338460 File Offset: 0x00336660
	private static Color GetSolidLiquidMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x0600815D RID: 33117 RVA: 0x003384AC File Offset: 0x003366AC
	private static Color GetStateChangeColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		if (!element.IsVacuum)
		{
			float num = Grid.Temperature[cell];
			float num2 = element.lowTemp * 0.05f;
			float a = Mathf.Abs(num - element.lowTemp) / num2;
			float num3 = element.highTemp * 0.05f;
			float b = Mathf.Abs(num - element.highTemp) / num3;
			float t = Mathf.Max(0f, 1f - Mathf.Min(a, b));
			result = Color.Lerp(Color.black, Color.red, t);
		}
		return result;
	}

	// Token: 0x0600815E RID: 33118 RVA: 0x00338544 File Offset: 0x00336744
	private static Color GetDecorColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.Solid[cell])
		{
			float num = GameUtil.GetDecorAtCell(cell) / 100f;
			if (num > 0f)
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorPositive, Mathf.Abs(num));
			}
			else
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorNegative, Mathf.Abs(num));
			}
		}
		return result;
	}

	// Token: 0x0600815F RID: 33119 RVA: 0x003385E4 File Offset: 0x003367E4
	private static Color GetDangerColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		SimDebugView.DangerAmount dangerAmount = SimDebugView.DangerAmount.None;
		if (!Grid.Element[cell].IsSolid)
		{
			float num = 0f;
			if (Grid.Temperature[cell] < SimDebugView.minMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.minMinionTemperature);
			}
			if (Grid.Temperature[cell] > SimDebugView.maxMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.maxMinionTemperature);
			}
			if (num > 0f)
			{
				if (num < 10f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryLow;
				}
				else if (num < 30f)
				{
					dangerAmount = SimDebugView.DangerAmount.Low;
				}
				else if (num < 100f)
				{
					dangerAmount = SimDebugView.DangerAmount.Moderate;
				}
				else if (num < 200f)
				{
					dangerAmount = SimDebugView.DangerAmount.High;
				}
				else if (num < 400f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryHigh;
				}
				else if (num > 800f)
				{
					dangerAmount = SimDebugView.DangerAmount.Extreme;
				}
			}
		}
		if (dangerAmount < SimDebugView.DangerAmount.VeryHigh && (Grid.Element[cell].IsVacuum || (Grid.Element[cell].IsGas && (Grid.Element[cell].id != SimHashes.Oxygen || Grid.Pressure[cell] < SimDebugView.minMinionPressure))))
		{
			dangerAmount++;
		}
		if (dangerAmount != SimDebugView.DangerAmount.None)
		{
			float num2 = (float)dangerAmount / 6f;
			result = Color.HSVToRGB((80f - num2 * 80f) / 360f, 1f, 1f);
		}
		return result;
	}

	// Token: 0x06008160 RID: 33120 RVA: 0x0033872C File Offset: 0x0033692C
	private static Color GetSimCheckErrorMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		float num = Grid.Mass[cell];
		float num2 = Grid.Temperature[cell];
		if (float.IsNaN(num) || float.IsNaN(num2) || num > 10000f || num2 > 10000f)
		{
			return Color.red;
		}
		if (element.IsVacuum)
		{
			if (num2 != 0f)
			{
				result = Color.yellow;
			}
			else if (num != 0f)
			{
				result = Color.blue;
			}
			else
			{
				result = Color.gray;
			}
		}
		else if (num2 < 10f)
		{
			result = Color.red;
		}
		else if (Grid.Mass[cell] < 1f && Grid.Pressure[cell] < 1f)
		{
			result = Color.green;
		}
		else if (num2 > element.highTemp + 3f && element.highTempTransition != null)
		{
			result = Color.magenta;
		}
		else if (num2 < element.lowTemp + 3f && element.lowTempTransition != null)
		{
			result = Color.cyan;
		}
		return result;
	}

	// Token: 0x06008161 RID: 33121 RVA: 0x000F5129 File Offset: 0x000F3329
	private static Color GetFakeFloorColour(SimDebugView instance, int cell)
	{
		if (!Grid.FakeFloor[cell])
		{
			return Color.black;
		}
		return Color.cyan;
	}

	// Token: 0x06008162 RID: 33122 RVA: 0x000F5143 File Offset: 0x000F3343
	private static Color GetFoundationColour(SimDebugView instance, int cell)
	{
		if (!Grid.Foundation[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008163 RID: 33123 RVA: 0x000F515D File Offset: 0x000F335D
	private static Color GetDupePassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupePassable[cell])
		{
			return Color.black;
		}
		return Color.green;
	}

	// Token: 0x06008164 RID: 33124 RVA: 0x000F5177 File Offset: 0x000F3377
	private static Color GetCritterImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.CritterImpassable[cell])
		{
			return Color.black;
		}
		return Color.yellow;
	}

	// Token: 0x06008165 RID: 33125 RVA: 0x000F5191 File Offset: 0x000F3391
	private static Color GetDupeImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupeImpassable[cell])
		{
			return Color.black;
		}
		return Color.red;
	}

	// Token: 0x06008166 RID: 33126 RVA: 0x000F51AB File Offset: 0x000F33AB
	private static Color GetMinionOccupiedColour(SimDebugView instance, int cell)
	{
		if (!(Grid.Objects[cell, 0] != null))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008167 RID: 33127 RVA: 0x000F51CC File Offset: 0x000F33CC
	private static Color GetMinionGroupProberColour(SimDebugView instance, int cell)
	{
		if (!MinionGroupProber.Get().IsReachable(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008168 RID: 33128 RVA: 0x000F51E6 File Offset: 0x000F33E6
	private static Color GetPathProberColour(SimDebugView instance, int cell)
	{
		if (!(instance.selectedPathProber != null) || instance.selectedPathProber.GetCost(cell) == -1)
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008169 RID: 33129 RVA: 0x000F5210 File Offset: 0x000F3410
	private static Color GetReservedColour(SimDebugView instance, int cell)
	{
		if (!Grid.Reserved[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x0600816A RID: 33130 RVA: 0x000F522A File Offset: 0x000F342A
	private static Color GetAllowPathFindingColour(SimDebugView instance, int cell)
	{
		if (!Grid.AllowPathfinding[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x0600816B RID: 33131 RVA: 0x00338834 File Offset: 0x00336A34
	private static Color GetMassColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!SimDebugView.IsInsulated(cell))
		{
			float num = Grid.Mass[cell];
			if (num > 0f)
			{
				float num2 = (num - SimDebugView.Instance.minMassExpected) / (SimDebugView.Instance.maxMassExpected - SimDebugView.Instance.minMassExpected);
				result = Color.HSVToRGB(1f - num2, 1f, 1f);
			}
		}
		return result;
	}

	// Token: 0x0600816C RID: 33132 RVA: 0x000F5244 File Offset: 0x000F3444
	public static Color GetScenePartitionerColour(SimDebugView instance, int cell)
	{
		if (!GameScenePartitioner.Instance.DoDebugLayersContainItemsOnCell(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x040061EB RID: 25067
	[SerializeField]
	public Material material;

	// Token: 0x040061EC RID: 25068
	public Material diseaseMaterial;

	// Token: 0x040061ED RID: 25069
	public bool hideFOW;

	// Token: 0x040061EE RID: 25070
	public const int colourSize = 4;

	// Token: 0x040061EF RID: 25071
	private byte[] texBytes;

	// Token: 0x040061F0 RID: 25072
	private int currentFrame;

	// Token: 0x040061F1 RID: 25073
	[SerializeField]
	private Texture2D tex;

	// Token: 0x040061F2 RID: 25074
	[SerializeField]
	private GameObject plane;

	// Token: 0x040061F3 RID: 25075
	private HashedString mode = global::OverlayModes.Power.ID;

	// Token: 0x040061F4 RID: 25076
	private SimDebugView.GameGridMode gameGridMode = SimDebugView.GameGridMode.DigAmount;

	// Token: 0x040061F5 RID: 25077
	private PathProber selectedPathProber;

	// Token: 0x040061F6 RID: 25078
	public float minTempExpected = 173.15f;

	// Token: 0x040061F7 RID: 25079
	public float maxTempExpected = 423.15f;

	// Token: 0x040061F8 RID: 25080
	public float minMassExpected = 1.0001f;

	// Token: 0x040061F9 RID: 25081
	public float maxMassExpected = 10000f;

	// Token: 0x040061FA RID: 25082
	public float minPressureExpected = 1.300003f;

	// Token: 0x040061FB RID: 25083
	public float maxPressureExpected = 201.3f;

	// Token: 0x040061FC RID: 25084
	public float minThermalConductivity;

	// Token: 0x040061FD RID: 25085
	public float maxThermalConductivity = 30f;

	// Token: 0x040061FE RID: 25086
	public float thresholdRange = 0.001f;

	// Token: 0x040061FF RID: 25087
	public float thresholdOpacity = 0.8f;

	// Token: 0x04006200 RID: 25088
	public static float minimumBreathable = 0.05f;

	// Token: 0x04006201 RID: 25089
	public static float optimallyBreathable = 1f;

	// Token: 0x04006202 RID: 25090
	public SimDebugView.ColorThreshold[] temperatureThresholds;

	// Token: 0x04006203 RID: 25091
	public Vector2 user_temperatureThresholds = Vector2.zero;

	// Token: 0x04006204 RID: 25092
	public SimDebugView.ColorThreshold[] heatFlowThresholds;

	// Token: 0x04006205 RID: 25093
	public Color32[] networkColours;

	// Token: 0x04006206 RID: 25094
	public Gradient breathableGradient = new Gradient();

	// Token: 0x04006207 RID: 25095
	public Color32 unbreathableColour = new Color(0.5f, 0f, 0f);

	// Token: 0x04006208 RID: 25096
	public Color32[] toxicColour = new Color32[]
	{
		new Color(0.5f, 0f, 0.5f),
		new Color(1f, 0f, 1f)
	};

	// Token: 0x04006209 RID: 25097
	public static SimDebugView Instance;

	// Token: 0x0400620A RID: 25098
	private WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData> updateSimViewWorkItems = new WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData>();

	// Token: 0x0400620B RID: 25099
	private int selectedCell;

	// Token: 0x0400620C RID: 25100
	private Dictionary<HashedString, Action<SimDebugView, Texture>> dataUpdateFuncs = new Dictionary<HashedString, Action<SimDebugView, Texture>>
	{
		{
			global::OverlayModes.Temperature.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Decor.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint)
		},
		{
			global::OverlayModes.Disease.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDisease)
		}
	};

	// Token: 0x0400620D RID: 25101
	private static float[] relativeTemperatureColorIntervals = new float[]
	{
		0.4f,
		0.05f,
		0.05f,
		0.05f,
		0.05f,
		0.2f,
		0.2f
	};

	// Token: 0x0400620E RID: 25102
	private static float[] absoluteTemperatureColorIntervals = new float[]
	{
		273.15f,
		10f,
		10f,
		10f,
		7f,
		63f,
		1700f,
		10000f
	};

	// Token: 0x0400620F RID: 25103
	private Dictionary<HashedString, Func<SimDebugView, int, Color>> getColourFuncs = new Dictionary<HashedString, Func<SimDebugView, int, Color>>
	{
		{
			global::OverlayModes.ThermalConductivity.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetThermalConductivityColour)
		},
		{
			global::OverlayModes.Temperature.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetNormalizedTemperatureColourMode)
		},
		{
			global::OverlayModes.Disease.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDiseaseColour)
		},
		{
			global::OverlayModes.Decor.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDecorColour)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetOxygenMapColour)
		},
		{
			global::OverlayModes.Light.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetLightColour)
		},
		{
			global::OverlayModes.Radiation.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRadiationColour)
		},
		{
			global::OverlayModes.Rooms.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRoomsColour)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileColour)
		},
		{
			global::OverlayModes.Suit.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Priorities.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Crop.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Harvest.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			SimDebugView.OverlayModes.GameGrid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetGameGridColour)
		},
		{
			SimDebugView.OverlayModes.StateChange,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateChangeColour)
		},
		{
			SimDebugView.OverlayModes.SimCheckErrorMap,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSimCheckErrorMapColour)
		},
		{
			SimDebugView.OverlayModes.Foundation,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFoundationColour)
		},
		{
			SimDebugView.OverlayModes.FakeFloor,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFakeFloorColour)
		},
		{
			SimDebugView.OverlayModes.DupePassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupePassableColour)
		},
		{
			SimDebugView.OverlayModes.DupeImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupeImpassableColour)
		},
		{
			SimDebugView.OverlayModes.CritterImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetCritterImpassableColour)
		},
		{
			SimDebugView.OverlayModes.MinionGroupProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionGroupProberColour)
		},
		{
			SimDebugView.OverlayModes.PathProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPathProberColour)
		},
		{
			SimDebugView.OverlayModes.Reserved,
			new Func<SimDebugView, int, Color>(SimDebugView.GetReservedColour)
		},
		{
			SimDebugView.OverlayModes.AllowPathFinding,
			new Func<SimDebugView, int, Color>(SimDebugView.GetAllowPathFindingColour)
		},
		{
			SimDebugView.OverlayModes.Danger,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDangerColour)
		},
		{
			SimDebugView.OverlayModes.MinionOccupied,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionOccupiedColour)
		},
		{
			SimDebugView.OverlayModes.Pressure,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPressureMapColour)
		},
		{
			SimDebugView.OverlayModes.TileType,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileTypeColour)
		},
		{
			SimDebugView.OverlayModes.State,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateMapColour)
		},
		{
			SimDebugView.OverlayModes.SolidLiquid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSolidLiquidMapColour)
		},
		{
			SimDebugView.OverlayModes.Mass,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMassColour)
		},
		{
			SimDebugView.OverlayModes.Joules,
			new Func<SimDebugView, int, Color>(SimDebugView.GetJoulesColour)
		},
		{
			SimDebugView.OverlayModes.ScenePartitioner,
			new Func<SimDebugView, int, Color>(SimDebugView.GetScenePartitionerColour)
		}
	};

	// Token: 0x04006210 RID: 25104
	public static readonly Color[] dbColours = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 1f, 1f, 0.3f),
		new Color(0.7058824f, 0.8235294f, 1f, 0.2f),
		new Color(0f, 0.3137255f, 1f, 0.3f),
		new Color(0.7058824f, 1f, 0.7058824f, 0.5f),
		new Color(0.078431375f, 1f, 0f, 0.7f),
		new Color(1f, 0.9019608f, 0.7058824f, 0.9f),
		new Color(1f, 0.8235294f, 0f, 0.9f),
		new Color(1f, 0.7176471f, 0.3019608f, 0.9f),
		new Color(1f, 0.41568628f, 0f, 0.9f),
		new Color(1f, 0.7058824f, 0.7058824f, 1f),
		new Color(1f, 0f, 0f, 1f),
		new Color(1f, 0f, 0f, 1f)
	};

	// Token: 0x04006211 RID: 25105
	private static float minMinionTemperature = 260f;

	// Token: 0x04006212 RID: 25106
	private static float maxMinionTemperature = 310f;

	// Token: 0x04006213 RID: 25107
	private static float minMinionPressure = 80f;

	// Token: 0x0200186D RID: 6253
	public static class OverlayModes
	{
		// Token: 0x04006214 RID: 25108
		public static readonly HashedString Mass = "Mass";

		// Token: 0x04006215 RID: 25109
		public static readonly HashedString Pressure = "Pressure";

		// Token: 0x04006216 RID: 25110
		public static readonly HashedString GameGrid = "GameGrid";

		// Token: 0x04006217 RID: 25111
		public static readonly HashedString ScenePartitioner = "ScenePartitioner";

		// Token: 0x04006218 RID: 25112
		public static readonly HashedString ConduitUpdates = "ConduitUpdates";

		// Token: 0x04006219 RID: 25113
		public static readonly HashedString Flow = "Flow";

		// Token: 0x0400621A RID: 25114
		public static readonly HashedString StateChange = "StateChange";

		// Token: 0x0400621B RID: 25115
		public static readonly HashedString SimCheckErrorMap = "SimCheckErrorMap";

		// Token: 0x0400621C RID: 25116
		public static readonly HashedString DupePassable = "DupePassable";

		// Token: 0x0400621D RID: 25117
		public static readonly HashedString Foundation = "Foundation";

		// Token: 0x0400621E RID: 25118
		public static readonly HashedString FakeFloor = "FakeFloor";

		// Token: 0x0400621F RID: 25119
		public static readonly HashedString CritterImpassable = "CritterImpassable";

		// Token: 0x04006220 RID: 25120
		public static readonly HashedString DupeImpassable = "DupeImpassable";

		// Token: 0x04006221 RID: 25121
		public static readonly HashedString MinionGroupProber = "MinionGroupProber";

		// Token: 0x04006222 RID: 25122
		public static readonly HashedString PathProber = "PathProber";

		// Token: 0x04006223 RID: 25123
		public static readonly HashedString Reserved = "Reserved";

		// Token: 0x04006224 RID: 25124
		public static readonly HashedString AllowPathFinding = "AllowPathFinding";

		// Token: 0x04006225 RID: 25125
		public static readonly HashedString Danger = "Danger";

		// Token: 0x04006226 RID: 25126
		public static readonly HashedString MinionOccupied = "MinionOccupied";

		// Token: 0x04006227 RID: 25127
		public static readonly HashedString TileType = "TileType";

		// Token: 0x04006228 RID: 25128
		public static readonly HashedString State = "State";

		// Token: 0x04006229 RID: 25129
		public static readonly HashedString SolidLiquid = "SolidLiquid";

		// Token: 0x0400622A RID: 25130
		public static readonly HashedString Joules = "Joules";
	}

	// Token: 0x0200186E RID: 6254
	public enum GameGridMode
	{
		// Token: 0x0400622C RID: 25132
		GameSolidMap,
		// Token: 0x0400622D RID: 25133
		Lighting,
		// Token: 0x0400622E RID: 25134
		RoomMap,
		// Token: 0x0400622F RID: 25135
		Style,
		// Token: 0x04006230 RID: 25136
		PlantDensity,
		// Token: 0x04006231 RID: 25137
		DigAmount,
		// Token: 0x04006232 RID: 25138
		DupePassable
	}

	// Token: 0x0200186F RID: 6255
	[Serializable]
	public struct ColorThreshold
	{
		// Token: 0x04006233 RID: 25139
		public string colorName;

		// Token: 0x04006234 RID: 25140
		public float value;
	}

	// Token: 0x02001870 RID: 6256
	private struct UpdateSimViewSharedData
	{
		// Token: 0x06008170 RID: 33136 RVA: 0x000F525E File Offset: 0x000F345E
		public UpdateSimViewSharedData(SimDebugView instance, byte[] texture_bytes, HashedString sim_view_mode, SimDebugView sim_debug_view)
		{
			this.instance = instance;
			this.textureBytes = texture_bytes;
			this.simViewMode = sim_view_mode;
			this.simDebugView = sim_debug_view;
		}

		// Token: 0x04006235 RID: 25141
		public SimDebugView instance;

		// Token: 0x04006236 RID: 25142
		public HashedString simViewMode;

		// Token: 0x04006237 RID: 25143
		public SimDebugView simDebugView;

		// Token: 0x04006238 RID: 25144
		public byte[] textureBytes;
	}

	// Token: 0x02001871 RID: 6257
	private struct UpdateSimViewWorkItem : IWorkItem<SimDebugView.UpdateSimViewSharedData>
	{
		// Token: 0x06008171 RID: 33137 RVA: 0x003390D0 File Offset: 0x003372D0
		public UpdateSimViewWorkItem(int x0, int y0, int x1, int y1)
		{
			this.x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
			this.x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
			this.y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
			this.y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x00339130 File Offset: 0x00337330
		public void Run(SimDebugView.UpdateSimViewSharedData shared_data)
		{
			Func<SimDebugView, int, Color> func;
			if (!shared_data.instance.getColourFuncs.TryGetValue(shared_data.simViewMode, out func))
			{
				func = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
			}
			for (int i = this.y0; i <= this.y1; i++)
			{
				int num = Grid.XYToCell(this.x0, i);
				int num2 = Grid.XYToCell(this.x1, i);
				for (int j = num; j <= num2; j++)
				{
					int num3 = j * 4;
					if (Grid.IsActiveWorld(j))
					{
						Color color = func(shared_data.instance, j);
						shared_data.textureBytes[num3] = (byte)(Mathf.Min(color.r, 1f) * 255f);
						shared_data.textureBytes[num3 + 1] = (byte)(Mathf.Min(color.g, 1f) * 255f);
						shared_data.textureBytes[num3 + 2] = (byte)(Mathf.Min(color.b, 1f) * 255f);
						shared_data.textureBytes[num3 + 3] = (byte)(Mathf.Min(color.a, 1f) * 255f);
					}
					else
					{
						shared_data.textureBytes[num3] = 0;
						shared_data.textureBytes[num3 + 1] = 0;
						shared_data.textureBytes[num3 + 2] = 0;
						shared_data.textureBytes[num3 + 3] = 0;
					}
				}
			}
		}

		// Token: 0x04006239 RID: 25145
		private int x0;

		// Token: 0x0400623A RID: 25146
		private int y0;

		// Token: 0x0400623B RID: 25147
		private int x1;

		// Token: 0x0400623C RID: 25148
		private int y1;
	}

	// Token: 0x02001872 RID: 6258
	public enum DangerAmount
	{
		// Token: 0x0400623E RID: 25150
		None,
		// Token: 0x0400623F RID: 25151
		VeryLow,
		// Token: 0x04006240 RID: 25152
		Low,
		// Token: 0x04006241 RID: 25153
		Moderate,
		// Token: 0x04006242 RID: 25154
		High,
		// Token: 0x04006243 RID: 25155
		VeryHigh,
		// Token: 0x04006244 RID: 25156
		Extreme,
		// Token: 0x04006245 RID: 25157
		MAX_DANGERAMOUNT = 6
	}
}
