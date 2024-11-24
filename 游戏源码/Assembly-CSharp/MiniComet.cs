using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MiniComet")]
public class MiniComet : KMonoBehaviour, ISim33ms
{
	[MyCmpGet]
	private PrimaryElement pe;

	public Vector2 spawnVelocity = new Vector2(7f, 9f);

	public Vector2 spawnAngle = new Vector2(30f, 150f);

	public SpawnFXHashes explosionEffectHash;

	public int addDiseaseCount;

	public byte diseaseIdx = byte.MaxValue;

	public Vector2I explosionOreCount = new Vector2I(1, 1);

	public Vector2 explosionSpeedRange = new Vector2(0f, 0f);

	public string impactSound;

	public string flyingSound;

	public int flyingSoundID;

	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	public bool Targeted;

	[Serialize]
	protected Vector3 offsetPosition;

	[Serialize]
	protected Vector2 velocity;

	private Vector3 previousPosition;

	private bool hasExploded;

	public string[] craterPrefabs;

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
		StartLoopingSound();
		bool flag = offsetPosition.x != 0f || offsetPosition.y != 0f;
		selectable.enabled = !flag;
		typeID = GetComponent<KPrefabID>().PrefabTag;
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
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

	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(explosionOreCount.x, explosionOreCount.y + 1);
	}

	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		_ = Grid.WorldIdx[cell];
		PlayImpactSound(pos);
		Vector3 vector = pos;
		vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (explosionEffectHash != 0)
		{
			Game.Instance.SpawnFX(explosionEffectHash, vector, 0f);
		}
		Substance substance = element.substance;
		int randomNumOres = GetRandomNumOres();
		Vector2 vector2 = -velocity.normalized;
		Vector2 vector3 = new Vector2(vector2.y, 0f - vector2.x);
		float mass = ((randomNumOres > 0) ? (pe.Mass / (float)randomNumOres) : 1f);
		for (int i = 0; i < randomNumOres; i++)
		{
			Vector2 normalized = (vector2 + vector3 * UnityEngine.Random.Range(-1f, 1f)).normalized;
			Vector3 vector4 = normalized * UnityEngine.Random.Range(explosionSpeedRange.x, explosionSpeedRange.y);
			Vector3 position = vector + (Vector3)(normalized.normalized * 1.25f);
			GameObject go = substance.SpawnResource(position, mass, pe.Temperature, pe.DiseaseIdx, pe.DiseaseCount / randomNumOres);
			if (GameComps.Fallers.Has(go))
			{
				GameComps.Fallers.Remove(go);
			}
			GameComps.Fallers.Add(go, vector4);
		}
		if (OnImpact != null)
		{
			OnImpact();
		}
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
			Grid.PosToCell(vector5);
			loopingSounds.UpdateVelocity(flyingSound, vector5 - position);
			if (vector5.x < vector3.x || vector4.x < vector5.x || vector5.y < vector3.y)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.PosToCell(previousPosition);
			if (num != num2 && Grid.IsValidCell(num) && Grid.Solid[num])
			{
				PrimaryElement component = GetComponent<PrimaryElement>();
				Explode(position, num, num2, component.Element);
				hasExploded = true;
				Util.KDestroyGameObject(base.gameObject);
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
		Util.KDestroyGameObject(base.gameObject);
	}
}
