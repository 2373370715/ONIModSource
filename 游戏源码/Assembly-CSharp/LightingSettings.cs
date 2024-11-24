using System;
using UnityEngine;

// Token: 0x02001784 RID: 6020
public class LightingSettings : ScriptableObject
{
	// Token: 0x04005D30 RID: 23856
	[Header("Global")]
	public bool UpdateLightSettings;

	// Token: 0x04005D31 RID: 23857
	public float BloomScale;

	// Token: 0x04005D32 RID: 23858
	public Color32 LightColour = Color.white;

	// Token: 0x04005D33 RID: 23859
	[Header("Digging")]
	public float DigMapScale;

	// Token: 0x04005D34 RID: 23860
	public Color DigMapColour;

	// Token: 0x04005D35 RID: 23861
	public Texture2D DigDamageMap;

	// Token: 0x04005D36 RID: 23862
	[Header("State Transition")]
	public Texture2D StateTransitionMap;

	// Token: 0x04005D37 RID: 23863
	public Color StateTransitionColor;

	// Token: 0x04005D38 RID: 23864
	public float StateTransitionUVScale;

	// Token: 0x04005D39 RID: 23865
	public Vector2 StateTransitionUVOffsetRate;

	// Token: 0x04005D3A RID: 23866
	[Header("Falling Solids")]
	public Texture2D FallingSolidMap;

	// Token: 0x04005D3B RID: 23867
	public Color FallingSolidColor;

	// Token: 0x04005D3C RID: 23868
	public float FallingSolidUVScale;

	// Token: 0x04005D3D RID: 23869
	public Vector2 FallingSolidUVOffsetRate;

	// Token: 0x04005D3E RID: 23870
	[Header("Metal Shine")]
	public Vector2 ShineCenter;

	// Token: 0x04005D3F RID: 23871
	public Vector2 ShineRange;

	// Token: 0x04005D40 RID: 23872
	public float ShineZoomSpeed;

	// Token: 0x04005D41 RID: 23873
	[Header("Water")]
	public Color WaterTrimColor;

	// Token: 0x04005D42 RID: 23874
	public float WaterTrimSize;

	// Token: 0x04005D43 RID: 23875
	public float WaterAlphaTrimSize;

	// Token: 0x04005D44 RID: 23876
	public float WaterAlphaThreshold;

	// Token: 0x04005D45 RID: 23877
	public float WaterCubesAlphaThreshold;

	// Token: 0x04005D46 RID: 23878
	public float WaterWaveAmplitude;

	// Token: 0x04005D47 RID: 23879
	public float WaterWaveFrequency;

	// Token: 0x04005D48 RID: 23880
	public float WaterWaveSpeed;

	// Token: 0x04005D49 RID: 23881
	public float WaterDetailSpeed;

	// Token: 0x04005D4A RID: 23882
	public float WaterDetailTiling;

	// Token: 0x04005D4B RID: 23883
	public float WaterDetailTiling2;

	// Token: 0x04005D4C RID: 23884
	public Vector2 WaterDetailDirection;

	// Token: 0x04005D4D RID: 23885
	public float WaterWaveAmplitude2;

	// Token: 0x04005D4E RID: 23886
	public float WaterWaveFrequency2;

	// Token: 0x04005D4F RID: 23887
	public float WaterWaveSpeed2;

	// Token: 0x04005D50 RID: 23888
	public float WaterCubeMapScale;

	// Token: 0x04005D51 RID: 23889
	public float WaterColorScale;

	// Token: 0x04005D52 RID: 23890
	public float WaterDistortionScaleStart;

	// Token: 0x04005D53 RID: 23891
	public float WaterDistortionScaleEnd;

	// Token: 0x04005D54 RID: 23892
	public float WaterDepthColorOpacityStart;

	// Token: 0x04005D55 RID: 23893
	public float WaterDepthColorOpacityEnd;

	// Token: 0x04005D56 RID: 23894
	[Header("Liquid")]
	public float LiquidMin;

	// Token: 0x04005D57 RID: 23895
	public float LiquidMax;

	// Token: 0x04005D58 RID: 23896
	public float LiquidCutoff;

	// Token: 0x04005D59 RID: 23897
	public float LiquidTransparency;

