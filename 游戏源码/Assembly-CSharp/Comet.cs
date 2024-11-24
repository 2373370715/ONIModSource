using System;
using System.Collections.Generic;
using FMOD.Studio;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class Comet : KMonoBehaviour, ISim33ms
{
	public SimHashes EXHAUST_ELEMENT = SimHashes.CarbonDioxide;

	public float EXHAUST_RATE = 50f;

	public Vector2 spawnVelocity = new Vector2(12f, 15f);

	public Vector2 spawnAngle = new Vector2(-100f, -80f);

	public Vector2 massRange;

	public Vector2 temperatureRange;

	public SpawnFXHashes explosionEffectHash;

	public int splashRadius = 1;

	public int addTiles;

	public int addTilesMinHeight;

	public int addTilesMaxHeight;

	public int entityDamage = 1;

	public float totalTileDamage = 0.2f;

	protected float addTileMass;

	public int addDiseaseCount;

	public byte diseaseIdx = byte.MaxValue;

	public Vector2 elementReplaceTileTemperatureRange = new Vector2(800f, 1000f);

	public Vector2I explosionOreCount = new Vector2I(0, 0);

	private float explosionMass;

	public Vector2 explosionTemperatureRange = new Vector2(500f, 700f);

	public Vector2 explosionSpeedRange = new Vector2(8f, 14f);

	public float windowDamageMultiplier = 5f;

	public float bunkerDamageMultiplier;

	public string impactSound;

	public string flyingSound;

	public int flyingSoundID;

	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	public bool affectedByDifficulty = true;

	public bool Targeted;

	[Serialize]
	protected Vector3 offsetPosition;

	[Serialize]
	protected Vector2 velocity;

	[Serialize]
	private float remainingTileDamage;

	private Vector3 previousPosition;

	private bool hasExploded;

	public bool canHitDuplicants;

	public string[] craterPrefabs;

	public string[] lootOnDestroyedByMissile;

	public bool destroyOnExplode = true;

	public bool spawnWithOffset;

	private float age;

	public System.Action OnImpact;

	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	[MyCmpGet]
	private KBatchedAnimController anim;

	[MyCmpGet]
	private KSelectable selectable;

	public Tag typeID;

	private LoopingSounds loopingSounds;

	private List<GameObject> damagedEntities = new List<GameObject>();

	private List<int> destroyedCells = new List<int>();

	private const float MAX_DISTANCE_TEST = 6f;

	public float ExplosionMass => explosionMass;

	public float AddTileMass => addTileMass;

	public Vector3 TargetPosition => anim.PositionIncludingOffset;

	public Vector2 Velocity
	{
		get
		{
			return velocity;
		}
		set
		{
			velocity = value;
		}
	}

	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && selectable != null && selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		remainingTileDamage = totalTileDamage;
		loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		flyingSound = GlobalAssets.GetSound("Meteor_LP");
		RandomizeVelocity();
	}

	protected override void OnSpawn()
	{
		anim.Offset = offsetPosition;
		if (spawnWithOffset)
		{
			SetupOffset();
		}
		base.OnSpawn();
		RandomizeMassAndTemperature();
		StartLoopingSound();
		bool flag = offsetPosition.x != 0f || offsetPosition.y != 0f;
		selectable.enabled = !flag;
		typeID = GetComponent<KPrefabID>().PrefabTag;
		Components.Meteors.Add(base.gameObject.GetMyWorldId(), this);
	}

	protected override void OnCleanUp()
	{
		Components.Meteors.Remove(base.gameObject.GetMyWorldId(), this);
	}

	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(velocity.x, velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * ((float)Math.PI / 180f);
		float num2 = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * num2;
		float num3 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (!(vector2.x >= (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters) || !(vector2.x <= num3))
		{
			float num4 = ((vector.x < 0f) ? (num3 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters));
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * ((float)Math.PI / 180f);
			num2 = Mathf.Abs(num4 / Mathf.Cos(f));
		}
		Vector3 vector3 = -vector.normalized * num2;
		Vector3 vector4 = position2 + vector3;
		vector4.z = position.z;
		offsetPosition = vector3;
		anim.Offset = offsetPosition;
	}

	public virtual void RandomizeVelocity()
	{
		float num = UnityEngine.Random.Range(spawnAngle.x, spawnAngle.y);
		float f = num * (float)Math.PI / 180f;
		float num2 = UnityEngine.Random.Range(spawnVelocity.x, spawnVelocity.y);
		velocity = new Vector2((0f - Mathf.Cos(f)) * num2, Mathf.Sin(f) * num2);
		GetComponent<KBatchedAnimController>().Rotation = 0f - num - 90f;
	}

	public void RandomizeMassAndTemperature()
	{
		float num = UnityEngine.Random.Range(massRange.x, massRange.y) * GetMassMultiplier();
		PrimaryElement component = GetComponent<PrimaryElement>();
		component.Mass = num;
		component.Temperature = UnityEngine.Random.Range(temperatureRange.x, temperatureRange.y);
		if (addTiles > 0)
		{
			float num2 = UnityEngine.Random.Range(0.95f, 0.98f);
			explosionMass = num * (1f - num2);
			addTileMass = num * num2;
		}
		else
		{
			explosionMass = num;
			addTileMass = 0f;
		}
	}

	public float GetMassMultiplier()
	{
		float num = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
		if (affectedByDifficulty && currentQualitySetting != null)
		{
			switch (currentQualitySetting.id)
			{
			case "Infrequent":
				num *= 1f;
				break;
			case "Intense":
				num *= 0.8f;
				break;
			case "Doomed":
				num *= 0.5f;
				break;
			}
		}
		return num;
	}

	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(explosionOreCount.x, explosionOreCount.y + 1);
	}

	public float GetRandomTemperatureForOres()
	{
		return UnityEngine.Random.Range(explosionTemperatureRange.x, explosionTemperatureRange.y);
	}

	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		int world = Grid.WorldIdx[cell];
		PlayImpactSound(pos);
		Vector3 pos2 = pos;
		pos2.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (explosionEffectHash != 0)
		{
			Game.Instance.SpawnFX(explosionEffectHash, pos2, 0f);
		}
		Substance substance = element.substance;
		int randomNumOres = GetRandomNumOres();
		Vector2 vector = -velocity.normalized;
		Vector2 vector2 = new Vector2(vector.y, 0f - vector.x);
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x - 3, (int)pos.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry item in pooledList)
		{
			GameObject gameObject = (item.obj as Pickupable).gameObject;
			if (!(gameObject.GetComponent<MinionIdentity>() != null) && !(gameObject.GetComponent<CreatureBrain>() != null) && gameObject.GetDef<RobotAi.Def>() == null)
			{
				Vector2 normalized = ((Vector2)(gameObject.transform.GetPosition() - pos)).normalized;
				normalized += new Vector2(0f, 0.55f);
				normalized *= 0.5f * UnityEngine.Random.Range(explosionSpeedRange.x, explosionSpeedRange.y);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				if (GameComps.Gravities.Has(gameObject))
				{
					GameComps.Gravities.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, normalized);
			}
		}
		pooledList.Recycle();
		int num = splashRadius + 1;
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				int num2 = Grid.OffsetCell(cell, j, i);
				if (Grid.IsValidCellInWorld(num2, world) && !destroyedCells.Contains(num2))
				{
					float num3 = (1f - (float)Mathf.Abs(j) / (float)num) * (1f - (float)Mathf.Abs(i) / (float)num);
					if (num3 > 0f)
					{
						DamageTiles(num2, prev_cell, num3 * totalTileDamage * 0.5f);
					}
				}
			}
		}
		float mass = ((randomNumOres > 0) ? (explosionMass / (float)randomNumOres) : 1f);
		float randomTemperatureForOres = GetRandomTemperatureForOres();
		PrimaryElement component = GetComponent<PrimaryElement>();
		for (int k = 0; k < randomNumOres; k++)
		{
			Vector2 normalized2 = (vector + vector2 * UnityEngine.Random.Range(-1f, 1f)).normalized;
			Vector3 vector3 = normalized2 * UnityEngine.Random.Range(explosionSpeedRange.x, explosionSpeedRange.y);
			Vector3 position = normalized2.normalized * 0.75f;
			position += new Vector3(0f, 0.55f, 0f);
			position += pos;
			GameObject go = substance.SpawnResource(position, mass, randomTemperatureForOres, component.DiseaseIdx, component.DiseaseCount / (randomNumOres + addTiles));
			if (GameComps.Fallers.Has(go))
			{
				GameComps.Fallers.Remove(go);
			}
			GameComps.Fallers.Add(go, vector3);
		}
		if (addTiles > 0)
		{
			DepositTiles(cell, element, world, prev_cell, randomTemperatureForOres);
		}
		SpawnCraterPrefabs();
		if (OnImpact != null)
		{
			OnImpact();
		}
	}

	protected virtual void DepositTiles(int cell, Element element, int world, int prev_cell, float temperature)
	{
		int depthOfElement = GetDepthOfElement(cell, element, world);
		float num = 1f;
		float num2 = (float)(depthOfElement - addTilesMinHeight) / (float)(addTilesMaxHeight - addTilesMinHeight);
		if (!float.IsNaN(num2))
		{
			num -= num2;
		}
		int num3 = Mathf.Min(addTiles, Mathf.Clamp(Mathf.RoundToInt((float)addTiles * num), 1, addTiles));
		HashSetPool<int, Comet>.PooledHashSet pooledHashSet = HashSetPool<int, Comet>.Allocate();
		HashSetPool<int, Comet>.PooledHashSet pooledHashSet2 = HashSetPool<int, Comet>.Allocate();
		QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
		int num4 = -1;
		int num5 = 1;
		if (velocity.x < 0f)
		{
			num4 *= -1;
			num5 *= -1;
		}
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = prev_cell,
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num4, 0)),
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num5, 0)),
			depth = 0
		});
		Func<int, bool> condition = (int cell) => Grid.IsValidCellInWorld(cell, world) && !Grid.Solid[cell];
		GameUtil.FloodFillConditional(pooledQueue, condition, pooledHashSet2, pooledHashSet, 10);
		float mass = ((num3 > 0) ? (addTileMass / (float)addTiles) : 1f);
		int disease_count = addDiseaseCount / num3;
		if (element.HasTag(GameTags.Unstable))
		{
			UnstableGroundManager component = World.Instance.GetComponent<UnstableGroundManager>();
			foreach (int item in pooledHashSet)
			{
				if (num3 <= 0)
				{
					break;
				}
				component.Spawn(item, element, mass, temperature, byte.MaxValue, 0);
				num3--;
			}
		}
		else
		{
			foreach (int item2 in pooledHashSet)
			{
				if (num3 <= 0)
				{
					break;
				}
				SimMessages.AddRemoveSubstance(item2, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, diseaseIdx, disease_count);
				num3--;
			}
		}
		pooledHashSet.Recycle();
		pooledHashSet2.Recycle();
		pooledQueue.Recycle();
	}

	protected virtual void SpawnCraterPrefabs()
	{
		if (craterPrefabs != null && craterPrefabs.Length != 0)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(craterPrefabs[UnityEngine.Random.Range(0, craterPrefabs.Length)]), Grid.CellToPos(Grid.PosToCell(this)));
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -19.5f);
			gameObject.SetActive(value: true);
		}
	}

	protected int GetDepthOfElement(int cell, Element element, int world)
	{
		int num = 0;
		int num2 = Grid.CellBelow(cell);
		while (Grid.IsValidCellInWorld(num2, world) && Grid.Element[num2] == element)
		{
			num++;
			num2 = Grid.CellBelow(num2);
		}
		return num;
	}

	[ContextMenu("DamageTiles")]
	private float DamageTiles(int cell, int prev_cell, float input_damage)
	{
		GameObject gameObject = Grid.Objects[cell, 9];
		float num = 1f;
		bool flag = false;
		if (gameObject != null)
		{
			if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Window))
			{
				num = windowDamageMultiplier;
			}
			else if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker))
			{
				num = bunkerDamageMultiplier;
				if (gameObject.GetComponent<Door>() != null)
				{
					Game.Instance.savedInfo.blockedCometWithBunkerDoor = true;
				}
			}
			SimCellOccupier component = gameObject.GetComponent<SimCellOccupier>();
			if (component != null && !component.doReplaceElement)
			{
				flag = true;
			}
		}
		Element element = ((!flag) ? Grid.Element[cell] : gameObject.GetComponent<PrimaryElement>().Element);
		if (element.strength == 0f)
		{
			return 0f;
		}
		float num2 = input_damage * num / element.strength;
		PlayTileDamageSound(element, Grid.CellToPos(cell), gameObject);
		if (num2 == 0f)
		{
			return 0f;
		}
		float num3;
		if (flag)
		{
			BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
			float a = (float)component2.HitPoints / (float)component2.MaxHitPoints;
			float f = num2 * (float)component2.MaxHitPoints;
			component2.gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = Mathf.RoundToInt(f),
				source = BUILDINGS.DAMAGESOURCES.COMET,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
			});
			num3 = Mathf.Min(a, num2);
		}
		else
		{
			num3 = WorldDamage.Instance.ApplyDamage(cell, num2, prev_cell, BUILDINGS.DAMAGESOURCES.COMET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET);
		}
		destroyedCells.Add(cell);
		float num4 = num3 / num2;
		return input_damage * (1f - num4);
	}

	private void DamageThings(Vector3 pos, int cell, int damage, GameObject ignoreObject = null)
	{
		if (damage == 0 || !Grid.IsValidCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null && gameObject != ignoreObject)
		{
			BuildingHP component = gameObject.GetComponent<BuildingHP>();
			Building component2 = gameObject.GetComponent<Building>();
			if (component != null && !damagedEntities.Contains(gameObject))
			{
				float f = (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? ((float)damage * bunkerDamageMultiplier) : ((float)damage));
				if (component2 != null && component2.Def != null)
				{
					PlayBuildingDamageSound(component2.Def, Grid.CellToPos(cell), gameObject);
				}
				component.gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
				{
					damage = Mathf.RoundToInt(f),
					source = BUILDINGS.DAMAGESOURCES.COMET,
					popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
				});
				damagedEntities.Add(gameObject);
			}
		}
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x, (int)pos.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry item in pooledList)
		{
			Pickupable pickupable = item.obj as Pickupable;
			Health component3 = pickupable.GetComponent<Health>();
			if (component3 != null && !damagedEntities.Contains(pickupable.gameObject))
			{
				float amount = (pickupable.KPrefabID.HasTag(GameTags.Bunker) ? ((float)damage * bunkerDamageMultiplier) : ((float)damage));
				component3.Damage(amount);
				damagedEntities.Add(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	public float GetDistanceFromImpact()
	{
		float num = velocity.x / velocity.y;
		Vector3 position = base.transform.GetPosition();
		float num2 = 0f;
		while (num2 > -6f)
		{
			num2 -= 1f;
			num2 = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
			float x = num2 * num;
			Vector3 vector = new Vector3(x, num2, 0f);
			int num3 = Grid.PosToCell(position + vector);
			if (Grid.IsValidCell(num3) && Grid.Solid[num3])
			{
				return vector.magnitude;
			}
		}
		return 6f;
	}

	public float GetSoundDistance()
	{
		return GetDistanceFromImpact();
	}

	private void PlayTileDamageSound(Element element, Vector3 pos, GameObject tile_go)
	{
		string text = element.substance.GetMiningBreakSound();
		if (text == null)
		{
			text = (element.HasTag(GameTags.RefinedMetal) ? "RefinedMetal" : ((!element.HasTag(GameTags.Metal)) ? "Rock" : "RawMetal"));
		}
		text = "MeteorDamage_" + text;
		text = GlobalAssets.GetSound(text);
		if ((bool)CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
		{
			float volume = GetVolume(tile_go);
			KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos), volume);
		}
	}

	private void PlayBuildingDamageSound(BuildingDef def, Vector3 pos, GameObject building_go)
	{
		if (def != null)
		{
			string sound = GlobalAssets.GetSound(StringFormatter.Combine("MeteorDamage_Building_", def.AudioCategory));
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("MeteorDamage_Building_Metal");
			}
			if (sound != null && (bool)CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, sound))
			{
				float volume = GetVolume(building_go);
				KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos), volume);
			}
		}
	}

	public void Sim33ms(float dt)
	{
		if (hasExploded)
		{
			return;
		}
		if (offsetPosition.y > 0f)
		{
			Vector3 vector = new Vector3(velocity.x * dt, velocity.y * dt, 0f);
			Vector3 vector2 = offsetPosition + vector;
			offsetPosition = vector2;
			anim.Offset = offsetPosition;
		}
		else
		{
			if (anim.Offset != Vector3.zero)
			{
				anim.Offset = Vector3.zero;
			}
			if (!selectable.enabled)
			{
				selectable.enabled = true;
			}
			Vector2 vector3 = new Vector2(Grid.WidthInCells, Grid.HeightInCells) * -0.1f;
			Vector2 vector4 = new Vector2(Grid.WidthInCells, Grid.HeightInCells) * 1.1f;
			Vector3 position = base.transform.GetPosition();
			Vector3 vector5 = position + new Vector3(velocity.x * dt, velocity.y * dt, 0f);
			int num = Grid.PosToCell(vector5);
			loopingSounds.UpdateVelocity(flyingSound, vector5 - position);
			Element element = ElementLoader.FindElementByHash(EXHAUST_ELEMENT);
			if (EXHAUST_ELEMENT != SimHashes.Void && Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				SimMessages.EmitMass(num, element.idx, dt * EXHAUST_RATE, element.defaultValues.temperature, diseaseIdx, Mathf.RoundToInt((float)addDiseaseCount * dt));
			}
			if (vector5.x < vector3.x || vector4.x < vector5.x || vector5.y < vector3.y)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
			int num2 = Grid.PosToCell(this);
			int num3 = Grid.PosToCell(previousPosition);
			if (num2 != num3)
			{
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					PrimaryElement component = GetComponent<PrimaryElement>();
					remainingTileDamage = DamageTiles(num2, num3, remainingTileDamage);
					if (remainingTileDamage <= 0f)
					{
						Explode(position, num2, num3, component.Element);
						hasExploded = true;
						if (destroyOnExplode)
						{
							Util.KDestroyGameObject(base.gameObject);
						}
						return;
					}
				}
				else
				{
					GameObject ignoreObject = ((ignoreObstacleForDamage.Get() == null) ? null : ignoreObstacleForDamage.Get().gameObject);
					DamageThings(position, num2, entityDamage, ignoreObject);
				}
			}
			if (canHitDuplicants && age > 0.25f && Grid.Objects[Grid.PosToCell(position), 0] != null)
			{
				base.transform.position = Grid.CellToPos(Grid.PosToCell(position));
				Explode(position, num2, num3, GetComponent<PrimaryElement>().Element);
				if (destroyOnExplode)
				{
					Util.KDestroyGameObject(base.gameObject);
				}
				return;
			}
			previousPosition = position;
			base.transform.SetPosition(vector5);
		}
		age += dt;
	}

	private void PlayImpactSound(Vector3 pos)
	{
		if (impactSound == null)
		{
			impactSound = "Meteor_Large_Impact";
		}
		loopingSounds.StopSound(flyingSound);
		string sound = GlobalAssets.GetSound(impactSound);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
			KFMOD.EndOneShot(instance);
		}
	}

	private void StartLoopingSound()
	{
		loopingSounds.StartSound(flyingSound);
		loopingSounds.UpdateFirstParameter(flyingSound, FLYING_SOUND_ID_PARAMETER, flyingSoundID);
	}

	public void Explode()
	{
		PrimaryElement component = GetComponent<PrimaryElement>();
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		Explode(position, num, num, component.Element);
		hasExploded = true;
		if (destroyOnExplode)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}
}
