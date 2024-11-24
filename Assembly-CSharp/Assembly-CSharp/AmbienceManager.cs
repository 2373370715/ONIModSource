﻿using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AmbienceManager")]
public class AmbienceManager : KMonoBehaviour
{
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

	private float emitterZPosition;

	public AmbienceManager.QuadrantDef[] quadrantDefs;

	public AmbienceManager.Quadrant[] quadrants = new AmbienceManager.Quadrant[4];

	public class Tuning : TuningData<AmbienceManager.Tuning>
	{
		public int backwallTileValue = 1;

		public int foundationTileValue = 2;

		public int buildingTileValue = 3;
	}

	public class Layer : IComparable<AmbienceManager.Layer>
	{
		public Layer(EventReference sound, EventReference one_shot_sound = default(EventReference))
		{
			this.sound = sound;
			this.oneShotSound = one_shot_sound;
		}

		public void Reset()
		{
			this.tileCount = 0;
			this.averageTemperature = 0f;
			this.averageRadiation = 0f;
		}

		public void UpdatePercentage(int cell_count)
		{
			this.tilePercentage = (float)this.tileCount / (float)cell_count;
		}

		public void UpdateAverageTemperature()
		{
			this.averageTemperature /= (float)this.tileCount;
			this.soundEvent.setParameterByName("averageTemperature", this.averageTemperature, false);
		}

		public void UpdateAverageRadiation()
		{
			this.averageRadiation = ((this.tileCount > 0) ? (this.averageRadiation / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("averageRadiation", this.averageRadiation, false);
		}

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

		public void SetCustomParameter(string parameterName, float value)
		{
			this.soundEvent.setParameterByName(parameterName, value, false);
		}

		public int CompareTo(AmbienceManager.Layer layer)
		{
			return layer.tileCount - this.tileCount;
		}

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

		public void Stop()
		{
			if (this.soundEvent.isValid())
			{
				this.soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.soundEvent.release();
			}
			this.isRunning = false;
		}

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

		private const string TILE_PERCENTAGE_ID = "tilePercentage";

		private const string AVERAGE_TEMPERATURE_ID = "averageTemperature";

		private const string AVERAGE_RADIATION_ID = "averageRadiation";

		public EventReference sound;

		public EventReference oneShotSound;

		public int tileCount;

		public float tilePercentage;

		public float volume;

		public bool isRunning;

		private EventInstance soundEvent;

		public float averageTemperature;

		public float averageRadiation;
	}

	[Serializable]
	public class QuadrantDef
	{
		public string name;

		public EventReference[] liquidSounds;

		public EventReference[] gasSounds;

		public EventReference[] solidSounds;

		public EventReference fogSound;

		public EventReference spaceSound;

		public EventReference rocketInteriorSound;

		public EventReference facilitySound;

		public EventReference radiationSound;
	}

	public class Quadrant
	{
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

		public List<AmbienceManager.Layer> GetAllLayers()
		{
			return this.allLayers;
		}

		public string name;

		public Vector3 emitterPosition;

		public AmbienceManager.Layer[] gasLayers = new AmbienceManager.Layer[4];

		public AmbienceManager.Layer[] liquidLayers = new AmbienceManager.Layer[4];

		public AmbienceManager.Layer fogLayer;

		public AmbienceManager.Layer spaceLayer;

		public AmbienceManager.Layer rocketInteriorLayer;

		public AmbienceManager.Layer facilityLayer;

		public AmbienceManager.Layer radiationLayer;

		public AmbienceManager.Layer[] solidLayers = new AmbienceManager.Layer[19];

		private List<AmbienceManager.Layer> allLayers = new List<AmbienceManager.Layer>();

		private List<AmbienceManager.Layer> loopingLayers = new List<AmbienceManager.Layer>();

		private List<AmbienceManager.Layer> oneShotLayers = new List<AmbienceManager.Layer>();

		private List<AmbienceManager.Layer> topLayers = new List<AmbienceManager.Layer>();

		public static int activeSolidLayerCount = 2;

		public int totalTileCount;

		private bool m_isRadiationEnabled;

		private bool m_isClusterSpaceEnabled;

		private const string ROCKET_STATE_FOR_AMBIENCE = "RocketState";

		private AmbienceManager.Quadrant.SolidTimer[] solidTimers;

		public class SolidTimer
		{
			public SolidTimer()
			{
				this.solidTargetTime = Time.unscaledTime + UnityEngine.Random.value * AmbienceManager.Quadrant.SolidTimer.solidMinTime;
			}

			public bool ShouldPlay()
			{
				if (Time.unscaledTime > this.solidTargetTime)
				{
					this.solidTargetTime = Time.unscaledTime + AmbienceManager.Quadrant.SolidTimer.solidMinTime + UnityEngine.Random.value * (AmbienceManager.Quadrant.SolidTimer.solidMaxTime - AmbienceManager.Quadrant.SolidTimer.solidMinTime);
					return true;
				}
				return false;
			}

			public static float solidMinTime = 9f;

			public static float solidMaxTime = 15f;

			public float solidTargetTime;
		}
	}
}