	// Token: 0x04005D5A RID: 23898
	public float LiquidAmountOffset;

	// Token: 0x04005D5B RID: 23899
	public float LiquidMaxMass;

	// Token: 0x04005D5C RID: 23900
	[Header("Grid")]
	public float GridLineWidth;

	// Token: 0x04005D5D RID: 23901
	public float GridSize;

	// Token: 0x04005D5E RID: 23902
	public float GridMaxIntensity;

	// Token: 0x04005D5F RID: 23903
	public float GridMinIntensity;

	// Token: 0x04005D60 RID: 23904
	public Color GridColor;

	// Token: 0x04005D61 RID: 23905
	[Header("Terrain")]
	public float EdgeGlowCutoffStart;

	// Token: 0x04005D62 RID: 23906
	public float EdgeGlowCutoffEnd;

	// Token: 0x04005D63 RID: 23907
	public float EdgeGlowIntensity;

	// Token: 0x04005D64 RID: 23908
	public int BackgroundLayers;

	// Token: 0x04005D65 RID: 23909
	public float BackgroundBaseParallax;

	// Token: 0x04005D66 RID: 23910
	public float BackgroundLayerParallax;

	// Token: 0x04005D67 RID: 23911
	public float BackgroundDarkening;

	// Token: 0x04005D68 RID: 23912
	public float BackgroundClip;

	// Token: 0x04005D69 RID: 23913
	public float BackgroundUVScale;

	// Token: 0x04005D6A RID: 23914
	public global::LightingSettings.EdgeLighting substanceEdgeParameters;

	// Token: 0x04005D6B RID: 23915
	public global::LightingSettings.EdgeLighting tileEdgeParameters;

	// Token: 0x04005D6C RID: 23916
	public float AnimIntensity;

	// Token: 0x04005D6D RID: 23917
	public float GasMinOpacity;

	// Token: 0x04005D6E RID: 23918
	public float GasMaxOpacity;

	// Token: 0x04005D6F RID: 23919
	public Color[] DarkenTints;

	// Token: 0x04005D70 RID: 23920
	public global::LightingSettings.LightingColours characterLighting;

	// Token: 0x04005D71 RID: 23921
	public Color BrightenOverlayColour;

	// Token: 0x04005D72 RID: 23922
	public Color[] ColdColours;

	// Token: 0x04005D73 RID: 23923
	public Color[] HotColours;

	// Token: 0x04005D74 RID: 23924
	[Header("Temperature Overlay Effects")]
	public Vector4 TemperatureParallax;

	// Token: 0x04005D75 RID: 23925
	public Texture2D EmberTex;

	// Token: 0x04005D76 RID: 23926
	public Texture2D FrostTex;

	// Token: 0x04005D77 RID: 23927
	public Texture2D Thermal1Tex;

	// Token: 0x04005D78 RID: 23928
	public Texture2D Thermal2Tex;

	// Token: 0x04005D79 RID: 23929
	public Vector2 ColdFGUVOffset;

	// Token: 0x04005D7A RID: 23930
	public Vector2 ColdMGUVOffset;

	// Token: 0x04005D7B RID: 23931
	public Vector2 ColdBGUVOffset;

	// Token: 0x04005D7C RID: 23932
	public Vector2 HotFGUVOffset;

	// Token: 0x04005D7D RID: 23933
	public Vector2 HotMGUVOffset;

	// Token: 0x04005D7E RID: 23934
	public Vector2 HotBGUVOffset;

	// Token: 0x04005D7F RID: 23935
	public Texture2D DustTex;

	// Token: 0x04005D80 RID: 23936
	public Color DustColour;

	// Token: 0x04005D81 RID: 23937
	public float DustScale;

	// Token: 0x04005D82 RID: 23938
	public Vector3 DustMovement;

	// Token: 0x04005D83 RID: 23939
	public float ShowGas;

	// Token: 0x04005D84 RID: 23940
	public float ShowTemperature;

	// Token: 0x04005D85 RID: 23941
	public float ShowDust;

	// Token: 0x04005D86 RID: 23942
	public float ShowShadow;

	// Token: 0x04005D87 RID: 23943
	public Vector4 HeatHazeParameters;

