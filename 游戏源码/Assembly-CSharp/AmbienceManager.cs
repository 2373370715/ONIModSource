using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000944 RID: 2372
[AddComponentMenu("KMonoBehaviour/scripts/AmbienceManager")]
public class AmbienceManager : KMonoBehaviour
{
	// Token: 0x06002AE0 RID: 10976 RVA: 0x001DC6EC File Offset: 0x001DA8EC
	protected override void OnSpawn()
	{
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			this.quadrants[i] = new AmbienceManager.Quadrant(this.quadrantDefs[i]);
		}
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x001DC730 File Offset: 0x001DA930
	protected override void OnForcedCleanUp()
	{
		AmbienceManager.Quadrant[] array = this.quadrants;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (AmbienceManager.Layer layer in array[i].GetAllLayers())
			{
				layer.Stop();
			}
		}
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x001DC798 File Offset: 0x001DA998
	private void LateUpdate()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = visibleArea.Min;
		Vector2I max = visibleArea.Max;
		Vector2I vector2I = min + (max - min) / 2;
		Vector3 a = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = vector + (a - vector) / 2f;
		Vector3 vector3 = a - vector;
		if (vector3.x > vector3.y)
		{
			vector3.y = vector3.x;
		}
		else
		{
			vector3.x = vector3.y;
		}
		a = vector2 + vector3 / 2f;
		vector = vector2 - vector3 / 2f;
		Vector3 vector4 = vector3 / 2f / 2f;
		this.quadrants[0].Update(new Vector2I(min.x, min.y), new Vector2I(vector2I.x, vector2I.y), new Vector3(vector.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[1].Update(new Vector2I(vector2I.x, min.y), new Vector2I(max.x, vector2I.y), new Vector3(vector2.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[2].Update(new Vector2I(min.x, vector2I.y), new Vector2I(vector2I.x, max.y), new Vector3(vector.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		this.quadrants[3].Update(new Vector2I(vector2I.x, vector2I.y), new Vector2I(max.x, max.y), new Vector3(vector2.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			num += (float)this.quadrants[i].spaceLayer.tileCount;
			num2 += (float)this.quadrants[i].facilityLayer.tileCount;
			num3 += (float)this.quadrants[i].totalTileCount;
		}
		AudioMixer.instance.UpdateSpaceVisibleSnapshot(num / num3);
		AudioMixer.instance.UpdateFacilityVisibleSnapshot(num2 / num3);
	}

	// Token: 0x04001C83 RID: 7299
	private float emitterZPosition;

	// Token: 0x04001C84 RID: 7300
	public AmbienceManager.QuadrantDef[] quadrantDefs;

	// Token: 0x04001C85 RID: 7301
	public AmbienceManager.Quadrant[] quadrants = new AmbienceManager.Quadrant[4];

	// Token: 0x02000945 RID: 2373
	public class Tuning : TuningData<AmbienceManager.Tuning>
	{
		// Token: 0x04001C86 RID: 7302
		public int backwallTileValue = 1;

		// Token: 0x04001C87 RID: 7303
		public int foundationTileValue = 2;

		// Token: 0x04001C88 RID: 7304
		public int buildingTileValue = 3;
	}

	// Token: 0x02000946 RID: 2374
	public class Layer : IComparable<AmbienceManager.Layer>
	{
		// Token: 0x06002AE5 RID: 10981 RVA: 0x000BBD5C File Offset: 0x000B9F5C
		public Layer(EventReference sound, EventReference one_shot_sound = default(EventReference))
		{
			this.sound = sound;
			this.oneShotSound = one_shot_sound;
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000BBD72 File Offset: 0x000B9F72
		public void Reset()
		{
			this.tileCount = 0;
			this.averageTemperature = 0f;
			this.averageRadiation = 0f;
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x000BBD91 File Offset: 0x000B9F91
		public void UpdatePercentage(int cell_count)
		{
			this.tilePercentage = (float)this.tileCount / (float)cell_count;
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x000BBDA3 File Offset: 0x000B9FA3
		public void UpdateAverageTemperature()
		{
			this.averageTemperature /= (float)this.tileCount;
			this.soundEvent.setParameterByName("averageTemperature", this.averageTemperature, false);
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x000BBDD1 File Offset: 0x000B9FD1
		public void UpdateAverageRadiation()
		{
			this.averageRadiation = ((this.tileCount > 0) ? (this.averageRadiation / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("averageRadiation", this.averageRadiation, false);
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x001DCAB4 File Offset: 0x001DACB4
		public void UpdateParameters(Vector3 emitter_position)
		{
			if (!this.soundEvent.isValid())
			{
				return;
			}
			Vector3 pos = new Vector3(emitter_position.x, emitter_position.y, 0f);
			this.soundEvent.set3DAttributes(pos.To3DAttributes());
			this.soundEvent.setParameterByName("tilePercentage", this.tilePercentage, false);
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x000BBE0F File Offset: 0x000BA00F
		public void SetCustomParameter(string parameterName, float value)
		{
			this.soundEvent.setParameterByName(parameterName, value, false);
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x000BBE20 File Offset: 0x000BA020
		public int CompareTo(AmbienceManager.Layer layer)
		{
			return layer.tileCount - this.tileCount;
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x000BBE2F File Offset: 0x000BA02F
		public void SetVolume(float volume)
		{
			if (this.volume != volume)
			{
				this.volume = volume;
				if (this.soundEvent.isValid())
				{
					this.soundEvent.setVolume(volume);
				}
			}
		}

		// Token: 0x06002AEE RID: 10990 RVA: 0x000BBE5B File Offset: 0x000BA05B
		public void Stop()
		{
			if (this.soundEvent.isValid())
			{
				this.soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.soundEvent.release();
			}
			this.isRunning = false;
		}

		// Token: 0x06002AEF RID: 10991 RVA: 0x001DCB14 File Offset: 0x001DAD14
		public void Start(Vector3 emitter_position)
		{
			if (!this.isRunning)
			{
				if (!this.oneShotSound.IsNull)
				{
					EventInstance eventInstance = KFMOD.CreateInstance(this.oneShotSound);
					if (!eventInstance.isValid())
					{
						string str = "Could not find event: ";
						EventReference eventReference = this.oneShotSound;
						global::Debug.LogWarning(str + eventReference.ToString());
						return;
					}
					ATTRIBUTES_3D attributes = new Vector3(emitter_position.x, emitter_position.y, 0f).To3DAttributes();
					eventInstance.set3DAttributes(attributes);
					eventInstance.setVolume(this.tilePercentage * 2f);
					eventInstance.start();
					eventInstance.release();
					return;
				}
				else
				{
					this.soundEvent = KFMOD.CreateInstance(this.sound);
					if (this.soundEvent.isValid())
					{
						this.soundEvent.start();
					}
					this.isRunning = true;
				}
			}
		}

		// Token: 0x04001C89 RID: 7305
		private const string TILE_PERCENTAGE_ID = "tilePercentage";

		// Token: 0x04001C8A RID: 7306
		private const string AVERAGE_TEMPERATURE_ID = "averageTemperature";

		// Token: 0x04001C8B RID: 7307
		private const string AVERAGE_RADIATION_ID = "averageRadiation";

		// Token: 0x04001C8C RID: 7308
		public EventReference sound;

		// Token: 0x04001C8D RID: 7309
		public EventReference oneShotSound;

		// Token: 0x04001C8E RID: 7310
		public int tileCount;

		// Token: 0x04001C8F RID: 7311
		public float tilePercentage;

		// Token: 0x04001C90 RID: 7312
		public float volume;

		// Token: 0x04001C91 RID: 7313
		public bool isRunning;

		// Token: 0x04001C92 RID: 7314
		private EventInstance soundEvent;

		// Token: 0x04001C93 RID: 7315
		public float averageTemperature;

		// Token: 0x04001C94 RID: 7316
		public float averageRadiation;
	}

	// Token: 0x02000947 RID: 2375
	[Serializable]
	public class QuadrantDef
	{
		// Token: 0x04001C95 RID: 7317
		public string name;

		// Token: 0x04001C96 RID: 7318
		public EventReference[] liquidSounds;

		// Token: 0x04001C97 RID: 7319
		public EventReference[] gasSounds;

		// Token: 0x04001C98 RID: 7320
		public EventReference[] solidSounds;

		// Token: 0x04001C99 RID: 7321
		public EventReference fogSound;

		// Token: 0x04001C9A RID: 7322
		public EventReference spaceSound;

		// Token: 0x04001C9B RID: 7323
		public EventReference rocketInteriorSound;

		// Token: 0x04001C9C RID: 7324
		public EventReference facilitySound;

		// Token: 0x04001C9D RID: 7325
		public EventReference radiationSound;
	}

	// Token: 0x02000948 RID: 2376
	public class Quadrant
	{
		// Token: 0x06002AF1 RID: 10993 RVA: 0x001DCBF0 File Offset: 0x001DADF0
		public Quadrant(AmbienceManager.QuadrantDef def)
		{
			this.name = def.name;
			this.fogLayer = new AmbienceManager.Layer(def.fogSound, default(EventReference));
			this.allLayers.Add(this.fogLayer);
			this.loopingLayers.Add(this.fogLayer);
			this.spaceLayer = new AmbienceManager.Layer(def.spaceSound, default(EventReference));
			this.allLayers.Add(this.spaceLayer);
			this.loopingLayers.Add(this.spaceLayer);
			this.m_isClusterSpaceEnabled = DlcManager.FeatureClusterSpaceEnabled();
			if (this.m_isClusterSpaceEnabled)
			{
				this.rocketInteriorLayer = new AmbienceManager.Layer(def.rocketInteriorSound, default(EventReference));
				this.allLayers.Add(this.rocketInteriorLayer);
			}
			this.facilityLayer = new AmbienceManager.Layer(def.facilitySound, default(EventReference));
			this.allLayers.Add(this.facilityLayer);
			this.loopingLayers.Add(this.facilityLayer);
			this.m_isRadiationEnabled = Sim.IsRadiationEnabled();
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer = new AmbienceManager.Layer(def.radiationSound, default(EventReference));
				this.allLayers.Add(this.radiationLayer);
			}
			for (int i = 0; i < 4; i++)
			{
				this.gasLayers[i] = new AmbienceManager.Layer(def.gasSounds[i], default(EventReference));
				this.liquidLayers[i] = new AmbienceManager.Layer(def.liquidSounds[i], default(EventReference));
				this.allLayers.Add(this.gasLayers[i]);
				this.allLayers.Add(this.liquidLayers[i]);
				this.loopingLayers.Add(this.gasLayers[i]);
				this.loopingLayers.Add(this.liquidLayers[i]);
			}
			for (int j = 0; j < this.solidLayers.Length; j++)
			{
				if (j >= def.solidSounds.Length)
				{
					string str = "Missing solid layer: ";
					SolidAmbienceType solidAmbienceType = (SolidAmbienceType)j;
					global::Debug.LogError(str + solidAmbienceType.ToString());
				}
				this.solidLayers[j] = new AmbienceManager.Layer(default(EventReference), def.solidSounds[j]);
				this.allLayers.Add(this.solidLayers[j]);
				this.oneShotLayers.Add(this.solidLayers[j]);
			}
			this.solidTimers = new AmbienceManager.Quadrant.SolidTimer[AmbienceManager.Quadrant.activeSolidLayerCount];
			for (int k = 0; k < AmbienceManager.Quadrant.activeSolidLayerCount; k++)
			{
				this.solidTimers[k] = new AmbienceManager.Quadrant.SolidTimer();
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x001DCEE8 File Offset: 0x001DB0E8
		public void Update(Vector2I min, Vector2I max, Vector3 emitter_position)
		{
			this.emitterPosition = emitter_position;
			this.totalTileCount = 0;
			for (int i = 0; i < this.allLayers.Count; i++)
			{
				this.allLayers[i].Reset();
			}
			for (int j = min.y; j < max.y; j++)
			{
				if (j % 2 != 1)
				{
					for (int k = min.x; k < max.x; k++)
					{
						if (k % 2 != 0)
						{
							int num = Grid.XYToCell(k, j);
							if (Grid.IsValidCell(num))
							{
								this.totalTileCount++;
								if (Grid.IsVisible(num))
								{
									if (Grid.GravitasFacility[num])
									{
										this.facilityLayer.tileCount += 8;
									}
									else
									{
										Element element = Grid.Element[num];
										if (element != null)
										{
											if (element.IsLiquid && Grid.IsSubstantialLiquid(num, 0.35f))
											{
												AmbienceType ambience = element.substance.GetAmbience();
												if (ambience != AmbienceType.None)
												{
													this.liquidLayers[(int)ambience].tileCount++;
													this.liquidLayers[(int)ambience].averageTemperature += Grid.Temperature[num];
												}
											}
											else if (element.IsGas)
											{
												AmbienceType ambience2 = element.substance.GetAmbience();
												if (ambience2 != AmbienceType.None)
												{
													this.gasLayers[(int)ambience2].tileCount++;
													this.gasLayers[(int)ambience2].averageTemperature += Grid.Temperature[num];
												}
											}
											else if (element.IsSolid)
											{
												SolidAmbienceType solidAmbienceType = element.substance.GetSolidAmbience();
												if (Grid.Foundation[num])
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
												}
												else if (Grid.Objects[num, 2] != null)
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
												}
												else if (solidAmbienceType != SolidAmbienceType.None)
												{
													this.solidLayers[(int)solidAmbienceType].tileCount++;
												}
												else if (element.id == SimHashes.Regolith || element.id == SimHashes.MaficRock)
												{
													this.spaceLayer.tileCount++;
												}
											}
											else if (element.id == SimHashes.Vacuum && CellSelectionObject.IsExposedToSpace(num))
											{
												if (Grid.Objects[num, 1] != null)
												{
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().buildingTileValue;
												}
												this.spaceLayer.tileCount++;
											}
										}
									}
									if (Grid.Radiation[num] > 0f)
									{
										this.radiationLayer.averageRadiation += Grid.Radiation[num];
										this.radiationLayer.tileCount++;
									}
								}
								else
								{
									this.fogLayer.tileCount++;
								}
							}
						}
					}
				}
			}
			Vector2I vector2I = max - min;
			int cell_count = vector2I.x * vector2I.y;
			for (int l = 0; l < this.allLayers.Count; l++)
			{
				this.allLayers[l].UpdatePercentage(cell_count);
			}
			this.loopingLayers.Sort();
			this.topLayers.Clear();
			for (int m = 0; m < this.loopingLayers.Count; m++)
			{
				AmbienceManager.Layer layer = this.loopingLayers[m];
				if (m < 3 && layer.tilePercentage > 0f)
				{
					layer.Start(emitter_position);
					layer.UpdateAverageTemperature();
					layer.UpdateParameters(emitter_position);
					this.topLayers.Add(layer);
				}
				else
				{
					layer.Stop();
				}
			}
			if (this.m_isClusterSpaceEnabled)
			{
				float volume = 0f;
				if (ClusterManager.Instance != null && ClusterManager.Instance.activeWorld != null && ClusterManager.Instance.activeWorld.IsModuleInterior)
				{
					volume = 1f;
				}
				this.rocketInteriorLayer.Start(emitter_position);
				this.rocketInteriorLayer.SetCustomParameter("RocketState", (float)ClusterManager.RocketInteriorState);
				this.rocketInteriorLayer.SetVolume(volume);
			}
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer.Start(emitter_position);
				this.radiationLayer.UpdateAverageRadiation();
				this.radiationLayer.UpdateParameters(emitter_position);
			}
			this.oneShotLayers.Sort();
			for (int n = 0; n < AmbienceManager.Quadrant.activeSolidLayerCount; n++)
			{
				if (this.solidTimers[n].ShouldPlay() && this.oneShotLayers[n].tilePercentage > 0f)
				{
					this.oneShotLayers[n].Start(emitter_position);
				}
			}
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000BBE8A File Offset: 0x000BA08A
		public List<AmbienceManager.Layer> GetAllLayers()
		{
			return this.allLayers;
		}

		// Token: 0x04001C9E RID: 7326
		public string name;

		// Token: 0x04001C9F RID: 7327
		public Vector3 emitterPosition;

		// Token: 0x04001CA0 RID: 7328
		public AmbienceManager.Layer[] gasLayers = new AmbienceManager.Layer[4];

		// Token: 0x04001CA1 RID: 7329
		public AmbienceManager.Layer[] liquidLayers = new AmbienceManager.Layer[4];

		// Token: 0x04001CA2 RID: 7330
		public AmbienceManager.Layer fogLayer;

		// Token: 0x04001CA3 RID: 7331
		public AmbienceManager.Layer spaceLayer;

		// Token: 0x04001CA4 RID: 7332
		public AmbienceManager.Layer rocketInteriorLayer;

		// Token: 0x04001CA5 RID: 7333
		public AmbienceManager.Layer facilityLayer;

		// Token: 0x04001CA6 RID: 7334
		public AmbienceManager.Layer radiationLayer;

		// Token: 0x04001CA7 RID: 7335
		public AmbienceManager.Layer[] solidLayers = new AmbienceManager.Layer[20];

		// Token: 0x04001CA8 RID: 7336
		private List<AmbienceManager.Layer> allLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001CA9 RID: 7337
		private List<AmbienceManager.Layer> loopingLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001CAA RID: 7338
		private List<AmbienceManager.Layer> oneShotLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001CAB RID: 7339
		private List<AmbienceManager.Layer> topLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001CAC RID: 7340
		public static int activeSolidLayerCount = 2;

		// Token: 0x04001CAD RID: 7341
		public int totalTileCount;

		// Token: 0x04001CAE RID: 7342
		private bool m_isRadiationEnabled;

		// Token: 0x04001CAF RID: 7343
		private bool m_isClusterSpaceEnabled;

		// Token: 0x04001CB0 RID: 7344
		private const string ROCKET_STATE_FOR_AMBIENCE = "RocketState";

		// Token: 0x04001CB1 RID: 7345
		private AmbienceManager.Quadrant.SolidTimer[] solidTimers;

		// Token: 0x02000949 RID: 2377
		public class SolidTimer
		{
			// Token: 0x06002AF5 RID: 10997 RVA: 0x000BBE9A File Offset: 0x000BA09A
			public SolidTimer()
			{
				this.solidTargetTime = Time.unscaledTime + UnityEngine.Random.value * AmbienceManager.Quadrant.SolidTimer.solidMinTime;
			}

			// Token: 0x06002AF6 RID: 10998 RVA: 0x000BBEB9 File Offset: 0x000BA0B9
			public bool ShouldPlay()
			{
				if (Time.unscaledTime > this.solidTargetTime)
				{
					this.solidTargetTime = Time.unscaledTime + AmbienceManager.Quadrant.SolidTimer.solidMinTime + UnityEngine.Random.value * (AmbienceManager.Quadrant.SolidTimer.solidMaxTime - AmbienceManager.Quadrant.SolidTimer.solidMinTime);
					return true;
				}
				return false;
			}

			// Token: 0x04001CB2 RID: 7346
			public static float solidMinTime = 9f;

			// Token: 0x04001CB3 RID: 7347
			public static float solidMaxTime = 15f;

			// Token: 0x04001CB4 RID: 7348
			public float solidTargetTime;
		}
	}
}
