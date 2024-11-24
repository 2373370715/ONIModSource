﻿using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

[AddComponentMenu("KMonoBehaviour/scripts/PropertyTextures")]
public class PropertyTextures : KMonoBehaviour, ISim200ms
{
	public static void DestroyInstance()
	{
		ShaderReloader.Unregister(new System.Action(PropertyTextures.instance.OnShadersReloaded));
		PropertyTextures.externalFlowTex = IntPtr.Zero;
		PropertyTextures.externalLiquidTex = IntPtr.Zero;
		PropertyTextures.externalExposedToSunlight = IntPtr.Zero;
		PropertyTextures.externalSolidDigAmountTex = IntPtr.Zero;
		PropertyTextures.instance = null;
	}

	protected override void OnPrefabInit()
	{
		PropertyTextures.instance = this;
		base.OnPrefabInit();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

		public static bool IsFogOfWarEnabled
	{
		get
		{
			return PropertyTextures.FogOfWarScale < 1f;
		}
	}

	public Texture GetTexture(PropertyTextures.Property property)
	{
		return this.textureBuffers[(int)property].texture;
	}

	private string GetShaderPropertyName(PropertyTextures.Property property)
	{
		return "_" + property.ToString() + "Tex";
	}

	protected override void OnSpawn()
	{
		if (GenericGameSettings.instance.disableFogOfWar)
		{
			PropertyTextures.FogOfWarScale = 1f;
		}
		this.WorldSizeID = Shader.PropertyToID("_WorldSizeInfo");
		this.ClusterWorldSizeID = Shader.PropertyToID("_ClusterWorldSizeInfo");
		this.FogOfWarScaleID = Shader.PropertyToID("_FogOfWarScale");
		this.PropTexWsToCsID = Shader.PropertyToID("_PropTexWsToCs");
		this.PropTexCsToWsID = Shader.PropertyToID("_PropTexCsToWs");
		this.TopBorderHeightID = Shader.PropertyToID("_TopBorderHeight");
		this.CameraZoomID = Shader.PropertyToID("_CameraZoomInfo");
	}

	public void OnReset(object data = null)
	{
		this.lerpers = new TextureLerper[14];
		this.texturePagePool = new TexturePagePool();
		this.textureBuffers = new TextureBuffer[14];
		this.externallyUpdatedTextures = new Texture2D[14];
		for (int i = 0; i < 14; i++)
		{
			PropertyTextures.TextureProperties textureProperties = new PropertyTextures.TextureProperties
			{
				textureFormat = TextureFormat.Alpha8,
				filterMode = FilterMode.Bilinear,
				blend = false,
				blendSpeed = 1f
			};
			for (int j = 0; j < this.textureProperties.Length; j++)
			{
				if (i == (int)this.textureProperties[j].simProperty)
				{
					textureProperties = this.textureProperties[j];
				}
			}
			PropertyTextures.Property property = (PropertyTextures.Property)i;
			textureProperties.name = property.ToString();
			if (this.externallyUpdatedTextures[i] != null)
			{
				UnityEngine.Object.Destroy(this.externallyUpdatedTextures[i]);
				this.externallyUpdatedTextures[i] = null;
			}
			Texture texture;
			if (textureProperties.updatedExternally)
			{
				this.externallyUpdatedTextures[i] = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureUtil.TextureFormatToGraphicsFormat(textureProperties.textureFormat), TextureCreationFlags.None);
				texture = this.externallyUpdatedTextures[i];
			}
			else
			{
				TextureBuffer[] array = this.textureBuffers;
				int num = i;
				property = (PropertyTextures.Property)i;
				array[num] = new TextureBuffer(property.ToString(), Grid.WidthInCells, Grid.HeightInCells, textureProperties.textureFormat, textureProperties.filterMode, this.texturePagePool);
				texture = this.textureBuffers[i].texture;
			}
			if (textureProperties.blend)
			{
				TextureLerper[] array2 = this.lerpers;
				int num2 = i;
				Texture target_texture = texture;
				property = (PropertyTextures.Property)i;
				array2[num2] = new TextureLerper(target_texture, property.ToString(), texture.filterMode, textureProperties.textureFormat);
				this.lerpers[i].Speed = textureProperties.blendSpeed;
			}
			string shaderPropertyName = this.GetShaderPropertyName((PropertyTextures.Property)i);
			texture.name = shaderPropertyName;
			textureProperties.texturePropertyName = shaderPropertyName;
			Shader.SetGlobalTexture(shaderPropertyName, texture);
			this.allTextureProperties.Add(textureProperties);
		}
	}