	// Token: 0x04005D88 RID: 23944
	public Texture2D HeatHazeTexture;

	// Token: 0x04005D89 RID: 23945
	[Header("Biome")]
	public float WorldZoneGasBlend;

	// Token: 0x04005D8A RID: 23946
	public float WorldZoneLiquidBlend;

	// Token: 0x04005D8B RID: 23947
	public float WorldZoneForegroundBlend;

	// Token: 0x04005D8C RID: 23948
	public float WorldZoneSimpleAnimBlend;

	// Token: 0x04005D8D RID: 23949
	public float WorldZoneAnimBlend;

	// Token: 0x04005D8E RID: 23950
	[Header("FX")]
	public Color32 SmokeDamageTint;

	// Token: 0x04005D8F RID: 23951
	[Header("Building Damage")]
	public Texture2D BuildingDamagedTex;

	// Token: 0x04005D90 RID: 23952
	public Vector4 BuildingDamagedUVParameters;

	// Token: 0x04005D91 RID: 23953
	[Header("Disease")]
	public Texture2D DiseaseOverlayTex;

	// Token: 0x04005D92 RID: 23954
	public Vector4 DiseaseOverlayTexInfo;

	// Token: 0x04005D93 RID: 23955
	[Header("Conduits")]
	public ConduitFlowVisualizer.Tuning GasConduit;

	// Token: 0x04005D94 RID: 23956
	public ConduitFlowVisualizer.Tuning LiquidConduit;

	// Token: 0x04005D95 RID: 23957
	public SolidConduitFlowVisualizer.Tuning SolidConduit;

	// Token: 0x04005D96 RID: 23958
	[Header("Radiation Overlay")]
	public bool ShowRadiation;

	// Token: 0x04005D97 RID: 23959
	public Texture2D Radiation1Tex;

	// Token: 0x04005D98 RID: 23960
	public Texture2D Radiation2Tex;

	// Token: 0x04005D99 RID: 23961
	public Texture2D Radiation3Tex;

	// Token: 0x04005D9A RID: 23962
	public Texture2D Radiation4Tex;

	// Token: 0x04005D9B RID: 23963
	public Texture2D NoiseTex;

	// Token: 0x04005D9C RID: 23964
	public Color RadColor;

	// Token: 0x04005D9D RID: 23965
	public Vector2 Rad1UVOffset;

	// Token: 0x04005D9E RID: 23966
	public Vector2 Rad2UVOffset;

	// Token: 0x04005D9F RID: 23967
	public Vector2 Rad3UVOffset;

	// Token: 0x04005DA0 RID: 23968
	public Vector2 Rad4UVOffset;

	// Token: 0x04005DA1 RID: 23969
	public Vector4 RadUVScales;

	// Token: 0x04005DA2 RID: 23970
	public Vector2 Rad1Range;

	// Token: 0x04005DA3 RID: 23971
	public Vector2 Rad2Range;

	// Token: 0x04005DA4 RID: 23972
	public Vector2 Rad3Range;

	// Token: 0x04005DA5 RID: 23973
	public Vector2 Rad4Range;

	// Token: 0x02001785 RID: 6021
	[Serializable]
	public struct EdgeLighting
	{
		// Token: 0x04005DA6 RID: 23974
		public float intensity;

		// Token: 0x04005DA7 RID: 23975
		public float edgeIntensity;

		// Token: 0x04005DA8 RID: 23976
		public float directSunlightScale;

		// Token: 0x04005DA9 RID: 23977
		public float power;
	}

	// Token: 0x02001786 RID: 6022
	public enum TintLayers
	{
		// Token: 0x04005DAB RID: 23979
		Background,
		// Token: 0x04005DAC RID: 23980
		Midground,
		// Token: 0x04005DAD RID: 23981
		Foreground,
		// Token: 0x04005DAE RID: 23982
		NumLayers
	}

	// Token: 0x02001787 RID: 6023
	[Serializable]
	public struct LightingColours
	{
		// Token: 0x04005DAF RID: 23983
		public Color32 litColour;

		// Token: 0x04005DB0 RID: 23984
		public Color32 unlitColour;
	}
}
