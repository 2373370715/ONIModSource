using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x020014E3 RID: 5347
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MiniComet")]
public class MiniComet : KMonoBehaviour, ISim33ms
{
	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06006F3F RID: 28479 RVA: 0x000E8D5E File Offset: 0x000E6F5E
	public Vector3 TargetPosition
	{
		get
		{
			return this.anim.PositionIncludingOffset;
		}
	}

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06006F40 RID: 28480 RVA: 0x000E8D6B File Offset: 0x000E6F6B
	// (set) Token: 0x06006F41 RID: 28481 RVA: 0x000E8D73 File Offset: 0x000E6F73
	public Vector2 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x06006F42 RID: 28482 RVA: 0x002F24A0 File Offset: 0x002F06A0
	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && this.selectable != null && this.selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06006F43 RID: 28483 RVA: 0x000E8D7C File Offset: 0x000E6F7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Meteor_LP", false);
		this.RandomizeVelocity();
	}

	// Token: 0x06006F44 RID: 28484 RVA: 0x002F24E0 File Offset: 0x002F06E0
	protected override void OnSpawn()
	{
		this.anim.Offset = this.offsetPosition;
		if (this.spawnWithOffset)
		{
			this.SetupOffset();
		}
		base.OnSpawn();
		this.StartLoopingSound();
		bool flag = this.offsetPosition.x != 0f || this.offsetPosition.y != 0f;
		this.selectable.enabled = !flag;
		this.typeID = base.GetComponent<KPrefabID>().PrefabTag;
	}

	// Token: 0x06006F45 RID: 28485 RVA: 0x000BFD4F File Offset: 0x000BDF4F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06006F46 RID: 28486 RVA: 0x002F2564 File Offset: 0x002F0764
	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(this.velocity.x, this.velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * 0.017453292f;
		float d = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * d;
		float num2 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (vector2.x < (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters || vector2.x > num2)
		{
			float num3 = (vector.x < 0f) ? (num2 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters);
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * 0.017453292f;
			d = Mathf.Abs(num3 / Mathf.Cos(f));
		}
		Vector3 b = -vector.normalized * d;
		(position2 + b).z = position.z;
		this.offsetPosition = b;
		this.anim.Offset = this.offsetPosition;
	}

	// Token: 0x06006F47 RID: 28487 RVA: 0x002F2728 File Offset: 0x002F0928
	public virtual void RandomizeVelocity()
	{
		float num = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
		float f = num * 3.1415927f / 180f;
		float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
		this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
		base.GetComponent<KBatchedAnimController>().Rotation = -num - 90f;
	}

	// Token: 0x06006F48 RID: 28488 RVA: 0x000E8DAC File Offset: 0x000E6FAC
	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
	}

	// Token: 0x06006F49 RID: 28489 RVA: 0x002F27AC File Offset: 0x002F09AC
	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		byte b = Grid.WorldIdx[cell];
		this.PlayImpactSound(pos);
		Vector3 vector = pos;
		vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (this.explosionEffectHash != SpawnFXHashes.None)
		{
			Game.Instance.SpawnFX(this.explosionEffectHash, vector, 0f);
		}
		if (element != null)
		{
			Substance substance = element.substance;
			int randomNumOres = this.GetRandomNumOres();
			Vector2 vector2 = -this.velocity.normalized;
			Vector2 a = new Vector2(vector2.y, -vector2.x);
			float mass = (randomNumOres > 0) ? (this.pe.Mass / (float)randomNumOres) : 1f;
			for (int i = 0; i < randomNumOres; i++)
			{
				Vector2 normalized = (vector2 + a * UnityEngine.Random.Range(-1f, 1f)).normalized;
				Vector3 v = normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
				Vector3 position = vector + normalized.normalized * 1.25f;
				GameObject go = substance.SpawnResource(position, mass, this.pe.Temperature, this.pe.DiseaseIdx, this.pe.DiseaseCount / randomNumOres, false, false, false);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, v);
			}
		}
		if (this.OnImpact != null)
		{
			this.OnImpact();
		}
	}

	// Token: 0x06006F4A RID: 28490 RVA: 0x002F2944 File Offset: 0x002F0B44
	public float GetDistanceFromImpact()
	{
		float num = this.velocity.x / this.velocity.y;
		Vector3 position = base.transform.GetPosition();
		float num2 = 0f;
		while (num2 > -6f)
		{
			num2 -= 1f;
			num2 = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
			float x = num2 * num;
			Vector3 b = new Vector3(x, num2, 0f);
			int num3 = Grid.PosToCell(position + b);
			if (Grid.IsValidCell(num3) && Grid.Solid[num3])
			{
				return b.magnitude;
			}
		}
		return 6f;
	}

	// Token: 0x06006F4B RID: 28491 RVA: 0x000E8DCB File Offset: 0x000E6FCB
	public float GetSoundDistance()
	{
		return this.GetDistanceFromImpact();
	}

	// Token: 0x06006F4C RID: 28492 RVA: 0x002F29F0 File Offset: 0x002F0BF0
	public void Sim33ms(float dt)
	{
		if (this.hasExploded)
		{
			return;
		}
		if (this.offsetPosition.y > 0f)
		{
			Vector3 b = new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Vector3 vector = this.offsetPosition + b;
			this.offsetPosition = vector;
			this.anim.Offset = this.offsetPosition;
		}
		else
		{
			if (this.anim.Offset != Vector3.zero)
			{
				this.anim.Offset = Vector3.zero;
			}
			if (!this.selectable.enabled)
			{
				this.selectable.enabled = true;
			}
			Vector2 vector2 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * -0.1f;
			Vector2 vector3 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * 1.1f;
			Vector3 position = base.transform.GetPosition();
			Vector3 vector4 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Grid.PosToCell(vector4);
			this.loopingSounds.UpdateVelocity(this.flyingSound, vector4 - position);
			if (vector4.x < vector2.x || vector3.x < vector4.x || vector4.y < vector2.y)
			{
				global::Util.KDestroyGameObject(base.gameObject);
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.PosToCell(this.previousPosition);
			if (num != num2 && Grid.IsValidCell(num) && Grid.Solid[num])
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				this.Explode(position, num, num2, component.Element);
				this.hasExploded = true;
				global::Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.previousPosition = position;
			base.transform.SetPosition(vector4);
		}
		this.age += dt;
	}

	// Token: 0x06006F4D RID: 28493 RVA: 0x002F2C00 File Offset: 0x002F0E00
	private void PlayImpactSound(Vector3 pos)
	{
		if (this.impactSound == null)
		{
			this.impactSound = "Meteor_Large_Impact";
		}
		this.loopingSounds.StopSound(this.flyingSound);
		string sound = GlobalAssets.GetSound(this.impactSound, false);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = this.GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06006F4E RID: 28494 RVA: 0x000E8DD3 File Offset: 0x000E6FD3
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
		this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float)this.flyingSoundID);
	}

	// Token: 0x06006F4F RID: 28495 RVA: 0x002F2CA4 File Offset: 0x002F0EA4
	public void Explode()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		this.Explode(position, num, num, component.Element);
		this.hasExploded = true;
		global::Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0400532A RID: 21290
	[MyCmpGet]
	private PrimaryElement pe;

	// Token: 0x0400532B RID: 21291
	public Vector2 spawnVelocity = new Vector2(7f, 9f);

	// Token: 0x0400532C RID: 21292
	public Vector2 spawnAngle = new Vector2(30f, 150f);

	// Token: 0x0400532D RID: 21293
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x0400532E RID: 21294
	public int addDiseaseCount;

	// Token: 0x0400532F RID: 21295
	public byte diseaseIdx = byte.MaxValue;

	// Token: 0x04005330 RID: 21296
	public Vector2I explosionOreCount = new Vector2I(1, 1);

	// Token: 0x04005331 RID: 21297
	public Vector2 explosionSpeedRange = new Vector2(0f, 0f);

	// Token: 0x04005332 RID: 21298
	public string impactSound;

	// Token: 0x04005333 RID: 21299
	public string flyingSound;

	// Token: 0x04005334 RID: 21300
	public int flyingSoundID;

	// Token: 0x04005335 RID: 21301
	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	// Token: 0x04005336 RID: 21302
	public bool Targeted;

	// Token: 0x04005337 RID: 21303
	[Serialize]
	protected Vector3 offsetPosition;

	// Token: 0x04005338 RID: 21304
	[Serialize]
	protected Vector2 velocity;

	// Token: 0x04005339 RID: 21305
	private Vector3 previousPosition;

	// Token: 0x0400533A RID: 21306
	private bool hasExploded;

	// Token: 0x0400533B RID: 21307
	public string[] craterPrefabs;

	// Token: 0x0400533C RID: 21308
	public bool spawnWithOffset;

	// Token: 0x0400533D RID: 21309
	private float age;

	// Token: 0x0400533E RID: 21310
	public System.Action OnImpact;

	// Token: 0x0400533F RID: 21311
	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	// Token: 0x04005340 RID: 21312
	[MyCmpGet]
	private KBatchedAnimController anim;

	// Token: 0x04005341 RID: 21313
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04005342 RID: 21314
	public Tag typeID;

	// Token: 0x04005343 RID: 21315
	private LoopingSounds loopingSounds;

	// Token: 0x04005344 RID: 21316
	private List<GameObject> damagedEntities = new List<GameObject>();

	// Token: 0x04005345 RID: 21317
	private List<int> destroyedCells = new List<int>();

	// Token: 0x04005346 RID: 21318
	private const float MAX_DISTANCE_TEST = 6f;
}