	private void OnShadersReloaded()
	{
		for (int i = 0; i < 14; i++)
		{
			TextureLerper textureLerper = this.lerpers[i];
			if (textureLerper != null)
			{
				Shader.SetGlobalTexture(this.allTextureProperties[i].texturePropertyName, textureLerper.Update());
			}
		}
	}

	public void Sim200ms(float dt)
	{
		if (this.lerpers == null || this.lerpers.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.lerpers.Length; i++)
		{
			TextureLerper textureLerper = this.lerpers[i];
			if (textureLerper != null)
			{
				textureLerper.LongUpdate(dt);
			}
		}
	}

	private void UpdateTextureThreaded(TextureRegion texture_region, int x0, int y0, int x1, int y1, PropertyTextures.WorkItem.Callback update_texture_cb)
	{
		this.workItems.Reset(null);
		int num = 16;
		for (int i = y0; i <= y1; i += num)
		{
			int y2 = Math.Min(i + num - 1, y1);
			this.workItems.Add(new PropertyTextures.WorkItem(texture_region, x0, i, x1, y2, update_texture_cb));
		}
		GlobalJobManager.Run(this.workItems);
	}

	private void UpdateProperty(ref PropertyTextures.TextureProperties p, int x0, int y0, int x1, int y1)
	{
		if (Game.Instance == null || Game.Instance.IsLoading())
		{
			return;
		}
		int simProperty = (int)p.simProperty;
		if (!p.updatedExternally)
		{
			TextureRegion texture_region = this.textureBuffers[simProperty].Lock(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
			switch (p.simProperty)
			{
			case PropertyTextures.Property.StateChange:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateStateChange));
				break;
			case PropertyTextures.Property.GasPressure:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdatePressure));
				break;
			case PropertyTextures.Property.GasColour:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateGasColour));
				break;
			case PropertyTextures.Property.GasDanger:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateDanger));
				break;
			case PropertyTextures.Property.FogOfWar:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateFogOfWar));
				break;
			case PropertyTextures.Property.SolidDigAmount:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidDigAmount));
				break;
			case PropertyTextures.Property.SolidLiquidGasMass:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateSolidLiquidGasMass));
				break;
			case PropertyTextures.Property.WorldLight:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateWorldLight));
				break;
			case PropertyTextures.Property.Temperature:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateTemperature));
				break;
			case PropertyTextures.Property.FallingSolid:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateFallingSolidChange));
				break;
			case PropertyTextures.Property.Radiation:
				this.UpdateTextureThreaded(texture_region, x0, y0, x1, y1, new PropertyTextures.WorkItem.Callback(PropertyTextures.UpdateRadiation));
				break;
			}
			texture_region.Unlock();
			return;
		}
		PropertyTextures.Property simProperty2 = p.simProperty;
		if (simProperty2 != PropertyTextures.Property.Flow)
		{
			if (simProperty2 != PropertyTextures.Property.Liquid)
			{
				if (simProperty2 == PropertyTextures.Property.ExposedToSunlight)
				{
					this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalExposedToSunlight, Grid.WidthInCells * Grid.HeightInCells);
				}
			}
			else
			{
				this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalLiquidTex, 4 * Grid.WidthInCells * Grid.HeightInCells);
			}
		}
		else
		{
			this.externallyUpdatedTextures[simProperty].LoadRawTextureData(PropertyTextures.externalFlowTex, 8 * Grid.WidthInCells * Grid.HeightInCells);
		}
		this.externallyUpdatedTextures[simProperty].Apply();
	}

	public static Vector4 CalculateClusterWorldSize()
	{
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 zero = Vector4.zero;
		if (DlcManager.IsPureVanilla() || (CameraController.Instance != null && CameraController.Instance.ignoreClusterFX))
		{
			zero = new Vector4((float)Grid.WidthInCells, (float)Grid.HeightInCells, 0f, 0f);
		}
		else
		{
			zero = new Vector4((float)worldSize.x, (float)worldSize.y, (float)worldOffset.x, (float)worldOffset.y);
		}
		return zero;
	}

	private void LateUpdate()
	{
		if (!Grid.IsInitialized())
		{
			return;
		}
		Shader.SetGlobalVector(this.WorldSizeID, new Vector4((float)Grid.WidthInCells, (float)Grid.HeightInCells, 1f / (float)Grid.WidthInCells, 1f / (float)Grid.HeightInCells));
		Vector4 value = PropertyTextures.CalculateClusterWorldSize();
		float num = CameraController.Instance.FreeCameraEnabled ? TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : 20f;
		Shader.SetGlobalVector(this.CameraZoomID, new Vector4(CameraController.Instance.OrthographicSize, CameraController.Instance.minOrthographicSize, num, (CameraController.Instance.OrthographicSize - CameraController.Instance.minOrthographicSize) / (num - CameraController.Instance.minOrthographicSize)));
		Shader.SetGlobalVector(this.ClusterWorldSizeID, value);
		Shader.SetGlobalVector(this.PropTexWsToCsID, new Vector4(0f, 0f, 1f, 1f));
		Shader.SetGlobalVector(this.PropTexCsToWsID, new Vector4(0f, 0f, 1f, 1f));
		Shader.SetGlobalFloat(this.TopBorderHeightID, ClusterManager.Instance.activeWorld.FullyEnclosedBorder ? 0f : ((float)Grid.TopBorderHeight));
		int x;
		int y;
		int x2;
		int y2;
		this.GetVisibleCellRange(out x, out y, out x2, out y2);
		Shader.SetGlobalFloat(this.FogOfWarScaleID, PropertyTextures.FogOfWarScale);
		int nextPropertyIdx = this.NextPropertyIdx;
		this.NextPropertyIdx = nextPropertyIdx + 1;
		int num2 = nextPropertyIdx % this.allTextureProperties.Count;
		PropertyTextures.TextureProperties textureProperties = this.allTextureProperties[num2];
		while (textureProperties.updateEveryFrame)
		{
			nextPropertyIdx = this.NextPropertyIdx;
			this.NextPropertyIdx = nextPropertyIdx + 1;
			num2 = nextPropertyIdx % this.allTextureProperties.Count;
			textureProperties = this.allTextureProperties[num2];
		}
		for (int i = 0; i < this.allTextureProperties.Count; i++)
		{
			PropertyTextures.TextureProperties textureProperties2 = this.allTextureProperties[i];
			if (num2 == i || textureProperties2.updateEveryFrame || GameUtil.IsCapturingTimeLapse())
			{
				this.UpdateProperty(ref textureProperties2, x, y, x2, y2);
			}
		}
		for (int j = 0; j < 14; j++)
		{
			TextureLerper textureLerper = this.lerpers[j];
			if (textureLerper != null)
			{
				if (Time.timeScale == 0f)
				{
					textureLerper.LongUpdate(Time.unscaledDeltaTime);
				}
				Shader.SetGlobalTexture(this.allTextureProperties[j].texturePropertyName, textureLerper.Update());
			}
		}
	}

	private void GetVisibleCellRange(out int x0, out int y0, out int x1, out int y1)
	{
		int num = 16;
		Grid.GetVisibleExtents(out x0, out y0, out x1, out y1);
		int widthInCells = Grid.WidthInCells;
		int heightInCells = Grid.HeightInCells;
		int num2 = 0;
		int num3 = 0;
		x0 = Math.Max(num2, x0 - num);
		y0 = Math.Max(num3, y0 - num);
		x0 = Mathf.Min(x0, widthInCells - 1);
		y0 = Mathf.Min(y0, heightInCells - 1);
		x1 = Mathf.CeilToInt((float)(x1 + num));
		y1 = Mathf.CeilToInt((float)(y1 + num));
		x1 = Mathf.Max(x1, num2);
		y1 = Mathf.Max(y1, num3);
		x1 = Mathf.Min(x1, widthInCells - 1);
		y1 = Mathf.Min(y1, heightInCells - 1);
	}

	private static void UpdateFogOfWar(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		byte[] visible = Grid.Visible;
		int y2 = Grid.HeightInCells;
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			y2 = activeWorld.WorldSize.y + activeWorld.WorldOffset.y - 1;
		}
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					int num2 = Grid.XYToCell(j, y2);
					if (Grid.IsValidCell(num2))
					{
						region.SetBytes(j, i, visible[num2]);
					}
					else
					{
						region.SetBytes(j, i, 0);
					}
				}
				else
				{
					region.SetBytes(j, i, visible[num]);
				}
			}
		}
	}

	private static void UpdatePressure(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 pressureRange = PropertyTextures.instance.PressureRange;
		float minPressureVisibility = PropertyTextures.instance.MinPressureVisibility;
		float num = pressureRange.y - pressureRange.x;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num2 = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num2))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num3 = 0f;
					Element element = Grid.Element[num2];
					if (element.IsGas)
					{
						float num4 = Grid.Pressure[num2];
						float b = (num4 > 0f) ? minPressureVisibility : 0f;
						num3 = Mathf.Max(Mathf.Clamp01((num4 - pressureRange.x) / num), b);
					}
					else if (element.IsLiquid)
					{
						int num5 = Grid.CellAbove(num2);
						if (Grid.IsValidCell(num5) && Grid.Element[num5].IsGas)
						{
							float num6 = Grid.Pressure[num5];
							float b2 = (num6 > 0f) ? minPressureVisibility : 0f;
							num3 = Mathf.Max(Mathf.Clamp01((num6 - pressureRange.x) / num), b2);
						}
					}
					region.SetBytes(j, i, (byte)(num3 * 255f));
				}
			}
		}
	}

	private static void UpdateDanger(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					byte b = (Grid.Element[num].id == SimHashes.Oxygen) ? 0 : byte.MaxValue;
					region.SetBytes(j, i, b);
				}
			}
		}
	}

	private static void UpdateStateChange(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		float temperatureStateChangeRange = PropertyTextures.instance.TemperatureStateChangeRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num2 = 0f;
					Element element = Grid.Element[num];
					if (!element.IsVacuum)
					{
						float num3 = Grid.Temperature[num];
						float num4 = element.lowTemp * temperatureStateChangeRange;
						float a = Mathf.Abs(num3 - element.lowTemp) / num4;
						float num5 = element.highTemp * temperatureStateChangeRange;
						float b = Mathf.Abs(num3 - element.highTemp) / num5;
						num2 = Mathf.Max(num2, 1f - Mathf.Min(a, b));
					}
					region.SetBytes(j, i, (byte)(num2 * 255f));
				}
			}
		}
	}

	private static void UpdateFallingSolidChange(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0);
				}
				else
				{
					float num2 = 0f;
					Element element = Grid.Element[num];
					if (element.id == SimHashes.Mud || element.id == SimHashes.ToxicMud)
					{
						num2 = 0.65f;
					}
					region.SetBytes(j, i, (byte)(num2 * 255f));
				}
			}
		}
	}

	private static void UpdateGasColour(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0, 0);
				}
				else
				{
					Element element = Grid.Element[num];
					if (element.IsGas)
					{
						region.SetBytes(j, i, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
					}
					else if (element.IsLiquid)
					{
						if (Grid.IsValidCell(Grid.CellAbove(num)))
						{
							region.SetBytes(j, i, element.substance.colour.r, element.substance.colour.g, element.substance.colour.b, byte.MaxValue);
						}
						else
						{
							region.SetBytes(j, i, 0, 0, 0, 0);
						}
					}
					else
					{
						region.SetBytes(j, i, 0, 0, 0, 0);
					}
				}
			}
		}
	}

	private static void UpdateLiquid(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = x0; i <= x1; i++)
		{
			int num = Grid.XYToCell(i, y1);
			Element element = Grid.Element[num];
			for (int j = y1; j >= y0; j--)
			{
				int num2 = Grid.XYToCell(i, j);
				if (!Grid.IsActiveWorld(num2))
				{
					region.SetBytes(i, j, 0, 0, 0, 0);
				}
				else
				{
					Element element2 = Grid.Element[num2];
					if (element2.IsLiquid)
					{
						Color32 colour = element2.substance.colour;
						float liquidMaxMass = Lighting.Instance.Settings.LiquidMaxMass;
						float liquidAmountOffset = Lighting.Instance.Settings.LiquidAmountOffset;
						float num3;
						if (element.IsLiquid || element.IsSolid)
						{
							num3 = 1f;
						}
						else
						{
							num3 = liquidAmountOffset + (1f - liquidAmountOffset) * Mathf.Min(Grid.Mass[num2] / liquidMaxMass, 1f);
							num3 = Mathf.Pow(Mathf.Min(Grid.Mass[num2] / liquidMaxMass, 1f), 0.45f);
						}
						region.SetBytes(i, j, (byte)(num3 * 255f), colour.r, colour.g, colour.b);
					}
					else
					{
						region.SetBytes(i, j, 0, 0, 0, 0);
					}
					element = element2;
				}
			}
		}
	}

	private static void UpdateSolidDigAmount(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Void);
		for (int i = y0; i <= y1; i++)
		{
			int num = Grid.XYToCell(x0, i);
			int num2 = Grid.XYToCell(x1, i);
			int j = num;
			int num3 = x0;
			while (j <= num2)
			{
				byte b = 0;
				byte b2 = 0;
				byte b3 = 0;
				if (Grid.ElementIdx[j] != elementIndex)
				{
					b3 = byte.MaxValue;
				}
				if (Grid.Solid[j])
				{
					b = byte.MaxValue;
					b2 = (byte)(255f * Grid.Damage[j]);
				}
				region.SetBytes(num3, i, b, b2, b3);
				j++;
				num3++;
			}
		}
	}

	private static void UpdateSolidLiquidGasMass(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0, 0);
				}
				else
				{
					Element element = Grid.Element[num];
					byte b = 0;
					byte b2 = 0;
					byte b3 = 0;
					if (element.IsSolid)
					{
						b = byte.MaxValue;
					}
					else if (element.IsLiquid)
					{
						b2 = byte.MaxValue;
					}
					else if (element.IsGas || element.IsVacuum)
					{
						b3 = byte.MaxValue;
					}
					float num2 = Grid.Mass[num];
					float num3 = Mathf.Min(1f, num2 / 2000f);
					if (num2 > 0f)
					{
						num3 = Mathf.Max(0.003921569f, num3);
					}
					region.SetBytes(j, i, b, b2, b3, (byte)(num3 * 255f));
				}
			}
		}
	}

	private static void GetTemperatureAlpha(float t, Vector2 cold_range, Vector2 hot_range, out byte cold_alpha, out byte hot_alpha)
	{
		cold_alpha = 0;
		hot_alpha = 0;
		if (t <= cold_range.y)
		{
			float num = Mathf.Clamp01((cold_range.y - t) / (cold_range.y - cold_range.x));
			cold_alpha = (byte)(num * 255f);
			return;
		}
		if (t >= hot_range.x)
		{
			float num2 = Mathf.Clamp01((t - hot_range.x) / (hot_range.y - hot_range.x));
			hot_alpha = (byte)(num2 * 255f);
		}
	}

	private static void UpdateTemperature(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 cold_range = PropertyTextures.instance.coldRange;
		Vector2 hot_range = PropertyTextures.instance.hotRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0);
				}
				else
				{
					float num2 = Grid.Temperature[num];
					byte b;
					byte b2;
					PropertyTextures.GetTemperatureAlpha(num2, cold_range, hot_range, out b, out b2);
					byte b3 = (byte)(255f * Mathf.Pow(Mathf.Clamp(num2 / 1000f, 0f, 1f), 0.45f));
					region.SetBytes(j, i, b, b2, b3);
				}
			}
		}
	}

	private static void UpdateWorldLight(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		if (!PropertyTextures.instance.ForceLightEverywhere)
		{
			for (int i = y0; i <= y1; i++)
			{
				int num = Grid.XYToCell(x0, i);
				int num2 = Grid.XYToCell(x1, i);
				int j = num;
				int num3 = x0;
				while (j <= num2)
				{
					Color32 color = (Grid.LightCount[j] > 0) ? Lighting.Instance.Settings.LightColour : new Color32(0, 0, 0, byte.MaxValue);
					region.SetBytes(num3, i, color.r, color.g, color.b, (color.r + color.g + color.b > 0) ? byte.MaxValue : 0);
					j++;
					num3++;
				}
			}
			return;
		}
		for (int k = y0; k <= y1; k++)
		{
			for (int l = x0; l <= x1; l++)
			{
				region.SetBytes(l, k, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}
	}

	private static void UpdateRadiation(TextureRegion region, int x0, int y0, int x1, int y1)
	{
		Vector2 vector = PropertyTextures.instance.coldRange;
		Vector2 vector2 = PropertyTextures.instance.hotRange;
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (!Grid.IsActiveWorld(num))
				{
					region.SetBytes(j, i, 0, 0, 0);
				}
				else
				{
					float v = Grid.Radiation[num];
					region.SetBytes(j, i, v);
				}
			}
		}
	}

	[NonSerialized]
	public bool ForceLightEverywhere;

	[SerializeField]
	private Vector2 PressureRange = new Vector2(15f, 200f);

	[SerializeField]
	private float MinPressureVisibility = 0.1f;

	[SerializeField]
	[Range(0f, 1f)]
	private float TemperatureStateChangeRange = 0.05f;

	public static PropertyTextures instance;

	public static IntPtr externalFlowTex;

	public static IntPtr externalLiquidTex;

	public static IntPtr externalExposedToSunlight;

	public static IntPtr externalSolidDigAmountTex;

	[SerializeField]
	private Vector2 coldRange;

	[SerializeField]
	private Vector2 hotRange;

	public static float FogOfWarScale;

	private int WorldSizeID;

	private int ClusterWorldSizeID;

	private int FogOfWarScaleID;

	private int PropTexWsToCsID;

	private int PropTexCsToWsID;

	private int TopBorderHeightID;

	private int CameraZoomID;

	private int NextPropertyIdx;

	public TextureBuffer[] textureBuffers;

	public TextureLerper[] lerpers;

	private TexturePagePool texturePagePool;

	[SerializeField]
	private Texture2D[] externallyUpdatedTextures;

	private PropertyTextures.TextureProperties[] textureProperties = new PropertyTextures.TextureProperties[]
	{
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Flow,
			textureFormat = TextureFormat.RGFloat,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Liquid,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = true,
			blendSpeed = 1f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.ExposedToSunlight,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = true,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.SolidDigAmount,
			textureFormat = TextureFormat.RGB24,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasColour,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasDanger,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.GasPressure,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = true,
			blendSpeed = 0.25f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.FogOfWar,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.WorldLight,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.StateChange,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.FallingSolid,
			textureFormat = TextureFormat.Alpha8,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.SolidLiquidGasMass,
			textureFormat = TextureFormat.RGBA32,
			filterMode = FilterMode.Point,
			updateEveryFrame = true,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Temperature,
			textureFormat = TextureFormat.RGB24,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		},
		new PropertyTextures.TextureProperties
		{
			simProperty = PropertyTextures.Property.Radiation,
			textureFormat = TextureFormat.RFloat,
			filterMode = FilterMode.Bilinear,
			updateEveryFrame = false,
			updatedExternally = false,
			blend = false,
			blendSpeed = 0f
		}
	};

	private List<PropertyTextures.TextureProperties> allTextureProperties = new List<PropertyTextures.TextureProperties>();

	private WorkItemCollection<PropertyTextures.WorkItem, object> workItems = new WorkItemCollection<PropertyTextures.WorkItem, object>();

	public enum Property
	{
		StateChange,
		GasPressure,
		GasColour,
		GasDanger,
		FogOfWar,
		Flow,
		SolidDigAmount,
		SolidLiquidGasMass,
		WorldLight,
		Liquid,
		Temperature,
		ExposedToSunlight,
		FallingSolid,
		Radiation,
		Num
	}

	private struct TextureProperties
	{
		public string name;

		public PropertyTextures.Property simProperty;

		public TextureFormat textureFormat;

		public FilterMode filterMode;

		public bool updateEveryFrame;

		public bool updatedExternally;

		public bool blend;

		public float blendSpeed;

		public string texturePropertyName;
	}

	private struct WorkItem : IWorkItem<object>
	{
		public WorkItem(TextureRegion texture_region, int x0, int y0, int x1, int y1, PropertyTextures.WorkItem.Callback update_texture_cb)
		{
			this.textureRegion = texture_region;
			this.x0 = x0;
			this.y0 = y0;
			this.x1 = x1;
			this.y1 = y1;
			this.updateTextureCb = update_texture_cb;
		}

		public void Run(object shared_data)
		{
			this.updateTextureCb(this.textureRegion, this.x0, this.y0, this.x1, this.y1);
		}

		private int x0;

		private int y0;

		private int x1;

		private int y1;

		private TextureRegion textureRegion;

		private PropertyTextures.WorkItem.Callback updateTextureCb;

				public delegate void Callback(TextureRegion texture_region, int x0, int y0, int x1, int y1);
	}
}
